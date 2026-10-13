using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000326 RID: 806
	public class BattleLogEntry_Event : LogEntry
	{
		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06001417 RID: 5143 RVA: 0x0005EA1F File Offset: 0x0005CC1F
		private string SubjectName
		{
			get
			{
				if (this.subjectPawn == null)
				{
					return "null";
				}
				return this.subjectPawn.LabelShort;
			}
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x0005EA3A File Offset: 0x0005CC3A
		public BattleLogEntry_Event() : base(null)
		{
		}

		// Token: 0x06001419 RID: 5145 RVA: 0x0005EA44 File Offset: 0x0005CC44
		public BattleLogEntry_Event(Thing subject, RulePackDef eventDef, Thing initiator) : base(null)
		{
			if (subject is Pawn)
			{
				this.subjectPawn = (subject as Pawn);
			}
			else if (subject != null)
			{
				this.subjectThing = subject.def;
			}
			if (initiator is Pawn)
			{
				this.initiatorPawn = (initiator as Pawn);
			}
			else if (initiator != null)
			{
				this.initiatorThing = initiator.def;
			}
			this.eventDef = eventDef;
		}

		// Token: 0x0600141A RID: 5146 RVA: 0x0005EAA9 File Offset: 0x0005CCA9
		public override bool Concerns(Thing t)
		{
			return t == this.subjectPawn || t == this.initiatorPawn;
		}

		// Token: 0x0600141B RID: 5147 RVA: 0x0005EABF File Offset: 0x0005CCBF
		public override IEnumerable<Thing> GetConcerns()
		{
			if (this.subjectPawn != null)
			{
				yield return this.subjectPawn;
			}
			if (this.initiatorPawn != null)
			{
				yield return this.initiatorPawn;
			}
			yield break;
		}

		// Token: 0x0600141C RID: 5148 RVA: 0x0005EACF File Offset: 0x0005CCCF
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return (pov == this.subjectPawn && CameraJumper.CanJump(this.initiatorPawn)) || (pov == this.initiatorPawn && CameraJumper.CanJump(this.subjectPawn));
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x0005EB09 File Offset: 0x0005CD09
		public override void ClickedFromPOV(Thing pov)
		{
			if (pov == this.subjectPawn)
			{
				CameraJumper.TryJumpAndSelect(this.initiatorPawn, CameraJumper.MovementMode.Pan);
				return;
			}
			if (pov == this.initiatorPawn)
			{
				CameraJumper.TryJumpAndSelect(this.subjectPawn, CameraJumper.MovementMode.Pan);
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x0005EB48 File Offset: 0x0005CD48
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Includes.Add(this.eventDef);
			if (this.subjectPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("SUBJECT", this.subjectPawn, result.Constants, true, true));
			}
			else if (this.subjectThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("SUBJECT", this.subjectThing));
			}
			if (this.initiatorPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiatorPawn, result.Constants, true, true));
			}
			else if (this.initiatorThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("INITIATOR", this.initiatorThing));
			}
			return result;
		}

		// Token: 0x0600141F RID: 5151 RVA: 0x0005EC18 File Offset: 0x0005CE18
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RulePackDef>(ref this.eventDef, "eventDef");
			Scribe_References.Look<Pawn>(ref this.subjectPawn, "subjectPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.subjectThing, "subjectThing");
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.initiatorThing, "initiatorThing");
		}

		// Token: 0x06001420 RID: 5152 RVA: 0x0005EC7D File Offset: 0x0005CE7D
		public override string ToString()
		{
			string defName = this.eventDef.defName;
			string str = ": ";
			Pawn pawn = this.subjectPawn;
			return defName + str + ((pawn != null) ? pawn.ToString() : null);
		}

		// Token: 0x04001300 RID: 4864
		protected RulePackDef eventDef;

		// Token: 0x04001301 RID: 4865
		protected Pawn subjectPawn;

		// Token: 0x04001302 RID: 4866
		protected ThingDef subjectThing;

		// Token: 0x04001303 RID: 4867
		protected Pawn initiatorPawn;

		// Token: 0x04001304 RID: 4868
		protected ThingDef initiatorThing;
	}
}
