namespace TB.Common.DataModel.Quote.Struct

{
    public struct TBKLine
    {
        public long time;          // 毫秒
        public double close;
        public double open;
        public double high;
        public double low;
        public long vol;
        public long openint;       // 持仓量
    };
}
