/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flow;
using NUnit.Framework;

namespace Flow.Test
{
	[TestFixture]
	public class TestMockGame : TestBase
	{
		// timout is always 1
		[TestCase(0.5f, false, 2.5f, false)] // timeout not trigger, first roll not avail
		[TestCase(1.5f, false, 0.5f, true)] // timeout not trigger, first roll avail
		[TestCase(0.5f, false, 0.3f, true)] // timeout not trigger
		[TestCase(1.5f, true, 999f, false)] // timeout trigger
		public void TestMockDiceRoll(float runTime, bool timedOut, float firstRollSecs, bool firstAvail)
		{
			_kernel.DebugLevel = EDebugLevel.Medium;

			// roll for player1
			var roll0 = _flow.Future<int>(); //.SetName("Roll0");

			// roll for player2
			var roll1 = _flow.Future<int>(); //.SetName("Roll1");

			// either player cancel
			var cancel = _flow.Future<bool>().SetName("Cancel");

			// global timeout
			var timeout = _flow.OneShotTimer(TimeSpan.FromSeconds(1)).SetName("OneShotTimer");

			// will complete when any of the given transients complete
			var trigger = _flow.Trigger(
				roll0, roll1, cancel, timeout
			).SetName("Trigger");

			// setup first time for kernel
			_kernel.Step();

			var firstRoll = _kernel.Time.Now + TimeSpan.FromSeconds(firstRollSecs);

			// NOTE we need to add the timeout, as well as the trigger adding the timeout
			_root.Add(
				timeout,
				trigger,
				_flow.Do(() =>
				{
					if (!roll0.Available && _kernel.Time.Now > firstRoll)
					{
						_kernel.Trace.Log("Rolling first die");
						roll0.Value = 0;
					}
				})
			);

			RunKernel(runTime);

			Assert.AreEqual(timedOut, !timeout.Active);
			Assert.AreEqual(firstAvail, roll0.Available);
		}
	}
}
*/
