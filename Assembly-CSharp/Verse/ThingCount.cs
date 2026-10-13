using System;

namespace Verse
{
	// Token: 0x02000C8C RID: 3212
	public struct ThingCount : IEquatable<ThingCount>, IExposable
	{
		// Token: 0x17000CA9 RID: 3241
		// (get) Token: 0x0600527E RID: 21118 RVA: 0x001AA810 File Offset: 0x001A8A10
		public Thing Thing
		{
			get
			{
				return this.thing;
			}
		}

		// Token: 0x17000CAA RID: 3242
		// (get) Token: 0x0600527F RID: 21119 RVA: 0x001AA818 File Offset: 0x001A8A18
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x06005280 RID: 21120 RVA: 0x001AA820 File Offset: 0x001A8A20
		public ThingCount(Thing thing, int count, bool ignoreStackLimit = false)
		{
			if (count < 0)
			{
				Log.Warning("Tried to set ThingCount stack count to " + count.ToString() + ". thing=" + ((thing != null) ? thing.ToString() : null));
				count = 0;
			}
			if (count > thing.stackCount && !ignoreStackLimit)
			{
				Log.Warning(string.Concat(new string[]
				{
					"Tried to set ThingCount stack count to ",
					count.ToString(),
					", but thing's stack count is only ",
					thing.stackCount.ToString(),
					". thing=",
					(thing != null) ? thing.ToString() : null
				}));
				count = thing.stackCount;
			}
			this.thing = thing;
			this.count = count;
		}

		// Token: 0x06005281 RID: 21121 RVA: 0x001AA8CE File Offset: 0x001A8ACE
		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		// Token: 0x06005282 RID: 21122 RVA: 0x001AA8F3 File Offset: 0x001A8AF3
		public ThingCount WithCount(int newCount)
		{
			return new ThingCount(this.thing, newCount, false);
		}

		// Token: 0x06005283 RID: 21123 RVA: 0x001AA902 File Offset: 0x001A8B02
		public override bool Equals(object obj)
		{
			return obj is ThingCount && this.Equals((ThingCount)obj);
		}

		// Token: 0x06005284 RID: 21124 RVA: 0x001AA91A File Offset: 0x001A8B1A
		public bool Equals(ThingCount other)
		{
			return this == other;
		}

		// Token: 0x06005285 RID: 21125 RVA: 0x001AA928 File Offset: 0x001A8B28
		public static bool operator ==(ThingCount a, ThingCount b)
		{
			return a.thing == b.thing && a.count == b.count;
		}

		// Token: 0x06005286 RID: 21126 RVA: 0x001AA948 File Offset: 0x001A8B48
		public static bool operator !=(ThingCount a, ThingCount b)
		{
			return !(a == b);
		}

		// Token: 0x06005287 RID: 21127 RVA: 0x001AA954 File Offset: 0x001A8B54
		public override int GetHashCode()
		{
			return Gen.HashCombine<Thing>(this.count, this.thing);
		}

		// Token: 0x06005288 RID: 21128 RVA: 0x001AA968 File Offset: 0x001A8B68
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.count.ToString(),
				"x ",
				(this.thing != null) ? this.thing.LabelShort : "null",
				")"
			});
		}

		// Token: 0x06005289 RID: 21129 RVA: 0x001AA9C3 File Offset: 0x001A8BC3
		public static implicit operator ThingCount(ThingCountClass t)
		{
			if (t == null)
			{
				return new ThingCount(null, 0, false);
			}
			return new ThingCount(t.thing, t.Count, false);
		}

		// Token: 0x04003B0A RID: 15114
		private Thing thing;

		// Token: 0x04003B0B RID: 15115
		private int count;
	}
}
