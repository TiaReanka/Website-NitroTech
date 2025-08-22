<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" 
AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NitroTechWebsite._Default" 
%> 
 
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server"> 
 
    <!-- Hidden audio --> 
    <audio id="introAudio"> 
        <source src='<%= ResolveUrl("~/Audio/Ferrari V12 sound start up.mp3") %>' 
type="audio/mpeg" /> 
        Your browser does not support the audio element. 
    </audio> 
 
    <style> 
        body, form { 
            margin: 0; 
            padding: 0; 
        } 
 
        .divider { 
            border: 0; 
            height: 2px;           
            background-color: white; 
            margin: 0 auto 30px auto;  
            width: 80%;            
        } 
 
        @keyframes fadeInUp { 
            0% { 
                opacity: 0; 
                transform: translateY(50px); 
            } 
            100% { 
                opacity: 1; 
                transform: translateY(0); 
            } 
        } 
 
        .fade-box { 
            border: 2px solid white;  
            border-radius: 20px;  
            padding: 30px;  
            text-align: center;  
            margin: 65px 65px 150px 45px;  
            max-width: 80%;  
            background-color: rgba(255,255,255,0.1); 
            box-shadow: 4px 4px 15px rgba(0,0,0,0.3),  
                        inset -2px -2px 5px rgba(255,255,255,0.2),  
                        inset 2px 2px 5px rgba(0,0,0,0.2); 
 
            opacity: 0; 
            animation: fadeInUp 1s ease-out forwards;  
            animation-delay: 0.3s; 
            cursor: pointer; 
            transition: transform 0.2s ease; 
        } 
 
        .fade-on-scroll { 
            opacity: 0; 
            transform: translateY(50px); 
            transition: opacity 1s ease-out, transform 1s ease-out; 
        } 
 
        .fade-on-scroll.visible { 
            opacity: 1; 
            transform: translateY(0); 
        } 
    </style> 
 
    <!-- Top half: Left and right sections --> 
    <div style="display: flex; height: 100vh;"> 
        <!-- Left half: Welcome Text --> 
        <div style="flex: 1; display: flex; justify-content: center; align-items: flex-start; position: 
relative;"> 
            <div class="fade-box" id="clickableBox"> 
                <h1 style="margin: 0; font-size: 100px;">Welcome To</h1> 
                <h1 style="margin: 0; font-size: 110px;">NitroTech</h1> 
                <img src="~/Images/logo.jpg" alt="Logo" style="margin-top: 30px; max-width: 80%; 
height: auto;" runat="server" /> 
            </div> 
        </div> 
 
 
 
 
        <!-- Logging in aspect on the right side --> 
        <div style="flex: 1;"> 
            <!-- Future content --> 
        </div> 
    </div> 
 
    <!-- Horizontal white line separator --> 
    <hr class="divider" /> 
 
    <!-- Image under the line --> 
    <div style="display: flex; justify-content: center; margin-bottom: 30px;" class="fade-on
scroll"> 
        <img src="~/Images/banner.logo" alt="Banner" style="max-width: 70%; height: auto;" 
runat="server" /> 
    </div> 
 
    <!-- Bottom half: Paragraph --> 
    <div style="display: flex; justify-content: center; align-items: flex-start; padding-top: 20px; 
text-align: center;"> 
        <div style="max-width: 80%;" class="fade-on-scroll"> 
            <p style="font-size: 20px; color: white; margin-top: 20px;"> 
               At JAE Auto, we're building the future of vehicle care, one expertly serviced car at a time. 
Our process is a testament to our dedication: we take the time to meticulously diagnose every 
fault, ensuring a precise and lasting repair. We're not just fixing cars; we're restoring confidence 
and performance. Our commitment to excellence extends to every part of our business, which 
is why we're moving forward with a new, highly-efficient system. This upgrade allows us to serve 
you with unmatched speed and accuracy, ensuring that when you drive away, you're not just 
satisfied, you're thrilled. We're proud of the work we do and excited to welcome you into the JAE 
Auto family. 
            </p> 
        </div> 
    </div> 
 
    <script> 
        // Fade-in paragraph on scroll 
        document.addEventListener("DOMContentLoaded", () => { 
            const elements = document.querySelectorAll('.fade-on-scroll'); 
            const observer = new IntersectionObserver((entries, obs) => { 
                entries.forEach(entry => { 
                    if (entry.isIntersecting) { 
                        entry.target.classList.add('visible'); 
                        obs.unobserve(entry.target); 
                    } 
                }); 
            }, { threshold: 0.2 }); 
 
            elements.forEach(el => observer.observe(el)); 
        }); 
 
        // Play audio and subtle springy bounce on first click 
        document.addEventListener("DOMContentLoaded", () => { 
            const audio = document.getElementById("introAudio"); 
            const box = document.getElementById("clickableBox"); 
 
            box.addEventListener("click", () => { 
                // Play audio 
                audio.play().catch(err => console.log("Audio blocked:", err)); 
 
                // Subtle springy bounce 
                box.animate([ 
                    { transform: 'scale(1)' }, 
                    { transform: 'scale(0.97)' }, 
                    { transform: 'scale(1)' } 
                ], { 
                    duration: 300, 
                    easing: 'ease-out', 
                    fill: 'forwards' 
                }); 
            }, { once: true }); // Only plays once 
        }); 
    </script> 
</asp:Content>