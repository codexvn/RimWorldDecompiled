using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020007AD RID: 1965
	[StaticConstructorOnStartup]
	public sealed class PawnDestinationReservationManager : IExposable
	{
		// Token: 0x06003152 RID: 12626 RVA: 0x00100567 File Offset: 0x000FE767
		public PawnDestinationReservationManager.PawnDestinationSet GetPawnDestinationSetFor(Faction faction)
		{
			if (!this.reservedDestinations.ContainsKey(faction))
			{
				this.reservedDestinations.Add(faction, new PawnDestinationReservationManager.PawnDestinationSet());
			}
			return this.reservedDestinations[faction];
		}

		// Token: 0x06003153 RID: 12627 RVA: 0x00100594 File Offset: 0x000FE794
		public void Reserve(Pawn p, Job job, IntVec3 loc)
		{
			if (p.Faction == null)
			{
				return;
			}
			Pawn pawn;
			if (p.Drafted && p.Faction == Faction.OfPlayer && this.IsReserved(loc, out pawn) && pawn != p && !pawn.HostileTo(p) && pawn.Faction != p.Faction)
			{
				MentalStateDef mentalStateDef = pawn.MentalStateDef;
				if (mentalStateDef == null || mentalStateDef.category != MentalStateCategory.Aggro)
				{
					MentalStateDef mentalStateDef2 = pawn.MentalStateDef;
					if (mentalStateDef2 == null || mentalStateDef2.category != MentalStateCategory.Malicious)
					{
						pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				}
			}
			this.ObsoleteAllClaimedBy(p);
			this.GetPawnDestinationSetFor(p.Faction).list.Add(new PawnDestinationReservationManager.PawnDestinationReservation
			{
				target = loc,
				claimant = p,
				job = job
			});
		}

		// Token: 0x06003154 RID: 12628 RVA: 0x00100660 File Offset: 0x000FE860
		public PawnDestinationReservationManager.PawnDestinationReservation MostRecentReservationFor(Pawn p)
		{
			if (p.Faction == null)
			{
				return null;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(p.Faction).list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].claimant == p && !list[i].obsolete)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x06003155 RID: 12629 RVA: 0x001006C0 File Offset: 0x000FE8C0
		public IntVec3 FirstObsoleteReservationFor(Pawn p)
		{
			if (p.Faction == null)
			{
				return IntVec3.Invalid;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(p.Faction).list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].claimant == p && list[i].obsolete)
				{
					return list[i].target;
				}
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06003156 RID: 12630 RVA: 0x00100730 File Offset: 0x000FE930
		public Job FirstObsoleteReservationJobFor(Pawn p)
		{
			if (p.Faction == null)
			{
				return null;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(p.Faction).list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].claimant == p && list[i].obsolete)
				{
					return list[i].job;
				}
			}
			return null;
		}

		// Token: 0x06003157 RID: 12631 RVA: 0x00100798 File Offset: 0x000FE998
		public bool IsReserved(IntVec3 loc)
		{
			Pawn pawn;
			return this.IsReserved(loc, out pawn);
		}

		// Token: 0x06003158 RID: 12632 RVA: 0x001007B0 File Offset: 0x000FE9B0
		public bool IsReserved(IntVec3 loc, out Pawn claimant)
		{
			foreach (KeyValuePair<Faction, PawnDestinationReservationManager.PawnDestinationSet> keyValuePair in this.reservedDestinations)
			{
				List<PawnDestinationReservationManager.PawnDestinationReservation> list = keyValuePair.Value.list;
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].target == loc)
					{
						claimant = list[i].claimant;
						return true;
					}
				}
			}
			claimant = null;
			return false;
		}

		// Token: 0x06003159 RID: 12633 RVA: 0x00100848 File Offset: 0x000FEA48
		public bool CanReserve(IntVec3 c, Pawn searcher, bool draftedOnly = false)
		{
			if (searcher.Faction == null)
			{
				return true;
			}
			if (searcher.Faction == Faction.OfPlayer)
			{
				return this.CanReserveInt(c, searcher.Faction, searcher, draftedOnly);
			}
			foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
			{
				if (!faction.HostileTo(searcher.Faction) && !this.CanReserveInt(c, faction, searcher, draftedOnly))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600315A RID: 12634 RVA: 0x001008E0 File Offset: 0x000FEAE0
		private bool CanReserveInt(IntVec3 c, Faction faction, Pawn ignoreClaimant = null, bool draftedOnly = false)
		{
			if (faction == null)
			{
				return true;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(faction).list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].target == c && (ignoreClaimant == null || list[i].claimant != ignoreClaimant) && (!draftedOnly || list[i].claimant.Drafted))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600315B RID: 12635 RVA: 0x00100950 File Offset: 0x000FEB50
		public Pawn FirstReserverOf(IntVec3 c, Faction faction)
		{
			if (faction == null)
			{
				return null;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(faction).list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].target == c)
				{
					return list[i].claimant;
				}
			}
			return null;
		}

		// Token: 0x0600315C RID: 12636 RVA: 0x001009A4 File Offset: 0x000FEBA4
		public void ReleaseAllObsoleteClaimedBy(Pawn p)
		{
			if (p.Faction == null)
			{
				return;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(p.Faction).list;
			int i = 0;
			while (i < list.Count)
			{
				if (list[i].claimant == p && list[i].obsolete)
				{
					list[i] = list[list.Count - 1];
					list.RemoveLast<PawnDestinationReservationManager.PawnDestinationReservation>();
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x0600315D RID: 12637 RVA: 0x00100A18 File Offset: 0x000FEC18
		public void ReleaseAllClaimedBy(Pawn p)
		{
			if (p.Faction == null)
			{
				return;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(p.Faction).list;
			int i = 0;
			while (i < list.Count)
			{
				if (list[i].claimant == p)
				{
					list[i] = list[list.Count - 1];
					list.RemoveLast<PawnDestinationReservationManager.PawnDestinationReservation>();
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x0600315E RID: 12638 RVA: 0x00100A80 File Offset: 0x000FEC80
		public void ReleaseClaimedBy(Pawn p, Job job)
		{
			if (p.Faction == null)
			{
				return;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(p.Faction).list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].claimant == p && list[i].job == job)
				{
					list[i].job = null;
					if (list[i].obsolete)
					{
						list[i] = list[list.Count - 1];
						list.RemoveLast<PawnDestinationReservationManager.PawnDestinationReservation>();
						i--;
					}
				}
			}
		}

		// Token: 0x0600315F RID: 12639 RVA: 0x00100B11 File Offset: 0x000FED11
		public void Notify_FactionRemoved(Faction faction)
		{
			if (this.reservedDestinations.ContainsKey(faction))
			{
				this.reservedDestinations.Remove(faction);
			}
		}

		// Token: 0x06003160 RID: 12640 RVA: 0x00100B30 File Offset: 0x000FED30
		public void ObsoleteAllClaimedBy(Pawn p)
		{
			if (p.Faction == null)
			{
				return;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(p.Faction).list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].claimant == p)
				{
					list[i].obsolete = true;
					if (list[i].job == null)
					{
						list[i] = list[list.Count - 1];
						list.RemoveLast<PawnDestinationReservationManager.PawnDestinationReservation>();
						i--;
					}
				}
			}
		}

		// Token: 0x06003161 RID: 12641 RVA: 0x00100BB4 File Offset: 0x000FEDB4
		public void DebugDrawDestinations()
		{
			foreach (PawnDestinationReservationManager.PawnDestinationReservation pawnDestinationReservation in this.GetPawnDestinationSetFor(Faction.OfPlayer).list)
			{
				if (!(pawnDestinationReservation.claimant.Position == pawnDestinationReservation.target))
				{
					IntVec3 target = pawnDestinationReservation.target;
					Vector3 s = new Vector3(1f, 1f, 1f);
					Matrix4x4 matrix = default(Matrix4x4);
					matrix.SetTRS(target.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays), Quaternion.identity, s);
					Graphics.DrawMesh(MeshPool.plane10, matrix, PawnDestinationReservationManager.DestinationMat, 0);
					if (Find.Selector.IsSelected(pawnDestinationReservation.claimant))
					{
						Graphics.DrawMesh(MeshPool.plane10, matrix, PawnDestinationReservationManager.DestinationSelectionMat, 0);
					}
				}
			}
		}

		// Token: 0x06003162 RID: 12642 RVA: 0x00100C9C File Offset: 0x000FEE9C
		public void DebugDrawReservations()
		{
			foreach (KeyValuePair<Faction, PawnDestinationReservationManager.PawnDestinationSet> keyValuePair in this.reservedDestinations)
			{
				foreach (PawnDestinationReservationManager.PawnDestinationReservation pawnDestinationReservation in keyValuePair.Value.list)
				{
					IntVec3 target = pawnDestinationReservation.target;
					MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
					materialPropertyBlock.SetColor("_Color", keyValuePair.Key.Color);
					Vector3 s = new Vector3(1f, 1f, 1f);
					Matrix4x4 matrix = default(Matrix4x4);
					matrix.SetTRS(target.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays), Quaternion.identity, s);
					Graphics.DrawMesh(MeshPool.plane10, matrix, PawnDestinationReservationManager.DestinationMat, 0, Camera.main, 0, materialPropertyBlock);
					if (Find.Selector.IsSelected(pawnDestinationReservation.claimant))
					{
						Graphics.DrawMesh(MeshPool.plane10, matrix, PawnDestinationReservationManager.DestinationSelectionMat, 0);
					}
				}
			}
		}

		// Token: 0x06003163 RID: 12643 RVA: 0x00100DD0 File Offset: 0x000FEFD0
		public void ExposeData()
		{
			Scribe_Collections.Look<Faction, PawnDestinationReservationManager.PawnDestinationSet>(ref this.reservedDestinations, "reservedDestinations", LookMode.Reference, LookMode.Deep, ref this.reservedDestinationsKeysWorkingList, ref this.reservedDestinationsValuesWorkingList, true, false, false);
		}

		// Token: 0x040025C1 RID: 9665
		private Dictionary<Faction, PawnDestinationReservationManager.PawnDestinationSet> reservedDestinations = new Dictionary<Faction, PawnDestinationReservationManager.PawnDestinationSet>();

		// Token: 0x040025C2 RID: 9666
		private static readonly Material DestinationMat = MaterialPool.MatFrom("UI/Overlays/ReservedDestination");

		// Token: 0x040025C3 RID: 9667
		private static readonly Material DestinationSelectionMat = MaterialPool.MatFrom("UI/Overlays/ReservedDestinationSelection");

		// Token: 0x040025C4 RID: 9668
		private List<Faction> reservedDestinationsKeysWorkingList;

		// Token: 0x040025C5 RID: 9669
		private List<PawnDestinationReservationManager.PawnDestinationSet> reservedDestinationsValuesWorkingList;

		// Token: 0x020007AE RID: 1966
		public class PawnDestinationReservation : IExposable
		{
			// Token: 0x06003166 RID: 12646 RVA: 0x00100E34 File Offset: 0x000FF034
			public void ExposeData()
			{
				Scribe_Values.Look<IntVec3>(ref this.target, "target", default(IntVec3), false);
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
				Scribe_References.Look<Job>(ref this.job, "job", false);
				Scribe_Values.Look<bool>(ref this.obsolete, "obsolete", false, false);
			}

			// Token: 0x040025C6 RID: 9670
			public IntVec3 target;

			// Token: 0x040025C7 RID: 9671
			public Pawn claimant;

			// Token: 0x040025C8 RID: 9672
			public Job job;

			// Token: 0x040025C9 RID: 9673
			public bool obsolete;
		}

		// Token: 0x020007AF RID: 1967
		public class PawnDestinationSet : IExposable
		{
			// Token: 0x06003168 RID: 12648 RVA: 0x00100E90 File Offset: 0x000FF090
			public void ExposeData()
			{
				Scribe_Collections.Look<PawnDestinationReservationManager.PawnDestinationReservation>(ref this.list, "list", LookMode.Deep, Array.Empty<object>());
				if (Scribe.mode == LoadSaveMode.PostLoadInit)
				{
					if (this.list.RemoveAll((PawnDestinationReservationManager.PawnDestinationReservation x) => x.claimant.DestroyedOrNull()) != 0)
					{
						Log.Warning("Some destination reservations had null or destroyed claimant.");
					}
				}
			}

			// Token: 0x040025CA RID: 9674
			public List<PawnDestinationReservationManager.PawnDestinationReservation> list = new List<PawnDestinationReservationManager.PawnDestinationReservation>();
		}
	}
}
