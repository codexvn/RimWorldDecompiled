using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A35 RID: 10805
	public class Building_MechRelay : Building
	{
		// Token: 0x1700254E RID: 9550
		// (get) Token: 0x0600EFCC RID: 61388 RVA: 0x004598FB File Offset: 0x00457AFB
		private Material ShadowMaterial
		{
			get
			{
				if (this.cachedShadowMaterial == null)
				{
					this.cachedShadowMaterial = MaterialPool.MatFrom("Things/Skyfaller/SkyfallerShadowDropPod", ShaderDatabase.Transparent);
				}
				return this.cachedShadowMaterial;
			}
		}

		// Token: 0x0600EFCD RID: 61389 RVA: 0x00459928 File Offset: 0x00457B28
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			if (base.Spawned)
			{
				if (this.compMechRelay == null)
				{
					this.compMechRelay = base.GetComp<CompMechRelay>();
				}
				Skyfaller.DrawDropSpotShadow(drawLoc, Rot4.North, this.ShadowMaterial, this.def.size.ToVector2(), 65);
				float a = 0.55f + 0.5f * (1f + Mathf.Sin(6.2831855f * (float)GenTicks.TicksGame / 300f)) * 0.35f;
				float t = (this.compMechRelay.DestabilizationTick == -1) ? 0f : Mathf.Pow(1f - Mathf.Clamp01((float)(this.compMechRelay.DestabilizationTick - Find.TickManager.TicksGame) / 120f), 2f);
				float num = Mathf.Lerp(a, 0f, t);
				float extraRotation = Mathf.Lerp(0f, 45f, t);
				drawLoc.z += num;
				this.Graphic.Draw(drawLoc, flip ? base.Rotation.Opposite : base.Rotation, this, extraRotation);
				SilhouetteUtility.DrawGraphicSilhouette(this, drawLoc);
				return;
			}
			base.DrawAt(drawLoc, flip);
		}

		// Token: 0x04009E63 RID: 40547
		[Unsaved(false)]
		private Material cachedShadowMaterial;

		// Token: 0x04009E64 RID: 40548
		[Unsaved(false)]
		private CompMechRelay compMechRelay;

		// Token: 0x04009E65 RID: 40549
		private const float ZOffset = 0.55f;

		// Token: 0x04009E66 RID: 40550
		private const float BobHeight = 0.35f;

		// Token: 0x04009E67 RID: 40551
		private const float CrashTicks = 120f;

		// Token: 0x04009E68 RID: 40552
		private const float CrashRotation = 45f;

		// Token: 0x04009E69 RID: 40553
		private const float CrashProgressExponent = 2f;
	}
}
