using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities
{
    public static class CommonEvents
    {
        /// <summary>
        /// Event args for an event which represents the change of selection of the passed object type.
        /// </summary>
        /// <typeparam name="TSelectedObject"></typeparam>
        public class SelectionChangedEventArgs<TSelectedObject> : EventArgs
        {
            public TSelectedObject SelectedObject = default(TSelectedObject);
            public SelectionChangedEventArgs(TSelectedObject newObject)
            {
                this.SelectedObject = newObject;
            }
        }

        /// <summary>
        /// Event arguments used by event propogators of error type messages
        /// </summary>
        public class ErrorEventArgs : EventArgs
        {
            public ErrorEventArgs(string errorMessage)
            {
                this.Errors.AddMessage(errorMessage);
            }

            public ErrorEventArgs(ValidationError errorMessage)
            {
                this.Errors.Add(errorMessage);
            }

            public ErrorEventArgs(ValidationErrorCollection errorMessages)
            {
                this.Errors = errorMessages;
            }

            public Indigo.CrossCutting.Utilities.ValidationErrorCollection Errors = new ValidationErrorCollection();
        }

        /// <summary>
        /// Event arguments used by event propogators of confirmation type messages
        /// </summary>
        public class ConfirmEventArgs : EventArgs
        {
            public ConfirmEventArgs(string message)
            {
                this.Message = message;
            }

            public string Message = "";
        }

        /// <summary>
        /// Event arguments used by event propogators of progress type messagess
        /// </summary>
        public class ProgressEventArgs : EventArgs
        {
            public long CurrentValue;
            public long MaximumValue;
            public string Comment;
            public ProgressEventArgs(long currentValue, long maxValue, string comment)
            {
                this.CurrentValue = currentValue;
                this.MaximumValue = maxValue;
                this.Comment = comment;
            }
        }

    }
}
