using System;
using System.Collections.Generic;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D51 RID: 7505
	public class GameCondition_VolcanicAsh : GameCondition
	{
		// Token: 0x170019FE RID: 6654
		// (get) Token: 0x0600A836 RID: 43062 RVA: 0x00222D4F File Offset: 0x00220F4F
		public override int TransitionTicks
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x0600A837 RID: 43063 RVA: 0x003000A3 File Offset: 0x002FE2A3
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 1f);
		}

		// Token: 0x0600A838 RID: 43064 RVA: 0x003026B1 File Offset: 0x003008B1
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(0f, GameCondition_VolcanicAsh.AshSkyColors, 1f, 0f));
		}

		// Token: 0x0600A839 RID: 43065 RVA: 0x003026D1 File Offset: 0x003008D1
		public override void Init()
		{
			if (!ModLister.CheckOdyssey("Volcanic ash"))
			{
				this.End();
				return;
			}
			base.Init();
		}

		// Token: 0x0600A83A RID: 43066 RVA: 0x003026EC File Offset: 0x003008EC
		public override List<SkyOverlay> SkyOverlays(Map map)
		{
			return GameCondition_VolcanicAsh.overlays;
		}

		// Token: 0x0600A83B RID: 43067 RVA: 0x003026F3 File Offset: 0x003008F3
		public override void GameConditionDraw(Map map)
		{
			if (!base.HiddenByOtherCondition(map))
			{
				GameCondition_VolcanicAsh.volcanicAshOverlay.DrawOverlay(map);
			}
		}

		// Token: 0x0600A83C RID: 43068 RVA: 0x0030270C File Offset: 0x0030090C
		public override void GameConditionTick()
		{
			if (Find.TickManager.TicksGame % 3251 == 0)
			{
				foreach (Map map in base.AffectedMaps)
				{
					foreach (Pawn pawn in map.mapPawns.AllPawnsSpawned)
					{
						if (!pawn.Position.Roofed(map) && !pawn.kindDef.immuneToGameConditionEffects && GasUtility.IsAffectedByExposure(pawn))
						{
							this.GiveOrUpdateHediff(pawn);
						}
					}
				}
			}
			foreach (Map map2 in base.AffectedMaps)
			{
				if (!base.HiddenByOtherCondition(map2))
				{
					GameCondition_VolcanicAsh.volcanicAshOverlay.TickOverlay(map2, 1f);
				}
			}
		}

		// Token: 0x0600A83D RID: 43069 RVA: 0x00302828 File Offset: 0x00300A28
		private void GiveOrUpdateHediff(Pawn target)
		{
			Hediff hediff = target.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.VolcanicAsh, false);
			if (hediff == null)
			{
				hediff = target.health.AddHediff(HediffDefOf.VolcanicAsh, null, null, null);
			}
			hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = 3256;
		}

		// Token: 0x0400745B RID: 29787
		private static readonly SkyColorSet AshSkyColors = new SkyColorSet(new Color(0.482f, 0.403f, 0.402f), Color.white, new Color(0.6f, 0.6f, 0.6f), 1f);

		// Token: 0x0400745C RID: 29788
		private static readonly GameCondition_VolcanicAsh.VolcanicAshOverlay volcanicAshOverlay = new GameCondition_VolcanicAsh.VolcanicAshOverlay();

		// Token: 0x0400745D RID: 29789
		private static readonly List<SkyOverlay> overlays = new List<SkyOverlay>
		{
			GameCondition_VolcanicAsh.volcanicAshOverlay
		};

		// Token: 0x0400745E RID: 29790
		private const int CheckInterval = 3251;

		// Token: 0x02001D52 RID: 7506
		[StaticConstructorOnStartup]
		private class VolcanicAshOverlay : WeatherOverlayDualPanner
		{
			// Token: 0x0600A840 RID: 43072 RVA: 0x003028E4 File Offset: 0x00300AE4
			public override void DrawOverlay(Map map)
			{
				SkyOverlay.DrawWorldOverlay(map, GameCondition_VolcanicAsh.VolcanicAshOverlay.CloudLayer, 0);
				SkyOverlay.DrawWorldOverlay(map, GameCondition_VolcanicAsh.VolcanicAshOverlay.ParticleLayer, 0);
			}

			// Token: 0x0600A841 RID: 43073 RVA: 0x003028FE File Offset: 0x00300AFE
			public override void SetOverlayColor(Color color)
			{
				GameCondition_VolcanicAsh.VolcanicAshOverlay.CloudLayer.color = color;
				GameCondition_VolcanicAsh.VolcanicAshOverlay.ParticleLayer.color = color;
			}

			// Token: 0x0600A842 RID: 43074 RVA: 0x00302916 File Offset: 0x00300B16
			public override void TickOverlay(Map map, float lerpFactor)
			{
				this.panner0.Tick();
				this.panner1.Tick();
				this.panner2.Tick();
				this.panner3.Tick();
			}

			// Token: 0x0400745F RID: 29791
			private static readonly Material CloudLayer = MatLoader.LoadMat("Weather/VolcanicAsh/CloudLayer", -1);

			// Token: 0x04007460 RID: 29792
			private static readonly Material ParticleLayer = MatLoader.LoadMat("Weather/VolcanicAsh/ParticleLayer", -1);

			// Token: 0x04007461 RID: 29793
			private static readonly ComplexCurve speedCurve = new ComplexCurve(new UnityEngine.Keyframe[]
			{
				new UnityEngine.Keyframe(0f, 0f),
				new UnityEngine.Keyframe(1f, 1f),
				new UnityEngine.Keyframe(2f, 1.15f),
				new UnityEngine.Keyframe(3f, 1.3f)
			});

			// Token: 0x04007462 RID: 29794
			private TexturePannerSpeedCurve panner0 = new TexturePannerSpeedCurve(GameCondition_VolcanicAsh.VolcanicAshOverlay.CloudLayer, "_MainTex", GameCondition_VolcanicAsh.VolcanicAshOverlay.speedCurve, new Vector2(1f, -0.2f), 0.007f);

			// Token: 0x04007463 RID: 29795
			private TexturePannerSpeedCurve panner1 = new TexturePannerSpeedCurve(GameCondition_VolcanicAsh.VolcanicAshOverlay.CloudLayer, "_MainTex2", GameCondition_VolcanicAsh.VolcanicAshOverlay.speedCurve, new Vector2(1f, -0.28f), 0.002f);

			// Token: 0x04007464 RID: 29796
			private TexturePannerSpeedCurve panner2 = new TexturePannerSpeedCurve(GameCondition_VolcanicAsh.VolcanicAshOverlay.ParticleLayer, "_MainTex", GameCondition_VolcanicAsh.VolcanicAshOverlay.speedCurve, new Vector2(-0.28f, -1f), 0.0025f);

			// Token: 0x04007465 RID: 29797
			private TexturePannerSpeedCurve panner3 = new TexturePannerSpeedCurve(GameCondition_VolcanicAsh.VolcanicAshOverlay.ParticleLayer, "_MainTex2", GameCondition_VolcanicAsh.VolcanicAshOverlay.speedCurve, new Vector2(-0.1f, -1f), 0.002f);
		}
	}
}
