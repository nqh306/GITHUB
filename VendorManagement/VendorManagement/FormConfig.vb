Imports DevExpress.XtraEditors.Controls
Public Class oo
    Public appPath As String = AppDomain.CurrentDomain.BaseDirectory
    Public link_app_config As String = appPath & "Application_Config.db"
    Public link_application_config As String = Module_Letter_Management.SQL_QUERY_TO_STRING(link_app_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'Link_Application_Config'")
    Private Sub FormConfig_Load(sender As Object, e As EventArgs) Handles Me.Load
        On Error GoTo err_handle

        SQLITE_CREATE_REPORT_APPCONFIG(link_application_config)

        EnableDoubleBuffered(DataGridView_CROWN)
        EnableDoubleBuffered(DataGridView_LogicCreateBIll)

        'Add value for comboBox Vendor_Name
        ComboBox_VendorName.Items.Add("TASETCO EXPRESS")
        ComboBox_VendorName.Items.Add("VIETTEL POST")

        'Add value for combobox_Logic_Type
        Logic_ComboBoxType.Items.Add("Date")
        Logic_ComboBoxType.Items.Add("Text")
        Logic_ComboBoxType.Items.Add("Numberic (Fixed)")
        Logic_ComboBoxType.Items.Add("Numberic (Period)")

        'Add value for ComboBox Frequency
        Logic_ComboBox_Frequency.Items.Add("Daily")
        Logic_ComboBox_Frequency.Items.Add("Monthly")

        Logic_tbPeriodFrom.Maximum = 1000000000
        Logic_tbPeriodFrom.Minimum = 0

        Logic_tbPeriodTo.Maximum = 1000000000
        Logic_tbPeriodTo.Minimum = 0

        Logic_tbValueAsNo.Maximum = 1000000000
        Logic_tbValueAsNo.Minimum = 0

        'Add value for ComboBox_Type_Load_CUSTCONT
        ComboBox_Type_Load_CUSTCONT.Items.Add("csv Format (Fixed)")
        ComboBox_Type_Load_CUSTCONT.Items.Add("csv Format (ByDate)")
        ComboBox_Type_Load_CUSTCONT.Items.Add("zip Format (Fixed)")
        ComboBox_Type_Load_CUSTCONT.Items.Add("zip Format (ByDate)")

        ComboBox_Type_Load_CUSTCONT.Text = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'CUSTCONT_Type'")
        ButtonEdit_CUSTCONT.Text = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'CUSTCONT_File_Path'")
        tbCUSTCONT_FileName.Text = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'CUSTCONT_File_Name'")
        tbCUSTCONT_FileName_ZIP.Text = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'CUSTCONT_File_Name_byZip'")

        'Add value for ComboBox_UpdateDatabase_VisibleWebBrowser
        ComboBox_UpdateDatabase_VisibleWebBrowser.Text = Module_Letter_Management.SQL_QUERY_TO_STRING(link_app_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'Update_Database_Visible_WebBrowser'")

        'Add value for DatabaseName
        Dim dt_DatabaseName As DataTable = Module_Letter_Management.SQL_QUERY_TO_DATATABLE(link_application_config, "SELECT DISTINCT Database_Name FROM LIST_DATABASE")
        If dt_DatabaseName.Rows.Count > 0 Then
            For i As Integer = 0 To dt_DatabaseName.Rows.Count - 1
                ComboBox_DatabaseName.Items.Add(dt_DatabaseName.Rows(i).Item(0).ToString)
            Next
            ComboBox_DatabaseName.Text = dt_DatabaseName.Rows(0).Item(0).ToString
        End If

        'Get information of database Special List
        ButtonEdit_LinkDatabaseSpecialList.Text = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'Link_database_Special_List'")
        Dim dt As DataTable = Module_Letter_Management.GET_ALL_TABLE_NAME_IN_DATABASE(ButtonEdit_LinkDatabaseSpecialList.Text)
        If dt.Rows.Count > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                ComboBox_TableName.Items.Add(dt.Rows(i).Item(0).ToString)
                ComboBox_ListSpecialList.Items.Add(dt.Rows(i).Item(0).ToString)
            Next
            ComboBox_TableName.Text = dt.Rows(0).Item(0).ToString
        End If

        'Get information of database Portfolio
        ButtonEdit_LinkFolder_Portfolio.Text = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'Portfolio_File_Path'")
        tbDatabase_FileName_Portfolio.Text = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'Portfolio_File_Name'")

        'Get information of database CUSTDTL
        ButtonEdit_LinkFolder_CUSTDTL.Text = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'CUSTDTL_File_Path'")
        tbDatabase_FileName_CUSTDTL.Text = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'CUSTDTL_File_Name'")

        'add value visible webbrowser 
        ComboBox_UpdateDatabase_VisibleWebBrowser.Items.Add("True")
        ComboBox_UpdateDatabase_VisibleWebBrowser.Items.Add("False")

        'Get information of database CUSTDTL
        ButtonEdit_LinkAppConfig.Text = Module_Letter_Management.SQL_QUERY_TO_STRING(link_app_config, "SELECT Field_value1 FROM Config WHERE Field_name = 'Link_Application_Config'")

        'load database list CROWN into GridView
        Dim DT_CROWN As New DataTable
        DT_CROWN = Module_Letter_Management.SQL_QUERY_TO_DATATABLE(link_application_config, "SELECT * FROM [LIST_DATABASE_CROWN]")
        DataGridView_CROWN.DataSource = DT_CROWN

        'Create database Report in not exists

        EnableDoubleBuffered(DataGridView_Report)
        ComboBox_Report_PaperKind.Items.Add("Portrait")
        ComboBox_Report_PaperKind.Items.Add("Landscape")

        ComboBox_Report_Vendor.Items.Add("TASETCO")
        ComboBox_Report_Vendor.Items.Add("VIETTEL")
        ComboBox_Report_Vendor.Items.Add("Both")

        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub ButtonEdit_LinkDatabaseSpecialList_Click(sender As Object, e As EventArgs) Handles ButtonEdit_LinkDatabaseSpecialList.Click
        On Error GoTo err_handle
        Dim fd As OpenFileDialog = New OpenFileDialog()
        fd.Title = "Select database Special List"
        fd.InitialDirectory = "C:\"
        fd.Filter = "SQLite Database (*.db)|*.db|SQLite Database (*.txt)|*.txt|All files (*.*)|*.*"

        fd.FilterIndex = 2
        fd.RestoreDirectory = True

        If fd.ShowDialog() = DialogResult.OK Then
            If fd.FileName.Length > 0 Then
                ButtonEdit_LinkDatabaseSpecialList.Text = fd.FileName
                ComboBox_TableName.Items.Clear()
                Dim dt As DataTable = Module_Letter_Management.GET_ALL_TABLE_NAME_IN_DATABASE(fd.FileName)
                If dt.Rows.Count > 0 Then
                    For i As Integer = 0 To dt.Rows.Count - 1
                        ComboBox_TableName.Items.Add(dt.Rows(i).Item(0).ToString)
                    Next
                    ComboBox_TableName.Text = dt.Rows(0).Item(0).ToString
                End If
            End If
        End If
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub
    Private Sub ButtonEdit_LinkFolder_Portfolio_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles ButtonEdit_LinkFolder_Portfolio.ButtonClick
        On Error GoTo err_handle
        Dim folderDlg As New FolderBrowserDialog
        folderDlg.ShowNewFolderButton = True

        If (folderDlg.ShowDialog() = DialogResult.OK) Then
            ButtonEdit_LinkFolder_Portfolio.Text = folderDlg.SelectedPath
            If ButtonEdit_LinkFolder_Portfolio.Text.Substring(ButtonEdit_LinkFolder_Portfolio.Text.Length - 1, 1) = "\" Then
                ButtonEdit_LinkFolder_Portfolio.Text = ButtonEdit_LinkFolder_Portfolio.Text.Substring(1, ButtonEdit_LinkFolder_Portfolio.Text.Length - 1)
            End If
            Dim root As Environment.SpecialFolder = folderDlg.RootFolder
        End If
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub ButtonEdit_LinkFolder_CUSTDTL_Click(sender As Object, e As EventArgs) Handles ButtonEdit_LinkFolder_CUSTDTL.Click
        On Error GoTo err_handle
        Dim folderDlg As New FolderBrowserDialog
        folderDlg.ShowNewFolderButton = True

        If (folderDlg.ShowDialog() = DialogResult.OK) Then
            ButtonEdit_LinkFolder_CUSTDTL.Text = folderDlg.SelectedPath
            If ButtonEdit_LinkFolder_CUSTDTL.Text.Substring(ButtonEdit_LinkFolder_CUSTDTL.Text.Length - 1, 1) = "\" Then
                ButtonEdit_LinkFolder_CUSTDTL.Text = ButtonEdit_LinkFolder_CUSTDTL.Text.Substring(1, ButtonEdit_LinkFolder_CUSTDTL.Text.Length - 1)
            End If
            Dim root As Environment.SpecialFolder = folderDlg.RootFolder
        End If
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub
    Private Sub btDatabase_ChangeLinkCUSTDTL_Click(sender As Object, e As EventArgs) Handles btDatabase_ChangeLinkCUSTDTL.Click
        On Error GoTo err_handle
        Module_Letter_Management.SQL_QUERY(link_application_config, "UPDATE Config SET [Field_value1] = '" & ButtonEdit_LinkFolder_CUSTDTL.Text & "' WHERE [Field_name] = 'CUSTDTL_File_Path'")
        Module_Letter_Management.SQL_QUERY(link_application_config, "UPDATE Config SET [Field_value1] = '" & tbDatabase_FileName_CUSTDTL.Text & "' WHERE [Field_name] = 'CUSTDTL_File_Name'")
        Main.Statusbar_item1.Caption = ""
        Main.Statusbar_item2.Caption = "Changed link source file CUSTDTL to " & ButtonEdit_LinkFolder_CUSTDTL.Text & "\" & tbDatabase_FileName_CUSTDTL.Text & ": COMPLETED"
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub
    Private Sub btDatabase_ChangeLinkSpecial_Click(sender As Object, e As EventArgs) Handles btDatabase_ChangeLinkSpecial.Click
        On Error GoTo err_handle
        Module_Letter_Management.SQL_QUERY(link_application_config, "UPDATE Config SET [Field_value1] = '" & ButtonEdit_LinkDatabaseSpecialList.Text & "' WHERE [Field_name] = 'Link_database_Special_List'")
        Main.Statusbar_item1.Caption = ""
        Main.Statusbar_item2.Caption = "Changed database Special List to " & ButtonEdit_LinkDatabaseSpecialList.Text & ": COMPLETED"
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub btDatabase_Special_CreateNew_Click(sender As Object, e As EventArgs) Handles btDatabase_Special_CreateNew.Click
        On Error GoTo err_handle
        Dim folderDlg As New FolderBrowserDialog
        Dim link_database As String
        folderDlg.ShowNewFolderButton = True
        If (folderDlg.ShowDialog() = DialogResult.OK) Then
            If folderDlg.SelectedPath.Substring(folderDlg.SelectedPath.Length - 1, 1) = "\" Then
                link_database = folderDlg.SelectedPath & "Database_Special_List.txt"
            Else
                link_database = folderDlg.SelectedPath & "\Database_Special_List.txt"
            End If

            Module_Letter_Management.SQLITE_CREATE_DATABASE_FILE(link_database)
            Module_Letter_Management.SQL_QUERY(link_application_config, "UPDATE Config SET [Field_value1] = '" & link_database & "' WHERE [Field_name] = 'Link_database_Special_List'")
            ButtonEdit_LinkDatabaseSpecialList.Text = link_database

            ComboBox_TableName.Items.Clear()
            Dim dt As DataTable = Module_Letter_Management.GET_ALL_TABLE_NAME_IN_DATABASE(link_database)
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    ComboBox_TableName.Items.Add(dt.Rows(i).Item(0).ToString)
                Next
                ComboBox_TableName.Text = dt.Rows(0).Item(0).ToString
            End If
            Main.Statusbar_item1.Caption = ""
            Main.Statusbar_item2.Caption = "Create new database Special List " & ButtonEdit_LinkDatabaseSpecialList.Text & ": COMPLETED"
        End If
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub btDatabase_CreateNewTable_Click(sender As Object, e As EventArgs) Handles btDatabase_CreateNewTable.Click
        On Error GoTo err_handle
        Dim iRet As String = MsgBox("Do you want to create new table for database :" & ButtonEdit_LinkDatabaseSpecialList.Text & "?", vbYesNo, "Vendor Management - Config")
        If iRet = vbYes Then
            Dim table_name As String = InputBox("Please input table name for create new table: ", "Vendor Managment - Config")

            Dim all_table_name As String = ""
            Dim dt As DataTable = Module_Letter_Management.GET_ALL_TABLE_NAME_IN_DATABASE(ButtonEdit_LinkDatabaseSpecialList.Text)
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    all_table_name = all_table_name & ";" & dt.Rows(i).Item(0).ToString
                Next
            End If

            If InStr(all_table_name, table_name) > 0 Then
                MsgBox("Table " & table_name & " exists", vbCritical, "Vendor Management - Config")
                Exit Sub
            End If
            Module_Letter_Management.SQLITE_CREATE_TABLE_SPECIAL_LIST_IF_NOT_EXISTS(ButtonEdit_LinkDatabaseSpecialList.Text, table_name)



            Main.Statusbar_item1.Caption = ""
            Main.Statusbar_item2.Caption = "Add new table " & table_name & " for database Special List: COMPLETED"
        End If
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub btDatabase_ChangeLinkPortfolio_Click_1(sender As Object, e As EventArgs) Handles btDatabase_ChangeLinkPortfolio.Click
        On Error GoTo err_handle
        Module_Letter_Management.SQL_QUERY(link_application_config, "UPDATE Config SET [Field_value1] = '" & ButtonEdit_LinkFolder_Portfolio.Text & "' WHERE [Field_name] = 'Portfolio_File_Path'")
        Module_Letter_Management.SQL_QUERY(link_application_config, "UPDATE Config SET [Field_value1] = '" & tbDatabase_FileName_Portfolio.Text & "' WHERE [Field_name] = 'Portfolio_File_Name'")
        Main.Statusbar_item1.Caption = ""
        Main.Statusbar_item2.Caption = "Changed link source file Portfolio to " & ButtonEdit_LinkFolder_Portfolio.Text & "\" & tbDatabase_FileName_Portfolio.Text & ": COMPLETED"
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub btUpdateDatabase_ChangeVisible_Click(sender As Object, e As EventArgs) Handles btUpdateDatabase_ChangeVisible.Click
        Module_Letter_Management.SQL_QUERY(link_application_config, "UPDATE Config SET [Field_value1] = '" & ComboBox_UpdateDatabase_VisibleWebBrowser.Text & "' WHERE [Field_name] = 'Update_Database_Visible_WebBrowser'")
        Main.Statusbar_item2.Caption = "Changed Visible of WebBrowser to " & ComboBox_UpdateDatabase_VisibleWebBrowser.Text & ": COMPLETED"
    End Sub

    Private Sub btCROWN_Add_Click(sender As Object, e As EventArgs) Handles btCROWN_Add.Click
        On Error GoTo err_handle
        If tbCROWN_DatabaseName.Text.Length = 0 Or ButtonEditCROWN_FolderPath.Text.Length = 0 Then
            MsgBox("no DATA")
            Exit Sub
        End If
        Dim iRet = MsgBox("Do you want to add new database?", vbYesNo, "Config - Vendor Management")
        If iRet = vbYes Then
            If Module_Letter_Management.SQL_QUERY_TO_INTEGER(link_application_config, "SELECT COUNT(Database_Name) FROM [LIST_DATABASE_CROWN] WHERE [Database_Name]='" & tbCROWN_DatabaseName.Text & "'") > 0 Then
                MsgBox("Can't create new database. Database name " & tbCROWN_DatabaseName.Text & " existed", vbCritical, "Config - Vendor Management - Error")
                Exit Sub
            End If
            Dim Str_SQL_Add_Record As String = "INSERT INTO LIST_DATABASE_CROWN ([Database_Name], [Folder_Path], [User_Created]) VALUES ('" & UCase(tbCROWN_DatabaseName.Text) & "', '" & ButtonEditCROWN_FolderPath.Text & "', '" & Environment.UserName & "_" & Now.ToString("ddMMyyyy hh:mm:ss") & "')"
            Module_Letter_Management.SQL_QUERY(link_application_config, Str_SQL_Add_Record)
            Main.Statusbar_item2.Caption = "Add new database " & tbCROWN_DatabaseName.Text & ": COMPLETED"
        End If

        Dim DT As New DataTable
        DT = Module_Letter_Management.SQL_QUERY_TO_DATATABLE(link_application_config, "SELECT * FROM [LIST_DATABASE_CROWN]")
        DataGridView_CROWN.DataSource = DT
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub ButtonEdit_LinkAppConfig_Click(sender As Object, e As EventArgs) Handles ButtonEdit_LinkAppConfig.Click
        Dim fd As OpenFileDialog = New OpenFileDialog()
        fd.Title = "Open File Dialog"
        fd.InitialDirectory = "C:\"
        fd.Filter = "|All Files|*.*" +
                    "Database files|*.txt"

        fd.FilterIndex = 2
        fd.RestoreDirectory = True

        If fd.ShowDialog() = DialogResult.OK Then
            If fd.FileName.Length > 0 Then
                ButtonEdit_LinkAppConfig.Text = fd.FileName
            End If
        End If
    End Sub

    Private Sub btChange_Click(sender As Object, e As EventArgs) Handles btChange.Click
        On Error GoTo err_handle
        Module_Letter_Management.SQL_QUERY(link_app_config, "UPDATE Config SET [Field_value1] = '" & ButtonEdit_LinkAppConfig.Text & "' WHERE [Field_name] = 'Link_Application_Config'")
        Main.Statusbar_item1.Caption = ""
        Main.Statusbar_item2.Caption = "Changed link application config to " & ButtonEdit_LinkAppConfig.Text & ": COMPLETED"
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub
    Private Sub ButtonEdit_CUSTCONT_Click(sender As Object, e As EventArgs) Handles ButtonEdit_CUSTCONT.Click
        On Error GoTo err_handle
        Dim folderDlg As New FolderBrowserDialog
        folderDlg.ShowNewFolderButton = True

        If (folderDlg.ShowDialog() = DialogResult.OK) Then
            ButtonEdit_CUSTCONT.Text = folderDlg.SelectedPath
            If ButtonEdit_CUSTCONT.Text.Substring(ButtonEdit_CUSTCONT.Text.Length - 1, 1) = "\" Then
                ButtonEdit_CUSTCONT.Text = ButtonEdit_CUSTCONT.Text.Substring(1, ButtonEdit_LinkFolder_CUSTDTL.Text.Length - 1)
            End If
            Dim root As Environment.SpecialFolder = folderDlg.RootFolder
        End If
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub btCUSTCONT_ChangeLink_Click(sender As Object, e As EventArgs) Handles btCUSTCONT_ChangeLink.Click
        On Error GoTo err_handle
        Module_Letter_Management.SQL_QUERY(link_application_config, "UPDATE Config SET [Field_value1] = '" & ComboBox_Type_Load_CUSTCONT.Text & "' WHERE [Field_name] = 'CUSTCONT_Type'")
        Module_Letter_Management.SQL_QUERY(link_application_config, "UPDATE Config SET [Field_value1] = '" & tbCUSTCONT_FileName_ZIP.Text & "' WHERE [Field_name] = 'CUSTCONT_File_Name_byZip'")
        Module_Letter_Management.SQL_QUERY(link_application_config, "UPDATE Config SET [Field_value1] = '" & ButtonEdit_CUSTCONT.Text & "' WHERE [Field_name] = 'CUSTCONT_File_Path'")
        Module_Letter_Management.SQL_QUERY(link_application_config, "UPDATE Config SET [Field_value1] = '" & tbCUSTCONT_FileName.Text & "' WHERE [Field_name] = 'CUSTCONT_File_Name'")
        Main.Statusbar_item1.Caption = ""
        Main.Statusbar_item2.Caption = "Changed link source file CUSTCONT to " & ButtonEdit_CUSTCONT.Text & "\" & tbCUSTCONT_FileName.Text & ": COMPLETED"
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub ComboBox_DatabaseName_TextChanged(sender As Object, e As EventArgs) Handles ComboBox_DatabaseName.TextChanged
        On Error GoTo err_handle

        If ComboBox_DatabaseName.Text.Length = 0 Then Exit Sub

        tbFolderPath.Text = ""
        tbUserUpdatingDatabase.Text = ""
        tbYearUpdatingDatabase.Text = ""
        ComboBox_ListSpecialList.Text = ""
        Logic_ComboBoxType.Text = ""
        Logic_tbFormat.Text = ""
        Logic_tbPeriodFrom.Value = 0
        Logic_tbPeriodTo.Value = 0
        Logic_tbSequence.Text = ""
        Logic_tbValueAsNo.Value = 0
        Logic_tbValueAsText.Text = ""
        ComboBox_VendorName.Text = ""

        DataGridView_LogicCreateBIll.DataSource = Nothing


        ComboBox_VendorName.Text = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT DISTINCT Vendor_Name FROM LIST_DATABASE WHERE Database_Name = '" & ComboBox_DatabaseName.Text & "'")
        tbFolderPath.Text = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT DISTINCT Folder_Path FROM LIST_DATABASE WHERE Database_Name = '" & ComboBox_DatabaseName.Text & "'")
        ComboBox_ListSpecialList.Text = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT DISTINCT Table_Special_List FROM LIST_DATABASE WHERE Database_Name = '" & ComboBox_DatabaseName.Text & "'")
        tbUserUpdatingDatabase.Text = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT DISTINCT User_Updating_Database FROM LIST_DATABASE WHERE Database_Name = '" & ComboBox_DatabaseName.Text & "'")
        tbYearUpdatingDatabase.Text = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT DISTINCT Year_Updating_Database FROM LIST_DATABASE WHERE Database_Name = '" & ComboBox_DatabaseName.Text & "'")

        Dim dt_List_Logic As DataTable = Module_Letter_Management.SQL_QUERY_TO_DATATABLE(link_application_config, "SELECT Logic_Frequency, Logic_SequenceNo, Logic_Type, Logic_Value_As_Number, Logic_Value_As_Text, Logic_Period_From, Logic_Period_To, Logic_Format FROM LIST_DATABASE WHERE Database_Name = '" & ComboBox_DatabaseName.Text & "' ORDER BY Logic_Frequency ASC, Logic_SequenceNo ASC")
        DataGridView_LogicCreateBIll.DataSource = dt_List_Logic

        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub Database_btAdd_Click(sender As Object, e As EventArgs) Handles Database_btAdd.Click
        On Error GoTo err_handle
        Dim database_name = InputBox("Please input Database Name for create!", "Vendor Management - Add Database")
        If Module_Letter_Management.SQL_QUERY_TO_INTEGER(link_application_config, "SELECT COUNT(Database_Name) FROM LIST_DATABASE WHERE [Database_Name]='" & database_name & "'") > 0 Then
            MsgBox("Database Name " & database_name & " exists", vbCritical)
            Exit Sub
        Else
            Dim Str_SQL_Add_Record As String = "INSERT INTO LIST_DATABASE ([Database_Name], [Logic_Frequency], [Logic_SequenceNo]) VALUES ('" & UCase(database_name) & "', 'Daily', 1)"
            Module_Letter_Management.SQL_QUERY(link_application_config, Str_SQL_Add_Record)
            ComboBox_DatabaseName.Text = UCase(database_name)
        End If

        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub Database_btUpdate_Click(sender As Object, e As EventArgs) Handles Database_btUpdate.Click
        On Error GoTo err_handle

        If ComboBox_DatabaseName.Text.Length = 0 Then Exit Sub

        Dim database_name As String = ComboBox_DatabaseName.Text


        Dim iRet = MsgBox("Do you want to update information for database " & ComboBox_DatabaseName.Text & "?", vbYesNo)
        If iRet = vbYes Then
            Dim str_update As String = "UPDATE LIST_DATABASE" &
                                         " SET [Vendor_name] = '" & ComboBox_VendorName.Text & "'," &
                                            " [Folder_Path] = '" & tbFolderPath.Text & "'," &
                                            " [Table_Special_List] = '" & ComboBox_ListSpecialList.Text & "'," &
                                            " [User_Updating_Database] = '" & tbUserUpdatingDatabase.Text & "'," &
                                            " [Year_Updating_Database] = '" & tbYearUpdatingDatabase.Text & "'" &
                                         " WHERE [Database_Name] = '" & ComboBox_DatabaseName.Text & "'"

            Module_Letter_Management.SQL_QUERY(link_application_config, str_update)
            ComboBox_DatabaseName.Text = ""
            ComboBox_DatabaseName.Text = database_name
        End If
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub Database_btDelete_Click(sender As Object, e As EventArgs) Handles Database_btDelete.Click
        On Error GoTo err_handle
        If ComboBox_DatabaseName.Text.Length = 0 Then Exit Sub
        Dim iRet = MsgBox("Do you want to delete database " & ComboBox_DatabaseName.Text & "?", vbYesNo)
        If iRet = vbYes Then
            Dim SQL_string As String = "DELETE FROM LIST_DATABASE WHERE [Database_Name]='" & ComboBox_DatabaseName.Text & "'"
            Module_Letter_Management.SQL_QUERY(link_application_config, SQL_string)

            ComboBox_DatabaseName.Text = ""
            tbFolderPath.Text = ""
            tbUserUpdatingDatabase.Text = ""
            tbYearUpdatingDatabase.Text = ""
            ComboBox_ListSpecialList.Text = ""
            Logic_ComboBox_Frequency.Text = ""
            Logic_ComboBoxType.Text = ""
            Logic_tbFormat.Text = ""
            Logic_tbPeriodFrom.Value = 0
            Logic_tbPeriodTo.Value = 0
            Logic_tbSequence.Text = ""
            Logic_tbValueAsNo.Value = 0
            Logic_tbValueAsText.Text = ""
            DataGridView_LogicCreateBIll.DataSource = Nothing
        End If
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub Logic_ComboBoxType_TextChanged(sender As Object, e As EventArgs) Handles Logic_ComboBoxType.TextChanged
        On Error GoTo err_handle
        If Logic_ComboBoxType.Text.Length = 0 Then Exit Sub

        Logic_tbFormat.Text = ""
        Logic_tbPeriodFrom.Value = 0
        Logic_tbPeriodTo.Value = 0
        Logic_tbValueAsNo.Value = 0
        Logic_tbValueAsText.Text = ""

        If Logic_ComboBoxType.Text = "Date" Then
            Logic_tbFormat.Enabled = True
            Logic_tbPeriodFrom.Enabled = False
            Logic_tbPeriodTo.Enabled = False
            Logic_tbValueAsNo.Enabled = False
            Logic_tbValueAsText.Enabled = False
        End If
        If Logic_ComboBoxType.Text = "Text" Then
            Logic_tbFormat.Enabled = True
            Logic_tbPeriodFrom.Enabled = False
            Logic_tbPeriodTo.Enabled = False
            Logic_tbValueAsNo.Enabled = False
            Logic_tbValueAsText.Enabled = True
        End If
        If Logic_ComboBoxType.Text = "Numberic (Fixed)" Then
            Logic_tbFormat.Enabled = False
            Logic_tbPeriodFrom.Enabled = False
            Logic_tbPeriodTo.Enabled = False
            Logic_tbValueAsNo.Enabled = True
            Logic_tbValueAsText.Enabled = False
        End If
        If Logic_ComboBoxType.Text = "Numberic (Period)" Then
            Logic_tbFormat.Enabled = True
            Logic_tbPeriodFrom.Enabled = True
            Logic_tbPeriodTo.Enabled = True
            Logic_tbValueAsNo.Enabled = False
            Logic_tbValueAsText.Enabled = False
        End If
        If Logic_ComboBoxType.Text = "Auto" Then
            Logic_tbFormat.Enabled = True
            Logic_tbPeriodFrom.Enabled = False
            Logic_tbPeriodTo.Enabled = False
            Logic_tbValueAsNo.Enabled = False
            Logic_tbValueAsText.Enabled = False
        End If

        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub Logic_btAdd_Click(sender As Object, e As EventArgs) Handles Logic_btAdd.Click
        ComboBox_VendorName.Text = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT DISTINCT Vendor_Name FROM LIST_DATABASE WHERE Database_Name = '" & ComboBox_DatabaseName.Text & "'")
        tbFolderPath.Text = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT DISTINCT Folder_Path FROM LIST_DATABASE WHERE Database_Name = '" & ComboBox_DatabaseName.Text & "'")
        ComboBox_ListSpecialList.Text = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT DISTINCT Table_Special_List FROM LIST_DATABASE WHERE Database_Name = '" & ComboBox_DatabaseName.Text & "'")
        tbUserUpdatingDatabase.Text = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT DISTINCT User_Updating_Database FROM LIST_DATABASE WHERE Database_Name = '" & ComboBox_DatabaseName.Text & "'")
        tbYearUpdatingDatabase.Text = Module_Letter_Management.SQL_QUERY_TO_STRING(link_application_config, "SELECT DISTINCT Year_Updating_Database FROM LIST_DATABASE WHERE Database_Name = '" & ComboBox_DatabaseName.Text & "'")

        Dim max_seq_no As Integer = Module_Letter_Management.SQL_QUERY_TO_INTEGER(link_application_config, "SELECT MAX(Logic_SequenceNo) FROM LIST_DATABASE WHERE Logic_Frequency = '" & Logic_ComboBox_Frequency.Text & "' AND Database_Name = '" & ComboBox_DatabaseName.Text & "'")

        If Logic_ComboBoxType.Text = "Auto" Or Logic_ComboBoxType.Text = "Numberic (Period)" Then
            If Module_Letter_Management.SQL_QUERY_TO_INTEGER(link_application_config, "SELECT COUNT(Logic_Type) FROM LIST_DATABASE WHERE Logic_Type = 'Auto' AND Logic_Frequency = '" & Logic_ComboBox_Frequency.Text & "' AND [Database_Name]='" & ComboBox_DatabaseName.Text & "'") = 1 Then
                MsgBox("Type 'Auto' can only appear once", vbCritical)
                Exit Sub
            End If
            If Module_Letter_Management.SQL_QUERY_TO_INTEGER(link_application_config, "SELECT COUNT(Logic_Type) FROM LIST_DATABASE WHERE Logic_Type = 'Numberic (Period)' AND Logic_Frequency = '" & Logic_ComboBox_Frequency.Text & "' AND [Database_Name]='" & ComboBox_DatabaseName.Text & "'") = 1 Then
                MsgBox("Type 'Numberic (Period)' can only appear once", vbCritical)
                Exit Sub
            End If
        End If

        Dim Str_SQL_Add_Record As String = "INSERT INTO LIST_DATABASE ([Vendor_Name], [Database_Name], [Folder_Path], [Table_Special_List], [User_Updating_Database], [Year_Updating_Database], [Logic_Frequency], [Logic_SequenceNo], [Logic_Type], [Logic_Value_As_Number], [Logic_Value_As_Text], [Logic_Period_From], [Logic_Period_To], [Logic_Format]) VALUES ('" & ComboBox_VendorName.Text & "', '" & ComboBox_DatabaseName.Text & "', '" & tbFolderPath.Text & "', '" & ComboBox_ListSpecialList.Text & "', '" & tbUserUpdatingDatabase.Text & "', '" & tbYearUpdatingDatabase.Text & "', '" & Logic_ComboBox_Frequency.Text & "', " & max_seq_no + 1 & ", '" & Logic_ComboBoxType.Text & "', " & Logic_tbValueAsNo.Value & ", '" & Logic_tbValueAsText.Text & "', " & Logic_tbPeriodFrom.Value & ", " & Logic_tbPeriodTo.Value & ", '" & Logic_tbFormat.Text & "')"
        Module_Letter_Management.SQL_QUERY(link_application_config, Str_SQL_Add_Record)

        Dim dt_List_Logic As DataTable = Module_Letter_Management.SQL_QUERY_TO_DATATABLE(link_application_config, "SELECT Logic_Frequency, Logic_SequenceNo, Logic_Type, Logic_Value_As_Number, Logic_Value_As_Text, Logic_Period_From, Logic_Period_To, Logic_Format FROM LIST_DATABASE WHERE Database_Name = '" & ComboBox_DatabaseName.Text & "' ORDER BY Logic_Frequency ASC, Logic_SequenceNo ASC")
        DataGridView_LogicCreateBIll.DataSource = dt_List_Logic
    End Sub

    Private Sub Logic_btEdit_Click(sender As Object, e As EventArgs) Handles Logic_btEdit.Click
        On Error GoTo err_handle

        If ComboBox_DatabaseName.Text.Length = 0 Then Exit Sub
        If Logic_tbSequence.Text.Length = 0 Then Exit Sub

        Dim database_name As String = ComboBox_DatabaseName.Text

        Dim iRet = MsgBox("Do you want to update information for database " & ComboBox_DatabaseName.Text & "and sequence logic " & Logic_tbSequence.Text & "?", vbYesNo)
        If iRet = vbYes Then
            Dim str_update As String = "UPDATE LIST_DATABASE" &
                                         " SET [Logic_Type] = '" & Logic_ComboBoxType.Text & "'," &
                                            " [Logic_Value_As_Number] = " & Logic_tbValueAsNo.Value & "," &
                                            " [Logic_Value_As_Text] = '" & Logic_tbValueAsText.Text & "'," &
                                            " [Logic_Period_From] = " & Logic_tbPeriodFrom.Value & "," &
                                            " [Logic_Period_To] = " & Logic_tbPeriodTo.Value & "," &
                                            " [Logic_Format] = '" & Logic_tbFormat.Text & "'" &
                                         " WHERE [Database_Name] = '" & ComboBox_DatabaseName.Text & "'" &
                                         " AND [Logic_SequenceNo] = '" & CInt(Logic_tbSequence.Text) & "'" &
                                         " AND [Logic_Frequency] = '" & Logic_ComboBox_Frequency.Text & "'"

            Module_Letter_Management.SQL_QUERY(link_application_config, str_update)
            ComboBox_DatabaseName.Text = ""
            ComboBox_DatabaseName.Text = database_name
        End If
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub Logic_Delete_Click(sender As Object, e As EventArgs) Handles Logic_Delete.Click
        On Error GoTo err_handle
        If ComboBox_DatabaseName.Text.Length = 0 Then Exit Sub
        If Logic_tbSequence.Text.Length = 0 Then Exit Sub

        Dim iRet = MsgBox("Do you want to delete logic sequence " & Logic_tbSequence.Text & "?", vbYesNo)
        If iRet = vbYes Then
            Dim SQL_string As String = "DELETE FROM LIST_DATABASE WHERE [Logic_Frequency] = '" & Logic_ComboBox_Frequency.Text & "' AND [Logic_SequenceNo] = " & CInt(Logic_tbSequence.Text) & " AND [Database_Name]='" & ComboBox_DatabaseName.Text & "'"
            Module_Letter_Management.SQL_QUERY(link_application_config, SQL_string)

            Logic_ComboBoxType.Text = ""
            Logic_tbFormat.Text = ""
            Logic_tbPeriodFrom.Value = 0
            Logic_tbPeriodTo.Value = 0
            Logic_tbSequence.Text = ""
            Logic_tbValueAsNo.Value = 0
            Logic_tbValueAsText.Text = ""

            Dim dt_List_Logic As DataTable = Module_Letter_Management.SQL_QUERY_TO_DATATABLE(link_application_config, "SELECT Logic_Frequency, Logic_SequenceNo, Logic_Type, Logic_Value_As_Number, Logic_Value_As_Text, Logic_Period_From, Logic_Period_To, Logic_Format FROM LIST_DATABASE WHERE Database_Name = '" & ComboBox_DatabaseName.Text & "'")
            DataGridView_LogicCreateBIll.DataSource = dt_List_Logic

        End If
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub DataGridView_LogicCreateBIll_CellMouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView_LogicCreateBIll.CellMouseDoubleClick
        If Len(DataGridView_LogicCreateBIll.Rows(e.RowIndex).Cells(0).Value.ToString()) > 0 Then
            Logic_ComboBox_Frequency.Text = DataGridView_LogicCreateBIll.Rows(e.RowIndex).Cells(0).Value.ToString()
        Else
            Logic_ComboBox_Frequency.Text = ""
        End If
        If Len(DataGridView_LogicCreateBIll.Rows(e.RowIndex).Cells(1).Value.ToString()) > 0 Then
            Logic_tbSequence.Text = DataGridView_LogicCreateBIll.Rows(e.RowIndex).Cells(1).Value.ToString()
        Else
            Logic_tbSequence.Text = ""
        End If
        If Len(DataGridView_LogicCreateBIll.Rows(e.RowIndex).Cells(2).Value.ToString()) > 0 Then
            Logic_ComboBoxType.Text = DataGridView_LogicCreateBIll.Rows(e.RowIndex).Cells(2).Value.ToString()
        Else
            Logic_ComboBoxType.Text = ""
        End If
        If Len(DataGridView_LogicCreateBIll.Rows(e.RowIndex).Cells(3).Value.ToString()) > 0 Then
            Logic_tbValueAsNo.Value = CInt(DataGridView_LogicCreateBIll.Rows(e.RowIndex).Cells(3).Value.ToString())
        Else
            Logic_tbValueAsNo.Value = 0
        End If
        If Len(DataGridView_LogicCreateBIll.Rows(e.RowIndex).Cells(4).Value.ToString()) > 0 Then
            Logic_tbValueAsText.Text = DataGridView_LogicCreateBIll.Rows(e.RowIndex).Cells(4).Value.ToString()
        Else
            Logic_tbValueAsText.Text = ""
        End If
        If Len(DataGridView_LogicCreateBIll.Rows(e.RowIndex).Cells(5).Value.ToString()) > 0 Then
            Logic_tbPeriodFrom.Value = CInt(DataGridView_LogicCreateBIll.Rows(e.RowIndex).Cells(5).Value.ToString())
        Else
            Logic_tbPeriodFrom.Value = 0
        End If
        If Len(DataGridView_LogicCreateBIll.Rows(e.RowIndex).Cells(6).Value.ToString()) > 0 Then
            Logic_tbPeriodTo.Value = CInt(DataGridView_LogicCreateBIll.Rows(e.RowIndex).Cells(6).Value.ToString())
        Else
            Logic_tbPeriodTo.Text = 0
        End If
        If Len(DataGridView_LogicCreateBIll.Rows(e.RowIndex).Cells(7).Value.ToString()) > 0 Then
            Logic_tbFormat.Text = DataGridView_LogicCreateBIll.Rows(e.RowIndex).Cells(7).Value.ToString()
        Else
            Logic_tbFormat.Text = ""
        End If
    End Sub

    Private Sub btReport_Refresh_Click(sender As Object, e As EventArgs) Handles btReport_Refresh.Click
        On Error GoTo err_handle

        SQL_QUERY(link_application_config, "CREATE TABLE IF NOT EXISTS LIST_DATABASE_REPORT(Report_Name VARCHAR, Title_Report VARCHAR, Paper_Kind VARCHAR, WaterMark_Text VARCHAR, SQL_String VARCHAR, User_Created VARCHAR, User_Modified VARCHAR)")
        DataGridView_Report.DataSource = SQL_QUERY_TO_DATATABLE(link_application_config, "SELECT * FROM LIST_DATABASE_REPORT")

        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub btReport_Add_Click(sender As Object, e As EventArgs) Handles btReport_Add.Click
        On Error GoTo err_handle

        SQL_QUERY(link_application_config, "CREATE TABLE IF NOT EXISTS LIST_DATABASE_REPORT(Report_Name VARCHAR, Title_Report VARCHAR, Paper_Kind VARCHAR, WaterMark_Text VARCHAR, SQL_String VARCHAR, User_Created VARCHAR, User_Modified VARCHAR)")

        Dim iRet = MsgBox("Do you want to add new report?", vbYesNo)
        If iRet = vbYes Then
            If SQL_QUERY_TO_INTEGER(link_application_config, "SELECT COUNT(SQL_String) FROM LIST_DATABASE_REPORT WHERE Report_Name = '" & tbReportName.Text & "'") > 0 Then
                MsgBox("Report Name " & tbReportName.Text & " exist", vbCritical)
                Exit Sub
            End If

            Dim Str_SQL_Add_Record As String = "INSERT INTO LIST_DATABASE_REPORT ([Report_Name], [Title_Report], [Paper_Kind], [WaterMark_Text], [SQL_String], [Vendor], [User_Created]) VALUES ('" & tbReportName.Text & "', '" & tbReport_Title.Text & "', '" & ComboBox_Report_PaperKind.Text & "', '" & tbReport_WaterMark.Text & "', '" & tbReport_SQLStr.Text & "', '" & ComboBox_Report_Vendor.Text & "', '" & Environment.UserName & "_" & Now.ToString("dd/MM/yyyy hh:mm:ss tt") & "')"
            Module_Letter_Management.SQL_QUERY(link_application_config, Str_SQL_Add_Record)

            DataGridView_Report.DataSource = SQL_QUERY_TO_DATATABLE(link_application_config, "SELECT * FROM LIST_DATABASE_REPORT")
        End If

        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub btReport_Edit_Click(sender As Object, e As EventArgs) Handles btReport_Edit.Click
        On Error GoTo err_handle

        Dim iRet = MsgBox("Do you want to update details for report?", vbYesNo)
        If iRet = vbYes Then

            Dim str_update As String = "UPDATE LIST_DATABASE_REPORT" &
                                         " SET [Title_Report] = '" & tbReport_Title.Text & "'," &
                                            " [Paper_Kind] = '" & ComboBox_Report_PaperKind.Text & "'," &
                                            " [WaterMark_Text] = '" & tbReport_WaterMark.Text & "'," &
                                            " [SQL_String] = '" & tbReport_SQLStr.Text & "'," &
                                            " [Vendor] = '" & ComboBox_Report_Vendor.Text & "'," &
                                            " [User_Modified] = '" & Environment.UserName & "_" & Now.ToString("dd/MM/yyyy hh:mm:ss tt") & "'" &
                                         " WHERE [Report_Name] = '" & tbReportName.Text & "'"

            SQL_QUERY(link_application_config, str_update)
            DataGridView_Report.DataSource = SQL_QUERY_TO_DATATABLE(link_application_config, "SELECT * FROM LIST_DATABASE_REPORT")
        End If
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub btReport_Delete_Click(sender As Object, e As EventArgs) Handles btReport_Delete.Click
        On Error GoTo err_handle

        If tbReportName.Text.Length = 0 Then Exit Sub
        Dim iRet = MsgBox("Do you want to delete report " & tbReportName.Text & "?", vbYesNo)
        If iRet = vbYes Then
            Dim SQL_string As String = "DELETE FROM LIST_DATABASE_REPORT WHERE [Report_Name]='" & tbReportName.Text & "'"
            Module_Letter_Management.SQL_QUERY(link_application_config, SQL_string)
            DataGridView_Report.DataSource = SQL_QUERY_TO_DATATABLE(link_application_config, "SELECT * FROM LIST_DATABASE_REPORT")
        End If
        Exit Sub
err_handle:
        Module_Letter_Management.Error_handle()
    End Sub

    Private Sub DataGridView_Report_CellMouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView_Report.CellMouseDoubleClick
        If Len(DataGridView_Report.Rows(e.RowIndex).Cells(0).Value.ToString()) > 0 Then
            tbReportName.Text = DataGridView_Report.Rows(e.RowIndex).Cells(0).Value.ToString()
        Else
            tbReportName.Text = ""
        End If
        If Len(DataGridView_Report.Rows(e.RowIndex).Cells(1).Value.ToString()) > 0 Then
            tbReport_Title.Text = DataGridView_Report.Rows(e.RowIndex).Cells(1).Value.ToString()
        Else
            tbReport_Title.Text = ""
        End If
        If Len(DataGridView_Report.Rows(e.RowIndex).Cells(2).Value.ToString()) > 0 Then
            ComboBox_Report_PaperKind.Text = DataGridView_Report.Rows(e.RowIndex).Cells(2).Value.ToString()
        Else
            ComboBox_Report_PaperKind.Text = ""
        End If
        If Len(DataGridView_Report.Rows(e.RowIndex).Cells(3).Value.ToString()) > 0 Then
            tbReport_WaterMark.Text = DataGridView_Report.Rows(e.RowIndex).Cells(3).Value.ToString()
        Else
            tbReport_WaterMark.Text = ""
        End If
        If Len(DataGridView_Report.Rows(e.RowIndex).Cells(4).Value.ToString()) > 0 Then
            tbReport_SQLStr.Text = DataGridView_Report.Rows(e.RowIndex).Cells(4).Value.ToString()
        Else
            tbReport_SQLStr.Text = ""
        End If
        If Len(DataGridView_Report.Rows(e.RowIndex).Cells(5).Value.ToString()) > 0 Then
            ComboBox_Report_Vendor.Text = DataGridView_Report.Rows(e.RowIndex).Cells(5).Value.ToString()
        Else
            ComboBox_Report_Vendor.Text = ""
        End If
    End Sub
End Class