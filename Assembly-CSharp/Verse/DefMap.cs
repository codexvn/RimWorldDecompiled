using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000C7A RID: 3194
	public class DefMap<D, V> : IExposable, IEnumerable<KeyValuePair<D, V>>, IEnumerable where D : Def, new() where V : new()
	{
		// Token: 0x17000C6E RID: 3182
		// (get) Token: 0x060051AC RID: 20908 RVA: 0x001A7D08 File Offset: 0x001A5F08
		public int Count
		{
			get
			{
				return this.values.Count;
			}
		}

		// Token: 0x17000C6F RID: 3183
		public V this[D def]
		{
			get
			{
				return this.values[(int)def.index];
			}
			set
			{
				this.values[(int)def.index] = value;
			}
		}

		// Token: 0x17000C70 RID: 3184
		public V this[int index]
		{
			get
			{
				return this.values[index];
			}
			set
			{
				this.values[index] = value;
			}
		}

		// Token: 0x060051B1 RID: 20913 RVA: 0x001A7D64 File Offset: 0x001A5F64
		public DefMap()
		{
			int defCount = DefDatabase<D>.DefCount;
			if (defCount == 0)
			{
				string[] array = new string[5];
				array[0] = "Constructed DefMap<";
				int num = 1;
				Type typeFromHandle = typeof(D);
				array[num] = ((typeFromHandle != null) ? typeFromHandle.ToString() : null);
				array[2] = ", ";
				int num2 = 3;
				Type typeFromHandle2 = typeof(V);
				array[num2] = ((typeFromHandle2 != null) ? typeFromHandle2.ToString() : null);
				array[4] = "> without defs being initialized. Try constructing it in ResolveReferences instead of the constructor.";
				throw new Exception(string.Concat(array));
			}
			this.values = new List<V>(defCount);
			for (int i = 0; i < defCount; i++)
			{
				this.values.Add(Activator.CreateInstance<V>());
			}
		}

		// Token: 0x060051B2 RID: 20914 RVA: 0x001A7E04 File Offset: 0x001A6004
		public void ExposeData()
		{
			Scribe_Collections.Look<V>(ref this.values, "vals", LookMode.Undefined, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				int defCount = DefDatabase<D>.DefCount;
				for (int i = this.values.Count; i < defCount; i++)
				{
					this.values.Add(Activator.CreateInstance<V>());
				}
				while (this.values.Count > defCount)
				{
					this.values.RemoveLast<V>();
				}
			}
		}

		// Token: 0x060051B3 RID: 20915 RVA: 0x001A7E78 File Offset: 0x001A6078
		public void SetAll(V val)
		{
			for (int i = 0; i < this.values.Count; i++)
			{
				this.values[i] = val;
			}
		}

		// Token: 0x060051B4 RID: 20916 RVA: 0x001A7EA8 File Offset: 0x001A60A8
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060051B5 RID: 20917 RVA: 0x001A7EB0 File Offset: 0x001A60B0
		public IEnumerator<KeyValuePair<D, V>> GetEnumerator()
		{
			return (from d in DefDatabase<D>.AllDefsListForReading
			select new KeyValuePair<D, V>(d, this[d])).GetEnumerator();
		}

		// Token: 0x04003ACB RID: 15051
		private List<V> values;
	}
}
