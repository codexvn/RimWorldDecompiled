namespace AssetsTools.NET.Extra;

public class UnityVersion
{
	public int major;

	public int minor;

	public int patch;

	public string type;

	public int typeNum;

	public UnityVersion()
	{
	}

	public UnityVersion(string version)
	{
		string[] array = version.Split(new char[1] { '.' });
		major = int.Parse(array[0]);
		minor = int.Parse(array[1]);
		int num = array[2].IndexOfAny(new char[6] { 'f', 'p', 'a', 'b', 'c', 'x' });
		if (num != -1)
		{
			type = array[2][num].ToString();
			patch = int.Parse(array[2].Substring(0, num));
			string text = array[2].Substring(num + 1);
			if (!int.TryParse(text, out typeNum))
			{
				string text2 = "";
				for (int i = 0; i < text.Length && text[i] >= '0' && text[i] <= '9'; i++)
				{
					text2 += text[i];
				}
				if (text2.Length > 0)
				{
					typeNum = int.Parse(text2);
				}
			}
		}
		else
		{
			patch = int.Parse(array[2]);
			type = "";
			typeNum = 0;
		}
	}

	public override string ToString()
	{
		if (type == string.Empty)
		{
			return $"{major}.{minor}.{patch}";
		}
		return $"{major}.{minor}.{patch}{type}{typeNum}";
	}

	public ulong ToUInt64()
	{
		byte b = type switch
		{
			"a" => 0, 
			"b" => 1, 
			"c" => 2, 
			"f" => 3, 
			"p" => 4, 
			"x" => 5, 
			_ => byte.MaxValue, 
		};
		return (ulong)(((long)major << 48) | ((long)minor << 32) | ((long)patch << 16)) | ((ulong)b << 8) | (uint)typeNum;
	}

	public static UnityVersion FromUInt64(ulong data)
	{
		UnityVersion unityVersion = new UnityVersion();
		unityVersion.major = (int)((data >> 48) & 0xFFFF);
		unityVersion.minor = (int)((data >> 32) & 0xFFFF);
		unityVersion.patch = (int)((data >> 16) & 0xFFFF);
		UnityVersion unityVersion2 = unityVersion;
		ulong num = (data >> 8) & 0xFF;
		if (num > 5)
		{
			goto IL_00a2;
		}
		switch (num)
		{
		case 0uL:
			break;
		case 1uL:
			goto IL_007a;
		case 2uL:
			goto IL_0082;
		case 3uL:
			goto IL_008a;
		case 4uL:
			goto IL_0092;
		case 5uL:
			goto IL_009a;
		default:
			goto IL_00a2;
		}
		string text = "a";
		goto IL_00a8;
		IL_00a2:
		text = "?";
		goto IL_00a8;
		IL_00a8:
		unityVersion2.type = text;
		unityVersion.typeNum = (int)(data & 0xFF);
		return unityVersion;
		IL_007a:
		text = "b";
		goto IL_00a8;
		IL_0082:
		text = "c";
		goto IL_00a8;
		IL_008a:
		text = "f";
		goto IL_00a8;
		IL_0092:
		text = "p";
		goto IL_00a8;
		IL_009a:
		text = "x";
		goto IL_00a8;
	}
}
