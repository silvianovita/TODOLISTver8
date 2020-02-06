using Data.Base;
using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model
{
    public class ToDoList : BaseModel
    {
        public string Name { get; set; }
        public int Status { get; set; }
        public string userId { get; set; }
        public ToDoList() {  }
        public ToDoList(ToDoListVM toDoListVM)
        {
            this.Name= toDoListVM.Name;
            this.Status= toDoListVM.Status;
            this.userId = toDoListVM.userId;

            this.IsDelete = false;
            this.CreateDate = DateTimeOffset.Now;
        }
        public void Update(ToDoListVM toDoListVM)
        {
            this.Name = toDoListVM.Name;
            this.Status = toDoListVM.Status;

            this.UpdateDate = DateTimeOffset.Now;
        }
        public void Delete(ToDoListVM toDoListVM)
        {
            this.IsDelete = true;
            this.DeleteDate = DateTimeOffset.Now;
        }
    }
}
