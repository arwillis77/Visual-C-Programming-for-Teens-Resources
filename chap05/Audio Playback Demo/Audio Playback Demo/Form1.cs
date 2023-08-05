using System;
using System.Media;
using System.Reflection;
using System.IO;
using System.Windows.Forms;

namespace Audio_Playback_Demo
{
    public partial class Form1 : Form
    {
        //Microsoft.VisualBasic.Devices.Audio audioDevice;
        System.Media.SoundPlayer[] audio;


        public SoundPlayer LoadSoundFile(string filename)
        {
            SoundPlayer sound = null;
            try
            {
                sound = new SoundPlayer();
                sound.SoundLocation = filename;
                sound.Load();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "Error loading sound");
            }
            return sound;
        }

        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            audio = new SoundPlayer[5];

            //load audio using constructor
            audio[0] = new SoundPlayer("launch1.wav");

            //load audio using Load method
            audio[1] = new SoundPlayer();
            audio[1].SoundLocation = "launch2.wav";
            audio[1].Load();

            //load audio from wave file
            audio[2] = LoadSoundFile("missed1.wav");
            audio[3] = LoadSoundFile("laser.wav");

            //load audio from resource
            audio[4] = new SoundPlayer();
            audio[4].Stream = Properties.Resources.foom;

        }

        private void button_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.Text == "Asterisk")
            {
                SystemSounds.Asterisk.Play();
            }
            else if (button.Text == "Beep")
            {
                SystemSounds.Beep.Play();
            }
            else if (button.Text == "Exclamation")
            {
                SystemSounds.Exclamation.Play();
            }
            else if (button.Text == "Hand")
            {
                SystemSounds.Hand.Play();
            }
            else if (button.Text == "Question")
            {
                SystemSounds.Question.Play();
            }
            else if (button.Text == "Launch1")
            {
                audio[0].Play();
            }
            else if (button.Text == "Launch2")
            {
                audio[1].Play();
            }
            else if (button.Text == "Missed1")
            {
                audio[2].Play();
            }
            else if (button.Text == "Laser")
            {
                audio[3].Play();
            }
            else if (button.Text == "Foom")
            {
                audio[4].Play();
            }

        }
    }
}
