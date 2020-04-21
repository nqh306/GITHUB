<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Me.RibbonControl = New DevExpress.XtraBars.Ribbon.RibbonControl()
        Me.ApplicationMenu1 = New DevExpress.XtraBars.Ribbon.ApplicationMenu(Me.components)
        Me.BarButtonItem_ChangePW = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem_ChangeUserAccess = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem_Quit = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem_PaymentInstruction = New DevExpress.XtraBars.BarButtonItem()
        Me.StatusBar_Version = New DevExpress.XtraBars.BarHeaderItem()
        Me.BarButtonItem_Config = New DevExpress.XtraBars.BarButtonItem()
        Me.BarEditItem_User = New DevExpress.XtraBars.BarEditItem()
        Me.RepositoryItemButtonEdit_ChangeUser = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.BarButtonItem_UserFunction = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem_SQL_QUERY = New DevExpress.XtraBars.BarButtonItem()
        Me.ImageCollection1 = New DevExpress.Utils.ImageCollection(Me.components)
        Me.RibbonPage_Home = New DevExpress.XtraBars.Ribbon.RibbonPage()
        Me.RibbonPageGroup1 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.RibbonPage_Config = New DevExpress.XtraBars.Ribbon.RibbonPage()
        Me.RibbonPageGroup2 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.RibbonPage_Administrator = New DevExpress.XtraBars.Ribbon.RibbonPage()
        Me.RibbonPageGroup_UserAccess = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.RibbonStatusBar = New DevExpress.XtraBars.Ribbon.RibbonStatusBar()
        Me.DefaultLookAndFeel1 = New DevExpress.LookAndFeel.DefaultLookAndFeel(Me.components)
        Me.DocumentManager1 = New DevExpress.XtraBars.Docking2010.DocumentManager(Me.components)
        Me.TabbedView1 = New DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(Me.components)
        CType(Me.RibbonControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ApplicationMenu1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemButtonEdit_ChangeUser, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ImageCollection1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DocumentManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TabbedView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RibbonControl
        '
        Me.RibbonControl.ApplicationButtonDropDownControl = Me.ApplicationMenu1
        Me.RibbonControl.ExpandCollapseItem.Id = 0
        Me.RibbonControl.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.RibbonControl.ExpandCollapseItem, Me.BarButtonItem_PaymentInstruction, Me.StatusBar_Version, Me.BarButtonItem_Config, Me.BarEditItem_User, Me.BarButtonItem_ChangePW, Me.BarButtonItem_ChangeUserAccess, Me.BarButtonItem_Quit, Me.BarButtonItem_UserFunction, Me.BarButtonItem_SQL_QUERY})
        Me.RibbonControl.LargeImages = Me.ImageCollection1
        Me.RibbonControl.Location = New System.Drawing.Point(0, 0)
        Me.RibbonControl.MaxItemId = 19
        Me.RibbonControl.Name = "RibbonControl"
        Me.RibbonControl.PageHeaderItemLinks.Add(Me.BarEditItem_User)
        Me.RibbonControl.Pages.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPage() {Me.RibbonPage_Home, Me.RibbonPage_Config, Me.RibbonPage_Administrator})
        Me.RibbonControl.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemButtonEdit_ChangeUser})
        Me.RibbonControl.Size = New System.Drawing.Size(874, 144)
        Me.RibbonControl.StatusBar = Me.RibbonStatusBar
        '
        'ApplicationMenu1
        '
        Me.ApplicationMenu1.ItemLinks.Add(Me.BarButtonItem_ChangePW)
        Me.ApplicationMenu1.ItemLinks.Add(Me.BarButtonItem_ChangeUserAccess)
        Me.ApplicationMenu1.ItemLinks.Add(Me.BarButtonItem_Quit)
        Me.ApplicationMenu1.Name = "ApplicationMenu1"
        Me.ApplicationMenu1.Ribbon = Me.RibbonControl
        '
        'BarButtonItem_ChangePW
        '
        Me.BarButtonItem_ChangePW.Caption = "Change Password"
        Me.BarButtonItem_ChangePW.Id = 12
        Me.BarButtonItem_ChangePW.ImageOptions.Image = CType(resources.GetObject("BarButtonItem_ChangePW.ImageOptions.Image"), System.Drawing.Image)
        Me.BarButtonItem_ChangePW.ImageOptions.LargeImage = CType(resources.GetObject("BarButtonItem_ChangePW.ImageOptions.LargeImage"), System.Drawing.Image)
        Me.BarButtonItem_ChangePW.Name = "BarButtonItem_ChangePW"
        '
        'BarButtonItem_ChangeUserAccess
        '
        Me.BarButtonItem_ChangeUserAccess.Caption = "Run As Another User"
        Me.BarButtonItem_ChangeUserAccess.Id = 13
        Me.BarButtonItem_ChangeUserAccess.ImageOptions.Image = CType(resources.GetObject("BarButtonItem_ChangeUserAccess.ImageOptions.Image"), System.Drawing.Image)
        Me.BarButtonItem_ChangeUserAccess.Name = "BarButtonItem_ChangeUserAccess"
        '
        'BarButtonItem_Quit
        '
        Me.BarButtonItem_Quit.Caption = "Quit"
        Me.BarButtonItem_Quit.Id = 14
        Me.BarButtonItem_Quit.ImageOptions.Image = CType(resources.GetObject("BarButtonItem_Quit.ImageOptions.Image"), System.Drawing.Image)
        Me.BarButtonItem_Quit.Name = "BarButtonItem_Quit"
        '
        'BarButtonItem_PaymentInstruction
        '
        Me.BarButtonItem_PaymentInstruction.Caption = "Payment Instruction"
        Me.BarButtonItem_PaymentInstruction.Id = 1
        Me.BarButtonItem_PaymentInstruction.ImageOptions.Image = CType(resources.GetObject("BarButtonItem_PaymentInstruction.ImageOptions.Image"), System.Drawing.Image)
        Me.BarButtonItem_PaymentInstruction.ImageOptions.LargeImage = CType(resources.GetObject("BarButtonItem_PaymentInstruction.ImageOptions.LargeImage"), System.Drawing.Image)
        Me.BarButtonItem_PaymentInstruction.Name = "BarButtonItem_PaymentInstruction"
        '
        'StatusBar_Version
        '
        Me.StatusBar_Version.Caption = "StatusBar_Version"
        Me.StatusBar_Version.Id = 3
        Me.StatusBar_Version.Name = "StatusBar_Version"
        '
        'BarButtonItem_Config
        '
        Me.BarButtonItem_Config.Caption = "Config"
        Me.BarButtonItem_Config.Id = 4
        Me.BarButtonItem_Config.ImageOptions.LargeImageIndex = 2
        Me.BarButtonItem_Config.Name = "BarButtonItem_Config"
        '
        'BarEditItem_User
        '
        Me.BarEditItem_User.Edit = Me.RepositoryItemButtonEdit_ChangeUser
        Me.BarEditItem_User.Id = 10
        Me.BarEditItem_User.Name = "BarEditItem_User"
        '
        'RepositoryItemButtonEdit_ChangeUser
        '
        Me.RepositoryItemButtonEdit_ChangeUser.AutoHeight = False
        EditorButtonImageOptions1.Image = CType(resources.GetObject("EditorButtonImageOptions1.Image"), System.Drawing.Image)
        Me.RepositoryItemButtonEdit_ChangeUser.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(EditorButtonImageOptions1, DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, Nothing)})
        Me.RepositoryItemButtonEdit_ChangeUser.Name = "RepositoryItemButtonEdit_ChangeUser"
        Me.RepositoryItemButtonEdit_ChangeUser.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        '
        'BarButtonItem_UserFunction
        '
        Me.BarButtonItem_UserFunction.Caption = "User Management"
        Me.BarButtonItem_UserFunction.Id = 15
        Me.BarButtonItem_UserFunction.ImageOptions.LargeImageIndex = 3
        Me.BarButtonItem_UserFunction.Name = "BarButtonItem_UserFunction"
        '
        'BarButtonItem_SQL_QUERY
        '
        Me.BarButtonItem_SQL_QUERY.Caption = "SQL QUERY"
        Me.BarButtonItem_SQL_QUERY.Id = 18
        Me.BarButtonItem_SQL_QUERY.ImageOptions.LargeImageIndex = 5
        Me.BarButtonItem_SQL_QUERY.Name = "BarButtonItem_SQL_QUERY"
        '
        'ImageCollection1
        '
        Me.ImageCollection1.ImageSize = New System.Drawing.Size(64, 64)
        Me.ImageCollection1.ImageStream = CType(resources.GetObject("ImageCollection1.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        Me.ImageCollection1.Images.SetKeyName(0, "107-256.png")
        Me.ImageCollection1.Images.SetKeyName(1, "Bank_statement_banking_finance_invoice_mail_mail_box-256.png")
        Me.ImageCollection1.Images.SetKeyName(2, "seo_web_2-09-512.png")
        Me.ImageCollection1.Images.SetKeyName(3, "personal.png")
        Me.ImageCollection1.Images.SetKeyName(4, "Backer_Report-256.png")
        Me.ImageCollection1.Images.SetKeyName(5, "document-sql-512.png")
        '
        'RibbonPage_Home
        '
        Me.RibbonPage_Home.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.RibbonPageGroup1})
        Me.RibbonPage_Home.Name = "RibbonPage_Home"
        Me.RibbonPage_Home.Text = "Home"
        '
        'RibbonPageGroup1
        '
        Me.RibbonPageGroup1.ItemLinks.Add(Me.BarButtonItem_PaymentInstruction, True)
        Me.RibbonPageGroup1.Name = "RibbonPageGroup1"
        Me.RibbonPageGroup1.Text = "Home"
        '
        'RibbonPage_Config
        '
        Me.RibbonPage_Config.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.RibbonPageGroup2})
        Me.RibbonPage_Config.Name = "RibbonPage_Config"
        Me.RibbonPage_Config.Text = "Config"
        '
        'RibbonPageGroup2
        '
        Me.RibbonPageGroup2.ItemLinks.Add(Me.BarButtonItem_Config, True)
        Me.RibbonPageGroup2.Name = "RibbonPageGroup2"
        Me.RibbonPageGroup2.Text = "Config"
        '
        'RibbonPage_Administrator
        '
        Me.RibbonPage_Administrator.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.RibbonPageGroup_UserAccess})
        Me.RibbonPage_Administrator.Name = "RibbonPage_Administrator"
        Me.RibbonPage_Administrator.Text = "Administrator"
        '
        'RibbonPageGroup_UserAccess
        '
        Me.RibbonPageGroup_UserAccess.ItemLinks.Add(Me.BarButtonItem_UserFunction, True)
        Me.RibbonPageGroup_UserAccess.ItemLinks.Add(Me.BarButtonItem_SQL_QUERY, True)
        Me.RibbonPageGroup_UserAccess.Name = "RibbonPageGroup_UserAccess"
        Me.RibbonPageGroup_UserAccess.Text = "User Access"
        '
        'RibbonStatusBar
        '
        Me.RibbonStatusBar.ItemLinks.Add(Me.StatusBar_Version)
        Me.RibbonStatusBar.Location = New System.Drawing.Point(0, 488)
        Me.RibbonStatusBar.Name = "RibbonStatusBar"
        Me.RibbonStatusBar.Ribbon = Me.RibbonControl
        Me.RibbonStatusBar.Size = New System.Drawing.Size(874, 32)
        '
        'DefaultLookAndFeel1
        '
        Me.DefaultLookAndFeel1.EnableBonusSkins = True
        Me.DefaultLookAndFeel1.LookAndFeel.SkinName = "Office 2010 Blue"
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
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(874, 520)
        Me.Controls.Add(Me.RibbonStatusBar)
        Me.Controls.Add(Me.RibbonControl)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.Name = "MainForm"
        Me.Ribbon = Me.RibbonControl
        Me.StatusBar = Me.RibbonStatusBar
        Me.Text = "SSO Tools"
        CType(Me.RibbonControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ApplicationMenu1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemButtonEdit_ChangeUser, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ImageCollection1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DocumentManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TabbedView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents RibbonControl As DevExpress.XtraBars.Ribbon.RibbonControl
    Friend WithEvents RibbonPage_Home As DevExpress.XtraBars.Ribbon.RibbonPage
    Friend WithEvents RibbonPageGroup1 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents RibbonStatusBar As DevExpress.XtraBars.Ribbon.RibbonStatusBar
    Friend WithEvents ImageCollection1 As DevExpress.Utils.ImageCollection
    Friend WithEvents BarButtonItem_PaymentInstruction As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents DefaultLookAndFeel1 As DevExpress.LookAndFeel.DefaultLookAndFeel
    Friend WithEvents DocumentManager1 As DevExpress.XtraBars.Docking2010.DocumentManager
    Friend WithEvents TabbedView1 As DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView
    Friend WithEvents StatusBar_Version As DevExpress.XtraBars.BarHeaderItem
    Friend WithEvents BarButtonItem_Config As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents RibbonPage_Config As DevExpress.XtraBars.Ribbon.RibbonPage
    Friend WithEvents RibbonPageGroup2 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents BarEditItem_User As DevExpress.XtraBars.BarEditItem
    Friend WithEvents RepositoryItemButtonEdit_ChangeUser As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Friend WithEvents ApplicationMenu1 As DevExpress.XtraBars.Ribbon.ApplicationMenu
    Friend WithEvents BarButtonItem_ChangePW As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem_ChangeUserAccess As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem_Quit As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents RibbonPage_Administrator As DevExpress.XtraBars.Ribbon.RibbonPage
    Friend WithEvents RibbonPageGroup_UserAccess As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents BarButtonItem_UserFunction As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem_SQL_QUERY As DevExpress.XtraBars.BarButtonItem
End Class
