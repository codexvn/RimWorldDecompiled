using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000764 RID: 1892
	public class Hediff_Psylink : Hediff_Level
	{
		// Token: 0x06002E68 RID: 11880 RVA: 0x000EECB1 File Offset: 0x000ECEB1
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			this.TryGiveAbilityOfLevel(this.level, !this.suppressPostAddLetter);
			Pawn_PsychicEntropyTracker psychicEntropy = this.pawn.psychicEntropy;
			if (psychicEntropy == null)
			{
				return;
			}
			psychicEntropy.Notify_GainedPsylink();
		}

		// Token: 0x06002E69 RID: 11881 RVA: 0x000EECE4 File Offset: 0x000ECEE4
		public void ChangeLevel(int levelOffset, bool sendLetter)
		{
			if (levelOffset > 0)
			{
				float num = Math.Min((float)levelOffset, this.def.maxSeverity - (float)this.level);
				int num2 = 0;
				while ((float)num2 < num)
				{
					int abilityLevel = this.level + 1 + num2;
					this.TryGiveAbilityOfLevel(abilityLevel, sendLetter);
					Pawn_PsychicEntropyTracker psychicEntropy = this.pawn.psychicEntropy;
					if (psychicEntropy != null)
					{
						psychicEntropy.Notify_GainedPsylink();
					}
					num2++;
				}
			}
			base.ChangeLevel(levelOffset);
		}

		// Token: 0x06002E6A RID: 11882 RVA: 0x000EED4D File Offset: 0x000ECF4D
		public override void ChangeLevel(int levelOffset)
		{
			this.ChangeLevel(levelOffset, true);
		}

		// Token: 0x06002E6B RID: 11883 RVA: 0x000EED58 File Offset: 0x000ECF58
		public static string MakeLetterTextNewPsylinkLevel(Pawn pawn, int abilityLevel, IEnumerable<AbilityDef> newAbilities = null)
		{
			string text = ((abilityLevel == 1) ? "LetterPsylinkLevelGained_First" : "LetterPsylinkLevelGained_NotFirst").Translate(pawn.Named("USER"));
			if (!newAbilities.EnumerableNullOrEmpty<AbilityDef>())
			{
				text += "\n\n" + "LetterPsylinkLevelGained_PsycastLearned".Translate(pawn.Named("USER"), abilityLevel, (from a in newAbilities
				select a.LabelCap.Resolve()).ToLineList(null, false));
			}
			return text;
		}

		// Token: 0x06002E6C RID: 11884 RVA: 0x000EEDF8 File Offset: 0x000ECFF8
		public void TryGiveAbilityOfLevel(int abilityLevel, bool sendLetter = true)
		{
			string str = "LetterLabelPsylinkLevelGained".Translate() + ": " + this.pawn.LabelShortCap;
			string str2;
			if (!this.pawn.abilities.abilities.Any((Ability a) => a.def.IsPsycast && a.def.level == abilityLevel))
			{
				AbilityDef abilityDef = (from a in DefDatabase<AbilityDef>.AllDefs
				where a.IsPsycast && a.level == abilityLevel
				select a).RandomElement<AbilityDef>();
				this.pawn.abilities.GainAbility(abilityDef);
				str2 = Hediff_Psylink.MakeLetterTextNewPsylinkLevel(this.pawn, abilityLevel, Gen.YieldSingle<AbilityDef>(abilityDef));
			}
			else
			{
				str2 = Hediff_Psylink.MakeLetterTextNewPsylinkLevel(this.pawn, abilityLevel, null);
			}
			if (sendLetter && PawnUtility.ShouldSendNotificationAbout(this.pawn) && !this.pawn.IsDuplicate)
			{
				Find.LetterStack.ReceiveLetter(str, str2, LetterDefOf.PositiveEvent, this.pawn, null, null, null, null, 0, true);
			}
		}

		// Token: 0x06002E6D RID: 11885 RVA: 0x000EEF04 File Offset: 0x000ED104
		public override void CopyFrom(Hediff other)
		{
			base.CopyFrom(other);
			Hediff_Psylink hediff_Psylink = other as Hediff_Psylink;
			if (hediff_Psylink != null)
			{
				this.level = hediff_Psylink.level;
				this.suppressPostAddLetter = true;
			}
		}

		// Token: 0x06002E6E RID: 11886 RVA: 0x000EEF35 File Offset: 0x000ED135
		public override void PostRemoved()
		{
			base.PostRemoved();
			Pawn_NeedsTracker needs = this.pawn.needs;
			if (needs == null)
			{
				return;
			}
			needs.AddOrRemoveNeedsAsAppropriate();
		}

		// Token: 0x04002464 RID: 9316
		public bool suppressPostAddLetter;
	}
}
