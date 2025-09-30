using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using BF2Statistics.Data;
using BF2Statistics.Models;

namespace BF2Statistics.Services
{
    public class GameSpyConnectionHandler : ConnectionHandler
    {
        private readonly ILogger<GameSpyConnectionHandler> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public GameSpyConnectionHandler(ILogger<GameSpyConnectionHandler> logger, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public override async Task OnConnectedAsync(ConnectionContext context)
        {
            var connectionId = context.ConnectionId;
            var remoteIpAddress = (context.RemoteEndPoint as System.Net.IPEndPoint)?.Address ?? System.Net.IPAddress.Loopback;
            
            _logger.LogInformation("GameSpy连接已建立: {ConnectionId} 来自 {RemoteEndPoint}", 
                connectionId, remoteIpAddress);

            try
            {
                // 读取TCP数据
                var input = context.Transport.Input;
                var result = await input.ReadAsync();
                var buffer = result.Buffer;
                
                if (buffer.Length > 0)
                {
                    var data = Encoding.UTF8.GetString(buffer);
                    input.AdvanceTo(buffer.End);
                    
                    _logger.LogInformation("连接 {ConnectionId} 接收到数据，长度: {Length}", connectionId, data.Length);
                    _logger.LogDebug("原始数据: {Data}", data.Length > 500 ? data.Substring(0, 500) + "..." : data);

                    // 检测是否为HTTP POST请求（来自Python miniclient）
                    if (data.StartsWith("POST ") && data.Contains("HTTP/1.1"))
                    {
                        _logger.LogInformation("检测到HTTP POST请求，处理snapshot数据");
                        await ProcessHttpPostRequest(context, data, remoteIpAddress);
                        return;
                    }
                    
                    _logger.LogWarning("接收到非HTTP POST请求，连接将被拒绝。数据: {Data}", data.Length > 100 ? data.Substring(0, 100) + "..." : data);
                }
                else
                {
                    _logger.LogWarning("连接 {ConnectionId} 接收到空数据", connectionId);
                }
                
                // 对所有非法的或空的请求发送一个标准的HTTP 400 Bad Request响应
                await SendHttpResponse(context, 200, "ERROR");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理连接时发生错误: {ConnectionId}", connectionId);
                // 确保在异常情况下也尝试发送一个标准的HTTP错误响应
                try
                {
                    await SendHttpResponse(context, 200, "ERROR");
                }
                catch (Exception sendEx)
                {
                    _logger.LogError(sendEx, "发送错误响应时失败: {ConnectionId}", connectionId);
                }
            }
            finally
            {
                _logger.LogInformation("连接已断开: {ConnectionId}", connectionId);
            }
        }

        // Removed ProcessReceivedData method - logic moved to HandleConnectionAsync

        private async Task ProcessGameSpyStatistics(string gameSpyData, System.Net.IPAddress serverIpAddress)
        {
            try
            {
                _logger.LogInformation("开始处理GameSpy统计数据");
                
                // 解析GameSpy数据 (\key\value\key\value格式)
                var parameters = new Dictionary<string, string>();
                
                // 更健壮的GameSpy数据解析器
                parameters = ParseGameSpyData(gameSpyData);
                
                _logger.LogInformation("解析出 {Count} 个有效GameSpy参数", parameters.Count);
                
                // 处理特定的GameSpy统计数据
                if (parameters.ContainsKey("mapname"))
                    _logger.LogInformation("地图: {MapName}", parameters["mapname"]);
                if (parameters.ContainsKey("gameport"))
                    _logger.LogInformation("游戏端口: {GamePort}", parameters["gameport"]);
                if (parameters.ContainsKey("queryport"))
                    _logger.LogInformation("查询端口: {QueryPort}", parameters["queryport"]);
                if (parameters.ContainsKey("mapstart"))
                    _logger.LogInformation("地图开始时间: {MapStart}", parameters["mapstart"]);
                if (parameters.ContainsKey("mapend"))
                    _logger.LogInformation("地图结束时间: {MapEnd}", parameters["mapend"]);
                if (parameters.ContainsKey("mapid"))
                    _logger.LogInformation("地图ID: {MapId}", parameters["mapid"]);
                
                // 保存到数据库
                using var scope = _serviceProvider.CreateScope();
                var gameSessionService = scope.ServiceProvider.GetRequiredService<GameSessionService>();
                var playerDataProcessor = scope.ServiceProvider.GetRequiredService<PlayerDataProcessor>();
                var statisticsService = scope.ServiceProvider.GetRequiredService<StatisticsService>();
                
                var serverIp = serverIpAddress.ToString();
                _logger.LogInformation("GameSpy数据来自服务器IP: {ServerIp}", serverIp);

                
                var gameSession = await gameSessionService.SaveGameSessionAsync(parameters, serverIp);
                if (gameSession != null)
                {
                    _logger.LogInformation("GameSpy数据已成功保存到数据库，会话ID: {SessionId}", gameSession.Id);
                    
                    // 处理玩家数据
                    await playerDataProcessor.ProcessSnapshotDataAsync(parameters);
                    
                    // 处理统计数据（奖励、地图等）
                    var gameData = string.Join("\\", parameters.SelectMany(kvp => new[] { kvp.Key, kvp.Value }));
                    await statisticsService.ProcessGameData(gameData, serverIpAddress);
                }
                else
                {
                    _logger.LogWarning("GameSpy数据保存到数据库失败");
                }
                
                _logger.LogInformation("GameSpy统计数据处理完成");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理GameSpy统计数据时发生错误: {Message}", ex.Message);
            }
        }

        /// <summary>
        /// 处理HTTP POST请求（来自Python miniclient）
        /// </summary>
        private async Task ProcessHttpPostRequest(ConnectionContext context, string httpData, System.Net.IPAddress serverIpAddress)
        {
            try
            {
                _logger.LogInformation("开始处理HTTP POST请求");
                
                // 解析HTTP请求，提取POST数据
                var lines = httpData.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                var contentLengthLine = lines.FirstOrDefault(l => l.StartsWith("Content-Length:", StringComparison.OrdinalIgnoreCase));
                
                if (contentLengthLine == null)
                {
                    _logger.LogWarning("HTTP请求中缺少Content-Length头");
                    await SendHttpResponse(context, 400, "Bad Request - Missing Content-Length");
                    return;
                }
                
                // 提取Content-Length值
                var contentLengthStr = contentLengthLine.Split(':')[1].Trim();
                if (!int.TryParse(contentLengthStr, out int contentLength))
                {
                    _logger.LogWarning("无效的Content-Length值: {ContentLength}", contentLengthStr);
                    await SendHttpResponse(context, 400, "Bad Request - Invalid Content-Length");
                    return;
                }
                
                // 找到HTTP头和body的分界线（空行）
                var emptyLineIndex = Array.FindIndex(lines, string.IsNullOrEmpty);
                if (emptyLineIndex == -1 || emptyLineIndex + 1 >= lines.Length)
                {
                    _logger.LogWarning("HTTP请求格式无效，找不到请求体");
                    await SendHttpResponse(context, 400, "Bad Request - Invalid HTTP format");
                    return;
                }
                
                // 提取POST数据（snapshot数据）
                var postData = string.Join("\n", lines.Skip(emptyLineIndex + 1));
                
                _logger.LogInformation("提取到POST数据，长度: {Length}", postData.Length);
                _logger.LogDebug("POST数据: {Data}", postData.Length > 500 ? postData.Substring(0, 500) + "..." : postData);
                
                // 处理snapshot数据
                await ProcessGameSpyStatistics(postData, serverIpAddress);
                
                // 发送HTTP 200 OK响应，响应体以'O'开头表示成功
                await SendHttpResponse(context, 200, "OK");
                
                _logger.LogInformation("HTTP POST请求处理完成");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理HTTP POST请求时发生错误");
                await SendHttpResponse(context, 200, "Error");
            }
        }

        /// <summary>
        /// 解析GameSpy数据格式 (\key\value\key\value...)
        /// </summary>
        private Dictionary<string, string> ParseGameSpyData(string gameSpyData)
        {
            var parameters = new Dictionary<string, string>();
            
            if (string.IsNullOrEmpty(gameSpyData))
            {
                _logger.LogWarning("GameSpy数据为空");
                return parameters;
            }
            
            try
            {
                // 记录原始数据用于调试
                _logger.LogDebug("原始GameSpy数据: {Data}", gameSpyData.Length > 200 ? gameSpyData.Substring(0, 200) + "..." : gameSpyData);
                
                // GameSpy数据格式: \key\value\key\value...
                // 直接使用Split方法，然后正确配对键值
                var cleanData = gameSpyData.Trim();
                
                // 移除开头和结尾的反斜杠
                if (cleanData.StartsWith("\\"))
                    cleanData = cleanData.Substring(1);
                if (cleanData.EndsWith("\\"))
                    cleanData = cleanData.Substring(0, cleanData.Length - 1);
                
                // 按反斜杠分割
                var tokens = cleanData.Split('\\');
                
                _logger.LogDebug("分割后得到 {Count} 个token", tokens.Length);
                
                // GameSpy数据格式分析：\default server\gameport\16567\queryport\29990\mapname\dragon_valley\...
                // default server 是服务器名称，从gameport开始才是键值对
                // 正确解析：gameport=16567, queryport=29990, mapname=dragon_valley
                
                // 从索引0开始处理键值对（跳过服务器名称后）
                // 但需要找到第一个真正的键（通常是gameport、queryport等）
                int startIndex = 0;
                
                // 如果第一个token看起来像服务器名称而不是键，则跳过
                if (tokens.Length > 0 && (tokens[0].Contains(" ") || tokens[0].ToLower().Contains("server")))
                {
                    startIndex = 1;
                }
                
                // 按键值对处理：从startIndex开始，token[i]=key, token[i+1]=value
                for (int i = startIndex; i < tokens.Length - 1; i += 2)
                {
                    var key = tokens[i].Trim();
                    var value = tokens[i + 1].Trim();
                    
                    if (!string.IsNullOrEmpty(key))
                    {
                        parameters[key] = value;
                        _logger.LogDebug("解析到键值对: '{Key}' = '{Value}'", key, value);
                    }
                }
                
                // 如果有奇数个token（从startIndex开始计算），最后一个作为键，值为空
                int remainingTokens = tokens.Length - startIndex;
                if (remainingTokens % 2 == 1)
                {
                    var lastKey = tokens[tokens.Length - 1].Trim();
                    if (!string.IsNullOrEmpty(lastKey))
                    {
                        parameters[lastKey] = "";
                        _logger.LogDebug("解析到最后的键（无值）: '{Key}' = ''", lastKey);
                    }
                }
                
                _logger.LogInformation("成功解析GameSpy数据，共 {Count} 个参数", parameters.Count);
                
                // 记录所有参数用于调试
                foreach (var kvp in parameters)
                {
                    _logger.LogDebug("参数: '{Key}' = '{Value}'", kvp.Key, kvp.Value);
                }
                
                return parameters;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "解析GameSpy数据时发生错误");
                return parameters;
            }
        }

        /// <summary>
        /// 发送HTTP响应
        /// </summary>
        private async Task SendHttpResponse(ConnectionContext context, int statusCode, string statusText)
        {
            try
            {
                var responseBody = statusText == "OK" ? "OK" : "ERROR";
                var response = $"HTTP/1.1 {statusCode} {statusText}\r\n" +
                              $"Content-Type: text/plain\r\n" +
                              $"Content-Length: {responseBody.Length}\r\n" +
                              $"Connection: close\r\n" +
                              $"\r\n" +
                              $"{responseBody}";
                
                var responseBytes = Encoding.UTF8.GetBytes(response);
                await context.Transport.Output.WriteAsync(responseBytes);
                
                _logger.LogDebug("发送HTTP响应: {StatusCode} {StatusText}, Body: {Body}", statusCode, statusText, responseBody);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发送HTTP响应时发生错误");
            }
        }

         // Removed all ASPX processing methods - GameSpyConnectionHandler now only handles GameSpy protocol data
    }
}