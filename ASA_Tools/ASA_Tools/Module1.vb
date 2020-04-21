Imports System.Data.SQLite
Imports System.IO.Compression
Imports Microsoft.Data.Sqlite.Extensions

Module Module1
    Public appPath As String = AppDomain.CurrentDomain.BaseDirectory
    Public local_app_config As String = appPath & "App_Config.txt"

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
            'System.IO.File.AppendAllText(strPath_Log & strFileName, (strErrorText & Convert.ToString(" - ")) + DateTime.Now.ToString() + vbCr & vbLf)
            System.IO.File.AppendAllText(strPath_Log & strFileName, (DateTime.Now.ToString() & Convert.ToString(" - ")) + strErrorText + vbCr + vbLf)
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

    Public Sub SQL_QUERY_FROM_FILE_NO_LOG(LINK_DATABASE As String, sql_string As String)
        Dim MYCONNECTION As SQLiteConnection
        MYCONNECTION = New SQLiteConnection("DataSource=" & LINK_DATABASE & ";version=3;new=False;datetimeformat=CurrentCulture;")
        MYCONNECTION.Open()

        Dim CMD As New SQLiteCommand
        CMD.CommandText = sql_string
        CMD.Connection = MYCONNECTION
        Dim RDR As SQLiteDataReader = CMD.ExecuteReader
    End Sub

    Public Function SQL_FROMFILE_TO_INTEGER_NO_LOG(link_database As String, sql_string As String) As Integer
        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & ";version=3;new=False;datetimeformat=CurrentCulture;")
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
    End Function

    Public Function SQL_FROMFILE_TO_STRING_NO_LOG(link_database As String, sql_string As String) As String
        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & ";version=3;new=False;datetimeformat=CurrentCulture;")
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

    Public Function SQL_QUERY_TO_STRING(link_database As String, sql_string As String) As String
        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & ";version=3;new=False;datetimeformat=CurrentCulture;")
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
    End Function

    Public Function SQL_QUERY_TO_INTEGER(link_database As String, sql_string As String) As Integer
        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & ";version=3;new=False;datetimeformat=CurrentCulture;")
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
    End Function

    Public Function SQL_FROMFILE_COUNT_ROW(link_database As String, sql_string As String) As Integer
        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & ";version=3;new=False;datetimeformat=CurrentCulture;")
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
    End Function

    Public Sub SQL_QUERY(LINK_DATABASE As String, sql_string As String)
        Dim MYCONNECTION As SQLiteConnection
        MYCONNECTION = New SQLiteConnection("DataSource=" & LINK_DATABASE & ";version=3;new=False;datetimeformat=CurrentCulture;")
        MYCONNECTION.Open()

        Dim CMD As New SQLiteCommand
        CMD.CommandText = sql_string
        CMD.Connection = MYCONNECTION
        Dim RDR As SQLiteDataReader = CMD.ExecuteReader
        WriteLog_Full("[SQL QUERY] - " & sql_string & vbLf & " - Link database: " & LINK_DATABASE)
    End Sub

    Public Sub ShowForm(ByVal _childForm As Form)
        Dim objForms As Form
        Dim _parrentForm As Form = formMain

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

    Function Unzip_To_File_Path(source_file_path As String, destination_folder_path As String) As String
        WriteLog_Full("[UNZIP TO FILE PATH] - Source file: " & source_file_path & "- Destination Path: " & destination_folder_path)

        If destination_folder_path.Substring(destination_folder_path.Length - 1, 1) <> "\" Then
            destination_folder_path = destination_folder_path & "\"
        End If

        ZipFile.ExtractToDirectory(source_file_path, destination_folder_path)

        WriteLog_Full("[UNZIP TO FILE PATH] - Source file: " & source_file_path & "- Destination Path: " & destination_folder_path & " - SUCCESSFUL")
        Return destination_folder_path & System.IO.Path.GetFileNameWithoutExtension(source_file_path)
    End Function

    Public Sub SQLITE_BULK_COPY(DataTable As DataTable, link_database As String, table_name As String)
        Dim TimerStart As DateTime
        TimerStart = Now

        Dim datareader As New DataTableReader(DataTable)

        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & ";version=3;new=False;datetimeformat=CurrentCulture;")

        Dim BulkCopy As SqliteBulkCopy = New SqliteBulkCopy(MYCONNECTION)

        BulkCopy.DestinationTableName = table_name

        BulkCopy.ColumnMappings.Clear()

        For i As Integer = 0 To DataTable.Columns.Count - 1
            WriteLog_Full("[SQLITE BULK COPY] - Table name: " & table_name & " - Columns: " & DataTable.Columns(i).ColumnName.ToString() & vbLf & " - Link database: " & MYCONNECTION.ConnectionString.ToString)
            BulkCopy.ColumnMappings.Add(DataTable.Columns(i).ColumnName.ToString(), 0)
        Next

        BulkCopy.WriteToServer(datareader)

        Dim TimeSpent As System.TimeSpan
        TimeSpent = Now.Subtract(TimerStart)
        WriteLog_Full("[SQLITE BULK COPY] - Successful - Table name: " & table_name & " - in " & Format(TimeSpent.TotalSeconds, "0.00") & " seconds" & vbLf & " - Link database: " & MYCONNECTION.ConnectionString.ToString)
    End Sub

End Module
