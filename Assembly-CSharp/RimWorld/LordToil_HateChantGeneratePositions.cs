using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016FF RID: 5887
	public class LordToil_HateChantGeneratePositions : LordToil
	{
		// Token: 0x1700154B RID: 5451
		// (get) Token: 0x06008AC6 RID: 35526 RVA: 0x0029625B File Offset: 0x0029445B
		protected LordToilData_HateChantGeneratePositions Data
		{
			get
			{
				return (LordToilData_HateChantGeneratePositions)this.data;
			}
		}

		// Token: 0x1700154C RID: 5452
		// (get) Token: 0x06008AC7 RID: 35527 RVA: 0x00296268 File Offset: 0x00294468
		public List<PsychicRitualParticipant> Participants
		{
			get
			{
				return this.Data.chanters;
			}
		}

		// Token: 0x06008AC8 RID: 35528 RVA: 0x00296278 File Offset: 0x00294478
		public LordToil_HateChantGeneratePositions(IEnumerable<PsychicRitualParticipant> chanterPositionPairs, string readySignal)
		{
			PsychicRitualParticipant[] collection = (chanterPositionPairs as PsychicRitualParticipant[]) ?? chanterPositionPairs.ToArray<PsychicRitualParticipant>();
			this.data = new LordToilData_HateChantGeneratePositions();
			this.Data.chanters = new List<PsychicRitualParticipant>(collection);
			this.Data.foundPositions = new HashSet<IntVec3>();
			this.ReadySignal = readySignal;
		}

		// Token: 0x06008AC9 RID: 35529 RVA: 0x002962E0 File Offset: 0x002944E0
		public override void UpdateAllDuties()
		{
			foreach (PsychicRitualParticipant psychicRitualParticipant in this.Data.chanters)
			{
				Pawn pawn;
				IntVec3 intVec;
				psychicRitualParticipant.Deconstruct(out pawn, out intVec);
				Pawn pawn2 = pawn;
				IntVec3 c = intVec;
				if (((pawn2 != null) ? pawn2.mindState : null) != null)
				{
					pawn2.mindState.duty = new PawnDuty(DutyDefOf.Idle, c, -1f);
				}
			}
		}

		// Token: 0x06008ACA RID: 35530 RVA: 0x0029636C File Offset: 0x0029456C
		public override void LordToilTick()
		{
			base.LordToilTick();
			int count = this.Data.foundPositions.Count;
			if (count == this.Data.chanters.Count)
			{
				if (DebugViewSettings.drawHateChanterPositions)
				{
					Material mat = DebugSolidColorMats.MaterialOf(Color.magenta * new Color(1f, 1f, 1f, 0.4f));
					foreach (PsychicRitualParticipant psychicRitualParticipant in this.Participants)
					{
						base.Map.debugDrawer.FlashCell(psychicRitualParticipant.location, mat, null, 100000);
					}
				}
				Find.SignalManager.SendSignal(new Signal(this.ReadySignal, false));
				return;
			}
			if (count > this.Data.chanters.Count)
			{
				Log.Error("Generated more positions than chanters.");
			}
			if (base.Map.IsHashIntervalTick(this.GenerationInterval))
			{
				using (new ProfilerBlock("HateChantersBatch"))
				{
					this.ProcessBatch(count);
				}
				if (this.Data.foundPositions.Count == count)
				{
					Log.Error("No positions found in this batch, something is wrong.");
				}
			}
		}

		// Token: 0x06008ACB RID: 35531 RVA: 0x002964C8 File Offset: 0x002946C8
		private void ProcessBatch(int foundPositionsCount)
		{
			int num = Mathf.Min(5, this.Data.chanters.Count - foundPositionsCount);
			for (int i = 0; i < num; i++)
			{
				PsychicRitualParticipant psychicRitualParticipant = this.Data.chanters[foundPositionsCount + i];
				IntVec3 position = psychicRitualParticipant.pawn.Position;
				IntVec3 intVec = this.FindChantingPosition(position, 4, 5, 2000);
				if (intVec != IntVec3.Invalid)
				{
					psychicRitualParticipant.location = intVec;
					if (!this.Data.foundPositions.Add(intVec))
					{
						Log.Error(string.Format("Tried adding pos to found pos twice: {0}", intVec));
					}
				}
				else
				{
					IntVec3 intVec2 = CellFinder.RandomClosewalkCellNear(position, base.Map, 5, null);
					if (this.Data.foundPositions.Add(intVec2))
					{
						psychicRitualParticipant.location = intVec2;
					}
					else
					{
						psychicRitualParticipant.location = position;
						this.Data.foundPositions.Add(position);
					}
				}
			}
		}

		// Token: 0x06008ACC RID: 35532 RVA: 0x002965BC File Offset: 0x002947BC
		private IntVec3 FindChantingPosition(IntVec3 start, int edificeCheckRadius, int minEdificeCount, int maxIterations)
		{
			LordToil_HateChantGeneratePositions.Frontier.Clear();
			LordToil_HateChantGeneratePositions.Visited.Clear();
			int num = 0;
			LordToil_HateChantGeneratePositions.Frontier.Enqueue(start);
			LordToil_HateChantGeneratePositions.Visited.Add(start);
			while (LordToil_HateChantGeneratePositions.Frontier.Count > 0)
			{
				IntVec3 a = LordToil_HateChantGeneratePositions.Frontier.Dequeue();
				for (int i = 0; i < 4; i++)
				{
					IntVec3 intVec = a + GenAdj.CardinalDirections[i];
					if (this.ValidateChantingPosition(intVec, edificeCheckRadius, minEdificeCount, LordToil_HateChantGeneratePositions.Visited))
					{
						return intVec;
					}
					LordToil_HateChantGeneratePositions.Frontier.Enqueue(intVec);
					LordToil_HateChantGeneratePositions.Visited.Add(intVec);
				}
				num++;
				if (num > maxIterations)
				{
					return IntVec3.Invalid;
				}
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06008ACD RID: 35533 RVA: 0x0029666C File Offset: 0x0029486C
		private bool ValidateChantingPosition(IntVec3 validatedPos, int edificeCheckRadius, int minEdificeCount, HashSet<IntVec3> visited)
		{
			if (!validatedPos.InBounds(base.Map) || visited.Contains(validatedPos) || this.Data.foundPositions.Contains(validatedPos))
			{
				return false;
			}
			if ((!base.Map.TileInfo.AllowRoofedEdgeWalkIn && validatedPos.Roofed(base.Map)) || !validatedPos.Standable(base.Map) || base.Map.avoidGrid[validatedPos] != 0 || validatedPos.GetFirstPawn(base.Map) != null)
			{
				return false;
			}
			District district = validatedPos.GetDistrict(base.Map, RegionType.Set_Passable);
			if (!district.Room.TouchesMapEdge || !district.Room.PsychologicallyOutdoors)
			{
				return false;
			}
			int num = 0;
			using (CellRect.Enumerator enumerator = CellRect.CenteredOn(validatedPos, edificeCheckRadius).ClipInsideMap(base.Map).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetEdifice(base.Map) != null)
					{
						num++;
					}
					if (num == minEdificeCount)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04005A4E RID: 23118
		private static readonly Queue<IntVec3> Frontier = new Queue<IntVec3>();

		// Token: 0x04005A4F RID: 23119
		private static readonly HashSet<IntVec3> Visited = new HashSet<IntVec3>();

		// Token: 0x04005A50 RID: 23120
		private const int PositionsPerBatch = 5;

		// Token: 0x04005A51 RID: 23121
		private readonly int GenerationInterval = 0.2f.SecondsToTicks();

		// Token: 0x04005A52 RID: 23122
		private readonly string ReadySignal;
	}
}
