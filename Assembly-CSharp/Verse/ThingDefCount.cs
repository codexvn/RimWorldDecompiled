using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000C8F RID: 3215
	public struct ThingDefCount : IEquatable<ThingDefCount>, IExposable
	{
		// Token: 0x17000CAC RID: 3244
		// (get) Token: 0x06005293 RID: 21139 RVA: 0x001AAC1B File Offset: 0x001A8E1B
		public ThingDef ThingDef
		{
			get
			{
				return this.thingDef;
			}
		}

		// Token: 0x17000CAD RID: 3245
		// (get) Token: 0x06005294 RID: 21140 RVA: 0x001AAC23 File Offset: 0x001A8E23
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x17000CAE RID: 3246
		// (get) Token: 0x06005295 RID: 21141 RVA: 0x001AAC2B File Offset: 0x001A8E2B
		public string Label
		{
			get
			{
				return GenLabel.ThingLabel(this.thingDef, null, this.count);
			}
		}

		// Token: 0x17000CAF RID: 3247
		// (get) Token: 0x06005296 RID: 21142 RVA: 0x001AAC3F File Offset: 0x001A8E3F
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.thingDef);
			}
		}

		// Token: 0x06005297 RID: 21143 RVA: 0x001AAC52 File Offset: 0x001A8E52
		public ThingDefCount(ThingDef thingDef, int count)
		{
			if (count < 0)
			{
				Log.Warning(string.Format("Tried to set {0} count to {1}. thingDef={2}", "ThingDefCount", count, thingDef));
				count = 0;
			}
			this.thingDef = thingDef;
			this.count = count;
		}

		// Token: 0x06005298 RID: 21144 RVA: 0x001AAC84 File Offset: 0x001A8E84
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		// Token: 0x06005299 RID: 21145 RVA: 0x001AACA8 File Offset: 0x001A8EA8
		public ThingDefCount WithCount(int newCount)
		{
			return new ThingDefCount(this.thingDef, newCount);
		}

		// Token: 0x0600529A RID: 21146 RVA: 0x001AACB6 File Offset: 0x001A8EB6
		public override bool Equals(object obj)
		{
			return obj is ThingDefCount && this.Equals((ThingDefCount)obj);
		}

		// Token: 0x0600529B RID: 21147 RVA: 0x001AACCE File Offset: 0x001A8ECE
		public bool Equals(ThingDefCount other)
		{
			return this == other;
		}

		// Token: 0x0600529C RID: 21148 RVA: 0x001AACDC File Offset: 0x001A8EDC
		public static bool operator ==(ThingDefCount a, ThingDefCount b)
		{
			return a.thingDef == b.thingDef && a.count == b.count;
		}

		// Token: 0x0600529D RID: 21149 RVA: 0x001AACFC File Offset: 0x001A8EFC
		public static bool operator !=(ThingDefCount a, ThingDefCount b)
		{
			return !(a == b);
		}

		// Token: 0x0600529E RID: 21150 RVA: 0x001AAD08 File Offset: 0x001A8F08
		public override int GetHashCode()
		{
			return Gen.HashCombine<ThingDef>(this.count, this.thingDef);
		}

		// Token: 0x0600529F RID: 21151 RVA: 0x001AAD1B File Offset: 0x001A8F1B
		public override string ToString()
		{
			return string.Format("({0}x {1})", this.count, (this.thingDef != null) ? this.thingDef.defName : "null");
		}

		// Token: 0x060052A0 RID: 21152 RVA: 0x001AAD4C File Offset: 0x001A8F4C
		public static implicit operator ThingDefCount(ThingDefCountClass t)
		{
			if (t == null)
			{
				return new ThingDefCount(null, 0);
			}
			return new ThingDefCount(t.thingDef, t.count);
		}

		// Token: 0x04003B0E RID: 15118
		private ThingDef thingDef;

		// Token: 0x04003B0F RID: 15119
		private int count;
	}
}
