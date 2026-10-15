using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

[StaticConstructorOnStartup]
public static class HostilityResponseModeUtility
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass9_0
	{
		public Pawn p;
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass9_1
	{
		public HostilityResponseMode response;

		public _003C_003Ec__DisplayClass9_0 CS_0024_003C_003E8__locals1;

		internal void _003CDrawResponseButton_GenerateMenu_003Eb__0()
		{
			CS_0024_003C_003E8__locals1.p.playerSettings.hostilityResponse = response;
		}
	}

	[CompilerGenerated]
	private sealed class _003CDrawResponseButton_GenerateMenu_003Ed__9 : IEnumerable<Widgets.DropdownMenuElement<HostilityResponseMode>>, IEnumerable, IEnumerator<Widgets.DropdownMenuElement<HostilityResponseMode>>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Widgets.DropdownMenuElement<HostilityResponseMode> _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn p;

		public Pawn _003C_003E3__p;

		private _003C_003Ec__DisplayClass9_0 _003C_003E8__1;

		private IEnumerator _003C_003E7__wrap1;

		Widgets.DropdownMenuElement<HostilityResponseMode> IEnumerator<Widgets.DropdownMenuElement<HostilityResponseMode>>.Current
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
		public _003CDrawResponseButton_GenerateMenu_003Ed__9(int _003C_003E1__state)
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
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass9_0();
					_003C_003E8__1.p = p;
					_003C_003E7__wrap1 = Enum.GetValues(typeof(HostilityResponseMode)).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					_003C_003Ec__DisplayClass9_1 CS_0024_003C_003E8__locals7 = new _003C_003Ec__DisplayClass9_1
					{
						CS_0024_003C_003E8__locals1 = _003C_003E8__1,
						response = (HostilityResponseMode)_003C_003E7__wrap1.Current
					};
					if (CS_0024_003C_003E8__locals7.response != HostilityResponseMode.Attack || !CS_0024_003C_003E8__locals7.CS_0024_003C_003E8__locals1.p.WorkTagIsDisabled(WorkTags.Violent))
					{
						_003C_003E2__current = new Widgets.DropdownMenuElement<HostilityResponseMode>
						{
							option = new FloatMenuOption(CS_0024_003C_003E8__locals7.response.GetLabel(), delegate
							{
								CS_0024_003C_003E8__locals7.CS_0024_003C_003E8__locals1.p.playerSettings.hostilityResponse = CS_0024_003C_003E8__locals7.response;
							}, CS_0024_003C_003E8__locals7.response.GetIcon(), Color.white),
							payload = CS_0024_003C_003E8__locals7.response
						};
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
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
			if (_003C_003E7__wrap1 is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Widgets.DropdownMenuElement<HostilityResponseMode>> IEnumerable<Widgets.DropdownMenuElement<HostilityResponseMode>>.GetEnumerator()
		{
			_003CDrawResponseButton_GenerateMenu_003Ed__9 _003CDrawResponseButton_GenerateMenu_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CDrawResponseButton_GenerateMenu_003Ed__ = this;
			}
			else
			{
				_003CDrawResponseButton_GenerateMenu_003Ed__ = new _003CDrawResponseButton_GenerateMenu_003Ed__9(0);
			}
			_003CDrawResponseButton_GenerateMenu_003Ed__.p = _003C_003E3__p;
			return _003CDrawResponseButton_GenerateMenu_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Widgets.DropdownMenuElement<HostilityResponseMode>>)this).GetEnumerator();
		}
	}

	private static readonly Texture2D IgnoreIcon = ContentFinder<Texture2D>.Get("UI/Icons/HostilityResponse/Ignore");

	private static readonly Texture2D AttackIcon = ContentFinder<Texture2D>.Get("UI/Icons/HostilityResponse/Attack");

	private static readonly Texture2D FleeIcon = ContentFinder<Texture2D>.Get("UI/Icons/HostilityResponse/Flee");

	private static readonly Color IconColor = new Color(0.84f, 0.84f, 0.84f);

	public static Texture2D GetIcon(this HostilityResponseMode response)
	{
		return response switch
		{
			HostilityResponseMode.Ignore => IgnoreIcon, 
			HostilityResponseMode.Attack => AttackIcon, 
			HostilityResponseMode.Flee => FleeIcon, 
			_ => BaseContent.BadTex, 
		};
	}

	public static HostilityResponseMode GetNextResponse(Pawn pawn)
	{
		switch (pawn.playerSettings.hostilityResponse)
		{
		case HostilityResponseMode.Ignore:
			if (pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				return HostilityResponseMode.Flee;
			}
			return HostilityResponseMode.Attack;
		case HostilityResponseMode.Attack:
			return HostilityResponseMode.Flee;
		case HostilityResponseMode.Flee:
			return HostilityResponseMode.Ignore;
		default:
			return HostilityResponseMode.Ignore;
		}
	}

	public static string GetLabel(this HostilityResponseMode response)
	{
		return ("HostilityResponseMode_" + response).Translate();
	}

	public static void DrawResponseButton(Rect rect, Pawn pawn, bool paintable)
	{
		Widgets.Dropdown(rect, pawn, IconColor, DrawResponseButton_GetResponse, DrawResponseButton_GenerateMenu, null, pawn.playerSettings.hostilityResponse.GetIcon(), null, null, delegate
		{
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.HostilityResponse, KnowledgeAmount.SpecificInteraction);
		}, paintable, 4f);
		UIHighlighter.HighlightOpportunity(rect, "HostilityResponse");
		if (Mouse.IsOver(rect))
		{
			TooltipHandler.TipRegion(rect, "HostilityReponseTip".Translate() + "\n\n" + "HostilityResponseCurrentMode".Translate() + ": " + pawn.playerSettings.hostilityResponse.GetLabel());
		}
	}

	private static HostilityResponseMode DrawResponseButton_GetResponse(Pawn pawn)
	{
		return pawn.playerSettings.hostilityResponse;
	}

	[IteratorStateMachine(typeof(_003CDrawResponseButton_GenerateMenu_003Ed__9))]
	private static IEnumerable<Widgets.DropdownMenuElement<HostilityResponseMode>> DrawResponseButton_GenerateMenu(Pawn p)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CDrawResponseButton_GenerateMenu_003Ed__9(-2)
		{
			_003C_003E3__p = p
		};
	}
}
