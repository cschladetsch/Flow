using System;
using System.Linq;
using System.Text;

// There are a lot of accessess to properties that could return null, but won't
// ReSharper disable PossibleNullReferenceException

namespace Flow
{
    public static class Print
    {
        public static string Object(ITransient trans)
        {
            return PrettyPrinter.ToString(trans);
        }
    }

    public class PrettyPrinter
    {
        public int Indenting = 4;

        public static string ToString(ITransient trans)
        {
            return new PrettyPrinter(trans)._sb.ToString();
        }

        public PrettyPrinter(ITransient trans)
        {
            _sb.Append("# ");
            Print(trans, 0);
        }

        private int Print(ITransient trans, int level)
        {
            if (trans == null)
                return level;

            Lead(level);
            Header(trans);
            var group = trans as IGroup;
            return group == null ? level : Contents(group, level + 1);
        }

        private void Lead(int level)
        {
            _sb.Append(' ', level*Indenting);
        }

        bool ImplementsGenericInterface(Type given, Type iface)
        {
            return given.GetInterfaces().Any(x => x.IsGenericType &&
              x.GetGenericTypeDefinition() == iface);
        }

        private StringBuilder Header(ITransient trans)
        {
            var name = trans.Name ?? "";
            var tyName = trans.GetType().Name;
            var ty = trans.GetType();

            // test for generics
            if (ty.IsGenericType)
            {
                var arg = ty.GetGenericArguments()[0];
                if (ImplementsGenericInterface(ty, typeof(ITimedFuture<>)))
                {
                    var avail = (bool) ty.GetProperty("Available")?.GetValue(trans);
                    object val = "<unset>";
                    if (avail)
                        val = ty.GetProperty("Value")?.GetValue(trans);
                    return _sb.AppendFormat($"TimedFuture<{arg.Name}>: {name} Available={avail}, Value={val}\n");
                }
                if (ImplementsGenericInterface(ty, typeof(IFuture<>)))
                {
                    var avail = (bool) ty.GetProperty("Available")?.GetValue(trans);
                    object val = "<unset>";
                    if (avail)
                        val = ty.GetProperty("Value")?.GetValue(trans);
                    return _sb.AppendFormat($"Future<{arg.Name}>: {name} Available={avail}, Value={val}\n");
                }
                if (ImplementsGenericInterface(ty, typeof(IChannel<>)))
                {
                    return _sb.AppendFormat($"Channel<{arg.Name}>: {name}\n");
                }
            }

            if (typeof(ITimer).IsAssignableFrom(ty))
            {
                var timer = (ITimer) trans;
                var ends = timer.TimeEnds;
                return _sb.AppendFormat($"Timer: {name} ends in {(ends - timer.Kernel.Time.Now).TotalSeconds:N2}s, {GeneratorInfo(trans)}\n");
            }
            if (typeof(IPeriodic).IsAssignableFrom(ty))
            {
                var p = (IPeriodic) trans;
                return _sb.AppendFormat(
                    $"Periodic: {name}, started={p.TimeStarted}, interval={p.Interval}, remaining={p.TimeRemaining}: {GeneratorInfo(trans)}\n");
            }
            if (typeof(ITrigger).IsAssignableFrom(ty))
            {
                return _sb.AppendFormat($"Trigger: {name}: {GeneratorInfo(trans)}\n");
            }
            if (typeof(IBarrier).IsAssignableFrom(ty))
            {
                return _sb.AppendFormat($"Barrier: {name}: {GeneratorInfo(trans)}\n");
            }
            if (typeof(ISequence).IsAssignableFrom(ty))
            {
                return _sb.AppendFormat($"Sequence {name}: {GeneratorInfo(trans)}\n");
            }
            if (typeof(INode).IsAssignableFrom(ty))
            {
                return _sb.AppendFormat($"Node {name}: {GeneratorInfo(trans)}\n");
            }
            if (typeof(IGroup).IsAssignableFrom(ty))
            {
                return _sb.AppendFormat($"Group: {name}: {GeneratorInfo(trans)}\n");
            }
            if (typeof(ICoroutine).IsAssignableFrom(ty))
            {
                return _sb.AppendFormat($"Coroutine {name}: {GeneratorInfo(trans)}\n");
            }
            if (typeof(ITransient).IsAssignableFrom(ty))
                return _sb.AppendFormat($"{tyName}={name}:\n");

            return _sb.Append("??");
        }

        private int Contents(IGroup group, int level)
        {
            foreach (var tr in group.Contents)
            {
                Print(tr, level + 1);
                _sb.Append('\n');
            }
            return level;
        }

        string GeneratorInfo(ITransient trans)
        {
            var gen = trans as IGenerator;
            return gen == null ? "" : $"running={gen.Running}, step={gen.StepNumber}";
        }

        private readonly StringBuilder _sb = new StringBuilder();
    }
}
