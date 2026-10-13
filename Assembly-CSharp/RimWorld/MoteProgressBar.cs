using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B1B RID: 11035
	[StaticConstructorOnStartup]
	public class MoteProgressBar : MoteDualAttached
	{
		// Token: 0x17002698 RID: 9880
		// (get) Token: 0x0600F697 RID: 63127 RVA: 0x00479C10 File Offset: 0x00477E10
		protected virtual bool OnlyShowForClosestZoom
		{
			get
			{
				return !this.alwaysShow;
			}
		}

		// Token: 0x0600F698 RID: 63128 RVA: 0x00479C1C File Offset: 0x00477E1C
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.UpdatePositionAndRotation();
			if ((!this.OnlyShowForClosestZoom || Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest) && !Find.ScreenshotModeHandler.Active)
			{
				GenDraw.FillableBarRequest r = default(GenDraw.FillableBarRequest);
				r.center = this.exactPosition;
				r.center.z = r.center.z + this.offsetZ;
				r.size = new Vector2(this.linearScale.x, this.linearScale.z);
				r.fillPercent = this.progress;
				r.filledMat = MoteProgressBar.FilledMat;
				r.unfilledMat = MoteProgressBar.UnfilledMat;
				r.margin = 0.12f;
				if (this.offsetZ >= -0.8f && this.offsetZ <= -0.3f && this.AnyThingWithQualityHere())
				{
					r.center.z = r.center.z + 0.25f;
				}
				GenDraw.DrawFillableBar(r);
			}
		}

		// Token: 0x0600F699 RID: 63129 RVA: 0x00479D10 File Offset: 0x00477F10
		private bool AnyThingWithQualityHere()
		{
			IntVec3 c = this.exactPosition.ToIntVec3();
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].TryGetComp<CompQuality>() != null && (thingList[i].DrawPos - this.exactPosition).MagnitudeHorizontalSquared() < 0.0001f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400A247 RID: 41543
		public float progress;

		// Token: 0x0400A248 RID: 41544
		public float offsetZ;

		// Token: 0x0400A249 RID: 41545
		public bool alwaysShow;

		// Token: 0x0400A24A RID: 41546
		private static readonly Material UnfilledMat = SolidColorMaterials.NewSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f, 0.65f), ShaderDatabase.MetaOverlay);

		// Token: 0x0400A24B RID: 41547
		private static readonly Material FilledMat = SolidColorMaterials.NewSolidColorMaterial(new Color(0.9f, 0.85f, 0.2f, 0.65f), ShaderDatabase.MetaOverlay);
	}
}
