Public Class PathPoint
    Inherits Label
    Private _F As Integer = -1
    Public Property F As Integer ' = -1
        Set(value As Integer)
            _F = value
            LabelF.Text = value
            If (value = 0) Then
                LabelF.Visible = False
            ElseIf (LabelF.Visible = False) Then
                LabelF.Visible = True
            End If
        End Set
        Get
            Return _F
        End Get
    End Property
    Private _isPathWay As Boolean = False
    Public Property isPathWay As Boolean
        Get
            Return _isPathWay
        End Get
        Set(value As Boolean)
            _isPathWay = value
            If (value) Then
                Me.BackColor = Color.Cyan
            Else
                Me.BackColor = Color.DarkGray
            End If
        End Set
    End Property
    Private _G As Integer = -1
    Public Property G As Integer '= -1
        Get
            Return _G
        End Get
        Set(value As Integer)
            _G = value
            LabelG.Text = value
            If (value = 0) Then
                LabelG.Visible = False
            ElseIf (LabelG.Visible = False) Then
                LabelG.Visible = True
            End If
            If (H > 0) Then
                F = G + H
            Else
                F = G
            End If
        End Set
    End Property
    Private _H As Integer = -1
    Public Property H As Integer ' = -1
        Get
            Return _H
        End Get
        Set(value As Integer)
            _H = value
            LabelH.Text = value
            If (value = 0) Then
                LabelH.Visible = False
            ElseIf (LabelH.Visible = False) Then
                LabelH.Visible = True
            End If
            F = G + H
        End Set
    End Property
    Public Enum PointType As Byte
        BLOCK = 0
        START = 1
        [END] = 2
        NOMORE = 3
    End Enum
    Private _x As Integer = Nothing
    Public ReadOnly Property X As Integer
        Get
            Return _x
        End Get
    End Property
    Private _y As Integer = Nothing
    Public ReadOnly Property Y As Integer
        Get
            Return _y
        End Get
    End Property
    Private LabelF As Label = Nothing
    Private LabelG As Label = Nothing
    Private LabelH As Label = Nothing
    Private _isEnable As PointType = PointType.BLOCK
    Private _isFasted As Boolean = False
    Public Property isFasted As Boolean
        Get
            Return _isFasted
        End Get
        Set(value As Boolean)
            _isFasted = value
            If (value) Then
                Me.BackColor = Color.Yellow
            ElseIf (PathType = PointType.NOMORE) Then
                Me.BackColor = Color.DarkGray
            End If
        End Set
    End Property
    Public Property PathType As PointType
        Get
            Return _isEnable
        End Get
        Set(value As PointType)
            _isEnable = value
            If (value = PointType.BLOCK) Then
                Me.BackColor = Color.Blue
            ElseIf (value = PointType.START) Then
                Me.BackColor = Color.Green
                StartPoint = Me
            ElseIf (value = PointType.END) Then
                Me.BackColor = Color.Red
                EndPoint = Me
            ElseIf (value = PointType.NOMORE) Then
                Me.BackColor = Color.DarkGray
            End If
        End Set
    End Property
    Public Sub New(px As Integer, py As Integer, pType As PointType)
        _x = px
        _y = py
        PathType = pType
        Me.AutoSize = False
        Me.Width = 40
        Me.Height = 40
        Me.BorderStyle = BorderStyle.FixedSingle
        LabelF = New Label
        LabelF.Width = 29
        LabelF.Height = 12
        LabelF.Text = 0
        LabelF.AutoSize = True
        LabelF.Left = 0
        LabelF.Top = 0
        Me.Controls.Add(LabelF)
        LabelG = New Label
        LabelG.Width = 29
        LabelG.Height = 12
        LabelG.Text = 0
        LabelG.AutoSize = True
        LabelG.Left = 0
        LabelG.Top = Me.Height - LabelG.Height
        Me.Controls.Add(LabelG)
        LabelH = New Label
        LabelH.Width = 29
        LabelH.Height = 12
        LabelH.Text = 0
        LabelH.AutoSize = True
        LabelH.Left = Me.Width - LabelH.Width / 2 - 4
        LabelH.Top = Me.Height - LabelH.Height
        Me.Controls.Add(LabelH)
        F = 0
        G = 0
        H = 0
    End Sub

    Private Sub PathPoint_Click(sender As Object, e As System.EventArgs) Handles Me.Click
        If (PathType = PointType.BLOCK) Then
            PathType = PointType.START
        ElseIf (PathType = PointType.START) Then
            PathType = PointType.END
        ElseIf (PathType = PointType.END) Then
            PathType = PointType.NOMORE
        Else
            PathType = PointType.BLOCK
        End If
    End Sub
End Class
