using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using TouchSocket.Core;
using TouchSocket.Sockets;
using static CMKZ.LocalStorage;
using System.Threading.Tasks;
using System.Linq;

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
    public class TcpServer {
        public int Port;
        public Dictionary<string, Func<Dictionary<string, string>, SocketClient, Dictionary<string, string>>> OnRead = new();
        public Action<string, SocketClient> OnReceive;
        public Action<SocketClient> OnConnect;
        public Action<SocketClient> OnDisconnect;
        public string Version;
        public TcpService Server = new TcpService();
        public TcpServer(int port) {
            Port = port;
        }
        public TcpServer(int port, string Y) {
            Port = port;
            Version = Y;
        }
        public void Start() {
            Server.Connected = (client, e) => {
                OnConnect?.Invoke(client);
            };
            Server.Disconnected = (client, e) => {
                OnDisconnect?.Invoke(client);
            };
            Server.Received = (client, byteBlock, requestInfo) => {
                OnReceive?.Invoke(byteBlock.ToString(), client);
                var A = byteBlock.ToString().JsonToCS<Dictionary<string, string>>(false);
                Dictionary<string, string> B = null;
                try {
                    B = OnRead[A["标题"]](A, client);
                } catch (Exception ex) {
                    //输出红色错误信息
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"处理消息[\"{A["标题"]}\"]时遇到错误：" + ex.Message);
                    Console.ResetColor();
                }

                if (B != null) {
                    B["_ID"] = A["_ID"];
                    client.Send(B.ToJson(false));//返回消息
                }
            };
            OnRead["_版本检测"] = (t, c) => {
                if (t["版本"] == Version) {
                    return new Dictionary<string, string> { { "版本正确", "正确" } };
                } else {
                    return new Dictionary<string, string> { { "版本正确", "错误" } };
                }
            };
            OnRead["测试信息"] = (t, c) => {
                return new() { { "返回", $"您发来的消息是 {t["内容"]}" } };
            };
            Server.Setup(new TouchSocketConfig()
                .SetListenIPHosts(new IPHost[] { new IPHost(Port) })
                .SetDataHandlingAdapter(() => new FixedHeaderPackageAdapter() { FixedHeaderType = FixedHeaderType.Int }))
            .Start();
        }
        public void AllSend(Dictionary<string, string> X) {
            foreach (var i in Server.GetClients()) {
                i.Send(X.ToJson(false));
            }
        }
        public void Send(SocketClient X, Dictionary<string, string> Y) {
            X.Send(Y.ToJson(false));
        }
        public void Send(string ip, Dictionary<string, string> Y) {
            var 客户端 = Server.GetClients().FirstOrDefault(i => i.IP == ip);
            if (客户端 == null || !客户端.Online) return;
            客户端.Send(Y.ToJson(false));
        }
        public async void SendAsync(string ip, Dictionary<string, string> Y) {
            var 客户端 = Server.GetClients().FirstOrDefault(i => i.IP == ip);
            if (客户端 == null || !客户端.Online) return;
            await 客户端.SendAsync(Y.ToJson(false));
        }
    }
}
