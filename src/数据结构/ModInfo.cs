using Newtonsoft.Json;

namespace TGZG.战雷革命房间服务器 {
	[JsonObject]
	public class ModInfo {
		[JsonProperty(Required = Required.Always, PropertyName = "Name")]
		private string _name;
		[JsonProperty(Required = Required.Always, PropertyName = "Description")]
		private string _description;
		[JsonProperty(Required = Required.Always, PropertyName = "Author")]
		private string _author;
		[JsonProperty(Required = Required.Always, PropertyName = "Version")]
		private string _version;
		[JsonProperty(Required = Required.Always, PropertyName = "Guid")]
		private string _guid;
		[JsonProperty(Required = Required.Always, PropertyName = "ModPackSha512SumAsBase64EncodedString")]
		internal string _modPackSha512SumAsBase64EncodedString;

		/// <summary>
		/// 模组包文件的Sha512校验和(以BASE64编码)。
		/// </summary>
		[JsonIgnore]
		public string ModPackSha512SumAsBase64EncodedString { get => this._modPackSha512SumAsBase64EncodedString; }

		/// <summary>
		/// 模组的一般名称
		/// </summary>
		[JsonIgnore]
		public string Name { get => this._name; }

		/// <summary>
		/// 模组的描述
		/// </summary>
		[JsonIgnore]
		public string Description { get => this._description; }

		/// <summary>
		/// 模组的作者
		/// </summary>
		[JsonIgnore]
		public string Author { get => this._author; }

		/// <summary>
		/// 模组的版本
		/// </summary>
		[JsonIgnore]
		public string Version { get => this._version; }

		/// <summary>
		/// 模组的GUID标识符
		/// </summary>
		[JsonIgnore]
		public string Guid { get => this._guid; }
	}
}