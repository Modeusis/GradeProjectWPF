using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TournamentsApplication.Utility
{
    public class ImageConverter : IValueConverter
    {
        public static byte[] StandardUserIcon => standardUserIcon.Value;
        public static byte[] StandardHeaderIcon => standardHeaderIcon.Value;
        public static byte[] BlankImage => blankImage.Value;
        private static readonly Lazy<byte[]> standardUserIcon = new Lazy<byte[]>(() =>
        {
            try
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri("pack://application:,,,/Resources/Images/StandardUserImage.png");
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return ImageToByteArray(bitmapImage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading image: {ex.Message}");
                return null;
            }
        });
        private static readonly Lazy<byte[]> standardHeaderIcon = new Lazy<byte[]>(() =>
        {
            try
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri("pack://application:,,,/Resources/Images/UserHeaderImage.jpg");
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return ImageToByteArray(bitmapImage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading image: {ex.Message}");
                return null;
            }
        });
        private static readonly Lazy<byte[]> blankImage = new Lazy<byte[]>(() =>
        {
            try
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri("pack://application:,,,/Resources/Images/blankImage.png");
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return ImageToByteArray(bitmapImage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading image: {ex.Message}");
                return null;
            }
        });


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
        public static byte[] LoadImageAsByteArray(string imageUri)
        {
            try
            {
                if (string.IsNullOrEmpty(imageUri))
                {
                    throw new ArgumentException("Image URI is null or empty.", nameof(imageUri));
                }

                var resourceInfo = Application.GetResourceStream(new Uri(imageUri, UriKind.RelativeOrAbsolute));
                if (resourceInfo == null)
                {
                    throw new FileNotFoundException("Image resource not found.", imageUri);
                }

                using (var ms = new MemoryStream())
                {
                    resourceInfo.Stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading image from URI: {ex.Message}");
                return null;
            }
        }
        public static byte[] OpenAndLoadImage()
        {
            try
            {
                var openFileDialog = new OpenFileDialog
                {
                    Filter = "PNG Files (*.png)|*.png|All Files (*.*)|*.*|JPG Files (*.jpg)|*.jpg",
                    Title = "Select an Image File"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    string selectedFilePath = openFileDialog.FileName;
                    return File.ReadAllBytes(selectedFilePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening image file: {ex.Message}");
            }

            return null;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte[] btImage)
            {
                return ByteArrayToImage(btImage);
            }
            return ByteArrayToImage(BlankImage);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ImageSource Image) 
            {
                return ImageToByteArray(Image);
            }
            return BlankImage;
        }
    }
}
