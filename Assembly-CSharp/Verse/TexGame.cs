using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001BD RID: 445
	[StaticConstructorOnStartup]
	public static class TexGame
	{
		// Token: 0x06000C53 RID: 3155 RVA: 0x0003A5E0 File Offset: 0x000387E0
		static TexGame()
		{
			Shader.SetGlobalTexture("_NoiseTex", TexGame.NoiseTex);
			Shader.SetGlobalTexture("_RippleTex", TexGame.RippleTex);
			Shader.SetGlobalTexture("_SpecularNoiseTex", TexGame.SpecularNoiseTex);
		}

		// Token: 0x0400070C RID: 1804
		public static readonly Texture2D AlphaAddTex = ContentFinder<Texture2D>.Get("Other/RoughAlphaAdd", true);

		// Token: 0x0400070D RID: 1805
		public static readonly Texture2D RippleTex = ContentFinder<Texture2D>.Get("Other/Ripples", true);

		// Token: 0x0400070E RID: 1806
		public static readonly Texture2D NoiseTex = ContentFinder<Texture2D>.Get("Other/Noise", true);

		// Token: 0x0400070F RID: 1807
		public static readonly Texture2D InvisDistortion = ContentFinder<Texture2D>.Get("Other/InvisDistortion", true);

		// Token: 0x04000710 RID: 1808
		public static readonly Texture2D SpecularNoiseTex = ContentFinder<Texture2D>.Get("Other/SpecularMetal", true);
	}
}
