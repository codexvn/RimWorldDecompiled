using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002961 RID: 10593
	public class Graphic_LinkedTransmitter : Graphic_Linked
	{
		// Token: 0x0600E886 RID: 59526 RVA: 0x0012A6B4 File Offset: 0x001288B4
		public Graphic_LinkedTransmitter(Graphic subGraphic) : base(subGraphic)
		{
		}

		// Token: 0x0600E887 RID: 59527 RVA: 0x0043B33F File Offset: 0x0043953F
		public override bool ShouldLinkWith(IntVec3 c, Thing parent)
		{
			return c.InBounds(parent.Map) && (base.ShouldLinkWith(c, parent) || parent.Map.powerNetGrid.TransmittedPowerNetAt(c) != null);
		}

		// Token: 0x0600E888 RID: 59528 RVA: 0x0043B374 File Offset: 0x00439574
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			base.Print(layer, thing, extraRotation);
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec = thing.Position + GenAdj.CardinalDirections[i];
				if (intVec.InBounds(thing.Map))
				{
					Building transmitter = intVec.GetTransmitter(thing.Map);
					if (transmitter != null && !transmitter.def.graphicData.Linked)
					{
						Material mat = this.LinkedDrawMatFrom(thing, intVec);
						Printer_Plane.PrintPlane(layer, intVec.ToVector3ShiftedWithAltitude(thing.def.Altitude), Vector2.one, mat, extraRotation, false, null, null, 0.01f, 0f);
					}
				}
			}
		}
	}
}
