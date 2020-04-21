



Imports DevExpress.XtraEditors.Repository

Public Class XuLyToTrinh
    Public link_folder_database As String = "D:\App_BanTongHop\database_bantonghop.txt"

    Private Sub XuLyToTrinh_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cbFilterNguoiXuLy.EditWidth = 200
        CType(RepositoryItemComboBox_NguoiXuLy, RepositoryItemComboBox).Items.Add("Tất cả")
        CType(RepositoryItemComboBox_NguoiXuLy, RepositoryItemComboBox).Items.Add("Ngô Quốc Huy")
        CType(RepositoryItemComboBox_NguoiXuLy, RepositoryItemComboBox).Items.Add("Hoàng Văn Đạt")
        CType(RepositoryItemComboBox_NguoiXuLy, RepositoryItemComboBox).Items.Add("Hoàng Văn Long")
        CType(RepositoryItemComboBox_NguoiXuLy, RepositoryItemComboBox).Items.Add("Trương Thị Huyền Trang")
        CType(RepositoryItemComboBox_NguoiXuLy, RepositoryItemComboBox).Items.Add("Nguyễn Quang Huy")
        cbNguoiThucHien.EditValue = "Tất cả"

        cbAction.EditWidth = 200
        CType(RepositoryItemComboBox_Action, RepositoryItemComboBox).Items.Add("Tất cả")
        CType(RepositoryItemComboBox_Action, RepositoryItemComboBox).Items.Add("Chờ xử lý")
        CType(RepositoryItemComboBox_Action, RepositoryItemComboBox).Items.Add("Chờ giải trình")
        CType(RepositoryItemComboBox_Action, RepositoryItemComboBox).Items.Add("Đã xử lý")
        cbAction.EditValue = "Tất cả"

    End Sub

    Private Sub btLoad_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btLoad.ItemClick
        If cbFilterNguoiXuLy.EditValue.ToString = "Tất cả" Then
            If cbAction.EditValue.ToString = "Tất cả" Then
                GridControl1.DataSource = SQL_QUERY_TO_DATATABLE(link_folder_database, "SELECT * FROM DATABASE_EOFFICE")
            Else
                GridControl1.DataSource = SQL_QUERY_TO_DATATABLE(link_folder_database, "SELECT * FROM DATABASE_EOFFICE WHERE STATUS = '" & cbAction.EditValue.ToString & "'")
            End If
        Else
            If cbAction.EditValue.ToString = "Tất cả" Then
                GridControl1.DataSource = SQL_QUERY_TO_DATATABLE(link_folder_database, "SELECT * FROM DATABASE_EOFFICE WHERE NGUOITHUCHIEN = '" & cbFilterNguoiXuLy.EditValue.ToString & "'")
            Else
                GridControl1.DataSource = SQL_QUERY_TO_DATATABLE(link_folder_database, "SELECT * FROM DATABASE_EOFFICE WHERE NGUOITHUCHIEN = '" & cbFilterNguoiXuLy.EditValue.ToString & "' AND STATUS = '" & cbAction.EditValue.ToString & "'")
            End If
        End If

    End Sub



End Class