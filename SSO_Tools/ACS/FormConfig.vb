Imports System.Data.SQLite
Imports DevExpress.XtraBars
Imports DevExpress.XtraEditors

Public Class FormConfig

    Public appPath As String = AppDomain.CurrentDomain.BaseDirectory
    Public link_app_config As String = appPath & "App_Config.txt"
    Public global_app_config As String = SQL_FROMFILE_TO_STRING(local_app_config, "SELECT Field_Value_1 FROM Config WHERE Field_Name = 'Link_Global_Config'")
    Dim MYCONNECTION As SQLiteConnection
    Dim ds As DataSet
    Dim da As SQLiteDataAdapter

    Private Sub FormConfig_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'ẩn icon trên row filter của grid control
        WindowsFormsSettings.AllowAutoFilterConditionChange = DevExpress.Utils.DefaultBoolean.False

        GridView1.ShowFindPanel()

        Taoketnoi()
        Dim DT_table_name As DataTable = GET_ALL_TABLE_NAME_IN_DATABASE(MYCONNECTION)

        If DT_table_name.Rows.Count > 0 Then
            For i As Integer = 0 To DT_table_name.Rows.Count - 1
                If DT_table_name.Rows(i).Item(0).ToString <> "DATABASE_USER" Then
                    cbTableName.Items.Add(DT_table_name.Rows(i).Item(0).ToString)
                End If
            Next
        End If
    End Sub

    Private Sub cbTableName_TextChanged(sender As Object, e As EventArgs) Handles cbTableName.TextChanged
        GridControl1.DataSource = LayDulieu.Tables(cbTableName.Text)
    End Sub

    Public Sub Taoketnoi()
        MYCONNECTION = New SQLiteConnection("DataSource=" & global_app_config & ";version=3;new=False;datetimeformat=CurrentCulture;Password=0915330999;")
        MYCONNECTION.Open()
    End Sub

    Public Sub Dongketnoi()
        MYCONNECTION.Close()
    End Sub

    Public Function LayDulieu() As DataSet
        GridView1.Columns.Clear()

        da = New SQLiteDataAdapter("SELECT * FROM " & cbTableName.Text, MYCONNECTION)
        ds = New DataSet
        da.FillSchema(ds, SchemaType.Source, cbTableName.Text)
        da.Fill(ds, cbTableName.Text)
        da.AcceptChangesDuringFill = False
        Return ds
    End Function

    Private Sub BarButtonItem_Add_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarButtonItem_Add.ItemClick
        GridView1.AddNewRow()
        GridView1.OptionsNavigation.AutoFocusNewRow = True
    End Sub

    Private Sub BarButtonItem_Delete_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarButtonItem_Delete.ItemClick
        Dim iRet = MsgBox("Do you want to delete this record?", vbYesNo + vbQuestion)

        If iRet = vbYes Then
            GridView1.DeleteSelectedRows()
            Dim cb As New SQLiteCommandBuilder(da)
            da.Update(ds.Tables(cbTableName.Text))
            ds.AcceptChanges()
        End If
    End Sub

    Private Sub BarButtonItem_Save_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarButtonItem_Save.ItemClick
        If ds.HasChanges Then
            Dim iRet = MsgBox("Do you want to save this change?", vbYesNo + vbQuestion, "SSO")

            If iRet = vbYes Then
                Dim cb As New SQLiteCommandBuilder(da)
                da.Update(ds.Tables(cbTableName.Text))
                ds.AcceptChanges()
                MsgBox("Saved", vbInformation + vbOKOnly, "SSO")
            End If
        End If
    End Sub


End Class