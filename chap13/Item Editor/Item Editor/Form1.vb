Imports System.Windows.Forms
Imports System.Xml
Imports System.Data

Public Class Form1
    Dim device As Graphics
    Dim surface As Bitmap
    Dim g_filename As String = "items.item"
    Dim currentIndex As Integer

    Private Sub Form1_Load(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles MyBase.Load
        surface = New Bitmap(Size.Width, Size.Height)
        picDrop.Image = surface
        device = Graphics.FromImage(surface)
        clearFields()
        loadFile(g_filename)
    End Sub

    Private Sub showItem(ByVal index As Integer)
        clearFields()

        Dim item As Item = lstItems.Items(index)
        txtName.Text = item.Name
        txtDesc.Text = item.Description
        txtDropImageFilename.Text = item.DropImageFilename
        txtInventoryImageFilename.Text = item.InvImageFilename
        cboCategory.Text = item.Category
        txtWeight.Text = item.Weight.ToString()
        txtValue.Text = item.Value.ToString()
        cboAttackNumDice.Text = item.AttackNumDice.ToString()
        cboAttackDie.Text = "D" + item.AttackDie.ToString()
        txtDefense.Text = item.Defense.ToString()
        txtSTR.Text = item.STR.ToString()
        txtDEX.Text = item.DEX.ToString()
        txtSTA.Text = item.STA.ToString()
        txtINT.Text = item.INT.ToString()
        txtCHA.Text = item.CHA.ToString()

        txtSummary.Text = item.Summary
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
            Dim list As XmlNodeList = doc.GetElementsByTagName("item")
            For Each node As XmlNode In list
                Dim element As XmlElement = node
                Dim item As New Item()
                item.Name = getElement("name", element)
                item.Description = getElement("description", element)
                item.DropImageFilename = getElement("dropimagefilename", element)
                item.InvImageFilename = getElement("invimagefilename", element)
                item.Category = getElement("category", element)
                item.Weight = Convert.ToSingle(getElement("weight", element))
                item.Value = Convert.ToSingle(getElement("value", element))
                item.AttackNumDice = Convert.ToInt32(getElement("attacknumdice", element))
                item.AttackDie = Convert.ToInt32(getElement("attackdie", element))
                item.Defense = Convert.ToInt32(getElement("defense", element))
                item.STR = Convert.ToInt32(getElement("STR", element))
                item.DEX = Convert.ToInt32(getElement("DEX", element))
                item.STA = Convert.ToInt32(getElement("STA", element))
                item.INT = Convert.ToInt32(getElement("INT", element))
                item.CHA = Convert.ToInt32(getElement("CHA", element))
                lstItems.Items.Add(item)
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return
        End Try

    End Sub

    Private Sub saveFile(ByVal filename As String)
        Try
            REM create data type templates
            Dim typeInt As System.Type
            Dim typeSingle As System.Type
            Dim typeStr As System.Type
            typeInt = System.Type.GetType("System.Int32")
            typeStr = System.Type.GetType("System.String")
            typeSingle = System.Type.GetType("System.Single")

            REM create xml schema
            Dim table As New DataTable("item")
            table.Columns.Add(New DataColumn("name", typeStr))
            table.Columns.Add(New DataColumn("description", typeStr))
            table.Columns.Add(New DataColumn("dropimagefilename", typeStr))
            table.Columns.Add(New DataColumn("invimagefilename", typeStr))
            table.Columns.Add(New DataColumn("category", typeStr))
            table.Columns.Add(New DataColumn("weight", typeSingle))
            table.Columns.Add(New DataColumn("value", typeSingle))
            table.Columns.Add(New DataColumn("attacknumdice", typeInt))
            table.Columns.Add(New DataColumn("attackdie", typeInt))
            table.Columns.Add(New DataColumn("defense", typeInt))
            table.Columns.Add(New DataColumn("STR", typeInt))
            table.Columns.Add(New DataColumn("DEX", typeInt))
            table.Columns.Add(New DataColumn("STA", typeInt))
            table.Columns.Add(New DataColumn("INT", typeInt))
            table.Columns.Add(New DataColumn("CHA", typeInt))

            REM copy character data into datatable
            For Each item As Item In lstItems.Items
                Dim row As DataRow = table.NewRow()
                row("name") = item.Name
                row("description") = item.Description
                row("dropimagefilename") = item.DropImageFilename
                row("invimagefilename") = item.InvImageFilename
                row("category") = item.Category
                row("weight") = item.Weight
                row("value") = item.Value
                row("attacknumdice") = item.AttackNumDice
                row("attackdie") = item.AttackDie
                row("defense") = item.Defense
                row("STR") = item.STR
                row("DEX") = item.DEX
                row("STA") = item.STA
                row("INT") = item.INT
                row("CHA") = item.CHA
                table.Rows.Add(row)
            Next

            REM save xml file
            table.WriteXml(filename)
            table.Dispose()

        Catch es As Exception
            MessageBox.Show(es.Message)
        End Try
    End Sub

    Private Sub clearFields()
        txtName.Text = ""
        txtDesc.Text = ""
        txtDropImageFilename.Text = ""
        txtInventoryImageFilename.Text = ""
        cboCategory.Text = ""
        txtWeight.Text = "0"
        txtValue.Text = "0"
        cboAttackNumDice.Text = "0"
        cboAttackDie.Text = "D0"
        txtDefense.Text = "0"
        txtSTR.Text = "0"
        txtDEX.Text = "0"
        txtSTA.Text = "0"
        txtINT.Text = "0"
        txtCHA.Text = "0"
    End Sub

    Private Sub saveCurrentItem()
        Dim item As Item

        If currentIndex < 0 Or txtName.Text = "" Then
            Return
        End If

        Try
            item = lstItems.Items(currentIndex)

            item.Name = txtName.Text
            item.Description = txtDesc.Text
            item.DropImageFilename = txtDropImageFilename.Text
            item.InvImageFilename = txtInventoryImageFilename.Text
            item.Category = cboCategory.Text
            item.Weight = Convert.ToSingle(txtWeight.Text)
            item.Value = Convert.ToSingle(txtValue.Text)
            item.AttackNumDice = Convert.ToInt32(cboAttackNumDice.Text)
            item.AttackDie = Convert.ToInt32(cboAttackDie.Text.Substring(1))
            item.Defense = Convert.ToInt32(txtDefense.Text)
            item.STR = Convert.ToInt32(txtSTR.Text)
            item.DEX = Convert.ToInt32(txtDEX.Text)
            item.STA = Convert.ToInt32(txtSTA.Text)
            item.INT = Convert.ToInt32(txtINT.Text)
            item.CHA = Convert.ToInt32(txtCHA.Text)

            lstItems.Items(currentIndex) = item
            'lstItems.Update()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles SaveAsToolStripMenuItem.Click
        Save1.DefaultExt = ".char"
        Save1.Filter = "Item Data Files|*.item"
        Save1.Title = "Save Item File"
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

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        saveCurrentItem()
        lstItems.Items.Add(New Item())
    End Sub

    Private Sub lstItems_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) _
            Handles lstItems.MouseClick
        If lstItems.SelectedIndex >= 0 Then
            saveCurrentItem()
            currentIndex = lstItems.SelectedIndex
            showItem(currentIndex)
        End If
    End Sub

    Private Sub lstItems_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) _
            Handles lstItems.SelectedIndexChanged
        If lstItems.SelectedIndex >= 0 Then

        End If
    End Sub

    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        saveCurrentItem()
        saveFile(g_filename)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        lstItems.Items.Clear()
        g_filename = ""
        clearFields()
    End Sub

    Private Sub LoadToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoadCharacterToolStripMenuItem.Click
        Open1.DefaultExt = ".item"
        Open1.Filter = "Item Data Files|*.item"
        Open1.Multiselect = False
        Open1.Title = "Load Item File"
        Open1.InitialDirectory = Environment.CurrentDirectory
        Dim result As DialogResult
        result = Open1.ShowDialog(Me)
        If result <> DialogResult.OK Then Return
        g_filename = Open1.FileName
        clearFields()
        loadFile(g_filename)
    End Sub

    Private Sub btnLoadDropImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadDropImage.Click
        Open1.DefaultExt = ".png"
        Open1.Filter = "Bitmap Files|*.bmp;*.png;*.jpg"
        Open1.Multiselect = False
        Open1.Title = "Load Bitmap File"
        Open1.InitialDirectory = Environment.CurrentDirectory
        Dim result As DialogResult
        result = Open1.ShowDialog(Me)
        If result <> DialogResult.OK Then Return
        txtDropImageFilename.Text = IO.Path.GetFileName(Open1.FileName)
    End Sub

    Private Sub btnLoadInvImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadInvImage.Click
        Open1.DefaultExt = ".png"
        Open1.Filter = "Bitmap Files|*.bmp;*.png;*.jpg"
        Open1.Multiselect = False
        Open1.Title = "Load Bitmap File"
        Open1.InitialDirectory = Environment.CurrentDirectory
        Dim result As DialogResult
        result = Open1.ShowDialog(Me)
        If result <> DialogResult.OK Then Return
        txtInventoryImageFilename.Text = IO.Path.GetFileName(Open1.FileName)
    End Sub

    Private Sub txtDropImageFilename_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDropImageFilename.TextChanged
        picDrop.Image = LoadBitmap(txtDropImageFilename.Text)
    End Sub

    Private Sub txtInventoryImageFilename_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtInventoryImageFilename.TextChanged
        picInventory.Image = LoadBitmap(txtInventoryImageFilename.Text)
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        MessageBox.Show("There is no item delete, just rename/recycle.")
    End Sub
End Class

