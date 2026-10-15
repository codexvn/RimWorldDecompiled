using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld;

public class Hive : Building, IAttackTarget, ILoadReferenceable
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__27 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Hive _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__27(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Hive hive = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Building)hive).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -4;
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
					_003C_003E7__wrap1 = QuestUtility.GetQuestRelatedGizmos(hive).GetEnumerator();
					_003C_003E1__state = -4;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					Gizmo current2 = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current2;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap1 = null;
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
			_003CGetGizmos_003Ed__27 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__27(0)
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

	public const int PawnSpawnRadius = 2;

	public const float MaxSpawnedPawnsPoints = 500f;

	public const float InitialPawnsPoints = 200f;

	public static List<PawnKindDef> spawnablePawnKinds = new List<PawnKindDef>();

	public static readonly string MemoAttackedByEnemy = "HiveAttacked";

	public static readonly string MemoDeSpawned = "HiveDeSpawned";

	public static readonly string MemoBurnedBadly = "HiveBurnedBadly";

	public static readonly string MemoDestroyedNonRoofCollapse = "HiveDestroyedNonRoofCollapse";

	public CompCanBeDormant CompDormant => GetComp<CompCanBeDormant>();

	Thing IAttackTarget.Thing => this;

	public float TargetPriorityFactor => 0.4f;

	public LocalTargetInfo TargetCurrentlyAimingAt => LocalTargetInfo.Invalid;

	public CompSpawnerPawn PawnSpawner => GetComp<CompSpawnerPawn>();

	public bool ThreatDisabled(IAttackTargetSearcher disabledFor)
	{
		if (!base.Spawned)
		{
			return true;
		}
		CompCanBeDormant comp = GetComp<CompCanBeDormant>();
		if (comp != null && !comp.Awake)
		{
			return true;
		}
		return false;
	}

	public static void ResetStaticData()
	{
		spawnablePawnKinds.Clear();
		spawnablePawnKinds.Add(PawnKindDefOf.Megascarab);
		spawnablePawnKinds.Add(PawnKindDefOf.Spelopede);
		spawnablePawnKinds.Add(PawnKindDefOf.Megaspider);
		if (ModsConfig.OdysseyActive)
		{
			spawnablePawnKinds.Add(PawnKindDefOf.Locust);
		}
	}

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		base.SpawnSetup(map, respawningAfterLoad);
		if (base.Faction == null)
		{
			SetFaction(Faction.OfInsects);
		}
	}

	protected override void TickInterval(int delta)
	{
		base.TickInterval(delta);
		if (base.Spawned && !CompDormant.Awake && !base.Position.Fogged(base.Map))
		{
			CompDormant.WakeUp();
		}
	}

	public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
	{
		Map map = base.Map;
		base.DeSpawn(mode);
		if (HiveUtility.TotalSpawnedHivesCount(map, filterFogged: true) == 0)
		{
			foreach (Pawn item in map.mapPawns.FreeColonistsSpawned)
			{
				item.needs?.mood?.thoughts?.memories?.TryGainMemory(ThoughtDefOf.DefeatedInsectHive);
			}
		}
		if (map.generatorDef != MapGeneratorDefOf.InsectLair)
		{
			List<Lord> lords = map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				lords[i].ReceiveMemo(MemoDeSpawned);
			}
		}
		HiveUtility.Notify_HiveDespawned(this, map);
	}

	public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
	{
		if (!questTags.NullOrEmpty())
		{
			bool flag = false;
			List<Thing> list = base.Map.listerThings.ThingsOfDef(def);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] is Hive hive && hive != this && hive.CompDormant.Awake && !hive.questTags.NullOrEmpty() && QuestUtility.AnyMatchingTags(hive.questTags, questTags))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				QuestUtility.SendQuestTargetSignals(questTags, "AllHivesDestroyed");
			}
		}
		base.Destroy(mode);
	}

	public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
	{
		if (dinfo.Def.ExternalViolenceFor(this) && dinfo.Instigator != null && dinfo.Instigator.Faction != null)
		{
			GetComp<CompSpawnerPawn>().Lord?.ReceiveMemo(MemoAttackedByEnemy);
		}
		if (dinfo.Def == DamageDefOf.Flame && (float)HitPoints < (float)base.MaxHitPoints * 0.3f)
		{
			GetComp<CompSpawnerPawn>().Lord?.ReceiveMemo(MemoBurnedBadly);
		}
		base.PostApplyDamage(dinfo, totalDamageDealt);
	}

	public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
	{
		if (base.Spawned && (!dinfo.HasValue || dinfo.Value.Category != DamageInfo.SourceCategory.Collapse))
		{
			List<Lord> lords = base.Map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				lords[i].ReceiveMemo(MemoDestroyedNonRoofCollapse);
			}
		}
		base.Kill(dinfo, exactCulprit);
	}

	public override bool PreventPlayerSellingThingsNearby(out string reason)
	{
		if (PawnSpawner.spawnedPawns.Count > 0 && PawnSpawner.spawnedPawns.Any((Pawn p) => !p.Downed))
		{
			reason = def.label;
			return true;
		}
		reason = null;
		return false;
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__27))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__27(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		if (Scribe.mode != LoadSaveMode.Saving)
		{
			bool value = false;
			Scribe_Values.Look(ref value, "active", defaultValue: false);
			if (value)
			{
				CompDormant.WakeUp();
			}
		}
	}
}
