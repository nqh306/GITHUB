Imports DevExpress.XtraBars
Imports System.Data.OleDb
Imports System.Data.SQLite
Imports DevExpress.XtraEditors.Repository
Public Class FormSpecialClient
    Public appPath As String = AppDomain.CurrentDomain.BaseDirectory
    Public link_app_config As String = appPath & "Application_Config.db"
    Public link_application_config As String = Module_Letter_Management.SQL_QUERY_TO_STRING(link_app_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'Link_Application_Config'")
    Public link_database_special_List As String
    Public table_name_Special_List As String
    Private Sub FormSpecialClient_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        On Error GoTo err_handle

        EnableDoubleBuffered(DataGridView_Special_List)

        LayoutControl_LabelCaseID.Enabled = False
        LayoutControlItem_tbUserCreated.Enabled = False
        LayoutControlItem_tbUserModified.Enabled = False
        LabelCaseID.Text = "Case ID: "

        tbAttention.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        tbClientName.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        tbMailingAddress.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        tbMasterNo.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        tbRemark.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        tbStrSearch.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        tbUserCreated.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        tbUserModified.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center

        'add value link database Special List
        BarEdit_RepositoryItemComboBox_Link_Database.EditWidth = 300
        CType(BarEdit_RepositoryItemComboBox_Link_Database.Edit, RepositoryItemComboBox).Items.Add(Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'Link_database_Special_List'"))
        BarEdit_RepositoryItemComboBox_Link_Database.EditValue = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'Link_database_Special_List'")

        'add value table name of Special List
        BarEdit_RepositoryItemComboBox_Table_Name.EditWidth = 150
        Dim dt As DataTable = Module_Letter_Management.GET_ALL_TABLE_NAME_IN_DATABASE(BarEdit_RepositoryItemComboBox_Link_Database.EditValue.ToString)
        If dt.Rows.Count > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                CType(BarEdit_RepositoryItemComboBox_Table_Name.Edit, RepositoryItemComboBox).Items.Add(dt.Rows(i).Item(0).ToString)
            Next
            BarEdit_RepositoryItemComboBox_Table_Name.EditValue = dt.Rows(0).Item(0).ToString
        End If

        btLoadDatabase.PerformClick()
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub
    Private Sub btLoadDatabase_ItemClick(sender As Object, e As ItemClickEventArgs) Handles btLoadDatabase.ItemClick
        On Error GoTo err_handle
        link_database_special_List = BarEdit_RepositoryItemComboBox_Link_Database.EditValue
        table_name_Special_List = BarEdit_RepositoryItemComboBox_Table_Name.EditValue

        DataGridView_Special_List.DataSource = Module_Letter_Management.SQL_QUERY_TO_DATATABLE(link_database_special_List, "SELECT * FROM " & table_name_Special_List)

        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub btClear_Click(sender As Object, e As EventArgs) Handles btClear.Click
        On Error GoTo err_handle
        LabelCaseID.Text = "Case ID: "
        tbAttention.Text = ""
        tbClientName.Text = ""
        tbMailingAddress.Text = ""
        tbMasterNo.Text = ""
        tbRemark.Text = ""
        tbStrSearch.Text = ""
        tbUserCreated.Text = ""
        tbUserModified.Text = ""
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub btAdd_Click(sender As Object, e As EventArgs) Handles btAdd.Click
        On Error GoTo err_handle
        If tbClientName.Text.Length = 0 Or tbMailingAddress.Text.Length = 0 Then Exit Sub

        Dim iRet = MsgBox("Do you want to add new record?", vbYesNo, "Vendor Management")
        If iRet = vbYes Then
            Dim Str_SQL_Add_Record As String = "INSERT INTO " & table_name_Special_List & " ([Master_No], [Client_Name], [Client_address], [Client_Attention], [Remark], [User_Created], [Case_ID]) VALUES ('" & UCase(tbMasterNo.Text) & "', '" & UCase(tbClientName.Text) & "', '" & UCase(tbMailingAddress.Text) & "', '" & UCase(tbAttention.Text) & "', '" & UCase(tbRemark.Text) & "', '" & UCase(Environment.UserName) & "_" & Now.ToString("dd/MM/yyyy hh:mm:ss") & "', '" & tbMasterNo.Text & "_" & Now.ToString("ddMMyyyyhhmmss") & "');"
            Module_Letter_Management.SQL_QUERY(link_database_special_List, Str_SQL_Add_Record)
            DataGridView_Special_List.DataSource = Module_Letter_Management.SQL_QUERY_TO_DATATABLE(link_database_special_List, "SELECT * FROM " & table_name_Special_List)
        End If
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub btUpdate_Click(sender As Object, e As EventArgs) Handles btUpdate.Click
        On Error GoTo err_handle
        If LabelCaseID.Text.Length = 0 Then Exit Sub

        Dim iRet = MsgBox("Do you want to update information for master " & tbMasterNo.Text & "?", vbYesNo, "Vendor Management")
        If iRet = vbYes Then
            Dim str_update As String = "UPDATE " & table_name_Special_List &
                                    " SET [Client_Name] = '" & tbClientName.Text & "'," &
                                    " [Client_address] = '" & tbMailingAddress.Text & "'," &
                                    " [Client_attention] = '" & tbAttention.Text & "'," &
                                    " [Remark] = '" & tbRemark.Text & "'," &
                                    " [User_Modified] = '" & UCase(Environment.UserName) & "_" & Now.ToString("dd/MM/yyyy hh:mm:ss") & "'" &
                                 " WHERE [Case_ID] = '" & LabelCaseID.Text & "'"
            Module_Letter_Management.SQL_QUERY(link_database_special_List, str_update)
            DataGridView_Special_List.DataSource = Module_Letter_Management.SQL_QUERY_TO_DATATABLE(link_database_special_List, "SELECT * FROM " & table_name_Special_List)
        End If
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub btDelete_Click(sender As Object, e As EventArgs) Handles btDelete.Click
        On Error GoTo err_handle
        If LabelCaseID.Text.Length = 0 Then Exit Sub

        Dim iRet = MsgBox("Do you want to delete this record: " & LabelCaseID.Text & "?", vbYesNo, "Vendor Management")
        If iRet = vbYes Then
            Dim sql_string As String = "DELETE FROM " & table_name_Special_List & " WHERE [Case_ID] = '" & LabelCaseID.Text & "'"
            Module_Letter_Management.SQL_QUERY(link_database_special_List, sql_string)
            DataGridView_Special_List.DataSource = Module_Letter_Management.SQL_QUERY_TO_DATATABLE(link_database_special_List, "SELECT * FROM " & table_name_Special_List)
        End If
        btClear.PerformClick()
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub btExportToExcel_Click(sender As Object, e As EventArgs) Handles btExportToExcel.Click
        On Error GoTo err_handle
        Module_Letter_Management.EXPORT_DATAGRIDVIEW_TO_EXCEL(DataGridView_Special_List)
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub btImportFromExcel_Click(sender As Object, e As EventArgs) Handles btImportFromExcel.Click
        On Error GoTo err_handle
        Dim fname As String = ""
        fname = SELECT_EXCEL_FILE_RETURNED_FULL_PATH("Select excel file (.xls)")

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
            Dim Str_SQL_Add_Record As String = "INSERT INTO " & table_name_Special_List & " (" & GET_LIST_FIELD_NAME_FROM_DATABASE(link_database_special_List, table_name_Special_List) & ") VALUES ('" & Drr(0).ToString & "', '" & Drr(1).ToString & "', '" & Drr(2).ToString & "', '" & Drr(3).ToString & "', '" & Drr(4).ToString & "', '" & Drr(5).ToString & "', '" & Drr(6).ToString & "', '" & Drr(7).ToString & "');"
            SQL_QUERY(link_database_special_List, Str_SQL_Add_Record)
            i = i + 1
        Next
        conn.Close()
        btClear.PerformClick()
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub tbStrSearch_EditValueChanged(sender As Object, e As EventArgs) Handles tbStrSearch.EditValueChanged
        On Error GoTo err_handle
        Dim SQL_Str_Search As String = ""

        Dim MYCONNECTION As New SQLiteConnection("DataSource=" & link_database_special_List & "; Version=3;")
        MYCONNECTION.Open()
        Dim CMD2 As New SQLiteCommand
        CMD2.CommandText = "SELECT * FROM " & table_name_Special_List & " LIMIT 1"
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
        DataGridView_Special_List.DataSource = Module_Letter_Management.SQL_QUERY_TO_DATATABLE(link_database_special_List, "SELECT * FROM " & table_name_Special_List & " WHERE " & SQL_Str_Search)
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub DataGridView_Special_List_CellMouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView_Special_List.CellMouseDoubleClick
        On Error GoTo err_handle
        If Len(DataGridView_Special_List.Rows(e.RowIndex).Cells(0).Value.ToString()) > 0 Then
            tbMasterNo.Text = DataGridView_Special_List.Rows(e.RowIndex).Cells(0).Value.ToString()
        Else
            tbMasterNo.Text = ""
        End If
        If Len(DataGridView_Special_List.Rows(e.RowIndex).Cells(1).Value.ToString()) > 0 Then
            tbClientName.Text = DataGridView_Special_List.Rows(e.RowIndex).Cells(1).Value.ToString()
        Else
            tbClientName.Text = ""
        End If
        If Len(DataGridView_Special_List.Rows(e.RowIndex).Cells(2).Value.ToString()) > 0 Then
            tbMailingAddress.Text = DataGridView_Special_List.Rows(e.RowIndex).Cells(2).Value.ToString()
        Else
            tbMailingAddress.Text = ""
        End If
        If Len(DataGridView_Special_List.Rows(e.RowIndex).Cells(3).Value.ToString()) > 0 Then
            tbAttention.Text = DataGridView_Special_List.Rows(e.RowIndex).Cells(3).Value.ToString()
        Else
            tbAttention.Text = ""
        End If
        If Len(DataGridView_Special_List.Rows(e.RowIndex).Cells(4).Value.ToString()) > 0 Then
            tbRemark.Text = DataGridView_Special_List.Rows(e.RowIndex).Cells(4).Value.ToString()
        Else
            tbRemark.Text = ""
        End If
        If Len(DataGridView_Special_List.Rows(e.RowIndex).Cells(5).Value.ToString()) > 0 Then
            tbUserModified.Text = DataGridView_Special_List.Rows(e.RowIndex).Cells(5).Value.ToString()
        Else
            tbUserModified.Text = ""
        End If
        If Len(DataGridView_Special_List.Rows(e.RowIndex).Cells(6).Value.ToString()) > 0 Then
            tbUserCreated.Text = DataGridView_Special_List.Rows(e.RowIndex).Cells(6).Value.ToString()
        Else
            tbUserCreated.Text = ""
        End If
        If Len(DataGridView_Special_List.Rows(e.RowIndex).Cells(7).Value.ToString()) > 0 Then
            LabelCaseID.Text = DataGridView_Special_List.Rows(e.RowIndex).Cells(7).Value.ToString()
        Else
            LabelCaseID.Text = ""
        End If
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

End Class