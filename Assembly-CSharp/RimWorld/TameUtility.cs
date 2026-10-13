using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020030CB RID: 12491
	public static class TameUtility
	{
		// Token: 0x0601189B RID: 71835 RVA: 0x00509190 File Offset: 0x00507390
		public static void ShowDesignationWarnings(Pawn pawn, bool showManhunterOnTameFailWarning = true)
		{
			if (showManhunterOnTameFailWarning)
			{
				float manhunterOnTameFailChance = PawnUtility.GetManhunterOnTameFailChance(pawn);
				if (manhunterOnTameFailChance >= 0.015f)
				{
					Messages.Message("MessageAnimalManhuntsOnTameFailed".Translate(pawn.kindDef.GetLabelPlural(-1).CapitalizeFirst(), manhunterOnTameFailChance.ToStringPercent(), pawn.Named("ANIMAL")), pawn, MessageTypeDefOf.CautionInput, false);
				}
			}
			if (pawn.RaceProps.Insect && pawn.HostileTo(Faction.OfPlayer))
			{
				Messages.Message("MessageInsectsHostileOnTaming".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.CautionInput, false);
			}
			IEnumerable<Pawn> source = from c in pawn.Map.mapPawns.FreeColonistsSpawned
			where c.workSettings.WorkIsActive(WorkTypeDefOf.Handling)
			select c;
			if (!source.Any<Pawn>())
			{
				source = pawn.Map.mapPawns.FreeColonistsSpawned;
			}
			if (source.Any<Pawn>())
			{
				IEnumerable<Pawn> source2 = from p in source
				where p.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && p.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation)
				select p;
				if (source2.Any<Pawn>())
				{
					Pawn pawn2 = source2.MaxBy((Pawn p) => p.skills.GetSkill(SkillDefOf.Animals).Level);
					int level = pawn2.skills.GetSkill(SkillDefOf.Animals).Level;
					int num = TrainableUtility.MinimumHandlingSkill(pawn);
					if (num > level)
					{
						Messages.Message("MessageNoHandlerSkilledEnough".Translate(pawn.kindDef.label, num.ToStringCached(), SkillDefOf.Animals.LabelCap, pawn2.LabelShort, level, pawn.Named("ANIMAL"), pawn2.Named("HANDLER")), pawn, MessageTypeDefOf.CautionInput, false);
					}
				}
			}
		}

		// Token: 0x0601189C RID: 71836 RVA: 0x00509388 File Offset: 0x00507588
		public static bool CanTame(Pawn pawn)
		{
			return pawn.AnimalOrWildMan() && (pawn.Faction == null || !pawn.Faction.def.humanlikeFaction) && pawn.GetStatValue(StatDefOf.Wildness, true, -1) < 1f && pawn.RaceProps.animalType != AnimalType.Dryad && !pawn.health.hediffSet.HasHediff(HediffDefOf.Scaria, false);
		}

		// Token: 0x0601189D RID: 71837 RVA: 0x005093F4 File Offset: 0x005075F4
		public static bool TriedToTameTooRecently(Pawn animal)
		{
			return Find.TickManager.TicksGame < animal.mindState.lastAssignedInteractTime + 30000;
		}

		// Token: 0x0400B509 RID: 46345
		public const int MinTameInterval = 30000;
	}
}
