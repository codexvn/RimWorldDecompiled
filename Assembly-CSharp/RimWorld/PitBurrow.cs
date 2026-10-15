using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld;

public class PitBurrow : Crater
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__14 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PitBurrow _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__14(int _003C_003E1__state)
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
				PitBurrow CS_0024_003C_003E8__locals5 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Crater)CS_0024_003C_003E8__locals5).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_006f;
				case 1:
					_003C_003E1__state = -3;
					goto IL_006f;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_006f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (!DebugSettings.ShowDevGizmos)
					{
						break;
					}
					_003C_003E2__current = new Command_Action
					{
						action = delegate
						{
							if (CS_0024_003C_003E8__locals5.emergingFleshbeasts == null)
							{
								CS_0024_003C_003E8__locals5.emergingFleshbeasts = new List<Pawn>();
							}
							PawnKindDef fingerspike = PawnKindDefOf.Fingerspike;
							Pawn item = PawnGenerator.GeneratePawn(fingerspike, FactionUtility.DefaultFactionFrom(fingerspike.defaultFactionDef), null);
							CS_0024_003C_003E8__locals5.emergingFleshbeasts.Add(item);
							CS_0024_003C_003E8__locals5.FlushSpawnQueue();
						},
						defaultLabel = "DEV: Spawn Fleshbeast"
					};
					_003C_003E1__state = 2;
					return true;
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
			_003CGetGizmos_003Ed__14 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__14(0)
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

	public List<Pawn> emergingFleshbeasts;

	public int emergeDelay;

	public bool assaultColony = true;

	private int collapseTick;

	private const int SpawnIntervalSeconds = 1;

	private static readonly IntRange CollapseTicksRange = new IntRange(720000, 1200000);

	protected override EffecterDef FilledInEffecter => EffecterDefOf.ImpactSmallDustCloud;

	protected override SoundDef FilledInSound => SoundDefOf.PitBurrow_Collapse;

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		if (!ModLister.CheckAnomaly("Pit burrow"))
		{
			Destroy();
			return;
		}
		base.SpawnSetup(map, respawningAfterLoad);
		if (!respawningAfterLoad)
		{
			collapseTick = Find.TickManager.TicksGame + CollapseTicksRange.RandomInRange;
			if (!emergingFleshbeasts.NullOrEmpty())
			{
				FlushSpawnQueue();
			}
		}
	}

	private void FlushSpawnQueue()
	{
		CellRect cellRect = GenAdj.OccupiedRect(base.Position, Rot4.North, ThingDefOf.PitBurrow.Size).ContractedBy(2);
		List<PawnFlyer> list = new List<PawnFlyer>();
		List<IntVec3> list2 = new List<IntVec3>();
		foreach (Pawn emergingFleshbeast in emergingFleshbeasts)
		{
			IntVec3 randomCell = cellRect.RandomCell;
			GenSpawn.Spawn(emergingFleshbeast, randomCell, base.Map);
			if (CellFinder.TryFindRandomCellNear(base.Position, base.Map, def.size.x / 2 + 1, (IntVec3 c) => !c.Fogged(base.Map) && c.Walkable(base.Map) && !c.Impassable(base.Map), out var result))
			{
				emergingFleshbeast.rotationTracker.FaceCell(result);
				list.Add(PawnFlyer.MakeFlyer(ThingDefOf.PawnFlyer_Stun, emergingFleshbeast, result, null, null, flyWithCarriedThing: false, randomCell.ToVector3() + new Vector3(0f, 0f, -1f)));
				list2.Add(randomCell);
			}
		}
		if (list2.Count != 0)
		{
			SpawnRequest spawnRequest = new SpawnRequest(list.Cast<Thing>().ToList(), list2, 1, 1f);
			spawnRequest.initialDelay = emergeDelay;
			if (assaultColony)
			{
				spawnRequest.lord = LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_FleshbeastAssault(), base.Map);
			}
			base.Map.deferredSpawner.AddRequest(spawnRequest);
			SoundDefOf.Pawn_Fleshbeast_EmergeFromPitGate.PlayOneShot(this);
			emergingFleshbeasts.Clear();
		}
	}

	protected override void Tick()
	{
		base.Tick();
		if (Find.TickManager.TicksGame > collapseTick)
		{
			Messages.Message("MessagePitBurrowCollapsed".Translate(), new TargetInfo(base.Position, base.Map), MessageTypeDefOf.NeutralEvent, historical: false);
			Collapse();
		}
	}

	private void Collapse()
	{
		EffecterDefOf.ImpactSmallDustCloud.Spawn(base.Position, base.Map).Cleanup();
		FillIn();
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__14))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__14(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref emergeDelay, "emergeDelay", 0);
		Scribe_Collections.Look(ref emergingFleshbeasts, "emergingFleshbeasts", LookMode.Deep);
		Scribe_Values.Look(ref assaultColony, "assaultColony", defaultValue: true);
		Scribe_Values.Look(ref collapseTick, "collapseTick", 0);
	}
}
