using System.Collections.Generic;
using AssetsTools.NET.Extra;

namespace AssetsTools.NET;

public class RefTypeManager
{
	private Dictionary<AssetTypeReference, AssetTypeTemplateField> typeTreeLookup;

	private Dictionary<AssetTypeReference, AssetTypeTemplateField> monoTemplateLookup;

	private IMonoBehaviourTemplateGenerator monoTemplateGenerator;

	private UnityVersion unityVersion;

	private bool isSharedMonoLookup;

	public RefTypeManager()
	{
		typeTreeLookup = new Dictionary<AssetTypeReference, AssetTypeTemplateField>();
	}

	public void Clear()
	{
		typeTreeLookup.Clear();
		if (!isSharedMonoLookup)
		{
			monoTemplateLookup.Clear();
		}
	}

	public void FromTypeTree(AssetsFileMetadata metadata)
	{
		if (!metadata.TypeTreeEnabled || metadata.RefTypes == null)
		{
			return;
		}
		foreach (TypeTreeType refType in metadata.RefTypes)
		{
			if (refType.IsRefType)
			{
				AssetTypeTemplateField assetTypeTemplateField = new AssetTypeTemplateField();
				assetTypeTemplateField.FromTypeTree(refType);
				if (assetTypeTemplateField.Children.Count > 0 && assetTypeTemplateField.Children[assetTypeTemplateField.Children.Count - 1].ValueType == AssetValueType.ManagedReferencesRegistry)
				{
					assetTypeTemplateField.Children.RemoveAt(assetTypeTemplateField.Children.Count - 1);
				}
				typeTreeLookup[refType.TypeReference] = assetTypeTemplateField;
			}
		}
	}

	public void WithMonoTemplateGenerator(AssetsFileMetadata metadata, IMonoBehaviourTemplateGenerator monoTemplateGenerator, Dictionary<AssetTypeReference, AssetTypeTemplateField> monoTemplateFieldCache = null)
	{
		this.monoTemplateGenerator = monoTemplateGenerator;
		unityVersion = new UnityVersion(metadata.UnityVersion);
		monoTemplateLookup = monoTemplateFieldCache ?? new Dictionary<AssetTypeReference, AssetTypeTemplateField>();
		isSharedMonoLookup = monoTemplateLookup != null;
	}

	public AssetTypeTemplateField GetTemplateField(AssetTypeReference type)
	{
		if (type == null || (string.IsNullOrEmpty(type.ClassName) && string.IsNullOrEmpty(type.Namespace) && string.IsNullOrEmpty(type.AsmName)) || type.Equals(AssetTypeReference.TERMINUS))
		{
			return null;
		}
		if (typeTreeLookup.TryGetValue(type, out var value))
		{
			return value;
		}
		if (monoTemplateGenerator != null)
		{
			if (monoTemplateLookup.TryGetValue(type, out value))
			{
				return value;
			}
			value = new AssetTypeTemplateField
			{
				Name = "Base",
				Type = type.ClassName,
				ValueType = AssetValueType.None,
				IsArray = false,
				IsAligned = false,
				HasValue = false,
				Children = new List<AssetTypeTemplateField>(0)
			};
			value = monoTemplateGenerator.GetTemplateField(value, type.AsmName, type.Namespace, type.ClassName, unityVersion);
			if (value != null)
			{
				monoTemplateLookup[type] = value;
				return value;
			}
		}
		return null;
	}
}
