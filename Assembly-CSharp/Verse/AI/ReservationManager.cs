using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000FD1 RID: 4049
	[StaticConstructorOnStartup]
	public sealed class ReservationManager : IExposable
	{
		// Token: 0x17000FB8 RID: 4024
		// (get) Token: 0x060063EF RID: 25583 RVA: 0x001FB3A5 File Offset: 0x001F95A5
		public List<ReservationManager.Reservation> ReservationsReadOnly
		{
			get
			{
				return this.reservations;
			}
		}

		// Token: 0x060063F0 RID: 25584 RVA: 0x001FB3AD File Offset: 0x001F95AD
		public ReservationManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x060063F1 RID: 25585 RVA: 0x001FB3D4 File Offset: 0x001F95D4
		public void ExposeData()
		{
			Scribe_Collections.Look<ReservationManager.Reservation>(ref this.reservations, "reservations", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int i = this.reservations.Count - 1; i >= 0; i--)
				{
					ReservationManager.Reservation reservation = this.reservations[i];
					if (reservation.Target.Thing != null && reservation.Target.Thing.Destroyed)
					{
						string str = "Loaded reservation with destroyed target: ";
						ReservationManager.Reservation reservation2 = reservation;
						Log.Error(str + ((reservation2 != null) ? reservation2.ToString() : null) + ". Deleting it...");
						this.reservations.Remove(reservation);
					}
					if (reservation.Claimant != null && reservation.Claimant.Destroyed)
					{
						string str2 = "Loaded reservation with destroyed claimant: ";
						ReservationManager.Reservation reservation3 = reservation;
						Log.Error(str2 + ((reservation3 != null) ? reservation3.ToString() : null) + ". Deleting it...");
						this.reservations.Remove(reservation);
					}
					if (reservation.Claimant == null)
					{
						string str3 = "Loaded reservation with null claimant: ";
						ReservationManager.Reservation reservation4 = reservation;
						Log.Error(str3 + ((reservation4 != null) ? reservation4.ToString() : null) + ". Deleting it...");
						this.reservations.Remove(reservation);
					}
					if (reservation.Job == null)
					{
						string str4 = "Loaded reservation with null job: ";
						ReservationManager.Reservation reservation5 = reservation;
						Log.Error(str4 + ((reservation5 != null) ? reservation5.ToString() : null) + ". Deleting it...");
						this.reservations.Remove(reservation);
					}
				}
			}
		}

		// Token: 0x060063F2 RID: 25586 RVA: 0x001FB530 File Offset: 0x001F9730
		public bool IsReserved(LocalTargetInfo target)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				if (this.reservations[i].Target == target)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060063F3 RID: 25587 RVA: 0x001FB570 File Offset: 0x001F9770
		public void ReserversOf(LocalTargetInfo target, HashSet<Pawn> reserversOut)
		{
			this.map.physicalInteractionReservationManager.ReserversOf(target, reserversOut);
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (reservation.Target == target)
				{
					reserversOut.Add(reservation.Claimant);
				}
			}
		}

		// Token: 0x060063F4 RID: 25588 RVA: 0x001FB5D0 File Offset: 0x001F97D0
		public bool HasReservedOfDef(Pawn claimant, ThingDef thingDef)
		{
			foreach (ReservationManager.Reservation reservation in this.reservations)
			{
				if (reservation.Target.HasThing && reservation.Target.Thing.def == thingDef && reservation.Claimant == claimant)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060063F5 RID: 25589 RVA: 0x001FB654 File Offset: 0x001F9854
		public bool CanReserve(Pawn claimant, LocalTargetInfo target, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			if (claimant == null)
			{
				Log.Error("CanReserve with null claimant");
				return false;
			}
			if (!claimant.Spawned || claimant.Map != this.map)
			{
				return false;
			}
			if (!target.IsValid || target.ThingDestroyed)
			{
				return false;
			}
			if (target.HasThing && target.Thing.SpawnedOrAnyParentSpawned && target.Thing.MapHeld != this.map)
			{
				return false;
			}
			int num = target.HasThing ? target.Thing.stackCount : 1;
			int num2 = (stackCount == -1) ? num : stackCount;
			if (num2 > num)
			{
				return false;
			}
			if (!ignoreOtherReservations)
			{
				if (this.map.physicalInteractionReservationManager.IsReserved(target) && !this.map.physicalInteractionReservationManager.IsReservedBy(claimant, target))
				{
					return false;
				}
				if (this.IsAlreadyReserved(claimant, target, layer, num2))
				{
					return true;
				}
				if (target.HasThing)
				{
					Building building = target.Thing as Building;
					if (building != null && building.def.hasInteractionCell)
					{
						IntVec3 interactionCell = building.InteractionCell;
						Building edifice = interactionCell.GetEdifice(claimant.Map);
						if (edifice != null)
						{
							Pawn pawn;
							if (claimant.Map.reservationManager.TryGetReserver(edifice, claimant.Faction, out pawn) && pawn.Spawned && pawn != claimant)
							{
								return false;
							}
							goto IL_189;
						}
						else
						{
							Pawn pawn2;
							if (claimant.Map.reservationManager.TryGetReserver(interactionCell, claimant.Faction, out pawn2) && pawn2.Spawned && pawn2 != claimant)
							{
								return false;
							}
							goto IL_189;
						}
					}
				}
				if (this.IsAlreadyReserved(claimant, target, layer, num2))
				{
					return true;
				}
				IL_189:
				this.tmpReservers.Clear();
				int num3 = 0;
				int num4 = 0;
				for (int i = 0; i < this.reservations.Count; i++)
				{
					ReservationManager.Reservation reservation = this.reservations[i];
					if (!(reservation.Target != target) && reservation.Layer == layer && reservation.Claimant != claimant && !this.tmpReservers.Contains(reservation.Claimant) && ReservationManager.RespectsReservationsOf(claimant, reservation.Claimant))
					{
						if (reservation.MaxPawns != maxPawns)
						{
							return false;
						}
						this.tmpReservers.Add(reservation.Claimant);
						num3++;
						if (reservation.StackCount == -1)
						{
							num4 += num;
						}
						else
						{
							num4 += reservation.StackCount;
						}
						if (num3 >= maxPawns || num2 + num4 > num)
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x060063F6 RID: 25590 RVA: 0x001FB8C4 File Offset: 0x001F9AC4
		private bool IsAlreadyReserved(Pawn claimant, LocalTargetInfo target, ReservationLayerDef layer, int toReserveResolved)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (reservation.Target == target && reservation.Layer == layer && reservation.Claimant == claimant && (reservation.StackCount == -1 || reservation.StackCount >= toReserveResolved))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060063F7 RID: 25591 RVA: 0x001FB92C File Offset: 0x001F9B2C
		public int CanReserveStack(Pawn claimant, LocalTargetInfo target, int maxPawns = 1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			if (claimant == null)
			{
				Log.Error("CanReserve with null claimant");
				return 0;
			}
			if (!claimant.Spawned || claimant.Map != this.map)
			{
				return 0;
			}
			if (!target.IsValid || target.ThingDestroyed)
			{
				return 0;
			}
			if (target.HasThing && target.Thing.SpawnedOrAnyParentSpawned && target.Thing.MapHeld != this.map)
			{
				return 0;
			}
			int num = target.HasThing ? target.Thing.stackCount : 1;
			int num2 = 0;
			if (!ignoreOtherReservations)
			{
				if (this.map.physicalInteractionReservationManager.IsReserved(target) && !this.map.physicalInteractionReservationManager.IsReservedBy(claimant, target))
				{
					return 0;
				}
				int num3 = 0;
				for (int i = 0; i < this.reservations.Count; i++)
				{
					ReservationManager.Reservation reservation = this.reservations[i];
					if (!(reservation.Target != target) && reservation.Layer == layer && reservation.Claimant != claimant && ReservationManager.RespectsReservationsOf(claimant, reservation.Claimant))
					{
						if (reservation.MaxPawns != maxPawns)
						{
							return 0;
						}
						num3++;
						if (reservation.StackCount == -1)
						{
							num2 += num;
						}
						else
						{
							num2 += reservation.StackCount;
						}
						if (num3 >= maxPawns || num2 >= num)
						{
							return 0;
						}
					}
				}
			}
			return Mathf.Max(num - num2, 0);
		}

		// Token: 0x060063F8 RID: 25592 RVA: 0x001FBA88 File Offset: 0x001F9C88
		public bool Reserve(Pawn claimant, Job job, LocalTargetInfo target, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool errorOnFailed = true, bool ignoreOtherReservations = false, bool canReserversStartJobs = true)
		{
			if (maxPawns > 1 && stackCount == -1)
			{
				Log.ErrorOnce("Reserving with maxPawns > 1 and stackCount = All; this will not have a useful effect (suppressing future warnings)", 83269);
			}
			if (job == null)
			{
				Log.Warning(claimant.ToStringSafe<Pawn>() + " tried to reserve thing " + target.ToStringSafe<LocalTargetInfo>() + " without a valid job");
				return false;
			}
			int num = target.HasThing ? target.Thing.stackCount : 1;
			int num2 = (stackCount == -1) ? num : stackCount;
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (reservation.Target == target && reservation.Claimant == claimant && reservation.Job == job && reservation.Layer == layer && (reservation.StackCount == -1 || reservation.StackCount >= num2))
				{
					return true;
				}
			}
			if (!target.IsValid || target.ThingDestroyed)
			{
				return false;
			}
			if (this.CanReserve(claimant, target, maxPawns, stackCount, layer, ignoreOtherReservations))
			{
				ReservationManager.Reservation reservation2 = new ReservationManager.Reservation(claimant, job, maxPawns, stackCount, target, layer);
				this.reservations.Add(reservation2);
				this.map.events.Notify_ReservationAdded(reservation2);
				return true;
			}
			if (job.playerForced && this.CanReserve(claimant, target, maxPawns, stackCount, layer, true))
			{
				this.reservations.Add(new ReservationManager.Reservation(claimant, job, maxPawns, stackCount, target, layer));
				foreach (ReservationManager.Reservation reservation3 in this.reservations.ToList<ReservationManager.Reservation>())
				{
					if (reservation3.Claimant != claimant && reservation3.Layer == layer && ReservationManager.RespectsReservationsOf(claimant, reservation3.Claimant))
					{
						if (reservation3.Target == target)
						{
							reservation3.Claimant.jobs.EndCurrentOrQueuedJob(reservation3.Job, JobCondition.InterruptForced, canReserversStartJobs, true);
						}
						else if (target.HasThing)
						{
							Building building = target.Thing as Building;
							if (building != null && building.def.hasInteractionCell)
							{
								IntVec3 interactionCell = building.InteractionCell;
								Building edifice = interactionCell.GetEdifice(claimant.Map);
								if (reservation3.Claimant != claimant && reservation3.Layer == layer && ReservationManager.RespectsReservationsOf(claimant, reservation3.Claimant))
								{
									if (edifice != null && reservation3.Target == edifice)
									{
										reservation3.Claimant.jobs.EndCurrentOrQueuedJob(reservation3.Job, JobCondition.InterruptForced, true, true);
									}
									else if (reservation3.Target == interactionCell)
									{
										reservation3.Claimant.jobs.EndCurrentOrQueuedJob(reservation3.Job, JobCondition.InterruptForced, true, true);
									}
								}
							}
						}
					}
				}
				return true;
			}
			if (errorOnFailed)
			{
				this.LogCouldNotReserveError(claimant, job, target, maxPawns, stackCount, layer);
			}
			return false;
		}

		// Token: 0x060063F9 RID: 25593 RVA: 0x001FBD94 File Offset: 0x001F9F94
		public void Release(LocalTargetInfo target, Pawn claimant, Job job)
		{
			if (target.ThingDestroyed)
			{
				string str = "Releasing destroyed thing ";
				LocalTargetInfo localTargetInfo = target;
				Log.Warning(str + localTargetInfo.ToString() + " for " + ((claimant != null) ? claimant.ToString() : null));
			}
			ReservationManager.Reservation reservation = null;
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation2 = this.reservations[i];
				if (reservation2.Target == target && reservation2.Claimant == claimant && reservation2.Job == job)
				{
					reservation = reservation2;
					break;
				}
			}
			if (reservation == null && !target.ThingDestroyed)
			{
				string[] array = new string[5];
				array[0] = "Tried to release ";
				int num = 1;
				LocalTargetInfo localTargetInfo = target;
				array[num] = localTargetInfo.ToString();
				array[2] = " that wasn't reserved by ";
				array[3] = ((claimant != null) ? claimant.ToString() : null);
				array[4] = ".";
				Log.Error(string.Concat(array));
				return;
			}
			this.reservations.Remove(reservation);
			this.map.events.Notify_ReservationRemoved(reservation);
		}

		// Token: 0x060063FA RID: 25594 RVA: 0x001FBE98 File Offset: 0x001FA098
		public void ReleaseAllForTarget(Thing t)
		{
			if (t == null)
			{
				return;
			}
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].Target.Thing == t)
				{
					this.reservations.RemoveAt(i);
				}
			}
		}

		// Token: 0x060063FB RID: 25595 RVA: 0x001FBEEC File Offset: 0x001FA0EC
		public void ReleaseClaimedBy(Pawn claimant, Job job)
		{
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].Claimant == claimant && this.reservations[i].Job == job)
				{
					this.reservations.RemoveAt(i);
				}
			}
		}

		// Token: 0x060063FC RID: 25596 RVA: 0x001FBF48 File Offset: 0x001FA148
		public void ReleaseAllClaimedBy(Pawn claimant)
		{
			if (claimant == null)
			{
				return;
			}
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].Claimant == claimant)
				{
					this.reservations.RemoveAt(i);
				}
			}
		}

		// Token: 0x060063FD RID: 25597 RVA: 0x001FBF94 File Offset: 0x001FA194
		public LocalTargetInfo FirstReservationFor(Pawn claimant)
		{
			if (claimant == null)
			{
				return LocalTargetInfo.Invalid;
			}
			for (int i = 0; i < this.reservations.Count; i++)
			{
				if (this.reservations[i].Claimant == claimant)
				{
					return this.reservations[i].Target;
				}
			}
			return LocalTargetInfo.Invalid;
		}

		// Token: 0x060063FE RID: 25598 RVA: 0x001FBFEC File Offset: 0x001FA1EC
		public bool IsReservedByAnyoneOf(LocalTargetInfo target, Faction faction)
		{
			Pawn pawn;
			return this.TryGetReserver(target, faction, out pawn);
		}

		// Token: 0x060063FF RID: 25599 RVA: 0x001FC004 File Offset: 0x001FA204
		public bool TryGetReserver(LocalTargetInfo target, Faction faction, out Pawn reserver)
		{
			reserver = null;
			if (!target.IsValid)
			{
				return false;
			}
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (reservation.Target == target && reservation.Claimant.Faction == faction)
				{
					reserver = reservation.Claimant;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006400 RID: 25600 RVA: 0x001FC06C File Offset: 0x001FA26C
		public bool OnlyReservationsForJobDef(LocalTargetInfo target, JobDef requiredJobDef, bool requireAtLeastOne = false)
		{
			if (!target.IsValid)
			{
				return false;
			}
			bool flag = false;
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (!(reservation.Target != target))
				{
					if (reservation.Job.def != requiredJobDef)
					{
						return false;
					}
					flag = true;
				}
			}
			return !requireAtLeastOne || flag;
		}

		// Token: 0x06006401 RID: 25601 RVA: 0x001FC0CF File Offset: 0x001FA2CF
		public bool IsReservedAndRespected(LocalTargetInfo target, Pawn claimant)
		{
			return this.FirstRespectedReserver(target, claimant, null) != null;
		}

		// Token: 0x06006402 RID: 25602 RVA: 0x001FC0E0 File Offset: 0x001FA2E0
		public bool IsReservedAndRespected(LocalTargetInfo target, Faction faction)
		{
			if (!target.IsValid)
			{
				return false;
			}
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (!(reservation.Target != target) && ReservationManager.RespectsReservationsOf(faction, reservation.Claimant))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006403 RID: 25603 RVA: 0x001FC13C File Offset: 0x001FA33C
		public Pawn FirstRespectedReserver(LocalTargetInfo target, Pawn claimant, ReservationLayerDef layer = null)
		{
			if (!target.IsValid)
			{
				return null;
			}
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (reservation.Target == target && (layer == null || reservation.Layer == layer) && ReservationManager.RespectsReservationsOf(claimant, reservation.Claimant))
				{
					return reservation.Claimant;
				}
			}
			return null;
		}

		// Token: 0x06006404 RID: 25604 RVA: 0x001FC1A8 File Offset: 0x001FA3A8
		public bool ReservedBy(LocalTargetInfo target, Pawn claimant, Job job = null)
		{
			if (!target.IsValid)
			{
				return false;
			}
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (reservation.Target == target && reservation.Claimant == claimant && (job == null || reservation.Job == job))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006405 RID: 25605 RVA: 0x001FC20C File Offset: 0x001FA40C
		public bool ReservedBy<TDriver>(LocalTargetInfo target, Pawn claimant, LocalTargetInfo? targetAIsNot = null, LocalTargetInfo? targetBIsNot = null, LocalTargetInfo? targetCIsNot = null)
		{
			if (!target.IsValid)
			{
				return false;
			}
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (reservation.Target == target && reservation.Claimant == claimant && reservation.Job != null && typeof(TDriver).IsAssignableFrom(reservation.Job.def.driverClass) && (targetAIsNot == null || reservation.Job.targetA != targetAIsNot) && (targetBIsNot == null || reservation.Job.targetB != targetBIsNot) && (targetCIsNot == null || reservation.Job.targetC != targetCIsNot))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006406 RID: 25606 RVA: 0x001FC334 File Offset: 0x001FA534
		public IEnumerable<Thing> AllReservedThings()
		{
			return from res in this.reservations
			select res.Target.Thing;
		}

		// Token: 0x06006407 RID: 25607 RVA: 0x001FC360 File Offset: 0x001FA560
		private static bool RespectsReservationsOf(Faction faction, Pawn claimant)
		{
			return faction != null && ((claimant != null) ? claimant.Faction : null) != null && (faction == claimant.Faction || !faction.HostileTo(claimant.Faction) || faction == claimant.HostFaction);
		}

		// Token: 0x06006408 RID: 25608 RVA: 0x001FC39C File Offset: 0x001FA59C
		private static bool RespectsReservationsOf(Pawn newClaimant, Pawn oldClaimant)
		{
			if (newClaimant == oldClaimant)
			{
				return true;
			}
			if (newClaimant.Faction == null || oldClaimant.Faction == null)
			{
				return false;
			}
			if (newClaimant.Faction == oldClaimant.Faction)
			{
				return true;
			}
			if (!newClaimant.Faction.HostileTo(oldClaimant.Faction))
			{
				return true;
			}
			if (oldClaimant.HostFaction != null && oldClaimant.HostFaction == newClaimant.HostFaction)
			{
				return true;
			}
			if (newClaimant.HostFaction != null)
			{
				if (oldClaimant.HostFaction != null)
				{
					return true;
				}
				if (newClaimant.HostFaction == oldClaimant.Faction)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006409 RID: 25609 RVA: 0x001FC424 File Offset: 0x001FA624
		internal string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("All reservation in ReservationManager:");
			for (int i = 0; i < this.reservations.Count; i++)
			{
				stringBuilder.AppendLine("[" + i.ToString() + "] " + this.reservations[i].ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600640A RID: 25610 RVA: 0x001FC490 File Offset: 0x001FA690
		internal void DebugDrawReservations()
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (reservation.Target.Thing != null)
				{
					if (reservation.Target.Thing.Spawned)
					{
						Thing thing = reservation.Target.Thing;
						Vector3 s = new Vector3((float)thing.RotatedSize.x, 1f, (float)thing.RotatedSize.z);
						Matrix4x4 matrix = default(Matrix4x4);
						matrix.SetTRS(thing.DrawPos + Vector3.up * 0.1f, Quaternion.identity, s);
						Graphics.DrawMesh(MeshPool.plane10, matrix, ReservationManager.DebugReservedThingIcon, 0);
						GenDraw.DrawLineBetween(reservation.Claimant.DrawPos, reservation.Target.Thing.DrawPos);
					}
					else
					{
						Graphics.DrawMesh(MeshPool.plane03, reservation.Claimant.DrawPos + Vector3.up + new Vector3(0.5f, 0f, 0.5f), Quaternion.identity, ReservationManager.DebugReservedThingIcon, 0);
					}
				}
				else
				{
					Graphics.DrawMesh(MeshPool.plane10, reservation.Target.Cell.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays), Quaternion.identity, ReservationManager.DebugReservedThingIcon, 0);
					GenDraw.DrawLineBetween(reservation.Claimant.DrawPos, reservation.Target.Cell.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays));
				}
			}
		}

		// Token: 0x0600640B RID: 25611 RVA: 0x001FC62C File Offset: 0x001FA82C
		private void LogCouldNotReserveError(Pawn claimant, Job job, LocalTargetInfo target, int maxPawns, int stackCount, ReservationLayerDef layer)
		{
			Job curJob = claimant.CurJob;
			string text = "null";
			int num = -1;
			if (curJob != null)
			{
				text = curJob.ToString();
				if (claimant.jobs.curDriver != null)
				{
					num = claimant.jobs.curDriver.CurToilIndex;
				}
			}
			string text2;
			if (target.HasThing && target.Thing.def.stackLimit != 1)
			{
				text2 = "(current stack count: " + target.Thing.stackCount.ToString() + ")";
			}
			else
			{
				text2 = "";
			}
			string text3 = string.Concat(new string[]
			{
				"Could not reserve ",
				target.ToStringSafe<LocalTargetInfo>(),
				text2,
				" (layer: ",
				layer.ToStringSafe<ReservationLayerDef>(),
				") for ",
				claimant.ToStringSafe<Pawn>(),
				" for job ",
				job.ToStringSafe<Job>(),
				" (now doing job ",
				text,
				"(curToil=",
				num.ToString(),
				")) for maxPawns ",
				maxPawns.ToString(),
				" and stackCount ",
				stackCount.ToString(),
				"."
			});
			int num2 = 0;
			bool flag = false;
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (reservation.Target == target && (layer == null || reservation.Layer == layer) && ReservationManager.RespectsReservationsOf(claimant, reservation.Claimant))
				{
					if (!flag)
					{
						text3 += "\nExisting reservers:";
						flag = true;
					}
					string text4 = "null";
					int num3 = -1;
					Job curJob2 = reservation.Claimant.CurJob;
					if (curJob2 != null)
					{
						text4 = curJob2.ToStringSafe<Job>();
						if (reservation.Claimant.jobs.curDriver != null)
						{
							num3 = reservation.Claimant.jobs.curDriver.CurToilIndex;
						}
					}
					text3 += string.Format("\n   [{0}] {1} (job: {2}, toil: {3}, maxPawns: {4}, stackCount: {5})", new object[]
					{
						num2++,
						reservation.Claimant.ToStringSafe<Pawn>(),
						text4,
						num3,
						reservation.MaxPawns,
						reservation.StackCount
					});
				}
			}
			if (!flag)
			{
				text3 += " No existing reserver.";
			}
			Pawn pawn = this.map.physicalInteractionReservationManager.FirstReserverOf(target);
			if (pawn != null)
			{
				text3 = text3 + " Physical interaction reserver: " + pawn.ToStringSafe<Pawn>();
			}
			Log.Error(text3);
		}

		// Token: 0x040047B5 RID: 18357
		private Map map;

		// Token: 0x040047B6 RID: 18358
		private List<ReservationManager.Reservation> reservations = new List<ReservationManager.Reservation>();

		// Token: 0x040047B7 RID: 18359
		private static readonly Material DebugReservedThingIcon = MaterialPool.MatFrom("UI/Overlays/ReservedForWork", ShaderDatabase.Cutout);

		// Token: 0x040047B8 RID: 18360
		public const int StackCount_All = -1;

		// Token: 0x040047B9 RID: 18361
		private List<Pawn> tmpReservers = new List<Pawn>();

		// Token: 0x02000FD2 RID: 4050
		public class Reservation : IExposable
		{
			// Token: 0x17000FB9 RID: 4025
			// (get) Token: 0x0600640D RID: 25613 RVA: 0x001FC8EF File Offset: 0x001FAAEF
			public Pawn Claimant
			{
				get
				{
					return this.claimant;
				}
			}

			// Token: 0x17000FBA RID: 4026
			// (get) Token: 0x0600640E RID: 25614 RVA: 0x001FC8F7 File Offset: 0x001FAAF7
			public Job Job
			{
				get
				{
					return this.job;
				}
			}

			// Token: 0x17000FBB RID: 4027
			// (get) Token: 0x0600640F RID: 25615 RVA: 0x001FC8FF File Offset: 0x001FAAFF
			public LocalTargetInfo Target
			{
				get
				{
					return this.target;
				}
			}

			// Token: 0x17000FBC RID: 4028
			// (get) Token: 0x06006410 RID: 25616 RVA: 0x001FC907 File Offset: 0x001FAB07
			public ReservationLayerDef Layer
			{
				get
				{
					return this.layer;
				}
			}

			// Token: 0x17000FBD RID: 4029
			// (get) Token: 0x06006411 RID: 25617 RVA: 0x001FC90F File Offset: 0x001FAB0F
			public int MaxPawns
			{
				get
				{
					return this.maxPawns;
				}
			}

			// Token: 0x17000FBE RID: 4030
			// (get) Token: 0x06006412 RID: 25618 RVA: 0x001FC917 File Offset: 0x001FAB17
			public int StackCount
			{
				get
				{
					return this.stackCount;
				}
			}

			// Token: 0x17000FBF RID: 4031
			// (get) Token: 0x06006413 RID: 25619 RVA: 0x001FC91F File Offset: 0x001FAB1F
			public Faction Faction
			{
				get
				{
					return this.claimant.Faction;
				}
			}

			// Token: 0x06006414 RID: 25620 RVA: 0x001FC92C File Offset: 0x001FAB2C
			public Reservation()
			{
			}

			// Token: 0x06006415 RID: 25621 RVA: 0x001FC93B File Offset: 0x001FAB3B
			public Reservation(Pawn claimant, Job job, int maxPawns, int stackCount, LocalTargetInfo target, ReservationLayerDef layer)
			{
				this.claimant = claimant;
				this.job = job;
				this.maxPawns = maxPawns;
				this.stackCount = stackCount;
				this.target = target;
				this.layer = layer;
			}

			// Token: 0x06006416 RID: 25622 RVA: 0x001FC978 File Offset: 0x001FAB78
			public void ExposeData()
			{
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
				Scribe_References.Look<Job>(ref this.job, "job", false);
				Scribe_TargetInfo.Look(ref this.target, "target");
				Scribe_Values.Look<int>(ref this.maxPawns, "maxPawns", 0, false);
				Scribe_Values.Look<int>(ref this.stackCount, "stackCount", 0, false);
				Scribe_Defs.Look<ReservationLayerDef>(ref this.layer, "layer");
			}

			// Token: 0x06006417 RID: 25623 RVA: 0x001FC9EC File Offset: 0x001FABEC
			public override string ToString()
			{
				return string.Concat(new string[]
				{
					(this.claimant != null) ? this.claimant.LabelShort : "null",
					":",
					this.job.ToStringSafe<Job>(),
					", ",
					this.target.ToStringSafe<LocalTargetInfo>(),
					", ",
					this.layer.ToStringSafe<ReservationLayerDef>(),
					", ",
					this.maxPawns.ToString(),
					", ",
					this.stackCount.ToString()
				});
			}

			// Token: 0x040047BA RID: 18362
			private Pawn claimant;

			// Token: 0x040047BB RID: 18363
			private Job job;

			// Token: 0x040047BC RID: 18364
			private LocalTargetInfo target;

			// Token: 0x040047BD RID: 18365
			private ReservationLayerDef layer;

			// Token: 0x040047BE RID: 18366
			private int maxPawns;

			// Token: 0x040047BF RID: 18367
			private int stackCount = -1;
		}
	}
}
