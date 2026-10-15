using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class RitualObligationTargetWorker_SkyLanterns : RitualObligationTargetWorker_ThingDef
{
	[CompilerGenerated]
	private sealed class _003CGetBlockingIssues_003Ed__7 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private TargetInfo target;

		public TargetInfo _003C_003E3__target;

		private RitualRoleAssignments assignments;

		public RitualRoleAssignments _003C_003E3__assignments;

		public RitualObligationTargetWorker_SkyLanterns _003C_003E4__this;

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
		public _003CGetBlockingIssues_003Ed__7(int _003C_003E1__state)
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
			RitualObligationTargetWorker_SkyLanterns ritualObligationTargetWorker_SkyLanterns = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				Dictionary<Thing, int> dictionary = new Dictionary<Thing, int>();
				List<Thing> list = target.Map.listerThings.ThingsOfDef(ThingDefOf.WoodLog);
				bool flag = true;
				int num2 = 0;
				blocked.Clear();
				List<Pawn> participants = assignments.Participants;
				foreach (Pawn item in participants)
				{
					int num3 = Math.Max(ritualObligationTargetWorker_SkyLanterns.def.woodPerParticipant - item.inventory.Count(ThingDefOf.WoodLog), 0);
					bool flag2 = false;
					for (int i = 0; i < list.Count; i++)
					{
						Thing thing = list[i];
						if (thing.IsForbidden(item) || !item.CanReserveAndReach(thing, PathEndMode.Touch, item.NormalMaxDanger()))
						{
							continue;
						}
						int num4 = thing.stackCount;
						if (dictionary.TryGetValue(thing, out var value))
						{
							num4 = Math.Max(num4 - value, 0);
						}
						if (num4 >= num3)
						{
							if (dictionary.ContainsKey(thing))
							{
								dictionary[thing] += num3;
							}
							else
							{
								dictionary[thing] = num3;
							}
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						blocked.Add(item);
						flag = false;
						num2++;
					}
				}
				if (!flag)
				{
					TaggedString taggedString = "RitualTargetWoodInfo".Translate(ritualObligationTargetWorker_SkyLanterns.def.woodPerParticipant, ritualObligationTargetWorker_SkyLanterns.def.woodPerParticipant * participants.Count());
					if (num2 == 1)
					{
						taggedString += $" {"RitualTargetWoodInfoAppend".Translate(num2)}";
					}
					else
					{
						taggedString += $" {"RitualTargetWoodInfoAppendMultiple".Translate(num2)}";
					}
					_003C_003E2__current = taggedString;
					_003C_003E1__state = 1;
					return true;
				}
				break;
			}
			case 1:
				_003C_003E1__state = -1;
				break;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CGetBlockingIssues_003Ed__7 _003CGetBlockingIssues_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetBlockingIssues_003Ed__ = this;
			}
			else
			{
				_003CGetBlockingIssues_003Ed__ = new _003CGetBlockingIssues_003Ed__7(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetBlockingIssues_003Ed__.target = _003C_003E3__target;
			_003CGetBlockingIssues_003Ed__.assignments = _003C_003E3__assignments;
			return _003CGetBlockingIssues_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetTargetInfos_003Ed__4 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

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
		public _003CGetTargetInfos_003Ed__4(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = "RitualTargetCampfirePartyInfo".Translate();
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = ThingDefOf.RitualSpot.label;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				return false;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CGetTargetInfos_003Ed__4(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetTargets_003Ed__2 : IEnumerable<TargetInfo>, IEnumerable, IEnumerator<TargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private TargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Map map;

		public Map _003C_003E3__map;

		private List<Thing> _003CritualSpot_003E5__2;

		private List<Thing> _003Ccampfire_003E5__3;

		private int _003Cj_003E5__4;

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
		public _003CGetTargets_003Ed__2(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CritualSpot_003E5__2 = null;
			_003Ccampfire_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!ModLister.CheckIdeology("Skylantern target"))
				{
					return false;
				}
				_003CritualSpot_003E5__2 = map.listerThings.ThingsOfDef(ThingDefOf.RitualSpot);
				_003Cj_003E5__4 = 0;
				goto IL_0090;
			case 1:
				_003C_003E1__state = -1;
				_003Cj_003E5__4++;
				goto IL_0090;
			case 2:
				{
					_003C_003E1__state = -1;
					_003Cj_003E5__4++;
					break;
				}
				IL_0090:
				if (_003Cj_003E5__4 < _003CritualSpot_003E5__2.Count)
				{
					_003C_003E2__current = _003CritualSpot_003E5__2[_003Cj_003E5__4];
					_003C_003E1__state = 1;
					return true;
				}
				_003Ccampfire_003E5__3 = map.listerThings.ThingsOfDef(ThingDefOf.Campfire);
				_003Cj_003E5__4 = 0;
				break;
			}
			if (_003Cj_003E5__4 < _003Ccampfire_003E5__3.Count)
			{
				_003C_003E2__current = _003Ccampfire_003E5__3[_003Cj_003E5__4];
				_003C_003E1__state = 2;
				return true;
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
		IEnumerator<TargetInfo> IEnumerable<TargetInfo>.GetEnumerator()
		{
			_003CGetTargets_003Ed__2 _003CGetTargets_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetTargets_003Ed__ = this;
			}
			else
			{
				_003CGetTargets_003Ed__ = new _003CGetTargets_003Ed__2(0);
			}
			_003CGetTargets_003Ed__.map = _003C_003E3__map;
			return _003CGetTargets_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TargetInfo>)this).GetEnumerator();
		}
	}

	private static readonly HashSet<Pawn> blocked = new HashSet<Pawn>();

	public RitualObligationTargetWorker_SkyLanterns()
	{
	}

	public RitualObligationTargetWorker_SkyLanterns(RitualObligationTargetFilterDef def)
		: base(def)
	{
	}

	[IteratorStateMachine(typeof(_003CGetTargets_003Ed__2))]
	public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTargets_003Ed__2(-2)
		{
			_003C_003E3__map = map
		};
	}

	protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
	{
		Thing thing = target.Thing;
		if (thing.def != ThingDefOf.RitualSpot && thing.def != ThingDefOf.Campfire)
		{
			return false;
		}
		Room room = thing.GetRoom();
		if (def.colonistThingsOnly && (thing.Faction == null || !thing.Faction.IsPlayer))
		{
			return false;
		}
		int num = 0;
		foreach (IntVec3 item in GenRadial.RadialCellsAround(target.Cell, def.unroofedCellSearchRadius, useCenter: false))
		{
			if (item.InBounds(target.Map) && !item.Roofed(target.Map) && item.GetRoom(thing.Map) == room)
			{
				num++;
				if (num >= def.minUnroofedCells)
				{
					break;
				}
			}
		}
		if (num < def.minUnroofedCells)
		{
			return "RitualTargetNeedUnroofedCells".Translate(def.minUnroofedCells);
		}
		if (thing.def == ThingDefOf.RitualSpot)
		{
			return true;
		}
		if (thing.def == ThingDefOf.Campfire)
		{
			if (target.Cell.Roofed(target.Map))
			{
				return "RitualTargetCampfireMustBeUnroofed".Translate();
			}
			CompRefuelable compRefuelable = thing.TryGetComp<CompRefuelable>();
			if (compRefuelable != null)
			{
				if (compRefuelable.HasFuel)
				{
					return true;
				}
				return "RitualTargetCampfireNoFuel".Translate();
			}
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CGetTargetInfos_003Ed__4))]
	public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTargetInfos_003Ed__4(-2);
	}

	public override bool ShouldGrayOut(Pawn pawn, ILordJobAssignmentsManager<RitualRole> assignments, out TaggedString reason)
	{
		if (blocked.Contains(pawn) && assignments.PawnParticipating(pawn))
		{
			reason = "RitualTargetWoodInfoWarning".Translate(pawn.Named("PAWN"));
			return true;
		}
		reason = TaggedString.Empty;
		return false;
	}

	[IteratorStateMachine(typeof(_003CGetBlockingIssues_003Ed__7))]
	public override IEnumerable<string> GetBlockingIssues(TargetInfo target, RitualRoleAssignments assignments)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetBlockingIssues_003Ed__7(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__target = target,
			_003C_003E3__assignments = assignments
		};
	}
}
