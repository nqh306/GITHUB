Public Class Main
    Public appPath As String = AppDomain.CurrentDomain.BaseDirectory
    Private Sub BarButton_Tasetco_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButton_Tasetco.ItemClick
        Module_Letter_Management.ShowForm(FormTasetco)
    End Sub

    Private Sub BarButton_Config_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButton_Config.ItemClick
        Module_Letter_Management.ShowForm(oo)
    End Sub

    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Clear message in status bar
        Statusbar_item1.Caption = ""
        Statusbar_item2.Caption = ""
    End Sub

    Private Sub BarButton_UpdateDatabase_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButton_UpdateDatabase.ItemClick
        'Module_Letter_Management.ShowForm(FormUpdateDatabase)
        Process.Start(appPath & "UpdateLetterDatabase.exe")

    End Sub

    Private Sub BarButton_Viettel_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButton_Viettel.ItemClick
        Module_Letter_Management.ShowForm(FormViettel)
    End Sub

    Private Sub BarButton_SpecialList_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButton_SpecialList.ItemClick
        Module_Letter_Management.ShowForm(FormSpecialClient)
    End Sub

    Private Sub BarButton_SQLQuery_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButton_SQLQuery.ItemClick
        Module_Letter_Management.ShowForm(FormSQL)
    End Sub

    Private Sub BarButton_CROWN_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButton_CROWN.ItemClick
        Module_Letter_Management.ShowForm(formCROWN)
    End Sub

    Private Sub BarButtonItem_Report_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem_Report.ItemClick
        Module_Letter_Management.ShowForm(FormReport)
    End Sub
End Class