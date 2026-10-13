using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000239 RID: 569
	public class DamageGraphicData
	{
		// Token: 0x06000EBD RID: 3773 RVA: 0x00044676 File Offset: 0x00042876
		public void ResolveReferencesSpecial()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (this.scratches != null)
				{
					this.scratchMats = new List<Material>();
					for (int i = 0; i < this.scratches.Count; i++)
					{
						Material material = MaterialPool.MatFrom(this.scratches[i], ShaderDatabase.Transparent, 2905);
						this.scratchMats.Add(material);
						GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)material.mainTexture, null);
					}
				}
				if (this.cornerTL != null)
				{
					this.cornerTLMat = MaterialPool.MatFrom(this.cornerTL, ShaderDatabase.Transparent, 2905);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.cornerTLMat.mainTexture, null);
				}
				if (this.cornerTR != null)
				{
					this.cornerTRMat = MaterialPool.MatFrom(this.cornerTR, ShaderDatabase.Transparent, 2905);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.cornerTRMat.mainTexture, null);
				}
				if (this.cornerBL != null)
				{
					this.cornerBLMat = MaterialPool.MatFrom(this.cornerBL, ShaderDatabase.Transparent, 2905);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.cornerBLMat.mainTexture, null);
				}
				if (this.cornerBR != null)
				{
					this.cornerBRMat = MaterialPool.MatFrom(this.cornerBR, ShaderDatabase.Transparent, 2905);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.cornerBRMat.mainTexture, null);
				}
				if (this.edgeTop != null)
				{
					this.edgeTopMat = MaterialPool.MatFrom(this.edgeTop, ShaderDatabase.Transparent, 2905);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.edgeTopMat.mainTexture, null);
				}
				if (this.edgeBot != null)
				{
					this.edgeBotMat = MaterialPool.MatFrom(this.edgeBot, ShaderDatabase.Transparent, 2905);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.edgeBotMat.mainTexture, null);
				}
				if (this.edgeLeft != null)
				{
					this.edgeLeftMat = MaterialPool.MatFrom(this.edgeLeft, ShaderDatabase.Transparent, 2905);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.edgeLeftMat.mainTexture, null);
				}
				if (this.edgeRight != null)
				{
					this.edgeRightMat = MaterialPool.MatFrom(this.edgeRight, ShaderDatabase.Transparent, 2905);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.edgeRightMat.mainTexture, null);
				}
			});
		}

		// Token: 0x04000987 RID: 2439
		public bool enabled = true;

		// Token: 0x04000988 RID: 2440
		public Rect rectN;

		// Token: 0x04000989 RID: 2441
		public Rect rectE;

		// Token: 0x0400098A RID: 2442
		public Rect rectS;

		// Token: 0x0400098B RID: 2443
		public Rect rectW;

		// Token: 0x0400098C RID: 2444
		public Rect rect;

		// Token: 0x0400098D RID: 2445
		[NoTranslate]
		public List<string> scratches;

		// Token: 0x0400098E RID: 2446
		[NoTranslate]
		public string cornerTL;

		// Token: 0x0400098F RID: 2447
		[NoTranslate]
		public string cornerTR;

		// Token: 0x04000990 RID: 2448
		[NoTranslate]
		public string cornerBL;

		// Token: 0x04000991 RID: 2449
		[NoTranslate]
		public string cornerBR;

		// Token: 0x04000992 RID: 2450
		[NoTranslate]
		public string edgeLeft;

		// Token: 0x04000993 RID: 2451
		[NoTranslate]
		public string edgeRight;

		// Token: 0x04000994 RID: 2452
		[NoTranslate]
		public string edgeTop;

		// Token: 0x04000995 RID: 2453
		[NoTranslate]
		public string edgeBot;

		// Token: 0x04000996 RID: 2454
		[Unsaved(false)]
		public List<Material> scratchMats;

		// Token: 0x04000997 RID: 2455
		[Unsaved(false)]
		public Material cornerTLMat;

		// Token: 0x04000998 RID: 2456
		[Unsaved(false)]
		public Material cornerTRMat;

		// Token: 0x04000999 RID: 2457
		[Unsaved(false)]
		public Material cornerBLMat;

		// Token: 0x0400099A RID: 2458
		[Unsaved(false)]
		public Material cornerBRMat;

		// Token: 0x0400099B RID: 2459
		[Unsaved(false)]
		public Material edgeLeftMat;

		// Token: 0x0400099C RID: 2460
		[Unsaved(false)]
		public Material edgeRightMat;

		// Token: 0x0400099D RID: 2461
		[Unsaved(false)]
		public Material edgeTopMat;

		// Token: 0x0400099E RID: 2462
		[Unsaved(false)]
		public Material edgeBotMat;
	}
}
