using System;
using System.Collections.Generic;

namespace Manualfac
{
    class Disposer : Disposable
    {
        #region Please implements the following methods

        /*
         * The disposer is used for disposing all disposable items added when it is disposed.
         */

        private readonly Stack<IDisposable> disposables = new Stack<IDisposable>();

        public void AddItemsToDispose(object item)
        {
            var disposable = item as IDisposable;
            if (disposable != null)
            {
                disposables.Push(disposable);
            }
        }

        protected override void Dispose(bool disposing)
        {
            while (disposables.Count != 0)
            {
                var disposable = disposables.Pop();
                disposable.Dispose();
            }
        }

        #endregion
    }
}