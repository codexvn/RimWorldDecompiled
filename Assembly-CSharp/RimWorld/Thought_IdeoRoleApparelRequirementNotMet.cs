using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200253F RID: 9535
	public class Thought_IdeoRoleApparelRequirementNotMet : Thought_Situational
	{
		// Token: 0x17002013 RID: 8211
		// (get) Token: 0x0600D09B RID: 53403 RVA: 0x003D5602 File Offset: 0x003D3802
		public Precept_Role Role
		{
			get
			{
				return (Precept_Role)this.sourcePrecept;
			}
		}

		// Token: 0x17002014 RID: 8212
		// (get) Token: 0x0600D09C RID: 53404 RVA: 0x003D582F File Offset: 0x003D3A2F
		public override string LabelCap
		{
			get
			{
				return base.CurStage.LabelCap.Formatted(this.Role.Named("ROLE"));
			}
		}

		// Token: 0x17002015 RID: 8213
		// (get) Token: 0x0600D09D RID: 53405 RVA: 0x003D5858 File Offset: 0x003D3A58
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.Role.Named("ROLE")) + ":\n\n" + this.GetAllRequiredApparel(this.pawn).ToLineList(" -  ");
			}
		}

		// Token: 0x0600D09E RID: 53406 RVA: 0x003D58B0 File Offset: 0x003D3AB0
		protected override ThoughtState CurrentStateInternal()
		{
			using (IEnumerator<Pawn> enumerator = this.Role.ChosenPawns().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == this.pawn && this.GetAllRequiredApparel(this.pawn).Count > 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600D09F RID: 53407 RVA: 0x003D5928 File Offset: 0x003D3B28
		private List<string> GetAllRequiredApparel(Pawn p)
		{
			this.unmetReqsListTmp.Clear();
			if (ModsConfig.IdeologyActive)
			{
				Ideo ideo = p.Ideo;
				Precept_Role precept_Role = (ideo != null) ? ideo.GetRole(p) : null;
				if (precept_Role != null && !precept_Role.apparelRequirements.NullOrEmpty<PreceptApparelRequirement>())
				{
					for (int i = 0; i < precept_Role.apparelRequirements.Count; i++)
					{
						ApparelRequirement requirement = precept_Role.apparelRequirements[i].requirement;
						if (requirement.IsActive(p) && !requirement.IsMet(p))
						{
							if (!requirement.groupLabel.NullOrEmpty())
							{
								this.unmetReqsListTmp.Add(requirement.groupLabel);
							}
							else
							{
								foreach (ThingDef thingDef in requirement.AllRequiredApparelForPawn(p, false, false))
								{
									this.unmetReqsListTmp.Add(thingDef.LabelCap);
								}
							}
						}
					}
				}
			}
			return this.unmetReqsListTmp;
		}

		// Token: 0x04008DD4 RID: 36308
		private List<string> unmetReqsListTmp = new List<string>();
	}
}
