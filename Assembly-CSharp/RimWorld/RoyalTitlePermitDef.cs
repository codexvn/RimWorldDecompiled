using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class RoyalTitlePermitDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__17 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RoyalTitlePermitDef _003C_003E4__this;

		private int _003Ci_003E5__2;

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
		public _003CConfigErrors_003Ed__17(int _003C_003E1__state)
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
			RoyalTitlePermitDef royalTitlePermitDef = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!typeof(RoyalTitlePermitWorker).IsAssignableFrom(royalTitlePermitDef.workerClass))
				{
					_003C_003E2__current = $"RoyalTitlePermitDef {royalTitlePermitDef.defName} has worker class {royalTitlePermitDef.workerClass}, which is not deriving from {typeof(RoyalTitlePermitWorker).FullName}";
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0087;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0087;
			case 2:
				_003C_003E1__state = -1;
				goto IL_00c8;
			case 3:
				_003C_003E1__state = -1;
				goto IL_0122;
			case 4:
				_003C_003E1__state = -1;
				goto IL_015a;
			case 5:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_016a:
				if (_003Ci_003E5__2 < royalTitlePermitDef.royalAid.itemsToDrop.Count)
				{
					if (royalTitlePermitDef.royalAid.itemsToDrop[_003Ci_003E5__2].count <= 0)
					{
						_003C_003E2__current = "item count should be greater than 0.";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_0122;
				}
				goto IL_0185;
				IL_0087:
				if (royalTitlePermitDef.royalAid == null)
				{
					break;
				}
				if (royalTitlePermitDef.royalAid.pawnKindDef != null && royalTitlePermitDef.royalAid.pawnCount <= 0)
				{
					_003C_003E2__current = "pawnCount should be greater than 0, if you specify pawnKindDef";
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_00c8;
				IL_015a:
				_003Ci_003E5__2++;
				goto IL_016a;
				IL_0185:
				if (royalTitlePermitDef.royalAid.favorCost <= 0)
				{
					_003C_003E2__current = "favor cost should be greater than 0.";
					_003C_003E1__state = 5;
					return true;
				}
				break;
				IL_0122:
				if (royalTitlePermitDef.royalAid.itemsToDrop[_003Ci_003E5__2].thingDef == null)
				{
					_003C_003E2__current = "thingDef not defined.";
					_003C_003E1__state = 4;
					return true;
				}
				goto IL_015a;
				IL_00c8:
				if (!royalTitlePermitDef.royalAid.itemsToDrop.NullOrEmpty())
				{
					_003Ci_003E5__2 = 0;
					goto IL_016a;
				}
				goto IL_0185;
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
			_003CConfigErrors_003Ed__17 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__17(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public Type workerClass = typeof(RoyalTitlePermitWorker);

	public RoyalAid royalAid;

	public float cooldownDays;

	public RoyalTitleDef minTitle;

	public int permitPointCost;

	public FactionDef faction;

	public bool usableOnWorldMap;

	public RoyalTitlePermitDef prerequisite;

	public Vector2 uiPosition;

	public List<PlanetLayerDef> layerBlacklist = new List<PlanetLayerDef>();

	private RoyalTitlePermitWorker worker;

	public int CooldownTicks => (int)(cooldownDays * 60000f);

	public RoyalTitlePermitWorker Worker
	{
		get
		{
			if (worker == null)
			{
				worker = (RoyalTitlePermitWorker)Activator.CreateInstance(workerClass);
				worker.def = this;
			}
			return worker;
		}
	}

	public bool AvailableForPawn(Pawn pawn, Faction faction)
	{
		if (pawn.royalty == null)
		{
			return false;
		}
		if (pawn.royalty.HasPermit(this, faction))
		{
			return false;
		}
		if (prerequisite != null && !pawn.royalty.HasPermit(prerequisite, faction))
		{
			return false;
		}
		if (pawn.royalty.GetPermitPoints(faction) < permitPointCost)
		{
			return false;
		}
		RoyalTitleDef currentTitle = pawn.royalty.GetCurrentTitle(faction);
		if (currentTitle == null && minTitle == null)
		{
			return true;
		}
		if (currentTitle == null && minTitle != null)
		{
			return false;
		}
		return currentTitle.seniority >= minTitle.seniority;
	}

	public bool IsPrerequisiteOfHeldPermit(Pawn pawn, Faction faction)
	{
		List<FactionPermit> allFactionPermits = pawn.royalty.AllFactionPermits;
		for (int i = 0; i < allFactionPermits.Count; i++)
		{
			if (allFactionPermits[i].Permit.prerequisite == this && allFactionPermits[i].Faction == faction)
			{
				return true;
			}
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__17))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__17(-2)
		{
			_003C_003E4__this = this
		};
	}
}
