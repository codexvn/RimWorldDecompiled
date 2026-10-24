using System;

namespace PsdSharp.Parsing;

internal static class ColorModeDataParser
{
	public static byte[] Parse(ParseContext ctx)
	{
		uint num = ctx.Reader.ReadUInt32();
		if (num == 0)
		{
			return Array.Empty<byte>();
		}
		byte[] array = new byte[num];
		ctx.Reader.ReadIntoBuffer(array);
		return array;
	}
}
