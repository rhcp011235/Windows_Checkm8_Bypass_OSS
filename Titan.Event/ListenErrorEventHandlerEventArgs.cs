using System;
using Titan.Enumerates;

namespace Titan.Event
{
	public class ListenErrorEventHandlerEventArgs : EventArgs
	{
		private string errorMessage;

		private ListenErrorEventType errorType;

		public string ErrorMessage => errorMessage;

		public ListenErrorEventType ErrorType => errorType;

		public ListenErrorEventHandlerEventArgs(string errorMessage, ListenErrorEventType errorType)
		{
			this.errorMessage = errorMessage;
			this.errorType = errorType;
		}
	}
}
