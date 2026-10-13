using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002897 RID: 10391
	public abstract class ScenPart_ConfigPage_ConfigureStartingPawnsBase : ScenPart_ConfigPage
	{
		// Token: 0x1700229A RID: 8858
		// (get) Token: 0x0600E37B RID: 58235
		protected abstract int TotalPawnCount { get; }

		// Token: 0x0600E37C RID: 58236
		protected abstract void GenerateStartingPawns();

		// Token: 0x0600E37D RID: 58237 RVA: 0x00427B9C File Offset: 0x00425D9C
		public override void PostIdeoChosen()
		{
			Find.GameInitData.startingPawnCount = this.TotalPawnCount;
			if (ModsConfig.BiotechActive)
			{
				Current.Game.customXenotypeDatabase.customXenotypes.Clear();
				foreach (Ideo ideo in Find.IdeoManager.IdeosListForReading)
				{
					foreach (Precept precept in ideo.PreceptsListForReading)
					{
						Precept_Xenotype precept_Xenotype = precept as Precept_Xenotype;
						if (precept_Xenotype != null && precept_Xenotype.customXenotype != null && !Current.Game.customXenotypeDatabase.customXenotypes.Contains(precept_Xenotype.customXenotype))
						{
							Current.Game.customXenotypeDatabase.customXenotypes.Add(precept_Xenotype.customXenotype);
						}
					}
				}
			}
			if (ModsConfig.IdeologyActive)
			{
				Faction ofPlayerSilentFail = Faction.OfPlayerSilentFail;
				bool flag;
				if (ofPlayerSilentFail == null)
				{
					flag = (null != null);
				}
				else
				{
					FactionIdeosTracker ideos = ofPlayerSilentFail.ideos;
					flag = (((ideos != null) ? ideos.PrimaryIdeo : null) != null);
				}
				if (flag)
				{
					foreach (Precept precept2 in Faction.OfPlayerSilentFail.ideos.PrimaryIdeo.PreceptsListForReading)
					{
						if (precept2.def.defaultDrugPolicyOverride != null)
						{
							Current.Game.drugPolicyDatabase.MakePolicyDefault(precept2.def.defaultDrugPolicyOverride);
						}
					}
				}
			}
			this.GenerateStartingPawns();
			while (Find.GameInitData.startingAndOptionalPawns.Count < this.pawnChoiceCount)
			{
				StartingPawnUtility.AddNewPawn(-1);
			}
		}

		// Token: 0x0600E37E RID: 58238 RVA: 0x00427D5C File Offset: 0x00425F5C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.pawnChoiceCount, "pawnChoiceCount", 0, false);
		}

		// Token: 0x0600E37F RID: 58239 RVA: 0x00427D76 File Offset: 0x00425F76
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ this.pawnChoiceCount;
		}

		// Token: 0x0400985A RID: 39002
		public int pawnChoiceCount = 10;

		// Token: 0x0400985B RID: 39003
		protected const int MaxPawnCount = 10;
	}
}
