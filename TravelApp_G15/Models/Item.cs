using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp_G15.Models
{
    public class Item : INotifyPropertyChanged
    {
        public int ItemID { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
       
        public Category Category { get; set; }

        public Item()
        {

        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChange([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        private bool isChecked;
        public bool Checked
        {
            get
            {
                return isChecked;
            }
            set
            {
                isChecked = value;
                OnPropertyChange();
            }
        }
    }
}
