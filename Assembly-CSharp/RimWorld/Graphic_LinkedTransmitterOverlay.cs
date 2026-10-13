using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002962 RID: 10594
	public class Graphic_LinkedTransmitterOverlay : Graphic_Linked
	{
		// Token: 0x0600E889 RID: 59529 RVA: 0x0043B418 File Offset: 0x00439618
		public Graphic_LinkedTransmitterOverlay()
		{
		}

		// Token: 0x0600E88A RID: 59530 RVA: 0x0012A6B4 File Offset: 0x001288B4
		public Graphic_LinkedTransmitterOverlay(Graphic subGraphic) : base(subGraphic)
		{
		}

		// Token: 0x0600E88B RID: 59531 RVA: 0x0043B420 File Offset: 0x00439620
		public override bool ShouldLinkWith(IntVec3 c, Thing parent)
		{
			return c.InBounds(parent.Map) && parent.Map.powerNetGrid.TransmittedPowerNetAt(c) != null;
		}

		// Token: 0x0600E88C RID: 59532 RVA: 0x0043B448 File Offset: 0x00439648
		public override void Print(SectionLayer layer, Thing parent, float extraRotation)
		{
			foreach (IntVec3 cell in parent.OccupiedRect())
			{
				Vector3 center = cell.ToVector3ShiftedWithAltitude(AltitudeLayer.MapDataOverlay);
				Printer_Plane.PrintPlane(layer, center, new Vector2(1f, 1f), this.LinkedDrawMatFrom(parent, cell), extraRotation, false, null, null, 0.01f, 0f);
			}
		}
	}
}
