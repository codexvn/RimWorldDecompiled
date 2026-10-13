using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A9B RID: 6811
	public class MonolithLevelDef : Def
	{
		// Token: 0x1700175C RID: 5980
		// (get) Token: 0x06009882 RID: 39042 RVA: 0x002C7E8F File Offset: 0x002C608F
		public Texture2D UIIcon
		{
			get
			{
				return this.uiIcon;
			}
		}

		// Token: 0x06009883 RID: 39043 RVA: 0x002C7E97 File Offset: 0x002C6097
		public override void PostLoad()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (!this.uiIconPath.NullOrEmpty())
				{
					this.uiIcon = ContentFinder<Texture2D>.Get(this.uiIconPath, true);
				}
			});
		}

		// Token: 0x06009884 RID: 39044 RVA: 0x002C7EAA File Offset: 0x002C60AA
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.entityCatagoryCompletionRequired != null && this.entityCountCompletionRequired <= 0)
			{
				yield return "entityCatagoryCompletionRequired is set but entityCountCompletionRequired is not";
			}
			yield break;
			yield break;
		}

		// Token: 0x040066B9 RID: 26297
		public int level;

		// Token: 0x040066BA RID: 26298
		public int graphicIndex;

		// Token: 0x040066BB RID: 26299
		public bool monolithGlows;

		// Token: 0x040066BC RID: 26300
		public int anomalyThreatTier;

		// Token: 0x040066BD RID: 26301
		public bool useInactiveAnomalyThreatFraction;

		// Token: 0x040066BE RID: 26302
		public bool useActiveAnomalyThreatFraction;

		// Token: 0x040066BF RID: 26303
		public float anomalyThreatFractionFactor = 1f;

		// Token: 0x040066C0 RID: 26304
		public float anomalyThreatFraction;

		// Token: 0x040066C1 RID: 26305
		public List<IncidentDef> incidentsOnReached;

		// Token: 0x040066C2 RID: 26306
		public bool triggersGrayPall;

		// Token: 0x040066C3 RID: 26307
		public List<GameConditionDef> unreachableDuringConditions;

		// Token: 0x040066C4 RID: 26308
		public bool advanceThroughActivation;

		// Token: 0x040066C5 RID: 26309
		public int desiredHarbingerTreeCount;

		// Token: 0x040066C6 RID: 26310
		public bool postEndgame;

		// Token: 0x040066C7 RID: 26311
		public int monolithGlowRadiusOverride = -1;

		// Token: 0x040066C8 RID: 26312
		public KnowledgeCategoryDef monolithStudyCategory;

		// Token: 0x040066C9 RID: 26313
		public EntityCategoryDef entityCatagoryCompletionRequired;

		// Token: 0x040066CA RID: 26314
		public int entityCountCompletionRequired;

		// Token: 0x040066CB RID: 26315
		public float anomalyMentalBreakChance;

		// Token: 0x040066CC RID: 26316
		public SoundDef activateSound;

		// Token: 0x040066CD RID: 26317
		public SoundDef activatedSound;

		// Token: 0x040066CE RID: 26318
		public List<MonolithAttachment> attachments;

		// Token: 0x040066CF RID: 26319
		public IntVec2? sizeIncludingAttachments;

		// Token: 0x040066D0 RID: 26320
		public string uiIconPath;

		// Token: 0x040066D1 RID: 26321
		[MustTranslate]
		public string monolithLabel;

		// Token: 0x040066D2 RID: 26322
		[MustTranslate]
		public string monolithDescription;

		// Token: 0x040066D3 RID: 26323
		[MustTranslate]
		public string levelInspectText;

		// Token: 0x040066D4 RID: 26324
		[MustTranslate]
		public string extraQuestDescription;

		// Token: 0x040066D5 RID: 26325
		[MustTranslate]
		public string activateGizmoText;

		// Token: 0x040066D6 RID: 26326
		[MustTranslate]
		public string activateFloatMenuText;

		// Token: 0x040066D7 RID: 26327
		[MustTranslate]
		public string activateGizmoDescription;

		// Token: 0x040066D8 RID: 26328
		[MustTranslate]
		public string pawnSentToActivateMessage;

		// Token: 0x040066D9 RID: 26329
		[MustTranslate]
		public string monolithCanBeActivatedText;

		// Token: 0x040066DA RID: 26330
		[MustTranslate]
		public string activateQuestText;

		// Token: 0x040066DB RID: 26331
		[MustTranslate]
		public string activatableLetterLabel;

		// Token: 0x040066DC RID: 26332
		[MustTranslate]
		public string activatableLetterText;

		// Token: 0x040066DD RID: 26333
		[MustTranslate]
		public string activatedLetterText;

		// Token: 0x040066DE RID: 26334
		private Texture2D uiIcon;
	}
}
