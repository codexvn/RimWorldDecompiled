using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002A9E RID: 10910
	[StaticConstructorOnStartup]
	public class GroundSpawner : ThingWithComps
	{
		// Token: 0x170025FC RID: 9724
		// (get) Token: 0x0600F302 RID: 62210 RVA: 0x0046903F File Offset: 0x0046723F
		protected virtual IntRange ResultSpawnDelay
		{
			get
			{
				return GroundSpawner.DefaultSpawnDelay;
			}
		}

		// Token: 0x170025FD RID: 9725
		// (get) Token: 0x0600F303 RID: 62211 RVA: 0x00469046 File Offset: 0x00467246
		protected virtual SoundDef SustainerSound
		{
			get
			{
				return SoundDefOf.Tunnel;
			}
		}

		// Token: 0x170025FE RID: 9726
		// (get) Token: 0x0600F304 RID: 62212 RVA: 0x0046904D File Offset: 0x0046724D
		public int TicksUntilSpawn
		{
			get
			{
				return this.secondarySpawnTick - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x170025FF RID: 9727
		// (get) Token: 0x0600F305 RID: 62213 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool SpawnRubble
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600F306 RID: 62214 RVA: 0x00469060 File Offset: 0x00467260
		public static void ResetStaticData()
		{
			GroundSpawner.filthTypes.Clear();
			GroundSpawner.filthTypes.Add(ThingDefOf.Filth_Dirt);
			GroundSpawner.filthTypes.Add(ThingDefOf.Filth_Dirt);
			GroundSpawner.filthTypes.Add(ThingDefOf.Filth_Dirt);
			GroundSpawner.filthTypes.Add(ThingDefOf.Filth_RubbleRock);
		}

		// Token: 0x0600F307 RID: 62215 RVA: 0x004690B4 File Offset: 0x004672B4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.secondarySpawnTick, "secondarySpawnTick", 0, false);
			Scribe_Values.Look<float>(ref this.dustMoteSpawnMTB, "dustMoteSpawnMTB", 0.2f, false);
			Scribe_Values.Look<float>(ref this.filthSpawnMTB, "filthSpawnMTB", 0.3f, false);
			Scribe_Values.Look<float>(ref this.filthSpawnRadius, "filthSpawnRadius", 3f, false);
		}

		// Token: 0x0600F308 RID: 62216 RVA: 0x0046911C File Offset: 0x0046731C
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.secondarySpawnTick = Find.TickManager.TicksGame + this.ResultSpawnDelay.RandomInRange;
			}
			this.CreateFX();
		}

		// Token: 0x0600F309 RID: 62217 RVA: 0x0046915C File Offset: 0x0046735C
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
			Effecter effecter = this.sustainedFx;
			if (effecter != null)
			{
				effecter.EffectTick(this, this);
			}
			if (this.SpawnRubble && Rand.MTBEventOccurs(this.filthSpawnMTB, 60f, 1f))
			{
				IntVec3 c;
				if (CellFinder.TryFindRandomReachableNearbyCell(this.OccupiedRect().RandomCell, base.Map, this.filthSpawnRadius, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false), null, null, out c, 999999))
				{
					FilthMaker.TryMakeFilth(c, base.Map, GroundSpawner.filthTypes.RandomElement<ThingDef>(), 1, FilthSourceFlags.None, true);
				}
				Plant plant = this.OccupiedRect().RandomCell.GetPlant(base.Map);
				if (plant != null)
				{
					plant.Destroy(DestroyMode.Vanish);
				}
			}
			if (this.secondarySpawnTick <= Find.TickManager.TicksGame)
			{
				Sustainer sustainer2 = this.sustainer;
				if (sustainer2 != null)
				{
					sustainer2.End();
				}
				Map map = base.Map;
				IntVec3 position = base.Position;
				BuildingProperties building = this.def.building;
				if (building != null)
				{
					EffecterDef groundSpawnerCompleteEffecter = building.groundSpawnerCompleteEffecter;
					if (groundSpawnerCompleteEffecter != null)
					{
						groundSpawnerCompleteEffecter.SpawnMaintained(position, map, 1f);
					}
				}
				Effecter effecter2 = this.sustainedFx;
				if (effecter2 != null)
				{
					effecter2.Cleanup();
				}
				Thing.allowDestroyNonDestroyable = true;
				this.Destroy(DestroyMode.Vanish);
				Thing.allowDestroyNonDestroyable = false;
				this.Spawn(map, position);
			}
		}

		// Token: 0x0600F30A RID: 62218 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void Spawn(Map map, IntVec3 loc)
		{
		}

		// Token: 0x0600F30B RID: 62219 RVA: 0x000026EA File Offset: 0x000008EA
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
		}

		// Token: 0x0600F30C RID: 62220 RVA: 0x004692D2 File Offset: 0x004674D2
		private void CreateFX()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				SoundDef sustainerSound = this.SustainerSound;
				this.sustainer = ((sustainerSound != null) ? sustainerSound.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick)) : null);
				BuildingProperties building = this.def.building;
				Effecter effecter;
				if (building == null)
				{
					effecter = null;
				}
				else
				{
					EffecterDef groundSpawnerSustainedEffecter = building.groundSpawnerSustainedEffecter;
					effecter = ((groundSpawnerSustainedEffecter != null) ? groundSpawnerSustainedEffecter.Spawn(this, base.Map, 1f) : null);
				}
				this.sustainedFx = effecter;
			});
		}

		// Token: 0x0600F30D RID: 62221 RVA: 0x004692E5 File Offset: 0x004674E5
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Set spawn delay",
					action = delegate()
					{
						List<FloatMenuOption> list = new List<FloatMenuOption>();
						for (int i = 0; i < 6; i++)
						{
							int ticks = i * 60;
							list.Add(new FloatMenuOption(i.ToString() + "s", delegate()
							{
								this.secondarySpawnTick = Find.TickManager.TicksGame + ticks;
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
						Find.WindowStack.Add(new FloatMenu(list));
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600F30E RID: 62222 RVA: 0x004692F8 File Offset: 0x004674F8
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder(base.GetInspectString());
			stringBuilder.AppendLineIfNotEmpty();
			stringBuilder.Append("Emergence".Translate() + ": " + this.TicksUntilSpawn.ToStringTicksToPeriod(true, false, true, true, false));
			return stringBuilder.ToString();
		}

		// Token: 0x0400A011 RID: 40977
		protected int secondarySpawnTick;

		// Token: 0x0400A012 RID: 40978
		protected float dustMoteSpawnMTB = 0.2f;

		// Token: 0x0400A013 RID: 40979
		protected float filthSpawnMTB = 0.3f;

		// Token: 0x0400A014 RID: 40980
		protected float filthSpawnRadius = 3f;

		// Token: 0x0400A015 RID: 40981
		private Sustainer sustainer;

		// Token: 0x0400A016 RID: 40982
		private Effecter sustainedFx;

		// Token: 0x0400A017 RID: 40983
		private static readonly IntRange DefaultSpawnDelay = new IntRange(1560, 1800);

		// Token: 0x0400A018 RID: 40984
		private static List<ThingDef> filthTypes = new List<ThingDef>();
	}
}
