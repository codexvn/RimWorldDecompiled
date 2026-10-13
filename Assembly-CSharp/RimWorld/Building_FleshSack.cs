using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020029FF RID: 10751
	public class Building_FleshSack : Building_Casket
	{
		// Token: 0x170024CD RID: 9421
		// (get) Token: 0x0600EDB8 RID: 60856 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CanOpen
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600EDB9 RID: 60857 RVA: 0x0044F698 File Offset: 0x0044D898
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (base.Spawned && this.innerContainer.Count > 0 && mode != DestroyMode.Vanish)
			{
				Messages.Message("FleshmassContainedMessage".Translate() + ": " + this.innerContainer.ContentsString.CapitalizeFirst(), new LookTargets(base.Position, base.Map), MessageTypeDefOf.NegativeEvent, true);
			}
			base.Destroy(mode);
		}
	}
}
