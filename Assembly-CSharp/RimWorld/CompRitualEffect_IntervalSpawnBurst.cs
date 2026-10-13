using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020027F3 RID: 10227
	public abstract class CompRitualEffect_IntervalSpawnBurst : CompRitualEffect_IntervalSpawn
	{
		// Token: 0x0600DFDE RID: 57310 RVA: 0x0041BD64 File Offset: 0x00419F64
		public override Mote SpawnMote(LordJob_Ritual ritual, Vector3? forcedPos = null)
		{
			for (int i = 0; i < base.Props.spawnCount; i++)
			{
				base.SpawnMote(ritual, null);
			}
			this.lastSpawnTick = GenTicks.TicksGame;
			this.burstsDone++;
			return null;
		}

		// Token: 0x0600DFDF RID: 57311 RVA: 0x0041BDB4 File Offset: 0x00419FB4
		public override void SpawnFleck(LordJob_Ritual ritual, Vector3? forcedPos = null, float? exactRotation = null)
		{
			for (int i = 0; i < base.Props.spawnCount; i++)
			{
				base.SpawnFleck(ritual, null, null);
			}
			this.lastSpawnTick = GenTicks.TicksGame;
			this.burstsDone++;
		}
	}
}
