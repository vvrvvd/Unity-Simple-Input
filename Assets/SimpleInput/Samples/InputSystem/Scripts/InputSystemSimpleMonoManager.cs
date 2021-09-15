using UnityEngine;

namespace SimpleInput.InputSystem
{
    public class InputSystemSimpleMonoManager : MonoBehaviour
    {
        public InputSystemSimpleManager InputManager {get; private set;}

		public void Awake()
		{
			InputManager = new InputSystemSimpleManager();
		}

		public void Update()
		{
			InputManager.Update();
		}
	}
}
