using System;
using System.Net;
using System.Linq;
using Ease.PPIC.BO;
using System.Windows;
using MVVM.Framework;
using Ease.PPIC.Common;
using Ease.CoreSL.Model;
using System.Windows.Ink;
using System.Windows.Input;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Documents;
using Telerik.Windows.Controls;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Windows.Media.Animation;
using System.ComponentModel.Composition;

namespace Ease.PPIC.Modules
{
    public class TravelAuthRequestViewModel : BaseViewModel
    {
        #region Private fields

        private RelayCommand _saveCommand;
        private RelayCommand _addTravelCommand;
        private RelayCommand<TravelOrEventEstimatedCostModel> _addCostItemCommand;
        private RelayCommand<TravelOrEventEstimatedCostModel> _deleteCostItemCommand;
        private RelayCommand<TravelOrEventEstimatedCostModel> _calculateCommand;
        private RelayCommand<TravelOrEventPlanModel> _addTravelOrEventPlanItemCommand;
        private RelayCommand<TravelOrEventPlanModel> _deleteTravelOrEventPlanItemCommand;
        //private RelayCommand _pickItemHeirarchyCommand;
        //private RelayCommand _clearItemHeirarchyCommand;
        private RelayCommand _closeCommand;
        //private RelayCommand _refreshCommand;
        //private bool _isEdit;
        //private DepartmentViewModel _item;
        private TravelOrEvent _tempTravelOrEvent;
        //private event OnCompletion<long> DeleteColmpleted;
        //private SmartObservableCollection<FinancialRequisitionCategoryItemModel> _items;
        public event OnCompletion<bool> ItemSaveColmpleted;

        #endregion

        #region Constructor

        public TravelAuthRequestViewModel()
            : base()
        {
            Initialize(false);           
        }
        public TravelAuthRequestViewModel(bool isEvent)
            : base()
        {
            
            Initialize(isEvent);
        }
        #endregion

        #region Public Properties

        #region CultureWithFormattedPeriod : CultureInfo
        public CultureInfo CultureWithFormattedPeriod
        {
            get //added by srijon
            {
                var tempCultureInfo = new CultureInfo("en-US");
                tempCultureInfo.DateTimeFormat.ShortDatePattern = "dd MMM yyyy";
                return tempCultureInfo;
            }
        }
        #endregion

        #region AdvanceReqItemVisibility : string
        private string _advanceReqItemVisibility;

        public string AdvanceReqItemVisibility
        {
            get { return _advanceReqItemVisibility; }
            set
            {
                _advanceReqItemVisibility = value;
                base.RaisePropertyChanged("AdvanceReqItemVisibility");
            }
        }
        #endregion

        #region AdvanceReqLabelVisibility : string
        private string _advanceReqLabelVisibility;

        public string AdvanceReqLabelVisibility
        {
            get { return _advanceReqLabelVisibility; }
            set
            {
                _advanceReqLabelVisibility = value;
                base.RaisePropertyChanged("AdvanceReqLabelVisibility");
            }
        }
        #endregion
        #region EventorTravelDetailsLabel:string
        private string _eventorTravelDetailsLabel;

        public string EventorTravelDetailsLabel
        {
            get { return _eventorTravelDetailsLabel; }
            set
            {
                _eventorTravelDetailsLabel = value;
                base.RaisePropertyChanged("EventorTravelDetailsLabel");
            }
        }
        #endregion

        #region FinancialRequisitionModelItem : FinancialRequisitionModel
        private FinancialRequisitionModel _financialRequisitionModelItem;

        public FinancialRequisitionModel FinancialRequisitionModelItem
        {
            get { return _financialRequisitionModelItem; }
            set
            {
                _financialRequisitionModelItem = value;
                base.RaisePropertiesChanged("FinancialRequisitionModelItem");
            }
        } 
        #endregion

        #region TravelOrEventModelItem: TravelOrEventModel
        private TravelOrEventModel _travelOrEventModelItem;
        public TravelOrEventModel TravelOrEventModelItem
        {
            get { return _travelOrEventModelItem; }
            set
            {
                _travelOrEventModelItem = value;
                base.RaisePropertyChanged("TravelOrEventModelItem");
            }
        }
        #endregion

        #region Name :string
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }
        #endregion

        #region Code :string
        private string _code;
        public string Code
        {
            get { return _code; }
            set
            {
                if (_code != value)
                {
                    _code = value;
                    RaisePropertyChanged("Code");
                }
            }
        }
        #endregion

        #region Purpose :string
        private string _Purpose;
        public string Purpose
        {
            get { return _Purpose; }
            set
            {
                if (_Purpose != value)
                {
                    _Purpose = value;
                    RaisePropertyChanged("Purpose");
                }
            }
        }
        #endregion

        #region Venue :string
        private string _Venue;
        public string Venue
        {
            get { return _Venue; }
            set
            {
                _Venue = value;
                RaisePropertyChanged("Venue");
            }
        }
        #endregion

        #region DateOfEvent :DateTime
        private DateTime _dateofEvent;
        public DateTime DateOfEvent
        {
            get { return _dateofEvent; }
            set
            {
                _dateofEvent = value;
                base.RaisePropertiesChanged("DateOfEvent");
            }
        }
        #endregion

        #region Property TravelTypes: Dictionary<TravelTypesEnum, string>

        private Dictionary<TravelTypesEnum, string> _TravelTypes;
        public Dictionary<TravelTypesEnum, string> TravelTypes
        {
            get
            {
                if (_TravelTypes != null) return _TravelTypes;
                _TravelTypes = new Dictionary<TravelTypesEnum, string>();
                foreach (TravelTypesEnum value in Enum.GetValues(typeof(TravelTypesEnum)))
                {
                    _TravelTypes.Add(value, EnumDescription.GetEnumDescription(value));
                }
                return _TravelTypes;
            }
        }
        #endregion

        #region AmountInBDT :decimal
        private decimal _amountInBDT;

        public decimal AmountInBDT
        {
            get { return _amountInBDT; }
            set 
            { 
                _amountInBDT = value;
                base.RaisePropertyChanged("AmountInBDT");
            }
        } 
        #endregion
        
        #region Property FRType: TravelTypesEnum

        private FRTypeEnum _fRType;
        public FRTypeEnum FRType
        {
            get
            {
                return _fRType;
            }
            set
            {
                _fRType = value;
                if(_fRType==FRTypeEnum.AuthorizationWithOutAdvance)
                {
                    AdvanceReqItemVisibility = "0";
                    AdvanceReqLabelVisibility = "0";
                }
                base.RaisePropertiesChanged("FRType");
            }
        }
        #endregion

        #region Property SelectedTravelType: TravelTypesEnum

        private TravelTypesEnum _selectedTravelType;
        public TravelTypesEnum SelectedTravelType
        {
            get
            {
                return _selectedTravelType;
            }
            set
            {
                _selectedTravelType = value;
                base.RaisePropertiesChanged("SelectedTravelType");
            }
        }
        #endregion

        #region Property ModeofTravelsEnum: Dictionary<TravelTypesEnum, string>

        private Dictionary<ModeOfTravelEnum, string> _modeofTravels;
        public Dictionary<ModeOfTravelEnum, string> ModeofTravels
        {
            get
            {
                if (_modeofTravels != null) return _modeofTravels;
                _modeofTravels = new Dictionary<ModeOfTravelEnum, string>();
                foreach (ModeOfTravelEnum value in Enum.GetValues(typeof(ModeOfTravelEnum)))
                {
                    _modeofTravels.Add(value, EnumDescription.GetEnumDescription(value));
                }
                return _modeofTravels;
            }
        }
        #endregion

        #region NoOfParticipant : long
        private long _noOfParticipant;

        public long NoOfParticipant
        {
            get { return _noOfParticipant; }
            set
            {
                _noOfParticipant = value;
                RaisePropertyChanged("NoOfParticipant");
            }
        }
        #endregion

        #region TotalEstAmount : long
        private decimal _totalEstAmount;

        public decimal TotalEstAmount
        {
            get { return _totalEstAmount; }
            set
            {
                _totalEstAmount = value;
                RaisePropertyChanged("TotalEstAmount");
            }
        }
        #endregion

        #region AdvanceAmount : decimal
        private decimal? _advanceAmount;
        public decimal? AdvanceAmount
        {
            get
            {
                return _advanceAmount;
            }
            set
            {
                _advanceAmount = value;
                AmountInBDT = (_advanceAmount * _conversionRate) ?? 0;
                base.RaisePropertyChanged("AdvanceAmount");
            }
        }
        #endregion

        #region ConversionRate : decimal
        private decimal? _conversionRate;
        public decimal? ConversionRate
        {
            get
            {
                return _conversionRate;
            }
            set
            {
                _conversionRate = value;
                AmountInBDT = (_advanceAmount * _conversionRate) ?? 0;
                base.RaisePropertyChanged("ConversionRate");
            }
        }
        #endregion

        #region AdvanceCurrency :string
        private Currency _currency;
        public Currency AdvanceCurrency
        {
            get
            {
                return _currency;
            }
            set
            {
                _currency = value;
                base.RaisePropertyChanged("AdvanceCurrency");
            }
        }

        #endregion

        #region AdvanceDateRequired :DateTime
        private DateTime _dateRequired;
        public DateTime AdvanceDateRequired
        {
            get
            {
                return _dateRequired;
            }
            set
            {
                _dateRequired = value;
                base.RaisePropertyChanged("AdvanceDateRequired");
            }
        }
        #endregion

        #region AdvanceDateRefund :DateTime
        private DateTime _dateRefund;
        public DateTime AdvanceDateRefund
        {
            get
            {
                return _dateRefund;
            }
            set
            {
                _dateRefund = value;
                base.RaisePropertyChanged("AdvanceDateRefund");
            }
        }
        #endregion

        #region RequisitionID :long
        private long _requisitionID;
        public long RequisitionID
        {
            get
            {
                return _requisitionID;
            }
            set
            {
                _requisitionID = value;
                base.RaisePropertiesChanged("RequisitionID");
            }
        }
        #endregion

        #region Description : string
        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                base.RaisePropertiesChanged("Description");
            }
        }
        #endregion

        #region IsAdvanceCash :bool
        private bool _isCash;
        public bool IsAdvanceCash
        {
            get
            {
                return _isCash;
            }
            set
            {
                _isCash = value;
                base.RaisePropertyChanged("IsAdvanceCash");
            }
        }
        #endregion
        
        #region TravOrEvtTypeLabel : string
        private string _travOrEvtTypeLabel;

        public string TravOrEvtTypeLabel
        {
            get { return _travOrEvtTypeLabel; }
            set 
            { 
                _travOrEvtTypeLabel = value;
                base.RaisePropertyChanged("TravOrEvtTypeLabel");
            }
        } 
        #endregion
        
        #region IsAdvanceCredit :bool
        private bool _isAdvanceCredit;
        public bool IsAdvanceCredit
        {
            get
            {
                return _isAdvanceCredit;
            }
            set
            {
                _isAdvanceCredit = value;
                base.RaisePropertyChanged("IsAdvanceCredit");
            }
        }
        #endregion

        #region IsEvent :bool
        private bool _isEvent;
        public bool IsEvent
        {
            get
            {
                return _isEvent;
            }
            set
            {
                _isEvent = value;
                if (_isEvent)
                {
                    DateOfEvent = DateTime.Now;
                    TravOrEvtTypeLabel = "Event Type :";
                    EventorTravelDetailsLabel = "Event Details";
                    //getFRCategories();
                }
                else if (!_isEvent)
                {
                    TravelOrEventPlanModelItems.Clear();
                    TravOrEvtTypeLabel = "Travel Type :";
                    EventorTravelDetailsLabel = "Travel Details";
                }
                //if (_isEvent)  else 
                base.RaisePropertyChanged("IsEvent");
            }
        }
        #endregion

        #region IsAdvance :bool
        private bool _isAdvance;
        public bool IsAdvance
        {
            get
            {
                return _isAdvance;
            }
            set
            {
                _isAdvance = value;
                base.RaisePropertyChanged("IsAdvance");
            }
        }
        #endregion

        #region SelectedTravelOrEventEstimatedCostModelItem : TravelOrEventEstimatedCostModel
        private TravelOrEventEstimatedCostModel _SelectedTravelOrEventEstimatedCostModelItem;
        public TravelOrEventEstimatedCostModel SelectedTravelOrEventEstimatedCostModelItem
        {
            get { return _SelectedTravelOrEventEstimatedCostModelItem; }
            set
            {
                _SelectedTravelOrEventEstimatedCostModelItem = value;
                base.RaisePropertyChanged("SelectedTravelOrEventEstimatedCostModelItem");
            }
        }
        #endregion

        #region TravelOrEventEstimatedCostModelItems : SmartObservableCollection<TravelOrEventEstimatedCostModel>
        private SmartObservableCollection<TravelOrEventEstimatedCostModel> _travelOrEventEstimatedCostModelItems;
        public SmartObservableCollection<TravelOrEventEstimatedCostModel> TravelOrEventEstimatedCostModelItems
        {
            get
            {
                return _travelOrEventEstimatedCostModelItems;
            }
            set
            {
                _travelOrEventEstimatedCostModelItems = value;
                base.RaisePropertyChanged("TravelOrEventEstimatedCostModelItems");
            }
        }
        #endregion
        
        #region SelectedTravelTypeItemID: long

        private long _selectedTravelTypeItemID;
        public long SelectedTravelTypeItemID
        {
            get { return _selectedTravelTypeItemID; }
            set
            {
                _selectedTravelTypeItemID = value;
                RaisePropertyChanged("SelectedTravelTypeItemID");
            }
        }

        #endregion
        #region SelectedTravelTypeItem : FinancialRequisitionCategoryModel
        private FinancialRequisitionCategoryModel _selectedTravelTypeItem;
        public FinancialRequisitionCategoryModel SelectedTravelTypeItem
        {
            get { return _selectedTravelTypeItem; }
            set
            {
                _selectedTravelTypeItem = value;
                base.RaisePropertyChanged("SelectedTravelTypeItem");
            }
        }
        #endregion

        #region TravelTypeItems : SmartObservableCollection<FinancialRequisitionCategoryModel>
        private SmartObservableCollection<FinancialRequisitionCategoryModel> _TravelTypeItems;
        public SmartObservableCollection<FinancialRequisitionCategoryModel> TravelTypeItems
        {
            get
            {
                return _TravelTypeItems;
            }
            set
            {
                _TravelTypeItems = value;
                base.RaisePropertyChanged("TravelTypeItems");
            }
        }
        #endregion

        #region SelectedTravelOrEventPlanModelItem : TravelOrEventPlanModel
        private TravelOrEventPlanModel _selectedTravelOrEventPlanModelItem;
        public TravelOrEventPlanModel SelectedTravelOrEventPlanModelItem
        {
            get { return _selectedTravelOrEventPlanModelItem; }
            set
            {
                _selectedTravelOrEventPlanModelItem = value;
                base.RaisePropertyChanged("SelectedTravelOrEventPlanModelItem");
            }
        }
        #endregion

        #region TravelOrEventPlanModelItems : SmartObservableCollection<TravelOrEventPlanModel>
        private SmartObservableCollection<TravelOrEventPlanModel> _travelOrEventPlanModelItems;
        public SmartObservableCollection<TravelOrEventPlanModel> TravelOrEventPlanModelItems
        {
            get
            {
                return _travelOrEventPlanModelItems;
            }
            set
            {
                _travelOrEventPlanModelItems = value;
                base.RaisePropertyChanged("TravelOrEventPlanModelItems");
            }
        }
        #endregion

        #region CurrencyItems: SmartObservableCollection<CurrencyModel>

        private SmartObservableCollection<CurrencyModel> _currencyItems;
        public SmartObservableCollection<CurrencyModel> CurrencyItems
        {
            get { return _currencyItems; }
            set
            {
                if (_currencyItems != value)
                {
                    _currencyItems = value;
                    RaisePropertyChanged("CurrencyItems");
                }
            }
        }

        #endregion

        #region property FinancialRequisitionViewModelItem : FinancialRequisitionViewModel

        private FinancialRequisitionViewModel _financialRequisitionViewModelItem;
        public FinancialRequisitionViewModel FinancialRequisitionViewModelItem
        {
            get { return _financialRequisitionViewModelItem; }
            set
            {
                _financialRequisitionViewModelItem = value;
                RaisePropertyChanged("FinancialRequisitionViewModelItem");
            }
        }

        #endregion

       

        #region Property CurrencyItem : CurrencyModel

        private CurrencyModel _currencyItem;
        public CurrencyModel CurrencyItem
        {
            get { return _currencyItem; }
            set
            {
                _currencyItem = value;
                RaisePropertyChanged("CurrencyItem");
            }
        }

        #endregion
        #region InventoryTypes : ObjectsTemplate<InventoryType>
        public ObjectsTemplate<InventoryType> InventoryTypes { get; set; }
        #endregion

        #region RadController: RadWindowController

        //public RadWindowController RadController
        //{
        //    get;
        //    set;
        //}
        public RadWindowController RadWindowController
        {
            get;
            set;
        }
        #endregion

        #endregion

        #region RelayCommand
        public RelayCommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new RelayCommand(() => saveCommand())); }
        }
        public RelayCommand<TravelOrEventEstimatedCostModel> AddCostItem
        {
            get 
            { 
                return _addCostItemCommand ??  (_addCostItemCommand = new RelayCommand<TravelOrEventEstimatedCostModel>((parm) => addCostItemCommand(parm))); 
            }
        }
        public RelayCommand<TravelOrEventEstimatedCostModel> DeleteCostItem
        {
            get { return _deleteCostItemCommand ?? (_deleteCostItemCommand = new RelayCommand<TravelOrEventEstimatedCostModel>((parm) => deleteCostItemCommand(parm))); }
        }

        public RelayCommand<TravelOrEventEstimatedCostModel> Calculate
        {
            get { return _calculateCommand ?? (_calculateCommand = new RelayCommand<TravelOrEventEstimatedCostModel>((parm) => calculate(parm))); }
        }
        public RelayCommand<TravelOrEventPlanModel> AddTravelOrEventPlanItem
        {
            get { return _addTravelOrEventPlanItemCommand ?? (_addTravelOrEventPlanItemCommand = new RelayCommand<TravelOrEventPlanModel>((parm) => addTravelOrEventPlanItemCommand(parm))); }
        }
        public RelayCommand<TravelOrEventPlanModel> DeleteTravelOrEventPlanItem
        {
            get { return _deleteTravelOrEventPlanItemCommand ?? (_deleteTravelOrEventPlanItemCommand = new RelayCommand<TravelOrEventPlanModel>((parm) => deleteTravelOrEventPlanItemCommand(parm))); }
        }
        public RelayCommand AddTravelCommand
        {
            get { return _addTravelCommand ?? (_addTravelCommand = new RelayCommand(() => addTravel())); }
        }
        public RelayCommand CloseView
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(() => closeView())); }
        }

        #endregion

        #region Allow permission

        #region AllowAdd: string

        public string AllowAdd
        {
            get { return VisibilityHelper.ConvertFrom(User.IsLoggedIn && User.CurrentUser.IsPermitted(KeyConstatns.Department, PermissionTypeEnum.Create)); }
        }

        #endregion

        #region AllowEdit: string

        public string AllowEdit
        {
            get { return VisibilityHelper.ConvertFrom(User.IsLoggedIn && User.CurrentUser.IsPermitted(KeyConstatns.Department, PermissionTypeEnum.Modify)); }
        }

        #endregion

        #region AllowDelete: string
        public string AllowDelete
        {
            get { return VisibilityHelper.ConvertFrom(User.IsLoggedIn && User.CurrentUser.IsPermitted(KeyConstatns.Department, PermissionTypeEnum.Delete)); }
        }

        #endregion

        #endregion

        #region Public Methods
        public void Initialize(bool isEvent)
        {
            _isEvent = isEvent;
            _dateofEvent = DateTime.Now;
            _advanceAmount = 0;
            _amountInBDT = 0;
            _travelOrEventModelItem = new TravelOrEventModel();
            _travelOrEventEstimatedCostModelItems = new SmartObservableCollection<TravelOrEventEstimatedCostModel>();
            _travelOrEventPlanModelItems = new SmartObservableCollection<TravelOrEventPlanModel>();
            _SelectedTravelOrEventEstimatedCostModelItem = new TravelOrEventEstimatedCostModel();
            _selectedTravelOrEventPlanModelItem = new TravelOrEventPlanModel();
            _travelOrEventEstimatedCostModelItems.Add(new TravelOrEventEstimatedCostModel() { PKID = -1 });
            _travelOrEventPlanModelItems.Add(new TravelOrEventPlanModel() { PKID = -1 });
            
            if (_isEvent)
            {
                DateOfEvent = DateTime.Now;
                TravOrEvtTypeLabel = "Event Type :";
                EventorTravelDetailsLabel = "Event Details";               
            }
            else if (!_isEvent)
            {
                TravelOrEventPlanModelItems.Clear();
                TravOrEvtTypeLabel = "Travel Type :";
                EventorTravelDetailsLabel = "Travel Details";
            }
            IsAdvanceCash = false;
            IsAdvanceCredit = false;
            AdvanceReqItemVisibility = "1*";
            AdvanceReqLabelVisibility = "20";
            ConversionRate = 1;
            AdvanceDateRequired = DateTime.Now;
            AdvanceDateRefund = DateTime.Now;
            loadCurrencyItems();
            getFRCategories();
            GetInventoryTypes(); 
            base.RaisePropertiesChanged("TravelOrEventPlanModelItems", "TravelOrEventEstimatedCostModelItems");

        }
        public override void Cleanup()
        {

        }

        public void RaiseAllProperties()
        {
        }
        public void LoadItemsForEdit()
        {
            try
            {
                Services.BasicService.GetTravelOrEventByID(FinancialRequisitionViewModelItem.ReferenceID ?? 0, getTravelOrEvent_Completed);
                Services.BasicService.GetFinancialRequisitionByID(FinancialRequisitionViewModelItem.PKID, getFinancialRequisition_Completed);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
        
        #endregion

        #region Private Callback functions
        void getFinancialRequisition_Completed(FinancialRequisition result, Exception error)
        {
            if (error != null)
                throw error;
            if (result != null)
            {
                FinancialRequisitionModelItem = FinancialRequisitionAssembler<FinancialRequisition, FinancialRequisitionModel,
                                                     FinancialRequesitionItem, FRItemViewModel, FRPaymentSchedule,
                                                     FRPaymentScheduleViewModel>.AssembleContract(result);
            }
        }
        void getTravelOrEvent_Completed(TravelOrEvent result, Exception error)
        {
            if (error != null)
                throw error;
            if (result != null)
            {
                _tempTravelOrEvent = result;

                loadAllItems();
            }
        }
        void employeeViewModel_ItemsSelectionComplete(SmartObservableCollection<EmployeeModel> results, Exception error)
        {
            if (error != null)
                throw error;
            try
            {
                foreach (var emp in results)
                {
                    if (!_travelOrEventPlanModelItems.Where(x => x.TravellerEmployeeID == emp.PKID).Any())
                    {
                        _travelOrEventPlanModelItems.Insert((_travelOrEventPlanModelItems.Count - 1),
                                                                      new TravelOrEventPlanModel() { TravellerEmployeeID = emp.PKID, TravellerName = emp.Name, FromDate = DateTime.Now, ToDate = DateTime.Now });
                    }
                }
                base.RaisePropertyChanged("TravelOrEventPlanModelItems");

            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
        void viewModel_ItemsSelectionComplete(SmartObservableCollection<SKUModel> results, Exception error)
        {
            if (error != null)
                throw error;
            try
            {
                foreach (var sku in results)
                {
                    if (!_travelOrEventEstimatedCostModelItems.Where(x => x.ItemID == sku.PKID).Any())
                    {
                        _travelOrEventEstimatedCostModelItems.Insert((_travelOrEventEstimatedCostModelItems.Count - 1),
                                                                      new TravelOrEventEstimatedCostModel() { ItemID = sku.PKID, ItemName = sku.ItemName });
                    }
                }
                base.RaisePropertyChanged("TravelOrEventEstimatedCostModelItems");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }

        }
        void getCurrency_Completed(ObjectsTemplate<Currency> results, Exception error)
        {
            if (error != null)
                throw error;

            try
            {
                CurrencyItems = CurrencyAssembler<Currency, CurrencyModel>.AssembleContracts(results);
                CurrencyItems.OrderBy(x => x.SequenceID);
                CurrencyItem = CurrencyItems.Where(x => x.SequenceID == CurrencyItems.Min(c => c.SequenceID).ToEnumerable().FirstOrDefault()).FirstOrDefault();
                base.RaisePropertiesChanged("CurrencyItems");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
            finally
            {
                error = null;
                results.Clear();
                ResetStatus();
            }
        }
        void travelORevent_saveCompleted(long result, Exception error)
        {
            base.ResetStatus();
            if (error != null)
                throw error;

            if (result > 0)
            {
                MessageBox.Show("Data Saved Successfully", "Success", MessageBoxButton.OK);
                this.closeView();
            }
        }
        void getFRCategoies_Completed(ObjectsTemplate<FinancialRequisitionCategory> retValue, Exception error)
        {
            ResetStatus();
            if (error != null)
                throw error;
            if (retValue != null)
            {
                TravelTypeItems = FinancialRequesitionCategoryAssembler<FinancialRequisitionCategory, FinancialRequisitionCategoryModel>.AssembleContracts(retValue);
            }
        }
        void getinventoryTypes_Colmpleted(ObjectsTemplate<InventoryType> results, Exception error)
        {
            if (error != null)
            {
                ResetStatus();
                throw error;
            }

            InventoryTypes = results;
        }
        void getCategoryChildItems_Completed(ObjectsTemplate<FinancialRequisitionCategoryItem> results, Exception error)
        {
            ResetStatus();
            if (error != null)
                throw error;
            if (error == null)
            {
                SKUPicker item = new SKUPicker();
                item.ViewModel = new SKUPickerViewModel(true, false, InventoryTypes);
                item.ViewModel.ItemsSelectionComplete += viewModel_ItemsSelectionComplete;
                item.SetSelectColVisibility();
                item.ViewModel.CreateCopyVisible = true;
                item.ViewModel.IsItemGL = true;
                item.ViewModel.InventoryType = InventoryTypeAssembler<InventoryType, InventoryTypeModel>.AssembleContract(InventoryTypes[0]);
                item.ViewModel.InventoryTypeID = InventoryTypes[0].PKID;
                item.ViewModel.IsInventoryCboEnabled = false;
                //item.ViewModel.Pickhierachynode = true;
                item.ViewModel.NodeNameCollection = string.Join(", ", results.Select(x => x.Name).ToList());
                item.ViewModel.HierarchyNodeIDList = results.Select(x => x.ProductHierarchyNodeID).ToList();
                item.ViewModel.Controller = new RadWindowController();
                item.ViewModel.Controller.ShowRadWindow(item, "Pick SKU...");
            }
        }
        #endregion

        #region Private Methods
        void getFRCategories()
        {
            try
            {
                SetStatus("Loading data, please wait...");
                if (!_isEvent)
                    Services.BasicService.GetCategoryList(string.Empty, string.Empty, FRCategoryTypeEnum.BusinessTravel, null, getFRCategoies_Completed);
                else if (_isEvent)
                    Services.BasicService.GetCategoryList(string.Empty, string.Empty, FRCategoryTypeEnum.BusinessEvent, true, getFRCategoies_Completed);
            }
            catch (Exception e)
            {
                ResetStatus();
                throw new Exception(e.Message, e);
            }
        }
        void loadAllItems()
        {
            Code = _tempTravelOrEvent.TravelCode;
            Name = _tempTravelOrEvent.EventName;
            IsEvent = _tempTravelOrEvent.IsEvent;
            if (_tempTravelOrEvent.IsEvent)
            {
                DateOfEvent = _tempTravelOrEvent.TravDate ?? DateTime.Now;
                NoOfParticipant = _tempTravelOrEvent.NoOfParticipant;
                Venue = _tempTravelOrEvent.Venue;
            }
            Purpose = _tempTravelOrEvent.Purpose;
            SelectedTravelTypeItem = TravelTypeItems.Where(x => x.PKID == _tempTravelOrEvent.TravelTypeItem.PKID).FirstOrDefault();
            if (SelectedTravelTypeItem != null) SelectedTravelTypeItemID = SelectedTravelTypeItem.PKID;            
            TravelOrEventEstimatedCostModelItems = TravelOrEventEstimatedCostAssembler<TravelOrEventEstimatedCost, TravelOrEventEstimatedCostModel>.AssembleContracts(_tempTravelOrEvent.TravelOrEventEstimatedCostItems);
            var temp = TravelOrEventPlanAssembler<TravelOrEventPlan, TravelOrEventPlanModel>.AssembleContracts(_tempTravelOrEvent.TravelOrEventPlanItems); ;
            foreach (var itm in temp)
                TravelOrEventPlanModelItems.Add(itm);
            AdvanceAmount = _tempTravelOrEvent.AdvanceAmount;

            CurrencyItem = CurrencyItems.Where(x => x.PKID == _tempTravelOrEvent.AdvanceCurrency.PKID).FirstOrDefault();
            ConversionRate = _tempTravelOrEvent.ConversionRate;
            AdvanceDateRequired = _tempTravelOrEvent.AdvanceDateRequired;
            AdvanceDateRefund = _tempTravelOrEvent.AdvanceDateRefund;
            IsAdvanceCash = _tempTravelOrEvent.IsAdvanceCash;
            IsAdvanceCredit = _tempTravelOrEvent.IsAdvanceCredit;
            FRType = FinancialRequisitionViewModelItem.FRType;           
            TravelOrEventModelItem = TravelAuthReqAssembler<TravelOrEvent, TravelOrEventModel>.AssembleContract(_tempTravelOrEvent);
        }
       
        void addTravelOrEventPlanItemCommand(TravelOrEventPlanModel parm)
        {
            try
            {
                EmployeePickerView item = new EmployeePickerView();
                item.ViewModel = new EmployeePickerViewModel(true);
                item.SetSelectColVisibility();
                item.ViewModel.ItemsSelectionComplete += new OnCompletion<SmartObservableCollection<EmployeeModel>>(employeeViewModel_ItemsSelectionComplete);
                item.ViewModel.Controller = new RadWindowController();
                item.ViewModel.Controller.ShowRadWindow(item, "Pick Employee...");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
        void deleteTravelOrEventPlanItemCommand(TravelOrEventPlanModel parm)
        {
            if (parm.TravellerEmployeeID > 0 && IsEvent)
                TravelOrEventPlanModelItems.Remove(parm);

            else if (_travelOrEventPlanModelItems.Count > 0 && !IsEvent)
                TravelOrEventPlanModelItems.Remove(parm);
        }
        void GetInventoryTypes()
        {
            try
            {
                SetStatus("Loading data, please wait...");
                Services.BasicService.GetAllInventoryTypes(FilterEnum.NoFilter, "NI", CodeOf2Enum.Code1, string.Empty, getinventoryTypes_Colmpleted);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
        void addCostItemCommand(TravelOrEventEstimatedCostModel parm)
        {
            try
            {
              
                if(_isEvent && SelectedTravelTypeItem==null)
                {
                    MessageBox.Show("Please Select a Event Type.","Warnning",MessageBoxButton.OK);
                    return;
                }
                else if(!_isEvent && SelectedTravelTypeItem==null)
                {
                    MessageBox.Show("Please Select a Travel Type.", "Warnning", MessageBoxButton.OK);
                    return;
                }
                Services.BasicService.GetCategoryChildItems(SelectedTravelTypeItem.PKID, getCategoryChildItems_Completed);        
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        
        void deleteCostItemCommand(TravelOrEventEstimatedCostModel parm)
        {
            if (parm.ItemID > 0)
                TravelOrEventEstimatedCostModelItems.Remove(parm);

        }
        void addTravel()
        {
            _travelOrEventPlanModelItems.Add(new TravelOrEventPlanModel() { FromDate = DateTime.Now, ToDate = DateTime.Now });
            base.RaisePropertiesChanged("TravelOrEventPlanModelItems");
        }
        void calculate(TravelOrEventEstimatedCostModel parm)
        {
            TotalEstAmount = _travelOrEventEstimatedCostModelItems.Sum(x => x.Amount) ?? 0;
        }
        void saveCommand()
        {
            try
            {
                base.SetStatus("Saving...,Please Wait");    
                refreshItems();
                var temp = TravelAuthReqAssembler<TravelOrEvent, TravelOrEventModel>.AssembleContract(TravelOrEventModelItem);               
                Services.BasicService.SaveTravelOrEvent(temp, User.CurrentUser, travelORevent_saveCompleted);
            }
            catch (Exception e)
            {
                base.ResetStatus();
                throw new Exception(e.Message, e);
            }
        }
        bool recheckErrors(out string message)
        {
            bool hasFault = false;
            List<string> msgs = new List<string>();
            message = string.Empty;
            if (_isEvent)
            {
                if (NoOfParticipant <= 0)
                {
                    msgs.Add("No of Participant should be grater than zero.");
                    hasFault = true;
                }
                if (_travelOrEventPlanModelItems.Count > 1 && _travelOrEventPlanModelItems
                                                              .Where(x => (x.ModeOfTravel == null && x.TravellerEmployeeID > 0) ||
                                                                          (x.ModeOfTravel == ModeOfTravelEnum.None && x.TravellerEmployeeID > 0)).Any())
                {
                    msgs.Add("Please Insert Proper Mode Of Travel in Travel or Event Plan.");
                    hasFault = true;
                }
            }
            if (!_isEvent)
            {
                if (SelectedTravelType == TravelTypesEnum.None)
                {
                    msgs.Add("Please Select a travel type.");
                    hasFault = true;
                }
                if (_travelOrEventPlanModelItems.Where(x => (x.PKID != -1 && x.ModeOfTravel == null) || (x.PKID != -1 && x.ModeOfTravel == ModeOfTravelEnum.None)).Any())
                {
                    msgs.Add("Please Insert Proper Mode Of Travel in Travel or Event Plan.");
                    hasFault = true;
                }

            }
            if (_travelOrEventEstimatedCostModelItems.Count > 1 && _travelOrEventEstimatedCostModelItems.Where(x => x.Amount <= 0).Any())
            {
                msgs.Add("Please Insert Proper amount in estimated cost.");
                hasFault = true;
            }
            if (AdvanceAmount <= 0)
            {
                msgs.Add("Please Insert Proper Advance Amount.");
                hasFault = true;
            }
            if (CurrencyItem == null)
            {
                msgs.Add("Please Select a Currency Item.");
                hasFault = true;
            }
            if (ConversionRate <= 0)
            {
                msgs.Add("Please Insert Proper Convertion Rate.");
                hasFault = true;
            }
            return hasFault;
        }
        void refreshItems()
        {
            if (TravelOrEventModelItem == null) TravelOrEventModelItem = new TravelOrEventModel();
            TravelOrEventModelItem.TravelCode = Code;
            TravelOrEventModelItem.EventName = Name;
            TravelOrEventModelItem.TravDate = DateOfEvent;
            TravelOrEventModelItem.Purpose = Purpose;
            TravelOrEventModelItem.NoOfParticipant = NoOfParticipant;
            TravelOrEventModelItem.Venue = Venue;
            //TravelOrEventModelItem.TravelType = SelectedTravelType;
            TravelOrEventModelItem.TravelTypeItem = SelectedTravelTypeItem;

            TravelOrEventModelItem.TravelOrEventEstimatedCostModelItems = TravelOrEventEstimatedCostModelItems;
            TravelOrEventModelItem.TravelOrEventPlanModelItems = TravelOrEventPlanModelItems;

            TravelOrEventModelItem.AdvanceAmount = AdvanceAmount;
            TravelOrEventModelItem.AdvanceCurrency = CurrencyAssembler<Currency, CurrencyModel>.AssembleContract(CurrencyItem);
            TravelOrEventModelItem.ConversionRate = ConversionRate;
            TravelOrEventModelItem.AdvanceDateRequired = AdvanceDateRequired;
            TravelOrEventModelItem.AdvanceDateRefund = AdvanceDateRefund;
            TravelOrEventModelItem.IsAdvanceCash = IsAdvanceCash;
            TravelOrEventModelItem.IsAdvanceCredit = IsAdvanceCredit;
            TravelOrEventModelItem.FRType = FRType;
            TravelOrEventModelItem.IsEvent = IsEvent;
            TravelOrEventModelItem.FRModelItem = FinancialRequisitionModelItem;
            if (!IsEvent && TravelOrEventModelItem.PKID <= 0)
            {
                TravelOrEventModelItem.TravDate = null;
                TravelOrEventModelItem.TravellerUserID = User.CurrentUser.PKID;
            }

            if (TravelOrEventModelItem.PKID <= 0)
            {
                TravelOrEventModelItem.CreatedBy = User.CurrentUser.PKID;
            }
            if (TravelOrEventModelItem.PKID > 0)
            {
                TravelOrEventModelItem.ModifiedBy = User.CurrentUser.PKID;

            }
        }
        void loadCurrencyItems()
        {
            try
            {
                SetStatus("Loading Currencies, please wait...");
                Services.BasicService.GetCurrencies(FilterEnum.Active, string.Empty, CodeISOOf2Enum.None, string.Empty, getCurrency_Completed);
            }
            catch (Exception e)
            {
                ResetStatus();
                throw new Exception(e.Message, e);
            }
        }
        void closeView()
        {
            Cleanup();
            RadWindowController.CloseRadWindow();
        }

        #endregion
    }

    public class TravelReportViewModel : BaseViewModel, ITravelReportViewModel
    {
        #region Private fields

        //private int _count;
        //private string _code;
        //private string _name;

        //private FinancialRequisitionCategory _finReqCat;
        //private FilterEnum _filter;
        private RelayCommand _addItemCommand;
        private RelayCommand<TravelOrEventReportModel> _deleteCommand;
        private RelayCommand<string> _calculateCommand;
        //private RelayCommand _pickItemHeirarchyCommand;
        //private RelayCommand _clearItemHeirarchyCommand;
        private RelayCommand _closeCommand;
        private RelayCommand _saveCommand;
        //private RelayCommand _refreshCommand;
        private RelayCommand _pickTravRequtionCommand;
        private SmartObservableCollection<SKUViewModel> _sKUViewModelItems;
        //private DepartmentViewModel _item;
        private List<MapTravelItemModel> _mapedItems;
        private ObjectsTemplate<FinancialRequesitionItem> _fRItems;
        //private int ftseq = 1, mVisitseq = 1, bVisitseq = 1, cVisitseq = 1, relationseq = 1;
        List<MapTravelItemModel> _mapModels;
        //private event OnCompletion<long> DeleteColmpleted;

        //public event OnCompletion<bool> ItemSaveColmpleted;

        #endregion

        #region Constructor

        public TravelReportViewModel()
            : base()
        {
            Initialize();
        }

        #endregion

        #region Public Properties

        #region CultureWithFormattedPeriod : CultureInfo
        public CultureInfo CultureWithFormattedPeriod
        {
            get //added by srijon
            {
                var tempCultureInfo = new CultureInfo("en-US");
                tempCultureInfo.DateTimeFormat.ShortDatePattern = "dd MMM yyyy";
                return tempCultureInfo;
            }
        }
        #endregion

        #region IsAccepted : bool
        private bool isAccepted;

        public bool IsAccepted
        {
            get { return isAccepted; }
            set
            {
                isAccepted = value;
                base.RaisePropertyChanged("IsAccepted");
            }
        }

        #endregion

        #region Code: string
        public string Code
        {
            get
            {
                return _financialRequisitionModelItem != null ? _financialRequisitionModelItem.RequisitionNo : string.Empty;
            }

        }
        #endregion

        #region SumCalculatedTotal: decimal
        private decimal _sumCalculatedTotal;
        public decimal SumCalculatedTotal
        {
            get { return _sumCalculatedTotal; }
            set
            {
                _sumCalculatedTotal = value;
                base.RaisePropertyChanged("SumCalculatedTotal");
            }
        }
        #endregion

        #region Items : TravelOrEventReportModel
        private TravelOrEventModel _travOrEvitem;
        public TravelOrEventModel TravOrEvitem
        {
            get { return _travOrEvitem; }
            set
            {
                if (_travOrEvitem != value)
                {
                    _travOrEvitem = value;
                    RaisePropertyChanged("TravOrEvitem");
                }
            }
        }
        #endregion

        #region property FinancialRequisitionViewModelItem : FinancialRequisitionViewModel

        private FinancialRequisitionViewModel _financialRequisitionViewModelItem;
        public FinancialRequisitionViewModel FinancialRequisitionViewModelItem
        {
            get { return _financialRequisitionViewModelItem; }
            set
            {
                _financialRequisitionViewModelItem = value;
                RaisePropertyChanged("FinancialRequisitionViewModelItem");
            }
        }

        #endregion

        #region Items : SmartObservableCollection<TravelOrEventReportModel>
        private SmartObservableCollection<TravelOrEventReportModel> _items;
        public SmartObservableCollection<TravelOrEventReportModel> Items
        {
            get { return _items; }
            set
            {
                if (_items != value)
                {
                    _items = value;
                    RaisePropertyChanged("Items");
                }
            }
        }
        #endregion

        #region TravelOrEventReportModel : TravelOrEventReportModel
        private TravelOrEventReportModel _travelOrEventReportModel;
        public TravelOrEventReportModel TravelOrEventReportModelItem
        {
            get { return _travelOrEventReportModel; }
            set
            {

                _travelOrEventReportModel = value;
                RaisePropertyChanged("TravelOrEventReportModelItem");

            }
        }
        #endregion

        #region Property TravelTypesItems: Dictionary<TravelTypesEnum, string>

        private Dictionary<TravelTypesEnum, string> _travelTypesItems;
        public Dictionary<TravelTypesEnum, string> TravelTypesItems
        {
            get
            {
                if (_travelTypesItems != null) return _travelTypesItems;
                _travelTypesItems = new Dictionary<TravelTypesEnum, string>();
                foreach (TravelTypesEnum value in Enum.GetValues(typeof(TravelTypesEnum)))
                {
                    _travelTypesItems.Add(value, EnumDescription.GetEnumDescription(value));
                }
                return _travelTypesItems;
            }
        }
        #endregion

        #region Property SelectedTravelTypesItem: TravelTypesItemEnum

        private TravelTypesEnum _selectedTravelTypesItem;
        public TravelTypesEnum SelectedTravelTypesItem
        {
            get
            {
                return _selectedTravelTypesItem;
            }
            set
            {
                _selectedTravelTypesItem = value;
                base.RaisePropertiesChanged("SelectedTravelTypesItem");
            }
        }
        #endregion

        #region Property ModeOfTravelItems: Dictionary<ModeOfTravelEnum, string>

        private Dictionary<ModeOfTravelEnum, string> _modeOfTravelItems;
        public Dictionary<ModeOfTravelEnum, string> ModeOfTravelItems
        {
            get
            {
                if (_modeOfTravelItems != null) return _modeOfTravelItems;
                _modeOfTravelItems = new Dictionary<ModeOfTravelEnum, string>();
                foreach (ModeOfTravelEnum value in Enum.GetValues(typeof(ModeOfTravelEnum)))
                {
                    _modeOfTravelItems.Add(value, EnumDescription.GetEnumDescription(value));
                }
                return _modeOfTravelItems;
            }
        }
        #endregion

        #region TravelOrEventModelItem:TravelOrEventModel
        private TravelOrEventModel _travelOrEventModelItem;

        public TravelOrEventModel TravelOrEventModelItem
        {
            get { return _travelOrEventModelItem; }
            set
            {
                _travelOrEventModelItem = value;
                base.RaisePropertiesChanged("TravelOrEventModelItem");
            }
        }
        #endregion

        #region TravelDetails :string
        private string _travelDetails;
        public string TravelDetails
        {
            get
            {
                return _travelDetails;
            }
            set
            {
                _travelDetails = value;
                base.RaisePropertyChanged("TravelDetails");
            }
        }

        #endregion

        #region FinancialRequisitionModelItem : FinancialRequisitionModel

        private FinancialRequisitionModel _financialRequisitionModelItem;
        public FinancialRequisitionModel FinancialRequisitionModelItem
        {
            get { return _financialRequisitionModelItem; }
            set
            {
                _financialRequisitionModelItem = value;
                base.RaisePropertiesChanged("FinancialRequisitionModelItem", "Code");
            }
        }

        #endregion

        #region FinancialRequisitionItems : SmartObservableCollection<FinancialRequesitionItemModel>

        private SmartObservableCollection<FinancialRequesitionItemModel> _financialRequisitionItems;
        public SmartObservableCollection<FinancialRequesitionItemModel> FinancialRequisitionItems
        {
            get { return _financialRequisitionItems; }
            set
            {
                if (_financialRequisitionItems == null)
                    _financialRequisitionItems = new SmartObservableCollection<FinancialRequesitionItemModel>();
                _financialRequisitionItems = value;
                base.RaisePropertiesChanged("FinancialRequisitionItems");
            }
        }

        #endregion

        #region SelectedTravelTypeItem : FinancialRequisitionCategoryModel
        private FinancialRequisitionCategoryModel _selectedTravelTypeItem;
        public FinancialRequisitionCategoryModel SelectedTravelTypeItem
        {
            get { return _selectedTravelTypeItem; }
            set
            {
                _selectedTravelTypeItem = value;
                base.RaisePropertyChanged("SelectedTravelTypeItem");
            }
        }
        #endregion

        #region TravelTypeItems : SmartObservableCollection<FinancialRequisitionCategoryModel>
        private SmartObservableCollection<FinancialRequisitionCategoryModel> _TravelTypeItems;
        public SmartObservableCollection<FinancialRequisitionCategoryModel> TravelTypeItems
        {
            get
            {
                return _TravelTypeItems;
            }
            set
            {
                _TravelTypeItems = value;
                base.RaisePropertyChanged("TravelTypeItems");
            }
        }
        #endregion

        #region Controller
        public ITravelReportController Controller
        {
            get;
            set;
        }
        public RadWindowController RadWindowController
        {
            get;
            set;
        }

        #endregion

        #endregion

        #region RelayCommand
        public RelayCommand AddItem
        {
            get { return _addItemCommand ?? (_addItemCommand = new RelayCommand(() => addItem())); }
        }

        public RelayCommand CloseView
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(() => closeView())); }
        }
        public RelayCommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new RelayCommand(() => saveItems())); }
        }

        public RelayCommand PickTravRequtionCommand
        {
            get { return _pickTravRequtionCommand ?? (_pickTravRequtionCommand = new RelayCommand(() => pickTravReqution())); }
        }
        //public RelayCommand ClearItemHeirarchy
        //{
        //    get { return _clearItemHeirarchyCommand ?? (_clearItemHeirarchyCommand = new RelayCommand(() => clearItemHeirarchy())); }
        //}

        //public RelayCommand PickItemHeirarchy
        //{
        //    get { return _pickItemHeirarchyCommand ?? (_pickItemHeirarchyCommand = new RelayCommand(() => pickItemHeirarchy())); }
        //}

        public RelayCommand<TravelOrEventReportModel> DeleteItem
        {
            get { return _deleteCommand ?? (_deleteCommand = new RelayCommand<TravelOrEventReportModel>((parm) => deleteItem(parm))); }
        }
        public RelayCommand<string> CalculateCommand
        {
            get { return _calculateCommand ?? (_calculateCommand = new RelayCommand<string>((parm) => calculate(parm))); }
        }

        #endregion

        #region Allow permission

        #region AllowAdd: string

        public string AllowAdd
        {
            get { return VisibilityHelper.ConvertFrom(User.IsLoggedIn && User.CurrentUser.IsPermitted(KeyConstatns.Department, PermissionTypeEnum.Create)); }
        }

        #endregion

        #region AllowEdit: string

        public string AllowEdit
        {
            get { return VisibilityHelper.ConvertFrom(User.IsLoggedIn && User.CurrentUser.IsPermitted(KeyConstatns.Department, PermissionTypeEnum.Modify)); }
        }

        #endregion

        #region AllowDelete: string

        public string AllowDelete
        {
            get { return VisibilityHelper.ConvertFrom(User.IsLoggedIn && User.CurrentUser.IsPermitted(KeyConstatns.Department, PermissionTypeEnum.Delete)); }
        }

        #endregion

        #endregion

        #region Public Methods
        public void GetFinancialSKU()
        {
            try
            {
                _mapModels = new List<MapTravelItemModel>();
                // Corporate Visit Items Static Temp Mapping 
                _mapModels.Add(new MapTravelItemModel(1, 18, TravelTypesEnum.CorporateVisit, 3485, "TRAVEL FARE"));
                _mapModels.Add(new MapTravelItemModel(2, 18, TravelTypesEnum.CorporateVisit, 3486, "TRAVEL FARE"));
                _mapModels.Add(new MapTravelItemModel(3, 18, TravelTypesEnum.CorporateVisit, 3487, "TRAVEL FARE"));
                _mapModels.Add(new MapTravelItemModel(4, 18, TravelTypesEnum.CorporateVisit, 3488, "TRAVEL FARE"));
                _mapModels.Add(new MapTravelItemModel(5, 18, TravelTypesEnum.CorporateVisit, 3489, "TRAVEL FARE"));
                //_mapModels.Add(new MapTravelItemModel(1, 18, TravelTypesEnum.CorporateVisit, 3485, "TRAVEL FARE"));
                _mapModels.Add(new MapTravelItemModel(6, 18, TravelTypesEnum.CorporateVisit, 3491, "TRAVEL FARE"));



                //SetStatus("Loading items,please wait...");
                //Services.BasicService.GetSKUs(null, FilterEnum.NoFilter, string.Empty, string.Empty, AvailabilityEnum.Both, null, false, new List<long>() { 252 }, 0, null, string.Empty, getFinancialSKU_Completed);
            }
            catch (Exception e)
            {
                ResetStatus();
                throw new Exception(e.Message, e);
            }
        }
        public void Initialize()
        {
            _travelOrEventReportModel = new TravelOrEventReportModel();
            _travelOrEventModelItem = new TravelOrEventModel();//force Insert Before Make Picker. 
            _items = new SmartObservableCollection<TravelOrEventReportModel>();
            _items.Add(new TravelOrEventReportModel() { TravelDate = DateTime.Now });
            _mapedItems = new List<MapTravelItemModel>();
            _selectedTravelTypeItem = new FinancialRequisitionCategoryModel() { PKID = -1 };
            _financialRequisitionModelItem = new FinancialRequisitionModel() { PKID = -1 };
            GetFinancialSKU();
            //GetInventoryTypes();
            getFRCategories();
            base.RaisePropertiesChanged("Items", "TravelOrEventReportModelItem");
        }

        public override void Cleanup()
        {
        }
        public void LoadItemsForEdit()
        {

        }
        public void RaiseAllProperties()
        {
            base.RaisePropertiesChanged("Items", "Name", "Code", "ItemHierarchyItems");
        }

        #endregion

        #region Private Callback functions
        void getFRCategoies_Completed(ObjectsTemplate<FinancialRequisitionCategory> retValue, Exception error)
        {
            ResetStatus();
            if (error != null)
                throw error;
            if (retValue != null)
            {
                TravelTypeItems = FinancialRequesitionCategoryAssembler<FinancialRequisitionCategory, FinancialRequisitionCategoryModel>.AssembleContracts(retValue);
            }
        }
        void getFinancialSKU_Completed(ObjectsTemplate<SKU> results, Exception error)
        {
            try
            {

                if (error != null)
                    throw error;

                _sKUViewModelItems = SKUAssembler<SKU, SKUViewModel>.AssembleContracts(results);

                foreach (var itm in _sKUViewModelItems)
                {
                    //if (itm.HierarchyNodeName.ToLower().Contains("foreign travel"))
                    //{
                    //    _mapedItems.Add(new MapTravelItemModel(ftseq, TravelTypesEnum.ForeignTravel, itm.PKID, itm.ItemName));
                    //    ftseq++;
                    //}
                    //else if (itm.HierarchyNodeName.ToLower().Contains("market visit"))
                    //{
                    //    _mapedItems.Add(new MapTravelItemModel(mVisitseq, TravelTypesEnum.MarketVisit, itm.PKID, itm.ItemName));
                    //    mVisitseq++;
                    //}
                    //else if (itm.HierarchyNodeName.ToLower().Contains("branch visit"))
                    //{
                    //    _mapedItems.Add(new MapTravelItemModel(bVisitseq, TravelTypesEnum.BranchVisit, itm.PKID, itm.ItemName));
                    //    bVisitseq++;
                    //}
                    ////else if (itm.HierarchyNodeName.ToLower().Contains("corporate visit"))
                    //else if (itm.HierarchyNodeName.ToLower().Contains("coporate visit"))
                    //{
                    //    _mapedItems.Add(new MapTravelItemModel(cVisitseq, TravelTypesEnum.CorporateVisit, itm.PKID, itm.ItemName));
                    //    cVisitseq++;
                    //}
                    //else if (itm.HierarchyNodeName.ToLower().Contains("relocation"))
                    //{
                    //    _mapedItems.Add(new MapTravelItemModel(relationseq, TravelTypesEnum.Relocation, itm.PKID, itm.ItemName));
                    //    relationseq++;
                    //}
                }
            }
            catch (Exception e)
            {
                ResetStatus();
                throw new Exception(e.Message, e);
            }
            finally
            {
                ResetStatus();
                error = null;
                results.Clear();
            }
        }
        private void financialRequisitionPicker_ItemsSelectionCompleted(FinancialRequisitionModel result, Exception error)
        {
            if (error != null)
                throw error;
            if (result != null)
            {
                FinancialRequisitionModelItem = result;
                try
                {
                    SetStatus("Loading data, please wait...");
                    Services.BasicService.GetCategoryByFinReqID(_financialRequisitionModelItem.PKID, getCategoryByFinReqID_Completed);
                }
                catch (Exception e)
                {
                    ResetStatus();
                    throw new Exception(e.Message, e);
                }
                //_travelOrEventModelItem.FRType = result.FRType;
                //_travelOrEventModelItem.Purpose = result.Purpose;
                //_travelOrEventModelItem.Description = result.Remarks;
                //_travelOrEventModelItem.CreatedBy = result.CreatedBy;
                //_travelOrEventModelItem.PKID = result.ReferenceID ?? -1;
                //_travelOrEventModelItem.CreatedDate =result.CreatedDate;
                //_travelOrEventModelItem.CreatedBy = result.RequisitionRaisedByID;
                //_travelOrEventModelItem.AdvanceAmount = (decimal)result.Advance ;
                //_travelOrEventModelItem.TravelType = FRCategoryTypeEnum.Travel;
            }
        }

        void getCategoryByFinReqID_Completed(FinancialRequisitionCategory result, Exception error)
        {
            base.ResetStatus();
            if (error != null)
                throw error;
            if (error == null && result != null)
            {
                SelectedTravelTypeItem = TravelTypeItems.Where(x => x.PKID == result.PKID).FirstOrDefault();
                SelectedTravelTypeItem.TravelorEventID = result.TravelorEventID;
            }
        }
        void saveTravelOrEventReport_Completed(long retValue, Exception error)
        {
            base.ResetStatus();
            if (error != null)
                throw error;
            if (error == null)
            {
                MessageBox.Show("Save Successful", "Success", MessageBoxButton.OK);
                closeView();
            }
        }
        #endregion

        #region Private Methods
        void getFRCategories()
        {
            try
            {
                SetStatus("Loading data, please wait...");
                Services.BasicService.GetCategoryList(string.Empty, string.Empty, FRCategoryTypeEnum.None, null, getFRCategoies_Completed);
            }
            catch (Exception e)
            {
                ResetStatus();
                throw new Exception(e.Message, e);
            }
        }
        void addItem()
        {
            var temp = new TravelOrEventReportModel();
            temp.TravelTypesItem = SelectedTravelTypesItem;
            temp.TravelDate = DateTime.Now;
            if (TravOrEvitem != null)
                temp.TravelOrEventID = TravOrEvitem.PKID;
            else
                temp.TravelOrEventID = 0;
            _items.Add(temp);
            base.RaisePropertyChanged("Items");


            ////Market Visit
            //mapedItems.Add(new MapTravelItem(1, 1, 1));
            //mapedItems.Add(new MapTravelItem(1, 2, 1));
            //mapedItems.Add(new MapTravelItem(1, 3, 1));
            //mapedItems.Add(new MapTravelItem(1, 4, 1));
            //mapedItems.Add(new MapTravelItem(1, 5, 1));
            //mapedItems.Add(new MapTravelItem(1, 6, 1));

            ////Foreign Travel
            //mapedItems.Add(new MapTravelItem(2, 1, 1));
            //mapedItems.Add(new MapTravelItem(2, 2, 1));
            //mapedItems.Add(new MapTravelItem(2, 3, 1));
            //mapedItems.Add(new MapTravelItem(2, 4, 1));
            //mapedItems.Add(new MapTravelItem(2, 5, 1));


            //// while submit
            //// get item TravelType dropdown value
            //// Item sequence

            //FinancialRequesitionItem item = new FinancialRequesitionItem();

        }
        void deleteItem(TravelOrEventReportModel parm)
        {
            _items.Remove(parm);
            if (Items.Count <= 0)
            {
                SumCalculatedTotal = 0;
            }
            else
            {
                calculate(string.Empty);
            }
        }
        void calculate(string parm)
        {
            if (Items.Count > 0)
            {
                Items.ToList().ForEach(itm =>
                {
                    itm.Total = (itm.Others ?? 0) + (itm.Food ?? 0) + (itm.Fare ?? 0) + (itm.Entertainment ?? 0) + (itm.Convence ?? 0) + (itm.Accomodation ?? 0);
                });
                SumCalculatedTotal = Items.Sum(x => x.Total) ?? 0;
            }
        }
        void closeView()
        {
            _items.Clear();
            Cleanup();
            RadWindowController.CloseRadWindow();
        }
        void saveItems()
        {
            try
            {
                if (_items.Count <= 0)
                {
                    MessageBox.Show("Please Insert atleast one Item.", "Warnning", MessageBoxButton.OK);
                    return;
                }
                if (TravelOrEventReportModelItem == null)
                {
                    MessageBox.Show("Please Select Travel Requisition.", "Warnning", MessageBoxButton.OK);
                    return;
                }
                if (_selectedTravelTypeItem.PKID <= 0)
                {
                    MessageBox.Show("Please Select a Travel Type.", "Warnning", MessageBoxButton.OK);
                    return;
                }
                if (FinancialRequisitionModelItem.PKID <= 0)
                {
                    MessageBox.Show("Please Pick a Requisition.", "Warnning", MessageBoxButton.OK);
                    return;
                }
                Items.ToList().ForEach(x =>
                {
                    x.TravelDetails = TravelDetails;
                    x.ParentFinReq = _financialRequisitionModelItem;
                    // x.TravelTypesItem = SelectedTravelTypesItem;
                    x.TravelOrEventID = SelectedTravelTypeItem.TravelorEventID ?? -1;
                    x.FinCategoryTravelType = SelectedTravelTypeItem;

                });
                makeFinitemsCollection();
                
                var temp = TravelReportAssembler<TravelOrEventReport, TravelOrEventReportModel>.AssembleContracts(Items);
                SetStatus("Saving, please wait...");
                Services.BasicService.SaveTravelOrEventReport(_fRItems, temp, User.CurrentUser, saveTravelOrEventReport_Completed);
            }
            catch (Exception e)
            {
                ResetStatus();
                throw new Exception(e.Message, e);
            }
        }
        void pickTravReqution()
        {
            FinancialRequisitionPickerView item = new FinancialRequisitionPickerView();
            item.ViewModel = new FinancialRequisitionPickerViewModel();
            item.ViewModel.FRCategoryType = FRCategoryTypeEnum.BusinessTravel;
            item.ViewModel.RadController = new RadWindowController();
            item.ViewModel.IsTravelExpressReport = true;
            //FRCategoryTypeEnum.Travel;
            item.ViewModel.ItemSelectionCompleted += financialRequisitionPicker_ItemsSelectionCompleted;
            item.ViewModel.RadController.ShowRadWindow(item, "Financial Requisition Picker for Travel");

        }
        void makeFinitemsCollection()
        {


            if (_fRItems == null)
            {
                _fRItems = new ObjectsTemplate<FinancialRequesitionItem>();
            }
            _fRItems.Clear();

            #region Static FinReqItems Create for testing
            //Static Push One
            if (Items.Sum(x => x.Fare) > 0 && _financialRequisitionModelItem != null)
            {   //_fRItems.Add(new FinancialRequesitionItem()
                //{
                //    // FinancialRequisitionID = _financialRequisitionModelItem.PKID,

                //    SKUID = _mapModels.Where(x => x.ItemtypeID == SelectedTravelTypeItem.PKID && x.Travelcatsequence == 1).FirstOrDefault().SKUID,
                //    Amount = (Items.Sum(x => x.Fare ?? 0)),
                //    //AmountBDT=(Items.Sum(x => x.Fare??0)),
                //    BeneficiaryTypeID = User.CurrentUser.EmployeeID,
                //    BeneficiaryType = PaymentToTypeEnum.Employee

                //});
                var temp = new FinancialRequesitionItem();
                temp.SKUID = _mapModels.Where(x => x.ItemtypeID == SelectedTravelTypeItem.PKID && x.Travelcatsequence == 1).FirstOrDefault().SKUID;
                temp.Amount = (Items.Sum(x => x.Fare ?? 0));
                temp.BeneficiaryTypeID = User.CurrentUser.EmployeeID;
                temp.BeneficiaryType = PaymentToTypeEnum.Employee;
                _fRItems.Add(temp);
            }

            //Static Push Two
            if (Items.Sum(x => x.Accomodation) > 0 && _financialRequisitionModelItem != null)
                _fRItems.Add(new FinancialRequesitionItem()
                {
                    //FinancialRequisitionID = _financialRequisitionModelItem.PKID,
                    SKUID = _mapModels.Where(x => x.ItemtypeID == SelectedTravelTypeItem.PKID && x.Travelcatsequence == 2).FirstOrDefault().SKUID,
                    Amount = (Items.Sum(x => x.Accomodation ?? 0)),
                    //AmountBDT=(Items.Sum(x => x.Fare??0)),
                    BeneficiaryTypeID = User.CurrentUser.EmployeeID,
                    BeneficiaryType = PaymentToTypeEnum.Employee

                });
            //Static Push Three
            if (Items.Sum(x => x.Food) > 0 && _financialRequisitionModelItem != null)
                _fRItems.Add(new FinancialRequesitionItem()
                {
                    //FinancialRequisitionID = _financialRequisitionModelItem.PKID,
                    SKUID = _mapModels.Where(x => x.ItemtypeID == SelectedTravelTypeItem.PKID && x.Travelcatsequence == 3).FirstOrDefault().SKUID,
                    Amount = (Items.Sum(x => x.Food ?? 0)),
                    //AmountBDT=(Items.Sum(x => x.Fare??0)),
                    BeneficiaryTypeID = User.CurrentUser.EmployeeID,
                    BeneficiaryType = PaymentToTypeEnum.Employee

                });
            //Static Push Four
            if (Items.Sum(x => x.Entertainment) > 0 && _financialRequisitionModelItem != null)
                _fRItems.Add(new FinancialRequesitionItem()
                {
                    //FinancialRequisitionID = _financialRequisitionModelItem.PKID,
                    SKUID = _mapModels.Where(x => x.ItemtypeID == SelectedTravelTypeItem.PKID && x.Travelcatsequence == 4).FirstOrDefault().SKUID,
                    Amount = (Items.Sum(x => x.Entertainment ?? 0)),
                    //AmountBDT=(Items.Sum(x => x.Fare??0)),
                    BeneficiaryTypeID = User.CurrentUser.EmployeeID,
                    BeneficiaryType = PaymentToTypeEnum.Employee

                });

            //Static Push Five
            if (Items.Sum(x => x.Convence) > 0 && _financialRequisitionModelItem != null)
                _fRItems.Add(new FinancialRequesitionItem()
                {
                    FinancialRequisitionID = _financialRequisitionModelItem.PKID,
                    SKUID = _mapModels.Where(x => x.ItemtypeID == SelectedTravelTypeItem.PKID && x.Travelcatsequence == 5).FirstOrDefault().SKUID,
                    Amount = (Items.Sum(x => x.Convence ?? 0)),
                    //AmountBDT=(Items.Sum(x => x.Fare??0)),
                    BeneficiaryTypeID = User.CurrentUser.EmployeeID,
                    BeneficiaryType = PaymentToTypeEnum.Employee

                });

            //Static Push Six
            if (Items.Sum(x => x.Others) > 0 && _financialRequisitionModelItem != null)
                _fRItems.Add(new FinancialRequesitionItem()
                {
                    //FinancialRequisitionID = _financialRequisitionModelItem.PKID,
                    SKUID = _mapModels.Where(x => x.ItemtypeID == SelectedTravelTypeItem.PKID && x.Travelcatsequence == 6).FirstOrDefault().SKUID,
                    Amount = (Items.Sum(x => x.Others ?? 0)),
                    //AmountBDT=(Items.Sum(x => x.Fare??0)),
                    BeneficiaryTypeID = User.CurrentUser.EmployeeID,
                    BeneficiaryType = PaymentToTypeEnum.Employee

                });
            #endregion
        }

        #endregion

        #region SubClass MapTravelItemModel
        public class MapTravelItemModel : BaseViewModel
        {

            #region TravelCategory : TravelTypesEnum
            private TravelTypesEnum _travelCategory;
            public TravelTypesEnum TravelCategory
            {
                get { return _travelCategory; }
                set
                {
                    _travelCategory = value;
                    base.RaisePropertyChanged("TravelCategory");
                }
            }
            #endregion

            #region Travelcatsequence : int
            private int _travelcatsequence;
            public int Travelcatsequence
            {
                get { return _travelcatsequence; }
                set
                {
                    _travelcatsequence = value;
                    base.RaisePropertyChanged("Travelcatsequence");
                }
            }
            #endregion

            #region SKUID : long
            private long _sKUID;
            public long SKUID
            {
                get { return _sKUID; }
                set
                {
                    _sKUID = value;
                    base.RaisePropertyChanged("SKUID");
                }
            }
            #endregion

            #region ItemtypeID : long
            private long _itemtypeID;
            public long ItemtypeID
            {
                get { return _itemtypeID; }
                set
                {
                    _itemtypeID = value;
                    base.RaisePropertyChanged("ItemtypeID");
                }
            }
            #endregion

            #region SKUName : string
            private string _sKUName;

            public string SKUName
            {
                get { return _sKUName; }
                set
                {
                    _sKUName = value;
                    base.RaisePropertyChanged("SKUName");
                }
            }
            #endregion

            public MapTravelItemModel(int travelcatsequence, long itemtypeID, TravelTypesEnum itemtype, long skuid, string skuname)
            {
                this.Travelcatsequence = travelcatsequence;
                this.SKUID = skuid;
                this.ItemtypeID = itemtypeID;
                this.TravelCategory = itemtype;
                this.SKUName = skuname;
            }
        }

        #endregion

    }
    public interface ITravelReportViewModel
    {
        ITravelReportController Controller { get; set; }
        void Initialize();
    }
}

