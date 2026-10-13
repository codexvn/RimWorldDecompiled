using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020010D0 RID: 4304
	public class Trigger_ChanceOnPlayerHarmNPCBuilding : Trigger
	{
		// Token: 0x06006920 RID: 26912 RVA: 0x0021263E File Offset: 0x0021083E
		public Trigger_ChanceOnPlayerHarmNPCBuilding(float chance)
		{
			this.chance = chance;
		}

		// Token: 0x06006921 RID: 26913 RVA: 0x00212658 File Offset: 0x00210858
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.BuildingDamaged && signal.dinfo.Def.ExternalViolenceFor(signal.thing) && signal.thing.def.category == ThingCategory.Building && signal.dinfo.Instigator != null && signal.dinfo.Instigator.Faction == Faction.OfPlayer && signal.thing.Faction != Faction.OfPlayer && Rand.Value < this.chance;
		}

		// Token: 0x04004A6E RID: 19054
		private float chance = 1f;
	}
}
