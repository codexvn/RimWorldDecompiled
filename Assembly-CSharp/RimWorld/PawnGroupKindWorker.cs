using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AA9 RID: 6825
	public abstract class PawnGroupKindWorker
	{
		// Token: 0x060098DB RID: 39131
		public abstract float MinPointsToGenerateAnything(PawnGroupMaker groupMaker, FactionDef faction, PawnGroupMakerParms parms = null);

		// Token: 0x060098DC RID: 39132 RVA: 0x002C9070 File Offset: 0x002C7270
		public List<Pawn> GeneratePawns(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, bool errorOnZeroResults = true)
		{
			List<Pawn> list = new List<Pawn>();
			PawnGroupKindWorker.pawnsBeingGeneratedNow.Add(list);
			try
			{
				this.GeneratePawns(parms, groupMaker, list, errorOnZeroResults);
			}
			catch (Exception ex)
			{
				string str = "Exception while generating pawn group: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				for (int i = 0; i < list.Count; i++)
				{
					list[i].Destroy(DestroyMode.Vanish);
				}
				list.Clear();
			}
			finally
			{
				PawnGroupKindWorker.pawnsBeingGeneratedNow.Remove(list);
			}
			return list;
		}

		// Token: 0x060098DD RID: 39133
		protected abstract void GeneratePawns(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, List<Pawn> outPawns, bool errorOnZeroResults = true);

		// Token: 0x060098DE RID: 39134 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanGenerateFrom(PawnGroupMakerParms parms, PawnGroupMaker groupMaker)
		{
			return true;
		}

		// Token: 0x060098DF RID: 39135
		public abstract IEnumerable<PawnKindDef> GeneratePawnKindsExample(PawnGroupMakerParms parms, PawnGroupMaker groupMaker);

		// Token: 0x040067B4 RID: 26548
		public PawnGroupKindDef def;

		// Token: 0x040067B5 RID: 26549
		public static List<List<Pawn>> pawnsBeingGeneratedNow = new List<List<Pawn>>();
	}
}
