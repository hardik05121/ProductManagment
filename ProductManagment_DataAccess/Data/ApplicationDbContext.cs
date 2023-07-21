using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductManagment_Models.Models;
using System;

namespace ProductManagment_DataAccess.Data
{

    
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public virtual DbSet<Brand> Brands { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<City> Cities { get; set; }

        public virtual DbSet<Company> Companies { get; set; }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Expense> Expenses { get; set; }

        public virtual DbSet<ExpenseCategory> ExpenseCategories { get; set; }

        public virtual DbSet<Inquiry> Inquiries { get; set; }

        public virtual DbSet<InquirySource> InquirySources { get; set; }

        public virtual DbSet<InquiryStatus> InquiryStatuses { get; set; }

        public virtual DbSet<Inventory> Inventorys { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<PurChaseOrder> PurChaseOrders { get; set; }

        public virtual DbSet<PurChaseOrderXproduct> PurChaseOrderXproducts { get; set; }

        public virtual DbSet<Quotation> Quotations { get; set; }

        public virtual DbSet<QuotationXproduct> QuotationXproducts { get; set; }

        public virtual DbSet<State> States { get; set; }

        public virtual DbSet<Supplier> Suppliers { get; set; }

        public virtual DbSet<Tax> Taxs { get; set; }

        public virtual DbSet<Unit> Units { get; set; }

        public virtual DbSet<Warehouse> Warehouses { get; set; }
        public virtual DbSet<User> Users { get; set; }
     
    }
   
}
