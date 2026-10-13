using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DD5 RID: 11733
	[StaticConstructorOnStartup]
	public class CompRadialPulse : ThingComp
	{
		// Token: 0x17002A1B RID: 10779
		// (get) Token: 0x060107FC RID: 67580 RVA: 0x004C232E File Offset: 0x004C052E
		private CompProperties_RadialPulse Props
		{
			get
			{
				return (CompProperties_RadialPulse)this.props;
			}
		}

		// Token: 0x17002A1C RID: 10780
		// (get) Token: 0x060107FD RID: 67581 RVA: 0x004C233B File Offset: 0x004C053B
		private float RingLerpFactor
		{
			get
			{
				return (float)(Find.TickManager.TicksGame % this.Props.ticksBetweenPulses) / (float)this.Props.ticksPerPulse;
			}
		}

		// Token: 0x17002A1D RID: 10781
		// (get) Token: 0x060107FE RID: 67582 RVA: 0x004C2361 File Offset: 0x004C0561
		private float RingScale
		{
			get
			{
				return this.Props.radius * Mathf.Lerp(0f, 2f, this.RingLerpFactor) * 1.1601562f;
			}
		}

		// Token: 0x17002A1E RID: 10782
		// (get) Token: 0x060107FF RID: 67583 RVA: 0x004C238C File Offset: 0x004C058C
		private bool ParentIsActive
		{
			get
			{
				CompSendSignalOnMotion comp = this.parent.GetComp<CompSendSignalOnMotion>();
				return comp != null && comp.Sent;
			}
		}

		// Token: 0x06010800 RID: 67584 RVA: 0x004C23B0 File Offset: 0x004C05B0
		public override void PostDraw()
		{
			if (this.ParentIsActive)
			{
				return;
			}
			Vector3 pos = this.parent.Position.ToVector3Shifted();
			pos.y = AltitudeLayer.MoteOverhead.AltitudeFor();
			Color color = this.Props.color;
			color.a = Mathf.Lerp(this.Props.color.a, 0f, this.RingLerpFactor);
			CompRadialPulse.MatPropertyBlock.SetColor(ShaderPropertyIDs.Color, color);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(pos, Quaternion.identity, new Vector3(this.RingScale, 1f, this.RingScale));
			Graphics.DrawMesh(MeshPool.plane10, matrix, CompRadialPulse.RingMat, 0, null, 0, CompRadialPulse.MatPropertyBlock);
		}

		// Token: 0x0400AC14 RID: 44052
		private static readonly Material RingMat = MaterialPool.MatFrom("Other/ForceField", ShaderDatabase.MoteGlow);

		// Token: 0x0400AC15 RID: 44053
		private static readonly MaterialPropertyBlock MatPropertyBlock = new MaterialPropertyBlock();

		// Token: 0x0400AC16 RID: 44054
		private const float TextureActualRingSizeFactor = 1.1601562f;
	}
}
