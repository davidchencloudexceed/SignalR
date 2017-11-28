namespace TB.Common.DataModel.Quote.Struct
{
    public struct TBSpecDetail
    {
        public bool enable;             // 记录是否有效(不能删除记录，只能失效它)
        public long updateTime;                // 记录的最后更新时间（副索引 秒）
        public TBSpecDetailValue value;
    }
}
