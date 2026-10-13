using System;
using LudeonTK;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008AE RID: 2222
	public class Graphic_Shadow : Graphic
	{
		// Token: 0x06003840 RID: 14400 RVA: 0x0012C3C2 File Offset: 0x0012A5C2
		public Graphic_Shadow(ShadowData shadowInfo)
		{
			this.shadowInfo = shadowInfo;
			if (shadowInfo == null)
			{
				throw new ArgumentNullException("shadowInfo");
			}
			this.shadowMesh = ShadowMeshPool.GetShadowMesh(shadowInfo);
		}

		// Token: 0x06003841 RID: 14401 RVA: 0x0012C3EC File Offset: 0x0012A5EC
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			if (this.shadowMesh != null && thingDef != null && this.shadowInfo != null && (Find.CurrentMap == null || !Find.CurrentMap.Biome.disableShadows) && (Find.CurrentMap == null || !loc.ToIntVec3().InBounds(Find.CurrentMap) || !Find.CurrentMap.roofGrid.Roofed(loc.ToIntVec3())) && DebugViewSettings.drawShadows)
			{
				Vector3 position = loc + this.shadowInfo.offset;
				position.y = AltitudeLayer.Shadows.AltitudeFor();
				Graphics.DrawMesh(this.shadowMesh, position, rot.AsQuat, MatBases.SunShadowFade, 0);
			}
		}

		// Token: 0x06003842 RID: 14402 RVA: 0x0012C4A4 File Offset: 0x0012A6A4
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			Vector3 center = thing.TrueCenter() + (this.shadowInfo.offset + new Vector3(Graphic_Shadow.GlobalShadowPosOffsetX, 0f, Graphic_Shadow.GlobalShadowPosOffsetZ)).RotatedBy(thing.Rotation);
			center.y = AltitudeLayer.Shadows.AltitudeFor();
			Printer_Shadow.PrintShadow(layer, center, this.shadowInfo, thing.Rotation);
		}

		// Token: 0x06003843 RID: 14403 RVA: 0x0012C50D File Offset: 0x0012A70D
		public override string ToString()
		{
			string str = "Graphic_Shadow(";
			ShadowData shadowData = this.shadowInfo;
			return str + ((shadowData != null) ? shadowData.ToString() : null) + ")";
		}

		// Token: 0x040029BB RID: 10683
		private Mesh shadowMesh;

		// Token: 0x040029BC RID: 10684
		private ShadowData shadowInfo;

		// Token: 0x040029BD RID: 10685
		[TweakValue("Graphics_Shadow", -5f, 5f)]
		private static float GlobalShadowPosOffsetX;

		// Token: 0x040029BE RID: 10686
		[TweakValue("Graphics_Shadow", -5f, 5f)]
		private static float GlobalShadowPosOffsetZ;
	}
}
