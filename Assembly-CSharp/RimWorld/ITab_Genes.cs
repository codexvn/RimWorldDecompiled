using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200339F RID: 13215
	public class ITab_Genes : ITab
	{
		// Token: 0x17003097 RID: 12439
		// (get) Token: 0x06012931 RID: 76081 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool Hidden
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17003098 RID: 12440
		// (get) Token: 0x06012932 RID: 76082 RVA: 0x0057C785 File Offset: 0x0057A985
		public override bool IsVisible
		{
			get
			{
				return ITab_Genes.CanShowGenesTab();
			}
		}

		// Token: 0x17003099 RID: 12441
		// (get) Token: 0x06012933 RID: 76083 RVA: 0x0057C78C File Offset: 0x0057A98C
		protected override bool StillValid
		{
			get
			{
				return base.StillValid && ITab_Genes.ThingWithGenes() != null;
			}
		}

		// Token: 0x1700309A RID: 12442
		// (get) Token: 0x06012934 RID: 76084 RVA: 0x0057C7A0 File Offset: 0x0057A9A0
		protected Pawn SelPawnForGenes
		{
			get
			{
				return ITab_Genes.PawnForGenes(base.SelThing);
			}
		}

		// Token: 0x06012935 RID: 76085 RVA: 0x0057C7AD File Offset: 0x0057A9AD
		public ITab_Genes()
		{
			this.size = new Vector2(Mathf.Min(736f, (float)UI.screenWidth), 550f);
			this.labelKey = "TabGenes";
		}

		// Token: 0x06012936 RID: 76086 RVA: 0x0057C7E0 File Offset: 0x0057A9E0
		public override void OnOpen()
		{
			if (!ModLister.CheckBiotech("genes viewing"))
			{
				this.CloseTab();
				return;
			}
			base.OnOpen();
		}

		// Token: 0x06012937 RID: 76087 RVA: 0x0057C7FC File Offset: 0x0057A9FC
		protected override void FillTab()
		{
			GeneUIUtility.DrawGenesInfo(new Rect(0f, 20f, this.size.x, this.size.y - 20f), ITab_Genes.ThingWithGenes(), 550f, ref this.size, ref this.scrollPosition, null);
		}

		// Token: 0x06012938 RID: 76088 RVA: 0x0057C850 File Offset: 0x0057AA50
		private static Thing ThingWithGenes()
		{
			Thing singleSelectedThing = Find.Selector.SingleSelectedThing;
			Pawn pawn = ITab_Genes.PawnForGenes(singleSelectedThing);
			if (pawn != null)
			{
				return pawn;
			}
			if (singleSelectedThing is GeneSetHolderBase)
			{
				return singleSelectedThing;
			}
			Building_GrowthVat building_GrowthVat = singleSelectedThing as Building_GrowthVat;
			if (building_GrowthVat != null)
			{
				return building_GrowthVat.selectedEmbryo;
			}
			return null;
		}

		// Token: 0x06012939 RID: 76089 RVA: 0x0057C890 File Offset: 0x0057AA90
		private static Pawn PawnForGenes(Thing thing)
		{
			Pawn pawn = thing as Pawn;
			if (pawn != null && pawn.genes != null)
			{
				return pawn;
			}
			Corpse corpse = thing as Corpse;
			if (corpse != null && corpse.InnerPawn.genes != null)
			{
				return corpse.InnerPawn;
			}
			return null;
		}

		// Token: 0x0601293A RID: 76090 RVA: 0x0057C8D0 File Offset: 0x0057AAD0
		public static bool CanShowGenesTab()
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			Pawn pawn = ITab_Genes.PawnForGenes(Find.Selector.SingleSelectedThing);
			return (pawn != null && pawn.genes != null) || ITab_Genes.ThingWithGenes() != null;
		}

		// Token: 0x0400C0E5 RID: 49381
		protected Vector2 scrollPosition;

		// Token: 0x0400C0E6 RID: 49382
		protected const float TopPadding = 20f;

		// Token: 0x0400C0E7 RID: 49383
		public const float GeneSize = 90f;

		// Token: 0x0400C0E8 RID: 49384
		public const float GeneGap = 6f;

		// Token: 0x0400C0E9 RID: 49385
		public const int MaxGenesHorizontal = 7;

		// Token: 0x0400C0EA RID: 49386
		public const float InitialWidth = 736f;

		// Token: 0x0400C0EB RID: 49387
		protected const float InitialHeight = 550f;
	}
}
