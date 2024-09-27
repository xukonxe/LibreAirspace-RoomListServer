using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace 战雷革命房间列表服务器.Migrations.自由空域数据库_SQLiteMigrations
{
    /// <inheritdoc />
    public partial class _20240923 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "玩家档案",
                columns: table => new
                {
                    _ = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    账号名 = table.Column<string>(type: "TEXT", nullable: true),
                    密码 = table.Column<string>(type: "TEXT", nullable: true),
                    击杀数 = table.Column<int>(type: "INTEGER", nullable: false),
                    死亡数 = table.Column<int>(type: "INTEGER", nullable: false),
                    助攻数 = table.Column<int>(type: "INTEGER", nullable: false),
                    爬高总高度 = table.Column<long>(type: "INTEGER", nullable: false),
                    能量转化总量 = table.Column<long>(type: "INTEGER", nullable: false),
                    语音总时长 = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    消息发送总数 = table.Column<long>(type: "INTEGER", nullable: false),
                    射出子弹总数 = table.Column<long>(type: "INTEGER", nullable: false),
                    子弹命中次数 = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_玩家档案", x => x._);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "玩家档案");
        }
    }
}
