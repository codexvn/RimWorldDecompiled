using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008A9 RID: 2217
	public class Graphic_Multi_BuildingWorking : Graphic_Multi_AgeSecs
	{
		// Token: 0x0600381F RID: 14367 RVA: 0x0012BA6C File Offset: 0x00129C6C
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			this.MatAt(rot, thing);
			Building building = thing as Building;
			if (building != null)
			{
				this.propertyBlock.SetFloat(ShaderPropertyIDs.Working, building.IsWorking() ? 1f : 0f);
			}
			this.propertyBlock.SetFloat(ShaderPropertyIDs.Rotation, (float)rot.AsInt);
			if (thing != null)
			{
				this.propertyBlock.SetFloat(ShaderPropertyIDs.RandomPerObject, (float)thing.thingIDNumber.HashOffset());
			}
			base.DrawWorker(loc, rot, thingDef, thing, extraRotation);
		}
	}
}
