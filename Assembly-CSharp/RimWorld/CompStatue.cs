using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02002E57 RID: 11863
	public class CompStatue : CompArt
	{
		// Token: 0x17002ACB RID: 10955
		// (get) Token: 0x06010B21 RID: 68385 RVA: 0x004CFF79 File Offset: 0x004CE179
		public override bool Active
		{
			get
			{
				return this.fakePawn != null;
			}
		}

		// Token: 0x17002ACC RID: 10956
		// (get) Token: 0x06010B22 RID: 68386 RVA: 0x004CFF84 File Offset: 0x004CE184
		public new CompProperties_Statue Props
		{
			get
			{
				return (CompProperties_Statue)this.props;
			}
		}

		// Token: 0x17002ACD RID: 10957
		// (get) Token: 0x06010B23 RID: 68387 RVA: 0x004CFF91 File Offset: 0x004CE191
		public Graphic StatueBaseGraphic
		{
			get
			{
				return this.Props.statueBaseGraphic.GraphicColoredFor(this.parent);
			}
		}

		// Token: 0x06010B24 RID: 68388 RVA: 0x004CFFAC File Offset: 0x004CE1AC
		public static List<Pawn> GetAllPossiblePawnsToSelectFrom(Pawn crafter)
		{
			CompStatue.tmpPawnsToSelectFrom.Clear();
			CompStatue.tmpPawnsToSelectFrom.AddRange(SocialCardUtility.PawnsForSocialInfo(crafter));
			CompStatue.tmpPawnsToSelectFrom.AddRange(from p in crafter.relations.RelatedPawns
			where p.RaceProps.Humanlike && !p.Spawned
			select p);
			return CompStatue.tmpPawnsToSelectFrom;
		}

		// Token: 0x06010B25 RID: 68389 RVA: 0x004D0014 File Offset: 0x004CE214
		public static Thing GenerateSpecificStatue(Pawn crafter, Pawn subject)
		{
			Thing thing = ThingMaker.MakeThing(ThingDefOf.Statue, ThingDefOf.Steel);
			CompStatue compStatue = thing.TryGetComp<CompStatue>();
			compStatue.CreateSnapshotOfPawn(subject);
			compStatue.InitFakePawn();
			compStatue.titleInt = compStatue.GenerateTitle(ArtGenerationContext.Colony);
			compStatue.descriptionSeed = Rand.Int;
			subject.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.MadeStatueOfMe, crafter, null);
			return thing;
		}

		// Token: 0x06010B26 RID: 68390 RVA: 0x000026EA File Offset: 0x000008EA
		protected override void InitializeArtInternal(Thing relatedThing, ArtGenerationContext source)
		{
		}

		// Token: 0x06010B27 RID: 68391 RVA: 0x004D0080 File Offset: 0x004CE280
		private void InitializeAsFactionLeaderStatue()
		{
			List<Pawn> list = Find.WorldPawns.GetPawnsBySituation(WorldPawnSituation.FactionLeader).Where(delegate(Pawn p)
			{
				if (p.RaceProps.Humanlike && !p.IsSubhuman)
				{
					Faction faction = p.Faction;
					return faction != null && !faction.temporary && !faction.Hidden;
				}
				return false;
			}).ToList<Pawn>();
			if (list.NullOrEmpty<Pawn>())
			{
				Log.Error(string.Format("CompStatue at {0} could not find any faction leaders to become a statue of. Destroying. This is likely a bug.", this.parent.PositionHeld));
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			Pawn p2 = list.RandomElement<Pawn>();
			this.CreateSnapshotOfPawn(p2);
			this.InitFakePawn();
			this.titleInt = this.GenerateTitle(ArtGenerationContext.Outsider);
			this.descriptionSeed = Rand.Int;
		}

		// Token: 0x06010B28 RID: 68392 RVA: 0x004D0128 File Offset: 0x004CE328
		public override void JustCreatedBy(Pawn pawn)
		{
			base.JustCreatedBy(pawn);
			if (!base.CanShowArt)
			{
				return;
			}
			Pawn pawn2 = CompStatue.GetAllPossiblePawnsToSelectFrom(pawn).RandomElementByWeight((Pawn p) => CompStatue.sculptureTargetWeightByOpinionCurve.Evaluate((float)pawn.relations.OpinionOf(p)));
			if (pawn2 == null)
			{
				return;
			}
			Pawn_NeedsTracker needs = pawn2.needs;
			if (needs != null)
			{
				Need_Mood mood = needs.mood;
				if (mood != null)
				{
					ThoughtHandler thoughts = mood.thoughts;
					if (thoughts != null)
					{
						MemoryThoughtHandler memories = thoughts.memories;
						if (memories != null)
						{
							memories.TryGainMemory(ThoughtDefOf.MadeStatueOfMe, pawn, null);
						}
					}
				}
			}
			this.CreateSnapshotOfPawn(pawn2);
			this.InitFakePawn();
			this.titleInt = this.GenerateTitle(ArtGenerationContext.Colony);
			this.descriptionSeed = Rand.Int;
		}

		// Token: 0x06010B29 RID: 68393 RVA: 0x004D01E0 File Offset: 0x004CE3E0
		public override void PostPostGeneratedForTrader(TraderKindDef trader, PlanetTile forTile, Faction forFaction)
		{
			this.InitializeAsFactionLeaderStatue();
		}

		// Token: 0x06010B2A RID: 68394 RVA: 0x004D01E8 File Offset: 0x004CE3E8
		private void CreateSnapshotOfPawn(Pawn p)
		{
			this.bodyType = p.story.bodyType;
			this.headType = p.story.headType;
			this.hairDef = p.story.hairDef;
			this.hairColor = p.story.HairColor;
			Pawn_StyleTracker style = p.style;
			this.beard = ((style != null) ? style.beardDef : null);
			this.name = p.Name;
			this.gender = p.gender;
			this.lifestageIndex = p.ageTracker.CurLifeStageIndex;
			this.xenotype = p.genes.Xenotype;
			this.apparel.Clear();
			foreach (Apparel apparel in p.apparel.WornApparel)
			{
				if (!apparel.def.thingCategories.NotNullAndContains(ThingCategoryDefOf.ApparelUtility) && (!PawnApparelGenerator.IsHeadgear(apparel.def) || Rand.Bool))
				{
					this.apparel.Add(apparel.def);
				}
			}
			this.nonXenotypeGenes.Clear();
			if (ModsConfig.BiotechActive)
			{
				foreach (Gene gene in p.genes.GenesListForReading)
				{
					if (!this.xenotype.genes.Contains(gene.def))
					{
						this.nonXenotypeGenes.Add(gene.def);
					}
				}
			}
			this.hediffsWhichAffectRendering.Clear();
			foreach (Hediff hediff in p.health.hediffSet.hediffs)
			{
				if (hediff.def.HasDefinedGraphicProperties)
				{
					this.hediffsWhichAffectRendering.Add(new CompStatue.SavedHediffProps(hediff.Part, hediff.def, hediff.Severity));
				}
			}
			this.additionalSavedPawnDataForMods.Clear();
			this.CreateSnapshotOfPawn_HookForMods(p, this.additionalSavedPawnDataForMods);
		}

		// Token: 0x06010B2B RID: 68395 RVA: 0x000026EA File Offset: 0x000008EA
		[UsedImplicitly]
		private void CreateSnapshotOfPawn_HookForMods(Pawn p, Dictionary<string, object> dictToStoreDataIn)
		{
		}

		// Token: 0x06010B2C RID: 68396 RVA: 0x004D0430 File Offset: 0x004CE630
		private void InitFakePawn()
		{
			this.fakePawn = (Pawn)ThingMaker.MakeThing(ThingDefOf.Human, null);
			PawnComponentsUtility.CreateInitialComponents(this.fakePawn);
			this.fakePawn.Name = this.name;
			this.fakePawn.gender = this.gender;
			this.fakePawn.kindDef = PawnKindDefOf.Colonist;
			if (ModsConfig.BiotechActive)
			{
				this.fakePawn.genes.SetXenotype(this.xenotype ?? XenotypeDefOf.Baseliner);
			}
			this.fakePawn.ageTracker.LockCurrentLifeStageIndex(this.lifestageIndex);
			this.fakePawn.story.bodyType = this.bodyType;
			this.fakePawn.story.headType = this.headType;
			this.fakePawn.story.hairDef = this.hairDef;
			this.fakePawn.story.HairColor = this.hairColor;
			this.fakePawn.style.beardDef = (this.beard ?? BeardDefOf.NoBeard);
			this.fakePawn.apparel.DestroyAll(DestroyMode.Vanish);
			foreach (ThingDef thingDef in this.apparel)
			{
				Apparel item;
				if (thingDef.MadeFromStuff)
				{
					item = (Apparel)ThingMaker.MakeThing(thingDef, GenStuff.DefaultStuffFor(thingDef));
				}
				else
				{
					item = (Apparel)ThingMaker.MakeThing(thingDef, null);
				}
				this.fakePawn.apparel.WornApparel.Add(item);
			}
			if (!this.nonXenotypeGenes.NullOrEmpty<GeneDef>())
			{
				foreach (GeneDef geneDef in this.nonXenotypeGenes)
				{
					this.fakePawn.genes.AddGene(geneDef, true);
				}
			}
			if (!this.hediffsWhichAffectRendering.NullOrEmpty<CompStatue.SavedHediffProps>())
			{
				foreach (CompStatue.SavedHediffProps savedHediffProps in this.hediffsWhichAffectRendering)
				{
					this.fakePawn.health.AddHediff(savedHediffProps.hediffDef, savedHediffProps.bodyPart, null, null).Severity = savedHediffProps.severity;
				}
			}
			this.InitFakePawn_HookForMods(this.fakePawn, this.additionalSavedPawnDataForMods);
			this.fakePawn.Drawer.renderer.SetStatue(this.parent.Stuff);
			this.fakePawn.Drawer.renderer.SetAllGraphicsDirty();
			this.fakePawn.Drawer.renderer.EnsureGraphicsInitialized();
			this.Notify_ColorChanged();
		}

		// Token: 0x06010B2D RID: 68397 RVA: 0x000026EA File Offset: 0x000008EA
		[UsedImplicitly]
		private void InitFakePawn_HookForMods(Pawn fakePawn, Dictionary<string, object> additionalSavedPawnDataForMods)
		{
		}

		// Token: 0x06010B2E RID: 68398 RVA: 0x004D0710 File Offset: 0x004CE910
		public override TaggedString GenerateImageDescription()
		{
			if (this.fakePawn == null)
			{
				Log.ErrorOnce(string.Format("CompStatue at {0} trying to GenerateImageDescription before initializing pawn. This will not work. This message will not appear again for this statue.", this.parent.PositionHeld), this.parent.thingIDNumber ^ 61006258);
				return null;
			}
			GrammarRequest request = default(GrammarRequest);
			request.Includes.Add(this.Props.descriptionMaker);
			request.Rules.AddRange(GrammarUtility.RulesForPawn("SUBJECT", this.fakePawn, request.Constants, true, true));
			Rand.PushState();
			Rand.Seed = this.descriptionSeed;
			string str = GrammarResolver.Resolve("r_statue_description", request, string.Format("statue_description_{0}", this.name), false, null, null, null, true);
			Rand.PopState();
			return str;
		}

		// Token: 0x06010B2F RID: 68399 RVA: 0x004D07DC File Offset: 0x004CE9DC
		protected override string GenerateTitle(ArtGenerationContext context)
		{
			GrammarRequest request = default(GrammarRequest);
			request.Includes.Add(this.Props.nameMaker);
			request.Rules.AddRange(GrammarUtility.RulesForPawn("SUBJECT", this.fakePawn, request.Constants, true, true));
			return GrammarResolver.Resolve("r_statue_name", request, string.Format("statue_name_{0}", this.name), false, null, null, null, true);
		}

		// Token: 0x06010B30 RID: 68400 RVA: 0x004D0850 File Offset: 0x004CEA50
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (text == null)
			{
				return null;
			}
			string str = text;
			string str2 = "\n" + "Subject".Translate() + ": ";
			Name name = this.name;
			return str + str2 + ((name != null) ? name.ToString() : null);
		}

		// Token: 0x06010B31 RID: 68401 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool DontDrawParent()
		{
			return true;
		}

		// Token: 0x06010B32 RID: 68402 RVA: 0x004D08A8 File Offset: 0x004CEAA8
		public override void DrawAt(Vector3 drawPos, bool flip = false)
		{
			Vector3 loc = new Vector3(drawPos.x, drawPos.y - 0.03658537f, drawPos.z - 0.15f);
			this.StatueBaseGraphic.Draw(loc, flip ? this.parent.Rotation.Opposite : this.parent.Rotation, this.parent, 0f);
			if (this.fakePawn == null)
			{
				if (this.name != null)
				{
					Log.ErrorOnce(string.Format("CompStatue at {0} has chosen a target pawn but didn't successfully initialize for rendering. This is a bug. The statue will not render. This message will not appear again for this statue.", this.parent.PositionHeld), this.parent.thingIDNumber ^ 61010354);
					return;
				}
				this.InitializeAsFactionLeaderStatue();
			}
			float num = this.fakePawn.ageTracker.CurLifeStage.headSizeFactor ?? 1f;
			float d = (1f - num) * 0.5f;
			drawPos += Vector3.back * d;
			drawPos -= Altitudes.AltIncVect * 0.25f;
			this.fakePawn.Drawer.renderer.RenderPawnAt(drawPos, new Rot4?(Rot4.South), true);
		}

		// Token: 0x06010B33 RID: 68403 RVA: 0x004D09E4 File Offset: 0x004CEBE4
		public override void Notify_ColorChanged()
		{
			if (this.fakePawn == null)
			{
				return;
			}
			this.fakePawn.Drawer.renderer.SetStatue(this.parent.Stuff);
			Building building = this.parent as Building;
			if (building == null || building.PaintColorDef == null)
			{
				this.fakePawn.Drawer.renderer.SetStatuePaintColor(null);
			}
			else
			{
				this.fakePawn.Drawer.renderer.SetStatuePaintColor(new Color?(building.PaintColorDef.color));
			}
			this.fakePawn.Drawer.renderer.SetAllGraphicsDirty();
			this.fakePawn.Drawer.renderer.EnsureGraphicsInitialized();
		}

		// Token: 0x06010B34 RID: 68404 RVA: 0x004D0AA0 File Offset: 0x004CECA0
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<BodyTypeDef>(ref this.bodyType, "bodyType");
			Scribe_Defs.Look<HeadTypeDef>(ref this.headType, "headType");
			Scribe_Defs.Look<HairDef>(ref this.hairDef, "hairDef");
			Scribe_Defs.Look<XenotypeDef>(ref this.xenotype, "xenotype");
			Scribe_Defs.Look<BeardDef>(ref this.beard, "beard");
			Scribe_Collections.Look<ThingDef>(ref this.apparel, "apparel", LookMode.Def, Array.Empty<object>());
			Scribe_Deep.Look<Name>(ref this.name, "name", Array.Empty<object>());
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
			Scribe_Values.Look<int>(ref this.lifestageIndex, "lifestageIndex", 0, false);
			Scribe_Values.Look<Color>(ref this.hairColor, "hairColor", default(Color), false);
			Scribe_Values.Look<int>(ref this.descriptionSeed, "descriptionSeed", 0, false);
			Scribe_Collections.Look<GeneDef>(ref this.nonXenotypeGenes, "nonXenotypeGenes", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<CompStatue.SavedHediffProps>(ref this.hediffsWhichAffectRendering, "hediffsWhichAffectRendering", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<string, object>(ref this.additionalSavedPawnDataForMods, "additionalSavedPawnDataForMods", LookMode.Value, LookMode.Deep);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.descriptionSeed == 0)
				{
					this.descriptionSeed = Rand.Int;
				}
				if (this.additionalSavedPawnDataForMods == null)
				{
					this.additionalSavedPawnDataForMods = new Dictionary<string, object>();
				}
				LongEventHandler.ExecuteWhenFinished(new Action(this.InitFakePawn));
			}
		}

		// Token: 0x0400AE08 RID: 44552
		private static readonly SimpleCurve sculptureTargetWeightByOpinionCurve = new SimpleCurve(new CurvePoint[]
		{
			new CurvePoint(-100f, 1f),
			new CurvePoint(0f, 2f),
			new CurvePoint(100f, 10f)
		});

		// Token: 0x0400AE09 RID: 44553
		private static List<Pawn> tmpPawnsToSelectFrom = new List<Pawn>();

		// Token: 0x0400AE0A RID: 44554
		private Pawn fakePawn;

		// Token: 0x0400AE0B RID: 44555
		private BodyTypeDef bodyType;

		// Token: 0x0400AE0C RID: 44556
		private HeadTypeDef headType;

		// Token: 0x0400AE0D RID: 44557
		private HairDef hairDef;

		// Token: 0x0400AE0E RID: 44558
		private XenotypeDef xenotype;

		// Token: 0x0400AE0F RID: 44559
		public BeardDef beard;

		// Token: 0x0400AE10 RID: 44560
		private Color hairColor;

		// Token: 0x0400AE11 RID: 44561
		private List<ThingDef> apparel = new List<ThingDef>();

		// Token: 0x0400AE12 RID: 44562
		private List<GeneDef> nonXenotypeGenes = new List<GeneDef>();

		// Token: 0x0400AE13 RID: 44563
		private List<CompStatue.SavedHediffProps> hediffsWhichAffectRendering = new List<CompStatue.SavedHediffProps>();

		// Token: 0x0400AE14 RID: 44564
		private Name name;

		// Token: 0x0400AE15 RID: 44565
		private Gender gender;

		// Token: 0x0400AE16 RID: 44566
		private int lifestageIndex;

		// Token: 0x0400AE17 RID: 44567
		private int descriptionSeed;

		// Token: 0x0400AE18 RID: 44568
		private Dictionary<string, object> additionalSavedPawnDataForMods = new Dictionary<string, object>();

		// Token: 0x02002E58 RID: 11864
		public class SavedHediffProps : IExposable
		{
			// Token: 0x06010B37 RID: 68407 RVA: 0x004D0C99 File Offset: 0x004CEE99
			public SavedHediffProps(BodyPartRecord bodyPart, HediffDef hediffDef, float severity)
			{
				this.bodyPart = bodyPart;
				this.hediffDef = hediffDef;
				this.severity = severity;
			}

			// Token: 0x06010B38 RID: 68408 RVA: 0x000024DF File Offset: 0x000006DF
			public SavedHediffProps()
			{
			}

			// Token: 0x06010B39 RID: 68409 RVA: 0x004D0CB6 File Offset: 0x004CEEB6
			public void ExposeData()
			{
				Scribe_BodyParts.Look(ref this.bodyPart, "bodyPart", null);
				Scribe_Defs.Look<HediffDef>(ref this.hediffDef, "hediffDef");
				Scribe_Values.Look<float>(ref this.severity, "severity", 0f, false);
			}

			// Token: 0x0400AE19 RID: 44569
			public BodyPartRecord bodyPart;

			// Token: 0x0400AE1A RID: 44570
			public HediffDef hediffDef;

			// Token: 0x0400AE1B RID: 44571
			public float severity;
		}
	}
}
