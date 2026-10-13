using System;
using System.Collections.Generic;
using RimWorld;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000752 RID: 1874
	public class Hediff_DarknessExposure : Hediff
	{
		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x06002DD0 RID: 11728 RVA: 0x000EBB9A File Offset: 0x000E9D9A
		public override bool ShouldRemove
		{
			get
			{
				return !this.pawn.Spawned || this.pawn.Downed || !GameCondition_UnnaturalDarkness.InUnnaturalDarkness(this.pawn);
			}
		}

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x06002DD1 RID: 11729 RVA: 0x000EBBC6 File Offset: 0x000E9DC6
		private DamageDef RandomDamageDef
		{
			get
			{
				if (Hediff_DarknessExposure.possibleDamageDefs == null)
				{
					Hediff_DarknessExposure.possibleDamageDefs = new List<DamageDef>
					{
						DamageDefOf.Scratch,
						DamageDefOf.Bite,
						DamageDefOf.Cut
					};
				}
				return Hediff_DarknessExposure.possibleDamageDefs.RandomElement<DamageDef>();
			}
		}

		// Token: 0x06002DD2 RID: 11730 RVA: 0x000EBC04 File Offset: 0x000E9E04
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (!ModLister.CheckAnomaly("Darkness exposure"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			base.PostAdd(dinfo);
		}

		// Token: 0x06002DD3 RID: 11731 RVA: 0x000EBC2C File Offset: 0x000E9E2C
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.nextDamageTick < 0)
			{
				this.nextDamageTick = Find.TickManager.TicksGame + Hediff_DarknessExposure.DamageIntervalTicks_Fresh.RandomInRange;
			}
			if (Find.TickManager.TicksGame >= this.nextDamageTick)
			{
				SoundDefOf.DarknessDamage.PlayOneShot(this.pawn);
				BattleLogEntry_DamageTaken battleLogEntry_DamageTaken = new BattleLogEntry_DamageTaken(this.pawn, RulePackDefOf.DamageEvent_UnnaturalDarkness, null);
				Find.BattleLog.Add(battleLogEntry_DamageTaken);
				this.pawn.TakeDamage(new DamageInfo(this.RandomDamageDef, Hediff_DarknessExposure.DamageRange.RandomInRange, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false)).AssociateWithLog(battleLogEntry_DamageTaken);
				this.nextDamageTick = Find.TickManager.TicksGame + Hediff_DarknessExposure.DamageIntervalTicks_NonFresh.RandomInRange;
				this.CheckNotifyPlayer();
			}
		}

		// Token: 0x06002DD4 RID: 11732 RVA: 0x000EBD10 File Offset: 0x000E9F10
		private void CheckNotifyPlayer()
		{
			if (!PawnUtility.ShouldSendNotificationAbout(this.pawn) && !this.pawn.IsColonySubhumanPlayerControlled)
			{
				return;
			}
			if (this.lastNotifyTick + 5000 < Find.TickManager.TicksGame)
			{
				GameCondition_UnnaturalDarkness activeCondition = this.pawn.MapHeld.gameConditionManager.GetActiveCondition<GameCondition_UnnaturalDarkness>();
				if (activeCondition != null && !activeCondition.anyColonistAttacked)
				{
					Find.LetterStack.ReceiveLetter("PawnAttackedInDarknessLabel".Translate(this.pawn), "PawnAttackedInDarknessText".Translate(this.pawn), LetterDefOf.ThreatSmall, this.pawn, null, null, null, null, 0, true);
					activeCondition.anyColonistAttacked = true;
				}
				else
				{
					Messages.Message("MessagePawnAttackedInDarkness".Translate(this.pawn), this.pawn, MessageTypeDefOf.ThreatSmall, false);
				}
				this.lastNotifyTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06002DD5 RID: 11733 RVA: 0x00002501 File Offset: 0x00000701
		public override bool TryMergeWith(Hediff other)
		{
			return false;
		}

		// Token: 0x06002DD6 RID: 11734 RVA: 0x000EBE07 File Offset: 0x000EA007
		public override void Notify_Downed()
		{
			this.nextDamageTick = -1;
		}

		// Token: 0x06002DD7 RID: 11735 RVA: 0x000EBE10 File Offset: 0x000EA010
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.nextDamageTick, "nextDamageTick", -1, false);
			Scribe_Values.Look<int>(ref this.lastNotifyTick, "lastNotifyTick", -1, false);
		}

		// Token: 0x04002421 RID: 9249
		private int nextDamageTick = -1;

		// Token: 0x04002422 RID: 9250
		private int lastNotifyTick = -1;

		// Token: 0x04002423 RID: 9251
		private static readonly IntRange DamageIntervalTicks_Fresh = new IntRange(480, 720);

		// Token: 0x04002424 RID: 9252
		private static readonly IntRange DamageIntervalTicks_NonFresh = new IntRange(180, 300);

		// Token: 0x04002425 RID: 9253
		private static readonly FloatRange DamageRange = new FloatRange(2.5f, 5.5f);

		// Token: 0x04002426 RID: 9254
		private const int TicksBetweenWarnings = 5000;

		// Token: 0x04002427 RID: 9255
		private static List<DamageDef> possibleDamageDefs;
	}
}
