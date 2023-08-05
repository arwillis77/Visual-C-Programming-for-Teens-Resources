partial class Form1
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;
     
    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.button9 = new System.Windows.Forms.Button();
        this.button10 = new System.Windows.Forms.Button();
        this.button11 = new System.Windows.Forms.Button();
        this.button12 = new System.Windows.Forms.Button();
        this.button13 = new System.Windows.Forms.Button();
        this.button14 = new System.Windows.Forms.Button();
        this.button15 = new System.Windows.Forms.Button();
        this.button16 = new System.Windows.Forms.Button();
        this.pictureBox1 = new System.Windows.Forms.PictureBox();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
        this.SuspendLayout();
        // 
        // button9
        // 
        this.button9.Location = new System.Drawing.Point(13, 13);
        this.button9.Name = "button9";
        this.button9.Size = new System.Drawing.Size(75, 23);
        this.button9.TabIndex = 1;
        this.button9.Text = "Rotate 90";
        this.button9.UseVisualStyleBackColor = true;
        this.button9.Click += new System.EventHandler(this.button9_Click);
        // 
        // button10
        // 
        this.button10.Location = new System.Drawing.Point(13, 42);
        this.button10.Name = "button10";
        this.button10.Size = new System.Drawing.Size(75, 23);
        this.button10.TabIndex = 2;
        this.button10.Text = "Rotate 180";
        this.button10.UseVisualStyleBackColor = true;
        this.button10.Click += new System.EventHandler(this.button10_Click);
        // 
        // button11
        // 
        this.button11.Location = new System.Drawing.Point(13, 71);
        this.button11.Name = "button11";
        this.button11.Size = new System.Drawing.Size(75, 23);
        this.button11.TabIndex = 3;
        this.button11.Text = "Rotate 270";
        this.button11.UseVisualStyleBackColor = true;
        this.button11.Click += new System.EventHandler(this.button11_Click);
        // 
        // button12
        // 
        this.button12.Location = new System.Drawing.Point(12, 100);
        this.button12.Name = "button12";
        this.button12.Size = new System.Drawing.Size(75, 23);
        this.button12.TabIndex = 4;
        this.button12.Text = "Flip X";
        this.button12.UseVisualStyleBackColor = true;
        this.button12.Click += new System.EventHandler(this.button12_Click);
        // 
        // button13
        // 
        this.button13.Location = new System.Drawing.Point(12, 129);
        this.button13.Name = "button13";
        this.button13.Size = new System.Drawing.Size(75, 23);
        this.button13.TabIndex = 5;
        this.button13.Text = "Flip Y";
        this.button13.UseVisualStyleBackColor = true;
        this.button13.Click += new System.EventHandler(this.button13_Click);
        // 
        // button14
        // 
        this.button14.Location = new System.Drawing.Point(13, 158);
        this.button14.Name = "button14";
        this.button14.Size = new System.Drawing.Size(75, 23);
        this.button14.TabIndex = 6;
        this.button14.Text = "Flip XY";
        this.button14.UseVisualStyleBackColor = true;
        this.button14.Click += new System.EventHandler(this.button14_Click);
        // 
        // button15
        // 
        this.button15.Location = new System.Drawing.Point(13, 216);
        this.button15.Name = "button15";
        this.button15.Size = new System.Drawing.Size(75, 23);
        this.button15.TabIndex = 7;
        this.button15.Text = "Color Key";
        this.button15.UseVisualStyleBackColor = true;
        this.button15.Click += new System.EventHandler(this.button15_Click);
        // 
        // button16
        // 
        this.button16.Location = new System.Drawing.Point(13, 187);
        this.button16.Name = "button16";
        this.button16.Size = new System.Drawing.Size(75, 23);
        this.button16.TabIndex = 8;
        this.button16.Text = "Turn Green";
        this.button16.UseVisualStyleBackColor = true;
        this.button16.Click += new System.EventHandler(this.button16_Click);
        // 
        // pictureBox1
        // 
        this.pictureBox1.BackColor = System.Drawing.Color.Black;
        this.pictureBox1.Location = new System.Drawing.Point(131, 1);
        this.pictureBox1.Name = "pictureBox1";
        this.pictureBox1.Size = new System.Drawing.Size(500, 500);
        this.pictureBox1.TabIndex = 9;
        this.pictureBox1.TabStop = false;
        // 
        // Form1
        // 
        this.ClientSize = new System.Drawing.Size(633, 502);
        this.Controls.Add(this.pictureBox1);
        this.Controls.Add(this.button16);
        this.Controls.Add(this.button15);
        this.Controls.Add(this.button14);
        this.Controls.Add(this.button13);
        this.Controls.Add(this.button12);
        this.Controls.Add(this.button11);
        this.Controls.Add(this.button10);
        this.Controls.Add(this.button9);
        this.Name = "Form1";
        this.Load += new System.EventHandler(this.Form1_Load);
        this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button button9;
    private System.Windows.Forms.Button button10;
    private System.Windows.Forms.Button button11;
    private System.Windows.Forms.Button button12;
    private System.Windows.Forms.Button button13;
    private System.Windows.Forms.Button button14;
    private System.Windows.Forms.Button button15;
    private System.Windows.Forms.Button button16;
    private System.Windows.Forms.PictureBox pictureBox1;
}
