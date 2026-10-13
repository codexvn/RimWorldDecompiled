using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E23 RID: 11811
	public class CompSpawnEffecterOnDestroy : ThingComp
	{
		// Token: 0x17002A9C RID: 10908
		// (get) Token: 0x06010A1C RID: 68124 RVA: 0x004CC048 File Offset: 0x004CA248
		private CompProperties_SpawnEffecterOnDestroy Props
		{
			get
			{
				return (CompProperties_SpawnEffecterOnDestroy)this.props;
			}
		}

		// Token: 0x06010A1D RID: 68125 RVA: 0x004CC055 File Offset: 0x004CA255
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			this.Props.effect.Spawn(this.parent.Position, previousMap, 1f);
		}
	}
}
