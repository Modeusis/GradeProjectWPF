using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TournamentsApplication.Utility
{
    public class ImageConverter
    {
        public static byte[] ImageToByteArray(System.Windows.Media.ImageSource imageSource)
        {
            var bitmap = imageSource as BitmapSource;
            if (bitmap == null) return null;

            using (var ms = new MemoryStream())
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.Save(ms);
                return ms.ToArray();
            }
        }
        public static System.Windows.Media.ImageSource ByteArrayToImage(byte[] byteArray)
        {
            using (var ms = new MemoryStream(byteArray))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                bitmap.Freeze();
                return bitmap;
            }
        }
    }
}
