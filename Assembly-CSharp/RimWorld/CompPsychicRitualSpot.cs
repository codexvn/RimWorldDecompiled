using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;
using Verse.AI.Group;

namespace RimWorld;

public class CompPsychicRitualSpot : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__12 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompPsychicRitualSpot _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__12(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				CompPsychicRitualSpot compPsychicRitualSpot = _003C_003E4__this;
				PsychicRitual psychicRitual;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingComp)compPsychicRitualSpot).CompGetGizmosExtra().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007c;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007c;
				case 2:
					_003C_003E1__state = -1;
					goto IL_015a;
				case 3:
					_003C_003E1__state = -4;
					goto IL_0140;
				case 4:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_007c:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					psychicRitual = compPsychicRitualSpot.GetPsychicRitual();
					if (psychicRitual != null)
					{
						_003C_003E2__current = PsychicRitualGizmo.CancelGizmo(psychicRitual);
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003E7__wrap1 = PsychicRitualGizmo.GetGizmos(compPsychicRitualSpot.parent).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0140;
					IL_0140:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current2 = _003C_003E7__wrap1.Current;
						if (compPsychicRitualSpot.Obstructed)
						{
							current2.Disabled = true;
							current2.disabledReason = "PsychicRitualAreaObstructed".Translate().CapitalizeFirst();
						}
						_003C_003E2__current = current2;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					goto IL_015a;
					IL_015a:
					if (DebugSettings.ShowDevGizmos)
					{
						Command_Action command_Action = new Command_Action
						{
							defaultLabel = "DEV: Reset psychic ritual cooldowns",
							action = delegate
							{
								Find.PsychicRitualManager.ClearAllCooldowns();
							}
						};
						_003C_003E2__current = command_Action;
						_003C_003E1__state = 4;
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

		private void _003C_003Em__Finally2()
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
			_003CCompGetGizmosExtra_003Ed__12 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__12(0)
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

	private List<Thing> obstructingThings = new List<Thing>();

	private HashSet<IntVec3> occupiedCells;

	private bool Obstructed => obstructingThings.Count > 0;

	public HashSet<IntVec3> OccupiedCells => occupiedCells;

	public Lord GetLord()
	{
		if (parent is Building b)
		{
			return b.GetLord();
		}
		if (parent is Pawn p)
		{
			return p.GetLord();
		}
		if (parent is Corpse c)
		{
			return c.GetLord();
		}
		return null;
	}

	public PsychicRitual GetPsychicRitual()
	{
		return (GetLord()?.CurLordToil as LordToil_PsychicRitual)?.RitualData.psychicRitual;
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		base.PostSpawnSetup(respawningAfterLoad);
		occupiedCells = new HashSet<IntVec3>(GenRadial.RadialCellsAround(parent.Position, (float)parent.def.Size.x + 0.9f, useCenter: true));
	}

	public override string CompInspectStringExtra()
	{
		StringBuilder stringBuilder = new StringBuilder(base.CompInspectStringExtra());
		if (Obstructed)
		{
			stringBuilder.AppendInNewLine("PsychicRitualSpotObstructed".Translate());
		}
		Lord lord = GetLord();
		if (lord != null && lord.CurLordToil is LordToil_PsychicRitual lordToil_PsychicRitual)
		{
			PsychicRitual psychicRitual = lordToil_PsychicRitual.RitualData.psychicRitual;
			PsychicRitualToil psychicRitualToil = lordToil_PsychicRitual.RitualData.psychicRitualToil;
			stringBuilder.AppendInNewLine($"{"PsychicRitual".Translate().CapitalizeFirst()}: {psychicRitual.def.LabelCap}");
			stringBuilder.AppendInNewLine(psychicRitualToil.GetReport(psychicRitual, null).CapitalizeFirst().EndWithPeriod());
		}
		return stringBuilder.ToString();
	}

	public override void PostDrawExtraSelectionOverlays()
	{
		base.PostDrawExtraSelectionOverlays();
		if (parent.Faction != Faction.OfPlayer)
		{
			return;
		}
		foreach (Thing obstructingThing in obstructingThings)
		{
			GenDraw.DrawLineBetween(parent.TrueCenter(), obstructingThing.TrueCenter(), SimpleColor.Red);
		}
	}

	public override void CompTick()
	{
		base.CompTick();
		obstructingThings.Clear();
		foreach (IntVec3 occupiedCell in occupiedCells)
		{
			List<Thing> list = parent.Map.thingGrid.ThingsListAt(occupiedCell);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.passability != 0)
				{
					obstructingThings.Add(list[i]);
				}
			}
		}
		PsychicRitual psychicRitual = GetPsychicRitual();
		if (psychicRitual != null && parent.Faction == Faction.OfPlayer && Obstructed)
		{
			psychicRitual.CancelPsychicRitual("PsychicRitualAreaObstructed".Translate());
		}
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__12))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__12(-2)
		{
			_003C_003E4__this = this
		};
	}
}
