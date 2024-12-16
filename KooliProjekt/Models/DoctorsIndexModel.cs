using KooliProjekt.Data;

using KooliProjekt.Search;

namespace KooliProjekt.Models

{

    public class DoctorsIndexModel

    {

        public DoctorsSearch Search { get; set; }

        public PagedResult<Doctor> Data { get; set; }

    }

}

