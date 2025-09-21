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

        if (userMessage.Contains("navigate") || userMessage.Contains("find") || userMessage.Contains("locate") || userMessage.Contains("go to"))
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
            if ((userMessage.Contains("transfer")) && (((userMessage.Contains("vehicle"))) || ((userMessage.Contains("vehicle"))))) return "Click on Customers in the top menu, then select Transfer Vehicles from the dropdown.";
            if ((userMessage.Contains("customers")) || (userMessage.Contains("customer")))
            {
                state["expecting"] = "customers";
                return "What would you like to see under Customers? (Update Customer Details, Transfer Vehicles)";
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
            if (userMessage.Contains("feedback")) return "Click on Feedback in the top menu to share your feedback to the developers.";

            // If no target found, ask follow-up
            state["expecting"] = "navigate";
            return "Sure, where would you like to go?";
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

        // Direct contact commands in the same message
        if (userMessage.Contains("contact"))
        {
            if (userMessage.Contains("support")) return "You can reach support at support@pain.com";
            if (userMessage.Contains("sales")) return "Sales team: sales@pain.com";
            if (userMessage.Contains("office")) return "Our office is at 123 Calvin is tired Street, City.";

            // If no target found, ask follow-up
            state["expecting"] = "contact";
            return "Who would you like to contact? (Support / Sales / Office)";
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

                return "I didn’t recognize that page. Try one of our services instead!";
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
        }

        // Custom phrases
        if (userMessage.Contains("hello")) return "Wk stekkie. What you chuning?";

        // Default fallback
        return "I can help with: Navigate, Contact, or Help.";
    }


    public class ChatRequest
    {
        public string Text { get; set; }
        public string SessionId { get; set; } // send a unique ID from the client (like session or user ID)
    }
}