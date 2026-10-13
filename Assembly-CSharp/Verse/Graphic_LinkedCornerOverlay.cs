using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008A1 RID: 2209
	public class Graphic_LinkedCornerOverlay : Graphic_Linked
	{
		// Token: 0x060037EF RID: 14319 RVA: 0x0012AC7C File Offset: 0x00128E7C
		public Graphic_LinkedCornerOverlay(Graphic subGraphic) : base(subGraphic)
		{
			this.subGraphic = subGraphic;
			this.data = subGraphic.data;
			this.overlayGraphic = (GraphicDatabase.Get<Graphic_Random>(this.data.cornerOverlayPath, ShaderDatabase.Transparent, this.drawSize, this.color) as Graphic_Random);
		}

		// Token: 0x060037F0 RID: 14320 RVA: 0x0012ACD0 File Offset: 0x00128ED0
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			base.Print(layer, thing, extraRotation);
			IntVec3 position = thing.Position;
			if (this.ShouldLinkWith(position + IntVec3.East, thing) && this.ShouldLinkWith(position + IntVec3.North, thing) && this.ShouldLinkWith(position + IntVec3.NorthEast, thing))
			{
				Rand.PushState(thing.thingIDNumber * 9);
				Material mat = this.overlayGraphic.MatSingleFor(thing);
				float rot = Rand.Range(0f, 360f);
				bool @bool = Rand.Bool;
				Vector2[] uvs;
				Color32 color;
				Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Building, @bool, false, out mat, out uvs, out color);
				Printer_Plane.PrintPlane(layer, thing.TrueCenter() + new Vector3(0.5f, 0.1f, 0.5f), Vector3.one, mat, rot, false, uvs, null, 0.01f, 0f);
				Rand.PopState();
			}
		}

		// Token: 0x040029A8 RID: 10664
		public Graphic_Random overlayGraphic;
	}
}
