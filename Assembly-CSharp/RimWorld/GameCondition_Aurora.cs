using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D34 RID: 7476
	public class GameCondition_Aurora : GameCondition
	{
		// Token: 0x170019DB RID: 6619
		// (get) Token: 0x0600A76D RID: 42861 RVA: 0x002FFBCB File Offset: 0x002FDDCB
		public Color CurrentColor
		{
			get
			{
				return Color.Lerp(GameCondition_Aurora.Colors[this.prevColorIndex], GameCondition_Aurora.Colors[this.curColorIndex], this.curColorTransition);
			}
		}

		// Token: 0x170019DC RID: 6620
		// (get) Token: 0x0600A76E RID: 42862 RVA: 0x002FFBF8 File Offset: 0x002FDDF8
		private int TransitionDurationTicks
		{
			get
			{
				if (!base.Permanent)
				{
					return 280;
				}
				return 3750;
			}
		}

		// Token: 0x170019DD RID: 6621
		// (get) Token: 0x0600A76F RID: 42863 RVA: 0x002FFC10 File Offset: 0x002FDE10
		private bool BrightInAllMaps
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (GenCelestial.CurCelestialSunGlow(maps[i]) <= 0.5f)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x170019DE RID: 6622
		// (get) Token: 0x0600A770 RID: 42864 RVA: 0x002FFC4C File Offset: 0x002FDE4C
		public bool AlwaysDarkInAllMaps
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (!maps[i].GameConditionManager.IsAlwaysDarkOutside)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x170019DF RID: 6623
		// (get) Token: 0x0600A771 RID: 42865 RVA: 0x00222D4F File Offset: 0x00220F4F
		public override int TransitionTicks
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x0600A772 RID: 42866 RVA: 0x002FFC88 File Offset: 0x002FDE88
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.curColorIndex, "curColorIndex", 0, false);
			Scribe_Values.Look<int>(ref this.prevColorIndex, "prevColorIndex", 0, false);
			Scribe_Values.Look<float>(ref this.curColorTransition, "curColorTransition", 0f, false);
		}

		// Token: 0x0600A773 RID: 42867 RVA: 0x002FFCD5 File Offset: 0x002FDED5
		public override void Init()
		{
			base.Init();
			this.curColorIndex = Rand.Range(0, GameCondition_Aurora.Colors.Length);
			this.prevColorIndex = this.curColorIndex;
			this.curColorTransition = 1f;
		}

		// Token: 0x0600A774 RID: 42868 RVA: 0x002FFD07 File Offset: 0x002FDF07
		public override float SkyGazeChanceFactor(Map map)
		{
			if (map.GameConditionManager.IsAlwaysDarkOutside)
			{
				return 0f;
			}
			return 8f;
		}

		// Token: 0x0600A775 RID: 42869 RVA: 0x002FFD21 File Offset: 0x002FDF21
		public override float SkyGazeJoyGainFactor(Map map)
		{
			if (map.GameConditionManager.IsAlwaysDarkOutside)
			{
				return 0f;
			}
			return 5f;
		}

		// Token: 0x0600A776 RID: 42870 RVA: 0x002FFD3B File Offset: 0x002FDF3B
		public override float SkyTargetLerpFactor(Map map)
		{
			if (map.GameConditionManager.IsAlwaysDarkOutside)
			{
				return 0f;
			}
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 1f);
		}

		// Token: 0x0600A777 RID: 42871 RVA: 0x002FFD64 File Offset: 0x002FDF64
		public override SkyTarget? SkyTarget(Map map)
		{
			if (map.GameConditionManager.IsAlwaysDarkOutside)
			{
				return null;
			}
			Color currentColor = this.CurrentColor;
			SkyColorSet colorSet = new SkyColorSet(Color.Lerp(Color.white, currentColor, 0.075f) * this.Brightness(map), new Color(0.92f, 0.92f, 0.92f), Color.Lerp(Color.white, currentColor, 0.025f) * this.Brightness(map), 1f);
			return new SkyTarget?(new SkyTarget(Mathf.Max(GenCelestial.CurCelestialSunGlow(map), 0.25f), colorSet, 1f, 1f));
		}

		// Token: 0x0600A778 RID: 42872 RVA: 0x002FFE0C File Offset: 0x002FE00C
		private float Brightness(Map map)
		{
			return Mathf.Max(0.73f, GenCelestial.CurCelestialSunGlow(map));
		}

		// Token: 0x0600A779 RID: 42873 RVA: 0x002FFE20 File Offset: 0x002FE020
		public override void GameConditionTick()
		{
			this.curColorTransition += 1f / (float)this.TransitionDurationTicks;
			if (this.curColorTransition >= 1f)
			{
				this.prevColorIndex = this.curColorIndex;
				this.curColorIndex = this.GetNewColorIndex();
				this.curColorTransition = 0f;
			}
			if (!base.Permanent && base.TicksLeft > this.TransitionTicks)
			{
				if (this.BrightInAllMaps)
				{
					base.TicksLeft = this.TransitionTicks;
				}
				if (this.AlwaysDarkInAllMaps)
				{
					base.TicksLeft = this.TransitionTicks;
				}
			}
		}

		// Token: 0x0600A77A RID: 42874 RVA: 0x002FFEB5 File Offset: 0x002FE0B5
		private int GetNewColorIndex()
		{
			return (from x in Enumerable.Range(0, GameCondition_Aurora.Colors.Length)
			where x != this.curColorIndex
			select x).RandomElement<int>();
		}

		// Token: 0x040073ED RID: 29677
		private int curColorIndex = -1;

		// Token: 0x040073EE RID: 29678
		private int prevColorIndex = -1;

		// Token: 0x040073EF RID: 29679
		private float curColorTransition;

		// Token: 0x040073F0 RID: 29680
		public const float MaxSunGlow = 0.5f;

		// Token: 0x040073F1 RID: 29681
		private const float Glow = 0.25f;

		// Token: 0x040073F2 RID: 29682
		private const float SkyColorStrength = 0.075f;

		// Token: 0x040073F3 RID: 29683
		private const float OverlayColorStrength = 0.025f;

		// Token: 0x040073F4 RID: 29684
		private const float BaseBrightness = 0.73f;

		// Token: 0x040073F5 RID: 29685
		private const int TransitionDurationTicks_NotPermanent = 280;

		// Token: 0x040073F6 RID: 29686
		private const int TransitionDurationTicks_Permanent = 3750;

		// Token: 0x040073F7 RID: 29687
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
	}
}
