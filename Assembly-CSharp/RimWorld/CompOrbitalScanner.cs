using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld;

public class CompOrbitalScanner : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__20 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompOrbitalScanner _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__20(int _003C_003E1__state)
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
			CompOrbitalScanner compOrbitalScanner = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!DebugSettings.ShowDevGizmos)
				{
					return false;
				}
				if (!compOrbitalScanner.IsLocating)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "Find Signal",
						action = compOrbitalScanner.ReceiveSignal
					};
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "Locate Signal",
					action = compOrbitalScanner.LocateSignal
				};
				_003C_003E1__state = 2;
				return true;
			case 1:
				_003C_003E1__state = -1;
				break;
			case 2:
				_003C_003E1__state = -1;
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
			_003CCompGetGizmosExtra_003Ed__20 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__20(0)
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

	private int locateSignalTick = -1;

	private CompPowerTrader compPowerTrader;

	private OrbitalScannerWorldComponent orbitalScannerWorldComponent;

	private List<QuestScriptDef> scannerQuests;

	private static readonly IntRange TrackSignalDurationRangeTicks = new IntRange(480000, 600000);

	private CompPowerTrader PowerTrader => compPowerTrader ?? (compPowerTrader = parent.TryGetComp<CompPowerTrader>());

	private OrbitalScannerWorldComponent OrbitalScannerWorldComponent => orbitalScannerWorldComponent ?? (orbitalScannerWorldComponent = Find.World.GetComponent<OrbitalScannerWorldComponent>());

	private List<QuestScriptDef> ScannerQuests => scannerQuests ?? (scannerQuests = QuestUtility.GetGiverQuests(QuestGiverTag.OrbitalScanner).ToList());

	private bool IsLocating => locateSignalTick > 0;

	private int TicksUntilLocated => locateSignalTick - Find.TickManager.TicksGame;

	public override void PostExposeData()
	{
		Scribe_Values.Look(ref locateSignalTick, "locateSignalTick", 0);
	}

	public override void CompTick()
	{
		if (!PowerTrader.PowerOn)
		{
			if (locateSignalTick > 0)
			{
				locateSignalTick++;
			}
		}
		else if (!IsLocating)
		{
			OrbitalScannerWorldComponent.Notify_ScannerWorking(this);
		}
		else if (Find.TickManager.TicksGame >= locateSignalTick)
		{
			LocateSignal();
		}
	}

	public void ReceiveSignal()
	{
		locateSignalTick = Find.TickManager.TicksGame + TrackSignalDurationRangeTicks.RandomInRange;
		Messages.Message($"{"MessageOrbitalSignalDetected".Translate()} ({"OrbitalScannerCompletesIn".Translate(TicksUntilLocated.ToStringTicksToPeriod())}).", parent, MessageTypeDefOf.PositiveEvent);
	}

	private void LocateSignal()
	{
		float points = StorytellerUtility.DefaultThreatPointsNow(parent.Map);
		Slate slate = new Slate();
		slate.Set("points", points);
		slate.Set("discoveryMethod", "QuestDiscoveredFromOrbitalScanner".Translate());
		QuestScriptDef questScriptDef = ScannerQuests.RandomElementByWeight((QuestScriptDef q) => NaturalRandomQuestChooser.GetNaturalRandomSelectionWeight(q, points, Find.World.StoryState));
		Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(questScriptDef, slate);
		if (!quest.hidden && questScriptDef.sendAvailableLetter)
		{
			QuestUtility.SendLetterQuestAvailable(quest, slate.Get<string>("discoveryMethod"));
		}
		locateSignalTick = -1;
	}

	public override string CompInspectStringExtra()
	{
		if (!PowerTrader.PowerOn)
		{
			return null;
		}
		if (IsLocating)
		{
			return "OrbitalScannerLocating".Translate() + ": " + TicksUntilLocated.ToStringTicksToPeriod();
		}
		if (PowerTrader.PowerOn)
		{
			return "OrbitalScannerScanning".Translate();
		}
		return null;
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__20))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__20(-2)
		{
			_003C_003E4__this = this
		};
	}
}
