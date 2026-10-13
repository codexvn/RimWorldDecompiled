using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002C5A RID: 11354
	public class CompDevourer : ThingComp, IThingHolder
	{
		// Token: 0x17002819 RID: 10265
		// (get) Token: 0x0600FE93 RID: 65171 RVA: 0x0049CB03 File Offset: 0x0049AD03
		public CompProperties_Devourer Props
		{
			get
			{
				return (CompProperties_Devourer)this.props;
			}
		}

		// Token: 0x1700281A RID: 10266
		// (get) Token: 0x0600FE94 RID: 65172 RVA: 0x0049CB10 File Offset: 0x0049AD10
		public Thing DigestingThing
		{
			get
			{
				if (this.innerContainer.InnerListForReading.Count <= 0)
				{
					return null;
				}
				return this.innerContainer.InnerListForReading[0];
			}
		}

		// Token: 0x1700281B RID: 10267
		// (get) Token: 0x0600FE95 RID: 65173 RVA: 0x0049CB38 File Offset: 0x0049AD38
		public Pawn DigestingPawn
		{
			get
			{
				Thing digestingThing = this.DigestingThing;
				if (digestingThing == null)
				{
					return null;
				}
				Corpse corpse = digestingThing as Corpse;
				if (corpse != null)
				{
					return corpse.InnerPawn;
				}
				return digestingThing as Pawn;
			}
		}

		// Token: 0x1700281C RID: 10268
		// (get) Token: 0x0600FE96 RID: 65174 RVA: 0x0049CB68 File Offset: 0x0049AD68
		public bool Digesting
		{
			get
			{
				return this.DigestingThing != null;
			}
		}

		// Token: 0x1700281D RID: 10269
		// (get) Token: 0x0600FE97 RID: 65175 RVA: 0x0048AE48 File Offset: 0x00489048
		public Pawn Pawn
		{
			get
			{
				return this.parent as Pawn;
			}
		}

		// Token: 0x0600FE98 RID: 65176 RVA: 0x0049CB73 File Offset: 0x0049AD73
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600FE99 RID: 65177 RVA: 0x0049CB81 File Offset: 0x0049AD81
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x0600FE9A RID: 65178 RVA: 0x0049CB89 File Offset: 0x0049AD89
		public CompDevourer()
		{
			this.innerContainer = new ThingOwner<Thing>(this, LookMode.Deep, false);
		}

		// Token: 0x0600FE9B RID: 65179 RVA: 0x0049CB9F File Offset: 0x0049AD9F
		public override void CompTick()
		{
			if (this.Digesting)
			{
				this.ticksDigesting++;
			}
			if (this.Digesting && this.DigestingPawn.Dead)
			{
				this.CompleteDigestion();
			}
		}

		// Token: 0x0600FE9C RID: 65180 RVA: 0x0049CBD4 File Offset: 0x0049ADD4
		public override string CompInspectStringExtra()
		{
			if (!this.Digesting)
			{
				return null;
			}
			int digestionTicks = this.GetDigestionTicks();
			int ticksLeftThisToil = this.Pawn.jobs.curDriver.ticksLeftThisToil;
			int num = (ticksLeftThisToil < 0) ? digestionTicks : ticksLeftThisToil;
			float num2 = (float)(digestionTicks - (digestionTicks - num)) / 60f;
			return this.Props.digestingInspector.Formatted(this.DigestingThing.Named("PAWN"), num2.Named("SECONDS"));
		}

		// Token: 0x0600FE9D RID: 65181 RVA: 0x0049CC53 File Offset: 0x0049AE53
		public override void Notify_Downed()
		{
			this.AbortDigestion(this.Pawn.MapHeld);
		}

		// Token: 0x0600FE9E RID: 65182 RVA: 0x0049CC66 File Offset: 0x0049AE66
		public override void Notify_Killed(Map prevMap, DamageInfo? _ = null)
		{
			this.AbortDigestion(prevMap);
		}

		// Token: 0x0600FE9F RID: 65183 RVA: 0x0049CC6F File Offset: 0x0049AE6F
		public void DigestJobFinished()
		{
			if (this.Pawn.BeingTransportedOnGravship)
			{
				return;
			}
			if (this.ticksDigesting >= this.ticksToDigestFully)
			{
				this.CompleteDigestion();
				return;
			}
			this.AbortDigestion(this.Pawn.MapHeld);
		}

		// Token: 0x0600FEA0 RID: 65184 RVA: 0x0049CCA8 File Offset: 0x0049AEA8
		public override void PostSwapMap()
		{
			if (this.Digesting)
			{
				this.Pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.DevourerDigest), JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
			}
		}

		// Token: 0x0600FEA1 RID: 65185 RVA: 0x0049CCF4 File Offset: 0x0049AEF4
		private void AbortDigestion(Map map)
		{
			if (!this.Digesting)
			{
				return;
			}
			Pawn pawn = this.DropPawn(map);
			Find.BattleLog.Add(new BattleLogEntry_Event(pawn, RulePackDefOf.Event_DevourerDigestionAborted, this.Pawn));
			float amount = this.Props.timeDamageCurve.Evaluate((float)this.ticksDigesting / 60f);
			DamageInfo dinfo = new DamageInfo(DamageDefOf.AcidBurn, amount, 0f, -1f, this.Pawn, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
			dinfo.SetApplyAllDamage(true);
			pawn.TakeDamage(dinfo);
			if (pawn.Faction == Faction.OfPlayer)
			{
				string text = this.Pawn.Dead ? this.Props.messageEmergedCorpse : this.Props.messageEmerged;
				if (!text.NullOrEmpty())
				{
					text = text.Formatted(pawn.Named("PAWN"));
					Messages.Message(text, pawn, MessageTypeDefOf.NeutralEvent, true);
				}
			}
			this.EndDigestingJob();
			this.Pawn.Drawer.renderer.SetAllGraphicsDirty();
			if (this.Pawn.Drawer.renderer.CurAnimation == AnimationDefOf.DevourerDigesting)
			{
				this.Pawn.Drawer.renderer.SetAnimation(null);
			}
		}

		// Token: 0x0600FEA2 RID: 65186 RVA: 0x0049CE34 File Offset: 0x0049B034
		private void CompleteDigestion()
		{
			if (!this.Digesting)
			{
				return;
			}
			Pawn pawn = this.DropPawn(this.Pawn.MapHeld);
			Find.BattleLog.Add(new BattleLogEntry_Event(pawn, RulePackDefOf.Event_DevourerDigestionCompleted, this.Pawn));
			DamageInfo dinfo = new DamageInfo(DamageDefOf.AcidBurn, (float)this.Props.completeDigestionDamage, 0f, -1f, this.Pawn, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
			dinfo.SetApplyAllDamage(true);
			pawn.TakeDamage(dinfo);
			if (!this.Props.messageDigestionCompleted.NullOrEmpty() && !pawn.Dead && pawn.Faction == Faction.OfPlayer)
			{
				Messages.Message(this.Props.messageDigestionCompleted.Formatted(pawn.Named("PAWN")), pawn, MessageTypeDefOf.NegativeEvent, true);
			}
			this.Pawn.Drawer.renderer.SetAllGraphicsDirty();
			if (this.Pawn.Drawer.renderer.CurAnimation == AnimationDefOf.DevourerDigesting)
			{
				this.Pawn.Drawer.renderer.SetAnimation(null);
			}
		}

		// Token: 0x0600FEA3 RID: 65187 RVA: 0x0049CF58 File Offset: 0x0049B158
		public void StartDigesting(IntVec3 origin, LocalTargetInfo target)
		{
			if (target.HasThing)
			{
				Pawn pawn = target.Thing as Pawn;
				if (pawn != null && pawn.Spawned)
				{
					DamageInfo dinfo = new DamageInfo(DamageDefOf.AcidBurn, 99f, 0f, -1f, this.parent, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
					Lord lord = pawn.GetLord();
					if (lord != null)
					{
						lord.Notify_PawnDamaged(pawn, dinfo);
					}
					if (pawn.drafter != null)
					{
						this.wasDrafted = pawn.drafter.Drafted;
					}
					pawn.DeSpawn(DestroyMode.Vanish);
					this.ticksDigesting = 0;
					this.innerContainer.TryAdd(pawn, true);
					this.ticksToDigestFully = this.GetDigestionTicks() - 30;
					this.Pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.DevourerDigest), JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
					if (!this.Props.messageDigested.NullOrEmpty() && pawn.Faction == Faction.OfPlayer)
					{
						Messages.Message(this.Props.messageDigested.Formatted(pawn.Named("PAWN")), this.Pawn, MessageTypeDefOf.NegativeEvent, true);
					}
					this.Pawn.Rotation = Rot4.FromAngleFlat((this.parent.Position - origin).AngleFlat);
					this.Pawn.Drawer.renderer.SetAllGraphicsDirty();
					if (this.Pawn.Drawer.renderer.CurAnimation != AnimationDefOf.DevourerDigesting)
					{
						this.Pawn.Drawer.renderer.SetAnimation(AnimationDefOf.DevourerDigesting);
					}
					Find.BattleLog.Add(new BattleLogEntry_Event(pawn, RulePackDefOf.Event_DevourerConsumeLeap, this.Pawn));
					return;
				}
			}
			this.Pawn.abilities.GetAbility(AbilityDefOf.ConsumeLeap_Devourer, false).ResetCooldown();
		}

		// Token: 0x0600FEA4 RID: 65188 RVA: 0x0049D140 File Offset: 0x0049B340
		private Pawn DropPawn(Map map)
		{
			if (!this.Digesting)
			{
				return null;
			}
			Thing thing;
			if (!this.innerContainer.TryDrop(this.DigestingThing, this.Pawn.PositionHeld, map, ThingPlaceMode.Near, out thing, null, null))
			{
				IntVec3 loc;
				if (!RCellFinder.TryFindRandomCellNearWith(this.Pawn.PositionHeld, (IntVec3 c) => c.Standable(map), map, out loc, 1, 2147483647))
				{
					Debug.LogError("Could not drop digesting pawn from devourer!");
					return null;
				}
				thing = GenSpawn.Spawn(this.innerContainer.Take(this.DigestingThing), loc, map, WipeMode.Vanish);
			}
			Corpse corpse = thing as Corpse;
			if (corpse != null)
			{
				return corpse.InnerPawn;
			}
			Pawn pawn = (Pawn)thing;
			pawn.stances.stunner.StunFor(60, this.Pawn, false, false, false);
			if (pawn.drafter != null)
			{
				pawn.drafter.Drafted = this.wasDrafted;
			}
			return pawn;
		}

		// Token: 0x0600FEA5 RID: 65189 RVA: 0x0049D234 File Offset: 0x0049B434
		public int GetDigestionTicks()
		{
			if (this.DigestingThing == null)
			{
				return 0;
			}
			return Mathf.CeilToInt(this.Props.bodySizeDigestTimeCurve.Evaluate(this.DigestingPawn.BodySize) * 60f);
		}

		// Token: 0x0600FEA6 RID: 65190 RVA: 0x0049D268 File Offset: 0x0049B468
		private void EndDigestingJob()
		{
			if (!this.Pawn.Dead && this.Pawn.CurJobDef == JobDefOf.DevourerDigest && this.Pawn.jobs.curDriver != null && !this.Pawn.jobs.curDriver.ended)
			{
				this.Pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		// Token: 0x0600FEA7 RID: 65191 RVA: 0x0049D2D4 File Offset: 0x0049B4D4
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksDigesting, "ticksDigesting", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToDigestFully, "ticksToDigestFully", 0, false);
			Scribe_Values.Look<bool>(ref this.wasDrafted, "wasDrafted", false, false);
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.innerContainer.removeContentsIfDestroyed)
			{
				this.innerContainer.removeContentsIfDestroyed = false;
			}
		}

		// Token: 0x0400A6CF RID: 42703
		private ThingOwner<Thing> innerContainer;

		// Token: 0x0400A6D0 RID: 42704
		private int ticksDigesting;

		// Token: 0x0400A6D1 RID: 42705
		private int ticksToDigestFully;

		// Token: 0x0400A6D2 RID: 42706
		private bool wasDrafted;
	}
}
