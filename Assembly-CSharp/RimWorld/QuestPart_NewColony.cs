using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C67 RID: 7271
	public class QuestPart_NewColony : QuestPart
	{
		// Token: 0x170018EC RID: 6380
		// (get) Token: 0x0600A20A RID: 41482 RVA: 0x002E8F37 File Offset: 0x002E7137
		// (set) Token: 0x0600A20B RID: 41483 RVA: 0x002E8F3E File Offset: 0x002E713E
		public static bool IsGeneratingNewColony { get; private set; }

		// Token: 0x0600A20C RID: 41484 RVA: 0x002E8F48 File Offset: 0x002E7148
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				Find.MainTabsRoot.EscapeCurrentTab(false);
				Find.World.renderer.RegenerateLayersIfDirtyInLongEvent();
				Find.WindowStack.Add(new Dialog_ChooseThingsForNewColony(new Action<List<Thing>>(this.PostThingsSelected), 5, 5, this.maxRelics, 7, delegate()
				{
					if (!this.outSignalCancelled.NullOrEmpty())
					{
						Find.SignalManager.SendSignal(new Signal(this.outSignalCancelled, false));
					}
				}));
			}
		}

		// Token: 0x0600A20D RID: 41485 RVA: 0x002E8FBC File Offset: 0x002E71BC
		private void PostThingsSelected(List<Thing> allThings)
		{
			Action<PlanetTile> <>9__3;
			Action <>9__4;
			Find.WindowStack.Add(new Screen_ArchonexusSettlementCinematics(delegate()
			{
				CameraJumper.TryJump(CameraJumper.GetWorldTarget(allThings.First((Thing t) => t is Pawn)), CameraJumper.MovementMode.Pan);
			}, delegate()
			{
				Action<PlanetTile> targetChosen;
				if ((targetChosen = <>9__3) == null)
				{
					targetChosen = (<>9__3 = delegate(PlanetTile choseTile)
					{
						this.TileChosen(choseTile, allThings);
					});
				}
				Action noTileChosen;
				if ((noTileChosen = <>9__4) == null)
				{
					noTileChosen = (<>9__4 = delegate()
					{
						if (!this.outSignalCancelled.NullOrEmpty())
						{
							Find.SignalManager.SendSignal(new Signal(this.outSignalCancelled, false));
						}
					});
				}
				MoveColonyUtility.PickNewColonyTile(targetChosen, noTileChosen);
				ScreenFader.StartFade(Color.clear, 2f);
			}));
		}

		// Token: 0x0600A20E RID: 41486 RVA: 0x002E9004 File Offset: 0x002E7204
		private void TileChosen(PlanetTile chosenTile, List<Thing> allThings)
		{
			if (ModsConfig.IdeologyActive && !Find.IdeoManager.classicMode)
			{
				this.tmpPawns.Clear();
				for (int i = 0; i < allThings.Count; i++)
				{
					Pawn pawn = allThings[i] as Pawn;
					if (pawn != null && pawn.IsColonist)
					{
						this.tmpPawns.Add(pawn);
					}
				}
				Find.WindowStack.Add(new Dialog_ConfigureIdeo(this.tmpPawns, delegate()
				{
					this.InitMoveColony(allThings, chosenTile);
				}, true));
				this.tmpPawns.Clear();
				return;
			}
			this.InitMoveColony(allThings, chosenTile);
		}

		// Token: 0x0600A20F RID: 41487 RVA: 0x002E90CC File Offset: 0x002E72CC
		private void InitMoveColony(List<Thing> things, PlanetTile choseTile)
		{
			LongEventHandler.QueueLongEvent(delegate()
			{
				Find.MusicManagerPlay.ForceFadeoutAndSilenceFor(120f, 10f, false);
				List<Thing> list = new List<Thing>();
				list.AddRange(MoveColonyUtility.GetStartingThingsForNewColony());
				list.AddRange(things);
				QuestPart_NewColony.IsGeneratingNewColony = true;
				Settlement settlement = MoveColonyUtility.MoveColonyAndReset(choseTile, list, this.otherFaction, this.worldObjectDef);
				QuestPart_NewColony.IsGeneratingNewColony = false;
				CameraJumper.TryJump(MapGenerator.PlayerStartSpot, settlement.Map, CameraJumper.MovementMode.Pan);
				Find.TickManager.CurTimeSpeed = TimeSpeed.Normal;
				if (!this.outSignalCompleted.NullOrEmpty())
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalCompleted, false));
				}
			}, "GeneratingMap", false, null, true, false, null);
		}

		// Token: 0x0600A210 RID: 41488 RVA: 0x002E9102 File Offset: 0x002E7302
		public override void Notify_FactionRemoved(Faction f)
		{
			base.Notify_FactionRemoved(f);
			if (this.otherFaction == f)
			{
				this.otherFaction = null;
			}
		}

		// Token: 0x0600A211 RID: 41489 RVA: 0x002E911C File Offset: 0x002E731C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Faction>(ref this.otherFaction, "otherFaction", false);
			Scribe_Values.Look<string>(ref this.outSignalCompleted, "outSignalCompleted", null, false);
			Scribe_Values.Look<string>(ref this.outSignalCancelled, "outSignalCancelled", null, false);
			Scribe_Defs.Look<WorldObjectDef>(ref this.worldObjectDef, "worldObjecctDef");
			Scribe_Values.Look<int>(ref this.maxRelics, "maxRelics", 0, false);
		}

		// Token: 0x04006FF8 RID: 28664
		public string inSignal;

		// Token: 0x04006FF9 RID: 28665
		public Faction otherFaction;

		// Token: 0x04006FFA RID: 28666
		public string outSignalCompleted;

		// Token: 0x04006FFB RID: 28667
		public string outSignalCancelled;

		// Token: 0x04006FFC RID: 28668
		public WorldObjectDef worldObjectDef;

		// Token: 0x04006FFD RID: 28669
		public int maxRelics = 1;

		// Token: 0x04006FFE RID: 28670
		private List<Pawn> tmpPawns = new List<Pawn>();
	}
}
