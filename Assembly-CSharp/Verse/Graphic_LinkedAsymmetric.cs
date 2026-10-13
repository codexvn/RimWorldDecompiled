using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200089E RID: 2206
	public class Graphic_LinkedAsymmetric : Graphic_Linked
	{
		// Token: 0x060037E4 RID: 14308 RVA: 0x0012A6B4 File Offset: 0x001288B4
		public Graphic_LinkedAsymmetric(Graphic subGraphic) : base(subGraphic)
		{
		}

		// Token: 0x17000A38 RID: 2616
		// (get) Token: 0x060037E5 RID: 14309 RVA: 0x00123991 File Offset: 0x00121B91
		public override LinkDrawerType LinkerType
		{
			get
			{
				return LinkDrawerType.Asymmetric;
			}
		}

		// Token: 0x060037E6 RID: 14310 RVA: 0x0012A6BD File Offset: 0x001288BD
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return new Graphic_LinkedAsymmetric(this.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo))
			{
				data = this.data
			};
		}

		// Token: 0x060037E7 RID: 14311 RVA: 0x0012A6E0 File Offset: 0x001288E0
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			Graphic_LinkedAsymmetric.<>c__DisplayClass4_0 CS$<>8__locals1;
			CS$<>8__locals1.thing = thing;
			CS$<>8__locals1.layer = layer;
			CS$<>8__locals1.extraRotation = extraRotation;
			base.Print(CS$<>8__locals1.layer, CS$<>8__locals1.thing, CS$<>8__locals1.extraRotation);
			if (CS$<>8__locals1.thing.def.graphicData.asymmetricLink == null || !CS$<>8__locals1.thing.def.graphicData.asymmetricLink.linkToDoors)
			{
				return;
			}
			CS$<>8__locals1.cell = CS$<>8__locals1.thing.Position;
			CS$<>8__locals1.map = CS$<>8__locals1.thing.Map;
			if (CS$<>8__locals1.thing.def.graphicData.asymmetricLink.drawDoorBorderEast != null)
			{
				Graphic_LinkedAsymmetric.<Print>g__DrawBorder|4_0(IntVec3.East, CS$<>8__locals1.thing.def.graphicData.asymmetricLink.drawDoorBorderEast, ref CS$<>8__locals1);
			}
			if (CS$<>8__locals1.thing.def.graphicData.asymmetricLink.drawDoorBorderWest != null)
			{
				Graphic_LinkedAsymmetric.<Print>g__DrawBorder|4_0(IntVec3.West, CS$<>8__locals1.thing.def.graphicData.asymmetricLink.drawDoorBorderWest, ref CS$<>8__locals1);
			}
		}

		// Token: 0x060037E8 RID: 14312 RVA: 0x0012A7FC File Offset: 0x001289FC
		public override bool ShouldLinkWith(IntVec3 c, Thing parent)
		{
			if (base.ShouldLinkWith(c, parent))
			{
				return true;
			}
			if (!c.InBounds(parent.Map))
			{
				return (parent.def.graphicData.linkFlags & LinkFlags.MapEdge) > LinkFlags.None;
			}
			if (parent.def.graphicData.asymmetricLink != null)
			{
				if ((parent.Map.linkGrid.LinkFlagsAt(c) & parent.def.graphicData.asymmetricLink.linkFlags) != LinkFlags.None)
				{
					return true;
				}
				if (parent.def.graphicData.asymmetricLink.linkToDoors && c.GetDoor(parent.Map) != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060037E9 RID: 14313 RVA: 0x0012A8A0 File Offset: 0x00128AA0
		[CompilerGenerated]
		internal static void <Print>g__DrawBorder|4_0(IntVec3 dir, AsymmetricLinkData.BorderData border, ref Graphic_LinkedAsymmetric.<>c__DisplayClass4_0 A_2)
		{
			IntVec3 c = A_2.cell + dir;
			if (c.InBounds(A_2.map) && c.GetDoor(A_2.map) != null)
			{
				Vector3 center = A_2.thing.DrawPos + border.offset + Altitudes.AltIncVect;
				Printer_Plane.PrintPlane(A_2.layer, center, border.size, border.Mat, A_2.extraRotation, false, null, null, 0.01f, 0f);
			}
		}
	}
}
