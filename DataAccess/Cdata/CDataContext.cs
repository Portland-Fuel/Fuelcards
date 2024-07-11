using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Cdata;

public partial class CDataContext : DbContext
{
    public CDataContext()
    {
    }

    public CDataContext(DbContextOptions<CDataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccessTable> AccessTables { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Counterparty> Counterparties { get; set; }

    public virtual DbSet<CustomerContact> CustomerContacts { get; set; }

    public virtual DbSet<PortlandIdToXeroId> PortlandIdToXeroIds { get; set; }

    public virtual DbSet<Useraccess> Useraccesses { get; set; }

    public virtual DbSet<XeroCustomer> XeroCustomers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=78.32.37.42;Database=cdata;User Id=it;Password=nz2orJEzA5SeKqYFEG4ND88e&B8AGncV!*mq7fv^gz7%7TWZUQ;Port=59734");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("postgres_fdw");

        modelBuilder.Entity<AccessTable>(entity =>
        {
            entity.HasKey(e => e.UserType).HasName("access_table_pkey");

            entity.ToTable("access_table", tb => tb.HasComment("This table shows the user type that require access to the customer database"));

            entity.Property(e => e.UserType)
                .HasMaxLength(50)
                .HasColumnName("user_type");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("contacts_pkey");

            entity.ToTable("contacts");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Dateoptinforemail).HasColumnName("dateoptinforemail");
            entity.Property(e => e.Dateoptinforphone).HasColumnName("dateoptinforphone");
            entity.Property(e => e.Dateoptoutforemail).HasColumnName("dateoptoutforemail");
            entity.Property(e => e.Dateoptoutforphone).HasColumnName("dateoptoutforphone");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Fax)
                .HasMaxLength(35)
                .HasColumnName("fax");
            entity.Property(e => e.Firstname)
                .HasMaxLength(255)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(255)
                .HasColumnName("lastname");
            entity.Property(e => e.Mobile)
                .HasMaxLength(35)
                .HasColumnName("mobile");
            entity.Property(e => e.Optinemailsetid).HasColumnName("optinemailsetid");
            entity.Property(e => e.Optinforemail)
                .HasDefaultValue(false)
                .HasColumnName("optinforemail");
            entity.Property(e => e.Optinforphone)
                .HasDefaultValue(false)
                .HasColumnName("optinforphone");
            entity.Property(e => e.Optinphonesetid).HasColumnName("optinphonesetid");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(35)
                .HasColumnName("phone_number");
            entity.Property(e => e.Salutation)
                .HasMaxLength(50)
                .HasColumnName("salutation");
        });

        modelBuilder.Entity<Counterparty>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("counterparties_pkey");

            entity.ToTable("counterparties");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
        });

        modelBuilder.Entity<CustomerContact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("customer_contacts_pkey");

            entity.ToTable("customer_contacts");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Addtoemailsout).HasColumnName("addtoemailsout");
            entity.Property(e => e.Addtosettlementsemails).HasColumnName("addtosettlementsemails");
            entity.Property(e => e.ContactId).HasColumnName("contact_id");
            entity.Property(e => e.DateAdded).HasColumnName("date_added");
            entity.Property(e => e.Dateoptinformail).HasColumnName("dateoptinformail");
            entity.Property(e => e.Dateoptoutformail).HasColumnName("dateoptoutformail");
            entity.Property(e => e.Optinformail)
                .HasDefaultValue(false)
                .HasColumnName("optinformail");
            entity.Property(e => e.Optinformailsetid).HasColumnName("optinformailsetid");
            entity.Property(e => e.PortlandId).HasColumnName("portland_id");
            entity.Property(e => e.StaffId).HasColumnName("staff_id");
        });

        modelBuilder.Entity<PortlandIdToXeroId>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.XeroId }).HasName("portland_id_to_xero_id_pkey");

            entity.ToTable("portland_id_to_xero_id");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.XeroId)
                .HasColumnType("character varying")
                .HasColumnName("xero_id");
            entity.Property(e => e.PortlandId).HasColumnName("portland_id");
            entity.Property(e => e.XeroTennant).HasColumnName("xero_tennant");
        });

        modelBuilder.Entity<Useraccess>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("useraccess_pkey");

            entity.ToTable("useraccess");

            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
            entity.Property(e => e.Company)
                .HasMaxLength(150)
                .HasColumnName("company");
            entity.Property(e => e.DefaultAddOn)
                .HasDefaultValueSql("0")
                .HasColumnName("default_add_on");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(75)
                .HasColumnName("first_name");
            entity.Property(e => e.FuelcardsSession)
                .HasMaxLength(20)
                .HasColumnName("fuelcards_session");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.LastName)
                .HasMaxLength(75)
                .HasColumnName("last_name");
            entity.Property(e => e.MicrosoftId)
                .HasColumnType("character varying")
                .HasColumnName("microsoft_id");
            entity.Property(e => e.OnTrial).HasColumnName("on_trial");
            entity.Property(e => e.Password)
                .HasMaxLength(150)
                .HasColumnName("password");
            entity.Property(e => e.PricesSession)
                .HasMaxLength(20)
                .HasColumnName("prices_session");
            entity.Property(e => e.SessionId)
                .HasMaxLength(20)
                .HasColumnName("session_id");
            entity.Property(e => e.SignupDate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("signup_date");
            entity.Property(e => e.Telephone)
                .HasMaxLength(30)
                .HasColumnName("telephone");
            entity.Property(e => e.TradingSession)
                .HasMaxLength(20)
                .HasColumnName("trading_session");
            entity.Property(e => e.TrialendDate).HasColumnName("trialend_date");
        });

        modelBuilder.Entity<XeroCustomer>(entity =>
        {
            entity.HasKey(e => e.PortlandId).HasName("xero_customers_pkey");

            entity.ToTable("xero_customers");

            entity.Property(e => e.PortlandId)
                .HasDefaultValueSql("nextval('xero_portland_id_seq'::regclass)")
                .HasColumnName("portland_id");
            entity.Property(e => e.Accountnumber)
                .HasMaxLength(50)
                .HasColumnName("accountnumber");
            entity.Property(e => e.Accountspayabletaxcodename)
                .HasMaxLength(50)
                .HasColumnName("accountspayabletaxcodename");
            entity.Property(e => e.Accountsreceivabletaxcodename)
                .HasMaxLength(50)
                .HasColumnName("accountsreceivabletaxcodename");
            entity.Property(e => e.Bankaccountname)
                .HasMaxLength(255)
                .HasColumnName("bankaccountname");
            entity.Property(e => e.Bankaccountnumber)
                .HasMaxLength(50)
                .HasColumnName("bankaccountnumber");
            entity.Property(e => e.Bankaccountparticulars)
                .HasMaxLength(50)
                .HasColumnName("bankaccountparticulars");
            entity.Property(e => e.Brandingtheme)
                .HasMaxLength(255)
                .HasColumnName("brandingtheme");
            entity.Property(e => e.Companynumber)
                .HasMaxLength(50)
                .HasColumnName("companynumber");
            entity.Property(e => e.Contactid)
                .HasMaxLength(255)
                .HasColumnName("contactid");
            entity.Property(e => e.Contactname)
                .HasMaxLength(255)
                .HasColumnName("contactname");
            entity.Property(e => e.Ddinumber)
                .HasMaxLength(50)
                .HasColumnName("ddinumber");
            entity.Property(e => e.Defaulttaxbills)
                .HasMaxLength(255)
                .HasColumnName("defaulttaxbills");
            entity.Property(e => e.Defaulttaxsales)
                .HasMaxLength(255)
                .HasColumnName("defaulttaxsales");
            entity.Property(e => e.Discount)
                .HasMaxLength(50)
                .HasColumnName("discount");
            entity.Property(e => e.Duedatebillday).HasColumnName("duedatebillday");
            entity.Property(e => e.Duedatebillterm)
                .HasMaxLength(50)
                .HasColumnName("duedatebillterm");
            entity.Property(e => e.Duedatesalesday).HasColumnName("duedatesalesday");
            entity.Property(e => e.Duedatesalesterm)
                .HasMaxLength(50)
                .HasColumnName("duedatesalesterm");
            entity.Property(e => e.Emailaddress)
                .HasMaxLength(255)
                .HasColumnName("emailaddress");
            entity.Property(e => e.Faxnumber)
                .HasMaxLength(50)
                .HasColumnName("faxnumber");
            entity.Property(e => e.Firstname)
                .HasMaxLength(255)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(255)
                .HasColumnName("lastname");
            entity.Property(e => e.Legalname)
                .HasMaxLength(255)
                .HasColumnName("legalname");
            entity.Property(e => e.Mobilenumber)
                .HasMaxLength(50)
                .HasColumnName("mobilenumber");
            entity.Property(e => e.Person1email)
                .HasMaxLength(255)
                .HasColumnName("person1email");
            entity.Property(e => e.Person1firstname)
                .HasMaxLength(255)
                .HasColumnName("person1firstname");
            entity.Property(e => e.Person1secondname)
                .HasMaxLength(255)
                .HasColumnName("person1secondname");
            entity.Property(e => e.Person2email)
                .HasMaxLength(255)
                .HasColumnName("person2email");
            entity.Property(e => e.Person2firstname)
                .HasMaxLength(255)
                .HasColumnName("person2firstname");
            entity.Property(e => e.Person2secondname)
                .HasMaxLength(255)
                .HasColumnName("person2secondname");
            entity.Property(e => e.Person3email)
                .HasMaxLength(255)
                .HasColumnName("person3email");
            entity.Property(e => e.Person3firstname)
                .HasMaxLength(255)
                .HasColumnName("person3firstname");
            entity.Property(e => e.Person3secondname)
                .HasMaxLength(255)
                .HasColumnName("person3secondname");
            entity.Property(e => e.Person4email)
                .HasMaxLength(255)
                .HasColumnName("person4email");
            entity.Property(e => e.Person4firstname)
                .HasMaxLength(255)
                .HasColumnName("person4firstname");
            entity.Property(e => e.Person4secondname)
                .HasMaxLength(255)
                .HasColumnName("person4secondname");
            entity.Property(e => e.Person5email)
                .HasMaxLength(255)
                .HasColumnName("person5email");
            entity.Property(e => e.Person5firstname)
                .HasMaxLength(255)
                .HasColumnName("person5firstname");
            entity.Property(e => e.Person5secondname)
                .HasMaxLength(255)
                .HasColumnName("person5secondname");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(50)
                .HasColumnName("phonenumber");
            entity.Property(e => e.Poaddressline1)
                .HasMaxLength(500)
                .HasColumnName("poaddressline1");
            entity.Property(e => e.Poaddressline2)
                .HasMaxLength(500)
                .HasColumnName("poaddressline2");
            entity.Property(e => e.Poaddressline3)
                .HasMaxLength(500)
                .HasColumnName("poaddressline3");
            entity.Property(e => e.Poaddressline4)
                .HasMaxLength(500)
                .HasColumnName("poaddressline4");
            entity.Property(e => e.Poattentionto)
                .HasMaxLength(255)
                .HasColumnName("poattentionto");
            entity.Property(e => e.Pocity)
                .HasMaxLength(255)
                .HasColumnName("pocity");
            entity.Property(e => e.Pocountry)
                .HasMaxLength(50)
                .HasColumnName("pocountry");
            entity.Property(e => e.Popostalcode)
                .HasMaxLength(50)
                .HasColumnName("popostalcode");
            entity.Property(e => e.Poregion)
                .HasMaxLength(255)
                .HasColumnName("poregion");
            entity.Property(e => e.Purchasesaccount)
                .HasMaxLength(50)
                .HasColumnName("purchasesaccount");
            entity.Property(e => e.Purchasestrackingoption1)
                .HasMaxLength(50)
                .HasColumnName("purchasestrackingoption1");
            entity.Property(e => e.Purchasestrackingoption2)
                .HasMaxLength(50)
                .HasColumnName("purchasestrackingoption2");
            entity.Property(e => e.Saaddressline1)
                .HasMaxLength(500)
                .HasColumnName("saaddressline1");
            entity.Property(e => e.Saaddressline2)
                .HasMaxLength(500)
                .HasColumnName("saaddressline2");
            entity.Property(e => e.Saaddressline3)
                .HasMaxLength(500)
                .HasColumnName("saaddressline3");
            entity.Property(e => e.Saaddressline4)
                .HasMaxLength(500)
                .HasColumnName("saaddressline4");
            entity.Property(e => e.Saattentionto)
                .HasMaxLength(255)
                .HasColumnName("saattentionto");
            entity.Property(e => e.Sacity)
                .HasMaxLength(255)
                .HasColumnName("sacity");
            entity.Property(e => e.Sacountry)
                .HasMaxLength(50)
                .HasColumnName("sacountry");
            entity.Property(e => e.Salesaccount)
                .HasMaxLength(50)
                .HasColumnName("salesaccount");
            entity.Property(e => e.Salestrackingoption1)
                .HasMaxLength(50)
                .HasColumnName("salestrackingoption1");
            entity.Property(e => e.Salestrackingoption2)
                .HasMaxLength(50)
                .HasColumnName("salestrackingoption2");
            entity.Property(e => e.Sapostalcode)
                .HasMaxLength(50)
                .HasColumnName("sapostalcode");
            entity.Property(e => e.Saregion)
                .HasMaxLength(255)
                .HasColumnName("saregion");
            entity.Property(e => e.Skypename)
                .HasMaxLength(255)
                .HasColumnName("skypename");
            entity.Property(e => e.Taxnumber)
                .HasMaxLength(50)
                .HasColumnName("taxnumber");
            entity.Property(e => e.Trackingname1)
                .HasMaxLength(255)
                .HasColumnName("trackingname1");
            entity.Property(e => e.Trackingname2)
                .HasMaxLength(255)
                .HasColumnName("trackingname2");
            entity.Property(e => e.Website)
                .HasMaxLength(255)
                .HasColumnName("website");
        });
        modelBuilder.HasSequence("contacts_id_seq").HasMax(2147483647L);
        modelBuilder.HasSequence("useraccess_id_seq").HasMax(999999999L);
        modelBuilder.HasSequence("xero_portland_id_seq")
            .HasMin(100001L)
            .HasMax(2147483647L);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
