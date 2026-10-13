using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C2A RID: 11306
	public class CompBladelinkWeapon : CompBiocodable
	{
		// Token: 0x170027DE RID: 10206
		// (get) Token: 0x0600FD6E RID: 64878 RVA: 0x004984FC File Offset: 0x004966FC
		public List<WeaponTraitDef> TraitsListForReading
		{
			get
			{
				return this.traits;
			}
		}

		// Token: 0x170027DF RID: 10207
		// (get) Token: 0x0600FD6F RID: 64879 RVA: 0x00498504 File Offset: 0x00496704
		public int TicksSinceLastKill
		{
			get
			{
				if (this.lastKillTick < 0)
				{
					return 0;
				}
				return Find.TickManager.TicksAbs - this.lastKillTick;
			}
		}

		// Token: 0x170027E0 RID: 10208
		// (get) Token: 0x0600FD70 RID: 64880 RVA: 0x00498524 File Offset: 0x00496724
		public override bool Biocodable
		{
			get
			{
				if (!this.traits.NullOrEmpty<WeaponTraitDef>())
				{
					for (int i = 0; i < this.traits.Count; i++)
					{
						if (this.traits[i].neverBond)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x0600FD71 RID: 64881 RVA: 0x0049856A File Offset: 0x0049676A
		public override void PostPostMake()
		{
			this.InitializeTraits();
		}

		// Token: 0x0600FD72 RID: 64882 RVA: 0x00498572 File Offset: 0x00496772
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			this.UnCode();
		}

		// Token: 0x0600FD73 RID: 64883 RVA: 0x00498572 File Offset: 0x00496772
		public override void Notify_MapRemoved()
		{
			this.UnCode();
		}

		// Token: 0x0600FD74 RID: 64884 RVA: 0x0049857C File Offset: 0x0049677C
		private void InitializeTraits()
		{
			IEnumerable<WeaponTraitDef> allDefs = DefDatabase<WeaponTraitDef>.AllDefs;
			if (this.traits == null)
			{
				this.traits = new List<WeaponTraitDef>();
			}
			Map mapBeingGenerated = MapGenerator.mapBeingGenerated;
			using (new RandBlock((mapBeingGenerated != null) ? mapBeingGenerated.NextGenSeed : this.parent.HashOffset()))
			{
				int randomInRange = CompBladelinkWeapon.TraitsRange.RandomInRange;
				for (int i = 0; i < randomInRange; i++)
				{
					IEnumerable<WeaponTraitDef> source = allDefs.Where(new Func<WeaponTraitDef, bool>(this.CanAddTrait));
					if (source.Any<WeaponTraitDef>())
					{
						this.traits.Add(source.RandomElementByWeight((WeaponTraitDef x) => x.commonality));
					}
				}
			}
		}

		// Token: 0x0600FD75 RID: 64885 RVA: 0x00498650 File Offset: 0x00496850
		private bool CanAddTrait(WeaponTraitDef trait)
		{
			if (trait.weaponCategory != WeaponCategoryDefOf.BladeLink)
			{
				return false;
			}
			if (!this.traits.NullOrEmpty<WeaponTraitDef>())
			{
				for (int i = 0; i < this.traits.Count; i++)
				{
					if (trait.Overlaps(this.traits[i]))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600FD76 RID: 64886 RVA: 0x004986A8 File Offset: 0x004968A8
		public override void Notify_Equipped(Pawn pawn)
		{
			if (!ModLister.CheckRoyalty("Persona weapon"))
			{
				return;
			}
			base.Notify_Equipped(pawn);
			if (!this.traits.NullOrEmpty<WeaponTraitDef>())
			{
				for (int i = 0; i < this.traits.Count; i++)
				{
					this.traits[i].Worker.Notify_Equipped(pawn);
				}
			}
		}

		// Token: 0x0600FD77 RID: 64887 RVA: 0x00498704 File Offset: 0x00496904
		public override void CodeFor(Pawn pawn)
		{
			if (!base.Biocodable)
			{
				return;
			}
			if (pawn.IsColonistPlayerControlled && base.CodedPawn == null)
			{
				Find.LetterStack.ReceiveLetter("LetterBladelinkWeaponBondedLabel".Translate(pawn.Named("PAWN"), this.parent.Named("WEAPON")), "LetterBladelinkWeaponBonded".Translate(pawn.Named("PAWN"), this.parent.Named("WEAPON")), LetterDefOf.PositiveEvent, new LookTargets(pawn), null, null, null, null, 0, true);
			}
			base.CodeFor(pawn);
		}

		// Token: 0x0600FD78 RID: 64888 RVA: 0x00498798 File Offset: 0x00496998
		protected override void OnCodedFor(Pawn pawn)
		{
			this.lastKillTick = GenTicks.TicksAbs;
			pawn.equipment.bondedWeapon = this.parent;
			if (!this.traits.NullOrEmpty<WeaponTraitDef>())
			{
				for (int i = 0; i < this.traits.Count; i++)
				{
					this.traits[i].Worker.Notify_Bonded(pawn);
				}
			}
		}

		// Token: 0x0600FD79 RID: 64889 RVA: 0x004987FC File Offset: 0x004969FC
		public override void Notify_KilledPawn(Pawn pawn)
		{
			this.lastKillTick = Find.TickManager.TicksAbs;
			if (!this.traits.NullOrEmpty<WeaponTraitDef>())
			{
				for (int i = 0; i < this.traits.Count; i++)
				{
					this.traits[i].Worker.Notify_KilledPawn(pawn);
				}
			}
		}

		// Token: 0x0600FD7A RID: 64890 RVA: 0x00498854 File Offset: 0x00496A54
		public void Notify_EquipmentLost(Pawn pawn)
		{
			if (!this.traits.NullOrEmpty<WeaponTraitDef>())
			{
				for (int i = 0; i < this.traits.Count; i++)
				{
					this.traits[i].Worker.Notify_EquipmentLost(pawn);
				}
			}
		}

		// Token: 0x0600FD7B RID: 64891 RVA: 0x0049889C File Offset: 0x00496A9C
		public void Notify_WieldedOtherWeapon()
		{
			if (!this.traits.NullOrEmpty<WeaponTraitDef>())
			{
				for (int i = 0; i < this.traits.Count; i++)
				{
					this.traits[i].Worker.Notify_OtherWeaponWielded(this);
				}
			}
		}

		// Token: 0x0600FD7C RID: 64892 RVA: 0x004988E4 File Offset: 0x00496AE4
		public override void UnCode()
		{
			if (base.CodedPawn != null)
			{
				base.CodedPawn.equipment.bondedWeapon = null;
				if (!this.traits.NullOrEmpty<WeaponTraitDef>())
				{
					for (int i = 0; i < this.traits.Count; i++)
					{
						this.traits[i].Worker.Notify_Unbonded(base.CodedPawn);
					}
				}
			}
			base.UnCode();
			this.lastKillTick = -1;
		}

		// Token: 0x0600FD7D RID: 64893 RVA: 0x00498958 File Offset: 0x00496B58
		public override string CompInspectStringExtra()
		{
			string text = "";
			if (!this.traits.NullOrEmpty<WeaponTraitDef>())
			{
				text += "Stat_Thing_PersonaWeaponTrait_Label".Translate() + ": " + (from x in this.traits
				select x.label).ToCommaList(false, false).CapitalizeFirst();
			}
			if (this.Biocodable)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				if (base.CodedPawn == null)
				{
					text += "NotBonded".Translate();
				}
				else
				{
					text += "BondedWith".Translate(base.CodedPawnLabel.ApplyTag(TagType.Name, null)).Resolve();
				}
			}
			return text;
		}

		// Token: 0x0600FD7E RID: 64894 RVA: 0x00498A3C File Offset: 0x00496C3C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.lastKillTick, "lastKillTick", -1, false);
			Scribe_Collections.Look<WeaponTraitDef>(ref this.traits, "traits", LookMode.Def, Array.Empty<object>());
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				Scribe_Values.Look<bool>(ref this.oldBonded, "bonded", false, false);
				Scribe_Values.Look<string>(ref this.oldBondedPawnLabel, "bondedPawnLabel", null, false);
				Scribe_References.Look<Pawn>(ref this.oldBondedPawn, "bondedPawn", true);
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.oldBonded)
				{
					this.CodeFor(this.oldBondedPawn);
				}
				if (this.traits == null)
				{
					this.traits = new List<WeaponTraitDef>();
				}
				if (this.oldBondedPawn != null)
				{
					if (string.IsNullOrEmpty(this.oldBondedPawnLabel) || !this.oldBonded)
					{
						this.codedPawnLabel = this.oldBondedPawn.Name.ToStringFull;
						this.biocoded = true;
					}
					if (this.oldBondedPawn.equipment.bondedWeapon == null)
					{
						this.oldBondedPawn.equipment.bondedWeapon = this.parent;
						return;
					}
					if (this.oldBondedPawn.equipment.bondedWeapon != this.parent)
					{
						this.UnCode();
					}
				}
			}
		}

		// Token: 0x0600FD7F RID: 64895 RVA: 0x00009C2B File Offset: 0x00007E2B
		public override string TransformLabel(string label)
		{
			return label;
		}

		// Token: 0x0600FD80 RID: 64896 RVA: 0x00498B67 File Offset: 0x00496D67
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				RoyalTitleDef minTitleToUse = ThingRequiringRoyalPermissionUtility.GetMinTitleToUse(this.parent.def, faction, 0);
				if (minTitleToUse != null)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawnImportant, "Stat_Thing_MinimumRoyalTitle_Name".Translate(faction.Named("FACTION")).Resolve(), minTitleToUse.GetLabelCapForBothGenders(), "Stat_Thing_Weapon_MinimumRoyalTitle_Desc".Translate(faction.Named("FACTION")).Resolve(), 2100, null, null, false, false);
				}
			}
			IEnumerator<Faction> enumerator = null;
			if (!this.traits.NullOrEmpty<WeaponTraitDef>())
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Stat_Thing_PersonaWeaponTrait_Desc".Translate());
				stringBuilder.AppendLine();
				for (int i = 0; i < this.traits.Count; i++)
				{
					stringBuilder.AppendLine(this.traits[i].LabelCap + ": " + this.traits[i].description);
					if (i < this.traits.Count - 1)
					{
						stringBuilder.AppendLine();
					}
				}
				yield return new StatDrawEntry(this.parent.def.IsMeleeWeapon ? StatCategoryDefOf.Weapon_Melee : StatCategoryDefOf.Weapon_Ranged, "Stat_Thing_PersonaWeaponTrait_Label".Translate(), (from x in this.traits
				select x.label).ToCommaList(false, false).CapitalizeFirst(), stringBuilder.ToString(), 1104, null, null, false, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x0400A616 RID: 42518
		private int lastKillTick = -1;

		// Token: 0x0400A617 RID: 42519
		private List<WeaponTraitDef> traits = new List<WeaponTraitDef>();

		// Token: 0x0400A618 RID: 42520
		private static readonly IntRange TraitsRange = new IntRange(1, 2);

		// Token: 0x0400A619 RID: 42521
		private bool oldBonded;

		// Token: 0x0400A61A RID: 42522
		private string oldBondedPawnLabel;

		// Token: 0x0400A61B RID: 42523
		private Pawn oldBondedPawn;
	}
}
