using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class Verb_MeleeAttackDamage : Verb_MeleeAttack
{
	[CompilerGenerated]
	private sealed class _003CDamageInfosToApply_003Ed__2 : IEnumerable<DamageInfo>, IEnumerable, IEnumerator<DamageInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private DamageInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Verb_MeleeAttackDamage _003C_003E4__this;

		private LocalTargetInfo target;

		public LocalTargetInfo _003C_003E3__target;

		private BodyPartGroupDef _003CbodyPartGroupDef_003E5__2;

		private HediffDef _003ChediffDef_003E5__3;

		private ThingDef _003Csource_003E5__4;

		private Vector3 _003Cdirection_003E5__5;

		private List<ExtraDamage>.Enumerator _003C_003E7__wrap5;

		private IEnumerator<ExtraDamage> _003C_003E7__wrap6;

		DamageInfo IEnumerator<DamageInfo>.Current
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
		public _003CDamageInfosToApply_003Ed__2(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003CbodyPartGroupDef_003E5__2 = null;
			_003ChediffDef_003E5__3 = null;
			_003Csource_003E5__4 = null;
			_003C_003E7__wrap5 = default(List<ExtraDamage>.Enumerator);
			_003C_003E7__wrap6 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Verb_MeleeAttackDamage verb_MeleeAttackDamage = _003C_003E4__this;
				IEnumerable<ExtraDamage> enumerable;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					float num2 = verb_MeleeAttackDamage.verbProps.AdjustedMeleeDamageAmount(verb_MeleeAttackDamage, verb_MeleeAttackDamage.CasterPawn);
					float armorPenetration = verb_MeleeAttackDamage.verbProps.AdjustedArmorPenetration(verb_MeleeAttackDamage, verb_MeleeAttackDamage.CasterPawn);
					DamageDef def = verb_MeleeAttackDamage.verbProps.meleeDamageDef;
					_003CbodyPartGroupDef_003E5__2 = null;
					_003ChediffDef_003E5__3 = null;
					QualityCategory qc = QualityCategory.Normal;
					num2 = Rand.Range(num2 * 0.8f, num2 * 1.2f);
					if (verb_MeleeAttackDamage.CasterIsPawn)
					{
						_003CbodyPartGroupDef_003E5__2 = verb_MeleeAttackDamage.verbProps.AdjustedLinkedBodyPartsGroup(verb_MeleeAttackDamage.tool);
						if (num2 >= 1f)
						{
							if (verb_MeleeAttackDamage.HediffCompSource != null)
							{
								_003ChediffDef_003E5__3 = verb_MeleeAttackDamage.HediffCompSource.Def;
							}
						}
						else
						{
							num2 = 1f;
							def = DamageDefOf.Blunt;
						}
					}
					if (verb_MeleeAttackDamage.EquipmentSource != null)
					{
						_003Csource_003E5__4 = verb_MeleeAttackDamage.EquipmentSource.def;
						verb_MeleeAttackDamage.EquipmentSource.TryGetQuality(out qc);
					}
					else
					{
						_003Csource_003E5__4 = verb_MeleeAttackDamage.CasterPawn.def;
					}
					_003Cdirection_003E5__5 = (target.Thing.Position - verb_MeleeAttackDamage.CasterPawn.Position).ToVector3();
					bool instigatorGuilty = !(verb_MeleeAttackDamage.caster is Pawn pawn) || !pawn.Drafted;
					DamageInfo damageInfo = new DamageInfo(def, num2, armorPenetration, -1f, verb_MeleeAttackDamage.caster, null, _003Csource_003E5__4, DamageInfo.SourceCategory.ThingOrUnknown, null, instigatorGuilty);
					damageInfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
					damageInfo.SetWeaponBodyPartGroup(_003CbodyPartGroupDef_003E5__2);
					damageInfo.SetWeaponHediff(_003ChediffDef_003E5__3);
					damageInfo.SetAngle(_003Cdirection_003E5__5);
					damageInfo.SetTool(verb_MeleeAttackDamage.tool);
					damageInfo.SetWeaponQuality(qc);
					_003C_003E2__current = damageInfo;
					_003C_003E1__state = 1;
					return true;
				}
				case 1:
					_003C_003E1__state = -1;
					if (verb_MeleeAttackDamage.tool != null && verb_MeleeAttackDamage.tool.extraMeleeDamages != null)
					{
						_003C_003E7__wrap5 = verb_MeleeAttackDamage.tool.extraMeleeDamages.GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_02e8;
					}
					goto IL_030a;
				case 2:
					_003C_003E1__state = -3;
					goto IL_02e8;
				case 3:
					{
						_003C_003E1__state = -4;
						goto IL_04b5;
					}
					IL_02e8:
					while (_003C_003E7__wrap5.MoveNext())
					{
						ExtraDamage current = _003C_003E7__wrap5.Current;
						if (Rand.Chance(current.chance))
						{
							float num2 = current.amount;
							num2 = Rand.Range(num2 * 0.8f, num2 * 1.2f);
							DamageInfo damageInfo = new DamageInfo(current.def, num2, current.AdjustedArmorPenetration(verb_MeleeAttackDamage, verb_MeleeAttackDamage.CasterPawn), -1f, verb_MeleeAttackDamage.caster, null, _003Csource_003E5__4);
							damageInfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
							damageInfo.SetWeaponBodyPartGroup(_003CbodyPartGroupDef_003E5__2);
							damageInfo.SetWeaponHediff(_003ChediffDef_003E5__3);
							damageInfo.SetAngle(_003Cdirection_003E5__5);
							_003C_003E2__current = damageInfo;
							_003C_003E1__state = 2;
							return true;
						}
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap5 = default(List<ExtraDamage>.Enumerator);
					goto IL_030a;
					IL_04b5:
					if (_003C_003E7__wrap6.MoveNext())
					{
						ExtraDamage current2 = _003C_003E7__wrap6.Current;
						int num3 = GenMath.RoundRandom(current2.AdjustedDamageAmount(verb_MeleeAttackDamage, verb_MeleeAttackDamage.CasterPawn));
						float armorPenetration2 = current2.AdjustedArmorPenetration(verb_MeleeAttackDamage, verb_MeleeAttackDamage.CasterPawn);
						DamageInfo damageInfo2 = new DamageInfo(current2.def, num3, armorPenetration2, -1f, verb_MeleeAttackDamage.caster, null, _003Csource_003E5__4);
						damageInfo2.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
						damageInfo2.SetWeaponBodyPartGroup(_003CbodyPartGroupDef_003E5__2);
						damageInfo2.SetWeaponHediff(_003ChediffDef_003E5__3);
						damageInfo2.SetAngle(_003Cdirection_003E5__5);
						_003C_003E2__current = damageInfo2;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap6 = null;
					break;
					IL_030a:
					if (!verb_MeleeAttackDamage.surpriseAttack || ((verb_MeleeAttackDamage.verbProps.surpriseAttack == null || verb_MeleeAttackDamage.verbProps.surpriseAttack.extraMeleeDamages.NullOrEmpty()) && (verb_MeleeAttackDamage.tool == null || verb_MeleeAttackDamage.tool.surpriseAttack == null || verb_MeleeAttackDamage.tool.surpriseAttack.extraMeleeDamages.NullOrEmpty())))
					{
						break;
					}
					enumerable = Enumerable.Empty<ExtraDamage>();
					if (verb_MeleeAttackDamage.verbProps.surpriseAttack != null && verb_MeleeAttackDamage.verbProps.surpriseAttack.extraMeleeDamages != null)
					{
						enumerable = enumerable.Concat(verb_MeleeAttackDamage.verbProps.surpriseAttack.extraMeleeDamages);
					}
					if (verb_MeleeAttackDamage.tool != null && verb_MeleeAttackDamage.tool.surpriseAttack != null && !verb_MeleeAttackDamage.tool.surpriseAttack.extraMeleeDamages.NullOrEmpty())
					{
						enumerable = enumerable.Concat(verb_MeleeAttackDamage.tool.surpriseAttack.extraMeleeDamages);
					}
					_003C_003E7__wrap6 = enumerable.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_04b5;
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
			((IDisposable)_003C_003E7__wrap5/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap6 != null)
			{
				_003C_003E7__wrap6.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<DamageInfo> IEnumerable<DamageInfo>.GetEnumerator()
		{
			_003CDamageInfosToApply_003Ed__2 _003CDamageInfosToApply_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CDamageInfosToApply_003Ed__ = this;
			}
			else
			{
				_003CDamageInfosToApply_003Ed__ = new _003CDamageInfosToApply_003Ed__2(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CDamageInfosToApply_003Ed__.target = _003C_003E3__target;
			return _003CDamageInfosToApply_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<DamageInfo>)this).GetEnumerator();
		}
	}

	private const float MeleeDamageRandomFactorMin = 0.8f;

	private const float MeleeDamageRandomFactorMax = 1.2f;

	[IteratorStateMachine(typeof(_003CDamageInfosToApply_003Ed__2))]
	private IEnumerable<DamageInfo> DamageInfosToApply(LocalTargetInfo target)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CDamageInfosToApply_003Ed__2(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__target = target
		};
	}

	protected override DamageWorker.DamageResult ApplyMeleeDamageToTarget(LocalTargetInfo target)
	{
		DamageWorker.DamageResult result = new DamageWorker.DamageResult();
		foreach (DamageInfo item in DamageInfosToApply(target))
		{
			if (target.ThingDestroyed)
			{
				break;
			}
			result = target.Thing.TakeDamage(item);
		}
		return result;
	}
}
