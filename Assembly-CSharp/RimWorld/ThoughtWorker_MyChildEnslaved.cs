using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018DE RID: 6366
	public class ThoughtWorker_MyChildEnslaved : ThoughtWorker
	{
		// Token: 0x060090FA RID: 37114 RVA: 0x002AAA04 File Offset: 0x002A8C04
		public override float MoodMultiplier(Pawn p)
		{
			return (float)Mathf.Min(this.def.stackLimit, ThoughtWorker_MyChildEnslaved.ChildrenEnslavedCount(p));
		}

		// Token: 0x060090FB RID: 37115 RVA: 0x002AAA20 File Offset: 0x002A8C20
		public override string PostProcessLabel(Pawn p, string label)
		{
			int num = Mathf.RoundToInt(this.MoodMultiplier(p));
			if (num <= 1)
			{
				return base.PostProcessLabel(p, label);
			}
			return base.PostProcessLabel(p, label) + " x" + num.ToString();
		}

		// Token: 0x060090FC RID: 37116 RVA: 0x002AAA60 File Offset: 0x002A8C60
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.BiotechActive)
			{
				return ThoughtState.Inactive;
			}
			if (p.Suspended)
			{
				return ThoughtState.Inactive;
			}
			if (ThoughtWorker_MyChildEnslaved.ChildrenEnslavedCount(p) <= 0)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveDefault;
		}

		// Token: 0x060090FD RID: 37117 RVA: 0x002AAA91 File Offset: 0x002A8C91
		private static int ChildrenEnslavedCount(Pawn parent)
		{
			return parent.relations.Children.Count((Pawn child) => child.DevelopmentalStage.Juvenile() && !child.Dead && child.IsSlave);
		}
	}
}
