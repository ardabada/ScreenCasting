using System.Drawing;

namespace ScreenCasting.Common
{
    public class DesktopImageCommand : ICommand
    {
        public Image Image { get; set; }

        public CommandType Command => CommandType.DesktopImage;

        public void Deserialize(byte[] data)
        {
            Image = (Bitmap)((new ImageConverter()).ConvertFrom(data));
        }

        public byte[] Serialize()
        {
            ImageConverter _imageConverter = new ImageConverter();
            byte[] xByte = (byte[])_imageConverter.ConvertTo(Image, typeof(byte[]));
            return xByte;
        }
    }
}
