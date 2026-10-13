using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Steamworks;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200012C RID: 300
	public static class ParseHelper
	{
		// Token: 0x06000937 RID: 2359 RVA: 0x0002C003 File Offset: 0x0002A203
		public static string ParseString(string str)
		{
			return str.Replace("\\n", "\n");
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x0002C015 File Offset: 0x0002A215
		public static uint ParseUInt(string str)
		{
			return uint.Parse(str, NumberStyles.Any, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x0002C028 File Offset: 0x0002A228
		public static int ParseIntPermissive(string str)
		{
			int result;
			if (!int.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				result = (int)float.Parse(str, CultureInfo.InvariantCulture);
				Log.Warning("Parsed " + str + " as int.");
			}
			return result;
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x0002C06C File Offset: 0x0002A26C
		public static Vector3 FromStringVector3(string Str)
		{
			Str = Str.TrimStart('(');
			Str = Str.TrimEnd(')');
			string[] array = Str.Split(',', StringSplitOptions.None);
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			float x = float.Parse(array[0], invariantCulture);
			float y = float.Parse(array[1], invariantCulture);
			float z = float.Parse(array[2], invariantCulture);
			return new Vector3(x, y, z);
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x0002C0C4 File Offset: 0x0002A2C4
		public static Vector2 FromStringVector2(string Str)
		{
			Str = Str.TrimStart('(');
			Str = Str.TrimEnd(')');
			string[] array = Str.Split(',', StringSplitOptions.None);
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			float x;
			float y;
			if (array.Length == 1)
			{
				y = (x = Convert.ToSingle(array[0], invariantCulture));
			}
			else
			{
				if (array.Length != 2)
				{
					throw new InvalidOperationException();
				}
				x = Convert.ToSingle(array[0], invariantCulture);
				y = Convert.ToSingle(array[1], invariantCulture);
			}
			return new Vector2(x, y);
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x0002C134 File Offset: 0x0002A334
		public static Vector4 FromStringVector4Adaptive(string Str)
		{
			Str = Str.TrimStart('(');
			Str = Str.TrimEnd(')');
			string[] array = Str.Split(',', StringSplitOptions.None);
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			float x = 0f;
			float y = 0f;
			float z = 0f;
			float w = 0f;
			if (array.Length >= 1)
			{
				x = Convert.ToSingle(array[0], invariantCulture);
			}
			if (array.Length >= 2)
			{
				y = Convert.ToSingle(array[1], invariantCulture);
			}
			if (array.Length >= 3)
			{
				z = Convert.ToSingle(array[2], invariantCulture);
			}
			if (array.Length >= 4)
			{
				w = Convert.ToSingle(array[3], invariantCulture);
			}
			if (array.Length >= 5)
			{
				Log.ErrorOnce(string.Format("Too many elements in vector {0}", Str), 16139142);
			}
			return new Vector4(x, y, z, w);
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x0002C1E8 File Offset: 0x0002A3E8
		public static Quaternion FromStringQuaternion(string Str)
		{
			Str = Str.TrimStart('(');
			Str = Str.TrimEnd(')');
			string[] array = Str.Split(',', StringSplitOptions.None);
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			float x = 0f;
			float y = 0f;
			float z = 0f;
			float w = 0f;
			if (array.Length >= 1)
			{
				x = Convert.ToSingle(array[0], invariantCulture);
			}
			if (array.Length >= 2)
			{
				y = Convert.ToSingle(array[1], invariantCulture);
			}
			if (array.Length >= 3)
			{
				z = Convert.ToSingle(array[2], invariantCulture);
			}
			if (array.Length >= 4)
			{
				w = Convert.ToSingle(array[3], invariantCulture);
			}
			if (array.Length >= 5)
			{
				Log.ErrorOnce(string.Format("Too many elements in quaternion {0}", Str), 16139142);
			}
			return new Quaternion(x, y, z, w);
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x0002C29C File Offset: 0x0002A49C
		public static Rect FromStringRect(string str)
		{
			str = str.TrimStart('(');
			str = str.TrimEnd(')');
			string[] array = str.Split(',', StringSplitOptions.None);
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			float x = Convert.ToSingle(array[0], invariantCulture);
			float y = Convert.ToSingle(array[1], invariantCulture);
			float width = Convert.ToSingle(array[2], invariantCulture);
			float height = Convert.ToSingle(array[3], invariantCulture);
			return new Rect(x, y, width, height);
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x0002C2FE File Offset: 0x0002A4FE
		public static float ParseFloat(string str)
		{
			return float.Parse(str, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0002C30B File Offset: 0x0002A50B
		public static bool ParseBool(string str)
		{
			return bool.Parse(str);
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x0002C313 File Offset: 0x0002A513
		public static ulong ParseULong(string str)
		{
			return ulong.Parse(str, NumberStyles.Any, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x0002C325 File Offset: 0x0002A525
		public static long ParseLong(string str)
		{
			return long.Parse(str, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x0002C332 File Offset: 0x0002A532
		public static double ParseDouble(string str)
		{
			return double.Parse(str, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x0002C33F File Offset: 0x0002A53F
		public static sbyte ParseSByte(string str)
		{
			return sbyte.Parse(str, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x0002C34C File Offset: 0x0002A54C
		public static byte ParseByte(string str)
		{
			return byte.Parse(str, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x0002C359 File Offset: 0x0002A559
		public static ushort ParseUShort(string str)
		{
			return ushort.Parse(str, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x0002C366 File Offset: 0x0002A566
		public static short ParseShort(string str)
		{
			return short.Parse(str, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x0002C374 File Offset: 0x0002A574
		public static Type ParseType(string str)
		{
			if (str == "null" || str == "Null")
			{
				return null;
			}
			Type type = GenTypes.GetTypeInAnyAssembly(str, null);
			if (type == null)
			{
				type = BackCompatibility.GetBackCompatibleTypeDirect(typeof(Type), str);
				if (type == null)
				{
					Log.Error("Could not find a type named " + str);
				}
			}
			return type;
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0002C3DC File Offset: 0x0002A5DC
		public static Action ParseAction(string str)
		{
			string[] array = str.Split('.', StringSplitOptions.None);
			string methodName = array[array.Length - 1];
			string typeName;
			if (array.Length == 3)
			{
				typeName = array[0] + "." + array[1];
			}
			else
			{
				typeName = array[0];
			}
			MethodInfo method = GenTypes.GetTypeInAnyAssembly(typeName, null).GetMethods().First((MethodInfo m) => m.Name == methodName);
			return (Action)Delegate.CreateDelegate(typeof(Action), method);
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x0002C458 File Offset: 0x0002A658
		public static Color ParseColor(string str)
		{
			str = str.TrimStart(ParseHelper.colorTrimStartParameters);
			str = str.TrimEnd(ParseHelper.colorTrimEndParameters);
			string[] array = str.Split(',', StringSplitOptions.None);
			float num = ParseHelper.ParseFloat(array[0]);
			float num2 = ParseHelper.ParseFloat(array[1]);
			float num3 = ParseHelper.ParseFloat(array[2]);
			bool flag = num > 1f || num3 > 1f || num2 > 1f;
			float num4 = (float)(flag ? 255 : 1);
			if (array.Length == 4)
			{
				num4 = ParseHelper.FromString<float>(array[3]);
			}
			Color result;
			if (!flag)
			{
				result.r = num;
				result.g = num2;
				result.b = num3;
				result.a = num4;
			}
			else
			{
				result = GenColor.FromBytes(Mathf.RoundToInt(num), Mathf.RoundToInt(num2), Mathf.RoundToInt(num3), Mathf.RoundToInt(num4));
			}
			return result;
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0002C524 File Offset: 0x0002A724
		public static PublishedFileId_t ParsePublishedFileId(string str)
		{
			return new PublishedFileId_t(ulong.Parse(str));
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0002C531 File Offset: 0x0002A731
		public static IntVec2 ParseIntVec2(string str)
		{
			return IntVec2.FromString(str);
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x0002C539 File Offset: 0x0002A739
		public static IntVec3 ParseIntVec3(string str)
		{
			return IntVec3.FromString(str);
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x0002C541 File Offset: 0x0002A741
		public static Rot4 ParseRot4(string str)
		{
			return Rot4.FromString(str);
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x0002C549 File Offset: 0x0002A749
		public static CellRect ParseCellRect(string str)
		{
			return CellRect.FromString(str);
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0002C551 File Offset: 0x0002A751
		public static CurvePoint ParseCurvePoint(string str)
		{
			return CurvePoint.FromString(str);
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0002C559 File Offset: 0x0002A759
		public static NameTriple ParseNameTriple(string str)
		{
			return NameTriple.FromString(str, false);
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0002C562 File Offset: 0x0002A762
		public static FloatRange ParseFloatRange(string str)
		{
			return FloatRange.FromString(str);
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0002C56A File Offset: 0x0002A76A
		public static IntRange ParseIntRange(string str)
		{
			return IntRange.FromString(str);
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x0002C572 File Offset: 0x0002A772
		public static QualityRange ParseQualityRange(string str)
		{
			return QualityRange.FromString(str);
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x0002C57C File Offset: 0x0002A77C
		public static ColorInt ParseColorInt(string str)
		{
			str = str.TrimStart(ParseHelper.colorTrimStartParameters);
			str = str.TrimEnd(ParseHelper.colorTrimEndParameters);
			string[] array = str.Split(',', StringSplitOptions.None);
			ColorInt result = new ColorInt(255, 255, 255, 255);
			result.r = ParseHelper.ParseIntPermissive(array[0]);
			result.g = ParseHelper.ParseIntPermissive(array[1]);
			result.b = ParseHelper.ParseIntPermissive(array[2]);
			if (array.Length == 4)
			{
				result.a = ParseHelper.ParseIntPermissive(array[3]);
			}
			else
			{
				result.a = 255;
			}
			return result;
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x0002C619 File Offset: 0x0002A819
		public static TaggedString ParseTaggedString(string str)
		{
			return str;
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x0002C621 File Offset: 0x0002A821
		public static PlanetTile ParsePlanetTile(string str)
		{
			return PlanetTile.FromString(str);
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x0002C62C File Offset: 0x0002A82C
		static ParseHelper()
		{
			ParseHelper.Parsers<string>.Register(new Func<string, string>(ParseHelper.ParseString));
			ParseHelper.Parsers<bool>.Register(new Func<string, bool>(ParseHelper.ParseBool));
			ParseHelper.Parsers<sbyte>.Register(new Func<string, sbyte>(ParseHelper.ParseSByte));
			ParseHelper.Parsers<byte>.Register(new Func<string, byte>(ParseHelper.ParseByte));
			ParseHelper.Parsers<uint>.Register(new Func<string, uint>(ParseHelper.ParseUInt));
			ParseHelper.Parsers<int>.Register(new Func<string, int>(ParseHelper.ParseIntPermissive));
			ParseHelper.Parsers<ushort>.Register(new Func<string, ushort>(ParseHelper.ParseUShort));
			ParseHelper.Parsers<short>.Register(new Func<string, short>(ParseHelper.ParseShort));
			ParseHelper.Parsers<ulong>.Register(new Func<string, ulong>(ParseHelper.ParseULong));
			ParseHelper.Parsers<long>.Register(new Func<string, long>(ParseHelper.ParseLong));
			ParseHelper.Parsers<float>.Register(new Func<string, float>(ParseHelper.ParseFloat));
			ParseHelper.Parsers<double>.Register(new Func<string, double>(ParseHelper.ParseDouble));
			ParseHelper.Parsers<Vector3>.Register(new Func<string, Vector3>(ParseHelper.FromStringVector3));
			ParseHelper.Parsers<Vector2>.Register(new Func<string, Vector2>(ParseHelper.FromStringVector2));
			ParseHelper.Parsers<Vector4>.Register(new Func<string, Vector4>(ParseHelper.FromStringVector4Adaptive));
			ParseHelper.Parsers<Quaternion>.Register(new Func<string, Quaternion>(ParseHelper.FromStringQuaternion));
			ParseHelper.Parsers<Rect>.Register(new Func<string, Rect>(ParseHelper.FromStringRect));
			ParseHelper.Parsers<Type>.Register(new Func<string, Type>(ParseHelper.ParseType));
			ParseHelper.Parsers<Action>.Register(new Func<string, Action>(ParseHelper.ParseAction));
			ParseHelper.Parsers<Color>.Register(new Func<string, Color>(ParseHelper.ParseColor));
			ParseHelper.Parsers<PublishedFileId_t>.Register(new Func<string, PublishedFileId_t>(ParseHelper.ParsePublishedFileId));
			ParseHelper.Parsers<IntVec2>.Register(new Func<string, IntVec2>(ParseHelper.ParseIntVec2));
			ParseHelper.Parsers<IntVec3>.Register(new Func<string, IntVec3>(ParseHelper.ParseIntVec3));
			ParseHelper.Parsers<Rot4>.Register(new Func<string, Rot4>(ParseHelper.ParseRot4));
			ParseHelper.Parsers<CellRect>.Register(new Func<string, CellRect>(ParseHelper.ParseCellRect));
			ParseHelper.Parsers<CurvePoint>.Register(new Func<string, CurvePoint>(ParseHelper.ParseCurvePoint));
			ParseHelper.Parsers<NameTriple>.Register(new Func<string, NameTriple>(ParseHelper.ParseNameTriple));
			ParseHelper.Parsers<FloatRange>.Register(new Func<string, FloatRange>(ParseHelper.ParseFloatRange));
			ParseHelper.Parsers<IntRange>.Register(new Func<string, IntRange>(ParseHelper.ParseIntRange));
			ParseHelper.Parsers<QualityRange>.Register(new Func<string, QualityRange>(ParseHelper.ParseQualityRange));
			ParseHelper.Parsers<ColorInt>.Register(new Func<string, ColorInt>(ParseHelper.ParseColorInt));
			ParseHelper.Parsers<TaggedString>.Register(new Func<string, TaggedString>(ParseHelper.ParseTaggedString));
			ParseHelper.Parsers<PlanetTile>.Register(new Func<string, PlanetTile>(ParseHelper.ParsePlanetTile));
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x0002C89C File Offset: 0x0002AA9C
		public static T FromString<T>(string str)
		{
			Func<string, T> parser = ParseHelper.Parsers<T>.parser;
			if (parser != null)
			{
				return parser(str);
			}
			return (T)((object)ParseHelper.FromString(str, typeof(T)));
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x0002C8D0 File Offset: 0x0002AAD0
		public static object FromString(string str, Type itemType)
		{
			object result;
			try
			{
				itemType = (Nullable.GetUnderlyingType(itemType) ?? itemType);
				if (itemType.IsEnum)
				{
					try
					{
						object obj = BackCompatibility.BackCompatibleEnum(itemType, str);
						if (obj != null)
						{
							return obj;
						}
						return Enum.Parse(itemType, str);
					}
					catch (ArgumentException innerException)
					{
						string[] array = new string[5];
						array[0] = "'";
						array[1] = str;
						array[2] = "' is not a valid value for ";
						int num = 3;
						Type type = itemType;
						array[num] = ((type != null) ? type.ToString() : null);
						array[4] = ". Valid values are: \n";
						throw new ArgumentException(string.Concat(array) + GenText.StringFromEnumerable(Enum.GetValues(itemType)), innerException);
					}
				}
				Func<string, object> func;
				if (ParseHelper.parsers.TryGetValue(itemType, out func))
				{
					result = func(str);
				}
				else if (GenTypes.IsSlateRef(itemType))
				{
					ISlateRef slateRef = (ISlateRef)Activator.CreateInstance(itemType);
					slateRef.SlateRef = str;
					result = slateRef;
				}
				else
				{
					if (!itemType.IsSubclassOf(typeof(Delegate)))
					{
						throw new ArgumentException(string.Concat(new string[]
						{
							"Trying to parse to unknown data type ",
							itemType.Name,
							". Content is '",
							str,
							"'."
						}));
					}
					int num2 = str.LastIndexOf('.');
					string typeName = (num2 >= 0) ? str.Substring(0, num2) : "";
					string method = str.Substring(num2 + 1, str.Length - (num2 + 1));
					Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(typeName, null);
					result = Delegate.CreateDelegate(itemType, typeInAnyAssembly, method);
				}
			}
			catch (Exception innerException2)
			{
				string[] array2 = new string[5];
				array2[0] = "Exception parsing ";
				int num3 = 1;
				Type type2 = itemType;
				array2[num3] = ((type2 != null) ? type2.ToString() : null);
				array2[2] = " from \"";
				array2[3] = str;
				array2[4] = "\"";
				throw new ArgumentException(string.Concat(array2), innerException2);
			}
			return result;
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x0002CAA8 File Offset: 0x0002ACA8
		public static bool HandlesType(Type type)
		{
			type = (Nullable.GetUnderlyingType(type) ?? type);
			return type.IsPrimitive || type.IsEnum || ParseHelper.parsers.ContainsKey(type) || type.IsSubclassOf(typeof(Delegate)) || GenTypes.IsSlateRef(type);
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x0002CAFC File Offset: 0x0002ACFC
		public static bool CanParse(Type type, string str)
		{
			if (!ParseHelper.HandlesType(type))
			{
				return false;
			}
			try
			{
				ParseHelper.FromString(str, type);
			}
			catch (ArgumentException)
			{
				return false;
			}
			catch (FormatException)
			{
				return false;
			}
			return true;
		}

		// Token: 0x040004A9 RID: 1193
		private static Dictionary<Type, Func<string, object>> parsers = new Dictionary<Type, Func<string, object>>();

		// Token: 0x040004AA RID: 1194
		private static readonly char[] colorTrimStartParameters = new char[]
		{
			'(',
			'R',
			'G',
			'B',
			'A'
		};

		// Token: 0x040004AB RID: 1195
		private static readonly char[] colorTrimEndParameters = new char[]
		{
			')'
		};

		// Token: 0x0200012D RID: 301
		public static class Parsers<T>
		{
			// Token: 0x0600095D RID: 2397 RVA: 0x0002CB48 File Offset: 0x0002AD48
			public static void Register(Func<string, T> method)
			{
				ParseHelper.Parsers<T>.parser = method;
				ParseHelper.parsers.Add(typeof(T), (string str) => method(str));
			}

			// Token: 0x040004AC RID: 1196
			public static Func<string, T> parser;

			// Token: 0x040004AD RID: 1197
			public static readonly string profilerLabel = "ParseHelper.FromString<" + typeof(T).FullName + ">()";
		}
	}
}
