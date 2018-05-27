using Microsoft.AspNetCore.Mvc;
using MusicStoreDemo.AdminSite.Models.Enums;
using MusicStoreDemo.AdminSite.Models.LayoutViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreDemo.AdminSite
{
    public static class ControllerExtensions
    {
        public static void SetBootstrapPageAlert(this Controller ctrl, string title, string message, BootstrapAlertType alertType)
        {
            BootstrapPageAlertViewModel alertModel = new BootstrapPageAlertViewModel()
            {
                AlertType = alertType,
                Message = message,
                Title = title
            };
            ctrl.TempData.Add("pageAlertMessage", JsonConvert.SerializeObject(alertModel));
        }
        public static void SetBootstrapPageAlert(this Controller ctrl, string message, BootstrapAlertType alertType)
        {
            BootstrapPageAlertViewModel alertModel = new BootstrapPageAlertViewModel()
            {
                AlertType = alertType,
                Message = message
            };
            ctrl.TempData.Add("pageAlertMessage", JsonConvert.SerializeObject(alertModel));
        }
    }
}
