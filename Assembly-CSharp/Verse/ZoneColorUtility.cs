using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse;

public static class ZoneColorUtility
{
	[CompilerGenerated]
	private sealed class _003CFishingZoneColors_003Ed__13 : IEnumerable<Color>, IEnumerable, IEnumerator<Color>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Color _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		Color IEnumerator<Color>.Current
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
		public _003CFishingZoneColors_003Ed__13(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = Color.Lerp(new Color(1f, 0.8f, 0f), Color.gray, 0.25f);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Color.Lerp(new Color(1f, 41f / 85f, 0f), Color.gray, 0.25f);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Color.Lerp(new Color(1f, 0.36862746f, 0f), Color.gray, 0.25f);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Color.Lerp(new Color(1f, 0.96862745f, 0f), Color.gray, 0.25f);
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = Color.Lerp(new Color(1f, 0.14901961f, 0f), Color.gray, 0.25f);
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				return false;
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
		IEnumerator<Color> IEnumerable<Color>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CFishingZoneColors_003Ed__13(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Color>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGrowingZoneColors_003Ed__11 : IEnumerable<Color>, IEnumerable, IEnumerator<Color>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Color _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		Color IEnumerator<Color>.Current
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
		public _003CGrowingZoneColors_003Ed__11(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = Color.Lerp(new Color(0f, 1f, 0f), Color.gray, 0.5f);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Color.Lerp(new Color(1f, 1f, 0f), Color.gray, 0.5f);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Color.Lerp(new Color(0.5f, 1f, 0f), Color.gray, 0.5f);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Color.Lerp(new Color(1f, 1f, 0.5f), Color.gray, 0.5f);
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = Color.Lerp(new Color(0.5f, 1f, 0.5f), Color.gray, 0.5f);
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				return false;
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
		IEnumerator<Color> IEnumerable<Color>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CGrowingZoneColors_003Ed__11(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Color>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CStorageZoneColors_003Ed__12 : IEnumerable<Color>, IEnumerable, IEnumerator<Color>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Color _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		Color IEnumerator<Color>.Current
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
		public _003CStorageZoneColors_003Ed__12(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = Color.Lerp(new Color(1f, 0f, 0f), Color.gray, 0.5f);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Color.Lerp(new Color(1f, 0f, 1f), Color.gray, 0.5f);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Color.Lerp(new Color(0f, 0f, 1f), Color.gray, 0.5f);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Color.Lerp(new Color(1f, 0f, 0.5f), Color.gray, 0.5f);
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = Color.Lerp(new Color(0f, 0.5f, 1f), Color.gray, 0.5f);
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = Color.Lerp(new Color(0.5f, 0f, 1f), Color.gray, 0.5f);
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				return false;
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
		IEnumerator<Color> IEnumerable<Color>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CStorageZoneColors_003Ed__12(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Color>)this).GetEnumerator();
		}
	}

	private static List<Color> growingZoneColors;

	private static List<Color> storageZoneColors;

	private static List<Color> fishingZoneColors;

	private static int nextGrowingZoneColorIndex;

	private static int nextStorageZoneColorIndex;

	private static int nextFishingZoneColorIndex;

	private const float ZoneOpacity = 0.09f;

	static ZoneColorUtility()
	{
		growingZoneColors = new List<Color>();
		storageZoneColors = new List<Color>();
		fishingZoneColors = new List<Color>();
		nextGrowingZoneColorIndex = 0;
		nextStorageZoneColorIndex = 0;
		nextFishingZoneColorIndex = 0;
		foreach (Color item4 in GrowingZoneColors())
		{
			Color item = new Color(item4.r, item4.g, item4.b, 0.09f);
			growingZoneColors.Add(item);
		}
		foreach (Color item5 in StorageZoneColors())
		{
			Color item2 = new Color(item5.r, item5.g, item5.b, 0.09f);
			storageZoneColors.Add(item2);
		}
		foreach (Color item6 in FishingZoneColors())
		{
			Color item3 = new Color(item6.r, item6.g, item6.b, 0.09f);
			fishingZoneColors.Add(item3);
		}
	}

	public static Color NextGrowingZoneColor()
	{
		Color result = growingZoneColors[nextGrowingZoneColorIndex];
		nextGrowingZoneColorIndex++;
		if (nextGrowingZoneColorIndex >= growingZoneColors.Count)
		{
			nextGrowingZoneColorIndex = 0;
		}
		return result;
	}

	public static Color NextStorageZoneColor()
	{
		Color result = storageZoneColors[nextStorageZoneColorIndex];
		nextStorageZoneColorIndex++;
		if (nextStorageZoneColorIndex >= storageZoneColors.Count)
		{
			nextStorageZoneColorIndex = 0;
		}
		return result;
	}

	public static Color NextFishingZoneColor()
	{
		Color result = fishingZoneColors[nextFishingZoneColorIndex];
		nextFishingZoneColorIndex++;
		if (nextFishingZoneColorIndex >= fishingZoneColors.Count)
		{
			nextFishingZoneColorIndex = 0;
		}
		return result;
	}

	[IteratorStateMachine(typeof(_003CGrowingZoneColors_003Ed__11))]
	public static IEnumerable<Color> GrowingZoneColors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGrowingZoneColors_003Ed__11(-2);
	}

	[IteratorStateMachine(typeof(_003CStorageZoneColors_003Ed__12))]
	public static IEnumerable<Color> StorageZoneColors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CStorageZoneColors_003Ed__12(-2);
	}

	[IteratorStateMachine(typeof(_003CFishingZoneColors_003Ed__13))]
	public static IEnumerable<Color> FishingZoneColors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CFishingZoneColors_003Ed__13(-2);
	}
}
