using System;

namespace PsdSharp;

public class GlobalLayerMaskInfo
{
	public enum KindEnum : byte
	{
		ColorSelected = 0,
		ColorProtected = 1,
		UseValueStoredPerlayer = 0x80
	}

	private ushort _opacity;

	public ushort OverlayColorSpace { get; set; }

	public ushort ColorComponent1 { get; set; }

	public ushort ColorComponent2 { get; set; }

	public ushort ColorComponent3 { get; set; }

	public ushort ColorComponent4 { get; set; }

	public ushort Opacity
	{
		get
		{
			return _opacity;
		}
		set
		{
			if (value > 100)
			{
				throw new ArgumentOutOfRangeException("Opacity must be between 0 and 100");
			}
			_opacity = value;
		}
	}

	public KindEnum Kind { get; set; }
}
