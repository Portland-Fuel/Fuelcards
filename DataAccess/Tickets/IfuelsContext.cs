using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Tickets;

public partial class IfuelsContext : DbContext
{
    public IfuelsContext()
    {
    }

    public IfuelsContext(DbContextOptions<IfuelsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<IfuelsAddon> IfuelsAddons { get; set; }

    public virtual DbSet<IfuelsCustomer> IfuelsCustomers { get; set; }

    public virtual DbSet<IfuelsUser> IfuelsUsers { get; set; }

    public virtual DbSet<PaymentTerm> PaymentTerms { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=212.227.199.54;Database=Ifuels;User Id=postgres;Password=8gE&yhDGUe7cuYL;Port=5432");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IfuelsAddon>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ifuels_addons_pkey");

            entity.ToTable("ifuels_addons");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('ifuels_addons_id_seq1'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Addon).HasColumnName("addon");
            entity.Property(e => e.CustomerNumber).HasColumnName("customer_number");
            entity.Property(e => e.DateModified)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_modified");
            entity.Property(e => e.EffectiveFrom).HasColumnName("effective_from");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(20)
                .HasColumnName("ip_address");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.PaymentTerms).HasColumnName("payment_terms");
        });

        modelBuilder.Entity<IfuelsCustomer>(entity =>
        {
            entity.HasKey(e => e.CustomerNumber).HasName("ifuels_customers_pkey");

            entity.ToTable("ifuels_customers");

            entity.Property(e => e.CustomerNumber)
                .ValueGeneratedNever()
                .HasColumnName("customer_number");
            entity.Property(e => e.AddedBy)
                .HasColumnType("character varying")
                .HasColumnName("added_by");
            entity.Property(e => e.Banding)
                .HasMaxLength(100)
                .HasColumnName("banding");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(255)
                .HasColumnName("customer_name");
            entity.Property(e => e.DateAdded).HasColumnName("date_added");
            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .HasColumnName("type");
        });

        modelBuilder.Entity<IfuelsUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ifuels_users_pkey");

            entity.ToTable("ifuels_users");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('ifuels_users_id_seq1'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Admin).HasColumnName("admin");
            entity.Property(e => e.DateCreated).HasColumnName("date_created");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.EmailConfirmed).HasColumnName("email_confirmed");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.ForgotPasswordCode).HasColumnName("forgot_password_code");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.TempPasswordDate).HasColumnName("temp_password_date");
        });

        modelBuilder.Entity<PaymentTerm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("payment_terms_pkey");

            entity.ToTable("payment_terms");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('payment_terms_id_seq1'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.PaymentTerms).HasColumnName("payment_terms");
        });
        modelBuilder.HasSequence("ifuels_addons_id_seq");
        modelBuilder.HasSequence("ifuels_users_id_seq");
        modelBuilder.HasSequence("payment_terms_id_seq");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
