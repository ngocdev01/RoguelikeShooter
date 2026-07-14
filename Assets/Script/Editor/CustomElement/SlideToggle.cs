namespace NgocDev
{
    using NgocDev.Editor;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UIElements;

    [UxmlElement]
    public partial class SlideToggle : BaseField<bool>
    {
        public static readonly new string ussClassName = "slide-toggle";
        public static readonly new string inputUssClassName = "slide-toggle__input";
        public static readonly string inputKnobUssClassName = "slide-toggle__input-knob";
        public static readonly string inputCheckedUssClassName = "slide-toggle__input--checked";

        private VisualElement _input;
        private VisualElement _knob;


        public SlideToggle() : this(null)
        {
        }

        public SlideToggle(string label) : base(label, null)
        {
            AddToClassList(ussClassName);


            _input = this.Q(className: BaseField<bool>.inputUssClassName);
            _input.AddToClassList(inputUssClassName);


            _knob = new VisualElement();
            _knob.AddToClassList(inputKnobUssClassName);
            _input.Add(_knob);

            styleSheets.Add(CustomElement.LoadMainStyleSheet());


            RegisterCallback<ClickEvent>(OnClick);
            RegisterCallback<KeyDownEvent>(OnKeydownEvent);
            RegisterCallback<NavigationSubmitEvent>(OnSubmit);
        }

        private static void OnClick(ClickEvent evt)
        {
            if (evt.currentTarget is SlideToggle slideToggle) slideToggle.ToggleValue();
            evt.StopPropagation();
        }

        private static void OnSubmit(NavigationSubmitEvent evt)
        {
            if (evt.currentTarget is SlideToggle slideToggle) slideToggle.ToggleValue();
            evt.StopPropagation();
        }

        private static void OnKeydownEvent(KeyDownEvent evt)
        {
            if (evt.target is SlideToggle slideToggle)
            {
                if (slideToggle.panel.contextType == ContextType.Player)
                    return;
                if (evt.keyCode == KeyCode.KeypadEnter || evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.Space)
                {
                    slideToggle.ToggleValue();
                    evt.StopPropagation();
                }
            }
        }

        void ToggleValue()
        {
            value = !value;
        }


        public override void SetValueWithoutNotify(bool newValue)
        {
            base.SetValueWithoutNotify(newValue);
            _input.EnableInClassList(inputCheckedUssClassName, newValue);
        }
    }
}