using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TouchSocket.Sockets;

namespace TGZG.战雷革命房间服务器 {
    public static partial class 公共空间 {
        public static string 版本 => "0.0.4";
        public static List<房间数据类> 房间列表 = new();
        public static 自由空域数据库 数据库 = new();
        public static Dictionary<SocketClient, string> 在线玩家 = new();
        public static void 启动() {
            TGZG.战雷革命房间服务器.房间管理信道.启动();
            TGZG.战雷革命房间服务器.玩家客户端管理信道.启动();
            while (true) {
                string 指令 = Console.ReadLine();
            }
        }
    }
}