using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextProcessor.Logic
{
    public class ProcessResult
    {
        public bool isSuccess = true;

        public List<(string Name, bool IsMet)> PreConditions { get; private set; } = new List<(string Name, bool IsMet)>();
        public List<(string Name, bool IsMet)> PostConditions { get; private set; } = new List<(string Name, bool IsMet)>();
        public string OutputText { get; set; } = string.Empty;

        public void AddPre(string Name, bool IsMet)
        {
            PreConditions.Add((Name, IsMet));
        }
        public void AddPost(string Name, bool IsMet)
        {
            PostConditions.Add((Name, IsMet));
        }
    }
}
