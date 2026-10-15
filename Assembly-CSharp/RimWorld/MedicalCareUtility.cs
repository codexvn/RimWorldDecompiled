using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

[StaticConstructorOnStartup]
public static class MedicalCareUtility
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass10_0
	{
		public Pawn p;
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass10_1
	{
		public MedicalCareCategory mc;

		public _003C_003Ec__DisplayClass10_0 CS_0024_003C_003E8__locals1;

		internal void _003CMedicalCareSelectButton_GenerateMenu_003Eb__0()
		{
			CS_0024_003C_003E8__locals1.p.playerSettings.medCare = mc;
		}
	}

	[CompilerGenerated]
	private sealed class _003CMedicalCareSelectButton_GenerateMenu_003Ed__10 : IEnumerable<Widgets.DropdownMenuElement<MedicalCareCategory>>, IEnumerable, IEnumerator<Widgets.DropdownMenuElement<MedicalCareCategory>>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Widgets.DropdownMenuElement<MedicalCareCategory> _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn p;

		public Pawn _003C_003E3__p;

		private _003C_003Ec__DisplayClass10_0 _003C_003E8__1;

		private int _003Ci_003E5__2;

		Widgets.DropdownMenuElement<MedicalCareCategory> IEnumerator<Widgets.DropdownMenuElement<MedicalCareCategory>>.Current
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
		public _003CMedicalCareSelectButton_GenerateMenu_003Ed__10(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass10_0();
				_003C_003E8__1.p = p;
				_003Ci_003E5__2 = 0;
				goto IL_00eb;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
				goto IL_00eb;
			case 2:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_00eb:
				if (_003Ci_003E5__2 < 5)
				{
					_003C_003Ec__DisplayClass10_1 CS_0024_003C_003E8__locals5 = new _003C_003Ec__DisplayClass10_1
					{
						CS_0024_003C_003E8__locals1 = _003C_003E8__1,
						mc = (MedicalCareCategory)_003Ci_003E5__2
					};
					_003C_003E2__current = new Widgets.DropdownMenuElement<MedicalCareCategory>
					{
						option = new FloatMenuOption(CS_0024_003C_003E8__locals5.mc.GetLabel().CapitalizeFirst(), delegate
						{
							CS_0024_003C_003E8__locals5.CS_0024_003C_003E8__locals1.p.playerSettings.medCare = CS_0024_003C_003E8__locals5.mc;
						}, MedicalCareIcon(CS_0024_003C_003E8__locals5.mc), Color.white),
						payload = CS_0024_003C_003E8__locals5.mc
					};
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E2__current = new Widgets.DropdownMenuElement<MedicalCareCategory>
				{
					option = new FloatMenuOption("ChangeDefaults".Translate(), delegate
					{
						Find.WindowStack.Add(new Dialog_MedicalDefaults());
					})
				};
				_003C_003E1__state = 2;
				return true;
			}
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
		IEnumerator<Widgets.DropdownMenuElement<MedicalCareCategory>> IEnumerable<Widgets.DropdownMenuElement<MedicalCareCategory>>.GetEnumerator()
		{
			_003CMedicalCareSelectButton_GenerateMenu_003Ed__10 _003CMedicalCareSelectButton_GenerateMenu_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CMedicalCareSelectButton_GenerateMenu_003Ed__ = this;
			}
			else
			{
				_003CMedicalCareSelectButton_GenerateMenu_003Ed__ = new _003CMedicalCareSelectButton_GenerateMenu_003Ed__10(0);
			}
			_003CMedicalCareSelectButton_GenerateMenu_003Ed__.p = _003C_003E3__p;
			return _003CMedicalCareSelectButton_GenerateMenu_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Widgets.DropdownMenuElement<MedicalCareCategory>>)this).GetEnumerator();
		}
	}

	private static Texture2D[] careTextures;

	public const float CareSetterHeight = 28f;

	public const float CareSetterWidth = 140f;

	private static bool medicalCarePainting;

	public static void Reset()
	{
		LongEventHandler.ExecuteWhenFinished(delegate
		{
			careTextures = new Texture2D[5];
			careTextures[0] = ContentFinder<Texture2D>.Get("UI/Icons/Medical/NoCare");
			careTextures[1] = ContentFinder<Texture2D>.Get("UI/Icons/Medical/NoMeds");
			careTextures[2] = ThingDefOf.MedicineHerbal.uiIcon;
			careTextures[3] = ThingDefOf.MedicineIndustrial.uiIcon;
			careTextures[4] = ThingDefOf.MedicineUltratech.uiIcon;
		});
	}

	public static void MedicalCareSetter(Rect rect, ref MedicalCareCategory medCare)
	{
		Rect rect2 = new Rect(rect.x, rect.y, rect.width / 5f, rect.height);
		for (int i = 0; i < 5; i++)
		{
			MedicalCareCategory mc = (MedicalCareCategory)i;
			Widgets.DrawHighlightIfMouseover(rect2);
			MouseoverSounds.DoRegion(rect2);
			GUI.DrawTexture(rect2, careTextures[i]);
			Widgets.DraggableResult draggableResult = Widgets.ButtonInvisibleDraggable(rect2);
			if (draggableResult == Widgets.DraggableResult.Dragged)
			{
				medicalCarePainting = true;
			}
			if ((medicalCarePainting && Mouse.IsOver(rect2) && medCare != mc) || draggableResult.AnyPressed())
			{
				medCare = mc;
				SoundDefOf.Tick_High.PlayOneShotOnCamera();
			}
			if (medCare == mc)
			{
				Widgets.DrawBox(rect2, 2);
			}
			if (Mouse.IsOver(rect2))
			{
				TooltipHandler.TipRegion(rect2, () => mc.GetLabel().CapitalizeFirst(), 632165 + i * 17);
			}
			rect2.x += rect2.width;
		}
		if (!Input.GetMouseButton(0))
		{
			medicalCarePainting = false;
		}
	}

	public static string GetLabel(this MedicalCareCategory cat)
	{
		return ("MedicalCareCategory_" + cat).Translate();
	}

	public static bool AllowsMedicine(this MedicalCareCategory cat, ThingDef meds)
	{
		return cat switch
		{
			MedicalCareCategory.NoCare => false, 
			MedicalCareCategory.NoMeds => false, 
			MedicalCareCategory.HerbalOrWorse => meds.GetStatValueAbstract(StatDefOf.MedicalPotency) <= ThingDefOf.MedicineHerbal.GetStatValueAbstract(StatDefOf.MedicalPotency), 
			MedicalCareCategory.NormalOrWorse => meds.GetStatValueAbstract(StatDefOf.MedicalPotency) <= ThingDefOf.MedicineIndustrial.GetStatValueAbstract(StatDefOf.MedicalPotency), 
			MedicalCareCategory.Best => true, 
			_ => throw new InvalidOperationException(), 
		};
	}

	public static void MedicalCareSelectButton(Rect rect, Pawn pawn)
	{
		Widgets.Dropdown(rect, pawn, MedicalCareSelectButton_GetMedicalCare, MedicalCareSelectButton_GenerateMenu, null, careTextures[(uint)pawn.playerSettings.medCare], null, null, null, paintable: true);
	}

	private static MedicalCareCategory MedicalCareSelectButton_GetMedicalCare(Pawn pawn)
	{
		return pawn.playerSettings.medCare;
	}

	[IteratorStateMachine(typeof(_003CMedicalCareSelectButton_GenerateMenu_003Ed__10))]
	private static IEnumerable<Widgets.DropdownMenuElement<MedicalCareCategory>> MedicalCareSelectButton_GenerateMenu(Pawn p)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMedicalCareSelectButton_GenerateMenu_003Ed__10(-2)
		{
			_003C_003E3__p = p
		};
	}

	private static Texture2D MedicalCareIcon(MedicalCareCategory category)
	{
		return category switch
		{
			MedicalCareCategory.NoCare => careTextures[0], 
			MedicalCareCategory.NoMeds => careTextures[1], 
			MedicalCareCategory.HerbalOrWorse => careTextures[2], 
			MedicalCareCategory.NormalOrWorse => careTextures[3], 
			MedicalCareCategory.Best => careTextures[4], 
			_ => null, 
		};
	}
}
