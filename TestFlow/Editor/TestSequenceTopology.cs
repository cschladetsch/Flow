using NUnit.Framework;

namespace Flow.Test
{
	[TestFixture]
	public class TestSequenceTopology : TestBase
	{
		[Test()]
		public void TestSequence()
		{
			var stepNum = 0;
			_root.Add(
				_flow.Sequence(
					_flow.Do(() => stepNum += _kernel.StepNumber),
					_flow.Do(() => stepNum += _kernel.StepNumber),
					_flow.Do(() => stepNum += _kernel.StepNumber)
				)
			);
			Step(3);
			Assert.AreEqual(stepNum, 0 + 1 + 2);
		}

		[Test()]
		public void TestParallel()
		{
			var stepNum = 0;
			_root.Add(
				_flow.Parallel(
					_flow.Do(() => stepNum += _kernel.StepNumber),
					_flow.Do(() => stepNum += _kernel.StepNumber),
					_flow.Do(() => stepNum += _kernel.StepNumber)
				)
			);
			Step(3);
			Assert.AreEqual(stepNum, 0*3 + 1*3 + 2*3);
		}
	}
}
