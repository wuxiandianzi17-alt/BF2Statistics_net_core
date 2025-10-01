using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BF2Statistics.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "army",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    time0 = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    time1 = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    time2 = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    time3 = table.Column<int>(type: "INTEGER", nullable: false),
                    time4 = table.Column<int>(type: "INTEGER", nullable: false),
                    time5 = table.Column<int>(type: "INTEGER", nullable: false),
                    time6 = table.Column<int>(type: "INTEGER", nullable: false),
                    time7 = table.Column<int>(type: "INTEGER", nullable: false),
                    time8 = table.Column<int>(type: "INTEGER", nullable: false),
                    win0 = table.Column<int>(type: "INTEGER", nullable: false),
                    win1 = table.Column<int>(type: "INTEGER", nullable: false),
                    win2 = table.Column<int>(type: "INTEGER", nullable: false),
                    win3 = table.Column<int>(type: "INTEGER", nullable: false),
                    win4 = table.Column<int>(type: "INTEGER", nullable: false),
                    win5 = table.Column<int>(type: "INTEGER", nullable: false),
                    win6 = table.Column<int>(type: "INTEGER", nullable: false),
                    win7 = table.Column<int>(type: "INTEGER", nullable: false),
                    win8 = table.Column<int>(type: "INTEGER", nullable: false),
                    loss0 = table.Column<int>(type: "INTEGER", nullable: false),
                    loss1 = table.Column<int>(type: "INTEGER", nullable: false),
                    loss2 = table.Column<int>(type: "INTEGER", nullable: false),
                    loss3 = table.Column<int>(type: "INTEGER", nullable: false),
                    loss4 = table.Column<int>(type: "INTEGER", nullable: false),
                    loss5 = table.Column<int>(type: "INTEGER", nullable: false),
                    loss6 = table.Column<int>(type: "INTEGER", nullable: false),
                    loss7 = table.Column<int>(type: "INTEGER", nullable: false),
                    loss8 = table.Column<int>(type: "INTEGER", nullable: false),
                    score0 = table.Column<int>(type: "INTEGER", nullable: false),
                    score1 = table.Column<int>(type: "INTEGER", nullable: false),
                    score2 = table.Column<int>(type: "INTEGER", nullable: false),
                    score3 = table.Column<int>(type: "INTEGER", nullable: false),
                    score4 = table.Column<int>(type: "INTEGER", nullable: false),
                    score5 = table.Column<int>(type: "INTEGER", nullable: false),
                    score6 = table.Column<int>(type: "INTEGER", nullable: false),
                    score7 = table.Column<int>(type: "INTEGER", nullable: false),
                    score8 = table.Column<int>(type: "INTEGER", nullable: false),
                    best0 = table.Column<int>(type: "INTEGER", nullable: false),
                    best1 = table.Column<int>(type: "INTEGER", nullable: false),
                    best2 = table.Column<int>(type: "INTEGER", nullable: false),
                    best3 = table.Column<int>(type: "INTEGER", nullable: false),
                    best4 = table.Column<int>(type: "INTEGER", nullable: false),
                    best5 = table.Column<int>(type: "INTEGER", nullable: false),
                    best6 = table.Column<int>(type: "INTEGER", nullable: false),
                    best7 = table.Column<int>(type: "INTEGER", nullable: false),
                    best8 = table.Column<int>(type: "INTEGER", nullable: false),
                    worst0 = table.Column<int>(type: "INTEGER", nullable: false),
                    worst1 = table.Column<int>(type: "INTEGER", nullable: false),
                    worst2 = table.Column<int>(type: "INTEGER", nullable: false),
                    worst3 = table.Column<int>(type: "INTEGER", nullable: false),
                    worst4 = table.Column<int>(type: "INTEGER", nullable: false),
                    worst5 = table.Column<int>(type: "INTEGER", nullable: false),
                    worst6 = table.Column<int>(type: "INTEGER", nullable: false),
                    worst7 = table.Column<int>(type: "INTEGER", nullable: false),
                    worst8 = table.Column<int>(type: "INTEGER", nullable: false),
                    wins = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    losses = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    score = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    bestscore = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    worstscore = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    cmdtime = table.Column<int>(type: "INTEGER", nullable: false),
                    sqltime = table.Column<int>(type: "INTEGER", nullable: false),
                    sqmtime = table.Column<int>(type: "INTEGER", nullable: false),
                    lwtime = table.Column<int>(type: "INTEGER", nullable: false),
                    kills = table.Column<int>(type: "INTEGER", nullable: false),
                    deaths = table.Column<int>(type: "INTEGER", nullable: false),
                    captures = table.Column<int>(type: "INTEGER", nullable: false),
                    neutralizes = table.Column<int>(type: "INTEGER", nullable: false),
                    captureassists = table.Column<int>(type: "INTEGER", nullable: false),
                    neutralizeassists = table.Column<int>(type: "INTEGER", nullable: false),
                    defends = table.Column<int>(type: "INTEGER", nullable: false),
                    damageassists = table.Column<int>(type: "INTEGER", nullable: false),
                    heals = table.Column<int>(type: "INTEGER", nullable: false),
                    revives = table.Column<int>(type: "INTEGER", nullable: false),
                    ammos = table.Column<int>(type: "INTEGER", nullable: false),
                    repairs = table.Column<int>(type: "INTEGER", nullable: false),
                    targetassists = table.Column<int>(type: "INTEGER", nullable: false),
                    driverspecials = table.Column<int>(type: "INTEGER", nullable: false),
                    driverassists = table.Column<int>(type: "INTEGER", nullable: false),
                    passengerassists = table.Column<int>(type: "INTEGER", nullable: false),
                    teamkills = table.Column<int>(type: "INTEGER", nullable: false),
                    teamdamage = table.Column<int>(type: "INTEGER", nullable: false),
                    teamvehicledamage = table.Column<int>(type: "INTEGER", nullable: false),
                    suicides = table.Column<int>(type: "INTEGER", nullable: false),
                    killstreak = table.Column<int>(type: "INTEGER", nullable: false),
                    deathstreak = table.Column<int>(type: "INTEGER", nullable: false),
                    rndscore = table.Column<int>(type: "INTEGER", nullable: false),
                    bested = table.Column<int>(type: "INTEGER", nullable: false),
                    worsted = table.Column<int>(type: "INTEGER", nullable: false),
                    brnd0 = table.Column<int>(type: "INTEGER", nullable: false),
                    brnd1 = table.Column<int>(type: "INTEGER", nullable: false),
                    brnd2 = table.Column<int>(type: "INTEGER", nullable: false),
                    brnd3 = table.Column<int>(type: "INTEGER", nullable: false),
                    brnd4 = table.Column<int>(type: "INTEGER", nullable: false),
                    brnd5 = table.Column<int>(type: "INTEGER", nullable: false),
                    brnd6 = table.Column<int>(type: "INTEGER", nullable: false),
                    brnd7 = table.Column<int>(type: "INTEGER", nullable: false),
                    brnd8 = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_army", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "awards",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    pid = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    awardid = table.Column<int>(type: "INTEGER", nullable: false),
                    level = table.Column<byte>(type: "INTEGER", nullable: false, defaultValue: (byte)0),
                    when = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    first = table.Column<byte>(type: "INTEGER", nullable: false, defaultValue: (byte)0),
                    earned = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_awards", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "game_sessions",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    server_ip = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false, defaultValue: ""),
                    game_port = table.Column<int>(type: "INTEGER", nullable: false),
                    query_port = table.Column<int>(type: "INTEGER", nullable: false),
                    map_name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, defaultValue: ""),
                    map_id = table.Column<int>(type: "INTEGER", nullable: false),
                    map_start = table.Column<double>(type: "REAL", nullable: false),
                    map_end = table.Column<double>(type: "REAL", nullable: false),
                    winner = table.Column<int>(type: "INTEGER", nullable: false),
                    game_mode = table.Column<int>(type: "INTEGER", nullable: false),
                    mod_id = table.Column<int>(type: "INTEGER", nullable: false),
                    version = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false, defaultValue: "bf2"),
                    pc = table.Column<int>(type: "INTEGER", nullable: false),
                    ra1 = table.Column<int>(type: "INTEGER", nullable: false),
                    rs1 = table.Column<int>(type: "INTEGER", nullable: false),
                    ra2 = table.Column<int>(type: "INTEGER", nullable: false),
                    rs2 = table.Column<int>(type: "INTEGER", nullable: false),
                    rst2 = table.Column<int>(type: "INTEGER", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValue: new DateTime(2025, 10, 1, 8, 5, 36, 594, DateTimeKind.Utc).AddTicks(2928)),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValue: new DateTime(2025, 10, 1, 8, 5, 36, 594, DateTimeKind.Utc).AddTicks(3180))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_game_sessions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ip2nation",
                columns: table => new
                {
                    ip = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    country = table.Column<string>(type: "TEXT", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ip2nation", x => x.ip);
                });

            migrationBuilder.CreateTable(
                name: "kills",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    victim = table.Column<int>(type: "INTEGER", nullable: false),
                    attacker = table.Column<int>(type: "INTEGER", nullable: false),
                    count = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kills", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "kits",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    time0 = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    time1 = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    time2 = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    time3 = table.Column<int>(type: "INTEGER", nullable: false),
                    time4 = table.Column<int>(type: "INTEGER", nullable: false),
                    time5 = table.Column<int>(type: "INTEGER", nullable: false),
                    time6 = table.Column<int>(type: "INTEGER", nullable: false),
                    kills0 = table.Column<int>(type: "INTEGER", nullable: false),
                    kills1 = table.Column<int>(type: "INTEGER", nullable: false),
                    kills2 = table.Column<int>(type: "INTEGER", nullable: false),
                    kills3 = table.Column<int>(type: "INTEGER", nullable: false),
                    kills4 = table.Column<int>(type: "INTEGER", nullable: false),
                    kills5 = table.Column<int>(type: "INTEGER", nullable: false),
                    kills6 = table.Column<int>(type: "INTEGER", nullable: false),
                    deaths0 = table.Column<int>(type: "INTEGER", nullable: false),
                    deaths1 = table.Column<int>(type: "INTEGER", nullable: false),
                    deaths2 = table.Column<int>(type: "INTEGER", nullable: false),
                    deaths3 = table.Column<int>(type: "INTEGER", nullable: false),
                    deaths4 = table.Column<int>(type: "INTEGER", nullable: false),
                    deaths5 = table.Column<int>(type: "INTEGER", nullable: false),
                    deaths6 = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kits", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mapinfo",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    score = table.Column<int>(type: "INTEGER", nullable: false),
                    time = table.Column<int>(type: "INTEGER", nullable: false),
                    kills = table.Column<int>(type: "INTEGER", nullable: false),
                    deaths = table.Column<int>(type: "INTEGER", nullable: false),
                    captures = table.Column<int>(type: "INTEGER", nullable: false),
                    assists = table.Column<int>(type: "INTEGER", nullable: false),
                    rounds = table.Column<int>(type: "INTEGER", nullable: false),
                    times = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mapinfo", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "maps",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false),
                    mapid = table.Column<int>(type: "INTEGER", nullable: false),
                    time = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    win = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    loss = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    best = table.Column<int>(type: "INTEGER", nullable: false),
                    worst = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_maps", x => new { x.id, x.mapid });
                });

            migrationBuilder.CreateTable(
                name: "player",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 45, nullable: false, defaultValue: ""),
                    country = table.Column<string>(type: "TEXT", maxLength: 2, nullable: false, defaultValue: "xx"),
                    time = table.Column<int>(type: "INTEGER", nullable: false),
                    rounds = table.Column<int>(type: "INTEGER", nullable: false),
                    ip = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false, defaultValue: ""),
                    score = table.Column<int>(type: "INTEGER", nullable: false),
                    cmdscore = table.Column<int>(type: "INTEGER", nullable: false),
                    skillscore = table.Column<int>(type: "INTEGER", nullable: false),
                    teamscore = table.Column<int>(type: "INTEGER", nullable: false),
                    kills = table.Column<int>(type: "INTEGER", nullable: false),
                    deaths = table.Column<int>(type: "INTEGER", nullable: false),
                    captures = table.Column<int>(type: "INTEGER", nullable: false),
                    neutralizes = table.Column<int>(type: "INTEGER", nullable: false),
                    captureassists = table.Column<int>(type: "INTEGER", nullable: false),
                    neutralizeassists = table.Column<int>(type: "INTEGER", nullable: false),
                    defends = table.Column<int>(type: "INTEGER", nullable: false),
                    damageassists = table.Column<int>(type: "INTEGER", nullable: false),
                    heals = table.Column<int>(type: "INTEGER", nullable: false),
                    revives = table.Column<int>(type: "INTEGER", nullable: false),
                    ammos = table.Column<int>(type: "INTEGER", nullable: false),
                    repairs = table.Column<int>(type: "INTEGER", nullable: false),
                    targetassists = table.Column<int>(type: "INTEGER", nullable: false),
                    driverspecials = table.Column<int>(type: "INTEGER", nullable: false),
                    driverassists = table.Column<int>(type: "INTEGER", nullable: false),
                    passengerassists = table.Column<int>(type: "INTEGER", nullable: false),
                    teamkills = table.Column<int>(type: "INTEGER", nullable: false),
                    teamdamage = table.Column<int>(type: "INTEGER", nullable: false),
                    teamvehicledamage = table.Column<int>(type: "INTEGER", nullable: false),
                    suicides = table.Column<int>(type: "INTEGER", nullable: false),
                    killstreak = table.Column<int>(type: "INTEGER", nullable: false),
                    deathstreak = table.Column<int>(type: "INTEGER", nullable: false),
                    rank = table.Column<byte>(type: "INTEGER", nullable: false),
                    banned = table.Column<int>(type: "INTEGER", nullable: false),
                    kicked = table.Column<int>(type: "INTEGER", nullable: false),
                    cmdtime = table.Column<int>(type: "INTEGER", nullable: false),
                    sqltime = table.Column<int>(type: "INTEGER", nullable: false),
                    sqmtime = table.Column<int>(type: "INTEGER", nullable: false),
                    lwtime = table.Column<int>(type: "INTEGER", nullable: false),
                    wins = table.Column<int>(type: "INTEGER", nullable: false),
                    losses = table.Column<int>(type: "INTEGER", nullable: false),
                    availunlocks = table.Column<byte>(type: "INTEGER", nullable: false),
                    usedunlocks = table.Column<byte>(type: "INTEGER", nullable: false),
                    joined = table.Column<int>(type: "INTEGER", nullable: false),
                    rndscore = table.Column<int>(type: "INTEGER", nullable: false),
                    lastonline = table.Column<int>(type: "INTEGER", nullable: false),
                    chng = table.Column<byte>(type: "INTEGER", nullable: false),
                    decr = table.Column<byte>(type: "INTEGER", nullable: false),
                    mode0 = table.Column<int>(type: "INTEGER", nullable: false),
                    mode1 = table.Column<int>(type: "INTEGER", nullable: false),
                    mode2 = table.Column<int>(type: "INTEGER", nullable: false),
                    permban = table.Column<byte>(type: "INTEGER", nullable: false),
                    clantag = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "servers",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ip = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false, defaultValue: ""),
                    prefix = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false, defaultValue: ""),
                    name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    port = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    queryport = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    rcon_port = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 4711),
                    rcon_password = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    lastupdate = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vehicles",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    time0 = table.Column<int>(type: "INTEGER", nullable: false),
                    time1 = table.Column<int>(type: "INTEGER", nullable: false),
                    time2 = table.Column<int>(type: "INTEGER", nullable: false),
                    time3 = table.Column<int>(type: "INTEGER", nullable: false),
                    time4 = table.Column<int>(type: "INTEGER", nullable: false),
                    time5 = table.Column<int>(type: "INTEGER", nullable: false),
                    time6 = table.Column<int>(type: "INTEGER", nullable: false),
                    timepara = table.Column<int>(type: "INTEGER", nullable: false),
                    kills0 = table.Column<int>(type: "INTEGER", nullable: false),
                    kills1 = table.Column<int>(type: "INTEGER", nullable: false),
                    kills2 = table.Column<int>(type: "INTEGER", nullable: false),
                    kills3 = table.Column<int>(type: "INTEGER", nullable: false),
                    kills4 = table.Column<int>(type: "INTEGER", nullable: false),
                    kills5 = table.Column<int>(type: "INTEGER", nullable: false),
                    kills6 = table.Column<int>(type: "INTEGER", nullable: false),
                    deaths0 = table.Column<int>(type: "INTEGER", nullable: false),
                    deaths1 = table.Column<int>(type: "INTEGER", nullable: false),
                    deaths2 = table.Column<int>(type: "INTEGER", nullable: false),
                    deaths3 = table.Column<int>(type: "INTEGER", nullable: false),
                    deaths4 = table.Column<int>(type: "INTEGER", nullable: false),
                    deaths5 = table.Column<int>(type: "INTEGER", nullable: false),
                    deaths6 = table.Column<int>(type: "INTEGER", nullable: false),
                    rk0 = table.Column<int>(type: "INTEGER", nullable: false),
                    rk1 = table.Column<int>(type: "INTEGER", nullable: false),
                    rk2 = table.Column<int>(type: "INTEGER", nullable: false),
                    rk3 = table.Column<int>(type: "INTEGER", nullable: false),
                    rk4 = table.Column<int>(type: "INTEGER", nullable: false),
                    rk5 = table.Column<int>(type: "INTEGER", nullable: false),
                    rk6 = table.Column<int>(type: "INTEGER", nullable: false),
                    time = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    kills = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    deaths = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    roadkills = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    roadkills0 = table.Column<int>(type: "INTEGER", nullable: false),
                    roadkills1 = table.Column<int>(type: "INTEGER", nullable: false),
                    roadkills2 = table.Column<int>(type: "INTEGER", nullable: false),
                    roadkills3 = table.Column<int>(type: "INTEGER", nullable: false),
                    roadkills4 = table.Column<int>(type: "INTEGER", nullable: false),
                    roadkills5 = table.Column<int>(type: "INTEGER", nullable: false),
                    roadkills6 = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "weapons",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    time0 = table.Column<int>(type: "INTEGER", nullable: false),
                    time1 = table.Column<int>(type: "INTEGER", nullable: false),
                    time2 = table.Column<int>(type: "INTEGER", nullable: false),
                    time3 = table.Column<int>(type: "INTEGER", nullable: false),
                    time4 = table.Column<int>(type: "INTEGER", nullable: false),
                    time5 = table.Column<int>(type: "INTEGER", nullable: false),
                    time6 = table.Column<int>(type: "INTEGER", nullable: false),
                    time7 = table.Column<int>(type: "INTEGER", nullable: false),
                    time8 = table.Column<int>(type: "INTEGER", nullable: false),
                    knifetime = table.Column<int>(type: "INTEGER", nullable: false),
                    c4time = table.Column<int>(type: "INTEGER", nullable: false),
                    handgrenadetime = table.Column<int>(type: "INTEGER", nullable: false),
                    claymoretime = table.Column<int>(type: "INTEGER", nullable: false),
                    shockpadtime = table.Column<int>(type: "INTEGER", nullable: false),
                    atminetime = table.Column<int>(type: "INTEGER", nullable: false),
                    tacticaltime = table.Column<int>(type: "INTEGER", nullable: false),
                    grapplinghooktime = table.Column<int>(type: "INTEGER", nullable: false),
                    ziplinetime = table.Column<int>(type: "INTEGER", nullable: false),
                    kills0 = table.Column<int>(type: "INTEGER", nullable: false),
                    kills1 = table.Column<int>(type: "INTEGER", nullable: false),
                    kills2 = table.Column<int>(type: "INTEGER", nullable: false),
                    kills3 = table.Column<int>(type: "INTEGER", nullable: false),
                    kills4 = table.Column<int>(type: "INTEGER", nullable: false),
                    kills5 = table.Column<int>(type: "INTEGER", nullable: false),
                    kills6 = table.Column<int>(type: "INTEGER", nullable: false),
                    kills7 = table.Column<int>(type: "INTEGER", nullable: false),
                    kills8 = table.Column<int>(type: "INTEGER", nullable: false),
                    knifekills = table.Column<int>(type: "INTEGER", nullable: false),
                    c4kills = table.Column<int>(type: "INTEGER", nullable: false),
                    handgrenadekills = table.Column<int>(type: "INTEGER", nullable: false),
                    claymorekills = table.Column<int>(type: "INTEGER", nullable: false),
                    shockpadkills = table.Column<int>(type: "INTEGER", nullable: false),
                    atminekills = table.Column<int>(type: "INTEGER", nullable: false),
                    tacticalkills = table.Column<int>(type: "INTEGER", nullable: false),
                    grapplinghookkills = table.Column<int>(type: "INTEGER", nullable: false),
                    ziplinekills = table.Column<int>(type: "INTEGER", nullable: false),
                    deaths0 = table.Column<int>(type: "INTEGER", nullable: false),
                    deaths1 = table.Column<int>(type: "INTEGER", nullable: false),
                    deaths2 = table.Column<int>(type: "INTEGER", nullable: false),
                    deaths3 = table.Column<int>(type: "INTEGER", nullable: false),
                    deaths4 = table.Column<int>(type: "INTEGER", nullable: false),
                    deaths5 = table.Column<int>(type: "INTEGER", nullable: false),
                    deaths6 = table.Column<int>(type: "INTEGER", nullable: false),
                    deaths7 = table.Column<int>(type: "INTEGER", nullable: false),
                    deaths8 = table.Column<int>(type: "INTEGER", nullable: false),
                    knifedeaths = table.Column<int>(type: "INTEGER", nullable: false),
                    c4deaths = table.Column<int>(type: "INTEGER", nullable: false),
                    handgrenadedeaths = table.Column<int>(type: "INTEGER", nullable: false),
                    claymoredeaths = table.Column<int>(type: "INTEGER", nullable: false),
                    shockpaddeaths = table.Column<int>(type: "INTEGER", nullable: false),
                    atminedeaths = table.Column<int>(type: "INTEGER", nullable: false),
                    ziplinedeaths = table.Column<int>(type: "INTEGER", nullable: false),
                    grapplinghookdeaths = table.Column<int>(type: "INTEGER", nullable: false),
                    tacticaldeaths = table.Column<int>(type: "INTEGER", nullable: false),
                    tacticaldeployed = table.Column<int>(type: "INTEGER", nullable: false),
                    grapplinghookdeployed = table.Column<int>(type: "INTEGER", nullable: false),
                    ziplinedeployed = table.Column<int>(type: "INTEGER", nullable: false),
                    fired0 = table.Column<int>(type: "INTEGER", nullable: false),
                    fired1 = table.Column<int>(type: "INTEGER", nullable: false),
                    fired2 = table.Column<int>(type: "INTEGER", nullable: false),
                    fired3 = table.Column<int>(type: "INTEGER", nullable: false),
                    fired4 = table.Column<int>(type: "INTEGER", nullable: false),
                    fired5 = table.Column<int>(type: "INTEGER", nullable: false),
                    fired6 = table.Column<int>(type: "INTEGER", nullable: false),
                    fired7 = table.Column<int>(type: "INTEGER", nullable: false),
                    fired8 = table.Column<int>(type: "INTEGER", nullable: false),
                    knifefired = table.Column<int>(type: "INTEGER", nullable: false),
                    c4fired = table.Column<int>(type: "INTEGER", nullable: false),
                    claymorefired = table.Column<int>(type: "INTEGER", nullable: false),
                    handgrenadefired = table.Column<int>(type: "INTEGER", nullable: false),
                    shockpadfired = table.Column<int>(type: "INTEGER", nullable: false),
                    atminefired = table.Column<int>(type: "INTEGER", nullable: false),
                    tacticalfired = table.Column<int>(type: "INTEGER", nullable: false),
                    grapplinghookfired = table.Column<int>(type: "INTEGER", nullable: false),
                    ziplinefired = table.Column<int>(type: "INTEGER", nullable: false),
                    hit0 = table.Column<int>(type: "INTEGER", nullable: false),
                    hit1 = table.Column<int>(type: "INTEGER", nullable: false),
                    hit2 = table.Column<int>(type: "INTEGER", nullable: false),
                    hit3 = table.Column<int>(type: "INTEGER", nullable: false),
                    hit4 = table.Column<int>(type: "INTEGER", nullable: false),
                    hit5 = table.Column<int>(type: "INTEGER", nullable: false),
                    hit6 = table.Column<int>(type: "INTEGER", nullable: false),
                    hit7 = table.Column<int>(type: "INTEGER", nullable: false),
                    hit8 = table.Column<int>(type: "INTEGER", nullable: false),
                    knifehit = table.Column<int>(type: "INTEGER", nullable: false),
                    c4hit = table.Column<int>(type: "INTEGER", nullable: false),
                    claymorehit = table.Column<int>(type: "INTEGER", nullable: false),
                    handgrenadehit = table.Column<int>(type: "INTEGER", nullable: false),
                    shockpadhit = table.Column<int>(type: "INTEGER", nullable: false),
                    atminehit = table.Column<int>(type: "INTEGER", nullable: false),
                    tacticalhit = table.Column<int>(type: "INTEGER", nullable: false),
                    grapplinghookhit = table.Column<int>(type: "INTEGER", nullable: false),
                    ziplinehit = table.Column<int>(type: "INTEGER", nullable: false),
                    Kills = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    Deaths = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    Fired = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    Hit = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weapons", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "unlocks",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false),
                    kit = table.Column<int>(type: "INTEGER", nullable: false),
                    state = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_unlocks", x => new { x.id, x.kit });
                    table.ForeignKey(
                        name: "FK_unlocks_player_id",
                        column: x => x.id,
                        principalTable: "player",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_game_sessions_created_at",
                table: "game_sessions",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_game_sessions_map_name",
                table: "game_sessions",
                column: "map_name");

            migrationBuilder.CreateIndex(
                name: "IX_game_sessions_map_start",
                table: "game_sessions",
                column: "map_start");

            migrationBuilder.CreateIndex(
                name: "IX_game_sessions_server_ip",
                table: "game_sessions",
                column: "server_ip");

            migrationBuilder.CreateIndex(
                name: "IX_servers_ip_port",
                table: "servers",
                columns: new[] { "ip", "port" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "army");

            migrationBuilder.DropTable(
                name: "awards");

            migrationBuilder.DropTable(
                name: "game_sessions");

            migrationBuilder.DropTable(
                name: "ip2nation");

            migrationBuilder.DropTable(
                name: "kills");

            migrationBuilder.DropTable(
                name: "kits");

            migrationBuilder.DropTable(
                name: "mapinfo");

            migrationBuilder.DropTable(
                name: "maps");

            migrationBuilder.DropTable(
                name: "servers");

            migrationBuilder.DropTable(
                name: "unlocks");

            migrationBuilder.DropTable(
                name: "vehicles");

            migrationBuilder.DropTable(
                name: "weapons");

            migrationBuilder.DropTable(
                name: "player");
        }
    }
}
