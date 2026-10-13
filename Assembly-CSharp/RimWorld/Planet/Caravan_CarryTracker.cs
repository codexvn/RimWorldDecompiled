using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D91 RID: 15761
	public class Caravan_CarryTracker : IExposable
	{
		// Token: 0x17003563 RID: 13667
		// (get) Token: 0x0601577B RID: 87931 RVA: 0x0066ED00 File Offset: 0x0066CF00
		public List<Pawn> CarriedPawnsListForReading
		{
			get
			{
				return this.carriedPawns;
			}
		}

		// Token: 0x0601577C RID: 87932 RVA: 0x0066ED08 File Offset: 0x0066CF08
		public Caravan_CarryTracker()
		{
		}

		// Token: 0x0601577D RID: 87933 RVA: 0x0066ED1B File Offset: 0x0066CF1B
		public Caravan_CarryTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x0601577E RID: 87934 RVA: 0x0066ED35 File Offset: 0x0066CF35
		public void CarryTrackerTickInterval(int delta)
		{
			this.RecalculateCarriedPawns();
		}

		// Token: 0x0601577F RID: 87935 RVA: 0x0066ED3D File Offset: 0x0066CF3D
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecalculateCarriedPawns();
			}
		}

		// Token: 0x06015780 RID: 87936 RVA: 0x0066ED4D File Offset: 0x0066CF4D
		public bool IsCarried(Pawn p)
		{
			return this.carriedPawns.Contains(p);
		}

		// Token: 0x06015781 RID: 87937 RVA: 0x0066ED5C File Offset: 0x0066CF5C
		private void RecalculateCarriedPawns()
		{
			this.carriedPawns.Clear();
			if (!this.caravan.Spawned)
			{
				return;
			}
			if (this.caravan.pather.MovingNow)
			{
				Caravan_CarryTracker.tmpPawnsWhoCanCarry.Clear();
				this.CalculatePawnsWhoCanCarry(Caravan_CarryTracker.tmpPawnsWhoCanCarry);
				int num = 0;
				while (num < this.caravan.pawns.Count && Caravan_CarryTracker.tmpPawnsWhoCanCarry.Any<Pawn>())
				{
					Pawn pawn = this.caravan.pawns[num];
					if (this.WantsToBeCarried(pawn) && Caravan_CarryTracker.tmpPawnsWhoCanCarry.Any<Pawn>())
					{
						this.carriedPawns.Add(pawn);
						Caravan_CarryTracker.tmpPawnsWhoCanCarry.RemoveLast<Pawn>();
					}
					num++;
				}
				Caravan_CarryTracker.tmpPawnsWhoCanCarry.Clear();
			}
		}

		// Token: 0x06015782 RID: 87938 RVA: 0x0066ED35 File Offset: 0x0066CF35
		public void Notify_CaravanSpawned()
		{
			this.RecalculateCarriedPawns();
		}

		// Token: 0x06015783 RID: 87939 RVA: 0x0066ED35 File Offset: 0x0066CF35
		public void Notify_PawnRemoved()
		{
			this.RecalculateCarriedPawns();
		}

		// Token: 0x06015784 RID: 87940 RVA: 0x0066EE1C File Offset: 0x0066D01C
		private void CalculatePawnsWhoCanCarry(List<Pawn> outPawns)
		{
			outPawns.Clear();
			for (int i = 0; i < this.caravan.pawns.Count; i++)
			{
				Pawn pawn = this.caravan.pawns[i];
				if (pawn.RaceProps.Humanlike && !pawn.Downed && !pawn.InMentalState && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) && !this.WantsToBeCarried(pawn))
				{
					outPawns.Add(pawn);
				}
			}
		}

		// Token: 0x06015785 RID: 87941 RVA: 0x0066EEA0 File Offset: 0x0066D0A0
		private bool WantsToBeCarried(Pawn p)
		{
			return p.health.beCarriedByCaravanIfSick && CaravanCarryUtility.WouldBenefitFromBeingCarried(p);
		}

		// Token: 0x06015786 RID: 87942 RVA: 0x0066EEB8 File Offset: 0x0066D0B8
		public string GetInspectStringLine()
		{
			if (!this.carriedPawns.Any<Pawn>())
			{
				return null;
			}
			Caravan_CarryTracker.tmpPawnLabels.Clear();
			int num = 0;
			for (int i = 0; i < this.carriedPawns.Count; i++)
			{
				Caravan_CarryTracker.tmpPawnLabels.Add(this.carriedPawns[i].LabelShort);
				if (this.caravan.beds.IsInBed(this.carriedPawns[i]))
				{
					num++;
				}
			}
			string str = (Caravan_CarryTracker.tmpPawnLabels.Count > 5) ? (Caravan_CarryTracker.tmpPawnLabels.Take(5).ToCommaList(false, false) + "...") : Caravan_CarryTracker.tmpPawnLabels.ToCommaList(true, false);
			string result = CaravanBedUtility.AppendUsingBedsLabel("BeingCarriedDueToIllness".Translate() + ": " + str.CapitalizeFirst(), num);
			Caravan_CarryTracker.tmpPawnLabels.Clear();
			return result;
		}

		// Token: 0x0400EEF5 RID: 61173
		public Caravan caravan;

		// Token: 0x0400EEF6 RID: 61174
		private List<Pawn> carriedPawns = new List<Pawn>();

		// Token: 0x0400EEF7 RID: 61175
		private static List<Pawn> tmpPawnsWhoCanCarry = new List<Pawn>();

		// Token: 0x0400EEF8 RID: 61176
		private static readonly List<string> tmpPawnLabels = new List<string>();
	}
}
