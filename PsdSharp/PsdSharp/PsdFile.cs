using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PsdSharp.Images;
using PsdSharp.Parsing;
using PsdSharp.TaggedBlocks;

namespace PsdSharp;

public class PsdFile
{
	public PsdHeader Header { get; private set; }

	public IReadOnlyCollection<Layer> Layers { get; internal set; } = (IReadOnlyCollection<Layer>)(object)Array.Empty<Layer>();

	public IReadOnlyCollection<Layer> RootLayers { get; internal set; } = (IReadOnlyCollection<Layer>)(object)Array.Empty<Layer>();

	public GlobalLayerMaskInfo? GlobalLayerMaskInfo { get; internal set; } = new GlobalLayerMaskInfo();

	public TaggedBlocksCollection? TaggedBlocks { get; internal set; } = new TaggedBlocksCollection(new List<TaggedBlock>());

	public IReadOnlyCollection<ImageResource> ImageResources { get; internal set; } = (IReadOnlyCollection<ImageResource>)(object)Array.Empty<ImageResource>();

	public ImageData? ImageData { get; internal set; }

	public IReadOnlyList<string> Warnings { get; internal set; } = Array.Empty<string>();

	internal PsdFile(PsdHeader header)
	{
		Header = header;
	}

	public PsdFile(PsdFileType fileType, ushort numberOfChannels, uint heightInPixels, uint widthInPixels, byte numBitsPerChannel, ColorMode colorMode)
	{
		Header = new PsdHeader
		{
			PsdFileType = fileType,
			NumberOfChannels = numberOfChannels,
			HeightInPixels = heightInPixels,
			WidthInPixels = widthInPixels,
			ChannelDepth = numBitsPerChannel,
			ColorMode = colorMode
		};
	}

	public static PsdFile Open(Stream stream, PsdLoadOptions? options = null)
	{
		if (options == null)
		{
			options = new PsdLoadOptions();
		}
		if (options.StringEncoding == null)
		{
			options.StringEncoding = Encoding.Latin1;
		}
		PsdFile result = new PsdParser(new ParseContext(new BigEndianReader(stream, options.StringEncoding, options.LeaveInputOpen), options)).Parse();
		if (!options.LeaveInputOpen)
		{
			stream.Close();
		}
		return result;
	}
}
