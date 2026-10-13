using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem;

public interface IInputActionCollection : IEnumerable<InputAction>, IEnumerable
{
	InputBinding? bindingMask { get; set; }

	ReadOnlyArray<InputControlScheme> controlSchemes { get; }

	ReadOnlyArray<InputDevice>? devices { get; set; }

	bool Contains(InputAction action);

	void Disable();

	void Enable();
}
