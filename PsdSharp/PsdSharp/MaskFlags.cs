namespace PsdSharp;

public class MaskFlags : BitFlags
{
	public bool PositionRelativeToLayer
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

	public bool LayerMaskDisabled
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

	public bool UserMaskCameFromRenderingOtherData
	{
		get
		{
			return GetBit(3);
		}
		set
		{
			SetBit(3, value);
		}
	}

	public bool MaskParametersPresent
	{
		get
		{
			return GetBit(4);
		}
		set
		{
			SetBit(4, value);
		}
	}

	internal MaskFlags(byte flags)
		: base(flags)
	{
	}

	public MaskFlags()
		: base(0)
	{
	}
}
