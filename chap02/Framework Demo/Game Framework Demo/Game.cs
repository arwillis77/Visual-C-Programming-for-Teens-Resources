using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;

public class Game
{
    private Graphics p_device;
    private Bitmap p_surface;
    private PictureBox p_pb;
    private Form p_frm;

    public Game(Form1 form, int width, int height)
    {
        Trace.WriteLine("Game class constructor");

        //set form properties
        p_frm = form;
        p_frm.FormBorderStyle = FormBorderStyle.FixedSingle;
        p_frm.MaximizeBox = false;
        p_frm.Size = new Size(width, height);

        //create a picturebox
        p_pb = new PictureBox();
        p_pb.Parent = p_frm;
        p_pb.Dock = DockStyle.Fill;
        p_pb.BackColor = Color.Black;

        //create graphics device
        p_surface = new Bitmap(p_frm.Size.Width, p_frm.Size.Height);
        p_pb.Image = p_surface;
        p_device = Graphics.FromImage(p_surface);
    }

    ~Game()
    {
        Trace.WriteLine("Game class destructor");
        p_device.Dispose();
        p_surface.Dispose();
        p_pb.Dispose();
    }

    public Graphics Device
    {
        get { return p_device; }
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

    public void Update()
    {
        //refresh the drawing surface
        p_pb.Image = p_surface;
    }

}
