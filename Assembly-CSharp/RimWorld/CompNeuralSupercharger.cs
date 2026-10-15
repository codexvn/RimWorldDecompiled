using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

[StaticConstructorOnStartup]
public class CompNeuralSupercharger : CompRechargeable
{
	public enum AutoUseMode
	{
		NoAutoUse,
		AutoUseWithDesire,
		AutoUseForEveryone
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass15_0
	{
		public CompNeuralSupercharger _003C_003E4__this;

		public Pawn selPawn;

		internal void _003CCompFloatMenuOptions_003Eb__0()
		{
			Job job = JobMaker.MakeJob(JobDefOf.GetNeuralSupercharge, _003C_003E4__this.parent);
			selPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompFloatMenuOptions_003Ed__15 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompNeuralSupercharger _003C_003E4__this;

		private Pawn selPawn;

		public Pawn _003C_003E3__selPawn;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CCompFloatMenuOptions_003Ed__15(int _003C_003E1__state)
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
			CompNeuralSupercharger compNeuralSupercharger = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass15_0 CS_0024_003C_003E8__locals6 = new _003C_003Ec__DisplayClass15_0
				{
					_003C_003E4__this = _003C_003E4__this,
					selPawn = selPawn
				};
				if (!ModLister.CheckIdeology("Neural supercharger"))
				{
					return false;
				}
				if (!compNeuralSupercharger.Charged)
				{
					_003C_003E2__current = new FloatMenuOption(compNeuralSupercharger.Props.jobString + " (" + "NeuralSuperchargeNotReady".Translate() + ")", null);
					_003C_003E1__state = 1;
					return true;
				}
				if (CS_0024_003C_003E8__locals6.selPawn.CurJob != null && CS_0024_003C_003E8__locals6.selPawn.CurJob.def == JobDefOf.GetNeuralSupercharge && CS_0024_003C_003E8__locals6.selPawn.CurJob.targetA.Thing == compNeuralSupercharger.parent)
				{
					_003C_003E2__current = new FloatMenuOption(compNeuralSupercharger.Props.jobString + " (" + "NeuralSuperchargeAlreadyGetting".Translate() + ")", null);
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(compNeuralSupercharger.Props.jobString, delegate
				{
					Job job = JobMaker.MakeJob(JobDefOf.GetNeuralSupercharge, CS_0024_003C_003E8__locals6._003C_003E4__this.parent);
					CS_0024_003C_003E8__locals6.selPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
				}), CS_0024_003C_003E8__locals6.selPawn, compNeuralSupercharger.parent);
				_003C_003E1__state = 3;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				return false;
			case 2:
				_003C_003E1__state = -1;
				return false;
			case 3:
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CCompFloatMenuOptions_003Ed__15 _003CCompFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCompFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CCompFloatMenuOptions_003Ed__ = new _003CCompFloatMenuOptions_003Ed__15(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CCompFloatMenuOptions_003Ed__.selPawn = _003C_003E3__selPawn;
			return _003CCompFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__16 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompNeuralSupercharger _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__16(int _003C_003E1__state)
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
				CompNeuralSupercharger CS_0024_003C_003E8__locals5 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((CompRechargeable)CS_0024_003C_003E8__locals5).CompGetGizmosExtra().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0076;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0076;
				case 2:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Toggle
					{
						defaultLabel = "CommandNeuralSuperchargerAllowGuests".Translate(),
						defaultDesc = "CommandNeuralSuperchargerAllowGuestsDescription".Translate(),
						icon = ColonistOnlyCommandTex,
						isActive = () => CS_0024_003C_003E8__locals5.allowGuests,
						toggleAction = delegate
						{
							CS_0024_003C_003E8__locals5.allowGuests = !CS_0024_003C_003E8__locals5.allowGuests;
						},
						activateSound = SoundDefOf.Tick_Tiny
					};
					_003C_003E1__state = 3;
					return true;
				case 3:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_0076:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E2__current = new Command_SetNeuralSuperchargerAutoUse(CS_0024_003C_003E8__locals5);
					_003C_003E1__state = 2;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__16 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__16(0)
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

	private static Texture2D colonistOnlyCommandTex;

	public AutoUseMode autoUseMode = AutoUseMode.AutoUseWithDesire;

	public bool allowGuests;

	private Effecter effecterCharged;

	private static Texture2D ColonistOnlyCommandTex
	{
		get
		{
			if (colonistOnlyCommandTex == null)
			{
				colonistOnlyCommandTex = ContentFinder<Texture2D>.Get("UI/Gizmos/NeuralSupercharger_AllowGuests");
			}
			return colonistOnlyCommandTex;
		}
	}

	private CompProperties_NeuralSupercharger Props => (CompProperties_NeuralSupercharger)props;

	public bool CanAutoUse(Pawn pawn)
	{
		if (!allowGuests && pawn.IsQuestLodger())
		{
			return false;
		}
		switch (autoUseMode)
		{
		case AutoUseMode.NoAutoUse:
			return false;
		case AutoUseMode.AutoUseWithDesire:
			if (pawn.Ideo != null)
			{
				return pawn.Ideo.HasPrecept(PreceptDefOf.NeuralSupercharge_Preferred);
			}
			return false;
		case AutoUseMode.AutoUseForEveryone:
			return true;
		default:
			Log.Error($"Unknown auto use mode: {autoUseMode}");
			return false;
		}
	}

	public override void Initialize(CompProperties props)
	{
		base.Initialize(props);
		autoUseMode = AutoUseMode.AutoUseWithDesire;
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref autoUseMode, "autoUseMode", AutoUseMode.AutoUseWithDesire);
		Scribe_Values.Look(ref allowGuests, "allowGuests", defaultValue: false);
	}

	public override void CompTick()
	{
		base.CompTick();
		if (Props.effectCharged == null)
		{
			return;
		}
		if (parent.Spawned)
		{
			if (base.Charged)
			{
				if (effecterCharged == null)
				{
					effecterCharged = Props.effectCharged.Spawn();
					effecterCharged.Trigger(parent, new TargetInfo(parent.InteractionCell, parent.Map));
				}
				effecterCharged.EffectTick(parent, new TargetInfo(parent.InteractionCell, parent.Map));
			}
			else if (effecterCharged != null)
			{
				DespawnEffecter();
			}
		}
		else
		{
			DespawnEffecter();
		}
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		if (!ModLister.CheckIdeology("Neural supercharger"))
		{
			parent.Destroy();
		}
		else
		{
			base.PostSpawnSetup(respawningAfterLoad);
		}
	}

	private void DespawnEffecter()
	{
		effecterCharged?.Cleanup();
		effecterCharged = null;
	}

	[IteratorStateMachine(typeof(_003CCompFloatMenuOptions_003Ed__15))]
	public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompFloatMenuOptions_003Ed__15(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__selPawn = selPawn
		};
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__16))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__16(-2)
		{
			_003C_003E4__this = this
		};
	}
}
