using System.Collections.Generic;
using System.Linq;

namespace ScreenCasting.Common
{
    public class ScreenCastingCommand
    {
        public static ICommand GetCommand(byte[] data)
        {
            CommandType type = (CommandType)data[0];
            byte[] command = data.Skip(1).ToArray();
            ICommand result = null;
            switch (type)
            {
                case CommandType.ScreenResolution:
                    result = new ScreenResolutionCommand();
                    break;
                case CommandType.DesktopImage:
                    result = new DesktopImageCommand();
                    break;
            }
            if (result == null)
                return null;
            result.Deserialize(command);
            return result;
        }

        public static byte[] Transform(ICommand cmd)
        {
            List<byte> result = new List<byte>();
            result.Add((byte)cmd.Command);
            result.AddRange(cmd.Serialize());
            return result.ToArray();
        }
    }
}
