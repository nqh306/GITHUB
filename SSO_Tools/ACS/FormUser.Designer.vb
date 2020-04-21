<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Public Class FormUser
    Inherits DevExpress.XtraBars.Ribbon.RibbonForm
    ''' <summary>
    ''' Required designer variable.
    ''' </summary>
    Private components As System.ComponentModel.IContainer = Nothing

    ''' <summary>
    ''' Clean up any resources being used.
    ''' </summary>
    ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso (components IsNot Nothing) Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

#Region "Component Designer generated code"

    ''' <summary>
    ''' Required method for Designer support - do not modify
    ''' the contents of this method with the code editor.
    ''' </summary>
    Private Sub InitializeComponent()
        Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormUser))
        Dim EditorButtonImageOptions2 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Me.gridControl = New DevExpress.XtraGrid.GridControl()
        Me.gridView = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.col_Add = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.bt_Add = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.col_Delete = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.btDelete = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.BANK_ID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.USER_MAKER = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.USER_CHECKER = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.USER_ADMIN = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Function_User = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.STATUS = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ribbonControl = New DevExpress.XtraBars.Ribbon.RibbonControl()
        Me.bbiPrintPreview = New DevExpress.XtraBars.BarButtonItem()
        Me.btAdd = New DevExpress.XtraBars.BarButtonItem()
        Me.btResetPassword = New DevExpress.XtraBars.BarButtonItem()
        Me.ribbonPage1 = New DevExpress.XtraBars.Ribbon.RibbonPage()
        Me.ribbonPageGroup1 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.ribbonPageGroup2 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        CType(Me.gridControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bt_Add, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.btDelete, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ribbonControl, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'gridControl
        '
        Me.gridControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridControl.Location = New System.Drawing.Point(0, 143)
        Me.gridControl.MainView = Me.gridView
        Me.gridControl.MenuManager = Me.ribbonControl
        Me.gridControl.Name = "gridControl"
        Me.gridControl.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.bt_Add, Me.btDelete})
        Me.gridControl.Size = New System.Drawing.Size(784, 449)
        Me.gridControl.TabIndex = 2
        Me.gridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gridView})
        '
        'gridView
        '
        Me.gridView.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.gridView.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.col_Add, Me.col_Delete, Me.BANK_ID, Me.USER_MAKER, Me.USER_CHECKER, Me.USER_ADMIN, Me.STATUS, Me.Function_User})
        Me.gridView.GridControl = Me.gridControl
        Me.gridView.Name = "gridView"
        Me.gridView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.[True]
        Me.gridView.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.[True]
        Me.gridView.OptionsClipboard.AllowCopy = DevExpress.Utils.DefaultBoolean.[False]
        Me.gridView.OptionsView.ShowAutoFilterRow = True
        '
        'col_Add
        '
        Me.col_Add.ColumnEdit = Me.bt_Add
        Me.col_Add.Name = "col_Add"
        Me.col_Add.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways
        Me.col_Add.Visible = True
        Me.col_Add.VisibleIndex = 0
        Me.col_Add.Width = 50
        '
        'bt_Add
        '
        EditorButtonImageOptions1.Image = CType(resources.GetObject("EditorButtonImageOptions1.Image"), System.Drawing.Image)
        Me.bt_Add.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(EditorButtonImageOptions1, DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, Nothing)})
        Me.bt_Add.Name = "bt_Add"
        Me.bt_Add.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor
        '
        'col_Delete
        '
        Me.col_Delete.ColumnEdit = Me.btDelete
        Me.col_Delete.Name = "col_Delete"
        Me.col_Delete.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways
        Me.col_Delete.Visible = True
        Me.col_Delete.VisibleIndex = 1
        Me.col_Delete.Width = 50
        '
        'btDelete
        '
        Me.btDelete.AutoHeight = False
        EditorButtonImageOptions2.Image = CType(resources.GetObject("EditorButtonImageOptions2.Image"), System.Drawing.Image)
        Me.btDelete.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(EditorButtonImageOptions2, DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, Nothing)})
        Me.btDelete.Name = "btDelete"
        Me.btDelete.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor
        '
        'BANK_ID
        '
        Me.BANK_ID.Caption = "BANK_ID"
        Me.BANK_ID.FieldName = "BANK_ID"
        Me.BANK_ID.Name = "BANK_ID"
        Me.BANK_ID.Visible = True
        Me.BANK_ID.VisibleIndex = 2
        Me.BANK_ID.Width = 109
        '
        'USER_MAKER
        '
        Me.USER_MAKER.Caption = "USER_MAKER"
        Me.USER_MAKER.FieldName = "USER_MAKER"
        Me.USER_MAKER.Name = "USER_MAKER"
        Me.USER_MAKER.Visible = True
        Me.USER_MAKER.VisibleIndex = 3
        Me.USER_MAKER.Width = 109
        '
        'USER_CHECKER
        '
        Me.USER_CHECKER.Caption = "USER_CHECKER"
        Me.USER_CHECKER.FieldName = "USER_CHECKER"
        Me.USER_CHECKER.Name = "USER_CHECKER"
        Me.USER_CHECKER.Visible = True
        Me.USER_CHECKER.VisibleIndex = 4
        Me.USER_CHECKER.Width = 109
        '
        'USER_ADMIN
        '
        Me.USER_ADMIN.Caption = "USER_ADMIN"
        Me.USER_ADMIN.FieldName = "USER_ADMIN"
        Me.USER_ADMIN.Name = "USER_ADMIN"
        Me.USER_ADMIN.Visible = True
        Me.USER_ADMIN.VisibleIndex = 5
        Me.USER_ADMIN.Width = 109
        '
        'Function_User
        '
        Me.Function_User.Caption = "Function User"
        Me.Function_User.FieldName = "Function_User"
        Me.Function_User.Name = "Function_User"
        Me.Function_User.Width = 109
        '
        'STATUS
        '
        Me.STATUS.Caption = "STATUS"
        Me.STATUS.FieldName = "STATUS"
        Me.STATUS.Name = "STATUS"
        Me.STATUS.Visible = True
        Me.STATUS.VisibleIndex = 6
        Me.STATUS.Width = 123
        '
        'ribbonControl
        '
        Me.ribbonControl.ExpandCollapseItem.Id = 0
        Me.ribbonControl.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.ribbonControl.ExpandCollapseItem, Me.bbiPrintPreview, Me.btAdd, Me.btResetPassword})
        Me.ribbonControl.Location = New System.Drawing.Point(0, 0)
        Me.ribbonControl.MaxItemId = 22
        Me.ribbonControl.Name = "ribbonControl"
        Me.ribbonControl.Pages.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPage() {Me.ribbonPage1})
        Me.ribbonControl.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2013
        Me.ribbonControl.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.[False]
        Me.ribbonControl.Size = New System.Drawing.Size(784, 143)
        Me.ribbonControl.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden
        '
        'bbiPrintPreview
        '
        Me.bbiPrintPreview.Caption = "Print Preview"
        Me.bbiPrintPreview.Id = 14
        Me.bbiPrintPreview.ImageOptions.ImageUri.Uri = "Preview"
        Me.bbiPrintPreview.Name = "bbiPrintPreview"
        '
        'btAdd
        '
        Me.btAdd.Caption = "Add"
        Me.btAdd.Id = 20
        Me.btAdd.ImageOptions.Image = CType(resources.GetObject("btAdd.ImageOptions.Image"), System.Drawing.Image)
        Me.btAdd.ImageOptions.LargeImage = CType(resources.GetObject("btAdd.ImageOptions.LargeImage"), System.Drawing.Image)
        Me.btAdd.Name = "btAdd"
        '
        'btResetPassword
        '
        Me.btResetPassword.Caption = "Reset Password"
        Me.btResetPassword.Id = 21
        Me.btResetPassword.ImageOptions.Image = CType(resources.GetObject("btResetPassword.ImageOptions.Image"), System.Drawing.Image)
        Me.btResetPassword.ImageOptions.LargeImage = CType(resources.GetObject("btResetPassword.ImageOptions.LargeImage"), System.Drawing.Image)
        Me.btResetPassword.Name = "btResetPassword"
        '
        'ribbonPage1
        '
        Me.ribbonPage1.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.ribbonPageGroup1, Me.ribbonPageGroup2})
        Me.ribbonPage1.MergeOrder = 0
        Me.ribbonPage1.Name = "ribbonPage1"
        Me.ribbonPage1.Text = "Home"
        '
        'ribbonPageGroup1
        '
        Me.ribbonPageGroup1.AllowTextClipping = False
        Me.ribbonPageGroup1.ItemLinks.Add(Me.btAdd, True)
        Me.ribbonPageGroup1.ItemLinks.Add(Me.btResetPassword, True)
        Me.ribbonPageGroup1.Name = "ribbonPageGroup1"
        Me.ribbonPageGroup1.ShowCaptionButton = False
        Me.ribbonPageGroup1.Text = "Tasks"
        '
        'ribbonPageGroup2
        '
        Me.ribbonPageGroup2.AllowTextClipping = False
        Me.ribbonPageGroup2.ItemLinks.Add(Me.bbiPrintPreview)
        Me.ribbonPageGroup2.Name = "ribbonPageGroup2"
        Me.ribbonPageGroup2.ShowCaptionButton = False
        Me.ribbonPageGroup2.Text = "Print and Export"
        '
        'FormUser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 592)
        Me.Controls.Add(Me.gridControl)
        Me.Controls.Add(Me.ribbonControl)
        Me.Name = "FormUser"
        Me.Ribbon = Me.ribbonControl
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "User Management"
        CType(Me.gridControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bt_Add, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.btDelete, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ribbonControl, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private WithEvents gridControl As DevExpress.XtraGrid.GridControl
    Private WithEvents gridView As DevExpress.XtraGrid.Views.Grid.GridView
    Private WithEvents ribbonControl As DevExpress.XtraBars.Ribbon.RibbonControl
    Private WithEvents ribbonPage1 As DevExpress.XtraBars.Ribbon.RibbonPage
    Private WithEvents ribbonPageGroup1 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Private WithEvents bbiPrintPreview As DevExpress.XtraBars.BarButtonItem
    Private WithEvents ribbonPageGroup2 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents col_Add As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents col_Delete As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents btDelete As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Friend WithEvents BANK_ID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents USER_MAKER As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents USER_CHECKER As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents USER_ADMIN As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents STATUS As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents btAdd As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents Function_User As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents btResetPassword As DevExpress.XtraBars.BarButtonItem
    Public WithEvents bt_Add As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
End Class
