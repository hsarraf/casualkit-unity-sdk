using System;


namespace CasualKit.Factory
{
    public enum BindingType
    {
        instance,
        singletone,
    }

    public class Binding
    {
        public Binding(Type toType, BindingType bindingType)
        {
            (_toType, _bindingType) = (toType, bindingType);
        }
        public object _object;
        public Type _toType;
        public BindingType _bindingType;
    }

}