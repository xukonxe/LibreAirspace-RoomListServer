using Newtonsoft.Json;

namespace TGZG.战雷革命房间服务器 {
    public static partial class 公共空间 {
        public static string 版本 => "0.0.3";
        public static List<房间数据类> 房间列表 = new();
        public static void 启动() {
            TGZG.战雷革命房间服务器.房间管理信道.启动();
            TGZG.战雷革命房间服务器.玩家客户端管理信道.启动();
            while (true) {
                string 指令 = Console.ReadLine();
            }
        }
    }
    public struct 所有房间数据类 {
        public List<房间数据类> 房间列表;
        [JsonIgnore]
        public int 房间总数 => 房间列表.Count;
        [JsonIgnore]
        public 房间数据类 this[int index] => 房间列表[index];
        [JsonIgnore]
        public 房间数据类 this[string 房间名] => 房间列表.Find(x => x.房间名 == 房间名);
        public bool 存在房间(string 房间名) => 房间列表.Exists(x => x.房间名 == 房间名);
    }
    public struct 房间数据类 {
        public string IP;
        public string 房间名;
        public string 房间描述;
        public string 房主;
        public int 人数;
        public string 地图名;
        public bool 房间密码;
        public string 房间版本;
        public int 每秒同步次数;
        public DateTime 房间创建时间;
    }
    public class 房间参数类 {
        public string 房间名;
        public string 房间描述;
        public string 房主;
        public int 人数;
        public string 地图名;
        public string 房间密码;
        public string 房间版本;
        public int 每秒同步次数;
        public DateTime 房间创建时间;
        public 房间数据类 To房间数据类() {
            return new 房间数据类 {
                房间名 = 房间名,
                房间描述 = 房间描述,
                房主 = 房主,
                人数 = 人数,
                地图名 = 地图名,
                房间密码 = 房间密码 is not null or "",
                房间创建时间 = 房间创建时间,
                房间版本 = 房间版本,
                每秒同步次数 = 每秒同步次数                
            };
        }
    }
}