using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000711 RID: 1809
	public class HediffCompProperties_VerbGiver : HediffCompProperties
	{
		// Token: 0x06002C2E RID: 11310 RVA: 0x000E5B31 File Offset: 0x000E3D31
		public HediffCompProperties_VerbGiver()
		{
			this.compClass = typeof(HediffComp_VerbGiver);
		}

		// Token: 0x06002C2F RID: 11311 RVA: 0x000E5B4C File Offset: 0x000E3D4C
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.tools != null)
			{
				for (int i = 0; i < this.tools.Count; i++)
				{
					this.tools[i].id = i.ToString();
				}
			}
		}

		// Token: 0x06002C30 RID: 11312 RVA: 0x000E5B95 File Offset: 0x000E3D95
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.tools != null)
			{
				Tool tool = this.tools.SelectMany((Tool lhs) => from rhs in this.tools
				where lhs != rhs && lhs.id == rhs.id
				select rhs).FirstOrDefault<Tool>();
				if (tool != null)
				{
					yield return string.Format("duplicate hediff tool id {0}", tool.id);
				}
				foreach (Tool tool2 in this.tools)
				{
					foreach (string text2 in tool2.ConfigErrors())
					{
						yield return text2;
					}
					enumerator = null;
				}
				List<Tool>.Enumerator enumerator2 = default(List<Tool>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x0400237D RID: 9085
		public List<VerbProperties> verbs;

		// Token: 0x0400237E RID: 9086
		public List<Tool> tools;

		// Token: 0x0400237F RID: 9087
		public ImplementOwnerTypeDef ownerTypeOverride;
	}
}
