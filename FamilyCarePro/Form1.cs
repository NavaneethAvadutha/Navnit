using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using FOCUSAPILib;
using System.Threading.Tasks;
using System.Windows.Forms;
using FamilyCarePro.Classess;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;

namespace FamilyCarePro
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Boolean blnIsEditing = false;
        string mySql = "";
        string sqlserver2 = "";
        string sqlserver3 = "";
        string initialcatalog2 = "";
        string Companycode2 = "";
        string Username2 = "";
        string password2 = "";
        string initialcatalog3 = "";
        string Companycode3 = "";
        string Username3 = "";
        string password3 = "";
        Boolean blnErr_Log = false;
        DataSet ds, dsr;
        DataTable dt;
        SqlCommand ChkCmd;
        SqlDataAdapter da;
        string sAcc = "";
        string sTmp = "";
        string ChkConStr = "";
        string mySql1 = "";

        private void Form1_Load(object sender, EventArgs e)
        {
            //mySql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES";
            mySql = "select Name from dbo.mr002 where Name in ('Rev V','Canc Rev','Cr. Notes','Refund','Discount Permitted','Receipt') and Name<>''";
            DataSet ds = ClsSql.GetDs(mySql);
            DataTable dt = ds.Tables[0];
            cmbVoucher.DataSource = ds.Tables[0];
            cmbVoucher.DisplayMember = "Name";
            DataRow dr = dt.NewRow();
            dr["Name"] = "ALL";
            dt.Rows.InsertAt(dr, 0);

            cmbVoucher.Text = "ALL";
            this.dtpFromDate.Value = DateTime.Now;
            this.dtpToDate.Value = DateTime.Now;
            
        }
        public void GetData()
        {
            ////calculation
            //double CrSum = 0, DrSum = 0;
            //for (int i = 0; i < dgv.Rows.Count; i++)
            //{
            //    CrSum += Convert.ToDouble(dgv.Rows[i].Cells["Credit"].Value);
            //    DrSum += Convert.ToDouble(dgv.Rows[i].Cells["Debit"].Value);
            //}
            //txtCr.Text = CrSum.ToString();
            //txtDr.Text = DrSum.ToString();
            //txtCr.ReadOnly = true;
            //txtDr.ReadOnly = true;
            ////End of Calculation
        }

        public void NotEqual()
        {
            //lblNot.Text = "Unable to Post Because Credit and Debit Values are not equal please check below....";
            //mySql = "select SUM(credit) as Credit,SUM(debit) as Debit,BillNo from SDC_Revenue3 " +
            //         "group by BillNo having SUM(credit) <> SUM(debit)";
            //DataSet ds = ClsSql.GetDs2(mySql);
            //dgvNEqual.DataSource = ds.Tables[0];
            //string TableName = ((DataTable)dgvNEqual.DataSource).TableName;
        }
        //Posting method posting into Focus
        private void btnGet_Click(object sender, EventArgs e)
        {
            //started
            PostInAllMethods();
            if (blnErr_Log == true)
            {
                MessageBox.Show("Integration Completed, Check Error Log for issues....");
            }
            else
            {
                MessageBox.Show("Integration Completed....");
            }
        }
        //string path = @"D:\FocusRt\LogFile\ErrorLog.txt";
        string path = AppDomain.CurrentDomain.BaseDirectory + "FamilyCarerLog.txt";
        // File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "SMLOG",
        public void ErrorLog(string _message)
        {
            StreamWriter objLogFile = null;
            try
            {

                if (!File.Exists(path))
                {
                    File.Create(path);
                }
                objLogFile = new StreamWriter(path, true);
                objLogFile.WriteLine("Start Date and Time:" + DateTime.Now.ToString());
                objLogFile.WriteLine(_message);
                objLogFile.WriteLine("End Date and Time:" + DateTime.Now.ToString());
                objLogFile.WriteLine("---------------------------------------------------------------------------------------");
                objLogFile.Close();

            }
            catch (Exception)
            {
                objLogFile.Close();
                objLogFile = null;
            }
        }      
        private void PostInAllMethods()
        {
            if (cmbVoucher.Text == "ALL")
            {
                Revenue_Post();
                Collection_Post();
                CrNotes_Post();
                Discounts_Post();
                Refund_Post();
                Revenue_Canc_Post();
                //Patient_Issue_Post();      //Done Posting 23/07/2019  Code Perfect
                //Patient_Issue_rtn_Post();  //Done Posting 23/07/2019  Code Perfect
                //Purchase_Post();           //Done Posting 23/07/2019  Code Perfect
                //Purchase_rtn_Post();       //Done Posting 23/07/2019 Code Perfect

                //Stock_adj_Post();          //Done Posting 23/07/2019  Code Perfect
                //Stock_Consum_Post();       //Done Posting 23/07/2019 Code Perfect 
                //Stock_Consum_rtn_Post();   //Done Posting 23/07/2019 Code Perfect 
                //Stock_dispose_Post();      //Done Posting 23/07/2019 Code Perfect 
                //Stock_Issue_Post();       //Done Posting 23/07/2019 Code Perfect 
                //Stock_Issue_rtn_Post();    //Done Posting 23/07/2019 Code Perfect 
                //WorkOrder_Post();         //Done Posting 23/07/2019 Code Perfect 
                return;
            }
            if (cmbVoucher.Text == "Rev V")
            {
                Revenue_Post();
            }
            if (cmbVoucher.Text == "Receipt")
            {
                Collection_Post();
            }
            if (cmbVoucher.Text == "Cr. Notes")
            {
                CrNotes_Post();
            }
            if (cmbVoucher.Text == "Discount Permitted")
            {
                Discounts_Post();
            }
            if (cmbVoucher.Text == "Refund")
            {
                Refund_Post();
            }
            if (cmbVoucher.Text == "Canc Rev")
            {
                Revenue_Canc_Post();
            }
            //if (cmbVoucher.Text == "Patient Issue")
            //{               
            //    Patient_Issue_Post();                      
            //}
            //if (cmbVoucher.Text == "Patient Return")
            //{                
            //    Patient_Issue_rtn_Post();                   
            //}
            //if (cmbVoucher.Text == "Stores")
            //{                
            //    Purchase_Post();                            
            //}
            //if (cmbVoucher.Text == "Store Purchase Return")
            //{                
            //    Purchase_rtn_Post();                        
            //}

            //if (cmbVoucher.Text == "Store Adjustment")
            //{
            //    Stock_adj_Post();                           
            //}
            //if (cmbVoucher.Text == "Store Consumption")
            //{                
            //    Stock_Consum_Post();                           
            //}
            //if (cmbVoucher.Text == "Store Consumption Return")
            //{
            //    Stock_Consum_rtn_Post();                           
            //}
            //if (cmbVoucher.Text == "Store Dispose")
            //{                
            //    Stock_dispose_Post();
            //}
            //if (cmbVoucher.Text == "Store Issue")
            //{                
            //    Stock_Issue_Post();
            //}
            //if (cmbVoucher.Text == "Store Issue Return")
            //{                
            //    Stock_Issue_rtn_Post();
            //}
            //if (cmbVoucher.Text == "Work Order")
            //{               
            //    WorkOrder_Post();
            //}
            return ;
        }
        private void Create_AccMaster(string sAccName)
        {
            var fm = new FMaster();
            int Strseqid = 0;
            if (fm.OpenMaster("Account", "Name", sAccName) <= 0)
            {
                fm.New("Account");
                fm.SetField("Name", sAccName);
                fm.SetField("Code", sAccName);
                fm.SetField("IsGroup", "false");
                Strseqid = fm.Save();
            }
            fm.Close();
        }
        private void Revenue_Post()
        {
            string sVoucherType = "Rev V";
            var sConstr_IntDB = ClsComm.sqlConectionStr_IntDB();
            SqlConnection Chkconn = new SqlConnection(sConstr_IntDB);
            var constr2 = ClsComm.SqlConnectionString();

            //Get the data from Integrated DB
            string fDate = dtpFromDate.Value.ToString("yyyy-MM-dd");
            string tDate = dtpToDate.Value.ToString("yyyy-MM-dd");
            mySql = "select * from dbo.SDC_Revenue where CAST(BillDate AS DATE) between '" + fDate + "' and '" + tDate + "'";
            DataSet ds = ClsSql.GetDs2(mySql);
            dgv.DataSource = ds.Tables[0];

            //1st data is delete and Insert into Buffer table through type table
            SqlConnection conn = new SqlConnection(constr2);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Sp_Insert_SDC_Revenue", conn);
            cmd.Parameters.AddWithValue("@Revenue", ds.Tables[0]);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            conn.Close();
            //End.

            //Checking if master is Available are Not  
            mySql = "SELECT Distinct[GLNm] FROM dbo.SDC_Revenue fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
            ds = ClsSql.GetDs(mySql);
            var Err1 = new List<string>();
            if (ds.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {                    
                    Err1.Add(row["GLNm"].ToString());
                }
                string msg = string.Join(Environment.NewLine, Err1);                
                string str = msg.Replace("  ", " ");                
                using (StringReader reader = new StringReader(str))
                {
                    // Loop over the lines in the string.
                    string data;
                    while ((data = reader.ReadLine()) != null)
                    {
                        Create_AccMaster(data);
                    }
                }
            }//end of checking masters 
            double dCr_Amt = 0, dDr_Amt = 0;
            var ft = new Transaction();
            var sVNo = string.Empty;
            string sTmp = "";
            try
            {
                DataSet dst = new DataSet();
                string result = string.Format("select distinct BillNo from dbo.SDC_Revenue where '" + sVoucherType + "' + '^' + BillNo not in(select distinct Name + '^' + BillNoYH from dbo.v_BillCheck)");
                dst = ClsSql.GetDs(result);
                DataTable dt2 = dst.Tables[0];
                string sBillNo = "";
                Boolean blnPost = false;
                for (int z = 0; z < dst.Tables[0].Rows.Count; z++)
                {
                    dCr_Amt = 0;
                    dDr_Amt = 0;
                    sBillNo = dst.Tables[0].Rows[z]["BillNo"].ToString();
                    //checking data is exist or not if not existed data posting into focus 
                    //mySql = "SELECT * FROM dbo.SDC_Revenue fc where '" + sVoucherType + "' + '^' + fc.BillNo COLLATE DATABASE_DEFAULT not in" +
                    //       "(select distinct Name + '^' + BillNoYH from dbo.v_BillCheck) and BillNo='" + dst.Tables[0].Rows[z]["BillNo"].ToString() + "' ";
                    mySql = "SELECT * FROM dbo.SDC_Revenue fc where BillNo='" + sBillNo + "' ";
                    ds = ClsSql.GetDs(mySql);
                    DataTable dt = ds.Tables[0];
                    dgv.AllowUserToAddRows = false;
                    dgv.DataSource = dt;                
                    for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1) // 2nd data
                    {
                        if (n == 0)
                        {
                            sVNo = ft.GetNextVoucherNo("Jrn");
                            ft.NewDocument("Jrn", sVNo);

                            ft.SetField("Date", dgv.Rows[0].Cells["BillDate"].Value.ToString().Trim());
                            ft.SetField("VoucherType Name", sVoucherType);
                        }

                        sAcc = dgv.Rows[n].Cells["GLNm"].Value.ToString().Trim();
                        ft.SetField("Account Name", sAcc);

                        double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value.ToString().Trim());
                        double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value.ToString().Trim());

                        if (str != 0)
                        {
                            ft.SetField("Amount", str.ToString());
                            ft.SetField("DrCr", "Cr");
                            dCr_Amt = dCr_Amt + str;
                        }
                        else if (str1 != 0)
                        {
                            ft.SetField("Amount", str1.ToString());
                            ft.SetField("DrCr", "Dr");
                            dDr_Amt = dDr_Amt + str1;
                        }
                        var cell = dgv.Rows[n].Cells["PayeeId"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["PayeeId"].Value.ToString().Trim()) : "";
                        ft.SetField("Payee ID", sTmp);

                        cell = dgv.Rows[n].Cells["PayeeType"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["PayeeType"].Value.ToString().Trim()) : "";
                        ft.SetField("Payee Type", sTmp);
                        
                        ft.SetField("Bill No", sBillNo);

                        cell = dgv.Rows[n].Cells["BillDate"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["BillDate"].Value.ToString().Trim()) : "";
                        ft.SetField("Bill Date", sTmp);

                        cell = dgv.Rows[n].Cells["BillType"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["BillType"].Value.ToString().Trim()) : "";
                        ft.SetField("Bill Type", sTmp);

                        cell = dgv.Rows[n].Cells["FileNo"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["FileNo"].Value.ToString().Trim()) : "";
                        ft.SetField("File No", sTmp);

                        cell = dgv.Rows[n].Cells["VisitId"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VisitId"].Value.ToString().Trim()) : "";
                        ft.SetField("Visit Id", sTmp);

                        cell = dgv.Rows[n].Cells["Nationality"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Nationality"].Value.ToString().Trim()) : "";
                        ft.SetField("Nationality", sTmp);

                        cell = dgv.Rows[n].Cells["ServCode"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ServCode"].Value.ToString().Trim()) : "";
                        ft.SetField("Serv Code", sTmp);

                        cell = dgv.Rows[n].Cells["DeptId"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["DeptId"].Value.ToString().Trim()) : "";
                        ft.SetField("Dept Id", sTmp);

                        cell = dgv.Rows[n].Cells["NetAmt"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["NetAmt"].Value.ToString().Trim()) : "";
                        ft.SetField("Net Amt", sTmp);

                        cell = dgv.Rows[n].Cells["CashierId"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["CashierId"].Value.ToString().Trim()) : "";
                        ft.SetField("Cashier Id", sTmp);

                        cell = dgv.Rows[n].Cells["DrId"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["DrId"].Value.ToString().Trim()) : "";
                        ft.SetField("Dr ID", sTmp);

                        cell = dgv.Rows[n].Cells["Source"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString().Trim()) : "";
                        ft.SetField("Source", sTmp);
                        //ft.SetField("Status", dgv.Rows[n].Cells["Status"].Value.ToString());

                        ft.AddRow();
                        blnPost = true;
                    }
                    if (blnPost == true)
                    {
                        ft.SetField("Approved", "1");
                        int k = ft.SaveDocument();
                        if (k != 1)
                        {

                            if (dDr_Amt != dCr_Amt)
                            {
                                blnErr_Log = true;
                                ErrorLog("Unable To Post the Bill : " + sVoucherType + "-" + sBillNo + ", Debit and Credit Values are not Equal ");
                               
                            }
                            else
                            {
                                blnErr_Log = true;
                                ErrorLog("Unable To Post the Bill : " + sVoucherType + "-" + sBillNo + ", Check the Entry and masters");
                                
                            }
                        }
                    }
                }// 1st billno  for loop
            }
            catch (Exception ex)
            {
                blnErr_Log = true;
                ErrorLog(ex.Message);                
            }

        }
        private void Collection_Post()
        {

            string sVoucherType = "Receipt";
            var ChkConStr = ClsComm.sqlConectionStr_IntDB();
            SqlConnection Chkconn = new SqlConnection(ChkConStr);
            var constr2 = ClsComm.SqlConnectionString();


            string fDate = dtpFromDate.Value.ToString("yyyy-MM-dd");
            string tDate = dtpToDate.Value.ToString("yyyy-MM-dd");
            //Get the data from Integrated DB
            mySql = "select * from dbo.SDC_Collection where CAST(BillDate AS DATE) between '" + fDate + "' and '" + tDate + "'";
            ds = ClsSql.GetDs2(mySql);
            dgv.DataSource = ds.Tables[0];

            //1st data is delete and Insert into Buffer table through type table
            SqlConnection conn = new SqlConnection(constr2);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Sp_Insert_SDC_Collection", conn);
            cmd.Parameters.AddWithValue("@Collection", ds.Tables[0]);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            conn.Close();
            //End.

            //Checking if master is Available are Not  
            mySql = "SELECT Distinct[GLNm] FROM dbo.SDC_Collection fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
            ds = ClsSql.GetDs(mySql);
            var Err1 = new List<string>();
            if (ds.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {                    
                    Err1.Add(row["GLNm"].ToString());
                }
                string msg = string.Join(Environment.NewLine, Err1);                
                string str = msg.Replace("  ", " ");                
                using (StringReader reader = new StringReader(str))
                {
                    // Loop over the lines in the string.
                    string data;
                    while ((data = reader.ReadLine()) != null)
                    {
                        Create_AccMaster(data);
                    }
                }
            }
            double dCr_Amt = 0, dDr_Amt = 0;
            var ft = new Transaction();
            var sVNo = string.Empty;
            string sTmp = "";
            //End
            try
            {
                DataSet dst = new DataSet();
                string result = string.Format("select distinct BillNo from dbo.SDC_Collection where '" + sVoucherType + "' +'^'+BillNo not in(select distinct Name + '^' + BillNoYH from dbo.v_BillCheck)");
                dst = ClsSql.GetDs(result);
                DataTable dt2 = dst.Tables[0];
                string sBillNo = "";
                Boolean blnPost = false;
                for (int z = 0; z < dst.Tables[0].Rows.Count; z++)
                {
                    dCr_Amt = 0;
                    dDr_Amt = 0;
                    sBillNo = dst.Tables[0].Rows[z]["BillNo"].ToString();
                    //Checking here data is exist or not in focus table  
                    //mySql = "SELECT * FROM dbo.SDC_Collection fc where '" + sVoucherType + "' + '^' + fc.BillNo COLLATE DATABASE_DEFAULT not in" +
                    //       "(select distinct Name + '^' + BillNoYH from dbo.v_BillCheck) and BillNo='" + dst.Tables[0].Rows[z]["BillNo"].ToString() + "' ";
                    mySql = "SELECT * FROM dbo.SDC_Collection fc where BillNo='" + sBillNo + "'";
                    ds = ClsSql.GetDs(mySql);
                    dt = ds.Tables[0];
                    dgv.AllowUserToAddRows = false;
                    dgv.DataSource = dt;
                    int cr = 0;
                    int dr = 0;
                    for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
                    {
                        if (n == 0)
                        {
                            sVNo = ft.GetNextVoucherNo("Jrn");
                            ft.NewDocument("Jrn", sVNo);

                            ft.SetField("Date", dgv.Rows[0].Cells["BillDate"].Value.ToString().Trim());
                            ft.SetField("VoucherType Name", sVoucherType);

                        }
                        //var cell = dgv.Rows[n].Cells["GLNm"];
                        ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString().Trim());

                        double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value.ToString().Trim());
                        double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value.ToString().Trim());
                        if (str != 0)
                        {
                            ft.SetField("Amount", str.ToString());
                            ft.SetField("DrCr", "Cr");
                            dCr_Amt = dCr_Amt + str;
                        }
                        else if (str1 != 0)
                        {
                            ft.SetField("Amount", str1.ToString());
                            ft.SetField("DrCr", "Dr");
                            dDr_Amt = dDr_Amt + str1;
                        }

                        var cell = dgv.Rows[n].Cells["PayeeId"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["PayeeId"].Value.ToString().Trim()) : "";
                        ft.SetField("Payee ID", sTmp);

                        cell = dgv.Rows[n].Cells["PayeeType"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["PayeeType"].Value.ToString().Trim()) : "";
                        ft.SetField("Payee Type", sTmp);
                        
                        ft.SetField("Bill No", sBillNo);

                        cell = dgv.Rows[n].Cells["BillDate"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["BillDate"].Value.ToString().Trim()) : "";
                        ft.SetField("Bill Date", sTmp);

                        cell = dgv.Rows[n].Cells["BillType"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["BillType"].Value.ToString().Trim()) : "";
                        ft.SetField("Bill Type", sTmp);

                        cell = dgv.Rows[n].Cells["FileNo"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["FileNo"].Value.ToString().Trim()) : "";
                        ft.SetField("File No", sTmp);

                        cell = dgv.Rows[n].Cells["VisitId"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VisitId"].Value.ToString().Trim()) : "";
                        ft.SetField("Visit ID", sTmp);

                        cell = dgv.Rows[n].Cells["Nationality"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Nationality"].Value.ToString().Trim()) : "";
                        ft.SetField("Nationality", sTmp);

                        cell = dgv.Rows[n].Cells["ReceiptNo"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ReceiptNo"].Value.ToString().Trim()) : "";
                        ft.SetField("Receipt No", sTmp);

                        cell = dgv.Rows[n].Cells["NetAmt"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["NetAmt"].Value.ToString().Trim()) : "";
                        ft.SetField("Net Amt", sTmp);

                        cell = dgv.Rows[n].Cells["VATAmt"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VATAmt"].Value.ToString().Trim()) : "";
                        ft.SetField("VAT Amt", sTmp);

                        cell = dgv.Rows[n].Cells["DrId"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["DrId"].Value.ToString().Trim()) : "";
                        ft.SetField("Dr ID", sTmp);

                        cell = dgv.Rows[n].Cells["Source"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString().Trim()) : "";
                        ft.SetField("Source", sTmp);

                        ft.AddRow();
                        blnPost = true;
                    }
                    if (blnPost == true)
                    {
                        ft.SetField("Approved", "1");
                        int k = ft.SaveDocument();
                        if (k != 1)
                        {
                            if (dDr_Amt != dCr_Amt)
                            {
                                blnErr_Log = true;
                                ErrorLog("Unable To Post the Bill : " + sVoucherType + "-" + sBillNo + ", Debit and Credit Values are not Equal ");
                            }
                            else
                            {
                                blnErr_Log = true;
                                ErrorLog("Unable To Post the Bill : " + sVoucherType + "-" + sBillNo + ", Check the Entry and masters");
                            }
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                blnErr_Log = true;
                ErrorLog(ex.Message);            
            }

        }
        private void CrNotes_Post()
        {
            string sVoucherType = "Cr. Notes";
            var ChkConStr = ClsComm.sqlConectionStr_IntDB();
            SqlConnection Chkconn = new SqlConnection(ChkConStr);
            var constr2 = ClsComm.SqlConnectionString();

            string fDate = dtpFromDate.Value.ToString("yyyy-MM-dd");
            string tDate = dtpToDate.Value.ToString("yyyy-MM-dd");
            //Get the data from Integrated DB
            mySql = "select * from dbo.SDC_CrNotes where CAST(BillDate AS DATE) between '" + fDate + "' and '" + tDate + "'";
            DataSet ds = ClsSql.GetDs2(mySql);
            dgv.DataSource = ds.Tables[0];

            //1st delete the data and Insert into Buffer table through type table
            SqlConnection conn = new SqlConnection(constr2);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Sp_Insert_SDC_CrNotes", conn);
            cmd.Parameters.AddWithValue("@CrNotes", ds.Tables[0]);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            conn.Close();
            //End.

            //Checking if master is Available are Not  
            mySql = "SELECT Distinct[GLNm] FROM dbo.SDC_CrNotes fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
            ds = ClsSql.GetDs(mySql);
            var Err1 = new List<string>();
            if (ds.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {                    
                    Err1.Add(row["GLNm"].ToString());
                }
                string msg = string.Join(Environment.NewLine, Err1);                
                string str = msg.Replace("  ", " ");
                using (StringReader reader = new StringReader(str))
                {
                    // Loop over the lines in the string.
                    string data;
                    while ((data = reader.ReadLine()) != null)
                    {
                        Create_AccMaster(data);
                    }                    
                }
            }
            double dCr_Amt = 0, dDr_Amt = 0;
            var ft = new Transaction();
            var sVNo = string.Empty;
            string sTmp = "";
            try
            {
                DataSet dst = new DataSet();
                string result = string.Format("select distinct BillNo from dbo.SDC_CrNotes where '" + sVoucherType + "' + '^' + BillNo not in(select distinct Name + '^' + BillNoYH from dbo.v_BillCheck)");
                dst = ClsSql.GetDs(result);
                DataTable dt2 = dst.Tables[0];
                string sBillNo = "";
                Boolean blnPost = false;
                for (int z = 0; z < dst.Tables[0].Rows.Count; z++)
                {
                    dCr_Amt = 0;
                    dDr_Amt = 0;
                    sBillNo = dst.Tables[0].Rows[z]["BillNo"].ToString();
                    //checking data is exist or not if not exist posting into focus
                    //mySql = "SELECT * FROM dbo.SDC_CrNotes fc where '" + sVoucherType + "'  + '^' + fc.BillNo COLLATE DATABASE_DEFAULT not in" +
                    //       "(select distinct Name + '^' + BillNoYH from dbo.v_BillCheck) and BillNo='" + dst.Tables[0].Rows[z]["BillNo"].ToString() + "' ";
                    mySql = "SELECT * FROM dbo.SDC_CrNotes fc where BillNo='" + sBillNo + "'";
                    ds = ClsSql.GetDs(mySql);
                    dt = ds.Tables[0];
                    dgv.AllowUserToAddRows = false;
                    dgv.DataSource = dt;
                    for (int n = 0; n < dgv.Rows.Count; n = n + 1)
                    {
                        if (n == 0)
                        {
                            sVNo = ft.GetNextVoucherNo("Jrn");
                            ft.NewDocument("Jrn", sVNo);

                            ft.SetField("Date", dgv.Rows[0].Cells["BillDate"].Value.ToString().Trim());
                            ft.SetField("VoucherType Name", sVoucherType);
                        }

                        //var cell = dgv.Rows[n].Cells["GLNm"];
                        ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString().Trim());

                        double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value.ToString().Trim());
                        double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value.ToString().Trim());
                        if (str != 0)
                        {
                            ft.SetField("Amount", str.ToString());
                            ft.SetField("DrCr", "Cr");
                            dCr_Amt = dCr_Amt + str;
                        }
                        else if (str1 != 0)
                        {
                            ft.SetField("Amount", str1.ToString());
                            ft.SetField("DrCr", "Dr");
                            dDr_Amt = dDr_Amt + str1;
                        }

                        var cell = dgv.Rows[n].Cells["PayeeId"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["PayeeId"].Value.ToString().Trim()) : "";
                        ft.SetField("Payee ID", sTmp);

                        cell = dgv.Rows[n].Cells["PayeeType"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["PayeeType"].Value.ToString().Trim()) : "";
                        ft.SetField("Payee Type", sTmp);

                        ft.SetField("Bill No", sBillNo);

                        cell = dgv.Rows[n].Cells["BillDate"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["BillDate"].Value.ToString().Trim()) : "";
                        ft.SetField("Bill Date", sTmp);

                        cell = dgv.Rows[n].Cells["BillType"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["BillType"].Value.ToString().Trim()) : "";
                        ft.SetField("Bill Type", sTmp);

                        cell = dgv.Rows[n].Cells["FileNo"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["FileNo"].Value.ToString().Trim()) : "";
                        ft.SetField("File No", sTmp);

                        cell = dgv.Rows[n].Cells["VisitId"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VisitId"].Value.ToString().Trim()) : "";
                        ft.SetField("Visit ID", sTmp);

                        cell = dgv.Rows[n].Cells["Nationality"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Nationality"].Value.ToString().Trim()) : "";
                        ft.SetField("Nationality", sTmp);

                        cell = dgv.Rows[n].Cells["CrNoteId"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["CrNoteId"].Value.ToString().Trim()) : "";
                        ft.SetField("Cr.Note ID", sTmp);

                        cell = dgv.Rows[n].Cells["CrNoteDate"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["CrNoteDate"].Value.ToString().Trim()) : "";
                        ft.SetField("Cr.Note Date", sTmp);

                        cell = dgv.Rows[n].Cells["NetAmt"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["NetAmt"].Value.ToString().Trim()) : "";
                        ft.SetField("Net Amt", sTmp);

                        cell = dgv.Rows[n].Cells["CashierId"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["CashierId"].Value.ToString().Trim()) : "";
                        ft.SetField("Cashier ID", sTmp);

                        cell = dgv.Rows[n].Cells["DrId"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["DrId"].Value.ToString().Trim()) : "";
                        ft.SetField("Dr ID", sTmp);

                        cell = dgv.Rows[n].Cells["Source"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString().Trim()) : "";
                        ft.SetField("Source", sTmp);

                        ft.AddRow();
                        blnPost = true;
                    }
                    if (blnPost == true)
                    {
                        ft.SetField("Approved", "1");
                        int k = ft.SaveDocument();
                        if (k != 1)
                        {
                            if (dDr_Amt != dCr_Amt)
                            {
                                blnErr_Log = true;
                                ErrorLog("Unable To Post the Bill : " + sVoucherType + "-" + sBillNo + ", Debit and Credit Values are not Equal ");
                            }
                            else
                            {
                                blnErr_Log = true;
                                ErrorLog("Unable To Post the Bill : " + sVoucherType + "-" + sBillNo + ", Check the Entry and masters");
                            }
                        }
                    }      
                }
            }
            catch (Exception ex)
            {
                blnErr_Log = true;
                ErrorLog(ex.Message);        
            }
        }
        private void Discounts_Post()
        {
            string sVoucherType = @"Discount Permitted";
            var ChkConStr = ClsComm.sqlConectionStr_IntDB();
            SqlConnection Chkconn = new SqlConnection(ChkConStr);
            var constr2 = ClsComm.SqlConnectionString();

            string fDate = dtpFromDate.Value.ToString("yyyy-MM-dd");
            string tDate = dtpToDate.Value.ToString("yyyy-MM-dd");
            //Get the data from Integrated DB
            mySql = "select * from dbo.SDC_Discounts where CAST(BillDate AS DATE) between '" + fDate + "' and '" + tDate + "'";
            DataSet ds = ClsSql.GetDs2(mySql);
            dgv.DataSource = ds.Tables[0];

            //1st Delete the data and Insert into Buffer table through type table
            SqlConnection conn = new SqlConnection(constr2);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Sp_Insert_SDC_Discounts", conn);
            cmd.Parameters.AddWithValue("@Discounts", ds.Tables[0]);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            conn.Close();
            //End.

            //Checking if master is Available are Not  
            mySql = "SELECT Distinct[GLNm] FROM dbo.SDC_Discounts fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
            ds = ClsSql.GetDs(mySql);
            var Err1 = new List<string>();
            if (ds.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {                   
                    Err1.Add(row["GLNm"].ToString());
                }
                string msg = string.Join(Environment.NewLine, Err1);
                string str = msg.Replace("  ", " ");
                using (StringReader reader = new StringReader(str))
                {
                    // Loop over the lines in the string.
                    string data;
                    while ((data = reader.ReadLine()) != null)
                    {
                        Create_AccMaster(data);
                    }                   
                }

            }//End of Checking masters
            double dCr_Amt = 0, dDr_Amt = 0;
            var ft = new Transaction();
            var sVNo = string.Empty;
            string sTmp = "";
            try
            {
                DataSet dst = new DataSet();
                string result = string.Format("select distinct BillNo from dbo.SDC_Discounts where '" + sVoucherType + "' + '^' + BillNo not in(select distinct Name + '^' + BillNoYH from dbo.v_BillCheck)");
                dst = ClsSql.GetDs(result);
                DataTable dt2 = dst.Tables[0];
                string sBillNo = "";
                Boolean blnPost = false;
                for (int z = 0; z < dst.Tables[0].Rows.Count; z++)
                {
                    dCr_Amt = 0;
                    dDr_Amt = 0;
                    sBillNo = dst.Tables[0].Rows[z]["BillNo"].ToString();                    
                    mySql = "SELECT * FROM dbo.SDC_Discounts fc where BillNo='" + sBillNo + "'";
                    ds = ClsSql.GetDs(mySql);
                    dt = ds.Tables[0];
                    dgv.AllowUserToAddRows = false;
                    dgv.DataSource = dt;

                    for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
                    {
                        if (n == 0)
                        {
                            sVNo = ft.GetNextVoucherNo("Jrn");
                            ft.NewDocument("Jrn", sVNo);

                            ft.SetField("Date", dgv.Rows[0].Cells["BillDate"].Value.ToString().Trim());
                            ft.SetField("VoucherType Name", sVoucherType);
                        }

                        //var cell = dgv.Rows[n].Cells["GLNm"];
                        ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString().Trim());

                        double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value.ToString().Trim());
                        double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value.ToString().Trim());
                        if (str != 0)
                        {
                            ft.SetField("Amount", str.ToString());
                            ft.SetField("DrCr", "Cr");
                            dCr_Amt = dCr_Amt + str;
                        }
                        else if (str1 != 0)
                        {
                            ft.SetField("Amount", str1.ToString());
                            ft.SetField("DrCr", "Dr");
                            dDr_Amt = dDr_Amt + str1;
                        }
                        var cell = dgv.Rows[n].Cells["PayeeId"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["PayeeId"].Value.ToString().Trim()) : "";
                        ft.SetField("Payee ID", sTmp);

                        cell = dgv.Rows[n].Cells["PayeeType"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["PayeeType"].Value.ToString().Trim()) : "";
                        ft.SetField("Payee Type", sTmp);

                        ft.SetField("Bill No", sBillNo);

                        cell = dgv.Rows[n].Cells["BillDate"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["BillDate"].Value.ToString().Trim()) : "";
                        ft.SetField("Bill Date", sTmp);

                        cell = dgv.Rows[n].Cells["BillType"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["BillType"].Value.ToString().Trim()) : "";
                        ft.SetField("Bill Type", sTmp);

                        cell = dgv.Rows[n].Cells["FileNo"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["FileNo"].Value.ToString().Trim()) : "";
                        ft.SetField("File No", sTmp);

                        cell = dgv.Rows[n].Cells["VisitId"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VisitId"].Value.ToString().Trim()) : "";
                        ft.SetField("Visit ID", sTmp);

                        cell = dgv.Rows[n].Cells["Nationality"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Nationality"].Value.ToString().Trim()) : "";
                        ft.SetField("Nationality", sTmp);

                        cell = dgv.Rows[n].Cells["ServCode"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ServCode"].Value.ToString().Trim()) : "";
                        ft.SetField("Serv Code", sTmp);

                        cell = dgv.Rows[n].Cells["DeptId"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["DeptId"].Value.ToString().Trim()) : "";
                        ft.SetField("Dept ID", sTmp);

                        cell = dgv.Rows[n].Cells["NetAmt"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["NetAmt"].Value.ToString().Trim()) : "";
                        ft.SetField("Net Amt", sTmp);

                        cell = dgv.Rows[n].Cells["CashierId"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["CashierId"].Value.ToString().Trim()) : "";
                        ft.SetField("Cashier ID", sTmp);

                        cell = dgv.Rows[n].Cells["DrId"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["DrId"].Value.ToString().Trim()) : "";
                        ft.SetField("Dr ID", sTmp);

                        cell = dgv.Rows[n].Cells["CancelDate"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["CancelDate"].Value.ToString().Trim()) : "";
                        ft.SetField("Cancel Date", sTmp);

                        //cell = dgv.Rows[n].Cells["CancelerId"];// Nul value handling
                        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["CancelerId"].Value.ToString()) : "";
                        //ft.SetField("Cancel ID", sTmp);

                        cell = dgv.Rows[n].Cells["Source"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString().Trim()) : "";
                        ft.SetField("Source", sTmp);

                        ft.AddRow();
                        blnPost = true;
                    }
                    if (blnPost == true)
                    {
                        ft.SetField("Approved", "1");
                        int k = ft.SaveDocument();
                        if (k != 1)
                        {
                            if (dDr_Amt != dCr_Amt)
                            {
                                blnErr_Log = true;
                                ErrorLog("Unable To Post the Bill : " + sVoucherType + "-" + sBillNo + ", Debit and Credit Values are not Equal ");
                                
                            }
                            else
                            {
                                blnErr_Log = true;
                                ErrorLog("Unable To Post the Bill : " + sVoucherType + "-" + sBillNo + ", Check the Entry and masters");
                            }
                        }
                    }                   
                }
            }
            catch (Exception ex)
            {
                blnErr_Log = true;
                ErrorLog(ex.Message);
            }
        }
        private void Patient_Issue_Post()
        {
            //string Pstr = "Patient Issue";
            //var ChkConStr = ClsComm.SqlConnectionString2();
            //SqlConnection Chkconn = new SqlConnection(ChkConStr);
            //var constr2 = ClsComm.SqlConnectionString();
            //var connsplit = constr2.Split('=', ';');
            //var connsplit2 = ChkConStr.Split('=', ';');
            //for (int count = 0; count <= connsplit.Length - 1; count++)
            //{
            //    if (count % 2 == 0)
            //    { }
            //    else
            //    {
            //        sqlserver2 = connsplit[1];
            //        initialcatalog2 = connsplit[3];
            //        Companycode2 = "030";
            //        Username2 = connsplit[5];
            //        password2 = connsplit[7];
            //    }
            //}
            //for (int count = 0; count <= connsplit2.Length - 1; count++)
            //{
            //    if (count % 2 == 0)
            //    { }
            //    else
            //    {
            //        sqlserver3 = connsplit2[1];
            //        initialcatalog3 = connsplit2[3];
            //        Companycode3 = "060";
            //        Username3 = connsplit2[5];
            //        password3 = connsplit2[7];
            //    }

            //}
            //var ft = new Transaction();
            //var sVNo = string.Empty;
            //blnIsEditing = false;
            //string sTmp = "";
            //if (blnIsEditing == false)
            //{
            //    sVNo = ft.GetNextVoucherNo("Jrn");
            //    ft.NewDocument("Jrn", sVNo);
            //}
            //else
            //{
            //    ft.DeleteDocument("Jrn", sVNo);
            //}

            //ft.SetField("Date", dtpToDate.Value.ToString(/*"yyyy/MM/dd"*/));
            //ft.SetField("VoucherType Name", Pstr);
            //////Select Data from SDC_Patient_Issue based on the bill no and comparing 
            ////mySql = "SELECT [GLNm],[Credit],[Debit],[FileNo],[VisitNo],[VisitDate],[IssueNo],[IssueDate],[ItemId],[ItemQty],[ItemRate],[ItemTotal],[ItemType],[StoreId],[Source],[Status],[Remark],[PostingDateTime]" +
            ////   "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Patient_Issue fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [" + sqlserver2 + "].[Focus5030].dbo.SDC_Patient_Issue fd)";           
            //mySql = "SELECT * FROM[" + sqlserver3 + "].[" + initialcatalog3 + "].dbo.SDC_Patient_Issue fc where '" + Pstr + "'  + '^' + fc.VisitNo COLLATE DATABASE_DEFAULT not in" +
            //       "(select distinct Name + '^' + BillNoYH from[" + sqlserver2 + "].[" + initialcatalog2 + "].dbo.v_BillCheck) ";
            //ds = ClsSql.GetDs2(mySql);
            //dt = ds.Tables[0];
            //dgv.AllowUserToAddRows = false;
            //dgv.DataSource = dt;
            ////Close
            //try
            //{
            //    for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
            //    {

            //        //var cell = dgv.Rows[n].Cells["GLNm"];
            //        ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString());

            //        double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value);
            //        double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value);
            //        if (str != 0)
            //        {
            //            ft.SetField("Amount", str.ToString());
            //            ft.SetField("DrCr", "Cr");
            //        }
            //        else if (str1 != 0)
            //        {
            //            ft.SetField("Amount", str1.ToString());
            //            ft.SetField("DrCr", "Dr");
            //        }

            //        var cell = dgv.Rows[n].Cells["FileNo"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["FileNo"].Value.ToString()) : "";
            //        ft.SetField("File No", sTmp);

            //        ////Not confirmed this field visitNo is not their in JV
            //        //cell = dgv.Rows[n].Cells["VisitNo"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VisitNo"].Value.ToString()) : "";
            //        //ft.SetField("Visit No", sTmp);

            //        //Not confirmed this field visitNo is not their in JV
            //        //cell = dgv.Rows[n].Cells["VisitDate"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VisitDate"].Value.ToString()) : "";
            //        //ft.SetField("Visit Date", sTmp);

            //        cell = dgv.Rows[n].Cells["IssueNo"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["IssueNo"].Value.ToString()) : "";
            //        ft.SetField("P Issue", sTmp);

            //        cell = dgv.Rows[n].Cells["IssueDate"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["IssueDate"].Value.ToString()) : "";
            //        ft.SetField("P Issue Date", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemId"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemId"].Value.ToString()) : "";
            //        ft.SetField("Item ID", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemQty"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemQty"].Value.ToString()) : "";
            //        ft.SetField("Item Q", sTmp);

            //        //Not confirmed this field visitNo is not their in JV
            //        //cell = dgv.Rows[n].Cells["ItemRate"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemRate"].Value.ToString()) : "";
            //        //ft.SetField("Item Rate", sTmp);

            //        //Not confirmed this field visitNo is not their in JV
            //        //cell = dgv.Rows[n].Cells["ItemTotal"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemTotal"].Value.ToString()) : "";
            //        //ft.SetField("Item Total", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemType"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemType"].Value.ToString()) : "";
            //        ft.SetField("Item Type", sTmp);

            //        cell = dgv.Rows[n].Cells["StoreId"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["StoreId"].Value.ToString()) : "";
            //        ft.SetField("Store ID", sTmp);

            //        cell = dgv.Rows[n].Cells["Source"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString()) : "";
            //        ft.SetField("Source", sTmp);

            //        ft.AddRow();
            //    }
            //    ft.SetField("Approved", "1");
            //    int k = ft.SaveDocument();
            //    if (k != 1)
            //    {
            //        MessageBox.Show("Unable to post the Sales Invoice", "Focus", MessageBoxButtons.OK,
            //            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            //        //Checking if master is Available are Not                                  
            //        Chkconn = new SqlConnection(constr2);
            //        Chkconn.Open();
            //        mySql = "SELECT Distinct[GLNm] FROM [" + sqlserver3 + "].[" + initialcatalog3 + "].dbo.SDC_Patient_Issue fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from [" + sqlserver2 + "].[" + initialcatalog2 + "].dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
            //        ds = ClsSql.GetDs2(mySql);
            //        var Err = new List<string>();
            //        if (dgv.Rows[0].Cells["GLNm"] == null)
            //        {
            //            ErrorLog("Already JV Posted ");
            //        }
            //        else
            //        {
            //            foreach (DataRow row in ds.Tables[0].Rows)
            //            {
            //                //ErrorLog("Account Masters are not Available in Focus:" + row["GLNm"].ToString());
            //                Err.Add(row["GLNm"].ToString());
            //            }
            //            string msg = string.Join(Environment.NewLine, Err);
            //            ErrorLog("Account Masters are not Available in Focus:Voucher Type:'SDC_Patient_Issue\n" + msg);
            //            Chkconn.Close();
            //        }
            //        //End of Checking Masters
            //        //checking If credit and debit Notes arenot equal 
            //        mySql = "select SUM(credit) as Credit,SUM(debit) as Debit,BillNo from [" + sqlserver3 + "].[" + initialcatalog3 + "].dbo.SDC_Patient_Issue " +
            //         "group by BillNo having SUM(credit) <> SUM(debit)";
            //        ds = ClsSql.GetDs2(mySql);
            //        foreach (DataRow row in ds.Tables[0].Rows)
            //        {
            //            ErrorLog("....Credit and Debit Notes are not Equal:....\n Credit Value:" + row["Credit"].ToString() + ",Debit Value:" + row["Debit"].ToString() + ",BillNo:" + row["BillNo"].ToString());
            //        }
            //        dgvNEqual.DataSource = ds.Tables[0];
            //        //Close
            //    }
            //    else
            //    {
            //        MessageBox.Show("Sales Invoice In Entry voucher Posted Successfully with document No. " + sVNo /*+ docno.Text*/, "Focus", MessageBoxButtons.OK,
            //        MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            //        //If Posting Success insert into Buffer DB 	SDC_Patient_Issue
            //        dgv.DataSource = ds.Tables[0];
            //        foreach (DataGridViewRow dr in dgv.Rows)
            //        {
            //        //    string Chkstr = "INSERT INTO[" + sqlserver2 + "].[Focus5030].dbo.SDC_Patient_Issue([GLNm],[Credit],[Debit],[FileNo],[VisitNo],[VisitDate],[IssueNo],[IssueDate],[ItemId],[ItemQty],[ItemRate],[ItemTotal],[ItemType],[StoreId],[Source],[Status],[Remark],[PostingDateTime])" +
            //        //                                 "SELECT [GLNm],[Credit],[Debit],[FileNo],[VisitNo],[VisitDate],[IssueNo],[IssueDate],[ItemId],[ItemQty],[ItemRate],[ItemTotal],[ItemType],[StoreId],[Source],[Status],[Remark],[PostingDateTime]" +
            //        //                                 "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Patient_Issue fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [Focus5030].dbo.SDC_Patient_Issue fd)";
            //            string dtdate = Convert.ToDateTime(dr.Cells["VisitDate"].Value).ToString("yyyy-MM-dd HH:mm:ss");
            //            string RDate = Convert.ToDateTime(dr.Cells["IssueDate"].Value).ToString("yyyy-MM-dd HH:mm:ss");
            //            mySql1 = mySql1 + "\n" + string.Format("INSERT INTO [" + sqlserver2 + "].[" + initialcatalog2 + "].dbo.SDC_Patient_Issue([VoucherType],[GLNm],[Credit],[Debit],[FileNo],[VisitNo],[VisitDate],[IssueNo],[IssueDate],[ItemId],[ItemQty],[ItemRate],[ItemTotal],[ItemType],[StoreId],[Source],[Status],[Remark],[PostingDateTime])" +
            //                     "VALUES('" + Pstr + "','" + dr.Cells["GLnm"].Value + "'," + dr.Cells["Credit"].Value + "," + dr.Cells["Debit"].Value + "," + dr.Cells["FileNo"].Value + "," + dr.Cells["VisitNo"].Value + ",'"+dtdate+"'," + dr.Cells["IssueNo"].Value + ",'" + RDate + "'," + dr.Cells["ItemId"].Value + ", " + dr.Cells["ItemQty"].Value + ", " + dr.Cells["ItemRate"].Value + "," + dr.Cells["ItemTotal"].Value + ",'" + dr.Cells["ItemType"].Value + "'," + dr.Cells["StoreId"].Value + ",'" + dr.Cells["Source"].Value + "','" + dr.Cells["Status"].Value + "','" + dr.Cells["Remark"].Value + "','" + dr.Cells["PostingDateTime"].Value + "')");
            //        }
            //        ChkCmd = new SqlCommand(mySql1, Chkconn);
            //        da = new SqlDataAdapter(ChkCmd);
            //        ds = new DataSet();
            //        da.Fill(ds);
            //        //Close
            //    }
            //}
            //catch (Exception ex) { ErrorLog(ex.ToString()); }
        }       //Done Posting 23/07/2019  Code Perfect 
        private void Patient_Issue_rtn_Post()
        {
            //string Pstr = "Patient Return";
            //var ChkConStr = ClsComm.SqlConnectionString2();
            //SqlConnection Chkconn = new SqlConnection(ChkConStr);
            //var constr2 = ClsComm.SqlConnectionString();
            //var connsplit = constr2.Split('=', ';');
            //var connsplit2 = ChkConStr.Split('=', ';');
            //for (int count = 0; count <= connsplit.Length - 1; count++)
            //{
            //    if (count % 2 == 0)
            //    { }
            //    else
            //    {
            //        sqlserver2 = connsplit[1];
            //        initialcatalog2 = connsplit[3];
            //        Companycode2 = "030";
            //        Username2 = connsplit[5];
            //        password2 = connsplit[7];
            //    }
            //}
            //for (int count = 0; count <= connsplit2.Length - 1; count++)
            //{
            //    if (count % 2 == 0)
            //    { }
            //    else
            //    {
            //        sqlserver3 = connsplit2[1];
            //        initialcatalog3 = connsplit2[3];
            //        Companycode3 = "060";
            //        Username3 = connsplit2[5];
            //        password3 = connsplit2[7];
            //    }

            //}
            //var ft = new Transaction();
            //var sVNo = string.Empty;
            //blnIsEditing = false;
            //string sTmp = "";
            //if (blnIsEditing == false)
            //{
            //    sVNo = ft.GetNextVoucherNo("Jrn");
            //    ft.NewDocument("Jrn", sVNo);
            //}
            //else
            //{
            //    ft.DeleteDocument("Jrn", sVNo);
            //}

            //ft.SetField("Date", dtpToDate.Value.ToString(/*"yyyy/MM/dd"*/));
            //ft.SetField("VoucherType Name", Pstr);
            ////Select Data from SDC_Patient_Issue_Return based on the bill no and comparing
            //mySql = "SELECT * FROM[" + sqlserver3 + "].[" + initialcatalog3 + "].dbo.SDC_Patient_Issue_Return fc where '" + Pstr + "'  + '^' + fc.BillNo COLLATE DATABASE_DEFAULT not in" +
            //       "(select distinct Name + '^' + BillNoYH from[" + sqlserver2 + "].[" + initialcatalog2 + "].dbo.v_BillCheck) ";
            //ds = ClsSql.GetDs2(mySql);
            //dt = ds.Tables[0];
            //dgv.AllowUserToAddRows = false;
            //dgv.DataSource = dt;
            ////Close
            //try
            //{
            //    for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
            //    {

            //        //var cell = dgv.Rows[n].Cells["GLNm"];
            //        ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString());

            //        double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value);
            //        double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value);
            //        if (str != 0)
            //        {
            //            ft.SetField("Amount", str.ToString());
            //            ft.SetField("DrCr", "Cr");
            //        }
            //        else if (str1 != 0)
            //        {
            //            ft.SetField("Amount", str1.ToString());
            //            ft.SetField("DrCr", "Dr");
            //        }

            //        var cell = dgv.Rows[n].Cells["FileNo"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["FileNo"].Value.ToString()) : "";
            //        ft.SetField("File No", sTmp);

            //        //Not confirmed this field visitNo is not their in JV
            //        //cell = dgv.Rows[n].Cells["VisitNo"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VisitNo"].Value.ToString()) : "";
            //        //ft.SetField("Visit No", sTmp);

            //        //Not confirmed this field visitNo is not their in JV
            //        //cell = dgv.Rows[n].Cells["VisitDate"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VisitDate"].Value.ToString()) : "";
            //        //ft.SetField("Visit Date", sTmp);

            //        //Not confirmed this field visitNo is not their in JV
            //        //cell = dgv.Rows[n].Cells["ReturnNo"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ReturnNo"].Value.ToString()) : "";
            //        //ft.SetField("Return No", sTmp);

            //        //Not confirmed this field visitNo is not their in JV
            //        //cell = dgv.Rows[n].Cells["ReturnDate"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ReturnDate"].Value.ToString()) : "";
            //        //ft.SetField("Return Date", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemId"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemId"].Value.ToString()) : "";
            //        ft.SetField("Item ID", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemQty"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemQty"].Value.ToString()) : "";
            //        ft.SetField("Item Q", sTmp);

            //        //Not confirmed this field visitNo is not their in JV
            //        //cell = dgv.Rows[n].Cells["ItemRate"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemRate"].Value.ToString()) : "";
            //        //ft.SetField("Item Rate", sTmp);

            //        //Not confirmed this field visitNo is not their in JV
            //        //cell = dgv.Rows[n].Cells["ItemTotal"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemTotal"].Value.ToString()) : "";
            //        //ft.SetField("Item Total", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemType"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemType"].Value.ToString()) : "";
            //        ft.SetField("Item Type", sTmp);

            //        cell = dgv.Rows[n].Cells["StoreId"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["StoreId"].Value.ToString()) : "";
            //        ft.SetField("Store ID", sTmp);

            //        cell = dgv.Rows[n].Cells["Source"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString()) : "";
            //        ft.SetField("Source", sTmp);

            //        ft.AddRow();
            //    }
            //    ft.SetField("Approved", "1");
            //    int k = ft.SaveDocument();
            //    if (k != 1)
            //    {
            //        MessageBox.Show("Unable to post the Sales Invoice", "Focus", MessageBoxButtons.OK,
            //            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            //        //Checking if master is Available are Not                                  
            //        Chkconn = new SqlConnection(constr2);
            //        Chkconn.Open();
            //        mySql = "SELECT Distinct[GLNm] FROM [" + sqlserver3 + "].[" + initialcatalog3 + "].dbo.SDC_Patient_Issue_Return fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from [" + sqlserver2 + "].[" + initialcatalog2 + "].dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
            //        ds = ClsSql.GetDs2(mySql);
            //        var Err = new List<string>();
            //        if (dgv.Rows[0].Cells["GLNm"] == null)
            //        {
            //            ErrorLog("Already JV Posted ");
            //        }
            //        else
            //        {
            //            foreach (DataRow row in ds.Tables[0].Rows)
            //            {
            //                //ErrorLog("Account Masters are not Available in Focus:" + row["GLNm"].ToString());
            //                Err.Add(row["GLNm"].ToString());
            //            }
            //            string msg = string.Join(Environment.NewLine, Err);
            //            ErrorLog("Account Masters are not Available in Focus:Voucher Type:'SDC_Patient_Issue_Return'\n" + msg);
            //            Chkconn.Close();
            //        }
            //        //End of Checking Masters
            //        //checking If credit and debit Notes arenot equal 
            //        mySql = "select SUM(credit) as Credit,SUM(debit) as Debit,BillNo from [" + sqlserver3 + "].[" + initialcatalog3 + "].dbo.SDC_Patient_Issue_Return" +
            //         "group by BillNo having SUM(credit) <> SUM(debit)";
            //        ds = ClsSql.GetDs2(mySql);
            //        foreach (DataRow row in ds.Tables[0].Rows)
            //        {
            //            ErrorLog("....Credit and Debit Notes are not Equal:....\n Credit Value:" + row["Credit"].ToString() + ",Debit Value:" + row["Debit"].ToString() + ",BillNo:" + row["BillNo"].ToString());
            //        }
            //        dgvNEqual.DataSource = ds.Tables[0];
            //        //Close
            //    }
            //    else
            //    {
            //        MessageBox.Show("Sales Invoice In Entry voucher Posted Successfully with document No. " + sVNo /*+ docno.Text*/, "Focus", MessageBoxButtons.OK,
            //        MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            //        //If Posting Success insert into Buffer DB 	SDC_Patient_Issue_Return
            //        dgv.DataSource = ds.Tables[0];
            //        foreach (DataGridViewRow dr in dgv.Rows)
            //        {
            //            //string Chkstr = "INSERT INTO[" + sqlserver2 + "].[Focus5030].dbo.SDC_Patient_Issue_Return([GLNm],[Credit],[Debit],[FileNo],[VisitNo],[VisitDate],[ReturnNo],[ReturnDate],[ItemId],[ItemQty],[ItemRate],[ItemTotal],[ItemType],[StoreId],[Source],[Status],[Remark],[PostingDateTime])" +
            //            //                             "SELECT [GLNm],[Credit],[Debit],[FileNo],[VisitNo],[VisitDate],[ReturnNo],[ReturnDate],[ItemId],[ItemQty],[ItemRate],[ItemTotal],[ItemType],[StoreId],[Source],[Status],[Remark],[PostingDateTime]" +
            //            //                             "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Patient_Issue_Return fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [Focus5030].dbo.SDC_Patient_Issue_Return fd)";
            //            string dtdate = Convert.ToDateTime(dr.Cells["VisitDate"].Value).ToString("yyyy-MM-dd HH:mm:ss");
            //            string RDate = Convert.ToDateTime(dr.Cells["ReturnDate"].Value).ToString("yyyy-MM-dd HH:mm:ss");
            //            mySql1 = mySql1 + "\n" + string.Format("INSERT INTO [" + sqlserver2 + "].[" + initialcatalog2 + "].dbo.SDC_Patient_Issue_Return([VoucherType],[GLNm],[Credit],[Debit],[FileNo],[VisitNo],[VisitDate],[ReturnNo],[ReturnDate],[ItemId],[ItemQty],[ItemRate],[ItemTotal],[ItemType],[StoreId],[Source],[Status],[Remark],[PostingDateTime])" +
            //                     "VALUES('" + Pstr + "','" + dr.Cells["GLnm"].Value + "'," + dr.Cells["Credit"].Value + "," + dr.Cells["Debit"].Value + "," + dr.Cells["FileNo"].Value + "," + dr.Cells["VisitNo"].Value + ",'" + dtdate + "'," + dr.Cells["ReturnNo"].Value + ",'" + RDate + "'," + dr.Cells["ItemId"].Value + ", " + dr.Cells["ItemQty"].Value + ", " + dr.Cells["ItemRate"].Value + "," + dr.Cells["ItemTotal"].Value + ",'" + dr.Cells["ItemType"].Value + "'," + dr.Cells["StoreId"].Value + ",'" + dr.Cells["Source"].Value + "','" + dr.Cells["Status"].Value + "','" + dr.Cells["Remark"].Value + "','" + dr.Cells["PostingDateTime"].Value + "')");
            //        }
            //        ChkCmd = new SqlCommand(mySql1, Chkconn);
            //        da = new SqlDataAdapter(ChkCmd);
            //        ds = new DataSet();
            //        da.Fill(ds);
            //        //Close
            //    }
            //}
            //catch (Exception ex) { ErrorLog(ex.ToString()); }
        }   //Done Posting 23/07/2019 Code Perfect
        private void Purchase_Post()
        {
            //string Pstr = "Stores";
            //var ChkConStr = ClsComm.SqlConnectionString2();
            //SqlConnection Chkconn = new SqlConnection(ChkConStr);
            //var constr2 = ClsComm.SqlConnectionString();
            //var connsplit = constr2.Split('=', ';');
            //var connsplit2 = ChkConStr.Split('=', ';');
            //for (int count = 0; count <= connsplit.Length - 1; count++)
            //{
            //    if (count % 2 == 0)
            //    { }
            //    else
            //    {
            //        sqlserver2 = connsplit[1];
            //        initialcatalog2 = connsplit[3];
            //        Companycode2 = "030";
            //        Username2 = connsplit[5];
            //        password2 = connsplit[7];
            //    }
            //}
            //for (int count = 0; count <= connsplit2.Length - 1; count++)
            //{
            //    if (count % 2 == 0)
            //    { }
            //    else
            //    {
            //        sqlserver3 = connsplit2[1];
            //        initialcatalog3 = connsplit2[3];
            //        Companycode3 = "060";
            //        Username3 = connsplit2[5];
            //        password3 = connsplit2[7];
            //    }

            //}
            //var ft = new Transaction();
            //var sVNo = string.Empty;
            //blnIsEditing = false;
            //string sTmp = "";
            //if (blnIsEditing == false)
            //{
            //    sVNo = ft.GetNextVoucherNo("Jrn");
            //    ft.NewDocument("Jrn", sVNo);
            //}
            //else
            //{
            //    ft.DeleteDocument("Jrn", sVNo);
            //}

            //ft.SetField("Date", dtpToDate.Value.ToString(/*"yyyy/MM/dd"*/));
            //ft.SetField("VoucherType Name", Pstr);
            ////Select Data from SDC_Purchase based on the bill no and comparing 
            ////mySql = "SELECT [GLNm],[Credit],[Debit],[GRNNo],[SupplierId],[PONo],[SupBillNo],[ItemId],[ItemType],[VATType],[ItemQty],[FreeQty],[ItemPrice],[Disc],[Net],[VAT],[StoreId],[GRNDate],[DueDate],[Source],[Status],[Remark],[PostingDateTime]" +
            ////   "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Purchase fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [" + sqlserver2 + "].[Focus5030].dbo.SDC_Purchase fd)";
            //mySql = "SELECT * FROM[" + sqlserver3 + "].[" + initialcatalog3 + "].dbo.SDC_Purchase fc where '" + Pstr + "'  + '^' + fc.BillNo COLLATE DATABASE_DEFAULT not in" +
            //      "(select distinct Name + '^' + BillNoYH from[" + sqlserver2 + "].[" + initialcatalog2 + "].dbo.v_BillCheck) ";
            //ds = ClsSql.GetDs2(mySql);
            //dt = ds.Tables[0];
            //dgv.AllowUserToAddRows = false;
            //dgv.DataSource = dt;
            ////Close
            //try
            //{
            //    for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
            //    {

            //        //var cell = dgv.Rows[n].Cells["GLNm"];
            //        ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString());

            //        double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value);
            //        double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value);
            //        if (str != 0)
            //        {
            //            ft.SetField("Amount", str.ToString());
            //            ft.SetField("DrCr", "Cr");
            //        }
            //        else if (str1 != 0)
            //        {
            //            ft.SetField("Amount", str1.ToString());
            //            ft.SetField("DrCr", "Dr");
            //        }
            //        var cell = dgv.Rows[n].Cells["GRNNo"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["GRNNo"].Value.ToString()) : "";
            //        ft.SetField("GRN", sTmp);

            //        cell = dgv.Rows[n].Cells["SupplierId"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["SupplierId"].Value.ToString()) : "";
            //        ft.SetField("Supplier ID", sTmp);

            //        cell = dgv.Rows[n].Cells["PONo"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["PONo"].Value.ToString()) : "";
            //        ft.SetField("PO", sTmp);

            //        cell = dgv.Rows[n].Cells["SubBillNo"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["SubBillNo"].Value.ToString()) : "";
            //        ft.SetField("Sup Bill", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemId"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemId"].Value.ToString()) : "";
            //        ft.SetField("Item ID", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemType"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemType"].Value.ToString()) : "";
            //        ft.SetField("Item Type", sTmp);

            //        cell = dgv.Rows[n].Cells["VATType"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VATType"].Value.ToString()) : "";
            //        ft.SetField("VAT Type", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemQty"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemQty"].Value.ToString()) : "";
            //        ft.SetField("Item Q", sTmp);

            //        cell = dgv.Rows[n].Cells["FreeQty"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["FreeQty"].Value.ToString()) : "";
            //        ft.SetField("Free Q", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemPrice"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemPrice"].Value.ToString()) : "";
            //        ft.SetField("Item Price", sTmp);

            //        cell = dgv.Rows[n].Cells["Disc"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Disc"].Value.ToString()) : "";
            //        ft.SetField("Disc", sTmp);

            //        cell = dgv.Rows[n].Cells["Net"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Net"].Value.ToString()) : "";
            //        ft.SetField("Net", sTmp);

            //        cell = dgv.Rows[n].Cells["VAT"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VAT"].Value.ToString()) : "";
            //        ft.SetField("VAT", sTmp);

            //        cell = dgv.Rows[n].Cells["StoreId"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["StoreId"].Value.ToString()) : "";
            //        ft.SetField("Store ID", sTmp);

            //        //Not confirmed this field visitNo is not their in JV
            //        //cell = dgv.Rows[n].Cells["GRNDate"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["GRNDate"].Value.ToString()) : "";
            //        //ft.SetField("GRN Date", sTmp);

            //        cell = dgv.Rows[n].Cells["DueDate"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["DueDate"].Value.ToString()) : "";
            //        ft.SetField("Due Date", sTmp);

            //        cell = dgv.Rows[n].Cells["Source"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString()) : "";
            //        ft.SetField("Source", sTmp);

            //        ft.AddRow();
            //    }
            //    ft.SetField("Approved", "1");
            //    int k = ft.SaveDocument();
            //    if (k != 1)
            //    {
            //        MessageBox.Show("Unable to post the Sales Invoice", "Focus", MessageBoxButtons.OK,
            //            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            //        //Checking if master is Available are Not                                  
            //        Chkconn = new SqlConnection(constr2);
            //        Chkconn.Open();
            //        mySql = "SELECT Distinct[GLNm] FROM [" + sqlserver3 + "].[" + initialcatalog3 + "].dbo.SDC_Purchase fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from [" + sqlserver2 + "].[" + initialcatalog2 + "].dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
            //        ds = ClsSql.GetDs2(mySql);
            //        var Err = new List<string>();
            //        if (dgv.Rows[0].Cells["GLNm"] == null)
            //        {
            //            ErrorLog("Already JV Posted ");
            //        }
            //        else
            //        {
            //            foreach (DataRow row in ds.Tables[0].Rows)
            //            {
            //                //ErrorLog("Account Masters are not Available in Focus:" + row["GLNm"].ToString());
            //                Err.Add(row["GLNm"].ToString());
            //            }
            //            string msg = string.Join(Environment.NewLine, Err);
            //            ErrorLog("Account Masters are not Available in Focus:Voucher Type:'SDC_Purchase'\n" + msg);
            //            Chkconn.Close();
            //        }
            //        //End of Checking Masters
            //        //checking If credit and debit Notes arenot equal 
            //        mySql = "select SUM(credit) as Credit,SUM(debit) as Debit,BillNo from [" + sqlserver3 + "].[" + initialcatalog3 + "].dbo.SDC_Purchase" +
            //         "group by BillNo having SUM(credit) <> SUM(debit)";
            //        ds = ClsSql.GetDs2(mySql);
            //        foreach (DataRow row in ds.Tables[0].Rows)
            //        {
            //            ErrorLog("....Credit and Debit Notes are not Equal:....\n Credit Value:" + row["Credit"].ToString() + ",Debit Value:" + row["Debit"].ToString() + ",BillNo:" + row["BillNo"].ToString());
            //        }
            //        dgvNEqual.DataSource = ds.Tables[0];
            //        //Close
            //    }
            //    else
            //    {
            //        MessageBox.Show("Sales Invoice In Entry voucher Posted Successfully with document No. " + sVNo /*+ docno.Text*/, "Focus", MessageBoxButtons.OK,
            //        MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            //        //If Posting Success insert into Buffer DB 	SDC_Purchase
            //        dgv.DataSource = ds.Tables[0];
            //        foreach (DataGridViewRow dr in dgv.Rows)
            //        {
            //            //string Chkstr = "INSERT INTO[" + sqlserver2 + "].[Focus5030].dbo.SDC_Purchase([GLNm],[Credit],[Debit],[GRNNo],[SupplierId],[PONo],[SupBillNo],[ItemId],[ItemType],[VATType],[ItemQty],[FreeQty],[ItemPrice],[Disc],[Net],[VAT],[StoreId],[GRNDate],[DueDate],[Source],[Status],[Remark],[PostingDateTime])" +
            //            //                             "SELECT [GLNm],[Credit],[Debit],[GRNNo],[SupplierId],[PONo],[SupBillNo],[ItemId],[ItemType],[VATType],[ItemQty],[FreeQty],[ItemPrice],[Disc],[Net],[VAT],[StoreId],[GRNDate],[DueDate],[Source],[Status],[Remark],[PostingDateTime]" +
            //            //                             "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Purchase fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [Focus5030].dbo.SDC_Purchase fd)";
            //            string dtdate = Convert.ToDateTime(dr.Cells["GRNDate"].Value).ToString("yyyy-MM-dd HH:mm:ss");
            //            string RDate = Convert.ToDateTime(dr.Cells["DueDate"].Value).ToString("yyyy-MM-dd HH:mm:ss");
            //            mySql1 = mySql1 + "\n" + string.Format("INSERT INTO [" + sqlserver2 + "].[" + initialcatalog2 + "].dbo.SDC_Purchase([VoucherType],[GLNm],[Credit],[Debit],[GRNNo],[SupplierId],[PONo],[SupBillNo],[ItemId],[ItemType],[VATType],[ItemQty],[FreeQty],[ItemPrice],[Disc],[Net],[VAT],[StoreId],[GRNDate],[DueDate],[Source],[Status],[Remark],[PostingDateTime])" +
            //                     "VALUES('" + Pstr + "','" + dr.Cells["GLnm"].Value + "'," + dr.Cells["Credit"].Value + "," + dr.Cells["Debit"].Value + "," + dr.Cells["GRNNo"].Value + "," + dr.Cells["SupplierId"].Value + ",'" + dr.Cells["PONo"].Value + "','" + dr.Cells["SupBillNo"].Value + "'," + dr.Cells["ItemId"].Value + ",'" + dr.Cells["ItemType"].Value + "','" + dr.Cells["VATType"].Value + "'," + dr.Cells["ItemQty"].Value + "," + dr.Cells["FreeQty"].Value + "," + dr.Cells["ItemPrice"].Value + "," + dr.Cells["Disc"].Value + ", " + dr.Cells["Net"].Value + ", " + dr.Cells["VAT"].Value + "," + dr.Cells["StoreId"].Value + ",'" + dtdate + "','" + RDate + "','" + dr.Cells["Source"].Value + "','" + dr.Cells["Status"].Value + "','" + dr.Cells["Remark"].Value + "','" + dr.Cells["PostingDateTime"].Value + "')");
            //        }
            //        ChkCmd = new SqlCommand(mySql1, Chkconn);
            //        da = new SqlDataAdapter(ChkCmd);
            //        ds = new DataSet();
            //        da.Fill(ds);
            //        //Close
            //    }
            //}
            //catch (Exception ex) { ErrorLog(ex.ToString()); }
        }            //Done Posting 23/07/2019 Code Perfect
        private void Purchase_rtn_Post()
        {
            //string Pstr = "Store Purchase Return";
            //var ChkConStr = ClsComm.SqlConnectionString2();
            //SqlConnection Chkconn = new SqlConnection(ChkConStr);
            //var constr2 = ClsComm.SqlConnectionString();
            //var connsplit = constr2.Split('=', ';');
            //var connsplit2 = ChkConStr.Split('=', ';');
            //for (int count = 0; count <= connsplit.Length - 1; count++)
            //{
            //    if (count % 2 == 0)
            //    { }
            //    else
            //    {
            //        sqlserver2 = connsplit[1];
            //        initialcatalog2 = connsplit[3];
            //        Companycode2 = "030";
            //        Username2 = connsplit[5];
            //        password2 = connsplit[7];
            //    }
            //}
            //for (int count = 0; count <= connsplit2.Length - 1; count++)
            //{
            //    if (count % 2 == 0)
            //    { }
            //    else
            //    {
            //        sqlserver3 = connsplit2[1];
            //        initialcatalog3 = connsplit2[3];
            //        Companycode3 = "060";
            //        Username3 = connsplit2[5];
            //        password3 = connsplit2[7];
            //    }

            //}
            //var ft = new Transaction();
            //var sVNo = string.Empty;
            //blnIsEditing = false;
            //string sTmp = "";
            //if (blnIsEditing == false)
            //{
            //    sVNo = ft.GetNextVoucherNo("Jrn");
            //    ft.NewDocument("Jrn", sVNo);
            //}
            //else
            //{
            //    ft.DeleteDocument("Jrn", sVNo);
            //}

            //ft.SetField("Date", dtpToDate.Value.ToString(/*"yyyy/MM/dd"*/));
            //ft.SetField("VoucherType Name", Pstr);
            ////Select Data from SDC_Purchase_Return based on the bill no and comparing 
            ////mySql = "SELECT [GLNm],[Credit],[Debit],[GRNNo],[SupplierId],[ReturnNo],[ReturnDate],[ItemId],[ItemType],[VATType],[ItemQty],[FreeQty],[ItemPrice],[Disc],[Net],[VAT],[StoreId],[Source],[Status],[Remark],[PostingDateTime]" +
            ////   "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Purchase_Return fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [" + sqlserver2 + "].[Focus5030].dbo.SDC_Purchase_Return fd)";
            //mySql = "SELECT * FROM[" + sqlserver3 + "].[" + initialcatalog3 + "].dbo.SDC_Purchase_Return fc where '" + Pstr + "'  + '^' + fc.BillNo COLLATE DATABASE_DEFAULT not in" +
            //     "(select distinct Name + '^' + BillNoYH from[" + sqlserver2 + "].[" + initialcatalog2 + "].dbo.v_BillCheck) ";
            //ds = ClsSql.GetDs2(mySql);
            //dt = ds.Tables[0];
            //dgv.AllowUserToAddRows = false;
            //dgv.DataSource = dt;
            ////Close
            //try
            //{
            //    for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
            //    {

            //        //var cell = dgv.Rows[n].Cells["GLNm"];
            //        ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString());

            //        double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value);
            //        double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value);
            //        if (str != 0)
            //        {
            //            ft.SetField("Amount", str.ToString());
            //            ft.SetField("DrCr", "Cr");
            //        }
            //        else if (str1 != 0)
            //        {
            //            ft.SetField("Amount", str1.ToString());
            //            ft.SetField("DrCr", "Dr");
            //        }
            //        var cell = dgv.Rows[n].Cells["GRNNo"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["GRNNo"].Value.ToString()) : "";
            //        ft.SetField("GRN", sTmp);

            //        cell = dgv.Rows[n].Cells["SupplierId"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["SupplierId"].Value.ToString()) : "";
            //        ft.SetField("Supplier ID", sTmp);

            //        //Not confirmed this field visitNo is not their in JV
            //        //cell = dgv.Rows[n].Cells["ReturnNo"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ReturnNo"].Value.ToString()) : "";
            //        //ft.SetField("Return No", sTmp);

            //        //Not confirmed this field visitNo is not their in JV
            //        //cell = dgv.Rows[n].Cells["ReturnDate"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ReturnDate"].Value.ToString()) : "";
            //        //ft.SetField("ReturnDate", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemId"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemId"].Value.ToString()) : "";
            //        ft.SetField("Item ID", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemType"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemType"].Value.ToString()) : "";
            //        ft.SetField("Item Type", sTmp);

            //        cell = dgv.Rows[n].Cells["VATType"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VATType"].Value.ToString()) : "";
            //        ft.SetField("VAT Type", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemQty"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemQty"].Value.ToString()) : "";
            //        ft.SetField("Item Q", sTmp);

            //        cell = dgv.Rows[n].Cells["FreeQty"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["FreeQty"].Value.ToString()) : "";
            //        ft.SetField("Free Q", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemPrice"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemPrice"].Value.ToString()) : "";
            //        ft.SetField("Item Price", sTmp);

            //        cell = dgv.Rows[n].Cells["Disc"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Disc"].Value.ToString()) : "";
            //        ft.SetField("Disc", sTmp);

            //        cell = dgv.Rows[n].Cells["Net"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Net"].Value.ToString()) : "";
            //        ft.SetField("Net", sTmp);

            //        cell = dgv.Rows[n].Cells["VAT"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VAT"].Value.ToString()) : "";
            //        ft.SetField("VAT", sTmp);

            //        cell = dgv.Rows[n].Cells["StoreId"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["StoreId"].Value.ToString()) : "";
            //        ft.SetField("Store ID", sTmp);

            //        cell = dgv.Rows[n].Cells["Source"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString()) : "";
            //        ft.SetField("Source", sTmp);

            //        ft.AddRow();
            //    }
            //    ft.SetField("Approved", "1");
            //    int k = ft.SaveDocument();
            //    if (k != 1)
            //    {
            //        MessageBox.Show("Unable to post the Sales Invoice", "Focus", MessageBoxButtons.OK,
            //            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            //        //Checking if master is Available are Not                                  
            //        Chkconn = new SqlConnection(constr2);
            //        Chkconn.Open();
            //        mySql = "SELECT Distinct[GLNm] FROM [" + sqlserver3 + "].["+initialcatalog3+"].dbo.SDC_Purchase_Return fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from [" + sqlserver2 + "].["+initialcatalog2+"].dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
            //        ds = ClsSql.GetDs2(mySql);
            //        var Err = new List<string>();
            //        if (dgv.Rows[0].Cells["GLNm"] == null)
            //        {
            //            ErrorLog("Already JV Posted ");
            //        }
            //        else
            //        {
            //            foreach (DataRow row in ds.Tables[0].Rows)
            //            {
            //                //ErrorLog("Account Masters are not Available in Focus:" + row["GLNm"].ToString());
            //                Err.Add(row["GLNm"].ToString());
            //            }
            //            string msg = string.Join(Environment.NewLine, Err);
            //            ErrorLog("Account Masters are not Available in Focus:Voucher Type:'SDC_Purchase_Return'\n" + msg);
            //            Chkconn.Close();
            //        }
            //        //End of Checking Masters
            //        //checking If credit and debit Notes arenot equal 
            //        mySql = "select SUM(credit) as Credit,SUM(debit) as Debit,BillNo from [" + sqlserver3 + "].[" + initialcatalog3 + "].dbo.SDC_Purchase_Return" +
            //         "group by BillNo having SUM(credit) <> SUM(debit)";
            //        ds = ClsSql.GetDs2(mySql);
            //        foreach (DataRow row in ds.Tables[0].Rows)
            //        {
            //            ErrorLog("....Credit and Debit Notes are not Equal:....\n Credit Value:" + row["Credit"].ToString() + ",Debit Value:" + row["Debit"].ToString() + ",BillNo:" + row["BillNo"].ToString());
            //        }
            //        dgvNEqual.DataSource = ds.Tables[0];
            //        //Close
            //    }
            //    else
            //    {
            //        MessageBox.Show("Sales Invoice In Entry voucher Posted Successfully with document No. " + sVNo /*+ docno.Text*/, "Focus", MessageBoxButtons.OK,
            //        MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            //        //If Posting Success insert into Buffer DB 	SDC_Purchase_Return		
            //        dgv.DataSource = ds.Tables[0];
            //        foreach (DataGridViewRow dr in dgv.Rows)
            //        {
            //            //string Chkstr = "INSERT INTO[" + sqlserver2 + "].[Focus5030].dbo.SDC_Purchase_Return([GLNm],[Credit],[Debit],[GRNNo],[SupplierId],[ReturnNo],[ReturnDate],[ItemId],[ItemType],[VATType],[ItemQty],[FreeQty],[ItemPrice],[Disc],[Net],[VAT],[StoreId],[Source],[Status],[Remark],[PostingDateTime])" +
            //            //                             "SELECT [GLNm],[Credit],[Debit],[GRNNo],[SupplierId],[ReturnNo],[ReturnDate],[ItemId],[ItemType],[VATType],[ItemQty],[FreeQty],[ItemPrice],[Disc],[Net],[VAT],[StoreId],[Source],[Status],[Remark],[PostingDateTime]" +
            //            //                             "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Purchase_Return fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [Focus5030].dbo.SDC_Purchase_Return fd)";
            //            string dtdate = Convert.ToDateTime(dr.Cells["ReturnDate"].Value).ToString("yyyy-MM-dd HH:mm:ss");
            //            //string RDate = Convert.ToDateTime(dr.Cells["DueDate"].Value).ToString("yyyy-MM-dd HH:mm:ss");
            //            mySql1 = mySql1 + "\n" + string.Format("INSERT INTO [" + sqlserver2 + "].[" + initialcatalog2 + "].dbo.SDC_Purchase_Return([VoucherType],[GLNm],[Credit],[Debit],[GRNNo],[SupplierId],[ReturnNo],[ReturnDate],[ItemId],[ItemType],[VATType],[ItemQty],[FreeQty],[ItemPrice],[Disc],[Net],[VAT],[StoreId],[Source],[Status],[Remark],[PostingDateTime])" +
            //                     "VALUES('" + Pstr + "','" + dr.Cells["GLnm"].Value + "'," + dr.Cells["Credit"].Value + "," + dr.Cells["Debit"].Value + "," + dr.Cells["GRNNo"].Value + "," + dr.Cells["SupplierId"].Value + ",'" + dr.Cells["ReturnNo"].Value + "','" + dtdate + "'," + dr.Cells["ItemId"].Value + ",'" + dr.Cells["ItemType"].Value + "','" + dr.Cells["VATType"].Value + "'," + dr.Cells["ItemQty"].Value + "," + dr.Cells["FreeQty"].Value + "," + dr.Cells["ItemPrice"].Value + "," + dr.Cells["Disc"].Value + ", " + dr.Cells["Net"].Value + ", " + dr.Cells["VAT"].Value + "," + dr.Cells["StoreId"].Value + ",'" + dr.Cells["Source"].Value + "','" + dr.Cells["Status"].Value + "','" + dr.Cells["Remark"].Value + "','" + dr.Cells["PostingDateTime"].Value + "')");
            //        }
            //        ChkCmd = new SqlCommand(mySql1, Chkconn);
            //        da = new SqlDataAdapter(ChkCmd);
            //        ds = new DataSet();
            //        da.Fill(ds);
            //        //Close	
            //    }
            //}
            //catch (Exception ex) { ErrorLog(ex.ToString()); }
        }        //Done Posting 23/07/2019 Code Perfect
        private void Refund_Post()
        {
            string sVoucherType = "Refund";
            var ChkConStr = ClsComm.sqlConectionStr_IntDB();
            SqlConnection Chkconn = new SqlConnection(ChkConStr);
            var constr2 = ClsComm.SqlConnectionString();

            //Get the data from Integrated DB
            string fDate = dtpFromDate.Value.ToString("yyyy-MM-dd");
            string tDate = dtpToDate.Value.ToString("yyyy-MM-dd");
            mySql = "select * from dbo.SDC_Refund where CAST(RefundDate AS DATE) between '" + fDate + "' and '" + tDate + "'";
            DataSet ds = ClsSql.GetDs2(mySql);
            dgv.DataSource = ds.Tables[0];

            //1st Delete the data and Insert into Buffer table through type table
            SqlConnection conn = new SqlConnection(constr2);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Sp_Insert_SDC_Refund", conn);
            cmd.Parameters.AddWithValue("@Refund", ds.Tables[0]);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            conn.Close();
            //End.

            //Checking if master is Available are Not  
            mySql = "SELECT Distinct[GLNm] FROM dbo.SDC_Refund fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
            ds = ClsSql.GetDs(mySql);
            var Err1 = new List<string>();
            if (ds.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {                   
                    Err1.Add(row["GLNm"].ToString());
                }
                string msg = string.Join(Environment.NewLine, Err1);
                string str = msg.Replace("  ", " ");
                using (StringReader reader = new StringReader(str))
                {
                    // Loop over the lines in the string.
                    string data;
                    while ((data = reader.ReadLine()) != null)
                    {
                        Create_AccMaster(data);
                    }                 
                }
            }//End of checking Masters
            double dCr_Amt = 0, dDr_Amt = 0;
            var ft = new Transaction();
            var sVNo = string.Empty;
            string sTmp = "";
            try
            {
                DataSet dst = new DataSet();
                string result = string.Format("select distinct CrNoteNo from dbo.SDC_Refund where '" + sVoucherType + "' + '^' + CrNoteNo not in(select distinct Name + '^' + BillNoYH from dbo.v_BillCheck)");
                dst = ClsSql.GetDs(result);
                DataTable dt2 = dst.Tables[0];
                string sBillNo = "";
                Boolean blnPost = false;
                for (int z = 0; z < dst.Tables[0].Rows.Count; z++)
                {
                    dCr_Amt = 0;
                    dDr_Amt = 0;
                    sBillNo = dst.Tables[0].Rows[z]["CrNoteNo"].ToString();
                    //checking data is exist or not not exist then posting inot focus
                    mySql = "SELECT * FROM dbo.SDC_Refund fc where CrNoteNo='" + sBillNo + "'";
                    ds = ClsSql.GetDs(mySql);
                    dt = ds.Tables[0];
                    dgv.AllowUserToAddRows = false;
                    dgv.DataSource = dt;
                    for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
                    {
                        if (n == 0)
                        {
                            sVNo = ft.GetNextVoucherNo("Jrn");
                            ft.NewDocument("Jrn", sVNo);

                            ft.SetField("Date", dgv.Rows[0].Cells["RefundDate"].Value.ToString().Trim());
                            ft.SetField("VoucherType Name", sVoucherType);
                        }
                        //var cell = dgv.Rows[n].Cells["GLNm"];
                        ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString().Trim());

                        double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value.ToString().Trim());
                        double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value.ToString().Trim());
                        if (str != 0)
                        {
                            ft.SetField("Amount", str.ToString());
                            ft.SetField("DrCr", "Cr");
                            dCr_Amt = dCr_Amt + str;
                        }
                        else if (str1 != 0)
                        {
                            ft.SetField("Amount", str1.ToString());
                            ft.SetField("DrCr", "Dr");
                            dDr_Amt = dDr_Amt + str1;
                        }

                        var cell = dgv.Rows[n].Cells["PayeeId"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["PayeeId"].Value.ToString().Trim()) : "";
                        ft.SetField("Payee ID", sTmp);

                        cell = dgv.Rows[n].Cells["PayeeType"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["PayeeType"].Value.ToString().Trim()) : "";
                        ft.SetField("Payee Type", sTmp);

                        ft.SetField("Bill No", sBillNo);

                        cell = dgv.Rows[n].Cells["CrNoteDate"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["CrNoteDate"].Value.ToString().Trim()) : "";
                        ft.SetField("Cr Note Date", sTmp);

                        cell = dgv.Rows[n].Cells["RefundDate"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["RefundDate"].Value.ToString().Trim()) : "";
                        ft.SetField("Refund Date", sTmp);

                        cell = dgv.Rows[n].Cells["RefundNo"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["RefundNo"].Value.ToString().Trim()) : "";
                        ft.SetField("Refund No", sTmp);

                        cell = dgv.Rows[n].Cells["FileNo"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["FileNo"].Value.ToString().Trim()) : "";
                        ft.SetField("File No", sTmp);

                        cell = dgv.Rows[n].Cells["VisitId"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VisitId"].Value.ToString().Trim()) : "";
                        ft.SetField("Visit ID", sTmp);

                        cell = dgv.Rows[n].Cells["Nationality"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Nationality"].Value.ToString().Trim()) : "";
                        ft.SetField("Nationality", sTmp);

                        cell = dgv.Rows[n].Cells["CrNoteId"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["CrNoteId"].Value.ToString().Trim()) : "";
                        ft.SetField("Cr Note ID", sTmp);

                        cell = dgv.Rows[n].Cells["VatAmt"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VatAmt"].Value.ToString().Trim()) : "";
                        ft.SetField("Vat Amt", sTmp);

                        cell = dgv.Rows[n].Cells["CashierId"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["CashierId"].Value.ToString().Trim()) : "";
                        ft.SetField("Cashier ID", sTmp);

                        cell = dgv.Rows[n].Cells["Source"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString().Trim()) : "";
                        ft.SetField("Source", sTmp);

                        ft.AddRow();
                        blnPost = true;
                    }
                    if (blnPost == true)
                    {
                        ft.SetField("Approved", "1");
                        int k = ft.SaveDocument();
                        if (k != 1)
                        {
                            if (dDr_Amt != dCr_Amt)
                            {
                                blnErr_Log = true;
                                ErrorLog("Unable To Post the Bill : " + sVoucherType + "-" + sBillNo + ", Debit and Credit Values are not Equal ");
                            }
                            else
                            {
                                blnErr_Log = true;
                                ErrorLog("Unable To Post the Bill : " + sVoucherType + "-" + sBillNo + ", Check the Entry and masters");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                blnErr_Log = true;
                ErrorLog(ex.Message);           
            }
        }
        private void Revenue_Canc_Post()
        {
            string sVoucherType = "Canc Rev";
            var ChkConStr = ClsComm.sqlConectionStr_IntDB();
            SqlConnection Chkconn = new SqlConnection(ChkConStr);
            var constr2 = ClsComm.SqlConnectionString();

            string fDate = dtpFromDate.Value.ToString("yyyy-MM-dd");
            string tDate = dtpToDate.Value.ToString("yyyy-MM-dd");
            //Get the data from Integrated DB
            mySql = "select * from dbo.SDC_Revenue_Canc where CAST(BillDate AS DATE) between '" + fDate + "' and '" + tDate + "'";
            DataSet ds = ClsSql.GetDs2(mySql);
            dgv.DataSource = ds.Tables[0];

            //1st data is delete and Insert into Buffer table through type table
            SqlConnection conn = new SqlConnection(constr2);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Sp_Insert_SDC_Revenue_Canc", conn);
            cmd.Parameters.AddWithValue("@Revenue_Canc", ds.Tables[0]);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            conn.Close();
            //End.

            //Checking if master is Available are Not  
            mySql = "SELECT Distinct[GLNm] FROM dbo.SDC_Revenue_Canc fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
            ds = ClsSql.GetDs(mySql);
            var Err1 = new List<string>();
            if (ds.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {                    
                    Err1.Add(row["GLNm"].ToString());
                }
                string msg = string.Join(Environment.NewLine, Err1);                
                string str = msg.Replace("  ", " ");                
                using (StringReader reader = new StringReader(str))
                {
                    // Loop over the lines in the string.
                    string data;
                    while ((data = reader.ReadLine()) != null)
                    {
                        Create_AccMaster(data);
                    }                   
                }
            }//end of checking masters
            double dCr_Amt = 0, dDr_Amt = 0;
            var ft = new Transaction();
            var sVNo = string.Empty;
            string sTmp = "";
            try
            {
                DataSet dst = new DataSet();
                string result = string.Format("select distinct BillNo from dbo.SDC_Revenue_Canc where '" + sVoucherType + "' + '^' + BillNo not in(select distinct Name + '^' + BillNoYH from dbo.v_BillCheck)");
                dst = ClsSql.GetDs(result);
                DataTable dt2 = dst.Tables[0];
                string sBillNo = "";
                Boolean blnPost = false;
                for (int z = 0; z < dst.Tables[0].Rows.Count; z++)
                {
                    dCr_Amt = 0;
                    dDr_Amt = 0;
                    sBillNo = dst.Tables[0].Rows[z]["BillNo"].ToString();
                    //checking if data is exist or not if not existed data posting into focus
                    mySql = "SELECT * FROM dbo.SDC_Revenue_Canc fc where BillNo='" + sBillNo + "'";
                    ds = ClsSql.GetDs(mySql);
                    dt = ds.Tables[0];
                    dgv.AllowUserToAddRows = false;
                    dgv.DataSource = dt;

                    for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
                    {
                        if (n == 0)
                        {
                            sVNo = ft.GetNextVoucherNo("Jrn");
                            ft.NewDocument("Jrn", sVNo);

                            ft.SetField("Date", dgv.Rows[0].Cells["BillDate"].Value.ToString().Trim());
                            ft.SetField("VoucherType Name", sVoucherType);
                        }
                        //var cell = dgv.Rows[n].Cells["GLNm"];
                        ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString().Trim());
                        
                        double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value.ToString().Trim());
                        double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value.ToString().Trim());
                        if (str != 0)
                        {
                            ft.SetField("Amount", str.ToString());
                            ft.SetField("DrCr", "Cr");
                            dCr_Amt = dCr_Amt + str;
                        }
                        else if (str1 != 0)
                        {
                            ft.SetField("Amount", str1.ToString());
                            ft.SetField("DrCr", "Dr");
                            dDr_Amt = dDr_Amt + str1;
                        }

                        var cell = dgv.Rows[n].Cells["PayeeId"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["PayeeId"].Value.ToString().Trim()) : "";
                        ft.SetField("Payee ID", sTmp);

                        cell = dgv.Rows[n].Cells["PayeeType"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["PayeeType"].Value.ToString().Trim()) : "";
                        ft.SetField("Payee Type", sTmp);

                        ft.SetField("Bill No", sBillNo);

                        cell = dgv.Rows[n].Cells["BillDate"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["BillDate"].Value.ToString().Trim()) : "";
                        ft.SetField("Bill Date", sTmp);

                        cell = dgv.Rows[n].Cells["BillType"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["BillType"].Value.ToString().Trim()) : "";
                        ft.SetField("Bill Type", sTmp);

                        cell = dgv.Rows[n].Cells["FileNo"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["FileNo"].Value.ToString().Trim()) : "";
                        ft.SetField("File No", sTmp);

                        cell = dgv.Rows[n].Cells["VisitId"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VisitId"].Value.ToString().Trim()) : "";
                        ft.SetField("Visit ID", sTmp);

                        cell = dgv.Rows[n].Cells["Nationality"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Nationality"].Value.ToString().Trim()) : "";
                        ft.SetField("Nationality", sTmp);

                        cell = dgv.Rows[n].Cells["ServCode"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ServCode"].Value.ToString().Trim()) : "";
                        ft.SetField("Serv Code", sTmp);

                        cell = dgv.Rows[n].Cells["DeptId"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["DeptId"].Value.ToString().Trim()) : "";
                        ft.SetField("Dept ID", sTmp);

                        cell = dgv.Rows[n].Cells["NetAmt"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["NetAmt"].Value.ToString().Trim()) : "";
                        ft.SetField("Net Amt", sTmp);

                        cell = dgv.Rows[n].Cells["CashierId"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["CashierId"].Value.ToString().Trim()) : "";
                        ft.SetField("Cashier ID", sTmp);

                        cell = dgv.Rows[n].Cells["DrId"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["DrId"].Value.ToString().Trim()) : "";
                        ft.SetField("Dr ID", sTmp);

                        cell = dgv.Rows[n].Cells["CancelDate"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["CancelDate"].Value.ToString().Trim()) : "";
                        ft.SetField("Cancel Date", sTmp);

                        //cell = dgv.Rows[n].Cells["CancelerId"];// Nul value handling
                        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["CancelerId"].Value.ToString()) : "";
                        //ft.SetField("Cancel ID", sTmp);

                        cell = dgv.Rows[n].Cells["Source"];// Nul value handling
                        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString().Trim()) : "";
                        ft.SetField("Source", sTmp);
                        //ft.SetField("Status", dgv.Rows[n].Cells["Status"].Value.ToString());

                        ft.AddRow();
                        blnPost = true;
                    }
                    if (blnPost == true)
                    {
                        ft.SetField("Approved", "1");
                        int k = ft.SaveDocument();
                        if (k != 1)
                        {
                            if (dDr_Amt != dCr_Amt)
                            {
                                blnErr_Log = true;
                                ErrorLog("Unable To Post the Bill : " + sVoucherType + "-" + sBillNo + ", Debit and Credit Values are not Equal ");
                            }
                            else
                            {
                                blnErr_Log = true;
                                ErrorLog("Unable To Post the Bill : " + sVoucherType + "-" + sBillNo + ", Check the Entry and masters");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                blnErr_Log = true;
                ErrorLog(ex.Message);
            }
        }
        private void Stock_adj_Post()
        {
            //string Pstr = "Store Adjustment";
            //var ChkConStr = ClsComm.SqlConnectionString2();
            //SqlConnection Chkconn = new SqlConnection(ChkConStr);
            //var constr2 = ClsComm.SqlConnectionString();
            //var connsplit = constr2.Split('=', ';');
            //var connsplit2 = ChkConStr.Split('=', ';');
            //for (int count = 0; count <= connsplit.Length - 1; count++)
            //{
            //    if (count % 2 == 0)
            //    { }
            //    else
            //    {
            //        sqlserver2 = connsplit[1];
            //        initialcatalog2 = connsplit[3];
            //        Companycode2 = "030";
            //        Username2 = connsplit[5];
            //        password2 = connsplit[7];
            //    }
            //}
            //for (int count = 0; count <= connsplit2.Length - 1; count++)
            //{
            //    if (count % 2 == 0)
            //    { }
            //    else
            //    {
            //        sqlserver3 = connsplit2[1];
            //        initialcatalog3 = connsplit2[3];
            //        Companycode3 = "060";
            //        Username3 = connsplit2[5];
            //        password3 = connsplit2[7];
            //    }

            //}
            //var ft = new Transaction();
            //var sVNo = string.Empty;
            //blnIsEditing = false;
            //string sTmp = "";
            //if (blnIsEditing == false)
            //{
            //    sVNo = ft.GetNextVoucherNo("Jrn");
            //    ft.NewDocument("Jrn", sVNo);
            //}
            //else
            //{
            //    ft.DeleteDocument("Jrn", sVNo);
            //}

            //ft.SetField("Date", dtpToDate.Value.ToString(/*"yyyy/MM/dd"*/));
            //ft.SetField("VoucherType Name", Pstr);
            ////Select Data from SDC_Stock_Adjust based on the bill no and comparing
            ////mySql = "SELECT [GLNm],[Credit],[Debit],[AdjNo],[AdjDate],[AdjStoreId],[AdjItemId],[ItemQty],[ItemRate],[TotalAdj],[ItemType],[Source],[Status],[Remark],[PostingDateTime]" +
            ////   "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Stock_Adjust fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [" + sqlserver2 + "].[Focus5030].dbo.SDC_Stock_Adjust fd)";
            //mySql = "SELECT * FROM[" + sqlserver3 + "].[" + initialcatalog3 + "].dbo.SDC_Stock_Adjust fc where '" + Pstr + "'  + '^' + fc.CrNoteNo COLLATE DATABASE_DEFAULT not in" +
            //       "(select distinct Name + '^' + BillNoYH from[" + sqlserver2 + "].[" + initialcatalog2 + "].dbo.v_BillCheck) ";
            //ds = ClsSql.GetDs2(mySql);
            //dt = ds.Tables[0];
            //dgv.AllowUserToAddRows = false;
            //dgv.DataSource = dt;
            ////Close
            //try
            //{
            //    for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
            //    {

            //        //var cell = dgv.Rows[n].Cells["GLNm"];
            //        ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString());

            //        double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value);
            //        double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value);
            //        if (str != 0)
            //        {
            //            ft.SetField("Amount", str.ToString());
            //            ft.SetField("DrCr", "Cr");
            //        }
            //        else if (str1 != 0)
            //        {
            //            ft.SetField("Amount", str1.ToString());
            //            ft.SetField("DrCr", "Dr");
            //        }
            //        //var cell = dgv.Rows[n].Cells["AdjNo"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["AdjNo"].Value.ToString()) : "";
            //        //ft.SetField("AdjNo", sTmp);

            //        //cell = dgv.Rows[n].Cells["AdjDate"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["AdjDate"].Value.ToString()) : "";
            //        //ft.SetField("Adj Date", sTmp);

            //        //cell = dgv.Rows[n].Cells["AdjStoreId"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["AdjStoreId"].Value.ToString()) : "";
            //        //ft.SetField("AdjStore Id", sTmp);

            //        //cell = dgv.Rows[n].Cells["AdjItemId"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["AdjItemId"].Value.ToString()) : "";
            //        //ft.SetField("AdjItem Id", sTmp);

            //        var cell = dgv.Rows[n].Cells["ItemQty"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemQty"].Value.ToString()) : "";
            //        ft.SetField("Item Q", sTmp);

            //        //cell = dgv.Rows[n].Cells["ItemRate"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemRate"].Value.ToString()) : "";
            //        //ft.SetField("Item Rate", sTmp);

            //        //cell = dgv.Rows[n].Cells["TotalAdj"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["TotalAdj"].Value.ToString()) : "";
            //        //ft.SetField("TotalAdj", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemType"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemType"].Value.ToString()) : "";
            //        ft.SetField("Item Type", sTmp);

            //        cell = dgv.Rows[n].Cells["Source"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString()) : "";
            //        ft.SetField("Source", sTmp);

            //        ft.AddRow();
            //    }
            //    ft.SetField("Approved", "1");
            //    int k = ft.SaveDocument();
            //    if (k != 1)
            //    {
            //        MessageBox.Show("Unable to post the Sales Invoice", "Focus", MessageBoxButtons.OK,
            //            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            //        //Checking if master is Available are Not                                  
            //        Chkconn = new SqlConnection(constr2);
            //        Chkconn.Open();
            //        mySql = "SELECT Distinct[GLNm] FROM [" + sqlserver3 + "].["+initialcatalog3+"].dbo.SDC_Stock_Adjust fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from [" + sqlserver2 + "].["+initialcatalog2+"].dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
            //        ds = ClsSql.GetDs2(mySql);
            //        var Err = new List<string>();
            //        if (dgv.Rows[0].Cells["GLNm"] == null)
            //        {
            //            ErrorLog("Already JV Posted ");
            //        }
            //        else
            //        {
            //            foreach (DataRow row in ds.Tables[0].Rows)
            //            {
            //                // ErrorLog("Account Masters are not Available in Focus:" + row["GLNm"].ToString());
            //                Err.Add(row["GLNm"].ToString());
            //            }
            //            string msg = string.Join(Environment.NewLine, Err);
            //            ErrorLog("Account Masters are not Available in Focus:Voucher Type:'SDC_Stock_Adjust'\n" + msg);
            //            Chkconn.Close();
            //        }
            //        //End of Checking Masters
            //        //checking If credit and debit Notes arenot equal 
            //        mySql = "select SUM(credit) as Credit,SUM(debit) as Debit,BillNo from [" + sqlserver3 + "].[" + initialcatalog3 + "].dbo.SDC_Stock_Adjust" +
            //         "group by BillNo having SUM(credit) <> SUM(debit)";
            //        ds = ClsSql.GetDs2(mySql);
            //        foreach (DataRow row in ds.Tables[0].Rows)
            //        {
            //            ErrorLog("....Credit and Debit Notes are not Equal:....\n Credit Value:" + row["Credit"].ToString() + ",Debit Value:" + row["Debit"].ToString() + ",BillNo:" + row["BillNo"].ToString());
            //        }
            //        dgvNEqual.DataSource = ds.Tables[0];
            //        //Close
            //    }
            //    else
            //    {
            //        MessageBox.Show("Sales Invoice In Entry voucher Posted Successfully with document No. " + sVNo /*+ docno.Text*/, "Focus", MessageBoxButtons.OK,
            //        MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            //        //If Posting Success insert into Buffer DB 	SDC_Stock_Adjust
            //        dgv.DataSource = ds.Tables[0];
            //        foreach (DataGridViewRow dr in dgv.Rows)
            //        {
            //            //string Chkstr = "INSERT INTO[" + sqlserver2 + "].[Focus5030].dbo.SDC_Stock_Adjust([GLNm],[Credit],[Debit],[AdjNo],[AdjDate],[AdjStoreId],[AdjItemId],[ItemQty],[ItemRate],[TotalAdj],[ItemType],[Source],[Status],[Remark],[PostingDateTime])" +
            //            //                             "SELECT [GLNm],[Credit],[Debit],[AdjNo],[AdjDate],[AdjStoreId],[AdjItemId],[ItemQty],[ItemRate],[TotalAdj],[ItemType],[Source],[Status],[Remark],[PostingDateTime]" +
            //            //                             "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Stock_Adjust fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [Focus5030].dbo.SDC_Stock_Adjust fd)";
            //            string dtdate = Convert.ToDateTime(dr.Cells["AdjDate"].Value).ToString("yyyy-MM-dd HH:mm:ss");
            //            //string RDate = Convert.ToDateTime(dr.Cells["CancelDate"].Value).ToString("yyyy-MM-dd HH:mm:ss");
            //            mySql1 = mySql1 + "\n" + string.Format("INSERT INTO [" + sqlserver2 + "].[" + initialcatalog2 + "].dbo.SDC_Stock_Adjust([VoucherType],[GLNm],[Credit],[Debit],[AdjNo],[AdjDate],[AdjStoreId],[AdjItemId],[ItemQty],[ItemRate],[TotalAdj],[ItemType],[Source],[Status],[Remark],[PostingDateTime])" +
            //                     "VALUES('" + Pstr + "','" + dr.Cells["GLnm"].Value + "'," + dr.Cells["Credit"].Value + "," + dr.Cells["Debit"].Value + "," + dr.Cells["AdjNo"].Value + ",'" + dtdate + "'," + dr.Cells["AdjStoreId"].Value + "," + dr.Cells["AdjItemId"].Value + "," + dr.Cells["ItemQty"].Value + "," + dr.Cells["ItemRate"].Value + "," + dr.Cells["TotalAdj"].Value + ",'" + dr.Cells["ItemType"].Value + "','" + dr.Cells["Source"].Value + "','" + dr.Cells["Status"].Value + "','" + dr.Cells["Remark"].Value + "','" + dr.Cells["PostingDateTime"].Value + "')");
            //        }
            //        ChkCmd = new SqlCommand(mySql1, Chkconn);
            //        da = new SqlDataAdapter(ChkCmd);
            //        ds = new DataSet();
            //        da.Fill(ds);
            //        //Close	
            //    }
            //}
            //catch (Exception ex) { ErrorLog(ex.ToString()); }
        }     //Done Posting 23/07/2019  Code Perfect
        private void Stock_Consum_Post()
        {
            //string Pstr = "Store Consumption";
            //var ChkConStr = ClsComm.SqlConnectionString2();
            //SqlConnection Chkconn = new SqlConnection(ChkConStr);
            //var constr2 = ClsComm.SqlConnectionString();
            //var connsplit = constr2.Split('=', ';');
            //var connsplit2 = ChkConStr.Split('=', ';');
            //for (int count = 0; count <= connsplit.Length - 1; count++)
            //{
            //    if (count % 2 == 0)
            //    { }
            //    else
            //    {
            //        sqlserver2 = connsplit[1];
            //        initialcatalog2 = connsplit[3];
            //        Companycode2 = "030";
            //        Username2 = connsplit[5];
            //        password2 = connsplit[7];
            //    }
            //}
            //for (int count = 0; count <= connsplit2.Length - 1; count++)
            //{
            //    if (count % 2 == 0)
            //    { }
            //    else
            //    {
            //        sqlserver3 = connsplit2[1];
            //        initialcatalog3 = connsplit2[3];
            //        Companycode3 = "060";
            //        Username3 = connsplit2[5];
            //        password3 = connsplit2[7];
            //    }

            //}
            //var ft = new Transaction();
            //var sVNo = string.Empty;
            //blnIsEditing = false;
            //string sTmp = "";
            //if (blnIsEditing == false)
            //{
            //    sVNo = ft.GetNextVoucherNo("Jrn");
            //    ft.NewDocument("Jrn", sVNo);
            //}
            //else
            //{
            //    ft.DeleteDocument("Jrn", sVNo);
            //}

            //ft.SetField("Date", dtpToDate.Value.ToString(/*"yyyy/MM/dd"*/));
            //ft.SetField("VoucherType Name", Pstr);
            ////Select Data from SDC_Stock_Consum based on the bill no and comparing 
            ////mySql = "SELECT [GLNm],[Credit],[Debit],[ConsNo],[ConsDate],[ConsStoreId],[ItemId],[ItemRate],[ItemQty],[TotalCons],[ItemType],[Source],[Status],[Remark],[PostingDateTime]" +
            ////   "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Stock_Consum fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [" + sqlserver2 + "].[Focus5030].dbo.SDC_Stock_Consum fd)";
            //mySql = "SELECT * FROM[" + sqlserver3 + "].[" + initialcatalog3 + "].dbo.SDC_Stock_Consum fc where '" + Pstr + "'  + '^' + fc.BillNo COLLATE DATABASE_DEFAULT not in" +
            //      "(select distinct Name + '^' + BillNoYH from[" + sqlserver2 + "].[" + initialcatalog2 + "].dbo.v_BillCheck) ";
            //ds = ClsSql.GetDs2(mySql);
            //dt = ds.Tables[0];
            //dgv.AllowUserToAddRows = false;
            //dgv.DataSource = dt;
            ////Close
            //try
            //{
            //    for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
            //    {

            //        //var cell = dgv.Rows[n].Cells["GLNm"];
            //        ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString());

            //        double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value);
            //        double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value);
            //        if (str != 0)
            //        {
            //            ft.SetField("Amount", str.ToString());
            //            ft.SetField("DrCr", "Cr");
            //        }
            //        else if (str1 != 0)
            //        {
            //            ft.SetField("Amount", str1.ToString());
            //            ft.SetField("DrCr", "Dr");
            //        }
            //        //var cell = dgv.Rows[n].Cells["ConsNo"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ConsNo"].Value.ToString()) : "";
            //        //ft.SetField("Cons No", sTmp);

            //        //cell = dgv.Rows[n].Cells["ConsDate"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ConsDate"].Value.ToString()) : "";
            //        //ft.SetField("Cons Date", sTmp);

            //        //cell = dgv.Rows[n].Cells["ConsStoreId"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ConsStoreId"].Value.ToString()) : "";
            //        //ft.SetField("ConsStore Id", sTmp);

            //        var cell = dgv.Rows[n].Cells["ItemId"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemId"].Value.ToString()) : "";
            //        ft.SetField("Item ID", sTmp);

            //        //cell = dgv.Rows[n].Cells["ItemRate"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemRate"].Value.ToString()) : "";
            //        //ft.SetField("Item Rate", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemQty"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemQty"].Value.ToString()) : "";
            //        ft.SetField("Item Q", sTmp);

            //        //cell = dgv.Rows[n].Cells["TotalCons"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["TotalCons"].Value.ToString()) : "";
            //        //ft.SetField("TotalCons", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemType"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemType"].Value.ToString()) : "";
            //        ft.SetField("Item Type", sTmp);

            //        cell = dgv.Rows[n].Cells["Source"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString()) : "";
            //        ft.SetField("Source", sTmp);

            //        ft.AddRow();
            //    }
            //    ft.SetField("Approved", "1");
            //    int k = ft.SaveDocument();
            //    if (k != 1)
            //    {
            //        MessageBox.Show("Unable to post the Sales Invoice", "Focus", MessageBoxButtons.OK,
            //            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            //        //Checking if master is Available are Not                                  
            //        Chkconn = new SqlConnection(constr2);
            //        Chkconn.Open();
            //        mySql = "SELECT Distinct[GLNm] FROM [" + sqlserver3 + "].["+initialcatalog3+"].dbo.SDC_Stock_Consum fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from [" + sqlserver2 + "].["+initialcatalog2+"].dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
            //        ds = ClsSql.GetDs2(mySql);
            //        var Err = new List<string>();
            //        if (dgv.Rows[0].Cells["GLNm"] == null)
            //        {
            //            ErrorLog("Already JV Posted ");
            //        }
            //        else
            //        {
            //            foreach (DataRow row in ds.Tables[0].Rows)
            //            {
            //                //ErrorLog("Account Masters are not Available in Focus:" + row["GLNm"].ToString());
            //                Err.Add(row["GLNm"].ToString());
            //            }
            //            string msg = string.Join(Environment.NewLine, Err);
            //            ErrorLog("Account Masters are not Available in Focus:Voucher Type:'SDC_Stock_Consum'\n" + msg);
            //            Chkconn.Close();
            //        }
            //        //End of Checking Masters
            //        //checking If credit and debit Notes arenot equal 
            //        mySql = "select SUM(credit) as Credit,SUM(debit) as Debit,BillNo from [" + sqlserver3 + "].[" + initialcatalog3 + "].dbo.SDC_Stock_Consum" +
            //         "group by BillNo having SUM(credit) <> SUM(debit)";
            //        ds = ClsSql.GetDs(mySql);
            //        foreach (DataRow row in ds.Tables[0].Rows)
            //        {
            //            ErrorLog("....Credit and Debit Notes are not Equal:....\n Credit Value:" + row["Credit"].ToString() + ",Debit Value:" + row["Debit"].ToString() + ",BillNo:" + row["BillNo"].ToString());
            //        }
            //        dgvNEqual.DataSource = ds.Tables[0];
            //        //Close
            //    }
            //    else
            //    {
            //        MessageBox.Show("Sales Invoice In Entry voucher Posted Successfully with document No. " + sVNo /*+ docno.Text*/, "Focus", MessageBoxButtons.OK,
            //        MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            //        //If Posting Success insert into Buffer DB 	SDC_Stock_Consum
            //        dgv.DataSource = ds.Tables[0];
            //        foreach (DataGridViewRow dr in dgv.Rows)
            //        {
            //            //string Chkstr = "INSERT INTO[" + sqlserver2 + "].[Focus5030].dbo.SDC_Stock_Consum([GLNm],[Credit],[Debit],[ConsNo],[ConsDate],[ConsStoreId],[ItemId],[ItemRate],[ItemQty],[TotalCons],[ItemType],[Source],[Status],[Remark],[PostingDateTime])" +
            //            //                             "SELECT [GLNm],[Credit],[Debit],[ConsNo],[ConsDate],[ConsStoreId],[ItemId],[ItemRate],[ItemQty],[TotalCons],[ItemType],[Source],[Status],[Remark],[PostingDateTime]" +
            //            //                             "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Stock_Consum fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [Focus5030].dbo.SDC_Stock_Consum fd)";
            //            string dtdate = Convert.ToDateTime(dr.Cells["ConsDate"].Value).ToString("yyyy-MM-dd HH:mm:ss");
            //            string RDate = Convert.ToDateTime(dr.Cells["CancelDate"].Value).ToString("yyyy-MM-dd HH:mm:ss");
            //            mySql1 = mySql1 + "\n" + string.Format("INSERT INTO [" + sqlserver2 + "].[" + initialcatalog2 + "].dbo.SDC_Stock_Consum([VoucherType],[GLNm],[Credit],[Debit],[ConsNo],[ConsDate],[ConsStoreId],[ItemId],[ItemRate],[ItemQty],[TotalCons],[ItemType],[Source],[Status],[Remark],[PostingDateTime])" +
            //                     "VALUES('" + Pstr + "','" + dr.Cells["GLnm"].Value + "'," + dr.Cells["Credit"].Value + "," + dr.Cells["Debit"].Value + "," + dr.Cells["ConsNo"].Value + ",'" + dtdate + "'," + dr.Cells["ConsStoreId"].Value + "," + dr.Cells["ItemId"].Value + "," + dr.Cells["ItemRate"].Value + "," + dr.Cells["ItemQty"].Value + "," + dr.Cells["TotalCons"].Value + ",'" + dr.Cells["ItemType"].Value + "','" + dr.Cells["Source"].Value + "','" + dr.Cells["Status"].Value + "','" + dr.Cells["Remark"].Value + "','" + dr.Cells["PostingDateTime"].Value + "')");
            //        }
            //        ChkCmd = new SqlCommand(mySql1, Chkconn);
            //        da = new SqlDataAdapter(ChkCmd);
            //        ds = new DataSet();
            //        da.Fill(ds);
            //        //Close
            //    }
            //}
            //catch (Exception ex) { ErrorLog(ex.ToString()); }
        }   //Done Posting 23/07/2019  Code Perfect
        private void Stock_Consum_rtn_Post()
        {
            //string Pstr = "Store Consumption Return";
            //var ChkConStr = ClsComm.SqlConnectionString2();
            //SqlConnection Chkconn = new SqlConnection(ChkConStr);
            //var constr2 = ClsComm.SqlConnectionString();
            //var connsplit = constr2.Split('=', ';');
            //var connsplit2 = ChkConStr.Split('=', ';');
            //for (int count = 0; count <= connsplit.Length - 1; count++)
            //{
            //    if (count % 2 == 0)
            //    { }
            //    else
            //    {
            //        sqlserver2 = connsplit[1];
            //        initialcatalog2 = connsplit[3];
            //        Companycode2 = "030";
            //        Username2 = connsplit[5];
            //        password2 = connsplit[7];
            //    }
            //}
            //for (int count = 0; count <= connsplit2.Length - 1; count++)
            //{
            //    if (count % 2 == 0)
            //    { }
            //    else
            //    {
            //        sqlserver3 = connsplit2[1];
            //        initialcatalog3 = connsplit2[3];
            //        Companycode3 = "060";
            //        Username3 = connsplit2[5];
            //        password3 = connsplit2[7];
            //    }

            //}
            //var ft = new Transaction();
            //var sVNo = string.Empty;
            //blnIsEditing = false;
            //string sTmp = "";
            //if (blnIsEditing == false)
            //{
            //    sVNo = ft.GetNextVoucherNo("Jrn");
            //    ft.NewDocument("Jrn", sVNo);
            //}
            //else
            //{
            //    ft.DeleteDocument("Jrn", sVNo);
            //}

            //ft.SetField("Date", dtpToDate.Value.ToString(/*"yyyy/MM/dd"*/));
            //ft.SetField("VoucherType Name", Pstr);
            ////Select Data from SDC_Stock_Consum_Return based on the bill no and comparing
            ////mySql = "SELECT [GLNm],[Credit],[Debit],[ConsNo],[ConsDate],[ConsStoreId],[ItemId],[ItemRate],[ItemQty],[TotalCons],[ItemType],[ConsCancDate],[ConsCancNo],[Source],[Status],[Remark],[PostingDateTime]" +
            ////   "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Stock_Consum_Return fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [" + sqlserver2 + "].[Focus5030].dbo.SDC_Stock_Consum_Return fd)";
            //mySql = "SELECT * FROM[" + sqlserver3 + "].[" + initialcatalog3 + "].dbo.SDC_Stock_Consum_Return fc where '" + Pstr + "'  + '^' + fc.BillNo COLLATE DATABASE_DEFAULT not in" +
            //    "(select distinct Name + '^' + BillNoYH from[" + sqlserver2 + "].[" + initialcatalog2 + "].dbo.v_BillCheck) ";
            //ds = ClsSql.GetDs2(mySql);
            //dt = ds.Tables[0];
            //dgv.AllowUserToAddRows = false;
            //dgv.DataSource = dt;
            ////Close
            //try
            //{
            //    for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
            //    {

            //        //var cell = dgv.Rows[n].Cells["GLNm"];
            //        ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString());

            //        double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value);
            //        double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value);
            //        if (str != 0)
            //        {
            //            ft.SetField("Amount", str.ToString());
            //            ft.SetField("DrCr", "Cr");
            //        }
            //        else if (str1 != 0)
            //        {
            //            ft.SetField("Amount", str1.ToString());
            //            ft.SetField("DrCr", "Dr");
            //        }
            //        //var cell = dgv.Rows[n].Cells["ConsNo"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ConsNo"].Value.ToString()) : "";
            //        //ft.SetField("Cons No", sTmp);

            //        //cell = dgv.Rows[n].Cells["ConsDate"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ConsDate"].Value.ToString()) : "";
            //        //ft.SetField("Cons Date", sTmp);

            //        //cell = dgv.Rows[n].Cells["ConsStoreId"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ConsStoreId"].Value.ToString()) : "";
            //        //ft.SetField("ConsStore Id", sTmp);

            //        var cell = dgv.Rows[n].Cells["ItemId"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemId"].Value.ToString()) : "";
            //        ft.SetField("Item ID", sTmp);

            //        //cell = dgv.Rows[n].Cells["ItemRate"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemRate"].Value.ToString()) : "";
            //        //ft.SetField("Item Rate", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemQty"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemQty"].Value.ToString()) : "";
            //        ft.SetField("Item Q", sTmp);

            //        //cell = dgv.Rows[n].Cells["TotalCons"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["TotalCons"].Value.ToString()) : "";
            //        //ft.SetField("TotalCons", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemType"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemType"].Value.ToString()) : "";
            //        ft.SetField("Item Type", sTmp);

            //        //cell = dgv.Rows[n].Cells["ConsCancDate"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ConsCancDate"].Value.ToString()) : "";
            //        //ft.SetField("ConsCancDate", sTmp);

            //        //cell = dgv.Rows[n].Cells["ConsCancNo"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ConsCancNo"].Value.ToString()) : "";
            //        //ft.SetField("ConsCancNo", sTmp);

            //        cell = dgv.Rows[n].Cells["Source"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString()) : "";
            //        ft.SetField("Source", sTmp);

            //        ft.AddRow();
            //    }
            //    ft.SetField("Approved", "1");
            //    int k = ft.SaveDocument();
            //    if (k != 1)
            //    {
            //        MessageBox.Show("Unable to post the Sales Invoice", "Focus", MessageBoxButtons.OK,
            //            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            //        //Checking if master is Available are Not                                  
            //        Chkconn = new SqlConnection(constr2);
            //        Chkconn.Open();
            //        mySql = "SELECT Distinct[GLNm] FROM [" + sqlserver3 + "].["+initialcatalog3+"].dbo.SDC_Stock_Consum_Return fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from [" + sqlserver2 + "].["+initialcatalog2+"].dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
            //        ds = ClsSql.GetDs2(mySql);
            //        var Err = new List<string>();
            //        if (dgv.Rows[0].Cells["GLNm"] == null)
            //        {
            //            ErrorLog("Already JV Posted ");
            //        }
            //        else
            //        {
            //            foreach (DataRow row in ds.Tables[0].Rows)
            //            {
            //                // ErrorLog("Account Masters are not Available in Focus:" + row["GLNm"].ToString());
            //                Err.Add(row["GLNm"].ToString());
            //            }
            //            string msg = string.Join(Environment.NewLine, Err);
            //            ErrorLog("Account Masters are not Available in Focus:Voucher Type:'SDC_Stock_Consum_Return'\n" + msg);
            //            Chkconn.Close();
            //        }
            //        //End of Checking Masters
            //        //checking If credit and debit Notes arenot equal 
            //        mySql = "select SUM(credit) as Credit,SUM(debit) as Debit,BillNo from [" + sqlserver3 + "].[" + initialcatalog3 + "].dbo.SDC_Stock_Consum_Return" +
            //         "group by BillNo having SUM(credit) <> SUM(debit)";
            //        ds = ClsSql.GetDs2(mySql);
            //        foreach (DataRow row in ds.Tables[0].Rows)
            //        {
            //            ErrorLog("....Credit and Debit Notes are not Equal:....\n Credit Value:" + row["Credit"].ToString() + ",Debit Value:" + row["Debit"].ToString() + ",BillNo:" + row["BillNo"].ToString());
            //        }
            //        dgvNEqual.DataSource = ds.Tables[0];
            //        //Close
            //    }
            //    else
            //    {
            //        MessageBox.Show("Sales Invoice In Entry voucher Posted Successfully with document No. " + sVNo /*+ docno.Text*/, "Focus", MessageBoxButtons.OK,
            //        MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            //        //If Posting Success insert into Buffer DB 	SDC_Stock_Consum_Return	
            //        dgv.DataSource = ds.Tables[0];
            //        foreach (DataGridViewRow dr in dgv.Rows)
            //        {
            //            //string Chkstr = "INSERT INTO[" + sqlserver2 + "].[Focus5030].dbo.SDC_Stock_Consum_Return([GLNm],[Credit],[Debit],[ConsNo],[ConsDate],[ConsStoreId],[ItemId],[ItemRate],[ItemQty],[TotalCons],[ItemType],[ConsCancDate],[ConsCancNo],[Source],[Status],[Remark],[PostingDateTime])" +
            //            //                             "SELECT [GLNm],[Credit],[Debit],[ConsNo],[ConsDate],[ConsStoreId],[ItemId],[ItemRate],[ItemQty],[TotalCons],[ItemType],[ConsCancDate],[ConsCancNo],[Source],[Status],[Remark],[PostingDateTime]" +
            //            //                             "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Stock_Consum_Return fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [Focus5030].dbo.SDC_Stock_Consum_Return fd)";
            //            string dtdate = Convert.ToDateTime(dr.Cells["ConsDate"].Value).ToString("yyyy-MM-dd HH:mm:ss");
            //            string RDate = Convert.ToDateTime(dr.Cells["ConsCancDate"].Value).ToString("yyyy-MM-dd HH:mm:ss");
            //            mySql1 = mySql1 + "\n" + string.Format("INSERT INTO [" + sqlserver2 + "].[" + initialcatalog2 + "].dbo.SDC_Stock_Consum_Return([VoucherType],[GLNm],[Credit],[Debit],[ConsNo],[ConsDate],[ConsStoreId],[ItemId],[ItemRate],[ItemQty],[TotalCons],[ItemType],[ConsCancDate],[ConsCancNo],[Source],[Status],[Remark],[PostingDateTime])" +
            //                     "VALUES('" + Pstr + "','" + dr.Cells["GLnm"].Value + "'," + dr.Cells["Credit"].Value + "," + dr.Cells["Debit"].Value + "," + dr.Cells["ConsNo"].Value + ",'" + dtdate + "'," + dr.Cells["ConsStoreId"].Value + "," + dr.Cells["ItemId"].Value + "," + dr.Cells["ItemRate"].Value + "," + dr.Cells["ItemQty"].Value + "," + dr.Cells["TotalCons"].Value + ",'" + dr.Cells["ItemType"].Value + "','"+RDate+"',"+dr.Cells["ConsCancNo"].Value +",'" + dr.Cells["Source"].Value + "','" + dr.Cells["Status"].Value + "','" + dr.Cells["Remark"].Value + "','" + dr.Cells["PostingDateTime"].Value + "')");
            //        }
            //        ChkCmd = new SqlCommand(mySql1, Chkconn);
            //        da = new SqlDataAdapter(ChkCmd);
            //        ds = new DataSet();
            //        da.Fill(ds);
            //        //Close
            //    }
            //}
            //catch (Exception ex) { ErrorLog(ex.ToString()); }
        }    // Done Posting 23/07/2019  Code Perfect
        private void Stock_dispose_Post()
        {
            //string Pstr = "Store Dispose";
            //var ChkConStr = ClsComm.SqlConnectionString2();
            //SqlConnection Chkconn = new SqlConnection(ChkConStr);
            //var constr2 = ClsComm.SqlConnectionString();
            //var connsplit = constr2.Split('=', ';');
            //var connsplit2 = ChkConStr.Split('=', ';');
            //for (int count = 0; count <= connsplit.Length - 1; count++)
            //{
            //    if (count % 2 == 0)
            //    { }
            //    else
            //    {
            //        sqlserver2 = connsplit[1];
            //        initialcatalog2 = connsplit[3];
            //        Companycode2 = "030";
            //        Username2 = connsplit[5];
            //        password2 = connsplit[7];
            //    }
            //}
            //for (int count = 0; count <= connsplit2.Length - 1; count++)
            //{
            //    if (count % 2 == 0)
            //    { }
            //    else
            //    {
            //        sqlserver3 = connsplit2[1];
            //        initialcatalog3 = connsplit2[3];
            //        Companycode3 = "060";
            //        Username3 = connsplit2[5];
            //        password3 = connsplit2[7];
            //    }

            //}
            //var ft = new Transaction();
            //var sVNo = string.Empty;
            //blnIsEditing = false;
            //string sTmp = "";
            //if (blnIsEditing == false)
            //{
            //    sVNo = ft.GetNextVoucherNo("Jrn");
            //    ft.NewDocument("Jrn", sVNo);
            //}
            //else
            //{
            //    ft.DeleteDocument("Jrn", sVNo);
            //}

            //ft.SetField("Date", dtpToDate.Value.ToString(/*"yyyy/MM/dd"*/));
            //ft.SetField("VoucherType Name", Pstr);
            ////Select Data from SDC_Stock_Dispose based on the bill no and comparing 
            ////mySql = "SELECT [GLNm],[Credit],[Debit],[DisposeId],[DisposeNo],[DisposeStoreId],[ItemId],[ItemRate],[ItemQty],[TotalDispose],[ItemType],[Source],[Status],[Remark],[PostingDateTime]" +
            ////   "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Stock_Dispose fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [" + sqlserver2 + "].[Focus5030].dbo.SDC_Stock_Dispose fd)";
            //mySql = "SELECT * FROM[" + sqlserver3 + "].[" + initialcatalog3 + "].dbo.SDC_Stock_Dispose fc where '" + Pstr + "'  + '^' + fc.BillNo COLLATE DATABASE_DEFAULT not in" +
            // "(select distinct Name + '^' + BillNoYH from[" + sqlserver2 + "].[" + initialcatalog2 + "].dbo.v_BillCheck) ";
            //ds = ClsSql.GetDs2(mySql);
            //dt = ds.Tables[0];
            //dgv.AllowUserToAddRows = false;
            //dgv.DataSource = dt;
            ////Close
            //try
            //{
            //    for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
            //    {

            //        //var cell = dgv.Rows[n].Cells["GLNm"];
            //        ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString());

            //        double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value);
            //        double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value);
            //        if (str != 0)
            //        {
            //            ft.SetField("Amount", str.ToString());
            //            ft.SetField("DrCr", "Cr");
            //        }
            //        else if (str1 != 0)
            //        {
            //            ft.SetField("Amount", str1.ToString());
            //            ft.SetField("DrCr", "Dr");
            //        }
            //        //var cell = dgv.Rows[n].Cells["DisposeId"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["DisposeId"].Value.ToString()) : "";
            //        //ft.SetField("Dispose Id", sTmp);

            //        //cell = dgv.Rows[n].Cells["DisposeNo"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["DisposeNo"].Value.ToString()) : "";
            //        //ft.SetField("Dispose No", sTmp);

            //        //cell = dgv.Rows[n].Cells["DisposeStoreId"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["DisposeStoreId"].Value.ToString()) : "";
            //        //ft.SetField("DisposeStore Id", sTmp);

            //        //cell = dgv.Rows[n].Cells["DisposeDate"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["DisposeDate"].Value.ToString()) : "";
            //        //ft.SetField("Dispose Date", sTmp);

            //        var cell = dgv.Rows[n].Cells["ItemId"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemId"].Value.ToString()) : "";
            //        ft.SetField("Item ID", sTmp);

            //        //cell = dgv.Rows[n].Cells["ItemRate"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemRate"].Value.ToString()) : "";
            //        //ft.SetField("Item Rate", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemQty"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemQty"].Value.ToString()) : "";
            //        ft.SetField("Item Q", sTmp);

            //        //cell = dgv.Rows[n].Cells["TotalDispose"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["TotalDispose"].Value.ToString()) : "";
            //        //ft.SetField("TotalDispose", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemType"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemType"].Value.ToString()) : "";
            //        ft.SetField("Item Type", sTmp);

            //        cell = dgv.Rows[n].Cells["Source"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString()) : "";
            //        ft.SetField("Source", sTmp);

            //        ft.AddRow();
            //    }
            //    ft.SetField("Approved", "1");
            //    int k = ft.SaveDocument();
            //    if (k != 1)
            //    {
            //        MessageBox.Show("Unable to post the Sales Invoice", "Focus", MessageBoxButtons.OK,
            //            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            //        //Checking if master is Available are Not                                  
            //        Chkconn = new SqlConnection(constr2);
            //        Chkconn.Open();
            //        mySql = "SELECT Distinct[GLNm] FROM [" + sqlserver3 + "].["+initialcatalog3+"].dbo.SDC_Stock_Dispose fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from [" + sqlserver2 + "].["+initialcatalog2+"].dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
            //        ds = ClsSql.GetDs2(mySql);
            //        var Err = new List<string>();
            //        if (dgv.Rows[0].Cells["GLNm"] == null)
            //        {
            //            ErrorLog("Already JV Posted ");
            //        }
            //        else
            //        {
            //            foreach (DataRow row in ds.Tables[0].Rows)
            //            {
            //                //ErrorLog("Account Masters are not Available in Focus:" + row["GLNm"].ToString());
            //                Err.Add(row["GLNm"].ToString());
            //            }
            //            string msg = string.Join(Environment.NewLine, Err);
            //            ErrorLog("Account Masters are not Available in Focus:Voucher Type:'SDC_Stock_Dispose'\n" + msg);
            //            Chkconn.Close();
            //        }
            //        //End of Checking Masters
            //        //checking If credit and debit Notes arenot equal 
            //        mySql = "select SUM(credit) as Credit,SUM(debit) as Debit,BillNo from [" + sqlserver3 + "].[" + initialcatalog3 + "].dbo.SDC_Stock_Dispose" +
            //         "group by BillNo having SUM(credit) <> SUM(debit)";
            //        ds = ClsSql.GetDs(mySql);
            //        foreach (DataRow row in ds.Tables[0].Rows)
            //        {
            //            ErrorLog("....Credit and Debit Notes are not Equal:....\n Credit Value:" + row["Credit"].ToString() + ",Debit Value:" + row["Debit"].ToString() + ",BillNo:" + row["BillNo"].ToString());
            //        }
            //        dgvNEqual.DataSource = ds.Tables[0];
            //        //Close
            //    }
            //    else
            //    {
            //        MessageBox.Show("Sales Invoice In Entry voucher Posted Successfully with document No. " + sVNo /*+ docno.Text*/, "Focus", MessageBoxButtons.OK,
            //        MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            //        //If Posting Success insert into Buffer DB 	SDC_Stock_Dispose
            //        dgv.DataSource = ds.Tables[0];
            //        foreach (DataGridViewRow dr in dgv.Rows)
            //        {
            //            //string Chkstr = "INSERT INTO[" + sqlserver2 + "].[Focus5030].dbo.SDC_Stock_Dispose([GLNm],[Credit],[Debit],[DisposeId],[DisposeNo],[DisposeStoreId],[ItemId],[ItemRate],[ItemQty],[TotalDispose],[ItemType],[Source],[Status],[Remark],[PostingDateTime])" +
            //            //                             "SELECT [GLNm],[Credit],[Debit],[DisposeId],[DisposeNo],[DisposeStoreId],[ItemId],[ItemRate],[ItemQty],[TotalDispose],[ItemType],[Source],[Status],[Remark],[PostingDateTime]" +
            //            //                             "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Stock_Dispose fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [Focus5030].dbo.SDC_Stock_Dispose fd)";
            //            //string dtdate = Convert.ToDateTime(dr.Cells["ConsDate"].Value).ToString("yyyy-MM-dd HH:mm:ss");
            //            //string RDate = Convert.ToDateTime(dr.Cells["ConsCancDate"].Value).ToString("yyyy-MM-dd HH:mm:ss");
            //            mySql1 = mySql1 + "\n" + string.Format("INSERT INTO [" + sqlserver2 + "].[" + initialcatalog2 + "].dbo.SDC_Stock_Consum_Return([VoucherType],[GLNm],[Credit],[Debit],[DisposeId],[DisposeNo],[DisposeStoreId],[ItemId],[ItemRate],[ItemQty],[TotalDispose],[ItemType],[Source],[Status],[Remark],[PostingDateTime])" +
            //                     "VALUES('" + Pstr + "','" + dr.Cells["GLnm"].Value + "'," + dr.Cells["Credit"].Value + "," + dr.Cells["Debit"].Value + "," + dr.Cells["DisposeId"].Value + ",'" + dr.Cells["DisposeNo"].Value + "'," + dr.Cells["DisposeStoreId"].Value + "," + dr.Cells["ItemId"].Value + "," + dr.Cells["ItemRate"].Value + "," + dr.Cells["ItemQty"].Value + "," + dr.Cells["TotalDispose"].Value + ",'" + dr.Cells["ItemType"].Value + "','" + dr.Cells["Source"].Value + "','" + dr.Cells["Status"].Value + "','" + dr.Cells["Remark"].Value + "','" + dr.Cells["PostingDateTime"].Value + "')");
            //        }
            //        ChkCmd = new SqlCommand(mySql1, Chkconn);
            //        da = new SqlDataAdapter(ChkCmd);
            //        ds = new DataSet();
            //        da.Fill(ds);
            //        //Close
            //    }
            //}
            //catch (Exception ex) { ErrorLog(ex.ToString()); }
        }   // Done Posting 23/07/2019  Code Perfect
        private void Stock_Issue_Post()
        {
            //string Pstr = "Store Issue";
            //var ChkConStr = ClsComm.SqlConnectionString2();
            //SqlConnection Chkconn = new SqlConnection(ChkConStr);
            //var constr2 = ClsComm.SqlConnectionString();
            //var connsplit = constr2.Split('=', ';');
            //var connsplit2 = ChkConStr.Split('=', ';');
            //for (int count = 0; count <= connsplit.Length - 1; count++)
            //{
            //    if (count % 2 == 0)
            //    { }
            //    else
            //    {
            //        sqlserver2 = connsplit[1];
            //        initialcatalog2 = connsplit[3];
            //        Companycode2 = "030";
            //        Username2 = connsplit[5];
            //        password2 = connsplit[7];
            //    }
            //}
            //for (int count = 0; count <= connsplit2.Length - 1; count++)
            //{
            //    if (count % 2 == 0)
            //    { }
            //    else
            //    {
            //        sqlserver3 = connsplit2[1];
            //        initialcatalog3 = connsplit2[3];
            //        Companycode3 = "060";
            //        Username3 = connsplit2[5];
            //        password3 = connsplit2[7];
            //    }

            //}
            //var ft = new Transaction();
            //var sVNo = string.Empty;
            //blnIsEditing = false;
            //string sTmp = "";
            //if (blnIsEditing == false)
            //{
            //    sVNo = ft.GetNextVoucherNo("Jrn");
            //    ft.NewDocument("Jrn", sVNo);
            //}
            //else
            //{
            //    ft.DeleteDocument("Jrn", sVNo);
            //}

            //ft.SetField("Date", dtpToDate.Value.ToString(/*"yyyy/MM/dd"*/));
            //ft.SetField("VoucherType Name", Pstr);
            ////Select Data from SDC_Stock_Issue based on the bill no and comparing 
            ////mySql = "SELECT [GLNm],[Credit],[Debit],[IssueNo],[IssueDate],[ReqNo],[OutStoreId],[InStoreId],[ItemId],[ItemQty],[TotalIssue],[ItemType],[Source],[Status],[Remark],[PostingDateTime]" +
            ////   "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Stock_Issue fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [" + sqlserver2 + "].[Focus5030].dbo.SDC_Stock_Issue fd)";
            //mySql = "SELECT * FROM[" + sqlserver3 + "].[" + initialcatalog3 + "].dbo.SDC_Stock_Issue fc where '" + Pstr + "'  + '^' + fc.BillNo COLLATE DATABASE_DEFAULT not in" +
            //      "(select distinct Name + '^' + BillNoYH from[" + sqlserver2 + "].[" + initialcatalog2 + "].dbo.v_BillCheck) ";
            //ds = ClsSql.GetDs2(mySql);
            //dt = ds.Tables[0];
            //dgv.AllowUserToAddRows = false;
            ////Close
            //try
            //{
            //    for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
            //    {

            //        //var cell = dgv.Rows[n].Cells["GLNm"];
            //        ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString());

            //        double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value);
            //        double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value);
            //        if (str != 0)
            //        {
            //            ft.SetField("Amount", str.ToString());
            //            ft.SetField("DrCr", "Cr");
            //        }
            //        else if (str1 != 0)
            //        {
            //            ft.SetField("Amount", str1.ToString());
            //            ft.SetField("DrCr", "Dr");
            //        }
            //        var cell = dgv.Rows[n].Cells["IssueNo"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["IssueNo"].Value.ToString()) : "";
            //        ft.SetField("P Issue", sTmp);

            //        cell = dgv.Rows[n].Cells["IssueDate"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["IssueDate"].Value.ToString()) : "";
            //        ft.SetField("P Issue Date", sTmp);

            //        cell = dgv.Rows[n].Cells["ReqNo"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ReqNo"].Value.ToString()) : "";
            //        ft.SetField("Req", sTmp);

            //        cell = dgv.Rows[n].Cells["OutStoreId"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["OutStoreId"].Value.ToString()) : "";
            //        ft.SetField("Out Store ID", sTmp);

            //        cell = dgv.Rows[n].Cells["InStoreId"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["InStoreId"].Value.ToString()) : "";
            //        ft.SetField("In Store ID", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemId"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemId"].Value.ToString()) : "";
            //        ft.SetField("Item ID", sTmp);

            //        //cell = dgv.Rows[n].Cells["ItemRate"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemRate"].Value.ToString()) : "";
            //        //ft.SetField("Item Rate", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemQty"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemQty"].Value.ToString()) : "";
            //        ft.SetField("Item Q", sTmp);

            //        //cell = dgv.Rows[n].Cells["TotalIssue"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["TotalIssue"].Value.ToString()) : "";
            //        //ft.SetField("Total Issue", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemType"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemType"].Value.ToString()) : "";
            //        ft.SetField("Item Type", sTmp);

            //        cell = dgv.Rows[n].Cells["Source"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString()) : "";
            //        ft.SetField("Source", sTmp);

            //        ft.AddRow();
            //    }
            //    ft.SetField("Approved", "1");
            //    int k = ft.SaveDocument();
            //    if (k != 1)
            //    {
            //        MessageBox.Show("Unable to post the Sales Invoice", "Focus", MessageBoxButtons.OK,
            //            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            //        //Checking if master is Available are Not                                  
            //        Chkconn = new SqlConnection(constr2);
            //        Chkconn.Open();
            //        mySql = "SELECT Distinct[GLNm] FROM [" + sqlserver3 + "].["+initialcatalog3+"].dbo.SDC_Stock_Issue fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from [" + sqlserver2 + "].["+initialcatalog2+"].dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
            //        ds = ClsSql.GetDs2(mySql);
            //        var Err = new List<string>();
            //        if (dgv.Rows[0].Cells["GLNm"] == null)
            //        {
            //            ErrorLog("Already JV Posted ");
            //        }
            //        else
            //        {
            //            foreach (DataRow row in ds.Tables[0].Rows)
            //            {
            //                //ErrorLog("Account Masters are not Available in Focus:" + row["GLNm"].ToString());
            //                Err.Add(row["GLNm"].ToString());
            //            }
            //            string msg = string.Join(Environment.NewLine, Err);
            //            ErrorLog("Account Masters are not Available in Focus:Voucher Type:'SDC_Stock_Issue'\n" + msg);
            //            Chkconn.Close();
            //        }
            //        //End of Checking Masters
            //        //checking If credit and debit Notes arenot equal 
            //        mySql = "select SUM(credit) as Credit,SUM(debit) as Debit,BillNo from [" + sqlserver3 + "].[" + initialcatalog3 + "].dbo.SDC_Stock_Issue" +
            //         "group by BillNo having SUM(credit) <> SUM(debit)";
            //        ds = ClsSql.GetDs2(mySql);
            //        foreach (DataRow row in ds.Tables[0].Rows)
            //        {
            //            ErrorLog("....Credit and Debit Notes are not Equal:....\n Credit Value:" + row["Credit"].ToString() + ",Debit Value:" + row["Debit"].ToString() + ",BillNo:" + row["BillNo"].ToString());
            //        }
            //        dgvNEqual.DataSource = ds.Tables[0];
            //        //Close
            //    }
            //    else
            //    {
            //        MessageBox.Show("Sales Invoice In Entry voucher Posted Successfully with document No. " + sVNo /*+ docno.Text*/, "Focus", MessageBoxButtons.OK,
            //        MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            //        //If Posting Success insert into Buffer DB 	SDC_Stock_Issue	
            //        dgv.DataSource = ds.Tables[0];
            //        foreach (DataGridViewRow dr in dgv.Rows)
            //        {
            //            string Chkstr = "INSERT INTO[" + sqlserver2 + "].[Focus5030].dbo.SDC_Stock_Issue([GLNm],[Credit],[Debit],[IssueNo],[IssueDate],[ReqNo],[OutStoreId],[InStoreId],[ItemId],[ItemQty],[TotalIssue],[ItemType],[Source],[Status],[Remark],[PostingDateTime])" +
            //                                         "SELECT [GLNm],[Credit],[Debit],[IssueNo],[IssueDate],[ReqNo],[OutStoreId],[InStoreId],[ItemId],[ItemQty],[TotalIssue],[ItemType],[Source],[Status],[Remark],[PostingDateTime]" +
            //                                         "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Stock_Issue fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [Focus5030].dbo.SDC_Stock_Issue fd)";
            //            string dtdate = Convert.ToDateTime(dr.Cells["IssueDate"].Value).ToString("yyyy-MM-dd HH:mm:ss");
            //            //string RDate = Convert.ToDateTime(dr.Cells["CancelDate"].Value).ToString("yyyy-MM-dd HH:mm:ss");
            //            mySql1 = mySql1 + "\n" + string.Format("INSERT INTO [" + sqlserver2 + "].[" + initialcatalog2 + "].dbo.SDC_Stock_Issue([VoucherType],[GLNm],[Credit],[Debit],[IssueNo],[IssueDate],[ReqNo],[OutStoreId],[InStoreId],[ItemId],[ItemQty],[TotalIssue],[ItemType],[Source],[Status],[Remark],[PostingDateTime])" +
            //                "VALUES('" + Pstr + "','" + dr.Cells["GLnm"].Value + "'," + dr.Cells["Credit"].Value + "," + dr.Cells["Debit"].Value + "," + dr.Cells["IssueNo"].Value + ",'"+dtdate+"','" + dr.Cells["ReqNo"].Value + "',"+dr.Cells["OutStoreId"].Value+"," + dr.Cells["InStoreId"].Value + "," + dr.Cells["ItemId"].Value + "," + dr.Cells["ItemQty"].Value + "," + dr.Cells["TotalIssue"].Value + ",'" + dr.Cells["ItemType"].Value + "','" + dr.Cells["Source"].Value + "','" + dr.Cells["Status"].Value + "','" + dr.Cells["Remark"].Value + "','" + dr.Cells["PostingDateTime"].Value + "')");
            //        }
            //        ChkCmd = new SqlCommand(mySql1, Chkconn);
            //        da = new SqlDataAdapter(ChkCmd);
            //        ds = new DataSet();
            //        da.Fill(ds);
            //        //Close	
            //    }
            //}
            //catch (Exception ex) { ErrorLog(ex.ToString()); }
        }     //Done Posting 23/07/2019 code Perfect 
        private void Stock_Issue_rtn_Post()
        {
            //string Pstr = "Store Issue Return";
            //var ChkConStr = ClsComm.SqlConnectionString2();
            //SqlConnection Chkconn = new SqlConnection(ChkConStr);
            //var constr2 = ClsComm.SqlConnectionString();
            //var connsplit = constr2.Split('=', ';');
            //var connsplit2 = ChkConStr.Split('=', ';');
            //for (int count = 0; count <= connsplit.Length - 1; count++)
            //{
            //    if (count % 2 == 0)
            //    { }
            //    else
            //    {
            //        sqlserver2 = connsplit[1];
            //        initialcatalog2 = connsplit[3];
            //        Companycode2 = "030";
            //        Username2 = connsplit[5];
            //        password2 = connsplit[7];
            //    }
            //}
            //for (int count = 0; count <= connsplit2.Length - 1; count++)
            //{
            //    if (count % 2 == 0)
            //    { }
            //    else
            //    {
            //        sqlserver3 = connsplit2[1];
            //        initialcatalog3 = connsplit2[3];
            //        Companycode3 = "060";
            //        Username3 = connsplit2[5];
            //        password3 = connsplit2[7];
            //    }

            //}
            //var ft = new Transaction();
            //var sVNo = string.Empty;
            //blnIsEditing = false;
            //string sTmp = "";
            //if (blnIsEditing == false)
            //{
            //    sVNo = ft.GetNextVoucherNo("Jrn");
            //    ft.NewDocument("Jrn", sVNo);
            //}
            //else
            //{
            //    ft.DeleteDocument("Jrn", sVNo);
            //}

            //ft.SetField("Date", dtpToDate.Value.ToString(/*"yyyy/MM/dd"*/));
            //ft.SetField("VoucherType Name", Pstr);
            ////Select Data from SDC_Stock_Issue_Return based on the bill no and comparing
            ////mySql = "SELECT [GLNm],[Credit],[Debit],[MatRetNo],[MatRetDate],[OutStoreId],[InStoreId],[ItemId],[ItemRate],[ItemQty],[TotalReturn],[ItemType],[Source],[Status],[Remark],[PostingDateTime]" +
            ////   "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Stock_Issue_Return fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [" + sqlserver2 + "].[Focus5030].dbo.SDC_Stock_Issue_Return fd)";
            //mySql = "SELECT * FROM[" + sqlserver3 + "].[" + initialcatalog3 + "].dbo.SDC_Stock_Issue_Return fc where '" + Pstr + "'  + '^' + fc.BillNo COLLATE DATABASE_DEFAULT not in" +
            //     "(select distinct Name + '^' + BillNoYH from[" + sqlserver2 + "].[" + initialcatalog2 + "].dbo.v_BillCheck) ";
            //ds = ClsSql.GetDs2(mySql);
            //dt = ds.Tables[0];
            //dgv.AllowUserToAddRows = false;
            //dgv.DataSource = dt;
            ////Close
            //try
            //{
            //    for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
            //    {

            //        //var cell = dgv.Rows[n].Cells["GLNm"];
            //        ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString());

            //        double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value);
            //        double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value);
            //        if (str != 0)
            //        {
            //            ft.SetField("Amount", str.ToString());
            //            ft.SetField("DrCr", "Cr");
            //        }
            //        else if (str1 != 0)
            //        {
            //            ft.SetField("Amount", str1.ToString());
            //            ft.SetField("DrCr", "Dr");
            //        }
            //        //var cell = dgv.Rows[n].Cells["MatMetNo"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["MatMetNo"].Value.ToString()) : "";
            //        //ft.SetField("MatMetNo", sTmp);

            //        //cell = dgv.Rows[n].Cells["MatMetNo"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["MatMetNo"].Value.ToString()) : "";
            //        //ft.SetField("MatMet Date", sTmp);

            //        var cell = dgv.Rows[n].Cells["OutStoreId"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["OutStoreId"].Value.ToString()) : "";
            //        ft.SetField("Out Store ID", sTmp);

            //        cell = dgv.Rows[n].Cells["InStoreId"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["InStoreId"].Value.ToString()) : "";
            //        ft.SetField("In Store ID", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemId"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemId"].Value.ToString()) : "";
            //        ft.SetField("Item ID", sTmp);

            //        //cell = dgv.Rows[n].Cells["ItemRate"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemRate"].Value.ToString()) : "";
            //        //ft.SetField("Item Rate", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemQty"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemQty"].Value.ToString()) : "";
            //        ft.SetField("Item Q", sTmp);

            //        //cell = dgv.Rows[n].Cells["TotalReturn"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["TotalReturn"].Value.ToString()) : "";
            //        //ft.SetField("TotalReturn", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemType"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemType"].Value.ToString()) : "";
            //        ft.SetField("Item Type", sTmp);

            //        cell = dgv.Rows[n].Cells["Source"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString()) : "";
            //        ft.SetField("Source", sTmp);

            //        ft.AddRow();
            //    }
            //    ft.SetField("Approved", "1");
            //    int k = ft.SaveDocument();
            //    if (k != 1)
            //    {
            //        MessageBox.Show("Unable to post the Sales Invoice", "Focus", MessageBoxButtons.OK,
            //            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            //        //Checking if master is Available are Not                                  
            //        Chkconn = new SqlConnection(constr2);
            //        Chkconn.Open();
            //        mySql = "SELECT Distinct[GLNm] FROM [" + sqlserver3 + "].["+initialcatalog3+"].dbo.SDC_Stock_Issue_Return fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from [" + sqlserver2 + "].["+initialcatalog2+"].dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
            //        ds = ClsSql.GetDs2(mySql);
            //        var Err = new List<string>();
            //        if (dgv.Rows[0].Cells["GLNm"] == null)
            //        {
            //            ErrorLog("Already JV Posted ");
            //        }
            //        else
            //        {
            //            foreach (DataRow row in ds.Tables[0].Rows)
            //            {
            //                // ErrorLog("Account Masters are not Available in Focus:" + row["GLNm"].ToString());
            //                Err.Add(row["GLNm"].ToString());
            //            }
            //            string msg = string.Join(Environment.NewLine, Err);
            //            ErrorLog("Account Masters are not Available in Focus:Voucher Type:'Discount'\n" + msg);
            //            Chkconn.Close();
            //        }
            //        //End of Checking Masters
            //        //checking If credit and debit Notes arenot equal 
            //        mySql = "select SUM(credit) as Credit,SUM(debit) as Debit,BillNo from [" + sqlserver3 + "].[" + initialcatalog3 + "].dbo.SDC_Stock_Issue_Return" +
            //         "group by BillNo having SUM(credit) <> SUM(debit)";
            //        ds = ClsSql.GetDs2(mySql);
            //        foreach (DataRow row in ds.Tables[0].Rows)
            //        {
            //            ErrorLog("....Credit and Debit Notes are not Equal:....\n Credit Value:" + row["Credit"].ToString() + ",Debit Value:" + row["Debit"].ToString() + ",BillNo:" + row["BillNo"].ToString());
            //        }
            //        dgvNEqual.DataSource = ds.Tables[0];
            //        //Close
            //    }
            //    else
            //    {
            //        MessageBox.Show("Sales Invoice In Entry voucher Posted Successfully with document No. " + sVNo /*+ docno.Text*/, "Focus", MessageBoxButtons.OK,
            //        MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            //        //If Posting Success insert into Buffer DB 	SDC_Stock_Issue_Return		
            //        dgv.DataSource = ds.Tables[0];
            //        foreach (DataGridViewRow dr in dgv.Rows)
            //        {
            //            //string Chkstr = "INSERT INTO[" + sqlserver2 + "].[Focus5030].dbo.SDC_Stock_Issue_Return([GLNm],[Credit],[Debit],[MatRetNo],[MatRetDate],[OutStoreId],[InStoreId],[ItemId],[ItemRate],[ItemQty],[TotalReturn],[ItemType],[Source],[Status],[Remark],[PostingDateTime])" +
            //            //                             "SELECT [GLNm],[Credit],[Debit],[MatRetNo],[MatRetDate],[OutStoreId],[InStoreId],[ItemId],[ItemRate],[ItemQty],[TotalReturn],[ItemType],[Source],[Status],[Remark],[PostingDateTime]" +
            //            //                             "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Stock_Issue_Return fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [Focus5030].dbo.SDC_Stock_Issue_Return fd)";
            //            string dtdate = Convert.ToDateTime(dr.Cells["MatRetDate"].Value).ToString("yyyy-MM-dd HH:mm:ss");
            //            //string RDate = Convert.ToDateTime(dr.Cells["CancelDate"].Value).ToString("yyyy-MM-dd HH:mm:ss");
            //            mySql1 = mySql1 + "\n" + string.Format("INSERT INTO [" + sqlserver2 + "].[" + initialcatalog2 + "].dbo.SDC_Stock_Issue([VoucherType],[GLNm],[Credit],[Debit],[MatRetNo],[MatRetDate],[OutStoreId],[InStoreId],[ItemId],[ItemRate],[ItemQty],[TotalReturn],[ItemType],[Source],[Status],[Remark],[PostingDateTime])" +
            //               "VALUES('" + Pstr + "','" + dr.Cells["GLnm"].Value + "'," + dr.Cells["Credit"].Value + "," + dr.Cells["Debit"].Value + "," + dr.Cells["MatRetNo"].Value + ",'" + dtdate + "'," + dr.Cells["OutStoreId"].Value + "," + dr.Cells["InStoreId"].Value + "," + dr.Cells["ItemId"].Value + "," + dr.Cells["ItemRate"].Value + "," + dr.Cells["ItemQty"].Value + "," + dr.Cells["TotalReturn"].Value + ",'" + dr.Cells["ItemType"].Value + "','" + dr.Cells["Source"].Value + "','" + dr.Cells["Status"].Value + "','" + dr.Cells["Remark"].Value + "','" + dr.Cells["PostingDateTime"].Value + "')");
            //        }
            //        ChkCmd = new SqlCommand(mySql1, Chkconn);
            //        da = new SqlDataAdapter(ChkCmd);
            //        ds = new DataSet();
            //        da.Fill(ds);
            //        //Close
            //    }
            //}
            //catch (Exception ex) { ErrorLog(ex.ToString()); }
        }    //Done Posting 23/07/2019 Code Perfect 
        private void WorkOrder_Post()
        {
            //string Pstr = "Work Order";
            //var ChkConStr = ClsComm.SqlConnectionString2();
            //SqlConnection Chkconn = new SqlConnection(ChkConStr);
            //var constr2 = ClsComm.SqlConnectionString();
            //var connsplit = constr2.Split('=', ';');
            //var connsplit2 = ChkConStr.Split('=', ';');
            //for (int count = 0; count <= connsplit.Length - 1; count++)
            //{
            //    if (count % 2 == 0)
            //    { }
            //    else
            //    {
            //        sqlserver2 = connsplit[1];
            //        initialcatalog2 = connsplit[3];
            //        Companycode2 = "030";
            //        Username2 = connsplit[5];
            //        password2 = connsplit[7];
            //    }
            //}
            //for (int count = 0; count <= connsplit2.Length - 1; count++)
            //{
            //    if (count % 2 == 0)
            //    { }
            //    else
            //    {
            //        sqlserver3 = connsplit2[1];
            //        initialcatalog3 = connsplit2[3];
            //        Companycode3 = "060";
            //        Username3 = connsplit2[5];
            //        password3 = connsplit2[7];
            //    }

            //}
            //var ft = new Transaction();
            //var sVNo = string.Empty;
            //blnIsEditing = false;
            //string sTmp = "";
            //if (blnIsEditing == false)
            //{
            //    sVNo = ft.GetNextVoucherNo("Jrn");
            //    ft.NewDocument("Jrn", sVNo);
            //}
            //else
            //{
            //    ft.DeleteDocument("Jrn", sVNo);
            //}

            //ft.SetField("Date", dtpToDate.Value.ToString(/*"yyyy/MM/dd"*/));
            //ft.SetField("VoucherType Name", Pstr);
            ////Select Data from SDC_WorkOrder based on the bill no and comparing 
            ////mySql = "SELECT [GLNm],[Credit],[Debit],[WorkOrdNo],[SupplierId],[PONo],[SupBillNo],[ItemId],[ItemType],[VATType],[ItemQty],[FreeQty],[ItemPrice],[Disc],[Net],[VAT],[StoreId],[GRNDate],[DueDate],[Source],[Status],[Remark],[PostingDateTime]" +
            ////   "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_WorkOrder fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [" + sqlserver2 + "].[Focus5030].dbo.SDC_WorkOrder fd)";
            //mySql = "SELECT * FROM[" + sqlserver3 + "].[" + initialcatalog3 + "].dbo.SDC_WorkOrder fc where '" + Pstr + "'  + '^' + fc.BillNo COLLATE DATABASE_DEFAULT not in" +
            //     "(select distinct Name + '^' + BillNoYH from[" + sqlserver2 + "].[" + initialcatalog2 + "].dbo.v_BillCheck) ";
            //ds = ClsSql.GetDs2(mySql);
            //dt = ds.Tables[0];
            //dgv.AllowUserToAddRows = false;
            //dgv.DataSource = dt;
            ////Close
            //try
            //{
            //    for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
            //    {

            //        //var cell = dgv.Rows[n].Cells["GLNm"];
            //        ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString());

            //        double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value);
            //        double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value);
            //        if (str != 0)
            //        {
            //            ft.SetField("Amount", str.ToString());
            //            ft.SetField("DrCr", "Cr");
            //        }
            //        else if (str1 != 0)
            //        {
            //            ft.SetField("Amount", str1.ToString());
            //            ft.SetField("DrCr", "Dr");
            //        }
            //        //var cell = dgv.Rows[n].Cells["WorkOrdNo"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["WorkOrdNo"].Value.ToString()) : "";
            //        //ft.SetField("WorkOrdNo", sTmp);

            //        var cell = dgv.Rows[n].Cells["SupplierId"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["SupplierId"].Value.ToString()) : "";
            //        ft.SetField("Supplier ID", sTmp);

            //        cell = dgv.Rows[n].Cells["PONo"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["PONo"].Value.ToString()) : "";
            //        ft.SetField("PO", sTmp);

            //        cell = dgv.Rows[n].Cells["SubBillNo"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["SubBillNo"].Value.ToString()) : "";
            //        ft.SetField("Sup Bill", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemId"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemId"].Value.ToString()) : "";
            //        ft.SetField("Item ID", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemType"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemType"].Value.ToString()) : "";
            //        ft.SetField("Item Type", sTmp);

            //        cell = dgv.Rows[n].Cells["VATType"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VATType"].Value.ToString()) : "";
            //        ft.SetField("VAT Type", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemQty"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemQty"].Value.ToString()) : "";
            //        ft.SetField("Item Q", sTmp);

            //        cell = dgv.Rows[n].Cells["FreeQty"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["FreeQty"].Value.ToString()) : "";
            //        ft.SetField("Free Q", sTmp);

            //        cell = dgv.Rows[n].Cells["ItemPrice"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemPrice"].Value.ToString()) : "";
            //        ft.SetField("Item Price", sTmp);

            //        cell = dgv.Rows[n].Cells["Disc"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Disc"].Value.ToString()) : "";
            //        ft.SetField("Disc", sTmp);

            //        cell = dgv.Rows[n].Cells["Net"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Net"].Value.ToString()) : "";
            //        ft.SetField("Net", sTmp);

            //        cell = dgv.Rows[n].Cells["VAT"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VAT"].Value.ToString()) : "";
            //        ft.SetField("VAT", sTmp);

            //        cell = dgv.Rows[n].Cells["StoreId"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["StoreId"].Value.ToString()) : "";
            //        ft.SetField("Store ID", sTmp);
            //        //JV
            //        //cell = dgv.Rows[n].Cells["GRNDate"];// Nul value handling
            //        //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["GRNDate"].Value.ToString()) : "";
            //        //ft.SetField("GRN Date", sTmp);

            //        cell = dgv.Rows[n].Cells["DueDate"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["DueDate"].Value.ToString()) : "";
            //        ft.SetField("Due Date", sTmp);

            //        cell = dgv.Rows[n].Cells["Source"];// Nul value handling
            //        sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString()) : "";
            //        ft.SetField("Source", sTmp);

            //        ft.AddRow();
            //    }
            //    ft.SetField("Approved", "1");
            //    int k = ft.SaveDocument();
            //    if (k != 1)
            //    {
            //        MessageBox.Show("Unable to post the Sales Invoice", "Focus", MessageBoxButtons.OK,
            //            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            //        //Checking if master is Available are Not                                  
            //        Chkconn = new SqlConnection(constr2);
            //        Chkconn.Open();
            //        mySql = "SELECT Distinct[GLNm] FROM [" + sqlserver3 + "].["+initialcatalog3+"].dbo.SDC_WorkOrder fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from [" + sqlserver2 + "].["+initialcatalog2+"].dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
            //        ds = ClsSql.GetDs2(mySql);
            //        var Err = new List<string>();
            //        if (dgv.Rows[0].Cells["GLNm"] == null)
            //        {
            //            ErrorLog("Already JV Posted ");
            //        }
            //        else
            //        {
            //            foreach (DataRow row in ds.Tables[0].Rows)
            //            {
            //                //ErrorLog("Account Masters are not Available in Focus:" + row["GLNm"].ToString());
            //                Err.Add(row["GLNm"].ToString());
            //            }
            //            string msg = string.Join(Environment.NewLine, Err);
            //            ErrorLog("Account Masters are not Available in Focus:Voucher Type:'Discount'\n" + msg);
            //            Chkconn.Close();
            //        }
            //        //End of Checking Masters
            //        //checking If credit and debit Notes arenot equal 
            //        mySql = "select SUM(credit) as Credit,SUM(debit) as Debit,BillNo from [" + sqlserver3 + "].[" + initialcatalog3 + "].dbo.SDC_WorkOrder" +
            //         "group by BillNo having SUM(credit) <> SUM(debit)";
            //        ds = ClsSql.GetDs2(mySql);
            //        foreach (DataRow row in ds.Tables[0].Rows)
            //        {
            //            ErrorLog("....Credit and Debit Notes are not Equal:....\n Credit Value:" + row["Credit"].ToString() + ",Debit Value:" + row["Debit"].ToString() + ",BillNo:" + row["BillNo"].ToString());
            //        }
            //        dgvNEqual.DataSource = ds.Tables[0];
            //        //Close
            //    }
            //    else
            //    {
            //        MessageBox.Show("Sales Invoice In Entry voucher Posted Successfully with document No. " + sVNo /*+ docno.Text*/, "Focus", MessageBoxButtons.OK,
            //        MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            //        //If Posting Success insert into Buffer DB 	SDC_WorkOrder	
            //        dgv.DataSource = ds.Tables[0];
            //        foreach (DataGridViewRow dr in dgv.Rows)
            //        {
            //            //string Chkstr = "INSERT INTO[" + sqlserver2 + "].[Focus5030].dbo.SDC_WorkOrder([GLNm],[Credit],[Debit],[WorkOrdNo],[SupplierId],[PONo],[SupBillNo],[ItemId],[ItemType],[VATType],[ItemQty],[FreeQty],[ItemPrice],[Disc],[Net],[VAT],[StoreId],[GRNDate],[DueDate],[Source],[Status],[Remark],[PostingDateTime])" +
            //            //                             "SELECT [GLNm],[Credit],[Debit],[WorkOrdNo],[SupplierId],[PONo],[SupBillNo],[ItemId],[ItemType],[VATType],[ItemQty],[FreeQty],[ItemPrice],[Disc],[Net],[VAT],[StoreId],[GRNDate],[DueDate],[Source],[Status],[Remark],[PostingDateTime]" +
            //            //                             "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_WorkOrder fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [Focus5030].dbo.SDC_WorkOrder fd)";
            //            string dtdate = Convert.ToDateTime(dr.Cells["GRNDate"].Value).ToString("yyyy-MM-dd HH:mm:ss");
            //            string RDate = Convert.ToDateTime(dr.Cells["DueDate"].Value).ToString("yyyy-MM-dd HH:mm:ss");
            //            mySql1 = mySql1 + "\n" + string.Format("INSERT INTO [" + sqlserver2 + "].[" + initialcatalog2 + "].dbo.SDC_WorkOrder([VoucherType],[GLNm],[Credit],[Debit],[WorkOrdNo],[SupplierId],[PONo],[SupBillNo],[ItemId],[ItemType],[VATType],[ItemQty],[FreeQty],[ItemPrice],[Disc],[Net],[VAT],[StoreId],[GRNDate],[DueDate],[Source],[Status],[Remark],[PostingDateTime])" +
            //                     "VALUES('" + Pstr + "','" + dr.Cells["GLnm"].Value + "'," + dr.Cells["Credit"].Value + "," + dr.Cells["Debit"].Value + "," + dr.Cells["WorkOrdNo"].Value + ",'" + dr.Cells["SupplierId"].Value + "','" + dr.Cells["PONo"].Value + "','" + dr.Cells["SupBillNo"].Value + "','" + dr.Cells["BillType"].Value + "'," + dr.Cells["ItemId"].Value + ",'" + dr.Cells["ItemType"].Value + "','" + dr.Cells["VATType"].Value + "'," + dr.Cells["ItemQty"].Value + "," + dr.Cells["FreeQty"].Value + ", " + dr.Cells["ItemPrice"].Value + ", " + dr.Cells["Disc"].Value + "," + dr.Cells["Net"].Value + ",'" + dr.Cells["VAT"].Value + "'," + dr.Cells["StoreId"].Value + ",'"+dtdate+"','"+RDate+"','" + dr.Cells["Source"].Value + "','" + dr.Cells["Status"].Value + "','" + dr.Cells["Remark"].Value + "','" + dr.Cells["PostingDateTime"].Value + "')");
            //        }
            //        ChkCmd = new SqlCommand(mySql1, Chkconn);
            //        da = new SqlDataAdapter(ChkCmd);
            //        ds = new DataSet();
            //        da.Fill(ds);
            //        //Close
            //    }
            //}
            //catch (Exception ex) { ErrorLog(ex.ToString()); }

        }            //Done Posting 23/07/2019 Code Perfect 

        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void cmbVoucher_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbVoucher_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void cmbVoucher_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            // GetData();
            //  this.btnPost.Enabled = true;
        }
    }
}
