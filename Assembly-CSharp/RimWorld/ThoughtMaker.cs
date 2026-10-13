using System;

namespace RimWorld
{
	// Token: 0x02002536 RID: 9526
	public static class ThoughtMaker
	{
		// Token: 0x0600D064 RID: 53348 RVA: 0x003D4D17 File Offset: 0x003D2F17
		public static Thought MakeThought(ThoughtDef def)
		{
			Thought thought = (Thought)Activator.CreateInstance(def.ThoughtClass);
			thought.def = def;
			thought.Init();
			return thought;
		}

		// Token: 0x0600D065 RID: 53349 RVA: 0x003D4D36 File Offset: 0x003D2F36
		public static Thought_Memory MakeThought(ThoughtDef def, int forcedStage)
		{
			Thought_Memory thought_Memory = (Thought_Memory)Activator.CreateInstance(def.ThoughtClass);
			thought_Memory.def = def;
			thought_Memory.SetForcedStage(forcedStage);
			thought_Memory.Init();
			return thought_Memory;
		}

		// Token: 0x0600D066 RID: 53350 RVA: 0x003D4D5C File Offset: 0x003D2F5C
		public static Thought_Memory MakeThought(ThoughtDef def, Precept sourcePrecept)
		{
			Thought_Memory thought_Memory = (Thought_Memory)Activator.CreateInstance(def.ThoughtClass);
			thought_Memory.def = def;
			thought_Memory.sourcePrecept = sourcePrecept;
			thought_Memory.Init();
			return thought_Memory;
		}
	}
}
