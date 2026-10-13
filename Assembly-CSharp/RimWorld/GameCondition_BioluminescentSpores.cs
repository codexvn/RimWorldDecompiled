using System;
using System.Collections.Generic;
using System.Linq;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D35 RID: 7477
	public class GameCondition_BioluminescentSpores : GameCondition
	{
		// Token: 0x170019E0 RID: 6624
		// (get) Token: 0x0600A77E RID: 42878 RVA: 0x002FFFEE File Offset: 0x002FE1EE
		public Color CurrentColor
		{
			get
			{
				return Color.Lerp(GameCondition_BioluminescentSpores.Colors[this.prevColorIndex], GameCondition_BioluminescentSpores.Colors[this.curColorIndex], this.curColorTransition);
			}
		}

		// Token: 0x170019E1 RID: 6625
		// (get) Token: 0x0600A77F RID: 42879 RVA: 0x0030001B File Offset: 0x002FE21B
		private int TransitionDurationTicks
		{
			get
			{
				return 280;
			}
		}

		// Token: 0x170019E2 RID: 6626
		// (get) Token: 0x0600A780 RID: 42880 RVA: 0x00222D4F File Offset: 0x00220F4F
		public override int TransitionTicks
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x0600A781 RID: 42881 RVA: 0x00300024 File Offset: 0x002FE224
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.curColorIndex, "curColorIndex", 0, false);
			Scribe_Values.Look<int>(ref this.prevColorIndex, "prevColorIndex", 0, false);
			Scribe_Values.Look<float>(ref this.curColorTransition, "curColorTransition", 0f, false);
		}

		// Token: 0x0600A782 RID: 42882 RVA: 0x00300071 File Offset: 0x002FE271
		public override void Init()
		{
			base.Init();
			this.curColorIndex = Rand.Range(0, GameCondition_BioluminescentSpores.Colors.Length);
			this.prevColorIndex = this.curColorIndex;
			this.curColorTransition = 1f;
		}

		// Token: 0x0600A783 RID: 42883 RVA: 0x0001331E File Offset: 0x0001151E
		public override float SkyGazeChanceFactor(Map map)
		{
			return 8f;
		}

		// Token: 0x0600A784 RID: 42884 RVA: 0x00259689 File Offset: 0x00257889
		public override float SkyGazeJoyGainFactor(Map map)
		{
			return 5f;
		}

		// Token: 0x0600A785 RID: 42885 RVA: 0x003000A3 File Offset: 0x002FE2A3
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 1f);
		}

		// Token: 0x0600A786 RID: 42886 RVA: 0x003000B8 File Offset: 0x002FE2B8
		public override SkyTarget? SkyTarget(Map map)
		{
			Color currentColor = this.CurrentColor;
			SkyColorSet colorSet = new SkyColorSet(Color.Lerp(Color.white, currentColor, 0.075f) * this.Brightness(map), new Color(0.92f, 0.92f, 0.92f), Color.Lerp(Color.white, currentColor, 0.025f) * this.Brightness(map), 1f);
			return new SkyTarget?(new SkyTarget(0f, colorSet, 1f, 1f));
		}

		// Token: 0x0600A787 RID: 42887 RVA: 0x002FFE0C File Offset: 0x002FE00C
		private float Brightness(Map map)
		{
			return Mathf.Max(0.73f, GenCelestial.CurCelestialSunGlow(map));
		}

		// Token: 0x0600A788 RID: 42888 RVA: 0x0030013E File Offset: 0x002FE33E
		public override void GameConditionDraw(Map map)
		{
			if (!base.HiddenByOtherCondition(map))
			{
				GameCondition_BioluminescentSpores.glowSporeOverlay.DrawOverlay(map);
			}
		}

		// Token: 0x0600A789 RID: 42889 RVA: 0x00300154 File Offset: 0x002FE354
		public override void GameConditionTick()
		{
			this.curColorTransition += 1f / (float)this.TransitionDurationTicks;
			if (this.curColorTransition >= 1f)
			{
				this.prevColorIndex = this.curColorIndex;
				this.curColorIndex = this.GetNewColorIndex();
				this.curColorTransition = 0f;
			}
			foreach (Map map in base.AffectedMaps)
			{
				if (!base.HiddenByOtherCondition(map))
				{
					GameCondition_BioluminescentSpores.glowSporeOverlay.TickOverlay(map, 1f);
				}
			}
		}

		// Token: 0x0600A78A RID: 42890 RVA: 0x00300204 File Offset: 0x002FE404
		private int GetNewColorIndex()
		{
			return (from x in Enumerable.Range(0, GameCondition_BioluminescentSpores.Colors.Length)
			where x != this.curColorIndex
			select x).RandomElement<int>();
		}

		// Token: 0x0600A78B RID: 42891 RVA: 0x00300229 File Offset: 0x002FE429
		public override List<SkyOverlay> SkyOverlays(Map map)
		{
			return GameCondition_BioluminescentSpores.overlays;
		}

		// Token: 0x040073F8 RID: 29688
		private int curColorIndex = -1;

		// Token: 0x040073F9 RID: 29689
		private int prevColorIndex = -1;

		// Token: 0x040073FA RID: 29690
		private float curColorTransition;

		// Token: 0x040073FB RID: 29691
		private const float SkyColorStrength = 0.075f;

		// Token: 0x040073FC RID: 29692
		private const float OverlayColorStrength = 0.025f;

		// Token: 0x040073FD RID: 29693
		private const float BaseBrightness = 0.73f;

		// Token: 0x040073FE RID: 29694
		private const int TransitionDurationTicks_NotPermanent = 280;

		// Token: 0x040073FF RID: 29695
		private static readonly GameCondition_BioluminescentSpores.GlowSporeOverlay glowSporeOverlay = new GameCondition_BioluminescentSpores.GlowSporeOverlay();

		// Token: 0x04007400 RID: 29696
		private static readonly List<SkyOverlay> overlays = new List<SkyOverlay>
		{
			GameCondition_BioluminescentSpores.glowSporeOverlay
		};

		// Token: 0x04007401 RID: 29697
		private static readonly Color[] Colors = new Color[]
		{
			new Color(0f, 1f, 0f),
			new Color(0.3f, 1f, 0f),
			new Color(0f, 1f, 0.7f),
			new Color(0.3f, 1f, 0.7f),
			new Color(0f, 0.5f, 1f),
			new Color(0f, 0f, 1f),
			new Color(0.87f, 0f, 1f),
			new Color(0.75f, 0f, 1f)
		};

		// Token: 0x02001D36 RID: 7478
		[StaticConstructorOnStartup]
		private class GlowSporeOverlay : SkyOverlay
		{
			// Token: 0x0600A78F RID: 42895 RVA: 0x00300365 File Offset: 0x002FE565
			public override void SetOverlayColor(Color color)
			{
				GameCondition_BioluminescentSpores.GlowSporeOverlay.GlowSporeOverlayWorld.color = color;
			}

			// Token: 0x0600A790 RID: 42896 RVA: 0x00300372 File Offset: 0x002FE572
			public override void DrawOverlay(Map map)
			{
				SkyOverlay.DrawWorldOverlay(map, GameCondition_BioluminescentSpores.GlowSporeOverlay.GlowSporeOverlayWorld, 0);
			}

			// Token: 0x0600A791 RID: 42897 RVA: 0x00300380 File Offset: 0x002FE580
			public override void TickOverlay(Map map, float lerpFactor)
			{
				this.panner0.Tick();
				this.panner1.Tick();
			}

			// Token: 0x04007402 RID: 29698
			private static readonly Material GlowSporeOverlayWorld = MatLoader.LoadMat("Weather/GlowSporeOverlayWorld", -1);

			// Token: 0x04007403 RID: 29699
			private static readonly ComplexCurve speedCurve = new ComplexCurve(new UnityEngine.Keyframe[]
			{
				new UnityEngine.Keyframe(0f, 0f),
				new UnityEngine.Keyframe(1f, 1f),
				new UnityEngine.Keyframe(2f, 1.1f),
				new UnityEngine.Keyframe(3f, 1.2f)
			});

			// Token: 0x04007404 RID: 29700
			private TexturePannerSpeedCurve panner0 = new TexturePannerSpeedCurve(GameCondition_BioluminescentSpores.GlowSporeOverlay.GlowSporeOverlayWorld, "_MainTex", GameCondition_BioluminescentSpores.GlowSporeOverlay.speedCurve, new Vector2(-1f, -0.2f), 0.0001f);

			// Token: 0x04007405 RID: 29701
			private TexturePannerSpeedCurve panner1 = new TexturePannerSpeedCurve(GameCondition_BioluminescentSpores.GlowSporeOverlay.GlowSporeOverlayWorld, "_MainTex2", GameCondition_BioluminescentSpores.GlowSporeOverlay.speedCurve, new Vector2(0.35f, -1f), 5E-05f);
		}
	}
}
