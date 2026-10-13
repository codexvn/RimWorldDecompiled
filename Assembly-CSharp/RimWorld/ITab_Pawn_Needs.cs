using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020033C1 RID: 13249
	public class ITab_Pawn_Needs : ITab
	{
		// Token: 0x170030AD RID: 12461
		// (get) Token: 0x060129E1 RID: 76257 RVA: 0x005825A8 File Offset: 0x005807A8
		public override bool IsVisible
		{
			get
			{
				if (this.SelPawn.RaceProps.Animal && this.SelPawn.Faction == null)
				{
					return false;
				}
				if (this.SelPawn.RaceProps.Insect && this.SelPawn.Faction != Faction.OfPlayer)
				{
					return false;
				}
				CompHoldingPlatformTarget compHoldingPlatformTarget = this.SelPawn.TryGetComp<CompHoldingPlatformTarget>();
				return (compHoldingPlatformTarget == null || !compHoldingPlatformTarget.CurrentlyHeldOnPlatform) && this.SelPawn.needs != null && this.SelPawn.needs.AllNeeds.Count > 0;
			}
		}

		// Token: 0x060129E2 RID: 76258 RVA: 0x0058263C File Offset: 0x0058083C
		public ITab_Pawn_Needs()
		{
			this.labelKey = "TabNeeds";
			this.tutorTag = "Needs";
		}

		// Token: 0x060129E3 RID: 76259 RVA: 0x0058265A File Offset: 0x0058085A
		public override void OnOpen()
		{
			this.thoughtScrollPosition = default(Vector2);
		}

		// Token: 0x060129E4 RID: 76260 RVA: 0x00582668 File Offset: 0x00580868
		protected override void FillTab()
		{
			NeedsCardUtility.DoNeedsMoodAndThoughts(new Rect(0f, 0f, this.size.x, this.size.y), this.SelPawn, ref this.thoughtScrollPosition);
		}

		// Token: 0x060129E5 RID: 76261 RVA: 0x005826A0 File Offset: 0x005808A0
		protected override void UpdateSize()
		{
			this.size = NeedsCardUtility.GetSize(this.SelPawn);
		}

		// Token: 0x0400C164 RID: 49508
		private Vector2 thoughtScrollPosition;
	}
}
