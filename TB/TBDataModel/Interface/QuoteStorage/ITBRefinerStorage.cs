using System.Collections.Generic;
using TB.Common.DataModel.Quote.Struct;

namespace TB.Common.DataModel.Interface.QuoteStorage
{
    /// <summary>
    /// the interface that refiner used to CRUD storage
    /// includes function like
    ///    1. CRUD tick data
    ///    2. CRUD spec detail
    ///    3. CRUD kline
    ///    4. CRUD timesale
    ///    5. CRUD deallist
    /// </summary>
    public abstract class TBRefinerStorageBase
    {
        /// <summary>
        /// refiner pushes real time ticks to storage.
        /// </summary>
        /// <param name="ticks">ticks number will be 1+</param>
        public abstract void UpdateTicks(List<TBTickDeepLevel> ticks);
        /// <summary>
        /// refiner pushes real time spec detail to storage
        /// </summary>
        /// <param name="specDetails"></param>
        public abstract void UpdateSpecDetails(List<TBSpecDetail> specDetails);
    }
}
