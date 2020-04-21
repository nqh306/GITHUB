Imports System.Threading
Public Class FormLogin
    Public appPath As String = AppDomain.CurrentDomain.BaseDirectory
    Public link_app_config As String = appPath & "App_Config.txt"
    Public link_database_user As String = SQL_QUERY_TO_STRING(link_app_config, "SELECT Field_Value FROM Config WHERE Field_Name = 'Link_Database_User'")

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        Main.BarStaticItem_User.Caption = UsernameTextBox.Text
        Dim cipherText As String = Module_DA.SQL_QUERY_TO_STRING(link_database_user, "SELECT Password_Str FROM LIST_USER WHERE Bank_ID = '" & Main.BarStaticItem_User.Caption & "'")
        Dim password_true As String = EncryptDecrypt.DecryptData(cipherText)

        If PasswordTextBox.Text = password_true Then
            check_password = True
            Main.start_thearing()
            'MsgBox("Completed", vbInformation, "Change User")
        Else
            MsgBox("Invalid password!", vbCritical)
        End If

    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub

End Class
