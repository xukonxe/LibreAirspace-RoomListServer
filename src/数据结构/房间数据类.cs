using System;
using System.Collections.Generic;

namespace TGZG.战雷革命房间服务器 {
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
		public ModInfo[] 模组列表;
        public 模式类型 模式;
        public List<载具类型> 可选载具;
        public List<队伍> 可选队伍;
    }
}