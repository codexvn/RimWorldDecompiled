using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200362D RID: 13869
	public static class EquipmentUtility
	{
		// Token: 0x06013424 RID: 78884 RVA: 0x005B69E0 File Offset: 0x005B4BE0
		public static bool CanEquip(Thing thing, Pawn pawn)
		{
			string text;
			return EquipmentUtility.CanEquip(thing, pawn, out text, true);
		}

		// Token: 0x06013425 RID: 78885 RVA: 0x005B69F8 File Offset: 0x005B4BF8
		public static bool CanEquip(Thing thing, Pawn pawn, out string cantReason, bool checkBonded = true)
		{
			cantReason = null;
			CompBladelinkWeapon compBladelinkWeapon = thing.TryGetComp<CompBladelinkWeapon>();
			if (compBladelinkWeapon != null && compBladelinkWeapon.Biocodable && compBladelinkWeapon.CodedPawn != null && compBladelinkWeapon.CodedPawn != pawn)
			{
				cantReason = "BladelinkBondedToSomeoneElse".Translate();
				return false;
			}
			if (CompBiocodable.IsBiocoded(thing) && !CompBiocodable.IsBiocodedFor(thing, pawn))
			{
				cantReason = "BiocodedCodedForSomeoneElse".Translate();
				return false;
			}
			if (checkBonded && EquipmentUtility.AlreadyBondedToWeapon(thing, pawn))
			{
				cantReason = "BladelinkAlreadyBondedMessage".Translate(pawn.Named("PAWN"), pawn.equipment.bondedWeapon.Named("BONDEDWEAPON"));
				return false;
			}
			if (EquipmentUtility.RolePreventsFromUsing(pawn, thing, out cantReason))
			{
				return false;
			}
			if (thing.def.IsApparel && !thing.def.apparel.developmentalStageFilter.Has(pawn.DevelopmentalStage))
			{
				cantReason = "WrongDevelopmentalStageForClothing".Translate(pawn.DevelopmentalStage.ToString().Translate(), Find.ActiveLanguageWorker.WithIndefiniteArticlePostProcessed(thing.def.apparel.developmentalStageFilter.ToCommaListOr(), false, false));
				return false;
			}
			return true;
		}

		// Token: 0x06013426 RID: 78886 RVA: 0x005B6B30 File Offset: 0x005B4D30
		public static bool AlreadyBondedToWeapon(Thing thing, Pawn pawn)
		{
			CompBladelinkWeapon compBladelinkWeapon = thing.TryGetComp<CompBladelinkWeapon>();
			return compBladelinkWeapon != null && compBladelinkWeapon.Biocodable && pawn.equipment.bondedWeapon != null && pawn.equipment.bondedWeapon != thing;
		}

		// Token: 0x06013427 RID: 78887 RVA: 0x005B6B74 File Offset: 0x005B4D74
		public static string GetPersonaWeaponConfirmationText(Thing item, Pawn p)
		{
			CompBladelinkWeapon compBladelinkWeapon = item.TryGetComp<CompBladelinkWeapon>();
			if (compBladelinkWeapon != null && compBladelinkWeapon.Biocodable && compBladelinkWeapon.CodedPawn != p)
			{
				TaggedString taggedString = "BladelinkEquipWarning".Translate();
				List<WeaponTraitDef> traitsListForReading = compBladelinkWeapon.TraitsListForReading;
				if (!traitsListForReading.NullOrEmpty<WeaponTraitDef>())
				{
					taggedString += "\n\n" + "BladelinkEquipWarningTraits".Translate() + ":";
					for (int i = 0; i < traitsListForReading.Count; i++)
					{
						taggedString += "\n\n" + traitsListForReading[i].LabelCap + ": " + traitsListForReading[i].description;
					}
				}
				taggedString += "\n\n" + "RoyalWeaponEquipConfirmation".Translate();
				return taggedString;
			}
			return null;
		}

		// Token: 0x06013428 RID: 78888 RVA: 0x005B6C50 File Offset: 0x005B4E50
		public static bool IsBondedTo(Thing thing, Pawn pawn)
		{
			CompBladelinkWeapon compBladelinkWeapon = thing.TryGetComp<CompBladelinkWeapon>();
			return compBladelinkWeapon != null && compBladelinkWeapon.CodedPawn == pawn;
		}

		// Token: 0x06013429 RID: 78889 RVA: 0x005B6C74 File Offset: 0x005B4E74
		public static bool QuestLodgerCanEquip(Thing thing, Pawn pawn)
		{
			return (pawn.equipment.Primary == null || EquipmentUtility.QuestLodgerCanUnequip(pawn.equipment.Primary, pawn)) && (CompBiocodable.IsBiocodedFor(thing, pawn) || EquipmentUtility.AlreadyBondedToWeapon(thing, pawn) || thing.def.IsWeapon);
		}

		// Token: 0x0601342A RID: 78890 RVA: 0x005B6CC4 File Offset: 0x005B4EC4
		public static bool RolePreventsFromUsing(Pawn pawn, Thing thing, out string reason)
		{
			if (ModsConfig.IdeologyActive && pawn.Ideo != null)
			{
				Precept_Role role = pawn.Ideo.GetRole(pawn);
				if (role != null && !role.CanEquip(pawn, thing, out reason))
				{
					return true;
				}
			}
			reason = null;
			return false;
		}

		// Token: 0x0601342B RID: 78891 RVA: 0x005B6D01 File Offset: 0x005B4F01
		public static bool QuestLodgerCanUnequip(Thing thing, Pawn pawn)
		{
			return !CompBiocodable.IsBiocodedFor(thing, pawn) && !EquipmentUtility.IsBondedTo(thing, pawn);
		}

		// Token: 0x0601342C RID: 78892 RVA: 0x005B6D1C File Offset: 0x005B4F1C
		public static Verb_LaunchProjectile GetRecoilVerb(List<Verb> allWeaponVerbs)
		{
			Verb_LaunchProjectile verb_LaunchProjectile = null;
			foreach (Verb verb in allWeaponVerbs)
			{
				Verb_LaunchProjectile verb_LaunchProjectile2 = verb as Verb_LaunchProjectile;
				if (verb_LaunchProjectile2 != null && (verb_LaunchProjectile == null || verb_LaunchProjectile.LastShotTick < verb_LaunchProjectile2.LastShotTick))
				{
					verb_LaunchProjectile = verb_LaunchProjectile2;
				}
			}
			return verb_LaunchProjectile;
		}

		// Token: 0x0601342D RID: 78893 RVA: 0x005B6D84 File Offset: 0x005B4F84
		public static void Recoil(ThingDef weaponDef, Verb_LaunchProjectile shootVerb, out Vector3 drawOffset, out float angleOffset, float aimAngle)
		{
			drawOffset = Vector3.zero;
			angleOffset = 0f;
			if (weaponDef.recoilPower > 0f && shootVerb != null)
			{
				Rand.PushState(shootVerb.LastShotTick);
				try
				{
					int num = Find.TickManager.TicksGame - shootVerb.LastShotTick;
					if ((float)num < weaponDef.recoilRelaxation)
					{
						float num2 = Mathf.Clamp01((float)num / weaponDef.recoilRelaxation);
						float num3 = Mathf.Lerp(weaponDef.recoilPower, 0f, num2);
						drawOffset = new Vector3((float)Rand.Sign * EquipmentUtility.RecoilCurveAxisX.Evaluate(num2), 0f, -EquipmentUtility.RecoilCurveAxisY.Evaluate(num2)) * num3;
						angleOffset = (float)Rand.Sign * EquipmentUtility.RecoilCurveRotation.Evaluate(num2) * num3;
						aimAngle += angleOffset;
						drawOffset = drawOffset.RotatedBy(aimAngle);
					}
				}
				finally
				{
					Rand.PopState();
				}
			}
		}

		// Token: 0x0601342E RID: 78894 RVA: 0x005B6E80 File Offset: 0x005B5080
		public static bool TryGenerateWeaponByTag(string weaponTag, out Thing weapon, float marketValue = 99999f)
		{
			EquipmentUtility.workingWeapons.Clear();
			weapon = null;
			foreach (ThingStuffPair thingStuffPair in PawnWeaponGenerator.AllWeaponPairs)
			{
				if (thingStuffPair.Price <= marketValue && thingStuffPair.thing.weaponTags != null && thingStuffPair.thing.weaponTags.Contains(weaponTag))
				{
					EquipmentUtility.workingWeapons.Add(thingStuffPair);
				}
			}
			if (EquipmentUtility.workingWeapons.Count == 0)
			{
				return false;
			}
			ThingStuffPair thingStuffPair2;
			if (!EquipmentUtility.workingWeapons.TryRandomElementByWeight((ThingStuffPair w) => w.Commonality * w.Price, out thingStuffPair2))
			{
				return false;
			}
			weapon = (ThingWithComps)ThingMaker.MakeThing(thingStuffPair2.thing, thingStuffPair2.stuff);
			return true;
		}

		// Token: 0x0400D5C0 RID: 54720
		private static readonly SimpleCurve RecoilCurveAxisX = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1f, 0.02f),
				true
			},
			{
				new CurvePoint(2f, 0.03f),
				true
			}
		};

		// Token: 0x0400D5C1 RID: 54721
		private static readonly SimpleCurve RecoilCurveAxisY = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1f, 0.05f),
				true
			},
			{
				new CurvePoint(2f, 0.075f),
				true
			}
		};

		// Token: 0x0400D5C2 RID: 54722
		private static readonly SimpleCurve RecoilCurveRotation = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1f, 3f),
				true
			},
			{
				new CurvePoint(2f, 4f),
				true
			}
		};

		// Token: 0x0400D5C3 RID: 54723
		private static List<ThingStuffPair> workingWeapons = new List<ThingStuffPair>();
	}
}
