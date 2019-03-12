using System;
using System.Collections.Generic;
using System.Reflection;

namespace Dekuple.Registry
{
    partial class Registry<TBase>
    {
        /// <summary>
        /// Represents an actual injection to a set of values and/or properties to target object.
        /// </summary>
        private class Injections
        {
            private readonly IRegistry<TBase> _reg;
            private readonly List<Inject> _injections = new List<Inject>();

            private static BindingFlags Flags =>
                BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            internal Injections(IRegistry<TBase> reg, Type ty)
            {
                _reg = reg;
                AddPropertyInjections(ty);
                AddFieldInjections(ty);
            }

            private void AddFieldInjections(Type ty)
            {
                foreach (var field in ty.GetFields(Flags))
                {
                    var inject = field.GetCustomAttribute<Inject>();
                    if (inject == null)
                        continue;
                    inject.FieldInfo = field;
                    inject.ValueType = field.FieldType;
                    _injections.Add(inject);
                }
            }

            private void AddPropertyInjections(Type ty)
            {
                foreach (var prop in ty.GetProperties(Flags))
                {
                    var inject = prop.GetCustomAttribute<Inject>();
                    if (inject == null)
                        continue;
                    inject.PropertyInfo = prop;
                    inject.ValueType = prop.PropertyType;
                    _injections.Add(inject);
                }
            }

            /// <summary>
            /// Perform the injection
            /// </summary>
            /// <returns>What was injected</returns>
            public TBase Inject(TBase model, Type iface = null, TBase single = null)
            {
                model.Registry = _reg;
                foreach (var inject in _injections)
                    _reg.Inject(model, inject, iface, single);

                return model;
            }
        }

        /// <summary>
        /// Used to postpone depdancy injection to avoid cyclic dependancy issues
        /// </summary>
        private class PendingInjection
        {
            internal readonly TBase TargetModel;
            internal readonly Inject Injection;
            internal readonly TBase Single;
            internal readonly Type Interface;
            internal readonly Type ModelType;

            public PendingInjection(TBase targetModel, Inject inject, Type modelType, Type iface = null,
                TBase single = null)
            {
                TargetModel = targetModel;
                Injection = inject;
                ModelType = modelType;
                Interface = iface;
                Single = single;
            }

            public override string ToString()
            {
                return $"PendingInject: {Injection.ValueType} into {TargetModel}";
            }
        }

        public TBase Inject(TBase model, Inject inject, Type iface, TBase single)
        {
            var val = GetSingle(inject.ValueType);
            if (val == null)
            {
                val = NewInstance(inject.ValueType, inject.Args);
                switch (val)
                {
                    case null when _resolved:
                        Error($"Cannot resolve interface {inject.ValueType}");
                        return null;
                    case null:
                        var pi = new PendingInjection(model, inject, model.GetType(), iface, single);
                        Verbose(30, $"Adding {pi}");
                        _pendingInjections.Add(pi);
                        break;
                }
            }

            if (inject.PropertyInfo != null)
                inject.PropertyInfo.SetValue(model, val);
            else
                inject.FieldInfo.SetValue(model, val);

            return model;
        }
    }
}
