using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();

            string queryString =
                "SELECT * FROM Sellers";
            SqlDataAdapter adapter = new SqlDataAdapter(queryString, "Data Source=.\\SQLEXPRESS;Initial Catalog=WindowsFormsApplication.MyDbContext;Integrated Security=True");
            adapter.Fill(_WindowsFormsApplication_MyDbContextDataSet, "Sellers");
            
            queryString =
                "SELECT * FROM dbo.Sales";
            adapter = new SqlDataAdapter(queryString, "Data Source=.\\SQLEXPRESS;Initial Catalog=WindowsFormsApplication.MyDbContext;Integrated Security=True");
            //_WindowsFormsApplication_MyDbContextDataSet.EnforceConstraints = false;
            adapter.Fill(_WindowsFormsApplication_MyDbContextDataSet, "Sales");

            // Create Relation
            _WindowsFormsApplication_MyDbContextDataSet.Relations.Add("CustSelesRelation",
                _WindowsFormsApplication_MyDbContextDataSet.Tables["Sellers"].Columns["SellerId"],
                _WindowsFormsApplication_MyDbContextDataSet.Tables["Sales"].Columns["SellerId"]);



            BindingSource bsSellers = new BindingSource();
            bsSellers.DataSource = _WindowsFormsApplication_MyDbContextDataSet;
            //Console.WriteLine(_WindowsFormsApplication_MyDbContextDataSet.Tables["Sellers"].Columns["SellerId"]);
            bsSellers.DataMember = "Sellers";

            BindingSource bsSales = new BindingSource();
            bsSales.DataSource = bsSellers;
            bsSales.DataMember = "CustSelesRelation";

            // Bind Data to DataGridViews 
            dgvCustomer.DataSource = bsSellers;
            dgvOrder.DataSource = bsSales;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "DataSet2.DataTable1". При необходимости она может быть перемещена или удалена.
            this.DataTable1TableAdapter.Fill(this.DataSet2.DataTable1);
            Database.SetInitializer(
                new DropCreateDatabaseIfModelChanges<MyDbContext>());


            this.reportViewer1.RefreshReport();
        }
    }
}
