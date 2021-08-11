using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace RTech.Demo.Utilities
{
    public class CaptchaHelper
    {
        public CaptchaHelper()
        {

        }

        /// <summary>
        /// Session Key
        /// </summary>
        //private const string SessionKey = "__imageSessionKey_";

        /// <summary>
        /// Generate random string value
        /// </summary>
        /// <returns></returns>
        private string getRandomString()
        {

            string returnString = string.Empty;
            string letters = "0123456789";

            Random rand = new Random();

            int length = rand.Next(5, 8);
            for (int i = 0; i < length; i++)
            {
                int pos = rand.Next(0, 10);
                returnString += letters[pos].ToString();
            }
            return returnString;
        }

        /// <summary>
        /// Create image Byte[]
        /// </summary>
        /// <returns></returns>
        public byte[] DrawByte()
        {
            byte[] returnByte = { };
            Bitmap bitmapImage = new Bitmap(150, 30, PixelFormat.Format32bppArgb);
            string randomString = getRandomString();
            if (randomString.Length > 4)
            {
                randomString = randomString.Substring(0, 4);
            }
            //
            // Here we generate random string
            string key = randomString;

            //
            // key string adding to Session
            RiddhaSession.Captcha = key;
            //HttpContext.Current.Session.Add(SessionKey, key);

            //
            // Creating image with key
            using (Graphics g = Graphics.FromImage(bitmapImage))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                Rectangle rect = new Rectangle(0, 0, 150, 30);
                HatchBrush hBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White);
                g.FillRectangle(hBrush, rect);
                hBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.Red, Color.Black);
                float fontSize = 20;
                Font font = new Font(FontFamily.GenericSerif, fontSize, FontStyle.Underline);
                float x = 10;
                float y = 1;
                PointF fPoint = new PointF(x, y);
                g.DrawString(key, font, hBrush, fPoint);

                using (MemoryStream ms = new MemoryStream())
                {
                    bitmapImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    returnByte = ms.ToArray();
                }
            }
            return returnByte;
        }

        public bool Verify(string key)
        {
            bool success = false;
            if (RiddhaSession.Captcha != null)
            {
                if (RiddhaSession.Captcha == key)
                {
                    success = true;
                }
            }
            return success;
        }
    }
}