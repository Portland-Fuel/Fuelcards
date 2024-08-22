using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;
using Portland.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Repositorys;
using Portland.Data.Repository;
namespace DataAccess.Repositorys.UnitOfWork
{
    public class FuelcardUnitOfWork : IFuelcardUnitOfWork
    {

        private readonly FuelcardsContext _db;

        private IFgCardRepository _fgCards { get; }
        private IKfE19CardsRepository _KfE19Cards { get; }
        private IUkFuelCardRepository _UkFuelCard { get; }
        private ITexacoCardRepository _TexacoCard { get; }
        private IAccountToPortlandIDRepository _AccountToPortlandID { get; }
        private IKfE01Repository _KfE01 { get; }
        private IFgTransactionRepository _FgTransaction { get; }
        private IUkfTransactionsRepository _UkfTransactions { get; }
        private ITexacoTracactionRepository _TexacoTransaction { get; }
        private IAddonRepository _Addon { get; }
        private IcustomerpricingaddonRepository _CustomerPricingAddon { get; }

        private IKfE2DeliveryRepository _KfE2Delivery { get; }
        private IKfE4SundrySales _KfE4SundrySales { get; }
        private IKfE5Repository _KfE5 { get; }
        private IKfE6Repository _KfE6 { get; }
        private IKfE11ProductsRepository _KfE11Products { get; }
        private IKfE20Repository _KfE20 { get; }
        private IKfE21AccountsRepository _KfE21Accounts { get; }
        private IKfE22AccountsStoppedRepository _KfE22AccountsStopped { get; }
        private IKfE23NewClosedSitesRepository _KfE23NewClosedSitesRepository { get; }
        private IFcControlRepository _FcControl { get; }
        private IFcMaskedCardsRepository _FcMaskedCards { get; }
        private IFcRequiredEdiReportsRepository _FcRequiredEdiReports { get; }
        private ITransactionalSitesRepository _TransactionalSites { get; }
        private ITransactionalSiteSurchargeRepository _TransactionalSiteSurcharge { get; }
        private ISiteNumberToBandRepository _SiteNumberToBand { get; }
        private IFixedPriceContractsRepository _FixedPriceContracts { get; }
        private IFuelcardBasePricesRepository _FuelcardBasePrices { get; }
        private IAllocatedVolumeRepository _AllocatedVolume { get; }
        private IInvoicingOptionsRepository _InvoiceOptions { get; }
        private IfcgradesRepository _fcgrades { get; }
        private IFixFrequencyRepository _FixFrequency { get; }
        private IFixAllocationDateRepository _FixAllocationDate { get; }
        private IInvoiceFormatGroupsRepository _InvoiceFormatGroups { get; }
        private ICustomerMultipleAccountSameNetworkRepository _CustomerMultipleAccountSameNetwork { get; }
        private IAllocatedVolumeSnapshotRepository _AllocatedVolumeSnapshot { get; }
        private ISiteBandInfoRepository _SiteBandInfo { get; }
        private IFcHiddenCardsRepository _FcHiddenCards { get; }
        private IMissingProductsRepository _MissingProducts { get; }
        private IPaymentTermsRepository _PaymentTerms { get; }
        private IInvoiceNumberRepository _InvoiceNumber { get; }
        private IInvoiceReportRepository _InvoiceReport { get; }
        private IFcEmailsRepository _FcEmails { get; }
        private IFcProspectsRepository _FcProspects { get; }
        private IFcProspectsDetailsRepository _FcProspectsDetails { get; }




        public IFgCardRepository FgCards => _fgCards ?? new FgCardRepository(_db);
        public IKfE19CardsRepository KfE19Cards => _KfE19Cards ?? new KfE19CardsRepository(_db);
        public IUkFuelCardRepository UkFuelCard => _UkFuelCard ?? new UkFuelCardRepository(_db);
        public ITexacoCardRepository TexacoCard => _TexacoCard ?? new TexacoCardRepository(_db);
        public IPaymentTermsRepository PaymentTerms => _PaymentTerms ?? new PaymentTermsRepository(_db);

        public IAccountToPortlandIDRepository AccountToPortlandID => _AccountToPortlandID ?? new AccountToPortlandIDRepository(_db);
        public IKfE01Repository KfE01 => _KfE01 ?? new KfE01Repository(_db);
        public IFgTransactionRepository FgTransaction => _FgTransaction ?? new FgTransactionRepository(_db);
        public IUkfTransactionsRepository UkfTransactions => _UkfTransactions ?? new UkfTraansactionsRepository(_db);
        public ITexacoTracactionRepository TexacoTransaction => _TexacoTransaction ?? new TexacoTransactionRepository(_db);
        public IAddonRepository Addon => _Addon ?? new AddonRepository(_db);
        public IcustomerpricingaddonRepository CustomerPricingAddon => _CustomerPricingAddon ?? new customerpricingaddonRepository(_db);


        public IKfE2DeliveryRepository KfE2Delivery => _KfE2Delivery ?? new KfE2DeliveryRepository(_db);
        public IKfE4SundrySales KfE4SundrySales => _KfE4SundrySales ?? new KfE4SundrySalesRepository(_db);
        public IKfE5Repository KfE5 => _KfE5 ?? new KfE5Repository(_db);

        public IKfE6Repository KfE6 => _KfE6 ?? new KfE6Repository(_db);
        public IKfE11ProductsRepository KfE11Products => _KfE11Products ?? new KfE11ProductsRepository(_db);
        public IKfE20Repository KfE20 => _KfE20 ?? new KfE20Repository(_db);
        public IKfE21AccountsRepository KfE21Accounts => _KfE21Accounts ?? new KfE21AccountsRepository(_db);
        public IKfE22AccountsStoppedRepository KfE22AccountsStopped => _KfE22AccountsStopped ?? new KfE22AccountsStoppedRepository(_db);
        public IKfE23NewClosedSitesRepository KfE23NewClosedSites => _KfE23NewClosedSitesRepository ?? new KfE23NewClosedSitesRepository(_db);
        public IFcControlRepository FcControl => _FcControl ?? new FcControlRepository(_db);
        public IFcMaskedCardsRepository FcMaskedCards => _FcMaskedCards ?? new FcMaskedCardsRepository(_db);
        public IFcRequiredEdiReportsRepository FcRequiredEdiReports => _FcRequiredEdiReports ?? new FcRequiredEdiReportsRepository(_db);
        public ITransactionalSitesRepository TransactionalSites => _TransactionalSites ?? new TransactionalSitesRepository(_db);
        public ITransactionalSiteSurchargeRepository TransactionalSiteSurcharge => _TransactionalSiteSurcharge ?? new TransactionalSiteSurchargeRepository(_db);
        public ISiteNumberToBandRepository SiteNumberToBand => _SiteNumberToBand ?? new SiteNumberToBandRepository(_db);
        public IFixedPriceContractsRepository FixedPriceContracts => _FixedPriceContracts ?? new FixedPriceContractsRepository(_db);
        public IFuelcardBasePricesRepository FuelcardBasePrices => _FuelcardBasePrices ?? new FuelcardBasePricesRepository(_db);
        public IAllocatedVolumeRepository AllocatedVolume => _AllocatedVolume ?? new AllocatedVolumeRepository(_db);
        public IInvoicingOptionsRepository InvoiceOptions => _InvoiceOptions ?? new InvoicingOptionsRepository(_db);
        public IfcgradesRepository fcgrades => _fcgrades ?? new fcgradesRepository(_db);
        public IFixFrequencyRepository FixFrequency => _FixFrequency ?? new FixFrequencyRepository(_db);
        public IFixAllocationDateRepository FixAllocationDate => _FixAllocationDate ?? new FixAllocationDateRepository(_db);
        public IInvoiceFormatGroupsRepository InvoiceFormatGroups => _InvoiceFormatGroups ?? new InvoiceFormatGroupsRepository(_db);
        public ICustomerMultipleAccountSameNetworkRepository CustomerMultipleAccountSameNetwork => _CustomerMultipleAccountSameNetwork ?? new CustomerMultipleAccountSameNetworkRepository(_db);
        public IAllocatedVolumeSnapshotRepository AllocatedVolumeSnapshot => _AllocatedVolumeSnapshot ?? new AllocatedVolumeSnapshotRepository(_db);
        public ISiteBandInfoRepository SiteBandInfo => _SiteBandInfo ?? new SiteBandInfoRepository(_db);
        public IFcHiddenCardsRepository FcHiddenCards => _FcHiddenCards ?? new FcHiddenCardsRepository(_db);
        public IMissingProductsRepository MissingProducts => _MissingProducts ?? new MissingProductsRepository(_db);
        public IInvoiceNumberRepository InvoiceNumber => _InvoiceNumber ?? new InvoiceNumberRepository(_db);
        public IInvoiceReportRepository InvoiceReport => _InvoiceReport ?? new InvoiceReportRepository(_db);
        public IFcEmailsRepository FcEmails => _FcEmails ?? new FcEmailsRepository(_db);
        public IFcProspectsRepository FcProspects => _FcProspects ?? new FcProspectsRepository(_db);
        public IFcProspectsDetailsRepository FcProspectsDetails => _FcProspectsDetails ?? new FcProspectsDetailsRepository(_db);

        public FuelcardUnitOfWork(FuelcardsContext db)
        {
            _db = db;
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task DisposeAsync()
        {
            await _db.DisposeAsync();
        }


        public void Save()
        {
            _db.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
        public IQueryable<T> Set<T>() where T : class
        {
            return _db.Set<T>();
        }
    }
}
