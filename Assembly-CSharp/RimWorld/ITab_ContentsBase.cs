using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200338F RID: 13199
	public abstract class ITab_ContentsBase : ITab
	{
		// Token: 0x1700307B RID: 12411
		// (get) Token: 0x060128D6 RID: 75990
		public abstract IList<Thing> container { get; }

		// Token: 0x1700307C RID: 12412
		// (get) Token: 0x060128D7 RID: 75991 RVA: 0x00579AD7 File Offset: 0x00577CD7
		public override bool IsVisible
		{
			get
			{
				return base.SelThing.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x1700307D RID: 12413
		// (get) Token: 0x060128D8 RID: 75992 RVA: 0x00003D0D File Offset: 0x00001F0D
		public virtual IntVec3 DropOffset
		{
			get
			{
				return IntVec3.Zero;
			}
		}

		// Token: 0x1700307E RID: 12414
		// (get) Token: 0x060128D9 RID: 75993 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool UseDiscardMessage
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060128DA RID: 75994 RVA: 0x00579AEB File Offset: 0x00577CEB
		public ITab_ContentsBase()
		{
			this.size = new Vector2(460f, 450f);
		}

		// Token: 0x060128DB RID: 75995 RVA: 0x00579B1C File Offset: 0x00577D1C
		protected override void FillTab()
		{
			this.thingsToSelect.Clear();
			Rect outRect = new Rect(default(Vector2), this.size).ContractedBy(10f);
			outRect.yMin += 20f;
			Rect rect = new Rect(0f, 0f, outRect.width - 16f, Mathf.Max(this.lastDrawnHeight, outRect.height));
			Text.Font = GameFont.Small;
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect, true);
			float num = 0f;
			this.DoItemsLists(rect, ref num);
			this.lastDrawnHeight = num;
			Widgets.EndScrollView();
			if (this.thingsToSelect.Any<Thing>())
			{
				ITab_Pawn_FormingCaravan.SelectNow(this.thingsToSelect);
				this.thingsToSelect.Clear();
			}
		}

		// Token: 0x060128DC RID: 75996 RVA: 0x00579BE8 File Offset: 0x00577DE8
		protected virtual void DoItemsLists(Rect inRect, ref float curY)
		{
			Widgets.BeginGroup(inRect);
			Widgets.ListSeparator(ref curY, inRect.width, this.containedItemsKey.Translate());
			IList<Thing> container = this.container;
			bool flag = false;
			for (int i = 0; i < container.Count; i++)
			{
				Thing t = container[i];
				if (t != null)
				{
					flag = true;
					ITab_ContentsBase.tmpSingleThing.Clear();
					ITab_ContentsBase.tmpSingleThing.Add(t);
					this.DoThingRow(t.def, t.stackCount, ITab_ContentsBase.tmpSingleThing, inRect.width, ref curY, delegate(int x)
					{
						this.OnDropThing(t, x);
					});
					ITab_ContentsBase.tmpSingleThing.Clear();
				}
			}
			if (!flag)
			{
				Widgets.NoneLabel(ref curY, inRect.width, null);
			}
			Widgets.EndGroup();
		}

		// Token: 0x060128DD RID: 75997 RVA: 0x00579CCC File Offset: 0x00577ECC
		protected virtual void OnDropThing(Thing t, int count)
		{
			Thing thing;
			GenDrop.TryDropSpawn(t.SplitOff(count), base.SelThing.Position + this.DropOffset, base.SelThing.Map, ThingPlaceMode.Near, out thing, null, null, true);
		}

		// Token: 0x060128DE RID: 75998 RVA: 0x00579D10 File Offset: 0x00577F10
		protected void DoThingRow(ThingDef thingDef, int count, List<Thing> things, float width, ref float curY, Action<int> discardAction)
		{
			Rect rect = new Rect(0f, curY, width, 28f);
			if (this.canRemoveThings)
			{
				if (count != 1 && Widgets.ButtonImage(new Rect(rect.x + rect.width - 24f, rect.y + (rect.height - 24f) / 2f, 24f, 24f), CaravanThingsTabUtility.AbandonSpecificCountButtonTex, true, null))
				{
					Find.WindowStack.Add(new Dialog_Slider("RemoveSliderText".Translate(thingDef.label), 1, count, discardAction, int.MinValue, 1f));
				}
				rect.width -= 24f;
				if (Widgets.ButtonImage(new Rect(rect.x + rect.width - 24f, rect.y + (rect.height - 24f) / 2f, 24f, 24f), CaravanThingsTabUtility.AbandonButtonTex, true, null))
				{
					if (this.UseDiscardMessage)
					{
						string value = thingDef.label;
						if (things.Count == 1 && things[0] is Pawn)
						{
							value = ((Pawn)things[0]).LabelShortCap;
						}
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmRemoveItemDialog".Translate(value), delegate()
						{
							discardAction(count);
						}, false, null, WindowLayer.Dialog));
					}
					else
					{
						discardAction(count);
					}
				}
				rect.width -= 24f;
			}
			if (things.Count == 1)
			{
				Widgets.InfoCardButton(rect.width - 24f, curY, things[0]);
			}
			else
			{
				Widgets.InfoCardButton(rect.width - 24f, curY, thingDef);
			}
			rect.width -= 24f;
			if (Mouse.IsOver(rect))
			{
				GUI.color = ITab_ContentsBase.ThingHighlightColor;
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
			if (thingDef.DrawMatSingle != null && thingDef.DrawMatSingle.mainTexture != null)
			{
				Rect rect2 = new Rect(4f, curY, 28f, 28f);
				if (things.Count == 1)
				{
					Widgets.ThingIcon(rect2, things[0], 1f, null, false, 1f, false);
				}
				else
				{
					Widgets.ThingIcon(rect2, thingDef, null, null, 1f, null, null, 1f);
				}
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			GUI.color = ITab_ContentsBase.ThingLabelColor;
			Rect rect3 = new Rect(36f, curY, rect.width - 36f, rect.height);
			string text;
			if (things.Count == 1 && count == things[0].stackCount)
			{
				text = things[0].LabelCap;
			}
			else if (thingDef == ThingDefOf.MinifiedThing)
			{
				text = GenLabel.ThingLabel(things[0].GetInnerIfMinified(), count, true, true).CapitalizeFirst();
			}
			else
			{
				text = GenLabel.ThingLabel(thingDef, null, count).CapitalizeFirst();
			}
			Text.WordWrap = false;
			Widgets.Label(rect3, text.StripTags().Truncate(rect3.width, null));
			Text.WordWrap = true;
			Text.Anchor = TextAnchor.UpperLeft;
			TooltipHandler.TipRegion(rect, text);
			if (Widgets.ButtonInvisible(rect, true))
			{
				this.SelectLater(things);
			}
			if (Mouse.IsOver(rect))
			{
				for (int i = 0; i < things.Count; i++)
				{
					TargetHighlighter.Highlight(things[i], true, true, false);
				}
			}
			curY += 28f;
		}

		// Token: 0x060128DF RID: 75999 RVA: 0x0057A0FE File Offset: 0x005782FE
		private void SelectLater(List<Thing> things)
		{
			this.thingsToSelect.Clear();
			this.thingsToSelect.AddRange(things);
		}

		// Token: 0x0400C0BD RID: 49341
		private Vector2 scrollPosition;

		// Token: 0x0400C0BE RID: 49342
		private float lastDrawnHeight;

		// Token: 0x0400C0BF RID: 49343
		private List<Thing> thingsToSelect = new List<Thing>();

		// Token: 0x0400C0C0 RID: 49344
		public bool canRemoveThings = true;

		// Token: 0x0400C0C1 RID: 49345
		protected static List<Thing> tmpSingleThing = new List<Thing>();

		// Token: 0x0400C0C2 RID: 49346
		protected const float TopPadding = 20f;

		// Token: 0x0400C0C3 RID: 49347
		protected const float SpaceBetweenItemsLists = 10f;

		// Token: 0x0400C0C4 RID: 49348
		protected const float ThingRowHeight = 28f;

		// Token: 0x0400C0C5 RID: 49349
		protected const float ThingIconSize = 28f;

		// Token: 0x0400C0C6 RID: 49350
		protected const float ThingLeftX = 36f;

		// Token: 0x0400C0C7 RID: 49351
		protected static readonly Color ThingLabelColor = ITab_Pawn_Gear.ThingLabelColor;

		// Token: 0x0400C0C8 RID: 49352
		protected static readonly Color ThingHighlightColor = ITab_Pawn_Gear.HighlightColor;

		// Token: 0x0400C0C9 RID: 49353
		public string containedItemsKey;
	}
}
