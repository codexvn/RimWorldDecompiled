using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200319F RID: 12703
	public class Dialog_Negotiation : Dialog_NodeTree
	{
		// Token: 0x17002E74 RID: 11892
		// (get) Token: 0x06011DFC RID: 73212 RVA: 0x005306D1 File Offset: 0x0052E8D1
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(720f, 600f);
			}
		}

		// Token: 0x06011DFD RID: 73213 RVA: 0x005306E2 File Offset: 0x0052E8E2
		public Dialog_Negotiation(Pawn negotiator, ICommunicable commTarget, DiaNode startNode, bool radioMode) : base(startNode, radioMode, false, null)
		{
			this.negotiator = negotiator;
			this.commTarget = commTarget;
		}

		// Token: 0x06011DFE RID: 73214 RVA: 0x00530700 File Offset: 0x0052E900
		public override void DoWindowContents(Rect inRect)
		{
			Widgets.BeginGroup(inRect);
			Rect rect = new Rect(0f, 0f, inRect.width / 2f, 70f);
			Rect rect2 = new Rect(0f, rect.yMax, rect.width, 60f);
			Rect rect3 = new Rect(inRect.width / 2f, 0f, inRect.width / 2f, 70f);
			Rect rect4 = new Rect(inRect.width / 2f, rect.yMax, rect.width, 60f);
			Text.Font = GameFont.Medium;
			Widgets.Label(rect, this.negotiator.LabelCap);
			Text.Anchor = TextAnchor.UpperRight;
			Widgets.Label(rect3, this.commTarget.GetCallLabel());
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			GUI.color = new Color(1f, 1f, 1f, 0.7f);
			Widgets.Label(rect2, "SocialSkillIs".Translate(this.negotiator.skills.GetSkill(SkillDefOf.Social).Level));
			Text.Anchor = TextAnchor.UpperRight;
			Widgets.Label(rect4, this.commTarget.GetInfoText());
			Faction faction = this.commTarget.GetFaction();
			if (faction != null)
			{
				FactionRelationKind playerRelationKind = faction.PlayerRelationKind;
				GUI.color = playerRelationKind.GetColor();
				Widgets.Label(new Rect(rect4.x, rect4.y + Text.CalcHeight(this.commTarget.GetInfoText(), rect4.width) + Text.SpaceBetweenLines, rect4.width, 30f), playerRelationKind.GetLabelCap());
			}
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
			Widgets.EndGroup();
			float num = 147f;
			Rect rect5 = new Rect(0f, num, inRect.width, inRect.height - num);
			base.DrawNode(rect5);
		}

		// Token: 0x0400B8D4 RID: 47316
		protected Pawn negotiator;

		// Token: 0x0400B8D5 RID: 47317
		protected ICommunicable commTarget;

		// Token: 0x0400B8D6 RID: 47318
		private const float TitleHeight = 70f;

		// Token: 0x0400B8D7 RID: 47319
		private const float InfoHeight = 60f;
	}
}
