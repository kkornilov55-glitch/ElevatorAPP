using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator.Logic
{
    public class ProcessResult
    {
        public bool isSuccess = true;

        private List<(string Name, bool IsMet)> PreConditions = new List<(string Name, bool IsMet)>();
        private List<(string Name, bool IsMet)> PostConditions = new List<(string Name, bool IsMet)>();
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
