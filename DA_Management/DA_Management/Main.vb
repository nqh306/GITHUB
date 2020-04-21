Imports System.IO
Imports DevExpress.XtraBars
Imports System.Data.SQLite
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraEditors
Imports System.Threading
Imports System.Reflection

Public Class Main
    Public appPath As String = AppDomain.CurrentDomain.BaseDirectory
    Public link_app_config As String
    Public link_global_config As String
    Public link_database_user As String


    Public folder_path As String = ""
    Public folder_path_save_file As String = ""
    Public link_database_DA As String = ""

    Sub refresh_link()
        'On Error GoTo err_handle

        link_app_config = appPath & "App_Config.txt"

        If Not My.Computer.FileSystem.FileExists(link_app_config) Then
            SQLiteConnection.CreateFile(link_app_config)
        End If

        SQL_QUERY(link_app_config, "CREATE TABLE IF NOT EXISTS Config(Field_Name VARCHAR NOT NULL UNIQUE PRIMARY KEY,Field_Value VARCHAR)")

        If SQL_QUERY_TO_INTEGER(link_app_config, "SELECT COUNT(Field_Value) FROM Config WHERE [Field_Name] = 'Link_Global_Config'") = 0 Then
            SQL_QUERY(link_app_config, "INSERT INTO Config([Field_Name], [Field_Value]) VALUES ('Link_Global_Config','C:\SSetup_w7\Application_Config.txt');")
        End If

        link_global_config = SQL_QUERY_TO_STRING(link_app_config, "SELECT Field_Value FROM Config WHERE Field_Name = 'Link_Global_Config'")
        If Not My.Computer.FileSystem.FileExists(link_global_config) Then
            SQLiteConnection.CreateFile(link_global_config)
        End If
        SQL_QUERY(link_global_config, "CREATE TABLE IF NOT EXISTS Config(Field_Name VARCHAR NOT NULL UNIQUE PRIMARY KEY,Field_value1 VARCHAR)")
        If SQL_QUERY_TO_INTEGER(link_global_config, "SELECT COUNT(Field_value1) FROM Config WHERE [Field_Name] = 'Link_Folder_Path_DA'") = 0 Then
            SQL_QUERY(link_global_config, "INSERT INTO Config([Field_Name], [Field_value1]) VALUES ('Link_Folder_Path_DA','C:\SSetup_w7\');")
        End If

        If SQL_QUERY_TO_INTEGER(link_app_config, "SELECT COUNT(Field_Value) FROM Config WHERE [Field_Name] = 'Link_Database_User'") = 0 Then
            SQL_QUERY(link_app_config, "INSERT INTO Config([Field_Name], [Field_Value]) VALUES ('Link_Database_User','C:\SSetup_w7\Application_Config.txt');")
        End If

        If SQL_QUERY_TO_INTEGER(link_app_config, "SELECT COUNT(Field_Value) FROM Config WHERE [Field_Name] = 'Current_Version'") = 0 Then
            SQL_QUERY(link_app_config, "INSERT INTO Config([Field_Name], [Field_Value]) VALUES ('Current_Version','" & Assembly.GetExecutingAssembly().GetName().Version.ToString() & "');")
        Else
            SQL_QUERY(link_app_config, "UPDATE Config SET [Field_Value] = '" & Assembly.GetExecutingAssembly().GetName().Version.ToString() & "' WHERE [Field_Name] = 'Current_Version'")
        End If

        link_database_user = SQL_QUERY_TO_STRING(link_app_config, "SELECT Field_Value FROM Config WHERE Field_Name = 'Link_Database_User'")

        If Not My.Computer.FileSystem.FileExists(link_database_user) Then
            SQLiteConnection.CreateFile(link_database_user)
        End If

        SQL_QUERY(link_database_user, "CREATE TABLE IF NOT EXISTS LIST_USER(Bank_ID VARCHAR NOT NULL UNIQUE PRIMARY KEY,Function_Add BOOLEAN,Function_Edit BOOLEAN,Function_Set_Expired BOOLEAN,Function_User BOOLEAN,Status BOOLEAN,Password_Str VARCHAR)")

        If SQL_QUERY_TO_INTEGER(link_database_user, "SELECT COUNT(Password_Str) FROM LIST_USER WHERE [Bank_ID] = 'Admin'") = 0 Then
            SQL_QUERY(link_database_user, "INSERT INTO LIST_USER([Bank_ID], [Function_User], [Status], [Password_Str]) VALUES ('Admin','1','1','hunLfrCJiXGy7AU0Dn60EQ==');")
        End If

        If SQL_QUERY_TO_INTEGER(link_app_config, "SELECT COUNT(Field_Value) FROM Config WHERE [Field_Name] = 'Link_Folder_Path_DA'") = 0 Then
            SQL_QUERY(link_app_config, "INSERT INTO Config([Field_Name], [Field_Value]) VALUES ('Link_Folder_Path_DA','C:\SSetup_w7');")
        End If

        folder_path = SQL_QUERY_TO_STRING(link_global_config, "SELECT Field_value1 FROM Config WHERE Field_Name = 'Link_Folder_Path_DA'")
        If folder_path.Substring(folder_path.Length - 1, 1) <> "\" Then
            folder_path = folder_path & "\"
        End If

        SQL_QUERY(link_app_config, "UPDATE Config SET [Field_Value] = '" & folder_path & "' WHERE [Field_Name] = 'Link_Folder_Path_DA'")

        folder_path_save_file = folder_path & "ARCHIVE"
        link_database_DA = folder_path & "Database_DA.txt"

        If Not My.Computer.FileSystem.FileExists(link_database_DA) Then
            SQLiteConnection.CreateFile(link_database_DA)
            SQL_QUERY(link_database_DA, "CREATE TABLE IF NOT EXISTS Main_DA(Department VARCHAR,DA_To VARCHAR,PWID_DA_To VARCHAR,Position_To VARCHAR,Delegation VARCHAR,Effective_Date VARCHAR,Expiry_Date VARCHAR,Status VARCHAR,Note VARCHAR,DA_From VARCHAR,PWID_DA_From VARCHAR,Position_From VARCHAR,File_Name VARCHAR,User_Created VARCHAR,User_Modified VARCHAR,Segment VARCHAR,CaseID VARCHAR NOT NULL UNIQUE PRIMARY KEY,Deleted_Date VARCHAR)")
        End If

        backup_database()

        Exit Sub
err_handle:
        Module_DA.Error_handle()
    End Sub
    Private Sub Main2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.WindowState = FormWindowState.Maximized
        SplitContainerControl1.SplitterPosition = SplitContainerControl1.Width

        lbCaseID.Text = ""
        refresh_link()
        'update_expiry_DA(link_database_DA)

        'config GridView
        GridView1.OptionsBehavior.Editable = False
        WindowsFormsSettings.AllowAutoFilterConditionChange = DevExpress.Utils.DefaultBoolean.False

        BarStaticItem_User.Caption = Environment.UserName

        Check_user_access()

        tbEffectiveDate.Select()
        Auto_Set_Expired()
    End Sub

    Sub Check_user_access()

        SQL_QUERY(link_database_user, "CREATE TABLE IF NOT EXISTS LIST_USER(Bank_ID VARCHAR NOT NULL UNIQUE PRIMARY KEY,Function_Add BOOLEAN,Function_Edit BOOLEAN,Function_Set_Expired BOOLEAN,Function_User BOOLEAN,Status BOOLEAN,Password_Str VARCHAR)")
        If CHECK_COLUMN_EXISTS(link_database_user, "LIST_USER", "Function_DataMaintenance") = False Then
            SQL_QUERY(link_database_user, "ALTER TABLE LIST_USER ADD COLUMN Function_DataMaintenance BOOLEAN")
        End If

        Dim form1 As New Add_Item
        Dim form2 As New Config
        Dim form3 As New Form_Edit
        Dim form4 As New FormUser
        Dim form5 As New FormLogin
        Dim form6 As New FormDataMaintenance

        Dim frmCollection = System.Windows.Forms.Application.OpenForms
        For i As Int16 = 0I To frmCollection.Count - 1I
            If frmCollection.Item(i).Name = form1.Name Or frmCollection.Item(i).Name = form2.Name Or frmCollection.Item(i).Name = form3.Name Or frmCollection.Item(i).Name = form4.Name Or frmCollection.Item(i).Name = form5.Name Or frmCollection.Item(i).Name = form6.Name Then
                frmCollection.Item(i).Close()
            End If
        Next i

        Dim user_name As String = BarStaticItem_User.Caption

        BarButtonItem_AddNewRecord.Enabled = False
        BarButtonItem_Edit.Enabled = False
        BarButtonItem_User.Enabled = False
        btSetExpired.Enabled = False
        BarButtonItem_Config.Enabled = False
        BarButtonItem_DataMaintenance.Enabled = False

        Dim function_add As Boolean = SQL_QUERY_TO_BOOLEAN(link_database_user, "SELECT Function_Add FROM LIST_USER WHERE Bank_ID = '" & user_name & "'")
        If function_add = True Then
            BarButtonItem_AddNewRecord.Enabled = True
        End If

        Dim function_edit As Boolean = SQL_QUERY_TO_BOOLEAN(link_database_user, "SELECT Function_Edit FROM LIST_USER WHERE Bank_ID = '" & user_name & "'")
        If function_edit = True Then
            BarButtonItem_Edit.Enabled = True
        End If

        Dim function_set_expired As Boolean = SQL_QUERY_TO_BOOLEAN(link_database_user, "SELECT Function_Set_Expired FROM LIST_USER WHERE Bank_ID = '" & user_name & "'")
        If function_set_expired = True Then
            btSetExpired.Enabled = True
        End If

        Dim function_datamaintenance As Boolean = SQL_QUERY_TO_BOOLEAN(link_database_user, "SELECT Function_DataMaintenance FROM LIST_USER WHERE Bank_ID = '" & user_name & "'")
        If function_datamaintenance = True Then
            BarButtonItem_DataMaintenance.Enabled = True
        End If

        Dim function_user As Boolean = SQL_QUERY_TO_BOOLEAN(link_database_user, "SELECT Function_User FROM LIST_USER WHERE Bank_ID = '" & user_name & "'")
        If function_user = True Then
            BarButtonItem_User.Enabled = True
            BarButtonItem_Config.Enabled = True
        End If

    End Sub

    Private Sub btExportToExcel_Click(sender As Object, e As EventArgs) Handles btExportToExcel.Click
        On Error GoTo err_handle
        EXPORT_DATAGRIDVIEW_TO_EXCEL(GridView1)
        Exit Sub
err_handle:
        Error_handle()
    End Sub

    Private Sub BarToggleSwitchItem_WB_CheckedChanged(sender As Object, e As ItemClickEventArgs) Handles BarToggleSwitchItem_WB.CheckedChanged
        On Error GoTo err_handle

        refresh_link()

        tbDAFrom.Text = ""
        tbDA_To.Text = ""
        tbDelegation.Text = ""
        tbEffectiveDate.Text = ""
        tbExpiryDate.Text = ""
        tbNote.Text = ""
        tbStatus.Text = ""
        tbDepartment.Text = ""
        tbStrSearch.Text = ""
        lbCaseID.Text = ""
        Dim list_segment As String = ""

        If BarToggleSwitchItem_SME.Checked = True Then
            If list_segment.Length = 0 Then
                list_segment = "'SME'"
            Else
                list_segment = list_segment & ", 'SME'"
            End If
        End If
        If BarToggleSwitchItem_WB.Checked = True Then
            If list_segment.Length = 0 Then
                list_segment = "'WB'"
            Else
                list_segment = list_segment & ", 'WB'"
            End If
        End If
        If BarToggleSwitchItem_Individual.Checked = True Then
            If list_segment.Length = 0 Then
                list_segment = "'INDIVIDUAL'"
            Else
                list_segment = list_segment & ", 'INDIVIDUAL'"
            End If
        End If

        If list_segment.Length = 0 Then
            GridControl1.DataSource = Nothing
            GridView1.Columns.Clear()
        End If

        If cbFilterAll.Checked = True Then
            GridControl1.DataSource = SQL_QUERY_TO_DATATABLE(link_database_DA, "SELECT Department,DA_To,PWID_DA_To,Position_To,Delegation,Effective_Date,Expiry_Date,Status,Note,DA_From,PWID_DA_From,Position_From,File_Name,User_Created,User_Modified,Segment,CaseID FROM Main_DA WHERE Deleted_Date IS NULL AND Segment IN (" & list_segment & ") ORDER BY [CaseID] ASC")
        Else
            GridControl1.DataSource = SQL_QUERY_TO_DATATABLE(link_database_DA, "SELECT Department,DA_To,PWID_DA_To,Position_To,Delegation,Effective_Date,Expiry_Date,Status,Note,DA_From,PWID_DA_From,Position_From,File_Name,User_Created,User_Modified,Segment,CaseID FROM Main_DA WHERE Deleted_Date IS NULL AND Status = 'In Progress' AND Segment IN (" & list_segment & ") ORDER BY [CaseID] ASC")
        End If

        GridView1.BestFitColumns()
        Exit Sub
err_handle:
        Module_DA.Error_handle()
    End Sub

    Private Sub BarToggleSwitchItem_SME_CheckedChanged(sender As Object, e As ItemClickEventArgs) Handles BarToggleSwitchItem_SME.CheckedChanged
        On Error GoTo err_handle

        refresh_link()

        tbDAFrom.Text = ""
        tbDA_To.Text = ""
        tbDelegation.Text = ""
        tbEffectiveDate.Text = ""
        tbExpiryDate.Text = ""
        tbNote.Text = ""
        tbStatus.Text = ""
        tbStrSearch.Text = ""
        tbDepartment.Text = ""
        lbCaseID.Text = ""
        Dim list_segment As String = ""

        If BarToggleSwitchItem_SME.Checked = True Then
            If list_segment.Length = 0 Then
                list_segment = "'SME'"
            Else
                list_segment = list_segment & ", 'SME'"
            End If
        End If
        If BarToggleSwitchItem_WB.Checked = True Then
            If list_segment.Length = 0 Then
                list_segment = "'WB'"
            Else
                list_segment = list_segment & ", 'WB'"
            End If
        End If
        If BarToggleSwitchItem_Individual.Checked = True Then
            If list_segment.Length = 0 Then
                list_segment = "'INDIVIDUAL'"
            Else
                list_segment = list_segment & ", 'INDIVIDUAL'"
            End If
        End If

        If list_segment.Length = 0 Then
            GridControl1.DataSource = Nothing
            GridView1.Columns.Clear()
        End If

        If cbFilterAll.Checked = True Then
            GridControl1.DataSource = SQL_QUERY_TO_DATATABLE(link_database_DA, "SELECT Department,DA_To,PWID_DA_To,Position_To,Delegation,Effective_Date,Expiry_Date,Status,Note,DA_From,PWID_DA_From,Position_From,File_Name,User_Created,User_Modified,Segment,CaseID FROM Main_DA WHERE Deleted_Date IS NULL AND Segment IN (" & list_segment & ") ORDER BY [CaseID] ASC")
        Else
            GridControl1.DataSource = SQL_QUERY_TO_DATATABLE(link_database_DA, "SELECT Department,DA_To,PWID_DA_To,Position_To,Delegation,Effective_Date,Expiry_Date,Status,Note,DA_From,PWID_DA_From,Position_From,File_Name,User_Created,User_Modified,Segment,CaseID FROM Main_DA WHERE Deleted_Date IS NULL AND Status = 'In Progress' AND Segment IN (" & list_segment & ") ORDER BY [CaseID] ASC")
        End If

        GridView1.BestFitColumns()
        Exit Sub
err_handle:
        Module_DA.Error_handle()
    End Sub

    Private Sub BarToggleSwitchItem_Individual_CheckedChanged(sender As Object, e As ItemClickEventArgs) Handles BarToggleSwitchItem_Individual.CheckedChanged
        On Error GoTo err_handle

        refresh_link()

        tbDAFrom.Text = ""
        tbDA_To.Text = ""
        tbDelegation.Text = ""
        tbEffectiveDate.Text = ""
        tbExpiryDate.Text = ""
        tbNote.Text = ""
        tbStatus.Text = ""
        tbStrSearch.Text = ""
        tbDepartment.Text = ""
        lbCaseID.Text = ""
        Dim list_segment As String = ""

        If BarToggleSwitchItem_SME.Checked = True Then
            If list_segment.Length = 0 Then
                list_segment = "'SME'"
            Else
                list_segment = list_segment & ", 'SME'"
            End If
        End If
        If BarToggleSwitchItem_WB.Checked = True Then
            If list_segment.Length = 0 Then
                list_segment = "'WB'"
            Else
                list_segment = list_segment & ", 'WB'"
            End If
        End If
        If BarToggleSwitchItem_Individual.Checked = True Then
            If list_segment.Length = 0 Then
                list_segment = "'INDIVIDUAL'"
            Else
                list_segment = list_segment & ", 'INDIVIDUAL'"
            End If
        End If

        If list_segment.Length = 0 Then
            GridControl1.DataSource = Nothing
            GridView1.Columns.Clear()
        End If

        If cbFilterAll.Checked = True Then
            GridControl1.DataSource = SQL_QUERY_TO_DATATABLE(link_database_DA, "SELECT Department,DA_To,PWID_DA_To,Position_To,Delegation,Effective_Date,Expiry_Date,Status,Note,DA_From,PWID_DA_From,Position_From,File_Name,User_Created,User_Modified,Segment,CaseID FROM Main_DA WHERE Deleted_Date IS NULL AND Segment IN (" & list_segment & ") ORDER BY [CaseID] ASC")
        Else
            GridControl1.DataSource = SQL_QUERY_TO_DATATABLE(link_database_DA, "SELECT Department,DA_To,PWID_DA_To,Position_To,Delegation,Effective_Date,Expiry_Date,Status,Note,DA_From,PWID_DA_From,Position_From,File_Name,User_Created,User_Modified,Segment,CaseID FROM Main_DA WHERE Deleted_Date IS NULL AND Status = 'In Progress' AND Segment IN (" & list_segment & ") ORDER BY [CaseID] ASC")
        End If

        GridView1.BestFitColumns()
        Exit Sub
err_handle:
        Module_DA.Error_handle()
    End Sub

    Private Sub BarButtonItem_AddNewRecord_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarButtonItem_AddNewRecord.ItemClick
        Dim frm As New Add_Item
        frm.Show()
    End Sub

    Private Sub BarButtonItem_Config_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarButtonItem_Config.ItemClick
        Dim frm As New Config
        frm.Show()
    End Sub

    Private Sub BarButtonItem_Exit_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarButtonItem_Exit.ItemClick
        Dim Response = MsgBox("Do you really want to exit?", vbYesNo + vbQuestion)
        If Response = vbYes Then
            Me.Close()
        End If
    End Sub

    Private Sub BarButtonItem_Edit_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarButtonItem_Edit.ItemClick
        Dim frm As New Form_Edit
        frm.Show()
    End Sub

    Private Sub btRefresh_Click(sender As Object, e As EventArgs) Handles btRefresh.Click
        On Error GoTo err_handle

        refresh_link()

        tbDAFrom.Text = ""
        tbDA_To.Text = ""
        tbDelegation.Text = ""
        tbEffectiveDate.Text = ""
        tbExpiryDate.Text = ""
        tbNote.Text = ""
        tbStatus.Text = ""
        tbStrSearch.Text = ""
        tbDepartment.Text = ""
        lbCaseID.Text = ""
        Dim list_segment As String = ""

        If BarToggleSwitchItem_SME.Checked = True Then
            If list_segment.Length = 0 Then
                list_segment = "'SME'"
            Else
                list_segment = list_segment & ", 'SME'"
            End If
        End If
        If BarToggleSwitchItem_WB.Checked = True Then
            If list_segment.Length = 0 Then
                list_segment = "'WB'"
            Else
                list_segment = list_segment & ", 'WB'"
            End If
        End If
        If BarToggleSwitchItem_Individual.Checked = True Then
            If list_segment.Length = 0 Then
                list_segment = "'INDIVIDUAL'"
            Else
                list_segment = list_segment & ", 'INDIVIDUAL'"
            End If
        End If

        If list_segment.Length = 0 Then
            GridControl1.DataSource = Nothing
            GridView1.Columns.Clear()
        End If

        If cbFilterAll.Checked = True Then
            GridControl1.DataSource = SQL_QUERY_TO_DATATABLE(link_database_DA, "SELECT Department,DA_To,PWID_DA_To,Position_To,Delegation,Effective_Date,Expiry_Date,Status,Note,DA_From,PWID_DA_From,Position_From,File_Name,User_Created,User_Modified,Segment,CaseID FROM Main_DA WHERE Deleted_Date IS NULL AND Segment IN (" & list_segment & ") ORDER BY [CaseID] ASC")
        Else
            GridControl1.DataSource = SQL_QUERY_TO_DATATABLE(link_database_DA, "SELECT Department,DA_To,PWID_DA_To,Position_To,Delegation,Effective_Date,Expiry_Date,Status,Note,DA_From,PWID_DA_From,Position_From,File_Name,User_Created,User_Modified,Segment,CaseID FROM Main_DA WHERE Deleted_Date IS NULL AND Status = 'In Progress' AND Segment IN (" & list_segment & ") ORDER BY [CaseID] ASC")
        End If
        Exit Sub
err_handle:
        Module_DA.Error_handle()
    End Sub

    Private Sub SplitContainerControl1_DoubleClick(sender As Object, e As EventArgs) Handles SplitContainerControl1.DoubleClick
        If SplitContainerControl1.SplitterPosition > SplitContainerControl1.Width * 0.31 Then
            SplitContainerControl1.SplitterPosition = SplitContainerControl1.Width * 0.3
        Else
            SplitContainerControl1.SplitterPosition = SplitContainerControl1.Width
        End If
    End Sub

    Private Sub GridView1_RowClick(sender As Object, e As RowClickEventArgs) Handles GridView1.RowClick
        On Error GoTo err_handle

        If folder_path_save_file.Substring(folder_path_save_file.Length - 1, 1) <> "\" Then
            folder_path_save_file = folder_path_save_file & "\"
        End If


        tbDAFrom.Text = ""
        tbDA_To.Text = ""
        If GridView1.GetFocusedRowCellDisplayText("PWID_DA_To").ToString.Length > 0 Then
            If tbDA_To.Text.Length = 0 Then
                tbDA_To.Text = GridView1.GetFocusedRowCellDisplayText("PWID_DA_To").ToString
            Else
                tbDA_To.Text = tbDA_To.Text & "; " & GridView1.GetFocusedRowCellDisplayText("PWID_DA_To").ToString
            End If
        End If
        If GridView1.GetFocusedRowCellDisplayText("DA_To").ToString.Length > 0 Then
            If tbDA_To.Text.Length = 0 Then
                tbDA_To.Text = GridView1.GetFocusedRowCellDisplayText("DA_To").ToString
            Else
                tbDA_To.Text = tbDA_To.Text & "; " & GridView1.GetFocusedRowCellDisplayText("DA_To").ToString
            End If
        End If
        If GridView1.GetFocusedRowCellDisplayText("Position_To").ToString.Length > 0 Then
            If tbDA_To.Text.Length = 0 Then
                tbDA_To.Text = GridView1.GetFocusedRowCellDisplayText("Position_To").ToString
            Else
                tbDA_To.Text = tbDA_To.Text & "; " & GridView1.GetFocusedRowCellDisplayText("Position_To").ToString
            End If
        End If

        If GridView1.GetFocusedRowCellDisplayText("PWID_DA_From").ToString.Length > 0 Then
            If tbDAFrom.Text.Length = 0 Then
                tbDAFrom.Text = GridView1.GetFocusedRowCellDisplayText("PWID_DA_From").ToString
            Else
                tbDAFrom.Text = tbDAFrom.Text & "; " & GridView1.GetFocusedRowCellDisplayText("PWID_DA_From").ToString
            End If
        End If
        If GridView1.GetFocusedRowCellDisplayText("DA_From").ToString.Length > 0 Then
            If tbDAFrom.Text.Length = 0 Then
                tbDAFrom.Text = GridView1.GetFocusedRowCellDisplayText("DA_From").ToString
            Else
                tbDAFrom.Text = tbDAFrom.Text & "; " & GridView1.GetFocusedRowCellDisplayText("DA_From").ToString
            End If
        End If
        If GridView1.GetFocusedRowCellDisplayText("Position_From").ToString.Length > 0 Then
            If tbDAFrom.Text.Length = 0 Then
                tbDAFrom.Text = GridView1.GetFocusedRowCellDisplayText("Position_From").ToString
            Else
                tbDAFrom.Text = tbDAFrom.Text & "; " & GridView1.GetFocusedRowCellDisplayText("Position_From").ToString
            End If
        End If
        tbDelegation.Text = GridView1.GetFocusedRowCellDisplayText("Delegation").ToString

        tbEffectiveDate.Text = GridView1.GetFocusedRowCellDisplayText("Effective_Date").ToString
        tbExpiryDate.Text = GridView1.GetFocusedRowCellDisplayText("Expiry_Date").ToString
        tbNote.Text = GridView1.GetFocusedRowCellDisplayText("Note").ToString
        tbStatus.Text = GridView1.GetFocusedRowCellDisplayText("Status").ToString
        tbDepartment.Text = GridView1.GetFocusedRowCellDisplayText("Department").ToString
        lbCaseID.Text = GridView1.GetFocusedRowCellDisplayText("CaseID").ToString

        Exit Sub
err_handle:
        Module_DA.Error_handle()
    End Sub

    Private Sub GridView1_DoubleClick(sender As Object, e As EventArgs) Handles GridView1.DoubleClick
        On Error GoTo err_handle
        lbCaseID.Text = GridView1.GetFocusedRowCellDisplayText("CaseID").ToString

        Dim file_name As String = GridView1.GetFocusedRowCellDisplayText("File_Name").ToString

        If folder_path_save_file.Length = 0 Then
            Exit Sub
        End If
        If file_name.Length = 0 Then
            Exit Sub
        End If

        If folder_path_save_file.Substring(folder_path_save_file.Length - 1, 1) <> "\" Then
            folder_path_save_file = folder_path_save_file & "\"
        End If

        Dim pdfDoc As String = folder_path_save_file & file_name

        If File.Exists(pdfDoc) Then
            SplitContainerControl1.SplitterPosition = SplitContainerControl1.Width * 0.3
            PDFViewer.src = pdfDoc
        Else
            MsgBox("Not found pdf file: " & pdfDoc, vbCritical)
        End If

        If GridView1.GetFocusedRowCellDisplayText("PWID_DA_To").ToString.Length > 0 Then
            If tbDA_To.Text.Length = 0 Then
                tbDA_To.Text = GridView1.GetFocusedRowCellDisplayText("PWID_DA_To").ToString
            Else
                tbDA_To.Text = tbDA_To.Text & "; " & GridView1.GetFocusedRowCellDisplayText("PWID_DA_To").ToString
            End If
        End If
        If GridView1.GetFocusedRowCellDisplayText("DA_To").ToString.Length > 0 Then
            If tbDA_To.Text.Length = 0 Then
                tbDA_To.Text = GridView1.GetFocusedRowCellDisplayText("DA_To").ToString
            Else
                tbDA_To.Text = tbDA_To.Text & "; " & GridView1.GetFocusedRowCellDisplayText("DA_To").ToString
            End If
        End If
        If GridView1.GetFocusedRowCellDisplayText("Position_To").ToString.Length > 0 Then
            If tbDA_To.Text.Length = 0 Then
                tbDA_To.Text = GridView1.GetFocusedRowCellDisplayText("Position_To").ToString
            Else
                tbDA_To.Text = tbDA_To.Text & "; " & GridView1.GetFocusedRowCellDisplayText("Position_To").ToString
            End If
        End If


        If GridView1.GetFocusedRowCellDisplayText("PWID_DA_From").ToString.Length > 0 Then
            If tbDAFrom.Text.Length = 0 Then
                tbDAFrom.Text = GridView1.GetFocusedRowCellDisplayText("PWID_DA_From").ToString
            Else
                tbDAFrom.Text = tbDAFrom.Text & "; " & GridView1.GetFocusedRowCellDisplayText("PWID_DA_From").ToString
            End If
        End If
        If GridView1.GetFocusedRowCellDisplayText("DA_From").ToString.Length > 0 Then
            If tbDAFrom.Text.Length = 0 Then
                tbDAFrom.Text = GridView1.GetFocusedRowCellDisplayText("DA_From").ToString
            Else
                tbDAFrom.Text = tbDAFrom.Text & "; " & GridView1.GetFocusedRowCellDisplayText("DA_From").ToString
            End If
        End If
        If GridView1.GetFocusedRowCellDisplayText("Position_From").ToString.Length > 0 Then
            If tbDAFrom.Text.Length = 0 Then
                tbDAFrom.Text = GridView1.GetFocusedRowCellDisplayText("Position_From").ToString
            Else
                tbDAFrom.Text = tbDAFrom.Text & "; " & GridView1.GetFocusedRowCellDisplayText("Position_From").ToString
            End If
        End If
        tbDelegation.Text = GridView1.GetFocusedRowCellDisplayText("Delegation").ToString
        tbEffectiveDate.Text = GridView1.GetFocusedRowCellDisplayText("Effective_Date").ToString
        tbExpiryDate.Text = GridView1.GetFocusedRowCellDisplayText("Expiry_Date").ToString
        tbNote.Text = GridView1.GetFocusedRowCellDisplayText("Note").ToString
        tbStatus.Text = GridView1.GetFocusedRowCellDisplayText("Status").ToString
        tbDepartment.Text = GridView1.GetFocusedRowCellDisplayText("Department").ToString


        Exit Sub
err_handle:
        Module_DA.Error_handle()
    End Sub

    Private Sub cbFilterAll_CheckedChanged(sender As Object, e As EventArgs) Handles cbFilterAll.CheckedChanged
        On Error GoTo err_handle

        If tbStrSearch.Text.Length > 0 Then
            Dim txt_str_search As String = tbStrSearch.Text
            tbStrSearch.Text = ""
            tbStrSearch.Text = txt_str_search
        Else
            refresh_link()

            Dim list_segment As String = ""

            If BarToggleSwitchItem_SME.Checked = True Then
                If list_segment.Length = 0 Then
                    list_segment = "'SME'"
                Else
                    list_segment = list_segment & ", 'SME'"
                End If
            End If
            If BarToggleSwitchItem_WB.Checked = True Then
                If list_segment.Length = 0 Then
                    list_segment = "'WB'"
                Else
                    list_segment = list_segment & ", 'WB'"
                End If
            End If
            If BarToggleSwitchItem_Individual.Checked = True Then
                If list_segment.Length = 0 Then
                    list_segment = "'INDIVIDUAL'"
                Else
                    list_segment = list_segment & ", 'INDIVIDUAL'"
                End If
            End If

            If list_segment.Length = 0 Then
                GridControl1.DataSource = Nothing
                GridView1.Columns.Clear()
            End If

            If cbFilterAll.Checked = True Then
                GridControl1.DataSource = SQL_QUERY_TO_DATATABLE(link_database_DA, "SELECT Department,DA_To,PWID_DA_To,Position_To,Delegation,Effective_Date,Expiry_Date,Status,Note,DA_From,PWID_DA_From,Position_From,File_Name,User_Created,User_Modified,Segment,CaseID FROM Main_DA WHERE Deleted_Date IS NULL AND Segment IN (" & list_segment & ") ORDER BY [CaseID] ASC")
            Else
                GridControl1.DataSource = SQL_QUERY_TO_DATATABLE(link_database_DA, "SELECT Department,DA_To,PWID_DA_To,Position_To,Delegation,Effective_Date,Expiry_Date,Status,Note,DA_From,PWID_DA_From,Position_From,File_Name,User_Created,User_Modified,Segment,CaseID FROM Main_DA WHERE Deleted_Date IS NULL AND Status = 'In Progress' AND Segment IN (" & list_segment & ") ORDER BY [CaseID] ASC")
            End If

        End If
        GridView1.BestFitColumns()
        Exit Sub
err_handle:
        Module_DA.Error_handle()
    End Sub

    Private Sub tbStrSearch_EditValueChanged(sender As Object, e As EventArgs) Handles tbStrSearch.EditValueChanged
        On Error GoTo err_handle

        Dim list_segment As String = ""
        If BarToggleSwitchItem_SME.Checked = True Then
            If list_segment.Length = 0 Then
                list_segment = "'SME'"
            Else
                list_segment = list_segment & ", 'SME'"
            End If
        End If
        If BarToggleSwitchItem_WB.Checked = True Then
            If list_segment.Length = 0 Then
                list_segment = "'WB'"
            Else
                list_segment = list_segment & ", 'WB'"
            End If
        End If
        If BarToggleSwitchItem_Individual.Checked = True Then
            If list_segment.Length = 0 Then
                list_segment = "'INDIVIDUAL'"
            Else
                list_segment = list_segment & ", 'INDIVIDUAL'"
            End If
        End If

        Dim DT_ListCol As DataTable = SQL_QUERY_TO_DATATABLE(link_database_DA, "SELECT * FROM Main_DA LIMIT 1")

        Dim SQL_Str_Search As String = ""
        For i = 0 To DT_ListCol.Columns.Count - 1
            If SQL_Str_Search.Length = 0 Then
                SQL_Str_Search = "[" & DT_ListCol.Columns(i).ColumnName.ToString() & "] LIKE '%" & tbStrSearch.Text & "%' "
            Else
                SQL_Str_Search = SQL_Str_Search & " OR [" & DT_ListCol.Columns(i).ColumnName.ToString() & "] LIKE '%" & tbStrSearch.Text & "%' "
            End If
        Next

        GridControl1.DataSource = Nothing
        GridView1.Columns.Clear()

        If cbFilterAll.Checked = True Then
            GridControl1.DataSource = SQL_QUERY_TO_DATATABLE(link_database_DA, "SELECT Department,DA_To,PWID_DA_To,Position_To,Delegation,Effective_Date,Expiry_Date,Status,Note,DA_From,PWID_DA_From,Position_From,File_Name,User_Created,User_Modified,Segment,CaseID FROM Main_DA WHERE Deleted_Date IS NULL AND Segment IN (" & list_segment & ") AND " & SQL_Str_Search & " ORDER BY [CaseID] ASC")
        Else
            GridControl1.DataSource = SQL_QUERY_TO_DATATABLE(link_database_DA, "SELECT Department,DA_To,PWID_DA_To,Position_To,Delegation,Effective_Date,Expiry_Date,Status,Note,DA_From,PWID_DA_From,Position_From,File_Name,User_Created,User_Modified,Segment,CaseID FROM Main_DA WHERE (Deleted_Date IS NULL AND Status = 'In Progress' AND Segment IN (" & list_segment & ")) AND (" & SQL_Str_Search & ") ORDER BY [CaseID] ASC")
        End If

        SQL_Str_Search = ""
        Exit Sub
err_handle:
        Module_DA.Error_handle()
    End Sub

    Public Sub backup_database()
        If folder_path.Substring(folder_path.Length - 1, 1) <> "\" Then
            folder_path = folder_path & "\"
        End If

        Dim full_name_backup_DA As String = folder_path & "\BACKUP\Database_DA_" & Now().ToString("yyyyMMdd") & ".txt"

        If Not My.Computer.FileSystem.FileExists(full_name_backup_DA) Then
            If My.Computer.FileSystem.FileExists(link_database_DA) Then
                My.Computer.FileSystem.CopyFile(link_database_DA, full_name_backup_DA)
                WriteLog("Daily backup Database: " & full_name_backup_DA)
            End If
        End If

        For i As Integer = 2 To 30
            If My.Computer.FileSystem.FileExists(folder_path & "\BACKUP\Database_DA_" & Now.AddDays(-i).ToString("yyyyMMdd") & ".txt") Then
                Kill(folder_path & "\BACKUP\Database_DA_" & Now.AddDays(-i).ToString("yyyyMMdd") & ".txt")
            End If
        Next
    End Sub

    Private Sub btSetExpired_Click(sender As Object, e As EventArgs) Handles btSetExpired.Click
        On Error GoTo err_handle

        If lbCaseID.Text.Length = 0 Then Exit Sub
        Dim iRet = MsgBox("Do you want to set expired for this case?", vbYesNo + vbQuestion)

        If iRet = vbYes Then
            Dim str_update As String = "UPDATE Main_DA" &
                                        " SET [Status] = 'Expired'," &
                                        " [User_Modified] = '" & BarStaticItem_User.Caption & "_" & Now.ToString("dd/MM/yyyy hh:mm:ss tt") & "'" &
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

    Private Sub BarButtonItem_User_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarButtonItem_User.ItemClick
        Dim frm As New FormUser
        frm.Show()
    End Sub

    Private Sub BarButtonItem_ChangeLevelAccess_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarButtonItem_ChangeLevelAccess.ItemClick
        'Dim cipherText As String = Module_DA.SQL_QUERY_TO_STRING(link_database_user, "SELECT Password_Str FROM LIST_USER WHERE Bank_ID = '" & Environment.UserName & "'")
        'MsgBox("Password is: " & EncryptDecrypt.DecryptData(cipherText))

        Check_user_access()
        Dim f As New FormLogin
        f.Show()

    End Sub

#Region "XuLy nhận lệnh"
    Public Delegate Sub process_data()
    Dim th_NhanDuLieu As Thread
    Public Sub XuLy()
        Try
            While True
                If check_password = True Then
                    Me.BeginInvoke(New process_data(AddressOf Check_user_access))
                End If
                check_password = False
                Thread.Sleep(300)
            End While
        Catch ex As Exception
        End Try
    End Sub

    Public Sub start_thearing()
        th_NhanDuLieu = New Thread(AddressOf XuLy)
        th_NhanDuLieu.IsBackground = True
        th_NhanDuLieu.Start()
    End Sub
#End Region

    Private Sub BarButtonItem_ChangePW_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarButtonItem_ChangePW.ItemClick
        Dim f As New FormChangePW
        f.Show()
    End Sub

    Private Sub BarButtonItem_DataMaintenance_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarButtonItem_DataMaintenance.ItemClick
        Dim f As New FormMappingPDF_DA
        f.Show()
    End Sub

    Public Sub Auto_Set_Expired()
        Dim to_date As String = "'" & Format(Now.AddDays(-1), "yyyyMMdd") & "'"

        Dim str_update As String = "UPDATE Main_DA" &
                                     " SET [Status] = 'Expired'," &
                                     " [User_Modified] = 'SYSTEM_" & Now.ToString("dd/MM/yyyy hh:mm:ss tt") & "'" &
                                     " WHERE SUBSTR(Expiry_Date,7)||SUBSTR(Expiry_Date,4,2)||SUBSTR(Expiry_Date,1,2) BETWEEN '19000101' AND " & to_date

        SQL_QUERY_WRITE_LOG(link_database_DA, str_update)
    End Sub
End Class