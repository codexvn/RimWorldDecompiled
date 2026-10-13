using System;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200362F RID: 13871
	public static class ExecutionUtility
	{
		// Token: 0x06013433 RID: 78899 RVA: 0x005B707C File Offset: 0x005B527C
		public static void DoExecutionByCut(Pawn executioner, Pawn victim, int bloodPerWeight = 8, bool spawnBlood = true)
		{
			ExecutionUtility.ExecutionInt(executioner, victim, false, bloodPerWeight, spawnBlood);
		}

		// Token: 0x06013434 RID: 78900 RVA: 0x005B7088 File Offset: 0x005B5288
		public static void DoHuntingExecution(Pawn executioner, Pawn victim)
		{
			ExecutionUtility.ExecutionInt(executioner, victim, true, 8, true);
		}

		// Token: 0x06013435 RID: 78901 RVA: 0x005B7094 File Offset: 0x005B5294
		private static void ExecutionInt(Pawn executioner, Pawn victim, bool huntingExecution = false, int bloodPerWeight = 8, bool spawnBlood = true)
		{
			if (spawnBlood)
			{
				int num = Mathf.Max(GenMath.RoundRandom(victim.BodySize * (float)bloodPerWeight), 1);
				for (int i = 0; i < num; i++)
				{
					victim.health.DropBloodFilth();
				}
			}
			if (!huntingExecution && victim.RaceProps.Animal && ModsConfig.IdeologyActive)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.SlaughteredAnimal, executioner.Named(HistoryEventArgsNames.Doer)), true);
			}
			BodyPartRecord bodyPartRecord = ExecutionUtility.ExecuteCutPart(victim);
			int num2 = (int)victim.health.hediffSet.GetPartHealth(bodyPartRecord);
			DamageInfo damageInfo = new DamageInfo(DamageDefOf.ExecutionCut, (float)Mathf.Min(num2 - 1, 1), 999f, -1f, executioner, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown, null, false, spawnBlood, QualityCategory.Normal, true, false);
			damageInfo.SetIgnoreArmor(true);
			if (ModsConfig.BiotechActive && victim.genes != null && victim.genes.HasActiveGene(GeneDefOf.Deathless))
			{
				damageInfo.SetAmount(9999f);
				damageInfo.SetAllowDamagePropagation(false);
			}
			victim.TakeDamage(damageInfo);
			if (!victim.Dead)
			{
				victim.Kill(new DamageInfo?(damageInfo), null);
			}
			SoundDefOf.Execute_Cut.PlayOneShot(victim);
		}

		// Token: 0x06013436 RID: 78902 RVA: 0x005B71BC File Offset: 0x005B53BC
		public static BodyPartRecord ExecuteCutPart(Pawn pawn)
		{
			BodyPartRecord bodyPartRecord = null;
			if (ModsConfig.BiotechActive && pawn.genes != null && pawn.genes.HasActiveGene(GeneDefOf.Deathless))
			{
				bodyPartRecord = pawn.health.hediffSet.GetBrain();
			}
			if (bodyPartRecord == null)
			{
				bodyPartRecord = (from x in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
				where x.def.executionPartPriority > 0f
				orderby x.def.executionPartPriority descending
				select x).FirstOrDefault<BodyPartRecord>();
			}
			if (bodyPartRecord != null)
			{
				return bodyPartRecord;
			}
			Log.Error("No good execution cut part found for " + ((pawn != null) ? pawn.ToString() : null));
			return pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).RandomElementByWeight((BodyPartRecord x) => x.coverageAbsWithChildren);
		}
	}
}
