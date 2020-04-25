Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Grid

Public Class XuLyToTrinh
    Dim USERNAME As String = user_login

    Private Sub XuLyToTrinh_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        WindowsFormsSettings.AllowAutoFilterConditionChange = DevExpress.Utils.DefaultBoolean.False

        cbFilterNguoiXuLy.EditWidth = 200
        Dim FULLNAME As String = SQL_QUERY_TO_STRING(link_folder_database, "SELECT FULLNAME FROM DATABASE_USER WHERE USERNAME = '" & user_login & "'")
        Dim get_to_combobox As Boolean = False

        CType(RepositoryItemComboBox_NguoiXuLy, RepositoryItemComboBox).Items.Add("Tất cả")
        Dim dt As DataTable = SQL_QUERY_TO_DATATABLE(link_folder_database, "SELECT DISTINCT NGUOITHUCHIEN FROM DATABASE_EOFFICE ORDER BY NGUOITHUCHIEN")
        For Each DRR As DataRow In dt.Rows
            If DRR("NGUOITHUCHIEN").ToString.Length > 0 Then
                If FULLNAME = DRR("NGUOITHUCHIEN").ToString Then
                    get_to_combobox = True
                End If
                CType(RepositoryItemComboBox_NguoiXuLy, RepositoryItemComboBox).Items.Add(DRR("NGUOITHUCHIEN").ToString)

            End If
        Next

        If get_to_combobox = False Then
            cbFilterNguoiXuLy.EditValue = "Tất cả"
        Else
            cbFilterNguoiXuLy.EditValue = FULLNAME
        End If

        cbAction.EditWidth = 200
        CType(RepositoryItemComboBox_Action, RepositoryItemComboBox).Items.Add("Tất cả")
        CType(RepositoryItemComboBox_Action, RepositoryItemComboBox).Items.Add("Cần xử lý")
        CType(RepositoryItemComboBox_Action, RepositoryItemComboBox).Items.Add("Đã xử lý")
        cbAction.EditValue = "Tất cả"

        Dim dt2 As DataTable = SQL_QUERY_TO_DATATABLE(link_folder_database, "SELECT DISTINCT FULLNAME FROM DATABASE_USER WHERE STATUS != TRUE ORDER BY FULLNAME")
        For Each DRR As DataRow In dt2.Rows
            cbNguoiThucHien.Properties.Items.Add(DRR("FULLNAME").ToString)
        Next

    End Sub

    Private Sub btLoad_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btLoad.ItemClick
        Load_Database()
    End Sub

    Private Sub btRefresh_Click(sender As Object, e As EventArgs) Handles btRefresh.Click
        Load_Database()
        tbCaseID.Text = ""
        tbSoToTrinh.Text = ""
        tbNgayToTrinh.EditValue = ""
        tbBanTrinh.Text = ""
        tbNoiDungTrinh.Text = ""
        tbSoNghiQuyet.Text = ""
        tbNgayNghiQuyet.EditValue = ""
        tbSoQuyetDinh.Text = ""
        tbNgayQuyetDinh.EditValue = ""
        cbNguoiThucHien.Text = ""
        tbNgayThucHien.EditValue = ""
        tbThoiGianXuLy.Text = ""
        tbGhiChu.Text = ""
        tbYKienHDTV.EditValue = ""
        tbLog.EditValue = ""
    End Sub

    Sub Load_Database()
        If cbFilterNguoiXuLy.EditValue.ToString = "Tất cả" Then
            If cbAction.EditValue.ToString = "Tất cả" Then
                GridControl1.DataSource = SQL_QUERY_TO_DATATABLE(link_folder_database, "SELECT * FROM DATABASE_EOFFICE WHERE STATUS_DELETED IS NULL")
            Else
                If cbAction.EditValue.ToString = "Cần xử lý" Then
                    GridControl1.DataSource = SQL_QUERY_TO_DATATABLE(link_folder_database, "SELECT * FROM DATABASE_EOFFICE WHERE STATUS_DELETED IS NULL AND STATUS IN ('Chờ xử lý', 'Chờ giải trình')")
                Else
                    GridControl1.DataSource = SQL_QUERY_TO_DATATABLE(link_folder_database, "SELECT * FROM DATABASE_EOFFICE WHERE STATUS_DELETED IS NULL AND STATUS = '" & cbAction.EditValue.ToString & "'")
                End If
            End If
        Else
            If cbAction.EditValue.ToString = "Tất cả" Then
                GridControl1.DataSource = SQL_QUERY_TO_DATATABLE(link_folder_database, "SELECT * FROM DATABASE_EOFFICE WHERE STATUS_DELETED IS NULL AND NGUOITHUCHIEN = '" & cbFilterNguoiXuLy.EditValue.ToString & "'")
            Else
                If cbAction.EditValue.ToString = "Cần xử lý" Then
                    GridControl1.DataSource = SQL_QUERY_TO_DATATABLE(link_folder_database, "SELECT * FROM DATABASE_EOFFICE WHERE STATUS_DELETED IS NULL AND NGUOITHUCHIEN = '" & cbFilterNguoiXuLy.EditValue.ToString & "' AND STATUS IN ('Chờ xử lý', 'Chờ giải trình')")
                Else
                    GridControl1.DataSource = SQL_QUERY_TO_DATATABLE(link_folder_database, "SELECT * FROM DATABASE_EOFFICE WHERE STATUS_DELETED IS NULL AND NGUOITHUCHIEN = '" & cbFilterNguoiXuLy.EditValue.ToString & "' AND STATUS = '" & cbAction.EditValue.ToString & "'")
                End If
            End If
        End If
    End Sub

    Sub Show_GridView_To_TEXTBOX()
        Try
            tbCaseID.Text = GridView1.GetFocusedRowCellDisplayText("CASEID").ToString
            tbSoToTrinh.Text = GridView1.GetFocusedRowCellDisplayText("SOTOTRINH").ToString

            Try
                tbNgayToTrinh.EditValue = Format(DateTime.ParseExact(GridView1.GetFocusedRowCellDisplayText("NGAYTOTRINH").ToString, "dd/MM/yyyy", Nothing), "dd/MM/yyyy")
            Catch ex As Exception
                tbNgayToTrinh.EditValue = ""
            End Try

            tbBanTrinh.Text = GridView1.GetFocusedRowCellDisplayText("BANTRINH").ToString
            tbNoiDungTrinh.Text = GridView1.GetFocusedRowCellDisplayText("NOIDUNGTRINH").ToString
            tbSoNghiQuyet.Text = GridView1.GetFocusedRowCellDisplayText("SONGHIQUYET").ToString


            Try
                tbNgayNghiQuyet.EditValue = Format(DateTime.ParseExact(GridView1.GetFocusedRowCellDisplayText("NGAYNGHIQUYET").ToString, "dd/MM/yyyy", Nothing), "dd/MM/yyyy")
            Catch ex As Exception
                tbNgayNghiQuyet.EditValue = ""
            End Try

            tbSoQuyetDinh.Text = GridView1.GetFocusedRowCellDisplayText("SOQUYETDINH_VANBAN").ToString
            Try
                tbNgayQuyetDinh.EditValue = Format(DateTime.ParseExact(GridView1.GetFocusedRowCellDisplayText("NGAYQUYETDINH_VANBAN").ToString, "dd/MM/yyyy", Nothing), "dd/MM/yyyy")
            Catch ex As Exception
                tbNgayQuyetDinh.EditValue = ""
            End Try

            cbNguoiThucHien.Text = GridView1.GetFocusedRowCellDisplayText("NGUOITHUCHIEN").ToString

            Try
                tbNgayThucHien.EditValue = Format(DateTime.ParseExact(GridView1.GetFocusedRowCellDisplayText("NGAYTHUCHIEN").ToString, "dd/MM/yyyy", Nothing), "dd/MM/yyyy")
            Catch ex As Exception
                tbNgayThucHien.EditValue = ""
            End Try
            tbThoiGianXuLy.Text = GridView1.GetFocusedRowCellDisplayText("THOIGIANXULY").ToString
            tbGhiChu.Text = Replace(GridView1.GetFocusedRowCellDisplayText("GHICHU").ToString, Char.ConvertFromUtf32(10), Environment.NewLine)

            tbYKienHDTV.EditValue = Replace(GridView1.GetFocusedRowCellDisplayText("YKIEN_HDTV").ToString, Char.ConvertFromUtf32(10), Environment.NewLine)
            tbLog.EditValue = Replace(GridView1.GetFocusedRowCellDisplayText("LOG").ToString, Char.ConvertFromUtf32(10), Environment.NewLine)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Ban Tổng Hợp - EVNGENCO1")
        End Try
    End Sub

    Private Sub GridView1_RowCellClick(sender As Object, e As RowCellClickEventArgs) Handles GridView1.RowCellClick
        Show_GridView_To_TEXTBOX()
    End Sub

    Private Sub GridView1_KeyUp(sender As Object, e As KeyEventArgs) Handles GridView1.KeyUp
        Show_GridView_To_TEXTBOX()
    End Sub

    Private Sub btXuLyNhanh_Click(sender As Object, e As EventArgs) Handles btXuLyNhanh.Click
        Try
            For i As Integer = 0 To GridView1.RowCount - 1
                If GridView1.IsRowSelected(i) = True Then
                    Dim STR_LOG As String = GridView1.GetFocusedRowCellDisplayText("LOG").ToString & Chr(10) & USERNAME & "_" & Now.ToString("dd/MM/yyyy hh:mm:ss") & "- Chuyển trạng thái tờ trình sang [Đã xử lý]"

                    SQL_QUERY(link_folder_database, True, "UPDATE DATABASE_EOFFICE SET LOG = '" & STR_LOG & "', STATUS = 'Đã xử lý' WHERE CASEID = '" & GridView1.GetRowCellValue(i, "CASEID").ToString & "'")
                End If
            Next

            Load_Database()
            MsgBox("Hoàn thành !!!", vbInformation, "Ban Tổng Hợp - EVNGENCO1")

        Catch ex As Exception
            MsgBox(ex.Message, vbCritical, "Ban Tổng Hợp - EVNGENCO1")
        End Try


    End Sub

    Private Sub btEdit_Click(sender As Object, e As EventArgs) Handles btEdit.Click
        If tbCaseID.Text.Length = 0 Then
            Exit Sub
        End If

        Dim IRET = MsgBox("Bạn có muốn cập nhật thông tin cho tờ trình [" & tbSoToTrinh.Text & "] không?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Ban Tổng Hợp - EVNGENCO1")

        If IRET = vbNo Then
            Exit Sub
        End If

        Try
            Dim DT As DataTable = SQL_QUERY_TO_DATATABLE(link_folder_database, "SELECT * FROM DATABASE_EOFFICE WHERE CASEID = '" & tbCaseID.Text & "'")
            Dim STR_UPDATE_DATABASE As String = ""
            Dim STR_LOG As String = DT.Rows(0).Item("LOG").ToString & Chr(10) & USERNAME & "_" & Now.ToString("dd/MM/yyyy hh:mm:ss") & ": Cập nhật thông tin bản ghi"

            Dim SOTOTRINH As String = DT.Rows(0).Item("SOTOTRINH").ToString
            If SOTOTRINH <> tbSoToTrinh.Text Then
                STR_UPDATE_DATABASE = STR_UPDATE_DATABASE & ",SOTOTRINH = '" & Replace(Replace(tbSoToTrinh.Text, Chr(34), ""), "'", "") & "'"
                STR_LOG = STR_LOG & Chr(10) & "- Cập nhật số tờ trình từ [" & SOTOTRINH & "] thành [" & tbSoToTrinh.Text & "]"
            End If

            Dim NGAYTOTRINH As String = DT.Rows(0).Item("NGAYTOTRINH").ToString
            If NGAYTOTRINH <> tbNgayToTrinh.Text Then
                STR_UPDATE_DATABASE = STR_UPDATE_DATABASE & ",NGAYTOTRINH = '" & Replace(Replace(tbNgayToTrinh.Text, Chr(34), ""), "'", "") & "'"
                STR_LOG = STR_LOG & Chr(10) & "- Cập nhật ngày tờ trình từ [" & NGAYTOTRINH & "] thành [" & tbNgayToTrinh.Text & "]"
            End If

            Dim BANTRINH As String = DT.Rows(0).Item("BANTRINH").ToString
            If BANTRINH <> tbBanTrinh.Text Then
                STR_UPDATE_DATABASE = STR_UPDATE_DATABASE & ",BANTRINH = '" & Replace(Replace(tbBanTrinh.Text, Chr(34), ""), "'", "") & "'"
                STR_LOG = STR_LOG & Chr(10) & "- Cập nhật ban trình từ [" & BANTRINH & "] thành [" & tbBanTrinh.Text & "]"
            End If

            Dim NOIDUNGTRINH As String = DT.Rows(0).Item("NOIDUNGTRINH").ToString
            If NOIDUNGTRINH <> tbNoiDungTrinh.Text Then
                STR_UPDATE_DATABASE = STR_UPDATE_DATABASE & ",NOIDUNGTRINH = '" & Replace(Replace(tbNoiDungTrinh.Text, Chr(34), ""), "'", "") & "'"
                STR_LOG = STR_LOG & Chr(10) & "- Cập nhật nội dung trình từ [" & NOIDUNGTRINH & "] thành [" & tbNoiDungTrinh.Text & "]"
            End If

            Dim SONGHIQUYET As String = DT.Rows(0).Item("SONGHIQUYET").ToString
            If SONGHIQUYET <> tbSoNghiQuyet.Text Then
                STR_UPDATE_DATABASE = STR_UPDATE_DATABASE & ",SONGHIQUYET = '" & Replace(Replace(tbSoNghiQuyet.Text, Chr(34), ""), "'", "") & "'"
                STR_LOG = STR_LOG & Chr(10) & "- Cập nhật số nghị quyết từ [" & SONGHIQUYET & "] thành [" & tbSoNghiQuyet.Text & "]"
            End If

            Dim NGAYNGHIQUYET As String = DT.Rows(0).Item("NGAYNGHIQUYET").ToString
            If NGAYNGHIQUYET <> tbNgayNghiQuyet.Text Then
                STR_UPDATE_DATABASE = STR_UPDATE_DATABASE & ",NGAYNGHIQUYET = '" & Replace(Replace(tbNgayNghiQuyet.Text, Chr(34), ""), "'", "") & "'"
                STR_LOG = STR_LOG & Chr(10) & "- Cập nhật ngày nghị quyết từ [" & NGAYNGHIQUYET & "] thành [" & tbNgayNghiQuyet.Text & "]"
            End If

            Dim SOQUYETDINH_VANBAN As String = DT.Rows(0).Item("SOQUYETDINH_VANBAN").ToString
            If SOQUYETDINH_VANBAN <> tbSoQuyetDinh.Text Then
                STR_UPDATE_DATABASE = STR_UPDATE_DATABASE & ",SOQUYETDINH_VANBAN = '" & Replace(Replace(tbSoQuyetDinh.Text, Chr(34), ""), "'", "") & "'"
                STR_LOG = STR_LOG & Chr(10) & "- Cập nhật số quyết định/ văn bản từ [" & SOQUYETDINH_VANBAN & "] thành [" & tbSoQuyetDinh.Text & "]"
            End If

            Dim NGAYQUYETDINH_VANBAN As String = DT.Rows(0).Item("NGAYQUYETDINH_VANBAN").ToString
            If NGAYQUYETDINH_VANBAN <> tbNgayQuyetDinh.Text Then
                STR_UPDATE_DATABASE = STR_UPDATE_DATABASE & ",NGAYQUYETDINH_VANBAN = '" & Replace(Replace(tbNgayQuyetDinh.Text, Chr(34), ""), "'", "") & "'"
                STR_LOG = STR_LOG & Chr(10) & "- Cập nhật ngày quyết định/ văn bản từ [" & NGAYQUYETDINH_VANBAN & "] thành [" & tbNgayQuyetDinh.Text & "]"
            End If

            Dim NGUOITHUCHIEN As String = DT.Rows(0).Item("NGUOITHUCHIEN").ToString
            If NGUOITHUCHIEN <> cbNguoiThucHien.Text Then
                STR_UPDATE_DATABASE = STR_UPDATE_DATABASE & ",NGUOITHUCHIEN = '" & Replace(Replace(cbNguoiThucHien.Text, Chr(34), ""), "'", "") & "'"
                STR_LOG = STR_LOG & Chr(10) & "- Cập nhật người thực hiện từ [" & NGUOITHUCHIEN & "] thành [" & cbNguoiThucHien.Text & "]"
            End If

            Dim NGAYTHUCHIEN As String = DT.Rows(0).Item("NGAYTHUCHIEN").ToString
            If NGAYTHUCHIEN <> tbNgayThucHien.Text Then
                STR_UPDATE_DATABASE = STR_UPDATE_DATABASE & ",NGAYTHUCHIEN = '" & Replace(Replace(tbNgayThucHien.Text, Chr(34), ""), "'", "") & "'"
                STR_LOG = STR_LOG & Chr(10) & "- Cập nhật ngày thực hiện từ [" & NGAYTHUCHIEN & "] thành [" & tbNgayThucHien.Text & "]"
            End If

            Dim THOIGIANXULY As String = DT.Rows(0).Item("THOIGIANXULY").ToString
            If THOIGIANXULY <> tbThoiGianXuLy.Text Then
                STR_UPDATE_DATABASE = STR_UPDATE_DATABASE & ",THOIGIANXULY = '" & Replace(Replace(tbThoiGianXuLy.Text, Chr(34), ""), "'", "") & "'"
                STR_LOG = STR_LOG & Chr(10) & "- Cập nhật thời gian xử lý từ [" & THOIGIANXULY & "] thành [" & tbThoiGianXuLy.Text & "]"
            End If

            Dim GHICHU As String = DT.Rows(0).Item("GHICHU").ToString
            If GHICHU <> tbGhiChu.Text Then
                STR_UPDATE_DATABASE = STR_UPDATE_DATABASE & ",GHICHU = '" & Replace(Replace(tbGhiChu.Text, Chr(34), ""), "'", "") & "'"
                STR_LOG = STR_LOG & Chr(10) & "- Cập nhật ghi chú từ [" & GHICHU & "] thành [" & tbGhiChu.Text & "]"
            End If

            STR_LOG = STR_LOG & Chr(10) & "- Chuyển trạng thái tờ trình sang [Đã xử lý]"

            If STR_UPDATE_DATABASE.Length > 0 Then
                STR_UPDATE_DATABASE = STR_UPDATE_DATABASE.Substring(1, Len(STR_UPDATE_DATABASE) - 1) & ", LOG = '" & STR_LOG & "'"
            Else
                If DT.Rows(0).Item("STATUS").ToString <> "Đã xử lý" Then
                    Dim iRet2 = MsgBox("Không có thông tin được cập nhật cho tờ trình [" & tbSoToTrinh.Text & "]" & Chr(10) & Chr(10) & "Bạn có muốn tiếp tục xử lý tờ trình này không?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Ban Tổng Hợp - EVNGENCO1")
                    If iRet2 = vbNo Then
                        Exit Sub
                    Else
                        SQL_QUERY(link_folder_database, True, "UPDATE DATABASE_EOFFICE SET LOG = '" & STR_LOG & "', STATUS = 'Đã xử lý' WHERE CASEID = '" & tbCaseID.Text & "'")
                        MsgBox("Hoàn thành!!!", MsgBoxStyle.Information, "Ban Tổng Hợp - EVNGENCO1")
                        Load_Database()
                        Exit Sub
                    End If
                Else
                    MsgBox("Không có thông tin được cập nhật cho tờ trình [" & tbSoToTrinh.Text & "]", MsgBoxStyle.Critical, "Ban Tổng Hợp - EVNGENCO1")
                    Exit Sub
                End If
            End If

            SQL_QUERY(link_folder_database, True, "UPDATE DATABASE_EOFFICE SET " & STR_UPDATE_DATABASE & ", STATUS = 'Đã xử lý' WHERE CASEID = '" & tbCaseID.Text & "'")
            Load_Database()
            MsgBox("Đã cập nhật thông tin cho tờ trinh [" & tbSoToTrinh.Text & "] !!!", MsgBoxStyle.Information, "Ban Tổng Hợp - EVNGENCO1")
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Ban Tổng Hợp - EVNGENCO1")
        End Try
    End Sub

    Private Sub btAdd_Click(sender As Object, e As EventArgs) Handles btAdd.Click
        If tbSoToTrinh.Text.Length = 0 Or tbNgayToTrinh.Text.Length = 0 Then
            MsgBox("Không có thông tin Số tờ trình và Ngày tờ trình", MsgBoxStyle.Critical, "Ban Tổng Hợp - EVNGENCO1")
            Exit Sub
        End If

        Dim iRet = MsgBox("Bạn có muốn thêm tờ trình [" & tbSoToTrinh.Text & "] không?", vbYesNo + vbQuestion, "Ban Tổng Hợp - EVNGENCO1")
        If iRet = vbYes Then

            Dim STR_LOG As String = USERNAME & "_" & Now.ToString("yyyy/MM/dd hh:mm:ss") & ": Khởi tạo bản ghi"
            Dim STR_STATUS As String = ""
            Dim STR_REMARKS As String = ""

            If tbSoNghiQuyet.Text.Length = 0 And tbSoQuyetDinh.Text.Length = 0 Then
                STR_STATUS = "Chờ giải trình"
                STR_REMARKS = "Không ban hành Nghị quyết/Văn bản"
            Else
                If CInt(tbThoiGianXuLy.Text) < 0 Then
                    STR_STATUS = "Chờ giải trình"
                    STR_REMARKS = "Thời gian xử lý không đạt yêu cầu"
                Else
                    If CInt(tbThoiGianXuLy.Text) > 15 Then
                        STR_STATUS = "Chờ giải trình"
                        STR_REMARKS = "Thời gian xử lý không đạt yêu cầu"
                    Else
                        If (tbSoNghiQuyet.Text.Length > 0 And tbNgayNghiQuyet.Text = "") Or (tbSoQuyetDinh.Text.Length > 0 And tbNgayQuyetDinh.Text = "") Then
                            STR_STATUS = "Chờ giải trình"
                            STR_REMARKS = "Không tìm thấy thông tin văn bản đã ban hành"
                        Else
                            STR_STATUS = "Chờ xử lý"
                        End If
                    End If
                End If
            End If


            SQL_QUERY(link_folder_database, True, "INSERT INTO DATABASE_EOFFICE(CASEID, SOTOTRINH, NGAYTOTRINH, BANTRINH, NOIDUNGTRINH, SOVANBAN, SONGHIQUYET, NGAYNGHIQUYET, SOQUYETDINH_VANBAN, NGAYQUYETDINH_VANBAN, YKIEN_HDTV, NGAY_YKIEN_HDTV_GANNHAT, NGUOITHUCHIEN, NGAYTHUCHIEN, THOIGIANXULY, GHICHU, LOG, STATUS, USER_CREATED, LAST_USER_MODIFIED, REMARKS) " &
                                                    "VALUES ('" & Now.ToString("yyyyMMddhhmmss") & "_" & USERNAME & "_manual', '" & Replace(Replace(tbSoToTrinh.Text, Chr(34), ""), "'", "") & "', '" & Replace(Replace(tbNgayToTrinh.Text, Chr(34), ""), "'", "") & "', '" & Replace(Replace(tbBanTrinh.Text, Chr(34), ""), "'", "") & "', '" & Replace(Replace(tbNoiDungTrinh.Text, Chr(34), ""), "'", "") & "', '" & Replace(Replace(tbSoNghiQuyet.Text, Chr(34), ""), "'", "") & "," & Replace(Replace(tbSoQuyetDinh.Text, Chr(34), ""), "'", "") & "', '" & Replace(Replace(tbSoNghiQuyet.Text, Chr(34), ""), "'", "") & "', '" & Replace(Replace(tbNgayNghiQuyet.Text, Chr(34), ""), "'", "") & "', '" & Replace(Replace(tbSoQuyetDinh.Text, Chr(34), ""), "'", "") & "', '" & Replace(Replace(tbNgayQuyetDinh.Text, Chr(34), ""), "'", "") & "', '" & Replace(Replace(tbYKienHDTV.EditValue.ToString, Chr(34), ""), "'", "") & "', '" & "" & "', '" & Replace(Replace(cbNguoiThucHien.Text, Chr(34), ""), "'", "") & "', '" & Replace(Replace(tbNgayThucHien.Text, Chr(34), ""), "'", "") & "', " & Replace(Replace(tbThoiGianXuLy.Text, Chr(34), ""), "'", "") & ", '" & Replace(Replace(tbGhiChu.Text, Chr(34), ""), "'", "") & "', '" & STR_LOG & "', '" & STR_STATUS & "', '" & USERNAME & "_" & Now.ToString("yyyy/MM/dd hh:mm:ss") & "', '', '" & STR_REMARKS & "')")
        End If

    End Sub

    Private Sub btDelete_Click(sender As Object, e As EventArgs) Handles btDelete.Click
        If tbCaseID.Text.Length = 0 Then
            MsgBox("Vui lòng chọn tờ trình trước khi xóa", vbCritical, "Ban Tổng Hợp - EVNGENCO1")
            Exit Sub
        End If

        Dim iRet = MsgBox("Bạn có chắc chắn muốn xóa tờ trình [" & tbSoToTrinh.Text & "] không?", vbQuestion + vbYesNo, "Ban Tổng Hợp - EVNGENCO1")
        If iRet = vbYes Then
            Dim iRet2 = MsgBox("Bạn có muốn hệ thống lấy lại tờ trình [" & tbSoToTrinh.Text & "] từ eOffice vào lần cập nhật tự động tiếp theo không?", vbQuestion + vbYesNo, "Ban Tổng Hợp - EVNGENCO1")
            If iRet2 = vbYes Then
                SQL_QUERY(link_folder_database, True, "UPDATE DATABASE_EOFFICE SET STATUS_DELETED = 'Yêu cầu tự động lấy lại' WHERE CASEID = '" & tbCaseID.Text & "'")
            Else
                SQL_QUERY(link_folder_database, True, "UPDATE DATABASE_EOFFICE SET STATUS_DELETED = 'Vĩnh viễn xóa, Không cần tự động lấy lại' WHERE CASEID = '" & tbCaseID.Text & "'")
            End If
        End If
        Load_Database()
    End Sub

End Class