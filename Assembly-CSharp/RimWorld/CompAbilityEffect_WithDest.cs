using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020022AB RID: 8875
	public abstract class CompAbilityEffect_WithDest : CompAbilityEffect, ITargetingSource
	{
		// Token: 0x17001D84 RID: 7556
		// (get) Token: 0x0600C10B RID: 49419 RVA: 0x0037E897 File Offset: 0x0037CA97
		public new CompProperties_EffectWithDest Props
		{
			get
			{
				return (CompProperties_EffectWithDest)this.props;
			}
		}

		// Token: 0x17001D85 RID: 7557
		// (get) Token: 0x0600C10C RID: 49420 RVA: 0x0037E8A4 File Offset: 0x0037CAA4
		public virtual TargetingParameters targetParams
		{
			get
			{
				return new TargetingParameters
				{
					canTargetLocations = true
				};
			}
		}

		// Token: 0x17001D86 RID: 7558
		// (get) Token: 0x0600C10D RID: 49421 RVA: 0x00002501 File Offset: 0x00000701
		public bool MultiSelect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001D87 RID: 7559
		// (get) Token: 0x0600C10E RID: 49422 RVA: 0x00375CA4 File Offset: 0x00373EA4
		public Thing Caster
		{
			get
			{
				return this.parent.pawn;
			}
		}

		// Token: 0x17001D88 RID: 7560
		// (get) Token: 0x0600C10F RID: 49423 RVA: 0x00375CA4 File Offset: 0x00373EA4
		public Pawn CasterPawn
		{
			get
			{
				return this.parent.pawn;
			}
		}

		// Token: 0x17001D89 RID: 7561
		// (get) Token: 0x0600C110 RID: 49424 RVA: 0x00002C42 File Offset: 0x00000E42
		public Verb GetVerb
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001D8A RID: 7562
		// (get) Token: 0x0600C111 RID: 49425 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool CasterIsPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001D8B RID: 7563
		// (get) Token: 0x0600C112 RID: 49426 RVA: 0x00002501 File Offset: 0x00000701
		public bool IsMeleeAttack
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001D8C RID: 7564
		// (get) Token: 0x0600C113 RID: 49427 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool Targetable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001D8D RID: 7565
		// (get) Token: 0x0600C114 RID: 49428 RVA: 0x0037E8B2 File Offset: 0x0037CAB2
		public Texture2D UIIcon
		{
			get
			{
				return BaseContent.BadTex;
			}
		}

		// Token: 0x17001D8E RID: 7566
		// (get) Token: 0x0600C115 RID: 49429 RVA: 0x00002C42 File Offset: 0x00000E42
		public ITargetingSource DestinationSelector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001D8F RID: 7567
		// (get) Token: 0x0600C116 RID: 49430 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool HidePawnTooltips
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C117 RID: 49431 RVA: 0x0037E8BC File Offset: 0x0037CABC
		public LocalTargetInfo GetDestination(LocalTargetInfo target)
		{
			Map map = this.parent.pawn.Map;
			switch (this.Props.destination)
			{
			case AbilityEffectDestination.Caster:
				return new LocalTargetInfo(this.parent.pawn.InteractionCell);
			case AbilityEffectDestination.RandomInRange:
			{
				this.cells.Clear();
				int num = GenRadial.NumCellsInRadius(this.Props.randomRange.max);
				for (int i = 0; i < num; i++)
				{
					IntVec3 intVec = GenRadial.RadialPattern[i];
					if (intVec.DistanceTo(IntVec3.Zero) >= this.Props.randomRange.min)
					{
						IntVec3 intVec2 = target.Cell + intVec;
						if (intVec2.Standable(map) && (!this.Props.requiresLineOfSight || GenSight.LineOfSight(target.Cell, intVec2, map)) && !intVec2.Fogged(map))
						{
							this.cells.Add(intVec2);
						}
					}
				}
				if (this.cells.Any<IntVec3>())
				{
					return new LocalTargetInfo(this.cells.RandomElement<IntVec3>());
				}
				Messages.Message("NoValidDestinationFound".Translate(this.parent.def.LabelCap), MessageTypeDefOf.RejectInput, true);
				return LocalTargetInfo.Invalid;
			}
			case AbilityEffectDestination.Selected:
				return target;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x0600C118 RID: 49432 RVA: 0x0037EA18 File Offset: 0x0037CC18
		public bool CanPlaceSelectedTargetAt(LocalTargetInfo target)
		{
			Pawn pawn = this.selectedTarget.Pawn;
			if (pawn != null)
			{
				Building_Door door = target.Cell.GetDoor(pawn.MapHeld);
				return (door == null || door.CanPhysicallyPass(pawn)) && (pawn.Spawned && !target.Cell.Impassable(this.parent.pawn.Map)) && target.Cell.WalkableBy(this.parent.pawn.Map, pawn);
			}
			return CompAbilityEffect_WithDest.CanTeleportThingTo(target, this.parent.pawn.Map);
		}

		// Token: 0x0600C119 RID: 49433 RVA: 0x0037EAB4 File Offset: 0x0037CCB4
		public static bool CanTeleportThingTo(LocalTargetInfo target, Map map)
		{
			Building edifice = target.Cell.GetEdifice(map);
			if (edifice != null && edifice.def.surfaceType != SurfaceType.Item && edifice.def.surfaceType != SurfaceType.Eat)
			{
				Building_Door building_Door = edifice as Building_Door;
				if (building_Door == null || !building_Door.Open)
				{
					return false;
				}
			}
			List<Thing> thingList = target.Cell.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.category == ThingCategory.Item)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600C11A RID: 49434 RVA: 0x0037EB38 File Offset: 0x0037CD38
		public virtual bool CanHitTarget(LocalTargetInfo target)
		{
			return target.IsValid && (this.Props.range <= 0f || target.Cell.DistanceTo(this.selectedTarget.Cell) <= this.Props.range) && (!this.Props.requiresLineOfSight || GenSight.LineOfSight(this.selectedTarget.Cell, target.Cell, this.parent.pawn.Map));
		}

		// Token: 0x0600C11B RID: 49435 RVA: 0x0037EBC1 File Offset: 0x0037CDC1
		public virtual bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			return this.CanHitTarget(target);
		}

		// Token: 0x0600C11C RID: 49436 RVA: 0x0037EBCC File Offset: 0x0037CDCC
		public void DrawHighlight(LocalTargetInfo target)
		{
			if (this.Props.range > 0f)
			{
				if (this.Props.requiresLineOfSight)
				{
					GenDraw.DrawRadiusRing(this.selectedTarget.Cell, this.Props.range, Color.white, (IntVec3 c) => GenSight.LineOfSight(this.selectedTarget.Cell, c, this.parent.pawn.Map) && this.CanPlaceSelectedTargetAt(c));
				}
				else
				{
					GenDraw.DrawRadiusRing(this.selectedTarget.Cell, this.Props.range);
				}
			}
			if (target.IsValid)
			{
				GenDraw.DrawTargetHighlight(target);
			}
		}

		// Token: 0x0600C11D RID: 49437 RVA: 0x0037EC50 File Offset: 0x0037CE50
		public void OnGUI(LocalTargetInfo target)
		{
			Texture2D icon;
			if (target.IsValid)
			{
				icon = this.parent.def.uiIcon;
			}
			else
			{
				icon = TexCommand.CannotShoot;
			}
			GenUI.DrawMouseAttachment(icon);
			string text = this.ExtraLabelMouseAttachment(target);
			if (!text.NullOrEmpty())
			{
				Widgets.MouseAttachedLabel(text, 0f, 0f, null);
			}
		}

		// Token: 0x0600C11E RID: 49438 RVA: 0x0037ECAE File Offset: 0x0037CEAE
		public void OrderForceTarget(LocalTargetInfo target)
		{
			this.parent.QueueCastingJob(this.selectedTarget, target);
			this.selectedTarget = LocalTargetInfo.Invalid;
		}

		// Token: 0x0600C11F RID: 49439 RVA: 0x0037ECCD File Offset: 0x0037CECD
		public void SetTarget(LocalTargetInfo target)
		{
			this.selectedTarget = target;
		}

		// Token: 0x0600C120 RID: 49440 RVA: 0x0037ECD6 File Offset: 0x0037CED6
		public virtual void SelectDestination()
		{
			Find.Targeter.BeginTargeting(this, null, false, null, null, true);
		}

		// Token: 0x0600C121 RID: 49441 RVA: 0x0037ECE8 File Offset: 0x0037CEE8
		public bool SelectedTargetInvalidated()
		{
			return this.selectedTarget.HasThing && !this.selectedTarget.Thing.Spawned;
		}

		// Token: 0x0400841B RID: 33819
		protected LocalTargetInfo selectedTarget = LocalTargetInfo.Invalid;

		// Token: 0x0400841C RID: 33820
		private List<IntVec3> cells = new List<IntVec3>();
	}
}
