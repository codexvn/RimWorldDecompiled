using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008AA RID: 2218
	public class Graphic_Multi_Mote : Graphic_Multi_AgeSecs
	{
		// Token: 0x06003821 RID: 14369 RVA: 0x0012BB00 File Offset: 0x00129D00
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Mote mote = thing as Mote;
			if (mote != null)
			{
				this.propertyBlock.SetColor(ShaderPropertyIDs.Color, mote.instanceColor);
			}
			base.DrawWorker(loc, rot, thingDef, thing, extraRotation);
		}

		// Token: 0x06003822 RID: 14370 RVA: 0x0012BA43 File Offset: 0x00129C43
		protected override void DrawMeshInt(Mesh mesh, Vector3 loc, Quaternion quat, Material mat)
		{
			Graphics.DrawMesh(mesh, loc, quat, mat, 0, null, 0, this.propertyBlock);
		}
	}
}
