::
:: SVN status query to retrieve latest revision number in supplied working copy directory
:: 
:: Usage:
::      GetWorkingCopyRevision.bat branch_name
::
:: where: branch_name = branch name in the launcher.fas repository
::
:: Returns: The revision number retrieved if successful, -1 otherwise.
::
:: History
:: ------------ ----------- -----------------------------------------------------------
:: 02-Nov-2011  CMadden     v2 updates based on original
:: 12-Feb-2013  CMadden     Updated to accept Working_Dir parameter.
::
@ECHO OFF
SETLOCAL ENABLEDELAYEDEXPANSION

SET WORKDIR=%1
SET BRANCH=%2

IF "X%WORKDIR%"=="X" GOTO :usage
IF "X%BRANCH%"=="X" GOTO :usage

SET rev=-1
REM SET working_copy=%1
REM SET PATH=%PATH%;U:\svn_testing\svnbin

:: Call "svn status" on the working copy directory and sort the resulting output on 
:: the file revsion value found starting at the 24th char (in reverse order).  This
:: will result in the latest revision being at the top of the list.
:: Store the results in a text file for further processing.
%WORKDIR%\svnbin\svn status -u -v %BRANCH% 2>NUL | SORT /+24 /R > %BRANCH%_rev.txt 2>&1

:: Keep track of how many lines are searched to ensure we don't spend too much time searching..
SET max_loops=10
SET /a i=0

:: For each line in the file..
FOR /F "tokens=*" %%A IN (%BRANCH%_rev.txt) do (
    REM echo.-------------------------------------
    REM @ECHO %%A
    
    REM increment our line counter..
    SET /a i=i+1
    
    REM If we haven't found it yet, bail!
    IF "!i!" == "%max_loops%" GOTO DONE
    
    REM There will be a single summary line of the form: "Status against revision:    123"
    REM We'll prefix each line with "@@@" and then skip any line starting with "@@@Status against revision:"
    set s=@@@%%A
    REM echo !s!
    
    if "!s!"=="!s:@@@Status against revision:=!" (
        REM echo.No
        
        REM remove the prefix..
        set s=!s:@@@=!
        REM remove the the set of 6 spaces between the first and second values..
        set s=!s:      = !
        
        REM echo !s!
        
        REM Split the line into 2 values delimited by a space char.. 
        for /f "tokens=1,2 delims= " %%a in ("!s!") do set head=%%a&set rev=%%b
        REM echo.head=!head!
        REM echo.rev=!rev!
        IF NOT "X!rev!" == "X" GOTO :DONE
    )
)

:DONE
:: remove our temp output file..
DEL /F %BRANCH%_rev.txt
GOTO :output



:usage
ECHO.
ECHO Usage: %0 working_directory_path branch_path
ECHO e.g. GetWorkingCopyRevision.bat U:\svn_testing ..\AnnuitiesFVUtility
ECHO.

GOTO :end

:output
IF "!rev!" == "-1" (
    ECHO No revision found.
) ELSE (
    ECHO Revision:!rev!
)

:end
EXIT /B !rev!
