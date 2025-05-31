using System.Collections.Generic;

namespace Core.Data.Models
{
    public class Stage
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Task> Tasks { get; set; }
    }
}