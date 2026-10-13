using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F28 RID: 12072
	public class CompTargetable_SinglePawn : CompTargetable
	{
		// Token: 0x17002C0B RID: 11275
		// (get) Token: 0x060110B7 RID: 69815 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060110B8 RID: 69816 RVA: 0x004E6212 File Offset: 0x004E4412
		protected override TargetingParameters GetTargetingParameters()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false
			};
		}

		// Token: 0x060110B9 RID: 69817 RVA: 0x004E6673 File Offset: 0x004E4873
		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			yield return targetChosenByPlayer;
			yield break;
		}
	}
}
