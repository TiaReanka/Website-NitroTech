using NitroTechWebsite;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Web.Http;

public class ChatbotController : ApiController
{
    private static ConcurrentDictionary<string, Dictionary<string, string>> _chatStates
        = new ConcurrentDictionary<string, Dictionary<string, string>>();

    [HttpPost]
    [Route("api/chatbot/respond")]
    public IHttpActionResult Respond([FromBody] ChatRequest request)
    {
        // Use a unique key per user; if session not available, fallback to temp ID
        string sessionKey = request.SessionId ?? "anonymous";

        // Get or create state dictionary
        var state = _chatStates.GetOrAdd(sessionKey, new Dictionary<string, string>());

        string reply = GetBotReply(request.Text, state);

        // Save state back
        _chatStates[sessionKey] = state;

        return Ok(new { reply });
    }

    private string GetBotReply(string userMessage, Dictionary<string, string> state)
    {
        userMessage = userMessage.Trim().ToLower();

        if (userMessage.Contains("navigate") || userMessage.Contains("find") || userMessage.Contains("locate") || userMessage.Contains("go to") || userMessage.Contains("navigation"))
        {
            if (userMessage.Contains("home")) return "Click on NitroTech to take you to the Home Page";
            if (userMessage.Contains("profile")) return "Click on the Profile Icon on the top right to view your username and role within the system.";

            // Quotations
            if (((userMessage.Contains("generate")) || ((userMessage.Contains("add"))) || (userMessage.Contains("create"))) && ((userMessage.Contains("quotation")) || (userMessage.Contains("quotation")))) return "Click on Quotations in the top menu, then select Generate Quotation from the dropdown.";
            if ((userMessage.Contains("active")) && ((userMessage.Contains("quotation")) || (userMessage.Contains("quotation")))) return "Click on Quotations in the top menu, then select Active Quotations from the dropdown.";
            if ((userMessage.Contains("add")) || (userMessage.Contains("job"))) return "Click on Quotations in the top menu, then select Add Job from the dropdown.";
            if ((userMessage.Contains("close")) || (userMessage.Contains("job"))) return "Click on Quotations in the top menu, then select Close Job from the dropdown.";
            if ((userMessage.Contains("quotations")) || (userMessage.Contains("quotation")))
            {
                state["expecting"] = "quotations";
                return "What would you like to see under Quotations?";
            }
            // Invoices
            if ((userMessage.Contains("invoices")) || (userMessage.Contains("invoice"))) return "Click on Invoices in the top menu to access invoices.";

            // Statements
            if (((userMessage.Contains("generate")) || ((userMessage.Contains("add"))) || (userMessage.Contains("create"))) && ((userMessage.Contains("statement")) || (userMessage.Contains("statements")))) return "Click on Statements in the top menu, then select Generate Statement from the dropdown.";
            if (((userMessage.Contains("review")) || (userMessage.Contains("view")) && ((userMessage.Contains("statement"))) || (userMessage.Contains("statements")))) return "Click on Statements in the top menu, then select Review Statements from the dropdown.";
            if ((userMessage.Contains("statements")) || (userMessage.Contains("statement")))
            {
                state["expecting"] = "statements";
                return "What would you like to see under Statements?";
            }

            // Customers
            if (((userMessage.Contains("update")) || (userMessage.Contains("change"))) && ((userMessage.Contains("details")) || (userMessage.Contains("information"))) && ((userMessage.Contains("customers")) || (userMessage.Contains("customer")))) return "Click on Customers in the top menu, then select Update Customer Details from the dropdown.";
            if ((userMessage.Contains("transfer")) && (((userMessage.Contains("vehicle"))) || ((userMessage.Contains("vehicles"))))) return "Click on Customers in the top menu, then select Transfer Vehicles from the dropdown.";
            if ((userMessage.Contains("customers")) || (userMessage.Contains("customer")))
            {
                state["expecting"] = "customers";
                return "What would you like to see under Customers? (Update Customer Details, Transfer Vehicles)";
            }

            // Reporting options
            if (((userMessage.Contains("parts")) || (userMessage.Contains("report"))) || ((userMessage.Contains("crystal")) || (userMessage.Contains("reports"))))
                return "Go to Review Parts Report and Find the Download button to view the Parts Report via Crystal Reports.";

            if (((userMessage.Contains("analysis")) || (userMessage.Contains("data")) || (userMessage.Contains("dashboard"))) ||((userMessage.Contains("powerbi")) || (userMessage.Contains("power bi"))))
                return "Click on PowerBI in the top menu to view the Analysis Dashboard.";

            if ((userMessage.Contains("reporting")) || (userMessage.Contains("report")))
            {
                state["expecting"] = "reporting";
                return "What would you like to see under Reporting? (Crystal Reports, Power BI Reports)";
            }


            // Payments
            if ((userMessage.Contains("payment")) || (userMessage.Contains("payments"))) return "Click on Payments in the top menu to access payment options.";

            // Parts
            if ((userMessage.Contains("add")) && ((userMessage.Contains("stock")) || (userMessage.Contains("part")))) return "Click on Parts in the top menu, then select Add Part from the dropdown.";
            if ((userMessage.Contains("update")) && ((userMessage.Contains("stock")) || (userMessage.Contains("part")))) return "Click on Parts in the top menu, then select Update Part Levels from the dropdown.";
            if (((userMessage.Contains("review")) || (userMessage.Contains("view")) || (userMessage.Contains("history"))) && ((userMessage.Contains("stock")) || (userMessage.Contains("part")))) return "Click on Parts in the top menu, then select Review Parts History from the dropdown.";
            if (userMessage.Contains("parts") || userMessage.Contains("stock"))
            {
                state["expecting"] = "parts";
                return "What would you like to see under Parts?";
            }

            // Account
            if (((userMessage.Contains("add")) || (userMessage.Contains("create"))) && ((userMessage.Contains("user")) || (userMessage.Contains("account")))) return "Click on Account in the top menu, then select Create an Account from the dropdown.";
            if ((userMessage.Contains("archive")) && ((userMessage.Contains("user")) || (userMessage.Contains("account")))) return "Click on Account in the top menu, then select Archive User from the dropdown.";
            if (((userMessage.Contains("reset")) || (userMessage.Contains("reset"))) && (userMessage.Contains("password"))) return "Click on Account in the top menu, then select Reset Password from the dropdown. You can also log out and reset it from the login page by pressing the Reset Password hyperlink at the bottom.";
            if ((userMessage.Contains("logout")) || (userMessage.Contains("log out")) || (userMessage.Contains("log off"))) return "Click on Account in the top menu, then select Logout from the dropdown.";
            if ((userMessage.Contains("account")) || (userMessage.Contains("user")))
            {
                state["expecting"] = "account";
                return "What would you like to see under Account? (Create an Account, Archive User, Reset Password, Logout)";
            }

            // About
            if (userMessage.Contains("about")) return "Click on About in the top menu to view more information about NitroTech.";

            // Feedback
            if ((userMessage.Contains("feedback")) || (userMessage.Contains("contact"))) return "Click on Feedback in the top menu to share your feedback to the developers.";

            // If no target found, ask follow-up
            state["expecting"] = "navigate";
            return "Sure, where would you like to go?";
        }

        //Explain

        if ((userMessage.Contains("explain")) || (userMessage.Contains("work?")))
        {
            if (((userMessage.Contains("generate")) || ((userMessage.Contains("add"))) || (userMessage.Contains("create"))) && ((userMessage.Contains("statement")) || (userMessage.Contains("statements"))))
            {
                return "Start generating a statement by selecting an existing customer’s ID. The statement is generated by first requiring the user to enter the ID of the customer. Once the button is clicked, A unique statement number is then generated. The totals from tables ‘Invoice’ and ‘Payment’ from the past month including the cost in table ‘Customer’ is used in the calculation to determine the amount that will reflect in the statement. A message will be displayed to confirm the statement was generated with the new statement number being displayed.\r\n";
            }
            if (((userMessage.Contains("review")) || (userMessage.Contains("view")) || (userMessage.Contains("history"))) && ((userMessage.Contains("stock")) || (userMessage.Contains("part"))))
            {
                return "To review an existing statement, enter a customers ID to display all statements that are associated with the customers ID. The statements will be displayed in grid view format below the button.";
            }
            if (((userMessage.Contains("reset")) || (userMessage.Contains("reset"))) && (userMessage.Contains("password")))
            {
                return "You will be required to login before gaining further access.  Enter your assigned username and password into the text boxes. Once you are sure that your credentials are correct, press the login button to continue. \r\nIf you wish to change your password, the “change password?” button is available located below the login button. Once press it will redirect to change your password.";
            }
            if ((userMessage.Contains("invoices")) || (userMessage.Contains("invoice")))
            {
                return "This tab allows the user to select a quotation from the drop down list(the options are all jobs that have been closed aka. Finished). They are prompted to confirm their selection and thereafter can generate an invoice based on the selected quotation. Once the invoice is generated, it adds the amount to what the customer owes.";
            }
            if ((userMessage.Contains("generate")) && (userMessage.Contains("statement")))
            {
                return "The Generate Statement page lets you produce a financial summary for a customer. The statement aggregates invoices, payments, and costs for the past month, generating a statement number and showing a confirmation message once complete.";
            }
            if ((userMessage.Contains("review")) && (userMessage.Contains("statement")))
            {
                return "The Review Statements page lets you enter a customer ID to view all statements associated with that customer. The statements are displayed in a grid format for easy reference.";
            }
            if (((userMessage.Contains("update")) || (userMessage.Contains("change"))) &&
    ((userMessage.Contains("details")) || (userMessage.Contains("information"))))
            {
                return "The Update Customer Details page allows you to modify a customer’s personal or contact information. Simply enter their ID, edit the details, and save the changes.";
            }
            if ((userMessage.Contains("transfer")) && (userMessage.Contains("vehicle")))
            {
                return "The Transfer Vehicles page lets you move a vehicle record from one customer to another. Enter both customer IDs and select the vehicle to complete the transfer.";
            }
            if ((userMessage.Contains("add")) && (userMessage.Contains("part")))
            {
                return "The Add Part page is used to add new parts into the system. You’ll enter the part number, name, description, and stock quantity before saving.";
            }
            if ((userMessage.Contains("update")) && (userMessage.Contains("part")))
            {
                return "The Update Part Levels page allows you to modify stock levels or other details for existing parts.";
            }
            if (((userMessage.Contains("review")) || (userMessage.Contains("history"))) && (userMessage.Contains("part")))
            {
                return "The Review Parts History page displays a list of parts used in quotations or jobs, along with historical data on quantities used and stock updates.";
            }
            if ((userMessage.Contains("payment")) || (userMessage.Contains("payments")))
            {
                return "The Payments page allows you to view, process, and record customer payments. Payments made here reduce the customer’s outstanding balance and are reflected in their statements.";
            }
            if ((userMessage.Contains("create")) && (userMessage.Contains("account")))
            {
                return "The Create Account page allows administrators to register new users. You’ll enter a username, password, and select a role before saving.";
            }
            if ((userMessage.Contains("archive")) && (userMessage.Contains("user")))
            {
                return "The Archive User page allows you to deactivate users who no longer need system access, while retaining their historical data.";
            }
            if ((userMessage.Contains("reset")) && (userMessage.Contains("password")))
            {
                return "The Reset Password page enables you to reset or change your login credentials. You can do this within your account settings or through the login screen via the 'Forgot Password' link.";
            }
            if ((userMessage.Contains("logout")) || (userMessage.Contains("log out")) || (userMessage.Contains("log off")))
            {
                return "Logging out ends your current session and returns you to the login page. It’s recommended to log out after each use for security.";
            }
            if (((userMessage.Contains("crystal")) || (userMessage.Contains("reports")) || (userMessage.Contains("parts"))))
            {
                return "The Crystal Reports page provides printable report, the Parts Report. You can view and export structured data summaries in PDF format for record-keeping.";
            }
            if ((userMessage.Contains("powerbi")) || (userMessage.Contains("power bi")) ||
                (userMessage.Contains("analysis")) || (userMessage.Contains("dashboard")) || (userMessage.Contains("data")))
            {
                return "The Power BI Reports page offers dashboards and analytics visualizations, helping you monitor key business metrics like quotation nuumbers over the past month, payments, and active jobs.";
            }
            if (userMessage.Contains("about"))
            {
                return "The About page contains information about NitroTech, its mission, and development team.";
            }
            if ((userMessage.Contains("feedback")) || (userMessage.Contains("contact")))
            {
                return "The Feedback page allows you to submit comments or issues directly to the NitroTech development team.";

            }
            if (userMessage.Contains("profile"))
            {
                return "The Profile icon shows your account details, including username and role, which dictate permissions within the NitroTech system.";
            }
            if (userMessage.Contains("home"))
            {
                return "The Home Page provides an overview of your current session and shortcuts to all major sections of the system.";
            }
            state["expecting"] = "explain";
            return "Sure, where would you like to know or learn about?";
        }

        if (userMessage.Contains("gay"))
        {
            return "Because you exist.";
        }


        if (userMessage.Contains("football"))
        {
            if (userMessage.Contains("best"))
            {
                if (userMessage.Contains("team"))
                {
                    return "Liverpool are the best team.";
                }
                if (userMessage.Contains("player"))
                {
                    return "Lionel Messi is the GOAT.";
                }
                state["expecting"] = "best";
                return "The best team or player?";
            }

            if (userMessage.Contains("worst"))
            {
                if (userMessage.Contains("team"))
                {
                    return "Manchester United are the worst team.";
                }
                state["expecting"] = "worst";
                return "The worst team?";
            }
            state["expecting"] = "football";
            return "What do you want to know about football?";
        }


        // Direct help commands in the same message
        if (userMessage.Contains("help") || userMessage.Contains("assistance"))
        {
            if (userMessage.Contains("login")) return "If you forgot your password, click 'Forgot Password' on the login page.";
            if (userMessage.Contains("account")) return "You can update your account in the Profile section.";
            if (userMessage.Contains("payment")) return "Payments are handled securely under the Billing section.";

            // If no target found, ask follow-up
            state["expecting"] = "help";
            return "What do you need help with?";
        }

        // Handle expecting follow-ups (if user didn't type full command)
        if (state.ContainsKey("expecting"))
        {
            string expecting = state["expecting"];
            state.Remove("expecting");

            if (expecting == "navigate")
            {
                if (userMessage.Contains("home")) return "Click on NitroTech to take you to the Home Page";
                if (userMessage.Contains("profile")) return "Click on the Profile Icon on the top right to view your username and role within the system.";
                if ((userMessage.Contains("invoices")) || (userMessage.Contains("invoice"))) return "Click on Invoices in the top menu to access invoices.";
                if ((userMessage.Contains("payment")) || (userMessage.Contains("payments"))) return "Click on Payments in the top menu to access payment options.";
                if (userMessage.Contains("about")) return "Click on About in the top menu to view more information about NitroTech.";
                if (userMessage.Contains("feedback")) return "Click on Feedback in the top menu to share your feedback to the developers.";

                // Dropdown menus — redirect state
                if (userMessage.Contains("quotations") || userMessage.Contains("quotation"))
                {
                    state["expecting"] = "quotations";
                    return "What would you like to see under Quotations? (Generate Quotation, Active Quotations, Add Job, Close Job)";
                }
                if (userMessage.Contains("statements") || userMessage.Contains("statement"))
                {
                    state["expecting"] = "statements";
                    return "What would you like to see under Statements? (Generate Statement, Review Statements)";
                }
                if (userMessage.Contains("customers") || userMessage.Contains("customer"))
                {
                    state["expecting"] = "customers";
                    return "What would you like to see under Customers? (Update Customer Details, Transfer Vehicles)";
                }
                if (userMessage.Contains("parts") || userMessage.Contains("part"))
                {
                    state["expecting"] = "parts";
                    return "What would you like to see under Parts? (Add Part, Update Part Levels, Review Parts History)";
                }
                if ((userMessage.Contains("account")) || userMessage.Contains("accounts"))
                {
                    state["expecting"] = "account";
                    return "What would you like to see under Account? (Create an Account, Archive User, Reset Password, Logout)";
                }
                if ((userMessage.Contains("reporting")) || (userMessage.Contains("report")))
                {
                    state["expecting"] = "reporting";
                    return "What would you like to see under Reporting? (Crystal Reports, PowerBI)";
                }

                return "I didn’t recognize that page. Try one of our services instead!";
            }
            else if (expecting == "reporting")
            {
                // Crystal Reports options
                if ((userMessage.Contains("crystal")) ||
                    (userMessage.Contains("reports")) || (userMessage.Contains("parts")))
                    return "Click on Review Parts Report in the top menu, then select Download Report to view the Parts Report via Crystal Reports.";

                // Power BI options
                if ((userMessage.Contains("powerbi")) || (userMessage.Contains("power bi")) ||
                    (userMessage.Contains("analysis")) || (userMessage.Contains("dashboard")) || (userMessage.Contains("data")))
                    return "Click on Power BI in the top menu to view the Analysis Dashboard.";

                // Default fallback if the user didn’t specify which one
                return "Please choose what you'd like to see under Reporting (Crystal Reports or Power BI Reports).";
            }

            else if (expecting == "quotations")
            {
                if ((userMessage.Contains("generate")) || ((userMessage.Contains("add"))) || (userMessage.Contains("create"))) return "Click on Quotations in the top menu, then select Generate Quotation from the dropdown.";
                if (userMessage.Contains("active")) return "Click on Quotations in the top menu, then select Active Quotations from the dropdown.";
                if (userMessage.Contains("add") || userMessage.Contains("job")) return "Click on Quotations in the top menu, then select Add Job from the dropdown.";
                if (userMessage.Contains("close") || userMessage.Contains("job")) return "Click on Quotations in the top menu, then select Close Job from the dropdown.";
                return "Please pick one of our services (Generate, Active, Add, Close).";
            }
            else if (expecting == "parts")
            {
                if (userMessage.Contains("add")) return "Click on Parts in the top menu, then select Add Part from the dropdown.";
                if (userMessage.Contains("update")) return "Click on Parts in the top menu, then select Update Part Levels from the dropdown.";
                if (userMessage.Contains("review") || userMessage.Contains("view") || userMessage.Contains("history")) return "Click on Parts in the top menu, then select Review Parts History from the dropdown.";
                return "Please select one of our services (Add, Update, Review)";
            }
            else if (expecting == "contact")
            {
                if (userMessage.Contains("support")) return "You can reach support at support@yourdomain.com";
                if (userMessage.Contains("sales")) return "Sales team: sales@yourdomain.com";
                if (userMessage.Contains("office")) return "Our office is at 123 Main Street, City.";
                return "Please specify 'Support', 'Sales', or 'Office'.";
            }
            else if (expecting == "statements")
            {
                if ((userMessage.Contains("generate")) || ((userMessage.Contains("add"))) || (userMessage.Contains("create"))) return "Click on Statements in the top menu, then select Generate Statement from the dropdown.";
                if (userMessage.Contains("review") || userMessage.Contains("view")) return "Click on Statements in the top menu, then select Review Statements from the dropdown.";
                return "Please pick one of our services.";
            }
            else if (expecting == "customers")
            {
                if (((userMessage.Contains("update")) || ((userMessage.Contains("change")))) && ((userMessage.Contains("details")) || (userMessage.Contains("information")))) return "Click on Customers in the top menu, then select Update Customer Details from the dropdown.";
                if (userMessage.Contains("transfer") && userMessage.Contains("vehicle")) return "Click on Customers in the top menu, then select Transfer Vehicles from the dropdown.";
                return "Please pick one of our services.";
            }
            else if (expecting == "help")
            {
                if (userMessage.Contains("login")) return "If you forgot your password, click 'Forgot Password' on the login page.";
                if (userMessage.Contains("account")) return "You can update your account in the Profile section.";
                if (userMessage.Contains("payment")) return "Payments are handled securely under the Billing section.";
                return "Help topics: 'Login', 'Account', or 'Payment'.";
            }
            else if (expecting == "account")
            {
                if (userMessage.Contains("create") || userMessage.Contains("add")) return "Click on Account in the top menu, then select Create an Account from the dropdown.";
                if (userMessage.Contains("archive")) return "Click on Account in the top menu, then select Archive User from the dropdown.";
                if ((userMessage.Contains("reset")) || (userMessage.Contains("change")) && userMessage.Contains("password")) return "Click on Account in the top menu, then select Reset Password from the dropdown. You can also log out and reset it from the login page by pressing the Reset Password hyperlink at the bottom.";
                if (userMessage.Contains("logout") || userMessage.Contains("log out") || userMessage.Contains("log off")) return "Click on Account in the top menu, then select Logout from the dropdown.";
                return "What would you like to see under Account? (Create an Account, Archive User, Reset Password, Logout)";
            }
            else if (expecting == "football")
            {
                if (userMessage.Contains("best"))
                {
                    if (userMessage.Contains("team"))
                    {
                        return "Liverpool are the best team.";
                    }
                    if (userMessage.Contains("player"))
                    {
                        return "Lionel Messi is the GOAT.";
                    }
                    state["expecting"] = "best";
                    return "The best team or player?";
                }
                return "What do you want to know about football?";
            }
            else if (expecting == "best")
            {
                if (userMessage.Contains("team"))
                {
                    return "Liverpool are the best team.";
                }
                if (userMessage.Contains("player"))
                {
                    return "Lionel Messi is the GOAT.";
                }
                return "The best team or player?";
            }
            else if (expecting == "worst")
            {
                if (userMessage.Contains("team"))
                {
                    return "Manchester United are the worst team.";
                }
                
                return "The worst team?";
            }

            else if (expecting == "explain")
            {
                if (((userMessage.Contains("generate")) || ((userMessage.Contains("add"))) || (userMessage.Contains("create"))) && ((userMessage.Contains("statement")) || (userMessage.Contains("statements"))))
                {
                    return "Start generating a statement by selecting an existing customer’s ID. The statement is generated by first requiring the user to enter the ID of the customer. Once the button is clicked, A unique statement number is then generated. The totals from tables ‘Invoice’ and ‘Payment’ from the past month including the cost in table ‘Customer’ is used in the calculation to determine the amount that will reflect in the statement. A message will be displayed to confirm the statement was generated with the new statement number being displayed.\r\n";
                }
                if (((userMessage.Contains("review")) || (userMessage.Contains("view")) || (userMessage.Contains("history"))) && ((userMessage.Contains("stock")) || (userMessage.Contains("part"))))
                {
                    return "To review an existing statement, enter a customers ID to display all statements that are associated with the customers ID. The statements will be displayed in grid view format below the button.";
                }
                if (((userMessage.Contains("reset")) || (userMessage.Contains("reset"))) && (userMessage.Contains("password")))
                {
                    return "You will be required to login before gaining further access.  Enter your assigned username and password into the text boxes. Once you are sure that your credentials are correct, press the login button to continue. \r\nIf you wish to change your password, the “change password?” button is available located below the login button. Once press it will redirect to change your password.";
                }
                if ((userMessage.Contains("invoices")) || (userMessage.Contains("invoice")))
                {
                    return "This tab allows the user to select a quotation from the drop down list(the options are all jobs that have been closed aka. Finished). They are prompted to confirm their selection and thereafter can generate an invoice based on the selected quotation. Once the invoice is generated, it adds the amount to what the customer owes.";
                }
                if ((userMessage.Contains("generate")) && (userMessage.Contains("statement")))
                {
                    return "The Generate Statement page lets you produce a financial summary for a customer. The statement aggregates invoices, payments, and costs for the past month, generating a statement number and showing a confirmation message once complete.";
                }
                if ((userMessage.Contains("review")) && (userMessage.Contains("statement")))
                {
                    return "The Review Statements page lets you enter a customer ID to view all statements associated with that customer. The statements are displayed in a grid format for easy reference.";
                }
                if (((userMessage.Contains("update")) || (userMessage.Contains("change"))) &&
        ((userMessage.Contains("details")) || (userMessage.Contains("information"))))
                {
                    return "The Update Customer Details page allows you to modify a customer’s personal or contact information. Simply enter their ID, edit the details, and save the changes.";
                }
                if ((userMessage.Contains("transfer")) && (userMessage.Contains("vehicle")))
                {
                    return "The Transfer Vehicles page lets you move a vehicle record from one customer to another. Enter both customer IDs and select the vehicle to complete the transfer.";
                }
                if ((userMessage.Contains("add")) && (userMessage.Contains("part")))
                {
                    return "The Add Part page is used to add new parts into the system. You’ll enter the part number, name, description, and stock quantity before saving.";
                }
                if ((userMessage.Contains("update")) && (userMessage.Contains("part")))
                {
                    return "The Update Part Levels page allows you to modify stock levels or other details for existing parts.";
                }
                if (((userMessage.Contains("review")) || (userMessage.Contains("history"))) && (userMessage.Contains("part")))
                {
                    return "The Review Parts History page displays a list of parts used in quotations or jobs, along with historical data on quantities used and stock updates.";
                }
                if ((userMessage.Contains("payment")) || (userMessage.Contains("payments")))
                {
                    return "The Payments page allows you to view, process, and record customer payments. Payments made here reduce the customer’s outstanding balance and are reflected in their statements.";
                }
                if ((userMessage.Contains("create")) && (userMessage.Contains("account")))
                {
                    return "The Create Account page allows administrators to register new users. You’ll enter a username, password, and select a role before saving.";
                }
                if ((userMessage.Contains("archive")) && (userMessage.Contains("user")))
                {
                    return "The Archive User page allows you to deactivate users who no longer need system access, while retaining their historical data.";
                }
                if ((userMessage.Contains("reset")) && (userMessage.Contains("password")))
                {
                    return "The Reset Password page enables you to reset or change your login credentials. You can do this within your account settings or through the login screen via the 'Forgot Password' link.";
                }
                if ((userMessage.Contains("logout")) || (userMessage.Contains("log out")) || (userMessage.Contains("log off")))
                {
                    return "Logging out ends your current session and returns you to the login page. It’s recommended to log out after each use for security.";
                }
                if (((userMessage.Contains("crystal")) || (userMessage.Contains("reports")) || (userMessage.Contains("parts"))))
                {
                    return "The Crystal Reports page provides printable report, the Parts Report. You can view and export structured data summaries in PDF format for record-keeping.";
                }
                if ((userMessage.Contains("powerbi")) || (userMessage.Contains("power bi")) ||
                    (userMessage.Contains("analysis")) || (userMessage.Contains("dashboard")) || (userMessage.Contains("data")))
                {
                    return "The Power BI Reports page offers dashboards and analytics visualizations, helping you monitor key business metrics like quotation nuumbers over the past month, payments, and active jobs.";
                }
                if (userMessage.Contains("about"))
                {
                    return "The About page contains information about NitroTech, its mission, and development team.";
                }
                if ((userMessage.Contains("feedback")) || (userMessage.Contains("contact")))
                {
                    return "The Feedback page allows you to submit comments or issues directly to the NitroTech development team.";

                }
                if (userMessage.Contains("profile"))
                {
                    return "The Profile icon shows your account details, including username and role, which dictate permissions within the NitroTech system.";
                }
                if (userMessage.Contains("home"))
                {
                    return "The Home Page provides an overview of your current session and shortcuts to all major sections of the system.";
                }
                return "Please specify what you want to know or learn about.";
            }

            

        }

        // Custom phrases
        if (userMessage.Contains("hello")) return "Hello, I am Plumeria, your virtual assistant. How may I be of service today?";

        // Default fallback
        return "I can help with: Navigate, Contact, or Help.";
    }


    public class ChatRequest
    {
        public string Text { get; set; }
        public string SessionId { get; set; } // send a unique ID from the client (like session or user ID)
    }
}