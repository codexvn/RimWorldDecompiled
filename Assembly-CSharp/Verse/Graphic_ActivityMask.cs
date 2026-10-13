using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200088A RID: 2186
	public class Graphic_ActivityMask : Graphic_WithPropertyBlock
	{
		// Token: 0x06003778 RID: 14200 RVA: 0x00128CC4 File Offset: 0x00126EC4
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			CompActivity compActivity = thing.TryGetComp<CompActivity>();
			if (compActivity == null)
			{
				Log.ErrorOnce(thingDef.defName + ": Graphic_ActivityMask requires CompActivity.", 6134621);
				return;
			}
			Color colorTwo = this.colorTwo;
			colorTwo.a = Mathf.Clamp01(compActivity.ActivityLevel);
			this.propertyBlock.SetColor(ShaderPropertyIDs.ColorTwo, colorTwo);
			base.DrawWorker(loc, rot, thingDef, thing, extraRotation);
		}
	}
}
