namespace AssetsTools.NET;

public class AssetTypeReference
{
	public static readonly AssetTypeReference TERMINUS = new AssetTypeReference("Terminus", "UnityEngine.DMAT", "FAKE_ASM");

	public string ClassName { get; set; }

	public string Namespace { get; set; }

	public string AsmName { get; set; }

	public AssetTypeReference()
	{
	}

	public AssetTypeReference(string className, string nameSpace, string asmName)
	{
		ClassName = className;
		Namespace = nameSpace;
		AsmName = asmName;
	}

	public void ReadMetadata(AssetsFileReader reader)
	{
		ClassName = reader.ReadNullTerminated();
		Namespace = reader.ReadNullTerminated();
		AsmName = reader.ReadNullTerminated();
	}

	public void ReadAsset(AssetsFileReader reader)
	{
		ClassName = reader.ReadCountStringInt32();
		reader.Align();
		Namespace = reader.ReadCountStringInt32();
		reader.Align();
		AsmName = reader.ReadCountStringInt32();
		reader.Align();
	}

	public void WriteMetadata(AssetsFileWriter writer)
	{
		writer.WriteNullTerminated(ClassName);
		writer.WriteNullTerminated(Namespace);
		writer.WriteNullTerminated(AsmName);
	}

	public void WriteAsset(AssetsFileWriter writer)
	{
		writer.WriteCountStringInt32(ClassName);
		writer.Align();
		writer.WriteCountStringInt32(Namespace);
		writer.Align();
		writer.WriteCountStringInt32(AsmName);
		writer.Align();
	}

	public override bool Equals(object obj)
	{
		if (!(obj is AssetTypeReference assetTypeReference))
		{
			return false;
		}
		if (ClassName == assetTypeReference.ClassName && Namespace == assetTypeReference.Namespace)
		{
			return AsmName == assetTypeReference.AsmName;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return ((17 * 23 + ClassName.GetHashCode()) * 23 + Namespace.GetHashCode()) * 23 + AsmName.GetHashCode();
	}
}
