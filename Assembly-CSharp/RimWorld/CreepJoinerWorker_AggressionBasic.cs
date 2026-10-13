using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020022CE RID: 8910
	public class CreepJoinerWorker_AggressionBasic : BaseCreepJoinerWorker
	{
		// Token: 0x0600C1DF RID: 49631 RVA: 0x00382D10 File Offset: 0x00380F10
		public override void DoResponse(List<TargetInfo> looktargets, List<NamedArgument> namedArgs)
		{
			base.Pawn.SetFaction(Find.FactionManager.OfEntities, null);
			base.Pawn.guest.Recruitable = false;
			Lord lord = base.Pawn.GetLord();
			if (lord != null)
			{
				lord.RemovePawn(base.Pawn);
			}
			lord = LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_AssaultColony(Faction.OfEntities, false, false, false, false, false, false, false), base.Pawn.MapHeld, null);
			lord.AddPawn(base.Pawn);
		}
	}
}
