using iTextSharp.text.pdf;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace K2.WebAPI.Service
{
    public class CUtility
    {
        private readonly Logger logger = LogManager.GetLogger(string.Empty);
        public CUtility() { }

        public string DocuFloDownloadDoc
        {
            get
            {
                return ConfigurationManager.AppSettings["Docuflo:DownloadDoc"];
            }
        }

        public string DocuFloCheckOutDoc
        {
            get
            {
                return ConfigurationManager.AppSettings["Docuflo:CheckOutDoc"];
            }
        }
        public string DocuFloCheckInDoc
        {
            get
            {
                return ConfigurationManager.AppSettings["Docuflo:CheckInDoc"];
            }
        }
        public string DocuFloVersionDoc
        {
            get
            {
                return ConfigurationManager.AppSettings["Docuflo:GetVersion"];
            }
        }
        public string URLDMSUAT
        {
            get
            {
                 return ConfigurationManager.AppSettings["URLDMSUAT"];
            }
        }

        public string URLDMSPROD
        {
            get { return ConfigurationManager.AppSettings["URLDMSPROD"]; }
        }

        public System.Drawing.Image LoadImage(string charbase64)
        {
            //data:image/gif;base64,
            //this image is a single pixel (black)
            
            byte[] bytes = Convert.FromBase64String(charbase64);

            System.Drawing.Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = System.Drawing.Image.FromStream(ms);
                //image.Save(System.IO.Path.GetTempPath() + "\\myImage.Jpeg", ImageFormat.Jpeg);
            }

            return image;
        }

        public System.Drawing.Image Resize(System.Drawing.Image image, int newWidth, int maxHeight, bool onlyResizeIfWider)
        {
            if (onlyResizeIfWider && image.Width <= newWidth) newWidth = image.Width;

            var newHeight = image.Height * newWidth / image.Width;
            if (newHeight > maxHeight)
            {
                // Resize with height instead  
                newWidth = image.Width * maxHeight / image.Height;
                newHeight = maxHeight;
            }

            var res = new System.Drawing.Bitmap(newWidth, newHeight);

            using (var graphic = System.Drawing.Graphics.FromImage(res))
            {
                graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphic.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                graphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphic.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            return res;
        }
        public byte[] PutOnCanvas(System.Drawing.Image image, int width, int height, System.Drawing.Color canvasColor)
        {
            var res = new System.Drawing.Bitmap(width, height);
            double ratio = Math.Max((double)image.Width / (double)width, (double)image.Height / (double)height);
            using (var g = System.Drawing.Graphics.FromImage(res))
            {
                g.Clear(canvasColor);
                ratio = 10;
                var x = (int)((width - image.Width) / ratio);//200 - 125/70 = 1.07 asli 200
                var y = (int)((height - image.Height) / ratio);//180 - 100/70 = 1.14 asli 100
                g.DrawImageUnscaled(image, x, y, image.Width, image.Height);//image.Width, image.Height);


            }
            res.MakeTransparent();
            var ret = new MemoryStream();
            res.Save(ret, System.Drawing.Imaging.ImageFormat.Png);
            return ret.ToArray();
        }
    }
}