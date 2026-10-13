using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000905 RID: 2309
	public abstract class ThingComp
	{
		// Token: 0x17000B15 RID: 2837
		// (get) Token: 0x06003B96 RID: 15254 RVA: 0x0013A73A File Offset: 0x0013893A
		public IThingHolder ParentHolder
		{
			get
			{
				return this.parent.ParentHolder;
			}
		}

		// Token: 0x06003B97 RID: 15255 RVA: 0x0013A747 File Offset: 0x00138947
		public virtual void Initialize(CompProperties props)
		{
			this.props = props;
		}

		// Token: 0x06003B98 RID: 15256 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ReceiveCompSignal(string signal)
		{
		}

		// Token: 0x06003B99 RID: 15257 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostExposeData()
		{
		}

		// Token: 0x06003B9A RID: 15258 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostSpawnSetup(bool respawningAfterLoad)
		{
		}

		// Token: 0x06003B9B RID: 15259 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostMapInit()
		{
		}

		// Token: 0x06003B9C RID: 15260 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
		}

		// Token: 0x06003B9D RID: 15261 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostDestroy(DestroyMode mode, Map previousMap)
		{
		}

		// Token: 0x06003B9E RID: 15262 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostPostMake()
		{
		}

		// Token: 0x06003B9F RID: 15263 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void CompTick()
		{
		}

		// Token: 0x06003BA0 RID: 15264 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void CompTickInterval(int delta)
		{
		}

		// Token: 0x06003BA1 RID: 15265 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void CompTickRare()
		{
		}

		// Token: 0x06003BA2 RID: 15266 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void CompTickLong()
		{
		}

		// Token: 0x06003BA3 RID: 15267 RVA: 0x00131DB4 File Offset: 0x0012FFB4
		public virtual void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			absorbed = false;
		}

		// Token: 0x06003BA4 RID: 15268 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		// Token: 0x06003BA5 RID: 15269 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void DrawAt(Vector3 drawLoc, bool flip = false)
		{
		}

		// Token: 0x06003BA6 RID: 15270 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostDraw()
		{
		}

		// Token: 0x06003BA7 RID: 15271 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostDrawExtraSelectionOverlays()
		{
		}

		// Token: 0x06003BA8 RID: 15272 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostPrintOnto(SectionLayer layer)
		{
		}

		// Token: 0x06003BA9 RID: 15273 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void CompPrintForPowerGrid(SectionLayer layer)
		{
		}

		// Token: 0x06003BAA RID: 15274 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PreAbsorbStack(Thing otherStack, int count)
		{
		}

		// Token: 0x06003BAB RID: 15275 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostSplitOff(Thing piece)
		{
		}

		// Token: 0x06003BAC RID: 15276 RVA: 0x00009C2B File Offset: 0x00007E2B
		public virtual string TransformLabel(string label)
		{
			return label;
		}

		// Token: 0x06003BAD RID: 15277 RVA: 0x000B7861 File Offset: 0x000B5A61
		public virtual IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			return Enumerable.Empty<Gizmo>();
		}

		// Token: 0x06003BAE RID: 15278 RVA: 0x000B7861 File Offset: 0x000B5A61
		public virtual IEnumerable<Gizmo> CompGetWornGizmosExtra()
		{
			return Enumerable.Empty<Gizmo>();
		}

		// Token: 0x06003BAF RID: 15279 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool AllowStackWith(Thing other)
		{
			return true;
		}

		// Token: 0x06003BB0 RID: 15280 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string CompInspectStringExtra()
		{
			return null;
		}

		// Token: 0x06003BB1 RID: 15281 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string CompTipStringExtra()
		{
			return null;
		}

		// Token: 0x06003BB2 RID: 15282 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string GetDescriptionPart()
		{
			return null;
		}

		// Token: 0x06003BB3 RID: 15283 RVA: 0x000FB0F4 File Offset: 0x000F92F4
		public virtual IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			return Enumerable.Empty<FloatMenuOption>();
		}

		// Token: 0x06003BB4 RID: 15284 RVA: 0x000FB0F4 File Offset: 0x000F92F4
		public virtual IEnumerable<FloatMenuOption> CompMultiSelectFloatMenuOptions(IEnumerable<Pawn> selPawns)
		{
			return Enumerable.Empty<FloatMenuOption>();
		}

		// Token: 0x06003BB5 RID: 15285 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PrePreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
		}

		// Token: 0x06003BB6 RID: 15286 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PrePostIngested(Pawn ingester)
		{
		}

		// Token: 0x06003BB7 RID: 15287 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostIngested(Pawn ingester)
		{
		}

		// Token: 0x06003BB8 RID: 15288 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostPostGeneratedForTrader(TraderKindDef trader, PlanetTile forTile, Faction forFaction)
		{
		}

		// Token: 0x06003BB9 RID: 15289 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_SignalReceived(Signal signal)
		{
		}

		// Token: 0x06003BBA RID: 15290 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_LordDestroyed()
		{
		}

		// Token: 0x06003BBB RID: 15291 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_MapRemoved()
		{
		}

		// Token: 0x06003BBC RID: 15292 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void DrawGUIOverlay()
		{
		}

		// Token: 0x06003BBD RID: 15293 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			return null;
		}

		// Token: 0x06003BBE RID: 15294 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_Equipped(Pawn pawn)
		{
		}

		// Token: 0x06003BBF RID: 15295 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_Unequipped(Pawn pawn)
		{
		}

		// Token: 0x06003BC0 RID: 15296 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_UsedVerb(Pawn pawn, Verb verb)
		{
		}

		// Token: 0x06003BC1 RID: 15297 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_UsedWeapon(Pawn pawn)
		{
		}

		// Token: 0x06003BC2 RID: 15298 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
		{
		}

		// Token: 0x06003BC3 RID: 15299 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_KilledPawn(Pawn pawn)
		{
		}

		// Token: 0x06003BC4 RID: 15300 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PassedToWorld()
		{
		}

		// Token: 0x06003BC5 RID: 15301 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_WearerDied()
		{
		}

		// Token: 0x06003BC6 RID: 15302 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_Downed()
		{
		}

		// Token: 0x06003BC7 RID: 15303 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_Released()
		{
		}

		// Token: 0x06003BC8 RID: 15304 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_DefsHotReloaded()
		{
		}

		// Token: 0x06003BC9 RID: 15305 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_AddBedThoughts(Pawn pawn)
		{
		}

		// Token: 0x06003BCA RID: 15306 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_AbandonedAtTile(PlanetTile tile)
		{
		}

		// Token: 0x06003BCB RID: 15307 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_KilledLeavingsLeft(List<Thing> leavings)
		{
		}

		// Token: 0x06003BCC RID: 15308 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_Arrested(bool succeeded)
		{
		}

		// Token: 0x06003BCD RID: 15309 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PrisonBreakout()
		{
		}

		// Token: 0x06003BCE RID: 15310 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_Hacked(Pawn hacker = null)
		{
		}

		// Token: 0x06003BCF RID: 15311 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_ColorChanged()
		{
		}

		// Token: 0x06003BD0 RID: 15312 RVA: 0x0013A750 File Offset: 0x00138950
		public virtual IEnumerable<ThingDefCountClass> GetAdditionalLeavings(Map map, DestroyMode mode)
		{
			return Enumerable.Empty<ThingDefCountClass>();
		}

		// Token: 0x06003BD1 RID: 15313 RVA: 0x0013A750 File Offset: 0x00138950
		public virtual IEnumerable<ThingDefCountClass> GetAdditionalHarvestYield()
		{
			return Enumerable.Empty<ThingDefCountClass>();
		}

		// Token: 0x06003BD2 RID: 15314 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void CompDrawWornExtras()
		{
		}

		// Token: 0x06003BD3 RID: 15315 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CompAllowVerbCast(Verb verb)
		{
			return true;
		}

		// Token: 0x06003BD4 RID: 15316 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool CompPreventClaimingBy(Faction faction)
		{
			return false;
		}

		// Token: 0x06003BD5 RID: 15317 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool CompForceDeconstructable()
		{
			return false;
		}

		// Token: 0x06003BD6 RID: 15318 RVA: 0x00006AAC File Offset: 0x00004CAC
		public virtual float CompGetSpecialApparelScoreOffset()
		{
			return 0f;
		}

		// Token: 0x06003BD7 RID: 15319 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_RecipeProduced(Pawn pawn)
		{
		}

		// Token: 0x06003BD8 RID: 15320 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_DuplicatedFrom(Pawn source)
		{
		}

		// Token: 0x06003BD9 RID: 15321 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_BecameVisible()
		{
		}

		// Token: 0x06003BDA RID: 15322 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_BecameInvisible()
		{
		}

		// Token: 0x06003BDB RID: 15323 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_ForcedVisible()
		{
		}

		// Token: 0x06003BDC RID: 15324 RVA: 0x0004FC72 File Offset: 0x0004DE72
		public virtual float GetStatFactor(StatDef stat)
		{
			return 1f;
		}

		// Token: 0x06003BDD RID: 15325 RVA: 0x00006AAC File Offset: 0x00004CAC
		public virtual float GetStatOffset(StatDef stat)
		{
			return 0f;
		}

		// Token: 0x06003BDE RID: 15326 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void GetStatsExplanation(StatDef stat, StringBuilder sb, string whitespace = "")
		{
		}

		// Token: 0x06003BDF RID: 15327 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PreSwapMap()
		{
		}

		// Token: 0x06003BE0 RID: 15328 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostSwapMap()
		{
		}

		// Token: 0x06003BE1 RID: 15329 RVA: 0x0013A758 File Offset: 0x00138958
		public virtual Color? ForceColor()
		{
			return null;
		}

		// Token: 0x06003BE2 RID: 15330 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool DontDrawParent()
		{
			return false;
		}

		// Token: 0x06003BE3 RID: 15331 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool WantHoldWeapon(Pawn pawn)
		{
			return false;
		}

		// Token: 0x06003BE4 RID: 15332 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual List<PawnRenderNode> CompRenderNodes()
		{
			return null;
		}

		// Token: 0x06003BE5 RID: 15333 RVA: 0x0013A770 File Offset: 0x00138970
		public override string ToString()
		{
			string[] array = new string[6];
			array[0] = base.GetType().Name;
			array[1] = "(parent=";
			int num = 2;
			ThingWithComps thingWithComps = this.parent;
			array[num] = ((thingWithComps != null) ? thingWithComps.ToString() : null);
			array[3] = " at=";
			int num2 = 4;
			ThingWithComps thingWithComps2 = this.parent;
			array[num2] = ((thingWithComps2 != null) ? thingWithComps2.Position : IntVec3.Invalid).ToString();
			array[5] = ")";
			return string.Concat(array);
		}

		// Token: 0x04002AFA RID: 11002
		public ThingWithComps parent;

		// Token: 0x04002AFB RID: 11003
		public CompProperties props;
	}
}
