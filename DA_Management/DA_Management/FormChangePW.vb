Public Class FormChangePW
    Public appPath As String = AppDomain.CurrentDomain.BaseDirectory
    Public link_app_config As String = appPath & "App_Config.txt"
    Public link_database_user As String = SQL_QUERY_TO_STRING(link_app_config, "SELECT Field_Value FROM Config WHERE Field_Name = 'Link_Database_User'")
    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        If SQL_QUERY_TO_INTEGER(link_database_user, "Select Count(Status) FROM LIST_USER WHERE Bank_ID = '" & UsernameTextBox.Text & "'") = 0 Then
            MsgBox("Not found user name " & UsernameTextBox.Text & " . Please request Administrator for create.", vbCritical)
            Exit Sub
        Else
            Dim cipherText As String = Module_DA.SQL_QUERY_TO_STRING(link_database_user, "SELECT Password_Str FROM LIST_USER WHERE Bank_ID = '" & UsernameTextBox.Text & "'")
            Dim password_true As String = EncryptDecrypt.DecryptData(cipherText)

            If PasswordTextBox.Text = password_true Then
                Dim Password_store_db As String = EncryptDecrypt.EncryptData(NewPasswordTextbox.Text)
                SQL_QUERY(link_database_user, "UPDATE LIST_USER SET Password_Str = '" & Password_store_db & "' WHERE Bank_ID = '" & UsernameTextBox.Text & "'")
                MsgBox("Completed", vbInformation)
            Else
                MsgBox("Invalid old password!", vbCritical)
            End If
        End If
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub

End Class
