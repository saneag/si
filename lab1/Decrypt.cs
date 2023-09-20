namespace aes;

public class Decrypt
{
    private readonly int _nb;
    private readonly int _nk;
    private readonly int _nr;

    public Decrypt(int nb, int nk, int nr)
    {
        _nb = nb;
        _nk = nk;
        _nr = nr;

        KeyExpansion._nb = _nb;
        KeyExpansion._nk = _nk;
        KeyExpansion._nr = _nr;
    }

    public byte[,] InvSubBytes(byte[,] state)
    {
        var newState = new byte[4, 4];

        for (var row = 0; row < 4; row++)
        {
            for (var col = 0; col < _nb; col++)
            {
                var hex = state[row, col];
                var first = hex >> 4;
                var second = hex & 0x0F;
                newState[row, col] = Constants.InvSBox[first, second];
            }
        }

        return newState;
    }

    public byte[,] InvShiftRows(byte[,] state)
    {
        var newState = new byte[4, 4];

        for (var row = 0; row < 4; row++)
        for (var col = 0; col < _nb; col++)
            newState[row, (col + row) % _nb] = state[row, col];

        return newState;
    }

    public byte[,] InvMixColumns(byte[,] state)
    {
        var newState = new byte[4, 4];

        for (var col = 0; col < _nb; col++)
        {
            newState[0, col] = (byte)(gfmultby0e(state[0, col]) ^ gfmultby0b(state[1, col]) ^
                                      gfmultby0d(state[2, col]) ^ gfmultby09(state[3, col]));
            newState[1, col] = (byte)(gfmultby09(state[0, col]) ^ gfmultby0e(state[1, col]) ^
                                      gfmultby0b(state[2, col]) ^ gfmultby0d(state[3, col]));
            newState[2, col] = (byte)(gfmultby0d(state[0, col]) ^ gfmultby09(state[1, col]) ^
                                      gfmultby0e(state[2, col]) ^ gfmultby0b(state[3, col]));
            newState[3, col] = (byte)(gfmultby0b(state[0, col]) ^ gfmultby0d(state[1, col]) ^
                                      gfmultby09(state[2, col]) ^ gfmultby0e(state[3, col]));
        }

        return newState;
    }

    private byte gfmultby0e(byte b)
    {
        return (byte)(gfmultby02(gfmultby02(gfmultby02(b))) ^
                      gfmultby02(gfmultby02(b)) ^
                      gfmultby02(b));
    }

    private byte gfmultby0b(byte b)
    {
        return (byte)(gfmultby02(gfmultby02(gfmultby02(b))) ^
                      gfmultby02(b) ^
                      b);
    }

    private byte gfmultby0d(byte b)
    {
        return (byte)(gfmultby02(gfmultby02(gfmultby02(b))) ^
                      gfmultby02(gfmultby02(b)) ^
                      b);
    }

    private byte gfmultby09(byte b)
    {
        return (byte)((int)gfmultby02(gfmultby02(gfmultby02(b))) ^
                      (int)b);
    }

    private byte gfmultby02(byte b)
    {
        if (b < 0x80)
            return (byte)(b << 1);
        return (byte)((b << 1) ^ 0x1b);
    }

    public byte[,] InvAddRoundKey(byte[,] state, byte[,] roundKey)
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
    
    public byte[] DecryptText(byte[] cipherText, byte[] key)
    {
        var roundKeys = KeyExpansion.KeyExp(key);
        // HelpCode.ShowByteArray(roundKeys);
        
        var blockSize = 16;
        var numBlocks = (cipherText.Length + blockSize - 1) / blockSize;
        var paddedLength = numBlocks * blockSize;
        var paddedPlainText = new byte[paddedLength];
        
        Array.Copy(cipherText, paddedPlainText, cipherText.Length);
        
        var plainText = new byte[paddedLength];
        
        for (var block = 0; block < numBlocks; block++)
        {
            var state = new byte[4, 4];
            
            for (var i = 0; i < 4; i++)
            for (var j = 0; j < 4; j++)
                state[j, i] = paddedPlainText[block * blockSize + i * 4 + j];
            
            // HelpCode.Init(state);
            
            state = InvAddRoundKey(state, KeyExpansion.SubKey(roundKeys, _nr * blockSize));
            
            // HelpCode.Mid(state);
            
            for (var round = _nr - 1; round >= 0; round--)
            {
                state = InvShiftRows(state);
                state = InvSubBytes(state);
                state = InvAddRoundKey(state, KeyExpansion.SubKey(roundKeys, round * blockSize));
                
                if (round > 0) state = InvMixColumns(state);
                
                // HelpCode.Final(state, round);
            }
            
            for (var i = 0; i < 4; i++)
            for (var j = 0; j < 4; j++)
                plainText[block * blockSize + i * 4 + j] = state[j, i];
        }
        
        return plainText;
    }
}