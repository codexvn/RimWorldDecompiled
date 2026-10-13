using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002151 RID: 8529
	[StaticConstructorOnStartup]
	public static class PowerNetGraphics
	{
		// Token: 0x0600BA63 RID: 47715 RVA: 0x0035C9FC File Offset: 0x0035ABFC
		public static void PrintWirePieceConnecting(SectionLayer layer, Thing A, Thing B, bool forPowerOverlay)
		{
			Material mat = PowerNetGraphics.WireMat;
			float y = AltitudeLayer.SmallWire.AltitudeFor();
			if (forPowerOverlay)
			{
				mat = PowerOverlayMats.MatConnectorLine;
				y = AltitudeLayer.MapDataOverlay.AltitudeFor();
			}
			Vector3 center = (A.TrueCenter() + A.Graphic.DrawOffset(A.Rotation) + B.TrueCenter() + B.Graphic.DrawOffset(B.Rotation)) / 2f;
			center.y = y;
			Vector3 v = B.TrueCenter() - A.TrueCenter();
			Vector2 size = new Vector2(1f, v.MagnitudeHorizontal());
			float rot = v.AngleFlat();
			Printer_Plane.PrintPlane(layer, center, size, mat, rot, false, null, null, 0.01f, 0f);
		}

		// Token: 0x0600BA64 RID: 47716 RVA: 0x0035CAC0 File Offset: 0x0035ACC0
		public static void RenderAnticipatedWirePieceConnecting(IntVec3 userPos, Rot4 rotation, IntVec2 thingSize, Thing transmitter)
		{
			Vector3 vector = GenThing.TrueCenter(userPos, rotation, thingSize, AltitudeLayer.MapDataOverlay.AltitudeFor());
			if (userPos == transmitter.Position)
			{
				return;
			}
			Vector3 vector2 = transmitter.TrueCenter();
			vector2.y = AltitudeLayer.MapDataOverlay.AltitudeFor();
			Vector3 pos = (vector + vector2) / 2f;
			Vector3 v = vector2 - vector;
			Vector3 s = new Vector3(1f, 1f, v.MagnitudeHorizontal());
			Quaternion q = Quaternion.LookRotation(vector2 - vector);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(pos, q, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, PowerOverlayMats.MatConnectorAnticipated, 0);
		}

		// Token: 0x0600BA65 RID: 47717 RVA: 0x0035CB68 File Offset: 0x0035AD68
		public static void PrintOverlayConnectorBaseFor(SectionLayer layer, Thing t)
		{
			Vector3 center = t.TrueCenter();
			center.y = AltitudeLayer.MapDataOverlay.AltitudeFor();
			Printer_Plane.PrintPlane(layer, center, new Vector2(1f, 1f), PowerOverlayMats.MatConnectorBase, 0f, false, null, null, 0.01f, 0f);
		}

		// Token: 0x04008086 RID: 32902
		private const AltitudeLayer WireAltitude = AltitudeLayer.SmallWire;

		// Token: 0x04008087 RID: 32903
		private static readonly Material WireMat = MaterialPool.MatFrom("Things/Special/Power/Wire");
	}
}
