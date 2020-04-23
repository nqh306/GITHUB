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
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim EditorButtonImageOptions2 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim SerializableAppearanceObject5 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject6 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject7 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject8 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Me.gridControl = New DevExpress.XtraGrid.GridControl()
        Me.gridView = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.col_Add = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.bt_Add = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.col_Delete = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.btDelete = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.USERNAME = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.MAKER = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.CHECKER = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ADMIN = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.STATUS = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ribbonControl = New DevExpress.XtraBars.Ribbon.RibbonControl()
        Me.bbiPrintPreview = New DevExpress.XtraBars.BarButtonItem()
        Me.btAdd = New DevExpress.XtraBars.BarButtonItem()
        Me.btResetPassword = New DevExpress.XtraBars.BarButtonItem()
        Me.ribbonPage1 = New DevExpress.XtraBars.Ribbon.RibbonPage()
        Me.ribbonPageGroup1 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.ribbonPageGroup2 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.FULLNAME = New DevExpress.XtraGrid.Columns.GridColumn()
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
        Me.gridControl.Location = New System.Drawing.Point(0, 146)
        Me.gridControl.MainView = Me.gridView
        Me.gridControl.MenuManager = Me.ribbonControl
        Me.gridControl.Name = "gridControl"
        Me.gridControl.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.bt_Add, Me.btDelete})
        Me.gridControl.Size = New System.Drawing.Size(784, 351)
        Me.gridControl.TabIndex = 2
        Me.gridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gridView})
        '
        'gridView
        '
        Me.gridView.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.gridView.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.col_Add, Me.col_Delete, Me.USERNAME, Me.FULLNAME, Me.MAKER, Me.CHECKER, Me.ADMIN, Me.STATUS})
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
        Me.bt_Add.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
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
        Me.btDelete.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, EditorButtonImageOptions2, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject5, SerializableAppearanceObject6, SerializableAppearanceObject7, SerializableAppearanceObject8, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.btDelete.Name = "btDelete"
        Me.btDelete.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor
        '
        'USERNAME
        '
        Me.USERNAME.Caption = "USERNAME"
        Me.USERNAME.FieldName = "USERNAME"
        Me.USERNAME.Name = "USERNAME"
        Me.USERNAME.Visible = True
        Me.USERNAME.VisibleIndex = 2
        Me.USERNAME.Width = 109
        '
        'MAKER
        '
        Me.MAKER.Caption = "MAKER"
        Me.MAKER.FieldName = "MAKER"
        Me.MAKER.Name = "MAKER"
        Me.MAKER.Visible = True
        Me.MAKER.VisibleIndex = 4
        Me.MAKER.Width = 109
        '
        'CHECKER
        '
        Me.CHECKER.Caption = "CHECKER"
        Me.CHECKER.FieldName = "CHECKER"
        Me.CHECKER.Name = "CHECKER"
        Me.CHECKER.Visible = True
        Me.CHECKER.VisibleIndex = 5
        Me.CHECKER.Width = 109
        '
        'ADMIN
        '
        Me.ADMIN.Caption = "ADMIN"
        Me.ADMIN.FieldName = "ADMIN"
        Me.ADMIN.Name = "ADMIN"
        Me.ADMIN.Visible = True
        Me.ADMIN.VisibleIndex = 6
        Me.ADMIN.Width = 109
        '
        'STATUS
        '
        Me.STATUS.Caption = "STATUS"
        Me.STATUS.FieldName = "STATUS"
        Me.STATUS.Name = "STATUS"
        Me.STATUS.Visible = True
        Me.STATUS.VisibleIndex = 7
        Me.STATUS.Width = 123
        '
        'ribbonControl
        '
        Me.ribbonControl.ExpandCollapseItem.Id = 0
        Me.ribbonControl.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.ribbonControl.ExpandCollapseItem, Me.ribbonControl.SearchEditItem, Me.bbiPrintPreview, Me.btAdd, Me.btResetPassword})
        Me.ribbonControl.Location = New System.Drawing.Point(0, 0)
        Me.ribbonControl.MaxItemId = 22
        Me.ribbonControl.Name = "ribbonControl"
        Me.ribbonControl.Pages.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPage() {Me.ribbonPage1})
        Me.ribbonControl.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2013
        Me.ribbonControl.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.[False]
        Me.ribbonControl.Size = New System.Drawing.Size(784, 146)
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
        Me.ribbonPageGroup1.CaptionButtonVisible = DevExpress.Utils.DefaultBoolean.[False]
        Me.ribbonPageGroup1.ItemLinks.Add(Me.btAdd, True)
        Me.ribbonPageGroup1.ItemLinks.Add(Me.btResetPassword, True)
        Me.ribbonPageGroup1.Name = "ribbonPageGroup1"
        Me.ribbonPageGroup1.Text = "Tasks"
        '
        'ribbonPageGroup2
        '
        Me.ribbonPageGroup2.AllowTextClipping = False
        Me.ribbonPageGroup2.CaptionButtonVisible = DevExpress.Utils.DefaultBoolean.[False]
        Me.ribbonPageGroup2.ItemLinks.Add(Me.bbiPrintPreview)
        Me.ribbonPageGroup2.Name = "ribbonPageGroup2"
        Me.ribbonPageGroup2.Text = "Print and Export"
        '
        'FULLNAME
        '
        Me.FULLNAME.Caption = "FULLNAME"
        Me.FULLNAME.FieldName = "FULLNAME"
        Me.FULLNAME.Name = "FULLNAME"
        Me.FULLNAME.Visible = True
        Me.FULLNAME.VisibleIndex = 3
        '
        'FormUser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 497)
        Me.Controls.Add(Me.gridControl)
        Me.Controls.Add(Me.ribbonControl)
        Me.Name = "FormUser"
        Me.Ribbon = Me.ribbonControl
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Tài khoản"
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
    Friend WithEvents MAKER As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CHECKER As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ADMIN As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents STATUS As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents btAdd As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents btResetPassword As DevExpress.XtraBars.BarButtonItem
    Private WithEvents bt_Add As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Private WithEvents USERNAME As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents FULLNAME As DevExpress.XtraGrid.Columns.GridColumn
End Class
