using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020029F5 RID: 10741
	public class Building_Door : Building
	{
		// Token: 0x17002497 RID: 9367
		// (get) Token: 0x0600ED19 RID: 60697 RVA: 0x0044D0CF File Offset: 0x0044B2CF
		private int CloseDelayAdjusted
		{
			get
			{
				return Mathf.FloorToInt(110f * (this.DoorPowerOn ? this.def.building.poweredDoorCloseSpeedFactor : this.def.building.unpoweredDoorCloseSpeedFactor));
			}
		}

		// Token: 0x17002498 RID: 9368
		// (get) Token: 0x0600ED1A RID: 60698 RVA: 0x0044D106 File Offset: 0x0044B306
		private int WillCloseSoonThreshold
		{
			get
			{
				return this.CloseDelayAdjusted + 1;
			}
		}

		// Token: 0x17002499 RID: 9369
		// (get) Token: 0x0600ED1B RID: 60699 RVA: 0x0044D110 File Offset: 0x0044B310
		public bool Open
		{
			get
			{
				return this.openInt;
			}
		}

		// Token: 0x1700249A RID: 9370
		// (get) Token: 0x0600ED1C RID: 60700 RVA: 0x0044D118 File Offset: 0x0044B318
		public bool HoldOpen
		{
			get
			{
				return this.holdOpenInt;
			}
		}

		// Token: 0x1700249B RID: 9371
		// (get) Token: 0x0600ED1D RID: 60701 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool CheckFaction
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700249C RID: 9372
		// (get) Token: 0x0600ED1E RID: 60702 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool CanDrawMovers
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700249D RID: 9373
		// (get) Token: 0x0600ED1F RID: 60703 RVA: 0x0044D120 File Offset: 0x0044B320
		public override bool ExchangeVacuum
		{
			get
			{
				return base.ExchangeVacuum || this.Open;
			}
		}

		// Token: 0x1700249E RID: 9374
		// (get) Token: 0x0600ED20 RID: 60704 RVA: 0x0044D132 File Offset: 0x0044B332
		protected virtual float TempEqualizeRate
		{
			get
			{
				return this.def.building.doorTempEqualizeRate;
			}
		}

		// Token: 0x1700249F RID: 9375
		// (get) Token: 0x0600ED21 RID: 60705 RVA: 0x0044D144 File Offset: 0x0044B344
		public virtual bool FreePassage
		{
			get
			{
				return this.openInt && (this.holdOpenInt || !this.WillCloseSoon);
			}
		}

		// Token: 0x170024A0 RID: 9376
		// (get) Token: 0x0600ED22 RID: 60706 RVA: 0x0044D164 File Offset: 0x0044B364
		public int TicksTillFullyOpened
		{
			get
			{
				int num = this.TicksToOpenNow - this.ticksSinceOpen;
				if (num < 0)
				{
					num = 0;
				}
				return num;
			}
		}

		// Token: 0x170024A1 RID: 9377
		// (get) Token: 0x0600ED23 RID: 60707 RVA: 0x0044D188 File Offset: 0x0044B388
		public bool WillCloseSoon
		{
			get
			{
				if (!base.Spawned)
				{
					return true;
				}
				if (!this.openInt)
				{
					return true;
				}
				if (this.holdOpenInt)
				{
					return false;
				}
				if (this.ticksUntilClose > 0 && this.ticksUntilClose <= this.WillCloseSoonThreshold && !this.BlockedOpenMomentary)
				{
					return true;
				}
				if (this.CanTryCloseAutomatically && !this.BlockedOpenMomentary)
				{
					return true;
				}
				foreach (IntVec3 intVec in this.OccupiedRect())
				{
					for (int i = 0; i < 5; i++)
					{
						IntVec3 c = intVec + GenAdj.CardinalDirectionsAndInside[i];
						if (c.InBounds(base.Map))
						{
							List<Thing> thingList = c.GetThingList(base.Map);
							for (int j = 0; j < thingList.Count; j++)
							{
								Pawn pawn = thingList[j] as Pawn;
								if (pawn != null && !pawn.HostileTo(this) && !pawn.Downed && this.PawnCanOpen(pawn) && (pawn.Position == intVec || (pawn.pather.Moving && pawn.pather.nextCell == intVec)))
								{
									return true;
								}
							}
						}
					}
				}
				return false;
			}
		}

		// Token: 0x170024A2 RID: 9378
		// (get) Token: 0x0600ED24 RID: 60708 RVA: 0x0044D2F8 File Offset: 0x0044B4F8
		public bool ContainmentBreached
		{
			get
			{
				return this.openInt && this.ticksOpen >= 600;
			}
		}

		// Token: 0x170024A3 RID: 9379
		// (get) Token: 0x0600ED25 RID: 60709 RVA: 0x0044D314 File Offset: 0x0044B514
		public bool BlockedOpenMomentary
		{
			get
			{
				if (this.StuckOpen)
				{
					return true;
				}
				foreach (IntVec3 c in this.OccupiedRect())
				{
					List<Thing> thingList = c.GetThingList(base.Map);
					for (int i = 0; i < thingList.Count; i++)
					{
						Thing thing = thingList[i];
						if (thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Pawn || thing is Corpse)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x170024A4 RID: 9380
		// (get) Token: 0x0600ED26 RID: 60710 RVA: 0x00002501 File Offset: 0x00000701
		protected virtual bool AlwaysOpen
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170024A5 RID: 9381
		// (get) Token: 0x0600ED27 RID: 60711 RVA: 0x0044D3C4 File Offset: 0x0044B5C4
		protected bool StuckOpen
		{
			get
			{
				if (this.AlwaysOpen)
				{
					return true;
				}
				if (!base.Spawned || this.def.size == IntVec2.One)
				{
					return false;
				}
				using (IEnumerator<IntVec3> enumerator = DoorUtility.WallRequirementCells(this.def, base.Position, base.Rotation).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!DoorUtility.EncapsulatingWallAt(enumerator.Current, base.Map, false))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x170024A6 RID: 9382
		// (get) Token: 0x0600ED28 RID: 60712 RVA: 0x0044D45C File Offset: 0x0044B65C
		public bool DoorPowerOn
		{
			get
			{
				return this.powerComp != null && this.powerComp.PowerOn;
			}
		}

		// Token: 0x170024A7 RID: 9383
		// (get) Token: 0x0600ED29 RID: 60713 RVA: 0x0044D473 File Offset: 0x0044B673
		public bool SlowsPawns
		{
			get
			{
				return !this.DoorPowerOn || this.TicksToOpenNow > 20;
			}
		}

		// Token: 0x170024A8 RID: 9384
		// (get) Token: 0x0600ED2A RID: 60714 RVA: 0x0044D48C File Offset: 0x0044B68C
		public int TicksToOpenNow
		{
			get
			{
				float num = 45f / this.GetStatValue(StatDefOf.DoorOpenSpeed, true, -1);
				if (this.DoorPowerOn)
				{
					num *= 0.25f * this.def.building.poweredDoorOpenSpeedFactor;
				}
				else
				{
					num *= this.def.building.unpoweredDoorOpenSpeedFactor;
				}
				return Mathf.RoundToInt(num);
			}
		}

		// Token: 0x170024A9 RID: 9385
		// (get) Token: 0x0600ED2B RID: 60715 RVA: 0x0044D4E9 File Offset: 0x0044B6E9
		private bool CanTryCloseAutomatically
		{
			get
			{
				return this.FriendlyTouchedRecently && !this.HoldOpen;
			}
		}

		// Token: 0x170024AA RID: 9386
		// (get) Token: 0x0600ED2C RID: 60716 RVA: 0x0044D4FE File Offset: 0x0044B6FE
		private bool FriendlyTouchedRecently
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastFriendlyTouchTick + 120;
			}
		}

		// Token: 0x170024AB RID: 9387
		// (get) Token: 0x0600ED2D RID: 60717 RVA: 0x0044D515 File Offset: 0x0044B715
		public override bool FireBulwark
		{
			get
			{
				return !this.Open && base.FireBulwark;
			}
		}

		// Token: 0x170024AC RID: 9388
		// (get) Token: 0x0600ED2E RID: 60718 RVA: 0x0044D527 File Offset: 0x0044B727
		protected virtual float OpenPct
		{
			get
			{
				return Mathf.Clamp01((float)this.ticksSinceOpen / (float)this.TicksToOpenNow);
			}
		}

		// Token: 0x0600ED2F RID: 60719 RVA: 0x0044D53D File Offset: 0x0044B73D
		public override void PostMake()
		{
			base.PostMake();
			this.powerComp = base.GetComp<CompPowerTrader>();
		}

		// Token: 0x0600ED30 RID: 60720 RVA: 0x0044D551 File Offset: 0x0044B751
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.powerComp = base.GetComp<CompPowerTrader>();
			this.ClearReachabilityCache(map);
			if (this.BlockedOpenMomentary)
			{
				this.DoorOpen(110);
			}
		}

		// Token: 0x0600ED31 RID: 60721 RVA: 0x0044D580 File Offset: 0x0044B780
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			this.ClearReachabilityCache(map);
		}

		// Token: 0x0600ED32 RID: 60722 RVA: 0x0044D5A4 File Offset: 0x0044B7A4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.openInt, "open", false, false);
			Scribe_Values.Look<bool>(ref this.holdOpenInt, "holdOpen", false, false);
			Scribe_Values.Look<int>(ref this.lastFriendlyTouchTick, "lastFriendlyTouchTick", 0, false);
			Scribe_Values.Look<int>(ref this.ticksUntilClose, "ticksUntilClose", 0, false);
			Scribe_References.Look<Pawn>(ref this.approachingPawn, "approachingPawn", false);
			Scribe_Values.Look<int>(ref this.ticksOpen, "ticksOpen", 0, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.openInt)
			{
				this.ticksSinceOpen = this.TicksToOpenNow;
			}
		}

		// Token: 0x0600ED33 RID: 60723 RVA: 0x0044D63E File Offset: 0x0044B83E
		public override void SetFaction(Faction newFaction, Pawn recruiter = null)
		{
			base.SetFaction(newFaction, recruiter);
			if (base.Spawned)
			{
				this.ClearReachabilityCache(base.Map);
			}
		}

		// Token: 0x0600ED34 RID: 60724 RVA: 0x0044D65C File Offset: 0x0044B85C
		protected override void Tick()
		{
			base.Tick();
			if (this.FreePassage != this.freePassageWhenClearedReachabilityCache)
			{
				this.ClearReachabilityCache(base.Map);
			}
			if (!this.openInt)
			{
				if (this.ticksSinceOpen > 0)
				{
					this.ticksSinceOpen--;
				}
				this.ticksOpen = 0;
				if (this.IsHashIntervalTick(this.def.building.doorTempEqualizeIntervalClosed))
				{
					GenTemperature.EqualizeTemperaturesThroughBuilding(this, this.TempEqualizeRate, false);
					return;
				}
			}
			else if (this.openInt)
			{
				if (this.ticksSinceOpen < this.TicksToOpenNow)
				{
					this.ticksSinceOpen++;
				}
				this.ticksOpen++;
				foreach (IntVec3 c in this.OccupiedRect())
				{
					List<Thing> thingList = c.GetThingList(base.Map);
					for (int i = 0; i < thingList.Count; i++)
					{
						Pawn pawn = thingList[i] as Pawn;
						if (pawn != null)
						{
							this.CheckFriendlyTouched(pawn);
						}
					}
				}
				if (this.ticksUntilClose > 0)
				{
					foreach (IntVec3 c2 in this.OccupiedRect())
					{
						if (base.Map.thingGrid.CellContains(c2, ThingCategory.Pawn))
						{
							this.ticksUntilClose = this.CloseDelayAdjusted;
							break;
						}
					}
					this.ticksUntilClose--;
					if (this.ticksUntilClose <= 0 && !this.holdOpenInt && !this.DoorTryClose())
					{
						this.ticksUntilClose = 1;
					}
				}
				else if (this.CanTryCloseAutomatically)
				{
					this.ticksUntilClose = this.CloseDelayAdjusted;
				}
				if ((Find.TickManager.TicksGame + this.thingIDNumber.HashOffset()) % this.def.building.doorTempEqualizeIntervalOpen == 0)
				{
					GenTemperature.EqualizeTemperaturesThroughBuilding(this, this.TempEqualizeRate, false);
				}
				if (this.OpenPct >= 0.4f && this.approachingPawn != null)
				{
					base.Map.fogGrid.Notify_PawnEnteringDoor(this, this.approachingPawn);
					this.approachingPawn = null;
				}
			}
		}

		// Token: 0x0600ED35 RID: 60725 RVA: 0x0044D8A0 File Offset: 0x0044BAA0
		public void CheckFriendlyTouched(Pawn p)
		{
			if (!p.HostileTo(this) && this.PawnCanOpen(p))
			{
				this.lastFriendlyTouchTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x0600ED36 RID: 60726 RVA: 0x0044D8C4 File Offset: 0x0044BAC4
		public void Notify_PawnApproaching(Pawn p, float moveCost)
		{
			this.CheckFriendlyTouched(p);
			bool flag = this.PawnCanOpen(p);
			if (flag || this.Open)
			{
				this.approachingPawn = p;
			}
			if (flag && !this.SlowsPawns)
			{
				this.DoorOpen(Mathf.Max(300, Mathf.CeilToInt(moveCost) + 1));
			}
		}

		// Token: 0x0600ED37 RID: 60727 RVA: 0x0044D913 File Offset: 0x0044BB13
		public bool CanPhysicallyPass(Pawn p)
		{
			return this.FreePassage || this.PawnCanOpen(p) || (this.Open && p.HostileTo(this));
		}

		// Token: 0x0600ED38 RID: 60728 RVA: 0x0044D93C File Offset: 0x0044BB3C
		public virtual bool PawnCanOpen(Pawn p)
		{
			if (!p.CanOpenDoors)
			{
				return false;
			}
			Map map = base.Map;
			if (((map != null) ? map.Parent : null) != null && base.Map.Parent.doorsAlwaysOpenForPlayerPawns && p.Faction == Faction.OfPlayer)
			{
				return true;
			}
			if (p.CanOpenAnyDoor)
			{
				return true;
			}
			if (p.RaceProps.FenceBlocked && !this.def.building.roamerCanOpen && (!p.roping.IsRopedByPawn || !this.PawnCanOpen(p.roping.RopedByPawn)))
			{
				return false;
			}
			if (base.Faction == null)
			{
				return p.RaceProps.canOpenFactionlessDoors;
			}
			return (p.guest != null && p.guest.Released) || !this.CheckFaction || GenAI.MachinesLike(base.Faction, p);
		}

		// Token: 0x0600ED39 RID: 60729 RVA: 0x0044DA14 File Offset: 0x0044BC14
		public override bool BlocksPawn(Pawn p)
		{
			return !this.openInt && !this.PawnCanOpen(p);
		}

		// Token: 0x0600ED3A RID: 60730 RVA: 0x0044DA2C File Offset: 0x0044BC2C
		protected virtual void DoorOpen(int ticksToClose = 110)
		{
			if (this.openInt)
			{
				this.ticksUntilClose = ticksToClose;
			}
			else
			{
				this.ticksUntilClose = this.TicksToOpenNow + ticksToClose;
			}
			if (!this.openInt)
			{
				this.openInt = true;
				this.CheckClearReachabilityCacheBecauseOpenedOrClosed();
				if (this.DoorPowerOn)
				{
					this.def.building.soundDoorOpenPowered.PlayOneShot(this);
				}
				else
				{
					this.def.building.soundDoorOpenManual.PlayOneShot(this);
				}
				base.Map.events.Notify_DoorOpened(this);
			}
		}

		// Token: 0x0600ED3B RID: 60731 RVA: 0x0044DAC0 File Offset: 0x0044BCC0
		protected bool DoorTryClose()
		{
			if (this.holdOpenInt || this.BlockedOpenMomentary)
			{
				return false;
			}
			this.openInt = false;
			this.CheckClearReachabilityCacheBecauseOpenedOrClosed();
			if (this.DoorPowerOn)
			{
				this.def.building.soundDoorClosePowered.PlayOneShot(this);
			}
			else
			{
				this.def.building.soundDoorCloseManual.PlayOneShot(this);
			}
			base.Map.events.Notify_DoorClosed(this);
			return true;
		}

		// Token: 0x0600ED3C RID: 60732 RVA: 0x0044DB3E File Offset: 0x0044BD3E
		public void StartManualOpenBy(Pawn opener)
		{
			this.DoorOpen(110);
		}

		// Token: 0x0600ED3D RID: 60733 RVA: 0x0044DB48 File Offset: 0x0044BD48
		public void StartManualCloseBy(Pawn closer)
		{
			this.ticksUntilClose = this.CloseDelayAdjusted;
		}

		// Token: 0x0600ED3E RID: 60734 RVA: 0x0044DB58 File Offset: 0x0044BD58
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.DoorPreDraw();
			if (this.CanDrawMovers)
			{
				float offsetDist = 0f + 0.45f * this.OpenPct;
				this.DrawMovers(drawLoc, offsetDist, this.Graphic, AltitudeLayer.DoorMoveable.AltitudeFor(), Vector3.one, this.Graphic.ShadowGraphic);
			}
			base.Comps_PostDraw();
		}

		// Token: 0x0600ED3F RID: 60735 RVA: 0x0044DBB4 File Offset: 0x0044BDB4
		protected void DrawMovers(Vector3 drawPos, float offsetDist, Graphic graphic, float altitude, Vector3 drawScaleFactor, Graphic_Shadow shadowGraphic)
		{
			for (int i = 0; i < 2; i++)
			{
				Vector3 vector;
				Mesh mesh;
				if (i == 0)
				{
					vector = new Vector3(0f, 0f, (float)(-(float)this.def.size.x));
					mesh = MeshPool.plane10;
				}
				else
				{
					vector = new Vector3(0f, 0f, (float)this.def.size.x);
					mesh = MeshPool.plane10Flip;
				}
				Rot4 rotation = base.Rotation;
				rotation.Rotate(RotationDirection.Clockwise);
				vector = rotation.AsQuat * vector;
				Vector3 vector2 = drawPos;
				vector2.y = altitude;
				vector2 += vector * offsetDist;
				Graphics.DrawMesh(mesh, Matrix4x4.TRS(vector2, base.Rotation.AsQuat, new Vector3((float)this.def.size.x * drawScaleFactor.x, drawScaleFactor.y, (float)this.def.size.z * drawScaleFactor.z)), graphic.MatAt(base.Rotation, this), 0);
				if (shadowGraphic != null)
				{
					shadowGraphic.DrawWorker(vector2, base.Rotation, this.def, this, 0f);
				}
			}
		}

		// Token: 0x0600ED40 RID: 60736 RVA: 0x0044DCE9 File Offset: 0x0044BEE9
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if ((base.Faction == Faction.OfPlayer || base.Faction == null) && !this.AlwaysOpen)
			{
				yield return new Command_Toggle
				{
					defaultLabel = "CommandToggleDoorHoldOpen".Translate(),
					defaultDesc = "CommandToggleDoorHoldOpenDesc".Translate(),
					hotKey = KeyBindingDefOf.Misc3,
					icon = TexCommand.HoldOpen,
					isActive = (() => this.holdOpenInt),
					toggleAction = delegate()
					{
						this.holdOpenInt = !this.holdOpenInt;
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600ED41 RID: 60737 RVA: 0x0044DCF9 File Offset: 0x0044BEF9
		private void ClearReachabilityCache(Map map)
		{
			map.reachability.ClearCache();
			this.freePassageWhenClearedReachabilityCache = this.FreePassage;
		}

		// Token: 0x0600ED42 RID: 60738 RVA: 0x0044DD12 File Offset: 0x0044BF12
		private void CheckClearReachabilityCacheBecauseOpenedOrClosed()
		{
			if (base.Spawned)
			{
				base.Map.reachability.ClearCacheForHostile(this);
			}
		}

		// Token: 0x0600ED43 RID: 60739 RVA: 0x0044DD2D File Offset: 0x0044BF2D
		protected void DoorPreDraw()
		{
			if (this.def.size == IntVec2.One)
			{
				base.Rotation = DoorUtility.DoorRotationAt(base.Position, base.Map, this.def.building.preferConnectingToFences);
			}
		}

		// Token: 0x0600ED44 RID: 60740 RVA: 0x0044DD70 File Offset: 0x0044BF70
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (this.StuckOpen && !this.AlwaysOpen)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "DoorMustBeEnclosedByWalls".Translate().Colorize(ColorLibrary.RedReadable);
			}
			return text;
		}

		// Token: 0x04009D0B RID: 40203
		public CompPowerTrader powerComp;

		// Token: 0x04009D0C RID: 40204
		protected bool openInt;

		// Token: 0x04009D0D RID: 40205
		protected bool holdOpenInt;

		// Token: 0x04009D0E RID: 40206
		private int lastFriendlyTouchTick = -9999;

		// Token: 0x04009D0F RID: 40207
		protected int ticksUntilClose;

		// Token: 0x04009D10 RID: 40208
		private Pawn approachingPawn;

		// Token: 0x04009D11 RID: 40209
		private int ticksOpen;

		// Token: 0x04009D12 RID: 40210
		protected int ticksSinceOpen;

		// Token: 0x04009D13 RID: 40211
		private bool freePassageWhenClearedReachabilityCache;

		// Token: 0x04009D14 RID: 40212
		private const float OpenTicks = 45f;

		// Token: 0x04009D15 RID: 40213
		private const int CloseDelayTicks = 110;

		// Token: 0x04009D16 RID: 40214
		private const int ApproachCloseDelayTicks = 300;

		// Token: 0x04009D17 RID: 40215
		private const int MaxTicksSinceFriendlyTouchToAutoClose = 120;

		// Token: 0x04009D18 RID: 40216
		private const float PowerOffDoorOpenSpeedFactor = 0.25f;

		// Token: 0x04009D19 RID: 40217
		private const float VisualDoorOffsetStart = 0f;

		// Token: 0x04009D1A RID: 40218
		private const float VisualDoorOffsetEnd = 0.45f;

		// Token: 0x04009D1B RID: 40219
		private const float NotifyFogGridDoorOpenPct = 0.4f;

		// Token: 0x04009D1C RID: 40220
		private const int TicksOpenToBreach = 600;
	}
}
