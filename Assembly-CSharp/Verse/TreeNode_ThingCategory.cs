using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000BC3 RID: 3011
	public class TreeNode_ThingCategory : TreeNode
	{
		// Token: 0x17000BD5 RID: 3029
		// (get) Token: 0x06004D0D RID: 19725 RVA: 0x001885CC File Offset: 0x001867CC
		public string Label
		{
			get
			{
				return this.catDef.label;
			}
		}

		// Token: 0x17000BD6 RID: 3030
		// (get) Token: 0x06004D0E RID: 19726 RVA: 0x001885D9 File Offset: 0x001867D9
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x17000BD7 RID: 3031
		// (get) Token: 0x06004D0F RID: 19727 RVA: 0x001885E6 File Offset: 0x001867E6
		public IEnumerable<TreeNode_ThingCategory> ChildCategoryNodesAndThis
		{
			get
			{
				foreach (ThingCategoryDef thingCategoryDef in this.catDef.ThisAndChildCategoryDefs)
				{
					yield return thingCategoryDef.treeNode;
				}
				IEnumerator<ThingCategoryDef> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000BD8 RID: 3032
		// (get) Token: 0x06004D10 RID: 19728 RVA: 0x001885F6 File Offset: 0x001867F6
		public IEnumerable<TreeNode_ThingCategory> ChildCategoryNodes
		{
			get
			{
				foreach (ThingCategoryDef thingCategoryDef in this.catDef.childCategories)
				{
					yield return thingCategoryDef.treeNode;
				}
				List<ThingCategoryDef>.Enumerator enumerator = default(List<ThingCategoryDef>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x06004D11 RID: 19729 RVA: 0x00188606 File Offset: 0x00186806
		public TreeNode_ThingCategory(ThingCategoryDef def)
		{
			this.catDef = def;
		}

		// Token: 0x06004D12 RID: 19730 RVA: 0x00188615 File Offset: 0x00186815
		public override string ToString()
		{
			return this.catDef.defName;
		}

		// Token: 0x040036C7 RID: 14023
		public ThingCategoryDef catDef;
	}
}
