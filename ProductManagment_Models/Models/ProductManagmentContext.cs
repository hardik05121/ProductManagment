using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace ProductManagment_Models.Models;

public partial class ProductManagmentContext : DbContext
{
    public ProductManagmentContext()
    {
    }

    public ProductManagmentContext(DbContextOptions<ProductManagmentContext> options)
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

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Tax> Taxs { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=HARDIK\\SQLEXPRESS;Database=ProductManagment;Trusted_Connection=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Brand");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_City");

            entity.HasOne(d => d.Country).WithMany(p => p.Cities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_City_Countries");

            entity.HasOne(d => d.State).WithMany(p => p.Cities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_City_States");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Country");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasOne(d => d.City).WithMany(p => p.Customers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Customers_Cities");

            entity.HasOne(d => d.Country).WithMany(p => p.Customers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Customers_Countries");

            entity.HasOne(d => d.State).WithMany(p => p.Customers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Customers_States");
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasOne(d => d.ExpenseCategory).WithMany(p => p.Expenses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Expenses_ExpenseCategories");
        });

        modelBuilder.Entity<ExpenseCategory>(entity =>
        {
            entity.Property(e => e.ExpenseCategoryName).IsFixedLength();
        });

        modelBuilder.Entity<Inquiry>(entity =>
        {
            entity.Property(e => e.Address).IsFixedLength();
            entity.Property(e => e.ContactPerson).IsFixedLength();
            entity.Property(e => e.Email).IsFixedLength();
            entity.Property(e => e.Message).IsFixedLength();
            entity.Property(e => e.Organization).IsFixedLength();
            entity.Property(e => e.Website).IsFixedLength();

            entity.HasOne(d => d.City).WithMany(p => p.Inquiries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inquiries_Cities");

            entity.HasOne(d => d.Country).WithMany(p => p.Inquiries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inquiries_Countries");

            entity.HasOne(d => d.InquirySource).WithMany(p => p.Inquiries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inquiries_InquirySources");

            entity.HasOne(d => d.InquiryStatus).WithMany(p => p.Inquiries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inquiries_InquiryStatuses");

            entity.HasOne(d => d.Product).WithMany(p => p.Inquiries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inquiries_Products");

            entity.HasOne(d => d.State).WithMany(p => p.Inquiries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inquiries_States");
        });

        modelBuilder.Entity<InquirySource>(entity =>
        {
            entity.Property(e => e.InquirySourceName).IsFixedLength();
        });

        modelBuilder.Entity<InquiryStatus>(entity =>
        {
            entity.Property(e => e.InquiryStatusName).IsFixedLength();
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasOne(d => d.Product).WithMany(p => p.Inventories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventorys_Inventorys");

            entity.HasOne(d => d.Unit).WithMany(p => p.Inventories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventorys_Inventorys1");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.Inventories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventorys_Inventorys2");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Brands");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Categories");

            entity.HasOne(d => d.Tax).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Taxs");

            entity.HasOne(d => d.Unit).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Units");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Warehouses1");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_State");

            entity.HasOne(d => d.Country).WithMany(p => p.States)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_State_Country");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasOne(d => d.BillingCity).WithMany(p => p.SupplierBillingCities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Suppliers_Cities1");

            entity.HasOne(d => d.BillingCountry).WithMany(p => p.SupplierBillingCountries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Suppliers_Countries1");

            entity.HasOne(d => d.BillingState).WithMany(p => p.SupplierBillingStates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Suppliers_States1");

            entity.HasOne(d => d.City).WithMany(p => p.SupplierCities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Suppliers_Cities");

            entity.HasOne(d => d.Country).WithMany(p => p.SupplierCountries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Suppliers_Countries");

            entity.HasOne(d => d.ShippingCity).WithMany(p => p.SupplierShippingCities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Suppliers_Cities2");

            entity.HasOne(d => d.ShippingCountry).WithMany(p => p.SupplierShippingCountries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Suppliers_Countries2");

            entity.HasOne(d => d.ShippingState).WithMany(p => p.SupplierShippingStates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Suppliers_States2");

            entity.HasOne(d => d.State).WithMany(p => p.SupplierStates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Suppliers_States");
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
