Imports System.Windows.Forms
Imports System.Xml
Imports System.Data

Public Class Form1
    Dim device As Graphics
    Dim surface As Bitmap
    Dim animationImage As Bitmap
    Dim sprite As Sprite
    Dim rand As Random
    Dim g_filename As String
    Dim currentAnim As String

    Private Sub Form1_Load(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles MyBase.Load
        surface = New Bitmap(Size.Width, Size.Height)
        PictureBox1.Image = surface
        device = Graphics.FromImage(surface)
        sprite = New Sprite(device)
        animationImage = Nothing
        rand = New Random()
    End Sub

    Public Function LoadBitmap(ByVal filename As String)
        Dim bmp As Bitmap
        Try
            bmp = New Bitmap(filename)
        Catch ex As Exception
            bmp = Nothing
        End Try
        Return bmp
    End Function

    Private Sub btnWalkFile_Click(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles btnWalkFile.Click
        Open1.DefaultExt = ".png"
        Open1.Filter = "Bitmap Files|*.bmp;*.png;*.jpg"
        Open1.Multiselect = False
        Open1.Title = "Load Bitmap File"
        Open1.InitialDirectory = Environment.CurrentDirectory
        Dim result As DialogResult
        result = Open1.ShowDialog(Me)
        If result <> DialogResult.OK Then Return
        txtWalkFile.Text = IO.Path.GetFileName(Open1.FileName)
        animationImage = LoadBitmap(txtWalkFile.Text)
    End Sub

    Private Sub btnAttackFile_Click(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles btnAttackFile.Click
        Open1.DefaultExt = ".png"
        Open1.Filter = "Bitmap Files|*.bmp;*.png;*.jpg"
        Open1.Multiselect = False
        Open1.Title = "Load Bitmap File"
        Open1.InitialDirectory = Environment.CurrentDirectory
        Dim result As DialogResult
        result = Open1.ShowDialog(Me)
        If result <> DialogResult.OK Then Return
        txtAttackFile.Text = IO.Path.GetFileName(Open1.FileName)
        animationImage = LoadBitmap(txtAttackFile.Text)
    End Sub

    Private Sub btnDieFile_Click(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles btnDieFile.Click
        Open1.DefaultExt = ".png"
        Open1.Filter = "Bitmap Files|*.bmp;*.png;*.jpg"
        Open1.Multiselect = False
        Open1.Title = "Load Bitmap File"
        Open1.InitialDirectory = Environment.CurrentDirectory
        Dim result As DialogResult
        result = Open1.ShowDialog(Me)
        If result <> DialogResult.OK Then Return
        txtDieFile.Text = IO.Path.GetFileName(Open1.FileName)
        animationImage = LoadBitmap(txtDieFile.Text)
    End Sub


    Private Sub Timer1_Tick(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles Timer1.Tick
        If currentAnim = "walk" Then
            sprite.Width = numWidth.Value
            sprite.Height = numHeight.Value
            sprite.Columns = numColumns.Value
        ElseIf currentAnim = "attack" Then
            sprite.Width = numWidth2.Value
            sprite.Height = numHeight2.Value
            sprite.Columns = numColumns2.Value
        ElseIf currentAnim = "die" Then
            sprite.Width = numWidth3.Value
            sprite.Height = numHeight3.Value
            sprite.Columns = numColumns3.Value
        End If
        sprite.TotalFrames = sprite.Columns * 8
        sprite.Image = animationImage
        sprite.Animate(0, sprite.TotalFrames - 1)
        device.Clear(Color.DarkGray)
        sprite.Draw()
        PictureBox1.Image = surface
    End Sub

    Private Sub btnRoll_Click(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles btnRoll.Click
        txtRollStr.Text = rand.Next(1, 6 * numDCount.Value).ToString()
        txtRollDex.Text = rand.Next(1, 6 * numDCount.Value).ToString()
        txtRollSta.Text = rand.Next(1, 6 * numDCount.Value).ToString()
        txtRollInt.Text = rand.Next(1, 6 * numDCount.Value).ToString()
        txtRollCha.Text = rand.Next(1, 6 * numDCount.Value).ToString()
    End Sub

    Private Function getElement(ByVal field As String, ByRef element As XmlElement) As String
        Dim value As String = ""
        Try
            value = element.GetElementsByTagName(field)(0).InnerText
        Catch ex As Exception
            REM ignore error, just return empty
            Console.WriteLine(ex.Message)
        End Try
        Return value
    End Function

    Private Sub loadFile(ByVal filename As String)
        Try
            REM open the xml file 
            Dim doc As New XmlDocument()
            doc.Load(filename)
            Dim list As XmlNodeList = doc.GetElementsByTagName("character")
            Dim element As XmlElement = list(0)

            clearFields()

            REM read data fields
            txtName.Text = getElement("name", element)
            cboClass.Text = getElement("class", element)
            cboRace.Text = getElement("race", element)
            txtDesc.Text = getElement("desc", element)
            txtStr.Text = getElement("str", element)
            txtDex.Text = getElement("dex", element)
            txtSta.Text = getElement("sta", element)
            txtInt.Text = getElement("int", element)
            txtCha.Text = getElement("cha", element)
            txtHP.Text = getElement("hitpoints", element)
            txtWalkFile.Text = getElement("anim_walk_filename", element)

            numWidth.Value = Convert.ToInt32( _
                getElement("anim_walk_width", element))

            numHeight.Value = Convert.ToInt32( _
                getElement("anim_walk_height", element))

            numColumns.Value = Convert.ToInt32( _
                getElement("anim_walk_columns", element))

            txtAttackFile.Text = getElement( _
                "anim_attack_filename", element)

            numWidth2.Value = Convert.ToInt32( _
                getElement("anim_attack_width", element))

            numHeight2.Value = Convert.ToInt32( _
                getElement("anim_attack_height", element))

            numColumns2.Value = Convert.ToInt32( _
                getElement("anim_attack_columns", element))

            txtDieFile.Text = getElement( _
                "anim_die_filename", element)

            numWidth3.Value = Convert.ToInt32( _
                getElement("anim_die_width", element))

            numHeight3.Value = Convert.ToInt32( _
                getElement("anim_die_height", element))

            numColumns3.Value = Convert.ToInt32( _
                getElement("anim_die_columns", element))

            numGold1.Value = Convert.ToInt32( _
                getElement("dropgold1", element))

            numGold2.Value = Convert.ToInt32( _
                getElement("dropgold2", element))

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub saveFile(ByVal filename As String)
        Try
            REM create xml schema
            Dim table As New DataTable("character")
            table.Columns.Add(New DataColumn("name", _
                System.Type.GetType("System.String")))

            table.Columns.Add(New DataColumn("class", _
                System.Type.GetType("System.String")))

            table.Columns.Add(New DataColumn("race", _
                System.Type.GetType("System.String")))

            table.Columns.Add(New DataColumn("desc", _
                System.Type.GetType("System.String")))

            table.Columns.Add(New DataColumn("str", _
                System.Type.GetType("System.Int32")))

            table.Columns.Add(New DataColumn("dex", _
                System.Type.GetType("System.Int32")))

            table.Columns.Add(New DataColumn("sta", _
                System.Type.GetType("System.Int32")))

            table.Columns.Add(New DataColumn("int", _
                System.Type.GetType("System.Int32")))

            table.Columns.Add(New DataColumn("cha", _
                System.Type.GetType("System.Int32")))

            table.Columns.Add(New DataColumn("hitpoints", _
                System.Type.GetType("System.Int32")))

            table.Columns.Add(New DataColumn("anim_walk_filename", _
                System.Type.GetType("System.String")))

            table.Columns.Add(New DataColumn("anim_walk_width", _
                System.Type.GetType("System.Int32")))

            table.Columns.Add(New DataColumn("anim_walk_height", _
                System.Type.GetType("System.Int32")))

            table.Columns.Add(New DataColumn("anim_walk_columns", _
                System.Type.GetType("System.Int32")))

            table.Columns.Add(New DataColumn("anim_attack_filename", _
                System.Type.GetType("System.String")))

            table.Columns.Add(New DataColumn("anim_attack_width", _
                System.Type.GetType("System.Int32")))

            table.Columns.Add(New DataColumn("anim_attack_height", _
                System.Type.GetType("System.Int32")))

            table.Columns.Add(New DataColumn("anim_attack_columns", _
                System.Type.GetType("System.Int32")))

            table.Columns.Add(New DataColumn("anim_die_filename", _
                System.Type.GetType("System.String")))

            table.Columns.Add(New DataColumn("anim_die_width", _
                System.Type.GetType("System.Int32")))

            table.Columns.Add(New DataColumn("anim_die_height", _
                System.Type.GetType("System.Int32")))

            table.Columns.Add(New DataColumn("anim_die_columns", _
                System.Type.GetType("System.Int32")))

            table.Columns.Add(New DataColumn("dropgold1", _
                System.Type.GetType("System.Int32")))

            table.Columns.Add(New DataColumn("dropgold2", _
                System.Type.GetType("System.Int32")))

            REM copy character data into datatable
            Dim row As DataRow = table.NewRow()
            row("name") = txtName.Text
            row("class") = cboClass.Text
            row("race") = cboRace.Text
            row("desc") = txtDesc.Text
            row("str") = txtStr.Text
            row("dex") = txtDex.Text
            row("sta") = txtSta.Text
            row("int") = txtInt.Text
            row("cha") = txtCha.Text
            row("hitpoints") = txtHP.Text
            row("anim_walk_filename") = txtWalkFile.Text
            row("anim_walk_width") = numWidth.Value
            row("anim_walk_height") = numHeight.Value
            row("anim_walk_columns") = numColumns.Value
            row("anim_attack_filename") = txtAttackFile.Text
            row("anim_attack_width") = numWidth2.Value
            row("anim_attack_height") = numHeight2.Value
            row("anim_attack_columns") = numColumns2.Value
            row("anim_die_filename") = txtDieFile.Text
            row("anim_die_width") = numWidth3.Value
            row("anim_die_height") = numHeight3.Value
            row("anim_die_columns") = numColumns3.Value
            row("dropgold1") = numGold1.Value
            row("dropgold2") = numGold2.Value
            table.Rows.Add(row)

            REM save xml file
            table.WriteXml(filename)
            table.Dispose()

        Catch es As Exception
            MessageBox.Show(es.Message)
        End Try
    End Sub

    Private Sub clearFields()
        g_filename = ""
        txtName.Text = ""
        cboClass.Text = ""
        cboRace.Text = ""
        txtDesc.Text = ""
        txtStr.Text = "0"
        txtDex.Text = "0"
        txtSta.Text = "0"
        txtInt.Text = "0"
        txtCha.Text = "0"
        txtHP.Text = "0"
        txtRollStr.Text = "0"
        txtRollDex.Text = "0"
        txtRollSta.Text = "0"
        txtRollInt.Text = "0"
        txtRollCha.Text = "0"
        txtModStr.Text = "0"
        txtModDex.Text = "0"
        txtModSta.Text = "0"
        txtModInt.Text = "0"
        txtModCha.Text = "0"
        txtWalkFile.Text = ""
        numWidth.Value = 96
        numHeight.Value = 96
        numColumns.Value = 8
        txtAttackFile.Text = ""
        numWidth2.Value = 96
        numHeight2.Value = 96
        numColumns2.Value = 8
        txtDieFile.Text = ""
        numWidth3.Value = 96
        numHeight3.Value = 96
        numColumns3.Value = 8
        numGold1.Value = 0
        numGold2.Value = 0
    End Sub

    Private Sub NewCharacterToolStripMenuItem_Click( _
            ByVal sender As System.Object, ByVal e As System.EventArgs) _
            Handles NewCharacterToolStripMenuItem.Click
        clearFields()
    End Sub

    Private Sub LoadCharacterToolStripMenuItem_Click( _
            ByVal sender As System.Object, ByVal e As System.EventArgs) _
            Handles LoadCharacterToolStripMenuItem.Click
        Open1.DefaultExt = ".char"
        Open1.Filter = "Character Data Files|*.char"
        Open1.Multiselect = False
        Open1.Title = "Load Character File"
        Open1.InitialDirectory = Environment.CurrentDirectory
        Dim result As DialogResult
        result = Open1.ShowDialog(Me)
        If result <> DialogResult.OK Then Return
        g_filename = Open1.FileName
        loadFile(g_filename)
    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles SaveAsToolStripMenuItem.Click
        Save1.DefaultExt = ".char"
        Save1.Filter = "Character Data Files|*.char"
        Save1.Title = "Save Character File"
        Save1.InitialDirectory = Environment.CurrentDirectory
        Dim result As DialogResult
        result = Save1.ShowDialog(Me)
        If result <> Windows.Forms.DialogResult.OK Then Return
        g_filename = Save1.FileName
        saveFile(g_filename)
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        If g_filename = "" Then
            SaveAsToolStripMenuItem_Click(Nothing, Nothing)
        Else
            saveFile(g_filename)
        End If
    End Sub

    Private Sub btnRollHP_Click(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles btnRollHP.Click
        Try
            Dim stamina As Integer = Convert.ToInt32(txtSta.Text)
            Dim dice As Integer = numRollHP.Value
            Dim hp As Integer = stamina + rand.Next(1, 8)
            txtHP.Text = hp.ToString()
        Catch ex As Exception
            txtHP.Text = "0"
            Return
        End Try
    End Sub

    Private Sub btnAnimate3_Click(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles btnAnimate3.Click
        currentAnim = "die"
        animationImage = LoadBitmap(txtDieFile.Text)
        Timer1.Interval = numRate.Value
        Timer1.Enabled = Not Timer1.Enabled
    End Sub

    Private Sub btnAnimate2_Click(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles btnAnimate2.Click
        currentAnim = "attack"
        animationImage = LoadBitmap(txtAttackFile.Text)
        Timer1.Interval = numRate.Value
        Timer1.Enabled = Not Timer1.Enabled
    End Sub

    Private Sub btnAnimate_Click(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles btnAnimate.Click
        currentAnim = "walk"
        animationImage = LoadBitmap(txtWalkFile.Text)
        Timer1.Interval = numRate.Value
        Timer1.Enabled = Not Timer1.Enabled
    End Sub

    Private Sub numRate_ValueChanged(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles numRate.ValueChanged
        Timer1.Interval = numRate.Value
    End Sub

    Private Sub cboClass_SelectedIndexChanged( _
            ByVal sender As System.Object, ByVal e As System.EventArgs) _
            Handles cboClass.SelectedIndexChanged
        Dim cls As String = cboClass.Text.ToLower()
        If cls = "warrior" Then
            txtModStr.Text = "8"
            txtModDex.Text = "3"
            txtModSta.Text = "4"
            txtModInt.Text = "0"
            txtModCha.Text = "0"
        ElseIf cls = "paladin" Then
            txtModStr.Text = "3"
            txtModDex.Text = "3"
            txtModSta.Text = "8"
            txtModInt.Text = "0"
            txtModCha.Text = "1"
        ElseIf cls = "hunter" Then
            txtModStr.Text = "2"
            txtModDex.Text = "8"
            txtModSta.Text = "4"
            txtModInt.Text = "0"
            txtModCha.Text = "1"
        ElseIf cls = "priest" Then
            txtModStr.Text = "0"
            txtModDex.Text = "6"
            txtModSta.Text = "1"
            txtModInt.Text = "8"
            txtModCha.Text = "0"
        End If
    End Sub

    Private Sub txtStr_TextChanged(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles txtStr.TextChanged
        If Convert.ToInt32(txtStr.Text) < 0 Then
            txtStr.Text = "0"
        End If
    End Sub

    Private Sub txtDex_TextChanged(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles txtDex.TextChanged
        If Convert.ToInt32(txtDex.Text) < 0 Then
            txtDex.Text = "0"
        End If
    End Sub

    Private Sub txtSta_TextChanged(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles txtSta.TextChanged
        If Convert.ToInt32(txtSta.Text) < 0 Then
            txtSta.Text = "0"
        End If
    End Sub

    Private Sub txtInt_TextChanged(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles txtInt.TextChanged
        If Convert.ToInt32(txtInt.Text) < 0 Then
            txtInt.Text = "0"
        End If
    End Sub

    Private Sub txtCha_TextChanged(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles txtCha.TextChanged
        If Convert.ToInt32(txtCha.Text) < 0 Then
            txtCha.Text = "0"
        End If
    End Sub

    Private Sub txtRollStr_TextChanged(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles txtRollStr.TextChanged
        Try
            If txtRollStr.Text = "" Then txtRollStr.Text = "0"
            If txtModStr.Text = "" Then txtModStr.Text = "0"
            Dim str As Integer = Convert.ToInt32(txtRollStr.Text)
            str += Convert.ToInt32(txtModStr.Text)
            txtStr.Text = str.ToString()
        Catch ex As Exception
            txtStr.Text = "0"
            Return
        End Try
    End Sub

    Private Sub txtRollDex_TextChanged(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles txtRollDex.TextChanged
        Try
            If txtRollDex.Text = "" Then txtRollDex.Text = "0"
            If txtModDex.Text = "" Then txtModDex.Text = "0"
            Dim s As Integer = Convert.ToInt32(txtRollDex.Text)
            s += Convert.ToInt32(txtModDex.Text)
            txtDex.Text = s.ToString()
        Catch ex As Exception
            txtDex.Text = "0"
            Return
        End Try
    End Sub

    Private Sub txtRollSta_TextChanged(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles txtRollSta.TextChanged
        Try
            If txtRollSta.Text = "" Then txtRollSta.Text = "0"
            If txtModSta.Text = "" Then txtModSta.Text = "0"
            Dim s As Integer = Convert.ToInt32(txtRollSta.Text)
            s += Convert.ToInt32(txtModSta.Text)
            txtSta.Text = s.ToString()
        Catch ex As Exception
            txtSta.Text = "0"
            Return
        End Try
    End Sub

    Private Sub txtRollInt_TextChanged(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles txtRollInt.TextChanged
        Try
            If txtRollInt.Text = "" Then txtRollInt.Text = "0"
            If txtModInt.Text = "" Then txtModInt.Text = "0"
            Dim s As Integer = Convert.ToInt32(txtRollInt.Text)
            s += Convert.ToInt32(txtModInt.Text)
            txtInt.Text = s.ToString()
        Catch ex As Exception
            txtInt.Text = "0"
            Return
        End Try
    End Sub

    Private Sub txtRollCha_TextChanged(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles txtRollCha.TextChanged
        Try
            If txtRollCha.Text = "" Then txtRollCha.Text = "0"
            If txtModCha.Text = "" Then txtModCha.Text = "0"
            Dim s As Integer = Convert.ToInt32(txtRollCha.Text)
            s += Convert.ToInt32(txtModCha.Text)
            txtCha.Text = s.ToString()
        Catch ex As Exception
            txtCha.Text = "0"
            Return
        End Try
    End Sub

    Private Sub txtModStr_TextChanged(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles txtModStr.TextChanged
        txtRollStr_TextChanged(Nothing, Nothing)
    End Sub

    Private Sub txtModDex_TextChanged(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles txtModDex.TextChanged
        txtRollDex_TextChanged(Nothing, Nothing)
    End Sub

    Private Sub txtModSta_TextChanged(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles txtModSta.TextChanged
        txtRollSta_TextChanged(Nothing, Nothing)
    End Sub

    Private Sub txtModInt_TextChanged(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles txtModInt.TextChanged
        txtRollInt_TextChanged(Nothing, Nothing)
    End Sub

    Private Sub txtModCha_TextChanged(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles txtModCha.TextChanged
        txtRollCha_TextChanged(Nothing, Nothing)
    End Sub
End Class

