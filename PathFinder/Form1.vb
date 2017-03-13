Public Class Form1

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs)
        
    End Sub
    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        For y As Integer = 0 To 10
            For x As Integer = 0 To 10
                Dim l As New PathPoint(x, y, PathPoint.PointType.NOMORE)
                l.BackColor = Color.DarkGray
                l.Left = x * 40
                l.Top = y * 40
                PathList.Add(l)
                Me.Controls.Add(l)
                Application.DoEvents()
            Next
        Next
    End Sub

    Private Sub Button1_Click_1(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        PathWays.Clear()
        OpenList.Clear()
        ClosedList.Clear()
        ' PathList.Clear()
        Call DoFindPath()
        'ClosedList.Add(StartPoint)
        ' ClosedList.Add(EndPoint)
        'Call CoursePoint(StartPoint)
    End Sub

    Private Sub doPoint()
       
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        For Each p As PathPoint In PathList
            p.isFasted = False
            p.G = 0
            p.H = 0
        Next
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click

    End Sub
End Class
