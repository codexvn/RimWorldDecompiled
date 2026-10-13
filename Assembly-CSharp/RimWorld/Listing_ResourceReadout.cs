using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003492 RID: 13458
	[StaticConstructorOnStartup]
	public class Listing_ResourceReadout : Listing_Tree
	{
		// Token: 0x17003160 RID: 12640
		// (get) Token: 0x06012EDB RID: 77531 RVA: 0x0059AC5F File Offset: 0x00598E5F
		protected override float LabelWidth
		{
			get
			{
				return base.ColumnWidth;
			}
		}

		// Token: 0x06012EDC RID: 77532 RVA: 0x0059AC67 File Offset: 0x00598E67
		public Listing_ResourceReadout(Map map)
		{
			this.map = map;
		}

		// Token: 0x06012EDD RID: 77533 RVA: 0x0059AC78 File Offset: 0x00598E78
		public void DoCategory(TreeNode_ThingCategory node, int nestLevel, int openMask)
		{
			int countIn = this.map.resourceCounter.GetCountIn(node.catDef);
			if (countIn == 0)
			{
				return;
			}
			base.OpenCloseWidget(node, nestLevel, openMask);
			Rect rect = new Rect(0f, this.curY, this.LabelWidth, this.lineHeight)
			{
				xMin = base.XAtIndentLevel(nestLevel) + 18f
			};
			Rect position = rect;
			position.width = 80f;
			position.yMax -= 3f;
			position.yMin += 3f;
			GUI.DrawTexture(position, Listing_ResourceReadout.SolidCategoryBG);
			if (Mouse.IsOver(rect))
			{
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, new TipSignal(node.catDef.LabelCap, node.catDef.GetHashCode()));
			}
			Rect position2 = new Rect(rect);
			position2.width = (position2.height = 28f);
			position2.y = rect.y + rect.height / 2f - position2.height / 2f;
			GUI.DrawTexture(position2, node.catDef.icon);
			Widgets.Label(new Rect(rect)
			{
				xMin = position2.xMax + 6f
			}, countIn.ToStringCached());
			base.EndLine();
			if (this.IsOpen(node, openMask))
			{
				this.DoCategoryChildren(node, nestLevel + 1, openMask);
			}
		}

		// Token: 0x06012EDE RID: 77534 RVA: 0x0059ADFC File Offset: 0x00598FFC
		public void DoCategoryChildren(TreeNode_ThingCategory node, int indentLevel, int openMask)
		{
			foreach (TreeNode_ThingCategory treeNode_ThingCategory in node.ChildCategoryNodes)
			{
				if (!treeNode_ThingCategory.catDef.resourceReadoutRoot)
				{
					this.DoCategory(treeNode_ThingCategory, indentLevel, openMask);
				}
			}
			foreach (ThingDef thingDef in node.catDef.childThingDefs)
			{
				if (thingDef.PlayerAcquirable)
				{
					this.DoThingDef(thingDef, indentLevel + 1);
				}
			}
		}

		// Token: 0x06012EDF RID: 77535 RVA: 0x0059AEAC File Offset: 0x005990AC
		private void DoThingDef(ThingDef thingDef, int nestLevel)
		{
			int count = this.map.resourceCounter.GetCount(thingDef);
			if (count == 0)
			{
				return;
			}
			Rect rect = new Rect(0f, this.curY, this.LabelWidth, this.lineHeight)
			{
				xMin = base.XAtIndentLevel(nestLevel) + 18f
			};
			if (Mouse.IsOver(rect))
			{
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, new TipSignal(() => thingDef.LabelCap + ": " + thingDef.description.CapitalizeFirst(), (int)thingDef.shortHash));
			}
			Rect rect2 = new Rect(rect);
			rect2.width = (rect2.height = 28f);
			rect2.y = rect.y + rect.height / 2f - rect2.height / 2f;
			Widgets.ThingIcon(rect2, thingDef, null, null, 1f, null, null, 1f);
			Widgets.Label(new Rect(rect)
			{
				xMin = rect2.xMax + 6f
			}, count.ToStringCached());
			base.EndLine();
		}

		// Token: 0x0400C40F RID: 50191
		private Map map;

		// Token: 0x0400C410 RID: 50192
		private static Texture2D SolidCategoryBG = SolidColorMaterials.NewSolidColorTexture(new Color(0.1f, 0.1f, 0.1f, 0.6f));
	}
}
