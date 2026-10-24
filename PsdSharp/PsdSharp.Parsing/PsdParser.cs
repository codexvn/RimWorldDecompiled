using System.Collections.Generic;
using PsdSharp.Images;
using PsdSharp.TaggedBlocks;

namespace PsdSharp.Parsing;

internal class PsdParser(ParseContext context)
{
	public PsdFile Parse()
	{
		PsdHeader header = HeaderParser.Parse(context);
		List<ImageResource> imageResources = ImageResourcesParser.Parse(context);
		(List<Layer>, GlobalLayerMaskInfo, TaggedBlocksCollection) tuple = LayerAndMaskInformationParser.Parse(context, header);
		List<Layer> rootLayers = ParseLayerTree(tuple.Item1);
		return new PsdFile(header)
		{
			ImageResources = imageResources,
			Layers = tuple.Item1.AsReadOnly(),
			RootLayers = rootLayers,
			GlobalLayerMaskInfo = tuple.Item2,
			TaggedBlocks = tuple.Item3,
			ImageData = new CompositeImageData(context, header),
			Warnings = context.Warnings
		};
	}

	private List<Layer> ParseLayerTree(List<Layer> layers)
	{
		List<Layer> list = new List<Layer>();
		Layer layer = null;
		foreach (Layer layer2 in layers)
		{
			if (layer2.Bounds.Height != 0 && layer != null)
			{
				layer.AddChild(layer2);
			}
			else if (layer2.Bounds.Height != 0 && layer == null)
			{
				list.Add(layer2);
			}
			else if (layer2.Name == "</Layer group>")
			{
				layer2.Parent = layer;
				layer = layer2;
			}
			else
			{
				if (layer == null)
				{
					continue;
				}
				layer2.Parent = layer.Parent;
				foreach (Layer child in layer.Children)
				{
					layer2.AddChild(child);
				}
				if (layer.Parent == null)
				{
					list.Add(layer2);
				}
				else
				{
					layer.Parent.AddChild(layer2);
				}
				layer = layer.Parent;
			}
		}
		return list;
	}
}
