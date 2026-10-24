using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Waylith.Profiling;

namespace Waylith.UnityCompat;

public sealed class UnityWorld
{
	private abstract class UnityObjectState
	{
		public long CreationIndex { get; }

		public bool Destroyed { get; set; }

		public object Instance { get; }

		public int InstanceId { get; }

		public string Name { get; set; }

		protected UnityObjectState(object instance, int instanceId, long creationIndex, string name)
		{
			Instance = instance;
			InstanceId = instanceId;
			CreationIndex = creationIndex;
			Name = name;
		}
	}

	private sealed class UnityGameObjectState : UnityObjectState
	{
		public bool ActiveSelf { get; set; } = true;

		public List<UnityComponentState> Components { get; } = new List<UnityComponentState>();

		public bool IsAssetToken { get; }

		public string SceneName { get; set; }

		public UnityTransformState Transform { get; set; }

		public UnityWorld World => CurrentWorld;

		public UnityGameObjectState(object instance, int instanceId, long creationIndex, string name, bool isAssetToken)
			: base(instance, instanceId, creationIndex, name)
		{
			IsAssetToken = isAssetToken;
			SceneName = "Main";
		}
	}

	private class UnityComponentState : UnityObjectState
	{
		public UnityGameObjectState GameObject { get; }

		public UnityMessageCache Messages { get; init; } = UnityMessageCache.Empty;

		public UnityComponentState(object instance, int instanceId, long creationIndex, string name, UnityGameObjectState gameObject)
			: base(instance, instanceId, creationIndex, name)
		{
			GameObject = gameObject;
		}
	}

	private sealed class UnityBehaviourState : UnityComponentState
	{
		public bool Enabled { get; set; } = true;

		public bool HasAwoken { get; set; }

		public bool HasStarted { get; set; }

		public bool IsActiveInHierarchy { get; set; }

		public int GuiDepth { get; set; }

		public UnityBehaviourState(object instance, int instanceId, long creationIndex, string name, UnityGameObjectState gameObject)
			: base(instance, instanceId, creationIndex, name, gameObject)
		{
		}
	}

	private sealed class UnityTransformState : UnityComponentState
	{
		public List<UnityTransformState> Children { get; } = new List<UnityTransformState>();

		public UnityTransformState? Parent { get; set; }

		public UnityTransformState(object instance, int instanceId, long creationIndex, string name, UnityGameObjectState gameObject)
			: base(instance, instanceId, creationIndex, name, gameObject)
		{
		}
	}

	private sealed record UnityResourceEntry(string Path, Type Type, object Asset);

	private readonly record struct UnityMessageCache(Func<object, object?>? Awake, Func<object, object?>? OnEnable, Func<object, object?>? Start, Func<object, object?>? Update, Func<object, object?>? LateUpdate, Func<object, object?>? OnDisable, Func<object, object?>? OnApplicationQuit, Func<object, object?>? OnDestroy, Func<object, object?>? OnGUI)
	{
		public static UnityMessageCache Empty { get; } = new UnityMessageCache(null, null, null, null, null, null, null, null, null);

		public Func<object, object?>? Get(UnityMessageType type)
		{
			return type switch
			{
				UnityMessageType.Awake => Awake, 
				UnityMessageType.OnEnable => OnEnable, 
				UnityMessageType.Start => Start, 
				UnityMessageType.Update => Update, 
				UnityMessageType.LateUpdate => LateUpdate, 
				UnityMessageType.OnDisable => OnDisable, 
				UnityMessageType.OnApplicationQuit => OnApplicationQuit, 
				UnityMessageType.OnDestroy => OnDestroy, 
				UnityMessageType.OnGUI => OnGUI, 
				_ => null, 
			};
		}
	}

	private enum UnityMessageType
	{
		Awake,
		OnEnable,
		Start,
		Update,
		LateUpdate,
		OnDisable,
		OnApplicationQuit,
		OnDestroy,
		OnGUI
	}

	[CompilerGenerated]
	private sealed class _003CEnumerateLiveGameObjects_003Ed__101 : IEnumerable<UnityGameObjectState>, IEnumerable, IEnumerator<UnityGameObjectState>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private UnityGameObjectState _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public UnityWorld _003C_003E4__this;

		private IEnumerator<UnityGameObjectState> _003C_003E7__wrap1;

		UnityGameObjectState IEnumerator<UnityGameObjectState>.Current
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
		public _003CEnumerateLiveGameObjects_003Ed__101(int _003C_003E1__state)
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
				UnityWorld unityWorld = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = unityWorld._objects.Values.OfType<UnityGameObjectState>().GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					UnityGameObjectState current = _003C_003E7__wrap1.Current;
					if (!current.Destroyed && !current.IsAssetToken)
					{
						_003C_003E2__current = current;
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
		IEnumerator<UnityGameObjectState> IEnumerable<UnityGameObjectState>.GetEnumerator()
		{
			_003CEnumerateLiveGameObjects_003Ed__101 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CEnumerateLiveGameObjects_003Ed__101(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<UnityGameObjectState>)this).GetEnumerator();
		}
	}

	private const string DefaultSceneName = "Main";

	private const string DontDestroyOnLoadSceneName = "DontDestroyOnLoad";

	private static readonly LogCategory Log = LogCategory.Make("UnityWorld");

	private readonly Lock _gate = new Lock();

	private readonly ConcurrentDictionary<object, UnityObjectState> _objects = new ConcurrentDictionary<object, UnityObjectState>(ReferenceEqualityComparer.Instance);

	private readonly Dictionary<object, Func<object>> _cloneFactories = new Dictionary<object, Func<object>>(ReferenceEqualityComparer.Instance);

	private readonly Dictionary<Type, int> _executionOrders = new Dictionary<Type, int>();

	private readonly Dictionary<Type, UnityMessageCache> _messageCache = new Dictionary<Type, UnityMessageCache>();

	private static readonly ConcurrentDictionary<Type, string> _profilerLabels = new ConcurrentDictionary<Type, string>();

	private readonly List<UnityGameObjectState> _rootGameObjects = new List<UnityGameObjectState>();

	private readonly List<UnityBehaviourState> _behaviours = new List<UnityBehaviourState>();

	private readonly List<UnityBehaviourState> _behaviourDispatchScratch = new List<UnityBehaviourState>();

	private readonly List<UnityBehaviourState> _guiSnapshotScratch = new List<UnityBehaviourState>();

	private readonly List<(int Depth, int Order, UnityBehaviourState Behaviour)> _guiDispatchScratch = new List<(int, int, UnityBehaviourState)>();

	private readonly List<UnityBehaviourState> _pendingActivationScratch = new List<UnityBehaviourState>();

	private readonly List<UnityBehaviourState> _pendingStartScratch = new List<UnityBehaviourState>();

	private readonly List<UnityBehaviourState> _shutdownScratch = new List<UnityBehaviourState>();

	private readonly List<UnityBehaviourState> _pendingActivation = new List<UnityBehaviourState>();

	private readonly HashSet<UnityBehaviourState> _pendingActivationSet = new HashSet<UnityBehaviourState>();

	private readonly List<UnityBehaviourState> _pendingStart = new List<UnityBehaviourState>();

	private readonly HashSet<UnityBehaviourState> _pendingStartSet = new HashSet<UnityBehaviourState>();

	private readonly List<UnityObjectState> _pendingDestroy = new List<UnityObjectState>();

	private readonly HashSet<UnityObjectState> _pendingDestroySet = new HashSet<UnityObjectState>();

	private readonly List<UnityResourceEntry> _resources = new List<UnityResourceEntry>();

	private int _constructionDepth;

	private int _nextInstanceId = 1;

	private long _nextCreationIndex;

	private bool _shutdownInvoked;

	private bool _isFlushingPendingActivation;

	private bool _isFlushingPendingStart;

	public int GuiDepth { get; set; }

	public Action<object, IEnumerator>? MessageCoroutineStarter { get; set; }

	public string ActiveSceneName { get; set; } = "Main";

	public static UnityWorld? Current { get; set; }

	private static UnityWorld CurrentWorld => Current ?? throw new InvalidOperationException("Unity compat world has not been initialized.");

	public event Action<UnityFrame>? Updating;

	public event Action<UnityFrame>? LateUpdating;

	public event Action<UnityFrame>? PostUpdate;

	public event Action<UnityFrame>? PostLateUpdate;

	public event Action<object>? ObjectDestroyed;

	public void RegisterExecutionOrder(Type type, int order)
	{
		_executionOrders[type] = order;
		SortBehavioursByExecutionOrder();
	}

	public void RegisterResource(string path, Type type, object asset)
	{
		_resources.RemoveAll((UnityResourceEntry entry) => entry.Path == path && entry.Type == type);
		_resources.Add(new UnityResourceEntry(path, type, asset));
	}

	public void RegisterPrefab(string path, Type type, object assetToken, Func<object> instantiate)
	{
		RegisterResource(path, type, assetToken);
		_cloneFactories[assetToken] = instantiate;
	}

	public void RegisterCloneFactory(object assetToken, Func<object> instantiate)
	{
		using (_gate.EnterScope())
		{
			_cloneFactories[assetToken] = instantiate;
		}
	}

	public object? LoadResource(string path, Type requestedType)
	{
		for (int num = _resources.Count - 1; num >= 0; num--)
		{
			UnityResourceEntry unityResourceEntry = _resources[num];
			if (unityResourceEntry.Path == path && requestedType.IsAssignableFrom(unityResourceEntry.Type))
			{
				return unityResourceEntry.Asset;
			}
		}
		return null;
	}

	public Array LoadAllResources(string path, Type requestedType)
	{
		List<object> list = new List<object>();
		string value = (path.EndsWith('/') ? path : (path + "/"));
		foreach (UnityResourceEntry resource in _resources)
		{
			if (requestedType.IsAssignableFrom(resource.Type) && (resource.Path == path || resource.Path.StartsWith(value, StringComparison.Ordinal)))
			{
				list.Add(resource.Asset);
			}
		}
		Array array = Array.CreateInstance(requestedType, list.Count);
		for (int i = 0; i < list.Count; i++)
		{
			array.SetValue(list[i], i);
		}
		return array;
	}

	public void InitializeGameObject(object gameObject, object transform, string name, bool isAssetToken)
	{
		using (_gate.EnterScope())
		{
			UnityGameObjectState unityGameObjectState = new UnityGameObjectState(gameObject, NextInstanceId(), NextCreationIndex(), name, isAssetToken);
			UnityTransformState unityTransformState2 = (unityGameObjectState.Transform = new UnityTransformState(transform, NextInstanceId(), NextCreationIndex(), "Transform", unityGameObjectState));
			unityGameObjectState.Components.Add(unityTransformState2);
			if (!isAssetToken)
			{
				unityGameObjectState.SceneName = ActiveSceneName;
			}
			_objects[gameObject] = unityGameObjectState;
			_objects[transform] = unityTransformState2;
			if (!isAssetToken)
			{
				_rootGameObjects.Add(unityGameObjectState);
			}
		}
	}

	public object AddComponent(object gameObject, Type componentType)
	{
		using (_gate.EnterScope())
		{
			UnityGameObjectState gameObjectState = GetGameObjectState(gameObject);
			if (gameObjectState.Transform != null && componentType.IsInstanceOfType(gameObjectState.Transform.Instance))
			{
				return gameObjectState.Transform.Instance;
			}
			object obj = Activator.CreateInstance(componentType) ?? throw new InvalidOperationException("Failed to instantiate component " + componentType.FullName + ".");
			UnityComponentState unityComponentState = CreateComponentState(obj, gameObjectState);
			RegisterComponent(unityComponentState);
			if (_constructionDepth == 0)
			{
				if (_isFlushingPendingActivation && unityComponentState is UnityBehaviourState behaviour)
				{
					FlushPendingActivationInline(behaviour);
				}
				else
				{
					FlushPendingActivation();
				}
			}
			return obj;
		}
	}

	public object? InstantiateObject(object original)
	{
		using (_gate.EnterScope())
		{
			if (!_cloneFactories.TryGetValue(original, out Func<object> value))
			{
				return original;
			}
			_constructionDepth++;
			try
			{
				return value();
			}
			finally
			{
				_constructionDepth--;
				if (_constructionDepth == 0)
				{
					FlushPendingActivation();
				}
			}
		}
	}

	public object? FindGameObject(string name)
	{
		foreach (UnityGameObjectState item in EnumerateLiveGameObjects())
		{
			if (item.Name == name)
			{
				return item.Instance;
			}
		}
		return null;
	}

	public Array FindObjectsByType(Type requestedType, bool includeInactive)
	{
		List<object> list = new List<object>();
		if (IsGameObjectType(requestedType))
		{
			foreach (UnityGameObjectState item in EnumerateLiveGameObjects())
			{
				if (includeInactive || IsGameObjectActiveInHierarchy(item))
				{
					list.Add(item.Instance);
				}
			}
		}
		else
		{
			foreach (UnityComponentState item2 in _objects.Values.OfType<UnityComponentState>())
			{
				if (!item2.GameObject.IsAssetToken && !item2.Destroyed && requestedType.IsInstanceOfType(item2.Instance) && (includeInactive || IsComponentVisibleForFind(item2)))
				{
					list.Add(item2.Instance);
				}
			}
		}
		Array array = Array.CreateInstance(requestedType, list.Count);
		for (int i = 0; i < list.Count; i++)
		{
			array.SetValue(list[i], i);
		}
		return array;
	}

	public object? GetComponent(object gameObject, Type componentType)
	{
		foreach (UnityComponentState component in GetGameObjectState(gameObject).Components)
		{
			if (!component.Destroyed && componentType.IsInstanceOfType(component.Instance))
			{
				return component.Instance;
			}
		}
		return null;
	}

	public Array GetComponents(object gameObject, Type componentType)
	{
		UnityGameObjectState gameObjectState = GetGameObjectState(gameObject);
		List<object> list = new List<object>();
		foreach (UnityComponentState component in gameObjectState.Components)
		{
			if (!component.Destroyed && componentType.IsInstanceOfType(component.Instance))
			{
				list.Add(component.Instance);
			}
		}
		Array array = Array.CreateInstance(componentType, list.Count);
		for (int i = 0; i < list.Count; i++)
		{
			array.SetValue(list[i], i);
		}
		return array;
	}

	public Array GetComponentsInChildren(object gameObject, Type componentType, bool includeInactive)
	{
		List<object> list = new List<object>();
		AppendComponentsInChildren(GetGameObjectState(gameObject), componentType, includeInactive, list);
		Array array = Array.CreateInstance(componentType, list.Count);
		for (int i = 0; i < list.Count; i++)
		{
			array.SetValue(list[i], i);
		}
		return array;
	}

	public object? GetComponentInChildren(object gameObject, Type componentType, bool includeInactive)
	{
		UnityGameObjectState gameObjectState = GetGameObjectState(gameObject);
		foreach (UnityComponentState component in gameObjectState.Components)
		{
			if (!component.Destroyed && componentType.IsInstanceOfType(component.Instance))
			{
				return component.Instance;
			}
		}
		foreach (UnityTransformState child in gameObjectState.Transform.Children)
		{
			if (includeInactive || IsGameObjectActiveInHierarchy(child.GameObject))
			{
				object componentInChildren = GetComponentInChildren(child.GameObject.Instance, componentType, includeInactive);
				if (componentInChildren != null)
				{
					return componentInChildren;
				}
			}
		}
		return null;
	}

	public object GetGameObject(object component)
	{
		return GetComponentState(component).GameObject.Instance;
	}

	public object GetTransform(object componentOrGameObject)
	{
		if (_objects.TryGetValue(componentOrGameObject, out UnityObjectState value))
		{
			if (value is UnityGameObjectState unityGameObjectState)
			{
				return unityGameObjectState.Transform.Instance;
			}
			if (value is UnityComponentState unityComponentState)
			{
				return unityComponentState.GameObject.Transform.Instance;
			}
		}
		throw new InvalidOperationException("Object is not tracked by the Unity world.");
	}

	public bool TryGetComponent(object gameObject, Type componentType, out object? component)
	{
		component = GetComponent(gameObject, componentType);
		return component != null;
	}

	public string? GetName(object instance)
	{
		if (!_objects.TryGetValue(instance, out UnityObjectState value))
		{
			return null;
		}
		return value.Name;
	}

	public void SetName(object instance, string name)
	{
		if (_objects.TryGetValue(instance, out UnityObjectState value))
		{
			value.Name = name;
		}
	}

	public int GetInstanceId(object instance)
	{
		if (!_objects.TryGetValue(instance, out UnityObjectState value))
		{
			return 0;
		}
		return value.InstanceId;
	}

	public bool IsDestroyed(object instance)
	{
		if (_objects.TryGetValue(instance, out UnityObjectState value))
		{
			return value.Destroyed;
		}
		return false;
	}

	public bool IsGameObjectActiveInHierarchy(object gameObject)
	{
		return IsGameObjectActiveInHierarchy(GetGameObjectState(gameObject));
	}

	public bool IsGameObjectActiveSelf(object gameObject)
	{
		return GetGameObjectState(gameObject).ActiveSelf;
	}

	public void SetGameObjectActive(object gameObject, bool active)
	{
		using (_gate.EnterScope())
		{
			UnityGameObjectState gameObjectState = GetGameObjectState(gameObject);
			if (gameObjectState.ActiveSelf != active)
			{
				gameObjectState.ActiveSelf = active;
				RefreshHierarchyActivity(gameObjectState);
				if (_constructionDepth == 0)
				{
					FlushPendingActivation();
				}
			}
		}
	}

	public bool IsBehaviourEnabled(object behaviour)
	{
		return GetBehaviourState(behaviour).Enabled;
	}

	public void SetBehaviourEnabled(object behaviour, bool enabled)
	{
		using (_gate.EnterScope())
		{
			UnityBehaviourState behaviourState = GetBehaviourState(behaviour);
			if (behaviourState.Enabled != enabled)
			{
				behaviourState.Enabled = enabled;
				RefreshBehaviourActivity(behaviourState);
				if (_constructionDepth == 0)
				{
					FlushPendingActivation();
				}
			}
		}
	}

	public bool IsBehaviourActiveAndEnabled(object behaviour)
	{
		UnityBehaviourState behaviourState = GetBehaviourState(behaviour);
		if (behaviourState.Enabled)
		{
			return behaviourState.IsActiveInHierarchy;
		}
		return false;
	}

	public int GetTransformChildCount(object transform)
	{
		return GetTransformState(transform).Children.Count;
	}

	public object GetTransformChild(object transform, int index)
	{
		return GetTransformState(transform).Children[index].Instance;
	}

	public object? GetTransformParent(object transform)
	{
		return GetTransformState(transform).Parent?.Instance;
	}

	public Array GetTransformChildren(object transform)
	{
		UnityTransformState transformState = GetTransformState(transform);
		Array array = Array.CreateInstance(transform.GetType(), transformState.Children.Count);
		for (int i = 0; i < transformState.Children.Count; i++)
		{
			array.SetValue(transformState.Children[i].Instance, i);
		}
		return array;
	}

	public int GetTransformSiblingIndex(object transform)
	{
		UnityTransformState transformState = GetTransformState(transform);
		return transformState.Parent?.Children.IndexOf(transformState) ?? _rootGameObjects.IndexOf(transformState.GameObject);
	}

	public void SetTransformSiblingIndex(object transform, int index)
	{
		using (_gate.EnterScope())
		{
			UnityTransformState transformState = GetTransformState(transform);
			if (transformState.Parent != null)
			{
				transformState.Parent.Children.Remove(transformState);
				transformState.Parent.Children.Insert(System.Math.Clamp(index, 0, transformState.Parent.Children.Count), transformState);
			}
			else if (_rootGameObjects.Remove(transformState.GameObject))
			{
				_rootGameObjects.Insert(System.Math.Clamp(index, 0, _rootGameObjects.Count), transformState.GameObject);
			}
		}
	}

	public void SetTransformParent(object transform, object? parentTransform)
	{
		using (_gate.EnterScope())
		{
			UnityTransformState transformState = GetTransformState(transform);
			UnityTransformState unityTransformState = ((parentTransform == null) ? null : GetTransformState(parentTransform));
			if (transformState.Parent != unityTransformState)
			{
				if (transformState.Parent != null)
				{
					transformState.Parent.Children.Remove(transformState);
				}
				else
				{
					_rootGameObjects.Remove(transformState.GameObject);
				}
				transformState.Parent = unityTransformState;
				if (unityTransformState != null)
				{
					unityTransformState.Children.Add(transformState);
					SetSceneRecursive(transformState.GameObject, unityTransformState.GameObject.SceneName);
				}
				else
				{
					_rootGameObjects.Add(transformState.GameObject);
				}
				RefreshHierarchyActivity(transformState.GameObject);
				if (_constructionDepth == 0)
				{
					FlushPendingActivation();
				}
			}
		}
	}

	public void DontDestroyOnLoad(object target)
	{
		using (_gate.EnterScope())
		{
			_objects.TryGetValue(target, out UnityObjectState value);
			UnityGameObjectState unityGameObjectState2 = ((value is UnityGameObjectState unityGameObjectState) ? unityGameObjectState : ((!(value is UnityComponentState unityComponentState)) ? null : unityComponentState.GameObject));
			UnityGameObjectState unityGameObjectState3 = unityGameObjectState2;
			if (unityGameObjectState3 != null)
			{
				SetSceneRecursive(unityGameObjectState3, "DontDestroyOnLoad");
			}
		}
	}

	public string GetSceneName(object gameObject)
	{
		return GetGameObjectState(gameObject).SceneName;
	}

	public void UnloadScene(string sceneName)
	{
		UnityGameObjectState[] array = (from state in EnumerateLiveGameObjects()
			where state.SceneName == sceneName && state.Transform.Parent == null
			select state).ToArray();
		foreach (UnityGameObjectState gameObject in array)
		{
			DestroyNow(gameObject);
		}
	}

	public Array GetSceneRootGameObjects(string sceneName, Type gameObjectType)
	{
		object[] array = (from state in EnumerateLiveGameObjects()
			where state.SceneName == sceneName && state.Transform.Parent == null
			select state.Instance).ToArray();
		Array array2 = Array.CreateInstance(gameObjectType, array.Length);
		for (int i = 0; i < array.Length; i++)
		{
			array2.SetValue(array[i], i);
		}
		return array2;
	}

	public void DestroyObject(object target, bool immediate)
	{
		using (_gate.EnterScope())
		{
			if (!_objects.TryGetValue(target, out UnityObjectState value))
			{
				return;
			}
			UnityGameObjectState unityGameObjectState;
			if (value is UnityComponentState unityComponentState)
			{
				if (unityComponentState is UnityTransformState)
				{
					return;
				}
				unityGameObjectState = unityComponentState.GameObject;
			}
			else
			{
				if (!(value is UnityGameObjectState unityGameObjectState2))
				{
					return;
				}
				unityGameObjectState = unityGameObjectState2;
			}
			if (!unityGameObjectState.IsAssetToken && !value.Destroyed)
			{
				if (immediate)
				{
					DestroyNow(value);
				}
				else if (_pendingDestroySet.Add(value))
				{
					_pendingDestroy.Add(value);
				}
			}
		}
	}

	public void Update(in UnityFrame frame)
	{
		using (FrameProfiler.Scope("World.Update"))
		{
			ProcessPendingDestroy();
			FlushPendingActivation();
			FlushPendingStart();
			this.Updating?.Invoke(frame);
			InvokeMessagesForActiveBehaviours(UnityMessageType.Update);
			this.PostUpdate?.Invoke(frame);
		}
	}

	public void LateUpdate(in UnityFrame frame)
	{
		using (FrameProfiler.Scope("World.LateUpdate"))
		{
			this.LateUpdating?.Invoke(frame);
			InvokeMessagesForActiveBehaviours(UnityMessageType.LateUpdate);
			ProcessPendingDestroy();
			this.PostLateUpdate?.Invoke(frame);
		}
	}

	public void Shutdown()
	{
		using (_gate.EnterScope())
		{
			if (!_shutdownInvoked)
			{
				_shutdownInvoked = true;
				ProcessPendingDestroy();
				FlushPendingActivation();
				FlushPendingStart();
				InvokeShutdownMessages();
				UnityGameObjectState[] array = _rootGameObjects.ToArray();
				foreach (UnityGameObjectState gameObject in array)
				{
					DestroyNow(gameObject);
				}
				ProcessPendingDestroy();
			}
		}
	}

	public UnityWorldDiagnosticsSnapshot GetDiagnosticsSnapshot()
	{
		int count = _objects.Count;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		foreach (UnityObjectState value in _objects.Values)
		{
			if (!(value is UnityGameObjectState unityGameObjectState))
			{
				if (!(value is UnityBehaviourState unityBehaviourState))
				{
					if (value is UnityComponentState unityComponentState)
					{
						if (unityComponentState.Destroyed)
						{
							num5++;
						}
						else
						{
							num2++;
						}
					}
				}
				else if (unityBehaviourState.Destroyed)
				{
					num5++;
				}
				else
				{
					num2++;
					num3++;
					if (unityBehaviourState.Enabled && unityBehaviourState.IsActiveInHierarchy)
					{
						num4++;
					}
				}
			}
			else
			{
				if (unityGameObjectState.IsAssetToken)
				{
					num6++;
				}
				if (unityGameObjectState.Destroyed)
				{
					num5++;
				}
				else if (!unityGameObjectState.IsAssetToken)
				{
					num++;
				}
			}
		}
		for (int i = 0; i < _rootGameObjects.Count; i++)
		{
			UnityGameObjectState unityGameObjectState2 = _rootGameObjects[i];
			if (!unityGameObjectState2.Destroyed && !unityGameObjectState2.IsAssetToken)
			{
				num7++;
			}
		}
		return new UnityWorldDiagnosticsSnapshot(count, num, num7, num2, num3, num4, num5, num6, _resources.Count, _pendingActivation.Count, _pendingStart.Count, _pendingDestroy.Count, _shutdownInvoked);
	}

	[IteratorStateMachine(typeof(_003CEnumerateLiveGameObjects_003Ed__101))]
	private IEnumerable<UnityGameObjectState> EnumerateLiveGameObjects()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CEnumerateLiveGameObjects_003Ed__101(-2)
		{
			_003C_003E4__this = this
		};
	}

	private static bool IsGameObjectType(Type type)
	{
		return type.FullName == "UnityEngine.GameObject";
	}

	private void AppendComponentsInChildren(UnityGameObjectState gameObject, Type componentType, bool includeInactive, List<object> matches)
	{
		if (includeInactive || IsGameObjectActiveInHierarchy(gameObject))
		{
			foreach (UnityComponentState component in gameObject.Components)
			{
				if (!component.Destroyed && componentType.IsInstanceOfType(component.Instance))
				{
					matches.Add(component.Instance);
				}
			}
		}
		foreach (UnityTransformState child in gameObject.Transform.Children)
		{
			AppendComponentsInChildren(child.GameObject, componentType, includeInactive, matches);
		}
	}

	private UnityComponentState CreateComponentState(object component, UnityGameObjectState gameObject)
	{
		Type type = component.GetType();
		bool num = IsAssignableToUnityType(type, "UnityEngine.Transform");
		bool flag = IsAssignableToUnityType(type, "UnityEngine.Behaviour");
		UnityMessageCache messages = GetMessages(type);
		if (num)
		{
			if (gameObject.Transform != null)
			{
				return gameObject.Transform;
			}
			return new UnityTransformState(component, NextInstanceId(), NextCreationIndex(), type.Name, gameObject)
			{
				Messages = messages
			};
		}
		if (!flag)
		{
			return new UnityComponentState(component, NextInstanceId(), NextCreationIndex(), type.Name, gameObject)
			{
				Messages = messages
			};
		}
		return new UnityBehaviourState(component, NextInstanceId(), NextCreationIndex(), type.Name, gameObject)
		{
			Messages = messages
		};
	}

	private void RegisterComponent(UnityComponentState componentState)
	{
		_objects[componentState.Instance] = componentState;
		componentState.GameObject.Components.Add(componentState);
		if (componentState is UnityTransformState transform)
		{
			componentState.GameObject.Transform = transform;
		}
		else if (componentState is UnityBehaviourState behaviour)
		{
			InsertBehaviourSorted(behaviour);
			RefreshBehaviourActivity(behaviour);
		}
	}

	private void RefreshHierarchyActivity(UnityGameObjectState gameObject)
	{
		foreach (UnityBehaviourState item in gameObject.Components.OfType<UnityBehaviourState>())
		{
			RefreshBehaviourActivity(item);
		}
		foreach (UnityTransformState child in gameObject.Transform.Children)
		{
			RefreshHierarchyActivity(child.GameObject);
		}
	}

	private void RefreshBehaviourActivity(UnityBehaviourState behaviour)
	{
		if (!behaviour.Enabled || !IsGameObjectActiveInHierarchy(behaviour.GameObject) || behaviour.Destroyed)
		{
			if (behaviour.IsActiveInHierarchy)
			{
				behaviour.IsActiveInHierarchy = false;
				InvokeMessage(behaviour, UnityMessageType.OnDisable);
			}
		}
		else if (_pendingActivationSet.Add(behaviour))
		{
			_pendingActivation.Add(behaviour);
		}
	}

	private void FlushPendingActivationInline(UnityBehaviourState behaviour)
	{
		if (behaviour.Destroyed || behaviour.IsActiveInHierarchy || !behaviour.Enabled || !IsGameObjectActiveInHierarchy(behaviour.GameObject))
		{
			return;
		}
		if (!behaviour.HasAwoken)
		{
			InvokeMessage(behaviour, UnityMessageType.Awake);
			behaviour.HasAwoken = true;
		}
		if (!behaviour.Destroyed && !behaviour.IsActiveInHierarchy && behaviour.Enabled && IsGameObjectActiveInHierarchy(behaviour.GameObject))
		{
			InvokeMessage(behaviour, UnityMessageType.OnEnable);
			behaviour.IsActiveInHierarchy = true;
			if (!behaviour.HasStarted && _pendingStartSet.Add(behaviour))
			{
				_pendingStart.Add(behaviour);
			}
		}
	}

	private void FlushPendingActivation()
	{
		using (_gate.EnterScope())
		{
			if (_isFlushingPendingActivation || _pendingActivation.Count == 0)
			{
				return;
			}
			_isFlushingPendingActivation = true;
			try
			{
				while (_pendingActivation.Count > 0)
				{
					_pendingActivation.Sort(CompareBehaviourStates);
					CopyBehaviours(_pendingActivation, _pendingActivationScratch);
					_pendingActivation.Clear();
					_pendingActivationSet.Clear();
					foreach (UnityBehaviourState item in _pendingActivationScratch)
					{
						if (!item.Destroyed && !item.IsActiveInHierarchy && item.Enabled && IsGameObjectActiveInHierarchy(item.GameObject) && !item.HasAwoken)
						{
							InvokeMessage(item, UnityMessageType.Awake);
							item.HasAwoken = true;
						}
					}
					foreach (UnityBehaviourState item2 in _pendingActivationScratch)
					{
						if (!item2.Destroyed && !item2.IsActiveInHierarchy && item2.Enabled && IsGameObjectActiveInHierarchy(item2.GameObject))
						{
							InvokeMessage(item2, UnityMessageType.OnEnable);
							item2.IsActiveInHierarchy = true;
							if (!item2.HasStarted && _pendingStartSet.Add(item2))
							{
								_pendingStart.Add(item2);
							}
						}
					}
					_pendingActivationScratch.Clear();
				}
			}
			finally
			{
				_pendingActivationScratch.Clear();
				_isFlushingPendingActivation = false;
			}
		}
	}

	private void FlushPendingStart()
	{
		using (_gate.EnterScope())
		{
			if (_isFlushingPendingStart || _pendingStart.Count == 0)
			{
				return;
			}
			_isFlushingPendingStart = true;
			try
			{
				while (_pendingStart.Count > 0)
				{
					_pendingStart.Sort(CompareBehaviourStates);
					CopyBehaviours(_pendingStart, _pendingStartScratch);
					_pendingStart.Clear();
					_pendingStartSet.Clear();
					foreach (UnityBehaviourState item in _pendingStartScratch)
					{
						if (!item.Destroyed && !item.HasStarted && item.IsActiveInHierarchy)
						{
							InvokeMessage(item, UnityMessageType.Start);
							item.HasStarted = true;
						}
					}
					_pendingStartScratch.Clear();
				}
			}
			finally
			{
				_pendingStartScratch.Clear();
				_isFlushingPendingStart = false;
			}
		}
	}

	private void ProcessPendingDestroy()
	{
		using (_gate.EnterScope())
		{
			if (_pendingDestroy.Count != 0)
			{
				UnityObjectState[] array = _pendingDestroy.ToArray();
				_pendingDestroy.Clear();
				_pendingDestroySet.Clear();
				UnityObjectState[] array2 = array;
				foreach (UnityObjectState state in array2)
				{
					DestroyNow(state);
				}
			}
		}
	}

	private void DestroyNow(UnityObjectState state)
	{
		if (state is UnityGameObjectState gameObject)
		{
			DestroyNow(gameObject);
		}
		else if (state is UnityComponentState component)
		{
			DestroyNow(component);
		}
	}

	private void DestroyNow(UnityComponentState component)
	{
		if (component.Destroyed)
		{
			return;
		}
		if (component is UnityBehaviourState unityBehaviourState)
		{
			_pendingActivationSet.Remove(unityBehaviourState);
			_pendingStartSet.Remove(unityBehaviourState);
			if (unityBehaviourState.IsActiveInHierarchy)
			{
				unityBehaviourState.IsActiveInHierarchy = false;
				InvokeMessage(unityBehaviourState, UnityMessageType.OnDisable);
			}
			InvokeMessage(unityBehaviourState, UnityMessageType.OnDestroy);
			_behaviours.Remove(unityBehaviourState);
		}
		component.Destroyed = true;
		component.GameObject.Components.Remove(component);
		_objects.Remove(component.Instance, out var _);
		this.ObjectDestroyed?.Invoke(component.Instance);
	}

	private void DestroyNow(UnityGameObjectState gameObject)
	{
		if (!gameObject.Destroyed)
		{
			gameObject.Destroyed = true;
			UnityTransformState[] array = gameObject.Transform.Children.ToArray();
			foreach (UnityTransformState unityTransformState in array)
			{
				DestroyNow(unityTransformState.GameObject);
			}
			UnityComponentState[] array2 = gameObject.Components.ToArray();
			foreach (UnityComponentState component in array2)
			{
				DestroyNow(component);
			}
			if (gameObject.Transform.Parent != null)
			{
				gameObject.Transform.Parent.Children.Remove(gameObject.Transform);
			}
			else
			{
				_rootGameObjects.Remove(gameObject);
			}
			_objects.Remove(gameObject.Instance, out var _);
			this.ObjectDestroyed?.Invoke(gameObject.Instance);
		}
	}

	private void InvokeMessagesForActiveBehaviours(UnityMessageType messageType)
	{
		using (_gate.EnterScope())
		{
			CopyBehaviours(_behaviours, _behaviourDispatchScratch);
		}
		foreach (UnityBehaviourState item in _behaviourDispatchScratch)
		{
			if (item.IsActiveInHierarchy && !item.Destroyed)
			{
				InvokeMessage(item, messageType);
			}
		}
		_behaviourDispatchScratch.Clear();
	}

	public void DispatchOnGUI(bool repaint, Action<object>? beforeEach = null, Action<object>? afterEach = null)
	{
		using (_gate.EnterScope())
		{
			CopyBehaviours(_behaviours, _guiSnapshotScratch);
		}
		List<(int, int, UnityBehaviourState)> guiDispatchScratch = _guiDispatchScratch;
		for (int i = 0; i < _guiSnapshotScratch.Count; i++)
		{
			UnityBehaviourState unityBehaviourState = _guiSnapshotScratch[i];
			if (unityBehaviourState.Messages.OnGUI != null && unityBehaviourState.IsActiveInHierarchy && !unityBehaviourState.Destroyed)
			{
				guiDispatchScratch.Add((unityBehaviourState.GuiDepth, i, unityBehaviourState));
			}
		}
		_guiSnapshotScratch.Clear();
		guiDispatchScratch.Sort(((int Depth, int Order, UnityBehaviourState Behaviour) a, (int Depth, int Order, UnityBehaviourState Behaviour) b) => (a.Depth == b.Depth) ? a.Order.CompareTo(b.Order) : b.Depth.CompareTo(a.Depth));
		if (!repaint)
		{
			guiDispatchScratch.Reverse();
		}
		try
		{
			foreach (var item2 in guiDispatchScratch)
			{
				UnityBehaviourState item = item2.Item3;
				if (!item.Destroyed && item.IsActiveInHierarchy)
				{
					beforeEach?.Invoke(item.Instance);
					GuiDepth = item.GuiDepth;
					InvokeMessage(item, UnityMessageType.OnGUI);
					item.GuiDepth = GuiDepth;
					afterEach?.Invoke(item.Instance);
				}
			}
		}
		finally
		{
			guiDispatchScratch.Clear();
		}
	}

	private void InvokeShutdownMessages()
	{
		CopyBehaviours(_behaviours, _shutdownScratch);
		foreach (UnityBehaviourState item in _shutdownScratch)
		{
			if (!item.Destroyed)
			{
				InvokeMessage(item, UnityMessageType.OnApplicationQuit);
			}
		}
		_shutdownScratch.Clear();
	}

	private void InvokeMessage(UnityComponentState component, UnityMessageType messageType)
	{
		Func<object, object> func = component.Messages.Get(messageType);
		if (func == null)
		{
			return;
		}
		bool flag = FrameProfiler.Enabled && FrameProfiler.TryEnterScope(GetProfilerLabel(component));
		try
		{
			if (func(component.Instance) is IEnumerator arg)
			{
				MessageCoroutineStarter?.Invoke(component.Instance, arg);
			}
		}
		catch (Exception value)
		{
			LogCategory log = Log;
			bool shouldAppend;
			LogString<Error> message = new LogString<Error>(11, 3, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("[");
				message.AppendFormatted(component.Instance.GetType().FullName);
				message.AppendLiteral("] ");
				message.AppendFormatted(messageType);
				message.AppendLiteral(" threw: ");
				message.AppendFormatted(value);
			}
			log.Error(message, "InvokeMessage", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith.UnityCompat\\UnityWorld.cs", 1063);
		}
		finally
		{
			if (flag)
			{
				FrameProfiler.LeaveScope();
			}
		}
	}

	private static string GetProfilerLabel(UnityComponentState component)
	{
		return _profilerLabels.GetOrAdd(component.Instance.GetType(), (Type type) => type.Name);
	}

	private bool IsGameObjectActiveInHierarchy(UnityGameObjectState gameObject)
	{
		if (gameObject.Destroyed || !gameObject.ActiveSelf)
		{
			return false;
		}
		for (UnityTransformState parent = gameObject.Transform.Parent; parent != null; parent = parent.Parent)
		{
			if (!parent.GameObject.ActiveSelf || parent.GameObject.Destroyed)
			{
				return false;
			}
		}
		return true;
	}

	private bool IsComponentVisibleForFind(UnityComponentState component)
	{
		if (!(component is UnityBehaviourState { IsActiveInHierarchy: var isActiveInHierarchy }))
		{
			return IsGameObjectActiveInHierarchy(component.GameObject);
		}
		return isActiveInHierarchy;
	}

	private void SetSceneRecursive(UnityGameObjectState gameObject, string sceneName)
	{
		gameObject.SceneName = sceneName;
		foreach (UnityTransformState child in gameObject.Transform.Children)
		{
			SetSceneRecursive(child.GameObject, sceneName);
		}
	}

	private UnityGameObjectState GetGameObjectState(object instance)
	{
		if (_objects.TryGetValue(instance, out UnityObjectState value) && value is UnityGameObjectState result)
		{
			return result;
		}
		throw new InvalidOperationException("GameObject is not tracked by the Unity world.");
	}

	private UnityComponentState GetComponentState(object instance)
	{
		if (_objects.TryGetValue(instance, out UnityObjectState value) && value is UnityComponentState result)
		{
			return result;
		}
		throw new InvalidOperationException("Component is not tracked by the Unity world.");
	}

	private UnityBehaviourState GetBehaviourState(object instance)
	{
		if (_objects.TryGetValue(instance, out UnityObjectState value) && value is UnityBehaviourState result)
		{
			return result;
		}
		throw new InvalidOperationException("Behaviour is not tracked by the Unity world.");
	}

	private UnityTransformState GetTransformState(object instance)
	{
		if (_objects.TryGetValue(instance, out UnityObjectState value) && value is UnityTransformState result)
		{
			return result;
		}
		throw new InvalidOperationException("Transform is not tracked by the Unity world.");
	}

	private int GetExecutionOrder(UnityBehaviourState behaviour)
	{
		if (!_executionOrders.TryGetValue(behaviour.Instance.GetType(), out var value))
		{
			return 0;
		}
		return value;
	}

	private int CompareBehaviourStates(UnityBehaviourState left, UnityBehaviourState right)
	{
		int num = GetExecutionOrder(left).CompareTo(GetExecutionOrder(right));
		if (num != 0)
		{
			return num;
		}
		return left.CreationIndex.CompareTo(right.CreationIndex);
	}

	private void InsertBehaviourSorted(UnityBehaviourState behaviour)
	{
		int num = _behaviours.BinarySearch(behaviour, Comparer<UnityBehaviourState>.Create(CompareBehaviourStates));
		if (num < 0)
		{
			num = ~num;
		}
		_behaviours.Insert(num, behaviour);
	}

	private void SortBehavioursByExecutionOrder()
	{
		_behaviours.Sort(CompareBehaviourStates);
	}

	private static void CopyBehaviours(List<UnityBehaviourState> source, List<UnityBehaviourState> destination)
	{
		destination.Clear();
		destination.EnsureCapacity(source.Count);
		destination.AddRange(source);
	}

	private UnityMessageCache GetMessages(Type type)
	{
		if (_messageCache.TryGetValue(type, out var value))
		{
			return value;
		}
		value = new UnityMessageCache(FindMessage(type, "Awake"), FindMessage(type, "OnEnable"), FindMessage(type, "Start"), FindMessage(type, "Update"), FindMessage(type, "LateUpdate"), FindMessage(type, "OnDisable"), FindMessage(type, "OnApplicationQuit"), FindMessage(type, "OnDestroy"), FindMessage(type, "OnGUI"));
		_messageCache[type] = value;
		return value;
	}

	private static Func<object, object?>? FindMessage(Type type, string name)
	{
		Type type2 = type;
		while (type2 != null)
		{
			MethodInfo method = type2.GetMethod(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
			if (method != null)
			{
				return CompileMessage(method);
			}
			type2 = type2.BaseType;
		}
		return null;
	}

	private static Func<object, object?> CompileMessage(MethodInfo method)
	{
		ParameterExpression parameterExpression = Expression.Parameter(typeof(object), "instance");
		Expression expression = Expression.Call(Expression.Convert(parameterExpression, method.DeclaringType), method);
		return Expression.Lambda<Func<object, object>>((method.ReturnType == typeof(void)) ? ((Expression)Expression.Block(expression, Expression.Constant(null, typeof(object)))) : ((Expression)Expression.Convert(expression, typeof(object))), "InvokeMessageMethod_" + method.DeclaringType.FullName + "." + method.Name, new global::_003C_003Ez__ReadOnlySingleElementList<ParameterExpression>(parameterExpression)).Compile();
	}

	private static bool IsAssignableToUnityType(Type type, string fullName)
	{
		Type type2 = type;
		while (type2 != null)
		{
			if (type2.FullName == fullName)
			{
				return true;
			}
			type2 = type2.BaseType;
		}
		return false;
	}

	private int NextInstanceId()
	{
		return _nextInstanceId++;
	}

	private long NextCreationIndex()
	{
		return _nextCreationIndex++;
	}
}
