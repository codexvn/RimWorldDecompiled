using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse;

public class Hediff_MetalhorrorImplant : HediffWithComps
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass42_0
	{
		public Hediff_MetalhorrorImplant _003C_003E4__this;

		public int hours;

		internal void _003CGetGizmos_003Eb__0()
		{
			_003C_003E4__this.Emerge("MetalhorrorReasonRandom".Translate(_003C_003E4__this.pawn.Named("INFECTED")));
		}

		internal void _003CGetGizmos_003Eb__1()
		{
			_003C_003E4__this.filthTimer = 0;
		}

		internal void _003CGetGizmos_003Eb__2()
		{
			_003C_003E4__this.debugDiscoverNextInteraction = true;
		}

		internal void _003CGetGizmos_003Eb__3()
		{
			if (hours < 24)
			{
				_003C_003E4__this.ageTicks = 62500;
			}
			else
			{
				_003C_003E4__this.ageTicks = 182500;
			}
		}

		internal void _003CGetGizmos_003Eb__4()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (Pawn item in _003C_003E4__this.pawn.MapHeld.mapPawns.AllHumanlike)
			{
				_003C_003Ec__DisplayClass42_1 CS_0024_003C_003E8__locals5 = new _003C_003Ec__DisplayClass42_1
				{
					CS_0024_003C_003E8__locals1 = this
				};
				if (MetalhorrorUtility.IsInfected(item, out CS_0024_003C_003E8__locals5.implant) && !list.ContainsAny((FloatMenuOption x) => x.Label == CS_0024_003C_003E8__locals5.implant.BiosignatureName))
				{
					list.Add(new FloatMenuOption(CS_0024_003C_003E8__locals5.implant.BiosignatureName, delegate
					{
						CS_0024_003C_003E8__locals5.CS_0024_003C_003E8__locals1._003C_003E4__this.implantSource.DebugSetBiosignature(CS_0024_003C_003E8__locals5.implant.Biosignature);
					}));
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass42_1
	{
		public Hediff_MetalhorrorImplant implant;

		public _003C_003Ec__DisplayClass42_0 CS_0024_003C_003E8__locals1;

		internal bool _003CGetGizmos_003Eb__5(FloatMenuOption x)
		{
			return x.Label == implant.BiosignatureName;
		}

		internal void _003CGetGizmos_003Eb__6()
		{
			CS_0024_003C_003E8__locals1._003C_003E4__this.implantSource.DebugSetBiosignature(implant.Biosignature);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__42 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Hediff_MetalhorrorImplant _003C_003E4__this;

		private _003C_003Ec__DisplayClass42_0 _003C_003E8__1;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

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
		public _003CGetGizmos_003Ed__42(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Hediff_MetalhorrorImplant hediff_MetalhorrorImplant = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass42_0();
					_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
					_003C_003E7__wrap1 = ((HediffWithComps)hediff_MetalhorrorImplant).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_009e;
				case 1:
					_003C_003E1__state = -3;
					goto IL_009e;
				case 2:
					_003C_003E1__state = -1;
					if (!hediff_MetalhorrorImplant.filthPos.IsValid)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Mark for flesh drop",
							action = delegate
							{
								_003C_003E8__1._003C_003E4__this.filthTimer = 0;
							}
						};
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_015f;
				case 3:
					_003C_003E1__state = -1;
					goto IL_015f;
				case 4:
					_003C_003E1__state = -1;
					goto IL_01a9;
				case 5:
					_003C_003E1__state = -1;
					goto IL_0237;
				case 6:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_0237:
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Change biosignature",
						action = delegate
						{
							List<FloatMenuOption> list = new List<FloatMenuOption>();
							foreach (Pawn item in _003C_003E8__1._003C_003E4__this.pawn.MapHeld.mapPawns.AllHumanlike)
							{
								_003C_003Ec__DisplayClass42_1 CS_0024_003C_003E8__locals5 = new _003C_003Ec__DisplayClass42_1
								{
									CS_0024_003C_003E8__locals1 = _003C_003E8__1
								};
								if (MetalhorrorUtility.IsInfected(item, out CS_0024_003C_003E8__locals5.implant) && !list.ContainsAny((FloatMenuOption x) => x.Label == CS_0024_003C_003E8__locals5.implant.BiosignatureName))
								{
									list.Add(new FloatMenuOption(CS_0024_003C_003E8__locals5.implant.BiosignatureName, delegate
									{
										CS_0024_003C_003E8__locals5.CS_0024_003C_003E8__locals1._003C_003E4__this.implantSource.DebugSetBiosignature(CS_0024_003C_003E8__locals5.implant.Biosignature);
									}));
								}
							}
							Find.WindowStack.Add(new FloatMenu(list));
						}
					};
					_003C_003E1__state = 6;
					return true;
					IL_015f:
					if (!hediff_MetalhorrorImplant.debugDiscoverNextInteraction)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Discover next interaction",
							action = delegate
							{
								_003C_003E8__1._003C_003E4__this.debugDiscoverNextInteraction = true;
							}
						};
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_01a9;
					IL_01a9:
					_003C_003E8__1.hours = hediff_MetalhorrorImplant.ageTicks / 2500;
					if (_003C_003E8__1.hours < 72)
					{
						string text = "Larva";
						if (_003C_003E8__1.hours > 24)
						{
							text = "Juvenile";
						}
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Inc Lifestage (" + text + ")",
							action = delegate
							{
								if (_003C_003E8__1.hours < 24)
								{
									_003C_003E8__1._003C_003E4__this.ageTicks = 62500;
								}
								else
								{
									_003C_003E8__1._003C_003E4__this.ageTicks = 182500;
								}
							}
						};
						_003C_003E1__state = 5;
						return true;
					}
					goto IL_0237;
					IL_009e:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (!DebugSettings.godMode || hediff_MetalhorrorImplant.emerging)
					{
						return false;
					}
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Emerge",
						action = delegate
						{
							_003C_003E8__1._003C_003E4__this.Emerge("MetalhorrorReasonRandom".Translate(_003C_003E8__1._003C_003E4__this.pawn.Named("INFECTED")));
						}
					};
					_003C_003E1__state = 2;
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__42 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__42(0)
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

	private ImplantSource implantSource = new ImplantSource();

	private bool emerging;

	private bool knowsDetected;

	private bool sympatheticEmergence;

	private bool sentMaturingMessage;

	private string emergeReason;

	private int emergeAt;

	private bool liedAboutInspection;

	private int filthTimer;

	private int spawnFilthAt;

	private IntVec3 filthPos = IntVec3.Invalid;

	private Map filthMap;

	public bool debugDiscoverNextInteraction;

	private static readonly IntRange TicksToEmerge = new IntRange(60, 180);

	private static readonly IntRange TicksToEmergeSympathetic = new IntRange(60, 600);

	private static readonly FloatRange InitialFilthDelayDays = new FloatRange(1.5f, 2.5f);

	private static readonly FloatRange FilthDelayDays = new FloatRange(3f, 10f);

	private static readonly FloatRange SpawnFilthDelayHours = new FloatRange(1f, 12f);

	private static readonly List<(Hediff_MetalhorrorImplant hediff, string message)> TmpEmerging = new List<(Hediff_MetalhorrorImplant, string)>();

	public ImplantSource ImplantSource
	{
		get
		{
			return implantSource;
		}
		set
		{
			implantSource = value;
		}
	}

	public int Biosignature => implantSource.Biosignature;

	public string BiosignatureName => implantSource.BiosignatureName;

	public bool Emerging => emerging;

	public bool KnowsDetected => knowsDetected;

	public bool LiedAboutInspection
	{
		get
		{
			return liedAboutInspection;
		}
		set
		{
			liedAboutInspection = value || liedAboutInspection;
		}
	}

	public override string TipStringExtra => implantSource.GetSourceDesc().CapitalizeFirst();

	public override void PostAdd(DamageInfo? dinfo)
	{
		if (!ModLister.CheckAnomaly("Metalhorror implant"))
		{
			pawn.health.RemoveHediff(this);
			return;
		}
		base.PostAdd(dinfo);
		filthTimer = Mathf.CeilToInt(InitialFilthDelayDays.RandomInRange * 60000f);
	}

	public override void PostTickInterval(int delta)
	{
		base.PostTickInterval(delta);
		if (pawn.SpawnedOrAnyParentSpawned)
		{
			DoPassiveTransmission();
			FilthTick(delta);
			if (Emerging && GenTicks.TicksGame >= emergeAt)
			{
				MetalhorrorUtility.SpawnMetalhorror(pawn, this);
			}
			if (knowsDetected)
			{
				Emerge("");
			}
			else if (MetalhorrorUtility.ShouldRandomEmerge(pawn, delta))
			{
				Emerge("MetalhorrorReasonRandom".Translate(pawn.Named("INFECTED")));
			}
		}
	}

	public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
	{
		Emerge("MetalhorrorReasonHostDied".Translate(pawn.Named("INFECTED")));
	}

	public void Emerge(string reason, bool isSympathetic = false)
	{
		if (emerging || !pawn.SpawnedOrAnyParentSpawned)
		{
			return;
		}
		Find.Anomaly.emergedBiosignatures.Add(Biosignature);
		SetVisible();
		emerging = true;
		knowsDetected = true;
		sympatheticEmergence = isSympathetic || sympatheticEmergence;
		if (string.IsNullOrEmpty(emergeReason))
		{
			emergeReason = reason;
		}
		if (sympatheticEmergence)
		{
			emergeAt = GenTicks.TicksGame + TicksToEmergeSympathetic.RandomInRange;
		}
		else
		{
			emergeAt = GenTicks.TicksGame + TicksToEmerge.RandomInRange;
		}
		if (!pawn.DeadOrDowned)
		{
			HealthUtility.DamageUntilDowned(pawn, allowBleedingWounds: true, DamageDefOf.Cut, ThingDefOf.Metalhorror, BodyPartGroupDefOf.LeftBlade);
		}
		else if (!pawn.Dead)
		{
			DamageWorker.DamageResult damageResult = pawn.TakeDamage(new DamageInfo(DamageDefOf.Cut, 20f));
			if (damageResult.hediffs != null)
			{
				foreach (Hediff hediff in damageResult.hediffs)
				{
					hediff.sourceDef = ThingDefOf.Metalhorror;
					hediff.sourceBodyPartGroup = BodyPartGroupDefOf.LeftBlade;
				}
			}
		}
		if (!sympatheticEmergence)
		{
			TmpEmerging.Clear();
			foreach (Pawn item in pawn.MapHeld.mapPawns.AllHumanlike)
			{
				TryTriggerSympatheticEmergence(item, TmpEmerging);
			}
			foreach (Thing item2 in pawn.MapHeld.listerThings.ThingsInGroup(ThingRequestGroup.Corpse))
			{
				if (item2 is Corpse { InnerPawn: not null } corpse)
				{
					TryTriggerSympatheticEmergence(corpse.InnerPawn, TmpEmerging);
				}
			}
			TmpEmerging.SortByDescending(((Hediff_MetalhorrorImplant hediff, string message) tuple) => tuple.hediff.ageTicks);
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			foreach (var (hediff_MetalhorrorImplant, value) in TmpEmerging)
			{
				stringBuilder.AppendLine(value);
				if (hediff_MetalhorrorImplant.LiedAboutInspection)
				{
					if (stringBuilder2.Length == 0)
					{
						stringBuilder2.AppendLine($"{"MetalhorrorSurgicalInspectionDetails".Translate()}:\n");
					}
					stringBuilder2.AppendLine($" - {hediff_MetalhorrorImplant.pawn.NameShortColored}");
				}
			}
			TaggedString taggedString = ((TmpEmerging.Count != 1) ? "MetalhorrorEmergingPluralDesc".Translate() : "MetalhorrorEmergingDesc".Translate());
			taggedString += "\n\n" + stringBuilder;
			if (stringBuilder2.Length > 0)
			{
				taggedString += "\n" + stringBuilder2;
			}
			taggedString += "\n" + emergeReason;
			taggedString += "\n\n" + "MetalhorrorEmergingDescAppended".Translate();
			Find.LetterStack.ReceiveLetter("MetalhorrorEmergingLabel".Translate(), taggedString.Resolve(), LetterDefOf.ThreatBig, TmpEmerging.Select(((Hediff_MetalhorrorImplant hediff, string message) hd) => hd.hediff.pawn).ToList());
			Find.AnalysisManager.RemoveAnalysisDetails(Biosignature);
			TmpEmerging.Clear();
		}
		DoEmergingEffects();
		if (pawn.Dead)
		{
			MetalhorrorUtility.SpawnMetalhorror(pawn, this);
		}
	}

	private void TryTriggerSympatheticEmergence(Pawn target, List<(Hediff_MetalhorrorImplant hediff, string message)> list)
	{
		Hediff_MetalhorrorImplant firstHediff = target.health.hediffSet.GetFirstHediff<Hediff_MetalhorrorImplant>();
		if (firstHediff != null && firstHediff.Biosignature == Biosignature)
		{
			if (target != pawn)
			{
				MetalhorrorUtility.TryEmerge(target, null, sympathetic: true);
			}
			string item = $"  - {target.NameShortColored} ({firstHediff.ImplantSource.GetSourceDesc()})";
			list.Add((firstHediff, item));
		}
	}

	private void DoEmergingEffects()
	{
		EffecterDefOf.MetalhorrorEmerging.Spawn(pawn.PositionHeld, pawn.MapHeld).Cleanup();
		IntVec3 positionHeld = pawn.PositionHeld;
		Map mapHeld = pawn.MapHeld;
		CellRect cellRect = new CellRect(positionHeld.x, positionHeld.z, 3, 3);
		for (int i = 0; i < 5; i++)
		{
			IntVec3 randomCell = cellRect.RandomCell;
			if (randomCell.InBounds(mapHeld) && GenSight.LineOfSight(randomCell, positionHeld, mapHeld))
			{
				FilthMaker.TryMakeFilth(randomCell, mapHeld, (i % 2 == 0) ? ThingDefOf.Filth_Blood : ThingDefOf.Filth_GrayFlesh);
			}
		}
	}

	public void Detect(string reason, bool noticed)
	{
		SetVisible();
		knowsDetected = noticed;
		if (knowsDetected)
		{
			Emerge(reason);
		}
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__42))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__42(-2)
		{
			_003C_003E4__this = this
		};
	}

	private void DoPassiveTransmission()
	{
		if (!pawn.InBed())
		{
			return;
		}
		foreach (Pawn curOccupant in pawn.CurrentBed().CurOccupants)
		{
			if (curOccupant != pawn)
			{
				MetalhorrorUtility.Infect(curOccupant, pawn, "SleepImplant");
			}
		}
	}

	private void FilthTick(int delta)
	{
		UpdateFilthTimer(delta);
		if (filthTimer <= 0)
		{
			if (!FilthMaker.CanMakeFilth(pawn.PositionHeld, pawn.MapHeld, ThingDefOf.Filth_GrayFleshNoticeable))
			{
				filthTimer = 600;
			}
			else
			{
				filthTimer = Mathf.CeilToInt(FilthDelayDays.RandomInRange * 60000f);
				spawnFilthAt = GenTicks.TicksGame + Mathf.CeilToInt(SpawnFilthDelayHours.RandomInRange * 2500f);
				filthMap = pawn.MapHeld;
				filthPos = pawn.PositionHeld;
			}
		}
		TryPlaceFilth();
	}

	private void TryPlaceFilth()
	{
		if (!filthPos.IsValid)
		{
			return;
		}
		if (filthMap == null || filthMap.Disposed)
		{
			if (pawn.SpawnedOrAnyParentSpawned)
			{
				filthMap = pawn.MapHeld;
				filthPos = pawn.PositionHeld;
				spawnFilthAt = GenTicks.TicksGame + 2500;
			}
		}
		else if (GenTicks.TicksGame >= spawnFilthAt)
		{
			if (FilthMaker.TryMakeFilth(filthPos, filthMap, ThingDefOf.Filth_GrayFleshNoticeable, out var outFilth))
			{
				((FilthGrayFleshNoticeable)outFilth).biosignature = Biosignature;
			}
			filthPos = IntVec3.Invalid;
			filthMap = null;
		}
	}

	public override string GetInspectString()
	{
		if (Prefs.DevMode && DebugSettings.godMode)
		{
			return $"DEV: Bio: {BiosignatureName}, mark filth: {filthTimer.TicksToSeconds():0}s {((spawnFilthAt < GenTicks.TicksGame) ? "" : $" (spawns: {(spawnFilthAt - GenTicks.TicksGame).TicksToSeconds():0}s)")}";
		}
		return base.GetInspectString();
	}

	private void UpdateFilthTimer(int delta)
	{
		if (!pawn.InBed() && !pawn.GetPosture().Laying() && !pawn.Downed)
		{
			filthTimer -= delta;
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref emerging, "emerging", defaultValue: false);
		Scribe_Values.Look(ref emergeAt, "emergeAt", 0);
		Scribe_Values.Look(ref filthTimer, "filthTimer", 0);
		Scribe_Values.Look(ref knowsDetected, "knowsDetected", defaultValue: false);
		Scribe_Values.Look(ref sympatheticEmergence, "sympatheticEmergence", defaultValue: false);
		Scribe_Values.Look(ref sentMaturingMessage, "sentMaturingMessage", defaultValue: false);
		Scribe_Values.Look(ref spawnFilthAt, "spawnFilthAt", 0);
		Scribe_Values.Look(ref filthPos, "filthPos");
		Scribe_Values.Look(ref liedAboutInspection, "liedAboutInspection", defaultValue: false);
		Scribe_References.Look(ref filthMap, "filthMap");
		Scribe_Deep.Look(ref implantSource, "implantSource");
	}
}
