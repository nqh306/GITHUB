Partial Public Class Home
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

#Region "Windows Form Designer generated code"

    ''' <summary>
    ''' Required method for Designer support - do not modify
    ''' the contents of this method with the code editor.
    ''' </summary>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Home))
        Dim EditorButtonImageOptions2 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim SerializableAppearanceObject5 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject6 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject7 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject8 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Me.ribbonControl1 = New DevExpress.XtraBars.Ribbon.RibbonControl()
        Me.bt_XuLyToTrinh = New DevExpress.XtraBars.BarButtonItem()
        Me.bt_User = New DevExpress.XtraBars.BarButtonItem()
        Me.BarEditItem_Username = New DevExpress.XtraBars.BarEditItem()
        Me.RepositoryItemButtonEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.btBaoCaoThamTra = New DevExpress.XtraBars.BarButtonItem()
        Me.ribbonPage1 = New DevExpress.XtraBars.Ribbon.RibbonPage()
        Me.ribbonPageGroup1 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.RibbonPage2 = New DevExpress.XtraBars.Ribbon.RibbonPage()
        Me.RibbonPageGroup2 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.DocumentManager1 = New DevExpress.XtraBars.Docking2010.DocumentManager(Me.components)
        Me.TabbedView1 = New DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(Me.components)
        Me.BarButtonItem_Report = New DevExpress.XtraBars.BarButtonItem()
        CType(Me.ribbonControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemButtonEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DocumentManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TabbedView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ribbonControl1
        '
        Me.ribbonControl1.ExpandCollapseItem.Id = 0
        Me.ribbonControl1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.ribbonControl1.ExpandCollapseItem, Me.ribbonControl1.SearchEditItem, Me.bt_XuLyToTrinh, Me.bt_User, Me.BarEditItem_Username, Me.btBaoCaoThamTra, Me.BarButtonItem_Report})
        Me.ribbonControl1.Location = New System.Drawing.Point(0, 0)
        Me.ribbonControl1.MaxItemId = 6
        Me.ribbonControl1.Name = "ribbonControl1"
        Me.ribbonControl1.PageHeaderItemLinks.Add(Me.BarEditItem_Username)
        Me.ribbonControl1.Pages.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPage() {Me.ribbonPage1, Me.RibbonPage2})
        Me.ribbonControl1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemButtonEdit1})
        Me.ribbonControl1.Size = New System.Drawing.Size(879, 146)
        '
        'bt_XuLyToTrinh
        '
        Me.bt_XuLyToTrinh.Caption = "Xử lý tờ trình"
        Me.bt_XuLyToTrinh.Id = 1
        Me.bt_XuLyToTrinh.ImageOptions.Image = CType(resources.GetObject("bt_XuLyToTrinh.ImageOptions.Image"), System.Drawing.Image)
        Me.bt_XuLyToTrinh.ImageOptions.LargeImage = CType(resources.GetObject("bt_XuLyToTrinh.ImageOptions.LargeImage"), System.Drawing.Image)
        Me.bt_XuLyToTrinh.Name = "bt_XuLyToTrinh"
        '
        'bt_User
        '
        Me.bt_User.Caption = "Tài khoản"
        Me.bt_User.Id = 2
        Me.bt_User.ImageOptions.Image = CType(resources.GetObject("bt_User.ImageOptions.Image"), System.Drawing.Image)
        Me.bt_User.ImageOptions.LargeImage = CType(resources.GetObject("bt_User.ImageOptions.LargeImage"), System.Drawing.Image)
        Me.bt_User.Name = "bt_User"
        '
        'BarEditItem_Username
        '
        Me.BarEditItem_Username.Edit = Me.RepositoryItemButtonEdit1
        Me.BarEditItem_Username.Id = 3
        Me.BarEditItem_Username.Name = "BarEditItem_Username"
        '
        'RepositoryItemButtonEdit1
        '
        Me.RepositoryItemButtonEdit1.AutoHeight = False
        EditorButtonImageOptions2.Image = CType(resources.GetObject("EditorButtonImageOptions2.Image"), System.Drawing.Image)
        Me.RepositoryItemButtonEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, EditorButtonImageOptions2, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject5, SerializableAppearanceObject6, SerializableAppearanceObject7, SerializableAppearanceObject8, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.RepositoryItemButtonEdit1.Name = "RepositoryItemButtonEdit1"
        '
        'btBaoCaoThamTra
        '
        Me.btBaoCaoThamTra.Caption = "Báo cáo thẩm tra"
        Me.btBaoCaoThamTra.Id = 4
        Me.btBaoCaoThamTra.ImageOptions.Image = CType(resources.GetObject("btBaoCaoThamTra.ImageOptions.Image"), System.Drawing.Image)
        Me.btBaoCaoThamTra.ImageOptions.LargeImage = CType(resources.GetObject("btBaoCaoThamTra.ImageOptions.LargeImage"), System.Drawing.Image)
        Me.btBaoCaoThamTra.Name = "btBaoCaoThamTra"
        '
        'ribbonPage1
        '
        Me.ribbonPage1.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.ribbonPageGroup1})
        Me.ribbonPage1.Name = "ribbonPage1"
        Me.ribbonPage1.Text = "Home"
        '
        'ribbonPageGroup1
        '
        Me.ribbonPageGroup1.ItemLinks.Add(Me.btBaoCaoThamTra, True)
        Me.ribbonPageGroup1.ItemLinks.Add(Me.bt_XuLyToTrinh, True)
        Me.ribbonPageGroup1.ItemLinks.Add(Me.BarButtonItem_Report, True)
        Me.ribbonPageGroup1.Name = "ribbonPageGroup1"
        Me.ribbonPageGroup1.Text = "Công Việc"
        '
        'RibbonPage2
        '
        Me.RibbonPage2.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.RibbonPageGroup2})
        Me.RibbonPage2.Name = "RibbonPage2"
        Me.RibbonPage2.Text = "Quản trị"
        '
        'RibbonPageGroup2
        '
        Me.RibbonPageGroup2.ItemLinks.Add(Me.bt_User, True)
        Me.RibbonPageGroup2.Name = "RibbonPageGroup2"
        Me.RibbonPageGroup2.Text = "Quản trị"
        '
        'DocumentManager1
        '
        Me.DocumentManager1.MdiParent = Me
        Me.DocumentManager1.MenuManager = Me.ribbonControl1
        Me.DocumentManager1.View = Me.TabbedView1
        Me.DocumentManager1.ViewCollection.AddRange(New DevExpress.XtraBars.Docking2010.Views.BaseView() {Me.TabbedView1})
        '
        'BarButtonItem_Report
        '
        Me.BarButtonItem_Report.Caption = "Báo Cáo"
        Me.BarButtonItem_Report.Id = 5
        Me.BarButtonItem_Report.ImageOptions.Image = CType(resources.GetObject("BarButtonItem_Report.ImageOptions.Image"), System.Drawing.Image)
        Me.BarButtonItem_Report.ImageOptions.LargeImage = CType(resources.GetObject("BarButtonItem_Report.ImageOptions.LargeImage"), System.Drawing.Image)
        Me.BarButtonItem_Report.Name = "BarButtonItem_Report"
        '
        'Home
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(879, 476)
        Me.Controls.Add(Me.ribbonControl1)
        Me.IsMdiContainer = True
        Me.Name = "Home"
        Me.Ribbon = Me.ribbonControl1
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Ban Tổng Hợp - EVNGENCO1"
        CType(Me.ribbonControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemButtonEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DocumentManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TabbedView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private WithEvents ribbonControl1 As DevExpress.XtraBars.Ribbon.RibbonControl
    Private WithEvents ribbonPage1 As DevExpress.XtraBars.Ribbon.RibbonPage
    Private WithEvents ribbonPageGroup1 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents DocumentManager1 As DevExpress.XtraBars.Docking2010.DocumentManager
    Friend WithEvents TabbedView1 As DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView
    Friend WithEvents bt_XuLyToTrinh As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents bt_User As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents RibbonPage2 As DevExpress.XtraBars.Ribbon.RibbonPage
    Friend WithEvents RibbonPageGroup2 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents BarEditItem_Username As DevExpress.XtraBars.BarEditItem
    Friend WithEvents RepositoryItemButtonEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Friend WithEvents btBaoCaoThamTra As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem_Report As DevExpress.XtraBars.BarButtonItem
End Class
