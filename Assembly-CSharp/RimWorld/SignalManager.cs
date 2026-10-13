using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D23 RID: 7459
	public class SignalManager
	{
		// Token: 0x0600A6D9 RID: 42713 RVA: 0x002FC97D File Offset: 0x002FAB7D
		public void RegisterReceiver(ISignalReceiver receiver)
		{
			if (receiver == null)
			{
				Log.Error("Tried to register a null receiver.");
				return;
			}
			if (this.receivers.Contains(receiver))
			{
				Log.Error("Tried to register the same receiver twice: " + receiver.ToStringSafe<ISignalReceiver>());
				return;
			}
			this.receivers.Add(receiver);
		}

		// Token: 0x0600A6DA RID: 42714 RVA: 0x002FC9BD File Offset: 0x002FABBD
		public void DeregisterReceiver(ISignalReceiver receiver)
		{
			this.receivers.Remove(receiver);
		}

		// Token: 0x0600A6DB RID: 42715 RVA: 0x002FC9CC File Offset: 0x002FABCC
		public void SendSignal(Signal signal)
		{
			if (this.signalsThisFrame >= 3000)
			{
				if (this.signalsThisFrame == 3000)
				{
					Log.Error("Reached max signals per frame (" + 3000.ToString() + "). Ignoring further signals.");
				}
				this.signalsThisFrame++;
				return;
			}
			this.signalsThisFrame++;
			if (DebugViewSettings.logSignals)
			{
				Log.Message("Signal: tag=" + signal.tag.ToStringSafe<string>() + " args=" + signal.args.Args.ToStringSafeEnumerable());
			}
			SignalManager.tmpReceivers.Clear();
			SignalManager.tmpReceivers.AddRange(this.receivers);
			for (int i = 0; i < SignalManager.tmpReceivers.Count; i++)
			{
				try
				{
					SignalManager.tmpReceivers[i].Notify_SignalReceived(signal);
				}
				catch (Exception ex)
				{
					string str = "Error while sending signal to ";
					string str2 = SignalManager.tmpReceivers[i].ToStringSafe<ISignalReceiver>();
					string str3 = ": ";
					Exception ex2 = ex;
					Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
		}

		// Token: 0x0600A6DC RID: 42716 RVA: 0x002FCAF0 File Offset: 0x002FACF0
		public void SignalManagerUpdate()
		{
			this.signalsThisFrame = 0;
		}

		// Token: 0x04007387 RID: 29575
		private int signalsThisFrame;

		// Token: 0x04007388 RID: 29576
		private const int MaxSignalsPerFrame = 3000;

		// Token: 0x04007389 RID: 29577
		public List<ISignalReceiver> receivers = new List<ISignalReceiver>();

		// Token: 0x0400738A RID: 29578
		private static List<ISignalReceiver> tmpReceivers = new List<ISignalReceiver>();
	}
}
