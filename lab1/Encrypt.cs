namespace aes;

public class Encrypt
{
    private readonly int _nb;
    private readonly int _nk;
    private readonly int _nr;

    public Encrypt(int nb, int nk, int nr)
    {
        _nb = nb;
        _nk = nk;
        _nr = nr;

        KeyExpansion._nb = _nb;
        KeyExpansion._nk = _nk;
        KeyExpansion._nr = _nr;
    }

    public byte[,] SubBytes(byte[,] state)
    {
        var newState = new byte[4, 4];
        
        for (var row = 0; row < 4; row++)
        for (var col = 0; col < _nb; col++)
        {
            var r = state[row, col] >> 4;
            var c = state[row, col] & 0x0f;
            newState[row, col] = Constants.SBox[r, c];
        }

        return newState;
    }

    public byte[,] ShiftRows(byte[,] state)
    {
        var newState = new byte[4, 4];

        for (var row = 0; row < 4; row++)
        for (var col = 0; col < _nb; col++)
            newState[row, col] = state[row, (col + row) % _nb];

        return newState;
    }

    public byte[,] MixColumns(byte[,] state)
    {
        var newState = new byte[4, 4];

        for (var col = 0; col < _nb; col++)
        {
            newState[0, col] =
                (byte)(gfmultby02(state[0, col]) ^ gfmultby03(state[1, col]) ^ state[2, col] ^ state[3, col]);
            newState[1, col] =
                (byte)(state[0, col] ^ gfmultby02(state[1, col]) ^ gfmultby03(state[2, col]) ^ state[3, col]);
            newState[2, col] =
                (byte)(state[0, col] ^ state[1, col] ^ gfmultby02(state[2, col]) ^ gfmultby03(state[3, col]));
            newState[3, col] =
                (byte)(gfmultby03(state[0, col]) ^ state[1, col] ^ state[2, col] ^ gfmultby02(state[3, col]));
        }

        return newState;
    }

    private byte gfmultby02(byte b)
    {
        if (b < 0x80)
            return (byte)(b << 1);
        return (byte)((b << 1) ^ 0x1b);
    }

    private byte gfmultby03(byte b)
    {
        return (byte)(gfmultby02(b) ^ b);
    }

    public byte[,] AddRoundKey(byte[,] state, byte[,] roundKey)
    {
        var newState = new byte[4, 4];

        for (var col = 0; col < _nb; col++)
        {
            newState[0, col] = (byte)(state[0, col] ^ roundKey[0, col]);
            newState[1, col] = (byte)(state[1, col] ^ roundKey[1, col]);
            newState[2, col] = (byte)(state[2, col] ^ roundKey[2, col]);
            newState[3, col] = (byte)(state[3, col] ^ roundKey[3, col]);
        }

        return newState;
    }

    public byte[] EncryptText(byte[] input, byte[] key)
    {
        var roundKeys = KeyExpansion.KeyExp(key);
        // HelpCode.ShowByteArray(roundKeys);

        var blockSize = 16;
        var numBlocks = (input.Length + blockSize - 1) / blockSize;
        var paddedLength = numBlocks * blockSize;
        var paddedPlainText = new byte[paddedLength];

        Array.Copy(input, paddedPlainText, input.Length);

        var cipherText = new byte[paddedLength];

        for (var block = 0; block < numBlocks; block++)
        {
            var state = new byte[4, 4];

            for (var i = 0; i < 4; i++)
            for (var j = 0; j < 4; j++)
                state[j, i] = paddedPlainText[block * blockSize + i * 4 + j];
            
            // HelpCode.Init(state);
            
            state = AddRoundKey(state, KeyExpansion.SubKey(roundKeys, 0));
            
            // HelpCode.Mid(state);
            
            for (var round = 1; round <= _nr; round++)
            {
                state = SubBytes(state);
                state = ShiftRows(state);

                if (round < _nr) state = MixColumns(state);

                state = AddRoundKey(state, KeyExpansion.SubKey(roundKeys, round * blockSize));
                // HelpCode.Final(state, round);
            }

            for (var i = 0; i < 4; i++)
            for (var j = 0; j < 4; j++)
                cipherText[block * blockSize + i * 4 + j] = state[j, i];
        }

        Console.WriteLine();
        return cipherText;
    }
}