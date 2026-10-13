using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C10 RID: 7184
	public class QuestPart_Alert : QuestPartActivable
	{
		// Token: 0x17001895 RID: 6293
		// (get) Token: 0x06009FFD RID: 40957 RVA: 0x002E2239 File Offset: 0x002E0439
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				GlobalTargetInfo globalTargetInfo2 = this.lookTargets.TryGetPrimaryTarget();
				if (globalTargetInfo2.IsValid)
				{
					yield return globalTargetInfo2;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17001896 RID: 6294
		// (get) Token: 0x06009FFE RID: 40958 RVA: 0x002E2249 File Offset: 0x002E0449
		public override string AlertLabel
		{
			get
			{
				return this.resolvedLabel;
			}
		}

		// Token: 0x17001897 RID: 6295
		// (get) Token: 0x06009FFF RID: 40959 RVA: 0x002E2251 File Offset: 0x002E0451
		public override string AlertExplanation
		{
			get
			{
				return this.resolvedExplanation;
			}
		}

		// Token: 0x17001898 RID: 6296
		// (get) Token: 0x0600A000 RID: 40960 RVA: 0x002E2259 File Offset: 0x002E0459
		public override bool AlertCritical
		{
			get
			{
				return this.critical;
			}
		}

		// Token: 0x17001899 RID: 6297
		// (get) Token: 0x0600A001 RID: 40961 RVA: 0x002E2261 File Offset: 0x002E0461
		public override AlertReport AlertReport
		{
			get
			{
				if (base.State != QuestPartState.Enabled)
				{
					return AlertReport.Inactive;
				}
				if (this.resolvedLookTargets.IsValid())
				{
					return AlertReport.CulpritsAre(this.resolvedLookTargets.targets);
				}
				return AlertReport.Active;
			}
		}

		// Token: 0x0600A002 RID: 40962 RVA: 0x002E2298 File Offset: 0x002E0498
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.resolvedLabel = receivedArgs.GetFormattedText(this.label);
			this.resolvedExplanation = receivedArgs.GetFormattedText(this.explanation);
			this.resolvedLookTargets = this.lookTargets;
			if (this.getLookTargetsFromSignal && !this.resolvedLookTargets.IsValid())
			{
				SignalArgsUtility.TryGetLookTargets(receivedArgs, "SUBJECT", out this.resolvedLookTargets);
			}
		}

		// Token: 0x0600A003 RID: 40963 RVA: 0x002E231C File Offset: 0x002E051C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Values.Look<string>(ref this.explanation, "explanation", null, false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.critical, "critical", false, false);
			Scribe_Values.Look<bool>(ref this.getLookTargetsFromSignal, "getLookTargetsFromSignal", false, false);
			Scribe_Values.Look<string>(ref this.resolvedLabel, "resolvedLabel", null, false);
			Scribe_Values.Look<string>(ref this.resolvedExplanation, "resolvedExplanation", null, false);
			Scribe_Deep.Look<LookTargets>(ref this.resolvedLookTargets, "resolvedLookTargets", Array.Empty<object>());
		}

		// Token: 0x0600A004 RID: 40964 RVA: 0x002E23C5 File Offset: 0x002E05C5
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.label = "DEV: Test";
			this.explanation = "Test text";
		}

		// Token: 0x04006EA5 RID: 28325
		public string label;

		// Token: 0x04006EA6 RID: 28326
		public string explanation;

		// Token: 0x04006EA7 RID: 28327
		public LookTargets lookTargets;

		// Token: 0x04006EA8 RID: 28328
		public bool critical;

		// Token: 0x04006EA9 RID: 28329
		public bool getLookTargetsFromSignal;

		// Token: 0x04006EAA RID: 28330
		private string resolvedLabel;

		// Token: 0x04006EAB RID: 28331
		private string resolvedExplanation;

		// Token: 0x04006EAC RID: 28332
		private LookTargets resolvedLookTargets;
	}
}
