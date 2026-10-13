using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000135 RID: 309
	[StaticConstructorOnStartup]
	public static class BaseContent
	{
		// Token: 0x0600098C RID: 2444 RVA: 0x0002D5C3 File Offset: 0x0002B7C3
		public static bool NullOrBad(this Material mat)
		{
			return mat == null || mat == BaseContent.BadMat;
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x0002D5DB File Offset: 0x0002B7DB
		public static bool NullOrBad(this Texture2D tex)
		{
			return tex == null || tex == BaseContent.BadTex;
		}

		// Token: 0x040004C0 RID: 1216
		public static readonly string BadTexPath = "UI/Misc/BadTexture";

		// Token: 0x040004C1 RID: 1217
		public static readonly string PlaceholderImagePath = "PlaceholderImage";

		// Token: 0x040004C2 RID: 1218
		public static readonly string PlaceholderGearImagePath = "PlaceholderImage_Gear";

		// Token: 0x040004C3 RID: 1219
		public static readonly Material BadMat = MaterialPool.MatFrom(BaseContent.BadTexPath, ShaderDatabase.Cutout);

		// Token: 0x040004C4 RID: 1220
		public static readonly Texture2D BadTex = ContentFinder<Texture2D>.Get(BaseContent.BadTexPath, true);

		// Token: 0x040004C5 RID: 1221
		public static readonly Graphic BadGraphic = GraphicDatabase.Get<Graphic_Single>(BaseContent.BadTexPath);

		// Token: 0x040004C6 RID: 1222
		public static readonly Texture2D PlaceholderImage = ContentFinder<Texture2D>.Get(BaseContent.PlaceholderImagePath, true);

		// Token: 0x040004C7 RID: 1223
		public static readonly Texture2D BlackTex = SolidColorMaterials.NewSolidColorTexture(Color.black);

		// Token: 0x040004C8 RID: 1224
		public static readonly Texture2D GreyTex = SolidColorMaterials.NewSolidColorTexture(Color.grey);

		// Token: 0x040004C9 RID: 1225
		public static readonly Texture2D WhiteTex = SolidColorMaterials.NewSolidColorTexture(Color.white);

		// Token: 0x040004CA RID: 1226
		public static readonly Texture2D ClearTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);

		// Token: 0x040004CB RID: 1227
		public static readonly Texture2D YellowTex = SolidColorMaterials.NewSolidColorTexture(Color.yellow);

		// Token: 0x040004CC RID: 1228
		public static readonly Material BlackMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.black, false);

		// Token: 0x040004CD RID: 1229
		public static readonly Material WhiteMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.white, false);

		// Token: 0x040004CE RID: 1230
		public static readonly Material ClearMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.clear, false);
	}
}
