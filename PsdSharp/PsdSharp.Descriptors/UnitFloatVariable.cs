using System.Collections.Generic;
using System.Linq;
using PsdSharp.Parsing;

namespace PsdSharp.Descriptors;

public class UnitFloatVariable : Variable
{
	public class UnitType
	{
		public readonly string Key;

		public static readonly UnitType AngleDegrees = new UnitType("#Ang");

		public static readonly UnitType DistanceBasePerInch = new UnitType("#Rsl");

		public static readonly UnitType DistanceBase72Ppi = new UnitType("#Rlt");

		public static readonly UnitType None = new UnitType("#Nne");

		public static readonly UnitType Percent = new UnitType("#Prc");

		public static readonly UnitType Pixels = new UnitType("#Pxl");

		public static readonly UnitType Points = new UnitType("#Pnt");

		public static readonly UnitType Millimeters = new UnitType("#Mlm");

		internal static readonly UnitType[] All = new UnitType[8] { AngleDegrees, DistanceBasePerInch, DistanceBase72Ppi, None, Percent, Pixels, Points, Millimeters };

		internal static Dictionary<string, UnitType> ByKey = All.ToDictionary((UnitType x) => x.Key);

		public override bool Equals(object? obj)
		{
			if (obj is UnitType other)
			{
				return Equals(other);
			}
			return false;
		}

		public bool Equals(UnitType other)
		{
			return Key == other.Key;
		}

		public override int GetHashCode()
		{
			return Key.GetHashCode();
		}

		private UnitType(string key)
		{
			Key = key;
		}

		internal static UnitType Get(string key)
		{
			if (!ByKey.TryGetValue(key, out UnitType value))
			{
				return new UnitType(key);
			}
			return value;
		}
	}

	public readonly UnitType Unit;

	public readonly double ValueDouble;

	public override object Value => ValueDouble;

	internal UnitFloatVariable(string key, BigEndianReader reader)
		: base(key)
	{
		Unit = UnitType.Get(reader.ReadFixedLengthString(4));
		ValueDouble = reader.ReadDouble();
	}
}
