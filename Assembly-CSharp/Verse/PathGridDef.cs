using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x020002B1 RID: 689
	public class PathGridDef : Def
	{
		// Token: 0x060010E9 RID: 4329 RVA: 0x0004F86F File Offset: 0x0004DA6F
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.workerType == null)
			{
				yield return "Path grid def worker type is not present.";
			}
			else if (!typeof(PathGrid).IsAssignableFrom(this.workerType))
			{
				yield return "Path grid def worker type is not a subclass of PathGrid, type was: " + this.workerType.FullName;
			}
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x04000E61 RID: 3681
		public Type workerType = typeof(PathGrid);

		// Token: 0x04000E62 RID: 3682
		public bool fencePassable;

		// Token: 0x04000E63 RID: 3683
		public bool flying;
	}
}
