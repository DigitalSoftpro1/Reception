using Reception.Models;
using System;

public class TransactionVisitCalculatorService
{
    private readonly ReceptionDbContext _context;

    public TransactionVisitCalculatorService(ReceptionDbContext context)
    {
        _context = context;
    }

    public void Recalculate(int visitId)
    {
        var visit = _context.TransactionVisit.First(x => x.Id == visitId);

        var totalTreatments = _context.TransactionVisitTreatment
            .Where(x => x.TransactionVisitId == visitId && x.IsActive && !x.IsRefused)
            .Sum(x => x.PatientPrice);

        var totalPaid = _context.TransactionVisitPayment
            .Where(x => x.TransactionVisitId == visitId && x.IsActive)
            .Sum(x => x.PaidValue);

        visit.InvoiceTotal = totalTreatments;
        visit.AmountToPay = totalTreatments - visit.Discount;
        visit.TotalPaid = totalPaid;
        visit.Remain = visit.AmountToPay - totalPaid;

        _context.SaveChanges();
    }
}
