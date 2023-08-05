using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LuaInterface;

namespace Lua_Script_Demo
{
    public partial class Form1 : Form
    {
        private TextBox textBox1;
        public Lua lua;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Lua Script Demo";

            textBox1 = new TextBox();
            textBox1.Dock = DockStyle.Fill;
            textBox1.Multiline = true;
            textBox1.Font = new Font("System", 14, FontStyle.Regular);
            this.Controls.Add(textBox1);

            //create lua object
            lua = new Lua();

            //link a C# function to Lua 
            lua.RegisterFunction("DoPrint", this, this.GetType().GetMethod("DoPrint"));

            //load lua script file
            lua.DoFile("script.lua");

            //get globals from lua
            string name = lua.GetString("name");
            double age = lua.GetNumber("age");
            
            DoPrint("name = " + name);
            DoPrint("age = " + age.ToString());

        }

        //this function is visible to Lua script
        public void DoPrint(string text)
        {
            textBox1.Text += text + "\r\n";
        }

    }
}
