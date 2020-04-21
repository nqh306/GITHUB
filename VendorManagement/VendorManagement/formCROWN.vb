Imports DevExpress.XtraEditors.Repository
Imports System.Data.OleDb
Imports Microsoft.Data.Sqlite.Extensions
Imports System.Data.SQLite
Imports DevExpress.XtraBars

Public Class formCROWN
    Public appPath As String = AppDomain.CurrentDomain.BaseDirectory
    Public link_app_config As String = appPath & "Application_Config.db"
    Public link_application_config As String = Module_Letter_Management.SQL_QUERY_TO_STRING(link_app_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'Link_Application_Config'")

    Public Link_folder_database As String = ""
    Public link_database_CROWN As String = ""
    Public table_name_CROWN As String = ""
    Private Sub formCROWN_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Add value database
        BarEditItem_RepositoryItemComboBox_DatabaseName.EditWidth = 200
        Dim DT As DataTable = Module_Letter_Management.SQL_QUERY_TO_DATATABLE(link_application_config, "SELECT DISTINCT Database_Name FROM LIST_DATABASE_CROWN")
        If DT.Rows.Count > 0 Then
            For i As Integer = 0 To DT.Rows.Count - 1
                CType(BarEditItem_RepositoryItemComboBox_DatabaseName.Edit, RepositoryItemComboBox).Items.Add(DT.Rows(i).Item(0).ToString)
            Next
            BarEditItem_RepositoryItemComboBox_DatabaseName.EditValue = DT.Rows(0).Item(0).ToString
        End If
        refresh_link_CROWN()
        tbUserCreated.ReadOnly = True
        tbUserModified.ReadOnly = True
    End Sub

    Public Sub refresh_link_CROWN()
        'Get link_folder_database
        Link_folder_database = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Folder_Path FROM LIST_DATABASE_CROWN WHERE Database_Name = '" & BarEditItem_RepositoryItemComboBox_DatabaseName.EditValue.ToString & "'")
        If Link_folder_database.Substring(Link_folder_database.Length - 1, 1) <> "\" Then
            Link_folder_database = Link_folder_database & "\"
        End If
        link_database_CROWN = Link_folder_database & "Database_CROWN.txt"
        table_name_CROWN = BarEditItem_RepositoryItemComboBox_DatabaseName.EditValue

        'Create database file if not exist
        Module_Letter_Management.SQLITE_CREATE_DATABASE_FILE(link_database_CROWN)
        SQL_QUERY(link_database_CROWN, "CREATE TABLE IF NOT EXISTS " & table_name_CROWN & " (Case_ID VARCHAR,Client_name VARCHAR,Master_No VARCHAR,eOps_Ref_No VARCHAR,Document_Type VARCHAR,Document_Description VARCHAR,Filing_Date VARCHAR,Sequence_No VARCHAR,Box_Barcode VARCHAR,CROWN_Pickup_Date VARCHAR,Status VARCHAR,User_Created VARCHAR,User_Modified VARCHAR)")

    End Sub
    Private Sub BarButtonItem_ChooseDatabase_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarButtonItem_ChooseDatabase.ItemClick
        On Error GoTo err_handle
        Dim iRet = MsgBox("Do you want to open database " & BarEditItem_RepositoryItemComboBox_DatabaseName.EditValue.ToString & "?", vbYesNo)
        If iRet = vbYes Then
            refresh_link_CROWN()
            Main.Statusbar_item1.Caption = ""
            Main.Statusbar_item2.Caption = "Load database: '" & link_database_CROWN
        End If
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub
    Private Sub btRefresh_Click(sender As Object, e As EventArgs) Handles btRefresh.Click

        Dim TimerStart As DateTime
        TimerStart = Now

        tbBarcode.Text = ""
        tbCaseID.Text = ""
        tbCrownPickupDate.Text = ""
        tbDescription.Text = ""
        tbeOpsRefNo.Text = ""
        tbFilingDate.Text = ""
        tbMasterNo.Text = ""
        tbName.Text = ""
        tbSequenceNo.Text = ""
        tbStrSearch.Text = ""
        tbUserCreated.Text = ""
        tbUserModified.Text = ""
        ComboBox_DocType.Text = ""
        ComboBox_Status.Text = ""
        Dim DT As New DataTable
        DT = Module_Letter_Management.SQL_QUERY_TO_DATATABLE(link_database_CROWN, "Select * FROM " & table_name_CROWN)
        GridControl1.DataSource = DT

        Dim TimeSpent As System.TimeSpan
        TimeSpent = Now.Subtract(TimerStart)
        Main.Statusbar_item2.Caption = "Load database in " & Format(TimeSpent.TotalSeconds, "0.00") & " seconds"
    End Sub

    Private Sub DataGridView_CROWN_CellMouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs)
        Try
            tbCaseID.Text = GridView1.GetFocusedRowCellDisplayText("Case_ID").ToString
            tbName.Text = GridView1.GetFocusedRowCellDisplayText("Client_name").ToString
            tbMasterNo.Text = GridView1.GetFocusedRowCellDisplayText("Master_No").ToString
            tbeOpsRefNo.Text = GridView1.GetFocusedRowCellDisplayText("eOps_Ref_No").ToString
            ComboBox_DocType.Text = GridView1.GetFocusedRowCellDisplayText("Document_Type").ToString
            tbDescription.Text = GridView1.GetFocusedRowCellDisplayText("Document_Description").ToString
            tbFilingDate.Text = GridView1.GetFocusedRowCellDisplayText("Filing_Date").ToString
            tbSequenceNo.Text = GridView1.GetFocusedRowCellDisplayText("Sequence_No").ToString
            tbBarcode.Text = GridView1.GetFocusedRowCellDisplayText("Box_Barcode").ToString
            tbCrownPickupDate.Text = GridView1.GetFocusedRowCellDisplayText("CROWN_Pickup_Date").ToString
            ComboBox_Status.Text = GridView1.GetFocusedRowCellDisplayText("Status").ToString
            tbUserCreated.Text = GridView1.GetFocusedRowCellDisplayText("User_Created").ToString
            tbUserModified.Text = GridView1.GetFocusedRowCellDisplayText("User_Modified").ToString
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btAdd_Click(sender As Object, e As EventArgs) Handles btAdd.Click
        On Error GoTo err_handle
        'Check du thong tin de tao Bill hay chua
        If tbName.Text.Length = 0 Then
            MsgBox("no DATA")
            Exit Sub
        End If
        Dim iRet = MsgBox("Do you want To add New record?", vbYesNo, "Letter Management")
        If iRet = vbYes Then
            Dim Case_ID As String = Now.ToString("ddMMyyyyhhmmssffff") & "_" & tbName.Text
            If tbMasterNo.Text.Length > 0 Then
                Case_ID = Case_ID & "_" & tbMasterNo.Text
            End If
            If tbSequenceNo.Text.Length > 0 Then
                Case_ID = Case_ID & "_" & tbSequenceNo.Text
            End If
            If Module_Letter_Management.SQL_QUERY_TO_INTEGER(link_database_CROWN, "Select COUNT([Case_ID]) FROM " & table_name_CROWN & " WHERE [Case_ID]='" & Case_ID & "'") > 0 Then
                MsgBox("Can't create new record. Case ID " & Case_ID & " existed", vbCritical, "Vendor Management - Error")
            End If
            Dim Str_SQL_Add_Record As String = "INSERT INTO " & table_name_CROWN & " ([Case_ID], [Client_name], [Master_No], [eOps_Ref_No], [Document_Type], [Document_Description], [Filing_Date], [Sequence_No], [Box_Barcode], [CROWN_Pickup_Date], [Status], [User_Created]) VALUES ('" & UCase(Case_ID) & "', '" & UCase(tbName.Text) & "', '" & tbMasterNo.Text & "', '" & UCase(tbeOpsRefNo.Text) & "', '" & UCase(ComboBox_DocType.Text) & "', '" & UCase(tbDescription.Text) & "', '" & UCase(tbFilingDate.Text) & "', '" & UCase(tbSequenceNo.Text) & "', '" & UCase(tbBarcode.Text) & "', '" & UCase(tbCrownPickupDate.Text) & "', '" & UCase(ComboBox_Status.Text) & "', '" & UCase(tbUserCreated.Text) & "')"
            Module_Letter_Management.SQL_QUERY(link_database_CROWN, Str_SQL_Add_Record)
        End If
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub btUpdate_Click(sender As Object, e As EventArgs) Handles btUpdate.Click
        On Error GoTo err_handle
        If tbCaseID.Text.Length = 0 Then Exit Sub
        Dim iRet = MsgBox("Do you want to update information for case " & tbCaseID.Text & "?", vbYesNo, "Letter Management")
        If iRet = vbYes Then
            Dim str_update As String = ""
            str_update = "UPDATE " & table_name_CROWN &
                         " SET [Client_name] = '" & tbName.Text & "'," &
                            " [Master_No] = '" & tbMasterNo.Text & "'," &
                            " [eOps_Ref_No] = '" & tbeOpsRefNo.Text & "'," &
                            " [Document_Type] = '" & ComboBox_DocType.Text & "'," &
                            " [Document_Description] = '" & tbDescription.Text & "'," &
                            " [Filing_Date] = '" & tbFilingDate.Text & "'," &
                            " [Sequence_No] = '" & tbSequenceNo.Text & "'," &
                            " [Box_Barcode] = '" & tbBarcode.Text & "'," &
                            " [CROWN_Pickup_Date] = '" & tbCrownPickupDate.Text & "'," &
                            " [Status] = '" & ComboBox_Status.Text & "'," &
                            " [User_Modified] = '" & Environment.UserName & "_" & Now.ToString("ddMMyyyyhhmmss") & "'" &
                         " WHERE [Case_ID] = '" & tbCaseID.Text & "'"
            Module_Letter_Management.SQL_QUERY(link_database_CROWN, str_update)
        End If
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub btDelete_Click(sender As Object, e As EventArgs) Handles btDelete.Click
        On Error GoTo err_handle
        If tbCaseID.Text.Length = 0 Then Exit Sub
        Dim iRet = MsgBox("Do you want to delete case " & tbCaseID.Text & "?", vbYesNo, "Vendor Management")
        If iRet = vbYes Then
            Dim SQL_string As String = "DELETE FROM " & table_name_CROWN & " WHERE [Case_ID]='" & tbCaseID.Text & "'"
            Module_Letter_Management.SQL_QUERY(link_database_CROWN, SQL_string)
        End If
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub btExportToExcel_Click(sender As Object, e As EventArgs) Handles btExportToExcel.Click
        On Error GoTo err_handle
        EXPORT_GRIDVIEW_TO_EXCEL(GridView1)
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub btImportFromExcel_Click(sender As Object, e As EventArgs) Handles btImportFromExcel.Click
        On Error GoTo err_handle
        Dim fname As String = ""
        fname = SELECT_EXCEL_FILE_RETURNED_FULL_PATH("Please select excel file (.xls)")

        Dim conn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & fname & ";Extended Properties='Excel 8.0;HDR=Yes'")

        conn.Open()
        Dim user_create As String = Environ("username") & "_" & Now.ToString("dd/MM/yyyy hh:mm:ss tt")
        Dim sql = "SELECT FORMAT(now,'ddMMyyyyhhmmss')&'_'&IIF(LEN(Client_name)>0,Client_name,'')&IIF(LEN(Master_No)>0,'_'&Master_No,'')&IIF(LEN(Sequence_No)>0,'_'&Sequence_No,'')&'_'&[No] as [Case_ID], '" & user_create & "' as [User_Created], Client_name, Master_No, eOps_Ref_No, Document_Type, Document_Description, Filing_Date, Sequence_No, Box_Barcode, CROWN_Pickup_Date, Status FROM [Sheet1$]"

        Dim cmdDataGrid As OleDbCommand = New OleDbCommand(sql, conn)
        Dim da As New OleDbDataAdapter
        da.SelectCommand = cmdDataGrid
        Dim dt As New DataTable
        da.Fill(dt)

        Dim datareader As New DataTableReader(dt)

        conn.Close()

        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database_CROWN & "; Version=3;")

        Dim BulkCopy As SqliteBulkCopy = New SqliteBulkCopy(MYCONNECTION)

        BulkCopy.DestinationTableName = table_name_CROWN

        BulkCopy.ColumnMappings.Clear()

        For i As Integer = 0 To dt.Columns.Count - 1
            BulkCopy.ColumnMappings.Add(dt.Columns(i).ColumnName.ToString(), 0)
        Next

        BulkCopy.WriteToServer(datareader)

        Main.Statusbar_item2.Caption = "Import to database from source: " & fname & ": Completed"
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub tbStrSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles tbStrSearch.KeyDown
        Dim TimerStart As DateTime
        TimerStart = Now

        If e.KeyCode = Keys.Return Then
            If link_database_CROWN.Length = 0 Then Exit Sub
            If tbStrSearch.Text.Length = 0 Then Exit Sub

            On Error GoTo err_handle
            Dim SQL_Str_Search As String = ""

            Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database_CROWN & "; Version=3;")
            MYCONNECTION.Open()
            Dim CMD2 As New SQLiteCommand
            CMD2.CommandText = "SELECT * FROM " & table_name_CROWN & " LIMIT 1"
            CMD2.Connection = MYCONNECTION
            Dim RDR2 As SQLiteDataReader = CMD2.ExecuteReader
            Dim DS2 As New DataTable
            DS2.Load(RDR2)
            RDR2.Close()
            For i = 0 To DS2.Columns.Count - 1
                If SQL_Str_Search.Length = 0 Then
                    SQL_Str_Search = "[" & DS2.Columns(i).ColumnName.ToString() & "] LIKE '%" & tbStrSearch.Text & "%' "
                Else
                    SQL_Str_Search = SQL_Str_Search & " OR [" & DS2.Columns(i).ColumnName.ToString() & "] LIKE '%" & tbStrSearch.Text & "%' "
                End If
            Next
            GridControl1.DataSource = Module_Letter_Management.SQL_QUERY_TO_DATATABLE(link_database_CROWN, "SELECT * FROM " & table_name_CROWN & " WHERE " & SQL_Str_Search)
        End If

        Dim TimeSpent As System.TimeSpan
        TimeSpent = Now.Subtract(TimerStart)
        Main.Statusbar_item2.Caption = "Load database in " & Format(TimeSpent.TotalSeconds, "0.00") & " seconds"
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()

    End Sub


End Class