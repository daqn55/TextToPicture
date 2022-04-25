using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextToPicture
{
    public partial class Form1 : Form
    {
        private Image generatedImage;
        private Image generatedImageMobile;

        public Form1()
        {
            InitializeComponent();
        }

        private Image DrawText(String text, Font font, Color textColor, Color backColor, int width)
        {
            //first, create a dummy bitmap just to get a graphics object
            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            //measure the string to see how big the image needs to be
            SizeF textSize = drawing.MeasureString(text, font, width);

            //free up the dummy image and old graphics object
            img.Dispose();
            drawing.Dispose();

            //create a new image of the right size

            img = new Bitmap(width, (int)textSize.Height);

            drawing = Graphics.FromImage(img);

            //paint the background
            drawing.Clear(backColor);

            //create a brush for the text
            Brush textBrush = new SolidBrush(textColor);

            drawing.TextRenderingHint = TextRenderingHint.AntiAlias;

            RectangleF rectF = new RectangleF(0, 0, width, textSize.Height);

            StringFormat sf = new StringFormat()
            {
                Alignment = StringAlignment.Near
            };

            drawing.DrawString(text, font, textBrush, rectF, sf);

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();

            return img;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                this.generatedImage = DrawText(textBox1.Text, new Font("Times New Roman", 14f), Color.Black, Color.White, 580);
                this.generatedImageMobile = DrawText(textBox1.Text, new Font("Times New Roman", 41f), Color.Black, Color.White, 1000);
                pictureBox1.Image = this.generatedImage;
                pictureBox2.Image = this.generatedImageMobile;
            }
        }

        private void SaveAs(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                FileStream fs = (FileStream)saveFileDialog1.OpenFile();

                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        this.generatedImage.Save(fs, ImageFormat.Jpeg);
                        break;

                    case 2:
                        this.generatedImage.Save(fs, ImageFormat.Bmp);
                        break;

                    case 3:
                        this.generatedImage.Save(fs, ImageFormat.Gif);
                        break;
                }

                fs.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var textToSave = SaveText.Text;
            var pathStandartFolder = "StandarPictures";
            var pathMobileFolder = "MobilePictures";
            var currentDiretory = AppDomain.CurrentDomain.BaseDirectory;
            if (!string.IsNullOrEmpty(textToSave))
            {
                DirectoryInfo standartDirectory = Directory.CreateDirectory(currentDiretory + pathStandartFolder);
                DirectoryInfo mobileDirectory = Directory.CreateDirectory(currentDiretory + pathMobileFolder);

                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Save(currentDiretory + pathStandartFolder + "\\" + "anot" + textToSave + ".jpg", ImageFormat.Jpeg);
                    pictureBox2.Image.Save(currentDiretory + pathMobileFolder + "\\" + "anot" + textToSave + ".jpg", ImageFormat.Jpeg);
                }
            }
        }
    }
}
