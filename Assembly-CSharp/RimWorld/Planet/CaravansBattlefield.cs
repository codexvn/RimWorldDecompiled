using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003DA3 RID: 15779
	public class CaravansBattlefield : MapParent
	{
		// Token: 0x17003573 RID: 13683
		// (get) Token: 0x060157EC RID: 88044 RVA: 0x00671027 File Offset: 0x0066F227
		public bool WonBattle
		{
			get
			{
				return this.wonBattle;
			}
		}

		// Token: 0x17003574 RID: 13684
		// (get) Token: 0x060157ED RID: 88045 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool CanReformFoggedEnemies
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060157EE RID: 88046 RVA: 0x0067102F File Offset: 0x0066F22F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.wonBattle, "wonBattle", false, false);
		}

		// Token: 0x060157EF RID: 88047 RVA: 0x0065E340 File Offset: 0x0065C540
		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = false;
			if (base.Map.mapPawns.AnyPawnBlockingMapRemoval)
			{
				return false;
			}
			if (TransporterUtility.IncomingTransporterPreventingMapRemoval(base.Map))
			{
				return false;
			}
			alsoRemoveWorldObject = true;
			return true;
		}

		// Token: 0x060157F0 RID: 88048 RVA: 0x00671049 File Offset: 0x0066F249
		protected override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (base.HasMap)
			{
				this.CheckWonBattle();
			}
		}

		// Token: 0x060157F1 RID: 88049 RVA: 0x00671060 File Offset: 0x0066F260
		public override void PostMapGenerate()
		{
			base.PostMapGenerate();
			base.GetComponent<TimedDetectionRaids>().StartDetectionCountdown(240000, -1);
		}

		// Token: 0x060157F2 RID: 88050 RVA: 0x0067107C File Offset: 0x0066F27C
		private void CheckWonBattle()
		{
			if (this.wonBattle)
			{
				return;
			}
			if (GenHostility.AnyHostileActiveThreatToPlayer(base.Map, false, false))
			{
				return;
			}
			TimedDetectionRaids component = base.GetComponent<TimedDetectionRaids>();
			component.SetNotifiedSilently();
			string detectionCountdownTimeLeftString = component.DetectionCountdownTimeLeftString;
			Find.LetterStack.ReceiveLetter("LetterLabelCaravansBattlefieldVictory".Translate(), "LetterCaravansBattlefieldVictory".Translate(detectionCountdownTimeLeftString), LetterDefOf.PositiveEvent, this, null, null, null, null, 0, true);
			TaleRecorder.RecordTale(TaleDefOf.CaravanAmbushDefeated, new object[]
			{
				base.Map.mapPawns.FreeColonists.RandomElement<Pawn>()
			});
			this.wonBattle = true;
		}

		// Token: 0x0400EF31 RID: 61233
		private bool wonBattle;
	}
}
