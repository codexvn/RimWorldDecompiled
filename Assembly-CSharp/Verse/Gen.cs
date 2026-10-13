using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000E3 RID: 227
	public static class Gen
	{
		// Token: 0x06000640 RID: 1600 RVA: 0x0001EBCC File Offset: 0x0001CDCC
		public static Vector3 AveragePosition(List<IntVec3> cells)
		{
			return new Vector3((float)cells.Average((IntVec3 c) => c.x) + 0.5f, 0f, (float)cells.Average((IntVec3 c) => c.z) + 0.5f);
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x0001EC3C File Offset: 0x0001CE3C
		public static T RandomEnumValue<T>(bool disallowFirstValue)
		{
			int minInclusive = disallowFirstValue ? 1 : 0;
			T[] array = (T[])Enum.GetValues(typeof(T));
			int num = Rand.Range(minInclusive, array.Length);
			return array[num];
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x0001EC75 File Offset: 0x0001CE75
		public static Vector3 RandomHorizontalVector(float max)
		{
			return new Vector3(Rand.Range(-max, max), 0f, Rand.Range(-max, max));
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x0001EC91 File Offset: 0x0001CE91
		public static Vector3 Random2DVector(Vector3 max)
		{
			return new Vector3(Rand.Range(-max.x, max.x), 0f, Rand.Range(-max.z, max.z));
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x0001ECC4 File Offset: 0x0001CEC4
		public static int GetBitCountOf(long lValue)
		{
			int num = 0;
			while (lValue != 0L)
			{
				lValue &= lValue - 1L;
				num++;
			}
			return num;
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x0001ECE5 File Offset: 0x0001CEE5
		public static IEnumerable<T> GetAllSelectedItems<T>(this Enum value)
		{
			CultureInfo cult = CultureInfo.InvariantCulture;
			int valueAsInt = Convert.ToInt32(value, cult);
			foreach (object obj in Enum.GetValues(typeof(T)))
			{
				int num = Convert.ToInt32(obj, cult);
				if (num == (valueAsInt & num))
				{
					yield return (T)((object)obj);
				}
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x0001ECF5 File Offset: 0x0001CEF5
		public static IEnumerable<T> YieldSingle<T>(T val)
		{
			yield return val;
			yield break;
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x0001ED05 File Offset: 0x0001CF05
		public static IEnumerable YieldSingleNonGeneric<T>(T val)
		{
			yield return val;
			yield break;
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x0001ED18 File Offset: 0x0001CF18
		public static string ToStringSafe<T>(this T obj)
		{
			if (obj == null)
			{
				return "null";
			}
			string result;
			try
			{
				result = obj.ToString();
			}
			catch (Exception ex)
			{
				int num = 0;
				bool flag = false;
				try
				{
					num = obj.GetHashCode();
					flag = true;
				}
				catch
				{
				}
				if (flag)
				{
					string str = "Exception in ToString(): ";
					Exception ex2 = ex;
					Log.ErrorOnce(str + ((ex2 != null) ? ex2.ToString() : null), num ^ 1857461521);
				}
				else
				{
					string str2 = "Exception in ToString(): ";
					Exception ex3 = ex;
					Log.Error(str2 + ((ex3 != null) ? ex3.ToString() : null));
				}
				result = "error";
			}
			return result;
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x0001EDC8 File Offset: 0x0001CFC8
		public static string ToStringSafeEnumerable(this IEnumerable enumerable)
		{
			if (enumerable == null)
			{
				return "null";
			}
			string result;
			try
			{
				string text = "";
				foreach (object obj in enumerable)
				{
					if (text.Length > 0)
					{
						text += ", ";
					}
					text += obj.ToStringSafe<object>();
				}
				result = text;
			}
			catch (Exception ex)
			{
				int num = 0;
				bool flag = false;
				try
				{
					num = enumerable.GetHashCode();
					flag = true;
				}
				catch
				{
				}
				if (flag)
				{
					string str = "Exception while enumerating: ";
					Exception ex2 = ex;
					Log.ErrorOnce(str + ((ex2 != null) ? ex2.ToString() : null), num ^ 581736153);
				}
				else
				{
					string str2 = "Exception while enumerating: ";
					Exception ex3 = ex;
					Log.Error(str2 + ((ex3 != null) ? ex3.ToString() : null));
				}
				result = "error";
			}
			return result;
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x0001EECC File Offset: 0x0001D0CC
		public static void Swap<T>(ref T x, ref T y)
		{
			T t = y;
			y = x;
			x = t;
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x0001EEF3 File Offset: 0x0001D0F3
		public static T MemberwiseClone<T>(T obj)
		{
			if (Gen.s_memberwiseClone == null)
			{
				Gen.s_memberwiseClone = typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
			}
			return (T)((object)Gen.s_memberwiseClone.Invoke(obj, null));
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x0001EF34 File Offset: 0x0001D134
		public static void MemberwiseShallowCopy(object from, object to)
		{
			foreach (FieldInfo fieldInfo in from.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				fieldInfo.SetValue(to, fieldInfo.GetValue(from));
			}
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x0001EF70 File Offset: 0x0001D170
		public static int FixedTimeStepUpdate(ref float timeBuffer, float fps)
		{
			timeBuffer += Mathf.Min(Time.deltaTime, 1f);
			float num = 1f / fps;
			int num2 = Mathf.FloorToInt(timeBuffer / num);
			timeBuffer -= (float)num2 * num;
			return num2;
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x0001EFB0 File Offset: 0x0001D1B0
		public static int HashCombine<T>(int seed, T obj)
		{
			int num = (obj == null) ? 0 : obj.GetHashCode();
			return (int)((long)seed ^ (long)num + (long)((ulong)-1640531527) + (long)((long)seed << 6) + (long)(seed >> 2));
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0001EFED File Offset: 0x0001D1ED
		public static int HashCombineStruct<T>(int seed, T obj) where T : struct
		{
			return (int)((long)seed ^ (long)obj.GetHashCode() + (long)((ulong)-1640531527) + (long)((long)seed << 6) + (long)(seed >> 2));
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x0001F012 File Offset: 0x0001D212
		public static int HashCombineInt(int seed, int value)
		{
			return (int)((long)seed ^ (long)value + (long)((ulong)-1640531527) + (long)((long)seed << 6) + (long)(seed >> 2));
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x0001F02C File Offset: 0x0001D22C
		public static int HashCombineInt(int v1, int v2, int v3, int v4)
		{
			int num = 352654597;
			int num2 = num;
			num = ((num << 5) + num + (num >> 27) ^ v1);
			num2 = ((num2 << 5) + num2 + (num2 >> 27) ^ v2);
			num = ((num << 5) + num + (num >> 27) ^ v3);
			num2 = ((num2 << 5) + num2 + (num2 >> 27) ^ v4);
			return num + num2 * 1566083941;
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0001F07E File Offset: 0x0001D27E
		public static int HashOffset(this int baseInt)
		{
			return Gen.HashCombineInt(baseInt, 169495093);
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x0001F08B File Offset: 0x0001D28B
		public static int HashOffset(this Thing t)
		{
			return t.thingIDNumber.HashOffset();
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x0001F098 File Offset: 0x0001D298
		public static int HashOffset(this WorldObject o)
		{
			return o.ID.HashOffset();
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x0001F0A5 File Offset: 0x0001D2A5
		public static bool IsHashIntervalTick(this Map m, int interval)
		{
			return m.HashOffsetTicks() % interval == 0;
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x0001F0B2 File Offset: 0x0001D2B2
		public static bool IsHashIntervalTick(this Map m, int interval, int offset)
		{
			return (m.HashOffsetTicks() + offset) % interval == 0;
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x0001F0C1 File Offset: 0x0001D2C1
		public static int HashOffsetTicks(this Map m)
		{
			return Find.TickManager.TicksGame + m.uniqueID.HashOffset();
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x0001F0D9 File Offset: 0x0001D2D9
		public static bool IsHashIntervalTick(this Thing t, int interval)
		{
			return t.HashOffsetTicks() % interval == 0;
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x0001F0E6 File Offset: 0x0001D2E6
		public static bool IsHashIntervalTick(this Thing t, int interval, int delta)
		{
			return Math.Abs(t.HashOffsetTicks() % interval) < delta;
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x0001F0F8 File Offset: 0x0001D2F8
		public static int HashOffsetTicks(this Thing t)
		{
			return Find.TickManager.TicksGame + t.thingIDNumber.HashOffset();
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x0001F110 File Offset: 0x0001D310
		public static bool IsHashIntervalTick(this WorldObject o, int interval)
		{
			if (interval <= 0)
			{
				return true;
			}
			int num = o.HashOffsetTicks() % interval;
			if (num < 0)
			{
				num += interval;
			}
			return num == 0;
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x0001F138 File Offset: 0x0001D338
		public static bool IsHashIntervalTick(this WorldObject o, int interval, int delta)
		{
			if (interval <= 0)
			{
				return true;
			}
			if (delta <= 0)
			{
				return false;
			}
			int num = o.HashOffsetTicks() % interval;
			if (num < 0)
			{
				num += interval;
			}
			return num < delta;
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x0001F166 File Offset: 0x0001D366
		public static int HashOffsetTicks(this WorldObject o)
		{
			return Find.TickManager.TicksGame + o.ID.HashOffset();
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x0001F17E File Offset: 0x0001D37E
		public static bool IsHashIntervalTick(this Faction f, int interval)
		{
			return f.HashOffsetTicks() % interval == 0;
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x0001F18B File Offset: 0x0001D38B
		public static int HashOffsetTicks(this Faction f)
		{
			return Find.TickManager.TicksGame + f.randomKey.HashOffset();
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x0001F1A3 File Offset: 0x0001D3A3
		public static int HashOrderless(int v1, int v2)
		{
			return Gen.HashCombineInt(Math.Min(v1, v2), Math.Max(v1, v2));
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x0001F1B8 File Offset: 0x0001D3B8
		public static bool IsNestedHashIntervalTick(this Thing t, int outerInterval, int approxInnerInterval)
		{
			int num = Mathf.Max(Mathf.RoundToInt((float)approxInnerInterval / (float)outerInterval), 1);
			return t.HashOffsetTicks() / outerInterval % num == 0;
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x0001F1E4 File Offset: 0x0001D3E4
		public static void ReplaceNullFields<T>(ref T replaceIn, T replaceWith)
		{
			if (replaceIn == null || replaceWith == null)
			{
				return;
			}
			foreach (FieldInfo fieldInfo in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (fieldInfo.GetValue(replaceIn) == null)
				{
					object value = fieldInfo.GetValue(replaceWith);
					if (value != null)
					{
						object obj = replaceIn;
						fieldInfo.SetValue(obj, value);
						replaceIn = (T)((object)obj);
					}
				}
			}
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x0001F274 File Offset: 0x0001D474
		public static void EnsureAllFieldsNullable(Type type)
		{
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				Type fieldType = fieldInfo.FieldType;
				if (fieldType.IsValueType && !(Nullable.GetUnderlyingType(fieldType) != null))
				{
					Log.Warning(string.Concat(new string[]
					{
						"Field ",
						type.Name,
						".",
						fieldInfo.Name,
						" is not nullable."
					}));
				}
			}
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x0001F2F4 File Offset: 0x0001D4F4
		public static string GetNonNullFieldsDebugInfo(object obj)
		{
			if (obj == null)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (FieldInfo fieldInfo in obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				object value = fieldInfo.GetValue(obj);
				if (value != null)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(fieldInfo.Name + "=" + value.ToStringSafe<object>());
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x0001F376 File Offset: 0x0001D576
		public static bool InBounds<T>(this T[,] array, int i, int j)
		{
			return i >= 0 && j >= 0 && i < array.GetLength(0) && j < array.GetLength(1);
		}

		// Token: 0x04000333 RID: 819
		private static MethodInfo s_memberwiseClone;
	}
}
