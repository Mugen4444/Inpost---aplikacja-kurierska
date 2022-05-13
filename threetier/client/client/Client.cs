using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

internal class Client
{
    private int port;
    private string ipAddress;

    public Client(string ipAddress, int port)
    {
        if (!IsValidIp(ipAddress))
        {
            throw new ApplicationException("Niepoprawny IP adres");
        }

        if (!IsValidPort(port))
        {
            throw new ApplicationException("Niepoprawny port");
        }

        this.ipAddress = ipAddress;
        this.port = port;
    }

    public Client() : this("127.0.0.1", 3001) { }

    public void Run()
    {
        Console.WriteLine("Wybierz operacje (WYSLIJ, ODBIERZ): ");

        try
        {
            var operacja = new Regex(@"[^A-Za-z]").Replace(
                Console.ReadLine().Trim().ToUpper().Replace(" ", ""), "");

            switch (operacja)
            {
                case "WYSLIJ":
                    WyslijPaczke();
                    break;
                case "ODBIERZ":
                    OdbierzPaczke();
                    break;
                default:
                    throw new ApplicationException("Niepoprawna operacja");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Wystapil blad: " + ex.Message);
        }
    }

    private void WyslijPaczke()
    {
        Console.WriteLine("Co wysylasz?");
        var zawartosc = new Regex(@"[^A-Za-z\s]").Replace(
            Console.ReadLine().Trim(), "");

        Console.WriteLine(sendMessage("WYSLIJ " + new Paczka(zawartosc).Zawartosc));
    }

    private void OdbierzPaczke()
    {
        Console.WriteLine("Podaj Id paczki: ");

        var idStr = new Regex(@"[^0-9]*").Replace(
                Console.ReadLine().Trim().Replace(" ", ""), "");


        if (!IsValidIdStr(idStr))
        {
            throw new ApplicationException("Niepoprawny Id paczki");
        }

        var idPaczki = int.Parse(idStr);

        Console.WriteLine(sendMessage("ODBIERZ " + idPaczki));
    }

    private string sendMessage(string message)
    {
        string response = "";
        try
        {
            TcpClient client = new TcpClient(ipAddress, port); // Create a new connection  
            client.NoDelay = true; // please check TcpClient for more optimization
                                   // messageToByteArray- discussed later
            byte[] messageBytes = messageToByteArray(message);

            using (NetworkStream stream = client.GetStream())
            {
                stream.Write(messageBytes, 0, messageBytes.Length);

                // Message sent!  Wait for the response stream of bytes...
                // streamToMessage - discussed later
                response = streamToMessage(stream);
            }
            client.Close();
        }
        catch (Exception e) { Console.WriteLine(e.Message); }
        return response;
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

    private bool IsValidPort(int port)
    {
        return !(port < 0 || port > 65535);
    }

    private bool IsValidIp(string ip)
    {
        if (!new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}").IsMatch(ip))
        {
            return false;
        }

        string[] num = ip.Split(".");

        if (num.Length < 4)
            return false;

        foreach (var n in num)
        {
            try
            {
                var nn = int.Parse(n);

                if (nn < 0 || nn > 255)
                    return false;
            }
            catch
            {
                return false;
            }
        }

        return true;
    }
}
