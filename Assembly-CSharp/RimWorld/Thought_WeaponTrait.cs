using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018B1 RID: 6321
	public class Thought_WeaponTrait : Thought_Memory
	{
		// Token: 0x170015E5 RID: 5605
		// (get) Token: 0x06009071 RID: 36977 RVA: 0x002A956B File Offset: 0x002A776B
		public override bool ShouldDiscard
		{
			get
			{
				return base.ShouldDiscard || !this.HasWeapon;
			}
		}

		// Token: 0x170015E6 RID: 5606
		// (get) Token: 0x06009072 RID: 36978 RVA: 0x002A9580 File Offset: 0x002A7780
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.Name.Named("WEAPON")).CapitalizeFirst();
			}
		}

		// Token: 0x170015E7 RID: 5607
		// (get) Token: 0x06009073 RID: 36979 RVA: 0x002A95BC File Offset: 0x002A77BC
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.Name.Named("WEAPON")).CapitalizeFirst();
			}
		}

		// Token: 0x170015E8 RID: 5608
		// (get) Token: 0x06009074 RID: 36980 RVA: 0x002A95F6 File Offset: 0x002A77F6
		protected bool HasWeapon
		{
			get
			{
				return this.weapon != null && !this.weapon.Destroyed;
			}
		}

		// Token: 0x170015E9 RID: 5609
		// (get) Token: 0x06009075 RID: 36981 RVA: 0x002A9610 File Offset: 0x002A7810
		private string Name
		{
			get
			{
				CompGeneratedNames compGeneratedNames = this.weapon.TryGetComp<CompGeneratedNames>();
				if (compGeneratedNames != null)
				{
					return compGeneratedNames.Name;
				}
				return this.weapon.LabelNoCount;
			}
		}

		// Token: 0x06009076 RID: 36982 RVA: 0x002A963E File Offset: 0x002A783E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<ThingWithComps>(ref this.weapon, "weapon", false);
		}

		// Token: 0x04005C45 RID: 23621
		public ThingWithComps weapon;
	}
}
