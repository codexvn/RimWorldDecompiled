using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020022A2 RID: 8866
	public class CompAbilityEffect_Wallraise : CompAbilityEffect
	{
		// Token: 0x17001D7F RID: 7551
		// (get) Token: 0x0600C0E1 RID: 49377 RVA: 0x0037DCEC File Offset: 0x0037BEEC
		public new CompProperties_AbilityWallraise Props
		{
			get
			{
				return (CompProperties_AbilityWallraise)this.props;
			}
		}

		// Token: 0x0600C0E2 RID: 49378 RVA: 0x0037DCFC File Offset: 0x0037BEFC
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Map map = this.parent.pawn.Map;
			List<Thing> list = new List<Thing>();
			list.AddRange(this.AffectedCells(target, map).SelectMany((IntVec3 c) => from t in c.GetThingList(map)
			where t.def.category == ThingCategory.Item
			select t));
			foreach (Thing thing in list)
			{
				thing.DeSpawn(DestroyMode.Vanish);
			}
			foreach (IntVec3 intVec in this.AffectedCells(target, map))
			{
				GenSpawn.Spawn(ThingDefOf.RaisedRocks, intVec, map, WipeMode.Vanish);
				FleckMaker.ThrowDustPuffThick(intVec.ToVector3Shifted(), map, Rand.Range(1.5f, 3f), CompAbilityEffect_Wallraise.DustColor);
				CompAbilityEffect_Teleport.SendSkipUsedSignal(intVec, this.parent.pawn);
			}
			foreach (Thing thing2 in list)
			{
				IntVec3 intVec2 = IntVec3.Invalid;
				for (int i = 0; i < 9; i++)
				{
					IntVec3 intVec3 = thing2.Position + GenRadial.RadialPattern[i];
					if (intVec3.InBounds(map) && intVec3.Walkable(map) && map.thingGrid.ThingsListAtFast(intVec3).Count <= 0)
					{
						intVec2 = intVec3;
						break;
					}
				}
				if (intVec2 != IntVec3.Invalid)
				{
					GenSpawn.Spawn(thing2, intVec2, map, WipeMode.Vanish);
				}
				else
				{
					GenPlace.TryPlaceThing(thing2, thing2.Position, map, ThingPlaceMode.Near, null, null, null, 1);
				}
			}
		}

		// Token: 0x0600C0E3 RID: 49379 RVA: 0x0037DF18 File Offset: 0x0037C118
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Valid(target, true);
		}

		// Token: 0x0600C0E4 RID: 49380 RVA: 0x0037DF24 File Offset: 0x0037C124
		public override void DrawEffectPreview(LocalTargetInfo target)
		{
			GenDraw.DrawFieldEdges(this.AffectedCells(target, this.parent.pawn.Map).ToList<IntVec3>(), this.Valid(target, false) ? Color.white : Color.red, null, null, 2900);
		}

		// Token: 0x0600C0E5 RID: 49381 RVA: 0x0037DF77 File Offset: 0x0037C177
		private IEnumerable<IntVec3> AffectedCells(LocalTargetInfo target, Map map)
		{
			foreach (IntVec2 intVec in this.Props.pattern)
			{
				IntVec3 intVec2 = target.Cell + new IntVec3(intVec.x, 0, intVec.z);
				if (intVec2.InBounds(map))
				{
					yield return intVec2;
				}
			}
			List<IntVec2>.Enumerator enumerator = default(List<IntVec2>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600C0E6 RID: 49382 RVA: 0x0037DF98 File Offset: 0x0037C198
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			if (this.AffectedCells(target, this.parent.pawn.Map).Any((IntVec3 c) => c.Filled(this.parent.pawn.Map)))
			{
				if (throwMessages)
				{
					Messages.Message("CannotUseAbility".Translate(this.parent.def.label) + ": " + "AbilityOccupiedCells".Translate(), target.ToTargetInfo(this.parent.pawn.Map), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (this.AffectedCells(target, this.parent.pawn.Map).Any((IntVec3 c) => !c.Standable(this.parent.pawn.Map)))
			{
				if (throwMessages)
				{
					Messages.Message("CannotUseAbility".Translate(this.parent.def.label) + ": " + "AbilityUnwalkable".Translate(), target.ToTargetInfo(this.parent.pawn.Map), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (this.AffectedCells(target, this.parent.pawn.Map).Any((IntVec3 c) => c.GetThingList(this.parent.pawn.Map).Any((Thing t) => !t.def.destroyable)))
			{
				if (throwMessages)
				{
					Messages.Message("CannotUseAbility".Translate(this.parent.def.label) + ": " + "AbilityNotEnoughFreeSpace".Translate(), target.ToTargetInfo(this.parent.pawn.Map), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x040083FA RID: 33786
		public static Color DustColor = new Color(0.55f, 0.55f, 0.55f, 4f);
	}
}
