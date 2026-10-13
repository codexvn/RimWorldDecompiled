using System;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D3B RID: 15675
	public static class CaravanAbandonOrBanishUtility
	{
		// Token: 0x060155BE RID: 87486 RVA: 0x00662CAC File Offset: 0x00660EAC
		public static void TryAbandonOrBanishViaInterface(Thing t, Caravan caravan)
		{
			Pawn p = t as Pawn;
			if (p == null)
			{
				Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation("ConfirmAbandonItemDialog".Translate(t.Label), delegate()
				{
					Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(caravan, t);
					if (ownerOf == null)
					{
						string str = "Could not find owner of ";
						Thing t2 = t;
						Log.Error(str + ((t2 != null) ? t2.ToString() : null));
						return;
					}
					t.Notify_AbandonedAtTile(caravan.GetTileCurrentlyOver());
					ownerOf.inventory.innerContainer.Remove(t);
					t.Destroy(DestroyMode.Vanish);
					caravan.RecacheInventory();
				}, true, null, WindowLayer.Dialog);
				Find.WindowStack.Add(window);
				return;
			}
			if (!caravan.PawnsListForReading.Any((Pawn x) => x != p && caravan.IsOwner(x)))
			{
				Messages.Message("MessageCantBanishLastColonist".Translate(), caravan, MessageTypeDefOf.RejectInput, false);
				return;
			}
			PawnBanishUtility.ShowBanishPawnConfirmationDialog(p, null);
		}

		// Token: 0x060155BF RID: 87487 RVA: 0x00662D74 File Offset: 0x00660F74
		public static void TryAbandonOrBanishViaInterface(TransferableImmutable t, Caravan caravan)
		{
			Pawn pawn = t.AnyThing as Pawn;
			if (pawn != null)
			{
				CaravanAbandonOrBanishUtility.TryAbandonOrBanishViaInterface(pawn, caravan);
				return;
			}
			Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation("ConfirmAbandonItemDialog".Translate(t.LabelWithTotalStackCount), delegate()
			{
				for (int i = 0; i < t.things.Count; i++)
				{
					Thing thing = t.things[i];
					Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(caravan, thing);
					if (ownerOf == null)
					{
						string str = "Could not find owner of ";
						Thing thing2 = thing;
						Log.Error(str + ((thing2 != null) ? thing2.ToString() : null));
						return;
					}
					thing.Notify_AbandonedAtTile(caravan.GetTileCurrentlyOver());
					ownerOf.inventory.innerContainer.Remove(thing);
					thing.Destroy(DestroyMode.Vanish);
				}
				caravan.RecacheInventory();
			}, true, null, WindowLayer.Dialog);
			Find.WindowStack.Add(window);
		}

		// Token: 0x060155C0 RID: 87488 RVA: 0x00662DF0 File Offset: 0x00660FF0
		public static void TryAbandonSpecificCountViaInterface(Thing t, Caravan caravan)
		{
			Find.WindowStack.Add(new Dialog_Slider("AbandonSliderText".Translate(t.LabelNoCount), 1, t.stackCount, delegate(int x)
			{
				Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(caravan, t);
				if (ownerOf == null)
				{
					string str = "Could not find owner of ";
					Thing t2 = t;
					Log.Error(str + ((t2 != null) ? t2.ToString() : null));
					return;
				}
				if (x >= t.stackCount)
				{
					t.Notify_AbandonedAtTile(caravan.GetTileCurrentlyOver());
					ownerOf.inventory.innerContainer.Remove(t);
					t.Destroy(DestroyMode.Vanish);
				}
				else
				{
					Thing thing = t.SplitOff(x);
					thing.Notify_AbandonedAtTile(caravan.GetTileCurrentlyOver());
					thing.Destroy(DestroyMode.Vanish);
				}
				caravan.RecacheInventory();
			}, int.MinValue, 1f));
		}

		// Token: 0x060155C1 RID: 87489 RVA: 0x00662E64 File Offset: 0x00661064
		public static void TryAbandonSpecificCountViaInterface(TransferableImmutable t, Caravan caravan)
		{
			Find.WindowStack.Add(new Dialog_Slider("AbandonSliderText".Translate(t.Label), 1, t.TotalStackCount, delegate(int x)
			{
				int num = x;
				int num2 = 0;
				while (num2 < t.things.Count && num > 0)
				{
					Thing thing = t.things[num2];
					Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(caravan, thing);
					if (ownerOf == null)
					{
						string str = "Could not find owner of ";
						Thing thing2 = thing;
						Log.Error(str + ((thing2 != null) ? thing2.ToString() : null));
						return;
					}
					if (num >= thing.stackCount)
					{
						num -= thing.stackCount;
						thing.Notify_AbandonedAtTile(caravan.GetTileCurrentlyOver());
						ownerOf.inventory.innerContainer.Remove(thing);
						thing.Destroy(DestroyMode.Vanish);
					}
					else
					{
						Thing thing3 = thing.SplitOff(num);
						thing3.Notify_AbandonedAtTile(caravan.GetTileCurrentlyOver());
						thing3.Destroy(DestroyMode.Vanish);
						num = 0;
					}
					num2++;
				}
				caravan.RecacheInventory();
			}, int.MinValue, 1f));
		}

		// Token: 0x060155C2 RID: 87490 RVA: 0x00662ED8 File Offset: 0x006610D8
		public static string GetAbandonOrBanishButtonTooltip(Thing t, bool abandonSpecificCount)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				return PawnBanishUtility.GetBanishButtonTip(pawn);
			}
			return CaravanAbandonOrBanishUtility.GetAbandonItemButtonTooltip(t.stackCount, abandonSpecificCount);
		}

		// Token: 0x060155C3 RID: 87491 RVA: 0x00662F04 File Offset: 0x00661104
		public static string GetAbandonOrBanishButtonTooltip(TransferableImmutable t, bool abandonSpecificCount)
		{
			Pawn pawn = t.AnyThing as Pawn;
			if (pawn != null)
			{
				return PawnBanishUtility.GetBanishButtonTip(pawn);
			}
			return CaravanAbandonOrBanishUtility.GetAbandonItemButtonTooltip(t.TotalStackCount, abandonSpecificCount);
		}

		// Token: 0x060155C4 RID: 87492 RVA: 0x00662F34 File Offset: 0x00661134
		private static string GetAbandonItemButtonTooltip(int currentStackCount, bool abandonSpecificCount)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (currentStackCount == 1)
			{
				stringBuilder.AppendLine("AbandonTip".Translate());
			}
			else if (abandonSpecificCount)
			{
				stringBuilder.AppendLine("AbandonSpecificCountTip".Translate());
			}
			else
			{
				stringBuilder.AppendLine("AbandonAllTip".Translate());
			}
			stringBuilder.AppendLine();
			stringBuilder.Append("AbandonItemTipExtraText".Translate());
			return stringBuilder.ToString();
		}
	}
}
