using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003479 RID: 13433
	public class PawnColumnWorker_Sterilize : PawnColumnWorker_Checkbox
	{
		// Token: 0x06012E32 RID: 77362 RVA: 0x0059812B File Offset: 0x0059632B
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (this.AnimalSterile(pawn))
			{
				this.DrawSterile(rect, pawn);
				return;
			}
			base.DoCell(rect, pawn, table);
		}

		// Token: 0x06012E33 RID: 77363 RVA: 0x00598148 File Offset: 0x00596348
		private void DrawSterile(Rect rect, Pawn animal)
		{
			int num = (int)((rect.width - 24f) / 2f);
			int num2 = Mathf.Max(3, 0);
			Vector2 vector = new Vector2(rect.x + (float)num, rect.y + (float)num2);
			Rect rect2 = new Rect(vector.x, vector.y, 24f, 24f);
			GUI.DrawTexture(rect2, PawnColumnWorker_Sterilize.AnimalSterilized.Texture);
			if (Mouse.IsOver(rect2))
			{
				string tip = this.GetTip(animal);
				if (!tip.NullOrEmpty())
				{
					TooltipHandler.TipRegion(rect2, tip);
				}
			}
		}

		// Token: 0x06012E34 RID: 77364 RVA: 0x005981E4 File Offset: 0x005963E4
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06012E35 RID: 77365 RVA: 0x00598207 File Offset: 0x00596407
		private int GetValueToCompare(Pawn pawn)
		{
			if (this.AnimalSterile(pawn))
			{
				return -1;
			}
			if (this.SterilizeOperations(pawn).Any<Bill>())
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06012E36 RID: 77366 RVA: 0x00598225 File Offset: 0x00596425
		protected override string GetTip(Pawn pawn)
		{
			if (this.AnimalSterile(pawn))
			{
				return "AnimalAlreadySterile".Translate();
			}
			return "SterilizeAnimal".Translate();
		}

		// Token: 0x06012E37 RID: 77367 RVA: 0x0059824F File Offset: 0x0059644F
		protected override bool GetValue(Pawn pawn)
		{
			return this.SterilizeOperations(pawn).Any<Bill>();
		}

		// Token: 0x06012E38 RID: 77368 RVA: 0x00598260 File Offset: 0x00596460
		protected override void SetValue(Pawn pawn, bool value, PawnTable table)
		{
			if (this.AnimalSterile(pawn))
			{
				return;
			}
			bool flag = this.SterilizeOperations(pawn).Any<Bill>();
			if (value == flag)
			{
				return;
			}
			if (table.SortingBy == this.def)
			{
				table.SetDirty();
			}
			if (!value)
			{
				this.CancelSterilizeOperations(pawn);
				return;
			}
			if (pawn.HomeFaction != Faction.OfPlayer && pawn.HomeFaction != null)
			{
				TaggedString taggedString = "AnimalSterilizeConfirm".Translate(pawn.Named("PAWN"), pawn.HomeFaction.Named("FACTION"));
				Find.WindowStack.Add(new Dialog_Confirm(taggedString, delegate()
				{
					this.AddSterilizeOperation(pawn);
				}));
				return;
			}
			this.AddSterilizeOperation(pawn);
		}

		// Token: 0x06012E39 RID: 77369 RVA: 0x00598348 File Offset: 0x00596548
		private bool AnimalSterile(Pawn animal)
		{
			return animal.health.hediffSet.HasHediff(HediffDefOf.Sterilized, false);
		}

		// Token: 0x06012E3A RID: 77370 RVA: 0x00598360 File Offset: 0x00596560
		private List<Bill> SterilizeOperations(Pawn animal)
		{
			return (from b in animal.BillStack.Bills
			where b.recipe == RecipeDefOf.Sterilize
			select b).ToList<Bill>();
		}

		// Token: 0x06012E3B RID: 77371 RVA: 0x00598396 File Offset: 0x00596596
		private void AddSterilizeOperation(Pawn animal)
		{
			HealthCardUtility.CreateSurgeryBill(animal, RecipeDefOf.Sterilize, null, null, true);
		}

		// Token: 0x06012E3C RID: 77372 RVA: 0x005983A8 File Offset: 0x005965A8
		private void CancelSterilizeOperations(Pawn animal)
		{
			foreach (Bill bill in this.SterilizeOperations(animal))
			{
				animal.BillStack.Delete(bill);
			}
		}

		// Token: 0x0400C3CE RID: 50126
		public static readonly CachedTexture AnimalSterilized = new CachedTexture("UI/Icons/Animal/Sterile");
	}
}
