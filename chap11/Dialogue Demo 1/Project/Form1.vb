
Public Class Form1

    Public Structure keyStates
        Public up, down, left, right As Boolean
    End Structure

    Private game As Game
    Private level As Level
    Private keyState As keyStates
    Private gameover As Boolean = False
    Private hero As Character
    Private vendor As Character
    Private talkFlag As Boolean = False
    Private talking As Boolean = False

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "NPC Dialogue Demo 1"

        REM create game object
        game = New Game(Me, 800, 600)

        REM create tilemap
        level = New Level(game, 25, 19, 32)
        level.loadTilemap("sample.level")
        level.loadPalette("palette.bmp", 5)

        REM load hero
        hero = New Character(game)
        hero.Load("paladin.char")
        hero.Position = New Point(400 - 48, 300 - 48)

        REM load vendor
        vendor = New Character(game)
        vendor.Load("vendor.char")
        vendor.Position = New Point(600, 300)

        While Not gameover
            doUpdate()
        End While

    End Sub


    Private Sub Form1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case (e.KeyCode)
            Case Keys.Up, Keys.W : keyState.up = True
            Case Keys.Down, Keys.S : keyState.down = True
            Case Keys.Left, Keys.A : keyState.left = True
            Case Keys.Right, Keys.D : keyState.right = True
            Case Keys.Space : talkFlag = True
        End Select
    End Sub

    Private Sub Form1_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        Select Case (e.KeyCode)
            Case Keys.Escape : End
            Case Keys.Up, Keys.W : keyState.up = False
            Case Keys.Down, Keys.S : keyState.down = False
            Case Keys.Left, Keys.A : keyState.left = False
            Case Keys.Right, Keys.D : keyState.right = False
            Case Keys.Space : talkFlag = False
        End Select
    End Sub

    Private Sub doUpdate()
        Dim frameRate As Integer = game.FrameRate()
        Dim ticks As Integer = Environment.TickCount()
        Static drawLast As Integer = 0

        If ticks > drawLast + 16 Then
            drawLast = ticks

            doScrolling()

            doHero()

            doVendor()

            REM print stats
            game.Print(700, 0, frameRate.ToString())
            Dim y As Integer = 0
            game.Print(0, 0, "Scroll " + level.ScrollPos.ToString())
            game.Print(0, 20, "Player " + hero.Position.ToString())

            REM get position under player's feet
            Dim feet As Point = HeroFeet()
            Dim tilex As Integer = (level.ScrollPos.X + feet.X) / 32
            Dim tiley As Integer = (level.ScrollPos.Y + feet.Y) / 32
            Dim ts As Level.tilemapStruct
            ts = level.getTile(tilex, tiley)
            game.Print(0, 40, "Tile " + tilex.ToString() + "," + tiley.ToString() + " = " + ts.tilenum.ToString())

            REM talk flag
            game.Print(0, 60, "Talking: " + talking.ToString())

            REM refresh window
            game.Update()
            Application.DoEvents()
        Else
            REM throttle the cpu
            Threading.Thread.Sleep(1)
        End If
    End Sub

    Private Sub doScrolling()
        REM move the tilemap scroll position
        Dim steps As Integer = 4
        Dim pos As PointF = level.ScrollPos

        REM up key movement
        If keyState.up Then
            If hero.Y > 300 - 48 Then
                hero.Y -= steps
            Else
                pos.Y -= steps
                If pos.Y <= 0 Then
                    hero.Y -= steps
                End If
            End If

            REM down key movement
        ElseIf keyState.down Then
            If hero.Y < 300 - 48 Then
                hero.Y += steps
            Else
                pos.Y += steps
                If pos.Y >= (127 - 19) * 32 Then
                    hero.Y += steps
                End If
            End If
        End If

        REM left key movement
        If keyState.left Then
            If hero.X > 400 - 48 Then
                hero.X -= steps
            Else
                pos.X -= steps
                If pos.X <= 0 Then
                    hero.X -= steps
                End If
            End If

            REM right key movement
        ElseIf keyState.right Then
            If hero.X < 400 - 48 Then
                hero.X += steps
            Else
                pos.X += steps
                If pos.X >= (127 - 25) * 32 Then
                    hero.X += steps
                End If
            End If
        End If

        REM update scroller position
        level.ScrollPos = pos
        level.Update()

        REM draw the tilemap
        level.Draw(0, 0, 800, 600)

    End Sub

    Private Sub doHero()
        REM limit player sprite to the screen boundary
        If hero.X < -32 Then
            hero.X = -32
        ElseIf hero.X > 800 - 65 Then
            hero.X = 800 - 65
        End If
        If hero.Y < -48 Then
            hero.Y = -48
        ElseIf hero.Y > 600 - 81 Then
            hero.Y = 600 - 81
        End If

        REM orient the player in the right direction
        If keyState.up And keyState.right Then
            hero.Direction = 1
        ElseIf keyState.right And keyState.down Then
            hero.Direction = 3
        ElseIf keyState.down And keyState.left Then
            hero.Direction = 5
        ElseIf keyState.left And keyState.up Then
            hero.Direction = 7
        ElseIf keyState.up Then
            hero.Direction = 0
        ElseIf keyState.right Then
            hero.Direction = 2
        ElseIf keyState.down Then
            hero.Direction = 4
        ElseIf keyState.left Then
            hero.Direction = 6
        Else
            hero.Direction = -1
        End If

        REM draw the hero
        hero.Draw()

    End Sub

    Private Sub doVendor()
        Dim relativePos As PointF

        REM draw the vendor sprite
        If vendor.X > level.ScrollPos.X _
        And vendor.X < level.ScrollPos.X + 23 * 32 _
        And vendor.Y > level.ScrollPos.Y _
        And vendor.Y < level.ScrollPos.Y + 17 * 32 Then
            relativePos.X = Math.Abs(level.ScrollPos.X - vendor.X)
            relativePos.Y = Math.Abs(level.ScrollPos.Y - vendor.Y)
            vendor.GetSprite.Draw(relativePos.X, relativePos.Y)
        End If

        Dim talkRadius As Integer = 70

        REM get center of hero sprite
        Dim heroCenter As PointF = HeroFeet()
        heroCenter.X += 16
        heroCenter.Y += 16
        game.Device.DrawRectangle(Pens.Red, heroCenter.X - 2, heroCenter.Y - 2, 4, 4)

        REM get center of NPC
        Dim vendorCenter As PointF = relativePos
        vendorCenter.X += vendor.GetSprite.Width / 2
        vendorCenter.Y += vendor.GetSprite.Height / 2

        game.Device.DrawRectangle(Pens.Red, vendorCenter.X - 2, vendorCenter.Y - 2, 4, 4)


        Dim dist As Single = Distance(heroCenter, vendorCenter)
        Dim color As Pen

        REM draw line connecting player to vendor
        If dist < talkRadius Then
            color = New Pen(Brushes.Blue, 2.0)
        Else
            color = New Pen(Brushes.Red, 2.0)
        End If
        game.Device.DrawLine(color, heroCenter, vendorCenter)

        REM print distance
        game.Print(relativePos.X, relativePos.Y, _
            "D = " + dist.ToString("N0"), Brushes.White)

        REM draw circle around vendor to show talk radius
        Dim spriteSize As Single = vendor.GetSprite.Width / 2
        Dim centerx As Single = relativePos.X + spriteSize
        Dim centery As Single = relativePos.Y + spriteSize
        Dim circleRect As New RectangleF( _
            centerx - talkRadius, _
            centery - talkRadius, _
            talkRadius * 2, talkRadius * 2)
        game.Device.DrawEllipse(color, circleRect)

        REM is playing trying to talk to this vendor?
        If dist < talkRadius Then
            If talkFlag Then
                talking = True
            End If
        Else
            talking = False
        End If

    End Sub

    Public Function Distance(ByVal first As PointF, ByVal second As PointF) As Single
        Dim deltaX As Single = second.X - first.X
        Dim deltaY As Single = second.Y - first.Y
        Dim dist = Math.Sqrt(deltaX * deltaX + deltaY * deltaY)
        Return dist
    End Function

    REM return bottom center position of hero sprite where feet are touching ground
    Private Function HeroFeet() As Point
        Return New Point(hero.X + 32, hero.Y + 32 + 16)
    End Function

End Class
