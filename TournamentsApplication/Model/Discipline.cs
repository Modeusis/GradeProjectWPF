using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentsApplication.Utility;

namespace TournamentsApplication.Model
{
    public class Discipline : ViewModelBase
    {
        private int discipline_id;
        private string discipline_name;
        private string discription;
        private DateTime created_at;
        private DateTime? updated_at;

        public int DisciplineId
        {
            get { return discipline_id; }
            set { discipline_id = value; OnPropertyChanged(nameof(DisciplineId)); }
        }
        public string DisciplineName
        {
            get { return discipline_name; }
            set { discipline_name = value; OnPropertyChanged(nameof(DisciplineName)); }
        }
        public string Description
        {
            get { return discription; }
            set { discription = value; OnPropertyChanged(nameof(Description)); }
        }
        public DateTime CreatedAt
        {
            get { return created_at; }
            set { created_at = value; OnPropertyChanged(nameof(CreatedAt)); }
        }
        public DateTime? UpdatedAt
        {
            get { return updated_at; }
            set { updated_at = value; OnPropertyChanged(nameof(UpdatedAt)); }
        }

        public virtual ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
    }
}
