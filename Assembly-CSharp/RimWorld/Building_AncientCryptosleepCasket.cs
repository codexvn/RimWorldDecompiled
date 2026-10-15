using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI.Group;

namespace RimWorld;

public class Building_AncientCryptosleepCasket : Building_CryptosleepCasket
{
	[CompilerGenerated]
	private sealed class _003CUnopenedCasketsInGroup_003Ed__4 : IEnumerable<Building_AncientCryptosleepCasket>, IEnumerable, IEnumerator<Building_AncientCryptosleepCasket>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Building_AncientCryptosleepCasket _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_AncientCryptosleepCasket _003C_003E4__this;

		private List<Thing>.Enumerator _003C_003E7__wrap1;

		Building_AncientCryptosleepCasket IEnumerator<Building_AncientCryptosleepCasket>.Current
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
		public _003CUnopenedCasketsInGroup_003Ed__4(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 2)
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
				Building_AncientCryptosleepCasket building_AncientCryptosleepCasket = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E2__current = building_AncientCryptosleepCasket;
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					if (building_AncientCryptosleepCasket.groupID == -1)
					{
						break;
					}
					_003C_003E7__wrap1 = building_AncientCryptosleepCasket.Map.listerThings.ThingsOfDef(ThingDefOf.AncientCryptosleepCasket).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00c1;
				case 2:
					{
						_003C_003E1__state = -3;
						goto IL_00c1;
					}
					IL_00c1:
					while (_003C_003E7__wrap1.MoveNext())
					{
						Building_AncientCryptosleepCasket building_AncientCryptosleepCasket2 = _003C_003E7__wrap1.Current as Building_AncientCryptosleepCasket;
						if (building_AncientCryptosleepCasket2.groupID == building_AncientCryptosleepCasket.groupID && !building_AncientCryptosleepCasket2.contentsKnown)
						{
							_003C_003E2__current = building_AncientCryptosleepCasket2;
							_003C_003E1__state = 2;
							return true;
						}
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<Thing>.Enumerator);
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Building_AncientCryptosleepCasket> IEnumerable<Building_AncientCryptosleepCasket>.GetEnumerator()
		{
			_003CUnopenedCasketsInGroup_003Ed__4 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CUnopenedCasketsInGroup_003Ed__4(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Building_AncientCryptosleepCasket>)this).GetEnumerator();
		}
	}

	public int groupID = -1;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref groupID, "groupID", 0);
	}

	public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
	{
		base.PreApplyDamage(ref dinfo, out absorbed);
		if (absorbed)
		{
			return;
		}
		if (!contentsKnown && innerContainer.Count > 0 && dinfo.Def.harmsHealth && dinfo.Instigator != null && dinfo.Instigator.Faction != null)
		{
			bool flag = false;
			foreach (Thing item in (IEnumerable<Thing>)innerContainer)
			{
				if (item is Pawn)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				EjectContents();
			}
		}
		absorbed = false;
	}

	public override void EjectContents()
	{
		bool num = contentsKnown;
		List<Thing> list = null;
		if (!num)
		{
			list = new List<Thing>();
			list.AddRange(innerContainer);
			list.AddRange(UnopenedCasketsInGroup().SelectMany((Building_AncientCryptosleepCasket c) => c.innerContainer));
			list.RemoveDuplicates();
		}
		base.EjectContents();
		if ((bool)ClaimableBy(Faction.OfPlayer))
		{
			SetFaction(null);
		}
		if (num)
		{
			return;
		}
		ThingDef filth_Slime = ThingDefOf.Filth_Slime;
		FilthMaker.TryMakeFilth(base.Position, base.Map, filth_Slime, Rand.Range(8, 12));
		foreach (Building_AncientCryptosleepCasket item in UnopenedCasketsInGroup())
		{
			item.contentsKnown = true;
			item.EjectContents();
		}
		IEnumerable<Pawn> enumerable = from p in list.OfType<Pawn>().ToList()
			where p.RaceProps.Humanlike && p.GetLord() == null && p.Faction == Faction.OfAncientsHostile
			select p;
		if (enumerable.Any())
		{
			LordMaker.MakeNewLord(Faction.OfAncientsHostile, new LordJob_AssaultColony(Faction.OfAncientsHostile, canKidnap: false, canTimeoutOrFlee: true, sappers: false, useAvoidGridSmart: false, canSteal: false), base.Map, enumerable);
		}
	}

	[IteratorStateMachine(typeof(_003CUnopenedCasketsInGroup_003Ed__4))]
	private IEnumerable<Building_AncientCryptosleepCasket> UnopenedCasketsInGroup()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CUnopenedCasketsInGroup_003Ed__4(-2)
		{
			_003C_003E4__this = this
		};
	}
}
