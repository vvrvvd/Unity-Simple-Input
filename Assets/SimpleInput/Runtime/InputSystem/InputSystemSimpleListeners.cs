using UnityEngine.InputSystem.Controls;

namespace SimpleInput.InputSystem
{
    using Vector2 = UnityEngine.Vector2;

    #region Button Control

    public class UnityButtonKeyInputListener : SimpleInputListener
    {
        private ButtonControl buttonInput;

        public UnityButtonKeyInputListener(ButtonControl buttonInput)
        {
            this.buttonInput = buttonInput;
        }

        public override bool IsPressed => buttonInput.wasPressedThisFrame;

        public override bool IsDown => buttonInput.isPressed;

        public override bool IsReleased => buttonInput.wasReleasedThisFrame;
    }

    public class UnityButtonInputListener_Axis : InputListener<float>
    {
        private ButtonControl positive;
        private ButtonControl negative;

        public UnityButtonInputListener_Axis(ButtonControl positive, ButtonControl negative) {
            this.positive = positive;
            this.negative = negative;
        }

        public override bool IsActive => positive.isPressed || negative.isPressed;

        public override float Value {
            get {
                var axisValue = positive.isPressed ? 1 : negative.isPressed ? -1 : 0;
                return axisValue;
            }
        }
    }

    public class UnityButtonInputListener_Vector2 : InputListener<Vector2>
    {
        private ButtonControl left;
        private ButtonControl right;
        private ButtonControl up;
        private ButtonControl down;

        public UnityButtonInputListener_Vector2(ButtonControl left, ButtonControl right, ButtonControl up, ButtonControl down)
        {
            this.left = left;
            this.right = right;
            this.up = up;
            this.down = down;
        }

        public override bool IsActive => left.isPressed || right.isPressed || up.isPressed || down.isPressed;

        public override Vector2 Value 
        {
            get 
            {
                var x = right.isPressed ? 1 : left.isPressed ? -1 : 0;
                var y = up.isPressed ? 1 : down.isPressed ? -1 : 0;
                return new Vector2(x, y);
            }
        }
    }

    #endregion

    #region Key Control

    public class UnityKeyInputListener : SimpleInputListener
    {
        private KeyControl keyInput;

        public UnityKeyInputListener(KeyControl keyInput)
        {
            this.keyInput = keyInput;
        }

        public override bool IsPressed => keyInput.wasPressedThisFrame;

        public override bool IsDown => keyInput.isPressed;

        public override bool IsReleased => keyInput.wasReleasedThisFrame;
    }

    public class UnityKeyInputListener_Axis : InputListener<float>
    {
        private KeyControl positive;
        private KeyControl negative;

        public UnityKeyInputListener_Axis(KeyControl positive, KeyControl negative) {
            this.positive = positive;
            this.negative = negative;
        }

        public override bool IsActive => positive.isPressed || negative.isPressed;

        public override float Value {
            get {
                var axisValue = positive.isPressed ? 1 : negative.isPressed ? -1 : 0;
                return axisValue;
            }
        }
    }

    public class UnityKeyInputListener_Vector2 : InputListener<Vector2>
    {

        private KeyControl left;
        private KeyControl right;
        private KeyControl up;
        private KeyControl down;

        public UnityKeyInputListener_Vector2(KeyControl up, KeyControl down, KeyControl left, KeyControl right)
        {
            this.left = left;
            this.right = right;
            this.up = up;
            this.down = down;
        }

        public override bool IsActive => left.isPressed || right.isPressed || up.isPressed || down.isPressed;

        public override Vector2 Value 
        {
            get 
            {
                var x = right.isPressed ? 1 : left.isPressed ? -1 : 0;
                var y = up.isPressed ? 1 : down.isPressed ? -1 : 0;
                return new Vector2(x, y);
            }
        }
    }

    #endregion

    #region Axis Control

    public class UnityControlInputListener_Axis : InputListener<float>
    {
        private AxisControl control;

        public UnityControlInputListener_Axis(AxisControl control)
        {
            this.control = control;
        }

        public override bool IsActive => control.ReadValue() != 0f;

        public override float Value 
        {
            get 
            {
                return control.ReadValue();
            }
        }
        
    }

    #endregion

    #region Vector2 Control

    public class UnityControlInputListener_Vector2 : InputListener<Vector2>
    {
        private float deadZone;
        private Vector2Control control;

        public UnityControlInputListener_Vector2(Vector2Control control, float deadZone = 0f)
        {
            this.control = control;
            this.deadZone = deadZone;
        }

        public override bool IsActive => control.ReadValue().magnitude > deadZone;

        public override Vector2 Value 
        {
            get 
            {
                return control.ReadValue();
            }
        }
    }

    #endregion

    #region InputAction Extensions

    public static class InputAction_UnityExtension
    {
        public static SimpleInputListener RegisterInputListener(this SimpleInputAction inputAction, ButtonControl buttonInput)
        {
            var inputListener = new UnityButtonKeyInputListener(buttonInput);
            inputAction.RegisterInputListener(inputListener);

            return inputListener;
        }

        public static SimpleInputListener RegisterInputListener(this SimpleInputAction inputAction, KeyControl keyInput)
        {
            var inputListener = new UnityButtonKeyInputListener(keyInput);
            inputAction.RegisterInputListener(inputListener);

            return inputListener;
        }
    }

    public static class InputActionGeneric_UnityExtension
    {

        public static InputListener<float> RegisterInputListener(this SimpleInputAction<float> inputAction, AxisControl control)
        {
			var inputListener = new UnityControlInputListener_Axis(control);
			inputAction.RegisterInputListener(inputListener);

            return inputListener;
        }

        public static InputListener<float> RegisterInputListener(this SimpleInputAction<float> inputAction, ButtonControl positive, ButtonControl negative) {
            var inputListener = new UnityButtonInputListener_Axis(positive, negative);
            inputAction.RegisterInputListener(inputListener);

            return inputListener;
        }

        public static InputListener<float> RegisterInputListener(this SimpleInputAction<float> inputAction, KeyControl positive, KeyControl negative) {
            var inputListener = new UnityKeyInputListener_Axis(positive, negative);
            inputAction.RegisterInputListener(inputListener);

            return inputListener;
        }

        public static InputListener<Vector2> RegisterInputListener(this SimpleInputAction<Vector2> inputAction, Vector2Control control, float deadZone = 0f)
        {
			var inputListener = new UnityControlInputListener_Vector2(control, deadZone);
			inputAction.RegisterInputListener(inputListener);

            return inputListener;
        }

        public static InputListener<Vector2> RegisterInputListener(this SimpleInputAction<Vector2> inputAction, ButtonControl up, ButtonControl down, ButtonControl left, ButtonControl right) {
            var inputListener = new UnityButtonInputListener_Vector2(up, down, left, right);
            inputAction.RegisterInputListener(inputListener);

            return inputListener;
        }

        public static InputListener<Vector2> RegisterInputListener(this SimpleInputAction<Vector2> inputAction, KeyControl up, KeyControl down, KeyControl left, KeyControl right) {
            var inputListener = new UnityKeyInputListener_Vector2(up, down, left, right);
            inputAction.RegisterInputListener(inputListener);

            return inputListener;
        }
    }
    #endregion
}
