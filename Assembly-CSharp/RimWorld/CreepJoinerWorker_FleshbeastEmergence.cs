using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020022CF RID: 8911
	public class CreepJoinerWorker_FleshbeastEmergence : BaseCreepJoinerWorker
	{
		// Token: 0x17001DA9 RID: 7593
		// (get) Token: 0x0600C1E1 RID: 49633 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool CanOccurOnDeath
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C1E2 RID: 49634 RVA: 0x00382D9C File Offset: 0x00380F9C
		public override void DoResponse(List<TargetInfo> looktargets, List<NamedArgument> namedArgs)
		{
			Pawn t = FleshbeastUtility.SpawnFleshbeastFromPawn(base.Pawn, true, false, new PawnKindDef[]
			{
				PawnKindDefOf.Bulbfreak
			});
			looktargets.Add(t);
		}
	}
}
