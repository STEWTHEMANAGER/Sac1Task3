using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sac1Task3
{
    class Textbooks
    {
        public Textbooks(string textbook, string subject, string purchasePrice, string salePrice, string rating)
        {
            this.Textbook = textbook;
            this.Subject = subject;
            this.PurchasePrice = purchasePrice;
            this.SalsePrice = salePrice;
            this.Rating = rating;
        }

        public string Textbook { get; set; }
        public string Subject { get; set; }
        public string PurchasePrice { get; set; }
        public string SalsePrice { get; set; }
        public string Rating { get; set; }
    }
}
