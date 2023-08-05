using System;
using System.Drawing;
using System.Windows.Forms;

public partial class Form1 : Form
{
    Bitmap surface;
    Graphics device;
    Bitmap image;

    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        //set up the form
        this.Text = "Bitmap Drawing Demo";
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.
            FixedSingle;
        this.MaximizeBox = false;
        
        //create graphics device
        surface = new Bitmap(this.Size.Width, this.Size.Height);
        pictureBox1.Image = surface;
        device = Graphics.FromImage(surface);

        //load the bitmap
        image = LoadBitmap("skellyarcher.png");

        //draw the bitmap 
        device.DrawImage(image, 0, 0);
    }

    public Bitmap LoadBitmap(string filename)
    {
        Bitmap bmp = null;
        try
        {
            bmp = new Bitmap(filename);
        }
        catch (Exception ex) { }
        return bmp;
    }


    private void Form1_FormClosed(object sender, FormClosedEventArgs e)
    {
        device.Dispose();
        surface.Dispose();
        image.Dispose();
    }

    private void button9_Click(object sender, EventArgs e)
    {
        image.RotateFlip(RotateFlipType.Rotate90FlipNone);
        device.DrawImage(image, 0, 0);
        pictureBox1.Image = surface;
    }

    private void button10_Click(object sender, EventArgs e)
    {
        image.RotateFlip(RotateFlipType.Rotate180FlipNone);
        device.DrawImage(image, 0, 0);
        pictureBox1.Image = surface;
    }

    private void button11_Click(object sender, EventArgs e)
    {
        image.RotateFlip(RotateFlipType.Rotate270FlipNone);
        device.DrawImage(image, 0, 0);
        pictureBox1.Image = surface;
    }

    private void button12_Click(object sender, EventArgs e)
    {
        image.RotateFlip(RotateFlipType.RotateNoneFlipX);
        device.DrawImage(image, 0, 0);
        pictureBox1.Image = surface;
    }

    private void button13_Click(object sender, EventArgs e)
    {
        image.RotateFlip(RotateFlipType.RotateNoneFlipY);
        device.DrawImage(image, 0, 0);
        pictureBox1.Image = surface;
    }

    private void button14_Click(object sender, EventArgs e)
    {
        image.RotateFlip(RotateFlipType.RotateNoneFlipXY);
        device.DrawImage(image, 0, 0);
        pictureBox1.Image = surface;
    }

    private void button15_Click(object sender, EventArgs e)
    {
        Color white = Color.FromArgb(255, 255, 255);
        Color black = Color.FromArgb(0, 0, 0);

        for (int x = 0; x < image.Width - 1; x++)
        {
            for (int y = 0; y < image.Height - 1; y++)
            {
                if (image.GetPixel(x,y) == white)
                    image.SetPixel(x, y, black);
            }
        }
        device.DrawImage(image, 0, 0);
        pictureBox1.Image = surface;
    }

    private void button16_Click(object sender, EventArgs e)
    {
        for (int x = 0; x < image.Width - 1; x++)
        {
            for (int y = 0; y < image.Height - 1; y++)
            {
                Color pixelColor = image.GetPixel(x, y);
                Color newColor = Color.FromArgb(0, pixelColor.G, 0);
                image.SetPixel(x, y, newColor);
            }
        }
        device.DrawImage(image, 0, 0);
        pictureBox1.Image = surface;
    }

}

