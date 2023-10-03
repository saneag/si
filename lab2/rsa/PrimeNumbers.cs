namespace rsa;

public static class PrimeNumbers
{
    public static int GetPrimeNumber()
    {
        var random = new Random();
        var primeNumber = random.Next(2, 10_000);
        while (!IsPrime(primeNumber))
        {
            primeNumber = random.Next(2, 10_000);
        }

        return primeNumber;
    }
    
    private static bool IsPrime(int number)
    {
        if (number <= 1) return false;
        if (number == 2) return true;
        if (number % 2 == 0) return false;

        var boundary = (int) Math.Floor(Math.Sqrt(number));

        for (var i = 3; i <= boundary; i += 2)
            if (number % i == 0)
                return false;

        return true;
    }

    public static bool AreCoprime(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        
        return a == 1;
    }
    
    public static int GetMultiplicativeInverse(int a, int b)
    {
        var b0 = b;
        var x0 = 0;
        var x1 = 1;

        if (b == 1)
        {
            return 1;
        }

        while (a > 1)
        {
            var q = a / b;
            var amb = a % b;
            a = b;
            b = amb;

            var xqx = x1 - q * x0;
            x1 = x0;
            x0 = xqx;
        }

        if (x1 < 0)
        {
            x1 += b0;
        }

        return x1;
    }
}