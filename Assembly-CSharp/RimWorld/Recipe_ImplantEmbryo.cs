using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002300 RID: 8960
	public class Recipe_ImplantEmbryo : Recipe_Surgery
	{
		// Token: 0x0600C2BA RID: 49850 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			return false;
		}

		// Token: 0x0600C2BB RID: 49851 RVA: 0x003878C8 File Offset: 0x00385AC8
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			HumanEmbryo humanEmbryo = (HumanEmbryo)ingredients.FirstOrDefault((Thing t) => t.def == ThingDefOf.HumanEmbryo);
			if (humanEmbryo == null)
			{
				Log.Warning(string.Format("Tried to perform implant embryo surgery on {0} but no embryo was in the ingredients list", pawn));
				return;
			}
			if (base.CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
			{
				return;
			}
			if (Rand.Chance(PregnancyUtility.PregnancyChanceImplantEmbryo(pawn)))
			{
				Hediff_Pregnant hediff_Pregnant = (Hediff_Pregnant)HediffMaker.MakeHediff(HediffDefOf.PregnantHuman, pawn, null);
				hediff_Pregnant.SetParents(humanEmbryo.Mother, humanEmbryo.Father, humanEmbryo.GeneSet);
				pawn.health.AddHediff(hediff_Pregnant, null, null, null);
				return;
			}
			Messages.Message("ImplantFailedMessage".Translate(humanEmbryo.Label, pawn), pawn, MessageTypeDefOf.NegativeHealthEvent, true);
		}

		// Token: 0x0600C2BC RID: 49852 RVA: 0x003879B0 File Offset: 0x00385BB0
		public override string LabelFromUniqueIngredients(Bill bill)
		{
			Bill_Medical bill_Medical = bill as Bill_Medical;
			if (bill_Medical != null && !bill_Medical.uniqueRequiredIngredients.NullOrEmpty<Thing>())
			{
				foreach (Thing thing in bill_Medical.uniqueRequiredIngredients)
				{
					HumanEmbryo humanEmbryo = thing as HumanEmbryo;
					if (humanEmbryo != null && !humanEmbryo.DestroyedOrNull())
					{
						return "ImplantSurgeryLabel".Translate(humanEmbryo.Label);
					}
				}
			}
			return null;
		}

		// Token: 0x0600C2BD RID: 49853 RVA: 0x00387A44 File Offset: 0x00385C44
		public override bool CompletableEver(Pawn surgeryTarget)
		{
			return base.CompletableEver(surgeryTarget) && !surgeryTarget.Sterile();
		}
	}
}
