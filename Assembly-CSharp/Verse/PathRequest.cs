using System;
using RimWorld;
using Unity.Collections;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x020005CA RID: 1482
	public class PathRequest : IDisposable
	{
		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x0600260B RID: 9739 RVA: 0x000C8C90 File Offset: 0x000C6E90
		public IntVec3 Start
		{
			get
			{
				return this.start;
			}
		}

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x0600260C RID: 9740 RVA: 0x000C8C98 File Offset: 0x000C6E98
		public LocalTargetInfo Target
		{
			get
			{
				return this.dest;
			}
		}

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x0600260D RID: 9741 RVA: 0x000C8CA0 File Offset: 0x000C6EA0
		public IntVec3 ExactDestination
		{
			get
			{
				IntVec3? intVec = this.exactDestination;
				if (intVec == null)
				{
					return this.dest.Cell;
				}
				return intVec.GetValueOrDefault();
			}
		}

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x0600260E RID: 9742 RVA: 0x000C8CD0 File Offset: 0x000C6ED0
		public TraverseParms TraverseParms
		{
			get
			{
				return this.traverseParms;
			}
		}

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x0600260F RID: 9743 RVA: 0x000C8CD8 File Offset: 0x000C6ED8
		public PathFinderCostTuning Tuning
		{
			get
			{
				return this.tuning;
			}
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06002610 RID: 9744 RVA: 0x000C8CE0 File Offset: 0x000C6EE0
		public PathEndMode EndMode
		{
			get
			{
				return this.endMode;
			}
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06002611 RID: 9745 RVA: 0x000C8CE8 File Offset: 0x000C6EE8
		public int TickStart
		{
			get
			{
				return this.tickStart;
			}
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06002612 RID: 9746 RVA: 0x000C8CF0 File Offset: 0x000C6EF0
		public bool ResultIsReady
		{
			get
			{
				return this.found != null;
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06002613 RID: 9747 RVA: 0x000C8CFD File Offset: 0x000C6EFD
		public bool Cancelled
		{
			get
			{
				return this.cancelled;
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06002614 RID: 9748 RVA: 0x000C8D05 File Offset: 0x000C6F05
		public bool? Found
		{
			get
			{
				return this.found;
			}
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06002615 RID: 9749 RVA: 0x000C8D10 File Offset: 0x000C6F10
		public Faction RequesterFaction
		{
			get
			{
				if (this.pawn == null)
				{
					return null;
				}
				Pawn_GuestTracker guest = this.pawn.guest;
				if (guest == null)
				{
					return this.pawn.Faction;
				}
				return guest.HostFaction ?? this.pawn.Faction;
			}
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06002616 RID: 9750 RVA: 0x000C8D57 File Offset: 0x000C6F57
		public Lord RequesterLord
		{
			get
			{
				if (this.pawn == null)
				{
					return null;
				}
				return this.pawn.lord;
			}
		}

		// Token: 0x06002617 RID: 9751 RVA: 0x000C8D70 File Offset: 0x000C6F70
		public PathRequest(Map map, IntVec3 start, LocalTargetInfo dest, IntVec3? exactDestination, TraverseParms traverseParms, PathFinderCostTuning tuning, PathEndMode peMode, Pawn pawn, int tickCreated, int tickStart, int tickDeadline, PathRequest.IPathGridCustomizer customizer = null)
		{
			this.map = map;
			this.start = start;
			this.dest = dest;
			this.exactDestination = exactDestination;
			this.traverseParms = traverseParms;
			this.tuning = tuning;
			this.endMode = peMode;
			this.pawn = pawn;
			this.tickCreated = tickCreated;
			this.tickStart = tickStart;
			this.tickDeadline = tickDeadline;
			this.customizer = customizer;
			this.area = PathUtility.GetAllowedArea(pawn);
		}

		// Token: 0x06002618 RID: 9752 RVA: 0x000C8DED File Offset: 0x000C6FED
		public bool Validate()
		{
			bool flag = this.ValidateInt();
			if (!flag)
			{
				this.found = new bool?(false);
			}
			return flag;
		}

		// Token: 0x06002619 RID: 9753 RVA: 0x000C8E04 File Offset: 0x000C7004
		private bool ValidateInt()
		{
			Pawn pawn = this.traverseParms.pawn;
			if (pawn != null && pawn.Map != this.map)
			{
				Log.Error(string.Concat(new string[]
				{
					"Tried to FindPath for pawn which is spawned in another map. Its map PathFinder should have been used, not this one. pawn=",
					pawn.ToStringSafe<Pawn>(),
					" pawn.Map=",
					pawn.Map.ToStringSafe<Map>(),
					" map=",
					this.map.ToStringSafe<Map>()
				}));
				return false;
			}
			if (!this.start.IsValid)
			{
				Log.Error("Tried to FindPath with invalid start " + this.start.ToStringSafe<IntVec3>() + ", pawn= " + pawn.ToStringSafe<Pawn>());
				return false;
			}
			if (!this.dest.IsValid)
			{
				return false;
			}
			using (ProfilerBlock.Scope("Reachability"))
			{
				if (this.traverseParms.mode == TraverseMode.ByPawn)
				{
					if (!pawn.CanReach(this.dest, this.endMode, Danger.Deadly, this.traverseParms.canBashDoors, this.traverseParms.canBashFences, this.traverseParms.mode))
					{
						return false;
					}
				}
				else if (!this.map.reachability.CanReach(this.start, this.dest, this.endMode, this.traverseParms))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600261A RID: 9754 RVA: 0x000C8F64 File Offset: 0x000C7164
		public void Resolve(PawnPath p)
		{
			if (p != null && p.Found)
			{
				this.path = p;
				this.found = new bool?(true);
				return;
			}
			if (p != null)
			{
				p.Dispose();
			}
			this.found = new bool?(false);
		}

		// Token: 0x0600261B RID: 9755 RVA: 0x000C8F9A File Offset: 0x000C719A
		public bool TryGetPath(out PawnPath outPath)
		{
			if (this.ResultIsReady)
			{
				outPath = this.path;
				return true;
			}
			outPath = null;
			return false;
		}

		// Token: 0x0600261C RID: 9756 RVA: 0x000C8FB2 File Offset: 0x000C71B2
		public void ClaimCalculatedPath()
		{
			this.path = null;
		}

		// Token: 0x0600261D RID: 9757 RVA: 0x000C8FBC File Offset: 0x000C71BC
		public override string ToString()
		{
			return string.Format("({0} -> {1}{2}, {3}, for {4})", new object[]
			{
				this.start,
				this.dest,
				this.dest.HasThing ? string.Format(" @ {0}", this.dest.Cell) : "",
				this.endMode,
				this.pawn
			});
		}

		// Token: 0x0600261E RID: 9758 RVA: 0x000C903D File Offset: 0x000C723D
		public void Dispose()
		{
			this.cancelled = true;
			if (this.path != null)
			{
				this.path.Dispose();
				this.path = null;
			}
		}

		// Token: 0x04001F96 RID: 8086
		public Map map;

		// Token: 0x04001F97 RID: 8087
		private IntVec3 start;

		// Token: 0x04001F98 RID: 8088
		private LocalTargetInfo dest;

		// Token: 0x04001F99 RID: 8089
		private IntVec3? exactDestination;

		// Token: 0x04001F9A RID: 8090
		private TraverseParms traverseParms;

		// Token: 0x04001F9B RID: 8091
		private PathFinderCostTuning tuning;

		// Token: 0x04001F9C RID: 8092
		private PathEndMode endMode;

		// Token: 0x04001F9D RID: 8093
		private int tickCreated;

		// Token: 0x04001F9E RID: 8094
		private int tickStart;

		// Token: 0x04001F9F RID: 8095
		private int tickDeadline;

		// Token: 0x04001FA0 RID: 8096
		public Pawn pawn;

		// Token: 0x04001FA1 RID: 8097
		public PathRequest.IPathGridCustomizer customizer;

		// Token: 0x04001FA2 RID: 8098
		public Area area;

		// Token: 0x04001FA3 RID: 8099
		private bool cancelled;

		// Token: 0x04001FA4 RID: 8100
		private PawnPath path;

		// Token: 0x04001FA5 RID: 8101
		private bool? found;

		// Token: 0x020005CB RID: 1483
		public interface IPathGridCustomizer
		{
			// Token: 0x0600261F RID: 9759
			NativeArray<ushort> GetOffsetGrid();
		}
	}
}
