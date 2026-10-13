using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002EB5 RID: 11957
	public class CompTurretGun : ThingComp, IAttackTargetSearcher
	{
		// Token: 0x17002B6F RID: 11119
		// (get) Token: 0x06010DD6 RID: 69078 RVA: 0x0048FDB8 File Offset: 0x0048DFB8
		public Thing Thing
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x17002B70 RID: 11120
		// (get) Token: 0x06010DD7 RID: 69079 RVA: 0x004DB72F File Offset: 0x004D992F
		public CompProperties_TurretGun Props
		{
			get
			{
				return (CompProperties_TurretGun)this.props;
			}
		}

		// Token: 0x17002B71 RID: 11121
		// (get) Token: 0x06010DD8 RID: 69080 RVA: 0x004DB73C File Offset: 0x004D993C
		public Verb CurrentEffectiveVerb
		{
			get
			{
				return this.AttackVerb;
			}
		}

		// Token: 0x17002B72 RID: 11122
		// (get) Token: 0x06010DD9 RID: 69081 RVA: 0x004DB744 File Offset: 0x004D9944
		public LocalTargetInfo LastAttackedTarget
		{
			get
			{
				return this.lastAttackedTarget;
			}
		}

		// Token: 0x17002B73 RID: 11123
		// (get) Token: 0x06010DDA RID: 69082 RVA: 0x004DB74C File Offset: 0x004D994C
		public int LastAttackTargetTick
		{
			get
			{
				return this.lastAttackTargetTick;
			}
		}

		// Token: 0x17002B74 RID: 11124
		// (get) Token: 0x06010DDB RID: 69083 RVA: 0x004DB754 File Offset: 0x004D9954
		public CompEquippable GunCompEq
		{
			get
			{
				return this.gun.TryGetComp<CompEquippable>();
			}
		}

		// Token: 0x17002B75 RID: 11125
		// (get) Token: 0x06010DDC RID: 69084 RVA: 0x004DB761 File Offset: 0x004D9961
		public Verb AttackVerb
		{
			get
			{
				return this.GunCompEq.PrimaryVerb;
			}
		}

		// Token: 0x17002B76 RID: 11126
		// (get) Token: 0x06010DDD RID: 69085 RVA: 0x004DB76E File Offset: 0x004D996E
		private bool WarmingUp
		{
			get
			{
				return this.burstWarmupTicksLeft > 0;
			}
		}

		// Token: 0x17002B77 RID: 11127
		// (get) Token: 0x06010DDE RID: 69086 RVA: 0x004DB77C File Offset: 0x004D997C
		private bool CanShoot
		{
			get
			{
				Pawn pawn = this.parent as Pawn;
				if (pawn != null)
				{
					if (!pawn.Spawned || pawn.Downed || pawn.Dead || !pawn.Awake())
					{
						return false;
					}
					if (pawn.stances.stunner.Stunned)
					{
						return false;
					}
					if (this.TurretDestroyed)
					{
						return false;
					}
					if (pawn.IsColonyMechPlayerControlled && !this.fireAtWill)
					{
						return false;
					}
				}
				CompCanBeDormant compCanBeDormant = this.parent.TryGetComp<CompCanBeDormant>();
				return compCanBeDormant == null || compCanBeDormant.Awake;
			}
		}

		// Token: 0x17002B78 RID: 11128
		// (get) Token: 0x06010DDF RID: 69087 RVA: 0x004DB804 File Offset: 0x004D9A04
		public bool TurretDestroyed
		{
			get
			{
				Pawn pawn = this.parent as Pawn;
				return pawn != null && this.AttackVerb.verbProps.linkedBodyPartsGroup != null && this.AttackVerb.verbProps.ensureLinkedBodyPartsGroupAlwaysUsable && PawnCapacityUtility.CalculateNaturalPartsAverageEfficiency(pawn.health.hediffSet, this.AttackVerb.verbProps.linkedBodyPartsGroup) <= 0f;
			}
		}

		// Token: 0x17002B79 RID: 11129
		// (get) Token: 0x06010DE0 RID: 69088 RVA: 0x004DB86E File Offset: 0x004D9A6E
		public bool AutoAttack
		{
			get
			{
				return this.Props.autoAttack;
			}
		}

		// Token: 0x06010DE1 RID: 69089 RVA: 0x004DB87B File Offset: 0x004D9A7B
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.MakeGun();
		}

		// Token: 0x06010DE2 RID: 69090 RVA: 0x004DB889 File Offset: 0x004D9A89
		private void MakeGun()
		{
			this.gun = ThingMaker.MakeThing(this.Props.turretDef, null);
			this.UpdateGunVerbs();
		}

		// Token: 0x06010DE3 RID: 69091 RVA: 0x004DB8A8 File Offset: 0x004D9AA8
		private void UpdateGunVerbs()
		{
			List<Verb> allVerbs = this.gun.TryGetComp<CompEquippable>().AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				Verb verb = allVerbs[i];
				verb.caster = this.parent;
				verb.castCompleteCallback = delegate()
				{
					this.burstCooldownTicksLeft = this.AttackVerb.verbProps.defaultCooldownTime.SecondsToTicks();
				};
			}
		}

		// Token: 0x06010DE4 RID: 69092 RVA: 0x004DB8FC File Offset: 0x004D9AFC
		public override void CompTick()
		{
			if (!this.CanShoot)
			{
				return;
			}
			if (this.currentTarget.IsValid)
			{
				this.curRotation = (this.currentTarget.Cell.ToVector3Shifted() - this.parent.DrawPos).AngleFlat() + this.Props.angleOffset;
			}
			this.AttackVerb.VerbTick();
			if (this.AttackVerb.state != VerbState.Bursting)
			{
				if (this.WarmingUp)
				{
					this.burstWarmupTicksLeft--;
					if (this.burstWarmupTicksLeft == 0)
					{
						this.AttackVerb.TryStartCastOn(this.currentTarget, false, true, false, true);
						this.lastAttackTargetTick = Find.TickManager.TicksGame;
						this.lastAttackedTarget = this.currentTarget;
						return;
					}
				}
				else
				{
					if (this.burstCooldownTicksLeft > 0)
					{
						this.burstCooldownTicksLeft--;
					}
					if (this.burstCooldownTicksLeft <= 0 && this.parent.IsHashIntervalTick(10))
					{
						this.currentTarget = (Thing)AttackTargetFinder.BestShootTargetFromCurrentPosition(this, TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable, null, 0f, 9999f);
						if (this.currentTarget.IsValid)
						{
							this.burstWarmupTicksLeft = 1;
							return;
						}
						this.ResetCurrentTarget();
					}
				}
			}
		}

		// Token: 0x06010DE5 RID: 69093 RVA: 0x004DBA39 File Offset: 0x004D9C39
		private void ResetCurrentTarget()
		{
			this.currentTarget = LocalTargetInfo.Invalid;
			this.burstWarmupTicksLeft = 0;
		}

		// Token: 0x06010DE6 RID: 69094 RVA: 0x004DBA4D File Offset: 0x004D9C4D
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			Pawn pawn = this.parent as Pawn;
			if (pawn != null && pawn.IsColonyMechPlayerControlled)
			{
				yield return new Command_Toggle
				{
					defaultLabel = "CommandToggleTurret".Translate(),
					defaultDesc = "CommandToggleTurretDesc".Translate(),
					isActive = (() => this.fireAtWill),
					icon = CompTurretGun.ToggleTurretIcon.Texture,
					toggleAction = delegate()
					{
						this.fireAtWill = !this.fireAtWill;
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x06010DE7 RID: 69095 RVA: 0x004DBA60 File Offset: 0x004D9C60
		public override List<PawnRenderNode> CompRenderNodes()
		{
			if (!this.Props.renderNodeProperties.NullOrEmpty<PawnRenderNodeProperties>())
			{
				Pawn pawn = this.parent as Pawn;
				if (pawn != null)
				{
					List<PawnRenderNode> list = new List<PawnRenderNode>();
					foreach (PawnRenderNodeProperties pawnRenderNodeProperties in this.Props.renderNodeProperties)
					{
						PawnRenderNode_TurretGun pawnRenderNode_TurretGun = (PawnRenderNode_TurretGun)Activator.CreateInstance(pawnRenderNodeProperties.nodeClass, new object[]
						{
							pawn,
							pawnRenderNodeProperties,
							pawn.Drawer.renderer.renderTree
						});
						pawnRenderNode_TurretGun.turretComp = this;
						list.Add(pawnRenderNode_TurretGun);
					}
					return list;
				}
			}
			return base.CompRenderNodes();
		}

		// Token: 0x06010DE8 RID: 69096 RVA: 0x004DBB28 File Offset: 0x004D9D28
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.Props.turretDef != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.PawnCombat, "Turret".Translate(), this.Props.turretDef.LabelCap, "Stat_Thing_TurretDesc".Translate(), 5600, null, Gen.YieldSingle<Dialog_InfoCard.Hyperlink>(new Dialog_InfoCard.Hyperlink(this.Props.turretDef, -1)), false, false);
			}
			yield break;
		}

		// Token: 0x06010DE9 RID: 69097 RVA: 0x004DBB38 File Offset: 0x004D9D38
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.burstCooldownTicksLeft, "burstCooldownTicksLeft", 0, false);
			Scribe_Values.Look<int>(ref this.burstWarmupTicksLeft, "burstWarmupTicksLeft", 0, false);
			Scribe_TargetInfo.Look(ref this.currentTarget, "currentTarget");
			Scribe_Deep.Look<Thing>(ref this.gun, "gun", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.fireAtWill, "fireAtWill", true, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.gun == null)
				{
					Log.Error("CompTurrentGun had null gun after loading. Recreating.");
					this.MakeGun();
					return;
				}
				this.UpdateGunVerbs();
			}
		}

		// Token: 0x0400AF94 RID: 44948
		private const int StartShootIntervalTicks = 10;

		// Token: 0x0400AF95 RID: 44949
		private static readonly CachedTexture ToggleTurretIcon = new CachedTexture("UI/Gizmos/ToggleTurret");

		// Token: 0x0400AF96 RID: 44950
		public Thing gun;

		// Token: 0x0400AF97 RID: 44951
		protected int burstCooldownTicksLeft;

		// Token: 0x0400AF98 RID: 44952
		protected int burstWarmupTicksLeft;

		// Token: 0x0400AF99 RID: 44953
		protected LocalTargetInfo currentTarget = LocalTargetInfo.Invalid;

		// Token: 0x0400AF9A RID: 44954
		private bool fireAtWill = true;

		// Token: 0x0400AF9B RID: 44955
		private LocalTargetInfo lastAttackedTarget = LocalTargetInfo.Invalid;

		// Token: 0x0400AF9C RID: 44956
		private int lastAttackTargetTick;

		// Token: 0x0400AF9D RID: 44957
		public float curRotation;
	}
}
