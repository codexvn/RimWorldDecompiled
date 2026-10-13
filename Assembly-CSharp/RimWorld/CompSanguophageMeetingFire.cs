using System;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002DF7 RID: 11767
	[StaticConstructorOnStartup]
	public class CompSanguophageMeetingFire : CompFireOverlayBase
	{
		// Token: 0x17002A55 RID: 10837
		// (get) Token: 0x060108F7 RID: 67831 RVA: 0x004C6473 File Offset: 0x004C4673
		public new CompProperties_SanguophageMeetingFire Props
		{
			get
			{
				return (CompProperties_SanguophageMeetingFire)this.props;
			}
		}

		// Token: 0x060108F8 RID: 67832 RVA: 0x004C6480 File Offset: 0x004C4680
		public override void PostDraw()
		{
			base.PostDraw();
			CompGlower compGlower = this.parent.TryGetComp<CompGlower>();
			if (compGlower != null && !compGlower.Glows)
			{
				return;
			}
			Vector3 drawPos = this.parent.DrawPos;
			drawPos.y += 0.03658537f;
			CompSanguophageMeetingFire.RedlightGraphic.Draw(drawPos + this.Props.offset, Rot4.North, this.parent, 0f);
		}

		// Token: 0x060108F9 RID: 67833 RVA: 0x004C64F7 File Offset: 0x004C46F7
		public override bool CompPreventClaimingBy(Faction faction)
		{
			Lord lord = ((Building)this.parent).GetLord();
			return ((lord != null) ? lord.CurLordToil : null) is LordToil_SanguophageMeeting;
		}

		// Token: 0x0400ACA2 RID: 44194
		public static readonly Graphic RedlightGraphic = GraphicDatabase.Get<Graphic_Flicker>("Things/Special/Redlight", ShaderDatabase.TransparentPostLight, Vector2.one, Color.white);
	}
}
