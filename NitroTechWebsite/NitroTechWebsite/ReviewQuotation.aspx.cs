using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NitroTechWebsite
{
    public partial class AddJob : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadQuotations();
                LoadDummyJobs();
            }
        }

        private void LoadQuotations()
        {
            ddlQuotations.Items.Clear();
            ddlQuotations.Items.Add(new System.Web.UI.WebControls.ListItem("Select a quotation...", "")); 
            ddlQuotations.Items.Add(new System.Web.UI.WebControls.ListItem("Quotation #1001","1001"));
            ddlQuotations.Items.Add(new System.Web.UI.WebControls.ListItem("Quotation #1002","1002"));
            ddlQuotations.Items.Add(new System.Web.UI.WebControls.ListItem("Quotation #1003","1003"));
        }

        private void LoadDummyJobs()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("JobID");
            dt.Columns.Add("QuotationID");
            dt.Columns.Add("JobStatus");

            dt.Rows.Add("J001", "1001", "In Progress");
            dt.Rows.Add("J002", "1002", "Pending");
            dt.Rows.Add("J003", "1003", "Completed");

            gvJobs.DataSource = dt;
            gvJobs.DataBind();
        }

        protected void btnAddJob_Click(object sender, EventArgs e)
        {
            string selectedQuotation = ddlQuotations.SelectedValue;

            if (!string.IsNullOrEmpty(selectedQuotation))
            {
                // TODO: Add to database instead of dummy reload 
                LoadDummyJobs();
                Response.Write($"<script>alert('Job added for Quotation { selectedQuotation}');</script>"); 
            }
            else
            {
                Response.Write("<script>alert('Please select a quotation first.');</script>");
            }
        }
    }
}