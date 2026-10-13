using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Grammar;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002DE6 RID: 11750
	public class CompRevenant : ThingComp
	{
		// Token: 0x17002A43 RID: 10819
		// (get) Token: 0x0601088F RID: 67727 RVA: 0x0049B545 File Offset: 0x00499745
		private Pawn Revenant
		{
			get
			{
				return (Pawn)this.parent;
			}
		}

		// Token: 0x17002A44 RID: 10820
		// (get) Token: 0x06010890 RID: 67728 RVA: 0x004C4950 File Offset: 0x004C2B50
		public string BiosignatureName
		{
			get
			{
				string result;
				if ((result = this.biosignatureName) == null)
				{
					result = (this.biosignatureName = AnomalyUtility.GetBiosignatureName(this.biosignature));
				}
				return result;
			}
		}

		// Token: 0x17002A45 RID: 10821
		// (get) Token: 0x06010891 RID: 67729 RVA: 0x004C497C File Offset: 0x004C2B7C
		public HediffComp_Invisibility Invisibility
		{
			get
			{
				if (this.invisibility != null)
				{
					return this.invisibility;
				}
				Hediff hediff = this.Revenant.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.HoraxianInvisibility, false);
				if (hediff == null)
				{
					hediff = this.Revenant.health.AddHediff(HediffDefOf.HoraxianInvisibility, null, null, null);
				}
				this.invisibility = ((hediff != null) ? hediff.TryGetComp<HediffComp_Invisibility>() : null);
				return this.invisibility;
			}
		}

		// Token: 0x06010892 RID: 67730 RVA: 0x004C49F0 File Offset: 0x004C2BF0
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<RevenantState>(ref this.revenantState, "revenantState", RevenantState.Wander, false);
			Scribe_Collections.Look<Pawn>(ref this.revenantVictims, "revenantVictims", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.everRevealed, "everRevealed", false, false);
			Scribe_Values.Look<bool>(ref this.revenantSmearNotified, "revenantSmearNotified", false, false);
			Scribe_Values.Look<int>(ref this.revenantLastLeftSmear, "revenantLastLeftSmear", 0, false);
			Scribe_Values.Look<int>(ref this.becomeInvisibleTick, "becomeInvisibleTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastForcedVisibilityMessage, "lastForcedVisibilityMessage", 0, false);
			Scribe_Values.Look<int>(ref this.escapeSecondStageStartedTick, "escapeSecondStageStartedTick", 0, false);
			Scribe_Values.Look<int>(ref this.biosignature, "biosignature", 0, false);
			Scribe_Values.Look<bool>(ref this.injuredWhileAttacking, "injuredWhileAttacking", false, false);
			Scribe_Values.Look<int>(ref this.nextHypnosis, "nextHypnosis", 0, false);
			Scribe_Values.Look<int>(ref this.lastHeardMessageTick, "lastHeardMessageTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastSeenLetterTick, "lastSeenLetterTick", 0, false);
		}

		// Token: 0x06010893 RID: 67731 RVA: 0x004C4AF1 File Offset: 0x004C2CF1
		public void SetState(RevenantState state)
		{
			this.revenantState = state;
			this.Revenant.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
		}

		// Token: 0x06010894 RID: 67732 RVA: 0x004C4B0E File Offset: 0x004C2D0E
		public override void PostPostMake()
		{
			this.biosignature = Rand.Int;
		}

		// Token: 0x06010895 RID: 67733 RVA: 0x004C4B1C File Offset: 0x004C2D1C
		public override void CompTick()
		{
			if (Find.TickManager.TicksGame > this.becomeInvisibleTick)
			{
				this.Invisibility.BecomeInvisible(false);
				this.becomeInvisibleTick = int.MaxValue;
			}
			if (!this.Revenant.Spawned)
			{
				return;
			}
			if (this.Revenant.IsHashIntervalTick(60) && this.Revenant.IsPsychologicallyInvisible())
			{
				this.CheckIfHeard();
			}
			if (this.Revenant.IsHashIntervalTick(90))
			{
				this.CheckIfSeen();
			}
		}

		// Token: 0x06010896 RID: 67734 RVA: 0x004C4B98 File Offset: 0x004C2D98
		private void CheckIfHeard()
		{
			AnalysisDetails analysisDetails;
			if (!Find.AnalysisManager.TryGetAnalysisProgress(this.biosignature, out analysisDetails) || analysisDetails.timesDone < 2)
			{
				return;
			}
			List<Pawn> freeColonistsSpawned = this.Revenant.Map.mapPawns.FreeColonistsSpawned;
			for (int i = 0; i < freeColonistsSpawned.Count; i++)
			{
				Pawn pawn = freeColonistsSpawned[i];
				if (this.Revenant.PositionHeld.InHorDistOf(pawn.PositionHeld, 7.9f) && WanderUtility.InSameRoom(pawn.PositionHeld, this.Revenant.PositionHeld, this.Revenant.Map) && !MentalStateUtility.IsHavingMentalBreak(pawn))
				{
					MoteMaker.MakeAttachedOverlay(pawn, ThingDefOf.Mote_RevenantHeard, new Vector3(0f, 0f, 1.1f), 1f, -1f);
					GrammarRequest request = default(GrammarRequest);
					request.Includes.Add(RulePackDefOf.RevenantNoises);
					if (Find.TickManager.TicksGame > this.lastHeardMessageTick + 1200)
					{
						Messages.Message("MessageRevenantHeard".Translate(pawn.Named("PAWN"), GrammarResolver.Resolve("verb", request, null, false, null, null, null, false)), pawn, MessageTypeDefOf.ThreatSmall, true);
						this.lastHeardMessageTick = Find.TickManager.TicksGame;
					}
				}
			}
		}

		// Token: 0x06010897 RID: 67735 RVA: 0x004C4CFC File Offset: 0x004C2EFC
		private void CheckIfSeen()
		{
			AnalysisDetails analysisDetails;
			if (!Find.AnalysisManager.TryGetAnalysisProgress(this.biosignature, out analysisDetails) || !analysisDetails.Satisfied)
			{
				return;
			}
			List<Pawn> freeColonistsSpawned = this.Revenant.Map.mapPawns.FreeColonistsSpawned;
			bool flag = false;
			for (int i = 0; i < freeColonistsSpawned.Count; i++)
			{
				Pawn pawn = freeColonistsSpawned[i];
				if (!PawnUtility.IsBiologicallyOrArtificiallyBlind(pawn) && this.Revenant.PositionHeld.InHorDistOf(pawn.PositionHeld, 8.9f) && GenSight.LineOfSightToThing(pawn.PositionHeld, this.Revenant, this.Revenant.Map, false, null))
				{
					if (this.Revenant.IsPsychologicallyInvisible() && Find.TickManager.TicksGame > this.lastSeenLetterTick + 1200)
					{
						Find.LetterStack.ReceiveLetter("LetterRevenantSeenLabel".Translate(), "LetterRevenantSeen".Translate(pawn.Named("PAWN")), LetterDefOf.ThreatBig, pawn, null, null, null, null, 0, true);
						this.lastSeenLetterTick = Find.TickManager.TicksGame;
					}
					flag = true;
					break;
				}
			}
			if (flag)
			{
				this.Invisibility.BecomeVisible(false);
				this.becomeInvisibleTick = Find.TickManager.TicksGame + 140;
			}
		}

		// Token: 0x06010898 RID: 67736 RVA: 0x004C4E4A File Offset: 0x004C304A
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (this.Revenant.Dead)
			{
				return;
			}
			if (this.revenantState == RevenantState.Attack)
			{
				this.injuredWhileAttacking = true;
			}
			if (this.revenantState == RevenantState.Sleep)
			{
				this.SetState(RevenantState.Escape);
			}
		}

		// Token: 0x06010899 RID: 67737 RVA: 0x004C4E7A File Offset: 0x004C307A
		public override void Notify_BecameVisible()
		{
			if (!this.Revenant.Spawned)
			{
				return;
			}
			SoundDefOf.Pawn_Revenant_Revealed.PlayOneShotOnCamera(null);
			this.everRevealed = true;
			this.revenantSmearNotified = true;
		}

		// Token: 0x0601089A RID: 67738 RVA: 0x004C4EA3 File Offset: 0x004C30A3
		public override void Notify_BecameInvisible()
		{
			if (!this.Revenant.Spawned)
			{
				return;
			}
			SoundDefOf.Pawn_Revenant_Stealth.PlayOneShot(this.Revenant);
		}

		// Token: 0x0601089B RID: 67739 RVA: 0x004C4EC8 File Offset: 0x004C30C8
		public override void Notify_ForcedVisible()
		{
			if (!this.Revenant.Spawned)
			{
				return;
			}
			if (!this.Revenant.Position.Fogged(this.Revenant.Map))
			{
				SoundDefOf.Pawn_Revenant_StartledScream.PlayOneShot(this.Revenant);
				if (Find.TickManager.TicksGame > this.lastForcedVisibilityMessage + 2400)
				{
					Messages.Message("MessageRevenantForcedVisibility".Translate(), this.Revenant, MessageTypeDefOf.ThreatSmall, true);
					this.lastForcedVisibilityMessage = Find.TickManager.TicksGame;
				}
			}
		}

		// Token: 0x0601089C RID: 67740 RVA: 0x004C4F62 File Offset: 0x004C3162
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			this.ClearVictims();
		}

		// Token: 0x0601089D RID: 67741 RVA: 0x004C4F62 File Offset: 0x004C3162
		public override void Notify_PassedToWorld()
		{
			this.ClearVictims();
		}

		// Token: 0x0601089E RID: 67742 RVA: 0x004C4F6C File Offset: 0x004C316C
		private void ClearVictims()
		{
			if (!this.revenantVictims.NullOrEmpty<Pawn>())
			{
				foreach (Pawn pawn in this.revenantVictims)
				{
					Hediff hediff;
					if (pawn == null)
					{
						hediff = null;
					}
					else
					{
						Pawn_HealthTracker health = pawn.health;
						if (health == null)
						{
							hediff = null;
						}
						else
						{
							HediffSet hediffSet = health.hediffSet;
							hediff = ((hediffSet != null) ? hediffSet.GetFirstHediffOfDef(HediffDefOf.RevenantHypnosis, false) : null);
						}
					}
					Hediff hediff2 = hediff;
					if (hediff2 != null)
					{
						pawn.health.RemoveHediff(hediff2);
					}
				}
				this.revenantVictims.Clear();
			}
		}

		// Token: 0x0601089F RID: 67743 RVA: 0x004C500C File Offset: 0x004C320C
		public void Hypnotize(Pawn victim)
		{
			if (victim.Dead)
			{
				return;
			}
			victim.health.AddHediff(HediffDefOf.RevenantHypnosis, null, null, null);
			RestUtility.WakeUp(victim, true);
			this.revenantVictims.Add(victim);
			this.Revenant.Drawer.renderer.SetAnimation(AnimationDefOf.RevenantSpasm);
			this.Revenant.mindState.lastEngageTargetTick = Find.TickManager.TicksGame;
			this.Revenant.mindState.enemyTarget = null;
			this.nextHypnosis = Find.TickManager.TicksGame + Mathf.FloorToInt(RevenantUtility.SearchForTargetCooldownRangeDays.RandomInRange * 60000f);
			if (PawnUtility.ShouldSendNotificationAbout(victim))
			{
				Find.LetterStack.ReceiveLetter("LetterLabelPawnHypnotized".Translate(victim.Named("PAWN")), "LetterPawnHypnotized".Translate(victim.Named("PAWN")), LetterDefOf.NegativeEvent, victim, null, null, null, null, 0, true);
			}
		}

		// Token: 0x060108A0 RID: 67744 RVA: 0x004C510C File Offset: 0x004C330C
		public override string CompInspectStringExtra()
		{
			TaggedString taggedString = "Biosignature".Translate() + ": " + this.BiosignatureName;
			if (DebugSettings.showHiddenInfo)
			{
				taggedString += "\nState: " + this.revenantState.ToString();
			}
			return taggedString;
		}

		// Token: 0x060108A1 RID: 67745 RVA: 0x004C5168 File Offset: 0x004C3368
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!DebugSettings.ShowDevGizmos)
			{
				yield break;
			}
			if (Find.TickManager.TicksGame < this.nextHypnosis)
			{
				yield return new Command_Action
				{
					defaultLabel = "Reset hypnosis cooldown",
					action = delegate()
					{
						this.nextHypnosis = Find.TickManager.TicksGame;
					}
				};
			}
			if (this.revenantState != RevenantState.Wander)
			{
				yield return new Command_Action
				{
					defaultLabel = "Change to wander mode",
					action = delegate()
					{
						this.revenantState = RevenantState.Wander;
						this.Revenant.mindState.enemyTarget = null;
						this.Revenant.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				};
			}
			if (this.revenantState != RevenantState.Sleep)
			{
				yield return new Command_Action
				{
					defaultLabel = "Change to sleep mode",
					action = delegate()
					{
						this.revenantState = RevenantState.Sleep;
						this.Revenant.mindState.enemyTarget = null;
						this.Revenant.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				};
			}
			if (this.revenantState == RevenantState.Wander || this.revenantState == RevenantState.Sleep)
			{
				yield return new Command_Action
				{
					defaultLabel = "Find target",
					action = delegate()
					{
						Pawn pawn = RevenantUtility.ScanForTarget(this.Revenant, true);
						if (pawn != null)
						{
							this.Revenant.mindState.enemyTarget = pawn;
							this.revenantState = RevenantState.Attack;
							this.Revenant.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
						}
					}
				};
			}
			yield break;
		}

		// Token: 0x0400AC6E RID: 44142
		private const int ForcedVisibilityMessageCooldownTicks = 2400;

		// Token: 0x0400AC6F RID: 44143
		private const int HeardCheckInterval = 60;

		// Token: 0x0400AC70 RID: 44144
		private const int SeenCheckInterval = 90;

		// Token: 0x0400AC71 RID: 44145
		private const float HeardDistance = 7.9f;

		// Token: 0x0400AC72 RID: 44146
		private const float SeenDistance = 8.9f;

		// Token: 0x0400AC73 RID: 44147
		private const int HeardMessageCooldownTicks = 1200;

		// Token: 0x0400AC74 RID: 44148
		private const int SeenLetterCooldownTicks = 1200;

		// Token: 0x0400AC75 RID: 44149
		public const int LongerTracksUnlockIndex = 1;

		// Token: 0x0400AC76 RID: 44150
		public const int CanHearUnlockIndex = 2;

		// Token: 0x0400AC77 RID: 44151
		public RevenantState revenantState;

		// Token: 0x0400AC78 RID: 44152
		public List<Pawn> revenantVictims = new List<Pawn>();

		// Token: 0x0400AC79 RID: 44153
		public bool everRevealed;

		// Token: 0x0400AC7A RID: 44154
		public bool revenantSmearNotified;

		// Token: 0x0400AC7B RID: 44155
		public int revenantLastLeftSmear = -99999;

		// Token: 0x0400AC7C RID: 44156
		public int becomeInvisibleTick = int.MaxValue;

		// Token: 0x0400AC7D RID: 44157
		public int lastForcedVisibilityMessage = -99999;

		// Token: 0x0400AC7E RID: 44158
		public int escapeSecondStageStartedTick = -99999;

		// Token: 0x0400AC7F RID: 44159
		public int biosignature;

		// Token: 0x0400AC80 RID: 44160
		public bool injuredWhileAttacking;

		// Token: 0x0400AC81 RID: 44161
		public int nextHypnosis = -99999;

		// Token: 0x0400AC82 RID: 44162
		private int lastHeardMessageTick = -99999;

		// Token: 0x0400AC83 RID: 44163
		private int lastSeenLetterTick = -99999;

		// Token: 0x0400AC84 RID: 44164
		private string biosignatureName;

		// Token: 0x0400AC85 RID: 44165
		[Unsaved(false)]
		private HediffComp_Invisibility invisibility;
	}
}
