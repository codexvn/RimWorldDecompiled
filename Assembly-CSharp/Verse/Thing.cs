using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020008CC RID: 2252
	public class Thing : Entity, ISelectable, ILoadReferenceable, ISignalReceiver, IExposable, IEquatable<Thing>
	{
		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x060038F0 RID: 14576 RVA: 0x0012FC30 File Offset: 0x0012DE30
		// (set) Token: 0x060038F1 RID: 14577 RVA: 0x0012FC38 File Offset: 0x0012DE38
		public virtual int HitPoints
		{
			get
			{
				return this.hitPointsInt;
			}
			set
			{
				this.hitPointsInt = value;
			}
		}

		// Token: 0x17000A7D RID: 2685
		// (get) Token: 0x060038F2 RID: 14578 RVA: 0x0012FC41 File Offset: 0x0012DE41
		public int MaxHitPoints
		{
			get
			{
				return Mathf.RoundToInt(this.GetStatValue(StatDefOf.MaxHitPoints, true, 10));
			}
		}

		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x060038F3 RID: 14579 RVA: 0x0012FC56 File Offset: 0x0012DE56
		public virtual float MarketValue
		{
			get
			{
				return this.GetStatValue(StatDefOf.MarketValue, true, -1);
			}
		}

		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x060038F4 RID: 14580 RVA: 0x0012FC65 File Offset: 0x0012DE65
		public virtual float RoyalFavorValue
		{
			get
			{
				return this.GetStatValue(StatDefOf.RoyalFavorValue, true, -1);
			}
		}

		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x060038F5 RID: 14581 RVA: 0x0012FC74 File Offset: 0x0012DE74
		public virtual int? OverrideGraphicIndex
		{
			get
			{
				return this.overrideGraphicIndex;
			}
		}

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x060038F6 RID: 14582 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual Texture UIIconOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x060038F7 RID: 14583 RVA: 0x0012FC7C File Offset: 0x0012DE7C
		// (set) Token: 0x060038F8 RID: 14584 RVA: 0x0012FC84 File Offset: 0x0012DE84
		public bool EverSeenByPlayer
		{
			get
			{
				return this.GetEverSeenByPlayer();
			}
			set
			{
				this.SetEverSeenByPlayer(value);
			}
		}

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x060038F9 RID: 14585 RVA: 0x0012FC8D File Offset: 0x0012DE8D
		// (set) Token: 0x060038FA RID: 14586 RVA: 0x0012FC95 File Offset: 0x0012DE95
		public virtual ThingStyleDef StyleDef
		{
			get
			{
				return this.GetStyleDef();
			}
			set
			{
				this.styleGraphicInt = null;
				this.SetStyleDef(value);
			}
		}

		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x060038FB RID: 14587 RVA: 0x0012FCA5 File Offset: 0x0012DEA5
		// (set) Token: 0x060038FC RID: 14588 RVA: 0x0012FCAD File Offset: 0x0012DEAD
		public Precept_ThingStyle StyleSourcePrecept
		{
			get
			{
				return this.GetStyleSourcePrecept();
			}
			set
			{
				this.SetStyleSourcePrecept(value);
			}
		}

		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x060038FD RID: 14589 RVA: 0x0012FCB8 File Offset: 0x0012DEB8
		public bool FlammableNow
		{
			get
			{
				if (this.GetStatValue(StatDefOf.Flammability, true, -1) < 0.01f)
				{
					return false;
				}
				if (this.Spawned && !this.FireBulwark)
				{
					List<Thing> thingList = this.Position.GetThingList(this.Map);
					if (thingList != null)
					{
						for (int i = 0; i < thingList.Count; i++)
						{
							if (thingList[i].FireBulwark)
							{
								return false;
							}
						}
					}
				}
				return true;
			}
		}

		// Token: 0x17000A86 RID: 2694
		// (get) Token: 0x060038FE RID: 14590 RVA: 0x0012FD22 File Offset: 0x0012DF22
		public virtual bool FireBulwark
		{
			get
			{
				return this.def.Fillage == FillCategory.Full;
			}
		}

		// Token: 0x17000A87 RID: 2695
		// (get) Token: 0x060038FF RID: 14591 RVA: 0x0012FD32 File Offset: 0x0012DF32
		public bool Destroyed
		{
			get
			{
				return this.mapIndexOrState == -2 || this.mapIndexOrState == -3;
			}
		}

		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x06003900 RID: 14592 RVA: 0x0012FD4A File Offset: 0x0012DF4A
		public bool Discarded
		{
			get
			{
				return this.mapIndexOrState == -3;
			}
		}

		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x06003901 RID: 14593 RVA: 0x0012FD58 File Offset: 0x0012DF58
		public bool Spawned
		{
			get
			{
				if (this.mapIndexOrState < 0 || Find.Maps == null)
				{
					return false;
				}
				if ((int)this.mapIndexOrState < Find.Maps.Count)
				{
					return true;
				}
				Log.ErrorOnce(string.Format("Thing {0} is associated with invalid map index {1}", this.ThingID, this.mapIndexOrState), 64664487);
				return false;
			}
		}

		// Token: 0x17000A8A RID: 2698
		// (get) Token: 0x06003902 RID: 14594 RVA: 0x0012FDB1 File Offset: 0x0012DFB1
		public bool SpawnedOrAnyParentSpawned
		{
			get
			{
				return this.SpawnedParentOrMe != null;
			}
		}

		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x06003903 RID: 14595 RVA: 0x0012FDBC File Offset: 0x0012DFBC
		public Thing SpawnedParentOrMe
		{
			get
			{
				if (this.Spawned)
				{
					return this;
				}
				if (this.ParentHolder != null)
				{
					return ThingOwnerUtility.SpawnedParentOrMe(this.ParentHolder);
				}
				return null;
			}
		}

		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x06003904 RID: 14596 RVA: 0x0012FDDD File Offset: 0x0012DFDD
		public int TickSpawned
		{
			get
			{
				return this.spawnedTick;
			}
		}

		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x06003905 RID: 14597 RVA: 0x0012FDE5 File Offset: 0x0012DFE5
		public int TickDeSpawned
		{
			get
			{
				return this.despawnedTick;
			}
		}

		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x06003906 RID: 14598 RVA: 0x0012FDED File Offset: 0x0012DFED
		public Map Map
		{
			get
			{
				if (this.mapIndexOrState < 0)
				{
					return null;
				}
				List<Map> maps = Find.Maps;
				if (maps == null)
				{
					return null;
				}
				return maps[(int)this.mapIndexOrState];
			}
		}

		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x06003907 RID: 14599 RVA: 0x0012FE10 File Offset: 0x0012E010
		public Map MapHeld
		{
			get
			{
				if (this.Spawned)
				{
					return this.Map;
				}
				if (this.ParentHolder == null)
				{
					return null;
				}
				return ThingOwnerUtility.GetRootMap(this.ParentHolder);
			}
		}

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x06003908 RID: 14600 RVA: 0x0012FE36 File Offset: 0x0012E036
		// (set) Token: 0x06003909 RID: 14601 RVA: 0x0012FE40 File Offset: 0x0012E040
		public IntVec3 Position
		{
			get
			{
				return this.positionInt;
			}
			set
			{
				if (value == this.positionInt)
				{
					return;
				}
				if (this.Spawned)
				{
					if (this.def.AffectsRegions)
					{
						Log.Warning("Changed position of a spawned thing which affects regions. This is not supported.");
					}
					this.DirtyMapMesh(this.Map);
					RegionListersUpdater.DeregisterInRegions(this, this.Map);
					this.Map.thingGrid.Deregister(this, false);
					this.Map.coverGrid.DeRegister(this);
				}
				this.positionInt = value;
				if (this.Spawned)
				{
					this.Map.thingGrid.Register(this);
					this.Map.coverGrid.Register(this);
					this.Map.gasGrid.Notify_ThingSpawned(this);
					RegionListersUpdater.RegisterInRegions(this, this.Map);
					this.DirtyMapMesh(this.Map);
					if (this.def.AffectsReachability)
					{
						this.Map.reachability.ClearCache();
					}
				}
			}
		}

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x0600390A RID: 14602 RVA: 0x0012FF30 File Offset: 0x0012E130
		public IntVec3 PositionHeld
		{
			get
			{
				if (this.Spawned)
				{
					return this.Position;
				}
				IntVec3 rootPosition = ThingOwnerUtility.GetRootPosition(this.ParentHolder);
				if (rootPosition.IsValid)
				{
					return rootPosition;
				}
				return this.Position;
			}
		}

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x0600390B RID: 14603 RVA: 0x0012FF69 File Offset: 0x0012E169
		// (set) Token: 0x0600390C RID: 14604 RVA: 0x0012FF74 File Offset: 0x0012E174
		public Rot4 Rotation
		{
			get
			{
				return this.rotationInt;
			}
			set
			{
				if (value == this.rotationInt || this.debugRotLocked)
				{
					return;
				}
				if (this.Spawned && (this.def.size.x != 1 || this.def.size.z != 1))
				{
					if (this.def.AffectsRegions)
					{
						Log.Warning("Changed rotation of a spawned non-single-cell thing which affects regions. This is not supported.");
					}
					RegionListersUpdater.DeregisterInRegions(this, this.Map);
					this.Map.thingGrid.Deregister(this, false);
				}
				this.rotationInt = value;
				if (this.Spawned && (this.def.size.x != 1 || this.def.size.z != 1))
				{
					this.Map.thingGrid.Register(this);
					RegionListersUpdater.RegisterInRegions(this, this.Map);
					this.Map.gasGrid.Notify_ThingSpawned(this);
					if (this.def.AffectsReachability)
					{
						this.Map.reachability.ClearCache();
					}
				}
			}
		}

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x0600390D RID: 14605 RVA: 0x0013007B File Offset: 0x0012E27B
		public bool Smeltable
		{
			get
			{
				return !this.IsRelic() && this.def.smeltable && (!this.def.MadeFromStuff || this.Stuff.smeltable);
			}
		}

		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x0600390E RID: 14606 RVA: 0x001300B0 File Offset: 0x0012E2B0
		public bool BurnableByRecipe
		{
			get
			{
				return this.def.burnableByRecipe && (!this.def.MadeFromStuff || this.Stuff.burnableByRecipe);
			}
		}

		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x0600390F RID: 14607 RVA: 0x001300DB File Offset: 0x0012E2DB
		public IThingHolder ParentHolder
		{
			get
			{
				ThingOwner thingOwner = this.holdingOwner;
				if (thingOwner == null)
				{
					return null;
				}
				return thingOwner.Owner;
			}
		}

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x06003910 RID: 14608 RVA: 0x001300EE File Offset: 0x0012E2EE
		public Faction Faction
		{
			get
			{
				return this.factionInt;
			}
		}

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x06003911 RID: 14609 RVA: 0x001300F6 File Offset: 0x0012E2F6
		// (set) Token: 0x06003912 RID: 14610 RVA: 0x0013012C File Offset: 0x0012E32C
		public string ThingID
		{
			get
			{
				if (this.def.HasThingIDNumber)
				{
					return this.def.defName + this.thingIDNumber.ToString();
				}
				return this.def.defName;
			}
			set
			{
				this.thingIDNumber = Thing.IDNumberFromThingID(value);
			}
		}

		// Token: 0x06003913 RID: 14611 RVA: 0x0013013C File Offset: 0x0012E33C
		public static int IDNumberFromThingID(string thingID)
		{
			string value = Regex.Match(thingID, "\\d+$").Value;
			int result = 0;
			try
			{
				CultureInfo invariantCulture = CultureInfo.InvariantCulture;
				result = Convert.ToInt32(value, invariantCulture);
			}
			catch (Exception ex)
			{
				string[] array = new string[6];
				array[0] = "Could not convert id number from thingID=";
				array[1] = thingID;
				array[2] = ", numString=";
				array[3] = value;
				array[4] = " Exception=";
				int num = 5;
				Exception ex2 = ex;
				array[num] = ((ex2 != null) ? ex2.ToString() : null);
				Log.Error(string.Concat(array));
			}
			return result;
		}

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x06003914 RID: 14612 RVA: 0x001301C0 File Offset: 0x0012E3C0
		public IntVec2 RotatedSize
		{
			get
			{
				if (!this.rotationInt.IsHorizontal)
				{
					return this.def.size;
				}
				return new IntVec2(this.def.size.z, this.def.size.x);
			}
		}

		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x06003915 RID: 14613 RVA: 0x00130200 File Offset: 0x0012E400
		public virtual CellRect? CustomRectForSelector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x06003916 RID: 14614 RVA: 0x00130216 File Offset: 0x0012E416
		public override string Label
		{
			get
			{
				if (this.stackCount > 1)
				{
					return this.LabelNoCount + " x" + this.stackCount.ToStringCached();
				}
				return this.LabelNoCount;
			}
		}

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x06003917 RID: 14615 RVA: 0x00130243 File Offset: 0x0012E443
		public virtual string LabelNoCount
		{
			get
			{
				return GenLabel.ThingLabel(this, 1, true, true);
			}
		}

		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x06003918 RID: 14616 RVA: 0x0013024E File Offset: 0x0012E44E
		public override string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x06003919 RID: 14617 RVA: 0x00130261 File Offset: 0x0012E461
		public virtual string LabelCapNoCount
		{
			get
			{
				return this.LabelNoCount.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x0600391A RID: 14618 RVA: 0x00130274 File Offset: 0x0012E474
		public override string LabelShort
		{
			get
			{
				return this.LabelNoCount;
			}
		}

		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x0600391B RID: 14619 RVA: 0x0013027C File Offset: 0x0012E47C
		public virtual string LabelNoParenthesis
		{
			get
			{
				return GenLabel.ThingLabel(this, 1, false, false);
			}
		}

		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x0600391C RID: 14620 RVA: 0x00130287 File Offset: 0x0012E487
		public string LabelNoParenthesisCap
		{
			get
			{
				return this.LabelNoParenthesis.CapitalizeFirst();
			}
		}

		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x0600391D RID: 14621 RVA: 0x00130294 File Offset: 0x0012E494
		public virtual ModContentPack ContentSource
		{
			get
			{
				return this.def.modContentPack;
			}
		}

		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x0600391E RID: 14622 RVA: 0x001302A1 File Offset: 0x0012E4A1
		public virtual bool IngestibleNow
		{
			get
			{
				return !this.IsBurning() && this.def.IsIngestible;
			}
		}

		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x0600391F RID: 14623 RVA: 0x001302B8 File Offset: 0x0012E4B8
		public ThingDef Stuff
		{
			get
			{
				return this.stuffInt;
			}
		}

		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x06003920 RID: 14624 RVA: 0x001302C0 File Offset: 0x0012E4C0
		public Graphic DefaultGraphic
		{
			get
			{
				if (this.graphicInt == null)
				{
					if (this.def.graphicData == null)
					{
						return BaseContent.BadGraphic;
					}
					this.graphicInt = this.def.graphicData.GraphicColoredFor(this);
				}
				return this.graphicInt;
			}
		}

		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x06003921 RID: 14625 RVA: 0x001302FC File Offset: 0x0012E4FC
		public virtual Graphic Graphic
		{
			get
			{
				ThingStyleDef styleDef = this.StyleDef;
				if (((styleDef != null) ? styleDef.Graphic : null) != null)
				{
					if (this.styleGraphicInt == null)
					{
						if (styleDef.graphicData != null)
						{
							this.styleGraphicInt = styleDef.graphicData.GraphicColoredFor(this);
						}
						else
						{
							this.styleGraphicInt = styleDef.Graphic;
						}
					}
					return this.styleGraphicInt;
				}
				return this.DefaultGraphic;
			}
		}

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x06003922 RID: 14626 RVA: 0x0013035B File Offset: 0x0012E55B
		public virtual List<IntVec3> InteractionCells
		{
			get
			{
				return ThingUtility.InteractionCellsWhenAt(this.def, this.Position, this.Rotation, this.Map, true);
			}
		}

		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x06003923 RID: 14627 RVA: 0x0013037B File Offset: 0x0012E57B
		public virtual IntVec3 InteractionCell
		{
			get
			{
				return ThingUtility.InteractionCellWhenAt(this.def, this.Position, this.Rotation, this.Map);
			}
		}

		// Token: 0x17000AA8 RID: 2728
		// (get) Token: 0x06003924 RID: 14628 RVA: 0x0013039C File Offset: 0x0012E59C
		public float AmbientTemperature
		{
			get
			{
				if (this.Spawned)
				{
					return GenTemperature.GetTemperatureForCell(this.Position, this.Map);
				}
				if (this.ParentHolder != null)
				{
					for (IThingHolder parentHolder = this.ParentHolder; parentHolder != null; parentHolder = parentHolder.ParentHolder)
					{
						float result;
						if (ThingOwnerUtility.TryGetFixedTemperature(parentHolder, this, out result))
						{
							return result;
						}
					}
				}
				if (this.SpawnedOrAnyParentSpawned)
				{
					return GenTemperature.GetTemperatureForCell(this.PositionHeld, this.MapHeld);
				}
				if (this.Tile.Valid)
				{
					return GenTemperature.GetTemperatureAtTile(this.Tile);
				}
				return 21f;
			}
		}

		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x06003925 RID: 14629 RVA: 0x00130426 File Offset: 0x0012E626
		public PlanetTile Tile
		{
			get
			{
				if (this.Spawned)
				{
					return this.Map.Tile;
				}
				if (this.ParentHolder != null)
				{
					return ThingOwnerUtility.GetRootTile(this.ParentHolder);
				}
				return PlanetTile.Invalid;
			}
		}

		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x06003926 RID: 14630 RVA: 0x00130455 File Offset: 0x0012E655
		public virtual bool Suspended
		{
			get
			{
				return !this.Spawned && this.ParentHolder != null && ThingOwnerUtility.ContentsSuspended(this.ParentHolder);
			}
		}

		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x06003927 RID: 14631 RVA: 0x00130476 File Offset: 0x0012E676
		public bool InCryptosleep
		{
			get
			{
				return !this.Spawned && this.ParentHolder != null && ThingOwnerUtility.ContentsInCryptosleep(this.ParentHolder);
			}
		}

		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x06003928 RID: 14632 RVA: 0x00130497 File Offset: 0x0012E697
		public virtual string DescriptionDetailed
		{
			get
			{
				return this.def.DescriptionDetailed;
			}
		}

		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x06003929 RID: 14633 RVA: 0x001304A4 File Offset: 0x0012E6A4
		public virtual string DescriptionFlavor
		{
			get
			{
				return this.def.description;
			}
		}

		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x0600392A RID: 14634 RVA: 0x001304B1 File Offset: 0x0012E6B1
		public bool IsOnHoldingPlatform
		{
			get
			{
				return ModsConfig.AnomalyActive && this.ParentHolder is Building_HoldingPlatform;
			}
		}

		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x0600392B RID: 14635 RVA: 0x001304CA File Offset: 0x0012E6CA
		public TerrainAffordanceDef TerrainAffordanceNeeded
		{
			get
			{
				return this.def.GetTerrainAffordanceNeed(this.stuffInt);
			}
		}

		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x0600392C RID: 14636 RVA: 0x001304DD File Offset: 0x0012E6DD
		public bool BeingTransportedOnGravship
		{
			get
			{
				return this.beingTransportedOnGravship;
			}
		}

		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x0600392D RID: 14637 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual int MinTickIntervalRate
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x0600392E RID: 14638 RVA: 0x001304E5 File Offset: 0x0012E6E5
		protected virtual int MaxTickIntervalRate
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x0600392F RID: 14639 RVA: 0x001304E9 File Offset: 0x0012E6E9
		protected virtual int UpdateRateTickOffset
		{
			get
			{
				return this.HashOffset();
			}
		}

		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x06003930 RID: 14640 RVA: 0x001304F1 File Offset: 0x0012E6F1
		public virtual int UpdateRateTicks
		{
			get
			{
				return GenTicks.GetCameraUpdateRate(this);
			}
		}

		// Token: 0x06003931 RID: 14641 RVA: 0x001304FC File Offset: 0x0012E6FC
		public void DoTick()
		{
			if (this.Destroyed)
			{
				return;
			}
			if (!this.cached)
			{
				this.cached = true;
				this.cachedHolder = (this as IThingHolder);
				this.cachedTickable = (this as IThingHolderTickable);
				this.cachedIsHolder = (this.cachedHolder != null);
			}
			bool flag = this.holdingOwner != null && !this.Spawned;
			if (this.def.tickerType == TickerType.Normal)
			{
				using (ProfilerBlock.Scope("DoTick()"))
				{
					using (ProfilerBlock.Scope("Tick()"))
					{
						this.Tick();
					}
					if (this.Destroyed)
					{
						return;
					}
					this.tickDelta++;
					int num = Mathf.Min(Mathf.Max(this.UpdateRateTicks, this.MinTickIntervalRate), this.MaxTickIntervalRate);
					if (this.tickDelta >= num || GenTicks.IsTickInterval(this.UpdateRateTickOffset, num))
					{
						using (ProfilerBlock.Scope("TickInterval()"))
						{
							this.TickInterval(this.tickDelta);
						}
						this.tickDelta = 0;
					}
					goto IL_1AF;
				}
			}
			if (this.def.tickerType == TickerType.Rare && ((!this.cachedIsHolder && !flag) || this.IsHashIntervalTick(250)))
			{
				using (ProfilerBlock.Scope("TickRare()"))
				{
					this.TickRare();
					goto IL_1AF;
				}
			}
			if (this.def.tickerType == TickerType.Long && ((!this.cachedIsHolder && !flag) || this.IsHashIntervalTick(2000)))
			{
				using (ProfilerBlock.Scope("TickLong()"))
				{
					this.TickLong();
				}
			}
			IL_1AF:
			if (this.Destroyed)
			{
				return;
			}
			if (!this.cachedIsHolder)
			{
				return;
			}
			if (this.cachedTickable == null || this.cachedTickable.ShouldTickContents)
			{
				if (this.tmpHolders == null)
				{
					this.tmpHolders = new List<IThingHolder>(8);
				}
				this.tmpHolders.Add(this.cachedHolder);
				this.cachedHolder.GetChildHolders(this.tmpHolders);
				for (int i = 0; i < this.tmpHolders.Count; i++)
				{
					ThingOwner directlyHeldThings = this.tmpHolders[i].GetDirectlyHeldThings();
					if (directlyHeldThings != null)
					{
						directlyHeldThings.DoTick();
						if (this.Destroyed)
						{
							break;
						}
					}
				}
				this.tmpHolders.Clear();
			}
		}

		// Token: 0x06003932 RID: 14642 RVA: 0x001307A4 File Offset: 0x0012E9A4
		public virtual void PostMake()
		{
			ThingIDMaker.GiveIDTo(this);
			if (this.def.useHitPoints)
			{
				this.HitPoints = Mathf.RoundToInt((float)this.MaxHitPoints * Mathf.Clamp01(this.def.startingHpRange.RandomInRange));
			}
		}

		// Token: 0x06003933 RID: 14643 RVA: 0x001307E4 File Offset: 0x0012E9E4
		public virtual void PostPostMake()
		{
			if (!this.def.randomStyle.NullOrEmpty<ThingStyleChance>() && Rand.Chance(this.def.randomStyleChance))
			{
				this.StyleDef = this.def.randomStyle.RandomElementByWeight((ThingStyleChance x) => x.Chance).StyleDef;
			}
		}

		// Token: 0x06003934 RID: 14644 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostQualitySet()
		{
		}

		// Token: 0x06003935 RID: 14645 RVA: 0x0013084F File Offset: 0x0012EA4F
		public string GetUniqueLoadID()
		{
			return "Thing_" + this.ThingID;
		}

		// Token: 0x06003936 RID: 14646 RVA: 0x00130864 File Offset: 0x0012EA64
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (this.Destroyed)
			{
				Log.Error(string.Concat(new string[]
				{
					"Spawning destroyed thing ",
					(this != null) ? this.ToString() : null,
					" at ",
					this.Position.ToString(),
					". Correcting."
				}));
				this.mapIndexOrState = -1;
				if (this.HitPoints <= 0 && this.def.useHitPoints)
				{
					this.HitPoints = 1;
				}
			}
			if (this.Spawned)
			{
				Log.Error("Tried to spawn already-spawned thing " + ((this != null) ? this.ToString() : null) + " at " + this.Position.ToString());
				return;
			}
			int num = Find.Maps.IndexOf(map);
			if (num < 0)
			{
				Log.Error("Tried to spawn thing " + ((this != null) ? this.ToString() : null) + ", but the map provided does not exist.");
				return;
			}
			if (this.stackCount > this.def.stackLimit)
			{
				Log.Error(string.Concat(new string[]
				{
					"Spawned ",
					(this != null) ? this.ToString() : null,
					" with stackCount ",
					this.stackCount.ToString(),
					" but stackLimit is ",
					this.def.stackLimit.ToString(),
					". Truncating."
				}));
				this.stackCount = this.def.stackLimit;
			}
			this.mapIndexOrState = (sbyte)num;
			RegionListersUpdater.RegisterInRegions(this, map);
			if (!map.spawnedThings.TryAdd(this, false))
			{
				Log.Error("Couldn't add thing " + ((this != null) ? this.ToString() : null) + " to spawned things.");
			}
			map.listerThings.Add(this);
			map.thingGrid.Register(this);
			map.gasGrid.Notify_ThingSpawned(this);
			map.mapTemperature.Notify_ThingSpawned(this);
			if (map.IsPlayerHome)
			{
				this.EverSeenByPlayer = true;
			}
			if (Find.TickManager != null)
			{
				Find.TickManager.RegisterAllTickabilityFor(this);
			}
			this.DirtyMapMesh(map);
			if (this.def.drawerType != DrawerType.MapMeshOnly)
			{
				map.dynamicDrawManager.RegisterDrawable(this);
			}
			map.tooltipGiverList.Notify_ThingSpawned(this);
			if (this.def.CanAffectLinker)
			{
				map.linkGrid.Notify_LinkerCreatedOrDestroyed(this);
				map.mapDrawer.MapMeshDirty(this.Position, MapMeshFlagDefOf.Things, true, false);
			}
			if (!this.def.CanOverlapZones)
			{
				map.zoneManager.Notify_NoZoneOverlapThingSpawned(this);
			}
			if (this.def.AffectsRegions)
			{
				map.regionDirtyer.Notify_ThingAffectingRegionsSpawned(this);
			}
			if (this.def.pathCost != 0 || this.def.passability == Traversability.Impassable)
			{
				map.pathing.RecalculatePerceivedPathCostUnderThing(this);
			}
			if (this.def.AffectsReachability)
			{
				map.reachability.ClearCache();
			}
			map.coverGrid.Register(this);
			if (this.def.category == ThingCategory.Item)
			{
				map.listerHaulables.Notify_Spawned(this);
				map.listerMergeables.Notify_Spawned(this);
			}
			map.attackTargetsCache.Notify_ThingSpawned(this);
			Region validRegionAt_NoRebuild = map.regionGrid.GetValidRegionAt_NoRebuild(this.Position);
			if (validRegionAt_NoRebuild != null)
			{
				Room room = validRegionAt_NoRebuild.Room;
				if (room != null)
				{
					room.Notify_ContainedThingSpawnedOrDespawned(this);
				}
			}
			StealAIDebugDrawer.Notify_ThingChanged(this);
			IHaulDestination haulDestination = this as IHaulDestination;
			if (haulDestination != null)
			{
				map.haulDestinationManager.AddHaulDestination(haulDestination);
			}
			IHaulSource haulSource = this as IHaulSource;
			if (haulSource != null)
			{
				map.haulDestinationManager.AddHaulSource(haulSource);
			}
			if (this is IThingHolder && Find.ColonistBar != null)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
			if (this.def.category == ThingCategory.Item)
			{
				SlotGroup slotGroup = this.Position.GetSlotGroup(map);
				ISlotGroupParent slotGroupParent = (slotGroup != null) ? slotGroup.parent : null;
				if (slotGroupParent != null)
				{
					slotGroupParent.Notify_ReceivedThing(this);
					GenThing.TryDirtyAdjacentGroupContainers(slotGroupParent, map);
				}
			}
			if (this.def.receivesSignals)
			{
				Find.SignalManager.RegisterReceiver(this);
			}
			if (!this.BeingTransportedOnGravship)
			{
				SoundDef soundSpawned = this.def.soundSpawned;
				if (soundSpawned != null)
				{
					soundSpawned.PlayOneShot(this);
				}
				if (!respawningAfterLoad)
				{
					QuestUtility.SendQuestTargetSignals(this.questTags, "Spawned", this.Named("SUBJECT"));
					this.spawnedTick = Find.TickManager.TicksGame;
					this.despawnedTick = -1;
					List<EntityCodexEntryDef> entries;
					if (AnomalyUtility.ShouldNotifyCodex(this, EntityDiscoveryType.Spawn, out entries))
					{
						Find.EntityCodex.SetDiscovered(entries, this.def, this);
					}
					else
					{
						Find.HiddenItemsManager.SetDiscovered(this.def);
					}
				}
			}
			map.events.Notify_ThingSpawned(this);
		}

		// Token: 0x06003937 RID: 14647 RVA: 0x00130CE4 File Offset: 0x0012EEE4
		public bool DeSpawnOrDeselect(DestroyMode mode = DestroyMode.Vanish)
		{
			bool flag = Current.ProgramState == ProgramState.Playing && Find.Selector.IsSelected(this);
			if (this.Spawned)
			{
				this.DeSpawn(mode);
			}
			else if (flag)
			{
				Find.Selector.Deselect(this);
				Find.MainButtonsRoot.tabs.Notify_SelectedObjectDespawned();
			}
			return flag;
		}

		// Token: 0x06003938 RID: 14648 RVA: 0x00130D38 File Offset: 0x0012EF38
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.Destroyed)
			{
				Log.Error("Tried to despawn " + this.ToStringSafe<Thing>() + " which is already destroyed.");
				return;
			}
			if (!this.Spawned)
			{
				Log.Error("Tried to despawn " + this.ToStringSafe<Thing>() + " which is not spawned.");
				return;
			}
			Map map = this.Map;
			map.overlayDrawer.DisposeHandle(this);
			RegionListersUpdater.DeregisterInRegions(this, map);
			map.spawnedThings.Remove(this);
			map.listerThings.Remove(this);
			map.thingGrid.Deregister(this, false);
			map.coverGrid.DeRegister(this);
			if (this.def.receivesSignals)
			{
				Find.SignalManager.DeregisterReceiver(this);
			}
			map.tooltipGiverList.Notify_ThingDespawned(this);
			if (this.def.CanAffectLinker)
			{
				map.linkGrid.Notify_LinkerCreatedOrDestroyed(this);
				map.mapDrawer.MapMeshDirty(this.Position, MapMeshFlagDefOf.Things, true, false);
			}
			if (Find.Selector.IsSelected(this))
			{
				Find.Selector.Deselect(this);
				Find.MainButtonsRoot.tabs.Notify_SelectedObjectDespawned();
			}
			this.DirtyMapMesh(map);
			if (this.def.drawerType != DrawerType.MapMeshOnly)
			{
				map.dynamicDrawManager.DeRegisterDrawable(this);
			}
			Region validRegionAt_NoRebuild = map.regionGrid.GetValidRegionAt_NoRebuild(this.Position);
			if (validRegionAt_NoRebuild != null)
			{
				Room room = validRegionAt_NoRebuild.Room;
				if (room != null)
				{
					room.Notify_ContainedThingSpawnedOrDespawned(this);
				}
			}
			if (this.def.AffectsRegions)
			{
				map.regionDirtyer.Notify_ThingAffectingRegionsDespawned(this);
			}
			if (this.def.pathCost != 0 || this.def.passability == Traversability.Impassable)
			{
				map.pathing.RecalculatePerceivedPathCostUnderThing(this);
			}
			if (this.def.AffectsReachability)
			{
				map.reachability.ClearCache();
			}
			Find.TickManager.DeRegisterAllTickabilityFor(this);
			this.mapIndexOrState = -1;
			if (this.def.category == ThingCategory.Item)
			{
				map.listerHaulables.Notify_DeSpawned(this);
				map.listerMergeables.Notify_DeSpawned(this);
			}
			map.attackTargetsCache.Notify_ThingDespawned(this);
			map.physicalInteractionReservationManager.ReleaseAllForTarget(this);
			IHaulEnroute haulEnroute = this as IHaulEnroute;
			if (haulEnroute != null)
			{
				map.enrouteManager.Notify_ContainerDespawned(haulEnroute);
			}
			StealAIDebugDrawer.Notify_ThingChanged(this);
			IHaulDestination haulDestination = this as IHaulDestination;
			if (haulDestination != null)
			{
				map.haulDestinationManager.RemoveHaulDestination(haulDestination);
			}
			IHaulSource haulSource = this as IHaulSource;
			if (haulSource != null)
			{
				map.haulDestinationManager.RemoveHaulSource(haulSource);
			}
			if (this is IThingHolder && Find.ColonistBar != null)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
			if (this.def.category == ThingCategory.Item)
			{
				SlotGroup slotGroup = this.Position.GetSlotGroup(map);
				ISlotGroupParent slotGroupParent = (slotGroup != null) ? slotGroup.parent : null;
				if (slotGroupParent != null)
				{
					slotGroupParent.Notify_LostThing(this);
					GenThing.TryDirtyAdjacentGroupContainers(slotGroupParent, map);
				}
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "Despawned", this.Named("SUBJECT"));
			this.spawnedTick = -1;
			this.despawnedTick = Find.TickManager.TicksGame;
			map.events.Notify_ThingDespawned(this);
		}

		// Token: 0x06003939 RID: 14649 RVA: 0x00131029 File Offset: 0x0012F229
		public virtual void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
		{
			this.Destroy(DestroyMode.KillFinalize);
		}

		// Token: 0x0600393A RID: 14650 RVA: 0x00131034 File Offset: 0x0012F234
		public virtual void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (!Thing.allowDestroyNonDestroyable && !this.def.destroyable)
			{
				Log.Error("Tried to destroy non-destroyable thing " + ((this != null) ? this.ToString() : null));
				return;
			}
			if (this.Destroyed)
			{
				Log.Error("Tried to destroy already-destroyed thing " + ((this != null) ? this.ToString() : null));
				return;
			}
			bool spawned = this.Spawned;
			Map map = this.Map;
			if (this.StyleSourcePrecept != null)
			{
				this.StyleSourcePrecept.Notify_ThingLost(this, spawned);
			}
			if (this.Spawned)
			{
				this.DeSpawn(mode);
			}
			else if (Current.ProgramState == ProgramState.Playing && Find.Selector.IsSelected(this))
			{
				Find.Selector.Deselect(this);
				Find.MainButtonsRoot.tabs.Notify_SelectedObjectDespawned();
			}
			this.mapIndexOrState = -2;
			if (this.def.DiscardOnDestroyed)
			{
				this.Discard(false);
			}
			CompExplosive compExplosive = this.TryGetComp<CompExplosive>();
			if (spawned)
			{
				List<Thing> list = new List<Thing>();
				GenLeaving.DoLeavingsFor(this, map, mode, list);
				if (compExplosive != null)
				{
					compExplosive.AddThingsIgnoredByExplosion(list);
				}
				this.Notify_KilledLeavingsLeft(list);
			}
			if (this.holdingOwner != null)
			{
				this.holdingOwner.Notify_ContainedItemDestroyed(this);
			}
			this.RemoveAllReservationsAndDesignationsOnThis();
			if (!(this is Pawn))
			{
				this.stackCount = 0;
			}
			if (mode != DestroyMode.QuestLogic)
			{
				QuestUtility.SendQuestTargetSignals(this.questTags, "Destroyed", this.Named("SUBJECT"));
			}
			if (mode == DestroyMode.KillFinalize)
			{
				QuestUtility.SendQuestTargetSignals(this.questTags, "Killed", this.Named("SUBJECT"), map.Named("MAP"));
			}
		}

		// Token: 0x0600393B RID: 14651 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
		}

		// Token: 0x0600393C RID: 14652 RVA: 0x001311B2 File Offset: 0x0012F3B2
		public virtual void PostGeneratedForTrader(TraderKindDef trader, PlanetTile forTile, Faction forFaction)
		{
			if (this.def.colorGeneratorInTraderStock != null)
			{
				this.SetColor(this.def.colorGeneratorInTraderStock.NewRandomizedColor(), true);
			}
		}

		// Token: 0x0600393D RID: 14653 RVA: 0x001311D8 File Offset: 0x0012F3D8
		public virtual float GetBeauty(bool outside)
		{
			if (!outside || !this.def.StatBaseDefined(StatDefOf.BeautyOutdoors))
			{
				return this.GetStatValue(StatDefOf.Beauty, true, -1);
			}
			return this.GetStatValue(StatDefOf.BeautyOutdoors, true, -1);
		}

		// Token: 0x0600393E RID: 14654 RVA: 0x0013120C File Offset: 0x0012F40C
		public virtual void Notify_MyMapRemoved()
		{
			if (this.def.receivesSignals)
			{
				Find.SignalManager.DeregisterReceiver(this);
			}
			if (this.StyleSourcePrecept != null)
			{
				this.StyleSourcePrecept.Notify_ThingLost(this, false);
			}
			if (!ThingOwnerUtility.AnyParentIs<Pawn>(this))
			{
				this.mapIndexOrState = -3;
			}
			ThingOwner thingOwner = this.holdingOwner;
			if (thingOwner != null && thingOwner.Owner is Map)
			{
				this.holdingOwner = null;
			}
			this.RemoveAllReservationsAndDesignationsOnThis();
		}

		// Token: 0x0600393F RID: 14655 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_LordDestroyed()
		{
		}

		// Token: 0x06003940 RID: 14656 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_AbandonedAtTile(PlanetTile tile)
		{
		}

		// Token: 0x06003941 RID: 14657 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_KilledLeavingsLeft(List<Thing> leavings)
		{
		}

		// Token: 0x06003942 RID: 14658 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_Studied(Pawn studier, float amount, KnowledgeCategoryDef category = null)
		{
		}

		// Token: 0x06003943 RID: 14659 RVA: 0x0013127C File Offset: 0x0012F47C
		public virtual void Notify_Unfogged()
		{
			if (this.beenRevealed)
			{
				return;
			}
			this.beenRevealed = true;
			List<EntityCodexEntryDef> entries;
			if (ModsConfig.AnomalyActive && AnomalyUtility.ShouldNotifyCodex(this, EntityDiscoveryType.Unfog, out entries))
			{
				Find.EntityCodex.SetDiscovered(entries, this.def, this);
			}
			else
			{
				Find.HiddenItemsManager.SetDiscovered(this.def);
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "Unfogged", this);
			CompLetterOnRevealed compLetterOnRevealed = this.TryGetComp<CompLetterOnRevealed>();
			if (compLetterOnRevealed != null)
			{
				Find.LetterStack.ReceiveLetter(compLetterOnRevealed.Props.label, compLetterOnRevealed.Props.text, compLetterOnRevealed.Props.letterDef, this, null, null, null, null, 0, true);
			}
		}

		// Token: 0x06003944 RID: 14660 RVA: 0x00131330 File Offset: 0x0012F530
		public void ForceSetStateToUnspawned()
		{
			this.mapIndexOrState = -1;
		}

		// Token: 0x06003945 RID: 14661 RVA: 0x0013133C File Offset: 0x0012F53C
		public void DecrementMapIndex()
		{
			if (this.mapIndexOrState <= 0)
			{
				Log.Warning("Tried to decrement map index for " + ((this != null) ? this.ToString() : null) + ", but mapIndexOrState=" + this.mapIndexOrState.ToString());
				return;
			}
			this.mapIndexOrState -= 1;
		}

		// Token: 0x06003946 RID: 14662 RVA: 0x00131390 File Offset: 0x0012F590
		private void RemoveAllReservationsAndDesignationsOnThis()
		{
			if (this.def.category == ThingCategory.Mote)
			{
				return;
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].reservationManager.ReleaseAllForTarget(this);
				maps[i].physicalInteractionReservationManager.ReleaseAllForTarget(this);
				IAttackTarget attackTarget = this as IAttackTarget;
				if (attackTarget != null)
				{
					maps[i].attackTargetReservationManager.ReleaseAllForTarget(attackTarget);
				}
				maps[i].designationManager.RemoveAllDesignationsOn(this, false);
			}
		}

		// Token: 0x06003947 RID: 14663 RVA: 0x0013141C File Offset: 0x0012F61C
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.tickDelta, "tickDelta", 0, false);
			if (this.def.HasThingIDNumber)
			{
				string thingID = this.ThingID;
				Scribe_Values.Look<string>(ref thingID, "id", null, false);
				if (Scribe.mode != LoadSaveMode.Saving)
				{
					this.ThingID = thingID;
				}
			}
			Scribe_Values.Look<sbyte>(ref this.mapIndexOrState, "map", -1, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.mapIndexOrState >= 0)
			{
				this.mapIndexOrState = -1;
			}
			Scribe_Values.Look<IntVec3>(ref this.positionInt, "pos", IntVec3.Invalid, false);
			Scribe_Values.Look<Rot4>(ref this.rotationInt, "rot", Rot4.North, false);
			Scribe_Values.Look<bool>(ref this.debugRotLocked, "debugRotLocked", false, false);
			if (this.def.useHitPoints)
			{
				Scribe_Values.Look<int>(ref this.hitPointsInt, "health", -1, false);
			}
			bool flag = this.def.tradeability != Tradeability.None && this.def.category == ThingCategory.Item;
			if (this.def.stackLimit > 1 || flag)
			{
				Scribe_Values.Look<int>(ref this.stackCount, "stackCount", 0, true);
			}
			Scribe_Defs.Look<ThingDef>(ref this.stuffInt, "stuff");
			string facID = (this.factionInt != null) ? this.factionInt.GetUniqueLoadID() : "null";
			Scribe_Values.Look<string>(ref facID, "faction", "null", false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (facID == "null")
				{
					this.factionInt = null;
				}
				else if (Find.World != null && Find.FactionManager != null)
				{
					this.factionInt = Find.FactionManager.AllFactions.FirstOrDefault((Faction fa) => fa.GetUniqueLoadID() == facID);
				}
				else
				{
					Thing.facIDsCached.SetOrAdd(this, facID);
				}
			}
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				if (facID == "null" && Thing.facIDsCached.TryGetValue(this, out facID))
				{
					Thing.facIDsCached.Remove(this);
				}
				if (facID != "null")
				{
					this.factionInt = Find.FactionManager.AllFactions.FirstOrDefault((Faction fa) => fa.GetUniqueLoadID() == facID);
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Thing.facIDsCached.Clear();
				if (this.def.MadeFromStuff && this.Stuff == null)
				{
					Log.Error(string.Format("{0} is made from stuff but has no stuff set. Setting default stuff.", this));
					this.SetStuffDirect(GenStuff.DefaultStuffFor(this.def));
					if (this.Stuff == null)
					{
						Log.Error(string.Format("Failed to find stuff for {0} after loading.", this));
					}
				}
			}
			Scribe_Collections.Look<string>(ref this.questTags, "questTags", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<int?>(ref this.overrideGraphicIndex, "overrideGraphicIndex", null, false);
			Scribe_Values.Look<int>(ref this.spawnedTick, "spawnedTick", -1, false);
			Scribe_Values.Look<int>(ref this.despawnedTick, "despawnedTick", 0, false);
			Scribe_Values.Look<bool>(ref this.beenRevealed, "beenRevealed", false, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06003948 RID: 14664 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostMapInit()
		{
		}

		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x06003949 RID: 14665 RVA: 0x00131730 File Offset: 0x0012F930
		public Vector3? DrawPosHeld
		{
			get
			{
				if (this.Spawned)
				{
					return new Vector3?(this.DrawPos);
				}
				Thing thing = ThingOwnerUtility.SpawnedParentOrMe(this.ParentHolder);
				if (thing == null)
				{
					return null;
				}
				return new Vector3?(thing.DrawPos);
			}
		}

		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x0600394A RID: 14666 RVA: 0x00131774 File Offset: 0x0012F974
		public virtual Vector3 DrawPos
		{
			get
			{
				return this.TrueCenter();
			}
		}

		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x0600394B RID: 14667 RVA: 0x0013177C File Offset: 0x0012F97C
		public virtual Vector2 DrawSize
		{
			get
			{
				if (this.def.graphicData != null)
				{
					return this.def.graphicData.drawSize;
				}
				return Vector2.one;
			}
		}

		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x0600394C RID: 14668 RVA: 0x001317A1 File Offset: 0x0012F9A1
		// (set) Token: 0x0600394D RID: 14669 RVA: 0x001317E0 File Offset: 0x0012F9E0
		public virtual Color DrawColor
		{
			get
			{
				if (this.Stuff != null)
				{
					return this.def.GetColorForStuff(this.Stuff);
				}
				if (this.def.graphicData != null)
				{
					return this.def.graphicData.color;
				}
				return Color.white;
			}
			set
			{
				Log.Error(string.Format("Cannot set instance color on non-ThingWithComps {0} at {1}.", this.LabelCap, this.Position));
			}
		}

		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x0600394E RID: 14670 RVA: 0x00131802 File Offset: 0x0012FA02
		public virtual Color DrawColorTwo
		{
			get
			{
				if (this.def.graphicData != null)
				{
					return this.def.graphicData.colorTwo;
				}
				return Color.white;
			}
		}

		// Token: 0x0600394F RID: 14671 RVA: 0x00131827 File Offset: 0x0012FA27
		public void DrawNowAt(Vector3 drawLoc, bool flip = false)
		{
			this.DynamicDrawPhaseAt(DrawPhase.Draw, drawLoc, flip);
		}

		// Token: 0x06003950 RID: 14672 RVA: 0x00131832 File Offset: 0x0012FA32
		public void DynamicDrawPhase(DrawPhase phase)
		{
			if (this.def.drawerType == DrawerType.MapMeshOnly)
			{
				return;
			}
			this.DynamicDrawPhaseAt(phase, this.DrawPos, false);
		}

		// Token: 0x06003951 RID: 14673 RVA: 0x00131851 File Offset: 0x0012FA51
		public virtual void DynamicDrawPhaseAt(DrawPhase phase, Vector3 drawLoc, bool flip = false)
		{
			if (phase == DrawPhase.Draw)
			{
				this.DrawAt(drawLoc, flip);
			}
		}

		// Token: 0x06003952 RID: 14674 RVA: 0x00131860 File Offset: 0x0012FA60
		protected virtual void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			if (this.def.drawerType == DrawerType.RealtimeOnly || !this.Spawned)
			{
				this.Graphic.Draw(drawLoc, flip ? this.Rotation.Opposite : this.Rotation, this, 0f);
			}
			SilhouetteUtility.DrawGraphicSilhouette(this, drawLoc);
		}

		// Token: 0x06003953 RID: 14675 RVA: 0x001318B5 File Offset: 0x0012FAB5
		public virtual void Print(SectionLayer layer)
		{
			if (!this.def.dontPrint)
			{
				this.Graphic.Print(layer, this, 0f);
			}
		}

		// Token: 0x06003954 RID: 14676 RVA: 0x001318D8 File Offset: 0x0012FAD8
		public void DirtyMapMesh(Map map)
		{
			if (this.def.drawerType != DrawerType.RealtimeOnly)
			{
				foreach (IntVec3 loc in this.OccupiedRect())
				{
					map.mapDrawer.MapMeshDirty(loc, MapMeshFlagDefOf.Things);
				}
			}
		}

		// Token: 0x06003955 RID: 14677 RVA: 0x0013194C File Offset: 0x0012FB4C
		public virtual void DrawGUIOverlay()
		{
			if (Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest)
			{
				if (this.def.stackLimit > 1)
				{
					GenMapUI.DrawThingLabel(this, this.stackCount.ToStringCached());
					return;
				}
				QualityCategory cat;
				if (this.def.drawGUIOverlayQuality && this.TryGetQuality(out cat))
				{
					GenMapUI.DrawThingLabel(this, cat.GetLabelShort());
				}
			}
		}

		// Token: 0x06003956 RID: 14678 RVA: 0x001319A8 File Offset: 0x0012FBA8
		public virtual void DrawExtraSelectionOverlays()
		{
			if (this.def.specialDisplayRadius > 0.1f)
			{
				GenDraw.DrawRadiusRing(this.Position, this.def.specialDisplayRadius);
			}
			if (this.def.drawPlaceWorkersWhileSelected && this.def.PlaceWorkers != null)
			{
				for (int i = 0; i < this.def.PlaceWorkers.Count; i++)
				{
					this.def.PlaceWorkers[i].DrawGhost(this.def, this.Position, this.Rotation, Color.white, this);
				}
			}
			GenDraw.DrawInteractionCells(this.def, this.Position, this.rotationInt);
		}

		// Token: 0x06003957 RID: 14679 RVA: 0x00131A57 File Offset: 0x0012FC57
		public virtual string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			QuestUtility.AppendInspectStringsFromQuestParts(stringBuilder, this);
			return stringBuilder.ToString();
		}

		// Token: 0x06003958 RID: 14680 RVA: 0x00131A6C File Offset: 0x0012FC6C
		public virtual string GetInspectStringLowPriority()
		{
			string result = null;
			Thing.tmpDeteriorationReasons.Clear();
			float f = SteadyEnvironmentEffects.FinalDeteriorationRate(this, Thing.tmpDeteriorationReasons);
			if (Thing.tmpDeteriorationReasons.Count != 0)
			{
				result = string.Format("{0}: {1} ({2})", "DeterioratingBecauseOf".Translate(), Thing.tmpDeteriorationReasons.ToCommaList(false, false).CapitalizeFirst(), "PerDay".Translate(f.ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute)));
			}
			return result;
		}

		// Token: 0x06003959 RID: 14681 RVA: 0x00131AE5 File Offset: 0x0012FCE5
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			Gizmo gizmo = ContainingSelectionUtility.SelectContainingThingGizmo(this);
			if (gizmo != null)
			{
				yield return gizmo;
			}
			Thing.showingGizmosForRitualsTmp.Clear();
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				int i = 0;
				while (i < ideo.PreceptsListForReading.Count)
				{
					Precept precept = ideo.PreceptsListForReading[i];
					Precept_Ritual ritual = precept as Precept_Ritual;
					if (ritual == null)
					{
						goto IL_19D;
					}
					if (!precept.def.mergeRitualGizmosFromAllIdeos || !Thing.showingGizmosForRitualsTmp.Contains(ritual.sourcePattern))
					{
						if (ritual.ShouldShowGizmo(this))
						{
							foreach (Gizmo gizmo2 in ritual.GetGizmoFor(this))
							{
								yield return gizmo2;
								Thing.showingGizmosForRitualsTmp.Add(ritual.sourcePattern);
							}
							IEnumerator<Gizmo> enumerator2 = null;
							goto IL_19D;
						}
						goto IL_19D;
					}
					IL_1A4:
					int num = i;
					i = num + 1;
					continue;
					IL_19D:
					ritual = null;
					goto IL_1A4;
				}
				ideo = null;
			}
			IEnumerator<Ideo> enumerator = null;
			List<LordJob_Ritual> activeRituals = Find.IdeoManager.GetActiveRituals(this.MapHeld);
			foreach (LordJob_Ritual lordJob_Ritual in activeRituals)
			{
				if (lordJob_Ritual.selectedTarget == this)
				{
					yield return lordJob_Ritual.GetCancelGizmo();
				}
			}
			List<LordJob_Ritual>.Enumerator enumerator3 = default(List<LordJob_Ritual>.Enumerator);
			if (ModsConfig.AnomalyActive)
			{
				Gizmo gizmo3 = AnomalyUtility.OpenCodexGizmo(this);
				if (gizmo3 != null)
				{
					yield return gizmo3;
				}
			}
			if (DebugSettings.ShowDevGizmos && this.HasAttachment(ThingDefOf.Fire))
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Extinguish",
					action = delegate()
					{
						Thing attachment = this.GetAttachment(ThingDefOf.Fire);
						if (attachment == null)
						{
							return;
						}
						attachment.Destroy(DestroyMode.Vanish);
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600395A RID: 14682 RVA: 0x000FB0F4 File Offset: 0x000F92F4
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			return Enumerable.Empty<FloatMenuOption>();
		}

		// Token: 0x0600395B RID: 14683 RVA: 0x000FB0F4 File Offset: 0x000F92F4
		public virtual IEnumerable<FloatMenuOption> GetMultiSelectFloatMenuOptions(IEnumerable<Pawn> selPawns)
		{
			return Enumerable.Empty<FloatMenuOption>();
		}

		// Token: 0x0600395C RID: 14684 RVA: 0x00131AF5 File Offset: 0x0012FCF5
		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return this.def.inspectorTabsResolved;
		}

		// Token: 0x0600395D RID: 14685 RVA: 0x00131B02 File Offset: 0x0012FD02
		public virtual string GetCustomLabelNoCount(bool includeHp = true)
		{
			return GenLabel.ThingLabel(this, 1, includeHp, true);
		}

		// Token: 0x0600395E RID: 14686 RVA: 0x00131B10 File Offset: 0x0012FD10
		public DamageWorker.DamageResult TakeDamage(DamageInfo dinfo)
		{
			if (this.Destroyed)
			{
				return new DamageWorker.DamageResult();
			}
			if (dinfo.Amount == 0f)
			{
				return new DamageWorker.DamageResult();
			}
			if (this.def.damageMultipliers != null)
			{
				for (int i = 0; i < this.def.damageMultipliers.Count; i++)
				{
					if (this.def.damageMultipliers[i].damageDef == dinfo.Def)
					{
						int num = Mathf.RoundToInt(dinfo.Amount * this.def.damageMultipliers[i].multiplier);
						dinfo.SetAmount((float)num);
					}
				}
			}
			bool flag;
			this.PreApplyDamage(ref dinfo, out flag);
			if (flag)
			{
				return new DamageWorker.DamageResult();
			}
			bool spawnedOrAnyParentSpawned = this.SpawnedOrAnyParentSpawned;
			Map mapHeld = this.MapHeld;
			DamageWorker.DamageResult damageResult = dinfo.Def.Worker.Apply(dinfo, this);
			if (dinfo.Def.harmsHealth && spawnedOrAnyParentSpawned)
			{
				mapHeld.damageWatcher.Notify_DamageTaken(this, damageResult.totalDamageDealt);
			}
			Pawn pawn = dinfo.Instigator as Pawn;
			if (pawn != null)
			{
				foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
				{
					hediff.Notify_PawnDamagedThing(this, dinfo, damageResult);
				}
			}
			if (dinfo.Def.ExternalViolenceFor(this))
			{
				if (dinfo.SpawnFilth)
				{
					GenLeaving.DropFilthDueToDamage(this, damageResult.totalDamageDealt);
				}
				if (dinfo.Instigator != null)
				{
					Pawn pawn2 = dinfo.Instigator as Pawn;
					if (pawn2 != null)
					{
						pawn2.records.AddTo(RecordDefOf.DamageDealt, damageResult.totalDamageDealt);
					}
					if (dinfo.Instigator.Faction == Faction.OfPlayer)
					{
						QuestUtility.SendQuestTargetSignals(this.questTags, "TookDamageFromPlayer", this.Named("SUBJECT"), dinfo.Instigator.Named("INSTIGATOR"));
					}
				}
				QuestUtility.SendQuestTargetSignals(this.questTags, "TookDamage", this.Named("SUBJECT"), dinfo.Instigator.Named("INSTIGATOR"), mapHeld.Named("MAP"));
			}
			if (!this.Destroyed && this.FlammableNow && dinfo.Def.igniteChanceByTargetFlammability != null && Rand.Chance(dinfo.Def.igniteChanceByTargetFlammability.Evaluate(this.GetStatValue(StatDefOf.Flammability, true, -1))))
			{
				this.TryAttachFire(Rand.Range(0.55f, 0.85f), dinfo.Instigator);
			}
			this.PostApplyDamage(dinfo, damageResult.totalDamageDealt);
			return damageResult;
		}

		// Token: 0x0600395F RID: 14687 RVA: 0x00131DB4 File Offset: 0x0012FFB4
		public virtual void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			absorbed = false;
		}

		// Token: 0x06003960 RID: 14688 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		// Token: 0x06003961 RID: 14689 RVA: 0x00131DBC File Offset: 0x0012FFBC
		public virtual bool CanStackWith(Thing other)
		{
			return !this.Destroyed && !other.Destroyed && this.def.category == ThingCategory.Item && !this.IsRelic() && !other.IsRelic() && this.def == other.def && this.Stuff == other.Stuff;
		}

		// Token: 0x06003962 RID: 14690 RVA: 0x00131E1C File Offset: 0x0013001C
		public virtual bool TryAbsorbStack(Thing other, bool respectStackLimit)
		{
			if (!this.CanStackWith(other))
			{
				return false;
			}
			int num = ThingUtility.TryAbsorbStackNumToTake(this, other, respectStackLimit);
			if (this.def.useHitPoints)
			{
				this.HitPoints = Mathf.CeilToInt((float)(this.HitPoints * this.stackCount + other.HitPoints * num) / (float)(this.stackCount + num));
			}
			this.stackCount += num;
			other.stackCount -= num;
			if (this.Map != null)
			{
				this.DirtyMapMesh(this.Map);
			}
			StealAIDebugDrawer.Notify_ThingChanged(this);
			if (this.Spawned)
			{
				this.Map.listerMergeables.Notify_ThingStackChanged(this);
			}
			if (other.stackCount <= 0)
			{
				other.Destroy(DestroyMode.Vanish);
				return true;
			}
			return false;
		}

		// Token: 0x06003963 RID: 14691 RVA: 0x00131ED8 File Offset: 0x001300D8
		public virtual Thing SplitOff(int count)
		{
			if (count <= 0)
			{
				throw new ArgumentException("SplitOff with count <= 0", "count");
			}
			if (count >= this.stackCount)
			{
				if (count > this.stackCount)
				{
					Log.Error(string.Concat(new string[]
					{
						"Tried to split off ",
						count.ToString(),
						" of ",
						(this != null) ? this.ToString() : null,
						" but there are only ",
						this.stackCount.ToString()
					}));
				}
				this.DeSpawnOrDeselect(DestroyMode.Vanish);
				ThingOwner thingOwner = this.holdingOwner;
				if (thingOwner != null)
				{
					thingOwner.Remove(this);
				}
				return this;
			}
			Thing thing = ThingMaker.MakeThing(this.def, this.Stuff);
			thing.stackCount = count;
			this.stackCount -= count;
			if (this.Map != null)
			{
				this.DirtyMapMesh(this.Map);
			}
			if (this.Spawned)
			{
				this.Map.listerMergeables.Notify_ThingStackChanged(this);
			}
			if (this.def.useHitPoints)
			{
				thing.HitPoints = this.HitPoints;
			}
			return thing;
		}

		// Token: 0x06003964 RID: 14692 RVA: 0x00131FE6 File Offset: 0x001301E6
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.Stuff != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Stat_Stuff_Name".Translate(), this.Stuff.LabelCap, "Stat_Stuff_Desc".Translate(), 1100, null, new Dialog_InfoCard.Hyperlink[]
				{
					new Dialog_InfoCard.Hyperlink(this.Stuff, -1)
				}, false, false);
			}
			if (ModsConfig.IdeologyActive && !Find.IdeoManager.classicMode)
			{
				Thing.tmpIdeoNames.Clear();
				ThingStyleDef styleDef = this.StyleDef;
				StyleCategoryDef styleCategoryDef = ((styleDef != null) ? styleDef.Category : null) ?? this.def.dominantStyleCategory;
				if (styleCategoryDef != null)
				{
					foreach (Ideo ideo in Find.IdeoManager.IdeosListForReading)
					{
						if (IdeoUtility.ThingSatisfiesIdeo(this, ideo))
						{
							Thing.tmpIdeoNames.Add(ideo.name.Colorize(ideo.Color));
						}
					}
					yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawn, "Stat_Thing_StyleDominanceCategory".Translate(), styleCategoryDef.LabelCap, "Stat_Thing_StyleDominanceCategoryDesc".Translate() + "\n\n" + "Stat_Thing_IdeosSatisfied".Translate() + ":" + "\n" + Thing.tmpIdeoNames.ToLineList("  - "), 6005, null, null, false, false);
				}
			}
			yield break;
		}

		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x06003965 RID: 14693 RVA: 0x00131FF6 File Offset: 0x001301F6
		public virtual IEnumerable<DefHyperlink> DescriptionHyperlinks
		{
			get
			{
				if (this.def.descriptionHyperlinks != null)
				{
					int num;
					for (int i = 0; i < this.def.descriptionHyperlinks.Count; i = num + 1)
					{
						yield return this.def.descriptionHyperlinks[i];
						num = i;
					}
				}
				yield break;
			}
		}

		// Token: 0x06003966 RID: 14694 RVA: 0x00132008 File Offset: 0x00130208
		public virtual void Notify_ColorChanged()
		{
			this.graphicInt = null;
			this.styleGraphicInt = null;
			if (this.Spawned && (this.def.drawerType == DrawerType.MapMeshOnly || this.def.drawerType == DrawerType.MapMeshAndRealTime))
			{
				this.Map.mapDrawer.MapMeshDirty(this.Position, MapMeshFlagDefOf.Things);
			}
		}

		// Token: 0x06003967 RID: 14695 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_Equipped(Pawn pawn)
		{
		}

		// Token: 0x06003968 RID: 14696 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_Unequipped(Pawn pawn)
		{
		}

		// Token: 0x06003969 RID: 14697 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_UsedVerb(Pawn pawn, Verb verb)
		{
		}

		// Token: 0x0600396A RID: 14698 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_UsedWeapon(Pawn pawn)
		{
		}

		// Token: 0x0600396B RID: 14699 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_DebugSpawned()
		{
		}

		// Token: 0x0600396C RID: 14700 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_RecipeProduced(Pawn pawn)
		{
		}

		// Token: 0x0600396D RID: 14701 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_SignalReceived(Signal signal)
		{
		}

		// Token: 0x0600396E RID: 14702 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_Explosion(Explosion explosion)
		{
		}

		// Token: 0x0600396F RID: 14703 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_BulletImpactNearby(BulletImpactData impactData)
		{
		}

		// Token: 0x06003970 RID: 14704 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_ThingSelected()
		{
		}

		// Token: 0x06003971 RID: 14705 RVA: 0x00132068 File Offset: 0x00130268
		public virtual TipSignal GetTooltip()
		{
			string text = this.LabelCap;
			if (this.def.useHitPoints)
			{
				text = string.Concat(new string[]
				{
					text,
					"\n",
					this.HitPoints.ToString(),
					" / ",
					this.MaxHitPoints.ToString()
				});
			}
			return new TipSignal(text, this.thingIDNumber * 251235);
		}

		// Token: 0x06003972 RID: 14706 RVA: 0x001320DD File Offset: 0x001302DD
		public virtual bool BlocksPawn(Pawn p)
		{
			return this.def.passability == Traversability.Impassable || (this.def.IsFence && p.def.race.FenceBlocked);
		}

		// Token: 0x06003973 RID: 14707 RVA: 0x00132111 File Offset: 0x00130311
		public void SetFactionDirect(Faction newFaction)
		{
			if (!this.def.CanHaveFaction)
			{
				Log.Error("Tried to SetFactionDirect on " + ((this != null) ? this.ToString() : null) + " which cannot have a faction.");
				return;
			}
			this.factionInt = newFaction;
		}

		// Token: 0x06003974 RID: 14708 RVA: 0x0013214C File Offset: 0x0013034C
		public virtual void SetFaction(Faction newFaction, Pawn recruiter = null)
		{
			if (!this.def.CanHaveFaction)
			{
				Log.Error("Tried to SetFaction on " + ((this != null) ? this.ToString() : null) + " which cannot have a faction.");
				return;
			}
			Faction previous = this.factionInt;
			this.factionInt = newFaction;
			if (this.Spawned)
			{
				IAttackTarget attackTarget = this as IAttackTarget;
				if (attackTarget != null)
				{
					this.Map.attackTargetsCache.UpdateTarget(attackTarget);
				}
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "ChangedFaction", this.Named("SUBJECT"), newFaction.Named("FACTION"));
			if (newFaction != Faction.OfPlayer)
			{
				QuestUtility.SendQuestTargetSignals(this.questTags, "ChangedFactionToNonPlayer", this.Named("SUBJECT"), newFaction.Named("FACTION"));
			}
			else
			{
				QuestUtility.SendQuestTargetSignals(this.questTags, "ChangedFactionToPlayer", this.Named("SUBJECT"), newFaction.Named("FACTION"));
			}
			if (this.Spawned)
			{
				this.Map.events.Notify_ThingFactionChanged(previous, this.factionInt);
			}
		}

		// Token: 0x06003975 RID: 14709 RVA: 0x000F9927 File Offset: 0x000F7B27
		public virtual AcceptanceReport ClaimableBy(Faction by)
		{
			return false;
		}

		// Token: 0x06003976 RID: 14710 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool AdoptableBy(Faction by, StringBuilder reason = null)
		{
			return false;
		}

		// Token: 0x06003977 RID: 14711 RVA: 0x00132254 File Offset: 0x00130454
		public bool FactionPreventsClaimingOrAdopting(Faction faction, bool forClaim, out string reason)
		{
			reason = null;
			if (faction == null)
			{
				return false;
			}
			if (faction == Faction.OfInsects)
			{
				if (HiveUtility.AnyHivePreventsClaiming(this))
				{
					return true;
				}
			}
			else
			{
				if (faction == Faction.OfMechanoids)
				{
					if (MechClusterUtility.PartOfActiveMechCluster(this))
					{
						return true;
					}
					using (HashSet<IAttackTarget>.Enumerator enumerator = this.MapHeld.attackTargetsCache.TargetsHostileToFaction(Faction.OfPlayer).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							IAttackTarget attackTarget = enumerator.Current;
							if (attackTarget.Thing != null && attackTarget.Thing.Faction == faction)
							{
								Pawn pawn = attackTarget.Thing as Pawn;
								if (pawn == null)
								{
									if (forClaim)
									{
										reason = "MessageCannotClaimWhenThreatsAreNear".Translate(this.Named("CLAIMABLE"), attackTarget.Named("THREAT"));
									}
									else
									{
										reason = "MessageCannotAdoptWhileThreatsAreNear".Translate(this.Named("CLAIMABLE"), attackTarget.Named("THREAT"));
									}
									return true;
								}
								if (GenHostility.IsActiveThreatToPlayer(pawn, false))
								{
									if (forClaim)
									{
										reason = "MessageCannotClaimWhenPawnThreatsAreNear".Translate(this.Named("CLAIMABLE"), pawn.Named("THREAT"));
									}
									else
									{
										reason = "MessageCannotAdoptWhilePawnThreatsAreNear".Translate(this.Named("CLAIMABLE"), pawn.Named("THREAT"));
									}
									return true;
								}
							}
						}
						return false;
					}
				}
				if (faction == Faction.OfAncients && this.Spawned && !this.Map.IsPlayerHome && GenHostility.AnyHostileActiveThreatToPlayer(this.Map, true, true))
				{
					return true;
				}
				if (this.Spawned && faction != Faction.OfPlayer)
				{
					List<Pawn> list = this.Map.mapPawns.SpawnedPawnsInFaction(faction);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].RaceProps.ToolUser && GenHostility.IsPotentialThreat(list[i]))
						{
							if (forClaim)
							{
								reason = "MessageCannotClaimWhenThreatsAreNear".Translate(this.Named("CLAIMABLE"), list[i].Named("THREAT"));
							}
							else
							{
								reason = "MessageCannotAdoptWhileThreatsAreNear".Translate(this.Named("CLAIMABLE"), list[i].Named("THREAT"));
							}
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06003978 RID: 14712 RVA: 0x001324E4 File Offset: 0x001306E4
		public void SetPositionDirect(IntVec3 newPos)
		{
			this.positionInt = newPos;
		}

		// Token: 0x06003979 RID: 14713 RVA: 0x001324ED File Offset: 0x001306ED
		public void SetStuffDirect(ThingDef newStuff)
		{
			this.stuffInt = newStuff;
		}

		// Token: 0x0600397A RID: 14714 RVA: 0x001324F6 File Offset: 0x001306F6
		public override string ToString()
		{
			if (this.def != null)
			{
				return this.ThingID;
			}
			return base.GetType().ToString();
		}

		// Token: 0x0600397B RID: 14715 RVA: 0x00132512 File Offset: 0x00130712
		public bool Equals(Thing other)
		{
			if (other == null)
			{
				return false;
			}
			if (this.def.category == ThingCategory.Mote)
			{
				return this == other;
			}
			return this.thingIDNumber == other.thingIDNumber && this.def.Equals(other.def);
		}

		// Token: 0x0600397C RID: 14716 RVA: 0x0013254E File Offset: 0x0013074E
		public override int GetHashCode()
		{
			if (this.thingIDNumber == -1)
			{
				return base.GetHashCode();
			}
			return this.thingIDNumber;
		}

		// Token: 0x0600397D RID: 14717 RVA: 0x00132568 File Offset: 0x00130768
		public virtual void Discard(bool silentlyRemoveReferences = false)
		{
			if (this.mapIndexOrState != -2)
			{
				Log.Warning(string.Concat(new string[]
				{
					"Tried to discard ",
					(this != null) ? this.ToString() : null,
					" whose state is ",
					this.mapIndexOrState.ToString(),
					"."
				}));
				return;
			}
			this.mapIndexOrState = -3;
		}

		// Token: 0x0600397E RID: 14718 RVA: 0x001325CE File Offset: 0x001307CE
		public virtual void Notify_DefsHotReloaded()
		{
			this.graphicInt = null;
		}

		// Token: 0x0600397F RID: 14719 RVA: 0x001325D7 File Offset: 0x001307D7
		public virtual IEnumerable<Thing> ButcherProducts(Pawn butcher, float efficiency)
		{
			if (this.def.butcherProducts != null)
			{
				int num2;
				for (int i = 0; i < this.def.butcherProducts.Count; i = num2 + 1)
				{
					ThingDefCountClass thingDefCountClass = this.def.butcherProducts[i];
					int num = GenMath.RoundRandom((float)thingDefCountClass.count * efficiency);
					if (num > 0)
					{
						Thing thing = ThingMaker.MakeThing(thingDefCountClass.thingDef, null);
						thing.stackCount = num;
						yield return thing;
					}
					num2 = i;
				}
			}
			yield break;
		}

		// Token: 0x06003980 RID: 14720 RVA: 0x001325EE File Offset: 0x001307EE
		public virtual IEnumerable<Thing> SmeltProducts(float efficiency)
		{
			List<ThingDefCountClass> costListAdj = this.def.CostListAdjusted(this.Stuff, true);
			int num2;
			for (int i = 0; i < costListAdj.Count; i = num2 + 1)
			{
				if (!costListAdj[i].thingDef.intricate && costListAdj[i].thingDef.smeltable)
				{
					int num = GenMath.RoundRandom((float)costListAdj[i].count * 0.25f);
					if (num > 0)
					{
						Thing thing = ThingMaker.MakeThing(costListAdj[i].thingDef, null);
						thing.stackCount = num;
						yield return thing;
					}
				}
				num2 = i;
			}
			if (this.def.smeltProducts != null)
			{
				for (int i = 0; i < this.def.smeltProducts.Count; i = num2 + 1)
				{
					ThingDefCountClass thingDefCountClass = this.def.smeltProducts[i];
					Thing thing2 = ThingMaker.MakeThing(thingDefCountClass.thingDef, null);
					thing2.stackCount = thingDefCountClass.count;
					yield return thing2;
					num2 = i;
				}
			}
			yield break;
		}

		// Token: 0x06003981 RID: 14721 RVA: 0x00132600 File Offset: 0x00130800
		public float Ingested(Pawn ingester, float nutritionWanted)
		{
			if (this.Destroyed)
			{
				Log.Error(((ingester != null) ? ingester.ToString() : null) + " ingested destroyed thing " + ((this != null) ? this.ToString() : null));
				return 0f;
			}
			if (!this.IngestibleNow)
			{
				Log.Error(((ingester != null) ? ingester.ToString() : null) + " ingested IngestibleNow=false thing " + ((this != null) ? this.ToString() : null));
				return 0f;
			}
			ingester.mindState.lastIngestTick = Find.TickManager.TicksGame;
			if (ingester.needs.mood != null)
			{
				List<FoodUtility.ThoughtFromIngesting> list = FoodUtility.ThoughtsFromIngesting(ingester, this, this.def);
				for (int i = 0; i < list.Count; i++)
				{
					Thought_Memory thought_Memory = ThoughtMaker.MakeThought(list[i].thought, list[i].fromPrecept);
					Thought_FoodEaten thought_FoodEaten = thought_Memory as Thought_FoodEaten;
					if (thought_FoodEaten != null)
					{
						thought_FoodEaten.SetFood(this);
					}
					ingester.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, null);
				}
			}
			Need_Chemical_Any drugsDesire = ingester.needs.drugsDesire;
			if (drugsDesire != null)
			{
				drugsDesire.Notify_IngestedDrug(this);
			}
			bool flag = FoodUtility.IsHumanlikeCorpseOrHumanlikeMeat(this, this.def);
			bool flag2 = FoodUtility.IsHumanlikeCorpseOrHumanlikeMeatOrIngredient(this);
			if (flag && ingester.IsColonist)
			{
				TaleRecorder.RecordTale(TaleDefOf.AteRawHumanlikeMeat, new object[]
				{
					ingester
				});
			}
			if (flag2)
			{
				ingester.mindState.lastHumanMeatIngestedTick = Find.TickManager.TicksGame;
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteHumanMeat, ingester.Named(HistoryEventArgsNames.Doer)), false);
				if (flag)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteHumanMeatDirect, ingester.Named(HistoryEventArgsNames.Doer)), false);
				}
			}
			else if (ModsConfig.IdeologyActive && !FoodUtility.AcceptableCannibalNonHumanlikeMeatFood(this.def))
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteNonCannibalFood, ingester.Named(HistoryEventArgsNames.Doer)), false);
			}
			if (this.def.ingestible.ateEvent != null)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(this.def.ingestible.ateEvent, ingester.Named(HistoryEventArgsNames.Doer)), false);
			}
			if (ModsConfig.IdeologyActive)
			{
				FoodKind foodKind = FoodUtility.GetFoodKind(this);
				if (foodKind != FoodKind.Any && !this.def.IsProcessedFood)
				{
					if (foodKind == FoodKind.Meat)
					{
						if (!flag2)
						{
							Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteMeat, ingester.Named(HistoryEventArgsNames.Doer)), false);
						}
					}
					else if (!this.def.IsDrug && this.def.ingestible.CachedNutrition > 0f)
					{
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteNonMeat, ingester.Named(HistoryEventArgsNames.Doer)), false);
					}
				}
				if (FoodUtility.IsVeneratedAnimalMeatOrCorpseOrHasIngredients(this, ingester))
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteVeneratedAnimalMeat, ingester.Named(HistoryEventArgsNames.Doer)), false);
				}
				if (this.def.thingCategories != null && this.def.thingCategories.Contains(ThingCategoryDefOf.PlantFoodRaw))
				{
					if (this.def.IsFungus)
					{
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteFungus, ingester.Named(HistoryEventArgsNames.Doer)), false);
					}
					else
					{
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteNonFungusPlant, ingester.Named(HistoryEventArgsNames.Doer)), false);
					}
				}
			}
			CompIngredients compIngredients = this.TryGetComp<CompIngredients>();
			if (compIngredients != null)
			{
				bool flag3 = false;
				bool flag4 = false;
				bool flag5 = false;
				bool flag6 = false;
				bool flag7 = false;
				for (int j = 0; j < compIngredients.ingredients.Count; j++)
				{
					if (!flag3 && FoodUtility.GetMeatSourceCategory(compIngredients.ingredients[j]) == MeatSourceCategory.Humanlike)
					{
						ingester.mindState.lastHumanMeatIngestedTick = Find.TickManager.TicksGame;
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteHumanMeatAsIngredient, ingester.Named(HistoryEventArgsNames.Doer)), false);
						flag3 = true;
					}
					else if (!flag4 && ingester.Ideo != null && compIngredients.ingredients[j].IsMeat && ingester.Ideo.IsVeneratedAnimal(compIngredients.ingredients[j].ingestible.sourceDef))
					{
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteVeneratedAnimalMeat, ingester.Named(HistoryEventArgsNames.Doer)), false);
						flag4 = true;
					}
					if (!flag5 && FoodUtility.GetMeatSourceCategory(compIngredients.ingredients[j]) == MeatSourceCategory.Insect)
					{
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteInsectMeatAsIngredient, ingester.Named(HistoryEventArgsNames.Doer)), false);
						flag5 = true;
					}
					if (ModsConfig.IdeologyActive && !flag6 && compIngredients.ingredients[j].thingCategories.Contains(ThingCategoryDefOf.PlantFoodRaw))
					{
						if (compIngredients.ingredients[j].IsFungus)
						{
							Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteFungusAsIngredient, ingester.Named(HistoryEventArgsNames.Doer)), false);
							flag6 = true;
						}
						else
						{
							flag7 = true;
						}
					}
				}
				if (ModsConfig.IdeologyActive && !flag6 && flag7)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteNonFungusMealWithPlants, ingester.Named(HistoryEventArgsNames.Doer)), false);
				}
			}
			int num;
			float result;
			this.IngestedCalculateAmounts(ingester, nutritionWanted, out num, out result);
			if (!ingester.Dead && ingester.needs.joy != null && Mathf.Abs(this.def.ingestible.joy) > 0.0001f && num > 0)
			{
				ingester.needs.joy.GainJoy((float)num * this.def.ingestible.joy, this.def.ingestible.joyKind ?? JoyKindDefOf.Gluttonous);
			}
			float num2;
			float chance = FoodUtility.TryGetFoodPoisoningChanceOverrideFromTraits(ingester, this, out num2) ? num2 : (this.GetStatValue(StatDefOf.FoodPoisonChanceFixedHuman, true, -1) * FoodUtility.GetFoodPoisonChanceFactor(ingester));
			if (ingester.RaceProps.Humanlike && Rand.Chance(chance))
			{
				FoodUtility.AddFoodPoisoningHediff(ingester, this, FoodPoisonCause.DangerousFoodType);
			}
			List<Hediff> hediffs = ingester.health.hediffSet.hediffs;
			for (int k = 0; k < hediffs.Count; k++)
			{
				hediffs[k].Notify_IngestedThing(this, num);
			}
			Pawn_GeneTracker genes = ingester.genes;
			if (genes != null)
			{
				genes.Notify_IngestedThing(this, num);
			}
			bool flag8 = false;
			if (num > 0)
			{
				if (this.stackCount == 0)
				{
					Log.Error(((this != null) ? this.ToString() : null) + " stack count is 0.");
				}
				if (num == this.stackCount)
				{
					flag8 = true;
				}
				else
				{
					this.SplitOff(num);
				}
			}
			this.PrePostIngested(ingester);
			if (flag8)
			{
				ingester.carryTracker.innerContainer.Remove(this);
			}
			if (this.def.ingestible.outcomeDoers != null)
			{
				for (int l = 0; l < this.def.ingestible.outcomeDoers.Count; l++)
				{
					this.def.ingestible.outcomeDoers[l].DoIngestionOutcome(ingester, this, num);
				}
			}
			if (flag8 && !this.Destroyed)
			{
				this.Destroy(DestroyMode.Vanish);
			}
			this.PostIngested(ingester);
			return result;
		}

		// Token: 0x06003982 RID: 14722 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void PrePostIngested(Pawn ingester)
		{
		}

		// Token: 0x06003983 RID: 14723 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void PostIngested(Pawn ingester)
		{
		}

		// Token: 0x06003984 RID: 14724 RVA: 0x00132D04 File Offset: 0x00130F04
		protected virtual void IngestedCalculateAmounts(Pawn ingester, float nutritionWanted, out int numTaken, out float nutritionIngested)
		{
			float num = FoodUtility.NutritionForEater(ingester, this);
			numTaken = Mathf.CeilToInt(nutritionWanted / num);
			numTaken = Mathf.Min(numTaken, this.stackCount);
			if (this.def.ingestible.maxNumToIngestAtOnce > 0)
			{
				numTaken = Mathf.Min(numTaken, this.def.ingestible.maxNumToIngestAtOnce);
			}
			numTaken = Mathf.Max(numTaken, 1);
			nutritionIngested = (float)numTaken * num;
		}

		// Token: 0x06003985 RID: 14725 RVA: 0x00132D70 File Offset: 0x00130F70
		public virtual bool PreventPlayerSellingThingsNearby(out string reason)
		{
			reason = null;
			return false;
		}

		// Token: 0x06003986 RID: 14726 RVA: 0x00132D76 File Offset: 0x00130F76
		public virtual void PreSwapMap()
		{
			this.beingTransportedOnGravship = true;
		}

		// Token: 0x06003987 RID: 14727 RVA: 0x00132D7F File Offset: 0x00130F7F
		public virtual void PostSwapMap()
		{
			this.beingTransportedOnGravship = false;
			QuestUtility.SendQuestTargetSignals(this.questTags, "SwappedMap", this.Named("SUBJECT"));
		}

		// Token: 0x06003988 RID: 14728 RVA: 0x00132DA4 File Offset: 0x00130FA4
		public virtual void Notify_LeftBehind()
		{
			QuestUtility.SendQuestTargetSignals(this.questTags, "LeftBehind", this.Named("SUBJECT"));
			IThingHolder thingHolder = this as IThingHolder;
			if (thingHolder != null && thingHolder.GetDirectlyHeldThings() != null)
			{
				foreach (Thing thing in thingHolder.GetDirectlyHeldThings().ToList<Thing>())
				{
					thing.Notify_LeftBehind();
				}
			}
		}

		// Token: 0x04002A1C RID: 10780
		public ThingDef def;

		// Token: 0x04002A1D RID: 10781
		public int thingIDNumber = -1;

		// Token: 0x04002A1E RID: 10782
		private sbyte mapIndexOrState = -1;

		// Token: 0x04002A1F RID: 10783
		private IntVec3 positionInt = IntVec3.Invalid;

		// Token: 0x04002A20 RID: 10784
		private Rot4 rotationInt = Rot4.North;

		// Token: 0x04002A21 RID: 10785
		public int stackCount = 1;

		// Token: 0x04002A22 RID: 10786
		protected Faction factionInt;

		// Token: 0x04002A23 RID: 10787
		private ThingDef stuffInt;

		// Token: 0x04002A24 RID: 10788
		private Graphic graphicInt;

		// Token: 0x04002A25 RID: 10789
		protected Graphic styleGraphicInt;

		// Token: 0x04002A26 RID: 10790
		private int hitPointsInt = -1;

		// Token: 0x04002A27 RID: 10791
		public ThingOwner holdingOwner;

		// Token: 0x04002A28 RID: 10792
		public List<string> questTags;

		// Token: 0x04002A29 RID: 10793
		public int spawnedTick = -1;

		// Token: 0x04002A2A RID: 10794
		public int despawnedTick = -1;

		// Token: 0x04002A2B RID: 10795
		public int? overrideGraphicIndex;

		// Token: 0x04002A2C RID: 10796
		public bool debugRotLocked;

		// Token: 0x04002A2D RID: 10797
		private bool beingTransportedOnGravship;

		// Token: 0x04002A2E RID: 10798
		private int tickDelta;

		// Token: 0x04002A2F RID: 10799
		private bool beenRevealed;

		// Token: 0x04002A30 RID: 10800
		public bool shouldHighlightCached;

		// Token: 0x04002A31 RID: 10801
		public int shouldHighlightCachedTick;

		// Token: 0x04002A32 RID: 10802
		public Color highlightColorCached;

		// Token: 0x04002A33 RID: 10803
		public int highlightColorCachedTick;

		// Token: 0x04002A34 RID: 10804
		protected const sbyte UnspawnedState = -1;

		// Token: 0x04002A35 RID: 10805
		private const sbyte MemoryState = -2;

		// Token: 0x04002A36 RID: 10806
		private const sbyte DiscardedState = -3;

		// Token: 0x04002A37 RID: 10807
		private List<IThingHolder> tmpHolders;

		// Token: 0x04002A38 RID: 10808
		private bool cached;

		// Token: 0x04002A39 RID: 10809
		private bool cachedIsHolder;

		// Token: 0x04002A3A RID: 10810
		private IThingHolder cachedHolder;

		// Token: 0x04002A3B RID: 10811
		private IThingHolderTickable cachedTickable;

		// Token: 0x04002A3C RID: 10812
		public static bool allowDestroyNonDestroyable = false;

		// Token: 0x04002A3D RID: 10813
		private static Dictionary<Thing, string> facIDsCached = new Dictionary<Thing, string>();

		// Token: 0x04002A3E RID: 10814
		private static List<string> tmpDeteriorationReasons = new List<string>();

		// Token: 0x04002A3F RID: 10815
		public static HashSet<RitualPatternDef> showingGizmosForRitualsTmp = new HashSet<RitualPatternDef>();

		// Token: 0x04002A40 RID: 10816
		private static List<string> tmpIdeoNames = new List<string>();

		// Token: 0x04002A41 RID: 10817
		public const float SmeltCostRecoverFraction = 0.25f;
	}
}
