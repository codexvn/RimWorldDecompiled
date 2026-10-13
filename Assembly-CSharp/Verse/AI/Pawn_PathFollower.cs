using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000FBD RID: 4029
	public class Pawn_PathFollower : IExposable
	{
		// Token: 0x17000F96 RID: 3990
		// (get) Token: 0x060062FC RID: 25340 RVA: 0x001F309D File Offset: 0x001F129D
		public LocalTargetInfo Destination
		{
			get
			{
				return this.destination;
			}
		}

		// Token: 0x17000F97 RID: 3991
		// (get) Token: 0x060062FD RID: 25341 RVA: 0x001F30A5 File Offset: 0x001F12A5
		public bool Moving
		{
			get
			{
				return this.moving;
			}
		}

		// Token: 0x17000F98 RID: 3992
		// (get) Token: 0x060062FE RID: 25342 RVA: 0x001F30AD File Offset: 0x001F12AD
		public bool MovingNow
		{
			get
			{
				return this.Moving && !this.WillCollideNextCell;
			}
		}

		// Token: 0x17000F99 RID: 3993
		// (get) Token: 0x060062FF RID: 25343 RVA: 0x001F30C2 File Offset: 0x001F12C2
		public float MovePercentage
		{
			get
			{
				return this.cachedMovePercentage;
			}
		}

		// Token: 0x17000F9A RID: 3994
		// (get) Token: 0x06006300 RID: 25344 RVA: 0x001F30CA File Offset: 0x001F12CA
		public int LastMovedTick
		{
			get
			{
				return this.lastMovedTick;
			}
		}

		// Token: 0x17000F9B RID: 3995
		// (get) Token: 0x06006301 RID: 25345 RVA: 0x001F30D2 File Offset: 0x001F12D2
		public bool WillCollideNextCell
		{
			get
			{
				return this.cachedWillCollideNextCell;
			}
		}

		// Token: 0x17000F9C RID: 3996
		// (get) Token: 0x06006302 RID: 25346 RVA: 0x001F30DC File Offset: 0x001F12DC
		public IntVec3 LastPassableCellInPath
		{
			get
			{
				if (!this.Moving || this.curPath == null)
				{
					return IntVec3.Invalid;
				}
				if (!this.Destination.Cell.Impassable(this.pawn.Map))
				{
					return this.Destination.Cell;
				}
				List<IntVec3> nodesReversed = this.curPath.NodesReversed;
				for (int i = 0; i < nodesReversed.Count; i++)
				{
					if (!nodesReversed[i].Impassable(this.pawn.Map))
					{
						return nodesReversed[i];
					}
				}
				if (!this.pawn.Position.Impassable(this.pawn.Map))
				{
					return this.pawn.Position;
				}
				return IntVec3.Invalid;
			}
		}

		// Token: 0x06006303 RID: 25347 RVA: 0x001F319C File Offset: 0x001F139C
		public Pawn_PathFollower(Pawn newPawn)
		{
			this.pawn = newPawn;
		}

		// Token: 0x06006304 RID: 25348 RVA: 0x001F31F8 File Offset: 0x001F13F8
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.moving, "moving", true, false);
			Scribe_Values.Look<IntVec3>(ref this.nextCell, "nextCell", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.nextCellCostLeft, "nextCellCostLeft", 0f, false);
			Scribe_Values.Look<float>(ref this.nextCellCostTotal, "nextCellCostInitial", 0f, false);
			Scribe_Values.Look<PathEndMode>(ref this.peMode, "peMode", PathEndMode.None, false);
			Scribe_Values.Look<int>(ref this.cellsUntilClamor, "cellsUntilClamor", 0, false);
			Scribe_Values.Look<int>(ref this.lastEnteredCellTick, "lastEnteredCellTick", -999999, false);
			Scribe_Values.Look<int>(ref this.lastMovedTick, "lastMovedTick", -999999, false);
			Scribe_Values.Look<bool>(ref this.debugDisabled, "debugDisabled", false, false);
			Scribe_Values.Look<bool>(ref this.curPathJobIsStale, "curPathJobIsStale", false, false);
			if (this.moving)
			{
				Scribe_TargetInfo.Look(ref this.destination, "destination");
			}
		}

		// Token: 0x06006305 RID: 25349 RVA: 0x001F32EC File Offset: 0x001F14EC
		public void StartPath(LocalTargetInfo dest, PathEndMode peMode)
		{
			dest = (LocalTargetInfo)GenPath.ResolvePathMode(this.pawn, dest.ToTargetInfo(this.pawn.Map), ref peMode);
			if (dest.HasThing && dest.ThingDestroyed)
			{
				Log.Error(string.Format("{0} pathing to destroyed thing {1} curJob={2}", this.pawn, dest.Thing, this.pawn.CurJob.ToStringSafe<Job>()));
				this.PatherFailed();
				return;
			}
			if (!this.PawnCanOccupy(this.pawn.Position) && !this.TryRecoverFromUnwalkablePosition(true))
			{
				return;
			}
			if (this.moving && this.curPath != null && this.destination == dest && this.peMode == peMode)
			{
				return;
			}
			if (!this.pawn.Map.reachability.CanReach(this.pawn.Position, dest, peMode, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false)))
			{
				this.PatherFailed();
				return;
			}
			this.peMode = peMode;
			this.destination = dest;
			if (!this.IsNextCellWalkable() || this.NextCellDoorToWaitForOrManuallyOpen() != null || Mathf.Approximately(this.nextCellCostLeft, this.nextCellCostTotal) || (this.nextCell != this.pawn.Position && this.WillCollideWithPawnAt(this.nextCell, false, false)))
			{
				this.ResetToCurrentPosition();
			}
			PawnDestinationReservationManager.PawnDestinationReservation pawnDestinationReservation = this.pawn.Map.pawnDestinationReservationManager.MostRecentReservationFor(this.pawn);
			if (pawnDestinationReservation != null && ((this.destination.HasThing && pawnDestinationReservation.target != this.destination.Cell) || (pawnDestinationReservation.job != this.pawn.CurJob && pawnDestinationReservation.target != this.destination.Cell)))
			{
				this.pawn.Map.pawnDestinationReservationManager.ObsoleteAllClaimedBy(this.pawn);
			}
			if (this.AtDestinationPosition())
			{
				this.PatherArrived();
				return;
			}
			if (this.pawn.Downed && !this.pawn.health.CanCrawl)
			{
				Log.Error(this.pawn.LabelCap + " tried to path while downed. This should never happen. curJob=" + this.pawn.CurJob.ToStringSafe<Job>());
				this.PatherFailed();
				return;
			}
			this.moving = true;
			this.pawn.jobs.posture = PawnPosture.Standing;
			this.cachedMovePercentage = 0f;
			this.cachedWillCollideNextCell = false;
			this.curPathJobIsStale = true;
			this.SetNewPathRequest();
		}

		// Token: 0x06006306 RID: 25350 RVA: 0x001F3560 File Offset: 0x001F1760
		public void StopDead()
		{
			this.DisposeAndClearCurPathRequest();
			this.DisposeAndClearCurPath();
			this.moving = false;
			this.nextCell = this.pawn.Position;
			this.cachedMovePercentage = 0f;
			this.nextCellCostLeft = 0f;
			this.nextCellCostTotal = 1f;
			this.cachedWillCollideNextCell = false;
			this.curPathJobIsStale = false;
		}

		// Token: 0x06006307 RID: 25351 RVA: 0x001F35C0 File Offset: 0x001F17C0
		public void PatherTick()
		{
			if (this.pawn.RaceProps.doesntMove || this.debugDisabled)
			{
				return;
			}
			this.TrySetMovePercent();
			if (this.WillCollideWithPawnAt(this.pawn.Position, true, true))
			{
				if (this.FailedToFindCloseUnoccupiedCellRecently())
				{
					return;
				}
				IntVec3 intVec;
				if (CellFinder.TryFindBestPawnStandCell(this.pawn, out intVec, true) && intVec != this.pawn.Position)
				{
					if (DebugViewSettings.drawPatherState)
					{
						MoteMaker.ThrowText(this.pawn.DrawPos, this.pawn.Map, "Unstuck", -1f);
					}
					this.pawn.Position = intVec;
					this.ResetToCurrentPosition();
					if (this.moving)
					{
						this.SetNewPathRequest();
						return;
					}
				}
				else
				{
					this.failedToFindCloseUnoccupiedCellTicks = Find.TickManager.TicksGame;
				}
				return;
			}
			else
			{
				if (this.pawn.stances.FullBodyBusy)
				{
					return;
				}
				if (this.moving)
				{
					if (this.AtDestinationPosition())
					{
						this.PatherArrived();
						return;
					}
					PawnPath pawnPath;
					if (this.curPathRequest != null && this.curPathRequest.TryGetPath(out pawnPath))
					{
						this.curPathJobIsStale = false;
						bool? found = this.curPathRequest.Found;
						bool flag = false;
						if (found.GetValueOrDefault() == flag & found != null)
						{
							this.PatherFailed();
						}
						else if (!pawnPath.Found)
						{
							Log.Error(string.Format("{0} got an invalid path from path finder (maybe reachability and path finder state are out of sync).", this.pawn));
							pawnPath.Dispose();
							this.PatherFailed();
						}
						else
						{
							if (this.debugLog)
							{
								Log.Message(string.Format("{0}: Claimed new path: {1}", this.pawn, pawnPath));
							}
							this.DisposeAndClearCurPath();
							this.curPath = pawnPath;
							this.curPathRequest.ClaimCalculatedPath();
							this.DisposeAndClearCurPathRequest();
							List<IntVec3> list = this.curPath.PeekNextCells(20, 0);
							for (int i = 0; i < list.Count; i++)
							{
								if (PawnUtility.KnownDangerAt(list[i], this.pawn.Map, this.pawn))
								{
									this.foundPathWithDanger = GenTicks.TicksGame;
									break;
								}
								if (this.WillCollideWithPawnAt(list[i], false, false))
								{
									this.foundPathWhichCollidesWithPawns = GenTicks.TicksGame;
								}
							}
							this.lastMovedTick = GenTicks.TicksGame;
							if (!this.nextCell.IsValid || this.pawn.Position == this.nextCell)
							{
								this.SetupMoveIntoNextCell();
							}
						}
					}
					else if ((this.destination.IsValid && this.curPathRequest == null && this.curPath == null && !this.pawn.CanReachImmediate(this.destination, this.peMode)) || (this.pawn.IsHashIntervalTick(30) && this.NeedNewPath()))
					{
						if (this.debugLog)
						{
							Log.Message(string.Format("{0}: Needs a new path (maybe path blocked), will create new path request", this.pawn));
						}
						this.SetNewPathRequest();
					}
				}
				if (this.moving && this.curPath == null)
				{
					return;
				}
				this.cachedWillCollideNextCell = this.WillCollideWithPawnAt(this.nextCell, false, false);
				if (this.moving && this.WillCollideNextCell)
				{
					PawnPath pawnPath2 = this.curPath;
					bool flag2 = pawnPath2 != null && pawnPath2.NodesLeftCount < 15;
					if (Find.TickManager.TicksGame - this.lastMovedTick >= 180)
					{
						Pawn pawn = PawnUtility.PawnBlockingPathAt(this.nextCell, this.pawn, false, false, false, false);
						if (pawn != null && this.pawn.HostileTo(pawn))
						{
							if (this.pawn.CanAttackWhenPathingBlocked && this.pawn.TryGetAttackVerb(pawn, false, false) != null)
							{
								Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, pawn);
								job.maxNumMeleeAttacks = 1;
								job.expiryInterval = 300;
								this.pawn.jobs.StartJob(job, JobCondition.Incompletable, null, false, true, null, null, false, false, null, false, true, false);
								return;
							}
							this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
							return;
						}
					}
					this.cachedMovePercentage = 1f - this.nextCellCostLeft / this.nextCellCostTotal;
					if (flag2 && !this.BestPathHadPawnsInTheWayRecently())
					{
						this.TrySetNewPathRequest();
					}
					return;
				}
				if (this.curPath != null && (this.nextCellCostLeft > 0f || this.moving))
				{
					this.curPathJobIsStale = false;
					if (this.nextCellCostLeft > 0f)
					{
						this.nextCellCostLeft -= this.CostToPayThisTick();
					}
					else
					{
						this.TryEnterNextPathCell();
					}
					this.lastMovedTick = Find.TickManager.TicksGame;
					this.TrySetMovePercent();
				}
				return;
			}
		}

		// Token: 0x06006308 RID: 25352 RVA: 0x001F3A54 File Offset: 0x001F1C54
		private void TrySetMovePercent()
		{
			if (this.moving && this.pawn.Spawned && this.BuildingBlockingNextPathCell() == null && this.NextCellDoorToWaitForOrManuallyOpen() == null)
			{
				this.cachedMovePercentage = Mathf.Clamp01(1f - this.nextCellCostLeft / this.nextCellCostTotal);
			}
		}

		// Token: 0x06006309 RID: 25353 RVA: 0x001F3AA4 File Offset: 0x001F1CA4
		public void DrawDebugGUI()
		{
			Rect adjustedScreenspaceRect = SilhouetteUtility.GetAdjustedScreenspaceRect(this.pawn, 0.025f);
			Color? color = null;
			if (this.WillCollideWithPawnAt(this.pawn.Position, true, false))
			{
				color = new Color?(new Color(0.14f, 0.93f, 1f, 0.5f));
			}
			else if (this.pawn.stances.FullBodyBusy)
			{
				color = new Color?(new Color(0.37f, 1f, 0.19f, 0.5f));
			}
			else if (this.Moving && this.WillCollideNextCell)
			{
				color = new Color?(new Color(1f, 0.71f, 0.22f, 0.5f));
			}
			if (color != null)
			{
				GUI.DrawTexture(adjustedScreenspaceRect, TexUI.DotHighlight, ScaleMode.ScaleToFit, true, 0f, color.Value, 0f, 0f);
			}
		}

		// Token: 0x0600630A RID: 25354 RVA: 0x001F3B91 File Offset: 0x001F1D91
		public void TryResumePathingAfterLoading()
		{
			if (this.moving)
			{
				if (this.destination.HasThing && this.destination.ThingDestroyed)
				{
					this.PatherFailed();
					return;
				}
				this.StartPath(this.destination, this.peMode);
			}
		}

		// Token: 0x0600630B RID: 25355 RVA: 0x001F3BCE File Offset: 0x001F1DCE
		public void Notify_Teleported_Int()
		{
			this.StopDead();
			this.ResetToCurrentPosition();
		}

		// Token: 0x0600630C RID: 25356 RVA: 0x001F3BDC File Offset: 0x001F1DDC
		public void ResetToCurrentPosition()
		{
			this.DisposeAndClearCurPathRequest();
			this.DisposeAndClearCurPath();
			this.nextCell = this.pawn.Position;
			this.nextCellCostLeft = 0f;
			this.nextCellCostTotal = 1f;
			this.cachedMovePercentage = 0f;
			this.lastEnteredCellTick = GenTicks.TicksGame;
			if (this.moving && this.destination.IsValid)
			{
				this.SetNewPathRequest();
			}
		}

		// Token: 0x0600630D RID: 25357 RVA: 0x001F3C50 File Offset: 0x001F1E50
		private bool PawnCanOccupy(IntVec3 c)
		{
			if (!c.WalkableBy(this.pawn.Map, this.pawn))
			{
				return false;
			}
			Building_Door door = c.GetDoor(this.pawn.Map);
			return door == null || door.PawnCanOpen(this.pawn) || door.Open;
		}

		// Token: 0x0600630E RID: 25358 RVA: 0x001F3CA8 File Offset: 0x001F1EA8
		private Building BuildingBlockingNextPathCell()
		{
			Building edifice = this.nextCell.GetEdifice(this.pawn.Map);
			if (edifice != null && edifice.BlocksPawn(this.pawn))
			{
				return edifice;
			}
			return null;
		}

		// Token: 0x0600630F RID: 25359 RVA: 0x001F3CE0 File Offset: 0x001F1EE0
		public void NotifyThingTransformed(Thing from, Thing to)
		{
			if (!this.destination.HasThing || this.destination.Thing != from)
			{
				return;
			}
			this.destination = new LocalTargetInfo(to);
		}

		// Token: 0x06006310 RID: 25360 RVA: 0x001F3D0A File Offset: 0x001F1F0A
		private bool IsNextCellWalkable()
		{
			return this.nextCell.WalkableBy(this.pawn.Map, this.pawn) && !this.WillCollideWithPawnAt(this.nextCell, false, false);
		}

		// Token: 0x06006311 RID: 25361 RVA: 0x001F3D40 File Offset: 0x001F1F40
		private bool WillCollideWithPawnAt(IntVec3 c, bool forceOnlyStanding = false, bool useId = false)
		{
			return PawnUtility.ShouldCollideWithPawns(this.pawn) && PawnUtility.AnyPawnBlockingPathAt(c, this.pawn, false, forceOnlyStanding || (this.pawn.IsShambler && !this.pawn.mindState.anyCloseHostilesRecently), false, useId);
		}

		// Token: 0x06006312 RID: 25362 RVA: 0x001F3D94 File Offset: 0x001F1F94
		public Building_Door NextCellDoorToWaitForOrManuallyOpen()
		{
			Building_Door door = this.nextCell.GetDoor(this.pawn.Map);
			if (door != null && door.SlowsPawns && (!door.Open || door.TicksTillFullyOpened > 0) && door.PawnCanOpen(this.pawn))
			{
				return door;
			}
			return null;
		}

		// Token: 0x06006313 RID: 25363 RVA: 0x001F3DE8 File Offset: 0x001F1FE8
		private Pawn RopeeWithStretchedRopeAtNextPathCell()
		{
			List<Pawn> ropees = this.pawn.roping.Ropees;
			for (int i = 0; i < ropees.Count; i++)
			{
				Pawn pawn = ropees[i];
				if (!pawn.Position.InHorDistOf(this.nextCell, 8f))
				{
					return pawn;
				}
			}
			return null;
		}

		// Token: 0x06006314 RID: 25364 RVA: 0x001F3E40 File Offset: 0x001F2040
		public void PatherDraw()
		{
			if (!Find.ScreenshotModeHandler.Active && DebugViewSettings.drawPaths && this.curPath != null && Find.Selector.IsSelected(this.pawn) && !this.curPathJobIsStale)
			{
				this.curPath.DrawPath(this.pawn);
			}
		}

		// Token: 0x06006315 RID: 25365 RVA: 0x001F3E93 File Offset: 0x001F2093
		public bool ChangedCellRecently(int ticks)
		{
			return Find.TickManager.TicksGame - this.lastEnteredCellTick <= ticks;
		}

		// Token: 0x06006316 RID: 25366 RVA: 0x001F3EAC File Offset: 0x001F20AC
		public bool MovedRecently(int ticks)
		{
			return Find.TickManager.TicksGame - this.lastMovedTick <= ticks;
		}

		// Token: 0x06006317 RID: 25367 RVA: 0x001F3EC8 File Offset: 0x001F20C8
		public bool TryRecoverFromUnwalkablePosition(bool error = true)
		{
			bool flag = false;
			int i = 0;
			while (i < GenRadial.RadialPattern.Length)
			{
				IntVec3 intVec = this.pawn.Position + GenRadial.RadialPattern[i];
				if (this.PawnCanOccupy(intVec))
				{
					if (intVec == this.pawn.Position)
					{
						return true;
					}
					if (error)
					{
						Log.Warning(string.Format("{0} on unwalkable cell {1}. Teleporting to {2}", this.pawn, this.pawn.Position, intVec));
					}
					this.pawn.Position = intVec;
					this.pawn.Notify_Teleported(true, false);
					flag = true;
					break;
				}
				else
				{
					i++;
				}
			}
			if (!flag)
			{
				this.pawn.Destroy(DestroyMode.Vanish);
				Log.Error(string.Format("{0} on unwalkable cell {1}. Could not find walkable position nearby. Destroyed.", this.pawn.ToStringSafe<Pawn>(), this.pawn.Position));
			}
			return flag;
		}

		// Token: 0x06006318 RID: 25368 RVA: 0x001F3FB0 File Offset: 0x001F21B0
		private void PatherArrived()
		{
			if (this.debugLog)
			{
				Log.Message(string.Format("{0}: Pather arrived", this.pawn));
			}
			this.StopDead();
			if (this.pawn.jobs.curJob != null && !this.curPathJobIsStale)
			{
				this.pawn.jobs.curDriver.Notify_PatherArrived();
			}
		}

		// Token: 0x06006319 RID: 25369 RVA: 0x001F4010 File Offset: 0x001F2210
		private void PatherFailed()
		{
			if (this.debugLog)
			{
				Log.Message(string.Format("{0}: Pather failed", this.pawn));
			}
			this.StopDead();
			if (this.pawn.jobs.curJob != null && !this.curPathJobIsStale)
			{
				this.pawn.jobs.curDriver.Notify_PatherFailed();
			}
		}

		// Token: 0x0600631A RID: 25370 RVA: 0x001F4070 File Offset: 0x001F2270
		private void TryEnterNextPathCell()
		{
			Building building = this.BuildingBlockingNextPathCell();
			if (building != null)
			{
				Building_Door building_Door = building as Building_Door;
				if (building_Door == null || !building_Door.FreePassage)
				{
					if ((this.pawn.CurJob != null && this.pawn.CurJob.canBashDoors) || this.pawn.HostileTo(building))
					{
						this.MakeBashBlockerJob(building);
						return;
					}
					this.ResetToCurrentPosition();
					return;
				}
				else
				{
					if (building.def.IsFence && this.pawn.def.race.FenceBlocked && this.pawn.CurJob != null && this.pawn.CurJob.canBashFences)
					{
						this.MakeBashBlockerJob(building);
						return;
					}
					this.ResetToCurrentPosition();
					return;
				}
			}
			else
			{
				if (this.WillCollideNextCell)
				{
					return;
				}
				Building_Door building_Door2 = this.NextCellDoorToWaitForOrManuallyOpen();
				if (building_Door2 != null)
				{
					if (building_Door2.IsForbiddenToPass(this.pawn))
					{
						this.TrySetNewPathRequest();
						return;
					}
					if (!building_Door2.Open)
					{
						building_Door2.StartManualOpenBy(this.pawn);
					}
					Stance_Cooldown stance = new Stance_Cooldown(building_Door2.TicksTillFullyOpened, building_Door2, null)
					{
						neverAimWeapon = true
					};
					this.pawn.stances.SetStance(stance);
					building_Door2.CheckFriendlyTouched(this.pawn);
					return;
				}
				else
				{
					this.lastCell = this.pawn.Position;
					this.pawn.Position = this.nextCell;
					this.lastMoveDirection = (this.nextCell - this.lastCell).AngleFlat;
					this.lastEnteredCellTick = GenTicks.TicksGame;
					if (this.pawn.RaceProps.Humanlike)
					{
						this.cellsUntilClamor--;
						if (this.cellsUntilClamor <= 0)
						{
							GenClamor.DoClamor(this.pawn, 7f, ClamorDefOf.Movement);
							this.cellsUntilClamor = 12;
						}
					}
					this.pawn.filth.Notify_EnteredNewCell();
					if (this.pawn.BodySize > 0.9f)
					{
						this.pawn.Map.snowGrid.AddDepth(this.pawn.Position, -0.001f);
					}
					Building_Door door = this.lastCell.GetDoor(this.pawn.Map);
					if (door != null && !this.pawn.HostileTo(door))
					{
						door.CheckFriendlyTouched(this.pawn);
						if (!door.BlockedOpenMomentary && !door.HoldOpen && door.SlowsPawns && door.PawnCanOpen(this.pawn))
						{
							door.StartManualCloseBy(this.pawn);
							return;
						}
					}
					Pawn pawn = this.RopeeWithStretchedRopeAtNextPathCell();
					if (pawn != null)
					{
						Stance_Cooldown stance_Cooldown = new Stance_Cooldown(60, pawn, null);
						stance_Cooldown.neverAimWeapon = true;
						this.pawn.stances.SetStance(stance_Cooldown);
						return;
					}
					if (this.AtDestinationPosition())
					{
						this.PatherArrived();
						return;
					}
					if (this.curPath.NodesLeftCount <= 1)
					{
						this.TrySetNewPathRequest();
						return;
					}
					this.SetupMoveIntoNextCell();
					return;
				}
			}
		}

		// Token: 0x0600631B RID: 25371 RVA: 0x001F4344 File Offset: 0x001F2544
		private void MakeBashBlockerJob(Building blocker)
		{
			Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, blocker);
			job.expiryInterval = 300;
			this.pawn.jobs.StartJob(job, JobCondition.Incompletable, null, false, true, null, null, false, false, null, false, true, false);
		}

		// Token: 0x0600631C RID: 25372 RVA: 0x001F439C File Offset: 0x001F259C
		private void SetupMoveIntoNextCell()
		{
			if (this.curPath.NodesLeftCount <= 1)
			{
				Log.Error(string.Format("{0} at {1} ran out of path nodes while pathing to {2}.", this.pawn, this.pawn.Position, this.destination));
				this.PatherFailed();
				return;
			}
			IntVec3 a = this.nextCell;
			this.nextCell = this.curPath.ConsumeNextNode();
			if (a == this.nextCell)
			{
				if (this.curPath.NodesLeftCount <= 1)
				{
					if (this.AtDestinationPosition())
					{
						this.PatherArrived();
						return;
					}
					this.SetNewPathRequest();
					return;
				}
				else
				{
					this.nextCell = this.curPath.ConsumeNextNode();
				}
			}
			if (!this.nextCell.WalkableBy(this.pawn.Map, this.pawn))
			{
				this.TrySetNewPathRequest();
				return;
			}
			float moveCost = this.CostToMoveIntoCell(this.nextCell);
			this.nextCellCostTotal = moveCost;
			this.nextCellCostLeft = moveCost;
			this.cachedWillCollideNextCell = this.WillCollideWithPawnAt(this.nextCell, false, false);
			this.cachedMovePercentage = Mathf.Clamp01(1f - this.nextCellCostLeft / this.nextCellCostTotal);
			Building_Door door = this.nextCell.GetDoor(this.pawn.Map);
			if (door == null)
			{
				return;
			}
			door.Notify_PawnApproaching(this.pawn, moveCost);
		}

		// Token: 0x0600631D RID: 25373 RVA: 0x001F44E0 File Offset: 0x001F26E0
		private float CostToMoveIntoCell(IntVec3 c)
		{
			return Pawn_PathFollower.CostToMoveIntoCell(this.pawn, c);
		}

		// Token: 0x0600631E RID: 25374 RVA: 0x001F44F0 File Offset: 0x001F26F0
		public static int? GetPawnCellBaseCostOverride(Pawn pawn, IntVec3 c)
		{
			if (c.GetTerrain(pawn.Map).IsWater && pawn.WaterCellCost != null)
			{
				return pawn.WaterCellCost;
			}
			return null;
		}

		// Token: 0x0600631F RID: 25375 RVA: 0x001F4530 File Offset: 0x001F2730
		private static float CostToMoveIntoCell(Pawn pawn, IntVec3 c)
		{
			float num;
			if (c.x == pawn.Position.x || c.z == pawn.Position.z)
			{
				num = pawn.TicksPerMoveCardinal;
			}
			else
			{
				num = pawn.TicksPerMoveDiagonal;
			}
			int? pawnCellBaseCostOverride = Pawn_PathFollower.GetPawnCellBaseCostOverride(pawn, c);
			num += (float)pawn.Map.pathing.For(pawn).pathGrid.CalculatedCostAt(c, false, pawn.Position, pawnCellBaseCostOverride);
			Building edifice = c.GetEdifice(pawn.Map);
			if (edifice != null)
			{
				num += (float)edifice.PathWalkCostFor(pawn);
			}
			TerrainDef terrain = c.GetTerrain(pawn.Map);
			if (terrain.tags != null)
			{
				foreach (string key in terrain.tags)
				{
					float num2;
					if (pawn.kindDef.moveSpeedFactorByTerrainTag.TryGetValue(key, out num2))
					{
						num /= num2;
					}
				}
			}
			if (num > 450f)
			{
				num = 450f;
			}
			if (pawn.CurJob != null)
			{
				Pawn locomotionUrgencySameAs = pawn.jobs.curDriver.locomotionUrgencySameAs;
				if (locomotionUrgencySameAs != null && locomotionUrgencySameAs != pawn && locomotionUrgencySameAs.Spawned)
				{
					float num3 = Pawn_PathFollower.CostToMoveIntoCell(locomotionUrgencySameAs, c);
					if (num < num3)
					{
						num = num3;
					}
				}
				else
				{
					switch (pawn.jobs.curJob.locomotionUrgency)
					{
					case LocomotionUrgency.Amble:
						num *= 3f;
						if (num < 60f)
						{
							num = 60f;
						}
						break;
					case LocomotionUrgency.Walk:
						num *= 2f;
						if (num < 50f)
						{
							num = 50f;
						}
						break;
					case LocomotionUrgency.Jog:
						num *= 1f;
						break;
					case LocomotionUrgency.Sprint:
						num = (float)Mathf.RoundToInt(num * 0.75f);
						break;
					}
				}
			}
			return Mathf.Max(num, 1f);
		}

		// Token: 0x06006320 RID: 25376 RVA: 0x001F4700 File Offset: 0x001F2900
		private float CostToPayThisTick()
		{
			float num = 1f;
			if (this.pawn.stances.stagger.Staggered)
			{
				num *= this.pawn.stances.stagger.StaggerMoveSpeedFactor;
			}
			if (this.pawn.Flying)
			{
				num *= this.pawn.RaceProps.flightSpeedFactor;
			}
			if (num < this.nextCellCostTotal / 450f)
			{
				num = this.nextCellCostTotal / 450f;
			}
			return num;
		}

		// Token: 0x06006321 RID: 25377 RVA: 0x001F477F File Offset: 0x001F297F
		private void TrySetNewPathRequest()
		{
			if (this.curPathRequest == null)
			{
				this.SetNewPathRequest();
			}
		}

		// Token: 0x06006322 RID: 25378 RVA: 0x001F478F File Offset: 0x001F298F
		private void SetNewPathRequest()
		{
			this.DisposeAndClearCurPathRequest();
			this.curPathRequest = this.GenerateNewPathRequest();
		}

		// Token: 0x06006323 RID: 25379 RVA: 0x001F47A4 File Offset: 0x001F29A4
		private PathRequest GenerateNewPathRequest()
		{
			if (this.debugLog)
			{
				Log.Message(string.Format("{0}: Created request for a new path to {1} with {2} from {3}", new object[]
				{
					this.pawn,
					this.destination,
					this.peMode,
					this.nextCell
				}));
			}
			this.lastPathedTargetPosition = this.destination.Cell;
			PathFinder pathFinder = this.pawn.Map.pathFinder;
			PathFinderCostTuning value = PathFinderCostTuning.For(this.pawn);
			PathRequest pathRequest = pathFinder.CreateRequest(this.nextCell, this.destination, null, this.pawn, new PathFinderCostTuning?(value), this.peMode, null);
			pathFinder.PushRequest(pathRequest);
			return pathRequest;
		}

		// Token: 0x06006324 RID: 25380 RVA: 0x001F4864 File Offset: 0x001F2A64
		public void DisposeAndClearCurPathRequest()
		{
			if (this.curPathRequest != null)
			{
				this.curPathRequest.Dispose();
			}
			this.curPathRequest = null;
		}

		// Token: 0x06006325 RID: 25381 RVA: 0x001F4880 File Offset: 0x001F2A80
		public void DisposeAndClearCurPath()
		{
			if (this.curPath != null)
			{
				this.curPath.Dispose();
			}
			this.curPath = null;
		}

		// Token: 0x06006326 RID: 25382 RVA: 0x001F489C File Offset: 0x001F2A9C
		private bool AtDestinationPosition()
		{
			return this.pawn.CanReachImmediate(this.destination, this.peMode);
		}

		// Token: 0x06006327 RID: 25383 RVA: 0x001F48B8 File Offset: 0x001F2AB8
		private bool NeedNewPath()
		{
			if (this.destination.IsValid && this.pawn.CanReachImmediate(this.destination, this.peMode))
			{
				return false;
			}
			if (!this.destination.IsValid || this.curPath == null || !this.curPath.Found || this.curPath.NodesLeftCount == 0)
			{
				return true;
			}
			if (this.destination.HasThing && this.destination.Thing.Map != this.pawn.Map)
			{
				return true;
			}
			if ((this.pawn.Position.InHorDistOf(this.curPath.LastNode, 15f) || this.pawn.Position.InHorDistOf(this.destination.Cell, 15f)) && !ReachabilityImmediate.CanReachImmediate(this.curPath.LastNode, this.destination, this.pawn.Map, this.peMode, this.pawn))
			{
				return true;
			}
			if (this.curPath.UsedRegionHeuristics && this.curPath.NodesConsumedCount >= 75)
			{
				return true;
			}
			if (this.lastPathedTargetPosition != this.destination.Cell)
			{
				float num = (float)(this.pawn.Position - this.destination.Cell).LengthHorizontalSquared;
				float num2;
				if (num > 900f)
				{
					num2 = 10f;
				}
				else if (num > 289f)
				{
					num2 = 5f;
				}
				else if (num > 100f)
				{
					num2 = 3f;
				}
				else if (num > 49f)
				{
					num2 = 2f;
				}
				else
				{
					num2 = 0.5f;
				}
				if ((float)(this.lastPathedTargetPosition - this.destination.Cell).LengthHorizontalSquared > num2 * num2)
				{
					return true;
				}
			}
			PathingContext pc = this.pawn.Map.pathing.For(this.pawn);
			bool canBashFences = this.pawn.CurJob != null && this.pawn.CurJob.canBashFences;
			IntVec3 intVec = IntVec3.Invalid;
			int num3 = 0;
			while (num3 < 20 && num3 < this.curPath.NodesLeftCount)
			{
				IntVec3 intVec2 = this.curPath.Peek(num3);
				if (!intVec2.WalkableBy(this.pawn.Map, this.pawn))
				{
					return true;
				}
				if (!this.BestPathHadPawnsInTheWayRecently() && this.WillCollideWithPawnAt(intVec2, false, false))
				{
					if (!this.ChangedCellRecently((int)this.nextCellCostTotal + 10))
					{
						this.ResetToCurrentPosition();
					}
					this.foundPathWhichCollidesWithPawns = GenTicks.TicksGame;
					return true;
				}
				if (!this.BestPathHadDangerRecently() && PawnUtility.KnownDangerAt(intVec2, this.pawn.Map, this.pawn))
				{
					return true;
				}
				Building_Door building_Door = intVec2.GetEdifice(this.pawn.Map) as Building_Door;
				if (building_Door != null)
				{
					if (building_Door.BlocksPawn(this.pawn) && !building_Door.CanPhysicallyPass(this.pawn) && !this.pawn.HostileTo(building_Door))
					{
						return true;
					}
					if (building_Door.IsForbiddenToPass(this.pawn))
					{
						return true;
					}
				}
				if (num3 != 0 && intVec2.AdjacentToDiagonal(intVec) && (PathUtility.BlocksDiagonalMovement(intVec2.x, intVec.z, pc, canBashFences) || PathUtility.BlocksDiagonalMovement(intVec.x, intVec2.z, pc, canBashFences)))
				{
					return true;
				}
				intVec = intVec2;
				num3++;
			}
			return false;
		}

		// Token: 0x06006328 RID: 25384 RVA: 0x001F4C2C File Offset: 0x001F2E2C
		public bool BestPathHadPawnsInTheWayRecently()
		{
			int num = this.pawn.IsPlayerControlled ? 15 : 100;
			return this.foundPathWhichCollidesWithPawns + num > Find.TickManager.TicksGame;
		}

		// Token: 0x06006329 RID: 25385 RVA: 0x001F4C61 File Offset: 0x001F2E61
		private bool BestPathHadDangerRecently()
		{
			return this.foundPathWithDanger + 240 > Find.TickManager.TicksGame;
		}

		// Token: 0x0600632A RID: 25386 RVA: 0x001F4C7B File Offset: 0x001F2E7B
		private bool FailedToFindCloseUnoccupiedCellRecently()
		{
			return this.failedToFindCloseUnoccupiedCellTicks + 100 > Find.TickManager.TicksGame;
		}

		// Token: 0x040046B6 RID: 18102
		protected Pawn pawn;

		// Token: 0x040046B7 RID: 18103
		private bool moving;

		// Token: 0x040046B8 RID: 18104
		public IntVec3 nextCell;

		// Token: 0x040046B9 RID: 18105
		private IntVec3 lastCell;

		// Token: 0x040046BA RID: 18106
		public float lastMoveDirection;

		// Token: 0x040046BB RID: 18107
		public float nextCellCostLeft;

		// Token: 0x040046BC RID: 18108
		public float nextCellCostTotal = 1f;

		// Token: 0x040046BD RID: 18109
		private int cellsUntilClamor;

		// Token: 0x040046BE RID: 18110
		private int lastEnteredCellTick = -999999;

		// Token: 0x040046BF RID: 18111
		private int lastMovedTick = -999999;

		// Token: 0x040046C0 RID: 18112
		public bool debugDisabled;

		// Token: 0x040046C1 RID: 18113
		public bool curPathJobIsStale;

		// Token: 0x040046C2 RID: 18114
		private LocalTargetInfo destination;

		// Token: 0x040046C3 RID: 18115
		private PathEndMode peMode;

		// Token: 0x040046C4 RID: 18116
		public PathRequest curPathRequest;

		// Token: 0x040046C5 RID: 18117
		public PawnPath curPath;

		// Token: 0x040046C6 RID: 18118
		public IntVec3 lastPathedTargetPosition;

		// Token: 0x040046C7 RID: 18119
		private int foundPathWhichCollidesWithPawns = -999999;

		// Token: 0x040046C8 RID: 18120
		private int foundPathWithDanger = -999999;

		// Token: 0x040046C9 RID: 18121
		private int failedToFindCloseUnoccupiedCellTicks = -999999;

		// Token: 0x040046CA RID: 18122
		private float cachedMovePercentage;

		// Token: 0x040046CB RID: 18123
		private bool cachedWillCollideNextCell;

		// Token: 0x040046CC RID: 18124
		public bool debugLog;

		// Token: 0x040046CD RID: 18125
		private const int MaxMoveTicks = 450;

		// Token: 0x040046CE RID: 18126
		private const int MaxCheckAheadNodes = 20;

		// Token: 0x040046CF RID: 18127
		private const int CheckNeedNewPathIntervalTicks = 30;

		// Token: 0x040046D0 RID: 18128
		private const float SnowReductionFromWalking = 0.001f;

		// Token: 0x040046D1 RID: 18129
		private const int ClamorCellsInterval = 12;

		// Token: 0x040046D2 RID: 18130
		private const int MinCostWalk = 50;

		// Token: 0x040046D3 RID: 18131
		private const int MinCostAmble = 60;

		// Token: 0x040046D4 RID: 18132
		private const int CheckForMovingCollidingPawnsIfCloserToTargetThanX = 15;

		// Token: 0x040046D5 RID: 18133
		private const int AttackBlockingHostilePawnAfterTicks = 180;

		// Token: 0x040046D6 RID: 18134
		private const int WaitForRopeeTicks = 60;

		// Token: 0x040046D7 RID: 18135
		private const float RopeLength = 8f;
	}
}
