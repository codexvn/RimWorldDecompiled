using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002771 RID: 10097
	public class RitualOutcomeEffectWorker_ConnectToTree : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x170021E4 RID: 8676
		// (get) Token: 0x0600DD79 RID: 56697 RVA: 0x00002501 File Offset: 0x00000701
		public override bool SupportsAttachableOutcomeEffect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600DD7A RID: 56698 RVA: 0x00411501 File Offset: 0x0040F701
		public RitualOutcomeEffectWorker_ConnectToTree()
		{
		}

		// Token: 0x0600DD7B RID: 56699 RVA: 0x00411509 File Offset: 0x0040F709
		public RitualOutcomeEffectWorker_ConnectToTree(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x0600DD7C RID: 56700 RVA: 0x004132D8 File Offset: 0x004114D8
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			Thing thing = jobRitual.selectedTarget.Thing;
			float quality = base.GetQuality(jobRitual, progress);
			int num = Mathf.Max(1, Mathf.RoundToInt(quality * 50f));
			CompSpawnSubplantDuration compSpawnSubplantDuration = thing.TryGetComp<CompSpawnSubplantDuration>();
			if (compSpawnSubplantDuration != null)
			{
				foreach (Pawn pawn in totalPresence.Keys)
				{
					for (int i = 0; i < num; i++)
					{
						compSpawnSubplantDuration.DoGrowSubplant(true);
					}
				}
				compSpawnSubplantDuration.SetupNextSubplantTick();
			}
			Pawn pawn2 = jobRitual.PawnWithRole("connector");
			CompTreeConnection compTreeConnection = thing.TryGetComp<CompTreeConnection>();
			if (pawn2 != null && compTreeConnection != null)
			{
				compTreeConnection.ConnectToPawn(pawn2, quality);
				Find.LetterStack.ReceiveLetter("LetterLabelPawnConnected".Translate(thing.Named("TREE")), "LetterTextPawnConnected".Translate(thing.Named("TREE"), pawn2.Named("CONNECTOR")), LetterDefOf.RitualOutcomePositive, pawn2, null, null, new List<ThingDef>
				{
					thing.def
				}, null, 0, true);
			}
		}

		// Token: 0x04009549 RID: 38217
		private const float NumMossPerQuality = 50f;
	}
}
