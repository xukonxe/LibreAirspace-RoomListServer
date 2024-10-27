using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using TouchSocket.Core;
using TouchSocket.Sockets;
using static CMKZ.LocalStorage;
using System.Threading.Tasks;

namespace CMKZ {
	public class TcpClient {
        public Action OnConnect;
        public Action<ITcpClientBase> OnDisconnect;
        public Action<string> OnSend;
        public Action<string, TouchSocket.Sockets.TcpClient> OnReceive;
        public Dictionary<string, Action<Dictionary<string, string>>> OnRead = new();
        public string IP = "127.0.0.1:7789";
        public TouchSocket.Sockets.TcpClient Client = new TouchSocket.Sockets.TcpClient();
        public int ID = 0;
        public ConcurrentDictionary<string, Action<Dictionary<string, string>>> Success = new();
        public TcpClient() {

        }
        public TcpClient(string X) {
            IP = X;
        }
        public TcpClient(string X, string Y) {
            IP = X;
            OnConnect += () => {
                Send(new() { { "标题", "_版本检测" }, { "版本", Y } }, t => {
                    if (t["版本正确"] == "错误") {
                        Client.Close();
                    }
                });
            };
        }
        public void Start() {
            Client.Connected = (client, e) => OnConnect?.Invoke();
            Client.Disconnected = (client, e) => OnDisconnect?.Invoke(client);
            Client.Received = (client, byteBlock, requestInfo) => {
                OnReceive?.Invoke(byteBlock.ToString(), client);
                var A = Encoding.UTF8.GetString(byteBlock.Buffer, 0, byteBlock.Len).JsonToCS<Dictionary<string, string>>(false);
                //下面这段代码应该在主线程中执行
                Task.Run(() => {
                    if (A.ContainsKey("_ID")) {
                        Success[A["_ID"]](A);
                        Success.Remove(t => t.Key == A["_ID"]);
                    } else if (OnRead.ContainsKey(A["标题"])) {
                        OnRead[A["标题"]](A);
                    }
                });
            };
            Client.Setup(new TouchSocketConfig()
                .SetRemoteIPHost(new IPHost(IP))
                .UsePlugin()
                //.ConfigurePlugins(a => a.UseReconnection(5, true, 1000))
                .SetBufferLength(1024 * 64)
                .SetDataHandlingAdapter(() => new FixedHeaderPackageAdapter() { FixedHeaderType = FixedHeaderType.Int }));
            Client.Connect();
        }
        public void Send(Dictionary<string, string> X, Action<Dictionary<string, string>> Y = null) {
            X["_ID"] = ID++.ToString();
            if (Y != null) Success[X["_ID"]] = Y;
            var A = X.ToJson(false);
            Client.Send(A);
            OnSend?.Invoke(A);
        }
    }
}
