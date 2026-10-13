using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008F3 RID: 2291
	public class CompAffectsSky : ThingComp
	{
		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x06003B34 RID: 15156 RVA: 0x0013971B File Offset: 0x0013791B
		public CompProperties_AffectsSky Props
		{
			get
			{
				return (CompProperties_AffectsSky)this.props;
			}
		}

		// Token: 0x17000AFD RID: 2813
		// (get) Token: 0x06003B35 RID: 15157 RVA: 0x00139728 File Offset: 0x00137928
		public virtual float LerpFactor
		{
			get
			{
				if (this.HasAutoAnimation)
				{
					int ticksGame = Find.TickManager.TicksGame;
					float num;
					if (ticksGame < this.autoAnimationStartTick + this.fadeInDuration)
					{
						num = (float)(ticksGame - this.autoAnimationStartTick) / (float)this.fadeInDuration;
					}
					else if (ticksGame < this.autoAnimationStartTick + this.fadeInDuration + this.holdDuration)
					{
						num = 1f;
					}
					else
					{
						num = 1f - (float)(ticksGame - this.autoAnimationStartTick - this.fadeInDuration - this.holdDuration) / (float)this.fadeOutDuration;
					}
					return Mathf.Clamp01(num * this.autoAnimationTarget);
				}
				return 0f;
			}
		}

		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x06003B36 RID: 15158 RVA: 0x001397C7 File Offset: 0x001379C7
		public bool HasAutoAnimation
		{
			get
			{
				return Find.TickManager.TicksGame < this.autoAnimationStartTick + this.fadeInDuration + this.holdDuration + this.fadeOutDuration;
			}
		}

		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x06003B37 RID: 15159 RVA: 0x001397F0 File Offset: 0x001379F0
		public virtual SkyTarget SkyTarget
		{
			get
			{
				return new SkyTarget(this.Props.glow, this.Props.skyColors, this.Props.lightsourceShineSize, this.Props.lightsourceShineIntensity);
			}
		}

		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x06003B38 RID: 15160 RVA: 0x00139824 File Offset: 0x00137A24
		public virtual Vector2? OverrideShadowVector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06003B39 RID: 15161 RVA: 0x0013983C File Offset: 0x00137A3C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.autoAnimationStartTick, "autoAnimationStartTick", 0, false);
			Scribe_Values.Look<int>(ref this.fadeInDuration, "fadeInDuration", 0, false);
			Scribe_Values.Look<int>(ref this.holdDuration, "holdDuration", 0, false);
			Scribe_Values.Look<int>(ref this.fadeOutDuration, "fadeOutDuration", 0, false);
			Scribe_Values.Look<float>(ref this.autoAnimationTarget, "autoAnimationTarget", 0f, false);
		}

		// Token: 0x06003B3A RID: 15162 RVA: 0x001398AD File Offset: 0x00137AAD
		public void StartFadeInHoldFadeOut(int fadeInDuration, int holdDuration, int fadeOutDuration, float target = 1f)
		{
			this.autoAnimationStartTick = Find.TickManager.TicksGame;
			this.fadeInDuration = fadeInDuration;
			this.holdDuration = holdDuration;
			this.fadeOutDuration = fadeOutDuration;
			this.autoAnimationTarget = target;
		}

		// Token: 0x04002ADF RID: 10975
		private int autoAnimationStartTick;

		// Token: 0x04002AE0 RID: 10976
		private int fadeInDuration;

		// Token: 0x04002AE1 RID: 10977
		private int holdDuration;

		// Token: 0x04002AE2 RID: 10978
		private int fadeOutDuration;

		// Token: 0x04002AE3 RID: 10979
		private float autoAnimationTarget;
	}
}
