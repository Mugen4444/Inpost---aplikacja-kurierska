using System.Net;
using System.Net.Sockets;
using System.Text;

internal class Server
{
    private volatile bool running;
    private Thread server;

    private TcpListener listener;
    private IPEndPoint ep;

    private DbClient db;

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

        db = new DbClient();
    }

    public Server() : this(3001) { }

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
            return "Wystapil blad";
        }

        var operacja = dane[0];
        

        switch (operacja.ToUpper())
        {
            case "WYSLIJ":
                string zawartoscPaczki = "";
                for (var i = 1; i < dane.Length; i++)
                {
                    zawartoscPaczki += (i == 1 ? "" : " ") + dane[i];
                }

                if (zawartoscPaczki.Length == 0)
                {
                    return "Paczka nie moze byc pusta";
                }
                var paczka = new Paczka(new Random().Next(0, 9999), zawartoscPaczki);
                try
                {
                    Console.WriteLine("LOG: " + db.Query("ADD", paczka.ToString()));
                    return $"Wyslano paczke {paczka}";
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERR: " + ex.Message);
                    return "Nie udalo sie wyslac paczki";
                }

            case "ODBIERZ":
                var idPaczki = int.Parse(dane[1]);
                try
                {
                    var result = db.Query("GET", idPaczki.ToString());
                    Console.WriteLine("LOG: " + result);

                    paczka = Paczka.FromString(result);
                    db.Query("DELETE", paczka.Id.ToString());
                    return "Odebrano paczke z zawartoscia " + paczka.Zawartosc + " Id paczki " + paczka.Id;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERR: " + ex.Message);
                    return "Nie udalo sie odebrac paczke";
                }
            default:
                return "Niepoprawna operacja";
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
        Console.WriteLine("Server starting !");
        listener.Start();
        Console.WriteLine("Server listening on: {0}:{1}", ep.Address, ep.Port);

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
}
