using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000490 RID: 1168
	public abstract class SectionLayer_Things : SectionLayer
	{
		// Token: 0x06001E87 RID: 7815 RVA: 0x000A037E File Offset: 0x0009E57E
		public SectionLayer_Things(Section section) : base(section)
		{
		}

		// Token: 0x06001E88 RID: 7816 RVA: 0x000A0392 File Offset: 0x0009E592
		public override CellRect GetBoundaryRect()
		{
			return this.bounds;
		}

		// Token: 0x06001E89 RID: 7817 RVA: 0x000A039C File Offset: 0x0009E59C
		public override void DrawLayer()
		{
			if (!DebugViewSettings.drawThingsPrinted)
			{
				return;
			}
			if (WorldComponent_GravshipController.CutsceneInProgress && !WorldComponent_GravshipController.GravshipRenderInProgess)
			{
				this.tmpFormerlyEnabled.Clear();
				for (int i = 0; i < this.subMeshes.Count; i++)
				{
					LayerSubMesh layerSubMesh = this.subMeshes[i];
					if (!layerSubMesh.disabled && layerSubMesh.material.renderQueue == 2950)
					{
						this.tmpFormerlyEnabled.Add(layerSubMesh);
						layerSubMesh.disabled = true;
					}
				}
				base.DrawLayer();
				for (int j = 0; j < this.tmpFormerlyEnabled.Count; j++)
				{
					this.tmpFormerlyEnabled[j].disabled = false;
				}
				return;
			}
			base.DrawLayer();
		}

		// Token: 0x06001E8A RID: 7818 RVA: 0x000A0458 File Offset: 0x0009E658
		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			this.bounds = this.section.CellRect;
			foreach (IntVec3 intVec in this.section.CellRect)
			{
				List<Thing> list = base.Map.thingGrid.ThingsListAt(intVec);
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					Thing thing = list[i];
					if ((thing.def.seeThroughFog || !base.Map.fogGrid.IsFogged(thing.Position)) && thing.def.drawerType != DrawerType.None && (thing.def.drawerType != DrawerType.RealtimeOnly || !this.requireAddToMapMesh) && (thing.def.hideAtSnowOrSandDepth >= 1f || Math.Max(base.Map.snowGrid.GetDepth(thing.Position), thing.Position.GetSandDepth(base.Map)) <= thing.def.hideAtSnowOrSandDepth) && (thing.def.plant == null || thing.def.plant.showInFrozenWater || thing.Position.GetTerrain(base.Map) != TerrainDefOf.ThinIce) && thing.Position.x == intVec.x && thing.Position.z == intVec.z)
					{
						this.TakePrintFrom(thing);
						this.bounds = this.bounds.Encapsulate(thing.OccupiedDrawRect());
					}
				}
			}
			base.FinalizeMesh(MeshParts.All);
		}

		// Token: 0x06001E8B RID: 7819
		protected abstract void TakePrintFrom(Thing t);

		// Token: 0x04001A4D RID: 6733
		private CellRect bounds;

		// Token: 0x04001A4E RID: 6734
		private List<LayerSubMesh> tmpFormerlyEnabled = new List<LayerSubMesh>();

		// Token: 0x04001A4F RID: 6735
		protected bool requireAddToMapMesh;
	}
}
