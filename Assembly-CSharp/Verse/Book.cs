using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse.AI;
using Verse.Grammar;

namespace Verse;

public class Book : ThingWithComps
{
	private struct BookSubjectSymbol
	{
		public string keyword;

		public List<(string, string)> subSymbols;
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass50_0
	{
		public Book _003C_003E4__this;

		public Pawn selPawn;

		internal void _003CGetFloatMenuOptions_003Eb__0()
		{
			_003C_003E4__this.PawnReadNow(selPawn);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetFloatMenuOptions_003Ed__50 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Book _003C_003E4__this;

		private Pawn selPawn;

		public Pawn _003C_003E3__selPawn;

		private _003C_003Ec__DisplayClass50_0 _003C_003E8__1;

		private IEnumerator<FloatMenuOption> _003C_003E7__wrap1;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CGetFloatMenuOptions_003Ed__50(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 2)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Book book = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass50_0();
					_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
					_003C_003E8__1.selPawn = selPawn;
					FloatMenuOption floatMenuOption = new FloatMenuOption("AssignReadNow".Translate(book.Label), delegate
					{
						_003C_003E8__1._003C_003E4__this.PawnReadNow(_003C_003E8__1.selPawn);
					});
					if (!BookUtility.CanReadBook(book, _003C_003E8__1.selPawn, out var reason))
					{
						floatMenuOption.Label = $"{"AssignCannotReadNow".Translate(book.Label)}: {reason}";
						floatMenuOption.Disabled = true;
					}
					Pawn pawn = _003C_003E8__1.selPawn.Map.reservationManager.FirstRespectedReserver(book, _003C_003E8__1.selPawn) ?? _003C_003E8__1.selPawn.Map.physicalInteractionReservationManager.FirstReserverOf(book);
					if (pawn != null)
					{
						floatMenuOption.Label += " (" + "ReservedBy".Translate(pawn.LabelShort, pawn) + ")";
					}
					floatMenuOption.iconThing = book;
					_003C_003E2__current = floatMenuOption;
					_003C_003E1__state = 1;
					return true;
				}
				case 1:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingWithComps)book).GetFloatMenuOptions(_003C_003E8__1.selPawn).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 2:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					FloatMenuOption current = _003C_003E7__wrap1.Current;
					current.iconThing = book;
					_003C_003E2__current = current;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetFloatMenuOptions_003Ed__50 _003CGetFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetFloatMenuOptions_003Ed__ = new _003CGetFloatMenuOptions_003Ed__50(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetFloatMenuOptions_003Ed__.selPawn = _003C_003E3__selPawn;
			return _003CGetFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetHyperlinks_003Ed__49 : IEnumerable<Dialog_InfoCard.Hyperlink>, IEnumerable, IEnumerator<Dialog_InfoCard.Hyperlink>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Dialog_InfoCard.Hyperlink _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Book _003C_003E4__this;

		private IEnumerator<BookOutcomeDoer> _003C_003E7__wrap1;

		private IEnumerator<Dialog_InfoCard.Hyperlink> _003C_003E7__wrap2;

		Dialog_InfoCard.Hyperlink IEnumerator<Dialog_InfoCard.Hyperlink>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CGetHyperlinks_003Ed__49(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || num == 1)
			{
				try
				{
					if (num == -4 || num == 1)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Book book = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -4;
					goto IL_0092;
				}
				_003C_003E1__state = -1;
				_003C_003E7__wrap1 = book.BookComp.Doers.GetEnumerator();
				_003C_003E1__state = -3;
				goto IL_00ac;
				IL_0092:
				if (_003C_003E7__wrap2.MoveNext())
				{
					Dialog_InfoCard.Hyperlink current = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap2 = null;
				goto IL_00ac;
				IL_00ac:
				if (_003C_003E7__wrap1.MoveNext())
				{
					BookOutcomeDoer current2 = _003C_003E7__wrap1.Current;
					_003C_003E7__wrap2 = current2.GetHyperlinks().GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0092;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Dialog_InfoCard.Hyperlink> IEnumerable<Dialog_InfoCard.Hyperlink>.GetEnumerator()
		{
			_003CGetHyperlinks_003Ed__49 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetHyperlinks_003Ed__49(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Dialog_InfoCard.Hyperlink>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__51 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Book _003C_003E4__this;

		private IEnumerator<StatDrawEntry> _003C_003E7__wrap1;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CSpecialDisplayStats_003Ed__51(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Book book = _003C_003E4__this;
				StringBuilder stringBuilder;
				JoyKindDef reading;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingWithComps)book).SpecialDisplayStats().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0074;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0074;
				case 2:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_0074:
					if (_003C_003E7__wrap1.MoveNext())
					{
						StatDrawEntry current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					stringBuilder = new StringBuilder();
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("Stat_RecreationType_Desc".Translate());
					stringBuilder.AppendLine();
					foreach (JoyKindDef allDef in DefDatabase<JoyKindDef>.AllDefs)
					{
						stringBuilder.AppendLine("  - " + allDef.LabelCap);
					}
					reading = JoyKindDefOf.Reading;
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "StatsReport_JoyKind".Translate(), reading.LabelCap, stringBuilder.ToString(), 4750, reading.LabelCap);
					_003C_003E1__state = 2;
					return true;
				}
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__51 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__51(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	private string title;

	private bool isOpen;

	private bool descCanBeInvalidated;

	private float mentalBreakChancePerHour;

	private float joyFactor = 1f;

	private string descriptionFlavor;

	private string description;

	private int generations;

	private Graphic openGraphic;

	private Graphic verticalGraphic;

	private CompBook cachedComp;

	private static List<BookSubjectSymbol> subjects = new List<BookSubjectSymbol>();

	public CompBook BookComp => cachedComp ?? (cachedComp = GetComp<CompBook>());

	private Graphic OpenGraphic => openGraphic ?? (openGraphic = BookComp.Props.openGraphic.Graphic);

	public Graphic VerticalGraphic => verticalGraphic ?? (verticalGraphic = BookComp.Props.verticalGraphic.Graphic);

	public override string LabelNoCount => title + GenLabel.LabelExtras(this, includeHp: true, includeQuality: true);

	public override string LabelNoParenthesis => title;

	public string FlavorUI => descriptionFlavor;

	public float MentalBreakChancePerHour => mentalBreakChancePerHour;

	public float JoyFactor => joyFactor;

	public string Title => title;

	public bool IsOpen
	{
		get
		{
			return isOpen;
		}
		set
		{
			isOpen = value;
		}
	}

	public virtual bool IsReadable => true;

	public override string DescriptionFlavor => DescriptionDetailed;

	public override string DescriptionDetailed
	{
		get
		{
			EnsureDescriptionUpToDate();
			return description;
		}
	}

	public override void PostPostMake()
	{
		base.PostPostMake();
		if (!this.HasComp<CompQuality>())
		{
			GenerateBook(null, null);
		}
	}

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		base.SpawnSetup(map, respawningAfterLoad);
		LessonAutoActivator.TeachOpportunity(ConceptDefOf.Books, OpportunityType.GoodToKnow);
	}

	public override bool CanStackWith(Thing other)
	{
		return false;
	}

	protected override void DrawAt(Vector3 drawLoc, bool flip = false)
	{
		if (isOpen)
		{
			Rot4 rot = ((!(base.ParentHolder is Pawn_CarryTracker pawn_CarryTracker)) ? base.Rotation : pawn_CarryTracker.pawn.Rotation);
			OpenGraphic.Draw(drawLoc, flip ? rot.Opposite : rot, this);
		}
		else
		{
			base.DrawAt(drawLoc, flip);
		}
	}

	public void PawnReadNow(Pawn pawn)
	{
		Job job = JobMaker.MakeJob(JobDefOf.Reading, this);
		pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
	}

	public override void PostQualitySet()
	{
		base.PostQualitySet();
		GenerateBook(null, null);
	}

	public void OnBookReadTick(Pawn pawn, int delta, float roomBonusFactor)
	{
		float factor = pawn.GetStatValue(StatDefOf.ReadingSpeed) * roomBonusFactor * (float)delta;
		foreach (BookOutcomeDoer doer in BookComp.Doers)
		{
			doer.OnReadingTick(pawn, factor);
		}
		if (ModsConfig.AnomalyActive && MentalBreakChancePerHour > 0f && Rand.MTBEventOccurs(1f / MentalBreakChancePerHour, 2500f, delta) && pawn.mindState.mentalBreaker.TryGetRandomMentalBreak(BookComp.Props.mentalBreakIntensity, out var breakDef))
		{
			TaggedString taggedString = "BookMentalBreakMessage".Translate(Label);
			pawn.mindState.mentalBreaker.TryDoMentalBreak(taggedString, breakDef);
		}
	}

	public bool ProvidesOutcome(Pawn reader)
	{
		foreach (BookOutcomeDoer doer in BookComp.Doers)
		{
			if (doer.DoesProvidesOutcome(reader))
			{
				return true;
			}
		}
		return false;
	}

	public void SetMentalBreakChance(float chance)
	{
		mentalBreakChancePerHour = Mathf.Clamp01(chance);
	}

	public void SetJoyFactor(float factor)
	{
		joyFactor = factor;
	}

	[IteratorStateMachine(typeof(_003CGetHyperlinks_003Ed__49))]
	public virtual IEnumerable<Dialog_InfoCard.Hyperlink> GetHyperlinks()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetHyperlinks_003Ed__49(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CGetFloatMenuOptions_003Ed__50))]
	public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFloatMenuOptions_003Ed__50(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__selPawn = selPawn
		};
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__51))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__51(-2)
		{
			_003C_003E4__this = this
		};
	}

	public virtual void GenerateBook(Pawn author = null, long? fixedDate = null)
	{
		using (Rand.Block(Gen.HashCombineInt(thingIDNumber, generations++)))
		{
			mentalBreakChancePerHour = 0f;
			subjects.Clear();
			GrammarRequest common = default(GrammarRequest);
			long absTicks = fixedDate ?? (GenTicks.TicksAbs - (long)(BookComp.Props.ageYearsRange.RandomInRange * 3600000f));
			common.Rules.Add(new Rule_String("date", GenDate.DateFullStringAt(absTicks, Vector2.zero)));
			common.Rules.Add(new Rule_String("date_season", GenDate.DateMonthYearStringAt(absTicks, Vector2.zero)));
			if (this.HasComp<CompQuality>())
			{
				common.Constants.Add("quality", ((int)GetComp<CompQuality>().Quality).ToString());
			}
			foreach (Rule rule in ((author == null) ? TaleData_Pawn.GenerateRandom(humanLike: true) : TaleData_Pawn.GenerateFrom(author)).GetRules("ANYPAWN", common.Constants))
			{
				common.Rules.Add(rule);
			}
			AppendDoerRules(author, ref common);
			AppendRulesForSubject(subjects, common.Rules, common.Constants, "primary", 0);
			AppendRulesForSubject(subjects, common.Rules, common.Constants, "secondary", 1);
			AppendRulesForSubject(subjects, common.Rules, common.Constants, "tertiary", 2);
			GrammarRequest request = common;
			request.Includes.Add(BookComp.Props.nameMaker);
			title = GenText.CapitalizeAsTitle(GrammarResolver.Resolve("title", request)).StripTags();
			GrammarRequest request2 = common;
			request2.Includes.Add(BookComp.Props.descriptionMaker);
			request2.Includes.Add(RulePackDefOf.TalelessImages);
			request2.Includes.Add(RulePackDefOf.ArtDescriptionRoot_Taleless);
			request2.Includes.Add(RulePackDefOf.ArtDescriptionUtility_Global);
			descriptionFlavor = GrammarResolver.Resolve("desc", request2).StripTags();
			description = GenerateFullDescription();
			subjects.Clear();
		}
	}

	private void AppendDoerRules(Pawn author, ref GrammarRequest common)
	{
		foreach (BookOutcomeDoer doer in BookComp.Doers)
		{
			doer.Reset();
			doer.OnBookGenerated(author);
			IEnumerable<RulePack> topicRulePacks = doer.GetTopicRulePacks();
			if (topicRulePacks != null)
			{
				foreach (RulePack item in topicRulePacks)
				{
					common.IncludesBare.Add(item);
					List<(string, string)> list = new List<(string, string)>();
					foreach (Rule rule in item.Rules)
					{
						if (rule.keyword.StartsWith("subject_"))
						{
							list.Add((rule.keyword.Substring("subject_".Length), GrammarResolver.Resolve(rule.keyword, common, null, forceLog: false, null, null, null, capitalizeFirstSentence: false)));
						}
					}
					subjects.Add(new BookSubjectSymbol
					{
						keyword = GrammarResolver.Resolve("subject", common, null, forceLog: false, null, null, null, capitalizeFirstSentence: false),
						subSymbols = list
					});
				}
			}
			IEnumerable<Rule_String> topicRuleStrings = doer.GetTopicRuleStrings();
			if (topicRuleStrings == null)
			{
				continue;
			}
			foreach (Rule_String item2 in topicRuleStrings)
			{
				common.Rules.Add(item2);
			}
		}
	}

	private void EnsureDescriptionUpToDate()
	{
		if (descCanBeInvalidated)
		{
			description = GenerateFullDescription();
		}
	}

	private string GenerateFullDescription()
	{
		using (Rand.Block(Gen.HashCombineInt(thingIDNumber, generations)))
		{
			StringBuilder stringBuilder = new StringBuilder();
			descCanBeInvalidated = false;
			stringBuilder.AppendLine(title.Colorize(ColoredText.TipSectionTitleColor) + GenLabel.LabelExtras(this, includeHp: false, includeQuality: true) + "\n");
			stringBuilder.AppendLine(descriptionFlavor + "\n");
			if (MentalBreakChancePerHour > 0f)
			{
				stringBuilder.AppendLine($" - {"BookMentalBreak".Translate()}: {"PerHour".Translate(MentalBreakChancePerHour.ToStringPercent("0.0"))}");
			}
			foreach (BookOutcomeDoer doer in BookComp.Doers)
			{
				string benefitsString = doer.GetBenefitsString();
				if (!string.IsNullOrEmpty(benefitsString))
				{
					if (doer.BenefitDetailsCanChange())
					{
						descCanBeInvalidated = true;
					}
					stringBuilder.AppendLine(benefitsString);
				}
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}
	}

	private static void AppendRulesForSubject(List<BookSubjectSymbol> subjects, List<Rule> rules, Dictionary<string, string> constants, string postfix, int i)
	{
		if (i < subjects.Count)
		{
			rules.Add(new Rule_String("subject_" + postfix, subjects[i].keyword));
			constants.Add("length_subject_" + postfix, subjects[i].keyword.Length.ToString());
			constants.Add("has_subject_" + postfix, "true");
			{
				foreach (var subSymbol in subjects[i].subSymbols)
				{
					rules.Add(new Rule_String("subject_" + postfix + "_" + subSymbol.Item1, subSymbol.Item2));
				}
				return;
			}
		}
		constants.Add("has_subject_" + postfix, "false");
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref title, "title");
		Scribe_Values.Look(ref descriptionFlavor, "descriptionFlavor");
		Scribe_Values.Look(ref mentalBreakChancePerHour, "mentalBreakChancePerHour", 0f);
		Scribe_Values.Look(ref joyFactor, "joyFactor", 0f);
		Scribe_Values.Look(ref isOpen, "isOpen", defaultValue: false);
		Scribe_Values.Look(ref descCanBeInvalidated, "descCanBeInvalidated", defaultValue: false);
		Scribe_Values.Look(ref description, "description");
		Scribe_Values.Look(ref generations, "generations", 0);
	}
}
