using System;
using System.Text;

namespace AssetsTools.NET;

public class AssetTypeValue
{
	public AssetValueType ValueType { get; set; }

	private object Value { get; set; }

	public bool AsBool
	{
		get
		{
			object value = Value;
			if (value is bool)
			{
				return (bool)value;
			}
			if (Value is byte b)
			{
				return b == 1;
			}
			return false;
		}
		set
		{
			Value = value;
		}
	}

	public sbyte AsSByte
	{
		get
		{
			object value = Value;
			if (value is sbyte)
			{
				return (sbyte)value;
			}
			return (sbyte)Convert.ChangeType(Value, typeof(sbyte));
		}
		set
		{
			Value = value;
		}
	}

	public byte AsByte
	{
		get
		{
			object value = Value;
			if (value is byte)
			{
				return (byte)value;
			}
			return (byte)Convert.ChangeType(Value, typeof(byte));
		}
		set
		{
			Value = value;
		}
	}

	public short AsShort
	{
		get
		{
			object value = Value;
			if (value is short)
			{
				return (short)value;
			}
			return (short)Convert.ChangeType(Value, typeof(short));
		}
		set
		{
			Value = value;
		}
	}

	public ushort AsUShort
	{
		get
		{
			object value = Value;
			if (value is ushort)
			{
				return (ushort)value;
			}
			return (ushort)Convert.ChangeType(Value, typeof(ushort));
		}
		set
		{
			Value = value;
		}
	}

	public int AsInt
	{
		get
		{
			object value = Value;
			if (value is int)
			{
				return (int)value;
			}
			return (int)Convert.ChangeType(Value, typeof(int));
		}
		set
		{
			Value = value;
		}
	}

	public uint AsUInt
	{
		get
		{
			object value = Value;
			if (value is uint)
			{
				return (uint)value;
			}
			return (uint)Convert.ChangeType(Value, typeof(uint));
		}
		set
		{
			Value = value;
		}
	}

	public long AsLong
	{
		get
		{
			object value = Value;
			if (value is long)
			{
				return (long)value;
			}
			return (long)Convert.ChangeType(Value, typeof(long));
		}
		set
		{
			Value = value;
		}
	}

	public ulong AsULong
	{
		get
		{
			object value = Value;
			if (value is ulong)
			{
				return (ulong)value;
			}
			return (ulong)Convert.ChangeType(Value, typeof(ulong));
		}
		set
		{
			Value = value;
		}
	}

	public float AsFloat
	{
		get
		{
			object value = Value;
			if (value is float)
			{
				return (float)value;
			}
			return (float)Convert.ChangeType(Value, typeof(float));
		}
		set
		{
			Value = value;
		}
	}

	public double AsDouble
	{
		get
		{
			object value = Value;
			if (value is double)
			{
				return (double)value;
			}
			return (double)Convert.ChangeType(Value, typeof(double));
		}
		set
		{
			Value = value;
		}
	}

	public string AsString
	{
		get
		{
			if (ValueType == AssetValueType.String)
			{
				return Encoding.UTF8.GetString((byte[])Value);
			}
			if (ValueType == AssetValueType.Bool)
			{
				if (!(bool)Value)
				{
					return "false";
				}
				return "true";
			}
			if (ValueType == AssetValueType.ByteArray)
			{
				return SimpleHexDump((byte[])Value);
			}
			return Value.ToString();
		}
		set
		{
			Value = Encoding.UTF8.GetBytes(value);
		}
	}

	public AssetTypeArrayInfo AsArray
	{
		get
		{
			return (AssetTypeArrayInfo)Value;
		}
		set
		{
			Value = value;
		}
	}

	public byte[] AsByteArray
	{
		get
		{
			return (byte[])Value;
		}
		set
		{
			Value = value;
		}
	}

	public ManagedReferencesRegistry AsManagedReferencesRegistry
	{
		get
		{
			return (ManagedReferencesRegistry)Value;
		}
		set
		{
			Value = value;
		}
	}

	public object AsObject
	{
		get
		{
			return Value;
		}
		set
		{
			if (value is string s)
			{
				Value = Encoding.UTF8.GetBytes(s);
			}
			else
			{
				Value = value;
			}
		}
	}

	public AssetTypeValue(bool value)
	{
		ValueType = AssetValueType.Bool;
		Value = value;
	}

	public AssetTypeValue(sbyte value)
	{
		ValueType = AssetValueType.Int8;
		Value = value;
	}

	public AssetTypeValue(byte value)
	{
		ValueType = AssetValueType.UInt8;
		Value = value;
	}

	public AssetTypeValue(short value)
	{
		ValueType = AssetValueType.Int16;
		Value = value;
	}

	public AssetTypeValue(ushort value)
	{
		ValueType = AssetValueType.UInt16;
		Value = value;
	}

	public AssetTypeValue(int value)
	{
		ValueType = AssetValueType.Int32;
		Value = value;
	}

	public AssetTypeValue(uint value)
	{
		ValueType = AssetValueType.UInt32;
		Value = value;
	}

	public AssetTypeValue(long value)
	{
		ValueType = AssetValueType.Int64;
		Value = value;
	}

	public AssetTypeValue(ulong value)
	{
		ValueType = AssetValueType.UInt64;
		Value = value;
	}

	public AssetTypeValue(float value)
	{
		ValueType = AssetValueType.Float;
		Value = value;
	}

	public AssetTypeValue(double value)
	{
		ValueType = AssetValueType.Double;
		Value = value;
	}

	public AssetTypeValue(string value)
	{
		ValueType = AssetValueType.String;
		Value = Encoding.UTF8.GetBytes(value);
	}

	public AssetTypeValue(byte[] value, bool asString)
	{
		ValueType = (asString ? AssetValueType.String : AssetValueType.ByteArray);
		Value = value;
	}

	public AssetTypeValue(ManagedReferencesRegistry value)
	{
		ValueType = AssetValueType.ManagedReferencesRegistry;
		Value = value;
	}

	public AssetTypeValue(AssetValueType valueType, object value = null)
	{
		ValueType = valueType;
		if (value is string s)
		{
			Value = Encoding.UTF8.GetBytes(s);
		}
		else
		{
			Value = value;
		}
	}

	public override string ToString()
	{
		return AsString;
	}

	private string SimpleHexDump(byte[] byteArray)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (byteArray.Length == 0)
		{
			return string.Empty;
		}
		int i;
		for (i = 0; i < byteArray.Length - 1; i++)
		{
			stringBuilder.Append(byteArray[i].ToString("x2"));
			stringBuilder.Append(" ");
		}
		stringBuilder.Append(byteArray[i].ToString("x2"));
		return stringBuilder.ToString();
	}
}
