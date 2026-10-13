using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A91 RID: 10897
	public class CocoonSpawner : TunnelHiveSpawner
	{
		// Token: 0x0600F2A2 RID: 62114 RVA: 0x004674D0 File Offset: 0x004656D0
		protected override void Spawn(Map map, IntVec3 loc)
		{
			Thing thing = GenSpawn.Spawn(ThingMaker.MakeThing(this.cocoon, null), loc, map, WipeMode.Vanish);
			thing.SetFaction(Faction.OfInsects, null);
			thing.questTags = this.questTags;
			if (this.groupID >= 0)
			{
				CompWakeUpDormant compWakeUpDormant = thing.TryGetComp<CompWakeUpDormant>();
				if (compWakeUpDormant != null)
				{
					compWakeUpDormant.groupID = this.groupID;
				}
			}
		}

		// Token: 0x0600F2A3 RID: 62115 RVA: 0x00467529 File Offset: 0x00465729
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.cocoon, "cocoon");
			Scribe_Values.Look<int>(ref this.groupID, "groupID", -1, false);
		}

		// Token: 0x04009FD9 RID: 40921
		public ThingDef cocoon;

		// Token: 0x04009FDA RID: 40922
		public int groupID = -1;
	}
}
