using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001B05 RID: 6917
	public class TaleDef : Def
	{
		// Token: 0x06009A6C RID: 39532 RVA: 0x002CE57C File Offset: 0x002CC77C
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.taleClass == null)
			{
				yield return this.defName + " taleClass is null.";
			}
			if (this.expireDays < 0f)
			{
				if (this.type == TaleType.Expirable)
				{
					yield return "Expirable tale type is used but expireDays<0";
				}
			}
			else if (this.type != TaleType.Expirable)
			{
				yield return "Non expirable tale type is used but expireDays>=0";
			}
			if (this.baseInterest > 1E-06f && !this.usableForArt)
			{
				yield return "Non-zero baseInterest but not usable for art";
			}
			if (this.firstPawnSymbol == "pawn" || this.secondPawnSymbol == "pawn")
			{
				yield return "pawn symbols should not be 'pawn', this is the default and only choice for SinglePawn tales so using it here is confusing.";
			}
			yield break;
			yield break;
		}

		// Token: 0x06009A6D RID: 39533 RVA: 0x002CE58C File Offset: 0x002CC78C
		public static TaleDef Named(string str)
		{
			return DefDatabase<TaleDef>.GetNamed(str, true);
		}

		// Token: 0x04006A69 RID: 27241
		public TaleType type;

		// Token: 0x04006A6A RID: 27242
		public Type taleClass;

		// Token: 0x04006A6B RID: 27243
		public bool usableForArt = true;

		// Token: 0x04006A6C RID: 27244
		public bool colonistOnly = true;

		// Token: 0x04006A6D RID: 27245
		public bool usableWithChildren = true;

		// Token: 0x04006A6E RID: 27246
		public int maxPerPawn = -1;

		// Token: 0x04006A6F RID: 27247
		public float ignoreChance;

		// Token: 0x04006A70 RID: 27248
		public float expireDays = -1f;

		// Token: 0x04006A71 RID: 27249
		public RulePack rulePack;

		// Token: 0x04006A72 RID: 27250
		[NoTranslate]
		public string firstPawnSymbol;

		// Token: 0x04006A73 RID: 27251
		[NoTranslate]
		public string secondPawnSymbol;

		// Token: 0x04006A74 RID: 27252
		[NoTranslate]
		public string defSymbol;

		// Token: 0x04006A75 RID: 27253
		public Type defType = typeof(ThingDef);

		// Token: 0x04006A76 RID: 27254
		public float baseInterest;

		// Token: 0x04006A77 RID: 27255
		public Color historyGraphColor = Color.white;
	}
}
