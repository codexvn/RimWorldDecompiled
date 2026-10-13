using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002836 RID: 10294
	public class OrbitLayer : PlanetLayer
	{
		// Token: 0x0600E138 RID: 57656 RVA: 0x00420BC4 File Offset: 0x0041EDC4
		public OrbitLayer()
		{
		}

		// Token: 0x0600E139 RID: 57657 RVA: 0x00420BCC File Offset: 0x0041EDCC
		public OrbitLayer(int layerId, PlanetLayerDef def, float radius, Vector3 origin, Vector3 viewCenter, float viewAngle, int subdivisions, float extraCameraAltitude, float backgroundWorldCameraOffset, float backgroundWorldCameraParallaxDistancePer100Cells) : base(layerId, def, radius, origin, viewCenter, viewAngle, subdivisions, extraCameraAltitude, backgroundWorldCameraOffset, backgroundWorldCameraParallaxDistancePer100Cells)
		{
		}

		// Token: 0x0600E13A RID: 57658 RVA: 0x00420BF0 File Offset: 0x0041EDF0
		public override AcceptanceReport CanSelectLayer()
		{
			AcceptanceReport acceptanceReport = base.CanSelectLayer();
			if (!acceptanceReport)
			{
				return acceptanceReport;
			}
			if (!Find.WorldObjects.AnyWorldObjectOnLayer(this))
			{
				return "CannotSelectOrbitReason".Translate();
			}
			return true;
		}
	}
}
