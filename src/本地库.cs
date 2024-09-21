using System;//Action
using System.Collections;
using System.Collections.Generic;//List
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
using MySql.Data.MySqlClient;
using static CMKZ.LocalStorage;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TGZG.战雷革命房间服务器 {
    public static partial class 公共空间 {
        /// <summary>
        /// 每n秒执行一次,精度15毫秒
        /// </summary>
        public static void 每_秒(this double i, Action X) {
            DateTime 上帧 = DateTime.Now;
            ThreadPool.QueueUserWorkItem(_ => {
                while (true) {
                    var 本帧 = DateTime.Now;
                    var 上帧时间 = (本帧 - 上帧).TotalSeconds;
                    //向下取整
                    var n次 = (int)(上帧时间 / i);
                    if ((本帧 - 上帧).TotalSeconds >= i) {
                        for (int j = 0; j < n次; j++) {
                            X();
                        }
                        上帧 = 本帧;
                    }
                }
            });
        }
        /// <summary>
        /// 每n秒执行一次,精度15毫秒
        /// </summary>
        public static void 每_秒(this int i, Action X) {
            ((double)i).每_秒(X);
        }

        public static void Log(this object 消息) {
            消息.log();
        }
        public static void log(this object 消息) {
            Console.WriteLine(消息);
        }
        public static void logwarring(this object 消息) {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(消息);
            Console.ResetColor();
        }
        public static void logerror(this object 消息) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(消息);
            Console.ResetColor();
        }
    }
}