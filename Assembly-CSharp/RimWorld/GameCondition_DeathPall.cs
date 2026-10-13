using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D38 RID: 7480
	public class GameCondition_DeathPall : GameCondition_ForceWeather
	{
		// Token: 0x0600A79B RID: 42907 RVA: 0x00300568 File Offset: 0x002FE768
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.nextResurrectTick, "nextResurrectTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastResurrectMessageTick, "lastResurrectMessageTick", -9999, false);
		}

		// Token: 0x0600A79C RID: 42908 RVA: 0x00300598 File Offset: 0x002FE798
		public override void Init()
		{
			if (!ModLister.CheckAnomaly("Death pall"))
			{
				this.End();
				return;
			}
			base.Init();
			this.nextResurrectTick = Find.TickManager.TicksGame + GameCondition_DeathPall.ResurrectIntervalRange.RandomInRange;
		}

		// Token: 0x0600A79D RID: 42909 RVA: 0x003005DC File Offset: 0x002FE7DC
		public override void GameConditionTick()
		{
			if (Find.TickManager.TicksGame < this.nextResurrectTick || Find.TickManager.TicksGame % 60 != 0)
			{
				return;
			}
			foreach (Map map in base.AffectedMaps)
			{
				foreach (Thing thing in map.listerThings.ThingsInGroup(ThingRequestGroup.Corpse))
				{
					Corpse corpse = thing as Corpse;
					if (corpse != null && MutantUtility.CanResurrectAsShambler(corpse, false) && corpse.Age >= 15000)
					{
						Pawn pawn = this.ResurrectPawn(corpse);
						if (!pawn.Position.Fogged(map))
						{
							this.TryDoMessage(pawn);
						}
						this.nextResurrectTick = Find.TickManager.TicksGame + GameCondition_DeathPall.ResurrectIntervalRange.RandomInRange;
						return;
					}
				}
			}
		}

		// Token: 0x0600A79E RID: 42910 RVA: 0x003006F8 File Offset: 0x002FE8F8
		private Pawn ResurrectPawn(Corpse corpse)
		{
			Pawn innerPawn = corpse.InnerPawn;
			MutantUtility.ResurrectAsShambler(innerPawn, 60000, null);
			return innerPawn;
		}

		// Token: 0x0600A79F RID: 42911 RVA: 0x0030070C File Offset: 0x002FE90C
		public override void End()
		{
			Find.LetterStack.ReceiveLetter("LetterLabelDeathPallEnded".Translate(), "LetterDeathPallEnded".Translate(), LetterDefOf.NeutralEvent, null, 0, true);
			base.End();
			base.SingleMap.weatherDecider.StartNextWeather();
		}

		// Token: 0x0600A7A0 RID: 42912 RVA: 0x0030074C File Offset: 0x002FE94C
		private void TryDoMessage(LookTargets target)
		{
			if (Find.TickManager.TicksGame - this.lastResurrectMessageTick < 60000)
			{
				return;
			}
			Messages.Message("DeathPallResurrectedMessage".Translate(), target, MessageTypeDefOf.NegativeEvent, false);
			this.lastResurrectMessageTick = Find.TickManager.TicksGame;
		}

		// Token: 0x04007407 RID: 29703
		private static readonly IntRange ResurrectIntervalRange = new IntRange(600, 1800);

		// Token: 0x04007408 RID: 29704
		private const int ResurrectCheckInterval = 60;

		// Token: 0x04007409 RID: 29705
		private const int ResurrectCooldown = 15000;

		// Token: 0x0400740A RID: 29706
		private const int ResurrectMessageCooldown = 60000;

		// Token: 0x0400740B RID: 29707
		private int nextResurrectTick;

		// Token: 0x0400740C RID: 29708
		private int lastResurrectMessageTick = -9999;
	}
}
