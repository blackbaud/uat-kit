If (Not IsArray($CmdLine) Or $CmdLine[0] < 2) Then
	$user = InputBox("User", "Please enter your user", "")
	$pass = InputBox("Password", "Please enter your password", "", "*M")
Else
	$user = $CmdLine[1]
	$pass = $CmdLine[2]
EndIf

$hWnd = WinWait("", "Chrome Legacy Window", 20)
If WinExists("", "Chrome Legacy Window") Then
	$sControl = ControlGetFocus($hWnd)
	ControlSend($hWnd,"",$sControl,$user)
	ControlSend($hWnd,"",$sControl,"{TAB}")
	ControlSend($hWnd,"",$sControl,$pass)
	ControlSend($hWnd,"",$sControl,"{ENTER}")
EndIf