using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld;

public class Bill_ResurrectMech : Bill_Mech
{
	[CompilerGenerated]
	private sealed class _003CAllAvailableResurrectionCorpses_003Ed__7 : IEnumerable<Corpse>, IEnumerable, IEnumerator<Corpse>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Corpse _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Bill_ResurrectMech _003C_003E4__this;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private List<Thing>.Enumerator _003C_003E7__wrap1;

		Corpse IEnumerator<Corpse>.Current
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
		public _003CAllAvailableResurrectionCorpses_003Ed__7(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<Thing>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Bill_ResurrectMech bill_ResurrectMech = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					List<Thing> list = bill_ResurrectMech.Map.listerThings.ThingsInGroup(ThingRequestGroup.Corpse);
					_003C_003E7__wrap1 = list.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				}
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					Corpse corpse = (Corpse)_003C_003E7__wrap1.Current;
					if (bill_ResurrectMech.recipe.fixedIngredientFilter.Allows(corpse) && !corpse.IsForbidden(pawn) && pawn.CanReach(corpse, PathEndMode.Touch, Danger.Deadly))
					{
						_003C_003E2__current = corpse;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<Thing>.Enumerator);
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Corpse> IEnumerable<Corpse>.GetEnumerator()
		{
			_003CAllAvailableResurrectionCorpses_003Ed__7 _003CAllAvailableResurrectionCorpses_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAllAvailableResurrectionCorpses_003Ed__ = this;
			}
			else
			{
				_003CAllAvailableResurrectionCorpses_003Ed__ = new _003CAllAvailableResurrectionCorpses_003Ed__7(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CAllAvailableResurrectionCorpses_003Ed__.pawn = _003C_003E3__pawn;
			return _003CAllAvailableResurrectionCorpses_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Corpse>)this).GetEnumerator();
		}
	}

	public override float BandwidthCost
	{
		get
		{
			if (base.Gestator.ActiveBill == this)
			{
				Pawn gestatingMech = base.Gestator.GestatingMech;
				if (gestatingMech != null)
				{
					return gestatingMech.GetStatValue(StatDefOf.BandwidthCost);
				}
				Corpse resurrectingMechCorpse = base.Gestator.ResurrectingMechCorpse;
				if (resurrectingMechCorpse != null)
				{
					return resurrectingMechCorpse.InnerPawn.GetStatValue(StatDefOf.BandwidthCost);
				}
			}
			return 0f;
		}
	}

	public Bill_ResurrectMech()
	{
	}

	public Bill_ResurrectMech(RecipeDef recipe, Precept_ThingStyle precept = null)
		: base(recipe, precept)
	{
	}

	public override bool PawnAllowedToStartAnew(Pawn p)
	{
		if (!base.PawnAllowedToStartAnew(p))
		{
			return false;
		}
		if (base.State == FormingState.Gathering && base.Gestator.ResurrectingMechCorpse == null)
		{
			IEnumerable<Corpse> source = AllAvailableResurrectionCorpses(p);
			if (source.Any() && !source.Any((Corpse c) => p.mechanitor.HasBandwidthToResurrect(c)))
			{
				JobFailReason.Is("NotEnoughBandwidth".Translate());
				return false;
			}
		}
		return true;
	}

	public override Thing CreateProducts()
	{
		Pawn innerPawn = base.Gestator.ResurrectingMechCorpse.InnerPawn;
		ResurrectionUtility.TryResurrect(innerPawn, new ResurrectionParams
		{
			dontSpawn = true
		});
		innerPawn.needs.energy.CurLevel = innerPawn.needs.energy.MaxLevel * 0.5f;
		innerPawn.health.RemoveAllHediffs();
		base.BoundPawn.relations.AddDirectRelation(PawnRelationDefOf.Overseer, innerPawn);
		if (innerPawn.IsWorldPawn())
		{
			Find.WorldPawns.RemovePawn(innerPawn);
		}
		return innerPawn;
	}

	public override bool IsFixedOrAllowedIngredient(Thing thing)
	{
		if (!base.IsFixedOrAllowedIngredient(thing))
		{
			return false;
		}
		if (thing is Corpse corpse && corpse.InnerPawn.Faction != Faction.OfPlayer)
		{
			return false;
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CAllAvailableResurrectionCorpses_003Ed__7))]
	private IEnumerable<Corpse> AllAvailableResurrectionCorpses(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllAvailableResurrectionCorpses_003Ed__7(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__pawn = pawn
		};
	}

	public override void AppendInspectionData(StringBuilder sb)
	{
		switch (base.State)
		{
		case FormingState.Gathering:
			AppendCurrentIngredientCount(sb);
			break;
		case FormingState.Forming:
			if (base.Gestator?.ResurrectingMechCorpse != null)
			{
				sb.AppendLine("ResurrectingMech".Translate(base.Gestator.ResurrectingMechCorpse.InnerPawn.LabelCap));
			}
			break;
		case FormingState.Formed:
			if (base.Gestator?.GestatingMech != null && base.BoundPawn != null)
			{
				sb.AppendLine("ResurrectedMech".Translate(base.Gestator.GestatingMech.LabelCap, base.BoundPawn.Named("PAWN")) + " (" + "GestatedMechRequiresMechanitor".Translate(base.BoundPawn.Named("PAWN")) + ")");
			}
			break;
		}
		base.AppendInspectionData(sb);
	}
}
