using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A34 RID: 6708
	public class FleshTypeDef : Def
	{
		// Token: 0x0600972C RID: 38700 RVA: 0x002C2BC0 File Offset: 0x002C0DC0
		public FleshTypeDef.ResolvedWound ChooseBandagedOverlay()
		{
			if (this.bandagedWounds == null)
			{
				return null;
			}
			if (this.bandagedWoundsResolved == null)
			{
				this.bandagedWoundsResolved = (from wound in this.bandagedWounds
				select wound.Resolve()).ToList<FleshTypeDef.ResolvedWound>();
			}
			return this.bandagedWoundsResolved.RandomElement<FleshTypeDef.ResolvedWound>();
		}

		// Token: 0x0600972D RID: 38701 RVA: 0x002C2C20 File Offset: 0x002C0E20
		public FleshTypeDef.ResolvedWound ChooseWoundOverlay(Hediff hediff)
		{
			if (this.genericWounds == null)
			{
				return null;
			}
			if (this.hediffWounds != null)
			{
				foreach (FleshTypeDef.HediffWound hediffWound in this.hediffWounds)
				{
					if (hediffWound.hediff == hediff.def)
					{
						FleshTypeDef.ResolvedWound resolvedWound = hediffWound.ChooseWoundOverlay(hediff);
						if (resolvedWound != null)
						{
							if (resolvedWound.wound.missingBodyPartFresh != null && !resolvedWound.wound.missingBodyPartFresh.Value)
							{
								return resolvedWound;
							}
							if (hediff.IsTended())
							{
								return this.ChooseBandagedOverlay();
							}
							return resolvedWound;
						}
					}
				}
			}
			if (!(hediff is Hediff_Injury))
			{
				Hediff_MissingPart hediff_MissingPart = hediff as Hediff_MissingPart;
				if (hediff_MissingPart == null || !hediff_MissingPart.IsFresh)
				{
					return null;
				}
			}
			if (hediff.IsTended())
			{
				return this.ChooseBandagedOverlay();
			}
			if (this.woundsResolved == null)
			{
				this.woundsResolved = (from wound in this.genericWounds
				select wound.Resolve()).ToList<FleshTypeDef.ResolvedWound>();
			}
			return this.woundsResolved.RandomElement<FleshTypeDef.ResolvedWound>();
		}

		// Token: 0x04006416 RID: 25622
		public ThoughtDef ateDirect;

		// Token: 0x04006417 RID: 25623
		public ThoughtDef ateAsIngredient;

		// Token: 0x04006418 RID: 25624
		public ThingCategoryDef corpseCategory;

		// Token: 0x04006419 RID: 25625
		public EffecterDef damageEffecter;

		// Token: 0x0400641A RID: 25626
		public bool isOrganic = true;

		// Token: 0x0400641B RID: 25627
		public List<FleshTypeDef.Wound> genericWounds;

		// Token: 0x0400641C RID: 25628
		public List<FleshTypeDef.Wound> bandagedWounds;

		// Token: 0x0400641D RID: 25629
		public List<FleshTypeDef.HediffWound> hediffWounds;

		// Token: 0x0400641E RID: 25630
		private List<FleshTypeDef.ResolvedWound> bandagedWoundsResolved;

		// Token: 0x0400641F RID: 25631
		private List<FleshTypeDef.ResolvedWound> woundsResolved;

		// Token: 0x02001A35 RID: 6709
		public class HediffWound
		{
			// Token: 0x0600972F RID: 38703 RVA: 0x002C2D6C File Offset: 0x002C0F6C
			public FleshTypeDef.ResolvedWound ChooseWoundOverlay(Hediff hediff)
			{
				if (this.wounds == null || (this.onlyHumanlikes && !hediff.pawn.RaceProps.Humanlike))
				{
					return null;
				}
				if (this.woundsResolved == null)
				{
					this.woundsResolved = (from wound in this.wounds
					select wound.Resolve()).ToList<FleshTypeDef.ResolvedWound>();
				}
				return (from w in this.woundsResolved
				where w.wound.Fits(hediff)
				select w).RandomElementWithFallback(null);
			}

			// Token: 0x04006420 RID: 25632
			public bool onlyHumanlikes = true;

			// Token: 0x04006421 RID: 25633
			public HediffDef hediff;

			// Token: 0x04006422 RID: 25634
			public List<FleshTypeDef.Wound> wounds;

			// Token: 0x04006423 RID: 25635
			private List<FleshTypeDef.ResolvedWound> woundsResolved;
		}

		// Token: 0x02001A38 RID: 6712
		public class Wound
		{
			// Token: 0x06009736 RID: 38710 RVA: 0x002C2E40 File Offset: 0x002C1040
			public bool Fits(Hediff hediff)
			{
				if (this.onlyOnPart != null && (hediff.Part == null || hediff.Part.def != this.onlyOnPart))
				{
					return false;
				}
				if (this.missingBodyPartFresh != null)
				{
					Hediff_MissingPart hediff_MissingPart = hediff as Hediff_MissingPart;
					if (hediff_MissingPart != null && this.missingBodyPartFresh.Value != (hediff_MissingPart.IsFresh || hediff_MissingPart.Bleeding))
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x06009737 RID: 38711 RVA: 0x002C2EAC File Offset: 0x002C10AC
			public FleshTypeDef.ResolvedWound Resolve()
			{
				Shader shader = this.tintWithSkinColor ? ShaderDatabase.WoundSkin : ShaderDatabase.Wound;
				if (this.texture != null)
				{
					return new FleshTypeDef.ResolvedWound(this, MaterialPool.MatFrom(this.texture, shader, this.color));
				}
				return new FleshTypeDef.ResolvedWound(this, MaterialPool.MatFrom(this.textureSouth, shader, this.color), MaterialPool.MatFrom(this.textureEast, shader, this.color), MaterialPool.MatFrom(this.textureNorth, shader, this.color), MaterialPool.MatFrom(this.textureWest, shader, this.color), this.flipSouth, this.flipEast, this.flipNorth, this.flipWest);
			}

			// Token: 0x04006427 RID: 25639
			[NoTranslate]
			public string texture;

			// Token: 0x04006428 RID: 25640
			[NoTranslate]
			public string textureSouth;

			// Token: 0x04006429 RID: 25641
			[NoTranslate]
			public string textureEast;

			// Token: 0x0400642A RID: 25642
			[NoTranslate]
			public string textureNorth;

			// Token: 0x0400642B RID: 25643
			[NoTranslate]
			public string textureWest;

			// Token: 0x0400642C RID: 25644
			public bool flipSouth;

			// Token: 0x0400642D RID: 25645
			public bool flipEast;

			// Token: 0x0400642E RID: 25646
			public bool flipNorth;

			// Token: 0x0400642F RID: 25647
			public bool flipWest;

			// Token: 0x04006430 RID: 25648
			public float scale = 1f;

			// Token: 0x04006431 RID: 25649
			public Vector3 drawOffsetSouth;

			// Token: 0x04006432 RID: 25650
			public Vector3 drawOffsetEastWest;

			// Token: 0x04006433 RID: 25651
			public Color color = Color.white;

			// Token: 0x04006434 RID: 25652
			public bool tintWithSkinColor;

			// Token: 0x04006435 RID: 25653
			[NoTranslate]
			public string flipOnWoundAnchorTag;

			// Token: 0x04006436 RID: 25654
			public Rot4 flipOnRotation;

			// Token: 0x04006437 RID: 25655
			public BodyPartDef onlyOnPart;

			// Token: 0x04006438 RID: 25656
			public bool displayOverApparel = true;

			// Token: 0x04006439 RID: 25657
			public bool displayPermanent;

			// Token: 0x0400643A RID: 25658
			public bool? missingBodyPartFresh;
		}

		// Token: 0x02001A39 RID: 6713
		public class ResolvedWound
		{
			// Token: 0x06009739 RID: 38713 RVA: 0x002C2F7C File Offset: 0x002C117C
			public ResolvedWound(FleshTypeDef.Wound wound, Material material)
			{
				this.wound = wound;
				this.matWest = material;
				this.matNorth = material;
				this.matEast = material;
				this.matSouth = material;
			}

			// Token: 0x0600973A RID: 38714 RVA: 0x002C2FB8 File Offset: 0x002C11B8
			public ResolvedWound(FleshTypeDef.Wound wound, Material matSouth, Material matEast, Material matNorth, Material matWest, bool flipSouth, bool flipEast, bool flipNorth, bool flipWest)
			{
				this.wound = wound;
				this.matSouth = matSouth;
				this.matEast = matEast;
				this.matNorth = matNorth;
				this.matWest = matWest;
				this.flipSouth = flipSouth;
				this.flipEast = flipEast;
				this.flipNorth = flipNorth;
				this.flipWest = flipWest;
			}

			// Token: 0x0600973B RID: 38715 RVA: 0x002C3010 File Offset: 0x002C1210
			public Material GetMaterial(Rot4 rotation, out bool flip)
			{
				flip = false;
				if (rotation == Rot4.South)
				{
					flip = this.flipSouth;
					return this.matSouth;
				}
				if (rotation == Rot4.East)
				{
					flip = this.flipEast;
					return this.matEast;
				}
				if (rotation == Rot4.West)
				{
					flip = this.flipWest;
					return this.matWest;
				}
				if (rotation == Rot4.North)
				{
					flip = this.flipNorth;
					return this.matNorth;
				}
				return this.matSouth;
			}

			// Token: 0x0400643B RID: 25659
			public Material material;

			// Token: 0x0400643C RID: 25660
			public Material matSouth;

			// Token: 0x0400643D RID: 25661
			public Material matEast;

			// Token: 0x0400643E RID: 25662
			public Material matNorth;

			// Token: 0x0400643F RID: 25663
			public Material matWest;

			// Token: 0x04006440 RID: 25664
			public bool flipSouth;

			// Token: 0x04006441 RID: 25665
			public bool flipEast;

			// Token: 0x04006442 RID: 25666
			public bool flipNorth;

			// Token: 0x04006443 RID: 25667
			public bool flipWest;

			// Token: 0x04006444 RID: 25668
			public FleshTypeDef.Wound wound;
		}
	}
}
