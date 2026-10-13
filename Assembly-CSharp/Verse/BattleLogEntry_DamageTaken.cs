using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000324 RID: 804
	public class BattleLogEntry_DamageTaken : LogEntry_DamageResult
	{
		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06001403 RID: 5123 RVA: 0x0005E7CE File Offset: 0x0005C9CE
		private string RecipientName
		{
			get
			{
				if (this.recipientPawn == null)
				{
					return "null";
				}
				return this.recipientPawn.LabelShort;
			}
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x0005E7E9 File Offset: 0x0005C9E9
		public BattleLogEntry_DamageTaken() : base(null)
		{
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x0005E7F2 File Offset: 0x0005C9F2
		public BattleLogEntry_DamageTaken(Pawn recipient, RulePackDef ruleDef, Pawn initiator = null) : base(null)
		{
			this.initiatorPawn = initiator;
			this.recipientPawn = recipient;
			this.ruleDef = ruleDef;
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x0005E810 File Offset: 0x0005CA10
		public override bool Concerns(Thing t)
		{
			return t == this.initiatorPawn || t == this.recipientPawn;
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x0005E826 File Offset: 0x0005CA26
		public override IEnumerable<Thing> GetConcerns()
		{
			if (this.initiatorPawn != null)
			{
				yield return this.initiatorPawn;
			}
			if (this.recipientPawn != null)
			{
				yield return this.recipientPawn;
			}
			yield break;
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x0005E836 File Offset: 0x0005CA36
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return CameraJumper.CanJump(this.recipientPawn);
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x0005E848 File Offset: 0x0005CA48
		public override void ClickedFromPOV(Thing pov)
		{
			CameraJumper.TryJumpAndSelect(this.recipientPawn, CameraJumper.MovementMode.Pan);
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x0005E85B File Offset: 0x0005CA5B
		public override Texture2D IconFromPOV(Thing pov)
		{
			return LogEntry.Blood;
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x0005E862 File Offset: 0x0005CA62
		protected override BodyDef DamagedBody()
		{
			if (this.recipientPawn == null)
			{
				return null;
			}
			return this.recipientPawn.RaceProps.body;
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x0005E880 File Offset: 0x0005CA80
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			if (this.recipientPawn == null)
			{
				Log.ErrorOnce("BattleLogEntry_DamageTaken has a null recipient.", 60465709);
			}
			result.Includes.Add(this.ruleDef);
			result.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipientPawn, result.Constants, true, true));
			return result;
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x0005E8E3 File Offset: 0x0005CAE3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<RulePackDef>(ref this.ruleDef, "ruleDef");
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x0005E91D File Offset: 0x0005CB1D
		public override string ToString()
		{
			return "BattleLogEntry_DamageTaken: " + this.RecipientName;
		}

		// Token: 0x040012F9 RID: 4857
		private Pawn initiatorPawn;

		// Token: 0x040012FA RID: 4858
		private Pawn recipientPawn;

		// Token: 0x040012FB RID: 4859
		private RulePackDef ruleDef;
	}
}
