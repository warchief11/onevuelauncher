using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
//'****************************************************************************************************
//Class:	frmLauncher
//
//	Form for automating the downloading of OneVue test builds from the SVN launcher repository at
//	svn://launcher.fas.au.challenger.net/fas/onevuereleases
//
//	Please refer to the FAS Notebook document for information on configuring workstation .NET security 
//	policies to enable this application to run over the network:
//
//   onenote:///L:\IT\Funds%20&%20Life%20IT\FAS\Documentation\OneNote\OneVue%20Documentation\OneVue\System%20Documentation\Build%20and%20Deploy.one#Launcher%20-%20Installation%20Notes&section-id={5E7F9CB8-2655-4439-A30D-D45E0EE3FB91}&page-id={A50FC9E4-DB94-4D8A-A3A8-F861BF401873}&end
//   
// ---------------------------------------------------------------------------------------------------
//
//    Modification(History)
//
//Date           Developer   Description
//----------------------------------------------------------------------------------------------------
//24/4/2007      HHa         Always copy the svn_testing_template files and added a label to show status
//08-Oct-2008    CMadden     VS2008 upgrade - Added param type to LoadData() and added config file for
//                           storing the location of the Branches and Training XML data files to enable 
//                           running versions of the application from different locations, using common
//                           data files.
//07-Jan-2009    HHa         Added template path to configuration options.
//01-Nov-2011    CMadden     Updated the updatereleases.bat batch file to return the branch revision 
//                           number and use that to display the revision number in the status string.  
//                           If the SVN command fails to retrieve a valid revision number for any reason, 
//                           displays "Update failed." as the status and prompts the user to ask if they 
//                           wish to run the previous version.  Added "Force Update" option to delete a 
//                           branch prior to updating for situations when updates fail.
//01-Dec-2011    CMadden     Added revision validation.  Removed call to runOneVue.bat in favour of 
//                           running executable directly.
//12-Feb-2013    CMadden     Added -WORK_DIR command line arg to override standard working directory 
//                           location (required for VDI use). Rejigged svn_testing directory structure 
//                           and updated batch files to suit.
//16-Jul-2013    CMadden     Added checking for TemplatePath existence in MakeDirStructure()
//26-Jul-2013    CMadden     v3.0 
//                             - Replaced template SVN batch files with libSVN API
//                             - Added frmCheckout progress dialog
//                             - Added RepositoryURI and WorkingDirectoryPath app.Config values to 
//                               enabled remote working directory e.g. U:\OneVueLauncherWorking.
//30-Jul-2013    CMadden     Added optional branch BranchWorkingDirectory property to enable an individual 
//                           branch to use a different working copy path.  This is useful in cases where 
//                           the default WorkingDirectoryPath is on a network drive, but a particular app 
//                           will only execute from a local drive.
//31-Jul-2013    CMadden     v3.1 Added "Date Released" item to branch details and checking of the branch 
//                           working copy to determine if it is at the current revision prior to checkout 
//                           to improve performance.
//22-Jan-2015    CMadden     Parameterised the Production and Training options to permit a separate 
//                           Ambit-specific Launcher instance.
//*******************************************************************************************************/

using System.Xml;
using System.IO;
using OneVueLauncher.Extensions;
namespace OneVueLauncher
{
	public class frmLauncher : System.Windows.Forms.Form
	{

		#region " Windows Form Designer generated code "

		public frmLauncher() : base()
		{
			Load += frmLauncher_Load;

			//This call is required by the Windows Form Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call

		}

		//Form overrides dispose to clean up the component list.
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if ((components != null)) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		//Required by the Windows Form Designer

		private System.ComponentModel.IContainer components;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		private System.Windows.Forms.RadioButton withEventsField_radProduction;
		internal System.Windows.Forms.RadioButton radProduction {
			get { return withEventsField_radProduction; }
			set {
				if (withEventsField_radProduction != null) {
					withEventsField_radProduction.CheckedChanged -= radProduction_CheckedChanged;
				}
				withEventsField_radProduction = value;
				if (withEventsField_radProduction != null) {
					withEventsField_radProduction.CheckedChanged += radProduction_CheckedChanged;
				}
			}
		}
		private System.Windows.Forms.RadioButton withEventsField_radBranches;
		internal System.Windows.Forms.RadioButton radBranches {
			get { return withEventsField_radBranches; }
			set {
				if (withEventsField_radBranches != null) {
					withEventsField_radBranches.CheckedChanged -= radBranches_CheckedChanged;
				}
				withEventsField_radBranches = value;
				if (withEventsField_radBranches != null) {
					withEventsField_radBranches.CheckedChanged += radBranches_CheckedChanged;
				}
			}
		}
		internal System.Windows.Forms.GroupBox grpBranch;
		internal System.Windows.Forms.GroupBox grpDetails;
		internal System.Windows.Forms.Label Label1;
		internal System.Windows.Forms.Label Label2;
		internal System.Windows.Forms.Label Label3;
		private System.Windows.Forms.ComboBox withEventsField_cboBranches;
		internal System.Windows.Forms.ComboBox cboBranches {
			get { return withEventsField_cboBranches; }
			set {
				if (withEventsField_cboBranches != null) {
					withEventsField_cboBranches.SelectedIndexChanged -= cboBranches_SelectedIndexChanged;
				}
				withEventsField_cboBranches = value;
				if (withEventsField_cboBranches != null) {
					withEventsField_cboBranches.SelectedIndexChanged += cboBranches_SelectedIndexChanged;
				}
			}
		}
		internal System.Windows.Forms.Label lblDisplayName;
		internal System.Windows.Forms.Label lblBranchDescription;
		private System.Windows.Forms.Button withEventsField_btnOK;
		internal System.Windows.Forms.Button btnOK {
			get { return withEventsField_btnOK; }
			set {
				if (withEventsField_btnOK != null) {
					withEventsField_btnOK.Click -= btnOK_Click;
				}
				withEventsField_btnOK = value;
				if (withEventsField_btnOK != null) {
					withEventsField_btnOK.Click += btnOK_Click;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_btnCancel;
		internal System.Windows.Forms.Button btnCancel {
			get { return withEventsField_btnCancel; }
			set {
				if (withEventsField_btnCancel != null) {
					withEventsField_btnCancel.Click -= btnCancel_Click;
				}
				withEventsField_btnCancel = value;
				if (withEventsField_btnCancel != null) {
					withEventsField_btnCancel.Click += btnCancel_Click;
				}
			}
		}
		internal System.Windows.Forms.ErrorProvider ErrorProvider1;
		internal System.Windows.Forms.Label lblRevision;
		internal System.Windows.Forms.Label Label4;
		internal System.Windows.Forms.Label Label5;
		internal System.Windows.Forms.Label lblBranchContact;
		internal System.Windows.Forms.Label lblVersionNo;
		private System.Windows.Forms.RadioButton withEventsField_radTraining;
		internal System.Windows.Forms.RadioButton radTraining {
			get { return withEventsField_radTraining; }
			set {
				if (withEventsField_radTraining != null) {
					withEventsField_radTraining.CheckedChanged -= radTraining_CheckedChanged;
				}
				withEventsField_radTraining = value;
				if (withEventsField_radTraining != null) {
					withEventsField_radTraining.CheckedChanged += radTraining_CheckedChanged;
				}
			}
		}
		private System.Windows.Forms.ComboBox withEventsField_cboTraining;
		internal System.Windows.Forms.ComboBox cboTraining {
			get { return withEventsField_cboTraining; }
			set {
				if (withEventsField_cboTraining != null) {
					withEventsField_cboTraining.SelectedIndexChanged -= cboTraining_SelectedIndexChanged;
				}
				withEventsField_cboTraining = value;
				if (withEventsField_cboTraining != null) {
					withEventsField_cboTraining.SelectedIndexChanged += cboTraining_SelectedIndexChanged;
				}
			}
		}
		private System.Windows.Forms.CheckBox withEventsField_chkForceRefresh;
		internal System.Windows.Forms.CheckBox chkForceRefresh {
			get { return withEventsField_chkForceRefresh; }
			set {
				if (withEventsField_chkForceRefresh != null) {
					withEventsField_chkForceRefresh.CheckedChanged -= chkForceRefresh_CheckedChanged;
				}
				withEventsField_chkForceRefresh = value;
				if (withEventsField_chkForceRefresh != null) {
					withEventsField_chkForceRefresh.CheckedChanged += chkForceRefresh_CheckedChanged;
				}
			}
		}
		internal System.Windows.Forms.Label lblRevisionInfo;
		internal System.Windows.Forms.Label Label6;
		internal System.Windows.Forms.Label lblStatusCurrent;
		internal System.Windows.Forms.Label lblStatusUpdate;
		internal System.Windows.Forms.Panel pnlTraining;
		internal System.Windows.Forms.Label lblStatus;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLauncher));
			this.radProduction = new System.Windows.Forms.RadioButton();
			this.radBranches = new System.Windows.Forms.RadioButton();
			this.cboBranches = new System.Windows.Forms.ComboBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.grpBranch = new System.Windows.Forms.GroupBox();
			this.chkForceRefresh = new System.Windows.Forms.CheckBox();
			this.cboTraining = new System.Windows.Forms.ComboBox();
			this.grpDetails = new System.Windows.Forms.GroupBox();
			this.lblStatusCurrent = new System.Windows.Forms.Label();
			this.lblStatusUpdate = new System.Windows.Forms.Label();
			this.Label6 = new System.Windows.Forms.Label();
			this.lblRevisionInfo = new System.Windows.Forms.Label();
			this.lblBranchContact = new System.Windows.Forms.Label();
			this.lblVersionNo = new System.Windows.Forms.Label();
			this.Label5 = new System.Windows.Forms.Label();
			this.Label4 = new System.Windows.Forms.Label();
			this.lblRevision = new System.Windows.Forms.Label();
			this.lblDisplayName = new System.Windows.Forms.Label();
			this.lblBranchDescription = new System.Windows.Forms.Label();
			this.Label3 = new System.Windows.Forms.Label();
			this.Label2 = new System.Windows.Forms.Label();
			this.Label1 = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.ErrorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
			this.radTraining = new System.Windows.Forms.RadioButton();
			this.lblStatus = new System.Windows.Forms.Label();
			this.pnlTraining = new System.Windows.Forms.Panel();
			this.grpBranch.SuspendLayout();
			this.grpDetails.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.ErrorProvider1).BeginInit();
			this.pnlTraining.SuspendLayout();
			this.SuspendLayout();
			//
			//radProduction
			//
			this.radProduction.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(224)), Convert.ToInt32(Convert.ToByte(224)), Convert.ToInt32(Convert.ToByte(224)));
			this.radProduction.Location = new System.Drawing.Point(16, 32);
			this.radProduction.Name = "radProduction";
			this.radProduction.Size = new System.Drawing.Size(88, 16);
			this.radProduction.TabIndex = 0;
			this.radProduction.Text = "Production";
			this.radProduction.UseVisualStyleBackColor = false;
			//
			//radBranches
			//
			this.radBranches.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(224)), Convert.ToInt32(Convert.ToByte(224)), Convert.ToInt32(Convert.ToByte(224)));
			this.radBranches.Location = new System.Drawing.Point(16, 64);
			this.radBranches.Name = "radBranches";
			this.radBranches.Size = new System.Drawing.Size(72, 21);
			this.radBranches.TabIndex = 1;
			this.radBranches.Text = "Testing";
			this.radBranches.UseVisualStyleBackColor = false;
			//
			//cboBranches
			//
			this.cboBranches.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboBranches.Location = new System.Drawing.Point(112, 64);
			this.cboBranches.Name = "cboBranches";
			this.cboBranches.Size = new System.Drawing.Size(256, 21);
			this.cboBranches.TabIndex = 2;
			//
			//btnOK
			//
			this.btnOK.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnOK.BackColor = System.Drawing.Color.White;
			this.btnOK.Enabled = false;
			this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnOK.Image = (System.Drawing.Image)resources.GetObject("btnOK.Image");
			this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnOK.Location = new System.Drawing.Point(286, 368);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(96, 23);
			this.btnOK.TabIndex = 3;
			this.btnOK.Text = "Launch";
			this.btnOK.UseVisualStyleBackColor = false;
			//
			//grpBranch
			//
			this.grpBranch.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(224)), Convert.ToInt32(Convert.ToByte(224)), Convert.ToInt32(Convert.ToByte(224)));
			this.grpBranch.Controls.Add(this.pnlTraining);
			this.grpBranch.Controls.Add(this.chkForceRefresh);
			this.grpBranch.Location = new System.Drawing.Point(8, 8);
			this.grpBranch.Name = "grpBranch";
			this.grpBranch.Size = new System.Drawing.Size(376, 111);
			this.grpBranch.TabIndex = 4;
			this.grpBranch.TabStop = false;
			this.grpBranch.Text = "OneVue";
			//
			//chkForceRefresh
			//
			this.chkForceRefresh.AutoSize = true;
			this.chkForceRefresh.Location = new System.Drawing.Point(104, 25);
			this.chkForceRefresh.Name = "chkForceRefresh";
			this.chkForceRefresh.Size = new System.Drawing.Size(93, 17);
			this.chkForceRefresh.TabIndex = 11;
			this.chkForceRefresh.Text = "Force Refresh";
			this.chkForceRefresh.UseVisualStyleBackColor = true;
			//
			//cboTraining
			//
			this.cboTraining.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboTraining.Enabled = false;
			this.cboTraining.Location = new System.Drawing.Point(100, 3);
			this.cboTraining.Name = "cboTraining";
			this.cboTraining.Size = new System.Drawing.Size(256, 21);
			this.cboTraining.TabIndex = 3;
			//
			//grpDetails
			//
			this.grpDetails.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(224)), Convert.ToInt32(Convert.ToByte(224)), Convert.ToInt32(Convert.ToByte(224)));
			this.grpDetails.Controls.Add(this.lblStatusCurrent);
			this.grpDetails.Controls.Add(this.lblStatusUpdate);
			this.grpDetails.Controls.Add(this.Label6);
			this.grpDetails.Controls.Add(this.lblRevisionInfo);
			this.grpDetails.Controls.Add(this.lblBranchContact);
			this.grpDetails.Controls.Add(this.lblVersionNo);
			this.grpDetails.Controls.Add(this.Label5);
			this.grpDetails.Controls.Add(this.Label4);
			this.grpDetails.Controls.Add(this.lblRevision);
			this.grpDetails.Controls.Add(this.lblDisplayName);
			this.grpDetails.Controls.Add(this.lblBranchDescription);
			this.grpDetails.Controls.Add(this.Label3);
			this.grpDetails.Controls.Add(this.Label2);
			this.grpDetails.Controls.Add(this.Label1);
			this.grpDetails.Location = new System.Drawing.Point(8, 126);
			this.grpDetails.Name = "grpDetails";
			this.grpDetails.Size = new System.Drawing.Size(376, 230);
			this.grpDetails.TabIndex = 5;
			this.grpDetails.TabStop = false;
			this.grpDetails.Text = "Details";
			//
			//lblStatusCurrent
			//
			this.lblStatusCurrent.ForeColor = System.Drawing.Color.Green;
			this.lblStatusCurrent.Location = new System.Drawing.Point(270, 20);
			this.lblStatusCurrent.Name = "lblStatusCurrent";
			this.lblStatusCurrent.Size = new System.Drawing.Size(90, 16);
			this.lblStatusCurrent.TabIndex = 14;
			this.lblStatusCurrent.Text = "(Current)";
			//
			//lblStatusUpdate
			//
			this.lblStatusUpdate.ForeColor = System.Drawing.Color.Red;
			this.lblStatusUpdate.Location = new System.Drawing.Point(270, 20);
			this.lblStatusUpdate.Name = "lblStatusUpdate";
			this.lblStatusUpdate.Size = new System.Drawing.Size(100, 16);
			this.lblStatusUpdate.TabIndex = 13;
			this.lblStatusUpdate.Text = "(Update Required)";
			//
			//Label6
			//
			this.Label6.Location = new System.Drawing.Point(19, 20);
			this.Label6.Name = "Label6";
			this.Label6.Size = new System.Drawing.Size(79, 16);
			this.Label6.TabIndex = 12;
			this.Label6.Text = "Date Released";
			//
			//lblRevisionInfo
			//
			this.lblRevisionInfo.BackColor = System.Drawing.Color.White;
			this.lblRevisionInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblRevisionInfo.Location = new System.Drawing.Point(104, 15);
			this.lblRevisionInfo.Name = "lblRevisionInfo";
			this.lblRevisionInfo.Size = new System.Drawing.Size(160, 23);
			this.lblRevisionInfo.TabIndex = 11;
			this.lblRevisionInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			//lblBranchContact
			//
			this.lblBranchContact.BackColor = System.Drawing.Color.White;
			this.lblBranchContact.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblBranchContact.Location = new System.Drawing.Point(104, 199);
			this.lblBranchContact.Name = "lblBranchContact";
			this.lblBranchContact.Size = new System.Drawing.Size(256, 23);
			this.lblBranchContact.TabIndex = 10;
			this.lblBranchContact.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			//lblVersionNo
			//
			this.lblVersionNo.BackColor = System.Drawing.Color.White;
			this.lblVersionNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblVersionNo.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblVersionNo.Location = new System.Drawing.Point(104, 42);
			this.lblVersionNo.Name = "lblVersionNo";
			this.lblVersionNo.Size = new System.Drawing.Size(160, 23);
			this.lblVersionNo.TabIndex = 9;
			this.lblVersionNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			//Label5
			//
			this.Label5.Location = new System.Drawing.Point(16, 50);
			this.Label5.Name = "Label5";
			this.Label5.Size = new System.Drawing.Size(96, 16);
			this.Label5.TabIndex = 8;
			this.Label5.Text = "OneVue Version";
			//
			//Label4
			//
			this.Label4.Location = new System.Drawing.Point(16, 74);
			this.Label4.Name = "Label4";
			this.Label4.Size = new System.Drawing.Size(88, 16);
			this.Label4.TabIndex = 7;
			this.Label4.Text = "SVN Revision";
			//
			//lblRevision
			//
			this.lblRevision.BackColor = System.Drawing.Color.White;
			this.lblRevision.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblRevision.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblRevision.Location = new System.Drawing.Point(104, 69);
			this.lblRevision.Name = "lblRevision";
			this.lblRevision.Size = new System.Drawing.Size(80, 23);
			this.lblRevision.TabIndex = 6;
			this.lblRevision.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			//lblDisplayName
			//
			this.lblDisplayName.BackColor = System.Drawing.Color.White;
			this.lblDisplayName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblDisplayName.Location = new System.Drawing.Point(104, 96);
			this.lblDisplayName.Name = "lblDisplayName";
			this.lblDisplayName.Size = new System.Drawing.Size(192, 23);
			this.lblDisplayName.TabIndex = 5;
			this.lblDisplayName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			//lblBranchDescription
			//
			this.lblBranchDescription.AutoEllipsis = true;
			this.lblBranchDescription.BackColor = System.Drawing.Color.White;
			this.lblBranchDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblBranchDescription.Location = new System.Drawing.Point(104, 123);
			this.lblBranchDescription.Name = "lblBranchDescription";
			this.lblBranchDescription.Size = new System.Drawing.Size(256, 72);
			this.lblBranchDescription.TabIndex = 4;
			//
			//Label3
			//
			this.Label3.Location = new System.Drawing.Point(16, 202);
			this.Label3.Name = "Label3";
			this.Label3.Size = new System.Drawing.Size(80, 16);
			this.Label3.TabIndex = 2;
			this.Label3.Text = "Main Contact";
			//
			//Label2
			//
			this.Label2.Location = new System.Drawing.Point(16, 122);
			this.Label2.Name = "Label2";
			this.Label2.Size = new System.Drawing.Size(72, 16);
			this.Label2.TabIndex = 1;
			this.Label2.Text = "Description";
			//
			//Label1
			//
			this.Label1.Location = new System.Drawing.Point(16, 98);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(80, 16);
			this.Label1.TabIndex = 0;
			this.Label1.Text = "Branch Name";
			//
			//btnCancel
			//
			this.btnCancel.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnCancel.BackColor = System.Drawing.Color.White;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCancel.Image = (System.Drawing.Image)resources.GetObject("btnCancel.Image");
			this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnCancel.Location = new System.Drawing.Point(182, 368);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(96, 23);
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "Close";
			this.btnCancel.UseVisualStyleBackColor = false;
			//
			//ErrorProvider1
			//
			this.ErrorProvider1.ContainerControl = this;
			this.ErrorProvider1.DataMember = "";
			//
			//radTraining
			//
			this.radTraining.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(224)), Convert.ToInt32(Convert.ToByte(224)), Convert.ToInt32(Convert.ToByte(224)));
			this.radTraining.Location = new System.Drawing.Point(4, 3);
			this.radTraining.Name = "radTraining";
			this.radTraining.Size = new System.Drawing.Size(72, 23);
			this.radTraining.TabIndex = 7;
			this.radTraining.Text = "Training";
			this.radTraining.UseVisualStyleBackColor = false;
			//
			//lblStatus
			//
			this.lblStatus.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblStatus.Location = new System.Drawing.Point(14, 368);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(160, 23);
			this.lblStatus.TabIndex = 8;
			//
			//pnlTraining
			//
			this.pnlTraining.Controls.Add(this.cboTraining);
			this.pnlTraining.Controls.Add(this.radTraining);
			this.pnlTraining.Location = new System.Drawing.Point(4, 79);
			this.pnlTraining.Name = "pnlTraining";
			this.pnlTraining.Size = new System.Drawing.Size(366, 28);
			this.pnlTraining.TabIndex = 12;
			//
			//frmLauncher
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(224)), Convert.ToInt32(Convert.ToByte(224)), Convert.ToInt32(Convert.ToByte(224)));
			this.ClientSize = new System.Drawing.Size(394, 404);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.grpDetails);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.cboBranches);
			this.Controls.Add(this.radBranches);
			this.Controls.Add(this.radProduction);
			this.Controls.Add(this.grpBranch);
			this.Controls.Add(this.btnCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			this.MaximizeBox = false;
			this.Name = "frmLauncher";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "OneVue Launcher v3.0";
			this.grpBranch.ResumeLayout(false);
			this.grpBranch.PerformLayout();
			this.grpDetails.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.ErrorProvider1).EndInit();
			this.pnlTraining.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		#region "Private Variables"
		private const string APPLICATION_TITLE = "OneVue Launcher";
		private int _currentBranchIndex;
		private string _currentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
		private System.Collections.Specialized.OrderedDictionary _branches = new System.Collections.Specialized.OrderedDictionary();
		private string _working_dir_path = "c:\\svn_testing";
		private string _repository_uri = "svn://launcher.fas.au.challenger.net/fas/onevuereleases2009/";
		private const string PROD_BRANCHNAME = "production";
		private const string PROD_EXENAME = "OneVue.exe";

		private static Random rnd = new Random();
		#endregion

		#region " Private Methods"

		/// <summary>
		/// Load the XML Data
		/// </summary>
		private void LoadData(string strFileName)
		{
			try {
				//Remove all items
				_branches.Clear();
				XmlDocument xmlDoc = new XmlDocument();
				XmlNodeList branchNodeList = null;
				XmlNode branchNode = null;
				xmlDoc.Load(Path.Combine(_currentDirectory, strFileName));
				branchNodeList = xmlDoc.GetElementsByTagName("Branch");
				foreach (XmlNode branchNode_loopVariable in branchNodeList) {
					branchNode = branchNode_loopVariable;
					XmlNodeList baseDataNodes = null;
					XmlNode baseData = null;
					Branch newBranch = new Branch();
					newBranch.BranchExeName = "OneVue.exe";
					baseDataNodes = branchNode.ChildNodes;
					foreach (XmlNode baseData_loopVariable in baseDataNodes) {
						baseData = baseData_loopVariable;
						switch (baseData.Name) {
							case "BranchName":
								newBranch.BranchName = baseData.InnerText;
								break;
							case "BranchVersion":
								newBranch.BranchVersion = baseData.InnerText;
								break;
							case "BranchRevision":
								newBranch.BranchRevision = baseData.InnerText;
								break;
							case "BranchDescription":
								newBranch.BranchDescription = baseData.InnerText;
								break;
							case "BranchDisplayName":
								newBranch.BranchDisplayName = baseData.InnerText;
								break;
							case "BranchContact":
								newBranch.BranchContact = baseData.InnerText;
								break;
							case "BranchExeName":
								newBranch.BranchExeName = baseData.InnerText;
								break;
							case "BranchWorkingDirectory":
								newBranch.BranchWorkingDirectory = baseData.InnerText;
								break;
						}
					}
					if (!string.IsNullOrEmpty(newBranch.BranchName)) {
						_branches.Add(newBranch.BranchName, newBranch);
					} else {
						// This must be a separator that won't need to be retrieved, so just use a random key..
						_branches.Add(GetRandomString(10), newBranch);
					}
				}
			} catch (System.Exception ex) {
				Interaction.MsgBox(ex.ToString());
			}

		}

		/// <summary>
		/// Generates a random string of alpha-numeric characters of the specified length.
		/// </summary>
		/// <param name="length">Length of the returned string</param>
		/// <returns>A random string of alpha-numeric characters</returns>
		/// <remarks></remarks>
		private string GetRandomString(int length)
		{
			string legalCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
			System.Text.StringBuilder str = new System.Text.StringBuilder();
			char ch = '\0';
			for (int i = 0; i <= length - 1; i++) {
				ch = legalCharacters[rnd.Next(0, legalCharacters.Length)];
				str.Append(ch);
			}
			return str.ToString();
		}

		/// <summary>
		/// Load the combo that displays the list of Branches
		/// </summary>
		private void LoadBranchCombo()
		{
			cboBranches.Items.Clear();
			if (_branches.Count > 0) {
				Branch Branch = null;
				foreach (Branch Branch_loopVariable in _branches.Values) {
					Branch = Branch_loopVariable;
					cboBranches.Items.Add(Branch.BranchDisplayName);
				}
			}
		}

		/// <summary>
		/// Load the combo that displays the list of Branches
		/// </summary>
		private void LoadTrainingCombo()
		{
			cboTraining.Items.Clear();
			if (_branches.Count > 0) {
				Branch Branch = null;
				foreach (Branch Branch_loopVariable in _branches.Values) {
					Branch = Branch_loopVariable;
					cboTraining.Items.Add(Branch.BranchDisplayName);
				}
			}
		}

		/// <summary>
		/// If Production radio button activated 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void radProduction_CheckedChanged(System.Object sender, System.EventArgs e)
		{
			if (radProduction.Checked) {
				radBranches.Checked = false;
				radTraining.Checked = false;
				cboBranches.Enabled = false;
				cboBranches.SelectedIndex = -1;

				cboTraining.Enabled = false;
				cboTraining.SelectedIndex = -1;

				lblStatus.Text = "";
				lblDisplayName.Text = "";
				lblVersionNo.Text = "";
				lblRevision.Text = "";
				lblBranchDescription.Text = "";
				lblBranchContact.Text = "";
				btnOK.Enabled = true;
			} else {
				btnOK.Enabled = false;
			}
			update_revision_info();
		}

		/// <summary>
		/// Form Load
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmLauncher_Load(object sender, System.EventArgs e)
		{
			string[] args = Environment.GetCommandLineArgs();
			bool IsIncludeProduction = true;
			bool IsIncludeTraining = true;

			if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings.Get("IncludeProduction"))) {
				IsIncludeProduction = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings.Get("IncludeProduction"));
			}

			if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings.Get("IncludeTraining"))) {
				IsIncludeTraining = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings.Get("IncludeTraining"));
			}

			//
			// See if an alternate working directory has been set in the config file..
			//
			if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings.Get("WorkingDirectoryPath"))) {
				_working_dir_path = System.Configuration.ConfigurationManager.AppSettings.Get("WorkingDirectoryPath");
			}

			//
			// See if an alternate working directory path has been assigned
			// via the command line. (note that the executable name is always arg(0))
			// A working directory assigned via the command line overrides others.
			//
			for (Int16 i = 1; i <= args.Length - 1; i++) {
				switch (args[i].Trim().ToUpper()) {
					case "-WORK_DIR":
						if (args.Length > i + 1) {
							_working_dir_path = args[i + 1];
							// Skip the next arg as we've already grabbed it..
							i += 1;
						} else {
							ShowUsage();
						}
						break;
					default:
						ShowUsage();
						break;
				}
			}

			if (IsIncludeTraining) {
				this.pnlTraining.Visible = true;
				// Check that the Training config file is available first though..
				if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings.Get("TrainingXMLPath"))) {
					if ((!File.Exists(_currentDirectory + "\\Training.xml"))) {
						Interaction.MsgBox("File Training.xml was not found, OneVue Launcher cannot continue without it.");
						this.Close();
						return;
					}
				} else {
					if ((!File.Exists(System.Configuration.ConfigurationManager.AppSettings.Get("TrainingXMLPath")))) {
						Interaction.MsgBox("The Training data file (" + System.Configuration.ConfigurationManager.AppSettings.Get("TrainingXMLPath") + ") was not found, OneVue Launcher cannot continue without it.");
						this.Close();
						return;
					}
				}
			} else {
				this.pnlTraining.Visible = false;
			}

			if (IsIncludeProduction) {
				this.radProduction.Visible = true;
			} else {
				this.radProduction.Visible = false;
			}

			// Now check that the Branches config file is available and load it..
			if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings.Get("BranchesXMLPath"))) {
				if ((!File.Exists(_currentDirectory + "\\Branches.xml"))) {
					Interaction.MsgBox("File Branches.xml was not found, OneVue Launcher cannot continue without it.");
					this.Close();
					return;
				}
			} else {
				if ((!File.Exists(System.Configuration.ConfigurationManager.AppSettings.Get("BranchesXMLPath")))) {
					Interaction.MsgBox("The Branches data file (" + System.Configuration.ConfigurationManager.AppSettings.Get("BranchesXMLPath") + ") was not found, OneVue Launcher cannot continue without it.");
					this.Close();
					return;
				}
			}

			if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings.Get("RepositoryURI"))) {
				_repository_uri = System.Configuration.ConfigurationManager.AppSettings.Get("RepositoryURI");
			}

			// Default is to load Branches. Setting the radio button calls 
			// the OnChange handler which loads the file data..
			this.radBranches.Checked = true;
			lblStatusCurrent.Visible = false;
			lblStatusUpdate.Visible = false;
			LoadBranchCombo();
			this.Text = GetVersionNo();

			if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings.Get("BackColour"))) {
				System.Drawing.Color custom_colour = default(System.Drawing.Color);
				custom_colour = (Color)System.ComponentModel.TypeDescriptor.GetConverter(typeof(Color)).ConvertFromString(System.Configuration.ConfigurationManager.AppSettings.Get("BackColour"));
				this.BackColor = custom_colour;
				pnlTraining.BackColor = custom_colour;
				grpBranch.BackColor = custom_colour;
				grpDetails.BackColor = custom_colour;
				radBranches.BackColor = custom_colour;
				radProduction.BackColor = custom_colour;
				radTraining.BackColor = custom_colour;
			}

		}

		/// <summary>
		/// Displays program usage and quits.
		/// </summary>
		/// <remarks></remarks>
		private void ShowUsage()
		{
			ShowUsage("", true);
		}

		/// <summary>
		/// Displays program usage and quits if required.
		/// </summary>
		/// <param name="Message">An optional message to display.</param>
		/// <param name="CloseForm">Closes the form when True</param>
		/// <remarks></remarks>
		private void ShowUsage(string Message, bool CloseForm)
		{
			string filename = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
			filename = System.IO.Path.GetFileNameWithoutExtension(filename);
			MessageBox.Show(filename + " [-WORK_DIR <path to svn_testing dir>] " + Constants.vbCrLf + Message, "Usage", MessageBoxButtons.OK, MessageBoxIcon.Information);
			if (CloseForm) {
				this.Close();
			}
		}

		/// <summary>
		/// Get the Version No
		/// </summary>
		/// <returns></returns>
		private string GetVersionNo()
		{
			string NameAndVersionNo = null;
			string VersionMajor = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major.ToString();
			string VersionMinor = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString();

			if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings.Get("ApplicationTitle"))) {
				NameAndVersionNo = string.Format("{0} {1}.{2}", System.Configuration.ConfigurationManager.AppSettings.Get("ApplicationTitle"), VersionMajor, VersionMinor);
			} else {
				NameAndVersionNo = string.Format("{0} {1}.{2}", APPLICATION_TITLE, VersionMajor, VersionMinor);
			}

			return NameAndVersionNo;
		}

		/// <summary>
		/// Combo box index changed 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cboBranches_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int index = cboBranches.SelectedIndex;
			lblStatus.Text = "";
			if (!(index == -1)) {
				//
				// If warning, then clear warning as we have just selected a branch
				ErrorProvider1.SetError(cboBranches, "");
				radBranches.Checked = true;
				_currentBranchIndex = index;
				Branch theSelectedBranch = _branches[index] as Branch;
				lblDisplayName.Text = theSelectedBranch.BranchName;
				lblVersionNo.Text = theSelectedBranch.BranchVersion;
				lblRevision.Text = theSelectedBranch.BranchRevision;
				lblBranchDescription.Text = theSelectedBranch.BranchDescription;
				lblBranchContact.Text = theSelectedBranch.BranchContact;
				if (theSelectedBranch.BranchName == null) {
					btnOK.Enabled = false;
				} else {
					btnOK.Enabled = true;
				}
			} else {
				btnOK.Enabled = false;
			}
			update_revision_info();
		}

		/// <summary>
		/// Combo box index changed 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cboTraining_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int index = cboTraining.SelectedIndex;
			lblStatus.Text = "";
			if (!(index == -1)) {
				//
				// If warning, then clear warning as we have just selected a branch
				ErrorProvider1.SetError(cboTraining, "");
				radTraining.Checked = true;
				_currentBranchIndex = index;
                Branch theSelectedBranch = _branches[index] as Branch;
				lblDisplayName.Text = theSelectedBranch.BranchName;
				lblVersionNo.Text = theSelectedBranch.BranchVersion;
				lblRevision.Text = theSelectedBranch.BranchRevision;
				lblBranchDescription.Text = theSelectedBranch.BranchDescription;
				lblBranchContact.Text = theSelectedBranch.BranchContact;
				if (theSelectedBranch.BranchName == null) {
					btnOK.Enabled = false;
				} else {
					btnOK.Enabled = true;
				}
			} else {
				btnOK.Enabled = false;
			}
			update_revision_info();
		}

		/// <summary>
		/// Call the batch file to update
		/// </summary>
		private void UpdateBranch()
		{
			string branch_path = null;
			string branch_working_directory = null;

			try {
				lblStatus.Text = "Updating branch";
				Application.DoEvents();

				frmCheckout frmCheckout = new frmCheckout();
				frmCheckout.RepositoryURI = _repository_uri;
				frmCheckout.StartPosition = FormStartPosition.CenterParent;
				//
				// Set the branch name depending on the selected form option..
				//
				if (radBranches.Checked | radTraining.Checked) {
					frmCheckout.Branch = lblDisplayName.Text;
					frmCheckout.Revision = lblRevision.Text;
					//
					// Set the branch working directory to either the default or a specifically configured one..
					//
					branch_working_directory = ((Branch)_branches[lblDisplayName.Text]).BranchWorkingDirectory;
					if (string.IsNullOrEmpty(branch_working_directory)) {
						branch_working_directory = _working_dir_path;
					}
					frmCheckout.WorkingDir = branch_working_directory;
					branch_path = System.IO.Path.Combine(branch_working_directory, lblDisplayName.Text);
				} else {
					branch_path = _working_dir_path;
					frmCheckout.Branch = PROD_BRANCHNAME;
					frmCheckout.Revision = "HEAD";
					frmCheckout.WorkingDir = branch_path;
				}

				bool is_ok_to_launch = false;
				System.Windows.Forms.DialogResult checkout_result = System.Windows.Forms.DialogResult.None;
				System.Windows.Forms.DialogResult dialog_result = default(System.Windows.Forms.DialogResult);

				checkout_result = frmCheckout.ShowDialog(this);

				if (checkout_result == System.Windows.Forms.DialogResult.OK) {
					lblStatus.Text = "rev. " + frmCheckout.CheckedOutRevision.ToString();
					Application.DoEvents();
					if (validate_branch_revision(frmCheckout.CheckedOutRevision)) {
						is_ok_to_launch = true;
					} else {
						is_ok_to_launch = false;
						MessageBox.Show("SVN revision validation failed." + Constants.vbCrLf + "Requested r" + lblRevision.Text + ", but retrieved r" + frmCheckout.CheckedOutRevision.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}

				} else {
					is_ok_to_launch = false;
					if (checkout_result != System.Windows.Forms.DialogResult.Cancel) {
						if (!string.IsNullOrEmpty(frmCheckout.ErrorMessage)) {
							MessageBox.Show("SVN checkout failed." + Constants.vbCrLf + frmCheckout.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						} else {
							long rev = -1;
							if (long.TryParse(lblRevision.Text, out rev)) {
								if (frmCheckout.CheckedOutRevision.ToString() != lblRevision.Text) {
									MessageBox.Show("SVN revision validation failed." + Constants.vbCrLf + "Requested r" + lblRevision.Text + ", but retrieved r" + frmCheckout.CheckedOutRevision, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
								}
							} else {
								MessageBox.Show("SVN revision validation failed." + Constants.vbCrLf, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							}

						}
					}
				}

				set_working_copy_current_status(is_ok_to_launch);

				//
				// Prompt user if branch retrieval fails..
				//
				if (!is_ok_to_launch) {
					if (checkout_result == System.Windows.Forms.DialogResult.Cancel) {
						lblStatus.Text = "Update cancelled.";
						Application.DoEvents();
					} else {
						lblStatus.Text = "Update failed.";
						Application.DoEvents();
						if (System.IO.Directory.Exists(branch_path)) {
							dialog_result = MessageBox.Show("Launcher failed to retrieve the specified SVN revision." + Constants.vbCrLf + "Do you wish to run the previous version?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
							if (dialog_result == System.Windows.Forms.DialogResult.Yes) {
								is_ok_to_launch = true;
							}
						} else {
							MessageBox.Show("Launcher failed to retrieve the specified SVN revision.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
							is_ok_to_launch = false;
						}
					}
				}

				//
				// Launch retrieved executable..
				//
				if (is_ok_to_launch) {
					ExecuteBranch();
				}

			} catch (System.Exception ex) {
				lblStatus.Text = "Error";
				Interaction.MsgBox(ex.ToString());
			}

		}

		/// <summary>
		/// Launches the currently selected branch executable.
		/// </summary>
		/// <remarks></remarks>
		private void ExecuteBranch()
		{
			string working_copy_path = get_current_working_copy_path();

			string executable_path = null;
			if (radBranches.Checked | radTraining.Checked) {
				executable_path = System.IO.Path.Combine(working_copy_path, ((Branch)_branches[_currentBranchIndex]).BranchExeName);
			} else {
				executable_path = System.IO.Path.Combine(working_copy_path, PROD_EXENAME);
			}
			if (!System.IO.File.Exists(executable_path)) {
				throw new ApplicationException("File not found: \"" + executable_path + "\"");
			}
			Interaction.Shell("\"" + executable_path + "\"", AppWinStyle.NormalFocus, false);
		}

		/// <summary>
		/// If a revision number is specified for the branch, check if it 
		/// is the same as the supplied revision number.
		/// </summary>
		/// <param name="revision">The retrieved revision number</param>
		/// <returns>
		/// Boolean True if the supplied revision is the same as 
		/// that specified on the form, False otherwise.
		/// </returns>
		/// <remarks></remarks>
		private bool validate_branch_revision(long? revision)
		{
			bool result = true;
            long requested_revision = 0;
			//
			// We only want to validate if an actual revision number has been 
			// specified for the branch rather than "HEAD", "BASE", etc.
			//
            if (long.TryParse(lblRevision.Text, out requested_revision))
            {
				result = revision == requested_revision;
			}
			return result;
		}

		/// <summary>
		/// Deletes the current selected branch working directory.
		/// </summary>
		/// <remarks></remarks>
		private bool DeleteBranch()
		{
			bool result = false;
			string branch_path = string.Empty;
			string branch_working_directory = string.Empty;
			try {
				lblStatus.Text = "Deleting branch";
				Application.DoEvents();
				branch_path = get_current_working_copy_path();
				if (System.IO.Directory.Exists(branch_path)) {
					DeleteDirectory(branch_path);
				}
				result = true;
			} catch (System.Exception ex) {
				lblStatus.Text = "Error";
				Interaction.MsgBox(ex.ToString());
			}
			return result;
		}

		/// <summary>
		/// Gets the working directory of the currently selected branch.
		/// </summary>
		/// <returns></returns>
		/// <remarks></remarks>
		private string get_current_working_copy_path()
		{
			string branch_path = string.Empty;
			string branch_working_directory = string.Empty;
			if (radBranches.Checked | radTraining.Checked) {
				if (!string.IsNullOrEmpty(lblDisplayName.Text)) {
					branch_working_directory = ((Branch)_branches[lblDisplayName.Text]).BranchWorkingDirectory;
					if (string.IsNullOrEmpty(branch_working_directory)) {
						branch_working_directory = _working_dir_path;
					}
					branch_path = System.IO.Path.Combine(branch_working_directory, lblDisplayName.Text);
				}
			} else {
				branch_working_directory = _working_dir_path;
				branch_path = System.IO.Path.Combine(branch_working_directory, PROD_BRANCHNAME);
			}
			return branch_path;
		}

		/// <summary>
		/// Deletes a directory tree ensuring any read-only files are also deleted
		/// </summary>
		/// <param name="target_dir"></param>
		/// <returns></returns>
		/// <remarks></remarks>
		public static bool DeleteDirectory(string target_dir)
		{
			bool result = false;
			string[] files = Directory.GetFiles(target_dir);
			string[] dirs = Directory.GetDirectories(target_dir);

			foreach (string file_name in files) {
				File.SetAttributes(file_name, FileAttributes.Normal);
				File.Delete(file_name);
			}

			foreach (string dir in dirs) {
				DeleteDirectory(dir);
			}

			Directory.Delete(target_dir, true);

			return result;
		}

		/// <summary>
		/// OK button clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnOK_Click(System.Object sender, System.EventArgs e)
		{
			bool success = true;
			string branch_working_directory = null;
			ErrorProvider1.SetError(cboBranches, "");
			ErrorProvider1.SetError(cboTraining, "");
			try {
				this.Cursor = Cursors.WaitCursor;
				if (lblStatusCurrent.Visible & !chkForceRefresh.Checked) {
					ExecuteBranch();
				} else {
					//
					// Ensure the specified working directory path is valid..
					//
					branch_working_directory = get_current_working_copy_path();
					if (!System.IO.Directory.Exists(branch_working_directory)) {
						System.IO.DirectoryInfo inf = null;
						inf = System.IO.Directory.CreateDirectory(branch_working_directory);
					}
					if (chkForceRefresh.Checked) {
						success = DeleteBranch();
					}
					if (success) {
						UpdateBranch();
					}
				}
			} catch (System.Exception ex) {
				MessageBox.Show(ex.Format(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				if (lblStatusCurrent.Visible) {
					set_working_copy_current_status(false);
				}
				return;
			} finally {
				this.Cursor = Cursors.Default;
			}
		}

		/// <summary>
		/// Close the App
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnCancel_Click(System.Object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void radTraining_CheckedChanged(System.Object sender, System.EventArgs e)
		{
			if (radTraining.Checked) {
				radProduction.Checked = false;
				radBranches.Checked = false;
				this.Cursor = Cursors.WaitCursor;
				cboBranches.Enabled = false;
				cboBranches.SelectedIndex = -1;

				lblDisplayName.Text = "";
				lblVersionNo.Text = "";
				lblRevision.Text = "";
				lblBranchDescription.Text = "";
				lblBranchContact.Text = "";

				cboTraining.Enabled = true;
				if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings.Get("TrainingXMLPath"))) {
					LoadData(_currentDirectory + "\\Training.xml");
				} else {
					LoadData(System.Configuration.ConfigurationManager.AppSettings.Get("TrainingXMLPath"));
				}

				LoadTrainingCombo();
				this.Cursor = Cursors.Default;
				update_revision_info();
			}
		}

		private void radBranches_CheckedChanged(System.Object sender, System.EventArgs e)
		{
			if (radBranches.Checked) {
				radProduction.Checked = false;
				radTraining.Checked = false;
				this.Cursor = Cursors.WaitCursor;
				cboTraining.Enabled = false;
				cboTraining.SelectedIndex = -1;

				lblStatus.Text = string.Empty;

				lblDisplayName.Text = "";
				lblVersionNo.Text = "";
				lblRevision.Text = "";
				lblBranchDescription.Text = "";
				lblBranchContact.Text = "";

				cboBranches.Enabled = true;

				//LoadData("Branches.xml")
				if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings.Get("BranchesXMLPath"))) {
					LoadData(_currentDirectory + "\\Branches.xml");
				} else {
					LoadData(System.Configuration.ConfigurationManager.AppSettings.Get("BranchesXMLPath"));
				}

				LoadBranchCombo();
				this.Cursor = Cursors.Default;
				update_revision_info();
			}
		}

		/// <summary>
		/// chkForceRefresh CheckedChanged handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks></remarks>
		private void chkForceRefresh_CheckedChanged(System.Object sender, System.EventArgs e)
		{
			lblStatus.Text = "";
		}

		/// <summary>
		/// Gets the current selected branch info.
		/// </summary>
		/// <remarks></remarks>
		private void update_revision_info()
		{
			string branch = null;
			string revision = null;
			Nullable<DateTime> last_changed_datetime = default(Nullable<DateTime>);
			string last_changed_author = string.Empty;
			Nullable<long> last_changed_revision = default(Nullable<long>);

			try {
				this.Cursor = Cursors.WaitCursor;
				this.lblRevisionInfo.Text = "";
				lblStatusCurrent.Visible = false;
				lblStatusUpdate.Visible = false;
				if (!string.IsNullOrEmpty(lblDisplayName.Text) | radProduction.Checked) {
					//
					// Set the branch name depending on the selected form option..
					//
					if (radBranches.Checked | radTraining.Checked) {
						branch = lblDisplayName.Text;
						revision = lblRevision.Text;
					} else {
						branch = PROD_BRANCHNAME;
						revision = "HEAD";
					}

					libSVN.SVN.GetRevisionInfo(System.IO.Path.Combine(_repository_uri, branch), revision, out last_changed_datetime, out last_changed_author, out last_changed_revision);
					if (last_changed_datetime.HasValue) {
						this.lblRevisionInfo.Text = (last_changed_datetime.HasValue ? last_changed_datetime.ToString() : "").ToString();
					}
					if (last_changed_revision.HasValue) {
						string working_copy_path = get_current_working_copy_path();
						set_working_copy_current_status(is_working_copy_version(working_copy_path, last_changed_revision.Value));
					}
				}
			} catch (System.Exception ex) {
				MessageBox.Show(ex.Format(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			} finally {
				this.Cursor = Cursors.Default;
			}
		}

		/// <summary>
		/// Updates the Working Copy Status labels.
		/// </summary>
		/// <param name="IsCurrent"></param>
		/// <remarks></remarks>
		private void set_working_copy_current_status(bool IsCurrent)
		{
			lblStatusUpdate.Visible = !IsCurrent;
			lblStatusCurrent.Visible = IsCurrent;
		}

		/// <summary>
		/// Verifies that the supplied SVN working copy path is at the same revision as the supplied value.
		/// </summary>
		/// <param name="WorkingCopyPath"></param>
		/// <param name="Revision"></param>
		/// <returns></returns>
		/// <remarks></remarks>
		private bool is_working_copy_version(string WorkingCopyPath, long Revision)
		{
			bool is_current_version = false;
			if (!string.IsNullOrEmpty(WorkingCopyPath)) {
				if (libSVN.SVN.IsWorkingCopyVersioned(WorkingCopyPath)) {
					if (libSVN.SVN.GetWorkingCopyRevision(WorkingCopyPath) == Revision) {
						is_current_version = true;
					}
				}
			}
			return is_current_version;
		}

		#endregion

	}
}
