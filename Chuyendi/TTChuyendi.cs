using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chuyendi
{
    
    public class TTChuyendi
    {
        public static List<string> STATUS = new List<string>(){ "Chuẩn bị", "Đang đi", "Đến nơi", "Đang về", "Kết thúc" };

        public string Name { set; get; }
        public string Status { set; get; }
        public string Place { set; get; }
        public string ImgLink { set; get; }
        public List<Thanhvien> Members {set; get;}
        public double Total { set; get; }
        public double Avg { set; get; }

        // Update all infomation after change
        public void Update()
        {
            calcCost();
        } 

        private void calcCost()
        {
            // Calc Total
            Total = 0;
            foreach (var mem in Members)
            {
                mem.Update();
                Total += mem.Paid;
            }

            // Calc Avg
            Avg = 0;
            if (Members.Count > 0)
            {
                Avg = Total / Members.Count;
            }

            foreach (var mem in Members)
            {
                mem.Debt = Avg - mem.Paid;
            }
        }

    }


    public class Thanhvien
    {
        public string Name { set; get; }
        public double Paid { set; get; }
        public double Debt { set; get; }
        public List<Bill> Bills{ set; get; }

        public void Update()
        {
            Paid = 0;
            foreach (var bill in Bills)
            {
                Paid += bill.Cost;
            }
        }


    }

    public class Bill
    {
        public string Name { set; get; }
        public double Cost { set; get; }

        public Bill()
        {
        }

        public Bill( Bill bill)
        {
            this.Cost = bill.Cost;
            this.Name = bill.Name;
        }

        public Bill (string name, double cost )
        {
            Name = name;
            Cost = cost;
        }

    }
}
