Imports System.Threading
Public Class FormLogin
    Public appPath As String = AppDomain.CurrentDomain.BaseDirectory
    Public link_app_config As String = appPath & "App_Config.txt"
    Public global_app_config As String = SQL_FROMFILE_TO_STRING(local_app_config, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Link_Global_Config'")
    Public link_folder_database As String
    Public link_database_ACS As String

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        Dim cipherText As String = SQL_FROMFILE_TO_STRING(global_app_config, "SELECT Password_Str FROM DATABASE_USER WHERE Bank_ID = '" & UsernameTextBox.Text & "'")
        Dim password_true As String = EncryptDecrypt.DecryptData(cipherText)

        If PasswordTextBox.Text = password_true Then
            check_password = True
            MainForm.BarEditItem_User.EditValue = UsernameTextBox.Text
            MainForm.start_thearing()
        Else
            MsgBox("Invalid password!", vbCritical)
        End If

    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub

    Private Sub FormLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        link_folder_database = SQL_FROMFILE_TO_STRING(global_app_config, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Link_Folder_Database'")
        link_database_ACS = link_folder_database & "Database_SSO.txt"
    End Sub
End Class
