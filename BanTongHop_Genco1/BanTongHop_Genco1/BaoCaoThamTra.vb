Imports DevExpress.XtraEditors.Controls
Imports System.Text.RegularExpressions


Public Class BaoCaoThamTra
    Private Sub BaoCaoThamTra_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SQL_QUERY(link_folder_database, False, "CREATE TABLE IF NOT EXISTS DATABASE_CANCUTHAMTRA(CASEID VARCHAR NOTNULL, LINHVUC VARCHAR, SOVANBAN VARCHAR, NGAYVANBAN VARCHAR, TRINHLUC VARCHAR, NOIDUNGVANBAN VARCHAR)")

        RichEdit.Document.LoadDocument("D:\CODE\GITHUB\BanTongHop_Genco1\packages\Template_BaoCaoThamTra.docx")
    End Sub

    Private Sub tbSoToTrinh_Validated(sender As Object, e As EventArgs) Handles tbSoToTrinh.Validated
        Try
            If tbNgayTrinh.Text.Length > 0 And tbNoiDungTrinh.Text.Length > 0 And tbSoToTrinh.Text.Length > 0 Then
                Dim new_tomtatnoidung As String = "Ngày " & Format(tbNgayTrinh.EditValue, "dd/MM/yyyy") & ": TGĐ trình HĐTV Tờ trình số " & tbSoToTrinh.Text & " ngày " & Format(tbNgayTrinh.EditValue, "dd/MM/yyyy") & " về việc " & tbNoiDungTrinh.Text & "."
                If tbTomTatNoiDung.Text.Length > 0 Then
                    If tbTomTatNoiDung.Text <> new_tomtatnoidung Then
                        Dim iRet = MsgBox("Bạn có muốn thay đổi [Tóm tắt nội dung] và [Kiến nghị] không?", vbYesNo + vbQuestion, "Ban Tổng Hợp - EVNGENCO1")
                        If iRet = vbYes Then
                            tbTomTatNoiDung.EditValue =
                            tbKienNghi.Text = "Trên cơ sở nội dung Tờ trình số " & tbSoToTrinh.Text & " ngày " & Format(tbNgayTrinh.EditValue, "dd/MM/yyyy") & " và qua kết quả thẩm tra này, Ban TH kính đề nghị TV HĐTV xem xét, cho ý kiến vào Phiếu lấy ý kiến về nội dung Tờ trình nêu trên"
                        End If
                    End If
                Else
                    tbTomTatNoiDung.EditValue = "Ngày " & Format(tbNgayTrinh.EditValue, "dd/MM/yyyy") & ": TGĐ trình HĐTV Tờ trình số " & tbSoToTrinh.Text & " ngày " & Format(tbNgayTrinh.EditValue, "dd/MM/yyyy") & " về việc " & tbNoiDungTrinh.Text & "."
                    tbKienNghi.Text = "Trên cơ sở nội dung Tờ trình số " & tbSoToTrinh.Text & " ngày " & Format(tbNgayTrinh.EditValue, "dd/MM/yyyy") & " và qua kết quả thẩm tra này, Ban TH kính đề nghị TV HĐTV xem xét, cho ý kiến vào Phiếu lấy ý kiến về nội dung Tờ trình nêu trên"
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message, vbCritical, "Ban Tổng Hợp - EVNGENCO1")
        End Try
    End Sub

    Private Sub tbNgayTrinh_Validated(sender As Object, e As EventArgs) Handles tbNgayTrinh.Validated
        Try
            If tbNgayTrinh.Text.Length > 0 And tbNoiDungTrinh.Text.Length > 0 And tbSoToTrinh.Text.Length > 0 Then
                Dim new_tomtatnoidung As String = "Ngày " & Format(tbNgayTrinh.EditValue, "dd/MM/yyyy") & ": TGĐ trình HĐTV Tờ trình số " & tbSoToTrinh.Text & " ngày " & Format(tbNgayTrinh.EditValue, "dd/MM/yyyy") & " về việc " & tbNoiDungTrinh.Text & "."
                If tbTomTatNoiDung.Text.Length > 0 Then
                    If tbTomTatNoiDung.Text <> new_tomtatnoidung Then
                        Dim iRet = MsgBox("Bạn có muốn thay đổi [Tóm tắt nội dung] và [Kiến nghị] không?", vbYesNo + vbQuestion, "Ban Tổng Hợp - EVNGENCO1")
                        If iRet = vbYes Then
                            tbTomTatNoiDung.EditValue =
                            tbKienNghi.Text = "Trên cơ sở nội dung Tờ trình số " & tbSoToTrinh.Text & " ngày " & Format(tbNgayTrinh.EditValue, "dd/MM/yyyy") & " và qua kết quả thẩm tra này, Ban TH kính đề nghị TV HĐTV xem xét, cho ý kiến vào Phiếu lấy ý kiến về nội dung Tờ trình nêu trên"
                        End If
                    End If
                Else
                    tbTomTatNoiDung.EditValue = "Ngày " & Format(tbNgayTrinh.EditValue, "dd/MM/yyyy") & ": TGĐ trình HĐTV Tờ trình số " & tbSoToTrinh.Text & " ngày " & Format(tbNgayTrinh.EditValue, "dd/MM/yyyy") & " về việc " & tbNoiDungTrinh.Text & "."
                    tbKienNghi.Text = "Trên cơ sở nội dung Tờ trình số " & tbSoToTrinh.Text & " ngày " & Format(tbNgayTrinh.EditValue, "dd/MM/yyyy") & " và qua kết quả thẩm tra này, Ban TH kính đề nghị TV HĐTV xem xét, cho ý kiến vào Phiếu lấy ý kiến về nội dung Tờ trình nêu trên"
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message, vbCritical, "Ban Tổng Hợp - EVNGENCO1")
        End Try
    End Sub

    Private Sub tbNoiDungTrinh_Validated(sender As Object, e As EventArgs) Handles tbNoiDungTrinh.Validated
        Try
            If tbNgayTrinh.Text.Length > 0 And tbNoiDungTrinh.Text.Length > 0 And tbSoToTrinh.Text.Length > 0 Then
                Dim new_tomtatnoidung As String = "Ngày " & Format(tbNgayTrinh.EditValue, "dd/MM/yyyy") & ": TGĐ trình HĐTV Tờ trình số " & tbSoToTrinh.Text & " ngày " & Format(tbNgayTrinh.EditValue, "dd/MM/yyyy") & " về việc " & tbNoiDungTrinh.Text & "."
                If tbTomTatNoiDung.Text.Length > 0 Then
                    If tbTomTatNoiDung.Text <> new_tomtatnoidung Then
                        Dim iRet = MsgBox("Bạn có muốn thay đổi [Tóm tắt nội dung] và [Kiến nghị] không?", vbYesNo + vbQuestion, "Ban Tổng Hợp - EVNGENCO1")
                        If iRet = vbYes Then
                            tbTomTatNoiDung.EditValue =
                            tbKienNghi.Text = "Trên cơ sở nội dung Tờ trình số " & tbSoToTrinh.Text & " ngày " & Format(tbNgayTrinh.EditValue, "dd/MM/yyyy") & " và qua kết quả thẩm tra này, Ban TH kính đề nghị TV HĐTV xem xét, cho ý kiến vào Phiếu lấy ý kiến về nội dung Tờ trình nêu trên"
                        End If
                    End If
                Else
                    tbTomTatNoiDung.EditValue = "Ngày " & Format(tbNgayTrinh.EditValue, "dd/MM/yyyy") & ": TGĐ trình HĐTV Tờ trình số " & tbSoToTrinh.Text & " ngày " & Format(tbNgayTrinh.EditValue, "dd/MM/yyyy") & " về việc " & tbNoiDungTrinh.Text & "."
                    tbKienNghi.Text = "Trên cơ sở nội dung Tờ trình số " & tbSoToTrinh.Text & " ngày " & Format(tbNgayTrinh.EditValue, "dd/MM/yyyy") & " và qua kết quả thẩm tra này, Ban TH kính đề nghị TV HĐTV xem xét, cho ý kiến vào Phiếu lấy ý kiến về nội dung Tờ trình nêu trên"
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message, vbCritical, "Ban Tổng Hợp - EVNGENCO1")
        End Try
    End Sub
End Class