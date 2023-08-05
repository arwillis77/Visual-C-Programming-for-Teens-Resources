using System;
using System.Drawing;
using System.Windows.Forms;

public partial class Form1 : Form
{
    string[] text = { 
        "AVATAR!",
        "Know that Brittania has entered into a new age of",
        "enlightenment! Know that the time has finally come",
        "for the one true Lord of Brittania to take his place",
        "at the head of his people. Under my guidance, Brit-",
        "tania will flourish. And all of the people shall",
        "rejoice and pay homage to their new... guardian!",
        "Know that you, too, shall kneel before me, Avatar.",
        "You, too, will soon acknowledge my authority. For I",
        "shall be your companion... your provider... and your",
        "master!", "",
        "Ultima VII: The Black Gate",
        "Copyright 1992 by Electronic Arts"
    };

    PictureBox pb;
    Bitmap surface;
    Graphics device;
    Random rand;

    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        //initialize
        this.Text = "Text Drawing Demo";
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.
            FixedSingle;
        this.MaximizeBox = false;
        this.Size = new Size(600, 500);
        rand = new Random();

        //create a new picturebox
        pb = new PictureBox();
        pb.Parent = this;
        pb.Dock = DockStyle.Fill;
        pb.BackColor = Color.Black;

        //create graphics device
        surface = new Bitmap(this.Size.Width, this.Size.Height);
        pb.Image = surface;
        device = Graphics.FromImage(surface);

        //make a new font
        Font font = new Font("Times New Roman", 26, FontStyle.Regular, 
            GraphicsUnit.Pixel);

        //draw the text
        for (int n = 0; n < text.Length; n++)
        {
            device.DrawString(text[n], font, Brushes.Red, 10, 10 + n*28);
        }

        //refresh the drawing surface
        pb.Image = surface;
    }

    private void Form1_FormClosed(object sender, FormClosedEventArgs e)
    {
        device.Dispose();
        surface.Dispose();
    }
}
