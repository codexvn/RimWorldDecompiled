using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001751 RID: 5969
	public class RaidStrategyWorker_ShamblerAssault : RaidStrategyWorker_ImmediateAttack
	{
		// Token: 0x06008C1B RID: 35867 RVA: 0x0029B49E File Offset: 0x0029969E
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			return new LordJob_ShamblerAssault();
		}
	}
}
