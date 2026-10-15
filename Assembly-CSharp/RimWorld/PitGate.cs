using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.Sound;

namespace RimWorld;

[StaticConstructorOnStartup]
public class PitGate : MapPortal
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass45_0
	{
		public PitGateIncidentDef incident;

		public PitGate _003C_003E4__this;

		internal void _003CGetGizmos_003Eb__0()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			using (IEnumerator<float> enumerator = DebugActionsUtility.PointsOptions(extended: false).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					_003C_003Ec__DisplayClass45_1 CS_0024_003C_003E8__locals4 = new _003C_003Ec__DisplayClass45_1
					{
						CS_0024_003C_003E8__locals1 = this,
						points = enumerator.Current
					};
					list.Add(new FloatMenuOption(CS_0024_003C_003E8__locals4.points + " Points", delegate
					{
						CS_0024_003C_003E8__locals4.CS_0024_003C_003E8__locals1._003C_003E4__this.TryFireIncident(CS_0024_003C_003E8__locals4.CS_0024_003C_003E8__locals1.incident, force: true, CS_0024_003C_003E8__locals4.points);
					}));
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		internal void _003CGetGizmos_003Eb__1()
		{
			_003C_003E4__this.TryFireIncident(incident, force: true);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass45_1
	{
		public float points;

		public _003C_003Ec__DisplayClass45_0 CS_0024_003C_003E8__locals1;

		internal void _003CGetGizmos_003Eb__2()
		{
			CS_0024_003C_003E8__locals1._003C_003E4__this.TryFireIncident(CS_0024_003C_003E8__locals1.incident, force: true, points);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__45 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PitGate _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

		private IEnumerator<PitGateIncidentDef> _003C_003E7__wrap2;

		Gizmo IEnumerator<Gizmo>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CGetGizmos_003Ed__45(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				PitGate CS_0024_003C_003E8__locals18 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((MapPortal)CS_0024_003C_003E8__locals18).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007a;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007a;
				case 2:
					_003C_003E1__state = -4;
					goto IL_016a;
				case 3:
					_003C_003E1__state = -1;
					goto IL_01c9;
				case 4:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_007a:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (CS_0024_003C_003E8__locals18.isCollapsing)
					{
						return false;
					}
					if (!DebugSettings.ShowDevGizmos)
					{
						return false;
					}
					_003C_003E7__wrap2 = CS_0024_003C_003E8__locals18.Incidents.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_016a;
					IL_016a:
					if (_003C_003E7__wrap2.MoveNext())
					{
						_003C_003Ec__DisplayClass45_0 CS_0024_003C_003E8__locals15 = new _003C_003Ec__DisplayClass45_0
						{
							_003C_003E4__this = CS_0024_003C_003E8__locals18,
							incident = _003C_003E7__wrap2.Current
						};
						Command_Action command_Action = new Command_Action
						{
							defaultLabel = "DEV: " + CS_0024_003C_003E8__locals15.incident.defName
						};
						if (CS_0024_003C_003E8__locals15.incident.usesThreatPoints)
						{
							command_Action.action = delegate
							{
								List<FloatMenuOption> list = new List<FloatMenuOption>();
								using (IEnumerator<float> enumerator = DebugActionsUtility.PointsOptions(extended: false).GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										_003C_003Ec__DisplayClass45_1 CS_0024_003C_003E8__locals13 = new _003C_003Ec__DisplayClass45_1
										{
											CS_0024_003C_003E8__locals1 = CS_0024_003C_003E8__locals15,
											points = enumerator.Current
										};
										list.Add(new FloatMenuOption(CS_0024_003C_003E8__locals13.points + " Points", delegate
										{
											CS_0024_003C_003E8__locals13.CS_0024_003C_003E8__locals1._003C_003E4__this.TryFireIncident(CS_0024_003C_003E8__locals13.CS_0024_003C_003E8__locals1.incident, force: true, CS_0024_003C_003E8__locals13.points);
										}));
									}
								}
								Find.WindowStack.Add(new FloatMenu(list));
							};
						}
						else
						{
							command_Action.action = delegate
							{
								CS_0024_003C_003E8__locals15._003C_003E4__this.TryFireIncident(CS_0024_003C_003E8__locals15.incident, force: true);
							};
						}
						_003C_003E2__current = command_Action;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
					if (CS_0024_003C_003E8__locals18.IsCoolingDown)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: End cooldown",
							action = delegate
							{
								CS_0024_003C_003E8__locals18.lastIncidentTick = -999999;
							}
						};
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_01c9;
					IL_01c9:
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Collapse Pit Gate",
						action = CS_0024_003C_003E8__locals18.BeginCollapsing
					};
					_003C_003E1__state = 4;
					return true;
				}
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__45 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__45(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	private const float IncidentMTBDays = 2f;

	private const float IncidentMinimumIntervalDays = 12f;

	private const int InitialIncidentDelayTicks = 450;

	private static readonly IntRange IncidentDelayTicksRange = new IntRange(1800, 3600);

	private static readonly IntRange CollapseDurationTicks = new IntRange(25000, 27500);

	private const int CollapseStageTwoDurationTicks = 3600;

	private const float StageTwoShakeAmount = 0.2f;

	private const float StageTwoShakeMTBSeconds = 2f;

	private const float OpenedScreenShakeMagnitude = 0.1f;

	private const int OpenedScreenShakeDuration = 120;

	public float pointsMultiplier = 1f;

	private int tickOpened = -999999;

	private int lastIncidentTick = -999999;

	private int nextIncidentTick = -999999;

	private int collapseTick = -999999;

	private bool isCollapsing;

	private PitGateIncidentWorker currentIncident;

	private int workerEnd = -999999;

	private int numIncidentsFired;

	private Sustainer collapseSustainer;

	private Effecter collapseEffecter1;

	private Effecter collapseEffecter2;

	private IEnumerable<PitGateIncidentDef> Incidents => DefDatabase<PitGateIncidentDef>.AllDefsListForReading;

	private bool HasActiveIncident => currentIncident != null;

	private bool IsCoolingDown
	{
		get
		{
			if (!((float)Find.TickManager.TicksGame < (float)lastIncidentTick + 720000f))
			{
				return Find.TickManager.TicksGame < tickOpened + 450;
			}
			return true;
		}
	}

	public bool IsCollapsing => isCollapsing;

	public int CollapseStage
	{
		get
		{
			if (collapseTick - Find.TickManager.TicksGame >= 3600)
			{
				return 1;
			}
			return 2;
		}
	}

	public int TicksUntilCollapse => collapseTick - Find.TickManager.TicksGame;

	public override bool AutoDraftOnEnter => true;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref pointsMultiplier, "pointsMultiplier", 0f);
		Scribe_Values.Look(ref tickOpened, "tickOpened", 0);
		Scribe_Values.Look(ref lastIncidentTick, "lastIncidentTick", 0);
		Scribe_Values.Look(ref nextIncidentTick, "nextIncidentTick", 0);
		Scribe_Values.Look(ref collapseTick, "collapseTick", 0);
		Scribe_Values.Look(ref isCollapsing, "isCollapsing", defaultValue: false);
		Scribe_Deep.Look(ref currentIncident, "currentWorker");
		Scribe_Values.Look(ref workerEnd, "workerEnd", 0);
		Scribe_Values.Look(ref numIncidentsFired, "numIncidentsFired", 0);
	}

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		base.SpawnSetup(map, respawningAfterLoad);
		if (!respawningAfterLoad)
		{
			tickOpened = Find.TickManager.TicksGame;
			EffecterDefOf.ImpactDustCloud.Spawn(base.Position, base.Map).Cleanup();
			Find.CameraDriver.shaker.DoShake(0.1f, 120);
			SoundDefOf.PitGateOpen.PlayOneShot(SoundInfo.InMap(this));
		}
	}

	protected override void Tick()
	{
		base.Tick();
		if (IsCollapsing)
		{
			if (CollapseStage == 1)
			{
				if (collapseEffecter1 == null)
				{
					collapseEffecter1 = EffecterDefOf.PitGateAboveGroundCollapseStage1.Spawn(this, base.Map);
				}
			}
			else if (CollapseStage == 2)
			{
				if (collapseSustainer == null)
				{
					collapseSustainer = SoundDefOf.PitGateCollapsing.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
				}
				collapseSustainer.Maintain();
				if (collapseEffecter2 == null)
				{
					collapseEffecter2 = EffecterDefOf.PitGateAboveGroundCollapseStage2.Spawn(this, base.Map);
				}
				if (Find.CurrentMap == base.Map && Rand.MTBEventOccurs(2f, 60f, 1f))
				{
					Find.CameraDriver.shaker.DoShake(0.2f);
				}
			}
			collapseEffecter1?.EffectTick(this, this);
			collapseEffecter2?.EffectTick(this, this);
			if (Find.TickManager.TicksGame >= collapseTick)
			{
				Collapse();
			}
			return;
		}
		if (HasActiveIncident)
		{
			if (Find.TickManager.TicksGame > workerEnd)
			{
				currentIncident = null;
				return;
			}
			currentIncident.Tick();
		}
		if (!IsCoolingDown && numIncidentsFired < 1)
		{
			FireRandomIncidentByWeight();
		}
		else if (!IsCoolingDown && Rand.MTBEventOccurs(2f, 60000f, 1f))
		{
			nextIncidentTick = Find.TickManager.TicksGame + IncidentDelayTicksRange.RandomInRange;
			Find.LetterStack.ReceiveLetter("EmergenceWarningLabel".Translate(), "EmergenceWarningText".Translate(), LetterDefOf.ThreatBig, this);
		}
		if (nextIncidentTick > 0 && Find.TickManager.TicksGame > nextIncidentTick)
		{
			FireRandomIncidentByWeight();
			nextIncidentTick = -999999;
		}
	}

	public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
	{
		if (mode == DestroyMode.Vanish)
		{
			base.Destroy(mode);
			return;
		}
		Map map = base.Map;
		base.Destroy(mode);
		EffecterDefOf.ImpactDustCloud.Spawn(base.Position, map).Cleanup();
		Messages.Message("MessagePitGateCollapsed".Translate(), new TargetInfo(base.Position, map), MessageTypeDefOf.NeutralEvent);
	}

	private void FireRandomIncidentByWeight()
	{
		List<PitGateIncidentDef> list = Incidents.ToList();
		while (list.Any())
		{
			PitGateIncidentDef item = list.RandomElementByWeightWithFallback((PitGateIncidentDef i) => i.baseChance);
			list.Remove(item);
			if (TryFireIncident(item))
			{
				break;
			}
		}
	}

	private bool TryFireIncident(PitGateIncidentDef def, bool force = false, float points = -1f)
	{
		PitGateIncidentWorker pitGateIncidentWorker = (PitGateIncidentWorker)Activator.CreateInstance(def.workerClass);
		pitGateIncidentWorker.pitGate = this;
		pitGateIncidentWorker.def = def;
		if (points < 0f)
		{
			points = StorytellerUtility.DefaultThreatPointsNow(base.Map);
		}
		points *= pointsMultiplier;
		if (force)
		{
			currentIncident = null;
			workerEnd = -999999;
		}
		if (pitGateIncidentWorker.CanFireNow())
		{
			currentIncident = pitGateIncidentWorker;
			currentIncident.Setup(points);
			workerEnd = Find.TickManager.TicksGame + def.durationRangeTicks.RandomInRange;
			Find.LetterStack.ReceiveLetter(def.letterLabel, def.letterText, LetterDefOf.ThreatBig, new TargetInfo(this));
			numIncidentsFired++;
			lastIncidentTick = Find.TickManager.TicksGame;
			return true;
		}
		return false;
	}

	public void BeginCollapsing()
	{
		int randomInRange = CollapseDurationTicks.RandomInRange;
		collapseTick = Find.TickManager.TicksGame + randomInRange;
		pocketMap?.GetComponent<UndercaveMapComponent>().Notify_BeginCollapsing(randomInRange);
		isCollapsing = true;
	}

	private void Collapse()
	{
		collapseSustainer.End();
		collapseEffecter2?.Cleanup();
		collapseEffecter2 = null;
		collapseEffecter1?.Cleanup();
		collapseEffecter1 = null;
		EffecterDefOf.PitGateAboveGroundCollapsed.Spawn(base.Position, base.Map);
		if (Find.CurrentMap == pocketMap)
		{
			SoundDefOf.UndercaveCollapsing_End.PlayOneShotOnCamera();
		}
		else
		{
			SoundDefOf.PitGateCollapsing_End.PlayOneShot(new TargetInfo(base.Position, base.Map));
		}
		if (base.PocketMapExists)
		{
			DamageInfo damageInfo = new DamageInfo(DamageDefOf.Crush, 99999f, 999f);
			for (int num = pocketMap.mapPawns.AllPawns.Count - 1; num >= 0; num--)
			{
				Pawn pawn = pocketMap.mapPawns.AllPawns[num];
				pawn.TakeDamage(damageInfo);
				if (!pawn.Dead)
				{
					pawn.Kill(damageInfo);
				}
			}
			PocketMapUtility.DestroyPocketMap(pocketMap);
		}
		Thing.allowDestroyNonDestroyable = true;
		Destroy(DestroyMode.Deconstruct);
		Thing.allowDestroyNonDestroyable = false;
	}

	public override bool IsEnterable(out string reason)
	{
		reason = "";
		if (Find.TickManager.TicksGame < tickOpened + 450)
		{
			reason = "PitGateSettling".Translate();
			return false;
		}
		if (HasActiveIncident && Find.TickManager.TicksGame < currentIncident.fireTick + currentIncident.def.disableEnteringTicks)
		{
			reason = currentIncident.def.disableEnteringReason.CapitalizeFirst();
			return false;
		}
		if (isCollapsing && !beenEntered)
		{
			reason = "PitGateCollapsing".Translate();
			return false;
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__45))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__45(-2)
		{
			_003C_003E4__this = this
		};
	}
}
