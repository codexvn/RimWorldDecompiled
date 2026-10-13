using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020037AE RID: 14254
	public class RoomRequirement_ThingAnyOfCount : RoomRequirement_ThingAnyOf
	{
		// Token: 0x06013C08 RID: 80904 RVA: 0x005DD3F0 File Offset: 0x005DB5F0
		public override string Label(Room r = null)
		{
			bool flag = !this.labelKey.NullOrEmpty();
			string text = flag ? this.labelKey.Translate() : this.things[0].label;
			if (r != null)
			{
				return string.Concat(new string[]
				{
					text,
					" ",
					this.ThingCount(r).ToString(),
					"/",
					this.count.ToString()
				});
			}
			if (!flag)
			{
				return GenLabel.ThingLabel(this.things[0], null, this.count);
			}
			return text + " x" + this.count.ToString();
		}

		// Token: 0x06013C09 RID: 80905 RVA: 0x005DD4A8 File Offset: 0x005DB6A8
		public override bool Met(Room r, Pawn p = null)
		{
			return this.ThingCount(r) >= this.count;
		}

		// Token: 0x06013C0A RID: 80906 RVA: 0x005DD4BC File Offset: 0x005DB6BC
		public override bool SameOrSubsetOf(RoomRequirement other)
		{
			return base.SameOrSubsetOf(other) && this.count <= (other as RoomRequirement_ThingAnyOfCount).count;
		}

		// Token: 0x06013C0B RID: 80907 RVA: 0x005DD4E0 File Offset: 0x005DB6E0
		private int ThingCount(Room r)
		{
			int num = 0;
			foreach (Thing t in r.ContainedThingsList(this.things))
			{
				if (this.CountThing(t))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06013C0C RID: 80908 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool CountThing(Thing t)
		{
			return true;
		}

		// Token: 0x06013C0D RID: 80909 RVA: 0x005DD53C File Offset: 0x005DB73C
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.count <= 0)
			{
				yield return "count must be larger than 0";
			}
			yield break;
			yield break;
		}

		// Token: 0x06013C0E RID: 80910 RVA: 0x005DD54C File Offset: 0x005DB74C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
		}

		// Token: 0x0400DAC5 RID: 56005
		public int count;
	}
}
