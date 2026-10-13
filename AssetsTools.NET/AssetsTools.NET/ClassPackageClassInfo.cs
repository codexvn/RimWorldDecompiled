using System.Collections.Generic;
using AssetsTools.NET.Extra;

namespace AssetsTools.NET;

public class ClassPackageClassInfo
{
	public int ClassId { get; set; }

	public List<KeyValuePair<UnityVersion, ClassPackageType>> Classes { get; set; }

	public void Read(AssetsFileReader reader)
	{
		ClassId = reader.ReadInt32();
		int num = reader.ReadInt32();
		Classes = new List<KeyValuePair<UnityVersion, ClassPackageType>>(num);
		for (int i = 0; i < num; i++)
		{
			UnityVersion key = UnityVersion.FromUInt64(reader.ReadUInt64());
			bool num2 = reader.ReadBoolean();
			ClassPackageType classPackageType = null;
			if (num2)
			{
				classPackageType = new ClassPackageType();
				classPackageType.Read(reader, ClassId);
			}
			Classes.Add(new KeyValuePair<UnityVersion, ClassPackageType>(key, classPackageType));
		}
	}

	public void Write(AssetsFileWriter writer)
	{
		writer.Write(ClassId);
		writer.Write(Classes.Count);
		for (int i = 0; i < Classes.Count; i++)
		{
			writer.Write(Classes[i].Key.ToUInt64());
			if (Classes[i].Value != null)
			{
				writer.Write((byte)1);
				Classes[i].Value.Write(writer);
			}
			else
			{
				writer.Write((byte)0);
			}
		}
	}

	public ClassPackageType GetTypeForVersion(UnityVersion version)
	{
		if (Classes.Count == 0)
		{
			return null;
		}
		if (Classes[0].Key.ToUInt64() > version.ToUInt64())
		{
			return null;
		}
		ClassPackageType value = Classes[0].Value;
		for (int i = 0; i < Classes.Count; i++)
		{
			if (Classes[i].Key.ToUInt64() == version.ToUInt64())
			{
				return Classes[i].Value;
			}
			if (Classes[i].Key.ToUInt64() > version.ToUInt64())
			{
				return value;
			}
			value = Classes[i].Value;
		}
		return value;
	}
}
