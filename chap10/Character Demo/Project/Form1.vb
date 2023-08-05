
Public Class Form1

    Public Structure keyStates
        Public up, down, left, right As Boolean
    End Structure

    Private game As Game
    Private level As Level
    Private keyState As keyStates
    Private gameover As Boolean = False
    Private hero As Character
    Private portalFlag As Boolean = False
    Private portalTarget As Point


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "Character Demo"

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
        End Select
    End Sub

    Private Sub Form1_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        Select Case (e.KeyCode)
            Case Keys.Escape : End
            Case Keys.Up, Keys.W : keyState.up = False
            Case Keys.Down, Keys.S : keyState.down = False
            Case Keys.Left, Keys.A : keyState.left = False
            Case Keys.Right, Keys.D : keyState.right = False
            Case Keys.Space
                If portalFlag Then
                    level.GridPos = portalTarget
                End If
            Case Keys.D1
                hero.AnimationState = Character.AnimationStates.Walking
            Case Keys.D2
                hero.AnimationState = Character.AnimationStates.Attacking
            Case Keys.D3
                hero.AnimationState = Character.AnimationStates.Dying
        End Select
    End Sub

    Private Sub doUpdate()
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

        REM get the untimed core frame rate 
        Dim frameRate As Integer = game.FrameRate()

        REM drawing code should be limited to 60 fps
        Dim ticks As Integer = Environment.TickCount()
        Static drawLast As Integer = 0
        If ticks > drawLast + 16 Then '1000/16 = ~60
            drawLast = ticks

            REM draw the tilemap
            level.Draw(0, 0, 800, 600)

            REM draw the hero
            hero.Draw()

            REM print stats
            game.Print(700, 0, frameRate.ToString())
            Dim y As Integer = 0
            game.Print(0, y, "Scroll " + level.ScrollPos.ToString())
            y += 20
            game.Print(0, y, "Player " + hero.Position.ToString())
            y += 20

            Dim feet As Point = HeroFeet()

            Dim tilex As Integer = (level.ScrollPos.X + feet.X) / 32
            Dim tiley As Integer = (level.ScrollPos.Y + feet.Y) / 32
            Dim ts As Level.tilemapStruct
            ts = level.getTile(tilex, tiley)
            game.Print(0, y, "Tile " + tilex.ToString() + "," + tiley.ToString() + " = " + ts.tilenum.ToString())
            y += 20
            If ts.collidable Then
                game.Print(0, y, "Collidable")
                y += 20
            End If
            If ts.portal Then
                game.Print(0, y, "Portal to " + ts.portalx.ToString() + "," + ts.portaly.ToString())
                portalFlag = True
                portalTarget = New Point(ts.portalx - feet.X / 32, ts.portaly - feet.Y / 32)
                y += 20
            Else
                portalFlag = False
            End If

            REM highlight collision areas around player
            game.Device.DrawRectangle(Pens.Blue, hero.GetSprite.Bounds())
            game.Device.DrawRectangle(Pens.Red, feet.X + 16 - 1, feet.Y + 16 - 1, 2, 2)
            game.Device.DrawRectangle(Pens.Red, feet.X, feet.Y, 32, 32)

            REM refresh window
            game.Update()
            Application.DoEvents()
        Else
            REM throttle the cpu
            Threading.Thread.Sleep(1)
        End If
    End Sub

    REM return bottom center position of hero sprite where feet are touching ground
    Private Function HeroFeet() As Point
        Return New Point(hero.X + 32, hero.Y + 32 + 16)
    End Function

End Class
