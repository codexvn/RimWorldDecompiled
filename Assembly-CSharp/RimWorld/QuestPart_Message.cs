using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C65 RID: 7269
	public class QuestPart_Message : QuestPart
	{
		// Token: 0x170018E9 RID: 6377
		// (get) Token: 0x0600A1FB RID: 41467 RVA: 0x002E8BC3 File Offset: 0x002E6DC3
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.addQuestLookTargets)
				{
					GlobalTargetInfo globalTargetInfo2 = this.lookTargets.TryGetPrimaryTarget();
					if (globalTargetInfo2.IsValid)
					{
						yield return globalTargetInfo2;
					}
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600A1FC RID: 41468 RVA: 0x002E8BD4 File Offset: 0x002E6DD4
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
				TaggedString formattedText = signal.args.GetFormattedText(this.message);
				if (!formattedText.NullOrEmpty())
				{
					Messages.Message(formattedText, lookTargets, this.messageType ?? MessageTypeDefOf.NeutralEvent, this.quest.hidden ? null : this.quest, this.historical);
				}
			}
		}

		// Token: 0x0600A1FD RID: 41469 RVA: 0x002E8C80 File Offset: 0x002E6E80
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.message, "message", null, false);
			Scribe_Defs.Look<MessageTypeDef>(ref this.messageType, "messageType");
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.historical, "historical", true, false);
			Scribe_Values.Look<bool>(ref this.getLookTargetsFromSignal, "getLookTargetsFromSignal", true, false);
			Scribe_Values.Look<bool>(ref this.addQuestLookTargets, "addQuestLookTargets", true, false);
		}

		// Token: 0x0600A1FE RID: 41470 RVA: 0x002E8D14 File Offset: 0x002E6F14
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			this.message = "DEV: Test";
			this.messageType = MessageTypeDefOf.PositiveEvent;
		}

		// Token: 0x04006FEB RID: 28651
		public string inSignal;

		// Token: 0x04006FEC RID: 28652
		public string message;

		// Token: 0x04006FED RID: 28653
		public MessageTypeDef messageType;

		// Token: 0x04006FEE RID: 28654
		public LookTargets lookTargets;

		// Token: 0x04006FEF RID: 28655
		public bool historical = true;

		// Token: 0x04006FF0 RID: 28656
		public bool getLookTargetsFromSignal = true;

		// Token: 0x04006FF1 RID: 28657
		public bool addQuestLookTargets = true;
	}
}
