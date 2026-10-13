namespace UnityEngine.InputSystem;

public static class InputActionSetupExtensions
{
	public struct BindingSyntax
	{
		private readonly InputAction _action;

		internal BindingSyntax(InputAction action)
		{
			_action = action;
		}
	}

	public struct CompositeSyntax
	{
		private readonly InputAction _action;

		private readonly int _bindingIndex;

		internal CompositeSyntax(InputAction action, int bindingIndex)
		{
			_action = action;
			_bindingIndex = bindingIndex;
		}

		public CompositeSyntax With(string name, string binding, string groups = null, string processors = null)
		{
			_action?.SetCompositeBindingPart(_bindingIndex, name, binding);
			return this;
		}
	}

	public static InputAction AddAction(this InputActionMap map, string name, InputActionType type = InputActionType.Value, string binding = null, string interactions = null, string processors = null, string groups = null, string expectedControlLayout = null)
	{
		InputAction inputAction = map.AddActionInternal(name, type);
		if (!string.IsNullOrEmpty(binding))
		{
			inputAction.AddBinding(binding);
		}
		return inputAction;
	}

	public static BindingSyntax AddBinding(this InputAction action, string path, string interactions = null, string processors = null, string groups = null)
	{
		action.AddBinding(path);
		return new BindingSyntax(action);
	}

	public static CompositeSyntax AddCompositeBinding(this InputAction action, string composite, string interactions = null, string processors = null)
	{
		return new CompositeSyntax(action, action.AddCompositeBinding(composite));
	}
}
