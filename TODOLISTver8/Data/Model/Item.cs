using Data.Base;
using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model
{
    public class Item :BaseModel
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public int Price { get; set; }

        public Supplier supplier { get; set; }

        public Item() { }
        public Item(ItemVM itemVM)
        {
            this.Name = itemVM.Name;
            this.Stock = itemVM.Stock;
            this.Price = itemVM.Price;
        }

        public void Update(ItemVM itemVM)
        {
            this.Name = itemVM.Name;
            this.Stock = itemVM.Stock;
            this.Price = itemVM.Price;
            this.UpdateDate = DateTimeOffset.Now;
        }
        public void Delete()
        {
            this.IsDelete = true;
            this.DeleteDate = DateTimeOffset.Now;
        }
    }
}
