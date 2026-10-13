using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020005F4 RID: 1524
	public class PawnRenderer
	{
		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x060026F4 RID: 9972 RVA: 0x000CE840 File Offset: 0x000CCA40
		public Graphic BodyGraphic
		{
			get
			{
				return this.renderTree.BodyGraphic;
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x060026F5 RID: 9973 RVA: 0x000CE84D File Offset: 0x000CCA4D
		public Graphic HeadGraphic
		{
			get
			{
				return this.renderTree.HeadGraphic;
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x060026F6 RID: 9974 RVA: 0x000CE85C File Offset: 0x000CCA5C
		public RotDrawMode CurRotDrawMode
		{
			get
			{
				if (this.pawn.IsMutant && this.pawn.mutant.Def.useCorpseGraphics)
				{
					return RottableUtility.GetRotDrawMode(this.pawn.mutant.rotStage);
				}
				if (this.pawn.Dead && this.pawn.Corpse != null)
				{
					return this.pawn.Corpse.CurRotDrawMode;
				}
				return RotDrawMode.Fresh;
			}
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x060026F7 RID: 9975 RVA: 0x000CE8CF File Offset: 0x000CCACF
		public PawnWoundDrawer WoundOverlays
		{
			get
			{
				return this.woundOverlays;
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x060026F8 RID: 9976 RVA: 0x000CE8D7 File Offset: 0x000CCAD7
		public PawnFirefoamDrawer FirefoamOverlays
		{
			get
			{
				return this.firefoamOverlays;
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x060026F9 RID: 9977 RVA: 0x000CE8DF File Offset: 0x000CCADF
		public PawnShamblerScarDrawer ShamblerScarDrawer
		{
			get
			{
				return this.shamblerScarDrawer;
			}
		}

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x060026FA RID: 9978 RVA: 0x000CE8E7 File Offset: 0x000CCAE7
		public PawnScariaSoreDrawer ScariaSoreDrawer
		{
			get
			{
				return this.scariaSoreDrawer;
			}
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x060026FB RID: 9979 RVA: 0x000CE8EF File Offset: 0x000CCAEF
		public PawnHeadOverlays StatusOverlays
		{
			get
			{
				return this.statusOverlays;
			}
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x060026FC RID: 9980 RVA: 0x000CE8F7 File Offset: 0x000CCAF7
		public AnimationDef CurAnimation
		{
			get
			{
				return this.renderTree.currentAnimation;
			}
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x060026FD RID: 9981 RVA: 0x000CE904 File Offset: 0x000CCB04
		public bool HasAnimation
		{
			get
			{
				return this.renderTree.currentAnimation != null;
			}
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x060026FE RID: 9982 RVA: 0x000CE914 File Offset: 0x000CCB14
		public Graphic SilhouetteGraphic
		{
			get
			{
				return this.silhouetteGraphic;
			}
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x060026FF RID: 9983 RVA: 0x000CE91C File Offset: 0x000CCB1C
		public Vector3 SilhouettePos
		{
			get
			{
				return this.silhouettePos;
			}
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06002700 RID: 9984 RVA: 0x000CE924 File Offset: 0x000CCB24
		public Material FlightShadowMaterial
		{
			get
			{
				return MaterialPool.MatFrom("Things/Skyfaller/SkyfallerShadowCircle", ShaderDatabase.Transparent, new Color(1f, 1f, 1f, 0.5f));
			}
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06002701 RID: 9985 RVA: 0x000CE950 File Offset: 0x000CCB50
		public Color? StatueColor
		{
			get
			{
				if (this.statueStuff == null)
				{
					return null;
				}
				Color? result = this.statuePaintColor;
				if (result != null)
				{
					return result;
				}
				ThingDef thingDef = this.statueStuff;
				if (thingDef == null)
				{
					return null;
				}
				StuffProperties stuffProps = thingDef.stuffProps;
				if (stuffProps == null)
				{
					return null;
				}
				return new Color?(stuffProps.color);
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06002702 RID: 9986 RVA: 0x000CE9B4 File Offset: 0x000CCBB4
		private PawnRenderFlags DefaultRenderFlagsNow
		{
			get
			{
				PawnRenderFlags pawnRenderFlags;
				this.PawnNeedsHediffMaterial(out pawnRenderFlags);
				if (!this.pawn.health.hediffSet.HasHead)
				{
					pawnRenderFlags |= PawnRenderFlags.HeadStump;
				}
				if (this.statueStuff != null)
				{
					pawnRenderFlags |= PawnRenderFlags.Statue;
				}
				return pawnRenderFlags;
			}
		}

		// Token: 0x06002703 RID: 9987 RVA: 0x000CE9F8 File Offset: 0x000CCBF8
		public PawnRenderer(Pawn pawn)
		{
			this.pawn = pawn;
			this.wiggler = new PawnDownedWiggler(pawn);
			this.statusOverlays = new PawnHeadOverlays(pawn);
			this.woundOverlays = new PawnWoundDrawer(pawn);
			this.firefoamOverlays = new PawnFirefoamDrawer(pawn);
			this.shamblerScarDrawer = new PawnShamblerScarDrawer(pawn);
			this.scariaSoreDrawer = new PawnScariaSoreDrawer(pawn);
			this.effecters = new PawnStatusEffecters(pawn);
			this.renderTree = new PawnRenderTree(pawn);
			this.flasher = new DamageFlasher(pawn);
			if (pawn.RaceProps.startingAnimation != null)
			{
				this.SetAnimation(pawn.RaceProps.startingAnimation);
			}
		}

		// Token: 0x06002704 RID: 9988 RVA: 0x000CEAA7 File Offset: 0x000CCCA7
		public void SetStatue(ThingDef stuff)
		{
			this.statueStuff = stuff;
		}

		// Token: 0x06002705 RID: 9989 RVA: 0x000CEAB0 File Offset: 0x000CCCB0
		public void SetStatuePaintColor(Color? color)
		{
			this.statuePaintColor = color;
		}

		// Token: 0x06002706 RID: 9990 RVA: 0x000CEABC File Offset: 0x000CCCBC
		private Mesh GetBlitMeshUpdatedFrame(PawnTextureAtlasFrameSet frameSet, Rot4 rotation, PawnDrawMode drawMode)
		{
			int index = frameSet.GetIndex(rotation, drawMode);
			if (frameSet.isDirty[index])
			{
				Find.PawnCacheCamera.rect = frameSet.uvRects[index];
				Find.PawnCacheRenderer.RenderPawn(this.pawn, frameSet.atlas, Vector3.zero, 1f, 0f, rotation, true, true, true, false, default(Vector3), null, null, false);
				Find.PawnCacheCamera.rect = new Rect(0f, 0f, 1f, 1f);
				frameSet.isDirty[index] = false;
			}
			return frameSet.meshes[index];
		}

		// Token: 0x06002707 RID: 9991 RVA: 0x000CEB68 File Offset: 0x000CCD68
		private void DrawShadowInternal(Vector3 drawLoc)
		{
			if (this.pawn.Flying)
			{
				Vector3 vector = this.pawn.DrawPos;
				vector.y = AltitudeLayer.Filth.AltitudeFor();
				vector -= new Vector3(0f, 0f, this.pawn.flight.PositionOffsetFactor);
				Matrix4x4 matrix = Matrix4x4.TRS(vector, Quaternion.identity, Vector3.one * 0.75f);
				Graphics.DrawMesh(MeshPool.plane10, matrix, this.FlightShadowMaterial, 0);
				return;
			}
			if (this.pawn.Swimming || this.pawn.DrawNonHumanlikeSwimmingGraphic)
			{
				return;
			}
			if (this.pawn.def.race.specialShadowData != null)
			{
				if (this.shadowGraphic == null)
				{
					this.shadowGraphic = new Graphic_Shadow(this.pawn.def.race.specialShadowData);
				}
				this.shadowGraphic.Draw(drawLoc, Rot4.North, this.pawn, 0f);
			}
			Graphic bodyGraphic = this.BodyGraphic;
			if (bodyGraphic == null)
			{
				return;
			}
			Graphic_Shadow graphic_Shadow = bodyGraphic.ShadowGraphic;
			if (graphic_Shadow == null)
			{
				return;
			}
			graphic_Shadow.Draw(drawLoc, Rot4.North, this.pawn, 0f);
		}

		// Token: 0x06002708 RID: 9992 RVA: 0x000CEC90 File Offset: 0x000CCE90
		public void DynamicDrawPhaseAt(DrawPhase phase, Vector3 drawLoc, Rot4? rotOverride = null, bool neverAimWeapon = false)
		{
			if (phase == DrawPhase.EnsureInitialized)
			{
				this.EnsureGraphicsInitialized();
				return;
			}
			if (phase == DrawPhase.ParallelPreDraw)
			{
				this.ParallelPreRenderPawnAt(drawLoc, rotOverride, neverAimWeapon);
				return;
			}
			if (phase == DrawPhase.Draw)
			{
				this.RenderPawnAt(drawLoc, rotOverride, neverAimWeapon);
			}
		}

		// Token: 0x06002709 RID: 9993 RVA: 0x000CECBC File Offset: 0x000CCEBC
		public void RenderPawnAt(Vector3 drawLoc, Rot4? rotOverride = null, bool neverAimWeapon = false)
		{
			if (!this.results.valid)
			{
				this.EnsureGraphicsInitialized();
				this.ParallelPreRenderPawnAt(drawLoc, rotOverride, neverAimWeapon);
			}
			if (!this.results.draw)
			{
				return;
			}
			if (this.results.useCached)
			{
				PawnTextureAtlasFrameSet pawnTextureAtlasFrameSet;
				bool flag;
				if (GlobalTextureAtlasManager.TryGetPawnFrameSet(this.pawn, out pawnTextureAtlasFrameSet, out flag, true))
				{
					using (new ProfilerBlock("Draw Cached Mesh"))
					{
						Rot4 facing = this.results.parms.facing;
						Vector3 bodyPos = this.results.bodyPos;
						float bodyAngle = this.results.bodyAngle;
						PawnDrawMode drawMode = this.results.showBody ? PawnDrawMode.BodyAndHead : PawnDrawMode.HeadOnly;
						Material mat = this.OverrideMaterialIfNeeded(MaterialPool.MatFrom(new MaterialRequest(pawnTextureAtlasFrameSet.atlas, ShaderDatabase.Cutout)), PawnRenderFlags.None);
						GenDraw.DrawMeshNowOrLater(this.GetBlitMeshUpdatedFrame(pawnTextureAtlasFrameSet, facing, drawMode), bodyPos, Quaternion.AngleAxis(bodyAngle, Vector3.up), mat, false);
						Vector3 drawPos = bodyPos.WithYOffset(PawnRenderUtility.AltitudeForLayer((facing == Rot4.North) ? -10f : 90f));
						PawnRenderUtility.DrawEquipmentAndApparelExtras(this.pawn, drawPos, facing, this.results.parms.flags);
						goto IL_18E;
					}
				}
				Log.ErrorOnce(string.Format("Attempted to use a cached frame set for pawn {0} but failed to get one.", this.pawn.Name), Gen.HashCombine<int>(this.pawn.GetHashCode(), 5938111));
			}
			else
			{
				using (new ProfilerBlock("Render Pawn Internal"))
				{
					this.RenderPawnInternal(this.results.parms);
				}
			}
			IL_18E:
			if (this.results.posture == PawnPosture.Standing && !this.results.parms.flags.FlagSet(PawnRenderFlags.Invisible))
			{
				using (new ProfilerBlock("Draw Shadow Internal"))
				{
					this.DrawShadowInternal(drawLoc);
				}
			}
			if (this.pawn.Spawned && !this.pawn.Dead)
			{
				this.pawn.stances.StanceTrackerDraw();
				this.pawn.pather.PatherDraw();
				this.pawn.roping.RopingDraw();
			}
			Graphic graphic;
			if (this.pawn.RaceProps.Humanlike)
			{
				graphic = this.pawn.ageTracker.CurLifeStage.silhouetteGraphicData.Graphic;
			}
			else if (this.pawn.ageTracker.CurKindLifeStage.silhouetteGraphicData != null)
			{
				graphic = this.pawn.ageTracker.CurKindLifeStage.silhouetteGraphicData.Graphic;
			}
			else
			{
				graphic = this.BodyGraphic;
			}
			this.SetSilhouetteData(graphic, this.results.bodyPos);
			this.DrawDebug();
			this.results = default(PawnRenderer.PreRenderResults);
		}

		// Token: 0x0600270A RID: 9994 RVA: 0x000CEFA0 File Offset: 0x000CD1A0
		private void SetSilhouetteData(Graphic graphic, Vector3 pos)
		{
			this.silhouetteGraphic = graphic;
			this.silhouettePos = pos;
		}

		// Token: 0x0600270B RID: 9995 RVA: 0x000CEFB0 File Offset: 0x000CD1B0
		public void RenderShadowOnlyAt(Vector3 drawLoc)
		{
			if (this.pawn.GetPosture() == PawnPosture.Standing && !this.DefaultRenderFlagsNow.FlagSet(PawnRenderFlags.Invisible))
			{
				this.DrawShadowInternal(drawLoc);
			}
		}

		// Token: 0x0600270C RID: 9996 RVA: 0x000CEFD4 File Offset: 0x000CD1D4
		public void EnsureGraphicsInitialized()
		{
			this.renderTree.EnsureInitialized(this.DefaultRenderFlagsNow);
		}

		// Token: 0x0600270D RID: 9997 RVA: 0x000CEFE7 File Offset: 0x000CD1E7
		private void ParallelPreRenderPawnAt(Vector3 drawLoc, Rot4? rotOverride = null, bool neverAimWeapon = false)
		{
			this.results = this.ParallelGetPreRenderResults(drawLoc, rotOverride, neverAimWeapon, false);
		}

		// Token: 0x0600270E RID: 9998 RVA: 0x000CEFFC File Offset: 0x000CD1FC
		private PawnRenderer.PreRenderResults ParallelGetPreRenderResults(Vector3 drawLoc, Rot4? rotOverride = null, bool neverAimWeapon = false, bool disableCache = false)
		{
			PawnRenderer.PreRenderResults preRenderResults = new PawnRenderer.PreRenderResults
			{
				valid = true
			};
			if (this.pawn.IsHiddenFromPlayer())
			{
				return preRenderResults;
			}
			preRenderResults.draw = true;
			if (!this.renderTree.Resolved)
			{
				this.renderTree.SetDirty();
			}
			PawnRenderFlags pawnRenderFlags = this.DefaultRenderFlagsNow | PawnRenderFlags.Clothes | PawnRenderFlags.Headgear;
			if (neverAimWeapon)
			{
				pawnRenderFlags |= PawnRenderFlags.NeverAimWeapon;
			}
			bool crawling = this.pawn.Crawling;
			bool swimming = this.pawn.Swimming;
			if (swimming)
			{
				pawnRenderFlags &= ~(PawnRenderFlags.Headgear | PawnRenderFlags.Clothes | PawnRenderFlags.NeverAimWeapon);
				pawnRenderFlags |= PawnRenderFlags.NoBody;
			}
			RotDrawMode curRotDrawMode = this.CurRotDrawMode;
			PawnPosture posture = this.pawn.GetPosture();
			bool showBody;
			Vector3 bodyPos = this.GetBodyPos(drawLoc, posture, out showBody);
			float num = (posture == PawnPosture.Standing) ? 0f : this.BodyAngle(pawnRenderFlags);
			Rot4 bodyFacing = rotOverride ?? ((posture == PawnPosture.Standing || crawling) ? this.pawn.Rotation : this.LayingFacing());
			preRenderResults.posture = posture;
			preRenderResults.showBody = showBody;
			preRenderResults.bodyPos = bodyPos;
			preRenderResults.bodyAngle = num;
			bool useCached;
			if (!disableCache && this.pawn.RaceProps.Humanlike && Find.CameraDriver.ZoomRootSize > 18f && !pawnRenderFlags.FlagSet(PawnRenderFlags.Portrait) && !pawnRenderFlags.FlagSet(PawnRenderFlags.Statue) && curRotDrawMode != RotDrawMode.Dessicated && !crawling && !swimming && this.renderTree.currentAnimation == null)
			{
				Pawn_CarryTracker carryTracker = this.pawn.carryTracker;
				if (((carryTracker != null) ? carryTracker.CarriedThing : null) == null && this.pawn.CarriedBy == null)
				{
					PawnRenderFlags pawnRenderFlags2;
					useCached = !this.PawnNeedsHediffMaterial(out pawnRenderFlags2);
					goto IL_1A0;
				}
			}
			useCached = false;
			IL_1A0:
			preRenderResults.useCached = useCached;
			preRenderResults.parms = this.GetDrawParms(bodyPos, num, bodyFacing, curRotDrawMode, pawnRenderFlags);
			if (preRenderResults.useCached)
			{
				return preRenderResults;
			}
			this.renderTree.ParallelPreDraw(preRenderResults.parms);
			return preRenderResults;
		}

		// Token: 0x0600270F RID: 9999 RVA: 0x000CF1E0 File Offset: 0x000CD3E0
		public void RenderCache(Rot4 rotation, float angle, Vector3 positionOffset, bool renderHead, bool portrait, bool renderHeadgear, bool renderClothes, IReadOnlyDictionary<Apparel, Color> overrideApparelColor = null, Color? overrideHairColor = null, bool stylingStation = false)
		{
			Vector3 zero = Vector3.zero;
			PawnRenderFlags pawnRenderFlags = this.DefaultRenderFlagsNow | PawnRenderFlags.Cache | PawnRenderFlags.DrawNow;
			if (portrait)
			{
				pawnRenderFlags |= PawnRenderFlags.Portrait;
			}
			if (!renderHead)
			{
				pawnRenderFlags |= PawnRenderFlags.HeadStump;
			}
			if (renderHeadgear)
			{
				pawnRenderFlags |= PawnRenderFlags.Headgear;
			}
			if (renderClothes)
			{
				pawnRenderFlags |= PawnRenderFlags.Clothes;
			}
			if (stylingStation)
			{
				pawnRenderFlags |= PawnRenderFlags.StylingStation;
			}
			this.tmpOriginalColors.Clear();
			try
			{
				if (overrideApparelColor != null)
				{
					foreach (KeyValuePair<Apparel, Color> keyValuePair in overrideApparelColor)
					{
						Apparel apparel;
						Color color;
						keyValuePair.Deconstruct(out apparel, out color);
						Apparel apparel2 = apparel;
						Color newColor = color;
						CompColorable compColorable = apparel2.TryGetComp<CompColorable>();
						if (compColorable != null)
						{
							this.tmpOriginalColors.Add(apparel2, new ValueTuple<Color, bool>(compColorable.Color, compColorable.Active));
							apparel2.SetColor(newColor, true);
						}
					}
				}
				Color hairColor = Color.white;
				if (this.pawn.story != null)
				{
					hairColor = this.pawn.story.HairColor;
					if (overrideHairColor != null)
					{
						this.pawn.story.HairColor = overrideHairColor.Value;
						this.pawn.Drawer.renderer.renderTree.SetDirty();
					}
				}
				PawnDrawParms drawParms = this.GetDrawParms(zero + positionOffset, angle, rotation, this.CurRotDrawMode, pawnRenderFlags);
				this.renderTree.EnsureInitialized(this.DefaultRenderFlagsNow);
				this.renderTree.ParallelPreDraw(drawParms);
				this.RenderPawnInternal(drawParms);
				foreach (KeyValuePair<Apparel, ValueTuple<Color, bool>> keyValuePair2 in this.tmpOriginalColors)
				{
					if (!keyValuePair2.Value.Item2)
					{
						keyValuePair2.Key.TryGetComp<CompColorable>().Disable();
					}
					else
					{
						keyValuePair2.Key.SetColor(keyValuePair2.Value.Item1, true);
					}
				}
				if (this.pawn.story != null && overrideHairColor != null)
				{
					this.pawn.story.HairColor = hairColor;
					this.pawn.Drawer.renderer.renderTree.SetDirty();
				}
			}
			catch (Exception ex)
			{
				string str = "Error rendering pawn portrait: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
			finally
			{
				this.tmpOriginalColors.Clear();
			}
		}

		// Token: 0x06002710 RID: 10000 RVA: 0x000CF48C File Offset: 0x000CD68C
		private Vector3 GetBodyPos(Vector3 drawLoc, PawnPosture posture, out bool showBody)
		{
			if (posture == PawnPosture.Standing)
			{
				showBody = true;
				return drawLoc;
			}
			Building_Bed building_Bed = this.pawn.CurrentBed();
			Vector3 result;
			if (building_Bed != null && this.pawn.RaceProps.Humanlike)
			{
				showBody = building_Bed.def.building.bed_showSleeperBody;
				AltitudeLayer altLayer = (AltitudeLayer)Mathf.Max((int)building_Bed.def.altitudeLayer, 20);
				Vector3 a = this.pawn.Position.ToVector3ShiftedWithAltitude(altLayer);
				Rot4 rotation = building_Bed.Rotation;
				rotation.AsInt += 2;
				float d = this.BaseHeadOffsetAt(Rot4.South).z + this.pawn.story.bodyType.bedOffset + building_Bed.def.building.bed_pawnDrawOffset;
				Vector3 a2 = rotation.FacingCell.ToVector3();
				result = a - a2 * d;
			}
			else
			{
				showBody = true;
				result = drawLoc;
				IThingHolderWithDrawnPawn thingHolderWithDrawnPawn = this.pawn.ParentHolder as IThingHolderWithDrawnPawn;
				if (thingHolderWithDrawnPawn != null)
				{
					result.y = thingHolderWithDrawnPawn.HeldPawnDrawPos_Y;
				}
				else
				{
					IThingHolderWithDrawnPawn thingHolderWithDrawnPawn2 = this.pawn.ParentHolder.ParentHolder as IThingHolderWithDrawnPawn;
					if (thingHolderWithDrawnPawn2 != null)
					{
						result.y = thingHolderWithDrawnPawn2.HeldPawnDrawPos_Y;
					}
					else if (!this.pawn.Dead && this.pawn.CarriedBy == null && this.pawn.ParentHolder.Isnt<PawnFlyer>())
					{
						result.y = AltitudeLayer.LayingPawn.AltitudeFor();
					}
				}
			}
			Pawn_MindState mindState = this.pawn.mindState;
			bool? flag;
			if (mindState == null)
			{
				flag = null;
			}
			else
			{
				PawnDuty duty = mindState.duty;
				if (duty == null)
				{
					flag = null;
				}
				else
				{
					DutyDef def = duty.def;
					flag = ((def != null) ? def.drawBodyOverride : null);
				}
			}
			showBody = (flag ?? showBody);
			return result;
		}

		// Token: 0x06002711 RID: 10001 RVA: 0x000CF66B File Offset: 0x000CD86B
		private bool PawnNeedsHediffMaterial(out PawnRenderFlags renderFlags)
		{
			renderFlags = PawnRenderFlags.None;
			if (this.pawn.IsPsychologicallyInvisible())
			{
				renderFlags |= PawnRenderFlags.Invisible;
				return true;
			}
			return false;
		}

		// Token: 0x06002712 RID: 10002 RVA: 0x000CF686 File Offset: 0x000CD886
		private void RenderPawnInternal(PawnDrawParms parms)
		{
			this.renderTree.Draw(parms);
		}

		// Token: 0x06002713 RID: 10003 RVA: 0x000CF694 File Offset: 0x000CD894
		private PawnDrawParms GetDrawParms(Vector3 rootLoc, float angle, Rot4 bodyFacing, RotDrawMode bodyDrawType, PawnRenderFlags flags)
		{
			PawnDrawParms result = default(PawnDrawParms);
			result.pawn = this.pawn;
			result.matrix = Matrix4x4.TRS(rootLoc + this.pawn.ageTracker.CurLifeStage.bodyDrawOffset, Quaternion.AngleAxis(angle, Vector3.up), Vector3.one);
			result.facing = bodyFacing;
			result.rotDrawMode = bodyDrawType;
			result.posture = this.pawn.GetPosture();
			result.flags = flags;
			result.tint = this.flasher.CurColor.ToTransparent(InvisibilityUtility.GetAlpha(this.pawn));
			result.bed = this.pawn.CurrentBed();
			result.coveredInFoam = this.FirefoamOverlays.coveredInFoam;
			Pawn_CarryTracker carryTracker = this.pawn.carryTracker;
			result.carriedThing = ((carryTracker != null) ? carryTracker.CarriedThing : null);
			result.dead = this.pawn.Dead;
			result.crawling = this.pawn.Crawling;
			result.swimming = this.pawn.Swimming;
			result.statueColor = this.StatueColor;
			return result;
		}

		// Token: 0x06002714 RID: 10004 RVA: 0x000CF7C1 File Offset: 0x000CD9C1
		private Material OverrideMaterialIfNeeded(Material original, PawnRenderFlags flags)
		{
			if (flags.FlagSet(PawnRenderFlags.Cache) || flags.FlagSet(PawnRenderFlags.Portrait))
			{
				return original;
			}
			if (this.pawn.IsPsychologicallyInvisible())
			{
				return InvisibilityMatPool.GetInvisibleMat(original);
			}
			return this.flasher.GetDamagedMat(original);
		}

		// Token: 0x06002715 RID: 10005 RVA: 0x000CF7F8 File Offset: 0x000CD9F8
		private Rot4 RotationForcedByJob()
		{
			Pawn_JobTracker jobs = this.pawn.jobs;
			if (((jobs != null) ? jobs.curDriver : null) != null && this.pawn.jobs.curDriver.ForcedLayingRotation.IsValid)
			{
				return this.pawn.jobs.curDriver.ForcedLayingRotation;
			}
			return Rot4.Invalid;
		}

		// Token: 0x06002716 RID: 10006 RVA: 0x000CF858 File Offset: 0x000CDA58
		public Rot4 LayingFacing()
		{
			Rot4 result = this.RotationForcedByJob();
			if (result.IsValid)
			{
				return result;
			}
			PawnPosture posture = this.pawn.GetPosture();
			if (posture == PawnPosture.LayingOnGroundFaceUp || this.pawn.Deathresting)
			{
				return Rot4.South;
			}
			if (this.pawn.RaceProps.Humanlike)
			{
				if (this.pawn.DevelopmentalStage.Baby())
				{
					Pawn_CarryTracker pawn_CarryTracker = this.pawn.ParentHolder as Pawn_CarryTracker;
					if (pawn_CarryTracker != null)
					{
						if (!(pawn_CarryTracker.pawn.Rotation == Rot4.West) && !(pawn_CarryTracker.pawn.Rotation == Rot4.North))
						{
							return Rot4.West;
						}
						return Rot4.East;
					}
				}
				if (posture.FaceUp() && this.pawn.CurrentBed() != null)
				{
					return Rot4.South;
				}
				switch (this.pawn.thingIDNumber % 4)
				{
				case 0:
					return Rot4.South;
				case 1:
					return Rot4.South;
				case 2:
					return Rot4.East;
				case 3:
					return Rot4.West;
				}
			}
			else
			{
				switch (this.pawn.thingIDNumber % 4)
				{
				case 0:
					return Rot4.South;
				case 1:
					return Rot4.East;
				case 2:
					return Rot4.West;
				case 3:
					return Rot4.West;
				}
			}
			return Rot4.Random;
		}

		// Token: 0x06002717 RID: 10007 RVA: 0x000CF9AC File Offset: 0x000CDBAC
		public float BodyAngle(PawnRenderFlags flags)
		{
			if (this.pawn.GetPosture() == PawnPosture.Standing)
			{
				return 0f;
			}
			Building_Bed building_Bed = this.pawn.CurrentBed();
			if (building_Bed != null && this.pawn.RaceProps.Humanlike)
			{
				Rot4 rotation = building_Bed.Rotation;
				rotation.AsInt += 2;
				return rotation.AsAngle;
			}
			IThingHolderWithDrawnPawn thingHolderWithDrawnPawn = this.pawn.ParentHolder as IThingHolderWithDrawnPawn;
			if (thingHolderWithDrawnPawn != null)
			{
				return thingHolderWithDrawnPawn.HeldPawnBodyAngle;
			}
			IThingHolderWithDrawnPawn thingHolderWithDrawnPawn2 = this.pawn.ParentHolder.ParentHolder as IThingHolderWithDrawnPawn;
			if (thingHolderWithDrawnPawn2 != null)
			{
				return thingHolderWithDrawnPawn2.HeldPawnBodyAngle;
			}
			Pawn_CarryTracker pawn_CarryTracker = this.pawn.ParentHolder as Pawn_CarryTracker;
			if (pawn_CarryTracker != null)
			{
				Rot4 rotation2 = pawn_CarryTracker.pawn.Rotation;
				return ((rotation2 == Rot4.West || rotation2 == Rot4.North) ? 290f : 70f) + pawn_CarryTracker.pawn.Drawer.renderer.BodyAngle(flags);
			}
			if (this.pawn.Crawling && !flags.FlagSet(PawnRenderFlags.Portrait))
			{
				return PawnRenderUtility.CrawlingBodyAngle(this.pawn.Rotation);
			}
			if (this.pawn.Downed || this.pawn.Dead)
			{
				return this.wiggler.downedAngle;
			}
			if (this.pawn.RaceProps.Humanlike)
			{
				return this.LayingFacing().AsAngle;
			}
			if (this.RotationForcedByJob().IsValid)
			{
				return 0f;
			}
			return ((this.pawn.thingIDNumber % 2 == 0) ? Rot4.West : Rot4.East).AsAngle;
		}

		// Token: 0x06002718 RID: 10008 RVA: 0x000CFB54 File Offset: 0x000CDD54
		public Vector3 BaseHeadOffsetAt(Rot4 rotation)
		{
			Vector2 vector = this.pawn.story.bodyType.headOffset * Mathf.Sqrt(this.pawn.ageTracker.CurLifeStage.bodySizeFactor);
			switch (rotation.AsInt)
			{
			case 0:
				return new Vector3(0f, 0f, vector.y);
			case 1:
				return new Vector3(vector.x, 0f, vector.y);
			case 2:
				return new Vector3(0f, 0f, vector.y);
			case 3:
				return new Vector3(-vector.x, 0f, vector.y);
			default:
			{
				string str = "BaseHeadOffsetAt error in ";
				Pawn pawn = this.pawn;
				Log.Error(str + ((pawn != null) ? pawn.ToString() : null));
				return Vector3.zero;
			}
			}
		}

		// Token: 0x06002719 RID: 10009 RVA: 0x000CFC38 File Offset: 0x000CDE38
		public void SetAnimation(AnimationDef animation)
		{
			if (animation == null)
			{
				this.renderTree.currentAnimation = null;
				this.renderTree.animationStartTick = -99999;
			}
			else
			{
				this.renderTree.currentAnimation = animation;
				this.renderTree.animationStartTick = Find.TickManager.TicksGame;
				if (animation.startOnRandomTick)
				{
					this.renderTree.animationStartTick += Rand.RangeSeeded(0, animation.durationTicks, this.pawn.thingIDNumber);
				}
			}
			this.renderTree.SetDirty();
		}

		// Token: 0x0600271A RID: 10010 RVA: 0x000CFCC3 File Offset: 0x000CDEC3
		public void SetAllGraphicsDirty()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (this.renderTree.Resolved)
				{
					this.renderTree.SetDirty();
					SilhouetteUtility.NotifyGraphicDirty(this.pawn);
					this.WoundOverlays.ClearCache();
					PortraitsCache.SetDirty(this.pawn);
					GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(this.pawn);
				}
			});
		}

		// Token: 0x0600271B RID: 10011 RVA: 0x000CFCD6 File Offset: 0x000CDED6
		public void Notify_DamageApplied(DamageInfo dam)
		{
			this.flasher.Notify_DamageApplied(dam);
			this.wiggler.Notify_DamageApplied(dam);
		}

		// Token: 0x0600271C RID: 10012 RVA: 0x000CFCF0 File Offset: 0x000CDEF0
		public void ProcessPostTickVisuals(int ticksPassed)
		{
			this.wiggler.ProcessPostTickVisuals(ticksPassed);
			if (this.renderTree.AnimationFinished)
			{
				this.SetAnimation(null);
			}
		}

		// Token: 0x0600271D RID: 10013 RVA: 0x000CFD12 File Offset: 0x000CDF12
		public void EffectersTick(bool suspended)
		{
			this.effecters.EffectersTick(suspended);
		}

		// Token: 0x0600271E RID: 10014 RVA: 0x000CFD20 File Offset: 0x000CDF20
		private void DrawDebug()
		{
			if (DebugViewSettings.drawDuties && Find.Selector.IsSelected(this.pawn))
			{
				Pawn_MindState mindState = this.pawn.mindState;
				if (mindState == null)
				{
					return;
				}
				PawnDuty duty = mindState.duty;
				if (duty == null)
				{
					return;
				}
				duty.DrawDebug(this.pawn);
			}
		}

		// Token: 0x0400205A RID: 8282
		private readonly Pawn pawn;

		// Token: 0x0400205B RID: 8283
		public PawnDownedWiggler wiggler;

		// Token: 0x0400205C RID: 8284
		private PawnHeadOverlays statusOverlays;

		// Token: 0x0400205D RID: 8285
		private PawnStatusEffecters effecters;

		// Token: 0x0400205E RID: 8286
		private PawnWoundDrawer woundOverlays;

		// Token: 0x0400205F RID: 8287
		private PawnFirefoamDrawer firefoamOverlays;

		// Token: 0x04002060 RID: 8288
		private PawnShamblerScarDrawer shamblerScarDrawer;

		// Token: 0x04002061 RID: 8289
		private PawnScariaSoreDrawer scariaSoreDrawer;

		// Token: 0x04002062 RID: 8290
		private Graphic_Shadow shadowGraphic;

		// Token: 0x04002063 RID: 8291
		public DamageFlasher flasher;

		// Token: 0x04002064 RID: 8292
		public PawnRenderTree renderTree;

		// Token: 0x04002065 RID: 8293
		private Graphic silhouetteGraphic;

		// Token: 0x04002066 RID: 8294
		private Vector3 silhouettePos;

		// Token: 0x04002067 RID: 8295
		private ThingDef statueStuff;

		// Token: 0x04002068 RID: 8296
		private Color? statuePaintColor;

		// Token: 0x04002069 RID: 8297
		private const float CarriedPawnDrawAngle = 70f;

		// Token: 0x0400206A RID: 8298
		private const float CachedPawnTextureMinCameraZoom = 18f;

		// Token: 0x0400206B RID: 8299
		private PawnRenderer.PreRenderResults results;

		// Token: 0x0400206C RID: 8300
		private readonly Dictionary<Apparel, ValueTuple<Color, bool>> tmpOriginalColors = new Dictionary<Apparel, ValueTuple<Color, bool>>();

		// Token: 0x020005F5 RID: 1525
		private struct PreRenderResults
		{
			// Token: 0x0400206D RID: 8301
			public bool valid;

			// Token: 0x0400206E RID: 8302
			public bool draw;

			// Token: 0x0400206F RID: 8303
			public bool useCached;

			// Token: 0x04002070 RID: 8304
			public bool showBody;

			// Token: 0x04002071 RID: 8305
			public float bodyAngle;

			// Token: 0x04002072 RID: 8306
			public PawnPosture posture;

			// Token: 0x04002073 RID: 8307
			public Vector3 bodyPos;

			// Token: 0x04002074 RID: 8308
			public PawnDrawParms parms;
		}
	}
}
