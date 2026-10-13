using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02003123 RID: 12579
	public class Dialog_BeginPsychicRitual : Dialog_BeginLordJob
	{
		// Token: 0x17002DF7 RID: 11767
		// (get) Token: 0x06011AC7 RID: 72391 RVA: 0x00514874 File Offset: 0x00512A74
		public override TaggedString HeaderLabel
		{
			get
			{
				return this.psychicRitualDef.LabelCap;
			}
		}

		// Token: 0x17002DF8 RID: 11768
		// (get) Token: 0x06011AC8 RID: 72392 RVA: 0x00514881 File Offset: 0x00512A81
		public override TaggedString DescriptionLabel
		{
			get
			{
				return this.psychicRitualDef.description.CapitalizeFirst();
			}
		}

		// Token: 0x17002DF9 RID: 11769
		// (get) Token: 0x06011AC9 RID: 72393 RVA: 0x00514898 File Offset: 0x00512A98
		public override TaggedString ExpectedQualityLabel
		{
			get
			{
				return "PsychicRitualExpectedQualityLabel".Translate().CapitalizeFirst();
			}
		}

		// Token: 0x17002DFA RID: 11770
		// (get) Token: 0x06011ACA RID: 72394 RVA: 0x005148B8 File Offset: 0x00512AB8
		public override TaggedString QualityFactorsLabel
		{
			get
			{
				return "PsychicRitualQualityFactorLabel".Translate().CapitalizeFirst();
			}
		}

		// Token: 0x17002DFB RID: 11771
		// (get) Token: 0x06011ACB RID: 72395 RVA: 0x005148D7 File Offset: 0x00512AD7
		public override Texture2D Icon
		{
			get
			{
				return this.psychicRitualDef.uiIcon;
			}
		}

		// Token: 0x06011ACC RID: 72396 RVA: 0x005148E4 File Offset: 0x00512AE4
		public Dialog_BeginPsychicRitual(PsychicRitualDef def, PsychicRitualCandidatePool candidatePool, PsychicRitualRoleAssignments assignments, Map map) : base(new PawnPsychicRitualRoleSelectionWidget(def, candidatePool, assignments)
		{
			showIdeoIcon = false
		})
		{
			this.psychicRitualDef = def;
			this.assignments = assignments;
			this.map = map;
		}

		// Token: 0x06011ACD RID: 72397 RVA: 0x00514911 File Offset: 0x00512B11
		protected override IEnumerable<string> BlockingIssues()
		{
			return this.assignments.BlockingIssues().Concat(this.psychicRitualDef.BlockingIssues(this.assignments, this.map));
		}

		// Token: 0x06011ACE RID: 72398 RVA: 0x0051493A File Offset: 0x00512B3A
		protected override void Start()
		{
			this.psychicRitualDef.MakeNewLord(this.assignments);
			base.Start();
			Find.PsychicRitualManager.RegisterCooldown(this.psychicRitualDef);
		}

		// Token: 0x06011ACF RID: 72399 RVA: 0x00514964 File Offset: 0x00512B64
		protected override List<QualityFactor> PopulateQualityFactors(out FloatRange qualityRange)
		{
			FloatRange floatRange;
			List<QualityFactor> list = base.PopulateQualityFactors(out floatRange);
			float num;
			this.psychicRitualDef.CalculateMaxPower(this.assignments, list, out num);
			if (num > 0f)
			{
				qualityRange = new FloatRange(num, num);
			}
			else
			{
				qualityRange = new FloatRange(0f, 0f);
			}
			foreach (QualityFactor qualityFactor in list)
			{
				qualityFactor.qualityChange = ((qualityFactor.quality >= 0f) ? "+ " : "- ") + Mathf.Abs(qualityFactor.quality).ToStringPercent();
			}
			return list;
		}

		// Token: 0x06011AD0 RID: 72400 RVA: 0x00514A30 File Offset: 0x00512C30
		private static List<Pawn> SleepingPawns(PsychicRitualRoleAssignments assignments)
		{
			Dialog_BeginPsychicRitual.tmpSleepingPawns.Clear();
			foreach (KeyValuePair<PsychicRitualRoleDef, List<Pawn>> keyValuePair in assignments.RoleAssignments)
			{
				PsychicRitualRoleDef psychicRitualRoleDef;
				List<Pawn> list;
				keyValuePair.Deconstruct(out psychicRitualRoleDef, out list);
				PsychicRitualRoleDef psychicRitualRoleDef2 = psychicRitualRoleDef;
				List<Pawn> list2 = list;
				if (!psychicRitualRoleDef2.ConditionAllowed(PsychicRitualRoleDef.Condition.Sleeping))
				{
					foreach (Pawn pawn in list2)
					{
						if (!pawn.Awake() && pawn.health.capacities.CanBeAwake)
						{
							Dialog_BeginPsychicRitual.tmpSleepingPawns.Add(pawn);
						}
					}
				}
			}
			return Dialog_BeginPsychicRitual.tmpSleepingPawns;
		}

		// Token: 0x06011AD1 RID: 72401 RVA: 0x00514AFC File Offset: 0x00512CFC
		private static List<Pawn> DraftedPawns(PsychicRitualRoleAssignments assignments)
		{
			Dialog_BeginPsychicRitual.tmpDraftedPawns.Clear();
			foreach (KeyValuePair<PsychicRitualRoleDef, List<Pawn>> keyValuePair in assignments.RoleAssignments)
			{
				PsychicRitualRoleDef psychicRitualRoleDef;
				List<Pawn> list;
				keyValuePair.Deconstruct(out psychicRitualRoleDef, out list);
				PsychicRitualRoleDef psychicRitualRoleDef2 = psychicRitualRoleDef;
				List<Pawn> list2 = list;
				if (!psychicRitualRoleDef2.ConditionAllowed(PsychicRitualRoleDef.Condition.Drafted))
				{
					foreach (Pawn pawn in list2)
					{
						if (pawn.Drafted)
						{
							Dialog_BeginPsychicRitual.tmpDraftedPawns.Add(pawn);
						}
					}
				}
			}
			return Dialog_BeginPsychicRitual.tmpDraftedPawns;
		}

		// Token: 0x06011AD2 RID: 72402 RVA: 0x00514BB8 File Offset: 0x00512DB8
		public override void DrawExtraOutcomeDescriptions(Rect viewRect, FloatRange qualityRange, string qualityNumber, ref float curY, ref float totalInfoHeight)
		{
			curY += Text.LineHeight;
			totalInfoHeight += Text.LineHeight;
			TaggedString label = this.psychicRitualDef.OutcomeDescription(qualityRange, qualityNumber, this.assignments);
			float num = Text.CalcHeight(label.Resolve(), viewRect.width);
			Widgets.Label(new Rect(viewRect.x, curY, viewRect.width, num), label);
			curY += num;
			totalInfoHeight += num;
			foreach (TaggedString taggedString in this.psychicRitualDef.OutcomeWarnings(this.assignments))
			{
				float num2 = Math.Max(Text.CalcHeight(taggedString, viewRect.width) + 3f, 28f);
				Widgets.Label(new Rect(viewRect.x, curY + 4f, viewRect.width, num2), taggedString);
				curY += num2;
				totalInfoHeight += num2;
			}
			List<Pawn> list = Dialog_BeginPsychicRitual.SleepingPawns(this.assignments);
			if (list.Count > 0)
			{
				string value = (from pawn in list
				select pawn.LabelShortCap).ToCommaList(true, false);
				TaggedString taggedString2 = ((list.Count > 1) ? "PsychicRitualWakingPawnsWarning" : "PsychicRitualWakingPawnWarning").Translate(value);
				float num3 = Math.Max(Text.CalcHeight(taggedString2, viewRect.width) + 3f, 28f);
				Widgets.Label(new Rect(viewRect.x, curY + 4f, viewRect.width, num3), taggedString2);
				curY += num3;
				totalInfoHeight += num3;
			}
			List<Pawn> list2 = Dialog_BeginPsychicRitual.DraftedPawns(this.assignments);
			if (list2.Count > 0)
			{
				string value2 = (from pawn in list2
				select pawn.LabelShortCap).ToCommaList(true, false);
				TaggedString taggedString3 = ((list2.Count > 1) ? "PsychicRitualUndraftPawnsWarning" : "PsychicRitualUndraftPawnWarning").Translate(value2);
				float num4 = Math.Max(Text.CalcHeight(taggedString3, viewRect.width) + 3f, 28f);
				Widgets.Label(new Rect(viewRect.x, curY + 4f, viewRect.width, num4), taggedString3);
				curY += num4;
				totalInfoHeight += num4;
			}
		}

		// Token: 0x06011AD3 RID: 72403 RVA: 0x00514E64 File Offset: 0x00513064
		public override TaggedString ExpectedDurationLabel(FloatRange qualityRange)
		{
			return this.psychicRitualDef.TimeAndOfferingLabel();
		}

		// Token: 0x0400B60A RID: 46602
		private PsychicRitualDef psychicRitualDef;

		// Token: 0x0400B60B RID: 46603
		private PsychicRitualRoleAssignments assignments;

		// Token: 0x0400B60C RID: 46604
		private Map map;

		// Token: 0x0400B60D RID: 46605
		private static List<Pawn> tmpSleepingPawns = new List<Pawn>(16);

		// Token: 0x0400B60E RID: 46606
		private static List<Pawn> tmpDraftedPawns = new List<Pawn>(16);
	}
}
