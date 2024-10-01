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
    //用于通信传输
    public class 玩家计分数据 {
        public int 击杀数 { get; set; }
        public int 死亡数 { get; set; }
        public int 助攻数 { get; set; }
        public long 爬高总高度 { get; set; }
        public long 能量转化总量 { get; set; }
        public TimeSpan 语音总时长 { get; set; }
        public long 消息发送总数 { get; set; }
        public long 射出子弹总数 { get; set; }
        public long 子弹命中次数 { get; set; }
        public void 档案更新(玩家档案 玩家档案) {
            击杀数 += 玩家档案.击杀数;
            死亡数 += 玩家档案.死亡数;
            助攻数 += 玩家档案.助攻数;
            爬高总高度 += 玩家档案.爬高总高度;
            能量转化总量 += 玩家档案.能量转化总量;
            语音总时长 += 玩家档案.语音总时长;
            消息发送总数 += 玩家档案.消息发送总数;
            射出子弹总数 += 玩家档案.射出子弹总数;
            子弹命中次数 += 玩家档案.子弹命中次数;
        }
    }
}