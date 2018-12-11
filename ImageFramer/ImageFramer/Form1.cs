using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

// This is the code for your desktop app.
// Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.

namespace ImageFramer
{
    public partial class Form1 : Form
    {
        public string sFilelist = "Hello";
        public int count = 0;


        string frm2Path = "C:/Users/Eng/Desktop/Classic_Gold_Frame_Transparent_PNG_Image.png"; // WideFrame path
        string frm1Path = "C:/Users/Eng/Desktop/Horizontal Classic_Gold_Frame_Transparent.png"; // Tall path
        string testImg = "C:/Users/Eng/Desktop/Wood Company/Tables/images.jpg";
        Image frame1;
        Image imgBackground;

        public Form1()
        {
            InitializeComponent();
            //pictureBox1.Controls.Add(pictureBox2);
            //pictureBox2.Location = new Point(100, 100);
            //pictureBox2.BackColor = Color.Transparent;

            //frame1 = (Bitmap)Image.FromFile(frm1Path);
        }

        private string InitFrame()
        {
            string filename = "";
            FolderBrowserDialog obj = new FolderBrowserDialog();
            DialogResult result = obj.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK)
            {
                filename = result.ToString();


            }
            return (filename);

        }

        //8888888888888888888888888888 Resizing Algorithm 888888888888888888888888888888888888//
        //************************************************************************************//
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return destImage;
        }

        //************************************************************************************//
        //88888888888888888888888888888 Framing Algorithm 888888888888888888888888888888888888//
        //************************************************************************************//
        public void FrameImage(string imgPath)
        {

            Image imgOverlay;
            imgBackground = (Bitmap)Image.FromFile(imgPath);

            if (imgBackground.Height > imgBackground.Width)
            {
                imgOverlay = (Bitmap)Image.FromFile(frm1Path);
            }
            else { imgOverlay = (Bitmap)Image.FromFile(frm2Path); }

            //Frame Sizing
            int newWidth, newHeight;
            newWidth = Convert.ToInt32(imgBackground.Width * 2);
            newHeight = Convert.ToInt32(imgBackground.Height * 2);
            imgOverlay = ResizeImage(imgOverlay, newWidth, newHeight);
            //imgBackground = ResizeImage(imgBackground,Convert.ToInt32(imgBackground.Width*1.1), Convert.ToInt32(imgBackground.Height*1.1));
            //imgBackground = ResizeImage(imgOverlay, newWidth, newHeight);

            Image tmp = new Bitmap(newWidth, newHeight);
            Graphics g = Graphics.FromImage(tmp);

            g.DrawImage(imgBackground, ((imgOverlay.Width - imgBackground.Width) / 3), ((imgOverlay.Height - imgBackground.Height) / 3));
            g.DrawImage(imgOverlay, 0, 0);

            string Path = "C:/Users/Eng/Desktop/Framed/img" + count + "_framed.png";
            tmp.Save(Path, System.Drawing.Imaging.ImageFormat.Png);
            tmp.Dispose();
            imgBackground.Dispose();
            imgOverlay.Dispose();
            g.Dispose();

            /*Dim imgOverlay As Bitmap = CType(Image.FromFile("a.png"), Bitmap);
            Dim imgBackground As Bitmap = CType(Image.FromFile("b.png"), Bitmap;

            Dim tmp As New Bitmap(imgOverlay.Width, imgOverlay.Height) 'New empty bitmap the size of the larger image
            Dim g As Graphics = Graphics.FromImage(tmp)
            g.DrawImage(imgBackground, CInt((imgOverlay.Width - imgBackground.Width) / 2), CInt((imgOverlay.Height - imgBackground.Height) / 2))

            imgOverlay.MakeTransparent(Color.White)
            g.DrawImage(imgOverlay, 0, 0)
            g.Dispose()

            tmp.Save("c.png", Imaging.ImageFormat.Png)

            tmp.Dispose()
            */

        }

        private int round(double v1, double v2)
        {
            throw new NotImplementedException();
        }

        //888888888888888888888888888888888888888888888888888888888888888888888888//
        //888888888888888888888 Listing files in the folder 8888888888888888888888//
        private void button1_Click(object sender, EventArgs e)
        {
            //richTextBox1.Text = "";
            FolderBrowserDialog obj = new FolderBrowserDialog();
            richTextBox1.Text = InitFrame();
            //Retrieving names of files in folder
            if (obj.ShowDialog() == DialogResult.OK)
            {
                string[] files = Directory.GetFiles(obj.SelectedPath, "*jpg");

                foreach (string file in files)
                {
                    count++;
                    richTextBox1.Text += count + " " + file + "\r\n";
                    FrameImage(file);
                }
            }

            //frame1 = ResizeImage(frame1, 300, 350);
            // pictureBox1.Image = frame1;
            //  pictureBox2.Image = frame1;

        }


    }
}
