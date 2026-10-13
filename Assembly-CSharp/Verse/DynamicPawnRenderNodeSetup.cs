using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Verse
{
	// Token: 0x02000607 RID: 1543
	public abstract class DynamicPawnRenderNodeSetup
	{
		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06002771 RID: 10097 RVA: 0x000D228F File Offset: 0x000D048F
		public virtual List<Type> SetupAfter { get; }

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x06002772 RID: 10098
		public abstract bool HumanlikeOnly { get; }

		// Token: 0x06002773 RID: 10099
		[return: TupleElementNames(new string[]
		{
			"node",
			"parent"
		})]
		public abstract IEnumerable<ValueTuple<PawnRenderNode, PawnRenderNode>> GetDynamicNodes(Pawn pawn, PawnRenderTree tree);
	}
}
