using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C87 RID: 7303
	public abstract class QuestPart_RequirementsToAccept : QuestPart
	{
		// Token: 0x170018FF RID: 6399
		// (get) Token: 0x0600A2BB RID: 41659 RVA: 0x002EC854 File Offset: 0x002EAA54
		public virtual IEnumerable<GlobalTargetInfo> Culprits
		{
			get
			{
				return Enumerable.Empty<GlobalTargetInfo>();
			}
		}

		// Token: 0x0600A2BC RID: 41660
		public abstract AcceptanceReport CanAccept();

		// Token: 0x17001900 RID: 6400
		// (get) Token: 0x0600A2BD RID: 41661 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool ShowInRequirementBox
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600A2BE RID: 41662 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanPawnAccept(Pawn p)
		{
			return true;
		}
	}
}
