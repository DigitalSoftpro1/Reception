using Reception.Models;
using Reception.View_Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reception.Extensions
{
    public static class TransactionVisitPaymentMappingExtension
    {
        public static TransactionVisitPaymentViewModel ToViewModel(this TransactionVisitPayment model)
        {
            return new TransactionVisitPaymentViewModel
            {
                Id = model.Id,
                TransactionVisitId = model.TransactionVisitId,
                PaidDate = model.PaidDate,
                PaidType = model.PaidType,
                PaidValue = model.PaidValue,
                IsActive = model.IsActive
            };
        }

        public static TransactionVisitPayment ToModel(this TransactionVisitPaymentViewModel viewModel)
        {
            return new TransactionVisitPayment
            {
                Id = viewModel.Id,
                TransactionVisitId = viewModel.TransactionVisitId ?? 0,
                PaidDate = viewModel.PaidDate ?? DateTime.Now,
                PaidType = viewModel.PaidType,
                PaidValue = viewModel.PaidValue ?? 0,
                IsActive = viewModel.IsActive
            };
        }

        public static List<TransactionVisitPaymentViewModel> ToViewModelList(this List<TransactionVisitPayment> models) =>
            models.Select(x => x.ToViewModel()).ToList();

        public static List<TransactionVisitPayment> ToModelList(this List<TransactionVisitPaymentViewModel> viewModels) =>
            viewModels.Select(x => x.ToModel()).ToList();

        public static TransactionVisitPaymentFullViewModel ToFullViewModel(this TransactionVisitPayment model)
        {
            return new TransactionVisitPaymentFullViewModel
            {
                Id = model.Id,
                TransactionVisitId = model.TransactionVisitId,
                TransactionVisitPaymentList = new List<TransactionVisitPaymentViewModel> { model.ToViewModel() }
            };
        }

        public static TransactionVisitPayment ToModel(this TransactionVisitPaymentFullViewModel fullViewModel)
        {
            var payment = fullViewModel.TransactionVisitPaymentList != null && fullViewModel.TransactionVisitPaymentList.Count > 0
                ? fullViewModel.TransactionVisitPaymentList[0].ToModel()
                : new TransactionVisitPayment();

            payment.Id = fullViewModel.Id;
            payment.TransactionVisitId = fullViewModel.TransactionVisitId ?? 0;

            return payment;
        }

        public static List<TransactionVisitPaymentFullViewModel> ToFullViewModelList(this List<TransactionVisitPayment> models) =>
            models.Select(x => x.ToFullViewModel()).ToList();

        public static List<TransactionVisitPayment> ToModelList(this List<TransactionVisitPaymentFullViewModel> fullViewModels) =>
            fullViewModels.Select(x => x.ToModel()).ToList();
    }
}
