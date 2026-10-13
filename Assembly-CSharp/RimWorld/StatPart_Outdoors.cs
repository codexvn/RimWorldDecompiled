using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003713 RID: 14099
	public class StatPart_Outdoors : StatPart
	{
		// Token: 0x060138C6 RID: 80070 RVA: 0x005CC242 File Offset: 0x005CA442
		public override void TransformValue(StatRequest req, ref float val)
		{
			val *= this.OutdoorsFactor(req);
		}

		// Token: 0x060138C7 RID: 80071 RVA: 0x005CC250 File Offset: 0x005CA450
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && req.Thing.GetRoom(RegionType.Set_All) != null)
			{
				string str;
				if (this.ConsideredOutdoors(req))
				{
					str = "Outdoors".Translate().CapitalizeFirst();
				}
				else
				{
					str = "Indoors".Translate();
				}
				return str + ": x" + this.OutdoorsFactor(req).ToStringPercent();
			}
			return null;
		}

		// Token: 0x060138C8 RID: 80072 RVA: 0x005CC2C2 File Offset: 0x005CA4C2
		private float OutdoorsFactor(StatRequest req)
		{
			if (this.ConsideredOutdoors(req))
			{
				return this.factorOutdoors;
			}
			return this.factorIndoors;
		}

		// Token: 0x060138C9 RID: 80073 RVA: 0x005CC2DC File Offset: 0x005CA4DC
		private bool ConsideredOutdoors(StatRequest req)
		{
			if (req.HasThing)
			{
				Room room = req.Thing.GetRoom(RegionType.Set_All);
				if (room != null)
				{
					return room.OutdoorsForWork || (req.HasThing && req.Thing.Spawned && !req.Thing.Map.roofGrid.Roofed(req.Thing.Position));
				}
			}
			return false;
		}

		// Token: 0x0400D8DA RID: 55514
		private float factorIndoors = 1f;

		// Token: 0x0400D8DB RID: 55515
		private float factorOutdoors = 1f;
	}
}
