using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008B1 RID: 2225
	public class Graphic_Single_SquashNStretch : Graphic_WithPropertyBlock
	{
		// Token: 0x06003854 RID: 14420 RVA: 0x0012A3C8 File Offset: 0x001285C8
		public float AgeSecs(Thing thing)
		{
			return (float)(Find.TickManager.TicksGame - thing.TickSpawned) / 60f;
		}

		// Token: 0x06003855 RID: 14421 RVA: 0x0012C758 File Offset: 0x0012A958
		public override void Init(GraphicRequest req)
		{
			base.Init(req);
			this.snsProps = new Vector4(this.data.maxSnS.x, this.data.maxSnS.y, this.data.offsetSnS.x, this.data.offsetSnS.y);
		}

		// Token: 0x06003856 RID: 14422 RVA: 0x0012C7B8 File Offset: 0x0012A9B8
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			this.mat.SetFloat(ShaderPropertyIDs.AgeSecs, this.AgeSecs(thing));
			this.propertyBlock.SetVector(ShaderPropertyIDs.SquashNStretch, this.snsProps);
			this.propertyBlock.SetFloat(ShaderPropertyIDs.RandomPerObject, (float)thing.thingIDNumber.HashOffset());
			base.DrawWorker(loc, rot, thingDef, thing, extraRotation);
		}

		// Token: 0x040029C1 RID: 10689
		private Vector4 snsProps;
	}
}
