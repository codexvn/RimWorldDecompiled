using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002278 RID: 8824
	public class CompAbilityEffect_PsychicSlaughter : CompAbilityEffect
	{
		// Token: 0x17001D63 RID: 7523
		// (get) Token: 0x0600C04D RID: 49229 RVA: 0x0037B354 File Offset: 0x00379554
		public new CompProperties_PsychicSlaughter Props
		{
			get
			{
				return (CompProperties_PsychicSlaughter)this.props;
			}
		}

		// Token: 0x0600C04E RID: 49230 RVA: 0x0037B364 File Offset: 0x00379564
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			Pawn pawn = target.Pawn;
			Map map = pawn.MapHeld;
			IntVec3 positionHeld = pawn.PositionHeld;
			base.Apply(target, dest);
			int num = Mathf.Max(GenMath.RoundRandom(pawn.GetStatValue(StatDefOf.MeatAmount, true, -1)), 3);
			pawn.Kill(new DamageInfo?(new DamageInfo(DamageDefOf.Psychic, 99999f, 0f, -1f, this.parent.pawn, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false)), null);
			Corpse corpse = pawn.Corpse;
			if (corpse != null)
			{
				corpse.Destroy(DestroyMode.Vanish);
			}
			EffecterDefOf.MeatExplosion.Spawn(positionHeld, map, 1f).Cleanup();
			FleshbeastUtility.MeatSplatter(CompAbilityEffect_PsychicSlaughter.BloodFilth.RandomInRange, positionHeld, map, FleshbeastUtility.MeatExplosionSize.Large);
			int num2 = CompAbilityEffect_PsychicSlaughter.MeatPieces.RandomInRange;
			int num3 = Mathf.CeilToInt((float)num / (float)ThingDefOf.Meat_Twisted.stackLimit);
			if (num2 < num3)
			{
				num2 = num3;
			}
			Predicate<IntVec3> <>9__0;
			for (int i = 0; i < num2; i++)
			{
				IntVec3 near = positionHeld;
				Predicate<IntVec3> validator;
				if ((validator = <>9__0) == null)
				{
					validator = (<>9__0 = ((IntVec3 x) => x.Walkable(map)));
				}
				IntVec3 loc;
				if (RCellFinder.TryFindRandomCellNearWith(near, validator, map, out loc, 1, 4))
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.Meat_Twisted, null);
					thing.stackCount = num / num2;
					GenSpawn.Spawn(thing, loc, map, WipeMode.Vanish);
				}
			}
		}

		// Token: 0x0600C04F RID: 49231 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AICanTargetNow(LocalTargetInfo target)
		{
			return false;
		}

		// Token: 0x0600C050 RID: 49232 RVA: 0x0037369F File Offset: 0x0037189F
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Valid(target, false);
		}

		// Token: 0x0600C051 RID: 49233 RVA: 0x0037B4CC File Offset: 0x003796CC
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			if (pawn == null)
			{
				return false;
			}
			if (!pawn.RaceProps.IsFlesh)
			{
				if (throwMessages)
				{
					Messages.Message("MessageSlaughterNoFlesh".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.RejectInput, true);
				}
				return false;
			}
			if ((double)pawn.BodySize > 2.5)
			{
				if (throwMessages)
				{
					Messages.Message("MessageSlaughterTooBig".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.RejectInput, true);
				}
				return false;
			}
			return true;
		}

		// Token: 0x040083C2 RID: 33730
		private static readonly IntRange MeatPieces = new IntRange(3, 4);

		// Token: 0x040083C3 RID: 33731
		private static readonly IntRange BloodFilth = new IntRange(3, 4);
	}
}
