using System;
using System.Collections.Generic;
using RimWorld;
using Unity.Collections;

namespace Verse
{
	// Token: 0x02000410 RID: 1040
	public sealed class FogGrid : IExposable, IDisposable
	{
		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06001A50 RID: 6736 RVA: 0x00084FD5 File Offset: 0x000831D5
		internal NativeBitArray FogGrid_Unsafe
		{
			get
			{
				return this.fogGrid;
			}
		}

		// Token: 0x06001A51 RID: 6737 RVA: 0x00084FDD File Offset: 0x000831DD
		public FogGrid(Map map)
		{
			this.map = map;
			this.fogGrid = new NativeBitArray(map.cellIndices.NumGridCells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x06001A52 RID: 6738 RVA: 0x00085009 File Offset: 0x00083209
		public void ExposeData()
		{
			DataExposeUtility.LookBitArray(ref this.fogGrid, this.map.Area, "fogGrid");
		}

		// Token: 0x06001A53 RID: 6739 RVA: 0x00085028 File Offset: 0x00083228
		public void Unfog(IntVec3 c)
		{
			this.UnfogWorker(c);
			List<Thing> thingList = c.GetThingList(this.map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (thing.def.Fillage == FillCategory.Full)
				{
					foreach (IntVec3 c2 in thing.OccupiedRect().Cells)
					{
						this.UnfogWorker(c2);
					}
				}
				thingList[i].Notify_Unfogged();
			}
			this.map.events.Notify_CellFogChanged(c, false);
		}

		// Token: 0x06001A54 RID: 6740 RVA: 0x000850DC File Offset: 0x000832DC
		private void UnfogWorker(IntVec3 c)
		{
			int pos = this.map.cellIndices.CellToIndex(c);
			if (!this.fogGrid.IsSet(pos))
			{
				return;
			}
			this.fogGrid.Set(pos, false);
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.FogOfWar | MapMeshFlagDefOf.Things);
			}
			Designation designation = this.map.designationManager.DesignationAt(c, DesignationDefOf.Mine);
			if (designation != null && c.GetFirstMineable(this.map) == null)
			{
				designation.Delete();
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.map.roofGrid.Drawer.SetDirty();
				this.map.mapTemperature.Drawer.SetDirty();
			}
		}

		// Token: 0x06001A55 RID: 6741 RVA: 0x000851A5 File Offset: 0x000833A5
		public bool IsFogged(IntVec3 c)
		{
			return this.fogGrid.IsCreated && c.InBounds(this.map) && this.fogGrid.IsSet(this.map.cellIndices.CellToIndex(c));
		}

		// Token: 0x06001A56 RID: 6742 RVA: 0x000851E0 File Offset: 0x000833E0
		public bool IsFogged(int index)
		{
			return this.fogGrid.IsCreated && this.fogGrid.IsSet(index);
		}

		// Token: 0x06001A57 RID: 6743 RVA: 0x00085200 File Offset: 0x00083400
		public void ClearAllFog()
		{
			for (int i = 0; i < this.map.Size.x; i++)
			{
				for (int j = 0; j < this.map.Size.z; j++)
				{
					this.Unfog(new IntVec3(i, 0, j));
				}
			}
		}

		// Token: 0x06001A58 RID: 6744 RVA: 0x00085254 File Offset: 0x00083454
		public void Refog(CellRect rect)
		{
			foreach (IntVec3 intVec in rect)
			{
				if (intVec.InBounds(this.map))
				{
					int pos = this.map.cellIndices.CellToIndex(intVec);
					if (!this.fogGrid.IsSet(pos))
					{
						this.fogGrid.Set(pos, true);
						if (Current.ProgramState == ProgramState.Playing)
						{
							this.map.mapDrawer.MapMeshDirty(intVec, MapMeshFlagDefOf.FogOfWar | MapMeshFlagDefOf.Things);
						}
						this.map.events.Notify_CellFogChanged(intVec, true);
					}
				}
			}
		}

		// Token: 0x06001A59 RID: 6745 RVA: 0x00085320 File Offset: 0x00083520
		public void Notify_FogBlockerRemoved(Thing thing)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			bool flag = false;
			foreach (IntVec3 c in GenAdj.CellsAdjacent8Way(thing))
			{
				if (c.InBounds(this.map))
				{
					Building edifice = c.GetEdifice(this.map);
					if (!this.IsFogged(c) && ((edifice != null && edifice.def.IsDoor) || (edifice == null || !edifice.def.MakeFog)))
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				return;
			}
			this.FloodUnfogAdjacent(thing, !this.map.generatorDef.ignoreAreaRevealedLetter);
		}

		// Token: 0x06001A5A RID: 6746 RVA: 0x000853E0 File Offset: 0x000835E0
		public void Notify_PawnEnteringDoor(Building_Door door, Pawn pawn)
		{
			if (pawn.Faction != Faction.OfPlayer && pawn.HostFaction != Faction.OfPlayer)
			{
				return;
			}
			this.FloodUnfogAdjacent(door.Position, false);
		}

		// Token: 0x06001A5B RID: 6747 RVA: 0x0008540C File Offset: 0x0008360C
		internal void SetAllFogged()
		{
			CellIndices cellIndices = this.map.cellIndices;
			foreach (IntVec3 c in this.map.AllCells)
			{
				this.fogGrid.Set(cellIndices.CellToIndex(c), true);
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.map.roofGrid.Drawer.SetDirty();
			}
			this.map.events.Notify_MapFogged();
		}

		// Token: 0x06001A5C RID: 6748 RVA: 0x000854A4 File Offset: 0x000836A4
		public void FloodUnfogAdjacent(IntVec3 c, bool sendLetters = true)
		{
			this.Unfog(c);
			bool flag = false;
			FloodUnfogResult unfogResult = default(FloodUnfogResult);
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec = c + GenAdj.CardinalDirections[i];
				if (intVec.InBounds(this.map) && intVec.Fogged(this.map))
				{
					Building edifice = intVec.GetEdifice(this.map);
					if (edifice == null || !edifice.def.MakeFog)
					{
						flag = true;
						unfogResult = FloodFillerFog.FloodUnfog(intVec, this.map);
					}
					else
					{
						this.Unfog(intVec);
					}
				}
			}
			for (int j = 0; j < 8; j++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCells[j];
				if (c2.InBounds(this.map))
				{
					Building edifice2 = c2.GetEdifice(this.map);
					if (edifice2 != null && edifice2.def.MakeFog)
					{
						this.Unfog(c2);
					}
				}
			}
			if (flag && sendLetters)
			{
				this.NotifyAreaRevealed(c, unfogResult);
			}
		}

		// Token: 0x06001A5D RID: 6749 RVA: 0x000855A0 File Offset: 0x000837A0
		public void FloodUnfogAdjacent(Thing thing, bool sendLetters = true)
		{
			this.Unfog(thing.Position);
			bool flag = false;
			FloodUnfogResult unfogResult = default(FloodUnfogResult);
			foreach (IntVec3 intVec in GenAdj.CellsAdjacentCardinal(thing))
			{
				if (intVec.InBounds(this.map) && intVec.Fogged(this.map))
				{
					Building edifice = intVec.GetEdifice(this.map);
					if (edifice == null || !edifice.def.MakeFog)
					{
						flag = true;
						unfogResult = FloodFillerFog.FloodUnfog(intVec, this.map);
					}
					else
					{
						this.Unfog(intVec);
					}
				}
			}
			foreach (IntVec3 c in GenAdj.CellsAdjacent8Way(thing))
			{
				if (c.InBounds(this.map))
				{
					Building edifice2 = c.GetEdifice(this.map);
					if (edifice2 != null && edifice2.def.MakeFog)
					{
						this.Unfog(c);
					}
				}
			}
			if (flag && sendLetters)
			{
				this.NotifyAreaRevealed(thing.Position, unfogResult);
			}
		}

		// Token: 0x06001A5E RID: 6750 RVA: 0x000856D0 File Offset: 0x000838D0
		private void NotifyAreaRevealed(IntVec3 c, FloodUnfogResult unfogResult)
		{
			if (unfogResult.mechanoidFound)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelAreaRevealed".Translate(), "AreaRevealedWithMechanoids".Translate(), LetterDefOf.ThreatBig, new TargetInfo(c, this.map, false), null, null, null, null, 0, true);
				return;
			}
			if (!unfogResult.allOnScreen || unfogResult.cellsUnfogged >= 600)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelAreaRevealed".Translate(), "AreaRevealed".Translate(), LetterDefOf.NeutralEvent, new TargetInfo(c, this.map, false), null, null, null, null, 0, true);
			}
		}

		// Token: 0x06001A5F RID: 6751 RVA: 0x00085771 File Offset: 0x00083971
		public void Dispose()
		{
			this.fogGrid.Dispose();
		}

		// Token: 0x04001749 RID: 5961
		private readonly Map map;

		// Token: 0x0400174A RID: 5962
		private NativeBitArray fogGrid;

		// Token: 0x0400174B RID: 5963
		private const int AlwaysSendLetterIfUnfoggedMoreCellsThan = 600;
	}
}
