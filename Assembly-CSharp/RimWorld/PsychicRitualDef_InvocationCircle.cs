using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld;

public class PsychicRitualDef_InvocationCircle : PsychicRitualDef
{
	public enum InvalidTargetReasonEnum
	{
		None,
		AreaNotClear
	}

	private class RitualQualityOffsetCount
	{
		public float offset;

		public int count;

		public RitualQualityOffsetCount(int count, float offset)
		{
			this.count = count;
			this.offset = offset;
		}
	}

	[CompilerGenerated]
	private sealed class _003CBlockingIssues_003Ed__40 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PsychicRitualDef_InvocationCircle _003C_003E4__this;

		private PsychicRitualRoleAssignments assignments;

		public PsychicRitualRoleAssignments _003C_003E3__assignments;

		private Map map;

		public Map _003C_003E3__map;

		private ProfilerBlock _003C_003E7__wrap1;

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
		public _003CBlockingIssues_003Ed__40(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || (uint)(num - 1) <= 1u)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				PsychicRitualDef_InvocationCircle psychicRitualDef_InvocationCircle = _003C_003E4__this;
				int reachableCount;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = new ProfilerBlock("PsychicRitualDef.BlockingIssues");
					_003C_003E1__state = -3;
					psychicRitualDef_InvocationCircle.tmpGatheringPawns.Clear();
					foreach (var (psychicRitualRoleDef2, collection) in assignments.RoleAssignments)
					{
						if (psychicRitualRoleDef2.CanHandleOfferings)
						{
							psychicRitualDef_InvocationCircle.tmpGatheringPawns.AddRange(collection);
						}
					}
					psychicRitualDef_InvocationCircle.tmpGatheringPawns.RemoveAll(map, (Map _map, Pawn _pawn) => _pawn.MapHeld != _map);
					if (psychicRitualDef_InvocationCircle.TargetRole != null && psychicRitualDef_InvocationCircle.InvokerRole != null)
					{
						Pawn pawn = assignments.FirstAssignedPawn(psychicRitualDef_InvocationCircle.TargetRole);
						if (pawn != null)
						{
							Pawn pawn2 = assignments.FirstAssignedPawn(psychicRitualDef_InvocationCircle.InvokerRole);
							if (pawn2 != null && pawn.IsPrisoner && !map.reachability.CanReach(assignments.Target.Cell, pawn.PositionHeld, PathEndMode.Touch, TraverseParms.For(pawn2)))
							{
								_003C_003E2__current = "PsychicRitualTargetUnreachableByInvoker".Translate(pawn.Named("TARGET"), pawn2.Named("INVOKER"));
								_003C_003E1__state = 1;
								return true;
							}
						}
					}
					goto IL_01aa;
				case 1:
					_003C_003E1__state = -3;
					goto IL_01aa;
				case 2:
					{
						_003C_003E1__state = -3;
						break;
					}
					IL_01aa:
					if (psychicRitualDef_InvocationCircle.RequiredOffering != null && !PsychicRitualDef.OfferingReachable(map, psychicRitualDef_InvocationCircle.tmpGatheringPawns, psychicRitualDef_InvocationCircle.RequiredOffering, out reachableCount))
					{
						_003C_003E2__current = "PsychicRitualOfferingsInsufficient".Translate(psychicRitualDef_InvocationCircle.RequiredOffering.SummaryFilterFirst, reachableCount);
						_003C_003E1__state = 2;
						return true;
					}
					break;
				}
				_003C_003Em__Finally1();
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CBlockingIssues_003Ed__40 _003CBlockingIssues_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CBlockingIssues_003Ed__ = this;
			}
			else
			{
				_003CBlockingIssues_003Ed__ = new _003CBlockingIssues_003Ed__40(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CBlockingIssues_003Ed__.assignments = _003C_003E3__assignments;
			_003CBlockingIssues_003Ed__.map = _003C_003E3__map;
			return _003CBlockingIssues_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003COutcomeWarnings_003Ed__33 : IEnumerable<TaggedString>, IEnumerable, IEnumerator<TaggedString>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private TaggedString _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private PsychicRitualRoleAssignments assignments;

		public PsychicRitualRoleAssignments _003C_003E3__assignments;

		public PsychicRitualDef_InvocationCircle _003C_003E4__this;

		private IEnumerator<Pawn> _003C_003E7__wrap1;

		TaggedString IEnumerator<TaggedString>.Current
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
		public _003COutcomeWarnings_003Ed__33(int _003C_003E1__state)
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
				PsychicRitualDef_InvocationCircle psychicRitualDef_InvocationCircle = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = assignments.AssignedPawns(psychicRitualDef_InvocationCircle.TargetRole).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					Pawn current = _003C_003E7__wrap1.Current;
					if (current.HomeFaction != null && current.HomeFaction != Faction.OfPlayer && current.HomeFaction.def.humanlikeFaction && !current.HomeFaction.def.PermanentlyHostileTo(FactionDefOf.PlayerColony) && !current.HomeFaction.temporary && !current.HomeFaction.Hidden)
					{
						_003C_003E2__current = "PsychicRitualFactionWarning".Translate(current.Named("PAWN"), current.HomeFaction.Named("FACTION")).Colorize(ColoredText.WarningColor);
						_003C_003E1__state = 1;
						return true;
					}
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
		IEnumerator<TaggedString> IEnumerable<TaggedString>.GetEnumerator()
		{
			_003COutcomeWarnings_003Ed__33 _003COutcomeWarnings_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003COutcomeWarnings_003Ed__ = this;
			}
			else
			{
				_003COutcomeWarnings_003Ed__ = new _003COutcomeWarnings_003Ed__33(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003COutcomeWarnings_003Ed__.assignments = _003C_003E3__assignments;
			return _003COutcomeWarnings_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TaggedString>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__43 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PsychicRitualDef_InvocationCircle _003C_003E4__this;

		private StatRequest req;

		public StatRequest _003C_003E3__req;

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
		public _003CSpecialDisplayStats_003Ed__43(int _003C_003E1__state)
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
				PsychicRitualDef_InvocationCircle psychicRitualDef_InvocationCircle = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)psychicRitualDef_InvocationCircle).SpecialDisplayStats(req).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0080;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0080;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00f9;
				case 3:
					_003C_003E1__state = -1;
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.PsychicRituals, "StatsReport_RitualCooldown".Translate(), (psychicRitualDef_InvocationCircle.cooldownHours * 2500).ToStringTicksToPeriod(), "StatsReport_RitualCooldown_Desc".Translate(), 100);
					_003C_003E1__state = 4;
					return true;
				case 4:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_0080:
					if (_003C_003E7__wrap1.MoveNext())
					{
						StatDrawEntry current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (psychicRitualDef_InvocationCircle.requiredOffering != null)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.PsychicRituals, "StatsReport_Offering".Translate(), psychicRitualDef_InvocationCircle.requiredOffering.SummaryFilterFirst, "StatsReport_Offering_Desc".Translate(), 1000);
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00f9;
					IL_00f9:
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.PsychicRituals, "StatsReport_RitualDuration".Translate(), Mathf.FloorToInt(psychicRitualDef_InvocationCircle.hoursUntilOutcome.min * 2500f).ToStringTicksToPeriod(), "StatsReport_RitualDuration_Desc".Translate(), 500);
					_003C_003E1__state = 3;
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
			_003CSpecialDisplayStats_003Ed__43 _003CSpecialDisplayStats_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CSpecialDisplayStats_003Ed__ = this;
			}
			else
			{
				_003CSpecialDisplayStats_003Ed__ = new _003CSpecialDisplayStats_003Ed__43(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CSpecialDisplayStats_003Ed__.req = _003C_003E3__req;
			return _003CSpecialDisplayStats_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	public FloatRange hoursUntilHoraxEffect;

	public FloatRange hoursUntilOutcome;

	public float invocationCircleRadius = 3.9f;

	[MustTranslate]
	public string outcomeDescription;

	public float psychicSensitivityPowerFactor = 0.25f;

	protected PsychicRitualRoleDef invokerRole;

	protected PsychicRitualRoleDef chanterRole;

	protected PsychicRitualRoleDef targetRole;

	protected PsychicRitualRoleDef defenderRole;

	protected IngredientCount requiredOffering;

	protected string timeAndOfferingLabelCached;

	public static readonly SimpleCurve PsychicSensitivityToPowerFactor = new SimpleCurve
	{
		new CurvePoint(0f, 0f),
		new CurvePoint(1f, 0.5f),
		new CurvePoint(2f, 0.9f),
		new CurvePoint(3f, 1f)
	};

	protected const int DurationTicksWaitPostEffect = 120;

	private static Dictionary<PsychicRitualRoleDef, List<IntVec3>> tmpParticipants = new Dictionary<PsychicRitualRoleDef, List<IntVec3>>(8);

	private List<Pawn> tmpGatheringPawns = new List<Pawn>(8);

	public virtual PsychicRitualRoleDef InvokerRole => invokerRole;

	public virtual PsychicRitualRoleDef ChanterRole => chanterRole;

	public virtual PsychicRitualRoleDef TargetRole => targetRole;

	public virtual PsychicRitualRoleDef DefenderRole => defenderRole;

	public virtual IngredientCount RequiredOffering => requiredOffering;

	public TaggedString CooldownLabel => "PsychicRitualCooldownLabel".Translate() + ": " + (cooldownHours * 2500).ToStringTicksToPeriod();

	public override List<PsychicRitualRoleDef> Roles
	{
		get
		{
			List<PsychicRitualRoleDef> roles = base.Roles;
			if (InvokerRole != null)
			{
				roles.Add(InvokerRole);
			}
			if (TargetRole != null)
			{
				roles.Add(TargetRole);
			}
			if (ChanterRole != null)
			{
				roles.Add(ChanterRole);
			}
			if (DefenderRole != null)
			{
				roles.Add(DefenderRole);
			}
			return roles;
		}
	}

	public override void ResolveReferences()
	{
		base.ResolveReferences();
		requiredOffering?.ResolveReferences();
		invokerRole = invokerRole ?? PsychicRitualRoleDefOf.Invoker;
		chanterRole = chanterRole ?? PsychicRitualRoleDefOf.Chanter;
	}

	public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph parent)
	{
		float randomInRange = hoursUntilOutcome.RandomInRange;
		IReadOnlyDictionary<PsychicRitualRoleDef, List<IntVec3>> readOnlyDictionary = GenerateRolePositions(psychicRitual.assignments);
		return new List<PsychicRitualToil>
		{
			new PsychicRitualToil_GatherForInvocation(psychicRitual, this, readOnlyDictionary),
			new PsychicRitualToil_InvokeHorax(InvokerRole, readOnlyDictionary.TryGetValue(InvokerRole), TargetRole, readOnlyDictionary.TryGetValue(TargetRole), ChanterRole, readOnlyDictionary.TryGetValue(ChanterRole), DefenderRole, readOnlyDictionary.TryGetValue(DefenderRole), RequiredOffering)
			{
				hoursUntilHoraxEffect = hoursUntilHoraxEffect.RandomInRange,
				hoursUntilOutcome = randomInRange
			},
			new PsychicRitualToil_Wait(120)
		};
	}

	public override bool IsValidTarget(TargetInfo target, out AnyEnum reason)
	{
		foreach (IntVec3 item in GenRadial.RadialCellsAround(target.Cell, invocationCircleRadius, useCenter: true))
		{
			if (!item.Standable(target.Map))
			{
				reason = AnyEnum.FromEnum(InvalidTargetReasonEnum.AreaNotClear);
				return false;
			}
		}
		reason = AnyEnum.None;
		return true;
	}

	public override TaggedString InvalidTargetReason(AnyEnum reason)
	{
		InvalidTargetReasonEnum? invalidTargetReasonEnum = reason.As<InvalidTargetReasonEnum>();
		if (invalidTargetReasonEnum.HasValue)
		{
			InvalidTargetReasonEnum valueOrDefault = invalidTargetReasonEnum.GetValueOrDefault();
			return valueOrDefault switch
			{
				InvalidTargetReasonEnum.None => TaggedString.Empty, 
				InvalidTargetReasonEnum.AreaNotClear => "PsychicRitualDef_InvocationCircle_AreaMustBeClear".Translate(), 
				_ => throw new InvalidOperationException($"Unknown reason {valueOrDefault}"), 
			};
		}
		return base.InvalidTargetReason(reason);
	}

	public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
	{
		return outcomeDescription.Formatted();
	}

	[IteratorStateMachine(typeof(_003COutcomeWarnings_003Ed__33))]
	public override IEnumerable<TaggedString> OutcomeWarnings(PsychicRitualRoleAssignments assignments)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003COutcomeWarnings_003Ed__33(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__assignments = assignments
		};
	}

	public override TaggedString TimeAndOfferingLabel()
	{
		if (timeAndOfferingLabelCached != null)
		{
			return timeAndOfferingLabelCached;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(DurationLabel());
		stringBuilder.Append(CooldownLabel);
		if (!OfferingLabel().NullOrEmpty())
		{
			stringBuilder.AppendLine();
			stringBuilder.Append(OfferingLabel());
		}
		timeAndOfferingLabelCached = stringBuilder.ToString();
		return timeAndOfferingLabelCached;
	}

	private TaggedString OfferingLabel()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (RequiredOffering != null)
		{
			stringBuilder.Append("PsychicRitualRequiredOffering".Translate().CapitalizeFirst());
			stringBuilder.Append(": ");
			stringBuilder.Append(RequiredOffering.SummaryFilterFirst);
		}
		return stringBuilder.ToString();
	}

	public TaggedString DurationLabel()
	{
		string value = ((int)(hoursUntilOutcome.Average * 2500f)).ToStringTicksToPeriod();
		TaggedString taggedString = ((hoursUntilOutcome.min != hoursUntilOutcome.max) ? "ExpectedLordJobDuration".Translate().CapitalizeFirst() : "PsychicRitualExpectedDurationLabel".Translate().CapitalizeFirst());
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(taggedString);
		stringBuilder.Append(": ");
		stringBuilder.Append(value);
		return stringBuilder.ToString();
	}

	private IReadOnlyDictionary<PsychicRitualRoleDef, List<IntVec3>> GenerateRolePositions(PsychicRitualRoleAssignments assignments)
	{
		tmpParticipants.ClearAndPoolValueLists();
		foreach (PsychicRitualRoleDef role in Roles)
		{
			tmpParticipants[role] = SimplePool<List<IntVec3>>.Get();
		}
		int num = assignments.RoleAssignedCount(ChanterRole) + assignments.RoleAssignedCount(InvokerRole);
		int num2 = 0;
		foreach (Pawn item in assignments.AssignedPawns(InvokerRole))
		{
			_ = item;
			int num3 = 0;
			IntVec3 cell;
			do
			{
				cell = assignments.Target.Cell;
				cell += IntVec3.FromPolar(360f * (float)num2++ / (float)num, invocationCircleRadius);
			}
			while (!cell.Walkable(assignments.Target.Map) && num3++ <= 10);
			if (num3 >= 10)
			{
				cell = assignments.Target.Cell;
			}
			tmpParticipants[InvokerRole].Add(cell);
		}
		foreach (Pawn item2 in assignments.AssignedPawns(ChanterRole))
		{
			_ = item2;
			IntVec3 cell2 = assignments.Target.Cell;
			cell2 += IntVec3.FromPolar(360f * (float)num2++ / (float)num, invocationCircleRadius);
			tmpParticipants[ChanterRole].Add(cell2);
		}
		foreach (Pawn item3 in assignments.AssignedPawns(TargetRole))
		{
			_ = item3;
			tmpParticipants[TargetRole].Add(assignments.Target.Cell);
		}
		if (DefenderRole != null)
		{
			num2 = 0;
			int num4 = assignments.RoleAssignedCount(DefenderRole);
			bool playerRitual = assignments.AllAssignedPawns.Any((Pawn x) => x.Faction == Faction.OfPlayer);
			foreach (Pawn item4 in assignments.AssignedPawns(DefenderRole))
			{
				_ = item4;
				IntVec3 cell3 = assignments.Target.Cell;
				cell3 += IntVec3.FromPolar(360f * (float)num2++ / (float)num4, invocationCircleRadius + 5f);
				cell3 = GetBestStandableRolePosition(playerRitual, cell3, assignments.Target.Cell, assignments.Target.Map);
				tmpParticipants[DefenderRole].Add(cell3);
			}
		}
		return tmpParticipants;
	}

	[IteratorStateMachine(typeof(_003CBlockingIssues_003Ed__40))]
	public override IEnumerable<string> BlockingIssues(PsychicRitualRoleAssignments assignments, Map map)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CBlockingIssues_003Ed__40(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__assignments = assignments,
			_003C_003E3__map = map
		};
	}

	public override void CalculateMaxPower(PsychicRitualRoleAssignments assignments, List<QualityFactor> powerFactorsOut, out float power)
	{
		power = 0f;
		foreach (Pawn item in assignments.AssignedPawns(InvokerRole))
		{
			float statValue = item.GetStatValue(StatDefOf.PsychicSensitivity);
			float num = PsychicSensitivityToPowerFactor.Evaluate(statValue);
			num *= psychicSensitivityPowerFactor;
			powerFactorsOut?.Add(new QualityFactor
			{
				label = "PsychicRitualDef_InvocationCircle_QualityFactor_PsychicSensitivity".Translate(item.Named("PAWN")),
				positive = (statValue >= 1f),
				count = statValue.ToStringPercent(),
				quality = num,
				toolTip = "PsychicRitualDef_InvocationCircle_QualityFactor_PsychicSensitivity_Tooltip".Translate(item.Named("PAWN"))
			});
			power += num;
		}
		base.CalculateMaxPower(assignments, powerFactorsOut, out var power2);
		power += power2;
		if (assignments.Target.Thing is Building building)
		{
			CalculateFacilityQualityOffset(powerFactorsOut, ref power, building);
		}
		power = Mathf.Clamp01(power);
	}

	private static void CalculateFacilityQualityOffset(List<QualityFactor> powerFactorsOut, ref float power, Building building)
	{
		Dictionary<ThingDef, RitualQualityOffsetCount> dictionary = new Dictionary<ThingDef, RitualQualityOffsetCount>();
		List<Thing> linkedFacilitiesListForReading = building.GetComp<CompAffectedByFacilities>().LinkedFacilitiesListForReading;
		for (int i = 0; i < linkedFacilitiesListForReading.Count; i++)
		{
			Thing thing = linkedFacilitiesListForReading[i];
			CompFacility compFacility = thing.TryGetComp<CompFacility>();
			if (compFacility?.StatOffsets == null)
			{
				continue;
			}
			for (int j = 0; j < compFacility.StatOffsets.Count; j++)
			{
				StatModifier statModifier = compFacility.StatOffsets[j];
				if (statModifier.stat == StatDefOf.PsychicRitualQuality)
				{
					if (dictionary.TryGetValue(thing.def, out var value))
					{
						value.count++;
						value.offset += statModifier.value;
					}
					else
					{
						dictionary.Add(thing.def, new RitualQualityOffsetCount(1, statModifier.value));
					}
				}
			}
		}
		foreach (KeyValuePair<ThingDef, RitualQualityOffsetCount> item in dictionary)
		{
			powerFactorsOut?.Add(new QualityFactor
			{
				label = Find.ActiveLanguageWorker.Pluralize(item.Key.label).CapitalizeFirst(),
				positive = true,
				count = item.Value.count + " / " + item.Key.GetCompProperties<CompProperties_Facility>().maxSimultaneous,
				quality = item.Value.offset,
				toolTip = "PsychicRitualDef_InvocationCircle_QualityFactor_Increase_Tooltip".Translate().CapitalizeFirst().EndWithPeriod()
			});
			power += item.Value.offset;
		}
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__43))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__43(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__req = req
		};
	}

	public override void CheckPsychicRitualCancelConditions(PsychicRitual psychicRitual)
	{
		base.CheckPsychicRitualCancelConditions(psychicRitual);
		if (!psychicRitual.canceled && invokerRole != null)
		{
			Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(InvokerRole);
			if (pawn != null && pawn.DeadOrDowned)
			{
				psychicRitual.CancelPsychicRitual("PsychicRitualDef_InvocationCircle_InvokerLost".Translate(pawn.Named("PAWN")));
			}
		}
	}
}
