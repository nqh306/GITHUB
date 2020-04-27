Imports DevExpress.XtraEditors.Controls
Imports System.Text.RegularExpressions


Public Class BaoCaoThamTra
    Private Sub BaoCaoThamTra_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RichEdit.Document.LoadDocument("D:\GITHUB\BanTongHop_Genco1\packages\Template_BaoCaoThamTra.docx")
    End Sub

    Private Sub tbSoToTrinh_Validated(sender As Object, e As EventArgs) Handles tbSoToTrinh.Validated
        Try
            If tbNgayTrinh.Text.Length > 0 And tbNoiDungTrinh.Text.Length > 0 And tbSoToTrinh.Text.Length > 0 Then
                If tbTomTatNoiDung.Text.Length > 0 Then
                    Dim iRet = MsgBox("Bạn có muốn thay đổi 'Tóm tắt nội dung' không?", vbYesNo + vbQuestion, "Ban Tổng Hợp - EVNGENCO1")
                    If iRet = vbYes Then
                        tbTomTatNoiDung.EditValue = "Ngày " & Format(tbNgayTrinh.EditValue, "dd/MM/yyyy") & ": TGĐ trình HĐTV Tờ trình số " & tbSoToTrinh.Text & " ngày " & Format(tbNgayTrinh.EditValue, "dd/MM/yyyy") & " về việc " & tbNoiDungTrinh.Text & "."
                    End If
                Else
                    tbTomTatNoiDung.EditValue = "Ngày " & Format(tbNgayTrinh.EditValue, "dd/MM/yyyy") & ": TGĐ trình HĐTV Tờ trình số " & tbSoToTrinh.Text & " ngày " & Format(tbNgayTrinh.EditValue, "dd/MM/yyyy") & " về việc " & tbNoiDungTrinh.Text & "."
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message, vbCritical, "Ban Tổng Hợp - EVNGENCO1")
        End Try
    End Sub

    Private Sub tbNgayTrinh_Validated(sender As Object, e As EventArgs) Handles tbNgayTrinh.Validated
        Try
            If tbNgayTrinh.Text.Length > 0 And tbNoiDungTrinh.Text.Length > 0 And tbSoToTrinh.Text.Length > 0 Then
                If tbTomTatNoiDung.Text.Length > 0 Then
                    Dim iRet = MsgBox("Bạn có muốn thay đổi 'Tóm tắt nội dung' không?", vbYesNo + vbQuestion, "Ban Tổng Hợp - EVNGENCO1")
                    If iRet = vbYes Then
                        tbTomTatNoiDung.EditValue = "Ngày " & Format(tbNgayTrinh.EditValue, "dd/MM/yyyy") & ": TGĐ trình HĐTV Tờ trình số " & tbSoToTrinh.Text & " ngày " & Format(tbNgayTrinh.EditValue, "dd/MM/yyyy") & " về việc " & tbNoiDungTrinh.Text & "."
                    End If
                Else
                    tbTomTatNoiDung.EditValue = "Ngày " & Format(tbNgayTrinh.EditValue, "dd/MM/yyyy") & ": TGĐ trình HĐTV Tờ trình số " & tbSoToTrinh.Text & " ngày " & Format(tbNgayTrinh.EditValue, "dd/MM/yyyy") & " về việc " & tbNoiDungTrinh.Text & "."
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message, vbCritical, "Ban Tổng Hợp - EVNGENCO1")
        End Try
    End Sub

    Private Sub tbNoiDungTrinh_Validated(sender As Object, e As EventArgs) Handles tbNoiDungTrinh.Validated
        Try
            If tbNgayTrinh.Text.Length > 0 And tbNoiDungTrinh.Text.Length > 0 And tbSoToTrinh.Text.Length > 0 Then
                If tbTomTatNoiDung.Text.Length > 0 Then
                    Dim iRet = MsgBox("Bạn có muốn thay đổi 'Tóm tắt nội dung' không?", vbYesNo + vbQuestion, "Ban Tổng Hợp - EVNGENCO1")
                    If iRet = vbYes Then
                        tbTomTatNoiDung.EditValue = "Ngày " & Format(tbNgayTrinh.EditValue, "dd/MM/yyyy") & ": TGĐ trình HĐTV Tờ trình số " & tbSoToTrinh.Text & " ngày " & Format(tbNgayTrinh.EditValue, "dd/MM/yyyy") & " về việc " & tbNoiDungTrinh.Text & "."
                    End If
                Else
                    tbTomTatNoiDung.EditValue = "Ngày " & Format(tbNgayTrinh.EditValue, "dd/MM/yyyy") & ": TGĐ trình HĐTV Tờ trình số " & tbSoToTrinh.Text & " ngày " & Format(tbNgayTrinh.EditValue, "dd/MM/yyyy") & " về việc " & tbNoiDungTrinh.Text & "."
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message, vbCritical, "Ban Tổng Hợp - EVNGENCO1")
        End Try
    End Sub
End Class