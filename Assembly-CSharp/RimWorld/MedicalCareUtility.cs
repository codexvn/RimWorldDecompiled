using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020024F5 RID: 9461
	[StaticConstructorOnStartup]
	public static class MedicalCareUtility
	{
		// Token: 0x0600CEAD RID: 52909 RVA: 0x003CAFBF File Offset: 0x003C91BF
		public static void Reset()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				MedicalCareUtility.careTextures = new Texture2D[5];
				MedicalCareUtility.careTextures[0] = ContentFinder<Texture2D>.Get("UI/Icons/Medical/NoCare", true);
				MedicalCareUtility.careTextures[1] = ContentFinder<Texture2D>.Get("UI/Icons/Medical/NoMeds", true);
				MedicalCareUtility.careTextures[2] = ThingDefOf.MedicineHerbal.uiIcon;
				MedicalCareUtility.careTextures[3] = ThingDefOf.MedicineIndustrial.uiIcon;
				MedicalCareUtility.careTextures[4] = ThingDefOf.MedicineUltratech.uiIcon;
			});
		}

		// Token: 0x0600CEAE RID: 52910 RVA: 0x003CAFE8 File Offset: 0x003C91E8
		public static void MedicalCareSetter(Rect rect, ref MedicalCareCategory medCare)
		{
			Rect rect2 = new Rect(rect.x, rect.y, rect.width / 5f, rect.height);
			for (int i = 0; i < 5; i++)
			{
				MedicalCareCategory mc = (MedicalCareCategory)i;
				Widgets.DrawHighlightIfMouseover(rect2);
				MouseoverSounds.DoRegion(rect2);
				GUI.DrawTexture(rect2, MedicalCareUtility.careTextures[i]);
				Widgets.DraggableResult draggableResult = Widgets.ButtonInvisibleDraggable(rect2, false);
				if (draggableResult == Widgets.DraggableResult.Dragged)
				{
					MedicalCareUtility.medicalCarePainting = true;
				}
				if ((MedicalCareUtility.medicalCarePainting && Mouse.IsOver(rect2) && medCare != mc) || draggableResult.AnyPressed())
				{
					medCare = mc;
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				if (medCare == mc)
				{
					Widgets.DrawBox(rect2, 2, null);
				}
				if (Mouse.IsOver(rect2))
				{
					TooltipHandler.TipRegion(rect2, () => mc.GetLabel().CapitalizeFirst(), 632165 + i * 17);
				}
				rect2.x += rect2.width;
			}
			if (!Input.GetMouseButton(0))
			{
				MedicalCareUtility.medicalCarePainting = false;
			}
		}

		// Token: 0x0600CEAF RID: 52911 RVA: 0x003CB0F6 File Offset: 0x003C92F6
		public static string GetLabel(this MedicalCareCategory cat)
		{
			return ("MedicalCareCategory_" + cat.ToString()).Translate();
		}

		// Token: 0x0600CEB0 RID: 52912 RVA: 0x003CB11C File Offset: 0x003C931C
		public static bool AllowsMedicine(this MedicalCareCategory cat, ThingDef meds)
		{
			switch (cat)
			{
			case MedicalCareCategory.NoCare:
				return false;
			case MedicalCareCategory.NoMeds:
				return false;
			case MedicalCareCategory.HerbalOrWorse:
				return meds.GetStatValueAbstract(StatDefOf.MedicalPotency, null) <= ThingDefOf.MedicineHerbal.GetStatValueAbstract(StatDefOf.MedicalPotency, null);
			case MedicalCareCategory.NormalOrWorse:
				return meds.GetStatValueAbstract(StatDefOf.MedicalPotency, null) <= ThingDefOf.MedicineIndustrial.GetStatValueAbstract(StatDefOf.MedicalPotency, null);
			case MedicalCareCategory.Best:
				return true;
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x0600CEB1 RID: 52913 RVA: 0x003CB194 File Offset: 0x003C9394
		public static void MedicalCareSelectButton(Rect rect, Pawn pawn)
		{
			Widgets.Dropdown<Pawn, MedicalCareCategory>(rect, pawn, new Func<Pawn, MedicalCareCategory>(MedicalCareUtility.MedicalCareSelectButton_GetMedicalCare), new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<MedicalCareCategory>>>(MedicalCareUtility.MedicalCareSelectButton_GenerateMenu), null, MedicalCareUtility.careTextures[(int)pawn.playerSettings.medCare], null, null, null, true);
		}

		// Token: 0x0600CEB2 RID: 52914 RVA: 0x003CB1D6 File Offset: 0x003C93D6
		private static MedicalCareCategory MedicalCareSelectButton_GetMedicalCare(Pawn pawn)
		{
			return pawn.playerSettings.medCare;
		}

		// Token: 0x0600CEB3 RID: 52915 RVA: 0x003CB1E3 File Offset: 0x003C93E3
		private static IEnumerable<Widgets.DropdownMenuElement<MedicalCareCategory>> MedicalCareSelectButton_GenerateMenu(Pawn p)
		{
			Widgets.DropdownMenuElement<MedicalCareCategory> dropdownMenuElement;
			int num;
			for (int i = 0; i < 5; i = num + 1)
			{
				MedicalCareCategory mc = (MedicalCareCategory)i;
				dropdownMenuElement = new Widgets.DropdownMenuElement<MedicalCareCategory>
				{
					option = new FloatMenuOption(mc.GetLabel().CapitalizeFirst(), delegate()
					{
						p.playerSettings.medCare = mc;
					}, MedicalCareUtility.MedicalCareIcon(mc), Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false),
					payload = mc
				};
				yield return dropdownMenuElement;
				num = i;
			}
			dropdownMenuElement = default(Widgets.DropdownMenuElement<MedicalCareCategory>);
			dropdownMenuElement.option = new FloatMenuOption("ChangeDefaults".Translate(), delegate()
			{
				Find.WindowStack.Add(new Dialog_MedicalDefaults());
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			yield return dropdownMenuElement;
			yield break;
		}

		// Token: 0x0600CEB4 RID: 52916 RVA: 0x003CB1F4 File Offset: 0x003C93F4
		private static Texture2D MedicalCareIcon(MedicalCareCategory category)
		{
			switch (category)
			{
			case MedicalCareCategory.NoCare:
				return MedicalCareUtility.careTextures[0];
			case MedicalCareCategory.NoMeds:
				return MedicalCareUtility.careTextures[1];
			case MedicalCareCategory.HerbalOrWorse:
				return MedicalCareUtility.careTextures[2];
			case MedicalCareCategory.NormalOrWorse:
				return MedicalCareUtility.careTextures[3];
			case MedicalCareCategory.Best:
				return MedicalCareUtility.careTextures[4];
			default:
				return null;
			}
		}

		// Token: 0x04008CA7 RID: 36007
		private static Texture2D[] careTextures;

		// Token: 0x04008CA8 RID: 36008
		public const float CareSetterHeight = 28f;

		// Token: 0x04008CA9 RID: 36009
		public const float CareSetterWidth = 140f;

		// Token: 0x04008CAA RID: 36010
		private static bool medicalCarePainting;
	}
}
