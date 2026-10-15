using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_ConstructFinishFrame : JobDriver, IBuildableDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass8_0
	{
		public Toil build;

		public JobDriver_ConstructFinishFrame _003C_003E4__this;

		internal void _003CMakeNewToils_003Eb__0()
		{
			GenClamor.DoClamor(build.actor, 15f, ClamorDefOf.Construction);
		}

		internal void _003CMakeNewToils_003Eb__1(int delta)
		{
			Pawn actor = build.actor;
			Frame frame = _003C_003E4__this.Frame;
			if (frame.resourceContainer.Count > 0 && actor.skills != null)
			{
				actor.skills.Learn(SkillDefOf.Construction, 0.25f * (float)delta);
			}
			actor.rotationTracker.FaceTarget(_003C_003E4__this.IsBuildingAttachment ? GenConstruct.GetWallAttachedTo(frame) : frame);
			float num = actor.GetStatValue(StatDefOf.ConstructionSpeed) * 1.7f * (float)delta;
			if (frame.Stuff != null)
			{
				num *= frame.Stuff.GetStatValueAbstract(StatDefOf.ConstructionSpeedFactor);
			}
			float workToBuild = frame.WorkToBuild;
			if (actor.Faction == Faction.OfPlayer)
			{
				float statValue = actor.GetStatValue(StatDefOf.ConstructSuccessChance);
				if (!TutorSystem.TutorialMode && Rand.Value < 1f - Mathf.Pow(statValue, num / workToBuild))
				{
					frame.FailConstruction(actor);
					_003C_003E4__this.ReadyForNextToil();
					return;
				}
			}
			if (frame.def.entityDefToBuild is TerrainDef)
			{
				_003C_003E4__this.Map.snowGrid.SetDepth(frame.Position, 0f);
				_003C_003E4__this.Map.sandGrid?.SetDepth(frame.Position, 0f);
			}
			frame.workDone += num;
			if (frame.workDone >= workToBuild)
			{
				frame.CompleteConstruction(actor);
				_003C_003E4__this.ReadyForNextToil();
			}
		}

		internal EffecterDef _003CMakeNewToils_003Eb__2()
		{
			return ((Frame)build.actor.jobs.curJob.GetTarget(TargetIndex.A).Thing).ConstructionEffect;
		}

		internal bool _003CMakeNewToils_003Eb__3()
		{
			return !GenConstruct.CanConstruct(_003C_003E4__this.Frame, _003C_003E4__this.pawn);
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__8 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_ConstructFinishFrame _003C_003E4__this;

		private _003C_003Ec__DisplayClass8_0 _003C_003E8__1;

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
		public _003CMakeNewToils_003Ed__8(int _003C_003E1__state)
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
			JobDriver_ConstructFinishFrame jobDriver_ConstructFinishFrame = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass8_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				if (jobDriver_ConstructFinishFrame.IsBuildingAttachment)
				{
					_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell).FailOnDespawnedNullOrForbidden(TargetIndex.A);
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E2__current = Toils_Goto.GotoBuild(TargetIndex.A).FailOnDespawnedNullOrForbidden(TargetIndex.A);
				_003C_003E1__state = 2;
				return true;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0099;
			case 2:
				_003C_003E1__state = -1;
				goto IL_0099;
			case 3:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_0099:
				_003C_003E8__1.build = ToilMaker.MakeToil("MakeNewToils");
				_003C_003E8__1.build.initAction = delegate
				{
					GenClamor.DoClamor(_003C_003E8__1.build.actor, 15f, ClamorDefOf.Construction);
				};
				_003C_003E8__1.build.tickIntervalAction = delegate(int delta)
				{
					Pawn actor = _003C_003E8__1.build.actor;
					Frame frame = _003C_003E8__1._003C_003E4__this.Frame;
					if (frame.resourceContainer.Count > 0 && actor.skills != null)
					{
						actor.skills.Learn(SkillDefOf.Construction, 0.25f * (float)delta);
					}
					actor.rotationTracker.FaceTarget(_003C_003E8__1._003C_003E4__this.IsBuildingAttachment ? GenConstruct.GetWallAttachedTo(frame) : frame);
					float num2 = actor.GetStatValue(StatDefOf.ConstructionSpeed) * 1.7f * (float)delta;
					if (frame.Stuff != null)
					{
						num2 *= frame.Stuff.GetStatValueAbstract(StatDefOf.ConstructionSpeedFactor);
					}
					float workToBuild = frame.WorkToBuild;
					if (actor.Faction == Faction.OfPlayer)
					{
						float statValue = actor.GetStatValue(StatDefOf.ConstructSuccessChance);
						if (!TutorSystem.TutorialMode && Rand.Value < 1f - Mathf.Pow(statValue, num2 / workToBuild))
						{
							frame.FailConstruction(actor);
							_003C_003E8__1._003C_003E4__this.ReadyForNextToil();
							return;
						}
					}
					if (frame.def.entityDefToBuild is TerrainDef)
					{
						_003C_003E8__1._003C_003E4__this.Map.snowGrid.SetDepth(frame.Position, 0f);
						_003C_003E8__1._003C_003E4__this.Map.sandGrid?.SetDepth(frame.Position, 0f);
					}
					frame.workDone += num2;
					if (frame.workDone >= workToBuild)
					{
						frame.CompleteConstruction(actor);
						_003C_003E8__1._003C_003E4__this.ReadyForNextToil();
					}
				};
				_003C_003E8__1.build.WithEffect(() => ((Frame)_003C_003E8__1.build.actor.jobs.curJob.GetTarget(TargetIndex.A).Thing).ConstructionEffect, TargetIndex.A, null);
				_003C_003E8__1.build.FailOnDespawnedNullOrForbidden(TargetIndex.A);
				_003C_003E8__1.build.FailOn(() => !GenConstruct.CanConstruct(_003C_003E8__1._003C_003E4__this.Frame, _003C_003E8__1._003C_003E4__this.pawn));
				_003C_003E8__1.build.defaultCompleteMode = ToilCompleteMode.Delay;
				_003C_003E8__1.build.defaultDuration = 5000;
				_003C_003E8__1.build.activeSkill = () => SkillDefOf.Construction;
				_003C_003E8__1.build.handlingFacing = true;
				_003C_003E2__current = _003C_003E8__1.build;
				_003C_003E1__state = 3;
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
		IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
		{
			_003CMakeNewToils_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__8(0)
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

	private const int JobEndInterval = 5000;

	private const TargetIndex BuildingInd = TargetIndex.A;

	private Frame Frame => (Frame)job.GetTarget(TargetIndex.A).Thing;

	private bool IsBuildingAttachment => ((GenConstruct.BuiltDefOf(Frame.def) as ThingDef)?.building)?.isAttachment ?? false;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
	}

	public bool TryGetBuildableRect(out CellRect rect)
	{
		rect = Frame.OccupiedRect();
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__8))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__8(-2)
		{
			_003C_003E4__this = this
		};
	}
}
