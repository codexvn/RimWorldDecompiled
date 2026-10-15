using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld;

public class CompStudiable : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__48 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompStudiable _003C_003E4__this;

		Gizmo IEnumerator<Gizmo>.Current
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
		public _003CCompGetGizmosExtra_003Ed__48(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			CompStudiable CS_0024_003C_003E8__locals14 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				string reason;
				bool flag = CS_0024_003C_003E8__locals14.EverStudiable(out reason);
				if (CS_0024_003C_003E8__locals14.Props.showToggleGizmo && (flag || reason != null))
				{
					Command_Toggle command_Toggle = new Command_Toggle
					{
						defaultLabel = "CommandToggleStudy".Translate(),
						defaultDesc = "CommandToggleStudyDesc".Translate(),
						icon = StudyToggleIcon.Texture,
						isActive = () => CS_0024_003C_003E8__locals14.studyEnabled,
						toggleAction = delegate
						{
							CS_0024_003C_003E8__locals14.SetStudyEnabled(!CS_0024_003C_003E8__locals14.studyEnabled);
						},
						hideIconIfDisabled = true
					};
					command_Toggle.tutorTag = "ToggleStudy";
					if (reason != null)
					{
						command_Toggle.Disable(reason);
					}
					_003C_003E2__current = command_Toggle;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00ea;
			}
			case 1:
				_003C_003E1__state = -1;
				goto IL_00ea;
			case 2:
				_003C_003E1__state = -1;
				goto IL_0143;
			case 3:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0143:
				if (!CS_0024_003C_003E8__locals14.Props.Completable || CS_0024_003C_003E8__locals14.Completed)
				{
					break;
				}
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Complete study",
					action = delegate
					{
						int num2 = 100;
						while (!CS_0024_003C_003E8__locals14.Completed && num2 > 0)
						{
							CS_0024_003C_003E8__locals14.Study(CS_0024_003C_003E8__locals14.parent.Map?.mapPawns?.FreeColonists?.RandomElement(), float.MaxValue);
							num2--;
						}
					}
				};
				_003C_003E1__state = 3;
				return true;
				IL_00ea:
				if (!DebugSettings.ShowDevGizmos)
				{
					break;
				}
				if (CS_0024_003C_003E8__locals14.EverStudiable() && CS_0024_003C_003E8__locals14.TicksTilNextStudy > 0)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: End study cooldown",
						action = delegate
						{
							CS_0024_003C_003E8__locals14.lastStudiedTick = Find.TickManager.TicksGame - CS_0024_003C_003E8__locals14.Props.frequencyTicks;
						}
					};
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_0143;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__48 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__48(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetRelatedQuestHyperlinks_003Ed__49 : IEnumerable<Dialog_InfoCard.Hyperlink>, IEnumerable, IEnumerator<Dialog_InfoCard.Hyperlink>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Dialog_InfoCard.Hyperlink _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompStudiable _003C_003E4__this;

		private List<Quest> _003Cquests_003E5__2;

		private int _003Ci_003E5__3;

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
		public _003CGetRelatedQuestHyperlinks_003Ed__49(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cquests_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			CompStudiable compStudiable = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0101;
			}
			_003C_003E1__state = -1;
			_003Cquests_003E5__2 = Find.QuestManager.QuestsListForReading;
			_003Ci_003E5__3 = 0;
			goto IL_0113;
			IL_0101:
			_003Ci_003E5__3++;
			goto IL_0113;
			IL_0113:
			if (_003Ci_003E5__3 < _003Cquests_003E5__2.Count)
			{
				if (!_003Cquests_003E5__2[_003Ci_003E5__3].hidden && (_003Cquests_003E5__2[_003Ci_003E5__3].State == QuestState.Ongoing || _003Cquests_003E5__2[_003Ci_003E5__3].State == QuestState.NotYetAccepted))
				{
					List<QuestPart> partsListForReading = _003Cquests_003E5__2[_003Ci_003E5__3].PartsListForReading;
					for (int i = 0; i < partsListForReading.Count; i++)
					{
						if (partsListForReading[i] is QuestPart_RequirementsToAcceptThingStudied questPart_RequirementsToAcceptThingStudied && questPart_RequirementsToAcceptThingStudied.thing == compStudiable.parent)
						{
							_003C_003E2__current = new Dialog_InfoCard.Hyperlink(_003Cquests_003E5__2[_003Ci_003E5__3]);
							_003C_003E1__state = 1;
							return true;
						}
					}
				}
				goto IL_0101;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Dialog_InfoCard.Hyperlink> IEnumerable<Dialog_InfoCard.Hyperlink>.GetEnumerator()
		{
			_003CGetRelatedQuestHyperlinks_003Ed__49 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetRelatedQuestHyperlinks_003Ed__49(0)
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
	private sealed class _003CSpecialDisplayStats_003Ed__52 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompStudiable _003C_003E4__this;

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
		public _003CSpecialDisplayStats_003Ed__52(int _003C_003E1__state)
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
				CompStudiable compStudiable = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					IEnumerable<StatDrawEntry> enumerable = ((ThingComp)compStudiable).SpecialDisplayStats();
					if (enumerable != null)
					{
						_003C_003E7__wrap1 = enumerable.GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_0081;
					}
					goto IL_009b;
				}
				case 1:
					_003C_003E1__state = -3;
					goto IL_0081;
				case 2:
					_003C_003E1__state = -1;
					goto IL_011e;
				case 3:
					_003C_003E1__state = -1;
					if (compStudiable.Props.frequencyTicks > 0)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Containment, "StudyFrequency".Translate(), compStudiable.Props.frequencyTicks.ToStringTicksToPeriod(), "StudyFrequencyDesc".Translate(), 2545, null, null, forceUnfinalizedMode: false, overridesHideStats: true);
						_003C_003E1__state = 4;
						return true;
					}
					break;
				case 4:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_011e:
					if (ModsConfig.AnomalyActive && compStudiable.AnomalyKnowledge > 0f && compStudiable.KnowledgeCategory != null && (!(compStudiable.parent is Pawn pawn) || compStudiable.IsMutant || compStudiable.RequiresHoldingPlatform || (compStudiable.RequiresImprisonment && (pawn.Inhumanized() || pawn.kindDef.studiableAsPrisoner))))
					{
						knowledgeSb.Clear();
						float f = compStudiable.AdjustedAnomalyKnowledge(knowledgeSb);
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Containment, "KnowledgeFromStudy".Translate(), "Stat_Knowledge".Translate(f.ToStringDecimalIfSmall(), compStudiable.KnowledgeCategory.label), "KnowledgeFromStudyDesc".Translate() + "\n\n" + knowledgeSb.ToString(), 2550, null, null, forceUnfinalizedMode: false, overridesHideStats: true);
						_003C_003E1__state = 3;
						return true;
					}
					break;
					IL_009b:
					if (compStudiable.Props.Completable)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.BasicsNonPawn, "Study".Translate(), compStudiable.studyPoints + " / " + compStudiable.Props.studyAmountToComplete, "Stat_Studiable_Desc".Translate(), 3000, null, compStudiable.GetRelatedQuestHyperlinks(), forceUnfinalizedMode: false, overridesHideStats: true);
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_011e;
					IL_0081:
					if (_003C_003E7__wrap1.MoveNext())
					{
						StatDrawEntry current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					goto IL_009b;
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
			_003CSpecialDisplayStats_003Ed__52 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__52(0)
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

	private static readonly CachedTexture StudyToggleIcon = new CachedTexture("UI/Icons/Study");

	public int lastStudiedTick = -9999999;

	public bool studyEnabled = true;

	public float studyPoints;

	public float anomalyKnowledgeGained;

	public int studyInteractions;

	private CompStudyUnlocks compStudyUnlocks;

	private CompActivity compActivity;

	private static StringBuilder knowledgeSb = new StringBuilder();

	public CompProperties_Studiable Props => (CompProperties_Studiable)props;

	public Pawn Pawn => parent as Pawn;

	public float ProgressPercent
	{
		get
		{
			if (!Props.Completable)
			{
				return 0f;
			}
			return studyPoints / Props.studyAmountToComplete;
		}
	}

	public float AdjustedAnomalyKnowledgePerStudy => AdjustedAnomalyKnowledge() / 5f;

	public bool Completed
	{
		get
		{
			if (Props.Completable)
			{
				return ProgressPercent >= 1f;
			}
			return false;
		}
	}

	public int TicksTilNextStudy => lastStudiedTick + Props.frequencyTicks - Find.TickManager.TicksGame;

	private bool IsMutant
	{
		get
		{
			if (Pawn != null)
			{
				return Pawn.IsMutant;
			}
			return false;
		}
	}

	public virtual float AnomalyKnowledge
	{
		get
		{
			if (!ModsConfig.AnomalyActive)
			{
				return 0f;
			}
			if (KnowledgeCategory == null)
			{
				return 0f;
			}
			float num = 0f;
			CompStudyUnlocks obj = compStudyUnlocks;
			if (obj != null && obj.StudyKnowledgeAmount.HasValue)
			{
				num = compStudyUnlocks.StudyKnowledgeAmount.Value;
			}
			else if (Props.anomalyKnowledge.HasValue)
			{
				num = Props.anomalyKnowledge.Value;
			}
			else if (Pawn != null)
			{
				num = Pawn.RaceProps.anomalyKnowledge;
			}
			else
			{
				UnityEngine.Debug.LogError($"Thing {parent} has studiable comp but has no overriden anomaly knowledge amount, ensure this is on a pawn with setup race props vars, or provides a knowledge amount value in this comp.");
			}
			CompStudyUnlocks obj2 = compStudyUnlocks;
			if (obj2 != null && obj2.StudyKnowledgeAmount.HasValue)
			{
				num = compStudyUnlocks.StudyKnowledgeAmount.Value;
			}
			if (IsMutant)
			{
				num += Pawn.mutant.Def.anomalyKnowledgeOffset;
			}
			return num;
		}
	}

	public virtual KnowledgeCategoryDef KnowledgeCategory
	{
		get
		{
			if (!ModsConfig.AnomalyActive)
			{
				return null;
			}
			if (IsMutant)
			{
				return Pawn.mutant.Def.knowledgeCategory;
			}
			if (Props.knowledgeCategory != null)
			{
				return Props.knowledgeCategory;
			}
			return Pawn?.RaceProps.knowledgeCategory;
		}
	}

	public bool RequiresHoldingPlatform
	{
		get
		{
			if (!IsMutant)
			{
				return Props.requiresHoldingPlatform;
			}
			return true;
		}
	}

	public bool RequiresImprisonment
	{
		get
		{
			if (!IsMutant)
			{
				return Props.requiresImprisonment;
			}
			return false;
		}
	}

	public bool Deactivated
	{
		get
		{
			if (Props.canBeActivityDeactivated)
			{
				return CompActivity.Deactivated;
			}
			return false;
		}
	}

	private bool DisplayStudyInfoOnInspectPane
	{
		get
		{
			if (parent.Destroyed)
			{
				return false;
			}
			if (Completed)
			{
				return false;
			}
			if (ModsConfig.AnomalyActive && Find.Anomaly.HighestLevelReached < Props.minMonolithLevelForStudy)
			{
				return false;
			}
			return true;
		}
	}

	private CompActivity CompActivity => compActivity ?? (compActivity = parent.GetComp<CompActivity>());

	public override void PostPostMake()
	{
		base.PostPostMake();
		studyEnabled = Props.studyEnabledByDefault;
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		compStudyUnlocks = parent.TryGetComp<CompStudyUnlocks>();
		Find.StudyManager.UpdateStudiableCache(parent, parent.Map ?? (parent.ParentHolder as Thing)?.Map);
	}

	public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
	{
		Find.StudyManager.UpdateStudiableCache(parent, map);
		base.PostDeSpawn(map, mode);
	}

	public void SetStudyEnabled(bool enabled)
	{
		studyEnabled = enabled;
	}

	public bool CurrentlyStudiable()
	{
		if (!EverStudiable(out var _))
		{
			return false;
		}
		if (!studyEnabled)
		{
			return false;
		}
		if (Props.frequencyTicks > 0 && TicksTilNextStudy > 0)
		{
			return false;
		}
		if (parent is Pawn pawn)
		{
			if (RequiresHoldingPlatform)
			{
				CompHoldingPlatformTarget compHoldingPlatformTarget = pawn.TryGetComp<CompHoldingPlatformTarget>();
				if (compHoldingPlatformTarget == null || !compHoldingPlatformTarget.CanStudy)
				{
					return false;
				}
			}
			if (RequiresImprisonment)
			{
				if (pawn.Downed || !pawn.Awake())
				{
					return false;
				}
				if (pawn.guest.IsInteractionDisabled(PrisonerInteractionModeDefOf.Study))
				{
					return false;
				}
			}
		}
		return true;
	}

	public bool EverStudiable()
	{
		string reason;
		return EverStudiable(out reason);
	}

	public bool EverStudiable(out string reason)
	{
		reason = null;
		if (!StudyUnlocked())
		{
			return false;
		}
		if (!EverStudiableCached(out reason))
		{
			return false;
		}
		return true;
	}

	public bool StudyUnlocked()
	{
		if (parent.Destroyed)
		{
			return false;
		}
		if (Deactivated)
		{
			return false;
		}
		if (ModsConfig.AnomalyActive && Find.Anomaly.HighestLevelReached < Props.minMonolithLevelForStudy && Find.Anomaly.GenerateMonolith)
		{
			return false;
		}
		return true;
	}

	public bool EverStudiableCached()
	{
		string reason;
		return EverStudiableCached(out reason);
	}

	public bool EverStudiableCached(out string reason)
	{
		reason = null;
		if (parent == null)
		{
			return false;
		}
		if (parent is Pawn pawn)
		{
			if (RequiresHoldingPlatform && !(parent.ParentHolder is Building_HoldingPlatform))
			{
				reason = "RequiresHoldingPlatform".Translate();
				return false;
			}
			if (RequiresImprisonment && ((!pawn.Inhumanized() && !pawn.kindDef.studiableAsPrisoner) || !pawn.IsPrisonerOfColony))
			{
				reason = "RequiresImprisonment".Translate();
				return false;
			}
		}
		return true;
	}

	public virtual void Study(Pawn studier, float studyAmount, float anomalyKnowledgeAmount = 0f)
	{
		bool completed = Completed;
		studyAmount *= Find.Storyteller.difficulty.researchSpeedFactor;
		studyAmount *= studier.GetStatValue(StatDefOf.ResearchSpeed);
		anomalyKnowledgeGained += anomalyKnowledgeAmount;
		Find.StudyManager.Study(parent, studier, studyAmount);
		if (ModsConfig.AnomalyActive && anomalyKnowledgeAmount > 0f)
		{
			Find.StudyManager.StudyAnomaly(parent, studier, anomalyKnowledgeAmount, KnowledgeCategory);
		}
		studier?.skills.Learn(SkillDefOf.Intellectual, 0.1f);
		if (!completed && Completed)
		{
			QuestUtility.SendQuestTargetSignals(parent.questTags, "Researched", parent.Named("SUBJECT"), studier.Named("STUDIER"));
			if (!Props.completedMessage.NullOrEmpty())
			{
				Messages.Message(Props.completedMessage, parent, MessageTypeDefOf.NeutralEvent);
			}
			if (studier != null && !Props.completedLetterText.NullOrEmpty() && !Props.completedLetterTitle.NullOrEmpty())
			{
				Find.LetterStack.ReceiveLetter(Props.completedLetterTitle.Formatted(studier.Named("STUDIER"), parent.Named("PARENT")), Props.completedLetterText.Formatted(studier.Named("STUDIER"), parent.Named("PARENT")), Props.completedLetterDef ?? LetterDefOf.NeutralEvent, new List<Thing> { parent, studier });
			}
		}
	}

	public void Notify_ActivityDeactivated()
	{
		if (Props.canBeActivityDeactivated)
		{
			studyEnabled = false;
		}
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__48))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__48(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CGetRelatedQuestHyperlinks_003Ed__49))]
	private IEnumerable<Dialog_InfoCard.Hyperlink> GetRelatedQuestHyperlinks()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetRelatedQuestHyperlinks_003Ed__49(-2)
		{
			_003C_003E4__this = this
		};
	}

	private float AdjustedAnomalyKnowledge(StringBuilder sb = null)
	{
		if (!ModsConfig.AnomalyActive)
		{
			return 0f;
		}
		float num = AnomalyKnowledge;
		sb?.AppendLine("StatsReport_BaseValue".Translate() + ": " + num.ToStringDecimalIfSmall());
		bool flag = false;
		if (parent.TryGetComp<CompHoldingPlatformTarget>(out var comp) && comp.CurrentlyHeldOnPlatform)
		{
			CompEntityHolder comp2 = comp.HeldPlatform.GetComp<CompEntityHolder>();
			float studyKnowledgeAmountMultiplier = ContainmentUtility.GetStudyKnowledgeAmountMultiplier(parent, comp2);
			if (!Mathf.Approximately(studyKnowledgeAmountMultiplier, 1f))
			{
				if (!flag)
				{
					sb?.AppendLine();
					flag = true;
				}
				num *= studyKnowledgeAmountMultiplier;
				sb?.AppendLine("FactorContainmentStrength".Translate() + " (" + StatDefOf.ContainmentStrength.Worker.ValueToString(comp2.ContainmentStrength, finalized: true) + ")" + ": x" + studyKnowledgeAmountMultiplier.ToStringPercent());
			}
			if (comp.HeldPlatform.HasAttachedElectroharvester)
			{
				if (!flag)
				{
					sb?.AppendLine();
					flag = true;
				}
				num *= 0.5f;
				sb?.AppendLine(ThingDefOf.Electroharvester.LabelCap + ": x" + 0.5f.ToStringPercent());
			}
		}
		if (parent.TryGetComp<CompActivity>(out var comp3))
		{
			if (!flag)
			{
				sb?.AppendLine();
				flag = true;
			}
			num *= comp3.ActivityResearchFactor;
			sb?.AppendLine("FactorActivity".Translate() + ": x" + comp3.ActivityResearchFactor.ToStringPercent());
		}
		if (!Mathf.Approximately(Props.knowledgeFactorOutdoors, 1f) && parent.IsOutside())
		{
			if (!flag)
			{
				sb?.AppendLine();
				flag = true;
			}
			num *= Props.knowledgeFactorOutdoors;
			sb?.AppendLine("Outdoors".Translate().CapitalizeFirst() + ": x" + Props.knowledgeFactorOutdoors.ToStringPercent());
		}
		if (!Mathf.Approximately(Find.Storyteller.difficulty.studyEfficiencyFactor, 1f))
		{
			if (!flag)
			{
				sb?.AppendLine();
				flag = true;
			}
			num *= Find.Storyteller.difficulty.studyEfficiencyFactor;
			sb?.AppendLine("DifficultyLevel".Translate() + ": x" + Find.Storyteller.difficulty.studyEfficiencyFactor.ToStringPercent());
		}
		sb?.AppendLine();
		sb?.AppendLine("StatsReport_FinalValue".Translate() + ": " + num.ToStringDecimalIfSmall());
		return num;
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__52))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__52(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void PostExposeData()
	{
		Scribe_Values.Look(ref lastStudiedTick, "lastStudiedTick", 0);
		Scribe_Values.Look(ref studyEnabled, "studyEnabled", defaultValue: true);
		Scribe_Values.Look(ref studyPoints, "studiedAmount", 0f);
		Scribe_Values.Look(ref anomalyKnowledgeGained, "anomalyKnowledgeGained", 0f);
		Scribe_Values.Look(ref studyInteractions, "studyInteractions", 0);
		if (Scribe.mode == LoadSaveMode.LoadingVars)
		{
			float value = 0f;
			Scribe_Values.Look(ref value, "progress", 0f);
			if (value > 0f)
			{
				float num = value / Props.studyAmountToComplete;
				studyPoints = Props.studyAmountToComplete * num;
			}
		}
		if (Scribe.mode == LoadSaveMode.PostLoadInit && Find.StudyManager.backCompatStudyProgress.TryGetValue(parent.def, out var value2))
		{
			studyPoints = Props.studyAmountToComplete * value2;
		}
	}

	public override string CompInspectStringExtra()
	{
		string text = string.Empty;
		if (!EverStudiable())
		{
			return text;
		}
		if (Props.Completable)
		{
			text = string.Concat(text, "StudyProgress".Translate() + ": " + studyPoints.ToString("F1") + " / ", Props.studyAmountToComplete.ToString());
			if (Completed)
			{
				text += " (" + "StudyCompleted".Translate() + ")";
			}
		}
		else if (!RequiresHoldingPlatform && !(parent is Pawn))
		{
			string inspectStringExtraFor = GetInspectStringExtraFor(parent);
			if (!inspectStringExtraFor.NullOrEmpty())
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += inspectStringExtraFor;
			}
		}
		return text;
	}

	public static string GetInspectStringExtraFor(Thing thing)
	{
		CompStudiable compStudiable = thing.TryGetComp<CompStudiable>();
		if (compStudiable != null && compStudiable.DisplayStudyInfoOnInspectPane && compStudiable.EverStudiable())
		{
			TaggedString empty = TaggedString.Empty;
			if (ModsConfig.AnomalyActive && compStudiable.AnomalyKnowledge > 0f && compStudiable.KnowledgeCategory != null)
			{
				empty += "KnowledgeCategory".Translate() + ": " + compStudiable.KnowledgeCategory.LabelCap;
				if (compStudiable.TicksTilNextStudy <= 0)
				{
					empty += $" ({"ReadyToStudy".Translate()})";
				}
				else if (compStudiable.TicksTilNextStudy > 0)
				{
					empty += $" ({"CanBeStudiedInDuration".Translate(compStudiable.TicksTilNextStudy.ToStringTicksToPeriod())})";
				}
				if (compStudiable.Props.knowledgeFactorOutdoors != 1f && compStudiable.parent.IsOutside())
				{
					empty += $"\n{"KnowledgeFactorOutdoors".Translate() + ": x" + compStudiable.Props.knowledgeFactorOutdoors.ToStringPercent()} ({"Outdoors".Translate()})";
				}
			}
			else if (compStudiable.TicksTilNextStudy <= 0)
			{
				empty += "ReadyToStudy".Translate().CapitalizeFirst();
			}
			else
			{
				empty += "CanBeStudiedInDuration".Translate(compStudiable.TicksTilNextStudy.ToStringTicksToPeriod());
			}
			return empty.Resolve();
		}
		return null;
	}
}
