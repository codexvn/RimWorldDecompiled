using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002D79 RID: 11641
	public abstract class CompObelisk : ThingComp, IActivity
	{
		// Token: 0x170029A5 RID: 10661
		// (get) Token: 0x060105B6 RID: 66998 RVA: 0x004B89B4 File Offset: 0x004B6BB4
		protected CompProperties_Obelisk Props
		{
			get
			{
				return (CompProperties_Obelisk)this.props;
			}
		}

		// Token: 0x170029A6 RID: 10662
		// (get) Token: 0x060105B7 RID: 66999 RVA: 0x0004FC72 File Offset: 0x0004DE72
		protected virtual float InteractionEffectChanceMTBDays
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170029A7 RID: 10663
		// (get) Token: 0x060105B8 RID: 67000 RVA: 0x004B89C4 File Offset: 0x004B6BC4
		protected CompStudyUnlocks StudyUnlocks
		{
			get
			{
				CompStudyUnlocks result;
				if ((result = this.studyInt) == null)
				{
					result = (this.studyInt = this.parent.TryGetComp<CompStudyUnlocks>());
				}
				return result;
			}
		}

		// Token: 0x170029A8 RID: 10664
		// (get) Token: 0x060105B9 RID: 67001 RVA: 0x004B89F0 File Offset: 0x004B6BF0
		public CompActivity ActivityComp
		{
			get
			{
				CompActivity result;
				if ((result = this.activityInt) == null)
				{
					result = (this.activityInt = this.parent.TryGetComp<CompActivity>());
				}
				return result;
			}
		}

		// Token: 0x060105BA RID: 67002
		public abstract void TriggerInteractionEffect(Pawn interactor, bool triggeredByPlayer = false);

		// Token: 0x170029A9 RID: 10665
		// (get) Token: 0x060105BB RID: 67003 RVA: 0x004B8A1B File Offset: 0x004B6C1B
		public bool StudyFinished
		{
			get
			{
				return this.StudyUnlocks.Completed;
			}
		}

		// Token: 0x170029AA RID: 10666
		// (get) Token: 0x060105BC RID: 67004 RVA: 0x004B8A28 File Offset: 0x004B6C28
		public int StudyLevel
		{
			get
			{
				return this.StudyUnlocks.Progress;
			}
		}

		// Token: 0x170029AB RID: 10667
		// (get) Token: 0x060105BD RID: 67005 RVA: 0x004B8A35 File Offset: 0x004B6C35
		public bool Activated
		{
			get
			{
				return this.activated;
			}
		}

		// Token: 0x060105BE RID: 67006 RVA: 0x004B8A3D File Offset: 0x004B6C3D
		public override void Initialize(CompProperties props)
		{
			if (!ModLister.CheckAnomaly("CompObelisk"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.Initialize(props);
		}

		// Token: 0x060105BF RID: 67007 RVA: 0x004B8A5F File Offset: 0x004B6C5F
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && !this.parent.BeingTransportedOnGravship)
			{
				this.OnImpact();
			}
		}

		// Token: 0x060105C0 RID: 67008 RVA: 0x004B8A80 File Offset: 0x004B6C80
		public override void Notify_KilledLeavingsLeft(List<Thing> leavings)
		{
			if (!this.ActivityComp.Deactivated)
			{
				return;
			}
			foreach (Thing thing in leavings)
			{
				thing.Destroy(DestroyMode.Vanish);
			}
			leavings.Clear();
		}

		// Token: 0x060105C1 RID: 67009 RVA: 0x004B8AE0 File Offset: 0x004B6CE0
		public override void CompTick()
		{
			this.SoundTick();
			if (!this.warmupComplete && GenTicks.TicksGame >= this.delayedActionTick)
			{
				this.warmupComplete = true;
				Effecter effecter = this.obeliskWarmupEffecter;
				if (effecter != null)
				{
					effecter.Cleanup();
				}
				this.obeliskWarmupEffecter = null;
			}
			else if (this.activated && this.obeliskWarmupEffecter == null && !this.warmupComplete)
			{
				this.obeliskWarmupEffecter = EffecterDefOf.ObeliskActionWarmup.Spawn(this.parent, this.parent.MapHeld, 1f).Trigger(this.parent, this.parent, -1);
			}
			Effecter effecter2 = this.obeliskWarmupEffecter;
			if (effecter2 == null)
			{
				return;
			}
			effecter2.EffectTick(this.parent, this.parent);
		}

		// Token: 0x060105C2 RID: 67010 RVA: 0x004B8BAC File Offset: 0x004B6DAC
		private void OnImpact()
		{
			EffecterDefOf.ImpactDustCloud.Spawn(this.parent.Position, this.parent.Map, 1f).Cleanup();
			for (int i = 0; i < this.Props.rubbleFilthCount; i++)
			{
				IntVec3 c;
				if (CellFinder.TryFindRandomCellNear(this.parent.OccupiedRect().RandomCell, this.parent.Map, 2, null, out c, -1))
				{
					FilthMaker.TryMakeFilth(c, this.parent.Map, Rand.Bool ? ThingDefOf.Filth_Dirt : ThingDefOf.Filth_RubbleRock, 1, FilthSourceFlags.None, true);
				}
			}
		}

		// Token: 0x060105C3 RID: 67011 RVA: 0x004B8C4C File Offset: 0x004B6E4C
		public void Notify_InteractedTick(Pawn interactor, int delta)
		{
			if (this.activated)
			{
				return;
			}
			if (this.lastInteractionEffectTick + this.Props.interactionEffectCooldownDays * 60000 > Find.TickManager.TicksGame)
			{
				return;
			}
			if (!Rand.MTBEventOccurs(this.InteractionEffectChanceMTBDays, 60000f, (float)delta))
			{
				return;
			}
			this.TriggerInteractionEffect(interactor, false);
		}

		// Token: 0x060105C4 RID: 67012 RVA: 0x004B8CA4 File Offset: 0x004B6EA4
		public virtual void OnActivityActivated()
		{
			this.activated = true;
			this.delayedActionTick = GenTicks.TicksGame + 240;
		}

		// Token: 0x060105C5 RID: 67013 RVA: 0x000026EA File Offset: 0x000008EA
		public void OnPassive()
		{
		}

		// Token: 0x060105C6 RID: 67014 RVA: 0x00002501 File Offset: 0x00000701
		public bool ShouldGoPassive()
		{
			return false;
		}

		// Token: 0x060105C7 RID: 67015 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool CanBeSuppressed()
		{
			return true;
		}

		// Token: 0x060105C8 RID: 67016 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool CanActivate()
		{
			return true;
		}

		// Token: 0x060105C9 RID: 67017 RVA: 0x00002C42 File Offset: 0x00000E42
		public string ActivityTooltipExtra()
		{
			return null;
		}

		// Token: 0x060105CA RID: 67018 RVA: 0x004B8CC0 File Offset: 0x004B6EC0
		private void SoundTick()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			if (this.parent.Fogged())
			{
				Sustainer sustainer = this.stageOneSustainer;
				if (sustainer != null)
				{
					sustainer.End();
				}
				Sustainer sustainer2 = this.stageTwoSustainer;
				if (sustainer2 != null)
				{
					sustainer2.End();
				}
				Sustainer sustainer3 = this.stageThreeSustainer;
				if (sustainer3 == null)
				{
					return;
				}
				sustainer3.End();
				return;
			}
			else
			{
				float num = CompObelisk.SustainerStudiedStageCurve.Evaluate(this.ActivityComp.ActivityLevel);
				if (num > 0f)
				{
					if (this.stageOneSustainer == null || this.stageOneSustainer.Ended)
					{
						this.stageOneSustainer = SoundDefOf.ObeliskAmbientStageOne.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.PerTick));
					}
					this.stageOneSustainer.info.volumeFactor = Mathf.Min(num, 1f);
					this.stageOneSustainer.Maintain();
				}
				else
				{
					Sustainer sustainer4 = this.stageOneSustainer;
					if (sustainer4 != null)
					{
						sustainer4.End();
					}
				}
				if (num > 1f)
				{
					if (this.stageTwoSustainer == null || this.stageTwoSustainer.Ended)
					{
						this.stageTwoSustainer = SoundDefOf.ObeliskAmbientStageTwo.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.PerTick));
					}
					this.stageTwoSustainer.info.volumeFactor = Mathf.Min(num - 1f, 1f);
					this.stageTwoSustainer.Maintain();
				}
				else
				{
					Sustainer sustainer5 = this.stageTwoSustainer;
					if (sustainer5 != null)
					{
						sustainer5.End();
					}
				}
				if (num > 2f)
				{
					if (this.stageThreeSustainer == null || this.stageThreeSustainer.Ended)
					{
						this.stageThreeSustainer = SoundDefOf.ObeliskAmbientStageThree.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.PerTick));
					}
					this.stageThreeSustainer.info.volumeFactor = Mathf.Min(num - 2f, 1f);
					this.stageThreeSustainer.Maintain();
					return;
				}
				Sustainer sustainer6 = this.stageThreeSustainer;
				if (sustainer6 == null)
				{
					return;
				}
				sustainer6.End();
				return;
			}
		}

		// Token: 0x060105CB RID: 67019 RVA: 0x004B8EA0 File Offset: 0x004B70A0
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DebugSettings.ShowDevGizmos)
			{
				Command_Action command_Action = new Command_Action
				{
					defaultLabel = "DEV: Trigger interaction effect",
					action = delegate()
					{
						this.TriggerInteractionEffect(Find.CurrentMap.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>(), false);
					}
				};
				if (Find.CurrentMap.mapPawns.FreeColonistsSpawned.Count == 0)
				{
					command_Action.Disable("No colonists to interact with");
				}
				yield return command_Action;
			}
			yield break;
			yield break;
		}

		// Token: 0x060105CC RID: 67020 RVA: 0x004B8EB0 File Offset: 0x004B70B0
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.lastInteractionEffectTick, "lastInteractionEffectTick", 0, false);
			Scribe_Values.Look<bool>(ref this.activated, "activated", false, false);
			Scribe_Values.Look<int>(ref this.delayedActionTick, "delayedActionTick", int.MaxValue, false);
		}

		// Token: 0x0400AAB0 RID: 43696
		protected Effecter obeliskWarmupEffecter;

		// Token: 0x0400AAB1 RID: 43697
		protected const int WarmupDelayTicks = 240;

		// Token: 0x0400AAB2 RID: 43698
		protected bool warmupComplete;

		// Token: 0x0400AAB3 RID: 43699
		private int delayedActionTick = int.MaxValue;

		// Token: 0x0400AAB4 RID: 43700
		protected bool activated;

		// Token: 0x0400AAB5 RID: 43701
		protected int lastInteractionEffectTick = -9999999;

		// Token: 0x0400AAB6 RID: 43702
		private Sustainer stageOneSustainer;

		// Token: 0x0400AAB7 RID: 43703
		private Sustainer stageTwoSustainer;

		// Token: 0x0400AAB8 RID: 43704
		private Sustainer stageThreeSustainer;

		// Token: 0x0400AAB9 RID: 43705
		private CompStudyUnlocks studyInt;

		// Token: 0x0400AABA RID: 43706
		private CompActivity activityInt;

		// Token: 0x0400AABB RID: 43707
		private static readonly SimpleCurve SustainerStudiedStageCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(0.1f, 1f),
				true
			},
			{
				new CurvePoint(0.5f, 2f),
				true
			},
			{
				new CurvePoint(1f, 3f),
				true
			}
		};
	}
}
