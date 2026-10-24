using System;
using System.Collections.Generic;
using System.Linq;
using PsdSharp.Images;
using PsdSharp.TaggedBlocks;

namespace PsdSharp.Parsing;

internal static class LayerAndMaskInformationParser
{
	public static (List<Layer> Layers, GlobalLayerMaskInfo GlobalLayerMaskInfo, TaggedBlocksCollection TaggedBlocks) Parse(ParseContext ctx, PsdHeader header)
	{
		ulong num = ctx.Traits.ReadLenN();
		long num2 = ctx.Reader.Position + (long)num;
		if (num == 0L)
		{
			return (Layers: new List<Layer>(), GlobalLayerMaskInfo: new GlobalLayerMaskInfo(), TaggedBlocks: new TaggedBlocksCollection(new List<TaggedBlock>()));
		}
		List<Layer> item = ParseLayerInfo(ctx, header);
		GlobalLayerMaskInfo item2 = ((ctx.Reader.Position >= num2) ? new GlobalLayerMaskInfo() : ParseGlobalLayerMaskInfo(ctx));
		TaggedBlocksCollection item3 = ((ctx.Reader.Position >= num2) ? new TaggedBlocksCollection(new List<TaggedBlock>()) : ParseTaggedBlocks(ctx, num2, 4));
		if (ctx.Reader.Position < num2)
		{
			ctx.Reader.Skip((int)(num2 - ctx.Reader.Position));
		}
		return (Layers: item, GlobalLayerMaskInfo: item2, TaggedBlocks: item3);
	}

	private static List<Layer> ParseLayerInfo(ParseContext ctx, PsdHeader header)
	{
		ulong num = ctx.Traits.ReadLenN();
		long num2 = ctx.Reader.Position + (long)num;
		if (num == 0L)
		{
			return new List<Layer>();
		}
		short num3 = Math.Abs(ctx.Reader.ReadInt16());
		Dictionary<uint, Layer> dictionary = new Dictionary<uint, Layer>();
		Dictionary<uint, (short, long)[]> dictionary2 = new Dictionary<uint, (short, long)[]>();
		for (uint num4 = 0u; num4 < num3; num4++)
		{
			var (value, value2) = ParseLayerRecord(ctx);
			dictionary.Add(num4, value);
			dictionary2.Add(num4, value2);
		}
		for (uint num5 = 0u; num5 < num3; num5++)
		{
			Layer layer = dictionary[num5];
			(short, long)[] channels = dictionary2[num5];
			layer.ImageData = new PerChannelImageData(ctx, header, layer.Bounds, channels);
		}
		if (ctx.Reader.Position < num2)
		{
			long num6 = num2 - ctx.Reader.Position;
			ctx.Reader.Skip((int)num6);
		}
		return dictionary.Values.ToList();
	}

	private static (Layer, (short Channelid, long DataLength)[]) ParseLayerRecord(ParseContext ctx)
	{
		int y = ctx.Reader.ReadInt32();
		int x = ctx.Reader.ReadInt32();
		int num = ctx.Reader.ReadInt32();
		int num2 = ctx.Reader.ReadInt32();
		ushort num3 = ctx.Reader.ReadUInt16();
		Dictionary<uint, (short, long)> dictionary = new Dictionary<uint, (short, long)>();
		for (uint num4 = 0u; num4 < num3; num4++)
		{
			short item = ctx.Reader.ReadInt16();
			ulong item2 = ctx.Traits.ReadLenN();
			dictionary[num4] = (item, (long)item2);
		}
		if (ctx.Reader.ReadSignature() != "8BIM")
		{
			throw ParserUtils.DataCorrupt();
		}
		string key = ctx.Reader.ReadSignature();
		if (!BlendModeKey.ByKey.TryGetValue(key, out BlendModeKey value))
		{
			throw ParserUtils.DataCorrupt();
		}
		BlendModeKey blendMode = value;
		byte opacity = ctx.Reader.ReadByte();
		bool clipping = ctx.Reader.ReadByte() > 0;
		LayerFlags flags = new LayerFlags(ctx.Reader.ReadByte());
		ctx.Reader.ReadByte();
		Layer layer = new Layer
		{
			Bounds = new Rectangle(new Point(x, y), new Point(num2 - 1, num - 1)),
			BlendMode = blendMode,
			Opacity = opacity,
			Clipping = clipping,
			Flags = flags
		};
		ParseExtraLayerData(ctx, ref layer);
		return (layer, dictionary.Values.ToArray());
	}

	private static void ParseExtraLayerData(ParseContext ctx, ref Layer layer)
	{
		uint num = ctx.Reader.ReadUInt32();
		long num2 = ctx.Reader.Position + num;
		if (num != 0)
		{
			ParseMaskData(ctx, ref layer);
			ParseBlendingRanges(ctx, ref layer);
			layer.Name = ctx.Reader.ReadPascalString(4).String;
			layer.TaggedBlocks = ParseTaggedBlocks(ctx, num2, 4);
			if (ctx.Reader.Position < num2)
			{
				ctx.Reader.Skip((int)(num2 - ctx.Reader.Position));
			}
		}
	}

	private static void ParseMaskData(ParseContext ctx, ref Layer layer)
	{
		uint num = ctx.Reader.ReadUInt32();
		long num2 = ctx.Reader.Position + num;
		if (num != 0)
		{
			int y = ctx.Reader.ReadInt32();
			int x = ctx.Reader.ReadInt32();
			int y2 = ctx.Reader.ReadInt32();
			int x2 = ctx.Reader.ReadInt32();
			byte defaultColor = ctx.Reader.ReadByte();
			MaskFlags maskFlags = new MaskFlags(ctx.Reader.ReadByte());
			MaskFlags realFlags = ((num >= 36) ? new MaskFlags(ctx.Reader.ReadByte()) : null);
			byte? realDefaultColor = ((num >= 36) ? new byte?(ctx.Reader.ReadByte()) : ((byte?)null));
			int? num3 = ((num >= 36) ? new int?(ctx.Reader.ReadInt32()) : ((int?)null));
			int? num4 = ((num >= 36) ? new int?(ctx.Reader.ReadInt32()) : ((int?)null));
			int? num5 = ((num >= 36) ? new int?(ctx.Reader.ReadInt32()) : ((int?)null));
			int? num6 = ((num >= 36) ? new int?(ctx.Reader.ReadInt32()) : ((int?)null));
			MaskParameters maskParameters = new MaskParameters();
			MaskParametersFlags maskParametersFlags = (maskFlags.MaskParametersPresent ? new MaskParametersFlags(ctx.Reader.ReadByte()) : new MaskParametersFlags());
			if (maskFlags.MaskParametersPresent && maskParametersFlags.UserMaskDensityPresent)
			{
				maskParameters.UserMaskDensity = ctx.Reader.ReadByte();
			}
			if (maskFlags.MaskParametersPresent && maskParametersFlags.UserMaskFeatherPresent)
			{
				maskParameters.UserMaskFeather = ctx.Reader.ReadDouble();
			}
			if (maskFlags.MaskParametersPresent && maskParametersFlags.VectorMaskDensityPresent)
			{
				maskParameters.VectorMaskDensity = ctx.Reader.ReadByte();
			}
			if (maskFlags.MaskParametersPresent && maskParametersFlags.VectorMaskFeatherPresent)
			{
				maskParameters.VectorMaskFeather = ctx.Reader.ReadDouble();
			}
			if (ctx.Reader.Position < num2)
			{
				ctx.Reader.Skip((int)(num2 - ctx.Reader.Position));
			}
			layer.LayerMaskData = new LayerMaskData(maskFlags, maskParametersFlags, maskParameters)
			{
				Bounds = new Rectangle(new Point(x, y), new Point(x2, y2)),
				DefaultColor = defaultColor,
				Flags = maskFlags,
				RealFlags = realFlags,
				RealDefaultColor = realDefaultColor,
				RealBounds = (num3.HasValue ? new Rectangle?(new Rectangle(new Point(num4.Value, num3.Value), new Point(num6.Value, num5.Value))) : ((Rectangle?)null))
			};
		}
	}

	private static void ParseBlendingRanges(ParseContext ctx, ref Layer layer)
	{
		uint num = ctx.Reader.ReadUInt32();
		long num2 = ctx.Reader.Position + num;
		if (num >= 8)
		{
			LayerBlendingRangesData layerBlendingRangesData = new LayerBlendingRangesData
			{
				CompositeGreyBlendSource = ctx.Reader.ReadUInt32(),
				CompositeGreyBlendDestination = ctx.Reader.ReadUInt32()
			};
			for (uint num3 = 8u; num3 < num; num3 += 8)
			{
				uint item = ctx.Reader.ReadUInt32();
				uint item2 = ctx.Reader.ReadUInt32();
				layerBlendingRangesData.ChannelBlendingRanges.Add((item, item2));
			}
			if (ctx.Reader.Position < num2)
			{
				ctx.Reader.Skip((int)(num2 - ctx.Reader.Position));
			}
			layer.BlendingRangesData = layerBlendingRangesData;
		}
	}

	private static TaggedBlocksCollection ParseTaggedBlocks(ParseContext ctx, long endPos, byte padding = 1)
	{
		List<TaggedBlock> list = new List<TaggedBlock>();
		while (ctx.Reader.Position + 12 <= endPos)
		{
			string text = ctx.Reader.ReadSignature();
			if (text != "8BIM" && text != "8B64")
			{
				ctx.Reader.Backtrack(ctx.StringEncoding.GetBytes(text));
				return new TaggedBlocksCollection(list);
			}
			string text2 = ctx.Reader.ReadSignature();
			AdditionalLayerInfoKey value;
			AdditionalLayerInfoKey additionalLayerInfoKey = (AdditionalLayerInfoKey.ByKey.TryGetValue(text2, out value) ? value : new AdditionalLayerInfoKey(text2, text2, 4));
			ulong num = ((ctx.Traits.PsdFileType == PsdFileType.Psd) ? ctx.Reader.ReadUInt32() : ((additionalLayerInfoKey.PsbLengthCountSizeBytes == 4) ? ctx.Reader.ReadUInt32() : ctx.Reader.ReadUInt64()));
			byte[] array = new byte[num];
			ctx.Reader.ReadIntoBuffer(array);
			ulong num2 = num % padding;
			if (num2 != 0)
			{
				ctx.Reader.Skip((int)(padding - num2));
			}
			if (TaggedBlocksRegistry.TaggedBlockTypes.TryGetValue(additionalLayerInfoKey, out (Type, Func<AdditionalLayerInfoKey, byte[], TaggedBlock>) value2))
			{
				try
				{
					list.Add(value2.Item2(additionalLayerInfoKey, array));
				}
				catch (Exception ex)
				{
					ctx.Warnings.Add("Could not parse '" + text2 + "' tagged block, keeping it raw: " + ex.Message);
					goto IL_013c;
				}
				continue;
			}
			goto IL_013c;
			IL_013c:
			list.Add(new TaggedBlock(additionalLayerInfoKey, array));
		}
		return new TaggedBlocksCollection(list);
	}

	private static GlobalLayerMaskInfo ParseGlobalLayerMaskInfo(ParseContext ctx)
	{
		uint num = ctx.Reader.ReadUInt32();
		switch (num)
		{
		case 0u:
			return new GlobalLayerMaskInfo();
		case 1u:
		case 2u:
		case 3u:
		case 4u:
		case 5u:
		case 6u:
		case 7u:
		case 8u:
		case 9u:
		case 10u:
		case 11u:
		case 12u:
			throw ParserUtils.DataCorrupt();
		default:
		{
			GlobalLayerMaskInfo result = new GlobalLayerMaskInfo
			{
				OverlayColorSpace = ctx.Reader.ReadUInt16(),
				ColorComponent1 = ctx.Reader.ReadUInt16(),
				ColorComponent2 = ctx.Reader.ReadUInt16(),
				ColorComponent3 = ctx.Reader.ReadUInt16(),
				ColorComponent4 = ctx.Reader.ReadUInt16(),
				Opacity = ctx.Reader.ReadUInt16(),
				Kind = (GlobalLayerMaskInfo.KindEnum)ctx.Reader.ReadByte()
			};
			if (num > 13)
			{
				ctx.Reader.Skip((int)(num - 13));
			}
			return result;
		}
		}
	}
}
