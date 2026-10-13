using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D8E RID: 15758
	public class Caravan_BabyTracker
	{
		// Token: 0x17003562 RID: 13666
		// (get) Token: 0x06015763 RID: 87907 RVA: 0x0066E621 File Offset: 0x0066C821
		private float IdeoExposurePerCaravanIdeoMember
		{
			get
			{
				return 0.003f / (float)this.ideosForExposure.Count;
			}
		}

		// Token: 0x06015764 RID: 87908 RVA: 0x0066E635 File Offset: 0x0066C835
		public Caravan_BabyTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x06015765 RID: 87909 RVA: 0x0066E65C File Offset: 0x0066C85C
		public void TickInterval(int delta)
		{
			if (!ModsConfig.IdeologyActive || !this.caravan.IsHashIntervalTick(30, delta))
			{
				return;
			}
			bool flag = false;
			foreach (Pawn pawn in this.babies)
			{
				if (pawn.Dead || !pawn.DevelopmentalStage.Baby())
				{
					flag = true;
				}
				else if (pawn.ideo != null)
				{
					float ideoExposurePerCaravanIdeoMember = this.IdeoExposurePerCaravanIdeoMember;
					foreach (Ideo ideo in this.ideosForExposure)
					{
						pawn.ideo.IncreaseIdeoExposureIfBaby(ideo, ideoExposurePerCaravanIdeoMember);
					}
				}
			}
			if (flag)
			{
				this.Recache();
			}
		}

		// Token: 0x06015766 RID: 87910 RVA: 0x0066E740 File Offset: 0x0066C940
		public void Recache()
		{
			this.RebuildBabyList();
			this.RebuildPawnIdeos();
		}

		// Token: 0x06015767 RID: 87911 RVA: 0x0066E750 File Offset: 0x0066C950
		private void RebuildBabyList()
		{
			this.babies.Clear();
			foreach (Pawn pawn in this.caravan.PawnsListForReading)
			{
				if (pawn.DevelopmentalStage.Baby())
				{
					this.babies.Add(pawn);
				}
			}
		}

		// Token: 0x06015768 RID: 87912 RVA: 0x0066E7C8 File Offset: 0x0066C9C8
		private void RebuildPawnIdeos()
		{
			if (!ModsConfig.IdeologyActive)
			{
				return;
			}
			this.ideosForExposure.Clear();
			foreach (Pawn pawn in this.caravan.PawnsListForReading)
			{
				if (pawn.Ideo != null)
				{
					this.ideosForExposure.Add(pawn.Ideo);
				}
			}
		}

		// Token: 0x0400EEEA RID: 61162
		private readonly Caravan caravan;

		// Token: 0x0400EEEB RID: 61163
		private readonly List<Pawn> babies = new List<Pawn>();

		// Token: 0x0400EEEC RID: 61164
		private readonly List<Ideo> ideosForExposure = new List<Ideo>();

		// Token: 0x0400EEED RID: 61165
		private const int IdeoExposureIntervalTicks = 30;
	}
}
