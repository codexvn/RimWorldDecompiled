using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_Mine : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass9_0
	{
		public JobDriver_Mine _003C_003E4__this;

		public Toil mine;

		internal bool _003CMakeNewToils_003Eb__0()
		{
			if (!_003C_003E4__this.pawn.IsPlayerControlled)
			{
				return false;
			}
			if (_003C_003E4__this.job.ignoreDesignations)
			{
				return false;
			}
			if (_003C_003E4__this.pawn.Map.designationManager.DesignationAt(_003C_003E4__this.TargetA.Cell, DesignationDefOf.Mine) == null)
			{
				return _003C_003E4__this.pawn.Map.designationManager.DesignationAt(_003C_003E4__this.TargetA.Cell, DesignationDefOf.MineVein) == null;
			}
			return false;
		}

		internal void _003CMakeNewToils_003Eb__1(int delta)
		{
			Pawn actor = mine.actor;
			Thing mineTarget = _003C_003E4__this.MineTarget;
			if (_003C_003E4__this.ticksToPickHit < -100)
			{
				_003C_003E4__this.ResetTicksToPickHit();
			}
			if (actor.skills != null && (mineTarget.Faction != actor.Faction || actor.Faction == null))
			{
				actor.skills.Learn(SkillDefOf.Mining, 0.07f * (float)delta);
			}
			_003C_003E4__this.ticksToPickHit -= delta;
			if (_003C_003E4__this.ticksToPickHit > 0)
			{
				return;
			}
			IntVec3 position = mineTarget.Position;
			if (_003C_003E4__this.effecter == null)
			{
				_003C_003E4__this.effecter = EffecterDefOf.Mine.Spawn();
			}
			_003C_003E4__this.effecter.Trigger(actor, mineTarget);
			_003C_003E4__this.DoDamage(mineTarget, mine, actor, position);
			if (mineTarget.Destroyed)
			{
				actor.Map.mineStrikeManager.CheckStruckOre(position, mineTarget.def, actor);
				actor.records.Increment(RecordDefOf.CellsMined);
				if (_003C_003E4__this.pawn.Faction != Faction.OfPlayer)
				{
					List<Thing> thingList = position.GetThingList(_003C_003E4__this.Map);
					for (int i = 0; i < thingList.Count; i++)
					{
						thingList[i].SetForbidden(value: true, warnOnFail: false);
					}
				}
				if (_003C_003E4__this.pawn.Faction == Faction.OfPlayer && MineStrikeManager.MineableIsVeryValuable(mineTarget.def))
				{
					TaleRecorder.RecordTale(TaleDefOf.MinedValuable, _003C_003E4__this.pawn, mineTarget.def.building.mineableThing);
				}
				if (_003C_003E4__this.pawn.Faction == Faction.OfPlayer && MineStrikeManager.MineableIsValuable(mineTarget.def) && !_003C_003E4__this.pawn.Map.IsPlayerHome)
				{
					TaleRecorder.RecordTale(TaleDefOf.CaravanRemoteMining, _003C_003E4__this.pawn, mineTarget.def.building.mineableThing);
				}
				_003C_003E4__this.ReadyForNextToil();
			}
			else
			{
				_003C_003E4__this.ResetTicksToPickHit();
			}
		}

		internal float _003CMakeNewToils_003Eb__2()
		{
			return 1f - (float)_003C_003E4__this.MineTarget.HitPoints / (float)_003C_003E4__this.MineTarget.MaxHitPoints;
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__9 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Mine _003C_003E4__this;

		private _003C_003Ec__DisplayClass9_0 _003C_003E8__1;

		Toil IEnumerator<Toil>.Current
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
		public _003CMakeNewToils_003Ed__9(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_Mine f = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass9_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				f.FailOnDespawnedNullOrForbidden(TargetIndex.A);
				f.FailOn(delegate
				{
					if (!_003C_003E8__1._003C_003E4__this.pawn.IsPlayerControlled)
					{
						return false;
					}
					if (_003C_003E8__1._003C_003E4__this.job.ignoreDesignations)
					{
						return false;
					}
					return _003C_003E8__1._003C_003E4__this.pawn.Map.designationManager.DesignationAt(_003C_003E8__1._003C_003E4__this.TargetA.Cell, DesignationDefOf.Mine) == null && _003C_003E8__1._003C_003E4__this.pawn.Map.designationManager.DesignationAt(_003C_003E8__1._003C_003E4__this.TargetA.Cell, DesignationDefOf.MineVein) == null;
				});
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E8__1.mine = ToilMaker.MakeToil("MakeNewToils");
				_003C_003E8__1.mine.tickIntervalAction = delegate(int delta)
				{
					Pawn actor = _003C_003E8__1.mine.actor;
					Thing mineTarget = _003C_003E8__1._003C_003E4__this.MineTarget;
					if (_003C_003E8__1._003C_003E4__this.ticksToPickHit < -100)
					{
						_003C_003E8__1._003C_003E4__this.ResetTicksToPickHit();
					}
					if (actor.skills != null && (mineTarget.Faction != actor.Faction || actor.Faction == null))
					{
						actor.skills.Learn(SkillDefOf.Mining, 0.07f * (float)delta);
					}
					_003C_003E8__1._003C_003E4__this.ticksToPickHit = _003C_003E8__1._003C_003E4__this.ticksToPickHit - delta;
					if (_003C_003E8__1._003C_003E4__this.ticksToPickHit <= 0)
					{
						IntVec3 position = mineTarget.Position;
						if (_003C_003E8__1._003C_003E4__this.effecter == null)
						{
							_003C_003E8__1._003C_003E4__this.effecter = EffecterDefOf.Mine.Spawn();
						}
						_003C_003E8__1._003C_003E4__this.effecter.Trigger(actor, mineTarget);
						_003C_003E8__1._003C_003E4__this.DoDamage(mineTarget, _003C_003E8__1.mine, actor, position);
						if (mineTarget.Destroyed)
						{
							actor.Map.mineStrikeManager.CheckStruckOre(position, mineTarget.def, actor);
							actor.records.Increment(RecordDefOf.CellsMined);
							if (_003C_003E8__1._003C_003E4__this.pawn.Faction != Faction.OfPlayer)
							{
								List<Thing> thingList = position.GetThingList(_003C_003E8__1._003C_003E4__this.Map);
								for (int i = 0; i < thingList.Count; i++)
								{
									thingList[i].SetForbidden(value: true, warnOnFail: false);
								}
							}
							if (_003C_003E8__1._003C_003E4__this.pawn.Faction == Faction.OfPlayer && MineStrikeManager.MineableIsVeryValuable(mineTarget.def))
							{
								TaleRecorder.RecordTale(TaleDefOf.MinedValuable, _003C_003E8__1._003C_003E4__this.pawn, mineTarget.def.building.mineableThing);
							}
							if (_003C_003E8__1._003C_003E4__this.pawn.Faction == Faction.OfPlayer && MineStrikeManager.MineableIsValuable(mineTarget.def) && !_003C_003E8__1._003C_003E4__this.pawn.Map.IsPlayerHome)
							{
								TaleRecorder.RecordTale(TaleDefOf.CaravanRemoteMining, _003C_003E8__1._003C_003E4__this.pawn, mineTarget.def.building.mineableThing);
							}
							_003C_003E8__1._003C_003E4__this.ReadyForNextToil();
						}
						else
						{
							_003C_003E8__1._003C_003E4__this.ResetTicksToPickHit();
						}
					}
				};
				_003C_003E8__1.mine.defaultCompleteMode = ToilCompleteMode.Never;
				_003C_003E8__1.mine.WithProgressBar(TargetIndex.A, () => 1f - (float)_003C_003E8__1._003C_003E4__this.MineTarget.HitPoints / (float)_003C_003E8__1._003C_003E4__this.MineTarget.MaxHitPoints);
				_003C_003E8__1.mine.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
				_003C_003E8__1.mine.activeSkill = () => SkillDefOf.Mining;
				_003C_003E2__current = _003C_003E8__1.mine;
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
		IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
		{
			_003CMakeNewToils_003Ed__9 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__9(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Toil>)this).GetEnumerator();
		}
	}

	private int ticksToPickHit = -1000;

	private Effecter effecter;

	public const int BaseTicksBetweenPickHits = 100;

	private const int BaseDamagePerPickHit_NaturalRock = 80;

	private const int BaseDamagePerPickHit_NotNaturalRock = 40;

	private const float MinMiningSpeedFactorForNPCs = 0.6f;

	private Thing MineTarget => job.GetTarget(TargetIndex.A).Thing;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(MineTarget, job, 1, -1, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__9))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__9(-2)
		{
			_003C_003E4__this = this
		};
	}

	private void DoDamage(Thing target, Toil mine, Pawn actor, IntVec3 mineablePos)
	{
		int num = (target.def.building.isNaturalRock ? 80 : 40);
		if (!(target is Mineable mineable) || target.HitPoints > num)
		{
			DamageInfo dinfo = new DamageInfo(DamageDefOf.Mining, num, 0f, -1f, mine.actor);
			target.TakeDamage(dinfo);
			return;
		}
		bool num2 = base.Map.designationManager.DesignationAt(mineable.Position, DesignationDefOf.MineVein) != null;
		mineable.Notify_TookMiningDamage(target.HitPoints, mine.actor);
		mineable.HitPoints = 0;
		mineable.DestroyMined(actor);
		if (num2)
		{
			IntVec3[] adjacentCells = GenAdj.AdjacentCells;
			foreach (IntVec3 intVec in adjacentCells)
			{
				Designator_MineVein.FloodFillDesignations(mineablePos + intVec, base.Map, mineable.def);
			}
		}
	}

	private void ResetTicksToPickHit()
	{
		float num = pawn.GetStatValue(StatDefOf.MiningSpeed);
		if (num < 0.6f && pawn.Faction != Faction.OfPlayer)
		{
			num = 0.6f;
		}
		ticksToPickHit = (int)Math.Round(100f / num);
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref ticksToPickHit, "ticksToPickHit", 0);
	}
}
