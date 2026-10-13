using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020037AA RID: 14250
	public class RoomRequirement_ThingCount : RoomRequirement_Thing
	{
		// Token: 0x06013BE7 RID: 80871 RVA: 0x005DCE1F File Offset: 0x005DB01F
		public override bool Met(Room r, Pawn p = null)
		{
			return this.Count(r) >= this.count;
		}

		// Token: 0x06013BE8 RID: 80872 RVA: 0x005DCE33 File Offset: 0x005DB033
		public int Count(Room r)
		{
			return r.ThingCount(this.thingDef);
		}

		// Token: 0x06013BE9 RID: 80873 RVA: 0x005DCE44 File Offset: 0x005DB044
		public override string Label(Room r = null)
		{
			bool flag = !this.labelKey.NullOrEmpty();
			string text = flag ? this.labelKey.Translate() : this.thingDef.label;
			if (r != null)
			{
				return string.Concat(new string[]
				{
					text,
					" ",
					this.Count(r).ToString(),
					"/",
					this.count.ToString()
				});
			}
			if (!flag)
			{
				return GenLabel.ThingLabel(this.thingDef, null, this.count);
			}
			return text + " x" + this.count.ToString();
		}

		// Token: 0x06013BEA RID: 80874 RVA: 0x005DCEF0 File Offset: 0x005DB0F0
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

		// Token: 0x06013BEB RID: 80875 RVA: 0x005DCF00 File Offset: 0x005DB100
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
		}

		// Token: 0x0400DAB9 RID: 55993
		public int count;
	}
}
