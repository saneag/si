using System.Text;
using aes;

public class Program
{
    public static void Main()
    {
        var input = "Two One Nine Two";

        var key = "Thats my Kung Fu";

        var aes = new AES(AES.KeySize.Bits128);

        var convertedText = Encoding.UTF8.GetBytes(input);
        var convertedKey = Encoding.UTF8.GetBytes(key);
        var encrypted = aes.Encrypt(convertedText, convertedKey);
        var decrypted = aes.Decrypt(encrypted, convertedKey);

        Console.WriteLine("Original: " + input);
        Console.WriteLine("Key: " + key);

        Console.WriteLine("Converted Text: " + aes.ByteArrayToString(convertedText));
        Console.WriteLine("Converted Key: " + aes.ByteArrayToString(convertedKey));

        Console.WriteLine("Encrypted: " + aes.ByteArrayToString(encrypted));
        Console.WriteLine("Decrypted: " + aes.ByteArrayToString(decrypted));

        Console.WriteLine("Decrypted Text: " + Encoding.UTF8.GetString(decrypted));
    }

    public static byte[] StringToByteArray(string hex)
    {
        return Enumerable.Range(0, hex.Length)
            .Where(x => x % 2 == 0)
            .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
            .ToArray();
    }
}