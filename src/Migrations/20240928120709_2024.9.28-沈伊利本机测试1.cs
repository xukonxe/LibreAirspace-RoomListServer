using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace 战雷革命房间列表服务器.Migrations
{
    /// <inheritdoc />
    public partial class _2024928沈伊利本机测试1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "玩家档案",
                columns: table => new
                {
                    _ = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    账号名 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    密码 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    击杀数 = table.Column<int>(type: "int", nullable: false),
                    死亡数 = table.Column<int>(type: "int", nullable: false),
                    助攻数 = table.Column<int>(type: "int", nullable: false),
                    爬高总高度 = table.Column<long>(type: "bigint", nullable: false),
                    能量转化总量 = table.Column<long>(type: "bigint", nullable: false),
                    语音总时长 = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    消息发送总数 = table.Column<long>(type: "bigint", nullable: false),
                    射出子弹总数 = table.Column<long>(type: "bigint", nullable: false),
                    子弹命中次数 = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_玩家档案", x => x._);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "玩家档案");
        }
    }
}
