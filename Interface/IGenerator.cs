// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;

namespace Flow
{
	/// <summary>
	///     Generator handler.
	/// </summary>
	public delegate void GeneratorHandler(IGenerator generator);

	/// <summary>
	///     A Generator does some work every time its Step method is called, unless it is Suspended or Completed.
	///     <para>All Generators are Resumed when they are first created by a Factory</para>
	/// </summary>
	public interface IGenerator : ITransient
	{
		/// <summary>
		///     Gets a value indicating whether this <see cref="Flow.IGenerator" /> is running.
		/// </summary>
		/// <value>
		///     <c>true</c> if running; otherwise, <c>false</c>.
		/// </value>
		bool Running { get; }

		/// <summary>
		///     Gets the step number; this increments each time a Generator is Stepped.
		/// </summary>
		/// <value>
		///     The step number.
		/// </value>
		int StepNumber { get; }

		/// <summary>
		///     Occurs when resumed.
		/// </summary>
		event GeneratorHandler Resumed;

		/// <summary>
		///     Occurs when stepped.
		/// </summary>
		event GeneratorHandler Stepped;

		/// <summary>
		///     Occurs when suspended.
		/// </summary>
		event GeneratorHandler Suspended;

		/// <summary>
		///     Resume this instance.
		/// </summary>
		void Resume();

		/// <summary>
		///     Step this Generator
		/// </summary>
		void Step();

		/// <summary>
		///     Called in a Step of the Kernel after all other generators have been Step'd by the Kernel.
		/// </summary>
		void Post();

		/// <summary>
		///     Suspend this instance. After this, Step() does nothing.
		/// </summary>
		void Suspend();

		/// <summary>
		///     Suspends this instance after another transient has been deleted
		/// </summary>
		/// <param name='other'>
		///     When the given transient is deleted, this instance will be suspended
		/// </param>
		void SuspendAfter(ITransient other);

		/// <summary>
		///     Resumes this instance after another transient has been deleted.
		/// </summary>
		/// <returns>
		///     True if the given transient exists.
		/// </returns>
		/// <param name='other'>
		///     When the given transient is deleted, this instance will be resumed.
		/// </param>
		bool ResumeAfter(ITransient other);

		/// <summary>
		///     Suspends this generator, and resumes it after a period of time.
		/// </summary>
		/// <returns>
		///     True
		/// </returns>
		/// <param name='span'>
		///     The length of time to pause the generator
		/// </param>
		bool ResumeAfter(TimeSpan span);

		/// <summary>
		///     Resumes this generator, and suspends it after a period of time
		/// </summary>
		/// <returns>
		///     True
		/// </returns>
		/// <param name='span'>
		///     The period of time to run the generator
		/// </param>
		bool SuspendAfter(TimeSpan span);
	}
}
