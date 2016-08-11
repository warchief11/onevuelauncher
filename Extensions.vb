Imports System.Text

Namespace Extensions
    ''' <summary>
    ''' Extension methods for Exceptions.
    ''' <example>
    ''' E.g. usage:
    ''' <code>
    ''' using Challenger.OneVue.Util.ExtensionMethods;
    ''' 
    '''     ...
    '''     catch(Exception ex){
    '''         error_msg = ex.Format()
    '''     }
    ''' </code>
    ''' example message format:
    ''' 
    ''' System.OverflowException was caught 
    ''' Message=Arithmetic operation resulted in an overflow. 
    ''' Source=Challenger.OneVue.Util.DevTest 
    ''' Stacktrace:    at Challenger.OneVue.Util.DevTest.Extensions_DevTest.Extensions_DevTest_Exceptions() 
    ''' in C:\svn\OneVue\branches\tran_batches_2013\Challenger.OneVue.Util\Challenger.OneVue.Util.DevTest\Extensions_DevTest.vb:line 44
    ''' 
    ''' </example>
    ''' </summary>
    ''' <remarks>
    ''' <code region="History" lang="other" title="History">
    ''' Date        Developer       Description
    ''' ----------- --------------- ------------------------------------------------------------------
    ''' 30-Jul-2013 CMadden         Original
    ''' </code>
    ''' </remarks>
    Public Module Exception
        ''' <summary>
        ''' Extends a System.Exception object to add a formatted string output.
        ''' </summary>
        ''' <param name="ex"></param>
        ''' <returns></returns>
        <System.Runtime.CompilerServices.Extension()> _
        Public Function Format(ByVal ex As System.Exception) As String
            Dim str As New StringBuilder()
            str.AppendLine(ex.[GetType]().ToString() & " was caught")
            str.AppendLine(FormatException(ex, ""))
            Return str.ToString()
        End Function
        <System.Runtime.CompilerServices.Extension()> _
        Private Function FormatException(ByVal ex As System.Exception, ByVal prefix As String) As String
            If ex IsNot Nothing Then
                Dim str As New StringBuilder()
                str.AppendLine(prefix & "Message=" & ex.Message)
                str.AppendLine(prefix & "Source=" & ex.Source)
                str.AppendLine(prefix & "Stacktrace:")
                If ex.StackTrace IsNot Nothing Then
                    For Each ln As String In ex.StackTrace.Split(New Char() {ControlChars.Cr, ControlChars.Lf})
                        str.AppendLine(prefix & ln)
                    Next
                End If
                If ex.InnerException IsNot Nothing Then
                    str.AppendLine()
                    str.AppendLine(prefix & "InnerException:")
                    str.AppendLine(prefix & FormatException(ex.InnerException, prefix & vbTab))
                End If
                Return str.ToString()
            Else
                Return [String].Empty
            End If
        End Function
    End Module
End Namespace
