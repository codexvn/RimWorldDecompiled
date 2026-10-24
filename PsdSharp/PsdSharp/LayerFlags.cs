namespace PsdSharp;

public class LayerFlags : BitFlags
{
	public bool TransparencyProtected
	{
		get
		{
			return GetBit(0);
		}
		set
		{
			SetBit(0, value);
		}
	}

	public bool IsVisible
	{
		get
		{
			return GetBit(1);
		}
		set
		{
			SetBit(1, value);
		}
	}

	public bool PixelDataIrrelevantToAppearanceOfDocument
	{
		get
		{
			if (GetBit(3))
			{
				return GetBit(4);
			}
			return false;
		}
		set
		{
			SetBit(3, value: true);
			SetBit(4, value);
		}
	}

	internal LayerFlags(byte flags)
		: base(flags)
	{
	}

	internal LayerFlags()
		: base(0)
	{
	}
}
