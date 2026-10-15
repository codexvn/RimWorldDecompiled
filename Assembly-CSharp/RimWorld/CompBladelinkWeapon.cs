using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld;

public class CompBladelinkWeapon : CompBiocodable
{
	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__27 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompBladelinkWeapon _003C_003E4__this;

		private IEnumerator<Faction> _003C_003E7__wrap1;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CSpecialDisplayStats_003Ed__27(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				CompBladelinkWeapon compBladelinkWeapon = _003C_003E4__this;
				StringBuilder stringBuilder;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = Find.FactionManager.AllFactions.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00e6;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00e6;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_00e6:
					while (_003C_003E7__wrap1.MoveNext())
					{
						Faction current = _003C_003E7__wrap1.Current;
						RoyalTitleDef minTitleToUse = ThingRequiringRoyalPermissionUtility.GetMinTitleToUse(compBladelinkWeapon.parent.def, current);
						if (minTitleToUse != null)
						{
							_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.BasicsNonPawnImportant, "Stat_Thing_MinimumRoyalTitle_Name".Translate(current.Named("FACTION")).Resolve(), minTitleToUse.GetLabelCapForBothGenders(), "Stat_Thing_Weapon_MinimumRoyalTitle_Desc".Translate(current.Named("FACTION")).Resolve(), 2100);
							_003C_003E1__state = 1;
							return true;
						}
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (compBladelinkWeapon.traits.NullOrEmpty())
					{
						break;
					}
					stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("Stat_Thing_PersonaWeaponTrait_Desc".Translate());
					stringBuilder.AppendLine();
					for (int i = 0; i < compBladelinkWeapon.traits.Count; i++)
					{
						stringBuilder.AppendLine(compBladelinkWeapon.traits[i].LabelCap + ": " + compBladelinkWeapon.traits[i].description);
						if (i < compBladelinkWeapon.traits.Count - 1)
						{
							stringBuilder.AppendLine();
						}
					}
					_003C_003E2__current = new StatDrawEntry(compBladelinkWeapon.parent.def.IsMeleeWeapon ? StatCategoryDefOf.Weapon_Melee : StatCategoryDefOf.Weapon_Ranged, "Stat_Thing_PersonaWeaponTrait_Label".Translate(), compBladelinkWeapon.traits.Select((WeaponTraitDef x) => x.label).ToCommaList().CapitalizeFirst(), stringBuilder.ToString(), 1104);
					_003C_003E1__state = 2;
					return true;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__27 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__27(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	private int lastKillTick = -1;

	private List<WeaponTraitDef> traits = new List<WeaponTraitDef>();

	private static readonly IntRange TraitsRange = new IntRange(1, 2);

	private bool oldBonded;

	private string oldBondedPawnLabel;

	private Pawn oldBondedPawn;

	public List<WeaponTraitDef> TraitsListForReading => traits;

	public int TicksSinceLastKill
	{
		get
		{
			if (lastKillTick < 0)
			{
				return 0;
			}
			return Find.TickManager.TicksAbs - lastKillTick;
		}
	}

	public override bool Biocodable
	{
		get
		{
			if (!traits.NullOrEmpty())
			{
				for (int i = 0; i < traits.Count; i++)
				{
					if (traits[i].neverBond)
					{
						return false;
					}
				}
			}
			return true;
		}
	}

	public override void PostPostMake()
	{
		InitializeTraits();
	}

	public override void PostDestroy(DestroyMode mode, Map previousMap)
	{
		UnCode();
	}

	public override void Notify_MapRemoved()
	{
		UnCode();
	}

	private void InitializeTraits()
	{
		IEnumerable<WeaponTraitDef> allDefs = DefDatabase<WeaponTraitDef>.AllDefs;
		if (traits == null)
		{
			traits = new List<WeaponTraitDef>();
		}
		using (new RandBlock(MapGenerator.mapBeingGenerated?.NextGenSeed ?? parent.HashOffset()))
		{
			int randomInRange = TraitsRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				IEnumerable<WeaponTraitDef> source = allDefs.Where(CanAddTrait);
				if (source.Any())
				{
					traits.Add(source.RandomElementByWeight((WeaponTraitDef x) => x.commonality));
				}
			}
		}
	}

	private bool CanAddTrait(WeaponTraitDef trait)
	{
		if (trait.weaponCategory != WeaponCategoryDefOf.BladeLink)
		{
			return false;
		}
		if (!traits.NullOrEmpty())
		{
			for (int i = 0; i < traits.Count; i++)
			{
				if (trait.Overlaps(traits[i]))
				{
					return false;
				}
			}
		}
		return true;
	}

	public override void Notify_Equipped(Pawn pawn)
	{
		if (!ModLister.CheckRoyalty("Persona weapon"))
		{
			return;
		}
		base.Notify_Equipped(pawn);
		if (!traits.NullOrEmpty())
		{
			for (int i = 0; i < traits.Count; i++)
			{
				traits[i].Worker.Notify_Equipped(pawn);
			}
		}
	}

	public override void CodeFor(Pawn pawn)
	{
		if (base.Biocodable)
		{
			if (pawn.IsColonistPlayerControlled && base.CodedPawn == null)
			{
				Find.LetterStack.ReceiveLetter("LetterBladelinkWeaponBondedLabel".Translate(pawn.Named("PAWN"), parent.Named("WEAPON")), "LetterBladelinkWeaponBonded".Translate(pawn.Named("PAWN"), parent.Named("WEAPON")), LetterDefOf.PositiveEvent, new LookTargets(pawn));
			}
			base.CodeFor(pawn);
		}
	}

	protected override void OnCodedFor(Pawn pawn)
	{
		lastKillTick = GenTicks.TicksAbs;
		pawn.equipment.bondedWeapon = parent;
		if (!traits.NullOrEmpty())
		{
			for (int i = 0; i < traits.Count; i++)
			{
				traits[i].Worker.Notify_Bonded(pawn);
			}
		}
	}

	public override void Notify_KilledPawn(Pawn pawn)
	{
		lastKillTick = Find.TickManager.TicksAbs;
		if (!traits.NullOrEmpty())
		{
			for (int i = 0; i < traits.Count; i++)
			{
				traits[i].Worker.Notify_KilledPawn(pawn);
			}
		}
	}

	public void Notify_EquipmentLost(Pawn pawn)
	{
		if (!traits.NullOrEmpty())
		{
			for (int i = 0; i < traits.Count; i++)
			{
				traits[i].Worker.Notify_EquipmentLost(pawn);
			}
		}
	}

	public void Notify_WieldedOtherWeapon()
	{
		if (!traits.NullOrEmpty())
		{
			for (int i = 0; i < traits.Count; i++)
			{
				traits[i].Worker.Notify_OtherWeaponWielded(this);
			}
		}
	}

	public override void UnCode()
	{
		if (base.CodedPawn != null)
		{
			base.CodedPawn.equipment.bondedWeapon = null;
			if (!traits.NullOrEmpty())
			{
				for (int i = 0; i < traits.Count; i++)
				{
					traits[i].Worker.Notify_Unbonded(base.CodedPawn);
				}
			}
		}
		base.UnCode();
		lastKillTick = -1;
	}

	public override string CompInspectStringExtra()
	{
		string text = "";
		if (!traits.NullOrEmpty())
		{
			text += "Stat_Thing_PersonaWeaponTrait_Label".Translate() + ": " + traits.Select((WeaponTraitDef x) => x.label).ToCommaList().CapitalizeFirst();
		}
		if (Biocodable)
		{
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			text = ((base.CodedPawn != null) ? (text + "BondedWith".Translate(base.CodedPawnLabel.ApplyTag(TagType.Name)).Resolve()) : ((string)(text + "NotBonded".Translate())));
		}
		return text;
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref lastKillTick, "lastKillTick", -1);
		Scribe_Collections.Look(ref traits, "traits", LookMode.Def);
		if (Scribe.mode != LoadSaveMode.Saving)
		{
			Scribe_Values.Look(ref oldBonded, "bonded", defaultValue: false);
			Scribe_Values.Look(ref oldBondedPawnLabel, "bondedPawnLabel");
			Scribe_References.Look(ref oldBondedPawn, "bondedPawn", saveDestroyedThings: true);
		}
		if (Scribe.mode != LoadSaveMode.PostLoadInit)
		{
			return;
		}
		if (oldBonded)
		{
			CodeFor(oldBondedPawn);
		}
		if (traits == null)
		{
			traits = new List<WeaponTraitDef>();
		}
		if (oldBondedPawn != null)
		{
			if (string.IsNullOrEmpty(oldBondedPawnLabel) || !oldBonded)
			{
				codedPawnLabel = oldBondedPawn.Name.ToStringFull;
				biocoded = true;
			}
			if (oldBondedPawn.equipment.bondedWeapon == null)
			{
				oldBondedPawn.equipment.bondedWeapon = parent;
			}
			else if (oldBondedPawn.equipment.bondedWeapon != parent)
			{
				UnCode();
			}
		}
	}

	public override string TransformLabel(string label)
	{
		return label;
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__27))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__27(-2)
		{
			_003C_003E4__this = this
		};
	}
}
