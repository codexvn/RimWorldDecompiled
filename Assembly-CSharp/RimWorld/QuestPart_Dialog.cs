using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C30 RID: 7216
	public class QuestPart_Dialog : QuestPart
	{
		// Token: 0x170018B2 RID: 6322
		// (get) Token: 0x0600A0A8 RID: 41128 RVA: 0x002E42AE File Offset: 0x002E24AE
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

		// Token: 0x170018B3 RID: 6323
		// (get) Token: 0x0600A0A9 RID: 41129 RVA: 0x002E42BE File Offset: 0x002E24BE
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in base.InvolvedFactions)
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.relatedFaction != null)
				{
					yield return this.relatedFaction;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600A0AA RID: 41130 RVA: 0x002E42D0 File Offset: 0x002E24D0
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				DiaNode diaNode = new DiaNode(signal.args.GetFormattedText(this.text));
				LookTargets resolvedLookTargets = this.lookTargets;
				if (this.getLookTargetsFromSignal && !resolvedLookTargets.IsValid())
				{
					SignalArgsUtility.TryGetLookTargets(signal.args, "SUBJECT", out resolvedLookTargets);
				}
				if (resolvedLookTargets.IsValid())
				{
					DiaOption diaOption = new DiaOption("JumpToLocation".Translate());
					diaOption.action = delegate()
					{
						CameraJumper.TryJumpAndSelect(resolvedLookTargets.TryGetPrimaryTarget(), CameraJumper.MovementMode.Pan);
					};
					diaOption.resolveTree = true;
					diaNode.options.Add(diaOption);
				}
				if (this.options.Any<QuestPart_Dialog.Option>())
				{
					for (int i = 0; i < this.options.Count; i++)
					{
						int localIndex = i;
						DiaOption diaOption2 = new DiaOption(signal.args.GetFormattedText(this.options[i].text));
						diaOption2.action = delegate()
						{
							Find.SignalManager.SendSignal(new Signal(this.options[localIndex].outSignal, false));
						};
						diaOption2.resolveTree = true;
						diaNode.options.Add(diaOption2);
					}
				}
				else
				{
					DiaOption diaOption3 = new DiaOption("OK".Translate());
					diaOption3.resolveTree = true;
					diaNode.options.Add(diaOption3);
				}
				TaggedString formattedText = signal.args.GetFormattedText(this.title);
				Dialog_NodeTreeWithFactionInfo dialog_NodeTreeWithFactionInfo = new Dialog_NodeTreeWithFactionInfo(diaNode, this.relatedFaction, true, this.radioMode, formattedText);
				QuestPartDialogCloseAction questPartDialogCloseAction = QuestPartDialogCloseActionRegistry.CloseActionOf(this.closeAction);
				if (questPartDialogCloseAction != null)
				{
					dialog_NodeTreeWithFactionInfo.soundClose = questPartDialogCloseAction.dialogCloseSound;
					dialog_NodeTreeWithFactionInfo.closeAction = questPartDialogCloseAction.dialogCloseAction;
				}
				Find.WindowStack.Add(dialog_NodeTreeWithFactionInfo);
				if (this.addToArchive)
				{
					Find.Archive.Add(new ArchivedDialog(diaNode.text, formattedText, this.relatedFaction));
				}
			}
		}

		// Token: 0x0600A0AB RID: 41131 RVA: 0x002E4508 File Offset: 0x002E2708
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Collections.Look<QuestPart_Dialog.Option>(ref this.options, "options", LookMode.Deep, Array.Empty<object>());
			Scribe_References.Look<Faction>(ref this.relatedFaction, "relatedFaction", false);
			Scribe_Values.Look<bool>(ref this.addToArchive, "addToArchive", true, false);
			Scribe_Values.Look<bool>(ref this.radioMode, "radioMode", false, false);
			Scribe_Values.Look<bool>(ref this.getLookTargetsFromSignal, "getLookTargetsFromSignal", false, false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_Values.Look<QuestPartDialogCloseAction.CloseActionKey>(ref this.closeAction, "closeAction", QuestPartDialogCloseAction.CloseActionKey.None, false);
		}

		// Token: 0x0600A0AC RID: 41132 RVA: 0x002E45D8 File Offset: 0x002E27D8
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			this.title = "Title";
			this.text = "DEV: Test";
			this.relatedFaction = Faction.OfMechanoids;
			this.addToArchive = false;
			QuestPart_Dialog.Option option = new QuestPart_Dialog.Option();
			option.text = "Option 1";
			option.outSignal = "DebugSignal" + Rand.Int.ToString();
			this.options.Add(option);
			QuestPart_Dialog.Option option2 = new QuestPart_Dialog.Option();
			option2.text = "Option 2";
			option2.outSignal = "DebugSignal" + Rand.Int.ToString();
			this.options.Add(option2);
		}

		// Token: 0x04006F11 RID: 28433
		public string inSignal;

		// Token: 0x04006F12 RID: 28434
		public string text;

		// Token: 0x04006F13 RID: 28435
		public string title;

		// Token: 0x04006F14 RID: 28436
		public List<QuestPart_Dialog.Option> options = new List<QuestPart_Dialog.Option>();

		// Token: 0x04006F15 RID: 28437
		public Faction relatedFaction;

		// Token: 0x04006F16 RID: 28438
		public bool addToArchive = true;

		// Token: 0x04006F17 RID: 28439
		public bool radioMode;

		// Token: 0x04006F18 RID: 28440
		public bool getLookTargetsFromSignal;

		// Token: 0x04006F19 RID: 28441
		public LookTargets lookTargets;

		// Token: 0x04006F1A RID: 28442
		public QuestPartDialogCloseAction.CloseActionKey closeAction;

		// Token: 0x02001C31 RID: 7217
		public class Option : IExposable
		{
			// Token: 0x0600A0B0 RID: 41136 RVA: 0x002E46BE File Offset: 0x002E28BE
			public void ExposeData()
			{
				Scribe_Values.Look<string>(ref this.text, "text", null, false);
				Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			}

			// Token: 0x04006F1B RID: 28443
			public string text;

			// Token: 0x04006F1C RID: 28444
			public string outSignal;
		}
	}
}
