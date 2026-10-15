using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

[StaticConstructorOnStartup]
public class Dialog_BeginRitual : Dialog_BeginLordJob
{
	public delegate bool ActionCallback(RitualRoleAssignments assignments);

	public delegate bool PawnFilter(Pawn pawn, bool voluntary, bool allowOtherIdeos);

	[CompilerGenerated]
	private sealed class _003CBlockingIssues_003Ed__35 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Dialog_BeginRitual _003C_003E4__this;

		private List<Pawn>.Enumerator _003C_003E7__wrap1;

		private IEnumerator<string> _003C_003E7__wrap2;

		private IEnumerator<IGrouping<string, RitualRole>> _003C_003E7__wrap3;

		private RitualRole _003CfirstRole_003E5__5;

		private int _003CrequiredPawnCount_003E5__6;

		private IEnumerable<Pawn> _003CselectedPawns_003E5__7;

		private IEnumerator<Pawn> _003C_003E7__wrap7;

		string IEnumerator<string>.Current
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
		public _003CBlockingIssues_003Ed__35(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			switch (num)
			{
			case -3:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 4:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -5:
			case 5:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			case -7:
			case -6:
			case 6:
			case 7:
			case 8:
				try
				{
					if (num == -7 || num == 6)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally5();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally4();
				}
				break;
			case -8:
			case 9:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally6();
				}
				break;
			}
			_003C_003E7__wrap1 = default(List<Pawn>.Enumerator);
			_003C_003E7__wrap2 = null;
			_003C_003E7__wrap3 = null;
			_003CfirstRole_003E5__5 = null;
			_003CselectedPawns_003E5__7 = null;
			_003C_003E7__wrap7 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Dialog_BeginRitual CS_0024_003C_003E8__locals29 = _003C_003E4__this;
				bool stillAddToPawnList;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (!CS_0024_003C_003E8__locals29.assignments.Participants.Any())
					{
						_003C_003E2__current = "MessageRitualNeedsAtLeastOnePerson".Translate();
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_008a;
				case 1:
					_003C_003E1__state = -1;
					goto IL_008a;
				case 2:
					_003C_003E1__state = -3;
					goto IL_013f;
				case 3:
					_003C_003E1__state = -1;
					goto IL_01aa;
				case 4:
					_003C_003E1__state = -4;
					goto IL_0219;
				case 5:
					_003C_003E1__state = -5;
					goto IL_029c;
				case 6:
					_003C_003E1__state = -7;
					goto IL_03f4;
				case 7:
					_003C_003E1__state = -6;
					goto IL_04dc;
				case 8:
					_003C_003E1__state = -6;
					goto IL_04dc;
				case 9:
					_003C_003E1__state = -8;
					goto IL_058b;
				case 10:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_029c:
					if (_003C_003E7__wrap2.MoveNext())
					{
						string current = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 5;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap2 = null;
					goto IL_02b6;
					IL_008a:
					_003C_003E7__wrap1 = CS_0024_003C_003E8__locals29.assignments.Participants.GetEnumerator();
					_003C_003E1__state = -3;
					while (_003C_003E7__wrap1.MoveNext())
					{
						Pawn current2 = _003C_003E7__wrap1.Current;
						if (!current2.IsPrisoner && !current2.SafeTemperatureRange().IncludesEpsilon(CS_0024_003C_003E8__locals29.target.Cell.GetTemperature(CS_0024_003C_003E8__locals29.target.Map)))
						{
							Precept_Ritual ritual = CS_0024_003C_003E8__locals29.ritual;
							if (ritual == null || !ritual.ignoreExtremeTemperatures)
							{
								_003C_003E2__current = "CantJoinRitualInExtremeWeather".Translate();
								_003C_003E1__state = 2;
								return true;
							}
						}
					}
					goto IL_013f;
					IL_03f4:
					while (_003C_003E7__wrap7.MoveNext())
					{
						Pawn current3 = _003C_003E7__wrap7.Current;
						string text = CS_0024_003C_003E8__locals29.assignments.PawnNotAssignableReason(current3, _003CfirstRole_003E5__5, out stillAddToPawnList);
						if (text != null)
						{
							_003C_003E2__current = text;
							_003C_003E1__state = 6;
							return true;
						}
					}
					_003C_003Em__Finally5();
					_003C_003E7__wrap7 = null;
					if (_003CrequiredPawnCount_003E5__6 == 1 && !_003CselectedPawns_003E5__7.Any())
					{
						_003C_003E2__current = "MessageLordJobNeedsAtLeastOneRolePawn".Translate(_003CfirstRole_003E5__5.Label.Resolve());
						_003C_003E1__state = 7;
						return true;
					}
					if (_003CrequiredPawnCount_003E5__6 > 1 && _003CselectedPawns_003E5__7.Count() < _003CrequiredPawnCount_003E5__6)
					{
						_003C_003E2__current = "MessageLordJobNeedsAtLeastNumRolePawn".Translate(Find.ActiveLanguageWorker.Pluralize(_003CfirstRole_003E5__5.Label), _003CrequiredPawnCount_003E5__6);
						_003C_003E1__state = 8;
						return true;
					}
					goto IL_04dc;
					IL_013f:
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<Pawn>.Enumerator);
					if (CS_0024_003C_003E8__locals29.ritual == null)
					{
						break;
					}
					if (CS_0024_003C_003E8__locals29.ritual.behavior.SpectatorsRequired() && CS_0024_003C_003E8__locals29.assignments.SpectatorsForReading.Count == 0)
					{
						_003C_003E2__current = "MessageRitualNeedsAtLeastOneSpectator".Translate();
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_01aa;
					IL_058b:
					while (_003C_003E7__wrap1.MoveNext())
					{
						Pawn current4 = _003C_003E7__wrap1.Current;
						string text2 = CS_0024_003C_003E8__locals29.assignments.PawnNotAssignableReason(current4, CS_0024_003C_003E8__locals29.assignments.RoleForPawn(current4), out stillAddToPawnList);
						if (text2 != null)
						{
							_003C_003E2__current = text2;
							_003C_003E1__state = 9;
							return true;
						}
					}
					_003C_003Em__Finally6();
					_003C_003E7__wrap1 = default(List<Pawn>.Enumerator);
					goto IL_05aa;
					IL_02b6:
					if (!CS_0024_003C_003E8__locals29.ritual.behavior.def.roles.NullOrEmpty())
					{
						_003C_003E7__wrap3 = (from r in CS_0024_003C_003E8__locals29.ritual.behavior.def.roles
							group r by r.mergeId ?? r.id).GetEnumerator();
						_003C_003E1__state = -6;
						goto IL_04ea;
					}
					goto IL_05aa;
					IL_01aa:
					if (CS_0024_003C_003E8__locals29.ritual.outcomeEffect != null)
					{
						_003C_003E7__wrap2 = CS_0024_003C_003E8__locals29.ritual.outcomeEffect.BlockingIssues(CS_0024_003C_003E8__locals29.ritual, CS_0024_003C_003E8__locals29.target, CS_0024_003C_003E8__locals29.assignments).GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_0219;
					}
					goto IL_0233;
					IL_04dc:
					_003CselectedPawns_003E5__7 = null;
					goto IL_04e3;
					IL_0219:
					if (_003C_003E7__wrap2.MoveNext())
					{
						string current5 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current5;
						_003C_003E1__state = 4;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
					goto IL_0233;
					IL_04e3:
					_003CfirstRole_003E5__5 = null;
					goto IL_04ea;
					IL_04ea:
					if (_003C_003E7__wrap3.MoveNext())
					{
						IGrouping<string, RitualRole> current6 = _003C_003E7__wrap3.Current;
						_003CfirstRole_003E5__5 = current6.First();
						_003CrequiredPawnCount_003E5__6 = current6.Count((RitualRole r) => r.required);
						if (_003CrequiredPawnCount_003E5__6 > 0)
						{
							_003CselectedPawns_003E5__7 = current6.SelectMany((RitualRole r) => CS_0024_003C_003E8__locals29.assignments.AssignedPawns(r));
							_003C_003E7__wrap7 = _003CselectedPawns_003E5__7.GetEnumerator();
							_003C_003E1__state = -7;
							goto IL_03f4;
						}
						goto IL_04e3;
					}
					_003C_003Em__Finally4();
					_003C_003E7__wrap3 = null;
					if (!CS_0024_003C_003E8__locals29.assignments.ExtraRequiredPawnsForReading.NullOrEmpty())
					{
						_003C_003E7__wrap1 = CS_0024_003C_003E8__locals29.assignments.ExtraRequiredPawnsForReading.GetEnumerator();
						_003C_003E1__state = -8;
						goto IL_058b;
					}
					goto IL_05aa;
					IL_0233:
					if (CS_0024_003C_003E8__locals29.ritual.obligationTargetFilter != null)
					{
						_003C_003E7__wrap2 = CS_0024_003C_003E8__locals29.ritual.obligationTargetFilter.GetBlockingIssues(CS_0024_003C_003E8__locals29.target, CS_0024_003C_003E8__locals29.assignments).GetEnumerator();
						_003C_003E1__state = -5;
						goto IL_029c;
					}
					goto IL_02b6;
					IL_05aa:
					if (CS_0024_003C_003E8__locals29.ritual.ritualOnlyForIdeoMembers && !CS_0024_003C_003E8__locals29.assignments.Participants.Any((Pawn p) => p.Ideo == CS_0024_003C_003E8__locals29.ritual.ideo))
					{
						_003C_003E2__current = "MessageNeedAtLeastOneParticipantOfIdeo".Translate(CS_0024_003C_003E8__locals29.ritual.ideo.memberName);
						_003C_003E1__state = 10;
						return true;
					}
					break;
				}
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally4()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap3 != null)
			{
				_003C_003E7__wrap3.Dispose();
			}
		}

		private void _003C_003Em__Finally5()
		{
			_003C_003E1__state = -6;
			if (_003C_003E7__wrap7 != null)
			{
				_003C_003E7__wrap7.Dispose();
			}
		}

		private void _003C_003Em__Finally6()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CBlockingIssues_003Ed__35 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CBlockingIssues_003Ed__35(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	protected RitualRoleAssignments assignments;

	protected Precept_Ritual ritual;

	protected TargetInfo target;

	protected RitualObligation obligation;

	protected RitualOutcomeEffectDef outcome;

	protected PawnFilter filter;

	protected Pawn organizer;

	protected Map map;

	protected ActionCallback action;

	protected List<string> extraInfos;

	private string sleepingMessage;

	protected string ritualExplanation;

	protected string ritualLabel;

	protected string okButtonText;

	protected string confirmText;

	private static List<Precept_Role> cachedRoles = new List<Precept_Role>();

	protected const float RoleHeight = 40f;

	protected const float TargetIconSize = 32f;

	public override TaggedString HeaderLabel => ritualLabel;

	public override TaggedString OkButtonLabel => okButtonText;

	public override Texture2D Icon => ritual?.sourcePattern?.Icon;

	public string SleepingWarning
	{
		get
		{
			if (sleepingMessage.NullOrEmpty())
			{
				sleepingMessage = "RitualBeginSleepingWarning".Translate();
			}
			if (assignments.Participants.Any((Pawn p) => !p.Awake()))
			{
				return sleepingMessage;
			}
			return null;
		}
	}

	public override TaggedString DescriptionLabel => (ritual?.behavior?.descriptionOverride ?? ritual?.Description)?.Formatted(organizer.Named("ORGANIZER")) ?? TaggedString.Empty;

	public override TaggedString ExtraExplanationLabel
	{
		get
		{
			string text = ritual?.behavior?.GetExplanation(ritual, assignments, PredictedQuality().min);
			if (!ritualExplanation.NullOrEmpty() || !text.NullOrEmpty())
			{
				string text2 = ritualExplanation;
				if (!text.NullOrEmpty())
				{
					if (!text2.NullOrEmpty())
					{
						text2 += "\n\n";
					}
					text2 += text;
				}
				return text2;
			}
			return "";
		}
	}

	public override TaggedString ExpectedQualityLabel
	{
		get
		{
			if (ritual?.outcomeEffect?.ExpectedQualityLabel() != null)
			{
				return ritual.outcomeEffect.ExpectedQualityLabel();
			}
			return base.ExpectedQualityLabel;
		}
	}

	public override Color QualitySummaryColor(FloatRange qualityRange)
	{
		if (!(qualityRange.min < 0.25f))
		{
			return Color.white;
		}
		return ColorLibrary.RedReadable;
	}

	[IteratorStateMachine(typeof(_003CBlockingIssues_003Ed__35))]
	protected override IEnumerable<string> BlockingIssues()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CBlockingIssues_003Ed__35(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override TaggedString ExpectedDurationLabel(FloatRange qualityRange)
	{
		if (ritual == null)
		{
			return TaggedString.Empty;
		}
		string text = ritual.behavior.ExpectedDuration(ritual, assignments, qualityRange.min);
		if (text.NullOrEmpty())
		{
			return null;
		}
		return "{0}: {1}".Formatted("ExpectedLordJobDuration".Translate(), text);
	}

	public override string OutcomeToolTip(ILordJobOutcomePossibility outcomeChance)
	{
		string text = outcome.OutcomeMoodBreakdown(outcomeChance as RitualOutcomePossibility);
		string text2 = base.OutcomeToolTip(outcomeChance);
		if (text2 != null && !text2.NullOrEmpty())
		{
			if (!text.NullOrEmpty())
			{
				text += "\n\n";
			}
			text += text2;
		}
		return text;
	}

	private Dialog_BeginRitual(RitualRoleAssignments assignments, Precept_Ritual ritual, TargetInfo target, RitualOutcomeEffectDef outcome)
		: base(new PawnRitualRoleSelectionWidget(assignments, ritual, target, outcome))
	{
		this.assignments = assignments;
		this.ritual = ritual;
		this.target = target;
		this.outcome = outcome;
	}

	public Dialog_BeginRitual(string ritualLabel, Precept_Ritual ritual, TargetInfo target, Map map, ActionCallback action, Pawn organizer, RitualObligation obligation, PawnFilter filter = null, string okButtonText = null, List<Pawn> requiredPawns = null, Dictionary<string, Pawn> forcedForRole = null, RitualOutcomeEffectDef outcome = null, List<string> extraInfoText = null, Pawn selectedPawn = null)
		: this(CreateRitualRoleAssignments(ritual, target, map, filter, requiredPawns, forcedForRole, selectedPawn), ritual, target, ritual?.outcomeEffect?.def ?? outcome)
	{
		this.obligation = obligation;
		this.filter = filter;
		this.organizer = organizer;
		this.map = map;
		this.action = action;
		ritualExplanation = ritual?.ritualExplanation;
		this.ritualLabel = ritualLabel;
		this.okButtonText = okButtonText ?? ((string)base.OkButtonLabel);
		extraInfos = extraInfoText;
		cachedRoles.Clear();
		if (ritual?.ideo != null)
		{
			cachedRoles.AddRange(ritual.ideo.RolesListForReading.Where((Precept_Role r) => !r.def.leaderRole));
			Precept_Role precept_Role = Faction.OfPlayer.ideos.PrimaryIdeo.RolesListForReading.FirstOrDefault((Precept_Role p) => p.def.leaderRole);
			if (precept_Role != null)
			{
				cachedRoles.Add(precept_Role);
			}
			cachedRoles.SortBy((Precept_Role x) => x.def.displayOrderInImpact);
		}
	}

	protected override void Start()
	{
		if (!confirmText.NullOrEmpty())
		{
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(confirmText, delegate
			{
				if (PredictedQuality().min < 0.25f && outcome.warnOnLowQuality)
				{
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("RitualQualityLowWarning".Translate(HeaderLabel, 0.25f.ToStringPercent()), InnerStart, destructive: true));
				}
				else
				{
					InnerStart();
				}
			}, destructive: true));
		}
		else
		{
			InnerStart();
		}
		void InnerStart()
		{
			ActionCallback actionCallback = action;
			if (actionCallback != null && actionCallback(assignments))
			{
				Close();
			}
		}
	}

	private FloatRange PredictedQuality(List<QualityFactor> expectedOutcomeEffects = null)
	{
		float num = outcome.startingQuality;
		float num2 = 0f;
		foreach (RitualOutcomeComp comp in outcome.comps)
		{
			QualityFactor qualityFactor = comp.GetQualityFactor(ritual, target, obligation, assignments, ritual?.outcomeEffect?.DataForComp(comp));
			if (qualityFactor != null)
			{
				if (!qualityFactor.label.NullOrEmpty())
				{
					expectedOutcomeEffects?.Add(qualityFactor);
				}
				if (qualityFactor.uncertainOutcome)
				{
					num2 += qualityFactor.quality;
				}
				else
				{
					num += qualityFactor.quality;
				}
			}
		}
		if (ritual != null && ritual.RepeatPenaltyActive)
		{
			num += ritual.RepeatQualityPenalty;
		}
		Tuple<ExpectationDef, float> expectationsOffset = RitualOutcomeEffectWorker_FromQuality.GetExpectationsOffset(map, ritual?.def);
		if (expectationsOffset != null)
		{
			num += expectationsOffset.Item2;
		}
		num = Mathf.Clamp(num, outcome.minQuality, outcome.maxQuality);
		num2 += num;
		num2 = Mathf.Clamp(num2, outcome.minQuality, outcome.maxQuality);
		return new FloatRange(num, num2);
	}

	protected override List<QualityFactor> PopulateQualityFactors(out FloatRange qualityRange)
	{
		List<QualityFactor> list = base.PopulateQualityFactors(out qualityRange);
		if (outcome == null)
		{
			return list;
		}
		float startingQuality = outcome.startingQuality;
		qualityRange = PredictedQuality(list);
		if (startingQuality > 0f)
		{
			list.Add(new QualityFactor
			{
				label = "StartingQuality".Translate(),
				qualityChange = "+" + startingQuality.ToStringPercent("F0"),
				quality = startingQuality,
				noMiddleColumnInfo = true,
				positive = true,
				priority = 5f
			});
		}
		if (ritual != null && ritual.RepeatPenaltyActive)
		{
			float repeatQualityPenalty = ritual.RepeatQualityPenalty;
			list.Add(new QualityFactor
			{
				label = "RitualOutcomePerformedRecently".Translate(),
				qualityChange = repeatQualityPenalty.ToStringPercent(),
				quality = repeatQualityPenalty,
				noMiddleColumnInfo = true,
				positive = false,
				priority = 5f
			});
		}
		Tuple<ExpectationDef, float> expectationsOffset = RitualOutcomeEffectWorker_FromQuality.GetExpectationsOffset(map, ritual?.def);
		if (expectationsOffset != null)
		{
			list.Add(new QualityFactor
			{
				label = "RitualQualityExpectations".Translate(expectationsOffset.Item1.LabelCap),
				qualityChange = "+" + expectationsOffset.Item2.ToStringPercent(),
				quality = expectationsOffset.Item2,
				noMiddleColumnInfo = true,
				positive = true,
				priority = 5f
			});
		}
		return list;
	}

	protected override List<ILordJobOutcomePossibility> PopulateOutcomePossibilities()
	{
		List<ILordJobOutcomePossibility> list = base.PopulateOutcomePossibilities();
		RitualOutcomeEffectDef ritualOutcomeEffectDef = outcome;
		if (ritualOutcomeEffectDef != null && ritualOutcomeEffectDef.outcomeChances?.NullOrEmpty() == false)
		{
			list.AddRange(outcome.outcomeChances);
			if (ritual != null && ritual.outcomeEffect != null)
			{
				RitualOutcomePossibility forcedOutcome = ritual.outcomeEffect.GetForcedOutcome(ritual, target, obligation, assignments);
				if (forcedOutcome != null)
				{
					list.Clear();
					list.Add(forcedOutcome);
				}
			}
		}
		return list;
	}

	public override void DrawExtraOutcomeDescriptions(Rect viewRect, FloatRange qualityRange, string qualityNumber, ref float curY, ref float totalInfoHeight)
	{
		DrawExtraRitualOutcomeDescriptions(viewRect, qualityRange.min, ref curY, ref totalInfoHeight);
		curY += 10f;
		totalInfoHeight += 10f;
		DrawExtraInfo(viewRect, ref curY, ref totalInfoHeight);
		DrawOutcomeWarnings(viewRect, ref curY, ref totalInfoHeight);
		DrawFinalOutcomeInfo(viewRect, ref curY, ref totalInfoHeight);
	}

	public void DrawExtraRitualOutcomeDescriptions(Rect viewRect, float totalQuality, ref float curY, ref float totalInfoHeight)
	{
		if (outcome.extraOutcomeDescriptions == null)
		{
			return;
		}
		curY += Text.LineHeight;
		totalInfoHeight += Text.LineHeight;
		for (int i = 0; i < outcome.extraOutcomeDescriptions?.Count; i++)
		{
			RitualOutcomeEffectDef.ExtraOutcomeChanceDescription extraOutcomeChanceDescription = outcome.extraOutcomeDescriptions[i];
			TaggedString taggedString = GrammarResolverSimpleStringExtensions.Formatted(arg1: extraOutcomeChanceDescription.qualityToValue(totalQuality), str: extraOutcomeChanceDescription.description);
			Vector2 vector = Text.CalcSize(taggedString);
			if (vector.x > viewRect.width)
			{
				vector = new Vector2(viewRect.width, Text.CalcHeight(taggedString, viewRect.width));
			}
			Widgets.Label(new Rect(viewRect.x, curY, vector.x, Mathf.Max(32f, vector.y)), taggedString);
			curY += vector.y;
			totalInfoHeight += vector.y;
		}
	}

	public void DrawExtraInfo(Rect viewRect, ref float curY, ref float totalInfoHeight)
	{
		if (extraInfos == null)
		{
			return;
		}
		foreach (string extraInfo in extraInfos)
		{
			float num = Math.Max(Text.CalcHeight(extraInfo, viewRect.width) + 3f, 28f);
			Widgets.Label(new Rect(viewRect.x, curY + 4f, viewRect.width, num), extraInfo);
			curY += num;
			totalInfoHeight += num;
		}
	}

	public void DrawOutcomeWarnings(Rect viewRect, ref float curY, ref float totalInfoHeight)
	{
		string sleepingWarning = SleepingWarning;
		if (!sleepingWarning.NullOrEmpty())
		{
			float num = Math.Max(Text.CalcHeight(sleepingWarning, viewRect.width) + 3f, 28f);
			Widgets.Label(new Rect(viewRect.x, curY + 4f, viewRect.width, num), sleepingWarning);
			curY += num;
		}
	}

	public void DrawFinalOutcomeInfo(Rect viewRect, ref float curY, ref float totalInfoHeight)
	{
		if (outcome.outcomeChances.NullOrEmpty())
		{
			return;
		}
		Precept_Ritual precept_Ritual = ritual;
		if (precept_Ritual == null || precept_Ritual.ideo?.Fluid != true)
		{
			return;
		}
		SimpleCurve developmentPointsOverOutcomeIndexCurveForRitual = IdeoDevelopmentUtility.GetDevelopmentPointsOverOutcomeIndexCurveForRitual(ritual.ideo, ritual);
		if (developmentPointsOverOutcomeIndexCurveForRitual != null)
		{
			curY += 10f;
			totalInfoHeight += 10f;
			TaggedString taggedString = "RitualDevelopmentPointRewards".Translate() + ":\n";
			float num = Text.CalcHeight(taggedString, viewRect.width);
			Widgets.Label(new Rect(viewRect.x, curY, viewRect.width, num), taggedString);
			curY += num;
			for (int i = 0; i < outcome.outcomeChances.Count; i++)
			{
				RitualOutcomePossibility ritualOutcomePossibility = outcome.outcomeChances[i];
				string label = "  - " + ritualOutcomePossibility.label + ": " + developmentPointsOverOutcomeIndexCurveForRitual.Evaluate(i).ToStringWithSign();
				Widgets.Label(new Rect(viewRect.x, curY, viewRect.width, 32f), label);
				curY += Text.LineHeight;
				totalInfoHeight += Text.LineHeight;
			}
		}
	}

	public override void DoExtraHeaderInfo(ref RectDivider layout, ref RectDivider headerLabelRow)
	{
		if (ritual == null)
		{
			return;
		}
		if (ModsConfig.IdeologyActive && ritual.ideo != null && !Find.IdeoManager.classicMode && !ritual.def.isNonIdeoRitual)
		{
			Ideo ideo = ritual.ideo;
			Vector2 vector = Text.CalcSize(ideo.name.CapitalizeFirst());
			IdeoUIUtility.DrawIdeoPlate(headerLabelRow.NewRow(vector.y, VerticalJustification.Top, null).NewCol(vector.x + 30f, HorizontalJustification.Right, null), ideo);
		}
		using (new TextBlock(GameFont.Small, TextAnchor.LowerRight, false, Color.gray))
		{
			if (!ritual.Label.EqualsIgnoreCase(ritual.UIInfoFirstLine))
			{
				Rect rect = new Rect(headerLabelRow);
				rect.height = Text.CalcHeight(ritual.UIInfoFirstLine, headerLabelRow.Rect.width);
				Widgets.Label(rect, ritual.UIInfoFirstLine);
			}
		}
	}

	public override void PostOpen()
	{
		base.PostOpen();
		assignments.FillPawns(filter, target);
		if (outcome == null || ritual == null || ritual.outcomeEffect == null)
		{
			return;
		}
		foreach (RitualOutcomeComp comp in outcome.comps)
		{
			comp.Notify_AssignmentsChanged(assignments, ritual.outcomeEffect.DataForComp(comp));
		}
	}

	protected override void DrawQualityFactors(Rect viewRect)
	{
		Precept_Ritual precept_Ritual = ritual;
		if (precept_Ritual != null && precept_Ritual.outcomeEffect?.ShowQuality == false)
		{
			Rect rect = viewRect;
			rect.y += 17f;
			string text = ritual.outcomeEffect.def.outcomeChances.MaxBy((RitualOutcomePossibility c) => c.positivityIndex).label.CapitalizeFirst();
			TaggedString taggedString = "RitualOutcomeNoQuality".Translate() + ":\n\n  - " + text + " " + ritual.Label;
			Rect rect2 = rect;
			rect2.width += 10f;
			rect2.height = Text.CalcHeight(taggedString, rect.width);
			rect2 = rect2.ExpandedBy(9f);
			GUI.color = new Color(0.25f, 0.25f, 0.25f);
			Widgets.DrawBox(rect2, 2);
			GUI.color = Color.white;
			Widgets.Label(rect, taggedString);
		}
		else
		{
			base.DrawQualityFactors(viewRect);
		}
	}

	private void UpdateRoleChangeTargetRole(Pawn p)
	{
		Precept_Role roleToChangeTo = null;
		if (p.Ideo?.GetRole(p) == null)
		{
			roleToChangeTo = RitualUtility.AllRolesForPawn(p).FirstOrDefault((Precept_Role r) => r.Active && r.RequirementsMet(p));
		}
		SetRoleToChangeTo(roleToChangeTo);
	}

	public void SetRoleToChangeTo(Precept_Role role)
	{
		assignments.SetRoleChangeSelection(role);
	}

	private void DrawRoleSelection(Pawn pawn, Rect rect)
	{
		Precept_Role roleChangeSelection = assignments.RoleChangeSelection;
		Precept_Role currentRole = pawn?.Ideo?.GetRole(pawn);
		if (roleChangeSelection == null && currentRole == null)
		{
			UpdateRoleChangeTargetRole(pawn);
			roleChangeSelection = assignments.RoleChangeSelection;
		}
		if (roleChangeSelection != null || currentRole != null)
		{
			SocialCardUtility.DrawPawnRole(pawn, roleChangeSelection, (roleChangeSelection != null) ? roleChangeSelection.LabelCap : "RemoveRole".Translate(currentRole.Label).Resolve(), rect, drawLine: false);
		}
		Rect rect2 = new Rect(rect.x + 220f, rect.y + 2f, 140f, 32f);
		bool flag = pawn?.Ideo != null;
		if (!flag)
		{
			GUI.color = Color.gray;
		}
		if (cachedRoles.Count > 1 && Widgets.ButtonText(rect2, "ChooseNewRole".Translate() + "...", drawBackground: true, doMouseoverSound: true, flag, null))
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			if (currentRole != null)
			{
				list.Add(new FloatMenuOption("None".Translate(), delegate
				{
					confirmText = "ChooseRoleConfirmUnassign".Translate(currentRole.Named("ROLE"), pawn.Named("PAWN")) + "\n\n" + "ChooseRoleConfirmAssignPostfix".Translate();
					SetRoleToChangeTo(null);
				}, Widgets.PlaceholderIconTex, Color.white));
			}
			foreach (Precept_Role cachedRole in cachedRoles)
			{
				Precept_Role newRole = cachedRole;
				if (newRole == roleChangeSelection || newRole == currentRole || !newRole.Active || !newRole.RequirementsMet(pawn) || (newRole.def.leaderRole && pawn.Ideo != Faction.OfPlayer.ideos.PrimaryIdeo))
				{
					continue;
				}
				string text = newRole.LabelForPawn(pawn) + " (" + newRole.def.label + ")";
				TaggedString confirmTextLocal = "ChooseRoleConfirmAssign".Translate(newRole.Named("ROLE"), pawn.Named("PAWN"));
				string extraConfirmText = RitualUtility.RoleChangeConfirmation(pawn, currentRole, newRole);
				Pawn pawn2 = newRole.ChosenPawns().FirstOrDefault();
				if (pawn2 != null && newRole is Precept_RoleSingle)
				{
					text = text + ": " + pawn2.LabelShort;
				}
				if (!extraConfirmText.NullOrEmpty())
				{
					list.Add(new FloatMenuOption(text, delegate
					{
						confirmText = confirmTextLocal + "\n\n" + extraConfirmText + "\n\n" + "ChooseRoleConfirmAssignPostfix".Translate();
						SetRoleToChangeTo(newRole);
					}, newRole.Icon, newRole.ideo.Color, MenuOptionPriority.Default, DrawTooltip)
					{
						orderInPriority = newRole.def.displayOrderInImpact
					});
				}
				else
				{
					list.Add(new FloatMenuOption(text, delegate
					{
						newRole.Assign(pawn, addThoughts: true);
					}, newRole.Icon, newRole.ideo.Color, MenuOptionPriority.Default, DrawTooltip)
					{
						orderInPriority = newRole.def.displayOrderInImpact
					});
				}
				void DrawTooltip(Rect r)
				{
					TipSignal tip = new TipSignal(() => newRole.GetTip(), pawn.thingIDNumber * 39);
					TooltipHandler.TipRegion(r, tip);
				}
			}
			foreach (Precept_Role cachedRole2 in cachedRoles)
			{
				if ((cachedRole2 != roleChangeSelection && !cachedRole2.RequirementsMet(pawn)) || !cachedRole2.Active)
				{
					string text2 = cachedRole2.LabelForPawn(pawn) + " (" + cachedRole2.def.label + ")";
					if (cachedRole2.ChosenPawnSingle() != null)
					{
						text2 = text2 + ": " + cachedRole2.ChosenPawnSingle().LabelShort;
					}
					else if (!cachedRole2.RequirementsMet(pawn))
					{
						text2 = text2 + ": " + cachedRole2.GetFirstUnmetRequirement(pawn).GetLabel(cachedRole2).CapitalizeFirst();
					}
					else if (!cachedRole2.Active && cachedRole2.def.activationBelieverCount > cachedRole2.ideo.ColonistBelieverCountCached)
					{
						text2 += ": " + "InactiveRoleRequiresMoreBelievers".Translate(cachedRole2.def.activationBelieverCount, cachedRole2.ideo.memberName, cachedRole2.ideo.ColonistBelieverCountCached).CapitalizeFirst();
					}
					list.Add(new FloatMenuOption(text2, null, cachedRole2.Icon, cachedRole2.ideo.Color)
					{
						orderInPriority = cachedRole2.def.displayOrderInImpact
					});
				}
			}
			if (list.Any())
			{
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}
		GUI.color = Color.white;
	}

	public void DoRoleSelection(ref RectDivider layout)
	{
		RitualRoleIdeoRoleChanger ritualRoleIdeoRoleChanger = assignments.AllRolesForReading.OfType<RitualRoleIdeoRoleChanger>().FirstOrDefault();
		if (ritualRoleIdeoRoleChanger != null && cachedRoles.Any())
		{
			Pawn pawn = assignments.FirstAssignedPawn(ritualRoleIdeoRoleChanger);
			if (pawn != null)
			{
				RectDivider rectDivider = layout.NewRow(40f, VerticalJustification.Top, 10f).NewCol(320f, HorizontalJustification.Left, null);
				DrawRoleSelection(pawn, rectDivider);
			}
		}
	}

	public override void DoRightColumn(ref RectDivider layout)
	{
		if (target.Thing != null)
		{
			Rect targetRow = layout.NewRow(32f, VerticalJustification.Bottom, 28f);
			DrawTargetLocation(targetRow);
		}
		DoRoleSelection(ref layout);
		base.DoRightColumn(ref layout);
	}

	public void DrawTargetLocation(Rect targetRow)
	{
		TaggedString taggedString = "RitualTakesPlaceAt".Translate() + ": ";
		TaggedString taggedString2 = taggedString + target.Thing.LabelShortCap;
		float x = Text.CalcSize(taggedString).x;
		float x2 = Text.CalcSize(target.Thing.LabelShortCap).x;
		float num = Text.CalcSize(taggedString2).x + 4f + 32f;
		Rect rect = new Rect(targetRow.xMax - (x2 + 4f + 32f), targetRow.y - 6f, 32f, 32f);
		Rect rect2 = new Rect(targetRow.xMax - num, targetRow.y, x, 32f);
		Rect rect3 = new Rect(targetRow.xMax - (x2 + 4f + 32f) + 32f + 4f, targetRow.y, x2, 32f);
		Widgets.Label(rect2, taggedString);
		Widgets.Label(rect3, target.Thing.LabelShortCap);
		Widgets.ThingIcon(rect, target.Thing, 1f, null);
		if (Mouse.IsOver(rect2) || Mouse.IsOver(rect3) || Mouse.IsOver(rect))
		{
			Find.WindowStack.ImmediateWindow(738453, new Rect(0f, 0f, UI.screenWidth, UI.screenHeight), WindowLayer.Super, delegate
			{
				GenUI.DrawArrowPointingAtWorldspace(target.Cell.ToVector3(), Find.Camera);
			}, doBackground: false, absorbInputAroundWindow: false, 0f);
		}
	}

	public static RitualRoleAssignments CreateRitualRoleAssignments(Precept_Ritual ritual, TargetInfo target, Map map, PawnFilter filter, List<Pawn> requiredPawns, Dictionary<string, Pawn> forcedForRole, Pawn selectedPawn)
	{
		RitualRoleAssignments ritualRoleAssignments = new RitualRoleAssignments(ritual, target);
		List<Pawn> list = new List<Pawn>(map.mapPawns.FreeColonistsAndPrisonersSpawned);
		List<Pawn> list2 = new List<Pawn>(list.Count);
		for (int num = list.Count - 1; num >= 0; num--)
		{
			Pawn pawn = list[num];
			if (filter != null && !filter(pawn, voluntary: true, allowOtherIdeos: true))
			{
				list.RemoveAt(num);
			}
			else
			{
				bool stillAddToPawnList;
				bool flag = ritualRoleAssignments.PawnNotAssignableReason(pawn, null, out stillAddToPawnList) == null || stillAddToPawnList;
				if (!flag && ritual != null)
				{
					if (pawn.DevelopmentalStage != DevelopmentalStage.Baby && pawn.DevelopmentalStage != DevelopmentalStage.Newborn)
					{
						list2.AddUnique(pawn);
					}
					foreach (RitualRole role in ritual.behavior.def.roles)
					{
						if ((ritualRoleAssignments.PawnNotAssignableReason(pawn, role, out stillAddToPawnList) == null || stillAddToPawnList) && (filter == null || filter(pawn, !(role is RitualRoleForced), role.allowOtherIdeos)) && (role.maxCount > 1 || forcedForRole == null || !forcedForRole.ContainsKey(role.id)))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					list.RemoveAt(num);
				}
			}
		}
		if (requiredPawns != null)
		{
			foreach (Pawn requiredPawn in requiredPawns)
			{
				list.AddUnique(requiredPawn);
			}
		}
		if (forcedForRole != null)
		{
			foreach (KeyValuePair<string, Pawn> item in forcedForRole)
			{
				list.AddUnique(item.Value);
			}
		}
		if (ritual != null)
		{
			foreach (RitualRole role2 in ritual.behavior.def.roles)
			{
				if (role2.Animal)
				{
					list.AddRange(map.mapPawns.SpawnedColonyAnimals.Where((Pawn p) => filter == null || filter(p, voluntary: true, allowOtherIdeos: true)));
					break;
				}
			}
		}
		ritualRoleAssignments.Setup(list, list2, forcedForRole, requiredPawns, selectedPawn);
		return ritualRoleAssignments;
	}
}
