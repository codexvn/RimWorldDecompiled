using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CCF RID: 11471
	public class CompProperties_Hackable : CompProperties
	{
		// Token: 0x060101A8 RID: 65960 RVA: 0x004A87F4 File Offset: 0x004A69F4
		public CompProperties_Hackable()
		{
			this.compClass = typeof(CompHackable);
		}

		// Token: 0x0400A83D RID: 43069
		public float defence;

		// Token: 0x0400A83E RID: 43070
		public EffecterDef effectHacking;

		// Token: 0x0400A83F RID: 43071
		public QuestScriptDef completedQuest;

		// Token: 0x0400A840 RID: 43072
		public bool glowIfHacked;

		// Token: 0x0400A841 RID: 43073
		public SoundDef hackingCompletedSound;

		// Token: 0x0400A842 RID: 43074
		public IntRange lockoutDurationHoursRange = IntRange.Zero;

		// Token: 0x0400A843 RID: 43075
		public bool showProgressAfterHackCompletion = true;

		// Token: 0x0400A844 RID: 43076
		public bool lockoutPermanently;

		// Token: 0x0400A845 RID: 43077
		public bool onlyRemotelyHackable;

		// Token: 0x0400A846 RID: 43078
		public int intellectualSkillPrerequisite;

		// Token: 0x0400A847 RID: 43079
		public List<ThingDefCountClass> dropOnHacked = new List<ThingDefCountClass>();

		// Token: 0x0400A848 RID: 43080
		public List<ThingDefCountClass> dropOnDestroyed = new List<ThingDefCountClass>();

		// Token: 0x0400A849 RID: 43081
		[MustTranslate]
		public string notHackedInspectString;

		// Token: 0x0400A84A RID: 43082
		[MustTranslate]
		public string hackedInspectString;

		// Token: 0x0400A84B RID: 43083
		[MustTranslate]
		public string hackedMessage;

		// Token: 0x0400A84C RID: 43084
		[MustTranslate]
		public string autohackWarningString;

		// Token: 0x0400A84D RID: 43085
		[MustTranslate]
		public string hackedLetterLabel;

		// Token: 0x0400A84E RID: 43086
		[MustTranslate]
		public string hackedLetterText;

		// Token: 0x0400A84F RID: 43087
		public LetterDef hackedLetterDef;

		// Token: 0x0400A850 RID: 43088
		[MustTranslate]
		public string destroyedLetterLabel;

		// Token: 0x0400A851 RID: 43089
		[MustTranslate]
		public string destroyedLetterText;

		// Token: 0x0400A852 RID: 43090
		public LetterDef destroyedLetterDef;

		// Token: 0x0400A853 RID: 43091
		public GraphicData unhackedGraphicData;

		// Token: 0x0400A854 RID: 43092
		public GraphicData hackedGraphicData;

		// Token: 0x0400A855 RID: 43093
		public Vector3 unhackedGraphicOffset;

		// Token: 0x0400A856 RID: 43094
		public Vector3 hackedGraphicOffset;

		// Token: 0x0400A857 RID: 43095
		public DrawerType graphicDrawerType = DrawerType.MapMeshOnly;
	}
}
