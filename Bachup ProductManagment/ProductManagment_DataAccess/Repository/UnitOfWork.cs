using ProductManagment_DataAccess.Data;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagment_DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public IBrandRepository Brand { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public ICityRepository City { get; private set; }
        public ICompanyRepository Company { get; private set; }
        public ICountryRepository Country { get; private set; }
        public ICustomerRepository Customer { get; private set; }
        public IProductRepository Product { get; private set; }
        public IStateRepository State { get; private set; }
        public ISupplierRepository Supplier { get; private set; }
        public ITaxRepository Tax { get; private set; }
        public IUnitRepository Unit { get; private set; }

        public IUserRepository User { get; private set; }

        public IWarehouseRepository Warehouse { get; private set; }
        public IInventoryRepository Inventory { get; private set; }
        public IInquiryRepository Inquiry { get; private set; }
        public IInquirySourceRepository InquirySource { get; private set; }
        public IInquiryStatusRepository InquiryStatus { get; private set; }
        public IExpenseCategoryRepository ExpenseCategory { get; private set; }
        public IExpenseRepository Expense { get; private set; }
        public IQuotationRepository Quotation { get; private set; }
        public IQuotationXproductRepository QuotationXproduct { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;

            Brand = new BrandRepository(_db);
            Category = new CategoryRepository(_db);
            City = new CityRepository(_db);
            Company = new CompanyRepository(_db);
            Country = new CountryRepository(_db);
            Customer = new CustomerRepository(_db);
            Product = new ProductRepository(_db);
            State = new StateRepository(_db);
            Supplier = new SupplierRepository(_db);
            Tax = new TaxRepository(_db);
            Unit = new UnitRepository(_db);
            User = new UserRepository(_db);
            Warehouse = new WarehouseRepository(_db);
            Inventory = new InventoryRepository(_db);
            Inquiry = new InquiryRepository(_db);
            InquirySource = new InquirySourceRepository(_db);
            InquiryStatus = new InquiryStatusRepository(_db);
            ExpenseCategory = new ExpenseCategoryRepository(_db);
            Expense = new ExpenseRepository(_db);
            Quotation = new QuotationRepository(_db);
            QuotationXproduct = new QuotationXproductRepository(_db);

        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
