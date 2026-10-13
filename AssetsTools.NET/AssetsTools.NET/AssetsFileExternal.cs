namespace AssetsTools.NET;

public class AssetsFileExternal
{
	public string VirtualAssetPathName { get; set; }

	public GUID128 Guid { get; set; }

	public AssetsFileExternalType Type { get; set; }

	public string PathName { get; set; }

	public string OriginalPathName { get; set; }

	public void Read(AssetsFileReader reader)
	{
		VirtualAssetPathName = reader.ReadNullTerminated();
		Guid = new GUID128(reader);
		Type = (AssetsFileExternalType)reader.ReadInt32();
		PathName = reader.ReadNullTerminated();
		OriginalPathName = PathName;
		if (PathName == "resources/unity_builtin_extra")
		{
			PathName = "Resources/unity_builtin_extra";
		}
		else if (PathName == "library/unity default resources" || PathName == "Library/unity default resources")
		{
			PathName = "Resources/unity default resources";
		}
		else if (PathName == "library/unity editor resources" || PathName == "Library/unity editor resources")
		{
			PathName = "Resources/unity editor resources";
		}
	}

	public void Write(AssetsFileWriter writer)
	{
		writer.WriteNullTerminated(VirtualAssetPathName);
		Guid.Write(writer);
		writer.Write((int)Type);
		string text = PathName;
		if ((PathName == "Resources/unity_builtin_extra" || PathName == "Resources/unity default resources" || PathName == "Resources/unity editor resources") && OriginalPathName != string.Empty)
		{
			text = OriginalPathName;
		}
		writer.WriteNullTerminated(text);
	}
}
