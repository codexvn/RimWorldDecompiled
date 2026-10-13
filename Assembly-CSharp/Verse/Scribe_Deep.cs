using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000863 RID: 2147
	public class Scribe_Deep
	{
		// Token: 0x06003635 RID: 13877 RVA: 0x00122628 File Offset: 0x00120828
		public static void Look<T>(ref T target, string label, params object[] ctorArgs)
		{
			Scribe_Deep.Look<T>(ref target, false, label, ctorArgs);
		}

		// Token: 0x06003636 RID: 13878 RVA: 0x00122634 File Offset: 0x00120834
		public static void Look<T>(ref T target, bool saveDestroyedThings, string label, params object[] ctorArgs)
		{
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				if (Scribe.mode == LoadSaveMode.LoadingVars)
				{
					try
					{
						IDisposable disposable = target as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
						target = ScribeExtractor.SaveableFromNode<T>(Scribe.loader.curXmlParent[label], ctorArgs);
					}
					catch (Exception ex)
					{
						string str = "Exception while loading ";
						string str2 = Scribe.loader.curXmlParent[label].ToStringSafe<XmlElement>();
						string str3 = ": ";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
						target = default(T);
					}
				}
				return;
			}
			Thing thing = target as Thing;
			if (thing != null && thing.Destroyed)
			{
				if (!saveDestroyedThings)
				{
					string str4 = "Deep-saving destroyed thing ";
					Thing thing2 = thing;
					Log.Warning(str4 + ((thing2 != null) ? thing2.ToString() : null) + " with saveDestroyedThings==false. label=" + label);
				}
				else if (thing.Discarded)
				{
					string str5 = "Deep-saving discarded thing ";
					Thing thing3 = thing;
					Log.Warning(str5 + ((thing3 != null) ? thing3.ToString() : null) + ". This mode means that the thing is no longer managed by anything in the code and should not be deep-saved anywhere. (even with saveDestroyedThings==true) , label=" + label);
				}
			}
			IExposable exposable = target as IExposable;
			if (target != null && exposable == null)
			{
				string str6 = "Cannot use LookDeep to save non-IExposable non-null ";
				string str7 = " of type ";
				Type typeFromHandle = typeof(T);
				Log.Error(str6 + label + str7 + ((typeFromHandle != null) ? typeFromHandle.ToString() : null));
				return;
			}
			if (target == null)
			{
				if (!Scribe.EnterNode(label))
				{
					goto IL_190;
				}
				try
				{
					Scribe.saver.WriteAttribute("IsNull", "True");
					goto IL_190;
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			if (Scribe.EnterNode(label))
			{
				try
				{
					if (target.GetType() != typeof(T) || typeof(T).IsGenericTypeDefinition)
					{
						Scribe.saver.WriteAttribute("Class", GenTypes.GetTypeNameWithoutIgnoredNamespaces(target.GetType()));
					}
					exposable.ExposeData();
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					string str8 = "Exception while saving ";
					string str9 = exposable.ToStringSafe<IExposable>();
					string str10 = ": ";
					Exception ex4 = ex3;
					Log.Error(str8 + str9 + str10 + ((ex4 != null) ? ex4.ToString() : null));
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			IL_190:
			Scribe.saver.loadIDsErrorsChecker.RegisterDeepSaved(target, label);
		}
	}
}
