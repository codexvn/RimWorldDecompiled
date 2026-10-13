using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000184 RID: 388
	public class PawnRenderTreeDef : Def
	{
		// Token: 0x06000AFD RID: 2813 RVA: 0x00034C48 File Offset: 0x00032E48
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

		// Token: 0x06000AFE RID: 2814 RVA: 0x00034C58 File Offset: 0x00032E58
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			PawnRenderNodeProperties pawnRenderNodeProperties = this.root;
			if (pawnRenderNodeProperties == null)
			{
				return;
			}
			pawnRenderNodeProperties.ResolveReferencesRecursive();
		}

		// Token: 0x04000649 RID: 1609
		public PawnRenderNodeProperties root;
	}
}
