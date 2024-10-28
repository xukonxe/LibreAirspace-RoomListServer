using System;//Action
using System.Collections;
using System.Collections.Generic;//List
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.IO;//File
using System.Linq;//from XX select XX
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;//Timer
using CMKZ;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using static CMKZ.LocalStorage;
using static TGZG.战雷革命房间服务器.公共空间;
using PktTypTab = TGZG.战雷革命房间服务器.CommunicateConstant.PacketType;

namespace TGZG.战雷革命房间服务器 {
    public class 房间管理信道 : TcpServer {
        public static int 端口 => 16312;
        //public static TcpServer 服务器 = new(16312, 版本);

		public 房间管理信道() : base(端口, 版本) {
			base.OnConnect += c => {
				Print($"游戏服务器 {c.IP} 连接");
			};
			base.OnReceive += (t, c) => {
				//Print($"用户 {c.IP} 发来消息：{t}");
			};
			base.OnDisconnect += c => {
				注销(c.IP);
			};
		}

		protected override void RegisterPacketHandler() {
			base.RegisterPacketHandler();
			base.m_PacketHandlerRegistry.RegisterPacketType(PktTypTab.注册_房间);
			base.m_PacketHandlerRegistry.RegisterPacketHandler(PktTypTab.注册_房间, "自由空域房间列表服务器__处理自由空域对局服务器注册房间请求",
				(客户端, 消息, 额外参数) => {
					var 数据 = 消息["数据"].JsonToCS<房间参数类>();
					if (房间列表.Exists(x => x.房间名 == 数据.房间名)) {
						SendRpcReturnValue(客户端, new() { { "失败", "房间名已存在" } }, 消息);
						return;
					}
					var 房间 = 数据.To房间数据类();
					房间.IP = 客户端.IP;
					lock (房间列表) {
						房间列表.Add(房间);
					}
					Print($"房间 {房间.房间名} 已注册");
					SendRpcReturnValue(客户端, new Dictionary<string, string>() { { "成功", "注册成功" } }, 消息);
				});
			base.m_PacketHandlerRegistry.RegisterPacketType(PktTypTab.验证登录);
			base.m_PacketHandlerRegistry.RegisterPacketHandler(PktTypTab.验证登录, "自由空域房间列表服务器__处理客户端验证登录",
				(客户端, 消息, 额外参数) => {
					//明文密码，难搞哦
					string 账号 = 消息["账号"];
					string 密码 = 消息["密码"];
					bool 验证 = 数据库.玩家档案
					   .AsNoTracking()
					   .Any(p => p.账号名 == 账号 && p.密码 == 密码);
					if (!验证) {
						SendRpcReturnValue(客户端, new Dictionary<string, string>() { { "验证失败", "账号或密码错误" } }, 消息);
						return;
					}
					if (!在线玩家.ContainsValue(账号)) {
						SendRpcReturnValue(客户端, new Dictionary<string, string>() { { "失败", "此玩家不在线" } }, 消息);
						return;
					}
					SendRpcReturnValue(客户端, new Dictionary<string, string>() { { "成功", "登录验证成功" } }, 消息);
				});
			base.m_PacketHandlerRegistry.RegisterPacketType(PktTypTab.房间数据更新);
			base.m_PacketHandlerRegistry.RegisterPacketHandler(PktTypTab.房间数据更新, "自由空域房间列表服务器__处理房间数据更新",
				(客户端, 消息, 额外参数) => {
					房间参数类 数据 = 消息["数据"].JsonToCS<房间参数类>();
					if (!房间列表.Exists(x => x.房间名 == 数据.房间名)) {
						SendRpcReturnValue(客户端, new Dictionary<string, string>() { { "失败", "房间名不存在" } }, 消息);
						return;
					}
					房间数据类 房间 = 数据.To房间数据类();
					房间.IP = 客户端.IP;
					lock (房间列表) {
						var 房间数据 = 房间列表.Find(x => x.房间名 == 数据.房间名);
						房间列表.Remove(房间数据);
						房间列表.Add(房间);
					}
					SendRpcReturnValue(客户端, new Dictionary<string, string>() { { "成功", "" } }, 消息);
				});
			base.m_PacketHandlerRegistry.RegisterPacketType(PktTypTab.玩家数据上传);
			base.m_PacketHandlerRegistry.RegisterPacketHandler(PktTypTab.玩家数据上传, "自由空域房间列表服务器__处理玩家数据上传",
				(客户端, 消息, 额外参数) => {
					var 账户名 = 消息["账号"];
					var 玩家数据 = 消息["数据"].JsonToCS<玩家计分数据>();
					var 玩家档案 = 数据库.玩家档案.FirstOrDefault(p => p.账号名 == 账户名);
					if (玩家档案 != null) {
						lock (玩家档案) {
							玩家数据.档案更新(玩家档案);
							数据库.SaveChanges();
						}
					}
				});
		}

		public void 启动() {
            base.Start();
            Print($"房间管理信道已在端口{端口}上启动");
        }
        public void 注销(房间数据类 数据) {
            房间列表.Remove(数据);
            Print($"房间 {数据.房间名} 已注销");
        }
        public void 注销(string IP) {
            var 房间 = 房间列表.Find(x => x.IP == IP);
            房间列表.Remove(房间);
            Print($"房间 {房间.房间名} 已注销");
        }
    }
}