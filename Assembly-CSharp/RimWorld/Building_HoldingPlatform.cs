using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002A25 RID: 10789
	[StaticConstructorOnStartup]
	public class Building_HoldingPlatform : Building, IThingHolderWithDrawnPawn, IThingHolder, IRoofCollapseAlert, ISearchableContents
	{
		// Token: 0x17002523 RID: 9507
		// (get) Token: 0x0600EF21 RID: 61217 RVA: 0x004508A5 File Offset: 0x0044EAA5
		public float HeldPawnDrawPos_Y
		{
			get
			{
				return this.DrawPos.y + 0.03658537f;
			}
		}

		// Token: 0x17002524 RID: 9508
		// (get) Token: 0x0600EF22 RID: 61218 RVA: 0x00456934 File Offset: 0x00454B34
		public float HeldPawnBodyAngle
		{
			get
			{
				return base.Rotation.AsAngle;
			}
		}

		// Token: 0x17002525 RID: 9509
		// (get) Token: 0x0600EF23 RID: 61219 RVA: 0x00075162 File Offset: 0x00073362
		public PawnPosture HeldPawnPosture
		{
			get
			{
				return PawnPosture.LayingOnGroundFaceUp;
			}
		}

		// Token: 0x17002526 RID: 9510
		// (get) Token: 0x0600EF24 RID: 61220 RVA: 0x0045694F File Offset: 0x00454B4F
		public Rot4 HeldPawnRotation
		{
			get
			{
				return base.Rotation;
			}
		}

		// Token: 0x17002527 RID: 9511
		// (get) Token: 0x0600EF25 RID: 61221 RVA: 0x00456957 File Offset: 0x00454B57
		public Vector3 PawnDrawOffset
		{
			get
			{
				return new Vector3(0f, 0f, 0.15f);
			}
		}

		// Token: 0x17002528 RID: 9512
		// (get) Token: 0x0600EF26 RID: 61222 RVA: 0x0045696D File Offset: 0x00454B6D
		public Pawn HeldPawn
		{
			get
			{
				return this.innerContainer.FirstOrDefault((Thing x) => x is Pawn) as Pawn;
			}
		}

		// Token: 0x17002529 RID: 9513
		// (get) Token: 0x0600EF27 RID: 61223 RVA: 0x0045699E File Offset: 0x00454B9E
		public bool Occupied
		{
			get
			{
				return this.HeldPawn != null;
			}
		}

		// Token: 0x1700252A RID: 9514
		// (get) Token: 0x0600EF28 RID: 61224 RVA: 0x004569A9 File Offset: 0x00454BA9
		public float AnimationAlpha
		{
			get
			{
				return Mathf.Clamp01((float)(Find.TickManager.TicksGame - this.heldPawnStartTick) / 20f);
			}
		}

		// Token: 0x1700252B RID: 9515
		// (get) Token: 0x0600EF29 RID: 61225 RVA: 0x004569C8 File Offset: 0x00454BC8
		private CompAffectedByFacilities FacilitiesComp
		{
			get
			{
				CompAffectedByFacilities result;
				if ((result = this.facilitiesComp) == null)
				{
					result = (this.facilitiesComp = base.GetComp<CompAffectedByFacilities>());
				}
				return result;
			}
		}

		// Token: 0x1700252C RID: 9516
		// (get) Token: 0x0600EF2A RID: 61226 RVA: 0x004569F0 File Offset: 0x00454BF0
		private CompAttachPoints AttachPointsComp
		{
			get
			{
				CompAttachPoints result;
				if ((result = this.attachPointsComp) == null)
				{
					result = (this.attachPointsComp = base.GetComp<CompAttachPoints>());
				}
				return result;
			}
		}

		// Token: 0x1700252D RID: 9517
		// (get) Token: 0x0600EF2B RID: 61227 RVA: 0x00456A16 File Offset: 0x00454C16
		public ThingOwner SearchableContents
		{
			get
			{
				return this.innerContainer;
			}
		}

		// Token: 0x1700252E RID: 9518
		// (get) Token: 0x0600EF2C RID: 61228 RVA: 0x00456A20 File Offset: 0x00454C20
		private AttachPointTracker TargetPawnAttachPoints
		{
			get
			{
				if (this.targetPoints != null && this.targetPoints.ThingId != this.HeldPawn.ThingID)
				{
					this.targetPoints = null;
				}
				bool flag = this.targetPoints == null;
				AttachPointTracker attachPointTracker;
				if ((attachPointTracker = this.targetPoints) == null)
				{
					CompAttachPoints compAttachPoints = this.HeldPawn.TryGetComp<CompAttachPoints>();
					attachPointTracker = ((compAttachPoints != null) ? compAttachPoints.points : null);
				}
				this.targetPoints = attachPointTracker;
				if (flag)
				{
					foreach (HediffComp_AttachPoints hediffComp_AttachPoints in this.HeldPawn.health.hediffSet.GetHediffComps<HediffComp_AttachPoints>())
					{
						if (hediffComp_AttachPoints.Points != null)
						{
							if (this.targetPoints == null)
							{
								this.targetPoints = hediffComp_AttachPoints.Points;
							}
							else
							{
								this.targetPoints.Add(hediffComp_AttachPoints.Points, true);
							}
						}
					}
				}
				return this.targetPoints;
			}
		}

		// Token: 0x1700252F RID: 9519
		// (get) Token: 0x0600EF2D RID: 61229 RVA: 0x00456B0C File Offset: 0x00454D0C
		public bool HasAttachedElectroharvester
		{
			get
			{
				foreach (Thing thing in this.FacilitiesComp.LinkedFacilitiesListForReading)
				{
					CompPowerPlantElectroharvester compPowerPlantElectroharvester = thing.TryGetComp<CompPowerPlantElectroharvester>();
					if (compPowerPlantElectroharvester != null && compPowerPlantElectroharvester.PowerOn)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17002530 RID: 9520
		// (get) Token: 0x0600EF2E RID: 61230 RVA: 0x00456B74 File Offset: 0x00454D74
		public bool HasAttachedBioferriteHarvester
		{
			get
			{
				foreach (Thing thing in this.FacilitiesComp.LinkedFacilitiesListForReading)
				{
					Building_BioferriteHarvester building_BioferriteHarvester = thing as Building_BioferriteHarvester;
					if (building_BioferriteHarvester != null && building_BioferriteHarvester.Power.PowerOn)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17002531 RID: 9521
		// (get) Token: 0x0600EF2F RID: 61231 RVA: 0x00456BE4 File Offset: 0x00454DE4
		private Graphic ChainsUntetheredGraphic
		{
			get
			{
				if (this.chainsUntetheredGraphic == null)
				{
					this.chainsUntetheredGraphic = GraphicDatabase.Get<Graphic_Single>(this.PlatformProps.untetheredGraphicTexPath, ShaderDatabase.Cutout, this.def.graphicData.drawSize, Color.white);
				}
				return this.chainsUntetheredGraphic;
			}
		}

		// Token: 0x17002532 RID: 9522
		// (get) Token: 0x0600EF30 RID: 61232 RVA: 0x00456C24 File Offset: 0x00454E24
		private CompProperties_EntityHolderPlatform PlatformProps
		{
			get
			{
				return base.GetComp<CompEntityHolderPlatform>().Props;
			}
		}

		// Token: 0x0600EF31 RID: 61233 RVA: 0x00456C31 File Offset: 0x00454E31
		public Building_HoldingPlatform()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x0600EF32 RID: 61234 RVA: 0x00456C53 File Offset: 0x00454E53
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (!ModLister.CheckAnomaly("Holding platform"))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
			Find.StudyManager.UpdateStudiableCache(this, base.Map);
			Find.Anomaly.hasBuiltHoldingPlatform = true;
		}

		// Token: 0x0600EF33 RID: 61235 RVA: 0x00456C8D File Offset: 0x00454E8D
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.WillReplace)
			{
				this.EjectContents();
			}
			this.platformPoints = null;
			base.DeSpawn(mode);
		}

		// Token: 0x0600EF34 RID: 61236 RVA: 0x00456CA7 File Offset: 0x00454EA7
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600EF35 RID: 61237 RVA: 0x00456A16 File Offset: 0x00454C16
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x0600EF36 RID: 61238 RVA: 0x00456CB8 File Offset: 0x00454EB8
		private bool TryGetFirstColonistDirection(out Vector2 direction)
		{
			foreach (Thing thing in GenRadial.RadialDistinctThingsAround(base.Position, base.Map, 4f, false))
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && pawn.IsColonist)
				{
					direction = pawn.Position.ToVector2() - base.Position.ToVector2();
					return true;
				}
			}
			direction = Vector2.zero;
			return false;
		}

		// Token: 0x0600EF37 RID: 61239 RVA: 0x00456D58 File Offset: 0x00454F58
		protected override void Tick()
		{
			base.Tick();
			if (this.Occupied && this.chains == null && this.AttachPointsComp != null)
			{
				this.chains = ((this.TargetPawnAttachPoints != null) ? this.BuildTargetPointMapping() : this.DefaultPointMapping);
			}
			if (!this.Occupied && this.chains != null)
			{
				this.chains = null;
			}
			if (this.Occupied && this.HasAttachedElectroharvester && Rand.MTBEventOccurs(2f, 60000f, 1f))
			{
				this.HeldPawn.TakeDamage(new DamageInfo(DamageDefOf.ElectricalBurn, Building_HoldingPlatform.Damage.RandomInRange, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			}
			if (this.Occupied && Rand.MTBEventOccurs(100f, 1f, 1f))
			{
				this.UpdateAnimation();
			}
			if (this.debugEscapeTick > 0 && Find.TickManager.TicksGame == this.debugEscapeTick && this.HeldPawn != null)
			{
				CompHoldingPlatformTarget compHoldingPlatformTarget = this.HeldPawn.TryGetComp<CompHoldingPlatformTarget>();
				if (compHoldingPlatformTarget != null)
				{
					compHoldingPlatformTarget.Escape(false);
				}
			}
			if (this.heldPawnStartTick == -1 && this.HeldPawn != null)
			{
				this.heldPawnStartTick = Find.TickManager.TicksGame;
				return;
			}
			if (this.HeldPawn == null)
			{
				this.heldPawnStartTick = -1;
			}
		}

		// Token: 0x0600EF38 RID: 61240 RVA: 0x00456EA4 File Offset: 0x004550A4
		private void UpdateAnimation()
		{
			CompHoldingPlatformTarget compHoldingPlatformTarget;
			if (this.HeldPawn.TryGetComp(out compHoldingPlatformTarget) && (!compHoldingPlatformTarget.Props.hasAnimation || this.HeldPawn.health.Downed))
			{
				this.HeldPawn.Drawer.renderer.SetAnimation(null);
				return;
			}
			SoundDef soundDef = this.PlatformProps.entityLungeSoundLow;
			AnimationDef animationDef = AnimationDefOf.HoldingPlatformWiggleLight;
			Vector2 vector;
			if (this.TryGetFirstColonistDirection(out vector))
			{
				if (this.TargetPawnAttachPoints != null && Rand.Chance(0.25f))
				{
					Vector2 lhs = vector.normalized.Cardinalize();
					if (lhs == Vector2.up)
					{
						animationDef = AnimationDefOf.HoldingPlatformLungeUp;
					}
					if (lhs == Vector2.right)
					{
						animationDef = AnimationDefOf.HoldingPlatformLungeRight;
					}
					if (lhs == Vector2.left)
					{
						animationDef = AnimationDefOf.HoldingPlatformLungeLeft;
					}
					if (lhs == Vector2.down)
					{
						animationDef = AnimationDefOf.HoldingPlatformLungeDown;
					}
					soundDef = this.PlatformProps.entityLungeSoundHi;
				}
				else
				{
					animationDef = AnimationDefOf.HoldingPlatformWiggleIntense;
				}
			}
			if (this.HeldPawn.Drawer.renderer.CurAnimation != animationDef)
			{
				if (soundDef != null)
				{
					soundDef.PlayOneShot(this);
				}
				this.HeldPawn.Drawer.renderer.SetAnimation(animationDef);
			}
		}

		// Token: 0x17002533 RID: 9523
		// (get) Token: 0x0600EF39 RID: 61241 RVA: 0x00456FD0 File Offset: 0x004551D0
		public PawnDrawParms HeldPawnDrawParms
		{
			get
			{
				return new PawnDrawParms
				{
					pawn = this.HeldPawn,
					facing = this.HeldPawn.Rotation,
					rotDrawMode = RotDrawMode.Fresh,
					posture = this.HeldPawn.GetPosture(),
					flags = (PawnRenderFlags.Headgear | PawnRenderFlags.Clothes),
					tint = Color.white
				};
			}
		}

		// Token: 0x0600EF3A RID: 61242 RVA: 0x00457034 File Offset: 0x00455234
		public List<Building_HoldingPlatform.Chain> BuildTargetPointMapping()
		{
			if (this.chains == null)
			{
				this.chains = new List<Building_HoldingPlatform.Chain>();
			}
			else
			{
				this.chains.Clear();
			}
			Vector3 a;
			Vector3 vector;
			Quaternion rotation;
			Vector3 vector2;
			this.HeldPawn.Drawer.renderer.renderTree.GetRootTPRS(this.HeldPawnDrawParms, out a, out vector, out rotation, out vector2);
			Vector3 a2 = this.DrawPos + this.PawnDrawOffset;
			Dictionary<AttachPointType, Vector3> dictionary = new Dictionary<AttachPointType, Vector3>();
			int num = 5;
			int num2 = 8;
			foreach (AttachPointType attachPointType in this.TargetPawnAttachPoints.PointTypes(num, num2))
			{
				Vector3 value = a2 + rotation * (a + this.TargetPawnAttachPoints.GetRotatedOffset(attachPointType, base.Rotation));
				dictionary.Add(attachPointType, value);
			}
			for (int i = num; i <= num2; i++)
			{
				Vector3 vector3 = this.GetPlatformPoints()[(AttachPointType)i];
				Vector3 vector4 = dictionary[(AttachPointType)i];
				Vector3 vector5 = Vector3.Lerp(vector3, vector4, this.AnimationAlpha);
				float x = Vector3.Distance(vector5, vector3);
				Vector2 vector6 = new Vector2(x, 1f);
				this.chains.Add(new Building_HoldingPlatform.Chain
				{
					from = vector3,
					to = vector5,
					graphic = (GraphicDatabase.Get<Graphic_Tiling>(this.PlatformProps.tilingChainTexPath, ShaderTypeDefOf.CutoutTiling.Shader, vector6, Color.white) as Graphic_Tiling).WithTiling(vector6),
					baseFastenerGraphic = GraphicDatabase.Get<Graphic_Single>(this.PlatformProps.baseChainFastenerTexPath, ShaderTypeDefOf.CutoutTiling.Shader, Vector2.one, Color.white),
					targetFastenerGraphic = GraphicDatabase.Get<Graphic_Single>(this.PlatformProps.targetChainFastenerTexPath, ShaderTypeDefOf.CutoutTiling.Shader, Vector2.one, Color.white),
					rotation = (vector4.WithY(0f) - vector3.WithY(0f)).normalized.ToAngleFlat()
				});
			}
			return this.chains;
		}

		// Token: 0x0600EF3B RID: 61243 RVA: 0x0045726C File Offset: 0x0045546C
		private Dictionary<AttachPointType, Vector3> GetPlatformPoints()
		{
			if (this.platformPoints == null)
			{
				this.platformPoints = new Dictionary<AttachPointType, Vector3>();
				int min = 5;
				int max = 8;
				foreach (AttachPointType attachPointType in this.AttachPointsComp.points.PointTypes(min, max))
				{
					this.platformPoints.Add(attachPointType, this.AttachPointsComp.points.GetWorldPos(attachPointType));
				}
			}
			return this.platformPoints;
		}

		// Token: 0x17002534 RID: 9524
		// (get) Token: 0x0600EF3C RID: 61244 RVA: 0x004572F8 File Offset: 0x004554F8
		public List<Building_HoldingPlatform.Chain> DefaultPointMapping
		{
			get
			{
				if (this.defaultPointMapping == null)
				{
					this.defaultPointMapping = new List<Building_HoldingPlatform.Chain>();
					Vector3 worldPos = this.AttachPointsComp.points.GetWorldPos(AttachPointType.PlatformRestraint0);
					Vector3 worldPos2 = this.AttachPointsComp.points.GetWorldPos(AttachPointType.PlatformRestraint1);
					Vector3 worldPos3 = this.AttachPointsComp.points.GetWorldPos(AttachPointType.PlatformRestraint2);
					Vector3 worldPos4 = this.AttachPointsComp.points.GetWorldPos(AttachPointType.PlatformRestraint3);
					Vector2 vector = new Vector2(Vector3.Distance(worldPos, worldPos3), 1f);
					this.defaultPointMapping.Add(new Building_HoldingPlatform.Chain
					{
						from = worldPos,
						to = worldPos3,
						graphic = (GraphicDatabase.Get<Graphic_Tiling>(this.PlatformProps.tilingChainTexPath, ShaderTypeDefOf.Cutout.Shader, vector, Color.white) as Graphic_Tiling).WithTiling(vector),
						baseFastenerGraphic = GraphicDatabase.Get<Graphic_Single>(this.PlatformProps.baseChainFastenerTexPath, ShaderTypeDefOf.Cutout.Shader, Vector2.one, Color.white),
						targetFastenerGraphic = GraphicDatabase.Get<Graphic_Single>(this.PlatformProps.targetChainFastenerTexPath, ShaderTypeDefOf.Cutout.Shader, Vector2.one, Color.white),
						rotation = (worldPos3.WithY(0f) - worldPos.WithY(0f)).normalized.ToAngleFlat()
					});
					vector = new Vector2(Vector3.Distance(worldPos2, worldPos4), 1f);
					this.defaultPointMapping.Add(new Building_HoldingPlatform.Chain
					{
						from = worldPos2,
						to = worldPos4,
						graphic = (GraphicDatabase.Get<Graphic_Tiling>(this.PlatformProps.tilingChainTexPath, ShaderTypeDefOf.Cutout.Shader, vector, Color.white) as Graphic_Tiling).WithTiling(vector),
						baseFastenerGraphic = GraphicDatabase.Get<Graphic_Single>(this.PlatformProps.baseChainFastenerTexPath, ShaderTypeDefOf.Cutout.Shader, Vector2.one, Color.white),
						targetFastenerGraphic = GraphicDatabase.Get<Graphic_Single>(this.PlatformProps.targetChainFastenerTexPath, ShaderTypeDefOf.Cutout.Shader, Vector2.one, Color.white),
						rotation = (worldPos4.WithY(0f) - worldPos2.WithY(0f)).normalized.ToAngleFlat()
					});
				}
				return this.defaultPointMapping;
			}
		}

		// Token: 0x0600EF3D RID: 61245 RVA: 0x00457550 File Offset: 0x00455750
		public override void DynamicDrawPhaseAt(DrawPhase phase, Vector3 drawLoc, bool flip = false)
		{
			base.DynamicDrawPhaseAt(phase, drawLoc, flip);
			Pawn heldPawn = this.HeldPawn;
			if (heldPawn != null)
			{
				Rot4 value = Rot4.South;
				if (heldPawn.IsShambler && heldPawn.RaceProps.Animal)
				{
					value = Rot4.East;
				}
				heldPawn.Drawer.renderer.DynamicDrawPhaseAt(phase, this.DrawPos + this.PawnDrawOffset, new Rot4?(value), true);
			}
		}

		// Token: 0x0600EF3E RID: 61246 RVA: 0x004575BC File Offset: 0x004557BC
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			if (this.HeldPawn != null)
			{
				this.DrawChains();
				return;
			}
			this.ChainsUntetheredGraphic.Draw(drawLoc + Vector3.up * 0.05f, base.Rotation, this, 0f);
		}

		// Token: 0x0600EF3F RID: 61247 RVA: 0x0045760C File Offset: 0x0045580C
		private void DrawChains()
		{
			if (this.chains != null)
			{
				this.chains = ((this.TargetPawnAttachPoints != null) ? this.BuildTargetPointMapping() : this.DefaultPointMapping);
				Vector3 vector = Vector3.up * 0.13658537f;
				foreach (Building_HoldingPlatform.Chain chain in this.chains)
				{
					Vector3 v = (chain.from + chain.to) / 2f;
					chain.graphic.Draw(v.WithY(this.DrawPos.y) + vector, base.Rotation, this, chain.rotation + 180f);
					chain.targetFastenerGraphic.Draw(chain.to + 2f * vector, base.Rotation, this, chain.rotation + 90f);
					chain.baseFastenerGraphic.Draw(chain.from + 2f * vector, base.Rotation, this, chain.rotation + 90f);
				}
			}
		}

		// Token: 0x0600EF40 RID: 61248 RVA: 0x00457750 File Offset: 0x00455950
		public void EjectContents()
		{
			this.defaultPointMapping = null;
			this.chains = null;
			Pawn heldPawn = this.HeldPawn;
			if (heldPawn != null)
			{
				heldPawn.Drawer.renderer.SetAnimation(null);
			}
			Pawn heldPawn2 = this.HeldPawn;
			if (heldPawn2 != null)
			{
				CompHoldingPlatformTarget comp = heldPawn2.GetComp<CompHoldingPlatformTarget>();
				if (comp != null)
				{
					comp.Notify_ReleasedFromPlatform();
				}
			}
			this.innerContainer.TryDropAll(base.Position, base.Map, ThingPlaceMode.Near, null, null, true);
		}

		// Token: 0x0600EF41 RID: 61249 RVA: 0x004577BF File Offset: 0x004559BF
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(selPawn))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (this.Occupied)
			{
				foreach (FloatMenuOption floatMenuOption2 in this.HeldPawn.GetFloatMenuOptions(selPawn))
				{
					yield return floatMenuOption2;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600EF42 RID: 61250 RVA: 0x004577D8 File Offset: 0x004559D8
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			Pawn heldPawn = this.HeldPawn;
			if (heldPawn != null)
			{
				TaggedString taggedString = "HoldingThing".Translate() + ": " + heldPawn.NameShortColored.CapitalizeFirst();
				bool flag = this.SafelyContains(heldPawn);
				if (!flag)
				{
					taggedString += " (" + "HoldingPlatformRequiresStrength".Translate(StatDefOf.MinimumContainmentStrength.Worker.ValueToString(heldPawn.GetStatValue(StatDefOf.MinimumContainmentStrength, true, -1), false, ToStringNumberSense.Absolute)) + ")";
				}
				text += taggedString.Colorize(flag ? Color.white : ColorLibrary.RedReadable);
			}
			else
			{
				text += "HoldingThing".Translate() + ": " + "Nothing".Translate().CapitalizeFirst();
			}
			if (heldPawn != null && heldPawn.def.IsStudiable)
			{
				string inspectStringExtraFor = CompStudiable.GetInspectStringExtraFor(heldPawn);
				if (!inspectStringExtraFor.NullOrEmpty())
				{
					text = text + "\n" + inspectStringExtraFor;
				}
			}
			CompProducesBioferrite compProducesBioferrite;
			if (heldPawn != null && heldPawn.TryGetComp(out compProducesBioferrite))
			{
				string text2 = compProducesBioferrite.CompInspectStringExtra();
				if (!text2.NullOrEmpty())
				{
					text = text + "\n" + text2;
				}
			}
			return text;
		}

		// Token: 0x0600EF43 RID: 61251 RVA: 0x0045793E File Offset: 0x00455B3E
		public override IEnumerable<InspectTabBase> GetInspectTabs()
		{
			foreach (InspectTabBase inspectTabBase in base.GetInspectTabs())
			{
				yield return inspectTabBase;
			}
			IEnumerator<InspectTabBase> enumerator = null;
			if (this.HeldPawn != null && this.HeldPawn.def.inspectorTabs.Contains(typeof(ITab_StudyNotes)))
			{
				yield return this.HeldPawn.GetInspectTabs().FirstOrDefault((InspectTabBase tab) => tab is ITab_StudyNotes);
			}
			yield break;
			yield break;
		}

		// Token: 0x0600EF44 RID: 61252 RVA: 0x00457950 File Offset: 0x00455B50
		public void Notify_PawnDied(Pawn pawn, DamageInfo? dinfo)
		{
			if (pawn == this.HeldPawn)
			{
				this.innerContainer.TryDropAll(base.Position, base.Map, ThingPlaceMode.Near, null, null, true);
				if (dinfo == null || !dinfo.Value.Def.execution)
				{
					Messages.Message("EntityDiedOnHoldingPlatform".Translate(pawn), pawn, MessageTypeDefOf.NegativeEvent, true);
				}
			}
		}

		// Token: 0x0600EF45 RID: 61253 RVA: 0x004579C7 File Offset: 0x00455BC7
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			CompActivity compActivity;
			if (this.HeldPawn != null && this.HeldPawn.TryGetComp(out compActivity))
			{
				foreach (Gizmo gizmo2 in compActivity.CompGetGizmosExtra())
				{
					yield return gizmo2;
				}
				enumerator = null;
			}
			CompStudiable compStudiable;
			if (this.HeldPawn != null && this.HeldPawn.TryGetComp(out compStudiable))
			{
				foreach (Gizmo gizmo3 in compStudiable.CompGetGizmosExtra())
				{
					yield return gizmo3;
				}
				enumerator = null;
			}
			CompHoldingPlatformTarget compHoldingPlatformTarget;
			if (this.HeldPawn != null && this.HeldPawn.TryGetComp(out compHoldingPlatformTarget))
			{
				foreach (Gizmo gizmo4 in compHoldingPlatformTarget.CompGetGizmosExtra())
				{
					yield return gizmo4;
				}
				enumerator = null;
			}
			foreach (Thing item in ((IEnumerable<Thing>)this.innerContainer))
			{
				Gizmo gizmo5 = Building.SelectContainedItemGizmo(this, item);
				if (gizmo5 != null)
				{
					yield return gizmo5;
				}
			}
			IEnumerator<Thing> enumerator2 = null;
			if (DebugSettings.ShowDevGizmos && this.HeldPawn != null)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Timed escape",
					action = delegate()
					{
						List<FloatMenuOption> list = new List<FloatMenuOption>();
						for (int i = 1; i < 21; i++)
						{
							int delay = i * 60;
							list.Add(new FloatMenuOption(delay.TicksToSeconds().ToString() + "s", delegate()
							{
								this.debugEscapeTick = Find.TickManager.TicksGame + delay;
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
						Find.WindowStack.Add(new FloatMenu(list));
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600EF46 RID: 61254 RVA: 0x004579D8 File Offset: 0x00455BD8
		public RoofCollapseResponse Notify_OnBeforeRoofCollapse()
		{
			if (!this.Occupied)
			{
				return RoofCollapseResponse.None;
			}
			IRoofCollapseAlert roofCollapseAlert = this.HeldPawn as IRoofCollapseAlert;
			if (roofCollapseAlert != null)
			{
				roofCollapseAlert.Notify_OnBeforeRoofCollapse();
			}
			foreach (IRoofCollapseAlert roofCollapseAlert2 in this.HeldPawn.GetComps<IRoofCollapseAlert>())
			{
				roofCollapseAlert2.Notify_OnBeforeRoofCollapse();
			}
			return RoofCollapseResponse.None;
		}

		// Token: 0x0600EF47 RID: 61255 RVA: 0x00457A4C File Offset: 0x00455C4C
		public override void Notify_DefsHotReloaded()
		{
			base.Notify_DefsHotReloaded();
			this.chains = null;
		}

		// Token: 0x0600EF48 RID: 61256 RVA: 0x00457A5C File Offset: 0x00455C5C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastDamaged, "lastDamaged", 0, false);
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<int>(ref this.heldPawnStartTick, "heldPawnStartTick", 0, false);
		}

		// Token: 0x04009E10 RID: 40464
		public ThingOwner innerContainer;

		// Token: 0x04009E11 RID: 40465
		private int lastDamaged;

		// Token: 0x04009E12 RID: 40466
		private Graphic chainsUntetheredGraphic;

		// Token: 0x04009E13 RID: 40467
		private List<Building_HoldingPlatform.Chain> chains;

		// Token: 0x04009E14 RID: 40468
		private CompAffectedByFacilities facilitiesComp;

		// Token: 0x04009E15 RID: 40469
		private CompAttachPoints attachPointsComp;

		// Token: 0x04009E16 RID: 40470
		private AttachPointTracker targetPoints;

		// Token: 0x04009E17 RID: 40471
		private List<Building_HoldingPlatform.Chain> defaultPointMapping;

		// Token: 0x04009E18 RID: 40472
		[Unsaved(false)]
		private int debugEscapeTick = -1;

		// Token: 0x04009E19 RID: 40473
		private int heldPawnStartTick = -1;

		// Token: 0x04009E1A RID: 40474
		private const float ChainsUntetheredYOffset = 0.05f;

		// Token: 0x04009E1B RID: 40475
		private const float ChainsTetheredYOffset = 0.13658537f;

		// Token: 0x04009E1C RID: 40476
		private const float LurchMTBTicks = 100f;

		// Token: 0x04009E1D RID: 40477
		private const float DamageMTBDays = 2f;

		// Token: 0x04009E1E RID: 40478
		private static readonly FloatRange Damage = new FloatRange(1f, 3f);

		// Token: 0x04009E1F RID: 40479
		private const float LungeAnimationChance = 0.25f;

		// Token: 0x04009E20 RID: 40480
		private Dictionary<AttachPointType, Vector3> platformPoints;

		// Token: 0x02002A26 RID: 10790
		public struct Chain
		{
			// Token: 0x04009E21 RID: 40481
			public Vector3 from;

			// Token: 0x04009E22 RID: 40482
			public Vector3 to;

			// Token: 0x04009E23 RID: 40483
			public Graphic graphic;

			// Token: 0x04009E24 RID: 40484
			public Graphic baseFastenerGraphic;

			// Token: 0x04009E25 RID: 40485
			public Graphic targetFastenerGraphic;

			// Token: 0x04009E26 RID: 40486
			public float rotation;
		}
	}
}
