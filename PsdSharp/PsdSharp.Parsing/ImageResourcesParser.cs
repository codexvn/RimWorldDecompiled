using System;
using System.Collections.Generic;
using PsdSharp.ImageResources;

namespace PsdSharp.Parsing;

internal static class ImageResourcesParser
{
	public static List<ImageResource> Parse(ParseContext ctx)
	{
		uint num = ctx.Reader.ReadUInt32();
		List<ImageResource> list = new List<ImageResource>();
		(string, ushort) tuple;
		byte[] array;
		for (uint num2 = 0u; num2 < num; num2 += (uint)(6 + tuple.Item2 + 4 + array.Length))
		{
			if (ctx.Reader.ReadSignature() != "8BIM")
			{
				throw ParserUtils.DataCorrupt();
			}
			ImageResourceId imageResourceId = (ImageResourceId)ctx.Reader.ReadUInt16();
			tuple = ctx.Reader.ReadPascalString(2);
			uint num3 = ctx.Reader.ReadUInt32();
			if (num3 % 2 == 1)
			{
				num3++;
			}
			array = ((num3 == 0) ? Array.Empty<byte>() : new byte[num3]);
			ctx.Reader.ReadIntoBuffer(array);
			if (ImageResourceRegistry.ResourceTypes.TryGetValue(imageResourceId, out Type value))
			{
				ImageResource item = (ImageResource)Activator.CreateInstance(value, tuple.Item1, array);
				list.Add(item);
			}
			else
			{
				list.Add(new ImageResource
				{
					Id = imageResourceId,
					Name = tuple.Item1,
					RawData = array
				});
			}
		}
		return list;
	}
}
