using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x020003B5 RID: 949
	public class AnimalPenBalanceCalculator
	{
		// Token: 0x06001845 RID: 6213 RVA: 0x0007B2BE File Offset: 0x000794BE
		public AnimalPenBalanceCalculator(Map map, bool considerInProgressMovement)
		{
			this.map = map;
			this.considerInProgressMovement = considerInProgressMovement;
		}

		// Token: 0x06001846 RID: 6214 RVA: 0x0007B2E6 File Offset: 0x000794E6
		public void MarkDirty()
		{
			this.dirty = true;
		}

		// Token: 0x06001847 RID: 6215 RVA: 0x0007B2F0 File Offset: 0x000794F0
		public bool IsBetterPen(CompAnimalPenMarker markerA, CompAnimalPenMarker markerB, bool leavingMarkerB, Pawn animal)
		{
			this.RecalculateIfDirty();
			District district = markerA.parent.GetDistrict(RegionType.Set_Passable);
			District district2 = markerB.parent.GetDistrict(RegionType.Set_Passable);
			if (district == district2)
			{
				return false;
			}
			float bodySize = animal.BodySize;
			float num = this.TotalBodySizeIn(district) + bodySize;
			float num2 = this.TotalBodySizeIn(district2) + (leavingMarkerB ? (-bodySize) : bodySize);
			float num3 = num / (float)district.CellCount;
			float num4 = num2 / (float)district2.CellCount;
			return num3 * 1.2f < num4;
		}

		// Token: 0x06001848 RID: 6216 RVA: 0x0007B364 File Offset: 0x00079564
		public float TotalBodySizeIn(District district)
		{
			this.RecalculateIfDirty();
			float num = 0f;
			foreach (AnimalPenBalanceCalculator.AnimalMembershipInfo animalMembershipInfo in this.membership)
			{
				if (animalMembershipInfo.pen == district)
				{
					num += animalMembershipInfo.animal.BodySize;
				}
			}
			return num;
		}

		// Token: 0x06001849 RID: 6217 RVA: 0x0007B3D4 File Offset: 0x000795D4
		private void RecalculateIfDirty()
		{
			if (!this.dirty)
			{
				return;
			}
			this.dirty = false;
			this.membership.Clear();
			foreach (Pawn pawn in this.map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer))
			{
				if (AnimalPenUtility.NeedsToBeManagedByRope(pawn))
				{
					District district = null;
					if (this.considerInProgressMovement && pawn.roping.IsRopedByPawn && pawn.roping.RopedByPawn.jobs.curDriver is JobDriver_RopeToPen)
					{
						Thing thing = pawn.roping.RopedByPawn.CurJob.GetTarget(TargetIndex.C).Thing;
						District district2 = (thing != null) ? thing.GetDistrict(RegionType.Set_Passable) : null;
						if (district2 != null && !district2.TouchesMapEdge)
						{
							district = district2;
						}
					}
					if (district == null)
					{
						CompAnimalPenMarker currentPenOf = AnimalPenUtility.GetCurrentPenOf(pawn, false);
						district = ((currentPenOf != null) ? currentPenOf.parent.GetDistrict(RegionType.Set_Passable) : null);
					}
					this.membership.Add(new AnimalPenBalanceCalculator.AnimalMembershipInfo
					{
						animal = pawn,
						pen = district
					});
				}
			}
		}

		// Token: 0x040015A4 RID: 5540
		private const float DensityTolerance = 0.2f;

		// Token: 0x040015A5 RID: 5541
		private readonly Map map;

		// Token: 0x040015A6 RID: 5542
		private bool considerInProgressMovement;

		// Token: 0x040015A7 RID: 5543
		private readonly List<AnimalPenBalanceCalculator.AnimalMembershipInfo> membership = new List<AnimalPenBalanceCalculator.AnimalMembershipInfo>();

		// Token: 0x040015A8 RID: 5544
		private bool dirty = true;

		// Token: 0x020003B6 RID: 950
		private struct AnimalMembershipInfo
		{
			// Token: 0x040015A9 RID: 5545
			public Pawn animal;

			// Token: 0x040015AA RID: 5546
			public District pen;
		}
	}
}
