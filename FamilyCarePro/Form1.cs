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
using System.Data;
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
        string initialcatalog = "";
        string Companycode = "";
        string Username = "";
        string password = "";
        DataSet ds, dsr;
        DataTable dt;
        SqlCommand ChkCmd;
        SqlDataAdapter da;
        string sAcc = "";
        string sTmp = "";
        string ChkConStr = "";

        private void Form1_Load(object sender, EventArgs e)
        {
            //GetData();
            dgvNEqual.Visible = false;

            //mySql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES";
            mySql = "select * from mr002 where Name<>''";
            DataSet ds = ClsSql.GetDs2(mySql);
            cmbVoucher.DataSource = ds.Tables[0];
            cmbVoucher.DisplayMember = "Name";

            cmbVoucher.Text = "";
            //this.btnPost.Enabled = false;
        }
        public void GetData()
        {
            //calculation
            double CrSum = 0, DrSum = 0;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                CrSum += Convert.ToDouble(dgv.Rows[i].Cells["Credit"].Value);
                DrSum += Convert.ToDouble(dgv.Rows[i].Cells["Debit"].Value);
            }
            txtCr.Text = CrSum.ToString();
            txtDr.Text = DrSum.ToString();
            txtCr.ReadOnly = true;
            txtDr.ReadOnly = true;
            //End of Calculation
        }

        public void NotEqual()
        {
            lblNot.Text = "Unable to Post Because Credit and Debit Values are not equal please check below....";
            mySql = "select SUM(credit) as Credit,SUM(debit) as Debit,BillNo from SDC_Revenue3 " +
                     "group by BillNo having SUM(credit) <> SUM(debit)";
            DataSet ds = ClsSql.GetDs(mySql);
            dgvNEqual.DataSource = ds.Tables[0];
            string TableName = ((DataTable)dgvNEqual.DataSource).TableName;
        }
        //Posting method posting into Focus
        private void btnGet_Click(object sender, EventArgs e)
        {
            PostInAllMethods();        
        }
        string path = @"D:\Navaneeth\PROJECTS\FamilyCarePro\FamilyCarePro\bin\Debug\ErrorLog.txt";
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
                objLogFile.WriteLine("Date and Time:" + DateTime.Now.ToString());
                objLogFile.WriteLine(_message);
                objLogFile.Close();
            }
            catch (Exception)
            {
                objLogFile.Close();
                objLogFile = null;
            }
        }
        string rtnMethod = "";
       
        private string PostInAllMethods()
        {
            if (cmbVoucher.Text=="Rev V")
            {                              
                Revenue_Post();
            }
            if (cmbVoucher.Text == "None")
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
            if (cmbVoucher.Text == "Patient Issue")
            {               
                Patient_Issue_Post();
            }
            if (cmbVoucher.Text == "Patient Return")
            {                
                Patient_Issue_rtn_Post();
            }
            if (cmbVoucher.Text == "Stores")
            {                
                Purchase_Post();
            }
            if (cmbVoucher.Text == "Store Purchase Return")
            {                
                Purchase_rtn_Post();
            }
            if (cmbVoucher.Text == "Refund")
            {                
                Refund_Post();
            }
            if (cmbVoucher.Text == "Canc Rev")
            {                
                Revenue_Canc_Post();
            }
            if (cmbVoucher.Text == "Store Adjustment")
            {
                Stock_adj_Post();
            }
            if (cmbVoucher.Text == "Store Consumption")
            {                
                Stock_Consum_Post();
            }
            if (cmbVoucher.Text == "Store Consumption Return")
            {
                Stock_Consum_rtn_Post();
            }
            if (cmbVoucher.Text == "Store Dispose")
            {                
                Stock_dispose_Post();
            }
            if (cmbVoucher.Text == "Store Issue")
            {                
                Stock_Issue_Post();
            }
            if (cmbVoucher.Text == "Store Issue Return")
            {                
                Stock_Issue_rtn_Post();
            }
            if (cmbVoucher.Text == "Work Order")
            {               
                WorkOrder_Post();
            }
            if (cmbVoucher.Text == "Voucher Type")
            {
                Revenue_Post();
                Collection_Post();
                CrNotes_Post();
                Discounts_Post();
                Patient_Issue_Post();
                Patient_Issue_rtn_Post();
                Purchase_Post();
                Purchase_rtn_Post();
                Refund_Post();
                Revenue_Canc_Post();
                Stock_adj_Post();
                Stock_Consum_Post();
                Stock_Consum_rtn_Post();
                Stock_dispose_Post();
                Stock_Issue_Post();
                Stock_Issue_rtn_Post();
                WorkOrder_Post();
            }
            return rtnMethod;
        }
        private void Revenue_Post()
        {
            string Pstr = "Rev V";
            var ChkConStr = ClsComm.SqlConnectionString();
            SqlConnection Chkconn = new SqlConnection(ChkConStr);
            var constr2 = ClsComm.SqlConnectionString2();
            var connsplit = constr2.Split('=', ';');
            var connsplit2 = ChkConStr.Split('=', ';');
            for (int count = 0; count <= connsplit.Length - 1; count++)
            {
                if (count % 2 == 0)
                {                }
                else
                {
                    sqlserver2 = connsplit[1];
                    initialcatalog = connsplit[3];
                    Companycode = "030";
                    Username = connsplit[5];
                    password = connsplit[7];
                }
            }
            for (int count = 0; count <= connsplit2.Length - 1; count++)
            {
                if (count % 2 == 0)
                {               }
                else
                {
                    sqlserver3 = connsplit2[1];
                    initialcatalog = connsplit2[3];
                    Companycode = "060";
                    Username = connsplit2[5];
                    password = connsplit2[7];
                }

            }
            var ft = new Transaction();
            var sVNo = string.Empty;
            blnIsEditing = false;
            string sTmp = "";
            if (blnIsEditing == false)
            {
                sVNo = ft.GetNextVoucherNo("Jrn");
                ft.NewDocument("Jrn", sVNo);
            }
            else
            {
                ft.DeleteDocument("Jrn", sVNo);
            }

            ft.SetField("Date", dtpToDate.Value.ToString(/*"yyyy/MM/dd"*/));
            ft.SetField("VoucherType Name", Pstr);

            //Select Data from SDC_Revenue based on the bill no and comparing
            mySql = "SELECT [GLNm],[Credit],[Debit],[PayeeId],[PayeeType],[BillNo],[BillDate],[BillType],[FileNo],[VisitId],[Nationality],[ServCode],[DeptId],[NetAmt],[CashierId],[DrId],[Source],[Status],[Remark],[PostingDateTime]" +
              "FROM [" + sqlserver2 + "].[FamilyCare Testing].dbo.SDC_Revenue fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [" + sqlserver2 + "].[Focus5030].dbo.SDC_Revenue fd)";
            DataSet ds = ClsSql.GetDs(mySql);
            DataTable dt = ds.Tables[0];
            dgv.AllowUserToAddRows = false;
            dgv.DataSource = dt;
            //Close
            GetData();
            try
            {
                for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
                {
                    sAcc = dgv.Rows[n].Cells["GLNm"].Value.ToString().Trim();
                    ft.SetField("Account Name", sAcc);

                    double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value.ToString().Trim());
                    double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value.ToString().Trim());

                    if (str != 0)
                    {
                        ft.SetField("Amount", str.ToString());
                        ft.SetField("DrCr", "Cr");
                    }
                    else if (str1 != 0)
                    {
                        ft.SetField("Amount", str1.ToString());
                        ft.SetField("DrCr", "Dr");
                    }
                    var cell = dgv.Rows[n].Cells["PayeeId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["PayeeId"].Value.ToString()) : "";
                    ft.SetField("Payee ID", sTmp);

                    cell = dgv.Rows[n].Cells["PayeeType"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["PayeeType"].Value.ToString()) : "";
                    ft.SetField("Payee Type", sTmp);

                    cell = dgv.Rows[n].Cells["BillNo"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["BillNo"].Value.ToString()) : "";
                    ft.SetField("Bill No", sTmp);

                    cell = dgv.Rows[n].Cells["BillDate"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["BillDate"].Value.ToString()) : "";
                    ft.SetField("Bill Date", sTmp);

                    cell = dgv.Rows[n].Cells["BillType"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["BillType"].Value.ToString()) : "";
                    ft.SetField("Bill Type", sTmp);

                    cell = dgv.Rows[n].Cells["FileNo"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["FileNo"].Value.ToString()) : "";
                    ft.SetField("File No", sTmp);

                    cell = dgv.Rows[n].Cells["VisitId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VisitId"].Value.ToString()) : "";
                    ft.SetField("Visit Id", sTmp);

                    cell = dgv.Rows[n].Cells["Nationality"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Nationality"].Value.ToString()) : "";
                    ft.SetField("Nationality", sTmp);

                    cell = dgv.Rows[n].Cells["ServCode"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ServCode"].Value.ToString()) : "";
                    ft.SetField("Serv Code", sTmp);

                    cell = dgv.Rows[n].Cells["DeptId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["DeptId"].Value.ToString()) : "";
                    ft.SetField("Dept Id", sTmp);

                    cell = dgv.Rows[n].Cells["NetAmt"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["NetAmt"].Value.ToString()) : "";
                    ft.SetField("Net Amt", sTmp);

                    cell = dgv.Rows[n].Cells["CashierId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["CashierId"].Value.ToString()) : "";
                    ft.SetField("Cashier Id", sTmp);

                    cell = dgv.Rows[n].Cells["DrId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["DrId"].Value.ToString()) : "";
                    ft.SetField("Dr ID", sTmp);

                    cell = dgv.Rows[n].Cells["Source"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString()) : "";
                    ft.SetField("Source", sTmp);
                    //ft.SetField("Status", dgv.Rows[n].Cells["Status"].Value.ToString());

                    ft.AddRow();
                }
                ft.SetField("Approved", "1");
                int k = ft.SaveDocument();
                if (k != 1)
                {

                    dgvNEqual.Visible = true;
                    NotEqual();
                    MessageBox.Show("Unable to post the Sales Invoice", "Focus", MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign); 
                    //Checking if master is Available are Not                                  
                    Chkconn = new SqlConnection(constr2);
                    Chkconn.Open();
                    mySql = "SELECT Distinct[GLNm] FROM [" + sqlserver2 + "].[FamilyCare Testing].dbo.SDC_Revenue fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from [" + sqlserver2 + "].[Focus5030].dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
                    ds = ClsSql.GetDs2(mySql);                  
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("Account Masters are not Available in Focus:"+row["GLNm"].ToString());                                                
                    }            
                    Chkconn.Close();
                    //End of Checking Masters
                    //checking If credit and debit Notes arenot equal 
                    mySql = "select SUM(credit) as Credit,SUM(debit) as Debit,BillNo from SDC_Revenue3" +
                     "group by BillNo having SUM(credit) <> SUM(debit)";
                    ds = ClsSql.GetDs(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("....Credit and Debit Notes are not Equal:....\n Credit Value:"+row["Credit"].ToString()+",Debit Value:"+row["Debit"].ToString()+",BillNo:"+row["BillNo"].ToString());
                    }
                    dgvNEqual.DataSource = ds.Tables[0];
                    //Close
                }
                else
                {
                    //Connection();
                    MessageBox.Show("Journal Entry Posted Successfully with document No. " + sVNo /*+ docno.Text*/, "Focus", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //If Posting Success insert into Buffer DB    
                     ChkConStr = ClsComm.SqlConnectionString();
                    Chkconn = new SqlConnection(ChkConStr);
                    string Chkstr = "INSERT INTO[" + sqlserver2 + "].[Focus5030].dbo.SDC_Revenue([GLNm],[Credit],[Debit],[PayeeId],[PayeeType],[BillNo],[BillDate],[BillType],[FileNo],[VisitId],[Nationality],[ServCode],[DeptId],[NetAmt],[CashierId],[DrId],[Source],[Status],[Remark],[PostingDateTime])" +
                             "SELECT [GLNm],[Credit],[Debit],[PayeeId],[PayeeType],[BillNo],[BillDate],[BillType],[FileNo],[VisitId],[Nationality],[ServCode],[DeptId],[NetAmt],[CashierId],[DrId],[Source],[Status],[Remark],[PostingDateTime]" +
                             "FROM [" + sqlserver2 + "].[FamilyCare Testing].dbo.SDC_Revenue fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [Focus5030].dbo.SDC_Revenue fd)";
                    ChkCmd = new SqlCommand(Chkstr, Chkconn);
                    da = new SqlDataAdapter(ChkCmd);
                    ds = new DataSet();
                    da.Fill(ds);
                    //Close                    
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex.ToString());

            }

        }
        private void Collection_Post()
        {
            string Pstr = "None";
            var ChkConStr = ClsComm.SqlConnectionString();
            SqlConnection Chkconn = new SqlConnection(ChkConStr);
            var constr2 = ClsComm.SqlConnectionString2();
            var connsplit = constr2.Split('=', ';');

            for (int count = 0; count <= connsplit.Length - 1; count++)
            {
                if (count % 2 == 0)
                {

                }
                else
                {
                    sqlserver2 = connsplit[1];
                    initialcatalog = connsplit[3];
                    Companycode = "030";
                    Username = connsplit[5];
                    password = connsplit[7];
                }
            }
            var ft = new Transaction();
            var sVNo = string.Empty;
            blnIsEditing = false;
            string sTmp = "";
            if (blnIsEditing == false)
            {
                sVNo = ft.GetNextVoucherNo("Jrn");
                ft.NewDocument("Jrn", sVNo);
            }
            else
            {
                ft.DeleteDocument("Jrn", sVNo);
            }

            ft.SetField("Date", dtpToDate.Value.ToString(/*"yyyy/MM/dd"*/));
            ft.SetField("VoucherType Name", Pstr);
            ////Select Data from SDC_Collection based on the bill no and comparing           
            mySql = "SELECT [GLNm],[Credit],[Debit],[PayeeId],[PayeeType],[BillNo],[BillDate],[BillType],[FileNo],[VisitId],[Nationality],[ReceiptNo],[NetAmt],[VATAmt],[DrId],[Source],[Status],[Remark],[PostingDateTime]" +
               "FROM [" + sqlserver2 + "].[FamilyCare Testing ].dbo.SDC_Collection fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [" + sqlserver2 + "].[Focus5030].dbo.SDC_Collection fd)";
            ds = ClsSql.GetDs(mySql);
            dt = ds.Tables[0];
            dgv.AllowUserToAddRows = false;
            dgv.DataSource = dt;
            //Close
            try
            {

                for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
                {

                    //var cell = dgv.Rows[n].Cells["GLNm"];
                    ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString().Trim());

                    double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value);
                    double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value);
                    if (str != 0)
                    {
                        ft.SetField("Amount", str.ToString());
                        ft.SetField("DrCr", "Cr");
                    }
                    else if (str1 != 0)
                    {
                        ft.SetField("Amount", str1.ToString());
                        ft.SetField("DrCr", "Dr");
                    }

                    var cell = dgv.Rows[n].Cells["PayeeId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["PayeeId"].Value.ToString()) : "";
                    ft.SetField("Payee ID", sTmp);

                    cell = dgv.Rows[n].Cells["PayeeType"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["PayeeType"].Value.ToString()) : "";
                    ft.SetField("Payee Type", sTmp);

                    cell = dgv.Rows[n].Cells["BillNo"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["BillNo"].Value.ToString()) : "";
                    ft.SetField("Bill No", sTmp);

                    cell = dgv.Rows[n].Cells["BillDate"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["BillDate"].Value.ToString()) : "";
                    ft.SetField("Bill Date", sTmp);

                    cell = dgv.Rows[n].Cells["BillType"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["BillType"].Value.ToString()) : "";
                    ft.SetField("Bill Type", sTmp);

                    cell = dgv.Rows[n].Cells["FileNo"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["FileNo"].Value.ToString()) : "";
                    ft.SetField("File No", sTmp);

                    cell = dgv.Rows[n].Cells["VisitId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VisitId"].Value.ToString()) : "";
                    ft.SetField("Visit ID", sTmp);

                    cell = dgv.Rows[n].Cells["Nationality"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Nationality"].Value.ToString()) : "";
                    ft.SetField("Nationality", sTmp);

                    cell = dgv.Rows[n].Cells["ReceiptNo"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ReceiptNo"].Value.ToString()) : "";
                    ft.SetField("Receipt No", sTmp);

                    cell = dgv.Rows[n].Cells["NetAmt"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["NetAmt"].Value.ToString()) : "";
                    ft.SetField("Net Amt", sTmp);

                    cell = dgv.Rows[n].Cells["VATAmt"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VATAmt"].Value.ToString()) : "";
                    ft.SetField("VAT Amt", sTmp);

                    cell = dgv.Rows[n].Cells["DrId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["DrId"].Value.ToString()) : "";
                    ft.SetField("Dr ID", sTmp);

                    cell = dgv.Rows[n].Cells["Source"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString()) : "";
                    ft.SetField("Source", sTmp);

                    ft.AddRow();
                }

                ft.SetField("Approved", "1");
                int k = ft.SaveDocument();
                if (k != 1)
                {
                    dgvNEqual.Visible = true;
                    NotEqual();
                    MessageBox.Show("Unable to post the Sales Invoice", "Focus", MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //Checking if master is Available are Not                                  
                    Chkconn = new SqlConnection(constr2);
                    Chkconn.Open();
                    mySql = "SELECT Distinct[GLNm] FROM [" + sqlserver2 + "].[FamilyCare Testing].dbo.SDC_Collection fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from [" + sqlserver2 + "].[Focus5030].dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
                    ds = ClsSql.GetDs2(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("Account Masters are not Available in Focus:" + row["GLNm"].ToString());
                    }
                    Chkconn.Close();
                    //End of Checking Masters
                    //checking If credit and debit Notes arenot equal 
                    mySql = "select SUM(credit) as Credit,SUM(debit) as Debit,BillNo from SDC_Collection " +
                     "group by BillNo having SUM(credit) <> SUM(debit)";
                    ds = ClsSql.GetDs(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("....Credit and Debit Notes are not Equal:....\n Credit Value:" + row["Credit"].ToString() + ",Debit Value:" + row["Debit"].ToString() + ",BillNo:" + row["BillNo"].ToString());
                    }
                    dgvNEqual.DataSource = ds.Tables[0];
                    //Close
                }
                else
                {
                    MessageBox.Show("Sales Invoice In Entry voucher Posted Successfully with document No. " + sVNo /*+ docno.Text*/, "Focus", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //If Posting Success insert into Buffer DB 	SDC_Collection				
                    string Chkstr = "INSERT INTO[" + sqlserver2 + "].[Focus5030].dbo.SDC_Collection([GLNm],[Credit],[Debit],[PayeeId],[PayeeType],[BillNo],[BillDate],[BillType],[FileNo],[VisitId],[Nationality],[ReceiptNo],[NetAmt],[VATAmt],[DrId],[Source],[Status],[Remark],[PostingDateTime])" +
                                                     "SELECT [GLNm],[Credit],[Debit],[PayeeId],[PayeeType],[BillNo],[BillDate],[BillType],[FileNo],[VisitId],[Nationality],[ReceiptNo],[NetAmt],[VATAmt],[DrId],[Source],[Status],[Remark],[PostingDateTime]" +
                                                     "FROM [" + sqlserver2 + "].[FamilyCare Testing ].dbo.SDC_Collection fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [Focus5030].dbo.SDC_Collection fd)";
                    ChkCmd = new SqlCommand(Chkstr, Chkconn);
                    da = new SqlDataAdapter(ChkCmd);
                    ds = new DataSet();
                    da.Fill(ds);
                    //Close	
                }
            }
            catch (Exception ex) { ErrorLog(ex.ToString()); }
        }
        private void CrNotes_Post()
        {
            string Pstr = "Cr. Notes";
            var ChkConStr = ClsComm.SqlConnectionString();
            SqlConnection Chkconn = new SqlConnection(ChkConStr);
            var constr2 = ClsComm.SqlConnectionString2();
            var connsplit = constr2.Split('=', ';');

            for (int count = 0; count <= connsplit.Length - 1; count++)
            {
                if (count % 2 == 0)
                {

                }
                else
                {
                    sqlserver2 = connsplit[1];
                    initialcatalog = connsplit[3];
                    Companycode = "030";
                    Username = connsplit[5];
                    password = connsplit[7];
                }
            }
            var ft = new Transaction();
            var sVNo = string.Empty;
            blnIsEditing = false;
            string sTmp = "";
            if (blnIsEditing == false)
            {
                sVNo = ft.GetNextVoucherNo("Jrn");
                ft.NewDocument("Jrn", sVNo);
            }
            else
            {
                ft.DeleteDocument("Jrn", sVNo);
            }

            ft.SetField("Date", dtpToDate.Value.ToString(/*"yyyy/MM/dd"*/));
            ft.SetField("VoucherType Name", Pstr);
            //Select Data from SDC_CrNotes based on the bill no and comparing
            mySql = "SELECT [GLNm],[Credit],[Debit],[PayeeId],[PayeeType],[BillNo],[BillDate],[BillType],[FileNo],[VisitId],[Nationality],[CrNoteId],[CrNoteDate],[NetAmt],[CashierId],[DrId],[Source],[Status],[Remark],[PostingDateTime]" +
               "FROM [" + sqlserver2 + "].[FamilyCare Testing].dbo.SDC_CrNotes fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [" + sqlserver2 + "].[Focus5030].dbo.SDC_CrNotes fd)";
            ds = ClsSql.GetDs(mySql);
            dt = ds.Tables[0];
            dgv.AllowUserToAddRows = false;
            dgv.DataSource = dt;
            //Close
            GetData();
            try
            {
                for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
                {

                    //var cell = dgv.Rows[n].Cells["GLNm"];
                    ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString().Trim());

                    double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value.ToString().Trim());
                    double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value.ToString().Trim());
                    if (str != 0)
                    {
                        ft.SetField("Amount", str.ToString());
                        ft.SetField("DrCr", "Cr");
                    }
                    else if (str1 != 0)
                    {
                        ft.SetField("Amount", str1.ToString());
                        ft.SetField("DrCr", "Dr");
                    }

                    var cell = dgv.Rows[n].Cells["PayeeId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["PayeeId"].Value.ToString()) : "";
                    ft.SetField("Payee ID", sTmp);

                    cell = dgv.Rows[n].Cells["PayeeType"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["PayeeType"].Value.ToString()) : "";
                    ft.SetField("Payee Type", sTmp);

                    cell = dgv.Rows[n].Cells["BillNo"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["BillNo"].Value.ToString()) : "";
                    ft.SetField("Bill No", sTmp);

                    cell = dgv.Rows[n].Cells["BillDate"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["BillDate"].Value.ToString()) : "";
                    ft.SetField("Bill Date", sTmp);

                    cell = dgv.Rows[n].Cells["BillType"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["BillType"].Value.ToString()) : "";
                    ft.SetField("Bill Type", sTmp);

                    cell = dgv.Rows[n].Cells["FileNo"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["FileNo"].Value.ToString()) : "";
                    ft.SetField("File No", sTmp);

                    cell = dgv.Rows[n].Cells["VisitId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VisitId"].Value.ToString()) : "";
                    ft.SetField("Visit ID", sTmp);

                    cell = dgv.Rows[n].Cells["Nationality"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Nationality"].Value.ToString()) : "";
                    ft.SetField("Nationality", sTmp);

                    cell = dgv.Rows[n].Cells["CrNoteId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["CrNoteId"].Value.ToString()) : "";
                    ft.SetField("Cr.Note ID", sTmp);

                    cell = dgv.Rows[n].Cells["CrNoteDate"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["CrNoteDate"].Value.ToString()) : "";
                    ft.SetField("Cr.Note Date", sTmp);

                    cell = dgv.Rows[n].Cells["NetAmt"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["NetAmt"].Value.ToString()) : "";
                    ft.SetField("Net Amt", sTmp);

                    cell = dgv.Rows[n].Cells["CashierId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["CashierId"].Value.ToString()) : "";
                    ft.SetField("Cashier ID", sTmp);

                    cell = dgv.Rows[n].Cells["DrId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["DrId"].Value.ToString()) : "";
                    ft.SetField("Dr ID", sTmp);

                    cell = dgv.Rows[n].Cells["Source"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString()) : "";
                    ft.SetField("Source", sTmp);

                    ft.AddRow();

                }

                ft.SetField("Approved", "1");
                int k = ft.SaveDocument();
                if (k != 1)
                {
                    dgvNEqual.Visible = true;
                    NotEqual();
                    MessageBox.Show("Unable to post the Sales Invoice", "Focus", MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //Checking if master is Available are Not                                  
                    Chkconn = new SqlConnection(constr2);
                    Chkconn.Open();
                    mySql = "SELECT Distinct[GLNm] FROM [" + sqlserver2 + "].[FamilyCare Testing].dbo.SDC_CrNotes fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from [" + sqlserver2 + "].[Focus5030].dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
                    ds = ClsSql.GetDs2(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("Account Masters are not Available in Focus:" + row["GLNm"].ToString());
                    }
                    Chkconn.Close();
                    //End of Checking Masters
                    //checking If credit and debit Notes arenot equal 
                    mySql = "select SUM(credit) as Credit,SUM(debit) as Debit,BillNo from SDC_CrNotes " +
                     "group by BillNo having SUM(credit) <> SUM(debit)";
                    ds = ClsSql.GetDs(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("....Credit and Debit Notes are not Equal:....\n Credit Value:" + row["Credit"].ToString() + ",Debit Value:" + row["Debit"].ToString() + ",BillNo:" + row["BillNo"].ToString());
                    }
                    dgvNEqual.DataSource = ds.Tables[0];
                    //Close
                }
                else
                {
                    MessageBox.Show("Sales Invoice In Entry voucher Posted Successfully with document No. " + sVNo /*+ docno.Text*/, "Focus", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //If Posting Success insert into Buffer DB 	SDC_CrNotes				
                    string Chkstr = "INSERT INTO[" + sqlserver2 + "].[Focus5030].dbo.SDC_CrNotes([GLNm],[Credit],[Debit],[PayeeId],[PayeeType],[BillNo],[BillDate],[BillType],[FileNo],[VisitId],[Nationality],[CrNoteId],[CrNoteDate],[NetAmt],[CashierId],[DrId],[Source],[Status],[Remark],[PostingDateTime])" +
                                                     "SELECT [GLNm],[Credit],[Debit],[PayeeId],[PayeeType],[BillNo],[BillDate],[BillType],[FileNo],[VisitId],[Nationality],[CrNoteId],[CrNoteDate],[NetAmt],[CashierId],[DrId],[Source],[Status],[Remark],[PostingDateTime]" +
                                                     "FROM [" + sqlserver2 + "].[FamilyCare Testing].dbo.SDC_CrNotes fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [" + sqlserver2 + "].[Focus5030].dbo.SDC_CrNotes fd)";
                    ChkCmd = new SqlCommand(Chkstr, Chkconn);
                    da = new SqlDataAdapter(ChkCmd);
                    ds = new DataSet();
                    da.Fill(ds);
                    //Close
                }
            }
            catch (Exception ex) { ErrorLog(ex.ToString()); }
        }
        private void Discounts_Post()
        {
            string Pstr = "Discount Permitted";          
            var ChkConStr = ClsComm.SqlConnectionString();
            SqlConnection Chkconn = new SqlConnection(ChkConStr);
            var constr2 = ClsComm.SqlConnectionString2();
            var connsplit = constr2.Split('=', ';');

            for (int count = 0; count <= connsplit.Length - 1; count++)
            {
                if (count % 2 == 0)
                {

                }
                else
                {
                    sqlserver2 = connsplit[1];
                    initialcatalog = connsplit[3];
                    Companycode = "030";
                    Username = connsplit[5];
                    password = connsplit[7];
                }
            }
            var ft = new Transaction();
            var sVNo = string.Empty;
            blnIsEditing = false;
            string sTmp = "";
            if (blnIsEditing == false)
            {
                sVNo = ft.GetNextVoucherNo("Jrn");
                ft.NewDocument("Jrn", sVNo);
            }
            else
            {
                ft.DeleteDocument("Jrn", sVNo);
            }

            ft.SetField("Date", dtpToDate.Value.ToString(/*"yyyy/MM/dd"*/));
            ft.SetField("VoucherType Name", Pstr);
            //Select Data from SDC_Discounts based on the bill no and comparing 
            mySql = "SELECT [GLNm],[Credit],[Debit],[PayeeId],[PayeeType],[BillNo],[BillDate],[BillType],[FileNo],[VisitId],[Nationality],[ServCode],[DeptId],[NetAmt],[CashierId],[DrId],[CancelDate],[CancelerId],[Source],[Status],[Remark],[PostingDateTime]" +
               "FROM [" + sqlserver2 + "].[FamilyCare Testing].dbo.SDC_Discounts fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [" + sqlserver2 + "].[Focus5030].dbo.SDC_Discounts fd)";
            ds = ClsSql.GetDs(mySql);
            dt = ds.Tables[0];
            dgv.AllowUserToAddRows = false;
            dgv.DataSource = dt;
            //Close
            try
            {
                for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
                {

                    //var cell = dgv.Rows[n].Cells["GLNm"];
                    ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString().Trim());

                    double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value);
                    double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value);
                    if (str != 0)
                    {
                        ft.SetField("Amount", str.ToString());
                        ft.SetField("DrCr", "Cr");
                    }
                    else if (str1 != 0)
                    {
                        ft.SetField("Amount", str1.ToString());
                        ft.SetField("DrCr", "Dr");
                    }
                    var cell = dgv.Rows[n].Cells["PayeeId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["PayeeId"].Value.ToString()) : "";
                    ft.SetField("Payee ID", sTmp);

                    cell = dgv.Rows[n].Cells["PayeeType"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["PayeeType"].Value.ToString()) : "";
                    ft.SetField("Payee Type", sTmp);

                    cell = dgv.Rows[n].Cells["BillNo"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["BillNo"].Value.ToString()) : "";
                    ft.SetField("Bill No", sTmp);

                    cell = dgv.Rows[n].Cells["BillDate"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["BillDate"].Value.ToString()) : "";
                    ft.SetField("Bill Date", sTmp);

                    cell = dgv.Rows[n].Cells["BillType"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["BillType"].Value.ToString()) : "";
                    ft.SetField("Bill Type", sTmp);

                    cell = dgv.Rows[n].Cells["FileNo"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["FileNo"].Value.ToString()) : "";
                    ft.SetField("File No", sTmp);

                    cell = dgv.Rows[n].Cells["VisitId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VisitId"].Value.ToString()) : "";
                    ft.SetField("Visit ID", sTmp);

                    cell = dgv.Rows[n].Cells["Nationality"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Nationality"].Value.ToString()) : "";
                    ft.SetField("Nationality", sTmp);

                    cell = dgv.Rows[n].Cells["ServCode"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ServCode"].Value.ToString()) : "";
                    ft.SetField("Serv Code", sTmp);

                    cell = dgv.Rows[n].Cells["DeptId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["DeptId"].Value.ToString()) : "";
                    ft.SetField("Dept ID", sTmp);

                    cell = dgv.Rows[n].Cells["NetAmt"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["NetAmt"].Value.ToString()) : "";
                    ft.SetField("Net Amt", sTmp);

                    cell = dgv.Rows[n].Cells["CashierId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["CashierId"].Value.ToString()) : "";
                    ft.SetField("Cashier ID", sTmp);

                    cell = dgv.Rows[n].Cells["DrId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["DrId"].Value.ToString()) : "";
                    ft.SetField("Dr ID", sTmp);

                    cell = dgv.Rows[n].Cells["CancelDate"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["CancelDate"].Value.ToString()) : "";
                    ft.SetField("Cancel Date", sTmp);

                    //cell = dgv.Rows[n].Cells["CancelerId"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["CancelerId"].Value.ToString()) : "";
                    //ft.SetField("Cancel ID", sTmp);

                    cell = dgv.Rows[n].Cells["Source"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString()) : "";
                    ft.SetField("Source", sTmp);

                    ft.AddRow();
                }
                MessageBox.Show("5");
                ft.SetField("Approved", "1");
                int k = ft.SaveDocument();
                if (k != 1)
                {
                    dgvNEqual.Visible = true;
                    NotEqual();
                    MessageBox.Show("Unable to post the Sales Invoice", "Focus", MessageBoxButtons.OK,
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //Checking if master is Available are Not                                  
                    Chkconn = new SqlConnection(constr2);
                    Chkconn.Open();
                    mySql = "SELECT Distinct[GLNm] FROM [" + sqlserver2 + "].[FamilyCare Testing].dbo.SDC_Discounts fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from [" + sqlserver2 + "].[Focus5030].dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
                    ds = ClsSql.GetDs2(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("Account Masters are not Available in Focus:" + row["GLNm"].ToString());
                    }
                    Chkconn.Close();
                    //End of Checking Masters
                    //checking If credit and debit Notes arenot equal 
                    mySql = "select SUM(credit) as Credit,SUM(debit) as Debit,BillNo from SDC_Discounts " +
                     "group by BillNo having SUM(credit) <> SUM(debit)";
                    ds = ClsSql.GetDs(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("....Credit and Debit Notes are not Equal:....\n Credit Value:" + row["Credit"].ToString() + ",Debit Value:" + row["Debit"].ToString() + ",BillNo:" + row["BillNo"].ToString());
                    }
                    dgvNEqual.DataSource = ds.Tables[0];
                    //Close
                }
                else
                {
                    MessageBox.Show("Sales Invoice In Entry voucher Posted Successfully with document No. " + sVNo /*+ docno.Text*/, "Focus", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //If Posting Success insert into Buffer DB 	SDC_Discounts				
                    string Chkstr = "INSERT INTO[" + sqlserver2 + "].[Focus5030].dbo.SDC_Discounts([GLNm],[Credit],[Debit],[PayeeId],[PayeeType],[BillNo],[BillDate],[BillType],[FileNo],[VisitId],[Nationality],[ServCode],[DeptId],[NetAmt],[CashierId],[DrId],[CancelDate],[CancelerId],[Source],[Status],[Remark],[PostingDateTime])" +
                                                     "SELECT [GLNm],[Credit],[Debit],[PayeeId],[PayeeType],[BillNo],[BillDate],[BillType],[FileNo],[VisitId],[Nationality],[ServCode],[DeptId],[NetAmt],[CashierId],[DrId],[CancelDate],[CancelerId],[Source],[Status],[Remark],[PostingDateTime]" +
                                                     "FROM [" + sqlserver2 + "].[FamilyCare Testing].dbo.SDC_Discounts fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [Focus5030].dbo.SDC_Discounts fd)";
                    ChkCmd = new SqlCommand(Chkstr, Chkconn);
                    da = new SqlDataAdapter(ChkCmd);
                    ds = new DataSet();
                    da.Fill(ds);
                    //Close
                }
            }
            catch (Exception ex) { ErrorLog(ex.ToString()); }
        }
        private void Patient_Issue_Post()
        {
            string Pstr = "Patient Issue";
            var ChkConStr = ClsComm.SqlConnectionString();
            SqlConnection Chkconn = new SqlConnection(ChkConStr);
            var constr2 = ClsComm.SqlConnectionString2();
            var connsplit = constr2.Split('=', ';');

            for (int count = 0; count <= connsplit.Length - 1; count++)
            {
                if (count % 2 == 0)
                {

                }
                else
                {
                    sqlserver2 = connsplit[1];
                    initialcatalog = connsplit[3];
                    Companycode = "030";
                    Username = connsplit[5];
                    password = connsplit[7];
                }
            }
            var ft = new Transaction();
            var sVNo = string.Empty;
            blnIsEditing = false;
            string sTmp = "";
            if (blnIsEditing == false)
            {
                sVNo = ft.GetNextVoucherNo("Jrn");
                ft.NewDocument("Jrn", sVNo);
            }
            else
            {
                ft.DeleteDocument("Jrn", sVNo);
            }

            ft.SetField("Date", dtpToDate.Value.ToString(/*"yyyy/MM/dd"*/));
            ft.SetField("VoucherType Name", Pstr);
            //Select Data from SDC_Patient_Issue based on the bill no and comparing 
            mySql = "SELECT [GLNm],[Credit],[Debit],[FileNo],[VisitNo],[VisitDate],[IssueNo],[IssueDate],[ItemId],[ItemQty],[ItemRate],[ItemTotal],[ItemType],[StoreId],[Source],[Status],[Remark],[PostingDateTime]" +
               "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Patient_Issue fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [" + sqlserver2 + "].[Focus5030].dbo.SDC_Patient_Issue fd)";
            ds = ClsSql.GetDs(mySql);
            dt = ds.Tables[0];
            dgv.AllowUserToAddRows = false;
            dgv.DataSource = dt;
            //Close
            try
            {
                for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
                {

                    //var cell = dgv.Rows[n].Cells["GLNm"];
                    ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString());

                    double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value);
                    double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value);
                    if (str != 0)
                    {
                        ft.SetField("Amount", str.ToString());
                        ft.SetField("DrCr", "Cr");
                    }
                    else if (str1 != 0)
                    {
                        ft.SetField("Amount", str1.ToString());
                        ft.SetField("DrCr", "Dr");
                    }

                    var cell = dgv.Rows[n].Cells["FileNo"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["FileNo"].Value.ToString()) : "";
                    ft.SetField("File No", sTmp);

                    ////Not confirmed this field visitNo is not their in JV
                    //cell = dgv.Rows[n].Cells["VisitNo"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VisitNo"].Value.ToString()) : "";
                    //ft.SetField("Visit No", sTmp);

                    //Not confirmed this field visitNo is not their in JV
                    //cell = dgv.Rows[n].Cells["VisitDate"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VisitDate"].Value.ToString()) : "";
                    //ft.SetField("Visit Date", sTmp);

                    cell = dgv.Rows[n].Cells["IssueNo"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["IssueNo"].Value.ToString()) : "";
                    ft.SetField("P Issue", sTmp);

                    cell = dgv.Rows[n].Cells["IssueDate"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["IssueDate"].Value.ToString()) : "";
                    ft.SetField("P Issue Date", sTmp);

                    cell = dgv.Rows[n].Cells["ItemId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemId"].Value.ToString()) : "";
                    ft.SetField("Item ID", sTmp);

                    cell = dgv.Rows[n].Cells["ItemQty"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemQty"].Value.ToString()) : "";
                    ft.SetField("Item Q", sTmp);

                    //Not confirmed this field visitNo is not their in JV
                    //cell = dgv.Rows[n].Cells["ItemRate"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemRate"].Value.ToString()) : "";
                    //ft.SetField("Item Rate", sTmp);

                    //Not confirmed this field visitNo is not their in JV
                    //cell = dgv.Rows[n].Cells["ItemTotal"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemTotal"].Value.ToString()) : "";
                    //ft.SetField("Item Total", sTmp);

                    cell = dgv.Rows[n].Cells["ItemType"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemType"].Value.ToString()) : "";
                    ft.SetField("Item Type", sTmp);

                    cell = dgv.Rows[n].Cells["StoreId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["StoreId"].Value.ToString()) : "";
                    ft.SetField("Store ID", sTmp);

                    cell = dgv.Rows[n].Cells["Source"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString()) : "";
                    ft.SetField("Source", sTmp);

                    ft.AddRow();
                }
                MessageBox.Show("5");
                ft.SetField("Approved", "1");
                int k = ft.SaveDocument();
                if (k != 1)
                {
                    dgvNEqual.Visible = true;
                    NotEqual();
                    MessageBox.Show("Unable to post the Sales Invoice", "Focus", MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //Checking if master is Available are Not                                  
                    Chkconn = new SqlConnection(constr2);
                    Chkconn.Open();
                    mySql = "SELECT Distinct[GLNm] FROM [" + sqlserver2 + "].[FamilyCare Testing].dbo.SDC_Patient_Issue fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from [" + sqlserver2 + "].[Focus5030].dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
                    ds = ClsSql.GetDs2(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("Account Masters are not Available in Focus:" + row["GLNm"].ToString());
                    }
                    Chkconn.Close();
                    //End of Checking Masters
                    //checking If credit and debit Notes arenot equal 
                    mySql = "select SUM(credit) as Credit,SUM(debit) as Debit,BillNo from SDC_Patient_Issue " +
                     "group by BillNo having SUM(credit) <> SUM(debit)";
                    ds = ClsSql.GetDs(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("....Credit and Debit Notes are not Equal:....\n Credit Value:" + row["Credit"].ToString() + ",Debit Value:" + row["Debit"].ToString() + ",BillNo:" + row["BillNo"].ToString());
                    }
                    dgvNEqual.DataSource = ds.Tables[0];
                    //Close
                }
                else
                {
                    MessageBox.Show("Sales Invoice In Entry voucher Posted Successfully with document No. " + sVNo /*+ docno.Text*/, "Focus", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //If Posting Success insert into Buffer DB 	SDC_Patient_Issue				
                    string Chkstr = "INSERT INTO[" + sqlserver2 + "].[Focus5030].dbo.SDC_Patient_Issue([GLNm],[Credit],[Debit],[FileNo],[VisitNo],[VisitDate],[IssueNo],[IssueDate],[ItemId],[ItemQty],[ItemRate],[ItemTotal],[ItemType],[StoreId],[Source],[Status],[Remark],[PostingDateTime])" +
                                                     "SELECT [GLNm],[Credit],[Debit],[FileNo],[VisitNo],[VisitDate],[IssueNo],[IssueDate],[ItemId],[ItemQty],[ItemRate],[ItemTotal],[ItemType],[StoreId],[Source],[Status],[Remark],[PostingDateTime]" +
                                                     "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Patient_Issue fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [Focus5030].dbo.SDC_Patient_Issue fd)";
                    ChkCmd = new SqlCommand(Chkstr, Chkconn);
                    da = new SqlDataAdapter(ChkCmd);
                    ds = new DataSet();
                    da.Fill(ds);
                    //Close
                }
            }
            catch (Exception ex) { ErrorLog(ex.ToString()); }
        }
        private void Patient_Issue_rtn_Post()
        {
            string Pstr = "Patient Return";
            var ChkConStr = ClsComm.SqlConnectionString();
            SqlConnection Chkconn = new SqlConnection(ChkConStr);
            var constr2 = ClsComm.SqlConnectionString2();
            var connsplit = constr2.Split('=', ';');

            for (int count = 0; count <= connsplit.Length - 1; count++)
            {
                if (count % 2 == 0)
                {

                }
                else
                {
                    sqlserver2 = connsplit[1];
                    initialcatalog = connsplit[3];
                    Companycode = "030";
                    Username = connsplit[5];
                    password = connsplit[7];
                }
            }
            var ft = new Transaction();
            var sVNo = string.Empty;
            blnIsEditing = false;
            string sTmp = "";
            if (blnIsEditing == false)
            {
                sVNo = ft.GetNextVoucherNo("Jrn");
                ft.NewDocument("Jrn", sVNo);
            }
            else
            {
                ft.DeleteDocument("Jrn", sVNo);
            }

            ft.SetField("Date", dtpToDate.Value.ToString(/*"yyyy/MM/dd"*/));
            ft.SetField("VoucherType Name", Pstr);
            //Select Data from SDC_Patient_Issue_Return based on the bill no and comparing
            mySql = "SELECT [GLNm],[Credit],[Debit],[FileNo],[VisitNo],[VisitDate],[ReturnNo],[ReturnDate],[ItemId],[ItemQty],[ItemRate],[ItemTotal],[ItemType],[StoreId],[Source],[Status],[Remark],[PostingDateTime]" +
               "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Patient_Issue_Return fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [" + sqlserver2 + "].[Focus5030].dbo.SDC_Patient_Issue_Return fd)";
            ds = ClsSql.GetDs(mySql);
            dt = ds.Tables[0];
            dgv.AllowUserToAddRows = false;
            dgv.DataSource = dt;
            //Close
            try
            {
                for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
                {

                    //var cell = dgv.Rows[n].Cells["GLNm"];
                    ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString());

                    double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value);
                    double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value);
                    if (str != 0)
                    {
                        ft.SetField("Amount", str.ToString());
                        ft.SetField("DrCr", "Cr");
                    }
                    else if (str1 != 0)
                    {
                        ft.SetField("Amount", str1.ToString());
                        ft.SetField("DrCr", "Dr");
                    }

                    var cell = dgv.Rows[n].Cells["FileNo"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["FileNo"].Value.ToString()) : "";
                    ft.SetField("File No", sTmp);

                    //Not confirmed this field visitNo is not their in JV
                    //cell = dgv.Rows[n].Cells["VisitNo"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VisitNo"].Value.ToString()) : "";
                    //ft.SetField("Visit No", sTmp);

                    //Not confirmed this field visitNo is not their in JV
                    //cell = dgv.Rows[n].Cells["VisitDate"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VisitDate"].Value.ToString()) : "";
                    //ft.SetField("Visit Date", sTmp);

                    //Not confirmed this field visitNo is not their in JV
                    //cell = dgv.Rows[n].Cells["ReturnNo"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ReturnNo"].Value.ToString()) : "";
                    //ft.SetField("Return No", sTmp);

                    //Not confirmed this field visitNo is not their in JV
                    //cell = dgv.Rows[n].Cells["ReturnDate"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ReturnDate"].Value.ToString()) : "";
                    //ft.SetField("Return Date", sTmp);

                    cell = dgv.Rows[n].Cells["ItemId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemId"].Value.ToString()) : "";
                    ft.SetField("Item ID", sTmp);

                    cell = dgv.Rows[n].Cells["ItemQty"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemQty"].Value.ToString()) : "";
                    ft.SetField("Item Q", sTmp);

                    //Not confirmed this field visitNo is not their in JV
                    //cell = dgv.Rows[n].Cells["ItemRate"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemRate"].Value.ToString()) : "";
                    //ft.SetField("Item Rate", sTmp);

                    //Not confirmed this field visitNo is not their in JV
                    //cell = dgv.Rows[n].Cells["ItemTotal"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemTotal"].Value.ToString()) : "";
                    //ft.SetField("Item Total", sTmp);

                    cell = dgv.Rows[n].Cells["ItemType"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemType"].Value.ToString()) : "";
                    ft.SetField("Item Type", sTmp);

                    cell = dgv.Rows[n].Cells["StoreId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["StoreId"].Value.ToString()) : "";
                    ft.SetField("Store ID", sTmp);

                    cell = dgv.Rows[n].Cells["Source"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString()) : "";
                    ft.SetField("Source", sTmp);

                    ft.AddRow();
                }
                MessageBox.Show("5");
                ft.SetField("Approved", "1");
                int k = ft.SaveDocument();
                if (k != 1)
                {
                    dgvNEqual.Visible = true;
                    NotEqual();
                    MessageBox.Show("Unable to post the Sales Invoice", "Focus", MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //Checking if master is Available are Not                                  
                    Chkconn = new SqlConnection(constr2);
                    Chkconn.Open();
                    mySql = "SELECT Distinct[GLNm] FROM [" + sqlserver2 + "].[FamilyCare Testing].dbo.SDC_Patient_Issue_Return fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from [" + sqlserver2 + "].[Focus5030].dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
                    ds = ClsSql.GetDs2(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("Account Masters are not Available in Focus:" + row["GLNm"].ToString());
                    }
                    Chkconn.Close();
                    //End of Checking Masters
                    //checking If credit and debit Notes arenot equal 
                    mySql = "select SUM(credit) as Credit,SUM(debit) as Debit,BillNo from SDC_Patient_Issue_Return" +
                     "group by BillNo having SUM(credit) <> SUM(debit)";
                    ds = ClsSql.GetDs(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("....Credit and Debit Notes are not Equal:....\n Credit Value:" + row["Credit"].ToString() + ",Debit Value:" + row["Debit"].ToString() + ",BillNo:" + row["BillNo"].ToString());
                    }
                    dgvNEqual.DataSource = ds.Tables[0];
                    //Close
                }
                else
                {
                    MessageBox.Show("Sales Invoice In Entry voucher Posted Successfully with document No. " + sVNo /*+ docno.Text*/, "Focus", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //If Posting Success insert into Buffer DB 	SDC_Patient_Issue_Return				
                    string Chkstr = "INSERT INTO[" + sqlserver2 + "].[Focus5030].dbo.SDC_Patient_Issue_Return([GLNm],[Credit],[Debit],[FileNo],[VisitNo],[VisitDate],[ReturnNo],[ReturnDate],[ItemId],[ItemQty],[ItemRate],[ItemTotal],[ItemType],[StoreId],[Source],[Status],[Remark],[PostingDateTime])" +
                                                     "SELECT [GLNm],[Credit],[Debit],[FileNo],[VisitNo],[VisitDate],[ReturnNo],[ReturnDate],[ItemId],[ItemQty],[ItemRate],[ItemTotal],[ItemType],[StoreId],[Source],[Status],[Remark],[PostingDateTime]" +
                                                     "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Patient_Issue_Return fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [Focus5030].dbo.SDC_Patient_Issue_Return fd)";
                    ChkCmd = new SqlCommand(Chkstr, Chkconn);
                    da = new SqlDataAdapter(ChkCmd);
                    ds = new DataSet();
                    da.Fill(ds);
                    //Close
                }
            }
            catch (Exception ex) { ErrorLog(ex.ToString()); }
        }
        private void Purchase_Post()
        {
            string Pstr = "Stores";
            var ChkConStr = ClsComm.SqlConnectionString();
            SqlConnection Chkconn = new SqlConnection(ChkConStr);
            var constr2 = ClsComm.SqlConnectionString2();
            var connsplit = constr2.Split('=', ';');

            for (int count = 0; count <= connsplit.Length - 1; count++)
            {
                if (count % 2 == 0)
                {

                }
                else
                {
                    sqlserver2 = connsplit[1];
                    initialcatalog = connsplit[3];
                    Companycode = "030";
                    Username = connsplit[5];
                    password = connsplit[7];
                }
            }
            var ft = new Transaction();
            var sVNo = string.Empty;
            blnIsEditing = false;
            string sTmp = "";
            if (blnIsEditing == false)
            {
                sVNo = ft.GetNextVoucherNo("Jrn");
                ft.NewDocument("Jrn", sVNo);
            }
            else
            {
                ft.DeleteDocument("Jrn", sVNo);
            }

            ft.SetField("Date", dtpToDate.Value.ToString(/*"yyyy/MM/dd"*/));
            ft.SetField("VoucherType Name", Pstr);
            //Select Data from SDC_Purchase based on the bill no and comparing 
            mySql = "SELECT [GLNm],[Credit],[Debit],[GRNNo],[SupplierId],[PONo],[SupBillNo],[ItemId],[ItemType],[VATType],[ItemQty],[FreeQty],[ItemPrice],[Disc],[Net],[VAT],[StoreId],[GRNDate],[DueDate],[Source],[Status],[Remark],[PostingDateTime]" +
               "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Purchase fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [" + sqlserver2 + "].[Focus5030].dbo.SDC_Purchase fd)";
            ds = ClsSql.GetDs(mySql);
            dt = ds.Tables[0];
            dgv.AllowUserToAddRows = false;
            dgv.DataSource = dt;
            //Close
            try
            {
                for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
                {

                    //var cell = dgv.Rows[n].Cells["GLNm"];
                    ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString());

                    double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value);
                    double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value);
                    if (str != 0)
                    {
                        ft.SetField("Amount", str.ToString());
                        ft.SetField("DrCr", "Cr");
                    }
                    else if (str1 != 0)
                    {
                        ft.SetField("Amount", str1.ToString());
                        ft.SetField("DrCr", "Dr");
                    }
                    var cell = dgv.Rows[n].Cells["GRNNo"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["GRNNo"].Value.ToString()) : "";
                    ft.SetField("GRN", sTmp);

                    cell = dgv.Rows[n].Cells["SupplierId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["SupplierId"].Value.ToString()) : "";
                    ft.SetField("Supplier ID", sTmp);

                    cell = dgv.Rows[n].Cells["PONo"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["PONo"].Value.ToString()) : "";
                    ft.SetField("PO", sTmp);

                    cell = dgv.Rows[n].Cells["SubBillNo"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["SubBillNo"].Value.ToString()) : "";
                    ft.SetField("Sup Bill", sTmp);

                    cell = dgv.Rows[n].Cells["ItemId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemId"].Value.ToString()) : "";
                    ft.SetField("Item ID", sTmp);

                    cell = dgv.Rows[n].Cells["ItemType"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemType"].Value.ToString()) : "";
                    ft.SetField("Item Type", sTmp);

                    cell = dgv.Rows[n].Cells["VATType"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VATType"].Value.ToString()) : "";
                    ft.SetField("VAT Type", sTmp);

                    cell = dgv.Rows[n].Cells["ItemQty"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemQty"].Value.ToString()) : "";
                    ft.SetField("Item Q", sTmp);

                    cell = dgv.Rows[n].Cells["FreeQty"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["FreeQty"].Value.ToString()) : "";
                    ft.SetField("Free Q", sTmp);

                    cell = dgv.Rows[n].Cells["ItemPrice"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemPrice"].Value.ToString()) : "";
                    ft.SetField("Item Price", sTmp);

                    cell = dgv.Rows[n].Cells["Disc"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Disc"].Value.ToString()) : "";
                    ft.SetField("Disc", sTmp);

                    cell = dgv.Rows[n].Cells["Net"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Net"].Value.ToString()) : "";
                    ft.SetField("Net", sTmp);

                    cell = dgv.Rows[n].Cells["VAT"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VAT"].Value.ToString()) : "";
                    ft.SetField("VAT", sTmp);

                    cell = dgv.Rows[n].Cells["StoreId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["StoreId"].Value.ToString()) : "";
                    ft.SetField("Store ID", sTmp);

                    //Not confirmed this field visitNo is not their in JV
                    //cell = dgv.Rows[n].Cells["GRNDate"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["GRNDate"].Value.ToString()) : "";
                    //ft.SetField("GRN Date", sTmp);

                    cell = dgv.Rows[n].Cells["DueDate"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["DueDate"].Value.ToString()) : "";
                    ft.SetField("Due Date", sTmp);

                    cell = dgv.Rows[n].Cells["Source"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString()) : "";
                    ft.SetField("Source", sTmp);

                    ft.AddRow();
                }
                MessageBox.Show("5");
                ft.SetField("Approved", "1");
                int k = ft.SaveDocument();
                if (k != 1)
                {
                    dgvNEqual.Visible = true;
                    NotEqual();
                    MessageBox.Show("Unable to post the Sales Invoice", "Focus", MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //Checking if master is Available are Not                                  
                    Chkconn = new SqlConnection(constr2);
                    Chkconn.Open();
                    mySql = "SELECT Distinct[GLNm] FROM [" + sqlserver2 + "].[FamilyCare Testing].dbo.SDC_Purchase fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from [" + sqlserver2 + "].[Focus5030].dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
                    ds = ClsSql.GetDs2(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("Account Masters are not Available in Focus:" + row["GLNm"].ToString());
                    }
                    Chkconn.Close();
                    //End of Checking Masters
                    //checking If credit and debit Notes arenot equal 
                    mySql = "select SUM(credit) as Credit,SUM(debit) as Debit,BillNo from SDC_Purchase" +
                     "group by BillNo having SUM(credit) <> SUM(debit)";
                    ds = ClsSql.GetDs(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("....Credit and Debit Notes are not Equal:....\n Credit Value:" + row["Credit"].ToString() + ",Debit Value:" + row["Debit"].ToString() + ",BillNo:" + row["BillNo"].ToString());
                    }
                    dgvNEqual.DataSource = ds.Tables[0];
                    //Close
                }
                else
                {
                    MessageBox.Show("Sales Invoice In Entry voucher Posted Successfully with document No. " + sVNo /*+ docno.Text*/, "Focus", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //If Posting Success insert into Buffer DB 	SDC_Purchase				
                    string Chkstr = "INSERT INTO[" + sqlserver2 + "].[Focus5030].dbo.SDC_Purchase([GLNm],[Credit],[Debit],[GRNNo],[SupplierId],[PONo],[SupBillNo],[ItemId],[ItemType],[VATType],[ItemQty],[FreeQty],[ItemPrice],[Disc],[Net],[VAT],[StoreId],[GRNDate],[DueDate],[Source],[Status],[Remark],[PostingDateTime])" +
                                                     "SELECT [GLNm],[Credit],[Debit],[GRNNo],[SupplierId],[PONo],[SupBillNo],[ItemId],[ItemType],[VATType],[ItemQty],[FreeQty],[ItemPrice],[Disc],[Net],[VAT],[StoreId],[GRNDate],[DueDate],[Source],[Status],[Remark],[PostingDateTime]" +
                                                     "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Purchase fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [Focus5030].dbo.SDC_Purchase fd)";
                    ChkCmd = new SqlCommand(Chkstr, Chkconn);
                    da = new SqlDataAdapter(ChkCmd);
                    ds = new DataSet();
                    da.Fill(ds);
                    //Close
                }
            }
            catch (Exception ex) { ErrorLog(ex.ToString()); }
        }
        private void Purchase_rtn_Post()
        {
            string Pstr = "Store Purchase Return";
            var ChkConStr = ClsComm.SqlConnectionString();
            SqlConnection Chkconn = new SqlConnection(ChkConStr);
            var constr2 = ClsComm.SqlConnectionString2();
            var connsplit = constr2.Split('=', ';');

            for (int count = 0; count <= connsplit.Length - 1; count++)
            {
                if (count % 2 == 0)
                {

                }
                else
                {
                    sqlserver2 = connsplit[1];
                    initialcatalog = connsplit[3];
                    Companycode = "030";
                    Username = connsplit[5];
                    password = connsplit[7];
                }
            }
            var ft = new Transaction();
            var sVNo = string.Empty;
            blnIsEditing = false;
            string sTmp = "";
            if (blnIsEditing == false)
            {
                sVNo = ft.GetNextVoucherNo("Jrn");
                ft.NewDocument("Jrn", sVNo);
            }
            else
            {
                ft.DeleteDocument("Jrn", sVNo);
            }

            ft.SetField("Date", dtpToDate.Value.ToString(/*"yyyy/MM/dd"*/));
            ft.SetField("VoucherType Name", Pstr);
            //Select Data from SDC_Purchase_Return based on the bill no and comparing 
            mySql = "SELECT [GLNm],[Credit],[Debit],[GRNNo],[SupplierId],[ReturnNo],[ReturnDate],[ItemId],[ItemType],[VATType],[ItemQty],[FreeQty],[ItemPrice],[Disc],[Net],[VAT],[StoreId],[Source],[Status],[Remark],[PostingDateTime]" +
               "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Purchase_Return fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [" + sqlserver2 + "].[Focus5030].dbo.SDC_Purchase_Return fd)";
            ds = ClsSql.GetDs(mySql);
            dt = ds.Tables[0];
            dgv.AllowUserToAddRows = false;
            dgv.DataSource = dt;
            //Close
            try
            {
                for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
                {

                    //var cell = dgv.Rows[n].Cells["GLNm"];
                    ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString());

                    double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value);
                    double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value);
                    if (str != 0)
                    {
                        ft.SetField("Amount", str.ToString());
                        ft.SetField("DrCr", "Cr");
                    }
                    else if (str1 != 0)
                    {
                        ft.SetField("Amount", str1.ToString());
                        ft.SetField("DrCr", "Dr");
                    }
                    var cell = dgv.Rows[n].Cells["GRNNo"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["GRNNo"].Value.ToString()) : "";
                    ft.SetField("GRN", sTmp);

                    cell = dgv.Rows[n].Cells["SupplierId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["SupplierId"].Value.ToString()) : "";
                    ft.SetField("Supplier ID", sTmp);

                    //Not confirmed this field visitNo is not their in JV
                    //cell = dgv.Rows[n].Cells["ReturnNo"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ReturnNo"].Value.ToString()) : "";
                    //ft.SetField("Return No", sTmp);

                    //Not confirmed this field visitNo is not their in JV
                    //cell = dgv.Rows[n].Cells["ReturnDate"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ReturnDate"].Value.ToString()) : "";
                    //ft.SetField("ReturnDate", sTmp);

                    cell = dgv.Rows[n].Cells["ItemId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemId"].Value.ToString()) : "";
                    ft.SetField("Item ID", sTmp);

                    cell = dgv.Rows[n].Cells["ItemType"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemType"].Value.ToString()) : "";
                    ft.SetField("Item Type", sTmp);

                    cell = dgv.Rows[n].Cells["VATType"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VATType"].Value.ToString()) : "";
                    ft.SetField("VAT Type", sTmp);

                    cell = dgv.Rows[n].Cells["ItemQty"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemQty"].Value.ToString()) : "";
                    ft.SetField("Item Q", sTmp);

                    cell = dgv.Rows[n].Cells["FreeQty"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["FreeQty"].Value.ToString()) : "";
                    ft.SetField("Free Q", sTmp);

                    cell = dgv.Rows[n].Cells["ItemPrice"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemPrice"].Value.ToString()) : "";
                    ft.SetField("Item Price", sTmp);

                    cell = dgv.Rows[n].Cells["Disc"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Disc"].Value.ToString()) : "";
                    ft.SetField("Disc", sTmp);

                    cell = dgv.Rows[n].Cells["Net"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Net"].Value.ToString()) : "";
                    ft.SetField("Net", sTmp);

                    cell = dgv.Rows[n].Cells["VAT"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VAT"].Value.ToString()) : "";
                    ft.SetField("VAT", sTmp);

                    cell = dgv.Rows[n].Cells["StoreId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["StoreId"].Value.ToString()) : "";
                    ft.SetField("Store ID", sTmp);

                    cell = dgv.Rows[n].Cells["Source"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString()) : "";
                    ft.SetField("Source", sTmp);

                    ft.AddRow();
                }
                MessageBox.Show("5");
                ft.SetField("Approved", "1");
                int k = ft.SaveDocument();
                if (k != 1)
                {
                    dgvNEqual.Visible = true;
                    NotEqual();
                    MessageBox.Show("Unable to post the Sales Invoice", "Focus", MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //Checking if master is Available are Not                                  
                    Chkconn = new SqlConnection(constr2);
                    Chkconn.Open();
                    mySql = "SELECT Distinct[GLNm] FROM [" + sqlserver2 + "].[FamilyCare Testing].dbo.SDC_Purchase_Return fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from [" + sqlserver2 + "].[Focus5030].dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
                    ds = ClsSql.GetDs2(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("Account Masters are not Available in Focus:" + row["GLNm"].ToString());
                    }
                    Chkconn.Close();
                    //End of Checking Masters
                    //checking If credit and debit Notes arenot equal 
                    mySql = "select SUM(credit) as Credit,SUM(debit) as Debit,BillNo from SDC_Purchase_Return" +
                     "group by BillNo having SUM(credit) <> SUM(debit)";
                    ds = ClsSql.GetDs(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("....Credit and Debit Notes are not Equal:....\n Credit Value:" + row["Credit"].ToString() + ",Debit Value:" + row["Debit"].ToString() + ",BillNo:" + row["BillNo"].ToString());
                    }
                    dgvNEqual.DataSource = ds.Tables[0];
                    //Close
                }
                else
                {
                    MessageBox.Show("Sales Invoice In Entry voucher Posted Successfully with document No. " + sVNo /*+ docno.Text*/, "Focus", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //If Posting Success insert into Buffer DB 	SDC_Purchase_Return				
                    string Chkstr = "INSERT INTO[" + sqlserver2 + "].[Focus5030].dbo.SDC_Purchase_Return([GLNm],[Credit],[Debit],[GRNNo],[SupplierId],[ReturnNo],[ReturnDate],[ItemId],[ItemType],[VATType],[ItemQty],[FreeQty],[ItemPrice],[Disc],[Net],[VAT],[StoreId],[Source],[Status],[Remark],[PostingDateTime])" +
                                                     "SELECT [GLNm],[Credit],[Debit],[GRNNo],[SupplierId],[ReturnNo],[ReturnDate],[ItemId],[ItemType],[VATType],[ItemQty],[FreeQty],[ItemPrice],[Disc],[Net],[VAT],[StoreId],[Source],[Status],[Remark],[PostingDateTime]" +
                                                     "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Purchase_Return fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [Focus5030].dbo.SDC_Purchase_Return fd)";
                    ChkCmd = new SqlCommand(Chkstr, Chkconn);
                    da = new SqlDataAdapter(ChkCmd);
                    ds = new DataSet();
                    da.Fill(ds);
                    //Close	
                }
            }
            catch (Exception ex) { ErrorLog(ex.ToString()); }
        }
        private void Refund_Post()
        {
            string Pstr = "Refund";
            var ChkConStr = ClsComm.SqlConnectionString();
            SqlConnection Chkconn = new SqlConnection(ChkConStr);
            var constr2 = ClsComm.SqlConnectionString2();
            var connsplit = constr2.Split('=', ';');

            for (int count = 0; count <= connsplit.Length - 1; count++)
            {
                if (count % 2 == 0)
                {

                }
                else
                {
                    sqlserver2 = connsplit[1];
                    initialcatalog = connsplit[3];
                    Companycode = "030";
                    Username = connsplit[5];
                    password = connsplit[7];
                }
            }
            var ft = new Transaction();
            var sVNo = string.Empty;
            blnIsEditing = false;
            string sTmp = "";
            if (blnIsEditing == false)
            {
                sVNo = ft.GetNextVoucherNo("Jrn");
                ft.NewDocument("Jrn", sVNo);
            }
            else
            {
                ft.DeleteDocument("Jrn", sVNo);
            }

            ft.SetField("Date", dtpToDate.Value.ToString(/*"yyyy/MM/dd"*/));
            ft.SetField("VoucherType Name", Pstr);
            //Select Data from SDC_Refund based on the bill no and comparing 
            mySql = "SELECT [GLNm],[Credit],[Debit],[PayeeId],[PayeeType],[CrNoteNo],[CrNoteDate],[RefundDate],[RefundNo],[FileNo],[VisitId],[Nationality],[CrNoteId],[VatAmt],[CashierId],[BillNo],[BillDate],[BillType],[Source],[Status],[Remark],[PostingDateTime]" +
               "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Refund fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [" + sqlserver2 + "].[Focus5030].dbo.SDC_Refund fd)";
            ds = ClsSql.GetDs(mySql);
            dt = ds.Tables[0];
            dgv.AllowUserToAddRows = false;
            dgv.DataSource = dt;
            //Close
            try
            {
                for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
                {

                    //var cell = dgv.Rows[n].Cells["GLNm"];
                    ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString());

                    double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value);
                    double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value);
                    if (str != 0)
                    {
                        ft.SetField("Amount", str.ToString());
                        ft.SetField("DrCr", "Cr");
                    }
                    else if (str1 != 0)
                    {
                        ft.SetField("Amount", str1.ToString());
                        ft.SetField("DrCr", "Dr");
                    }

                    var cell = dgv.Rows[n].Cells["PayeeId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["PayeeId"].Value.ToString()) : "";
                    ft.SetField("Payee ID", sTmp);

                    cell = dgv.Rows[n].Cells["PayeeType"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["PayeeType"].Value.ToString()) : "";
                    ft.SetField("Payee Type", sTmp);

                    //Not confirmed this field visitNo is not their in JV
                    //cell = dgv.Rows[n].Cells["CrNoteNo"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["CrNoteNo"].Value.ToString()) : "";
                    //ft.SetField("Cr Note No", sTmp);

                    cell = dgv.Rows[n].Cells["CrNoteDate"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["CrNoteDate"].Value.ToString()) : "";
                    ft.SetField("Cr Note Date", sTmp);

                    cell = dgv.Rows[n].Cells["RefundDate"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["RefundDate"].Value.ToString()) : "";
                    ft.SetField("Refund Date", sTmp);

                    cell = dgv.Rows[n].Cells["RefundNo"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["RefundNo"].Value.ToString()) : "";
                    ft.SetField("Refund No", sTmp);

                    cell = dgv.Rows[n].Cells["FileNo"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["FileNo"].Value.ToString()) : "";
                    ft.SetField("File No", sTmp);

                    cell = dgv.Rows[n].Cells["VisitId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VisitId"].Value.ToString()) : "";
                    ft.SetField("Visit ID", sTmp);

                    cell = dgv.Rows[n].Cells["Nationality"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Nationality"].Value.ToString()) : "";
                    ft.SetField("Nationality", sTmp);

                    cell = dgv.Rows[n].Cells["CrNoteId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["CrNoteId"].Value.ToString()) : "";
                    ft.SetField("Cr Note ID", sTmp);

                    cell = dgv.Rows[n].Cells["VatAmt"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VatAmt"].Value.ToString()) : "";
                    ft.SetField("Vat Amt", sTmp);

                    cell = dgv.Rows[n].Cells["CashierId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["CashierId"].Value.ToString()) : "";
                    ft.SetField("Cashier ID", sTmp);

                    cell = dgv.Rows[n].Cells["Source"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString()) : "";
                    ft.SetField("Source", sTmp);

                    ft.AddRow();
                }
                MessageBox.Show("5");
                ft.SetField("Approved", "1");
                int k = ft.SaveDocument();
                if (k != 1)
                {
                    dgvNEqual.Visible = true;
                    NotEqual();
                    MessageBox.Show("Unable to post the Sales Invoice", "Focus", MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //Checking if master is Available are Not                                  
                    Chkconn = new SqlConnection(constr2);
                    Chkconn.Open();
                    mySql = "SELECT Distinct[GLNm] FROM [" + sqlserver2 + "].[FamilyCare Testing].dbo.SDC_Refund fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from [" + sqlserver2 + "].[Focus5030].dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
                    ds = ClsSql.GetDs2(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("Account Masters are not Available in Focus:" + row["GLNm"].ToString());
                    }
                    Chkconn.Close();
                    //End of Checking Masters
                    //checking If credit and debit Notes arenot equal 
                    mySql = "select SUM(credit) as Credit,SUM(debit) as Debit,BillNo from SDC_Refund" +
                     "group by BillNo having SUM(credit) <> SUM(debit)";
                    ds = ClsSql.GetDs(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("....Credit and Debit Notes are not Equal:....\n Credit Value:" + row["Credit"].ToString() + ",Debit Value:" + row["Debit"].ToString() + ",BillNo:" + row["BillNo"].ToString());
                    }
                    dgvNEqual.DataSource = ds.Tables[0];
                    //Close
                }
                else
                {
                    MessageBox.Show("Sales Invoice In Entry voucher Posted Successfully with document No. " + sVNo /*+ docno.Text*/, "Focus", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //If Posting Success insert into Buffer DB 	SDC_Refund				
                    string Chkstr = "INSERT INTO[" + sqlserver2 + "].[Focus5030].dbo.SDC_Refund([GLNm],[Credit],[Debit],[PayeeId],[PayeeType],[CrNoteNo],[CrNoteDate],[RefundDate],[RefundNo],[FileNo],[VisitId],[Nationality],[CrNoteId],[VatAmt],[CashierId],[BillNo],[BillDate],[BillType],[Source],[Status],[Remark],[PostingDateTime])" +
                                                     "SELECT [GLNm],[Credit],[Debit],[PayeeId],[PayeeType],[CrNoteNo],[CrNoteDate],[RefundDate],[RefundNo],[FileNo],[VisitId],[Nationality],[CrNoteId],[VatAmt],[CashierId],[BillNo],[BillDate],[BillType],[Source],[Status],[Remark],[PostingDateTime]" +
                                                     "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Refund fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [Focus5030].dbo.SDC_Refund fd)";
                    ChkCmd = new SqlCommand(Chkstr, Chkconn);
                    da = new SqlDataAdapter(ChkCmd);
                    ds = new DataSet();
                    da.Fill(ds);
                    //Close	
                }
            }
            catch (Exception ex) { ErrorLog(ex.ToString()); }
        }
        private void Revenue_Canc_Post()
        {
            string Pstr = "Canc Rev";
            var ChkConStr = ClsComm.SqlConnectionString();
            SqlConnection Chkconn = new SqlConnection(ChkConStr);
            var constr2 = ClsComm.SqlConnectionString2();
            var connsplit = constr2.Split('=', ';');

            for (int count = 0; count <= connsplit.Length - 1; count++)
            {
                if (count % 2 == 0)
                {

                }
                else
                {
                    sqlserver2 = connsplit[1];
                    initialcatalog = connsplit[3];
                    Companycode = "030";
                    Username = connsplit[5];
                    password = connsplit[7];
                }
            }
            var ft = new Transaction();
            var sVNo = string.Empty;
            blnIsEditing = false;
            string sTmp = "";
            if (blnIsEditing == false)
            {
                sVNo = ft.GetNextVoucherNo("Jrn");
                ft.NewDocument("Jrn", sVNo);
            }
            else
            {
                ft.DeleteDocument("Jrn", sVNo);
            }

            ft.SetField("Date", dtpToDate.Value.ToString(/*"yyyy/MM/dd"*/));
            ft.SetField("VoucherType Name", Pstr);
            //Select Data from SDC_Revenue_Canc based on the bill no and comparing 
            mySql = "SELECT [GLNm],[Credit],[Debit],[PayeeId],[PayeeType],[BillNo],[BillDate],[BillType],[FileNo],[VisitId],[Nationality],[ServCode],[DeptId],[NetAmt],[CashierId],[DrId],[CancelDate],[CashierId],[Source],[Status],[Remark],[PostingDateTime]" +
               "FROM [" + sqlserver2 + "].[FamilyCare Testing].dbo.SDC_Revenue_Canc fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [" + sqlserver2 + "].[Focus5030].dbo.SDC_Revenue_Canc fd)";
            ds = ClsSql.GetDs(mySql);
            dt = ds.Tables[0];
            dgv.AllowUserToAddRows = false;
            dgv.DataSource = dt;
            //Close
            try
            {
                for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
                {
                    //var cell = dgv.Rows[n].Cells["GLNm"];
                    ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString());

                    //int str = Convert.ToInt32(dgv.Rows[n].Cells["Credit"].Value);
                    //int str1 = Convert.ToInt32(dgv.Rows[n].Cells["Debit"].Value);
                    double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value);
                    double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value);
                    if (str != 0)
                    {
                        ft.SetField("Amount", str.ToString());
                        ft.SetField("DrCr", "Cr");
                    }
                    else if (str1 != 0)
                    {
                        ft.SetField("Amount", str1.ToString());
                        ft.SetField("DrCr", "Dr");
                    }

                    var cell = dgv.Rows[n].Cells["PayeeId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["PayeeId"].Value.ToString()) : "";
                    ft.SetField("Payee ID", sTmp);

                    cell = dgv.Rows[n].Cells["PayeeType"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["PayeeType"].Value.ToString()) : "";
                    ft.SetField("Payee Type", sTmp);

                    cell = dgv.Rows[n].Cells["BillNo"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["BillNo"].Value.ToString()) : "";
                    ft.SetField("Bill No", sTmp);

                    cell = dgv.Rows[n].Cells["BillDate"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["BillDate"].Value.ToString()) : "";
                    ft.SetField("Bill Date", sTmp);

                    cell = dgv.Rows[n].Cells["BillType"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["BillType"].Value.ToString()) : "";
                    ft.SetField("Bill Type", sTmp);

                    cell = dgv.Rows[n].Cells["FileNo"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["FileNo"].Value.ToString()) : "";
                    ft.SetField("File No", sTmp);

                    cell = dgv.Rows[n].Cells["VisitId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VisitId"].Value.ToString()) : "";
                    ft.SetField("Visit ID", sTmp);

                    cell = dgv.Rows[n].Cells["Nationality"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Nationality"].Value.ToString()) : "";
                    ft.SetField("Nationality", sTmp);

                    cell = dgv.Rows[n].Cells["ServCode"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ServCode"].Value.ToString()) : "";
                    ft.SetField("Serv Code", sTmp);

                    cell = dgv.Rows[n].Cells["DeptId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["DeptId"].Value.ToString()) : "";
                    ft.SetField("Dept ID", sTmp);

                    cell = dgv.Rows[n].Cells["NetAmt"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["NetAmt"].Value.ToString()) : "";
                    ft.SetField("Net Amt", sTmp);

                    cell = dgv.Rows[n].Cells["CashierId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["CashierId"].Value.ToString()) : "";
                    ft.SetField("Cashier ID", sTmp);

                    cell = dgv.Rows[n].Cells["DrId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["DrId"].Value.ToString()) : "";
                    ft.SetField("Dr ID", sTmp);

                    cell = dgv.Rows[n].Cells["CancelDate"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["CancelDate"].Value.ToString()) : "";
                    ft.SetField("Cancel Date", sTmp);

                    //cell = dgv.Rows[n].Cells["CancelerId"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["CancelerId"].Value.ToString()) : "";
                    //ft.SetField("Cancel ID", sTmp);

                    cell = dgv.Rows[n].Cells["Source"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString()) : "";
                    ft.SetField("Source", sTmp);
                    //ft.SetField("Status", dgv.Rows[n].Cells["Status"].Value.ToString());

                    ft.AddRow();
                }
                MessageBox.Show("5");
                ft.SetField("Approved", "1");
                int k = ft.SaveDocument();
                if (k != 1)
                {
                
                    dgvNEqual.Visible = true;
                    NotEqual();
                    MessageBox.Show("Unable to post the Sales Invoice", "Focus", MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //Checking if master is Available are Not                                  
                    Chkconn = new SqlConnection(constr2);
                    Chkconn.Open();
                    mySql = "SELECT Distinct[GLNm] FROM [" + sqlserver2 + "].[FamilyCare Testing].dbo.SDC_Revenue_Canc fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from [" + sqlserver2 + "].[Focus5030].dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
                    ds = ClsSql.GetDs2(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("Account Masters are not Available in Focus:" + row["GLNm"].ToString());
                    }
                    Chkconn.Close();
                    //End of Checking Masters
                    //checking If credit and debit Notes arenot equal 
                    mySql = "select SUM(credit) as Credit,SUM(debit) as Debit,BillNo from SDC_Revenue_Canc" +
                     "group by BillNo having SUM(credit) <> SUM(debit)";
                    ds = ClsSql.GetDs(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("....Credit and Debit Notes are not Equal:....\n Credit Value:" + row["Credit"].ToString() + ",Debit Value:" + row["Debit"].ToString() + ",BillNo:" + row["BillNo"].ToString());
                    }
                    dgvNEqual.DataSource = ds.Tables[0];
                    //Close
                }
                else
                {                   
                    MessageBox.Show("Sales Invoice In Entry voucher Posted Successfully with document No. " + sVNo /*+ docno.Text*/, "Focus", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //If Posting Success insert into Buffer DB 	SDC_Revenue_Canc				                    
                    string Chkstr = "INSERT INTO[" + sqlserver2 + "].[Focus5030].dbo.SDC_Revenue_Canc([GLNm],[Credit],[Debit],[PayeeId],[PayeeType],[BillNo],[BillDate],[BillType],[FileNo],[VisitId],[Nationality],[ServCode],[DeptId],[NetAmt],[CashierId],[DrId],[CancelDate],[Source],[Status],[Remark],[PostingDateTime])" +
                                                     "SELECT [GLNm],[Credit],[Debit],[PayeeId],[PayeeType],[BillNo],[BillDate],[BillType],[FileNo],[VisitId],[Nationality],[ServCode],[DeptId],[NetAmt],[CashierId],[DrId],[CancelDate],[Source],[Status],[Remark],[PostingDateTime]" +
                                                     "FROM [" + sqlserver2 + "].[FamilyCare Testing].dbo.SDC_Revenue_Canc fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [" + sqlserver2 + "].[Focus5030].dbo.SDC_Revenue_Canc fd)";
                    ChkCmd = new SqlCommand(Chkstr, Chkconn);
                    da = new SqlDataAdapter(ChkCmd);
                    ds = new DataSet();
                    da.Fill(ds);
                    //Close                   
                }
            }
            catch (Exception ex) { ErrorLog(ex.ToString()); }
        }
        private void Stock_adj_Post()
        {
            string Pstr = "Store Adjustment";
            var ChkConStr = ClsComm.SqlConnectionString();
            SqlConnection Chkconn = new SqlConnection(ChkConStr);
            var constr2 = ClsComm.SqlConnectionString2();
            var connsplit = constr2.Split('=', ';');

            for (int count = 0; count <= connsplit.Length - 1; count++)
            {
                if (count % 2 == 0)
                {

                }
                else
                {
                    sqlserver2 = connsplit[1];
                    initialcatalog = connsplit[3];
                    Companycode = "030";
                    Username = connsplit[5];
                    password = connsplit[7];
                }
            }
            var ft = new Transaction();
            var sVNo = string.Empty;
            blnIsEditing = false;
            string sTmp = "";
            if (blnIsEditing == false)
            {
                sVNo = ft.GetNextVoucherNo("Jrn");
                ft.NewDocument("Jrn", sVNo);
            }
            else
            {
                ft.DeleteDocument("Jrn", sVNo);
            }

            ft.SetField("Date", dtpToDate.Value.ToString(/*"yyyy/MM/dd"*/));
            ft.SetField("VoucherType Name", Pstr);
            //Select Data from SDC_Stock_Adjust based on the bill no and comparing
            mySql = "SELECT [GLNm],[Credit],[Debit],[AdjNo],[AdjDate],[AdjStoreId],[AdjItemId],[ItemQty],[ItemRate],[TotalAdj],[ItemType],[Source],[Status],[Remark],[PostingDateTime]" +
               "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Stock_Adjust fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [" + sqlserver2 + "].[Focus5030].dbo.SDC_Stock_Adjust fd)";
            ds = ClsSql.GetDs(mySql);
            dt = ds.Tables[0];
            dgv.AllowUserToAddRows = false;
            dgv.DataSource = dt;
            //Close
            try
            {
                for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
                {

                    //var cell = dgv.Rows[n].Cells["GLNm"];
                    ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString());

                    double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value);
                    double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value);
                    if (str != 0)
                    {
                        ft.SetField("Amount", str.ToString());
                        ft.SetField("DrCr", "Cr");
                    }
                    else if (str1 != 0)
                    {
                        ft.SetField("Amount", str1.ToString());
                        ft.SetField("DrCr", "Dr");
                    }
                    //var cell = dgv.Rows[n].Cells["AdjNo"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["AdjNo"].Value.ToString()) : "";
                    //ft.SetField("AdjNo", sTmp);

                    //cell = dgv.Rows[n].Cells["AdjDate"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["AdjDate"].Value.ToString()) : "";
                    //ft.SetField("Adj Date", sTmp);

                    //cell = dgv.Rows[n].Cells["AdjStoreId"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["AdjStoreId"].Value.ToString()) : "";
                    //ft.SetField("AdjStore Id", sTmp);

                    //cell = dgv.Rows[n].Cells["AdjItemId"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["AdjItemId"].Value.ToString()) : "";
                    //ft.SetField("AdjItem Id", sTmp);

                    var cell = dgv.Rows[n].Cells["ItemQty"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemQty"].Value.ToString()) : "";
                    ft.SetField("Item Q", sTmp);

                    //cell = dgv.Rows[n].Cells["ItemRate"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemRate"].Value.ToString()) : "";
                    //ft.SetField("Item Rate", sTmp);

                    //cell = dgv.Rows[n].Cells["TotalAdj"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["TotalAdj"].Value.ToString()) : "";
                    //ft.SetField("TotalAdj", sTmp);

                    cell = dgv.Rows[n].Cells["ItemType"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemType"].Value.ToString()) : "";
                    ft.SetField("Item Type", sTmp);

                    cell = dgv.Rows[n].Cells["Source"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString()) : "";
                    ft.SetField("Source", sTmp);

                    ft.AddRow();
                }
                MessageBox.Show("5");
                ft.SetField("Approved", "1");
                int k = ft.SaveDocument();
                if (k != 1)
                {
                    dgvNEqual.Visible = true;
                    NotEqual();
                    MessageBox.Show("Unable to post the Sales Invoice", "Focus", MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //Checking if master is Available are Not                                  
                    Chkconn = new SqlConnection(constr2);
                    Chkconn.Open();
                    mySql = "SELECT Distinct[GLNm] FROM [" + sqlserver2 + "].[FamilyCare Testing].dbo.SDC_Stock_Adjust fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from [" + sqlserver2 + "].[Focus5030].dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
                    ds = ClsSql.GetDs2(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("Account Masters are not Available in Focus:" + row["GLNm"].ToString());
                    }
                    Chkconn.Close();
                    //End of Checking Masters
                    //checking If credit and debit Notes arenot equal 
                    mySql = "select SUM(credit) as Credit,SUM(debit) as Debit,BillNo from SDC_Stock_Adjust" +
                     "group by BillNo having SUM(credit) <> SUM(debit)";
                    ds = ClsSql.GetDs(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("....Credit and Debit Notes are not Equal:....\n Credit Value:" + row["Credit"].ToString() + ",Debit Value:" + row["Debit"].ToString() + ",BillNo:" + row["BillNo"].ToString());
                    }
                    dgvNEqual.DataSource = ds.Tables[0];
                    //Close
                }
                else
                {
                    MessageBox.Show("Sales Invoice In Entry voucher Posted Successfully with document No. " + sVNo /*+ docno.Text*/, "Focus", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //If Posting Success insert into Buffer DB 	SDC_Stock_Adjust				
                    string Chkstr = "INSERT INTO[" + sqlserver2 + "].[Focus5030].dbo.SDC_Stock_Adjust([GLNm],[Credit],[Debit],[AdjNo],[AdjDate],[AdjStoreId],[AdjItemId],[ItemQty],[ItemRate],[TotalAdj],[ItemType],[Source],[Status],[Remark],[PostingDateTime])" +
                                                     "SELECT [GLNm],[Credit],[Debit],[AdjNo],[AdjDate],[AdjStoreId],[AdjItemId],[ItemQty],[ItemRate],[TotalAdj],[ItemType],[Source],[Status],[Remark],[PostingDateTime]" +
                                                     "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Stock_Adjust fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [Focus5030].dbo.SDC_Stock_Adjust fd)";
                    ChkCmd = new SqlCommand(Chkstr, Chkconn);
                    da = new SqlDataAdapter(ChkCmd);
                    ds = new DataSet();
                    da.Fill(ds);
                    //Close	
                }
            }
            catch (Exception ex) { ErrorLog(ex.ToString()); }
        }
        private void Stock_Consum_Post()
        {
            string Pstr = "Store Consumption";
            var ChkConStr = ClsComm.SqlConnectionString();
            SqlConnection Chkconn = new SqlConnection(ChkConStr);
            var constr2 = ClsComm.SqlConnectionString2();
            var connsplit = constr2.Split('=', ';');

            for (int count = 0; count <= connsplit.Length - 1; count++)
            {
                if (count % 2 == 0)
                {

                }
                else
                {
                    sqlserver2 = connsplit[1];
                    initialcatalog = connsplit[3];
                    Companycode = "030";
                    Username = connsplit[5];
                    password = connsplit[7];
                }
            }
            var ft = new Transaction();
            var sVNo = string.Empty;
            blnIsEditing = false;
            string sTmp = "";
            if (blnIsEditing == false)
            {
                sVNo = ft.GetNextVoucherNo("Jrn");
                ft.NewDocument("Jrn", sVNo);
            }
            else
            {
                ft.DeleteDocument("Jrn", sVNo);
            }

            ft.SetField("Date", dtpToDate.Value.ToString(/*"yyyy/MM/dd"*/));
            ft.SetField("VoucherType Name", Pstr);
            //Select Data from SDC_Stock_Consum based on the bill no and comparing 
            mySql = "SELECT [GLNm],[Credit],[Debit],[ConsNo],[ConsDate],[ConsStoreId],[ItemId],[ItemRate],[ItemQty],[TotalCons],[ItemType],[Source],[Status],[Remark],[PostingDateTime]" +
               "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Stock_Consum fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [" + sqlserver2 + "].[Focus5030].dbo.SDC_Stock_Consum fd)";
            ds = ClsSql.GetDs(mySql);
            dt = ds.Tables[0];
            dgv.AllowUserToAddRows = false;
            dgv.DataSource = dt;
            //Close
            try
            {
                for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
                {

                    //var cell = dgv.Rows[n].Cells["GLNm"];
                    ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString());

                    double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value);
                    double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value);
                    if (str != 0)
                    {
                        ft.SetField("Amount", str.ToString());
                        ft.SetField("DrCr", "Cr");
                    }
                    else if (str1 != 0)
                    {
                        ft.SetField("Amount", str1.ToString());
                        ft.SetField("DrCr", "Dr");
                    }
                    //var cell = dgv.Rows[n].Cells["ConsNo"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ConsNo"].Value.ToString()) : "";
                    //ft.SetField("Cons No", sTmp);

                    //cell = dgv.Rows[n].Cells["ConsDate"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ConsDate"].Value.ToString()) : "";
                    //ft.SetField("Cons Date", sTmp);

                    //cell = dgv.Rows[n].Cells["ConsStoreId"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ConsStoreId"].Value.ToString()) : "";
                    //ft.SetField("ConsStore Id", sTmp);

                    var cell = dgv.Rows[n].Cells["ItemId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemId"].Value.ToString()) : "";
                    ft.SetField("Item ID", sTmp);

                    //cell = dgv.Rows[n].Cells["ItemRate"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemRate"].Value.ToString()) : "";
                    //ft.SetField("Item Rate", sTmp);

                    cell = dgv.Rows[n].Cells["ItemQty"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemQty"].Value.ToString()) : "";
                    ft.SetField("Item Q", sTmp);

                    //cell = dgv.Rows[n].Cells["TotalCons"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["TotalCons"].Value.ToString()) : "";
                    //ft.SetField("TotalCons", sTmp);

                    cell = dgv.Rows[n].Cells["ItemType"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemType"].Value.ToString()) : "";
                    ft.SetField("Item Type", sTmp);

                    cell = dgv.Rows[n].Cells["Source"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString()) : "";
                    ft.SetField("Source", sTmp);

                    ft.AddRow();
                }
                MessageBox.Show("5");
                ft.SetField("Approved", "1");
                int k = ft.SaveDocument();
                if (k != 1)
                {
                    dgvNEqual.Visible = true;
                    NotEqual();
                    MessageBox.Show("Unable to post the Sales Invoice", "Focus", MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //Checking if master is Available are Not                                  
                    Chkconn = new SqlConnection(constr2);
                    Chkconn.Open();
                    mySql = "SELECT Distinct[GLNm] FROM [" + sqlserver2 + "].[FamilyCare Testing].dbo.SDC_Stock_Consum fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from [" + sqlserver2 + "].[Focus5030].dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
                    ds = ClsSql.GetDs2(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("Account Masters are not Available in Focus:" + row["GLNm"].ToString());
                    }
                    Chkconn.Close();
                    //End of Checking Masters
                    //checking If credit and debit Notes arenot equal 
                    mySql = "select SUM(credit) as Credit,SUM(debit) as Debit,BillNo from SDC_Stock_Consum" +
                     "group by BillNo having SUM(credit) <> SUM(debit)";
                    ds = ClsSql.GetDs(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("....Credit and Debit Notes are not Equal:....\n Credit Value:" + row["Credit"].ToString() + ",Debit Value:" + row["Debit"].ToString() + ",BillNo:" + row["BillNo"].ToString());
                    }
                    dgvNEqual.DataSource = ds.Tables[0];
                    //Close
                }
                else
                {
                    MessageBox.Show("Sales Invoice In Entry voucher Posted Successfully with document No. " + sVNo /*+ docno.Text*/, "Focus", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //If Posting Success insert into Buffer DB 	SDC_Stock_Consum				
                    string Chkstr = "INSERT INTO[" + sqlserver2 + "].[Focus5030].dbo.SDC_Stock_Consum([GLNm],[Credit],[Debit],[ConsNo],[ConsDate],[ConsStoreId],[ItemId],[ItemRate],[ItemQty],[TotalCons],[ItemType],[Source],[Status],[Remark],[PostingDateTime])" +
                                                     "SELECT [GLNm],[Credit],[Debit],[ConsNo],[ConsDate],[ConsStoreId],[ItemId],[ItemRate],[ItemQty],[TotalCons],[ItemType],[Source],[Status],[Remark],[PostingDateTime]" +
                                                     "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Stock_Consum fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [Focus5030].dbo.SDC_Stock_Consum fd)";
                    ChkCmd = new SqlCommand(Chkstr, Chkconn);
                    da = new SqlDataAdapter(ChkCmd);
                    ds = new DataSet();
                    da.Fill(ds);
                    //Close
                }
            }
            catch (Exception ex) { ErrorLog(ex.ToString()); }
        }
        private void Stock_Consum_rtn_Post()
        {
            string Pstr = "Store Consumption Return";
            var ChkConStr = ClsComm.SqlConnectionString();
            SqlConnection Chkconn = new SqlConnection(ChkConStr);
            var constr2 = ClsComm.SqlConnectionString2();
            var connsplit = constr2.Split('=', ';');

            for (int count = 0; count <= connsplit.Length - 1; count++)
            {
                if (count % 2 == 0)
                {

                }
                else
                {
                    sqlserver2 = connsplit[1];
                    initialcatalog = connsplit[3];
                    Companycode = "030";
                    Username = connsplit[5];
                    password = connsplit[7];
                }
            }
            var ft = new Transaction();
            var sVNo = string.Empty;
            blnIsEditing = false;
            string sTmp = "";
            if (blnIsEditing == false)
            {
                sVNo = ft.GetNextVoucherNo("Jrn");
                ft.NewDocument("Jrn", sVNo);
            }
            else
            {
                ft.DeleteDocument("Jrn", sVNo);
            }

            ft.SetField("Date", dtpToDate.Value.ToString(/*"yyyy/MM/dd"*/));
            ft.SetField("VoucherType Name", Pstr);
            //Select Data from SDC_Stock_Consum_Return based on the bill no and comparing
            mySql = "SELECT [GLNm],[Credit],[Debit],[ConsNo],[ConsDate],[ConsStoreId],[ItemId],[ItemRate],[ItemQty],[TotalCons],[ItemType],[ConsCancDate],[ConsCancNo],[Source],[Status],[Remark],[PostingDateTime]" +
               "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Stock_Consum_Return fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [" + sqlserver2 + "].[Focus5030].dbo.SDC_Stock_Consum_Return fd)";
            ds = ClsSql.GetDs(mySql);
            dt = ds.Tables[0];
            dgv.AllowUserToAddRows = false;
            dgv.DataSource = dt;
            //Close
            try
            {
                for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
                {

                    //var cell = dgv.Rows[n].Cells["GLNm"];
                    ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString());

                    double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value);
                    double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value);
                    if (str != 0)
                    {
                        ft.SetField("Amount", str.ToString());
                        ft.SetField("DrCr", "Cr");
                    }
                    else if (str1 != 0)
                    {
                        ft.SetField("Amount", str1.ToString());
                        ft.SetField("DrCr", "Dr");
                    }
                    //var cell = dgv.Rows[n].Cells["ConsNo"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ConsNo"].Value.ToString()) : "";
                    //ft.SetField("Cons No", sTmp);

                    //cell = dgv.Rows[n].Cells["ConsDate"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ConsDate"].Value.ToString()) : "";
                    //ft.SetField("Cons Date", sTmp);

                    //cell = dgv.Rows[n].Cells["ConsStoreId"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ConsStoreId"].Value.ToString()) : "";
                    //ft.SetField("ConsStore Id", sTmp);

                    var cell = dgv.Rows[n].Cells["ItemId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemId"].Value.ToString()) : "";
                    ft.SetField("Item ID", sTmp);

                    //cell = dgv.Rows[n].Cells["ItemRate"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemRate"].Value.ToString()) : "";
                    //ft.SetField("Item Rate", sTmp);

                    cell = dgv.Rows[n].Cells["ItemQty"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemQty"].Value.ToString()) : "";
                    ft.SetField("Item Q", sTmp);

                    //cell = dgv.Rows[n].Cells["TotalCons"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["TotalCons"].Value.ToString()) : "";
                    //ft.SetField("TotalCons", sTmp);

                    cell = dgv.Rows[n].Cells["ItemType"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemType"].Value.ToString()) : "";
                    ft.SetField("Item Type", sTmp);

                    //cell = dgv.Rows[n].Cells["ConsCancDate"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ConsCancDate"].Value.ToString()) : "";
                    //ft.SetField("ConsCancDate", sTmp);

                    //cell = dgv.Rows[n].Cells["ConsCancNo"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ConsCancNo"].Value.ToString()) : "";
                    //ft.SetField("ConsCancNo", sTmp);

                    cell = dgv.Rows[n].Cells["Source"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString()) : "";
                    ft.SetField("Source", sTmp);

                    ft.AddRow();
                }
                MessageBox.Show("5");
                ft.SetField("Approved", "1");
                int k = ft.SaveDocument();
                if (k != 1)
                {
                    dgvNEqual.Visible = true;
                    NotEqual();
                    MessageBox.Show("Unable to post the Sales Invoice", "Focus", MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //Checking if master is Available are Not                                  
                    Chkconn = new SqlConnection(constr2);
                    Chkconn.Open();
                    mySql = "SELECT Distinct[GLNm] FROM [" + sqlserver2 + "].[FamilyCare Testing].dbo.SDC_Stock_Consum_Return fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from [" + sqlserver2 + "].[Focus5030].dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
                    ds = ClsSql.GetDs2(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("Account Masters are not Available in Focus:" + row["GLNm"].ToString());
                    }
                    Chkconn.Close();
                    //End of Checking Masters
                    //checking If credit and debit Notes arenot equal 
                    mySql = "select SUM(credit) as Credit,SUM(debit) as Debit,BillNo from SDC_Stock_Consum_Return" +
                     "group by BillNo having SUM(credit) <> SUM(debit)";
                    ds = ClsSql.GetDs(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("....Credit and Debit Notes are not Equal:....\n Credit Value:" + row["Credit"].ToString() + ",Debit Value:" + row["Debit"].ToString() + ",BillNo:" + row["BillNo"].ToString());
                    }
                    dgvNEqual.DataSource = ds.Tables[0];
                    //Close
                }
                else
                {
                    MessageBox.Show("Sales Invoice In Entry voucher Posted Successfully with document No. " + sVNo /*+ docno.Text*/, "Focus", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //If Posting Success insert into Buffer DB 	SDC_Stock_Consum_Return				
                    string Chkstr = "INSERT INTO[" + sqlserver2 + "].[Focus5030].dbo.SDC_Stock_Consum_Return([GLNm],[Credit],[Debit],[ConsNo],[ConsDate],[ConsStoreId],[ItemId],[ItemRate],[ItemQty],[TotalCons],[ItemType],[ConsCancDate],[ConsCancNo],[Source],[Status],[Remark],[PostingDateTime])" +
                                                     "SELECT [GLNm],[Credit],[Debit],[ConsNo],[ConsDate],[ConsStoreId],[ItemId],[ItemRate],[ItemQty],[TotalCons],[ItemType],[ConsCancDate],[ConsCancNo],[Source],[Status],[Remark],[PostingDateTime]" +
                                                     "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Stock_Consum_Return fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [Focus5030].dbo.SDC_Stock_Consum_Return fd)";
                    ChkCmd = new SqlCommand(Chkstr, Chkconn);
                    da = new SqlDataAdapter(ChkCmd);
                    ds = new DataSet();
                    da.Fill(ds);
                    //Close
                }
            }
            catch (Exception ex) { ErrorLog(ex.ToString()); }
        }
        private void Stock_dispose_Post()
        {
            string Pstr = "Store Dispose";
            var ChkConStr = ClsComm.SqlConnectionString();
            SqlConnection Chkconn = new SqlConnection(ChkConStr);
            var constr2 = ClsComm.SqlConnectionString2();
            var connsplit = constr2.Split('=', ';');

            for (int count = 0; count <= connsplit.Length - 1; count++)
            {
                if (count % 2 == 0)
                {

                }
                else
                {
                    sqlserver2 = connsplit[1];
                    initialcatalog = connsplit[3];
                    Companycode = "030";
                    Username = connsplit[5];
                    password = connsplit[7];
                }
            }
            var ft = new Transaction();
            var sVNo = string.Empty;
            blnIsEditing = false;
            string sTmp = "";
            if (blnIsEditing == false)
            {
                sVNo = ft.GetNextVoucherNo("Jrn");
                ft.NewDocument("Jrn", sVNo);
            }
            else
            {
                ft.DeleteDocument("Jrn", sVNo);
            }

            ft.SetField("Date", dtpToDate.Value.ToString(/*"yyyy/MM/dd"*/));
            ft.SetField("VoucherType Name", Pstr);
            //Select Data from SDC_Stock_Dispose based on the bill no and comparing 
            mySql = "SELECT [GLNm],[Credit],[Debit],[DisposeId],[DisposeNo],[DisposeStoreId],[ItemId],[ItemRate],[ItemQty],[TotalDispose],[ItemType],[Source],[Status],[Remark],[PostingDateTime]" +
               "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Stock_Dispose fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [" + sqlserver2 + "].[Focus5030].dbo.SDC_Stock_Dispose fd)";
            ds = ClsSql.GetDs(mySql);
            dt = ds.Tables[0];
            dgv.AllowUserToAddRows = false;
            dgv.DataSource = dt;
            //Close
            try
            {
                for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
                {

                    //var cell = dgv.Rows[n].Cells["GLNm"];
                    ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString());

                    double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value);
                    double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value);
                    if (str != 0)
                    {
                        ft.SetField("Amount", str.ToString());
                        ft.SetField("DrCr", "Cr");
                    }
                    else if (str1 != 0)
                    {
                        ft.SetField("Amount", str1.ToString());
                        ft.SetField("DrCr", "Dr");
                    }
                    //var cell = dgv.Rows[n].Cells["DisposeId"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["DisposeId"].Value.ToString()) : "";
                    //ft.SetField("Dispose Id", sTmp);

                    //cell = dgv.Rows[n].Cells["DisposeNo"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["DisposeNo"].Value.ToString()) : "";
                    //ft.SetField("Dispose No", sTmp);

                    //cell = dgv.Rows[n].Cells["DisposeStoreId"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["DisposeStoreId"].Value.ToString()) : "";
                    //ft.SetField("DisposeStore Id", sTmp);

                    //cell = dgv.Rows[n].Cells["DisposeDate"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["DisposeDate"].Value.ToString()) : "";
                    //ft.SetField("Dispose Date", sTmp);

                    var cell = dgv.Rows[n].Cells["ItemId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemId"].Value.ToString()) : "";
                    ft.SetField("Item ID", sTmp);

                    //cell = dgv.Rows[n].Cells["ItemRate"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemRate"].Value.ToString()) : "";
                    //ft.SetField("Item Rate", sTmp);

                    cell = dgv.Rows[n].Cells["ItemQty"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemQty"].Value.ToString()) : "";
                    ft.SetField("Item Q", sTmp);

                    //cell = dgv.Rows[n].Cells["TotalDispose"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["TotalDispose"].Value.ToString()) : "";
                    //ft.SetField("TotalDispose", sTmp);

                    cell = dgv.Rows[n].Cells["ItemType"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemType"].Value.ToString()) : "";
                    ft.SetField("Item Type", sTmp);

                    cell = dgv.Rows[n].Cells["Source"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString()) : "";
                    ft.SetField("Source", sTmp);

                    ft.AddRow();
                }
                MessageBox.Show("5");
                ft.SetField("Approved", "1");
                int k = ft.SaveDocument();
                if (k != 1)
                {
                    dgvNEqual.Visible = true;
                    NotEqual();
                    MessageBox.Show("Unable to post the Sales Invoice", "Focus", MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //Checking if master is Available are Not                                  
                    Chkconn = new SqlConnection(constr2);
                    Chkconn.Open();
                    mySql = "SELECT Distinct[GLNm] FROM [" + sqlserver2 + "].[FamilyCare Testing].dbo.SDC_Stock_Dispose fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from [" + sqlserver2 + "].[Focus5030].dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
                    ds = ClsSql.GetDs2(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("Account Masters are not Available in Focus:" + row["GLNm"].ToString());
                    }
                    Chkconn.Close();
                    //End of Checking Masters
                    //checking If credit and debit Notes arenot equal 
                    mySql = "select SUM(credit) as Credit,SUM(debit) as Debit,BillNo from SDC_Stock_Dispose" +
                     "group by BillNo having SUM(credit) <> SUM(debit)";
                    ds = ClsSql.GetDs(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("....Credit and Debit Notes are not Equal:....\n Credit Value:" + row["Credit"].ToString() + ",Debit Value:" + row["Debit"].ToString() + ",BillNo:" + row["BillNo"].ToString());
                    }
                    dgvNEqual.DataSource = ds.Tables[0];
                    //Close
                }
                else
                {
                    MessageBox.Show("Sales Invoice In Entry voucher Posted Successfully with document No. " + sVNo /*+ docno.Text*/, "Focus", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //If Posting Success insert into Buffer DB 	SDC_Stock_Dispose				
                    string Chkstr = "INSERT INTO[" + sqlserver2 + "].[Focus5030].dbo.SDC_Stock_Dispose([GLNm],[Credit],[Debit],[DisposeId],[DisposeNo],[DisposeStoreId],[ItemId],[ItemRate],[ItemQty],[TotalDispose],[ItemType],[Source],[Status],[Remark],[PostingDateTime])" +
                                                     "SELECT [GLNm],[Credit],[Debit],[DisposeId],[DisposeNo],[DisposeStoreId],[ItemId],[ItemRate],[ItemQty],[TotalDispose],[ItemType],[Source],[Status],[Remark],[PostingDateTime]" +
                                                     "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Stock_Dispose fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [Focus5030].dbo.SDC_Stock_Dispose fd)";
                    ChkCmd = new SqlCommand(Chkstr, Chkconn);
                    da = new SqlDataAdapter(ChkCmd);
                    ds = new DataSet();
                    da.Fill(ds);
                    //Close
                }
            }
            catch (Exception ex) { ErrorLog(ex.ToString()); }
        }
        private void Stock_Issue_Post()
        {
            string Pstr = "Store Issue";
            var ChkConStr = ClsComm.SqlConnectionString();
            SqlConnection Chkconn = new SqlConnection(ChkConStr);
            var constr2 = ClsComm.SqlConnectionString2();
            var connsplit = constr2.Split('=', ';');

            for (int count = 0; count <= connsplit.Length - 1; count++)
            {
                if (count % 2 == 0)
                {

                }
                else
                {
                    sqlserver2 = connsplit[1];
                    initialcatalog = connsplit[3];
                    Companycode = "030";
                    Username = connsplit[5];
                    password = connsplit[7];
                }
            }
            var ft = new Transaction();
            var sVNo = string.Empty;
            blnIsEditing = false;
            string sTmp = "";
            if (blnIsEditing == false)
            {
                sVNo = ft.GetNextVoucherNo("Jrn");
                ft.NewDocument("Jrn", sVNo);
            }
            else
            {
                ft.DeleteDocument("Jrn", sVNo);
            }

            ft.SetField("Date", dtpToDate.Value.ToString(/*"yyyy/MM/dd"*/));
            ft.SetField("VoucherType Name", Pstr);
            //Select Data from SDC_Stock_Issue based on the bill no and comparing 
            mySql = "SELECT [GLNm],[Credit],[Debit],[IssueNo],[IssueDate],[ReqNo],[OutStoreId],[InStoreId],[ItemId],[ItemQty],[TotalIssue],[ItemType],[Source],[Status],[Remark],[PostingDateTime]" +
               "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Stock_Issue fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [" + sqlserver2 + "].[Focus5030].dbo.SDC_Stock_Issue fd)";
            ds = ClsSql.GetDs(mySql);
            dt = ds.Tables[0];
            dgv.AllowUserToAddRows = false;
            //Close
            try
            {
                for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
                {

                    //var cell = dgv.Rows[n].Cells["GLNm"];
                    ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString());

                    double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value);
                    double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value);
                    if (str != 0)
                    {
                        ft.SetField("Amount", str.ToString());
                        ft.SetField("DrCr", "Cr");
                    }
                    else if (str1 != 0)
                    {
                        ft.SetField("Amount", str1.ToString());
                        ft.SetField("DrCr", "Dr");
                    }
                    var cell = dgv.Rows[n].Cells["IssueNo"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["IssueNo"].Value.ToString()) : "";
                    ft.SetField("P Issue", sTmp);

                    cell = dgv.Rows[n].Cells["IssueDate"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["IssueDate"].Value.ToString()) : "";
                    ft.SetField("P Issue Date", sTmp);

                    cell = dgv.Rows[n].Cells["ReqNo"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ReqNo"].Value.ToString()) : "";
                    ft.SetField("Req", sTmp);

                    cell = dgv.Rows[n].Cells["OutStoreId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["OutStoreId"].Value.ToString()) : "";
                    ft.SetField("Out Store ID", sTmp);

                    cell = dgv.Rows[n].Cells["InStoreId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["InStoreId"].Value.ToString()) : "";
                    ft.SetField("In Store ID", sTmp);

                    cell = dgv.Rows[n].Cells["ItemId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemId"].Value.ToString()) : "";
                    ft.SetField("Item ID", sTmp);

                    //cell = dgv.Rows[n].Cells["ItemRate"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemRate"].Value.ToString()) : "";
                    //ft.SetField("Item Rate", sTmp);

                    cell = dgv.Rows[n].Cells["ItemQty"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemQty"].Value.ToString()) : "";
                    ft.SetField("Item Q", sTmp);

                    //cell = dgv.Rows[n].Cells["TotalIssue"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["TotalIssue"].Value.ToString()) : "";
                    //ft.SetField("Total Issue", sTmp);

                    cell = dgv.Rows[n].Cells["ItemType"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemType"].Value.ToString()) : "";
                    ft.SetField("Item Type", sTmp);

                    cell = dgv.Rows[n].Cells["Source"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString()) : "";
                    ft.SetField("Source", sTmp);

                    ft.AddRow();
                }
                MessageBox.Show("5");
                ft.SetField("Approved", "1");
                int k = ft.SaveDocument();
                if (k != 1)
                {
                    dgvNEqual.Visible = true;
                    NotEqual();
                    MessageBox.Show("Unable to post the Sales Invoice", "Focus", MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //Checking if master is Available are Not                                  
                    Chkconn = new SqlConnection(constr2);
                    Chkconn.Open();
                    mySql = "SELECT Distinct[GLNm] FROM [" + sqlserver2 + "].[FamilyCare Testing].dbo.SDC_Stock_Issue fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from [" + sqlserver2 + "].[Focus5030].dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
                    ds = ClsSql.GetDs2(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("Account Masters are not Available in Focus:" + row["GLNm"].ToString());
                    }
                    Chkconn.Close();
                    //End of Checking Masters
                    //checking If credit and debit Notes arenot equal 
                    mySql = "select SUM(credit) as Credit,SUM(debit) as Debit,BillNo from SDC_Stock_Issue" +
                     "group by BillNo having SUM(credit) <> SUM(debit)";
                    ds = ClsSql.GetDs(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("....Credit and Debit Notes are not Equal:....\n Credit Value:" + row["Credit"].ToString() + ",Debit Value:" + row["Debit"].ToString() + ",BillNo:" + row["BillNo"].ToString());
                    }
                    dgvNEqual.DataSource = ds.Tables[0];
                    //Close
                }
                else
                {
                    MessageBox.Show("Sales Invoice In Entry voucher Posted Successfully with document No. " + sVNo /*+ docno.Text*/, "Focus", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //If Posting Success insert into Buffer DB 	SDC_Stock_Issue				
                    string Chkstr = "INSERT INTO[" + sqlserver2 + "].[Focus5030].dbo.SDC_Stock_Issue([GLNm],[Credit],[Debit],[IssueNo],[IssueDate],[ReqNo],[OutStoreId],[InStoreId],[ItemId],[ItemQty],[TotalIssue],[ItemType],[Source],[Status],[Remark],[PostingDateTime])" +
                                                     "SELECT [GLNm],[Credit],[Debit],[IssueNo],[IssueDate],[ReqNo],[OutStoreId],[InStoreId],[ItemId],[ItemQty],[TotalIssue],[ItemType],[Source],[Status],[Remark],[PostingDateTime]" +
                                                     "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Stock_Issue fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [Focus5030].dbo.SDC_Stock_Issue fd)";
                    ChkCmd = new SqlCommand(Chkstr, Chkconn);
                    da = new SqlDataAdapter(ChkCmd);
                    ds = new DataSet();
                    da.Fill(ds);
                    //Close	
                }
            }
            catch (Exception ex) { ErrorLog(ex.ToString()); }
        }
        private void Stock_Issue_rtn_Post()
        {
            string Pstr = "Store Issue Return";
            var ChkConStr = ClsComm.SqlConnectionString();
            SqlConnection Chkconn = new SqlConnection(ChkConStr);
            var constr2 = ClsComm.SqlConnectionString2();
            var connsplit = constr2.Split('=', ';');

            for (int count = 0; count <= connsplit.Length - 1; count++)
            {
                if (count % 2 == 0)
                {

                }
                else
                {
                    sqlserver2 = connsplit[1];
                    initialcatalog = connsplit[3];
                    Companycode = "030";
                    Username = connsplit[5];
                    password = connsplit[7];
                }
            }
            var ft = new Transaction();
            var sVNo = string.Empty;
            blnIsEditing = false;
            string sTmp = "";
            if (blnIsEditing == false)
            {
                sVNo = ft.GetNextVoucherNo("Jrn");
                ft.NewDocument("Jrn", sVNo);
            }
            else
            {
                ft.DeleteDocument("Jrn", sVNo);
            }

            ft.SetField("Date", dtpToDate.Value.ToString(/*"yyyy/MM/dd"*/));
            ft.SetField("VoucherType Name", Pstr);
            //Select Data from SDC_Stock_Issue_Return based on the bill no and comparing
            mySql = "SELECT [GLNm],[Credit],[Debit],[MatRetNo],[MatRetDate],[OutStoreId],[InStoreId],[ItemId],[ItemRate],[ItemQty],[TotalReturn],[ItemType],[Source],[Status],[Remark],[PostingDateTime]" +
               "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Stock_Issue_Return fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [" + sqlserver2 + "].[Focus5030].dbo.SDC_Stock_Issue_Return fd)";
            ds = ClsSql.GetDs(mySql);
            dt = ds.Tables[0];
            dgv.AllowUserToAddRows = false;
            dgv.DataSource = dt;
            //Close
            try
            {
                for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
                {

                    //var cell = dgv.Rows[n].Cells["GLNm"];
                    ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString());

                    double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value);
                    double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value);
                    if (str != 0)
                    {
                        ft.SetField("Amount", str.ToString());
                        ft.SetField("DrCr", "Cr");
                    }
                    else if (str1 != 0)
                    {
                        ft.SetField("Amount", str1.ToString());
                        ft.SetField("DrCr", "Dr");
                    }
                    //var cell = dgv.Rows[n].Cells["MatMetNo"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["MatMetNo"].Value.ToString()) : "";
                    //ft.SetField("MatMetNo", sTmp);

                    //cell = dgv.Rows[n].Cells["MatMetNo"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["MatMetNo"].Value.ToString()) : "";
                    //ft.SetField("MatMet Date", sTmp);

                    var cell = dgv.Rows[n].Cells["OutStoreId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["OutStoreId"].Value.ToString()) : "";
                    ft.SetField("Out Store ID", sTmp);

                    cell = dgv.Rows[n].Cells["InStoreId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["InStoreId"].Value.ToString()) : "";
                    ft.SetField("In Store ID", sTmp);

                    cell = dgv.Rows[n].Cells["ItemId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemId"].Value.ToString()) : "";
                    ft.SetField("Item ID", sTmp);

                    //cell = dgv.Rows[n].Cells["ItemRate"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemRate"].Value.ToString()) : "";
                    //ft.SetField("Item Rate", sTmp);

                    cell = dgv.Rows[n].Cells["ItemQty"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemQty"].Value.ToString()) : "";
                    ft.SetField("Item Q", sTmp);

                    //cell = dgv.Rows[n].Cells["TotalReturn"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["TotalReturn"].Value.ToString()) : "";
                    //ft.SetField("TotalReturn", sTmp);

                    cell = dgv.Rows[n].Cells["ItemType"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemType"].Value.ToString()) : "";
                    ft.SetField("Item Type", sTmp);

                    cell = dgv.Rows[n].Cells["Source"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString()) : "";
                    ft.SetField("Source", sTmp);

                    ft.AddRow();
                }
                MessageBox.Show("5");
                ft.SetField("Approved", "1");
                int k = ft.SaveDocument();
                if (k != 1)
                {
                    dgvNEqual.Visible = true;
                    NotEqual();
                    MessageBox.Show("Unable to post the Sales Invoice", "Focus", MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //Checking if master is Available are Not                                  
                    Chkconn = new SqlConnection(constr2);
                    Chkconn.Open();
                    mySql = "SELECT Distinct[GLNm] FROM [" + sqlserver2 + "].[FamilyCare Testing].dbo.SDC_Stock_Issue_Return fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from [" + sqlserver2 + "].[Focus5030].dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
                    ds = ClsSql.GetDs2(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("Account Masters are not Available in Focus:" + row["GLNm"].ToString());
                    }
                    Chkconn.Close();
                    //End of Checking Masters
                    //checking If credit and debit Notes arenot equal 
                    mySql = "select SUM(credit) as Credit,SUM(debit) as Debit,BillNo from SDC_Stock_Issue_Return" +
                     "group by BillNo having SUM(credit) <> SUM(debit)";
                    ds = ClsSql.GetDs(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("....Credit and Debit Notes are not Equal:....\n Credit Value:" + row["Credit"].ToString() + ",Debit Value:" + row["Debit"].ToString() + ",BillNo:" + row["BillNo"].ToString());
                    }
                    dgvNEqual.DataSource = ds.Tables[0];
                    //Close
                }
                else
                {
                    MessageBox.Show("Sales Invoice In Entry voucher Posted Successfully with document No. " + sVNo /*+ docno.Text*/, "Focus", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //If Posting Success insert into Buffer DB 	SDC_Stock_Issue_Return				
                    string Chkstr = "INSERT INTO[" + sqlserver2 + "].[Focus5030].dbo.SDC_Stock_Issue_Return([GLNm],[Credit],[Debit],[MatRetNo],[MatRetDate],[OutStoreId],[InStoreId],[ItemId],[ItemRate],[ItemQty],[TotalReturn],[ItemType],[Source],[Status],[Remark],[PostingDateTime])" +
                                                     "SELECT [GLNm],[Credit],[Debit],[MatRetNo],[MatRetDate],[OutStoreId],[InStoreId],[ItemId],[ItemRate],[ItemQty],[TotalReturn],[ItemType],[Source],[Status],[Remark],[PostingDateTime]" +
                                                     "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_Stock_Issue_Return fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [Focus5030].dbo.SDC_Stock_Issue_Return fd)";
                    ChkCmd = new SqlCommand(Chkstr, Chkconn);
                    da = new SqlDataAdapter(ChkCmd);
                    ds = new DataSet();
                    da.Fill(ds);
                    //Close
                }
            }
            catch (Exception ex) { ErrorLog(ex.ToString()); }
        }
        private void WorkOrder_Post()
        {
            string Pstr = "Work Order";
            var ChkConStr = ClsComm.SqlConnectionString();
            SqlConnection Chkconn = new SqlConnection(ChkConStr);
            var constr2 = ClsComm.SqlConnectionString2();
            var connsplit = constr2.Split('=', ';');

            for (int count = 0; count <= connsplit.Length - 1; count++)
            {
                if (count % 2 == 0)
                {

                }
                else
                {
                    sqlserver2 = connsplit[1];
                    initialcatalog = connsplit[3];
                    Companycode = "030";
                    Username = connsplit[5];
                    password = connsplit[7];
                }
            }
            var ft = new Transaction();
            var sVNo = string.Empty;
            blnIsEditing = false;
            string sTmp = "";
            if (blnIsEditing == false)
            {
                sVNo = ft.GetNextVoucherNo("Jrn");
                ft.NewDocument("Jrn", sVNo);
            }
            else
            {
                ft.DeleteDocument("Jrn", sVNo);
            }

            ft.SetField("Date", dtpToDate.Value.ToString(/*"yyyy/MM/dd"*/));
            ft.SetField("VoucherType Name", Pstr);
            //Select Data from SDC_WorkOrder based on the bill no and comparing 
            mySql = "SELECT [GLNm],[Credit],[Debit],[WorkOrdNo],[SupplierId],[PONo],[SupBillNo],[ItemId],[ItemType],[VATType],[ItemQty],[FreeQty],[ItemPrice],[Disc],[Net],[VAT],[StoreId],[GRNDate],[DueDate],[Source],[Status],[Remark],[PostingDateTime]" +
               "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_WorkOrder fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [" + sqlserver2 + "].[Focus5030].dbo.SDC_WorkOrder fd)";
            ds = ClsSql.GetDs(mySql);
            dt = ds.Tables[0];
            dgv.AllowUserToAddRows = false;
            dgv.DataSource = dt;
            //Close
            try
            {
                for (int n = 0; n <= dgv.Rows.Count - 1; n = n + 1)
                {

                    //var cell = dgv.Rows[n].Cells["GLNm"];
                    ft.SetField("Account Name", dgv.Rows[n].Cells["GLNm"].Value.ToString());

                    double str = Convert.ToDouble(dgv.Rows[n].Cells["Credit"].Value);
                    double str1 = Convert.ToDouble(dgv.Rows[n].Cells["Debit"].Value);
                    if (str != 0)
                    {
                        ft.SetField("Amount", str.ToString());
                        ft.SetField("DrCr", "Cr");
                    }
                    else if (str1 != 0)
                    {
                        ft.SetField("Amount", str1.ToString());
                        ft.SetField("DrCr", "Dr");
                    }
                    //var cell = dgv.Rows[n].Cells["WorkOrdNo"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["WorkOrdNo"].Value.ToString()) : "";
                    //ft.SetField("WorkOrdNo", sTmp);

                    var cell = dgv.Rows[n].Cells["SupplierId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["SupplierId"].Value.ToString()) : "";
                    ft.SetField("Supplier ID", sTmp);

                    cell = dgv.Rows[n].Cells["PONo"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["PONo"].Value.ToString()) : "";
                    ft.SetField("PO", sTmp);

                    cell = dgv.Rows[n].Cells["SubBillNo"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["SubBillNo"].Value.ToString()) : "";
                    ft.SetField("Sup Bill", sTmp);

                    cell = dgv.Rows[n].Cells["ItemId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemId"].Value.ToString()) : "";
                    ft.SetField("Item ID", sTmp);

                    cell = dgv.Rows[n].Cells["ItemType"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemType"].Value.ToString()) : "";
                    ft.SetField("Item Type", sTmp);

                    cell = dgv.Rows[n].Cells["VATType"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VATType"].Value.ToString()) : "";
                    ft.SetField("VAT Type", sTmp);

                    cell = dgv.Rows[n].Cells["ItemQty"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemQty"].Value.ToString()) : "";
                    ft.SetField("Item Q", sTmp);

                    cell = dgv.Rows[n].Cells["FreeQty"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["FreeQty"].Value.ToString()) : "";
                    ft.SetField("Free Q", sTmp);

                    cell = dgv.Rows[n].Cells["ItemPrice"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["ItemPrice"].Value.ToString()) : "";
                    ft.SetField("Item Price", sTmp);

                    cell = dgv.Rows[n].Cells["Disc"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Disc"].Value.ToString()) : "";
                    ft.SetField("Disc", sTmp);

                    cell = dgv.Rows[n].Cells["Net"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Net"].Value.ToString()) : "";
                    ft.SetField("Net", sTmp);

                    cell = dgv.Rows[n].Cells["VAT"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["VAT"].Value.ToString()) : "";
                    ft.SetField("VAT", sTmp);

                    cell = dgv.Rows[n].Cells["StoreId"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["StoreId"].Value.ToString()) : "";
                    ft.SetField("Store ID", sTmp);
                    //JV
                    //cell = dgv.Rows[n].Cells["GRNDate"];// Nul value handling
                    //sTmp = cell.Value != null ? (dgv.Rows[n].Cells["GRNDate"].Value.ToString()) : "";
                    //ft.SetField("GRN Date", sTmp);

                    cell = dgv.Rows[n].Cells["DueDate"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["DueDate"].Value.ToString()) : "";
                    ft.SetField("Due Date", sTmp);

                    cell = dgv.Rows[n].Cells["Source"];// Nul value handling
                    sTmp = cell.Value != null ? (dgv.Rows[n].Cells["Source"].Value.ToString()) : "";
                    ft.SetField("Source", sTmp);

                    ft.AddRow();
                }
                MessageBox.Show("5");
                ft.SetField("Approved", "1");
                int k = ft.SaveDocument();
                if (k != 1)
                {
                    dgvNEqual.Visible = true;
                    NotEqual();
                    MessageBox.Show("Unable to post the Sales Invoice", "Focus", MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //Checking if master is Available are Not                                  
                    Chkconn = new SqlConnection(constr2);
                    Chkconn.Open();
                    mySql = "SELECT Distinct[GLNm] FROM [" + sqlserver2 + "].[FamilyCare Testing].dbo.SDC_WorkOrder fc where fc.GLNm COLLATE DATABASE_DEFAULT Not IN(select fd.Name from [" + sqlserver2 + "].[Focus5030].dbo.mr000 fd)";//;  dgv.Rows[0].Cells["GLNm"].Value.ToString()     
                    ds = ClsSql.GetDs2(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("Account Masters are not Available in Focus:" + row["GLNm"].ToString());
                    }
                    Chkconn.Close();
                    //End of Checking Masters
                    //checking If credit and debit Notes arenot equal 
                    mySql = "select SUM(credit) as Credit,SUM(debit) as Debit,BillNo from SDC_WorkOrder" +
                     "group by BillNo having SUM(credit) <> SUM(debit)";
                    ds = ClsSql.GetDs(mySql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ErrorLog("....Credit and Debit Notes are not Equal:....\n Credit Value:" + row["Credit"].ToString() + ",Debit Value:" + row["Debit"].ToString() + ",BillNo:" + row["BillNo"].ToString());
                    }
                    dgvNEqual.DataSource = ds.Tables[0];
                    //Close
                }
                else
                {
                    MessageBox.Show("Sales Invoice In Entry voucher Posted Successfully with document No. " + sVNo /*+ docno.Text*/, "Focus", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    //If Posting Success insert into Buffer DB 	SDC_WorkOrder				
                    string Chkstr = "INSERT INTO[" + sqlserver2 + "].[Focus5030].dbo.SDC_WorkOrder([GLNm],[Credit],[Debit],[WorkOrdNo],[SupplierId],[PONo],[SupBillNo],[ItemId],[ItemType],[VATType],[ItemQty],[FreeQty],[ItemPrice],[Disc],[Net],[VAT],[StoreId],[GRNDate],[DueDate],[Source],[Status],[Remark],[PostingDateTime])" +
                                                     "SELECT [GLNm],[Credit],[Debit],[WorkOrdNo],[SupplierId],[PONo],[SupBillNo],[ItemId],[ItemType],[VATType],[ItemQty],[FreeQty],[ItemPrice],[Disc],[Net],[VAT],[StoreId],[GRNDate],[DueDate],[Source],[Status],[Remark],[PostingDateTime]" +
                                                     "FROM [" + sqlserver2 + "].[FamilyCareNew].dbo.SDC_WorkOrder fc where fc.BillNo COLLATE DATABASE_DEFAULT Not IN(select fd.BillNo from [Focus5030].dbo.SDC_WorkOrder fd)";
                    ChkCmd = new SqlCommand(Chkstr, Chkconn);
                    da = new SqlDataAdapter(ChkCmd);
                    ds = new DataSet();
                    da.Fill(ds);
                    //Close
                }
            }
            catch (Exception ex) { ErrorLog(ex.ToString()); }

        }
        
        private void btnGetData_Click(object sender, EventArgs e)
        {
            // GetData();
            //  this.btnPost.Enabled = true;
        }
    }
}
