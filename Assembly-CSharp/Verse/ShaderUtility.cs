using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000161 RID: 353
	public static class ShaderUtility
	{
		// Token: 0x06000A84 RID: 2692 RVA: 0x00031B40 File Offset: 0x0002FD40
		public static bool SupportsMaskTex(this Shader shader)
		{
			int propertyCount = shader.GetPropertyCount();
			for (int i = 0; i < propertyCount; i++)
			{
				if (shader.GetPropertyNameId(i) == ShaderPropertyIDs.MaskTex)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x00031B74 File Offset: 0x0002FD74
		public static Shader GetSkinShader(Pawn pawn)
		{
			foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
			{
				if (hediff.def.skinShader != null)
				{
					return hediff.def.skinShader.Shader;
				}
			}
			bool dead = pawn.Dead || (pawn.IsMutant && pawn.mutant.HasTurned && pawn.mutant.Def.useCorpseGraphics);
			return ShaderUtility.GetSkinShaderAbstract(pawn.story != null && pawn.story.SkinColorOverriden, dead);
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x00031C3C File Offset: 0x0002FE3C
		public static Shader GetSkinShaderAbstract(bool skinColorOverriden, bool dead)
		{
			if (skinColorOverriden)
			{
				return ShaderDatabase.CutoutSkinColorOverride;
			}
			if (dead)
			{
				return ShaderDatabase.Cutout;
			}
			return ShaderDatabase.CutoutSkin;
		}
	}
}
