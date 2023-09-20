namespace aes;

public static class HelpCode
{
    public static void ShowByteArray(byte[] encrypted)
    {
        var count = 0;
        foreach (var b in encrypted)
        {
            Console.Write("{0:x2} ", b);
            count++;
            if (count % 16 == 0) Console.Write("\n");
        }
    }

    public static void ShowByteMatrix(byte[,] matrix)
    {
        var count = 0;
        for (var row = 0; row < 4; row++)
        {
            for (int j = 0; j < 4; j++)
            {
                Console.Write("{0:x2} ", matrix[j, row]);
                count++;
                if (count % 16 == 0) Console.Write("\n");
            }
        }
    }

    public static void Init(byte[,] matrix)
    {
        Console.Write("Initial: ");
        ShowByteMatrix(matrix);
    }

    public static void Mid(byte[,] matrix)
    {
        Console.Write("Round 0: ");
        ShowByteMatrix(matrix);
    }

    public static void Final(byte[,] matrix, int round)
    {
        Console.Write("Round {0}: ", round);
        ShowByteMatrix(matrix);
    }
}