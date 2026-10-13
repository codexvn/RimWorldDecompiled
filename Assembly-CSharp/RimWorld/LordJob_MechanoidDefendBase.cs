using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001663 RID: 5731
	public abstract class LordJob_MechanoidDefendBase : LordJob
	{
		// Token: 0x170014B5 RID: 5301
		// (get) Token: 0x0600871C RID: 34588 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool KeepExistingWhileHasAnyBuilding
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600871D RID: 34589 RVA: 0x002858D0 File Offset: 0x00283AD0
		public override void LordJobTick()
		{
			base.LordJobTick();
			if (this.isMechCluster && !this.mechClusterDefeated && !MechClusterUtility.AnyThreatBuilding(this.things))
			{
				this.OnDefeat();
			}
		}

		// Token: 0x0600871E RID: 34590 RVA: 0x002858FB File Offset: 0x00283AFB
		public override void Notify_LordDestroyed()
		{
			if (this.isMechCluster && !this.mechClusterDefeated)
			{
				this.OnDefeat();
			}
		}

		// Token: 0x0600871F RID: 34591 RVA: 0x00285914 File Offset: 0x00283B14
		private void OnDefeat()
		{
			foreach (Thing thing in this.things)
			{
				thing.SetFaction(null, null);
				CompSendSignalOnMotion compSendSignalOnMotion = thing.TryGetComp<CompSendSignalOnMotion>();
				if (compSendSignalOnMotion != null)
				{
					compSendSignalOnMotion.Expire();
				}
				CompSendSignalOnCountdown compSendSignalOnCountdown = thing.TryGetComp<CompSendSignalOnCountdown>();
				if (compSendSignalOnCountdown != null)
				{
					compSendSignalOnCountdown.ticksLeft = 0;
				}
				ThingWithComps thingWithComps = thing as ThingWithComps;
				if (thingWithComps != null)
				{
					thingWithComps.BroadcastCompSignal("MechClusterDefeated");
				}
			}
			this.lord.Notify_MechClusterDefeated();
			for (int i = 0; i < this.thingsToNotifyOnDefeat.Count; i++)
			{
				this.thingsToNotifyOnDefeat[i].Notify_LordDestroyed();
			}
			if (!base.Map.IsPlayerHome)
			{
				IdeoUtility.Notify_PlayerRaidedSomeone(base.Map.mapPawns.FreeColonistsSpawned);
			}
			this.mechClusterDefeated = true;
			foreach (Pawn pawn in base.Map.mapPawns.FreeColonistsSpawned)
			{
				Pawn_NeedsTracker needs = pawn.needs;
				if (needs != null)
				{
					Need_Mood mood = needs.mood;
					if (mood != null)
					{
						mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DefeatedMechCluster, null, null);
					}
				}
			}
			QuestUtility.SendQuestTargetSignals(this.lord.questTags, "AllEnemiesDefeated");
			Messages.Message("MessageMechClusterDefeated".Translate(), new LookTargets(this.defSpot, base.Map), MessageTypeDefOf.PositiveEvent, true);
			SoundDefOf.MechClusterDefeated.PlayOneShotOnCamera(base.Map);
		}

		// Token: 0x06008720 RID: 34592 RVA: 0x00285ABC File Offset: 0x00283CBC
		public void AddThingToNotifyOnDefeat(Thing t)
		{
			this.thingsToNotifyOnDefeat.AddUnique(t);
		}

		// Token: 0x06008721 RID: 34593 RVA: 0x00285ACC File Offset: 0x00283CCC
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.defSpot, "defSpot", default(IntVec3), false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<float>(ref this.defendRadius, "defendRadius", 0f, false);
			Scribe_Values.Look<bool>(ref this.canAssaultColony, "canAssaultColony", false, false);
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.thingsToNotifyOnDefeat, "thingsToNotifyOnDefeat", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.isMechCluster, "isMechCluster", false, false);
			Scribe_Values.Look<bool>(ref this.mechClusterDefeated, "mechClusterDefeated", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x.DestroyedOrNull());
				this.thingsToNotifyOnDefeat.RemoveAll((Thing x) => x.DestroyedOrNull());
			}
		}

		// Token: 0x0400583F RID: 22591
		public List<Thing> things = new List<Thing>();

		// Token: 0x04005840 RID: 22592
		protected List<Thing> thingsToNotifyOnDefeat = new List<Thing>();

		// Token: 0x04005841 RID: 22593
		protected IntVec3 defSpot;

		// Token: 0x04005842 RID: 22594
		protected Faction faction;

		// Token: 0x04005843 RID: 22595
		protected float defendRadius;

		// Token: 0x04005844 RID: 22596
		protected bool canAssaultColony;

		// Token: 0x04005845 RID: 22597
		protected bool isMechCluster;

		// Token: 0x04005846 RID: 22598
		protected bool mechClusterDefeated;
	}
}
