using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002ABB RID: 10939
	public class SignalAction_Delay : SignalAction
	{
		// Token: 0x1700261A RID: 9754
		// (get) Token: 0x0600F3BD RID: 62397 RVA: 0x0046D239 File Offset: 0x0046B439
		public bool Activated
		{
			get
			{
				return this.activated;
			}
		}

		// Token: 0x1700261B RID: 9755
		// (get) Token: 0x0600F3BE RID: 62398 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual Alert_ActionDelay Alert
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700261C RID: 9756
		// (get) Token: 0x0600F3BF RID: 62399 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool ShouldRemoveNow
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600F3C0 RID: 62400 RVA: 0x0046D241 File Offset: 0x0046B441
		public override void Notify_SignalReceived(Signal signal)
		{
			if (signal.tag == this.signalTag)
			{
				this.DoAction(signal.args);
			}
		}

		// Token: 0x0600F3C1 RID: 62401 RVA: 0x0046D262 File Offset: 0x0046B462
		protected override void DoAction(SignalArgs args)
		{
			if (this.delayTicks <= 0)
			{
				this.CompleteInt();
				return;
			}
			this.activated = true;
		}

		// Token: 0x0600F3C2 RID: 62402 RVA: 0x0046D27B File Offset: 0x0046B47B
		protected virtual void Complete()
		{
			if (!this.completedSignalTag.NullOrEmpty())
			{
				Find.SignalManager.SendSignal(new Signal(this.completedSignalTag, false));
			}
		}

		// Token: 0x0600F3C3 RID: 62403 RVA: 0x0046D2A0 File Offset: 0x0046B4A0
		private void CompleteInt()
		{
			this.Complete();
			if (!base.Destroyed)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600F3C4 RID: 62404 RVA: 0x0046D2B7 File Offset: 0x0046B4B7
		protected override void Tick()
		{
			if (!this.activated)
			{
				return;
			}
			if (this.delayTicks <= 0)
			{
				this.CompleteInt();
				return;
			}
			if (this.ShouldRemoveNow)
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			this.delayTicks--;
		}

		// Token: 0x0600F3C5 RID: 62405 RVA: 0x0046D2F0 File Offset: 0x0046B4F0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.activated, "activated", false, false);
			Scribe_Values.Look<int>(ref this.delayTicks, "delayTicks", 0, false);
			Scribe_Values.Look<string>(ref this.completedSignalTag, "completedSignalTag", null, false);
		}

		// Token: 0x0400A093 RID: 41107
		public int delayTicks;

		// Token: 0x0400A094 RID: 41108
		public string completedSignalTag;

		// Token: 0x0400A095 RID: 41109
		private bool activated;
	}
}
