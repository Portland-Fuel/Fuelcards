using DataAccess.Fuelcards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Repositorys.IRepositorys;
using Portland.Data.Repository.IRepository;

namespace DataAccess.Repositorys.IRepositorys
{
    public interface IFuelcardUnitOfWork : IDisposable
    {
        IFgCardRepository FgCards { get; }
        IKfE19CardsRepository KfE19Cards { get; }
        IUkFuelCardRepository UkFuelCard { get; }
        ITexacoCardRepository TexacoCard { get; }
        IAccountToPortlandIDRepository AccountToPortlandID { get; }
        IKfE01Repository KfE01 { get; }
        IFgTransactionRepository FgTransaction { get; }
        IUkfTransactionsRepository UkfTransactions { get; }
        ITexacoTracactionRepository TexacoTransaction { get; }
        IAddonRepository Addon { get; }
        IcustomerpricingaddonRepository CustomerPricingAddon { get; }
        IKfE2DeliveryRepository KfE2Delivery { get; }
        IKfE4SundrySales KfE4SundrySales { get; }
        IKfE5Repository KfE5 { get; }
        IKfE6Repository KfE6 { get; }
        IKfE11ProductsRepository KfE11Products { get; }
        IKfE20Repository KfE20 { get; }
        IKfE21AccountsRepository KfE21Accounts { get; }
        IKfE22AccountsStoppedRepository KfE22AccountsStopped { get; }
        IKfE23NewClosedSitesRepository KfE23NewClosedSites { get; }
        IFcControlRepository FcControl { get; }
        IFcMaskedCardsRepository FcMaskedCards { get; }
        IFcRequiredEdiReportsRepository FcRequiredEdiReports { get; }
        ITransactionalSitesRepository TransactionalSites { get; }
        ITransactionalSiteSurchargeRepository TransactionalSiteSurcharge { get; }
        ISiteNumberToBandRepository SiteNumberToBand { get; }
        IFixedPriceContractsRepository FixedPriceContracts { get; }
        IFuelcardBasePricesRepository FuelcardBasePrices { get; }
        IAllocatedVolumeRepository AllocatedVolume { get; }
        IInvoicingOptionsRepository InvoiceOptions { get; }
        IfcgradesRepository fcgrades { get; }
        IFixFrequencyRepository FixFrequency { get; }
        IFixAllocationDateRepository FixAllocationDate { get; }
        IInvoiceFormatGroupsRepository InvoiceFormatGroups { get; }
        ICustomerMultipleAccountSameNetworkRepository CustomerMultipleAccountSameNetwork { get; }
        IAllocatedVolumeSnapshotRepository AllocatedVolumeSnapshot { get; }
        ISiteBandInfoRepository SiteBandInfo { get; }
        IFcHiddenCardsRepository FcHiddenCards { get; }
        IMissingProductsRepository MissingProducts { get; }
        IPaymentTermsRepository PaymentTerms { get; }
        IInvoiceNumberRepository InvoiceNumber { get; }
        IInvoiceReportRepository InvoiceReport { get; }
        IFcEmailsRepository FcEmails { get; }
        IFcProspectsRepository FcProspects { get; }
        IFcProspectsDetailsRepository FcProspectsDetails { get; }
        void Save();
        Task SaveAsync();
        IQueryable<T> Set<T>() where T : class;
    }
}
