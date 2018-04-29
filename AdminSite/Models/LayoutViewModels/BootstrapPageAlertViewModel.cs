using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicStoreDemo.AdminSite.Models.Enums;

namespace MusicStoreDemo.AdminSite.Models.LayoutViewModels
{
    public class BootstrapPageAlertViewModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public BootstrapAlertType AlertType { get; set; } = BootstrapAlertType.primary;
    }
}
