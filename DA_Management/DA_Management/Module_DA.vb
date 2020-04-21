Imports System.Reflection
Imports System.Data.SQLite
Imports DevExpress.XtraGrid.Views.Grid
Imports Excel = Microsoft.Office.Interop.Excel
Imports System.Data.OleDb
Imports Microsoft.Data.Sqlite.Extensions

Module Module_DA
    Public appPath As String = AppDomain.CurrentDomain.BaseDirectory
    Public link_app_config As String = appPath & "App_Config.txt"
    Public EncryptDecrypt As New Simple3Des("0915330999")
    Public check_password As Boolean

    Private Sub WriteErrorLog(strErrorText As String)
        Try
            Dim strPath As String = SQL_QUERY_TO_STRING_NO_LOG(link_app_config, "SELECT Field_Value FROM Config WHERE Field_Name = 'Link_Folder_Path_DA'")
            If strPath.Substring(strPath.Length - 1, 1) <> "\" Then
                strPath = strPath & "\"
            End If
            strPath = strPath & "LOG\Error\"

            If (Not System.IO.Directory.Exists(strPath)) Then
                System.IO.Directory.CreateDirectory(strPath)
            End If

            Dim strFileName As String = "errorLog_" & Environment.UserName & ".txt"
            System.IO.File.AppendAllText(strPath & strFileName, (strErrorText & Convert.ToString(" - ")) + DateTime.Now.ToString() + vbCr & vbLf)

        Catch ex As Exception
            WriteErrorLog("Error in WriteErrorLog: " + ex.Message)
        End Try
    End Sub

    Public Sub WriteLog_Full(strText As String)
        Try
            Dim strFileName As String = "Log_DA_" & Environment.UserName & "_" & Now.ToString("yyyyMMdd") & ".txt"
            Dim strPath As String = SQL_QUERY_TO_STRING_NO_LOG(link_app_config, "SELECT Field_Value FROM Config WHERE Field_Name = 'Link_Folder_Path_DA'")
            If strPath.Substring(strPath.Length - 1, 1) <> "\" Then
                strPath = strPath & "\"
            End If
            strPath = strPath & "LOG\Full\"

            If (Not System.IO.Directory.Exists(strPath)) Then
                System.IO.Directory.CreateDirectory(strPath)
            End If

            System.IO.File.AppendAllText(strPath & strFileName, (DateTime.Now.ToString() & Convert.ToString(" - ")) + strText + vbLf + vbLf)

        Catch ex As Exception
            WriteErrorLog("Error in WriteErrorLog: " + ex.Message)
        End Try
    End Sub

    Public Sub WriteLog(strText As String)
        Try
            Dim strFileName As String = "Log_DA_" & Environment.UserName & "_" & Now.ToString("yyyyMMdd") & ".txt"
            Dim strPath As String = SQL_QUERY_TO_STRING_NO_LOG(link_app_config, "SELECT Field_Value FROM Config WHERE Field_Name = 'Link_Folder_Path_DA'")
            If strPath.Substring(strPath.Length - 1, 1) <> "\" Then
                strPath = strPath & "\"
            End If
            strPath = strPath & "LOG\"

            If (Not System.IO.Directory.Exists(strPath)) Then
                System.IO.Directory.CreateDirectory(strPath)
            End If

            System.IO.File.AppendAllText(strPath & strFileName, (DateTime.Now.ToString() & Convert.ToString(" - ")) + strText + vbLf + vbLf)

        Catch ex As Exception
            WriteErrorLog("Error in WriteErrorLog: " + ex.Message)
        End Try
    End Sub

    Public Sub Error_handle()
        WriteErrorLog(Err.Number & " _ " & Err.Description)
        Call MsgBox("Contact To Administrator !!!" & Chr(10) & Chr(10) & Err.Number & Err.Description, vbCritical)
        Err.Clear()
    End Sub

    Public Function SQL_QUERY_TO_DATATABLE(Link_database As String, sql_string As String) As DataTable
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
        WriteLog_Full("[SQL QUERY TO DATATABLE] - " & sql_string & vbLf & " - Link database: " & Link_database)
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
        Else
            result = ""
        End If
        WriteLog_Full("[SQL QUERY TO STRING] - " & sql_string & vbLf & " - Link database: " & Link_database & vbLf & " - Result: " & result)
        Return result
    End Function

    Public Function SQL_QUERY_TO_STRING_NO_LOG(Link_database As String, sql_string As String) As String

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
        Else
            result = ""
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
        WriteLog_Full("[SQL QUERY TO INTEGER] - " & sql_string & vbLf & " - Link database: " & Link_database & vbLf & " - Result: " & result)
        Return result
    End Function

    Public Function SQL_QUERY_TO_BOOLEAN(Link_database As String, sql_string As String) As Boolean
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
        WriteLog_Full("[SQL QUERY TO BOOLEAN] - " & sql_string & vbLf & " - Link database: " & Link_database & vbLf & " - Result: " & result)
        Return result
    End Function

    Public Sub SQL_QUERY(Link_database As String, sql_string As String)
        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & Link_database & "; Version=3;")
        MYCONNECTION.Open()
        Dim CMD As New SQLiteCommand
        CMD.CommandText = sql_string
        CMD.Connection = MYCONNECTION
        Dim RDR As SQLiteDataReader = CMD.ExecuteReader

        WriteLog_Full("[SQL QUERY] - " & sql_string & vbLf & " - Link database: " & Link_database)
    End Sub

    Public Sub SQL_QUERY_WRITE_LOG(Link_database As String, sql_string As String)
        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & Link_database & "; Version=3;")
        MYCONNECTION.Open()
        Dim CMD As New SQLiteCommand
        CMD.CommandText = sql_string
        CMD.Connection = MYCONNECTION
        Dim RDR As SQLiteDataReader = CMD.ExecuteReader

        WriteLog_Full("[SQL QUERY] - " & sql_string & vbLf & " - Link database: " & Link_database)
        WriteLog("[SQL QUERY] - " & sql_string & vbLf & " - Link database: " & Link_database)
    End Sub

    Public Function SQL_SEARCH_IN_ALL_COLUMN(link_database As String, table_name As String, str_search As String) As DataTable
        Dim SQL_Str_Search As String = ""
        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & "; Version=3;")
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
        SQL_Str_Search = ""
        Dim DT2 As DataTable = SQL_QUERY_TO_DATATABLE(link_database, SQL_String)

        WriteLog_Full("SEACH VALUE '" & str_search & "' IN ALL COLUMN " & " - Table name: " & table_name & vbLf & " - Link database: " & link_database)
        Return DT2
    End Function

    Public Function CHECK_COLUMN_EXISTS(link_database As String, table_name As String, col_name As String) As Boolean
        Dim result As Boolean = False
        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & "; Version=3;")
        MYCONNECTION.Open()
        Dim CMD As New SQLiteCommand
        CMD.CommandText = "SELECT * FROM " & table_name & " LIMIT 1"
        CMD.Connection = MYCONNECTION
        Dim RDR As SQLiteDataReader = CMD.ExecuteReader
        Dim DT As New DataTable
        DT.Load(RDR)
        RDR.Close()
        For i = 0 To DT.Columns.Count - 1
            If DT.Columns(i).ColumnName.ToString = col_name Then
                result = True
                Exit For
            End If
        Next
        WriteLog_Full("CHECK COLUMN " & col_name & " EXISTS" & " - Table name: " & table_name & vbLf & " - Link database: " & link_database & vbLf & " - Result: " & result)
        Return result
    End Function

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
    Sub AUTO_COMPLETE_GET_DATA(ByVal dataCollection As AutoCompleteStringCollection, link_database As String, SQL_String As String)
        Try
            Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & "; Version=3;")
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
        Catch ex As Exception
            MessageBox.Show("Can not open connection ! ")
        End Try
    End Sub

    Public Sub EXPORT_DATAGRIDVIEW_TO_EXCEL(GRIDVIEW As GridView)
        Dim full_path As String = appPath & Now.ToString("ddMMyyyyhhmmssttttt") & ".xls"
        GRIDVIEW.ExportToXls(full_path)

        Dim ObjExcel As New Excel.Application
        ObjExcel.Visible = False
        Dim Objwb As Excel.Workbook = ObjExcel.Workbooks.Open(full_path)
        Dim Objws As Excel.Worksheet = Objwb.Sheets("Sheet")

        Dim Objwb_dest As Excel.Workbook = ObjExcel.Workbooks.Add()
        Dim Objws_dest As Excel.Worksheet = Objwb_dest.ActiveSheet()
        Objws_dest.Cells.NumberFormat = "@"

        Objws.Cells.Copy()
        Objws_dest.Range("A1").PasteSpecial(Excel.XlPasteType.xlPasteAll)
        ObjExcel.CutCopyMode = False

        Objws_dest.Cells.Borders.LineStyle = Excel.XlLineStyle.xlLineStyleNone

        Dim range_Status As Excel.Range = Objws_dest.Range(Objws_dest.Cells(1, 1), Objws_dest.Cells(1, 1000)).Find("Status")

        If Not IsNothing(range_Status) Then
            With Objws_dest.Range(Objws_dest.Cells(2, range_Status.Column), Objws_dest.Cells(60000, range_Status.Column)).Validation
                .Delete()
                .Add(Type:=Excel.XlDVType.xlValidateList, AlertStyle:=Excel.XlDVAlertStyle.xlValidAlertStop, Operator:=Excel.XlFormatConditionOperator.xlBetween, Formula1:="In Progress,Expired")
                .IgnoreBlank = True
                .InCellDropdown = True
                .InputTitle = ""
                .ErrorTitle = ""
                .InputMessage = ""
                .ErrorMessage = "Only value 'In progress' and 'Expired'"
                .ShowInput = True
                .ShowError = True
            End With
        End If

        Dim range_CaseID As Excel.Range = Objws_dest.Range(Objws_dest.Cells(1, 1), Objws_dest.Cells(1, 1000)).Find("Case ID")
        If Not range_CaseID Is Nothing Then
            range_CaseID.Replace("Case ID", "CaseID")
        End If

        Objwb.Close(SaveChanges:=False)
        Kill(full_path)
        ObjExcel.Visible = True
    End Sub
    Public Sub EnableDoubleBuffered(ByVal dgv As DataGridView)
        Dim dgvType As Type = dgv.[GetType]()
        Dim pi As PropertyInfo = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance Or BindingFlags.NonPublic)
        pi.SetValue(dgv, True, Nothing)
    End Sub

    Public Function base64Encode(ByVal sData As String) As String
        Try
            Dim encData_Byte As Byte() = New Byte(sData.Length - 1) {}
            encData_Byte = System.Text.Encoding.UTF8.GetBytes(sData)
            Dim encodedData As String = Convert.ToBase64String(encData_Byte)
            Return (encodedData)
        Catch ex As Exception
            Throw (New Exception("Error is base64Encode" & ex.Message))
        End Try
    End Function

    Public Function base64Decode(ByVal sData As String) As String
        Dim encoder As New System.Text.UTF8Encoding()
        Dim utf8Decode As System.Text.Decoder = encoder.GetDecoder()
        Dim todecode_byte As Byte() = Convert.FromBase64String(sData)
        Dim charCount As Integer = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length)
        Dim decoded_char As Char() = New Char(charCount - 1) {}
        utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0)
        Dim result As String = New [String](decoded_char)
        Return result
    End Function

    Public Function SELECT_EXCEL_FILE_RETURNED_FULL_PATH(str_title As String) As String
        Dim fd As OpenFileDialog = New OpenFileDialog()
        fd.Title = str_title
        fd.InitialDirectory = "C:\"
        fd.Filter = "Excel 2003(*.xls)|*.xls"
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

    Public Sub IMPORT_INTO_DATABASE_FROM_EXCEL(link_database As String, table_name As String)
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
        WriteLog_Full("[BULK INSERT] - IMPORT INTO DATABASE FROM EXCEL FILE" & " - Table name: " & table_name & vbLf & " - Link database: " & link_database & vbLf & " - Source file: " & fname)
    End Sub

    Public Sub SQLITE_BULK_COPY(DataTable As DataTable, link_database As String, table_name As String)
        Dim datareader As New DataTableReader(DataTable)

        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database & ";version=3;new=False;datetimeformat=CurrentCulture;")

        Dim BulkCopy As SqliteBulkCopy = New SqliteBulkCopy(MYCONNECTION)

        BulkCopy.DestinationTableName = table_name

        BulkCopy.ColumnMappings.Clear()

        For i As Integer = 0 To DataTable.Columns.Count - 1
            BulkCopy.ColumnMappings.Add(DataTable.Columns(i).ColumnName.ToString(), 0)
        Next

        BulkCopy.WriteToServer(datareader)
    End Sub
End Module
