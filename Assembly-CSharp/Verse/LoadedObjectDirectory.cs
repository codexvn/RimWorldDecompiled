using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000851 RID: 2129
	public class LoadedObjectDirectory
	{
		// Token: 0x060035D4 RID: 13780 RVA: 0x0011E67A File Offset: 0x0011C87A
		public void Clear()
		{
			this.allObjectsByLoadID.Clear();
			this.allThingsByThingID.Clear();
		}

		// Token: 0x060035D5 RID: 13781 RVA: 0x0011E694 File Offset: 0x0011C894
		public void RegisterLoaded(ILoadReferenceable reffable)
		{
			if (Prefs.DevMode)
			{
				string text = "[excepted]";
				try
				{
					text = reffable.GetUniqueLoadID();
				}
				catch (Exception)
				{
				}
				string text2 = "[excepted]";
				try
				{
					text2 = reffable.ToString();
				}
				catch (Exception)
				{
				}
				ILoadReferenceable loadReferenceable;
				if (this.allObjectsByLoadID.TryGetValue(text, out loadReferenceable))
				{
					string text3 = "";
					string[] array = new string[12];
					array[0] = "Cannot register ";
					int num = 1;
					Type type = reffable.GetType();
					array[num] = ((type != null) ? type.ToString() : null);
					array[2] = " ";
					array[3] = text2;
					array[4] = ", (id=";
					array[5] = text;
					array[6] = " in loaded object directory. Id already used by ";
					int num2 = 7;
					Type type2 = loadReferenceable.GetType();
					array[num2] = ((type2 != null) ? type2.ToString() : null);
					array[8] = " ";
					array[9] = loadReferenceable.ToStringSafe<ILoadReferenceable>();
					array[10] = ".";
					array[11] = text3;
					Log.Error(string.Concat(array));
					return;
				}
			}
			try
			{
				this.allObjectsByLoadID.Add(reffable.GetUniqueLoadID(), reffable);
			}
			catch (Exception ex)
			{
				string text4 = "[excepted]";
				try
				{
					text4 = reffable.GetUniqueLoadID();
				}
				catch (Exception)
				{
				}
				string text5 = "[excepted]";
				try
				{
					text5 = reffable.ToString();
				}
				catch (Exception)
				{
				}
				string[] array2 = new string[8];
				array2[0] = "Exception registering ";
				int num3 = 1;
				Type type3 = reffable.GetType();
				array2[num3] = ((type3 != null) ? type3.ToString() : null);
				array2[2] = " ";
				array2[3] = text5;
				array2[4] = " in loaded object directory with unique load ID ";
				array2[5] = text4;
				array2[6] = ": ";
				int num4 = 7;
				Exception ex2 = ex;
				array2[num4] = ((ex2 != null) ? ex2.ToString() : null);
				Log.Error(string.Concat(array2));
			}
			Thing thing = reffable as Thing;
			if (thing != null)
			{
				try
				{
					this.allThingsByThingID.Add(thing.thingIDNumber, reffable);
				}
				catch (Exception ex3)
				{
					string text6 = "[excepted]";
					try
					{
						text6 = reffable.ToString();
					}
					catch (Exception)
					{
					}
					string[] array3 = new string[8];
					array3[0] = "Exception registering ";
					int num5 = 1;
					Type type4 = reffable.GetType();
					array3[num5] = ((type4 != null) ? type4.ToString() : null);
					array3[2] = " ";
					array3[3] = text6;
					array3[4] = " in loaded object directory with unique thing ID ";
					array3[5] = thing.thingIDNumber.ToString();
					array3[6] = ": ";
					int num6 = 7;
					Exception ex4 = ex3;
					array3[num6] = ((ex4 != null) ? ex4.ToString() : null);
					Log.Error(string.Concat(array3));
				}
			}
		}

		// Token: 0x060035D6 RID: 13782 RVA: 0x0011E910 File Offset: 0x0011CB10
		public T ObjectWithLoadID<T>(string loadID)
		{
			if (loadID.NullOrEmpty() || loadID == "null")
			{
				T result = default(T);
				return result;
			}
			ILoadReferenceable loadReferenceable;
			if (this.allObjectsByLoadID.TryGetValue(loadID, out loadReferenceable))
			{
				if (loadReferenceable == null)
				{
					T result = default(T);
					return result;
				}
				try
				{
					return (T)((object)loadReferenceable);
				}
				catch (Exception ex)
				{
					string[] array = new string[8];
					array[0] = "Exception getting object with load id ";
					array[1] = loadID;
					array[2] = " of type ";
					int num = 3;
					Type typeFromHandle = typeof(T);
					array[num] = ((typeFromHandle != null) ? typeFromHandle.ToString() : null);
					array[4] = ". What we loaded was ";
					array[5] = loadReferenceable.ToStringSafe<ILoadReferenceable>();
					array[6] = ". Exception:\n";
					int num2 = 7;
					Exception ex2 = ex;
					array[num2] = ((ex2 != null) ? ex2.ToString() : null);
					Log.Error(string.Concat(array));
					return default(T);
				}
			}
			if (typeof(Thing).IsAssignableFrom(typeof(T)) && this.allThingsByThingID.TryGetValue(Thing.IDNumberFromThingID(loadID), out loadReferenceable))
			{
				Log.Warning("Could not resolve reference to Thing with loadID " + loadID + ". Resolving reference by using thingIDNumber instead (back-compat).");
				if (loadReferenceable == null)
				{
					return default(T);
				}
				try
				{
					return (T)((object)loadReferenceable);
				}
				catch (Exception ex3)
				{
					string[] array2 = new string[8];
					array2[0] = "Exception getting object with thing id ";
					array2[1] = Thing.IDNumberFromThingID(loadID).ToString();
					array2[2] = " of type ";
					int num3 = 3;
					Type typeFromHandle2 = typeof(T);
					array2[num3] = ((typeFromHandle2 != null) ? typeFromHandle2.ToString() : null);
					array2[4] = ". What we loaded was ";
					array2[5] = loadReferenceable.ToStringSafe<ILoadReferenceable>();
					array2[6] = ". Exception:\n";
					int num4 = 7;
					Exception ex4 = ex3;
					array2[num4] = ((ex4 != null) ? ex4.ToString() : null);
					Log.Error(string.Concat(array2));
					return default(T);
				}
			}
			string[] array3 = new string[8];
			array3[0] = "Could not resolve reference to object with loadID ";
			array3[1] = loadID;
			array3[2] = " of type ";
			int num5 = 3;
			Type typeFromHandle3 = typeof(T);
			array3[num5] = ((typeFromHandle3 != null) ? typeFromHandle3.ToString() : null);
			array3[4] = ". Was it compressed away, destroyed, had no ID number, or not saved/loaded right? curParent=";
			array3[5] = Scribe.loader.curParent.ToStringSafe<IExposable>();
			array3[6] = " curPathRelToParent=";
			array3[7] = Scribe.loader.curPathRelToParent;
			Log.Warning(string.Concat(array3));
			return default(T);
		}

		// Token: 0x04002892 RID: 10386
		private Dictionary<string, ILoadReferenceable> allObjectsByLoadID = new Dictionary<string, ILoadReferenceable>();

		// Token: 0x04002893 RID: 10387
		private Dictionary<int, ILoadReferenceable> allThingsByThingID = new Dictionary<int, ILoadReferenceable>();
	}
}
