using System.Collections.Generic;

namespace TB.Common.DataModel.Quote.Struct

{
    public struct TBTickDeepLevel
    {
        public TBCodeID codeId;                     // 代码信息
        public long time;                  // localtime时间
        public long pointCount;                // 小数位数
        public double preSettlePrice;        // 昨日结算价
        public double preClosePrice;     // 昨日收盘价
        public double open;                  // 今日开盘价
        public double last;                  // 最新价	
        public List<TBBidAsk> bidAsks;
        public double high;                  // 今日最高价
        public double low;                   // 今日最低价
        public double totalVol;              // 当日累计成交量
        public double preOpenInt;            // 昨持仓
        public double openInt;               // 持仓量
        public double avgPrice;              // 实时均价(今日结算价)
        public double volume;                // 最后一笔成交量
        public double turnOver;              // 总成交金额(单位：元)
        public double todayClose;            // 今日收盘价
        public double settlePrice;           // 当日结算价
        public double hisHigh;               // 历史最高价
        public double hisLow;                // 历史最低价
        public double upperLimit;            // 当天涨停板价
        public double lowerLimit;            // 当天跌停板价    
        public double insideVol;         // 内盘
        public double outsideVol;            // 外盘
        public double todayExitVol;          // 今日累计开仓
        public double todayEntryVol;     // 今日累计平仓
        public double tickChg;               // 本TICK与上一笔TICK的差值
        public long status;					// 状态
    }
}
