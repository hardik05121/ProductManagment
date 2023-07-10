using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagment_DataAccess.Repository.IRepository
{
    public interface IQuotationRepository : IRepository<Quotation>
    {
        void Update(Quotation obj);
    }
}
