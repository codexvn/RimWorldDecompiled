using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class ScenPart_OnPawnDeathExplode : ScenPart
{
	[CompilerGenerated]
	private sealed class _003CPossibleDamageDefs_003Ed__8 : IEnumerable<DamageDef>, IEnumerable, IEnumerator<DamageDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private DamageDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		DamageDef IEnumerator<DamageDef>.Current
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
		public _003CPossibleDamageDefs_003Ed__8(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = DamageDefOf.Bomb;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = DamageDefOf.Flame;
				_003C_003E1__state = 2;
				return true;
			case 2:
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
		IEnumerator<DamageDef> IEnumerable<DamageDef>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CPossibleDamageDefs_003Ed__8(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<DamageDef>)this).GetEnumerator();
		}
	}

	private float radius = 5.9f;

	private DamageDef damage;

	private string radiusBuf;

	public override void Randomize()
	{
		radius = (float)Rand.RangeInclusive(3, 8) - 0.1f;
		damage = PossibleDamageDefs().RandomElement();
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref radius, "radius", 0f);
		Scribe_Defs.Look(ref damage, "damage");
	}

	public override string Summary(Scenario scen)
	{
		return "ScenPart_OnPawnDeathExplode".Translate(damage.label, radius.ToString());
	}

	public override void DoEditInterface(Listing_ScenEdit listing)
	{
		Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f);
		Widgets.TextFieldNumericLabeled(scenPartRect.TopHalf(), "radius".Translate(), ref radius, ref radiusBuf);
		if (!Widgets.ButtonText(scenPartRect.BottomHalf(), damage.LabelCap, drawBackground: true, doMouseoverSound: true, active: true, null))
		{
			return;
		}
		FloatMenuUtility.MakeMenu(PossibleDamageDefs(), (DamageDef d) => d.LabelCap, (DamageDef d) => delegate
		{
			damage = d;
		});
	}

	public override void Notify_PawnDied(Corpse corpse)
	{
		if (corpse.Spawned)
		{
			GenExplosion.DoExplosion(corpse.Position, corpse.Map, radius, damage, null, -1, -1f, null, null, null, null, null, 0f, 1, null, null, 255, applyDamageToExplosionCellsNeighbors: false, null, 0f, 1, 0f, damageFalloff: false, null, null, null);
		}
	}

	[IteratorStateMachine(typeof(_003CPossibleDamageDefs_003Ed__8))]
	private IEnumerable<DamageDef> PossibleDamageDefs()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPossibleDamageDefs_003Ed__8(-2);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode() ^ ((damage != null) ? damage.GetHashCode() : 0) ^ radius.GetHashCode();
	}
}
