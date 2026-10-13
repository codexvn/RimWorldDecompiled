using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BCD RID: 11213
	public class CompAncientHeatVent : CompAncientVent
	{
		// Token: 0x1700274F RID: 10063
		// (get) Token: 0x0600FADA RID: 64218 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool AppliesEffectsToPawns
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002750 RID: 10064
		// (get) Token: 0x0600FADB RID: 64219 RVA: 0x0048E33D File Offset: 0x0048C53D
		public new CompProperties_AncientHeatVent Props
		{
			get
			{
				return (CompProperties_AncientHeatVent)this.props;
			}
		}

		// Token: 0x0600FADC RID: 64220 RVA: 0x000026EA File Offset: 0x000008EA
		protected override void ToggleIndividualVent(bool on)
		{
		}

		// Token: 0x0600FADD RID: 64221 RVA: 0x0048E34C File Offset: 0x0048C54C
		private void DrawFleckQuad(FleckDef def, MaterialPropertyBlock props)
		{
			GraphicData graphicData = def.graphicData;
			Vector3 pos = this.parent.Position.ToVector3Shifted() + graphicData.drawOffset;
			pos.y = AltitudeLayer.MoteOverhead.AltitudeFor();
			Matrix4x4 matrix = Matrix4x4.TRS(pos, Quaternion.identity, (this.parent.DrawSize * graphicData.drawSize).ToVector3());
			Graphics.DrawMesh(MeshPool.plane10, matrix, graphicData.Graphic.MatSingle, 0, null, 0, props);
		}

		// Token: 0x0600FADE RID: 64222 RVA: 0x0048E3D0 File Offset: 0x0048C5D0
		private void DrawGlow()
		{
			GraphicData graphicData = FleckDefOf.AncientVentHeatGlow.graphicData;
			this.glowProps = new MaterialPropertyBlock();
			this.glowProps.SetColor(ShaderPropertyIDs.Color, graphicData.color.WithAlpha(this.intensity * this.targetGlow * graphicData.color.a));
			this.DrawFleckQuad(FleckDefOf.AncientVentHeatGlow, this.glowProps);
		}

		// Token: 0x0600FADF RID: 64223 RVA: 0x0048E438 File Offset: 0x0048C638
		private void DrawHeatShimmer()
		{
			if (this.heatShimmerProps == null)
			{
				this.heatShimmerProps = new MaterialPropertyBlock();
			}
			this.heatShimmerProps.SetFloat(CompAncientHeatVent.shaderPropertyIDIntensity, this.intensity);
			this.DrawFleckQuad(FleckDefOf.AncientVentHeatShimmer, this.glowProps);
		}

		// Token: 0x0600FAE0 RID: 64224 RVA: 0x0048E474 File Offset: 0x0048C674
		public override void PostDraw()
		{
			if (!base.VentOn)
			{
				return;
			}
			this.DrawGlow();
			this.DrawHeatShimmer();
		}

		// Token: 0x0600FAE1 RID: 64225 RVA: 0x0048E48B File Offset: 0x0048C68B
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.intensity = (base.VentOn ? 1f : 0f);
			CompAncientHeatVent.shaderPropertyIDIntensity = Shader.PropertyToID("_Intensity");
		}

		// Token: 0x0600FAE2 RID: 64226 RVA: 0x0048E4C0 File Offset: 0x0048C6C0
		public override void CompTickInterval(int delta)
		{
			if (base.VentOn && this.intensity < 1f)
			{
				this.intensity += (float)delta * 0.016666668f / this.Props.rampUpTime;
				if (this.intensity > 1f)
				{
					this.intensity = 1f;
					return;
				}
			}
			else if (this.intensity > 0f)
			{
				this.targetGlow = Mathf.Lerp(this.Props.minGlowBrightness, this.Props.maxGlowBrightness, 0.5f + 0.5f * Mathf.Sin((float)GenTicks.TicksGame * 0.016666668f * this.Props.pulseSpeed));
				if (!base.VentOn)
				{
					this.intensity -= (float)delta * 0.016666668f / this.Props.rampDownTime;
					if (this.intensity < 0f)
					{
						this.intensity = 0f;
					}
				}
			}
		}

		// Token: 0x0400A4BA RID: 42170
		private float targetGlow = 1f;

		// Token: 0x0400A4BB RID: 42171
		private float intensity;

		// Token: 0x0400A4BC RID: 42172
		private MaterialPropertyBlock glowProps;

		// Token: 0x0400A4BD RID: 42173
		private MaterialPropertyBlock heatShimmerProps;

		// Token: 0x0400A4BE RID: 42174
		private static int shaderPropertyIDIntensity;
	}
}
