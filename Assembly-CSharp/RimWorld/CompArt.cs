using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BEA RID: 11242
	public class CompArt : ThingComp
	{
		// Token: 0x17002788 RID: 10120
		// (get) Token: 0x0600FBB3 RID: 64435 RVA: 0x004910A4 File Offset: 0x0048F2A4
		public TaggedString AuthorName
		{
			get
			{
				if (this.authorNameInt.NullOrEmpty())
				{
					return "UnknownLower".Translate().CapitalizeFirst();
				}
				return this.authorNameInt.Resolve();
			}
		}

		// Token: 0x17002789 RID: 10121
		// (get) Token: 0x0600FBB4 RID: 64436 RVA: 0x004910E4 File Offset: 0x0048F2E4
		// (set) Token: 0x0600FBB5 RID: 64437 RVA: 0x00491141 File Offset: 0x0048F341
		public string Title
		{
			get
			{
				if (this.parent.StyleSourcePrecept != null)
				{
					return this.parent.StyleSourcePrecept.LabelCap;
				}
				if (this.titleInt.NullOrEmpty())
				{
					Log.Error("CompArt got title but it wasn't configured.");
					this.titleInt = "Error";
				}
				return this.titleInt;
			}
			set
			{
				this.titleInt = value;
			}
		}

		// Token: 0x1700278A RID: 10122
		// (get) Token: 0x0600FBB6 RID: 64438 RVA: 0x0049114F File Offset: 0x0048F34F
		public TaleReference TaleRef
		{
			get
			{
				return this.taleRef;
			}
		}

		// Token: 0x1700278B RID: 10123
		// (get) Token: 0x0600FBB7 RID: 64439 RVA: 0x00491158 File Offset: 0x0048F358
		public bool CanShowArt
		{
			get
			{
				if (this.Props.mustBeFullGrave)
				{
					Building_Grave building_Grave = this.parent as Building_Grave;
					if (building_Grave == null || !building_Grave.HasCorpse)
					{
						return false;
					}
				}
				QualityCategory qualityCategory;
				return !this.parent.TryGetQuality(out qualityCategory) || qualityCategory >= this.Props.minQualityForArtistic;
			}
		}

		// Token: 0x1700278C RID: 10124
		// (get) Token: 0x0600FBB8 RID: 64440 RVA: 0x004911AD File Offset: 0x0048F3AD
		public virtual bool Active
		{
			get
			{
				return this.taleRef != null;
			}
		}

		// Token: 0x1700278D RID: 10125
		// (get) Token: 0x0600FBB9 RID: 64441 RVA: 0x004911B8 File Offset: 0x0048F3B8
		public CompProperties_Art Props
		{
			get
			{
				return (CompProperties_Art)this.props;
			}
		}

		// Token: 0x0600FBBA RID: 64442 RVA: 0x004911C5 File Offset: 0x0048F3C5
		public override string TransformLabel(string label)
		{
			if (this.Active && this.parent.StyleSourcePrecept != null)
			{
				return this.Title;
			}
			return base.TransformLabel(label);
		}

		// Token: 0x0600FBBB RID: 64443 RVA: 0x004911EA File Offset: 0x0048F3EA
		public void InitializeArt(ArtGenerationContext source)
		{
			this.InitializeArtInternal(null, source);
		}

		// Token: 0x0600FBBC RID: 64444 RVA: 0x004911F4 File Offset: 0x0048F3F4
		public void InitializeArt(Thing relatedThing)
		{
			this.InitializeArtInternal(relatedThing, ArtGenerationContext.Colony);
		}

		// Token: 0x0600FBBD RID: 64445 RVA: 0x00491200 File Offset: 0x0048F400
		protected virtual void InitializeArtInternal(Thing relatedThing, ArtGenerationContext source)
		{
			if (!this.titleInt.NullOrEmpty())
			{
				return;
			}
			if (this.taleRef != null)
			{
				this.taleRef.ReferenceDestroyed();
				this.taleRef = null;
			}
			if (this.CanShowArt)
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					if (relatedThing != null)
					{
						this.taleRef = Find.TaleManager.GetRandomTaleReferenceForArtConcerning(relatedThing);
					}
					else
					{
						this.taleRef = Find.TaleManager.GetRandomTaleReferenceForArt(source);
					}
				}
				else
				{
					this.taleRef = TaleReference.Taleless;
				}
				this.titleInt = this.GenerateTitle(source);
				return;
			}
			this.titleInt = null;
			this.taleRef = null;
		}

		// Token: 0x0600FBBE RID: 64446 RVA: 0x0049129F File Offset: 0x0048F49F
		public virtual void JustCreatedBy(Pawn pawn)
		{
			if (this.CanShowArt)
			{
				this.authorNameInt = pawn.NameFullColored;
			}
		}

		// Token: 0x0600FBBF RID: 64447 RVA: 0x004912B5 File Offset: 0x0048F4B5
		public void Clear()
		{
			this.authorNameInt = null;
			this.titleInt = null;
			if (this.taleRef != null)
			{
				this.taleRef.ReferenceDestroyed();
				this.taleRef = null;
			}
		}

		// Token: 0x0600FBC0 RID: 64448 RVA: 0x004912EC File Offset: 0x0048F4EC
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<TaggedString>(ref this.authorNameInt, "authorName", null, false);
			Scribe_Values.Look<TaggedString>(ref this.titleInt, "title", null, false);
			Scribe_Deep.Look<TaleReference>(ref this.taleRef, "taleRef", Array.Empty<object>());
		}

		// Token: 0x0600FBC1 RID: 64449 RVA: 0x00491344 File Offset: 0x0048F544
		public override string CompInspectStringExtra()
		{
			if (!this.Active)
			{
				return null;
			}
			return "Author".Translate() + ": " + this.AuthorName + ("\n" + "Title".Translate() + ": " + this.Title);
		}

		// Token: 0x0600FBC2 RID: 64450 RVA: 0x004913B2 File Offset: 0x0048F5B2
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.taleRef != null)
			{
				this.taleRef.ReferenceDestroyed();
				this.taleRef = null;
			}
		}

		// Token: 0x0600FBC3 RID: 64451 RVA: 0x004913D8 File Offset: 0x0048F5D8
		public override string GetDescriptionPart()
		{
			if (!this.Active)
			{
				return null;
			}
			return "" + this.Title + "\n\n" + this.GenerateImageDescription() + "\n\n" + ("Author".Translate() + ": " + this.AuthorName);
		}

		// Token: 0x0600FBC4 RID: 64452 RVA: 0x0049144C File Offset: 0x0048F64C
		public override bool AllowStackWith(Thing other)
		{
			return !this.Active;
		}

		// Token: 0x0600FBC5 RID: 64453 RVA: 0x0049145C File Offset: 0x0048F65C
		public virtual TaggedString GenerateImageDescription()
		{
			if (this.taleRef == null)
			{
				string str = "Did CompArt.GenerateImageDescription without initializing art: ";
				ThingWithComps parent = this.parent;
				Log.Error(str + ((parent != null) ? parent.ToString() : null));
				this.InitializeArt(ArtGenerationContext.Outsider);
			}
			return this.taleRef.GenerateText(TextGenerationPurpose.ArtDescription, this.Props.descriptionMaker, null, null);
		}

		// Token: 0x0600FBC6 RID: 64454 RVA: 0x004914B4 File Offset: 0x0048F6B4
		protected virtual string GenerateTitle(ArtGenerationContext context)
		{
			if (this.taleRef == null)
			{
				string str = "Did CompArt.GenerateTitle without initializing art: ";
				ThingWithComps parent = this.parent;
				Log.Error(str + ((parent != null) ? parent.ToString() : null));
				this.InitializeArt(ArtGenerationContext.Outsider);
			}
			return GenText.CapitalizeAsTitle(this.taleRef.GenerateText(TextGenerationPurpose.ArtName, this.Props.nameMaker, null, null));
		}

		// Token: 0x0400A51F RID: 42271
		private TaggedString authorNameInt = null;

		// Token: 0x0400A520 RID: 42272
		protected TaggedString titleInt = null;

		// Token: 0x0400A521 RID: 42273
		private TaleReference taleRef;
	}
}
