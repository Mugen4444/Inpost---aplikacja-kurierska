using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

internal class DbClient
{
    private string ipAddress;
    private int port;

    public DbClient(string ipAddress, int port)
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

    public DbClient() : this("127.0.0.1", 5601) { }

    public string Query(string operation, string data)
    {
        if ("ADD GET DELETE".Contains(operation.ToUpper()))
        {
            if (data.Length > 0)
            {
                var response = sendMessage($"{operation.ToUpper()} {data}");

                if (new Regex(@"NOT FOUND|INVALID|EMPTY|ERR").IsMatch(response))
                {
                    throw new ApplicationException(response);
                }

                return response;
            }
            else
            {
                throw new ApplicationException("Data cannot be empty (dbclient)");
            }
        }
        else
        {
            throw new ApplicationException("Invalid operation (dbclient)");
        }
    }

    public string sendMessage(string message)
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
