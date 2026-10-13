using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C17 RID: 7191
	public class QuestPart_CameraJump : QuestPart
	{
		// Token: 0x1700189C RID: 6300
		// (get) Token: 0x0600A021 RID: 40993 RVA: 0x002E2979 File Offset: 0x002E0B79
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

		// Token: 0x0600A022 RID: 40994 RVA: 0x002E298C File Offset: 0x002E0B8C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				LookTargets lookTargets = this.lookTargets;
				if (this.getLookTargetsFromSignal && !lookTargets.IsValid())
				{
					SignalArgsUtility.TryGetLookTargets(signal.args, "SUBJECT", out lookTargets);
				}
				if (lookTargets.IsValid())
				{
					if (this.select)
					{
						CameraJumper.TryJumpAndSelect(lookTargets.TryGetPrimaryTarget(), CameraJumper.MovementMode.Pan);
						return;
					}
					CameraJumper.TryJump(lookTargets.TryGetPrimaryTarget(), CameraJumper.MovementMode.Pan);
				}
			}
		}

		// Token: 0x0600A023 RID: 40995 RVA: 0x002E2A08 File Offset: 0x002E0C08
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.getLookTargetsFromSignal, "getLookTargetsFromSignal", true, false);
			Scribe_Values.Look<bool>(ref this.select, "select", true, false);
		}

		// Token: 0x0600A024 RID: 40996 RVA: 0x002E2A68 File Offset: 0x002E0C68
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			this.lookTargets = Find.Maps.SelectMany((Map x) => x.mapPawns.FreeColonistsSpawned).RandomElementWithFallback(null);
		}

		// Token: 0x04006EBE RID: 28350
		public string inSignal;

		// Token: 0x04006EBF RID: 28351
		public LookTargets lookTargets;

		// Token: 0x04006EC0 RID: 28352
		public bool getLookTargetsFromSignal = true;

		// Token: 0x04006EC1 RID: 28353
		public bool select = true;
	}
}
