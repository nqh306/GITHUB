Imports System.Data.SQLite
Imports System.IO
Public Class FormSQL
    Public appPath As String = AppDomain.CurrentDomain.BaseDirectory
    Public link_database_SQL As String = appPath & "Database_SQL.txt"
    Private Sub FormSQL_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'add value for combobox FormatFile
        ComboBox_FormatFile.Items.Add(".xls")
        ComboBox_FormatFile.Items.Add(".xlsx")
        ComboBox_FormatFile.Items.Add(".csv")
        ComboBox_FormatFile.Items.Add("SQLite Database (.db; .txt)")
        ComboBox_FormatFile.Items.Add("Access Database")
        ComboBox_FormatFile.Text = "SQLite Database (.db; .txt)"

        rbt_Single_database.Checked = False
        rbt_Multiple_Database.Checked = False
        LayoutControlItem_btAddConnection.Enabled = False
        LayoutControlItem_btDeleteConnection.Enabled = False
    End Sub
    Private Sub btAddConnection_MouseClick(sender As Object, e As MouseEventArgs) Handles btAddConnection.MouseClick
        On Error GoTo err_handle

        Dim fd As OpenFileDialog = New OpenFileDialog()
        fd.Title = "Open File Dialog"
        fd.RestoreDirectory = True

        Select Case ComboBox_FormatFile.Text
            Case ".xls"
                fd.Filter = "Database File - Excel|*.xls"
            Case ".xlsx"
                fd.Filter = "Database File - Excel|*.xlsx"
            Case ".csv"
                fd.Filter = "Comma-separated values|*.csv"
            Case "SQLite Database (.db; .txt)"
                fd.Filter = "SQLite Database|*.db;*.txt"
            Case "Access Database"
                fd.Filter = "Access Database|*.accbd"
        End Select

        fd.FilterIndex = 2
        fd.RestoreDirectory = True

        If fd.ShowDialog() = DialogResult.OK Then
            Dim full_path As String = fd.FileName
            Dim folder_path As String = System.IO.Path.GetDirectoryName(fd.FileName)
            Dim file_name As String = System.IO.Path.GetFileName(fd.FileName)

            If rbt_Single_database.Checked = True Then
                ListView_Connection.Items.Clear()
                tbNote.Text = ""
                ListView_Connection.Items.Add(ComboBox_FormatFile.Text)
                ListView_Connection.Items(0).SubItems.Add(folder_path)
                ListView_Connection.Items(0).SubItems.Add(file_name)


                Dim DT_GET_COLUMN As DataTable
                Dim Column_name As String = ""

                If ComboBox_FormatFile.Text = ".csv" Then
                    DT_GET_COLUMN = SQL_QUERY_CSV_TO_DATATABLE(folder_path, file_name, "SELECT TOP 1 * FROM " & file_name)

                    For i = 0 To DT_GET_COLUMN.Columns.Count - 1
                        If Column_name.Length = 0 Then
                            Column_name = "[" & DT_GET_COLUMN.Columns(i).ColumnName.ToString() & "]"
                        Else
                            Column_name = Column_name & ", [" & DT_GET_COLUMN.Columns(i).ColumnName.ToString() & "]"
                        End If
                    Next
                    tbNote.Text = tbNote.Text & file_name & Chr(13) & Chr(10) & Column_name & Chr(13) & Chr(10) & "__________________________" & Chr(13) & Chr(10)
                End If
                If ComboBox_FormatFile.Text = "SQLite Database (.db; .txt)" Then
                    Dim DT_list_table_in_database As DataTable = GET_ALL_TABLE_NAME_IN_DATABASE(fd.FileName)
                    For Each Drr As DataRow In DT_list_table_in_database.Rows
                        DT_GET_COLUMN = SQL_QUERY_TO_DATATABLE(full_path, "SELECT * FROM " & Drr(0).ToString & " LIMIT 1")
                        For i = 0 To DT_GET_COLUMN.Columns.Count - 1
                            If Column_name.Length = 0 Then
                                Column_name = "[" & DT_GET_COLUMN.Columns(i).ColumnName.ToString() & "]"
                            Else
                                Column_name = Column_name & ", [" & DT_GET_COLUMN.Columns(i).ColumnName.ToString() & "]"
                            End If
                        Next
                        tbNote.Text = tbNote.Text & Drr(0).ToString & Chr(13) & Chr(10) & Column_name & Chr(13) & Chr(10) & "__________________________" & Chr(13) & Chr(10)
                        Column_name = ""
                    Next
                End If
            End If
            If rbt_Multiple_Database.Checked = True Then
                AddConnection_MultipleDatabase(fd)
            End If
        End If
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub
    Private Sub AddConnection_MultipleDatabase(fd As OpenFileDialog)
        'check database exist
        If Not My.Computer.FileSystem.FileExists(link_database_SQL) Then
            SQLiteConnection.CreateFile(link_database_SQL)
        End If
        If fd.FileName.Length > 0 Then
            Dim table_name As String = ""
            Dim file_name As String = System.IO.Path.GetFileNameWithoutExtension(fd.FileName)
            Dim Column_Name_For_Create As String = ""
            Dim DT_full_data_for_import As DataTable

            If ComboBox_FormatFile.Text = "SQLite Database (.db; .txt)" Then
                Dim DT_list_table_in_database As DataTable = GET_ALL_TABLE_NAME_IN_DATABASE(fd.FileName)
                For Each Drr As DataRow In DT_list_table_in_database.Rows
                    table_name = file_name & "_" & Drr(0).ToString
                    If CHECK_TABLE_EXIST_OR_NOT(link_database_SQL, table_name) = True Then
                        MsgBox("Table name " & table_name & " exist", vbCritical, "Vendor Management")
                        Exit Sub
                    End If
                    DT_full_data_for_import = SQL_QUERY_TO_DATATABLE(fd.FileName, "SELECT * FROM " & Drr(0).ToString)

                    For i = 0 To DT_full_data_for_import.Columns.Count - 1
                        If Column_Name_For_Create.Length = 0 Then
                            Column_Name_For_Create = "[" & DT_full_data_for_import.Columns(i).ColumnName.ToString() & "] VARCHAR"
                        Else
                            Column_Name_For_Create = Column_Name_For_Create & ", [" & DT_full_data_for_import.Columns(i).ColumnName.ToString() & "] VARCHAR"
                        End If
                    Next

                    SQL_QUERY(link_database_SQL, "CREATE TABLE IF NOT EXISTS " & table_name & " (" & Column_Name_For_Create & ")")
                    SQLITE_BULK_COPY(DT_full_data_for_import, link_database_SQL, table_name)

                Next
            End If
            If ComboBox_FormatFile.Text = ".csv" Then
                table_name = file_name
                If CHECK_TABLE_EXIST_OR_NOT(link_database_SQL, table_name) = True Then
                    MsgBox("Table name " & table_name & " exist", vbCritical, "Vendor Management")
                    Exit Sub
                End If
                DT_full_data_for_import = SQL_QUERY_CSV_TO_DATATABLE(System.IO.Path.GetDirectoryName(fd.FileName), System.IO.Path.GetFileName(fd.FileName), "SELECT * FROM " & System.IO.Path.GetFileName(fd.FileName))

                For i = 0 To DT_full_data_for_import.Columns.Count - 1
                    If Column_Name_For_Create.Length = 0 Then
                        Column_Name_For_Create = "[" & DT_full_data_for_import.Columns(i).ColumnName.ToString() & "] VARCHAR"
                    Else
                        Column_Name_For_Create = Column_Name_For_Create & ", [" & DT_full_data_for_import.Columns(i).ColumnName.ToString() & "] VARCHAR"
                    End If
                Next

                SQL_QUERY(link_database_SQL, "CREATE TABLE IF NOT EXISTS " & table_name & " (" & Column_Name_For_Create & ")")
                SQLITE_BULK_COPY(DT_full_data_for_import, link_database_SQL, table_name)

            End If
        End If
        Load_Database_Multiple_SQL()
    End Sub
    Public Sub Load_Database_Multiple_SQL()
        On Error GoTo err_handle
        ListView_Connection.Items.Clear()
        tbNote.Text = ""
        Dim DT_tablename_SQL_Database As DataTable = GET_ALL_TABLE_NAME_IN_DATABASE(link_database_SQL)
        For Each Drr As DataRow In DT_tablename_SQL_Database.Rows
            ListView_Connection.Items.Add("")
            ListView_Connection.Items(ListView_Connection.Items.Count - 1).SubItems.Add(Drr(0).ToString)
            tbNote.Text = tbNote.Text & Drr(0).ToString & Chr(13) & Chr(10) & GET_LIST_FIELD_NAME_FROM_DATABASE(link_database_SQL, Drr(0).ToString) & Chr(13) & Chr(10) & "__________________________" & Chr(13) & Chr(10)
        Next
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub rbt_Single_database_MouseClick(sender As Object, e As MouseEventArgs) Handles rbt_Single_database.MouseClick
        LayoutControlItem_btAddConnection.Enabled = True
        LayoutControlItem_btDeleteConnection.Enabled = True
        'setup header for Listview_Connection
        tbNote.Text = ""
        With ListView_Connection
            .Items.Clear()
            .Columns.Clear()
            .GridLines = True
            .View = View.Details
            .CheckBoxes = False
            .Columns.Add("Format File", 100, HorizontalAlignment.Left)
            .Columns.Add("Folder Path", 200, HorizontalAlignment.Left)
            .Columns.Add("File Name", 200, HorizontalAlignment.Left)
        End With

    End Sub

    Private Sub rbt_Multiple_Database_MouseClick(sender As Object, e As MouseEventArgs) Handles rbt_Multiple_Database.MouseClick
        LayoutControlItem_btAddConnection.Enabled = True
        LayoutControlItem_btDeleteConnection.Enabled = True
        'setup header for Listview_Connection
        tbNote.Text = ""
        With ListView_Connection
            .Items.Clear()
            .Columns.Clear()
            .GridLines = True
            .View = View.Details
            .CheckBoxes = True
            .Columns.Add("", 20, HorizontalAlignment.Left)
            .Columns.Add("Table Name", 200, HorizontalAlignment.Left)
        End With
        Load_Database_Multiple_SQL()
    End Sub

    Private Sub btExportToExcel_MouseClick(sender As Object, e As MouseEventArgs) Handles btExportToExcel.MouseClick
        On Error GoTo err_handle
        Module_Letter_Management.EXPORT_GRIDVIEW_TO_EXCEL(GridView1)
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub btExecute_Click(sender As Object, e As EventArgs) Handles btExecute.Click
        On Error GoTo err_handle
        Dim DT As DataTable
        If rbt_Multiple_Database.Checked = True Then
            DT = SQL_QUERY_TO_DATATABLE(link_database_SQL, tbStrSQL.Text)
        End If
        If rbt_Single_database.Checked = True Then
            If ListView_Connection.Items(0).Text = ".csv" Then
                DT = SQL_QUERY_CSV_TO_DATATABLE(ListView_Connection.Items(0).SubItems(1).Text, ListView_Connection.Items(0).SubItems(2).Text, tbStrSQL.Text)
                GridView1.Columns.Clear()
                GridControl1.DataSource = DT
            End If
            If ComboBox_FormatFile.Text = "SQLite Database (.db; .txt)" Then
                Dim full_path As String
                If ListView_Connection.Items(0).SubItems(1).Text.Substring(ListView_Connection.Items(0).SubItems(1).Text.Length - 1, 1) <> "\" Then
                    full_path = ListView_Connection.Items(0).SubItems(1).Text & "\" & ListView_Connection.Items(0).SubItems(2).Text
                Else
                    full_path = ListView_Connection.Items(0).SubItems(1).Text & ListView_Connection.Items(0).SubItems(2).Text
                End If
                DT = SQL_QUERY_TO_DATATABLE(full_path, tbStrSQL.Text)
                GridView1.Columns.Clear()
                GridControl1.DataSource = DT
                GridView1.BestFitColumns()
            End If
        End If

        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub btDeleteConnection_Click(sender As Object, e As EventArgs) Handles btDeleteConnection.Click
        If rbt_Single_database.Checked = True Then
            ListView_Connection.Items.Clear()
            tbNote.Text = ""
        End If
        If rbt_Multiple_Database.Checked = True Then
            For i = 0 To ListView_Connection.Items.Count - 1

                If ListView_Connection.Items(i).Checked = True Then
                    SQL_QUERY(link_database_SQL, "DROP TABLE " & ListView_Connection.Items(i).SubItems(1).Text)
                    Load_Database_Multiple_SQL()
                End If
            Next
        End If
    End Sub
End Class