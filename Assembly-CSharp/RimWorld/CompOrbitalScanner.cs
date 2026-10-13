using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D8E RID: 11662
	public class CompOrbitalScanner : ThingComp
	{
		// Token: 0x170029C6 RID: 10694
		// (get) Token: 0x06010661 RID: 67169 RVA: 0x004BB878 File Offset: 0x004B9A78
		private CompPowerTrader PowerTrader
		{
			get
			{
				CompPowerTrader result;
				if ((result = this.compPowerTrader) == null)
				{
					result = (this.compPowerTrader = this.parent.TryGetComp<CompPowerTrader>());
				}
				return result;
			}
		}

		// Token: 0x170029C7 RID: 10695
		// (get) Token: 0x06010662 RID: 67170 RVA: 0x004BB8A4 File Offset: 0x004B9AA4
		private OrbitalScannerWorldComponent OrbitalScannerWorldComponent
		{
			get
			{
				OrbitalScannerWorldComponent result;
				if ((result = this.orbitalScannerWorldComponent) == null)
				{
					result = (this.orbitalScannerWorldComponent = Find.World.GetComponent<OrbitalScannerWorldComponent>());
				}
				return result;
			}
		}

		// Token: 0x170029C8 RID: 10696
		// (get) Token: 0x06010663 RID: 67171 RVA: 0x004BB8D0 File Offset: 0x004B9AD0
		private List<QuestScriptDef> ScannerQuests
		{
			get
			{
				List<QuestScriptDef> result;
				if ((result = this.scannerQuests) == null)
				{
					result = (this.scannerQuests = QuestUtility.GetGiverQuests(QuestGiverTag.OrbitalScanner).ToList<QuestScriptDef>());
				}
				return result;
			}
		}

		// Token: 0x170029C9 RID: 10697
		// (get) Token: 0x06010664 RID: 67172 RVA: 0x004BB8FB File Offset: 0x004B9AFB
		private bool IsLocating
		{
			get
			{
				return this.locateSignalTick > 0;
			}
		}

		// Token: 0x170029CA RID: 10698
		// (get) Token: 0x06010665 RID: 67173 RVA: 0x004BB906 File Offset: 0x004B9B06
		private int TicksUntilLocated
		{
			get
			{
				return this.locateSignalTick - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06010666 RID: 67174 RVA: 0x004BB919 File Offset: 0x004B9B19
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.locateSignalTick, "locateSignalTick", 0, false);
		}

		// Token: 0x06010667 RID: 67175 RVA: 0x004BB930 File Offset: 0x004B9B30
		public override void CompTick()
		{
			if (!this.PowerTrader.PowerOn)
			{
				if (this.locateSignalTick > 0)
				{
					this.locateSignalTick++;
				}
				return;
			}
			if (!this.IsLocating)
			{
				this.OrbitalScannerWorldComponent.Notify_ScannerWorking(this);
				return;
			}
			if (Find.TickManager.TicksGame >= this.locateSignalTick)
			{
				this.LocateSignal();
			}
		}

		// Token: 0x06010668 RID: 67176 RVA: 0x004BB990 File Offset: 0x004B9B90
		public void ReceiveSignal()
		{
			this.locateSignalTick = Find.TickManager.TicksGame + CompOrbitalScanner.TrackSignalDurationRangeTicks.RandomInRange;
			Messages.Message(string.Format("{0} ({1}).", "MessageOrbitalSignalDetected".Translate(), "OrbitalScannerCompletesIn".Translate(this.TicksUntilLocated.ToStringTicksToPeriod(true, false, true, true, false))), this.parent, MessageTypeDefOf.PositiveEvent, true);
		}

		// Token: 0x06010669 RID: 67177 RVA: 0x004BBA10 File Offset: 0x004B9C10
		private void LocateSignal()
		{
			float points = StorytellerUtility.DefaultThreatPointsNow(this.parent.Map);
			Slate slate = new Slate();
			slate.Set<float>("points", points, false);
			slate.Set<TaggedString>("discoveryMethod", "QuestDiscoveredFromOrbitalScanner".Translate(), false);
			QuestScriptDef questScriptDef = this.ScannerQuests.RandomElementByWeight((QuestScriptDef q) => NaturalRandomQuestChooser.GetNaturalRandomSelectionWeight(q, points, Find.World.StoryState));
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(questScriptDef, slate);
			if (!quest.hidden && questScriptDef.sendAvailableLetter)
			{
				QuestUtility.SendLetterQuestAvailable(quest, slate.Get<string>("discoveryMethod", null, false));
			}
			this.locateSignalTick = -1;
		}

		// Token: 0x0601066A RID: 67178 RVA: 0x004BBAB4 File Offset: 0x004B9CB4
		public override string CompInspectStringExtra()
		{
			if (!this.PowerTrader.PowerOn)
			{
				return null;
			}
			if (this.IsLocating)
			{
				return "OrbitalScannerLocating".Translate() + ": " + this.TicksUntilLocated.ToStringTicksToPeriod(true, false, true, true, false);
			}
			if (this.PowerTrader.PowerOn)
			{
				return "OrbitalScannerScanning".Translate();
			}
			return null;
		}

		// Token: 0x0601066B RID: 67179 RVA: 0x004BBB25 File Offset: 0x004B9D25
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!DebugSettings.ShowDevGizmos)
			{
				yield break;
			}
			if (!this.IsLocating)
			{
				yield return new Command_Action
				{
					defaultLabel = "Find Signal",
					action = new Action(this.ReceiveSignal)
				};
			}
			else
			{
				yield return new Command_Action
				{
					defaultLabel = "Locate Signal",
					action = new Action(this.LocateSignal)
				};
			}
			yield break;
		}

		// Token: 0x0400AB1C RID: 43804
		private int locateSignalTick = -1;

		// Token: 0x0400AB1D RID: 43805
		private CompPowerTrader compPowerTrader;

		// Token: 0x0400AB1E RID: 43806
		private OrbitalScannerWorldComponent orbitalScannerWorldComponent;

		// Token: 0x0400AB1F RID: 43807
		private List<QuestScriptDef> scannerQuests;

		// Token: 0x0400AB20 RID: 43808
		private static readonly IntRange TrackSignalDurationRangeTicks = new IntRange(480000, 600000);
	}
}
