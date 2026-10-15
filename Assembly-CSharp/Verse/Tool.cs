using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse;

public class Tool
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__31 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Tool _003C_003E4__this;

		string IEnumerator<string>.Current
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
		public _003CConfigErrors_003Ed__31(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Tool tool = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (tool.id.NullOrEmpty())
				{
					_003C_003E2__current = "tool has null id (power=" + tool.power.ToString("0.##") + ")";
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__31 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__31(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[Unsaved(false)]
	public string id;

	[MustTranslate]
	public string label;

	[MustTranslate]
	public string labelNoLocation;

	[Unsaved(false)]
	[TranslationHandle]
	public string untranslatedLabel;

	public bool labelUsedInLogging = true;

	public List<ToolCapacityDef> capacities = new List<ToolCapacityDef>();

	public float power;

	public float armorPenetration = -1f;

	public float cooldownTime;

	public SurpriseAttackProps surpriseAttack;

	public HediffDef hediff;

	public float chanceFactor = 1f;

	public bool alwaysTreatAsWeapon;

	public List<ExtraDamage> extraMeleeDamages;

	public SoundDef soundMeleeHit;

	public SoundDef soundMeleeMiss;

	public BodyPartGroupDef linkedBodyPartsGroup;

	public bool ensureLinkedBodyPartsGroupAlwaysUsable;

	[Unsaved(false)]
	private string cachedLabelCap;

	public string LabelCap
	{
		get
		{
			if (cachedLabelCap == null)
			{
				cachedLabelCap = label.CapitalizeFirst();
			}
			return cachedLabelCap;
		}
	}

	public IEnumerable<ManeuverDef> Maneuvers => DefDatabase<ManeuverDef>.AllDefsListForReading.Where((ManeuverDef x) => capacities.Contains(x.requiredCapacity));

	public IEnumerable<VerbProperties> VerbsProperties => Maneuvers.Select((ManeuverDef x) => x.verb);

	public float AdjustedBaseMeleeDamageAmount(Thing ownerEquipment, DamageDef damageDef)
	{
		float num = power;
		if (ownerEquipment != null)
		{
			num *= ownerEquipment.GetStatValue(StatDefOf.MeleeWeapon_DamageMultiplier);
			if (ownerEquipment.Stuff != null && damageDef != null)
			{
				num *= ownerEquipment.Stuff.GetStatValueAbstract(damageDef.armorCategory.multStat);
			}
		}
		return num;
	}

	public float AdjustedBaseMeleeDamageAmount(ThingDef ownerEquipment, ThingDef ownerEquipmentStuff, DamageDef damageDef)
	{
		float num = power;
		if (ownerEquipmentStuff != null)
		{
			num *= ownerEquipment.GetStatValueAbstract(StatDefOf.MeleeWeapon_DamageMultiplier, ownerEquipmentStuff);
			if (ownerEquipmentStuff != null && damageDef != null)
			{
				num *= ownerEquipmentStuff.GetStatValueAbstract(damageDef.armorCategory.multStat);
			}
		}
		return num;
	}

	public float AdjustedCooldown(Thing ownerEquipment)
	{
		return cooldownTime * (ownerEquipment?.GetStatValue(StatDefOf.MeleeWeapon_CooldownMultiplier) ?? 1f);
	}

	public float AdjustedCooldown(ThingDef ownerEquipment, ThingDef ownerEquipmentStuff)
	{
		return cooldownTime * (ownerEquipment?.GetStatValueAbstract(StatDefOf.MeleeWeapon_CooldownMultiplier, ownerEquipmentStuff) ?? 1f);
	}

	public override string ToString()
	{
		return label;
	}

	public void PostLoad()
	{
		untranslatedLabel = label;
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__31))]
	public IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__31(-2)
		{
			_003C_003E4__this = this
		};
	}
}
