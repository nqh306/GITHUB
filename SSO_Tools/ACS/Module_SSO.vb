Imports System.Data.SQLite
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Grid
Imports Microsoft.Data.Sqlite.Extensions
Imports Microsoft.Office.Interop
Imports System.Data.OleDb
Imports System.IO.Compression
Imports System.Text
Imports DevExpress.XtraPrinting
Imports System.IO

Module Module_SSO
    Public EncryptDecrypt As New Simple3Des("0915330999")
    Public check_password As Boolean
    Public appPath As String = AppDomain.CurrentDomain.BaseDirectory
    Public local_app_config As String = appPath & "App_Config.txt"

    Public Sub SetPasswordForNewDatabase(link_database As String)
        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;")
        MYCONNECTION.Open()
        MYCONNECTION.ChangePassword("0915330999")
        MYCONNECTION.Close()
    End Sub

    Public Sub ShowForm(ByVal _childForm As Form)
        Dim objForms As Form
        Dim _parrentForm As Form = MainForm
        For Each objForms In _parrentForm.MdiChildren
            If objForms.Name = _childForm.Name Then
                _childForm.Dispose()
                _childForm = Nothing
                objForms.Show()
                objForms.Visible = True
                objForms.Focus()
                Return
            End If
        Next
        With _childForm
            .MdiParent = _parrentForm
            .Show()
        End With
        _parrentForm.WindowState = FormWindowState.Maximized

    End Sub
    Public Sub WriteErrorLog(strErrorText As String)
        Try
            Dim global_app_config As String = SQL_FROMFILE_TO_STRING_NO_LOG(local_app_config, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Link_Global_Config'")
            Dim strPath_Log As String = SQL_FROMFILE_TO_STRING_NO_LOG(global_app_config, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Link_Folder_Database'")

            If strPath_Log.Substring(strPath_Log.Length - 1, 1) <> "\" Then
                strPath_Log = strPath_Log & "\"
            End If
            strPath_Log = strPath_Log & "LOG\Error\"

            If (Not System.IO.Directory.Exists(strPath_Log)) Then
                System.IO.Directory.CreateDirectory(strPath_Log)
            End If

            Dim strFileName As String = "errorLog_" & Environment.UserName & ".txt"
            System.IO.File.AppendAllText(strPath_Log & strFileName, (DateTime.Now.ToString() & Convert.ToString(" - ")) + strErrorText + vbCr + vbLf)
            WriteLog_Full((DateTime.Now.ToString() & Convert.ToString(" - ")) + strErrorText + vbCr + vbLf)
        Catch ex As Exception
            WriteErrorLog("Error in WriteErrorLog: " + ex.Message)
        End Try
    End Sub

    Public Sub WriteLog_Full(strText As String)
        Try
            Dim global_app_config As String = SQL_FROMFILE_TO_STRING_NO_LOG(local_app_config, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Link_Global_Config'")
            Dim strPath_Log As String = SQL_FROMFILE_TO_STRING_NO_LOG(global_app_config, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Link_Folder_Database'")

            Dim strFileName As String = "Log_ACS_" & Environment.UserName & "_" & Now.ToString("yyyyMMdd") & ".txt"
            If strPath_Log.Substring(strPath_Log.Length - 1, 1) <> "\" Then
                strPath_Log = strPath_Log & "\"
            End If
            strPath_Log = strPath_Log & "LOG\Full\"

            If (Not System.IO.Directory.Exists(strPath_Log)) Then
                System.IO.Directory.CreateDirectory(strPath_Log)
            End If

            System.IO.File.AppendAllText(strPath_Log & strFileName, (DateTime.Now.ToString() & Convert.ToString(" - ")) + strText + vbLf + vbLf)

        Catch ex As Exception
            WriteErrorLog("Error in WriteErrorLog: " + ex.Message)
        End Try
    End Sub

    Public Sub WriteLog(strText As String)
        Try
            Dim global_app_config As String = SQL_FROMFILE_TO_STRING_NO_LOG(local_app_config, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Link_Global_Config'")
            Dim strPath_Log As String = SQL_FROMFILE_TO_STRING_NO_LOG(global_app_config, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Link_Folder_Database'")

            Dim strFileName As String = "Log_ACS_" & Environment.UserName & "_" & Now.ToString("yyyyMMdd") & ".txt"
            If strPath_Log.Substring(strPath_Log.Length - 1, 1) <> "\" Then
                strPath_Log = strPath_Log & "\"
            End If
            strPath_Log = strPath_Log & "LOG\"

            If (Not System.IO.Directory.Exists(strPath_Log)) Then
                System.IO.Directory.CreateDirectory(strPath_Log)
            End If

            System.IO.File.AppendAllText(strPath_Log & strFileName, (DateTime.Now.ToString() & Convert.ToString(" - ")) + strText + vbLf + vbLf)

        Catch ex As Exception
            WriteErrorLog("Error in WriteErrorLog: " + ex.Message)
        End Try
    End Sub

    Public Function SQL_QUERY_TO_DATATABLE(MYCONNECTION As SQLiteConnection, sql_string As String) As DataTable
        Try
            Dim CMD As New SQLiteCommand
            CMD.CommandText = sql_string
            CMD.Connection = MYCONNECTION
            Dim RDR As SQLiteDataReader = CMD.ExecuteReader
            Dim DT As New DataTable
            DT.Load(RDR)
            RDR.Close()
            WriteLog_Full("[SQL QUERY TO DATATABLE] - " & sql_string & vbLf & " - Link database: " & Replace(MYCONNECTION.ConnectionString.ToString, ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;", "") & vbLf & " - Result: SUCCESFUL")
            Return DT
        Catch ex As Exception
            WriteErrorLog("[SQL QUERY TO DATATABLE] - " & sql_string & vbLf & " - Link database: " & Replace(MYCONNECTION.ConnectionString.ToString, ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;", "") & vbLf & " - Error Message: " & ex.ToString)
            MsgBox(ex.Message, vbCritical)
            Return Nothing
        End Try
    End Function

    Public Function SQL_FROMFILE_TO_DATATABLE(link_database As String, sql_string As String) As DataTable
        Try
            Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;")
            MYCONNECTION.Open()
            Dim CMD As New SQLiteCommand
            CMD.CommandText = sql_string
            CMD.Connection = MYCONNECTION
            Dim RDR As SQLiteDataReader = CMD.ExecuteReader
            Dim DT As New DataTable
            DT.Load(RDR)
            RDR.Close()
            WriteLog_Full("[SQL QUERY TO DATATABLE] - " & sql_string & vbLf & "Link database: " & link_database & vbLf & " - Result: SUCCESFUL")
            Return DT
        Catch ex As Exception
            WriteErrorLog("[SQL QUERY TO DATATABLE] - " & sql_string & vbLf & "Link database: " & link_database & vbLf & " - Error Message: " & ex.ToString)
            MsgBox(ex.Message, vbCritical)
            Return Nothing
        End Try
    End Function

    Public Function SQL_QUERY_TO_STRING(MYCONNECTION As SQLiteConnection, sql_string As String) As String
        Try
            Dim CMD As New SQLiteCommand
            CMD.CommandText = sql_string
            CMD.Connection = MYCONNECTION
            Dim RDR As SQLiteDataReader = CMD.ExecuteReader
            Dim DT As New DataTable
            Dim result As String = ""
            DT.Load(RDR)
            RDR.Close()
            If DT.Rows.Count > 0 Then
                result = DT.Rows(0).Item(0).ToString
            End If
            WriteLog_Full("[SQL QUERY TO STRING] - " & sql_string & vbLf & " - Link database: " & Replace(MYCONNECTION.ConnectionString.ToString, ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;", "") & vbLf & " - Result: " & result)
            Return result
        Catch ex As Exception
            WriteErrorLog("[SQL QUERY TO STRING] - " & sql_string & vbLf & " - Link database: " & Replace(MYCONNECTION.ConnectionString.ToString, ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;", "") & vbLf & " - Error Message: " & ex.ToString)
            MsgBox(ex.Message, vbCritical)
            Return Nothing
        End Try
    End Function

    Public Function SQL_QUERY_TO_DATE(MYCONNECTION As SQLiteConnection, sql_string As String) As DateTime
        Try
            Dim CMD As New SQLiteCommand
            CMD.CommandText = sql_string
            CMD.Connection = MYCONNECTION
            Dim RDR As SQLiteDataReader = CMD.ExecuteReader
            Dim DT As New DataTable
            Dim result As DateTime = "01/01/1900"
            DT.Load(RDR)
            RDR.Close()
            If DT.Rows.Count > 0 Then
                result = DT.Rows(0).Item(0)
            End If
            WriteLog_Full("[SQL QUERY TO DATE] - " & sql_string & vbLf & " - Link database: " & Replace(MYCONNECTION.ConnectionString.ToString, ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;", "") & vbLf & " - Result: " & result)
            Return result
        Catch ex As Exception
            WriteErrorLog("[SQL QUERY TO DATE] - " & sql_string & vbLf & " - Link database: " & Replace(MYCONNECTION.ConnectionString.ToString, ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;", "") & vbLf & " - Error Message: " & ex.ToString)
            MsgBox(ex.Message, vbCritical)
            Return Nothing
        End Try
    End Function

    Public Function SQL_QUERY_TO_INTEGER(MYCONNECTION As SQLiteConnection, sql_string As String) As Integer
        Try
            Dim CMD As New SQLiteCommand
            CMD.CommandText = sql_string
            CMD.Connection = MYCONNECTION
            Dim RDR As SQLiteDataReader = CMD.ExecuteReader
            Dim DT As New DataTable
            DT.Load(RDR)
            RDR.Close()
            Dim result As Integer
            If DT.Rows.Count = 0 Then
                result = 0
            Else
                If DT.Rows(0).Item(0).ToString.Length = 0 Then
                    result = 0
                Else
                    result = CInt(DT.Rows(0).Item(0).ToString)
                End If
            End If
            WriteLog_Full("[SQL QUERY TO INTEGER] - " & sql_string & vbLf & " - Link database: " & Replace(MYCONNECTION.ConnectionString.ToString, ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;", "") & vbLf & " - Result: " & result)
            Return result
        Catch ex As Exception
            WriteErrorLog("[SQL QUERY TO INTEGER] - " & sql_string & vbLf & " - Link database: " & Replace(MYCONNECTION.ConnectionString.ToString, ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;", "") & vbLf & " - Error Message: " & ex.ToString)
            MsgBox(ex.Message, vbCritical)
            Return Nothing
        End Try
    End Function

    Public Function SQL_QUERY_TO_LONG(MYCONNECTION As SQLiteConnection, sql_string As String) As Long
        Try
            Dim CMD As New SQLiteCommand
            CMD.CommandText = sql_string
            CMD.Connection = MYCONNECTION
            Dim RDR As SQLiteDataReader = CMD.ExecuteReader
            Dim DT As New DataTable
            DT.Load(RDR)
            RDR.Close()
            Dim result As Long
            If DT.Rows.Count = 0 Then
                result = 0
            Else
                If DT.Rows(0).Item(0).ToString.Length = 0 Then
                    result = 0
                Else
                    result = DT.Rows(0).Item(0)
                End If
            End If
            WriteLog_Full("[SQL QUERY TO LONG] - " & sql_string & vbLf & " - Link database: " & Replace(Replace(MYCONNECTION.ConnectionString.ToString, ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;", ""), "0915330999", "") & vbLf & " - Result: " & result)
            Return result
        Catch ex As Exception
            WriteErrorLog("[SQL QUERY TO LONG] - " & sql_string & vbLf & " - Link database: " & Replace(Replace(MYCONNECTION.ConnectionString.ToString, ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;", ""), "0915330999", "") & vbLf & " - Error Message: " & ex.ToString)
            MsgBox(ex.Message, vbCritical)
            Return Nothing
        End Try
    End Function

    Public Function SQL_QUERY_TO_INTEGER_NO_LOG(MYCONNECTION As SQLiteConnection, sql_string As String) As Integer
        Try
            Dim CMD As New SQLiteCommand
            CMD.CommandText = sql_string
            CMD.Connection = MYCONNECTION
            Dim RDR As SQLiteDataReader = CMD.ExecuteReader
            Dim DT As New DataTable
            DT.Load(RDR)
            RDR.Close()
            Dim result As Integer
            If DT.Rows.Count = 0 Then
                result = 0
            Else
                If DT.Rows(0).Item(0).ToString.Length = 0 Then
                    result = 0
                Else
                    result = CInt(DT.Rows(0).Item(0).ToString)
                End If
            End If
            Return result
        Catch ex As Exception
            MsgBox(ex.Message, vbCritical)
            Return Nothing
        End Try
    End Function

    Public Function SQL_QUERY_COUNT_ROW(MYCONNECTION As SQLiteConnection, sql_string As String) As Integer
        Try
            Dim CMD As New SQLiteCommand
            CMD.CommandText = sql_string
            CMD.Connection = MYCONNECTION
            Dim RDR As SQLiteDataReader = CMD.ExecuteReader
            Dim DT As New DataTable
            DT.Load(RDR)
            RDR.Close()
            WriteLog_Full("[SQL COUNT ROW] - " & sql_string & vbLf & " - Link database: " & Replace(MYCONNECTION.ConnectionString.ToString, ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;", "") & vbLf & " - Result: " & DT.Rows.Count)
            Return DT.Rows.Count
        Catch ex As Exception
            WriteErrorLog("[SQL COUNT ROW] - " & sql_string & vbLf & " - Link database: " & Replace(MYCONNECTION.ConnectionString.ToString, ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;", "") & vbLf & " - Error Message: " & ex.ToString)
            MsgBox(ex.Message, vbCritical)
            Return Nothing
        End Try
    End Function

    Public Function SQL_FROMFILE_COUNT_ROW(link_database As String, sql_string As String) As Integer
        Try
            Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;")
            MYCONNECTION.Open()

            Dim CMD As New SQLiteCommand
            CMD.CommandText = sql_string
            CMD.Connection = MYCONNECTION
            Dim RDR As SQLiteDataReader = CMD.ExecuteReader
            Dim DT As New DataTable
            DT.Load(RDR)
            RDR.Close()
            WriteLog_Full("[SQL COUNT ROW] - " & sql_string & vbLf & " - Link database: " & link_database & vbLf & " - Result: " & DT.Rows.Count)
            Return DT.Rows.Count
        Catch ex As Exception
            WriteErrorLog("[SQL COUNT ROW] - " & sql_string & vbLf & " - Link database: " & link_database & vbLf & " - Error Message: " & ex.ToString)
            MsgBox(ex.Message, vbCritical)
            Return Nothing
        End Try
    End Function

    Public Function SQL_QUERY_TO_BOOLEAN(MYCONNECTION As SQLiteConnection, sql_string As String) As Boolean
        Try
            Dim CMD As New SQLiteCommand
            CMD.CommandText = sql_string
            CMD.Connection = MYCONNECTION
            Dim RDR As SQLiteDataReader = CMD.ExecuteReader
            Dim DT As New DataTable
            DT.Load(RDR)
            RDR.Close()
            Dim result As Boolean
            If DT.Rows.Count = 0 Then
                result = False
            Else
                If DT.Rows(0).Item(0).ToString.Length = 0 Then
                    result = False
                Else
                    result = DT.Rows(0).Item(0)
                End If
            End If
            WriteLog_Full("[SQL QUERY TO BOOLEAN] - " & sql_string & vbLf & " - Link database: " & Replace(MYCONNECTION.ConnectionString.ToString, ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;", "") & vbLf & " - Result: " & result)
            Return result
        Catch ex As Exception
            WriteErrorLog("[SQL QUERY TO BOOLEAN] - " & sql_string & vbLf & " - Link database: " & Replace(MYCONNECTION.ConnectionString.ToString, ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;", "") & vbLf & " - Error Message: " & ex.ToString)
            MsgBox(ex.Message, vbCritical)
            Return Nothing
        End Try
    End Function

    Public Sub SQLITE_BULK_COPY(DataTable As DataTable, link_database As String, table_name As String)
        Dim TimerStart As DateTime = Now
        Dim showmsg As Boolean = False
        Dim err_msg As String = ""

        Do
            Try
                Dim datareader As New DataTableReader(DataTable)

                Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;")

                Dim BulkCopy As SqliteBulkCopy = New SqliteBulkCopy(MYCONNECTION)

                BulkCopy.DestinationTableName = table_name

                BulkCopy.ColumnMappings.Clear()

                For i As Integer = 0 To DataTable.Columns.Count - 1
                    WriteLog_Full("[SQLITE BULK COPY] - Table name: " & table_name & " - Columns: " & DataTable.Columns(i).ColumnName.ToString() & vbLf & " - Link database: " & Replace(MYCONNECTION.ConnectionString.ToString, ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;", ""))
                    BulkCopy.ColumnMappings.Add(DataTable.Columns(i).ColumnName.ToString(), 0)
                Next

                BulkCopy.WriteToServer(datareader)

                Dim TimeSpent As System.TimeSpan
                TimeSpent = Now.Subtract(TimerStart)
                WriteLog_Full("[SQLITE BULK COPY] - Successful - Table name: " & table_name & " - in " & Format(TimeSpent.TotalSeconds, "0.00") & " seconds" & vbLf & " - Link database: " & link_database)
                Exit Do
            Catch ex As Exception
                WriteLog_Full("[SQLITE BULK COPY] - Table name: " & table_name & vbLf & " - Link database: " & link_database & vbLf & "Error Message: " & Err.ToString)
                err_msg = ex.ToString
                If err_msg.Contains("used by another process") = True Then
                    showmsg = False
                Else
                    If err_msg.Contains("locked") = True Then
                        showmsg = False
                    Else
                        Exit Do
                        showmsg = True
                    End If
                End If

                Dim TimeSpent As System.TimeSpan = Now.Subtract(TimerStart)
                If Format(TimeSpent.TotalSeconds, "0.00") > 60 Then
                    Exit Do
                End If
            End Try
        Loop
        If showmsg = True Then
            MsgBox(err_msg, vbCritical)
        End If
    End Sub


    'Public Sub SQLITE_BULK_COPY(DataTable As DataTable, link_database As String, table_name As String)
    '    Dim TimerStart As DateTime
    '    TimerStart = Now

    '    WriteLog_Full("[SQLITE BULK COPY] - Table name: " & table_name & vbLf & " - Link database: " & link_database)

    '    Dim datareader As New DataTableReader(DataTable)

    '    Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;")

    '    Dim BulkCopy As SqliteBulkCopy = New SqliteBulkCopy(MYCONNECTION)

    '    BulkCopy.DestinationTableName = table_name

    '    BulkCopy.ColumnMappings.Clear()

    '    For i As Integer = 0 To DataTable.Columns.Count - 1
    '        WriteLog_Full("[SQLITE BULK COPY] - Table name: " & table_name & " - Columns: " & DataTable.Columns(i).ColumnName.ToString() & vbLf & " - Link database: " & Replace(MYCONNECTION.ConnectionString.ToString, ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;", ""))
    '        BulkCopy.ColumnMappings.Add(DataTable.Columns(i).ColumnName.ToString(), 0)
    '    Next

    '    BulkCopy.WriteToServer(datareader)

    '    Dim TimeSpent As System.TimeSpan
    '    TimeSpent = Now.Subtract(TimerStart)
    '    WriteLog_Full("[SQLITE BULK COPY] - Successful - Table name: " & table_name & " - in " & Format(TimeSpent.TotalSeconds, "0.00") & " seconds" & vbLf & " - Link database: " & Replace(MYCONNECTION.ConnectionString.ToString, ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;", ""))
    'End Sub

    'Public Sub SQL_QUERY(MYCONNECTION As SQLiteConnection, sql_string As String)
    '    WriteLog_Full("[SQL QUERY] - " & sql_string & vbLf & " - Link database: " & Replace(MYCONNECTION.ConnectionString.ToString, ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;", ""))
    '    Dim CMD As New SQLiteCommand
    '    CMD.CommandText = sql_string
    '    CMD.Connection = MYCONNECTION
    '    Dim RDR As SQLiteDataReader = CMD.ExecuteReader
    '    WriteLog_Full("[SQL QUERY] - " & sql_string & vbLf & " - Link database: " & Replace(MYCONNECTION.ConnectionString.ToString, ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;", "") & " - SUCCESSFUL")
    'End Sub

    Public Sub SQL_QUERY(MYCONNECTION As SQLiteConnection, sql_string As String)
        Dim TimerStart As DateTime = Now
        Dim showmsg As Boolean = False
        Dim err_msg As String = ""

        Do
            Try
                Dim CMD As New SQLiteCommand
                CMD.CommandText = sql_string
                CMD.Connection = MYCONNECTION
                Dim RDR As SQLiteDataReader = CMD.ExecuteReader

                WriteLog_Full("[SQL QUERY] - " & sql_string & vbLf & " - Link database: " & MYCONNECTION.ConnectionString.ToString & vbLf & " - Result: SUCCESSFUL")
                Exit Do
            Catch ex As Exception
                WriteErrorLog("[SQL QUERY] - " & sql_string & vbLf & " - Link database: " & MYCONNECTION.ConnectionString.ToString & vbLf & " - Error Reason: " & ex.ToString)
                err_msg = ex.ToString

                If err_msg.Contains("used by another process") = True Then
                    showmsg = False
                Else
                    If err_msg.Contains("locked") = True Then
                        showmsg = False
                    Else
                        Exit Do
                        showmsg = True
                    End If
                End If

                Dim TimeSpent As System.TimeSpan = Now.Subtract(TimerStart)
                If Format(TimeSpent.TotalSeconds, "0.00") > 60 Then
                    Exit Do
                End If

            End Try
        Loop
        If showmsg = True Then
            MsgBox(err_msg, vbCritical)
        End If
    End Sub

    Public Sub SQL_QUERY_FROM_FILE(LINK_DATABASE As String, sql_string As String)
        Dim TimerStart As DateTime = Now
        Dim showmsg As Boolean = False
        Dim err_msg As String = ""

        Do
            Try
                Dim MYCONNECTION As SQLiteConnection
                MYCONNECTION = New SQLiteConnection("DataSource=" & LINK_DATABASE & ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;")
                MYCONNECTION.Open()
                Dim CMD As New SQLiteCommand
                CMD.CommandText = sql_string
                CMD.Connection = MYCONNECTION
                Dim RDR As SQLiteDataReader = CMD.ExecuteReader

                WriteLog_Full("[SQL QUERY] - " & sql_string & vbLf & " - Link database: " & LINK_DATABASE & vbLf & " - Result: SUCCESSFUL")
                Exit Do
            Catch ex As Exception
                WriteErrorLog("[SQL QUERY] - " & sql_string & vbLf & " - Link database: " & LINK_DATABASE & vbLf & " - Error Reason: " & ex.ToString)
                err_msg = ex.ToString

                If err_msg.Contains("used by another process") = True Then
                    showmsg = False
                Else
                    If err_msg.Contains("locked") = True Then
                        showmsg = False
                    Else
                        Exit Do
                        showmsg = True
                    End If
                End If

                Dim TimeSpent As System.TimeSpan = Now.Subtract(TimerStart)
                If Format(TimeSpent.TotalSeconds, "0.00") > 60 Then
                    Exit Do
                End If

            End Try
        Loop
        If showmsg = True Then
            MsgBox(err_msg, vbCritical)
        End If
    End Sub

    'Public Sub SQL_QUERY_FROM_FILE(LINK_DATABASE As String, sql_string As String)
    '    Dim MYCONNECTION As SQLiteConnection
    '    MYCONNECTION = New SQLiteConnection("DataSource=" & LINK_DATABASE & ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;")
    '    MYCONNECTION.Open()
    '    WriteLog_Full("[SQL QUERY] - " & sql_string & vbLf & " - Link database: " & Replace(MYCONNECTION.ConnectionString.ToString, ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;", ""))

    '    Dim CMD As New SQLiteCommand
    '    CMD.CommandText = sql_string
    '    CMD.Connection = MYCONNECTION
    '    Dim RDR As SQLiteDataReader = CMD.ExecuteReader
    '    WriteLog_Full("[SQL QUERY] - " & sql_string & vbLf & " - Link database: " & Replace(MYCONNECTION.ConnectionString.ToString, ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;", "") & " - SUCCESSFUL")
    'End Sub

    Public Sub SQL_QUERY_FROM_FILE_NO_LOG(LINK_DATABASE As String, sql_string As String)
        Try
            Dim MYCONNECTION As SQLiteConnection
            MYCONNECTION = New SQLiteConnection("DataSource=" & LINK_DATABASE & ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;")
            MYCONNECTION.Open()

            Dim CMD As New SQLiteCommand
            CMD.CommandText = sql_string
            CMD.Connection = MYCONNECTION
            Dim RDR As SQLiteDataReader = CMD.ExecuteReader
        Catch ex As Exception
            Dim MYCONNECTION As SQLiteConnection
            MYCONNECTION = New SQLiteConnection("DataSource=" & LINK_DATABASE & ";version=3;new=False;datetimeformat=CurrentCulture;")
            MYCONNECTION.Open()

            Dim CMD As New SQLiteCommand
            CMD.CommandText = sql_string
            CMD.Connection = MYCONNECTION
            Dim RDR As SQLiteDataReader = CMD.ExecuteReader
        End Try

    End Sub

    Public Function MonthDifference(ByVal first As DateTime, ByVal second As DateTime) As Integer
        Return Math.Abs((first.Month - second.Month) + 12 * (first.Year - second.Year))
    End Function

    Public Function GridviewToDataTable(ByVal gridView1 As GridView) As DataTable
        Dim dt As New DataTable()
        For Each column As GridColumn In gridView1.VisibleColumns
            dt.Columns.Add(column.FieldName, column.ColumnType)
        Next column
        For i As Integer = 0 To gridView1.DataRowCount - 1
            Dim row As DataRow = dt.NewRow()
            For Each column As GridColumn In gridView1.VisibleColumns
                row(column.FieldName) = gridView1.GetRowCellValue(i, column)
            Next column
            dt.Rows.Add(row)
        Next i

        Return dt
    End Function

    Public Function GET_ALL_TABLE_NAME_IN_DATABASE(ByVal MYCONNECTION As SQLiteConnection) As DataTable
        Dim CMD As New SQLiteCommand
        CMD.CommandText = "select name from sqlite_master where type='table' order by name"
        CMD.Connection = MYCONNECTION
        Dim RDR As SQLiteDataReader = CMD.ExecuteReader
        Dim DS As New DataTable
        DS.Load(RDR)
        RDR.Close()
        Return DS
    End Function

    Public Function Check_table_exists(ByVal MYCONNECTION As SQLiteConnection, TABLE_NAME As String) As Boolean
        Try
            Dim DT As DataTable = GET_ALL_TABLE_NAME_IN_DATABASE(MYCONNECTION)
            Dim result As Boolean = False

            If DT.Rows.Count > 0 Then
                For i As Integer = 0 To DT.Rows.Count - 1
                    If DT.Rows(i).Item(0).ToString() = TABLE_NAME Then
                        result = True
                        Exit For
                    End If
                Next
            End If
            WriteLog_Full("[CHECK TABLE EXISTS] - " & TABLE_NAME & vbLf & " - Link database: " & Replace(MYCONNECTION.ConnectionString.ToString, ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;", "") & vbLf & " - Result: " & result)
            Return result
        Catch ex As Exception
            WriteErrorLog("[CHECK TABLE EXISTS] - " & TABLE_NAME & vbLf & " - Link database: " & Replace(MYCONNECTION.ConnectionString.ToString, ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;", "") & vbLf & " - Error Message: " & ex.ToString)
            MsgBox(ex.Message, vbCritical)
            Return Nothing
        End Try
    End Function

    Public Function SQL_FROMFILE_TO_STRING(link_database As String, sql_string As String) As String
        Try
            Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;")
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
            WriteLog_Full("[SQL QUERY TO STRING] - " & sql_string & vbLf & " - Link database: " & link_database & vbLf & " - Result: " & result)
            Return result
        Catch ex As Exception
            WriteErrorLog("[SQL QUERY TO STRING] - " & sql_string & vbLf & " - Link database: " & link_database & vbLf & " - Error Message: " & ex.ToString)
            MsgBox(ex.Message, vbCritical)
            Return Nothing
        End Try

    End Function

    Public Function SQL_FROMFILE_TO_STRING_NO_LOG(link_database As String, sql_string As String) As String
        Try
            Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;")
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
        Catch ex As Exception
            MsgBox(ex.Message, vbCritical)
            Return Nothing
        End Try
    End Function

    Public Function SQL_FROMFILE_TO_INTEGER(link_database As String, sql_string As String) As Integer
        Try
            Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;")
            MYCONNECTION.Open()

            Dim CMD As New SQLiteCommand
            CMD.CommandText = sql_string
            CMD.Connection = MYCONNECTION
            Dim RDR As SQLiteDataReader = CMD.ExecuteReader
            Dim DT As New DataTable
            DT.Load(RDR)
            RDR.Close()
            MYCONNECTION.Close()
            Dim result As Integer
            If DT.Rows.Count = 0 Then
                result = 0
            Else
                If DT.Rows(0).Item(0).ToString.Length = 0 Then
                    result = 0
                Else
                    result = CInt(DT.Rows(0).Item(0).ToString)
                End If
            End If
            WriteLog_Full("[SQL QUERY TO INTEGER] - " & sql_string & vbLf & " - Link database: " & link_database & vbLf & " - Result: " & result)
            Return result
        Catch ex As Exception
            WriteErrorLog("[SQL QUERY TO INTEGER] - " & sql_string & vbLf & " - Link database: " & link_database & vbLf & " - Error Message: " & ex.ToString)
            MsgBox(ex.Message, vbCritical)
            Return Nothing
        End Try

    End Function

    Public Function SQL_FROMFILE_TO_INTEGER_NO_LOG(link_database As String, sql_string As String) As Integer
        Try
            Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;")
            MYCONNECTION.Open()

            Dim CMD As New SQLiteCommand
            CMD.CommandText = sql_string
            CMD.Connection = MYCONNECTION
            Dim RDR As SQLiteDataReader = CMD.ExecuteReader
            Dim DT As New DataTable
            DT.Load(RDR)
            RDR.Close()
            MYCONNECTION.Close()
            Dim result As Integer
            If DT.Rows.Count = 0 Then
                result = 0
            Else
                If DT.Rows(0).Item(0).ToString.Length = 0 Then
                    result = 0
                Else
                    result = CInt(DT.Rows(0).Item(0).ToString)
                End If
            End If
            Return result
        Catch ex As Exception
            MsgBox(ex.Message, vbCritical)
            Return Nothing
        End Try
    End Function

    Public Sub EXPORT_GRIDVIEW_TO_EXCEL(GRIDVIEW As GridView)

        Dim full_path As String = appPath & Now.ToString("ddMMyyyyhhmmssttttt") & ".xls"
        GRIDVIEW.ExportToXls(full_path)

        Dim ObjExcel As Object = CreateObject("Excel.Application")
        ObjExcel.Visible = False
        Dim Objwb As Object = ObjExcel.Workbooks.open(full_path)
        Dim Objws As Object = Objwb.Sheets("Sheet")

        Dim Objwb_dest As Object = ObjExcel.Workbooks.Add()
        Dim Objws_dest As Object = Objwb_dest.ActiveSheet()
        Objws_dest.Cells.NumberFormat = "@"

        Objws.Cells.Copy()
        Objws_dest.Range("A1").PasteSpecial(Excel.XlPasteType.xlPasteAll)
        ObjExcel.CutCopyMode = False

        Objws_dest.Cells.Borders.LineStyle = Excel.XlLineStyle.xlLineStyleNone

        Objwb.Close(SaveChanges:=False)
        Kill(full_path)
        ObjExcel.Visible = True
    End Sub

    Public Function CSV_TO_DATATABLE_USING_TextFieldParser(csvFilePath As String) As DataTable
        Dim TextFileReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(csvFilePath)

        TextFileReader.TextFieldType = FileIO.FieldType.Delimited
        TextFileReader.SetDelimiters(New String() {vbTab})

        Dim TextFileTable As DataTable = Nothing

        Dim Column As DataColumn
        Dim Row As DataRow
        Dim UpperBound As Int32
        Dim ColumnCount As Int32
        Dim CurrentRow As String()

        While Not TextFileReader.EndOfData
            Try
                CurrentRow = TextFileReader.ReadFields()
                If Not CurrentRow Is Nothing Then
                    ''# Check if DataTable has been created
                    If TextFileTable Is Nothing Then
                        TextFileTable = New DataTable("TextFileTable")
                        ''# Get number of columns
                        UpperBound = CurrentRow.GetUpperBound(0)
                        ''# Create new DataTable
                        For ColumnCount = 0 To UpperBound
                            Column = New DataColumn()
                            Column.DataType = System.Type.GetType("System.String")
                            Column.ColumnName = "Column" & ColumnCount
                            Column.Caption = "Column" & ColumnCount
                            Column.ReadOnly = True
                            Column.Unique = False
                            TextFileTable.Columns.Add(Column)
                        Next
                    End If
                    Row = TextFileTable.NewRow
                    For ColumnCount = 0 To UpperBound
                        Row("Column" & ColumnCount) = CurrentRow(ColumnCount).ToString
                    Next
                    TextFileTable.Rows.Add(Row)
                End If
            Catch ex As _
            Microsoft.VisualBasic.FileIO.MalformedLineException
                MsgBox("Line " & ex.Message &
                "is not valid and will be skipped.")
            End Try
        End While
        TextFileReader.Dispose()
        Return TextFileTable
    End Function

    Public Function SQL_CSV_TO_DATATABLE(csvFilePath As String, csvFileName As String, SQL_STRING As String) As DataTable
        Try
            WriteLog_Full("[GET DATA FROM CSV] - " & SQL_STRING & vbLf & " - csvFilePath: " & csvFilePath & "- csvFileName: " & csvFileName)

            Dim conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & csvFilePath & ";Extended Properties='text;HDR=YES;FMT=TabDelimited'")
            conn.Open()
            Dim cmd2 As OleDbCommand = New OleDbCommand(SQL_STRING, conn)
            Dim da As OleDbDataAdapter = New OleDbDataAdapter(cmd2)
            Dim dt As DataTable = New DataTable
            da.Fill(dt)
            conn.Close()
            WriteLog_Full("[GET DATA FROM CSV] - " & SQL_STRING & vbLf & " - csvFilePath: " & csvFilePath & "- csvFileName: " & csvFileName & vbLf & "-Result: Get DataTable successful")
            Return dt
        Catch ex As Exception
            WriteErrorLog("[GET DATA FROM CSV] - " & SQL_STRING & vbLf & " - csvFilePath: " & csvFilePath & "- csvFileName: " & csvFileName & vbLf & " - Error Message: " & ex.ToString)
            MsgBox(ex.Message, vbCritical)
            Return Nothing
        End Try

    End Function

    Function Unzip_To_File_Path(source_file_path As String, destination_folder_path As String) As String
        Try
            If destination_folder_path.Substring(destination_folder_path.Length - 1, 1) <> "\" Then
                destination_folder_path = destination_folder_path & "\"
            End If

            ZipFile.ExtractToDirectory(source_file_path, destination_folder_path)

            WriteLog_Full("[UNZIP TO FILE PATH] - Source file: " & source_file_path & "- Destination Path: " & destination_folder_path & " - Result: SUCCESSFUL")
            Return destination_folder_path & System.IO.Path.GetFileNameWithoutExtension(source_file_path)
        Catch ex As Exception
            WriteErrorLog("[UNZIP TO FILE PATH] - Source file: " & source_file_path & "- Destination Path: " & destination_folder_path & vbLf & " - Error Message: " & ex.ToString)
            MsgBox(ex.Message, vbCritical)
            Return Nothing
        End Try

    End Function

    Public Function SQL_EXCEL_TO_DATATABLE(ExcelPath As String, SQL_STRING As String) As DataTable
        Try
            Dim extention As String = System.IO.Path.GetExtension(ExcelPath)
            Dim connection_string As String = ""
            Select Case extention
                Case ".xls" 'Excel 97-03.
                    connection_string = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & ExcelPath & ";Extended Properties='Excel 8.0;HDR=Yes'"
                Case ".xlsx" 'Excel 07 or higher.
                    connection_string = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source='" & ExcelPath & " '; " & "Extended Properties=Excel 8.0;"
            End Select

            If connection_string.Length = 0 Then
                WriteLog_Full("[SQL EXCEL TO DATATABLE] - Source file: " & ExcelPath & "- SQL STRING: " & SQL_STRING & " - FAIL")
                Return Nothing
            Else
                Dim conn As New OleDbConnection(connection_string)
                conn.Open()
                Dim cmdDataGrid As OleDbCommand = New OleDbCommand(SQL_STRING, conn)
                Dim da As New OleDbDataAdapter
                da.SelectCommand = cmdDataGrid
                Dim DT As New DataTable
                da.Fill(DT)
                WriteLog_Full("[SQL EXCEL TO DATATABLE] - Source file: " & ExcelPath & "- SQL STRING: " & SQL_STRING & " - SUCCESSFUL")
                Return DT
            End If
        Catch ex As Exception
            WriteErrorLog("[SQL EXCEL TO DATATABLE] - Source file: " & ExcelPath & "- SQL STRING: " & SQL_STRING & vbLf & " - Error Message: " & ex.ToString)
            MsgBox(ex.Message, vbCritical)
            Return Nothing
        End Try

    End Function

    Public Function SPLIT_TEXT_TO_LIST_BY_DELIMITOR(text As String, delimitor As String) As List(Of String)
        Dim result As List(Of String) = Split(text, delimitor).ToList
        Return result
    End Function

    Function ConvertToUnSign(ByVal sContent As String) As String
        Dim i As Long
        Dim intCode As Long
        Dim sChar As String
        Dim sConvert As String = ""
        'ConvertToUnSign = AscW(sContent)
        For i = 0 To Len(sContent) - 1
            sChar = sContent.Substring(i, 1)
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
                Case 777, 768, 803, 769, 771
                    sConvert = sConvert & ""
                Case Else
                    sConvert = sConvert & sChar
            End Select
        Next
        Return UCase(sConvert)
    End Function

    Public Function CONVERT_STRING_TO_DATE(STR_DATE As String, FORMAT_SOURCE As String, FORMAT_DESTINATION As String) As DateTime
        Return Format(DateTime.ParseExact(STR_DATE, FORMAT_SOURCE, Nothing), FORMAT_DESTINATION)
    End Function

    Public Function DateAddWeekDaysOnly(ByVal dDate As DateTime, ByVal iAddDays As Int32) As DateTime
        If iAddDays <> 0 Then
            Dim iIncrement As Int32 = If(iAddDays > 0, 1, -1)
            Dim iCounter As Int32

            Do
                dDate = dDate.AddDays(iIncrement)
                If dDate.DayOfWeek <> DayOfWeek.Saturday AndAlso dDate.DayOfWeek <> DayOfWeek.Sunday Then iCounter += iIncrement
            Loop Until iCounter = iAddDays
        End If

        Return dDate
    End Function

    Public Sub LISTVIEW_AUTO_RESIZE_COLUMNS_WIDTH(ByVal lvControlName As ListView)
        Dim minWidthArray(lvControlName.Columns.Count) As Integer
        For i = 0 To lvControlName.Columns.Count - 1
            'Resize to fit the header
            lvControlName.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize)
            'Store the minimum width required to display the header
            minWidthArray(i) = lvControlName.Columns(i).Width
            'Resize to fit contents
            lvControlName.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.ColumnContent)
            'Check to see if the minumum width is met
            If lvControlName.Columns(i).Width < minWidthArray(i) Then
                lvControlName.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize)
            End If
        Next
    End Sub

    Private Function IsWholeRowSelected(ByVal view As DevExpress.XtraGrid.Views.Grid.GridView, ByVal rowHandle As Int32) As Boolean
        For i As Integer = 0 To view.VisibleColumns.Count - 1
            If Not view.IsCellSelected(rowHandle, view.VisibleColumns(i)) Then Return False
        Next i
        Return True
    End Function

    Public Function GET_ONLY_NUMERIC_FROM_STRING(STR As String) As String
        Dim result As String = ""
        For i = 0 To STR.Length - 1
            If IsNumeric(STR.Substring(i, 1)) = True Then
                result = result & STR.Substring(i, 1)
            End If
        Next
        Return result
    End Function

    Public Function getDocText(ByVal filepath As String) As String
        Dim wordApp As New Word.Application
        wordApp.DisplayAlerts = False

        Dim nullobj As Object = System.Reflection.Missing.Value

        Dim doc As Word.Document = wordApp.Documents.Open(filepath, [ReadOnly]:=True)
        doc.ActiveWindow.Selection.WholeStory()
        doc.ActiveWindow.Selection.Copy()
        Dim data As IDataObject = Clipboard.GetDataObject
        Dim body As String = data.GetData(DataFormats.Html, True).ToString
        Dim delimiter As Char() = "<".ToCharArray()
        body = "<" + (body.Split(delimiter, 3))(2)
        doc.Close()
        wordApp.DisplayAlerts = True
        Return body
    End Function

    Public Function ConvertDataTableToHTML(ByVal dt As DataTable) As String
        ConvertDataTableToHTML =
            <table>
                <thead>
                    <tr>
                        <%=
                            From col As DataColumn In dt.Columns.Cast(Of DataColumn)()
                            Select <th><%= col.ColumnName %></th>
                        %>
                    </tr>
                </thead>
                <tbody>
                    <%=
                        From row As DataRow In dt.Rows.Cast(Of DataRow)()
                        Select
                        <tr>
                            <%=
                                From col As DataColumn In dt.Columns.Cast(Of DataColumn)()
                                Select <td><%= row(col) %></td>
                            %>
                        </tr>
                    %>
                </tbody>
            </table>.ToString()
    End Function

    Public Function ConvertDataTableToHTML2(ByVal dt As DataTable) As String

        'Building an HTML string.
        Dim html As New StringBuilder()

        'Table start.
        html.Append("<table border = '1'>")

        'Building the Header row.
        html.Append("<tr>")
        For Each column As DataColumn In dt.Columns
            html.Append("<th>")
            html.Append(column.ColumnName)
            html.Append("</th>")
        Next
        html.Append("</tr>")

        'Building the Data rows.
        For Each row As DataRow In dt.Rows
            html.Append("<tr>")
            For Each column As DataColumn In dt.Columns
                html.Append("<td>")
                html.Append(row(column.ColumnName))
                html.Append("</td>")
            Next
            html.Append("</tr>")
        Next

        'Table end.
        html.Append("</table>")

        'Append the HTML string to Placeholder.
        Return html.ToString()
    End Function

    Public Function ConvertDataTableToHTML4(ByVal myTable As DataTable) As String
        Dim myHtmlFile As String = ""
        Dim myBuilder As New StringBuilder
        If myTable Is Nothing Then
            Throw New System.ArgumentNullException("myTable")
        Else
            'Open tags and write the top portion. 
            myBuilder.Append("<html>")
            myBuilder.Append("<body>")
            myBuilder.Append("<br /><table border='0.01em' bgcolor='#FFFFFF' cellpadding='3' cellspacing='0' ")
            myBuilder.Append("style='border: solid 0.01em black; font-size: xx-small;'>")

            'Add the headings row.

            'myBuilder.Append("<br /><tr align='center' valign='top'>")

            For Each myColumn As DataColumn In myTable.Columns
                myBuilder.Append("<br /><td align='center' valign='top' style='border: solid 0.01em black;'>")
                myBuilder.Append(Trim(myColumn.ColumnName))
                myBuilder.Append("</td><p>")
            Next

            myBuilder.Append("</tr><p>")

            For Each myRow As DataRow In myTable.Rows
                myBuilder.Append("<br /><tr align='left' valign='top'>")
                For Each myColumn As DataColumn In myTable.Columns
                    myBuilder.Append("<br /><td align='left' valign='top' style='border: solid 0.01em black;'>")
                    myBuilder.Append(myRow(myColumn.ColumnName).ToString())
                    myBuilder.Append("</td><p>")
                Next
            Next
            myBuilder.Append("</tr><p>")
        End If

        'Close tags. 
        myBuilder.Append("</table><p>")
        myBuilder.Append("</body>")
        myBuilder.Append("</html>")

        'Get the string for return. myHtmlFile = myBuilder.ToString();
        myHtmlFile = myBuilder.ToString()
        Return myHtmlFile
    End Function

    Public Function ConvertDataTableToHTML3(ByVal myTable As DataTable) As String
        Dim myHtmlFile As String = ""
        Dim myBuilder As New StringBuilder
        If myTable Is Nothing Then
            Throw New System.ArgumentNullException("myTable")
        Else
            'Open tags and write the top portion. 
            myBuilder.Append("<html>")
            myBuilder.Append("<head>")
            myBuilder.Append("<title>Document</title>")
            myBuilder.Append("<meta HTTP-EQUIV='Content-Type' CONTENT='text/html; charset=utf-8'/>")
            myBuilder.Append("<style type='text/css'>")
            myBuilder.Append(".csCE774A1A {color:#000000;background-color:#A9A9A9;border-left:#808080 1px solid;border-top:#808080 1px solid;border-right:#808080 1px solid;border-bottom:#808080 1px solid;font-family:Tahoma; font-size:11px; font-weight:normal; font-style:normal; padding-left:2px;}")
            myBuilder.Append(".cs44CC9E99 {color:#000000;background-color:#A9A9A9;border-left:#808080 1px solid;border-top:#808080 1px solid;border-right-style: none;border-bottom:#808080 1px solid;font-family:Tahoma; font-size:11px; font-weight:normal; font-style:normal; }")
            myBuilder.Append(".cs7A7C2A2F {color:#000000;background-color:#A9A9A9;border-left-style: none;border-top:#808080 1px solid;border-right:#808080 1px solid;border-bottom:#808080 1px solid;font-family:Tahoma; font-size:11px; font-weight:normal; font-style:normal; }")
            myBuilder.Append(".csA244195D {color:#000000;background-color:#A9A9A9;border-left-style: none;border-top:#808080 1px solid;border-right-style: none;border-bottom:#808080 1px solid;font-family:Tahoma; font-size:11px; font-weight:normal; font-style:normal; }")
            myBuilder.Append(".cs5C43E5F9 {color:#000000;background-color:#D3D3D3;border-left:#A9A9A9 1px solid;border-top:#A9A9A9 1px solid;border-right:#A9A9A9 1px solid;border-bottom:#A9A9A9 1px solid;font-family:Tahoma; font-size:11px; font-weight:normal; font-style:normal; padding-left:2px;}")
            myBuilder.Append(".csF7D3565D {height:0px;width:0px;overflow:hidden;font-size:0px;line-height:0px;}")
            myBuilder.Append("</style>")
            myBuilder.Append("</head>")
            myBuilder.Append("<body leftMargin=10 topMargin=10 rightMargin=10 bottomMargin=10 style='background-color:#FFFFFF'>")
            myBuilder.Append("<table cellpadding='0' cellspacing='0' border='0' style='border-width:0px;empty-cells:show;width:2945px;height:34px;position:relative;'>")
            'myBuilder.Append("<tr>")
            myBuilder.Append("<tr style='vertical-align:top;'>")
            myBuilder.Append("<td style='width:0px;height:17px;'></td>")
            '
            'Add the headings row.

            'myBuilder.Append("<br /><tr align='center' valign='top'>")

            For Each myColumn As DataColumn In myTable.Columns
                myBuilder.Append("<td class='cs5C43E5F9' style='width:71px;height:15px;line-height:13px;text-align:center;vertical-align:middle;'><nobr>")
                myBuilder.Append(Trim(myColumn.ColumnName))
                myBuilder.Append("</nobr></td>")
            Next

            myBuilder.Append("</tr>")

            'For Each myRow As DataRow In myTable.Rows
            '    myBuilder.Append("<br /><tr align='left' valign='top'>")
            '    For Each myColumn As DataColumn In myTable.Columns
            '        myBuilder.Append("<br /><td align='left' valign='top' style='border: solid 0.01em black;'>")
            '        myBuilder.Append(myRow(myColumn.ColumnName).ToString())
            '        myBuilder.Append("</td><p>")
            '    Next
            'Next
            'myBuilder.Append("</tr><p>")
        End If

        'Close tags. 
        myBuilder.Append("</table>")
        myBuilder.Append("</body>")
        myBuilder.Append("</html>")

        'Get the string for return. myHtmlFile = myBuilder.ToString();
        myHtmlFile = myBuilder.ToString()
        Return myHtmlFile
    End Function

    Public Function ExportGridToHtml(ByVal grid As GridView) As String
        Dim htmlopt = New HtmlExportOptions With {.TableLayout = True}
        Dim msg As String

        Using ms = New MemoryStream()
            grid.BestFitColumns()
            grid.OptionsPrint.AutoWidth = False
            grid.OptionsView.ColumnAutoWidth = False
            grid.SelectAll()
            grid.ExportToHtml(ms, htmlopt)
            ms.Seek(0, SeekOrigin.Begin)

            Using sr = New StreamReader(ms)
                msg = Environment.NewLine + sr.ReadToEnd()
                sr.Close()
                ms.Close()
            End Using
        End Using

        Return msg
    End Function

    Public Sub DataTableToExcel(dt As DataTable)
        Dim ObjExcel As New Excel.Application
        Dim wBook As Excel.Workbook = ObjExcel.Workbooks.Add
        Dim ws As Excel.Worksheet = wBook.Sheets.Add

        Dim arr(dt.Rows.Count, dt.Columns.Count) As Object
        Dim r As Int32, c As Int32
        'copy the datatable to an array
        For r = 0 To dt.Rows.Count - 1
            For c = 0 To dt.Columns.Count - 1
                arr(r, c) = dt.Rows(r).Item(c)
            Next
        Next

        'add the column headers starting in A1
        c = 0
        For Each column As DataColumn In dt.Columns
            ws.Cells(1, c + 1) = column.ColumnName
            c += 1
        Next
        'add the data starting in cell A2
        ws.Range(ws.Cells(2, 1), ws.Cells(dt.Rows.Count, dt.Columns.Count)).Value = arr
    End Sub

    Public Function IsWorkbookAlreadyOpen(pathExcel As String) As Boolean
        Dim blnRetVal As Boolean = False
        Dim fs As FileStream

        Try
            fs = File.Open(pathExcel, FileMode.Open, FileAccess.Read, FileShare.None)
        Catch ex As Exception
            blnRetVal = True
        Finally
            If Not IsNothing(fs) Then : fs.Close() : End If
        End Try

        Return blnRetVal
    End Function

    Public Function GridView_To_Datatable(ByVal gridView1 As GridView) As DataTable
        Dim dt As New DataTable()
        For Each column As GridColumn In gridView1.VisibleColumns
            dt.Columns.Add(column.FieldName, column.ColumnType)
        Next column
        For i As Integer = 0 To gridView1.DataRowCount - 1
            Dim row As DataRow = dt.NewRow()
            For Each column As GridColumn In gridView1.VisibleColumns
                row(column.FieldName) = gridView1.GetRowCellValue(i, column)
            Next column
            dt.Rows.Add(row)
        Next i
        Return dt
    End Function
End Module
