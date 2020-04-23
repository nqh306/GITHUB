Imports DevExpress.XtraBars
Imports System.Data.SQLite
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraEditors
Imports System.ComponentModel

Partial Public Class FormUser
    Public appPath As String = AppDomain.CurrentDomain.BaseDirectory
    Public link_database_user As String = link_folder_database

    Private Sub bbiPrintPreview_ItemClick(ByVal sender As Object, ByVal e As ItemClickEventArgs) Handles bbiPrintPreview.ItemClick
        gridControl.ShowRibbonPrintPreview()
    End Sub

    Private Sub FormUser_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'ẩn icon trên row filter của grid control
        WindowsFormsSettings.AllowAutoFilterConditionChange = DevExpress.Utils.DefaultBoolean.False

        gridControl.DataSource = SQL_QUERY_TO_DATATABLE(link_database_user, "SELECT * FROM DATABASE_USER")
    End Sub

    'Private Sub gridControl_ProcessGridKey(sender As Object, e As KeyEventArgs) Handles gridControl.ProcessGridKey
    '    Dim grid = TryCast(sender, GridControl)
    '    Dim view = TryCast(grid.FocusedView, GridView)
    '    If e.KeyData = Keys.Delete Then
    '        Dim iRet = MsgBox("Do you want to delete this user?", vbYesNo + vbQuestion)

    '        If iRet = vbYes Then
    '            view.DeleteSelectedRows()
    '            e.Handled = True
    '            da.Update(ds.Tables("DATABASE_USER"))
    '            ds.AcceptChanges()
    '        End If
    '    End If
    'End Sub

    Private Sub gridView_RowUpdated(sender As Object, e As RowObjectEventArgs) Handles gridView.RowUpdated
        Dim Password_store_db As String = EncryptDecrypt.EncryptData("123")

        Dim maker As String = gridView.GetFocusedRowCellValue("MAKER").ToString
        If maker = "" Then maker = "False"
        Dim checker As String = gridView.GetFocusedRowCellValue("CHECKER").ToString
        If checker = "" Then checker = "False"
        Dim admin As String = gridView.GetFocusedRowCellValue("ADMIN").ToString
        If admin = "" Then admin = "False"
        Dim status As String = gridView.GetFocusedRowCellValue("STATUS").ToString
        If status = "" Then status = "False"
        Dim FULLNAME As String = gridView.GetFocusedRowCellValue("FULLNAME").ToString

        If SQL_QUERY_TO_INTEGER(link_database_user, "SELECT COUNT(*) FROM DATABASE_USER WHERE USERNAME = '" & gridView.GetFocusedRowCellValue("USERNAME").ToString & "'") = 0 Then
            SQL_QUERY(link_database_user, True, "INSERT INTO DATABASE_USER(USERNAME, FULLNAME, MAKER, CHECKER, ADMIN, STATUS, Password_Str) VALUES ('" & gridView.GetFocusedRowCellValue("USERNAME").ToString & "','" & FULLNAME & "', '" & maker & "','" & checker & "','" & admin & "','" & status & "','" & Password_store_db & "')")
        Else
            SQL_QUERY(link_database_user, True, "UPDATE DATABASE_USER SET MAKER = '" & maker & "', FULLNAME = '" & FULLNAME & "', CHECKER = '" & checker & "', ADMIN = '" & admin & "', STATUS = '" & status & "' WHERE USERNAME = '" & gridView.GetFocusedRowCellValue("USERNAME").ToString & "'")
        End If

        MsgBox("Hoàn thành!!!", vbInformation, "Ban Tổng Hợp - EVNGENCO1")
    End Sub

    Private Sub btDelete_Click(sender As Object, e As EventArgs) Handles btDelete.Click
        Dim iRet = MsgBox("Bạn có muốn xóa user [" & gridView.GetFocusedRowCellValue("USERNAME").ToString & "] không?", vbYesNo + vbQuestion, "Ban Tổng Hợp - EVNGENCO1")

        If iRet = vbYes Then
            SQL_QUERY(link_database_user, True, "DELETE FROM DATABASE_USER WHERE USERNAME = '" & gridView.GetFocusedRowCellValue("USERNAME").ToString & "'")
            gridView.DeleteSelectedRows()
            MsgBox("Hoàn thành!!!", vbInformation, "Ban Tổng Hợp - EVNGENCO1")
        End If
    End Sub

    Private Sub bt_Add_Click(sender As Object, e As EventArgs) Handles bt_Add.Click
        gridView.AddNewRow()
        gridView.OptionsNavigation.AutoFocusNewRow = True
    End Sub

    Private Sub btAdd_ItemClick(sender As Object, e As ItemClickEventArgs) Handles btAdd.ItemClick
        gridView.AddNewRow()
        gridView.OptionsNavigation.AutoFocusNewRow = True
    End Sub

    Private Sub btResetPassword_ItemClick(sender As Object, e As ItemClickEventArgs) Handles btResetPassword.ItemClick
        Try

            Dim user_reset_pw = InputBox("Điền username để khôi phục mật khẩu: ", "Khôi phục mật khẩu - Ban Tổng Hợp - EVNGENCO1")

            If user_reset_pw.Length = 0 Then
                Exit Sub
            End If

            If SQL_QUERY_TO_INTEGER(link_database_user, "Select COUNT(*) FROM DATABASE_USER WHERE USERNAME = '" & user_reset_pw & "'") = 0 Then
                MsgBox("Không tìm thấy username " & user_reset_pw, vbCritical, "Ban Tổng Hợp - EVNGENCO1")
                Exit Sub
            Else
                Dim Password_store_db As String = EncryptDecrypt.EncryptData("123")
                SQL_QUERY(link_database_user, True, "UPDATE DATABASE_USER SET Password_Str = '" & Password_store_db & "' WHERE Bank_ID = '" & user_reset_pw & "'")
                MsgBox("Completed", vbInformation)
            End If

        Catch ex As Exception
            MsgBox(ex.Message, vbCritical + vbOKOnly)
            WriteErrorLog(ex.ToString)
        End Try
    End Sub
End Class
