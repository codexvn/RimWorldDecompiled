using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000250 RID: 592
	[StaticConstructorOnStartup]
	public class LifeStageAge
	{
		// Token: 0x06000F0D RID: 3853 RVA: 0x000466EC File Offset: 0x000448EC
		public Texture2D GetIcon(Pawn forPawn)
		{
			if (this.def.iconTex != null)
			{
				return this.def.iconTex;
			}
			int count = forPawn.RaceProps.lifeStageAges.Count;
			int num = forPawn.RaceProps.lifeStageAges.IndexOf(this);
			if (num == count - 1)
			{
				return LifeStageAge.AdultIcon;
			}
			if (num == 0)
			{
				return LifeStageAge.VeryYoungIcon;
			}
			return LifeStageAge.YoungIcon;
		}

		// Token: 0x04000A7A RID: 2682
		public LifeStageDef def;

		// Token: 0x04000A7B RID: 2683
		public float minAge;

		// Token: 0x04000A7C RID: 2684
		public SoundDef soundCall;

		// Token: 0x04000A7D RID: 2685
		public SoundDef soundAngry;

		// Token: 0x04000A7E RID: 2686
		public SoundDef soundWounded;

		// Token: 0x04000A7F RID: 2687
		public SoundDef soundDeath;

		// Token: 0x04000A80 RID: 2688
		public SoundDef soundAmbience;

		// Token: 0x04000A81 RID: 2689
		public SoundDef soundAttack;

		// Token: 0x04000A82 RID: 2690
		private static readonly Texture2D VeryYoungIcon = ContentFinder<Texture2D>.Get("UI/Icons/LifeStage/VeryYoung", true);

		// Token: 0x04000A83 RID: 2691
		private static readonly Texture2D YoungIcon = ContentFinder<Texture2D>.Get("UI/Icons/LifeStage/Young", true);

		// Token: 0x04000A84 RID: 2692
		private static readonly Texture2D AdultIcon = ContentFinder<Texture2D>.Get("UI/Icons/LifeStage/Adult", true);
	}
}
