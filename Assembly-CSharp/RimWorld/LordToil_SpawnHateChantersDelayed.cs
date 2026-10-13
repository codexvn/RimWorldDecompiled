using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001727 RID: 5927
	public class LordToil_SpawnHateChantersDelayed : LordToil
	{
		// Token: 0x06008B7B RID: 35707 RVA: 0x00298D55 File Offset: 0x00296F55
		public LordToil_SpawnHateChantersDelayed(List<Pawn> pawns, string readySignal, int batchSize, float spawnInterval)
		{
			this.pawns = pawns;
			this.readySignal = readySignal;
			this.batchSize = batchSize;
			this.spawnInterval = spawnInterval;
		}

		// Token: 0x06008B7C RID: 35708 RVA: 0x00298D7C File Offset: 0x00296F7C
		public override void Init()
		{
			base.Init();
			foreach (Pawn pawn in this.pawns)
			{
				if (((pawn != null) ? pawn.mindState : null) != null)
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.Idle);
					pawn.mindState.Active = false;
				}
			}
			this.spawnRequest = base.Map.deferredSpawner.GetRequestByLord(this.lord);
			if (this.spawnRequest == null)
			{
				this.spawnRequest = new SpawnRequest(this.pawns.Cast<Thing>().ToList<Thing>(), this.batchSize, this.spawnInterval, this.lord);
				base.Map.deferredSpawner.AddRequest(this.spawnRequest, true);
			}
			else
			{
				this.spawnedCount = this.spawnRequest.spawnedThings.Count;
			}
			this.spawnRequested = true;
		}

		// Token: 0x06008B7D RID: 35709 RVA: 0x000026EA File Offset: 0x000008EA
		public override void UpdateAllDuties()
		{
		}

		// Token: 0x06008B7E RID: 35710 RVA: 0x00298E84 File Offset: 0x00297084
		public override void LordToilTick()
		{
			base.LordToilTick();
			if (!this.spawnRequested)
			{
				this.spawnRequest = base.Map.deferredSpawner.GetRequestByLord(this.lord);
				base.Map.deferredSpawner.AddRequest(this.spawnRequest, true);
				this.spawnRequested = true;
			}
			int count = this.spawnRequest.spawnedThings.Count;
			if (count > this.spawnedCount)
			{
				for (int i = this.spawnedCount; i < count; i++)
				{
					Hediff_DeathRefusal firstHediff = (this.spawnRequest.spawnedThings[i] as Pawn).health.hediffSet.GetFirstHediff<Hediff_DeathRefusal>();
					if (firstHediff != null)
					{
						firstHediff.AIEnabled = false;
					}
				}
				this.spawnedCount = count;
			}
			if (this.spawnRequest.done)
			{
				foreach (Thing thing in this.spawnRequest.spawnedThings)
				{
					Pawn pawn = (Pawn)thing;
					if (pawn.mindState != null)
					{
						pawn.mindState.Active = true;
						pawn.mindState.duty = new PawnDuty(DutyDefOf.Idle);
					}
				}
				Find.SignalManager.SendSignal(new Signal(this.readySignal, false));
			}
		}

		// Token: 0x04005AA2 RID: 23202
		private readonly string readySignal;

		// Token: 0x04005AA3 RID: 23203
		private readonly int batchSize;

		// Token: 0x04005AA4 RID: 23204
		private readonly float spawnInterval;

		// Token: 0x04005AA5 RID: 23205
		private readonly List<Pawn> pawns;

		// Token: 0x04005AA6 RID: 23206
		private int spawnedCount;

		// Token: 0x04005AA7 RID: 23207
		private bool spawnRequested;

		// Token: 0x04005AA8 RID: 23208
		private SpawnRequest spawnRequest;
	}
}
