using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class CompAbilityEffect_ResurrectMech : CompAbilityEffect
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__15 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompAbilityEffect_ResurrectMech _003C_003E4__this;

		private Command_Action _003Cadd_003E5__2;

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
		public _003CCompGetGizmosExtra_003Ed__15(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cadd_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			CompAbilityEffect_ResurrectMech CS_0024_003C_003E8__locals8 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals8.gizmo == null)
				{
					CS_0024_003C_003E8__locals8.gizmo = new Gizmo_MechResurrectionCharges(CS_0024_003C_003E8__locals8);
				}
				_003C_003E2__current = CS_0024_003C_003E8__locals8.gizmo;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				if (DebugSettings.ShowDevGizmos)
				{
					_003Cadd_003E5__2 = new Command_Action();
					_003Cadd_003E5__2.defaultLabel = "DEV: Add charge";
					_003Cadd_003E5__2.action = delegate
					{
						CS_0024_003C_003E8__locals8.resurrectCharges++;
					};
					_003C_003E2__current = _003Cadd_003E5__2;
					_003C_003E1__state = 2;
					return true;
				}
				break;
			case 2:
				_003C_003E1__state = -1;
				new Command_Action
				{
					defaultLabel = "DEV: Remove charge",
					action = delegate
					{
						CS_0024_003C_003E8__locals8.resurrectCharges--;
						CS_0024_003C_003E8__locals8.resurrectCharges = Mathf.Max(CS_0024_003C_003E8__locals8.resurrectCharges, 0);
					}
				};
				_003C_003E2__current = _003Cadd_003E5__2;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003Cadd_003E5__2 = null;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__15 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__15(0)
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
	private sealed class _003CCustomWarmupMotes_003Ed__16 : IEnumerable<Mote>, IEnumerable, IEnumerator<Mote>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Mote _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompAbilityEffect_ResurrectMech _003C_003E4__this;

		private LocalTargetInfo target;

		public LocalTargetInfo _003C_003E3__target;

		private IEnumerator<LocalTargetInfo> _003C_003E7__wrap1;

		Mote IEnumerator<Mote>.Current
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
		public _003CCustomWarmupMotes_003Ed__16(int _003C_003E1__state)
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
				CompAbilityEffect_ResurrectMech compAbilityEffect_ResurrectMech = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = compAbilityEffect_ResurrectMech.parent.GetAffectedTargets(target).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					Thing thing = _003C_003E7__wrap1.Current.Thing;
					_003C_003E2__current = MoteMaker.MakeAttachedOverlay(thing, ThingDefOf.Mote_MechResurrectWarmupOnTarget, Vector3.zero);
					_003C_003E1__state = 1;
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
		IEnumerator<Mote> IEnumerable<Mote>.GetEnumerator()
		{
			_003CCustomWarmupMotes_003Ed__16 _003CCustomWarmupMotes_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCustomWarmupMotes_003Ed__ = this;
			}
			else
			{
				_003CCustomWarmupMotes_003Ed__ = new _003CCustomWarmupMotes_003Ed__16(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CCustomWarmupMotes_003Ed__.target = _003C_003E3__target;
			return _003CCustomWarmupMotes_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Mote>)this).GetEnumerator();
		}
	}

	[LoadAlias("charges")]
	private int resurrectCharges = 30;

	private Dictionary<MechWeightClassDef, int> costsByWeightClass;

	private Gizmo gizmo;

	public new CompProperties_ResurrectMech Props => (CompProperties_ResurrectMech)props;

	public int ChargesRemaining => resurrectCharges;

	public override bool CanCast => resurrectCharges > 0;

	public override void Initialize(AbilityCompProperties props)
	{
		base.Initialize(props);
		costsByWeightClass = new Dictionary<MechWeightClassDef, int>();
		for (int i = 0; i < Props.costs.Count; i++)
		{
			costsByWeightClass[Props.costs[i].weightClass] = Props.costs[i].cost;
		}
	}

	public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
	{
		if (!base.CanApplyOn(target, dest))
		{
			return false;
		}
		if (!target.HasThing || !(target.Thing is Corpse corpse))
		{
			return false;
		}
		if (!CanResurrect(corpse))
		{
			return false;
		}
		return true;
	}

	private bool TryGetResurrectCost(Corpse corpse, out int cost)
	{
		if (corpse.InnerPawn.RaceProps.mechWeightClass != null && costsByWeightClass.ContainsKey(corpse.InnerPawn.RaceProps.mechWeightClass))
		{
			cost = costsByWeightClass[corpse.InnerPawn.RaceProps.mechWeightClass];
			return true;
		}
		cost = -1;
		return false;
	}

	private bool CanResurrect(Corpse corpse)
	{
		if (!corpse.InnerPawn.RaceProps.IsMechanoid || corpse.InnerPawn.RaceProps.mechWeightClass == null || !corpse.InnerPawn.RaceProps.mechWeightClass.canResurrect)
		{
			return false;
		}
		if (corpse.InnerPawn.Faction != parent.pawn.Faction)
		{
			return false;
		}
		if (corpse.InnerPawn.kindDef.abilities != null && corpse.InnerPawn.kindDef.abilities.Contains(AbilityDefOf.ResurrectionMech))
		{
			return false;
		}
		if (corpse.timeOfDeath < Find.TickManager.TicksGame - Props.maxCorpseAgeTicks)
		{
			return false;
		}
		if (!TryGetResurrectCost(corpse, out var cost) || cost > resurrectCharges)
		{
			return false;
		}
		return true;
	}

	public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
	{
		base.Apply(target, dest);
		Corpse corpse = (Corpse)target.Thing;
		if (CanResurrect(corpse) && TryGetResurrectCost(corpse, out var cost))
		{
			Pawn innerPawn = corpse.InnerPawn;
			resurrectCharges -= cost;
			ResurrectionUtility.TryResurrect(innerPawn);
			if (Props.appliedEffecterDef != null)
			{
				Effecter effecter = Props.appliedEffecterDef.SpawnAttached(innerPawn, innerPawn.MapHeld);
				effecter.Trigger(innerPawn, innerPawn);
				effecter.Cleanup();
			}
			innerPawn.stances.stagger.StaggerFor(60);
		}
	}

	public override bool GizmoDisabled(out string reason)
	{
		reason = null;
		return false;
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__15))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__15(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CCustomWarmupMotes_003Ed__16))]
	public override IEnumerable<Mote> CustomWarmupMotes(LocalTargetInfo target)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCustomWarmupMotes_003Ed__16(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__target = target
		};
	}

	public override void PostApplied(List<LocalTargetInfo> targets, Map map)
	{
		Vector3 zero = Vector3.zero;
		foreach (LocalTargetInfo target in targets)
		{
			zero += target.Cell.ToVector3Shifted();
		}
		zero /= (float)targets.Count;
		IntVec3 intVec = zero.ToIntVec3();
		EffecterDefOf.ApocrionAoeResolve.Spawn(intVec, map).EffectTick(new TargetInfo(intVec, map), new TargetInfo(intVec, map));
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref resurrectCharges, "resurrectCharges", 30);
	}
}
