using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020033EA RID: 13290
	public interface ITargetingSource
	{
		// Token: 0x06012AF9 RID: 76537
		bool CanHitTarget(LocalTargetInfo target);

		// Token: 0x06012AFA RID: 76538
		bool ValidateTarget(LocalTargetInfo target, bool showMessages = true);

		// Token: 0x06012AFB RID: 76539
		void DrawHighlight(LocalTargetInfo target);

		// Token: 0x06012AFC RID: 76540
		void OrderForceTarget(LocalTargetInfo target);

		// Token: 0x06012AFD RID: 76541
		void OnGUI(LocalTargetInfo target);

		// Token: 0x170030EA RID: 12522
		// (get) Token: 0x06012AFE RID: 76542
		bool CasterIsPawn { get; }

		// Token: 0x170030EB RID: 12523
		// (get) Token: 0x06012AFF RID: 76543
		bool IsMeleeAttack { get; }

		// Token: 0x170030EC RID: 12524
		// (get) Token: 0x06012B00 RID: 76544
		bool Targetable { get; }

		// Token: 0x170030ED RID: 12525
		// (get) Token: 0x06012B01 RID: 76545
		bool MultiSelect { get; }

		// Token: 0x170030EE RID: 12526
		// (get) Token: 0x06012B02 RID: 76546
		bool HidePawnTooltips { get; }

		// Token: 0x170030EF RID: 12527
		// (get) Token: 0x06012B03 RID: 76547
		Thing Caster { get; }

		// Token: 0x170030F0 RID: 12528
		// (get) Token: 0x06012B04 RID: 76548
		Pawn CasterPawn { get; }

		// Token: 0x170030F1 RID: 12529
		// (get) Token: 0x06012B05 RID: 76549
		Verb GetVerb { get; }

		// Token: 0x170030F2 RID: 12530
		// (get) Token: 0x06012B06 RID: 76550
		Texture2D UIIcon { get; }

		// Token: 0x170030F3 RID: 12531
		// (get) Token: 0x06012B07 RID: 76551
		TargetingParameters targetParams { get; }

		// Token: 0x170030F4 RID: 12532
		// (get) Token: 0x06012B08 RID: 76552
		ITargetingSource DestinationSelector { get; }
	}
}
