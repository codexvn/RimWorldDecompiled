using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CreditsAssembler
{
	[CompilerGenerated]
	private sealed class _003C_003CAllCredits_003Eg__Reformat2Cols_007C0_0_003Ed : IEnumerable<CreditsEntry>, IEnumerable, IEnumerator<CreditsEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private CreditsEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private List<CreditsEntry> entries;

		public List<CreditsEntry> _003C_003E3__entries;

		private string _003CcrediteePrev_003E5__2;

		private int _003Ci_003E5__3;

		private CreditsEntry _003ClangCred_003E5__4;

		CreditsEntry IEnumerator<CreditsEntry>.Current
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
		public _003C_003CAllCredits_003Eg__Reformat2Cols_007C0_0_003Ed(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CcrediteePrev_003E5__2 = null;
			_003ClangCred_003E5__4 = null;
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
				_003CcrediteePrev_003E5__2 = null;
				_003Ci_003E5__3 = 0;
				goto IL_011d;
			case 1:
				_003C_003E1__state = -1;
				_003CcrediteePrev_003E5__2 = null;
				goto IL_0106;
			case 2:
				_003C_003E1__state = -1;
				_003CcrediteePrev_003E5__2 = null;
				goto IL_00ea;
			case 3:
				_003C_003E1__state = -1;
				goto IL_0106;
			case 4:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_011d:
				if (_003Ci_003E5__3 < entries.Count)
				{
					_003ClangCred_003E5__4 = entries[_003Ci_003E5__3];
					if (_003ClangCred_003E5__4 is CreditRecord_Role creditRecord_Role)
					{
						if (_003CcrediteePrev_003E5__2 != null)
						{
							_003C_003E2__current = new CreditRecord_RoleTwoCols(_003CcrediteePrev_003E5__2, creditRecord_Role.creditee).Compress();
							_003C_003E1__state = 1;
							return true;
						}
						_003CcrediteePrev_003E5__2 = creditRecord_Role.creditee;
						goto IL_0106;
					}
					if (_003CcrediteePrev_003E5__2 != null)
					{
						_003C_003E2__current = new CreditRecord_RoleTwoCols(_003CcrediteePrev_003E5__2, "").Compress();
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00ea;
				}
				if (_003CcrediteePrev_003E5__2 != null)
				{
					_003C_003E2__current = new CreditRecord_RoleTwoCols(_003CcrediteePrev_003E5__2, "").Compress();
					_003C_003E1__state = 4;
					return true;
				}
				break;
				IL_0106:
				_003ClangCred_003E5__4 = null;
				_003Ci_003E5__3++;
				goto IL_011d;
				IL_00ea:
				_003C_003E2__current = _003ClangCred_003E5__4;
				_003C_003E1__state = 3;
				return true;
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
		IEnumerator<CreditsEntry> IEnumerable<CreditsEntry>.GetEnumerator()
		{
			_003C_003CAllCredits_003Eg__Reformat2Cols_007C0_0_003Ed _003C_003CAllCredits_003Eg__Reformat2Cols_007C0_0_003Ed;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003C_003CAllCredits_003Eg__Reformat2Cols_007C0_0_003Ed = this;
			}
			else
			{
				_003C_003CAllCredits_003Eg__Reformat2Cols_007C0_0_003Ed = new _003C_003CAllCredits_003Eg__Reformat2Cols_007C0_0_003Ed(0);
			}
			_003C_003CAllCredits_003Eg__Reformat2Cols_007C0_0_003Ed.entries = _003C_003E3__entries;
			return _003C_003CAllCredits_003Eg__Reformat2Cols_007C0_0_003Ed;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<CreditsEntry>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CAllCredits_003Ed__0 : IEnumerable<CreditsEntry>, IEnumerable, IEnumerator<CreditsEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private CreditsEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private bool _003CfirstModCredit_003E5__2;

		private HashSet<string> _003CallModders_003E5__3;

		private List<string> _003CtmpModders_003E5__4;

		private IEnumerator<CreditsEntry> _003C_003E7__wrap4;

		private IEnumerator<LoadedLanguage> _003C_003E7__wrap5;

		private LoadedLanguage _003Clang_003E5__7;

		private IEnumerator<ModMetaData> _003C_003E7__wrap7;

		private ModMetaData _003Cmod_003E5__9;

		private List<string>.Enumerator _003C_003E7__wrap9;

		private string _003Cmodder_003E5__11;

		CreditsEntry IEnumerator<CreditsEntry>.Current
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
		public _003CAllCredits_003Ed__0(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			switch (num)
			{
			case -3:
			case 37:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 40:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -5:
			case 43:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			case -6:
			case 46:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally4();
				}
				break;
			case -8:
			case -7:
			case 50:
			case 51:
				try
				{
					if (num == -8 || num == 51)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally6();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally5();
				}
				break;
			case -10:
			case -9:
			case 113:
			case 114:
				try
				{
					if (num == -10 || (uint)(num - 113) <= 1u)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally8();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally7();
				}
				break;
			}
			_003CallModders_003E5__3 = null;
			_003CtmpModders_003E5__4 = null;
			_003C_003E7__wrap4 = null;
			_003C_003E7__wrap5 = null;
			_003Clang_003E5__7 = null;
			_003C_003E7__wrap7 = null;
			_003Cmod_003E5__9 = null;
			_003C_003E7__wrap9 = default(List<string>.Enumerator);
			_003Cmodder_003E5__11 = null;
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
					_003C_003E2__current = new CreditRecord_Space(200f);
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Title("Credits_Design".Translate());
					_003C_003E1__state = 2;
					return true;
				case 2:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("", "Tynan Sylvester");
					_003C_003E1__state = 3;
					return true;
				case 3:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("", "Will Stacey");
					_003C_003E1__state = 4;
					return true;
				case 4:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("", "Thorsten Klotz");
					_003C_003E1__state = 5;
					return true;
				case 5:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Title("Credits_Developers".Translate());
					_003C_003E1__state = 6;
					return true;
				case 6:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("", "Piotr Walczak");
					_003C_003E1__state = 7;
					return true;
				case 7:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("", "Tynan Sylvester");
					_003C_003E1__state = 8;
					return true;
				case 8:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("", "Igor Lebedev");
					_003C_003E1__state = 9;
					return true;
				case 9:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("", "Matt Ritchie");
					_003C_003E1__state = 10;
					return true;
				case 10:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("", "Alex Mulford");
					_003C_003E1__state = 11;
					return true;
				case 11:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("", "Joe Gasparich");
					_003C_003E1__state = 12;
					return true;
				case 12:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("", "Máté Mészáros");
					_003C_003E1__state = 13;
					return true;
				case 13:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("", "Liam Harrison");
					_003C_003E1__state = 14;
					return true;
				case 14:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("", "Kenneth Ellersdorfer");
					_003C_003E1__state = 15;
					return true;
				case 15:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("", "Sam Byass");
					_003C_003E1__state = 16;
					return true;
				case 16:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("", "Ben Rog-Wilhelm");
					_003C_003E1__state = 17;
					return true;
				case 17:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("", "Matt Quail");
					_003C_003E1__state = 18;
					return true;
				case 18:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("", "Mark Kaldas");
					_003C_003E1__state = 19;
					return true;
				case 19:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("", "Nick Barrash");
					_003C_003E1__state = 20;
					return true;
				case 20:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("", "Don Bellenger");
					_003C_003E1__state = 21;
					return true;
				case 21:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("", "Jay Lemmon");
					_003C_003E1__state = 22;
					return true;
				case 22:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Space(50f);
					_003C_003E1__state = 23;
					return true;
				case 23:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Title("Credit_MusicAndSound".Translate());
					_003C_003E1__state = 24;
					return true;
				case 24:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("", "Alistair Lindsay");
					_003C_003E1__state = 25;
					return true;
				case 25:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Space(50f);
					_003C_003E1__state = 26;
					return true;
				case 26:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Title("Credit_GameArt".Translate());
					_003C_003E1__state = 27;
					return true;
				case 27:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("", "Oskar Potocki");
					_003C_003E1__state = 28;
					return true;
				case 28:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("", "Tynan Sylvester");
					_003C_003E1__state = 29;
					return true;
				case 29:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("", "Hayden Duvall");
					_003C_003E1__state = 30;
					return true;
				case 30:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("", "Mehran Iranloo");
					_003C_003E1__state = 31;
					return true;
				case 31:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("", "Ricardo Tomé");
					_003C_003E1__state = 32;
					return true;
				case 32:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("", "Rhopunzel");
					_003C_003E1__state = 33;
					return true;
				case 33:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("", "Tamara Osborn");
					_003C_003E1__state = 34;
					return true;
				case 34:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Space(50f);
					_003C_003E1__state = 35;
					return true;
				case 35:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Title("Credits_AdditionalDevelopment".Translate());
					_003C_003E1__state = 36;
					return true;
				case 36:
					_003C_003E1__state = -1;
					_003C_003E7__wrap4 = _003CAllCredits_003Eg__Reformat2Cols_007C0_0(new List<CreditsEntry>
					{
						new CreditRecord_Role("", "August Trollbäck").Compress(),
						new CreditRecord_Role("", "John Fernandes-Salling").Compress(),
						new CreditRecord_Role("", "Tate Conlon").Compress(),
						new CreditRecord_Role("", "Gavan Woolery").Compress(),
						new CreditRecord_Role("", "David 'Rez' Graham").Compress()
					}).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_08dd;
				case 37:
					_003C_003E1__state = -3;
					goto IL_08dd;
				case 38:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Title("Credit_Testers".Translate());
					_003C_003E1__state = 39;
					return true;
				case 39:
					_003C_003E1__state = -1;
					_003C_003E7__wrap4 = _003CAllCredits_003Eg__Reformat2Cols_007C0_0(new List<CreditsEntry>
					{
						new CreditRecord_Role("", "Fey Nickel"),
						new CreditRecord_Role("", "Morg"),
						new CreditRecord_Role("", "ItchyFlea"),
						new CreditRecord_Role("", "Sneaks"),
						new CreditRecord_Role("", "Elliott"),
						new CreditRecord_Role("", "James Miura")
					}).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0a26;
				case 40:
					_003C_003E1__state = -4;
					goto IL_0a26;
				case 41:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Title("Credit_SpecialThanks".Translate());
					_003C_003E1__state = 42;
					return true;
				case 42:
					_003C_003E1__state = -1;
					_003C_003E7__wrap4 = _003CAllCredits_003Eg__Reformat2Cols_007C0_0(new List<CreditsEntry>
					{
						new CreditRecord_Role("", "Tia Young").Compress(),
						new CreditRecord_Role("", "Kay Fedewa").Compress(),
						new CreditRecord_Role("", "Jon Larson").Compress(),
						new CreditRecord_Role("", "Zhentar").Compress(),
						new CreditRecord_Role("", "Haplo").Compress(),
						new CreditRecord_Role("", "iame6162013").Compress(),
						new CreditRecord_Role("", "Shinzy").Compress(),
						new CreditRecord_Role("", "John Woolley").Compress(),
						new CreditRecord_Role("", "Marta Fijak").Compress(),
						new CreditRecord_Role("", "Simon Warrener").Compress()
					}).GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_0bfd;
				case 43:
					_003C_003E1__state = -5;
					goto IL_0bfd;
				case 44:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Title("Credits_TitleTester".Translate());
					_003C_003E1__state = 45;
					return true;
				case 45:
					_003C_003E1__state = -1;
					_003C_003E7__wrap4 = _003CAllCredits_003Eg__Reformat2Cols_007C0_0(new List<CreditsEntry>
					{
						new CreditRecord_Role("", "Ramsis").Compress(),
						new CreditRecord_Role("", "Haplo").Compress(),
						new CreditRecord_Role("", "DubskiDude").Compress(),
						new CreditRecord_Role("", "Harry Bryant").Compress(),
						new CreditRecord_Role("", "ChJees").Compress(),
						new CreditRecord_Role("", "AWiseCorn").Compress(),
						new CreditRecord_Role("", "Zero747").Compress(),
						new CreditRecord_Role("", "Mehni").Compress(),
						new CreditRecord_Role("", "XeoNovaDan").Compress(),
						new CreditRecord_Role("", "alphaBeta").Compress(),
						new CreditRecord_Role("", "TheDee05").Compress(),
						new CreditRecord_Role("", "Oglis").Compress(),
						new CreditRecord_Role("", "Vas").Compress(),
						new CreditRecord_Role("", "Kiaayo").Compress(),
						new CreditRecord_Role("", "JimmyAgnt007").Compress(),
						new CreditRecord_Role("", "Gouda Quiche").Compress(),
						new CreditRecord_Role("", "Drb89").Compress(),
						new CreditRecord_Role("", "Jimyoda").Compress(),
						new CreditRecord_Role("", "Semmy").Compress(),
						new CreditRecord_Role("", "DianaWinters").Compress(),
						new CreditRecord_Role("", "Goldenpotatoes").Compress(),
						new CreditRecord_Role("", "Skissor").Compress(),
						new CreditRecord_Role("", "Laos").Compress(),
						new CreditRecord_Role("", "Evul").Compress(),
						new CreditRecord_Role("", "SoraHjort").Compress(),
						new CreditRecord_Role("", "Coenmjc").Compress(),
						new CreditRecord_Role("", "Boris(Eqz)").Compress(),
						new CreditRecord_Role("", "MarvinKosh").Compress(),
						new CreditRecord_Role("", "Gaesatae").Compress(),
						new CreditRecord_Role("", "Letharion").Compress(),
						new CreditRecord_Role("", "HeftySmurf").Compress(),
						new CreditRecord_Role("", "Skullywag").Compress(),
						new CreditRecord_Role("", "Jaxxa").Compress(),
						new CreditRecord_Role("", "Helixien").Compress(),
						new CreditRecord_Role("", "DeeGee").Compress(),
						new CreditRecord_Role("", "ReZpawner").Compress(),
						new CreditRecord_Role("", "Doomdrvk").Compress(),
						new CreditRecord_Role("", "tedvs").Compress(),
						new CreditRecord_Role("", "OneSpellPerDay").Compress(),
						new CreditRecord_Role("", "Turbulent Caterwocky").Compress(),
						new CreditRecord_Role("", "RawCode").Compress(),
						new CreditRecord_Role("", "Enystrom8734").Compress(),
						new CreditRecord_Role("", "TeiXeR").Compress(),
						new CreditRecord_Role("", "MortalSmurph").Compress(),
						new CreditRecord_Role("", "AdamVsEverything").Compress(),
						new CreditRecord_Role("", "Fawkes").Compress(),
						new CreditRecord_Role("", "LucyFeonix").Compress(),
						new CreditRecord_Role("", "Erin").Compress(),
						new CreditRecord_Role("", "Ragnar-F").Compress()
					}).GetEnumerator();
					_003C_003E1__state = -6;
					goto IL_1218;
				case 46:
					_003C_003E1__state = -6;
					goto IL_1218;
				case 47:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("", "... " + "AndOtherVolunteers".Translate());
					_003C_003E1__state = 48;
					return true;
				case 48:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Space(200f);
					_003C_003E1__state = 49;
					return true;
				case 49:
					_003C_003E1__state = -1;
					_003C_003E7__wrap5 = LanguageDatabase.AllLoadedLanguages.GetEnumerator();
					_003C_003E1__state = -7;
					goto IL_13d7;
				case 50:
					_003C_003E1__state = -7;
					goto IL_1360;
				case 51:
					_003C_003E1__state = -8;
					goto IL_13b6;
				case 52:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_TitleLocalization("Local Heroes Worldwide B.V.");
					_003C_003E1__state = 53;
					return true;
				case 53:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_LeadLocalizationProjectManager".Translate(), "Iris Kuppen").Compress().WithSmallFont();
					_003C_003E1__state = 54;
					return true;
				case 54:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_LocalizationProjectManager".Translate(), "Maikel Roelofs").Compress().WithSmallFont();
					_003C_003E1__state = 55;
					return true;
				case 55:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_TitleLocalization("Credits_French".Translate() + ": Loc-3 Ltd");
					_003C_003E1__state = 56;
					return true;
				case 56:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_ProjectManager".Translate(), "Eric Emanuel").Compress().WithSmallFont();
					_003C_003E1__state = 57;
					return true;
				case 57:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Translator".Translate(), "Florie Abélard").Compress().WithSmallFont();
					_003C_003E1__state = 58;
					return true;
				case 58:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Translator".Translate(), "Ophélie Colin").Compress().WithSmallFont();
					_003C_003E1__state = 59;
					return true;
				case 59:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Editor".Translate(), "Claire Deiller").Compress().WithSmallFont();
					_003C_003E1__state = 60;
					return true;
				case 60:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_TitleLocalization("Credits_Japanese".Translate() + ": DICO Co., Ltd.");
					_003C_003E1__state = 61;
					return true;
				case 61:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_ProjectManager".Translate(), "Nilgül Durali").Compress().WithSmallFont();
					_003C_003E1__state = 62;
					return true;
				case 62:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_ProjectManager".Translate(), "Ziya Sarper Ekim").Compress().WithSmallFont();
					_003C_003E1__state = 63;
					return true;
				case 63:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Translator".Translate(), "Karien Harimoto").Compress().WithSmallFont();
					_003C_003E1__state = 64;
					return true;
				case 64:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Translator".Translate(), "Keigo Yonemura").Compress().WithSmallFont();
					_003C_003E1__state = 65;
					return true;
				case 65:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Editor".Translate(), "Henry Buckley").Compress().WithSmallFont();
					_003C_003E1__state = 66;
					return true;
				case 66:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_TitleLocalization("Credits_Korean".Translate() + ": DICO Co., Ltd.");
					_003C_003E1__state = 67;
					return true;
				case 67:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_ProjectManager".Translate(), "Nilgül Durali").Compress().WithSmallFont();
					_003C_003E1__state = 68;
					return true;
				case 68:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_ProjectManager".Translate(), "Ziya Sarper Ekim").Compress().WithSmallFont();
					_003C_003E1__state = 69;
					return true;
				case 69:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Translator".Translate(), "Doyeon Jeong").Compress().WithSmallFont();
					_003C_003E1__state = 70;
					return true;
				case 70:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Translator".Translate(), "Junglim Kim").Compress().WithSmallFont();
					_003C_003E1__state = 71;
					return true;
				case 71:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Editor".Translate(), "Lim Yoon").Compress().WithSmallFont();
					_003C_003E1__state = 72;
					return true;
				case 72:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_TitleLocalization("Credits_Polish".Translate() + ": Albion Localisations");
					_003C_003E1__state = 73;
					return true;
				case 73:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Translator".Translate(), "Aleksandra Lubińska").Compress().WithSmallFont();
					_003C_003E1__state = 74;
					return true;
				case 74:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Translator".Translate(), "Łukasz Gładkowski").Compress().WithSmallFont();
					_003C_003E1__state = 75;
					return true;
				case 75:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Editor".Translate(), "Marcin Bojko").Compress().WithSmallFont();
					_003C_003E1__state = 76;
					return true;
				case 76:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_TitleLocalization("Credits_Russian".Translate() + ": Levsha");
					_003C_003E1__state = 77;
					return true;
				case 77:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_ProjectManager".Translate(), "Ekaterina Yamenskova").Compress().WithSmallFont();
					_003C_003E1__state = 78;
					return true;
				case 78:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Translator".Translate(), "Vitaliy Hristyuk").Compress().WithSmallFont();
					_003C_003E1__state = 79;
					return true;
				case 79:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Translator".Translate(), "Polina Karpova").Compress().WithSmallFont();
					_003C_003E1__state = 80;
					return true;
				case 80:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Translator".Translate(), "Artyom Petrov").Compress().WithSmallFont();
					_003C_003E1__state = 81;
					return true;
				case 81:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_TitleLocalization("Credits_ChineseSimplified".Translate() + ": Yeehe");
					_003C_003E1__state = 82;
					return true;
				case 82:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Translator".Translate(), "Jean-Luc Wu").Compress().WithSmallFont();
					_003C_003E1__state = 83;
					return true;
				case 83:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Translator".Translate(), "Roy Liu").Compress().WithSmallFont();
					_003C_003E1__state = 84;
					return true;
				case 84:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Translator".Translate(), "Twig Yu (爽朗的kk23)").Compress().WithSmallFont();
					_003C_003E1__state = 85;
					return true;
				case 85:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Editor".Translate(), "Marcos Wang").Compress().WithSmallFont();
					_003C_003E1__state = 86;
					return true;
				case 86:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Editor".Translate(), "ZISHA").Compress().WithSmallFont();
					_003C_003E1__state = 87;
					return true;
				case 87:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_TitleLocalization("Credits_ChineseTraditional".Translate() + ": Cowbay Entertainment");
					_003C_003E1__state = 88;
					return true;
				case 88:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_ProjectManager".Translate(), "Sean Chen").Compress().WithSmallFont();
					_003C_003E1__state = 89;
					return true;
				case 89:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Translator".Translate(), "Shiou Chen").Compress().WithSmallFont();
					_003C_003E1__state = 90;
					return true;
				case 90:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Translator".Translate(), "Michelle Wu").Compress().WithSmallFont();
					_003C_003E1__state = 91;
					return true;
				case 91:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Translator".Translate(), "Teddy Wu").Compress().WithSmallFont();
					_003C_003E1__state = 92;
					return true;
				case 92:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Editor".Translate(), "Jerry Lee").Compress().WithSmallFont();
					_003C_003E1__state = 93;
					return true;
				case 93:
					_003C_003E1__state = -1;
					goto IL_1e2a;
				case 94:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_TitleLocalization("Local Heroes Worldwide B.V.");
					_003C_003E1__state = 95;
					return true;
				case 95:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_LeadLocalizationProjectManager".Translate(), "Iris Kuppen").Compress().WithSmallFont();
					_003C_003E1__state = 96;
					return true;
				case 96:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_LocalizationProjectManager".Translate(), "Maikel Roelofs").Compress().WithSmallFont();
					_003C_003E1__state = 97;
					return true;
				case 97:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_TitleLocalization("Credits_Dutch".Translate() + ": The Translation Fource");
					_003C_003E1__state = 98;
					return true;
				case 98:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Translator".Translate(), "Sandra Vos").Compress().WithSmallFont();
					_003C_003E1__state = 99;
					return true;
				case 99:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Editor".Translate(), "René van Vemde").Compress().WithSmallFont();
					_003C_003E1__state = 100;
					return true;
				case 100:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Editor".Translate(), "Maikel Roelofs").Compress().WithSmallFont();
					_003C_003E1__state = 101;
					return true;
				case 101:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Editor".Translate(), "Sandra Vos").Compress().WithSmallFont();
					_003C_003E1__state = 102;
					return true;
				case 102:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_TitleLocalization("Credits_Russian".Translate() + ": Levsha");
					_003C_003E1__state = 103;
					return true;
				case 103:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_ProjectManager".Translate(), "Ekaterina Yamenskova").Compress().WithSmallFont();
					_003C_003E1__state = 104;
					return true;
				case 104:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Editor".Translate(), "Nadezhda Lynova").Compress().WithSmallFont();
					_003C_003E1__state = 105;
					return true;
				case 105:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_TitleLocalization("Credits_Turkish".Translate() + ": Locpick");
					_003C_003E1__state = 106;
					return true;
				case 106:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_ProjectManager".Translate(), "Batuhan Öztürk").Compress().WithSmallFont();
					_003C_003E1__state = 107;
					return true;
				case 107:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Editor".Translate(), "Bigem Gözen").Compress().WithSmallFont();
					_003C_003E1__state = 108;
					return true;
				case 108:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_TitleLocalization("Credits_Ukrainian".Translate() + ": Levsha");
					_003C_003E1__state = 109;
					return true;
				case 109:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_ProjectManager".Translate(), "Ekaterina Yamenskova").Compress().WithSmallFont();
					_003C_003E1__state = 110;
					return true;
				case 110:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Translator".Translate(), "Vitalii Khrystiuk").Compress().WithSmallFont();
					_003C_003E1__state = 111;
					return true;
				case 111:
					_003C_003E1__state = -1;
					_003C_003E2__current = new CreditRecord_Role("Credits_Editor".Translate(), "Konstantin Kopin").Compress().WithSmallFont();
					_003C_003E1__state = 112;
					return true;
				case 112:
					_003C_003E1__state = -1;
					goto IL_22b2;
				case 113:
					_003C_003E1__state = -10;
					_003CfirstModCredit_003E5__2 = true;
					break;
				case 114:
					{
						_003C_003E1__state = -10;
						_003Cmodder_003E5__11 = null;
						goto IL_2458;
					}
					IL_13d7:
					if (_003C_003E7__wrap5.MoveNext())
					{
						_003Clang_003E5__7 = _003C_003E7__wrap5.Current;
						_003Clang_003E5__7.LoadMetadata();
						if (_003Clang_003E5__7.info.credits.Count > 0)
						{
							_003C_003E2__current = new CreditRecord_Title("Credits_TitleLanguage".Translate(_003Clang_003E5__7.FriendlyNameEnglish, _003Clang_003E5__7.FriendlyNameNative));
							_003C_003E1__state = 50;
							return true;
						}
						goto IL_1360;
					}
					_003C_003Em__Finally5();
					_003C_003E7__wrap5 = null;
					if (ModLister.AnomalyInstalled)
					{
						_003C_003E2__current = new CreditRecord_TitleLocalization("Credits_Localization_Anomaly".Translate());
						_003C_003E1__state = 52;
						return true;
					}
					goto IL_1e2a;
					IL_1e2a:
					if (ModLister.OdysseyInstalled)
					{
						_003C_003E2__current = new CreditRecord_TitleLocalization("Credits_Localization_Odyssey".Translate());
						_003C_003E1__state = 94;
						return true;
					}
					goto IL_22b2;
					IL_22b2:
					_003CfirstModCredit_003E5__2 = false;
					_003CallModders_003E5__3 = new HashSet<string>();
					_003CtmpModders_003E5__4 = new List<string>();
					_003C_003E7__wrap7 = ModsConfig.ActiveModsInLoadOrder.InRandomOrder().GetEnumerator();
					_003C_003E1__state = -9;
					goto IL_2481;
					IL_2481:
					do
					{
						if (_003C_003E7__wrap7.MoveNext())
						{
							_003Cmod_003E5__9 = _003C_003E7__wrap7.Current;
							continue;
						}
						_003C_003Em__Finally7();
						_003C_003E7__wrap7 = null;
						return false;
					}
					while (_003Cmod_003E5__9.Official);
					_003CtmpModders_003E5__4.Clear();
					_003CtmpModders_003E5__4.AddRange(_003Cmod_003E5__9.Authors);
					for (int num = _003CtmpModders_003E5__4.Count - 1; num >= 0; num--)
					{
						_003CtmpModders_003E5__4[num] = _003CtmpModders_003E5__4[num].Trim();
						if (!_003CallModders_003E5__3.Add(_003CtmpModders_003E5__4[num].ToLowerInvariant()))
						{
							_003CtmpModders_003E5__4.RemoveAt(num);
						}
					}
					if (_003CtmpModders_003E5__4.Count > 0)
					{
						_003C_003E7__wrap9 = _003CtmpModders_003E5__4.GetEnumerator();
						_003C_003E1__state = -10;
						goto IL_2458;
					}
					goto IL_247a;
					IL_2458:
					if (_003C_003E7__wrap9.MoveNext())
					{
						_003Cmodder_003E5__11 = _003C_003E7__wrap9.Current;
						if (!_003CfirstModCredit_003E5__2)
						{
							_003C_003E2__current = new CreditRecord_Title("Credits_TitleMods".Translate());
							_003C_003E1__state = 113;
							return true;
						}
						break;
					}
					_003C_003Em__Finally8();
					_003C_003E7__wrap9 = default(List<string>.Enumerator);
					goto IL_247a;
					IL_247a:
					_003Cmod_003E5__9 = null;
					goto IL_2481;
					IL_1360:
					_003C_003E7__wrap4 = _003CAllCredits_003Eg__Reformat2Cols_007C0_0(_003Clang_003E5__7.info.credits).GetEnumerator();
					_003C_003E1__state = -8;
					goto IL_13b6;
					IL_13b6:
					if (_003C_003E7__wrap4.MoveNext())
					{
						CreditsEntry current = _003C_003E7__wrap4.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 51;
						return true;
					}
					_003C_003Em__Finally6();
					_003C_003E7__wrap4 = null;
					_003Clang_003E5__7 = null;
					goto IL_13d7;
					IL_1218:
					if (_003C_003E7__wrap4.MoveNext())
					{
						CreditsEntry current2 = _003C_003E7__wrap4.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 46;
						return true;
					}
					_003C_003Em__Finally4();
					_003C_003E7__wrap4 = null;
					_003C_003E2__current = new CreditRecord_Space(25f);
					_003C_003E1__state = 47;
					return true;
					IL_0bfd:
					if (_003C_003E7__wrap4.MoveNext())
					{
						CreditsEntry current3 = _003C_003E7__wrap4.Current;
						_003C_003E2__current = current3;
						_003C_003E1__state = 43;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap4 = null;
					_003C_003E2__current = new CreditRecord_Space(50f);
					_003C_003E1__state = 44;
					return true;
					IL_0a26:
					if (_003C_003E7__wrap4.MoveNext())
					{
						CreditsEntry current4 = _003C_003E7__wrap4.Current;
						_003C_003E2__current = current4;
						_003C_003E1__state = 40;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap4 = null;
					_003C_003E2__current = new CreditRecord_Space(50f);
					_003C_003E1__state = 41;
					return true;
					IL_08dd:
					if (_003C_003E7__wrap4.MoveNext())
					{
						CreditsEntry current5 = _003C_003E7__wrap4.Current;
						_003C_003E2__current = current5;
						_003C_003E1__state = 37;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap4 = null;
					_003C_003E2__current = new CreditRecord_Space(50f);
					_003C_003E1__state = 38;
					return true;
				}
				_003C_003E2__current = new CreditRecord_Role(_003Cmod_003E5__9.Name, _003Cmodder_003E5__11).Compress();
				_003C_003E1__state = 114;
				return true;
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
			if (_003C_003E7__wrap4 != null)
			{
				_003C_003E7__wrap4.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap4 != null)
			{
				_003C_003E7__wrap4.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap4 != null)
			{
				_003C_003E7__wrap4.Dispose();
			}
		}

		private void _003C_003Em__Finally4()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap4 != null)
			{
				_003C_003E7__wrap4.Dispose();
			}
		}

		private void _003C_003Em__Finally5()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap5 != null)
			{
				_003C_003E7__wrap5.Dispose();
			}
		}

		private void _003C_003Em__Finally6()
		{
			_003C_003E1__state = -7;
			if (_003C_003E7__wrap4 != null)
			{
				_003C_003E7__wrap4.Dispose();
			}
		}

		private void _003C_003Em__Finally7()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap7 != null)
			{
				_003C_003E7__wrap7.Dispose();
			}
		}

		private void _003C_003Em__Finally8()
		{
			_003C_003E1__state = -9;
			((IDisposable)_003C_003E7__wrap9/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<CreditsEntry> IEnumerable<CreditsEntry>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CAllCredits_003Ed__0(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<CreditsEntry>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CAllCredits_003Ed__0))]
	public static IEnumerable<CreditsEntry> AllCredits()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllCredits_003Ed__0(-2);
	}

	[IteratorStateMachine(typeof(_003C_003CAllCredits_003Eg__Reformat2Cols_007C0_0_003Ed))]
	[CompilerGenerated]
	internal static IEnumerable<CreditsEntry> _003CAllCredits_003Eg__Reformat2Cols_007C0_0(List<CreditsEntry> entries)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003C_003CAllCredits_003Eg__Reformat2Cols_007C0_0_003Ed(-2)
		{
			_003C_003E3__entries = entries
		};
	}
}
