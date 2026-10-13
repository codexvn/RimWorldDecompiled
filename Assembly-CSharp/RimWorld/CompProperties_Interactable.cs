using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CF8 RID: 11512
	public class CompProperties_Interactable : CompProperties
	{
		// Token: 0x060102AD RID: 66221 RVA: 0x004AD614 File Offset: 0x004AB814
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.compClass != null && !typeof(CompInteractable).IsAssignableFrom(this.compClass))
			{
				yield return parentDef.defName + " has compClass but is not subclass of CompInteractable.";
			}
			if (this.activateTexPath.NullOrEmpty())
			{
				yield return parentDef.defName + " has no activate texture.";
			}
			yield break;
			yield break;
		}

		// Token: 0x0400A8D7 RID: 43223
		public int cooldownTicks;

		// Token: 0x0400A8D8 RID: 43224
		public int activeTicks;

		// Token: 0x0400A8D9 RID: 43225
		public int ticksToActivate;

		// Token: 0x0400A8DA RID: 43226
		public int cooldownFleckSpawnIntervalTicks;

		// Token: 0x0400A8DB RID: 43227
		public float cooldownFleckScale = 1f;

		// Token: 0x0400A8DC RID: 43228
		public bool cooldownPreventsRefuel;

		// Token: 0x0400A8DD RID: 43229
		public bool maintainProgress;

		// Token: 0x0400A8DE RID: 43230
		public bool showMustBeActivatedByColonist = true;

		// Token: 0x0400A8DF RID: 43231
		public bool forceNormalSpeedOnInteracting;

		// Token: 0x0400A8E0 RID: 43232
		public bool remainingSecondsInInspectString;

		// Token: 0x0400A8E1 RID: 43233
		public bool ignoreForbidden;

		// Token: 0x0400A8E2 RID: 43234
		public bool requiresPower;

		// Token: 0x0400A8E3 RID: 43235
		public TargetingParameters targetingParameters;

		// Token: 0x0400A8E4 RID: 43236
		public StatDef activateStat;

		// Token: 0x0400A8E5 RID: 43237
		public FleckDef fleckOnUsed;

		// Token: 0x0400A8E6 RID: 43238
		public float fleckOnUsedScale;

		// Token: 0x0400A8E7 RID: 43239
		public FleckDef cooldownFleck;

		// Token: 0x0400A8E8 RID: 43240
		public SoundDef soundActivate;

		// Token: 0x0400A8E9 RID: 43241
		public EffecterDef interactionEffecter;

		// Token: 0x0400A8EA RID: 43242
		[MustTranslate]
		public string jobString;

		// Token: 0x0400A8EB RID: 43243
		[MustTranslate]
		public string activatingStringPending;

		// Token: 0x0400A8EC RID: 43244
		[MustTranslate]
		public string activatingString;

		// Token: 0x0400A8ED RID: 43245
		[MustTranslate]
		public string activateLabelString;

		// Token: 0x0400A8EE RID: 43246
		[MustTranslate]
		public string activateDescString;

		// Token: 0x0400A8EF RID: 43247
		[MustTranslate]
		public string guiLabelString;

		// Token: 0x0400A8F0 RID: 43248
		[MustTranslate]
		public string onCooldownString;

		// Token: 0x0400A8F1 RID: 43249
		[MustTranslate]
		public string inspectString;

		// Token: 0x0400A8F2 RID: 43250
		[MustTranslate]
		public string messageCompletedString;

		// Token: 0x0400A8F3 RID: 43251
		[MustTranslate]
		public string messageCooldownEnded;

		// Token: 0x0400A8F4 RID: 43252
		[NoTranslate]
		public string activateTexPath;
	}
}
