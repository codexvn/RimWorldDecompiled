using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000C92 RID: 3218
	public sealed class ThingDefCountRangeClass : IExposable
	{
		// Token: 0x17000CBB RID: 3259
		// (get) Token: 0x060052C2 RID: 21186 RVA: 0x001AB168 File Offset: 0x001A9368
		public int Min
		{
			get
			{
				return this.countRange.min;
			}
		}

		// Token: 0x17000CBC RID: 3260
		// (get) Token: 0x060052C3 RID: 21187 RVA: 0x001AB175 File Offset: 0x001A9375
		public int Max
		{
			get
			{
				return this.countRange.max;
			}
		}

		// Token: 0x17000CBD RID: 3261
		// (get) Token: 0x060052C4 RID: 21188 RVA: 0x001AB182 File Offset: 0x001A9382
		public int TrueMin
		{
			get
			{
				return this.countRange.TrueMin;
			}
		}

		// Token: 0x17000CBE RID: 3262
		// (get) Token: 0x060052C5 RID: 21189 RVA: 0x001AB18F File Offset: 0x001A938F
		public int TrueMax
		{
			get
			{
				return this.countRange.TrueMax;
			}
		}

		// Token: 0x060052C6 RID: 21190 RVA: 0x000024DF File Offset: 0x000006DF
		public ThingDefCountRangeClass()
		{
		}

		// Token: 0x060052C7 RID: 21191 RVA: 0x001AB19C File Offset: 0x001A939C
		public ThingDefCountRangeClass(ThingDef thingDef, int min, int max) : this(thingDef, new IntRange(min, max))
		{
		}

		// Token: 0x060052C8 RID: 21192 RVA: 0x001AB1AC File Offset: 0x001A93AC
		public ThingDefCountRangeClass(ThingDef thingDef, IntRange countRange)
		{
			this.thingDef = thingDef;
			this.countRange = countRange;
		}

		// Token: 0x060052C9 RID: 21193 RVA: 0x001AB1C4 File Offset: 0x001A93C4
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<IntRange>(ref this.countRange, "countRange", default(IntRange), false);
		}

		// Token: 0x060052CA RID: 21194 RVA: 0x001AB1FC File Offset: 0x001A93FC
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured ThingDefCountRangeClass: " + xmlRoot.OuterXml);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "thingDef", xmlRoot.Name, null, null, null);
			this.countRange = ParseHelper.FromString<IntRange>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x060052CB RID: 21195 RVA: 0x001AB258 File Offset: 0x001A9458
		public override string ToString()
		{
			string[] array = new string[5];
			array[0] = "(";
			int num = 1;
			IntRange intRange = this.countRange;
			array[num] = intRange.ToString();
			array[2] = "x ";
			array[3] = ((this.thingDef != null) ? this.thingDef.defName : "null");
			array[4] = ")";
			return string.Concat(array);
		}

		// Token: 0x060052CC RID: 21196 RVA: 0x001AB2BC File Offset: 0x001A94BC
		public static implicit operator ThingDefCountRangeClass(ThingDefCountRange t)
		{
			return new ThingDefCountRangeClass(t.ThingDef, t.CountRange);
		}

		// Token: 0x060052CD RID: 21197 RVA: 0x001AB2D1 File Offset: 0x001A94D1
		public static explicit operator ThingDefCountRangeClass(ThingDefCount t)
		{
			return new ThingDefCountRangeClass(t.ThingDef, t.Count, t.Count);
		}

		// Token: 0x060052CE RID: 21198 RVA: 0x001AB2ED File Offset: 0x001A94ED
		public static explicit operator ThingDefCountRangeClass(ThingDefCountClass t)
		{
			return new ThingDefCountRangeClass(t.thingDef, t.count, t.count);
		}

		// Token: 0x04003B18 RID: 15128
		public ThingDef thingDef;

		// Token: 0x04003B19 RID: 15129
		public IntRange countRange;
	}
}
