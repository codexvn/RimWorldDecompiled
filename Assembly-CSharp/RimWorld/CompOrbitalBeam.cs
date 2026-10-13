using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002D8D RID: 11661
	[StaticConstructorOnStartup]
	public class CompOrbitalBeam : ThingComp
	{
		// Token: 0x170029C2 RID: 10690
		// (get) Token: 0x06010654 RID: 67156 RVA: 0x004BB4C1 File Offset: 0x004B96C1
		public CompProperties_OrbitalBeam Props
		{
			get
			{
				return (CompProperties_OrbitalBeam)this.props;
			}
		}

		// Token: 0x170029C3 RID: 10691
		// (get) Token: 0x06010655 RID: 67157 RVA: 0x004BB4CE File Offset: 0x004B96CE
		private int TicksPassed
		{
			get
			{
				return Find.TickManager.TicksGame - this.startTick;
			}
		}

		// Token: 0x170029C4 RID: 10692
		// (get) Token: 0x06010656 RID: 67158 RVA: 0x004BB4E1 File Offset: 0x004B96E1
		private int TicksLeft
		{
			get
			{
				return this.totalDuration - this.TicksPassed;
			}
		}

		// Token: 0x170029C5 RID: 10693
		// (get) Token: 0x06010657 RID: 67159 RVA: 0x004BB4F0 File Offset: 0x004B96F0
		private float BeamEndHeight
		{
			get
			{
				return this.Props.width * 0.5f;
			}
		}

		// Token: 0x06010658 RID: 67160 RVA: 0x004BB504 File Offset: 0x004B9704
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Values.Look<int>(ref this.totalDuration, "totalDuration", 0, false);
			Scribe_Values.Look<int>(ref this.fadeOutDuration, "fadeOutDuration", 0, false);
			Scribe_Values.Look<float>(ref this.angle, "angle", 0f, false);
		}

		// Token: 0x06010659 RID: 67161 RVA: 0x004BB563 File Offset: 0x004B9763
		public void StartAnimation(int totalDuration, int fadeOutDuration, float angle)
		{
			this.startTick = Find.TickManager.TicksGame;
			this.totalDuration = totalDuration;
			this.fadeOutDuration = fadeOutDuration;
			this.angle = angle;
			this.CheckSpawnSustainer();
		}

		// Token: 0x0601065A RID: 67162 RVA: 0x004BB590 File Offset: 0x004B9790
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.CheckSpawnSustainer();
		}

		// Token: 0x0601065B RID: 67163 RVA: 0x004BB59F File Offset: 0x004B979F
		public override void CompTick()
		{
			base.CompTick();
			if (this.sustainer != null)
			{
				this.sustainer.Maintain();
				if (this.TicksLeft < this.fadeOutDuration)
				{
					this.sustainer.End();
					this.sustainer = null;
				}
			}
		}

		// Token: 0x0601065C RID: 67164 RVA: 0x004BB5DC File Offset: 0x004B97DC
		public override void PostDraw()
		{
			base.PostDraw();
			if (this.TicksLeft <= 0)
			{
				return;
			}
			Vector3 drawPos = this.parent.DrawPos;
			float num = ((float)this.parent.Map.Size.z - drawPos.z) * 1.4142135f;
			Vector3 a = Vector3Utility.FromAngleFlat(this.angle - 90f);
			Vector3 a2 = drawPos + a * num * 0.5f;
			a2.y = AltitudeLayer.MetaOverlays.AltitudeFor();
			float num2 = Mathf.Min((float)this.TicksPassed / 10f, 1f);
			Vector3 b = a * ((1f - num2) * num);
			float num3 = 0.975f + Mathf.Sin((float)this.TicksPassed * 0.3f) * 0.025f;
			if (this.TicksLeft < this.fadeOutDuration)
			{
				num3 *= (float)this.TicksLeft / (float)this.fadeOutDuration;
			}
			Color color = this.Props.color;
			color.a *= num3;
			CompOrbitalBeam.MatPropertyBlock.SetColor(ShaderPropertyIDs.Color, color);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(a2 + a * this.BeamEndHeight * 0.5f + b, Quaternion.Euler(0f, this.angle, 0f), new Vector3(this.Props.width, 1f, num));
			Graphics.DrawMesh(MeshPool.plane10, matrix, CompOrbitalBeam.BeamMat, 0, null, 0, CompOrbitalBeam.MatPropertyBlock);
			Vector3 pos = drawPos + b;
			pos.y = AltitudeLayer.MetaOverlays.AltitudeFor();
			Matrix4x4 matrix2 = default(Matrix4x4);
			matrix2.SetTRS(pos, Quaternion.Euler(0f, this.angle, 0f), new Vector3(this.Props.width, 1f, this.BeamEndHeight));
			Graphics.DrawMesh(MeshPool.plane10, matrix2, CompOrbitalBeam.BeamEndMat, 0, null, 0, CompOrbitalBeam.MatPropertyBlock);
		}

		// Token: 0x0601065D RID: 67165 RVA: 0x004BB7E1 File Offset: 0x004B99E1
		private void CheckSpawnSustainer()
		{
			if (this.TicksLeft >= this.fadeOutDuration && this.Props.sound != null)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.sustainer = this.Props.sound.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.PerTick));
				});
			}
		}

		// Token: 0x0400AB11 RID: 43793
		private int startTick;

		// Token: 0x0400AB12 RID: 43794
		private int totalDuration;

		// Token: 0x0400AB13 RID: 43795
		private int fadeOutDuration;

		// Token: 0x0400AB14 RID: 43796
		private float angle;

		// Token: 0x0400AB15 RID: 43797
		private Sustainer sustainer;

		// Token: 0x0400AB16 RID: 43798
		private const float AlphaAnimationSpeed = 0.3f;

		// Token: 0x0400AB17 RID: 43799
		private const float AlphaAnimationStrength = 0.025f;

		// Token: 0x0400AB18 RID: 43800
		private const float BeamEndHeightRatio = 0.5f;

		// Token: 0x0400AB19 RID: 43801
		private static readonly Material BeamMat = MaterialPool.MatFrom("Other/OrbitalBeam", ShaderDatabase.MoteGlow, MapMaterialRenderQueues.OrbitalBeam);

		// Token: 0x0400AB1A RID: 43802
		private static readonly Material BeamEndMat = MaterialPool.MatFrom("Other/OrbitalBeamEnd", ShaderDatabase.MoteGlow, MapMaterialRenderQueues.OrbitalBeam);

		// Token: 0x0400AB1B RID: 43803
		private static readonly MaterialPropertyBlock MatPropertyBlock = new MaterialPropertyBlock();
	}
}
