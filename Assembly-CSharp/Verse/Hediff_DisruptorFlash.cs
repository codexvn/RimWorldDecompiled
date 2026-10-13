using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000744 RID: 1860
	public class Hediff_DisruptorFlash : HediffWithComps
	{
		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x06002D69 RID: 11625 RVA: 0x000EA2E8 File Offset: 0x000E84E8
		public override HediffStage CurStage
		{
			get
			{
				return this.stage;
			}
		}

		// Token: 0x06002D6A RID: 11626 RVA: 0x000EA2F0 File Offset: 0x000E84F0
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			this.SetupStage();
		}

		// Token: 0x06002D6B RID: 11627 RVA: 0x000EA2FF File Offset: 0x000E84FF
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.SetupStage();
			}
		}

		// Token: 0x06002D6C RID: 11628 RVA: 0x000EA318 File Offset: 0x000E8518
		public void SetupStage()
		{
			this.stage = new HediffStage
			{
				capMods = new List<PawnCapacityModifier>
				{
					new PawnCapacityModifier
					{
						capacity = PawnCapacityDefOf.Consciousness,
						postFactor = 1f - this.pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1) * 0.1f
					},
					new PawnCapacityModifier
					{
						capacity = PawnCapacityDefOf.Moving,
						postFactor = 1f - this.pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1) * 0.2f
					}
				}
			};
		}

		// Token: 0x04002401 RID: 9217
		private HediffStage stage;
	}
}
