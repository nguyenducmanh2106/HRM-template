
namespace SV.HRM.Models.Common
{
    public class QueryItem<T>
    {
        public T Key { get; set; }
        public int CompareExpression { get; set; }//-11: < | -10: <= | 0: = | 10 :>= | 11: > 0
        public QueryItem(int compareExpression, T key = default(T))
        {
            CompareExpression = compareExpression;
            Key = key;
        }
    }
}
