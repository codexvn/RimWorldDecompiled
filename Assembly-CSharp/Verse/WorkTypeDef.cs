using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000318 RID: 792
	public class WorkTypeDef : Def
	{
		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x060013BD RID: 5053 RVA: 0x0005D72B File Offset: 0x0005B92B
		public bool VisibleCurrently
		{
			get
			{
				if (this.cachedFrameVisibleCurrently == -1 || this.cachedFrameVisibleCurrently < Time.frameCount - 30)
				{
					this.cachedVisibleCurrently = this.VisibleNow(null, null);
					this.cachedFrameVisibleCurrently = Time.frameCount;
				}
				return this.cachedVisibleCurrently;
			}
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x0005D768 File Offset: 0x0005B968
		public bool VisibleNow(Pawn ignorePawn = null, Pawn alsoCheckPawn = null)
		{
			if (!this.visible)
			{
				return false;
			}
			if (this.visibleOnlyWithChildrenInColony)
			{
				bool flag = false;
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction)
				{
					if (pawn.RaceProps.Humanlike && pawn != ignorePawn && pawn.DevelopmentalStage.Juvenile())
					{
						flag = true;
						break;
					}
				}
				if (alsoCheckPawn != null && alsoCheckPawn.DevelopmentalStage.Juvenile())
				{
					flag = true;
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x0005D804 File Offset: 0x0005BA04
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.naturalPriority < 0 || this.naturalPriority > 10000)
			{
				yield return "naturalPriority is " + this.naturalPriority.ToString() + ", but it must be between 0 and 10000";
			}
			yield break;
			yield break;
		}

		// Token: 0x060013C0 RID: 5056 RVA: 0x0005D814 File Offset: 0x0005BA14
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			foreach (WorkGiverDef item in from d in DefDatabase<WorkGiverDef>.AllDefs
			where d.workType == this
			orderby d.priorityInType descending
			select d)
			{
				this.workGiversByPriority.Add(item);
			}
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x0005D8A0 File Offset: 0x0005BAA0
		public override int GetHashCode()
		{
			return Gen.HashCombine<string>(this.defName.GetHashCode(), this.gerundLabel);
		}

		// Token: 0x040012CC RID: 4812
		public WorkTags workTags;

		// Token: 0x040012CD RID: 4813
		[MustTranslate]
		public string labelShort;

		// Token: 0x040012CE RID: 4814
		[MustTranslate]
		public string pawnLabel;

		// Token: 0x040012CF RID: 4815
		[MustTranslate]
		public string gerundLabel;

		// Token: 0x040012D0 RID: 4816
		[MustTranslate]
		public string verb;

		// Token: 0x040012D1 RID: 4817
		public bool visible = true;

		// Token: 0x040012D2 RID: 4818
		public bool visibleOnlyWithChildrenInColony;

		// Token: 0x040012D3 RID: 4819
		public int naturalPriority;

		// Token: 0x040012D4 RID: 4820
		public bool alwaysStartActive;

		// Token: 0x040012D5 RID: 4821
		public bool requireCapableColonist;

		// Token: 0x040012D6 RID: 4822
		public List<SkillDef> relevantSkills = new List<SkillDef>();

		// Token: 0x040012D7 RID: 4823
		public bool disabledForSlaves;

		// Token: 0x040012D8 RID: 4824
		[Unsaved(false)]
		public List<WorkGiverDef> workGiversByPriority = new List<WorkGiverDef>();

		// Token: 0x040012D9 RID: 4825
		[Unsaved(false)]
		private bool cachedVisibleCurrently;

		// Token: 0x040012DA RID: 4826
		[Unsaved(false)]
		private int cachedFrameVisibleCurrently = -1;
	}
}
