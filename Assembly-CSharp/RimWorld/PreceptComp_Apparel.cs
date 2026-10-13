using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002633 RID: 9779
	public class PreceptComp_Apparel : PreceptComp
	{
		// Token: 0x0600D652 RID: 54866 RVA: 0x003F98EC File Offset: 0x003F7AEC
		public Gender AffectedGender(Ideo ideo)
		{
			switch (this.gender)
			{
			case IdeoApparelGender.Any:
				return Gender.None;
			case IdeoApparelGender.SupremeGender:
				return ideo.SupremeGender;
			case IdeoApparelGender.SubordinateGender:
				return ideo.SupremeGender.Opposite();
			default:
				Log.Error("Unimplemented gender: " + this.gender.ToString());
				return Gender.None;
			}
		}

		// Token: 0x0600D653 RID: 54867 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanApplyToApparel(ThingDef apparelDef)
		{
			return true;
		}

		// Token: 0x0600D654 RID: 54868 RVA: 0x003F994C File Offset: 0x003F7B4C
		protected bool AppliesToPawn(Pawn pawn, Precept precept)
		{
			if (ModsConfig.IdeologyActive && pawn.Ideo != null)
			{
				Precept_Apparel precept_Apparel = precept as Precept_Apparel;
				if (precept_Apparel != null)
				{
					Gender targetGender = precept_Apparel.TargetGender;
					if (targetGender != Gender.None && pawn.gender != targetGender)
					{
						return false;
					}
					if (pawn.royalty != null)
					{
						foreach (RoyalTitle royalTitle in pawn.royalty.AllTitlesForReading)
						{
							if (!royalTitle.def.requiredApparel.NullOrEmpty<ApparelRequirement>())
							{
								using (List<ApparelRequirement>.Enumerator enumerator2 = royalTitle.def.requiredApparel.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										string text;
										if (ApparelUtility.IsRequirementActive(enumerator2.Current, ApparelRequirementSource.Title, pawn, out text))
										{
											return false;
										}
									}
								}
							}
						}
					}
					using (List<Apparel>.Enumerator enumerator3 = pawn.apparel.WornApparel.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							if (enumerator3.Current.def == precept_Apparel.apparelDef)
							{
								return false;
							}
						}
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600D655 RID: 54869 RVA: 0x003F9A90 File Offset: 0x003F7C90
		protected void GiveApparelToPawn(Pawn pawn, Precept_Apparel precept)
		{
			Apparel apparel = PawnApparelGenerator.GenerateApparelOfDefFor(pawn, precept.apparelDef);
			if (apparel != null && apparel.PawnCanWear(pawn, false))
			{
				PawnApparelGenerator.PostProcessApparel(apparel, pawn);
				PawnGenerator.PostProcessGeneratedGear(apparel, pawn);
				if (!pawn.kindDef.ignoreIdeoApparelColors)
				{
					apparel.SetColor(pawn.Ideo.ApparelColor, false);
				}
				pawn.apparel.Wear(apparel, false, false);
			}
		}

		// Token: 0x040091B4 RID: 37300
		private IdeoApparelGender gender;
	}
}
