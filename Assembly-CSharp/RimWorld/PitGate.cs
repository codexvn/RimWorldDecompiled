using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002B0F RID: 11023
	[StaticConstructorOnStartup]
	public class PitGate : MapPortal
	{
		// Token: 0x17002682 RID: 9858
		// (get) Token: 0x0600F627 RID: 63015 RVA: 0x004777C7 File Offset: 0x004759C7
		private IEnumerable<PitGateIncidentDef> Incidents
		{
			get
			{
				return DefDatabase<PitGateIncidentDef>.AllDefsListForReading;
			}
		}

		// Token: 0x17002683 RID: 9859
		// (get) Token: 0x0600F628 RID: 63016 RVA: 0x004777CE File Offset: 0x004759CE
		private bool HasActiveIncident
		{
			get
			{
				return this.currentIncident != null;
			}
		}

		// Token: 0x17002684 RID: 9860
		// (get) Token: 0x0600F629 RID: 63017 RVA: 0x004777D9 File Offset: 0x004759D9
		private bool IsCoolingDown
		{
			get
			{
				return (float)Find.TickManager.TicksGame < (float)this.lastIncidentTick + 720000f || Find.TickManager.TicksGame < this.tickOpened + 450;
			}
		}

		// Token: 0x17002685 RID: 9861
		// (get) Token: 0x0600F62A RID: 63018 RVA: 0x0047780F File Offset: 0x00475A0F
		public bool IsCollapsing
		{
			get
			{
				return this.isCollapsing;
			}
		}

		// Token: 0x17002686 RID: 9862
		// (get) Token: 0x0600F62B RID: 63019 RVA: 0x00477817 File Offset: 0x00475A17
		public int CollapseStage
		{
			get
			{
				if (this.collapseTick - Find.TickManager.TicksGame >= 3600)
				{
					return 1;
				}
				return 2;
			}
		}

		// Token: 0x17002687 RID: 9863
		// (get) Token: 0x0600F62C RID: 63020 RVA: 0x00477834 File Offset: 0x00475A34
		public int TicksUntilCollapse
		{
			get
			{
				return this.collapseTick - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17002688 RID: 9864
		// (get) Token: 0x0600F62D RID: 63021 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool AutoDraftOnEnter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600F62E RID: 63022 RVA: 0x00477848 File Offset: 0x00475A48
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.pointsMultiplier, "pointsMultiplier", 0f, false);
			Scribe_Values.Look<int>(ref this.tickOpened, "tickOpened", 0, false);
			Scribe_Values.Look<int>(ref this.lastIncidentTick, "lastIncidentTick", 0, false);
			Scribe_Values.Look<int>(ref this.nextIncidentTick, "nextIncidentTick", 0, false);
			Scribe_Values.Look<int>(ref this.collapseTick, "collapseTick", 0, false);
			Scribe_Values.Look<bool>(ref this.isCollapsing, "isCollapsing", false, false);
			Scribe_Deep.Look<PitGateIncidentWorker>(ref this.currentIncident, "currentWorker", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.workerEnd, "workerEnd", 0, false);
			Scribe_Values.Look<int>(ref this.numIncidentsFired, "numIncidentsFired", 0, false);
		}

		// Token: 0x0600F62F RID: 63023 RVA: 0x00477904 File Offset: 0x00475B04
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.tickOpened = Find.TickManager.TicksGame;
				EffecterDefOf.ImpactDustCloud.Spawn(base.Position, base.Map, 1f).Cleanup();
				Find.CameraDriver.shaker.DoShake(0.1f, 120);
				SoundDefOf.PitGateOpen.PlayOneShot(SoundInfo.InMap(this, MaintenanceType.None));
			}
		}

		// Token: 0x0600F630 RID: 63024 RVA: 0x00477978 File Offset: 0x00475B78
		protected override void Tick()
		{
			base.Tick();
			if (this.IsCollapsing)
			{
				if (this.CollapseStage == 1)
				{
					if (this.collapseEffecter1 == null)
					{
						this.collapseEffecter1 = EffecterDefOf.PitGateAboveGroundCollapseStage1.Spawn(this, base.Map, 1f);
					}
				}
				else if (this.CollapseStage == 2)
				{
					if (this.collapseSustainer == null)
					{
						this.collapseSustainer = SoundDefOf.PitGateCollapsing.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
					}
					this.collapseSustainer.Maintain();
					if (this.collapseEffecter2 == null)
					{
						this.collapseEffecter2 = EffecterDefOf.PitGateAboveGroundCollapseStage2.Spawn(this, base.Map, 1f);
					}
					if (Find.CurrentMap == base.Map && Rand.MTBEventOccurs(2f, 60f, 1f))
					{
						Find.CameraDriver.shaker.DoShake(0.2f);
					}
				}
				Effecter effecter = this.collapseEffecter1;
				if (effecter != null)
				{
					effecter.EffectTick(this, this);
				}
				Effecter effecter2 = this.collapseEffecter2;
				if (effecter2 != null)
				{
					effecter2.EffectTick(this, this);
				}
				if (Find.TickManager.TicksGame >= this.collapseTick)
				{
					this.Collapse();
				}
				return;
			}
			if (this.HasActiveIncident)
			{
				if (Find.TickManager.TicksGame > this.workerEnd)
				{
					this.currentIncident = null;
					return;
				}
				this.currentIncident.Tick();
			}
			if (!this.IsCoolingDown && this.numIncidentsFired < 1)
			{
				this.FireRandomIncidentByWeight();
			}
			else if (!this.IsCoolingDown && Rand.MTBEventOccurs(2f, 60000f, 1f))
			{
				this.nextIncidentTick = Find.TickManager.TicksGame + PitGate.IncidentDelayTicksRange.RandomInRange;
				Find.LetterStack.ReceiveLetter("EmergenceWarningLabel".Translate(), "EmergenceWarningText".Translate(), LetterDefOf.ThreatBig, this, null, null, null, null, 0, true);
			}
			if (this.nextIncidentTick > 0 && Find.TickManager.TicksGame > this.nextIncidentTick)
			{
				this.FireRandomIncidentByWeight();
				this.nextIncidentTick = -999999;
			}
		}

		// Token: 0x0600F631 RID: 63025 RVA: 0x00477B94 File Offset: 0x00475D94
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode == DestroyMode.Vanish)
			{
				base.Destroy(mode);
				return;
			}
			Map map = base.Map;
			base.Destroy(mode);
			EffecterDefOf.ImpactDustCloud.Spawn(base.Position, map, 1f).Cleanup();
			Messages.Message("MessagePitGateCollapsed".Translate(), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x0600F632 RID: 63026 RVA: 0x00477C04 File Offset: 0x00475E04
		private void FireRandomIncidentByWeight()
		{
			List<PitGateIncidentDef> list = this.Incidents.ToList<PitGateIncidentDef>();
			while (list.Any<PitGateIncidentDef>())
			{
				PitGateIncidentDef pitGateIncidentDef = list.RandomElementByWeightWithFallback((PitGateIncidentDef i) => i.baseChance, null);
				list.Remove(pitGateIncidentDef);
				if (this.TryFireIncident(pitGateIncidentDef, false, -1f))
				{
					break;
				}
			}
		}

		// Token: 0x0600F633 RID: 63027 RVA: 0x00477C68 File Offset: 0x00475E68
		private bool TryFireIncident(PitGateIncidentDef def, bool force = false, float points = -1f)
		{
			PitGateIncidentWorker pitGateIncidentWorker = (PitGateIncidentWorker)Activator.CreateInstance(def.workerClass);
			pitGateIncidentWorker.pitGate = this;
			pitGateIncidentWorker.def = def;
			if (points < 0f)
			{
				points = StorytellerUtility.DefaultThreatPointsNow(base.Map);
			}
			points *= this.pointsMultiplier;
			if (force)
			{
				this.currentIncident = null;
				this.workerEnd = -999999;
			}
			if (pitGateIncidentWorker.CanFireNow())
			{
				this.currentIncident = pitGateIncidentWorker;
				this.currentIncident.Setup(points);
				this.workerEnd = Find.TickManager.TicksGame + def.durationRangeTicks.RandomInRange;
				Find.LetterStack.ReceiveLetter(def.letterLabel, def.letterText, LetterDefOf.ThreatBig, new TargetInfo(this), null, null, null, null, 0, true);
				this.numIncidentsFired++;
				this.lastIncidentTick = Find.TickManager.TicksGame;
				return true;
			}
			return false;
		}

		// Token: 0x0600F634 RID: 63028 RVA: 0x00477D5C File Offset: 0x00475F5C
		public void BeginCollapsing()
		{
			int randomInRange = PitGate.CollapseDurationTicks.RandomInRange;
			this.collapseTick = Find.TickManager.TicksGame + randomInRange;
			Map pocketMap = this.pocketMap;
			if (pocketMap != null)
			{
				pocketMap.GetComponent<UndercaveMapComponent>().Notify_BeginCollapsing(randomInRange);
			}
			this.isCollapsing = true;
		}

		// Token: 0x0600F635 RID: 63029 RVA: 0x00477DA8 File Offset: 0x00475FA8
		private void Collapse()
		{
			this.collapseSustainer.End();
			Effecter effecter = this.collapseEffecter2;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.collapseEffecter2 = null;
			Effecter effecter2 = this.collapseEffecter1;
			if (effecter2 != null)
			{
				effecter2.Cleanup();
			}
			this.collapseEffecter1 = null;
			EffecterDefOf.PitGateAboveGroundCollapsed.Spawn(base.Position, base.Map, 1f);
			if (Find.CurrentMap == this.pocketMap)
			{
				SoundDefOf.UndercaveCollapsing_End.PlayOneShotOnCamera(null);
			}
			else
			{
				SoundDefOf.PitGateCollapsing_End.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			}
			if (base.PocketMapExists)
			{
				DamageInfo damageInfo = new DamageInfo(DamageDefOf.Crush, 99999f, 999f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
				for (int i = this.pocketMap.mapPawns.AllPawns.Count - 1; i >= 0; i--)
				{
					Pawn pawn = this.pocketMap.mapPawns.AllPawns[i];
					pawn.TakeDamage(damageInfo);
					if (!pawn.Dead)
					{
						pawn.Kill(new DamageInfo?(damageInfo), null);
					}
				}
				PocketMapUtility.DestroyPocketMap(this.pocketMap);
			}
			Thing.allowDestroyNonDestroyable = true;
			this.Destroy(DestroyMode.Deconstruct);
			Thing.allowDestroyNonDestroyable = false;
		}

		// Token: 0x0600F636 RID: 63030 RVA: 0x00477EEC File Offset: 0x004760EC
		public override bool IsEnterable(out string reason)
		{
			reason = "";
			if (Find.TickManager.TicksGame < this.tickOpened + 450)
			{
				reason = "PitGateSettling".Translate();
				return false;
			}
			if (this.HasActiveIncident && Find.TickManager.TicksGame < this.currentIncident.fireTick + this.currentIncident.def.disableEnteringTicks)
			{
				reason = this.currentIncident.def.disableEnteringReason.CapitalizeFirst();
				return false;
			}
			if (this.isCollapsing && !this.beenEntered)
			{
				reason = "PitGateCollapsing".Translate();
				return false;
			}
			return true;
		}

		// Token: 0x0600F637 RID: 63031 RVA: 0x00477F98 File Offset: 0x00476198
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.isCollapsing)
			{
				yield break;
			}
			if (!DebugSettings.ShowDevGizmos)
			{
				yield break;
			}
			using (IEnumerator<PitGateIncidentDef> enumerator2 = this.Incidents.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					PitGateIncidentDef incident = enumerator2.Current;
					Command_Action command_Action = new Command_Action();
					command_Action.defaultLabel = "DEV: " + incident.defName;
					if (incident.usesThreatPoints)
					{
						command_Action.action = delegate()
						{
							List<FloatMenuOption> list = new List<FloatMenuOption>();
							using (IEnumerator<float> enumerator3 = DebugActionsUtility.PointsOptions(false).GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									float points = enumerator3.Current;
									list.Add(new FloatMenuOption(points.ToString() + " Points", delegate()
									{
										this.TryFireIncident(incident, true, points);
									}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
								}
							}
							Find.WindowStack.Add(new FloatMenu(list));
						};
					}
					else
					{
						command_Action.action = delegate()
						{
							this.TryFireIncident(incident, true, -1f);
						};
					}
					yield return command_Action;
				}
			}
			IEnumerator<PitGateIncidentDef> enumerator2 = null;
			if (this.IsCoolingDown)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: End cooldown",
					action = delegate()
					{
						this.lastIncidentTick = -999999;
					}
				};
			}
			yield return new Command_Action
			{
				defaultLabel = "DEV: Collapse Pit Gate",
				action = new Action(this.BeginCollapsing)
			};
			yield break;
			yield break;
		}

		// Token: 0x0400A204 RID: 41476
		private const float IncidentMTBDays = 2f;

		// Token: 0x0400A205 RID: 41477
		private const float IncidentMinimumIntervalDays = 12f;

		// Token: 0x0400A206 RID: 41478
		private const int InitialIncidentDelayTicks = 450;

		// Token: 0x0400A207 RID: 41479
		private static readonly IntRange IncidentDelayTicksRange = new IntRange(1800, 3600);

		// Token: 0x0400A208 RID: 41480
		private static readonly IntRange CollapseDurationTicks = new IntRange(25000, 27500);

		// Token: 0x0400A209 RID: 41481
		private const int CollapseStageTwoDurationTicks = 3600;

		// Token: 0x0400A20A RID: 41482
		private const float StageTwoShakeAmount = 0.2f;

		// Token: 0x0400A20B RID: 41483
		private const float StageTwoShakeMTBSeconds = 2f;

		// Token: 0x0400A20C RID: 41484
		private const float OpenedScreenShakeMagnitude = 0.1f;

		// Token: 0x0400A20D RID: 41485
		private const int OpenedScreenShakeDuration = 120;

		// Token: 0x0400A20E RID: 41486
		public float pointsMultiplier = 1f;

		// Token: 0x0400A20F RID: 41487
		private int tickOpened = -999999;

		// Token: 0x0400A210 RID: 41488
		private int lastIncidentTick = -999999;

		// Token: 0x0400A211 RID: 41489
		private int nextIncidentTick = -999999;

		// Token: 0x0400A212 RID: 41490
		private int collapseTick = -999999;

		// Token: 0x0400A213 RID: 41491
		private bool isCollapsing;

		// Token: 0x0400A214 RID: 41492
		private PitGateIncidentWorker currentIncident;

		// Token: 0x0400A215 RID: 41493
		private int workerEnd = -999999;

		// Token: 0x0400A216 RID: 41494
		private int numIncidentsFired;

		// Token: 0x0400A217 RID: 41495
		private Sustainer collapseSustainer;

		// Token: 0x0400A218 RID: 41496
		private Effecter collapseEffecter1;

		// Token: 0x0400A219 RID: 41497
		private Effecter collapseEffecter2;
	}
}
