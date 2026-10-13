using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002A92 RID: 10898
	public class DelayedEffecterSpawner : Thing
	{
		// Token: 0x0600F2A5 RID: 62117 RVA: 0x00467562 File Offset: 0x00465762
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.spawnTick = GenTicks.TicksGame + this.emergeDelayTicks;
				if (this.preEmergeEffecter != null)
				{
					this.spawnPreEffectTick = this.spawnTick - this.preEmergeEffecter.maintainTicks;
				}
			}
		}

		// Token: 0x0600F2A6 RID: 62118 RVA: 0x004675A4 File Offset: 0x004657A4
		protected override void Tick()
		{
			if (!base.Spawned)
			{
				return;
			}
			if (GenTicks.TicksGame >= this.spawnPreEffectTick && !this.spawnedMistEffect)
			{
				this.spawnedMistEffect = true;
				EffecterDef effecterDef = this.preEmergeEffecter;
				if (effecterDef != null)
				{
					effecterDef.Spawn(base.Position, base.Map, 1f).Cleanup();
				}
			}
			if (GenTicks.TicksGame >= this.spawnTick)
			{
				Map map = base.Map;
				IntVec3 position = base.Position;
				this.Destroy(DestroyMode.Vanish);
				GenSpawn.Spawn(this.pawnToSpawn, position, map, WipeMode.Vanish);
				EffecterDef effecterDef2 = this.emergeEffecter;
				if (effecterDef2 != null)
				{
					effecterDef2.Spawn(position, map, 1f).Cleanup();
				}
				SoundDef soundDef = this.emergeSound;
				if (soundDef == null)
				{
					return;
				}
				soundDef.PlayOneShot(this.pawnToSpawn);
			}
		}

		// Token: 0x0600F2A7 RID: 62119 RVA: 0x00467668 File Offset: 0x00465868
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.emergeDelayTicks, "emergeDelayTicks", 0, false);
			Scribe_Values.Look<int>(ref this.spawnPreEffectTick, "spawnPreEffectTick", 0, false);
			Scribe_Values.Look<bool>(ref this.spawnedMistEffect, "spawnedMistEffect", false, false);
			Scribe_Values.Look<int>(ref this.spawnTick, "spawnTick", 0, false);
			Scribe_Defs.Look<EffecterDef>(ref this.emergeEffecter, "emergeEffecter");
			Scribe_Defs.Look<SoundDef>(ref this.emergeSound, "emergeSound");
			Scribe_Defs.Look<EffecterDef>(ref this.preEmergeEffecter, "preEmergeEffecter");
			Scribe_Deep.Look<Pawn>(ref this.pawnToSpawn, "pawnToSpawn", Array.Empty<object>());
		}

		// Token: 0x0600F2A8 RID: 62120 RVA: 0x00467708 File Offset: 0x00465908
		public static DelayedEffecterSpawner Spawn(Pawn pawn, IntVec3 pos, Map map, int delayTicks, EffecterDef emergeEffect = null, EffecterDef preEmergeEffect = null, SoundDef emergeSound = null)
		{
			DelayedEffecterSpawner delayedEffecterSpawner = (DelayedEffecterSpawner)ThingMaker.MakeThing(ThingDefOf.DelayedEffecterSpawner, null);
			delayedEffecterSpawner.pawnToSpawn = pawn;
			delayedEffecterSpawner.emergeDelayTicks = delayTicks;
			delayedEffecterSpawner.preEmergeEffecter = preEmergeEffect;
			delayedEffecterSpawner.emergeEffecter = emergeEffect;
			delayedEffecterSpawner.emergeSound = emergeSound;
			GenSpawn.Spawn(delayedEffecterSpawner, pos, map, WipeMode.Vanish);
			return delayedEffecterSpawner;
		}

		// Token: 0x04009FDB RID: 40923
		public Pawn pawnToSpawn;

		// Token: 0x04009FDC RID: 40924
		public int emergeDelayTicks;

		// Token: 0x04009FDD RID: 40925
		public EffecterDef emergeEffecter;

		// Token: 0x04009FDE RID: 40926
		public SoundDef emergeSound;

		// Token: 0x04009FDF RID: 40927
		public EffecterDef preEmergeEffecter;

		// Token: 0x04009FE0 RID: 40928
		private int spawnTick;

		// Token: 0x04009FE1 RID: 40929
		private int spawnPreEffectTick;

		// Token: 0x04009FE2 RID: 40930
		private bool spawnedMistEffect;
	}
}
