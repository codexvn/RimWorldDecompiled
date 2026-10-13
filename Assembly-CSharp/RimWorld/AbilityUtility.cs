using System;
using System.Collections.Generic;
using LudeonTK;
using Verse;

namespace RimWorld
{
	// Token: 0x020021E4 RID: 8676
	public static class AbilityUtility
	{
		// Token: 0x0600BDFE RID: 48638 RVA: 0x00371BF7 File Offset: 0x0036FDF7
		public static bool ValidateIsConscious(Pawn targetPawn, bool showMessages, Ability ability)
		{
			if (!targetPawn.health.capacities.CanBeAwake)
			{
				if (showMessages)
				{
					AbilityUtility.SendPostProcessedMessage("AbilityCantApplyUnconscious".Translate(targetPawn), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600BDFF RID: 48639 RVA: 0x00371C32 File Offset: 0x0036FE32
		public static bool ValidateIsAwake(Pawn targetPawn, bool showMessages, Ability ability)
		{
			if (!targetPawn.Awake())
			{
				if (showMessages)
				{
					AbilityUtility.SendPostProcessedMessage("AbilityCantApplyAsleep".Translate(targetPawn), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600BE00 RID: 48640 RVA: 0x00371C63 File Offset: 0x0036FE63
		public static bool ValidateNoMentalState(Pawn targetPawn, bool showMessages, Ability ability)
		{
			if (targetPawn.InMentalState)
			{
				if (showMessages)
				{
					AbilityUtility.SendPostProcessedMessage("AbilityCantApplyToMentallyBroken".Translate(targetPawn), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600BE01 RID: 48641 RVA: 0x00371C94 File Offset: 0x0036FE94
		public static bool ValidateIsMaddened(Pawn targetPawn, bool showMessages, Ability ability)
		{
			if (!targetPawn.InMentalState || targetPawn.MentalStateDef != MentalStateDefOf.Manhunter)
			{
				if (showMessages)
				{
					AbilityUtility.SendPostProcessedMessage((targetPawn.health.hediffSet.HasHediff(HediffDefOf.Scaria, false) ? "AbilityCantApplyToScaria" : "AbilityCanOnlyApplyToManhunter").Translate(targetPawn), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600BE02 RID: 48642 RVA: 0x00371CFC File Offset: 0x0036FEFC
		public static bool ValidateCanWalk(Pawn targetPawn, bool showMessages, Ability ability)
		{
			if (targetPawn.Downed)
			{
				if (showMessages)
				{
					AbilityUtility.SendPostProcessedMessage("AbilityCantApplyDowned".Translate(targetPawn), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600BE03 RID: 48643 RVA: 0x00371D30 File Offset: 0x0036FF30
		public static bool ValidateHasMentalState(Pawn targetPawn, bool showMessages, Ability ability)
		{
			if (!targetPawn.InMentalState && !targetPawn.health.hediffSet.HasHediff(HediffDefOf.CatatonicBreakdown, false))
			{
				if (showMessages)
				{
					AbilityUtility.SendPostProcessedMessage("AbilityCanOnlyApplyToMentallyBroken".Translate(targetPawn), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600BE04 RID: 48644 RVA: 0x00371D84 File Offset: 0x0036FF84
		public static bool ValidateHasResistance(Pawn targetPawn, bool showMessages, Ability ability)
		{
			Pawn_GuestTracker guest = targetPawn.guest;
			if (guest != null && guest.resistance <= 1E-45f)
			{
				if (showMessages)
				{
					AbilityUtility.SendPostProcessedMessage("AbilityCantApplyNoResistance".Translate(targetPawn), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600BE05 RID: 48645 RVA: 0x00371DD6 File Offset: 0x0036FFD6
		public static bool ValidateNoInspiration(Pawn targetPawn, bool showMessages, Ability ability)
		{
			if (targetPawn.Inspiration != null)
			{
				if (showMessages)
				{
					AbilityUtility.SendPostProcessedMessage("AbilityAlreadyInspired".Translate(targetPawn), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600BE06 RID: 48646 RVA: 0x00371E07 File Offset: 0x00370007
		public static bool ValidateCanGetInspiration(Pawn targetPawn, bool showMessages, Ability ability)
		{
			if (targetPawn.mindState.inspirationHandler.GetRandomAvailableInspirationDef() == null)
			{
				if (showMessages)
				{
					AbilityUtility.SendPostProcessedMessage("AbilityCantGetInspiredNow".Translate(targetPawn), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600BE07 RID: 48647 RVA: 0x00371E42 File Offset: 0x00370042
		public static bool ValidateMustBeHuman(Pawn targetPawn, bool showMessages, Ability ability)
		{
			if (targetPawn.NonHumanlikeOrWildMan())
			{
				if (showMessages)
				{
					AbilityUtility.SendPostProcessedMessage((targetPawn.IsWildMan() ? "AbilityMustBeHumanNonWild" : "AbilityMustBeHuman").Translate(), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600BE08 RID: 48648 RVA: 0x00371E7C File Offset: 0x0037007C
		public static bool ValidateMustBeHumanOrWildMan(Pawn targetPawn, bool showMessage, Ability ability)
		{
			if (!targetPawn.RaceProps.Humanlike)
			{
				if (showMessage)
				{
					AbilityUtility.SendPostProcessedMessage("AbilityMustBeHuman".Translate(), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600BE09 RID: 48649 RVA: 0x00371EAC File Offset: 0x003700AC
		public static bool ValidateMustNotBeBaby(Pawn targetPawn, bool showMessages, Ability ability)
		{
			if (targetPawn.DevelopmentalStage.Baby())
			{
				if (showMessages)
				{
					AbilityUtility.SendPostProcessedMessage("AbilityCantApplyOnBaby".Translate(), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600BE0A RID: 48650 RVA: 0x00371EDC File Offset: 0x003700DC
		public static bool ValidateMustBeAnimal(Pawn targetPawn, bool showMessages, Ability ability)
		{
			if (!targetPawn.IsAnimal)
			{
				if (showMessages)
				{
					AbilityUtility.SendPostProcessedMessage("AbilityMustBeAnimal".Translate(), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600BE0B RID: 48651 RVA: 0x00371F08 File Offset: 0x00370108
		public static bool ValidateNotSameIdeo(Pawn casterPawn, Pawn targetPawn, bool showMessages, Ability ability)
		{
			if (casterPawn.Ideo != null && casterPawn.Ideo == targetPawn.Ideo)
			{
				if (showMessages)
				{
					AbilityUtility.SendPostProcessedMessage("AbilityMustBeNotSameIdeo".Translate(targetPawn, casterPawn), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600BE0C RID: 48652 RVA: 0x00371F58 File Offset: 0x00370158
		public static bool ValidateSameIdeo(Pawn casterPawn, Pawn targetPawn, bool showMessages, Ability ability)
		{
			if (casterPawn.Ideo != targetPawn.Ideo)
			{
				if (showMessages)
				{
					AbilityUtility.SendPostProcessedMessage("AbilityMustBeSameIdeo".Translate(targetPawn, casterPawn), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600BE0D RID: 48653 RVA: 0x00371F98 File Offset: 0x00370198
		public static bool ValidateSickOrInjured(Pawn targetPawn, bool showMessage, Ability ability)
		{
			bool flag = false;
			bool flag2 = false;
			foreach (Hediff hediff in targetPawn.health.hediffSet.hediffs)
			{
				flag |= (hediff.TryGetComp<HediffComp_Immunizable>() != null);
				bool flag3 = flag2;
				Hediff_Injury hediff_Injury = hediff as Hediff_Injury;
				flag2 = (flag3 | (hediff_Injury != null && !hediff_Injury.IsPermanent() && hediff_Injury.CanHealNaturally()));
			}
			if (!flag && !flag2)
			{
				if (showMessage)
				{
					AbilityUtility.SendPostProcessedMessage("AbilityMustBeSickOrInjured".Translate(targetPawn), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600BE0E RID: 48654 RVA: 0x00372050 File Offset: 0x00370250
		public static bool ValidateNotGuilty(Pawn targetPawn, bool showMessages, Ability ability)
		{
			if (targetPawn.guilt != null && targetPawn.guilt.IsGuilty)
			{
				if (showMessages)
				{
					AbilityUtility.SendPostProcessedMessage("AbilityMustBeNotGuilty".Translate(targetPawn), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600BE0F RID: 48655 RVA: 0x00372090 File Offset: 0x00370290
		public static bool ValidateHasTendableWound(Pawn targetPawn, bool showMessages, Ability ability)
		{
			List<Hediff> hediffs = targetPawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if ((hediffs[i] is Hediff_Injury || hediffs[i] is Hediff_MissingPart) && hediffs[i].TendableNow(false))
				{
					return true;
				}
			}
			if (showMessages)
			{
				AbilityUtility.SendPostProcessedMessage("AbilityMustHaveTendableWound".Translate(targetPawn), targetPawn, ability);
			}
			return false;
		}

		// Token: 0x0600BE10 RID: 48656 RVA: 0x00372114 File Offset: 0x00370314
		private static void SendPostProcessedMessage(string message, LookTargets targets, Ability ability)
		{
			if (ability != null)
			{
				message = "CannotUseAbility".Translate(ability.def.label) + ": " + message;
			}
			Messages.Message(message, targets, MessageTypeDefOf.RejectInput, false);
		}

		// Token: 0x0600BE11 RID: 48657 RVA: 0x00372162 File Offset: 0x00370362
		public static void DoClamor(IntVec3 cell, float radius, Thing source, ClamorDef clamor)
		{
			if (clamor != null)
			{
				GenClamor.DoClamor(source, cell, radius, clamor);
			}
		}

		// Token: 0x0600BE12 RID: 48658 RVA: 0x00372170 File Offset: 0x00370370
		public static bool IsIncendiary(this Ability ability)
		{
			return ability.def.ai_IsIncendiary;
		}

		// Token: 0x0600BE13 RID: 48659 RVA: 0x0037217D File Offset: 0x0037037D
		public static Ability MakeAbility(AbilityDef def, Pawn pawn)
		{
			return Activator.CreateInstance(def.abilityClass, new object[]
			{
				pawn,
				def
			}) as Ability;
		}

		// Token: 0x0600BE14 RID: 48660 RVA: 0x0037219D File Offset: 0x0037039D
		public static Ability MakeAbility(AbilityDef def, Pawn pawn, Precept sourcePrecept)
		{
			return Activator.CreateInstance(def.abilityClass, new object[]
			{
				pawn,
				sourcePrecept,
				def
			}) as Ability;
		}

		// Token: 0x0600BE15 RID: 48661 RVA: 0x003721C4 File Offset: 0x003703C4
		public static void DoTable_AbilityCosts()
		{
			List<TableDataGetter<AbilityDef>> list = new List<TableDataGetter<AbilityDef>>();
			list.Add(new TableDataGetter<AbilityDef>("name", (AbilityDef a) => a.LabelCap));
			list.Add(new TableDataGetter<AbilityDef>("level", (AbilityDef a) => a.level));
			list.Add(new TableDataGetter<AbilityDef>("heat cost", (AbilityDef a) => a.EntropyGain));
			list.Add(new TableDataGetter<AbilityDef>("psyfocus cost", delegate(AbilityDef a)
			{
				if (a.PsyfocusCostRange.Span > 1E-45f)
				{
					return (a.PsyfocusCostRange.min * 100f).ToString() + "-" + (a.PsyfocusCostRange.max * 100f).ToString() + "%";
				}
				return a.PsyfocusCostRange.max.ToStringPercent();
			}));
			list.Add(new TableDataGetter<AbilityDef>("max psyfocus recovery time days", delegate(AbilityDef a)
			{
				if (a.PsyfocusCostRange.Span <= 1E-45f)
				{
					return a.PsyfocusCostRange.min / StatDefOf.MeditationFocusGain.defaultBaseValue;
				}
				return (a.PsyfocusCostRange.min / StatDefOf.MeditationFocusGain.defaultBaseValue).ToString() + "-" + (a.PsyfocusCostRange.max / StatDefOf.MeditationFocusGain.defaultBaseValue).ToString();
			}));
			DebugTables.MakeTablesDialog<AbilityDef>(DefDatabase<AbilityDef>.AllDefsListForReading, list.ToArray());
		}
	}
}
