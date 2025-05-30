using System.ComponentModel.DataAnnotations;

namespace SalesInformationSystem.Models
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }
        public DateOnly InvoiceDate { get; set; }
        public double PaymentStatus { get; set; }

        public ICollection<SalesOrder> SalesOrder { get; set; }
        public ICollection<Payment> Payment { get; set; }
    }
}
