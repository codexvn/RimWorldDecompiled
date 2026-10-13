using System;
using System.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C9F RID: 15519
	public abstract class WorldDrawLayer_WorldObjects : WorldDrawLayer
	{
		// Token: 0x1700348D RID: 13453
		// (get) Token: 0x06015177 RID: 86391 RVA: 0x00002501 File Offset: 0x00000701
		public override bool VisibleWhenLayerNotSelected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700348E RID: 13454
		// (get) Token: 0x06015178 RID: 86392 RVA: 0x00002501 File Offset: 0x00000701
		public override bool VisibleInBackground
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700348F RID: 13455
		// (get) Token: 0x06015179 RID: 86393 RVA: 0x0064FFBF File Offset: 0x0064E1BF
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawWorldObjects;
			}
		}

		// Token: 0x0601517A RID: 86394
		protected abstract bool ShouldSkip(WorldObject worldObject);

		// Token: 0x0601517B RID: 86395 RVA: 0x0064FFC6 File Offset: 0x0064E1C6
		public override IEnumerable Regenerate()
		{
			foreach (object obj in base.Regenerate())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			foreach (WorldObject worldObject in Find.WorldObjects.AllWorldObjectsOnLayer(this.planetLayer))
			{
				if (!worldObject.def.useDynamicDrawer && !this.ShouldSkip(worldObject))
				{
					Material material = worldObject.Material;
					if (material == null)
					{
						Log.ErrorOnce(string.Format("World object {0} returned null material.", worldObject), Gen.HashCombineInt(1948576891, worldObject.ID));
					}
					else
					{
						LayerSubMesh subMesh = base.GetSubMesh(material);
						Rand.PushState();
						Rand.Seed = worldObject.ID;
						worldObject.Print(subMesh);
						Rand.PopState();
					}
				}
			}
			base.FinalizeMesh(MeshParts.All);
			yield break;
			yield break;
		}
	}
}
