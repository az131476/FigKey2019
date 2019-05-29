%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil.exe "D:\work\project\FigKey\FigKeyLoggerServer\FigKeyLoggerServer\bin\Debug\FigKeyLoggerServer.exe"
Net Start FigKeyLoggerService
sc config FigKeyLoggerService start= auto
pause