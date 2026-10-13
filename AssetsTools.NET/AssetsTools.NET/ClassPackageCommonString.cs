using System.Collections.Generic;
using AssetsTools.NET.Extra;

namespace AssetsTools.NET;

public class ClassPackageCommonString
{
	public List<KeyValuePair<UnityVersion, byte>> VersionInformation { get; set; }

	public List<ushort> StringBufferIndices { get; set; }

	public void Read(AssetsFileReader reader)
	{
		int num = reader.ReadInt32();
		VersionInformation = new List<KeyValuePair<UnityVersion, byte>>(num);
		for (int i = 0; i < num; i++)
		{
			UnityVersion key = UnityVersion.FromUInt64(reader.ReadUInt64());
			byte value = reader.ReadByte();
			VersionInformation.Add(new KeyValuePair<UnityVersion, byte>(key, value));
		}
		int num2 = reader.ReadInt32();
		StringBufferIndices = new List<ushort>(num2);
		for (int j = 0; j < num2; j++)
		{
			StringBufferIndices.Add(reader.ReadUInt16());
		}
	}

	public void Write(AssetsFileWriter writer)
	{
		writer.Write(VersionInformation.Count);
		foreach (KeyValuePair<UnityVersion, byte> item in VersionInformation)
		{
			writer.Write(item.Key.ToUInt64());
			writer.Write(item.Value);
		}
		writer.Write(StringBufferIndices.Count);
		for (int i = 0; i < StringBufferIndices.Count; i++)
		{
			writer.Write(StringBufferIndices[i]);
		}
	}

	public byte GetCommonStringLengthForVersion(UnityVersion version)
	{
		if (VersionInformation.Count == 0)
		{
			return 0;
		}
		byte value = VersionInformation[0].Value;
		for (int i = 0; i < VersionInformation.Count; i++)
		{
			if (VersionInformation[i].Key.ToUInt64() >= version.ToUInt64())
			{
				return value;
			}
			value = VersionInformation[i].Value;
		}
		return value;
	}
}
