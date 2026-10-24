using System;
using System.Collections.Generic;
using System.Linq;

namespace PsdSharp.TaggedBlocks;

internal static class TaggedBlocksRegistry
{
	private static Dictionary<Type, AdditionalLayerInfoKey>? _keyByLayer;

	public static Dictionary<AdditionalLayerInfoKey, (Type Type, Func<AdditionalLayerInfoKey, byte[], TaggedBlock> Create)> TaggedBlockTypes { get; } = new Dictionary<AdditionalLayerInfoKey, (Type, Func<AdditionalLayerInfoKey, byte[], TaggedBlock>)>
	{
		{
			AdditionalLayerInfoKey.AnimationEffects,
			Entry((AdditionalLayerInfoKey _, byte[] d) => new AnimationEffects(d))
		},
		{
			AdditionalLayerInfoKey.Artboard1,
			Entry((AdditionalLayerInfoKey k, byte[] d) => new ArtboardData(k, d))
		},
		{
			AdditionalLayerInfoKey.Artboard2,
			Entry((AdditionalLayerInfoKey k, byte[] d) => new ArtboardData(k, d))
		},
		{
			AdditionalLayerInfoKey.Artboard3,
			Entry((AdditionalLayerInfoKey k, byte[] d) => new ArtboardData(k, d))
		},
		{
			AdditionalLayerInfoKey.BlackWhite,
			Entry((AdditionalLayerInfoKey _, byte[] d) => new BlackWhite(d))
		},
		{
			AdditionalLayerInfoKey.BrightnessAndContract,
			Entry((AdditionalLayerInfoKey _, byte[] d) => new BrightnessAndContract(d))
		},
		{
			AdditionalLayerInfoKey.ColorLookup,
			Entry((AdditionalLayerInfoKey _, byte[] d) => new ColorLookup(d))
		},
		{
			AdditionalLayerInfoKey.CompositorInfo,
			Entry((AdditionalLayerInfoKey _, byte[] d) => new CompositorInfo(d))
		},
		{
			AdditionalLayerInfoKey.ContentGeneratorExtraData,
			Entry((AdditionalLayerInfoKey _, byte[] d) => new ContentGeneratorExtraData(d))
		},
		{
			AdditionalLayerInfoKey.EffectsLayer,
			Entry((AdditionalLayerInfoKey _, byte[] d) => new EffectsLayer(d))
		},
		{
			AdditionalLayerInfoKey.Exposure,
			Entry((AdditionalLayerInfoKey _, byte[] d) => new Exposure(d))
		},
		{
			AdditionalLayerInfoKey.FilterMask,
			Entry((AdditionalLayerInfoKey _, byte[] d) => new FilterMask(d))
		},
		{
			AdditionalLayerInfoKey.LayerId,
			Entry((AdditionalLayerInfoKey _, byte[] d) => new LayerId(d))
		},
		{
			AdditionalLayerInfoKey.LayerVersion,
			Entry((AdditionalLayerInfoKey _, byte[] d) => new LayerVersion(d))
		},
		{
			AdditionalLayerInfoKey.ObjectsBasedEffectsLayerInfo,
			Entry((AdditionalLayerInfoKey _, byte[] d) => new ObjectsBasedEffectsLayerInfo(d))
		},
		{
			AdditionalLayerInfoKey.PixelSourceDataCc,
			Entry((AdditionalLayerInfoKey _, byte[] d) => new PixelSourceDataCc(d))
		},
		{
			AdditionalLayerInfoKey.PixelSourceDataCc2015,
			Entry((AdditionalLayerInfoKey _, byte[] d) => new PixelSourceDataCc2015(d))
		},
		{
			AdditionalLayerInfoKey.PlacedLayer1,
			Entry((AdditionalLayerInfoKey k, byte[] d) => new PlacedLayer(k, d))
		},
		{
			AdditionalLayerInfoKey.PlacedLayer2,
			Entry((AdditionalLayerInfoKey k, byte[] d) => new PlacedLayer(k, d))
		},
		{
			AdditionalLayerInfoKey.SmartObjectLayerData1,
			Entry((AdditionalLayerInfoKey _, byte[] d) => new PlacedLayerData(d))
		},
		{
			AdditionalLayerInfoKey.SmartObjectLayerData2,
			Entry((AdditionalLayerInfoKey _, byte[] d) => new SmartObjectLayerData(d))
		},
		{
			AdditionalLayerInfoKey.TextEngineData,
			Entry((AdditionalLayerInfoKey _, byte[] d) => new TextEngineData(d))
		},
		{
			AdditionalLayerInfoKey.UnicodePathName,
			Entry((AdditionalLayerInfoKey _, byte[] d) => new UnicodePathName(d))
		},
		{
			AdditionalLayerInfoKey.UserMask,
			Entry((AdditionalLayerInfoKey _, byte[] d) => new UserMask(d))
		},
		{
			AdditionalLayerInfoKey.Vibrance,
			Entry((AdditionalLayerInfoKey _, byte[] d) => new Vibrance(d))
		},
		{
			AdditionalLayerInfoKey.VectorOriginationData,
			Entry((AdditionalLayerInfoKey _, byte[] d) => new VectorOriginationData(d))
		},
		{
			AdditionalLayerInfoKey.VectorStrokeData,
			Entry((AdditionalLayerInfoKey _, byte[] d) => new VectorStrokeData(d))
		},
		{
			AdditionalLayerInfoKey.VectorStrokeContentData,
			Entry((AdditionalLayerInfoKey _, byte[] d) => new VectorStrokeContentData(d))
		},
		{
			AdditionalLayerInfoKey.UsingAlignedRendering,
			Entry((AdditionalLayerInfoKey _, byte[] d) => new UsingAlignedRendering(d))
		}
	};

	public static Dictionary<Type, AdditionalLayerInfoKey> KeyByLayer
	{
		get
		{
			if (_keyByLayer != null)
			{
				return _keyByLayer;
			}
			_keyByLayer = (from x in TaggedBlockTypes
				group x by x.Value.Type into x
				where x.Count() == 1
				select x).ToDictionary((IGrouping<Type, KeyValuePair<AdditionalLayerInfoKey, (Type Type, Func<AdditionalLayerInfoKey, byte[], TaggedBlock> Create)>> x) => x.Key, (IGrouping<Type, KeyValuePair<AdditionalLayerInfoKey, (Type Type, Func<AdditionalLayerInfoKey, byte[], TaggedBlock> Create)>> x) => x.First().Key);
			return _keyByLayer;
		}
	}

	private static (Type, Func<AdditionalLayerInfoKey, byte[], TaggedBlock>) Entry<T>(Func<AdditionalLayerInfoKey, byte[], T> create) where T : TaggedBlock
	{
		return (typeof(T), create);
	}
}
