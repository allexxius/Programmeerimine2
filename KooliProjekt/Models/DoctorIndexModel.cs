using KooliProjekt.Data;

using KooliProjekt.Search;

namespace KooliProjekt.Models

{

    public class DoctorIndexModel

    {

        public DoctorSearch Search { get; set; }

        public PagedResult<Doctor> Data { get; set; }

    }

}

