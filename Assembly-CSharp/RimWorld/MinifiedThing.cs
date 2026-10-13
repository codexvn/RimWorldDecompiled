using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002943 RID: 10563
	public class MinifiedThing : ThingWithComps, IThingHolder
	{
		// Token: 0x1700231E RID: 8990
		// (get) Token: 0x0600E743 RID: 59203 RVA: 0x004364AF File Offset: 0x004346AF
		// (set) Token: 0x0600E744 RID: 59204 RVA: 0x004364CC File Offset: 0x004346CC
		public Thing InnerThing
		{
			get
			{
				if (this.innerContainer.Count == 0)
				{
					return null;
				}
				return this.innerContainer[0];
			}
			set
			{
				if (value == this.InnerThing)
				{
					return;
				}
				if (value == null)
				{
					this.innerContainer.Clear();
					return;
				}
				if (this.innerContainer.Count != 0)
				{
					Log.Warning(string.Concat(new string[]
					{
						"Assigned 2 things to the same MinifiedThing ",
						this.ToStringSafe<MinifiedThing>(),
						" (first=",
						this.innerContainer[0].ToStringSafe<Thing>(),
						" second=",
						value.ToStringSafe<Thing>(),
						")"
					}));
					this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
				}
				this.innerContainer.TryAdd(value, true);
			}
		}

		// Token: 0x1700231F RID: 8991
		// (get) Token: 0x0600E745 RID: 59205 RVA: 0x00436570 File Offset: 0x00434770
		public override Graphic Graphic
		{
			get
			{
				if (this.cachedGraphic == null)
				{
					this.cachedGraphic = this.InnerThing.Graphic.ExtractInnerGraphicFor(this.InnerThing, null);
					if ((float)this.InnerThing.def.size.x > 1.1f || (float)this.InnerThing.def.size.z > 1.1f)
					{
						Vector2 minifiedDrawSize = this.GetMinifiedDrawSize(this.InnerThing.def.size.ToVector2(), 1.1f);
						Vector2 newDrawSize = new Vector2(minifiedDrawSize.x / (float)this.InnerThing.def.size.x * this.cachedGraphic.drawSize.x, minifiedDrawSize.y / (float)this.InnerThing.def.size.z * this.cachedGraphic.drawSize.y);
						this.cachedGraphic = this.cachedGraphic.GetCopy(newDrawSize, null);
					}
					if (Math.Abs(this.InnerThing.def.minifiedDrawScale - 1f) > 1E-45f)
					{
						this.cachedGraphic = this.cachedGraphic.GetCopy(new Vector2(this.InnerThing.def.minifiedDrawScale * this.cachedGraphic.drawSize.x, this.InnerThing.def.minifiedDrawScale * this.cachedGraphic.drawSize.y), null);
					}
				}
				return this.cachedGraphic;
			}
		}

		// Token: 0x17002320 RID: 8992
		// (get) Token: 0x0600E746 RID: 59206 RVA: 0x00436702 File Offset: 0x00434902
		public override string LabelNoCount
		{
			get
			{
				return this.InnerThing.LabelNoCount;
			}
		}

		// Token: 0x17002321 RID: 8993
		// (get) Token: 0x0600E747 RID: 59207 RVA: 0x0043670F File Offset: 0x0043490F
		public override string DescriptionDetailed
		{
			get
			{
				return this.InnerThing.DescriptionDetailed;
			}
		}

		// Token: 0x17002322 RID: 8994
		// (get) Token: 0x0600E748 RID: 59208 RVA: 0x0043671C File Offset: 0x0043491C
		public override string DescriptionFlavor
		{
			get
			{
				return this.InnerThing.DescriptionFlavor;
			}
		}

		// Token: 0x17002323 RID: 8995
		// (get) Token: 0x0600E749 RID: 59209 RVA: 0x00436729 File Offset: 0x00434929
		public override ModContentPack ContentSource
		{
			get
			{
				return this.InnerThing.ContentSource;
			}
		}

		// Token: 0x17002324 RID: 8996
		// (get) Token: 0x0600E74A RID: 59210 RVA: 0x00436736 File Offset: 0x00434936
		private Graphic CrateFrontGraphic
		{
			get
			{
				if (this.crateFrontGraphic == null)
				{
					this.crateFrontGraphic = this.LoadCrateFrontGraphic();
				}
				return this.crateFrontGraphic;
			}
		}

		// Token: 0x0600E74B RID: 59211 RVA: 0x00436752 File Offset: 0x00434952
		public static void TryInsertIntoAtlas()
		{
			GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Item, ContentFinder<Texture2D>.Get("Things/Item/Minified/CrateFront", true), null);
			GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Item, ContentFinder<Texture2D>.Get("Things/Item/Minified/BurlapBag", true), null);
		}

		// Token: 0x0600E74C RID: 59212 RVA: 0x0043677A File Offset: 0x0043497A
		public MinifiedThing()
		{
			this.innerContainer = new ThingOwner<Thing>(this, true, LookMode.Deep, true);
		}

		// Token: 0x0600E74D RID: 59213 RVA: 0x00436791 File Offset: 0x00434991
		protected virtual Graphic LoadCrateFrontGraphic()
		{
			return GraphicDatabase.Get<Graphic_Single>("Things/Item/Minified/CrateFront", ShaderDatabase.Cutout, this.GetMinifiedDrawSize(this.InnerThing.def.size.ToVector2(), 1.1f) * 1.16f, Color.white);
		}

		// Token: 0x0600E74E RID: 59214 RVA: 0x004367D1 File Offset: 0x004349D1
		protected override void Tick()
		{
			if (this.InnerThing == null)
			{
				Log.Error("MinifiedThing with null InnerThing. Destroying.");
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.Tick();
		}

		// Token: 0x0600E74F RID: 59215 RVA: 0x004367F3 File Offset: 0x004349F3
		protected override void TickInterval(int delta)
		{
			if (this.InnerThing == null && !base.Destroyed)
			{
				Log.Error("MinifiedThing with null InnerThing. Destroying.");
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.TickInterval(delta);
		}

		// Token: 0x0600E750 RID: 59216 RVA: 0x0043681E File Offset: 0x00434A1E
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x0600E751 RID: 59217 RVA: 0x00436826 File Offset: 0x00434A26
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600E752 RID: 59218 RVA: 0x00436834 File Offset: 0x00434A34
		public override Thing SplitOff(int count)
		{
			MinifiedThing minifiedThing = (MinifiedThing)base.SplitOff(count);
			if (minifiedThing != this)
			{
				minifiedThing.InnerThing = ThingMaker.MakeThing(this.InnerThing.def, this.InnerThing.Stuff);
				ThingWithComps thingWithComps = this.InnerThing as ThingWithComps;
				if (thingWithComps != null)
				{
					for (int i = 0; i < thingWithComps.AllComps.Count; i++)
					{
						thingWithComps.AllComps[i].PostSplitOff(minifiedThing.InnerThing);
					}
				}
			}
			return minifiedThing;
		}

		// Token: 0x0600E753 RID: 59219 RVA: 0x004368B0 File Offset: 0x00434AB0
		public override bool CanStackWith(Thing other)
		{
			MinifiedThing minifiedThing = other as MinifiedThing;
			return minifiedThing != null && base.CanStackWith(other) && this.InnerThing.CanStackWith(minifiedThing.InnerThing);
		}

		// Token: 0x0600E754 RID: 59220 RVA: 0x004368E5 File Offset: 0x00434AE5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
		}

		// Token: 0x0600E755 RID: 59221 RVA: 0x00436908 File Offset: 0x00434B08
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			Blueprint_Install blueprint_Install = InstallBlueprintUtility.ExistingBlueprintFor(this);
			if (blueprint_Install != null)
			{
				GenDraw.DrawLineBetween(this.TrueCenter(), blueprint_Install.TrueCenter());
			}
		}

		// Token: 0x0600E756 RID: 59222 RVA: 0x00436938 File Offset: 0x00434B38
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			if (this.InnerThing.def.minifiedManualDraw)
			{
				this.InnerThing.DrawNowAt(drawLoc, flip);
				this.CrateFrontGraphic.DrawFromDef(drawLoc + Altitudes.AltIncVect * 0.1f, Rot4.North, null, 0f);
				return;
			}
			this.CrateFrontGraphic.DrawFromDef(drawLoc + Altitudes.AltIncVect * 0.1f, Rot4.North, null, 0f);
			Rot4 rot = (this.Graphic is Graphic_Single) ? Rot4.North : Rot4.South;
			if (this.InnerThing.def.overrideMinifiedRot != Rot4.Invalid)
			{
				rot = this.InnerThing.def.overrideMinifiedRot;
			}
			Vector3 b = this.InnerThing.def.minifiedDrawOffset - this.Graphic.DrawOffset(rot);
			this.Graphic.Draw(drawLoc + b, rot, this, 0f);
		}

		// Token: 0x0600E757 RID: 59223 RVA: 0x00436A40 File Offset: 0x00434C40
		public override void Print(SectionLayer layer)
		{
			Vector3 drawPos = this.DrawPos;
			Material matSingle = this.CrateFrontGraphic.MatSingle;
			Vector2[] uvs;
			Color32 color;
			Graphic.TryGetTextureAtlasReplacementInfo(matSingle, TextureAtlasGroup.Item, false, false, out matSingle, out uvs, out color);
			Printer_Plane.PrintPlane(layer, drawPos + Altitudes.AltIncVect * 0.1f, this.CrateFrontGraphic.drawSize, matSingle, 0f, false, uvs, null, 0.01f, 0f);
			Rot4 rot = Rot4.South;
			if (this.Graphic is Graphic_Single)
			{
				rot = Rot4.North;
			}
			if (this.InnerThing.def.overrideMinifiedRot != Rot4.Invalid)
			{
				rot = this.InnerThing.def.overrideMinifiedRot;
			}
			Material mat = this.Graphic.MatAt(rot, this);
			Graphic.TryGetTextureAtlasReplacementInfo(mat, this.InnerThing.def.category.ToAtlasGroup(), false, false, out mat, out uvs, out color);
			Printer_Plane.PrintPlane(layer, drawPos + this.InnerThing.def.minifiedDrawOffset, this.Graphic.drawSize, mat, 0f, false, uvs, null, 0.01f, 0f);
		}

		// Token: 0x0600E758 RID: 59224 RVA: 0x00436B60 File Offset: 0x00434D60
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			bool spawned = base.Spawned;
			Map map = base.Map;
			base.Destroy(mode);
			if (this.InnerThing != null)
			{
				InstallBlueprintUtility.CancelBlueprintsFor(this);
				if (spawned)
				{
					if (mode == DestroyMode.Deconstruct)
					{
						SoundDefOf.Building_Deconstructed.PlayOneShot(new TargetInfo(base.Position, map, false));
						GenLeaving.DoLeavingsFor(this.InnerThing, map, mode, this.OccupiedRect(), null, null);
					}
					else if (mode == DestroyMode.KillFinalize)
					{
						GenLeaving.DoLeavingsFor(this.InnerThing, map, mode, this.OccupiedRect(), null, null);
					}
				}
				this.innerContainer.ClearAndDestroyContents(mode);
			}
		}

		// Token: 0x0600E759 RID: 59225 RVA: 0x00436BEF File Offset: 0x00434DEF
		public override void PreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
			base.PreTraded(action, playerNegotiator, trader);
			InstallBlueprintUtility.CancelBlueprintsFor(this);
		}

		// Token: 0x0600E75A RID: 59226 RVA: 0x00436C00 File Offset: 0x00434E00
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield return InstallationDesignatorDatabase.DesignatorFor(this.def);
			yield break;
			yield break;
		}

		// Token: 0x0600E75B RID: 59227 RVA: 0x00436C10 File Offset: 0x00434E10
		public override string GetInspectString()
		{
			string text = "NotInstalled".Translate();
			string inspectString = this.InnerThing.GetInspectString();
			if (!inspectString.NullOrEmpty())
			{
				text += "\n";
				text += inspectString;
			}
			return text;
		}

		// Token: 0x0600E75C RID: 59228 RVA: 0x00436C58 File Offset: 0x00434E58
		protected Vector2 GetMinifiedDrawSize(Vector2 drawSize, float maxSideLength)
		{
			float num = maxSideLength / Mathf.Max(drawSize.x, drawSize.y);
			if (num >= 1f)
			{
				return drawSize;
			}
			return drawSize * num;
		}

		// Token: 0x04009A8B RID: 39563
		public const float MaxMinifiedGraphicSize = 1.1f;

		// Token: 0x04009A8C RID: 39564
		public const float CrateToGraphicScale = 1.16f;

		// Token: 0x04009A8D RID: 39565
		private ThingOwner innerContainer;

		// Token: 0x04009A8E RID: 39566
		protected Graphic cachedGraphic;

		// Token: 0x04009A8F RID: 39567
		private Graphic crateFrontGraphic;
	}
}
