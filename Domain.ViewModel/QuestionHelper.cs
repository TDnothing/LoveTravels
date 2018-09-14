using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    /// <summary>
    /// 答题帮助
    /// </summary>
    public class QuestionHelper
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public List<QuestAnswer> Answer { get; set; }
    }

    /// <summary>
    /// 问题答案
    /// </summary>
    public class QuestAnswer
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Letter { get; set; }
        public string Summary { get; set; }
        public int Score { get; set; }
    }
}
