using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

namespace Dekuple.Registry
{
    using Model;

    /// <inheritdoc cref="ModelBase" />
    /// <summary>
    /// A mapping of Guid to Instance, and Guid to Type.
    /// Able to make a new instance given construction arguments
    /// that are called either on the ctor itself, or a method
    /// named 'Construct' that matches the passed creation method call.
    ///
    /// <b>NOTE</b> a Registry is a `Model` entirely so it has its own logging stream.
    /// It doesn't make much sense to use a Registry as an actual Model.
    /// 
    /// </summary>
    /// <typeparam name="TBase">The common interface for each instance stored in the registry</typeparam>
    public partial class Registry<TBase>
        : ModelBase
        , IRegistry<TBase>
        where TBase
            : class
            , IHasId
            , IHasRegistry<TBase>
            , IHasDestroyHandler<TBase>
    {
        public IEnumerable<TBase> Instances => _models.Values;
        public int NumInstances => _models.Count;

        private bool _resolved;
        private readonly List<PendingInjection> _pendingInjections = new List<PendingInjection>();
        private readonly Dictionary<Guid, TBase> _models = new Dictionary<Guid, TBase>();
        private readonly Dictionary<Guid, Type> _idToType = new Dictionary<Guid, Type>();
        private readonly Dictionary<Type, Guid> _typeToGuid = new Dictionary<Type, Guid>();
        private readonly Dictionary<Type, Type> _bindings = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, Injections> _preparers = new Dictionary<Type, Injections>();
        private readonly Dictionary<Type, TBase> _singles = new Dictionary<Type, TBase>();
        private IRegistry<TBase> _registry;

        public Registry()
            : base(null)
        {
            Verbosity = 2;
            ShowStack = false;
            ShowSource = true;
            LogSubject = this;
            LogPrefix = "Registry";
        }

        public bool Has(TBase instance)
        {
            return Instances.Contains(instance);
        }

        public bool Has(Guid id)
        {
            return Instances.Any(m => m.Id == id);
        }

        public TBase Get(Guid id)
        {
            if (_models.TryGetValue(id, out var model))
                return model;
            Warn($"Failed to find targetModel with id {id}");
            return null;
        }

        public bool Bind<TInterface, TImpl>()
            where TInterface
                : TBase
            where TImpl 
                : TInterface
        {
            var ity = typeof(TInterface);
            if (_bindings.ContainsKey(ity))
            {
                Warn($"Registry has already bound {ity} to {typeof(TImpl)}");
                return false;
            }

            // TODO: combine these to one lookup.
            // That is, put the TImpl into PrepareModel, and parameterise it.
            _bindings[ity] = typeof(TImpl);
            _preparers[ity] = new Injections(this, typeof(TImpl));

            return true;
        }

        public TIBase New<TIBase>(params object[] args)
            where TIBase
                : class, TBase, IHasRegistry<TBase>, IHasDestroyHandler<TBase>
        {
            var type = typeof(TIBase);

            if (GetSinglton(args, type, out TIBase singleton))
                return singleton;

            if (NewInstance(type, args) is TIBase instance)
                return StoreTypedIntance(instance, type);

            Error($"Failed to make or find instance for interface {type}");
            return null;
        }

        private bool GetSinglton<TIBase>(IReadOnlyCollection<object> args, Type type, out TIBase singleton)
            where TIBase
                : class, TBase, IHasRegistry<TBase>, IHasDestroyHandler<TBase>
        {
            singleton = null;
            var single = GetSingle(type);
            if (single == null) 
                return false;

            if (args.Count != 0)
                Error($"Attempt to get singleton {type}, when passing arguments {ToArgTypeList(args)}");

            var result = single as TIBase;
            if (result == null)
                Error($"Couldn't convert singleton {single.GetType()} to {type}");

            singleton = result;
            return true;
        }

        private TIBase StoreTypedIntance<TIBase>(TIBase iBase, Type type)
            where TIBase
                : class, TBase, IHasRegistry<TBase>, IHasDestroyHandler<TBase>
        {
            _models[iBase.Id] = iBase;
            if (!_typeToGuid.ContainsKey(type))
            {
                _idToType[iBase.Id] = type;
                _typeToGuid[type] = iBase.Id;
            }

            Verbose(10, $"Made a {typeof(TIBase)}");
            return iBase;
        }

        public bool Bind<TInterface, TImpl>(Func<TImpl> creator)
            where TInterface
                : TBase 
            where TImpl 
                : TInterface
        {
            throw new NotImplementedException();
        }

        public bool Bind<TInterface, TImpl, T0>(Func<T0, TImpl> creator)
            where TInterface
                : TBase 
            where TImpl 
                : TInterface
        {
            throw new NotImplementedException();
        }

        public bool Bind<TInterface, TImpl, T0, T1>(Func<T0, T1, TImpl> creator)
            where TInterface
                : TBase 
            where TImpl 
                : TInterface
        {
            throw new NotImplementedException();
        }

        public bool Bind<TInterface, TImpl>(TImpl single)
            where TInterface
                : TBase 
            where TImpl 
                : TInterface
        {
            var ity = typeof(TInterface);
            if (_singles.ContainsKey(ity))
            {
                Warn($"Already have singleton value for {ity}");
                return false;
            }

            var prep = new Injections(this, typeof(TImpl));
            _singles[ity] = Prepare(prep.Inject(single, ity, single));
            return true;
        }

        public bool Resolve()
        {
            if (_resolved)
            {
                Error("Registry already resolved");
                return false;
            }

            var pi = _pendingInjections.ToArray();
            AddPendingBindings(pi);
            ApplyInjections(pi);

            foreach (var p in _pendingInjections)
                Warn($"Failed to resolve for {p}");

            return _resolved = _pendingInjections.Count == 0;
        }

        public virtual TBase Prepare(TBase model)
        {
            Assert.IsNotNull(model);
            if (model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
                _models[model.Id] = model;
            }

            model.OnDestroyed += ModelDestroyed;
            model.Registry = this;
            return model;
        }

        private static string ToArgTypeList(IEnumerable<object> args)
        {
            if (args == null)
                return "";
            var result = "";
            var comma = "";
            foreach (var a in args)
            {
                result += comma;
                if (a == null)
                    result += "null";
                else
                    result += a.GetType().Name;
                comma = ", ";
            }

            return result;
        }

        private static string ToArgList(IEnumerable<object> args)
        {
            return args == null ? "" : string.Join(", ", args.Select(a => a.ToString()));
        }

        private void AddPendingBindings(IEnumerable<PendingInjection> pendingInjections)
        {
            foreach (var pi in pendingInjections)
            {
                if (pi.Single != null)
                {
                    Verbose(50, $"Setting delayed singleton for {pi.Interface}");
                    _singles[pi.Interface] = pi.Single;
                }
                else
                {
                    _bindings[pi.Interface] = pi.ModelType;
                }
            }
        }

        private void ApplyInjections(IEnumerable<PendingInjection> pendingInjections)
        {
            foreach (var pi in pendingInjections)
            {
                var inject = pi.Injection;
                var val = GetSingle(pi.Injection.ValueType);
                if (val == null)
                {
                    val = NewInstance(inject.ValueType, inject.Args);
                    if (val == null)
                    {
                        Error($"Failed to resolve deferred dependancy {pi}");
                        continue;
                    }
                }

                if (inject.PropertyInfo != null)
                    inject.PropertyInfo.SetValue(pi.TargetModel, val);
                else
                    inject.FieldInfo.SetValue(pi.TargetModel, val);

                _pendingInjections.Remove(pi);
            }
        }

        private void Remove(TBase model)
        {
            if (!_models.ContainsKey(model.Id))
                Warn($"Attempt to destroy unknown {model.GetType()} Id={model.Id}");
            else
                _models.Remove(model.Id);
        }

        private void ModelDestroyed<TIBase>(TIBase model)
            where TIBase
                : class
                , TBase
                , IHasRegistry<TBase>
                , IHasDestroyHandler<TBase>
        {
            if (model == null)
            {
                Verbose(10, "Attempt to destroy null model");
                return;
            }

            model.OnDestroyed -= ModelDestroyed;
            Remove(model);
        }

        private TBase GetSingle(Type ty)
        {
            return _singles.TryGetValue(ty, out var single) ? single : null;
        }

        internal TBase NewInstance(Type ity, object[] args)
        {
            if (!_bindings.TryGetValue(ity, out var ty))
            {
                if (_resolved)
                    Error($"Registry has no binding for {ity}");

                return null;
            }

            // find and invoke a matching ctor
            var cons = ty.GetConstructors();
            foreach (var con in cons)
            {
                if (!MatchingConstructor(args, con.GetParameters()))
                    continue;
                if (con.Invoke(args) is TBase model)
                    return Prepare(Prepare(ity, model));
            }

            // rather, search for a method called 'Construct'
            // this is mostly to allow for MonoBehaviour-based instances,
            // which cannot use ctors. In this case, any and all methods
            // named 'Construct' are treated as if they were ctors.
            foreach (var method in ty.GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                if (method.Name != "Construct")
                    continue;

                if (!MatchingConstructor(args, method.GetParameters()))
                    continue;

                if (!(Activator.CreateInstance(ty) is TBase model))
                {
                    Error($"Couldn't make a {ty} of type {typeof(TBase).Name}");
                    return null;
                }

                if ((bool) method.Invoke(model, args))
                    return Prepare(Prepare(ity, model));

                Error($"Create method failed for type {ty} with args {ToArgTypeList(args)}");
                return null;
            }

            Error($"No matching Ctor or 'Construct' method for {ty} with args '{ToArgTypeList(args)}'");
            return null;
        }

        private static bool MatchingConstructor(IReadOnlyList<object> args, IReadOnlyCollection<ParameterInfo> pars)
        {
            if (args == null)
                return pars.Count == 0;

            if (pars.Count != args.Count)
                return false;

            var n = 0;
            foreach (var param in pars.Select(p => p.ParameterType))
            {
                if (args[n] == null)
                {
                    ++n;
                    continue;
                }

                if (!param.IsAssignableFrom(args[n].GetType()))
                {
                    Debug.Log($"Cannot assign {args[n]} to {param}");
                    break;
                }

                ++n;
            }
            return n == args.Count;
        }

        protected TBase Prepare(Type ity, TBase model)
        {
            if (!_preparers.TryGetValue(ity, out var prep))
                throw new Exception($"No preparer for type {ity}");
            
            return prep.Inject(model);
        }

        public string Print()
        {
            var sb = new StringBuilder();
            sb.Append($"{_singles.Count} Singletons:\n");
            foreach (var s in _singles)
                sb.Append($"\t{s.Key} -> {s.Value}\n");

            sb.Append($"{NumInstances} Instances:\n");
            foreach (var kv in _models)
                sb.Append($"\t{kv.Value}\n");

            return sb.ToString();
        }
    }
}
