using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.Windows.Compatibility;

namespace VRCPictureToClipboard;

public static class QRReader
{
    public static string ReadQRCode(string ImagePath)
    {
        Bitmap image;
        try
        {
            image = (Bitmap) Bitmap.FromFile(ImagePath);
        }
        catch (Exception)
        {
            throw new FileNotFoundException("Resource not found: " + ImagePath);
        }

        using (image)
        {
            LuminanceSource source;
            source = new BitmapLuminanceSource(image);
            BinaryBitmap bitmap = new BinaryBitmap(new HybridBinarizer(source));
            Result result = new MultiFormatReader().decode(bitmap);
            if (result != null)
            {
                return result.Text;
            }
            else
            {
                return string.Empty;
            }
            
        }
       
    }
}