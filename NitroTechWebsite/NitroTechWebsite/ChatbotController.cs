using System.Runtime.InteropServices;
using System.Web.Http;

public class ChatbotController : ApiController
{
    [HttpPost]
    [Route("api/chatbot/respond")]
    public IHttpActionResult Respond([FromBody] ChatRequest request)
    {
        string reply = GetBotReply(request.Text);
        return Ok(new { reply });
    }

    private string GetBotReply(string userMessage)
    {
        userMessage = userMessage.ToLower();

        // Custom phrases
        if (userMessage.Contains("hello")) return "Wk stekkie. What you chuning?";
        if (userMessage.Contains("navigate home")) return "Click 'Home' in the top menu to return to the homepage.";
        if (userMessage.Contains("contact")) return "You can reach support via the Contact page in the navigation bar.";
        if (userMessage.Contains("help")) return "I can help you with navigation, support info, or FAQs.";

        // Default response
        return "Sorry, I didn’t understand that. Try asking about 'help', 'contact', or 'navigate'.";
    }

    public class ChatRequest
    {
        public string Text { get; set; }
    }
}