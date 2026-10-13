using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002C41 RID: 11329
	public class CompCerebrexCore : CompInteractable
	{
		// Token: 0x170027F9 RID: 10233
		// (get) Token: 0x0600FE00 RID: 65024 RVA: 0x0049ABA8 File Offset: 0x00498DA8
		private Graphic BrainGraphic
		{
			get
			{
				Graphic result;
				if ((result = this.cachedBrainGraphic) == null)
				{
					result = (this.cachedBrainGraphic = GraphicDatabase.Get<Graphic_Multi>("Things/Building/CerebrexCore/CerebrexCore_Brain", ShaderDatabase.Cutout, CompCerebrexCore.BrainDrawSize, Color.white));
				}
				return result;
			}
		}

		// Token: 0x170027FA RID: 10234
		// (get) Token: 0x0600FE01 RID: 65025 RVA: 0x00002501 File Offset: 0x00000701
		public override bool Active
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170027FB RID: 10235
		// (get) Token: 0x0600FE02 RID: 65026 RVA: 0x0049ABE6 File Offset: 0x00498DE6
		public Lord AssemblerLord
		{
			get
			{
				return this.assemblerLord;
			}
		}

		// Token: 0x0600FE03 RID: 65027 RVA: 0x0049ABF0 File Offset: 0x00498DF0
		public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
		{
			if (!this.defencesLowered)
			{
				return "CoreNotInteractableStabilizersRemaining".Translate(this.stabilizersRemaining);
			}
			if (this.deactivated)
			{
				return "CoreNotInteractableDeactivated".Translate();
			}
			return true;
		}

		// Token: 0x0600FE04 RID: 65028 RVA: 0x0049AC40 File Offset: 0x00498E40
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.defencesLowered, "defencesLowered", false, false);
			Scribe_Values.Look<bool>(ref this.deactivated, "deactivated", false, false);
			Scribe_References.Look<Pawn>(ref this.interactedPawn, "interactedPawn", false);
			Scribe_Values.Look<bool>(ref this.scavenging, "scavenging", false, false);
			Scribe_Values.Look<int>(ref this.closeTick, "closeTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastRaidTick, "lastRaidTick", 0, false);
			Scribe_References.Look<Lord>(ref this.assemblerLord, "assemblerLord", false);
			Scribe_Values.Look<int>(ref this.stabilizersRemaining, "stabilizersRemaining", 0, false);
		}

		// Token: 0x0600FE05 RID: 65029 RVA: 0x0049ACE4 File Offset: 0x00498EE4
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (respawningAfterLoad)
			{
				return;
			}
			this.assemblerLord = LordMaker.MakeNewLord(this.parent.Faction, new LordJob_DefendCerebrexCore(this.parent.Position), this.parent.Map, null);
			this.lastRaidTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600FE06 RID: 65030 RVA: 0x0049AD40 File Offset: 0x00498F40
		public override void CompTick()
		{
			base.CompTick();
			if (Find.TickManager.TicksGame > this.lastRaidTick + 60000 && Rand.MTBEventOccurs(48f, 2500f, 1f))
			{
				List<Pawn> list = this.AssemblerLord.ownedPawns.ToList<Pawn>();
				if (list.Any<Pawn>())
				{
					Lord lord = LordMaker.MakeNewLord(this.parent.Faction, new LordJob_AssaultColony(this.parent.Faction, true, true, false, false, true, false, false), this.parent.Map, null);
					this.AssemblerLord.RemoveAllPawns(true);
					lord.AddPawns(list, true);
					this.lastRaidTick = Find.TickManager.TicksGame;
					Find.LetterStack.ReceiveLetter("MechhiveRaidLetterLabel".Translate(), "MechhiveRaidLetterText".Translate(), LetterDefOf.ThreatBig, list, null, null, null, null, 0, true);
				}
			}
			if (this.closeTick <= 0)
			{
				return;
			}
			if (Find.TickManager.TicksGame == this.closeTick)
			{
				this.DeactivateCore(this.scavenging);
			}
		}

		// Token: 0x0600FE07 RID: 65031 RVA: 0x0049AE50 File Offset: 0x00499050
		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			float z = this.ZOffset + 0.5f * (1f + Mathf.Sin(6.2831855f * (float)GenTicks.TicksGame / 300f)) * this.BobHeight;
			Matrix4x4 matrix = Matrix4x4.TRS(drawLoc + new Vector3(0f, 0.35f, z), Quaternion.AngleAxis(0f, Vector3.up), Vector3.one);
			GenDraw.DrawMeshNowOrLater(this.BrainGraphic.MeshAt(Rot4.South), matrix, this.BrainGraphic.MatSouth, false, null);
		}

		// Token: 0x0600FE08 RID: 65032 RVA: 0x0049AEE4 File Offset: 0x004990E4
		private void DeactivateCore(bool scavenging)
		{
			Find.TickManager.Pause();
			QuestUtility.SendQuestTargetSignals(this.parent.questTags, "CoreDefeated", this.parent.Named("SUBJECT"));
			ScenPart_PursuingMechanoids scenPart_PursuingMechanoids = Find.Scenario.AllParts.FirstOrDefault((ScenPart x) => x is ScenPart_PursuingMechanoids) as ScenPart_PursuingMechanoids;
			if (scenPart_PursuingMechanoids != null)
			{
				scenPart_PursuingMechanoids.Notify_QuestCompleted();
			}
			if (scavenging)
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Apparel_CerebrexNode, null);
				GenPlace.TryPlaceThing(thing, this.parent.Position, this.parent.Map, ThingPlaceMode.Near, null, null, null, 1);
				Find.LetterStack.ReceiveLetter("CerebrexCoreScavengedLetterLabel".Translate(), "CerebrexCoreScavengedLetterText".Translate(this.interactedPawn.Named("PAWN")), LetterDefOf.PositiveEvent, thing, null, null, null, null, 0, true);
				GameVictoryUtility.ShowCredits("OdysseyScavengedCredits".Translate(this.interactedPawn.Named("PAWN")), SongDefOf.OdysseyCreditsSong, false, 0f);
				return;
			}
			Map map = this.parent.Map;
			Thing.allowDestroyNonDestroyable = true;
			this.parent.Destroy(DestroyMode.Vanish);
			Thing.allowDestroyNonDestroyable = false;
			GenSpawn.Spawn(ThingDefOf.CerebrexCore_Destroyed, this.parent.Position, map, WipeMode.Vanish);
			Find.LetterStack.ReceiveLetter("CerebrexCoreDestroyedLetterLabel".Translate(), "CerebrexCoreDestroyedLetterText".Translate(this.interactedPawn.Named("PAWN")), LetterDefOf.PositiveEvent, this.parent, null, null, null, null, 0, true);
			if (Faction.OfMechanoids != null)
			{
				Faction.OfMechanoids.deactivated = true;
			}
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction)
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
								memories.TryGainMemory(ThoughtDefOf.DestroyedMechhive, null, null);
							}
						}
					}
				}
			}
			GameVictoryUtility.ShowCredits("OdysseyDestroyedCredits".Translate(this.interactedPawn.Named("PAWN")), SongDefOf.OdysseyCreditsSong, false, 0f);
		}

		// Token: 0x0600FE09 RID: 65033 RVA: 0x0049B140 File Offset: 0x00499340
		public void Notify_StabilizerDisabled()
		{
			this.stabilizersRemaining--;
			if (this.stabilizersRemaining == 0 && !this.defencesLowered)
			{
				this.LowerDefences();
			}
		}

		// Token: 0x0600FE0A RID: 65034 RVA: 0x0049B168 File Offset: 0x00499368
		private void LowerDefences()
		{
			this.defencesLowered = true;
			Find.LetterStack.ReceiveLetter("CerebrexCoreLoweredLetterLabel".Translate(), "CerebrexCoreLoweredLetterText".Translate(), LetterDefOf.PositiveEvent, this.parent, null, null, null, null, 0, true);
		}

		// Token: 0x0600FE0B RID: 65035 RVA: 0x0049B1B0 File Offset: 0x004993B0
		protected override void OnInteracted(Pawn caster)
		{
			this.interactedPawn = caster;
			this.OpenDialog();
		}

		// Token: 0x0600FE0C RID: 65036 RVA: 0x0049B1C0 File Offset: 0x004993C0
		private void OpenDialog()
		{
			Dialog_NodeTree dialog_NodeTree = new Dialog_NodeTree(new DiaNode("CerebrexCoreDialog".Translate(this.interactedPawn.Named("PAWN")))
			{
				options = 
				{
					new DiaOption("CerebrexCoreOption_Destroy".Translate())
					{
						action = delegate()
						{
							this.StartCoreDeactivation(false);
						},
						resolveTree = true
					},
					new DiaOption("CerebrexCoreOption_Scavenge".Translate())
					{
						action = delegate()
						{
							this.StartCoreDeactivation(true);
						},
						resolveTree = true
					},
					new DiaOption("CerebrexCoreOption_Postpone".Translate())
					{
						resolveTree = true
					}
				}
			}, false, false, null);
			dialog_NodeTree.forcePause = true;
			Find.WindowStack.Add(dialog_NodeTree);
		}

		// Token: 0x0600FE0D RID: 65037 RVA: 0x0049B2A0 File Offset: 0x004994A0
		private void StartCoreDeactivation(bool scavenge)
		{
			this.deactivated = true;
			this.scavenging = scavenge;
			this.closeTick = Find.TickManager.TicksGame + 300;
			Find.TickManager.slower.SignalForceNormalSpeed();
			Find.MusicManagerPlay.ForceFadeoutAndSilenceFor(999f, 3f, false);
			SoundDefOf.CerebrexCore_Pain.PlayOneShotOnCamera(null);
			CameraJumper.TryJump(this.parent, CameraJumper.MovementMode.Cut);
			ScreenFader.StartFade(Color.white, 4.9166665f);
		}

		// Token: 0x0600FE0E RID: 65038 RVA: 0x0049B320 File Offset: 0x00499520
		public override string CompInspectStringExtra()
		{
			return string.Format("{0}: {1}/{2}", "StabilizersRemaining".Translate(), this.stabilizersRemaining, 3);
		}

		// Token: 0x0400A681 RID: 42625
		private const int FadeDurationTicks = 300;

		// Token: 0x0400A682 RID: 42626
		public const int StabilizerCount = 3;

		// Token: 0x0400A683 RID: 42627
		private static readonly Vector3 BrainDrawSize = new Vector3(7f, 7f, 7f);

		// Token: 0x0400A684 RID: 42628
		private float ZOffset = 2f;

		// Token: 0x0400A685 RID: 42629
		private float BobHeight = 0.35f;

		// Token: 0x0400A686 RID: 42630
		private const int RaidMTBHours = 48;

		// Token: 0x0400A687 RID: 42631
		private const int RaidMinTimeHours = 24;

		// Token: 0x0400A688 RID: 42632
		private bool defencesLowered;

		// Token: 0x0400A689 RID: 42633
		private bool deactivated;

		// Token: 0x0400A68A RID: 42634
		private Pawn interactedPawn;

		// Token: 0x0400A68B RID: 42635
		private bool scavenging;

		// Token: 0x0400A68C RID: 42636
		private int closeTick = -99999;

		// Token: 0x0400A68D RID: 42637
		public int stabilizersRemaining = 3;

		// Token: 0x0400A68E RID: 42638
		private Lord assemblerLord;

		// Token: 0x0400A68F RID: 42639
		private int lastRaidTick = -99999;

		// Token: 0x0400A690 RID: 42640
		[Unsaved(false)]
		private Graphic cachedBrainGraphic;
	}
}
