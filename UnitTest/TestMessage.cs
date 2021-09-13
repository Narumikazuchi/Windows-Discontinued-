using Narumikazuchi.Serialization;
using Narumikazuchi.Serialization.Bytes;
using System;
using System.Linq;
using System.Text;

namespace UnitTest
{
    public class TestMessage : IByteSerializable, IEquatable<TestMessage>
    {
        public TestMessage() { }
        public TestMessage(String msg) => this.Message = msg;

        public String Message { get; set; }

        UInt32 IByteSerializable.InitializeUninitializedState(Byte[] bytes) => throw new NotImplementedException();
        UInt32 IByteSerializable.SetState(Byte[] bytes)
        {
            Int32 size = BitConverter.ToInt32(bytes);
            String message = Encoding.UTF8.GetString(bytes, 4, size);
            this.Message = message;
            return (UInt32)(size + 4);
        }
        Byte[] ISerializable.ToBytes()
        {
            Byte[] data = Encoding.UTF8.GetBytes(this.Message);
            Byte[] size = BitConverter.GetBytes(data.Length);
            return size.Concat(data)
                       .ToArray();
        }

        public override Boolean Equals(Object obj) => obj is TestMessage other && this.Equals(other);

        public Boolean Equals(TestMessage other) => other is null ? false : this.Message.Equals(other.Message);
    }
}
