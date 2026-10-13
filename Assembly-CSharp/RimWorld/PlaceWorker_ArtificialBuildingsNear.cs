using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200380F RID: 14351
	public class PlaceWorker_ArtificialBuildingsNear : PlaceWorker
	{
		// Token: 0x06013DB7 RID: 81335 RVA: 0x005E5160 File Offset: 0x005E3360
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			FocusStrengthOffset_ArtificialBuildings focusStrengthOffset_ArtificialBuildings = ((CompProperties_MeditationFocus)def.CompDefFor<CompMeditationFocus>()).offsets.OfType<FocusStrengthOffset_ArtificialBuildings>().FirstOrDefault<FocusStrengthOffset_ArtificialBuildings>();
			if (focusStrengthOffset_ArtificialBuildings != null)
			{
				MeditationUtility.DrawArtificialBuildingOverlay(center, def, Find.CurrentMap, focusStrengthOffset_ArtificialBuildings.radius);
			}
		}
	}
}
