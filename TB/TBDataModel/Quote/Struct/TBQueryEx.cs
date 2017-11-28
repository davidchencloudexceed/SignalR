namespace TB.Common.DataModel.Quote.Struct
{
    public struct TBQueryEx
    {
        public bool isSeries;      // 是否请求连续图
        char byReqType;         // 请求类型 
                                // 0表示请求最近多少个单位的数据
                                // 1表示请求所有的数据(下载)
                                // 2表示请求某个时间以前(包括该时间)多少个单位的数据(dwSubscribeNum为0时表示该时间前所有的数据) 
                                // 3表示请求某个时间以后多少个单位的数据(dwSubscribeNum为0时表示该时间后所有的数据)
                                // 4表示请求一个时间段的数据
        public long dwSubscribeNum;     // 订阅数据的数量(请求类型为0、2、3时使用)
        public long tTime;             // 时间(请求类型为2、3时使用)(毫秒)
        public TBTimePair timePair;				// 时间段(请求类型为4时使用)
    }
}
