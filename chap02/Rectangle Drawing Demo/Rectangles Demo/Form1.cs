using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

public partial class Form1 : Form
{
    PictureBox pb;
    Timer timer;
    Bitmap surface;
    Graphics device;
    Random rand;

    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        //set up the form
        this.Text = "Rectangle Drawing Demo";
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.Size = new Size(600, 500);

        //create a new picturebox
        pb = new PictureBox();
        pb.Parent = this;
        pb.Dock = DockStyle.Fill;
        pb.BackColor = Color.Black;

        //create graphics device
        surface = new Bitmap(this.Size.Width, this.Size.Height);
        pb.Image = surface;
        device = Graphics.FromImage(surface);

        //create random generator
        rand = new Random();

        //set up the timer
        timer = new Timer();
        timer.Interval = 20;
        timer.Enabled = true;
        timer.Tick += new EventHandler(timer_Tick);
    }

    private void Form1_FormClosed(object sender, FormClosedEventArgs e)
    {
        device.Dispose();
        surface.Dispose();
        timer.Dispose();
    }


    private void drawRect()
    {
        //make a random color
        int A = rand.Next(0, 255);
        int R = rand.Next(0, 255);
        int G = rand.Next(0, 255);
        int B = rand.Next(0, 255);
        Color color = Color.FromArgb(A, R, G, B);

        //make pen out of color
        int width = rand.Next(2, 8);
        Pen pen = new Pen(color, width);

        //random line ends
        int x = rand.Next(1, this.Size.Width - 50);
        int y = rand.Next(1, this.Size.Height - 50);
        Rectangle rect = new Rectangle(x, y, 50, 50);

        //draw the rectangle
        device.DrawRectangle(pen, rect);

        //refresh the drawing surface
        pb.Image = surface;
    }

    private void timer_Tick(object source, EventArgs e) 
    {
        drawRect();
    }

}
