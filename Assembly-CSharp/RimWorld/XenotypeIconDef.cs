using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B37 RID: 6967
	public class XenotypeIconDef : Def
	{
		// Token: 0x170017F6 RID: 6134
		// (get) Token: 0x06009BA3 RID: 39843 RVA: 0x002D2540 File Offset: 0x002D0740
		public Texture2D Icon
		{
			get
			{
				if (this.cachedIcon == null)
				{
					this.cachedIcon = ContentFinder<Texture2D>.Get(this.texPath, true);
				}
				return this.cachedIcon;
			}
		}

		// Token: 0x06009BA4 RID: 39844 RVA: 0x002D2568 File Offset: 0x002D0768
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.texPath.NullOrEmpty())
			{
				yield return "texPath is empty";
			}
			yield break;
			yield break;
		}

		// Token: 0x04006C13 RID: 27667
		[NoTranslate]
		public string texPath;

		// Token: 0x04006C14 RID: 27668
		[Unsaved(false)]
		private Texture2D cachedIcon;
	}
}
