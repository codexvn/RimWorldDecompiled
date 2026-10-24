using System;

namespace PsdSharp.Parsing;

internal static class HeaderParser
{
	public static PsdHeader Parse(ParseContext ctx)
	{
		if (ctx.Reader.ReadSignature() != "8BPS")
		{
			throw ParserUtils.DataCorrupt();
		}
		PsdFileType psdFileType = ((ctx.Reader.ReadUInt16() != 2) ? PsdFileType.Psd : PsdFileType.Psb);
		ctx.InitPsdFileType(psdFileType);
		ctx.Reader.Skip(6);
		ushort numberOfChannels = ctx.Reader.ReadUInt16();
		uint num = ctx.Reader.ReadUInt32();
		if (num > 300000 || (ctx.Traits.PsdFileType == PsdFileType.Psd && num > 30000))
		{
			throw ParserUtils.DataCorrupt();
		}
		uint num2 = ctx.Reader.ReadUInt32();
		if (num2 > 300000 || (ctx.Traits.PsdFileType == PsdFileType.Psd && num2 > 30000))
		{
			throw ParserUtils.DataCorrupt();
		}
		ushort num3 = ctx.Reader.ReadUInt16();
		if (num3 != 1 && num3 != 8 && num3 != 16 && num3 != 32)
		{
			throw ParserUtils.DataCorrupt();
		}
		ushort num4 = ctx.Reader.ReadUInt16();
		if (!Enum.IsDefined(typeof(ColorMode), num4))
		{
			throw ParserUtils.DataCorrupt();
		}
		ctx.InitColorMode((ColorMode)num4);
		return new PsdHeader
		{
			PsdFileType = psdFileType,
			NumberOfChannels = numberOfChannels,
			HeightInPixels = num,
			WidthInPixels = num2,
			ChannelDepth = checked((byte)num3),
			ColorMode = (ColorMode)num4,
			ColorModeData = ColorModeDataParser.Parse(ctx)
		};
	}
}
