using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E04 RID: 7684
	public class IncidentWorker_ShamblerAssault : IncidentWorker_RaidEnemy
	{
		// Token: 0x0600AB0E RID: 43790 RVA: 0x0030E615 File Offset: 0x0030C815
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			parms.pawnGroupKind = PawnGroupKindDefOf.Shamblers;
			return base.TryExecuteWorker(parms);
		}

		// Token: 0x0600AB0F RID: 43791 RVA: 0x0030E632 File Offset: 0x0030C832
		protected override bool TryResolveRaidFaction(IncidentParms parms)
		{
			parms.faction = Faction.OfEntities;
			return true;
		}

		// Token: 0x0600AB10 RID: 43792 RVA: 0x0030E640 File Offset: 0x0030C840
		public override void ResolveRaidStrategy(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			parms.raidStrategy = RaidStrategyDefOf.ShamblerAssault;
		}

		// Token: 0x0600AB11 RID: 43793 RVA: 0x0030E650 File Offset: 0x0030C850
		protected override void PostProcessSpawnedPawns(IncidentParms parms, List<Pawn> pawns)
		{
			foreach (Pawn pawn in pawns)
			{
				Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Shambler, false);
				HediffComp_DisappearsAndKills hediffComp_DisappearsAndKills = (firstHediffOfDef != null) ? firstHediffOfDef.TryGetComp<HediffComp_DisappearsAndKills>() : null;
				if (firstHediffOfDef == null || hediffComp_DisappearsAndKills == null)
				{
					Log.ErrorOnce("ShamblerAssault spawned pawn without Shambler hediff", 23407834);
				}
				else
				{
					hediffComp_DisappearsAndKills.disabled = false;
					hediffComp_DisappearsAndKills.ticksToDisappear = IncidentWorker_ShamblerAssault.ShamblerLifespanTicksRange.RandomInRange;
				}
			}
			if (AnomalyIncidentUtility.IncidentShardChance(parms.points))
			{
				AnomalyIncidentUtility.PawnShardOnDeath(pawns.RandomElement<Pawn>());
			}
		}

		// Token: 0x0600AB12 RID: 43794 RVA: 0x0030E42E File Offset: 0x0030C62E
		protected override string GetLetterLabel(IncidentParms parms)
		{
			return parms.raidStrategy.letterLabelEnemy;
		}

		// Token: 0x0600AB13 RID: 43795 RVA: 0x0030E704 File Offset: 0x0030C904
		protected override string GetLetterText(IncidentParms parms, List<Pawn> pawns)
		{
			string text = string.Format(parms.raidArrivalMode.textEnemy, parms.pawnCount).CapitalizeFirst();
			text += "\n\n";
			text += parms.raidStrategy.arrivalTextEnemy;
			if (parms.pawnGroups != null && parms.PawnGroupCount > 1)
			{
				text += "\n\n" + "AttackingFromMultipleDirections".Translate();
			}
			int num = pawns.Count((Pawn e) => e.kindDef == PawnKindDefOf.ShamblerGorehulk);
			if (num == 1)
			{
				text += "\n\n" + "LetterText_ShamblerGorehulk".Translate();
			}
			else if (num > 1)
			{
				text += "\n\n" + "LetterText_ShamblerGorehulkPlural".Translate();
			}
			return text;
		}

		// Token: 0x040075BF RID: 30143
		private static readonly IntRange ShamblerLifespanTicksRange = new IntRange(10000, 25000);
	}
}
