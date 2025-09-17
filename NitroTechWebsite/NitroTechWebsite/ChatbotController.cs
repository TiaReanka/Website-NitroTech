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

        // Direct navigation commands in the same message
        if (userMessage.Contains("navigate") || userMessage.Contains("find") || userMessage.Contains("locate") || userMessage.Contains("go to"))
        {
            if (userMessage.Contains("home")) return "Taking you to the Home page: /Home";
            if (userMessage.Contains("dashboard")) return "Opening your Dashboard: /Dashboard";
            if (userMessage.Contains("profile")) return "Navigating to Profile: /Profile";

            // If no target found, ask follow-up
            state["expecting"] = "navigate";
            return "Sure, where would you like to go? (Home / Dashboard / Profile)";
        }

        // Direct contact commands in the same message
        if (userMessage.Contains("contact"))
        {
            if (userMessage.Contains("support")) return "You can reach support at support@yourdomain.com";
            if (userMessage.Contains("sales")) return "Sales team: sales@yourdomain.com";
            if (userMessage.Contains("office")) return "Our office is at 123 Main Street, City.";

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
                if (userMessage.Contains("home")) return "Taking you to the Home page: /Home";
                if (userMessage.Contains("dashboard")) return "Opening your Dashboard: /Dashboard";
                if (userMessage.Contains("profile")) return "Navigating to Profile: /Profile";
                return "I didn’t recognize that page. Try one of our services instead!";
            }
            else if (expecting == "contact")
            {
                if (userMessage.Contains("support")) return "You can reach support at support@yourdomain.com";
                if (userMessage.Contains("sales")) return "Sales team: sales@yourdomain.com";
                if (userMessage.Contains("office")) return "Our office is at 123 Main Street, City.";
                return "Please specify 'Support', 'Sales', or 'Office'.";
            }
            else if (expecting == "help")
            {
                if (userMessage.Contains("login")) return "If you forgot your password, click 'Forgot Password' on the login page.";
                if (userMessage.Contains("account")) return "You can update your account in the Profile section.";
                if (userMessage.Contains("payment")) return "Payments are handled securely under the Billing section.";
                return "Help topics: 'Login', 'Account', or 'Payment'.";
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