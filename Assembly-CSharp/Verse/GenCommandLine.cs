using System;
using System.Diagnostics;
using System.Threading;

namespace Verse
{
	// Token: 0x02000CEF RID: 3311
	public static class GenCommandLine
	{
		// Token: 0x06005483 RID: 21635 RVA: 0x001B3610 File Offset: 0x001B1810
		public static bool CommandLineArgPassed(string key)
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				if (string.Compare(commandLineArgs[i], key, true) == 0 || string.Compare(commandLineArgs[i], "-" + key, true) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005484 RID: 21636 RVA: 0x001B3658 File Offset: 0x001B1858
		public static bool TryGetCommandLineArg(string key, out string value)
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				if (commandLineArgs[i].Contains('='))
				{
					string[] array = commandLineArgs[i].Split('=', StringSplitOptions.None);
					if (array.Length == 2 && (string.Compare(array[0], key, true) == 0 || string.Compare(array[0], "-" + key, true) == 0))
					{
						value = array[1];
						return true;
					}
				}
			}
			value = null;
			return false;
		}

		// Token: 0x06005485 RID: 21637 RVA: 0x001B36C4 File Offset: 0x001B18C4
		public static void Restart()
		{
			try
			{
				string[] commandLineArgs = Environment.GetCommandLineArgs();
				string fileName = commandLineArgs[0];
				string text = "";
				for (int i = 1; i < commandLineArgs.Length; i++)
				{
					if (!text.NullOrEmpty())
					{
						text += " ";
					}
					text = text + "\"" + commandLineArgs[i].Replace("\"", "\\\"") + "\"";
				}
				new Process
				{
					StartInfo = new ProcessStartInfo(fileName, text)
				}.Start();
				Root.Shutdown();
				LongEventHandler.QueueLongEvent(delegate()
				{
					Thread.Sleep(10000);
				}, "Restarting", true, null, true, false, null);
			}
			catch (Exception ex)
			{
				string str = "Error restarting: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				Find.WindowStack.Add(new Dialog_MessageBox("FailedToRestart".Translate(), null, null, null, null, null, false, null, null, WindowLayer.Dialog));
			}
		}
	}
}
