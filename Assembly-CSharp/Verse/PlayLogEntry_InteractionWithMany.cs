using System;
using System.Collections.Generic;
using RimWorld;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000349 RID: 841
	public class PlayLogEntry_InteractionWithMany : PlayLogEntry_Interaction
	{
		// Token: 0x06001580 RID: 5504 RVA: 0x00065803 File Offset: 0x00063A03
		public PlayLogEntry_InteractionWithMany()
		{
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x0006580B File Offset: 0x00063A0B
		public PlayLogEntry_InteractionWithMany(InteractionDef intDef, Pawn initiator, List<Pawn> recipients, List<RulePackDef> extraSentencePacks)
		{
			this.intDef = intDef;
			this.initiator = initiator;
			this.recipients = recipients;
			this.extraSentencePacks = extraSentencePacks;
			this.initiatorFaction = initiator.Faction;
			this.initiatorIdeo = initiator.Ideo;
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x00065848 File Offset: 0x00063A48
		public override bool Concerns(Thing t)
		{
			Pawn pawn = t as Pawn;
			return pawn != null && (t == this.initiator || this.recipients.Contains(pawn));
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x00065878 File Offset: 0x00063A78
		public override IEnumerable<Thing> GetConcerns()
		{
			yield return this.initiator;
			foreach (Pawn pawn in this.recipients)
			{
				yield return pawn;
			}
			List<Pawn>.Enumerator enumerator = default(List<Pawn>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x00065888 File Offset: 0x00063A88
		protected override string ToGameStringFromPOV_Worker(Thing pov, bool forceLog)
		{
			if (this.initiator == null || this.recipients == null || this.recipients.Contains(null))
			{
				Log.ErrorOnce("PlayLogEntry_Interaction has a null pawn reference.", 34422);
				return "[" + this.intDef.label + " error: null pawn reference]";
			}
			Rand.PushState();
			Rand.Seed = this.logID;
			GrammarRequest request = base.GenerateGrammarRequest();
			string result;
			if (pov == this.initiator)
			{
				request.IncludesBare.Add(this.intDef.logRulesInitiator);
				request.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiator, request.Constants, true, true));
				result = GrammarResolver.Resolve("r_logentry", request, "interaction from initiator", forceLog, null, null, null, true);
			}
			else
			{
				Pawn pawn = pov as Pawn;
				if (pawn != null && this.recipients.Contains(pawn))
				{
					if (this.intDef.logRulesRecipient != null)
					{
						request.IncludesBare.Add(this.intDef.logRulesRecipient);
					}
					else
					{
						request.IncludesBare.Add(this.intDef.logRulesInitiator);
					}
					request.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiator, request.Constants, true, true));
					request.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", pawn, request.Constants, true, true));
					result = GrammarResolver.Resolve("r_logentry", request, "interaction from recipient", forceLog, null, null, null, true);
				}
				else
				{
					Log.ErrorOnce("Cannot display PlayLogEntry_Interaction from POV who isn't initiator or recipient.", 51251);
					result = this.ToString();
				}
			}
			Rand.PopState();
			return result;
		}

		// Token: 0x06001585 RID: 5509 RVA: 0x00065A28 File Offset: 0x00063C28
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return this.recipients.Contains(pov as Pawn) && CameraJumper.CanJump(this.initiator);
		}

		// Token: 0x06001586 RID: 5510 RVA: 0x00065A50 File Offset: 0x00063C50
		public override void ClickedFromPOV(Thing pov)
		{
			if (pov == this.initiator)
			{
				CameraJumper.TryJumpAndSelect(this.recipients.RandomElement<Pawn>(), CameraJumper.MovementMode.Pan);
				return;
			}
			if (this.recipients.Contains(pov as Pawn))
			{
				CameraJumper.TryJumpAndSelect(this.initiator, CameraJumper.MovementMode.Pan);
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06001587 RID: 5511 RVA: 0x00065AA7 File Offset: 0x00063CA7
		public override string ToString()
		{
			return this.intDef.label + ": " + base.InitiatorName + "-> Many";
		}

		// Token: 0x06001588 RID: 5512 RVA: 0x00065ACC File Offset: 0x00063CCC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.recipients, "recipients", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.recipients.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x040013FA RID: 5114
		private List<Pawn> recipients;
	}
}
