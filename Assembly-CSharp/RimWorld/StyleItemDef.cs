using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AFB RID: 6907
	public abstract class StyleItemDef : Def
	{
		// Token: 0x170017AD RID: 6061
		// (get) Token: 0x06009A45 RID: 39493 RVA: 0x002CDEA3 File Offset: 0x002CC0A3
		public virtual Texture2D Icon
		{
			get
			{
				if (this.noGraphic)
				{
					return BaseContent.ClearTex;
				}
				return ContentFinder<Texture2D>.Get(this.iconPath ?? (this.texPath + "_south"), true);
			}
		}

		// Token: 0x170017AE RID: 6062
		// (get) Token: 0x06009A46 RID: 39494 RVA: 0x002CDED3 File Offset: 0x002CC0D3
		public StyleItemCategoryDef StyleItemCategory
		{
			get
			{
				if (this.category == null)
				{
					return StyleItemCategoryDefOf.Misc;
				}
				return this.category;
			}
		}

		// Token: 0x170017AF RID: 6063
		// (get) Token: 0x06009A47 RID: 39495 RVA: 0x002CDEE9 File Offset: 0x002CC0E9
		public static IEnumerable<StyleItemDef> AllStyleItemDefs
		{
			get
			{
				foreach (HairDef hairDef in DefDatabase<HairDef>.AllDefs)
				{
					yield return hairDef;
				}
				IEnumerator<HairDef> enumerator = null;
				foreach (BeardDef beardDef in DefDatabase<BeardDef>.AllDefs)
				{
					yield return beardDef;
				}
				IEnumerator<BeardDef> enumerator2 = null;
				foreach (TattooDef tattooDef in DefDatabase<TattooDef>.AllDefs)
				{
					yield return tattooDef;
				}
				IEnumerator<TattooDef> enumerator3 = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x06009A48 RID: 39496
		public abstract Graphic GraphicFor(Pawn pawn, Color color);

		// Token: 0x06009A49 RID: 39497 RVA: 0x002CDEF2 File Offset: 0x002CC0F2
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.texPath.NullOrEmpty() && !this.noGraphic)
			{
				yield return "no texPath with noGraphic disabled.";
			}
			yield break;
			yield break;
		}

		// Token: 0x04006A47 RID: 27207
		[LoadAlias("hairTags")]
		[NoTranslate]
		public List<string> styleTags = new List<string>();

		// Token: 0x04006A48 RID: 27208
		[LoadAlias("hairGender")]
		public StyleGender styleGender = StyleGender.Any;

		// Token: 0x04006A49 RID: 27209
		private StyleItemCategoryDef category;

		// Token: 0x04006A4A RID: 27210
		public bool noGraphic;

		// Token: 0x04006A4B RID: 27211
		public ShaderTypeDef overrideShaderTypeDef;

		// Token: 0x04006A4C RID: 27212
		public GeneDef requiredGene;

		// Token: 0x04006A4D RID: 27213
		public MutantDef requiredMutant;

		// Token: 0x04006A4E RID: 27214
		[NoTranslate]
		public string texPath;

		// Token: 0x04006A4F RID: 27215
		[NoTranslate]
		public string iconPath;
	}
}
