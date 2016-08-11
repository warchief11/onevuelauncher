using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OneVueLauncher
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// <code region="History" lang="other" title="History">
    /// Date        Developer           Description
    /// ----------- ------------------- ------------------------------------------------------------------
    /// 12-Aug-2016 NSingh              Original
    /// </code>
    /// </remarks>
    static class Program
    {
        const string APP_EMAIL_SERVER = "smtpsydp.au.challenger.net";
        const string APP_LITERAL_QUOTE = "\"";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmLauncher());
        }
    }
}
