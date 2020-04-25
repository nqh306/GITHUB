Imports System.ComponentModel
Imports System.Text
Imports DevExpress.XtraBars
Imports DevExpress.XtraEditors.Controls

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
        Ribbon.Minimized = True
    End Sub

    Private Sub bt_XuLyToTrinh_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bt_XuLyToTrinh.ItemClick
        ShowForm(XuLyToTrinh)
    End Sub

    Private Sub Home_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BarEditItem_Username.EditWidth = 100
        BarEditItem_Username.EditValue = user_login

        Check_user_access()

    End Sub

    Private Sub bt_User_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bt_User.ItemClick
        ShowForm(FormUser)
    End Sub

    Private Sub Home_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Me.Hide()
        LoginForm1.Show()
    End Sub


    Sub Check_user_access()

        If SQL_QUERY_TO_BOOLEAN(link_folder_database, "SELECT ADMIN FROM DATABASE_USER WHERE USERNAME = '" & user_login & "'") = True Then
            bt_User.Enabled = True
        Else
            bt_User.Enabled = False
        End If


        'RepositoryItemButtonEdit1
        'Dim form1 As New FormReconcilePaymentInstruction
        'Dim form2 As New FormConfig
        'Dim form3 As New FormUser
        'Dim form4 As New FormChangePW
        'Dim form5 As New FormLogin

        'Dim frmCollection = System.Windows.Forms.Application.OpenForms
        'For i As Int16 = 0I To frmCollection.Count - 1I
        '    If frmCollection.Item(i).Name = form1.Name Or frmCollection.Item(i).Name = form2.Name Or frmCollection.Item(i).Name = form3.Name Or frmCollection.Item(i).Name = form4.Name Or frmCollection.Item(i).Name = form5.Name Then
        '        frmCollection.Item(i).Close()
        '    End If
        'Next i

        'Dim user_name As String = BarEditItem_User.EditValue

        'BarButtonItem_UserFunction.Enabled = False
        'BarButtonItem_PaymentInstruction.Enabled = False
        'BarButtonItem_Config.Enabled = False

        'If SQL_QUERY_TO_BOOLEAN(CONNECTION_CONFIG, "SELECT USER_MAKER FROM DATABASE_USER WHERE Bank_ID = '" & user_name & "'") = True Then
        '    BarButtonItem_PaymentInstruction.Enabled = True
        'End If

        'If SQL_QUERY_TO_BOOLEAN(CONNECTION_CONFIG, "SELECT USER_CHECKER FROM DATABASE_USER WHERE Bank_ID = '" & user_name & "'") = True Then
        '    BarButtonItem_PaymentInstruction.Enabled = True
        'End If

        'If SQL_QUERY_TO_BOOLEAN(CONNECTION_CONFIG, "SELECT USER_ADMIN FROM DATABASE_USER WHERE Bank_ID = '" & user_name & "'") = True Then
        '    BarButtonItem_Config.Enabled = True
        '    BarButtonItem_UserFunction.Enabled = True
        'End If

    End Sub

    Private Sub RepositoryItemButtonEdit1_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles RepositoryItemButtonEdit1.ButtonClick
        FormChangePW.Show()
    End Sub
End Class
