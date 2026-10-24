using System;
using System.IO;
using PsdSharp.Parsing;

namespace PsdSharp.TaggedBlocks;

public class EffectsLayer : TaggedBlock
{
	public class CommonStateInfo
	{
		public bool IsVisible { get; set; } = true;

		public CommonStateInfo()
		{
		}

		internal CommonStateInfo(BigEndianReader reader)
		{
			reader.Skip(8);
			IsVisible = reader.ReadByte() != 0;
			reader.Skip(2);
		}
	}

	public class ShadowInfo
	{
		public int BlurValuePixels { get; set; }

		public int IntensityPercent { get; set; }

		public int AngleDegrees { get; set; }

		public int DistancePixels { get; set; }

		public PsdColor PsdColor { get; set; }

		public (string Signature, string Key) BlendMode { get; set; }

		public bool IsEnabled { get; set; }

		public bool UseAngleInAllLayerEffects { get; set; }

		public double Opacity { get; set; }

		public PsdColor? NativeColor { get; set; }

		public ShadowInfo()
		{
		}

		internal ShadowInfo(BigEndianReader reader)
		{
			reader.Skip(4);
			int num = reader.ReadInt32();
			BlurValuePixels = reader.ReadInt32();
			IntensityPercent = reader.ReadInt32();
			AngleDegrees = reader.ReadInt32();
			DistancePixels = reader.ReadInt32();
			PsdColor = PsdColor.FromPhotoshopData(reader);
			BlendMode = (Signature: reader.ReadSignature(), Key: reader.ReadFixedLengthString(4));
			IsEnabled = reader.ReadByte() != 0;
			UseAngleInAllLayerEffects = reader.ReadByte() != 0;
			Opacity = Convert.ToDouble(reader.ReadByte()) / 255.0;
			if (num > 0)
			{
				NativeColor = PsdColor.FromPhotoshopData(reader);
			}
		}
	}

	public class OuterGlowInfo
	{
		public int BlurValuePixels { get; set; }

		public int IntensityPercent { get; set; }

		public PsdColor PsdColor { get; set; }

		public (string Signature, string Key) BlendMode { get; set; }

		public bool IsEnabled { get; set; }

		public double Opacity { get; set; }

		public PsdColor? NativeColor { get; set; }

		public OuterGlowInfo()
		{
		}

		internal OuterGlowInfo(BigEndianReader reader)
		{
			reader.Skip(4);
			int num = reader.ReadInt32();
			BlurValuePixels = reader.ReadInt32();
			IntensityPercent = reader.ReadInt32();
			PsdColor = PsdColor.FromPhotoshopData(reader);
			BlendMode = (Signature: reader.ReadSignature(), Key: reader.ReadFixedLengthString(4));
			IsEnabled = reader.ReadByte() != 0;
			Opacity = Convert.ToDouble(reader.ReadByte()) / 255.0;
			if (num > 0)
			{
				NativeColor = PsdColor.FromPhotoshopData(reader);
			}
		}
	}

	public class InnerGlowInfo : OuterGlowInfo
	{
		public bool? Invert { get; set; }

		public InnerGlowInfo()
		{
		}

		internal InnerGlowInfo(BigEndianReader reader)
		{
			reader.Skip(4);
			int num = reader.ReadInt32();
			base.BlurValuePixels = reader.ReadInt32();
			base.IntensityPercent = reader.ReadInt32();
			base.PsdColor = PsdColor.FromPhotoshopData(reader);
			base.BlendMode = (Signature: reader.ReadSignature(), Key: reader.ReadFixedLengthString(4));
			base.IsEnabled = reader.ReadByte() != 0;
			base.Opacity = Convert.ToDouble(reader.ReadByte()) / 255.0;
			if (num > 0)
			{
				Invert = reader.ReadByte() != 0;
				base.NativeColor = PsdColor.FromPhotoshopData(reader);
			}
		}
	}

	public class BevelInfo
	{
		public enum DirectionEnum : byte
		{
			Up,
			Down
		}

		public int AngleDegrees { get; set; }

		public int DepthInPixels { get; set; }

		public int BlurValuePixels { get; set; }

		public (string Signature, string Key) HighlightBlendMode { get; set; }

		public (string Signature, string Key) ShadowBlendMode { get; set; }

		public PsdColor HighlightPsdColor { get; set; }

		public PsdColor ShadowPsdColor { get; set; }

		public byte BevelStyle { get; set; }

		public double HighlightOpacity { get; set; }

		public double ShadowOpacity { get; set; }

		public bool IsEnabled { get; set; }

		public bool UseAngleInAllLayerEffects { get; set; }

		public DirectionEnum Direction { get; set; }

		public PsdColor? RealHighlightColor { get; set; }

		public PsdColor? RealShadowColor { get; set; }

		public BevelInfo()
		{
		}

		internal BevelInfo(BigEndianReader reader)
		{
			reader.Skip(4);
			int num = reader.ReadInt32();
			AngleDegrees = reader.ReadInt32();
			DepthInPixels = reader.ReadInt32();
			BlurValuePixels = reader.ReadInt32();
			HighlightBlendMode = (Signature: reader.ReadSignature(), Key: reader.ReadFixedLengthString(4));
			ShadowBlendMode = (Signature: reader.ReadSignature(), Key: reader.ReadFixedLengthString(4));
			HighlightPsdColor = PsdColor.FromPhotoshopData(reader);
			ShadowPsdColor = PsdColor.FromPhotoshopData(reader);
			BevelStyle = reader.ReadByte();
			HighlightOpacity = Convert.ToDouble(reader.ReadByte()) / 255.0;
			ShadowOpacity = Convert.ToDouble(reader.ReadByte()) / 255.0;
			IsEnabled = reader.ReadByte() != 0;
			UseAngleInAllLayerEffects = reader.ReadByte() != 0;
			Direction = (DirectionEnum)reader.ReadByte();
			if (num > 0)
			{
				RealHighlightColor = PsdColor.FromPhotoshopData(reader);
				RealShadowColor = PsdColor.FromPhotoshopData(reader);
			}
		}
	}

	public class SolidFillInfo
	{
		public string BlendModeKey { get; set; } = string.Empty;

		public PsdColor PsdColor { get; set; }

		public byte Opaciy { get; set; }

		public bool IsEnabled { get; set; }

		public PsdColor NativePsdColor { get; set; }

		public SolidFillInfo()
		{
		}

		internal SolidFillInfo(BigEndianReader reader)
		{
			reader.Skip(8);
			BlendModeKey = reader.ReadFixedLengthString(4);
			PsdColor = PsdColor.FromPhotoshopData(reader);
			Opaciy = reader.ReadByte();
			IsEnabled = reader.ReadByte() != 0;
			NativePsdColor = PsdColor.FromPhotoshopData(reader);
		}
	}

	public short Version { get; }

	public CommonStateInfo CommonState { get; set; } = new CommonStateInfo();

	public ShadowInfo DropShadow { get; set; } = new ShadowInfo();

	public ShadowInfo InnerShadow { get; set; } = new ShadowInfo();

	public OuterGlowInfo OuterGlow { get; set; } = new OuterGlowInfo();

	public InnerGlowInfo InnerGlow { get; set; } = new InnerGlowInfo();

	public BevelInfo Bevel { get; set; } = new BevelInfo();

	public SolidFillInfo SolidFill { get; set; } = new SolidFillInfo();

	public EffectsLayer(byte[] rawData)
		: base(AdditionalLayerInfoKey.EffectsLayer, rawData)
	{
		BigEndianReader bigEndianReader = new BigEndianReader(new MemoryStream(rawData));
		Version = bigEndianReader.ReadInt16();
		short num = bigEndianReader.ReadInt16();
		for (int i = 0; i < num; i++)
		{
			string text = bigEndianReader.ReadSignature();
			if (text != "8BIM")
			{
				throw new InvalidDataException("Invalid effects layer signature: " + text);
			}
			switch (bigEndianReader.ReadFixedLengthString(4))
			{
			case "cmnS":
				CommonState = new CommonStateInfo(bigEndianReader);
				break;
			case "dsdw":
				DropShadow = new ShadowInfo(bigEndianReader);
				break;
			case "isdw":
				InnerShadow = new ShadowInfo(bigEndianReader);
				break;
			case "oglw":
				OuterGlow = new OuterGlowInfo(bigEndianReader);
				break;
			case "iglw":
				InnerGlow = new InnerGlowInfo(bigEndianReader);
				break;
			case "bevl":
				Bevel = new BevelInfo(bigEndianReader);
				break;
			case "sofi":
				SolidFill = new SolidFillInfo(bigEndianReader);
				break;
			}
		}
	}
}
