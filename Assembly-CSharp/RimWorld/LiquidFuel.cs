using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B3F RID: 11071
	public class LiquidFuel : Filth
	{
		// Token: 0x0600F78E RID: 63374 RVA: 0x0047F82F File Offset: 0x0047DA2F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.spawnTick, "spawnTick", 0, false);
		}

		// Token: 0x0600F78F RID: 63375 RVA: 0x0047F849 File Offset: 0x0047DA49
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.spawnTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600F790 RID: 63376 RVA: 0x0047F863 File Offset: 0x0047DA63
		public void Refill()
		{
			this.spawnTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600F791 RID: 63377 RVA: 0x0047F875 File Offset: 0x0047DA75
		protected override void TickInterval(int delta)
		{
			if (this.spawnTick + 1500 < Find.TickManager.TicksGame)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600F792 RID: 63378 RVA: 0x0047F896 File Offset: 0x0047DA96
		public override void ThickenFilth()
		{
			base.ThickenFilth();
			this.Refill();
		}

		// Token: 0x0400A2E5 RID: 41701
		private int spawnTick;

		// Token: 0x0400A2E6 RID: 41702
		private const int DryOutTime = 1500;
	}
}
