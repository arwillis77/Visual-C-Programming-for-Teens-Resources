Imports System.Xml

Public Class Level

    Public Structure tilemapStruct
        Public tilenum As Integer
        Public data1 As String
        Public data2 As String
        Public data3 As String
        Public data4 As String
        Public collidable As Boolean
        Public portal As Boolean
        Public portalx As Integer
        Public portaly As Integer
        Public portalfile As String
    End Structure

    Private p_game As Game
    Private p_mapSize As New Point(0, 0)
    Private p_windowSize As New Point(0, 0)
    Private p_tileSize As Integer
    Private p_bmpTiles As Bitmap
    Private p_columns As Integer
    Private p_bmpScrollBuffer As Bitmap
    Private p_gfxScrollBuffer As Graphics
    Private p_tilemap() As tilemapStruct
    Private p_scrollPos As New PointF(0, 0)
    Private p_subtile As New PointF(0, 0)
    Private p_oldScrollPos As New PointF(-1, -1)

    Public Function getTile(ByVal p As PointF) As tilemapStruct
        Return getTile(p.Y * 128 + p.X)
    End Function

    Public Function getTile(ByVal pixelx As Integer, ByVal pixely As Integer) As tilemapStruct
        Return getTile(pixely * 128 + pixelx)
    End Function

    Public Function getTile(ByVal index As Integer) As tilemapStruct
        Return p_tilemap(index)
    End Function

    REM get/set scroll position by whole tile position
    Public Property GridPos() As Point
        Get
            Dim x As Integer = p_scrollPos.X / p_tileSize
            Dim y As Integer = p_scrollPos.Y / p_tileSize
            Return New Point(x, y)
        End Get
        Set(ByVal value As Point)
            Dim x As Single = value.X * p_tileSize
            Dim y As Single = value.Y * p_tileSize
            p_scrollPos = New PointF(x, y)
        End Set
    End Property

    REM get/set scroll position by pixel position 
    Public Property ScrollPos() As PointF
        Get
            Return p_scrollPos
        End Get
        Set(ByVal value As PointF)
            REM save new scroll position
            p_scrollPos = value
        End Set
    End Property

    Public Sub New(ByRef gameObject As Game, ByVal width As Integer, ByVal height As Integer, ByVal tileSize As Integer)
        p_game = gameObject
        p_windowSize = New Point(width, height)
        p_mapSize = New Point(width * tileSize, height * tileSize)
        p_tileSize = tileSize
        REM create scroll buffer
        p_bmpScrollBuffer = New Bitmap(p_mapSize.X + p_tileSize, p_mapSize.Y + p_tileSize)
        p_gfxScrollBuffer = Graphics.FromImage(p_bmpScrollBuffer)
        REM create tilemap 
        ReDim p_tilemap(128 * 128)
    End Sub

    Public Function loadTilemap(ByVal filename As String) As Boolean
        Try
            Dim doc As XmlDocument = New XmlDocument()
            doc.Load(filename)
            Dim nodelist As XmlNodeList = doc.GetElementsByTagName("tiles")
            For Each node As XmlNode In nodelist
                Dim element As XmlElement = node
                Dim index As Integer = 0
                Dim ts As tilemapStruct
                Dim data As String

                REM read data fields from xml 
                Data = element.GetElementsByTagName("tile")(0).InnerText
                index = Convert.ToInt32(data)
                data = element.GetElementsByTagName("value")(0).InnerText
                ts.tilenum = Convert.ToInt32(data)
                data = element.GetElementsByTagName("data1")(0).InnerText
                ts.data1 = Convert.ToString(data)
                data = element.GetElementsByTagName("data2")(0).InnerText
                ts.data2 = Convert.ToString(data)
                data = element.GetElementsByTagName("data3")(0).InnerText
                ts.data3 = Convert.ToString(data)
                data = element.GetElementsByTagName("data4")(0).InnerText
                ts.data4 = Convert.ToString(data)
                data = element.GetElementsByTagName("collidable")(0).InnerText
                ts.collidable = Convert.ToBoolean(data)
                data = element.GetElementsByTagName("portal")(0).InnerText
                ts.portal = Convert.ToBoolean(data)
                data = element.GetElementsByTagName("portalx")(0).InnerText
                ts.portalx = Convert.ToInt32(data)
                data = element.GetElementsByTagName("portaly")(0).InnerText
                ts.portaly = Convert.ToInt32(data)
                data = element.GetElementsByTagName("portalfile")(0).InnerText
                ts.portalfile = Convert.ToString(data)

                REM store data in tilemap
                p_tilemap(index) = ts
            Next
        Catch es As Exception
            MessageBox.Show(es.Message)
            Return False
        End Try
        Return True
    End Function

    Public Function loadPalette(ByVal filename As String, ByVal columns As Integer) As Boolean
        p_columns = columns
        Try
            p_bmpTiles = New Bitmap(filename)
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Public Sub Update()
        REM fill the scroll buffer only when moving
        If p_scrollPos <> p_oldScrollPos Then
            p_oldScrollPos = p_scrollPos

            REM validate X range
            If p_scrollPos.X < 0 Then p_scrollPos.X = 0
            If p_scrollPos.X > (127 - p_windowSize.X) * p_tileSize Then
                p_scrollPos.X = (127 - p_windowSize.X) * p_tileSize
            End If

            REM validate Y range
            If p_scrollPos.Y < 0 Then p_scrollPos.Y = 0
            If p_scrollPos.Y > (127 - p_windowSize.Y) * p_tileSize Then
                p_scrollPos.Y = (127 - p_windowSize.Y) * p_tileSize
            End If

            REM calculate sub-tile size
            p_subtile.X = p_scrollPos.X Mod p_tileSize
            p_subtile.Y = p_scrollPos.Y Mod p_tileSize


            REM fill scroll buffer with tiles
            Dim tilenum, sx, sy As Integer
            For x = 0 To p_windowSize.X
                For y = 0 To p_windowSize.Y
                    sx = p_scrollPos.X \ p_tileSize + x
                    sy = p_scrollPos.Y \ p_tileSize + y
                    tilenum = p_tilemap(sy * 128 + sx).tilenum
                    drawTileNumber(x, y, tilenum)
                Next
            Next

        End If

    End Sub

    Public Sub drawTileNumber(ByVal x As Integer, ByVal y As Integer, ByVal tile As Integer)
        Dim sx As Integer = (tile Mod p_columns) * (p_tileSize + 1)
        Dim sy As Integer = (tile \ p_columns) * (p_tileSize + 1)
        Dim src As New Rectangle(sx, sy, p_tileSize, p_tileSize)
        Dim dx As Integer = x * p_tileSize
        Dim dy As Integer = y * p_tileSize
        p_gfxScrollBuffer.DrawImage(p_bmpTiles, dx, dy, src, GraphicsUnit.Pixel)
    End Sub

    Public Sub Draw(ByVal rect As Rectangle)
        Draw(rect.X, rect.Y, rect.Width, rect.Height)
    End Sub
    Public Sub Draw(ByVal width As Integer, ByVal height As Integer)
        Draw(0, 0, width, height)
    End Sub
    Public Sub Draw(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        REM draw the scroll viewport
        Dim source As New Rectangle(p_subtile.X, p_subtile.Y, width, height)
        p_game.Device.DrawImage(p_bmpScrollBuffer, x, y, source, GraphicsUnit.Pixel)
    End Sub

End Class
