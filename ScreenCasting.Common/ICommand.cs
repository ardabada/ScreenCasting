using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenCasting.Common
{
    public interface ICommand
    {
        CommandType Command { get; }
        byte[] Serialize();
        void Deserialize(byte[] data);
    }
}
