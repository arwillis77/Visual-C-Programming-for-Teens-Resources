using System;
using System.Drawing;
using System.Windows.Forms;

public partial class Form1 : Form
{
    public Game game;
    public Bitmap planet;

    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        //set up the form
        this.Text = "Game Framework Demo";

        //create game object
        game = new Game(this, 600, 500);

        //load bitmap
        planet = game.LoadBitmap("planet.bmp");
        if (planet == null)
        {
            MessageBox.Show("Error loading planet.bmp");
            Environment.Exit(0);
        }

        //draw the bitmap 
        game.Device.DrawImage(planet, 10, 10);
        game.Device.DrawImage(planet, 400, 10, 100, 100);
        game.Update();
    
    }

    private void Form1_FormClosed(object sender, FormClosedEventArgs e)
    {
        //delete game object
        game = null;
    }

}
