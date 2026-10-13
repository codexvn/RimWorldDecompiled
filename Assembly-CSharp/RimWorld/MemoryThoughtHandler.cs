using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200252D RID: 9517
	public sealed class MemoryThoughtHandler : IExposable
	{
		// Token: 0x17001FF4 RID: 8180
		// (get) Token: 0x0600D026 RID: 53286 RVA: 0x003D366E File Offset: 0x003D186E
		public List<Thought_Memory> Memories
		{
			get
			{
				return this.memories;
			}
		}

		// Token: 0x0600D027 RID: 53287 RVA: 0x003D3676 File Offset: 0x003D1876
		public MemoryThoughtHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600D028 RID: 53288 RVA: 0x003D369C File Offset: 0x003D189C
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.tmpMemories.Clear();
				for (int i = this.memories.Count - 1; i >= 0; i--)
				{
					if (!this.memories[i].Save)
					{
						this.tmpMemories.Add(this.memories[i]);
						this.memories.Remove(this.memories[i]);
					}
				}
			}
			Scribe_Collections.Look<Thought_Memory>(ref this.memories, "memories", LookMode.Deep, Array.Empty<object>());
			foreach (Thought_Memory item in this.tmpMemories)
			{
				this.memories.Add(item);
			}
			this.tmpMemories.Clear();
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int j = this.memories.Count - 1; j >= 0; j--)
				{
					if (this.memories[j].def == null)
					{
						this.memories.RemoveAt(j);
					}
					else
					{
						this.memories[j].pawn = this.pawn;
						if (this.memories[j].permanent)
						{
							bool flag = false;
							foreach (Hediff hd in this.pawn.health.hediffSet.hediffs)
							{
								HediffComp_ThoughtSetter hediffComp_ThoughtSetter = hd.TryGetComp<HediffComp_ThoughtSetter>();
								if (hediffComp_ThoughtSetter != null && hediffComp_ThoughtSetter.Props.thought == this.memories[j].def)
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								this.memories[j].permanent = false;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600D029 RID: 53289 RVA: 0x003D3890 File Offset: 0x003D1A90
		public void MemoryThoughtInterval()
		{
			for (int i = 0; i < this.memories.Count; i++)
			{
				this.memories[i].ThoughtInterval();
			}
			this.RemoveExpiredMemories();
		}

		// Token: 0x0600D02A RID: 53290 RVA: 0x003D38CC File Offset: 0x003D1ACC
		private void RemoveExpiredMemories()
		{
			for (int i = this.memories.Count - 1; i >= 0; i--)
			{
				Thought_Memory thought_Memory = this.memories[i];
				if (thought_Memory.ShouldDiscard)
				{
					this.RemoveMemory(thought_Memory);
					if (thought_Memory.def.nextThought != null)
					{
						this.TryGainMemory(thought_Memory.def.nextThought, null, null);
					}
				}
			}
		}

		// Token: 0x0600D02B RID: 53291 RVA: 0x003D3930 File Offset: 0x003D1B30
		public void TryGainMemoryFast(ThoughtDef mem, Precept sourcePrecept = null)
		{
			Thought_Memory firstMemoryOfDef = this.GetFirstMemoryOfDef(mem);
			if (firstMemoryOfDef != null)
			{
				firstMemoryOfDef.Renew();
				return;
			}
			this.TryGainMemory(mem, null, sourcePrecept);
		}

		// Token: 0x0600D02C RID: 53292 RVA: 0x003D3958 File Offset: 0x003D1B58
		public void TryGainMemoryFast(ThoughtDef mem, int stage, Precept sourcePrecept = null)
		{
			Thought_Memory firstMemoryOfDef = this.GetFirstMemoryOfDef(mem);
			if (firstMemoryOfDef != null)
			{
				firstMemoryOfDef.Renew();
				firstMemoryOfDef.SetForcedStage(stage);
				return;
			}
			this.TryGainMemory(mem, null, sourcePrecept);
			Thought_Memory firstMemoryOfDef2 = this.GetFirstMemoryOfDef(mem);
			if (firstMemoryOfDef2 == null)
			{
				return;
			}
			firstMemoryOfDef2.SetForcedStage(stage);
		}

		// Token: 0x0600D02D RID: 53293 RVA: 0x003D3999 File Offset: 0x003D1B99
		public void TryGainMemory(ThoughtDef def, Pawn otherPawn = null, Precept sourcePrecept = null)
		{
			if (!def.IsMemory)
			{
				Log.Error(((def != null) ? def.ToString() : null) + " is not a memory thought.");
				return;
			}
			this.TryGainMemory(ThoughtMaker.MakeThought(def, sourcePrecept), otherPawn);
		}

		// Token: 0x0600D02E RID: 53294 RVA: 0x003D39D0 File Offset: 0x003D1BD0
		public void TryGainMemory(Thought_Memory newThought, Pawn otherPawn = null)
		{
			if (!ThoughtUtility.CanGetThought(this.pawn, newThought.def, false))
			{
				return;
			}
			if (newThought is Thought_MemorySocial)
			{
				if (newThought.otherPawn == null && otherPawn == null)
				{
					string str = "Can't gain social thought ";
					ThoughtDef def = newThought.def;
					Log.Error(str + ((def != null) ? def.ToString() : null) + " because its otherPawn is null and otherPawn passed to this method is also null. Social thoughts must have otherPawn.");
					return;
				}
				otherPawn = (otherPawn ?? newThought.otherPawn);
				if (!newThought.def.socialTargetDevelopmentalStageFilter.Has(otherPawn.DevelopmentalStage))
				{
					return;
				}
			}
			newThought.pawn = this.pawn;
			newThought.otherPawn = otherPawn;
			bool flag;
			if (!newThought.TryMergeWithExistingMemory(out flag))
			{
				this.memories.Add(newThought);
			}
			if (newThought.def.stackLimitForSameOtherPawn >= 0)
			{
				while (this.NumMemoriesInGroup(newThought) > newThought.def.stackLimitForSameOtherPawn)
				{
					this.RemoveMemory(this.OldestMemoryInGroup(newThought));
				}
			}
			if (newThought.def.stackLimit >= 0)
			{
				while (this.NumMemoriesOfDef(newThought.def) > newThought.def.stackLimit)
				{
					this.RemoveMemory(this.OldestMemoryOfDef(newThought.def));
				}
			}
			if (newThought.def.thoughtToMake != null)
			{
				this.TryGainMemory(newThought.def.thoughtToMake, newThought.otherPawn, null);
			}
			List<Thought_Memory> list = (newThought.def.replaceThoughts == null) ? this.memories : new List<Thought_Memory>(this.memories);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] != newThought && list[i].GroupsWith(newThought))
				{
					list[i].Notify_NewThoughtInGroupAdded(newThought);
				}
				if (newThought.def.replaceThoughts != null && newThought.def.replaceThoughts.Contains(list[i].def))
				{
					this.RemoveMemory(list[i]);
				}
			}
			if (flag && newThought.def.showBubble && this.pawn.Spawned && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				MoteMaker.MakeMoodThoughtBubble(this.pawn, newThought);
			}
		}

		// Token: 0x0600D02F RID: 53295 RVA: 0x003D3BD4 File Offset: 0x003D1DD4
		public Thought_Memory OldestMemoryInGroup(Thought_Memory group)
		{
			Thought_Memory result = null;
			int num = -9999;
			for (int i = 0; i < this.memories.Count; i++)
			{
				Thought_Memory thought_Memory = this.memories[i];
				if (thought_Memory.GroupsWith(group) && thought_Memory.age > num)
				{
					result = thought_Memory;
					num = thought_Memory.age;
				}
			}
			return result;
		}

		// Token: 0x0600D030 RID: 53296 RVA: 0x003D3C28 File Offset: 0x003D1E28
		public Thought_Memory OldestMemoryOfDef(ThoughtDef def)
		{
			Thought_Memory result = null;
			int num = -9999;
			for (int i = 0; i < this.memories.Count; i++)
			{
				Thought_Memory thought_Memory = this.memories[i];
				if (thought_Memory.def == def && thought_Memory.age > num)
				{
					result = thought_Memory;
					num = thought_Memory.age;
				}
			}
			return result;
		}

		// Token: 0x0600D031 RID: 53297 RVA: 0x003D3C7C File Offset: 0x003D1E7C
		public void RemoveMemory(Thought_Memory th)
		{
			if (!this.memories.Remove(th))
			{
				Log.Warning("Tried to remove memory thought of def " + th.def.defName + " but it's not here.");
			}
		}

		// Token: 0x0600D032 RID: 53298 RVA: 0x003D3CAC File Offset: 0x003D1EAC
		public int NumMemoriesInGroup(Thought_Memory group)
		{
			int num = 0;
			for (int i = 0; i < this.memories.Count; i++)
			{
				if (this.memories[i].GroupsWith(group))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600D033 RID: 53299 RVA: 0x003D3CEC File Offset: 0x003D1EEC
		public int NumMemoriesOfDef(ThoughtDef def)
		{
			int num = 0;
			for (int i = 0; i < this.memories.Count; i++)
			{
				if (this.memories[i].def == def)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600D034 RID: 53300 RVA: 0x003D3D2C File Offset: 0x003D1F2C
		public Thought_Memory GetFirstMemoryOfDef(ThoughtDef def)
		{
			for (int i = 0; i < this.memories.Count; i++)
			{
				if (this.memories[i].def == def)
				{
					return this.memories[i];
				}
			}
			return null;
		}

		// Token: 0x0600D035 RID: 53301 RVA: 0x003D3D74 File Offset: 0x003D1F74
		public void RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDef def, Pawn otherPawn)
		{
			for (int i = this.memories.Count - 1; i >= 0; i--)
			{
				Thought_Memory thought_Memory = this.memories[i];
				if (thought_Memory.def == def && thought_Memory.otherPawn == otherPawn)
				{
					this.RemoveMemory(thought_Memory);
				}
			}
		}

		// Token: 0x0600D036 RID: 53302 RVA: 0x003D3DC0 File Offset: 0x003D1FC0
		public void RemoveMemoriesWhereOtherPawnIs(Pawn otherPawn)
		{
			Predicate<Thought_Memory> <>9__0;
			for (;;)
			{
				List<Thought_Memory> list = this.memories;
				Predicate<Thought_Memory> match;
				if ((match = <>9__0) == null)
				{
					match = (<>9__0 = ((Thought_Memory x) => x.otherPawn == otherPawn));
				}
				Thought_Memory thought_Memory = list.Find(match);
				if (thought_Memory == null)
				{
					break;
				}
				this.RemoveMemory(thought_Memory);
			}
		}

		// Token: 0x0600D037 RID: 53303 RVA: 0x003D3E14 File Offset: 0x003D2014
		public void RemoveMemoriesOfDef(ThoughtDef def)
		{
			if (!def.IsMemory)
			{
				ThoughtDef def2 = def;
				Log.Warning(((def2 != null) ? def2.ToString() : null) + " is not a memory thought.");
				return;
			}
			Predicate<Thought_Memory> <>9__0;
			for (;;)
			{
				List<Thought_Memory> list = this.memories;
				Predicate<Thought_Memory> match;
				if ((match = <>9__0) == null)
				{
					match = (<>9__0 = ((Thought_Memory x) => x.def == def));
				}
				Thought_Memory thought_Memory = list.Find(match);
				if (thought_Memory == null)
				{
					break;
				}
				this.RemoveMemory(thought_Memory);
			}
		}

		// Token: 0x0600D038 RID: 53304 RVA: 0x003D3E94 File Offset: 0x003D2094
		public void RemoveMemoriesOfDefIf(ThoughtDef def, Func<Thought_Memory, bool> predicate)
		{
			if (!def.IsMemory)
			{
				ThoughtDef def2 = def;
				Log.Warning(((def2 != null) ? def2.ToString() : null) + " is not a memory thought.");
				return;
			}
			Predicate<Thought_Memory> <>9__0;
			for (;;)
			{
				List<Thought_Memory> list = this.memories;
				Predicate<Thought_Memory> match;
				if ((match = <>9__0) == null)
				{
					match = (<>9__0 = ((Thought_Memory x) => x.def == def && predicate(x)));
				}
				Thought_Memory thought_Memory = list.Find(match);
				if (thought_Memory == null)
				{
					break;
				}
				this.RemoveMemory(thought_Memory);
			}
		}

		// Token: 0x0600D039 RID: 53305 RVA: 0x003D3F1C File Offset: 0x003D211C
		public bool AnyMemoryConcerns(Pawn otherPawn)
		{
			for (int i = 0; i < this.memories.Count; i++)
			{
				if (this.memories[i].otherPawn == otherPawn)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600D03A RID: 53306 RVA: 0x003D3F56 File Offset: 0x003D2156
		public void Notify_PawnDiscarded(Pawn discarded)
		{
			this.RemoveMemoriesWhereOtherPawnIs(discarded);
		}

		// Token: 0x04008DA9 RID: 36265
		public Pawn pawn;

		// Token: 0x04008DAA RID: 36266
		private List<Thought_Memory> memories = new List<Thought_Memory>();

		// Token: 0x04008DAB RID: 36267
		private List<Thought_Memory> tmpMemories = new List<Thought_Memory>();
	}
}
