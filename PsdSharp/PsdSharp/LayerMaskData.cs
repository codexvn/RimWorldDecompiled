namespace PsdSharp;

public class LayerMaskData
{
	internal MaskFlags Flags { get; set; }

	internal MaskParametersFlags ParametersFlags { get; set; }

	internal MaskParameters Parameters { get; set; }

	public Rectangle Bounds { get; set; }

	public byte DefaultColor { get; set; }

	public bool PositionRelativeToLayer
	{
		get
		{
			return Flags.PositionRelativeToLayer;
		}
		set
		{
			Flags.PositionRelativeToLayer = value;
		}
	}

	public bool LayerMaskDisabled
	{
		get
		{
			return Flags.LayerMaskDisabled;
		}
		set
		{
			Flags.LayerMaskDisabled = value;
		}
	}

	public bool UserMaskCameFromRenderingOtherData
	{
		get
		{
			return Flags.UserMaskCameFromRenderingOtherData;
		}
		set
		{
			Flags.UserMaskCameFromRenderingOtherData = value;
		}
	}

	public byte? UserMaskDensity
	{
		get
		{
			if (!Flags.MaskParametersPresent || !ParametersFlags.UserMaskDensityPresent)
			{
				return null;
			}
			return Parameters.UserMaskDensity;
		}
		set
		{
			Parameters.UserMaskDensity = value;
			ParametersFlags.UserMaskDensityPresent = value.HasValue;
			Flags.MaskParametersPresent = !ParametersFlags.IsEmpty;
		}
	}

	public byte? VectorMaskDensity
	{
		get
		{
			if (!Flags.MaskParametersPresent || !ParametersFlags.VectorMaskDensityPresent)
			{
				return null;
			}
			return Parameters.VectorMaskDensity;
		}
		set
		{
			Parameters.VectorMaskDensity = value;
			ParametersFlags.VectorMaskDensityPresent = value.HasValue;
			Flags.MaskParametersPresent = !ParametersFlags.IsEmpty;
		}
	}

	public double? UserMaskFeather
	{
		get
		{
			if (!Flags.MaskParametersPresent || !ParametersFlags.UserMaskFeatherPresent)
			{
				return null;
			}
			return Parameters.UserMaskFeather;
		}
		set
		{
			Parameters.UserMaskFeather = value;
			ParametersFlags.UserMaskFeatherPresent = value.HasValue;
			Flags.MaskParametersPresent = !ParametersFlags.IsEmpty;
		}
	}

	public double? VectorMaskFeather
	{
		get
		{
			if (!Flags.MaskParametersPresent || !ParametersFlags.VectorMaskFeatherPresent)
			{
				return null;
			}
			return Parameters.VectorMaskFeather;
		}
		set
		{
			Parameters.VectorMaskFeather = value;
			ParametersFlags.VectorMaskFeatherPresent = value.HasValue;
			Flags.MaskParametersPresent = !ParametersFlags.IsEmpty;
		}
	}

	public MaskFlags? RealFlags { get; set; }

	public byte? RealDefaultColor { get; set; }

	public Rectangle? RealBounds { get; set; }

	internal LayerMaskData(MaskFlags flags, MaskParametersFlags parametersFlags, MaskParameters parameters)
	{
		Flags = flags;
		ParametersFlags = parametersFlags;
		Parameters = parameters;
	}

	public LayerMaskData()
	{
		Flags = new MaskFlags();
		ParametersFlags = new MaskParametersFlags();
		Parameters = new MaskParameters();
	}
}
