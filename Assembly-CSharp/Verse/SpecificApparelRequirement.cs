using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002C1 RID: 705
	public class SpecificApparelRequirement
	{
		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06001137 RID: 4407 RVA: 0x00050FA9 File Offset: 0x0004F1A9
		public BodyPartGroupDef BodyPartGroup
		{
			get
			{
				return this.bodyPartGroup;
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06001138 RID: 4408 RVA: 0x00050FB1 File Offset: 0x0004F1B1
		public ApparelLayerDef ApparelLayer
		{
			get
			{
				return this.apparelLayer;
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06001139 RID: 4409 RVA: 0x00050FB9 File Offset: 0x0004F1B9
		public ThingDef ApparelDef
		{
			get
			{
				return this.apparelDef;
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x0600113A RID: 4410 RVA: 0x00050FC1 File Offset: 0x0004F1C1
		public string RequiredTag
		{
			get
			{
				return this.requiredTag;
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x0600113B RID: 4411 RVA: 0x00050FC9 File Offset: 0x0004F1C9
		public List<SpecificApparelRequirement.TagChance> AlternateTagChoices
		{
			get
			{
				return this.alternateTagChoices;
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x0600113C RID: 4412 RVA: 0x00050FD1 File Offset: 0x0004F1D1
		public ThingDef Stuff
		{
			get
			{
				return this.stuff;
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x0600113D RID: 4413 RVA: 0x00050FD9 File Offset: 0x0004F1D9
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x0600113E RID: 4414 RVA: 0x00050FE1 File Offset: 0x0004F1E1
		public ColorGenerator ColorGenerator
		{
			get
			{
				return this.colorGenerator;
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x0600113F RID: 4415 RVA: 0x00050FE9 File Offset: 0x0004F1E9
		public bool Locked
		{
			get
			{
				return this.locked;
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06001140 RID: 4416 RVA: 0x00050FF1 File Offset: 0x0004F1F1
		public bool Biocode
		{
			get
			{
				return this.biocode;
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06001141 RID: 4417 RVA: 0x00050FF9 File Offset: 0x0004F1F9
		public ThingStyleDef StyleDef
		{
			get
			{
				return this.styleDef;
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06001142 RID: 4418 RVA: 0x00051001 File Offset: 0x0004F201
		public QualityCategory? Quality
		{
			get
			{
				return this.quality;
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06001143 RID: 4419 RVA: 0x00051009 File Offset: 0x0004F209
		public bool UseRandomStyleDef
		{
			get
			{
				return this.useRandomStyleDef;
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06001144 RID: 4420 RVA: 0x00051011 File Offset: 0x0004F211
		public bool IgnoreNaked
		{
			get
			{
				return this.ignoreNaked;
			}
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x0005101C File Offset: 0x0004F21C
		public Color GetColor()
		{
			if (this.color != default(Color))
			{
				return this.color;
			}
			if (this.colorGenerator != null)
			{
				return this.colorGenerator.NewRandomizedColor();
			}
			return default(Color);
		}

		// Token: 0x04000F55 RID: 3925
		private BodyPartGroupDef bodyPartGroup;

		// Token: 0x04000F56 RID: 3926
		private ApparelLayerDef apparelLayer;

		// Token: 0x04000F57 RID: 3927
		private ThingDef apparelDef;

		// Token: 0x04000F58 RID: 3928
		private string requiredTag;

		// Token: 0x04000F59 RID: 3929
		private List<SpecificApparelRequirement.TagChance> alternateTagChoices;

		// Token: 0x04000F5A RID: 3930
		private ThingDef stuff;

		// Token: 0x04000F5B RID: 3931
		private ThingStyleDef styleDef;

		// Token: 0x04000F5C RID: 3932
		private Color color;

		// Token: 0x04000F5D RID: 3933
		private ColorGenerator colorGenerator;

		// Token: 0x04000F5E RID: 3934
		private bool locked;

		// Token: 0x04000F5F RID: 3935
		private bool biocode;

		// Token: 0x04000F60 RID: 3936
		private QualityCategory? quality;

		// Token: 0x04000F61 RID: 3937
		private bool useRandomStyleDef;

		// Token: 0x04000F62 RID: 3938
		private bool ignoreNaked;

		// Token: 0x020002C2 RID: 706
		public struct TagChance
		{
			// Token: 0x04000F63 RID: 3939
			public string tag;

			// Token: 0x04000F64 RID: 3940
			public float chance;
		}
	}
}
