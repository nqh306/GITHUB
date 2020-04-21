Imports DevExpress.XtraBars
Imports System.Data.OleDb
Imports DevExpress.XtraEditors.Repository
Imports Excel = Microsoft.Office.Interop.Excel
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.Utils.Paint
Imports DevExpress.XtraGrid
Imports System.IO

Public Class FormTasetco
    Public appPath As String = AppDomain.CurrentDomain.BaseDirectory
    Public link_app_config As String = appPath & "Application_Config.db"
    Public link_application_config As String = Module_Letter_Management.SQL_QUERY_TO_STRING(link_app_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'Link_Application_Config'")
    Public Link_folder_database As String
    Public link_database_tasetco As String = ""
    Public table_name_Tasetco As String
    Public link_database_Special_List As String
    Public table_name_Special As String
    Public Link_database_Portfolio As String

    Private Sub Refresh_link_database()

        Link_folder_database = ""
        link_database_tasetco = ""

        'Get link_folder_database
        Link_folder_database = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Folder_Path FROM LIST_DATABASE WHERE Vendor_NAME = 'TASETCO EXPRESS' AND Database_Name = '" & BarEdit_Database.EditValue.ToString & "'")
        If Link_folder_database.Substring(Link_folder_database.Length - 1, 1) <> "\" Then
            Link_folder_database = Link_folder_database & "\"
        End If

        'Setup link database
        If Link_folder_database.Substring(Link_folder_database.Length - 1, 1) = "\" Then
            link_database_tasetco = Link_folder_database & "Database_Letter_Management.txt"
        Else
            link_database_tasetco = Link_folder_database & "\Database_Letter_Management.txt"
        End If

        'Setup table name of tasetco
        table_name_Tasetco = "Tasetco_" & BarEdit_Database.EditValue.ToString & "_" & BarEdit_Year.EditValue.ToString

        'Create database file and table tasetco if not exists
        Module_Letter_Management.SQLITE_CREATE_DATABASE_FILE(link_database_tasetco)
        Module_Letter_Management.SQLITE_CREATE_TABLE_TASETCO_IF_NOT_EXISTS(link_database_tasetco, table_name_Tasetco)

        'Get link database Special List
        link_database_Special_List = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Field_value1 FROM Config WHERE Field_name LIKE '%Link_database_Special_List%'")
        table_name_Special = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Table_Special_List FROM LIST_DATABASE WHERE Vendor_NAME = 'TASETCO EXPRESS' AND Database_Name = '" & BarEdit_Database.EditValue.ToString & "'")

        Module_Letter_Management.SQLITE_CREATE_DATABASE_FILE(link_database_Special_List)
        Module_Letter_Management.SQLITE_CREATE_TABLE_SPECIAL_LIST_IF_NOT_EXISTS(link_database_Special_List, table_name_Special)

        'Get link database Portfolio
        If Link_folder_database.Substring(Link_folder_database.Length - 1, 1) = "\" Then
            Link_database_Portfolio = Link_folder_database & "Database_Portfolio_" & Now.ToString("ddMMyyyy") & ".txt"
        Else
            Link_database_Portfolio = Link_folder_database & "\Database_Portfolio_" & Now.ToString("ddMMyyyy") & ".txt"
        End If

        'autocomplete tu dong dien thong tin cho textbox Document Note
        tbNote.AutoCompleteMode = AutoCompleteMode.Suggest
        tbNote.AutoCompleteSource = AutoCompleteSource.CustomSource
        Dim DataCollection_Note As New AutoCompleteStringCollection()
        Module_Letter_Management.AUTO_COMPLETE_GET_DATA(DataCollection_Note, link_database_tasetco, "SELECT DISTINCT Document_note FROM " & table_name_Tasetco)
        tbNote.AutoCompleteCustomSource = DataCollection_Note

        'autocomplete tu dong dien thong tin cho textbox Client_Name
        tbClientName.AutoCompleteMode = AutoCompleteMode.Suggest
        tbClientName.AutoCompleteSource = AutoCompleteSource.CustomSource
        Dim DataCollection_Client_Name As New AutoCompleteStringCollection()
        Module_Letter_Management.AUTO_COMPLETE_GET_DATA(DataCollection_Client_Name, link_database_tasetco, "SELECT DISTINCT Client_Name FROM " & table_name_Tasetco)
        tbClientName.AutoCompleteCustomSource = DataCollection_Client_Name

        'autocomplete tu dong dien thong tin cho textbox Client_address 
        tbAddress.AutoCompleteMode = AutoCompleteMode.Suggest
        tbAddress.AutoCompleteSource = AutoCompleteSource.CustomSource
        Dim DataCollection_Client_Address As New AutoCompleteStringCollection()
        Module_Letter_Management.AUTO_COMPLETE_GET_DATA(DataCollection_Client_Address, link_database_tasetco, "SELECT DISTINCT Mailing_address FROM " & table_name_Tasetco)
        tbAddress.AutoCompleteCustomSource = DataCollection_Client_Address

        'autocomplete tu dong dien thong tin cho textbox Client_attention 
        tbAttention.AutoCompleteMode = AutoCompleteMode.Suggest
        tbAttention.AutoCompleteSource = AutoCompleteSource.CustomSource
        Dim DataCollection_Attention As New AutoCompleteStringCollection()
        Module_Letter_Management.AUTO_COMPLETE_GET_DATA(DataCollection_Attention, link_database_tasetco, "SELECT DISTINCT Attention FROM " & table_name_Tasetco)
        tbAttention.AutoCompleteCustomSource = DataCollection_Attention

        backup_database(Link_folder_database)
    End Sub

    Private Sub FormTasetco_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'config GridView
        GridView1.OptionsBehavior.Editable = False
        WindowsFormsSettings.AllowAutoFilterConditionChange = DevExpress.Utils.DefaultBoolean.False

        'config format date for tbSentDate
        tbSentDate.Format = DateTimePickerFormat.Custom
        tbSentDate.CustomFormat = "dd/MM/yyyy"

        'Clear message in status bar
        Main.Statusbar_item1.Caption = ""
        Main.Statusbar_item2.Caption = ""

        'Add value database
        BarEdit_Database.EditWidth = 200
        Dim DT As DataTable = Module_Letter_Management.SQL_QUERY_TO_DATATABLE(link_application_config, "SELECT DISTINCT Database_Name FROM LIST_DATABASE WHERE Vendor_Name = 'TASETCO EXPRESS'")
        If DT.Rows.Count > 0 Then
            For i As Integer = 0 To DT.Rows.Count - 1
                CType(BarEdit_Database.Edit, RepositoryItemComboBox).Items.Add(DT.Rows(i).Item(0).ToString)
            Next
            BarEdit_Database.EditValue = DT.Rows(0).Item(0).ToString
        End If

        'add value year database
        For i = 2018 To CInt(Now.ToString("yyyy"))
            CType(BarEdit_Year.Edit, RepositoryItemComboBox).Items.Add(i)
        Next
        BarEdit_Year.EditWidth = 100
        BarEdit_Year.EditValue = CInt(Now.ToString("yyyy"))

        'Add value for combobox StrFilter
        ComboBox_StrFilter.Items.Add("")
        ComboBox_StrFilter.Items.Add("All")
        ComboBox_StrFilter.Items.Add("Waiting")
        ComboBox_StrFilter.Items.Add("Incomplete")
        ComboBox_StrFilter.Items.Add("Completed")
        ComboBox_StrFilter.Items.Add("Sent_date")
        ComboBox_StrFilter.Items.Add("Other")

        'Add value for combobox Final_Status
        ComboBox_FinalStatus.Items.Add("Completed")
        ComboBox_FinalStatus.Items.Add("Incomplete")
        ComboBox_FinalStatus.Items.Add("Waiting")
        ComboBox_FinalStatus.Items.Add("CROWN")

        'Can chinh hien thi cho textbox
        tbStrSearch.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        tbCROWN_Barcode.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        tbCROWN_SentDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center

        'disable textbox bill no and send date
        tbBillNo.ReadOnly = True
        tbSentDate.Enabled = False
    End Sub

    Private Sub BarButton_LoadDatabase_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarButton_LoadDatabase.ItemClick
        On Error GoTo err_handle
        Dim iRet = MsgBox("Do you want to open database " & BarEdit_Database.EditValue.ToString & " - " & BarEdit_Year.EditValue.ToString & "?", vbYesNo + vbQuestion)
        If iRet = vbYes Then
            Refresh_link_database()
            Main.Statusbar_item1.Caption = ""
            Main.Statusbar_item2.Caption = "Load database: '" & link_database_tasetco & "' - Table: " & BarEdit_Year.EditValue.ToString
        End If
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub btImportFromExcel_Click(sender As Object, e As EventArgs) Handles btImportFromExcel.Click
        On Error GoTo err_handle

        If link_database_tasetco.Length = 0 Then
            MsgBox("Please select database name and click button [Load Database]!", vbCritical)
            Exit Sub
        End If
        Module_Letter_Management.IMPORT_INTO_DATABASE_FROM_EXCEL(link_database_tasetco, table_name_Tasetco)

        MsgBox("Completed", vbOKOnly)
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub btRefresh_Click(sender As Object, e As EventArgs) Handles btRefresh.Click
        'On Error GoTo err_handle

        If link_database_tasetco.Length = 0 Then
            MsgBox("Please select database name and click button [Load Database]!", vbCritical)
            Exit Sub
        End If

        tbAttention.Text = ""
        tbBillNo.Text = ""
        tbClientName.Text = ""
        tbCROWN_Barcode.Text = ""
        tbCROWN_SentDate.Text = ""
        tbAddress.Text = ""
        tbMasterNo.Text = ""
        tbNote.Text = ""
        tbOtherVendor.Text = ""
        tbRemark.Text = ""
        tbSentDate.Text = ""
        tbStatus.Text = ""
        tbStrSearch.Text = ""
        tbTasetco_DateReceived.Text = ""
        tbTasetco_DateGetData.Text = ""
        tbTasetco_Status.Text = ""
        tbTasetco_Recipient.Text = ""
        tbUserModified.Text = ""
        ComboBox_FinalStatus.Text = ""
        ComboBox_StrFilter.Text = ""
        cbBatchBeginMonth.Checked = False
        rbtTasetco.Checked = True

        'PictureBox_TasetcoDetails.Image = Image.FromFile(filename)
        PictureBox_TasetcoDetails.Image = Nothing

        LOAD_DATABASE_TASETCO_TO_GRIDVIEW2(link_database_tasetco, table_name_Tasetco, "TODAY", "", GridControl_Tasetco, GridView1)
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub btSearch_Click(sender As Object, e As EventArgs) Handles btSearch.Click
        On Error GoTo err_handle

        GridControl_Tasetco.DataSource = Nothing
        GridView1.Columns.Clear()

        If link_database_tasetco.Length = 0 Then
            MsgBox("Please select database name and click button [Load Database]!", vbCritical)
            Exit Sub
        End If

        If tbStrSearch.Text.Length = 0 Then Exit Sub

        If Module_Letter_Management.SEARCH_DATABASE_BY_STRING_RETURN_BOOLEAN(link_database_Special_List, table_name_Special, tbStrSearch.Text) = True Then
            Dim iRet = MsgBox("Do you want to view the special address?", vbYesNo + vbQuestion)
            If iRet = vbYes Then

                Dim DT_Col As DataTable = SQL_QUERY_TO_DATATABLE(link_database_Special_List, "SELECT * FROM " & table_name_Special & " LIMIT 1")
                Dim SQL_Str_Search As String = ""
                For i = 0 To DT_Col.Columns.Count - 1
                    If SQL_Str_Search.Length = 0 Then
                        SQL_Str_Search = "[" & DT_Col.Columns(i).ColumnName.ToString() & "] LIKE '%" & tbStrSearch.Text & "%' "
                    Else
                        SQL_Str_Search = SQL_Str_Search & " OR [" & DT_Col.Columns(i).ColumnName.ToString() & "] LIKE '%" & tbStrSearch.Text & "%' "
                    End If
                Next

                GridControl_Tasetco.DataSource = SQL_QUERY_TO_DATATABLE(link_database_Special_List, "SELECT * FROM " & table_name_Special & " WHERE " & SQL_Str_Search)
            Else
                LOAD_DATABASE_TASETCO_TO_GRIDVIEW2(link_database_tasetco, table_name_Tasetco, "", tbStrSearch.Text, GridControl_Tasetco, GridView1)
            End If
        Else
            LOAD_DATABASE_TASETCO_TO_GRIDVIEW2(link_database_tasetco, table_name_Tasetco, "", tbStrSearch.Text, GridControl_Tasetco, GridView1)
        End If
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub btLoad_Click(sender As Object, e As EventArgs) Handles btLoad.Click
        On Error GoTo err_handle

        If link_database_tasetco.Length = 0 Then
            MsgBox("Please select database name and click button [Load Database]!", vbCritical)
            Exit Sub
        End If

        If ComboBox_StrFilter.Text.Length = 0 Then Exit Sub
        LOAD_DATABASE_TASETCO_TO_GRIDVIEW2(link_database_tasetco, table_name_Tasetco, ComboBox_StrFilter.Text, "", GridControl_Tasetco, GridView1)
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub


    '    Private Sub btLockUnlock_Click(sender As Object, e As EventArgs)
    '        On Error GoTo err_handle

    '        If tbTasetco_AddressRC.Enabled = False Then
    '            tbTasetco_AddressRC.Enabled = True
    '            tbTasetco_AcConsignee.Enabled = True
    '            tbTasetco_CusName.Enabled = True
    '            tbTasetco_CusNameRC.Enabled = True
    '            tbTasetco_DateReceived.Enabled = True
    '            tbTasetco_PO.Enabled = True
    '            tbTasetco_Status.Enabled = True
    '            tbTasetco_TP.Enabled = True
    '            tbTasetco_DateGetData.Enabled = True
    '            tbTasetco_PacketName.Enabled = True
    '        Else
    '            tbTasetco_AddressRC.Enabled = False
    '            tbTasetco_AcConsignee.Enabled = False
    '            tbTasetco_CusName.Enabled = False
    '            tbTasetco_CusNameRC.Enabled = False
    '            tbTasetco_DateReceived.Enabled = False
    '            tbTasetco_PO.Enabled = False
    '            tbTasetco_Status.Enabled = False
    '            tbTasetco_TP.Enabled = False
    '            tbTasetco_DateGetData.Enabled = False
    '            tbTasetco_PacketName.Enabled = False
    '        End If
    '        Exit Sub
    'err_handle:
    '        Module_Letter_Management.Error_handle()
    '    End Sub

    '    Private Sub btCopyToClipboard_Click(sender As Object, e As EventArgs)
    '        On Error GoTo err_handle
    '        Dim str_clipboard As String = ""
    '        str_clipboard = "Bill Number: " & tbBillNo.Text & Chr(10) &
    '                        "Name: " & tbClientName.Text & Chr(10) &
    '                        "Mailing Address: " & tbAddress.Text & Chr(10) &
    '                        "Attention: " & tbAttention.Text & Chr(10) &
    '                        "-----------------------" & Chr(10) &
    '                        "Cus Name:" & tbTasetco_CusName.Text & Chr(10) &
    '                        "Packet Name: " & tbTasetco_PacketName.Text & Chr(10) &
    '                        "PO: " & tbTasetco_PO.Text & Chr(10) &
    '                        "TP: " & tbTasetco_TP.Text & Chr(10) &
    '                        "Cus Name RC: " & tbTasetco_CusNameRC.Text & Chr(10) &
    '                        "Address RC: " & tbTasetco_AddressRC.Text & Chr(10) &
    '                        "Status: " & tbTasetco_Status.Text & Chr(10) &
    '                        "Cosignee: " & tbTasetco_AcConsignee.Text & Chr(10) &
    '                        "Date Received: " & tbTasetco_DateReceived.Text & Chr(10)

    '        Clipboard.SetText(str_clipboard)
    '        Exit Sub
    'err_handle:
    '        Module_Letter_Management.Error_handle()
    '    End Sub

    Private Sub btUpdate_Click(sender As Object, e As EventArgs) Handles btUpdate.Click
        On Error GoTo err_handle

        If link_database_tasetco.Length = 0 Then
            MsgBox("Please select database name and click button [Load Database]!", vbCritical)
            Exit Sub
        End If

        If tbBillNo.Text.Length = 0 Then Exit Sub

        Dim iRet = MsgBox("Do you want to update information for bill number " & tbBillNo.Text & "?", vbYesNo + vbQuestion)
        If iRet = vbYes Then
            Dim str_update As String = ""
            Dim str_vendor_name As String = ""
            If rbtTasetco.Checked = True Then
                str_vendor_name = "TASETCO"
            Else
                str_vendor_name = tbOtherVendor.Text
            End If

            Dim str_Batch_begin As String = ""
            If cbBatchBeginMonth.Checked = True Then
                str_Batch_begin = "Y"
            Else
                str_Batch_begin = "N"
            End If

            str_update = "UPDATE " & table_name_Tasetco &
                         " SET [Vendor_name] = '" & str_vendor_name & "'," &
                            " [Sent_date] = '" & tbSentDate.Text & "'," &
                            " [Client_name] = '" & tbClientName.Text & "'," &
                            " [Mailing_address] = '" & tbAddress.Text & "'," &
                            " [Attention] = '" & tbAttention.Text & "'," &
                            " [Master_No] = '" & tbMasterNo.Text & "'," &
                            " [Document_note] = '" & tbNote.Text & "'," &
                            " [ACS_Status] = '" & tbStatus.Text & "'," &
                            " [Remark] = '" & tbRemark.Text & "'," &
                            " [Batch_begin_month] = '" & str_Batch_begin & "'," &
                            " [Tasetco_Status] = '" & tbTasetco_Status.Text & "'," &
                            " [Tasetco_Ac_Cosignee] = '" & tbTasetco_Recipient.Text & "'," &
                            " [Tasetco_Date_Received] = '" & tbTasetco_DateReceived.Text & "'," &
                            " [Tasetco_Date_Get_Data] = '" & tbTasetco_DateGetData.Text & "'," &
                            " [User_Modified] = '" & Environment.UserName & "_" & Now.ToString("dd/MM/yyyy hh:mm:ss tt") & "'," &
                            " [Final_Result] = '" & ComboBox_FinalStatus.Text & "'," &
                            " [CROWN_Barcode] = '" & tbCROWN_Barcode.Text & "'," &
                            " [CROWN_Sent_date] = '" & tbCROWN_SentDate.Text & "'" &
                         " WHERE [Bill_Number] = '" & tbBillNo.Text & "'"

            Module_Letter_Management.SQL_QUERY(link_database_tasetco, str_update)
            LOAD_DATABASE_TASETCO_TO_GRIDVIEW2(link_database_tasetco, table_name_Tasetco, "", tbBillNo.Text, GridControl_Tasetco, GridView1)
        End If
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub btDelete_Click(sender As Object, e As EventArgs) Handles btDelete.Click
        On Error GoTo err_handle

        If link_database_tasetco.Length = 0 Then
            MsgBox("Please select database name and click button [Load Database]!", vbCritical)
            Exit Sub
        End If

        If tbBillNo.Text.Length = 0 Then Exit Sub

        Dim USER_CREATED As String = SQL_QUERY_TO_STRING(link_database_tasetco, "SELECT DISTINCT User_Created FROM " & table_name_Tasetco & " WHERE [Bill_Number] = '" & tbBillNo.Text & "'")

        If USER_CREATED.Contains(Environment.UserName) = False Then
            MsgBox("Access Denied", vbCritical)
            Exit Sub
        End If

        Dim iRet = MsgBox("Do you want to delete bill number " & tbBillNo.Text & "?", vbYesNo + vbQuestion)
        If iRet = vbYes Then
            'SQL_QUERY(link_database_tasetco, "UPDATE " & table_name_Tasetco & " SET [Deleted_Date] = '" & tbBillNo.Text & "' WHERE [Bill_Number] = '" & tbBillNo.Text & "'")
            'SQL_QUERY(link_database_tasetco, "UPDATE " & table_name_Tasetco & " SET [Bill_Number] = '" & tbBillNo.Text & "_deleted' WHERE [Deleted_Date] = '" & tbBillNo.Text & "'")
            'SQL_QUERY(link_database_tasetco, "UPDATE " & table_name_Tasetco & " SET [Deleted_Date] = '" & Now.ToString("dd/MM/yyyy") & "' WHERE [Bill_Number] = '" & tbBillNo.Text & "_deleted'")

            Dim SQL_string As String = "DELETE FROM " & table_name_Tasetco & " WHERE [Bill_number]='" & tbBillNo.Text & "'"
            Module_Letter_Management.SQL_QUERY(link_database_tasetco, SQL_string)
            LOAD_DATABASE_TASETCO_TO_GRIDVIEW2(link_database_tasetco, table_name_Tasetco, "TODAY", "", GridControl_Tasetco, GridView1)
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

    Private Sub GridView1_RowCellClick(sender As Object, e As RowCellClickEventArgs) Handles GridView1.RowCellClick
        PictureBox_TasetcoDetails.Image = Nothing
        If GridView1.Columns.Count = 4 Then
            tbRemark.Text = ""
            tbCROWN_Barcode.Text = ""
            tbCROWN_SentDate.Text = ""
            tbNote.Text = ""
            tbOtherVendor.Text = ""
            tbSentDate.Text = ""
            tbStatus.Text = ""
            tbTasetco_DateGetData.Text = ""
            tbTasetco_DateReceived.Text = ""
            tbTasetco_Recipient.Text = ""
            tbTasetco_Status.Text = ""
            tbUserModified.Text = ""
            cbBatchBeginMonth.Checked = False
            rbtTasetco.Checked = True

            tbMasterNo.Text = GridView1.GetFocusedRowCellDisplayText("MasterNo").ToString
            tbClientName.Text = GridView1.GetFocusedRowCellDisplayText("Client_Name").ToString
            tbAddress.Text = GridView1.GetFocusedRowCellDisplayText("Client_Address").ToString
            tbAttention.Text = GridView1.GetFocusedRowCellDisplayText("Client_Attention").ToString
        Else
            If GridView1.Columns.Count = 8 Then
                tbCROWN_Barcode.Text = ""
                tbCROWN_SentDate.Text = ""
                tbNote.Text = ""
                tbOtherVendor.Text = ""
                tbSentDate.Text = ""
                tbStatus.Text = ""
                tbTasetco_DateGetData.Text = ""
                tbTasetco_DateReceived.Text = ""
                tbTasetco_Recipient.Text = ""
                tbTasetco_Status.Text = ""
                tbUserModified.Text = ""
                cbBatchBeginMonth.Checked = False
                rbtTasetco.Checked = True
                tbMasterNo.Text = GridView1.GetFocusedRowCellDisplayText("Master_No").ToString
                tbClientName.Text = GridView1.GetFocusedRowCellDisplayText("Client_name").ToString
                tbAddress.Text = GridView1.GetFocusedRowCellDisplayText("Client_address").ToString
                tbAttention.Text = GridView1.GetFocusedRowCellDisplayText("Client_Attention").ToString
                tbRemark.Text = GridView1.GetFocusedRowCellDisplayText("Remark").ToString
            Else
                If GridView1.GetFocusedRowCellDisplayText("Vendor_Name").ToString = "TASETCO" Then
                    rbtTasetco.Checked = True
                Else
                    rbtOtherVendor.Checked = True
                    tbOtherVendor.Text = GridView1.GetFocusedRowCellDisplayText("Vendor_Name").ToString
                End If
                tbBillNo.Text = GridView1.GetFocusedRowCellDisplayText("Bill_Number").ToString
                tbSentDate.Text = DateTime.ParseExact(GridView1.GetFocusedRowCellDisplayText("Sent_date").ToString, "dd/MM/yyyy", Nothing)

                'tbSentDate.Text = GridView1.GetFocusedRowCellDisplayText("Sent_date").ToString
                tbClientName.Text = GridView1.GetFocusedRowCellDisplayText("Client_Name").ToString
                tbAddress.Text = GridView1.GetFocusedRowCellDisplayText("Mailing_address").ToString
                tbAttention.Text = GridView1.GetFocusedRowCellDisplayText("Attention").ToString
                tbMasterNo.Text = GridView1.GetFocusedRowCellDisplayText("Master_No").ToString
                tbNote.Text = GridView1.GetFocusedRowCellDisplayText("Document_note").ToString
                tbStatus.Text = GridView1.GetFocusedRowCellDisplayText("ACS_Status").ToString
                tbRemark.Text = GridView1.GetFocusedRowCellDisplayText("Remark").ToString

                If GridView1.GetFocusedRowCellDisplayText("Batch_begin_month").ToString = "Y" Then
                    cbBatchBeginMonth.Checked = True
                    tbBillNo.ReadOnly = True
                    tbSentDate.Enabled = False
                Else
                    If GridView1.GetFocusedRowCellDisplayText("Batch_begin_month").ToString = "N" Then
                        cbBatchBeginMonth.Checked = False
                    Else
                        If GridView1.GetFocusedRowCellDisplayText("Batch_begin_month").ToString = "M" Then
                            cbBatchBeginMonth.Checked = False
                            tbBillNo.ReadOnly = False
                            tbSentDate.Enabled = True
                        End If
                    End If
                End If
                tbTasetco_Status.Text = GridView1.GetFocusedRowCellDisplayText("Tasetco_Status").ToString
                tbTasetco_Recipient.Text = GridView1.GetFocusedRowCellDisplayText("Tasetco_Ac_Cosignee").ToString
                tbTasetco_DateReceived.Text = GridView1.GetFocusedRowCellDisplayText("Tasetco_Date_Received").ToString
                tbTasetco_DateGetData.Text = GridView1.GetFocusedRowCellDisplayText("Tasetco_Date_Get_Data").ToString
                tbUserModified.Text = GridView1.GetFocusedRowCellDisplayText("User_Modified").ToString
                ComboBox_FinalStatus.Text = GridView1.GetFocusedRowCellDisplayText("Final_Result").ToString
                tbCROWN_Barcode.Text = GridView1.GetFocusedRowCellDisplayText("CROWN_Barcode").ToString
                tbCROWN_SentDate.Text = GridView1.GetFocusedRowCellDisplayText("CROWN_Sent_Date").ToString
            End If
        End If
    End Sub

    Private Sub btAdd_Click(sender As Object, e As EventArgs) Handles btAdd.Click
        On Error GoTo err_handle

        If link_database_tasetco.Length = 0 Then
            MsgBox("Please select database name and click button [Load Database]!", vbCritical)
            Exit Sub
        End If

        Dim bill_number_string As String = ""
        Dim vendor_name As String = ""
        Dim batch_begin As String = ""
        Dim batch_frequency As String = ""

        'check database cua nam hien tai da co hay chua
        BarEdit_Year.EditValue = CInt(Now.ToString("yyyy"))
        Refresh_link_database()
        Dim database_name As String = BarEdit_Database.EditValue.ToString

        'Check du thong tin de tao Bill hay chua
        If tbClientName.Text.Length = 0 Or tbAddress.Text.Length = 0 Then
            MsgBox("no DATA")
            Exit Sub
        End If

        Dim iRet = MsgBox("Do you want to add new record?", vbYesNo + vbQuestion)
        If iRet = vbYes Then
            'create bill number

            If rbtOtherVendor.Checked = True Then
                vendor_name = tbOtherVendor.Text
                batch_begin = "M"
                If tbBillNo.Text.Length = 0 Then
                    bill_number_string = InputBox("Please input bill number for create!", "Vendor Management - Message")
                Else
                    bill_number_string = tbBillNo.Text
                End If
            Else
                vendor_name = "TASETCO"
                Dim count_bill_tasetco As Integer
                'create bill for tasetco vendor
                If cbBatchBeginMonth.Checked = False Then
                    batch_begin = "N"
                    batch_frequency = "Daily"
                    count_bill_tasetco = Module_Letter_Management.SQL_QUERY_TO_INTEGER(link_database_tasetco, "SELECT COUNT(Bill_Number) FROM " & table_name_Tasetco & " WHERE [Sent_date] = '" & Now.ToString("dd/MM/yyyy") & "' AND [Vendor_name]='TASETCO' AND [Batch_begin_month]='N'")
                Else
                    batch_begin = "Y"
                    batch_frequency = "Monthly"
                    count_bill_tasetco = Module_Letter_Management.SQL_QUERY_TO_INTEGER(link_database_tasetco, "SELECT COUNT(Bill_Number) FROM " & table_name_Tasetco & " WHERE [Sent_date] = '" & Now.ToString("dd/MM/yyyy") & "' AND [Vendor_name]='TASETCO' AND [Batch_begin_month]='Y'")
                End If
                Dim DT_LogicBill As DataTable = Module_Letter_Management.SQL_QUERY_TO_DATATABLE(link_application_config, "SELECT Logic_Frequency, Logic_SequenceNo, Logic_Type, Logic_Value_As_Number, Logic_Value_As_Text, Logic_Period_From, Logic_Period_To, Logic_Format FROM LIST_DATABASE WHERE Logic_Frequency = '" & batch_frequency & "' AND Database_Name = '" & database_name & "' ORDER BY Logic_SequenceNo ASC")
                If DT_LogicBill.Rows.Count = 0 Then
                    MsgBox("Not yet create logic bill for this database", vbCritical)
                    Exit Sub
                Else
                    For dong_logic As Integer = 0 To DT_LogicBill.Rows.Count - 1
                        If DT_LogicBill.Rows(dong_logic).Item(2).ToString = "Date" Then
                            Dim type_date_format As String = DT_LogicBill.Rows(dong_logic).Item(7).ToString
                            bill_number_string = bill_number_string & Now.ToString(type_date_format)
                        End If
                        If DT_LogicBill.Rows(dong_logic).Item(2).ToString = "Text" Then
                            Dim type_text_Value As String = DT_LogicBill.Rows(dong_logic).Item(4).ToString
                            Dim type_text_Format As String = DT_LogicBill.Rows(dong_logic).Item(7).ToString
                            If type_text_Format.Length = 0 Then
                                bill_number_string = bill_number_string & type_text_Value
                            Else
                                bill_number_string = bill_number_string & Format(type_text_Value, type_text_Format)
                            End If
                        End If
                        If DT_LogicBill.Rows(dong_logic).Item(2).ToString = "Numberic (Fixed)" Then
                            Dim type_numberic_fixed_Value As Integer = DT_LogicBill.Rows(dong_logic).Item(3).ToString
                            bill_number_string = bill_number_string & type_numberic_fixed_Value
                        End If
                        If DT_LogicBill.Rows(dong_logic).Item(2).ToString = "Numberic (Period)" Then
                            Dim type_numberic_period_from As Integer = DT_LogicBill.Rows(dong_logic).Item(5).ToString
                            Dim type_numberic_period_to As Integer = DT_LogicBill.Rows(dong_logic).Item(6).ToString
                            Dim type_numberic_format As String = DT_LogicBill.Rows(dong_logic).Item(7).ToString

                            If type_numberic_period_from + count_bill_tasetco > type_numberic_period_to Then
                                MsgBox("New bill Created Failed. Maximumum can create is " & count_bill_tasetco)
                            End If
                            bill_number_string = bill_number_string & Format(type_numberic_period_from + count_bill_tasetco, type_numberic_format)
                        End If
                    Next
                End If
            End If

            Dim Sent_date As String = Now.ToString("dd/MM/yyyy")

            If cbAutoCreateBill.Checked = False Then
                If tbBillNo.Text.Length = 0 Or tbSentDate.Text.Length = 0 Then
                    MsgBox("Please input Bill Number and Send Date for create new record.", vbCritical, "Vendor Management")
                    Exit Sub
                End If
                bill_number_string = tbBillNo.Text
                Sent_date = tbSentDate.Text
                batch_begin = "M"
            End If

            If Module_Letter_Management.SQL_QUERY_TO_INTEGER(link_database_tasetco, "SELECT COUNT(Bill_Number) FROM " & table_name_Tasetco & " WHERE [Bill_Number]='" & bill_number_string & "'") > 0 Then
                Dim bill_number_string2 As String = InputBox("Bill number " & bill_number_string & " existed. Please input new bill number for create:", "Vendor Management - Message")
                If Len(bill_number_string2) = 0 Then
                    Exit Sub
                Else
                    If Module_Letter_Management.SQL_QUERY_TO_INTEGER(link_database_tasetco, "SELECT COUNT(Bill_Number) FROM " & table_name_Tasetco & " WHERE [Bill_Number]='" & bill_number_string2 & "'") > 0 Then
                        MsgBox("Can't create new bill. Bill number " & bill_number_string2 & " existed", vbCritical, "Vendor Management - Error")
                        Exit Sub
                    Else
                        bill_number_string = bill_number_string2
                    End If
                End If
            End If

            Dim Str_SQL_Add_Record As String = "INSERT INTO " & table_name_Tasetco & " ([Vendor_Name], [Bill_Number], [Sent_date], [Client_Name], [Mailing_address], [Attention], [Master_No], [Document_note], [Batch_begin_month], [User_Created], [Final_Result]) VALUES ('" & UCase(vendor_name) & "', '" & UCase(bill_number_string) & "', '" & Sent_date & "', '" & UCase(tbClientName.Text) & "', '" & UCase(tbAddress.Text) & "', '" & UCase(tbAttention.Text) & "', '" & UCase(tbMasterNo.Text) & "', '" & UCase(tbNote.Text) & "', '" & UCase(batch_begin) & "', '" & UCase(Environment.UserName) & "_" & Now.ToString("dd/MM/yyyy hh:mm:ss tt") & "', 'Waiting');"
            Module_Letter_Management.SQL_QUERY(link_database_tasetco, Str_SQL_Add_Record)
            LOAD_DATABASE_TASETCO_TO_GRIDVIEW2(link_database_tasetco, table_name_Tasetco, "TODAY", "", GridControl_Tasetco, GridView1)
            Main.Statusbar_item2.Caption = "Add new bill " & tbBillNo.Text & ": COMPLETED"
        End If
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub btFromPortfolio_Click(sender As Object, e As EventArgs) Handles btFromPortfolio.Click
        On Error GoTo err_handle

        If link_database_tasetco.Length = 0 Then
            MsgBox("Please select database name and click button [Load Database]!", vbCritical)
            Exit Sub
        End If

        Dim TimerStart As DateTime
        Dim TimeSpent As System.TimeSpan

        If tbStrSearch.Text.Length = 0 Then Exit Sub

        GridControl_Tasetco.DataSource = Nothing
        GridView1.Columns.Clear()


        Dim DT As New DataTable

        If SEARCH_DATABASE_BY_STRING_RETURN_BOOLEAN(link_database_Special_List, table_name_Special, tbStrSearch.Text) = True Then
            Dim iRet = MsgBox("Do you want to view the special address?", vbYesNo + vbQuestion)
            If iRet = vbYes Then
                TimerStart = Now

                DT = SQL_SEARCH_IN_ALL_COLUMN(link_database_Special_List, table_name_Special, tbStrSearch.Text)
                GridControl_Tasetco.DataSource = DT

                TimeSpent = Now.Subtract(TimerStart)
                Main.Statusbar_item2.Caption = "Search " & tbStrSearch.Text & " in " & Format(TimeSpent.TotalSeconds, "0.00") & " seconds"
            Else
                TimerStart = Now
                CheckVersionPortfolioReport(Link_folder_database)
                DT = SQL_SEARCH_IN_ALL_COLUMN(Link_database_Portfolio, "Portfolio", tbStrSearch.Text)

                GridControl_Tasetco.DataSource = DT

                TimeSpent = Now.Subtract(TimerStart)
                Main.Statusbar_item2.Caption = "Search " & tbStrSearch.Text & " in " & Format(TimeSpent.TotalSeconds, "0.00") & " seconds"
            End If
        Else
            TimerStart = Now

            CheckVersionPortfolioReport(Link_folder_database)
            DT = SQL_SEARCH_IN_ALL_COLUMN(Link_database_Portfolio, "Portfolio", tbStrSearch.Text)

            GridControl_Tasetco.DataSource = DT
            GridView1.BestFitColumns()


            TimeSpent = Now.Subtract(TimerStart)
            Main.Statusbar_item2.Caption = "Search " & tbStrSearch.Text & " in " & Format(TimeSpent.TotalSeconds, "0.00") & " seconds"
        End If
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub btByBatch_Click(sender As Object, e As EventArgs) Handles btByBatch.Click
        On Error GoTo err_handle

        If link_database_tasetco.Length = 0 Then
            MsgBox("Please select database name and click button [Load Database]!", vbCritical)
            Exit Sub
        End If

        'check database cua nam hien tai da co hay chua
        BarEdit_Year.EditValue = CInt(Now.ToString("yyyy"))
        BarButton_LoadDatabase.PerformClick()

        Dim dt As New DataTable

        Dim str_Date_Bill2 As String = InputBox("Please input date for sending letter (Format: dd/MM/yyyy): ", "Vendor Management - Message", Now.ToString("dd/MM/yyyy"))

        Dim str_Date_Bill As Date = CDate(str_Date_Bill2.Substring(6, 4) & "-" & str_Date_Bill2.Substring(3, 2) & "-" & str_Date_Bill2.Substring(0, 2))

        If Len(str_Date_Bill) = 0 Then Exit Sub

        If str_Date_Bill.ToString("dd/MM/yyyy") < Now.ToString("dd/MM/yyyy") Then
            MsgBox("Can't create bill for the past", vbCritical, "Vendor Management - Error")
            Exit Sub
        End If

        Dim batch_begin_of_month As String = InputBox("Is this letter for batch begin of month: (Y/N)", "Vendor Management - Message", "N")
        batch_begin_of_month = UCase(batch_begin_of_month)
        If batch_begin_of_month.Length = 0 Then Exit Sub

        Dim file_name_Portfolio As String
        If Link_folder_database.Substring(Link_folder_database.Length - 1, 1) = "\" Then
            file_name_Portfolio = Link_folder_database & "Database_Portfolio_" & Now.ToString("ddMMyyyy") & ".txt"
        Else
            file_name_Portfolio = Link_folder_database & "\Database_Portfolio_" & Now.ToString("ddMMyyyy") & ".txt"
        End If

        If Not My.Computer.FileSystem.FileExists(file_name_Portfolio) Then
            Main.Statusbar_item2.Caption = "Importing database Portfolio..."
            Module_Letter_Management.CheckVersionPortfolioReport(Link_folder_database)

            Main.Statusbar_item2.Caption = "Importing database CUSTDTL_FOB..."
            Module_Letter_Management.Check_Version_CUSTDTL_FOB_Report(Link_folder_database)
        Else
            Main.Statusbar_item2.Caption = "Database Portfolio exists. Checking table CUSTDTL..."
            dt = Module_Letter_Management.GET_ALL_TABLE_NAME_IN_DATABASE(file_name_Portfolio)
            Dim create_table_CUSTDTL As Boolean = True
            Dim create_table_Portfolio As Boolean = True
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    If dt.Rows(i).Item(0).ToString() = "CUSTDTL" Then
                        create_table_CUSTDTL = False
                        Exit For
                    End If
                Next
                If create_table_CUSTDTL = True Then
                    Main.Statusbar_item2.Caption = "Importing database CUSTDTL..."
                    Module_Letter_Management.Check_Version_CUSTDTL_FOB_Report(Link_folder_database)
                End If
                For i As Integer = 0 To dt.Rows.Count - 1
                    If dt.Rows(i).Item(0).ToString() = "Portfolio" Then
                        create_table_Portfolio = False
                        Exit For
                    End If
                Next
                If create_table_Portfolio = True Then
                    Main.Statusbar_item2.Caption = "Importing database Portfolio..."
                    Module_Letter_Management.CheckVersionPortfolioReport(Link_folder_database)
                End If
            Else
                Main.Statusbar_item2.Caption = "Database Portfolio not ok. Delete exists Portfolio database file and reimport"
                Kill(file_name_Portfolio)
                Main.Statusbar_item2.Caption = "Importing database Portfolio..."
                Module_Letter_Management.CheckVersionPortfolioReport(Link_folder_database)
                Main.Statusbar_item2.Caption = "Importing database CUSTDTL..."
                Module_Letter_Management.Check_Version_CUSTDTL_FOB_Report(Link_folder_database)
            End If
        End If

        Main.Statusbar_item2.Caption = "Select file list Master No for create bill..."
        Dim source_file_list_master As String = SELECT_EXCEL_FILE_RETURNED_FULL_PATH("Please select excel file (.xls) - List MasterNo for create by batch")

        Main.Statusbar_item2.Caption = "[Creating bill]..."

        Main.Statusbar_item2.Caption = "[Creating bill] Create table for new batch"
        Dim table_of_Batch As String = "BATCH_IMPORT_" & Now.ToString("yyMMddhhmmss")
        SQL_QUERY(file_name_Portfolio, "CREATE TABLE " & table_of_Batch & " (Bill_Number VARCHAR, Client_Name VARCHAR, Client_address VARCHAR, Client_attention VARCHAR, Master_No VARCHAR, Document_Note VARCHAR)")


        Main.Statusbar_item2.Caption = "[Creating bill] Importing list master no"
        Dim conn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & source_file_list_master & ";Extended Properties='Excel 8.0;HDR=Yes'")

        dt.Clear()
        conn.Open()
        Dim sql = "SELECT Master_No, Document_Note FROM [Sheet1$] ORDER BY Branch ASC, Master_No ASC"
        Dim cmdDataGrid As OleDbCommand = New OleDbCommand(sql, conn)
        Dim da As New OleDbDataAdapter
        da.SelectCommand = cmdDataGrid
        da.Fill(dt)

        Dim Str_SQL_Add_Record As String = ""
        For Each Drr As DataRow In dt.Rows
            If Drr("Master_No").ToString.Length > 0 Then
                If Drr("Document_Note").ToString.Length > 0 Then
                    Str_SQL_Add_Record = "INSERT INTO " & table_of_Batch & " (Master_No, Document_Note) VALUES ('" & Drr("Master_No").ToString & "', '" & Drr("Document_Note").ToString & "')"
                Else
                    Str_SQL_Add_Record = "INSERT INTO " & table_of_Batch & " (Master_No) VALUES ('" & Drr("Master_No").ToString & "')"
                End If
            End If
            SQL_QUERY(file_name_Portfolio, Str_SQL_Add_Record)
        Next
        conn.Close()

        Dim Bill_Number As String = ""
        Dim Client_Name As String = ""
        Dim Client_address As String = ""
        Dim Client_attention As String = ""

        Dim database_name As String = BarEdit_Database.EditValue.ToString

        Main.Statusbar_item2.Caption = "[Creating bill] Get code for setup bill"

        dt.Clear()
        dt = SQL_QUERY_TO_DATATABLE(file_name_Portfolio, "SELECT * FROM " & table_of_Batch)
        If dt.Rows.Count = 0 Then
            Main.Statusbar_item2.Caption = "Create bill Failed. No master_no in datafile"
            Exit Sub
        End If

        Dim batch_frequency As String = ""
        If batch_begin_of_month = "N" Then
            batch_frequency = "Daily"
        Else
            batch_frequency = "Monthly"
        End If

        Dim DT_LogicBill As DataTable = Module_Letter_Management.SQL_QUERY_TO_DATATABLE(link_application_config, "SELECT Logic_Frequency, Logic_SequenceNo, Logic_Type, Logic_Value_As_Number, Logic_Value_As_Text, Logic_Period_From, Logic_Period_To, Logic_Format FROM LIST_DATABASE WHERE Logic_Frequency = '" & batch_frequency & "' AND Database_Name = '" & database_name & "' ORDER BY Logic_SequenceNo ASC")
        If DT_LogicBill.Rows.Count = 0 Then
            MsgBox("Not yet create logic bill for this database", vbCritical)
            Exit Sub
        End If

        For Each Drr In dt.Rows
            Main.Statusbar_item2.Caption = "[Creating bill] Processing master no " & Drr(4).ToString
            'Get information form table Special List
            Client_Name = SQL_QUERY_TO_STRING(link_database_Special_List, "SELECT Client_name FROM " & table_name_Special & " WHERE Master_No = '" & Drr(4).ToString & "'")
            Client_address = SQL_QUERY_TO_STRING(link_database_Special_List, "SELECT Client_address FROM " & table_name_Special & " WHERE Master_No = '" & Drr(4).ToString & "'")
            Client_attention = SQL_QUERY_TO_STRING(link_database_Special_List, "SELECT Client_Attention FROM " & table_name_Special & " WHERE Master_No = '" & Drr(4).ToString & "'")

            If Client_address.Length = 0 Then
                ' get information from table CUSTDTL - MAI
                Client_Name = SQL_QUERY_TO_STRING(file_name_Portfolio, "SELECT Client_Name FROM Portfolio WHERE MasterNo = '" & Drr(4).ToString & "'")
                Client_address = SQL_QUERY_TO_STRING(file_name_Portfolio, "SELECT Client_Address FROM CUSTDTL WHERE Add_Type_Code = 'MAI' AND Country_Code = 'VN' AND MasterNo = '" & Drr(4).ToString & "'")
                Client_attention = SQL_QUERY_TO_STRING(file_name_Portfolio, "SELECT GROUP_CONCAT(Client_Attention,'; ') AS Client_Attention FROM Portfolio WHERE MasterNo = '" & Drr(4).ToString & "'")
            End If

            If Client_address.Length = 0 Then
                ' get information from table CUSTDTL - LEG
                If SQL_QUERY_TO_INTEGER(file_name_Portfolio, "SELECT COUNT(Client_Address) FROM CUSTDTL WHERE MasterNo = '" & Drr(4).ToString & "' AND [Add_Type_Code]='MAI'") = 0 Then
                    Client_Name = SQL_QUERY_TO_STRING(file_name_Portfolio, "SELECT Client_Name FROM Portfolio WHERE MasterNo = '" & Drr(4).ToString & "'")
                    Client_address = SQL_QUERY_TO_STRING(file_name_Portfolio, "SELECT Client_Address FROM CUSTDTL WHERE Add_Type_Code = 'LEG' AND Country_Code = 'VN' AND MasterNo = '" & Drr(4).ToString & "'")
                    Client_attention = SQL_QUERY_TO_STRING(file_name_Portfolio, "SELECT GROUP_CONCAT(Client_Attention,'; ') AS Client_Attention FROM Portfolio WHERE MasterNo = '" & Drr(4).ToString & "'")
                End If
            End If

            If Client_address.Length = 0 Then
                ' get information from table CUSTDTL - RES
                If SQL_QUERY_TO_INTEGER(file_name_Portfolio, "SELECT COUNT(Client_Address) FROM CUSTDTL WHERE MasterNo = '" & Drr(4).ToString & "' AND [Add_Type_Code]='MAI'") = 0 Then
                    Client_Name = SQL_QUERY_TO_STRING(file_name_Portfolio, "SELECT Client_Name FROM Portfolio WHERE MasterNo = '" & Drr(4).ToString & "'")
                    Client_address = SQL_QUERY_TO_STRING(file_name_Portfolio, "SELECT Client_Address FROM CUSTDTL WHERE Add_Type_Code = 'RES' AND Country_Code = 'VN' AND MasterNo = '" & Drr(4).ToString & "'")
                    Client_attention = SQL_QUERY_TO_STRING(file_name_Portfolio, "SELECT GROUP_CONCAT(Client_Attention,'; ') AS Client_Attention FROM Portfolio WHERE MasterNo = '" & Drr(4).ToString & "'")
                End If
            End If

            If Client_address.Length > 0 Then
                Dim count_bill_tasetco As Integer

                'create bill for tasetco vendor

                count_bill_tasetco = Module_Letter_Management.SQL_QUERY_TO_INTEGER(link_database_tasetco, "SELECT COUNT(Bill_Number) FROM " & table_name_Tasetco & " WHERE [Sent_date]='" & str_Date_Bill.ToString("dd/MM/yyyy") & "' AND [Vendor_name]='TASETCO' AND [Batch_begin_month]='" & batch_begin_of_month & "'")

                Bill_Number = ""

                For dong_logic As Integer = 0 To DT_LogicBill.Rows.Count - 1
                    If DT_LogicBill.Rows(dong_logic).Item(2).ToString = "Date" Then
                        Dim type_date_format As String = DT_LogicBill.Rows(dong_logic).Item(7).ToString
                        Bill_Number = Bill_Number & Now.ToString(type_date_format)
                    End If
                    If DT_LogicBill.Rows(dong_logic).Item(2).ToString = "Text" Then
                        Dim type_text_Value As String = DT_LogicBill.Rows(dong_logic).Item(4).ToString
                        Dim type_text_Format As String = DT_LogicBill.Rows(dong_logic).Item(7).ToString
                        If type_text_Format.Length = 0 Then
                            Bill_Number = Bill_Number & type_text_Value
                        Else
                            Bill_Number = Bill_Number & Format(type_text_Value, type_text_Format)
                        End If
                    End If
                    If DT_LogicBill.Rows(dong_logic).Item(2).ToString = "Numberic (Fixed)" Then
                        Dim type_numberic_fixed_Value As Integer = DT_LogicBill.Rows(dong_logic).Item(3).ToString
                        Bill_Number = Bill_Number & type_numberic_fixed_Value
                    End If
                    If DT_LogicBill.Rows(dong_logic).Item(2).ToString = "Numberic (Period)" Then
                        Dim type_numberic_period_from As Integer = DT_LogicBill.Rows(dong_logic).Item(5).ToString
                        Dim type_numberic_period_to As Integer = DT_LogicBill.Rows(dong_logic).Item(6).ToString
                        Dim type_numberic_format As String = DT_LogicBill.Rows(dong_logic).Item(7).ToString
                        If type_numberic_period_from + count_bill_tasetco > type_numberic_period_to Then
                            MsgBox("New bill Created Failed. Maximumum can create is " & count_bill_tasetco)
                        End If
                        Bill_Number = Bill_Number & Format(type_numberic_period_from + count_bill_tasetco, type_numberic_format)
                    End If
                Next

                Dim str_update As String = "UPDATE " & table_of_Batch &
                                    " SET [Bill_Number] = '" & Bill_Number & "'," &
                                    " [Client_Name] = '" & Client_Name & "'," &
                                    " [Client_address] = '" & Client_address & "'," &
                                    " [Client_attention] = '" & Client_attention & "'" &
                                 " WHERE [Master_No] = '" & Drr(4).ToString & "'"
                Module_Letter_Management.SQL_QUERY(file_name_Portfolio, str_update)

                Dim Str_SQL_Add_Record2 As String = "INSERT INTO " & table_name_Tasetco & " ([Vendor_Name], [Bill_Number], [Sent_date], [Client_Name], [Mailing_address], [Attention], [Master_No], [Document_note], [Batch_begin_month], [User_Created], [Final_Result]) VALUES ('TASETCO', '" & UCase(Bill_Number) & "', '" & str_Date_Bill.ToString("dd/MM/yyyy") & "', '" & UCase(Client_Name) & "', '" & UCase(Client_address) & "', '" & UCase(Client_attention) & "', '" & Drr(4).ToString & "', '" & Drr(5).ToString & "', '" & UCase(batch_begin_of_month) & "', '" & UCase(Environment.UserName) & "_" & Now.ToString("dd/MM/yyyy hh:mm:ss") & "', 'Waiting');"
                Module_Letter_Management.SQL_QUERY(link_database_tasetco, Str_SQL_Add_Record2)

            End If
        Next

        dt.Clear()
        dt = SQL_QUERY_TO_DATATABLE(file_name_Portfolio, "SELECT * FROM " & table_of_Batch & " WHERE [Client_address] IS NULL")
        If dt.Rows.Count > 0 Then

            Dim objExcel As New Excel.Application
            Dim bkWorkBook As Excel.Workbook
            Dim shWorkSheet As Excel.Worksheet

            objExcel = New Excel.Application
            objExcel.Visible = True
            bkWorkBook = objExcel.Workbooks.Add
            shWorkSheet = CType(bkWorkBook.ActiveSheet, Excel.Worksheet)
            shWorkSheet.Name = "NOT_CREATED_BILL"
            shWorkSheet.Cells.NumberFormat = "@"

            Dim i As Long = 0

            For Each Drr As DataRow In dt.Rows
                shWorkSheet.Cells(i + 1, 1) = Drr(0).ToString
                shWorkSheet.Cells(i + 1, 2) = SQL_QUERY_TO_STRING(file_name_Portfolio, "SELECT Client_Name FROM Portfolio WHERE MasterNo = '" & Drr(4).ToString & "'")
                shWorkSheet.Cells(i + 1, 3) = ""
                shWorkSheet.Cells(i + 1, 4) = ""
                shWorkSheet.Cells(i + 1, 5) = Drr(4).ToString
                shWorkSheet.Cells(i + 1, 6) = Drr(5).ToString
                i = i + 1
            Next
        End If

        Main.Statusbar_item2.Caption = "Create Bill by batch: Completed"
        MsgBox("Completed", vbOKOnly)
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub


    '    Private Sub btByBatch_Click(sender As Object, e As EventArgs) Handles btByBatch.Click
    '        On Error GoTo err_handle

    '        If link_database_tasetco.Length = 0 Then
    '            MsgBox("Please select database name and click button [Load Database]!", vbCritical)
    '            Exit Sub
    '        End If

    '        'check database cua nam hien tai da co hay chua
    '        BarEdit_Year.EditValue = CInt(Now.ToString("yyyy"))
    '        BarButton_LoadDatabase.PerformClick()

    '        Dim dt As New DataTable

    '        Dim str_Date_Bill2 As String = InputBox("Please input date for sending letter (Format: dd/MM/yyyy): ", "Vendor Management - Message", Now.ToString("dd/MM/yyyy"))

    '        Dim str_Date_Bill As Date = CDate(str_Date_Bill2.Substring(6, 4) & "-" & str_Date_Bill2.Substring(3, 2) & "-" & str_Date_Bill2.Substring(0, 2))

    '        If Len(str_Date_Bill) = 0 Then Exit Sub

    '        If str_Date_Bill.ToString("dd/MM/yyyy") < Now.ToString("dd/MM/yyyy") Then
    '            MsgBox("Can't create bill for the past", vbCritical, "Vendor Management - Error")
    '            Exit Sub
    '        End If

    '        Dim batch_begin_of_month As String = InputBox("Is this letter for batch begin of month: (Y/N)", "Vendor Management - Message", "N")
    '        batch_begin_of_month = UCase(batch_begin_of_month)
    '        If batch_begin_of_month.Length = 0 Then Exit Sub

    '        Dim file_name_Portfolio As String
    '        If Link_folder_database.Substring(Link_folder_database.Length - 1, 1) = "\" Then
    '            file_name_Portfolio = Link_folder_database & "Database_Portfolio_" & Now.ToString("ddMMyyyy") & ".txt"
    '        Else
    '            file_name_Portfolio = Link_folder_database & "\Database_Portfolio_" & Now.ToString("ddMMyyyy") & ".txt"
    '        End If

    '        If Not My.Computer.FileSystem.FileExists(file_name_Portfolio) Then
    '            Main.Statusbar_item2.Caption = "Importing database Portfolio..."
    '            Module_Letter_Management.CheckVersionPortfolioReport(Link_folder_database)

    '            Main.Statusbar_item2.Caption = "Importing database CUSTDTL_FOB..."
    '            Module_Letter_Management.Check_Version_CUSTDTL_FOB_Report(Link_folder_database)
    '        Else
    '            Main.Statusbar_item2.Caption = "Database Portfolio exists. Checking table CUSTDTL..."
    '            dt = Module_Letter_Management.GET_ALL_TABLE_NAME_IN_DATABASE(file_name_Portfolio)
    '            Dim create_table_CUSTDTL As Boolean = True
    '            Dim create_table_Portfolio As Boolean = True
    '            If dt.Rows.Count > 0 Then
    '                For i As Integer = 0 To dt.Rows.Count - 1
    '                    If dt.Rows(i).Item(0).ToString() = "CUSTDTL" Then
    '                        create_table_CUSTDTL = False
    '                        Exit For
    '                    End If
    '                Next
    '                If create_table_CUSTDTL = True Then
    '                    Main.Statusbar_item2.Caption = "Importing database CUSTDTL..."
    '                    Module_Letter_Management.Check_Version_CUSTDTL_FOB_Report(Link_folder_database)
    '                End If
    '                For i As Integer = 0 To dt.Rows.Count - 1
    '                    If dt.Rows(i).Item(0).ToString() = "Portfolio" Then
    '                        create_table_Portfolio = False
    '                        Exit For
    '                    End If
    '                Next
    '                If create_table_Portfolio = True Then
    '                    Main.Statusbar_item2.Caption = "Importing database Portfolio..."
    '                    Module_Letter_Management.CheckVersionPortfolioReport(Link_folder_database)
    '                End If
    '            Else
    '                Main.Statusbar_item2.Caption = "Database Portfolio not ok. Delete exists Portfolio database file and reimport"
    '                Kill(file_name_Portfolio)
    '                Main.Statusbar_item2.Caption = "Importing database Portfolio..."
    '                Module_Letter_Management.CheckVersionPortfolioReport(Link_folder_database)
    '                Main.Statusbar_item2.Caption = "Importing database CUSTDTL..."
    '                Module_Letter_Management.Check_Version_CUSTDTL_FOB_Report(Link_folder_database)
    '            End If
    '        End If

    '        Main.Statusbar_item2.Caption = "Select file list Master No for create bill..."
    '        Dim source_file_list_master As String = SELECT_EXCEL_FILE_RETURNED_FULL_PATH("Please select excel file (.xls) - List MasterNo for create by batch")

    '        Main.Statusbar_item2.Caption = "[Creating bill]..."

    '        Main.Statusbar_item2.Caption = "[Creating bill] Create table for new batch"
    '        Dim table_of_Batch As String = "BATCH_IMPORT_" & Now.ToString("yyMMddhhmmss")
    '        SQL_QUERY(file_name_Portfolio, "CREATE TABLE " & table_of_Batch & " (Bill_Number VARCHAR, Client_Name VARCHAR, Client_address VARCHAR, Client_attention VARCHAR, Master_No VARCHAR, Document_Note VARCHAR, Branch VARCHAR)")


    '        Main.Statusbar_item2.Caption = "[Creating bill] Importing list master no"
    '        Dim conn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & source_file_list_master & ";Extended Properties='Excel 8.0;HDR=Yes'")

    '        dt.Clear()
    '        conn.Open()
    '        Dim sql = "SELECT Master_No, Document_Note, Branch FROM [Sheet1$] ORDER BY Branch ASC, Master_No ASC"
    '        Dim cmdDataGrid As OleDbCommand = New OleDbCommand(sql, conn)
    '        Dim da As New OleDbDataAdapter
    '        da.SelectCommand = cmdDataGrid
    '        da.Fill(dt)

    '        Dim Str_SQL_Add_Record As String = ""
    '        For Each Drr As DataRow In dt.Rows
    '            If Drr("Master_No").ToString.Length > 0 Then
    '                If Drr("Document_Note").ToString.Length > 0 Then
    '                    If Drr("Branch").ToString.Length > 0 Then
    '                        Str_SQL_Add_Record = "INSERT INTO " & table_of_Batch & " (Master_No, Document_Note, Branch) VALUES ('" & Drr("Master_No").ToString & "', '" & Drr("Document_Note").ToString & "', '" & Drr("Branch").ToString & "')"
    '                    Else
    '                        Str_SQL_Add_Record = "INSERT INTO " & table_of_Batch & " (Master_No, Document_Note) VALUES ('" & Drr("Master_No").ToString & "', '" & Drr("Document_Note").ToString & "')"
    '                    End If
    '                Else
    '                    Str_SQL_Add_Record = "INSERT INTO " & table_of_Batch & " (Master_No) VALUES ('" & Drr("Master_No").ToString & "')"
    '                End If
    '            End If
    '            SQL_QUERY(file_name_Portfolio, Str_SQL_Add_Record)
    '        Next
    '        conn.Close()

    '        Dim Bill_Number As String = ""
    '        Dim Client_Name As String = ""
    '        Dim Client_address As String = ""
    '        Dim Client_attention As String = ""

    '        Dim database_name As String = BarEdit_Database.EditValue.ToString

    '        Main.Statusbar_item2.Caption = "[Creating bill] Get code for setup bill"

    '        dt.Clear()
    '        dt = SQL_QUERY_TO_DATATABLE(file_name_Portfolio, "SELECT * FROM " & table_of_Batch)
    '        If dt.Rows.Count = 0 Then
    '            Main.Statusbar_item2.Caption = "Create bill Failed. No master_no in datafile"
    '            Exit Sub
    '        End If

    '        Dim table_batch_special_list As String = table_of_Batch & "_Special"
    '        SQL_QUERY(file_name_Portfolio, "CREATE TABLE " & table_batch_special_list & " (Bill_Number VARCHAR, Client_Name VARCHAR, Client_address VARCHAR, Client_attention VARCHAR, Master_No VARCHAR, Document_Note VARCHAR, Branch VARCHAR)")
    '        SQLITE_BULK_COPY(SQL_QUERY_TO_DATATABLE(Link_database_Portfolio, "SELECT " & table_name_Special & ".MasterNo AS MasterNo, " & table_name_Special & ".Client_Name AS Client_Name, " & table_name_Special & ".Client_Address AS Client_Address, " & table_name_Special & ".Attention AS Client_Attention FROM " & table_name_Special & " INNER JOIN " & table_of_Batch & " On " & table_of_Batch & ".MasterNo=" & table_name_Special & ".MasterNo"), Link_database_Portfolio, table_batch_special_list)

    '        Dim table_batch_special_list As String = table_of_Batch & "_Special"
    '        SQL_QUERY(file_name_Portfolio, "CREATE TABLE " & table_batch_special_list & " (Bill_Number VARCHAR, Client_Name VARCHAR, Client_address VARCHAR, Client_attention VARCHAR, Master_No VARCHAR, Document_Note VARCHAR, Branch VARCHAR)")
    '        SQLITE_BULK_COPY(SQL_QUERY_TO_DATATABLE(Link_database_Portfolio, "SELECT " & table_name_Special & ".MasterNo AS MasterNo, " & table_name_Special & ".Client_Name AS Client_Name, " & table_name_Special & ".Client_Address AS Client_Address, " & table_name_Special & ".Attention AS Client_Attention FROM " & table_name_Special & " INNER JOIN " & table_of_Batch & " On " & table_of_Batch & ".MasterNo=" & table_name_Special & ".MasterNo"), Link_database_Portfolio, table_batch_special_list)








    '        'Dim batch_frequency As String = ""
    '        'If batch_begin_of_month = "N" Then
    '        '    batch_frequency = "Daily"
    '        'Else
    '        '    batch_frequency = "Monthly"
    '        'End If

    '        'Dim DT_LogicBill As DataTable = SQL_QUERY_TO_DATATABLE(link_application_config, "SELECT Logic_Frequency, Logic_SequenceNo, Logic_Type, Logic_Value_As_Number, Logic_Value_As_Text, Logic_Period_From, Logic_Period_To, Logic_Format FROM LIST_DATABASE WHERE Logic_Frequency = '" & batch_frequency & "' AND Database_Name = '" & database_name & "' ORDER BY Logic_SequenceNo ASC")
    '        'If DT_LogicBill.Rows.Count = 0 Then
    '        '    MsgBox("Not yet create logic bill for this database", vbCritical)
    '        '    Exit Sub
    '        'End If

    '        'For Each Drr In dt.Rows
    '        '    Main.Statusbar_item2.Caption = "[Creating bill] Processing master no " & Drr(4).ToString
    '        '    'Get information form table Special List
    '        '    Client_Name = SQL_QUERY_TO_STRING(link_database_Special_List, "Select Client_name FROM " & table_name_Special & " WHERE Master_No = '" & Drr(4).ToString & "'")
    '        '    Client_address = SQL_QUERY_TO_STRING(link_database_Special_List, "SELECT Client_address FROM " & table_name_Special & " WHERE Master_No = '" & Drr(4).ToString & "'")
    '        '    Client_attention = SQL_QUERY_TO_STRING(link_database_Special_List, "SELECT Client_Attention FROM " & table_name_Special & " WHERE Master_No = '" & Drr(4).ToString & "'")

    '        '    If Client_address.Length = 0 Then
    '        '        ' get information from table CUSTDTL - MAI
    '        '        Client_Name = SQL_QUERY_TO_STRING(file_name_Portfolio, "SELECT Client_Name FROM Portfolio WHERE MasterNo = '" & Drr(4).ToString & "'")
    '        '        Client_address = SQL_QUERY_TO_STRING(file_name_Portfolio, "SELECT Client_Address FROM CUSTDTL WHERE Add_Type_Code = 'MAI' AND Country_Code = 'VN' AND MasterNo = '" & Drr(4).ToString & "'")
    '        '        Client_attention = SQL_QUERY_TO_STRING(file_name_Portfolio, "SELECT GROUP_CONCAT(Client_Attention,'; ') AS Client_Attention FROM Portfolio WHERE MasterNo = '" & Drr(4).ToString & "'")
    '        '    End If

    '        '    If Client_address.Length = 0 Then
    '        '        ' get information from table CUSTDTL - LEG
    '        '        If SQL_QUERY_TO_INTEGER(file_name_Portfolio, "SELECT COUNT(Client_Address) FROM CUSTDTL WHERE MasterNo = '" & Drr(4).ToString & "' AND [Add_Type_Code]='MAI'") = 0 Then
    '        '            Client_Name = SQL_QUERY_TO_STRING(file_name_Portfolio, "SELECT Client_Name FROM Portfolio WHERE MasterNo = '" & Drr(4).ToString & "'")
    '        '            Client_address = SQL_QUERY_TO_STRING(file_name_Portfolio, "SELECT Client_Address FROM CUSTDTL WHERE Add_Type_Code = 'LEG' AND Country_Code = 'VN' AND MasterNo = '" & Drr(4).ToString & "'")
    '        '            Client_attention = SQL_QUERY_TO_STRING(file_name_Portfolio, "SELECT GROUP_CONCAT(Client_Attention,'; ') AS Client_Attention FROM Portfolio WHERE MasterNo = '" & Drr(4).ToString & "'")
    '        '        End If
    '        '    End If

    '        '    If Client_address.Length = 0 Then
    '        '        ' get information from table CUSTDTL - RES
    '        '        If SQL_QUERY_TO_INTEGER(file_name_Portfolio, "SELECT COUNT(Client_Address) FROM CUSTDTL WHERE MasterNo = '" & Drr(4).ToString & "' AND [Add_Type_Code]='MAI'") = 0 Then
    '        '            Client_Name = SQL_QUERY_TO_STRING(file_name_Portfolio, "SELECT Client_Name FROM Portfolio WHERE MasterNo = '" & Drr(4).ToString & "'")
    '        '            Client_address = SQL_QUERY_TO_STRING(file_name_Portfolio, "SELECT Client_Address FROM CUSTDTL WHERE Add_Type_Code = 'RES' AND Country_Code = 'VN' AND MasterNo = '" & Drr(4).ToString & "'")
    '        '            Client_attention = SQL_QUERY_TO_STRING(file_name_Portfolio, "SELECT GROUP_CONCAT(Client_Attention,'; ') AS Client_Attention FROM Portfolio WHERE MasterNo = '" & Drr(4).ToString & "'")
    '        '        End If
    '        '    End If

    '        '    If Client_address.Length > 0 Then
    '        '        Dim count_bill_tasetco As Integer

    '        '        'create bill for tasetco vendor

    '        '        count_bill_tasetco = Module_Letter_Management.SQL_QUERY_TO_INTEGER(link_database_tasetco, "SELECT COUNT(Bill_Number) FROM " & table_name_Tasetco & " WHERE [Sent_date]='" & Now.ToString("dd/MM/yyyy") & "' AND [Vendor_name]='TASETCO' AND [Batch_begin_month]='" & batch_begin_of_month & "'")

    '        '        Bill_Number = ""

    '        '        For dong_logic As Integer = 0 To DT_LogicBill.Rows.Count - 1
    '        '            If DT_LogicBill.Rows(dong_logic).Item(2).ToString = "Date" Then
    '        '                Dim type_date_format As String = DT_LogicBill.Rows(dong_logic).Item(7).ToString
    '        '                Bill_Number = Bill_Number & Now.ToString(type_date_format)
    '        '            End If
    '        '            If DT_LogicBill.Rows(dong_logic).Item(2).ToString = "Text" Then
    '        '                Dim type_text_Value As String = DT_LogicBill.Rows(dong_logic).Item(4).ToString
    '        '                Dim type_text_Format As String = DT_LogicBill.Rows(dong_logic).Item(7).ToString
    '        '                If type_text_Format.Length = 0 Then
    '        '                    Bill_Number = Bill_Number & type_text_Value
    '        '                Else
    '        '                    Bill_Number = Bill_Number & Format(type_text_Value, type_text_Format)
    '        '                End If
    '        '            End If
    '        '            If DT_LogicBill.Rows(dong_logic).Item(2).ToString = "Numberic (Fixed)" Then
    '        '                Dim type_numberic_fixed_Value As Integer = DT_LogicBill.Rows(dong_logic).Item(3).ToString
    '        '                Bill_Number = Bill_Number & type_numberic_fixed_Value
    '        '            End If
    '        '            If DT_LogicBill.Rows(dong_logic).Item(2).ToString = "Numberic (Period)" Then
    '        '                Dim type_numberic_period_from As Integer = DT_LogicBill.Rows(dong_logic).Item(5).ToString
    '        '                Dim type_numberic_period_to As Integer = DT_LogicBill.Rows(dong_logic).Item(6).ToString
    '        '                Dim type_numberic_format As String = DT_LogicBill.Rows(dong_logic).Item(7).ToString
    '        '                If type_numberic_period_from + count_bill_tasetco > type_numberic_period_to Then
    '        '                    MsgBox("New bill Created Failed. Maximumum can create is " & count_bill_tasetco)
    '        '                End If
    '        '                Bill_Number = Bill_Number & Format(type_numberic_period_from + count_bill_tasetco, type_numberic_format)
    '        '            End If
    '        '        Next

    '        '        Dim str_update As String = "UPDATE " & table_of_Batch &
    '        '                            " SET [Bill_Number] = '" & Bill_Number & "'," &
    '        '                            " [Client_Name] = '" & Client_Name & "'," &
    '        '                            " [Client_address] = '" & Client_address & "'," &
    '        '                            " [Client_attention] = '" & Client_attention & "'" &
    '        '                         " WHERE [Master_No] = '" & Drr(4).ToString & "'"
    '        '        Module_Letter_Management.SQL_QUERY(file_name_Portfolio, str_update)

    '        '        Dim Str_SQL_Add_Record2 As String = "INSERT INTO " & table_name_Tasetco & " ([Vendor_Name], [Bill_Number], [Sent_date], [Client_Name], [Mailing_address], [Attention], [Master_No], [Document_note], [Batch_begin_month], [User_Created], [Final_Result]) VALUES ('TASETCO', '" & UCase(Bill_Number) & "', '" & str_Date_Bill.ToString("dd/MM/yyyy") & "', '" & UCase(Client_Name) & "', '" & UCase(Client_address) & "', '" & UCase(Client_attention) & "', '" & Drr(4).ToString & "', '" & Drr(5).ToString & "', '" & UCase(batch_begin_of_month) & "', '" & UCase(Environment.UserName) & "_" & Now.ToString("dd/MM/yyyy hh:mm:ss") & "', 'Incomplete');"
    '        '        Module_Letter_Management.SQL_QUERY(link_database_tasetco, Str_SQL_Add_Record2)

    '        '    End If
    '        'Next



    '        dt.Clear()
    '        dt = SQL_QUERY_TO_DATATABLE(file_name_Portfolio, "SELECT * FROM " & table_of_Batch & " WHERE [Client_address] IS NULL")
    '        If dt.Rows.Count > 0 Then

    '            Dim objExcel As New Excel.Application
    '            Dim bkWorkBook As Excel.Workbook
    '            Dim shWorkSheet As Excel.Worksheet

    '            objExcel = New Excel.Application
    '            objExcel.Visible = True
    '            bkWorkBook = objExcel.Workbooks.Add
    '            shWorkSheet = CType(bkWorkBook.ActiveSheet, Excel.Worksheet)
    '            shWorkSheet.Name = "NOT_CREATED_BILL"
    '            shWorkSheet.Cells.NumberFormat = "@"

    '            Dim i As Long = 0

    '            For Each Drr As DataRow In dt.Rows
    '                shWorkSheet.Cells(i + 1, 1) = Drr(0).ToString
    '                shWorkSheet.Cells(i + 1, 2) = SQL_QUERY_TO_STRING(file_name_Portfolio, "SELECT Client_Name FROM Portfolio WHERE MasterNo = '" & Drr(4).ToString & "'")
    '                shWorkSheet.Cells(i + 1, 3) = ""
    '                shWorkSheet.Cells(i + 1, 4) = ""
    '                shWorkSheet.Cells(i + 1, 5) = Drr(4).ToString
    '                shWorkSheet.Cells(i + 1, 6) = Drr(5).ToString
    '                i = i + 1
    '            Next
    '        End If

    '        Main.Statusbar_item2.Caption = "Create Bill by batch: Completed"
    '        MsgBox("Completed", vbOKOnly)
    '        Exit Sub
    'err_handle:
    '        Module_Letter_Management.Error_handle()
    '    End Sub

    Private Sub btPrintBill_Click(sender As Object, e As EventArgs) Handles btPrintBill.Click
        If GridView1.RowCount = 0 Then Exit Sub

        If link_database_tasetco.Length = 0 Then
            MsgBox("Please select database name and click button [Load Database]!", vbCritical)
            Exit Sub
        End If

        Dim iRet = MsgBox("Please choose page size for create bill: " & Chr(10) & Chr(10) & "YES = Page A5" & Chr(10) & "NO = Page A4", vbYesNoCancel + vbQuestion)

        If iRet = vbYes Then
            Module_Letter_Management.Mail_Merge("TASETCO", link_database_tasetco, table_name_Tasetco, BarEdit_Database.EditValue.ToString, GridView1, appPath & "Template\template_print_bill_Tasetco_A5.docx")
        Else
            If iRet = vbNo Then
                Module_Letter_Management.Mail_Merge("TASETCO", link_database_tasetco, table_name_Tasetco, BarEdit_Database.EditValue.ToString, GridView1, appPath & "Template\template_print_bill_Tasetco_A4.docx")
            Else
                If iRet = vbCancel Then Exit Sub
            End If
        End If
    End Sub

    Private Sub cbAutoCreateBill_CheckedChanged_1(sender As Object, e As EventArgs) Handles cbAutoCreateBill.CheckedChanged
        If cbAutoCreateBill.Checked = True Then
            tbBillNo.ReadOnly = True
            tbSentDate.Enabled = False
        Else
            tbBillNo.ReadOnly = False
            tbSentDate.Enabled = True
        End If
    End Sub

    Private Sub rbtOtherVendor_CheckedChanged(sender As Object, e As EventArgs) Handles rbtOtherVendor.CheckedChanged
        If rbtOtherVendor.Checked = True Then
            cbAutoCreateBill.Checked = False
        End If
    End Sub

    Private Sub rbtTasetco_CheckedChanged(sender As Object, e As EventArgs) Handles rbtTasetco.CheckedChanged
        If rbtTasetco.Checked = True Then
            cbAutoCreateBill.Checked = True
        End If
    End Sub

    Private Sub tbStrSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles tbStrSearch.KeyDown
        On Error GoTo err_handle

        If e.KeyCode = System.Windows.Forms.Keys.Return Then
            If tbStrSearch.Text.Length = 0 Then Exit Sub

            If Module_Letter_Management.SEARCH_DATABASE_BY_STRING_RETURN_BOOLEAN(link_database_Special_List, table_name_Special, tbStrSearch.Text) = True Then
                Dim iRet = MsgBox("Do you want to view the special address?", vbYesNo + vbQuestion)
                If iRet = vbYes Then
                    Dim DT As New DataTable
                    DT = Module_Letter_Management.SQL_QUERY_TO_DATATABLE(link_database_Special_List, "SELECT * FROM " & table_name_Special & " WHERE [Master_No] LIKE '%" & tbStrSearch.Text & "%'")
                    GridControl_Tasetco.DataSource = DT
                Else
                    LOAD_DATABASE_TASETCO_TO_GRIDVIEW2(link_database_tasetco, table_name_Tasetco, "", tbStrSearch.Text, GridControl_Tasetco, GridView1)
                End If
            Else
                LOAD_DATABASE_TASETCO_TO_GRIDVIEW2(link_database_tasetco, table_name_Tasetco, "", tbStrSearch.Text, GridControl_Tasetco, GridView1)
            End If
        End If
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub btUpdateByBatch_Click(sender As Object, e As EventArgs) Handles btUpdateByBatch.Click
        On Error GoTo err_handle

        If link_database_tasetco.Length = 0 Then
            MsgBox("Please select database name and click button [Load Database]!", vbCritical)
            Exit Sub
        End If
        Dim iRet = MsgBox("It's only update by batch for fields: STATUS; REMARK; FINAL_RESULT; CROWN_BARCODE; CROWN_Sent_Date" & Chr(10) & "Do you want to continue?", vbOKCancel)

        If iRet = vbOK Then
            Dim source_file_list_master As String = SELECT_EXCEL_FILE_RETURNED_FULL_PATH("Please select excel file (.xls)")

            Dim conn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & source_file_list_master & ";Extended Properties='Excel 8.0;HDR=Yes'")
            conn.Open()
            Dim sql = "SELECT Bill_Number, ACS_Status, Remark, Final_Result, CROWN_Barcode, CROWN_Sent_Date FROM [Sheet1$]"
            Dim cmdDataGrid As OleDbCommand = New OleDbCommand(sql, conn)
            Dim da As New OleDbDataAdapter
            da.SelectCommand = cmdDataGrid
            Dim DT As New DataTable
            da.Fill(DT)

            Dim Str_SQL_Add_Record As String = ""
            For Each Drr As DataRow In DT.Rows
                If Drr("Bill_Number").ToString.Length > 0 Then
                    Dim str_SET_UPDATE As String = ""
                    If Drr("ACS_Status").ToString.Length > 0 Then
                        str_SET_UPDATE = str_SET_UPDATE & " [ACS_Status] = '" & Drr("ACS_Status").ToString & "',"
                    Else
                        str_SET_UPDATE = str_SET_UPDATE & " [ACS_Status] = '',"
                    End If
                    If Drr("Remark").ToString.Length > 0 Then
                        str_SET_UPDATE = str_SET_UPDATE & " [Remark] = '" & Drr("Remark").ToString & "',"
                    Else
                        str_SET_UPDATE = str_SET_UPDATE & " [Remark] = '',"
                    End If
                    If Drr("Final_Result").ToString.Length > 0 Then
                        If Drr("Final_Result").ToString = "Completed" Then
                            str_SET_UPDATE = str_SET_UPDATE & " [Final_Result] = 'Completed',"
                        Else
                            If Drr("Final_Result").ToString = "CROWN" Then
                                str_SET_UPDATE = str_SET_UPDATE & " [Final_Result] = 'CROWN',"
                            Else
                                str_SET_UPDATE = str_SET_UPDATE & " [Final_Result] = 'Incomplete',"
                            End If
                        End If
                    End If
                    If Drr("CROWN_Barcode").ToString.Length > 0 Then
                        str_SET_UPDATE = str_SET_UPDATE & " [CROWN_Barcode] = '" & Drr("CROWN_Barcode").ToString & "',"
                    Else
                        str_SET_UPDATE = str_SET_UPDATE & " [CROWN_Barcode] = '',"
                    End If
                    If Drr("CROWN_Sent_Date").ToString.Length > 0 Then
                        str_SET_UPDATE = str_SET_UPDATE & " [CROWN_Sent_Date] = '" & Drr("CROWN_Sent_Date").ToString & "',"
                    Else
                        str_SET_UPDATE = str_SET_UPDATE & " [CROWN_Sent_Date] = '',"
                    End If
                    If str_SET_UPDATE <> "" Then
                        str_SET_UPDATE = str_SET_UPDATE & " [User_Modified] = '" & Environment.UserName & "_" & Now.ToString("ddMMyyyy hh:mm:ss") & "'"
                        Dim str_update As String = ""
                        str_update = "UPDATE " & table_name_Tasetco & " SET" & str_SET_UPDATE & " WHERE [Bill_Number] = '" & Drr("Bill_Number").ToString & "'"
                        SQL_QUERY(link_database_tasetco, str_update)
                    End If
                End If
            Next
            conn.Close()
            MsgBox("Completed!", vbInformation)
        End If
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub GridView1_KeyDown(sender As Object, e As KeyEventArgs) Handles GridView1.KeyDown
        Dim view As GridView = TryCast(sender, GridView)
        If e.KeyData = Keys.Delete Then
            view.DeleteSelectedRows()
            e.Handled = True
        End If
    End Sub

    Private Sub GridView1_CustomDrawCell(sender As Object, e As RowCellCustomDrawEventArgs) Handles GridView1.CustomDrawCell
        Dim view As GridView = CType(sender, GridView)
        If Not view.OptionsView.ShowAutoFilterRow OrElse Not view.IsDataRow(e.RowHandle) Then Return
        Dim filterCellText As String = view.GetRowCellDisplayText(GridControl.AutoFilterRowHandle, e.Column)
        If String.IsNullOrEmpty(filterCellText) Then Return
        Dim filterTextIndex As Integer = e.DisplayText.IndexOf(filterCellText, StringComparison.CurrentCultureIgnoreCase)
        If filterTextIndex = -1 Then Return
        XPaint.Graphics.DrawMultiColorString(e.Cache, e.Bounds, e.DisplayText, filterCellText, e.Appearance, Color.Black, Color.Gold, False, filterTextIndex)
        e.Handled = True
    End Sub

    Private Sub btTasetco_LoadImage_Click(sender As Object, e As EventArgs) Handles btTasetco_LoadImage.Click
        On Error GoTo err_handle

        Dim link_screenshot As String = Link_folder_database & "Screenshot\" & tbBillNo.Text & ".jpg"

        If Not My.Computer.FileSystem.FileExists(link_screenshot) Then
            MsgBox("Not found" & Chr(10) & Chr(10) & link_screenshot, vbCritical + vbOKOnly, "Vendor Management - Not found")
            Exit Sub
        End If

        PictureBox_TasetcoDetails.Image = GetImageFromFile(link_screenshot)

        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Public Function GetImageFromFile(path As String) As Image
        Dim fs As New FileStream(path, FileMode.Open, FileAccess.Read)
        Dim img = Image.FromStream(fs)
        Return img
    End Function

    Private Sub btTasetco_CopyToClipboard_Click(sender As Object, e As EventArgs) Handles btTasetco_CopyToClipboard.Click
        On Error GoTo err_handle

        Dim copy_text As Boolean = False
        Dim link_screeshot As String = Link_folder_database & "Screenshot\" & tbBillNo.Text & ".jpg"

        If Not My.Computer.FileSystem.FileExists(link_screeshot) Then
            copy_text = True
        Else
            Dim iRet = MsgBox("Do you want to copy screenshot?", vbYesNo + vbQuestion)
            If iRet = vbYes Then
                Clipboard.SetImage(Image.FromFile(link_screeshot))
                Main.Statusbar_item2.Caption = "Copied screenshot to clipboard"
            Else
                copy_text = True
            End If
        End If

        If copy_text = True Then
            Dim str_clipboard As String = ""
            str_clipboard = "Bill Number: " & tbBillNo.Text & Chr(10) &
                            "Name: " & tbClientName.Text & Chr(10) &
                            "Mailing Address: " & tbAddress.Text & Chr(10) &
                            "Attention: " & tbAttention.Text & Chr(10) &
                            "Document note: " & tbNote.Text & Chr(10) &
                            "-----------------------" & Chr(10) &
                            "Status: " & tbTasetco_Status.Text & Chr(10) &
                            "Recipient: " & tbTasetco_Recipient.Text & Chr(10) &
                            "Date Received: " & tbTasetco_DateReceived.Text & Chr(10)
            Clipboard.SetText(str_clipboard)
            Main.Statusbar_item2.Caption = "Copied text to clipboard"
        End If

        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub btDeletePortfolio_Click(sender As Object, e As EventArgs) Handles btDeletePortfolio.Click
        Try
            Dim iret = MsgBox("Do you want to delete Portfolio Database?", vbYesNo + vbQuestion)
            If iret = vbYes Then
                If My.Computer.FileSystem.FileExists(Link_database_Portfolio) Then
                    Kill(Link_database_Portfolio)
                End If
                WriteLog_Full("[DELETE FILE PORTFOLIO] - Link database: " & Link_database_Portfolio & vbLf & " - Result: Completed")
                MsgBox("Completed", vbInformation)
            End If
        Catch ex As Exception
            WriteErrorLog("[DELETE FILE PORTFOLIO] - Link database: " & Link_database_Portfolio & vbLf & "- Error Message: " & ex.ToString)
            MsgBox(ex.Message, vbCritical)
        End Try
    End Sub
End Class
