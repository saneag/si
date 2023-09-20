using System.Text;

namespace aes;

public class AES
{
    public enum KeySize
    {
        Bits128,
        Bits192,
        Bits256
    }

    private int _nb;
    private int _nk;
    private int _nr;

    public AES(KeySize keySize)
    {
        SetNbNkNr(keySize);
    }

    private void SetNbNkNr(KeySize keySize)
    {
        _nb = 4;

        if (keySize == KeySize.Bits128)
        {
            _nk = 4;
            _nr = 10;
        }
        else if (keySize == KeySize.Bits192)
        {
            _nk = 6;
            _nr = 12;
        }
        else if (keySize == KeySize.Bits256)
        {
            _nk = 8;
            _nr = 14;
        }
    }

    public byte[] Encrypt(byte[] input, byte[] key)
    {
        var encrypt = new Encrypt(_nb, _nk, _nr);

        return encrypt.EncryptText(input, key);
    }

    public byte[] Decrypt(byte[] input, byte[] key)
    {
        var decrypt = new Decrypt(_nb, _nk, _nr);

        return decrypt.DecryptText(input, key);
    }

    public string ByteArrayToString(byte[] encrypted)
    {
        var sb = new StringBuilder(encrypted.Length * 2);

        foreach (var b in encrypted) sb.AppendFormat("{0:x2} ", b);

        return sb.ToString();
    }
}