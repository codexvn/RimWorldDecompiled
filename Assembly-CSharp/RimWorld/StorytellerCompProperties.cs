using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AF3 RID: 6899
	public class StorytellerCompProperties
	{
		// Token: 0x170017A7 RID: 6055
		// (get) Token: 0x06009A2D RID: 39469 RVA: 0x002CDC08 File Offset: 0x002CBE08
		public bool Enabled
		{
			get
			{
				if (!this.enableIfAnyModActive.NullOrEmpty<string>())
				{
					for (int i = 0; i < this.enableIfAnyModActive.Count; i++)
					{
						if (ModsConfig.IsActive(this.enableIfAnyModActive[i]))
						{
							return true;
						}
					}
					return false;
				}
				if (!this.disableIfAnyModActive.NullOrEmpty<string>())
				{
					for (int j = 0; j < this.disableIfAnyModActive.Count; j++)
					{
						if (ModsConfig.IsActive(this.disableIfAnyModActive[j]))
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x06009A2E RID: 39470 RVA: 0x002CDC88 File Offset: 0x002CBE88
		public StorytellerCompProperties()
		{
		}

		// Token: 0x06009A2F RID: 39471 RVA: 0x002CDC9B File Offset: 0x002CBE9B
		public StorytellerCompProperties(Type compClass)
		{
			this.compClass = compClass;
		}

		// Token: 0x06009A30 RID: 39472 RVA: 0x002CDCB5 File Offset: 0x002CBEB5
		public virtual IEnumerable<string> ConfigErrors(StorytellerDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return "a StorytellerCompProperties has null compClass.";
			}
			if (!this.enableIfAnyModActive.NullOrEmpty<string>() && !this.disableIfAnyModActive.NullOrEmpty<string>())
			{
				yield return "enableIfAnyModActive and disableIfAnyModActive can't be used simultaneously";
			}
			yield break;
		}

		// Token: 0x06009A31 RID: 39473 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ResolveReferences(StorytellerDef parentDef)
		{
		}

		// Token: 0x04006A25 RID: 27173
		[TranslationHandle]
		public Type compClass;

		// Token: 0x04006A26 RID: 27174
		public float minDaysPassed;

		// Token: 0x04006A27 RID: 27175
		public List<IncidentTargetTagDef> allowedTargetTags;

		// Token: 0x04006A28 RID: 27176
		public List<IncidentTargetTagDef> disallowedTargetTags;

		// Token: 0x04006A29 RID: 27177
		public float minIncChancePopulationIntentFactor = 0.05f;

		// Token: 0x04006A2A RID: 27178
		public List<string> enableIfAnyModActive;

		// Token: 0x04006A2B RID: 27179
		public List<string> disableIfAnyModActive;
	}
}
