Imports DevExpress.XtraBars
Imports System.Data.SQLite
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraEditors

Partial Public Class FormUser
    Public appPath As String = AppDomain.CurrentDomain.BaseDirectory
    Public link_app_config As String = appPath & "App_Config.txt"
    Public global_app_config As String = SQL_FROMFILE_TO_STRING(local_app_config, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Link_Global_Config'")
    Public link_database_user As String = ""
    Dim MYCONNECTION As SQLiteConnection
    Dim ds As DataSet
    Dim da As SQLiteDataAdapter

    Private Sub bbiPrintPreview_ItemClick(ByVal sender As Object, ByVal e As ItemClickEventArgs) Handles bbiPrintPreview.ItemClick
        gridControl.ShowRibbonPrintPreview()
    End Sub

    Private Sub FormUser_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'ẩn icon trên row filter của grid control
        WindowsFormsSettings.AllowAutoFilterConditionChange = DevExpress.Utils.DefaultBoolean.False

        Dim link_folder_database As String = SQL_FROMFILE_TO_STRING(global_app_config, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Link_Folder_Database'")
        If link_folder_database.Substring(link_folder_database.Length - 1, 1) <> "\" Then
            link_folder_database = link_folder_database & "\"
        End If

        'link_database_user = link_folder_database & "Database_SSO.txt"

        gridControl.DataSource = LayDulieu.Tables("DATABASE_USER")
    End Sub

    Sub Taoketnoi()
        MYCONNECTION = New SQLiteConnection("DataSource=" & global_app_config & ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;")
        MYCONNECTION.Open()
    End Sub

    Sub Dongketnoi()
        MYCONNECTION.Close()
    End Sub

    Public Function LayDulieu() As DataSet
        Taoketnoi()
        da = New SQLiteDataAdapter("SELECT * FROM DATABASE_USER", MYCONNECTION)
        ds = New DataSet
        da.FillSchema(ds, SchemaType.Source, "DATABASE_USER")
        da.Fill(ds, "DATABASE_USER")
        da.AcceptChangesDuringFill = False
        Return ds
    End Function

    Private Sub gridControl_ProcessGridKey(sender As Object, e As KeyEventArgs) Handles gridControl.ProcessGridKey
        Dim grid = TryCast(sender, GridControl)
        Dim view = TryCast(grid.FocusedView, GridView)
        If e.KeyData = Keys.Delete Then
            Dim iRet = MsgBox("Do you want to delete this user?", vbYesNo + vbQuestion)

            If iRet = vbYes Then
                view.DeleteSelectedRows()
                e.Handled = True
                da.Update(ds.Tables("DATABASE_USER"))
                ds.AcceptChanges()
            End If
        End If
    End Sub

    Private Sub gridView_RowUpdated(sender As Object, e As RowObjectEventArgs) Handles gridView.RowUpdated
        If ds.HasChanges Then
            Dim cb As New SQLiteCommandBuilder(da)
            da.Update(ds.Tables("DATABASE_USER"))
            ds.AcceptChanges()

            Dim db_need_to_update_pw As DataTable = SQL_QUERY_TO_DATATABLE(MYCONNECTION, "SELECT Bank_ID FROM DATABASE_USER WHERE Password_Str IS NULL")
            Dim Password_store_db As String = EncryptDecrypt.EncryptData("123")
            For Each Drr As DataRow In db_need_to_update_pw.Rows
                SQL_QUERY(MYCONNECTION, "UPDATE DATABASE_USER SET Password_Str = '" & Password_store_db & "' WHERE Bank_ID = '" & Drr("Bank_ID").ToString & "'")
            Next

        End If
    End Sub

    Private Sub btDelete_Click(sender As Object, e As EventArgs) Handles btDelete.Click
        Dim iRet = MsgBox("Do you want to delete this user?", vbYesNo + vbQuestion)

        If iRet = vbYes Then
            gridView.DeleteSelectedRows()
            Dim cb As New SQLiteCommandBuilder(da)
            da.Update(ds.Tables("DATABASE_USER"))
            ds.AcceptChanges()
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

            Dim user_reset_pw = InputBox("Please input username for reset password: ", "Reset Password")

            If user_reset_pw.Length = 0 Then
                Exit Sub
            End If

            If SQL_FROMFILE_TO_INTEGER(link_database_user, "Select Count(Status) FROM DATABASE_USER WHERE Bank_ID = '" & user_reset_pw & "'") = 0 Then
                MsgBox("Not found user name " & user_reset_pw & " . Please request Administrator for create.", vbCritical)
                Exit Sub
            Else
                Dim Password_store_db As String = EncryptDecrypt.EncryptData("123")
                SQL_QUERY(MYCONNECTION, "UPDATE DATABASE_USER SET Password_Str = '" & Password_store_db & "' WHERE Bank_ID = '" & user_reset_pw & "'")
                MsgBox("Completed", vbInformation)
            End If

        Catch ex As Exception
            MsgBox(ex.Message, vbCritical + vbOKOnly)
            WriteErrorLog(ex.ToString)
        End Try
    End Sub
End Class
