using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagment_DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IBrandRepository Brand { get; }
        ICategoryRepository Category { get; }
        ICityRepository City { get; }
        ICompanyRepository Company { get; }
        ICountryRepository Country { get; }
        ICustomerRepository Customer { get; }
        IProductRepository Product { get; }
        IRoleRepository Role { get; }
        IStateRepository State { get; }
        ISupplierRepository Supplier { get; }
        ITaxRepository Tax { get; }
        IUnitRepository Unit { get; }
        IUserRepository User { get; }
        IUserRoleRepository UserRole { get; }
        IWarehouseRepository Warehouse { get; }
        IInventoryRepository Inventory { get; }
        IExpenseCategoryRepository ExpenseCategory { get; }
        IExpenseRepository Expense { get; }
        IInquirySourceRepository InquirySource { get; }
        IInquiryStatusRepository InquiryStatus { get; }
        IInquiryRepository Inquiry { get; }



        void Save();
    }
}
