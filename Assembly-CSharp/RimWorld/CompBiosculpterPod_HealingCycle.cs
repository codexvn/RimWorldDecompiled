using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C21 RID: 11297
	public abstract class CompBiosculpterPod_HealingCycle : CompBiosculpterPod_Cycle
	{
		// Token: 0x170027D8 RID: 10200
		// (get) Token: 0x0600FD51 RID: 64849 RVA: 0x00497BEF File Offset: 0x00495DEF
		public new CompProperties_BiosculpterPod_HealingCycle Props
		{
			get
			{
				return (CompProperties_BiosculpterPod_HealingCycle)this.props;
			}
		}

		// Token: 0x170027D9 RID: 10201
		// (get) Token: 0x0600FD52 RID: 64850
		public abstract bool Regenerate { get; }

		// Token: 0x0600FD53 RID: 64851 RVA: 0x00497BFC File Offset: 0x00495DFC
		public override string Description(Pawn tunedFor)
		{
			string text = base.Description(tunedFor);
			if (!this.Props.extraRequiredIngredients.NullOrEmpty<ThingDefCountClass>())
			{
				text += "\n\n" + "BiosculpterRequiresIngredients".Translate() + ":\n" + (from x in this.Props.extraRequiredIngredients
				select x.Summary).ToLineList("  - ", true);
			}
			string healingDescriptionForPawn = this.GetHealingDescriptionForPawn(tunedFor);
			if (!healingDescriptionForPawn.NullOrEmpty())
			{
				text = text + "\n\n" + healingDescriptionForPawn;
			}
			return text;
		}

		// Token: 0x0600FD54 RID: 64852 RVA: 0x00497CAC File Offset: 0x00495EAC
		public string GetHealingDescriptionForPawn(Pawn pawn)
		{
			string result;
			try
			{
				string text = "";
				if (pawn != null)
				{
					foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
					{
						if (this.WillHeal(pawn, hediff))
						{
							this.tmpWillHealHediffs.Add(CompBiosculpterPod_HealingCycle.<GetHealingDescriptionForPawn>g__HediffLabel|7_0(hediff));
						}
						else if (this.CanPotentiallyHeal(pawn, hediff) && !this.tmpCanHealHediffs.Contains(CompBiosculpterPod_HealingCycle.<GetHealingDescriptionForPawn>g__HediffLabel|7_0(hediff)))
						{
							this.tmpCanHealHediffs.Add(CompBiosculpterPod_HealingCycle.<GetHealingDescriptionForPawn>g__HediffLabel|7_0(hediff));
						}
					}
					if (this.tmpCanHealHediffs.Count == 1)
					{
						this.tmpWillHealHediffs.AddRange(this.tmpCanHealHediffs);
						this.tmpCanHealHediffs.Clear();
					}
					string text2 = string.Empty;
					if (this.tmpWillHealHediffs.Any<string>())
					{
						text2 += "HealingCycleWillHeal".Translate() + ":\n" + this.tmpWillHealHediffs.ToLineList("  - ", true);
					}
					if (this.tmpCanHealHediffs.Any<string>())
					{
						if (!text2.NullOrEmpty())
						{
							text2 += "\n\n";
						}
						text2 += "HealingCycleOneWillHeal".Translate() + ":\n" + this.tmpCanHealHediffs.ToLineList("  - ", true);
					}
					if (!text2.NullOrEmpty())
					{
						text += text2;
					}
				}
				result = text;
			}
			finally
			{
				this.tmpWillHealHediffs.Clear();
				this.tmpCanHealHediffs.Clear();
			}
			return result;
		}

		// Token: 0x0600FD55 RID: 64853 RVA: 0x00497E78 File Offset: 0x00496078
		public override void CycleCompleted(Pawn pawn)
		{
			if (pawn.health == null)
			{
				return;
			}
			this.tmpHediffs.Clear();
			this.tmpHediffs.AddRange(pawn.health.hediffSet.hediffs);
			try
			{
				foreach (Hediff hediff in this.tmpHediffs)
				{
					if (this.WillHeal(pawn, hediff))
					{
						HealthUtility.Cure(hediff);
					}
					else
					{
						Hediff_MissingPart hediff_MissingPart = hediff as Hediff_MissingPart;
						if (hediff_MissingPart != null && hediff_MissingPart.IsFresh)
						{
							hediff_MissingPart.IsFresh = false;
							pawn.health.Notify_HediffChanged(hediff_MissingPart);
						}
					}
				}
				this.tmpHediffs.Clear();
				foreach (Hediff hediff2 in pawn.health.hediffSet.hediffs)
				{
					if (this.CanPotentiallyHeal(pawn, hediff2))
					{
						this.tmpHediffs.Add(hediff2);
					}
				}
				Hediff hediff3;
				if (this.tmpHediffs.TryRandomElement(out hediff3))
				{
					HealthUtility.Cure(hediff3);
					Messages.Message("BiosculpterHealCompletedWithCureMessage".Translate(pawn.Named("PAWN"), hediff3.Named("HEDIFF")), pawn, MessageTypeDefOf.PositiveEvent, true);
				}
				else
				{
					Messages.Message("BiosculpterHealCompletedMessage".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.PositiveEvent, true);
				}
			}
			finally
			{
				this.tmpHediffs.Clear();
			}
		}

		// Token: 0x0600FD56 RID: 64854 RVA: 0x0049804C File Offset: 0x0049624C
		private bool WillHeal(Pawn pawn, Hediff hediff)
		{
			if (!hediff.def.everCurableByItem)
			{
				return false;
			}
			if (hediff.def.chronic)
			{
				return false;
			}
			if (hediff.def.countsAsAddedPartOrImplant)
			{
				return false;
			}
			if (hediff.def == HediffDefOf.BloodLoss)
			{
				return true;
			}
			if (this.Regenerate && hediff.Part != null && this.Props.bodyPartsToRestore != null && this.Props.bodyPartsToRestore.Contains(hediff.Part.def) && !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(hediff.Part))
			{
				return hediff.Part.def != BodyPartDefOf.Eye || pawn.Ideo == null || !pawn.Ideo.IdeoApprovesOfBlindness();
			}
			return hediff is Hediff_Injury && !hediff.IsPermanent();
		}

		// Token: 0x0600FD57 RID: 64855 RVA: 0x00498128 File Offset: 0x00496328
		private bool CanPotentiallyHeal(Pawn pawn, Hediff hediff)
		{
			return hediff.def.everCurableByItem && !hediff.def.countsAsAddedPartOrImplant && ((this.Props.conditionsToPossiblyCure != null && this.Props.conditionsToPossiblyCure.Contains(hediff.def)) || (this.Regenerate && hediff is Hediff_Injury && hediff.IsPermanent()));
		}

		// Token: 0x0600FD59 RID: 64857 RVA: 0x004981BE File Offset: 0x004963BE
		[CompilerGenerated]
		internal static string <GetHealingDescriptionForPawn>g__HediffLabel|7_0(Hediff hediff)
		{
			if (hediff.Part != null && !hediff.def.cureAllAtOnceIfCuredByItem)
			{
				return hediff.Part.Label + " (" + hediff.Label + ")";
			}
			return hediff.Label;
		}

		// Token: 0x0400A60E RID: 42510
		private List<string> tmpWillHealHediffs = new List<string>();

		// Token: 0x0400A60F RID: 42511
		private List<string> tmpCanHealHediffs = new List<string>();

		// Token: 0x0400A610 RID: 42512
		private List<Hediff> tmpHediffs = new List<Hediff>();
	}
}
