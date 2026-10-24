namespace PsdSharp;

public class BitFlags
{
	internal byte Flags;

	internal BitFlags(byte flags)
	{
		Flags = flags;
	}

	protected bool GetBit(int position)
	{
		return (Flags & (1 << position)) != 0;
	}

	protected void SetBit(int position, bool value)
	{
		Flags = (value ? ((byte)(Flags | (1 << position))) : ((byte)(Flags & ~(1 << position))));
	}
}
