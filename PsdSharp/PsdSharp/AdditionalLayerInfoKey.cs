using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PsdSharp;

[DebuggerDisplay("{Key}")]
public class AdditionalLayerInfoKey
{
	public readonly string Key;

	public readonly string Description;

	public readonly byte PsbLengthCountSizeBytes;

	public static readonly AdditionalLayerInfoKey Alpha = new AdditionalLayerInfoKey("Alph", "Alpha", 8);

	public static readonly AdditionalLayerInfoKey AnimationEffects = new AdditionalLayerInfoKey("anFX", "AnimationEffects", 4);

	public static readonly AdditionalLayerInfoKey Annotations = new AdditionalLayerInfoKey("Anno", "Annotations", 4);

	public static readonly AdditionalLayerInfoKey Artboard1 = new AdditionalLayerInfoKey("artb", "Artboard1", 4);

	public static readonly AdditionalLayerInfoKey Artboard2 = new AdditionalLayerInfoKey("artd", "Artboard2", 4);

	public static readonly AdditionalLayerInfoKey Artboard3 = new AdditionalLayerInfoKey("abdd", "Artboard3", 4);

	public static readonly AdditionalLayerInfoKey BlackWhite = new AdditionalLayerInfoKey("blwh", "BlackWhite", 4);

	public static readonly AdditionalLayerInfoKey BlendClippingElements = new AdditionalLayerInfoKey("clbl", "BlendClippingElements", 4);

	public static readonly AdditionalLayerInfoKey BlendFillOpacity = new AdditionalLayerInfoKey("iOpa", "BlendFillOpacity", 4);

	public static readonly AdditionalLayerInfoKey BlendInteriorElements = new AdditionalLayerInfoKey("infx", "BlendInteriorElements", 4);

	public static readonly AdditionalLayerInfoKey BrightnessAndContract = new AdditionalLayerInfoKey("brit", "BrightnessAndContract", 4);

	public static readonly AdditionalLayerInfoKey ChannelBalance = new AdditionalLayerInfoKey("blnc", "ChannelBalance", 4);

	public static readonly AdditionalLayerInfoKey ChannelBlendingRestrictions = new AdditionalLayerInfoKey("brst", "ChannelBlendingRestrictions", 4);

	public static readonly AdditionalLayerInfoKey ChannelMixer = new AdditionalLayerInfoKey("mixr", "ChannelMixer", 4);

	public static readonly AdditionalLayerInfoKey ColorLookup = new AdditionalLayerInfoKey("clrL", "ColorLookup", 4);

	public static readonly AdditionalLayerInfoKey CompositorInfo = new AdditionalLayerInfoKey("cinf", "CompositorInfo", 4);

	public static readonly AdditionalLayerInfoKey ContentGeneratorExtraData = new AdditionalLayerInfoKey("CgEd", "ContentGeneratorExtraData", 4);

	public static readonly AdditionalLayerInfoKey Curves = new AdditionalLayerInfoKey("curv", "Curves", 4);

	public static readonly AdditionalLayerInfoKey EffectsLayer = new AdditionalLayerInfoKey("lrFX", "EffectsLayer", 4);

	public static readonly AdditionalLayerInfoKey ExportSetting1 = new AdditionalLayerInfoKey("extd", "ExportSetting1", 4);

	public static readonly AdditionalLayerInfoKey ExportSetting2 = new AdditionalLayerInfoKey("extn", "ExportSetting2", 4);

	public static readonly AdditionalLayerInfoKey Exposure = new AdditionalLayerInfoKey("expA", "Exposure", 4);

	public static readonly AdditionalLayerInfoKey FilterEffects1 = new AdditionalLayerInfoKey("FXid", "FilterEffects1", 8);

	public static readonly AdditionalLayerInfoKey FilterEffects2 = new AdditionalLayerInfoKey("FEid", "FilterEffects2", 8);

	public static readonly AdditionalLayerInfoKey FilterEffects3 = new AdditionalLayerInfoKey("FELS", "FilterEffects3", 8);

	public static readonly AdditionalLayerInfoKey FilterMask = new AdditionalLayerInfoKey("FMsk", "FilterMask", 8);

	public static readonly AdditionalLayerInfoKey ForeignEffectId = new AdditionalLayerInfoKey("ffxi", "ForeignEffectId", 4);

	public static readonly AdditionalLayerInfoKey FramedGroup = new AdditionalLayerInfoKey("frgb", "FramedGroup", 4);

	public static readonly AdditionalLayerInfoKey GradientFill = new AdditionalLayerInfoKey("GdFl", "GradientFill", 4);

	public static readonly AdditionalLayerInfoKey GradientMap = new AdditionalLayerInfoKey("grdm", "GradientMap", 4);

	public static readonly AdditionalLayerInfoKey HueSaturation = new AdditionalLayerInfoKey("hue2", "HueSaturation", 4);

	public static readonly AdditionalLayerInfoKey HueSaturationV4 = new AdditionalLayerInfoKey("hue ", "HueSaturationV4", 4);

	public static readonly AdditionalLayerInfoKey Invert = new AdditionalLayerInfoKey("nvrt", "Invert", 4);

	public static readonly AdditionalLayerInfoKey Knockout = new AdditionalLayerInfoKey("knko", "Knockout", 4);

	public static readonly AdditionalLayerInfoKey Layer = new AdditionalLayerInfoKey("Layr", "Layer", 8);

	public static readonly AdditionalLayerInfoKey Layer16 = new AdditionalLayerInfoKey("Lr16", "Layer16", 8);

	public static readonly AdditionalLayerInfoKey Layer32 = new AdditionalLayerInfoKey("Lr32", "Layer32", 8);

	public static readonly AdditionalLayerInfoKey LayerId = new AdditionalLayerInfoKey("lyid", "LayerId", 4);

	public static readonly AdditionalLayerInfoKey LayerMaskAsGlobalMask = new AdditionalLayerInfoKey("lmgm", "LayerMaskAsGlobalMask", 4);

	public static readonly AdditionalLayerInfoKey LayerNameSource = new AdditionalLayerInfoKey("lnsr", "LayerNameSource", 4);

	public static readonly AdditionalLayerInfoKey LayerVersion = new AdditionalLayerInfoKey("lyvr", "LayerVersion", 4);

	public static readonly AdditionalLayerInfoKey Levels = new AdditionalLayerInfoKey("levl", "LayerVersion", 4);

	public static readonly AdditionalLayerInfoKey LinkedLayer1 = new AdditionalLayerInfoKey("lnkD", "LinkedLayer1", 4);

	public static readonly AdditionalLayerInfoKey LinkedLayer2 = new AdditionalLayerInfoKey("lnk2", "LinkedLayer2", 4);

	public static readonly AdditionalLayerInfoKey LinkedLayer3 = new AdditionalLayerInfoKey("lnk3", "LinkedLayer3", 4);

	public static readonly AdditionalLayerInfoKey LinkedLayerExternal = new AdditionalLayerInfoKey("lnkE", "LinkedLayerExternal", 4);

	public static readonly AdditionalLayerInfoKey Metadata = new AdditionalLayerInfoKey("shmd", "Metadata", 4);

	public static readonly AdditionalLayerInfoKey NestedSectionDivider = new AdditionalLayerInfoKey("lsdk", "NestedSectionDivider", 4);

	public static readonly AdditionalLayerInfoKey ObjectsBasedEffectsLayerInfo = new AdditionalLayerInfoKey("lfx2", "ObjectsBasedEffectsLayerInfo", 4);

	public static readonly AdditionalLayerInfoKey ObjectsBasedEffectsLayerInfoV0 = new AdditionalLayerInfoKey("lmfx", "ObjectsBasedEffectsLayerInfoV0", 4);

	public static readonly AdditionalLayerInfoKey ObjectsBasedEffectsLayerInfoV1 = new AdditionalLayerInfoKey("lfxs", "ObjectsBasedEffectsLayerInfoV1", 4);

	public static readonly AdditionalLayerInfoKey Pattern = new AdditionalLayerInfoKey("patt", "PatternData", 4);

	public static readonly AdditionalLayerInfoKey PatternData = new AdditionalLayerInfoKey("shpa", "PatternData", 4);

	public static readonly AdditionalLayerInfoKey PatternFill = new AdditionalLayerInfoKey("PtFl", "PatternFill", 4);

	public static readonly AdditionalLayerInfoKey Patterns1 = new AdditionalLayerInfoKey("Patt", "Patterns1", 4);

	public static readonly AdditionalLayerInfoKey Patterns2 = new AdditionalLayerInfoKey("Pat2", "Patterns2", 4);

	public static readonly AdditionalLayerInfoKey Patterns3 = new AdditionalLayerInfoKey("Pat3", "Patterns3", 4);

	public static readonly AdditionalLayerInfoKey PhotoFilter = new AdditionalLayerInfoKey("phfl", "PhotoFilter", 4);

	public static readonly AdditionalLayerInfoKey PixelSourceDataCc = new AdditionalLayerInfoKey("PxSc", "PixelSourceDataCc", 4);

	public static readonly AdditionalLayerInfoKey PixelSourceDataCc2015 = new AdditionalLayerInfoKey("PxSD", "PixelSourceDataCc2015", 8);

	public static readonly AdditionalLayerInfoKey PlacedLayer1 = new AdditionalLayerInfoKey("plLd", "PlacedLayer1", 4);

	public static readonly AdditionalLayerInfoKey PlacedLayer2 = new AdditionalLayerInfoKey("PlLd", "PlacedLayer2", 4);

	public static readonly AdditionalLayerInfoKey Posterize = new AdditionalLayerInfoKey("post", "Posterize", 4);

	public static readonly AdditionalLayerInfoKey Protected = new AdditionalLayerInfoKey("lspf", "Protected", 4);

	public static readonly AdditionalLayerInfoKey ReferencePoint = new AdditionalLayerInfoKey("fxrp", "ReferencePoint", 4);

	public static readonly AdditionalLayerInfoKey SavingMergedTransparency = new AdditionalLayerInfoKey("Mtrn", "SavingMergedTransparency", 8);

	public static readonly AdditionalLayerInfoKey SavingMergedTransparency16 = new AdditionalLayerInfoKey("Mt16", "SavingMergedTransparency16", 8);

	public static readonly AdditionalLayerInfoKey SavingMergedTransparency32 = new AdditionalLayerInfoKey("Mt32", "SavingMergedTransparency32", 8);

	public static readonly AdditionalLayerInfoKey SectionDivider = new AdditionalLayerInfoKey("lsct", "SectionDivider", 4);

	public static readonly AdditionalLayerInfoKey SelectiveColor = new AdditionalLayerInfoKey("selc", "SelectiveColor", 4);

	public static readonly AdditionalLayerInfoKey SheetColor = new AdditionalLayerInfoKey("lclr", "SheetColor", 4);

	public static readonly AdditionalLayerInfoKey SmartObjectLayerData1 = new AdditionalLayerInfoKey("soLD", "SmartObjectLayerData1", 4);

	public static readonly AdditionalLayerInfoKey SmartObjectLayerData2 = new AdditionalLayerInfoKey("SoLE", "SmartObjectLayerData2", 4);

	public static readonly AdditionalLayerInfoKey SolidColorSheet = new AdditionalLayerInfoKey("SoCo", "SolidColorSheet", 4);

	public static readonly AdditionalLayerInfoKey TextEngineData = new AdditionalLayerInfoKey("Txt2", "TextEngineData", 4);

	public static readonly AdditionalLayerInfoKey Threshold = new AdditionalLayerInfoKey("thrs", "Threshold", 4);

	public static readonly AdditionalLayerInfoKey TransparencyShapesLayer = new AdditionalLayerInfoKey("tsly", "TransparencyShapesLayer", 4);

	public static readonly AdditionalLayerInfoKey TypeToolInfo = new AdditionalLayerInfoKey("tySh", "TypeToolInfo", 4);

	public static readonly AdditionalLayerInfoKey TypeToolObject = new AdditionalLayerInfoKey("TySh", "TypeToolObject", 4);

	public static readonly AdditionalLayerInfoKey UnicodeLayerName = new AdditionalLayerInfoKey("luni", "UnicodeLayerName", 4);

	public static readonly AdditionalLayerInfoKey UnicodePathName = new AdditionalLayerInfoKey("pths", "UnicodePathName", 4);

	public static readonly AdditionalLayerInfoKey UserMask = new AdditionalLayerInfoKey("LMsk", "UserMask", 8);

	public static readonly AdditionalLayerInfoKey UsingAlignedRendering = new AdditionalLayerInfoKey("sn2P", "UsingAlignedRendering", 4);

	public static readonly AdditionalLayerInfoKey VectorMask = new AdditionalLayerInfoKey("vmsk", "VectorMask", 4);

	public static readonly AdditionalLayerInfoKey VectorMaskAsGlobalmask = new AdditionalLayerInfoKey("vmgm", "VectorMaskAsGlobalmask", 4);

	public static readonly AdditionalLayerInfoKey VectorMaskPhotoshop6 = new AdditionalLayerInfoKey("vsms", "VectorMaskPhotoshop6", 4);

	public static readonly AdditionalLayerInfoKey VectorOriginationData = new AdditionalLayerInfoKey("vogk", "VectorOriginationData", 4);

	public static readonly AdditionalLayerInfoKey VectorOriginationUnknown = new AdditionalLayerInfoKey("vowv", "VectorOriginationUnknown", 4);

	public static readonly AdditionalLayerInfoKey VectorStrokeContentData = new AdditionalLayerInfoKey("vscg", "VectorStrokeContentData", 4);

	public static readonly AdditionalLayerInfoKey VectorStrokeData = new AdditionalLayerInfoKey("vstk", "VectorStrokeData", 4);

	public static readonly AdditionalLayerInfoKey Vibrance = new AdditionalLayerInfoKey("vibA", "Vibrance", 4);

	public static readonly AdditionalLayerInfoKey Cai = new AdditionalLayerInfoKey("CAI ", "Cai", 4);

	public static readonly AdditionalLayerInfoKey Geni = new AdditionalLayerInfoKey("GenI", "Geni", 4);

	public static readonly AdditionalLayerInfoKey Ocio = new AdditionalLayerInfoKey("OCIO", "Ocio", 4);

	public static readonly AdditionalLayerInfoKey[] All = new AdditionalLayerInfoKey[94]
	{
		Alpha, AnimationEffects, Annotations, Artboard1, Artboard2, Artboard3, BlackWhite, BlendClippingElements, BlendFillOpacity, BlendInteriorElements,
		BrightnessAndContract, ChannelBalance, ChannelBlendingRestrictions, ChannelMixer, ColorLookup, CompositorInfo, ContentGeneratorExtraData, Curves, EffectsLayer, ExportSetting1,
		ExportSetting2, Exposure, FilterEffects1, FilterEffects2, FilterEffects3, FilterMask, ForeignEffectId, FramedGroup, GradientFill, GradientMap,
		HueSaturation, HueSaturationV4, Invert, Knockout, Layer, Layer16, Layer32, LayerId, LayerMaskAsGlobalMask, LayerNameSource,
		LayerVersion, Levels, LinkedLayer1, LinkedLayer2, LinkedLayer3, LinkedLayerExternal, Metadata, NestedSectionDivider, ObjectsBasedEffectsLayerInfo, ObjectsBasedEffectsLayerInfoV0,
		ObjectsBasedEffectsLayerInfoV1, Pattern, PatternData, PatternFill, Patterns1, Patterns2, Patterns3, PhotoFilter, PixelSourceDataCc, PixelSourceDataCc2015,
		PlacedLayer1, PlacedLayer2, Posterize, Protected, ReferencePoint, SavingMergedTransparency, SavingMergedTransparency16, SavingMergedTransparency32, SectionDivider, SelectiveColor,
		SheetColor, SmartObjectLayerData1, SmartObjectLayerData2, SolidColorSheet, TextEngineData, Threshold, TransparencyShapesLayer, TypeToolInfo, TypeToolObject, UnicodeLayerName,
		UnicodePathName, UserMask, UsingAlignedRendering, VectorMask, VectorMaskAsGlobalmask, VectorMaskPhotoshop6, VectorOriginationData, VectorOriginationUnknown, VectorStrokeContentData, VectorStrokeData,
		Vibrance, Cai, Geni, Ocio
	};

	public static readonly IDictionary<string, AdditionalLayerInfoKey> ByKey = All.ToDictionary((AdditionalLayerInfoKey k) => k.Key);

	internal AdditionalLayerInfoKey(string key, string description, byte psbLengthCountSizeBytes)
	{
		Key = key;
		Description = description;
		PsbLengthCountSizeBytes = psbLengthCountSizeBytes;
	}

	public override bool Equals(object? obj)
	{
		if (obj is AdditionalLayerInfoKey other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (Key.GetHashCode() * 397) ^ PsbLengthCountSizeBytes.GetHashCode();
	}

	public bool Equals(AdditionalLayerInfoKey other)
	{
		return Key == other.Key;
	}

	public override string ToString()
	{
		return Key;
	}
}
