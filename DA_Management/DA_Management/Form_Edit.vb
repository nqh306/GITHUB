Imports System.Data.OleDb
Imports DevExpress.XtraEditors
Public Class Form_Edit
    Public appPath As String = AppDomain.CurrentDomain.BaseDirectory
    Public link_app_config As String = appPath & "App_Config.txt"
    Public folder_path As String
    Public folder_path_save_file As String
    Public link_database_DA As String

    Sub refresh_link()
        folder_path = SQL_QUERY_TO_STRING(link_app_config, "SELECT Field_Value FROM Config WHERE Field_Name = 'Link_Folder_Path_DA'")
        If folder_path.Substring(folder_path.Length - 1, 1) <> "\" Then
            folder_path = folder_path & "\"
        End If
        folder_path_save_file = folder_path & "ARCHIVE"
        link_database_DA = folder_path & "Database_DA.txt"
    End Sub

    Private Sub Add_Item_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        refresh_link()

        lbCaseID.Text = ""
        tbDAFrom_PWID.Select()

        GridView1.OptionsBehavior.Editable = False
        WindowsFormsSettings.AllowAutoFilterConditionChange = DevExpress.Utils.DefaultBoolean.False

        'autocomplete tu dong dien thong tin cho textbox tbDAFrom_Name, tbDATo_Name
        Dim DataCollection_DA_Name As New AutoCompleteStringCollection()
        Module_DA.AUTO_COMPLETE_GET_DATA(DataCollection_DA_Name, link_database_DA, "SELECT DISTINCT DA_From FROM Main_DA")
        Module_DA.AUTO_COMPLETE_GET_DATA(DataCollection_DA_Name, link_database_DA, "SELECT DISTINCT DA_To FROM Main_DA")
        tbDAFrom_Name.AutoCompleteMode = AutoCompleteMode.Suggest
        tbDAFrom_Name.AutoCompleteSource = AutoCompleteSource.CustomSource
        tbDAFrom_Name.AutoCompleteCustomSource = DataCollection_DA_Name

        tbDATo_Name.AutoCompleteMode = AutoCompleteMode.Suggest
        tbDATo_Name.AutoCompleteSource = AutoCompleteSource.CustomSource
        tbDATo_Name.AutoCompleteCustomSource = DataCollection_DA_Name

        'autocomplete tu dong dien thong tin cho textbox tbDAFrom_PWID, tbDATo_PWID
        Dim DataCollection_DA_PWID As New AutoCompleteStringCollection()
        Module_DA.AUTO_COMPLETE_GET_DATA(DataCollection_DA_PWID, link_database_DA, "SELECT DISTINCT PWID_DA_From FROM Main_DA")
        Module_DA.AUTO_COMPLETE_GET_DATA(DataCollection_DA_PWID, link_database_DA, "SELECT DISTINCT PWID_DA_To FROM Main_DA")
        tbDAFrom_PWID.AutoCompleteMode = AutoCompleteMode.Suggest
        tbDAFrom_PWID.AutoCompleteSource = AutoCompleteSource.CustomSource
        tbDAFrom_PWID.AutoCompleteCustomSource = DataCollection_DA_PWID

        tbDATo_PWID.AutoCompleteMode = AutoCompleteMode.Suggest
        tbDATo_PWID.AutoCompleteSource = AutoCompleteSource.CustomSource
        tbDATo_PWID.AutoCompleteCustomSource = DataCollection_DA_PWID

        'autocomplete tu dong dien thong tin cho textbox tbDAFrom_Position, tbDATo_Position
        Dim DataCollection_DA_Position As New AutoCompleteStringCollection()
        Module_DA.AUTO_COMPLETE_GET_DATA(DataCollection_DA_Position, link_database_DA, "SELECT DISTINCT Position_To FROM Main_DA")
        Module_DA.AUTO_COMPLETE_GET_DATA(DataCollection_DA_Position, link_database_DA, "SELECT DISTINCT Position_From FROM Main_DA")
        tbDAFrom_Position.AutoCompleteMode = AutoCompleteMode.Suggest
        tbDAFrom_Position.AutoCompleteSource = AutoCompleteSource.CustomSource
        tbDAFrom_Position.AutoCompleteCustomSource = DataCollection_DA_Position

        tbDATo_Position.AutoCompleteMode = AutoCompleteMode.Suggest
        tbDATo_Position.AutoCompleteSource = AutoCompleteSource.CustomSource
        tbDATo_Position.AutoCompleteCustomSource = DataCollection_DA_Position

        'autocomplete tu dong dien thong tin cho textbox tbDepartment
        Dim DataCollection_DA_Department As New AutoCompleteStringCollection()
        Module_DA.AUTO_COMPLETE_GET_DATA(DataCollection_DA_Department, link_database_DA, "SELECT DISTINCT Department FROM Main_DA")
        tbDepartment.AutoCompleteMode = AutoCompleteMode.Suggest
        tbDepartment.AutoCompleteSource = AutoCompleteSource.CustomSource
        tbDepartment.AutoCompleteCustomSource = DataCollection_DA_Department

        'autocomplete tu dong dien thong tin cho textbox tbDepartment
        Dim DataCollection_DA_Delegation As New AutoCompleteStringCollection()
        Module_DA.AUTO_COMPLETE_GET_DATA(DataCollection_DA_Delegation, link_database_DA, "SELECT DISTINCT Delegation FROM Main_DA")
        tbDelegation.AutoCompleteMode = AutoCompleteMode.Suggest
        tbDelegation.AutoCompleteSource = AutoCompleteSource.CustomSource
        tbDelegation.AutoCompleteCustomSource = DataCollection_DA_Delegation

        'autocomplete tu dong dien thong tin cho textbox tbDepartment
        Dim DataCollection_DA_Note As New AutoCompleteStringCollection()
        Module_DA.AUTO_COMPLETE_GET_DATA(DataCollection_DA_Note, link_database_DA, "SELECT DISTINCT Note FROM Main_DA")
        tbNote.AutoCompleteMode = AutoCompleteMode.Suggest
        tbNote.AutoCompleteSource = AutoCompleteSource.CustomSource
        tbNote.AutoCompleteCustomSource = DataCollection_DA_Note

        tbEffectiveDate.Format = DateTimePickerFormat.Custom
        tbEffectiveDate.CustomFormat = "dd/MM/yyyy"
        tbExpiryDate.Format = DateTimePickerFormat.Custom
        tbExpiryDate.CustomFormat = "dd/MM/yyyy"
        tbExpiryDate.Value = DateTime.ParseExact("31/12/2099", "dd/MM/yyyy", Nothing)

        'add value to ComboBox_Segment
        ComboBox_Segment.Items.Add("WB")
        ComboBox_Segment.Items.Add("SME")
        ComboBox_Segment.Items.Add("INDIVIDUAL")
    End Sub

    Private Sub tbDAFrom_PWID_TextChanged(sender As Object, e As EventArgs) Handles tbDAFrom_PWID.TextChanged
        tbDAFrom_Name.Text = Module_DA.SQL_QUERY_TO_STRING(link_database_DA, "SELECT DISTINCT DA_From FROM Main_DA WHERE PWID_DA_From = '" & tbDAFrom_PWID.Text & "'")
        If tbDAFrom_Name.Text.Length = 0 Then
            tbDAFrom_Name.Text = Module_DA.SQL_QUERY_TO_STRING(link_database_DA, "SELECT DISTINCT DA_To FROM Main_DA WHERE PWID_DA_To = '" & tbDAFrom_PWID.Text & "'")
        End If

        tbDAFrom_Position.Text = Module_DA.SQL_QUERY_TO_STRING(link_database_DA, "SELECT DISTINCT Position_From FROM Main_DA WHERE PWID_DA_From = '" & tbDAFrom_PWID.Text & "'")
        If tbDAFrom_Position.Text.Length = 0 Then
            tbDAFrom_Position.Text = Module_DA.SQL_QUERY_TO_STRING(link_database_DA, "SELECT DISTINCT Position_To FROM Main_DA WHERE PWID_DA_To = '" & tbDAFrom_PWID.Text & "'")
        End If
    End Sub

    Private Sub tbDATo_PWID_TextChanged(sender As Object, e As EventArgs) Handles tbDATo_PWID.TextChanged
        tbDATo_Name.Text = Module_DA.SQL_QUERY_TO_STRING(link_database_DA, "SELECT DISTINCT DA_To FROM Main_DA WHERE PWID_DA_To = '" & tbDATo_PWID.Text & "'")
        If tbDATo_Name.Text.Length = 0 Then
            tbDATo_Name.Text = Module_DA.SQL_QUERY_TO_STRING(link_database_DA, "SELECT DISTINCT DA_From FROM Main_DA WHERE PWID_DA_From = '" & tbDATo_PWID.Text & "'")
        End If

        tbDATo_Position.Text = Module_DA.SQL_QUERY_TO_STRING(link_database_DA, "SELECT DISTINCT Position_To FROM Main_DA WHERE PWID_DA_To = '" & tbDATo_PWID.Text & "'")
        If tbDATo_Position.Text.Length = 0 Then
            tbDATo_Position.Text = Module_DA.SQL_QUERY_TO_STRING(link_database_DA, "SELECT DISTINCT Position_From FROM Main_DA WHERE PWID_DA_From = '" & tbDATo_PWID.Text & "'")
        End If
    End Sub

    Private Sub btClear_Click(sender As Object, e As EventArgs) Handles btClear.Click
        tbDAFrom_Name.Text = ""
        tbDAFrom_Position.Text = ""
        tbDAFrom_PWID.Text = ""
        tbDATo_Name.Text = ""
        tbDATo_Position.Text = ""
        tbDATo_PWID.Text = ""
        tbDelegation.Text = ""
        tbDepartment.Text = ""
        ComboBox_Segment.Text = ""
        tbEffectiveDate.Value = DateTime.ParseExact(Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", Nothing)
        tbExpiryDate.Value = DateTime.ParseExact("31/12/2099", "dd/MM/yyyy", Nothing)
        tbNote.Text = ""
        GridControl1.DataSource = SQL_QUERY_TO_DATATABLE(link_database_DA, "SELECT * FROM Main_DA WHERE Deleted_Date IS NULL ORDER BY [DA_To] ASC, [DA_From] ASC, CAST(SUBSTR([Effective_Date], 7, 4) AS INT) DESC, CAST(SUBSTR([Effective_Date], 4, 2) AS INT) DESC, CAST(SUBSTR([Effective_Date], 1, 2) AS INT) DESC")
        GridView1.BestFitColumns()
    End Sub

    Private Sub btEdit_Click(sender As Object, e As EventArgs) Handles btEdit.Click
        On Error GoTo err_handle

        If lbCaseID.Text.Length = 0 Then Exit Sub

        Dim iRet = MsgBox("Do you want to update information for case ID " & lbCaseID.Text & "?", vbYesNo + vbQuestion)
        If iRet = vbYes Then

            Dim str_update As String = "UPDATE Main_DA" &
                                     " SET [DA_From] = '" & tbDAFrom_Name.Text & "'," &
                                        " [Position_From] = '" & tbDAFrom_Position.Text & "'," &
                                        " [PWID_DA_From] = '" & tbDAFrom_PWID.Text & "'," &
                                        " [DA_To] = '" & tbDATo_Name.Text & "'," &
                                        " [Position_To] = '" & tbDATo_Position.Text & "'," &
                                        " [PWID_DA_To] = '" & tbDATo_PWID.Text & "'," &
                                        " [Delegation] = '" & tbDelegation.Text & "'," &
                                        " [Segment] = '" & ComboBox_Segment.Text & "'," &
                                        " [Department] = '" & tbDepartment.Text & "'," &
                                        " [Effective_Date] = '" & Format(tbEffectiveDate.Value, "dd/MM/yyyy") & "'," &
                                        " [Expiry_Date] = '" & Format(tbExpiryDate.Value, "dd/MM/yyyy") & "'," &
                                        " [Note] = '" & tbNote.Text & "'," &
                                        " [User_Modified] = '" & Main.BarStaticItem_User.Caption & "_" & Now.ToString("dd/MM/yyyy hh:mm:ss tt") & "'" &
                                     " WHERE [CaseID] = '" & lbCaseID.Text & "'"

            SQL_QUERY_WRITE_LOG(link_database_DA, str_update)

            GridControl1.DataSource = SQL_QUERY_TO_DATATABLE(link_database_DA, "SELECT * FROM Main_DA WHERE Deleted_Date IS NULL ORDER BY [DA_To] ASC, [DA_From] ASC, CAST(SUBSTR([Effective_Date], 7, 4) AS INT) DESC, CAST(SUBSTR([Effective_Date], 4, 2) AS INT) DESC, CAST(SUBSTR([Effective_Date], 1, 2) AS INT) DESC")
            GridView1.BestFitColumns()
            Main.Auto_Set_Expired()
        End If
        Exit Sub
err_handle:
        Error_handle()
    End Sub

    Private Sub tbStrSearch_EditValueChanged(sender As Object, e As EventArgs) Handles tbStrSearch.EditValueChanged
        On Error GoTo err_handle

        If tbStrSearch.Text.Length = 0 Then Exit Sub

        Dim DT_ListCol As DataTable = SQL_QUERY_TO_DATATABLE(link_database_DA, "SELECT * FROM [Main_DA] LIMIT 1")

        Dim SQL_Str_Search As String = ""

        For i = 0 To DT_ListCol.Columns.Count - 1
            If SQL_Str_Search.Length = 0 Then
                SQL_Str_Search = "[" & DT_ListCol.Columns(i).ColumnName.ToString() & "] LIKE '%" & tbStrSearch.Text & "%' "
            Else
                SQL_Str_Search = SQL_Str_Search & " OR [" & DT_ListCol.Columns(i).ColumnName.ToString() & "] LIKE '%" & tbStrSearch.Text & "%' "
            End If
        Next

        GridControl1.DataSource = SQL_QUERY_TO_DATATABLE(link_database_DA, "SELECT * FROM Main_DA WHERE Deleted_Date IS NULL AND (" & SQL_Str_Search & ") ORDER BY [DA_To] ASC, [DA_From] ASC, CAST(SUBSTR([Effective_Date], 7, 4) AS INT) DESC, CAST(SUBSTR([Effective_Date], 4, 2) AS INT) DESC, CAST(SUBSTR([Effective_Date], 1, 2) AS INT) DESC")
        GridView1.BestFitColumns()
        Exit Sub
err_handle:
        Module_DA.Error_handle()
    End Sub

    Private Sub GridView1_DoubleClick(sender As Object, e As EventArgs) Handles GridView1.DoubleClick
        tbDelegation.Text = GridView1.GetFocusedRowCellDisplayText("Delegation").ToString
        tbEffectiveDate.Text = DateTime.ParseExact(GridView1.GetFocusedRowCellDisplayText("Effective_Date").ToString, "dd/MM/yyyy", Nothing)
        tbExpiryDate.Text = DateTime.ParseExact(GridView1.GetFocusedRowCellDisplayText("Expiry_Date").ToString, "dd/MM/yyyy", Nothing)
        tbNote.Text = GridView1.GetFocusedRowCellDisplayText("Note").ToString
        tbDAFrom_Name.Text = GridView1.GetFocusedRowCellDisplayText("DA_From").ToString
        tbDAFrom_Position.Text = GridView1.GetFocusedRowCellDisplayText("Position_From").ToString
        tbDAFrom_PWID.Text = GridView1.GetFocusedRowCellDisplayText("PWID_DA_From").ToString
        tbDATo_Name.Text = GridView1.GetFocusedRowCellDisplayText("DA_To").ToString
        tbDATo_Position.Text = GridView1.GetFocusedRowCellDisplayText("Position_To").ToString
        tbDATo_PWID.Text = GridView1.GetFocusedRowCellDisplayText("PWID_DA_To").ToString
        ComboBox_Segment.Text = GridView1.GetFocusedRowCellDisplayText("Segment").ToString
        tbDepartment.Text = GridView1.GetFocusedRowCellDisplayText("Department").ToString
        lbCaseID.Text = GridView1.GetFocusedRowCellDisplayText("CaseID").ToString
    End Sub



    Private Sub btDeleteAllExpired_DA_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btDeleteAllExpired_DA.ItemClick
        On Error GoTo err_handle

        Dim iRet = MsgBox("Do you want to delete all expired DA?", vbYesNo + vbQuestion)

        If iRet = vbYes Then
            SQL_QUERY_WRITE_LOG(link_database_DA, "DELETE FROM Main_DA WHERE [Status] = 'Expired'")
            SQL_QUERY_WRITE_LOG(link_database_DA, "DELETE FROM Main_DA WHERE [Deleted_Date] IS NOT NULL")
            SQL_QUERY_WRITE_LOG(link_database_DA, "vacuum")
        End If
        Exit Sub
err_handle:
        Module_DA.Error_handle()
    End Sub

    Private Sub btDeleteCase_Click(sender As Object, e As EventArgs) Handles btDeleteCase.Click
        On Error GoTo err_handle

        If lbCaseID.Text.Length = 0 Then Exit Sub

        Dim iRet = MsgBox("Do you want to delete DA with CaseID: " & lbCaseID.Text & "?", vbYesNo + vbQuestion)
        If iRet = vbYes Then
            Dim str_update As String = "UPDATE Main_DA" &
                                     " SET [Deleted_Date] = '" & Main.BarStaticItem_User.Caption & "_" & Now.ToString("yyyyMMdd") & "'," &
                                        " [User_Modified] = '" & Main.BarStaticItem_User.Caption & "_" & Now.ToString("dd/MM/yyyy hh:mm:ss tt") & "'" &
                                     " WHERE [CaseID] = '" & lbCaseID.Text & "'"

            SQL_QUERY_WRITE_LOG(link_database_DA, str_update)

            MsgBox("Completed", vbOKOnly, "DA Management")
        End If
        Exit Sub
err_handle:
        Module_DA.Error_handle()
    End Sub

    Private Sub btExportToExcel_Click(sender As Object, e As EventArgs) Handles btExportToExcel.Click
        On Error GoTo err_handle
        EXPORT_DATAGRIDVIEW_TO_EXCEL(GridView1)
        Exit Sub
err_handle:
        Error_handle()
    End Sub

    Private Sub btBulkUpdateFrExcel_Click(sender As Object, e As EventArgs) Handles btBulkUpdateFrExcel.Click
        On Error GoTo err_handle

        Dim source_file_list_master As String = SELECT_EXCEL_FILE_RETURNED_FULL_PATH("Please select excel file (.xls)")

        Dim conn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & source_file_list_master & ";Extended Properties='Excel 8.0;HDR=Yes'")
        conn.Open()
        'Dim sql = "SELECT Department,DA_To,PWID_DA_To,Position_To,Delegation,Effective_Date,Expiry_Date,Status,Note,DA_From,PWID_DA_From,Position_From,Segment,CaseID FROM [Sheet1$]"
        Dim sql = "SELECT * FROM [Sheet1$]"
        Dim cmdDataGrid As OleDbCommand = New OleDbCommand(sql, conn)
        Dim da As New OleDbDataAdapter
        da.SelectCommand = cmdDataGrid
        Dim DT As New DataTable
        da.Fill(DT)

        Dim Str_SQL_Add_Record As String = ""
        For Each Drr As DataRow In DT.Rows
            If Drr("CaseID").ToString.Length > 0 Then
                Dim str_SET_UPDATE As String = ""

                If Drr("Department").ToString.Length > 0 Then
                    str_SET_UPDATE = str_SET_UPDATE & " [Department] = '" & Drr("Department").ToString & "',"
                Else
                    str_SET_UPDATE = str_SET_UPDATE & " [Department] = '',"
                End If

                If Drr("DA_To").ToString.Length > 0 Then
                    str_SET_UPDATE = str_SET_UPDATE & " [DA_To] = '" & Drr("DA_To").ToString & "',"
                Else
                    str_SET_UPDATE = str_SET_UPDATE & " [DA_To] = '',"
                End If

                If Drr("PWID_DA_To").ToString.Length > 0 Then
                    str_SET_UPDATE = str_SET_UPDATE & " [PWID_DA_To] = '" & Drr("PWID_DA_To").ToString & "',"
                Else
                    str_SET_UPDATE = str_SET_UPDATE & " [PWID_DA_To] = '',"
                End If

                If Drr("Position_To").ToString.Length > 0 Then
                    str_SET_UPDATE = str_SET_UPDATE & " [Position_To] = '" & Drr("Position_To").ToString & "',"
                Else
                    str_SET_UPDATE = str_SET_UPDATE & " [Position_To] = '',"
                End If

                If Drr("Delegation").ToString.Length > 0 Then
                    str_SET_UPDATE = str_SET_UPDATE & " [Delegation] = '" & Drr("Delegation").ToString & "',"
                Else
                    str_SET_UPDATE = str_SET_UPDATE & " [Delegation] = '',"
                End If

                If Drr("Effective_Date").ToString.Length > 0 Then
                    str_SET_UPDATE = str_SET_UPDATE & " [Effective_Date] = '" & Drr("Effective_Date").ToString & "',"
                Else
                    str_SET_UPDATE = str_SET_UPDATE & " [Effective_Date] = '',"
                End If

                If Drr("Expiry_Date").ToString.Length > 0 Then
                    str_SET_UPDATE = str_SET_UPDATE & " [Expiry_Date] = '" & Drr("Expiry_Date").ToString & "',"
                Else
                    str_SET_UPDATE = str_SET_UPDATE & " [Expiry_Date] = '',"
                End If

                If Drr("Status").ToString.Length > 0 Then
                    str_SET_UPDATE = str_SET_UPDATE & " [Status] = '" & Drr("Status").ToString & "',"
                Else
                    str_SET_UPDATE = str_SET_UPDATE & " [Status] = '',"
                End If

                If Drr("Note").ToString.Length > 0 Then
                    str_SET_UPDATE = str_SET_UPDATE & " [Note] = '" & Drr("Note").ToString & "',"
                Else
                    str_SET_UPDATE = str_SET_UPDATE & " [Note] = '',"
                End If

                If Drr("DA_From").ToString.Length > 0 Then
                    str_SET_UPDATE = str_SET_UPDATE & " [DA_From] = '" & Drr("DA_From").ToString & "',"
                Else
                    str_SET_UPDATE = str_SET_UPDATE & " [DA_From] = '',"
                End If

                If Drr("PWID_DA_From").ToString.Length > 0 Then
                    str_SET_UPDATE = str_SET_UPDATE & " [PWID_DA_From] = '" & Drr("PWID_DA_From").ToString & "',"
                Else
                    str_SET_UPDATE = str_SET_UPDATE & " [PWID_DA_From] = '',"
                End If

                If Drr("Position_From").ToString.Length > 0 Then
                    str_SET_UPDATE = str_SET_UPDATE & " [Position_From] = '" & Drr("Position_From").ToString & "',"
                Else
                    str_SET_UPDATE = str_SET_UPDATE & " [Position_From] = '',"
                End If

                If Drr("Segment").ToString.Length > 0 Then
                    str_SET_UPDATE = str_SET_UPDATE & " [Segment] = '" & Drr("Segment").ToString & "',"
                Else
                    str_SET_UPDATE = str_SET_UPDATE & " [Segment] = '',"
                End If

                If str_SET_UPDATE <> "" Then
                    str_SET_UPDATE = str_SET_UPDATE & " [User_Modified] = '" & Environment.UserName & "_" & Now.ToString("ddMMyyyy hh:mm:ss") & "'"
                    SQL_QUERY_WRITE_LOG(link_database_DA, "UPDATE Main_DA SET" & str_SET_UPDATE & " WHERE [CaseID] = '" & Drr("CaseID").ToString & "'")
                End If
            End If
        Next

        conn.Close()
        MsgBox("Completed!", vbInformation)
        Exit Sub
err_handle:
        Error_handle()
    End Sub

    Private Sub BarButtonItem_Reactivate_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem_Reactivate.ItemClick
        On Error GoTo err_handle

        Dim Expired_Date As DateTime = tbExpiryDate.Value

        If Expired_Date < Today Then
            MsgBox("This record has been expired on " & Format(tbExpiryDate.Value, "dd/MM/yyyy"), vbOKOnly + vbCritical)
            Exit Sub
        End If

        If lbCaseID.Text.Length = 0 Then Exit Sub

        Dim iRet = MsgBox("Do you want to reactivate for caseID " & lbCaseID.Text & "?", vbYesNo + vbQuestion)

        If iRet = vbYes Then
            Dim str_update As String = "UPDATE Main_DA" &
                                     " SET [Status] = 'In Progress'," &
                                        " [User_Modified] = '" & Main.BarStaticItem_User.Caption & "_" & Now.ToString("dd/MM/yyyy hh:mm:ss tt") & "'" &
                                     " WHERE [CaseID] = '" & lbCaseID.Text & "'"

            SQL_QUERY_WRITE_LOG(link_database_DA, str_update)

            For i As Integer = 0 To GridView1.DataRowCount - 1
                If GridView1.GetRowCellValue(i, "CaseID").ToString() = lbCaseID.Text Then
                    GridView1.SetRowCellValue(i, "Status", "In Progress")
                End If
            Next
        End If
        Exit Sub
err_handle:
        Module_DA.Error_handle()
    End Sub

    Private Sub btSetExpired_Click_1(sender As Object, e As EventArgs) Handles btSetExpried.Click
        On Error GoTo err_handle

        If lbCaseID.Text.Length = 0 Then Exit Sub

        Dim iRet = MsgBox("Do you want to set Expired for caseID " & lbCaseID.Text & "?", vbYesNo + vbQuestion)

        If iRet = vbYes Then
            Dim str_update As String = "UPDATE Main_DA" &
                                     " SET [Status] = 'Expired'," &
                                        " [User_Modified] = '" & Main.BarStaticItem_User.Caption & "_" & Now.ToString("dd/MM/yyyy hh:mm:ss tt") & "'" &
                                     " WHERE [CaseID] = '" & lbCaseID.Text & "'"

            SQL_QUERY_WRITE_LOG(link_database_DA, str_update)

            For i As Integer = 0 To GridView1.DataRowCount - 1
                If GridView1.GetRowCellValue(i, "CaseID").ToString() = lbCaseID.Text Then
                    GridView1.SetRowCellValue(i, "Status", "Expired")
                End If
            Next
        End If
        Exit Sub
err_handle:
        Module_DA.Error_handle()
    End Sub
End Class