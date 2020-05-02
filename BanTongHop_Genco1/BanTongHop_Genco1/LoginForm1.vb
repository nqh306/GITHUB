Public Class LoginForm1

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click

        If Not My.Computer.FileSystem.FileExists(link_folder_database) Then
            MsgBox("Không thể kết nối được với database: " & link_folder_database, vbCritical, "Ban Tổng Hợp - EVNGENCO1")
            Exit Sub
        End If

        If UCase(UsernameTextBox.Text) = "ADMIN" And PasswordTextBox.Text = "admin123" Then
            user_login = UsernameTextBox.Text

            UsernameTextBox.Text = ""
            PasswordTextBox.Text = ""

            Me.Hide()
            Home.Show()
        Else
            If SQL_QUERY_TO_INTEGER(link_folder_database, "SELECT COUNT(*) FROM DATABASE_USER WHERE USERNAME = '" & UsernameTextBox.Text & "'") = 0 Then
                MsgBox("Không tìm thấy tài khoản [" & UsernameTextBox.Text & "]", vbCritical, "Ban Tổng Hợp - EVNGENCO1")
                Exit Sub
            Else



                Dim password As String = SQL_QUERY_TO_STRING(link_folder_database, "SELECT Password_Str FROM DATABASE_USER WHERE USERNAME = '" & UsernameTextBox.Text & "'")
                If password <> EncryptDecrypt.EncryptData(PasswordTextBox.Text) Then
                    MsgBox("Mật khẩu không chính xác !!!", vbCritical, "Ban Tổng Hợp - EVNGENCO1")
                    Exit Sub
                End If
            End If

            user_login = UsernameTextBox.Text

            UsernameTextBox.Text = ""
            PasswordTextBox.Text = ""

            Me.Hide()
            Home.Show()
        End If
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub

    Private Sub LoginForm1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Computer.FileSystem.FileExists(global_config) Then
            link_folder_database = SQL_QUERY_TO_STRING(global_config, "SELECT Field_value1 FROM CONFIG WHERE Field_name = 'Link_database'")
        End If

        If My.Computer.FileSystem.FileExists(link_folder_database) Then
            SQL_QUERY(link_folder_database, False, "CREATE TABLE IF NOT EXISTS DATABASE_USER(USERNAME VARCHAR NOT NULL UNIQUE PRIMARY KEY, FULLNAME VARCHAR, MAKER BOOLEAN, CHECKER BOOLEAN, ADMIN BOOLEAN, STATUS BOOLEAN, Password_Str VARCHAR)")
        End If
    End Sub
End Class
