using System;
using System.Numerics;
using System.Text;

public class Decrypt
{
    private string _encryptedText;
    private int _d;
    private int _n;

    public Decrypt(string encryptedText, int d, int n)
    {
        _encryptedText = encryptedText;
        _d = d;
        _n = n;
    }

    public string GetDecryptedText()
    {
        var decryptedText = new StringBuilder();
        var splittedEncryptedText = _encryptedText.Trim().Split(" ");

        foreach (var c in splittedEncryptedText)
        {
            var encryptedChar = int.Parse(c);
            var decryptedChar = BigInteger.ModPow(encryptedChar, _d, _n);
            decryptedText.Append((char)decryptedChar);
        }

        return decryptedText.ToString();
    }
}