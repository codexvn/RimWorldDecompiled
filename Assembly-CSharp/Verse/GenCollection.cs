using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000F2 RID: 242
	public static class GenCollection
	{
		// Token: 0x060006F4 RID: 1780 RVA: 0x00021A9C File Offset: 0x0001FC9C
		public static void AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> enumerable)
		{
			foreach (T item in enumerable)
			{
				hashSet.Add(item);
			}
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x00021AE8 File Offset: 0x0001FCE8
		public static bool NullOrEmpty<T>(this HashSet<T> hashSet)
		{
			return hashSet == null || !hashSet.Any<T>();
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x00021AF8 File Offset: 0x0001FCF8
		public static bool Empty<T>(this Queue<T> queue)
		{
			return queue.Count == 0;
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x00021B04 File Offset: 0x0001FD04
		public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dest, IDictionary<TKey, TValue> source)
		{
			foreach (KeyValuePair<TKey, TValue> keyValuePair in source)
			{
				dest.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x00021B5C File Offset: 0x0001FD5C
		public static void RemoveRange<TKey, TValue>(this IDictionary<TKey, TValue> dict, ICollection<TKey> remove)
		{
			foreach (TKey key in remove)
			{
				if (dict.ContainsKey(key))
				{
					dict.Remove(key);
				}
			}
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x00021BB0 File Offset: 0x0001FDB0
		public static void SetOrAdd<K, V>(this Dictionary<K, V> dict, K key, V value)
		{
			dict[key] = value;
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x00021BBA File Offset: 0x0001FDBA
		public static void AddDistinct<K, V>(this Dictionary<K, V> dict, K key, V value)
		{
			dict.TryAdd(key, value);
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x00021BC8 File Offset: 0x0001FDC8
		public static void Increment<K>(this Dictionary<K, int> dict, K key)
		{
			if (!dict.TryAdd(key, 1))
			{
				int num = dict[key];
				dict[key] = num + 1;
			}
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00021BF4 File Offset: 0x0001FDF4
		public static bool SharesElementWith<T>(this IEnumerable<T> source, IEnumerable<T> other)
		{
			IList<T> list = source as IList<T>;
			if (list != null)
			{
				IList<T> list2 = other as IList<T>;
				if (list2 != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						for (int j = 0; j < list2.Count; j++)
						{
							if (EqualityComparer<T>.Default.Equals(list[i], list2[j]))
							{
								return true;
							}
						}
					}
					return false;
				}
			}
			return source.Any(new Func<T, bool>(other.Contains<T>));
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x00021C67 File Offset: 0x0001FE67
		public static IEnumerable<T> InRandomOrder<T>(this IEnumerable<T> source, IList<T> workingList = null)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (workingList == null)
			{
				workingList = source.ToList<T>();
			}
			else
			{
				workingList.Clear();
				foreach (T item in source)
				{
					workingList.Add(item);
				}
			}
			int num;
			for (int countUnChosen = workingList.Count; countUnChosen > 0; countUnChosen = num - 1)
			{
				int rand = Rand.Range(0, countUnChosen);
				yield return workingList[rand];
				IList<T> list = workingList;
				int index = rand;
				IList<T> list2 = workingList;
				num = countUnChosen - 1;
				T value = workingList[countUnChosen - 1];
				T value2 = workingList[rand];
				list[index] = value;
				list2[num] = value2;
				num = countUnChosen;
			}
			yield break;
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x00021C80 File Offset: 0x0001FE80
		public static T RandomElement<T>(this HashSet<T> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (source.Count == 0)
			{
				Log.Warning("Getting random element from empty hashset.");
				return default(T);
			}
			HashSet<T>.Enumerator enumerator = source.GetEnumerator();
			int num = Rand.Range(0, source.Count);
			T result = default(T);
			while (enumerator.MoveNext())
			{
				if (num == 0)
				{
					result = enumerator.Current;
					break;
				}
				num--;
			}
			enumerator.Dispose();
			return result;
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x00021CF8 File Offset: 0x0001FEF8
		public static T RandomElement<T>(this IEnumerable<T> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			ICollection<T> collection = source as ICollection<T>;
			if (collection != null)
			{
				if (collection.Count == 0)
				{
					Log.Warning("Getting random element from empty collection.");
					return default(T);
				}
				return source.ElementAt(Rand.Range(0, collection.Count));
			}
			else
			{
				List<T> list = source.ToList<T>();
				if (list.Count == 0)
				{
					Log.Warning("Getting random element from empty collection.");
					return default(T);
				}
				return list[Rand.Range(0, list.Count)];
			}
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x00021D84 File Offset: 0x0001FF84
		public static T RandomElementWithFallback<T>(this IEnumerable<T> source, T fallback = default(T))
		{
			T result;
			if (source.TryRandomElement(out result))
			{
				return result;
			}
			return fallback;
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x00021DA0 File Offset: 0x0001FFA0
		public static bool TryRandomElement<T>(this IEnumerable<T> source, Predicate<T> predicate, out T result)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			IList<T> list = source as IList<T>;
			if (list != null)
			{
				if (list.Count == 0)
				{
					result = default(T);
					return false;
				}
			}
			else
			{
				list = source.ToList<T>();
				if (!list.Any<T>())
				{
					result = default(T);
					return false;
				}
			}
			if (!list.Any((T x) => predicate(x)))
			{
				result = default(T);
				return false;
			}
			result = (from x in list
			where predicate(x)
			select x).RandomElement<T>();
			return true;
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x00021E38 File Offset: 0x00020038
		public static bool TryRandomElement<T>(this HashSet<T> source, out T result)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (source.Count == 0)
			{
				result = default(T);
				return false;
			}
			HashSet<T>.Enumerator enumerator = source.GetEnumerator();
			int num = Rand.Range(0, source.Count);
			result = default(T);
			while (enumerator.MoveNext())
			{
				if (num == 0)
				{
					result = enumerator.Current;
					break;
				}
				num--;
			}
			enumerator.Dispose();
			return true;
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x00021EA8 File Offset: 0x000200A8
		public static bool TryRandomElement<T>(this IEnumerable<T> source, out T result)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			ICollection<T> collection = source as ICollection<T>;
			if (collection != null)
			{
				if (collection.Count == 0)
				{
					result = default(T);
					return false;
				}
			}
			else
			{
				collection = source.ToList<T>();
				if (!collection.Any<T>())
				{
					result = default(T);
					return false;
				}
			}
			result = collection.RandomElement<T>();
			return true;
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x00021F04 File Offset: 0x00020104
		public static T RandomElementByWeight<T>(this IEnumerable<T> source, Func<T, float> weightSelector)
		{
			float num = 0f;
			IList<T> list = source as IList<T>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					float num2 = weightSelector(list[i]);
					if (num2 < 0f)
					{
						string str = "Negative weight in selector: ";
						string str2 = num2.ToString();
						string str3 = " from ";
						T t = list[i];
						Log.Error(str + str2 + str3 + ((t != null) ? t.ToString() : null));
						num2 = 0f;
					}
					num += num2;
				}
				if (list.Count == 1 && num > 0f)
				{
					return list[0];
				}
			}
			else
			{
				int num3 = 0;
				foreach (T t2 in source)
				{
					num3++;
					float num4 = weightSelector(t2);
					if (num4 < 0f)
					{
						string str4 = "Negative weight in selector: ";
						string str5 = num4.ToString();
						string str6 = " from ";
						T t = t2;
						Log.Error(str4 + str5 + str6 + ((t != null) ? t.ToString() : null));
						num4 = 0f;
					}
					num += num4;
				}
				if (num3 == 1 && num > 0f)
				{
					return source.First<T>();
				}
			}
			if (num <= 0f)
			{
				Log.Error("RandomElementByWeight with totalWeight=" + num.ToString() + " - use TryRandomElementByWeight.");
				T t = default(T);
				return t;
			}
			float num5 = Rand.Value * num;
			float num6 = 0f;
			if (list != null)
			{
				for (int j = 0; j < list.Count; j++)
				{
					float num7 = weightSelector(list[j]);
					if (num7 > 0f)
					{
						num6 += num7;
						if (num6 >= num5)
						{
							return list[j];
						}
					}
				}
			}
			else
			{
				foreach (T t3 in source)
				{
					float num8 = weightSelector(t3);
					if (num8 > 0f)
					{
						num6 += num8;
						if (num6 >= num5)
						{
							return t3;
						}
					}
				}
			}
			return default(T);
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x00022150 File Offset: 0x00020350
		public static T RandomElementByWeightWithFallback<T>(this IEnumerable<T> source, Func<T, float> weightSelector, T fallback = default(T))
		{
			T result;
			if (source.TryRandomElementByWeight(weightSelector, out result))
			{
				return result;
			}
			return fallback;
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0002216C File Offset: 0x0002036C
		public static bool TryRandomElementByWeight<T>(this IEnumerable<T> source, Func<T, float> weightSelector, out T result)
		{
			IList<T> list = source as IList<T>;
			if (list != null)
			{
				float num = 0f;
				for (int i = 0; i < list.Count; i++)
				{
					float num2 = weightSelector(list[i]);
					if (num2 < 0f)
					{
						string str = "Negative weight in selector: ";
						string str2 = num2.ToString();
						string str3 = " from ";
						T t = list[i];
						Log.Error(str + str2 + str3 + ((t != null) ? t.ToString() : null));
						num2 = 0f;
					}
					num += num2;
				}
				if (list.Count == 1 && num > 0f)
				{
					result = list[0];
					return true;
				}
				if (num == 0f)
				{
					result = default(T);
					return false;
				}
				num *= Rand.Value;
				for (int j = 0; j < list.Count; j++)
				{
					float num3 = weightSelector(list[j]);
					if (num3 > 0f)
					{
						num -= num3;
						if (num <= 0f)
						{
							result = list[j];
							return true;
						}
					}
				}
			}
			IEnumerator<T> enumerator = source.GetEnumerator();
			result = default(T);
			float num4 = 0f;
			while (num4 == 0f && enumerator.MoveNext())
			{
				result = enumerator.Current;
				num4 = weightSelector(result);
				if (num4 < 0f)
				{
					string str4 = "Negative weight in selector: ";
					string str5 = num4.ToString();
					string str6 = " from ";
					T t = result;
					Log.Error(str4 + str5 + str6 + ((t != null) ? t.ToString() : null));
					num4 = 0f;
				}
			}
			if (num4 == 0f)
			{
				result = default(T);
				return false;
			}
			while (enumerator.MoveNext())
			{
				T t2 = enumerator.Current;
				float num5 = weightSelector(t2);
				if (num5 < 0f)
				{
					string str7 = "Negative weight in selector: ";
					string str8 = num5.ToString();
					string str9 = " from ";
					T t = t2;
					Log.Error(str7 + str8 + str9 + ((t != null) ? t.ToString() : null));
					num5 = 0f;
				}
				if (Rand.Range(0f, num4 + num5) >= num4)
				{
					result = t2;
				}
				num4 += num5;
			}
			return true;
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x000223B0 File Offset: 0x000205B0
		public static T RandomElementByWeightWithDefault<T>(this IEnumerable<T> source, Func<T, float> weightSelector, float defaultValueWeight)
		{
			if (defaultValueWeight < 0f)
			{
				Log.Error("Negative default value weight.");
				defaultValueWeight = 0f;
			}
			float num = 0f;
			foreach (T t in source)
			{
				float num2 = weightSelector(t);
				if (num2 < 0f)
				{
					string str = "Negative weight in selector: ";
					string str2 = num2.ToString();
					string str3 = " from ";
					T t2 = t;
					Log.Error(str + str2 + str3 + ((t2 != null) ? t2.ToString() : null));
					num2 = 0f;
				}
				num += num2;
			}
			float num3 = defaultValueWeight + num;
			if (num3 <= 0f)
			{
				Log.Error("RandomElementByWeightWithDefault with totalWeight=" + num3.ToString());
				return default(T);
			}
			if (Rand.Value < defaultValueWeight / num3 || num == 0f)
			{
				return default(T);
			}
			return source.RandomElementByWeight(weightSelector);
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x000224B8 File Offset: 0x000206B8
		public static T FirstOrFallback<T>(this IEnumerable<T> source, T fallback = default(T))
		{
			using (IEnumerator<T> enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			return fallback;
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x00022500 File Offset: 0x00020700
		public static T FirstOrFallback<T>(this IEnumerable<T> source, Func<T, bool> predicate, T fallback = default(T))
		{
			return source.Where(predicate).FirstOrFallback(fallback);
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0002250F File Offset: 0x0002070F
		public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
		{
			return source.MaxBy(selector, Comparer<TKey>.Default);
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x00022520 File Offset: 0x00020720
		public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			TSource result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw new InvalidOperationException("Sequence contains no elements");
				}
				TSource tsource = enumerator.Current;
				TKey y = selector(tsource);
				while (enumerator.MoveNext())
				{
					TSource tsource2 = enumerator.Current;
					TKey tkey = selector(tsource2);
					if (comparer.Compare(tkey, y) > 0)
					{
						tsource = tsource2;
						y = tkey;
					}
				}
				result = tsource;
			}
			return result;
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x000225CC File Offset: 0x000207CC
		public static TSource MaxByWithFallback<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, TSource fallback = default(TSource))
		{
			return source.MaxByWithFallback(selector, Comparer<TKey>.Default, fallback);
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x000225DC File Offset: 0x000207DC
		public static TSource MaxByWithFallback<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer, TSource fallback = default(TSource))
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			TSource result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					result = fallback;
				}
				else
				{
					TSource tsource = enumerator.Current;
					TKey y = selector(tsource);
					while (enumerator.MoveNext())
					{
						TSource tsource2 = enumerator.Current;
						TKey tkey = selector(tsource2);
						if (comparer.Compare(tkey, y) > 0)
						{
							tsource = tsource2;
							y = tkey;
						}
					}
					result = tsource;
				}
			}
			return result;
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x00022684 File Offset: 0x00020884
		public static bool TryMaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, out TSource value)
		{
			return source.TryMaxBy(selector, Comparer<TKey>.Default, out value);
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x00022694 File Offset: 0x00020894
		public static bool TryMaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer, out TSource value)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			bool result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					value = default(TSource);
					result = false;
				}
				else
				{
					TSource tsource = enumerator.Current;
					TKey y = selector(tsource);
					while (enumerator.MoveNext())
					{
						TSource tsource2 = enumerator.Current;
						TKey tkey = selector(tsource2);
						if (comparer.Compare(tkey, y) > 0)
						{
							tsource = tsource2;
							y = tkey;
						}
					}
					value = tsource;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x00022748 File Offset: 0x00020948
		public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
		{
			return source.MinBy(selector, Comparer<TKey>.Default);
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x00022758 File Offset: 0x00020958
		public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			TSource result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw new InvalidOperationException("Sequence contains no elements");
				}
				TSource tsource = enumerator.Current;
				TKey y = selector(tsource);
				while (enumerator.MoveNext())
				{
					TSource tsource2 = enumerator.Current;
					TKey tkey = selector(tsource2);
					if (comparer.Compare(tkey, y) < 0)
					{
						tsource = tsource2;
						y = tkey;
					}
				}
				result = tsource;
			}
			return result;
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x00022804 File Offset: 0x00020A04
		public static bool TryMinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, out TSource value)
		{
			return source.TryMinBy(selector, Comparer<TKey>.Default, out value);
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x00022814 File Offset: 0x00020A14
		public static bool TryMinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer, out TSource value)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			bool result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					value = default(TSource);
					result = false;
				}
				else
				{
					TSource tsource = enumerator.Current;
					TKey y = selector(tsource);
					while (enumerator.MoveNext())
					{
						TSource tsource2 = enumerator.Current;
						TKey tkey = selector(tsource2);
						if (comparer.Compare(tkey, y) < 0)
						{
							tsource = tsource2;
							y = tkey;
						}
					}
					value = tsource;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x000228C8 File Offset: 0x00020AC8
		public static void SortBy<T, TSortBy>(this List<T> list, Func<T, TSortBy> selector) where TSortBy : IComparable<TSortBy>
		{
			if (list.Count <= 1)
			{
				return;
			}
			list.Sort(delegate(T a, T b)
			{
				TSortBy tsortBy = selector(a);
				return tsortBy.CompareTo(selector(b));
			});
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x00022900 File Offset: 0x00020B00
		public static void SortBy<T, TSortBy, TThenBy>(this List<T> list, Func<T, TSortBy> selector, Func<T, TThenBy> thenBySelector) where TSortBy : IComparable<TSortBy>, IEquatable<TSortBy> where TThenBy : IComparable<TThenBy>
		{
			if (list.Count <= 1)
			{
				return;
			}
			list.Sort(delegate(T a, T b)
			{
				TSortBy tsortBy = selector(a);
				TSortBy other = selector(b);
				if (!tsortBy.Equals(other))
				{
					return tsortBy.CompareTo(other);
				}
				TThenBy tthenBy = thenBySelector(a);
				return tthenBy.CompareTo(thenBySelector(b));
			});
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x00022940 File Offset: 0x00020B40
		public static void SortBy<T, TSortBy, TThenBy, TThenBy2>(this List<T> list, Func<T, TSortBy> selector, Func<T, TThenBy> thenBySelector, Func<T, TThenBy2> thenBy2Selector) where TSortBy : IComparable<TSortBy>, IEquatable<TSortBy> where TThenBy : IComparable<TThenBy>, IEquatable<TThenBy> where TThenBy2 : IComparable<TThenBy2>
		{
			if (list.Count <= 1)
			{
				return;
			}
			list.Sort(delegate(T a, T b)
			{
				TSortBy tsortBy = selector(a);
				TSortBy other = selector(b);
				if (!tsortBy.Equals(other))
				{
					return tsortBy.CompareTo(other);
				}
				TThenBy tthenBy = thenBySelector(a);
				TThenBy other2 = thenBySelector(b);
				if (!tthenBy.Equals(other2))
				{
					return tthenBy.CompareTo(other2);
				}
				TThenBy2 tthenBy2 = thenBy2Selector(a);
				return tthenBy2.CompareTo(thenBy2Selector(b));
			});
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x00022984 File Offset: 0x00020B84
		public static void SortByColor<T>(this List<T> colorDefs, Func<T, Color> getColor)
		{
			colorDefs.SortBy(delegate(T x)
			{
				float num;
				float a;
				float num2;
				Color.RGBToHSV(getColor(x), out num, out a, out num2);
				if (!Mathf.Approximately(a, 0f))
				{
					return (float)Mathf.RoundToInt(num * 100f);
				}
				return -1f;
			}, delegate(T x)
			{
				float num;
				float num2;
				float num3;
				Color.RGBToHSV(getColor(x), out num, out num2, out num3);
				return Mathf.RoundToInt(num3 * 100f);
			});
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x000229BC File Offset: 0x00020BBC
		public static void SortByDescending<T, TSortByDescending>(this List<T> list, Func<T, TSortByDescending> selector) where TSortByDescending : IComparable<TSortByDescending>
		{
			if (list.Count <= 1)
			{
				return;
			}
			list.Sort(delegate(T a, T b)
			{
				TSortByDescending tsortByDescending = selector(b);
				return tsortByDescending.CompareTo(selector(a));
			});
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x000229F4 File Offset: 0x00020BF4
		public static void SortByDescending<T, TSortByDescending, TThenByDescending>(this List<T> list, Func<T, TSortByDescending> selector, Func<T, TThenByDescending> thenByDescendingSelector) where TSortByDescending : IComparable<TSortByDescending>, IEquatable<TSortByDescending> where TThenByDescending : IComparable<TThenByDescending>
		{
			if (list.Count <= 1)
			{
				return;
			}
			list.Sort(delegate(T a, T b)
			{
				TSortByDescending other = selector(a);
				TSortByDescending other2 = selector(b);
				if (!other.Equals(other2))
				{
					return other2.CompareTo(other);
				}
				TThenByDescending tthenByDescending = thenByDescendingSelector(b);
				return tthenByDescending.CompareTo(thenByDescendingSelector(a));
			});
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x00022A34 File Offset: 0x00020C34
		public static void SortStable<T>(this IList<T> list, Func<T, T, int> comparator)
		{
			if (list.Count <= 1)
			{
				return;
			}
			List<Pair<T, int>> list2;
			bool flag;
			if (GenCollection.SortStableTempList<T>.working)
			{
				list2 = new List<Pair<T, int>>();
				flag = false;
			}
			else
			{
				list2 = GenCollection.SortStableTempList<T>.list;
				GenCollection.SortStableTempList<T>.working = true;
				flag = true;
			}
			try
			{
				list2.Clear();
				for (int i = 0; i < list.Count; i++)
				{
					list2.Add(new Pair<T, int>(list[i], i));
				}
				list2.Sort(delegate(Pair<T, int> lhs, Pair<T, int> rhs)
				{
					int num = comparator(lhs.First, rhs.First);
					if (num != 0)
					{
						return num;
					}
					return lhs.Second.CompareTo(rhs.Second);
				});
				list.Clear();
				for (int j = 0; j < list2.Count; j++)
				{
					list.Add(list2[j].First);
				}
				list2.Clear();
			}
			finally
			{
				if (flag)
				{
					GenCollection.SortStableTempList<T>.working = false;
				}
			}
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x00022B08 File Offset: 0x00020D08
		public static IComparer<T> ThenBy<T>(this IComparer<T> first, IComparer<T> second)
		{
			return new GenCollection.ComparerChain<T>(first, second);
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x00022B11 File Offset: 0x00020D11
		public static IComparer<T> Descending<T>(this IComparer<T> cmp)
		{
			return new GenCollection.DescendingComparer<T>(cmp);
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x00022B19 File Offset: 0x00020D19
		public static IComparer<T> CompareBy<T, TComparable>(Func<T, TComparable> selector) where TComparable : IComparable<TComparable>
		{
			return Comparer<T>.Create(delegate(T a, T b)
			{
				TComparable tcomparable = selector(a);
				return tcomparable.CompareTo(selector(b));
			});
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x00022B38 File Offset: 0x00020D38
		public static int RemoveAll<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Predicate<KeyValuePair<TKey, TValue>> predicate)
		{
			List<TKey> list = null;
			int result;
			try
			{
				foreach (KeyValuePair<TKey, TValue> obj in dictionary)
				{
					if (predicate(obj))
					{
						if (list == null)
						{
							list = SimplePool<List<TKey>>.Get();
						}
						list.Add(obj.Key);
					}
				}
				if (list != null)
				{
					int i = 0;
					int count = list.Count;
					while (i < count)
					{
						dictionary.Remove(list[i]);
						i++;
					}
					result = list.Count;
				}
				else
				{
					result = 0;
				}
			}
			finally
			{
				if (list != null)
				{
					list.Clear();
					SimplePool<List<TKey>>.Return(list);
				}
			}
			return result;
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x00022BF4 File Offset: 0x00020DF4
		public static void RemoveAll<T>(this List<T> list, Func<T, int, bool> predicate)
		{
			int num = 0;
			int count = list.Count;
			while (num < count && !predicate(list[num], num))
			{
				num++;
			}
			if (num >= count)
			{
				return;
			}
			int i = num + 1;
			while (i < count)
			{
				while (i < count && predicate(list[i], i))
				{
					i++;
				}
				if (i < count)
				{
					list[num++] = list[i++];
				}
			}
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x00022C65 File Offset: 0x00020E65
		public static void RemoveLast<T>(this List<T> list)
		{
			list.RemoveAt(list.Count - 1);
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x00022C75 File Offset: 0x00020E75
		public static T Pop<T>(this List<T> list)
		{
			T result = list[list.Count - 1];
			list.RemoveAt(list.Count - 1);
			return result;
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x00022C93 File Offset: 0x00020E93
		public static T PopFront<T>(this List<T> list)
		{
			T result = list[0];
			list.RemoveAt(0);
			return result;
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x00022CA3 File Offset: 0x00020EA3
		public static bool Any<T>(this List<T> list, Predicate<T> predicate)
		{
			return list.FindIndex(predicate) != -1;
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x00022CB2 File Offset: 0x00020EB2
		public static bool Any<T>(this List<T> list)
		{
			return list.Count != 0;
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00022CBD File Offset: 0x00020EBD
		public static bool Empty<T>(this List<T> list)
		{
			return list.Count == 0;
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x00022CC8 File Offset: 0x00020EC8
		public static bool Any<T>(this HashSet<T> list)
		{
			return list.Count != 0;
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x00022CD3 File Offset: 0x00020ED3
		public static bool Any<T>(this Stack<T> list)
		{
			return list.Count != 0;
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x00022CE0 File Offset: 0x00020EE0
		public static void AddRange<T>(this HashSet<T> set, List<T> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				set.Add(list[i]);
			}
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x00022D0C File Offset: 0x00020F0C
		public static void AddRange<T>(this HashSet<T> set, HashSet<T> other)
		{
			foreach (T item in other)
			{
				set.Add(item);
			}
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x00022D5C File Offset: 0x00020F5C
		public static void AddRangeNoAlloc<T>(this List<T> self, IEnumerator<T> other) where T : class
		{
			while (other.MoveNext())
			{
				T item = other.Current;
				self.Add(item);
			}
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00022D74 File Offset: 0x00020F74
		public static void AddRangeFast<T>(this List<T> self, IEnumerable<T> other) where T : class
		{
			using (IEnumerator<T> enumerator = other.GetEnumerator())
			{
				self.AddRangeNoAlloc(enumerator);
			}
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x00022DAC File Offset: 0x00020FAC
		public static void AddRangeWhereFast<T>(this List<T> self, IEnumerable<T> other, Predicate<T> condition) where T : class
		{
			foreach (T t in other)
			{
				if (condition(t))
				{
					self.Add(t);
				}
			}
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x00022E00 File Offset: 0x00021000
		public static int Count_EnumerableBase(IEnumerable e)
		{
			if (e == null)
			{
				return 0;
			}
			ICollection collection = e as ICollection;
			if (collection != null)
			{
				return collection.Count;
			}
			int num = 0;
			foreach (object obj in e)
			{
				num++;
			}
			return num;
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00022E68 File Offset: 0x00021068
		public static T FirstOrDefault<T>(this List<T> list, Predicate<T> predicate)
		{
			foreach (T t in list)
			{
				if (predicate(t))
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x00022EC8 File Offset: 0x000210C8
		public static object FirstOrDefault_EnumerableBase(IEnumerable e)
		{
			if (e == null)
			{
				return null;
			}
			IList list = e as IList;
			if (list == null)
			{
				using (IEnumerator enumerator = e.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						return enumerator.Current;
					}
				}
				return null;
			}
			if (list.Count == 0)
			{
				return null;
			}
			return list[0];
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x00022F38 File Offset: 0x00021138
		public static float AverageWeighted<T>(this IEnumerable<T> list, Func<T, float> weight, Func<T, float> value)
		{
			float num = 0f;
			float num2 = 0f;
			foreach (T arg in list)
			{
				float num3 = weight(arg);
				num += num3;
				num2 += value(arg) * num3;
			}
			return num2 / num;
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x00022FA4 File Offset: 0x000211A4
		public static void ExecuteEnumerable(this IEnumerable enumerable)
		{
			foreach (object obj in enumerable)
			{
			}
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x00022FEC File Offset: 0x000211EC
		public static IEnumerable<T> OrElseEmptyEnumerable<T>(this IEnumerable<T> enumerable)
		{
			if (enumerable == null)
			{
				return Enumerable.Empty<T>();
			}
			return enumerable;
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x00022FF8 File Offset: 0x000211F8
		public static bool EnumerableNullOrEmpty<T>(this IEnumerable<T> enumerable)
		{
			if (enumerable == null)
			{
				return true;
			}
			ICollection collection = enumerable as ICollection;
			if (collection != null)
			{
				return collection.Count == 0;
			}
			return !enumerable.Any<T>();
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00023028 File Offset: 0x00021228
		public static int EnumerableCount(this IEnumerable enumerable)
		{
			if (enumerable == null)
			{
				return 0;
			}
			ICollection collection = enumerable as ICollection;
			if (collection != null)
			{
				return collection.Count;
			}
			int num = 0;
			foreach (object obj in enumerable)
			{
				num++;
			}
			return num;
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x00023090 File Offset: 0x00021290
		public static int Count<T>(this List<T> list, Predicate<T> predicate)
		{
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (predicate(list[i]))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x000230C4 File Offset: 0x000212C4
		public static float Percent<T>(this IEnumerable<T> source, Func<T, bool> predicate)
		{
			int num = 0;
			int num2 = 0;
			foreach (T arg in source)
			{
				num2++;
				if (predicate(arg))
				{
					num++;
				}
			}
			if (num2 == 0)
			{
				throw new InvalidOperationException("Sequence contains no elements");
			}
			return (float)num / (float)num2;
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x00023130 File Offset: 0x00021330
		public static int FirstIndexOf<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
		{
			int num = 0;
			foreach (T arg in enumerable)
			{
				if (predicate(arg))
				{
					return num;
				}
				num++;
			}
			return -1;
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x00023188 File Offset: 0x00021388
		public static V TryGetValue<T, V>(this IReadOnlyDictionary<T, V> dict, T key, V fallback = default(V))
		{
			V result;
			if (key == null || !dict.TryGetValue(key, out result))
			{
				result = fallback;
			}
			return result;
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x000231AB File Offset: 0x000213AB
		public static IReadOnlyDictionary<T, V> Slice<T, V>(this IReadOnlyDictionary<T, V> dict, IEnumerable<T> keys)
		{
			return new GenCollection.DictionarySlice<T, V>(dict, keys);
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x000231AB File Offset: 0x000213AB
		public static IReadOnlyDictionary<T, V> Slice<T, V>(this IReadOnlyDictionary<T, V> dict, params T[] keys)
		{
			return new GenCollection.DictionarySlice<T, V>(dict, keys);
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x000231B4 File Offset: 0x000213B4
		public static IEnumerable<Pair<T, V>> Cross<T, V>(this IEnumerable<T> lhs, IEnumerable<V> rhs)
		{
			T[] lhsv = lhs.ToArray<T>();
			V[] rhsv = rhs.ToArray<V>();
			int num;
			for (int i = 0; i < lhsv.Length; i = num)
			{
				for (int j = 0; j < rhsv.Length; j = num)
				{
					yield return new Pair<T, V>(lhsv[i], rhsv[j]);
					num = j + 1;
				}
				num = i + 1;
			}
			yield break;
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x000231CB File Offset: 0x000213CB
		public static IEnumerable<T> Concat<T>(this IEnumerable<T> lhs, T rhs)
		{
			foreach (T t in lhs)
			{
				yield return t;
			}
			IEnumerator<T> enumerator = null;
			yield return rhs;
			yield break;
			yield break;
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x000231E4 File Offset: 0x000213E4
		public static void RemoveWhere<T>(this IList<T> list, Func<T, bool> predicate)
		{
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (predicate(list[i]))
				{
					list.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x0002321C File Offset: 0x0002141C
		public static bool ContainsAny<T>(this IList<T> list, Func<T, bool> predicate)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (predicate(list[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x0002324C File Offset: 0x0002144C
		public static IEnumerable<TSource> ConcatIfNotNull<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			if (first == null)
			{
				throw new ArgumentNullException("first");
			}
			if (second == null)
			{
				return first;
			}
			return first.Concat(second);
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x00023268 File Offset: 0x00021468
		public static LocalTargetInfo FirstValid(this List<LocalTargetInfo> source)
		{
			if (source == null)
			{
				return LocalTargetInfo.Invalid;
			}
			for (int i = 0; i < source.Count; i++)
			{
				if (source[i].IsValid)
				{
					return source[i];
				}
			}
			return LocalTargetInfo.Invalid;
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x000232AD File Offset: 0x000214AD
		public static IEnumerable<T> Except<T>(this IEnumerable<T> lhs, T rhs) where T : class
		{
			foreach (T t in lhs)
			{
				if (t != rhs)
				{
					yield return t;
				}
			}
			IEnumerator<T> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x000232C4 File Offset: 0x000214C4
		public static IEnumerable<ValueTuple<T, T>> Pairwise<T>(this IEnumerable<T> enumerable) where T : class
		{
			foreach (T item in enumerable)
			{
				IEnumerator<T> iterator;
				if (!iterator.MoveNext())
				{
					yield return ValueTuple.Create<T, T>(item, default(T));
				}
				else
				{
					yield return ValueTuple.Create<T, T>(item, iterator.Current);
				}
			}
			yield break;
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x000232D4 File Offset: 0x000214D4
		public static bool ListsEqual<T>(List<T> a, List<T> b) where T : class
		{
			if (a == b)
			{
				return true;
			}
			if (a.NullOrEmpty<T>() && b.NullOrEmpty<T>())
			{
				return true;
			}
			if (a.NullOrEmpty<T>() || b.NullOrEmpty<T>())
			{
				return false;
			}
			if (a.Count != b.Count)
			{
				return false;
			}
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int i = 0; i < a.Count; i++)
			{
				if (!@default.Equals(a[i], b[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x0002334C File Offset: 0x0002154C
		public static bool DictsEqual<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> a, IReadOnlyDictionary<TKey, TValue> b)
		{
			int num = (a != null) ? a.Count : 0;
			int num2 = (b != null) ? b.Count : 0;
			if (num != num2)
			{
				return false;
			}
			if (num == 0)
			{
				return true;
			}
			EqualityComparer<TValue> @default = EqualityComparer<TValue>.Default;
			foreach (KeyValuePair<TKey, TValue> keyValuePair in a)
			{
				TValue y;
				if (!b.TryGetValue(keyValuePair.Key, out y) || !@default.Equals(keyValuePair.Value, y))
				{
					return false;
				}
			}
			foreach (KeyValuePair<TKey, TValue> keyValuePair2 in b)
			{
				TValue y2;
				if (!a.TryGetValue(keyValuePair2.Key, out y2) || !@default.Equals(keyValuePair2.Value, y2))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x0002343C File Offset: 0x0002163C
		public static int DictHashCode<Key, Value>(IReadOnlyDictionary<Key, Value> dict)
		{
			int num = 0;
			foreach (KeyValuePair<Key, Value> keyValuePair in (dict ?? Enumerable.Empty<KeyValuePair<Key, Value>>()))
			{
				num ^= keyValuePair.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x0002349C File Offset: 0x0002169C
		public static void Deconstruct<Key, Value>(this KeyValuePair<Key, Value> tuple, out Key key, out Value value)
		{
			key = tuple.Key;
			value = tuple.Value;
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x000234B8 File Offset: 0x000216B8
		public static bool SetsEqual<T>(this List<T> a, List<T> b)
		{
			if (a == b)
			{
				return true;
			}
			if (a.NullOrEmpty<T>() && b.NullOrEmpty<T>())
			{
				return true;
			}
			if (a.NullOrEmpty<T>() || b.NullOrEmpty<T>())
			{
				return false;
			}
			for (int i = 0; i < a.Count; i++)
			{
				if (!b.Contains(a[i]))
				{
					return false;
				}
			}
			for (int j = 0; j < b.Count; j++)
			{
				if (!a.Contains(b[j]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x00023534 File Offset: 0x00021734
		public static IEnumerable<T> TakeRandom<T>(this List<T> list, int count)
		{
			if (list.NullOrEmpty<T>())
			{
				yield break;
			}
			int num;
			for (int i = 0; i < count; i = num)
			{
				yield return list[Rand.Range(0, list.Count)];
				num = i + 1;
			}
			yield break;
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0002354C File Offset: 0x0002174C
		public static List<T> TakeRandomDistinct<T>(this List<T> list, int count)
		{
			List<T> list2 = new List<T>();
			foreach (T item in list.Distinct<T>().InRandomOrder(null))
			{
				list2.Add(item);
				if (list2.Count == count)
				{
					return list2;
				}
			}
			return list2;
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x000235B8 File Offset: 0x000217B8
		public static List<T> TakeRandomDistinct<T>(this IEnumerable<T> list, int count)
		{
			List<T> list2 = new List<T>();
			foreach (T item in list.Distinct<T>().InRandomOrder(null))
			{
				list2.Add(item);
				if (list2.Count == count)
				{
					return list2;
				}
			}
			return list2;
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x00023624 File Offset: 0x00021824
		public static void AddDistinct<T>(this List<T> list, T obj)
		{
			list.AddUnique(obj);
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0002362D File Offset: 0x0002182D
		public static void AddUnique<T>(this List<T> list, T obj)
		{
			if (!list.Contains(obj))
			{
				list.Add(obj);
			}
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x00023640 File Offset: 0x00021840
		public static void AddRangeUnique<T>(this List<T> list, List<T> other)
		{
			for (int i = 0; i < other.Count; i++)
			{
				if (!list.Contains(other[i]))
				{
					list.Add(other[i]);
				}
			}
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x0002367C File Offset: 0x0002187C
		public static void AddRangeUnique<T>(this List<T> list, IEnumerable<T> other)
		{
			foreach (T item in other)
			{
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x000236D0 File Offset: 0x000218D0
		public static int Replace<T>(this IList<T> list, T replace, T with) where T : class
		{
			if (list == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] == replace)
				{
					list[i] = with;
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x00023716 File Offset: 0x00021916
		public static Pair<K, List<E>> ConvertIGroupingToPair<K, E>(IGrouping<K, E> g)
		{
			return new Pair<K, List<E>>(g.Key, g.ToList<E>());
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0002372C File Offset: 0x0002192C
		public static int GetCountGreaterOrEqualInSortedList(List<int> list, int val)
		{
			int num = list.BinarySearch(val);
			if (num >= 0)
			{
				return list.Count - num;
			}
			int num2 = ~num;
			return list.Count - num2;
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0002375C File Offset: 0x0002195C
		public static void InsertIntoSortedList<T>(this List<T> list, T val, IComparer<T> cmp)
		{
			if (list.Count == 0)
			{
				list.Add(val);
				return;
			}
			int num = list.BinarySearch(val, cmp);
			if (num >= 0)
			{
				list.Insert(num, val);
				return;
			}
			list.Insert(~num, val);
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x00023798 File Offset: 0x00021998
		public static void RemoveBatchUnordered<T>(this List<T> list, List<int> indices)
		{
			if (indices.Count == 0)
			{
				return;
			}
			int num = list.Count - 1;
			foreach (int num2 in indices)
			{
				if (num == num2)
				{
					num--;
				}
				else
				{
					if (num <= 0)
					{
						break;
					}
					list[num2] = list[num];
					num--;
				}
			}
			for (int i = list.Count - 1; i > num; i--)
			{
				list.RemoveAt(i);
			}
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x00023830 File Offset: 0x00021A30
		public static ValueType GetWithFallback<KeyType, ValueType>(this Dictionary<KeyType, ValueType> dictionary, KeyType key, ValueType fallback = default(ValueType))
		{
			ValueType result;
			if (dictionary.TryGetValue(key, out result))
			{
				return result;
			}
			return fallback;
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0002384C File Offset: 0x00021A4C
		public static void ClearNullAndDestroyed<T>(this List<T> list) where T : Thing
		{
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (list[i].DestroyedOrNull())
				{
					list.RemoveAt(i);
				}
			}
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x00023886 File Offset: 0x00021A86
		public static bool TryPop<T>(this Stack<T> stack, out T element)
		{
			if (stack.Count == 0)
			{
				element = default(T);
				return false;
			}
			element = stack.Pop();
			return true;
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x000238A6 File Offset: 0x00021AA6
		public static bool TryDequeue<T>(this Queue<T> queue, out T element)
		{
			if (queue.Count == 0)
			{
				element = default(T);
				return false;
			}
			element = queue.Dequeue();
			return true;
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x000238C8 File Offset: 0x00021AC8
		public static void CopyToList<T>(this IEnumerable<T> source, List<T> target, bool clear = true)
		{
			if (clear)
			{
				target.Clear();
			}
			ICollection<T> collection = source as ICollection<T>;
			if (collection != null)
			{
				int num = (clear ? 0 : target.Count) + collection.Count;
				if (target.Capacity < num)
				{
					target.Capacity = num;
				}
			}
			foreach (T item in source)
			{
				target.Add(item);
			}
		}

		// Token: 0x020000F3 RID: 243
		private static class SortStableTempList<T>
		{
			// Token: 0x040003A8 RID: 936
			public static List<Pair<T, int>> list = new List<Pair<T, int>>();

			// Token: 0x040003A9 RID: 937
			public static bool working;
		}

		// Token: 0x020000F4 RID: 244
		private class ComparerChain<T> : IComparer<T>
		{
			// Token: 0x0600075A RID: 1882 RVA: 0x00023954 File Offset: 0x00021B54
			public ComparerChain(IComparer<T> first, IComparer<T> second)
			{
				this.first = first;
				this.second = second;
			}

			// Token: 0x0600075B RID: 1883 RVA: 0x0002396C File Offset: 0x00021B6C
			public int Compare(T x, T y)
			{
				int num = this.first.Compare(x, y);
				if (num != 0)
				{
					return num;
				}
				return this.second.Compare(x, y);
			}

			// Token: 0x040003AA RID: 938
			private readonly IComparer<T> first;

			// Token: 0x040003AB RID: 939
			private readonly IComparer<T> second;
		}

		// Token: 0x020000F5 RID: 245
		private class DescendingComparer<T> : IComparer<T>
		{
			// Token: 0x0600075C RID: 1884 RVA: 0x00023999 File Offset: 0x00021B99
			public DescendingComparer(IComparer<T> cmp)
			{
				this.cmp = cmp;
			}

			// Token: 0x0600075D RID: 1885 RVA: 0x000239A8 File Offset: 0x00021BA8
			public int Compare(T x, T y)
			{
				return -this.cmp.Compare(x, y);
			}

			// Token: 0x040003AC RID: 940
			private readonly IComparer<T> cmp;
		}

		// Token: 0x020000F6 RID: 246
		public class DictionarySlice<T, V> : IReadOnlyDictionary<T, V>, IEnumerable<KeyValuePair<T, V>>, IEnumerable, IReadOnlyCollection<KeyValuePair<T, V>>
		{
			// Token: 0x17000141 RID: 321
			public V this[T key]
			{
				get
				{
					if (key == null)
					{
						throw new ArgumentNullException();
					}
					if (!this.keys.Contains(key))
					{
						throw new KeyNotFoundException(key.ToStringSafe<T>());
					}
					return this.underlying[key];
				}
			}

			// Token: 0x17000142 RID: 322
			// (get) Token: 0x0600075F RID: 1887 RVA: 0x000239EE File Offset: 0x00021BEE
			public IEnumerable<V> Values
			{
				get
				{
					foreach (T t in this.keys)
					{
						V v;
						if (t != null && this.underlying.TryGetValue(t, out v))
						{
							yield return v;
						}
					}
					HashSet<T>.Enumerator enumerator = default(HashSet<T>.Enumerator);
					yield break;
					yield break;
				}
			}

			// Token: 0x17000143 RID: 323
			// (get) Token: 0x06000760 RID: 1888 RVA: 0x000239FE File Offset: 0x00021BFE
			public IEnumerable<T> Keys
			{
				get
				{
					return this.keys;
				}
			}

			// Token: 0x17000144 RID: 324
			// (get) Token: 0x06000761 RID: 1889 RVA: 0x00023A06 File Offset: 0x00021C06
			public int Count
			{
				get
				{
					return this.keys.Count;
				}
			}

			// Token: 0x06000762 RID: 1890 RVA: 0x00023A13 File Offset: 0x00021C13
			public bool ContainsKey(T key)
			{
				return key != null && this.keys.Contains(key);
			}

			// Token: 0x06000763 RID: 1891 RVA: 0x00023A2B File Offset: 0x00021C2B
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x06000764 RID: 1892 RVA: 0x00023A33 File Offset: 0x00021C33
			public DictionarySlice(IReadOnlyDictionary<T, V> dict, IEnumerable<T> keys)
			{
				this.underlying = dict;
				this.keys = new HashSet<T>(keys);
			}

			// Token: 0x06000765 RID: 1893 RVA: 0x00023A4E File Offset: 0x00021C4E
			public IEnumerator<KeyValuePair<T, V>> GetEnumerator()
			{
				foreach (T t in this.keys)
				{
					V value;
					if (t != null && this.underlying.TryGetValue(t, out value))
					{
						yield return new KeyValuePair<T, V>(t, value);
					}
				}
				HashSet<T>.Enumerator enumerator = default(HashSet<T>.Enumerator);
				yield break;
				yield break;
			}

			// Token: 0x06000766 RID: 1894 RVA: 0x00023A5D File Offset: 0x00021C5D
			public bool TryGetValue(T key, out V value)
			{
				if (!this.keys.Contains(key))
				{
					value = default(V);
					return false;
				}
				return this.underlying.TryGetValue(key, out value);
			}

			// Token: 0x040003AD RID: 941
			private HashSet<T> keys;

			// Token: 0x040003AE RID: 942
			private IReadOnlyDictionary<T, V> underlying;
		}
	}
}
