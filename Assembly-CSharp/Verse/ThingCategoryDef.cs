using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002F5 RID: 757
	public class ThingCategoryDef : Def
	{
		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06001288 RID: 4744 RVA: 0x000564DB File Offset: 0x000546DB
		public List<ThingDef> SortedChildThingDefs
		{
			get
			{
				return this.sortedChildThingDefsCached;
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06001289 RID: 4745 RVA: 0x000564E3 File Offset: 0x000546E3
		public IEnumerable<ThingCategoryDef> Parents
		{
			get
			{
				if (this.parent != null)
				{
					yield return this.parent;
					foreach (ThingCategoryDef thingCategoryDef in this.parent.Parents)
					{
						yield return thingCategoryDef;
					}
					IEnumerator<ThingCategoryDef> enumerator = null;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x0600128A RID: 4746 RVA: 0x000564F3 File Offset: 0x000546F3
		public IEnumerable<ThingCategoryDef> ThisAndChildCategoryDefs
		{
			get
			{
				yield return this;
				foreach (ThingCategoryDef thingCategoryDef in this.childCategories)
				{
					foreach (ThingCategoryDef thingCategoryDef2 in thingCategoryDef.ThisAndChildCategoryDefs)
					{
						yield return thingCategoryDef2;
					}
					IEnumerator<ThingCategoryDef> enumerator2 = null;
				}
				List<ThingCategoryDef>.Enumerator enumerator = default(List<ThingCategoryDef>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x0600128B RID: 4747 RVA: 0x00056503 File Offset: 0x00054703
		public IEnumerable<ThingDef> DescendantThingDefs
		{
			get
			{
				foreach (ThingCategoryDef thingCategoryDef in this.ThisAndChildCategoryDefs)
				{
					foreach (ThingDef thingDef in thingCategoryDef.childThingDefs)
					{
						yield return thingDef;
					}
					List<ThingDef>.Enumerator enumerator2 = default(List<ThingDef>.Enumerator);
				}
				IEnumerator<ThingCategoryDef> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x0600128C RID: 4748 RVA: 0x00056513 File Offset: 0x00054713
		public IEnumerable<SpecialThingFilterDef> DescendantSpecialThingFilterDefs
		{
			get
			{
				foreach (ThingCategoryDef thingCategoryDef in this.ThisAndChildCategoryDefs)
				{
					foreach (SpecialThingFilterDef specialThingFilterDef in thingCategoryDef.childSpecialFilters)
					{
						yield return specialThingFilterDef;
					}
					List<SpecialThingFilterDef>.Enumerator enumerator2 = default(List<SpecialThingFilterDef>.Enumerator);
				}
				IEnumerator<ThingCategoryDef> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x0600128D RID: 4749 RVA: 0x00056523 File Offset: 0x00054723
		public IEnumerable<SpecialThingFilterDef> ParentsSpecialThingFilterDefs
		{
			get
			{
				foreach (ThingCategoryDef thingCategoryDef in this.Parents)
				{
					foreach (SpecialThingFilterDef specialThingFilterDef in thingCategoryDef.childSpecialFilters)
					{
						yield return specialThingFilterDef;
					}
					List<SpecialThingFilterDef>.Enumerator enumerator2 = default(List<SpecialThingFilterDef>.Enumerator);
				}
				IEnumerator<ThingCategoryDef> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x00056533 File Offset: 0x00054733
		public bool ContainedInThisOrDescendant(ThingDef thingDef)
		{
			return this.allChildThingDefsCached.Contains(thingDef);
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x00056544 File Offset: 0x00054744
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			this.allChildThingDefsCached = new HashSet<ThingDef>();
			foreach (ThingCategoryDef thingCategoryDef in this.ThisAndChildCategoryDefs)
			{
				foreach (ThingDef item in thingCategoryDef.childThingDefs)
				{
					this.allChildThingDefsCached.Add(item);
				}
			}
			this.sortedChildThingDefsCached = (from n in this.childThingDefs
			orderby n.label
			select n).ToList<ThingDef>();
		}

		// Token: 0x06001290 RID: 4752 RVA: 0x00056618 File Offset: 0x00054818
		public override void PostLoad()
		{
			this.treeNode = new TreeNode_ThingCategory(this);
			if (!this.iconPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.icon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				});
			}
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x00056644 File Offset: 0x00054844
		public static ThingCategoryDef Named(string defName)
		{
			return DefDatabase<ThingCategoryDef>.GetNamed(defName, true);
		}

		// Token: 0x04001125 RID: 4389
		public ThingCategoryDef parent;

		// Token: 0x04001126 RID: 4390
		[NoTranslate]
		public string iconPath;

		// Token: 0x04001127 RID: 4391
		public bool resourceReadoutRoot;

		// Token: 0x04001128 RID: 4392
		[Unsaved(false)]
		public TreeNode_ThingCategory treeNode;

		// Token: 0x04001129 RID: 4393
		[Unsaved(false)]
		public List<ThingCategoryDef> childCategories = new List<ThingCategoryDef>();

		// Token: 0x0400112A RID: 4394
		[Unsaved(false)]
		public List<ThingDef> childThingDefs = new List<ThingDef>();

		// Token: 0x0400112B RID: 4395
		[Unsaved(false)]
		private HashSet<ThingDef> allChildThingDefsCached;

		// Token: 0x0400112C RID: 4396
		[Unsaved(false)]
		private List<ThingDef> sortedChildThingDefsCached;

		// Token: 0x0400112D RID: 4397
		[Unsaved(false)]
		public List<SpecialThingFilterDef> childSpecialFilters = new List<SpecialThingFilterDef>();

		// Token: 0x0400112E RID: 4398
		[Unsaved(false)]
		public Texture2D icon = BaseContent.BadTex;
	}
}
