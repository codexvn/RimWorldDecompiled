using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A88 RID: 10888
	public class Dialog_MechBillConfig : Dialog_BillConfig
	{
		// Token: 0x0600F264 RID: 62052 RVA: 0x00465F7B File Offset: 0x0046417B
		public Dialog_MechBillConfig(Bill_Mech bill, IntVec3 billGiverPos) : base(bill, billGiverPos)
		{
			ModLister.CheckBiotech("BillWithFormingCycleDialog");
		}

		// Token: 0x0600F265 RID: 62053 RVA: 0x00465F90 File Offset: 0x00464190
		protected override void DoIngredientConfigPane(float x, ref float y, float width, float height)
		{
			float y2 = y;
			base.DoIngredientConfigPane(x, ref y2, width, height - Dialog_MechBillConfig.formingInfoHeight);
			Building_MechGestator building_MechGestator = this.bill.billStack.billGiver as Building_MechGestator;
			if (building_MechGestator != null && building_MechGestator.ActiveBill == this.bill)
			{
				Rect rect = new Rect(x, y2, width, 9999f);
				Listing_Standard listing_Standard = new Listing_Standard();
				listing_Standard.Begin(rect);
				StringBuilder stringBuilder = new StringBuilder();
				listing_Standard.Label("FormerIngredients".Translate() + ":", -1f, null);
				building_MechGestator.ActiveBill.AppendCurrentIngredientCount(stringBuilder);
				listing_Standard.Label(stringBuilder.ToString(), -1f, null);
				Bill_Mech bill_Mech = (Bill_Mech)this.bill;
				listing_Standard.Label("GestationCyclesCompleted".Translate() + ": " + bill_Mech.GestationCyclesCompleted.ToString() + " / " + bill_Mech.recipe.gestationCycles.ToString(), -1f, null);
				listing_Standard.Gap(12f);
				listing_Standard.End();
				Dialog_MechBillConfig.formingInfoHeight = listing_Standard.CurHeight;
			}
		}

		// Token: 0x04009FAC RID: 40876
		private static float formingInfoHeight;
	}
}
