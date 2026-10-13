using System;
using System.Collections.Generic;
using LudeonTK;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x0200032F RID: 815
	public class BattleLogEntry_RangedImpact : LogEntry_DamageResult
	{
		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06001470 RID: 5232 RVA: 0x0005FEEF File Offset: 0x0005E0EF
		private string InitiatorName
		{
			get
			{
				if (this.initiatorPawn != null)
				{
					return this.initiatorPawn.LabelShort;
				}
				if (this.initiatorThing != null)
				{
					return this.initiatorThing.defName;
				}
				return "null";
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06001471 RID: 5233 RVA: 0x0005FF1E File Offset: 0x0005E11E
		private string RecipientName
		{
			get
			{
				if (this.recipientPawn != null)
				{
					return this.recipientPawn.LabelShort;
				}
				if (this.recipientThing != null)
				{
					return this.recipientThing.defName;
				}
				return "null";
			}
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x0005E7E9 File Offset: 0x0005C9E9
		public BattleLogEntry_RangedImpact() : base(null)
		{
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x0005FF50 File Offset: 0x0005E150
		public BattleLogEntry_RangedImpact(Thing initiator, Thing recipient, Thing originalTarget, ThingDef weaponDef, ThingDef projectileDef, ThingDef coverDef) : base(null)
		{
			Pawn pawn = initiator as Pawn;
			if (pawn != null)
			{
				this.initiatorPawn = pawn;
			}
			else if (initiator != null)
			{
				this.initiatorThing = initiator.def;
			}
			Pawn pawn2 = recipient as Pawn;
			if (pawn2 != null)
			{
				this.recipientPawn = pawn2;
			}
			else if (recipient != null)
			{
				this.recipientThing = recipient.def;
			}
			Pawn pawn3 = originalTarget as Pawn;
			if (pawn3 != null)
			{
				this.originalTargetPawn = pawn3;
				this.originalTargetMobile = (!this.originalTargetPawn.Downed && !this.originalTargetPawn.Dead && this.originalTargetPawn.Awake());
			}
			else if (originalTarget != null)
			{
				this.originalTargetThing = originalTarget.def;
			}
			this.weaponDef = weaponDef;
			this.projectileDef = projectileDef;
			this.coverDef = coverDef;
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x00060010 File Offset: 0x0005E210
		public override bool Concerns(Thing t)
		{
			return t == this.initiatorPawn || t == this.recipientPawn || t == this.originalTargetPawn;
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x0006002F File Offset: 0x0005E22F
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
			if (this.originalTargetPawn != null)
			{
				yield return this.originalTargetPawn;
			}
			yield break;
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x00060040 File Offset: 0x0005E240
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return this.recipientPawn != null && ((pov == this.initiatorPawn && CameraJumper.CanJump(this.recipientPawn)) || (pov == this.recipientPawn && CameraJumper.CanJump(this.initiatorPawn)));
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x00060090 File Offset: 0x0005E290
		public override void ClickedFromPOV(Thing pov)
		{
			if (this.recipientPawn == null)
			{
				return;
			}
			if (pov == this.initiatorPawn)
			{
				CameraJumper.TryJumpAndSelect(this.recipientPawn, CameraJumper.MovementMode.Pan);
				return;
			}
			if (pov == this.recipientPawn)
			{
				CameraJumper.TryJumpAndSelect(this.initiatorPawn, CameraJumper.MovementMode.Pan);
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x000600E1 File Offset: 0x0005E2E1
		public override Texture2D IconFromPOV(Thing pov)
		{
			if (this.damagedParts.NullOrEmpty<BodyPartRecord>())
			{
				return null;
			}
			if (this.deflected)
			{
				return null;
			}
			if (pov == null || pov == this.recipientPawn)
			{
				return LogEntry.Blood;
			}
			if (pov == this.initiatorPawn)
			{
				return LogEntry.BloodTarget;
			}
			return null;
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x0006011E File Offset: 0x0005E31E
		protected override BodyDef DamagedBody()
		{
			Pawn pawn = this.recipientPawn;
			if (pawn == null)
			{
				return null;
			}
			return pawn.RaceProps.body;
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x00060138 File Offset: 0x0005E338
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			if (this.recipientPawn != null || this.recipientThing != null)
			{
				result.Includes.Add(this.deflected ? RulePackDefOf.Combat_RangedDeflect : RulePackDefOf.Combat_RangedDamage);
			}
			else
			{
				result.Includes.Add(RulePackDefOf.Combat_RangedMiss);
			}
			if (this.initiatorPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiatorPawn, result.Constants, true, true));
			}
			else if (this.initiatorThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("INITIATOR", this.initiatorThing));
			}
			else
			{
				result.Constants["INITIATOR_missing"] = "True";
			}
			if (this.recipientPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipientPawn, result.Constants, true, true));
			}
			else if (this.recipientThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("RECIPIENT", this.recipientThing));
			}
			else
			{
				result.Constants["RECIPIENT_missing"] = "True";
			}
			if (this.originalTargetPawn != this.recipientPawn || this.originalTargetThing != this.recipientThing)
			{
				if (this.originalTargetPawn != null)
				{
					result.Rules.AddRange(GrammarUtility.RulesForPawn("ORIGINALTARGET", this.originalTargetPawn, result.Constants, true, true));
					result.Constants["ORIGINALTARGET_mobile"] = this.originalTargetMobile.ToString();
				}
				else if (this.originalTargetThing != null)
				{
					result.Rules.AddRange(GrammarUtility.RulesForDef("ORIGINALTARGET", this.originalTargetThing));
				}
				else
				{
					result.Constants["ORIGINALTARGET_missing"] = "True";
				}
			}
			if (this.weaponDef != null)
			{
				result.Rules.AddRange(PlayLogEntryUtility.RulesForOptionalWeapon("WEAPON", this.weaponDef, this.projectileDef));
			}
			else
			{
				result.Constants["WEAPON_missing"] = "True";
				if (this.projectileDef != null)
				{
					result.Rules.AddRange(GrammarUtility.RulesForDef("PROJECTILE", this.projectileDef));
				}
			}
			if (this.initiatorPawn != null && this.initiatorPawn.skills != null)
			{
				result.Constants["INITIATOR_skill"] = this.initiatorPawn.skills.GetSkill(SkillDefOf.Shooting).Level.ToStringCached();
			}
			if (this.recipientPawn != null && this.recipientPawn.skills != null)
			{
				result.Constants["RECIPIENT_skill"] = this.recipientPawn.skills.GetSkill(SkillDefOf.Shooting).Level.ToStringCached();
			}
			result.Constants["COVER_missing"] = ((this.coverDef != null) ? "False" : "True");
			if (this.coverDef != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("COVER", this.coverDef));
			}
			return result;
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x00060448 File Offset: 0x0005E648
		public override bool ShowInCompactView()
		{
			if (!this.deflected)
			{
				if (this.recipientPawn != null)
				{
					return true;
				}
				if (this.originalTargetThing != null && this.originalTargetThing == this.recipientThing)
				{
					return true;
				}
			}
			int num = 1;
			if (this.weaponDef != null && !this.weaponDef.Verbs.NullOrEmpty<VerbProperties>())
			{
				num = this.weaponDef.Verbs[0].burstShotCount;
			}
			return Rand.ChanceSeeded(BattleLogEntry_RangedImpact.DisplayChanceOnMiss / (float)num, this.logID);
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x000604C8 File Offset: 0x0005E6C8
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (this.initiatorPawn != null && this.initiatorPawn.Discarded)
				{
					this.initiatorPawn = null;
				}
				if (this.recipientPawn != null && this.recipientPawn.Discarded)
				{
					this.recipientPawn = null;
				}
				if (this.originalTargetPawn != null && this.originalTargetPawn.Discarded)
				{
					this.originalTargetPawn = null;
				}
			}
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.initiatorThing, "initiatorThing");
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.recipientThing, "recipientThing");
			Scribe_References.Look<Pawn>(ref this.originalTargetPawn, "originalTargetPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.originalTargetThing, "originalTargetThing");
			Scribe_Values.Look<bool>(ref this.originalTargetMobile, "originalTargetMobile", false, false);
			Scribe_Defs.Look<ThingDef>(ref this.weaponDef, "weaponDef");
			Scribe_Defs.Look<ThingDef>(ref this.projectileDef, "projectileDef");
			Scribe_Defs.Look<ThingDef>(ref this.coverDef, "coverDef");
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x000605DC File Offset: 0x0005E7DC
		public override string ToString()
		{
			return "BattleLogEntry_RangedImpact: " + this.InitiatorName + "->" + this.RecipientName;
		}

		// Token: 0x0400132F RID: 4911
		private Pawn initiatorPawn;

		// Token: 0x04001330 RID: 4912
		private ThingDef initiatorThing;

		// Token: 0x04001331 RID: 4913
		private Pawn recipientPawn;

		// Token: 0x04001332 RID: 4914
		private ThingDef recipientThing;

		// Token: 0x04001333 RID: 4915
		private Pawn originalTargetPawn;

		// Token: 0x04001334 RID: 4916
		private ThingDef originalTargetThing;

		// Token: 0x04001335 RID: 4917
		private bool originalTargetMobile;

		// Token: 0x04001336 RID: 4918
		private ThingDef weaponDef;

		// Token: 0x04001337 RID: 4919
		private ThingDef projectileDef;

		// Token: 0x04001338 RID: 4920
		private ThingDef coverDef;

		// Token: 0x04001339 RID: 4921
		[TweakValue("LogFilter", 0f, 1f)]
		private static float DisplayChanceOnMiss = 0.25f;
	}
}
