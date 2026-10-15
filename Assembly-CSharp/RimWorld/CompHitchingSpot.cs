using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld;

public class CompHitchingSpot : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__8 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompHitchingSpot _003C_003E4__this;

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
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			CompHitchingSpot CS_0024_003C_003E8__locals3 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals3.parent.Faction == Faction.OfPlayer)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "CommandFormCaravan".Translate(),
						defaultDesc = "CommandFormCaravanDesc".Translate() + "\n\n" + "CommandFormCaravanDescAtSpot".Translate(),
						icon = FormCaravanComp.FormCaravanCommand,
						action = delegate
						{
							Find.WindowStack.Add(new Dialog_FormCaravan(CS_0024_003C_003E8__locals3.parent.Map, reform: false, null, mapAboutToBeRemoved: false, CS_0024_003C_003E8__locals3.parent.Position));
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

	private static readonly Vector3 HitchAdustment = new Vector3(0f, 0f, 0.15f);

	private List<Pawn> ropedPawns = new List<Pawn>();

	private Graphic hitchGraphic;

	private Graphic HitchGraphic
	{
		get
		{
			if (hitchGraphic == null)
			{
				hitchGraphic = GraphicDatabase.Get<Graphic_Single>("Things/Building/Misc/CaravanHitchingPin", ShaderDatabase.Cutout, Vector2.one, Color.white);
			}
			return hitchGraphic;
		}
	}

	public void AddPawn(Pawn ropedPawn)
	{
		ropedPawns.Add(ropedPawn);
	}

	public void RemovePawn(Pawn unropedPawn)
	{
		ropedPawns.Remove(unropedPawn);
	}

	public override void PostDraw()
	{
		base.PostDraw();
		if (ropedPawns.Count > 0)
		{
			HitchGraphic.DrawFromDef(parent.DrawPos + HitchAdustment + Altitudes.AltIncVect * 0.25f, Rot4.North, null);
		}
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

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Collections.Look(ref ropedPawns, "ropedPawns", LookMode.Reference);
		if (Scribe.mode != LoadSaveMode.PostLoadInit)
		{
			return;
		}
		foreach (Pawn ropedPawn in ropedPawns)
		{
			if (ropedPawn == null || ropedPawn.roping == null || ropedPawn.roping.RopedToHitchingSpot != parent)
			{
				RemovePawn(ropedPawn);
			}
		}
	}
}
