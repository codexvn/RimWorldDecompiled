using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020008F1 RID: 2289
	public class UnnaturalCorpse : Corpse
	{
		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x06003B16 RID: 15126 RVA: 0x00138EDC File Offset: 0x001370DC
		public CompForbiddable Forbiddable
		{
			get
			{
				CompForbiddable result;
				if ((result = this.compForbidInt) == null)
				{
					result = (this.compForbidInt = base.GetComp<CompForbiddable>());
				}
				return result;
			}
		}

		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x06003B17 RID: 15127 RVA: 0x00138F02 File Offset: 0x00137102
		public UnnaturalCorpseTracker Tracker
		{
			get
			{
				return this.tracker;
			}
		}

		// Token: 0x06003B18 RID: 15128 RVA: 0x00138F0A File Offset: 0x0013710A
		public UnnaturalCorpse()
		{
			this.innerContainer.contentsLookMode = LookMode.Deep;
		}

		// Token: 0x06003B19 RID: 15129 RVA: 0x00138F1E File Offset: 0x0013711E
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Destroy(mode);
			UnnaturalCorpseTracker unnaturalCorpseTracker = this.tracker;
			if (unnaturalCorpseTracker == null)
			{
				return;
			}
			unnaturalCorpseTracker.Notify_CorpseDestroyed();
		}

		// Token: 0x06003B1A RID: 15130 RVA: 0x00138F37 File Offset: 0x00137137
		public override void PostMake()
		{
			base.PostMake();
			this.lastTeleport = GenTicks.TicksGame;
			this.Forbiddable.Forbidden = true;
			this.TryGetComp<CompRottable>().disabled = true;
		}

		// Token: 0x06003B1B RID: 15131 RVA: 0x00138F64 File Offset: 0x00137164
		protected override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.disappear)
			{
				this.DoDisappear();
				return;
			}
			if (base.Destroyed)
			{
				return;
			}
			if (base.ParentHolder != null && !(base.ParentHolder is Map))
			{
				return;
			}
			if (Rand.MTBEventOccurs(24f, 2500f, (float)delta))
			{
				this.teleport = true;
			}
			IntVec3 intVec;
			if (this.teleport && this.IsOutsideView() && AnomalyUtility.TryGetNearbyUnseenCell(this.tracker.Haunted, out intVec))
			{
				if (base.MapHeld != this.tracker.Haunted.MapHeld)
				{
					SkipUtility.SkipTo(this, intVec, this.tracker.Haunted.MapHeld);
				}
				else
				{
					base.Position = intVec;
				}
				this.teleport = false;
				this.lastTeleport = GenTicks.TicksGame;
				base.Map.listerHaulables.Notify_Unforbidden(this);
			}
		}

		// Token: 0x06003B1C RID: 15132 RVA: 0x00139040 File Offset: 0x00137240
		public override void TickRare()
		{
			base.TickRareInt();
		}

		// Token: 0x06003B1D RID: 15133 RVA: 0x00139048 File Offset: 0x00137248
		public override void Notify_Studied(Pawn studier, float amount, KnowledgeCategoryDef category = null)
		{
			base.Notify_Studied(studier, amount, category);
			if (!category.IsAnomalyResearchCategory())
			{
				return;
			}
			this.tracker.Notify_Studied(studier, amount);
		}

		// Token: 0x06003B1E RID: 15134 RVA: 0x0013906C File Offset: 0x0013726C
		public void DoStudiedDeactivation(Pawn interactor)
		{
			IntVec3 positionHeld = base.PositionHeld;
			Map mapHeld = base.MapHeld;
			this.tracker.Notify_Finished();
			EffecterDefOf.MeatExplosion.Spawn(base.PositionHeld, base.MapHeld, 1f).Cleanup();
			this.Destroy(DestroyMode.Vanish);
			Thing t = GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Shard, null), positionHeld, mapHeld, WipeMode.Vanish);
			TaggedString label = "UnnaturalCorpseDeactivatedLetter".Translate();
			TaggedString taggedString = "UnnaturalCorpseDeactivatedLetterDesc".Translate(interactor.Named("PAWN"));
			taggedString += "\n\n" + "UnnaturalCorpseDeactivatedLetterDescAppended".Translate(this.tracker.Haunted.Named("PAWN"));
			Find.LetterStack.ReceiveLetter(label, taggedString, LetterDefOf.PositiveEvent, t, null, null, null, null, 0, true);
			Hediff hediff;
			if (this.tracker.Haunted.health.hediffSet.TryGetHediff(HediffDefOf.CorpseTorment, out hediff))
			{
				this.tracker.Haunted.health.RemoveHediff(hediff);
			}
			Find.Anomaly.RemoveCorpseTracker(this.tracker.Haunted);
		}

		// Token: 0x06003B1F RID: 15135 RVA: 0x00139190 File Offset: 0x00137390
		public void DoDisappear()
		{
			this.disappear = true;
			Thing spawnedParentOrMe = base.SpawnedParentOrMe;
			Building_Casket building_Casket = spawnedParentOrMe as Building_Casket;
			if (building_Casket != null)
			{
				this.SendDisappearedLetter();
				building_Casket.EjectContents();
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			if (spawnedParentOrMe != this)
			{
				this.SendDisappearedLetter();
				Thing thing;
				base.ParentHolder.GetDirectlyHeldThings().TryDrop(this, ThingPlaceMode.Near, 1, out thing, null, null);
				SkipUtility.SkipDeSpawn(this);
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			if (this.IsOutsideView())
			{
				this.SendDisappearedLetter();
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06003B20 RID: 15136 RVA: 0x00139210 File Offset: 0x00137410
		private void SendDisappearedLetter()
		{
			TaggedString label = "LetterLabelCorpseDisappeared".Translate();
			TaggedString taggedString = "LetterCorpseDisappeared".Translate();
			if (!this.tracker.Haunted.DestroyedOrNull() && !this.tracker.Haunted.Dead)
			{
				taggedString += " " + "LetterCorpseDisappearedPawnAliveAppend".Translate(this.tracker.Haunted.Named("PAWN"));
			}
			Find.LetterStack.ReceiveLetter(label, taggedString, LetterDefOf.NeutralEvent, null, 0, true);
		}

		// Token: 0x06003B21 RID: 15137 RVA: 0x0013929B File Offset: 0x0013749B
		public void LinkToTracker(UnnaturalCorpseTracker newTracker)
		{
			if (this.tracker != null)
			{
				Log.ErrorOnce("mysterious corpse " + this.Label + " was linked to a new tracker when it still had one assigned.", 62521241);
			}
			this.tracker = newTracker;
		}

		// Token: 0x06003B22 RID: 15138 RVA: 0x001392CB File Offset: 0x001374CB
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			Gizmo gizmo2 = AnomalyUtility.OpenCodexGizmo(this);
			if (gizmo2 != null)
			{
				yield return gizmo2;
			}
			if (!DebugSettings.ShowDevGizmos)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = "DEV: Disappear",
				action = delegate()
				{
					Find.Anomaly.DevRemoveUnnaturalCorpse(this.tracker.Haunted);
				}
			};
			if (!this.teleport)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Teleport",
					action = delegate()
					{
						this.teleport = true;
					}
				};
			}
			yield return new Command_Action
			{
				defaultLabel = "DEV: Mental break",
				action = delegate()
				{
					this.tracker.TryTriggerMentalBreak();
				}
			};
			yield return new Command_Action
			{
				defaultLabel = "DEV: Awake",
				action = delegate()
				{
					this.tracker.DevAwaken();
				}
			};
			if (!this.tracker.CanDestroyViaResearch)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Unlock deactivation",
					action = delegate()
					{
						this.tracker.DevUnlockDeactivation();
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x06003B23 RID: 15139 RVA: 0x001392DC File Offset: 0x001374DC
		private bool IsOutsideView()
		{
			return (!base.SpawnedOrAnyParentSpawned || !base.MapHeld.reservationManager.IsReservedByAnyoneOf(this, Faction.OfPlayer)) && (Find.CurrentMap != base.MapHeld || !Find.CameraDriver.CurrentViewRect.ExpandedBy(1).Contains(base.PositionHeld));
		}

		// Token: 0x06003B24 RID: 15140 RVA: 0x00139344 File Offset: 0x00137544
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<UnnaturalCorpseTracker>(ref this.tracker, "tracker", false);
			Scribe_Values.Look<bool>(ref this.disappear, "disappears", false, false);
			Scribe_Values.Look<bool>(ref this.teleport, "teleport", false, false);
			Scribe_Values.Look<int>(ref this.lastTeleport, "lastTeleport", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.tracker == null)
			{
				Log.Error("Failed to find corpse tracker, this should not happen. Destroying corpse.");
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x04002AD4 RID: 10964
		private UnnaturalCorpseTracker tracker;

		// Token: 0x04002AD5 RID: 10965
		private bool disappear;

		// Token: 0x04002AD6 RID: 10966
		private bool teleport;

		// Token: 0x04002AD7 RID: 10967
		private int lastTeleport;

		// Token: 0x04002AD8 RID: 10968
		private CompForbiddable compForbidInt;

		// Token: 0x04002AD9 RID: 10969
		private const int MTBTeleportHours = 24;
	}
}
