::
:: SVN checkout or update script
:: Called from OneVue Launcher.
:: Usage:
::      updatereleases.bat svn_testing_path branch_name rev
::
:: where: svn_testing_path = the path to the svn_testing directory
::        branch_name = branch name in the launcher.fas repository
::        rev = the required revision number (can also be HEAD etc. as per the svn.exe help documentation)
::
:: Returns: The revision number retrieved if successful, -1 otherwise.
::
:: History
:: ------------ ----------- -----------------------------------------------------------
:: 02-Nov-2011  CMadden     v2 updates based on original
:: 12-Feb-2013  CMadden     Updated to accept Working_Dir parameter and restructured 
::                          checkout directories to remove hard-coded paths and reflect 
::                          actual SVN paths.
::
@ECHO OFF
SETLOCAL ENABLEEXTENSIONS
:: ----------------------------------------------------
:: SVN Server Name
:: ----------------------------------------------------
SET WORKDIR=%1
SET BRANCH=%2
SET REV=%3
SET SVNSERVER=launcher.fas.au.challenger.net
:: The SVN root on the server
SET SVNROOT=fas/onevuereleases2009
:: A path string equivalent to the SVNROOT
SET CHECKOUT_DIR=%SVNROOT:/=\%
SET error_level=-1 

SET checkout_result=
SET update_result=

IF "X%WORKDIR%"=="X" GOTO :usage
IF "X%BRANCH%"=="X" GOTO :usage
IF "X%REV%"=="X" GOTO :usage


:: ----------------------------------------------------
:: DECIDE WHETHER TO CHECKOUT OR UPDATE
:: ----------------------------------------------------
IF NOT EXIST %WORKDIR%\%SVNSERVER%\%CHECKOUT_DIR% MKDIR %WORKDIR%\%SVNSERVER%\%CHECKOUT_DIR%

CHDIR /D %WORKDIR%\%SVNSERVER%\%CHECKOUT_DIR%
IF EXIST %BRANCH%\nul (GOTO :UPDATE) else (GOTO :CHECKOUT)


:CHECKOUT
:: ----------------------------------------------------
:: GET FROM REPOSITORY THE ONEVUE VERSION REQUIRED
:: ----------------------------------------------------

%WORKDIR%\svnbin\svn checkout "svn://%SVNSERVER%/%SVNROOT%/%BRANCH%" -r %REV% "%BRANCH%" > svn_checkout.txt
::Will output list of affected files and..
::Checked out revision 530.
FINDSTR /L /B /C:"Checked out revision " svn_checkout.txt > svn_checkout2.txt
SET /p checkout_result= < svn_checkout2.txt

::ECHO checkout_result:%checkout_result%

DEL svn_checkout*.txt

:: Removed "Checked out revision " from the string using substitution..
IF NOT "X%checkout_result%"=="X" SET checkout_result=%checkout_result:Checked out revision=%
IF NOT "X%checkout_result%"=="X" SET checkout_result=%checkout_result:~1,-1%

::ECHO checkout_result:%checkout_result%

IF NOT "X%checkout_result%"=="X" SET error_level=%checkout_result%

:: Grant Full access to the working folder for everyone else that may use this workstation..
CACLS %BRANCH% /T /E /C /G EVERYONE:F >NUL

CHDIR /D %BRANCH%

<nul set /p =Checkout 
GOTO :report_rev


:UPDATE
:: ----------------------------------------------------
:: UPDATE FROM REPOSITORY THE ONEVUE VERSION REQUIRED
:: ----------------------------------------------------
:: Grant Full access to the working folder for everyone else that may use this workstation..
CACLS %BRANCH% /T /E /C /G EVERYONE:F >NUL

CHDIR /D %BRANCH% 
%WORKDIR%\svnbin\svn update -r %REV% > svn_update.txt 2>&1 

FINDSTR /L /B /C:"At revision " svn_update.txt > svn_update2.txt
SET /p update_result= < svn_update2.txt

:: SVN update will return either "At revision 123." or "Updated to revision 123." as a succesful status message
:: Try to remove "At revision " from the string using substitution..
IF NOT "X%update_result%"=="X" SET update_result=%update_result:At revision=%
::ECHO 1 update_result:%update_result%

IF NOT "X%update_result%"=="X" SET update_result=%update_result:~1,-1%
::ECHO 2 update_result:%update_result%

::If nothing found, try looking for the string "Updated to revision "
IF "X%update_result%" == "X" FINDSTR /L /B /C:"Updated to revision " svn_update.txt > svn_update2.txt
IF "X%update_result%" == "X" SET /p update_result= < svn_update2.txt
::ECHO 3 update_result:%update_result%
    
:: Remove "Updated to revision " from the string using substitution..
IF NOT "X%update_result%"=="X" SET update_result=%update_result:Updated to revision=%
::ECHO 4 update_result:%update_result%
    
:: Trim leading space and trailing period
IF NOT "X%update_result%"=="X" SET update_result=%update_result:~1,-1%
::ECHO 5 update_result:%update_result%

DEL svn_update*.txt

IF NOT "X%update_result%"=="X" SET error_level=%update_result%
::ECHO error_level=[%error_level%]

<nul set /p =Update 
GOTO :report_rev


:report_rev
::
:: If the checkout or update received a revision number, it will be for 
:: the entire repository, so a call to GetWorkingCopyRevision.bat can 
:: be used to determine the revision number of the actual branch..
::
IF NOT %error_level% EQU -1 CALL %WORKDIR%\svnbin\GetWorkingCopyRevision.bat %WORKDIR% ..\%BRANCH%

:: GetWorkingCopyRevision also sets the revision number as it's ERRORLEVEL..
IF NOT %error_level% EQU -1 SET error_level=%ERRORLEVEL%

::ECHO rev:%error_level%

GOTO :end

:usage
ECHO.
ECHO USAGE %0 PATH_TO_SVN_TESTING BRANCH_NAME REVISION
ECHO.


:end
EXIT %error_level%

