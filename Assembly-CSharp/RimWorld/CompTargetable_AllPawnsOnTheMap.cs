using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F21 RID: 12065
	public class CompTargetable_AllPawnsOnTheMap : CompTargetable
	{
		// Token: 0x17002C01 RID: 11265
		// (get) Token: 0x0601108E RID: 69774 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0601108F RID: 69775 RVA: 0x004E6212 File Offset: 0x004E4412
		protected override TargetingParameters GetTargetingParameters()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false
			};
		}

		// Token: 0x06011090 RID: 69776 RVA: 0x004E6227 File Offset: 0x004E4427
		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			if (this.parent.MapHeld == null)
			{
				yield break;
			}
			TargetingParameters tp = this.GetTargetingParameters();
			foreach (Pawn pawn in this.parent.MapHeld.mapPawns.AllPawnsSpawned)
			{
				if (tp.CanTarget(pawn, null) && this.ValidateTarget(pawn, true))
				{
					yield return pawn;
				}
			}
			IEnumerator<Pawn> enumerator = null;
			yield break;
			yield break;
		}
	}
}
