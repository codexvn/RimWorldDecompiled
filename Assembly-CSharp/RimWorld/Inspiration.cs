using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200110F RID: 4367
	public class Inspiration : IExposable
	{
		// Token: 0x17001068 RID: 4200
		// (get) Token: 0x06006A72 RID: 27250 RVA: 0x00219D91 File Offset: 0x00217F91
		public int Age
		{
			get
			{
				return this.age;
			}
		}

		// Token: 0x17001069 RID: 4201
		// (get) Token: 0x06006A73 RID: 27251 RVA: 0x00219D99 File Offset: 0x00217F99
		public float AgeDays
		{
			get
			{
				return (float)this.age / 60000f;
			}
		}

		// Token: 0x1700106A RID: 4202
		// (get) Token: 0x06006A74 RID: 27252 RVA: 0x00219DA8 File Offset: 0x00217FA8
		public virtual string InspectLine
		{
			get
			{
				int numTicks = (int)((this.def.baseDurationDays - this.AgeDays) * 60000f);
				return this.def.baseInspectLine + " (" + "ExpiresIn".Translate() + ": " + numTicks.ToStringTicksToPeriod(true, false, true, true, false) + ")";
			}
		}

		// Token: 0x1700106B RID: 4203
		// (get) Token: 0x06006A75 RID: 27253 RVA: 0x00219E1C File Offset: 0x0021801C
		public virtual string LetterText
		{
			get
			{
				string text = this.def.beginLetter.Formatted(this.pawn.LabelCap, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true).Resolve();
				if (!string.IsNullOrWhiteSpace(this.reason))
				{
					text = this.reason.Formatted(this.pawn.LabelCap, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true).Resolve() + "\n\n" + text;
				}
				return text;
			}
		}

		// Token: 0x06006A76 RID: 27254 RVA: 0x00219ED7 File Offset: 0x002180D7
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<InspirationDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Values.Look<string>(ref this.reason, "reason", null, false);
		}

		// Token: 0x06006A77 RID: 27255 RVA: 0x00219F0D File Offset: 0x0021810D
		public virtual void InspirationTick(int delta)
		{
			this.age += delta;
			if (this.AgeDays >= this.def.baseDurationDays)
			{
				this.End();
			}
		}

		// Token: 0x06006A78 RID: 27256 RVA: 0x00219F36 File Offset: 0x00218136
		public virtual void PostStart(bool sendLetter = true)
		{
			if (sendLetter)
			{
				this.SendBeginLetter();
			}
		}

		// Token: 0x06006A79 RID: 27257 RVA: 0x00219F41 File Offset: 0x00218141
		public virtual void PostEnd()
		{
			this.AddEndMessage();
		}

		// Token: 0x06006A7A RID: 27258 RVA: 0x00219F49 File Offset: 0x00218149
		protected void End()
		{
			this.pawn.mindState.inspirationHandler.EndInspiration(this);
		}

		// Token: 0x06006A7B RID: 27259 RVA: 0x00219F64 File Offset: 0x00218164
		protected virtual void SendBeginLetter()
		{
			if (this.def.beginLetter.NullOrEmpty())
			{
				return;
			}
			if (!PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				return;
			}
			string str = (this.def.beginLetterLabel ?? this.def.LabelCap).CapitalizeFirst() + ": " + this.pawn.LabelShortCap;
			Find.LetterStack.ReceiveLetter(str, this.LetterText, this.def.beginLetterDef, this.pawn, null, null, null, null, 0, true);
		}

		// Token: 0x06006A7C RID: 27260 RVA: 0x0021A004 File Offset: 0x00218204
		protected virtual void AddEndMessage()
		{
			if (this.def.endMessage.NullOrEmpty())
			{
				return;
			}
			if (!PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				return;
			}
			Messages.Message(this.def.endMessage.Formatted(this.pawn.LabelCap, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true), this.pawn, MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x04004B2F RID: 19247
		public Pawn pawn;

		// Token: 0x04004B30 RID: 19248
		public InspirationDef def;

		// Token: 0x04004B31 RID: 19249
		private int age;

		// Token: 0x04004B32 RID: 19250
		public string reason;
	}
}
