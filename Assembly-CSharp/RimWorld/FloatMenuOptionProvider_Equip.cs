using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020032FB RID: 13051
	public class FloatMenuOptionProvider_Equip : FloatMenuOptionProvider
	{
		// Token: 0x17002F99 RID: 12185
		// (get) Token: 0x060125BC RID: 75196 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F9A RID: 12186
		// (get) Token: 0x060125BD RID: 75197 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F9B RID: 12187
		// (get) Token: 0x060125BE RID: 75198 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060125BF RID: 75199 RVA: 0x00569160 File Offset: 0x00567360
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return context.FirstSelectedPawn.equipment != null;
		}

		// Token: 0x060125C0 RID: 75200 RVA: 0x00569170 File Offset: 0x00567370
		protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
		{
			if (!clickedThing.HasComp<CompEquippable>())
			{
				return null;
			}
			string labelShort = clickedThing.LabelShort;
			if (clickedThing.def.IsWeapon && context.FirstSelectedPawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				return new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "IsIncapableOfViolenceLower".Translate(context.FirstSelectedPawn.LabelShort, context.FirstSelectedPawn), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (clickedThing.def.IsRangedWeapon && context.FirstSelectedPawn.WorkTagIsDisabled(WorkTags.Shooting))
			{
				return new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "IsIncapableOfShootingLower".Translate(context.FirstSelectedPawn), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!context.FirstSelectedPawn.CanReach(clickedThing, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!context.FirstSelectedPawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "Incapable".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (clickedThing.IsBurning())
			{
				return new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "BurningLower".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (context.FirstSelectedPawn.IsQuestLodger() && !EquipmentUtility.QuestLodgerCanEquip(clickedThing, context.FirstSelectedPawn))
			{
				return new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "QuestRelated".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			string str;
			if (!EquipmentUtility.CanEquip(clickedThing, context.FirstSelectedPawn, out str, false))
			{
				return new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + str.CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			string text = "Equip".Translate(labelShort);
			if (clickedThing.def.IsRangedWeapon && context.FirstSelectedPawn.story != null && context.FirstSelectedPawn.story.traits.HasTrait(TraitDefOf.Brawler))
			{
				text += " " + "EquipWarningBrawler".Translate();
			}
			if (EquipmentUtility.AlreadyBondedToWeapon(clickedThing, context.FirstSelectedPawn))
			{
				text += " " + "BladelinkAlreadyBonded".Translate();
				TaggedString dialogText = "BladelinkAlreadyBondedDialog".Translate(context.FirstSelectedPawn.Named("PAWN"), clickedThing.Named("WEAPON"), context.FirstSelectedPawn.equipment.bondedWeapon.Named("BONDEDWEAPON"));
				return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, delegate()
				{
					Find.WindowStack.Add(new Dialog_MessageBox(dialogText, null, null, null, null, null, false, null, null, WindowLayer.Dialog));
				}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedThing, "ReservedBy", null);
			}
			Action <>9__3;
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, delegate()
			{
				string personaWeaponConfirmationText = EquipmentUtility.GetPersonaWeaponConfirmationText(clickedThing, context.FirstSelectedPawn);
				if (!personaWeaponConfirmationText.NullOrEmpty())
				{
					WindowStack windowStack = Find.WindowStack;
					TaggedString text2 = personaWeaponConfirmationText;
					string buttonAText = "Yes".Translate();
					Action buttonAAction;
					if ((buttonAAction = <>9__3) == null)
					{
						buttonAAction = (<>9__3 = delegate()
						{
							base.<GetSingleOptionFor>g__Equip|2();
						});
					}
					windowStack.Add(new Dialog_MessageBox(text2, buttonAText, buttonAAction, "No".Translate(), null, null, false, null, null, WindowLayer.Dialog));
					return;
				}
				base.<GetSingleOptionFor>g__Equip|2();
			}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedThing, "ReservedBy", null);
		}
	}
}
