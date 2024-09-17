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
    public static partial class 玩家客户端管理信道 {
        public static int 端口 => 16313;
        public static TcpServer 服务器 = new(16313, 版本);
        public static void 启动() {
            服务器.OnConnect += c => {
                Print($"用户 {c.IP} 连接");
            };
            服务器.OnReceive += (t, c) => {
                //Print($"用户 {c.IP} 发来消息：{t}");
            };
            服务器.OnDisconnect += c => {
                Print($"用户 {c.IP} 断开连接");
            };
            服务器.OnRead["测试信息"] = (t, c) => {
                return new() { { "返回", $"您发来的消息是 {t["内容"]}" } };
            };
            服务器.OnRead["请求服务器列表"] = (t, c) => {
                var 数据 = new 所有房间数据类();
                lock (房间列表) {
                    数据.房间列表 = 房间列表;
                }
                return new() { { "房间列表", 数据.ToJson(格式美化: false) } };
            };
            服务器.Start();
            Print($"玩家管理信道已在端口{端口}上启动");
        }
    }
}