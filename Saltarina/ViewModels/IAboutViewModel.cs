using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saltarina.ViewModels
{
    public interface IAboutViewModel : INotifyPropertyChanged
    {
        string AboutBoxText { get; set; }
    }
}
