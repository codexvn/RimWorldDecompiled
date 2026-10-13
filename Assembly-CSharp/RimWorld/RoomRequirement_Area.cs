using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020037A4 RID: 14244
	public class RoomRequirement_Area : RoomRequirement
	{
		// Token: 0x06013BB9 RID: 80825 RVA: 0x005DC6E4 File Offset: 0x005DA8E4
		public override string Label(Room r = null)
		{
			return ((!this.labelKey.NullOrEmpty()) ? this.labelKey : "RoomRequirementArea").Translate(((r != null) ? (r.CellCount.ToString() + "/") : "") + this.area.ToString());
		}

		// Token: 0x06013BBA RID: 80826 RVA: 0x005DC74C File Offset: 0x005DA94C
		public override bool Met(Room r, Pawn p = null)
		{
			return r.CellCount >= this.area;
		}

		// Token: 0x06013BBB RID: 80827 RVA: 0x005DC75F File Offset: 0x005DA95F
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.area <= 0)
			{
				yield return "area must be larger than 0";
			}
			yield break;
			yield break;
		}

		// Token: 0x06013BBC RID: 80828 RVA: 0x005DC76F File Offset: 0x005DA96F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.area, "area", 0, false);
		}

		// Token: 0x0400DAA7 RID: 55975
		public int area;
	}
}
