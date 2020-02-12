using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModel
{
    public class ToDoListVM
    {
        public IEnumerable<ToDoListVM> data { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public string userId { get; set; }

        public int length { get; set; }
        public int filterLength { get; set; }
    }
}
