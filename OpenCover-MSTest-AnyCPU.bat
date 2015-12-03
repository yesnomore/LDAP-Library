packages\OpenCover.4.5.3427\OpenCover.Console.exe -register:user -target:"C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\MSTest.exe" -targetargs:"/noisolation /testcontainer:LDAPLibraryUnitTest\bin\Debug\LDAPLibraryUnitTest.dll" -filter:+[*]* -output:"OpenCoverOut\coverageResult.xml"
packages\ReportGenerator.2.0.2.0\ReportGenerator.exe -reports:"OpenCoverOut\coverageResult.xml" -targetdir:"LDAPLibraryCodeCoverage" 
pause
