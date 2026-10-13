using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D8F RID: 15759
	public class Caravan_BedsTracker : IExposable
	{
		// Token: 0x06015769 RID: 87913 RVA: 0x0066E848 File Offset: 0x0066CA48
		public Caravan_BedsTracker()
		{
		}

		// Token: 0x0601576A RID: 87914 RVA: 0x0066E85B File Offset: 0x0066CA5B
		public Caravan_BedsTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x0601576B RID: 87915 RVA: 0x0066E878 File Offset: 0x0066CA78
		public void BedsTrackerTickInterval(int delta)
		{
			this.RecalculateUsedBeds();
			foreach (KeyValuePair<Pawn, Building_Bed> keyValuePair in this.usedBeds)
			{
				PawnUtility.GainComfortFromThingIfPossible(keyValuePair.Key, keyValuePair.Value, delta);
			}
		}

		// Token: 0x0601576C RID: 87916 RVA: 0x0066E8E0 File Offset: 0x0066CAE0
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecalculateUsedBeds();
			}
		}

		// Token: 0x0601576D RID: 87917 RVA: 0x0066E8F0 File Offset: 0x0066CAF0
		private void RecalculateUsedBeds()
		{
			this.usedBeds.Clear();
			if (!this.caravan.Spawned)
			{
				return;
			}
			Caravan_BedsTracker.tmpUsableBeds.Clear();
			this.GetUsableBeds(Caravan_BedsTracker.tmpUsableBeds);
			if (!this.caravan.pather.MovingNow)
			{
				Caravan_BedsTracker.tmpUsableBeds.SortByDescending((Building_Bed x) => x.GetStatValue(StatDefOf.BedRestEffectiveness, true, -1));
				for (int i = 0; i < this.caravan.pawns.Count; i++)
				{
					Pawn pawn = this.caravan.pawns[i];
					if (pawn.needs != null && pawn.needs.rest != null)
					{
						Building_Bed andRemoveFirstAvailableBedFor = this.GetAndRemoveFirstAvailableBedFor(pawn, Caravan_BedsTracker.tmpUsableBeds);
						if (andRemoveFirstAvailableBedFor != null)
						{
							this.usedBeds.Add(pawn, andRemoveFirstAvailableBedFor);
						}
					}
				}
			}
			else
			{
				Caravan_BedsTracker.tmpUsableBeds.SortByDescending((Building_Bed x) => x.GetStatValue(StatDefOf.ImmunityGainSpeedFactor, true, -1));
				for (int j = 0; j < this.caravan.pawns.Count; j++)
				{
					Pawn pawn2 = this.caravan.pawns[j];
					if (pawn2.needs != null && pawn2.needs.rest != null && CaravanBedUtility.WouldBenefitFromRestingInBed(pawn2) && (!this.caravan.pather.MovingNow || pawn2.CarriedByCaravan()))
					{
						Building_Bed andRemoveFirstAvailableBedFor2 = this.GetAndRemoveFirstAvailableBedFor(pawn2, Caravan_BedsTracker.tmpUsableBeds);
						if (andRemoveFirstAvailableBedFor2 != null)
						{
							this.usedBeds.Add(pawn2, andRemoveFirstAvailableBedFor2);
						}
					}
				}
			}
			Caravan_BedsTracker.tmpUsableBeds.Clear();
		}

		// Token: 0x0601576E RID: 87918 RVA: 0x0066EA8E File Offset: 0x0066CC8E
		public void Notify_CaravanSpawned()
		{
			this.RecalculateUsedBeds();
		}

		// Token: 0x0601576F RID: 87919 RVA: 0x0066EA8E File Offset: 0x0066CC8E
		public void Notify_PawnRemoved()
		{
			this.RecalculateUsedBeds();
		}

		// Token: 0x06015770 RID: 87920 RVA: 0x0066EA98 File Offset: 0x0066CC98
		public Building_Bed GetBedUsedBy(Pawn p)
		{
			Building_Bed building_Bed;
			if (this.usedBeds.TryGetValue(p, out building_Bed) && !building_Bed.DestroyedOrNull())
			{
				return building_Bed;
			}
			return null;
		}

		// Token: 0x06015771 RID: 87921 RVA: 0x0066EAC0 File Offset: 0x0066CCC0
		public bool IsInBed(Pawn p)
		{
			return this.GetBedUsedBy(p) != null;
		}

		// Token: 0x06015772 RID: 87922 RVA: 0x0066EACC File Offset: 0x0066CCCC
		public int GetUsedBedCount()
		{
			return this.usedBeds.Count;
		}

		// Token: 0x06015773 RID: 87923 RVA: 0x0066EADC File Offset: 0x0066CCDC
		private void GetUsableBeds(List<Building_Bed> outBeds)
		{
			outBeds.Clear();
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(this.caravan);
			for (int i = 0; i < list.Count; i++)
			{
				Building_Bed building_Bed = list[i].GetInnerIfMinified() as Building_Bed;
				if (building_Bed != null && building_Bed.def.building.bed_caravansCanUse)
				{
					for (int j = 0; j < list[i].stackCount; j++)
					{
						for (int k = 0; k < building_Bed.SleepingSlotsCount; k++)
						{
							outBeds.Add(building_Bed);
						}
					}
				}
			}
		}

		// Token: 0x06015774 RID: 87924 RVA: 0x0066EB68 File Offset: 0x0066CD68
		private Building_Bed GetAndRemoveFirstAvailableBedFor(Pawn p, List<Building_Bed> beds)
		{
			for (int i = 0; i < beds.Count; i++)
			{
				if (RestUtility.CanUseBedEver(p, beds[i].def))
				{
					Building_Bed result = beds[i];
					beds.RemoveAt(i);
					return result;
				}
			}
			return null;
		}

		// Token: 0x06015775 RID: 87925 RVA: 0x0066EBAC File Offset: 0x0066CDAC
		public string GetInBedForMedicalReasonsInspectStringLine()
		{
			if (this.usedBeds.Count == 0)
			{
				return null;
			}
			Caravan_BedsTracker.tmpPawnLabels.Clear();
			foreach (KeyValuePair<Pawn, Building_Bed> keyValuePair in this.usedBeds)
			{
				if (!this.caravan.carryTracker.IsCarried(keyValuePair.Key) && CaravanBedUtility.WouldBenefitFromRestingInBed(keyValuePair.Key))
				{
					Caravan_BedsTracker.tmpPawnLabels.Add(keyValuePair.Key.LabelShort);
				}
			}
			if (!Caravan_BedsTracker.tmpPawnLabels.Any<string>())
			{
				return null;
			}
			string t = (Caravan_BedsTracker.tmpPawnLabels.Count > 5) ? (Caravan_BedsTracker.tmpPawnLabels.Take(5).ToCommaList(false, false) + "...") : Caravan_BedsTracker.tmpPawnLabels.ToCommaList(true, false);
			Caravan_BedsTracker.tmpPawnLabels.Clear();
			return "UsingBedrollsDueToIllness".Translate() + ": " + t;
		}

		// Token: 0x0400EEEE RID: 61166
		public Caravan caravan;

		// Token: 0x0400EEEF RID: 61167
		private Dictionary<Pawn, Building_Bed> usedBeds = new Dictionary<Pawn, Building_Bed>();

		// Token: 0x0400EEF0 RID: 61168
		private static List<Building_Bed> tmpUsableBeds = new List<Building_Bed>();

		// Token: 0x0400EEF1 RID: 61169
		private static List<string> tmpPawnLabels = new List<string>();
	}
}
