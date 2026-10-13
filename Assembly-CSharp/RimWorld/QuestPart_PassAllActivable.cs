using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B87 RID: 7047
	public class QuestPart_PassAllActivable : QuestPartActivable
	{
		// Token: 0x17001837 RID: 6199
		// (get) Token: 0x06009D5A RID: 40282 RVA: 0x002D9CD0 File Offset: 0x002D7ED0
		private bool AllSignalsReceived
		{
			get
			{
				if (this.inSignals.Count != this.signalsReceived.Count)
				{
					return false;
				}
				for (int i = 0; i < this.signalsReceived.Count; i++)
				{
					if (!this.signalsReceived[i])
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17001838 RID: 6200
		// (get) Token: 0x06009D5B RID: 40283 RVA: 0x002D9D1E File Offset: 0x002D7F1E
		public int SignalsReceivedCount
		{
			get
			{
				return this.signalsReceived.Count((bool s) => s);
			}
		}

		// Token: 0x17001839 RID: 6201
		// (get) Token: 0x06009D5C RID: 40284 RVA: 0x002D9D4C File Offset: 0x002D7F4C
		public override string ExpiryInfoPart
		{
			get
			{
				if (this.expiryInfoPartKey.NullOrEmpty())
				{
					return null;
				}
				return this.expiryInfoPartKey.Translate() + " " + this.SignalsReceivedCount.ToString() + " / " + this.inSignals.Count.ToString();
			}
		}

		// Token: 0x06009D5D RID: 40285 RVA: 0x002D9DAD File Offset: 0x002D7FAD
		protected override void Enable(SignalArgs receivedArgs)
		{
			this.signalsReceived.Clear();
			base.Enable(receivedArgs);
		}

		// Token: 0x06009D5E RID: 40286 RVA: 0x002D9DC4 File Offset: 0x002D7FC4
		protected override void ProcessQuestSignal(Signal signal)
		{
			base.ProcessQuestSignal(signal);
			int num = this.inSignals.IndexOf(signal.tag);
			if (num >= 0)
			{
				while (this.signalsReceived.Count <= num)
				{
					this.signalsReceived.Add(false);
				}
				this.signalsReceived[num] = true;
				if (!this.outSignalAny.NullOrEmpty())
				{
					SignalArgs args = signal.args;
					args.Add(this.SignalsReceivedCount.Named("COUNT"));
					Find.SignalManager.SendSignal(new Signal(this.outSignalAny, args, false));
				}
				if (this.AllSignalsReceived)
				{
					base.Complete();
				}
			}
		}

		// Token: 0x06009D5F RID: 40287 RVA: 0x002D9E6C File Offset: 0x002D806C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<bool>(ref this.signalsReceived, "signalsReceived", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignalAny, "outSignalAny", null, false);
			Scribe_Values.Look<string>(ref this.expiryInfoPartKey, "expiryInfoPartKey", null, false);
		}

		// Token: 0x06009D60 RID: 40288 RVA: 0x002D9ED0 File Offset: 0x002D80D0
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignals.Clear();
			for (int i = 0; i < 3; i++)
			{
				this.inSignals.Add("DebugSignal" + Rand.Int.ToString());
			}
		}

		// Token: 0x04006D13 RID: 27923
		public List<string> inSignals = new List<string>();

		// Token: 0x04006D14 RID: 27924
		public string outSignalAny;

		// Token: 0x04006D15 RID: 27925
		public string expiryInfoPartKey;

		// Token: 0x04006D16 RID: 27926
		private List<bool> signalsReceived = new List<bool>();
	}
}
