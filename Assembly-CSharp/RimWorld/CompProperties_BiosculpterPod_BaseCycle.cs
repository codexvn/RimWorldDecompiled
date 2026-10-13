using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C1C RID: 11292
	public abstract class CompProperties_BiosculpterPod_BaseCycle : CompProperties
	{
		// Token: 0x170027D5 RID: 10197
		// (get) Token: 0x0600FD46 RID: 64838 RVA: 0x00497A96 File Offset: 0x00495C96
		public Texture2D Icon
		{
			get
			{
				if (this.icon == null)
				{
					this.icon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				}
				return this.icon;
			}
		}

		// Token: 0x170027D6 RID: 10198
		// (get) Token: 0x0600FD47 RID: 64839 RVA: 0x00497ABE File Offset: 0x00495CBE
		public string LabelCap
		{
			get
			{
				return this.label.CapitalizeFirst();
			}
		}

		// Token: 0x0400A601 RID: 42497
		[NoTranslate]
		public string key;

		// Token: 0x0400A602 RID: 42498
		[MustTranslate]
		public string label;

		// Token: 0x0400A603 RID: 42499
		[MustTranslate]
		public string description;

		// Token: 0x0400A604 RID: 42500
		[NoTranslate]
		public string iconPath;

		// Token: 0x0400A605 RID: 42501
		public float durationDays;

		// Token: 0x0400A606 RID: 42502
		public Color operatingColor = new Color(0.5f, 0.7f, 0.5f);

		// Token: 0x0400A607 RID: 42503
		public ThoughtDef gainThoughtOnCompletion;

		// Token: 0x0400A608 RID: 42504
		public List<ResearchProjectDef> requiredResearch;

		// Token: 0x0400A609 RID: 42505
		public List<ThingDefCountClass> extraRequiredIngredients;

		// Token: 0x0400A60A RID: 42506
		private Texture2D icon;
	}
}
