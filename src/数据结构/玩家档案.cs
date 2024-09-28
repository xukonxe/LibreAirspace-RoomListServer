using System;//Action
using System.ComponentModel.DataAnnotations;

namespace TGZG.战雷革命房间服务器 {
	public class 玩家档案 {
		//TKTek: 这个是句柄吗？
        //沈伊利: 对，用作数据库内存储档案的唯一标识符。
        //EFCore强制要求的主键
        [Key]
        public int _ { get; set; }
        public string 账号名 { get; set; }
        public string 密码 { get; set; }

        public int 击杀数 { get; set; }
        public int 死亡数 { get; set; }
        public int 助攻数 { get; set; }
        public long 爬高总高度 { get; set; }
        public long 能量转化总量 { get; set; }
        public TimeSpan 语音总时长 { get; set; }
        public long 消息发送总数 { get; set; }
        public long 射出子弹总数 { get; set; }
        public long 子弹命中次数 { get; set; }
    }
}