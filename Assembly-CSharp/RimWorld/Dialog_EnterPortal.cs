using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200316F RID: 12655
	public class Dialog_EnterPortal : Window
	{
		// Token: 0x17002E35 RID: 11829
		// (get) Token: 0x06011CAA RID: 72874 RVA: 0x00526A3B File Offset: 0x00524C3B
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1024f, (float)UI.screenHeight);
			}
		}

		// Token: 0x17002E36 RID: 11830
		// (get) Token: 0x06011CAB RID: 72875 RVA: 0x00006AAC File Offset: 0x00004CAC
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06011CAC RID: 72876 RVA: 0x00526A4D File Offset: 0x00524C4D
		public Dialog_EnterPortal(MapPortal portal) : base(null)
		{
			this.portal = portal;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06011CAD RID: 72877 RVA: 0x00526A80 File Offset: 0x00524C80
		public override void PostOpen()
		{
			base.PostOpen();
			this.CalculateAndRecacheTransferables();
		}

		// Token: 0x06011CAE RID: 72878 RVA: 0x00526A90 File Offset: 0x00524C90
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(0f, 0f, inRect.width, 35f);
			using (new TextBlock(GameFont.Medium, TextAnchor.MiddleCenter))
			{
				Widgets.Label(rect, this.portal.EnterString);
			}
			Dialog_EnterPortal.tabsList.Clear();
			Dialog_EnterPortal.tabsList.Add(new TabRecord("PawnsTab".Translate(), delegate()
			{
				this.tab = Dialog_EnterPortal.Tab.Pawns;
			}, this.tab == Dialog_EnterPortal.Tab.Pawns));
			Dialog_EnterPortal.tabsList.Add(new TabRecord("ItemsTab".Translate(), delegate()
			{
				this.tab = Dialog_EnterPortal.Tab.Items;
			}, this.tab == Dialog_EnterPortal.Tab.Items));
			inRect.yMin += 67f;
			Widgets.DrawMenuSection(inRect);
			TabDrawer.DrawTabs<TabRecord>(inRect, Dialog_EnterPortal.tabsList, 200f);
			inRect = inRect.ContractedBy(17f);
			Widgets.BeginGroup(inRect);
			Rect rect2 = inRect.AtZero();
			this.DoBottomButtons(rect2);
			Rect inRect2 = rect2;
			inRect2.yMax -= 76f;
			bool flag = false;
			Dialog_EnterPortal.Tab tab = this.tab;
			if (tab != Dialog_EnterPortal.Tab.Pawns)
			{
				if (tab == Dialog_EnterPortal.Tab.Items)
				{
					this.itemsTransfer.OnGUI(inRect2, out flag);
				}
			}
			else
			{
				this.pawnsTransfer.OnGUI(inRect2, out flag);
			}
			Widgets.EndGroup();
		}

		// Token: 0x06011CAF RID: 72879 RVA: 0x00526C00 File Offset: 0x00524E00
		private void DoBottomButtons(Rect rect)
		{
			if (Widgets.ButtonText(new Rect(rect.width / 2f - this.BottomButtonSize.x / 2f, rect.height - 55f - 17f, this.BottomButtonSize.x, this.BottomButtonSize.y), "ResetButton".Translate(), true, true, true, null))
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				this.CalculateAndRecacheTransferables();
			}
			if (Widgets.ButtonText(new Rect(0f, rect.height - 55f - 17f, this.BottomButtonSize.x, this.BottomButtonSize.y), "CancelButton".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(rect.width - this.BottomButtonSize.x, rect.height - 55f - 17f, this.BottomButtonSize.x, this.BottomButtonSize.y), "AcceptButton".Translate(), true, true, true, null) && this.TryAccept())
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				this.Close(false);
			}
		}

		// Token: 0x06011CB0 RID: 72880 RVA: 0x00526D68 File Offset: 0x00524F68
		private bool TryAccept()
		{
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			this.portal.leftToLoad = new List<TransferableOneWay>();
			foreach (TransferableOneWay transferableOneWay in this.transferables)
			{
				this.portal.AddToTheToLoadList(transferableOneWay, transferableOneWay.CountToTransfer);
			}
			EnterPortalUtility.MakeLordsAsAppropriate(pawnsFromTransferables, this.portal);
			return true;
		}

		// Token: 0x06011CB1 RID: 72881 RVA: 0x00526DF0 File Offset: 0x00524FF0
		private void CalculateAndRecacheTransferables()
		{
			this.transferables = new List<TransferableOneWay>();
			if (this.portal.LoadInProgress)
			{
				foreach (TransferableOneWay item in this.portal.leftToLoad)
				{
					this.transferables.Add(item);
				}
			}
			this.AddPawnsToTransferables();
			this.AddItemsToTransferables();
			foreach (Thing t in EnterPortalUtility.ThingsBeingHauledTo(this.portal))
			{
				this.AddToTransferables(t);
			}
			this.pawnsTransfer = new TransferableOneWayWidget(null, null, null, "TransferMapPortalColonyThingCountTip".Translate(), true, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, true, () => float.MaxValue, 0f, false, new PlanetTile?(this.portal.Map.Tile), false, true, false, false, false, false, false, false, false, false);
			CaravanUIUtility.AddPawnsSections(this.pawnsTransfer, this.transferables);
			this.itemsTransfer = new TransferableOneWayWidget(from x in this.transferables
			where x.ThingDef.category != ThingCategory.Pawn
			select x, null, null, "TransferMapPortalColonyThingCountTip".Translate(), true, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, true, () => float.MaxValue, 0f, false, new PlanetTile?(this.portal.Map.Tile), false, false, false, false, false, false, false, false, false, false);
		}

		// Token: 0x06011CB2 RID: 72882 RVA: 0x00526FB4 File Offset: 0x005251B4
		private void AddToTransferables(Thing t)
		{
			if (this.portal.LoadInProgress && this.portal.leftToLoad.Any((TransferableOneWay trans) => trans.things.Contains(t)))
			{
				return;
			}
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching<TransferableOneWay>(t, this.transferables, TransferAsOneMode.PodsOrCaravanPacking);
			if (transferableOneWay == null)
			{
				transferableOneWay = new TransferableOneWay();
				this.transferables.Add(transferableOneWay);
			}
			if (transferableOneWay.things.Contains(t))
			{
				string str = "Tried to add the same thing twice to TransferableOneWay: ";
				Thing t2 = t;
				Log.Error(str + ((t2 != null) ? t2.ToString() : null));
				return;
			}
			transferableOneWay.things.Add(t);
		}

		// Token: 0x06011CB3 RID: 72883 RVA: 0x00527068 File Offset: 0x00525268
		private void AddPawnsToTransferables()
		{
			foreach (Pawn t in CaravanFormingUtility.AllSendablePawns(this.portal.Map, true, false, false, false, true, -1))
			{
				this.AddToTransferables(t);
			}
		}

		// Token: 0x06011CB4 RID: 72884 RVA: 0x005270CC File Offset: 0x005252CC
		private void AddItemsToTransferables()
		{
			bool isPocketMap = this.portal.Map.IsPocketMap;
			foreach (Thing t in CaravanFormingUtility.AllReachableColonyItems(this.portal.Map, isPocketMap, isPocketMap, false))
			{
				this.AddToTransferables(t);
			}
		}

		// Token: 0x06011CB5 RID: 72885 RVA: 0x00527140 File Offset: 0x00525340
		public override void OnAcceptKeyPressed()
		{
			if (this.TryAccept())
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				this.Close(false);
			}
		}

		// Token: 0x0400B7D1 RID: 47057
		private const float TitleRectHeight = 35f;

		// Token: 0x0400B7D2 RID: 47058
		private const float BottomAreaHeight = 55f;

		// Token: 0x0400B7D3 RID: 47059
		private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x0400B7D4 RID: 47060
		private MapPortal portal;

		// Token: 0x0400B7D5 RID: 47061
		private List<TransferableOneWay> transferables;

		// Token: 0x0400B7D6 RID: 47062
		private TransferableOneWayWidget pawnsTransfer;

		// Token: 0x0400B7D7 RID: 47063
		private TransferableOneWayWidget itemsTransfer;

		// Token: 0x0400B7D8 RID: 47064
		private Dialog_EnterPortal.Tab tab;

		// Token: 0x0400B7D9 RID: 47065
		private static List<TabRecord> tabsList = new List<TabRecord>();

		// Token: 0x02003170 RID: 12656
		private enum Tab
		{
			// Token: 0x0400B7DB RID: 47067
			Pawns,
			// Token: 0x0400B7DC RID: 47068
			Items
		}
	}
}
