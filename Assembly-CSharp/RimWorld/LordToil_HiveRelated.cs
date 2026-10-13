using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001701 RID: 5889
	public abstract class LordToil_HiveRelated : LordToil
	{
		// Token: 0x1700154D RID: 5453
		// (get) Token: 0x06008AD1 RID: 35537 RVA: 0x002967D3 File Offset: 0x002949D3
		private LordToil_HiveRelatedData Data
		{
			get
			{
				return (LordToil_HiveRelatedData)this.data;
			}
		}

		// Token: 0x06008AD2 RID: 35538 RVA: 0x002967E0 File Offset: 0x002949E0
		public LordToil_HiveRelated()
		{
			this.data = new LordToil_HiveRelatedData();
		}

		// Token: 0x06008AD3 RID: 35539 RVA: 0x002967F3 File Offset: 0x002949F3
		protected void FilterOutUnspawnedHives()
		{
			this.Data.assignedHives.RemoveAll((KeyValuePair<Pawn, Hive> x) => x.Value == null || !x.Value.Spawned);
		}

		// Token: 0x06008AD4 RID: 35540 RVA: 0x00296828 File Offset: 0x00294A28
		protected Hive GetHiveFor(Pawn pawn)
		{
			Hive hive;
			if (this.Data.assignedHives.TryGetValue(pawn, out hive))
			{
				return hive;
			}
			hive = this.FindClosestHive(pawn);
			if (hive != null)
			{
				this.Data.assignedHives.Add(pawn, hive);
			}
			return hive;
		}

		// Token: 0x06008AD5 RID: 35541 RVA: 0x0029686C File Offset: 0x00294A6C
		private Hive FindClosestHive(Pawn pawn)
		{
			if (!pawn.Spawned)
			{
				return null;
			}
			return (Hive)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.Hive), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 30f, (Thing x) => x.Faction == pawn.Faction, null, 0, 30, false, RegionType.Set_Passable, false, false);
		}

		// Token: 0x06008AD6 RID: 35542 RVA: 0x002968EC File Offset: 0x00294AEC
		public override void Notify_PawnAcquiredTarget(Pawn detector, Thing newTarg)
		{
			CompCanBeDormant compCanBeDormant = detector.TryGetComp<CompCanBeDormant>();
			if (compCanBeDormant != null)
			{
				compCanBeDormant.WakeUp();
			}
		}
	}
}
