Imports DevExpress.XtraGrid.Views.Grid
Imports Microsoft.Data.Sqlite.Extensions
Imports System.Data.OleDb
Imports System.Data.SQLite
Imports System.IO.Compression
Imports System.Reflection
Imports Excel = Microsoft.Office.Interop.Excel

Module Module_Letter_Management
    Public appPath As String = AppDomain.CurrentDomain.BaseDirectory
    Public link_app_config As String = appPath & "Application_Config.db"

    Public Sub ShowForm(ByVal _childForm As Form)
        Dim objForms As Form
        Dim _parrentForm As Form = Main
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
            Dim strPath_Log As String = appPath

            If strPath_Log.Substring(strPath_Log.Length - 1, 1) <> "\" Then
                strPath_Log = strPath_Log & "\"
            End If
            strPath_Log = strPath_Log & "LOG\Error\"

            If (Not System.IO.Directory.Exists(strPath_Log)) Then
                System.IO.Directory.CreateDirectory(strPath_Log)
            End If

            Dim strFileName As String = "errorLog_" & Environment.UserName & ".txt"
            Dim content_error As String = (DateTime.Now.ToString() & Convert.ToString(" - ")) + strErrorText + vbCr + vbLf
            System.IO.File.AppendAllText(strPath_Log & strFileName, content_error)
            WriteLog_Full(content_error)

        Catch ex As Exception
            WriteErrorLog("Error in WriteErrorLog: " + ex.Message)
        End Try
    End Sub

    Public Sub WriteLog_Full(strText As String)
        Try
            Dim strPath_Log As String = appPath

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
            Dim strPath_Log As String = appPath

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

    Public Sub SQLITE_CREATE_DATABASE_FILE(link_database As String)
        If Not My.Computer.FileSystem.FileExists(link_database) Then
            Try
                ' Create the SQLite database
                SQLiteConnection.CreateFile(link_database)
                Main.Statusbar_item1.Caption = "New Database Created..."
            Catch ex As Exception
                Main.Statusbar_item1.Caption = "Database Created Failed..."
            End Try
        End If
    End Sub
    Public Sub SQLITE_CREATE_DATABASE_SPECIAL_LIST(link_database As String)
        If Not My.Computer.FileSystem.FileExists(link_database) Then
            Try
                ' Create the SQLite database
                SQLiteConnection.CreateFile(link_database)
                Main.Statusbar_item1.Caption = "New Database Created..."
            Catch ex As Exception
                Main.Statusbar_item1.Caption = "Database Created Failed..."
            End Try
        End If
    End Sub
    Public Sub SQLITE_CREATE_TABLE_TASETCO_IF_NOT_EXISTS(link_database As String, table_name As String)
        SQL_QUERY(link_database, "CREATE TABLE IF NOT EXISTS " & table_name & " (Vendor_Name VARCHAR(20),Bill_Number VARCHAR UNIQUE NOT NULL,Sent_date VARCHAR(20),Client_Name VARCHAR,Mailing_address VARCHAR,Attention VARCHAR,Master_No VARCHAR,Document_note VARCHAR,ACS_Status VARCHAR,Remark VARCHAR,Batch_begin_month VARCHAR(1),Tasetco_Cus_name VARCHAR,Tasetco_packet_name VARCHAR,Tasetco_PO VARCHAR,Tasetco_TP VARCHAR,Tasetco_Cus_Name_RC VARCHAR,Tasetco_Address_RC VARCHAR,Tasetco_Status VARCHAR,Tasetco_Ac_Cosignee VARCHAR,Tasetco_Date_Received VARCHAR,Tasetco_Date_Get_Data VARCHAR,User_Modified VARCHAR,User_Created VARCHAR,Final_Result VARCHAR,CROWN_Barcode VARCHAR,CROWN_Sent_Date VARCHAR,Deleted_Date VARCHAR)")
    End Sub
    Public Sub SQLITE_CREATE_TABLE_VIETTEL_IF_NOT_EXISTS(link_database As String, table_name As String)
        SQL_QUERY(link_database, "CREATE TABLE IF NOT EXISTS " & table_name & " (Vendor_Name VARCHAR,Bill_Number VARCHAR UNIQUE NOT NULL,Sent_date VARCHAR,Client_Name VARCHAR,Mailing_address VARCHAR,Attention VARCHAR,Master_No VARCHAR,Document_note VARCHAR,ACS_Status VARCHAR,Remark VARCHAR,Batch_begin_month VARCHAR(1),Viettel_Size VARCHAR,Viettel_Service VARCHAR,Viettel_Status VARCHAR,Viettel_DateReceived VARCHAR,Viettel_Details_Status VARCHAR,Viettel_Date_Get_Data VARCHAR,User_Modified VARCHAR,User_Created VARCHAR,Final_Result VARCHAR,CROWN_Barcode VARCHAR,CROWN_Sent_Date VARCHAR,Deleted_Date VARCHAR)")
    End Sub
    Public Sub SQLITE_CREATE_TABLE_SPECIAL_LIST_IF_NOT_EXISTS(link_database As String, table_name As String)
        SQL_QUERY(link_database, "CREATE TABLE IF NOT EXISTS " & table_name & " (Master_No VARCHAR,Client_name VARCHAR,Client_address VARCHAR,Client_Attention VARCHAR,Remark VARCHAR,User_Modified VARCHAR,User_Created VARCHAR,Case_ID VARCHAR,Deleted_Date VARCHAR)")
    End Sub
    Public Sub SQLITE_CREATE_REPORT_APPCONFIG(link_database As String)
        SQL_QUERY(link_database, "CREATE TABLE IF NOT EXISTS LIST_DATABASE_REPORT (Report_Name VARCHAR UNIQUE NOT NULL, Title_Report VARCHAR, Paper_Kind VARCHAR, WaterMark_Text VARCHAR, SQL_String VARCHAR, Vendor VARCHAR, User_Created VARCHAR, User_Modified VARCHAR)")
    End Sub

    Public Function SQL_QUERY_CSV_TO_DATATABLE(csvFolderPath As String, csvFileName As String, sql_string As String) As DataTable
        Try
            Dim conn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & csvFolderPath & ";Extended Properties='Text;HDR=Yes;FMT=CSVDelimited'")
            conn.Open()
            Dim cmd2 As OleDbCommand = New OleDbCommand(sql_string, conn)
            Dim da As OleDbDataAdapter = New OleDbDataAdapter(cmd2)
            Dim dt As DataTable = New DataTable
            da.Fill(dt)
            conn.Close()

            WriteLog_Full("[SQL_QUERY_CSV_TO_DATATABLE] - " & sql_string & vbLf & " - Link database: " & csvFolderPath & "\" & csvFileName & vbLf & " - Result: Completed")
            Return dt
        Catch ex As Exception
            WriteErrorLog("[SQL_QUERY_CSV_TO_DATATABLE] - " & sql_string & vbLf & " - Link database: " & csvFolderPath & "\" & csvFileName & vbLf & "- Error Message: " & ex.ToString)
            MsgBox(ex.Message, vbCritical)
        End Try
    End Function

    Public Function SQL_QUERY_TO_DATATABLE(Link_database As String, sql_string As String) As DataTable
        Try
            Dim MYCONNECTION As New SQLiteConnection("DataSource=" & Link_database & ";version=3;new=False;datetimeformat=CurrentCulture;")
            MYCONNECTION.Open()
            Dim CMD As New SQLiteCommand
            CMD.CommandText = sql_string
            CMD.Connection = MYCONNECTION
            Dim RDR As SQLiteDataReader = CMD.ExecuteReader
            Dim DT As New DataTable
            DT.Load(RDR)
            RDR.Close()
            MYCONNECTION.Close()
            WriteLog_Full("[SQL_QUERY_TO_DATATABLE] - " & sql_string & vbLf & " - Link database: " & Link_database & vbLf & " - Result: Completed")
            Return DT
        Catch ex As Exception
            WriteErrorLog("[SQL_QUERY_TO_DATATABLE] - " & sql_string & vbLf & " - Link database: " & Link_database & vbLf & "- Error Message: " & ex.ToString)
            MsgBox(ex.Message, vbCritical)
        End Try
    End Function

    Public Function SQL_QUERY_TO_DATASET(Link_database As String, sql_string As String) As DataSet
        Try
            Dim MYCONNECTION As New SQLiteConnection("DataSource=" & Link_database & ";version=3;new=False;datetimeformat=CurrentCulture;")
            MYCONNECTION.Open()
            Dim CMD As New SQLiteCommand
            CMD.CommandText = sql_string
            CMD.Connection = MYCONNECTION
            Dim RDR As SQLiteDataReader = CMD.ExecuteReader
            Dim DT As New DataTable
            DT.Load(RDR)
            RDR.Close()
            MYCONNECTION.Close()
            Dim DS As New DataSet
            DS.Tables.Add(DT)
            WriteLog_Full("[SQL_QUERY_TO_DATASET] - " & sql_string & vbLf & " - Link database: " & Link_database & vbLf & " - Result: Completed")
            Return DS
        Catch ex As Exception
            WriteErrorLog("[SQL_QUERY_TO_DATASET] - " & sql_string & vbLf & " - Link database: " & Link_database & vbLf & "- Error Message: " & ex.ToString)
            MsgBox(ex.Message, vbCritical)
        End Try
    End Function

    Public Function SQL_QUERY_TO_STRING(Link_database As String, sql_string As String) As String
        Try
            Dim MYCONNECTION As New SQLiteConnection("DataSource=" & Link_database & ";version=3;new=False;datetimeformat=CurrentCulture;")
            MYCONNECTION.Open()
            Dim CMD As New SQLiteCommand
            CMD.CommandText = sql_string
            CMD.Connection = MYCONNECTION
            Dim RDR As SQLiteDataReader = CMD.ExecuteReader
            Dim DT As New DataTable
            Dim result As String = String.Empty
            DT.Load(RDR)
            RDR.Close()
            MYCONNECTION.Close()
            If DT.Rows.Count > 0 Then
                result = DT.Rows(0).Item(0).ToString
            End If
            WriteLog_Full("[SQL_QUERY_TO_STRING] - " & sql_string & vbLf & " - Link database: " & Link_database & vbLf & " - Result: " & result)
            Return result
        Catch ex As Exception
            WriteErrorLog("[SQL_QUERY_TO_STRING] - " & sql_string & vbLf & " - Link database: " & Link_database & vbLf & "- Error Message: " & ex.ToString)
            MsgBox(ex.Message, vbCritical)
        End Try
    End Function
    Public Function SQL_QUERY_TO_INTEGER(Link_database As String, sql_string As String) As Integer
        Try
            Dim MYCONNECTION As New SQLiteConnection("DataSource=" & Link_database & ";version=3;new=False;datetimeformat=CurrentCulture;")
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

            WriteLog_Full("[SQL_QUERY_TO_INTEGER] - " & sql_string & vbLf & " - Link database: " & Link_database & vbLf & " - Result: " & result)

            Return result
        Catch ex As Exception
            WriteErrorLog("[SQL_QUERY_TO_STRING] - " & sql_string & vbLf & " - Link database: " & Link_database & vbLf & "- Error Message: " & ex.ToString)
            MsgBox(ex.Message, vbCritical)
        End Try
    End Function

    Public Sub SQL_QUERY(Link_database As String, sql_string As String)
        Dim TimerStart As DateTime = Now
        Dim showmsg As Boolean = False
        Dim err_msg As String

        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & Link_database & ";version=3;new=False;datetimeformat=CurrentCulture;")

        Do
            Try


                MYCONNECTION.Open()

                Dim CMD As New SQLiteCommand
                CMD.CommandText = sql_string
                CMD.Connection = MYCONNECTION
                Dim RDR As SQLiteDataReader = CMD.ExecuteReader

                WriteLog_Full("[SQL QUERY] - " & sql_string & vbLf & " - Link database: " & Link_database & vbLf & " - Result: SUCCESSFUL")
                Exit Do
            Catch ex As Exception
                WriteErrorLog("[SQL QUERY] - " & sql_string & vbLf & " - Link database: " & Link_database & vbLf & " - Error Reason: " & ex.ToString)
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
        MYCONNECTION.Close()

        If showmsg = True Then
            MsgBox(err_msg, vbCritical)
        End If

        'Dim MYCONNECTION As New SQLiteConnection("DataSource=" & Link_database & ";version=3;new=False;datetimeformat=CurrentCulture;")
        'MYCONNECTION.Open()
        'Dim CMD As New SQLiteCommand
        'CMD.CommandText = sql_string
        'CMD.Connection = MYCONNECTION
        'Dim RDR As SQLiteDataReader = CMD.ExecuteReader
        'MYCONNECTION.Close()
        'MYCONNECTION.Dispose()

    End Sub
    Public Function SQL_SEARCH_IN_ALL_COLUMN(link_database As String, table_name As String, str_search As String) As DataTable
        Try
            Dim SQL_Str_Search As String = String.Empty
            Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & ";version=3;new=False;datetimeformat=CurrentCulture;")
            MYCONNECTION.Open()
            Dim CMD As New SQLiteCommand
            CMD.CommandText = "SELECT * FROM " & table_name & " LIMIT 1"
            CMD.Connection = MYCONNECTION
            Dim RDR As SQLiteDataReader = CMD.ExecuteReader
            Dim DT As New DataTable
            DT.Load(RDR)
            RDR.Close()
            For i = 0 To DT.Columns.Count - 1
                If SQL_Str_Search.Length = 0 Then
                    SQL_Str_Search = "[" & DT.Columns(i).ColumnName.ToString() & "] LIKE '%" & str_search & "%' "
                Else
                    SQL_Str_Search = SQL_Str_Search & " OR [" & DT.Columns(i).ColumnName.ToString() & "] LIKE '%" & str_search & "%' "
                End If
            Next
            Dim SQL_String As String = "SELECT * FROM " & table_name & " WHERE " & SQL_Str_Search
            SQL_Str_Search = String.Empty
            MYCONNECTION.Close()
            WriteLog_Full("[SQL_SEARCH_IN_ALL_COLUMN] - " & str_search & vbLf & " - Link database: " & link_database & vbLf & " - Table name: " & table_name & vbLf & " - Result: Successful")
            Return SQL_QUERY_TO_DATATABLE(link_database, SQL_String)
        Catch ex As Exception
            WriteErrorLog("[SQL_SEARCH_IN_ALL_COLUMN] - " & str_search & vbLf & " - Link database: " & link_database & vbLf & " - Table name: " & table_name & vbLf & "- Error Message: " & ex.ToString)
            MsgBox(ex.Message, vbCritical)
        End Try
    End Function
    Public Sub Error_handle()
        Call MsgBox("Contact To Administrator !!!" & Chr(10) & Chr(10) & Err.Number & Err.Description, vbCritical)
        Err.Clear()
    End Sub
    Public Sub IMPORT_INTO_DATABASE_FROM_EXCEL(link_database As String, table_name As String)
        Try
            Dim fname As String = String.Empty
            fname = SELECT_EXCEL_FILE_RETURNED_FULL_PATH("Please Select excel file (.xls)")

            Dim conn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & fname & ";Extended Properties='Excel 8.0;HDR=Yes'")

            conn.Open()
            Dim sql = "SELECT * FROM [Sheet1$]"
            Dim cmdDataGrid As OleDbCommand = New OleDbCommand(sql, conn)
            Dim da As New OleDbDataAdapter
            da.SelectCommand = cmdDataGrid
            Dim dt As New DataTable
            da.Fill(dt)

            Dim datareader As New DataTableReader(dt)

            conn.Close()

            Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & ";version=3;new=False;datetimeformat=CurrentCulture;")

            Dim BulkCopy As SqliteBulkCopy = New SqliteBulkCopy(MYCONNECTION)

            BulkCopy.DestinationTableName = table_name

            BulkCopy.ColumnMappings.Clear()

            For i As Integer = 0 To dt.Columns.Count - 1
                BulkCopy.ColumnMappings.Add(dt.Columns(i).ColumnName.ToString(), 0)
            Next

            BulkCopy.WriteToServer(datareader)
            WriteLog_Full("[IMPORT_INTO_DATABASE_FROM_EXCEL] - Link database: " & link_database & vbLf & " - Table name: " & table_name & vbLf & " - Result: Successful")
        Catch ex As Exception
            WriteErrorLog("[IMPORT_INTO_DATABASE_FROM_EXCEL] - Link database: " & link_database & vbLf & " - Table name: " & table_name & vbLf & "- Error Message: " & ex.ToString)
            MsgBox(ex.Message, vbCritical)
        End Try
    End Sub
    Public Sub SQLITE_BULK_COPY(DataTable As DataTable, link_database As String, table_name As String)
        Dim TimerStart As DateTime = Now
        Dim showmsg As Boolean = False
        Dim err_msg As String

        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & ";version=3;new=False;datetimeformat=CurrentCulture;")

        Do
            Try
                TimerStart = Now
                Dim datareader As New DataTableReader(DataTable)

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
                WriteLog_Full("[SQLITE BULK COPY] - Successful - Table name: " & table_name & " - in " & Format(TimeSpent.TotalSeconds, "0.00") & " seconds" & vbLf & " - Link database: " & link_database)
                Exit Do
            Catch ex As Exception
                WriteLog_Full("[SQL QUERY FROM FILE] - Table name: " & table_name & vbLf & " - Link database: " & link_database & vbLf & " - Error Reason: " & ex.ToString)
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

        MYCONNECTION.Close()

        If showmsg = True Then
            MsgBox(err_msg, vbCritical)
        End If
        'Dim TimerStart As DateTime
        'TimerStart = Now

        'Dim datareader As New DataTableReader(DataTable)

        'Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & ";version=3;new=False;datetimeformat=CurrentCulture;")

        'Dim BulkCopy As SqliteBulkCopy = New SqliteBulkCopy(MYCONNECTION)

        'BulkCopy.DestinationTableName = table_name

        'BulkCopy.ColumnMappings.Clear()

        'For i As Integer = 0 To DataTable.Columns.Count - 1
        '    BulkCopy.ColumnMappings.Add(DataTable.Columns(i).ColumnName.ToString(), 0)
        'Next

        'BulkCopy.WriteToServer(datareader)

        'MYCONNECTION.Close()
        'MYCONNECTION.Dispose()

        'Dim TimeSpent As System.TimeSpan
        'TimeSpent = Now.Subtract(TimerStart)
        'Main.Statusbar_item2.Caption = "SQLite Bulk Copy in " & Format(TimeSpent.TotalSeconds, "0.00") & " seconds"
    End Sub

    Public Sub IMPORT_INTO_DATABASE_FROM_EXCEL2(link_database As String, table_name As String)
        Dim fname As String = String.Empty
        fname = SELECT_EXCEL_FILE_RETURNED_FULL_PATH("Please select excel file (.xls)")

        Dim conn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & fname & ";Extended Properties='Excel 8.0;HDR=Yes'")

        conn.Open()
        Dim sql = "SELECT * FROM [Sheet1$]"
        Dim cmdDataGrid As OleDbCommand = New OleDbCommand(sql, conn)
        Dim da As New OleDbDataAdapter
        da.SelectCommand = cmdDataGrid
        Dim dt As New DataTable
        da.Fill(dt)
        Dim i As Long = 1
        For Each Drr As DataRow In dt.Rows
            Main.Statusbar_item2.Caption = "Importing row " & i & "/" & dt.Rows.Count
            Dim Str_SQL_Add_Record As String = "INSERT INTO " & table_name & " (" & GET_LIST_FIELD_NAME_FROM_DATABASE(link_database, table_name) & ") VALUES ('" & Drr(0).ToString & "', '" & Drr(1).ToString & "', '" & Drr(2).ToString & "', '" & Drr(3).ToString & "', '" & Drr(4).ToString & "', '" & Drr(5).ToString & "', '" & Drr(6).ToString & "', '" & Drr(7).ToString & "', '" & Drr(8).ToString & "', '" & Drr(9).ToString & "', '" & Drr(10).ToString & "', '" & Drr(11).ToString & "', '" & Drr(12).ToString & "', '" & Drr(13).ToString & "', '" & Drr(14).ToString & "', '" & Drr(15).ToString & "', '" & Drr(16).ToString & "', '" & Drr(17).ToString & "', '" & Drr(18).ToString & "', '" & Drr(19).ToString & "', '" & Drr(20).ToString & "', '" & Drr(21).ToString & "', '" & Drr(22).ToString & "', '" & Drr(23).ToString & "', '" & Drr(24).ToString & "', '" & Drr(25).ToString & "');"
            SQL_QUERY(link_database, Str_SQL_Add_Record)
            i = i + 1
        Next
        conn.Close()
    End Sub
    Public Function GET_ALL_TABLE_NAME_IN_DATABASE(ByVal link_database As String) As DataTable
        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & ";version=3;new=False;datetimeformat=CurrentCulture;")
        MYCONNECTION.Open()
        Dim CMD As New SQLiteCommand
        CMD.CommandText = "select name from sqlite_master where type='table' order by name"
        CMD.Connection = MYCONNECTION
        Dim RDR As SQLiteDataReader = CMD.ExecuteReader
        Dim DS As New DataTable
        DS.Load(RDR)
        RDR.Close()
        MYCONNECTION.Close()
        Return DS
    End Function

    Public Function GET_LIST_FIELD_NAME_FROM_DATABASE(LINK_DATABASE As String, TABLE_NAME As String) As String
        Dim i As Integer
        Dim SQL_Str_Search As String = String.Empty

        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & LINK_DATABASE & ";version=3;new=False;datetimeformat=CurrentCulture;")
        MYCONNECTION.Open()
        Dim CMD As New SQLiteCommand
        CMD.CommandText = "Select * FROM " & TABLE_NAME & " LIMIT 1"
        CMD.Connection = MYCONNECTION
        Dim RDR As SQLiteDataReader = CMD.ExecuteReader
        Dim DS As New DataTable
        DS.Load(RDR)
        RDR.Close()

        For i = 0 To DS.Columns.Count - 1
            If SQL_Str_Search.Length = 0 Then
                SQL_Str_Search = "[" & DS.Columns(i).ColumnName.ToString() & "]"
            Else
                SQL_Str_Search = SQL_Str_Search & ", [" & DS.Columns(i).ColumnName.ToString() & "]"
            End If
        Next
        MYCONNECTION.Close()

        If SQL_Str_Search.Length = 0 Then
            Return False
        Else
            Return SQL_Str_Search
        End If

    End Function
    Public Function SELECT_EXCEL_FILE_RETURNED_FULL_PATH(str_title As String) As String
        Dim fd As OpenFileDialog = New OpenFileDialog()
        fd.Title = str_title
        fd.RestoreDirectory = True
        fd.Filter = "|All Files|*.*" +
                    "Excel files|*.xls"

        fd.FilterIndex = 2
        fd.RestoreDirectory = True

        Dim result As String = String.Empty

        If fd.ShowDialog() = DialogResult.OK Then
            If fd.FileName.Length > 0 Then
                result = fd.FileName
            End If
        End If
        Return result
    End Function

    Public Function SEARCH_DATABASE_BY_STRING_RETURN_BOOLEAN(Link_database As String, table_name As String, str_search As String) As Boolean
        Dim i As Integer
        Dim SQL_Str_Search As String = String.Empty

        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & Link_database & ";version=3;new=False;datetimeformat=CurrentCulture;")
        MYCONNECTION.Open()
        Dim CMD As New SQLiteCommand
        CMD.CommandText = "Select * FROM " & table_name & " LIMIT 1"
        CMD.Connection = MYCONNECTION
        Dim RDR As SQLiteDataReader = CMD.ExecuteReader
        Dim DS As New DataTable
        DS.Load(RDR)
        RDR.Close()

        For i = 0 To DS.Columns.Count - 1
            If SQL_Str_Search.Length = 0 Then
                SQL_Str_Search = "[" & DS.Columns(i).ColumnName.ToString() & "] Like '%" & str_search & "%' "
            Else
                SQL_Str_Search = SQL_Str_Search & " OR [" & DS.Columns(i).ColumnName.ToString() & "] LIKE '%" & str_search & "%' "
            End If
        Next

        CMD.CommandText = "SELECT * FROM " & table_name & " WHERE " & SQL_Str_Search
        Dim RDR2 As SQLiteDataReader = CMD.ExecuteReader
        Dim DS2 As New DataTable
        DS2.Load(RDR2)
        RDR2.Close()
        SQL_Str_Search = String.Empty
        MYCONNECTION.Close()
        If DS2.Rows.Count = 0 Then
            Return False
        Else
            Return True
        End If

    End Function
    Public Sub EXPORT_DATAGRIDVIEW_TO_EXCEL(DataGridView_name As DataGridView)
        Dim objExcel As New Excel.Application
        Dim bkWorkBook As Excel.Workbook
        Dim shWorkSheet As Excel.Worksheet

        objExcel = New Excel.Application
        objExcel.Visible = True
        bkWorkBook = objExcel.Workbooks.Add
        shWorkSheet = CType(bkWorkBook.ActiveSheet, Excel.Worksheet)
        shWorkSheet.Cells.NumberFormat = "@"

        DataGridView_name.SelectAll()
        Dim DATAOBJ As DataObject = DataGridView_name.GetClipboardContent
        Clipboard.SetDataObject(DATAOBJ)

        shWorkSheet.Range("A1").PasteSpecial(Excel.XlPasteType.xlPasteAll)

        If Len(shWorkSheet.Range("A1").Value) = 0 Then
            shWorkSheet.Columns(1).delete
        End If
    End Sub

    Public Sub EXPORT_GRIDVIEW_TO_EXCEL(GRIDVIEW As GridView)

        Dim full_path As String = appPath & Now.ToString("ddMMyyyyhhmmssttttt") & ".xls"
        GRIDVIEW.ExportToXls(full_path)

        Dim ObjExcel As Object = CreateObject("Excel.Application")
        ObjExcel.Visible = True
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

        'Dim conn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & full_path & ";Extended Properties='Excel 8.0;HDR=Yes'")

        'conn.Open()
        'Dim sql = "SELECT * FROM [Sheet$]"
        'Dim cmdDataGrid As OleDbCommand = New OleDbCommand(sql, conn)
        'Dim da As New OleDbDataAdapter
        'da.SelectCommand = cmdDataGrid
        'Dim dt As New DataTable
        'da.Fill(dt)
        'COPY_DATATABLE_TO_EXCEL(dt)

        'conn.Close()
        'Kill(full_path)
    End Sub

    Sub COPY_DATATABLE_TO_EXCEL(dt As DataTable)
        Dim _excel As New Excel.Application
        _excel.Visible = True
        Dim wBook As Excel.Workbook
        Dim wSheet As Excel.Worksheet

        wBook = _excel.Workbooks.Add()
        wSheet = wBook.ActiveSheet()
        wSheet.Cells.NumberFormat = "@"

        Dim dc As System.Data.DataColumn
        Dim colIndex As Integer = 0
        Dim rowIndex As Integer = 0
        Dim Nbligne As Integer = dt.Rows.Count

        For Each dc In dt.Columns
            colIndex = colIndex + 1
            wSheet.Cells(1, colIndex) = dc.ColumnName
            wSheet.Cells(2, colIndex).Resize(Nbligne, ).Value = _excel.Application.transpose(dt.Rows.OfType(Of DataRow)().[Select](Function(k) CObj(k(dc.ColumnName))).ToArray())
        Next
    End Sub


    'Public Sub CheckVersionPortfolioReport2(link_folder_database As String)
    '    Dim link_application_config As String = SQL_QUERY_TO_STRING(link_app_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'Link_Application_Config'")
    '    Dim file_name_Portfolio As String
    '    If link_folder_database.Substring(link_folder_database.Length - 1, 1) = "\" Then
    '        file_name_Portfolio = link_folder_database & "Database_Portfolio_" & Now.ToString("ddMMyyyy") & ".txt"
    '    Else
    '        file_name_Portfolio = link_folder_database & "\Database_Portfolio_" & Now.ToString("ddMMyyyy") & ".txt"
    '    End If

    '    If Not My.Computer.FileSystem.FileExists(file_name_Portfolio) Then
    '        Try
    '            For i As Integer = 1 To 30
    '                If My.Computer.FileSystem.FileExists(link_folder_database & "Database_Portfolio_" & Now.AddDays(-i).ToString("ddMMyyyy") & ".txt") Then
    '                    Kill(link_folder_database & "Database_Portfolio_" & Now.AddDays(-i).ToString("ddMMyyyy") & ".txt")
    '                End If
    '            Next
    '            ' Create the SQLite database
    '            SQLiteConnection.CreateFile(file_name_Portfolio)
    '            SQL_QUERY(file_name_Portfolio, "CREATE TABLE IF NOT EXISTS Portfolio (MasterNo VARCHAR,Client_Name VARCHAR,Client_Address VARCHAR,Client_Attention VARCHAR)")
    '            Dim csvFilePath As String = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'Portfolio_File_Path'")
    '            Dim csvFileName As String = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'Portfolio_File_Name'")

    '            Dim sql_string As String = "INSERT INTO Portfolio (Master_No, Client_Name, Client_Address, Client_Attention)" &
    '                                        " SELECT DISTINCT MID(AccountNo,3,7) as Master_No, FullName as Client_Name, IIF(LEN(FlatNo)>0,FlatNo& ', ','')&IIF(LEN(BuildingName)>0,BuildingName& ', ','')&IIF(LEN(Street)>0,Street& ', ','') as Client_Address, TelephoneNo as Client_Attention" &
    '                                        " FROM [Text" &
    '                                            ";FMT=Delimited" &
    '                                            ";HDR=YES" &
    '                                            ";IMEX=2" &
    '                                            ";CharacterSet=437" &
    '                                            ";DATABASE=" & csvFilePath & "].[" & csvFileName & "]"
    '            '" WHERE [CustomerSegmentCode] IN ('025', '026', '300', '350', '359', '400', '420', '440', '460', '500', '510', '600', '630', '650')"
    '            Module_Letter_Management.SQL_QUERY(file_name_Portfolio, sql_string)
    '        Catch ex As Exception
    '            Main.Statusbar_item1.Caption = "Database Created Failed..."
    '        End Try
    '    End If
    'End Sub
    'Public Sub CheckVersionPortfolioReport3(link_folder_database As String)
    '    Dim link_application_config As String = SQL_QUERY_TO_STRING(link_app_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'Link_Application_Config'")
    '    Dim file_name_Portfolio As String
    '    If link_folder_database.Substring(link_folder_database.Length - 1, 1) = "\" Then
    '        file_name_Portfolio = link_folder_database & "Database_Portfolio_" & Now.ToString("ddMMyyyy") & ".txt"
    '    Else
    '        file_name_Portfolio = link_folder_database & "\Database_Portfolio_" & Now.ToString("ddMMyyyy") & ".txt"
    '    End If

    '    'Delete old data
    '    For i As Integer = 1 To 30
    '        If My.Computer.FileSystem.FileExists(link_folder_database & "Database_Portfolio_" & Now.AddDays(-i).ToString("ddMMyyyy") & ".txt") Then
    '            Kill(link_folder_database & "Database_Portfolio_" & Now.AddDays(-i).ToString("ddMMyyyy") & ".txt")
    '        End If
    '    Next

    '    If Not My.Computer.FileSystem.FileExists(file_name_Portfolio) Then
    '        'Load Portfolio.csv to datatable
    '        Dim csvFilePath As String = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'Portfolio_File_Path'")
    '        Dim csvFileName As String = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'Portfolio_File_Name'")

    '        Dim fname As String = csvFilePath & "\" & csvFileName

    '        Dim conn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & csvFilePath & ";Extended Properties='Text;HDR=Yes;FMT=CSVDelimited'")

    '        conn.Open()
    '        Dim sql As String = "Select DISTINCT MID(AccountNo, 3, 7) As MasterNo, FullName AS Client_Name, IIF(LEN(FlatNo)>0,FlatNo& ', ','')&IIF(LEN(BuildingName)>0,BuildingName& ', ','')&IIF(LEN(Street)>0,Street& ', ','') as Client_Address" &
    '            " FROM [" & csvFileName & "]" &
    '            " WHERE [FullName] <> 'DUPLICATED RELATIONSHIP'"

    '        Dim cmd2 As OleDbCommand = New OleDbCommand(sql, conn)
    '        Dim da As OleDbDataAdapter = New OleDbDataAdapter(cmd2)
    '        Dim dt As DataTable = New DataTable
    '        da.Fill(dt)

    '        ' Create the SQLite database
    '        SQLiteConnection.CreateFile(file_name_Portfolio)
    '        SQL_QUERY(file_name_Portfolio, "CREATE TABLE If Not EXISTS Portfolio_temp (MasterNo VARCHAR, Client_Name VARCHAR, Client_Address VARCHAR)")
    '        SQLITE_BULK_COPY(dt, file_name_Portfolio, "Portfolio_temp")
    '        conn.Close()

    '        'Load Portfolio.csv to datatable
    '        Dim csvFilePath_CUSTCONT As String = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'CUSTCONT_File_Path'")
    '        Dim csvFileName_CUSTCONT As String = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'CUSTCONT_File_Name'")

    '        Dim conn_CUSTCONT As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & csvFilePath_CUSTCONT & ";Extended Properties='Text;HDR=Yes;FMT=CSVDelimited'")

    '        conn_CUSTCONT.Open()
    '        sql = "Select DISTINCT [MASTERNO] AS MasterNo, [CONTACT]&IIF(LEN([ATTENTION PARTY])>0,' ('&[ATTENTION PARTY]&') ','') AS Attention" &
    '            " FROM [" & csvFileName_CUSTCONT & "]" &
    '            " WHERE [CONTACT TYPE CODE] IN ('MOB','TOF','TRS','MO2') AND [FULL NAME] <> 'DUPLICATED RELATIONSHIP' AND [CONTACT] <> '.' AND UCASE([CONTACT]) NOT LIKE '%CLOSED' AND UCASE([CONTACT]) NOT LIKE '%CLOSE' AND UCASE([CONTACT]) NOT LIKE '%CLS' AND UCASE([CONTACT]) NOT LIKE 'CLOSED%' AND UCASE([CONTACT]) NOT LIKE 'CLOSE%' AND UCASE([CONTACT]) NOT LIKE 'CLS%'"

    '        Dim cmd_CUSTCONT As OleDbCommand = New OleDbCommand(sql, conn_CUSTCONT)
    '        Dim da_CUSTCONT As OleDbDataAdapter = New OleDbDataAdapter(cmd_CUSTCONT)
    '        Dim dt_CUSTCONT As DataTable = New DataTable
    '        da_CUSTCONT.Fill(dt_CUSTCONT)

    '        SQL_QUERY(file_name_Portfolio, "CREATE TABLE If Not EXISTS CUSTCONT (MasterNo VARCHAR, Attention VARCHAR)")
    '        SQLITE_BULK_COPY(dt_CUSTCONT, file_name_Portfolio, "CUSTCONT")
    '        conn_CUSTCONT.Close()

    '        'Create FINAL REPORT PORTFOLIO
    '        SQL_QUERY(file_name_Portfolio, "CREATE TABLE If Not EXISTS Portfolio (MasterNo VARCHAR, Client_Name VARCHAR, Client_Address VARCHAR, Client_Attention VARCHAR)")

    '        sql = "SELECT Portfolio_temp.MasterNo AS MasterNo, Portfolio_temp.Client_Name AS Client_Name, Portfolio_temp.Client_Address AS Client_Address, CUSTCONT.Attention AS Client_Attention FROM Portfolio_temp" &
    '                " INNER JOIN CUSTCONT ON CUSTCONT.MasterNo=Portfolio_temp.MasterNo" &
    '                " UNION ALL" &
    '                " SELECT Portfolio_temp.MasterNo AS MasterNo, Portfolio_temp.Client_Name AS Client_Name, Portfolio_temp.Client_Address AS Client_Address, CUSTCONT.Attention AS Client_Attention FROM Portfolio_temp" &
    '                " LEFT OUTER JOIN CUSTCONT ON CUSTCONT.MasterNo=Portfolio_temp.MasterNo" &
    '                " WHERE CUSTCONT.MasterNo IS NULL"

    '        'sql = "SELECT T1.MasterNo AS Master_No, T1.Client_Name AS Client_Name, T1.Client_Address AS Client_Address, T2.Attention AS Attention " &
    '        '        "FROM Portfolio_temp AS T1 " &
    '        '        "INNER JOIN (SELECT MasterNo, Client_Name, GROUP_CONCAT(Attention,'; ') AS Attention FROM CUSTCONT GROUP BY MasterNo) AS T2 " &
    '        '        "ON T1.MasterNo=T2.MasterNo"

    '        Dim DT_FINAL As DataTable = SQL_QUERY_TO_DATATABLE(file_name_Portfolio, sql)
    '        SQLITE_BULK_COPY(DT_FINAL, file_name_Portfolio, "Portfolio")

    '        SQL_QUERY(file_name_Portfolio, "DROP TABLE CUSTCONT")
    '        SQL_QUERY(file_name_Portfolio, "DROP TABLE Portfolio_temp")
    '        SQL_QUERY(file_name_Portfolio, "vacuum")

    '    End If
    'End Sub
    Public Sub CheckVersionPortfolioReport(link_folder_database As String)
        Dim file_name_Portfolio As String

        Try
            Dim link_application_config As String = SQL_QUERY_TO_STRING(link_app_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'Link_Application_Config'")

            If link_folder_database.Substring(link_folder_database.Length - 1, 1) = "\" Then
                file_name_Portfolio = link_folder_database & "Database_Portfolio_" & Now.ToString("ddMMyyyy") & ".txt"
            Else
                file_name_Portfolio = link_folder_database & "\Database_Portfolio_" & Now.ToString("ddMMyyyy") & ".txt"
            End If

            'Delete old data
            For i As Integer = 1 To 30
                If My.Computer.FileSystem.FileExists(link_folder_database & "Database_Portfolio_" & Now.AddDays(-i).ToString("ddMMyyyy") & ".txt") Then
                    Kill(link_folder_database & "Database_Portfolio_" & Now.AddDays(-i).ToString("ddMMyyyy") & ".txt")
                End If
            Next

            If Not My.Computer.FileSystem.FileExists(file_name_Portfolio) Then
                'Load Portfolio.csv to datatable
                Dim csvFilePath As String = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'Portfolio_File_Path'")
                Dim csvFileName As String = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'Portfolio_File_Name'")

                If csvFilePath.Substring(csvFilePath.Length - 1, 1) <> "\" Then
                    csvFilePath = csvFilePath & "\"
                End If

                If Not My.Computer.FileSystem.FileExists(csvFilePath & "\" & csvFileName) Then
                    Dim folderDlg_Portfolio As New FolderBrowserDialog
                    folderDlg_Portfolio.ShowNewFolderButton = True

                    If (folderDlg_Portfolio.ShowDialog() = DialogResult.OK) Then
                        csvFilePath = folderDlg_Portfolio.SelectedPath
                        Dim root As Environment.SpecialFolder = folderDlg_Portfolio.RootFolder
                    End If
                End If

                If csvFilePath.Length = 0 Then Exit Sub

                'If IO.File.GetLastWriteTime(csvFilePath & csvFileName).ToString("dd/MM/yyyy") <> Now.ToString("dd/MM/yyyy") Then
                '    Dim iret_portfolio = MsgBox("Report Portfolio has been modified on " & IO.File.GetLastWriteTime(csvFilePath & "\" & csvFileName).ToString("dd/MM/yyyy") & " .Do you want to get latest source file Portfolio?", vbYesNo)

                '    If iret_portfolio = vbYes Then
                '        Dim folderDlg_Portfolio As New FolderBrowserDialog
                '        folderDlg_Portfolio.ShowNewFolderButton = True

                '        If (folderDlg_Portfolio.ShowDialog() = DialogResult.OK) Then
                '            csvFilePath = folderDlg_Portfolio.SelectedPath
                '            Dim root As Environment.SpecialFolder = folderDlg_Portfolio.RootFolder
                '        End If
                '    Else
                '        If My.Computer.FileSystem.FileExists(file_name_Portfolio) Then
                '            Kill(file_name_Portfolio)
                '        End If
                '        Exit Sub
                '    End If
                'End If

                Dim conn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & csvFilePath & ";Extended Properties='Text;HDR=Yes;FMT=CSVDelimited'")

                conn.Open()
                Dim sql As String = "Select DISTINCT MID(AccountNo, 3, 7) As MasterNo, FullName AS Client_Name, IIF(LEN(FlatNo)>0,FlatNo& ', ','')&IIF(LEN(BuildingName)>0,BuildingName& ', ','')&IIF(LEN(Street)>0,Street& ', ','') as Client_Address" &
                    " FROM [" & csvFileName & "]" &
                    " WHERE [FullName] <> 'DUPLICATED RELATIONSHIP'"

                Dim cmd2 As OleDbCommand = New OleDbCommand(sql, conn)
                Dim da As OleDbDataAdapter = New OleDbDataAdapter(cmd2)
                Dim dt As DataTable = New DataTable
                da.Fill(dt)

                ' Create the SQLite database
                SQLiteConnection.CreateFile(file_name_Portfolio)
                SQL_QUERY(file_name_Portfolio, "CREATE TABLE If Not EXISTS Portfolio_temp (MasterNo VARCHAR, Client_Name VARCHAR, Client_Address VARCHAR)")
                SQLITE_BULK_COPY(dt, file_name_Portfolio, "Portfolio_temp")
                conn.Close()

                'Load CUSTCONT.csv to datatable
                Dim FilePath_CUSTCONT As String = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'CUSTCONT_File_Path'")
                Dim FileName_CUSTCONT As String = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'CUSTCONT_File_Name'")
                Dim FileName_CUSTCONT_ZIP As String = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'CUSTCONT_File_Name_byZip'")
                Dim Type_CUSTCONT As String = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'CUSTCONT_Type'")

                If FilePath_CUSTCONT.Substring(FilePath_CUSTCONT.Length - 1, 1) <> "\" Then
                    FilePath_CUSTCONT = FilePath_CUSTCONT & "\"
                End If

                Dim csvFilePath_CUSTCONT As String = String.Empty
                Dim csvFileName_CUSTCONT As String = String.Empty
                Dim str_full_Path_zip_file As String = String.Empty
                Dim fd As OpenFileDialog

                Dim previous_working_date As Date = GetWorkingDays(Now, -1)

                Select Case Type_CUSTCONT
                    Case "csv Format (Fixed)"
                        csvFileName_CUSTCONT = FileName_CUSTCONT
                        csvFilePath_CUSTCONT = FilePath_CUSTCONT
                        If Not My.Computer.FileSystem.FileExists(csvFilePath_CUSTCONT & csvFileName_CUSTCONT) Then
                            Dim folderDlg As New FolderBrowserDialog
                            folderDlg.ShowNewFolderButton = True

                            If (folderDlg.ShowDialog() = DialogResult.OK) Then
                                csvFilePath_CUSTCONT = folderDlg.SelectedPath
                                Dim root As Environment.SpecialFolder = folderDlg.RootFolder
                            End If
                        End If
                        If csvFilePath_CUSTCONT.Length = 0 Then Exit Sub
                    Case "zip Format (ByDate)"

                        str_full_Path_zip_file = FilePath_CUSTCONT & previous_working_date.ToString("yyyy") & "\" & CInt(previous_working_date.ToString("MM")) & "\" & CInt(previous_working_date.ToString("dd")) & "\" & FileName_CUSTCONT_ZIP

                        If Not My.Computer.FileSystem.FileExists(str_full_Path_zip_file) Then
                            fd = New OpenFileDialog()
                            fd.Title = "Select Zip file report CUSTCONT.csv.zip"
                            fd.RestoreDirectory = True
                            fd.Filter = "|All Files|*.*" +
                                        "Zip File|*.zip"

                            fd.FilterIndex = 2
                            fd.RestoreDirectory = True

                            If fd.ShowDialog() = DialogResult.OK Then
                                If fd.FileName.Length > 0 Then
                                    str_full_Path_zip_file = fd.FileName
                                End If
                            End If
                        End If

                        If str_full_Path_zip_file.Length = 0 Then Exit Sub

                        ZipFile.ExtractToDirectory(str_full_Path_zip_file, appPath)
                        csvFileName_CUSTCONT = FileName_CUSTCONT
                        csvFilePath_CUSTCONT = appPath

                    Case "csv Format (ByDate)"
                        csvFilePath_CUSTCONT = FilePath_CUSTCONT & previous_working_date.ToString("yyyy") & "\" & CInt(previous_working_date.ToString("MM")) & "\" & CInt(previous_working_date.ToString("dd"))
                        csvFileName_CUSTCONT = FileName_CUSTCONT

                        If Not My.Computer.FileSystem.FileExists(csvFilePath_CUSTCONT) Then
                            fd = New OpenFileDialog()
                            fd.Title = "Select csv file report CUSTCONT.csv"
                            fd.RestoreDirectory = True
                            fd.Filter = "|All Files|*.*" +
                                        "Comma delimited|*.csv"

                            fd.FilterIndex = 2
                            fd.RestoreDirectory = True

                            If fd.ShowDialog() = DialogResult.OK Then
                                If fd.FileName.Length > 0 Then
                                    csvFilePath_CUSTCONT = fd.FileName
                                End If
                            End If
                        End If

                        If csvFilePath_CUSTCONT.Length = 0 Then Exit Sub

                    Case "zip Format (Fixed)"
                        str_full_Path_zip_file = FilePath_CUSTCONT & FileName_CUSTCONT_ZIP

                        If Not My.Computer.FileSystem.FileExists(str_full_Path_zip_file) Then
                            fd = New OpenFileDialog()
                            fd.Title = "Select Zip file report CUSTCONT.csv.zip"
                            fd.RestoreDirectory = True
                            fd.Filter = "|All Files|*.*" +
                                        "Zip File|*.zip"

                            fd.FilterIndex = 2
                            fd.RestoreDirectory = True

                            If fd.ShowDialog() = DialogResult.OK Then
                                If fd.FileName.Length > 0 Then
                                    str_full_Path_zip_file = fd.FileName
                                End If
                            End If
                        End If

                        If str_full_Path_zip_file.Length = 0 Then Exit Sub

                        ZipFile.ExtractToDirectory(str_full_Path_zip_file, appPath)

                        csvFileName_CUSTCONT = FileName_CUSTCONT
                        csvFilePath_CUSTCONT = appPath
                End Select

                Dim conn_CUSTCONT As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & csvFilePath_CUSTCONT & ";Extended Properties='Text;HDR=Yes;FMT=CSVDelimited'")

                conn_CUSTCONT.Open()
                sql = "Select DISTINCT [MASTERNO] AS MasterNo, [CONTACT]&IIF(LEN([ATTENTION PARTY])>0,' ('&[ATTENTION PARTY]&') ','') AS Attention" &
                    " FROM [" & csvFileName_CUSTCONT & "]" &
                    " WHERE [CONTACT TYPE CODE] IN ('MOB','TOF','TRS','MO2') AND [FULL NAME] <> 'DUPLICATED RELATIONSHIP' AND [CONTACT] <> '.' AND UCASE([CONTACT]) NOT LIKE '%CLOSED' AND UCASE([CONTACT]) NOT LIKE '%CLOSE' AND UCASE([CONTACT]) NOT LIKE '%CLS' AND UCASE([CONTACT]) NOT LIKE 'CLOSED%' AND UCASE([CONTACT]) NOT LIKE 'CLOSE%' AND UCASE([CONTACT]) NOT LIKE 'CLS%'"

                Dim cmd_CUSTCONT As OleDbCommand = New OleDbCommand(sql, conn_CUSTCONT)
                Dim da_CUSTCONT As OleDbDataAdapter = New OleDbDataAdapter(cmd_CUSTCONT)
                Dim dt_CUSTCONT As DataTable = New DataTable
                da_CUSTCONT.Fill(dt_CUSTCONT)

                SQL_QUERY(file_name_Portfolio, "CREATE TABLE If Not EXISTS CUSTCONT (MasterNo VARCHAR, Attention VARCHAR)")
                SQLITE_BULK_COPY(dt_CUSTCONT, file_name_Portfolio, "CUSTCONT")
                conn_CUSTCONT.Close()

                'Create FINAL REPORT PORTFOLIO
                SQL_QUERY(file_name_Portfolio, "CREATE TABLE If Not EXISTS Portfolio (MasterNo VARCHAR, Client_Name VARCHAR, Client_Address VARCHAR, Client_Attention VARCHAR)")

                sql = "SELECT Portfolio_temp.MasterNo AS MasterNo, Portfolio_temp.Client_Name AS Client_Name, Portfolio_temp.Client_Address AS Client_Address, CUSTCONT.Attention AS Client_Attention FROM Portfolio_temp" &
                        " INNER JOIN CUSTCONT ON CUSTCONT.MasterNo=Portfolio_temp.MasterNo" &
                        " UNION ALL" &
                        " SELECT Portfolio_temp.MasterNo AS MasterNo, Portfolio_temp.Client_Name AS Client_Name, Portfolio_temp.Client_Address AS Client_Address, CUSTCONT.Attention AS Client_Attention FROM Portfolio_temp" &
                        " LEFT OUTER JOIN CUSTCONT ON CUSTCONT.MasterNo=Portfolio_temp.MasterNo" &
                        " WHERE CUSTCONT.MasterNo IS NULL"

                Dim DT_FINAL As DataTable = SQL_QUERY_TO_DATATABLE(file_name_Portfolio, sql)
                SQLITE_BULK_COPY(DT_FINAL, file_name_Portfolio, "Portfolio")

                'SQL_QUERY(file_name_Portfolio, "DROP TABLE CUSTCONT")
                'SQL_QUERY(file_name_Portfolio, "DROP TABLE Portfolio_temp")
                'SQL_QUERY(file_name_Portfolio, "vacuum")

                If My.Computer.FileSystem.FileExists(appPath & "CUSTCONT.csv") Then
                    Kill(appPath & "CUSTCONT.csv")
                Else
                    If My.Computer.FileSystem.FileExists(appPath & "\CUSTCONT.csv") Then
                        Kill(appPath & "\CUSTCONT.csv")
                    End If
                End If
            End If
        Catch ex As Exception
            If My.Computer.FileSystem.FileExists(appPath & "CUSTCONT.csv") Then
                Kill(appPath & "CUSTCONT.csv")
            Else
                If My.Computer.FileSystem.FileExists(appPath & "\CUSTCONT.csv") Then
                    Kill(appPath & "\CUSTCONT.csv")
                End If
            End If
            If My.Computer.FileSystem.FileExists(file_name_Portfolio) Then
                Kill(file_name_Portfolio)
            End If
            MsgBox(ex.Message, vbCritical)
        End Try

    End Sub
    Public Sub Check_Version_CUSTDTL_FOB_Report(link_folder_database As String)
        On Error GoTo err_handle

        Dim link_application_config As String = SQL_QUERY_TO_STRING(link_app_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'Link_Application_Config'")
        Dim file_name_Portfolio As String
        If link_folder_database.Substring(link_folder_database.Length - 1, 1) = "\" Then
            file_name_Portfolio = link_folder_database & "Database_Portfolio_" & Now.ToString("ddMMyyyy") & ".txt"
        Else
            file_name_Portfolio = link_folder_database & "\Database_Portfolio_" & Now.ToString("ddMMyyyy") & ".txt"
        End If

        'Load CUSTDTL.csv to datatable
        Dim csvFilePath As String = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'CUSTDTL_File_Path'")
        Dim csvFileName As String = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'CUSTDTL_File_Name'")

        If csvFilePath.Substring(csvFilePath.Length - 1, 1) <> "\" Then
            csvFilePath = csvFilePath & "\"
        End If

        If Not My.Computer.FileSystem.FileExists(csvFilePath & "\" & csvFileName) Then
            Dim folderDlg_CUSTDTL As New FolderBrowserDialog
            folderDlg_CUSTDTL.ShowNewFolderButton = True

            If (folderDlg_CUSTDTL.ShowDialog() = DialogResult.OK) Then
                csvFilePath = folderDlg_CUSTDTL.SelectedPath
                Dim root As Environment.SpecialFolder = folderDlg_CUSTDTL.RootFolder
            End If
        End If


        'If IO.File.GetLastWriteTime(csvFilePath & csvFileName).ToString("dd/MM/yyyy") <> Now.ToString("dd/MM/yyyy") Then
        '    Dim iret_CUSTDTL = MsgBox("Report CUSTDTL has been modified on " & IO.File.GetLastWriteTime(csvFilePath & "\" & csvFileName).ToString("dd/MM/yyyy") & " .Do you want to get latest source file Portfolio?", vbYesNo)

        '    If iret_CUSTDTL = vbYes Then
        '        Dim folderDlg_CUSTDTL As New FolderBrowserDialog
        '        folderDlg_CUSTDTL.ShowNewFolderButton = True

        '        If (folderDlg_CUSTDTL.ShowDialog() = DialogResult.OK) Then
        '            csvFilePath = folderDlg_CUSTDTL.SelectedPath
        '            Dim root As Environment.SpecialFolder = folderDlg_CUSTDTL.RootFolder
        '        End If
        '    End If
        'End If

        Dim conn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & csvFilePath & ";Extended Properties='Text;HDR=Yes;FMT=CSVDelimited'")

        conn.Open()
        Dim sql = "Select DISTINCT [Master Number] AS MasterNo, [Add Type Code] AS Add_Type_Code, IIF(LEN([Flat No])>0,[Flat No]& ', ','')&IIF(LEN([Bldg Name])>0,[Bldg Name]& ', ','')&IIF(LEN(Street)>0,Street& ', ','') as Client_Address, [Country Code] as Country_Code" &
            " FROM [" & csvFileName & "] WHERE [Add Type Code] = 'MAI' OR [Add Type Code] = 'LEG' OR [Add Type Code] = 'RES'"

        Dim cmd2 As OleDbCommand = New OleDbCommand(sql, conn)
        Dim da As OleDbDataAdapter = New OleDbDataAdapter(cmd2)
        Dim dt As DataTable = New DataTable
        da.Fill(dt)

        ' Create the SQLite database
        SQL_QUERY(file_name_Portfolio, "CREATE TABLE If Not EXISTS CUSTDTL (MasterNo VARCHAR, Add_Type_Code VARCHAR, Client_Address VARCHAR, Country_Code VARCHAR)")

        SQLITE_BULK_COPY(dt, file_name_Portfolio, "CUSTDTL")
        conn.Close()

        Exit Sub
err_handle:
        If My.Computer.FileSystem.FileExists(appPath & "CUSTCONT.csv") Then
            Kill(appPath & "CUSTCONT.csv")
        Else
            If My.Computer.FileSystem.FileExists(appPath & "\CUSTCONT.csv") Then
                Kill(appPath & "\CUSTCONT.csv")
            End If
        End If
        If My.Computer.FileSystem.FileExists(file_name_Portfolio) Then
            Kill(file_name_Portfolio)
        End If
        Module_Letter_Management.Error_handle()
    End Sub
    Function ConvertToUnSign(ByVal sContent As String) As String
        Dim i As Long
        Dim intCode As Long
        Dim sChar As String
        Dim sConvert As String = String.Empty
        ConvertToUnSign = AscW(sContent)
        For i = 1 To Len(sContent)
            sChar = Mid(sContent, i, 1)
            If sChar <> String.Empty Then
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
    Public Sub Mail_Merge(vendor_name As String, link_database As String, table_name As String, database_name As String, GRIDVIEW As GridView, template_print_bill As String)
        On Error GoTo err_handle

        Dim strExcelFile As String = appPath & "list_print_bill_" & Now.ToString("ddMMyyyyhhmmss") & ".xls"

        GRIDVIEW.ExportToXls(strExcelFile)

        Dim ObjExcel As Object = CreateObject("Excel.Application")
        ObjExcel.Visible = True
        Dim Objwb As Object = ObjExcel.Workbooks.open(strExcelFile)
        Dim Objws As Object = Objwb.Sheets("Sheet")
        Objws.Cells.NumberFormat = "@"

        Objws.Cells(1, GRIDVIEW.Columns.Count + 1) = "No"
        Objws.Cells(1, GRIDVIEW.Columns.Count + 2) = "Database_Name"

        For i = GRIDVIEW.RowCount + 1 To 2 Step -1
            If Objws.Range("A" & i).Value <> vendor_name Then
                Objws.Rows(i).delete
            End If
        Next

        Dim numRow As Integer = 1
        While (Objws.Cells(numRow + 1, 4).Value IsNot Nothing)
            numRow = numRow + 1
        End While

        If numRow = 1 Then
            MsgBox("No case with Vendor " & vendor_name & " for create bill")
            Exit Sub
        End If






        Dim wdAffDoc As Object
        Dim wdAffApp As Object = CreateObject("Word.Application")
        wdAffDoc = wdAffApp.Documents.Open(template_print_bill)

        wdAffApp.Visible = True

        wdAffDoc.MailMerge.OpenDataSource(Name:=strExcelFile, SQLStatement:="SELECT * FROM [Sheet$]")

        '.Destination 0 = DOCUMENT, 1 = PRINTER
        wdAffApp.ActiveDocument.MailMerge.Destination = 0 'send to new document

        With wdAffApp.ActiveDocument.MailMerge.DataSource
            .FirstRecord = 1 'wdDefaultFirstRecord
            .LastRecord = -16 'wdDefaultLastRecord
        End With
        wdAffApp.ActiveDocument.MailMerge.Execute(Pause:=False)

        wdAffDoc.Close(SaveChanges:=False)

        wdAffDoc = Nothing
        wdAffApp = Nothing




        Dim number_col_FinalResult As Integer
        Dim number_col_Bill_No As Integer

        For i = 1 To GRIDVIEW.Columns.Count
            If Objws.cells(1, i).value = "Final_Result" Then
                number_col_FinalResult = i
            End If
            If Objws.cells(1, i).value = "Bill_Number" Then
                number_col_Bill_No = i
            End If
        Next

        For i = 2 To numRow
            Objws.Cells(i, GRIDVIEW.Columns.Count + 1) = i - 1
            Objws.Cells(i, GRIDVIEW.Columns.Count + 2) = database_name & "_" & Environment.UserName
            'If Objws.cells(i, number_col_FinalResult).value = "Waiting" Then
            SQL_QUERY(link_database, "UPDATE " & table_name & " SET [Tasetco_packet_name] = '" & Now.ToString("dd/MM/yyyy") & "',[Final_Result] = 'Incomplete' WHERE [Bill_Number] = '" & Objws.cells(i, number_col_Bill_No).value & "'")
            'End If
        Next

        Objwb.Save()
        Objwb.Close()
        ObjExcel.Quit()


        Kill(strExcelFile)

        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub
    Public Function CHECK_TABLE_EXIST_OR_NOT(ByVal link_database As String, TABLE_NAME_CHECKED As String) As Boolean
        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & ";version=3;new=False;datetimeformat=CurrentCulture;")
        MYCONNECTION.Open()
        Dim CMD As New SQLiteCommand
        CMD.CommandText = "select name from sqlite_master where type='table' order by name"
        CMD.Connection = MYCONNECTION
        Dim RDR As SQLiteDataReader = CMD.ExecuteReader
        Dim DS As New DataTable
        DS.Load(RDR)
        RDR.Close()
        Dim result As Boolean = False
        For Each Drr In DS.Rows
            If Drr(0).ToString = TABLE_NAME_CHECKED Then
                result = True
            End If
        Next

        MYCONNECTION.Close()
        Return result
    End Function
    Sub AUTO_COMPLETE_GET_DATA(ByVal dataCollection As AutoCompleteStringCollection, link_database As String, SQL_String As String)
        Try
            Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & ";version=3;new=False;datetimeformat=CurrentCulture;")
            MYCONNECTION.Open()
            Dim CMD As New SQLiteCommand
            CMD.CommandText = SQL_String
            CMD.Connection = MYCONNECTION
            Dim RDR As SQLiteDataReader = CMD.ExecuteReader
            Dim DS As New DataTable
            DS.Load(RDR)
            RDR.Close()

            For Each row As DataRow In DS.Rows
                dataCollection.Add(row(0).ToString())
            Next

            MYCONNECTION.Close()
        Catch ex As Exception
            MessageBox.Show("Can not open connection ! ")
        End Try
    End Sub
    Public Function GetWorkingDays(ByVal DateIn As DateTime, ByVal ShiftDate As Integer) As DateTime
        ' Adds the [ShiftDate] number of working days to DateIn
        Dim datDate As DateTime = DateIn.AddDays(ShiftDate)
        ' Loop around until we get the need non-weekend day
        While Weekday(datDate) = 1 Or Weekday(datDate) = 7
            datDate = datDate.AddDays(IIf(ShiftDate < 0, -1, 1))
        End While
        Return datDate
    End Function

    Public Sub backup_database(folder_path As String)
        If folder_path.Substring(folder_path.Length - 1, 1) <> "\" Then
            folder_path = folder_path & "\"
        End If

        Dim full_name_backup_Tasetco As String = folder_path & "\BACKUP\Database_Letter_Management_" & Now().ToString("yyyyMMdd") & ".txt"

        If Not My.Computer.FileSystem.FileExists(full_name_backup_Tasetco) Then
            If My.Computer.FileSystem.FileExists(folder_path & "Database_Letter_Management.txt") Then
                My.Computer.FileSystem.CopyFile(folder_path & "Database_Letter_Management.txt", full_name_backup_Tasetco)
            End If
        End If
    End Sub
    Public Sub EnableDoubleBuffered(ByVal dgv As DataGridView)

        Dim dgvType As Type = dgv.[GetType]()

        Dim pi As PropertyInfo = dgvType.GetProperty("DoubleBuffered",
                                                     BindingFlags.Instance Or BindingFlags.NonPublic)

        pi.SetValue(dgv, True, Nothing)

    End Sub
    Public Sub LOAD_DATABASE_TASETCO_TO_GRIDVIEW(Link_database As String, table_name As String, str_Filter As String, str_Search As String, GridControl_Tasetco As DataGridView)
        Dim TimerStart As DateTime
        TimerStart = Now
        GridControl_Tasetco.DataSource = Nothing

        Dim i As Integer = 0
        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & Link_database & ";version=3;new=False;datetimeformat=CurrentCulture;")
        MYCONNECTION.Open()
        Dim CMD As New SQLiteCommand
        Dim SQL_String As String = String.Empty

        Dim CMD2 As New SQLiteCommand
        CMD2.CommandText = "SELECT * FROM " & table_name & " LIMIT 1"
        CMD2.Connection = MYCONNECTION
        Dim RDR2 As SQLiteDataReader = CMD2.ExecuteReader
        Dim DS2 As New DataTable
        DS2.Load(RDR2)
        RDR2.Close()
        Dim List_col_in_database As String = String.Empty
        For i = 0 To DS2.Columns.Count - 1
            If List_col_in_database.Length = 0 Then
                List_col_in_database = "[" & DS2.Columns(i).ColumnName.ToString() & "]"
            Else
                List_col_in_database = List_col_in_database & ", [" & DS2.Columns(i).ColumnName.ToString() & "]"
            End If
        Next

        Select Case str_Filter
            Case "TODAY"
                SQL_String = "SELECT * FROM " & table_name & " WHERE [Sent_date] = " & Now().ToString("dd/MM/yyyy") & " ORDER BY [Vendor_Name] ASC, CAST(SUBSTR(Sent_date, 7, 4) AS INT) DESC, CAST(SUBSTR(Sent_date, 4, 2) AS INT) DESC, CAST(SUBSTR(Sent_date, 1, 2) AS INT) DESC, [Bill_Number] ASC"
            Case "All"
                SQL_String = "SELECT * FROM " & table_name & " ORDER BY [Vendor_Name] ASC, CAST(SUBSTR(Sent_date, 7, 4) AS INT) DESC, CAST(SUBSTR(Sent_date, 4, 2) AS INT) DESC, CAST(SUBSTR(Sent_date, 1, 2) AS INT) DESC, [Bill_Number] ASC"
            Case "Completed"
                SQL_String = "SELECT * FROM " & table_name & " WHERE [Final_result] = 'Completed' ORDER BY [Vendor_Name] ASC, CAST(SUBSTR(Sent_date, 7, 4) AS INT) DESC, CAST(SUBSTR(Sent_date, 4, 2) AS INT) DESC, CAST(SUBSTR(Sent_date, 1, 2) AS INT) DESC, [Bill_Number] ASC"
            Case "Incomplete"
                SQL_String = "SELECT * FROM " & table_name & " WHERE [Final_result] = 'Incomplete' ORDER BY [Vendor_Name] ASC, CAST(SUBSTR(Sent_date, 7, 4) AS INT) DESC, CAST(SUBSTR(Sent_date, 4, 2) AS INT) DESC, CAST(SUBSTR(Sent_date, 1, 2) AS INT) DESC, [Bill_Number] ASC"
            Case "CROWN"
                SQL_String = "SELECT * FROM " & table_name & " WHERE [Final_result] = 'CROWN' ORDER BY [Vendor_Name] ASC, CAST(SUBSTR(Sent_date, 7, 4) AS INT) DESC, CAST(SUBSTR(Sent_date, 4, 2) AS INT) DESC, CAST(SUBSTR(Sent_date, 1, 2) AS INT) DESC, [Bill_Number] ASC"
            Case "Sent_Date"
                Dim str_sent_date As String = InputBox("Please input sent date for filter:", "Letter Management - Message")
                SQL_String = "SELECT * FROM " & table_name & " WHERE [Sent_date] LIKE '" & str_sent_date & "' ORDER BY [Vendor_Name] ASC, CAST(SUBSTR(Sent_date, 7, 4) AS INT) DESC, CAST(SUBSTR(Sent_date, 4, 2) AS INT) DESC, CAST(SUBSTR(Sent_date, 1, 2) AS INT) DESC, [Bill_Number] ASC"
            Case "Other"
                Dim str_filter_by_where As String = InputBox("Please input value for filter:", "Letter Management - Message")
                SQL_String = "SELECT * FROM " & table_name & " WHERE " & str_filter_by_where & " ORDER BY [Vendor_Name] ASC, CAST(SUBSTR(Sent_date, 7, 4) AS INT) DESC, CAST(SUBSTR(Sent_date, 4, 2) AS INT) DESC, CAST(SUBSTR(Sent_date, 1, 2) AS INT) DESC, [Bill_Number] ASC"
            Case String.Empty
                Dim SQL_Str_Search As String = String.Empty
                For i = 0 To DS2.Columns.Count - 1
                    If SQL_Str_Search.Length = 0 Then
                        SQL_Str_Search = "[" & DS2.Columns(i).ColumnName.ToString() & "] LIKE '%" & str_Search & "%' "
                    Else
                        SQL_Str_Search = SQL_Str_Search & " OR [" & DS2.Columns(i).ColumnName.ToString() & "] LIKE '%" & str_Search & "%' "
                    End If
                Next
                SQL_String = "SELECT * FROM " & table_name & " WHERE " & SQL_Str_Search & " ORDER BY [Vendor_Name] ASC, CAST(SUBSTR(Sent_date, 7, 4) AS INT) DESC, CAST(SUBSTR(Sent_date, 4, 2) AS INT) DESC, CAST(SUBSTR(Sent_date, 1, 2) AS INT) DESC, [Bill_Number] ASC"
                SQL_Str_Search = String.Empty
        End Select

        CMD.CommandText = SQL_String
        CMD.Connection = MYCONNECTION
        Dim RDR As SQLiteDataReader = CMD.ExecuteReader
        Dim DS As New DataTable
        DS.Load(RDR)
        RDR.Close()

        GridControl_Tasetco.DataSource = DS

        Dim TimeSpent As System.TimeSpan
        TimeSpent = Now.Subtract(TimerStart)

        If str_Filter = "TODAY" Then
            Main.Statusbar_item1.Caption = "[" & Now().ToString("dd/MM/yyyy") & "] Total: " & GridControl_Tasetco.Rows.Count
        Else
            Main.Statusbar_item1.Caption = "Total: " & GridControl_Tasetco.Rows.Count
        End If

        Main.Statusbar_item2.Caption = "Load database in " & Format(TimeSpent.TotalSeconds, "0.00") & " seconds"
        MYCONNECTION.Close()
    End Sub
    Public Sub LOAD_DATABASE_TASETCO_TO_GRIDVIEW2(Link_database As String, table_name As String, str_Filter As String, str_Search As String, GRID_CONTROL As DevExpress.XtraGrid.GridControl, GRIDVIEW As GridView)
        Dim TimerStart As DateTime
        TimerStart = Now

        Dim i As Integer = 0
        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & Link_database & ";version=3")
        MYCONNECTION.Open()
        Dim CMD As New SQLiteCommand
        Dim SQL_String As String = String.Empty

        Dim CMD2 As New SQLiteCommand
        CMD2.CommandText = "SELECT * FROM " & table_name & " LIMIT 1"
        CMD2.Connection = MYCONNECTION
        Dim RDR2 As SQLiteDataReader = CMD2.ExecuteReader
        Dim DS2 As New DataTable
        DS2.Load(RDR2)
        RDR2.Close()

        Select Case str_Filter
            Case "TODAY"
                SQL_String = "SELECT * FROM " & table_name & " WHERE Sent_date = '" & Now().ToString("dd/MM/yyyy") & "' ORDER BY [Vendor_Name] ASC, CAST(SUBSTR(Sent_date, 7, 4) AS INT) DESC, CAST(SUBSTR(Sent_date, 4, 2) AS INT) DESC, CAST(SUBSTR(Sent_date, 1, 2) AS INT) DESC, [Bill_Number] ASC"
            Case "All"
                SQL_String = "SELECT * FROM " & table_name & " ORDER BY [Vendor_Name] ASC, CAST(SUBSTR(Sent_date, 7, 4) AS INT) DESC, CAST(SUBSTR(Sent_date, 4, 2) AS INT) DESC, CAST(SUBSTR(Sent_date, 1, 2) AS INT) DESC, [Bill_Number] ASC"
            Case "Waiting"
                SQL_String = "SELECT * FROM " & table_name & " WHERE [Final_result] = 'Waiting' ORDER BY [Vendor_Name] ASC, CAST(SUBSTR(Sent_date, 7, 4) AS INT) DESC, CAST(SUBSTR(Sent_date, 4, 2) AS INT) DESC, CAST(SUBSTR(Sent_date, 1, 2) AS INT) DESC, [Bill_Number] ASC"
            Case "Completed"
                SQL_String = "SELECT * FROM " & table_name & " WHERE [Final_result] = 'Completed' ORDER BY [Vendor_Name] ASC, CAST(SUBSTR(Sent_date, 7, 4) AS INT) DESC, CAST(SUBSTR(Sent_date, 4, 2) AS INT) DESC, CAST(SUBSTR(Sent_date, 1, 2) AS INT) DESC, [Bill_Number] ASC"
            Case "Incomplete"
                SQL_String = "SELECT * FROM " & table_name & " WHERE [Final_result] = 'Incomplete' ORDER BY [Vendor_Name] ASC, CAST(SUBSTR(Sent_date, 7, 4) AS INT) DESC, CAST(SUBSTR(Sent_date, 4, 2) AS INT) DESC, CAST(SUBSTR(Sent_date, 1, 2) AS INT) DESC, [Bill_Number] ASC"
            Case "CROWN"
                SQL_String = "SELECT * FROM " & table_name & " WHERE [Final_result] = 'CROWN' ORDER BY [Vendor_Name] ASC, CAST(SUBSTR(Sent_date, 7, 4) AS INT) DESC, CAST(SUBSTR(Sent_date, 4, 2) AS INT) DESC, CAST(SUBSTR(Sent_date, 1, 2) AS INT) DESC, [Bill_Number] ASC"
            Case "Sent_date"
                Dim str_sent_date = InputBox("Please input sent date for filter:", "Letter Management - Message")
                SQL_String = "SELECT * FROM " & table_name & " WHERE [Sent_date] = '" & str_sent_date & "' ORDER BY [Vendor_Name] ASC, CAST(SUBSTR(Sent_date, 7, 4) AS INT) DESC, CAST(SUBSTR(Sent_date, 4, 2) AS INT) DESC, CAST(SUBSTR(Sent_date, 1, 2) AS INT) DESC, [Bill_Number] ASC"
            Case "Other"
                Dim str_filter_by_where = InputBox("Please input value for filter:", "Letter Management - Message")
                SQL_String = "SELECT * FROM " & table_name & " WHERE " & str_filter_by_where & " ORDER BY [Vendor_Name] ASC, CAST(SUBSTR(Sent_date, 7, 4) AS INT) DESC, CAST(SUBSTR(Sent_date, 4, 2) AS INT) DESC, CAST(SUBSTR(Sent_date, 1, 2) AS INT) DESC, [Bill_Number] ASC"
            Case String.Empty
                Dim SQL_Str_Search As String = String.Empty
                For i = 0 To DS2.Columns.Count - 1
                    If SQL_Str_Search.Length = 0 Then
                        SQL_Str_Search = "[" & DS2.Columns(i).ColumnName.ToString() & "] LIKE '%" & str_Search & "%' "
                    Else
                        SQL_Str_Search = SQL_Str_Search & " OR [" & DS2.Columns(i).ColumnName.ToString() & "] LIKE '%" & str_Search & "%' "
                    End If
                Next
                SQL_String = "SELECT * FROM " & table_name & " WHERE " & SQL_Str_Search & " ORDER BY [Vendor_Name] ASC, CAST(SUBSTR(Sent_date, 7, 4) AS INT) DESC, CAST(SUBSTR(Sent_date, 4, 2) AS INT) DESC, CAST(SUBSTR(Sent_date, 1, 2) AS INT) DESC, [Bill_Number] ASC"
                SQL_Str_Search = String.Empty
        End Select

        CMD.CommandText = SQL_String
        CMD.Connection = MYCONNECTION
        Dim RDR As SQLiteDataReader = CMD.ExecuteReader
        Dim DS As New DataTable
        DS.Load(RDR)
        RDR.Close()

        GRID_CONTROL.DataSource = Nothing
        GRIDVIEW.Columns.Clear()
        GRID_CONTROL.DataSource = DS

        Dim TimeSpent As System.TimeSpan
        TimeSpent = Now.Subtract(TimerStart)

        If str_Filter = "TODAY" Then
            Main.Statusbar_item1.Caption = "[" & Now().ToString("dd/MM/yyyy") & "] Total: " & GRIDVIEW.RowCount.ToString
        Else
            Main.Statusbar_item1.Caption = "Total: " & GRIDVIEW.RowCount.ToString
        End If

        Main.Statusbar_item2.Caption = "Load database in " & Format(TimeSpent.TotalSeconds, "0.00") & " seconds"
        MYCONNECTION.Close()
    End Sub

End Module