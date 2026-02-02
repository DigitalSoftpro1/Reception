using Reception.Models;
using Reception.View_Model;

namespace Reception.Extensions
{
    public static class TransactionVisitMappingExtension
    {
        public static TransactionVisitViewModel ToViewModel(this TransactionVisit model)
        {
            return new TransactionVisitViewModel
            {
                Id = model.Id,
                VisitNo = model.VisitNo,
                VisitDate = model.VisitDate,
                PatientName = model.PatientName,
                ClinicName = model.ClinicName,
                VisitType = model.VisitType,
                InvoiceTotal = model.InvoiceTotal,
                Discount = model.Discount,
                AmountToPay = model.AmountToPay,
                TotalPaid = model.TotalPaid,
                Remain = model.Remain,
                Notes = model.Notes,
                IsActive = model.IsActive
            };
        }

        public static TransactionVisit ToModel(this TransactionVisitViewModel viewModel)
        {
            return new TransactionVisit
            {
                Id = viewModel.Id,
                VisitNo = viewModel.VisitNo,
                VisitDate = viewModel.VisitDate ?? DateTime.Now,
                PatientName = viewModel.PatientName,
                ClinicName = viewModel.ClinicName,
                VisitType = viewModel.VisitType,
                InvoiceTotal = viewModel.InvoiceTotal ?? 0,
                Discount = viewModel.Discount ?? 0,
                AmountToPay = viewModel.AmountToPay ?? 0,
                TotalPaid = viewModel.TotalPaid ?? 0,
                Remain = viewModel.Remain ?? 0,
                Notes = viewModel.Notes,
                IsActive = viewModel.IsActive
            };
        }

        public static List<TransactionVisitViewModel> ToViewModelList(this List<TransactionVisit> models)
        {
            return models.Select(x => x.ToViewModel()).ToList();
        }

        public static List<TransactionVisit> ToModelList(this List<TransactionVisitViewModel> viewModels)
        {
            return viewModels.Select(x => x.ToModel()).ToList();
        }

        public static TransactionVisitFullViewModel ToFullViewModel(this TransactionVisit model)
        {
            return new TransactionVisitFullViewModel
            {
                Id = model.Id,
                VisitNo = model.VisitNo,
                VisitDate = model.VisitDate,
                PatientName = model.PatientName,
                IsActive = model.IsActive,
                TransactionVisitList = new List<TransactionVisitViewModel> { model.ToViewModel() }
            };
        }

        public static TransactionVisit ToModel(this TransactionVisitFullViewModel fullViewModel)
        {
            var transactionVisit = fullViewModel.TransactionVisitList != null && fullViewModel.TransactionVisitList.Count > 0
                ? fullViewModel.TransactionVisitList[0].ToModel()
                : new TransactionVisit();

            transactionVisit.Id = fullViewModel.Id;
            transactionVisit.IsActive = fullViewModel.IsActive;

            return transactionVisit;
        }

        public static List<TransactionVisitFullViewModel> ToFullViewModelList(this List<TransactionVisit> models)
        {
            return models.Select(x => x.ToFullViewModel()).ToList();
        }

        public static List<TransactionVisit> ToModelList(this List<TransactionVisitFullViewModel> fullViewModels)
        {
            return fullViewModels.Select(x => x.ToModel()).ToList();
        }
    }
}
