#region "using"
using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
#endregion

    
public struct tilemapStruct
{
    public int tilenum;
    public string data1;
    public string data2;
    public string data3;
    public string data4;
    public bool collidable;
    public bool portal;
    public int portalx;
    public int portaly;
    public string portalfile;
}

public struct selectionStruct
{
    public int index;
    public int oldIndex;
    public int x, y;
}

public partial class Form1 : Form,IMessageFilter 
{
    const int paletteColumns = 5;
    const int mapSize = 128;

    Bitmap drawArea;
    int mousex, mousey;
    Graphics gfx;
    int gridx, gridy;
    Bitmap selectedBitmap;
    Graphics gfxSelected;
    Font fontArial;
    string g_filename = "";
    
    tilemapStruct[] tilemap;
    int paletteIndex = 0;
    int selectedPaletteTile = 0;

    selectionStruct selectedTile;


    private void Form1_FormClosed(object sender, FormClosedEventArgs e)
    {
        gfx.Dispose();
        drawArea.Dispose();
        gfxSelected.Dispose();
        selectedBitmap.Dispose();
    }

    public Form1()
    {
        InitializeComponent();
        Application.AddMessageFilter(this);

        //create tilemap
        tilemap = new tilemapStruct[mapSize*mapSize];
        
        //set up level drawing surface
        drawArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
        pictureBox1.Image = drawArea;
        gfx = Graphics.FromImage(drawArea);

        //selected image
        selectedBitmap = new Bitmap(picSelected.Size.Width, picSelected.Size.Height);
        picSelected.Image = selectedBitmap;
        gfxSelected = Graphics.FromImage(selectedBitmap);

        //create font
        fontArial = new Font("Arial Narrow", 8);
    }


    #region mouse wheel scrolling support 
    //adds mouse wheel support to scrollable controls
    // P/Invoke declarations
    [DllImport("user32.dll")]
    private static extern IntPtr WindowFromPoint(Point pt);
    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
    public bool PreFilterMessage(ref Message m)
    {
        if (m.Msg == 0x20a)
        {
            // WM_MOUSEWHEEL, find the control at screen position m.LParam
            Point pos = new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16);
            IntPtr hWnd = WindowFromPoint(pos);
            if (hWnd != IntPtr.Zero && hWnd != m.HWnd && Control.FromHandle(hWnd) != null)
            {
                SendMessage(hWnd, m.Msg, m.WParam, m.LParam);
                return true;
            }
        }
        return false;
    }
    #endregion


    public void setSelectedTile()
    {
        int sx = (selectedPaletteTile % paletteColumns) * 33;
        int sy = (selectedPaletteTile / paletteColumns) * 33;
        Rectangle src = new Rectangle(sx, sy, 32, 32);
        Rectangle dst = new Rectangle(0, 0, 32, 32);
        gfxSelected.DrawImage(picPalette.Image, dst, src, GraphicsUnit.Pixel);
        picSelected.Image = selectedBitmap;
    }

    public void drawTileNumber(int x, int y, int tile)
    {
        //save tilemap data
        tilemap[y * mapSize + x].tilenum = tile;

        //draw tile
        int sx = (tile % paletteColumns) * 33;
        int sy = (tile / paletteColumns) * 33;
        int dx = x * 32;
        int dy = y * 32;
        Rectangle src = new Rectangle(sx, sy, 32, 32);
        Rectangle dst = new Rectangle(dx, dy, 32, 32);
        gfx.DrawImage(picPalette.Image, dst, src, GraphicsUnit.Pixel);

        //print tilenum
        if (menuViewShowTileNum.Checked)
        {
            if (tile > 0)
                gfx.DrawString(tile.ToString(), fontArial, Brushes.White, x * 32, y * 32);
        }

        //print data value
        if (showDataToolStripMenuItem.Checked)
        {
            string data = tilemap[y * mapSize + x].data1;
            if (data != null)
            {
                if (data.Length > 5)
                    data = data.Substring(0, 5);
                gfx.DrawString(data, fontArial, Brushes.White, x * 32 + 1, y * 32 + 12);
            }
        }

        //print portal state
        if (showPortalsToolStripMenuItem.Checked)
        {
            bool portal = tilemap[y * mapSize + x].portal;
            if (portal)
                gfx.DrawString("P", fontArial, Brushes.White, x * 32+4, y * 32);
        }

        //print collidable state
        if (showCollidableToolStripMenuItem.Checked)
        {
            bool collidable = tilemap[y * mapSize + x].collidable;
            if (collidable)
                gfx.DrawString("C", fontArial, Brushes.White, x * 32 + 18, y * 32);
        }

        
        //save changes
        pictureBox1.Image = drawArea;
    }

    public void redrawTilemap()
    {
        for (int index = 0; index < mapSize * mapSize; index++)
        {
            int value = tilemap[index].tilenum;
            int x = index % mapSize;
            int y = index / mapSize;
            drawTileNumber(x, y, value);
        }
    }

    public void drawSelectedTile()
    {
        drawTileNumber(gridx, gridy, selectedPaletteTile);
    }

    public void hideSelectionBox()
    {
        //erase old selection box
        int oldx = selectedTile.oldIndex % mapSize;
        int oldy = selectedTile.oldIndex / mapSize;
        drawTileNumber(oldx, oldy, tilemap[selectedTile.oldIndex].tilenum);

    }

    public void drawSelectionBox(int gridx, int gridy)
    {
        hideSelectionBox();

        //remember current tile
        selectedTile.oldIndex = selectedTile.index;

        //draw selection box around tile
        int dx = gridx * 32;
        int dy = gridy * 32;
        Pen pen = new Pen(Color.DarkMagenta, 2);
        Rectangle rect = new Rectangle(dx + 1, dy + 1, 30, 30);
        gfx.DrawRectangle(pen, rect);

        //save changes
        pictureBox1.Image = drawArea;
    }

    private void clickDrawArea(MouseEventArgs e)
    {
        switch (e.Button)
        {
            case MouseButtons.Left:
                if (radioDrawMode.Checked)
                {
                    drawSelectedTile();
                }
                else
                {
                    //show selected tile # for editing
                    selectedTile.x = gridx;
                    selectedTile.y = gridy;
                    selectedTile.index = gridy * mapSize + gridx;
                    txtTileNum.Text = tilemap[selectedTile.index].tilenum.ToString();
                    txtData1.Text = tilemap[selectedTile.index].data1;
                    txtData2.Text = tilemap[selectedTile.index].data2;
                    txtData3.Text = tilemap[selectedTile.index].data3;
                    txtData4.Text = tilemap[selectedTile.index].data4;
                    chkCollidable.Checked = tilemap[selectedTile.index].collidable;
                    chkPortal.Checked = tilemap[selectedTile.index].portal;
                    txtPortalX.Text = tilemap[selectedTile.index].portalx.ToString();
                    txtPortalY.Text = tilemap[selectedTile.index].portaly.ToString();
                    txtPortalFile.Text = tilemap[selectedTile.index].portalfile;

                    //draw selection box
                    drawSelectionBox(gridx, gridy);

                }
                break;

            case MouseButtons.Right:
                if (radioDrawMode.Checked)
                    drawTileNumber(gridx, gridy, 0); //erase
                break;
        }
    }

    private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
    {
        clickDrawArea(e);
    }

    private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
    {
        gridx = e.X / 32;
        gridy = e.Y / 32;
        mousex = e.X;
        mousey = e.Y;
        lblMouseInfo.Text = "CURSOR " + e.X.ToString() + "," + e.Y.ToString() +
            " - GRID " + gridx.ToString() + "," + gridy.ToString();

        if (radioDrawMode.Checked)
        {
            int index = gridy * mapSize + gridx;
            txtTileNum.Text = tilemap[index].tilenum.ToString();
            txtData1.Text = tilemap[index].data1;
            txtData2.Text = tilemap[index].data2;
            txtData3.Text = tilemap[index].data3;
            txtData4.Text = tilemap[index].data4;
            chkCollidable.Checked = tilemap[index].collidable;
            chkPortal.Checked = tilemap[index].portal;
            txtPortalX.Text = tilemap[index].portalx.ToString();
            txtPortalY.Text = tilemap[index].portaly.ToString();
            txtPortalFile.Text = tilemap[index].portalfile;
        }

        clickDrawArea(e);
    }

    private void Form1_Load(object sender, EventArgs e){}

    private void palette_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.X < paletteColumns * 33)
        {
            gridx = e.X / 33;
            gridy = e.Y / 33;
            paletteIndex = gridy * paletteColumns + gridx;
            lblTileInfo.Text = "TILE #" + paletteIndex + " : " + gridx.ToString() + "," + gridy.ToString();
            lblSelected.Text = "SELECTED: " + selectedPaletteTile.ToString();
        }
    }
    private void palette_MouseClick(object sender, MouseEventArgs e)
    {
        if (e.X < paletteColumns * 33)
        {
            gridx = e.X / 33;
            gridy = e.Y / 33;

            switch (e.Button)
            {
                case MouseButtons.Left:
                    selectedPaletteTile = gridy * paletteColumns + gridx;
                    setSelectedTile();
                    break;
            }
        }
    }

    private void Form1_KeyUp(object sender, KeyEventArgs e) 
    {
        switch (e.KeyCode)
        {
            case Keys.Space:
                if (radioEditMode.Checked)
                {
                    chkCollidable.Checked = !chkCollidable.Checked;
                }
                break;
        }
    }

    private void Form1_KeyDown(object sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.Escape:
                //Close();
                break;
        }
    }

    //helper function for loadTilemapFile
    private string getElement(string field, ref XmlElement element)
    {
        string value = "";
        try
        {
            value = element.GetElementsByTagName(field)[0].InnerText;
        }
        catch (Exception e)
        {
            //ignore error, just return empty
            Console.WriteLine(e.Message);
        }
        return value;
    }

    private void loadTilemapFile()
    {
        //display the open file dialog
        openFileDialog1.DefaultExt = ".level";
        openFileDialog1.Filter = "Tilemap Files|*.level";
        openFileDialog1.Multiselect = false;
        openFileDialog1.Title = "Load Level File";
        openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
        DialogResult result = openFileDialog1.ShowDialog();
        if (result != DialogResult.OK) return;
        g_filename = openFileDialog1.SafeFileName;

        this.Cursor = Cursors.WaitCursor;

        try
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(g_filename);
            XmlNodeList list = doc.GetElementsByTagName("tiles");
            foreach (XmlNode node in list)
            {
                XmlElement element = (XmlElement)node;

                //read data fields
                int index = Convert.ToInt32(getElement("tile",ref element));
                tilemap[index].tilenum = Convert.ToInt32(getElement("value", ref element));
                tilemap[index].data1 = getElement("data1", ref element);
                tilemap[index].data2 = getElement("data2", ref element);
                tilemap[index].data3 = getElement("data3", ref element);
                tilemap[index].data4 = getElement("data4", ref element);
                tilemap[index].collidable = Convert.ToBoolean(getElement("collidable", ref element));
                tilemap[index].portal = Convert.ToBoolean(getElement("portal", ref element));
                tilemap[index].portalx = Convert.ToInt32(getElement("portalx", ref element));
                tilemap[index].portaly = Convert.ToInt32(getElement("portaly", ref element));
                tilemap[index].portalfile = getElement("portalfile", ref element);
            }
            redrawTilemap();
        }
        catch (Exception es)
        {
            MessageBox.Show(es.Message);
        }

        this.Cursor = Cursors.Arrow;
    }

    private void saveTilemapFileAs()
    {
        //display the open file dialog
        saveFileDialog1.DefaultExt = ".level";
        saveFileDialog1.Filter = "Tilemap Files|*.level";
        saveFileDialog1.Title = "Save Level File";
        saveFileDialog1.AddExtension = true;
        saveFileDialog1.OverwritePrompt = true;
        saveFileDialog1.InitialDirectory = Environment.CurrentDirectory;
        DialogResult result = saveFileDialog1.ShowDialog();
        if (result != DialogResult.OK) return;
        if (saveFileDialog1.FileName.Length == 0) return;
        g_filename = saveFileDialog1.FileName;
        saveTilemapFile();
    }

    private void saveTilemapFile()
    {
        if (g_filename.Length == 0)
        {
            saveTilemapFileAs();
            return;
        }

        this.Cursor = Cursors.WaitCursor;

        try
        {
            System.Data.DataSet ds;
            ds = new DataSet();

            //create xml schema
            System.Data.DataTable table;
            table = new DataTable("tiles");

            //add an autoincrement column
            DataColumn column1 = new DataColumn();
            column1.DataType = System.Type.GetType("System.Int32");
            column1.ColumnName = "tile";
            column1.AutoIncrement = true;
            table.Columns.Add(column1);

            //add index key
            DataColumn[] keys = new DataColumn[1];
            keys[0] = column1;
            table.PrimaryKey = keys;
            
            //tilemap data columns
            DataColumn column2 = new DataColumn();
            column2.DataType = System.Type.GetType("System.Int32");
            column2.ColumnName = "value";
            table.Columns.Add(column2);

            DataColumn data1 = new DataColumn();
            data1.DataType = System.Type.GetType("System.String");
            data1.ColumnName = "data1";
            table.Columns.Add(data1);

            DataColumn data2 = new DataColumn();
            data2.DataType = System.Type.GetType("System.String");
            data2.ColumnName = "data2";
            table.Columns.Add(data2);

            DataColumn data3 = new DataColumn();
            data3.DataType = System.Type.GetType("System.String");
            data3.ColumnName = "data3";
            table.Columns.Add(data3);

            DataColumn data4 = new DataColumn();
            data4.DataType = System.Type.GetType("System.String");
            data4.ColumnName = "data4";
            table.Columns.Add(data4);

            DataColumn column4 = new DataColumn();
            column4.DataType = System.Type.GetType("System.Boolean");
            column4.ColumnName = "collidable";
            table.Columns.Add(column4);

            DataColumn portal = new DataColumn();
            portal.DataType = System.Type.GetType("System.Boolean");
            portal.ColumnName = "portal";
            table.Columns.Add(portal);

            DataColumn portalx = new DataColumn();
            portalx.DataType = System.Type.GetType("System.Int32");
            portalx.ColumnName = "portalx";
            table.Columns.Add(portalx);

            DataColumn portaly = new DataColumn();
            portaly.DataType = System.Type.GetType("System.Int32");
            portaly.ColumnName = "portaly";
            table.Columns.Add(portaly);

            DataColumn portalfile = new DataColumn();
            portalfile.DataType = System.Type.GetType("System.String");
            portalfile.ColumnName = "portalfile";
            table.Columns.Add(portalfile);

            //copy tilemap array into datatable
            int index = 0;
            for (int n=0; n<mapSize*mapSize; n++)
            {
                DataRow row = table.NewRow();
                row["value"] = tilemap[index].tilenum;
                row["data1"] = tilemap[index].data1;
                row["data2"] = tilemap[index].data2;
                row["data3"] = tilemap[index].data3;
                row["data4"] = tilemap[index].data4;
                row["collidable"] = tilemap[index].collidable;
                row["portal"] = tilemap[index].portal;
                row["portalx"] = tilemap[index].portalx;
                row["portaly"] = tilemap[index].portaly;
                row["portalfile"] = tilemap[index].portalfile;
                table.Rows.Add(row);
                index++;
            }

            //save xml file
            table.WriteXml( g_filename );

            ds.Dispose();
            table.Dispose();
        }
        catch (Exception es)
        {
            MessageBox.Show(es.Message);
        }

        this.Cursor = Cursors.Arrow;
    }


    private void loadTilemapToolStripMenuItem1_Click(object sender, EventArgs e)
    {
        loadTilemapFile();
    }

    private void newTilemapToolStripMenuItem_Click(object sender, EventArgs e)
    {
        DialogResult result =
            MessageBox.Show("Save current map?", "Confirm", MessageBoxButtons.YesNoCancel);
        if (result == DialogResult.Cancel)
            return;
        else if (result == DialogResult.Yes)
            saveTilemapFile();

        //clear tilemap 
        for (int index = 0; index < mapSize * mapSize; index++)
        {
            tilemap[index].tilenum = 0;
            tilemap[index].data1 = "";
            tilemap[index].data2 = "";
            tilemap[index].data3 = "";
            tilemap[index].data4 = "";
            tilemap[index].collidable = false;
            tilemap[index].portal = false;
            tilemap[index].portalx = 0;
            tilemap[index].portaly = 0;
            tilemap[index].portalfile = "";
        }

        //refresh
        redrawTilemap();
    }

    private void saveLevelBMP4096x4096ToolStripMenuItem1_Click(object sender, EventArgs e)
    {
        bool ischecked = menuViewShowTileNumbers.Checked;
        if (ischecked)
        {
            menuViewShowTileNumbers.Checked = false;
            redrawTilemap();
        }

        //save the entire level as one huge bitmap
        drawArea.Save("level.bmp", System.Drawing.Imaging.ImageFormat.Bmp);

        if (ischecked)
        {
            menuViewShowTileNumbers.Checked = true;
            redrawTilemap();
        }
    }

    private void saveTilemapToolStripMenuItem_Click(object sender, EventArgs e)
    {
        saveTilemapFile();
    }

    private void toolStripMenuItem3_Click(object sender, EventArgs e)
    {
        saveTilemapFileAs();
    }

    private void savePaletteBMP512x512ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        //save formatted palette
        Graphics gfxSave;
        Bitmap bitmap;

        bitmap = new Bitmap(512, 512);
        gfxSave = Graphics.FromImage(bitmap);

        int tilenum = 0;
        for (int y = 0; y < 140 / paletteColumns; y++)
        {
            for (int x = 0; x < 512 / 32; x++)
            {
                int sx = (tilenum % paletteColumns) * 33;
                int sy = (tilenum / paletteColumns) * 33;
                Rectangle src = new Rectangle(sx, sy, 32, 32);
                Rectangle dst = new Rectangle(x * 32, y * 32, 32, 32);
                gfxSave.DrawImage(picPalette.Image, dst, src, GraphicsUnit.Pixel);
                tilenum++;
            }
        }

        //save tilemap palette
        bitmap.Save("tilemap.bmp", System.Drawing.Imaging.ImageFormat.Bmp);

        //clean up
        gfxSave.Dispose();
        bitmap.Dispose();
    }

    private void savePaletteBMP165x960ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        //save editor palette
        picPalette.Image.Save("palette.bmp");
    }

    private void showPortalsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        redrawTilemap();
    }

    private void menuViewShowTileNum_Click(object sender, EventArgs e)
    {
        redrawTilemap();
    }

    private void txtData1_TextChanged(object sender, EventArgs e)
    {
        if (radioEditMode.Checked)
        {
            tilemap[selectedTile.index].data1 = txtData1.Text;
        }
    }

    private void chkCollidable_Click(object sender, EventArgs e)
    {
    }

    private void chkCollidable_CheckedChanged(object sender, EventArgs e)
    {
        if (radioEditMode.Checked)
        {
            tilemap[selectedTile.index].collidable = chkCollidable.Checked;
        }
    }

    private void radioDrawMode_CheckedChanged(object sender, EventArgs e)
    {
        hideSelectionBox();
    }

    private void showDataToolStripMenuItem_Click(object sender, EventArgs e)
    {
        redrawTilemap();
    }

    private void showCollidableToolStripMenuItem_Click(object sender, EventArgs e)
    {
        redrawTilemap();
    }

    private void quitAltF4ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    private void fillEmptyTilesToolStripMenuItem_Click(object sender, EventArgs e)
    {
        string text = "This will fill only the empty tiles with the " +
            "currently selected palette tile, without changing data.";
        DialogResult result = MessageBox.Show(text, "Fill Empty Tiles?",MessageBoxButtons.YesNo);
        if (result == DialogResult.Yes)
        {
            for (int index = 0; index < mapSize * mapSize; index++)
            {
                if (tilemap[index].tilenum == 0)
                    tilemap[index].tilenum = selectedPaletteTile;
            }
            redrawTilemap();
        }
    }

    private void fillWholeMapToolStripMenuItem_Click(object sender, EventArgs e)
    {
        string text = "This will fill the entire tilemap with the " +
            "currently selected palette tile, without changing data.";
        DialogResult result = MessageBox.Show(text, "Fill Whole Map?", MessageBoxButtons.YesNo);
        if (result == DialogResult.Yes)
        {
            for (int index = 0; index < mapSize * mapSize; index++)
                tilemap[index].tilenum = selectedPaletteTile;
        }
        redrawTilemap();
    }

    private void clearAllDataToolStripMenuItem_Click(object sender, EventArgs e)
    {
        string text = "This will clear the four data fields without affecting " +
            "tile numbers, collidable, or portal properties.";
        DialogResult result = MessageBox.Show(text, "Clear Data?", MessageBoxButtons.YesNo);
        if (result == DialogResult.Yes)
        {
            for (int index = 0; index < mapSize * mapSize; index++)
            {
                tilemap[index].data1 = "";
                tilemap[index].data2 = "";
                tilemap[index].data3 = "";
                tilemap[index].data4 = "";
            }
        }
        redrawTilemap();
    }

    private void clearAllCollidablesToolStripMenuItem_Click(object sender, EventArgs e)
    {
        string text = "This will clear just the Collidable property " +
            "on all tiles without affecting any other data.";
        DialogResult result = MessageBox.Show(text, "Clear Collidable Property?", MessageBoxButtons.YesNo);
        if (result == DialogResult.Yes)
        {
            for (int index = 0; index < mapSize * mapSize; index++)
                tilemap[index].collidable = false;
        }
        redrawTilemap();
    }

    private void lblTileValue_Click(object sender, EventArgs e)
    {

    }

    private void txtTileNum_TextChanged(object sender, EventArgs e)
    {

    }

    private void chkPortal_CheckedChanged(object sender, EventArgs e)
    {
        if (radioEditMode.Checked)
        {
            tilemap[selectedTile.index].portal = chkPortal.Checked;
        }
    }

    private void txtPortalX_TextChanged(object sender, EventArgs e)
    {
        if (radioEditMode.Checked)
        {
            tilemap[selectedTile.index].portalx = Convert.ToInt32(txtPortalX.Text);
        }
    }

    private void txtPortalY_TextChanged(object sender, EventArgs e)
    {
        if (radioEditMode.Checked)
        {
            tilemap[selectedTile.index].portaly = Convert.ToInt32(txtPortalY.Text);
        }
    }

    private void txtPortalFile_TextChanged(object sender, EventArgs e)
    {
        if (radioEditMode.Checked)
        {
            tilemap[selectedTile.index].portalfile = txtPortalFile.Text;
        }
    }

    private void txtData2_TextChanged(object sender, EventArgs e)
    {
        if (radioEditMode.Checked)
        {
            tilemap[selectedTile.index].data2 = txtData2.Text;
        }
    }

    private void txtData3_TextChanged(object sender, EventArgs e)
    {
        if (radioEditMode.Checked)
        {
            tilemap[selectedTile.index].data3 = txtData3.Text;
        }
    }

    private void txtData4_TextChanged(object sender, EventArgs e)
    {
        if (radioEditMode.Checked)
        {
            tilemap[selectedTile.index].data4 = txtData4.Text;
        }
    }

    private void autoSetCollidableToolStripMenuItem_Click(object sender, EventArgs e)
    {
        string text = "This will set all tiles in the editor EXCEPT the currently selected palette tile to Collidable.";
        DialogResult result = MessageBox.Show(text, "Auto Set Collidable?", MessageBoxButtons.YesNo);
        if (result == DialogResult.Yes)
        {
            for (int index = 0; index < mapSize * mapSize; index++)
                if (tilemap[index].tilenum != selectedPaletteTile)
                    tilemap[index].collidable = true;
        }
        redrawTilemap();
    }

    private void clearAllPortalsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        string text = "This will clear all Portal-related fields throughout the level.";
        DialogResult result = MessageBox.Show(text, "Continue?", MessageBoxButtons.YesNo);
        if (result == DialogResult.Yes)
        {
            for (int index = 0; index < mapSize * mapSize; index++)
            {
                tilemap[index].portal = false;
                tilemap[index].portalx = 0;
                tilemap[index].portaly = 0;
                tilemap[index].portalfile = "";
            }
        }
        redrawTilemap();
    }



}
