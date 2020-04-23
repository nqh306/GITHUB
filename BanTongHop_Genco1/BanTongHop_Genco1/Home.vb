Imports System.ComponentModel
Imports System.Text
Imports DevExpress.XtraBars

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

    Private Sub Home_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BarEditItem_Username.EditWidth = 100
        BarEditItem_Username.EditValue = user_login
    End Sub

    Private Sub bt_User_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bt_User.ItemClick
        ShowForm(FormUser)
    End Sub

    Private Sub Home_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Me.Hide()
        LoginForm1.Show()
    End Sub

    Private Sub BarEditItem_Username_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarEditItem_Username.ItemClick
        FormChangePW.Show()
    End Sub
End Class
