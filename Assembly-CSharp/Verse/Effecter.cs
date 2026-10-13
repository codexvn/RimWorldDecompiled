using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CB4 RID: 3252
	public class Effecter
	{
		// Token: 0x0600534F RID: 21327 RVA: 0x001AD698 File Offset: 0x001AB898
		public Effecter(EffecterDef def)
		{
			this.def = def;
			this.spawnTick = Find.TickManager.TicksGame;
			for (int i = 0; i < def.children.Count; i++)
			{
				SubEffecterDef subEffecterDef = def.children[i];
				if (DebugSettings.anomalyDarkeningFX)
				{
					this.children.Add(subEffecterDef.Spawn(this));
				}
			}
		}

		// Token: 0x06005350 RID: 21328 RVA: 0x001AD71C File Offset: 0x001AB91C
		public void EffectTick(TargetInfo A, TargetInfo B)
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubEffectTick(A, B);
			}
		}

		// Token: 0x06005351 RID: 21329 RVA: 0x001AD754 File Offset: 0x001AB954
		public Effecter Trigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1)
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubTrigger(A, B, overrideSpawnTick, false);
			}
			return this;
		}

		// Token: 0x06005352 RID: 21330 RVA: 0x001AD78D File Offset: 0x001AB98D
		public void ForceEnd()
		{
			this.ticksLeft = 0;
			this.Cleanup();
		}

		// Token: 0x06005353 RID: 21331 RVA: 0x001AD79C File Offset: 0x001AB99C
		public void Cleanup()
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubCleanup();
			}
		}

		// Token: 0x04003C24 RID: 15396
		public EffecterDef def;

		// Token: 0x04003C25 RID: 15397
		public List<SubEffecter> children = new List<SubEffecter>();

		// Token: 0x04003C26 RID: 15398
		public int ticksLeft = -1;

		// Token: 0x04003C27 RID: 15399
		public Vector3 offset;

		// Token: 0x04003C28 RID: 15400
		public float scale = 1f;

		// Token: 0x04003C29 RID: 15401
		public int spawnTick;
	}
}
