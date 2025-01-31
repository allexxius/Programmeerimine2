﻿using KooliProjekt.Data;

namespace KooliProjekt.Services

{

    public interface IDoctorService

    {

        Task<PagedResult<Doctor>> List(int page, int pageSize, Search.DoctorsSearch search);

        Task<Doctor> Get(int id);

        Task Save(Doctor list);

        Task Delete(int id);

    }

}

