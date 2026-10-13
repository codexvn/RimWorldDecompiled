using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020022A0 RID: 8864
	public class CompAbilityEffect_UnnaturalHealing : CompAbilityEffect
	{
		// Token: 0x17001D7E RID: 7550
		// (get) Token: 0x0600C0DB RID: 49371 RVA: 0x0037DA01 File Offset: 0x0037BC01
		public new CompProperties_UnnaturalHealing Props
		{
			get
			{
				return (CompProperties_UnnaturalHealing)this.props;
			}
		}

		// Token: 0x0600C0DC RID: 49372 RVA: 0x0037DA10 File Offset: 0x0037BC10
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			if (pawn == null)
			{
				return false;
			}
			bool flag;
			if (MetalhorrorUtility.IsInfected(this.parent.pawn))
			{
				Hediff hediff;
				BodyPartRecord bodyPartRecord;
				flag = HealthUtility.TryGetWorstHealthCondition(pawn, out hediff, out bodyPartRecord, new HediffDef[]
				{
					HediffDefOf.MetalhorrorImplant
				});
			}
			else
			{
				Hediff hediff;
				BodyPartRecord bodyPartRecord;
				flag = HealthUtility.TryGetWorstHealthCondition(pawn, out hediff, out bodyPartRecord, Array.Empty<HediffDef>());
			}
			if (!flag && throwMessages)
			{
				Messages.Message(string.Format("{0}: {1}", "CannotUseAbility".Translate(this.parent.def.label), "AbilityCannotCastNoHealableInjury".Translate(pawn.Named("PAWN")).Resolve().StripTags()), pawn, MessageTypeDefOf.RejectInput, false);
			}
			return flag;
		}

		// Token: 0x0600C0DD RID: 49373 RVA: 0x0037DAD4 File Offset: 0x0037BCD4
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Pawn;
			if (pawn == null)
			{
				return;
			}
			foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
			{
				if (hediff.Bleeding)
				{
					hediff.Tended(1f, 1f, 1);
				}
			}
			bool flag = false;
			string text;
			if (MetalhorrorUtility.IsInfected(this.parent.pawn))
			{
				text = HealthUtility.FixWorstHealthCondition(pawn, new HediffDef[]
				{
					HediffDefOf.MetalhorrorImplant
				});
				MetalhorrorUtility.Infect(pawn, this.parent.pawn, "UnnaturalHealingImplant", null);
			}
			else
			{
				text = HealthUtility.FixWorstHealthCondition(pawn, Array.Empty<HediffDef>());
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("LetterUnnaturalHealing".Translate(this.parent.pawn.Named("CASTER"), pawn.Named("PAWN")));
			if (!string.IsNullOrEmpty(text))
			{
				stringBuilder.AppendLine("\n" + text);
			}
			if (pawn.ageTracker.Adult && Rand.Chance(0.25f) && FleshbeastUtility.TryGiveMutation(pawn, HediffDefOf.Tentacle))
			{
				stringBuilder.Append("\n" + "LetterUnnaturalHealingTentacle".Translate(pawn.Named("PAWN")));
				TaleRecorder.RecordTale(TaleDefOf.MutatedMyArm, new object[]
				{
					this.parent.pawn,
					pawn
				});
				flag = true;
			}
			else
			{
				TaleRecorder.RecordTale(TaleDefOf.HealedMe, new object[]
				{
					this.parent.pawn,
					pawn
				});
			}
			TaggedString label = "LetterUnnaturalHealingLabel".Translate();
			LetterDef textLetterDef = flag ? LetterDefOf.NegativeEvent : LetterDefOf.NeutralEvent;
			Find.LetterStack.ReceiveLetter(label, stringBuilder.ToString().TrimEndNewlines(), textLetterDef, pawn, null, null, null, null, 0, true);
		}

		// Token: 0x0600C0DE RID: 49374 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AICanTargetNow(LocalTargetInfo target)
		{
			return false;
		}

		// Token: 0x040083F8 RID: 33784
		private const float TentacleChance = 0.25f;
	}
}
