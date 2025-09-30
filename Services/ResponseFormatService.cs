using System.Text;

namespace BF2Statistics.Services
{
    /// <summary>
    /// 提供标准化的BF2统计响应格式，确保与原始BF2客户端和工具的兼容性
    /// </summary>
    public static class ResponseFormatService
    {
        /// <summary>
        /// 格式化标准的BF2统计响应
        /// </summary>
        /// <param name="isValid">响应是否有效，true使用"O"开头，false使用"E"开头</param>
        /// <param name="transpose">是否转置输出格式</param>
        /// <returns>格式化的响应字符串</returns>
        public static string FormatResponse(bool isValid = true, bool transpose = false)
        {
            return isValid ? "O" : "E";
        }

        /// <summary>
        /// 格式化带有数据的标准响应
        /// </summary>
        /// <param name="headers">表头数组</param>
        /// <param name="data">数据数组</param>
        /// <param name="isValid">响应是否有效</param>
        /// <param name="transpose">是否转置输出格式</param>
        /// <returns>格式化的响应字符串</returns>
        public static string FormatDataResponse(string[] headers, string[] data, bool isValid = true, bool transpose = false)
        {
            var response = new StringBuilder();
            response.Append(isValid ? "O" : "E");

            if (transpose)
            {
                // 转置格式：每行一个字段
                for (int i = 0; i < headers.Length && i < data.Length; i++)
                {
                    response.AppendLine();
                    response.Append($"D\t{headers[i]}\t{data[i]}");
                }
            }
            else
            {
                // 标准格式：表头和数据分别在不同行
                response.AppendLine();
                response.Append("H");
                foreach (var header in headers)
                {
                    response.Append($"\t{header}");
                }

                response.AppendLine();
                response.Append("D");
                foreach (var item in data)
                {
                    response.Append($"\t{item}");
                }
            }

            return response.ToString();
        }

        /// <summary>
        /// 格式化多行数据响应
        /// </summary>
        /// <param name="headers">表头数组</param>
        /// <param name="dataRows">多行数据</param>
        /// <param name="isValid">响应是否有效</param>
        /// <param name="transpose">是否转置输出格式</param>
        /// <returns>格式化的响应字符串</returns>
        public static string FormatMultiDataResponse(string[] headers, List<string[]> dataRows, bool isValid = true, bool transpose = false)
        {
            var response = new StringBuilder();
            response.Append(isValid ? "O" : "E");

            if (transpose)
            {
                // 转置格式：每个字段作为一列
                for (int col = 0; col < headers.Length; col++)
                {
                    response.AppendLine();
                    response.Append($"H\t{headers[col]}");
                    
                    foreach (var row in dataRows)
                    {
                        if (col < row.Length)
                        {
                            response.Append($"\t{row[col]}");
                        }
                    }
                }
            }
            else
            {
                // 标准格式
                response.AppendLine();
                response.Append("H");
                foreach (var header in headers)
                {
                    response.Append($"\t{header}");
                }

                foreach (var row in dataRows)
                {
                    response.AppendLine();
                    response.Append("D");
                    foreach (var item in row)
                    {
                        response.Append($"\t{item}");
                    }
                }
            }

            return response.ToString();
        }

        /// <summary>
        /// 格式化错误响应
        /// </summary>
        /// <param name="errorMessage">错误消息</param>
        /// <returns>格式化的错误响应</returns>
        public static string FormatError(string errorMessage)
        {
            var response = new StringBuilder();
            response.AppendLine("E");
            response.AppendLine("H\terr");
            response.Append($"D\t{errorMessage}");
            return response.ToString();
        }

        /// <summary>
        /// 格式化无效语法响应
        /// </summary>
        /// <returns>格式化的无效语法响应</returns>
        public static string InvalidSyntax()
        {
            return FormatError("Invalid Syntax!");
        }

        /// <summary>
        /// 格式化时间戳响应（用于asof字段）
        /// </summary>
        /// <param name="timestamp">Unix时间戳</param>
        /// <param name="message">附加消息</param>
        /// <param name="isValid">响应是否有效</param>
        /// <returns>格式化的时间戳响应</returns>
        public static string FormatTimestampResponse(long timestamp, string message = "", bool isValid = true)
        {
            var response = new StringBuilder();
            response.Append(isValid ? "O" : "E");
            response.AppendLine();
            response.Append("H\tasof");
            
            if (!string.IsNullOrEmpty(message))
            {
                response.Append("\terr");
            }
            
            response.AppendLine();
            response.Append($"D\t{timestamp}");
            
            if (!string.IsNullOrEmpty(message))
            {
                response.Append($"\t{message}");
            }

            return response.ToString();
        }

        /// <summary>
        /// 检查是否应该使用转置格式
        /// </summary>
        /// <param name="queryString">查询字符串字典</param>
        /// <returns>是否使用转置格式</returns>
        public static bool ShouldTranspose(IDictionary<string, Microsoft.Extensions.Primitives.StringValues> queryString)
        {
            return queryString.ContainsKey("transpose") && queryString["transpose"] == "1";
        }

        /// <summary>
        /// 创建标准BF2统计响应格式（保持向后兼容）
        /// </summary>
        /// <returns>格式化的响应字符串</returns>
        public static string FormatResponse(string content)
        {
            var response = new StringBuilder();
            response.AppendLine("O");
            response.AppendLine("H\tasof");
            response.AppendLine($"D\t{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}");
            response.Append(content);
            return response.ToString();
        }
    }
}