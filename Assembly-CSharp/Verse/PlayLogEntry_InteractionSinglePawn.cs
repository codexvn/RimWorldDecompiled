using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000347 RID: 839
	public class PlayLogEntry_InteractionSinglePawn : LogEntry
	{
		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06001569 RID: 5481 RVA: 0x0006541F File Offset: 0x0006361F
		protected string InitiatorName
		{
			get
			{
				if (this.initiator == null)
				{
					return "null";
				}
				return this.initiator.LabelShort;
			}
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x0005EA3A File Offset: 0x0005CC3A
		public PlayLogEntry_InteractionSinglePawn() : base(null)
		{
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x0006543A File Offset: 0x0006363A
		public PlayLogEntry_InteractionSinglePawn(InteractionDef intDef, Pawn initiator, List<RulePackDef> extraSentencePacks) : base(null)
		{
			this.intDef = intDef;
			this.initiator = initiator;
			this.extraSentencePacks = extraSentencePacks;
			this.initiatorFaction = initiator.Faction;
			this.initiatorIdeo = initiator.Ideo;
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x00065470 File Offset: 0x00063670
		public override bool Concerns(Thing t)
		{
			return t == this.initiator;
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x0006547B File Offset: 0x0006367B
		public override IEnumerable<Thing> GetConcerns()
		{
			if (this.initiator != null)
			{
				yield return this.initiator;
			}
			yield break;
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x0006548B File Offset: 0x0006368B
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return pov == this.initiator && CameraJumper.CanJump(this.initiator);
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x000654A8 File Offset: 0x000636A8
		public override void ClickedFromPOV(Thing pov)
		{
			if (pov == this.initiator)
			{
				CameraJumper.TryJumpAndSelect(this.initiator, CameraJumper.MovementMode.Pan);
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06001570 RID: 5488 RVA: 0x000654CA File Offset: 0x000636CA
		public override Texture2D IconFromPOV(Thing pov)
		{
			return this.intDef.GetSymbol(this.initiatorFaction, this.initiatorIdeo);
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x000654E3 File Offset: 0x000636E3
		public override Color? IconColorFromPOV(Thing pov)
		{
			return this.intDef.GetSymbolColor(this.initiatorFaction);
		}

		// Token: 0x06001572 RID: 5490 RVA: 0x000654F6 File Offset: 0x000636F6
		public override void Notify_FactionRemoved(Faction faction)
		{
			if (this.initiatorFaction == faction)
			{
				this.initiatorFaction = null;
			}
		}

		// Token: 0x06001573 RID: 5491 RVA: 0x00065508 File Offset: 0x00063708
		public override void Notify_IdeoRemoved(Ideo ideo)
		{
			if (this.initiatorIdeo == ideo)
			{
				this.initiatorIdeo = null;
			}
		}

		// Token: 0x06001574 RID: 5492 RVA: 0x0006551A File Offset: 0x0006371A
		public override string GetTipString()
		{
			return this.intDef.LabelCap + "\n" + base.GetTipString();
		}

		// Token: 0x06001575 RID: 5493 RVA: 0x00065544 File Offset: 0x00063744
		protected override string ToGameStringFromPOV_Worker(Thing pov, bool forceLog)
		{
			if (this.initiator == null)
			{
				Log.ErrorOnce("PlayLogEntry_InteractionSinglePawn has a null pawn reference.", 34422);
				return "[" + this.intDef.label + " error: null pawn reference]";
			}
			Rand.PushState();
			Rand.Seed = this.logID;
			GrammarRequest request = base.GenerateGrammarRequest();
			string text;
			if (pov == this.initiator)
			{
				request.IncludesBare.Add(this.intDef.logRulesInitiator);
				request.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiator, request.Constants, true, true));
				text = GrammarResolver.Resolve("r_logentry", request, "interaction from initiator", forceLog, null, null, null, true);
			}
			else
			{
				Log.ErrorOnce("Cannot display PlayLogEntry_InteractionSinglePawn from POV who isn't initiator.", 51251);
				text = this.ToString();
			}
			if (this.extraSentencePacks != null)
			{
				for (int i = 0; i < this.extraSentencePacks.Count; i++)
				{
					request.Clear();
					request.Includes.Add(this.extraSentencePacks[i]);
					request.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiator, request.Constants, true, true));
					text = text + " " + GrammarResolver.Resolve(this.extraSentencePacks[i].FirstRuleKeyword, request, "extraSentencePack", forceLog, this.extraSentencePacks[i].FirstUntranslatedRuleKeyword, null, null, true);
				}
			}
			Rand.PopState();
			return text;
		}

		// Token: 0x06001576 RID: 5494 RVA: 0x000656BC File Offset: 0x000638BC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<InteractionDef>(ref this.intDef, "intDef");
			Scribe_References.Look<Pawn>(ref this.initiator, "initiator", true);
			Scribe_Collections.Look<RulePackDef>(ref this.extraSentencePacks, "extras", LookMode.Undefined, Array.Empty<object>());
			Scribe_References.Look<Faction>(ref this.initiatorFaction, "initiatorFaction", false);
			Scribe_References.Look<Ideo>(ref this.initiatorIdeo, "initiatorIdeo", false);
		}

		// Token: 0x06001577 RID: 5495 RVA: 0x00065728 File Offset: 0x00063928
		public override string ToString()
		{
			return this.intDef.label + ": " + this.InitiatorName;
		}

		// Token: 0x040013F1 RID: 5105
		protected InteractionDef intDef;

		// Token: 0x040013F2 RID: 5106
		protected Pawn initiator;

		// Token: 0x040013F3 RID: 5107
		protected List<RulePackDef> extraSentencePacks;

		// Token: 0x040013F4 RID: 5108
		public Faction initiatorFaction;

		// Token: 0x040013F5 RID: 5109
		public Ideo initiatorIdeo;
	}
}
