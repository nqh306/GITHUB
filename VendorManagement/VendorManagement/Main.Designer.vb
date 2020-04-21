<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main
    Inherits DevExpress.XtraBars.Ribbon.RibbonForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main))
        Me.RibbonControl = New DevExpress.XtraBars.Ribbon.RibbonControl()
        Me.BarButton_Tasetco = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButton_Viettel = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButton_SpecialList = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButton_CROWN = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButton_Config = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButton_SQLQuery = New DevExpress.XtraBars.BarButtonItem()
        Me.BarHeaderItem1 = New DevExpress.XtraBars.BarHeaderItem()
        Me.Statusbar_item1 = New DevExpress.XtraBars.BarStaticItem()
        Me.Statusbar_item2 = New DevExpress.XtraBars.BarStaticItem()
        Me.BarButton_UpdateDatabase = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem_Report = New DevExpress.XtraBars.BarButtonItem()
        Me.ImageCollection1 = New DevExpress.Utils.ImageCollection(Me.components)
        Me.RibbonPage1 = New DevExpress.XtraBars.Ribbon.RibbonPage()
        Me.RibbonPageGroup1 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.RibbonPageGroup2 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.RibbonPageGroup4 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.RibbonPage2 = New DevExpress.XtraBars.Ribbon.RibbonPage()
        Me.RibbonPageGroup3 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.RibbonStatusBar = New DevExpress.XtraBars.Ribbon.RibbonStatusBar()
        Me.DocumentManager1 = New DevExpress.XtraBars.Docking2010.DocumentManager(Me.components)
        Me.TabbedView1 = New DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(Me.components)
        Me.DefaultLookAndFeel1 = New DevExpress.LookAndFeel.DefaultLookAndFeel(Me.components)
        CType(Me.RibbonControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ImageCollection1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DocumentManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TabbedView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RibbonControl
        '
        Me.RibbonControl.ExpandCollapseItem.Id = 0
        Me.RibbonControl.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.RibbonControl.ExpandCollapseItem, Me.BarButton_Tasetco, Me.BarButton_Viettel, Me.BarButton_SpecialList, Me.BarButton_CROWN, Me.BarButton_Config, Me.BarButton_SQLQuery, Me.BarHeaderItem1, Me.Statusbar_item1, Me.Statusbar_item2, Me.BarButton_UpdateDatabase, Me.BarButtonItem_Report})
        Me.RibbonControl.LargeImages = Me.ImageCollection1
        Me.RibbonControl.Location = New System.Drawing.Point(0, 0)
        Me.RibbonControl.MaxItemId = 13
        Me.RibbonControl.Name = "RibbonControl"
        Me.RibbonControl.Pages.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPage() {Me.RibbonPage1, Me.RibbonPage2})
        Me.RibbonControl.Size = New System.Drawing.Size(940, 144)
        Me.RibbonControl.StatusBar = Me.RibbonStatusBar
        '
        'BarButton_Tasetco
        '
        Me.BarButton_Tasetco.Caption = "Tasetco Express"
        Me.BarButton_Tasetco.Id = 1
        Me.BarButton_Tasetco.ImageOptions.LargeImageIndex = 4
        Me.BarButton_Tasetco.Name = "BarButton_Tasetco"
        '
        'BarButton_Viettel
        '
        Me.BarButton_Viettel.Caption = "Viettel Post"
        Me.BarButton_Viettel.Id = 2
        Me.BarButton_Viettel.ImageOptions.LargeImageIndex = 4
        Me.BarButton_Viettel.Name = "BarButton_Viettel"
        '
        'BarButton_SpecialList
        '
        Me.BarButton_SpecialList.Caption = "Special List"
        Me.BarButton_SpecialList.Id = 3
        Me.BarButton_SpecialList.ImageOptions.LargeImageIndex = 0
        Me.BarButton_SpecialList.Name = "BarButton_SpecialList"
        '
        'BarButton_CROWN
        '
        Me.BarButton_CROWN.Caption = "CROWN - Document Management"
        Me.BarButton_CROWN.Id = 4
        Me.BarButton_CROWN.ImageOptions.LargeImageIndex = 1
        Me.BarButton_CROWN.Name = "BarButton_CROWN"
        '
        'BarButton_Config
        '
        Me.BarButton_Config.Caption = "Config"
        Me.BarButton_Config.Id = 5
        Me.BarButton_Config.ImageOptions.LargeImageIndex = 6
        Me.BarButton_Config.Name = "BarButton_Config"
        '
        'BarButton_SQLQuery
        '
        Me.BarButton_SQLQuery.Caption = "SQL Query"
        Me.BarButton_SQLQuery.Id = 6
        Me.BarButton_SQLQuery.ImageOptions.LargeImageIndex = 3
        Me.BarButton_SQLQuery.Name = "BarButton_SQLQuery"
        '
        'BarHeaderItem1
        '
        Me.BarHeaderItem1.Caption = "[Vendor Management v1.0]"
        Me.BarHeaderItem1.Id = 7
        Me.BarHeaderItem1.Name = "BarHeaderItem1"
        '
        'Statusbar_item1
        '
        Me.Statusbar_item1.Caption = "BarStaticItem1"
        Me.Statusbar_item1.Id = 8
        Me.Statusbar_item1.Name = "Statusbar_item1"
        '
        'Statusbar_item2
        '
        Me.Statusbar_item2.Caption = "BarStaticItem2"
        Me.Statusbar_item2.Id = 9
        Me.Statusbar_item2.Name = "Statusbar_item2"
        '
        'BarButton_UpdateDatabase
        '
        Me.BarButton_UpdateDatabase.Caption = "Update Database"
        Me.BarButton_UpdateDatabase.Id = 10
        Me.BarButton_UpdateDatabase.ImageOptions.LargeImageIndex = 2
        Me.BarButton_UpdateDatabase.Name = "BarButton_UpdateDatabase"
        '
        'BarButtonItem_Report
        '
        Me.BarButtonItem_Report.Caption = "Report"
        Me.BarButtonItem_Report.Id = 12
        Me.BarButtonItem_Report.ImageOptions.Image = CType(resources.GetObject("BarButtonItem_Report.ImageOptions.Image"), System.Drawing.Image)
        Me.BarButtonItem_Report.ImageOptions.LargeImage = CType(resources.GetObject("BarButtonItem_Report.ImageOptions.LargeImage"), System.Drawing.Image)
        Me.BarButtonItem_Report.Name = "BarButtonItem_Report"
        '
        'ImageCollection1
        '
        Me.ImageCollection1.ImageSize = New System.Drawing.Size(32, 32)
        Me.ImageCollection1.ImageStream = CType(resources.GetObject("ImageCollection1.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        Me.ImageCollection1.Images.SetKeyName(0, "_Data_Management-512.png")
        Me.ImageCollection1.Images.SetKeyName(1, "_Safe-512.png")
        Me.ImageCollection1.Images.SetKeyName(2, "Data_management-512.png")
        Me.ImageCollection1.Images.SetKeyName(3, "document-sql-512.png")
        Me.ImageCollection1.Images.SetKeyName(4, "mail-512.png")
        Me.ImageCollection1.Images.SetKeyName(5, "personal.png")
        Me.ImageCollection1.Images.SetKeyName(6, "seo_web_2-09-512.png")
        '
        'RibbonPage1
        '
        Me.RibbonPage1.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.RibbonPageGroup1, Me.RibbonPageGroup2, Me.RibbonPageGroup4})
        Me.RibbonPage1.Name = "RibbonPage1"
        Me.RibbonPage1.Text = "Home"
        '
        'RibbonPageGroup1
        '
        Me.RibbonPageGroup1.ItemLinks.Add(Me.BarButton_Tasetco, True)
        Me.RibbonPageGroup1.ItemLinks.Add(Me.BarButton_Viettel, True)
        Me.RibbonPageGroup1.ItemLinks.Add(Me.BarButton_SpecialList, True)
        Me.RibbonPageGroup1.ItemLinks.Add(Me.BarButton_UpdateDatabase, True)
        Me.RibbonPageGroup1.Name = "RibbonPageGroup1"
        Me.RibbonPageGroup1.Text = "Letter Management"
        '
        'RibbonPageGroup2
        '
        Me.RibbonPageGroup2.ItemLinks.Add(Me.BarButton_CROWN)
        Me.RibbonPageGroup2.Name = "RibbonPageGroup2"
        Me.RibbonPageGroup2.Text = "Document Management"
        '
        'RibbonPageGroup4
        '
        Me.RibbonPageGroup4.ItemLinks.Add(Me.BarButtonItem_Report, True)
        Me.RibbonPageGroup4.Name = "RibbonPageGroup4"
        Me.RibbonPageGroup4.Text = "Report"
        '
        'RibbonPage2
        '
        Me.RibbonPage2.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.RibbonPageGroup3})
        Me.RibbonPage2.Name = "RibbonPage2"
        Me.RibbonPage2.Text = "Configuration"
        '
        'RibbonPageGroup3
        '
        Me.RibbonPageGroup3.ItemLinks.Add(Me.BarButton_Config, True)
        Me.RibbonPageGroup3.ItemLinks.Add(Me.BarButton_SQLQuery, True)
        Me.RibbonPageGroup3.Name = "RibbonPageGroup3"
        Me.RibbonPageGroup3.Text = "Configuration"
        '
        'RibbonStatusBar
        '
        Me.RibbonStatusBar.ItemLinks.Add(Me.BarHeaderItem1)
        Me.RibbonStatusBar.ItemLinks.Add(Me.Statusbar_item1)
        Me.RibbonStatusBar.ItemLinks.Add(Me.Statusbar_item2)
        Me.RibbonStatusBar.Location = New System.Drawing.Point(0, 481)
        Me.RibbonStatusBar.Name = "RibbonStatusBar"
        Me.RibbonStatusBar.Ribbon = Me.RibbonControl
        Me.RibbonStatusBar.Size = New System.Drawing.Size(940, 32)
        '
        'DocumentManager1
        '
        Me.DocumentManager1.MdiParent = Me
        Me.DocumentManager1.MenuManager = Me.RibbonControl
        Me.DocumentManager1.View = Me.TabbedView1
        Me.DocumentManager1.ViewCollection.AddRange(New DevExpress.XtraBars.Docking2010.Views.BaseView() {Me.TabbedView1})
        '
        'TabbedView1
        '
        Me.TabbedView1.RootContainer.Element = Nothing
        '
        'DefaultLookAndFeel1
        '
        Me.DefaultLookAndFeel1.LookAndFeel.SkinName = "Office 2010 Blue"
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(940, 513)
        Me.Controls.Add(Me.RibbonStatusBar)
        Me.Controls.Add(Me.RibbonControl)
        Me.IsMdiContainer = True
        Me.Name = "Main"
        Me.Ribbon = Me.RibbonControl
        Me.StatusBar = Me.RibbonStatusBar
        Me.Text = "Vendor Management - Provided by Account Services Operation [Standard Chartered Ba" &
    "nk]"
        CType(Me.RibbonControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ImageCollection1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DocumentManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TabbedView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents RibbonControl As DevExpress.XtraBars.Ribbon.RibbonControl
    Friend WithEvents RibbonPage1 As DevExpress.XtraBars.Ribbon.RibbonPage
    Friend WithEvents RibbonPageGroup1 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents RibbonStatusBar As DevExpress.XtraBars.Ribbon.RibbonStatusBar
    Friend WithEvents ImageCollection1 As DevExpress.Utils.ImageCollection
    Friend WithEvents DocumentManager1 As DevExpress.XtraBars.Docking2010.DocumentManager
    Friend WithEvents TabbedView1 As DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView
    Friend WithEvents DefaultLookAndFeel1 As DevExpress.LookAndFeel.DefaultLookAndFeel
    Friend WithEvents BarButton_Tasetco As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButton_Viettel As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButton_SpecialList As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButton_CROWN As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButton_Config As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButton_SQLQuery As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents RibbonPageGroup2 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents RibbonPage2 As DevExpress.XtraBars.Ribbon.RibbonPage
    Friend WithEvents RibbonPageGroup3 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents BarHeaderItem1 As DevExpress.XtraBars.BarHeaderItem
    Friend WithEvents Statusbar_item1 As DevExpress.XtraBars.BarStaticItem
    Friend WithEvents Statusbar_item2 As DevExpress.XtraBars.BarStaticItem
    Friend WithEvents BarButton_UpdateDatabase As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem_Report As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents RibbonPageGroup4 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
End Class
