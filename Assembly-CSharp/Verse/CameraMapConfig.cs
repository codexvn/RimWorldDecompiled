using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x020001D5 RID: 469
	public abstract class CameraMapConfig : IExposable
	{
		// Token: 0x06000CE5 RID: 3301 RVA: 0x0003CFF8 File Offset: 0x0003B1F8
		public virtual void ConfigFixedUpdate_60(ref Vector3 rootPos, ref Vector3 velocity)
		{
			if (this.followSelected)
			{
				List<Pawn> selectedPawns = Find.Selector.SelectedPawns;
				if (selectedPawns.Count > 0)
				{
					Vector3 vector = Vector3.zero;
					int num = 0;
					foreach (Pawn pawn in selectedPawns)
					{
						if (pawn.MapHeld == Find.CurrentMap)
						{
							vector += pawn.TrueCenter();
							num++;
						}
					}
					if (num > 0)
					{
						vector /= (float)num;
						vector.y = rootPos.y;
						rootPos = Vector3.MoveTowards(rootPos, vector, 0.02f * Mathf.Max(Find.TickManager.TickRateMultiplier, 1f) * this.moveSpeedScale);
					}
				}
			}
			if (this.autoPanSpeed > 0f && (Find.TickManager.CurTimeSpeed != TimeSpeed.Paused || this.autoPanWhilePaused))
			{
				velocity.x = Mathf.Cos(this.autoPanAngle) * this.autoPanSpeed;
				velocity.z = Mathf.Sin(this.autoPanAngle) * this.autoPanSpeed;
			}
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ConfigOnGUI()
		{
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x0003D128 File Offset: 0x0003B328
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.moveSpeedScale, "moveSpeedScale", 0f, false);
			Scribe_Values.Look<float>(ref this.zoomSpeed, "zoomSpeed", 0f, false);
			Scribe_Values.Look<FloatRange>(ref this.sizeRange, "sizeRange", default(FloatRange), false);
			Scribe_Values.Look<float>(ref this.zoomPreserveFactor, "zoomPreserveFactor", 0f, false);
			Scribe_Values.Look<bool>(ref this.smoothZoom, "smoothZoom", false, false);
			Scribe_Values.Look<bool>(ref this.followSelected, "followSelected", false, false);
			Scribe_Values.Look<float>(ref this.autoPanTargetAngle, "autoPanTargetAngle", 0f, false);
			Scribe_Values.Look<float>(ref this.autoPanSpeed, "autoPanSpeed", 0f, false);
			Scribe_Values.Look<string>(ref this.fileName, "fileName", null, false);
			Scribe_Values.Look<bool>(ref this.autoPanWhilePaused, "autoPanWhilePaused", false, false);
		}

		// Token: 0x0400076F RID: 1903
		public float dollyRateKeys = 50f;

		// Token: 0x04000770 RID: 1904
		public float dollyRateScreenEdge = 35f;

		// Token: 0x04000771 RID: 1905
		public float camSpeedDecayFactor = 0.85f;

		// Token: 0x04000772 RID: 1906
		public float moveSpeedScale = 2f;

		// Token: 0x04000773 RID: 1907
		public float zoomSpeed = 2.6f;

		// Token: 0x04000774 RID: 1908
		public FloatRange sizeRange = new FloatRange(SteamDeck.IsSteamDeck ? 7.2f : 11f, 60f);

		// Token: 0x04000775 RID: 1909
		public float zoomPreserveFactor;

		// Token: 0x04000776 RID: 1910
		public bool smoothZoom;

		// Token: 0x04000777 RID: 1911
		public bool followSelected;

		// Token: 0x04000778 RID: 1912
		public string fileName;

		// Token: 0x04000779 RID: 1913
		public bool autoPanWhilePaused;

		// Token: 0x0400077A RID: 1914
		public float autoPanTargetAngle;

		// Token: 0x0400077B RID: 1915
		public float autoPanAngle;

		// Token: 0x0400077C RID: 1916
		public float autoPanSpeed;

		// Token: 0x0400077D RID: 1917
		public bool gravshipFreeCam;

		// Token: 0x0400077E RID: 1918
		public bool gravshipPanOnCutsceneStart = true;

		// Token: 0x0400077F RID: 1919
		public bool gravshipEnableOverrideSizeRange;

		// Token: 0x04000780 RID: 1920
		public FloatRange gravshipOverrideSizeRange = new FloatRange(15f, 60f);
	}
}
