using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000CAE RID: 3246
	public static class DevelopmentalStageExtensions
	{
		// Token: 0x0600532B RID: 21291 RVA: 0x001ACE14 File Offset: 0x001AB014
		public static CachedTexture Icon(this DevelopmentalStage developmentalStage)
		{
			if (!DevelopmentalStageExtensions.ExactlyOneDevelopmentalStageSet(developmentalStage))
			{
				throw new ArgumentException(string.Format("Exactly one developmental stage may be set to get an icon, but was {0}.", developmentalStage));
			}
			if (developmentalStage == DevelopmentalStage.Baby)
			{
				return DevelopmentalStageExtensions.BabyTex;
			}
			if (developmentalStage == DevelopmentalStage.Child)
			{
				return DevelopmentalStageExtensions.ChildTex;
			}
			if (developmentalStage != DevelopmentalStage.Adult)
			{
				throw new NotImplementedException();
			}
			return DevelopmentalStageExtensions.AdultTex;
		}

		// Token: 0x0600532C RID: 21292 RVA: 0x001ACE64 File Offset: 0x001AB064
		public static bool ExactlyOneDevelopmentalStageSet(DevelopmentalStage developmentalStage)
		{
			return developmentalStage != DevelopmentalStage.None && (developmentalStage & developmentalStage - 1U) == DevelopmentalStage.None;
		}

		// Token: 0x0600532D RID: 21293 RVA: 0x001ACE80 File Offset: 0x001AB080
		public static bool Newborn(this DevelopmentalStage developmentalStage)
		{
			return (developmentalStage & DevelopmentalStage.Newborn) > DevelopmentalStage.None;
		}

		// Token: 0x0600532E RID: 21294 RVA: 0x001ACE88 File Offset: 0x001AB088
		public static bool Baby(this DevelopmentalStage developmentalStage)
		{
			return (developmentalStage & DevelopmentalStage.Baby) > DevelopmentalStage.None;
		}

		// Token: 0x0600532F RID: 21295 RVA: 0x001ACE90 File Offset: 0x001AB090
		public static bool Child(this DevelopmentalStage developmentalStage)
		{
			return (developmentalStage & DevelopmentalStage.Child) > DevelopmentalStage.None;
		}

		// Token: 0x06005330 RID: 21296 RVA: 0x001ACE98 File Offset: 0x001AB098
		public static bool Adult(this DevelopmentalStage developmentalStage)
		{
			return (developmentalStage & DevelopmentalStage.Adult) > DevelopmentalStage.None;
		}

		// Token: 0x06005331 RID: 21297 RVA: 0x001ACEA0 File Offset: 0x001AB0A0
		public static bool Juvenile(this DevelopmentalStage developmentalStage)
		{
			return developmentalStage.Baby() || developmentalStage.Child();
		}

		// Token: 0x06005332 RID: 21298 RVA: 0x001ACEB2 File Offset: 0x001AB0B2
		public static bool Has(this DevelopmentalStage developmentalStage, DevelopmentalStage query)
		{
			if (!DevelopmentalStageExtensions.ExactlyOneDevelopmentalStageSet(query))
			{
				Log.ErrorOnce("A single DevelopmentalStage was expected but multiple were set.", 845116642);
			}
			return developmentalStage.HasAny(query);
		}

		// Token: 0x06005333 RID: 21299 RVA: 0x000C57B3 File Offset: 0x000C39B3
		public static bool HasAny(this DevelopmentalStage developmentalStage, DevelopmentalStage query)
		{
			return (developmentalStage & query) > DevelopmentalStage.None;
		}

		// Token: 0x06005334 RID: 21300 RVA: 0x001ACED2 File Offset: 0x001AB0D2
		public static bool HasAll(this DevelopmentalStage developmentalStage, DevelopmentalStage query)
		{
			return (developmentalStage & query) == query;
		}

		// Token: 0x06005335 RID: 21301 RVA: 0x001ACEDC File Offset: 0x001AB0DC
		public static string ToCommaListOr(this DevelopmentalStage developmentalStages)
		{
			DevelopmentalStageExtensions.developmentalStageStrings.Clear();
			foreach (DevelopmentalStage developmentalStage in developmentalStages.Enumerate())
			{
				DevelopmentalStageExtensions.developmentalStageStrings.Add(developmentalStage.ToString().Translate());
			}
			return DevelopmentalStageExtensions.developmentalStageStrings.ToCommaListOr(false);
		}

		// Token: 0x06005336 RID: 21302 RVA: 0x001ACF44 File Offset: 0x001AB144
		public static string ToCommaList(this DevelopmentalStage developmentalStages, bool capitalize = false)
		{
			DevelopmentalStageExtensions.developmentalStageStrings.Clear();
			foreach (DevelopmentalStage developmentalStage in developmentalStages.Enumerate())
			{
				TaggedString taggedString = developmentalStage.ToString().Translate();
				DevelopmentalStageExtensions.developmentalStageStrings.Add(capitalize ? taggedString.CapitalizeFirst() : taggedString);
			}
			return DevelopmentalStageExtensions.developmentalStageStrings.ToCommaList(false, false);
		}

		// Token: 0x06005337 RID: 21303 RVA: 0x001ACFB8 File Offset: 0x001AB1B8
		public static DevelopmentalStageExtensions.EnumerationCodebehind Enumerate(this DevelopmentalStage developmentalStages)
		{
			return new DevelopmentalStageExtensions.EnumerationCodebehind(developmentalStages);
		}

		// Token: 0x04003C16 RID: 15382
		private static List<string> developmentalStageStrings = new List<string>();

		// Token: 0x04003C17 RID: 15383
		private const DevelopmentalStage lastdevelopmentalStage = DevelopmentalStage.Adult;

		// Token: 0x04003C18 RID: 15384
		public static CachedTexture BabyTex = new CachedTexture("UI/Icons/DevelopmentalStages/Baby");

		// Token: 0x04003C19 RID: 15385
		public static CachedTexture ChildTex = new CachedTexture("UI/Icons/DevelopmentalStages/Child");

		// Token: 0x04003C1A RID: 15386
		public static CachedTexture AdultTex = new CachedTexture("UI/Icons/DevelopmentalStages/Adult");

		// Token: 0x02000CAF RID: 3247
		public struct EnumerationCodebehind
		{
			// Token: 0x06005339 RID: 21305 RVA: 0x001ACFF9 File Offset: 0x001AB1F9
			public EnumerationCodebehind(DevelopmentalStage stages)
			{
				this.bit = 0U;
				this.stages = (uint)stages;
			}

			// Token: 0x17000CCD RID: 3277
			// (get) Token: 0x0600533A RID: 21306 RVA: 0x001AD009 File Offset: 0x001AB209
			public DevelopmentalStage Current
			{
				get
				{
					return (DevelopmentalStage)this.bit;
				}
			}

			// Token: 0x0600533B RID: 21307 RVA: 0x001AD014 File Offset: 0x001AB214
			public bool MoveNext()
			{
				if (this.bit == 0U)
				{
					this.bit = 1U;
				}
				else
				{
					this.bit <<= 1;
				}
				while (this.bit <= 8U)
				{
					if ((this.bit & this.stages) != 0U)
					{
						return true;
					}
					this.bit <<= 1;
				}
				return false;
			}

			// Token: 0x0600533C RID: 21308 RVA: 0x001AD06B File Offset: 0x001AB26B
			public DevelopmentalStageExtensions.EnumerationCodebehind GetEnumerator()
			{
				return this;
			}

			// Token: 0x0600533D RID: 21309 RVA: 0x000026EA File Offset: 0x000008EA
			public void Dispose()
			{
			}

			// Token: 0x04003C1B RID: 15387
			private uint bit;

			// Token: 0x04003C1C RID: 15388
			private uint stages;
		}
	}
}
