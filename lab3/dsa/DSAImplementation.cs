using System.Numerics;
using System.Security.Cryptography;
using System.Text;

public class DSAImplementation
{
    private BigInteger p, q, g;
    private BigInteger privateKey;
    private BigInteger publicKey;

    public void GenerateKeys()
    {
        p = 23;
        q = 11;
        g = 2;

        privateKey = 6;

        publicKey = BigInteger.ModPow(g, privateKey, p);

        Console.WriteLine("{p, q, g} = {" + p + ", " + q + ", " + g + "}");
        Console.WriteLine("Private Key: " + privateKey);
        Console.WriteLine("Public Key: " + publicKey);
    }

    public BigInteger[] Sign(string message)
    {
        BigInteger hash = HashMessage(message);
        BigInteger k = 4;
        BigInteger r = BigInteger.ModPow(g, k, p) % q;
        BigInteger s = (BigInteger.ModPow(k, q - 2, q) * (hash + privateKey * r)) % q;

        return new BigInteger[] { r, s };
    }

    public bool Verify(string message, BigInteger[] signature)
    {
        BigInteger r = signature[0];
        BigInteger s = signature[1];

        if (r <= 0 || r >= q || s <= 0 || s >= q)
            return false;

        BigInteger hash = HashMessage(message);
        BigInteger w = BigInteger.ModPow(s, q - 2, q);
        BigInteger u1 = (hash * w) % q;
        BigInteger u2 = (r * w) % q;
        BigInteger v = ((BigInteger.ModPow(g, u1, p) * BigInteger.ModPow(publicKey, u2, p)) % p) % q;

        return v == r;
    }

    private static byte[] ComputeSHA256(string message)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            return sha256.ComputeHash(bytes);
        }
    }

    private static BigInteger HashMessage(string message)
    {
        byte[] hashedBytes = ComputeSHA256(message);
        return new BigInteger(hashedBytes);
    }
}