namespace aes;

public static class KeyExpansion
{
    public static int _nb { get; set; }
    public static int _nk { get; set; }
    public static int _nr { get; set; }

    private static byte[] SubWord(byte[] word)
    {
        var result = new byte[4];

        for (var i = 0; i < 4; i++)
        {
            var row = word[i] >> 4;
            var col = word[i] & 0x0f;
            result[i] = Constants.SBox[row, col];
        }

        return result;
    }

    private static byte[] RotWord(byte[] word)
    {
        var result = new byte[4];
        result[0] = word[1];
        result[1] = word[2];
        result[2] = word[3];
        result[3] = word[0];
        return result;
    }

    public static byte[,] SubKey(byte[] roundKeys, int roundKeyIndex)
    {
        var subKey = new byte[4, 4];

        for (var col = 0; col < _nb; col++)
        {
            subKey[0, col] = roundKeys[roundKeyIndex + col * 4 + 0];
            subKey[1, col] = roundKeys[roundKeyIndex + col * 4 + 1];
            subKey[2, col] = roundKeys[roundKeyIndex + col * 4 + 2];
            subKey[3, col] = roundKeys[roundKeyIndex + col * 4 + 3];
        }

        return subKey;
    }

    public static byte[] KeyExp(byte[] key)
    {
        var roundKeys = new byte[4 * _nb * (_nr + 1)];

        for (var i = 0; i < _nk; i++)
        {
            roundKeys[4 * i + 0] = key[4 * i + 0];
            roundKeys[4 * i + 1] = key[4 * i + 1];
            roundKeys[4 * i + 2] = key[4 * i + 2];
            roundKeys[4 * i + 3] = key[4 * i + 3];
        }

        for (var i = _nk; i < _nb * (_nr + 1); i++)
        {
            var temp = new byte[4];
            temp[0] = roundKeys[4 * (i - 1) + 0];
            temp[1] = roundKeys[4 * (i - 1) + 1];
            temp[2] = roundKeys[4 * (i - 1) + 2];
            temp[3] = roundKeys[4 * (i - 1) + 3];

            if (i % _nk == 0)
            {
                temp = SubWord(RotWord(temp));
                temp[0] ^= Constants.RCon[i / _nk];
            }
            else if (_nk > 6 && i % _nk == 4)
            {
                temp = SubWord(temp);
            }

            roundKeys[4 * i + 0] = (byte)(roundKeys[4 * (i - _nk) + 0] ^ temp[0]);
            roundKeys[4 * i + 1] = (byte)(roundKeys[4 * (i - _nk) + 1] ^ temp[1]);
            roundKeys[4 * i + 2] = (byte)(roundKeys[4 * (i - _nk) + 2] ^ temp[2]);
            roundKeys[4 * i + 3] = (byte)(roundKeys[4 * (i - _nk) + 3] ^ temp[3]);
        }

        return roundKeys;
    }
}