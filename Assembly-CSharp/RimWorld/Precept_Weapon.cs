using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002675 RID: 9845
	public class Precept_Weapon : Precept
	{
		// Token: 0x17002121 RID: 8481
		// (get) Token: 0x0600D819 RID: 55321 RVA: 0x00400998 File Offset: 0x003FEB98
		public override string TipLabel
		{
			get
			{
				return this.def.issue.LabelCap + ": " + this.def.LabelCap;
			}
		}

		// Token: 0x17002122 RID: 8482
		// (get) Token: 0x0600D81A RID: 55322 RVA: 0x004009CC File Offset: 0x003FEBCC
		public override string UIInfoFirstLine
		{
			get
			{
				return "Noble".Translate().CapitalizeFirst() + ": " + this.noble.LabelCap;
			}
		}

		// Token: 0x17002123 RID: 8483
		// (get) Token: 0x0600D81B RID: 55323 RVA: 0x00400A0C File Offset: 0x003FEC0C
		public override string UIInfoSecondLine
		{
			get
			{
				return "Despised".Translate().CapitalizeFirst() + ": " + this.despised.LabelCap;
			}
		}

		// Token: 0x17002124 RID: 8484
		// (get) Token: 0x0600D81C RID: 55324 RVA: 0x000440FD File Offset: 0x000422FD
		public override Color LabelColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x17002125 RID: 8485
		// (get) Token: 0x0600D81D RID: 55325 RVA: 0x00400A4A File Offset: 0x003FEC4A
		protected HashSet<ThingDef> NobleWeapons
		{
			get
			{
				if (this.nobleWeaponsCached.EnumerableNullOrEmpty<ThingDef>())
				{
					this.nobleWeaponsCached = new HashSet<ThingDef>(from x in DefDatabase<ThingDef>.AllDefs
					where x.IsWeapon && x.PlayerAcquirable && !x.weaponClasses.NullOrEmpty<WeaponClassDef>() && x.weaponClasses.Contains(this.noble)
					select x);
				}
				return this.nobleWeaponsCached;
			}
		}

		// Token: 0x17002126 RID: 8486
		// (get) Token: 0x0600D81E RID: 55326 RVA: 0x00400A80 File Offset: 0x003FEC80
		protected HashSet<ThingDef> DespisedWeapons
		{
			get
			{
				if (this.despisedWeaponsCached.EnumerableNullOrEmpty<ThingDef>())
				{
					this.despisedWeaponsCached = new HashSet<ThingDef>(from x in DefDatabase<ThingDef>.AllDefs
					where x.IsWeapon && !x.weaponClasses.NullOrEmpty<WeaponClassDef>() && x.weaponClasses.Contains(this.despised)
					select x);
				}
				return this.despisedWeaponsCached;
			}
		}

		// Token: 0x0600D81F RID: 55327 RVA: 0x00400AB6 File Offset: 0x003FECB6
		private void RecacheData()
		{
			this.nobleWeaponsCached = null;
			this.despisedWeaponsCached = null;
			this.iconWeapon = null;
			this.ClearTipCache();
		}

		// Token: 0x0600D820 RID: 55328 RVA: 0x00400AD4 File Offset: 0x003FECD4
		public override void DrawIcon(Rect rect)
		{
			if (this.iconWeapon == null)
			{
				Rand.PushState(this.randomSeed);
				this.iconWeapon = (from w in this.NobleWeapons
				where !w.IsStuff
				select w).RandomElementWithFallback(null);
				Rand.PopState();
			}
			if (this.iconWeapon == null)
			{
				Rand.PushState(this.randomSeed);
				this.iconWeapon = (from w in this.DespisedWeapons
				where !w.IsStuff
				select w).RandomElementWithFallback(null);
				Rand.PopState();
			}
			Widgets.DefIcon(rect, this.iconWeapon, null, 1f, this.ideo.GetStyleFor(this.iconWeapon), false, null, null, null, 1f);
		}

		// Token: 0x0600D821 RID: 55329 RVA: 0x00400BBC File Offset: 0x003FEDBC
		public override string GetTip()
		{
			if (this.tipCached.NullOrEmpty())
			{
				string[] array = new string[9];
				array[0] = base.GetTip();
				array[1] = "\n\n";
				array[2] = base.ColorizeDescTitle("Noble".Translate().CapitalizeFirst());
				array[3] = ":\n";
				array[4] = (from x in this.NobleWeapons
				where x.canGenerateDefaultDesignator
				select x.LabelCap.Resolve()).ToCommaList(false, false);
				array[5] = "\n\n";
				array[6] = base.ColorizeDescTitle("Despised".Translate().CapitalizeFirst());
				array[7] = ":\n";
				array[8] = (from x in this.DespisedWeapons
				where x.canGenerateDefaultDesignator
				select x.LabelCap.Resolve()).ToCommaList(false, false);
				this.tipCached = string.Concat(array);
			}
			return this.tipCached;
		}

		// Token: 0x0600D822 RID: 55330 RVA: 0x00400D00 File Offset: 0x003FEF00
		public override IEnumerable<FloatMenuOption> EditFloatMenuOptions()
		{
			yield return new FloatMenuOption("SwapNobleAndDespised".Translate(), delegate()
			{
				Gen.Swap<WeaponClassDef>(ref this.noble, ref this.despised);
				this.RecacheData();
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			yield break;
		}

		// Token: 0x0600D823 RID: 55331 RVA: 0x00400D10 File Offset: 0x003FEF10
		public IdeoWeaponDisposition GetDispositionForWeapon(ThingDef td)
		{
			if (this.NobleWeapons.Contains(td))
			{
				return IdeoWeaponDisposition.Noble;
			}
			if (this.DespisedWeapons.Contains(td))
			{
				return IdeoWeaponDisposition.Despised;
			}
			return IdeoWeaponDisposition.None;
		}

		// Token: 0x0600D824 RID: 55332 RVA: 0x00400D34 File Offset: 0x003FEF34
		public override bool CompatibleWith(Precept other)
		{
			Precept_Weapon wep = other as Precept_Weapon;
			if (wep != null)
			{
				return !this.NobleWeapons.Any((ThingDef x) => wep.DespisedWeapons.Contains(x)) && !this.DespisedWeapons.Any((ThingDef x) => wep.NobleWeapons.Contains(x));
			}
			return base.CompatibleWith(other);
		}

		// Token: 0x0600D825 RID: 55333 RVA: 0x00400D98 File Offset: 0x003FEF98
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<WeaponClassDef>(ref this.noble, "noble");
			Scribe_Defs.Look<WeaponClassDef>(ref this.despised, "despised");
		}

		// Token: 0x0600D826 RID: 55334 RVA: 0x00400DC0 File Offset: 0x003FEFC0
		public override void CopyTo(Precept other)
		{
			base.CopyTo(other);
			Precept_Weapon precept_Weapon = (Precept_Weapon)other;
			precept_Weapon.noble = this.noble;
			precept_Weapon.despised = this.despised;
		}

		// Token: 0x0400927D RID: 37501
		public WeaponClassDef noble;

		// Token: 0x0400927E RID: 37502
		public WeaponClassDef despised;

		// Token: 0x0400927F RID: 37503
		[Unsaved(false)]
		private ThingDef iconWeapon;

		// Token: 0x04009280 RID: 37504
		[Unsaved(false)]
		private HashSet<ThingDef> nobleWeaponsCached;

		// Token: 0x04009281 RID: 37505
		[Unsaved(false)]
		private HashSet<ThingDef> despisedWeaponsCached;
	}
}
