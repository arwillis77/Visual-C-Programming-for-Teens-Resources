using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMPLib;

namespace Music_Playback_Demo
{
    public partial class Form1 : Form
    {

        WindowsMediaPlayerClass player;
                

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            player = new WindowsMediaPlayerClass();
            player.URL = "song.mid";
        }
    }
}
