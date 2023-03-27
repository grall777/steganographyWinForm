using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace steganographyWinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            Bitmap img = new Bitmap(txtImageFile.Text);
            string text = txtMessage.Text;

            int charIndex = 0;
            bool messageComplete = false;

            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color pixel = img.GetPixel(i, j);

                    // Hide message in red pixel values
                    if (charIndex < text.Length)
                    {
                        int asciiValue = Convert.ToInt32(text[charIndex]);
                        pixel = Color.FromArgb(asciiValue, pixel.G, pixel.B);
                        charIndex++;
                    }
                    else
                    {
                        messageComplete = true;
                        break;
                    }

                    img.SetPixel(i, j, pixel);
                }
                if (messageComplete) break;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Image Files (*.bmp)|*.bmp";
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName != "")
            {
                FileStream fileStream = (FileStream)saveFileDialog.OpenFile();
                img.Save(fileStream, System.Drawing.Imaging.ImageFormat.Bmp);
                fileStream.Close();
            }
        }

        private void btnExtract_Click(object sender, EventArgs e)
        {
            Bitmap img = new Bitmap(txtImageFile.Text);
            string message = "";

            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color pixel = img.GetPixel(i, j);

                    // Extract message from red pixel values
                    int asciiValue = pixel.R;
                    char c = Convert.ToChar(asciiValue);
                    message += c;
                }
            }

            MessageBox.Show(message);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.bmp)|*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtImageFile.Text = openFileDialog.FileName;
            }
        }
    }
}