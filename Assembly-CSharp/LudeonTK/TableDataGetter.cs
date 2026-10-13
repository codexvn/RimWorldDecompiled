using System;
using Verse;

namespace LudeonTK
{
	// Token: 0x02000098 RID: 152
	public class TableDataGetter<T>
	{
		// Token: 0x06000344 RID: 836 RVA: 0x000128C5 File Offset: 0x00010AC5
		public TableDataGetter(string label, Func<T, string> getter)
		{
			this.label = label;
			this.getter = getter;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x000128DC File Offset: 0x00010ADC
		public TableDataGetter(string label, Func<T, float> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString("0.#"));
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00012918 File Offset: 0x00010B18
		public TableDataGetter(string label, Func<T, int> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString("F0"));
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00012954 File Offset: 0x00010B54
		public TableDataGetter(string label, Func<T, ThingDef> getter)
		{
			this.label = label;
			this.getter = delegate(T t)
			{
				ThingDef thingDef = getter(t);
				if (thingDef == null)
				{
					return "";
				}
				return thingDef.defName;
			};
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00012990 File Offset: 0x00010B90
		public TableDataGetter(string label, Func<T, object> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString());
		}

		// Token: 0x0400020A RID: 522
		public string label;

		// Token: 0x0400020B RID: 523
		public Func<T, string> getter;
	}
}
