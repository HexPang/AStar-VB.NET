Module PathLogic
    Public OpenList As New List(Of PathPoint)
    Public ClosedList As New List(Of PathPoint)
    Public PathList As New List(Of PathPoint)
    Public PathWays As New List(Of PathPoint)
    Private WayList As List(Of PathPoint) = Nothing
    Private Structure FGH
        Dim F As Integer
        Dim G As Integer
        Dim H As Integer
    End Structure
    Public StartPoint As PathPoint = Nothing
    Public EndPoint As PathPoint = Nothing
    Public Function getMinF(pathS As List(Of PathPoint)) As PathPoint
        Dim minF As PathPoint = Nothing
        For Each p As PathPoint In pathS
            If (p IsNot Nothing) Then
                If (minF Is Nothing) Then
                    minF = p
                Else
                    If (p.F < minF.F) Then
                        minF = p
                    End If
                End If
            End If
        Next
        Return minF
    End Function
    Public Sub DoFindPath()
        Dim StartP As Point = Nothing
        Dim EndP As Point = Nothing
        Dim EndFound As Boolean = False
        For i As Integer = 0 To PathList.Count - 1
            Dim PP As Object = PathList(i)
            If (TypeOf (PP) Is PathPoint) Then
                If (CType(PP, PathPoint).PathType = PathPoint.PointType.START Or CType(PP, PathPoint).PathType = PathPoint.PointType.END) Then
                    If (CType(PP, PathPoint).PathType = PathPoint.PointType.START) Then
                        StartP = New Point(CType(PP, PathPoint).X, CType(PP, PathPoint).Y)
                        StartPoint = PP
                        ClosedList.Add(PP)
                    Else
                        EndP = New Point(CType(PP, PathPoint).X, CType(PP, PathPoint).Y)
                        EndPoint = PP
                        EndFound = True
                    End If
                    If (StartP.X >= 0 And EndP.X >= 0) Then
                        For y As Integer = -1 To 1
                            For x As Integer = -1 To 1
                                Dim p As PathPoint = getPoint(StartP.X + x, StartP.Y + y)
                                If (p IsNot Nothing) Then
                                    If (p.PathType = PathPoint.PointType.NOMORE) Then
                                        If (p IsNot Nothing) Then
                                            If (x = 0 Or y = 0) Then
                                                p.G = 10
                                            Else
                                                p.G = 14
                                            End If
                                            p.H = (Math.Abs(p.X - EndP.X) + Math.Abs(p.Y - EndP.Y)) * 10
                                            OpenList.Add(p)
                                            ' PathList.Add(p)
                                        End If
                                    End If
                                End If
                            Next
                        Next
                    End If
                End If
            End If
        Next
        Dim minF As PathPoint = getMinF(OpenList)
        minF.isFasted = True
        If (minF IsNot Nothing) Then
            OpenList.Remove(minF)
            ClosedList.Add(minF)
        End If
        ' Call FindOpenList()
        While minF IsNot Nothing
            Call CoursePoint(minF)
            minF = getMinF(OpenList)
            PathWays.Add(minF)
            'OpenList.Clear()
            If (minF Is Nothing) Then
                Exit While
            End If
            minF.isFasted = True
            ClosedList.Add(minF)
            OpenList.Remove(minF)
            Application.DoEvents()
        End While
        Call UpdateFGH(StartPoint)
        Dim ways As List(Of PathPoint) = GetFastArounds(EndPoint)
        ClosedList.Clear()
        ClosedList.Add(EndPoint)
        WayList = New List(Of PathPoint)
        While ways.Count > 0
            If (ways.Count = 1) Then
                minF = ways(0)
            Else
                minF = getMinF(ways)
            End If
            WayList.Add(minF)
            minF.isPathWay = True
            ClosedList.AddRange(ways)
            ways = GetFastArounds(minF)
            If (ways Is Nothing) Then
                Exit While
            End If
        End While
        'Exit Sub
        For Each p As PathPoint In PathList
            If (Not p.isPathWay) Then
                p.isFasted = False
                p.G = 0
                p.H = 0
                p.F = 0
            End If

        Next
        'minF = getMinF(MinPaths)
        'While minF IsNot Nothing
        'minF.isPathWay = True
        'MinPaths.Remove(minF)
        'minF = getMinF(MinPaths)
        'End While
    End Sub
    Private Sub UpdateFGH(target As PathPoint)
        For i As Integer = 0 To PathWays.Count - 1
            If (PathWays(i) IsNot Nothing) Then
                If (PathWays(i).X = target.X Or PathWays(i).Y = target.Y) Then
                    PathWays(i).G = 10
                Else
                    PathWays(i).G = 14
                End If
                PathWays(i).H = (Math.Abs(PathWays(i).X - target.X) + Math.Abs(PathWays(i).Y - target.Y)) * 10
            End If
        Next
    End Sub
    Private Function GetArounds(point As PathPoint) As List(Of PathPoint)
        Dim list As New List(Of PathPoint)
        'PathWays
        For y As Integer = -1 To 1
            For x As Integer = -1 To 1
                Dim p As PathPoint = getPoint(point.X + x, point.Y + y)
                If (p IsNot Nothing) Then
                    'If (p.isFasted) Then
                    'list.Add(p)
                    'End If
                    If (p.PathType = PathPoint.PointType.START Or p.PathType = PathPoint.PointType.END) Then
                        Return Nothing
                    Else
                        list.Add(p)
                    End If
                End If
            Next
        Next
        Return list
    End Function
    Private Function GetFastArounds(point As PathPoint) As List(Of PathPoint)
        Dim list As New List(Of PathPoint)
        'PathWays
        For y As Integer = -1 To 1
            For x As Integer = -1 To 1
                'If (y = 0 Or x = 0) Then
                Dim p As PathPoint = getPoint(point.X + x, point.Y + y)
                If (p IsNot Nothing And p IsNot point And ClosedList.IndexOf(p) = -1) Then
                    If (p.PathType = PathPoint.PointType.START Or p.PathType = PathPoint.PointType.END And p IsNot point) Then
                        Return Nothing
                    End If
                    If (p.isFasted) Then
                        list.Add(p)
                    End If
                End If
                ' End If

            Next
        Next
        Return list
    End Function
    Private Function getFGH(s As Point, e As Point) As FGH
        Dim gh_ As FGH
        If (s.X = e.X Or s.Y = e.Y) Then
            gh_.G = 10
        Else
            gh_.G = 14
        End If
        gh_.H = (Math.Abs(s.X - e.X) + Math.Abs(s.Y - e.Y)) * 10
        gh_.F = gh_.G + gh_.H
        Return gh_
    End Function
    Private Function getPoint(x As Integer, y As Integer) As PathPoint
        For Each PP As Object In PathList
            If (TypeOf (PP) Is PathPoint) Then
                If (CType(PP, PathPoint).X = x And CType(PP, PathPoint).Y = y) Then
                    Return PP
                End If
            End If
        Next
        Return Nothing
    End Function
    Public Sub CoursePoint(point As PathPoint)
        If (point Is Nothing) Then
            Exit Sub
        End If
        For y As Integer = -1 To 1
            For x As Integer = -1 To 1
                Dim p As PathPoint = getPoint(point.X + x, point.Y + y)
                If (p IsNot point And p IsNot Nothing) Then
                    If (p.PathType = PathPoint.PointType.NOMORE) Then
                        If (ClosedList.IndexOf(p) = -1) Then
                            If (OpenList.IndexOf(p) = -1) Then
                                Dim TempFGH As FGH = getFGH(New Point(p.X, p.Y), New Point(EndPoint.X, EndPoint.Y))
                                TempFGH = TempFGH
                                If (x = 0 Or y = 0) Then
                                    p.G = 10
                                Else
                                    p.G = 14
                                End If
                                p.H = (Math.Abs(p.X - EndPoint.X) + Math.Abs(p.Y - EndPoint.Y)) * 10
                                OpenList.Add(p)
                                'PathList.Add(p)
                            End If
                        End If
                    ElseIf p.PathType = PathPoint.PointType.END Then
                        OpenList.Clear()
                        ClosedList.Clear()
                        Exit Sub
                    End If
                End If
            Next
        Next
    End Sub
    Private Sub FindOpenList()
        While OpenList.Count > 0
            Dim Path As PathPoint = OpenList(0)
            OpenList.RemoveAt(0)
            ClosedList.Add(Path)
            For y As Integer = -1 To 1
                For x As Integer = -1 To 1
                    Dim p As PathPoint = getPoint(Path.X + x, Path.Y + y)
                    If (p IsNot Path And p IsNot Nothing) Then
                        If (p.PathType <> PathPoint.PointType.BLOCK) Then
                            If (ClosedList.IndexOf(p) = -1) Then
                                If (OpenList.IndexOf(p) = -1) Then
                                    Dim TempFGH As FGH = getFGH(New Point(p.X, p.Y), New Point(EndPoint.X, EndPoint.Y))
                                    TempFGH = TempFGH
                                    If (x = 0 Or y = 0) Then
                                        p.G = 10
                                    Else
                                        p.G = 14
                                    End If
                                    p.H = (Math.Abs(p.X - EndPoint.X) + Math.Abs(p.Y - EndPoint.Y)) * 10
                                    OpenList.Add(p)
                                    'PathList.Add(p)
                                End If
                            End If
                        End If
                    End If
                Next
            Next
            Application.DoEvents()
        End While
        Dim minF As PathPoint = getMinF(OpenList)
        minF.isFasted = True
        If (minF IsNot Nothing) Then
            OpenList.Remove(minF)
            ClosedList.Add(minF)
        End If
        Call FindOpenList()
    End Sub
End Module
