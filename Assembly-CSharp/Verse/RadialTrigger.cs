using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200087C RID: 2172
	public class RadialTrigger : PawnTrigger
	{
		// Token: 0x06003715 RID: 14101 RVA: 0x001275DC File Offset: 0x001257DC
		protected override void Tick()
		{
			if (!this.IsHashIntervalTick(60))
			{
				return;
			}
			Map map = base.Map;
			int num = GenRadial.NumCellsInRadius((float)this.maxRadius);
			for (int i = 0; i < num; i++)
			{
				IntVec3 c = base.Position + GenRadial.RadialPattern[i];
				if (c.InBounds(map))
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int j = 0; j < thingList.Count; j++)
					{
						if (base.TriggeredBy(thingList[j]) && (!this.lineOfSight || GenSight.LineOfSightToThing(base.Position, thingList[j], map, false, null)))
						{
							base.ActivatedBy((Pawn)thingList[j]);
							return;
						}
					}
				}
			}
		}

		// Token: 0x06003716 RID: 14102 RVA: 0x001276A0 File Offset: 0x001258A0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.maxRadius, "maxRadius", 0, false);
			Scribe_Values.Look<bool>(ref this.lineOfSight, "lineOfSight", false, false);
		}

		// Token: 0x04002951 RID: 10577
		public int maxRadius;

		// Token: 0x04002952 RID: 10578
		public bool lineOfSight;
	}
}
