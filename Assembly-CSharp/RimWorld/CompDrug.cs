using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C68 RID: 11368
	public class CompDrug : ThingComp
	{
		// Token: 0x1700282A RID: 10282
		// (get) Token: 0x0600FEE0 RID: 65248 RVA: 0x0049DEB7 File Offset: 0x0049C0B7
		public CompProperties_Drug Props
		{
			get
			{
				return (CompProperties_Drug)this.props;
			}
		}

		// Token: 0x0600FEE1 RID: 65249 RVA: 0x0049DEC4 File Offset: 0x0049C0C4
		public override void PrePostIngested(Pawn ingester)
		{
			if (this.Props.Addictive && ingester.RaceProps.IsFlesh)
			{
				HediffDef addictionHediff = this.Props.chemical.addictionHediff;
				Hediff_Addiction hediff_Addiction = AddictionUtility.FindAddictionHediff(ingester, this.Props.chemical);
				Hediff hediff = AddictionUtility.FindToleranceHediff(ingester, this.Props.chemical);
				float num = (hediff != null) ? hediff.Severity : 0f;
				if (hediff_Addiction != null)
				{
					hediff_Addiction.Severity += this.Props.existingAddictionSeverityOffset;
				}
				else
				{
					float num2 = DrugStatsUtility.GetAddictivenessAtTolerance(this.parent.def, num);
					if (ingester.genes != null)
					{
						num2 *= ingester.genes.AddictionChanceFactor(this.Props.chemical);
					}
					if (Rand.Value < num2 && num >= this.Props.minToleranceToAddict)
					{
						ingester.health.AddHediff(addictionHediff, null, null, null);
						if (PawnUtility.ShouldSendNotificationAbout(ingester))
						{
							Find.LetterStack.ReceiveLetter("LetterLabelNewlyAddicted".Translate(this.Props.chemical.label).CapitalizeFirst(), "LetterNewlyAddicted".Translate(ingester.LabelShort, this.Props.chemical.label, ingester.Named("PAWN")).AdjustedFor(ingester, "PAWN", true).CapitalizeFirst(), LetterDefOf.NegativeEvent, ingester, null, null, null, null, 0, true);
						}
						AddictionUtility.CheckDrugAddictionTeachOpportunity(ingester);
					}
				}
				Need need;
				if (addictionHediff.chemicalNeed != null && ingester.needs.TryGetNeed(addictionHediff.chemicalNeed, out need))
				{
					float needLevelOffset = this.Props.needLevelOffset;
					AddictionUtility.ModifyChemicalEffectForToleranceAndBodySize(ingester, this.Props.chemical, ref needLevelOffset, false, true);
					need.CurLevel += needLevelOffset;
				}
			}
		}

		// Token: 0x0600FEE2 RID: 65250 RVA: 0x0049E0B4 File Offset: 0x0049C2B4
		public override void PostIngested(Pawn ingester)
		{
			if (this.Props.Addictive && ingester.RaceProps.IsFlesh)
			{
				float num = 1f;
				if (ModsConfig.BiotechActive && ingester.genes != null)
				{
					foreach (Gene gene in ingester.genes.GenesListForReading)
					{
						if (gene.Active && gene.def.chemical == this.Props.chemical)
						{
							num *= gene.def.overdoseChanceFactor;
						}
					}
				}
				if (Rand.Chance(num))
				{
					Hediff firstHediffOfDef = ingester.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DrugOverdose, false);
					float num2 = (firstHediffOfDef != null) ? firstHediffOfDef.Severity : 0f;
					bool flag = false;
					if (ModsConfig.BiotechActive && ingester.genes != null)
					{
						foreach (Gene gene2 in ingester.genes.GenesListForReading)
						{
							Gene_ChemicalDependency gene_ChemicalDependency = gene2 as Gene_ChemicalDependency;
							if (gene_ChemicalDependency != null && gene_ChemicalDependency.def.chemical == this.Props.chemical)
							{
								flag = true;
								break;
							}
						}
					}
					if (num2 < 0.9f && !flag && Rand.Value < this.Props.largeOverdoseChance)
					{
						float num3 = Rand.Range(0.85f, 0.99f);
						HealthUtility.AdjustSeverity(ingester, HediffDefOf.DrugOverdose, num3 - num2);
						if (ingester.Faction == Faction.OfPlayer)
						{
							Messages.Message("MessageAccidentalOverdose".Translate(ingester.Named("INGESTER"), this.parent.LabelNoCount, this.parent.Named("DRUG")), ingester, MessageTypeDefOf.NegativeHealthEvent, true);
						}
					}
					else
					{
						float num4 = this.Props.overdoseSeverityOffset.RandomInRange / ingester.BodySize;
						if (num4 > 0f)
						{
							HealthUtility.AdjustSeverity(ingester, HediffDefOf.DrugOverdose, num4);
						}
					}
				}
			}
			if (this.Props.isCombatEnhancingDrug && !ingester.Dead)
			{
				ingester.mindState.lastTakeCombatEnhancingDrugTick = Find.TickManager.TicksGame;
			}
			if (this.parent.def.ingestible.drugCategory != DrugCategory.Medical && !ingester.Dead)
			{
				ingester.mindState.lastTakeRecreationalDrugTick = Find.TickManager.TicksGame;
			}
			Pawn_DrugPolicyTracker drugs = ingester.drugs;
			if (drugs != null)
			{
				drugs.Notify_DrugIngested(this.parent);
			}
			Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.IngestedDrug, ingester.Named(HistoryEventArgsNames.Doer)), true);
			if (this.parent.def.ingestible.drugCategory == DrugCategory.Hard)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.IngestedHardDrug, ingester.Named(HistoryEventArgsNames.Doer)), true);
			}
			if (this.parent.def.IsNonMedicalDrug)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.IngestedRecreationalDrug, ingester.Named(HistoryEventArgsNames.Doer)), true);
			}
		}
	}
}
