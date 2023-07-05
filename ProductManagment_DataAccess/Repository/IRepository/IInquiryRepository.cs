﻿using ProductManagment_Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagment_DataAccess.Repository.IRepository
{
    public interface IInquiryRepository : IRepository<InquiryMetadata>
    {
        void Update(InquiryMetadata obj);
    }
}
