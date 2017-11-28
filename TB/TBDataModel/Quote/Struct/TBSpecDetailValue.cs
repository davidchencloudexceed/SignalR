using TB.Common.DataModel.Quote.Enum;

namespace TB.Common.DataModel.Quote.Struct
{

    public struct TBSpecDetailValue                 //某一个品种的市场属性
    {
        public TBCodeID CodeId;                             // 品种代码
        public string strSymbolName;                  // 商品名称
        public string strSymbolDetail;                // 商品描述
        public string strDealTimes;                   // 交易时端，格式为： 09:00-10:15,10:30-11:30,13:30-14:15,14:30-15:00

        public bool bIsSeries;                         //是否是连续图
        public bool bEnable;                           //有效标记（如果有效，则客户端在请求代码链时将包括该记录）
        public bool bCanTrade;                         //是否可以交易（如果品种没有到期，则可以交易）


        public string strFormat;                      // 价格的格式化串     
        public TBMoneyMarkEnum CurrencyID;                  // 货币ID(交易结算货币)
        public string strCurrencySymbol;              // 货币符号(￥，$)
        public string strCurrencyName;                // 货币名称    
        public float fPriceScale;                        // 价格单位(1/100,1/1000)
        public int nDecDigits;                     // 小数点位数

        public int nMinMove;                       // 变动最小单位(1,20,50) 最小价格变动 = fPriceScale * nMinMove;
        public float fBigPointValue;                       // 每个整数点的价值
        public int nBidAskDepth;                   // 买卖盘个数
        public int nContractUnit;                  // 合约单位 期货中1张合约包含N吨铜，小麦等
        public int nMaxLimitShares;                // 单笔交易最大限量		
        public float fRiseLimitPrice;                  // 涨停板价格
        public float fFallLimitPrice;                  // 跌停板价格

        public int nTradeType;                     // 支持的交易类型  X X X X  2进值4位表示
                                                   // 依次分别为空头卖出，空头买入，多头卖出，多头买入
                                                   // 0000 - 不能交易
                                                   // 0001 - 可以多头买入
                                                   // 0010 - 可以多头卖出
                                                   // 0011 - 可以多头买入/卖出 。。。。。。
        public int nTradeSupport;                      // 支持的交易特性,用2进值位表示	
                                                       // bit0 - 是否允许市价指令
                                                       // bit1 - 是否允许获利止损指令
                                                       // bit2 - 是否允许挂单指令
                                                       // bit3 - 是否允许盘后指令
                                                       // ...

        public float fMarginRate;                      // 保证金比率(期货)
        public int nContractYear;                  // 合约年份
        public int nContractMonth;                 // 合约月份
        public string strExpiredDate;             // 到期日

        public int nMarginMode;                        // 保证金计算方式?
                                                       // 1 - EURUSD = Price * fInitialMargin * fLeverage
                                                       // 2 - USDJPY = 1     * fInitialMargin * fLeverage
                                                       // 3 - GBPJPY = (GBPUSD.MidPrice) * (GBPUSD.fInitialMargin) * fLeverage
                                                       // 4 - Future =  
                                                       // 5 - Stock

        public int nSpread;                            // 交易所世界标准时间偏移,nUTCOffset

        //////////////////////////////////////////////////////////////////////////
        // 以下为外汇专用字段
        public float fContract;                          // 合约的大小(外汇)
        public float fInitialMargin;                     // 合约的初始保证金
        public float fMaintenance;                     // 合约的维持保证金
        public string strRelativeSymbol;             // 交叉汇率的关联代码(nMarginMode = 3有效)
    };
}
