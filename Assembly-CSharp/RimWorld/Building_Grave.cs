using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A10 RID: 10768
	public class Building_Grave : Building_CorpseCasket, INotifyHauledTo
	{
		// Token: 0x170024E9 RID: 9449
		// (get) Token: 0x0600EE42 RID: 60994 RVA: 0x00452318 File Offset: 0x00450518
		public Pawn AssignedPawn
		{
			get
			{
				if (this.CompAssignableToPawn == null || !this.CompAssignableToPawn.AssignedPawnsForReading.Any<Pawn>())
				{
					return null;
				}
				return this.CompAssignableToPawn.AssignedPawnsForReading[0];
			}
		}

		// Token: 0x170024EA RID: 9450
		// (get) Token: 0x0600EE43 RID: 60995 RVA: 0x00452347 File Offset: 0x00450547
		public CompAssignableToPawn_Grave CompAssignableToPawn
		{
			get
			{
				return base.GetComp<CompAssignableToPawn_Grave>();
			}
		}

		// Token: 0x170024EB RID: 9451
		// (get) Token: 0x0600EE44 RID: 60996 RVA: 0x00452350 File Offset: 0x00450550
		public override Graphic Graphic
		{
			get
			{
				if (!base.HasCorpse)
				{
					return base.Graphic;
				}
				if (this.def.building.fullGraveGraphicData == null)
				{
					return base.Graphic;
				}
				if (this.cachedGraphicFull == null)
				{
					this.cachedGraphicFull = this.def.building.fullGraveGraphicData.GraphicColoredFor(this);
				}
				return this.cachedGraphicFull;
			}
		}

		// Token: 0x170024EC RID: 9452
		// (get) Token: 0x0600EE45 RID: 60997 RVA: 0x004523AF File Offset: 0x004505AF
		public override bool StorageTabVisible
		{
			get
			{
				return base.StorageTabVisible && this.AssignedPawn == null;
			}
		}

		// Token: 0x0600EE46 RID: 60998 RVA: 0x004523C4 File Offset: 0x004505C4
		public override void EjectContents()
		{
			base.EjectContents();
			if (base.Spawned)
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Things);
			}
		}

		// Token: 0x0600EE47 RID: 60999 RVA: 0x004523F4 File Offset: 0x004505F4
		public virtual void Notify_HauledTo(Pawn hauler, Thing thing, int count)
		{
			CompArt comp = base.GetComp<CompArt>();
			if (comp != null && !comp.Active && hauler.RaceProps.Humanlike)
			{
				comp.JustCreatedBy(hauler);
				comp.InitializeArt(base.Corpse.InnerPawn);
			}
			base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Buildings | MapMeshFlagDefOf.Things);
			hauler.records.Increment(RecordDefOf.CorpsesBuried);
			TaleDef buriedCorpse = TaleDefOf.BuriedCorpse;
			object[] array = new object[2];
			array[0] = hauler;
			int num = 1;
			Corpse corpse = base.Corpse;
			array[num] = ((corpse != null) ? corpse.InnerPawn : null);
			TaleRecorder.RecordTale(buriedCorpse, array);
		}

		// Token: 0x0600EE48 RID: 61000 RVA: 0x004524A0 File Offset: 0x004506A0
		public override bool Accepts(Thing thing)
		{
			if (!base.Accepts(thing))
			{
				return false;
			}
			if (base.HasCorpse)
			{
				return false;
			}
			if (this.AssignedPawn != null)
			{
				Corpse corpse = thing as Corpse;
				if (corpse == null)
				{
					return false;
				}
				if (corpse.InnerPawn != this.AssignedPawn)
				{
					return false;
				}
			}
			else if (!this.storageSettings.AllowedToAccept(thing))
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600EE49 RID: 61001 RVA: 0x004524F8 File Offset: 0x004506F8
		public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
		{
			if (base.TryAcceptThing(thing, allowSpecialEffects))
			{
				Corpse corpse = thing as Corpse;
				if (corpse != null && corpse.InnerPawn.ownership != null && corpse.InnerPawn.ownership.AssignedGrave != this)
				{
					corpse.InnerPawn.ownership.UnclaimGrave();
				}
				if (base.Spawned)
				{
					base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Things);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600EE4A RID: 61002 RVA: 0x00452575 File Offset: 0x00450775
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.StorageTabVisible)
			{
				foreach (Gizmo gizmo2 in StorageSettingsClipboard.CopyPasteGizmosFor(this.storageSettings))
				{
					yield return gizmo2;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600EE4B RID: 61003 RVA: 0x00452585 File Offset: 0x00450785
		public override void Notify_ColorChanged()
		{
			base.Notify_ColorChanged();
			this.cachedGraphicFull = null;
		}

		// Token: 0x0600EE4C RID: 61004 RVA: 0x00452594 File Offset: 0x00450794
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (base.HasCorpse)
			{
				if (base.Tile.Valid)
				{
					string value = GenDate.DateFullStringAt((long)GenDate.TickGameToAbs(base.Corpse.timeOfDeath), Find.WorldGrid.LongLatOf(base.Tile));
					stringBuilder.AppendLine();
					stringBuilder.Append("DiedOn".Translate(value));
				}
			}
			else if (this.AssignedPawn != null)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("AssignedColonist".Translate());
				stringBuilder.Append(": ");
				stringBuilder.Append(this.AssignedPawn.LabelCap);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04009D9A RID: 40346
		private Graphic cachedGraphicFull;
	}
}
