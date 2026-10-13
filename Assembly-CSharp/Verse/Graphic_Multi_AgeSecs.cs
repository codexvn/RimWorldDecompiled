using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008A8 RID: 2216
	public class Graphic_Multi_AgeSecs : Graphic_Multi
	{
		// Token: 0x0600381B RID: 14363 RVA: 0x0012A3C8 File Offset: 0x001285C8
		public float AgeSecs(Thing thing)
		{
			return (float)(Find.TickManager.TicksGame - thing.TickSpawned) / 60f;
		}

		// Token: 0x0600381C RID: 14364 RVA: 0x0012BA08 File Offset: 0x00129C08
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Material material = this.MatAt(rot, thing);
			if (thing != null)
			{
				material.SetFloat(ShaderPropertyIDs.AgeSecs, this.AgeSecs(thing));
			}
			base.DrawWorker(loc, rot, thingDef, thing, extraRotation);
		}

		// Token: 0x0600381D RID: 14365 RVA: 0x0012BA43 File Offset: 0x00129C43
		protected override void DrawMeshInt(Mesh mesh, Vector3 loc, Quaternion quat, Material mat)
		{
			Graphics.DrawMesh(mesh, loc, quat, mat, 0, null, 0, this.propertyBlock);
		}

		// Token: 0x040029B3 RID: 10675
		public MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
	}
}
