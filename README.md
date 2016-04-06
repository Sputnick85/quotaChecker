This is a small Nagiosplugin which should help you checking your quotas. It provides performancedata.

FS-Resource-Manager must be installed.
.NET Framework 4.5.2 must be installed.

Usage of quotaChecker:

quotaChecker.exe $PATH $WARNING $CRITICAL

$PATH -> C:\Folder 

$WARNING -> 90 (90% space used) 

$CRITICAL -> 95 (95% space used)

quotaChecker.exe C:\Folder 90 95

Output -> OK - c:\folder -> 6,79GB/35,00GB | 'c:\folder'=6957MB;32256;34048

Get the binarys from SourcForge https://sourceforge.net/projects/quotachecker/files/
