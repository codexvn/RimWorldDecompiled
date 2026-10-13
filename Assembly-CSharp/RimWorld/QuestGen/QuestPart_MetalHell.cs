using System;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld.QuestGen
{
	// Token: 0x02003C0A RID: 15370
	public class QuestPart_MetalHell : QuestPartActivable
	{
		// Token: 0x1700338C RID: 13196
		// (get) Token: 0x06014D86 RID: 85382 RVA: 0x0063E66F File Offset: 0x0063C86F
		private int VFXStartTick
		{
			get
			{
				return this.enableTick + 540 - 300;
			}
		}

		// Token: 0x1700338D RID: 13197
		// (get) Token: 0x06014D87 RID: 85383 RVA: 0x0063E683 File Offset: 0x0063C883
		private int SkipTick
		{
			get
			{
				return this.enableTick + 540;
			}
		}

		// Token: 0x06014D88 RID: 85384 RVA: 0x002D9B28 File Offset: 0x002D7D28
		public QuestPart_MetalHell()
		{
		}

		// Token: 0x06014D89 RID: 85385 RVA: 0x0063E691 File Offset: 0x0063C891
		public QuestPart_MetalHell(string activateMonolithSignal, string voidNodeTag)
		{
			this.inSignalEnable = activateMonolithSignal;
			this.voidNodeTag = voidNodeTag;
			this.reactivatable = true;
		}

		// Token: 0x06014D8A RID: 85386 RVA: 0x0063E6AE File Offset: 0x0063C8AE
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignalEnable)
			{
				signal.args.TryGetArg<Pawn>("ACTIVATOR", out this.pawnToSend);
			}
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x06014D8B RID: 85387 RVA: 0x0063E6E4 File Offset: 0x0063C8E4
		public override void QuestPartTick()
		{
			if (this.pawnToSend.Downed || this.pawnToSend.Dead)
			{
				Messages.Message("GleamingMonolithInterrupted".Translate(this.pawnToSend.Named("PAWN")), MessageTypeDefOf.NeutralEvent, true);
				this.Disable();
				return;
			}
			if (Find.TickManager.TicksGame > this.VFXStartTick && Find.TickManager.TicksGame < this.SkipTick)
			{
				if (this.monolithEffecter == null)
				{
					this.monolithEffecter = EffecterDefOf.AbductWarmup_Monolith.Spawn(Find.Anomaly.monolith, Find.Anomaly.monolith.Map, 1f);
				}
				if (this.pawnEffecter == null)
				{
					this.pawnEffecter = EffecterDefOf.AbductWarmup_Pawn.Spawn(this.pawnToSend, this.pawnToSend.Map, 1f);
				}
				this.pawnEffecter.EffectTick(this.pawnToSend, this.pawnToSend);
				this.monolithEffecter.EffectTick(Find.Anomaly.monolith, Find.Anomaly.monolith);
				Find.TickManager.slower.SignalForceNormalSpeedShort();
			}
			if (Find.TickManager.TicksGame == this.SkipTick)
			{
				ScreenFader.StartFade(Color.black, 0f);
			}
			if (Find.TickManager.TicksGame == this.SkipTick)
			{
				this.GenerateMetalHellAndSkipPawn();
				ScreenFader.StartFade(Color.clear, 7f);
				this.Disable();
			}
		}

		// Token: 0x06014D8C RID: 85388 RVA: 0x0063E874 File Offset: 0x0063CA74
		private void GenerateMetalHellAndSkipPawn()
		{
			if (this.metalHell == null)
			{
				this.metalHell = PocketMapUtility.GeneratePocketMap(new IntVec3(75, 1, 75), MapGeneratorDefOf.MetalHell, null, Find.Anomaly.monolith.Map);
			}
			Thing voidNode = this.metalHell.listerThings.ThingsOfDef(ThingDefOf.VoidNode).FirstOrDefault<Thing>();
			QuestUtility.AddQuestTag(ref voidNode.questTags, this.voidNodeTag);
			IntVec3 cell = CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(this.metalHell) && !c.InHorDistOf(this.metalHell.Center, 10f) && this.metalHell.reachability.CanReach(c, voidNode, PathEndMode.Touch, TraverseMode.PassDoors), this.metalHell, 1000);
			SkipUtility.SkipTo(this.pawnToSend, cell, this.metalHell);
			this.pawnToSend.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			this.pawnToSend.drafter.Drafted = true;
			CameraJumper.TryJump(this.pawnToSend, CameraJumper.MovementMode.Cut);
			Find.TickManager.slower.SignalForceNormalSpeed();
			Find.LetterStack.ReceiveLetter("VoidAwakeningFinalStructuresActivatedLabel".Translate(this.pawnToSend.Named("STUDIER")), "VoidAwakeningFinalStructuresActivatedText".Translate(this.pawnToSend.Named("STUDIER")), LetterDefOf.NeutralEvent, this.pawnToSend, null, null, null, null, 0, false);
			Find.MusicManagerPlay.CheckTransitions();
		}

		// Token: 0x06014D8D RID: 85389 RVA: 0x0063E9C8 File Offset: 0x0063CBC8
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving && this.metalHell != null && !Find.World.pocketMaps.Contains(this.metalHell.Parent))
			{
				this.metalHell = null;
			}
			Scribe_Values.Look<string>(ref this.voidNodeTag, "voidNodeTag", null, false);
			Scribe_References.Look<Map>(ref this.metalHell, "metalHell", false);
			Scribe_References.Look<Pawn>(ref this.pawnToSend, "pawnToSend", false);
		}

		// Token: 0x06014D8E RID: 85390 RVA: 0x0063EA42 File Offset: 0x0063CC42
		public override void Cleanup()
		{
			base.Cleanup();
			this.metalHell = null;
			this.pawnToSend = null;
		}

		// Token: 0x0400E869 RID: 59497
		private const int MetalHellSize = 75;

		// Token: 0x0400E86A RID: 59498
		private const int MinSpawnDistFromMetalHellCenter = 10;

		// Token: 0x0400E86B RID: 59499
		private const int SkipDelayTicks = 540;

		// Token: 0x0400E86C RID: 59500
		private const int EffectsDurationTicks = 300;

		// Token: 0x0400E86D RID: 59501
		private const int FadeOutDurationTicks = 0;

		// Token: 0x0400E86E RID: 59502
		private const int FadeInDurationTicks = 420;

		// Token: 0x0400E86F RID: 59503
		private string voidNodeTag;

		// Token: 0x0400E870 RID: 59504
		private Map metalHell;

		// Token: 0x0400E871 RID: 59505
		private Pawn pawnToSend;

		// Token: 0x0400E872 RID: 59506
		[Unsaved(false)]
		private Effecter monolithEffecter;

		// Token: 0x0400E873 RID: 59507
		[Unsaved(false)]
		private Effecter pawnEffecter;
	}
}
