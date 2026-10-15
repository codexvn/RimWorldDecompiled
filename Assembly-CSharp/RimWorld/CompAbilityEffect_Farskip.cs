using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

public class CompAbilityEffect_Farskip : CompAbilityEffect
{
	[CompilerGenerated]
	private sealed class _003CGetPreCastActions_003Ed__3 : IEnumerable<PreCastAction>, IEnumerable, IEnumerator<PreCastAction>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private PreCastAction _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompAbilityEffect_Farskip _003C_003E4__this;

		PreCastAction IEnumerator<PreCastAction>.Current
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
		public _003CGetPreCastActions_003Ed__3(int _003C_003E1__state)
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
			CompAbilityEffect_Farskip CS_0024_003C_003E8__locals1 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = new PreCastAction
				{
					action = delegate
					{
						foreach (Pawn item in CS_0024_003C_003E8__locals1.PawnsToSkip())
						{
							FleckCreationData dataAttachedOverlay = FleckMaker.GetDataAttachedOverlay(item, FleckDefOf.PsycastSkipFlashEntry, new Vector3(-0.5f, 0f, -0.5f));
							dataAttachedOverlay.link.detachAfterTicks = 5;
							item.Map.flecks.CreateFleck(dataAttachedOverlay);
						}
					},
					ticksAwayFromCast = 5
				};
				_003C_003E1__state = 1;
				return true;
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
		IEnumerator<PreCastAction> IEnumerable<PreCastAction>.GetEnumerator()
		{
			_003CGetPreCastActions_003Ed__3 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetPreCastActions_003Ed__3(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<PreCastAction>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CPawnsToSkip_003Ed__4 : IEnumerable<Pawn>, IEnumerable, IEnumerator<Pawn>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Pawn _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompAbilityEffect_Farskip _003C_003E4__this;

		private List<Pawn>.Enumerator _003C_003E7__wrap1;

		private bool _003ChomeMap_003E5__3;

		private bool _003CsentLodgerMessage_003E5__4;

		private IEnumerator<Thing> _003C_003E7__wrap4;

		Pawn IEnumerator<Pawn>.Current
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
		public _003CPawnsToSkip_003Ed__4(int _003C_003E1__state)
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
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = default(List<Pawn>.Enumerator);
			_003C_003E7__wrap4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				CompAbilityEffect_Farskip compAbilityEffect_Farskip = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					Caravan caravan = compAbilityEffect_Farskip.parent.pawn.GetCaravan();
					if (caravan != null)
					{
						_003C_003E7__wrap1 = caravan.pawns.GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_0088;
					}
					_003ChomeMap_003E5__3 = compAbilityEffect_Farskip.parent.pawn.Map.IsPlayerHome;
					_003CsentLodgerMessage_003E5__4 = false;
					_003C_003E7__wrap4 = GenRadial.RadialDistinctThingsAround(compAbilityEffect_Farskip.parent.pawn.Position, compAbilityEffect_Farskip.parent.pawn.Map, compAbilityEffect_Farskip.parent.def.EffectRadius, useCenter: true).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_01d7;
				}
				case 1:
					_003C_003E1__state = -3;
					goto IL_0088;
				case 2:
					{
						_003C_003E1__state = -4;
						goto IL_01d7;
					}
					IL_0088:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Pawn current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<Pawn>.Enumerator);
					break;
					IL_01d7:
					while (_003C_003E7__wrap4.MoveNext())
					{
						if (!(_003C_003E7__wrap4.Current is Pawn pawn))
						{
							continue;
						}
						if (pawn.IsQuestLodger())
						{
							if (!_003CsentLodgerMessage_003E5__4)
							{
								Messages.Message("MessageLodgersCantFarskip".Translate(), pawn, MessageTypeDefOf.NegativeEvent, historical: false);
							}
							_003CsentLodgerMessage_003E5__4 = true;
						}
						else if (!pawn.Dead && (pawn.IsColonist || pawn.IsPrisonerOfColony || (!_003ChomeMap_003E5__3 && pawn.IsAnimal && pawn.Faction != null && pawn.Faction.IsPlayer) || pawn.IsColonySubhuman))
						{
							_003C_003E2__current = pawn;
							_003C_003E1__state = 2;
							return true;
						}
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap4 = null;
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

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap4 != null)
			{
				_003C_003E7__wrap4.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
		{
			_003CPawnsToSkip_003Ed__4 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CPawnsToSkip_003Ed__4(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Pawn>)this).GetEnumerator();
		}
	}

	private new CompProperties_AbilityFarskip Props => (CompProperties_AbilityFarskip)props;

	public override void Apply(GlobalTargetInfo target)
	{
		Caravan caravan = parent.pawn.GetCaravan();
		Map targetMap = (target.WorldObject as MapParent)?.Map;
		IntVec3 targetCell = IntVec3.Invalid;
		List<Pawn> list = PawnsToSkip().ToList();
		if (parent.pawn.Spawned)
		{
			foreach (Pawn item in list)
			{
				parent.AddEffecterToMaintain(EffecterDefOf.Skip_Entry.Spawn(item, item.Map), item.Position, 60);
			}
			SoundDefOf.Psycast_Skip_Pulse.PlayOneShot(new TargetInfo(target.Cell, parent.pawn.Map));
		}
		if (ShouldEnterMap(target))
		{
			Pawn pawn = AlliedPawnOnMap(targetMap);
			if (pawn != null)
			{
				targetCell = pawn.Position;
			}
			else
			{
				targetCell = parent.pawn.Position;
			}
		}
		if (targetCell.IsValid)
		{
			foreach (Pawn item2 in list)
			{
				if (item2.Spawned)
				{
					item2.teleporting = true;
					item2.ExitMap(allowedToJoinOrCreateCaravan: false, Rot4.Invalid);
					AbilityUtility.DoClamor(item2.Position, Props.clamorRadius, parent.pawn, Props.clamorType);
					item2.teleporting = false;
				}
				CellFinder.TryFindRandomSpawnCellForPawnNear(targetCell, targetMap, out var result, 4, (IntVec3 cell) => cell != targetCell && cell.GetRoom(targetMap) == targetCell.GetRoom(targetMap));
				GenSpawn.Spawn(item2, result, targetMap);
				if (item2.drafter != null && item2.IsColonistPlayerControlled && !item2.Downed)
				{
					item2.drafter.Drafted = true;
				}
				item2.stances.stunner.StunFor(Props.stunTicks.RandomInRange, parent.pawn, addBattleLog: false);
				item2.Notify_Teleported();
				CompAbilityEffect_Teleport.SendSkipUsedSignal(item2, parent.pawn);
				if (item2.IsPrisoner)
				{
					item2.guest.WaitInsteadOfEscapingForDefaultTicks();
				}
				parent.AddEffecterToMaintain(EffecterDefOf.Skip_ExitNoDelay.Spawn(item2, item2.Map), item2.Position, 60, targetMap);
				SoundDefOf.Psycast_Skip_Exit.PlayOneShot(new TargetInfo(result, item2.Map));
				if ((item2.IsColonist || item2.RaceProps.packAnimal || item2.IsColonyMech) && item2.Map.IsPlayerHome)
				{
					item2.inventory.UnloadEverything = true;
				}
			}
			caravan?.Destroy();
			return;
		}
		if (target.WorldObject is Caravan caravan2 && caravan2.Faction == parent.pawn.Faction)
		{
			if (caravan != null)
			{
				caravan.pawns.TryTransferAllToContainer(caravan2.pawns);
				caravan2.Notify_Merged(new List<Caravan> { caravan });
				caravan.Destroy();
				return;
			}
			{
				foreach (Pawn item3 in list)
				{
					caravan2.AddPawn(item3, addCarriedPawnToWorldPawnsIfAny: true);
					item3.ExitMap(allowedToJoinOrCreateCaravan: false, Rot4.Invalid);
					AbilityUtility.DoClamor(item3.Position, Props.clamorRadius, parent.pawn, Props.clamorType);
				}
				return;
			}
		}
		if (caravan != null)
		{
			caravan.Tile = target.Tile;
			caravan.pather.StopDead();
			return;
		}
		CaravanMaker.MakeCaravan(list, parent.pawn.Faction, target.Tile, addToWorldPawnsIfNotAlready: false);
		foreach (Pawn item4 in list)
		{
			item4.ExitMap(allowedToJoinOrCreateCaravan: false, Rot4.Invalid);
		}
	}

	[IteratorStateMachine(typeof(_003CGetPreCastActions_003Ed__3))]
	public override IEnumerable<PreCastAction> GetPreCastActions()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetPreCastActions_003Ed__3(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CPawnsToSkip_003Ed__4))]
	private IEnumerable<Pawn> PawnsToSkip()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPawnsToSkip_003Ed__4(-2)
		{
			_003C_003E4__this = this
		};
	}

	private Pawn AlliedPawnOnMap(Map targetMap)
	{
		return targetMap.mapPawns.AllPawnsSpawned.FirstOrDefault((Pawn p) => !p.NonHumanlikeOrWildMan() && p.IsColonist && p.HomeFaction == Faction.OfPlayer && !PawnsToSkip().Contains(p));
	}

	private bool ShouldEnterMap(GlobalTargetInfo target)
	{
		if (target.WorldObject is Caravan caravan && caravan.Faction == parent.pawn.Faction)
		{
			return false;
		}
		if (target.WorldObject is MapParent { HasMap: not false } mapParent)
		{
			if (AlliedPawnOnMap(mapParent.Map) == null)
			{
				return mapParent.Map == parent.pawn.Map;
			}
			return true;
		}
		return false;
	}

	private bool ShouldJoinCaravan(GlobalTargetInfo target)
	{
		if (target.WorldObject is Caravan caravan)
		{
			return caravan.Faction == parent.pawn.Faction;
		}
		return false;
	}

	public override bool Valid(GlobalTargetInfo target, bool throwMessages = false)
	{
		Caravan caravan = parent.pawn.GetCaravan();
		if (caravan != null && caravan.ImmobilizedByMass)
		{
			return false;
		}
		Caravan caravan2 = target.WorldObject as Caravan;
		if (caravan != null && caravan == caravan2)
		{
			return false;
		}
		if (!ShouldEnterMap(target) && !ShouldJoinCaravan(target))
		{
			return false;
		}
		return base.Valid(target, throwMessages);
	}

	public override bool CanApplyOn(GlobalTargetInfo target)
	{
		if (target.WorldObject is MapParent { Map: not null } mapParent && AlliedPawnOnMap(mapParent.Map) == null)
		{
			return false;
		}
		return base.CanApplyOn(target);
	}

	public override Window ConfirmationDialog(GlobalTargetInfo target, Action confirmAction)
	{
		Pawn pawn = PawnsToSkip().FirstOrDefault((Pawn p) => p.IsQuestLodger());
		if (pawn != null)
		{
			return Dialog_MessageBox.CreateConfirmation("FarskipConfirmTeleportingLodger".Translate(pawn.Named("PAWN")), confirmAction);
		}
		return null;
	}

	public override string WorldMapExtraLabel(GlobalTargetInfo target)
	{
		Caravan caravan = parent.pawn.GetCaravan();
		if (caravan != null && caravan.ImmobilizedByMass)
		{
			return "CaravanImmobilizedByMass".Translate();
		}
		if (!Valid(target))
		{
			return "AbilityNeedAllyToSkip".Translate();
		}
		if (ShouldJoinCaravan(target))
		{
			return "AbilitySkipToJoinCaravan".Translate();
		}
		return "AbilitySkipToRandomAlly".Translate();
	}
}
