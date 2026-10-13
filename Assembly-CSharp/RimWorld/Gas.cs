using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002AE0 RID: 10976
	public class Gas : Thing
	{
		// Token: 0x0600F4AA RID: 62634 RVA: 0x00471FCC File Offset: 0x004701CC
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			for (;;)
			{
				Thing gas = base.Position.GetGas(map);
				if (gas == null)
				{
					break;
				}
				gas.Destroy(DestroyMode.Vanish);
			}
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad && !base.BeingTransportedOnGravship)
			{
				this.destroyTick = Find.TickManager.TicksGame + this.def.gas.expireSeconds.RandomInRange.SecondsToTicks();
			}
			this.graphicRotationSpeed = Rand.Range(-this.def.gas.rotationSpeed, this.def.gas.rotationSpeed) / 60f;
		}

		// Token: 0x0600F4AB RID: 62635 RVA: 0x00472062 File Offset: 0x00470262
		protected override void TickInterval(int delta)
		{
			if (this.destroyTick <= Find.TickManager.TicksGame)
			{
				this.Destroy(DestroyMode.Vanish);
			}
			this.graphicRotation += this.graphicRotationSpeed * (float)delta;
		}

		// Token: 0x0600F4AC RID: 62636 RVA: 0x00472093 File Offset: 0x00470293
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.destroyTick, "destroyTick", 0, false);
		}

		// Token: 0x0400A140 RID: 41280
		public int destroyTick;

		// Token: 0x0400A141 RID: 41281
		public float graphicRotation;

		// Token: 0x0400A142 RID: 41282
		public float graphicRotationSpeed;
	}
}
