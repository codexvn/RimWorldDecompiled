using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld;

public class LordJob_Ritual : LordJob_Joinable_Gathering
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass105_0
	{
		public LordJob_Ritual _003C_003E4__this;

		public Pawn p;

		internal void _003CGetPawnGizmos_003Eb__0()
		{
			_003C_003E4__this.pawnsForcedToLeave.Add(p);
			_003C_003E4__this.lord.Notify_PawnLost(p, PawnLostCondition.ForcedByPlayerAction, null);
			p.jobs?.EndCurrentJob(JobCondition.InterruptForced);
			SoundDefOf.Tick_Low.PlayOneShotOnCamera();
		}
	}

	[CompilerGenerated]
	private sealed class _003CAllSecondFoci_003Ed__118 : IEnumerable<TargetInfo>, IEnumerable, IEnumerator<TargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private TargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public LordJob_Ritual _003C_003E4__this;

		private int index;

		public int _003C_003E3__index;

		private List<RitualStagePawnSecondFocus>.Enumerator _003C_003E7__wrap1;

		TargetInfo IEnumerator<TargetInfo>.Current
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
		public _003CAllSecondFoci_003Ed__118(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<RitualStagePawnSecondFocus>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				LordJob_Ritual lordJob_Ritual = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E2__current = lordJob_Ritual.stageSecondFocus[index];
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = lordJob_Ritual.stagePawnSecondFocus.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 2:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					RitualStagePawnSecondFocus current = _003C_003E7__wrap1.Current;
					if (current.stageIndex == index)
					{
						_003C_003E2__current = current.target;
						_003C_003E1__state = 2;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<RitualStagePawnSecondFocus>.Enumerator);
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<TargetInfo> IEnumerable<TargetInfo>.GetEnumerator()
		{
			_003CAllSecondFoci_003Ed__118 _003CAllSecondFoci_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAllSecondFoci_003Ed__ = this;
			}
			else
			{
				_003CAllSecondFoci_003Ed__ = new _003CAllSecondFoci_003Ed__118(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CAllSecondFoci_003Ed__.index = _003C_003E3__index;
			return _003CAllSecondFoci_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TargetInfo>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CCallOffTriggers_003Ed__83 : IEnumerable<Trigger>, IEnumerable, IEnumerator<Trigger>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Trigger _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public LordJob_Ritual _003C_003E4__this;

		private List<RitualCancellationTrigger>.Enumerator _003C_003E7__wrap1;

		private IEnumerator<Trigger> _003C_003E7__wrap2;

		Trigger IEnumerator<Trigger>.Current
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
		public _003CCallOffTriggers_003Ed__83(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || num == 4)
			{
				try
				{
					if (num == -4 || num == 4)
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
			_003C_003E7__wrap1 = default(List<RitualCancellationTrigger>.Enumerator);
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				LordJob_Ritual lordJob_Ritual = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Trigger_TickCondition(lordJob_Ritual.ShouldBeCalledOff);
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Trigger_PawnKilled(lordJob_Ritual.pawnsDeathIgnored);
					_003C_003E1__state = 2;
					return true;
				case 2:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Trigger_Signal(lordJob_Ritual.CallOffSignal);
					_003C_003E1__state = 3;
					return true;
				case 3:
					_003C_003E1__state = -1;
					if (lordJob_Ritual.ritual.behavior.def.cancellationTriggers.NullOrEmpty())
					{
						break;
					}
					_003C_003E7__wrap1 = lordJob_Ritual.ritual.behavior.def.cancellationTriggers.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0168;
				case 4:
					{
						_003C_003E1__state = -4;
						goto IL_014e;
					}
					IL_0168:
					if (_003C_003E7__wrap1.MoveNext())
					{
						RitualCancellationTrigger current = _003C_003E7__wrap1.Current;
						_003C_003E7__wrap2 = current.CancellationTriggers(lordJob_Ritual.assignments).GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_014e;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<RitualCancellationTrigger>.Enumerator);
					break;
					IL_014e:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Trigger current2 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 4;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
					goto IL_0168;
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
		IEnumerator<Trigger> IEnumerable<Trigger>.GetEnumerator()
		{
			_003CCallOffTriggers_003Ed__83 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCallOffTriggers_003Ed__83(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Trigger>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetPawnGizmos_003Ed__105 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public LordJob_Ritual _003C_003E4__this;

		private Pawn p;

		public Pawn _003C_003E3__p;

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
		public _003CGetPawnGizmos_003Ed__105(int _003C_003E1__state)
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
			LordJob_Ritual lordJob_Ritual = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass105_0 CS_0024_003C_003E8__locals8 = new _003C_003Ec__DisplayClass105_0
				{
					_003C_003E4__this = _003C_003E4__this,
					p = p
				};
				if (CS_0024_003C_003E8__locals8.p != lordJob_Ritual.organizer && !lordJob_Ritual.assignments.ExtraRequiredPawnsForReading.Contains(CS_0024_003C_003E8__locals8.p) && (!lordJob_Ritual.TryGetRoleFor(CS_0024_003C_003E8__locals8.p, out var role) || !role.required))
				{
					Command_Action command_Action = new Command_Action
					{
						defaultLabel = "CommandLeaveRitual".Translate(lordJob_Ritual.ritual.Named("RITUAL")),
						defaultDesc = "CommandLeaveRitualDesc".Translate(lordJob_Ritual.ritual.Named("RITUAL")),
						icon = lordJob_Ritual.ritual.CancelIcon
					};
					if (!lordJob_Ritual.ritual.def.mergeRitualGizmosFromAllIdeos)
					{
						command_Action.defaultIconColor = lordJob_Ritual.ritual.ideo.Color;
					}
					command_Action.action = delegate
					{
						CS_0024_003C_003E8__locals8._003C_003E4__this.pawnsForcedToLeave.Add(CS_0024_003C_003E8__locals8.p);
						CS_0024_003C_003E8__locals8._003C_003E4__this.lord.Notify_PawnLost(CS_0024_003C_003E8__locals8.p, PawnLostCondition.ForcedByPlayerAction, null);
						CS_0024_003C_003E8__locals8.p.jobs?.EndCurrentJob(JobCondition.InterruptForced);
						SoundDefOf.Tick_Low.PlayOneShotOnCamera();
					};
					if (lordJob_Ritual.lord.ownedPawns.Count < 2)
					{
						command_Action.Disable("CommandLeaveLastParticipant".Translate(lordJob_Ritual.ritual.Named("RITUAL")));
					}
					command_Action.hotKey = KeyBindingDefOf.Misc5;
					_003C_003E2__current = command_Action;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_018a;
			}
			case 1:
				_003C_003E1__state = -1;
				goto IL_018a;
			case 2:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_018a:
				_003C_003E2__current = lordJob_Ritual.GetCancelGizmo();
				_003C_003E1__state = 2;
				return true;
			}
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
			_003CGetPawnGizmos_003Ed__105 _003CGetPawnGizmos_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetPawnGizmos_003Ed__ = this;
			}
			else
			{
				_003CGetPawnGizmos_003Ed__ = new _003CGetPawnGizmos_003Ed__105(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetPawnGizmos_003Ed__.p = _003C_003E3__p;
			return _003CGetPawnGizmos_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	protected Precept_Ritual ritual;

	public RitualObligation obligation;

	public TargetInfo selectedTarget;

	public RitualRoleAssignments assignments;

	private List<RitualStage> stages;

	public List<Pawn> pawnsDeathIgnored = new List<Pawn>();

	protected HashSet<Pawn> pawnsForcedToLeave = new HashSet<Pawn>();

	public List<Thing> usedThings = new List<Thing>();

	public Dictionary<Pawn, PawnTags> perPawnTags = new Dictionary<Pawn, PawnTags>();

	protected SoundDef ambienceDef;

	protected int stageTicks;

	protected int ticksPassed;

	protected int stageIndex;

	protected float ticksPassedWithProgress;

	public float? progressBarOverride;

	protected bool ended;

	public bool repeatPenalty;

	private List<TargetInfo> stageSecondFocus;

	private List<RitualStagePawnSecondFocus> stagePawnSecondFocus;

	private List<RitualStagePositions> ritualStagePositions;

	private List<RitualStageOnTickActions> ritualStageOnTickActions;

	private List<RitualVisualEffect> effectWorkers = new List<RitualVisualEffect>();

	private List<RitualVisualEffect> effectWorkersCurrentStage = new List<RitualVisualEffect>();

	private int lastEssentialStageIndex = -1;

	private int lastEssentialStageEndedTick = -1;

	private int ticksSinceLastEssentialStage;

	private bool ignoreDurationToFinish;

	protected Effecter progressBar;

	protected Sustainer ambiencePlaying;

	protected List<LordToil_Ritual> toils = new List<LordToil_Ritual>();

	public bool cancelled;

	private bool initedVisualEffects;

	private IntVec2 roomBoundsCached = IntVec2.Invalid;

	private Dictionary<IntVec3, Mote> highlightedPositions = new Dictionary<IntVec3, Mote>();

	private Dictionary<Pawn, Mote> highlightedPawns = new Dictionary<Pawn, Mote>();

	public const string RitualStartedSignal = "RitualStarted";

	public static readonly IntVec2 DefaultRitualVfxScale = new IntVec2(28, 28);

	private static readonly List<Pawn> tmpParticipants = new List<Pawn>(8);

	private Dictionary<Pawn, int> totalPresenceTmp = new Dictionary<Pawn, int>();

	private static HashSet<Pawn> reservers = new HashSet<Pawn>();

	private List<Pawn> tmpTagPawns;

	private List<Pawn> tmpSubRolePawns;

	private List<Pawn> tmpForcedRolePawns;

	private List<PawnTags> tmpTags;

	private List<string> tmpForcedRoleIds;

	private List<string> tmpSubRoleIds;

	public override bool AllowStartNewGatherings => false;

	public Precept_Ritual Ritual => ritual;

	public virtual string RitualLabel
	{
		get
		{
			if (ritual == null)
			{
				return "Ritual".Translate().Resolve();
			}
			return ritual.LabelCap;
		}
	}

	protected string CallOffSignal => lord.GetUniqueLoadID() + ".callOffRitual";

	protected string CancelSignal => lord.GetUniqueLoadID() + ".cancelRitual";

	public override int TicksLeft => (int)((float)durationTicks - ticksPassedWithProgress);

	public float Progress => ticksPassedWithProgress / (float)durationTicks;

	public float TicksPassedWithProgress => ticksPassedWithProgress;

	public int StageIndex => stageIndex;

	public override bool DontInterruptLayingPawnsOnCleanup => true;

	public RitualStage CurrentStage
	{
		get
		{
			if (StageIndex < 0 || stages.NullOrEmpty() || StageIndex >= stages.Count)
			{
				return null;
			}
			return stages[StageIndex];
		}
	}

	protected string TimeLeftPostfix
	{
		get
		{
			if (durationTicks <= 0)
			{
				return "";
			}
			return " (" + "RitualEndsIn".Translate(TicksLeft.ToStringTicksToPeriod()).Resolve() + ")";
		}
	}

	protected virtual int MinTicksToFinish
	{
		get
		{
			if (ignoreDurationToFinish)
			{
				return -1;
			}
			if (lastEssentialStageIndex != -1)
			{
				return lastEssentialStageEndedTick + 1;
			}
			return durationTicks;
		}
	}

	public Room GetRoom => spot.GetRoom(base.Map);

	public IntVec2 RoomBoundsCached
	{
		get
		{
			if (roomBoundsCached.IsInvalid)
			{
				new IntVec2(int.MaxValue, int.MaxValue);
				new IntVec2(int.MinValue, int.MinValue);
				Room getRoom = GetRoom;
				if (getRoom == null || getRoom.PsychologicallyOutdoors || !getRoom.ProperRoom)
				{
					roomBoundsCached = IntVec2.Invalid;
				}
				else
				{
					roomBoundsCached = new IntVec2(getRoom.ExtentsClose.Width, getRoom.ExtentsClose.Height);
				}
			}
			return roomBoundsCached;
		}
	}

	public Sustainer AmbiencePlaying
	{
		get
		{
			Sustainer soundPlaying = ambiencePlaying;
			if (soundPlaying == null)
			{
				Precept_Ritual precept_Ritual = ritual;
				if (precept_Ritual == null)
				{
					return null;
				}
				RitualBehaviorWorker behavior = precept_Ritual.behavior;
				if (behavior == null)
				{
					return null;
				}
				soundPlaying = behavior.SoundPlaying;
			}
			return soundPlaying;
		}
	}

	public virtual IEnumerable<Pawn> PawnsToCountTowardsPresence => lord.ownedPawns;

	public override AcceptanceReport AllowsDrafting(Pawn pawn)
	{
		return new AcceptanceReport("ParticipatingInRitual".Translate(pawn, RitualLabel));
	}

	public LordJob_Ritual()
	{
	}

	public LordJob_Ritual(TargetInfo selectedTarget, Precept_Ritual ritual, RitualObligation obligation, List<RitualStage> allStages, RitualRoleAssignments assignments, Pawn organizer = null, IntVec3? spotOverride = null)
	{
		spot = spotOverride ?? selectedTarget.CenterCell;
		this.selectedTarget = selectedTarget;
		this.ritual = ritual;
		this.obligation = obligation;
		stages = allStages;
		base.organizer = organizer;
		this.assignments = assignments;
		durationTicks = ritual.behavior.def.durationTicks.RandomInRange;
		repeatPenalty = ritual.RepeatPenaltyActive;
	}

	public override bool ShouldRemovePawn(Pawn p, PawnLostCondition reason)
	{
		if (reason == PawnLostCondition.Incapped)
		{
			RitualRoleAssignments ritualRoleAssignments = assignments;
			if (ritualRoleAssignments != null && ritualRoleAssignments.RoleForPawn(p)?.allowDowned == true)
			{
				return false;
			}
		}
		return base.ShouldRemovePawn(p, reason);
	}

	public override bool EndPawnJobOnCleanup(Pawn p)
	{
		return assignments?.RoleForPawn(p)?.endJobOnRitualCleanup ?? base.EndPawnJobOnCleanup(p);
	}

	public void PreparePawns()
	{
		tmpParticipants.Clear();
		tmpParticipants.AddRange(assignments.Participants);
		foreach (Pawn tmpParticipant in tmpParticipants)
		{
			if (tmpParticipant.drafter != null)
			{
				tmpParticipant.drafter.Drafted = false;
			}
			if (!tmpParticipant.Awake())
			{
				RestUtility.WakeUp(tmpParticipant);
			}
		}
	}

	public override AcceptanceReport AllowsFloatMenu(Pawn pawn)
	{
		return new AcceptanceReport("ParticipatingInRitual".Translate(pawn, RitualLabel));
	}

	public override bool BlocksSocialInteraction(Pawn pawn)
	{
		return RoleFor(pawn, includeForced: true)?.blocksSocialInteraction ?? false;
	}

	public override bool DutyActiveWhenDown(Pawn pawn)
	{
		RitualRole ritualRole = assignments?.RoleForPawn(pawn);
		if (ritualRole != null && ritualRole.allowDowned)
		{
			return true;
		}
		return base.DutyActiveWhenDown(pawn);
	}

	public override bool PrisonerSecure(Pawn pawn)
	{
		return true;
	}

	public override AcceptanceReport AbilityAllowed(Ability ability)
	{
		return new AcceptanceReport("AbilityDisabledInRitual".Translate(ability.pawn, RitualLabel));
	}

	public override void Notify_AddedToLord()
	{
		stageSecondFocus = new List<TargetInfo>();
		stagePawnSecondFocus = new List<RitualStagePawnSecondFocus>();
		ritualStagePositions = new List<RitualStagePositions>();
		ritualStageOnTickActions = new List<RitualStageOnTickActions>();
		if (stages != null)
		{
			for (int i = 0; i < stages.Count; i++)
			{
				RitualStage ritualStage = stages[i];
				if (ritualStage.essential && ritualStage != stages.Last())
				{
					lastEssentialStageIndex = i;
				}
				stageSecondFocus.Add(ritualStage.GetSecondFocus(this));
				IEnumerable<RitualStagePawnSecondFocus> pawnSecondFoci = ritualStage.GetPawnSecondFoci(this);
				if (pawnSecondFoci != null)
				{
					foreach (RitualStagePawnSecondFocus item in pawnSecondFoci)
					{
						item.stageIndex = i;
						stagePawnSecondFocus.Add(item);
					}
				}
				ritualStagePositions.Add(new RitualStagePositions());
				foreach (Pawn participant in assignments.Participants)
				{
					PawnStagePosition pawnPosition = ritualStage.GetPawnPosition(spot, participant, this);
					ritualStagePositions[i].referencePositions.Add(new PawnRitualReference(participant), pawnPosition);
				}
				List<ActionOnTick> list = new List<ActionOnTick>();
				if (ritualStage.tickActionMaker != null)
				{
					list.AddRange(ritualStage.tickActionMaker.GenerateTimedActions(this, ritualStage));
				}
				ritualStageOnTickActions.Add(new RitualStageOnTickActions
				{
					actions = list
				});
			}
		}
		if (ritual != null)
		{
			Ideo ideo = ritual.ideo;
			if (ritual.behavior != null && assignments != null && !ritual.behavior.def.useVisualEffectsFromRoleIdIdeo.NullOrEmpty())
			{
				Pawn pawn = assignments.FirstAssignedPawn(ritual.behavior.def.useVisualEffectsFromRoleIdIdeo);
				if (pawn != null)
				{
					ideo = pawn.Ideo;
				}
			}
			if (ideo != null && ritual.playsIdeoMusic)
			{
				ambienceDef = ideo.SoundOngoingRitual;
			}
			if (ritual.def.usesIdeoVisualEffects && ideo != null && ideo.RitualEffect != null)
			{
				RitualVisualEffect instance = ideo.RitualEffect.GetInstance();
				instance.Setup(this, loading: false);
				effectWorkers.Add(instance);
			}
		}
		bool flag = GetRoom != null && !GetRoom.PsychologicallyOutdoors && GetRoom.ProperRoom;
		lord.lordManager.stencilDrawers.Add(new StencilDrawerForCells
		{
			sourceLord = lord,
			cells = (flag ? GetRoom.Cells.ToList() : null),
			center = selectedTarget.CenterVector3,
			dimensionsIfNoCells = DefaultRitualVfxScale,
			ticksLeftWithoutLord = 60
		});
		initedVisualEffects = true;
	}

	public override StateGraph CreateGraph()
	{
		StateGraph stateGraph = new StateGraph();
		toils.Clear();
		foreach (RitualStage stage2 in stages)
		{
			LordToil_Ritual lordToil_Ritual = MakeToil(stage2);
			stateGraph.AddToil(lordToil_Ritual);
			toils.Add(lordToil_Ritual);
		}
		LordToil_End lordToil_End = new LordToil_End();
		stateGraph.AddToil(lordToil_End);
		for (int i = 0; i < stages.Count; i++)
		{
			LordToil_Ritual lordToil_Ritual2 = toils[i];
			RitualStage stage = stages[i];
			int iCapture = i;
			lordToil_Ritual2.startAction = delegate
			{
				ignoreDurationToFinish |= stage.ignoreDurationToFinishAfterStage;
				stageTicks = 0;
				Find.SignalManager.SendSignal(new Signal("RitualStarted"));
				Dictionary<IntVec3, Mote> existingPosHighlights = new Dictionary<IntVec3, Mote>();
				Dictionary<Pawn, Mote> existingPawnHighlights = new Dictionary<Pawn, Mote>();
				foreach (KeyValuePair<IntVec3, Mote> highlightedPosition in highlightedPositions)
				{
					existingPosHighlights.Add(highlightedPosition.Key, highlightedPosition.Value);
				}
				foreach (KeyValuePair<Pawn, Mote> highlightedPawn in highlightedPawns)
				{
					existingPawnHighlights.Add(highlightedPawn.Key, highlightedPawn.Value);
				}
				highlightedPositions.Clear();
				highlightedPawns.Clear();
				foreach (Pawn participant in assignments.Participants)
				{
					RitualRole ritualRole = RoleFor(participant, includeForced: true);
					if (ritualRole != null && stage.highlightRolePositions.Contains(ritualRole.id))
					{
						TryAddPosHighlight(participant.Position);
					}
					if (ritualRole != null && stage.highlightRolePawns.Contains(ritualRole.id))
					{
						TryAddPawnHighlight(participant);
					}
				}
				foreach (KeyValuePair<PawnRitualReference, PawnStagePosition> referencePosition in ritualStagePositions[iCapture].referencePositions)
				{
					if (referencePosition.Value.highlight)
					{
						TryAddPosHighlight(referencePosition.Value.cell);
					}
				}
				foreach (RitualVisualEffect item in effectWorkersCurrentStage)
				{
					item.Cleanup();
				}
				effectWorkersCurrentStage.Clear();
				if (stage.visualEffectDef != null)
				{
					RitualVisualEffect instance = stage.visualEffectDef.GetInstance();
					instance.Setup(this, loading: false);
					effectWorkersCurrentStage.Add(instance);
				}
				void TryAddPawnHighlight(Pawn pawn)
				{
					if (!existingPawnHighlights.ContainsKey(pawn))
					{
						AddPawnHighlight(pawn);
					}
					else
					{
						highlightedPawns.Add(pawn, existingPawnHighlights[pawn]);
					}
				}
				void TryAddPosHighlight(IntVec3 cell)
				{
					if (!existingPosHighlights.ContainsKey(cell))
					{
						AddPositionHighlight(cell);
					}
					else
					{
						highlightedPositions.Add(cell, existingPosHighlights[cell]);
					}
				}
			};
			if (stage.endTriggers.Any((StageEndTrigger e) => e.CountsTowardsProgress))
			{
				lordToil_Ritual2.tickAction = delegate
				{
					ticksPassedWithProgress += stage.ProgressPerTick(this);
				};
			}
			Transition transition = new Transition(lordToil_Ritual2, lordToil_End);
			foreach (Trigger item2 in CallOffTriggers())
			{
				transition.AddTrigger(item2);
			}
			if (organizer != null)
			{
				transition.AddTrigger(new Trigger_PawnLost(PawnLostCondition.LeftVoluntarily, organizer));
			}
			transition.AddPreAction(new TransitionAction_Custom((Action)delegate
			{
				ApplyOutcome(Progress());
			}));
			transition.AddPreAction(new TransitionAction_Custom((Action)delegate
			{
				stage.interruptedAction?.Apply(this);
			}));
			stateGraph.AddTransition(transition);
			Transition transition2 = new Transition(lordToil_Ritual2, lordToil_End);
			transition2.AddTrigger(new Trigger_Signal(CancelSignal));
			transition2.AddPreAction(new TransitionAction_Custom((Action)delegate
			{
				ApplyOutcome(0f, showFinishedMessage: false, showFailedMessage: true, cancelled: true);
			}));
			transition2.AddPreAction(new TransitionAction_Custom((Action)delegate
			{
				stage.interruptedAction?.Apply(this);
			}));
			stateGraph.AddTransition(transition2);
			if (!stage.failTriggers.NullOrEmpty())
			{
				foreach (StageFailTrigger f in stage.failTriggers)
				{
					int i2 = i;
					Transition transition3 = new Transition(lordToil_Ritual2, lordToil_End);
					transition3.AddTrigger(new Trigger_TickCondition(() => ticksPassed > f.allowanceTicks && f.Failed(this, selectedTarget, SecondFocusForStage(i2))));
					transition3.AddPreAction(new TransitionAction_Message("RitualCalledOff".Translate(ritual.Label) + " " + "Reason".Translate() + ": " + f.Reason(this, selectedTarget).CapitalizeFirst(), MessageTypeDefOf.NegativeEvent, selectedTarget));
					transition3.AddPreAction(new TransitionAction_Custom((Action)delegate
					{
						ApplyOutcome(Progress(), showFinishedMessage: false, showFailedMessage: false, cancelled: true);
					}));
					stateGraph.AddTransition(transition3);
				}
			}
			List<Trigger> list = new List<Trigger>();
			foreach (StageEndTrigger endTrigger in stage.endTriggers)
			{
				list.Add(endTrigger.MakeTrigger(this, selectedTarget, AllSecondFoci(i), stage));
			}
			bool flag = i == stages.Count - 1;
			Transition transition4 = new Transition(lordToil_Ritual2, flag ? ((LordToil)lordToil_End) : ((LordToil)toils[i + 1]));
			foreach (Trigger item3 in list)
			{
				transition4.AddTrigger(item3);
			}
			transition4.AddPreAction(new TransitionAction_Custom((Action)delegate
			{
				stage.preAction?.Apply(this);
			}));
			int i3 = i;
			transition4.AddPostAction(new TransitionAction_Custom((Action)delegate
			{
				stage.postAction?.Apply(this);
				if (i3 == lastEssentialStageIndex)
				{
					lastEssentialStageEndedTick = Mathf.FloorToInt(ticksPassedWithProgress);
				}
				stageIndex++;
			}));
			if (flag)
			{
				transition4.AddPreAction(new TransitionAction_Custom((Action)delegate
				{
					ApplyOutcome(1f);
				}));
			}
			stateGraph.AddTransition(transition4);
		}
		return stateGraph;
		float Progress()
		{
			float value = (float)ticksPassed / (float)durationTicks;
			if (lastEssentialStageIndex != -1)
			{
				int num = base.DurationTicks - lastEssentialStageEndedTick;
				value = (float)ticksSinceLastEssentialStage / (float)num;
			}
			return Mathf.Clamp01(value);
		}
	}

	[IteratorStateMachine(typeof(_003CCallOffTriggers_003Ed__83))]
	protected virtual IEnumerable<Trigger> CallOffTriggers()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCallOffTriggers_003Ed__83(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string GetJobReport(Pawn pawn)
	{
		return BehaviorFor(pawn, includeForced: true)?.GetJobReportOverride(pawn);
	}

	public override string GetReport(Pawn pawn)
	{
		return "LordReportAttending".Translate((ritual != null) ? ritual.Label : gatheringDef.label) + TimeLeftPostfix;
	}

	protected virtual LordToil_Ritual MakeToil(RitualStage stage)
	{
		return new LordToil_Ritual(spot, this, stage, organizer);
	}

	protected virtual bool RitualFinished(float progress, bool cancelled)
	{
		if (ticksPassedWithProgress >= (float)MinTicksToFinish && (lastEssentialStageIndex == -1 || lastEssentialStageEndedTick != -1))
		{
			return !cancelled;
		}
		return false;
	}

	public virtual void ApplyOutcome(float progress, bool showFinishedMessage = true, bool showFailedMessage = true, bool cancelled = false)
	{
		if (ended)
		{
			return;
		}
		ended = true;
		this.cancelled = cancelled;
		if (RitualFinished(progress, cancelled))
		{
			totalPresenceTmp.Clear();
			foreach (LordToil_Ritual toil in toils)
			{
				foreach (KeyValuePair<Pawn, int> presentForTick in toil.Data.presentForTicks)
				{
					if (presentForTick.Key != null && !presentForTick.Key.Dead)
					{
						if (!totalPresenceTmp.ContainsKey(presentForTick.Key))
						{
							totalPresenceTmp.Add(presentForTick.Key, presentForTick.Value);
						}
						else
						{
							totalPresenceTmp[presentForTick.Key] += presentForTick.Value;
						}
					}
				}
			}
			float tickScale = ticksPassedWithProgress / (float)ticksPassed;
			float targetDuration = ((durationTicks > 0) ? ((float)durationTicks) : ticksPassedWithProgress);
			totalPresenceTmp.RemoveAll((KeyValuePair<Pawn, int> tp) => targetDuration * (float)tp.Value < tickScale / 2f);
			if (totalPresenceTmp.Count > 0 || ritual.outcomeEffect.def.allowOutcomeWithNoAttendance)
			{
				AddParticipantThoughts();
				try
				{
					ritual.outcomeEffect.Apply(progress, totalPresenceTmp, this);
				}
				catch (Exception ex)
				{
					Log.Error("Error while applying ritual outcome effect: " + ex);
				}
				if (obligation != null)
				{
					ritual.RemoveObligation(obligation, completed: true);
				}
				if (showFinishedMessage && ritual.behavior.def.displayCompletedMessage)
				{
					Messages.Message("RitualFinished".Translate(ritual.Label), new TargetInfo(spot, base.Map), MessageTypeDefOf.SilentInput);
				}
			}
			else
			{
				Messages.Message("RitualNobodyAttended".Translate(ritual.Label), new TargetInfo(spot, base.Map), MessageTypeDefOf.NegativeEvent);
			}
			totalPresenceTmp.Clear();
			if (Ritual != null && Ritual.ideo != null)
			{
				foreach (Precept item in Ritual.ideo.PreceptsListForReading)
				{
					if (!(item is Precept_Ritual precept_Ritual) || precept_Ritual.obligationTriggers.NullOrEmpty())
					{
						continue;
					}
					foreach (RitualObligationTrigger obligationTrigger in precept_Ritual.obligationTriggers)
					{
						obligationTrigger.Notify_RitualExecuted(this);
					}
				}
			}
			ritual.lastFinishedTick = GenTicks.TicksGame;
		}
		else
		{
			if (showFailedMessage)
			{
				Messages.Message("RitualCalledOff".Translate(ritual.Label).CapitalizeFirst(), new TargetInfo(spot, base.Map), MessageTypeDefOf.NegativeEvent);
			}
			try
			{
				if (ritual.outcomeEffect.ApplyOnFailure)
				{
					ritual.outcomeEffect.Apply(progress, totalPresenceTmp, this);
				}
			}
			catch (Exception ex2)
			{
				Log.Error("Error while applying ritual outcome effect: " + ex2);
			}
		}
		ritual.outcomeEffect?.ResetCompDatas();
		base.Map.lordManager.RemoveLord(lord);
	}

	private void AddRelicInRoomThought()
	{
		if (!ModsConfig.IdeologyActive || Ritual?.ideo == null || selectedTarget.Map == null)
		{
			return;
		}
		Room room = selectedTarget.Cell.GetRoom(selectedTarget.Map);
		if (room == null || room.TouchesMapEdge)
		{
			return;
		}
		int num = 0;
		string str = string.Empty;
		foreach (Thing item in room.ContainedThings(ThingDefOf.Reliquary))
		{
			CompRelicContainer compRelicContainer = item.TryGetComp<CompRelicContainer>();
			if (compRelicContainer == null)
			{
				continue;
			}
			Precept_ThingStyle precept_ThingStyle = (compRelicContainer.ContainedThing as ThingWithComps)?.compStyleable?.SourcePrecept;
			if (precept_ThingStyle != null && precept_ThingStyle.ideo == Ritual.ideo)
			{
				if (num == 0)
				{
					str = compRelicContainer.ContainedThing.Label;
				}
				num++;
			}
		}
		if (num <= 0)
		{
			return;
		}
		foreach (KeyValuePair<Pawn, int> item2 in totalPresenceTmp)
		{
			if (item2.Key.Ideo == Ritual.ideo)
			{
				Thought_RelicAtRitual thought_RelicAtRitual = (Thought_RelicAtRitual)ThoughtMaker.MakeThought(ThoughtDefOf.RelicAtRitual, Mathf.Min(num, ThoughtDefOf.RelicAtRitual.stages.Count) - 1);
				thought_RelicAtRitual.relicName = Find.ActiveLanguageWorker.WithDefiniteArticle(str, Gender.None);
				item2.Key.needs.mood.thoughts.memories.TryGainMemory(thought_RelicAtRitual);
			}
		}
	}

	private void AddParticipantThoughts()
	{
		if (!ModsConfig.IdeologyActive || Ritual?.ideo == null || Ritual.def.mergeRitualGizmosFromAllIdeos)
		{
			return;
		}
		foreach (KeyValuePair<Pawn, int> item in totalPresenceTmp)
		{
			if (item.Key.Ideo != Ritual.ideo)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.ParticipatedInOthersRitual, item.Key.Named(HistoryEventArgsNames.Doer)));
			}
		}
		AddRelicInRoomThought();
	}

	protected override LordToil CreateGatheringToil(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef)
	{
		return MakeToil((!stages.NullOrEmpty()) ? stages[0] : null);
	}

	public bool TargetStillAllowed()
	{
		if (selectedTarget.ThingDestroyed)
		{
			return false;
		}
		if (ritual.behavior.ChecksReservations)
		{
			reservers.Clear();
			selectedTarget.Map.reservationManager.ReserversOf((LocalTargetInfo)selectedTarget, reservers);
			reservers.ExceptWith(assignments.Participants);
			if (reservers.Count > 0)
			{
				return false;
			}
		}
		if (!Ritual.behavior.TargetStillAllowed(selectedTarget, this))
		{
			return false;
		}
		return true;
	}

	protected override bool ShouldBeCalledOff()
	{
		if (lord.ownedPawns.Count == 0)
		{
			return true;
		}
		if (!TargetStillAllowed())
		{
			return true;
		}
		if (organizer != null && organizer.Downed)
		{
			return true;
		}
		foreach (RitualRole item in assignments.AllRolesForReading)
		{
			if (item.required)
			{
				Pawn pawn = assignments.FirstAssignedPawn(item);
				if (pawn != null && !lord.ownedPawns.Contains(pawn) && ShouldCallOffBecausePawnNoLongerOwned(pawn))
				{
					return true;
				}
			}
		}
		foreach (Pawn item2 in assignments.ExtraRequiredPawnsForReading)
		{
			if (!lord.ownedPawns.Contains(item2) && ShouldCallOffBecausePawnNoLongerOwned(item2))
			{
				return true;
			}
			IThingHolder thingHolder;
			if (item2.Corpse == null || item2.Corpse.ParentHolder == null)
			{
				IThingHolder carriedBy = item2.CarriedBy;
				thingHolder = carriedBy;
			}
			else
			{
				thingHolder = item2.Corpse.ParentHolder.ParentHolder;
			}
			if (thingHolder is Pawn p && p.GetLord() != lord)
			{
				return true;
			}
		}
		foreach (Pawn participant in assignments.Participants)
		{
			if (participant != null && participant.Map != null)
			{
				RitualRole ritualRole = assignments.RoleForPawn(participant);
				if (((ritualRole != null && ritualRole.required) || assignments.Required(participant) || assignments.ExtraRequiredPawnsForReading.Contains(participant)) && SelfDefenseUtility.ShouldStartFleeing(participant))
				{
					return true;
				}
			}
		}
		return false;
	}

	protected virtual bool ShouldCallOffBecausePawnNoLongerOwned(Pawn p)
	{
		return !pawnsDeathIgnored.Contains(p);
	}

	public bool RoleFilled(string roleId)
	{
		if (ritual.behavior.def.roles.NullOrEmpty())
		{
			return false;
		}
		if (ritual.behavior.def.roles.FirstOrDefault((RitualRole r) => r.id == roleId) == null)
		{
			return false;
		}
		return assignments.FirstAssignedPawn(roleId) != null;
	}

	public RitualRole RoleFor(Pawn p, bool includeForced = false)
	{
		return assignments?.RoleForPawn(p, includeForced);
	}

	public bool TryGetRoleFor(Pawn p, out RitualRole role, bool includeForced = false)
	{
		role = assignments?.RoleForPawn(p, includeForced);
		return role != null;
	}

	public Pawn PawnWithRole(string roleId)
	{
		return assignments.FirstAssignedPawn(roleId);
	}

	public RitualRole GetRole(string id)
	{
		if (ritual.behavior.def.roles.NullOrEmpty())
		{
			return null;
		}
		return ritual.behavior.def.roles.FirstOrDefault((RitualRole r) => r.id == id);
	}

	public RitualRoleBehavior BehaviorFor(Pawn pawn, bool includeForced)
	{
		return CurrentStage?.BehaviorForRole(RoleFor(pawn, includeForced)?.id);
	}

	[IteratorStateMachine(typeof(_003CGetPawnGizmos_003Ed__105))]
	public override IEnumerable<Gizmo> GetPawnGizmos(Pawn p)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetPawnGizmos_003Ed__105(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__p = p
		};
	}

	public Gizmo GetCancelGizmo()
	{
		Command_Action command_Action = new Command_Action();
		command_Action.defaultLabel = "CommandCancelRitual".Translate(ritual.Named("RITUAL"));
		command_Action.defaultDesc = "CommandCancelRitualDesc".Translate(ritual.Named("RITUAL"));
		command_Action.icon = ritual.CancelIcon;
		if (!ritual.def.mergeRitualGizmosFromAllIdeos)
		{
			command_Action.defaultIconColor = ritual.ideo.Color;
		}
		command_Action.action = delegate
		{
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("CommandCancelRitualConfirm".Translate(ritual.Named("RITUAL")), Cancel));
			SoundDefOf.Tick_Low.PlayOneShotOnCamera();
		};
		command_Action.hotKey = KeyBindingDefOf.Misc6;
		return command_Action;
	}

	public void Cancel()
	{
		Find.SignalManager.SendSignal(new Signal(CancelSignal));
	}

	public override float VoluntaryJoinPriorityFor(Pawn p)
	{
		if (IsInvited(p))
		{
			bool ignoreBleeding = RoleFor(p, includeForced: true)?.ignoreBleeding ?? ritual?.behavior.def.spectatorsIgnoreBleeding ?? false;
			if (!GatheringsUtility.ShouldPawnKeepAttendingRitual(p, ritual, ignoreBleeding))
			{
				return 0f;
			}
			if (spot.IsForbidden(p))
			{
				return 0f;
			}
			if (!lord.ownedPawns.Contains(p) && IsGatheringAboutToEnd())
			{
				return 0f;
			}
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].def.blocksSocialInteraction)
				{
					return 0f;
				}
			}
			return VoluntarilyJoinableLordJobJoinPriorities.SocialGathering;
		}
		return 0f;
	}

	protected override bool IsInvited(Pawn p)
	{
		if (pawnsForcedToLeave.Contains(p))
		{
			return false;
		}
		if (assignments != null && !assignments.PawnParticipating(p))
		{
			return false;
		}
		return true;
	}

	public override void LordJobTick()
	{
		base.LordJobTick();
		Building edifice = spot.GetEdifice(base.Map);
		TargetInfo targetInfo = ((edifice != null) ? ((TargetInfo)edifice) : new TargetInfo(spot, base.Map));
		if (CurrentStage == null || CurrentStage.showProgressBar)
		{
			if (progressBar == null)
			{
				progressBar = EffecterDefOf.ProgressBarAlwaysVisible.Spawn();
			}
			progressBar.EffectTick(targetInfo, TargetInfo.Invalid);
			MoteProgressBar mote = ((SubEffecter_ProgressBar)progressBar.children[0]).mote;
			if (mote != null)
			{
				mote.progress = progressBarOverride ?? (1f - Mathf.Clamp01((float)TicksLeft / (float)base.DurationTicks));
				mote.offsetZ = -0.5f;
			}
		}
		else if (progressBar != null)
		{
			progressBar.Cleanup();
			progressBar = null;
		}
		if (ritual != null)
		{
			if (lord.CurLordToil is LordToil_Ritual { stage: not null } lordToil_Ritual)
			{
				ritual.outcomeEffect.Tick(this, lordToil_Ritual.stage.ProgressPerTick(this));
			}
			else
			{
				ritual.outcomeEffect.Tick(this);
			}
		}
		if (ritual != null && ritual.behavior != null)
		{
			ritual.behavior.Tick(this);
		}
		if (DebugSettings.playRitualAmbience && ambienceDef != null && (ambiencePlaying == null || ambiencePlaying.Ended))
		{
			ambiencePlaying = ambienceDef.TrySpawnSustainer(SoundInfo.InMap(targetInfo, MaintenanceType.PerTick));
		}
		ambiencePlaying?.Maintain();
		if (lord.CurLordToil is LordToil_Ritual { stage: { } stage })
		{
			foreach (ActionOnTick action in ritualStageOnTickActions[stages.IndexOf(stage)].actions)
			{
				if (action.tick == stageTicks)
				{
					try
					{
						action.Apply(this);
					}
					catch (Exception ex)
					{
						Log.Error("Error while applying ritual on-tick action: " + ex);
					}
				}
			}
		}
		if (!initedVisualEffects)
		{
			foreach (RitualVisualEffect effectWorker in effectWorkers)
			{
				effectWorker.ritual = this;
				effectWorker.Setup(this, loading: true);
			}
			foreach (RitualVisualEffect item in effectWorkersCurrentStage)
			{
				item.ritual = this;
				item.Setup(this, loading: true);
			}
			initedVisualEffects = true;
		}
		foreach (RitualVisualEffect effectWorker2 in effectWorkers)
		{
			effectWorker2.Tick();
		}
		foreach (RitualVisualEffect item2 in effectWorkersCurrentStage)
		{
			item2.Tick();
		}
		foreach (KeyValuePair<IntVec3, Mote> highlightedPosition in highlightedPositions)
		{
			highlightedPosition.Value.Maintain();
		}
		foreach (KeyValuePair<Pawn, Mote> highlightedPawn in highlightedPawns)
		{
			highlightedPawn.Value.Maintain();
		}
		ticksPassed++;
		stageTicks++;
		if (lastEssentialStageEndedTick != -1 && ticksPassed > lastEssentialStageEndedTick)
		{
			ticksSinceLastEssentialStage++;
		}
	}

	public override void Cleanup()
	{
		base.Cleanup();
		if (progressBar != null)
		{
			progressBar.Cleanup();
			progressBar = null;
		}
		ritual?.behavior?.Cleanup(this);
		highlightedPositions.Clear();
		highlightedPawns.Clear();
		foreach (RitualVisualEffect effectWorker in effectWorkers)
		{
			effectWorker.Cleanup();
		}
		foreach (RitualVisualEffect item in effectWorkersCurrentStage)
		{
			item.Cleanup();
		}
	}

	public override void PostCleanup()
	{
		ritual?.behavior?.PostCleanup(this);
	}

	public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
	{
		base.Notify_PawnLost(p, condition);
		if (progressBar != null)
		{
			progressBar.Cleanup();
			progressBar = null;
		}
		if (lord.CurLordToil is LordToil_Ritual lordToil_Ritual)
		{
			lordToil_Ritual.stage?.pawnLeaveAction?.ApplyToPawn(this, p);
		}
		if (condition == PawnLostCondition.Killed)
		{
			RitualRole ritualRole = assignments.RoleForPawn(p);
			if (ritualRole != null && !ritualRole.removeFromAssignmentsOnDeath)
			{
				return;
			}
		}
		assignments.RemoveParticipant(p);
	}

	public override void Notify_InMentalState(Pawn pawn, MentalStateDef stateDef)
	{
		base.Notify_InMentalState(pawn, stateDef);
		lord.Notify_PawnLost(pawn, PawnLostCondition.InMentalState, null);
	}

	public void AddTagForPawn(Pawn p, string tag)
	{
		if (perPawnTags.ContainsKey(p))
		{
			if (!perPawnTags[p].tags.Contains(tag))
			{
				perPawnTags[p].tags.Add(tag);
			}
		}
		else
		{
			perPawnTags[p] = new PawnTags
			{
				tags = new List<string> { tag }
			};
		}
	}

	public bool PawnTagSet(Pawn p, string tag)
	{
		if (perPawnTags.ContainsKey(p))
		{
			return perPawnTags[p].tags.Contains(tag);
		}
		return false;
	}

	public TargetInfo SecondFocusForStage(int index, Pawn forPawn = null)
	{
		if (forPawn != null)
		{
			foreach (RitualStagePawnSecondFocus item in stagePawnSecondFocus)
			{
				if (item.stageIndex == index && item.pawn == forPawn)
				{
					return item.target;
				}
			}
		}
		return stageSecondFocus[index];
	}

	[IteratorStateMachine(typeof(_003CAllSecondFoci_003Ed__118))]
	public IEnumerable<TargetInfo> AllSecondFoci(int index)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllSecondFoci_003Ed__118(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__index = index
		};
	}

	public TargetInfo SecondFocusForStage(RitualStage stage, Pawn forPawn = null)
	{
		return SecondFocusForStage(stages.IndexOf(stage), forPawn);
	}

	public PawnStagePosition PawnPositionForStage(Pawn pawn, RitualStage stage)
	{
		int num = stages.IndexOf(stage);
		if (num == -1 || ritualStagePositions.Count <= num)
		{
			Log.Error("Invalid stage id for ritual stage position: " + num);
			return null;
		}
		foreach (KeyValuePair<PawnRitualReference, PawnStagePosition> referencePosition in ritualStagePositions[num].referencePositions)
		{
			if (referencePosition.Key.pawn == pawn)
			{
				return referencePosition.Value;
			}
		}
		return null;
	}

	public IntVec3 CurrentSpectatorCrowdCenter()
	{
		int num = 0;
		Vector3 zero = Vector3.zero;
		foreach (Pawn item in assignments.SpectatorsForReading)
		{
			Job curJob = item.CurJob;
			if (curJob != null && curJob.def == JobDefOf.SpectateCeremony)
			{
				LocalTargetInfo target = curJob.GetTarget(TargetIndex.A);
				if (target.IsValid)
				{
					zero += target.Cell.ToVector3();
					num++;
				}
			}
		}
		if (num == 0)
		{
			return IntVec3.Invalid;
		}
		zero /= (float)num;
		return new IntVec3((int)zero.x, (int)zero.y, (int)zero.z);
	}

	private void AddPositionHighlight(IntVec3 pos)
	{
		Mote mote = MoteMaker.MakeStaticMote(pos.ToVector3Shifted(), base.Map, ThingDefOf.Mote_RolePositionHighlight);
		highlightedPositions.Add(pos, mote);
		mote.Maintain();
	}

	private void AddPawnHighlight(Pawn pawn)
	{
		Mote mote = MoteMaker.MakeAttachedOverlay(pawn, ThingDefOf.Mote_RolePawnHighlight, new Vector3(0f, 0f, -0.4f));
		highlightedPawns.Add(pawn, mote);
		mote.Maintain();
	}

	public bool IsParticipating(Pawn p)
	{
		if (lord.ownedPawns.Contains(p))
		{
			return GatheringsUtility.InGatheringArea(p.Position, Spot, p.MapHeld);
		}
		return false;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_References.Look(ref ritual, "ritual");
		Scribe_References.Look(ref obligation, "obligation");
		Scribe_Values.Look(ref ticksPassed, "ticksPassed", 0);
		Scribe_Values.Look(ref ticksPassedWithProgress, "ticksPassedWithProgress", 0f);
		Scribe_Values.Look(ref progressBarOverride, "progressBarOverride", null);
		Scribe_Values.Look(ref stageTicks, "stageTicks", 0);
		Scribe_Values.Look(ref lastEssentialStageIndex, "lastEssentialStageIndex", 0);
		Scribe_Values.Look(ref lastEssentialStageEndedTick, "lastEssentialStageEndedTick", 0);
		Scribe_Values.Look(ref ticksSinceLastEssentialStage, "ticksSinceLastEssentialStage", 0);
		Scribe_Values.Look(ref repeatPenalty, "repeatPenalty", defaultValue: false);
		Scribe_Values.Look(ref stageIndex, "stageIndex", 0);
		Scribe_Defs.Look(ref ambienceDef, "ambienceDef");
		Scribe_TargetInfo.Look(ref selectedTarget, "selectedTarget");
		Scribe_Collections.Look(ref pawnsForcedToLeave, "pawnsForcedToLeave", LookMode.Reference);
		Scribe_Collections.Look(ref usedThings, "usedThings", LookMode.Reference);
		Scribe_Collections.Look(ref pawnsDeathIgnored, "pawnsDeathIgnored", true, LookMode.Reference);
		Scribe_Collections.Look(ref perPawnTags, "perPawnTags", LookMode.Reference, LookMode.Deep, ref tmpTagPawns, ref tmpTags, logNullErrors: true, saveDestroyedKeys: true);
		Scribe_Collections.Look(ref stages, "stages", LookMode.Deep);
		Scribe_Collections.Look(ref stageSecondFocus, "stageSecondFocus", LookMode.TargetInfo);
		Scribe_Collections.Look(ref stagePawnSecondFocus, "stagePawnSecondFocus", LookMode.Deep);
		Scribe_Collections.Look(ref ritualStagePositions, "ritualStagePositions", LookMode.Deep);
		Scribe_Collections.Look(ref ritualStageOnTickActions, "ritualStageOnTickActions", LookMode.Deep);
		Scribe_Deep.Look(ref assignments, "assignments");
		Scribe_Collections.Look(ref effectWorkers, "effectWorkers", LookMode.Deep);
		Scribe_Collections.Look(ref effectWorkersCurrentStage, "effectWorkersCurrentStage", LookMode.Deep);
		Scribe_Values.Look(ref ignoreDurationToFinish, "ignoreDurationToFinish", defaultValue: false);
		Scribe_Values.Look(ref ended, "ended", defaultValue: false);
	}
}
