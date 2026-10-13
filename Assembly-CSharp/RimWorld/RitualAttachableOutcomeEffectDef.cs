using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002686 RID: 9862
	public class RitualAttachableOutcomeEffectDef : Def
	{
		// Token: 0x17002145 RID: 8517
		// (get) Token: 0x0600D8B5 RID: 55477 RVA: 0x00403985 File Offset: 0x00401B85
		public RitualAttachableOutcomeEffectWorker Worker
		{
			get
			{
				if (this.workerInstance == null)
				{
					this.workerInstance = (RitualAttachableOutcomeEffectWorker)Activator.CreateInstance(this.workerClass);
					this.workerInstance.def = this;
				}
				return this.workerInstance;
			}
		}

		// Token: 0x17002146 RID: 8518
		// (get) Token: 0x0600D8B6 RID: 55478 RVA: 0x004039B7 File Offset: 0x00401BB7
		public bool AppliesToAllOutcomes
		{
			get
			{
				return !this.onlyBestOutcome && !this.onlyPositiveOutcomes && !this.onlyNegativeOutcomes && !this.onlyWorstOutcome;
			}
		}

		// Token: 0x0600D8B7 RID: 55479 RVA: 0x004039DC File Offset: 0x00401BDC
		public bool AppliesToSeveralOutcomes(Precept_Ritual ritual)
		{
			bool flag = false;
			foreach (RitualOutcomePossibility outcomeChance in ritual.outcomeEffect.def.outcomeChances)
			{
				if (this.AppliesToOutcome(ritual.outcomeEffect.def, outcomeChance))
				{
					if (flag)
					{
						return true;
					}
					flag = true;
				}
			}
			return false;
		}

		// Token: 0x17002147 RID: 8519
		// (get) Token: 0x0600D8B8 RID: 55480 RVA: 0x00403A54 File Offset: 0x00401C54
		private string RequiredMemeList
		{
			get
			{
				if (this.requiredMemeList == null && !this.requiredMemeAny.NullOrEmpty<MemeDef>())
				{
					this.requiredMemeList = (from m in this.requiredMemeAny
					select m.label.ResolveTags()).ToCommaList(false, false);
				}
				return this.requiredMemeList;
			}
		}

		// Token: 0x0600D8B9 RID: 55481 RVA: 0x00403AB4 File Offset: 0x00401CB4
		public AcceptanceReport CanAttachToRitual(Precept_Ritual ritual)
		{
			if (!this.requiredMemeAny.NullOrEmpty<MemeDef>() && !ritual.ideo.memes.SharesElementWith(this.requiredMemeAny))
			{
				return "RitualAttachedRewardRequiredMemeAny".Translate() + ": " + this.RequiredMemeList.CapitalizeFirst();
			}
			if ((!this.allowedRituals.NullOrEmpty<RitualPatternDef>() && !this.allowedRituals.Contains(ritual.sourcePattern)) || (!this.disallowedRituals.NullOrEmpty<RitualPatternDef>() && this.disallowedRituals.Contains(ritual.sourcePattern)))
			{
				return "RitualAttachedRewardCantUseWithRitual".Translate(this.LabelCap, ritual.LabelCap);
			}
			if (this.requiredFaction != null && Find.FactionManager.FirstFactionOfDef(this.requiredFaction) == null)
			{
				return "RitualAttachedRewardRequiredFaction".Translate(this.requiredFaction.LabelCap);
			}
			return true;
		}

		// Token: 0x0600D8BA RID: 55482 RVA: 0x00403BB8 File Offset: 0x00401DB8
		public bool AppliesToOutcome(RitualOutcomeEffectDef effectDef, RitualOutcomePossibility outcomeChance)
		{
			return ModLister.CheckIdeology("Attachable ritual reward") && (!this.onlyPositiveOutcomes || outcomeChance.Positive) && (!this.onlyNegativeOutcomes || !outcomeChance.Positive) && (!this.onlyBestOutcome || outcomeChance == effectDef.BestOutcome) && (!this.onlyWorstOutcome || outcomeChance == effectDef.WorstOutcome);
		}

		// Token: 0x0600D8BB RID: 55483 RVA: 0x00403C20 File Offset: 0x00401E20
		public string DescriptionForRitualValidated(Precept_Ritual ritual)
		{
			if (!this.AppliesToAllOutcomes)
			{
				return "RitualAttachedOutcomesInfo".Translate(ritual.shortDescOverride ?? ritual.def.label, this.AppliesToOutcomesListString(ritual.outcomeEffect.def)) + ", " + this.effectDesc;
			}
			return this.description.CapitalizeFirst();
		}

		// Token: 0x0600D8BC RID: 55484 RVA: 0x00403C98 File Offset: 0x00401E98
		public string DescriptionForRitualValidated(Precept_Ritual ritual, Map map)
		{
			string text = this.DescriptionForRitualValidated(ritual);
			AcceptanceReport report = this.Worker.CanApplyNow(ritual, map);
			if (!report)
			{
				text += " (" + "RitualAttachedOutcomeCantApply".Translate() + ": " + report.Reason + ")";
				text = text.Colorize(ColorLibrary.RedReadable);
			}
			return text;
		}

		// Token: 0x0600D8BD RID: 55485 RVA: 0x00403D10 File Offset: 0x00401F10
		public string TooltipForRitual(Precept_Ritual ritual)
		{
			string text = this.DescriptionForRitualValidated(ritual).ResolveTags();
			if (!this.requiredMemeAny.NullOrEmpty<MemeDef>())
			{
				text = text + "\n\n" + ("UnlockedByMeme".Translate().Resolve() + ":\n").Colorize(ColoredText.TipSectionTitleColor);
				foreach (MemeDef memeDef in this.requiredMemeAny)
				{
					text = text + "  - " + memeDef.LabelCap.Resolve() + "\n";
				}
			}
			return text;
		}

		// Token: 0x0600D8BE RID: 55486 RVA: 0x00403DCC File Offset: 0x00401FCC
		public string AppliesToOutcomesListString(RitualOutcomeEffectDef effectDef)
		{
			this.appliesToOutcomeListTmp.Clear();
			foreach (RitualOutcomePossibility ritualOutcomePossibility in effectDef.outcomeChances)
			{
				if (this.AppliesToOutcome(effectDef, ritualOutcomePossibility))
				{
					this.appliesToOutcomeListTmp.Add(ritualOutcomePossibility.label.ToLower());
				}
			}
			return this.appliesToOutcomeListTmp.ToCommaListOr(false);
		}

		// Token: 0x0600D8BF RID: 55487 RVA: 0x00403E50 File Offset: 0x00402050
		public string AppliesToOutcomesString(RitualOutcomeEffectDef effectDef)
		{
			if (this.onlyPositiveOutcomes)
			{
				return "RitualAttachedOutcomes_Positive".Translate();
			}
			if (this.onlyNegativeOutcomes)
			{
				return "RitualAttachedOutcomes_Negative".Translate();
			}
			if (this.onlyBestOutcome)
			{
				return effectDef.outcomeChances.MaxBy((RitualOutcomePossibility o) => o.positivityIndex).label;
			}
			if (this.onlyWorstOutcome)
			{
				return effectDef.outcomeChances.MinBy((RitualOutcomePossibility o) => o.positivityIndex).label;
			}
			throw new NotImplementedException();
		}

		// Token: 0x0600D8C0 RID: 55488 RVA: 0x00403F02 File Offset: 0x00402102
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.workerClass == null || this.workerClass.IsAbstract || this.workerClass.IsInterface)
			{
				string str = "worker class must be instantiable: ";
				Type type = this.workerClass;
				yield return str + ((type != null) ? type.ToString() : null);
			}
			if ((this.onlyPositiveOutcomes && this.onlyNegativeOutcomes) || (this.onlyPositiveOutcomes && this.onlyWorstOutcome) || (this.onlyNegativeOutcomes && this.onlyBestOutcome))
			{
				yield return "conflicting outcome positivity configuration";
			}
			yield break;
		}

		// Token: 0x040092E7 RID: 37607
		public Type workerClass = typeof(RitualAttachableOutcomeEffectWorker);

		// Token: 0x040092E8 RID: 37608
		public List<MemeDef> requiredMemeAny;

		// Token: 0x040092E9 RID: 37609
		public List<RitualPatternDef> allowedRituals;

		// Token: 0x040092EA RID: 37610
		public List<RitualPatternDef> disallowedRituals;

		// Token: 0x040092EB RID: 37611
		public FactionDef requiredFaction;

		// Token: 0x040092EC RID: 37612
		public bool onlyPositiveOutcomes = true;

		// Token: 0x040092ED RID: 37613
		public bool onlyBestOutcome;

		// Token: 0x040092EE RID: 37614
		public bool onlyNegativeOutcomes;

		// Token: 0x040092EF RID: 37615
		public bool onlyWorstOutcome;

		// Token: 0x040092F0 RID: 37616
		[MustTranslate]
		public string letterInfoText;

		// Token: 0x040092F1 RID: 37617
		[MustTranslate]
		public string effectDesc;

		// Token: 0x040092F2 RID: 37618
		private RitualAttachableOutcomeEffectWorker workerInstance;

		// Token: 0x040092F3 RID: 37619
		private string requiredMemeList;

		// Token: 0x040092F4 RID: 37620
		private List<string> appliesToOutcomeListTmp = new List<string>();
	}
}
