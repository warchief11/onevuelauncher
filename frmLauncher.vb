''****************************************************************************************************
'Class:	frmLauncher
'
'	Form for automating the downloading of OneVue test builds from the SVN launcher repository at
'	svn://launcher.fas.au.challenger.net/fas/onevuereleases
'
'	Please refer to the FAS Notebook document for information on configuring workstation .NET security 
'	policies to enable this application to run over the network:
'
'   onenote:///L:\IT\Funds%20&%20Life%20IT\FAS\Documentation\OneNote\OneVue%20Documentation\OneVue\System%20Documentation\Build%20and%20Deploy.one#Launcher%20-%20Installation%20Notes&section-id={5E7F9CB8-2655-4439-A30D-D45E0EE3FB91}&page-id={A50FC9E4-DB94-4D8A-A3A8-F861BF401873}&end
'   
' ---------------------------------------------------------------------------------------------------
'
'    Modification(History)
'
'Date           Developer   Description
'----------------------------------------------------------------------------------------------------
'24/4/2007      HHa         Always copy the svn_testing_template files and added a label to show status
'08-Oct-2008    CMadden     VS2008 upgrade - Added param type to LoadData() and added config file for
'                           storing the location of the Branches and Training XML data files to enable 
'                           running versions of the application from different locations, using common
'                           data files.
'07-Jan-2009    HHa         Added template path to configuration options.
'01-Nov-2011    CMadden     Updated the updatereleases.bat batch file to return the branch revision 
'                           number and use that to display the revision number in the status string.  
'                           If the SVN command fails to retrieve a valid revision number for any reason, 
'                           displays "Update failed." as the status and prompts the user to ask if they 
'                           wish to run the previous version.  Added "Force Update" option to delete a 
'                           branch prior to updating for situations when updates fail.
'01-Dec-2011    CMadden     Added revision validation.  Removed call to runOneVue.bat in favour of 
'                           running executable directly.
'12-Feb-2013    CMadden     Added -WORK_DIR command line arg to override standard working directory 
'                           location (required for VDI use). Rejigged svn_testing directory structure 
'                           and updated batch files to suit.
'16-Jul-2013    CMadden     Added checking for TemplatePath existence in MakeDirStructure()
'26-Jul-2013    CMadden     v3.0 
'                             - Replaced template SVN batch files with libSVN API
'                             - Added frmCheckout progress dialog
'                             - Added RepositoryURI and WorkingDirectoryPath app.Config values to 
'                               enabled remote working directory e.g. U:\OneVueLauncherWorking.
'30-Jul-2013    CMadden     Added optional branch BranchWorkingDirectory property to enable an individual 
'                           branch to use a different working copy path.  This is useful in cases where 
'                           the default WorkingDirectoryPath is on a network drive, but a particular app 
'                           will only execute from a local drive.
'31-Jul-2013    CMadden     v3.1 Added "Date Released" item to branch details and checking of the branch 
'                           working copy to determine if it is at the current revision prior to checkout 
'                           to improve performance.
'22-Jan-2015    CMadden     Parameterised the Production and Training options to permit a separate 
'                           Ambit-specific Launcher instance.
'*******************************************************************************************************/

Imports System.Xml
Imports System.IO
Imports OneVueLauncher.Extensions

Public Class frmLauncher
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents radProduction As System.Windows.Forms.RadioButton
    Friend WithEvents radBranches As System.Windows.Forms.RadioButton
    Friend WithEvents grpBranch As System.Windows.Forms.GroupBox
    Friend WithEvents grpDetails As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cboBranches As System.Windows.Forms.ComboBox
    Friend WithEvents lblDisplayName As System.Windows.Forms.Label
    Friend WithEvents lblBranchDescription As System.Windows.Forms.Label
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents lblRevision As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lblBranchContact As System.Windows.Forms.Label
    Friend WithEvents lblVersionNo As System.Windows.Forms.Label
    Friend WithEvents radTraining As System.Windows.Forms.RadioButton
    Friend WithEvents cboTraining As System.Windows.Forms.ComboBox
    Friend WithEvents chkForceRefresh As System.Windows.Forms.CheckBox
    Friend WithEvents lblRevisionInfo As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lblStatusCurrent As System.Windows.Forms.Label
    Friend WithEvents lblStatusUpdate As System.Windows.Forms.Label
    Friend WithEvents pnlTraining As System.Windows.Forms.Panel
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLauncher))
        Me.radProduction = New System.Windows.Forms.RadioButton()
        Me.radBranches = New System.Windows.Forms.RadioButton()
        Me.cboBranches = New System.Windows.Forms.ComboBox()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.grpBranch = New System.Windows.Forms.GroupBox()
        Me.chkForceRefresh = New System.Windows.Forms.CheckBox()
        Me.cboTraining = New System.Windows.Forms.ComboBox()
        Me.grpDetails = New System.Windows.Forms.GroupBox()
        Me.lblStatusCurrent = New System.Windows.Forms.Label()
        Me.lblStatusUpdate = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lblRevisionInfo = New System.Windows.Forms.Label()
        Me.lblBranchContact = New System.Windows.Forms.Label()
        Me.lblVersionNo = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblRevision = New System.Windows.Forms.Label()
        Me.lblDisplayName = New System.Windows.Forms.Label()
        Me.lblBranchDescription = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.radTraining = New System.Windows.Forms.RadioButton()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.pnlTraining = New System.Windows.Forms.Panel()
        Me.grpBranch.SuspendLayout()
        Me.grpDetails.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlTraining.SuspendLayout()
        Me.SuspendLayout()
        '
        'radProduction
        '
        Me.radProduction.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.radProduction.Location = New System.Drawing.Point(16, 32)
        Me.radProduction.Name = "radProduction"
        Me.radProduction.Size = New System.Drawing.Size(88, 16)
        Me.radProduction.TabIndex = 0
        Me.radProduction.Text = "Production"
        Me.radProduction.UseVisualStyleBackColor = False
        '
        'radBranches
        '
        Me.radBranches.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.radBranches.Location = New System.Drawing.Point(16, 64)
        Me.radBranches.Name = "radBranches"
        Me.radBranches.Size = New System.Drawing.Size(72, 21)
        Me.radBranches.TabIndex = 1
        Me.radBranches.Text = "Testing"
        Me.radBranches.UseVisualStyleBackColor = False
        '
        'cboBranches
        '
        Me.cboBranches.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBranches.Location = New System.Drawing.Point(112, 64)
        Me.cboBranches.Name = "cboBranches"
        Me.cboBranches.Size = New System.Drawing.Size(256, 21)
        Me.cboBranches.TabIndex = 2
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.BackColor = System.Drawing.Color.White
        Me.btnOK.Enabled = False
        Me.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOK.Image = CType(resources.GetObject("btnOK.Image"), System.Drawing.Image)
        Me.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnOK.Location = New System.Drawing.Point(286, 368)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(96, 23)
        Me.btnOK.TabIndex = 3
        Me.btnOK.Text = "Launch"
        Me.btnOK.UseVisualStyleBackColor = False
        '
        'grpBranch
        '
        Me.grpBranch.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.grpBranch.Controls.Add(Me.pnlTraining)
        Me.grpBranch.Controls.Add(Me.chkForceRefresh)
        Me.grpBranch.Location = New System.Drawing.Point(8, 8)
        Me.grpBranch.Name = "grpBranch"
        Me.grpBranch.Size = New System.Drawing.Size(376, 111)
        Me.grpBranch.TabIndex = 4
        Me.grpBranch.TabStop = False
        Me.grpBranch.Text = "OneVue"
        '
        'chkForceRefresh
        '
        Me.chkForceRefresh.AutoSize = True
        Me.chkForceRefresh.Location = New System.Drawing.Point(104, 25)
        Me.chkForceRefresh.Name = "chkForceRefresh"
        Me.chkForceRefresh.Size = New System.Drawing.Size(93, 17)
        Me.chkForceRefresh.TabIndex = 11
        Me.chkForceRefresh.Text = "Force Refresh"
        Me.chkForceRefresh.UseVisualStyleBackColor = True
        '
        'cboTraining
        '
        Me.cboTraining.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTraining.Enabled = False
        Me.cboTraining.Location = New System.Drawing.Point(100, 3)
        Me.cboTraining.Name = "cboTraining"
        Me.cboTraining.Size = New System.Drawing.Size(256, 21)
        Me.cboTraining.TabIndex = 3
        '
        'grpDetails
        '
        Me.grpDetails.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.grpDetails.Controls.Add(Me.lblStatusCurrent)
        Me.grpDetails.Controls.Add(Me.lblStatusUpdate)
        Me.grpDetails.Controls.Add(Me.Label6)
        Me.grpDetails.Controls.Add(Me.lblRevisionInfo)
        Me.grpDetails.Controls.Add(Me.lblBranchContact)
        Me.grpDetails.Controls.Add(Me.lblVersionNo)
        Me.grpDetails.Controls.Add(Me.Label5)
        Me.grpDetails.Controls.Add(Me.Label4)
        Me.grpDetails.Controls.Add(Me.lblRevision)
        Me.grpDetails.Controls.Add(Me.lblDisplayName)
        Me.grpDetails.Controls.Add(Me.lblBranchDescription)
        Me.grpDetails.Controls.Add(Me.Label3)
        Me.grpDetails.Controls.Add(Me.Label2)
        Me.grpDetails.Controls.Add(Me.Label1)
        Me.grpDetails.Location = New System.Drawing.Point(8, 126)
        Me.grpDetails.Name = "grpDetails"
        Me.grpDetails.Size = New System.Drawing.Size(376, 230)
        Me.grpDetails.TabIndex = 5
        Me.grpDetails.TabStop = False
        Me.grpDetails.Text = "Details"
        '
        'lblStatusCurrent
        '
        Me.lblStatusCurrent.ForeColor = System.Drawing.Color.Green
        Me.lblStatusCurrent.Location = New System.Drawing.Point(270, 20)
        Me.lblStatusCurrent.Name = "lblStatusCurrent"
        Me.lblStatusCurrent.Size = New System.Drawing.Size(90, 16)
        Me.lblStatusCurrent.TabIndex = 14
        Me.lblStatusCurrent.Text = "(Current)"
        '
        'lblStatusUpdate
        '
        Me.lblStatusUpdate.ForeColor = System.Drawing.Color.Red
        Me.lblStatusUpdate.Location = New System.Drawing.Point(270, 20)
        Me.lblStatusUpdate.Name = "lblStatusUpdate"
        Me.lblStatusUpdate.Size = New System.Drawing.Size(100, 16)
        Me.lblStatusUpdate.TabIndex = 13
        Me.lblStatusUpdate.Text = "(Update Required)"
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(19, 20)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(79, 16)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "Date Released"
        '
        'lblRevisionInfo
        '
        Me.lblRevisionInfo.BackColor = System.Drawing.Color.White
        Me.lblRevisionInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblRevisionInfo.Location = New System.Drawing.Point(104, 15)
        Me.lblRevisionInfo.Name = "lblRevisionInfo"
        Me.lblRevisionInfo.Size = New System.Drawing.Size(160, 23)
        Me.lblRevisionInfo.TabIndex = 11
        Me.lblRevisionInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblBranchContact
        '
        Me.lblBranchContact.BackColor = System.Drawing.Color.White
        Me.lblBranchContact.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblBranchContact.Location = New System.Drawing.Point(104, 199)
        Me.lblBranchContact.Name = "lblBranchContact"
        Me.lblBranchContact.Size = New System.Drawing.Size(256, 23)
        Me.lblBranchContact.TabIndex = 10
        Me.lblBranchContact.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblVersionNo
        '
        Me.lblVersionNo.BackColor = System.Drawing.Color.White
        Me.lblVersionNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblVersionNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblVersionNo.Location = New System.Drawing.Point(104, 42)
        Me.lblVersionNo.Name = "lblVersionNo"
        Me.lblVersionNo.Size = New System.Drawing.Size(160, 23)
        Me.lblVersionNo.TabIndex = 9
        Me.lblVersionNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(16, 50)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(96, 16)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "OneVue Version"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(16, 74)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(88, 16)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "SVN Revision"
        '
        'lblRevision
        '
        Me.lblRevision.BackColor = System.Drawing.Color.White
        Me.lblRevision.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblRevision.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRevision.Location = New System.Drawing.Point(104, 69)
        Me.lblRevision.Name = "lblRevision"
        Me.lblRevision.Size = New System.Drawing.Size(80, 23)
        Me.lblRevision.TabIndex = 6
        Me.lblRevision.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblDisplayName
        '
        Me.lblDisplayName.BackColor = System.Drawing.Color.White
        Me.lblDisplayName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblDisplayName.Location = New System.Drawing.Point(104, 96)
        Me.lblDisplayName.Name = "lblDisplayName"
        Me.lblDisplayName.Size = New System.Drawing.Size(192, 23)
        Me.lblDisplayName.TabIndex = 5
        Me.lblDisplayName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblBranchDescription
        '
        Me.lblBranchDescription.AutoEllipsis = True
        Me.lblBranchDescription.BackColor = System.Drawing.Color.White
        Me.lblBranchDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblBranchDescription.Location = New System.Drawing.Point(104, 123)
        Me.lblBranchDescription.Name = "lblBranchDescription"
        Me.lblBranchDescription.Size = New System.Drawing.Size(256, 72)
        Me.lblBranchDescription.TabIndex = 4
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(16, 202)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(80, 16)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Main Contact"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(16, 122)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(72, 16)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Description"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(16, 98)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(80, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Branch Name"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackColor = System.Drawing.Color.White
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Image = CType(resources.GetObject("btnCancel.Image"), System.Drawing.Image)
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(182, 368)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(96, 23)
        Me.btnCancel.TabIndex = 6
        Me.btnCancel.Text = "Close"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        Me.ErrorProvider1.DataMember = ""
        '
        'radTraining
        '
        Me.radTraining.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.radTraining.Location = New System.Drawing.Point(4, 3)
        Me.radTraining.Name = "radTraining"
        Me.radTraining.Size = New System.Drawing.Size(72, 23)
        Me.radTraining.TabIndex = 7
        Me.radTraining.Text = "Training"
        Me.radTraining.UseVisualStyleBackColor = False
        '
        'lblStatus
        '
        Me.lblStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.Location = New System.Drawing.Point(14, 368)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(160, 23)
        Me.lblStatus.TabIndex = 8
        '
        'pnlTraining
        '
        Me.pnlTraining.Controls.Add(Me.cboTraining)
        Me.pnlTraining.Controls.Add(Me.radTraining)
        Me.pnlTraining.Location = New System.Drawing.Point(4, 79)
        Me.pnlTraining.Name = "pnlTraining"
        Me.pnlTraining.Size = New System.Drawing.Size(366, 28)
        Me.pnlTraining.TabIndex = 12
        '
        'frmLauncher
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(394, 404)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.grpDetails)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.cboBranches)
        Me.Controls.Add(Me.radBranches)
        Me.Controls.Add(Me.radProduction)
        Me.Controls.Add(Me.grpBranch)
        Me.Controls.Add(Me.btnCancel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmLauncher"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "OneVue Launcher v3.0"
        Me.grpBranch.ResumeLayout(False)
        Me.grpBranch.PerformLayout()
        Me.grpDetails.ResumeLayout(False)
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlTraining.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Private Variables"
    Private Const APPLICATION_TITLE As String = "OneVue Launcher"
    Private _currentBranchIndex As Integer
    Private _currentDirectory As String = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()(0).FullyQualifiedName)
    Private _branches As New Collections.Specialized.OrderedDictionary()
    Private _working_dir_path As String = "c:\svn_testing"
    Private _repository_uri As String = "svn://launcher.fas.au.challenger.net/fas/onevuereleases2009/"
    Private Const PROD_BRANCHNAME As String = "production"
    Private Const PROD_EXENAME As String = "OneVue.exe"
    Private Shared rnd As Random = New Random()

#End Region

#Region " Private Methods"

    ''' <summary>
    ''' Load the XML Data
    ''' </summary>
    Private Sub LoadData(ByVal strFileName As String)
        Try
            'Remove all items
            _branches.Clear()
            Dim xmlDoc As New XmlDocument
            Dim branchNodeList As XmlNodeList
            Dim branchNode As XmlNode
            xmlDoc.Load(Path.Combine(_currentDirectory, strFileName))
            branchNodeList = xmlDoc.GetElementsByTagName("Branch")
            For Each branchNode In branchNodeList
                Dim baseDataNodes As XmlNodeList
                Dim baseData As XmlNode
                Dim newBranch As New Branch
                newBranch.BranchExeName = "OneVue.exe"
                baseDataNodes = branchNode.ChildNodes
                For Each baseData In baseDataNodes
                    Select Case baseData.Name
                        Case "BranchName"
                            newBranch.BranchName = baseData.InnerText
                        Case "BranchVersion"
                            newBranch.BranchVersion = baseData.InnerText
                        Case "BranchRevision"
                            newBranch.BranchRevision = baseData.InnerText
                        Case "BranchDescription"
                            newBranch.BranchDescription = baseData.InnerText
                        Case "BranchDisplayName"
                            newBranch.BranchDisplayName = baseData.InnerText
                        Case "BranchContact"
                            newBranch.BranchContact = baseData.InnerText
                        Case "BranchExeName"
                            newBranch.BranchExeName = baseData.InnerText
                        Case "BranchWorkingDirectory"
                            newBranch.BranchWorkingDirectory = baseData.InnerText
                    End Select
                Next
                If Not String.IsNullOrEmpty(newBranch.BranchName) Then
                    _branches.Add(newBranch.BranchName, newBranch)
                Else
                    ' This must be a separator that won't need to be retrieved, so just use a random key..
                    _branches.Add(GetRandomString(10), newBranch)
                End If
            Next
        Catch ex As System.Exception
            MsgBox(ex.ToString)
        End Try

    End Sub

    ''' <summary>
    ''' Generates a random string of alpha-numeric characters of the specified length.
    ''' </summary>
    ''' <param name="length">Length of the returned string</param>
    ''' <returns>A random string of alpha-numeric characters</returns>
    ''' <remarks></remarks>
    Private Function GetRandomString(ByVal length As Integer) As String
        Dim legalCharacters As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"
        Dim str As New System.Text.StringBuilder()
        Dim ch As Char
        For i As Integer = 0 To length - 1
            ch = legalCharacters(rnd.Next(0, legalCharacters.Length))
            str.Append(ch)
        Next
        Return str.ToString
    End Function

    ''' <summary>
    ''' Load the combo that displays the list of Branches
    ''' </summary>
    Private Sub LoadBranchCombo()
        cboBranches.Items.Clear()
        If _branches.Count > 0 Then
            Dim Branch As Branch
            For Each Branch In _branches.Values
                cboBranches.Items.Add(Branch.BranchDisplayName)
            Next
        End If
    End Sub

    ''' <summary>
    ''' Load the combo that displays the list of Branches
    ''' </summary>
    Private Sub LoadTrainingCombo()
        cboTraining.Items.Clear()
        If _branches.Count > 0 Then
            Dim Branch As Branch
            For Each Branch In _branches.Values
                cboTraining.Items.Add(Branch.BranchDisplayName)
            Next
        End If
    End Sub

    ''' <summary>
    ''' If Production radio button activated 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub radProduction_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radProduction.CheckedChanged
        If radProduction.Checked Then
            radBranches.Checked = False
            radTraining.Checked = False
            cboBranches.Enabled = False
            cboBranches.SelectedIndex = -1

            cboTraining.Enabled = False
            cboTraining.SelectedIndex = -1

            lblStatus.Text = ""
            lblDisplayName.Text = ""
            lblVersionNo.Text = ""
            lblRevision.Text = ""
            lblBranchDescription.Text = ""
            lblBranchContact.Text = ""
            btnOK.Enabled = True
        Else
            btnOK.Enabled = False
        End If
        update_revision_info()
    End Sub

    ''' <summary>
    ''' Form Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub frmLauncher_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim args As String() = Environment.GetCommandLineArgs()
        Dim IsIncludeProduction As Boolean = True
        Dim IsIncludeTraining As Boolean = True

        If Not String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings.Get("IncludeProduction")) Then
            IsIncludeProduction = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings.Get("IncludeProduction"))
        End If

        If Not String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings.Get("IncludeTraining")) Then
            IsIncludeTraining = System.Configuration.ConfigurationManager.AppSettings.Get("IncludeTraining")
        End If

        '
        ' See if an alternate working directory has been set in the config file..
        '
        If Not String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings.Get("WorkingDirectoryPath")) Then
            _working_dir_path = System.Configuration.ConfigurationManager.AppSettings.Get("WorkingDirectoryPath")
        End If

        '
        ' See if an alternate working directory path has been assigned
        ' via the command line. (note that the executable name is always arg(0))
        ' A working directory assigned via the command line overrides others.
        '
        For i As Int16 = 1 To args.Length - 1
            Select Case args(i).Trim().ToUpper()
                Case "-WORK_DIR"
                    If args.Length > i + 1 Then
                        _working_dir_path = args(i + 1)
                        ' Skip the next arg as we've already grabbed it..
                        i += 1
                    Else
                        ShowUsage()
                    End If
                Case Else
                    ShowUsage()
            End Select
        Next

        If IsIncludeTraining Then
            Me.pnlTraining.Visible = True
            ' Check that the Training config file is available first though..
            If String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings.Get("TrainingXMLPath")) Then
                If (Not File.Exists(_currentDirectory & "\Training.xml")) Then
                    MsgBox("File Training.xml was not found, OneVue Launcher cannot continue without it.")
                    Me.Close()
                    Exit Sub
                End If
            Else
                If (Not File.Exists(System.Configuration.ConfigurationManager.AppSettings.Get("TrainingXMLPath"))) Then
                    MsgBox("The Training data file (" + System.Configuration.ConfigurationManager.AppSettings.Get("TrainingXMLPath") + ") was not found, OneVue Launcher cannot continue without it.")
                    Me.Close()
                    Exit Sub
                End If
            End If
        Else
            Me.pnlTraining.Visible = False
        End If

        If IsIncludeProduction Then
            Me.radProduction.Visible = True
        Else
            Me.radProduction.Visible = False
        End If

        ' Now check that the Branches config file is available and load it..
        If String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings.Get("BranchesXMLPath")) Then
            If (Not File.Exists(_currentDirectory & "\Branches.xml")) Then
                MsgBox("File Branches.xml was not found, OneVue Launcher cannot continue without it.")
                Me.Close()
                Exit Sub
            End If
        Else
            If (Not File.Exists(System.Configuration.ConfigurationManager.AppSettings.Get("BranchesXMLPath"))) Then
                MsgBox("The Branches data file (" + System.Configuration.ConfigurationManager.AppSettings.Get("BranchesXMLPath") + ") was not found, OneVue Launcher cannot continue without it.")
                Me.Close()
                Exit Sub
            End If
        End If

        If Not String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings.Get("RepositoryURI")) Then
            _repository_uri = System.Configuration.ConfigurationManager.AppSettings.Get("RepositoryURI")
        End If

        ' Default is to load Branches. Setting the radio button calls 
        ' the OnChange handler which loads the file data..
        Me.radBranches.Checked = True
        lblStatusCurrent.Visible = False
        lblStatusUpdate.Visible = False
        LoadBranchCombo()
        Me.Text = GetVersionNo()

        If Not String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings.Get("BackColour")) Then
            Dim custom_colour As System.Drawing.Color
            custom_colour = CType(System.ComponentModel.TypeDescriptor.GetConverter(GetType(Color)).ConvertFromString(System.Configuration.ConfigurationManager.AppSettings.Get("BackColour")), Color)
            Me.BackColor = custom_colour
            pnlTraining.BackColor = custom_colour
            grpBranch.BackColor = custom_colour
            grpDetails.BackColor = custom_colour
            radBranches.BackColor = custom_colour
            radProduction.BackColor = custom_colour
            radTraining.BackColor = custom_colour
        End If

    End Sub

    ''' <summary>
    ''' Displays program usage and quits.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ShowUsage()
        ShowUsage("", True)
    End Sub

    ''' <summary>
    ''' Displays program usage and quits if required.
    ''' </summary>
    ''' <param name="Message">An optional message to display.</param>
    ''' <param name="CloseForm">Closes the form when True</param>
    ''' <remarks></remarks>
    Private Sub ShowUsage(ByVal Message As String, ByVal CloseForm As Boolean)
        Dim filename As String = System.Reflection.Assembly.GetExecutingAssembly().CodeBase
        filename = IO.Path.GetFileNameWithoutExtension(filename)
        MessageBox.Show(filename + " [-WORK_DIR <path to svn_testing dir>] " _
                        + vbCrLf + Message, "Usage", MessageBoxButtons.OK, MessageBoxIcon.Information)
        If CloseForm Then
            Me.Close()
        End If
    End Sub

    ''' <summary>
    ''' Get the Version No
    ''' </summary>
    ''' <returns></returns>
    Private Function GetVersionNo() As String
        Dim NameAndVersionNo As String
        Dim VersionMajor As String = System.Reflection.Assembly.GetExecutingAssembly.GetName().Version.Major.ToString
        Dim VersionMinor As String = System.Reflection.Assembly.GetExecutingAssembly.GetName().Version.Minor.ToString

        If Not String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings.Get("ApplicationTitle")) Then
            NameAndVersionNo = String.Format("{0} {1}.{2}", System.Configuration.ConfigurationManager.AppSettings.Get("ApplicationTitle"), VersionMajor, VersionMinor)
        Else
            NameAndVersionNo = String.Format("{0} {1}.{2}", APPLICATION_TITLE, VersionMajor, VersionMinor)
        End If

        Return NameAndVersionNo
    End Function

    ''' <summary>
    ''' Combo box index changed 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub cboBranches_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboBranches.SelectedIndexChanged
        Dim index As Integer = cboBranches.SelectedIndex
        lblStatus.Text = ""
        If Not index = -1 Then
            '
            ' If warning, then clear warning as we have just selected a branch
            ErrorProvider1.SetError(cboBranches, "")
            radBranches.Checked = True
            _currentBranchIndex = index
            Dim theSelectedBranch As Branch = _branches(index)
            lblDisplayName.Text = theSelectedBranch.BranchName
            lblVersionNo.Text = theSelectedBranch.BranchVersion
            lblRevision.Text = theSelectedBranch.BranchRevision
            lblBranchDescription.Text = theSelectedBranch.BranchDescription
            lblBranchContact.Text = theSelectedBranch.BranchContact
            If theSelectedBranch.BranchName Is Nothing Then
                btnOK.Enabled = False
            Else
                btnOK.Enabled = True
            End If
        Else
            btnOK.Enabled = False
        End If
        update_revision_info()
    End Sub

    ''' <summary>
    ''' Combo box index changed 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub cboTraining_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboTraining.SelectedIndexChanged
        Dim index As Integer = cboTraining.SelectedIndex
        lblStatus.Text = ""
        If Not index = -1 Then
            '
            ' If warning, then clear warning as we have just selected a branch
            ErrorProvider1.SetError(cboTraining, "")
            radTraining.Checked = True
            _currentBranchIndex = index
            Dim theSelectedBranch As Branch = _branches(index)
            lblDisplayName.Text = theSelectedBranch.BranchName
            lblVersionNo.Text = theSelectedBranch.BranchVersion
            lblRevision.Text = theSelectedBranch.BranchRevision
            lblBranchDescription.Text = theSelectedBranch.BranchDescription
            lblBranchContact.Text = theSelectedBranch.BranchContact
            If theSelectedBranch.BranchName Is Nothing Then
                btnOK.Enabled = False
            Else
                btnOK.Enabled = True
            End If
        Else
            btnOK.Enabled = False
        End If
        update_revision_info()
    End Sub

    ''' <summary>
    ''' Call the batch file to update
    ''' </summary>
    Private Sub UpdateBranch()
        Dim branch_path As String
        Dim branch_working_directory As String

        Try
            lblStatus.Text = "Updating branch"
            Application.DoEvents()

            Dim frmCheckout As New frmCheckout()
            frmCheckout.RepositoryURI = _repository_uri
            frmCheckout.StartPosition = FormStartPosition.CenterParent
            '
            ' Set the branch name depending on the selected form option..
            '
            If radBranches.Checked Or radTraining.Checked Then
                frmCheckout.Branch = lblDisplayName.Text
                frmCheckout.Revision = lblRevision.Text
                '
                ' Set the branch working directory to either the default or a specifically configured one..
                '
                branch_working_directory = CType(_branches(lblDisplayName.Text), Branch).BranchWorkingDirectory
                If String.IsNullOrEmpty(branch_working_directory) Then
                    branch_working_directory = _working_dir_path
                End If
                frmCheckout.WorkingDir = branch_working_directory
                branch_path = IO.Path.Combine(branch_working_directory, lblDisplayName.Text)
            Else
                branch_path = _working_dir_path
                frmCheckout.Branch = PROD_BRANCHNAME
                frmCheckout.Revision = "HEAD"
                frmCheckout.WorkingDir = branch_path
            End If

            Dim is_ok_to_launch As Boolean = False
            Dim checkout_result As Windows.Forms.DialogResult = Windows.Forms.DialogResult.None
            Dim dialog_result As Windows.Forms.DialogResult

            checkout_result = frmCheckout.ShowDialog(Me)

            If checkout_result = Windows.Forms.DialogResult.OK Then
                lblStatus.Text = "rev. " + frmCheckout.CheckedOutRevision.ToString()
                Application.DoEvents()
                If validate_branch_revision(frmCheckout.CheckedOutRevision) Then
                    is_ok_to_launch = True
                Else
                    is_ok_to_launch = False
                    MessageBox.Show("SVN revision validation failed." + vbCrLf _
                                                    + "Requested r" + lblRevision.Text _
                                                    + ", but retrieved r" + frmCheckout.CheckedOutRevision.ToString, "Error", _
                                                    MessageBoxButtons.OK, _
                                                    MessageBoxIcon.Exclamation)
                End If

            Else
                is_ok_to_launch = False
                If checkout_result <> Windows.Forms.DialogResult.Cancel Then
                    If Not String.IsNullOrEmpty(frmCheckout.ErrorMessage) Then
                        MessageBox.Show("SVN checkout failed." + vbCrLf _
                                                        + frmCheckout.ErrorMessage, "Error", _
                                                        MessageBoxButtons.OK, _
                                                        MessageBoxIcon.Exclamation)
                    Else
                        Dim rev As Long = -1
                        If Long.TryParse(lblRevision.Text, rev) Then
                            If frmCheckout.CheckedOutRevision <> lblRevision.Text Then
                                MessageBox.Show("SVN revision validation failed." + vbCrLf _
                                        + "Requested r" + lblRevision.Text _
                                        + ", but retrieved r" + frmCheckout.CheckedOutRevision, "Error", _
                                        MessageBoxButtons.OK, _
                                        MessageBoxIcon.Exclamation)
                            End If
                        Else
                            MessageBox.Show("SVN revision validation failed." + vbCrLf, _
                                            "Error", _
                                            MessageBoxButtons.OK, _
                                            MessageBoxIcon.Exclamation)
                        End If

                    End If
                End If
            End If

            set_working_copy_current_status(is_ok_to_launch)

            '
            ' Prompt user if branch retrieval fails..
            '
            If Not is_ok_to_launch Then
                If checkout_result = Windows.Forms.DialogResult.Cancel Then
                    lblStatus.Text = "Update cancelled."
                    Application.DoEvents()
                Else
                    lblStatus.Text = "Update failed."
                    Application.DoEvents()
                    If System.IO.Directory.Exists(branch_path) Then
                        dialog_result = MessageBox.Show("Launcher failed to retrieve the specified SVN revision." + vbCrLf _
                                                        + "Do you wish to run the previous version?", "Error", _
                                                        MessageBoxButtons.YesNo, _
                                                        MessageBoxIcon.Exclamation, _
                                                        MessageBoxDefaultButton.Button2)
                        If dialog_result = Windows.Forms.DialogResult.Yes Then
                            is_ok_to_launch = True
                        End If
                    Else
                        MessageBox.Show("Launcher failed to retrieve the specified SVN revision.", _
                                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                        is_ok_to_launch = False
                    End If
                End If
            End If

            '
            ' Launch retrieved executable..
            '
            If is_ok_to_launch Then
                ExecuteBranch()
            End If

        Catch ex As System.Exception
            lblStatus.Text = "Error"
            MsgBox(ex.ToString)
        End Try

    End Sub

    ''' <summary>
    ''' Launches the currently selected branch executable.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ExecuteBranch()
        Dim working_copy_path As String = get_current_working_copy_path()

        Dim executable_path As String
        If radBranches.Checked Or radTraining.Checked Then
            executable_path = IO.Path.Combine(working_copy_path, CType(_branches(_currentBranchIndex), Branch).BranchExeName)
        Else
            executable_path = IO.Path.Combine(working_copy_path, PROD_EXENAME)
        End If
        If Not IO.File.Exists(executable_path) Then
            Throw New ApplicationException("File not found: """ + executable_path + """")
        End If
        Shell("""" + executable_path + """", AppWinStyle.NormalFocus, False)
    End Sub

    ''' <summary>
    ''' If a revision number is specified for the branch, check if it 
    ''' is the same as the supplied revision number.
    ''' </summary>
    ''' <param name="revision">The retrieved revision number</param>
    ''' <returns>
    ''' Boolean True if the supplied revision is the same as 
    ''' that specified on the form, False otherwise.
    ''' </returns>
    ''' <remarks></remarks>
    Private Function validate_branch_revision(ByVal revision As Integer) As Boolean
        Dim result As Boolean = True
        Dim requested_revision As Integer
        '
        ' We only want to validate if an actual revision number has been 
        ' specified for the branch rather than "HEAD", "BASE", etc.
        '
        If Integer.TryParse(lblRevision.Text, requested_revision) Then
            result = revision = requested_revision
        End If
        Return result
    End Function

    ''' <summary>
    ''' Deletes the current selected branch working directory.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function DeleteBranch() As Boolean
        Dim result As Boolean = False
        Dim branch_path As String = String.Empty
        Dim branch_working_directory As String = String.Empty
        Try
            lblStatus.Text = "Deleting branch"
            Application.DoEvents()
            branch_path = get_current_working_copy_path()
            If System.IO.Directory.Exists(branch_path) Then
                DeleteDirectory(branch_path)
            End If
            result = True
        Catch ex As System.Exception
            lblStatus.Text = "Error"
            MsgBox(ex.ToString)
        End Try
        Return result
    End Function

    ''' <summary>
    ''' Gets the working directory of the currently selected branch.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function get_current_working_copy_path() As String
        Dim branch_path As String = String.Empty
        Dim branch_working_directory As String = String.Empty
        If radBranches.Checked Or radTraining.Checked Then
            If Not String.IsNullOrEmpty(lblDisplayName.Text) Then
                branch_working_directory = CType(_branches(lblDisplayName.Text), Branch).BranchWorkingDirectory
                If String.IsNullOrEmpty(branch_working_directory) Then
                    branch_working_directory = _working_dir_path
                End If
                branch_path = IO.Path.Combine(branch_working_directory, lblDisplayName.Text)
            End If
        Else
            branch_working_directory = _working_dir_path
            branch_path = IO.Path.Combine(branch_working_directory, PROD_BRANCHNAME)
        End If
        Return branch_path
    End Function

    ''' <summary>
    ''' Deletes a directory tree ensuring any read-only files are also deleted
    ''' </summary>
    ''' <param name="target_dir"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteDirectory(ByVal target_dir As String) As Boolean
        Dim result As Boolean = False
        Dim files As String() = Directory.GetFiles(target_dir)
        Dim dirs As String() = Directory.GetDirectories(target_dir)

        For Each file_name As String In files
            File.SetAttributes(file_name, FileAttributes.Normal)
            File.Delete(file_name)
        Next

        For Each dir As String In dirs
            DeleteDirectory(dir)
        Next

        Directory.Delete(target_dir, True)

        Return result
    End Function

    ''' <summary>
    ''' OK button clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Dim success As Boolean = True
        Dim branch_working_directory As String
        ErrorProvider1.SetError(cboBranches, "")
        ErrorProvider1.SetError(cboTraining, "")
        Try
            Me.Cursor = Cursors.WaitCursor
            If lblStatusCurrent.Visible And Not chkForceRefresh.Checked Then
                ExecuteBranch()
            Else
                '
                ' Ensure the specified working directory path is valid..
                '
                branch_working_directory = get_current_working_copy_path()
                If Not IO.Directory.Exists(branch_working_directory) Then
                    Dim inf As IO.DirectoryInfo
                    inf = IO.Directory.CreateDirectory(branch_working_directory)
                End If
                If chkForceRefresh.Checked Then
                    success = DeleteBranch()
                End If
                If success Then
                    UpdateBranch()
                End If
            End If
        Catch ex As System.Exception
            MessageBox.Show(ex.Format(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If lblStatusCurrent.Visible Then
                set_working_copy_current_status(False)
            End If
            Exit Sub
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary>
    ''' Close the App
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub radTraining_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radTraining.CheckedChanged
        If radTraining.Checked Then
            radProduction.Checked = False
            radBranches.Checked = False
            Me.Cursor = Cursors.WaitCursor
            cboBranches.Enabled = False
            cboBranches.SelectedIndex = -1

            lblDisplayName.Text = ""
            lblVersionNo.Text = ""
            lblRevision.Text = ""
            lblBranchDescription.Text = ""
            lblBranchContact.Text = ""

            cboTraining.Enabled = True
            If String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings.Get("TrainingXMLPath")) Then
                LoadData(_currentDirectory & "\Training.xml")
            Else
                LoadData(System.Configuration.ConfigurationManager.AppSettings.Get("TrainingXMLPath"))
            End If

            LoadTrainingCombo()
            Me.Cursor = Cursors.Default
            update_revision_info()
        End If
    End Sub

    Private Sub radBranches_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radBranches.CheckedChanged
        If radBranches.Checked Then
            radProduction.Checked = False
            radTraining.Checked = False
            Me.Cursor = Cursors.WaitCursor
            cboTraining.Enabled = False
            cboTraining.SelectedIndex = -1

            lblStatus.Text = String.Empty

            lblDisplayName.Text = ""
            lblVersionNo.Text = ""
            lblRevision.Text = ""
            lblBranchDescription.Text = ""
            lblBranchContact.Text = ""

            cboBranches.Enabled = True

            'LoadData("Branches.xml")
            If String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings.Get("BranchesXMLPath")) Then
                LoadData(_currentDirectory & "\Branches.xml")
            Else
                LoadData(System.Configuration.ConfigurationManager.AppSettings.Get("BranchesXMLPath"))
            End If

            LoadBranchCombo()
            Me.Cursor = Cursors.Default
            update_revision_info()
        End If
    End Sub

    ''' <summary>
    ''' chkForceRefresh CheckedChanged handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub chkForceRefresh_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                                                                    Handles chkForceRefresh.CheckedChanged
        lblStatus.Text = ""
    End Sub

    ''' <summary>
    ''' Gets the current selected branch info.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub update_revision_info()
        Dim branch As String
        Dim revision As String
        Dim last_changed_datetime As DateTime?
        Dim last_changed_author As String = String.Empty
        Dim last_changed_revision As Long?

        Try
            Me.Cursor = Cursors.WaitCursor
            Me.lblRevisionInfo.Text = ""
            lblStatusCurrent.Visible = False
            lblStatusUpdate.Visible = False
            If Not String.IsNullOrEmpty(lblDisplayName.Text) Or radProduction.Checked Then
                '
                ' Set the branch name depending on the selected form option..
                '
                If radBranches.Checked Or radTraining.Checked Then
                    branch = lblDisplayName.Text
                    revision = lblRevision.Text
                Else
                    branch = PROD_BRANCHNAME
                    revision = "HEAD"
                End If

                libSVN.SVN.GetRevisionInfo(IO.Path.Combine(_repository_uri, branch), _
                                           revision, _
                                           last_changed_datetime, _
                                           last_changed_author, _
                                           last_changed_revision)
                If last_changed_datetime.HasValue Then
                    Me.lblRevisionInfo.Text = IIf(last_changed_datetime.HasValue, _
                                                  last_changed_datetime.ToString(), "").ToString
                End If
                If last_changed_revision.HasValue Then
                    Dim working_copy_path As String = get_current_working_copy_path()
                    set_working_copy_current_status(is_working_copy_version(working_copy_path, _
                                                                            last_changed_revision.Value))
                End If
            End If
        Catch ex As System.Exception
            MessageBox.Show(ex.Format(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary>
    ''' Updates the Working Copy Status labels.
    ''' </summary>
    ''' <param name="IsCurrent"></param>
    ''' <remarks></remarks>
    Private Sub set_working_copy_current_status(ByVal IsCurrent As Boolean)
        lblStatusUpdate.Visible = Not IsCurrent
        lblStatusCurrent.Visible = IsCurrent
    End Sub

    ''' <summary>
    ''' Verifies that the supplied SVN working copy path is at the same revision as the supplied value.
    ''' </summary>
    ''' <param name="WorkingCopyPath"></param>
    ''' <param name="Revision"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function is_working_copy_version(ByVal WorkingCopyPath As String, ByVal Revision As Long) As Boolean
        Dim is_current_version As Boolean = False
        If Not String.IsNullOrEmpty(WorkingCopyPath) Then
            If libSVN.SVN.IsWorkingCopyVersioned(WorkingCopyPath) Then
                If libSVN.SVN.GetWorkingCopyRevision(WorkingCopyPath) = Revision Then
                    is_current_version = True
                End If
            End If
        End If
        Return is_current_version
    End Function

#End Region

End Class
