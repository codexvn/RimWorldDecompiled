using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C56 RID: 15446
	public class MoveableGrowZone : MoveableArea
	{
		// Token: 0x06014FBF RID: 85951 RVA: 0x006481D4 File Offset: 0x006463D4
		public MoveableGrowZone(Gravship gravship, Zone_Growing growZone) : base(gravship, growZone.label, growZone.RenamableLabel, growZone.color, growZone.ID)
		{
			this.plantDef = growZone.PlantDefToGrow;
			this.allowSow = growZone.allowSow;
			this.allowCut = growZone.allowCut;
		}

		// Token: 0x06014FC0 RID: 85952 RVA: 0x00648224 File Offset: 0x00646424
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.plantDef, "plantDef");
			Scribe_Values.Look<bool>(ref this.allowSow, "allowSow", false, false);
			Scribe_Values.Look<bool>(ref this.allowCut, "allowCut", false, false);
		}

		// Token: 0x06014FC1 RID: 85953 RVA: 0x00648260 File Offset: 0x00646460
		public void TryCreateGrowZone(ZoneManager zoneManager, IntVec3 newOrigin)
		{
			Zone_Growing zone_Growing = new Zone_Growing(zoneManager)
			{
				label = this.label,
				color = this.color,
				ID = this.id
			};
			zone_Growing.SetPlantDefToGrow(this.plantDef);
			zone_Growing.allowSow = this.allowSow;
			zone_Growing.allowCut = this.allowCut;
			zoneManager.RegisterZone(zone_Growing);
			foreach (IntVec3 b in base.RelativeCells)
			{
				zone_Growing.AddCell(newOrigin + b);
			}
		}

		// Token: 0x0400E9E9 RID: 59881
		public ThingDef plantDef;

		// Token: 0x0400E9EA RID: 59882
		public bool allowSow;

		// Token: 0x0400E9EB RID: 59883
		public bool allowCut;
	}
}
