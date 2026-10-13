using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020029FE RID: 10750
	public class Building_FleshmassHeart : Building
	{
		// Token: 0x170024C5 RID: 9413
		// (get) Token: 0x0600EDA2 RID: 60834 RVA: 0x0044EF60 File Offset: 0x0044D160
		private Graphic CenterPartGraphic
		{
			get
			{
				Graphic result;
				if ((result = this.cachedCenterPartGraphic) == null)
				{
					result = (this.cachedCenterPartGraphic = GraphicDatabase.Get<Graphic_Multi>("Things/Building/Fleshmass/FleshmassHeart/FleshmassHeart_CentralPart", ShaderDatabase.Cutout, Building_FleshmassHeart.PartDrawSize, Color.white));
				}
				return result;
			}
		}

		// Token: 0x170024C6 RID: 9414
		// (get) Token: 0x0600EDA3 RID: 60835 RVA: 0x0044EFA0 File Offset: 0x0044D1A0
		private Graphic LeftPartGraphic
		{
			get
			{
				Graphic result;
				if ((result = this.cachedLeftPartGraphic) == null)
				{
					result = (this.cachedLeftPartGraphic = GraphicDatabase.Get<Graphic_Multi>("Things/Building/Fleshmass/FleshmassHeart/FleshmassHeart_LeftPart", ShaderDatabase.Cutout, Building_FleshmassHeart.PartDrawSize, Color.white));
				}
				return result;
			}
		}

		// Token: 0x170024C7 RID: 9415
		// (get) Token: 0x0600EDA4 RID: 60836 RVA: 0x0044EFE0 File Offset: 0x0044D1E0
		private Graphic RightPartGraphic
		{
			get
			{
				Graphic result;
				if ((result = this.cachedRightPartGraphic) == null)
				{
					result = (this.cachedRightPartGraphic = GraphicDatabase.Get<Graphic_Multi>("Things/Building/Fleshmass/FleshmassHeart/FleshmassHeart_RightPart", ShaderDatabase.Cutout, Building_FleshmassHeart.PartDrawSize, Color.white));
				}
				return result;
			}
		}

		// Token: 0x170024C8 RID: 9416
		// (get) Token: 0x0600EDA5 RID: 60837 RVA: 0x0044F020 File Offset: 0x0044D220
		public CompBiosignatureOwner BiosignatureComp
		{
			get
			{
				CompBiosignatureOwner result;
				if ((result = this.biosignatureCompInt) == null)
				{
					result = (this.biosignatureCompInt = base.GetComp<CompBiosignatureOwner>());
				}
				return result;
			}
		}

		// Token: 0x170024C9 RID: 9417
		// (get) Token: 0x0600EDA6 RID: 60838 RVA: 0x0044F046 File Offset: 0x0044D246
		public int Biosignature
		{
			get
			{
				return this.BiosignatureComp.biosignature;
			}
		}

		// Token: 0x170024CA RID: 9418
		// (get) Token: 0x0600EDA7 RID: 60839 RVA: 0x0044F053 File Offset: 0x0044D253
		public object BiosignatureName
		{
			get
			{
				return this.BiosignatureComp.BiosignatureName;
			}
		}

		// Token: 0x170024CB RID: 9419
		// (get) Token: 0x0600EDA8 RID: 60840 RVA: 0x0044F060 File Offset: 0x0044D260
		public CompFleshmassHeart Comp
		{
			get
			{
				return base.GetComp<CompFleshmassHeart>();
			}
		}

		// Token: 0x170024CC RID: 9420
		// (get) Token: 0x0600EDA9 RID: 60841 RVA: 0x0044F068 File Offset: 0x0044D268
		public Lord DefendHeartLord
		{
			get
			{
				if (this.defendHeartLord == null)
				{
					this.defendHeartLord = LordMaker.MakeNewLord(base.Faction, new LordJob_DefendFleshmassHeart(this), base.Map, null);
				}
				return this.defendHeartLord;
			}
		}

		// Token: 0x0600EDAA RID: 60842 RVA: 0x0044F098 File Offset: 0x0044D298
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.bpmAccel, "bpmAccel", 0, false);
			Scribe_Values.Look<int>(ref this.bpm, "bpm", 0, false);
			Scribe_References.Look<Lord>(ref this.defendHeartLord, "defendHeartLord", false);
			Scribe_Values.Look<int>(ref this.overloadTick, "overloadTick", 0, false);
		}

		// Token: 0x0600EDAB RID: 60843 RVA: 0x0044F0F4 File Offset: 0x0044D2F4
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad && !base.BeingTransportedOnGravship)
			{
				Find.AnalysisManager.AddAnalysisTask(this.Biosignature, 3);
				if (base.Faction != Faction.OfEntities)
				{
					this.SetFaction(Faction.OfEntities, null);
				}
				EffecterDefOf.ImpactDustCloud.Spawn(base.Position, base.Map, 1f).Cleanup();
			}
		}

		// Token: 0x0600EDAC RID: 60844 RVA: 0x0044F160 File Offset: 0x0044D360
		public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
		{
			IntVec3 positionHeld = base.PositionHeld;
			Map mapHeld = base.MapHeld;
			base.Kill(dinfo, exactCulprit);
			Thing t;
			if (!GenDrop.TryDropSpawn(PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.FleshmassNucleus, Faction.OfEntities, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false)), positionHeld, mapHeld, ThingPlaceMode.Near, out t, null, null, true))
			{
				Log.ErrorOnce("Failed to place fleshmass nucleus", 9685694);
				return;
			}
			TaggedString label = "LetterLabelFleshmassHeartDestroyed".Translate();
			TaggedString text = "LetterFleshmassHeartDestroyed".Translate();
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.PositiveEvent, t, null, null, null, null, 0, true);
		}

		// Token: 0x0600EDAD RID: 60845 RVA: 0x0044F273 File Offset: 0x0044D473
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			FleshmassMapComponent component = base.Map.GetComponent<FleshmassMapComponent>();
			if (component != null)
			{
				component.DestroyFleshmass(60000, 1f, false, this);
			}
			base.Destroy(mode);
		}

		// Token: 0x0600EDAE RID: 60846 RVA: 0x0044F2A0 File Offset: 0x0044D4A0
		protected override void Tick()
		{
			base.Tick();
			if ((float)Find.TickManager.TicksGame > (float)this.lastBeatTick + 60f / (float)this.bpm * 60f)
			{
				this.Beat();
			}
			Thing.allowDestroyNonDestroyable = true;
			if (this.overloadTick > 0 && Find.TickManager.TicksGame > this.overloadTick)
			{
				this.Kill(null, null);
			}
			Thing.allowDestroyNonDestroyable = false;
		}

		// Token: 0x0600EDAF RID: 60847 RVA: 0x0044F318 File Offset: 0x0044D518
		private void Beat()
		{
			this.lastBeatTick = Find.TickManager.TicksGame;
			this.bpm += this.bpmAccel;
			SoundDefOf.FleshmassHeart_Throb.PlayOneShot(this);
		}

		// Token: 0x0600EDB0 RID: 60848 RVA: 0x0044F350 File Offset: 0x0044D550
		public void StartTachycardiacOverload()
		{
			this.bpm *= 2;
			this.bpmAccel = 15;
			this.overloadTick = Find.TickManager.TicksGame + EffecterDefOf.TachycardiacArrest.maintainTicks;
			EffecterDefOf.TachycardiacArrest.SpawnMaintained(base.Position, base.Map, 1f);
			Messages.Message("MessageHeartAttack".Translate(), this, MessageTypeDefOf.PositiveEvent, true);
		}

		// Token: 0x0600EDB1 RID: 60849 RVA: 0x0044F3CC File Offset: 0x0044D5CC
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			Matrix4x4 matrix = Matrix4x4.TRS(drawLoc + new Vector3(0f, 0.35f, -0.35f), Quaternion.AngleAxis(0f, Vector3.up), new Vector3(this.GetBeatScale(1.15f, 0), 1f, this.GetBeatScale(1.2f, 0)));
			GenDraw.DrawMeshNowOrLater(this.CenterPartGraphic.MeshAt(Rot4.South), matrix, this.CenterPartGraphic.MatSouth, false, null);
			matrix = Matrix4x4.TRS(drawLoc + new Vector3(-0.875f, 0.3f, 0f), Quaternion.AngleAxis(30f, Vector3.up), new Vector3(this.GetBeatScale(1.1f, 3), 1f, this.GetBeatScale(1.1f, 3)));
			GenDraw.DrawMeshNowOrLater(this.LeftPartGraphic.MeshAt(Rot4.South), matrix, this.LeftPartGraphic.MatSouth, false, null);
			matrix = Matrix4x4.TRS(drawLoc + new Vector3(0.875f, 0.3f, 0f), Quaternion.AngleAxis(10f, Vector3.up), new Vector3(this.GetBeatScale(1.1f, 7), 1f, this.GetBeatScale(1.1f, 7)));
			GenDraw.DrawMeshNowOrLater(this.RightPartGraphic.MeshAt(Rot4.South), matrix, this.RightPartGraphic.MatSouth, false, null);
		}

		// Token: 0x0600EDB2 RID: 60850 RVA: 0x0044F540 File Offset: 0x0044D740
		private float GetBeatScale(float maxScale, int delay = 0)
		{
			float num = (float)(Find.TickManager.TicksGame - (this.lastBeatTick + delay)) / 15f;
			if (num < 1f)
			{
				return Mathf.Lerp(1f, maxScale, Mathf.Sin(3.1415927f * num));
			}
			return 1f;
		}

		// Token: 0x0600EDB3 RID: 60851 RVA: 0x0044F58D File Offset: 0x0044D78D
		public override void PreSwapMap()
		{
			base.PreSwapMap();
			this.cachedLordPawns = new List<Pawn>();
			if (this.defendHeartLord != null)
			{
				this.cachedLordPawns.AddRange(this.defendHeartLord.ownedPawns);
			}
		}

		// Token: 0x0600EDB4 RID: 60852 RVA: 0x0044F5BE File Offset: 0x0044D7BE
		public override void PostSwapMap()
		{
			base.PostSwapMap();
			this.defendHeartLord = null;
			this.DefendHeartLord.AddPawns(this.cachedLordPawns, true);
			this.cachedLordPawns = null;
		}

		// Token: 0x0600EDB5 RID: 60853 RVA: 0x0044F5E8 File Offset: 0x0044D7E8
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLineIfNotEmpty().AppendLine("FleshmassHeartInvulnerable".Translate());
			stringBuilder.AppendLine(base.GetInspectString());
			stringBuilder.Append((this.Comp.GrowthPoints > 0) ? "FleshmassHeartGrowing".Translate() : "FleshmassHeartCollecting".Translate());
			return stringBuilder.ToString();
		}

		// Token: 0x04009D40 RID: 40256
		private const int BeatDurationTicks = 15;

		// Token: 0x04009D41 RID: 40257
		private static readonly Vector3 PartDrawSize = new Vector3(3.5f, 3.5f, 3.5f);

		// Token: 0x04009D42 RID: 40258
		private const int RequiredAnalysis = 3;

		// Token: 0x04009D43 RID: 40259
		private int bpm = 30;

		// Token: 0x04009D44 RID: 40260
		private int bpmAccel;

		// Token: 0x04009D45 RID: 40261
		private Lord defendHeartLord;

		// Token: 0x04009D46 RID: 40262
		private int overloadTick = -99999;

		// Token: 0x04009D47 RID: 40263
		[Unsaved(false)]
		private int lastBeatTick = -99999;

		// Token: 0x04009D48 RID: 40264
		[Unsaved(false)]
		private Graphic cachedCenterPartGraphic;

		// Token: 0x04009D49 RID: 40265
		[Unsaved(false)]
		private Graphic cachedLeftPartGraphic;

		// Token: 0x04009D4A RID: 40266
		[Unsaved(false)]
		private Graphic cachedRightPartGraphic;

		// Token: 0x04009D4B RID: 40267
		[Unsaved(false)]
		private CompBiosignatureOwner biosignatureCompInt;

		// Token: 0x04009D4C RID: 40268
		[Unsaved(false)]
		private List<Pawn> cachedLordPawns;
	}
}
