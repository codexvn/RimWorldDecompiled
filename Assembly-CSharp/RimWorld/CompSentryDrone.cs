using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002E00 RID: 11776
	public class CompSentryDrone : ThingComp
	{
		// Token: 0x17002A5D RID: 10845
		// (get) Token: 0x0601092B RID: 67883 RVA: 0x0048AE48 File Offset: 0x00489048
		private Pawn Drone
		{
			get
			{
				return this.parent as Pawn;
			}
		}

		// Token: 0x17002A5E RID: 10846
		// (get) Token: 0x0601092C RID: 67884 RVA: 0x004C6E0C File Offset: 0x004C500C
		// (set) Token: 0x0601092D RID: 67885 RVA: 0x004C6E14 File Offset: 0x004C5014
		public CompSentryDrone.SentryDroneMode Mode
		{
			get
			{
				return this.mode;
			}
			set
			{
				this.mode = value;
			}
		}

		// Token: 0x0601092E RID: 67886 RVA: 0x004C6E1D File Offset: 0x004C501D
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (respawningAfterLoad)
			{
				return;
			}
			this.spawnPos = this.parent.Position;
		}

		// Token: 0x0601092F RID: 67887 RVA: 0x004C6E34 File Offset: 0x004C5034
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (!this.Drone.Spawned || this.Drone.Dead)
			{
				return;
			}
			if (this.Mode == CompSentryDrone.SentryDroneMode.Attack)
			{
				return;
			}
			this.Mode = CompSentryDrone.SentryDroneMode.Attack;
			this.Drone.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			this.lastSensedHostileTick = (long)Find.TickManager.TicksGame;
		}

		// Token: 0x06010930 RID: 67888 RVA: 0x004C6E94 File Offset: 0x004C5094
		public override void CompTickInterval(int delta)
		{
			if (!this.parent.Spawned || !this.parent.IsHashIntervalTick(60, delta))
			{
				return;
			}
			if (this.Mode == CompSentryDrone.SentryDroneMode.Patrol)
			{
				Map map = this.parent.Map;
				IntVec3 position = this.parent.Position;
				using (IEnumerator<Pawn> enumerator = map.mapPawns.AllPawnsSpawned.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Pawn pawn = enumerator.Current;
						if (pawn.Position.DistanceTo(position) <= 20f && pawn.HostileTo(this.parent) && GenSight.LineOfSight(position, pawn.Position, map))
						{
							this.Mode = CompSentryDrone.SentryDroneMode.Attack;
							this.Drone.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
							break;
						}
					}
					return;
				}
			}
			if (this.Drone.mindState.anyCloseHostilesRecently)
			{
				this.lastSensedHostileTick = (long)Find.TickManager.TicksGame;
				return;
			}
			if (Math.Abs(this.lastSensedHostileTick - (long)Find.TickManager.TicksGame) < 3600L)
			{
				return;
			}
			this.Mode = CompSentryDrone.SentryDroneMode.Patrol;
			this.Drone.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
		}

		// Token: 0x06010931 RID: 67889 RVA: 0x004C6FCC File Offset: 0x004C51CC
		public IntVec3 GetNextPatrolDest()
		{
			if (!this.parent.Spawned)
			{
				return IntVec3.Invalid;
			}
			IntVec3 actualLastPatrolDest = this.lastPatrolDest;
			this.lastPatrolDest = this.parent.Position;
			Room room = this.parent.GetRoom(RegionType.Set_All);
			if (room == null || room.PsychologicallyOutdoors)
			{
				if (this.spawnPos.IsValid)
				{
					Room room4 = this.spawnPos.GetRoom(this.parent.Map);
					if (room4 != null && !room4.PsychologicallyOutdoors)
					{
						return room4.Cells.RandomElement<IntVec3>();
					}
				}
				return IntVec3.Invalid;
			}
			List<Room> adjacentRooms = CompSentryDrone.GetAdjacentRooms(room);
			if (adjacentRooms.Count > 0)
			{
				if (adjacentRooms.Count == 1)
				{
					return adjacentRooms[0].Cells.RandomElement<IntVec3>();
				}
				if (Rand.Chance(0.1f))
				{
					Room room2 = adjacentRooms.FirstOrDefault((Room r) => r.ContainsCell(actualLastPatrolDest));
					if (room2 != null)
					{
						return room2.Cells.RandomElement<IntVec3>();
					}
				}
				List<Room> list = (from r in adjacentRooms
				where !r.ContainsCell(actualLastPatrolDest)
				select r).ToList<Room>();
				if (list.Count == 0)
				{
					Log.Error("CompSentryDrone found multiple adjacent rooms but none of them were not the last room we were in?");
					return IntVec3.Invalid;
				}
				return list.RandomElement<Room>().Cells.RandomElement<IntVec3>();
			}
			else
			{
				List<IntVec3> list2 = GenRadial.RadialCellsAround(this.parent.Position, 14f, true).Where(delegate(IntVec3 c)
				{
					Room room3 = c.GetRoom(this.parent.Map);
					return room3 != null && !room3.PsychologicallyOutdoors && room3 != room;
				}).Take(30).ToList<IntVec3>();
				if (list2.Count > 0)
				{
					return list2.RandomElement<IntVec3>();
				}
				return room.Cells.RandomElement<IntVec3>();
			}
		}

		// Token: 0x06010932 RID: 67890 RVA: 0x004C717C File Offset: 0x004C537C
		private static List<Room> GetAdjacentRooms(Room room)
		{
			List<Room> list = new List<Room>();
			foreach (Region region in room.Regions)
			{
				foreach (RegionLink regionLink in region.links)
				{
					Region otherRegion = regionLink.GetOtherRegion(region);
					if (otherRegion.IsDoorway)
					{
						foreach (RegionLink regionLink2 in otherRegion.links)
						{
							Region otherRegion2 = regionLink2.GetOtherRegion(otherRegion);
							if (!otherRegion2.Room.PsychologicallyOutdoors && !list.Contains(otherRegion2.Room) && otherRegion2.Room != room)
							{
								list.Add(otherRegion2.Room);
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06010933 RID: 67891 RVA: 0x004C72A0 File Offset: 0x004C54A0
		public override bool WantHoldWeapon(Pawn pawn)
		{
			return this.mode == CompSentryDrone.SentryDroneMode.Attack;
		}

		// Token: 0x06010934 RID: 67892 RVA: 0x004C72AC File Offset: 0x004C54AC
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<CompSentryDrone.SentryDroneMode>(ref this.mode, "mode", CompSentryDrone.SentryDroneMode.Patrol, false);
			Scribe_Values.Look<IntVec3>(ref this.lastPatrolDest, "lastPatrolDest", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.spawnPos, "spawnPos", default(IntVec3), false);
			Scribe_Values.Look<long>(ref this.lastSensedHostileTick, "lastSensedHostileTick", 0L, false);
		}

		// Token: 0x0400ACBA RID: 44218
		private const int TicksUntilLeaveCombatState = 3600;

		// Token: 0x0400ACBB RID: 44219
		private const float DoubleBackChance = 0.1f;

		// Token: 0x0400ACBC RID: 44220
		private CompSentryDrone.SentryDroneMode mode;

		// Token: 0x0400ACBD RID: 44221
		private IntVec3 lastPatrolDest;

		// Token: 0x0400ACBE RID: 44222
		private IntVec3 spawnPos;

		// Token: 0x0400ACBF RID: 44223
		private long lastSensedHostileTick;

		// Token: 0x02002E01 RID: 11777
		public enum SentryDroneMode
		{
			// Token: 0x0400ACC1 RID: 44225
			Patrol,
			// Token: 0x0400ACC2 RID: 44226
			Attack
		}
	}
}
