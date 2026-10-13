using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F3D RID: 12093
	public class CompTargetEffect_PsychicShock : CompTargetEffect
	{
		// Token: 0x060110EB RID: 69867 RVA: 0x004E6BC8 File Offset: 0x004E4DC8
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = (Pawn)target;
			if (pawn.Dead)
			{
				return;
			}
			Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.PsychicShock, pawn, null);
			BodyPartRecord part;
			pawn.RaceProps.body.GetPartsWithTag(BodyPartTagDefOf.ConsciousnessSource).TryRandomElement(out part);
			BattleLogEntry_ItemUsed battleLogEntry_ItemUsed = new BattleLogEntry_ItemUsed(user, target, this.parent.def, RulePackDefOf.Event_ItemUsed);
			hediff.combatLogEntry = new Verse.WeakReference<LogEntry>(battleLogEntry_ItemUsed);
			hediff.combatLogText = battleLogEntry_ItemUsed.ToGameStringFromPOV(null, false);
			pawn.health.AddHediff(hediff, part, null, null);
			Find.BattleLog.Add(battleLogEntry_ItemUsed);
		}

		// Token: 0x060110EC RID: 69868 RVA: 0x004E6C64 File Offset: 0x004E4E64
		public override bool CanApplyOn(Thing target)
		{
			Pawn pawn = target as Pawn;
			if (pawn != null)
			{
				if (pawn.kindDef.forceDeathOnDowned)
				{
					return false;
				}
				if (pawn.IsMutant && pawn.mutant.Def.psychicShockUntargetable)
				{
					return false;
				}
			}
			return true;
		}
	}
}
