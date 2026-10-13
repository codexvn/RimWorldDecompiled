using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Verse
{
	// Token: 0x0200010D RID: 269
	public static class GenList
	{
		// Token: 0x060007E8 RID: 2024 RVA: 0x0002563B File Offset: 0x0002383B
		public static int CountAllowNull<T>(this IList<T> list)
		{
			if (list == null)
			{
				return 0;
			}
			return list.Count;
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x00025648 File Offset: 0x00023848
		public static bool NullOrEmpty<T>(this IList<T> list)
		{
			return list == null || list.Count == 0;
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x00025658 File Offset: 0x00023858
		public static bool HasData<T>(this IList<T> list)
		{
			return list != null && list.Count >= 1;
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0002566C File Offset: 0x0002386C
		public static List<T> ListFullCopy<T>(this List<T> source)
		{
			List<T> list = new List<T>(source.Count);
			for (int i = 0; i < source.Count; i++)
			{
				list.Add(source[i]);
			}
			return list;
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x000256A4 File Offset: 0x000238A4
		public static List<T> ListFullCopyOrNull<T>(this List<T> source)
		{
			if (source == null)
			{
				return null;
			}
			return source.ListFullCopy<T>();
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x000256B4 File Offset: 0x000238B4
		public static void ClearValueLists<T, J>(this Dictionary<T, List<J>> source)
		{
			foreach (KeyValuePair<T, List<J>> keyValuePair in source)
			{
				keyValuePair.Value.Clear();
			}
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x00025708 File Offset: 0x00023908
		public static void ClearAndPoolValueLists<T, V>(this Dictionary<T, List<V>> source)
		{
			foreach (List<V> list in source.Values)
			{
				list.Clear();
				SimplePool<List<V>>.Return(list);
			}
			source.Clear();
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x00025764 File Offset: 0x00023964
		public static void RemoveDuplicates<T>(this List<T> list, Func<T, T, bool> comparer = null) where T : class
		{
			if (list.Count <= 1)
			{
				return;
			}
			for (int i = list.Count - 1; i >= 0; i--)
			{
				for (int j = 0; j < i; j++)
				{
					if ((comparer == null && list[i] == list[j]) || (comparer != null && comparer(list[i], list[j])))
					{
						list.RemoveAt(i);
						break;
					}
				}
			}
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x000257D9 File Offset: 0x000239D9
		public static void TruncateToLength<T>(this List<T> list, int maxLength)
		{
			if (list.Count == 0 || list.Count <= maxLength)
			{
				return;
			}
			list.RemoveRange(maxLength, list.Count - maxLength);
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x000257FC File Offset: 0x000239FC
		public static void Shuffle<T>(this IList<T> list)
		{
			int i = list.Count;
			while (i > 1)
			{
				i--;
				int num = Rand.RangeInclusive(0, i);
				int index = num;
				int index2 = i;
				T value = list[i];
				T value2 = list[num];
				list[index] = value;
				list[index2] = value2;
			}
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x00025854 File Offset: 0x00023A54
		public static void InsertionSort<T>(this IList<T> list, Comparison<T> comparison)
		{
			int count = list.Count;
			for (int i = 1; i < count; i++)
			{
				T t = list[i];
				int num = i - 1;
				while (num >= 0 && comparison(list[num], t) > 0)
				{
					list[num + 1] = list[num];
					num--;
				}
				list[num + 1] = t;
			}
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x000258B5 File Offset: 0x00023AB5
		public static bool NotNullAndContains<T>(this IList<T> list, T element)
		{
			return !list.NullOrEmpty<T>() && list.Contains(element);
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x000258C8 File Offset: 0x00023AC8
		public static bool NotNullAndContainsAnyElement<T>(this IList<T> list, IList<T> other)
		{
			if (list.NullOrEmpty<T>() || other.NullOrEmpty<T>())
			{
				return false;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (other.Contains(list[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x0002590C File Offset: 0x00023B0C
		public static bool RemoveAll_IfNotAll<T, ContextType>(this List<T> list, ContextType context, Predicate<ContextType, T> predicate)
		{
			int num = list.Count - 1;
			while (num >= 0 && !predicate(context, list[num]))
			{
				num--;
			}
			if (num < 0)
			{
				return false;
			}
			T item = list[num];
			list.RemoveRange(num, list.Count - num);
			list.RemoveAll(context, predicate, true);
			list.Add(item);
			return true;
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x0002596C File Offset: 0x00023B6C
		public static int RemoveAll<T, ContextType>(this List<T> list, ContextType context, Predicate<ContextType, T> predicate, bool negatePredicate = false)
		{
			bool flag = !negatePredicate;
			int num = 0;
			while (num < list.Count && predicate(context, list[num]) != flag)
			{
				num++;
			}
			if (num == list.Count)
			{
				return 0;
			}
			for (int i = num + 1; i < list.Count; i++)
			{
				if (predicate(context, list[i]) != flag)
				{
					list[num++] = list[i];
				}
			}
			int num2 = list.Count - num;
			list.RemoveRange(num, num2);
			return num2;
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x000259F4 File Offset: 0x00023BF4
		public static void Swap<T>(this IList<T> list, int a, int b)
		{
			T value = list[b];
			T value2 = list[a];
			list[a] = value;
			list[b] = value2;
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x00025A2F File Offset: 0x00023C2F
		public static T GetLast<T>(this IList<T> source)
		{
			if (source.Count == 0)
			{
				throw new IndexOutOfRangeException("Attempted to get the last element of a list with zero length.");
			}
			return source[source.Count - 1];
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x00025A52 File Offset: 0x00023C52
		public static Span<T> AsSpan<T>(this List<T> list)
		{
			return new Span<T>(list.UnsafeGetBackingArray<T>(), 0, list.Count);
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x00025A66 File Offset: 0x00023C66
		public static Span<T> AsSpan<T>(this List<T> list, int length)
		{
			return new Span<T>(list.UnsafeGetBackingArray<T>(), 0, length);
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x00025A75 File Offset: 0x00023C75
		public static ReadOnlySpan<T> AsReadOnlySpan<T>(this List<T> list)
		{
			return new ReadOnlySpan<T>(list.UnsafeGetBackingArray<T>(), 0, list.Count);
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x00025A89 File Offset: 0x00023C89
		public static void UnsafeSetCount<T>(this List<T> list, int count)
		{
			FieldInfo field = typeof(List<T>).GetField("_size", BindingFlags.Instance | BindingFlags.NonPublic);
			if (field == null)
			{
				throw new InvalidOperationException("Failed to find the backing field for List<T> size.");
			}
			field.SetValue(list, count);
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00025AC1 File Offset: 0x00023CC1
		private unsafe static T[] UnsafeGetBackingArray<T>(this List<T> list)
		{
			return UnsafeUtility.As<List<T>, StrongBox<T[]>>(ref list)->Value;
		}
	}
}
