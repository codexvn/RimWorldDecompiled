using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A07 RID: 6663
	public class BodyTypeDef : Def
	{
		// Token: 0x170016EB RID: 5867
		// (get) Token: 0x06009653 RID: 38483 RVA: 0x002C0159 File Offset: 0x002BE359
		public virtual Texture2D Icon
		{
			get
			{
				return ContentFinder<Texture2D>.Get(this.bodyNakedGraphicPath + "_south", true);
			}
		}

		// Token: 0x04006291 RID: 25233
		[NoTranslate]
		public string bodyNakedGraphicPath;

		// Token: 0x04006292 RID: 25234
		[NoTranslate]
		public string bodyDessicatedGraphicPath;

		// Token: 0x04006293 RID: 25235
		public List<BodyTypeDef.WoundAnchor> woundAnchors;

		// Token: 0x04006294 RID: 25236
		public float woundScale = 1f;

		// Token: 0x04006295 RID: 25237
		public Vector2 headOffset;

		// Token: 0x04006296 RID: 25238
		public Vector2 bodyGraphicScale = Vector2.one;

		// Token: 0x04006297 RID: 25239
		public float bedOffset;

		// Token: 0x04006298 RID: 25240
		public List<AttachPoint> attachPoints;

		// Token: 0x04006299 RID: 25241
		public List<AttachPoint> attachPointsDessicated;

		// Token: 0x02001A08 RID: 6664
		public class WoundAnchor
		{
			// Token: 0x0400629A RID: 25242
			[NoTranslate]
			public string tag;

			// Token: 0x0400629B RID: 25243
			public BodyPartGroupDef group;

			// Token: 0x0400629C RID: 25244
			public bool? narrowCrown;

			// Token: 0x0400629D RID: 25245
			public Rot4? rotation;

			// Token: 0x0400629E RID: 25246
			public bool canMirror = true;

			// Token: 0x0400629F RID: 25247
			public Vector3 offset;

			// Token: 0x040062A0 RID: 25248
			public PawnOverlayDrawer.OverlayLayer layer;

			// Token: 0x040062A1 RID: 25249
			public Color debugColor;

			// Token: 0x040062A2 RID: 25250
			public float range;
		}
	}
}
