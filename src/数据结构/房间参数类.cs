using System;
using System.Collections.Generic;

namespace TGZG.战雷革命房间服务器 {
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
		public ModInfo[] 模组列表;
        public 模式类型 模式;
        public List<载具类型> 可选载具;
        public List<队伍> 可选队伍;
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
                每秒同步次数 = 每秒同步次数,
				模组列表 = 模组列表,
                模式 = 模式,
                可选载具 = 可选载具,
                可选队伍 = 可选队伍
            };
        }
    }
}