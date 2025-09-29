<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="YourNamespace.About" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>About Us - JAE Automotive Industries</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>

        <header>
            <h1>About Us – JAE Automotive Industries</h1>
        </header>

        <section id="company-overview">
            <h2>Company Overview</h2>
            <asp:Literal ID="litCompanyOverview" runat="server" 
                Text="JAE Automotive Industries was established to provide professional automotive engineering and technical solutions within the motor industry. Our operations focus on servicing, repair, and precision engineering for a wide range of vehicles. We are a registered and compliant business entity under South African business regulations and operate with a commitment to industry best practices."></asp:Literal>
        </section>

        <section id="mission-vision">
            <h2>Mission & Vision</h2>
            <p><strong>Mission:</strong> <asp:Literal ID="litMission" runat="server" Text="To deliver reliable automotive engineering solutions through advanced systems, skilled expertise, and a strong culture of accountability."></asp:Literal></p>
            <p><strong>Vision:</strong> <asp:Literal ID="litVision" runat="server" Text="To be recognized as a trusted automotive engineering partner, known for innovation, technical excellence, and sustainable business practices."></asp:Literal></p>
            <p><strong>Core Values:</strong> <asp:Literal ID="litCoreValues" runat="server" Text="Quality, Integrity, Safety, Innovation, and Collaboration."></asp:Literal></p>
        </section>


        <section id="operations-partnerships">
            <h2>Operations & Partnerships</h2>
            <asp:Literal ID="litOperations" runat="server" Text="We work in collaboration with trusted suppliers, system providers, and industry partners to maintain efficiency and uphold service standards. Our infrastructure includes advanced automotive diagnostic systems, inventory management, and secure document handling to ensure transparency and reliability in all projects."></asp:Literal>
        </section>

        <section id="achievements-milestones">
            <h2>Achievements & Milestones</h2>
            <asp:BulletedList ID="blAchievements" runat="server" DisplayMode="Text">
                <asp:ListItem>Successfully implemented structured automotive service and repair processes.</asp:ListItem>
                <asp:ListItem>Established partnerships with key suppliers and stakeholders in the automotive industry.</asp:ListItem>
            </asp:BulletedList>
        </section>

        <section id="corporate-responsibility">
            <h2>Corporate Responsibility</h2>
            <asp:BulletedList ID="blCSR" runat="server" DisplayMode="Text">
                <asp:ListItem>Promoting environmentally conscious workshop practices.</asp:ListItem>
                <asp:ListItem>Supporting skills development and training within the automotive sector.</asp:ListItem>
                <asp:ListItem>Engaging with the community through initiatives and partnerships.</asp:ListItem>
            </asp:BulletedList>
        </section>

        <footer>
            <p>&copy; 2025 JAE Automotive Industries. All rights reserved.</p>
        </footer>
    </form>
</body>
</html>

