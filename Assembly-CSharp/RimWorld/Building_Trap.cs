using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld;

public abstract class Building_Trap : Building, IPathFindCostProvider
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__31 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_Trap _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__31(int _003C_003E1__state)
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
				Building_Trap CS_0024_003C_003E8__locals5 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Building)CS_0024_003C_003E8__locals5).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0072:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (CS_0024_003C_003E8__locals5.CanSetAutoRearm)
					{
						_003C_003E2__current = new Command_Toggle
						{
							defaultLabel = "CommandAutoRearm".Translate(),
							defaultDesc = "CommandAutoRearmDesc".Translate(),
							hotKey = KeyBindingDefOf.Misc3,
							icon = TexCommand.RearmTrap,
							isActive = () => CS_0024_003C_003E8__locals5.autoRearm,
							toggleAction = delegate
							{
								CS_0024_003C_003E8__locals5.autoRearm = !CS_0024_003C_003E8__locals5.autoRearm;
							}
						};
						_003C_003E1__state = 2;
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__31 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__31(0)
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

	private bool autoRearm;

	private List<Pawn> touchingPawns = new List<Pawn>();

	private StunHandler stunner;

	private bool triedGettingStunner;

	private const float KnowerSpringChanceFactorSameFaction = 0.005f;

	private const float KnowerSpringChanceFactorWildAnimal = 0.2f;

	private const float KnowerSpringChanceFactorFactionlessHuman = 0.3f;

	private const float KnowerSpringChanceFactorOther = 0f;

	private const ushort KnowerPathFindCost = 800;

	private const ushort KnowerPathWalkCost = 40;

	private bool CanSetAutoRearm
	{
		get
		{
			if (base.Faction == Faction.OfPlayer && def.blueprintDef != null)
			{
				return def.IsResearchFinished;
			}
			return false;
		}
	}

	public virtual bool ShouldShowTrapDamageStat { get; } = true;

	protected bool IsStunned
	{
		get
		{
			if (!triedGettingStunner)
			{
				stunner = GetComp<CompStunnable>()?.StunHandler;
				triedGettingStunner = true;
			}
			return stunner?.Stunned ?? false;
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref autoRearm, "autoRearm", defaultValue: false);
		Scribe_Collections.Look(ref touchingPawns, "testees", LookMode.Reference);
		if (Scribe.mode == LoadSaveMode.PostLoadInit && touchingPawns.RemoveAll((Pawn x) => x == null) != 0)
		{
			Log.Error("Removed null pawns from touchingPawns.");
		}
	}

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		base.SpawnSetup(map, respawningAfterLoad);
		if (!respawningAfterLoad && !base.BeingTransportedOnGravship)
		{
			autoRearm = CanSetAutoRearm && map.areaManager.Home[base.Position];
		}
	}

	protected override void Tick()
	{
		if (base.Spawned && !IsStunned)
		{
			List<Thing> thingList = base.Position.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i] is Pawn { Flying: false } pawn && !touchingPawns.Contains(pawn))
				{
					touchingPawns.Add(pawn);
					CheckSpring(pawn);
				}
			}
			for (int num = touchingPawns.Count - 1; num >= 0; num--)
			{
				Pawn pawn2 = touchingPawns[num];
				if (pawn2 == null || !pawn2.Spawned || pawn2.Flying || pawn2.Position != base.Position)
				{
					touchingPawns.Remove(pawn2);
				}
			}
		}
		base.Tick();
	}

	private void CheckSpring(Pawn p)
	{
		if (Rand.Chance(SpringChance(p)))
		{
			Map map = base.Map;
			Spring(p);
			if (p.Faction == Faction.OfPlayer || p.HostFaction == Faction.OfPlayer)
			{
				Find.LetterStack.ReceiveLetter("LetterFriendlyTrapSprungLabel".Translate(p.LabelShort, p).CapitalizeFirst(), "LetterFriendlyTrapSprung".Translate(p.LabelShort, p).CapitalizeFirst(), LetterDefOf.NegativeEvent, new TargetInfo(base.Position, map));
			}
		}
	}

	protected virtual float SpringChance(Pawn p)
	{
		float num = 1f;
		if (p.kindDef.immuneToTraps)
		{
			return 0f;
		}
		if (KnowsOfTrap(p))
		{
			if (p.Faction != null)
			{
				num = ((p.Faction != base.Faction) ? 0f : 0.005f);
			}
			else if (p.IsAnimal)
			{
				num = 0.2f;
				num *= def.building.trapPeacefulWildAnimalsSpringChanceFactor;
			}
			else
			{
				num = 0.3f;
			}
		}
		num *= this.GetStatValue(StatDefOf.TrapSpringChance) * p.GetStatValue(StatDefOf.PawnTrapSpringChance);
		return Mathf.Clamp01(num);
	}

	public bool KnowsOfTrap(Pawn p)
	{
		if (p.Faction != null && !p.Faction.HostileTo(base.Faction))
		{
			return true;
		}
		if (p.Faction == null && p.IsAnimal && !p.InAggroMentalState)
		{
			return true;
		}
		if (p.guest != null && p.guest.Released)
		{
			return true;
		}
		if (!p.IsPrisoner && base.Faction != null && p.HostFaction == base.Faction)
		{
			return true;
		}
		if (p.RaceProps.Humanlike && p.IsFormingCaravan())
		{
			return true;
		}
		if (p.pather.cachedReturningToCell && base.Faction == p.HostFaction)
		{
			return true;
		}
		if (p.Faction == null && p.RaceProps.Humanlike)
		{
			return true;
		}
		if (p.lord?.LordJob is ILordAvoidTraps lordAvoidTraps && Rand.ValueAsync(HashCode.Combine(p.Faction?.loadID ?? p.lord.loadID, thingIDNumber)) < lordAvoidTraps.AvoidTrapRatio)
		{
			return true;
		}
		return false;
	}

	public CellRect GetOccupiedRect()
	{
		return this.OccupiedRect();
	}

	public ushort PathFindCostFor(Pawn p)
	{
		if (!KnowsOfTrap(p))
		{
			return 0;
		}
		return 800;
	}

	public override ushort PathWalkCostFor(Pawn p)
	{
		if (!KnowsOfTrap(p))
		{
			return 0;
		}
		return 40;
	}

	public override bool IsDangerousFor(Pawn p)
	{
		return KnowsOfTrap(p);
	}

	public void Spring(Pawn p)
	{
		bool spawned = base.Spawned;
		Map map = base.Map;
		SpringSub(p);
		if (def.building.trapDestroyOnSpring)
		{
			if (!base.Destroyed)
			{
				Destroy();
			}
			if (spawned)
			{
				CheckAutoRebuild(map);
			}
		}
	}

	public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
	{
		bool spawned = base.Spawned;
		Map map = base.Map;
		base.Kill(dinfo, exactCulprit);
		if (spawned)
		{
			CheckAutoRebuild(map);
		}
	}

	protected abstract void SpringSub(Pawn p);

	private void CheckAutoRebuild(Map map)
	{
		if (autoRearm && CanSetAutoRearm && map != null && GenConstruct.CanPlaceBlueprintAt(def, base.Position, base.Rotation, map, godMode: false, null, null, base.Stuff).Accepted)
		{
			GenConstruct.PlaceBlueprintForBuild(def, base.Position, map, base.Rotation, Faction.OfPlayer, base.Stuff);
		}
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__31))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__31(-2)
		{
			_003C_003E4__this = this
		};
	}
}
