using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008B0 RID: 2224
	public class Graphic_Single_AgeSecs : Graphic_Single
	{
		// Token: 0x06003851 RID: 14417 RVA: 0x0012A3C8 File Offset: 0x001285C8
		public float AgeSecs(Thing thing)
		{
			return (float)(Find.TickManager.TicksGame - thing.TickSpawned) / 60f;
		}

		// Token: 0x06003852 RID: 14418 RVA: 0x0012C72F File Offset: 0x0012A92F
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			this.mat.SetFloat(ShaderPropertyIDs.AgeSecs, this.AgeSecs(thing));
			base.DrawWorker(loc, rot, thingDef, thing, extraRotation);
		}
	}
}
