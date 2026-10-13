using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C4E RID: 11342
	[StaticConstructorOnStartup]
	public class CompDarklightOverlay : CompFireOverlayBase
	{
		// Token: 0x1700280C RID: 10252
		// (get) Token: 0x0600FE55 RID: 65109 RVA: 0x0049BE2F File Offset: 0x0049A02F
		public new CompProperties_DarklightOverlay Props
		{
			get
			{
				return (CompProperties_DarklightOverlay)this.props;
			}
		}

		// Token: 0x0600FE56 RID: 65110 RVA: 0x0049BE3C File Offset: 0x0049A03C
		public override void PostDraw()
		{
			base.PostDraw();
			if (this.refuelableComp != null && !this.refuelableComp.HasFuel)
			{
				return;
			}
			Vector3 drawPos = this.parent.DrawPos;
			drawPos.y += 0.03658537f;
			CompDarklightOverlay.DarklightGraphic.Draw(drawPos, Rot4.North, this.parent, 0f);
		}

		// Token: 0x0600FE57 RID: 65111 RVA: 0x0049BE9C File Offset: 0x0049A09C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
		}

		// Token: 0x0400A6B0 RID: 42672
		protected CompRefuelable refuelableComp;

		// Token: 0x0400A6B1 RID: 42673
		public static readonly Graphic DarklightGraphic = GraphicDatabase.Get<Graphic_Flicker>("Things/Special/Darklight", ShaderDatabase.TransparentPostLight, Vector2.one, Color.white);
	}
}
