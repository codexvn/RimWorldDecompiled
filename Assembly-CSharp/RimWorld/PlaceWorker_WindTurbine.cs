using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020037EB RID: 14315
	public class PlaceWorker_WindTurbine : PlaceWorker
	{
		// Token: 0x06013D56 RID: 81238 RVA: 0x005E3B3D File Offset: 0x005E1D3D
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			GenDraw.DrawFieldEdges(WindTurbineUtility.CalculateWindCells(center, rot, def.size).ToList<IntVec3>(), 2900);
		}
	}
}
