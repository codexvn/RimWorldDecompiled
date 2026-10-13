using System;
using System.Linq;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020010B0 RID: 4272
	public class TransitionAction_Message : TransitionAction
	{
		// Token: 0x060068D6 RID: 26838 RVA: 0x00211B55 File Offset: 0x0020FD55
		public TransitionAction_Message(string message, string repeatAvoiderTag = null, float repeatAvoiderSeconds = 1f) : this(message, MessageTypeDefOf.NeutralEvent, repeatAvoiderTag, repeatAvoiderSeconds, null)
		{
		}

		// Token: 0x060068D7 RID: 26839 RVA: 0x00211B66 File Offset: 0x0020FD66
		public TransitionAction_Message(string message, MessageTypeDef messageType, string repeatAvoiderTag = null, float repeatAvoiderSeconds = 1f, Func<bool> canSendMessage = null)
		{
			this.lookTarget = TargetInfo.Invalid;
			base..ctor();
			this.message = message;
			this.type = messageType;
			this.repeatAvoiderTag = repeatAvoiderTag;
			this.repeatAvoiderSeconds = repeatAvoiderSeconds;
			this.canSendMessage = canSendMessage;
		}

		// Token: 0x060068D8 RID: 26840 RVA: 0x00211B9E File Offset: 0x0020FD9E
		public TransitionAction_Message(string message, MessageTypeDef messageType, TargetInfo lookTarget, string repeatAvoiderTag = null, float repeatAvoiderSeconds = 1f)
		{
			this.lookTarget = TargetInfo.Invalid;
			base..ctor();
			this.message = message;
			this.type = messageType;
			this.lookTarget = lookTarget;
			this.repeatAvoiderTag = repeatAvoiderTag;
			this.repeatAvoiderSeconds = repeatAvoiderSeconds;
		}

		// Token: 0x060068D9 RID: 26841 RVA: 0x00211BD6 File Offset: 0x0020FDD6
		public TransitionAction_Message(string message, MessageTypeDef messageType, Func<TargetInfo> lookTargetGetter, string repeatAvoiderTag = null, float repeatAvoiderSeconds = 1f)
		{
			this.lookTarget = TargetInfo.Invalid;
			base..ctor();
			this.message = message;
			this.type = messageType;
			this.lookTargetGetter = lookTargetGetter;
			this.repeatAvoiderTag = repeatAvoiderTag;
			this.repeatAvoiderSeconds = repeatAvoiderSeconds;
		}

		// Token: 0x060068DA RID: 26842 RVA: 0x00211C10 File Offset: 0x0020FE10
		public override void DoAction(Transition trans)
		{
			if (!this.repeatAvoiderTag.NullOrEmpty() && !MessagesRepeatAvoider.MessageShowAllowed(this.repeatAvoiderTag, this.repeatAvoiderSeconds))
			{
				return;
			}
			if (this.canSendMessage != null && !this.canSendMessage())
			{
				return;
			}
			TargetInfo target = (this.lookTargetGetter != null) ? this.lookTargetGetter() : this.lookTarget;
			if (!target.IsValid)
			{
				target = trans.target.lord.ownedPawns.FirstOrDefault<Pawn>();
			}
			Messages.Message(this.message, target, this.type, true);
		}

		// Token: 0x04004A4B RID: 19019
		public string message;

		// Token: 0x04004A4C RID: 19020
		public MessageTypeDef type;

		// Token: 0x04004A4D RID: 19021
		public TargetInfo lookTarget;

		// Token: 0x04004A4E RID: 19022
		public Func<TargetInfo> lookTargetGetter;

		// Token: 0x04004A4F RID: 19023
		public string repeatAvoiderTag;

		// Token: 0x04004A50 RID: 19024
		public float repeatAvoiderSeconds;

		// Token: 0x04004A51 RID: 19025
		private Func<bool> canSendMessage;
	}
}
