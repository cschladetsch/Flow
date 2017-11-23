// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;

namespace Flow
{
	public delegate void GeneratorHandler(IGenerator generator);

	/// <summary>
	/// A Generator does some work every time its Step method is called, unless it is Suspended or Completed.
	/// <para>All Generators are Resumed when they are first created by a Factory</para>
	/// </summary>
	public interface IGenerator : ITransient
	{
		event GeneratorHandler Resumed;
		event GeneratorHandler Stepped;
		event GeneratorHandler Suspended;

		bool Running { get; }
		int StepNumber { get; }
		object Value { get; }

		void Resume();
		void Pre();
		void Step();
		void Post();
		void Suspend();

		IGenerator SuspendAfter(ITransient other);
		IGenerator SuspendAfter(TimeSpan span);
		IGenerator ResumeAfter(ITransient other);
		IGenerator ResumeAfter(TimeSpan span);
	}

	public delegate void GeneratorHandler<in T>(IGenerator<T> generator);

	public interface IGenerator<out T> : IGenerator
	{
		new T Value { get; }
	}
}
