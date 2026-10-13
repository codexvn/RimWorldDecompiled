using System;
using RimWorld;
using RimWorld.Planet;

namespace Verse.AI
{
	// Token: 0x02000F6B RID: 3947
	public class MentalFitGenerator : IExposable
	{
		// Token: 0x17000F6F RID: 3951
		// (get) Token: 0x0600618C RID: 24972 RVA: 0x001EDF3D File Offset: 0x001EC13D
		public bool CanDoRandomMentalFits
		{
			get
			{
				return this.pawn.RaceProps.Humanlike && !this.pawn.IsWorldPawn();
			}
		}

		// Token: 0x0600618D RID: 24973 RVA: 0x000024DF File Offset: 0x000006DF
		public MentalFitGenerator()
		{
		}

		// Token: 0x0600618E RID: 24974 RVA: 0x001EDF61 File Offset: 0x001EC161
		public MentalFitGenerator(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600618F RID: 24975 RVA: 0x000026EA File Offset: 0x000008EA
		public void Reset()
		{
		}

		// Token: 0x06006190 RID: 24976 RVA: 0x001EDF70 File Offset: 0x001EC170
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksUntilCanDoMentalFit, "ticksUntilCanDoMentalFit", 0, false);
		}

		// Token: 0x06006191 RID: 24977 RVA: 0x001EDF84 File Offset: 0x001EC184
		public void TickInterval(int delta)
		{
			if (this.ticksUntilCanDoMentalFit > 0)
			{
				if (this.pawn.Awake())
				{
					this.ticksUntilCanDoMentalFit -= delta;
				}
				return;
			}
			if (!this.pawn.Awake())
			{
				return;
			}
			if (!this.pawn.IsHashIntervalTick(150, delta))
			{
				return;
			}
			if (!this.CanDoRandomMentalFits)
			{
				return;
			}
			if (this.pawn.InMentalState)
			{
				return;
			}
			foreach (MentalFitDef mentalFitDef in DefDatabase<MentalFitDef>.AllDefsListForReading)
			{
				if (Rand.MTBEventOccurs(mentalFitDef.CalculateMTBDays(this.pawn), 60000f, 150f) && this.pawn.mindState.mentalStateHandler.TryStartMentalState(mentalFitDef.mentalState, null, false, false, false, null, true, false, false))
				{
					break;
				}
			}
		}

		// Token: 0x06006192 RID: 24978 RVA: 0x001EE074 File Offset: 0x001EC274
		public void Notify_RecoveredFromMentalState()
		{
			this.ticksUntilCanDoMentalFit = 600;
		}

		// Token: 0x0400462B RID: 17963
		private Pawn pawn;

		// Token: 0x0400462C RID: 17964
		private int ticksUntilCanDoMentalFit;

		// Token: 0x0400462D RID: 17965
		private const int CheckInterval = 150;

		// Token: 0x0400462E RID: 17966
		private const int MinTicksSinceRecoveryToHaveFit = 600;
	}
}
