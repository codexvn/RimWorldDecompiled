using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse.AI;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020001A5 RID: 421
	public class Book : ThingWithComps
	{
		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000BBB RID: 3003 RVA: 0x00038078 File Offset: 0x00036278
		public CompBook BookComp
		{
			get
			{
				CompBook result;
				if ((result = this.cachedComp) == null)
				{
					result = (this.cachedComp = base.GetComp<CompBook>());
				}
				return result;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000BBC RID: 3004 RVA: 0x000380A0 File Offset: 0x000362A0
		private Graphic OpenGraphic
		{
			get
			{
				Graphic result;
				if ((result = this.openGraphic) == null)
				{
					result = (this.openGraphic = this.BookComp.Props.openGraphic.Graphic);
				}
				return result;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000BBD RID: 3005 RVA: 0x000380D8 File Offset: 0x000362D8
		public Graphic VerticalGraphic
		{
			get
			{
				Graphic result;
				if ((result = this.verticalGraphic) == null)
				{
					result = (this.verticalGraphic = this.BookComp.Props.verticalGraphic.Graphic);
				}
				return result;
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000BBE RID: 3006 RVA: 0x0003810D File Offset: 0x0003630D
		public override string LabelNoCount
		{
			get
			{
				return this.title + GenLabel.LabelExtras(this, true, true);
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000BBF RID: 3007 RVA: 0x00038122 File Offset: 0x00036322
		public override string LabelNoParenthesis
		{
			get
			{
				return this.title;
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000BC0 RID: 3008 RVA: 0x0003812A File Offset: 0x0003632A
		public string FlavorUI
		{
			get
			{
				return this.descriptionFlavor;
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000BC1 RID: 3009 RVA: 0x00038132 File Offset: 0x00036332
		public float MentalBreakChancePerHour
		{
			get
			{
				return this.mentalBreakChancePerHour;
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000BC2 RID: 3010 RVA: 0x0003813A File Offset: 0x0003633A
		public float JoyFactor
		{
			get
			{
				return this.joyFactor;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000BC3 RID: 3011 RVA: 0x00038122 File Offset: 0x00036322
		public string Title
		{
			get
			{
				return this.title;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000BC4 RID: 3012 RVA: 0x00038142 File Offset: 0x00036342
		// (set) Token: 0x06000BC5 RID: 3013 RVA: 0x0003814A File Offset: 0x0003634A
		public bool IsOpen
		{
			get
			{
				return this.isOpen;
			}
			set
			{
				this.isOpen = value;
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000BC6 RID: 3014 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool IsReadable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000BC7 RID: 3015 RVA: 0x00038153 File Offset: 0x00036353
		public override string DescriptionFlavor
		{
			get
			{
				return this.DescriptionDetailed;
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000BC8 RID: 3016 RVA: 0x0003815B File Offset: 0x0003635B
		public override string DescriptionDetailed
		{
			get
			{
				this.EnsureDescriptionUpToDate();
				return this.description;
			}
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x0003816C File Offset: 0x0003636C
		public override void PostPostMake()
		{
			base.PostPostMake();
			if (!this.HasComp<CompQuality>())
			{
				this.GenerateBook(null, null);
			}
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x00038197 File Offset: 0x00036397
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.Books, OpportunityType.GoodToKnow);
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CanStackWith(Thing other)
		{
			return false;
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x000381AC File Offset: 0x000363AC
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			if (this.isOpen)
			{
				Pawn_CarryTracker pawn_CarryTracker = base.ParentHolder as Pawn_CarryTracker;
				Rot4 rotation;
				if (pawn_CarryTracker != null)
				{
					rotation = pawn_CarryTracker.pawn.Rotation;
				}
				else
				{
					rotation = base.Rotation;
				}
				this.OpenGraphic.Draw(drawLoc, flip ? rotation.Opposite : rotation, this, 0f);
				return;
			}
			base.DrawAt(drawLoc, flip);
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x00038210 File Offset: 0x00036410
		public void PawnReadNow(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Reading, this);
			pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x00038244 File Offset: 0x00036444
		public override void PostQualitySet()
		{
			base.PostQualitySet();
			this.GenerateBook(null, null);
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x00038268 File Offset: 0x00036468
		public void OnBookReadTick(Pawn pawn, int delta, float roomBonusFactor)
		{
			float factor = pawn.GetStatValue(StatDefOf.ReadingSpeed, true, -1) * roomBonusFactor * (float)delta;
			foreach (BookOutcomeDoer bookOutcomeDoer in this.BookComp.Doers)
			{
				bookOutcomeDoer.OnReadingTick(pawn, factor);
			}
			MentalBreakDef breakDef;
			if (ModsConfig.AnomalyActive && this.MentalBreakChancePerHour > 0f && Rand.MTBEventOccurs(1f / this.MentalBreakChancePerHour, 2500f, (float)delta) && pawn.mindState.mentalBreaker.TryGetRandomMentalBreak(this.BookComp.Props.mentalBreakIntensity, out breakDef))
			{
				TaggedString taggedString = "BookMentalBreakMessage".Translate(this.Label);
				pawn.mindState.mentalBreaker.TryDoMentalBreak(taggedString, breakDef);
			}
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x0003834C File Offset: 0x0003654C
		public bool ProvidesOutcome(Pawn reader)
		{
			using (IEnumerator<BookOutcomeDoer> enumerator = this.BookComp.Doers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.DoesProvidesOutcome(reader))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x000383A8 File Offset: 0x000365A8
		public void SetMentalBreakChance(float chance)
		{
			this.mentalBreakChancePerHour = Mathf.Clamp01(chance);
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x000383B6 File Offset: 0x000365B6
		public void SetJoyFactor(float factor)
		{
			this.joyFactor = factor;
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x000383BF File Offset: 0x000365BF
		public virtual IEnumerable<Dialog_InfoCard.Hyperlink> GetHyperlinks()
		{
			foreach (BookOutcomeDoer bookOutcomeDoer in this.BookComp.Doers)
			{
				foreach (Dialog_InfoCard.Hyperlink hyperlink in bookOutcomeDoer.GetHyperlinks())
				{
					yield return hyperlink;
				}
				IEnumerator<Dialog_InfoCard.Hyperlink> enumerator2 = null;
			}
			IEnumerator<BookOutcomeDoer> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x000383CF File Offset: 0x000365CF
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			FloatMenuOption floatMenuOption = new FloatMenuOption("AssignReadNow".Translate(this.Label), delegate()
			{
				this.PawnReadNow(selPawn);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			string arg;
			if (!BookUtility.CanReadBook(this, selPawn, out arg))
			{
				floatMenuOption.Label = string.Format("{0}: {1}", "AssignCannotReadNow".Translate(this.Label), arg);
				floatMenuOption.Disabled = true;
			}
			Pawn pawn = selPawn.Map.reservationManager.FirstRespectedReserver(this, selPawn, null) ?? selPawn.Map.physicalInteractionReservationManager.FirstReserverOf(this);
			if (pawn != null)
			{
				FloatMenuOption floatMenuOption2 = floatMenuOption;
				floatMenuOption2.Label += " (" + "ReservedBy".Translate(pawn.LabelShort, pawn) + ")";
			}
			floatMenuOption.iconThing = this;
			yield return floatMenuOption;
			foreach (FloatMenuOption floatMenuOption3 in base.GetFloatMenuOptions(selPawn))
			{
				floatMenuOption3.iconThing = this;
				yield return floatMenuOption3;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x000383E6 File Offset: 0x000365E6
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats())
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Stat_RecreationType_Desc".Translate());
			stringBuilder.AppendLine();
			foreach (JoyKindDef joyKindDef in DefDatabase<JoyKindDef>.AllDefs)
			{
				stringBuilder.AppendLine("  - " + joyKindDef.LabelCap);
			}
			JoyKindDef reading = JoyKindDefOf.Reading;
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "StatsReport_JoyKind".Translate(), reading.LabelCap, stringBuilder.ToString(), 4750, reading.LabelCap, null, false, false);
			yield break;
			yield break;
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x000383F8 File Offset: 0x000365F8
		public virtual void GenerateBook(Pawn author = null, long? fixedDate = null)
		{
			this.mentalBreakChancePerHour = 0f;
			Book.subjects.Clear();
			GrammarRequest grammarRequest = default(GrammarRequest);
			long absTicks = fixedDate ?? ((long)GenTicks.TicksAbs - (long)(this.BookComp.Props.ageYearsRange.RandomInRange * 3600000f));
			grammarRequest.Rules.Add(new Rule_String("date", GenDate.DateFullStringAt(absTicks, Vector2.zero)));
			grammarRequest.Rules.Add(new Rule_String("date_season", GenDate.DateMonthYearStringAt(absTicks, Vector2.zero)));
			if (this.HasComp<CompQuality>())
			{
				grammarRequest.Constants.Add("quality", ((int)base.GetComp<CompQuality>().Quality).ToString());
			}
			foreach (Rule item in ((author == null) ? TaleData_Pawn.GenerateRandom(true) : TaleData_Pawn.GenerateFrom(author)).GetRules("ANYPAWN", grammarRequest.Constants))
			{
				grammarRequest.Rules.Add(item);
			}
			this.AppendDoerRules(author, ref grammarRequest);
			Book.AppendRulesForSubject(Book.subjects, grammarRequest.Rules, grammarRequest.Constants, "primary", 0);
			Book.AppendRulesForSubject(Book.subjects, grammarRequest.Rules, grammarRequest.Constants, "secondary", 1);
			Book.AppendRulesForSubject(Book.subjects, grammarRequest.Rules, grammarRequest.Constants, "tertiary", 2);
			GrammarRequest request = grammarRequest;
			request.Includes.Add(this.BookComp.Props.nameMaker);
			this.title = GenText.CapitalizeAsTitle(GrammarResolver.Resolve("title", request, null, false, null, null, null, true)).StripTags();
			GrammarRequest request2 = grammarRequest;
			request2.Includes.Add(this.BookComp.Props.descriptionMaker);
			request2.Includes.Add(RulePackDefOf.TalelessImages);
			request2.Includes.Add(RulePackDefOf.ArtDescriptionRoot_Taleless);
			request2.Includes.Add(RulePackDefOf.ArtDescriptionUtility_Global);
			this.descriptionFlavor = GrammarResolver.Resolve("desc", request2, null, false, null, null, null, true).StripTags();
			this.description = this.GenerateFullDescription();
			Book.subjects.Clear();
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x00038658 File Offset: 0x00036858
		private void AppendDoerRules(Pawn author, ref GrammarRequest common)
		{
			foreach (BookOutcomeDoer bookOutcomeDoer in this.BookComp.Doers)
			{
				bookOutcomeDoer.Reset();
				bookOutcomeDoer.OnBookGenerated(author);
				IEnumerable<RulePack> topicRulePacks = bookOutcomeDoer.GetTopicRulePacks();
				if (topicRulePacks != null)
				{
					foreach (RulePack rulePack in topicRulePacks)
					{
						common.IncludesBare.Add(rulePack);
						List<ValueTuple<string, string>> list = new List<ValueTuple<string, string>>();
						foreach (Rule rule in rulePack.Rules)
						{
							if (rule.keyword.StartsWith("subject_"))
							{
								list.Add(new ValueTuple<string, string>(rule.keyword.Substring("subject_".Length), GrammarResolver.Resolve(rule.keyword, common, null, false, null, null, null, false)));
							}
						}
						Book.subjects.Add(new Book.BookSubjectSymbol
						{
							keyword = GrammarResolver.Resolve("subject", common, null, false, null, null, null, false),
							subSymbols = list
						});
					}
				}
				IEnumerable<Rule_String> topicRuleStrings = bookOutcomeDoer.GetTopicRuleStrings();
				if (topicRuleStrings != null)
				{
					foreach (Rule_String item in topicRuleStrings)
					{
						common.Rules.Add(item);
					}
				}
			}
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x00038858 File Offset: 0x00036A58
		private void EnsureDescriptionUpToDate()
		{
			if (this.descCanBeInvalidated)
			{
				this.description = this.GenerateFullDescription();
			}
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x00038870 File Offset: 0x00036A70
		private string GenerateFullDescription()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.descCanBeInvalidated = false;
			stringBuilder.AppendLine(this.title.Colorize(ColoredText.TipSectionTitleColor) + GenLabel.LabelExtras(this, false, true) + "\n");
			stringBuilder.AppendLine(this.descriptionFlavor + "\n");
			if (this.MentalBreakChancePerHour > 0f)
			{
				stringBuilder.AppendLine(string.Format(" - {0}: {1}", "BookMentalBreak".Translate(), "PerHour".Translate(this.MentalBreakChancePerHour.ToStringPercent("0.0"))));
			}
			foreach (BookOutcomeDoer bookOutcomeDoer in this.BookComp.Doers)
			{
				string benefitsString = bookOutcomeDoer.GetBenefitsString(null);
				if (!string.IsNullOrEmpty(benefitsString))
				{
					if (bookOutcomeDoer.BenefitDetailsCanChange(null))
					{
						this.descCanBeInvalidated = true;
					}
					stringBuilder.AppendLine(benefitsString);
				}
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x0003898C File Offset: 0x00036B8C
		private static void AppendRulesForSubject(List<Book.BookSubjectSymbol> subjects, List<Rule> rules, Dictionary<string, string> constants, string postfix, int i)
		{
			if (i < subjects.Count)
			{
				rules.Add(new Rule_String("subject_" + postfix, subjects[i].keyword));
				constants.Add("length_subject_" + postfix, subjects[i].keyword.Length.ToString());
				constants.Add("has_subject_" + postfix, "true");
				using (List<ValueTuple<string, string>>.Enumerator enumerator = subjects[i].subSymbols.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ValueTuple<string, string> valueTuple = enumerator.Current;
						rules.Add(new Rule_String("subject_" + postfix + "_" + valueTuple.Item1, valueTuple.Item2));
					}
					return;
				}
			}
			constants.Add("has_subject_" + postfix, "false");
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x00038A90 File Offset: 0x00036C90
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Values.Look<string>(ref this.descriptionFlavor, "descriptionFlavor", null, false);
			Scribe_Values.Look<float>(ref this.mentalBreakChancePerHour, "mentalBreakChancePerHour", 0f, false);
			Scribe_Values.Look<float>(ref this.joyFactor, "joyFactor", 0f, false);
			Scribe_Values.Look<bool>(ref this.isOpen, "isOpen", false, false);
			Scribe_Values.Look<bool>(ref this.descCanBeInvalidated, "descCanBeInvalidated", false, false);
			Scribe_Values.Look<string>(ref this.description, "description", null, false);
		}

		// Token: 0x040006CD RID: 1741
		private string title;

		// Token: 0x040006CE RID: 1742
		private bool isOpen;

		// Token: 0x040006CF RID: 1743
		private bool descCanBeInvalidated;

		// Token: 0x040006D0 RID: 1744
		private float mentalBreakChancePerHour;

		// Token: 0x040006D1 RID: 1745
		private float joyFactor = 1f;

		// Token: 0x040006D2 RID: 1746
		private string descriptionFlavor;

		// Token: 0x040006D3 RID: 1747
		private string description;

		// Token: 0x040006D4 RID: 1748
		private Graphic openGraphic;

		// Token: 0x040006D5 RID: 1749
		private Graphic verticalGraphic;

		// Token: 0x040006D6 RID: 1750
		private CompBook cachedComp;

		// Token: 0x040006D7 RID: 1751
		private static List<Book.BookSubjectSymbol> subjects = new List<Book.BookSubjectSymbol>();

		// Token: 0x020001A6 RID: 422
		private struct BookSubjectSymbol
		{
			// Token: 0x040006D8 RID: 1752
			public string keyword;

			// Token: 0x040006D9 RID: 1753
			public List<ValueTuple<string, string>> subSymbols;
		}
	}
}
