using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace OneVueLauncher
{
	[Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
	partial class frmCheckout : System.Windows.Forms.Form
	{

		//Form overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]
		protected override void Dispose(bool disposing)
		{
			try {
				if (disposing && components != null) {
					components.Dispose();
				}
			} finally {
				base.Dispose(disposing);
			}
		}


		//Required by the Windows Form Designer

		private System.ComponentModel.IContainer components;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCheckout));
			this.StatusStrip1 = new System.Windows.Forms.StatusStrip();
			this.btnCancel = new System.Windows.Forms.Button();
			this.txtCheckoutDetail = new System.Windows.Forms.TextBox();
			this.ProgressBar1 = new System.Windows.Forms.ProgressBar();
			this.SuspendLayout();
			//
			//StatusStrip1
			//
			this.StatusStrip1.AutoSize = false;
			this.StatusStrip1.Location = new System.Drawing.Point(0, 258);
			this.StatusStrip1.Name = "StatusStrip1";
			this.StatusStrip1.Size = new System.Drawing.Size(432, 41);
			this.StatusStrip1.TabIndex = 0;
			this.StatusStrip1.Text = "StatusStrip1";
			//
			//btnCancel
			//
			this.btnCancel.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnCancel.BackColor = System.Drawing.Color.White;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCancel.Image = (System.Drawing.Image)resources.GetObject("btnCancel.Image");
			this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnCancel.Location = new System.Drawing.Point(318, 268);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(96, 23);
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			//
			//txtCheckoutDetail
			//
			this.txtCheckoutDetail.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.txtCheckoutDetail.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtCheckoutDetail.Location = new System.Drawing.Point(0, 0);
			this.txtCheckoutDetail.Multiline = true;
			this.txtCheckoutDetail.Name = "txtCheckoutDetail";
			this.txtCheckoutDetail.ReadOnly = true;
			this.txtCheckoutDetail.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtCheckoutDetail.Size = new System.Drawing.Size(432, 258);
			this.txtCheckoutDetail.TabIndex = 8;
			//
			//ProgressBar1
			//
			this.ProgressBar1.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.ProgressBar1.Location = new System.Drawing.Point(13, 271);
			this.ProgressBar1.Name = "ProgressBar1";
			this.ProgressBar1.Size = new System.Drawing.Size(289, 18);
			this.ProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.ProgressBar1.TabIndex = 9;
			//
			//frmCheckout
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(432, 299);
			this.Controls.Add(this.ProgressBar1);
			this.Controls.Add(this.txtCheckoutDetail);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.StatusStrip1);
			this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmCheckout";
			this.Text = "Checkout Progress";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
        internal System.Windows.Forms.StatusStrip StatusStrip1;
        internal System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.TextBox txtCheckoutDetail;
        internal System.Windows.Forms.ProgressBar ProgressBar1;
	}
}
