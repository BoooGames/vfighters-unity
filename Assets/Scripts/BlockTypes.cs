using System.Text;

public enum BlockType
{
    Empty,
    A,
    B,
    C,
    D,
    X
}


public static class BlockDataHelper
{
    public static int Encode(BlockType blockType, bool isBomb, int blockTimer)
    {
        int encodedValue = 0;
        encodedValue |= (int)blockType & 0xF;
        encodedValue |= (isBomb ? 1 : 0) << 4;
        encodedValue |= (blockTimer & 0xF) << 5;
        return encodedValue;
    }

    public static void Decode(int encodedValue, out BlockType blockType, out bool isBomb, out int blockTimer)
    {
        blockType = (BlockType) (encodedValue & 0xF);
        isBomb = ((encodedValue >> 4) & 1) == 1;
        blockTimer = (encodedValue >> 5) & 0xF;
    }
}

public static class GridCellExtensions
{
    public static bool DecodeIsBomb(this int value)
    {
        return ((value >> 4) & 1) == 1;
    }

    public static int DecodeTimer(this int value)
    {
        return (value >> 5) & 0xF;
    }

    public static BlockType DecodeCellType(this int value)
    {
        return (BlockType) (value & 0xF);
    }
}