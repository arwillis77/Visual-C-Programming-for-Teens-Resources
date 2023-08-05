<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Open1 = New System.Windows.Forms.OpenFileDialog
        Me.Save1 = New System.Windows.Forms.SaveFileDialog
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LoadCharacterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SaveAsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.QuitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.txtCHA = New System.Windows.Forms.TextBox
        Me.txtSTR = New System.Windows.Forms.TextBox
        Me.txtINT = New System.Windows.Forms.TextBox
        Me.txtDEX = New System.Windows.Forms.TextBox
        Me.txtSTA = New System.Windows.Forms.TextBox
        Me.cboCategory = New System.Windows.Forms.ComboBox
        Me.txtDefense = New System.Windows.Forms.TextBox
        Me.txtValue = New System.Windows.Forms.TextBox
        Me.txtWeight = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.cboAttackNumDice = New System.Windows.Forms.ComboBox
        Me.cboAttackDie = New System.Windows.Forms.ComboBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnLoadInvImage = New System.Windows.Forms.Button
        Me.btnLoadDropImage = New System.Windows.Forms.Button
        Me.picInventory = New System.Windows.Forms.PictureBox
        Me.picDrop = New System.Windows.Forms.PictureBox
        Me.txtDropImageFilename = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtInventoryImageFilename = New System.Windows.Forms.TextBox
        Me.txtName = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtDesc = New System.Windows.Forms.TextBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.btnDelete = New System.Windows.Forms.Button
        Me.btnAdd = New System.Windows.Forms.Button
        Me.lstItems = New System.Windows.Forms.ListBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtSummary = New System.Windows.Forms.TextBox
        Me.MenuStrip1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.picInventory, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picDrop, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(699, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.LoadCharacterToolStripMenuItem, Me.SaveToolStripMenuItem, Me.SaveAsToolStripMenuItem, Me.ToolStripSeparator1, Me.QuitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.NewToolStripMenuItem.Text = "New"
        '
        'LoadCharacterToolStripMenuItem
        '
        Me.LoadCharacterToolStripMenuItem.Name = "LoadCharacterToolStripMenuItem"
        Me.LoadCharacterToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.LoadCharacterToolStripMenuItem.Text = "Load..."
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        '
        'SaveAsToolStripMenuItem
        '
        Me.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem"
        Me.SaveAsToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.SaveAsToolStripMenuItem.Text = "Save As..."
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(149, 6)
        '
        'QuitToolStripMenuItem
        '
        Me.QuitToolStripMenuItem.Name = "QuitToolStripMenuItem"
        Me.QuitToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.QuitToolStripMenuItem.Text = "Quit"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.txtSummary)
        Me.GroupBox3.Controls.Add(Me.Label6)
        Me.GroupBox3.Controls.Add(Me.Label15)
        Me.GroupBox3.Controls.Add(Me.GroupBox1)
        Me.GroupBox3.Controls.Add(Me.cboCategory)
        Me.GroupBox3.Controls.Add(Me.txtDefense)
        Me.GroupBox3.Controls.Add(Me.txtValue)
        Me.GroupBox3.Controls.Add(Me.txtWeight)
        Me.GroupBox3.Controls.Add(Me.Label8)
        Me.GroupBox3.Controls.Add(Me.cboAttackNumDice)
        Me.GroupBox3.Controls.Add(Me.cboAttackDie)
        Me.GroupBox3.Controls.Add(Me.Label7)
        Me.GroupBox3.Controls.Add(Me.Label5)
        Me.GroupBox3.Controls.Add(Me.Label1)
        Me.GroupBox3.Controls.Add(Me.btnLoadInvImage)
        Me.GroupBox3.Controls.Add(Me.btnLoadDropImage)
        Me.GroupBox3.Controls.Add(Me.picInventory)
        Me.GroupBox3.Controls.Add(Me.picDrop)
        Me.GroupBox3.Controls.Add(Me.txtDropImageFilename)
        Me.GroupBox3.Controls.Add(Me.Label2)
        Me.GroupBox3.Controls.Add(Me.txtInventoryImageFilename)
        Me.GroupBox3.Controls.Add(Me.txtName)
        Me.GroupBox3.Controls.Add(Me.Label9)
        Me.GroupBox3.Controls.Add(Me.Label3)
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Controls.Add(Me.txtDesc)
        Me.GroupBox3.Location = New System.Drawing.Point(182, 27)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(512, 523)
        Me.GroupBox3.TabIndex = 49
        Me.GroupBox3.TabStop = False
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(6, 39)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(56, 15)
        Me.Label15.TabIndex = 80
        Me.Label15.Text = "Category"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Controls.Add(Me.Label14)
        Me.GroupBox1.Controls.Add(Me.txtCHA)
        Me.GroupBox1.Controls.Add(Me.txtSTR)
        Me.GroupBox1.Controls.Add(Me.txtINT)
        Me.GroupBox1.Controls.Add(Me.txtDEX)
        Me.GroupBox1.Controls.Add(Me.txtSTA)
        Me.GroupBox1.Location = New System.Drawing.Point(327, 15)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(168, 175)
        Me.GroupBox1.TabIndex = 79
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Buffs/Modifiers"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(20, 26)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(31, 15)
        Me.Label10.TabIndex = 65
        Me.Label10.Text = "STR"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(20, 50)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(31, 15)
        Me.Label11.TabIndex = 66
        Me.Label11.Text = "DEX"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(20, 75)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(28, 15)
        Me.Label12.TabIndex = 67
        Me.Label12.Text = "STA"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(20, 97)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(26, 15)
        Me.Label13.TabIndex = 68
        Me.Label13.Text = "INT"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(20, 124)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(32, 15)
        Me.Label14.TabIndex = 69
        Me.Label14.Text = "CHA"
        '
        'txtCHA
        '
        Me.txtCHA.Location = New System.Drawing.Point(101, 124)
        Me.txtCHA.Name = "txtCHA"
        Me.txtCHA.Size = New System.Drawing.Size(40, 20)
        Me.txtCHA.TabIndex = 74
        '
        'txtSTR
        '
        Me.txtSTR.Location = New System.Drawing.Point(101, 26)
        Me.txtSTR.Name = "txtSTR"
        Me.txtSTR.Size = New System.Drawing.Size(40, 20)
        Me.txtSTR.TabIndex = 70
        '
        'txtINT
        '
        Me.txtINT.Location = New System.Drawing.Point(101, 100)
        Me.txtINT.Name = "txtINT"
        Me.txtINT.Size = New System.Drawing.Size(40, 20)
        Me.txtINT.TabIndex = 73
        '
        'txtDEX
        '
        Me.txtDEX.Location = New System.Drawing.Point(101, 50)
        Me.txtDEX.Name = "txtDEX"
        Me.txtDEX.Size = New System.Drawing.Size(40, 20)
        Me.txtDEX.TabIndex = 71
        '
        'txtSTA
        '
        Me.txtSTA.Location = New System.Drawing.Point(101, 75)
        Me.txtSTA.Name = "txtSTA"
        Me.txtSTA.Size = New System.Drawing.Size(40, 20)
        Me.txtSTA.TabIndex = 72
        '
        'cboCategory
        '
        Me.cboCategory.FormattingEnabled = True
        Me.cboCategory.Items.AddRange(New Object() {"Armor", "Misc", "Necklace", "Ring", "Weapon"})
        Me.cboCategory.Location = New System.Drawing.Point(112, 39)
        Me.cboCategory.Name = "cboCategory"
        Me.cboCategory.Size = New System.Drawing.Size(121, 21)
        Me.cboCategory.TabIndex = 78
        '
        'txtDefense
        '
        Me.txtDefense.Location = New System.Drawing.Point(112, 205)
        Me.txtDefense.Name = "txtDefense"
        Me.txtDefense.Size = New System.Drawing.Size(59, 20)
        Me.txtDefense.TabIndex = 77
        '
        'txtValue
        '
        Me.txtValue.Location = New System.Drawing.Point(112, 153)
        Me.txtValue.Name = "txtValue"
        Me.txtValue.Size = New System.Drawing.Size(59, 20)
        Me.txtValue.TabIndex = 76
        '
        'txtWeight
        '
        Me.txtWeight.Location = New System.Drawing.Point(112, 130)
        Me.txtWeight.Name = "txtWeight"
        Me.txtWeight.Size = New System.Drawing.Size(59, 20)
        Me.txtWeight.TabIndex = 75
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(6, 205)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(90, 15)
        Me.Label8.TabIndex = 63
        Me.Label8.Text = "Defense/Armor"
        '
        'cboAttackNumDice
        '
        Me.cboAttackNumDice.FormattingEnabled = True
        Me.cboAttackNumDice.Items.AddRange(New Object() {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10"})
        Me.cboAttackNumDice.Location = New System.Drawing.Point(112, 178)
        Me.cboAttackNumDice.Name = "cboAttackNumDice"
        Me.cboAttackNumDice.Size = New System.Drawing.Size(47, 21)
        Me.cboAttackNumDice.TabIndex = 62
        Me.cboAttackNumDice.Text = "0"
        '
        'cboAttackDie
        '
        Me.cboAttackDie.FormattingEnabled = True
        Me.cboAttackDie.Items.AddRange(New Object() {"", "D4", "D6", "D8", "D10", "D12", "D20"})
        Me.cboAttackDie.Location = New System.Drawing.Point(165, 178)
        Me.cboAttackDie.Name = "cboAttackDie"
        Me.cboAttackDie.Size = New System.Drawing.Size(47, 21)
        Me.cboAttackDie.TabIndex = 61
        Me.cboAttackDie.Text = "D6"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(6, 178)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(39, 15)
        Me.Label7.TabIndex = 60
        Me.Label7.Text = "Attack"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(6, 153)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(65, 15)
        Me.Label5.TabIndex = 59
        Me.Label5.Text = "Sale Value"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(6, 130)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(73, 15)
        Me.Label1.TabIndex = 58
        Me.Label1.Text = "Weight (lbs)"
        '
        'btnLoadInvImage
        '
        Me.btnLoadInvImage.Location = New System.Drawing.Point(238, 381)
        Me.btnLoadInvImage.Name = "btnLoadInvImage"
        Me.btnLoadInvImage.Size = New System.Drawing.Size(40, 19)
        Me.btnLoadInvImage.TabIndex = 56
        Me.btnLoadInvImage.Text = "Load"
        Me.btnLoadInvImage.UseVisualStyleBackColor = True
        '
        'btnLoadDropImage
        '
        Me.btnLoadDropImage.Location = New System.Drawing.Point(104, 382)
        Me.btnLoadDropImage.Name = "btnLoadDropImage"
        Me.btnLoadDropImage.Size = New System.Drawing.Size(40, 19)
        Me.btnLoadDropImage.TabIndex = 55
        Me.btnLoadDropImage.Text = "Load"
        Me.btnLoadDropImage.UseVisualStyleBackColor = True
        '
        'picInventory
        '
        Me.picInventory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picInventory.Location = New System.Drawing.Point(150, 253)
        Me.picInventory.Name = "picInventory"
        Me.picInventory.Size = New System.Drawing.Size(128, 128)
        Me.picInventory.TabIndex = 46
        Me.picInventory.TabStop = False
        '
        'picDrop
        '
        Me.picDrop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picDrop.Location = New System.Drawing.Point(16, 253)
        Me.picDrop.Name = "picDrop"
        Me.picDrop.Size = New System.Drawing.Size(128, 128)
        Me.picDrop.TabIndex = 45
        Me.picDrop.TabStop = False
        '
        'txtDropImageFilename
        '
        Me.txtDropImageFilename.Location = New System.Drawing.Point(16, 402)
        Me.txtDropImageFilename.Name = "txtDropImageFilename"
        Me.txtDropImageFilename.Size = New System.Drawing.Size(128, 20)
        Me.txtDropImageFilename.TabIndex = 25
        Me.txtDropImageFilename.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(6, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(41, 15)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Name"
        '
        'txtInventoryImageFilename
        '
        Me.txtInventoryImageFilename.Location = New System.Drawing.Point(150, 403)
        Me.txtInventoryImageFilename.Name = "txtInventoryImageFilename"
        Me.txtInventoryImageFilename.Size = New System.Drawing.Size(128, 20)
        Me.txtInventoryImageFilename.TabIndex = 26
        Me.txtInventoryImageFilename.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(112, 16)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(189, 20)
        Me.txtName.TabIndex = 11
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(6, 66)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(70, 15)
        Me.Label9.TabIndex = 43
        Me.Label9.Text = "Description"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(13, 384)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(58, 15)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Drop Img"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(153, 383)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(79, 15)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Inventory Img"
        '
        'txtDesc
        '
        Me.txtDesc.Location = New System.Drawing.Point(112, 64)
        Me.txtDesc.Multiline = True
        Me.txtDesc.Name = "txtDesc"
        Me.txtDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDesc.Size = New System.Drawing.Size(189, 60)
        Me.txtDesc.TabIndex = 42
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.btnDelete)
        Me.GroupBox2.Controls.Add(Me.btnAdd)
        Me.GroupBox2.Controls.Add(Me.lstItems)
        Me.GroupBox2.Location = New System.Drawing.Point(3, 27)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(175, 523)
        Me.GroupBox2.TabIndex = 53
        Me.GroupBox2.TabStop = False
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(113, 480)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(51, 23)
        Me.btnDelete.TabIndex = 54
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(6, 480)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(51, 23)
        Me.btnAdd.TabIndex = 53
        Me.btnAdd.Text = "Add"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'lstItems
        '
        Me.lstItems.FormattingEnabled = True
        Me.lstItems.Location = New System.Drawing.Point(6, 15)
        Me.lstItems.Name = "lstItems"
        Me.lstItems.Size = New System.Drawing.Size(158, 459)
        Me.lstItems.TabIndex = 51
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(324, 248)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(73, 13)
        Me.Label6.TabIndex = 81
        Me.Label6.Text = "SUMMARY:"
        '
        'txtSummary
        '
        Me.txtSummary.BackColor = System.Drawing.SystemColors.Control
        Me.txtSummary.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtSummary.Enabled = False
        Me.txtSummary.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSummary.Location = New System.Drawing.Point(327, 265)
        Me.txtSummary.Multiline = True
        Me.txtSummary.Name = "txtSummary"
        Me.txtSummary.Size = New System.Drawing.Size(168, 158)
        Me.txtSummary.TabIndex = 82
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(699, 560)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.MenuStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Item Editor"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.picInventory, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picDrop, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Open1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Save1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LoadCharacterToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveAsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents QuitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtDesc As System.Windows.Forms.TextBox
    Friend WithEvents txtDropImageFilename As System.Windows.Forms.TextBox
    Friend WithEvents txtInventoryImageFilename As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents lstItems As System.Windows.Forms.ListBox
    Friend WithEvents btnLoadDropImage As System.Windows.Forms.Button
    Friend WithEvents picInventory As System.Windows.Forms.PictureBox
    Friend WithEvents picDrop As System.Windows.Forms.PictureBox
    Friend WithEvents btnLoadInvImage As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cboAttackNumDice As System.Windows.Forms.ComboBox
    Friend WithEvents cboAttackDie As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtDefense As System.Windows.Forms.TextBox
    Friend WithEvents txtValue As System.Windows.Forms.TextBox
    Friend WithEvents txtWeight As System.Windows.Forms.TextBox
    Friend WithEvents txtCHA As System.Windows.Forms.TextBox
    Friend WithEvents txtINT As System.Windows.Forms.TextBox
    Friend WithEvents txtSTA As System.Windows.Forms.TextBox
    Friend WithEvents txtDEX As System.Windows.Forms.TextBox
    Friend WithEvents txtSTR As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cboCategory As System.Windows.Forms.ComboBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtSummary As System.Windows.Forms.TextBox

End Class
