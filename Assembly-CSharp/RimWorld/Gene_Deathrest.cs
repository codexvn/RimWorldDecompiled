using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003837 RID: 14391
	public class Gene_Deathrest : Gene
	{
		// Token: 0x170032A7 RID: 12967
		// (get) Token: 0x06013E29 RID: 81449 RVA: 0x005E73E4 File Offset: 0x005E55E4
		public Need_Deathrest DeathrestNeed
		{
			get
			{
				if (this.cachedDeathrestNeed == null)
				{
					this.pawn.needs.TryGetNeed<Need_Deathrest>(out this.cachedDeathrestNeed);
				}
				return this.cachedDeathrestNeed;
			}
		}

		// Token: 0x170032A8 RID: 12968
		// (get) Token: 0x06013E2A RID: 81450 RVA: 0x005E740B File Offset: 0x005E560B
		public List<Thing> BoundBuildings
		{
			get
			{
				return this.boundBuildings;
			}
		}

		// Token: 0x170032A9 RID: 12969
		// (get) Token: 0x06013E2B RID: 81451 RVA: 0x005E7414 File Offset: 0x005E5614
		public List<CompDeathrestBindable> BoundComps
		{
			get
			{
				if (this.cachedBoundComps == null)
				{
					this.cachedBoundComps = new List<CompDeathrestBindable>();
					foreach (Thing thing in this.boundBuildings)
					{
						if (this.CanUseBindableNow(thing))
						{
							this.cachedBoundComps.Add(thing.TryGetComp<CompDeathrestBindable>());
						}
					}
				}
				return this.cachedBoundComps;
			}
		}

		// Token: 0x170032AA RID: 12970
		// (get) Token: 0x06013E2C RID: 81452 RVA: 0x005E7494 File Offset: 0x005E5694
		public float DeathrestEfficiency
		{
			get
			{
				float num = 1f;
				foreach (CompDeathrestBindable compDeathrestBindable in this.BoundComps)
				{
					if (compDeathrestBindable.Props.deathrestEffectivenessFactor > 0f && compDeathrestBindable.CanIncreasePresence)
					{
						num *= compDeathrestBindable.Props.deathrestEffectivenessFactor;
					}
				}
				return num;
			}
		}

		// Token: 0x170032AB RID: 12971
		// (get) Token: 0x06013E2D RID: 81453 RVA: 0x005E7510 File Offset: 0x005E5710
		public int MinDeathrestTicks
		{
			get
			{
				return Mathf.RoundToInt(240000f / this.DeathrestEfficiency);
			}
		}

		// Token: 0x170032AC RID: 12972
		// (get) Token: 0x06013E2E RID: 81454 RVA: 0x005E7523 File Offset: 0x005E5723
		public int DeathrestCapacity
		{
			get
			{
				return this.deathrestCapacity;
			}
		}

		// Token: 0x170032AD RID: 12973
		// (get) Token: 0x06013E2F RID: 81455 RVA: 0x005E752C File Offset: 0x005E572C
		public int CurrentCapacity
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.boundBuildings.Count; i++)
				{
					CompDeathrestBindable compDeathrestBindable = this.boundBuildings[i].TryGetComp<CompDeathrestBindable>();
					if (compDeathrestBindable != null && compDeathrestBindable.Props.countsTowardsBuildingLimit)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x170032AE RID: 12974
		// (get) Token: 0x06013E30 RID: 81456 RVA: 0x005E7578 File Offset: 0x005E5778
		public bool AtBuildingCapacityLimit
		{
			get
			{
				return this.CurrentCapacity >= this.deathrestCapacity;
			}
		}

		// Token: 0x170032AF RID: 12975
		// (get) Token: 0x06013E31 RID: 81457 RVA: 0x005E758B File Offset: 0x005E578B
		public float DeathrestPercent
		{
			get
			{
				return Mathf.Clamp01((float)this.deathrestTicks / (float)this.MinDeathrestTicks);
			}
		}

		// Token: 0x170032B0 RID: 12976
		// (get) Token: 0x06013E32 RID: 81458 RVA: 0x005E75A1 File Offset: 0x005E57A1
		public bool ShowWakeAlert
		{
			get
			{
				return this.DeathrestPercent >= 1f && !this.autoWake;
			}
		}

		// Token: 0x06013E33 RID: 81459 RVA: 0x005E75BB File Offset: 0x005E57BB
		public override void PostAdd()
		{
			if (!ModLister.CheckBiotech("Deathrest"))
			{
				return;
			}
			base.PostAdd();
			this.Reset();
		}

		// Token: 0x06013E34 RID: 81460 RVA: 0x005E75D8 File Offset: 0x005E57D8
		public override void PostRemove()
		{
			base.PostRemove();
			Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Deathrest, false);
			if (firstHediffOfDef != null)
			{
				this.pawn.health.RemoveHediff(firstHediffOfDef);
			}
			Hediff firstHediffOfDef2 = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DeathrestExhaustion, false);
			if (firstHediffOfDef2 != null)
			{
				this.pawn.health.RemoveHediff(firstHediffOfDef2);
			}
			this.RemoveOldDeathrestBonuses();
			this.Reset();
		}

		// Token: 0x06013E35 RID: 81461 RVA: 0x005E7657 File Offset: 0x005E5857
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.pawn.IsHashIntervalTick(200, delta) && this.pawn.IsColonistPlayerControlled)
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.Deathrest, OpportunityType.Important);
			}
		}

		// Token: 0x06013E36 RID: 81462 RVA: 0x005E768C File Offset: 0x005E588C
		public void OffsetCapacity(int offset, bool sendNotification = true)
		{
			int num = this.deathrestCapacity;
			this.deathrestCapacity += offset;
			if (sendNotification && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Messages.Message("MessageDeathrestCapacityChanged".Translate(this.pawn.Named("PAWN"), num.Named("OLD"), this.deathrestCapacity.Named("NEW")), this.pawn, MessageTypeDefOf.PositiveEvent, true);
			}
		}

		// Token: 0x06013E37 RID: 81463 RVA: 0x005E7718 File Offset: 0x005E5918
		public void TickDeathresting(bool paused, int delta)
		{
			if (paused)
			{
				return;
			}
			if (this.DeathrestNeed != null)
			{
				this.DeathrestNeed.lastDeathrestTick = Find.TickManager.TicksGame;
			}
			this.deathrestTicks += delta;
			this.adjustedDeathrestTicks += this.DeathrestEfficiency * (float)delta;
			foreach (CompDeathrestBindable compDeathrestBindable in this.BoundComps)
			{
				compDeathrestBindable.TryIncreasePresence(delta);
			}
			if (this.DeathrestPercent >= 1f && !this.notifiedWakeOK)
			{
				this.notifiedWakeOK = true;
				if (this.autoWake)
				{
					this.Wake();
					return;
				}
				if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
				{
					Messages.Message("MessageDeathrestingPawnCanWakeSafely".Translate(this.pawn.Named("PAWN")), this.pawn, MessageTypeDefOf.PositiveEvent, true);
				}
			}
			if (this.pawn.Spawned && this.pawn.IsHashIntervalTick(150, delta) && this.PawnOrBedTouchingSunlight())
			{
				if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
				{
					Messages.Message("MessagePawnWokenFromSunlight".Translate(this.pawn.Named("PAWN")), this.pawn, MessageTypeDefOf.NegativeEvent, true);
				}
				this.Wake();
			}
		}

		// Token: 0x06013E38 RID: 81464 RVA: 0x005E788C File Offset: 0x005E5A8C
		private bool PawnOrBedTouchingSunlight()
		{
			Building_Bed building_Bed = this.pawn.CurrentBed();
			if (building_Bed != null)
			{
				using (CellRect.Enumerator enumerator = building_Bed.OccupiedRect().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.InSunlight(building_Bed.Map))
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06013E39 RID: 81465 RVA: 0x005E7900 File Offset: 0x005E5B00
		public void Notify_DeathrestStarted()
		{
			this.RemoveOldDeathrestBonuses();
			this.TryLinkToNearbyDeathrestBuildings();
			this.notifiedWakeOK = false;
		}

		// Token: 0x06013E3A RID: 81466 RVA: 0x005E7918 File Offset: 0x005E5B18
		public void Notify_DeathrestEnded()
		{
			foreach (Thing thing in this.boundBuildings)
			{
				thing.TryGetComp<CompDeathrestBindable>().Notify_DeathrestEnded();
			}
		}

		// Token: 0x06013E3B RID: 81467 RVA: 0x005E7970 File Offset: 0x005E5B70
		public void Notify_BoundBuildingDeSpawned(Thing building)
		{
			this.boundBuildings.Remove(building);
			this.cachedBoundComps = null;
		}

		// Token: 0x06013E3C RID: 81468 RVA: 0x005E7988 File Offset: 0x005E5B88
		public void TryLinkToNearbyDeathrestBuildings()
		{
			if (!ModsConfig.BiotechActive || !this.pawn.Spawned)
			{
				return;
			}
			this.cachedBoundComps = null;
			Room room = this.pawn.GetRoom(RegionType.Set_All);
			if (room == null)
			{
				return;
			}
			foreach (Region region in room.Regions)
			{
				foreach (Thing thing in region.ListerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial))
				{
					CompDeathrestBindable bindComp = thing.TryGetComp<CompDeathrestBindable>();
					if (this.CanBindToBindable(bindComp))
					{
						this.BindTo(bindComp);
					}
				}
			}
		}

		// Token: 0x06013E3D RID: 81469 RVA: 0x005E7A58 File Offset: 0x005E5C58
		public void RemoveOldDeathrestBonuses()
		{
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = hediffs.Count - 1; i >= 0; i--)
			{
				if (hediffs[i].def.removeOnDeathrestStart)
				{
					this.pawn.health.RemoveHediff(hediffs[i]);
				}
			}
			Gene_Hemogen firstGeneOfType = this.pawn.genes.GetFirstGeneOfType<Gene_Hemogen>();
			if (firstGeneOfType == null)
			{
				return;
			}
			firstGeneOfType.ResetMax();
		}

		// Token: 0x06013E3E RID: 81470 RVA: 0x005E7AD4 File Offset: 0x005E5CD4
		private void ApplyDeathrestBuildingBonuses()
		{
			if (this.deathrestTicks == 0)
			{
				return;
			}
			Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.InterruptedDeathrest, false);
			if (firstHediffOfDef != null)
			{
				this.pawn.health.RemoveHediff(firstHediffOfDef);
			}
			foreach (CompDeathrestBindable compDeathrestBindable in this.BoundComps)
			{
				if ((float)compDeathrestBindable.presenceTicks / (float)this.deathrestTicks >= 0.75f)
				{
					compDeathrestBindable.Apply();
				}
			}
		}

		// Token: 0x06013E3F RID: 81471 RVA: 0x005E7B78 File Offset: 0x005E5D78
		public bool CanBindToBindable(CompDeathrestBindable bindComp)
		{
			return ModsConfig.BiotechActive && bindComp != null && bindComp.parent.Faction == this.pawn.Faction && !this.boundBuildings.Contains(bindComp.parent) && (!bindComp.Props.countsTowardsBuildingLimit || !this.AtBuildingCapacityLimit) && bindComp.CanBindTo(this.pawn) && this.CanUseBindableNow(bindComp.parent) && !this.BindingWillExceedStackLimit(bindComp);
		}

		// Token: 0x06013E40 RID: 81472 RVA: 0x005E7C08 File Offset: 0x005E5E08
		public bool BindingWillExceedStackLimit(CompDeathrestBindable bindComp)
		{
			if (this.boundBuildings.Contains(bindComp.parent))
			{
				return false;
			}
			int stackLimit = bindComp.Props.stackLimit;
			if (stackLimit > 0)
			{
				int num = 0;
				for (int i = 0; i < this.boundBuildings.Count; i++)
				{
					if (this.boundBuildings[i].def == bindComp.parent.def)
					{
						num++;
						if (num >= stackLimit)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06013E41 RID: 81473 RVA: 0x005E7C7C File Offset: 0x005E5E7C
		private bool CanUseBindableNow(Thing building)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			CompDeathrestBindable compDeathrestBindable = building.TryGetComp<CompDeathrestBindable>();
			if (compDeathrestBindable == null)
			{
				return false;
			}
			Building_Bed building_Bed = this.pawn.CurrentBed();
			if (building_Bed == null)
			{
				return false;
			}
			if (compDeathrestBindable.Props.mustBeLayingInToBind)
			{
				if (building_Bed != building)
				{
					return false;
				}
			}
			else if (!building_Bed.def.HasComp(typeof(CompDeathrestBindable)))
			{
				return false;
			}
			return building.Spawned && building.Map == this.pawn.MapHeld && building.GetRoom(RegionType.Set_All) == this.pawn.GetRoom(RegionType.Set_All) && GenSight.LineOfSight(this.pawn.Position, building.OccupiedRect().CenterCell, this.pawn.Map);
		}

		// Token: 0x06013E42 RID: 81474 RVA: 0x005E7D40 File Offset: 0x005E5F40
		public void BindTo(CompDeathrestBindable bindComp)
		{
			bindComp.BindTo(this.pawn);
			this.boundBuildings.Add(bindComp.parent);
			this.cachedBoundComps = null;
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn) && bindComp.Props.countsTowardsBuildingLimit)
			{
				Messages.Message("MessageDeathrestBuildingBound".Translate(bindComp.parent.Named("BUILDING"), this.pawn.Named("PAWN"), this.CurrentCapacity.Named("CUR"), this.DeathrestCapacity.Named("MAX")), new LookTargets(new TargetInfo[]
				{
					bindComp.parent,
					this.pawn
				}), MessageTypeDefOf.NeutralEvent, false);
			}
		}

		// Token: 0x06013E43 RID: 81475 RVA: 0x005E7E24 File Offset: 0x005E6024
		public override void Reset()
		{
			this.deathrestCapacity = 1;
			foreach (CompDeathrestBindable compDeathrestBindable in this.BoundComps)
			{
				compDeathrestBindable.Notify_DeathrestGeneRemoved();
			}
			this.cachedBoundComps = null;
		}

		// Token: 0x06013E44 RID: 81476 RVA: 0x005E7E84 File Offset: 0x005E6084
		public override void Notify_NewColony()
		{
			if (this.pawn.BeingTransportedOnGravship)
			{
				return;
			}
			this.boundBuildings.Clear();
			this.cachedBoundComps = null;
		}

		// Token: 0x06013E45 RID: 81477 RVA: 0x005E7EA6 File Offset: 0x005E60A6
		public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
			base.Notify_PawnDied(dinfo, culprit);
			this.boundBuildings.Clear();
			this.cachedBoundComps = null;
			this.cachedDeathrestNeed = null;
		}

		// Token: 0x06013E46 RID: 81478 RVA: 0x005E7ECC File Offset: 0x005E60CC
		public void Wake()
		{
			if (this.DeathrestPercent < 1f)
			{
				this.pawn.health.AddHediff(HediffDefOf.InterruptedDeathrest, null, null, null);
			}
			else
			{
				this.ApplyDeathrestBuildingBonuses();
			}
			Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Deathrest, false);
			if (firstHediffOfDef != null)
			{
				this.pawn.health.RemoveHediff(firstHediffOfDef);
			}
			this.deathrestTicks = 0;
			this.adjustedDeathrestTicks = 0f;
		}

		// Token: 0x06013E47 RID: 81479 RVA: 0x005E7F51 File Offset: 0x005E6151
		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (!this.Active)
			{
				yield break;
			}
			if (this.gizmo == null)
			{
				this.gizmo = new GeneGizmo_DeathrestCapacity(this);
			}
			if (Find.Selector.SelectedPawns.Count == 1)
			{
				yield return this.gizmo;
			}
			if (this.pawn.Deathresting)
			{
				if (this.pawn.IsColonistPlayerControlled || this.pawn.IsPrisonerOfColony)
				{
					string text = "WakeDesc".Translate(this.pawn.Named("PAWN"), this.deathrestTicks.ToStringTicksToPeriod(true, false, true, true, false).Named("DURATION")).Resolve() + "\n\n";
					if (this.DeathrestPercent < 1f)
					{
						text += "WakeExtraDesc_Exhaustion".Translate(this.pawn.Named("PAWN"), this.MinDeathrestTicks.ToStringTicksToPeriod(true, false, true, true, false).Named("TOTAL")).Resolve();
					}
					else
					{
						text += "WakeExtraDesc_Safe".Translate(this.pawn.Named("PAWN")).Resolve();
					}
					Command_Action command_Action = new Command_Action
					{
						defaultLabel = "Wake".Translate().CapitalizeFirst(),
						defaultDesc = text,
						icon = Gene_Deathrest.WakeCommandTex.Texture,
						action = delegate()
						{
							if (this.DeathrestPercent < 1f)
							{
								Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation("WarningWakingInterruptsDeathrest".Translate(this.pawn.Named("PAWN"), this.MinDeathrestTicks.ToStringTicksToPeriod(true, false, true, true, false).Named("MINDURATION"), this.deathrestTicks.ToStringTicksToPeriod(true, false, true, true, false).Named("CURDURATION")), new Action(this.Wake), true, null, WindowLayer.Dialog);
								Find.WindowStack.Add(window);
								return;
							}
							this.Wake();
						}
					};
					if (SanguophageUtility.ShouldBeDeathrestingOrInComaInsteadOfDead(this.pawn))
					{
						command_Action.Disable("WakeDisabledCriticalCondition".Translate(this.pawn.Named("PAWN")));
					}
					yield return command_Action;
					if (this.DeathrestPercent < 1f)
					{
						Command_Toggle command_Toggle = new Command_Toggle
						{
							defaultLabel = "AutoWake".Translate().CapitalizeFirst(),
							defaultDesc = "AutoWakeDesc".Translate(this.pawn.Named("PAWN")).Resolve(),
							icon = Gene_Deathrest.AutoWakeCommandTex.Texture,
							isActive = (() => this.autoWake),
							toggleAction = delegate()
							{
								this.autoWake = !this.autoWake;
							}
						};
						yield return command_Toggle;
					}
				}
				if (DebugSettings.ShowDevGizmos)
				{
					Command_Action command_Action2 = new Command_Action
					{
						defaultLabel = "DEV: Wake and apply bonuses",
						action = delegate()
						{
							this.deathrestTicks = this.MinDeathrestTicks + 1;
							this.adjustedDeathrestTicks = 240001f;
							foreach (CompDeathrestBindable compDeathrestBindable in this.BoundComps)
							{
								compDeathrestBindable.presenceTicks = this.deathrestTicks;
							}
							this.Wake();
						}
					};
					yield return command_Action2;
				}
			}
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Set capacity",
					action = delegate()
					{
						List<FloatMenuOption> list = new List<FloatMenuOption>();
						for (int i = 1; i <= 20; i++)
						{
							int newCap = i;
							list.Add(new FloatMenuOption(newCap.ToString(), delegate()
							{
								this.OffsetCapacity(newCap - this.deathrestCapacity, false);
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
						Find.WindowStack.Add(new FloatMenu(list));
					}
				};
			}
			yield break;
		}

		// Token: 0x06013E48 RID: 81480 RVA: 0x005E7F61 File Offset: 0x005E6161
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			yield return new StatDrawEntry(StatCategoryDefOf.Genetics, "DeathrestCapacity".Translate().CapitalizeFirst(), this.deathrestCapacity.ToString(), "DeathrestCapacityDesc".Translate(), 1010, null, null, false, false);
			yield break;
		}

		// Token: 0x06013E49 RID: 81481 RVA: 0x005E7F74 File Offset: 0x005E6174
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.deathrestCapacity, "deathrestCapacity", 1, false);
			Scribe_Values.Look<float>(ref this.adjustedDeathrestTicks, "adjustedDeathrestTicks", 0f, false);
			Scribe_Values.Look<int>(ref this.deathrestTicks, "deathrestTicks", 0, false);
			Scribe_Values.Look<bool>(ref this.autoWake, "autoWake", false, false);
			Scribe_Values.Look<int>(ref this.chamberThoughtIndex, "chamberThoughtIndex", -1, false);
			Scribe_Values.Look<bool>(ref this.notifiedWakeOK, "notifiedWakeOK", false, false);
			Scribe_Collections.Look<Thing>(ref this.boundBuildings, "boundBuildings", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.boundBuildings.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x0400DBD9 RID: 56281
		private int deathrestCapacity;

		// Token: 0x0400DBDA RID: 56282
		private List<Thing> boundBuildings = new List<Thing>();

		// Token: 0x0400DBDB RID: 56283
		public float adjustedDeathrestTicks;

		// Token: 0x0400DBDC RID: 56284
		public int deathrestTicks;

		// Token: 0x0400DBDD RID: 56285
		public bool autoWake;

		// Token: 0x0400DBDE RID: 56286
		public int chamberThoughtIndex = -1;

		// Token: 0x0400DBDF RID: 56287
		private bool notifiedWakeOK;

		// Token: 0x0400DBE0 RID: 56288
		[Unsaved(false)]
		private Gene_Hemogen cachedHemogenGene;

		// Token: 0x0400DBE1 RID: 56289
		[Unsaved(false)]
		private Need_Deathrest cachedDeathrestNeed;

		// Token: 0x0400DBE2 RID: 56290
		[Unsaved(false)]
		private List<CompDeathrestBindable> cachedBoundComps;

		// Token: 0x0400DBE3 RID: 56291
		[Unsaved(false)]
		private GeneGizmo_DeathrestCapacity gizmo;

		// Token: 0x0400DBE4 RID: 56292
		private const int InitialDeathrestCapacity = 1;

		// Token: 0x0400DBE5 RID: 56293
		public const int BaseDeathrestTicksWithoutInterruptedHediff = 240000;

		// Token: 0x0400DBE6 RID: 56294
		public const float PresencePercentRequiredToApply = 0.75f;

		// Token: 0x0400DBE7 RID: 56295
		private static readonly CachedTexture WakeCommandTex = new CachedTexture("UI/Gizmos/Wake");

		// Token: 0x0400DBE8 RID: 56296
		private static readonly CachedTexture AutoWakeCommandTex = new CachedTexture("UI/Gizmos/DeathrestAutoWake");

		// Token: 0x0400DBE9 RID: 56297
		private const int LessonDeathrestTicks = 200;

		// Token: 0x0400DBEA RID: 56298
		private const int SunlightCheckInterval = 150;
	}
}
