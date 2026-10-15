using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using Brutal.GlfwApi;
using Brutal.Numerics;
using UnityEngine;
using UnityStubs.Internal;
using Waylith.Logging;

namespace RimWorldWaylith;

[SupportedOSPlatform("windows")]
internal sealed class WindowsImeBridge : IDisposable
{
	private delegate nint SubclassProc(nint hwnd, uint msg, nuint wParam, nint lParam, nuint id, nuint refData);

	private struct Point
	{
		public int X;

		public int Y;
	}

	private struct NativeRect
	{
		public int Left;

		public int Top;

		public int Right;

		public int Bottom;
	}

	private struct CandidateForm
	{
		public uint Index;

		public uint Style;

		public Point Position;

		public NativeRect Area;
	}

	private struct CompositionForm
	{
		public uint Style;

		public Point Position;

		public NativeRect Area;
	}

	internal static WindowsImeBridge? Current;

	private const uint WM_NCDESTROY = 130u;

	private const uint WM_KILLFOCUS = 8u;

	private const uint WM_KEYDOWN = 256u;

	private const uint WM_SYSKEYDOWN = 260u;

	private const uint WM_IME_STARTCOMPOSITION = 269u;

	private const uint WM_IME_ENDCOMPOSITION = 270u;

	private const uint WM_IME_COMPOSITION = 271u;

	private const uint WM_IME_SETCONTEXT = 641u;

	private const long ISC_SHOWUICOMPOSITIONWINDOW = 2147483648L;

	private const uint GCS_COMPSTR = 8u;

	private const uint GCS_CURSORPOS = 128u;

	private const uint GCS_RESULTSTR = 2048u;

	private const uint NI_COMPOSITIONSTR = 21u;

	private const uint CPS_CANCEL = 4u;

	private const uint CFS_CANDIDATEPOS = 64u;

	private const uint CFS_FORCE_POSITION = 32u;

	private const nuint SubclassId = 1u;

	private readonly GlfwWindow window;

	private readonly SubclassProc proc;

	private nint hwnd;

	private nint detachedContext;

	private bool detached;

	private bool composing;

	private bool failed;

	private int compositionOwner;

	private KeyCode pendingKey;

	private readonly HashSet<KeyCode> consumedKeys = new HashSet<KeyCode>();

	private Vector2 candidatePos = new Vector2(float.NaN, float.NaN);

	public WindowsImeBridge(GlfwWindow window)
	{
		this.window = window;
		proc = WindowProc;
	}

	public static bool Consumed(KeyCode key)
	{
		return Current?.consumedKeys.Contains(key) ?? false;
	}

	public void BeginFrame()
	{
		if (failed)
		{
			return;
		}
		try
		{
			if (hwnd != 0 || Attach())
			{
				consumedKeys.Clear();
				pendingKey = KeyCode.None;
				if (composing && GUIUtility.keyboardControl != compositionOwner)
				{
					Cancel();
				}
				bool flag = window.IsFocused;
				if (flag)
				{
					flag = ImeComposition.Mode switch
					{
						IMECompositionMode.On => true, 
						IMECompositionMode.Off => false, 
						_ => TextFocus.CapturesText, 
					};
				}
				bool associated = flag;
				SetAssociated(associated);
				if (!detached && ImeComposition.CandidatePos != candidatePos)
				{
					candidatePos = ImeComposition.CandidatePos;
					MoveCandidateWindow(candidatePos);
				}
			}
		}
		catch (Exception e)
		{
			Fail(e);
		}
	}

	private bool Attach()
	{
		nint win32Window = Glfw.PInvoke.GetWin32Window(window);
		if (win32Window == 0)
		{
			return false;
		}
		if (!SetWindowSubclass(win32Window, proc, 1u, 0u))
		{
			throw new InvalidOperationException("SetWindowSubclass failed");
		}
		hwnd = win32Window;
		VerboseLog.Log("WindowsImeBridge: attached to the game window");
		return true;
	}

	private void SetAssociated(bool wanted)
	{
		if (wanted && detached)
		{
			ImmAssociateContext(hwnd, detachedContext);
			detachedContext = 0;
			detached = false;
		}
		else if (!wanted && !detached)
		{
			Cancel();
			detachedContext = ImmAssociateContext(hwnd, 0);
			detached = true;
		}
	}

	private void Cancel()
	{
		pendingKey = KeyCode.None;
		if (composing && !detached)
		{
			nint num = ImmGetContext(hwnd);
			if (num != 0)
			{
				try
				{
					ImmNotifyIME(num, 21u, 4u, 0u);
				}
				finally
				{
					ImmReleaseContext(hwnd, num);
				}
			}
		}
		composing = false;
		ImeComposition.Set("", 0);
	}

	private void MoveCandidateWindow(Vector2 framebufferPoint)
	{
		int2 framebufferSize = window.FramebufferSize;
		if (framebufferSize.X <= 0 || framebufferSize.Y <= 0 || !GetClientRect(hwnd, out var rect))
		{
			return;
		}
		Point point = default(Point);
		point.X = (int)MathF.Round(framebufferPoint.x * (float)rect.Right / (float)framebufferSize.X);
		point.Y = (int)MathF.Round(framebufferPoint.y * (float)rect.Bottom / (float)framebufferSize.Y);
		Point position = point;
		NativeRect nativeRect = default(NativeRect);
		nativeRect.Left = position.X;
		nativeRect.Top = position.Y;
		nativeRect.Right = position.X + 1;
		nativeRect.Bottom = position.Y + 1;
		NativeRect area = nativeRect;
		CandidateForm candidateForm = default(CandidateForm);
		candidateForm.Style = 64u;
		candidateForm.Position = position;
		candidateForm.Area = area;
		CandidateForm form = candidateForm;
		CompositionForm compositionForm = default(CompositionForm);
		compositionForm.Style = 32u;
		compositionForm.Position = position;
		compositionForm.Area = area;
		CompositionForm form2 = compositionForm;
		nint num = ImmGetContext(hwnd);
		if (num == 0)
		{
			return;
		}
		try
		{
			ImmSetCandidateWindow(num, ref form);
			ImmSetCompositionWindow(num, ref form2);
		}
		finally
		{
			ImmReleaseContext(hwnd, num);
		}
	}

	private nint WindowProc(nint hwnd, uint msg, nuint wParam, nint lParam, nuint id, nuint refData)
	{
		try
		{
			switch (msg)
			{
			case 130u:
				Release();
				break;
			case 8u:
				Cancel();
				break;
			case 256u:
			case 260u:
				pendingKey = (composing ? ControlKey((int)wParam) : KeyCode.None);
				break;
			case 641u:
				lParam = (nint)(lParam & -2147483649L);
				break;
			case 269u:
				ConsumePending();
				composing = true;
				compositionOwner = GUIUtility.keyboardControl;
				ImeComposition.Set("", 0);
				break;
			case 271u:
				ConsumePending();
				ReadComposition(lParam);
				break;
			case 270u:
				ConsumePending();
				composing = false;
				ImeComposition.Set("", 0);
				break;
			}
		}
		catch (Exception e)
		{
			Fail(e);
		}
		return DefSubclassProc(hwnd, msg, wParam, lParam);
	}

	private void ReadComposition(long flags)
	{
		if (!composing)
		{
			composing = true;
			compositionOwner = GUIUtility.keyboardControl;
		}
		nint num = ImmGetContext(hwnd);
		if (num == 0)
		{
			return;
		}
		try
		{
			string text = (((flags & 8) != 0L) ? ReadString(num, 8u) : ((flags != 0L && (flags & 0x800) == 0L) ? ImeComposition.Text : ""));
			int val = (((flags & 0x80) != 0L) ? ImmGetCompositionStringW(num, 128u, null, 0u) : ImeComposition.Cursor);
			ImeComposition.Set(text, Math.Max(0, val));
		}
		finally
		{
			ImmReleaseContext(hwnd, num);
		}
	}

	private static string ReadString(nint context, uint index)
	{
		int num = ImmGetCompositionStringW(context, index, null, 0u);
		if (num <= 0)
		{
			return "";
		}
		byte[] array = new byte[num];
		int num2 = ImmGetCompositionStringW(context, index, array, (uint)num);
		if (num2 <= 0)
		{
			return "";
		}
		return Encoding.Unicode.GetString(array, 0, num2);
	}

	private void ConsumePending()
	{
		if (pendingKey != 0)
		{
			consumedKeys.Add(pendingKey);
			if (pendingKey == KeyCode.Return)
			{
				consumedKeys.Add(KeyCode.KeypadEnter);
			}
			pendingKey = KeyCode.None;
		}
	}

	private static KeyCode ControlKey(int vk)
	{
		return vk switch
		{
			8 => KeyCode.Backspace, 
			9 => KeyCode.Tab, 
			13 => KeyCode.Return, 
			27 => KeyCode.Escape, 
			33 => KeyCode.PageUp, 
			34 => KeyCode.PageDown, 
			35 => KeyCode.End, 
			36 => KeyCode.Home, 
			37 => KeyCode.LeftArrow, 
			38 => KeyCode.UpArrow, 
			39 => KeyCode.RightArrow, 
			40 => KeyCode.DownArrow, 
			46 => KeyCode.Delete, 
			_ => KeyCode.None, 
		};
	}

	private void Fail(Exception e)
	{
		Release();
		failed = true;
		Console.Error.WriteLine($"[WindowsImeBridge] IME bridge disabled, text input falls back to committed characters only: {e}");
	}

	private void Release()
	{
		if (hwnd != 0)
		{
			Cancel();
			if (detached)
			{
				ImmAssociateContext(hwnd, detachedContext);
			}
			RemoveWindowSubclass(hwnd, proc, 1u);
			hwnd = (detachedContext = 0);
			detached = false;
			consumedKeys.Clear();
		}
	}

	public void Dispose()
	{
		Release();
		if (Current == this)
		{
			Current = null;
		}
	}

	[DllImport("comctl32.dll")]
	private static extern bool SetWindowSubclass(nint hwnd, SubclassProc proc, nuint id, nuint refData);

	[DllImport("comctl32.dll")]
	private static extern bool RemoveWindowSubclass(nint hwnd, SubclassProc proc, nuint id);

	[DllImport("comctl32.dll")]
	private static extern nint DefSubclassProc(nint hwnd, uint msg, nuint wParam, nint lParam);

	[DllImport("user32.dll")]
	private static extern bool GetClientRect(nint hwnd, out NativeRect rect);

	[DllImport("imm32.dll")]
	private static extern nint ImmGetContext(nint hwnd);

	[DllImport("imm32.dll")]
	private static extern bool ImmReleaseContext(nint hwnd, nint context);

	[DllImport("imm32.dll")]
	private static extern nint ImmAssociateContext(nint hwnd, nint context);

	[DllImport("imm32.dll")]
	private static extern int ImmGetCompositionStringW(nint context, uint index, byte[]? buffer, uint bytes);

	[DllImport("imm32.dll")]
	private static extern bool ImmNotifyIME(nint context, uint action, uint index, uint value);

	[DllImport("imm32.dll")]
	private static extern bool ImmSetCandidateWindow(nint context, ref CandidateForm form);

	[DllImport("imm32.dll")]
	private static extern bool ImmSetCompositionWindow(nint context, ref CompositionForm form);
}
