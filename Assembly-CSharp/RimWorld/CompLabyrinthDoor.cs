using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class CompLabyrinthDoor : CompInteractable, IJammedDoorDrawer
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass7_0
	{
		public CompLabyrinthDoor _003C_003E4__this;

		public Pawn selPawn;

		internal void _003CCompFloatMenuOptions_003Eb__0()
		{
			_003C_003E4__this.OrderActivation(selPawn);
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompFloatMenuOptions_003Ed__7 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompLabyrinthDoor _003C_003E4__this;

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
		public _003CCompFloatMenuOptions_003Ed__7(int _003C_003E1__state)
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
			CompLabyrinthDoor compLabyrinthDoor = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass7_0 CS_0024_003C_003E8__locals3 = new _003C_003Ec__DisplayClass7_0
				{
					_003C_003E4__this = _003C_003E4__this,
					selPawn = selPawn
				};
				if (!compLabyrinthDoor.Door.Jammed)
				{
					return false;
				}
				AcceptanceReport acceptanceReport = compLabyrinthDoor.CanInteract(CS_0024_003C_003E8__locals3.selPawn);
				FloatMenuOption floatMenuOption = new FloatMenuOption(compLabyrinthDoor.Props.jobString.CapitalizeFirst(), delegate
				{
					CS_0024_003C_003E8__locals3._003C_003E4__this.OrderActivation(CS_0024_003C_003E8__locals3.selPawn);
				});
				if (!acceptanceReport.Accepted)
				{
					floatMenuOption.Disabled = true;
					floatMenuOption.Label = floatMenuOption.Label + " (" + acceptanceReport.Reason + ")";
				}
				_003C_003E2__current = floatMenuOption;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
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
			_003CCompFloatMenuOptions_003Ed__7 _003CCompFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCompFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CCompFloatMenuOptions_003Ed__ = new _003CCompFloatMenuOptions_003Ed__7(0)
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
	private sealed class _003CCompGetGizmosExtra_003Ed__8 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompLabyrinthDoor _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__8(int _003C_003E1__state)
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
				CompLabyrinthDoor compLabyrinthDoor = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (!compLabyrinthDoor.Door.Jammed)
					{
						return false;
					}
					_003C_003E7__wrap1 = ((CompInteractable)compLabyrinthDoor).CompGetGizmosExtra().GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					Gizmo current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__8(0)
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

	public Building_JammedDoor Door => (Building_JammedDoor)parent;

	public new CompProperties_LabyrinthDoor Props => (CompProperties_LabyrinthDoor)props;

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		if (!ModLister.CheckAnomaly("Labyrinth door"))
		{
			parent.Destroy();
			return;
		}
		base.PostSpawnSetup(respawningAfterLoad);
		if (!respawningAfterLoad && Rand.Chance(Props.unlockedChance))
		{
			Door.UnlockDoor();
		}
	}

	public override void OrderForceTarget(LocalTargetInfo target)
	{
		OrderActivation(target.Pawn);
	}

	public override string CompInspectStringExtra()
	{
		if (!Door.Jammed)
		{
			return null;
		}
		return "GrayDoorJammed".Translate();
	}

	[IteratorStateMachine(typeof(_003CCompFloatMenuOptions_003Ed__7))]
	public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompFloatMenuOptions_003Ed__7(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__selPawn = selPawn
		};
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__8))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__8(-2)
		{
			_003C_003E4__this = this
		};
	}

	protected override void OnInteracted(Pawn caster)
	{
		Door.UnlockAndOpenDoor();
		if (caster.IsColonist)
		{
			parent.Map.fogGrid.FloodUnfogAdjacent(parent.Position, sendLetters: false);
		}
	}

	private void OrderActivation(Pawn pawn)
	{
		Job job = JobMaker.MakeJob(JobDefOf.InteractThing, parent);
		job.count = 1;
		job.playerForced = true;
		pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
	}

	public void DrawJammed(Rot4 rotation)
	{
		Vector3 drawPos = Door.DrawPos;
		drawPos.y = AltitudeLayer.DoorMoveable.AltitudeFor();
		Props.jammed.Graphic.Draw(drawPos, rotation, Door);
	}
}
