using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Fuelcards;

public partial class FuelcardsContext : DbContext
{
    public FuelcardsContext()
    {
    }

    public FuelcardsContext(DbContextOptions<FuelcardsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AllocatedVolume> AllocatedVolumes { get; set; }

    public virtual DbSet<AllocatedVolumeTemp> AllocatedVolumeTemps { get; set; }

    public virtual DbSet<AppTask> AppTasks { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<ConsolidatedTexacoCard> ConsolidatedTexacoCards { get; set; }

    public virtual DbSet<CustomerMultipleAccountSameNetwork> CustomerMultipleAccountSameNetworks { get; set; }

    public virtual DbSet<CustomerPricingAddon> CustomerPricingAddons { get; set; }

    public virtual DbSet<FcControl> FcControls { get; set; }

    public virtual DbSet<FcEmail> FcEmails { get; set; }

    public virtual DbSet<FcGrade> FcGrades { get; set; }

    public virtual DbSet<FcHiddenCard> FcHiddenCards { get; set; }

    public virtual DbSet<FcIntroducersEmail> FcIntroducersEmails { get; set; }

    public virtual DbSet<FcMaskedCard> FcMaskedCards { get; set; }

    public virtual DbSet<FcNetwork> FcNetworks { get; set; }

    public virtual DbSet<FcNetworkAccNoToPortlandId> FcNetworkAccNoToPortlandIds { get; set; }

    public virtual DbSet<FcProspect> FcProspects { get; set; }

    public virtual DbSet<FcProspectsDetail> FcProspectsDetails { get; set; }

    public virtual DbSet<FcRequiredEdiReport> FcRequiredEdiReports { get; set; }

    public virtual DbSet<FcReseller> FcResellers { get; set; }

    public virtual DbSet<FgCard> FgCards { get; set; }

    public virtual DbSet<FgCardSwipe> FgCardSwipes { get; set; }

    public virtual DbSet<FgCardstatus> FgCardstatuses { get; set; }

    public virtual DbSet<FgRequestLog> FgRequestLogs { get; set; }

    public virtual DbSet<FgTransaction> FgTransactions { get; set; }

    public virtual DbSet<FgTransactionsTest> FgTransactionsTests { get; set; }

    public virtual DbSet<FixAllocationDate> FixAllocationDates { get; set; }

    public virtual DbSet<FixFrequency> FixFrequencies { get; set; }

    public virtual DbSet<FixedPriceContract> FixedPriceContracts { get; set; }

    public virtual DbSet<Fuelcard> Fuelcards { get; set; }

    public virtual DbSet<FuelcardBasePrice> FuelcardBasePrices { get; set; }

    public virtual DbSet<FuelcardPrice> FuelcardPrices { get; set; }

    public virtual DbSet<InvoiceFormatGroup> InvoiceFormatGroups { get; set; }

    public virtual DbSet<InvoiceNumber> InvoiceNumbers { get; set; }

    public virtual DbSet<InvoiceReport> InvoiceReports { get; set; }

    public virtual DbSet<InvoiceSent> InvoiceSents { get; set; }

    public virtual DbSet<InvoicingOption> InvoicingOptions { get; set; }

    public virtual DbSet<KeyFuelsCard> KeyFuelsCards { get; set; }

    public virtual DbSet<KfCard> KfCards { get; set; }

    public virtual DbSet<KfE11Product> KfE11Products { get; set; }

    public virtual DbSet<KfE19Card> KfE19Cards { get; set; }

    public virtual DbSet<KfE1E3Transaction> KfE1E3Transactions { get; set; }

    public virtual DbSet<KfE20StoppedCard> KfE20StoppedCards { get; set; }

    public virtual DbSet<KfE21Account> KfE21Accounts { get; set; }

    public virtual DbSet<KfE22AccountsStopped> KfE22AccountsStoppeds { get; set; }

    public virtual DbSet<KfE23NewClosedSite> KfE23NewClosedSites { get; set; }

    public virtual DbSet<KfE2Delivery> KfE2Deliveries { get; set; }

    public virtual DbSet<KfE4SundrySale> KfE4SundrySales { get; set; }

    public virtual DbSet<KfE5Stock> KfE5Stocks { get; set; }

    public virtual DbSet<KfE6Transfer> KfE6Transfers { get; set; }

    public virtual DbSet<MissingProductValue> MissingProductValues { get; set; }

    public virtual DbSet<OpsCustomer> OpsCustomers { get; set; }

    public virtual DbSet<OpsOrder> OpsOrders { get; set; }

    public virtual DbSet<PaymentTerm> PaymentTerms { get; set; }

    public virtual DbSet<PortlandFuelcard> PortlandFuelcards { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductDescriptionToInventoryItemCode> ProductDescriptionToInventoryItemCodes { get; set; }

    public virtual DbSet<RolledVolume> RolledVolumes { get; set; }

    public virtual DbSet<SandwichOrder> SandwichOrders { get; set; }

    public virtual DbSet<SiteBandInfo> SiteBandInfos { get; set; }

    public virtual DbSet<SiteNumberToBand> SiteNumberToBands { get; set; }

    public virtual DbSet<TexacoCard> TexacoCards { get; set; }

    public virtual DbSet<TexacoControl> TexacoControls { get; set; }

    public virtual DbSet<TexacoEdi> TexacoEdis { get; set; }

    public virtual DbSet<TexacoTransaction> TexacoTransactions { get; set; }

    public virtual DbSet<TransactionSiteSurcharge> TransactionSiteSurcharges { get; set; }

    public virtual DbSet<TransactionalSite> TransactionalSites { get; set; }

    public virtual DbSet<UkfTransaction> UkfTransactions { get; set; }

    public virtual DbSet<UkfuelCard> UkfuelCards { get; set; }

    public virtual DbSet<UkfuelTransaction> UkfuelTransactions { get; set; }

    public virtual DbSet<XeroCustomer> XeroCustomers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=78.32.37.42;Database=fuelcards;User Id=it;Password=nz2orJEzA5SeKqYFEG4ND88e&B8AGncV!*mq7fv^gz7%7TWZUQ;Port=59734");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("postgres_fdw");

        modelBuilder.Entity<AllocatedVolume>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("allocated_volume_pkey");

            entity.ToTable("allocated_volume");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AllocationId).HasColumnName("allocation_id");
            entity.Property(e => e.TradeId).HasColumnName("trade_id");
            entity.Property(e => e.Volume).HasColumnName("volume");

            entity.HasOne(d => d.Allocation).WithMany(p => p.AllocatedVolumes)
                .HasForeignKey(d => d.AllocationId)
                .HasConstraintName("allocation_id");

            entity.HasOne(d => d.Trade).WithMany(p => p.AllocatedVolumes)
                .HasForeignKey(d => d.TradeId)
                .HasConstraintName("trade_id");
        });

        modelBuilder.Entity<AllocatedVolumeTemp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("allocated_volume_temp_pkey");

            entity.ToTable("allocated_volume_temp");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AllocationId).HasColumnName("allocation_id");
            entity.Property(e => e.TradeId).HasColumnName("trade_id");
            entity.Property(e => e.Volume).HasColumnName("volume");
        });

        modelBuilder.Entity<AppTask>(entity =>
        {
            entity.Property(e => e.CreationTime).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Description).HasMaxLength(2400);
            entity.Property(e => e.Title).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<ConsolidatedTexacoCard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CollatedCards_pkey");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AccountNumber).HasColumnName("account_number");
            entity.Property(e => e.CustomerName)
                .HasColumnType("character varying")
                .HasColumnName("customer_name");
            entity.Property(e => e.PortlandId).HasColumnName("portland_id");
        });

        modelBuilder.Entity<CustomerMultipleAccountSameNetwork>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("customer_multiple_account_same_network_pkey");

            entity.ToTable("customer_multiple_account_same_network");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FcAccount).HasColumnName("fc_account");
            entity.Property(e => e.Network).HasColumnName("network");
            entity.Property(e => e.PortlandId).HasColumnName("portland_id");
        });

        modelBuilder.Entity<CustomerPricingAddon>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("customer_pricing_addon_pkey");

            entity.ToTable("customer_pricing_addon");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Addon).HasColumnName("addon");
            entity.Property(e => e.EffectiveDate).HasColumnName("effective_date");
            entity.Property(e => e.Network).HasColumnName("network");
            entity.Property(e => e.PortlandId).HasColumnName("portland_id");
        });

        modelBuilder.Entity<FcControl>(entity =>
        {
            entity.HasKey(e => e.ControlId).HasName("kf_control_pkey");

            entity.ToTable("fc_control");

            entity.Property(e => e.ControlId)
                .HasDefaultValueSql("nextval('kf_control_control_id_seq'::regclass)")
                .HasColumnName("control_id");
            entity.Property(e => e.BatchNumber).HasColumnName("batch_number");
            entity.Property(e => e.CostSign)
                .HasMaxLength(1)
                .HasColumnName("cost_sign");
            entity.Property(e => e.CreationDate).HasColumnName("creation_date");
            entity.Property(e => e.CreationTime).HasColumnName("creation_time");
            entity.Property(e => e.CustomerAc).HasColumnName("customer_ac");
            entity.Property(e => e.CustomerCode).HasColumnName("customer_code");
            entity.Property(e => e.Invoiced).HasColumnName("invoiced");
            entity.Property(e => e.Network).HasColumnName("network");
            entity.Property(e => e.QuantitySign)
                .HasMaxLength(1)
                .HasColumnName("quantity_sign");
            entity.Property(e => e.RecordCount).HasColumnName("record_count");
            entity.Property(e => e.ReportType).HasColumnName("report_type");
            entity.Property(e => e.TotalCost).HasColumnName("total_cost");
            entity.Property(e => e.TotalQuantity).HasColumnName("total_quantity");
        });

        modelBuilder.Entity<FcEmail>(entity =>
        {
            entity.HasKey(e => e.Account).HasName("fc_emails_pkey");

            entity.ToTable("fc_emails");

            entity.Property(e => e.Account)
                .ValueGeneratedNever()
                .HasColumnName("account");
            entity.Property(e => e.Bcc)
                .HasColumnType("character varying")
                .HasColumnName("bcc");
            entity.Property(e => e.Cc)
                .HasColumnType("character varying")
                .HasColumnName("cc");
            entity.Property(e => e.To)
                .HasColumnType("character varying")
                .HasColumnName("to");
        });

        modelBuilder.Entity<FcGrade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fc_grades_pkey");

            entity.ToTable("fc_grades");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Grade)
                .HasColumnType("character varying")
                .HasColumnName("grade");
            entity.Property(e => e.GradeId).HasColumnName("grade_id");
        });

        modelBuilder.Entity<FcHiddenCard>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CardNo }).HasName("fc_hidden_cards_pkey");

            entity.ToTable("fc_hidden_cards");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.CardNo)
                .HasColumnType("character varying")
                .HasColumnName("card_no");
            entity.Property(e => e.AccountNo).HasColumnName("account_no");
            entity.Property(e => e.CostCentre)
                .HasColumnType("character varying")
                .HasColumnName("cost_centre");
            entity.Property(e => e.Network).HasColumnName("network");
            entity.Property(e => e.PortlandId).HasColumnName("portland_id");
        });

        modelBuilder.Entity<FcIntroducersEmail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fc_introducers_pkey");

            entity.ToTable("fc_introducers_email");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('fc_introducers_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.ContactId).HasColumnName("contact_id");
            entity.Property(e => e.EmailField)
                .HasDefaultValue((short)0)
                .HasComment("0 - To, 1 - CC, 2 - BCC")
                .HasColumnName("email_field");
            entity.Property(e => e.IntroducerId).HasColumnName("introducer_id");
            entity.Property(e => e.Network)
                .HasComment("0 - Keyfuels, 1 - Uk Fuels, 2 - Texaco, 3 - FuelGenie, 100 - All Networks")
                .HasColumnName("network");
        });

        modelBuilder.Entity<FcMaskedCard>(entity =>
        {
            entity.HasKey(e => e.CardNumber).HasName("fc_masked_cards_pkey");

            entity.ToTable("fc_masked_cards", tb => tb.HasComment("A table for masked cards numbers and the portland id they relate to"));

            entity.Property(e => e.CardNumber)
                .HasPrecision(20)
                .HasColumnName("card_number");
            entity.Property(e => e.Network).HasColumnName("network");
            entity.Property(e => e.PortlandId).HasColumnName("portland_id");
        });

        modelBuilder.Entity<FcNetwork>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fc_networks_pkey");

            entity.ToTable("fc_networks", tb => tb.HasComment("A list of networks the fuelcards operate on"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NetworkName)
                .HasMaxLength(50)
                .HasColumnName("network_name");
        });

        modelBuilder.Entity<FcNetworkAccNoToPortlandId>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fc_network_acc_no_to_portland_id_pkey");

            entity.ToTable("fc_network_acc_no_to_portland_id", tb => tb.HasComment("the networks are as follows 0 - Keyfuels, 1 - UK Fuels, 2 - Texaco, 3 - FuelGenie"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FcAccountNo).HasColumnName("fc_account_no");
            entity.Property(e => e.Network).HasColumnName("network");
            entity.Property(e => e.PortlandId).HasColumnName("portland_id");
        });

        modelBuilder.Entity<FcProspect>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fc_prospects_pkey");

            entity.ToTable("fc_prospects");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<FcProspectsDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fc_prospects_details_pkey");

            entity.ToTable("fc_prospects_details");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Notes)
                .HasColumnType("character varying")
                .HasColumnName("notes");
            entity.Property(e => e.PrimaryContactName)
                .HasColumnType("character varying")
                .HasColumnName("primary_contact_name");
            entity.Property(e => e.PrimaryContactNumber).HasColumnName("primary_contact_number");
            entity.Property(e => e.TpChecled).HasColumnName("tp_checled");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.FcProspectsDetail)
                .HasForeignKey<FcProspectsDetail>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("id");
        });

        modelBuilder.Entity<FcRequiredEdiReport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fc_required_edi_reports_pkey");

            entity.ToTable("fc_required_edi_reports");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Fuelgenie)
                .HasDefaultValue(false)
                .HasColumnName("fuelgenie");
            entity.Property(e => e.IntroducerId).HasColumnName("introducer_id");
            entity.Property(e => e.Keyfuels)
                .HasDefaultValue(true)
                .HasColumnName("keyfuels");
            entity.Property(e => e.Texaco)
                .HasDefaultValue(true)
                .HasColumnName("texaco");
            entity.Property(e => e.UkFuels)
                .HasDefaultValue(true)
                .HasColumnName("uk_fuels");
        });

        modelBuilder.Entity<FcReseller>(entity =>
        {
            entity.HasKey(e => e.PortlandId).HasName("fc_resellers_pkey");

            entity.ToTable("fc_resellers");

            entity.Property(e => e.PortlandId)
                .ValueGeneratedNever()
                .HasColumnName("portland_id");
            entity.Property(e => e.Networks).HasColumnName("networks");
        });

        modelBuilder.Entity<FgCard>(entity =>
        {
            entity.HasKey(e => e.CardId).HasName("fg_cards_pkey");

            entity.ToTable("fg_cards", tb => tb.HasComment("Fuelgenie Portland Cards Details"));

            entity.Property(e => e.CardId)
                .ValueGeneratedNever()
                .HasColumnName("card_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CaptureMileagePos).HasColumnName("capture_mileage_pos");
            entity.Property(e => e.CarWash)
                .HasDefaultValue(true)
                .HasColumnName("car_wash");
            entity.Property(e => e.CardLimit).HasColumnName("card_limit");
            entity.Property(e => e.CostCentre)
                .HasMaxLength(5)
                .HasColumnName("cost_centre");
            entity.Property(e => e.Diesel)
                .HasDefaultValue(true)
                .HasColumnName("diesel");
            entity.Property(e => e.DisplayName)
                .HasMaxLength(27)
                .HasColumnName("display_name");
            entity.Property(e => e.EmployeeNumber)
                .HasMaxLength(10)
                .HasColumnName("employee_number");
            entity.Property(e => e.ExpiryDate).HasColumnName("expiry_date");
            entity.Property(e => e.IsActivated)
                .HasDefaultValue(false)
                .HasColumnName("is_activated");
            entity.Property(e => e.IsPoolCard).HasColumnName("is_pool_card");
            entity.Property(e => e.IsTest)
                .HasDefaultValue(false)
                .HasColumnName("is_test");
            entity.Property(e => e.Oil)
                .HasDefaultValue(true)
                .HasColumnName("oil");
            entity.Property(e => e.PanNumber)
                .HasMaxLength(19)
                .HasColumnName("pan_number");
            entity.Property(e => e.PortlandId).HasColumnName("portland_id");
            entity.Property(e => e.RegNo)
                .HasMaxLength(15)
                .HasColumnName("reg_no");
            entity.Property(e => e.RegionalLocation)
                .HasMaxLength(5)
                .HasColumnName("regional_location");
            entity.Property(e => e.Status)
                .HasMaxLength(27)
                .HasColumnName("status");
            entity.Property(e => e.ThirdPartyName)
                .HasMaxLength(27)
                .HasColumnName("third_party_name");
            entity.Property(e => e.Unleaded)
                .HasDefaultValue(true)
                .HasColumnName("unleaded");
            entity.Property(e => e.VehicleType)
                .HasMaxLength(10)
                .HasColumnName("vehicle_type");
        });

        modelBuilder.Entity<FgCardSwipe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fg_card_swipe_pkey");

            entity.ToTable("fg_card_swipe");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateSwiped).HasColumnName("date_swiped");
            entity.Property(e => e.ExpiryDate).HasColumnName("expiry_date");
            entity.Property(e => e.Pan)
                .HasMaxLength(19)
                .HasColumnName("pan");
            entity.Property(e => e.PurchaseLimitations)
                .HasMaxLength(5)
                .HasColumnName("purchase_limitations");
            entity.Property(e => e.RegCheckInd)
                .HasMaxLength(1)
                .HasColumnName("reg_check_ind");
            entity.Property(e => e.RegCheckNo)
                .HasMaxLength(2)
                .HasColumnName("reg_check_no");
            entity.Property(e => e.RegMileageEntry)
                .HasMaxLength(1)
                .HasColumnName("reg_mileage_entry");
            entity.Property(e => e.ServiceCode)
                .HasMaxLength(4)
                .HasColumnName("service_code");
            entity.Property(e => e.UserEntry)
                .HasMaxLength(50)
                .HasColumnName("user_entry");
        });

        modelBuilder.Entity<FgCardstatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fg_cardstatuses_pkey");

            entity.ToTable("fg_cardstatuses");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Status)
                .HasMaxLength(27)
                .HasColumnName("status");
        });

        modelBuilder.Entity<FgRequestLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fg_request_log_pkey");

            entity.ToTable("fg_request_log", tb => tb.HasComment("Logs the requests sent to fuelGenie"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Details)
                .HasColumnType("character varying")
                .HasColumnName("details");
            entity.Property(e => e.Errorinfo)
                .HasColumnType("character varying")
                .HasColumnName("errorinfo");
            entity.Property(e => e.Guid)
                .HasMaxLength(40)
                .HasColumnName("guid");
            entity.Property(e => e.RequestDate).HasColumnName("request_date");
            entity.Property(e => e.Result)
                .HasMaxLength(30)
                .HasColumnName("result");
            entity.Property(e => e.Type)
                .HasMaxLength(25)
                .HasColumnName("type");
        });

        modelBuilder.Entity<FgTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("fg_transactions_pkey");

            entity.ToTable("fg_transactions");

            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.AuthCode).HasColumnName("auth_code");
            entity.Property(e => e.CardName)
                .HasColumnType("character varying")
                .HasColumnName("card_name");
            entity.Property(e => e.CustomerNumber).HasColumnName("customer_number");
            entity.Property(e => e.EftNumber).HasColumnName("eft_number");
            entity.Property(e => e.FileProcessDate).HasColumnName("file_process_date");
            entity.Property(e => e.GrossAmount).HasColumnName("gross_amount");
            entity.Property(e => e.Invoiced).HasColumnName("invoiced");
            entity.Property(e => e.MerchantId).HasColumnName("merchant_id");
            entity.Property(e => e.MerchantName)
                .HasColumnType("character varying")
                .HasColumnName("merchant_name");
            entity.Property(e => e.Mileage).HasColumnName("mileage");
            entity.Property(e => e.NetAmount).HasColumnName("net_amount");
            entity.Property(e => e.PanNumber)
                .HasMaxLength(19)
                .HasColumnName("pan_number");
            entity.Property(e => e.PortlandId).HasColumnName("portland_id");
            entity.Property(e => e.ProductCode).HasColumnName("product_code");
            entity.Property(e => e.ProductName)
                .HasColumnType("character varying")
                .HasColumnName("product_name");
            entity.Property(e => e.ProductNumber).HasColumnName("product_number");
            entity.Property(e => e.ProductQuantity).HasColumnName("product_quantity");
            entity.Property(e => e.PurchaseRefund)
                .HasMaxLength(15)
                .HasColumnName("purchase_refund");
            entity.Property(e => e.RegNo)
                .HasColumnType("character varying")
                .HasColumnName("reg_no");
            entity.Property(e => e.Supermarket)
                .HasColumnType("character varying")
                .HasColumnName("supermarket");
            entity.Property(e => e.TransactionDate).HasColumnName("transaction_date");
            entity.Property(e => e.TransactionTime).HasColumnName("transaction_time");
            entity.Property(e => e.VatAmount).HasColumnName("vat_amount");
            entity.Property(e => e.VatRate).HasColumnName("vat_rate");
        });

        modelBuilder.Entity<FgTransactionsTest>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("fg_transactions_test_pkey");

            entity.ToTable("fg_transactions_test");

            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.AuthCode).HasColumnName("auth_code");
            entity.Property(e => e.CardName)
                .HasColumnType("character varying")
                .HasColumnName("card_name");
            entity.Property(e => e.CustomerNumber).HasColumnName("customer_number");
            entity.Property(e => e.EftNumber).HasColumnName("eft_number");
            entity.Property(e => e.FileProcessDate).HasColumnName("file_process_date");
            entity.Property(e => e.GrossAmount).HasColumnName("gross_amount");
            entity.Property(e => e.MerchantId).HasColumnName("merchant_id");
            entity.Property(e => e.MerchantName)
                .HasColumnType("character varying")
                .HasColumnName("merchant_name");
            entity.Property(e => e.Mileage).HasColumnName("mileage");
            entity.Property(e => e.NetAmount).HasColumnName("net_amount");
            entity.Property(e => e.PanNumber)
                .HasMaxLength(19)
                .HasColumnName("pan_number");
            entity.Property(e => e.PortlandId).HasColumnName("portland_id");
            entity.Property(e => e.ProductCode).HasColumnName("product_code");
            entity.Property(e => e.ProductName)
                .HasColumnType("character varying")
                .HasColumnName("product_name");
            entity.Property(e => e.ProductNumber).HasColumnName("product_number");
            entity.Property(e => e.ProductQuantity).HasColumnName("product_quantity");
            entity.Property(e => e.PurchaseRefund)
                .HasMaxLength(15)
                .HasColumnName("purchase_refund");
            entity.Property(e => e.RegNo)
                .HasColumnType("character varying")
                .HasColumnName("reg_no");
            entity.Property(e => e.Supermarket)
                .HasColumnType("character varying")
                .HasColumnName("supermarket");
            entity.Property(e => e.TransactionDate).HasColumnName("transaction_date");
            entity.Property(e => e.TransactionTime).HasColumnName("transaction_time");
            entity.Property(e => e.VatAmount).HasColumnName("vat_amount");
            entity.Property(e => e.VatRate).HasColumnName("vat_rate");
        });

        modelBuilder.Entity<FixAllocationDate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fix_allocation_date_pkey");

            entity.ToTable("fix_allocation_date");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Allocated).HasColumnName("allocated");
            entity.Property(e => e.AllocationEnd).HasColumnName("allocation_end");
            entity.Property(e => e.NewAllocationDate).HasColumnName("new_allocation_date");
            entity.Property(e => e.TradeId).HasColumnName("trade_id");

            entity.HasOne(d => d.Trade).WithMany(p => p.FixAllocationDates)
                .HasForeignKey(d => d.TradeId)
                .HasConstraintName("trade_id");
        });

        modelBuilder.Entity<FixFrequency>(entity =>
        {
            entity.HasKey(e => e.FrequencyId).HasName("fix_frequency_pkey");

            entity.ToTable("fix_frequency");

            entity.Property(e => e.FrequencyId).HasColumnName("frequency_id");
            entity.Property(e => e.FrequencyPeriod)
                .HasColumnType("character varying")
                .HasColumnName("frequency_period");
            entity.Property(e => e.NoDays).HasColumnName("no_days");
            entity.Property(e => e.PeriodsPerYear).HasColumnName("periods_per_year");
        });

        modelBuilder.Entity<FixedPriceContract>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fixed_price_contacts_pkey");

            entity.ToTable("fixed_price_contracts");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('fixed_price_contacts_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.EffectiveFrom).HasColumnName("effective_from");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.FcAccount).HasColumnName("fc_account");
            entity.Property(e => e.FixedPrice).HasColumnName("fixed_price");
            entity.Property(e => e.FixedPriceIncDuty).HasColumnName("fixed_price_inc_duty");
            entity.Property(e => e.FixedVolume).HasColumnName("fixed_volume");
            entity.Property(e => e.FrequencyId).HasColumnName("frequency_id");
            entity.Property(e => e.Grade).HasColumnName("grade");
            entity.Property(e => e.Network).HasColumnName("network");
            entity.Property(e => e.Period).HasColumnName("period");
            entity.Property(e => e.PortlandId).HasColumnName("portland_id");
            entity.Property(e => e.TerminationDate).HasColumnName("termination_date");
            entity.Property(e => e.TradeReference).HasColumnName("trade_reference");

            entity.HasOne(d => d.Frequency).WithMany(p => p.FixedPriceContracts)
                .HasForeignKey(d => d.FrequencyId)
                .HasConstraintName("frequency_id");
        });

        modelBuilder.Entity<Fuelcard>(entity =>
        {
            entity.HasKey(e => e.PortlandId).HasName("fuelcards_pkey");

            entity.ToTable("fuelcards", tb => tb.HasComment("Network:\n1) Texaco\n2) Fuel Genie\n3) Key Fuels\n4) Uk Fuels"));

            entity.Property(e => e.PortlandId)
                .ValueGeneratedNever()
                .HasColumnName("portland_id");
            entity.Property(e => e.AccountNumber).HasColumnName("account_number");
            entity.Property(e => e.Expiry).HasColumnName("expiry");
            entity.Property(e => e.Network).HasColumnName("network");
            entity.Property(e => e.Pan)
                .HasMaxLength(25)
                .HasColumnName("pan");
        });

        modelBuilder.Entity<FuelcardBasePrice>(entity =>
        {
            entity.HasKey(e => e.EffectiveFrom).HasName("fuelcard_base_price_pkey");

            entity.ToTable("fuelcard_base_price");

            entity.Property(e => e.EffectiveFrom).HasColumnName("effective_from");
            entity.Property(e => e.BasePrice).HasColumnName("base_price");
            entity.Property(e => e.EffectiveTo).HasColumnName("effective_to");
        });

        modelBuilder.Entity<FuelcardPrice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fuelcard_prices_pkey");

            entity.ToTable("fuelcard_prices");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EffectiveFrom).HasColumnName("effective_from");
            entity.Property(e => e.EffectiveTo).HasColumnName("effective_to");
            entity.Property(e => e.FixedPrice).HasColumnName("fixed_price");
            entity.Property(e => e.FixedVolume).HasColumnName("fixed_volume");
            entity.Property(e => e.FuelcardAccountNumber).HasColumnName("fuelcard_account_number");
            entity.Property(e => e.Network).HasColumnName("network");
        });

        modelBuilder.Entity<InvoiceFormatGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("invoice_format_groups_pkey");

            entity.ToTable("invoice_format_groups");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Group)
                .HasColumnType("character varying")
                .HasColumnName("group");
        });

        modelBuilder.Entity<InvoiceNumber>(entity =>
        {
            entity.HasKey(e => new { e.InvoiceNumber1, e.Network }).HasName("invoice_number_pkey");

            entity.ToTable("invoice_number");

            entity.Property(e => e.InvoiceNumber1)
                .ValueGeneratedOnAdd()
                .HasColumnName("invoice_number");
            entity.Property(e => e.Network).HasColumnName("network");
        });

        modelBuilder.Entity<InvoiceReport>(entity =>
        {
            entity.HasKey(e => new { e.InvoiceDate, e.AccountNo }).HasName("invoice_report_pkey");

            entity.ToTable("invoice_report");

            entity.Property(e => e.InvoiceDate).HasColumnName("invoice_date");
            entity.Property(e => e.AccountNo).HasColumnName("account_no");
            entity.Property(e => e.AdbluePrice).HasColumnName("adblue_price");
            entity.Property(e => e.AdblueVol).HasColumnName("adblue_vol");
            entity.Property(e => e.BrushTollPrice).HasColumnName("brush_toll_price");
            entity.Property(e => e.BrushTollVol).HasColumnName("brush_toll_vol");
            entity.Property(e => e.ComPayable)
                .HasColumnType("character varying")
                .HasColumnName("com_payable");
            entity.Property(e => e.Commission).HasColumnName("commission");
            entity.Property(e => e.Current).HasColumnName("current");
            entity.Property(e => e.DieselLifted).HasColumnName("diesel_lifted");
            entity.Property(e => e.DieselPrice).HasColumnName("diesel_price");
            entity.Property(e => e.DieselVol).HasColumnName("diesel_vol");
            entity.Property(e => e.Fixed).HasColumnName("fixed");
            entity.Property(e => e.Floating).HasColumnName("floating");
            entity.Property(e => e.GasoilPrice).HasColumnName("gasoil_price");
            entity.Property(e => e.GasoilVol).HasColumnName("gasoil_vol");
            entity.Property(e => e.InvNo)
                .HasColumnType("character varying")
                .HasColumnName("inv_no");
            entity.Property(e => e.LubesPrice).HasColumnName("lubes_price");
            entity.Property(e => e.LubesVol).HasColumnName("lubes_vol");
            entity.Property(e => e.NetTotal).HasColumnName("net_total");
            entity.Property(e => e.Network).HasColumnName("network");
            entity.Property(e => e.OtherVol).HasColumnName("other_vol");
            entity.Property(e => e.OthersPrice).HasColumnName("others_price");
            entity.Property(e => e.PayDate).HasColumnName("pay_date");
            entity.Property(e => e.PetrolPrice).HasColumnName("petrol_price");
            entity.Property(e => e.PetrolVol).HasColumnName("petrol_vol");
            entity.Property(e => e.PremDieselPrice).HasColumnName("prem_diesel_price");
            entity.Property(e => e.PremDieselVol).HasColumnName("prem_diesel_vol");
            entity.Property(e => e.RollAvailable).HasColumnName("roll_available");
            entity.Property(e => e.Rolled).HasColumnName("rolled");
            entity.Property(e => e.SainsburysPrice).HasColumnName("sainsburys_price");
            entity.Property(e => e.SainsburysVol).HasColumnName("sainsburys_vol");
            entity.Property(e => e.SuperUnleadedPrice).HasColumnName("super_unleaded_price");
            entity.Property(e => e.SuperUnleadedVol).HasColumnName("super_unleaded_vol");
            entity.Property(e => e.TescoPrice).HasColumnName("tesco_price");
            entity.Property(e => e.TescoSainsburys).HasColumnName("tesco_sainsburys");
            entity.Property(e => e.TescoVol).HasColumnName("tesco_vol");
            entity.Property(e => e.Total).HasColumnName("total");
            entity.Property(e => e.Vat).HasColumnName("vat");
        });

        modelBuilder.Entity<InvoiceSent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("invoice_sent_pkey");

            entity.ToTable("invoice_sent");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Sent).HasColumnName("sent");
        });

        modelBuilder.Entity<InvoicingOption>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("invoicing_options_pkey");

            entity.ToTable("invoicing_options");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Displaygroup).HasColumnName("displaygroup");
            entity.Property(e => e.GroupedNetwork).HasColumnName("grouped_network");
            entity.Property(e => e.PortlandId).HasColumnName("portland_id");

            entity.HasOne(d => d.DisplaygroupNavigation).WithMany(p => p.InvoicingOptions)
                .HasForeignKey(d => d.Displaygroup)
                .HasConstraintName("displaygroup");
        });

        modelBuilder.Entity<KeyFuelsCard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("keyFuelsCards_pkey");

            entity.ToTable("keyFuelsCards");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountNo).HasColumnName("account_no");
            entity.Property(e => e.ExpiryDate).HasColumnName("expiry_date");
            entity.Property(e => e.PanNumber)
                .HasMaxLength(40)
                .HasColumnName("pan_number");
            entity.Property(e => e.PortlandId).HasColumnName("portland_id");
        });

        modelBuilder.Entity<KfCard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("kf_cards_pkey");

            entity.ToTable("kf_cards");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Accountcode).HasColumnName("accountcode");
            entity.Property(e => e.Accountsuffix)
                .HasMaxLength(255)
                .HasColumnName("accountsuffix");
            entity.Property(e => e.Dailytransfuellimit).HasColumnName("dailytransfuellimit");
            entity.Property(e => e.Embossedtext)
                .HasMaxLength(255)
                .HasColumnName("embossedtext");
            entity.Property(e => e.European).HasColumnName("european");
            entity.Property(e => e.Expirydate).HasColumnName("expirydate");
            entity.Property(e => e.Fleetnumber).HasColumnName("fleetnumber");
            entity.Property(e => e.Fourthlineembossedtext)
                .HasMaxLength(255)
                .HasColumnName("fourthlineembossedtext");
            entity.Property(e => e.Fridayallowed).HasColumnName("fridayallowed");
            entity.Property(e => e.Mileagerequired).HasColumnName("mileagerequired");
            entity.Property(e => e.Mondayallowed).HasColumnName("mondayallowed");
            entity.Property(e => e.Nofalsepinentries).HasColumnName("nofalsepinentries");
            entity.Property(e => e.Notransperday).HasColumnName("notransperday");
            entity.Property(e => e.Notransperweek).HasColumnName("notransperweek");
            entity.Property(e => e.Pannumber).HasColumnName("pannumber");
            entity.Property(e => e.Pin).HasColumnName("pin");
            entity.Property(e => e.Pinlockoutminutes).HasColumnName("pinlockoutminutes");
            entity.Property(e => e.Pinrequired).HasColumnName("pinrequired");
            entity.Property(e => e.PortlandId).HasColumnName("portland_id");
            entity.Property(e => e.Recordtype).HasColumnName("recordtype");
            entity.Property(e => e.Saturdayallowed).HasColumnName("saturdayallowed");
            entity.Property(e => e.Singletransfuellimit).HasColumnName("singletransfuellimit");
            entity.Property(e => e.Smart).HasColumnName("smart");
            entity.Property(e => e.Stopstatus).HasColumnName("stopstatus");
            entity.Property(e => e.Sundayallowed).HasColumnName("sundayallowed");
            entity.Property(e => e.Thursdayallowed).HasColumnName("thursdayallowed");
            entity.Property(e => e.Tuesdayallowed).HasColumnName("tuesdayallowed");
            entity.Property(e => e.Validendtime).HasColumnName("validendtime");
            entity.Property(e => e.Validstarttime).HasColumnName("validstarttime");
            entity.Property(e => e.Vehicleregistration)
                .HasMaxLength(255)
                .HasColumnName("vehicleregistration");
            entity.Property(e => e.Wednesdayallowed).HasColumnName("wednesdayallowed");
            entity.Property(e => e.Weeklytransfuellimit).HasColumnName("weeklytransfuellimit");
        });

        modelBuilder.Entity<KfE11Product>(entity =>
        {
            entity.HasKey(e => e.ProductCode).HasName("kf_e11_products_pkey");

            entity.ToTable("kf_e11_products");

            entity.Property(e => e.ProductCode)
                .ValueGeneratedNever()
                .HasColumnName("product_code");
            entity.Property(e => e.ProductDescription)
                .HasMaxLength(30)
                .HasColumnName("product_description");
        });

        modelBuilder.Entity<KfE19Card>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("kf_e19_cards_pkey");

            entity.ToTable("kf_e19_cards");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActionStatus)
                .HasMaxLength(1)
                .HasColumnName("action_status");
            entity.Property(e => e.CardGrade).HasColumnName("card_grade");
            entity.Property(e => e.CustomerAccountCode).HasColumnName("customer_account_code");
            entity.Property(e => e.CustomerAccountSuffix).HasColumnName("customer_account_suffix");
            entity.Property(e => e.DailyTransFuelLimit).HasColumnName("daily_trans_fuel_limit");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.EmbossingDetails)
                .HasMaxLength(1)
                .HasColumnName("embossing_details");
            entity.Property(e => e.European).HasColumnName("european");
            entity.Property(e => e.ExpiryDate).HasColumnName("expiry_date");
            entity.Property(e => e.FridayAllowed).HasColumnName("friday_allowed");
            entity.Property(e => e.MileageEntryFlag)
                .HasMaxLength(1)
                .HasColumnName("mileage_entry_flag");
            entity.Property(e => e.MondayAllowed).HasColumnName("monday_allowed");
            entity.Property(e => e.NumberFalsePinEntries).HasColumnName("number_false_pin_entries");
            entity.Property(e => e.NumberTransPerDay).HasColumnName("number_trans_per_day");
            entity.Property(e => e.NumberTransPerWeek).HasColumnName("number_trans_per_week");
            entity.Property(e => e.OdometerUnit)
                .HasMaxLength(1)
                .HasColumnName("odometer_unit");
            entity.Property(e => e.PanNumber)
                .HasPrecision(19)
                .HasColumnName("pan_number");
            entity.Property(e => e.PinLockoutMinutes).HasColumnName("pin_lockout_minutes");
            entity.Property(e => e.PinNumber).HasColumnName("pin_number");
            entity.Property(e => e.PinRequired).HasColumnName("pin_required");
            entity.Property(e => e.PortlandId).HasColumnName("portland_id");
            entity.Property(e => e.SaturdayAllowed).HasColumnName("saturday_allowed");
            entity.Property(e => e.SingleTransFuelLimit).HasColumnName("single_trans_fuel_limit");
            entity.Property(e => e.Smart).HasColumnName("smart");
            entity.Property(e => e.SundayAllowed).HasColumnName("sunday_allowed");
            entity.Property(e => e.TelephoneRequired).HasColumnName("telephone_required");
            entity.Property(e => e.ThursdayAllowed).HasColumnName("thursday_allowed");
            entity.Property(e => e.Time).HasColumnName("time");
            entity.Property(e => e.TuesdayAllowed).HasColumnName("tuesday_allowed");
            entity.Property(e => e.ValidEndTime).HasColumnName("valid_end_time");
            entity.Property(e => e.ValidStartTime).HasColumnName("valid_start_time");
            entity.Property(e => e.VehicleReg)
                .HasMaxLength(12)
                .HasColumnName("vehicle_reg");
            entity.Property(e => e.WednesdayAllowed).HasColumnName("wednesday_allowed");
            entity.Property(e => e.WeeklyTransFuelLimit).HasColumnName("weekly_trans_fuel_limit");
        });

        modelBuilder.Entity<KfE1E3Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("kf_e1_e3_transactions_pkey");

            entity.ToTable("kf_e1_e3_transactions");

            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.AccurateMileage)
                .HasMaxLength(1)
                .HasColumnName("accurate_mileage");
            entity.Property(e => e.CardNumber)
                .HasPrecision(19)
                .HasColumnName("card_number");
            entity.Property(e => e.CardRegistration)
                .HasMaxLength(12)
                .HasColumnName("card_registration");
            entity.Property(e => e.Commission).HasColumnName("commission");
            entity.Property(e => e.ControlId).HasColumnName("control_id");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.CostSign)
                .HasMaxLength(1)
                .HasColumnName("cost_sign");
            entity.Property(e => e.CustomerAc).HasColumnName("customer_ac");
            entity.Property(e => e.CustomerCode).HasColumnName("customer_code");
            entity.Property(e => e.FleetNumber).HasColumnName("fleet_number");
            entity.Property(e => e.InvoiceNumber).HasColumnName("invoice_number");
            entity.Property(e => e.InvoicePrice).HasColumnName("invoice_price");
            entity.Property(e => e.Invoiced).HasColumnName("invoiced");
            entity.Property(e => e.Mileage).HasColumnName("mileage");
            entity.Property(e => e.Period).HasColumnName("period");
            entity.Property(e => e.PortlandId).HasColumnName("portland_id");
            entity.Property(e => e.PrimaryRegistration)
                .HasMaxLength(12)
                .HasColumnName("primary_registration");
            entity.Property(e => e.ProductCode).HasColumnName("product_code");
            entity.Property(e => e.PumpNumber).HasColumnName("pump_number");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.ReportType)
                .HasDefaultValue(true)
                .HasColumnName("report_type");
            entity.Property(e => e.Sign)
                .HasMaxLength(1)
                .HasColumnName("sign");
            entity.Property(e => e.SiteCode).HasColumnName("site_code");
            entity.Property(e => e.TransactionDate).HasColumnName("transaction_date");
            entity.Property(e => e.TransactionNumber).HasColumnName("transaction_number");
            entity.Property(e => e.TransactionSequence).HasColumnName("transaction_sequence");
            entity.Property(e => e.TransactionTime).HasColumnName("transaction_time");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(1)
                .HasColumnName("transaction_type");
            entity.Property(e => e.TransactonRegistration)
                .HasMaxLength(12)
                .HasColumnName("transacton_registration");
        });

        modelBuilder.Entity<KfE20StoppedCard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("kf_e20_stopped_cards_pkey");

            entity.ToTable("kf_e20_stopped_cards");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CardId).HasColumnName("card_id");
            entity.Property(e => e.CustomerAccountCode).HasColumnName("customer_account_code");
            entity.Property(e => e.CustomerAccountSuffix).HasColumnName("customer_account_suffix");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.PanNumber)
                .HasPrecision(19)
                .HasColumnName("pan_number");
            entity.Property(e => e.PortlandId).HasColumnName("portland_id");
            entity.Property(e => e.StopCode)
                .HasMaxLength(1)
                .HasColumnName("stop_code");
            entity.Property(e => e.Time).HasColumnName("time");
        });

        modelBuilder.Entity<KfE21Account>(entity =>
        {
            entity.HasKey(e => e.CustomerAccountCode).HasName("kf_e21_accounts_pkey");

            entity.ToTable("kf_e21_accounts");

            entity.Property(e => e.CustomerAccountCode)
                .ValueGeneratedNever()
                .HasColumnName("customer_account_code");
            entity.Property(e => e.ActionStatus)
                .HasMaxLength(1)
                .HasColumnName("action_status");
            entity.Property(e => e.AddressLine1)
                .HasMaxLength(30)
                .HasColumnName("address_line1");
            entity.Property(e => e.AddressLine2)
                .HasMaxLength(30)
                .HasColumnName("address_line2");
            entity.Property(e => e.County)
                .HasMaxLength(20)
                .HasColumnName("county");
            entity.Property(e => e.CustomerAccountSuffix).HasColumnName("customer_account_suffix");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
            entity.Property(e => e.PortlandId).HasColumnName("portland_id");
            entity.Property(e => e.Postcode)
                .HasMaxLength(10)
                .HasColumnName("postcode");
            entity.Property(e => e.Time).HasColumnName("time");
            entity.Property(e => e.Town)
                .HasMaxLength(30)
                .HasColumnName("town");
        });

        modelBuilder.Entity<KfE22AccountsStopped>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("kf_e22_accounts_stopped_pkey");

            entity.ToTable("kf_e22_accounts_stopped");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CustomerAccountCode).HasColumnName("customer_account_code");
            entity.Property(e => e.CustomerAccountSuffix).HasColumnName("customer_account_suffix");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.PersonRequestingStop)
                .HasMaxLength(30)
                .HasColumnName("person_requesting_stop");
            entity.Property(e => e.PortlandId).HasColumnName("portland_id");
            entity.Property(e => e.StopReferenceNumber).HasColumnName("stop_reference_number");
            entity.Property(e => e.StopStatusCode)
                .HasMaxLength(1)
                .HasColumnName("stop_status_code");
            entity.Property(e => e.Time).HasColumnName("time");
        });

        modelBuilder.Entity<KfE23NewClosedSite>(entity =>
        {
            entity.HasKey(e => e.SiteAccountCode).HasName("kf_e23_new_closed_sites_pkey");

            entity.ToTable("kf_e23_new_closed_sites");

            entity.Property(e => e.SiteAccountCode)
                .ValueGeneratedNever()
                .HasColumnName("site_account_code");
            entity.Property(e => e.AddressLine1)
                .HasMaxLength(30)
                .HasColumnName("address_line1");
            entity.Property(e => e.AddressLine2)
                .HasMaxLength(30)
                .HasColumnName("address_line2");
            entity.Property(e => e.Bar).HasColumnName("bar");
            entity.Property(e => e.CafeRestaurant).HasColumnName("cafe_restaurant");
            entity.Property(e => e.Canopy).HasColumnName("canopy");
            entity.Property(e => e.CashpointMachines).HasColumnName("cashpoint_machines");
            entity.Property(e => e.ContactName)
                .HasMaxLength(29)
                .HasColumnName("contact_name");
            entity.Property(e => e.County)
                .HasMaxLength(20)
                .HasColumnName("county");
            entity.Property(e => e.Directions)
                .HasMaxLength(225)
                .HasColumnName("directions");
            entity.Property(e => e.Gasoil).HasColumnName("gasoil");
            entity.Property(e => e.JunctionNumber).HasColumnName("junction_number");
            entity.Property(e => e.Lubricants).HasColumnName("lubricants");
            entity.Property(e => e.MachineType)
                .HasMaxLength(1)
                .HasColumnName("machine_type");
            entity.Property(e => e.MotorwayJunction).HasColumnName("motorway_junction");
            entity.Property(e => e.MotorwayNumber).HasColumnName("motorway_number");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
            entity.Property(e => e.OpeningHours1)
                .HasMaxLength(21)
                .HasColumnName("opening_hours_1");
            entity.Property(e => e.OpeningHours2)
                .HasMaxLength(21)
                .HasColumnName("opening_hours_2");
            entity.Property(e => e.OpeningHours3)
                .HasMaxLength(21)
                .HasColumnName("opening_hours_3");
            entity.Property(e => e.OvernightAccomodation).HasColumnName("overnight_accomodation");
            entity.Property(e => e.Parking).HasColumnName("parking");
            entity.Property(e => e.Payphone).HasColumnName("payphone");
            entity.Property(e => e.PoleSignSupplier).HasColumnName("pole_sign_supplier");
            entity.Property(e => e.Postcode)
                .HasMaxLength(10)
                .HasColumnName("postcode");
            entity.Property(e => e.Repairs).HasColumnName("repairs");
            entity.Property(e => e.RetailSite).HasColumnName("retail_site");
            entity.Property(e => e.Shop).HasColumnName("shop");
            entity.Property(e => e.Showers).HasColumnName("showers");
            entity.Property(e => e.SiteAccountSuffix).HasColumnName("site_account_suffix");
            entity.Property(e => e.SiteStatus)
                .HasMaxLength(1)
                .HasColumnName("site_status");
            entity.Property(e => e.SleeperCabsWelcome).HasColumnName("sleeper_cabs_welcome");
            entity.Property(e => e.TankCleaning).HasColumnName("tank_cleaning");
            entity.Property(e => e.TelephoneNumber)
                .HasMaxLength(15)
                .HasColumnName("telephone_number");
            entity.Property(e => e.Toilets).HasColumnName("toilets");
            entity.Property(e => e.Town)
                .HasMaxLength(30)
                .HasColumnName("town");
            entity.Property(e => e.VehicleClearanceAccepted).HasColumnName("vehicle_clearance_accepted");
            entity.Property(e => e.WindscreenReplacement).HasColumnName("windscreen_replacement");
        });

        modelBuilder.Entity<KfE2Delivery>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("kf_e2_deliveries_pkey");

            entity.ToTable("kf_e2_deliveries");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ControlId).HasColumnName("control_id");
            entity.Property(e => e.CustomerAc).HasColumnName("customer_ac");
            entity.Property(e => e.CustomerCode).HasColumnName("customer_code");
            entity.Property(e => e.CustomerOrderNo)
                .HasMaxLength(7)
                .HasColumnName("customer_order_no");
            entity.Property(e => e.CustomerOwnOrderNo)
                .HasMaxLength(15)
                .HasColumnName("customer_own_order_no");
            entity.Property(e => e.DeliveryNoteNo)
                .HasMaxLength(10)
                .HasColumnName("delivery_note_no");
            entity.Property(e => e.HandlingCharge)
                .HasMaxLength(1)
                .HasColumnName("handling_charge");
            entity.Property(e => e.Period).HasColumnName("period");
            entity.Property(e => e.ProductCode).HasColumnName("product_code");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.QuantitySign)
                .HasMaxLength(1)
                .HasColumnName("quantity_sign");
            entity.Property(e => e.SiteCode).HasColumnName("site_code");
            entity.Property(e => e.SupplierName)
                .HasMaxLength(30)
                .HasColumnName("supplier_name");
            entity.Property(e => e.TransactionDate).HasColumnName("transaction_date");
            entity.Property(e => e.TransactionNumber).HasColumnName("transaction_number");
            entity.Property(e => e.TransactionSequence).HasColumnName("transaction_sequence");
            entity.Property(e => e.TransactionTime).HasColumnName("transaction_time");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(1)
                .HasColumnName("transaction_type");
        });

        modelBuilder.Entity<KfE4SundrySale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("kf_e4_deliveries_pkey");

            entity.ToTable("kf_e4_sundry_sales");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CardNumber)
                .HasPrecision(19)
                .HasColumnName("card_number");
            entity.Property(e => e.ControlId).HasColumnName("control_id");
            entity.Property(e => e.CustomerAc).HasColumnName("customer_ac");
            entity.Property(e => e.CustomerCode).HasColumnName("customer_code");
            entity.Property(e => e.Invoiced)
                .HasDefaultValue(false)
                .HasColumnName("invoiced");
            entity.Property(e => e.Period).HasColumnName("period");
            entity.Property(e => e.ProductCode).HasColumnName("product_code");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.QuantitySign)
                .HasMaxLength(1)
                .HasColumnName("quantity_sign");
            entity.Property(e => e.Reference)
                .HasMaxLength(5)
                .HasColumnName("reference");
            entity.Property(e => e.TransactionDate).HasColumnName("transaction_date");
            entity.Property(e => e.TransactionNumber).HasColumnName("transaction_number");
            entity.Property(e => e.TransactionSequence).HasColumnName("transaction_sequence");
            entity.Property(e => e.TransactionTime).HasColumnName("transaction_time");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(1)
                .HasColumnName("transaction_type");
            entity.Property(e => e.Value).HasColumnName("value");
            entity.Property(e => e.ValueSign)
                .HasMaxLength(1)
                .HasColumnName("value_sign");
            entity.Property(e => e.VehicleRegistration)
                .HasMaxLength(12)
                .HasColumnName("vehicle_registration");
        });

        modelBuilder.Entity<KfE5Stock>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("kf_e5_stock_pkey");

            entity.ToTable("kf_e5_stock");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClosingBalanceSign)
                .HasMaxLength(1)
                .HasColumnName("closing_balance_sign");
            entity.Property(e => e.ClosingStockBalance).HasColumnName("closing_stock_balance");
            entity.Property(e => e.ControlId).HasColumnName("control_id");
            entity.Property(e => e.CustomerAc).HasColumnName("customer_ac");
            entity.Property(e => e.CustomerCode).HasColumnName("customer_code");
            entity.Property(e => e.DeliveryQuantity).HasColumnName("delivery_quantity");
            entity.Property(e => e.DeliveryQuantitySign)
                .HasMaxLength(1)
                .HasColumnName("delivery_quantity_sign");
            entity.Property(e => e.DrawingQuantity).HasColumnName("drawing_quantity");
            entity.Property(e => e.DrawingQuantitySign)
                .HasMaxLength(1)
                .HasColumnName("drawing_quantity_sign");
            entity.Property(e => e.NumberOfDeliveries).HasColumnName("number_of_deliveries");
            entity.Property(e => e.NumberOfDrawings).HasColumnName("number_of_drawings");
            entity.Property(e => e.OpeningBalanceSign)
                .HasMaxLength(1)
                .HasColumnName("opening_balance_sign");
            entity.Property(e => e.OpeningStockBalance).HasColumnName("opening_stock_balance");
            entity.Property(e => e.Period).HasColumnName("period");
            entity.Property(e => e.ProductCode).HasColumnName("product_code");
        });

        modelBuilder.Entity<KfE6Transfer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("kf_e6_transfers_pkey");

            entity.ToTable("kf_e6_transfers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ControlId).HasColumnName("control_id");
            entity.Property(e => e.Narrative)
                .HasMaxLength(70)
                .HasColumnName("narrative");
            entity.Property(e => e.TransactionNumber).HasColumnName("transaction_number");
            entity.Property(e => e.TransactionSequence).HasColumnName("transaction_sequence");
        });

        modelBuilder.Entity<MissingProductValue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("missing_product_values_pkey");

            entity.ToTable("missing_product_values");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Network).HasColumnName("network");
            entity.Property(e => e.Product).HasColumnName("product");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<OpsCustomer>(entity =>
        {
            entity.Property(e => e.CreationTime).HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<OpsOrder>(entity =>
        {
            entity.HasIndex(e => e.CustomerId, "IX_OpsOrders_CustomerId");

            entity.Property(e => e.CreationTime).HasColumnType("timestamp without time zone");
            entity.Property(e => e.DeliveryDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.OrderDate).HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.Customer).WithMany(p => p.OpsOrders).HasForeignKey(d => d.CustomerId);
        });

        modelBuilder.Entity<PaymentTerm>(entity =>
        {
            entity.HasKey(e => e.XeroId).HasName("payment_terms_pkey");

            entity.ToTable("payment_terms");

            entity.Property(e => e.XeroId)
                .HasColumnType("character varying")
                .HasColumnName("xero_id");
            entity.Property(e => e.Network).HasColumnName("network");
            entity.Property(e => e.PaymentTerms).HasColumnName("payment_terms");
        });

        modelBuilder.Entity<PortlandFuelcard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("portland_fuelcards_pkey");

            entity.ToTable("portland_fuelcards");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ExpiryDate).HasColumnName("expiry_date");
            entity.Property(e => e.FuelcardAccountNumber).HasColumnName("fuelcard_account_number");
            entity.Property(e => e.Network).HasColumnName("network");
            entity.Property(e => e.PanNumber)
                .HasMaxLength(19)
                .HasColumnName("pan_number");
            entity.Property(e => e.Portlandid).HasColumnName("portlandid");
            entity.Property(e => e.XeroId)
                .HasMaxLength(75)
                .HasColumnName("xero_id");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => new { e.Code, e.Network }).HasName("Products_pkey");

            entity.Property(e => e.Product1)
                .HasMaxLength(255)
                .HasColumnName("Product");
        });

        modelBuilder.Entity<ProductDescriptionToInventoryItemCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_description_to_inventory_item_code_pkey");

            entity.ToTable("product_description_to_inventory_item_code");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.InventoryItemcode)
                .HasColumnType("character varying")
                .HasColumnName("inventoryItemcode");
        });

        modelBuilder.Entity<RolledVolume>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("rolled_volume_pkey");

            entity.ToTable("rolled_volume");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateOfAllocation).HasColumnName("date_of_allocation");
            entity.Property(e => e.IsCurrent).HasColumnName("is_current");
            entity.Property(e => e.Period).HasColumnName("period");
            entity.Property(e => e.PortlandId).HasColumnName("portland_id");
            entity.Property(e => e.TradeReferenceId).HasColumnName("trade_reference_id");
            entity.Property(e => e.VolumeRolled).HasColumnName("volume_rolled");

            entity.HasOne(d => d.TradeReference).WithMany(p => p.RolledVolumes)
                .HasForeignKey(d => d.TradeReferenceId)
                .HasConstraintName("trade_reference_id");
        });

        modelBuilder.Entity<SandwichOrder>(entity =>
        {
            entity.Property(e => e.Filling).HasMaxLength(100);
            entity.Property(e => e.User).HasMaxLength(50);
        });

        modelBuilder.Entity<SiteBandInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("site_band_info_pkey");

            entity.ToTable("site_band_info");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Band)
                .HasMaxLength(15)
                .HasColumnName("band");
            entity.Property(e => e.CommercialPrice).HasColumnName("commercial_price");
            entity.Property(e => e.EffectiveFrom).HasColumnName("effective_from");
            entity.Property(e => e.NetworkId).HasColumnName("network_id");
        });

        modelBuilder.Entity<SiteNumberToBand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("site_number_to_band_new_pkey");

            entity.ToTable("site_number_to_band");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Band)
                .HasColumnType("character varying")
                .HasColumnName("band");
            entity.Property(e => e.Brand)
                .HasMaxLength(255)
                .HasColumnName("brand");
            entity.Property(e => e.Classification).HasColumnName("classification");
            entity.Property(e => e.EffectiveDate).HasColumnName("effective_date");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.NetworkId).HasColumnName("network_id");
            entity.Property(e => e.ShareTheBurdon).HasColumnName("share_the_burdon");
            entity.Property(e => e.SiteNumber).HasColumnName("site_number");
            entity.Property(e => e.Surcharge).HasColumnName("surcharge");

            entity.HasOne(d => d.Network).WithMany(p => p.SiteNumberToBands)
                .HasForeignKey(d => d.NetworkId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("network_id");
        });

        modelBuilder.Entity<TexacoCard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("texaco_cards_pkey");

            entity.ToTable("texaco_cards");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('texaco_cards_id_seq1'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.CarWash)
                .HasColumnType("char")
                .HasColumnName("car_wash");
            entity.Property(e => e.CardLinked)
                .HasColumnType("char")
                .HasColumnName("card_linked");
            entity.Property(e => e.ClientNo).HasColumnName("client_no");
            entity.Property(e => e.Company)
                .HasMaxLength(255)
                .HasColumnName("company");
            entity.Property(e => e.CostCenter).HasColumnName("cost_center");
            entity.Property(e => e.CustomerCountry)
                .HasMaxLength(255)
                .HasColumnName("customer_country");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(255)
                .HasColumnName("customer_name");
            entity.Property(e => e.CustomerNumber).HasColumnName("customer_number");
            entity.Property(e => e.Diesel)
                .HasColumnType("char")
                .HasColumnName("diesel");
            entity.Property(e => e.Division).HasColumnName("division");
            entity.Property(e => e.DivisionName)
                .HasMaxLength(255)
                .HasColumnName("division_name");
            entity.Property(e => e.Embossed3)
                .HasMaxLength(20)
                .HasColumnName("embossed_3");
            entity.Property(e => e.Embossed4).HasColumnName("embossed_4");
            entity.Property(e => e.ExpiryDate).HasColumnName("expiry_date");
            entity.Property(e => e.GasOil)
                .HasColumnType("char")
                .HasColumnName("gas_oil");
            entity.Property(e => e.Goods)
                .HasColumnType("char")
                .HasColumnName("goods");
            entity.Property(e => e.InvoiceCentreNumber).HasColumnName("invoice_centre_number");
            entity.Property(e => e.IssueDate).HasColumnName("issue_date");
            entity.Property(e => e.IssueNumber).HasColumnName("issue_number");
            entity.Property(e => e.Lpg)
                .HasColumnType("char")
                .HasColumnName("lpg");
            entity.Property(e => e.Lrp)
                .HasColumnType("char")
                .HasColumnName("lrp");
            entity.Property(e => e.LubeOil)
                .HasColumnType("char")
                .HasColumnName("lube_oil");
            entity.Property(e => e.Pan).HasColumnName("pan");
            entity.Property(e => e.PinNo).HasColumnName("pin_no");
            entity.Property(e => e.StopDate).HasColumnName("stop_date");
            entity.Property(e => e.StopFlag)
                .HasColumnType("char")
                .HasColumnName("stop_flag");
            entity.Property(e => e.SuperUnleaded)
                .HasColumnType("char")
                .HasColumnName("super_unleaded");
            entity.Property(e => e.Uid)
                .HasColumnType("character varying")
                .HasColumnName("uid");
            entity.Property(e => e.Unleaded)
                .HasColumnType("char")
                .HasColumnName("unleaded");
        });

        modelBuilder.Entity<TexacoControl>(entity =>
        {
            entity.HasKey(e => e.ControlId).HasName("texaco_control_pkey");

            entity.ToTable("texaco_control");

            entity.Property(e => e.ControlId).HasColumnName("control_id");
            entity.Property(e => e.ExportDate).HasColumnName("export_date");
            entity.Property(e => e.TotalQuantity).HasColumnName("total_quantity");
            entity.Property(e => e.TotalTransactions).HasColumnName("total_transactions");
        });

        modelBuilder.Entity<TexacoEdi>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("texacoEdi");

            entity.Property(e => e.BunkerSheet)
                .HasColumnType("character varying")
                .HasColumnName("bunker_sheet");
            entity.Property(e => e.CardNo).HasColumnName("card_no");
            entity.Property(e => e.CustNo).HasColumnName("cust_no");
            entity.Property(e => e.CustType)
                .HasColumnType("char")
                .HasColumnName("cust_type");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.DistNo).HasColumnName("dist_no");
            entity.Property(e => e.HostAccount).HasColumnName("host_account");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Mileage).HasColumnName("mileage");
            entity.Property(e => e.MonthNo).HasColumnName("month_no");
            entity.Property(e => e.ProductNo).HasColumnName("product_no");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Reg)
                .HasColumnType("character varying")
                .HasColumnName("reg");
            entity.Property(e => e.SiteNo).HasColumnName("site_no");
            entity.Property(e => e.Time)
                .HasColumnType("time with time zone")
                .HasColumnName("time");
            entity.Property(e => e.TransactionNo).HasColumnName("transaction_no");
            entity.Property(e => e.Value).HasColumnName("value");
            entity.Property(e => e.WeekNo).HasColumnName("week_no");
            entity.Property(e => e.WeekNoChange).HasColumnName("week_no (change)");
        });

        modelBuilder.Entity<TexacoTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("texaco_transactions_pkey");

            entity.ToTable("texaco_transactions");

            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.Batch).HasColumnName("batch");
            entity.Property(e => e.CardNo).HasColumnName("card_no");
            entity.Property(e => e.ClientType)
                .HasMaxLength(1)
                .HasColumnName("client_type");
            entity.Property(e => e.Commission).HasColumnName("commission");
            entity.Property(e => e.ControlId).HasColumnName("control_id");
            entity.Property(e => e.Customer).HasColumnName("customer");
            entity.Property(e => e.Division).HasColumnName("division");
            entity.Property(e => e.InvoiceNumber).HasColumnName("invoice_number");
            entity.Property(e => e.InvoicePrice).HasColumnName("invoice_price");
            entity.Property(e => e.Invoiced).HasColumnName("invoiced");
            entity.Property(e => e.IsoNumber).HasColumnName("iso_number");
            entity.Property(e => e.Mileage).HasColumnName("mileage");
            entity.Property(e => e.MonthNo).HasColumnName("month_no");
            entity.Property(e => e.PortlandId).HasColumnName("portland_id");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.ProdNo).HasColumnName("prod_no");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Registration)
                .HasMaxLength(12)
                .HasColumnName("registration");
            entity.Property(e => e.Site).HasColumnName("site");
            entity.Property(e => e.TranDate).HasColumnName("tran_date");
            entity.Property(e => e.TranNoItem).HasColumnName("tran_no_item");
            entity.Property(e => e.TranTime).HasColumnName("tran_time");
            entity.Property(e => e.WeekNo).HasColumnName("week_no");
        });

        modelBuilder.Entity<TransactionSiteSurcharge>(entity =>
        {
            entity.HasKey(e => e.EffectiveDate).HasName("transaction_site_surcharge_pkey");

            entity.ToTable("transaction_site_surcharge");

            entity.Property(e => e.EffectiveDate).HasColumnName("effective_date");
            entity.Property(e => e.ChargeType)
                .HasColumnType("character varying")
                .HasColumnName("charge_type");
            entity.Property(e => e.Network).HasColumnName("network");
            entity.Property(e => e.Surcharge).HasColumnName("surcharge");
        });

        modelBuilder.Entity<TransactionalSite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transactional_sites_pkey");

            entity.ToTable("transactional_sites");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EffectiveDate).HasColumnName("effective_date");
            entity.Property(e => e.Network).HasColumnName("network");
            entity.Property(e => e.SiteCode).HasColumnName("site_code");
        });

        modelBuilder.Entity<UkfTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("ukf_transactions_pkey");

            entity.ToTable("ukf_transactions");

            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.Batch).HasColumnName("batch");
            entity.Property(e => e.CardNo).HasColumnName("card_no");
            entity.Property(e => e.ClientType)
                .HasMaxLength(1)
                .HasColumnName("client_type");
            entity.Property(e => e.Commission).HasColumnName("commission");
            entity.Property(e => e.ControlId).HasColumnName("control_id");
            entity.Property(e => e.Customer).HasColumnName("customer");
            entity.Property(e => e.Division).HasColumnName("division");
            entity.Property(e => e.InvoiceNumber).HasColumnName("invoice_number");
            entity.Property(e => e.InvoicePrice).HasColumnName("invoice_price");
            entity.Property(e => e.Invoiced)
                .HasDefaultValue(false)
                .HasColumnName("invoiced");
            entity.Property(e => e.Mileage).HasColumnName("mileage");
            entity.Property(e => e.MonthNo).HasColumnName("month_no");
            entity.Property(e => e.PanNumber)
                .HasPrecision(20)
                .HasColumnName("pan_number");
            entity.Property(e => e.PortlandId).HasColumnName("portland_id");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.ProdNo).HasColumnName("prod_no");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.ReceiptNo)
                .HasMaxLength(4)
                .HasColumnName("receipt_no");
            entity.Property(e => e.Registration)
                .HasMaxLength(12)
                .HasColumnName("registration");
            entity.Property(e => e.Site).HasColumnName("site");
            entity.Property(e => e.TranDate).HasColumnName("tran_date");
            entity.Property(e => e.TranNoItem).HasColumnName("tran_no_item");
            entity.Property(e => e.TranTime).HasColumnName("tran_time");
            entity.Property(e => e.WeekNo).HasColumnName("week_no");
        });

        modelBuilder.Entity<UkfuelCard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("uk_fuelsCard_pkey");

            entity.ToTable("ukfuel_cards");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('\"uk_fuelsCard_id_seq\"'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.CarWash)
                .HasColumnType("char")
                .HasColumnName("car_wash");
            entity.Property(e => e.CardType)
                .HasMaxLength(20)
                .HasColumnName("card_type");
            entity.Property(e => e.ComapnyNo).HasColumnName("comapny_no");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(75)
                .HasColumnName("company_name");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(255)
                .HasColumnName("customer_name");
            entity.Property(e => e.CustomerNumber).HasColumnName("customer_number");
            entity.Property(e => e.CutomerCountry)
                .HasMaxLength(255)
                .HasColumnName("cutomer_country");
            entity.Property(e => e.Diesel)
                .HasColumnType("char")
                .HasColumnName("diesel");
            entity.Property(e => e.DivisionName)
                .HasMaxLength(255)
                .HasColumnName("division_name");
            entity.Property(e => e.DivisionNo).HasColumnName("division_no");
            entity.Property(e => e.Embossed3)
                .HasMaxLength(255)
                .HasColumnName("embossed_3");
            entity.Property(e => e.Embossed4)
                .HasMaxLength(255)
                .HasColumnName("embossed_4");
            entity.Property(e => e.ExpiryDate).HasColumnName("expiry_date");
            entity.Property(e => e.GasOil)
                .HasColumnType("char")
                .HasColumnName("gas_oil");
            entity.Property(e => e.Goods)
                .HasColumnType("char")
                .HasColumnName("goods");
            entity.Property(e => e.IssueDate).HasColumnName("issue_date");
            entity.Property(e => e.LastTransaction).HasColumnName("last_transaction");
            entity.Property(e => e.Lpg)
                .HasColumnType("char")
                .HasColumnName("lpg");
            entity.Property(e => e.Lrp)
                .HasColumnType("char")
                .HasColumnName("lrp");
            entity.Property(e => e.LubeOil)
                .HasColumnType("char")
                .HasColumnName("lube_oil");
            entity.Property(e => e.PanNumber).HasColumnName("pan_number");
            entity.Property(e => e.PinNo).HasColumnName("pin_no");
            entity.Property(e => e.StopDate).HasColumnName("stop_date");
            entity.Property(e => e.StopFlag)
                .HasColumnType("char")
                .HasColumnName("stop_flag");
            entity.Property(e => e.SuperUnleaded)
                .HasColumnType("char")
                .HasColumnName("super_unleaded");
            entity.Property(e => e.Unleaded)
                .HasColumnType("char")
                .HasColumnName("unleaded");
        });

        modelBuilder.Entity<UkfuelTransaction>(entity =>
        {
            entity.HasKey(e => e.Transactionnumber).HasName("ukfuel_transactions_pkey");

            entity.ToTable("ukfuel_transactions");

            entity.Property(e => e.Transactionnumber)
                .ValueGeneratedNever()
                .HasColumnName("transactionnumber");
            entity.Property(e => e.Batch).HasColumnName("batch");
            entity.Property(e => e.Cardnumber).HasColumnName("cardnumber");
            entity.Property(e => e.ClientType)
                .HasMaxLength(255)
                .HasColumnName("client_type");
            entity.Property(e => e.Customer).HasColumnName("customer");
            entity.Property(e => e.Division).HasColumnName("division");
            entity.Property(e => e.Invoiced).HasColumnName("invoiced");
            entity.Property(e => e.Mileage).HasColumnName("mileage");
            entity.Property(e => e.Month).HasColumnName("month");
            entity.Property(e => e.Pan).HasColumnName("pan");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Productnumber).HasColumnName("productnumber");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Receiptnumber).HasColumnName("receiptnumber");
            entity.Property(e => e.Registration)
                .HasMaxLength(255)
                .HasColumnName("registration");
            entity.Property(e => e.Site).HasColumnName("site");
            entity.Property(e => e.Transactiondate).HasColumnName("transactiondate");
            entity.Property(e => e.Transactiontime).HasColumnName("transactiontime");
            entity.Property(e => e.Week).HasColumnName("week");
        });

        modelBuilder.Entity<XeroCustomer>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("xero_customers");

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
            entity.Property(e => e.PortlandId).HasColumnName("portland_id");
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
        modelBuilder.HasSequence("fg_transactions_test_transaction_id_seq").HasMax(2147483647L);
        modelBuilder.HasSequence<int>("kf_control_control_id_seq");
        modelBuilder.HasSequence<int>("kf_e1_e3_transactions_transaction_id_seq");
        modelBuilder.HasSequence<int>("portland_fuelcards_id_seq");
        modelBuilder.HasSequence<int>("site_number_to_band_id_seq");
        modelBuilder.HasSequence<int>("texaco_cards_id_seq").HasMin(0L);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
