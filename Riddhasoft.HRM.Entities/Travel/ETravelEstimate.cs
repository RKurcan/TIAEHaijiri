using Riddhasoft.HRM.Entities.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.HRM.Entities.Travel
{
    public class ETravelEstimate
    {
        public int Id { get; set; }
        public ExpenseType ExpenseType { get; set; }
        public string Remark { get; set; }
        public Currency CurrencyPaidIn { get; set; }
        public decimal Amount { get; set; }
        public PaidBy PaidBy { get; set; }
        public int BranchId { get; set; }
        public int TravelRequestId { get; set; }

        public virtual ETravelRequest TravelRequest { get; set; }
    }
    public enum ExpenseType
    {
        HotelExpense,
        MealExpense,
        FuelExpense,
        BusinessTravelExpense,
        CharitableDonationExpense,
        GeneralOfficeSuppliesExpense,
    }
    public enum PaidBy
    {
        Office,
        Staff,
    }
}
