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

namespace TGZG.战雷革命房间服务器 {
    public static partial class 房间管理信道 {
        public static int 端口 => 16312;
        public static TcpServer 服务器 = new(16312, 版本);
        public static void 启动() {
            服务器.OnConnect += c => {
                Print($"游戏服务器 {c.IP} 连接");
            };
            服务器.OnReceive += (t, c) => {
                //Print($"用户 {c.IP} 发来消息：{t}");
            };
            服务器.OnDisconnect += c => {
                注销(c.IP);
            };
            服务器.OnRead["测试信息"] = (t, c) => {
                return new() { { "返回", $"您发来的消息是 {t["内容"]}" } };
            };
            服务器.OnRead["注册"] = (消息, 客户端) => {
                var 数据 = 消息["数据"].JsonToCS<房间参数类>();
                if (房间列表.Exists(x => x.房间名 == 数据.房间名)) {
                    return new() { { "失败", "房间名已存在" } };
                }
                var 房间 = 数据.To房间数据类();
                房间.IP = 客户端.IP;
                lock (房间列表) {
                    房间列表.Add(房间);
                }
                Print($"房间 {房间.房间名} 已注册");
                return new() { { "成功", "注册成功" } };
            };
            服务器.OnRead["验证登录"] = (t, c) => {
                var 账号 = t["账号"];
                var 密码 = t["密码"];
                bool 验证 = 数据库.玩家档案
                   .AsNoTracking()
                   .Any(p => p.账号名 == 账号 && p.密码 == 密码);
                if (!验证)
                    return new() { { "验证失败", "账号或密码错误" } };
                if (!在线玩家.ContainsValue(账号))
                    return new() { { "失败", "此玩家不在线" } };
                return new() { { "成功", "登录验证成功" } };
            };
            服务器.OnRead["房间数据更新"] = (t, c) => {
                var 数据 = t["数据"].JsonToCS<房间参数类>();
                if (!房间列表.Exists(x => x.房间名 == 数据.房间名)) {
                    return new() { { "失败", "房间名不存在" } };
                }
                var 房间 = 数据.To房间数据类();
                房间.IP = c.IP;
                lock (房间列表) {
                    var 房间数据 = 房间列表.Find(x => x.房间名 == 数据.房间名);
                    房间列表.Remove(房间数据);
                    房间列表.Add(房间);
                }
                return new() { { "成功", "" } };
            };
            服务器.Start();
            Print($"房间管理信道已在端口{端口}上启动");
            //10.每_秒(() => {
            //    //每十秒发送心跳询问
            //    foreach (var 房间 in 房间列表) {
            //        Task.Run(async () => {
            //            服务器.SendAsync(房间.IP, new() {
            //                { "标题", "心跳询问" }
            //            });
            //        });
            //    }
            //});
        }
        public static void 注销(房间数据类 数据) {
            房间列表.Remove(数据);
            Print($"房间 {数据.房间名} 已注销");
        }
        public static void 注销(string IP) {
            var 房间 = 房间列表.Find(x => x.IP == IP);
            房间列表.Remove(房间);
            Print($"房间 {房间.房间名} 已注销");
        }
    }
}