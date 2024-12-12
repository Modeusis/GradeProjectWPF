using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentsApplication.Utility
{
    public class StatusService
    {
        private static StatusService _instance;

        public static StatusService Instance => _instance ??= new StatusService();
        public string StatusText { get; set; }
        public double StatusOpacity { get; set; }

        public event Action StatusChanged;

        private StatusService()
        {
            StatusText = string.Empty;
            StatusOpacity = 0.0;
        }

        public void SetStatusMessage(string message)
        {
            StatusText = message;
            StatusOpacity = 1.0;
            StatusChanged?.Invoke(); 
        }
        public void ClearStatus()
        {
            StatusOpacity = 0.0;
            StatusChanged?.Invoke();
        }
    }

}
