using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003809 RID: 14345
	public class PlaceWorker_TurretTop : PlaceWorker
	{
		// Token: 0x06013DAB RID: 81323 RVA: 0x005E4E08 File Offset: 0x005E3008
		public override void DrawGhost(ThingDef def, IntVec3 loc, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			GhostUtility.GhostGraphicFor(GraphicDatabase.Get<Graphic_Single>(def.building.turretGunDef.graphicData.texPath, ShaderDatabase.Cutout, new Vector2(def.building.turretTopDrawSize, def.building.turretTopDrawSize), Color.white), def, ghostCol, null).DrawFromDef(GenThing.TrueCenter(loc, rot, def.Size, AltitudeLayer.MetaOverlays.AltitudeFor()), rot, def, (float)TurretTop.ArtworkRotation);
		}
	}
}
