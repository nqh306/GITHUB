Imports DevExpress.XtraEditors.Controls

Public Class Config
    Public appPath As String = AppDomain.CurrentDomain.BaseDirectory
    Public link_app_config As String = appPath & "App_Config.txt"
    Private Sub Config_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        tbFolderPath.Text = SQL_QUERY_TO_STRING(link_app_config, "SELECT Field_Value FROM Config WHERE Field_Name = 'Link_Folder_Path_DA'")

        tbDatabaseUser.Text = SQL_QUERY_TO_STRING(link_app_config, "SELECT Field_Value FROM Config WHERE Field_Name = 'Link_Database_User'")

        tbLinkGlobalConfig.Text = SQL_QUERY_TO_STRING(link_app_config, "SELECT Field_Value FROM Config WHERE Field_Name = 'Link_Global_Config'")
    End Sub

    Private Sub btChange_Click(sender As Object, e As EventArgs) Handles btChange.Click
        Dim link_global_config As String = SQL_QUERY_TO_STRING(link_app_config, "SELECT Field_Value FROM Config WHERE Field_Name = 'Link_Global_Config'")

        SQL_QUERY_WRITE_LOG(link_global_config, "Update Config SET Field_value1 = '" & tbFolderPath.Text & "' WHERE Field_Name = 'Link_Folder_Path_DA'")
        MsgBox("Completed")
    End Sub

    Private Sub btChangeDatabaseUser_Click(sender As Object, e As EventArgs) Handles btChangeDatabaseUser.Click
        SQL_QUERY_WRITE_LOG(link_app_config, "Update Config SET Field_Value = '" & tbDatabaseUser.Text & "' WHERE Field_Name = 'Link_Database_User'")
        MsgBox("Completed")
    End Sub

    Private Sub tbFolderPath_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles tbFolderPath.ButtonClick
        On Error GoTo err_handle
        Dim folderDlg As New FolderBrowserDialog
        folderDlg.ShowNewFolderButton = True

        If (folderDlg.ShowDialog() = DialogResult.OK) Then
            tbFolderPath.Text = folderDlg.SelectedPath
            Dim root As Environment.SpecialFolder = folderDlg.RootFolder
        End If
        Exit Sub
err_handle:
        Error_handle()
    End Sub

    Private Sub tbDatabaseUser_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles tbDatabaseUser.ButtonClick
        On Error GoTo err_handle
        Dim fd As OpenFileDialog = New OpenFileDialog()
        fd.Title = "Select database User"
        fd.InitialDirectory = "C:\"
        fd.Filter = "SQLite Database (*.db)|*.db|SQLite Database (*.txt)|*.txt|All files (*.*)|*.*"

        fd.FilterIndex = 2
        fd.RestoreDirectory = True

        If fd.ShowDialog() = DialogResult.OK Then
            If fd.FileName.Length > 0 Then
                tbDatabaseUser.Text = fd.FileName
            End If
        End If
        Exit Sub
err_handle:
        Error_handle()
    End Sub

    Private Sub tbLinkGlobalConfig_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles tbLinkGlobalConfig.ButtonClick
        On Error GoTo err_handle
        Dim fd As OpenFileDialog = New OpenFileDialog()
        fd.Title = "Select database Global Config"
        fd.InitialDirectory = "C:\"
        fd.Filter = "SQLite Database (*.db)|*.db|SQLite Database (*.txt)|*.txt|All files (*.*)|*.*"

        fd.FilterIndex = 2
        fd.RestoreDirectory = True

        If fd.ShowDialog() = DialogResult.OK Then
            If fd.FileName.Length > 0 Then
                tbLinkGlobalConfig.Text = fd.FileName
            End If
        End If
        Exit Sub
err_handle:
        Error_handle()
    End Sub

    Private Sub btChangeGlobalConfig_Click(sender As Object, e As EventArgs) Handles btChangeGlobalConfig.Click
        SQL_QUERY_WRITE_LOG(link_app_config, "Update Config SET Field_Value = '" & tbLinkGlobalConfig.Text & "' WHERE Field_Name = 'Link_Global_Config'")
        MsgBox("Completed")
    End Sub
End Class