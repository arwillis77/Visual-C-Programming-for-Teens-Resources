Imports System.Xml


Public Class Character
    Public Enum AnimationStates
        Walking
        Attacking
        Dying
    End Enum

    Private p_game As Game
    Private p_position As PointF
    Private p_direction As Integer
    Private p_state As AnimationStates

    REM character file properties
    Private p_name As String
    Private p_class As String
    Private p_race As String
    Private p_desc As String
    Private p_str As Integer
    Private p_dex As Integer
    Private p_sta As Integer
    Private p_int As Integer
    Private p_cha As Integer
    Private p_hitpoints As Integer
    Private p_dropGold1 As Integer
    Private p_dropGold2 As Integer
    Private p_walkFilename As String
    Private p_walkSprite As Sprite
    Private p_walkSize As Point
    Private p_walkColumns As Integer
    Private p_attackFilename As String
    Private p_attackSprite As Sprite
    Private p_attackSize As Point
    Private p_attackColumns As Integer
    Private p_dieFilename As String
    Private p_dieSprite As Sprite
    Private p_dieSize As Point
    Private p_dieColumns As Integer

    Public Sub New(ByRef game As Game)
        p_game = game
        p_position = New PointF(0, 0)
        p_direction = 1
        p_state = AnimationStates.Walking

        REM initialize loadable properties
        p_name = ""
        p_class = ""
        p_race = ""
        p_desc = ""
        p_str = 0
        p_dex = 0
        p_sta = 0
        p_int = 0
        p_cha = 0
        p_hitpoints = 0
        p_dropGold1 = 0
        p_dropGold2 = 0
        p_walkSprite = Nothing
        p_walkFilename = ""
        p_walkSize = New Point(0, 0)
        p_walkColumns = 0
        p_attackSprite = Nothing
        p_attackFilename = ""
        p_attackSize = New Point(0, 0)
        p_attackColumns = 0
        p_dieSprite = Nothing
        p_dieFilename = ""
        p_dieSize = New Point(0, 0)
        p_dieColumns = 0

    End Sub

    Public Property Name() As String
        Get
            Return p_name
        End Get
        Set(ByVal value As String)
            p_name = value
        End Set
    End Property

    Public Property PlayerClass() As String
        Get
            Return p_class
        End Get
        Set(ByVal value As String)
            p_class = value
        End Set
    End Property

    Public Property Race() As String
        Get
            Return p_race
        End Get
        Set(ByVal value As String)
            p_race = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return p_desc
        End Get
        Set(ByVal value As String)
            p_desc = value
        End Set
    End Property

    Public Property STR() As Integer
        Get
            Return p_str
        End Get
        Set(ByVal value As Integer)
            p_str = value
        End Set
    End Property

    Public Property DEX() As Integer
        Get
            Return p_dex
        End Get
        Set(ByVal value As Integer)
            p_dex = value
        End Set
    End Property

    Public Property STA() As Integer
        Get
            Return p_sta
        End Get
        Set(ByVal value As Integer)
            p_sta = value
        End Set
    End Property

    Public Property INT() As Integer
        Get
            Return p_int
        End Get
        Set(ByVal value As Integer)
            p_int = value
        End Set
    End Property

    Public Property CHA() As Integer
        Get
            Return p_cha
        End Get
        Set(ByVal value As Integer)
            p_cha = value
        End Set
    End Property

    Public Property HitPoints() As Integer
        Get
            Return p_hitpoints
        End Get
        Set(ByVal value As Integer)
            p_hitpoints = value
        End Set
    End Property

    Public Property DropGoldMin() As Integer
        Get
            Return p_dropGold1
        End Get
        Set(ByVal value As Integer)
            p_dropGold1 = value
        End Set
    End Property

    Public Property DropGoldMax() As Integer
        Get
            Return p_dropGold2
        End Get
        Set(ByVal value As Integer)
            p_dropGold2 = value
        End Set
    End Property

    Public ReadOnly Property GetSprite() As Sprite
        Get
            Select Case p_state
                Case AnimationStates.Walking
                    Return p_walkSprite
                Case AnimationStates.Attacking
                    Return p_attackSprite
                Case AnimationStates.Dying
                    Return p_dieSprite
                Case Else
                    Return p_walkSprite
            End Select
        End Get
    End Property

    'Public ReadOnly Property WalkSprite() As Sprite
    '    Get
    '        Return p_walkSprite
    '    End Get
    'End Property

    'Public ReadOnly Property AttackSprite() As Sprite
    '    Get
    '        Return p_attackSprite
    '    End Get
    'End Property

    'Public ReadOnly Property DieSprite() As Sprite
    '    Get
    '        Return p_dieSprite
    '    End Get
    'End Property

    Public Property Position() As PointF
        Get
            Return p_position
        End Get
        Set(ByVal value As PointF)
            p_position = value
        End Set
    End Property

    Public Property X() As Single
        Get
            Return p_position.X
        End Get
        Set(ByVal value As Single)
            p_position.X = value
        End Set
    End Property

    Public Property Y() As Single
        Get
            Return p_position.Y
        End Get
        Set(ByVal value As Single)
            p_position.Y = value
        End Set
    End Property

    Public Property Direction() As Integer
        Get
            Return p_direction
        End Get
        Set(ByVal value As Integer)
            p_direction = value
        End Set
    End Property

    Public Property AnimationState() As AnimationStates
        Get
            Return p_state
        End Get
        Set(ByVal value As AnimationStates)
            p_state = value
        End Set
    End Property

    Public Sub Draw()
        Dim startFrame As Integer
        Dim endFrame As Integer
        Select Case p_state
            Case AnimationStates.Walking
                p_walkSprite.Position = p_position
                If p_direction > -1 Then
                    startFrame = p_direction * p_walkColumns
                    endFrame = startFrame + p_walkColumns - 1
                    p_walkSprite.AnimationRate = 30
                    p_walkSprite.Animate(startFrame, endFrame)
                End If
                p_walkSprite.Draw()
            Case AnimationStates.Attacking
                p_attackSprite.Position = p_position
                If p_direction > -1 Then
                    startFrame = p_direction * p_attackColumns
                    endFrame = startFrame + p_attackColumns - 1
                    p_attackSprite.AnimationRate = 30
                    p_attackSprite.Animate(startFrame, endFrame)
                End If
                p_attackSprite.Draw()
            Case AnimationStates.Dying
                p_dieSprite.Position = p_position
                If p_direction > -1 Then
                    startFrame = p_direction * p_dieColumns
                    endFrame = startFrame + p_dieColumns - 1
                    p_dieSprite.AnimationRate = 30
                    p_dieSprite.Animate(startFrame, endFrame)
                End If
                p_dieSprite.Draw()
        End Select
    End Sub

    Public Function Load(ByVal filename As String)
        Try
            REM open the xml file 
            Dim doc As New XmlDocument()
            doc.Load(filename)
            Dim list As XmlNodeList = doc.GetElementsByTagName("character")
            Dim element As XmlElement = list(0)

            REM read data fields
            p_name = getElement("name", element)
            p_class = getElement("class", element)
            p_race = getElement("race", element)
            p_desc = getElement("desc", element)
            p_str = getElement("str", element)
            p_dex = getElement("dex", element)
            p_sta = getElement("sta", element)
            p_int = getElement("int", element)
            p_cha = getElement("cha", element)
            p_hitpoints = getElement("hitpoints", element)
            p_walkFilename = getElement("anim_walk_filename", element)

            p_walkSize.X = Convert.ToInt32( _
                getElement("anim_walk_width", element))

            p_walkSize.Y = Convert.ToInt32( _
                getElement("anim_walk_height", element))

            p_walkColumns = Convert.ToInt32( _
                getElement("anim_walk_columns", element))

            p_attackFilename = getElement( _
                "anim_attack_filename", element)

            p_attackSize.X = Convert.ToInt32( _
                getElement("anim_attack_width", element))

            p_attackSize.Y = Convert.ToInt32( _
                getElement("anim_attack_height", element))

            p_attackColumns = Convert.ToInt32( _
                getElement("anim_attack_columns", element))

            p_dieFilename = getElement( _
                "anim_die_filename", element)

            p_dieSize.X = Convert.ToInt32( _
                getElement("anim_die_width", element))

            p_dieSize.Y = Convert.ToInt32( _
                getElement("anim_die_height", element))

            p_dieColumns = Convert.ToInt32( _
                getElement("anim_die_columns", element))

            p_dropGold1 = Convert.ToInt32( _
                getElement("dropgold1", element))

            p_dropGold2 = Convert.ToInt32( _
                getElement("dropgold2", element))
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return False
        End Try

        REM create character sprites
        Try
            If p_walkFilename <> "" Then
                p_walkSprite = New Sprite(p_game)
                p_walkSprite.Image = LoadBitmap(p_walkFilename)
                p_walkSprite.Size = p_walkSize
                p_walkSprite.Columns = p_walkColumns
                p_walkSprite.TotalFrames = p_walkColumns * 8
            End If

            If p_attackFilename <> "" Then
                p_attackSprite = New Sprite(p_game)
                p_attackSprite.Image = LoadBitmap(p_attackFilename)
                p_attackSprite.Size = p_attackSize
                p_attackSprite.Columns = p_attackColumns
                p_attackSprite.TotalFrames = p_attackColumns * 8
            End If

            If p_dieFilename <> "" Then
                p_dieSprite = New Sprite(p_game)
                p_dieSprite.Image = LoadBitmap(p_dieFilename)
                p_dieSprite.Size = p_dieSize
                p_dieSprite.Columns = p_dieColumns
                p_dieSprite.TotalFrames = p_dieColumns * 8
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return False
        End Try

        Return True
    End Function

    Private Function LoadBitmap(ByVal filename As String)
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

End Class
