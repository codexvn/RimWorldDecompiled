using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020023EE RID: 9198
	public static class SanguophageUtility
	{
		// Token: 0x0600C776 RID: 51062 RVA: 0x003A1E18 File Offset: 0x003A0018
		public static bool ShouldBeDeathrestingOrInComaInsteadOfDead(Pawn pawn)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			if (!pawn.health.ShouldBeDead())
			{
				return false;
			}
			if (pawn.genes != null && pawn.genes.HasActiveGene(GeneDefOf.Deathless))
			{
				BodyPartRecord brain = pawn.health.hediffSet.GetBrain();
				if (brain != null && !pawn.health.hediffSet.PartIsMissing(brain) && pawn.health.hediffSet.GetPartHealth(brain) > 0f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C777 RID: 51063 RVA: 0x003A1E9C File Offset: 0x003A009C
		public static bool TryStartDeathrest(Pawn pawn, DeathrestStartReason reason)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			if (!pawn.Spawned)
			{
				return false;
			}
			if (pawn.Deathresting)
			{
				return false;
			}
			Pawn_GeneTracker genes = pawn.genes;
			Gene_Deathrest gene_Deathrest = (genes != null) ? genes.GetFirstGeneOfType<Gene_Deathrest>() : null;
			if (gene_Deathrest == null)
			{
				return false;
			}
			TaggedString label = "LetterLabelInvoluntaryDeathrest".Translate() + ": " + pawn.LabelShortCap;
			TaggedString taggedString = "LetterTextInvoluntaryDeathrestOrComa".Translate(pawn.Named("PAWN"), "LetterTextDeathrest".Translate(pawn.Named("PAWN")).Named("HEDIFFINFO"));
			if (reason == DeathrestStartReason.LethalDamage)
			{
				SanguophageUtility.DoXPLossFromDamage(pawn, ref taggedString);
				taggedString += "\n\n" + "Reason".Translate() + ": " + "DeathrestLethalDamage".Translate();
			}
			if (PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				Find.LetterStack.ReceiveLetter(label, taggedString, LetterDefOf.NegativeEvent, pawn, null, null, null, null, 0, true);
			}
			gene_Deathrest.autoWake = (reason > DeathrestStartReason.PlayerForced);
			pawn.health.AddHediff(HediffDefOf.Deathrest, null, null, null);
			return true;
		}

		// Token: 0x0600C778 RID: 51064 RVA: 0x003A1FC4 File Offset: 0x003A01C4
		public static bool TryStartRegenComa(Pawn pawn, DeathrestStartReason reason)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			if (!pawn.health.hediffSet.HasHediff(HediffDefOf.RegenerationComa, false))
			{
				TaggedString label = "LetterLabelRegenerationComa".Translate() + ": " + pawn.LabelShortCap;
				TaggedString text = "LetterTextInvoluntaryDeathrestOrComa".Translate(pawn.Named("PAWN"), "LetterTextRegenerationComa".Translate(pawn.Named("PAWN")).Named("HEDIFFINFO"));
				if (reason == DeathrestStartReason.LethalDamage)
				{
					SanguophageUtility.DoXPLossFromDamage(pawn, ref text);
				}
				if (PawnUtility.ShouldSendNotificationAbout(pawn))
				{
					Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NegativeEvent, pawn, null, null, null, null, 0, true);
				}
				pawn.health.AddHediff(HediffDefOf.RegenerationComa, null, null, null);
				return true;
			}
			return false;
		}

		// Token: 0x0600C779 RID: 51065 RVA: 0x003A20A0 File Offset: 0x003A02A0
		public static void DoXPLossFromDamage(Pawn pawn, ref TaggedString letterText)
		{
			Pawn_GeneTracker genes = pawn.genes;
			Gene_Deathless gene_Deathless = (genes != null) ? genes.GetFirstGeneOfType<Gene_Deathless>() : null;
			if (gene_Deathless != null && Find.TickManager.TicksGame - gene_Deathless.lastSkillReductionTick >= 60000)
			{
				SkillRecord skillRecord;
				if ((from x in pawn.skills.skills
				where !x.TotallyDisabled && x.XpTotalEarned > 0f
				select x).TryRandomElementByWeight((SkillRecord x) => (float)x.GetLevel(true) * 10f, out skillRecord))
				{
					float num = skillRecord.XpTotalEarned * SanguophageUtility.XPLossPercentFromDeathrestRange.RandomInRange;
					letterText += "\n\n" + "DeathrestLostSkill".Translate(pawn.Named("PAWN"), skillRecord.def.label.Named("SKILL"), ((int)num).Named("XPLOSS"));
					skillRecord.Learn(-num, true, false);
					gene_Deathless.lastSkillReductionTick = Find.TickManager.TicksGame;
				}
			}
		}

		// Token: 0x0600C77A RID: 51066 RVA: 0x003A21C0 File Offset: 0x003A03C0
		public static bool InSunlight(this IntVec3 cell, Map map)
		{
			return cell.InBounds(map) && !map.roofGrid.Roofed(cell) && map.skyManager.CurSkyGlow > 0.1f;
		}

		// Token: 0x0600C77B RID: 51067 RVA: 0x003A21F0 File Offset: 0x003A03F0
		public static string DeathrestJobReport(Pawn pawn)
		{
			Hediff_Deathrest hediff_Deathrest = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Deathrest, false) as Hediff_Deathrest;
			if (hediff_Deathrest != null && hediff_Deathrest.Paused)
			{
				return "DeathrestPaused".Translate() + ": " + "LethalInjuries".Translate();
			}
			Gene_Deathrest firstGeneOfType = pawn.genes.GetFirstGeneOfType<Gene_Deathrest>();
			TaggedString t = "Deathresting".Translate().CapitalizeFirst() + ": ";
			float deathrestPercent = firstGeneOfType.DeathrestPercent;
			if (deathrestPercent < 1f)
			{
				t += Mathf.Min(deathrestPercent, 0.99f).ToStringPercent("F0");
			}
			else
			{
				t += string.Format("{0} - {1}", "Complete".Translate().CapitalizeFirst(), "CanWakeSafely".Translate());
			}
			if (deathrestPercent < 1f)
			{
				t += ", " + "DurationLeft".Translate((firstGeneOfType.MinDeathrestTicks - firstGeneOfType.deathrestTicks).ToStringTicksToPeriod(true, false, true, true, false));
			}
			return t.Resolve();
		}

		// Token: 0x0600C77C RID: 51068 RVA: 0x003A2324 File Offset: 0x003A0524
		public static bool WouldDieFromAdditionalBloodLoss(this Pawn pawn, float severity)
		{
			if (pawn.Dead || !pawn.RaceProps.IsFlesh)
			{
				return false;
			}
			float num = severity;
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss, false);
			if (firstHediffOfDef != null)
			{
				num += firstHediffOfDef.Severity;
			}
			return num >= HediffDefOf.BloodLoss.lethalSeverity || HediffDefOf.BloodLoss.stages[HediffDefOf.BloodLoss.StageAtSeverity(num)].lifeThreatening;
		}

		// Token: 0x0600C77D RID: 51069 RVA: 0x003A23A0 File Offset: 0x003A05A0
		public static void DoBite(Pawn biter, Pawn victim, float targetHemogenGain, float nutritionGain, float targetBloodLoss, float victimResistanceGain, IntRange bloodFilthToSpawnRange, ThoughtDef thoughtDefToGiveTarget = null, ThoughtDef opinionThoughtToGiveTarget = null)
		{
			if (!ModLister.CheckBiotech("Sanguophage bite"))
			{
				return;
			}
			float num = SanguophageUtility.HemogenGainBloodlossFactor(victim, targetBloodLoss);
			float num2 = targetHemogenGain * victim.BodySize * num;
			GeneUtility.OffsetHemogen(biter, num2, true);
			GeneUtility.OffsetHemogen(victim, -num2, true);
			Pawn_NeedsTracker needs = biter.needs;
			if (((needs != null) ? needs.food : null) != null)
			{
				biter.needs.food.CurLevel += nutritionGain * num;
			}
			if (thoughtDefToGiveTarget != null)
			{
				Pawn_NeedsTracker needs2 = victim.needs;
				if (needs2 != null)
				{
					Need_Mood mood = needs2.mood;
					if (mood != null)
					{
						ThoughtHandler thoughts = mood.thoughts;
						if (thoughts != null)
						{
							MemoryThoughtHandler memories = thoughts.memories;
							if (memories != null)
							{
								memories.TryGainMemory((Thought_Memory)ThoughtMaker.MakeThought(thoughtDefToGiveTarget), biter);
							}
						}
					}
				}
			}
			if (opinionThoughtToGiveTarget != null)
			{
				Pawn_NeedsTracker needs3 = victim.needs;
				if (needs3 != null)
				{
					Need_Mood mood2 = needs3.mood;
					if (mood2 != null)
					{
						ThoughtHandler thoughts2 = mood2.thoughts;
						if (thoughts2 != null)
						{
							MemoryThoughtHandler memories2 = thoughts2.memories;
							if (memories2 != null)
							{
								memories2.TryGainMemory((Thought_Memory)ThoughtMaker.MakeThought(opinionThoughtToGiveTarget), biter);
							}
						}
					}
				}
			}
			if (targetBloodLoss > 0f)
			{
				victim.health.AddHediff(HediffDefOf.BloodfeederMark, ExecutionUtility.ExecuteCutPart(victim), null, null);
				Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.BloodLoss, victim, null);
				hediff.Severity = targetBloodLoss;
				victim.health.AddHediff(hediff, null, null, null);
			}
			if (victim.IsPrisoner && victimResistanceGain > 0f)
			{
				victim.guest.resistance = Mathf.Min(victim.guest.resistance + victimResistanceGain, victim.kindDef.initialResistanceRange.Value.TrueMax);
			}
			int randomInRange = bloodFilthToSpawnRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				IntVec3 c = victim.Position;
				if (randomInRange > 1 && Rand.Chance(0.8888f))
				{
					c = victim.Position.RandomAdjacentCell8Way();
				}
				if (c.InBounds(victim.MapHeld))
				{
					FilthMaker.TryMakeFilth(c, victim.MapHeld, victim.RaceProps.BloodDef, victim.LabelShort, 1, FilthSourceFlags.None);
				}
			}
		}

		// Token: 0x0600C77E RID: 51070 RVA: 0x003A25A0 File Offset: 0x003A07A0
		public static float HemogenGainBloodlossFactor(Pawn pawn, float targetBloodloss)
		{
			if (targetBloodloss > 0f)
			{
				Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss, false);
				if (firstHediffOfDef != null)
				{
					return Mathf.Min(1f, (HediffDefOf.BloodLoss.lethalSeverity - firstHediffOfDef.Severity) / targetBloodloss);
				}
			}
			return 1f;
		}

		// Token: 0x0600C77F RID: 51071 RVA: 0x003A25F4 File Offset: 0x003A07F4
		public static bool CanSafelyBeQueuedForHemogenExtraction(Pawn pawn)
		{
			if (ModsConfig.BiotechActive && pawn.Spawned && pawn.BillStack != null)
			{
				if (!pawn.BillStack.Bills.Any((Bill x) => x.recipe == RecipeDefOf.ExtractHemogenPack) && SanguophageUtility.PawnConsciousEnoughForExtraction(pawn) && RecipeDefOf.ExtractHemogenPack.Worker.AvailableOnNow(pawn, null))
				{
					return !pawn.health.hediffSet.HasHediff(HediffDefOf.BloodLoss, false);
				}
			}
			return false;
		}

		// Token: 0x0600C780 RID: 51072 RVA: 0x003A267F File Offset: 0x003A087F
		private static bool PawnConsciousEnoughForExtraction(Pawn pawn)
		{
			return pawn.health.capacities.GetLevel(PawnCapacityDefOf.Consciousness) > 0.45f;
		}

		// Token: 0x04008801 RID: 34817
		private static readonly FloatRange XPLossPercentFromDeathrestRange = new FloatRange(0.03f, 0.06f);

		// Token: 0x04008802 RID: 34818
		private const float MinConsciousnessForHemogenExtraction = 0.45f;
	}
}
