using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020024D1 RID: 9425
	[StaticConstructorOnStartup]
	public class Pawn_RopeTracker : IExposable
	{
		// Token: 0x0600CD7A RID: 52602 RVA: 0x003C4793 File Offset: 0x003C2993
		public Pawn_RopeTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17001F77 RID: 8055
		// (get) Token: 0x0600CD7B RID: 52603 RVA: 0x003C47B8 File Offset: 0x003C29B8
		public bool IsRoped
		{
			get
			{
				return this.ropedTo.IsValid;
			}
		}

		// Token: 0x17001F78 RID: 8056
		// (get) Token: 0x0600CD7C RID: 52604 RVA: 0x003C47C5 File Offset: 0x003C29C5
		public bool IsRopedByPawn
		{
			get
			{
				return this.RopedByPawn != null;
			}
		}

		// Token: 0x17001F79 RID: 8057
		// (get) Token: 0x0600CD7D RID: 52605 RVA: 0x003C47D0 File Offset: 0x003C29D0
		public bool IsRopedToSpot
		{
			get
			{
				return this.ropedTo.IsValid && !this.IsRopedByPawn;
			}
		}

		// Token: 0x17001F7A RID: 8058
		// (get) Token: 0x0600CD7E RID: 52606 RVA: 0x003C47EA File Offset: 0x003C29EA
		public bool IsRopedToHitchingPost
		{
			get
			{
				return this.RopedToHitchingSpot != null;
			}
		}

		// Token: 0x17001F7B RID: 8059
		// (get) Token: 0x0600CD7F RID: 52607 RVA: 0x003C47F5 File Offset: 0x003C29F5
		public bool IsRopingOthers
		{
			get
			{
				return this.Ropees.Any<Pawn>();
			}
		}

		// Token: 0x17001F7C RID: 8060
		// (get) Token: 0x0600CD80 RID: 52608 RVA: 0x003C4802 File Offset: 0x003C2A02
		public bool HasAnyRope
		{
			get
			{
				return this.IsRoped || this.IsRopingOthers;
			}
		}

		// Token: 0x17001F7D RID: 8061
		// (get) Token: 0x0600CD81 RID: 52609 RVA: 0x003C4814 File Offset: 0x003C2A14
		public Pawn RopedByPawn
		{
			get
			{
				return this.ropedTo.Thing as Pawn;
			}
		}

		// Token: 0x17001F7E RID: 8062
		// (get) Token: 0x0600CD82 RID: 52610 RVA: 0x003C4826 File Offset: 0x003C2A26
		public IntVec3 RopedToSpot
		{
			get
			{
				if (!this.IsRopedToSpot)
				{
					return IntVec3.Invalid;
				}
				return this.ropedTo.Cell;
			}
		}

		// Token: 0x17001F7F RID: 8063
		// (get) Token: 0x0600CD83 RID: 52611 RVA: 0x003C4841 File Offset: 0x003C2A41
		public LocalTargetInfo RopedTo
		{
			get
			{
				return this.ropedTo;
			}
		}

		// Token: 0x17001F80 RID: 8064
		// (get) Token: 0x0600CD84 RID: 52612 RVA: 0x003C4849 File Offset: 0x003C2A49
		public List<Pawn> Ropees
		{
			get
			{
				return this.ropees;
			}
		}

		// Token: 0x17001F81 RID: 8065
		// (get) Token: 0x0600CD85 RID: 52613 RVA: 0x003C4851 File Offset: 0x003C2A51
		public Building RopedToHitchingSpot
		{
			get
			{
				return this.hitchingSpotInt;
			}
		}

		// Token: 0x17001F82 RID: 8066
		// (get) Token: 0x0600CD86 RID: 52614 RVA: 0x003C485C File Offset: 0x003C2A5C
		public bool AnyRopeesFenceBlocked
		{
			get
			{
				if (this.ropees.Count == 0)
				{
					return false;
				}
				for (int i = 0; i < this.ropees.Count; i++)
				{
					if (this.ropees[i].def.race.FenceBlocked)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17001F83 RID: 8067
		// (get) Token: 0x0600CD87 RID: 52615 RVA: 0x003C48B0 File Offset: 0x003C2AB0
		public string InspectLine
		{
			get
			{
				if (this.IsRopedByPawn)
				{
					return "RopedByPawn".Translate() + ": " + this.RopedByPawn.LabelShort;
				}
				if (this.ropedTo.HasThing)
				{
					return "RopedToThing".Translate() + ": " + this.ropedTo.Label;
				}
				return "RopedToSpot".Translate();
			}
		}

		// Token: 0x0600CD88 RID: 52616 RVA: 0x003C4938 File Offset: 0x003C2B38
		public void RopingTick()
		{
			if (!this.HasAnyRope)
			{
				return;
			}
			if (this.pawn.Dead || this.pawn.Downed || this.pawn.Drafted || (!this.pawn.Awake() && this.IsRopedByPawn) || this.ShouldDropRopesDueToMentalState() || this.pawn.IsBurning())
			{
				this.BreakAllRopes();
				return;
			}
			if (this.ropees.Any<Pawn>() && !this.IsStillDoingRopingJob(this.pawn))
			{
				this.BreakAllRopes();
				return;
			}
			if (this.ropedTo.IsValid && !this.pawn.CanReach(this.ropedTo, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				this.BreakRopeWithRoper();
			}
			if (this.IsRopedToHitchingPost && !this.RopedToHitchingSpot.Spawned)
			{
				this.UnropeFromSpot();
			}
		}

		// Token: 0x0600CD89 RID: 52617 RVA: 0x003C4A0E File Offset: 0x003C2C0E
		private bool IsStillDoingRopingJob(Pawn roper)
		{
			return roper.jobs.curJob == null || roper.jobs.curDriver is JobDriver_RopeToDestination;
		}

		// Token: 0x0600CD8A RID: 52618 RVA: 0x003C4A32 File Offset: 0x003C2C32
		private bool ShouldDropRopesDueToMentalState()
		{
			return this.pawn.InMentalState && this.pawn.MentalStateDef != MentalStateDefOf.Roaming;
		}

		// Token: 0x0600CD8B RID: 52619 RVA: 0x003C4A58 File Offset: 0x003C2C58
		public void RopingDraw()
		{
			if (!this.ropedTo.IsValid)
			{
				return;
			}
			Vector3 vector = this.ropedTo.CenterVector3.Yto0();
			if (!this.ropedTo.HasThing && this.ropedTo.Cell.GetFirstThing(this.pawn.Map, ThingDefOf.CaravanPackingSpot) != null)
			{
				vector += Pawn_RopeTracker.CaravanHitchingPostOffset;
			}
			GenDraw.DrawLineBetween(this.pawn.DrawPos.Yto0(), vector, AltitudeLayer.PawnRope.AltitudeFor(), Pawn_RopeTracker.RopeLineMat, 0.2f);
		}

		// Token: 0x0600CD8C RID: 52620 RVA: 0x003C4AE6 File Offset: 0x003C2CE6
		public void Notify_DeSpawned()
		{
			this.BreakAllRopes();
		}

		// Token: 0x0600CD8D RID: 52621 RVA: 0x003C4AEE File Offset: 0x003C2CEE
		private void BreakRopeWithRoper()
		{
			Pawn ropedByPawn = this.RopedByPawn;
			if (ropedByPawn != null)
			{
				ropedByPawn.roping.DropRope(this.pawn);
			}
			this.ropedTo = LocalTargetInfo.Invalid;
		}

		// Token: 0x0600CD8E RID: 52622 RVA: 0x003C4B17 File Offset: 0x003C2D17
		public void BreakAllRopes()
		{
			this.BreakRopeWithRoper();
			this.DropRopes();
			this.TryUnropeFromHitchingSpot();
		}

		// Token: 0x0600CD8F RID: 52623 RVA: 0x003C4B2C File Offset: 0x003C2D2C
		public void RopeToSpot(IntVec3 spot)
		{
			Building edifice = spot.GetEdifice(this.pawn.Map);
			Pawn_RopeTracker.CreateRope(spot, this.pawn);
			if (edifice != null && edifice.def == ThingDefOf.CaravanPackingSpot)
			{
				this.hitchingSpotInt = edifice;
				this.RopedToHitchingSpot.GetComp<CompHitchingSpot>().AddPawn(this.pawn);
			}
		}

		// Token: 0x0600CD90 RID: 52624 RVA: 0x003C4B89 File Offset: 0x003C2D89
		public void TryUnropeFromHitchingSpot()
		{
			if (this.RopedToHitchingSpot != null)
			{
				this.RopedToHitchingSpot.GetComp<CompHitchingSpot>().RemovePawn(this.pawn);
			}
			this.hitchingSpotInt = null;
		}

		// Token: 0x0600CD91 RID: 52625 RVA: 0x003C4BB0 File Offset: 0x003C2DB0
		public void RopePawn(Pawn ropee)
		{
			Pawn_RopeTracker.CreateRope(this.pawn, ropee);
		}

		// Token: 0x0600CD92 RID: 52626 RVA: 0x003C4BC4 File Offset: 0x003C2DC4
		private static void CreateRope(LocalTargetInfo roperTarget, Pawn ropee)
		{
			Pawn pawn = roperTarget.Thing as Pawn;
			ropee.roping.DropRopes();
			ropee.roping.TryUnropeFromHitchingSpot();
			ropee.roping.ropedTo = roperTarget;
			if (pawn != null)
			{
				pawn.roping.ropees.Add(ropee);
				ReachabilityUtility.ClearCacheFor(pawn);
			}
			ReachabilityUtility.ClearCacheFor(ropee);
			if (ropee.jobs != null && ropee.CurJob != null)
			{
				ropee.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		// Token: 0x0600CD93 RID: 52627 RVA: 0x003C4C3F File Offset: 0x003C2E3F
		public void UnropeFromSpot()
		{
			if (!this.IsRopedToSpot)
			{
				Log.Warning(string.Format("Tried to unrope {0} from spot, but not roped to a spot", this.pawn));
				return;
			}
			this.TryUnropeFromHitchingSpot();
			this.ropedTo = LocalTargetInfo.Invalid;
			ReachabilityUtility.ClearCacheFor(this.pawn);
		}

		// Token: 0x0600CD94 RID: 52628 RVA: 0x003C4C7C File Offset: 0x003C2E7C
		public void DropRope(Pawn ropee)
		{
			if (this.pawn != ropee.roping.RopedByPawn)
			{
				Log.Warning(string.Format("{0} tried to drop for {1} but ropee holder was {2}", this.pawn, ropee, ropee.roping.RopedByPawn));
			}
			this.TryUnropeFromHitchingSpot();
			ropee.roping.ropedTo = LocalTargetInfo.Invalid;
			this.ropees.Remove(ropee);
			ReachabilityUtility.ClearCacheFor(ropee);
			ReachabilityUtility.ClearCacheFor(this.pawn);
		}

		// Token: 0x0600CD95 RID: 52629 RVA: 0x003C4CF4 File Offset: 0x003C2EF4
		public void DropRopes()
		{
			if (this.ropees.Count == 0)
			{
				return;
			}
			foreach (Pawn ropee in new List<Pawn>(this.ropees))
			{
				this.DropRope(ropee);
			}
		}

		// Token: 0x0600CD96 RID: 52630 RVA: 0x003C4D5C File Offset: 0x003C2F5C
		public void ExposeData()
		{
			Scribe_TargetInfo.Look(ref this.ropedTo, "ropedTo", LocalTargetInfo.Invalid);
			Scribe_References.Look<Building>(ref this.hitchingSpotInt, "hitchingPostInt", false);
			Scribe_Collections.Look<Pawn>(ref this.ropees, "ropees", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.ropees.RemoveAll((Pawn x) => x.DestroyedOrNull());
			}
		}

		// Token: 0x0600CD97 RID: 52631 RVA: 0x003C4DD8 File Offset: 0x003C2FD8
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Format("{0}", this.pawn));
			if (this.ropedTo.HasThing)
			{
				stringBuilder.Append(string.Format(" ropedBy: {0}", this.ropedTo.Thing));
			}
			else if (this.ropedTo.IsValid)
			{
				stringBuilder.Append(string.Format(" roped at: {0}", this.ropedTo.Cell));
			}
			stringBuilder.AppendLine();
			foreach (Pawn arg in this.Ropees)
			{
				stringBuilder.AppendLine(string.Format("  ropee: {0}", arg));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04008BED RID: 35821
		public const float RopeLength = 8f;

		// Token: 0x04008BEE RID: 35822
		private static readonly Vector3 CaravanHitchingPostOffset = new Vector3(0f, 0f, 0.19f);

		// Token: 0x04008BEF RID: 35823
		private static readonly string RopeTexPath = "UI/Overlays/Rope";

		// Token: 0x04008BF0 RID: 35824
		private static readonly Material RopeLineMat = MaterialPool.MatFrom(Pawn_RopeTracker.RopeTexPath, ShaderDatabase.Transparent, GenColor.FromBytes(99, 70, 41, 255));

		// Token: 0x04008BF1 RID: 35825
		private Pawn pawn;

		// Token: 0x04008BF2 RID: 35826
		private LocalTargetInfo ropedTo = LocalTargetInfo.Invalid;

		// Token: 0x04008BF3 RID: 35827
		private List<Pawn> ropees = new List<Pawn>();

		// Token: 0x04008BF4 RID: 35828
		private Building hitchingSpotInt;
	}
}
