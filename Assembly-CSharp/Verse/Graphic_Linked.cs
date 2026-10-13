using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200089D RID: 2205
	public class Graphic_Linked : Graphic
	{
		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x060037DB RID: 14299 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual LinkDrawerType LinkerType
		{
			get
			{
				return LinkDrawerType.Basic;
			}
		}

		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x060037DC RID: 14300 RVA: 0x0012A4ED File Offset: 0x001286ED
		public override Material MatSingle
		{
			get
			{
				return MaterialAtlasPool.SubMaterialFromAtlas(this.subGraphic.MatSingle, LinkDirections.None);
			}
		}

		// Token: 0x060037DD RID: 14301 RVA: 0x00129210 File Offset: 0x00127410
		public Graphic_Linked()
		{
		}

		// Token: 0x060037DE RID: 14302 RVA: 0x0012A500 File Offset: 0x00128700
		public Graphic_Linked(Graphic subGraphic)
		{
			this.subGraphic = subGraphic;
			this.data = subGraphic.data;
		}

		// Token: 0x060037DF RID: 14303 RVA: 0x0012A51B File Offset: 0x0012871B
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return new Graphic_Linked(this.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo))
			{
				data = this.data
			};
		}

		// Token: 0x060037E0 RID: 14304 RVA: 0x0012A53C File Offset: 0x0012873C
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			Material mat = this.LinkedDrawMatFrom(thing, thing.Position);
			Printer_Plane.PrintPlane(layer, thing.TrueCenter(), new Vector2(1f, 1f), mat, extraRotation, false, null, null, 0.01f, 0f);
			if (base.ShadowGraphic != null && thing != null)
			{
				base.ShadowGraphic.Print(layer, thing, 0f);
			}
		}

		// Token: 0x060037E1 RID: 14305 RVA: 0x0012A59E File Offset: 0x0012879E
		public override Material MatSingleFor(Thing thing)
		{
			return this.LinkedDrawMatFrom(thing, thing.Position);
		}

		// Token: 0x060037E2 RID: 14306 RVA: 0x0012A5B0 File Offset: 0x001287B0
		protected virtual Material LinkedDrawMatFrom(Thing parent, IntVec3 cell)
		{
			int num = 0;
			int num2 = 1;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = cell + GenAdj.CardinalDirections[i];
				if (this.ShouldLinkWith(c, parent))
				{
					num += num2;
				}
				num2 *= 2;
			}
			LinkDirections linkSet = (LinkDirections)num;
			return MaterialAtlasPool.SubMaterialFromAtlas(this.subGraphic.MatSingleFor(parent), linkSet);
		}

		// Token: 0x060037E3 RID: 14307 RVA: 0x0012A608 File Offset: 0x00128808
		public virtual bool ShouldLinkWith(IntVec3 c, Thing parent)
		{
			if (!parent.Spawned)
			{
				return false;
			}
			if (!c.InBounds(parent.Map))
			{
				return (parent.def.graphicData.linkFlags & LinkFlags.MapEdge) > LinkFlags.None;
			}
			if (ModsConfig.OdysseyActive)
			{
				TerrainDef terrainDef = parent.Map.terrainGrid.FoundationAt(c);
				bool flag = terrainDef != null && terrainDef.IsSubstructure;
				terrainDef = parent.Map.terrainGrid.FoundationAt(parent.Position);
				if (flag != (terrainDef != null && terrainDef.IsSubstructure))
				{
					return false;
				}
			}
			return (parent.Map.linkGrid.LinkFlagsAt(c) & parent.def.graphicData.linkFlags) > LinkFlags.None;
		}

		// Token: 0x0400299C RID: 10652
		protected Graphic subGraphic;
	}
}
