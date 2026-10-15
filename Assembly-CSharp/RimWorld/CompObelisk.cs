using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

public abstract class CompObelisk : ThingComp, IActivity
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__40 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompObelisk _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

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
		public _003CCompGetGizmosExtra_003Ed__40(int _003C_003E1__state)
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
				CompObelisk CS_0024_003C_003E8__locals2 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingComp)CS_0024_003C_003E8__locals2).CompGetGizmosExtra().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0072:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (DebugSettings.ShowDevGizmos)
					{
						Command_Action command_Action = new Command_Action
						{
							defaultLabel = "DEV: Trigger interaction effect",
							action = delegate
							{
								CS_0024_003C_003E8__locals2.TriggerInteractionEffect(Find.CurrentMap.mapPawns.FreeColonistsSpawned.RandomElement());
							}
						};
						if (Find.CurrentMap.mapPawns.FreeColonistsSpawned.Count == 0)
						{
							command_Action.Disable("No colonists to interact with");
						}
						_003C_003E2__current = command_Action;
						_003C_003E1__state = 2;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__40 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__40(0)
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

	protected Effecter obeliskWarmupEffecter;

	protected const int WarmupDelayTicks = 240;

	protected bool warmupComplete;

	private int delayedActionTick = int.MaxValue;

	protected bool activated;

	protected int lastInteractionEffectTick = -9999999;

	private Sustainer stageOneSustainer;

	private Sustainer stageTwoSustainer;

	private Sustainer stageThreeSustainer;

	private CompStudyUnlocks studyInt;

	private CompActivity activityInt;

	private static readonly SimpleCurve SustainerStudiedStageCurve = new SimpleCurve
	{
		new CurvePoint(0f, 0f),
		new CurvePoint(0.1f, 1f),
		new CurvePoint(0.5f, 2f),
		new CurvePoint(1f, 3f)
	};

	protected CompProperties_Obelisk Props => (CompProperties_Obelisk)props;

	protected virtual float InteractionEffectChanceMTBDays => 1f;

	protected CompStudyUnlocks StudyUnlocks => studyInt ?? (studyInt = parent.TryGetComp<CompStudyUnlocks>());

	public CompActivity ActivityComp => activityInt ?? (activityInt = parent.TryGetComp<CompActivity>());

	public bool StudyFinished => StudyUnlocks.Completed;

	public int StudyLevel => StudyUnlocks.Progress;

	public bool Activated => activated;

	public abstract void TriggerInteractionEffect(Pawn interactor, bool triggeredByPlayer = false);

	public override void Initialize(CompProperties props)
	{
		if (!ModLister.CheckAnomaly("CompObelisk"))
		{
			parent.Destroy();
		}
		else
		{
			base.Initialize(props);
		}
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		base.PostSpawnSetup(respawningAfterLoad);
		if (!respawningAfterLoad && !parent.BeingTransportedOnGravship)
		{
			OnImpact();
		}
	}

	public override void Notify_KilledLeavingsLeft(List<Thing> leavings)
	{
		if (!ActivityComp.Deactivated)
		{
			return;
		}
		foreach (Thing leaving in leavings)
		{
			leaving.Destroy();
		}
		leavings.Clear();
	}

	public override void CompTick()
	{
		SoundTick();
		if (!warmupComplete && GenTicks.TicksGame >= delayedActionTick)
		{
			warmupComplete = true;
			obeliskWarmupEffecter?.Cleanup();
			obeliskWarmupEffecter = null;
		}
		else if (activated && obeliskWarmupEffecter == null && !warmupComplete)
		{
			obeliskWarmupEffecter = EffecterDefOf.ObeliskActionWarmup.Spawn(parent, parent.MapHeld).Trigger(parent, parent);
		}
		obeliskWarmupEffecter?.EffectTick(parent, parent);
	}

	private void OnImpact()
	{
		EffecterDefOf.ImpactDustCloud.Spawn(parent.Position, parent.Map).Cleanup();
		for (int i = 0; i < Props.rubbleFilthCount; i++)
		{
			if (CellFinder.TryFindRandomCellNear(parent.OccupiedRect().RandomCell, parent.Map, 2, null, out var result))
			{
				FilthMaker.TryMakeFilth(result, parent.Map, Rand.Bool ? ThingDefOf.Filth_Dirt : ThingDefOf.Filth_RubbleRock);
			}
		}
	}

	public void Notify_InteractedTick(Pawn interactor, int delta)
	{
		if (!activated && lastInteractionEffectTick + Props.interactionEffectCooldownDays * 60000 <= Find.TickManager.TicksGame && Rand.MTBEventOccurs(InteractionEffectChanceMTBDays, 60000f, delta))
		{
			TriggerInteractionEffect(interactor);
		}
	}

	public virtual void OnActivityActivated()
	{
		activated = true;
		delayedActionTick = GenTicks.TicksGame + 240;
	}

	public void OnPassive()
	{
	}

	public bool ShouldGoPassive()
	{
		return false;
	}

	public bool CanBeSuppressed()
	{
		return true;
	}

	public bool CanActivate()
	{
		return true;
	}

	public string ActivityTooltipExtra()
	{
		return null;
	}

	private void SoundTick()
	{
		if (!parent.Spawned)
		{
			return;
		}
		if (parent.Fogged())
		{
			stageOneSustainer?.End();
			stageTwoSustainer?.End();
			stageThreeSustainer?.End();
			return;
		}
		float num = SustainerStudiedStageCurve.Evaluate(ActivityComp.ActivityLevel);
		if (num > 0f)
		{
			if (stageOneSustainer == null || stageOneSustainer.Ended)
			{
				stageOneSustainer = SoundDefOf.ObeliskAmbientStageOne.TrySpawnSustainer(SoundInfo.InMap(parent, MaintenanceType.PerTick));
			}
			stageOneSustainer.info.volumeFactor = Mathf.Min(num, 1f);
			stageOneSustainer.Maintain();
		}
		else
		{
			stageOneSustainer?.End();
		}
		if (num > 1f)
		{
			if (stageTwoSustainer == null || stageTwoSustainer.Ended)
			{
				stageTwoSustainer = SoundDefOf.ObeliskAmbientStageTwo.TrySpawnSustainer(SoundInfo.InMap(parent, MaintenanceType.PerTick));
			}
			stageTwoSustainer.info.volumeFactor = Mathf.Min(num - 1f, 1f);
			stageTwoSustainer.Maintain();
		}
		else
		{
			stageTwoSustainer?.End();
		}
		if (num > 2f)
		{
			if (stageThreeSustainer == null || stageThreeSustainer.Ended)
			{
				stageThreeSustainer = SoundDefOf.ObeliskAmbientStageThree.TrySpawnSustainer(SoundInfo.InMap(parent, MaintenanceType.PerTick));
			}
			stageThreeSustainer.info.volumeFactor = Mathf.Min(num - 2f, 1f);
			stageThreeSustainer.Maintain();
		}
		else
		{
			stageThreeSustainer?.End();
		}
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__40))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__40(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref lastInteractionEffectTick, "lastInteractionEffectTick", 0);
		Scribe_Values.Look(ref activated, "activated", defaultValue: false);
		Scribe_Values.Look(ref delayedActionTick, "delayedActionTick", int.MaxValue);
	}
}
