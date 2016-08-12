using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace OneVueLauncher
{
	/// <summary>
	/// Launcher Branch Properties.  These are loaded from the branches.xml file.
	/// </summary>
	/// <remarks>
	/// <code region="History" lang="other" title="History">
	/// Date        Developer       Description
	/// ----------- --------------- ------------------------------------------------------------------
	/// 30-Jul-2013 CMadden         Added BranchWorkingDirectory
	/// </code>
	/// </remarks>
	public class Branch
	{

		public string BranchName;
		public string BranchVersion;
		public string BranchRevision;
		public string BranchDisplayName;
		public string BranchDescription;
		public string BranchContact;
		public string BranchExeName;
        public SourceControl SourceControl;
		public string BranchWorkingDirectory;
	}

    public enum SourceControl
    {
        Svn = 0,
        Git
    }
}
