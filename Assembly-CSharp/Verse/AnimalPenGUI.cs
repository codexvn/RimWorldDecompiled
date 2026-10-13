using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003B0 RID: 944
	public static class AnimalPenGUI
	{
		// Token: 0x0600183B RID: 6203 RVA: 0x0007AF00 File Offset: 0x00079100
		public static void DoAllowedAreaMessage(Rect rect, Pawn pawn)
		{
			Text.Anchor = TextAnchor.MiddleCenter;
			Text.Font = GameFont.Tiny;
			CompAnimalPenMarker currentPenOf = AnimalPenUtility.GetCurrentPenOf(pawn, false);
			TaggedString taggedString;
			TaggedString str;
			if (currentPenOf != null)
			{
				taggedString = "InPen".Translate() + ": " + currentPenOf.label;
				str = taggedString;
			}
			else
			{
				GUI.color = Color.gray;
				taggedString = "(" + "Unpenned".Translate() + ")";
				str = "UnpennedTooltip".Translate();
			}
			Widgets.Label(rect, taggedString);
			TooltipHandler.TipRegion(rect, str);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
		}

		// Token: 0x0600183C RID: 6204 RVA: 0x0007AFA8 File Offset: 0x000791A8
		public static void DrawPlacingMouseAttachments(IntVec3 mouseCell, Map map, PenFoodCalculator calc)
		{
			AnimalPenGUI.<>c__DisplayClass3_0 CS$<>8__locals1;
			CS$<>8__locals1.calc = calc;
			CS$<>8__locals1.sb = new StringBuilder();
			Vector2 location = Find.WorldGrid.LongLatOf(map.Tile);
			CS$<>8__locals1.summerQuadrum = CS$<>8__locals1.calc.GetSummerOrBestQuadrum();
			CS$<>8__locals1.summerLabel = CS$<>8__locals1.summerQuadrum.GetSeason(location);
			CS$<>8__locals1.sb.AppendLine(CS$<>8__locals1.calc.PenSizeDescription());
			CS$<>8__locals1.sb.AppendLine("PenExampleAnimals".Translate() + ":");
			AnimalPenGUI.<DrawPlacingMouseAttachments>g__AppendCapacityOf|3_0(ThingDefOf.Cow, ref CS$<>8__locals1);
			AnimalPenGUI.<DrawPlacingMouseAttachments>g__AppendCapacityOf|3_0(ThingDefOf.Goat, ref CS$<>8__locals1);
			AnimalPenGUI.<DrawPlacingMouseAttachments>g__AppendCapacityOf|3_0(ThingDefOf.Chicken, ref CS$<>8__locals1);
			Widgets.MouseAttachedLabel(CS$<>8__locals1.sb.ToString().TrimEnd(), 8f, 35f, null);
		}

		// Token: 0x0600183D RID: 6205 RVA: 0x0007B088 File Offset: 0x00079288
		[CompilerGenerated]
		internal static void <DrawPlacingMouseAttachments>g__AppendCapacityOf|3_0(ThingDef animalDef, ref AnimalPenGUI.<>c__DisplayClass3_0 A_1)
		{
			A_1.sb.Append("PenCapacityDesc".Translate(animalDef.Named("ANIMALDEF")).CapitalizeFirst());
			A_1.sb.Append(" (").Append(A_1.summerLabel.Label()).Append("): ");
			if (A_1.calc.Unenclosed)
			{
				A_1.sb.Append("?");
			}
			else
			{
				A_1.sb.Append(A_1.calc.CapacityOf(A_1.summerQuadrum, animalDef).ToString("F1"));
			}
			A_1.sb.AppendLine();
		}

		// Token: 0x020003B1 RID: 945
		public class PenPainter : AnimalPenEnclosureCalculator
		{
			// Token: 0x0600183E RID: 6206 RVA: 0x0007B145 File Offset: 0x00079345
			protected override void VisitDirectlyConnectedRegion(Region r)
			{
				this.directEdgeCells.AddRange(r.Cells);
			}

			// Token: 0x0600183F RID: 6207 RVA: 0x0007B158 File Offset: 0x00079358
			protected override void VisitIndirectlyDirectlyConnectedRegion(Region r)
			{
				this.indirectEdgeCells.AddRange(r.Cells);
			}

			// Token: 0x06001840 RID: 6208 RVA: 0x0007B16B File Offset: 0x0007936B
			protected override void VisitPassableDoorway(Region r)
			{
				this.openDoorEdgeCells.AddRange(r.Cells);
			}

			// Token: 0x06001841 RID: 6209 RVA: 0x0007B180 File Offset: 0x00079380
			public void Paint(IntVec3 position, Map map)
			{
				this.directEdgeCells.Clear();
				this.indirectEdgeCells.Clear();
				this.openDoorEdgeCells.Clear();
				if (base.VisitPen(position, map))
				{
					GenDraw.DrawFieldEdges(this.directEdgeCells, Color.green, null, null, 2900);
					GenDraw.DrawFieldEdges(this.indirectEdgeCells, Color.white, null, null, 2900);
					GenDraw.DrawFieldEdges(this.openDoorEdgeCells, Color.white, null, null, 2900);
					return;
				}
				GenDraw.DrawFieldEdges(this.openDoorEdgeCells, Color.red, null, null, 2900);
			}

			// Token: 0x04001599 RID: 5529
			private readonly List<IntVec3> directEdgeCells = new List<IntVec3>();

			// Token: 0x0400159A RID: 5530
			private readonly List<IntVec3> indirectEdgeCells = new List<IntVec3>();

			// Token: 0x0400159B RID: 5531
			private readonly List<IntVec3> openDoorEdgeCells = new List<IntVec3>();
		}

		// Token: 0x020003B2 RID: 946
		public class PenBlueprintPainter
		{
			// Token: 0x06001843 RID: 6211 RVA: 0x0007B260 File Offset: 0x00079460
			public void Paint(IntVec3 position, Map map)
			{
				this.filler.VisitPen(position, map);
				if (this.filler.isEnclosed)
				{
					GenDraw.DrawFieldEdges(this.filler.cellsFound, Color.white, null, null, 2900);
				}
			}

			// Token: 0x0400159C RID: 5532
			private AnimalPenBlueprintEnclosureCalculator filler = new AnimalPenBlueprintEnclosureCalculator();
		}
	}
}
