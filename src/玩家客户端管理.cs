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
    public static partial class 公共空间 {
        public static bool 敏感字检查(this string 内容) {
            return false;
        }
    }
    public static partial class 玩家客户端管理信道 {
        public static int 端口 => 16313;
        public static TcpServer 服务器 = new(16313, 客户端版本);
        public static void 启动() {
            服务器.OnConnect += c => {
                Print($"用户 {c.IP} 连接");
            };
            服务器.OnReceive += (t, c) => {
                //Print($"用户 {c.IP} 发来消息：{t}");
            };
            服务器.OnDisconnect += c => {
                if (在线玩家.ContainsKey(c)) {
                    Print($"用户 {在线玩家[c]}({c.IP}) 断开连接");
                    在线玩家.Remove(c);
                } else {
                    Print($"未登录用户 {c.IP} 断开连接");
                }
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
            服务器.OnRead["注册"] = (t, c) => {
                var 账户名 = t["账号"];
                var 密码 = t["密码"];
                var 验证 = 数据库.注册检测(账户名, 密码);
                //验证失败返回对应消息
                if (验证 is not 注册验证状态.验证通过)
                    return 验证 switch {
                        注册验证状态.账户名已存在 => new() { { "验证失败", $"账号 {账户名} 已存在" } },
                        注册验证状态.账户名过长 => new() { { "验证失败", $"账号 {账户名} 过长" } },
                        注册验证状态.密码应在8到18位之间 => new() { { "验证失败", $"密码应在8到18位之间" } },
                        注册验证状态.账户名有敏感字 => new() { { "验证失败", $"账号 {账户名} 有敏感字" } },
                        _ => new() { { "验证失败", "未知错误" } }
                    };
                lock (数据库) {
                    数据库.玩家档案.Add(new 玩家档案() { 账号名 = 账户名, 密码 = 密码 });
                    数据库.SaveChanges();
                }
                bool 保存 = 数据库.玩家档案.Any(p => p.账号名 == 账户名);
                return 保存 switch {
                    true => new() { ["状态"] = $"注册成功" },
                    false => new() { ["状态"] = $"注册失败,服务器保存异常" }
                };
            };
            服务器.OnRead["登录"] = (t, c) => {
                var 账户名 = t["账号"];
                var 密码 = t["密码"];
                bool 验证 = 数据库.玩家档案
                    .AsNoTracking()
                    .Any(p => p.账号名 == 账户名 && p.密码 == 密码);
                //验证失败返回对应消息
                if (!验证) return new() { { "验证失败", "账号或密码错误" } };
                在线玩家[c] = 账户名;
                return new() { { "状态", "登录成功" } };
            };
            服务器.Start();
            Print($"玩家管理信道已在端口{端口}上启动");
        }
    }
}