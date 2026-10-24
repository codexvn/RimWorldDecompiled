namespace PsdSharp;

internal class MaskParametersFlags : BitFlags
{
	public bool UserMaskDensityPresent
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

	public bool UserMaskFeatherPresent
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

	public bool VectorMaskDensityPresent
	{
		get
		{
			return GetBit(2);
		}
		set
		{
			SetBit(2, value);
		}
	}

	public bool VectorMaskFeatherPresent
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

	public bool IsEmpty => Flags == 0;

	internal MaskParametersFlags(byte flags)
		: base(flags)
	{
	}

	internal MaskParametersFlags()
		: base(0)
	{
	}
}
