using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CompBreakdownable : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__16 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompBreakdownable _003C_003E4__this;

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
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			CompBreakdownable compBreakdownable = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (DebugSettings.ShowDevGizmos && compBreakdownable.BrokenDown)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Fix breakdown",
						action = compBreakdownable.Notify_Repaired
					};
					_003C_003E1__state = 1;
					return true;
				}
				break;
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

	private bool brokenDownInt;

	private CompPowerTrader powerComp;

	private const int BreakdownMTBTicks = 13680000;

	public const string BreakdownSignal = "Breakdown";

	private OverlayHandle? overlayBrokenDown;

	public bool BrokenDown => brokenDownInt;

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref brokenDownInt, "brokenDown", defaultValue: false);
	}

	private void UpdateOverlays()
	{
		if (parent.Spawned)
		{
			parent.Map.overlayDrawer.Disable(parent, ref overlayBrokenDown);
			if (brokenDownInt)
			{
				overlayBrokenDown = parent.Map.overlayDrawer.Enable(parent, OverlayTypes.BrokenDown);
			}
		}
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		base.PostSpawnSetup(respawningAfterLoad);
		powerComp = parent.GetComp<CompPowerTrader>();
		parent.Map.GetComponent<BreakdownManager>().Register(this);
		UpdateOverlays();
	}

	public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
	{
		base.PostDeSpawn(map, mode);
		map.GetComponent<BreakdownManager>().Deregister(this);
	}

	public void CheckForBreakdown()
	{
		if (CanBreakdownNow() && Rand.MTBEventOccurs(13680000f, 1f, 1041f))
		{
			DoBreakdown();
		}
	}

	protected bool CanBreakdownNow()
	{
		if (!BrokenDown)
		{
			if (powerComp != null)
			{
				return powerComp.PowerOn;
			}
			return true;
		}
		return false;
	}

	public void Notify_Repaired()
	{
		brokenDownInt = false;
		parent.Map.GetComponent<BreakdownManager>().Notify_Repaired(parent);
		if (parent is Building_PowerSwitch)
		{
			parent.Map.powerNetManager.Notfiy_TransmitterTransmitsPowerNowChanged(parent.GetComp<CompPower>());
		}
		UpdateOverlays();
	}

	public void DoBreakdown()
	{
		brokenDownInt = true;
		parent.BroadcastCompSignal("Breakdown");
		parent.Map.GetComponent<BreakdownManager>().Notify_BrokenDown(parent);
		UpdateOverlays();
	}

	public override string CompInspectStringExtra()
	{
		if (BrokenDown)
		{
			return "BrokenDown".Translate();
		}
		return null;
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
