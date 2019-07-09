namespace FamilyCarePro
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgv = new System.Windows.Forms.DataGridView();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.btnPost = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCr = new System.Windows.Forms.TextBox();
            this.txtDr = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvNEqual = new System.Windows.Forms.DataGridView();
            this.lblNot = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbVoucher = new System.Windows.Forms.ComboBox();
            this.btnGetData = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNEqual)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Location = new System.Drawing.Point(1, 82);
            this.dgv.Name = "dgv";
            this.dgv.Size = new System.Drawing.Size(528, 164);
            this.dgv.TabIndex = 0;
            // 
            // dtpToDate
            // 
            this.dtpToDate.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpToDate.Location = new System.Drawing.Point(268, 10);
            this.dtpToDate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(120, 22);
            this.dtpToDate.TabIndex = 6;
            this.dtpToDate.TabStop = false;
            this.dtpToDate.Value = new System.DateTime(2019, 4, 2, 17, 6, 0, 0);
            // 
            // btnPost
            // 
            this.btnPost.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPost.Location = new System.Drawing.Point(407, 9);
            this.btnPost.Name = "btnPost";
            this.btnPost.Size = new System.Drawing.Size(88, 23);
            this.btnPost.TabIndex = 10;
            this.btnPost.Text = "&Post Data";
            this.btnPost.UseVisualStyleBackColor = true;
            this.btnPost.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 16);
            this.label4.TabIndex = 12;
            this.label4.Text = "Voucher:";
            // 
            // txtCr
            // 
            this.txtCr.Location = new System.Drawing.Point(128, 251);
            this.txtCr.Name = "txtCr";
            this.txtCr.Size = new System.Drawing.Size(100, 20);
            this.txtCr.TabIndex = 14;
            // 
            // txtDr
            // 
            this.txtDr.Location = new System.Drawing.Point(337, 251);
            this.txtDr.Name = "txtDr";
            this.txtDr.Size = new System.Drawing.Size(100, 20);
            this.txtDr.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(32, 253);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 16);
            this.label1.TabIndex = 16;
            this.label1.Text = "Credit Total:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(242, 253);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 16);
            this.label2.TabIndex = 17;
            this.label2.Text = "Debit Total:";
            // 
            // dgvNEqual
            // 
            this.dgvNEqual.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dgvNEqual.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvNEqual.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNEqual.Location = new System.Drawing.Point(49, 323);
            this.dgvNEqual.Name = "dgvNEqual";
            this.dgvNEqual.Size = new System.Drawing.Size(388, 81);
            this.dgvNEqual.TabIndex = 18;
            // 
            // lblNot
            // 
            this.lblNot.AutoSize = true;
            this.lblNot.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNot.ForeColor = System.Drawing.Color.Red;
            this.lblNot.Location = new System.Drawing.Point(12, 295);
            this.lblNot.Name = "lblNot";
            this.lblNot.Size = new System.Drawing.Size(0, 16);
            this.lblNot.TabIndex = 19;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(221, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 16);
            this.label6.TabIndex = 22;
            this.label6.Text = "Date:";
            // 
            // cmbVoucher
            // 
            this.cmbVoucher.FormattingEnabled = true;
            this.cmbVoucher.Location = new System.Drawing.Point(87, 11);
            this.cmbVoucher.Name = "cmbVoucher";
            this.cmbVoucher.Size = new System.Drawing.Size(121, 21);
            this.cmbVoucher.TabIndex = 24;
            // 
            // btnGetData
            // 
            this.btnGetData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetData.Location = new System.Drawing.Point(407, 38);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(88, 23);
            this.btnGetData.TabIndex = 25;
            this.btnGetData.Text = "&Get Data";
            this.btnGetData.UseVisualStyleBackColor = true;
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 417);
            this.Controls.Add(this.btnGetData);
            this.Controls.Add(this.cmbVoucher);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblNot);
            this.Controls.Add(this.dgvNEqual);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDr);
            this.Controls.Add(this.txtCr);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnPost);
            this.Controls.Add(this.dtpToDate);
            this.Controls.Add(this.dgv);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNEqual)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Button btnPost;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCr;
        private System.Windows.Forms.TextBox txtDr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvNEqual;
        private System.Windows.Forms.Label lblNot;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbVoucher;
        private System.Windows.Forms.Button btnGetData;
    }
}

