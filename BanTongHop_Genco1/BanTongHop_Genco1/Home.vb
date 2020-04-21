Imports System.ComponentModel
Imports System.Text


Partial Public Class Home
    Shared Sub New()
        DevExpress.UserSkins.BonusSkins.Register()
    End Sub
    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub ShowForm(ByVal _childForm As Form)
        Dim objForms As Form
        Dim _parrentForm As Form = Me
        For Each objForms In _parrentForm.MdiChildren
            If objForms.Name = _childForm.Name Then
                _childForm.Dispose()
                _childForm = Nothing
                objForms.Show()
                objForms.Visible = True
                objForms.Focus()
                Return
            End If
        Next
        With _childForm
            .MdiParent = _parrentForm
            .Show()
        End With
        _parrentForm.WindowState = FormWindowState.Maximized
    End Sub

    Private Sub bt_XuLyToTrinh_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bt_XuLyToTrinh.ItemClick
        ShowForm(XuLyToTrinh)
    End Sub
End Class
