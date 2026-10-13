using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Verse
{
	// Token: 0x02000567 RID: 1383
	public class ModAssemblyHandler
	{
		// Token: 0x060023E7 RID: 9191 RVA: 0x000BA8F5 File Offset: 0x000B8AF5
		public ModAssemblyHandler(ModContentPack mod)
		{
			this.mod = mod;
		}

		// Token: 0x060023E8 RID: 9192 RVA: 0x000BA910 File Offset: 0x000B8B10
		public void ReloadAll()
		{
			if (!ModAssemblyHandler.globalResolverIsSet)
			{
				ResolveEventHandler @object = (object obj, ResolveEventArgs args) => Assembly.GetExecutingAssembly();
				AppDomain.CurrentDomain.AssemblyResolve += @object.Invoke;
				ModAssemblyHandler.globalResolverIsSet = true;
			}
			foreach (FileInfo fileInfo in from f in ModContentPack.GetAllFilesForModPreserveOrder(this.mod, "Assemblies/", (string e) => e.ToLower() == ".dll", null)
			select f.Item2)
			{
				Assembly assembly;
				try
				{
					assembly = Assembly.LoadFrom(fileInfo.FullName);
				}
				catch (Exception arg)
				{
					Log.Error(string.Format("Exception loading {0}: {1}", fileInfo.Name, arg));
					break;
				}
				if (this.AssemblyIsUsable(assembly))
				{
					GenTypes.ClearCache();
					this.loadedAssemblies.Add(assembly);
				}
			}
		}

		// Token: 0x060023E9 RID: 9193 RVA: 0x000BAA38 File Offset: 0x000B8C38
		private bool AssemblyIsUsable(Assembly asm)
		{
			try
			{
				asm.GetTypes();
			}
			catch (ReflectionTypeLoadException ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(string.Format("ReflectionTypeLoadException getting types in assembly {0}: {1}", asm.GetName().Name, ex));
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Loader exceptions:");
				if (ex.LoaderExceptions != null)
				{
					foreach (Exception arg in ex.LoaderExceptions)
					{
						stringBuilder.AppendLine(string.Format("   => {0}", arg));
					}
				}
				Log.Error(stringBuilder.ToString());
				return false;
			}
			catch (Exception ex2)
			{
				string str = "Exception getting types in assembly ";
				string name = asm.GetName().Name;
				string str2 = ": ";
				Exception ex3 = ex2;
				Log.Error(str + name + str2 + ((ex3 != null) ? ex3.ToString() : null));
				return false;
			}
			return true;
		}

		// Token: 0x04001DBB RID: 7611
		private ModContentPack mod;

		// Token: 0x04001DBC RID: 7612
		public List<Assembly> loadedAssemblies = new List<Assembly>();

		// Token: 0x04001DBD RID: 7613
		private static bool globalResolverIsSet;
	}
}
