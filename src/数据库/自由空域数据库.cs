using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;//from XX select XX
using System.Linq.Expressions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace TGZG.战雷革命房间服务器 {

	public abstract class 自由空域数据库 : DbContext {
		public virtual DbSet<玩家档案> 玩家档案 { get; set; }

		public virtual bool 验证重名(string 账户名) => 
			玩家档案.AsNoTracking().Any(p => p.账号名 == 账户名);

		public virtual 注册验证状态 注册检测(string 账户名, string 密码) {
			if (验证重名(账户名)) {
				return 注册验证状态.账户名已存在;
			}
			if (账户名.Length > 18) {
				return 注册验证状态.账户名过长;
			}
			if (密码.Length is > 18 or < 8) {
				return 注册验证状态.密码应在8到18位之间;
			}
			if (账户名.敏感字检查()) {
				return 注册验证状态.账户名有敏感字;
			}
			return 注册验证状态.验证通过;
		}
	}

	public class 自由空域数据库_MySql : 自由空域数据库 {
		public static string 连接数据 =
		   "server=localhost;" +
		   "database=自由空域档案;" +
		   "user=root;" +
		   "password=admin;";
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			optionsBuilder.UseMySql(连接数据, ServerVersion.AutoDetect(连接数据));
		}
	}

	public class 自由空域数据库_SQLite : 自由空域数据库 {
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			//optionsBuilder.UseMySql(连接数据, ServerVersion.AutoDetect(连接数据));
			optionsBuilder.UseSqlite(new SqliteConnectionStringBuilder() {
					DataSource = "playerdb.db",
					Mode = SqliteOpenMode.ReadWriteCreate
				}.ToString());
		}
	}
}
