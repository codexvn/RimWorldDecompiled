using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B13 RID: 6931
	public class TimeAssignmentDef : Def
	{
		// Token: 0x170017CD RID: 6093
		// (get) Token: 0x06009AD2 RID: 39634 RVA: 0x002CF817 File Offset: 0x002CDA17
		public Texture2D ColorTexture
		{
			get
			{
				if (this.colorTextureInt == null)
				{
					this.colorTextureInt = SolidColorMaterials.NewSolidColorTexture(this.color);
				}
				return this.colorTextureInt;
			}
		}

		// Token: 0x06009AD3 RID: 39635 RVA: 0x002CF83E File Offset: 0x002CDA3E
		public override void PostLoad()
		{
			base.PostLoad();
			this.cachedHighlightNotSelectedTag = "TimeAssignmentButton-" + this.defName + "-NotSelected";
		}

		// Token: 0x04006AEF RID: 27375
		public Color color;

		// Token: 0x04006AF0 RID: 27376
		public bool allowRest = true;

		// Token: 0x04006AF1 RID: 27377
		public bool allowJoy = true;

		// Token: 0x04006AF2 RID: 27378
		[Unsaved(false)]
		public string cachedHighlightNotSelectedTag;

		// Token: 0x04006AF3 RID: 27379
		private Texture2D colorTextureInt;
	}
}
