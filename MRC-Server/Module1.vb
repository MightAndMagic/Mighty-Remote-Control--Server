Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Module Module1
    Dim port As Integer = "27590"
    Dim server As TcpListener = Nothing
    Sub Main()
        Console.Title = "Mighty Remote Control Server"
        initialisieren(port)
        While True
            listen()
        End While
    End Sub
    Sub initialisieren(ByVal port)
        Try
            server = New TcpListener(IPAddress.Any, port)
            server.AllowNatTraversal(True)
            server.Start()
            Console.ForegroundColor = ConsoleColor.Gray
            Console.WriteLine("Server erstellt auf Port {0}.", port)
            Console.ForegroundColor = ConsoleColor.Gray
            Console.WriteLine("Warte auf Anfragen...")
        Catch e As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("Socket exception: {0}", e)
        End Try
    End Sub
    Sub listen()
        Dim bytes(128) As Byte
        Dim client As TcpClient = server.AcceptTcpClient()
        Dim stream As NetworkStream = client.GetStream()
        Dim msg As String
        Console.WriteLine("Client verbunden von {0}.", client.Client.RemoteEndPoint)
        Try
            While client.Connected
                Dim i As Int32 = stream.Read(bytes, 0, bytes.Length)
                msg = Encoding.Default.GetString(bytes, 0, i)
                execute(msg)
            End While
        Catch e As Exception
            Console.WriteLine("Verbindung wurde geschlossen.")
        End Try
    End Sub
    Sub execute(msg As String)
        Dim tray As Long
        If msg = "opC" Then
            Console.WriteLine("Öffne CD Fach.")
            tray = mciSendString("set CDAudio door open", "", 0, 0)
        ElseIf msg = "clC" Then
            Console.WriteLine("Schließe CD Fach.")
            tray = mciSendString("set CDAudio door closed", "", 0, 0)
        ElseIf msg = "xt" Then
            End
        Else
            Console.WriteLine("Unbekanntes Kommando: {0}", msg)
        End If
    End Sub
    Private Declare Function mciSendString Lib "winmm.dll" Alias "mciSendStringA" (ByVal lpstrCommand As String, ByVal lpstrReturnString As String, ByVal uReturnLength As Integer, ByVal hwndCallback As Integer) As Integer
End Module
