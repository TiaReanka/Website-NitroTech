using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.SessionState;
public static class DatabaseHelper
{
    public static string GetConnectionString()
    {
        var cs = ConfigurationManager.ConnectionStrings["WstGrp4"]?.ConnectionString; 
        if (string.IsNullOrWhiteSpace(cs))
            throw new InvalidOperationException("❌ Missing connection string in Web.config.");
        return cs;
    }
    public static SqlConnection OpenConnection()
    {
        var conn = new SqlConnection(GetConnectionString());
        conn.Open();
        return conn;
    }

    public static object ExecuteScalar(string query, SqlParameter[] parameters = null)
    {
        using (var conn = OpenConnection())
        using (var cmd = new SqlCommand(query, conn))
        {
            if (parameters != null)
                cmd.Parameters.AddRange(parameters);

            return cmd.ExecuteScalar();
        }
    }

    public static object ExecuteScalar(string query, SqlConnection conn, SqlTransaction tran, SqlParameter[] parameters = null)
    {
        using (var cmd = new SqlCommand(query, conn, tran))
        {
            if (parameters != null)
                cmd.Parameters.AddRange(parameters);

            return cmd.ExecuteScalar();
        }
    }
}

public static class LoginUtility
{
    public static bool ValidateUser(string username, string password, HttpSessionState session)
    {
        using (var conn = DatabaseHelper.OpenConnection())
        using (var cmd = new SqlCommand(@"
            SELECT userID, Username, userRole 
            FROM tblUsers
            WHERE Username=@Username 
              AND userPassword=@Password 
              AND userActiveStatus=1;", conn))
        {
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Password", password);

            using (var reader = cmd.ExecuteReader())
            {
                if (!reader.Read()) return false;

                session["UserType"] = "SystemUser";
                session["UserId"] = reader["userID"];
                session["Name"] = reader["Username"];
                session["Role"] = reader["userRole"];
                return true;
            }
        }
    }

    public static void EnsureLoggedIn(System.Web.UI.Page page, params string[] allowedRoles)
    {
        var session = page.Session;

        // 1️⃣ Check if user is logged in
        if (session == null || session["UserId"] == null)
        {
            page.Response.Redirect("Account.aspx");
            return;
        }

        // 2️⃣ Check if role restrictions apply
        if (allowedRoles != null && allowedRoles.Length > 0)
        {
            var role = (session["Role"] ?? "").ToString();
            bool roleAllowed = false;

            foreach (var r in allowedRoles)
            {
                if (string.Equals(r, role, System.StringComparison.OrdinalIgnoreCase))
                {
                    roleAllowed = true;
                    break;
                }
            }

            if (!roleAllowed)
            {
                page.Response.Redirect("Unauthorized.aspx");
                return;
            }
        }

        // If logged in and role (if required) matches, user continues to page
    }
}


public static class TransactionHelper
{
    public static DataTable GetInvoicesLastMonth(string customerId)
    {
        const string sql = @"
            SELECT invoiceNumber, invoiceDate, customerID,
                   quotationNumber, invoiceAmountDue, vehicleVIN
            FROM tblInvoice
            WHERE customerID = @CustomerID
              AND invoiceDate >= DATEADD(MONTH,-1,GETDATE());";

        using (var conn = DatabaseHelper.OpenConnection())
        using (var cmd = new SqlCommand(sql, conn))
        {
            cmd.Parameters.AddWithValue("@CustomerID", customerId);
            using (var da = new SqlDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }

    public static DataTable GetPaymentsLastMonth(string customerId)
    {
        const string sql = @"
            SELECT paymentID, paidAmount, dateOfPayment,
                   customerID, amountDue
            FROM tblPayment
            WHERE customerID = @CustomerID
              AND dateOfPayment >= DATEADD(MONTH,-1,GETDATE());";

        using (var conn = DatabaseHelper.OpenConnection())
        using (var cmd = new SqlCommand(sql, conn))
        {
            cmd.Parameters.AddWithValue("@CustomerID", customerId);
            using (var da = new SqlDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }
}
