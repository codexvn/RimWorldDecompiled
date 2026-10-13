using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020033D1 RID: 13265
	public class ITab_Storage : ITab
	{
		// Token: 0x170030B9 RID: 12473
		// (get) Token: 0x06012A2A RID: 76330 RVA: 0x00583B08 File Offset: 0x00581D08
		protected virtual IStoreSettingsParent SelStoreSettingsParent
		{
			get
			{
				Thing thing = base.SelObject as Thing;
				if (thing != null)
				{
					IStoreSettingsParent thingOrThingCompStoreSettingsParent = this.GetThingOrThingCompStoreSettingsParent(thing);
					if (thingOrThingCompStoreSettingsParent != null)
					{
						return thingOrThingCompStoreSettingsParent;
					}
					return null;
				}
				else
				{
					if (base.AllSelObjects.Count > 1)
					{
						bool flag = true;
						IStorageGroupMember storageGroupMember = base.AllSelObjects.First<object>() as IStorageGroupMember;
						StorageGroup storageGroup = (storageGroupMember != null) ? storageGroupMember.Group : null;
						if (storageGroup != null)
						{
							foreach (object obj in base.AllSelObjects)
							{
								IStorageGroupMember storageGroupMember2 = obj as IStorageGroupMember;
								if (storageGroupMember2 == null)
								{
									flag = false;
									break;
								}
								if (storageGroupMember2.Group != storageGroup)
								{
									flag = false;
									break;
								}
							}
							if (flag)
							{
								IStorageGroupMember storageGroupMember3 = base.AllSelObjects.First<object>() as IStorageGroupMember;
								if (storageGroupMember3 == null)
								{
									return null;
								}
								return storageGroupMember3.Group;
							}
						}
						return null;
					}
					return base.SelObject as IStoreSettingsParent;
				}
			}
		}

		// Token: 0x170030BA RID: 12474
		// (get) Token: 0x06012A2B RID: 76331 RVA: 0x00583BF0 File Offset: 0x00581DF0
		public override bool IsVisible
		{
			get
			{
				if (base.SelObject == null)
				{
					if (base.AllSelObjects.Count > 1)
					{
						using (List<object>.Enumerator enumerator = base.AllSelObjects.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								object obj = enumerator.Current;
								Thing thing = obj as Thing;
								if (thing != null && thing.Faction != null && thing.Faction != Faction.OfPlayer)
								{
									return false;
								}
							}
							goto IL_8F;
						}
						return false;
					}
					return false;
				}
				Thing thing2 = base.SelObject as Thing;
				if (thing2 != null && thing2.Faction != null && thing2.Faction != Faction.OfPlayer)
				{
					return false;
				}
				IL_8F:
				IStoreSettingsParent selStoreSettingsParent = this.SelStoreSettingsParent;
				return selStoreSettingsParent != null && selStoreSettingsParent.StorageTabVisible;
			}
		}

		// Token: 0x170030BB RID: 12475
		// (get) Token: 0x06012A2C RID: 76332 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool IsPrioritySettingVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170030BC RID: 12476
		// (get) Token: 0x06012A2D RID: 76333 RVA: 0x00583CB4 File Offset: 0x00581EB4
		private float TopAreaHeight
		{
			get
			{
				return (float)(this.IsPrioritySettingVisible ? 35 : 20);
			}
		}

		// Token: 0x06012A2E RID: 76334 RVA: 0x00583CC5 File Offset: 0x00581EC5
		public ITab_Storage()
		{
			this.size = ITab_Storage.WinSize;
			this.labelKey = "TabStorage";
			this.tutorTag = "Storage";
		}

		// Token: 0x06012A2F RID: 76335 RVA: 0x00583CF9 File Offset: 0x00581EF9
		public override void OnOpen()
		{
			base.OnOpen();
			this.thingFilterState.quickSearch.Reset();
		}

		// Token: 0x06012A30 RID: 76336 RVA: 0x00583D14 File Offset: 0x00581F14
		protected override void FillTab()
		{
			IStoreSettingsParent storeSettingsParent = this.SelStoreSettingsParent;
			StorageSettings settings = storeSettingsParent.GetStoreSettings();
			Rect rect = new Rect(0f, 0f, ITab_Storage.WinSize.x, ITab_Storage.WinSize.y).ContractedBy(10f);
			Widgets.BeginGroup(rect);
			if (this.IsPrioritySettingVisible)
			{
				Text.Font = GameFont.Small;
				Rect rect2 = new Rect(0f, 0f, 160f, this.TopAreaHeight - 6f);
				if (Widgets.ButtonText(rect2, "Priority".Translate() + ": " + settings.Priority.Label().CapitalizeFirst(), true, true, true, null))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (object obj in Enum.GetValues(typeof(StoragePriority)))
					{
						StoragePriority storagePriority = (StoragePriority)obj;
						if (storagePriority != StoragePriority.Unstored)
						{
							StoragePriority localPr = storagePriority;
							list.Add(new FloatMenuOption(localPr.Label().CapitalizeFirst(), delegate()
							{
								settings.Priority = localPr;
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
				UIHighlighter.HighlightOpportunity(rect2, "StoragePriority");
			}
			ThingFilter parentFilter = null;
			if (storeSettingsParent.GetParentStoreSettings() != null)
			{
				parentFilter = storeSettingsParent.GetParentStoreSettings().filter;
			}
			Rect rect3 = new Rect(0f, this.TopAreaHeight, rect.width, rect.height - this.TopAreaHeight);
			Bill[] first = (from b in BillUtility.GlobalBills()
			where b is Bill_Production && b.GetSlotGroup() == storeSettingsParent && b.recipe.WorkerCounter.CanPossiblyStore((Bill_Production)b, b.GetSlotGroup())
			select b).ToArray<Bill>();
			ThingFilterUI.DoThingFilterConfigWindow(rect3, this.thingFilterState, settings.filter, parentFilter, 8, null, this.HiddenSpecialThingFilters(), false, false, false, null, null);
			Bill[] second = (from b in BillUtility.GlobalBills()
			where b is Bill_Production && b.GetSlotGroup() == storeSettingsParent && b.recipe.WorkerCounter.CanPossiblyStore((Bill_Production)b, b.GetSlotGroup())
			select b).ToArray<Bill>();
			foreach (Bill bill in first.Except(second))
			{
				Messages.Message("MessageBillValidationStoreZoneInsufficient".Translate(bill.LabelCap, bill.billStack.billGiver.LabelShort.CapitalizeFirst(), SlotGroup.GetGroupLabel(bill.GetSlotGroup())), bill.billStack.billGiver as Thing, MessageTypeDefOf.RejectInput, false);
			}
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.StorageTab, KnowledgeAmount.FrameDisplayed);
			Widgets.EndGroup();
		}

		// Token: 0x06012A31 RID: 76337 RVA: 0x0058401C File Offset: 0x0058221C
		protected IStoreSettingsParent GetThingOrThingCompStoreSettingsParent(Thing t)
		{
			IStoreSettingsParent storeSettingsParent = t as IStoreSettingsParent;
			if (storeSettingsParent != null)
			{
				return storeSettingsParent;
			}
			ThingWithComps thingWithComps = t as ThingWithComps;
			if (thingWithComps != null)
			{
				List<ThingComp> allComps = thingWithComps.AllComps;
				for (int i = 0; i < allComps.Count; i++)
				{
					storeSettingsParent = (allComps[i] as IStoreSettingsParent);
					if (storeSettingsParent != null)
					{
						return storeSettingsParent;
					}
				}
			}
			return null;
		}

		// Token: 0x06012A32 RID: 76338 RVA: 0x0058406A File Offset: 0x0058226A
		public override void Notify_ClickOutsideWindow()
		{
			base.Notify_ClickOutsideWindow();
			this.thingFilterState.quickSearch.Unfocus();
		}

		// Token: 0x06012A33 RID: 76339 RVA: 0x00584082 File Offset: 0x00582282
		private IEnumerable<SpecialThingFilterDef> HiddenSpecialThingFilters()
		{
			if (ModsConfig.IdeologyActive)
			{
				yield return SpecialThingFilterDefOf.AllowVegetarian;
				yield return SpecialThingFilterDefOf.AllowCarnivore;
				yield return SpecialThingFilterDefOf.AllowCannibal;
				yield return SpecialThingFilterDefOf.AllowInsectMeat;
			}
			yield break;
		}

		// Token: 0x0400C18D RID: 49549
		private ThingFilterUI.UIState thingFilterState = new ThingFilterUI.UIState();

		// Token: 0x0400C18E RID: 49550
		private static readonly Vector2 WinSize = new Vector2(300f, 480f);
	}
}
