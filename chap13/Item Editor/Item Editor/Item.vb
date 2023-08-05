Public Class Item

    Private p_name As String
    Private p_desc As String
    Private p_dropfile As String
    Private p_invfile As String
    Private p_category As String
    Private p_weight As Single
    Private p_value As Single
    Private p_attacknumdice As Integer
    Private p_attackdie As Integer
    Private p_defense As Integer
    Private p_buffStr As Integer
    Private p_buffDex As Integer
    Private p_buffSta As Integer
    Private p_buffInt As Integer
    Private p_buffCha As Integer

    Public Sub New()
        p_name = "new item"
        p_desc = ""
        p_dropfile = ""
        p_invfile = ""
        p_category = ""
        p_weight = 0.0
        p_value = 0.0
        p_attacknumdice = 0
        p_attackdie = 0
        p_defense = 0
        p_buffStr = 0
        p_buffDex = 0
        p_buffSta = 0
        p_buffInt = 0
        p_buffCha = 0
    End Sub

    Public Property Name() As String
        Get
            Return p_name
        End Get
        Set(ByVal value As String)
            p_name = value
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

    Public Property DropImageFilename() As String
        Get
            Return p_dropfile
        End Get
        Set(ByVal value As String)
            p_dropfile = value
        End Set
    End Property

    Public Property InvImageFilename() As String
        Get
            Return p_invfile
        End Get
        Set(ByVal value As String)
            p_invfile = value
        End Set
    End Property

    Public Property Category() As String
        Get
            Return p_category
        End Get
        Set(ByVal value As String)
            p_category = value
        End Set
    End Property

    Public Property Weight() As Single
        Get
            Return p_weight
        End Get
        Set(ByVal value As Single)
            p_weight = value
        End Set
    End Property

    Public Property Value() As Single
        Get
            Return p_value
        End Get
        Set(ByVal value As Single)
            p_value = value
        End Set
    End Property

    Public Property AttackNumDice() As Integer
        Get
            Return p_attacknumdice
        End Get
        Set(ByVal value As Integer)
            p_attacknumdice = value
        End Set
    End Property

    Public Property AttackDie() As Integer
        Get
            Return p_attackdie
        End Get
        Set(ByVal value As Integer)
            p_attackdie = value
        End Set
    End Property

    Public Property Defense() As Integer
        Get
            Return p_defense
        End Get
        Set(ByVal value As Integer)
            p_defense = value
        End Set
    End Property

    Public Property STR() As Integer
        Get
            Return p_buffStr
        End Get
        Set(ByVal value As Integer)
            p_buffStr = value
        End Set
    End Property

    Public Property DEX() As Integer
        Get
            Return p_buffDex
        End Get
        Set(ByVal value As Integer)
            p_buffDex = value
        End Set
    End Property

    Public Property STA() As Integer
        Get
            Return p_buffSta
        End Get
        Set(ByVal value As Integer)
            p_buffSta = value
        End Set
    End Property

    Public Property INT() As Integer
        Get
            Return p_buffInt
        End Get
        Set(ByVal value As Integer)
            p_buffInt = value
        End Set
    End Property

    Public Property CHA() As Integer
        Get
            Return p_buffCha
        End Get
        Set(ByVal value As Integer)
            p_buffCha = value
        End Set
    End Property

    Public ReadOnly Property Summary() As String
        Get
            Dim text As String
            text = "This '" + p_name + "', "

            Dim weight As String = ""
            Select Case p_weight
                Case Is > 50 : weight = "a very heavy "
                Case Is > 25 : weight = "a heavy "
                Case Is > 15 : weight = "a "
                Case Is > 7 : weight = "a light "
                Case Is > 0 : weight = "a very light "
            End Select
            text += weight

            Select Case p_category
                Case "Weapon" : text += "weapon"
                Case "Armor" : text += "armor item"
                Case "Necklace" : text += "necklace"
                Case "Ring" : text += "ring"
                Case Else : text += p_category.ToLower() + " item"
            End Select

            If p_attacknumdice <> 0 Then
                text += ", attacks at " + p_attacknumdice.ToString() _
                    + "D" + p_attackdie.ToString() _
                    + " (" + p_attacknumdice.ToString() + " - " _
                    + (p_attackdie * p_attacknumdice).ToString() _
                    + " damage)"
            End If

            If p_defense <> 0 Then
                text += ", adds " + p_defense.ToString() + " armor points"
            End If

            Dim fmt As String = "+#;-#"
            If p_buffStr <> 0 Then
                text += ", " + p_buffStr.ToString(fmt) + " STR"
            End If
            If p_buffDex <> 0 Then
                text += ", " + p_buffDex.ToString(fmt) + " DEX"
            End If
            If p_buffSta <> 0 Then
                text += ", " + p_buffSta.ToString(fmt) + " STA"
            End If
            If p_buffInt <> 0 Then
                text += ", " + p_buffInt.ToString(fmt) + " INT"
            End If
            If p_buffCha <> 0 Then
                text += ", " + p_buffCha.ToString(fmt) + " CHA"
            End If

            Return text + "."
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return p_name
    End Function

End Class
