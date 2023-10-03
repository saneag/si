using rsa;

class Program
{
    public static void Main()
    {
        var input = "Text to be encrypted";
        
        var encrypt = new Encrypt(input);
        var encryptedText = encrypt.GetEncryptedText();
        
        var decrypt = new Decrypt(encryptedText, encrypt._d, encrypt._n);
        var decryptedText = decrypt.GetDecryptedText();
        
        Console.WriteLine($"Encrypted text: {encryptedText}");
        Console.WriteLine($"Decrypted text: {decryptedText}");
    }
}