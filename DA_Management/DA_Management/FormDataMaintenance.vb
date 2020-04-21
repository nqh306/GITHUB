Imports System.Data.SQLite
Imports DevExpress.XtraBars
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid

Partial Public Class FormDataMaintenance
    Public appPath As String = AppDomain.CurrentDomain.BaseDirectory
    Public link_app_config As String = appPath & "App_Config.txt"
    Public link_database_DA As String = ""

    Dim MYCONNECTION As SQLiteConnection
    Dim ds As DataSet
    Dim da As SQLiteDataAdapter

    Public Sub Taoketnoi()
        MYCONNECTION = New SQLiteConnection("DataSource=" & link_database_DA & ";version=3;new=False;datetimeformat=CurrentCulture;")
        MYCONNECTION.Open()
    End Sub

    Public Sub Dongketnoi()
        MYCONNECTION.Close()
    End Sub

    Public Function LayDulieu() As DataSet
        Taoketnoi()
        da = New SQLiteDataAdapter("SELECT * FROM Main_DA", MYCONNECTION)
        ds = New DataSet
        da.FillSchema(ds, SchemaType.Source, "Main_DA")
        da.Fill(ds, "Main_DA")
        da.AcceptChangesDuringFill = False
        Return ds
    End Function

    Private Sub bbiPrintPreview_ItemClick(ByVal sender As Object, ByVal e As ItemClickEventArgs) Handles bbiPrintPreview.ItemClick
        GridControl.ShowRibbonPrintPreview()
    End Sub

    Private Sub FormDataMaintenance_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GridView.ShowFindPanel()
        'ẩn icon trên row filter của grid control
        WindowsFormsSettings.AllowAutoFilterConditionChange = DevExpress.Utils.DefaultBoolean.False

        Dim folder_path As String = SQL_QUERY_TO_STRING(link_app_config, "SELECT Field_Value FROM Config WHERE Field_Name = 'Link_Folder_Path_DA'")
        If folder_path.Substring(folder_path.Length - 1, 1) <> "\" Then
            folder_path = folder_path & "\"
        End If
        link_database_DA = folder_path & "Database_DA.txt"

        GridControl.DataSource = LayDulieu.Tables("Main_DA")
        GridView.BestFitColumns()
        bsiRecordsCount.Caption = "RECORD: " & GridView.RowCount
    End Sub

    Private Sub bbiNew_ItemClick(sender As Object, e As ItemClickEventArgs) Handles bbiNew.ItemClick
        GridView.AddNewRow()
        GridView.OptionsNavigation.AutoFocusNewRow = True
    End Sub

    Private Sub bbiDelete_ItemClick(sender As Object, e As ItemClickEventArgs) Handles bbiDelete.ItemClick
        Dim iRet = MsgBox("Do you want to delete this record?", vbYesNo + vbQuestion)

        If iRet = vbYes Then
            GridView.DeleteSelectedRows()
            Dim cb As New SQLiteCommandBuilder(da)
            da.Update(ds.Tables("Main_DA"))
            ds.AcceptChanges()
            bsiRecordsCount.Caption = "RECORD: " & GridView.RowCount
        End If
    End Sub

    Private Sub bbiRefresh_ItemClick(sender As Object, e As ItemClickEventArgs) Handles bbiRefresh.ItemClick

        If ds.HasChanges Then
            Dim iRet = MsgBox("Do you want to save this change?", vbYesNo + vbQuestion)

            If iRet = vbYes Then
                Dim cb As New SQLiteCommandBuilder(da)
                da.Update(ds.Tables("Main_DA"))
                ds.AcceptChanges()
                MsgBox("Saved", vbInformation + vbOKOnly)
            End If
        End If
    End Sub

    Private Sub BarButtonItem_ExportToExcel_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarButtonItem_ExportToExcel.ItemClick
        On Error GoTo err_handle
        EXPORT_DATAGRIDVIEW_TO_EXCEL(GridView)
        Exit Sub
err_handle:
        Error_handle()
    End Sub

    Private Sub BarButtonItem_ImportFromExcel_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarButtonItem_ImportFromExcel.ItemClick
        On Error GoTo err_handle

        IMPORT_INTO_DATABASE_FROM_EXCEL(link_database_DA, "Main_DA")

        MsgBox("Completed", vbOKOnly + vbInformation)

        Exit Sub
err_handle:
        Error_handle()
    End Sub

    Private Sub GridControl1_ProcessGridKey(sender As Object, e As KeyEventArgs) Handles GridControl.ProcessGridKey
        Dim grid = TryCast(sender, GridControl)
        Dim view = TryCast(grid.FocusedView, GridView)
        If e.KeyData = Keys.Delete Then
            Dim iRet = MsgBox("Do you want to delete this record?", vbYesNo + vbQuestion)

            If iRet = vbYes Then
                view.DeleteSelectedRows()
                e.Handled = True
                da.Update(ds.Tables("Main_DA"))
                ds.AcceptChanges()
            End If
        End If
    End Sub

    Private Sub BarButtonItem_Refresh_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarButtonItem_Refresh.ItemClick
        GridControl.DataSource = LayDulieu.Tables("Main_DA")
        GridView.BestFitColumns()
        bsiRecordsCount.Caption = "RECORD: " & GridView.RowCount
    End Sub
End Class
