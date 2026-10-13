using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020026D9 RID: 9945
	public class RitualBehaviorWorker_GravshipLaunch : RitualBehaviorWorker
	{
		// Token: 0x0600D9F9 RID: 55801 RVA: 0x00407D5E File Offset: 0x00405F5E
		public RitualBehaviorWorker_GravshipLaunch()
		{
		}

		// Token: 0x0600D9FA RID: 55802 RVA: 0x00407D78 File Offset: 0x00405F78
		public RitualBehaviorWorker_GravshipLaunch(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x1700215D RID: 8541
		// (get) Token: 0x0600D9FB RID: 55803 RVA: 0x00002501 File Offset: 0x00000701
		public override bool ChecksReservations
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600D9FC RID: 55804 RVA: 0x00407D93 File Offset: 0x00405F93
		public override bool TargetStillAllowed(TargetInfo selectedTarget, LordJob_Ritual ritual)
		{
			return base.TargetStillAllowed(selectedTarget, ritual);
		}

		// Token: 0x0600D9FD RID: 55805 RVA: 0x00002C42 File Offset: 0x00000E42
		public override string ExpectedDuration(Precept_Ritual ritual, RitualRoleAssignments assignments, float quality)
		{
			return null;
		}

		// Token: 0x0600D9FE RID: 55806 RVA: 0x00407DA4 File Offset: 0x00405FA4
		private bool CanReachGravship(Pawn pawn, Building_GravEngine engine)
		{
			if (Find.TickManager.TicksGame != this.cacheTick)
			{
				this.cacheTick = Find.TickManager.TicksGame;
				this.cachedCanReachGravship.Clear();
			}
			bool result;
			if (this.cachedCanReachGravship.TryGetValue(pawn, out result))
			{
				return result;
			}
			IntVec3 intVec;
			bool flag = GravshipUtility.TryFindSpotOnGravship(pawn, engine, out intVec);
			this.cachedCanReachGravship[pawn] = flag;
			return flag;
		}

		// Token: 0x0600D9FF RID: 55807 RVA: 0x00407E08 File Offset: 0x00406008
		public override bool PawnCanFillRole(Pawn pawn, RitualRole role, out string reason, TargetInfo ritualTarget)
		{
			reason = null;
			CompPilotConsole compPilotConsole = ritualTarget.Thing.TryGetComp<CompPilotConsole>();
			Building_GravEngine building_GravEngine = (compPilotConsole != null) ? compPilotConsole.engine : null;
			if (building_GravEngine == null)
			{
				Log.ErrorOnce("Could not find engine for gravship launch", 23184679);
				return false;
			}
			if (role == null)
			{
				if (!this.CanReachGravship(pawn, building_GravEngine))
				{
					reason = "NoPathToGravship".Translate();
					return false;
				}
			}
			else if (!pawn.IsPrisoner && !pawn.CanReach(ritualTarget.Thing, PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				reason = "NoPathToPilotConsole".Translate();
				return false;
			}
			return true;
		}

		// Token: 0x04009391 RID: 37777
		private Dictionary<Pawn, bool> cachedCanReachGravship = new Dictionary<Pawn, bool>();

		// Token: 0x04009392 RID: 37778
		private int cacheTick = -1;
	}
}
