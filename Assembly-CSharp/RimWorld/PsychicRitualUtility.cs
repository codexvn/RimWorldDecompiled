using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public static class PsychicRitualUtility
{
	[CompilerGenerated]
	private sealed class _003CGetPsychicRitualSpotsAffectedByThing_003Ed__0 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Map map;

		public Map _003C_003E3__map;

		private CellRect thingRect;

		public CellRect _003C_003E3__thingRect;

		private List<Thing>.Enumerator _003C_003E7__wrap1;

		Thing IEnumerator<Thing>.Current
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
		public _003CGetPsychicRitualSpotsAffectedByThing_003Ed__0(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<Thing>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.PsychicRitualSpot)).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					Thing current = _003C_003E7__wrap1.Current;
					CompPsychicRitualSpot compPsychicRitualSpot = current.TryGetComp<CompPsychicRitualSpot>();
					if (compPsychicRitualSpot != null && compPsychicRitualSpot.OccupiedCells.Overlaps(thingRect.Cells))
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<Thing>.Enumerator);
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
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CGetPsychicRitualSpotsAffectedByThing_003Ed__0 _003CGetPsychicRitualSpotsAffectedByThing_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetPsychicRitualSpotsAffectedByThing_003Ed__ = this;
			}
			else
			{
				_003CGetPsychicRitualSpotsAffectedByThing_003Ed__ = new _003CGetPsychicRitualSpotsAffectedByThing_003Ed__0(0);
			}
			_003CGetPsychicRitualSpotsAffectedByThing_003Ed__.map = _003C_003E3__map;
			_003CGetPsychicRitualSpotsAffectedByThing_003Ed__.thingRect = _003C_003E3__thingRect;
			return _003CGetPsychicRitualSpotsAffectedByThing_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	private static List<SkillRecord> tmpCandidateSkills = new List<SkillRecord>();

	[IteratorStateMachine(typeof(_003CGetPsychicRitualSpotsAffectedByThing_003Ed__0))]
	public static IEnumerable<Thing> GetPsychicRitualSpotsAffectedByThing(Map map, CellRect thingRect)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetPsychicRitualSpotsAffectedByThing_003Ed__0(-2)
		{
			_003C_003E3__map = map,
			_003C_003E3__thingRect = thingRect
		};
	}

	public static void DrawPsychicRitualSpotsAffectedByThingOverlay(Map map, ThingDef def, IntVec3 pos, Rot4 rotation)
	{
		if (!ModsConfig.AnomalyActive)
		{
			return;
		}
		int num = 0;
		CellRect thingRect = GenAdj.OccupiedRect(pos, rotation, def.size);
		foreach (Thing item in GetPsychicRitualSpotsAffectedByThing(map, thingRect))
		{
			if (num++ > 10)
			{
				break;
			}
			GenDraw.DrawLineBetween(GenThing.TrueCenter(pos, rotation, def.size, def.Altitude), item.TrueCenter(), SimpleColor.Red);
		}
	}

	public static SkillDef GetPhilophagySkillAndXpTransfer(Pawn invoker, Pawn target, float xpTransferPercent, out float xpTransfer)
	{
		IOrderedEnumerable<SkillRecord> orderedEnumerable = from s in target.skills.skills
			where !s.TotallyDisabled && !invoker.skills.GetSkill(s.def).TotallyDisabled
			orderby s.XpTotalEarned descending
			select s;
		if (!orderedEnumerable.Any())
		{
			xpTransfer = 0f;
			return null;
		}
		tmpCandidateSkills.Clear();
		SkillRecord skillRecord = orderedEnumerable.First();
		foreach (SkillRecord item in orderedEnumerable)
		{
			if (item == skillRecord)
			{
				tmpCandidateSkills.Add(item);
			}
			else if (Mathf.Abs(item.XpTotalEarned - skillRecord.XpTotalEarned) < 0.01f)
			{
				tmpCandidateSkills.Add(item);
			}
		}
		SkillDef def = tmpCandidateSkills[0].def;
		foreach (SkillRecord tmpCandidateSkill in tmpCandidateSkills)
		{
			if (invoker.skills.GetSkill(def).XpTotalEarned < invoker.skills.GetSkill(tmpCandidateSkill.def).XpTotalEarned)
			{
				def = tmpCandidateSkill.def;
			}
		}
		float xpTotalEarned = target.skills.GetSkill(def).XpTotalEarned;
		xpTransfer = xpTotalEarned * xpTransferPercent;
		return def;
	}

	public static void RegisterAsExecutionIfPrisoner(Pawn target, Pawn invoker)
	{
		if (target.IsPrisonerOfColony)
		{
			ThoughtUtility.GiveThoughtsForPawnExecuted(target, invoker, PawnExecutionKind.GenericBrutal);
			TaleRecorder.RecordTale(TaleDefOf.ExecutedPrisoner, invoker, target);
		}
	}

	public static void AddPsychicRitualGuiltToPawns(PsychicRitualDef ritualDef, IEnumerable<Pawn> pawns)
	{
		foreach (Pawn pawn in pawns)
		{
			Thought_PsychicRitualGuilt thought_PsychicRitualGuilt = (Thought_PsychicRitualGuilt)ThoughtMaker.MakeThought(ThoughtDefOf.PsychicRitualGuilt);
			thought_PsychicRitualGuilt.ritualDef = ritualDef;
			pawn.needs?.mood?.thoughts?.memories?.TryGainMemory(thought_PsychicRitualGuilt);
		}
	}
}
