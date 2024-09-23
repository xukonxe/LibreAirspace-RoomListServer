using System.Collections.Generic;
using Newtonsoft.Json;

namespace TGZG.战雷革命房间服务器 {
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
}