using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace OneVueLauncher
{
    partial class frmLauncher : System.Windows.Forms.Form
    {
        //Form overrides dispose to clean up the component list.
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if ((components != null))
                {
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
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.radProduction = new System.Windows.Forms.RadioButton();
            this.radBranches = new System.Windows.Forms.RadioButton();
            this.cboBranches = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.grpBranch = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cboGitTesting = new System.Windows.Forms.ComboBox();
            this.radGitTesting = new System.Windows.Forms.RadioButton();
            this.pnlTraining = new System.Windows.Forms.Panel();
            this.cboTraining = new System.Windows.Forms.ComboBox();
            this.radTraining = new System.Windows.Forms.RadioButton();
            this.chkForceRefresh = new System.Windows.Forms.CheckBox();
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
            this.lblStatus = new System.Windows.Forms.Label();
            this.grpBranch.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlTraining.SuspendLayout();
            this.grpDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // radProduction
            // 
            this.radProduction.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.radProduction.Location = new System.Drawing.Point(16, 32);
            this.radProduction.Name = "radProduction";
            this.radProduction.Size = new System.Drawing.Size(88, 16);
            this.radProduction.TabIndex = 0;
            this.radProduction.Text = "Production";
            this.radProduction.UseVisualStyleBackColor = false;
            this.radProduction.CheckedChanged += new System.EventHandler(this.radProduction_CheckedChanged);
            // 
            // radBranches
            // 
            this.radBranches.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.radBranches.Location = new System.Drawing.Point(16, 64);
            this.radBranches.Name = "radBranches";
            this.radBranches.Size = new System.Drawing.Size(72, 21);
            this.radBranches.TabIndex = 1;
            this.radBranches.Text = "Testing";
            this.radBranches.UseVisualStyleBackColor = false;
            this.radBranches.Click += new System.EventHandler(this.radBranches_CheckedChanged);
            // 
            // cboBranches
            // 
            this.cboBranches.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBranches.Location = new System.Drawing.Point(112, 64);
            this.cboBranches.Name = "cboBranches";
            this.cboBranches.Size = new System.Drawing.Size(256, 21);
            this.cboBranches.TabIndex = 2;
            this.cboBranches.SelectedIndexChanged += new System.EventHandler(this.cboBranches_SelectedIndexChanged);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.BackColor = System.Drawing.Color.White;
            this.btnOK.Enabled = false;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(286, 442);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(96, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "Launch";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // grpBranch
            // 
            this.grpBranch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grpBranch.Controls.Add(this.panel1);
            this.grpBranch.Controls.Add(this.pnlTraining);
            this.grpBranch.Controls.Add(this.chkForceRefresh);
            this.grpBranch.Location = new System.Drawing.Point(8, 8);
            this.grpBranch.Name = "grpBranch";
            this.grpBranch.Size = new System.Drawing.Size(376, 148);
            this.grpBranch.TabIndex = 4;
            this.grpBranch.TabStop = false;
            this.grpBranch.Text = "OneVue";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cboGitTesting);
            this.panel1.Controls.Add(this.radGitTesting);
            this.panel1.Location = new System.Drawing.Point(4, 111);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(366, 28);
            this.panel1.TabIndex = 13;
            // 
            // cboGitTesting
            // 
            this.cboGitTesting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGitTesting.Enabled = false;
            this.cboGitTesting.Location = new System.Drawing.Point(100, 3);
            this.cboGitTesting.Name = "cboGitTesting";
            this.cboGitTesting.Size = new System.Drawing.Size(256, 21);
            this.cboGitTesting.TabIndex = 3;
            this.cboGitTesting.SelectedIndexChanged += new System.EventHandler(this.cboGitTesting_SelectedIndexChanged);
            // 
            // radGitTesting
            // 
            this.radGitTesting.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.radGitTesting.Location = new System.Drawing.Point(4, 3);
            this.radGitTesting.Name = "radGitTesting";
            this.radGitTesting.Size = new System.Drawing.Size(72, 23);
            this.radGitTesting.TabIndex = 7;
            this.radGitTesting.Text = "git testing";
            this.radGitTesting.UseVisualStyleBackColor = false;
            this.radGitTesting.CheckedChanged += new System.EventHandler(this.radGitTesting_CheckedChanged);
            // 
            // pnlTraining
            // 
            this.pnlTraining.Controls.Add(this.cboTraining);
            this.pnlTraining.Controls.Add(this.radTraining);
            this.pnlTraining.Location = new System.Drawing.Point(4, 79);
            this.pnlTraining.Name = "pnlTraining";
            this.pnlTraining.Size = new System.Drawing.Size(366, 28);
            this.pnlTraining.TabIndex = 12;
            // 
            // cboTraining
            // 
            this.cboTraining.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTraining.Enabled = false;
            this.cboTraining.Location = new System.Drawing.Point(100, 3);
            this.cboTraining.Name = "cboTraining";
            this.cboTraining.Size = new System.Drawing.Size(256, 21);
            this.cboTraining.TabIndex = 3;
            this.cboTraining.SelectedIndexChanged += new System.EventHandler(this.cboTraining_SelectedIndexChanged);
            // 
            // radTraining
            // 
            this.radTraining.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.radTraining.Location = new System.Drawing.Point(4, 3);
            this.radTraining.Name = "radTraining";
            this.radTraining.Size = new System.Drawing.Size(72, 23);
            this.radTraining.TabIndex = 7;
            this.radTraining.Text = "Training";
            this.radTraining.UseVisualStyleBackColor = false;
            this.radTraining.Click += new System.EventHandler(this.radTraining_CheckedChanged);
            // 
            // chkForceRefresh
            // 
            this.chkForceRefresh.AutoSize = true;
            this.chkForceRefresh.Location = new System.Drawing.Point(104, 25);
            this.chkForceRefresh.Name = "chkForceRefresh";
            this.chkForceRefresh.Size = new System.Drawing.Size(93, 17);
            this.chkForceRefresh.TabIndex = 11;
            this.chkForceRefresh.Text = "Force Refresh";
            this.chkForceRefresh.UseVisualStyleBackColor = true;
            this.chkForceRefresh.Click += new System.EventHandler(this.chkForceRefresh_CheckedChanged);
            // 
            // grpDetails
            // 
            this.grpDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
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
            this.grpDetails.Location = new System.Drawing.Point(6, 175);
            this.grpDetails.Name = "grpDetails";
            this.grpDetails.Size = new System.Drawing.Size(376, 230);
            this.grpDetails.TabIndex = 5;
            this.grpDetails.TabStop = false;
            this.grpDetails.Text = "Details";
            // 
            // lblStatusCurrent
            // 
            this.lblStatusCurrent.ForeColor = System.Drawing.Color.Green;
            this.lblStatusCurrent.Location = new System.Drawing.Point(270, 20);
            this.lblStatusCurrent.Name = "lblStatusCurrent";
            this.lblStatusCurrent.Size = new System.Drawing.Size(90, 16);
            this.lblStatusCurrent.TabIndex = 14;
            this.lblStatusCurrent.Text = "(Current)";
            // 
            // lblStatusUpdate
            // 
            this.lblStatusUpdate.ForeColor = System.Drawing.Color.Red;
            this.lblStatusUpdate.Location = new System.Drawing.Point(270, 20);
            this.lblStatusUpdate.Name = "lblStatusUpdate";
            this.lblStatusUpdate.Size = new System.Drawing.Size(100, 16);
            this.lblStatusUpdate.TabIndex = 13;
            this.lblStatusUpdate.Text = "(Update Required)";
            // 
            // Label6
            // 
            this.Label6.Location = new System.Drawing.Point(19, 20);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(79, 16);
            this.Label6.TabIndex = 12;
            this.Label6.Text = "Date Released";
            // 
            // lblRevisionInfo
            // 
            this.lblRevisionInfo.BackColor = System.Drawing.Color.White;
            this.lblRevisionInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRevisionInfo.Location = new System.Drawing.Point(104, 15);
            this.lblRevisionInfo.Name = "lblRevisionInfo";
            this.lblRevisionInfo.Size = new System.Drawing.Size(160, 23);
            this.lblRevisionInfo.TabIndex = 11;
            this.lblRevisionInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBranchContact
            // 
            this.lblBranchContact.BackColor = System.Drawing.Color.White;
            this.lblBranchContact.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBranchContact.Location = new System.Drawing.Point(104, 199);
            this.lblBranchContact.Name = "lblBranchContact";
            this.lblBranchContact.Size = new System.Drawing.Size(256, 23);
            this.lblBranchContact.TabIndex = 10;
            this.lblBranchContact.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVersionNo
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
            // Label5
            // 
            this.Label5.Location = new System.Drawing.Point(16, 50);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(96, 16);
            this.Label5.TabIndex = 8;
            this.Label5.Text = "OneVue Version";
            // 
            // Label4
            // 
            this.Label4.Location = new System.Drawing.Point(16, 74);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(88, 16);
            this.Label4.TabIndex = 7;
            this.Label4.Text = "SVN Revision";
            // 
            // lblRevision
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
            // lblDisplayName
            // 
            this.lblDisplayName.BackColor = System.Drawing.Color.White;
            this.lblDisplayName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDisplayName.Location = new System.Drawing.Point(104, 96);
            this.lblDisplayName.Name = "lblDisplayName";
            this.lblDisplayName.Size = new System.Drawing.Size(192, 23);
            this.lblDisplayName.TabIndex = 5;
            this.lblDisplayName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBranchDescription
            // 
            this.lblBranchDescription.AutoEllipsis = true;
            this.lblBranchDescription.BackColor = System.Drawing.Color.White;
            this.lblBranchDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBranchDescription.Location = new System.Drawing.Point(104, 123);
            this.lblBranchDescription.Name = "lblBranchDescription";
            this.lblBranchDescription.Size = new System.Drawing.Size(256, 72);
            this.lblBranchDescription.TabIndex = 4;
            // 
            // Label3
            // 
            this.Label3.Location = new System.Drawing.Point(16, 202);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(80, 16);
            this.Label3.TabIndex = 2;
            this.Label3.Text = "Main Contact";
            // 
            // Label2
            // 
            this.Label2.Location = new System.Drawing.Point(16, 122);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(72, 16);
            this.Label2.TabIndex = 1;
            this.Label2.Text = "Description";
            // 
            // Label1
            // 
            this.Label1.Location = new System.Drawing.Point(16, 98);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(80, 16);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "Branch Name";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.White;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(182, 442);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(96, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ErrorProvider1
            // 
            this.ErrorProvider1.ContainerControl = this;
            this.ErrorProvider1.DataMember = "";
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(14, 442);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(160, 23);
            this.lblStatus.TabIndex = 8;
            // 
            // frmLauncher
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(394, 478);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.grpDetails);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cboBranches);
            this.Controls.Add(this.radBranches);
            this.Controls.Add(this.radProduction);
            this.Controls.Add(this.grpBranch);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmLauncher";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OneVue Launcher v3.0";
            this.Load += new System.EventHandler(this.frmLauncher_Load);
            this.grpBranch.ResumeLayout(false);
            this.grpBranch.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.pnlTraining.ResumeLayout(false);
            this.grpDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        internal System.Windows.Forms.RadioButton radProduction;
        internal System.Windows.Forms.RadioButton radBranches; internal System.Windows.Forms.GroupBox grpBranch;
        internal System.Windows.Forms.GroupBox grpDetails;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.ComboBox cboBranches;
        internal System.Windows.Forms.Label lblDisplayName;
        internal System.Windows.Forms.Label lblBranchDescription;
        internal System.Windows.Forms.Button btnOK;
        internal System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.ErrorProvider ErrorProvider1;
        internal System.Windows.Forms.Label lblRevision;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label lblBranchContact;
        internal System.Windows.Forms.Label lblVersionNo;
        internal System.Windows.Forms.RadioButton radTraining;
        internal System.Windows.Forms.ComboBox cboTraining;
        internal System.Windows.Forms.CheckBox chkForceRefresh;
        internal System.Windows.Forms.Label lblRevisionInfo;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Label lblStatusCurrent;
        internal System.Windows.Forms.Label lblStatusUpdate;
        internal System.Windows.Forms.Panel pnlTraining;
        internal System.Windows.Forms.Label lblStatus;
        internal Panel panel1;
        internal ComboBox cboGitTesting;
        internal RadioButton radGitTesting;
    }
}
