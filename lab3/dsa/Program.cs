using System.Numerics;

DSAImplementation dsa = new DSAImplementation();
dsa.GenerateKeys();
string message = "Hello, World!";
BigInteger[] signature = dsa.Sign(message);
bool isVerified = dsa.Verify(message, signature);
Console.WriteLine($"Signature Verified: {isVerified}");