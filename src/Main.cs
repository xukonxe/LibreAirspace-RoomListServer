using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TouchSocket.Sockets;

namespace TGZG.战雷革命房间服务器 {
    public static partial class 公共空间 {
        public static string 版本 => "0.0.4";
        public static List<房间数据类> 房间列表 = new();
        public static 自由空域数据库 数据库 = new 自由空域数据库_SQLite();		//开发用SQLite数据库后端。
        public static Dictionary<SocketClient, string> 在线玩家 = new();
        public static void 启动() {
            TGZG.战雷革命房间服务器.房间管理信道.启动();
            TGZG.战雷革命房间服务器.玩家客户端管理信道.启动();
			//以后还要做一个命令系统！
			//这里暂时只挂一个循环转让处理器控制权给其它线程/进程的程序。
			for (; ; ) {
				System.Threading.Thread.Yield();
			}
        }
    }
}