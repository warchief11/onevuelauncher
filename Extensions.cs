using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Text;

namespace OneVueLauncher.Extensions
{
	/// <summary>
	/// Extension methods for Exceptions.
	/// <example>
	/// E.g. usage:
	/// <code>
	/// using Challenger.OneVue.Util.ExtensionMethods;
	/// 
	///     ...
	///     catch(Exception ex){
	///         error_msg = ex.Format()
	///     }
	/// </code>
	/// example message format:
	/// 
	/// System.OverflowException was caught 
	/// Message=Arithmetic operation resulted in an overflow. 
	/// Source=Challenger.OneVue.Util.DevTest 
	/// Stacktrace:    at Challenger.OneVue.Util.DevTest.Extensions_DevTest.Extensions_DevTest_Exceptions() 
	/// in C:\svn\OneVue\branches\tran_batches_2013\Challenger.OneVue.Util\Challenger.OneVue.Util.DevTest\Extensions_DevTest.vb:line 44
	/// 
	/// </example>
	/// </summary>
	/// <remarks>
	/// <code region="History" lang="other" title="History">
	/// Date        Developer       Description
	/// ----------- --------------- ------------------------------------------------------------------
	/// 30-Jul-2013 CMadden         Original
	/// </code>
	/// </remarks>
	public static class Exception
	{
		/// <summary>
		/// Extends a System.Exception object to add a formatted string output.
		/// </summary>
		/// <param name="ex"></param>
		/// <returns></returns>
		public static string Format(this System.Exception ex)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine(ex.GetType().ToString() + " was caught");
			str.AppendLine(FormatException(ex, ""));
			return str.ToString();
		}
        private static string FormatException(this System.Exception ex, string prefix)
		{
			if (ex != null) {
				StringBuilder str = new StringBuilder();
				str.AppendLine(prefix + "Message=" + ex.Message);
				str.AppendLine(prefix + "Source=" + ex.Source);
				str.AppendLine(prefix + "Stacktrace:");
				if (ex.StackTrace != null) {
					foreach (string ln in ex.StackTrace.Split(new char[] {
						ControlChars.Cr,
						ControlChars.Lf
					})) {
						str.AppendLine(prefix + ln);
					}
				}
				if (ex.InnerException != null) {
					str.AppendLine();
					str.AppendLine(prefix + "InnerException:");
					str.AppendLine(prefix + FormatException(ex.InnerException, prefix + Constants.vbTab));
				}
				return str.ToString();
			} else {
				return String.Empty;
			}
		}
	}
}
