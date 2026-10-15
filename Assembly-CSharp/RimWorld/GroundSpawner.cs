using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

[StaticConstructorOnStartup]
public class GroundSpawner : ThingWithComps
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass23_0
	{
		public int ticks;

		public GroundSpawner _003C_003E4__this;

		internal void _003CGetGizmos_003Eb__1()
		{
			_003C_003E4__this.secondarySpawnTick = Find.TickManager.TicksGame + ticks;
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__23 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public GroundSpawner _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__23(int _003C_003E1__state)
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
				GroundSpawner CS_0024_003C_003E8__locals2 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingWithComps)CS_0024_003C_003E8__locals2).GetGizmos().GetEnumerator();
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
						defaultLabel = "DEV: Set spawn delay",
						action = delegate
						{
							List<FloatMenuOption> list = new List<FloatMenuOption>();
							for (int i = 0; i < 6; i++)
							{
								_003C_003Ec__DisplayClass23_0 CS_0024_003C_003E8__locals5 = new _003C_003Ec__DisplayClass23_0
								{
									_003C_003E4__this = CS_0024_003C_003E8__locals2,
									ticks = i * 60
								};
								list.Add(new FloatMenuOption(i + "s", delegate
								{
									CS_0024_003C_003E8__locals5._003C_003E4__this.secondarySpawnTick = Find.TickManager.TicksGame + CS_0024_003C_003E8__locals5.ticks;
								}));
							}
							Find.WindowStack.Add(new FloatMenu(list));
						}
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
			_003CGetGizmos_003Ed__23 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__23(0)
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

	protected int secondarySpawnTick;

	protected float dustMoteSpawnMTB = 0.2f;

	protected float filthSpawnMTB = 0.3f;

	protected float filthSpawnRadius = 3f;

	private Sustainer sustainer;

	private Effecter sustainedFx;

	private static readonly IntRange DefaultSpawnDelay = new IntRange(1560, 1800);

	private static List<ThingDef> filthTypes = new List<ThingDef>();

	protected virtual IntRange ResultSpawnDelay => DefaultSpawnDelay;

	protected virtual SoundDef SustainerSound => SoundDefOf.Tunnel;

	public int TicksUntilSpawn => secondarySpawnTick - Find.TickManager.TicksGame;

	protected virtual bool SpawnRubble => true;

	public static void ResetStaticData()
	{
		filthTypes.Clear();
		filthTypes.Add(ThingDefOf.Filth_Dirt);
		filthTypes.Add(ThingDefOf.Filth_Dirt);
		filthTypes.Add(ThingDefOf.Filth_Dirt);
		filthTypes.Add(ThingDefOf.Filth_RubbleRock);
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref secondarySpawnTick, "secondarySpawnTick", 0);
		Scribe_Values.Look(ref dustMoteSpawnMTB, "dustMoteSpawnMTB", 0.2f);
		Scribe_Values.Look(ref filthSpawnMTB, "filthSpawnMTB", 0.3f);
		Scribe_Values.Look(ref filthSpawnRadius, "filthSpawnRadius", 3f);
	}

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		base.SpawnSetup(map, respawningAfterLoad);
		if (!respawningAfterLoad)
		{
			secondarySpawnTick = Find.TickManager.TicksGame + ResultSpawnDelay.RandomInRange;
		}
		CreateFX();
	}

	protected override void Tick()
	{
		base.Tick();
		if (!base.Spawned)
		{
			return;
		}
		Sustainer sustainer = this.sustainer;
		if (sustainer != null && !sustainer.Ended)
		{
			this.sustainer.Maintain();
		}
		sustainedFx?.EffectTick(this, this);
		if (SpawnRubble && Rand.MTBEventOccurs(filthSpawnMTB, 60f, 1f))
		{
			if (CellFinder.TryFindRandomReachableNearbyCell(this.OccupiedRect().RandomCell, base.Map, filthSpawnRadius, TraverseParms.For(TraverseMode.NoPassClosedDoors), null, null, out var result))
			{
				FilthMaker.TryMakeFilth(result, base.Map, filthTypes.RandomElement());
			}
			this.OccupiedRect().RandomCell.GetPlant(base.Map)?.Destroy();
		}
		if (secondarySpawnTick <= Find.TickManager.TicksGame)
		{
			this.sustainer?.End();
			Map map = base.Map;
			IntVec3 position = base.Position;
			def.building?.groundSpawnerCompleteEffecter?.SpawnMaintained(position, map);
			sustainedFx?.Cleanup();
			Thing.allowDestroyNonDestroyable = true;
			Destroy();
			Thing.allowDestroyNonDestroyable = false;
			Spawn(map, position);
		}
	}

	protected virtual void Spawn(Map map, IntVec3 loc)
	{
	}

	protected override void DrawAt(Vector3 drawLoc, bool flip = false)
	{
	}

	private void CreateFX()
	{
		LongEventHandler.ExecuteWhenFinished(delegate
		{
			sustainer = SustainerSound?.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
			sustainedFx = def.building?.groundSpawnerSustainedEffecter?.Spawn(this, base.Map);
		});
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__23))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__23(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string GetInspectString()
	{
		StringBuilder stringBuilder = new StringBuilder(base.GetInspectString());
		stringBuilder.AppendLineIfNotEmpty();
		stringBuilder.Append("Emergence".Translate() + ": " + TicksUntilSpawn.ToStringTicksToPeriod());
		return stringBuilder.ToString();
	}
}
