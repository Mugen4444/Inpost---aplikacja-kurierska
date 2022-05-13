using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

internal class Server
{
    private volatile bool running;
    private Thread server;

    private TcpListener listener;
    private IPEndPoint ep;

    private List<Paczka> paczki;

    public Server(int port)
    {
        running = false;

        if (port < 0 || port > 65535)
        {
            throw new ArgumentOutOfRangeException("Expected port in range 0 - 65535");
        }

        server = new Thread(() =>
        {
            while (running)
            {
                try
                {
                    Run();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Wystapil blad: " + ex.Message);
                }
            }
        });

        // IP Address to listen on. Loopback in this case
        IPAddress ipAddr = IPAddress.Loopback;
        // Create a network endpoint
        ep = new IPEndPoint(ipAddr, port);
        // Create and start a TCP listener
        listener = new TcpListener(ep);

        paczki = new List<Paczka>();
    }

    public Server() : this(5601) { }

    private void sendMessage(string message, TcpClient client)
    {
        // messageToByteArray- discussed later
        byte[] bytes = messageToByteArray(message);
        client.GetStream().Write(bytes, 0, bytes.Length);
    }

    public string MessageHandler(string message)
    {
        Console.WriteLine("Received message: " + message);

        var dane = message.Split(' ');

        if (dane.Length < 2)
        {
            return "ERR NO DATA";
        }

        var operacja = dane[0];

        switch (operacja.ToUpper())
        {
            case "ADD":
                if (!IsValidIdStr(dane[1]))
                {
                    return "INVALID ID";
                }
                int id = int.Parse(dane[1]);
                var zawartosc = "";
                for (var i = 2; i < dane.Length; i++)
                {
                    zawartosc += (i == 2 ? "" : " ") + dane[i];
                }

                if (zawartosc.Length > 0)
                {
                    paczki.Add(new Paczka(id, zawartosc));
                    return "OK";
                }
                return "EMPTY ZAWARTOSC";
            case "GET":
                if (!IsValidIdStr(dane[1]))
                {
                    return "INVALID ID";
                }
                id = int.Parse(dane[1]);
                var paczka = paczki.Find(paczka => paczka.Id == id);
                if (paczka is null)
                {
                    return "NOT FOUND";
                }

                return paczka.ToString();
            case "DELETE":
                if (!IsValidIdStr(dane[1]))
                {
                    return "INVALID ID";
                }
                id = int.Parse(dane[1]);
                paczka = paczki.Find(paczka => paczka.Id == id);
                if (paczka is null)
                {
                    return "NOT FOUND";
                }
                paczki.Remove(paczka);
                return "OK";
            default:
                return "INVALID OPERATION";
        }
    }

    private void Run()
    {
        var sender = listener.AcceptTcpClient();
        // streamToMessage - discussed later
        string request = streamToMessage(sender.GetStream());
        if (request != null)
        {
            string responseMessage = MessageHandler(request);
            sendMessage(responseMessage, sender);
        }
    }

    public void Start()
    {
        Console.WriteLine("Starting database...");
        listener.Start();
        Console.WriteLine("Database server listening on: {0}:{1}", ep.Address, ep.Port);

        // keep running
        running = true;
        server.Start();
    }

    public void Stop()
    {
        running = false;
        server.Join();
    }

    // using UTF8 encoding for the messages
    static Encoding encoding = Encoding.UTF8;
    private byte[] messageToByteArray(string message)
    {
        // get the size of original message
        byte[] messageBytes = encoding.GetBytes(message);
        int messageSize = messageBytes.Length;
        // add content length bytes to the original size
        int completeSize = messageSize + 4;
        // create a buffer of the size of the complete message size
        byte[] completemsg = new byte[completeSize];

        // convert message size to bytes
        byte[] sizeBytes = BitConverter.GetBytes(messageSize);
        // copy the size bytes and the message bytes to our overall message to be sent 
        sizeBytes.CopyTo(completemsg, 0);
        messageBytes.CopyTo(completemsg, 4);
        return completemsg;
    }

    private string streamToMessage(Stream stream)
    {
        // size bytes have been fixed to 4
        byte[] sizeBytes = new byte[4];
        // read the content length
        stream.Read(sizeBytes, 0, 4);
        int messageSize = BitConverter.ToInt32(sizeBytes, 0);
        // create a buffer of the content length size and read from the stream
        byte[] messageBytes = new byte[messageSize];
        stream.Read(messageBytes, 0, messageSize);
        // convert message byte array to the message string using the encoding
        string message = encoding.GetString(messageBytes);
        string result = null;
        foreach (var c in message)
            if (c != '\0')
                result += c;

        return result;
    }

    private bool IsValidIdStr(string idStr)
    {
        if (idStr.Length == 0)
        {
            return false;
        }

        if (new Regex(@"\D").IsMatch(idStr))
        {
            return false;
        }

        var idPaczki = int.Parse(idStr);

        if (idPaczki < 0 || idPaczki > 9999)
        {
            return false;
        }

        return true;
    }
}

