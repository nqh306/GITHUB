Public Class FormChangePW


    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        Dim dt As DataTable = SQL_QUERY_TO_DATATABLE(link_folder_database, "SELECT * FROM DATABASE_USER WHERE USERNAME = '" & UsernameTextBox.Text & "'")

        If dt.Rows.Count > 0 Then
            Dim drr As DataRow = dt.Rows(0)

            Dim oldpassword As String = drr("Password_Str").ToString

            If EncryptDecrypt.EncryptData(PasswordTextBox.Text) <> oldpassword Then
                MsgBox("Mật khẩu cũ không chính xác", vbCritical, "Ban Tổng Hợp - EVNGENCO1")
                Exit Sub
            Else
                If PasswordTextBox.Text = NewPasswordTextbox.Text Then
                    MsgBox("Mật khẩu cũ và mới không được giống nhau", vbCritical, "Ban Tổng Hợp - EVNGENCO1")
                    Exit Sub
                Else
                    Dim newpassword As String = EncryptDecrypt.EncryptData(NewPasswordTextbox.Text)
                    SQL_QUERY(link_folder_database, True, "UPDATE DATABASE_USER SET Password_Str = '" & newpassword & "' WHERE USERNAME = '" & UsernameTextBox.Text & "'")
                    MsgBox("Hoành thành!!!", vbInformation, "Ban Tổng Hợp - EVNGENCO1")
                End If
            End If
        Else
            MsgBox("Tên tài khoản không đúng !!!", vbCritical, "Ban Tổng Hợp - EVNGENCO1")
            Exit Sub
        End If
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub

End Class
