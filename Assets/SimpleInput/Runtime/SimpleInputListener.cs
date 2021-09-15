namespace SimpleInput
{
    public abstract class SimpleInputListener
    {
        public InputEventType ActiveEventType { get; protected set; }
        
        public abstract bool IsPressed { get; }
        public abstract bool IsDown { get; }
        public abstract bool IsReleased { get; }

        public virtual bool RefreshEventType()
        {
            if(IsPressed){
                ActiveEventType = InputEventType.Pressed;
            }
            else if(IsDown){
                ActiveEventType = InputEventType.Down;
            }
            else if(IsReleased){
                ActiveEventType = InputEventType.Released;
            }

            return IsPressed || IsDown || IsReleased;
        }
    }

    public abstract class InputListener<T>
    {
        public abstract T Value
        {
            get;
        }
        public abstract bool IsActive
        {
            get;
        }
    }
}