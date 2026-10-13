using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C94 RID: 3220
	public struct Pair<T1, T2> : IEquatable<Pair<T1, T2>>
	{
		// Token: 0x17000CBF RID: 3263
		// (get) Token: 0x060052D3 RID: 21203 RVA: 0x001AB3F8 File Offset: 0x001A95F8
		public T1 First
		{
			get
			{
				return this.first;
			}
		}

		// Token: 0x17000CC0 RID: 3264
		// (get) Token: 0x060052D4 RID: 21204 RVA: 0x001AB400 File Offset: 0x001A9600
		public T2 Second
		{
			get
			{
				return this.second;
			}
		}

		// Token: 0x060052D5 RID: 21205 RVA: 0x001AB408 File Offset: 0x001A9608
		public Pair(T1 first, T2 second)
		{
			this.first = first;
			this.second = second;
		}

		// Token: 0x060052D6 RID: 21206 RVA: 0x001AB418 File Offset: 0x001A9618
		public override string ToString()
		{
			string[] array = new string[5];
			array[0] = "(";
			int num = 1;
			T1 t = this.First;
			array[num] = t.ToString();
			array[2] = ", ";
			int num2 = 3;
			T2 t2 = this.Second;
			array[num2] = t2.ToString();
			array[4] = ")";
			return string.Concat(array);
		}

		// Token: 0x060052D7 RID: 21207 RVA: 0x001AB476 File Offset: 0x001A9676
		public override int GetHashCode()
		{
			return Gen.HashCombine<T2>(Gen.HashCombine<T1>(0, this.first), this.second);
		}

		// Token: 0x060052D8 RID: 21208 RVA: 0x001AB48F File Offset: 0x001A968F
		public override bool Equals(object other)
		{
			return other is Pair<T1, T2> && this.Equals((Pair<T1, T2>)other);
		}

		// Token: 0x060052D9 RID: 21209 RVA: 0x001AB4A7 File Offset: 0x001A96A7
		public bool Equals(Pair<T1, T2> other)
		{
			return EqualityComparer<T1>.Default.Equals(this.first, other.first) && EqualityComparer<T2>.Default.Equals(this.second, other.second);
		}

		// Token: 0x060052DA RID: 21210 RVA: 0x001AB4D9 File Offset: 0x001A96D9
		public static bool operator ==(Pair<T1, T2> lhs, Pair<T1, T2> rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x060052DB RID: 21211 RVA: 0x001AB4E3 File Offset: 0x001A96E3
		public static bool operator !=(Pair<T1, T2> lhs, Pair<T1, T2> rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x04003B1C RID: 15132
		private T1 first;

		// Token: 0x04003B1D RID: 15133
		private T2 second;
	}
}
