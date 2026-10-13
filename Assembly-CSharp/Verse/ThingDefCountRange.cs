using System;

namespace Verse
{
	// Token: 0x02000C91 RID: 3217
	public struct ThingDefCountRange : IEquatable<ThingDefCountRange>, IExposable
	{
		// Token: 0x17000CB5 RID: 3253
		// (get) Token: 0x060052AE RID: 21166 RVA: 0x001AAF8F File Offset: 0x001A918F
		public ThingDef ThingDef
		{
			get
			{
				return this.thingDef;
			}
		}

		// Token: 0x17000CB6 RID: 3254
		// (get) Token: 0x060052AF RID: 21167 RVA: 0x001AAF97 File Offset: 0x001A9197
		public IntRange CountRange
		{
			get
			{
				return this.countRange;
			}
		}

		// Token: 0x17000CB7 RID: 3255
		// (get) Token: 0x060052B0 RID: 21168 RVA: 0x001AAF9F File Offset: 0x001A919F
		public int Min
		{
			get
			{
				return this.countRange.min;
			}
		}

		// Token: 0x17000CB8 RID: 3256
		// (get) Token: 0x060052B1 RID: 21169 RVA: 0x001AAFAC File Offset: 0x001A91AC
		public int Max
		{
			get
			{
				return this.countRange.max;
			}
		}

		// Token: 0x17000CB9 RID: 3257
		// (get) Token: 0x060052B2 RID: 21170 RVA: 0x001AAFB9 File Offset: 0x001A91B9
		public int TrueMin
		{
			get
			{
				return this.countRange.TrueMin;
			}
		}

		// Token: 0x17000CBA RID: 3258
		// (get) Token: 0x060052B3 RID: 21171 RVA: 0x001AAFC6 File Offset: 0x001A91C6
		public int TrueMax
		{
			get
			{
				return this.countRange.TrueMax;
			}
		}

		// Token: 0x060052B4 RID: 21172 RVA: 0x001AAFD3 File Offset: 0x001A91D3
		public ThingDefCountRange(ThingDef thingDef, int min, int max)
		{
			this = new ThingDefCountRange(thingDef, new IntRange(min, max));
		}

		// Token: 0x060052B5 RID: 21173 RVA: 0x001AAFE3 File Offset: 0x001A91E3
		public ThingDefCountRange(ThingDef thingDef, IntRange countRange)
		{
			this.thingDef = thingDef;
			this.countRange = countRange;
		}

		// Token: 0x060052B6 RID: 21174 RVA: 0x001AAFF4 File Offset: 0x001A91F4
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<IntRange>(ref this.countRange, "countRange", default(IntRange), false);
		}

		// Token: 0x060052B7 RID: 21175 RVA: 0x001AB02B File Offset: 0x001A922B
		public ThingDefCountRange WithCountRange(IntRange newCountRange)
		{
			return new ThingDefCountRange(this.thingDef, newCountRange);
		}

		// Token: 0x060052B8 RID: 21176 RVA: 0x001AB039 File Offset: 0x001A9239
		public ThingDefCountRange WithCountRange(int newMin, int newMax)
		{
			return new ThingDefCountRange(this.thingDef, newMin, newMax);
		}

		// Token: 0x060052B9 RID: 21177 RVA: 0x001AB048 File Offset: 0x001A9248
		public override bool Equals(object obj)
		{
			return obj is ThingDefCountRange && this.Equals((ThingDefCountRange)obj);
		}

		// Token: 0x060052BA RID: 21178 RVA: 0x001AB060 File Offset: 0x001A9260
		public bool Equals(ThingDefCountRange other)
		{
			return this == other;
		}

		// Token: 0x060052BB RID: 21179 RVA: 0x001AB06E File Offset: 0x001A926E
		public static bool operator ==(ThingDefCountRange a, ThingDefCountRange b)
		{
			return a.thingDef == b.thingDef && a.countRange == b.countRange;
		}

		// Token: 0x060052BC RID: 21180 RVA: 0x001AB091 File Offset: 0x001A9291
		public static bool operator !=(ThingDefCountRange a, ThingDefCountRange b)
		{
			return !(a == b);
		}

		// Token: 0x060052BD RID: 21181 RVA: 0x001AB09D File Offset: 0x001A929D
		public override int GetHashCode()
		{
			return Gen.HashCombine<ThingDef>(this.countRange.GetHashCode(), this.thingDef);
		}

		// Token: 0x060052BE RID: 21182 RVA: 0x001AB0BC File Offset: 0x001A92BC
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

		// Token: 0x060052BF RID: 21183 RVA: 0x001AB120 File Offset: 0x001A9320
		public static implicit operator ThingDefCountRange(ThingDefCountRangeClass t)
		{
			return new ThingDefCountRange(t.thingDef, t.countRange);
		}

		// Token: 0x060052C0 RID: 21184 RVA: 0x001AB133 File Offset: 0x001A9333
		public static explicit operator ThingDefCountRange(ThingDefCount t)
		{
			return new ThingDefCountRange(t.ThingDef, t.Count, t.Count);
		}

		// Token: 0x060052C1 RID: 21185 RVA: 0x001AB14F File Offset: 0x001A934F
		public static explicit operator ThingDefCountRange(ThingDefCountClass t)
		{
			return new ThingDefCountRange(t.thingDef, t.count, t.count);
		}

		// Token: 0x04003B16 RID: 15126
		private ThingDef thingDef;

		// Token: 0x04003B17 RID: 15127
		private IntRange countRange;
	}
}
