using Narumikazuchi.Windows.Pipes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass]
    public partial class ClientTests
    {
        [TestMethod]
        public void GenericInstantiationTest()
        {
            using Client<TestMessage> client = Client<TestMessage>.CreateClient("127.0.0.1", "UnitTest");
            Assert.IsNull(client.DataProcessor);
        }

        [TestMethod]
        public void InstantiationTest()
        {
            using Client client = Client.CreateClient("127.0.0.1", "UnitTest");
            Assert.IsNull(client.DataProcessor);
        }

        [TestMethod]
        public void GenericConnectionTest()
        {
            using Client<TestMessage> client = Client<TestMessage>.CreateClient("127.0.0.1", "UnitTest");
            using Server<TestMessage> server = Server<TestMessage>.CreateServer("UnitTest", 24, () => true);
            server.Start();
            client.Connect();
            Int32 time = 0;
            while (!client.Connected)
            {
                Thread.Sleep(1);
                time++;
                if (time > 5000)
                {
                    Assert.AreNotEqual(client.Guid, default);
                    break;
                }
            }
            _instance.WriteLine(client.Guid.ToString());
            client.Disconnect();
            server.Stop();
        }

        [TestMethod]
        public void ConnectionTest()
        {
            using Client client = Client.CreateClient("127.0.0.1", "UnitTest");
            using Server server = Server.CreateServer("UnitTest", 24, () => true);
            server.Start();
            client.Connect();
            Int32 time = 0;
            while (!client.Connected)
            {
                Thread.Sleep(1);
                time++;
                if (time > 5000)
                {
                    Assert.AreNotEqual(client.Guid, default);
                    break;
                }
            }
            _instance.WriteLine(client.Guid.ToString());
            client.Disconnect();
            server.Stop();
        }

        [TestMethod]
        public async Task GenericSendTest()
        {
            using Client<TestMessage> client = Client<TestMessage>.CreateClient("127.0.0.1", "UnitTest");
            using Server<TestMessage> server = Server<TestMessage>.CreateServer("UnitTest", 24, () => true);
            server.DataProcessor = new ServerProcessor(server);
            server.Start();
            client.Connect();
            Int32 time = 0;
            while (!client.Connected)
            {
                Thread.Sleep(1);
                time++;
                if (time > 5000)
                {
                    Assert.AreNotEqual(client.Guid, default);
                    break;
                }
            }
            await client.SendAsync(new("test"));
            client.Disconnect();
            server.Stop();
        }

        [TestMethod]
        public async Task SendTest()
        {
            using Client client = Client.CreateClient("127.0.0.1", "UnitTest");
            using Server server = Server.CreateServer("UnitTest", 24, () => true);
            server.DataReceived += (s, e) => _instance.WriteLine($"Client[{e.FromClient}]: {String.Join(' ', e.Data.Select(b => b.ToString("X")))}");
            server.Start();
            client.Connect();
            Int32 time = 0;
            while (!client.Connected)
            {
                Thread.Sleep(1);
                time++;
                if (time > 5000)
                {
                    Assert.AreNotEqual(client.Guid, default);
                    break;
                }
            }
            await client.SendAsync(new Byte[] { 0xFF, 0xEF, 0xEB, 0x2E, 0x69 });
            client.Disconnect();
            server.Stop();
        }

        [TestMethod]
        public async Task GenericReceiveTest()
        {
            using Client<TestMessage> client = Client<TestMessage>.CreateClient("127.0.0.1", "UnitTest");
            using Server<TestMessage> server = Server<TestMessage>.CreateServer("UnitTest", 24, () => true);
            client.DataProcessor = new ClientProcessor(client);
            server.Start();
            client.Connect();
            Int32 time = 0;
            while (!client.Connected)
            {
                Thread.Sleep(1);
                time++;
                if (time > 5000)
                {
                    Assert.AreNotEqual(client.Guid, default);
                    break;
                }
            }
            await server.BroadcastAsync(new("test"));
            server.Stop();
        }

        [TestMethod]
        public async Task ReceiveTest()
        {
            using Client client = Client.CreateClient("127.0.0.1", "UnitTest");
            using Server server = Server.CreateServer("UnitTest", 24, () => true);
            client.DataReceived += (s, e) => _instance.WriteLine($"[{s.Guid}] From Server: {String.Join(' ', e.Data.Select(b => b.ToString("X")))}");
            server.Start();
            client.Connect();
            Int32 time = 0;
            while (!client.Connected)
            {
                Thread.Sleep(1);
                time++;
                if (time > 5000)
                {
                    Assert.AreNotEqual(client.Guid, default);
                    break;
                }
            }
            await server.BroadcastAsync(new Byte[] { 0xFF, 0xEF, 0xEB, 0x2E, 0x69 });
            server.Stop();
        }

        public TestContext TestContext
        {
            get => _instance;
            set => _instance = value;
        }

        public static TestContext _instance;
    }

    partial class ClientTests
    {
        public class ServerProcessor : ServerDataProcessor<TestMessage>
        {
            public ServerProcessor([DisallowNull] Server<TestMessage> server) : base(server)
            { }

            public override void ProcessReceivedData([DisallowNull] TestMessage data, in Guid fromClient)
            {
                _instance.WriteLine($"Client[{fromClient}]: {data.Message}");
            }
        }

        public class ClientProcessor : ClientDataProcessor<TestMessage>
        {
            public ClientProcessor([DisallowNull] Client<TestMessage> client) : base(client)
            { }

            public override void ProcessReceivedData([DisallowNull] TestMessage data)
            {
                _instance.WriteLine($"[{this.Client.Guid}] From Server: {data.Message}");
            }
        }
    }
}
