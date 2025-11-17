<%@ Page Title="About JAE Automotive Industries" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="NitroTechWebsite.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        body {
            margin: 0;
            background: linear-gradient(to right, purple, #1a2db9);
            color: white;
            font-family: Arial, sans-serif;
        }

        .about-section {
            background: linear-gradient(to right, purple, #1a2db9);
            padding: 20px;
            border-radius: 10px;
            color: white;
        }

        h1, h2 {
            color: #f0f0f0;
        }

        h2 {
            border-bottom: 2px solid rgba(255, 255, 255, 0.5);
            padding-bottom: 10px;
            margin-top: 20px;
        }

        h3 {
            color: #e0e0e0;
        }

        footer {
            margin-top: 30px;
            text-align: center;
            font-size: 0.9em;
            color: #ddd;
        }

        /* New styles for location section */
        .location-section {
            margin-top: 30px;
            text-align: center;
        }

            .location-section p {
                font-size: 16px;
                color: #fff;
            }

        .map-container {
            margin-top: 15px;
            display: flex;
            justify-content: center;
        }

        iframe {
            border: 0;
            width: 80%;
            height: 400px;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(0,0,0,0.3);
        }
    </style>

    <div class="about-section">
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
            <p>
                <strong>Mission:</strong>
                <asp:Literal ID="litMission" runat="server"
                    Text="To deliver reliable automotive engineering solutions through advanced systems, skilled expertise, and a strong culture of accountability."></asp:Literal>
            </p>
            <p>
                <strong>Vision:</strong>
                <asp:Literal ID="litVision" runat="server"
                    Text="To be recognized as a trusted automotive engineering partner, known for innovation, technical excellence, and sustainable business practices."></asp:Literal>
            </p>
            <p>
                <strong>Core Values:</strong>
                <asp:Literal ID="litCoreValues" runat="server"
                    Text="Quality, Integrity, Safety, Innovation, and Collaboration."></asp:Literal>
            </p>
        </section>

        <section id="operations-partnerships">
            <h2>Operations & Partnerships</h2>
            <asp:Literal ID="litOperations" runat="server"
                Text="We work in collaboration with trusted suppliers, system providers, and industry partners to maintain efficiency and uphold service standards. Our infrastructure includes advanced automotive diagnostic systems, inventory management, and secure document handling to ensure transparency and reliability in all projects."></asp:Literal>
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

        <section class="location-section" id="location">
            <h2>Our Location</h2>
            <p>
                <strong>Address:</strong><br />
                No. 6 Ashfield Avenue, Unit 1<br />
                Springfield Business Park, 4034<br />
                South Africa
            </p>

            <div class="map-container">
                <iframe
                    src="https://www.google.com/maps?q=No.6+Ashfield+Avenue,+Unit+1,+Springfield+Business+Park,+4034,+South+Africa&output=embed"
                    allowfullscreen=""
                    loading="lazy"
                    referrerpolicy="no-referrer-when-downgrade"></iframe>
            </div><br />
            
            <p>
                <strong>Contact:</strong><br />
                Telephone number:  031 577 9590
            </p>
        </section>

        <footer>
            <p>&copy; 2025 JAE Automotive Industries. All rights reserved.</p>
        </footer>
    </div>
</asp:Content>
