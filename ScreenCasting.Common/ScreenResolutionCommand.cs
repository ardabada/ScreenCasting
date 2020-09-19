using System.IO;

namespace ScreenCasting.Common
{
    public class ScreenResolutionCommand : ICommand
    {
        public int MonitorsCount { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public CommandType Command => CommandType.ScreenResolution;

        public void Deserialize(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    Width = reader.ReadInt32();
                    Height = reader.ReadInt32();
                }
            }
        }

        public byte[] Serialize()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(Width);
                    writer.Write(Height);
                }
                return stream.ToArray();
            }
        }

        public override string ToString()
        {
            return "Screen resolution: " + Width + "x" + Height;
        }
    }
}
