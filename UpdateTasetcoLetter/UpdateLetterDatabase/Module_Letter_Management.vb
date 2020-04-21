Imports System.Data.SQLite
Imports System.Data.OleDb
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid

Module Module_Letter_Management
    Public Function GET_ALL_TABLE_NAME_IN_DATABASE(ByVal link_database As String) As DataTable
        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & "; Version=3;")
        MYCONNECTION.Open()
        Dim CMD As New SQLiteCommand
        CMD.CommandText = "select name from sqlite_master where type='table' order by name"
        CMD.Connection = MYCONNECTION
        Dim RDR As SQLiteDataReader = CMD.ExecuteReader
        Dim DS As New DataTable
        DS.Load(RDR)
        RDR.Close()
        Return DS
    End Function

    Public Function SQL_QUERY_TO_DATATABLE(Link_database As String, sql_string As String) As DataTable
        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & Link_database & "; Version=3;")
        MYCONNECTION.Open()
        Dim CMD As New SQLiteCommand
        CMD.CommandText = sql_string
        CMD.Connection = MYCONNECTION
        Dim RDR As SQLiteDataReader = CMD.ExecuteReader
        Dim DT As New DataTable
        DT.Load(RDR)
        RDR.Close()
        MYCONNECTION.Close()
        Return DT
    End Function
    Public Function SQL_QUERY_TO_STRING(Link_database As String, sql_string As String) As String
        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & Link_database & "; Version=3;")
        MYCONNECTION.Open()
        Dim CMD As New SQLiteCommand
        CMD.CommandText = sql_string
        CMD.Connection = MYCONNECTION
        Dim RDR As SQLiteDataReader = CMD.ExecuteReader
        Dim DT As New DataTable
        Dim result As String = ""
        DT.Load(RDR)
        RDR.Close()
        MYCONNECTION.Close()
        If DT.Rows.Count > 0 Then
            result = DT.Rows(0).Item(0).ToString
        End If
        Return result
    End Function
    Public Function SQL_QUERY_TO_INTEGER(Link_database As String, sql_string As String) As Integer
        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & Link_database & "; Version=3;")
        MYCONNECTION.Open()
        Dim CMD As New SQLiteCommand
        CMD.CommandText = sql_string
        CMD.Connection = MYCONNECTION
        Dim RDR As SQLiteDataReader = CMD.ExecuteReader
        Dim DT As New DataTable
        DT.Load(RDR)
        RDR.Close()
        MYCONNECTION.Close()
        If DT.Rows.Count = 0 Then
            Return 0
        Else
            Return CInt(DT.Rows(0).Item(0).ToString)
        End If
    End Function
    Public Sub SQL_QUERY(Link_database As String, sql_string As String)
        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & Link_database & "; Version=3;")
        MYCONNECTION.Open()
        Dim CMD As New SQLiteCommand
        CMD.CommandText = sql_string
        CMD.Connection = MYCONNECTION
        Dim RDR As SQLiteDataReader = CMD.ExecuteReader
    End Sub
    Public Sub Error_handle()
        Call MsgBox("Contact To Nguyen Quang Huy - 1561501" & Chr(10) & Chr(10) & Err.Number & Err.Description, vbCritical, "Vendor Management - Message")
        Err.Clear()
    End Sub
    Public Sub LOAD_DATABASE_TASETCO_TO_GRIDVIEW(Link_database As String, table_name As String, str_Filter As String, str_Search As String, GridControl As GridControl, Gridview As GridView)
        Dim TimerStart As DateTime
        TimerStart = Now

        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim itemcoll(100) As String

        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & Link_database & "; Version=3;")
        MYCONNECTION.Open()
        Dim CMD As New SQLiteCommand
        Dim SQL_String As String = ""

        Select Case str_Filter
            Case "TODAY"
                SQL_String = "SELECT * FROM " & table_name & " WHERE [Sent_date] LIKE '" & Now().ToString("dd/MM/yyyy") & "' ORDER BY [Vendor_Name] ASC, CAST(SUBSTR(Sent_date, 7, 4) AS INT) DESC, CAST(SUBSTR(Sent_date, 4, 2) AS INT) DESC, CAST(SUBSTR(Sent_date, 1, 2) AS INT) DESC, [Bill_Number] ASC"
            Case "All"
                SQL_String = "SELECT * FROM " & table_name & " ORDER BY [Vendor_Name] ASC, CAST(SUBSTR(Sent_date, 7, 4) AS INT) DESC, CAST(SUBSTR(Sent_date, 4, 2) AS INT) DESC, CAST(SUBSTR(Sent_date, 1, 2) AS INT) DESC, [Bill_Number] ASC"
            Case "Completed"
                SQL_String = "SELECT * FROM " & table_name & " WHERE [Final_result] = 'Completed' ORDER BY [Vendor_Name] ASC, CAST(SUBSTR(Sent_date, 7, 4) AS INT) DESC, CAST(SUBSTR(Sent_date, 4, 2) AS INT) DESC, CAST(SUBSTR(Sent_date, 1, 2) AS INT) DESC, [Bill_Number] ASC"
            Case "Incomplete"
                SQL_String = "SELECT * FROM " & table_name & " WHERE [Final_result] = 'Incomplete' ORDER BY [Vendor_Name] ASC, CAST(SUBSTR(Sent_date, 7, 4) AS INT) DESC, CAST(SUBSTR(Sent_date, 4, 2) AS INT) DESC, CAST(SUBSTR(Sent_date, 1, 2) AS INT) DESC, [Bill_Number] ASC"
            Case "CROWN"
                SQL_String = "SELECT * FROM " & table_name & " WHERE [Final_result] = 'CROWN' ORDER BY [Vendor_Name] ASC, CAST(SUBSTR(Sent_date, 7, 4) AS INT) DESC, CAST(SUBSTR(Sent_date, 4, 2) AS INT) DESC, CAST(SUBSTR(Sent_date, 1, 2) AS INT) DESC, [Bill_Number] ASC"
            Case "Other"
                Dim str_filter_by_where As String = InputBox("Please input value for filter:", "Letter Management - Message")
                SQL_String = "SELECT * FROM " & table_name & " WHERE " & str_filter_by_where & " ORDER BY [Vendor_Name] ASC, CAST(SUBSTR(Sent_date, 7, 4) AS INT) DESC, CAST(SUBSTR(Sent_date, 4, 2) AS INT) DESC, CAST(SUBSTR(Sent_date, 1, 2) AS INT) DESC, [Bill_Number] ASC"
            Case ""
                Dim SQL_Str_Search As String = ""
                Dim CMD2 As New SQLiteCommand
                CMD2.CommandText = "SELECT * FROM " & table_name & " LIMIT 1"
                CMD2.Connection = MYCONNECTION
                Dim RDR2 As SQLiteDataReader = CMD2.ExecuteReader
                Dim DS2 As New DataTable
                DS2.Load(RDR2)
                RDR2.Close()
                For i = 0 To DS2.Columns.Count - 1
                    If SQL_Str_Search.Length = 0 Then
                        SQL_Str_Search = "[" & DS2.Columns(i).ColumnName.ToString() & "] LIKE '%" & str_Search & "%' "
                    Else
                        SQL_Str_Search = SQL_Str_Search & " OR [" & DS2.Columns(i).ColumnName.ToString() & "] LIKE '%" & str_Search & "%' "
                    End If
                Next
                SQL_String = "SELECT * FROM " & table_name & " WHERE " & SQL_Str_Search & " ORDER BY [Vendor_Name] ASC, CAST(SUBSTR(Sent_date, 7, 4) AS INT) DESC, CAST(SUBSTR(Sent_date, 4, 2) AS INT) DESC, CAST(SUBSTR(Sent_date, 1, 2) AS INT) DESC, [Bill_Number] ASC"
                SQL_Str_Search = ""
        End Select

        CMD.CommandText = SQL_String
        CMD.Connection = MYCONNECTION
        Dim RDR As SQLiteDataReader = CMD.ExecuteReader
        Dim DS As New DataTable
        DS.Load(RDR)
        RDR.Close()

        Gridview.Columns.Clear()
        GridControl.DataSource = DS

        Dim TimeSpent As System.TimeSpan
        TimeSpent = Now.Subtract(TimerStart)
    End Sub
    Function ConvertToUnSign(ByVal sContent As String) As String
        Dim i As Integer
        Dim intCode As Long
        Dim sChar As String
        Dim sConvert As String = ""
        'ConvertToUnSign = AscW(sContent)
        For i = 1 To Len(sContent)
            sChar = Mid(sContent, i, 1)
            If sChar <> "" Then
                intCode = AscW(sChar)
            End If
            Select Case intCode
                Case 273
                    sConvert = sConvert & "d"
                Case 272
                    sConvert = sConvert & "D"
                Case 224, 225, 226, 227, 259, 7841, 7843, 7845, 7847, 7849, 7851, 7853, 7855, 7857, 7859, 7861, 7863
                    sConvert = sConvert & "a"
                Case 192, 193, 194, 195, 258, 7840, 7842, 7844, 7846, 7848, 7850, 7852, 7854, 7856, 7858, 7860, 7862
                    sConvert = sConvert & "A"
                Case 232, 233, 234, 7865, 7867, 7869, 7871, 7873, 7875, 7877, 7879
                    sConvert = sConvert & "e"
                Case 200, 201, 202, 7864, 7866, 7868, 7870, 7872, 7874, 7876, 7878
                    sConvert = sConvert & "E"
                Case 236, 237, 297, 7881, 7883
                    sConvert = sConvert & "i"
                Case 204, 205, 296, 7880, 7882
                    sConvert = sConvert & "I"
                Case 242, 243, 244, 245, 417, 7885, 7887, 7889, 7891, 7893, 7895, 7897, 7899, 7901, 7903, 7905, 7907
                    sConvert = sConvert & "o"
                Case 210, 211, 212, 213, 416, 7884, 7886, 7888, 7890, 7892, 7894, 7896, 7898, 7900, 7902, 7904, 7906
                    sConvert = sConvert & "O"
                Case 249, 250, 361, 432, 7909, 7911, 7913, 7915, 7917, 7919, 7921
                    sConvert = sConvert & "u"
                Case 217, 218, 360, 431, 7908, 7910, 7912, 7914, 7916, 7918, 7920
                    sConvert = sConvert & "U"
                Case 253, 7923, 7925, 7927, 7929
                    sConvert = sConvert & "y"
                Case 221, 7922, 7924, 7926, 7928
                    sConvert = sConvert & "Y"
                Case Else
                    sConvert = sConvert & sChar
            End Select
        Next
        Return sConvert
    End Function
    Public Sub Wait(ByVal interval As Integer)
        Dim sw As New Stopwatch
        sw.Start()
        Do While sw.ElapsedMilliseconds < interval
            Application.DoEvents()
        Loop
        sw.Stop()
    End Sub
    Sub Capture_Screenshot_WebBrowser(WebBrowser1 As WebBrowser, Path_saved As String)
        Const ScrollbarWidth = 18
        Dim wbRect As Rectangle = WebBrowser1.ClientRectangle
        Dim wbBm As New Bitmap(WebBrowser1.ClientRectangle.Width - ScrollbarWidth, WebBrowser1.ClientRectangle.Height - ScrollbarWidth)
        Dim gwb As Graphics = Graphics.FromImage(wbBm)
        gwb.CopyFromScreen(WebBrowser1.PointToScreen(New Point(0, 0)), New Point(0, 0), New Size(WebBrowser1.Width - ScrollbarWidth, WebBrowser1.Height - ScrollbarWidth))
        gwb.Dispose()
        wbBm.Save(Path_saved)
        wbBm.Dispose()
    End Sub


End Module
