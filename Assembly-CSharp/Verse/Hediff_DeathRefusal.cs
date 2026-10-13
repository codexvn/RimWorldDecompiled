using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200073E RID: 1854
	public class Hediff_DeathRefusal : HediffWithComps
	{
		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x06002D36 RID: 11574 RVA: 0x000E9667 File Offset: 0x000E7867
		public bool PlayerControlled
		{
			get
			{
				return this.pawn.IsColonist && (this.pawn.HostFaction == null || this.pawn.IsSlave);
			}
		}

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x06002D37 RID: 11575 RVA: 0x000E9692 File Offset: 0x000E7892
		public bool InProgress
		{
			get
			{
				return !this.resurrectTimer.Finished;
			}
		}

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x06002D38 RID: 11576 RVA: 0x000E96A2 File Offset: 0x000E78A2
		public int UsesLeft
		{
			get
			{
				return this.usesLeft;
			}
		}

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x06002D39 RID: 11577 RVA: 0x000E96AA File Offset: 0x000E78AA
		// (set) Token: 0x06002D3A RID: 11578 RVA: 0x000E96B2 File Offset: 0x000E78B2
		public bool AIEnabled
		{
			get
			{
				return this.aiEnabled;
			}
			set
			{
				this.aiEnabled = value;
			}
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06002D3B RID: 11579 RVA: 0x000E96BB File Offset: 0x000E78BB
		public virtual int MaxUses
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x06002D3C RID: 11580 RVA: 0x000E96C0 File Offset: 0x000E78C0
		public override string LabelInBrackets
		{
			get
			{
				return this.UsesLeft.ToString() + " " + ((this.UsesLeft > 1) ? "DeathRefusalUsePlural".Translate() : "DeathRefusalUseSingular".Translate()).ToString();
			}
		}

		// Token: 0x06002D3D RID: 11581 RVA: 0x000E9712 File Offset: 0x000E7912
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (!ModLister.CheckAnomaly("Death refusal"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			base.PostAdd(dinfo);
			this.usesLeft = 1;
		}

		// Token: 0x06002D3E RID: 11582 RVA: 0x000E9740 File Offset: 0x000E7940
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.pawn.Dead && this.PlayerControlled)
			{
				Command_ActionWithLimitedUseCount cmdSelfResurrect = new Command_ActionWithLimitedUseCount();
				cmdSelfResurrect.defaultLabel = "CommandSelfResurrect".Translate();
				cmdSelfResurrect.defaultDesc = "CommandSelfResurrectDesc".Translate();
				cmdSelfResurrect.usesLeftGetter = (() => this.usesLeft);
				cmdSelfResurrect.maxUsesGetter = (() => this.MaxUses);
				cmdSelfResurrect.UpdateUsesLeft();
				cmdSelfResurrect.icon = Hediff_DeathRefusal.Icon.Texture;
				cmdSelfResurrect.action = delegate()
				{
					if (!this.resurrectTimer.Finished)
					{
						return;
					}
					this.Use();
					cmdSelfResurrect.UpdateUsesLeft();
				};
				yield return cmdSelfResurrect;
			}
			yield break;
			yield break;
		}

		// Token: 0x06002D3F RID: 11583 RVA: 0x000E9750 File Offset: 0x000E7950
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.usesLeft, "usesLeft", 0, false);
			Scribe_Values.Look<bool>(ref this.resurrecting, "resurrecting", false, false);
			Scribe_Values.Look<bool>(ref this.aiEnabled, "aiEnabled", false, false);
			Scribe_Deep.Look<TickTimer>(ref this.resurrectTimer, "resurrectTimer", Array.Empty<object>());
			Scribe_Deep.Look<TickTimer>(ref this.warmupTimer, "warmupTimer", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.resurrectTimer.OnFinish = new Action(this.Resurrect);
				this.warmupTimer.OnFinish = new Action(this.Use);
				if (!this.resurrecting && this.pawn.Dead)
				{
					this.TryTriggerAIWarmupResurrection();
				}
			}
		}

		// Token: 0x06002D40 RID: 11584 RVA: 0x000E9814 File Offset: 0x000E7A14
		public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
			base.Notify_PawnDied(dinfo, culprit);
			if (this.PlayerControlled && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Messages.Message("SelfResurrectText".Translate(this.pawn.Named("PAWN")), this.pawn, MessageTypeDefOf.PositiveEvent, true);
			}
			this.TryTriggerAIWarmupResurrection();
			this.TryTriggerReadyEffect();
		}

		// Token: 0x06002D41 RID: 11585 RVA: 0x000E9880 File Offset: 0x000E7A80
		private void TryTriggerAIWarmupResurrection()
		{
			if (this.PlayerControlled || this.resurrecting || !this.AIEnabled || this.usesLeft <= 0)
			{
				return;
			}
			this.resurrecting = true;
			this.warmupTimer.Start(GenTicks.TicksGame, Hediff_DeathRefusal.AIWarmupSeconds.RandomInRange.SecondsToTicks(), new Action(this.Use));
		}

		// Token: 0x06002D42 RID: 11586 RVA: 0x000E98E4 File Offset: 0x000E7AE4
		private void TryTriggerReadyEffect()
		{
			if (this.resurrecting || this.usesLeft <= 0)
			{
				return;
			}
			if (this.resurrectAvailableEffecter == null)
			{
				Corpse corpse = this.pawn.ParentHolder as Corpse;
				if (corpse != null)
				{
					this.resurrectAvailableEffecter = EffecterDefOf.DeathRefusalAvailable.Spawn(corpse, corpse.MapHeld, Vector3.zero);
					this.pawn.MapHeld.effecterMaintainer.AddEffecterToMaintain(this.resurrectAvailableEffecter, corpse, 250);
				}
			}
		}

		// Token: 0x06002D43 RID: 11587 RVA: 0x000E995C File Offset: 0x000E7B5C
		public override void TickInterval(int delta)
		{
			if (!this.resurrectTimer.Finished)
			{
				Corpse corpse = this.pawn.ParentHolder as Corpse;
				if (corpse != null)
				{
					if (this.effecter == null)
					{
						this.effecter = EffecterDefOf.CellPollution.Spawn(corpse, this.pawn.MapHeld, Vector3.zero);
						this.pawn.MapHeld.effecterMaintainer.AddEffecterToMaintain(this.effecter, corpse, 250);
					}
					if (this.resurrectSustainer == null)
					{
						SoundInfo info = SoundInfo.InMap(corpse, MaintenanceType.PerTickRare);
						this.resurrectSustainer = SoundDefOf.Pawn_SelfResurrection.TrySpawnSustainer(info);
					}
				}
			}
		}

		// Token: 0x06002D44 RID: 11588 RVA: 0x000E9A00 File Offset: 0x000E7C00
		public void TickRare()
		{
			if (!this.warmupTimer.Finished)
			{
				this.warmupTimer.TickIntervalDelta();
			}
			this.TryTriggerReadyEffect();
			if (!this.resurrectTimer.Finished)
			{
				this.resurrectTimer.TickIntervalDelta();
				if (this.resurrectSustainer != null && !this.resurrectSustainer.Ended)
				{
					Sustainer sustainer = this.resurrectSustainer;
					if (sustainer != null)
					{
						sustainer.Maintain();
					}
				}
			}
			if (this.effecter != null)
			{
				this.effecter.ticksLeft = (this.resurrectTimer.Finished ? 0 : (this.effecter.ticksLeft + 250));
			}
			if (this.resurrectAvailableEffecter != null)
			{
				this.resurrectAvailableEffecter.ticksLeft += 250;
			}
		}

		// Token: 0x06002D45 RID: 11589 RVA: 0x000E9ABC File Offset: 0x000E7CBC
		private void Resurrect()
		{
			Sustainer sustainer = this.resurrectSustainer;
			if (sustainer != null)
			{
				sustainer.End();
			}
			this.resurrecting = false;
			this.pawn.Drawer.renderer.SetAnimation(null);
			ResurrectionUtility.TryResurrect(this.pawn, new ResurrectionParams
			{
				gettingScarsChance = 0.1f,
				canKidnap = false,
				canTimeoutOrFlee = false,
				useAvoidGridSmart = true,
				canSteal = false,
				invisibleStun = true
			});
			if (this.usesLeft == 0)
			{
				this.Severity = 0f;
			}
			if (this.pawn.Faction != Faction.OfPlayer && !this.pawn.Downed)
			{
				Thing thing = GenClosest.ClosestThingReachable(this.pawn.Position, this.pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Weapon), PathEndMode.OnCell, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 5f, null, null, 0, -1, false, RegionType.Set_Passable, false, false);
				if (thing != null)
				{
					Job job = JobGiver_PickupDroppedWeapon.PickupWeaponJob(this.pawn, thing, true);
					if (job != null)
					{
						this.pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
					}
				}
			}
			this.pawn.health.AddHediff(HediffDefOf.DeathRefusalSickness, null, null, null);
		}

		// Token: 0x06002D46 RID: 11590 RVA: 0x000E9C15 File Offset: 0x000E7E15
		public void SetUseAmountDirect(int amount, bool ignoreLimit = false)
		{
			if (ignoreLimit)
			{
				this.usesLeft = amount;
				return;
			}
			this.usesLeft = Mathf.Clamp(amount, 0, this.MaxUses);
		}

		// Token: 0x06002D47 RID: 11591 RVA: 0x000E9C38 File Offset: 0x000E7E38
		private void Use()
		{
			Messages.Message("MessageUsingSelfResurrection".Translate(this.pawn), this.pawn, MessageTypeDefOf.NeutralEvent, true);
			this.resurrecting = true;
			this.usesLeft = Mathf.Max(this.usesLeft - 1, 0);
			this.resurrectTimer.Start(GenTicks.TicksGame, Hediff_DeathRefusal.ResurrectDurationSeconds.SecondsToTicks(), new Action(this.Resurrect));
			Effecter effecter = this.resurrectAvailableEffecter;
			if (effecter != null)
			{
				effecter.ForceEnd();
			}
			this.resurrectAvailableEffecter = null;
			Corpse corpse = this.pawn.ParentHolder as Corpse;
			if (corpse != null)
			{
				this.resurrectUsedEffecter = EffecterDefOf.DeathRefusalUse.Spawn(corpse, corpse.MapHeld, Vector3.zero);
				this.pawn.MapHeld.effecterMaintainer.AddEffecterToMaintain(this.resurrectUsedEffecter, corpse, 1000);
				this.pawn.Drawer.renderer.SetAnimation(AnimationDefOf.DeathRefusalTwitches);
			}
		}

		// Token: 0x06002D48 RID: 11592 RVA: 0x000E9D39 File Offset: 0x000E7F39
		public override void Notify_PawnCorpseDestroyed()
		{
			Sustainer sustainer = this.resurrectSustainer;
			if (sustainer == null)
			{
				return;
			}
			sustainer.End();
		}

		// Token: 0x040023E5 RID: 9189
		protected int usesLeft;

		// Token: 0x040023E6 RID: 9190
		private TickTimer resurrectTimer = new TickTimer();

		// Token: 0x040023E7 RID: 9191
		private TickTimer warmupTimer = new TickTimer();

		// Token: 0x040023E8 RID: 9192
		private Sustainer resurrectSustainer;

		// Token: 0x040023E9 RID: 9193
		private bool resurrecting;

		// Token: 0x040023EA RID: 9194
		private bool aiEnabled = true;

		// Token: 0x040023EB RID: 9195
		private Effecter effecter;

		// Token: 0x040023EC RID: 9196
		private Effecter resurrectAvailableEffecter;

		// Token: 0x040023ED RID: 9197
		private Effecter resurrectUsedEffecter;

		// Token: 0x040023EE RID: 9198
		private static readonly CachedTexture Icon = new CachedTexture("UI/Abilities/SelfResurrect");

		// Token: 0x040023EF RID: 9199
		private const float ScarredChance = 0.1f;

		// Token: 0x040023F0 RID: 9200
		private static readonly float ResurrectDurationSeconds = 3f;

		// Token: 0x040023F1 RID: 9201
		private static readonly FloatRange AIWarmupSeconds = new FloatRange(1f, 2f);
	}
}
