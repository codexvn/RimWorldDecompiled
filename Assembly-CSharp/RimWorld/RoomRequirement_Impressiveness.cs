using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020037B0 RID: 14256
	public class RoomRequirement_Impressiveness : RoomRequirement
	{
		// Token: 0x06013C1A RID: 80922 RVA: 0x005DD710 File Offset: 0x005DB910
		public override string Label(Room r = null)
		{
			return ((!this.labelKey.NullOrEmpty()) ? this.labelKey : "RoomRequirementImpressiveness").Translate() + " " + ((r != null) ? (Mathf.Round(r.GetStat(RoomStatDefOf.Impressiveness)).ToString() + "/") : "") + this.impressiveness.ToString();
		}

		// Token: 0x06013C1B RID: 80923 RVA: 0x005DD78C File Offset: 0x005DB98C
		public override bool Met(Room r, Pawn p = null)
		{
			return Mathf.Round(r.GetStat(RoomStatDefOf.Impressiveness)) >= (float)this.impressiveness;
		}

		// Token: 0x06013C1C RID: 80924 RVA: 0x005DD7AA File Offset: 0x005DB9AA
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.impressiveness <= 0)
			{
				yield return "impressiveness must be larger than 0";
			}
			yield break;
			yield break;
		}

		// Token: 0x06013C1D RID: 80925 RVA: 0x005DD7BA File Offset: 0x005DB9BA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.impressiveness, "impressiveness", 0, false);
		}

		// Token: 0x0400DACB RID: 56011
		public int impressiveness;
	}
}
