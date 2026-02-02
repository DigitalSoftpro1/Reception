using Reception.Models;
using Reception.View_Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reception.Extensions
{
    public static class TransactionVisitTreatmentMappingExtension
    {
        public static TransactionVisitTreatmentViewModel ToViewModel(this TransactionVisitTreatment model)
        {
            return new TransactionVisitTreatmentViewModel
            {
                Id = model.Id,
                TransactionVisitId = model.TransactionVisitId,
                TreatmentName = model.TreatmentName,
                OriginalPrice = model.OriginalPrice,
                PatientPrice = model.PatientPrice,
                IsRefused = model.IsRefused,
                IsActive = model.IsActive
            };
        }

        public static TransactionVisitTreatment ToModel(this TransactionVisitTreatmentViewModel viewModel)
        {
            return new TransactionVisitTreatment
            {
                Id = viewModel.Id,
                TransactionVisitId = viewModel.TransactionVisitId ?? 0,
                TreatmentName = viewModel.TreatmentName,
                OriginalPrice = viewModel.OriginalPrice ?? 0,
                PatientPrice = viewModel.PatientPrice ?? 0,
                IsRefused = viewModel.IsRefused ?? false,
                IsActive = viewModel.IsActive
            };
        }

        public static List<TransactionVisitTreatmentViewModel> ToViewModelList(this List<TransactionVisitTreatment> models) =>
            models.Select(x => x.ToViewModel()).ToList();

        public static List<TransactionVisitTreatment> ToModelList(this List<TransactionVisitTreatmentViewModel> viewModels) =>
            viewModels.Select(x => x.ToModel()).ToList();

        public static TransactionVisitTreatmentFullViewModel ToFullViewModel(this TransactionVisitTreatment model)
        {
            return new TransactionVisitTreatmentFullViewModel
            {
                Id = model.Id,
                TransactionVisitId = model.TransactionVisitId,
                TransactionVisitTreatmentList = new List<TransactionVisitTreatmentViewModel> { model.ToViewModel() }
            };
        }

        public static TransactionVisitTreatment ToModel(this TransactionVisitTreatmentFullViewModel fullViewModel)
        {
            var treatment = fullViewModel.TransactionVisitTreatmentList != null && fullViewModel.TransactionVisitTreatmentList.Count > 0
                ? fullViewModel.TransactionVisitTreatmentList[0].ToModel()
                : new TransactionVisitTreatment();

            treatment.Id = fullViewModel.Id;
            treatment.TransactionVisitId = fullViewModel.TransactionVisitId ?? 0;

            return treatment;
        }

        public static List<TransactionVisitTreatmentFullViewModel> ToFullViewModelList(this List<TransactionVisitTreatment> models) =>
            models.Select(x => x.ToFullViewModel()).ToList();

        public static List<TransactionVisitTreatment> ToModelList(this List<TransactionVisitTreatmentFullViewModel> fullViewModels) =>
            fullViewModels.Select(x => x.ToModel()).ToList();
    }
}
