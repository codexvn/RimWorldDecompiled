using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000331 RID: 817
	public class BattleLogEntry_StateTransition : LogEntry
	{
		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06001487 RID: 5255 RVA: 0x0006071B File Offset: 0x0005E91B
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

		// Token: 0x06001488 RID: 5256 RVA: 0x0005EA3A File Offset: 0x0005CC3A
		public BattleLogEntry_StateTransition() : base(null)
		{
		}

		// Token: 0x06001489 RID: 5257 RVA: 0x00060738 File Offset: 0x0005E938
		public BattleLogEntry_StateTransition(Thing subject, RulePackDef transitionDef, Pawn initiator, Hediff culpritHediff, BodyPartRecord culpritTargetDef) : base(null)
		{
			if (subject is Pawn)
			{
				this.subjectPawn = (subject as Pawn);
			}
			else if (subject != null)
			{
				this.subjectThing = subject.def;
			}
			this.transitionDef = transitionDef;
			this.initiator = initiator;
			if (culpritHediff != null)
			{
				this.culpritHediffDef = culpritHediff.def;
				if (culpritHediff.Part != null)
				{
					this.culpritHediffTargetPart = culpritHediff.Part;
				}
			}
			this.culpritTargetPart = culpritTargetDef;
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x000607AE File Offset: 0x0005E9AE
		public override bool Concerns(Thing t)
		{
			return t == this.subjectPawn || t == this.initiator;
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x000607C4 File Offset: 0x0005E9C4
		public override IEnumerable<Thing> GetConcerns()
		{
			if (this.initiator != null)
			{
				yield return this.initiator;
			}
			if (this.subjectPawn != null)
			{
				yield return this.subjectPawn;
			}
			yield break;
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x000607D4 File Offset: 0x0005E9D4
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return (pov == this.subjectPawn && CameraJumper.CanJump(this.initiator)) || (pov == this.initiator && CameraJumper.CanJump(this.subjectPawn));
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x0006080E File Offset: 0x0005EA0E
		public override void ClickedFromPOV(Thing pov)
		{
			if (pov == this.subjectPawn)
			{
				CameraJumper.TryJumpAndSelect(this.initiator, CameraJumper.MovementMode.Pan);
				return;
			}
			if (pov == this.initiator)
			{
				CameraJumper.TryJumpAndSelect(this.subjectPawn, CameraJumper.MovementMode.Pan);
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x0600148E RID: 5262 RVA: 0x0006084C File Offset: 0x0005EA4C
		public override Texture2D IconFromPOV(Thing pov)
		{
			if (pov == null || pov == this.subjectPawn)
			{
				if (this.transitionDef != RulePackDefOf.Transition_Downed)
				{
					return LogEntry.Skull;
				}
				return LogEntry.Downed;
			}
			else
			{
				if (pov != this.initiator)
				{
					return null;
				}
				if (this.transitionDef != RulePackDefOf.Transition_Downed)
				{
					return LogEntry.SkullTarget;
				}
				return LogEntry.DownedTarget;
			}
		}

		// Token: 0x0600148F RID: 5263 RVA: 0x000608A4 File Offset: 0x0005EAA4
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			if (this.subjectPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("SUBJECT", this.subjectPawn, result.Constants, true, true));
			}
			else if (this.subjectThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("SUBJECT", this.subjectThing));
			}
			result.Includes.Add(this.transitionDef);
			if (this.initiator != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiator, result.Constants, true, true));
			}
			if (this.culpritHediffDef != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForHediffDef("CULPRITHEDIFF", this.culpritHediffDef, this.culpritHediffTargetPart));
			}
			if (this.culpritHediffTargetPart != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForBodyPartRecord("CULPRITHEDIFF_target", this.culpritHediffTargetPart));
			}
			if (this.culpritTargetPart != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForBodyPartRecord("CULPRITHEDIFF_originaltarget", this.culpritTargetPart));
			}
			return result;
		}

		// Token: 0x06001490 RID: 5264 RVA: 0x000609C0 File Offset: 0x0005EBC0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RulePackDef>(ref this.transitionDef, "transitionDef");
			Scribe_References.Look<Pawn>(ref this.subjectPawn, "subjectPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.subjectThing, "subjectThing");
			Scribe_References.Look<Pawn>(ref this.initiator, "initiator", true);
			Scribe_Defs.Look<HediffDef>(ref this.culpritHediffDef, "culpritHediffDef");
			Scribe_BodyParts.Look(ref this.culpritHediffTargetPart, "culpritHediffTargetPart", null);
			Scribe_BodyParts.Look(ref this.culpritTargetPart, "culpritTargetPart", null);
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x00060A47 File Offset: 0x0005EC47
		public override string ToString()
		{
			string defName = this.transitionDef.defName;
			string str = ": ";
			Pawn pawn = this.subjectPawn;
			return defName + str + ((pawn != null) ? pawn.ToString() : null);
		}

		// Token: 0x0400133E RID: 4926
		private RulePackDef transitionDef;

		// Token: 0x0400133F RID: 4927
		private Pawn subjectPawn;

		// Token: 0x04001340 RID: 4928
		private ThingDef subjectThing;

		// Token: 0x04001341 RID: 4929
		private Pawn initiator;

		// Token: 0x04001342 RID: 4930
		private HediffDef culpritHediffDef;

		// Token: 0x04001343 RID: 4931
		private BodyPartRecord culpritHediffTargetPart;

		// Token: 0x04001344 RID: 4932
		private BodyPartRecord culpritTargetPart;
	}
}
