using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020024E2 RID: 9442
	public class Pawn_StyleTracker : IExposable
	{
		// Token: 0x17001F98 RID: 8088
		// (get) Token: 0x0600CE26 RID: 52774 RVA: 0x003C8823 File Offset: 0x003C6A23
		// (set) Token: 0x0600CE27 RID: 52775 RVA: 0x003C882B File Offset: 0x003C6A2B
		public TattooDef FaceTattoo
		{
			get
			{
				return this.faceTattoo;
			}
			set
			{
				if (!ModLister.CheckIdeology("Tattoos"))
				{
					return;
				}
				this.faceTattoo = value;
			}
		}

		// Token: 0x17001F99 RID: 8089
		// (get) Token: 0x0600CE28 RID: 52776 RVA: 0x003C8841 File Offset: 0x003C6A41
		// (set) Token: 0x0600CE29 RID: 52777 RVA: 0x003C8849 File Offset: 0x003C6A49
		public TattooDef BodyTattoo
		{
			get
			{
				return this.bodyTattoo;
			}
			set
			{
				if (!ModLister.CheckIdeology("Tattoos"))
				{
					return;
				}
				this.bodyTattoo = value;
			}
		}

		// Token: 0x17001F9A RID: 8090
		// (get) Token: 0x0600CE2A RID: 52778 RVA: 0x003C885F File Offset: 0x003C6A5F
		public bool ShouldSpawnHairFilth
		{
			get
			{
				return (this.nextHairDef != null || this.nextBeardDef != null) && (this.pawn.story.hairDef != this.nextHairDef || this.beardDef != this.nextBeardDef);
			}
		}

		// Token: 0x17001F9B RID: 8091
		// (get) Token: 0x0600CE2B RID: 52779 RVA: 0x003C88A0 File Offset: 0x003C6AA0
		public bool CanWantBeard
		{
			get
			{
				return (this.pawn.gender != Gender.Female || (this.pawn.genes != null && this.pawn.genes.CanHaveBeard)) && this.pawn.DevelopmentalStage.Adult();
			}
		}

		// Token: 0x17001F9C RID: 8092
		// (get) Token: 0x0600CE2C RID: 52780 RVA: 0x003C88F1 File Offset: 0x003C6AF1
		public bool LookChangeDesired
		{
			get
			{
				return this.lookChangeDesired && this.CanDesireLookChange;
			}
		}

		// Token: 0x17001F9D RID: 8093
		// (get) Token: 0x0600CE2D RID: 52781 RVA: 0x003C8904 File Offset: 0x003C6B04
		public bool CanDesireLookChange
		{
			get
			{
				return ModsConfig.IdeologyActive && this.pawn.IsColonistPlayerControlled && !this.pawn.guest.IsSlave && !this.pawn.IsQuestLodger() && !this.pawn.IsCreepJoiner;
			}
		}

		// Token: 0x17001F9E RID: 8094
		// (get) Token: 0x0600CE2E RID: 52782 RVA: 0x003C8954 File Offset: 0x003C6B54
		public bool HasAnyUnwantedStyleItem
		{
			get
			{
				return this.HasUnwantedHairStyle || this.HasUnwantedBeard || this.HasUnwantedFaceTattoo || this.HasUnwantedBodyTattoo;
			}
		}

		// Token: 0x17001F9F RID: 8095
		// (get) Token: 0x0600CE2F RID: 52783 RVA: 0x003C8978 File Offset: 0x003C6B78
		public bool HasUnwantedHairStyle
		{
			get
			{
				return !PawnStyleItemChooser.WantsToUseStyle(this.pawn, this.pawn.story.hairDef, null);
			}
		}

		// Token: 0x17001FA0 RID: 8096
		// (get) Token: 0x0600CE30 RID: 52784 RVA: 0x003C89AC File Offset: 0x003C6BAC
		public bool HasUnwantedBeard
		{
			get
			{
				return !PawnStyleItemChooser.WantsToUseStyle(this.pawn, this.beardDef, null);
			}
		}

		// Token: 0x17001FA1 RID: 8097
		// (get) Token: 0x0600CE31 RID: 52785 RVA: 0x003C89D6 File Offset: 0x003C6BD6
		public bool HasUnwantedFaceTattoo
		{
			get
			{
				return !PawnStyleItemChooser.WantsToUseStyle(this.pawn, this.faceTattoo, new TattooType?(TattooType.Face));
			}
		}

		// Token: 0x17001FA2 RID: 8098
		// (get) Token: 0x0600CE32 RID: 52786 RVA: 0x003C89F2 File Offset: 0x003C6BF2
		public bool HasUnwantedBodyTattoo
		{
			get
			{
				return !PawnStyleItemChooser.WantsToUseStyle(this.pawn, this.bodyTattoo, new TattooType?(TattooType.Body));
			}
		}

		// Token: 0x0600CE33 RID: 52787 RVA: 0x003C8A0E File Offset: 0x003C6C0E
		public Pawn_StyleTracker()
		{
		}

		// Token: 0x0600CE34 RID: 52788 RVA: 0x003C8A21 File Offset: 0x003C6C21
		public Pawn_StyleTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600CE35 RID: 52789 RVA: 0x003C8A3C File Offset: 0x003C6C3C
		public void StyleTrackerTickInterval(int delta)
		{
			if (!this.lookChangeDesired && this.pawn.IsHashIntervalTick(2500, delta) && this.CanDesireLookChange && this.HasAnyUnwantedStyleItem && Rand.MTBEventOccurs(20f, 60000f, 2500f))
			{
				this.RequestLookChange();
			}
		}

		// Token: 0x0600CE36 RID: 52790 RVA: 0x003C8A90 File Offset: 0x003C6C90
		public void RequestLookChange()
		{
			Find.LetterStack.ReceiveLetter("LetterWantLookChange".Translate() + ": " + Find.ActiveLanguageWorker.WithDefiniteArticle(this.pawn.Name.ToStringShort, this.pawn.gender, false, true), "LetterWantLookChangeDesc".Translate(this.pawn.Named("PAWN")), LetterDefOf.NeutralEvent, new LookTargets(this.pawn), null, null, new List<ThingDef>
			{
				ThingDefOf.StylingStation
			}, null, 0, true);
			this.lookChangeDesired = true;
			this.ResetNextStyleChangeAttemptTick();
		}

		// Token: 0x0600CE37 RID: 52791 RVA: 0x003C8B34 File Offset: 0x003C6D34
		public void ResetNextStyleChangeAttemptTick()
		{
			this.nextStyleChangeAttemptTick = Find.TickManager.TicksGame + Pawn_StyleTracker.AutoStyleChangeTicksOffsetRange.RandomInRange;
		}

		// Token: 0x0600CE38 RID: 52792 RVA: 0x003C8B60 File Offset: 0x003C6D60
		public void Notify_StyleItemChanged()
		{
			if (!this.HasAnyUnwantedStyleItem)
			{
				this.lookChangeDesired = false;
			}
			this.nextHairDef = null;
			this.nextBeardDef = null;
			this.nextFaceTattooDef = null;
			this.nextBodyTatooDef = null;
			this.pawn.Drawer.renderer.SetAllGraphicsDirty();
		}

		// Token: 0x0600CE39 RID: 52793 RVA: 0x003C8BAD File Offset: 0x003C6DAD
		public void FinalizeHairColor()
		{
			if (this.nextHairColor != null)
			{
				this.pawn.story.HairColor = this.nextHairColor.Value;
				this.pawn.Drawer.renderer.SetAllGraphicsDirty();
			}
		}

		// Token: 0x0600CE3A RID: 52794 RVA: 0x003C8BEC File Offset: 0x003C6DEC
		public void MakeHairFilth()
		{
			foreach (IntVec3 c in GenRadial.RadialCellsAround(this.pawn.Position, 1f, true))
			{
				if (c.InBounds(this.pawn.Map) && Rand.Value < 0.5f)
				{
					FilthMaker.TryMakeFilth(c, this.pawn.Map, ThingDefOf.Filth_Hair, this.pawn.LabelIndefinite(), Rand.Range(1, 3), FilthSourceFlags.None);
				}
			}
		}

		// Token: 0x0600CE3B RID: 52795 RVA: 0x003C8C8C File Offset: 0x003C6E8C
		public void SetupNextLookChangeData(HairDef hair = null, BeardDef beard = null, TattooDef faceTatoo = null, TattooDef bodyTattoo = null, Color? hairColor = null)
		{
			this.nextHairDef = hair;
			this.nextBeardDef = beard;
			this.nextFaceTattooDef = faceTatoo;
			this.nextBodyTatooDef = bodyTattoo;
			if (hairColor != null)
			{
				if (hairColor.Value == this.pawn.story.HairColor)
				{
					this.nextHairColor = null;
					return;
				}
				this.nextHairColor = new Color?(hairColor.Value);
			}
		}

		// Token: 0x0600CE3C RID: 52796 RVA: 0x003C8CFC File Offset: 0x003C6EFC
		public void SetupTattoos_NoIdeology()
		{
			this.faceTattoo = TattooDefOf.NoTattoo_Face;
			this.bodyTattoo = TattooDefOf.NoTattoo_Body;
		}

		// Token: 0x0600CE3D RID: 52797 RVA: 0x003C8D14 File Offset: 0x003C6F14
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.lookChangeDesired, "lookChangeDesired", false, false);
			Scribe_Values.Look<int>(ref this.nextStyleChangeAttemptTick, "nextStyleChangeAttemptTick", -99999, false);
			Scribe_Values.Look<Color?>(ref this.nextHairColor, "nextHairColor", null, false);
			Scribe_Defs.Look<BeardDef>(ref this.beardDef, "beardDef");
			Scribe_Defs.Look<TattooDef>(ref this.faceTattoo, "faceTattoo");
			Scribe_Defs.Look<TattooDef>(ref this.bodyTattoo, "bodyTattoo");
			Scribe_Defs.Look<HairDef>(ref this.nextHairDef, "nextHairDef");
			Scribe_Defs.Look<BeardDef>(ref this.nextBeardDef, "nextBeardDef");
			Scribe_Defs.Look<TattooDef>(ref this.nextFaceTattooDef, "nextFaceTattooDef");
			Scribe_Defs.Look<TattooDef>(ref this.nextBodyTatooDef, "nextBodyTattooDef");
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.beardDef == null)
				{
					this.beardDef = BeardDefOf.NoBeard;
				}
				if (!this.CanWantBeard && this.beardDef != BeardDefOf.NoBeard)
				{
					this.beardDef = BeardDefOf.NoBeard;
					Log.Error(this.pawn.LabelShort + " had a beard. Removed.");
				}
				if (this.faceTattoo == null)
				{
					this.faceTattoo = TattooDefOf.NoTattoo_Face;
				}
				if (this.bodyTattoo == null)
				{
					this.bodyTattoo = TattooDefOf.NoTattoo_Body;
				}
			}
		}

		// Token: 0x04008C4D RID: 35917
		public Pawn pawn;

		// Token: 0x04008C4E RID: 35918
		public BeardDef beardDef;

		// Token: 0x04008C4F RID: 35919
		public int nextStyleChangeAttemptTick = -99999;

		// Token: 0x04008C50 RID: 35920
		private TattooDef faceTattoo;

		// Token: 0x04008C51 RID: 35921
		private TattooDef bodyTattoo;

		// Token: 0x04008C52 RID: 35922
		private bool lookChangeDesired;

		// Token: 0x04008C53 RID: 35923
		public HairDef nextHairDef;

		// Token: 0x04008C54 RID: 35924
		public BeardDef nextBeardDef;

		// Token: 0x04008C55 RID: 35925
		public TattooDef nextFaceTattooDef;

		// Token: 0x04008C56 RID: 35926
		public TattooDef nextBodyTatooDef;

		// Token: 0x04008C57 RID: 35927
		public Color? nextHairColor;

		// Token: 0x04008C58 RID: 35928
		private static readonly IntRange AutoStyleChangeTicksOffsetRange = new IntRange(15000, 30000);

		// Token: 0x04008C59 RID: 35929
		private const float StyleChangeMTBDays = 20f;

		// Token: 0x04008C5A RID: 35930
		private const int LookChangeCheckInterval = 2500;
	}
}
