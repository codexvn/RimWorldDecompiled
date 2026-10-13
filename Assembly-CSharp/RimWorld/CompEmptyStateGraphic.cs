using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C72 RID: 11378
	public class CompEmptyStateGraphic : ThingComp
	{
		// Token: 0x17002839 RID: 10297
		// (get) Token: 0x0600FF29 RID: 65321 RVA: 0x0049F493 File Offset: 0x0049D693
		private CompProperties_EmptyStateGraphic Props
		{
			get
			{
				return (CompProperties_EmptyStateGraphic)this.props;
			}
		}

		// Token: 0x1700283A RID: 10298
		// (get) Token: 0x0600FF2A RID: 65322 RVA: 0x0049F4A0 File Offset: 0x0049D6A0
		public bool ParentIsEmpty
		{
			get
			{
				IThingHolder thingHolder = this.parent as IThingHolder;
				if (thingHolder != null && thingHolder.GetDirectlyHeldThings().NullOrEmpty<Thing>())
				{
					return true;
				}
				CompPawnSpawnOnWakeup compPawnSpawnOnWakeup = this.parent.TryGetComp<CompPawnSpawnOnWakeup>();
				return compPawnSpawnOnWakeup != null && !compPawnSpawnOnWakeup.CanSpawn;
			}
		}

		// Token: 0x0600FF2B RID: 65323 RVA: 0x0049F4E5 File Offset: 0x0049D6E5
		public override bool DontDrawParent()
		{
			return this.ParentIsEmpty && !this.Props.alwaysDrawParent;
		}

		// Token: 0x0600FF2C RID: 65324 RVA: 0x0049F500 File Offset: 0x0049D700
		public override void PostDraw()
		{
			if (!this.ParentIsEmpty || this.parent.def.drawerType == DrawerType.MapMeshOnly)
			{
				return;
			}
			Mesh mesh = this.Props.graphicData.Graphic.MeshAt(this.parent.Rotation);
			Vector3 drawPos = this.parent.DrawPos;
			Graphics.DrawMesh(mesh, drawPos + this.Props.graphicData.drawOffset.RotatedBy(this.parent.Rotation), Quaternion.identity, this.Props.graphicData.Graphic.MatAt(this.parent.Rotation, null), 0);
		}

		// Token: 0x0600FF2D RID: 65325 RVA: 0x0049F5A7 File Offset: 0x0049D7A7
		public override void PostPrintOnto(SectionLayer layer)
		{
			if (this.ParentIsEmpty)
			{
				this.Props.graphicData.Graphic.Print(layer, this.parent, 0f);
			}
		}
	}
}
