using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F77 RID: 8055
	public class LabyrinthMapComponent : CustomMapComponent
	{
		// Token: 0x0600B200 RID: 45568 RVA: 0x0032DD1F File Offset: 0x0032BF1F
		public LabyrinthMapComponent(Map map) : base(map)
		{
		}

		// Token: 0x0600B201 RID: 45569 RVA: 0x0032DD28 File Offset: 0x0032BF28
		public override void MapComponentTick()
		{
			if (!this.closing && GenTicks.IsTickInterval(300) && this.abductorObelisk.DestroyedOrNull() && !this.map.mapPawns.AnyColonistSpawned)
			{
				PocketMapUtility.DestroyPocketMap(this.map);
			}
			this.TeleportPawnsClosing();
		}

		// Token: 0x0600B202 RID: 45570 RVA: 0x0032DD7C File Offset: 0x0032BF7C
		private void TeleportPawnsClosing()
		{
			if (!this.closing || GenTicks.TicksGame < this.nextTeleportTick)
			{
				return;
			}
			Map dest = null;
			this.nextTeleportTick = GenTicks.TicksGame + LabyrinthMapComponent.TeleportDelayTicks.RandomInRange;
			foreach (Map map in Find.Maps)
			{
				if (map.IsPlayerHome)
				{
					dest = map;
					break;
				}
			}
			if (dest == null)
			{
				return;
			}
			IntVec3 cell;
			if (!CellFinderLoose.TryGetRandomCellWith((IntVec3 pos) => LabyrinthMapComponent.IsValidTeleportCell(pos, dest), dest, 1000, out cell))
			{
				return;
			}
			using (List<Pawn>.Enumerator enumerator2 = this.map.mapPawns.AllPawns.GetEnumerator())
			{
				if (enumerator2.MoveNext())
				{
					Pawn pawn = enumerator2.Current;
					Pawn pawn2 = SkipUtility.SkipTo(pawn, cell, dest) as Pawn;
					if (pawn2 != null && PawnUtility.ShouldSendNotificationAbout(pawn2))
					{
						Messages.Message("MessagePawnReappeared".Translate(pawn2.Named("PAWN")), pawn2, MessageTypeDefOf.NeutralEvent, false);
					}
					pawn.inventory.UnloadEverything = true;
					return;
				}
			}
			foreach (Thing thing in ((IEnumerable<Thing>)this.map.spawnedThings))
			{
				if (thing.def.category == ThingCategory.Item)
				{
					SkipUtility.SkipTo(thing, cell, dest);
					return;
				}
			}
			Find.LetterStack.ReceiveLetter("LetterLabelLabyrinthExit".Translate(), "LetterLabyrinthExit".Translate(), LetterDefOf.NeutralEvent, null, 0, true);
			PocketMapUtility.DestroyPocketMap(this.map);
			if (this.abductorObelisk != null)
			{
				this.abductorObelisk.GetComp<CompObelisk_Abductor>().Notify_MapDestroyed();
				if (this.abductorObelisk.Spawned)
				{
					EffecterDefOf.Skip_EntryNoDelay.Spawn(this.abductorObelisk.Position, this.abductorObelisk.Map, 2f).Cleanup();
					this.abductorObelisk.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x0600B203 RID: 45571 RVA: 0x0032DFD4 File Offset: 0x0032C1D4
		private static bool IsValidTeleportCell(IntVec3 pos, Map dest)
		{
			return !pos.Fogged(dest) && pos.Standable(dest) && dest.reachability.CanReachColony(pos);
		}

		// Token: 0x0600B204 RID: 45572 RVA: 0x0032DFF6 File Offset: 0x0032C1F6
		public void SetSpawnRooms(List<LayoutRoom> rooms)
		{
			this.spawnableRooms = rooms;
		}

		// Token: 0x0600B205 RID: 45573 RVA: 0x0032DFFF File Offset: 0x0032C1FF
		public void StartClosing()
		{
			this.closing = true;
		}

		// Token: 0x0600B206 RID: 45574 RVA: 0x0032E008 File Offset: 0x0032C208
		public Thing TeleportToLabyrinth(Thing thing)
		{
			IntVec3 dropPosition = this.GetDropPosition();
			Thing thing2 = SkipUtility.SkipTo(thing, dropPosition, this.map);
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				Pawn_NeedsTracker needs = pawn.needs;
				if (needs != null)
				{
					Need_Mood mood = needs.mood;
					if (mood != null)
					{
						ThoughtHandler thoughts = mood.thoughts;
						if (thoughts != null)
						{
							MemoryThoughtHandler memories = thoughts.memories;
							if (memories != null)
							{
								memories.TryGainMemory(ThoughtDefOf.ObeliskAbduction, null, null);
							}
						}
					}
				}
				if (PawnUtility.ShouldSendNotificationAbout(pawn))
				{
					Messages.Message("MessagePawnVanished".Translate(pawn.Named("PAWN")), thing2, MessageTypeDefOf.NeutralEvent, false);
				}
			}
			return thing2;
		}

		// Token: 0x0600B207 RID: 45575 RVA: 0x0032E0A4 File Offset: 0x0032C2A4
		private IntVec3 GetDropPosition()
		{
			foreach (CellRect cellRect in this.spawnableRooms.RandomElement<LayoutRoom>().rects)
			{
				LabyrinthMapComponent.tmpCells.AddRange(cellRect.ContractedBy(2));
			}
			IntVec3 root = LabyrinthMapComponent.tmpCells.RandomElement<IntVec3>();
			LabyrinthMapComponent.tmpCells.Clear();
			return CellFinder.StandableCellNear(root, this.map, 5f, null);
		}

		// Token: 0x0600B208 RID: 45576 RVA: 0x0032E138 File Offset: 0x0032C338
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.closing, "closing", false, false);
			Scribe_Values.Look<int>(ref this.nextTeleportTick, "nextTeleportTick", 0, false);
			Scribe_References.Look<Building>(ref this.labyrinthObelisk, "labyrinthObelisk", false);
			Scribe_References.Look<Building>(ref this.abductorObelisk, "abductorObelisk", false);
			Scribe_Deep.Look<LayoutStructureSketch>(ref this.structureSketch, "structureSketch", Array.Empty<object>());
			Scribe_Collections.Look<LayoutRoom>(ref this.spawnableRooms, "spawnableRects", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x040079AD RID: 31149
		public Building labyrinthObelisk;

		// Token: 0x040079AE RID: 31150
		public Building abductorObelisk;

		// Token: 0x040079AF RID: 31151
		private LayoutStructureSketch structureSketch;

		// Token: 0x040079B0 RID: 31152
		private List<LayoutRoom> spawnableRooms;

		// Token: 0x040079B1 RID: 31153
		private bool closing;

		// Token: 0x040079B2 RID: 31154
		private int nextTeleportTick;

		// Token: 0x040079B3 RID: 31155
		private static readonly IntRange TeleportDelayTicks = new IntRange(6, 60);

		// Token: 0x040079B4 RID: 31156
		private const int IntervalCheckCloseTicks = 300;

		// Token: 0x040079B5 RID: 31157
		private static readonly List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
