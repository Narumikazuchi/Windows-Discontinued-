using Narumikazuchi.Windows.Pipes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System;
using System.Threading;
using System.Linq;

namespace UnitTest
{
    [TestClass]
    public partial class ServerTests
    {
        [TestMethod]
        public void GenericInstantiationTest()
        {
            using Server<TestMessage> server = Server<TestMessage>.CreateServer("UnitTest", 24, () => true);
            Assert.IsNotNull(server);
            Assert.IsNull(server.DataProcessor);
        }

        [TestMethod]
        public void InstantiationTest()
        {
            using Server server = Server.CreateServer("UnitTest", 24, () => true);
            Assert.IsNotNull(server);
            Assert.IsNull(server.DataProcessor);
        }

        [TestMethod]
        public void GenericConnectionTest()
        {
            using Server<TestMessage> server = Server<TestMessage>.CreateServer("UnitTest", 24, () => true);
            using Client<TestMessage> client1 = Client<TestMessage>.CreateClient("127.0.0.1", "UnitTest");
            using Client<TestMessage> client2 = Client<TestMessage>.CreateClient("127.0.0.1", "UnitTest");
            server.Start();
            client1.Connect();
            client2.Connect();
            while (!client1.Connected ||
                   !client2.Connected)
            {
                Thread.Sleep(1);
            }
            _instance.WriteLine(client1.Guid.ToString());
            _instance.WriteLine(client2.Guid.ToString());
            server.Stop();
        }

        [TestMethod]
        public void ConnectionTest()
        {
            using Server server = Server.CreateServer("UnitTest", 24, () => true);
            using Client client1 = Client.CreateClient("127.0.0.1", "UnitTest");
            using Client client2 = Client.CreateClient("127.0.0.1", "UnitTest");
            server.Start();
            client1.Connect();
            client2.Connect();
            while (!client1.Connected ||
                   !client2.Connected)
            {
                Thread.Sleep(1);
            }
            _instance.WriteLine(client1.Guid.ToString());
            _instance.WriteLine(client2.Guid.ToString());
            server.Stop();
        }

        [TestMethod]
        public void GenericSendTest()
        {
            using Server<TestMessage> server = Server<TestMessage>.CreateServer("UnitTest", 24, () => true);
            using Client<TestMessage> client1 = Client<TestMessage>.CreateClient("127.0.0.1", "UnitTest");
            using Client<TestMessage> client2 = Client<TestMessage>.CreateClient("127.0.0.1", "UnitTest");
            client1.DataProcessor = new ClientProcessor(client1);
            client2.DataProcessor = new ClientProcessor(client2);
            server.Start();
            client1.Connect();
            client2.Connect();
            while (!client1.Connected ||
                   !client2.Connected)
            {
                Thread.Sleep(1);
            }
            server.BroadcastAsync(new("test"));
            //server.Send(new("test2"), client1.Guid);
        }

        [TestMethod]
        public void SendTest()
        {
            using Server server = Server.CreateServer("UnitTest", 24, () => true);
            using Client client1 = Client.CreateClient("127.0.0.1", "UnitTest");
            using Client client2 = Client.CreateClient("127.0.0.1", "UnitTest");
            client1.DataReceived += (s, e) => _instance.WriteLine($"[{s.Guid}] From Server: {String.Join(' ', e.Data.Select(b => b.ToString("X")))}");
            client2.DataReceived += (s, e) => _instance.WriteLine($"[{s.Guid}] From Server: {String.Join(' ', e.Data.Select(b => b.ToString("X")))}");
            server.Start();
            client1.Connect();
            client2.Connect();
            while (!client1.Connected ||
                   !client2.Connected)
            {
                Thread.Sleep(1);
            }
            _instance.WriteLine(client1.Guid.ToString());
            _instance.WriteLine(client2.Guid.ToString());
            foreach (Guid guid in server.Clients)
            {
                _instance.WriteLine(guid.ToString());
            }
            server.BroadcastAsync(new Byte[] { 0xFF, 0xEF, 0xEB, 0x2E, 0x69 });
            server.SendAsync(new Byte[] { 0xEF, 0xEB, 0xEA, 0x29 }, client1.Guid);
        }

        [TestMethod]
        public void GenericReceiveTest()
        {
            using Server<TestMessage> server = Server<TestMessage>.CreateServer("UnitTest", 24, () => true);
            using Client<TestMessage> client = Client<TestMessage>.CreateClient("127.0.0.1", "UnitTest");
            server.DataProcessor = new ServerProcessor(server);
            server.Start();
            client.Connect();
            while (!client.Connected)
            {
                Thread.Sleep(1);
            }
            client.SendAsync(new("test"));
            server.Stop();
        }

        [TestMethod]
        public void ReceiveTest()
        {
            using Server server = Server.CreateServer("UnitTest", 24, () => true);
            using Client client = Client.CreateClient("127.0.0.1", "UnitTest");
            server.DataReceived += (s, e) => _instance.WriteLine($"Client[{e.FromClient}]: {String.Join(' ', e.Data.Select(b => b.ToString("X")))}");
            server.Start();
            client.Connect();
            while (!client.Connected)
            {
                Thread.Sleep(1);
            }
            client.SendAsync(new Byte[] { 0xFF, 0xEF, 0xEB, 0x2E, 0x69 });
            server.Stop();
        }

        public TestContext TestContext
        {
            get => _instance;
            set => _instance = value;
        }

        public static TestContext _instance;
    }

    partial class ServerTests
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
