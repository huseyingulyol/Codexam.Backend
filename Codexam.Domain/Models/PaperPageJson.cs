using Codexam.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codexam.Domain.Models
{
    public class PaperPageJson
    {
        public string name { get; set; }
        public string student_number { get; set; }
        public AnswerJson[] answers { get; set; }
    }
}
