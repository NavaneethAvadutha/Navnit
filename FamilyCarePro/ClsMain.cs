using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCarePro
{
    public class ClsMain
    {
        public long GetDataFrmDBT(long hwnd, long Product, long SelDate)
        {
            using (Form1 fm = new Form1())
            {
                fm.ShowDialog(); 
            }
            return 1;
        }
    }
}
