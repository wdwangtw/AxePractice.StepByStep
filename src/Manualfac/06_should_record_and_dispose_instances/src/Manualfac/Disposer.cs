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

        private readonly List<Disposable> disposables = new List<Disposable>();

        public void AddItemsToDispose(object item)
        {
            var disposable = item as Disposable;
            if (disposable != null)
            {
                disposables.Add(disposable);
            }
        }

        protected override void Dispose(bool disposing)
        {
            disposables.ForEach(d => d.Dispose());
        }

        #endregion
    }
}