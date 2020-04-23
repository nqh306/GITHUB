Public Class LoginForm1
    Public link_folder_database As String = "D:\App_BanTongHop\database_bantonghop.txt"



    ' TODO: Insert code to perform custom authentication using the provided username and password 
    ' (See https://go.microsoft.com/fwlink/?LinkId=35339).  
    ' The custom principal can then be attached to the current thread's principal as follows: 
    '     My.User.CurrentPrincipal = CustomPrincipal
    ' where CustomPrincipal is the IPrincipal implementation used to perform authentication. 
    ' Subsequently, My.User will return identity information encapsulated in the CustomPrincipal object
    ' such as the username, display name, etc.

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
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

    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub

    Private Sub LoginForm1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SQL_QUERY(link_folder_database, False, "CREATE TABLE IF NOT EXISTS DATABASE_USER(USERNAME VARCHAR NOT NULL UNIQUE PRIMARY KEY, FULLNAME VARCHAR, MAKER BOOLEAN, CHECKER BOOLEAN, ADMIN BOOLEAN, STATUS BOOLEAN, Password_Str VARCHAR)")
        If SQL_QUERY_TO_INTEGER(link_folder_database, "SELECT COUNT (*) FROM DATABASE_USER WHERE USERNAME = 'admin'") = 0 Then
            SQL_QUERY(link_folder_database, False, "INSERT INTO DATABASE_USER(USERNAME, FULLNAME, MAKER, CHECKER, ADMIN, STATUS, Password_Str) VALUES ('admin', 'ADMIN', 'True', 'True', 'True', 'True', '" & EncryptDecrypt.EncryptData("admin123") & "')")
        End If
    End Sub
End Class
