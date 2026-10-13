using System;
using LudeonTK;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000546 RID: 1350
	public class TexturePannerSpeedCurve : TexturePanner
	{
		// Token: 0x0600230A RID: 8970 RVA: 0x000B6884 File Offset: 0x000B4A84
		public TexturePannerSpeedCurve(Material material, Vector2 direction, ComplexCurve timescaleCurve, float speed) : base(material, Shader.PropertyToID("_MainTex"), direction, speed)
		{
			this.timescaleCurve = timescaleCurve;
		}

		// Token: 0x0600230B RID: 8971 RVA: 0x000B68A1 File Offset: 0x000B4AA1
		public TexturePannerSpeedCurve(Material material, string property, ComplexCurve timescaleCurve, Vector2 direction, float speed) : base(material, Shader.PropertyToID(property), direction, speed)
		{
			this.timescaleCurve = timescaleCurve;
		}

		// Token: 0x0600230C RID: 8972 RVA: 0x000B68BB File Offset: 0x000B4ABB
		public TexturePannerSpeedCurve(Material material, int propertyID, ComplexCurve timescaleCurve, Vector2 direction, float speed) : base(material, propertyID, direction, speed)
		{
			this.timescaleCurve = timescaleCurve;
		}

		// Token: 0x0600230D RID: 8973 RVA: 0x000B68D0 File Offset: 0x000B4AD0
		public override void Tick()
		{
			this.pan -= this.direction * this.speed * this.material.GetTextureScale(this.propertyID).x * this.timescaleCurve.Evaluate(Find.TickManager.TickRateMultiplier);
			this.material.SetTextureOffset(this.propertyID, this.pan);
		}

		// Token: 0x04001D49 RID: 7497
		public ComplexCurve timescaleCurve;
	}
}
