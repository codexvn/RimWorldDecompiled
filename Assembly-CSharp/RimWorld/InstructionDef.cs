using System;
using System.Collections.Generic;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02001B26 RID: 6950
	public class InstructionDef : Def
	{
		// Token: 0x170017E0 RID: 6112
		// (get) Token: 0x06009B3C RID: 39740 RVA: 0x002D0F2F File Offset: 0x002CF12F
		public string Text
		{
			get
			{
				if (!SteamDeck.IsSteamDeckInNonKeyboardMode || this.textController.NullOrEmpty())
				{
					return this.text;
				}
				return this.textController;
			}
		}

		// Token: 0x06009B3D RID: 39741 RVA: 0x002D0F52 File Offset: 0x002CF152
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.instructionClass == null)
			{
				yield return "no instruction class";
			}
			if (this.text.NullOrEmpty())
			{
				yield return "no text";
			}
			if (this.eventTagInitiate.NullOrEmpty())
			{
				yield return "no eventTagInitiate";
			}
			InstructionDef.tmpParseErrors.Clear();
			this.text.AdjustedForKeys(InstructionDef.tmpParseErrors, false);
			int num;
			for (int i = 0; i < InstructionDef.tmpParseErrors.Count; i = num + 1)
			{
				yield return "text error: " + InstructionDef.tmpParseErrors[i];
				num = i;
			}
			if (!this.textController.NullOrEmpty())
			{
				InstructionDef.tmpParseErrors.Clear();
				this.textController.AdjustedForKeys(InstructionDef.tmpParseErrors, false);
				for (int i = 0; i < InstructionDef.tmpParseErrors.Count; i = num + 1)
				{
					yield return "text error (controller): " + InstructionDef.tmpParseErrors[i];
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x04006B84 RID: 27524
		public Type instructionClass = typeof(Instruction_Basic);

		// Token: 0x04006B85 RID: 27525
		[MustTranslate]
		public string text;

		// Token: 0x04006B86 RID: 27526
		[MustTranslate]
		public string textController;

		// Token: 0x04006B87 RID: 27527
		public bool startCentered;

		// Token: 0x04006B88 RID: 27528
		public bool tutorialModeOnly = true;

		// Token: 0x04006B89 RID: 27529
		[NoTranslate]
		public string eventTagInitiate;

		// Token: 0x04006B8A RID: 27530
		public InstructionDef eventTagInitiateSource;

		// Token: 0x04006B8B RID: 27531
		[NoTranslate]
		public List<string> eventTagsEnd;

		// Token: 0x04006B8C RID: 27532
		[NoTranslate]
		public List<string> actionTagsAllowed;

		// Token: 0x04006B8D RID: 27533
		[MustTranslate]
		public string rejectInputMessage;

		// Token: 0x04006B8E RID: 27534
		public ConceptDef concept;

		// Token: 0x04006B8F RID: 27535
		[NoTranslate]
		public List<string> highlightTags;

		// Token: 0x04006B90 RID: 27536
		[MustTranslate]
		public string onMapInstruction;

		// Token: 0x04006B91 RID: 27537
		public int targetCount;

		// Token: 0x04006B92 RID: 27538
		public ThingDef thingDef;

		// Token: 0x04006B93 RID: 27539
		public RecipeDef recipeDef;

		// Token: 0x04006B94 RID: 27540
		public int recipeTargetCount = 1;

		// Token: 0x04006B95 RID: 27541
		public ThingDef giveOnActivateDef;

		// Token: 0x04006B96 RID: 27542
		public int giveOnActivateCount;

		// Token: 0x04006B97 RID: 27543
		public bool endTutorial;

		// Token: 0x04006B98 RID: 27544
		public bool resetBuildDesignatorStuffs;

		// Token: 0x04006B99 RID: 27545
		private static List<string> tmpParseErrors = new List<string>();
	}
}
