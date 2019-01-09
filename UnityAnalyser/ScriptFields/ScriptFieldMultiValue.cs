using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{
    /// <summary>
    /// 表示多个值
    /// </summary>
    public class ScriptFieldMultiValue
    {
        public List<string> columnNameList;

        public List<Object> contentList;

        public ScriptFieldMultiValue()
        {
            columnNameList = new List<string>();
            contentList = new List<object>();
        }

        public void AddColumns(string[] columns)
        {
            columnNameList.AddRange(columns);
        }

        public void AddValue(object valuesOnRecord)
        {
            contentList.Add(valuesOnRecord);
        }
    }
}
