Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Repository
Imports Microsoft.Office.Interop

Public Class FromReport
    Private Sub FromReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        WindowsFormsSettings.AllowAutoFilterConditionChange = DevExpress.Utils.DefaultBoolean.False

        BarEditItem_Filter.EditWidth = 100
        CType(RepositoryItemComboBox1, RepositoryItemComboBox).Items.Add("Tất cả")

        For i = 0 To 100
            If CInt(Now.AddMonths(-i).ToString("yyyyMM")) > 201906 Then
                CType(RepositoryItemComboBox1, RepositoryItemComboBox).Items.Add(Now.AddMonths(-i).ToString("MM/yyyy"))
            Else
                Exit For
            End If
        Next
        BarEditItem_Filter.EditValue = Now.ToString("MM/yyyy")
    End Sub

    Private Sub BarButtonItem_Refresh_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem_Refresh.ItemClick
        Try
            If BarEditItem_Filter.EditValue.ToString.Length = 0 Then
                Exit Sub
            End If

            If BarEditItem_Filter.EditValue.ToString = "Tất cả" Then
                GridControl_DaXuLy.DataSource = SQL_QUERY_TO_DATATABLE(link_folder_database, "SELECT BANTRINH, NGAYTOTRINH, SOTOTRINH, NOIDUNGTRINH, SONGHIQUYET, NGAYNGHIQUYET, SOQUYETDINH_VANBAN, NGAYQUYETDINH_VANBAN, NGAY_YKIEN_HDTV_GANNHAT, NGUOITHUCHIEN, THOIGIANXULY, GHICHU FROM DATABASE_EOFFICE ORDER BY CAST(SUBSTR(NGAYTOTRINH,7,4) AS INT) DESC, CAST(SUBSTR(NGAYTOTRINH,4,2) AS INT) DESC, CAST(SUBSTR(NGAYTOTRINH,1,2) AS INT) DESC, SOTOTRINH ASC")
                GridControl_DangTheoDoi.DataSource = SQL_QUERY_TO_DATATABLE(link_folder_database, "SELECT BANTRINH AS [Ban Trình], NGAYTOTRINH AS [Ngày tờ trình], SOTOTRINH AS [Số tờ trình], NOIDUNGTRINH AS [Trích yếu], YKIEN_HDTV AS [Ý kiến HĐTV], NGUOITHUCHIEN AS [Người thực hiện], GHICHU AS [Ghi chú], STATUS AS [Tình trạng] FROM DATABASE_TOTRINH_THEODOI ORDER BY CAST(SUBSTR(NGAYTOTRINH,7,4) AS INT) DESC, CAST(SUBSTR(NGAYTOTRINH,4,2) AS INT) DESC, CAST(SUBSTR(NGAYTOTRINH,1,2) AS INT) DESC, SOTOTRINH ASC")
            Else
                GridControl_DaXuLy.DataSource = SQL_QUERY_TO_DATATABLE(link_folder_database, "SELECT BANTRINH, NGAYTOTRINH, SOTOTRINH, NOIDUNGTRINH, SONGHIQUYET, NGAYNGHIQUYET, SOQUYETDINH_VANBAN, NGAYQUYETDINH_VANBAN, NGAY_YKIEN_HDTV_GANNHAT, NGUOITHUCHIEN, THOIGIANXULY, GHICHU FROM DATABASE_EOFFICE WHERE NGAYTOTRINH LIKE '%" & BarEditItem_Filter.EditValue.ToString & "' OR NGAYTHUCHIEN LIKE '%" & BarEditItem_Filter.EditValue.ToString & "' OR NGAYNGHIQUYET LIKE '%" & BarEditItem_Filter.EditValue.ToString & "' OR NGAYQUYETDINH_VANBAN LIKE '%" & BarEditItem_Filter.EditValue.ToString & "' ORDER BY CAST(SUBSTR(NGAYTOTRINH,7,4) AS INT) DESC, CAST(SUBSTR(NGAYTOTRINH,4,2) AS INT) DESC, CAST(SUBSTR(NGAYTOTRINH,1,2) AS INT) DESC, SOTOTRINH ASC")
                GridControl_DangTheoDoi.DataSource = SQL_QUERY_TO_DATATABLE(link_folder_database, "SELECT BANTRINH AS [Ban Trình], NGAYTOTRINH AS [Ngày tờ trình], SOTOTRINH AS [Số tờ trình], NOIDUNGTRINH AS [Trích yếu], YKIEN_HDTV AS [Ý kiến HĐTV], NGUOITHUCHIEN AS [Người thực hiện], GHICHU AS [Ghi chú], STATUS AS [Tình trạng] FROM DATABASE_TOTRINH_THEODOI ORDER BY CAST(SUBSTR(NGAYTOTRINH,7,4) AS INT) DESC, CAST(SUBSTR(NGAYTOTRINH,4,2) AS INT) DESC, CAST(SUBSTR(NGAYTOTRINH,1,2) AS INT) DESC, SOTOTRINH ASC")
            End If
        Catch ex As Exception
            MsgBox(ex.Message, "Ban Tổng Hợp - EVNGENCO1")
        End Try
    End Sub

    Private Sub BarButtonItem_ExportToExcel_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem_ExportToExcel.ItemClick
        Try
            If GridView_DangTheoDoi.RowCount = 0 And GridView_DaXuLy.RowCount = 0 Then
                MsgBox("Chưa lấy dữ liệu", vbCritical, "Ban Tổng Hợp - EVNGENCO1")
                Exit Sub
            End If
            Dim full_path_daxuly As String = appPath & Now.ToString("yyyyMMddhhmmsstt") & "_daxuly.xls"
            GridControl_DaXuLy.ExportToXls(full_path_daxuly)

            Dim full_path_theodoi As String = appPath & Now.ToString("yyyyMMddhhmmsstt") & "_dangtheodoi.xls"
            GridControl_DangTheoDoi.ExportToXls(full_path_theodoi)

            Dim ObjExcel As Object = CreateObject("Excel.Application")
            ObjExcel.Visible = True
            Dim Objwb_dest As Object = ObjExcel.Workbooks.Add()

            If GridView_DaXuLy.RowCount > 0 Then
                Dim Objwb As Object = ObjExcel.Workbooks.open(full_path_daxuly)
                Dim Objws As Object = Objwb.Sheets("Sheet")
                Dim Objws_dest As Object = Objwb_dest.ActiveSheet()
                Objws_dest.Name = "Đã xử lý"
                Objws_dest.Cells.NumberFormat = "@"

                Objws.Cells.Copy()
                Objws_dest.Range("A1").PasteSpecial(Excel.XlPasteType.xlPasteAll)
                ObjExcel.CutCopyMode = False
                Objws_dest.Cells.Borders.LineStyle = Excel.XlLineStyle.xlLineStyleNone
                Objwb.Close(SaveChanges:=False)
            End If

            If GridView_DangTheoDoi.RowCount > 0 Then
                Dim Objwb2 As Object = ObjExcel.Workbooks.open(full_path_theodoi)
                Dim Objws2 As Object = Objwb2.Sheets("Sheet")
                Dim Objws_dest_2 As Object = Objwb_dest.sheets.add
                Objws_dest_2.Name = "Đang theo dõi"
                Objws_dest_2.Cells.NumberFormat = "@"

                Objws2.Cells.Copy()
                Objws_dest_2.Range("A1").PasteSpecial(Excel.XlPasteType.xlPasteAll)
                ObjExcel.CutCopyMode = False
                Objws_dest_2.Cells.Borders.LineStyle = Excel.XlLineStyle.xlLineStyleNone
                Objwb2.Close(SaveChanges:=False)
            End If
            If My.Computer.FileSystem.FileExists(full_path_daxuly) Then
                Kill(full_path_daxuly)
            End If
            If My.Computer.FileSystem.FileExists(full_path_theodoi) Then
                Kill(full_path_theodoi)
            End If
        Catch ex As Exception
            MsgBox(ex.Message, "Ban Tổng Hợp - EVNGENCO1")
        End Try
    End Sub
End Class