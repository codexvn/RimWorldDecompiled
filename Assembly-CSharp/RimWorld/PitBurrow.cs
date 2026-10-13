using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002B0D RID: 11021
	public class PitBurrow : Crater
	{
		// Token: 0x1700267E RID: 9854
		// (get) Token: 0x0600F611 RID: 62993 RVA: 0x00475E20 File Offset: 0x00474020
		protected override EffecterDef FilledInEffecter
		{
			get
			{
				return EffecterDefOf.ImpactSmallDustCloud;
			}
		}

		// Token: 0x1700267F RID: 9855
		// (get) Token: 0x0600F612 RID: 62994 RVA: 0x00477253 File Offset: 0x00475453
		protected override SoundDef FilledInSound
		{
			get
			{
				return SoundDefOf.PitBurrow_Collapse;
			}
		}

		// Token: 0x0600F613 RID: 62995 RVA: 0x0047725C File Offset: 0x0047545C
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (!ModLister.CheckAnomaly("Pit burrow"))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
			if (respawningAfterLoad)
			{
				return;
			}
			this.collapseTick = Find.TickManager.TicksGame + PitBurrow.CollapseTicksRange.RandomInRange;
			if (this.emergingFleshbeasts.NullOrEmpty<Pawn>())
			{
				return;
			}
			this.FlushSpawnQueue();
		}

		// Token: 0x0600F614 RID: 62996 RVA: 0x004772BC File Offset: 0x004754BC
		private void FlushSpawnQueue()
		{
			CellRect cellRect = GenAdj.OccupiedRect(base.Position, Rot4.North, ThingDefOf.PitBurrow.Size).ContractedBy(2);
			List<PawnFlyer> list = new List<PawnFlyer>();
			List<IntVec3> list2 = new List<IntVec3>();
			foreach (Pawn pawn in this.emergingFleshbeasts)
			{
				IntVec3 randomCell = cellRect.RandomCell;
				GenSpawn.Spawn(pawn, randomCell, base.Map, WipeMode.Vanish);
				IntVec3 intVec;
				if (CellFinder.TryFindRandomCellNear(base.Position, base.Map, this.def.size.x / 2 + 1, (IntVec3 c) => !c.Fogged(base.Map) && c.Walkable(base.Map) && !c.Impassable(base.Map), out intVec, -1))
				{
					pawn.rotationTracker.FaceCell(intVec);
					list.Add(PawnFlyer.MakeFlyer(ThingDefOf.PawnFlyer_Stun, pawn, intVec, null, null, false, new Vector3?(randomCell.ToVector3() + new Vector3(0f, 0f, -1f)), null, default(LocalTargetInfo)));
					list2.Add(randomCell);
				}
			}
			if (list2.Count == 0)
			{
				return;
			}
			SpawnRequest spawnRequest = new SpawnRequest(list.Cast<Thing>().ToList<Thing>(), list2, 1, 1f, null);
			spawnRequest.initialDelay = this.emergeDelay;
			if (this.assaultColony)
			{
				spawnRequest.lord = LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_FleshbeastAssault(), base.Map, null);
			}
			base.Map.deferredSpawner.AddRequest(spawnRequest, true);
			SoundDefOf.Pawn_Fleshbeast_EmergeFromPitGate.PlayOneShot(this);
			this.emergingFleshbeasts.Clear();
		}

		// Token: 0x0600F615 RID: 62997 RVA: 0x00477470 File Offset: 0x00475670
		protected override void Tick()
		{
			base.Tick();
			if (Find.TickManager.TicksGame > this.collapseTick)
			{
				Messages.Message("MessagePitBurrowCollapsed".Translate(), new TargetInfo(base.Position, base.Map, false), MessageTypeDefOf.NeutralEvent, false);
				this.Collapse();
			}
		}

		// Token: 0x0600F616 RID: 62998 RVA: 0x004774CC File Offset: 0x004756CC
		private void Collapse()
		{
			EffecterDefOf.ImpactSmallDustCloud.Spawn(base.Position, base.Map, 1f).Cleanup();
			this.FillIn();
		}

		// Token: 0x0600F617 RID: 62999 RVA: 0x004774F4 File Offset: 0x004756F4
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					action = delegate()
					{
						if (this.emergingFleshbeasts == null)
						{
							this.emergingFleshbeasts = new List<Pawn>();
						}
						PawnKindDef fingerspike = PawnKindDefOf.Fingerspike;
						Pawn item = PawnGenerator.GeneratePawn(fingerspike, FactionUtility.DefaultFactionFrom(fingerspike.defaultFactionDef), null);
						this.emergingFleshbeasts.Add(item);
						this.FlushSpawnQueue();
					},
					defaultLabel = "DEV: Spawn Fleshbeast"
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600F618 RID: 63000 RVA: 0x00477504 File Offset: 0x00475704
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.emergeDelay, "emergeDelay", 0, false);
			Scribe_Collections.Look<Pawn>(ref this.emergingFleshbeasts, "emergingFleshbeasts", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.assaultColony, "assaultColony", true, false);
			Scribe_Values.Look<int>(ref this.collapseTick, "collapseTick", 0, false);
		}

		// Token: 0x0400A1F9 RID: 41465
		public List<Pawn> emergingFleshbeasts;

		// Token: 0x0400A1FA RID: 41466
		public int emergeDelay;

		// Token: 0x0400A1FB RID: 41467
		public bool assaultColony = true;

		// Token: 0x0400A1FC RID: 41468
		private int collapseTick;

		// Token: 0x0400A1FD RID: 41469
		private const int SpawnIntervalSeconds = 1;

		// Token: 0x0400A1FE RID: 41470
		private static readonly IntRange CollapseTicksRange = new IntRange(720000, 1200000);
	}
}
