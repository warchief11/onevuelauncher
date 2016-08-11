Imports libSVN
''' <summary>
''' frmCheckout - processes SVN checkouts and displays progress.
''' </summary>
''' <remarks>
''' <code region="History" lang="other" title="History">
''' Date          Developer       Description
''' -----------   --------------- ------------------------------------------------------------------
''' 24-Jul-2013   CMadden         Original
''' 10-Dec-2013   CMadden         Fixed bug calculating the number of changed files for progress in 
'''                               bgrLongProcess_DoWork().
''' 14-Jul-2014   CMadden         Added merge conflict resolution for Config.ini files.
''' </code>
''' </remarks>
Public Class frmCheckout

    Friend WithEvents bgrLongProcess As System.ComponentModel.BackgroundWorker
    Private total_file_count As Integer = 0
    Private processed_count As Integer = 0
    ' This delegate enables asynchronous calls for setting
    ' the text property on a txtCheckoutDetail control.
    Delegate Sub SetTextCallback(ByRef txtControl As Control, ByVal [text] As String)

    Private _repository_uri As String
    ''' <summary>
    ''' The branch repository URI
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RepositoryURI() As String
        Get
            Return _repository_uri
        End Get
        Set(ByVal value As String)
            _repository_uri = value
        End Set
    End Property

    Private _branch As String
    ''' <summary>
    ''' The required branch name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Branch() As String
        Get
            Return _branch
        End Get
        Set(ByVal value As String)
            _branch = value
        End Set
    End Property

    Private _revision As String
    ''' <summary>
    ''' The required revision number or name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Revision() As String
        Get
            Return _revision
        End Get
        Set(ByVal value As String)
            _revision = value
        End Set
    End Property

    Private _working_dir As String
    ''' <summary>
    ''' The checkout working directory path
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property WorkingDir() As String
        Get
            Return _working_dir
        End Get
        Set(ByVal value As String)
            _working_dir = value
        End Set
    End Property

    Private _checked_out_revision As Long?
    ''' <summary>
    ''' The retrieved branch revision number
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property CheckedOutRevision() As Long?
        Get
            Return _checked_out_revision
        End Get
    End Property

    Private _error_message As String
    ''' <summary>
    ''' An error message set to indicate an issue with processing
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ErrorMessage() As String
        Get
            Return _error_message
        End Get
    End Property

#Region " Background worker "
    ''' <summary>
    ''' Displays the progress.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub bgrLongProcess_ProgressChanged(ByVal sender As Object, _
                                               ByVal e As System.ComponentModel.ProgressChangedEventArgs) _
                                                                    Handles bgrLongProcess.ProgressChanged
        ProgressBar1.Value = e.ProgressPercentage
    End Sub

    ''' <summary>
    ''' Background worker process RunWorkerCompleted handler.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub bgrLongProcess_RunWorkerCompleted(ByVal sender As Object, _
                                                  ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) _
                                                                    Handles bgrLongProcess.RunWorkerCompleted
        btnCancel.Enabled = False
    End Sub

    ''' <summary>
    ''' The SVN Notify callback handler. Updates progress details.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub svn_client_Notify(ByVal sender As Object, ByVal e As SharpSvn.SvnNotifyEventArgs)
        Dim action As String = String.Empty
        If bgrLongProcess.CancellationPending Then
            Dim msg As String = "Cancelling."
            update_txtCheckoutDetail(msg)
        End If
        If e IsNot Nothing Then
            Select Case e.Action.ToString
                Case "UpdateStarted" : action = "Starting update:  "
                Case "UpdateAdd" : action = "Adding:  "
                Case "UpdateReplace" : action = "Replacing:  "
                Case "UpdateUpdate" : action = "Updating:  "
                Case "UpdateDelete" : action = "Deleting:  "
                Case Else : action = e.Action.ToString + ":  "
            End Select

            update_txtCheckoutDetail(action)

            Select Case e.NodeKind
                Case SharpSvn.SvnNodeKind.File
                    processed_count += 1
                    update_txtCheckoutDetail(IO.Path.GetFileName(e.FullPath))
                Case SharpSvn.SvnNodeKind.Directory
                    processed_count += 1
                    update_txtCheckoutDetail(IO.Path.GetDirectoryName(e.FullPath))
                Case Else
                    update_txtCheckoutDetail(e.FullPath)
            End Select
            update_txtCheckoutDetail(vbCrLf)
            '
            ' Ensure the progressive count never exceeds the total.
            ' This may happen when a previous checkout operation was cancelled and 
            ' the total file count is adjusted to account for existing files. 
            '
            processed_count = IIf(processed_count > total_file_count, total_file_count, processed_count)
            bgrLongProcess.ReportProgress(CInt(100 * processed_count / total_file_count))
        End If
    End Sub

    ''' <summary>
    ''' Background worker process DoWork handler.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub bgrLongProcess_DoWork(ByVal sender As Object, _
     ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bgrLongProcess.DoWork
        Dim log As String = String.Empty
        Dim branch_uri As String = IO.Path.Combine(_repository_uri, _branch)
        Dim checkout_progress As New EventHandler(Of SharpSvn.SvnNotifyEventArgs)(AddressOf svn_client_Notify)
        _checked_out_revision = Nothing
        _error_message = String.Empty
        Try
            If bgrLongProcess.CancellationPending Then
                Exit Sub
            End If
            SVN.IsCancelled = False
            total_file_count = SVN.GetBranchRevisionTotalFileCount(branch_uri, _revision)
            If IO.Directory.Exists(IO.Path.Combine(_working_dir, _branch)) Then
                Dim current_revision As Long = libSVN.SVN.GetWorkingCopyRevision(IO.Path.Combine(_working_dir, _branch))
                Dim changed_file_count As Integer = SVN.GetChangedFilesCount(branch_uri, current_revision.ToString, _revision)
                total_file_count = changed_file_count
            End If
            processed_count = 0

            Dim d As New SetTextCallback(AddressOf UpdateTextBox)
            Dim msg As String = "Fetching branch: " + _branch + "  (" _
                            + total_file_count.ToString + " files to process)..." + vbCrLf

            update_txtCheckoutDetail(msg)

            SVN.CheckOutBranch(branch_uri, _
                               _revision, IO.Path.Combine(_working_dir, _branch), _
                               _checked_out_revision, _
                               checkout_progress)
            If _checked_out_revision.HasValue Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
            Else
                Me.DialogResult = Windows.Forms.DialogResult.None
                Me._error_message = "Failed to checkout revision"
            End If
        Catch cex As SVNConflictException
            '
            ' This exception indicates that a merge conflict occurred during the branch checkout.
            ' This situation will most likely ocurr when the local copy Config.ini file has been 
            ' changed by choosing to save an updated environment string and a subsequent revision 
            ' also has an updated Config.ini file, resulting in a merge conflict.  To alleviate 
            ' this common problem, we will attempt to delete any conflict files and repeat the SVN 
            ' checkout operation.
            '
            If cex.NodeKind = SVN.NodeKinds.File AndAlso cex.Path = "Config.ini" Then
                Dim msg As String = cex.Message + vbCrLf + "Attempting cleanup..." + vbCrLf
                update_txtCheckoutDetail(msg)
                Dim files() As String = {cex.BaseFile, cex.MergedFile, cex.TheirFile, cex.MyFile}
                For Each file As String In files
                    Try
                        IO.File.Delete(file)
                    Catch ex As IO.FileNotFoundException
                        ' Continue anyway..
                    End Try
                Next

                SVN.CheckOutBranch(branch_uri, _
                   _revision, _
                   IO.Path.Combine(_working_dir, _branch), _
                   _checked_out_revision)

                If _checked_out_revision.HasValue Then
                    Me.DialogResult = Windows.Forms.DialogResult.OK
                    Me._error_message = String.Empty
                Else
                    Me.DialogResult = Windows.Forms.DialogResult.None
                    Me._error_message = "Failed to checkout revision"
                End If

                Threading.Thread.Sleep(100000)

            Else
                _error_message = cex.Message
            End If
        Catch fex As SharpSvn.SvnFileSystemException
            _error_message = fex.Message
        Catch lex As SharpSvn.SvnWorkingCopyLockException
            _error_message = lex.Message
        Catch ex As Exception
            _error_message = ex.Message
        End Try
        If Not String.IsNullOrEmpty(_error_message) Then
            Me.DialogResult = Windows.Forms.DialogResult.Abort
            Me.SafeCloseForm()
        End If
    End Sub
#End Region

    ''' <summary>
    ''' Adds the text to the txtCheckoutDetail TextBox.
    ''' </summary>
    ''' <param name="text"></param>
    ''' <remarks></remarks>
    Private Sub update_txtCheckoutDetail(ByVal text As String)
        Dim d As New SetTextCallback(AddressOf UpdateTextBox)
        If Me.txtCheckoutDetail.InvokeRequired Then
            ' It's on a different thread, so use Invoke.
            Me.txtCheckoutDetail.Invoke(d, New Object() {Me.txtCheckoutDetail, text})
        Else
            ' It's on the same thread, no need for Invoke.
            Me.txtCheckoutDetail.AppendText(text)
        End If
    End Sub

    ''' <summary>
    ''' Adds the text to the supplied TextBox control.
    ''' </summary>
    ''' <param name="TextBoxControl"></param>
    ''' <param name="text"></param>
    ''' <remarks></remarks>
    Private Sub UpdateTextBox(ByRef TextBoxControl As Windows.Forms.TextBox, ByVal text As String)
        TextBoxControl.AppendText(text)
    End Sub

    ''' <summary>
    ''' Cancels the checkout operation.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        SVN.IsCancelled = True
        bgrLongProcess.CancelAsync()
    End Sub

    ''' <summary>
    ''' Closes this form.
    ''' </summary>
    ''' <remarks>
    ''' This method should be used to avoid a potential InvalidOperationException 
    ''' when attempting to close the form from other threads.
    ''' </remarks>
    Private Sub SafeCloseForm()
        If Me.InvokeRequired Then
            Me.BeginInvoke(New MethodInvoker(AddressOf close_form))
        Else
            close_form()
        End If
    End Sub

    ''' <summary>
    ''' Closes this form.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub close_form()
        Me.Close()
    End Sub

    ''' <summary>
    ''' Form load handler.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub frmCheckout_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.DialogResult = Windows.Forms.DialogResult.None
        If IsValid() Then
            btnCancel.Enabled = True
            ProgressBar1.Value = 0

            bgrLongProcess = New System.ComponentModel.BackgroundWorker()

            ' Start the worker.
            bgrLongProcess.WorkerReportsProgress = True
            bgrLongProcess.WorkerSupportsCancellation = True
            bgrLongProcess.RunWorkerAsync()
        Else
            Me.DialogResult = Windows.Forms.DialogResult.Abort
        End If
    End Sub

    ''' <summary>
    ''' Checkout validation routine.
    ''' </summary>
    ''' <returns>Boolean True when all required data has been provided, false otherwise.</returns>
    ''' <remarks></remarks>
    Private Function IsValid() As Boolean
        Dim is_valid As Boolean = False
        _error_message = String.Empty

        If String.IsNullOrEmpty(Me._repository_uri) Then
            _error_message += "Required Repository URI property has not been set."
        End If
        If String.IsNullOrEmpty(Me._branch) Then
            _error_message += "Required Branch property has not been set."
        End If
        If String.IsNullOrEmpty(Me._revision) Then
            _error_message += "Required Revision property has not been set."
        End If
        If String.IsNullOrEmpty(Me._working_dir) Then
            _error_message += "Required WorkingDirectory property has not been set."
        End If
        If _error_message = String.Empty Then
            is_valid = True
        End If

        Return is_valid
    End Function

End Class