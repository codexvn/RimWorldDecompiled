using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D71 RID: 15729
	public class Caravan_PathFollower : IExposable
	{
		// Token: 0x17003559 RID: 13657
		// (get) Token: 0x060156C9 RID: 87753 RVA: 0x00669421 File Offset: 0x00667621
		public PlanetTile Destination
		{
			get
			{
				return this.destTile;
			}
		}

		// Token: 0x1700355A RID: 13658
		// (get) Token: 0x060156CA RID: 87754 RVA: 0x00669429 File Offset: 0x00667629
		public bool Moving
		{
			get
			{
				return this.moving && this.caravan.Spawned;
			}
		}

		// Token: 0x1700355B RID: 13659
		// (get) Token: 0x060156CB RID: 87755 RVA: 0x00669440 File Offset: 0x00667640
		public bool MovingNow
		{
			get
			{
				return this.Moving && !this.Paused && !this.caravan.CantMove;
			}
		}

		// Token: 0x1700355C RID: 13660
		// (get) Token: 0x060156CC RID: 87756 RVA: 0x00669462 File Offset: 0x00667662
		public CaravanArrivalAction ArrivalAction
		{
			get
			{
				if (!this.Moving)
				{
					return null;
				}
				return this.arrivalAction;
			}
		}

		// Token: 0x1700355D RID: 13661
		// (get) Token: 0x060156CD RID: 87757 RVA: 0x00669474 File Offset: 0x00667674
		// (set) Token: 0x060156CE RID: 87758 RVA: 0x00669488 File Offset: 0x00667688
		public bool Paused
		{
			get
			{
				return this.Moving && this.paused;
			}
			set
			{
				if (value == this.paused)
				{
					return;
				}
				if (!value)
				{
					this.paused = false;
				}
				else if (!this.Moving)
				{
					Log.Error("Tried to pause caravan movement of " + this.caravan.ToStringSafe<Caravan>() + " but it's not moving.");
				}
				else
				{
					this.paused = true;
				}
				this.caravan.Notify_DestinationOrPauseStatusChanged();
			}
		}

		// Token: 0x060156CF RID: 87759 RVA: 0x006694E6 File Offset: 0x006676E6
		public Caravan_PathFollower(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x060156D0 RID: 87760 RVA: 0x00669518 File Offset: 0x00667718
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.moving, "moving", true, false);
			Scribe_Values.Look<bool>(ref this.paused, "paused", false, false);
			Scribe_Values.Look<PlanetTile>(ref this.nextTile, "nextTile", default(PlanetTile), false);
			Scribe_Values.Look<PlanetTile>(ref this.previousTileForDrawingIfInDoubt, "previousTileForDrawingIfInDoubt", default(PlanetTile), false);
			Scribe_Values.Look<float>(ref this.nextTileCostLeft, "nextTileCostLeft", 0f, false);
			Scribe_Values.Look<float>(ref this.nextTileCostTotal, "nextTileCostTotal", 0f, false);
			Scribe_Values.Look<PlanetTile>(ref this.destTile, "destTile", default(PlanetTile), false);
			Scribe_Deep.Look<CaravanArrivalAction>(ref this.arrivalAction, "arrivalAction", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && Current.ProgramState != ProgramState.Entry && this.moving && !this.StartPath(this.destTile, this.arrivalAction, true, false))
			{
				this.StopDead();
			}
		}

		// Token: 0x060156D1 RID: 87761 RVA: 0x0066960C File Offset: 0x0066780C
		public bool StartPath(PlanetTile destTile, CaravanArrivalAction arrivalAction, bool repathImmediately = false, bool resetPauseStatus = true)
		{
			this.caravan.autoJoinable = false;
			if (resetPauseStatus)
			{
				this.paused = false;
			}
			if (arrivalAction != null && !arrivalAction.StillValid(this.caravan, destTile))
			{
				return false;
			}
			if (!this.IsPassable(this.caravan.Tile) && !this.TryRecoverFromUnwalkablePosition())
			{
				return false;
			}
			if (this.moving && this.curPath != null && this.destTile == destTile)
			{
				this.arrivalAction = arrivalAction;
				return true;
			}
			if (!this.caravan.CanReach(destTile))
			{
				this.PatherFailed();
				return false;
			}
			this.destTile = destTile;
			this.arrivalAction = arrivalAction;
			this.caravan.Notify_DestinationOrPauseStatusChanged();
			if (!this.nextTile.Valid || !this.IsNextTilePassable())
			{
				this.nextTile = this.caravan.Tile;
				this.nextTileCostLeft = 0f;
				this.previousTileForDrawingIfInDoubt = PlanetTile.Invalid;
			}
			if (this.AtDestinationPosition())
			{
				this.PatherArrived();
				return true;
			}
			if (this.curPath != null)
			{
				this.curPath.ReleaseToPool();
			}
			this.curPath = null;
			this.moving = true;
			if (repathImmediately && this.TrySetNewPath() && this.nextTileCostLeft <= 0f && this.moving)
			{
				this.TryEnterNextPathTile();
			}
			return true;
		}

		// Token: 0x060156D2 RID: 87762 RVA: 0x00669750 File Offset: 0x00667950
		public void StopDead()
		{
			if (this.curPath != null)
			{
				this.curPath.ReleaseToPool();
			}
			this.curPath = null;
			this.moving = false;
			this.paused = false;
			this.nextTile = this.caravan.Tile;
			this.previousTileForDrawingIfInDoubt = PlanetTile.Invalid;
			this.arrivalAction = null;
			this.nextTileCostLeft = 0f;
			this.caravan.Notify_DestinationOrPauseStatusChanged();
		}

		// Token: 0x060156D3 RID: 87763 RVA: 0x006697C0 File Offset: 0x006679C0
		public void PatherTickInterval(int delta)
		{
			if (this.moving && this.arrivalAction != null && !this.arrivalAction.StillValid(this.caravan, this.Destination))
			{
				string failMessage = this.arrivalAction.StillValid(this.caravan, this.Destination).FailMessage;
				Messages.Message("MessageCaravanArrivalActionNoLongerValid".Translate(this.caravan.Name).CapitalizeFirst() + ((failMessage != null) ? (" " + failMessage) : ""), this.caravan, MessageTypeDefOf.NegativeEvent, true);
				this.StopDead();
			}
			if (this.caravan.CantMove || this.paused)
			{
				return;
			}
			if (this.nextTileCostLeft > 0f)
			{
				this.nextTileCostLeft -= this.CostToPayThisTick() * (float)delta;
				return;
			}
			if (this.moving)
			{
				this.TryEnterNextPathTile();
			}
		}

		// Token: 0x060156D4 RID: 87764 RVA: 0x006698C6 File Offset: 0x00667AC6
		public void Notify_Teleported_Int()
		{
			this.StopDead();
		}

		// Token: 0x060156D5 RID: 87765 RVA: 0x0065504E File Offset: 0x0065324E
		private bool IsPassable(PlanetTile tile)
		{
			return !Find.World.Impassable(tile);
		}

		// Token: 0x060156D6 RID: 87766 RVA: 0x006698CE File Offset: 0x00667ACE
		public bool IsNextTilePassable()
		{
			return this.IsPassable(this.nextTile);
		}

		// Token: 0x060156D7 RID: 87767 RVA: 0x006698DC File Offset: 0x00667ADC
		private bool TryRecoverFromUnwalkablePosition()
		{
			PlanetTile tile;
			if (GenWorldClosest.TryFindClosestTile(this.caravan.Tile, (PlanetTile t) => this.IsPassable(t), out tile, 2147483647, true))
			{
				string[] array = new string[5];
				int num = 0;
				Caravan caravan = this.caravan;
				array[num] = ((caravan != null) ? caravan.ToString() : null);
				array[1] = " on unwalkable tile ";
				array[2] = this.caravan.Tile.ToString();
				array[3] = ". Teleporting to ";
				array[4] = tile.ToString();
				Log.Warning(string.Concat(array));
				this.caravan.Tile = tile;
				this.caravan.Notify_Teleported();
				return true;
			}
			Caravan caravan2 = this.caravan;
			Log.Error(((caravan2 != null) ? caravan2.ToString() : null) + " on unwalkable tile " + this.caravan.Tile.ToString() + ". Could not find walkable position nearby. Removed.");
			this.caravan.Destroy();
			return false;
		}

		// Token: 0x060156D8 RID: 87768 RVA: 0x006699D4 File Offset: 0x00667BD4
		private void PatherArrived()
		{
			CaravanArrivalAction caravanArrivalAction = this.arrivalAction;
			this.StopDead();
			if (caravanArrivalAction != null && caravanArrivalAction.StillValid(this.caravan, this.caravan.Tile))
			{
				caravanArrivalAction.Arrived(this.caravan);
				return;
			}
			if (this.caravan.IsPlayerControlled && !this.caravan.VisibleToCameraNow())
			{
				Messages.Message("MessageCaravanArrivedAtDestination".Translate(this.caravan.Label), this.caravan, MessageTypeDefOf.TaskCompletion, true);
			}
		}

		// Token: 0x060156D9 RID: 87769 RVA: 0x006698C6 File Offset: 0x00667AC6
		private void PatherFailed()
		{
			this.StopDead();
		}

		// Token: 0x060156DA RID: 87770 RVA: 0x00669A6C File Offset: 0x00667C6C
		private void TryEnterNextPathTile()
		{
			if (!this.IsNextTilePassable())
			{
				this.PatherFailed();
				return;
			}
			this.caravan.Tile = this.nextTile;
			if (this.NeedNewPath() && !this.TrySetNewPath())
			{
				return;
			}
			if (this.AtDestinationPosition())
			{
				this.PatherArrived();
				return;
			}
			if (this.curPath.NodesLeftCount == 0)
			{
				Caravan caravan = this.caravan;
				Log.Error(((caravan != null) ? caravan.ToString() : null) + " ran out of path nodes. Force-arriving.");
				this.PatherArrived();
				return;
			}
			this.SetupMoveIntoNextTile();
		}

		// Token: 0x060156DB RID: 87771 RVA: 0x00669AF4 File Offset: 0x00667CF4
		private void SetupMoveIntoNextTile()
		{
			if (this.curPath.NodesLeftCount < 2)
			{
				string[] array = new string[6];
				int num = 0;
				Caravan caravan = this.caravan;
				array[num] = ((caravan != null) ? caravan.ToString() : null);
				array[1] = " at ";
				array[2] = this.caravan.Tile.ToString();
				array[3] = " ran out of path nodes while pathing to ";
				array[4] = this.destTile.ToString();
				array[5] = ".";
				Log.Error(string.Concat(array));
				this.PatherFailed();
				return;
			}
			this.nextTile = this.curPath.ConsumeNextNode();
			this.previousTileForDrawingIfInDoubt = PlanetTile.Invalid;
			if (Find.World.Impassable(this.nextTile))
			{
				Caravan caravan2 = this.caravan;
				Log.Error(((caravan2 != null) ? caravan2.ToString() : null) + " entering " + this.nextTile.ToString() + " which is unwalkable.");
			}
			int num2 = this.CostToMove(this.caravan.Tile, this.nextTile);
			this.nextTileCostTotal = (float)num2;
			this.nextTileCostLeft = (float)num2;
		}

		// Token: 0x060156DC RID: 87772 RVA: 0x00669C10 File Offset: 0x00667E10
		private int CostToMove(PlanetTile start, PlanetTile end)
		{
			return Caravan_PathFollower.CostToMove(this.caravan, start, end, null);
		}

		// Token: 0x060156DD RID: 87773 RVA: 0x00669C33 File Offset: 0x00667E33
		public static int CostToMove(Caravan caravan, PlanetTile start, PlanetTile end, int? ticksAbs = null)
		{
			return Caravan_PathFollower.CostToMove(caravan.TicksPerMove, start, end, ticksAbs, false, null, null, caravan.ImmobilizedByMass);
		}

		// Token: 0x060156DE RID: 87774 RVA: 0x00669C4C File Offset: 0x00667E4C
		public static int CostToMove(int caravanTicksPerMove, PlanetTile start, PlanetTile end, int? ticksAbs = null, bool perceivedStatic = false, StringBuilder explanation = null, string caravanTicksPerMoveExplanation = null, bool immobile = false)
		{
			if (start == end)
			{
				return 0;
			}
			if (explanation != null)
			{
				explanation.Append(caravanTicksPerMoveExplanation);
				explanation.AppendLine();
			}
			StringBuilder stringBuilder = (explanation != null) ? new StringBuilder() : null;
			float num;
			if (perceivedStatic && explanation == null)
			{
				num = Find.WorldPathGrid.PerceivedMovementDifficultyAt(end);
			}
			else
			{
				num = WorldPathGrid.CalculatedMovementDifficultyAt(end, perceivedStatic, ticksAbs, stringBuilder);
			}
			float roadMovementDifficultyMultiplier = Find.WorldGrid.GetRoadMovementDifficultyMultiplier(start, end, stringBuilder);
			if (explanation != null && !immobile)
			{
				explanation.AppendLine();
				explanation.Append("TileMovementDifficulty".Translate() + ":");
				explanation.AppendLine();
				explanation.Append(stringBuilder.ToString().Indented("  "));
				explanation.AppendLine();
				explanation.Append("  = " + (num * roadMovementDifficultyMultiplier).ToString("0.#"));
			}
			int num2 = (int)((float)caravanTicksPerMove * num * roadMovementDifficultyMultiplier);
			num2 = Mathf.Clamp(num2, 1, 30000);
			if (explanation != null)
			{
				explanation.AppendLine();
				if (immobile)
				{
					explanation.Append("EncumberedCaravanTilesPerDayTip".Translate());
				}
				else
				{
					explanation.AppendLine();
					explanation.Append("FinalCaravanMovementSpeed".Translate() + ":");
					int num3 = Mathf.CeilToInt((float)num2 / 1f);
					explanation.AppendLine();
					explanation.Append(string.Concat(new string[]
					{
						"  ",
						(60000f / (float)caravanTicksPerMove).ToString("0.#"),
						" / ",
						(num * roadMovementDifficultyMultiplier).ToString("0.#"),
						" = ",
						(60000f / (float)num3).ToString("0.#"),
						" "
					}) + "TilesPerDay".Translate());
				}
			}
			return num2;
		}

		// Token: 0x060156DF RID: 87775 RVA: 0x00669E48 File Offset: 0x00668048
		public static bool IsValidFinalPushDestination(PlanetTile tile)
		{
			List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
			for (int i = 0; i < allWorldObjects.Count; i++)
			{
				if (allWorldObjects[i].Tile == tile && !(allWorldObjects[i] is Caravan))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060156E0 RID: 87776 RVA: 0x00669E98 File Offset: 0x00668098
		private float CostToPayThisTick()
		{
			float num = 1f;
			if (DebugSettings.fastCaravans)
			{
				num = 100f;
			}
			if (num < this.nextTileCostTotal / 30000f)
			{
				num = this.nextTileCostTotal / 30000f;
			}
			return num;
		}

		// Token: 0x060156E1 RID: 87777 RVA: 0x00669ED8 File Offset: 0x006680D8
		private bool TrySetNewPath()
		{
			WorldPath worldPath = this.GenerateNewPath();
			if (!worldPath.Found)
			{
				this.PatherFailed();
				return false;
			}
			if (this.curPath != null)
			{
				this.curPath.ReleaseToPool();
			}
			this.curPath = worldPath;
			return true;
		}

		// Token: 0x060156E2 RID: 87778 RVA: 0x00669F18 File Offset: 0x00668118
		private WorldPath GenerateNewPath()
		{
			PlanetTile planetTile = (this.moving && this.nextTile.Valid && this.IsNextTilePassable()) ? this.nextTile : this.caravan.Tile;
			this.lastPathedTargetTile = this.destTile;
			WorldPath worldPath = planetTile.Layer.Pather.FindPath(planetTile, this.destTile, this.caravan, null);
			if (worldPath.Found && planetTile != this.caravan.Tile)
			{
				if (worldPath.NodesLeftCount >= 2 && worldPath.Peek(1) == this.caravan.Tile)
				{
					worldPath.ConsumeNextNode();
					if (this.moving)
					{
						this.previousTileForDrawingIfInDoubt = this.nextTile;
						this.nextTile = this.caravan.Tile;
						this.nextTileCostLeft = this.nextTileCostTotal - this.nextTileCostLeft;
					}
				}
				else
				{
					worldPath.AddNodeAtStart(this.caravan.Tile);
				}
			}
			return worldPath;
		}

		// Token: 0x060156E3 RID: 87779 RVA: 0x0066A015 File Offset: 0x00668215
		private bool AtDestinationPosition()
		{
			return this.caravan.Tile == this.destTile;
		}

		// Token: 0x060156E4 RID: 87780 RVA: 0x0066A030 File Offset: 0x00668230
		private bool NeedNewPath()
		{
			if (!this.moving)
			{
				return false;
			}
			if (this.curPath == null || !this.curPath.Found || this.curPath.NodesLeftCount == 0)
			{
				return true;
			}
			int num = 0;
			while (num < 20 && num < this.curPath.NodesLeftCount)
			{
				PlanetTile tileID = this.curPath.Peek(num);
				if (Find.World.Impassable(tileID))
				{
					return true;
				}
				num++;
			}
			return false;
		}

		// Token: 0x0400EE74 RID: 61044
		private Caravan caravan;

		// Token: 0x0400EE75 RID: 61045
		private bool moving;

		// Token: 0x0400EE76 RID: 61046
		private bool paused;

		// Token: 0x0400EE77 RID: 61047
		public PlanetTile nextTile = PlanetTile.Invalid;

		// Token: 0x0400EE78 RID: 61048
		public PlanetTile previousTileForDrawingIfInDoubt = PlanetTile.Invalid;

		// Token: 0x0400EE79 RID: 61049
		public float nextTileCostLeft;

		// Token: 0x0400EE7A RID: 61050
		public float nextTileCostTotal = 1f;

		// Token: 0x0400EE7B RID: 61051
		private PlanetTile destTile;

		// Token: 0x0400EE7C RID: 61052
		private CaravanArrivalAction arrivalAction;

		// Token: 0x0400EE7D RID: 61053
		public WorldPath curPath;

		// Token: 0x0400EE7E RID: 61054
		public PlanetTile lastPathedTargetTile;

		// Token: 0x0400EE7F RID: 61055
		public const int MaxMoveTicks = 30000;

		// Token: 0x0400EE80 RID: 61056
		private const int MaxCheckAheadNodes = 20;

		// Token: 0x0400EE81 RID: 61057
		private const int MinCostWalk = 50;

		// Token: 0x0400EE82 RID: 61058
		private const int MinCostAmble = 60;

		// Token: 0x0400EE83 RID: 61059
		public const float DefaultPathCostToPayPerTick = 1f;

		// Token: 0x0400EE84 RID: 61060
		public const int FinalNoRestPushMaxDurationTicks = 10000;
	}
}
