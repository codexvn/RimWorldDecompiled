using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200088B RID: 2187
	public class Graphic_ActivityMaskRandom : Graphic_WithPropertyBlockRandom
	{
		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x0600377A RID: 14202 RVA: 0x00128D36 File Offset: 0x00126F36
		protected override Type SingleGraphicType
		{
			get
			{
				return typeof(Graphic_ActivityMask);
			}
		}

		// Token: 0x0600377B RID: 14203 RVA: 0x00128D44 File Offset: 0x00126F44
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
