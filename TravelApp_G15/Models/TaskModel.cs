using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp_G15.Models
{
    public class TaskModel : INotifyPropertyChanged
    {
        public int TaskID { get; set; }
/*        public string Name { get; set; }
        public bool Checked { get; set; }*/

        public TaskModel()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChange([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private string name; 
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChange();
            }
        }

        private bool isCheck;
        public bool IsCheck
        {
            get
            {
                return isCheck;
            }
            set
            {
                isCheck = value;
                OnPropertyChange();
            }
        }
    }
}
