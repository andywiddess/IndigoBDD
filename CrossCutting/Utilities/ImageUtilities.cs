using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Helper class for Image conversions etc
    /// </summary>
    public static class ImageUtilities
    {

        /// <summary>
        /// Check to see if the passed byte array could be a PNG image
        /// According to Wikipedia, Png have a fixed initial header of (dec) 137 80 78 71 13 10 26 10
        /// http://en.wikipedia.org/wiki/Portable_Network_Graphics
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public static bool IsByteArrayPNGFormatImage(byte[] byteArray)
        {
            return AreArraysEquivalent(byteArray,pngFileHeader);
        }

        /// <summary>
        /// Return TRUE if the arrays are equal within their length - they can be differing lengths.
        /// </summary>
        /// <param name="array1"></param>
        /// <param name="array2"></param>
        /// <returns></returns>
        public static bool AreArraysEquivalent(byte[] array1, byte[] array2)
        {
            int checkLength = Math.Min(array1.Length, array2.Length);
            for (int count = 0; count < checkLength; count++)
            {
                if (array1[count] != array2[count])
                {
                    return false;
                }
            }
            return true;
        }

        private static byte[] pngFileHeader = new byte[8] { 137, 80, 78, 71, 13, 10, 26, 10 };

        /// <summary>
        /// Check to see if the passed byte array could be a JPG image
        /// http://www.fileformat.info/format/jpeg/egff.htm
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public static bool IsByteArrayJPGFormatImage(byte[] byteArray)
        {
            return AreArraysEquivalent(byteArray, jpgFileHeader);
        }

        private static byte[] jpgFileHeader = new byte[2] { 255, 216 };


        /// <summary>
        /// Convert an Image to byte[]
        /// </summary>
        /// <param name="image"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static byte[] ConvertImageToByteArray(System.Drawing.Image image, System.Drawing.Imaging.ImageFormat format)
        {
            byte[] returnValue = null;
            if (image == null)
            {
                returnValue = null;
            }
            else
            { 
                using(System.IO.MemoryStream stream = new System.IO.MemoryStream())
                {
                    image.Save(stream, format);
                    stream.Close();
                    returnValue = stream.ToArray();
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Convert byte[] to an Image
        /// </summary>
        /// <param name="imageBytes"></param>
        /// <returns></returns>
        public static System.Drawing.Image ConvertByteArrayToImage(byte[] imageBytes)
        {
            System.Drawing.Image image = null;
            if (imageBytes == null)
            {
                return null;
            }
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream(imageBytes))
            {
                image = System.Drawing.Image.FromStream(stream);
                stream.Close();
            }
            return new System.Drawing.Bitmap(image, image.Size);
        }
    }
}
