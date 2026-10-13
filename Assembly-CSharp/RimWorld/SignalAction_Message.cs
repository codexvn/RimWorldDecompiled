using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002AC0 RID: 10944
	public class SignalAction_Message : SignalAction
	{
		// Token: 0x0600F3D7 RID: 62423 RVA: 0x0046D878 File Offset: 0x0046BA78
		public override void Notify_SignalReceived(Signal signal)
		{
			if (signal.tag != this.signalTag)
			{
				return;
			}
			if (this.allMustBeAsleep)
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.lookTargets.targets)
				{
					if (!globalTargetInfo.Pawn.DestroyedOrNull() && globalTargetInfo.Pawn.Spawned && !globalTargetInfo.Pawn.Dead && globalTargetInfo.Pawn.Awake())
					{
						return;
					}
				}
			}
			base.Notify_SignalReceived(signal);
		}

		// Token: 0x0600F3D8 RID: 62424 RVA: 0x0046D928 File Offset: 0x0046BB28
		protected override void DoAction(SignalArgs args)
		{
			Messages.Message(this.message, this.lookTargets, this.messageType ?? MessageTypeDefOf.NeutralEvent, this.historical);
		}

		// Token: 0x0600F3D9 RID: 62425 RVA: 0x0046D950 File Offset: 0x0046BB50
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.message, "message", null, false);
			Scribe_Values.Look<bool>(ref this.historical, "historical", false, false);
			Scribe_Values.Look<bool>(ref this.allMustBeAsleep, "allMustBeAsleep", false, false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_Defs.Look<MessageTypeDef>(ref this.messageType, "messageType");
		}

		// Token: 0x0400A0AA RID: 41130
		public string message;

		// Token: 0x0400A0AB RID: 41131
		public bool historical = true;

		// Token: 0x0400A0AC RID: 41132
		public MessageTypeDef messageType;

		// Token: 0x0400A0AD RID: 41133
		public LookTargets lookTargets;

		// Token: 0x0400A0AE RID: 41134
		public bool allMustBeAsleep;
	}
}
