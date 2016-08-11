using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using libSVN;
using System.IO;
namespace OneVueLauncher
{
	/// <summary>
	/// frmCheckout - processes SVN checkouts and displays progress.
	/// </summary>
	/// <remarks>
	/// <code region="History" lang="other" title="History">
	/// Date          Developer       Description
	/// -----------   --------------- ------------------------------------------------------------------
	/// 24-Jul-2013   CMadden         Original
	/// 10-Dec-2013   CMadden         Fixed bug calculating the number of changed files for progress in 
	///                               bgrLongProcess_DoWork().
	/// 14-Jul-2014   CMadden         Added merge conflict resolution for Config.ini files.
	/// </code>
	/// </remarks>
	public partial class frmCheckout
	{

		private System.ComponentModel.BackgroundWorker withEventsField_bgrLongProcess;
		internal System.ComponentModel.BackgroundWorker bgrLongProcess {
			get { return withEventsField_bgrLongProcess; }
			set {
				if (withEventsField_bgrLongProcess != null) {
					withEventsField_bgrLongProcess.ProgressChanged -= bgrLongProcess_ProgressChanged;
					withEventsField_bgrLongProcess.RunWorkerCompleted -= bgrLongProcess_RunWorkerCompleted;
					withEventsField_bgrLongProcess.DoWork -= bgrLongProcess_DoWork;
				}
				withEventsField_bgrLongProcess = value;
				if (withEventsField_bgrLongProcess != null) {
					withEventsField_bgrLongProcess.ProgressChanged += bgrLongProcess_ProgressChanged;
					withEventsField_bgrLongProcess.RunWorkerCompleted += bgrLongProcess_RunWorkerCompleted;
					withEventsField_bgrLongProcess.DoWork += bgrLongProcess_DoWork;
				}
			}
		}
		private int total_file_count = 0;
		private int processed_count = 0;
		// This delegate enables asynchronous calls for setting
		// the text property on a txtCheckoutDetail control.
		public delegate void SetTextCallback(ref TextBox txtControl, string text);

		private string _repository_uri;
		/// <summary>
		/// The branch repository URI
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public string RepositoryURI {
			get { return _repository_uri; }
			set { _repository_uri = value; }
		}

		private string _branch;
		/// <summary>
		/// The required branch name
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public string Branch {
			get { return _branch; }
			set { _branch = value; }
		}

		private string _revision;
		/// <summary>
		/// The required revision number or name
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public string Revision {
			get { return _revision; }
			set { _revision = value; }
		}

		private string _working_dir;
		/// <summary>
		/// The checkout working directory path
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public string WorkingDir {
			get { return _working_dir; }
			set { _working_dir = value; }
		}

		private long? _checked_out_revision;
		/// <summary>
		/// The retrieved branch revision number
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public long? CheckedOutRevision {
			get { return _checked_out_revision; }
		}

		private string _error_message;
		/// <summary>
		/// An error message set to indicate an issue with processing
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public string ErrorMessage {
			get { return _error_message; }
		}

		#region " Background worker "
		/// <summary>
		/// Displays the progress.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks></remarks>
		private void bgrLongProcess_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
		{
			ProgressBar1.Value = e.ProgressPercentage;
		}

		/// <summary>
		/// Background worker process RunWorkerCompleted handler.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks></remarks>
		private void bgrLongProcess_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
		{
			btnCancel.Enabled = false;
		}

		/// <summary>
		/// The SVN Notify callback handler. Updates progress details.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks></remarks>
		private void svn_client_Notify(object sender, SharpSvn.SvnNotifyEventArgs e)
		{
			string action = string.Empty;
			if (bgrLongProcess.CancellationPending) {
				string msg = "Cancelling.";
				update_txtCheckoutDetail(msg);
			}
			if (e != null) {
				switch (e.Action.ToString()) {
					case "UpdateStarted":
						action = "Starting update:  ";
						break;
					case "UpdateAdd":
						action = "Adding:  ";
						break;
					case "UpdateReplace":
						action = "Replacing:  ";
						break;
					case "UpdateUpdate":
						action = "Updating:  ";
						break;
					case "UpdateDelete":
						action = "Deleting:  ";
						break;
					default:
						action = e.Action.ToString() + ":  ";
						break;
				}

				update_txtCheckoutDetail(action);

				switch (e.NodeKind) {
					case SharpSvn.SvnNodeKind.File:
						processed_count += 1;
						update_txtCheckoutDetail(System.IO.Path.GetFileName(e.FullPath));
						break;
					case SharpSvn.SvnNodeKind.Directory:
						processed_count += 1;
						update_txtCheckoutDetail(System.IO.Path.GetDirectoryName(e.FullPath));
						break;
					default:
						update_txtCheckoutDetail(e.FullPath);
						break;
				}
				update_txtCheckoutDetail(Constants.vbCrLf);
				//
				// Ensure the progressive count never exceeds the total.
				// This may happen when a previous checkout operation was cancelled and 
				// the total file count is adjusted to account for existing files. 
				//
				processed_count = (processed_count > total_file_count ? total_file_count : processed_count);
				bgrLongProcess.ReportProgress(Convert.ToInt32(100 * processed_count / total_file_count));
			}
		}

		/// <summary>
		/// Background worker process DoWork handler.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks></remarks>
		private void bgrLongProcess_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			string log = string.Empty;
			string branch_uri = System.IO.Path.Combine(_repository_uri, _branch);
			EventHandler<SharpSvn.SvnNotifyEventArgs> checkout_progress = new EventHandler<SharpSvn.SvnNotifyEventArgs>(svn_client_Notify);
			_checked_out_revision = null;
			_error_message = string.Empty;
			try {
				if (bgrLongProcess.CancellationPending) {
					return;
				}
				SVN.IsCancelled = false;
				total_file_count = SVN.GetBranchRevisionTotalFileCount(branch_uri, _revision);
				if (System.IO.Directory.Exists(System.IO.Path.Combine(_working_dir, _branch))) {
					long current_revision = libSVN.SVN.GetWorkingCopyRevision(System.IO.Path.Combine(_working_dir, _branch));
					int changed_file_count = SVN.GetChangedFilesCount(branch_uri, current_revision.ToString(), _revision);
					total_file_count = changed_file_count;
				}
				processed_count = 0;

				SetTextCallback d = new SetTextCallback(UpdateTextBox);
				string msg = "Fetching branch: " + _branch + "  (" + total_file_count.ToString() + " files to process)..." + Constants.vbCrLf;

				update_txtCheckoutDetail(msg);

				SVN.CheckOutBranch(branch_uri, _revision, System.IO.Path.Combine(_working_dir, _branch), out _checked_out_revision, ref checkout_progress);
				if (_checked_out_revision.HasValue) {
					this.DialogResult = System.Windows.Forms.DialogResult.OK;
				} else {
					this.DialogResult = System.Windows.Forms.DialogResult.None;
					this._error_message = "Failed to checkout revision";
				}
			} catch (SVNConflictException cex) {
				//
				// This exception indicates that a merge conflict occurred during the branch checkout.
				// This situation will most likely ocurr when the local copy Config.ini file has been 
				// changed by choosing to save an updated environment string and a subsequent revision 
				// also has an updated Config.ini file, resulting in a merge conflict.  To alleviate 
				// this common problem, we will attempt to delete any conflict files and repeat the SVN 
				// checkout operation.
				//
				if (cex.NodeKind == SVN.NodeKinds.File && cex.Path == "Config.ini") {
					string msg = cex.Message + Constants.vbCrLf + "Attempting cleanup..." + Constants.vbCrLf;
					update_txtCheckoutDetail(msg);
					string[] files = {
						cex.BaseFile,
						cex.MergedFile,
						cex.TheirFile,
						cex.MyFile
					};
					foreach (string file in files) {
						try {
							System.IO.File.Delete(file);
						} catch (FileNotFoundException ex) {
							// Continue anyway..
						}
					}

					SVN.CheckOutBranch(branch_uri, _revision, System.IO.Path.Combine(_working_dir, _branch), out _checked_out_revision);

					if (_checked_out_revision.HasValue) {
						this.DialogResult = System.Windows.Forms.DialogResult.OK;
						this._error_message = string.Empty;
					} else {
						this.DialogResult = System.Windows.Forms.DialogResult.None;
						this._error_message = "Failed to checkout revision";
					}

					System.Threading.Thread.Sleep(100000);

				} else {
					_error_message = cex.Message;
				}
			} catch (SharpSvn.SvnFileSystemException fex) {
				_error_message = fex.Message;
			} catch (SharpSvn.SvnWorkingCopyLockException lex) {
				_error_message = lex.Message;
			} catch (Exception ex) {
				_error_message = ex.Message;
			}
			if (!string.IsNullOrEmpty(_error_message)) {
				this.DialogResult = System.Windows.Forms.DialogResult.Abort;
				this.SafeCloseForm();
			}
		}
		#endregion

		/// <summary>
		/// Adds the text to the txtCheckoutDetail TextBox.
		/// </summary>
		/// <param name="text"></param>
		/// <remarks></remarks>
		private void update_txtCheckoutDetail(string text)
		{
			SetTextCallback d = new SetTextCallback(UpdateTextBox);
			if (this.txtCheckoutDetail.InvokeRequired) {
				// It's on a different thread, so use Invoke.
				this.txtCheckoutDetail.Invoke(d, new object[] {
					this.txtCheckoutDetail,
					text
				});
			} else {
				// It's on the same thread, no need for Invoke.
				this.txtCheckoutDetail.AppendText(text);
			}
		}

		/// <summary>
		/// Adds the text to the supplied TextBox control.
		/// </summary>
		/// <param name="TextBoxControl"></param>
		/// <param name="text"></param>
		/// <remarks></remarks>
		private void UpdateTextBox(ref System.Windows.Forms.TextBox TextBoxControl, string text)
		{
			TextBoxControl.AppendText(text);
		}

		/// <summary>
		/// Cancels the checkout operation.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks></remarks>
		private void btnCancel_Click(System.Object sender, System.EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			SVN.IsCancelled = true;
			bgrLongProcess.CancelAsync();
		}

		/// <summary>
		/// Closes this form.
		/// </summary>
		/// <remarks>
		/// This method should be used to avoid a potential InvalidOperationException 
		/// when attempting to close the form from other threads.
		/// </remarks>
		private void SafeCloseForm()
		{
			if (this.InvokeRequired) {
				this.BeginInvoke(new MethodInvoker(close_form));
			} else {
				close_form();
			}
		}

		/// <summary>
		/// Closes this form.
		/// </summary>
		/// <remarks></remarks>
		private void close_form()
		{
			this.Close();
		}

		/// <summary>
		/// Form load handler.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks></remarks>
		private void frmCheckout_Load(System.Object sender, System.EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.None;
			if (IsValid()) {
				btnCancel.Enabled = true;
				ProgressBar1.Value = 0;

				bgrLongProcess = new System.ComponentModel.BackgroundWorker();

				// Start the worker.
				bgrLongProcess.WorkerReportsProgress = true;
				bgrLongProcess.WorkerSupportsCancellation = true;
				bgrLongProcess.RunWorkerAsync();
			} else {
				this.DialogResult = System.Windows.Forms.DialogResult.Abort;
			}
		}

		/// <summary>
		/// Checkout validation routine.
		/// </summary>
		/// <returns>Boolean True when all required data has been provided, false otherwise.</returns>
		/// <remarks></remarks>
		private bool IsValid()
		{
			bool is_valid = false;
			_error_message = string.Empty;

			if (string.IsNullOrEmpty(this._repository_uri)) {
				_error_message += "Required Repository URI property has not been set.";
			}
			if (string.IsNullOrEmpty(this._branch)) {
				_error_message += "Required Branch property has not been set.";
			}
			if (string.IsNullOrEmpty(this._revision)) {
				_error_message += "Required Revision property has not been set.";
			}
			if (string.IsNullOrEmpty(this._working_dir)) {
				_error_message += "Required WorkingDirectory property has not been set.";
			}
			if (_error_message == string.Empty) {
				is_valid = true;
			}

			return is_valid;
		}
		public frmCheckout()
		{
			Load += frmCheckout_Load;
			InitializeComponent();
		}

	}
}
