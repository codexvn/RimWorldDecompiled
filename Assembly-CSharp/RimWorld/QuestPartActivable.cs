using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CCD RID: 7373
	public abstract class QuestPartActivable : QuestPart
	{
		// Token: 0x1700194C RID: 6476
		// (get) Token: 0x0600A44B RID: 42059 RVA: 0x002F1262 File Offset: 0x002EF462
		public QuestPartState State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x1700194D RID: 6477
		// (get) Token: 0x0600A44C RID: 42060 RVA: 0x002F126A File Offset: 0x002EF46A
		public int EnableTick
		{
			get
			{
				if (this.State != QuestPartState.Enabled)
				{
					return -1;
				}
				return this.enableTick;
			}
		}

		// Token: 0x1700194E RID: 6478
		// (get) Token: 0x0600A44D RID: 42061 RVA: 0x002F1280 File Offset: 0x002EF480
		public string OutSignalEnabled
		{
			get
			{
				return string.Concat(new string[]
				{
					"Quest",
					this.quest.id.ToString(),
					".Part",
					base.Index.ToString(),
					".Enabled"
				});
			}
		}

		// Token: 0x1700194F RID: 6479
		// (get) Token: 0x0600A44E RID: 42062 RVA: 0x002F12D4 File Offset: 0x002EF4D4
		public string OutSignalCompleted
		{
			get
			{
				return string.Concat(new string[]
				{
					"Quest",
					this.quest.id.ToString(),
					".Part",
					base.Index.ToString(),
					".Completed"
				});
			}
		}

		// Token: 0x17001950 RID: 6480
		// (get) Token: 0x0600A44F RID: 42063 RVA: 0x002F1328 File Offset: 0x002EF528
		public virtual string ExpiryInfoPart { get; }

		// Token: 0x17001951 RID: 6481
		// (get) Token: 0x0600A450 RID: 42064 RVA: 0x002F1330 File Offset: 0x002EF530
		public virtual string ExpiryInfoPartTip { get; }

		// Token: 0x17001952 RID: 6482
		// (get) Token: 0x0600A451 RID: 42065 RVA: 0x002F1338 File Offset: 0x002EF538
		public virtual AlertReport AlertReport
		{
			get
			{
				return AlertReport.Inactive;
			}
		}

		// Token: 0x17001953 RID: 6483
		// (get) Token: 0x0600A452 RID: 42066 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string AlertLabel
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001954 RID: 6484
		// (get) Token: 0x0600A453 RID: 42067 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string AlertExplanation
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001955 RID: 6485
		// (get) Token: 0x0600A454 RID: 42068 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool AlertCritical
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001956 RID: 6486
		// (get) Token: 0x0600A455 RID: 42069 RVA: 0x002F1340 File Offset: 0x002EF540
		public virtual GlobalTargetInfo? AlertCulpritTarget
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001957 RID: 6487
		// (get) Token: 0x0600A456 RID: 42070 RVA: 0x002F1356 File Offset: 0x002EF556
		public bool AlertDirty
		{
			get
			{
				return this.cachedAlert != null && ((this.AlertCritical && !(this.cachedAlert is Alert_CustomCritical)) || (!this.AlertCritical && !(this.cachedAlert is Alert_Custom)));
			}
		}

		// Token: 0x17001958 RID: 6488
		// (get) Token: 0x0600A457 RID: 42071 RVA: 0x002F1394 File Offset: 0x002EF594
		public Alert CachedAlert
		{
			get
			{
				AlertReport alertReport = this.AlertReport;
				if (this.cachedAlert == null)
				{
					if (!alertReport.active)
					{
						return null;
					}
					if (this.AlertCritical)
					{
						this.cachedAlert = new Alert_CustomCritical();
					}
					else
					{
						this.cachedAlert = new Alert_Custom();
					}
				}
				Alert_Custom alert_Custom = this.cachedAlert as Alert_Custom;
				if (alert_Custom != null)
				{
					if (alertReport.active)
					{
						alert_Custom.label = this.AlertLabel;
						alert_Custom.explanation = this.AlertExplanation;
						alertReport.culpritTarget = this.AlertCulpritTarget;
					}
					alert_Custom.report = alertReport;
				}
				Alert_CustomCritical alert_CustomCritical = this.cachedAlert as Alert_CustomCritical;
				if (alert_CustomCritical != null)
				{
					if (alertReport.active)
					{
						alert_CustomCritical.label = this.AlertLabel;
						alert_CustomCritical.explanation = this.AlertExplanation;
					}
					alert_CustomCritical.report = alertReport;
				}
				return this.cachedAlert;
			}
		}

		// Token: 0x0600A458 RID: 42072 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void QuestPartTick()
		{
		}

		// Token: 0x0600A459 RID: 42073 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string ExtraInspectString(ISelectable target)
		{
			return null;
		}

		// Token: 0x0600A45A RID: 42074 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual IEnumerable<Gizmo> ExtraGizmos(ISelectable target)
		{
			return null;
		}

		// Token: 0x0600A45B RID: 42075 RVA: 0x002F1459 File Offset: 0x002EF659
		public void ClearCachedAlert()
		{
			this.cachedAlert = null;
		}

		// Token: 0x0600A45C RID: 42076 RVA: 0x002F1464 File Offset: 0x002EF664
		protected virtual void Enable(SignalArgs receivedArgs)
		{
			if (this.state == QuestPartState.Enabled)
			{
				Log.Error("Tried to enable QuestPart while already enabled. part=" + ((this != null) ? this.ToString() : null));
				return;
			}
			this.state = QuestPartState.Enabled;
			this.enableTick = Find.TickManager.TicksGame;
			Find.SignalManager.SendSignal(new Signal(this.OutSignalEnabled, false));
		}

		// Token: 0x0600A45D RID: 42077 RVA: 0x002F14C4 File Offset: 0x002EF6C4
		protected void Complete()
		{
			this.Complete(default(SignalArgs));
		}

		// Token: 0x0600A45E RID: 42078 RVA: 0x002F14E0 File Offset: 0x002EF6E0
		protected void Complete(NamedArgument signalArg1)
		{
			this.Complete(new SignalArgs(signalArg1));
		}

		// Token: 0x0600A45F RID: 42079 RVA: 0x002F14EE File Offset: 0x002EF6EE
		protected void Complete(NamedArgument signalArg1, NamedArgument signalArg2)
		{
			this.Complete(new SignalArgs(signalArg1, signalArg2));
		}

		// Token: 0x0600A460 RID: 42080 RVA: 0x002F14FD File Offset: 0x002EF6FD
		protected void Complete(NamedArgument signalArg1, NamedArgument signalArg2, NamedArgument signalArg3)
		{
			this.Complete(new SignalArgs(signalArg1, signalArg2, signalArg3));
		}

		// Token: 0x0600A461 RID: 42081 RVA: 0x002F150D File Offset: 0x002EF70D
		protected void Complete(NamedArgument signalArg1, NamedArgument signalArg2, NamedArgument signalArg3, NamedArgument signalArg4)
		{
			this.Complete(new SignalArgs(signalArg1, signalArg2, signalArg3, signalArg4));
		}

		// Token: 0x0600A462 RID: 42082 RVA: 0x002F151F File Offset: 0x002EF71F
		protected void Complete(params NamedArgument[] signalArgs)
		{
			this.Complete(new SignalArgs(signalArgs));
		}

		// Token: 0x0600A463 RID: 42083 RVA: 0x002F1530 File Offset: 0x002EF730
		protected virtual void Complete(SignalArgs signalArgs)
		{
			if (this.state != QuestPartState.Enabled)
			{
				Log.Error("Tried to end QuestPart but its state is not Active. part=" + ((this != null) ? this.ToString() : null));
				return;
			}
			this.state = QuestPartState.Disabled;
			if (this.outcomeCompletedSignalArg != QuestEndOutcome.Unknown)
			{
				signalArgs.Add(this.outcomeCompletedSignalArg.Named("OUTCOME"));
			}
			Find.SignalManager.SendSignal(new Signal(this.OutSignalCompleted, signalArgs, false));
			if (!this.outSignalsCompleted.NullOrEmpty<string>())
			{
				for (int i = 0; i < this.outSignalsCompleted.Count; i++)
				{
					if (!this.outSignalsCompleted[i].NullOrEmpty())
					{
						Find.SignalManager.SendSignal(new Signal(this.outSignalsCompleted[i], signalArgs, false));
					}
				}
			}
		}

		// Token: 0x0600A464 RID: 42084 RVA: 0x002F15F8 File Offset: 0x002EF7F8
		protected virtual void Disable()
		{
			if (this.state != QuestPartState.Enabled)
			{
				Log.Error("Tried to disable QuestPart but its state is not enabled. part=" + ((this != null) ? this.ToString() : null));
				return;
			}
			this.state = QuestPartState.Disabled;
		}

		// Token: 0x0600A465 RID: 42085 RVA: 0x002F1628 File Offset: 0x002EF828
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignalEnable && (this.state == QuestPartState.NeverEnabled || (this.state == QuestPartState.Disabled && this.reactivatable)))
			{
				this.Enable(signal.args);
				return;
			}
			if (this.state == QuestPartState.Enabled)
			{
				if (signal.tag == this.inSignalDisable)
				{
					this.Disable();
					return;
				}
				this.ProcessQuestSignal(signal);
			}
		}

		// Token: 0x0600A466 RID: 42086 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void ProcessQuestSignal(Signal signal)
		{
		}

		// Token: 0x0600A467 RID: 42087 RVA: 0x002F1698 File Offset: 0x002EF898
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignalEnable = this.quest.InitiateSignal;
		}

		// Token: 0x0600A468 RID: 42088 RVA: 0x002D949A File Offset: 0x002D769A
		public void DebugForceComplete()
		{
			this.Complete();
		}

		// Token: 0x0600A469 RID: 42089 RVA: 0x002F16B4 File Offset: 0x002EF8B4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignalEnable, "inSignalEnable", null, false);
			Scribe_Values.Look<string>(ref this.inSignalDisable, "inSignalDisable", null, false);
			Scribe_Values.Look<bool>(ref this.reactivatable, "reactivatable", false, false);
			if (Scribe.mode != LoadSaveMode.Saving || !this.outSignalsCompleted.NullOrEmpty<string>())
			{
				Scribe_Collections.Look<string>(ref this.outSignalsCompleted, "outSignalsCompleted", LookMode.Value, Array.Empty<object>());
			}
			Scribe_Values.Look<QuestEndOutcome>(ref this.outcomeCompletedSignalArg, "outcomeCompletedSignalArg", QuestEndOutcome.Unknown, false);
			Scribe_Values.Look<QuestPartState>(ref this.state, "state", QuestPartState.NeverEnabled, false);
			Scribe_Values.Look<int>(ref this.enableTick, "enableTick", -1, false);
		}

		// Token: 0x04007172 RID: 29042
		public string inSignalEnable;

		// Token: 0x04007173 RID: 29043
		public string inSignalDisable;

		// Token: 0x04007174 RID: 29044
		public bool reactivatable;

		// Token: 0x04007175 RID: 29045
		public List<string> outSignalsCompleted = new List<string>();

		// Token: 0x04007176 RID: 29046
		public QuestEndOutcome outcomeCompletedSignalArg;

		// Token: 0x04007177 RID: 29047
		private QuestPartState state;

		// Token: 0x04007178 RID: 29048
		protected int enableTick = -1;

		// Token: 0x04007179 RID: 29049
		private Alert cachedAlert;
	}
}
