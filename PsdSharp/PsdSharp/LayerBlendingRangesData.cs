using System.Collections.Generic;

namespace PsdSharp;

public class LayerBlendingRangesData
{
	public uint CompositeGreyBlendSource { get; set; }

	public uint CompositeGreyBlendDestination { get; set; }

	public List<(uint Source, uint Destination)> ChannelBlendingRanges { get; set; } = new List<(uint, uint)>();
}
