using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020031B5 RID: 12725
	public class Dialog_SelectXenogerm : Window
	{
		// Token: 0x17002E84 RID: 11908
		// (get) Token: 0x06011E78 RID: 73336 RVA: 0x005181D8 File Offset: 0x005163D8
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 600f);
			}
		}

		// Token: 0x06011E79 RID: 73337 RVA: 0x00534ADC File Offset: 0x00532CDC
		public Dialog_SelectXenogerm(Pawn pawn, Map map, Xenogerm initialSelected, Action<Xenogerm> onSelect) : base(null)
		{
			this.pawn = pawn;
			this.onSelect = onSelect;
			foreach (Thing thing in map.listerThings.ThingsOfDef(ThingDefOf.Xenogerm))
			{
				if (!thing.PositionHeld.Fogged(map))
				{
					this.xenogerms.Add((Xenogerm)thing);
				}
			}
			if (initialSelected != null && this.xenogerms.Contains(initialSelected))
			{
				this.selected = initialSelected;
			}
			this.closeOnAccept = false;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06011E7A RID: 73338 RVA: 0x00534BA4 File Offset: 0x00532DA4
		public override void PostOpen()
		{
			if (!ModLister.CheckBiotech("xenogerm"))
			{
				this.Close(false);
				return;
			}
			base.PostOpen();
		}

		// Token: 0x06011E7B RID: 73339 RVA: 0x00534BC0 File Offset: 0x00532DC0
		public override void DoWindowContents(Rect rect)
		{
			Rect rect2 = rect;
			rect2.yMax -= Dialog_SelectXenogerm.ButSize.y + 4f;
			Text.Font = GameFont.Medium;
			Widgets.Label(rect2, "SelectXenogerm".Translate());
			Text.Font = GameFont.Small;
			rect2.yMin += 39f;
			this.DisplayXenogerms(rect2);
			Rect rect3 = rect;
			rect3.yMin = rect3.yMax - Dialog_SelectXenogerm.ButSize.y;
			if (this.selected != null)
			{
				if (Widgets.ButtonText(new Rect(rect3.xMax - Dialog_SelectXenogerm.ButSize.x, rect3.y, Dialog_SelectXenogerm.ButSize.x, Dialog_SelectXenogerm.ButSize.y), "Accept".Translate(), true, true, true, null))
				{
					this.Accept();
				}
				if (Widgets.ButtonText(new Rect(rect3.x, rect3.y, Dialog_SelectXenogerm.ButSize.x, Dialog_SelectXenogerm.ButSize.y), "Close".Translate(), true, true, true, null))
				{
					this.Close(true);
					return;
				}
			}
			else if (Widgets.ButtonText(new Rect((rect3.width - Dialog_SelectXenogerm.ButSize.x) / 2f, rect3.y, Dialog_SelectXenogerm.ButSize.x, Dialog_SelectXenogerm.ButSize.y), "Close".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
		}

		// Token: 0x06011E7C RID: 73340 RVA: 0x00534D58 File Offset: 0x00532F58
		private void DisplayXenogerms(Rect rect)
		{
			Widgets.DrawMenuSection(rect);
			rect = rect.ContractedBy(4f);
			GUI.BeginGroup(rect);
			Rect viewRect = new Rect(0f, 0f, rect.width - 16f, this.scrollViewHeight);
			float num = 0f;
			Widgets.BeginScrollView(rect.AtZero(), ref this.scrollPosition, viewRect, true);
			for (int i = 0; i < this.xenogerms.Count; i++)
			{
				float num2 = rect.width;
				if (this.scrollViewHeight > rect.height)
				{
					num2 -= 16f;
				}
				this.DrawXenogerm(new Rect(0f, num, num2, 32f), i);
				num += 32f;
			}
			if (Event.current.type == EventType.Layout)
			{
				this.scrollViewHeight = num;
			}
			Widgets.EndScrollView();
			GUI.EndGroup();
		}

		// Token: 0x06011E7D RID: 73341 RVA: 0x00534E30 File Offset: 0x00533030
		private void DrawXenogerm(Rect rect, int index)
		{
			Xenogerm xenogerm = this.xenogerms[index];
			if (index % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect);
			}
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			if (this.selected == xenogerm)
			{
				Widgets.DrawHighlightSelected(rect);
			}
			Widgets.InfoCardButton(rect.xMax - 24f, rect.y + 4f, xenogerm);
			rect.xMax -= 36f;
			for (int i = Mathf.Min(xenogerm.GeneSet.GenesListForReading.Count, 10) - 1; i >= 0; i--)
			{
				GeneDef geneDef = xenogerm.GeneSet.GenesListForReading[i];
				Rect rect2 = new Rect(rect.xMax - 11f, rect.yMax - rect.height / 2f - 11f, 22f, 22f);
				Widgets.DefIcon(rect2, geneDef, null, 1.25f, null, false, null, null, null, 1f);
				Rect rect3 = rect2;
				rect3.yMin = rect.yMin;
				rect3.yMax = rect.yMax;
				if (Mouse.IsOver(rect3))
				{
					Widgets.DrawHighlight(rect3);
					TooltipHandler.TipRegion(rect3, geneDef.LabelCap + "\n\n" + geneDef.DescriptionFull);
				}
				rect.xMax -= 22f;
			}
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, () => xenogerm.LabelCap + "\n\n" + "Genes".Translate().CapitalizeFirst() + ":\n" + (from x in xenogerm.GeneSet.GenesListForReading
				select x.LabelCap.ToString()).ToLineList("  - ", false), 128921381);
			}
			rect.xMin += 4f;
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect, xenogerm.LabelCap.Truncate(rect.width, this.truncateCache));
			Text.Anchor = TextAnchor.UpperLeft;
			if (Widgets.ButtonInvisible(rect, true))
			{
				this.selected = xenogerm;
			}
		}

		// Token: 0x06011E7E RID: 73342 RVA: 0x0053503C File Offset: 0x0053323C
		private void Accept()
		{
			if (this.pawn != null)
			{
				int num = GeneUtility.MetabolismAfterImplanting(this.pawn, this.selected.GeneSet);
				if (num < GeneTuning.BiostatRange.TrueMin)
				{
					Messages.Message("OrderImplantationIntoPawn".Translate(this.pawn.Named("PAWN")).Resolve().UncapitalizeFirst() + ": " + "ResultingMetTooLow".Translate() + " (" + num.ToString() + ")", this.pawn, MessageTypeDefOf.RejectInput, false);
					return;
				}
				if (this.selected.PawnIdeoDisallowsImplanting(this.pawn))
				{
					Messages.Message("CannotGenericWorkCustom".Translate("OrderImplantationIntoPawn".Translate(this.pawn.Named("PAWN")).Resolve().UncapitalizeFirst() + ": " + "IdeoligionForbids".Translate()), this.pawn, MessageTypeDefOf.RejectInput, false);
					return;
				}
			}
			if (this.onSelect != null)
			{
				this.onSelect(this.selected);
			}
			this.Close(true);
		}

		// Token: 0x0400B939 RID: 47417
		private Pawn pawn;

		// Token: 0x0400B93A RID: 47418
		private List<Xenogerm> xenogerms = new List<Xenogerm>();

		// Token: 0x0400B93B RID: 47419
		private Xenogerm selected;

		// Token: 0x0400B93C RID: 47420
		private Vector2 scrollPosition;

		// Token: 0x0400B93D RID: 47421
		private float scrollViewHeight;

		// Token: 0x0400B93E RID: 47422
		private Action<Xenogerm> onSelect;

		// Token: 0x0400B93F RID: 47423
		private const float HeaderHeight = 35f;

		// Token: 0x0400B940 RID: 47424
		public const float MiniGeneIconSize = 22f;

		// Token: 0x0400B941 RID: 47425
		private const float XenogermElementHeight = 32f;

		// Token: 0x0400B942 RID: 47426
		private const int MaxDisplayedGenes = 10;

		// Token: 0x0400B943 RID: 47427
		private static readonly Vector2 ButSize = new Vector2(150f, 38f);

		// Token: 0x0400B944 RID: 47428
		private Dictionary<string, string> truncateCache = new Dictionary<string, string>();
	}
}
