using System;
using System.Collections.Generic;

namespace Core.Data.Models
{
    [Serializable]
    public class Scenario
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Employee> Employees { get; set; }
        public List<Stage> Stages { get; set; }
    }
}