using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002D70 RID: 11632
	public class CompNociosphere : CompInteractable, IActivity, IRoofCollapseAlert
	{
		// Token: 0x17002994 RID: 10644
		// (get) Token: 0x06010570 RID: 66928 RVA: 0x004B7693 File Offset: 0x004B5893
		public new CompProperties_Nociosphere Props
		{
			get
			{
				return (CompProperties_Nociosphere)this.props;
			}
		}

		// Token: 0x17002995 RID: 10645
		// (get) Token: 0x06010571 RID: 66929 RVA: 0x004B76A0 File Offset: 0x004B58A0
		public CompCauseHediff_AoE AoEComp
		{
			get
			{
				CompCauseHediff_AoE result;
				if ((result = this.aoeComp) == null)
				{
					result = (this.aoeComp = this.Pawn.GetComp<CompCauseHediff_AoE>());
				}
				return result;
			}
		}

		// Token: 0x17002996 RID: 10646
		// (get) Token: 0x06010572 RID: 66930 RVA: 0x004B76CC File Offset: 0x004B58CC
		public CompPushable Pushable
		{
			get
			{
				CompPushable result;
				if ((result = this.pushComp) == null)
				{
					result = (this.pushComp = this.Pawn.GetComp<CompPushable>());
				}
				return result;
			}
		}

		// Token: 0x17002997 RID: 10647
		// (get) Token: 0x06010573 RID: 66931 RVA: 0x004B76F8 File Offset: 0x004B58F8
		public CompStudyUnlocks StudyUnlocks
		{
			get
			{
				CompStudyUnlocks result;
				if ((result = this.studyUnlocks) == null)
				{
					result = (this.studyUnlocks = this.Pawn.GetComp<CompStudyUnlocks>());
				}
				return result;
			}
		}

		// Token: 0x17002998 RID: 10648
		// (get) Token: 0x06010574 RID: 66932 RVA: 0x004B7724 File Offset: 0x004B5924
		public CompActivity Activity
		{
			get
			{
				CompActivity result;
				if ((result = this.activityComp) == null)
				{
					result = (this.activityComp = this.Pawn.GetComp<CompActivity>());
				}
				return result;
			}
		}

		// Token: 0x17002999 RID: 10649
		// (get) Token: 0x06010575 RID: 66933 RVA: 0x0049B545 File Offset: 0x00499745
		public Pawn Pawn
		{
			get
			{
				return (Pawn)this.parent;
			}
		}

		// Token: 0x1700299A RID: 10650
		// (get) Token: 0x06010576 RID: 66934 RVA: 0x004B774F File Offset: 0x004B594F
		public bool CanSend
		{
			get
			{
				return this.Pawn.IsOnHoldingPlatform && this.StudyUnlocks.Progress >= 2;
			}
		}

		// Token: 0x1700299B RID: 10651
		// (get) Token: 0x06010577 RID: 66935 RVA: 0x004B7771 File Offset: 0x004B5971
		public bool IsUnstable
		{
			get
			{
				return this.unstableTick > 0 && Find.TickManager.TicksGame > this.unstableTick;
			}
		}

		// Token: 0x1700299C RID: 10652
		// (get) Token: 0x06010578 RID: 66936 RVA: 0x004B7790 File Offset: 0x004B5990
		public bool IsBecomingUnstable
		{
			get
			{
				return this.unstableTick > 0 && Find.TickManager.TicksGame < this.unstableTick;
			}
		}

		// Token: 0x1700299D RID: 10653
		// (get) Token: 0x06010579 RID: 66937 RVA: 0x004B77AF File Offset: 0x004B59AF
		public override string ExposeKey
		{
			get
			{
				return "Nociosphere";
			}
		}

		// Token: 0x0601057A RID: 66938 RVA: 0x004B77B6 File Offset: 0x004B59B6
		public override void Notify_KilledLeavingsLeft(List<Thing> leavings)
		{
			if (this.thingsIgnoredByExplosion == null)
			{
				this.thingsIgnoredByExplosion = new List<Thing>();
			}
			else
			{
				this.thingsIgnoredByExplosion.Clear();
			}
			this.thingsIgnoredByExplosion.AddRange(leavings);
		}

		// Token: 0x0601057B RID: 66939 RVA: 0x004B77E4 File Offset: 0x004B59E4
		public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
		{
			IntVec3 positionHeld = this.Pawn.PositionHeld;
			float radius = 4.9f;
			DamageDef vaporize = DamageDefOf.Vaporize;
			Thing pawn = this.Pawn;
			int damAmount = -1;
			float armorPenetration = -1f;
			SoundDef explosionSound = null;
			ThingDef weapon = null;
			ThingDef projectile = null;
			Thing intendedTarget = null;
			ThingDef postExplosionSpawnThingDef = null;
			float postExplosionSpawnChance = 0f;
			int postExplosionSpawnThingCount = 1;
			List<Thing> ignoredThings = this.thingsIgnoredByExplosion;
			GenExplosion.DoExplosion(positionHeld, prevMap, radius, vaporize, pawn, damAmount, armorPenetration, explosionSound, weapon, projectile, intendedTarget, postExplosionSpawnThingDef, postExplosionSpawnChance, postExplosionSpawnThingCount, null, null, 255, false, null, 0f, 1, 0f, false, null, ignoredThings, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
			Find.LetterStack.ReceiveLetter("NociosphereDefeatedLabel".Translate(), "NociosphereDefeatedText".Translate(), LetterDefOf.NeutralEvent, new TargetInfo(this.parent.Position, prevMap, false), null, null, null, null, 0, true);
		}

		// Token: 0x0601057C RID: 66940 RVA: 0x004B78B8 File Offset: 0x004B5AB8
		public override void CompTick()
		{
			float num = 0f;
			if (this.Activity.IsDormant)
			{
				this.segScaleGoal = 0f;
			}
			else
			{
				if (!this.IsUnstable && this.ShouldGoPassive())
				{
					this.Activity.EnterPassiveState();
				}
				if (this.unstableTick > 0)
				{
					this.unstableTick++;
				}
				num = 0.5f + Mathf.Sin((float)GenTicks.TicksAbs / 60f) * 0.2f;
				if (this.segScale >= 0.99f)
				{
					this.segScaleGoal = 0f;
				}
				foreach (Ability ability in this.Pawn.abilities.abilities)
				{
					if (ability.Casting && ability.verb.verbProps.warmupTime > 0f)
					{
						num += ability.verb.WarmupProgress * 0.7f;
						break;
					}
				}
			}
			this.segScale = Mathf.SmoothDamp(this.segScale, Mathf.Clamp01(this.segScaleGoal + num), ref this.segScaleVel, 0.1f, 10f, 0.016666668f);
			if (this.becomingUnstableTick <= 0)
			{
				this.becomingUnstableTick = Find.TickManager.TicksGame + Mathf.RoundToInt(CompNociosphere.BecomingUnstableRangeDays.RandomInRange * 60000f);
			}
			else if (Find.TickManager.TicksGame > this.becomingUnstableTick && this.Activity.IsDormant)
			{
				if (this.unstableTick <= 0)
				{
					this.unstableTick = Find.TickManager.TicksGame + Mathf.RoundToInt(600000f);
					TaggedString taggedString = "NociosphereBecomingUnstableText".Translate();
					if (this.CanSend)
					{
						taggedString += "\n\n" + "NociosphereBecomingUnstableTextExtraSendable".Translate();
					}
					else
					{
						taggedString += "\n\n" + "NociosphereBecomingUnstableTextExtraNotSendable".Translate();
					}
					Find.LetterStack.ReceiveLetter("NociosphereBecomingUnstableLabel".Translate(), taggedString, LetterDefOf.NeutralEvent, this.parent, null, null, null, null, 0, true);
				}
				foreach (int num2 in CompNociosphere.WarningDays)
				{
					if (this.unstableTick - Find.TickManager.TicksGame == 60000 * num2)
					{
						Messages.Message(this.Props.unstableWarning.Formatted(num2), MessageTypeDefOf.NeutralEvent, true);
					}
				}
			}
			if (Find.TickManager.TicksGame == this.unstableTick && this.Activity.IsDormant)
			{
				TaggedString taggedString2 = "NociosphereUnstableText".Translate();
				if (this.CanSend)
				{
					taggedString2 += "\n\n" + "NociosphereUnstableTextExtraSendable".Translate();
				}
				else
				{
					taggedString2 += "\n\n" + "NociosphereUnstableTextExtraNotSendable".Translate();
				}
				Find.LetterStack.ReceiveLetter("NociosphereUnstableLabel".Translate(), taggedString2, LetterDefOf.ThreatSmall, this.parent, null, null, null, null, 0, true);
			}
			if (base.OnCooldown)
			{
				if (this.CanCooldown)
				{
					this.cooldownTicks--;
				}
				if (this.cooldownTicks <= 0)
				{
					this.CooldownEnded(true);
				}
			}
		}

		// Token: 0x0601057D RID: 66941 RVA: 0x004B7C30 File Offset: 0x004B5E30
		public override void Notify_UsedVerb(Pawn pawn, Verb verb)
		{
			Verb_CastAbility verb_CastAbility = verb as Verb_CastAbility;
			if (verb_CastAbility != null && verb_CastAbility.ability.def == AbilityDefOf.EntitySkip)
			{
				return;
			}
			this.segScaleGoal = 1.2f;
		}

		// Token: 0x0601057E RID: 66942 RVA: 0x004B7C68 File Offset: 0x004B5E68
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.Pawn.GetComp<CompActivity>().IsActive)
			{
				int num = Mathf.Max(this.endOnslaughtTick - GenTicks.TicksGame, 0);
				stringBuilder.Append(this.Props.onslaughtInspectText.Formatted(num / 60));
			}
			else if (this.IsBecomingUnstable)
			{
				int numTicks = Mathf.Max(this.unstableTick - GenTicks.TicksGame, 0);
				stringBuilder.AppendLineIfNotEmpty().Append((this.Props.becomingUnstableInspectText.Formatted(Array.Empty<NamedArgument>()) + ": " + numTicks.ToStringTicksToPeriod(true, false, true, true, false)).Colorize(ColoredText.WarningColor));
			}
			else if (this.IsUnstable)
			{
				stringBuilder.AppendLineIfNotEmpty().Append(this.Props.unstableInspectText.Colorize(ColoredText.WarningColor));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0601057F RID: 66943 RVA: 0x004B7D5C File Offset: 0x004B5F5C
		public override void Notify_Downed()
		{
			this.Pawn.Kill(null, null);
		}

		// Token: 0x06010580 RID: 66944 RVA: 0x004B7D7E File Offset: 0x004B5F7E
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!this.CanSend)
			{
				yield break;
			}
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06010581 RID: 66945 RVA: 0x004B7D90 File Offset: 0x004B5F90
		public void OnActivityActivated()
		{
			if (this.Pawn.MapHeld == null)
			{
				return;
			}
			if (this.Pawn.IsOnHoldingPlatform)
			{
				Building_HoldingPlatform building_HoldingPlatform = (Building_HoldingPlatform)this.Pawn.ParentHolder;
				Thing thing;
				building_HoldingPlatform.innerContainer.TryDrop(this.Pawn, building_HoldingPlatform.Position, building_HoldingPlatform.Map, ThingPlaceMode.Direct, 1, out thing, null, null);
				CompHoldingPlatformTarget compHoldingPlatformTarget = this.Pawn.TryGetComp<CompHoldingPlatformTarget>();
				if (compHoldingPlatformTarget != null)
				{
					compHoldingPlatformTarget.targetHolder = null;
				}
			}
			this.nociosphereVictims = 0;
			this.Pushable.canBePushed = false;
			this.AoEComp.range = (float)Mathf.Max(this.Pawn.MapHeld.Size.x, this.Pawn.MapHeld.Size.z);
			this.startedOnslaught = GenTicks.TicksGame;
			this.endOnslaughtTick = GenTicks.TicksGame + Mathf.RoundToInt(this.Props.sentOnslaughtDurationSeconds.RandomInRange * 60f);
			if (!this.sentOnslaught)
			{
				this.toHarm = Mathf.Max(Mathf.RoundToInt((float)this.Pawn.MapHeld.mapPawns.ColonistCount * CompNociosphere.ColonistHarmRange.RandomInRange), 1);
				this.sentFromLocation = this.Pawn.PositionHeld;
			}
			this.Pawn.MapHeld.attackTargetsCache.UpdateTarget(this.Pawn);
			Find.TickManager.slower.SignalForceNormalSpeed();
		}

		// Token: 0x06010582 RID: 66946 RVA: 0x004B7EFC File Offset: 0x004B60FC
		public void OnPassive()
		{
			if (this.IsBecomingUnstable || this.IsUnstable)
			{
				Find.LetterStack.ReceiveLetter(this.Props.leftLetterLabel, this.Props.leftLetterText, LetterDefOf.NeutralEvent, null, 0, true);
				NociosphereUtility.SkipTo(this.Pawn, this.Pawn.Position);
				if (!this.Pawn.DestroyedOrNull())
				{
					this.Pawn.DeSpawn(DestroyMode.Vanish);
					Lord lord = this.Pawn.GetLord();
					if (lord != null)
					{
						lord.Notify_PawnLost(this.Pawn, PawnLostCondition.ExitedMap, null);
					}
					Find.WorldPawns.PassToWorld(this.Pawn, PawnDiscardDecideMode.Discard);
					return;
				}
			}
			else
			{
				IntVec3 cell;
				if (CellFinder.TryFindRandomCellNear(this.sentFromLocation, this.Pawn.Map, 3, (IntVec3 c) => c.Walkable(this.Pawn.Map) && !c.Fogged(this.Pawn.Map) && c.GetFirstPawn(this.Pawn.Map) == null && c.GetRoom(this.Pawn.Map) == this.sentFromLocation.GetRoom(this.Pawn.Map), out cell, -1))
				{
					NociosphereUtility.SkipTo(this.Pawn, cell);
				}
				this.Deactivate();
				this.sentOnslaught = false;
				this.AoEComp.range = this.AoEComp.Props.range;
				Messages.Message(this.Props.onslaughtEndedMessage, this.Pawn, MessageTypeDefOf.NeutralEvent, true);
				this.Pawn.MapHeld.attackTargetsCache.UpdateTarget(this.Pawn);
				this.Activity.SetActivity(0f, false);
				this.StartCooldown();
			}
		}

		// Token: 0x06010583 RID: 66947 RVA: 0x004B8068 File Offset: 0x004B6268
		public bool ShouldGoPassive()
		{
			if (!this.Pawn.Spawned)
			{
				return false;
			}
			if (GenTicks.TicksGame < this.startedOnslaught + this.Props.minOnslaughtTicks)
			{
				return false;
			}
			if (GenTicks.TicksGame >= this.endOnslaughtTick)
			{
				return true;
			}
			if (!this.sentOnslaught && !this.Pawn.Map.mapPawns.AnyColonistSpawned)
			{
				return true;
			}
			List<Pawn> list = this.Pawn.Map.mapPawns.FreeHumanlikesOfFaction(Faction.OfPlayer);
			if (!this.sentOnslaught)
			{
				int num = 0;
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].DeadOrDowned)
					{
						num++;
					}
					if (num >= this.toHarm)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06010584 RID: 66948 RVA: 0x004B8123 File Offset: 0x004B6323
		public bool CanBeSuppressed()
		{
			return !this.IsUnstable;
		}

		// Token: 0x06010585 RID: 66949 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool CanActivate()
		{
			return true;
		}

		// Token: 0x06010586 RID: 66950 RVA: 0x004B812E File Offset: 0x004B632E
		public string ActivityTooltipExtra()
		{
			if (this.IsUnstable)
			{
				return this.Props.unstableInspectText.Colorize(ColoredText.WarningColor);
			}
			return "";
		}

		// Token: 0x06010587 RID: 66951 RVA: 0x004B8154 File Offset: 0x004B6354
		protected override void OnInteracted(Pawn caster)
		{
			this.sentOnslaught = true;
			this.sentFromLocation = this.Pawn.PositionHeld;
			NociosphereUtility.SkipTo(this.Pawn, this.sendLocation);
			this.Activity.EnterActiveState();
			Messages.Message("CommandSent".Translate(this.Pawn.Named("PAWN")), this.Pawn, MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x06010588 RID: 66952 RVA: 0x004B81CC File Offset: 0x004B63CC
		public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
		{
			AcceptanceReport result = base.CanInteract(activateBy, checkOptionalItems);
			if (!result.Accepted)
			{
				return result;
			}
			if (!this.CanSend)
			{
				return "CannotActivateEntity".Translate();
			}
			if (!this.Pawn.IsOnHoldingPlatform)
			{
				return "CannotActivateEntityPlatform".Translate();
			}
			return true;
		}

		// Token: 0x06010589 RID: 66953 RVA: 0x004B8228 File Offset: 0x004B6428
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			if (base.ValidateTarget(target, false))
			{
				this.TargetLocation(target.Pawn);
			}
		}

		// Token: 0x0601058A RID: 66954 RVA: 0x004B8244 File Offset: 0x004B6444
		private void TargetLocation(Pawn caster)
		{
			TargetingParameters targetingParameters = TargetingParameters.ForCell();
			targetingParameters.mapBoundsContractedBy = 1;
			targetingParameters.validator = ((TargetInfo c) => c.Cell.InBounds(caster.Map) && !c.Cell.Fogged(caster.Map));
			Find.Targeter.BeginTargeting(targetingParameters, delegate(LocalTargetInfo target)
			{
				this.sendLocation = target.Cell;
				this.<>n__1(caster);
			}, delegate(LocalTargetInfo t)
			{
				Widgets.MouseAttachedLabel("ChooseNociosphereDest".Translate(), 0f, 0f, null);
			});
		}

		// Token: 0x0601058B RID: 66955 RVA: 0x004B82BA File Offset: 0x004B64BA
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			if (!this.CanSend)
			{
				yield break;
			}
			AcceptanceReport acceptanceReport = this.CanInteract(selPawn, true);
			FloatMenuOption floatMenuOption = new FloatMenuOption(this.Props.jobString.CapitalizeFirst(), delegate()
			{
				this.TargetLocation(selPawn);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			if (!acceptanceReport.Accepted)
			{
				floatMenuOption.Disabled = true;
				floatMenuOption.Label = floatMenuOption.Label + " (" + acceptanceReport.Reason + ")";
			}
			yield return floatMenuOption;
			yield break;
		}

		// Token: 0x0601058C RID: 66956 RVA: 0x004B82D4 File Offset: 0x004B64D4
		public RoofCollapseResponse Notify_OnBeforeRoofCollapse()
		{
			IntVec3 cell;
			if (RCellFinder.TryFindRandomCellNearWith(this.Pawn.Position, (IntVec3 c) => CompNociosphere.IsValidCell(c, this.Pawn.MapHeld), this.Pawn.MapHeld, out cell, 10, 2147483647))
			{
				NociosphereUtility.SkipTo(this.Pawn, cell);
				this.Activity.AdjustActivity(this.Props.activityOnRoofCollapsed);
			}
			return RoofCollapseResponse.RemoveThing;
		}

		// Token: 0x0601058D RID: 66957 RVA: 0x004A433E File Offset: 0x004A253E
		private static bool IsValidCell(IntVec3 cell, Map map)
		{
			return cell.InBounds(map) && cell.Walkable(map);
		}

		// Token: 0x0601058E RID: 66958 RVA: 0x004B8338 File Offset: 0x004B6538
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.nociosphereVictims, "nociosphereVictims", 0, false);
			Scribe_Values.Look<int>(ref this.toHarm, "toHarm", 0, false);
			Scribe_Values.Look<bool>(ref this.sentOnslaught, "sentOnslaught", false, false);
			Scribe_Values.Look<int>(ref this.endOnslaughtTick, "endOnslaughtTick", 0, false);
			Scribe_Values.Look<int>(ref this.startedOnslaught, "startedOnslaught", 0, false);
			Scribe_Values.Look<IntVec3>(ref this.sendLocation, "sendLocation", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.sentFromLocation, "sentFromLocation", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.becomingUnstableTick, "becomingUnstableTick", 0, false);
			Scribe_Values.Look<int>(ref this.unstableTick, "unstableTick", 0, false);
			Scribe_Values.Look<float>(ref this.segScale, "segScale", 0f, false);
			Scribe_Values.Look<float>(ref this.segScaleVel, "segScaleVel", 0f, false);
			Scribe_Values.Look<float>(ref this.segScaleGoal, "segScaleGoal", 0f, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.Pawn.IsWorldPawn())
			{
				Find.WorldPawns.RemoveAndDiscardPawnViaGC(this.Pawn);
			}
		}

		// Token: 0x0601058F RID: 66959 RVA: 0x004B8464 File Offset: 0x004B6664
		public override void PostDrawExtraSelectionOverlays()
		{
			if (!this.Activity.IsActive)
			{
				GenDraw.DrawRadiusRing(this.Pawn.PositionHeld, this.AoEComp.range);
			}
		}

		// Token: 0x0400AA83 RID: 43651
		private int nociosphereVictims;

		// Token: 0x0400AA84 RID: 43652
		private int toHarm;

		// Token: 0x0400AA85 RID: 43653
		public bool sentOnslaught;

		// Token: 0x0400AA86 RID: 43654
		private int endOnslaughtTick;

		// Token: 0x0400AA87 RID: 43655
		private int startedOnslaught;

		// Token: 0x0400AA88 RID: 43656
		private IntVec3 sendLocation;

		// Token: 0x0400AA89 RID: 43657
		private IntVec3 sentFromLocation;

		// Token: 0x0400AA8A RID: 43658
		private int becomingUnstableTick = -1;

		// Token: 0x0400AA8B RID: 43659
		private int unstableTick = -1;

		// Token: 0x0400AA8C RID: 43660
		public float segScale;

		// Token: 0x0400AA8D RID: 43661
		private float segScaleVel;

		// Token: 0x0400AA8E RID: 43662
		private float segScaleGoal;

		// Token: 0x0400AA8F RID: 43663
		private CompCauseHediff_AoE aoeComp;

		// Token: 0x0400AA90 RID: 43664
		private CompPushable pushComp;

		// Token: 0x0400AA91 RID: 43665
		private CompActivity activityComp;

		// Token: 0x0400AA92 RID: 43666
		private CompStudyUnlocks studyUnlocks;

		// Token: 0x0400AA93 RID: 43667
		private List<Thing> thingsIgnoredByExplosion;

		// Token: 0x0400AA94 RID: 43668
		private static readonly FloatRange ColonistHarmRange = new FloatRange(0.4f, 0.6f);

		// Token: 0x0400AA95 RID: 43669
		private static readonly FloatRange BecomingUnstableRangeDays = new FloatRange(20f, 30f);

		// Token: 0x0400AA96 RID: 43670
		private const float UnstableDays = 10f;

		// Token: 0x0400AA97 RID: 43671
		private static readonly int[] WarningDays = new int[]
		{
			2,
			5
		};

		// Token: 0x0400AA98 RID: 43672
		private const float SmoothTime = 0.1f;

		// Token: 0x0400AA99 RID: 43673
		private const float ActivityPostOnslaught = 0f;

		// Token: 0x0400AA9A RID: 43674
		private const int SendableStudyIndex = 2;
	}
}
