using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


namespace CasualKit.Factory
{
    public class CKFactory
    {
        static Dictionary<Type, Binding> _injectionMap = new Dictionary<Type, Binding>();

        public static void Bind<I, C>(BindingType bindingType)
        {
            _injectionMap[typeof(I)] = new Binding(typeof(C), bindingType);
        }

        public static void Bind<C>(BindingType bindingType)
        {
            _injectionMap[typeof(C)] = new Binding(null, bindingType);
        }

        public static void Inject(object sender)
        {
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic |
                                        BindingFlags.Static | BindingFlags.Instance;
            foreach (FieldInfo field in sender.GetType().GetFields(bindingFlags))
            {
                if (field.GetCustomAttribute<Inject>() != null)
                {
                    if (_injectionMap.ContainsKey(field.FieldType))
                    {
                        Binding binding = _injectionMap[field.FieldType];
                        if (binding._bindingType == BindingType.singletone)
                        {
                            Type toType = binding._toType != null ? binding._toType : field.FieldType;
                            if (binding._object == null)
                            {
                                if (toType.IsSubclassOf(typeof(MonoBehaviour)))
                                {
                                    binding._object = UnityEngine.Object.FindObjectOfType(toType);
                                    if (binding._object == null)
                                        binding._object = new GameObject("INJECTED: " + sender.GetType().ToString() + " -> " + toType.ToString()).AddComponent(toType);
                                }
                                else
                                {
                                    binding._object = Activator.CreateInstance(toType);
                                }
                            }
                            field.SetValue(sender, binding._object);
                        }
                        else if (binding._bindingType == BindingType.instance)
                        {
                            Type toType = binding._toType != null ? binding._toType : field.FieldType;
                            if (toType.IsSubclassOf(typeof(MonoBehaviour)))
                            {
                                binding._object = UnityEngine.Object.FindObjectOfType(toType);
                                if (binding._object == null)
                                    binding._object = new GameObject("INJECTED: " + sender.GetType().ToString() + " -> " + toType.ToString()).AddComponent(toType);
                                field.SetValue(sender, binding._object);
                            }
                            else
                            {
                                field.SetValue(sender, Activator.CreateInstance(toType));
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("Injection failed, noType: " + field.FieldType);
                        continue;
                    }
                }
            }
        }

        public static void Withdraw(object sender)
        {
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic |
                            BindingFlags.Static | BindingFlags.Instance;
            foreach (FieldInfo field in sender.GetType().GetFields(bindingFlags))
            {
                if (field.GetCustomAttribute<Inject>() != null)
                {
                    if (_injectionMap.ContainsKey(field.FieldType))
                    {
                        Binding binding = _injectionMap[field.FieldType];
                        if (binding._bindingType == BindingType.singletone)
                        {
                            Type toType = binding._toType != null ? binding._toType : field.FieldType;
                            if (toType.IsSubclassOf(typeof(MonoBehaviour)))
                            {
                                _injectionMap[field.FieldType]._object = null;
                            }
                        }
                    }
                }
            }
        }

        public static void ClearMap()
        {
            _injectionMap.Clear();
        }
    }

}