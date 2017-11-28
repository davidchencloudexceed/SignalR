namespace TB.Common.DataModel.Quote.Struct

{
    public struct TBTimeSale    // 分笔成交
    {
        public long time;                  // 成交时间
        public double price;             // 成交价格
        public double avgPrice;				// 均价           
        public double volume;                // 该笔成交量
        public double openInt;               // 持仓量
        public double openIntChg;            // 持仓增量
        public int insideout;              // 内外盘标志,1-外盘，-1-内盘，0-不明盘
    };
}
