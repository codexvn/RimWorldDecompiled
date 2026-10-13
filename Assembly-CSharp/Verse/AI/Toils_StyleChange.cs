using System;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000F4B RID: 3915
	public static class Toils_StyleChange
	{
		// Token: 0x060060ED RID: 24813 RVA: 0x001EB684 File Offset: 0x001E9884
		public static Toil SetupLookChangeData()
		{
			Toil toil = ToilMaker.MakeToil("SetupLookChangeData");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Color? hairColor = null;
				if (Rand.Value < 0.05f)
				{
					hairColor = new Color?(PawnHairColors.RandomHairColor(actor, actor.story.SkinColor, 20));
				}
				actor.style.SetupNextLookChangeData(Toils_StyleChange.GetRandomStyleItemWeighted<HairDef>(actor, actor.story.hairDef, null), Toils_StyleChange.GetRandomStyleItemWeighted<BeardDef>(actor, actor.style.beardDef, null), Toils_StyleChange.GetRandomStyleItemWeighted<TattooDef>(actor, actor.style.FaceTattoo, new TattooType?(TattooType.Face)), Toils_StyleChange.GetRandomStyleItemWeighted<TattooDef>(actor, actor.style.BodyTattoo, new TattooType?(TattooType.Body)), hairColor);
			};
			return toil;
		}

		// Token: 0x060060EE RID: 24814 RVA: 0x001EB6C4 File Offset: 0x001E98C4
		public static Toil DoLookChange(TargetIndex StationIndex, Pawn pawn)
		{
			return Toils_General.WaitWith(StationIndex, 300, true, false, false, StationIndex, PathEndMode.Touch).FailOnDespawnedOrNull(StationIndex).PlaySustainerOrSound(SoundDefOf.HairCutting, 1f).WithEffect(EffecterDefOf.HairCutting, StationIndex, new Color?(pawn.story.HairColor));
		}

		// Token: 0x060060EF RID: 24815 RVA: 0x001EB714 File Offset: 0x001E9914
		public static Toil FinalizeLookChange()
		{
			Toil toil = ToilMaker.MakeToil("FinalizeLookChange");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				bool flag = false;
				if (actor.style.nextHairDef != null && actor.style.nextHairDef != actor.story.hairDef)
				{
					flag = true;
					actor.story.hairDef = actor.style.nextHairDef;
				}
				if (actor.style.CanWantBeard && actor.style.beardDef != null && actor.style.nextBeardDef != actor.style.beardDef)
				{
					flag = true;
					actor.style.beardDef = actor.style.nextBeardDef;
				}
				if (actor.style.nextFaceTattooDef != null)
				{
					actor.style.FaceTattoo = actor.style.nextFaceTattooDef;
				}
				if (actor.style.nextBodyTatooDef != null)
				{
					actor.style.BodyTattoo = actor.style.nextBodyTatooDef;
				}
				actor.style.Notify_StyleItemChanged();
				actor.style.ResetNextStyleChangeAttemptTick();
				if (flag)
				{
					actor.style.MakeHairFilth();
				}
			};
			return toil;
		}

		// Token: 0x060060F0 RID: 24816 RVA: 0x001EB754 File Offset: 0x001E9954
		private static T GetRandomStyleItemWeighted<T>(Pawn pawn, T current, TattooType? tattooType = null) where T : StyleItemDef
		{
			T result;
			if ((from x in DefDatabase<T>.AllDefs
			where PawnStyleItemChooser.WantsToUseStyle(pawn, x, tattooType)
			select x).TryRandomElementByWeight((T x) => PawnStyleItemChooser.TotalStyleItemLikelihood(x, pawn) * ((x == current) ? 0.05f : 1f), out result))
			{
				return result;
			}
			return default(T);
		}

		// Token: 0x040045E5 RID: 17893
		private const float ChanceToChangeHairColor = 0.05f;
	}
}
