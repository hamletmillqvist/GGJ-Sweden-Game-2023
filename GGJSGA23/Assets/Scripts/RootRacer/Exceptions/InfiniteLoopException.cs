using System;

namespace RootRacer.Exceptions
{
	public class InfiniteLoopException : Exception
	{
		public InfiniteLoopException() : base(
			"The program has entered a state of infinite looping and was forced to shut down!")
		{ }
	}
}