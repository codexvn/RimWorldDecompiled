using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class ExpansionDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__30 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ExpansionDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

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
		public _003CConfigErrors_003Ed__30(int _003C_003E1__state)
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
				ExpansionDef expansionDef = _003C_003E4__this;
				ModMetaData modWithIdentifier;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)expansionDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0071;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0071;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0071:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					modWithIdentifier = ModLister.GetModWithIdentifier(expansionDef.linkedMod);
					if (modWithIdentifier != null && !modWithIdentifier.Official)
					{
						_003C_003E2__current = modWithIdentifier.Name + " - ExpansionDefs are used for official content. For mods, you should define ModMetaData in About.xml.";
						_003C_003E1__state = 2;
						return true;
					}
					break;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__30 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__30(0)
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

	[NoTranslate]
	public string iconPath;

	[NoTranslate]
	public string notOwnedIconPath;

	[NoTranslate]
	public string backgroundPath;

	[NoTranslate]
	public string linkedMod;

	[NoTranslate]
	public string steamUrl;

	[NoTranslate]
	public string siteUrl;

	[NoTranslate]
	public string previewImagesFolderPath;

	public bool isCore;

	public Color primaryColor = Color.white;

	private Texture2D cachedIcon;

	private Texture2D cachedNotOwnedIcon;

	private Texture2D cachedBG;

	private List<Texture2D> cachedPreviewImages;

	public Texture2D IconFromStatus
	{
		get
		{
			if (Status != ExpansionStatus.NotInstalled)
			{
				return Icon;
			}
			return NotOwnedIcon;
		}
	}

	public Texture2D Icon
	{
		get
		{
			if (cachedIcon == null)
			{
				cachedIcon = ContentFinder<Texture2D>.Get(iconPath);
			}
			return cachedIcon;
		}
	}

	private Texture2D NotOwnedIcon
	{
		get
		{
			if (notOwnedIconPath.NullOrEmpty())
			{
				return Icon;
			}
			if (cachedNotOwnedIcon == null)
			{
				cachedNotOwnedIcon = ContentFinder<Texture2D>.Get(notOwnedIconPath);
			}
			return cachedNotOwnedIcon;
		}
	}

	public Texture2D BackgroundImage
	{
		get
		{
			if (cachedBG == null)
			{
				cachedBG = ContentFinder<Texture2D>.Get(backgroundPath);
			}
			return cachedBG;
		}
	}

	public List<Texture2D> PreviewImages
	{
		get
		{
			if (cachedPreviewImages.NullOrEmpty())
			{
				if (previewImagesFolderPath.NullOrEmpty())
				{
					return null;
				}
				cachedPreviewImages = new List<Texture2D>(ContentFinder<Texture2D>.GetAllInFolder(previewImagesFolderPath));
			}
			return cachedPreviewImages;
		}
	}

	public string StoreURL
	{
		get
		{
			if (!steamUrl.NullOrEmpty())
			{
				return steamUrl;
			}
			return siteUrl;
		}
	}

	public ExpansionStatus Status
	{
		get
		{
			if (ModsConfig.IsActive(linkedMod))
			{
				return ExpansionStatus.Active;
			}
			if (ModLister.AllInstalledMods.Any((ModMetaData m) => m.SamePackageId(linkedMod)))
			{
				return ExpansionStatus.Installed;
			}
			return ExpansionStatus.NotInstalled;
		}
	}

	public string StatusDescription => Status switch
	{
		ExpansionStatus.Active => "ContentActive".Translate(), 
		ExpansionStatus.Installed => "ContentInstalledButNotActive".Translate(), 
		_ => "ContentNotInstalled".Translate(), 
	};

	public override void PostLoad()
	{
		base.PostLoad();
		linkedMod = linkedMod.ToLower();
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__30))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__30(-2)
		{
			_003C_003E4__this = this
		};
	}
}
