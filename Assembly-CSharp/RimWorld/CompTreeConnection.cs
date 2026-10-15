using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

[StaticConstructorOnStartup]
public class CompTreeConnection : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__74 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompTreeConnection _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__74(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			CompTreeConnection CS_0024_003C_003E8__locals16 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals16.Connected)
				{
					Command_Action command_Action = new Command_Action
					{
						defaultLabel = "ChangeMode".Translate(),
						defaultDesc = "ChangeModeDesc".Translate(CS_0024_003C_003E8__locals16.parent.Named("TREE")),
						icon = ((CS_0024_003C_003E8__locals16.Mode == null) ? ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeDryads") : Widgets.GetIconFor(CS_0024_003C_003E8__locals16.Mode.pawnKindDef.race, null, null, null)),
						action = delegate
						{
							Find.WindowStack.Add(new Dialog_ChangeDryadCaste(CS_0024_003C_003E8__locals16.parent));
						}
					};
					if (!CS_0024_003C_003E8__locals16.ConnectedPawn.Spawned || CS_0024_003C_003E8__locals16.ConnectedPawn.Map != CS_0024_003C_003E8__locals16.parent.Map)
					{
						command_Action.Disable("ConnectedPawnAway".Translate(CS_0024_003C_003E8__locals16.ConnectedPawn.Named("PAWN")));
					}
					_003C_003E2__current = command_Action;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_015e;
			case 1:
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals16.pruningGizmo == null)
				{
					CS_0024_003C_003E8__locals16.pruningGizmo = new Gizmo_PruningConfig(CS_0024_003C_003E8__locals16);
				}
				_003C_003E2__current = CS_0024_003C_003E8__locals16.pruningGizmo;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				goto IL_015e;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Connection strength -10%",
					action = delegate
					{
						CS_0024_003C_003E8__locals16.ConnectionStrength -= 0.1f;
					}
				};
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Connection strength +10%",
					action = delegate
					{
						CS_0024_003C_003E8__locals16.ConnectionStrength += 0.1f;
					}
				};
				_003C_003E1__state = 5;
				return true;
			case 5:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_015e:
				if (DebugSettings.ShowDevGizmos)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Spawn dryad",
						action = delegate
						{
							CS_0024_003C_003E8__locals16.SpawnDryad();
						}
					};
					_003C_003E1__state = 3;
					return true;
				}
				break;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__74 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__74(0)
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

	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__79 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompTreeConnection _003C_003E4__this;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
		public _003CSpecialDisplayStats_003Ed__79(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			CompTreeConnection compTreeConnection = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.BasicsNonPawn, "ConnectedPawn".Translate(), (compTreeConnection.ConnectedPawn != null) ? compTreeConnection.ConnectedPawn.NameFullColored : "Nobody".Translate(), "ConnectedPawnDesc".Translate(1800000.ToStringTicksToPeriod().Named("DURATION"), compTreeConnection.parent.Named("TREE")), 6010, null, Gen.YieldSingle(new Dialog_InfoCard.Hyperlink(compTreeConnection.ConnectedPawn)));
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				if (compTreeConnection.Connected)
				{
					if (compTreeConnection.Mode != null)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.BasicsNonPawn, "GauranlenTreeMode".Translate(), compTreeConnection.currentMode.LabelCap, "GauranlenTreeModeDesc".Translate() + "\n\n" + compTreeConnection.currentMode.LabelCap + ": " + compTreeConnection.currentMode.Description, 5990, null, Gen.YieldSingle(new Dialog_InfoCard.Hyperlink(compTreeConnection.currentMode.pawnKindDef.race)));
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_0191;
				}
				goto IL_01e3;
			case 2:
				_003C_003E1__state = -1;
				goto IL_0191;
			case 3:
				_003C_003E1__state = -1;
				goto IL_01e3;
			case 4:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_0191:
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.BasicsNonPawn, "ConnectionStrength".Translate(), compTreeConnection.ConnectionStrength.ToStringPercent(), "ConnectionStrengthDesc".Translate(), 6000);
				_003C_003E1__state = 3;
				return true;
				IL_01e3:
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.BasicsNonPawn, "MaximumDryads".Translate(), compTreeConnection.MaxDryads.ToString(), "MaximumDryadsDesc".Translate() + "\n\n" + compTreeConnection.ConnectionStrengthToMaxDryadsDesc(), 5980);
				_003C_003E1__state = 4;
				return true;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__79 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__79(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	private Pawn connectedPawn;

	private int nextUntornTick = -1;

	private int spawnTick = -1;

	private float connectionStrength;

	private List<Pawn> dryads = new List<Pawn>();

	private int lastPrunedTick;

	private float desiredConnectionStrength = 0.5f;

	private GauranlenTreeModeDef currentMode;

	public Thing gaumakerPod;

	public GauranlenTreeModeDef desiredMode;

	private Material cachedPodMat;

	private Effecter leafEffecter;

	private Gizmo_PruningConfig pruningGizmo;

	private const int ConnectionTornDurationTicks = 1800000;

	private const int CheckPodSpawnInterval = 300;

	public const int DryadsToCreatePod = 3;

	private float TimeBetweenAutoPruning = 10000f;

	private const float PruningConnectionStrengthDithering = 0.03f;

	private const float PruningSpeedFactor_DisabledSkill = 0.75f;

	private List<Pawn> tmpDryads = new List<Pawn>();

	public CompProperties_TreeConnection Props => (CompProperties_TreeConnection)props;

	public Pawn ConnectedPawn => connectedPawn;

	public bool Connected => ConnectedPawn != null;

	public bool ConnectionTorn => nextUntornTick >= Find.TickManager.TicksGame;

	public bool HasProductionMode => desiredMode != null;

	public int UntornInDurationTicks => nextUntornTick - Find.TickManager.TicksGame;

	public GauranlenTreeModeDef Mode => currentMode;

	public PawnKindDef DryadKind => Mode?.pawnKindDef ?? PawnKindDefOf.Dryad_Basic;

	public int MaxDryads
	{
		get
		{
			if (!Connected)
			{
				return Props.maxDryadsWild;
			}
			return (int)Props.maxDryadsPerConnectionStrengthCurve.Evaluate(ConnectionStrength);
		}
	}

	private int SpawningDurationTicks => (int)(60000f * Props.spawnDays);

	public float DesiredConnectionStrength
	{
		get
		{
			return desiredConnectionStrength;
		}
		set
		{
			desiredConnectionStrength = Mathf.Clamp01(value);
		}
	}

	public float ConnectionStrength
	{
		get
		{
			return connectionStrength;
		}
		set
		{
			connectionStrength = Mathf.Clamp01(value);
		}
	}

	private Material PodMat
	{
		get
		{
			if (cachedPodMat == null)
			{
				cachedPodMat = MaterialPool.MatFrom("Things/Building/Misc/DryadFormingPod/DryadFormingPod", ShaderDatabase.Cutout);
			}
			return cachedPodMat;
		}
	}

	private List<Thing> BuildingsReducingConnectionStrength => GauranlenUtility.BuildingsAffectingConnectionStrengthAt(parent.Position, parent.Map, Props);

	public float ConnectionStrengthLossPerDay
	{
		get
		{
			float num = Props.connectionLossPerLevelCurve.Evaluate(ConnectionStrength);
			List<Thing> buildingsReducingConnectionStrength = BuildingsReducingConnectionStrength;
			if (parent.Spawned && buildingsReducingConnectionStrength.Any())
			{
				num += Props.connectionLossDailyPerBuildingDistanceCurve.Evaluate(ClosestDistanceToBlockingBuilding(buildingsReducingConnectionStrength));
			}
			return num;
		}
	}

	public float ConnectionStrengthGainPerHourOfPruning
	{
		get
		{
			float connectionStrengthGainPerHourPruningBase = Props.connectionStrengthGainPerHourPruningBase;
			connectionStrengthGainPerHourPruningBase = ((!StatDefOf.PruningSpeed.Worker.IsDisabledFor(ConnectedPawn)) ? (connectionStrengthGainPerHourPruningBase * ConnectedPawn.GetStatValue(StatDefOf.PruningSpeed)) : (connectionStrengthGainPerHourPruningBase * 0.75f));
			if (Props.connectionStrengthGainPerPlantSkill != null)
			{
				connectionStrengthGainPerHourPruningBase *= Props.connectionStrengthGainPerPlantSkill.Evaluate(ConnectedPawn.skills.GetSkill(SkillDefOf.Plants).Level);
			}
			return connectionStrengthGainPerHourPruningBase;
		}
	}

	private float MinConnectionStrengthForSingleDryad
	{
		get
		{
			foreach (CurvePoint point in Props.maxDryadsPerConnectionStrengthCurve.Points)
			{
				if (point.y > 0f)
				{
					return point.x;
				}
			}
			return 0f;
		}
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		if (!ModLister.CheckIdeology("Tree connection"))
		{
			parent.Destroy();
		}
		else if (!respawningAfterLoad)
		{
			lastPrunedTick = Find.TickManager.TicksGame;
			spawnTick = Find.TickManager.TicksGame + SpawningDurationTicks;
		}
	}

	public override void PostDestroy(DestroyMode mode, Map previousMap)
	{
		leafEffecter?.Cleanup();
		leafEffecter = null;
		ConnectedPawn?.connections?.Notify_ConnectedThingDestroyed(parent);
		for (int num = dryads.Count - 1; num >= 0; num--)
		{
			dryads[num].connections?.Notify_ConnectedThingDestroyed(parent);
			dryads[num].forceNoDeathNotification = true;
			dryads[num].Kill(null);
			dryads[num].forceNoDeathNotification = false;
		}
		if (Connected && ConnectedPawn.Faction == Faction.OfPlayer)
		{
			Find.LetterStack.ReceiveLetter("LetterLabelConnectedTreeDestroyed".Translate(parent.Named("TREE")), "LetterTextConnectedTreeDestroyed".Translate(parent.Named("TREE"), ConnectedPawn.Named("CONNECTEDPAWN")), LetterDefOf.NegativeEvent, ConnectedPawn);
		}
	}

	public override void Notify_MapRemoved()
	{
		ConnectedPawn?.connections?.Notify_ConnectedThingLeftBehind(parent);
		for (int num = dryads.Count - 1; num >= 0; num--)
		{
			Pawn pawn = dryads[num];
			pawn.forceNoDeathNotification = true;
			pawn.Kill(null);
			pawn.forceNoDeathNotification = false;
		}
	}

	public override void CompTick()
	{
		if (!ModsConfig.IdeologyActive)
		{
			return;
		}
		if (Find.TickManager.TicksGame >= spawnTick)
		{
			SpawnDryad();
		}
		if (leafEffecter == null)
		{
			leafEffecter = EffecterDefOf.GauranlenLeavesBatch.Spawn();
			leafEffecter.Trigger(parent, parent);
		}
		leafEffecter?.EffectTick(parent, parent);
		if (Connected && Find.TickManager.TicksGame - lastPrunedTick > 1)
		{
			ConnectionStrength -= ConnectionStrengthLossPerDay / 60000f;
		}
		if (!parent.IsHashIntervalTick(300))
		{
			return;
		}
		if (Mode == GauranlenTreeModeDefOf.Gaumaker && dryads.Count >= 3)
		{
			if (gaumakerPod == null && TryGetGaumakerCell(out var cell))
			{
				gaumakerPod = GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.GaumakerCocoon), cell, parent.Map);
			}
		}
		else if (gaumakerPod != null && !gaumakerPod.Destroyed)
		{
			gaumakerPod.Destroy();
			gaumakerPod = null;
		}
	}

	public void ConnectToPawn(Pawn pawn, float ritualQuality)
	{
		if (ModLister.CheckIdeology("Tree connection") && !ConnectionTorn)
		{
			connectedPawn = pawn;
			pawn.connections?.ConnectTo(parent);
			ConnectionStrength = Props.initialConnectionStrengthRange.LerpThroughRange(ritualQuality);
			lastPrunedTick = 0;
			for (int i = 0; i < dryads.Count; i++)
			{
				ResetDryad(dryads[i]);
				dryads[i].MentalState?.RecoverFromState();
			}
		}
	}

	public void FinalizeMode()
	{
		currentMode = desiredMode;
		if (Connected)
		{
			MoteMaker.MakeStaticMote((ConnectedPawn.Position.ToVector3Shifted() + parent.Position.ToVector3Shifted()) / 2f, parent.Map, ThingDefOf.Mote_GauranlenCasteChanged);
		}
	}

	public void Notify_PawnDied(Pawn p)
	{
		if (connectedPawn == p)
		{
			TearConnection();
			return;
		}
		for (int i = 0; i < dryads.Count; i++)
		{
			if (p == dryads[i])
			{
				if (Connected)
				{
					ConnectedPawn.needs?.mood?.thoughts?.memories.TryGainMemory(ThoughtDefOf.DryadDied);
					ConnectionStrength -= Props.connectionStrengthLossPerDryadDeath;
				}
				dryads.RemoveAt(i);
				break;
			}
		}
	}

	public void RemoveDryad(Pawn oldDryad)
	{
		dryads.Remove(oldDryad);
	}

	public bool ShouldReturnToTree(Pawn dryad)
	{
		if (dryads.NullOrEmpty() || !dryads.Contains(dryad))
		{
			return false;
		}
		int num = dryads.Count - MaxDryads;
		if (num <= 0)
		{
			return false;
		}
		tmpDryads.Clear();
		tmpDryads.AddRange(dryads);
		tmpDryads.SortBy((Pawn x) => x.kindDef == DryadKind, (Pawn x) => x.ageTracker.AgeChronologicalTicks);
		for (int i = 0; i < num; i++)
		{
			if (tmpDryads[i] == dryad)
			{
				tmpDryads.Clear();
				return true;
			}
		}
		tmpDryads.Clear();
		return false;
	}

	public bool ShouldEnterGaumakerPod(Pawn dryad)
	{
		if (gaumakerPod == null || gaumakerPod.Destroyed)
		{
			return false;
		}
		if (dryads.NullOrEmpty() || dryads.Count < 3 || !dryads.Contains(dryad))
		{
			return false;
		}
		tmpDryads.Clear();
		for (int i = 0; i < dryads.Count; i++)
		{
			if (dryads[i].kindDef == PawnKindDefOf.Dryad_Gaumaker)
			{
				tmpDryads.Add(dryads[i]);
			}
		}
		if (tmpDryads.Count < 3)
		{
			tmpDryads.Clear();
			return false;
		}
		tmpDryads.SortBy((Pawn x) => -x.ageTracker.AgeChronologicalTicks);
		for (int j = 0; j < 3; j++)
		{
			if (tmpDryads[j] == dryad)
			{
				tmpDryads.Clear();
				return true;
			}
		}
		tmpDryads.Clear();
		return false;
	}

	private void TearConnection()
	{
		Messages.Message("MessageConnectedPawnDied".Translate(parent.Named("TREE"), ConnectedPawn.Named("PAWN"), 1800000.ToStringTicksToDays().Named("DURATION")), parent, MessageTypeDefOf.NegativeEvent);
		for (int i = 0; i < dryads.Count; i++)
		{
			ResetDryad(dryads[i]);
		}
		SoundDefOf.GauranlenConnectionTorn.PlayOneShot(SoundInfo.InMap(parent));
		nextUntornTick = Find.TickManager.TicksGame + 1800000;
		connectedPawn = null;
		currentMode = null;
	}

	private void SpawnDryad()
	{
		spawnTick = Find.TickManager.TicksGame + (int)(60000f * Props.spawnDays);
		Pawn pawn = GenerateNewDryad(Props.pawnKind);
		GenSpawn.Spawn(pawn, parent.Position, parent.Map).Rotation = Rot4.South;
		EffecterDefOf.DryadSpawn.Spawn(parent.Position, parent.Map).Cleanup();
		SoundDefOf.Pawn_Dryad_Spawn.PlayOneShot(SoundInfo.InMap(pawn));
	}

	public Pawn GenerateNewDryad(PawnKindDef dryadCaste)
	{
		Gender? fixedGender = Gender.Male;
		Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(dryadCaste, null, PawnGenerationContext.NonPlayer, null, forceGenerateNewPawn: false, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, fixedGender, null, null, null, null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, null, null, null, null, null, 0f, DevelopmentalStage.Newborn, null, null, null));
		ResetDryad(pawn);
		pawn.connections?.ConnectTo(parent);
		dryads.Add(pawn);
		return pawn;
	}

	private void ResetDryad(Pawn dryad)
	{
		if (Connected && dryad.Faction != ConnectedPawn?.Faction)
		{
			dryad.SetFaction(ConnectedPawn?.Faction);
		}
		if (dryad.training == null)
		{
			return;
		}
		foreach (TrainableDef allDef in DefDatabase<TrainableDef>.AllDefs)
		{
			if (dryad.training.CanAssignToTrain(allDef).Accepted)
			{
				dryad.training.SetWantedRecursive(allDef, checkOn: true);
				dryad.training.Train(allDef, ConnectedPawn, complete: true);
				if (allDef == TrainableDefOf.Release)
				{
					dryad.playerSettings.followDrafted = true;
				}
			}
		}
	}

	public void Prune(int delta)
	{
		lastPrunedTick = Find.TickManager.TicksGame;
		ConnectionStrength += ConnectionStrengthGainPerHourOfPruning * (float)delta / 2500f;
	}

	public bool ShouldBePrunedNow(bool forced)
	{
		if (ConnectionStrength >= desiredConnectionStrength)
		{
			return false;
		}
		if (!forced)
		{
			if (ConnectionStrength >= desiredConnectionStrength - 0.03f)
			{
				return false;
			}
			if ((float)Find.TickManager.TicksGame < (float)lastPrunedTick + TimeBetweenAutoPruning)
			{
				return false;
			}
		}
		return true;
	}

	private float ClosestDistanceToBlockingBuilding(List<Thing> buildings)
	{
		float num = float.PositiveInfinity;
		for (int i = 0; i < buildings.Count; i++)
		{
			float num2 = buildings[i].Position.DistanceTo(parent.Position);
			if (num2 < num)
			{
				num = num2;
			}
		}
		return num;
	}

	private bool TryGetGaumakerCell(out IntVec3 cell)
	{
		cell = IntVec3.Invalid;
		if (CellFinder.TryFindRandomCellNear(parent.Position, parent.Map, 3, (IntVec3 c) => GauranlenUtility.CocoonAndPodCellValidator(c, parent.Map, ThingDefOf.Plant_PodGauranlen), out cell) || CellFinder.TryFindRandomCellNear(parent.Position, parent.Map, 3, (IntVec3 c) => GauranlenUtility.CocoonAndPodCellValidator(c, parent.Map, ThingDefOf.Plant_TreeGauranlen), out cell))
		{
			return true;
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__74))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__74(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void PostDraw()
	{
		if (dryads.Count < MaxDryads)
		{
			Matrix4x4 matrix = default(Matrix4x4);
			Vector3 pos = parent.Position.ToVector3ShiftedWithAltitude(AltitudeLayer.BuildingOnTop.AltitudeFor()) + Props.spawningPodOffset;
			float num = Props.spawningPodSizeRange.LerpThroughRange(1f - (float)spawnTick - (float)Find.TickManager.TicksGame / (float)SpawningDurationTicks);
			matrix.SetTRS(pos, Quaternion.identity, new Vector3(num, 1f, num));
			Graphics.DrawMesh(MeshPool.plane10, matrix, PodMat, 0);
		}
	}

	public override void PostDrawExtraSelectionOverlays()
	{
		connectedPawn?.connections?.DrawConnectionLine(parent);
	}

	public override string CompInspectStringExtra()
	{
		string text = base.CompInspectStringExtra();
		if (!text.NullOrEmpty())
		{
			text += "\n";
		}
		string text2 = string.Empty;
		if (dryads.Count < MaxDryads)
		{
			text2 = "SpawningDryadIn".Translate(NamedArgumentUtility.Named(Props.pawnKind, "DRYAD"), (spawnTick - Find.TickManager.TicksGame).ToStringTicksToPeriod().Named("TIME")).Resolve();
		}
		text = ((!ConnectionTorn) ? (text + "ConnectedPawn".Translate().Resolve() + ": " + (Connected ? connectedPawn.NameFullColored : "Nobody".Translate().CapitalizeFirst()).Resolve()) : (text + "ConnectionTorn".Translate(UntornInDurationTicks.ToStringTicksToPeriod()).Resolve()));
		if (Connected)
		{
			if (lastPrunedTick >= 0 && Find.TickManager.TicksGame - lastPrunedTick <= 60)
			{
				text = string.Concat(text, "\n", "PruningConnectionStrength".Translate(), ": ", "PerHour".Translate(ConnectionStrengthGainPerHourOfPruning.ToStringPercent()).Resolve());
			}
			if (Mode != null)
			{
				text += string.Concat("\n", "GauranlenTreeMode".Translate(), ": ") + Mode.LabelCap;
			}
			if (HasProductionMode && Mode != desiredMode)
			{
				text = text + "\n" + "WaitingForConnectorToChangeCaste".Translate(ConnectedPawn.Named("CONNECTEDPAWN")).Resolve();
			}
			if (!text2.NullOrEmpty())
			{
				text = text + "\n" + text2;
			}
			if (MaxDryads > 0)
			{
				text = string.Concat(text, "\n", "DryadPlural".Translate(), $" ({dryads.Count}/{MaxDryads})");
				if (dryads.Count > 0)
				{
					text = text + ": " + dryads.Select((Pawn x) => x.NameShortColored.Resolve()).ToCommaList().CapitalizeFirst();
				}
			}
			else
			{
				text = text + "\n" + "NotEnoughConnectionStrengthForSingleDryad".Translate(MinConnectionStrengthForSingleDryad.ToStringPercent()).Colorize(ColorLibrary.RedReadable);
			}
			if (!HasProductionMode)
			{
				text = text + "\n" + "AlertGauranlenTreeWithoutDryadTypeLabel".Translate().Colorize(ColorLibrary.RedReadable);
			}
			if (Mode == GauranlenTreeModeDefOf.Gaumaker && MaxDryads < 3)
			{
				text = text + "\n" + "ConnectionStrengthTooWeakForGaumakerPod".Translate().Colorize(ColorLibrary.RedReadable);
			}
			string text3 = AffectingBuildingsDescription("ConnectionStrengthAffectedBy");
			if (!text3.NullOrEmpty())
			{
				text = text + "\n" + text3;
			}
		}
		else if (!text2.NullOrEmpty())
		{
			text = text + "\n" + text2;
		}
		return text;
	}

	public string AffectingBuildingsDescription(string descKey)
	{
		List<Thing> buildingsReducingConnectionStrength = BuildingsReducingConnectionStrength;
		if (buildingsReducingConnectionStrength.Count > 0)
		{
			IEnumerable<string> source = buildingsReducingConnectionStrength.Select((Thing c) => GenLabel.ThingLabel(c, 1, includeHp: false)).Distinct();
			TaggedString taggedString = descKey.Translate() + ": " + source.Take(3).ToCommaList().CapitalizeFirst();
			if (source.Count() > 3)
			{
				taggedString += " " + "Etc".Translate();
			}
			return taggedString;
		}
		return null;
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__79))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__79(-2)
		{
			_003C_003E4__this = this
		};
	}

	private string ConnectionStrengthToMaxDryadsDesc()
	{
		string text = string.Concat("MaxDryadsBasedOnConnectionStrength".Translate() + ":\n -  " + "Unconnected".Translate() + ": ", Props.maxDryadsWild.ToString());
		foreach (CurvePoint item in Props.maxDryadsPerConnectionStrengthCurve)
		{
			text = string.Concat(text, "\n -  " + "ConnectionStrengthDisplay".Translate(item.x.ToStringPercent()) + ": ", item.y.ToString());
		}
		return text;
	}

	public float PruningHoursToMaintain(float desired)
	{
		float num = Props.connectionLossPerLevelCurve.Evaluate(desired);
		List<Thing> buildingsReducingConnectionStrength = BuildingsReducingConnectionStrength;
		if (buildingsReducingConnectionStrength.Any())
		{
			num += Props.connectionLossDailyPerBuildingDistanceCurve.Evaluate(ClosestDistanceToBlockingBuilding(buildingsReducingConnectionStrength));
		}
		return num / ConnectionStrengthGainPerHourOfPruning;
	}

	public bool WillBeAffectedBy(ThingDef def, Faction faction, IntVec3 pos, Rot4 rotation)
	{
		if (!MeditationUtility.CountsAsArtificialBuilding(def, faction))
		{
			return false;
		}
		if (GenAdj.OccupiedRect(pos, rotation, def.size).ClosestCellTo(parent.Position).InHorDistOf(parent.Position, Props.radiusToBuildingForConnectionStrengthLoss))
		{
			return true;
		}
		return false;
	}

	public override void PostExposeData()
	{
		Scribe_Defs.Look(ref currentMode, "currentMode");
		Scribe_Defs.Look(ref desiredMode, "desiredMode");
		Scribe_Values.Look(ref nextUntornTick, "nextUntornTick", -1);
		Scribe_Values.Look(ref spawnTick, "spawnTick", -1);
		Scribe_Values.Look(ref lastPrunedTick, "lastPrunedTick", 0);
		Scribe_Values.Look(ref desiredConnectionStrength, "desiredConnectionStrength", 0.5f);
		Scribe_Values.Look(ref connectionStrength, "connectionStrength", 0f);
		Scribe_References.Look(ref connectedPawn, "connectedPawn");
		Scribe_References.Look(ref gaumakerPod, "gaumakerPod");
		Scribe_Collections.Look(ref dryads, "dryads", LookMode.Reference);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			dryads.RemoveAll((Pawn x) => x?.Dead ?? true);
		}
	}
}
