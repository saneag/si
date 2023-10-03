using System.Numerics;
using System.Text;

namespace rsa;

public class Encrypt
{
    private List<string> _text = new List<string>();
    private int _p;
    private int _q;
    public int _n { get; private set; }
    private int _phi;
    private int _e;
    public int _d { get; private set; }

    public Encrypt(string input)
    {
        ConvertToBytes(input);
        InitializePrimeNumbers();
        CalculatePhi();
        CalculateE();
        CalculateD();

        // Console.Write("Text: ");
        // foreach (var c in _text)
        // {
        //     Console.Write(c + " ");
        // }
        //
        // Console.WriteLine($"p: {_p}");
        // Console.WriteLine($"q: {_q}");
        // Console.WriteLine($"n: {_n}");
        // Console.WriteLine($"phi: {_phi}");
        // Console.WriteLine($"e: {_e}");
        // Console.WriteLine($"d: {_d}");
    }

    private void ConvertToBytes(string input)
    {
        var splittedInput = input.Split("");
        foreach (var l in splittedInput)
        {
            var wordBytes = Encoding.ASCII.GetBytes(l);
            foreach (var b in wordBytes)
            {
                _text.Add(b.ToString());
            }
        }
    }

    private void InitializePrimeNumbers()
    {
        _p = PrimeNumbers.GetPrimeNumber();
        _q = PrimeNumbers.GetPrimeNumber();
        while (_p == _q)
        {
            _q = PrimeNumbers.GetPrimeNumber();
        }
        _n = _p * _q;
    }

    private void CalculatePhi() => _phi = (_p - 1) * (_q - 1);

    private void CalculateE()
    {
        var random = new Random();
        _e = random.Next(2, _phi);
        while (!PrimeNumbers.AreCoprime(_e, _phi))
        {
            _e = random.Next(2, _phi);
        }
    }

    private void CalculateD()
    {
        _d = PrimeNumbers.GetMultiplicativeInverse(_e, _phi);
    }
    
    public string GetEncryptedText()
    {
        var encryptedText = new StringBuilder();
        foreach (var t in _text)
        {
            if (int.TryParse(t, out int tInt))
            {
                var encrypted = BigInteger.ModPow(tInt, _e, _n);

                encryptedText.Append(encrypted);
                encryptedText.Append(' ');
            }
        }

        return encryptedText.ToString();
    }
}