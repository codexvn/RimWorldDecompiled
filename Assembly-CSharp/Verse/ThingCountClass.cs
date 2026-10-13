using System;

namespace Verse
{
	// Token: 0x02000C8E RID: 3214
	public sealed class ThingCountClass : IExposable
	{
		// Token: 0x17000CAB RID: 3243
		// (get) Token: 0x0600528C RID: 21132 RVA: 0x001AAA92 File Offset: 0x001A8C92
		// (set) Token: 0x0600528D RID: 21133 RVA: 0x001AAA9C File Offset: 0x001A8C9C
		public int Count
		{
			get
			{
				return this.countInt;
			}
			set
			{
				if (value < 0)
				{
					string str = "Tried to set ThingCountClass stack count to ";
					string str2 = value.ToString();
					string str3 = ". thing=";
					Thing thing = this.thing;
					Log.Warning(str + str2 + str3 + ((thing != null) ? thing.ToString() : null));
					this.countInt = 0;
					return;
				}
				if (this.thing != null && value > this.thing.stackCount)
				{
					string[] array = new string[6];
					array[0] = "Tried to set ThingCountClass stack count to ";
					array[1] = value.ToString();
					array[2] = ", but thing's stack count is only ";
					array[3] = this.thing.stackCount.ToString();
					array[4] = ". thing=";
					int num = 5;
					Thing thing2 = this.thing;
					array[num] = ((thing2 != null) ? thing2.ToString() : null);
					Log.Warning(string.Concat(array));
					this.countInt = this.thing.stackCount;
					return;
				}
				this.countInt = value;
			}
		}

		// Token: 0x0600528E RID: 21134 RVA: 0x000024DF File Offset: 0x000006DF
		public ThingCountClass()
		{
		}

		// Token: 0x0600528F RID: 21135 RVA: 0x001AAB6B File Offset: 0x001A8D6B
		public ThingCountClass(Thing thing, int count)
		{
			this.thing = thing;
			this.Count = count;
		}

		// Token: 0x06005290 RID: 21136 RVA: 0x001AAB81 File Offset: 0x001A8D81
		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
			Scribe_Values.Look<int>(ref this.countInt, "count", 1, false);
		}

		// Token: 0x06005291 RID: 21137 RVA: 0x001AABA8 File Offset: 0x001A8DA8
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.Count.ToString(),
				"x ",
				(this.thing != null) ? this.thing.LabelShort : "null",
				")"
			});
		}

		// Token: 0x06005292 RID: 21138 RVA: 0x001AAC06 File Offset: 0x001A8E06
		public static implicit operator ThingCountClass(ThingCount t)
		{
			return new ThingCountClass(t.Thing, t.Count);
		}

		// Token: 0x04003B0C RID: 15116
		public Thing thing;

		// Token: 0x04003B0D RID: 15117
		private int countInt;
	}
}
