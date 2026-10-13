using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003824 RID: 14372
	public class PlaceWorker_MechGestatorTop : PlaceWorker
	{
		// Token: 0x06013DF7 RID: 81399 RVA: 0x005E6404 File Offset: 0x005E4604
		public override void DrawGhost(ThingDef def, IntVec3 loc, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			GhostUtility.GhostGraphicFor(GraphicDatabase.Get<Graphic_Multi>(def.building.mechGestatorCylinderGraphic.texPath, ShaderDatabase.Cutout, def.building.mechGestatorCylinderGraphic.drawSize, Color.white), def, ghostCol, null).DrawFromDef(GenThing.TrueCenter(loc, rot, def.Size, AltitudeLayer.MetaOverlays.AltitudeFor()), rot, def, 0f);
			GhostUtility.GhostGraphicFor(GraphicDatabase.Get<Graphic_Multi>(def.building.mechGestatorTopGraphic.texPath, ShaderDatabase.Cutout, def.building.mechGestatorTopGraphic.drawSize, Color.white), def, ghostCol, null).DrawFromDef(GenThing.TrueCenter(loc, rot, def.Size, AltitudeLayer.MetaOverlays.AltitudeFor()), rot, def, 0f);
		}
	}
}
