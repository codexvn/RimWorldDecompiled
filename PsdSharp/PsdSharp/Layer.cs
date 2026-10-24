using System.Collections.Generic;
using PsdSharp.Images;
using PsdSharp.TaggedBlocks;

namespace PsdSharp;

public class Layer
{
	private readonly List<Layer> _children = new List<Layer>();

	internal LayerFlags Flags { get; set; } = new LayerFlags();

	public Rectangle Bounds { get; set; }

	public BlendModeKey BlendMode { get; set; }

	public byte Opacity { get; set; }

	public bool Clipping { get; set; }

	public bool TransparencyProtected
	{
		get
		{
			return Flags.TransparencyProtected;
		}
		set
		{
			Flags.TransparencyProtected = value;
		}
	}

	public bool IsVisible
	{
		get
		{
			return Flags.IsVisible;
		}
		set
		{
			Flags.IsVisible = value;
		}
	}

	public bool ShouldBeVisible
	{
		get
		{
			if (Parent == null)
			{
				return Flags.IsVisible;
			}
			if (Parent.ShouldBeVisible)
			{
				return Flags.IsVisible;
			}
			return false;
		}
	}

	public bool PixelDataIrrelevantToAppearanceOfDocument
	{
		get
		{
			return Flags.PixelDataIrrelevantToAppearanceOfDocument;
		}
		set
		{
			Flags.PixelDataIrrelevantToAppearanceOfDocument = value;
		}
	}

	public LayerMaskData? LayerMaskData { get; set; }

	public LayerBlendingRangesData? BlendingRangesData { get; set; }

	public string Name { get; set; } = string.Empty;

	public TaggedBlocksCollection TaggedBlocks { get; set; } = new TaggedBlocksCollection(new List<TaggedBlock>());

	public ImageData? ImageData { get; set; }

	public Layer? Parent { get; internal set; }

	public IReadOnlyCollection<Layer> Children => _children.AsReadOnly();

	internal void AddChild(Layer layer)
	{
		_children.Add(layer);
	}
}
