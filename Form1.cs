using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WindowsFormsApp1;


namespace Lab1_kg_
{
    public partial class Form1 : Form
    {
        private Bitmap image, image2;
        private BitmapData ImageData, ImageData2;
        private byte[] buffer, buffer2;
        private double gammacorrection;
        private int b, g, r, r_x, g_x, b_x, r_y, g_y, b_y, grayscale, location, location2;
        private sbyte weight_x, weight_y;
        private sbyte[,] weights_x;
        private sbyte[,] weights_y;

        //save
        private void save1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".txt";
            saveFileDialog.Filter = "Image files | *.png; *.jpg; *.bmp | All Files (*.*) | *.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK) //если в диалоговом окне нажата кнопка "ОК"
            {
                try
                {
                    pictureBox1.Image.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                catch
                {
                    MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            /*if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveFileDialog.FileName.Length > 0)
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName, true))
                {
                    sw.WriteLine(pictureBox1.Image);
                }
            }*/
        }

        private void gaussToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Gauss filter = new Gauss();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void openingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Filter filter = new Opening();
            //backgroundWorker1.RunWorkerAsync(filter);
        }

        private void closingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Filter filter = new Closing();
            //backgroundWorker1.RunWorkerAsync(filter);
        }

        private void gradToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Filter filter = new Grad();
            //backgroundWorker1.RunWorkerAsync(filter);
        }

        private void dilationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Filter filter = new Dilation();
            //backgroundWorker1.RunWorkerAsync(filter);
        }

        private void erosionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Filter filter = new Erosion();
            //backgroundWorker1.RunWorkerAsync(filter);
        }

        private void binarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // if it is to open file successful.
            if (image != null)
            {
                // Establish a color object.
                Color curColor;
                int ret;
                // The width of the image.
                for (int iX = 0; iX < image.Width; iX++)
                {
                    // The height of the image.
                    for (int iY = 0; iY < image.Height; iY++)
                    {
                        // Get the pixel from bitmap object.
                        curColor = image.GetPixel(iX, iY);
                        // Transform RGB to Y (gray scale)
                        ret = (int)(curColor.R * 0.299 + curColor.G * 0.578 + curColor.B * 0.114);
                        // This is our threshold, you can change it and to try what are different.
                        if (ret > 120)
                        {
                            ret = 255;
                        }
                        else
                        {
                            ret = 0;
                        }
                        // Set the pixel into the bitmap object.
                        image.SetPixel(iX, iY, Color.FromArgb(ret, ret, ret));
                    } // The closing 'The height of the image'.
                } // The closing 'The width of the image'.
                // Force to redraw.
                Invalidate();
                pictureBox1.Image = image;
            }
        }

        private IntPtr pointer, pointer2;

        public Form1()
        {
            InitializeComponent();
            weights_x = new sbyte[,] { { 1, 0, -1 }, { 2, 0, -2 }, { 1, 0, -1 } };
            weights_y = new sbyte[,] { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } };
        }

        //dropdown table
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files | *.png; *.jpg; *.bmp | All Files (*.*) | *.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                image = null;
                // image.Dispose();
                image = new Bitmap(dialog.FileName);
                image2 = new Bitmap(image.Width, image.Height);
                pictureBox1.Image = image;
                pictureBox1.Refresh();
            }

        }

        //inersion filter
        private void inversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InvertFilter filter = new InvertFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        //blur filter
        private void blurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new BlurFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        //sobel (black/white) filter
        /*private void sobelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new Sobel();
            backgroundWorker1.RunWorkerAsync(filter);
        }*/

        //median filter (often used to remove noise from an image or signal)
        private void medianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new Median();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        //waves filter
        private void wavesToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Filter filter = new Waves();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        //download stop 
        private void button1_Click(object sender, EventArgs e) { backgroundWorker1.CancelAsync(); }

        #region background
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Bitmap newImage = ((Filter)e.Argument).processImage(image, backgroundWorker1);
            if (backgroundWorker1.CancellationPending != true)
                image = newImage;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                pictureBox1.Image = image;
                pictureBox1.Refresh();
            }
            progressBar1.Value = 0;
        }



        #endregion

        //convert
        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap Y, I, Q;
            Convert Test = new Convert();
            Test.convertation(image, out Y, out I, out Q);

            pbY.Image = Y;
            pbI.Image = I;
            pbQ.Image = Q;
        }

        //back load
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //Gamma filter
        private void button3_Click(object sender, EventArgs e)
        {
            using (Bitmap buffer_image = (Bitmap)image.Clone())
            {
                gammacorrection = 1 / ((double)trackBar1.Value / 10);
                ImageData = buffer_image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                buffer = new byte[3 * image.Width * image.Height];
                pointer = ImageData.Scan0;
                Marshal.Copy(pointer, buffer, 0, buffer.Length);
                for (int i = 0; i < image.Height * 3 * image.Width; i += 3)
                {
                    b = (int)(255 * Math.Pow((buffer[i] / 255.0), gammacorrection));
                    g = (int)(255 * Math.Pow((buffer[i + 1] / 255.0), gammacorrection));
                    r = (int)(255 * Math.Pow((buffer[i + 2] / 255.0), gammacorrection));
                    if (b > 255) b = 255;
                    if (g > 255) g = 255;
                    if (r > 255) r = 255;
                    buffer[i] = (byte)b;
                    buffer[i + 1] = (byte)g;
                    buffer[i + 2] = (byte)r;
                }
                Marshal.Copy(buffer, 0, pointer, buffer.Length);
                buffer_image.UnlockBits(ImageData);
                pictureBox1.Image = (Bitmap)buffer_image.Clone();
            }
        }

        //Sobel filter
        private void sobelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            ImageData2 = image2.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            buffer = new byte[ImageData.Stride * image.Height];
            buffer2 = new byte[ImageData.Stride * image.Height];
            pointer = ImageData.Scan0;
            pointer2 = ImageData2.Scan0;
            Marshal.Copy(pointer, buffer, 0, buffer.Length);
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width * 3; x += 3)
                {
                    r_x = g_x = b_x = 0; //reset the gradients in x-direcion values
                    r_y = g_y = b_y = 0; //reset the gradients in y-direction values
                    location = x + y * ImageData.Stride;
                    for (int yy = -(int)Math.Floor(weights_y.GetLength(0) / 2.0d), yyy = 0; yy <= (int)Math.Floor(weights_y.GetLength(0) / 2.0d); yy++, yyy++)
                    {
                        if (y + yy >= 0 && y + yy < image.Height) //to prevent crossing the bounds of the array
                        {
                            for (int xx = -(int)Math.Floor(weights_x.GetLength(1) / 2.0d) * 3, xxx = 0; xx <= (int)Math.Floor(weights_x.GetLength(1) / 2.0d) * 3; xx += 3, xxx++)
                            {
                                if (x + xx >= 0 && x + xx <= image.Width * 3 - 3) //to prevent crossing the bounds of the array
                                {
                                    location2 = x + xx + (yy + y) * ImageData.Stride;
                                    weight_x = weights_x[yyy, xxx];
                                    weight_y = weights_y[yyy, xxx];
                                    //applying the same weight to all channels
                                    b_x += buffer[location2] * weight_x;
                                    g_x += buffer[location2 + 1] * weight_x; //G_X
                                    r_x += buffer[location2 + 2] * weight_x;
                                    b_y += buffer[location2] * weight_y;
                                    g_y += buffer[location2 + 1] * weight_y;//G_Y
                                    r_y += buffer[location2 + 2] * weight_y;
                                }
                            }
                        }
                    }
                    //getting the magnitude for each channel
                    b = (int)Math.Sqrt(Math.Pow(b_x, 2) + Math.Pow(b_y, 2));
                    g = (int)Math.Sqrt(Math.Pow(g_x, 2) + Math.Pow(g_y, 2));
                    r = (int)Math.Sqrt(Math.Pow(r_x, 2) + Math.Pow(r_y, 2));

                    if (b > 255) b = 255;
                    if (g > 255) g = 255;
                    if (r > 255) r = 255;


                    grayscale = (b + g + r) / 3;


                    buffer2[location] = (byte)grayscale;
                    buffer2[location + 1] = (byte)grayscale;
                    buffer2[location + 2] = (byte)grayscale;
                }
            }
            Marshal.Copy(buffer2, 0, pointer2, buffer.Length);
            image.UnlockBits(ImageData);
            image2.UnlockBits(ImageData2);
            pictureBox1.Image = image2;
        }

    }
}