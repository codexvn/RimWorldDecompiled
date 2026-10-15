using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public abstract class CompScanner : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__15 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompScanner _003C_003E4__this;

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
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			CompScanner CS_0024_003C_003E8__locals1 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (DebugSettings.ShowDevGizmos)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Find now",
						action = delegate
						{
							CS_0024_003C_003E8__locals1.DoFind(PawnsFinder.AllMaps_FreeColonists.RandomElement());
						}
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

	protected float daysWorkingSinceLastFinding;

	protected float lastUserSpeed = 1f;

	protected float lastScanTick = -1f;

	protected CompPowerTrader powerComp;

	protected CompForbiddable forbiddable;

	public CompProperties_Scanner Props => (CompProperties_Scanner)props;

	public virtual AcceptanceReport CanUseNow
	{
		get
		{
			if (!parent.Spawned)
			{
				return false;
			}
			if (powerComp != null && !powerComp.PowerOn)
			{
				return false;
			}
			if (RoofUtility.IsAnyCellUnderRoof(parent))
			{
				return "CannotUseScannerRoofed".Translate();
			}
			if (forbiddable != null && forbiddable.Forbidden)
			{
				return false;
			}
			return parent.Faction == Faction.OfPlayer;
		}
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref daysWorkingSinceLastFinding, "daysWorkingSinceLastFinding", 0f);
		Scribe_Values.Look(ref lastUserSpeed, "lastUserSpeed", 0f);
		Scribe_Values.Look(ref lastScanTick, "lastScanTick", 0f);
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		base.PostSpawnSetup(respawningAfterLoad);
		powerComp = parent.GetComp<CompPowerTrader>();
		forbiddable = parent.GetComp<CompForbiddable>();
	}

	public void Used(Pawn worker)
	{
		if (!CanUseNow)
		{
			Log.Error("Used while CanUseNow is false.");
		}
		lastScanTick = Find.TickManager.TicksGame;
		lastUserSpeed = 1f;
		if (Props.scanSpeedStat != null)
		{
			lastUserSpeed = worker.GetStatValue(Props.scanSpeedStat);
		}
		daysWorkingSinceLastFinding += lastUserSpeed / 60000f;
		if (TickDoesFind(lastUserSpeed))
		{
			DoFind(worker);
			daysWorkingSinceLastFinding = 0f;
		}
	}

	protected virtual bool TickDoesFind(float scanSpeed)
	{
		if (parent.IsHashIntervalTick(59) && (Rand.MTBEventOccurs(Props.scanFindMtbDays / scanSpeed, 60000f, 59f) || (Props.scanFindGuaranteedDays > 0f && daysWorkingSinceLastFinding >= Props.scanFindGuaranteedDays)))
		{
			return true;
		}
		return false;
	}

	public override string CompInspectStringExtra()
	{
		string text = "";
		if (lastScanTick > (float)(Find.TickManager.TicksGame - 30))
		{
			text += "UserScanAbility".Translate() + ": " + lastUserSpeed.ToStringPercent() + "\n" + "ScanAverageInterval".Translate() + ": " + "PeriodDays".Translate((Props.scanFindMtbDays / lastUserSpeed).ToString("F1")) + "\n";
		}
		return text + "ScanningProgressToGuaranteedFind".Translate() + ": " + (daysWorkingSinceLastFinding / Props.scanFindGuaranteedDays).ToStringPercent();
	}

	protected abstract void DoFind(Pawn worker);

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__15))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__15(-2)
		{
			_003C_003E4__this = this
		};
	}
}
