using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002B00 RID: 11008
	public class Hive : Building, IAttackTarget, ILoadReferenceable
	{
		// Token: 0x17002671 RID: 9841
		// (get) Token: 0x0600F5C6 RID: 62918 RVA: 0x00476047 File Offset: 0x00474247
		public CompCanBeDormant CompDormant
		{
			get
			{
				return base.GetComp<CompCanBeDormant>();
			}
		}

		// Token: 0x17002672 RID: 9842
		// (get) Token: 0x0600F5C7 RID: 62919 RVA: 0x000FD030 File Offset: 0x000FB230
		Thing IAttackTarget.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17002673 RID: 9843
		// (get) Token: 0x0600F5C8 RID: 62920 RVA: 0x0034FBF4 File Offset: 0x0034DDF4
		public float TargetPriorityFactor
		{
			get
			{
				return 0.4f;
			}
		}

		// Token: 0x17002674 RID: 9844
		// (get) Token: 0x0600F5C9 RID: 62921 RVA: 0x0047604F File Offset: 0x0047424F
		public LocalTargetInfo TargetCurrentlyAimingAt
		{
			get
			{
				return LocalTargetInfo.Invalid;
			}
		}

		// Token: 0x17002675 RID: 9845
		// (get) Token: 0x0600F5CA RID: 62922 RVA: 0x00476056 File Offset: 0x00474256
		public CompSpawnerPawn PawnSpawner
		{
			get
			{
				return base.GetComp<CompSpawnerPawn>();
			}
		}

		// Token: 0x0600F5CB RID: 62923 RVA: 0x00476060 File Offset: 0x00474260
		public bool ThreatDisabled(IAttackTargetSearcher disabledFor)
		{
			if (!base.Spawned)
			{
				return true;
			}
			CompCanBeDormant comp = base.GetComp<CompCanBeDormant>();
			return comp != null && !comp.Awake;
		}

		// Token: 0x0600F5CC RID: 62924 RVA: 0x0047608C File Offset: 0x0047428C
		public static void ResetStaticData()
		{
			Hive.spawnablePawnKinds.Clear();
			Hive.spawnablePawnKinds.Add(PawnKindDefOf.Megascarab);
			Hive.spawnablePawnKinds.Add(PawnKindDefOf.Spelopede);
			Hive.spawnablePawnKinds.Add(PawnKindDefOf.Megaspider);
			if (ModsConfig.OdysseyActive)
			{
				Hive.spawnablePawnKinds.Add(PawnKindDefOf.Locust);
			}
		}

		// Token: 0x0600F5CD RID: 62925 RVA: 0x004760E6 File Offset: 0x004742E6
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (base.Faction == null)
			{
				this.SetFaction(Faction.OfInsects, null);
			}
		}

		// Token: 0x0600F5CE RID: 62926 RVA: 0x00476104 File Offset: 0x00474304
		protected override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (base.Spawned && !this.CompDormant.Awake && !base.Position.Fogged(base.Map))
			{
				this.CompDormant.WakeUp();
			}
		}

		// Token: 0x0600F5CF RID: 62927 RVA: 0x00476140 File Offset: 0x00474340
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			if (HiveUtility.TotalSpawnedHivesCount(map, true) == 0)
			{
				foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
				{
					Pawn_NeedsTracker needs = pawn.needs;
					if (needs != null)
					{
						Need_Mood mood = needs.mood;
						if (mood != null)
						{
							ThoughtHandler thoughts = mood.thoughts;
							if (thoughts != null)
							{
								MemoryThoughtHandler memories = thoughts.memories;
								if (memories != null)
								{
									memories.TryGainMemory(ThoughtDefOf.DefeatedInsectHive, null, null);
								}
							}
						}
					}
				}
			}
			if (map.generatorDef != MapGeneratorDefOf.InsectLair)
			{
				List<Lord> lords = map.lordManager.lords;
				for (int i = 0; i < lords.Count; i++)
				{
					lords[i].ReceiveMemo(Hive.MemoDeSpawned);
				}
			}
			HiveUtility.Notify_HiveDespawned(this, map);
		}

		// Token: 0x0600F5D0 RID: 62928 RVA: 0x00476224 File Offset: 0x00474424
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (!this.questTags.NullOrEmpty<string>())
			{
				bool flag = false;
				List<Thing> list = base.Map.listerThings.ThingsOfDef(this.def);
				for (int i = 0; i < list.Count; i++)
				{
					Hive hive = list[i] as Hive;
					if (hive != null && hive != this && hive.CompDormant.Awake && !hive.questTags.NullOrEmpty<string>() && QuestUtility.AnyMatchingTags(hive.questTags, this.questTags))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					QuestUtility.SendQuestTargetSignals(this.questTags, "AllHivesDestroyed");
				}
			}
			base.Destroy(mode);
		}

		// Token: 0x0600F5D1 RID: 62929 RVA: 0x004762CC File Offset: 0x004744CC
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (dinfo.Def.ExternalViolenceFor(this) && dinfo.Instigator != null && dinfo.Instigator.Faction != null)
			{
				Lord lord = base.GetComp<CompSpawnerPawn>().Lord;
				if (lord != null)
				{
					lord.ReceiveMemo(Hive.MemoAttackedByEnemy);
				}
			}
			if (dinfo.Def == DamageDefOf.Flame && (float)this.HitPoints < (float)base.MaxHitPoints * 0.3f)
			{
				Lord lord2 = base.GetComp<CompSpawnerPawn>().Lord;
				if (lord2 != null)
				{
					lord2.ReceiveMemo(Hive.MemoBurnedBadly);
				}
			}
			base.PostApplyDamage(dinfo, totalDamageDealt);
		}

		// Token: 0x0600F5D2 RID: 62930 RVA: 0x00476360 File Offset: 0x00474560
		public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
		{
			if (base.Spawned && (dinfo == null || dinfo.Value.Category != DamageInfo.SourceCategory.Collapse))
			{
				List<Lord> lords = base.Map.lordManager.lords;
				for (int i = 0; i < lords.Count; i++)
				{
					lords[i].ReceiveMemo(Hive.MemoDestroyedNonRoofCollapse);
				}
			}
			base.Kill(dinfo, exactCulprit);
		}

		// Token: 0x0600F5D3 RID: 62931 RVA: 0x004763D0 File Offset: 0x004745D0
		public override bool PreventPlayerSellingThingsNearby(out string reason)
		{
			if (this.PawnSpawner.spawnedPawns.Count > 0)
			{
				if (this.PawnSpawner.spawnedPawns.Any((Pawn p) => !p.Downed))
				{
					reason = this.def.label;
					return true;
				}
			}
			reason = null;
			return false;
		}

		// Token: 0x0600F5D4 RID: 62932 RVA: 0x00476434 File Offset: 0x00474634
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			foreach (Gizmo gizmo2 in QuestUtility.GetQuestRelatedGizmos(this))
			{
				yield return gizmo2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600F5D5 RID: 62933 RVA: 0x00476444 File Offset: 0x00474644
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				bool flag = false;
				Scribe_Values.Look<bool>(ref flag, "active", false, false);
				if (flag)
				{
					this.CompDormant.WakeUp();
				}
			}
		}

		// Token: 0x0400A1C9 RID: 41417
		public const int PawnSpawnRadius = 2;

		// Token: 0x0400A1CA RID: 41418
		public const float MaxSpawnedPawnsPoints = 500f;

		// Token: 0x0400A1CB RID: 41419
		public const float InitialPawnsPoints = 200f;

		// Token: 0x0400A1CC RID: 41420
		public static List<PawnKindDef> spawnablePawnKinds = new List<PawnKindDef>();

		// Token: 0x0400A1CD RID: 41421
		public static readonly string MemoAttackedByEnemy = "HiveAttacked";

		// Token: 0x0400A1CE RID: 41422
		public static readonly string MemoDeSpawned = "HiveDeSpawned";

		// Token: 0x0400A1CF RID: 41423
		public static readonly string MemoBurnedBadly = "HiveBurnedBadly";

		// Token: 0x0400A1D0 RID: 41424
		public static readonly string MemoDestroyedNonRoofCollapse = "HiveDestroyedNonRoofCollapse";
	}
}
