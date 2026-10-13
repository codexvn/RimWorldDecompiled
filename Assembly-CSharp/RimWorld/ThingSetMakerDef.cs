using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B07 RID: 6919
	public class ThingSetMakerDef : Def
	{
		// Token: 0x06009A79 RID: 39545 RVA: 0x002CE88F File Offset: 0x002CCA8F
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			this.root.ResolveReferences();
		}

		// Token: 0x06009A7A RID: 39546 RVA: 0x002CE8A2 File Offset: 0x002CCAA2
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.root == null)
			{
				yield return "root is null.";
			}
			else
			{
				foreach (string text2 in this.root.ConfigErrors())
				{
					yield return text2;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x04006A7D RID: 27261
		public ThingSetMaker root;

		// Token: 0x04006A7E RID: 27262
		public ThingSetMakerParams debugParams;
	}
}
