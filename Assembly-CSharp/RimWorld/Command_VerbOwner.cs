using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BE4 RID: 11236
	public class Command_VerbOwner : Command_VerbTarget
	{
		// Token: 0x1700277B RID: 10107
		// (get) Token: 0x0600FB89 RID: 64393 RVA: 0x004909E8 File Offset: 0x0048EBE8
		public override string Label
		{
			get
			{
				if (this.comp.parent.def.mergeVerbGizmos || Find.Selector.SelectedPawns.Count < 2)
				{
					return base.Label;
				}
				return base.Label + " (" + this.verb.caster.LabelShortCap + ")";
			}
		}

		// Token: 0x0600FB8A RID: 64394 RVA: 0x00490A4A File Offset: 0x0048EC4A
		public Command_VerbOwner(CompApparelVerbOwner comp)
		{
			this.comp = comp;
		}

		// Token: 0x1700277C RID: 10108
		// (get) Token: 0x0600FB8B RID: 64395 RVA: 0x00490A59 File Offset: 0x0048EC59
		public override string TopRightLabel
		{
			get
			{
				return this.comp.GizmoExtraLabel;
			}
		}

		// Token: 0x1700277D RID: 10109
		// (get) Token: 0x0600FB8C RID: 64396 RVA: 0x00490A68 File Offset: 0x0048EC68
		public override Color IconDrawColor
		{
			get
			{
				Color? color = this.overrideColor;
				if (color == null)
				{
					return base.IconDrawColor;
				}
				return color.GetValueOrDefault();
			}
		}

		// Token: 0x0600FB8D RID: 64397 RVA: 0x00490A93 File Offset: 0x0048EC93
		public override void GizmoUpdateOnMouseover()
		{
			this.verb.DrawHighlight(LocalTargetInfo.Invalid);
		}

		// Token: 0x0600FB8E RID: 64398 RVA: 0x00490AA8 File Offset: 0x0048ECA8
		public override bool GroupsWith(Gizmo other)
		{
			if (!base.GroupsWith(other))
			{
				return false;
			}
			Command_VerbOwner command_VerbOwner = other as Command_VerbOwner;
			return command_VerbOwner != null && this.comp.parent.def == command_VerbOwner.comp.parent.def && this.comp.parent.def.mergeVerbGizmos;
		}

		// Token: 0x0400A50C RID: 42252
		private readonly CompApparelVerbOwner comp;

		// Token: 0x0400A50D RID: 42253
		public Color? overrideColor;
	}
}
