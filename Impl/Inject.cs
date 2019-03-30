using System;
using System.Reflection;

namespace Dekuple
{
    /// <inheritdoc />
    /// <summary>
    /// Used to inject a dependency as a field or method or property
    /// </summary>
    public class Inject : Attribute
    {
        public object[] Args;
        public PropertyInfo PropertyInfo;
        public FieldInfo FieldInfo;
        public Type ValueType;

        public Inject(params object[] args)
        {
            Args = args;
        }
    }
}
