using System;
using System.Collections.Generic;
using System.Reflection;
using RimWorld;

namespace Verse
{
	// Token: 0x02000218 RID: 536
	public class CostListForDifficulty
	{
		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000E33 RID: 3635 RVA: 0x00042E70 File Offset: 0x00041070
		public bool Applies
		{
			get
			{
				if (Find.Storyteller == null)
				{
					return false;
				}
				if (this.cachedDifficulty != Find.Storyteller.difficulty)
				{
					this.RecacheApplies();
				}
				return this.cachedApplies;
			}
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x00042E9C File Offset: 0x0004109C
		public void RecacheApplies()
		{
			this.cachedDifficulty = Find.Storyteller.difficulty;
			if (this.difficultyVar.NullOrEmpty())
			{
				this.cachedApplies = false;
				return;
			}
			FieldInfo field = typeof(Difficulty).GetField(this.difficultyVar, BindingFlags.Instance | BindingFlags.Public);
			this.cachedApplies = (bool)field.GetValue(this.cachedDifficulty);
			if (this.invert)
			{
				this.cachedApplies = !this.cachedApplies;
			}
		}

		// Token: 0x0400088A RID: 2186
		public string difficultyVar;

		// Token: 0x0400088B RID: 2187
		public List<ThingDefCountClass> costList;

		// Token: 0x0400088C RID: 2188
		public int costStuffCount;

		// Token: 0x0400088D RID: 2189
		public bool invert;

		// Token: 0x0400088E RID: 2190
		private bool cachedApplies;

		// Token: 0x0400088F RID: 2191
		private Difficulty cachedDifficulty;
	}
}
