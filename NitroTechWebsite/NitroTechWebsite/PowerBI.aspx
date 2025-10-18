<%@ Page Title="Analysis Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PowerBI.aspx.cs" Inherits="NitroTechWebsite.PowerBI" %>
<%@ Register Assembly="System.Web.DataVisualization"
    Namespace="System.Web.UI.DataVisualization.Charting"
    TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Heading + Month Selector -->
    <div style="display: flex; align-items: center; justify-content: space-between; margin-bottom: 20px; margin-top: 20px;">
        <!-- Page Heading with Right-Fading Underline -->
        <h2 class="dashboard-heading" style="margin:0; padding-bottom:5px; position:relative;">
            Analysis Dashboard
            <span style="position:absolute; left:0; bottom:0; width:100%; height:2px; 
                         background: linear-gradient(to right, white, rgba(255,255,255,0));"></span>
        </h2>

        <!-- Month Selector with Caption -->
        <div style="display: flex; align-items: center; gap: 10px;">
            <span style="color:white; font-size:14px;">According to month</span>
            <asp:DropDownList ID="ddlMonth" runat="server" CssClass="month-dropdown" style="padding:5px 10px; font-size:14px; border-radius:5px;">
                <asp:ListItem Text="January" Value="1"></asp:ListItem>
                <asp:ListItem Text="February" Value="2"></asp:ListItem>
                <asp:ListItem Text="March" Value="3"></asp:ListItem>
                <asp:ListItem Text="April" Value="4"></asp:ListItem>
                <asp:ListItem Text="May" Value="5"></asp:ListItem>
                <asp:ListItem Text="June" Value="6"></asp:ListItem>
                <asp:ListItem Text="July" Value="7"></asp:ListItem>
                <asp:ListItem Text="August" Value="8"></asp:ListItem>
                <asp:ListItem Text="September" Value="9"></asp:ListItem>
                <asp:ListItem Text="October" Value="10"></asp:ListItem>
                <asp:ListItem Text="November" Value="11"></asp:ListItem>
                <asp:ListItem Text="December" Value="12"></asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>

    <style>
        /* Heading underline */
        .dashboard-heading {
            display: inline-block;
            position: relative;
            padding-bottom: 5px;
        }

        .dashboard-heading::after {
            content: "";
            position: absolute;
            left: 0;
            bottom: 0;
            width: 100%;
            height: 2px;
            background: linear-gradient(to right, white, rgba(255,255,255,0));
        }

        /* Container for cards */
        .cards-container {
            display: flex;
            justify-content: space-between;
            margin-top: 20px;
            margin-bottom: 40px;
        }

        /* Individual cards with horizontal layout for icon + content */
        .dashboard-card {
            background-color: rgba(255, 255, 255, 0.1);
            color: white;
            padding: 20px;
            flex: 1;
            margin: 0 10px;
            border-radius: 8px;
            display: flex;
            align-items: center;
            transition: transform 0.2s ease, background-color 0.2s ease;
        }

        .dashboard-card:hover {
            transform: translateY(-5px);
            background-color: rgba(255, 255, 255, 0.2);
        }

        /* Icon on left */
        .dashboard-card img.icon {
            width: 40px;
            height: 40px;
            margin-right: 15px;
        }

        /* Text content inside card */
        .card-content {
            display: flex;
            flex-direction: column;
            justify-content: center;
        }

        .card-content .title {
            font-weight: bold;
            font-size: 16px;
        }

        .card-content .number {
            font-size: 24px;
            margin-top: 5px;
        }

        .jobs-values {
            display: flex;
            align-items: center;
            gap: 5px;
            font-size: 24px;
            font-weight: bold;
        }

        .vs-text {
            color: #fff;
            font-weight: normal;
        }

        .pending {
            color: #FFD700;
        }

        .completed {
            color: lawngreen;
        }

        .pending-text {
            color: #FFD700;
        }

        .completed-text {
            color: lawngreen;
        }

        .month-dropdown {
            padding: 5px 10px;
            border-radius: 5px;
            font-size: 14px;
            margin-right: 10px;
            color: black;
        }

        /* Large graph containers */
        .graph-container {
            display: flex;
            justify-content: space-between;
            margin-top: 30px;
            gap: 20px;
        }

        .graph-box {
            background-color: rgba(255,255,255,0.1);
            flex: 1;
            height: 300px; /* large enough for graphs */
            border-radius: 10px;
            display: flex;
            justify-content: center;
            align-items: center;
            color: white;
            font-weight: bold;
            font-size: 16px;
        }
    </style>

    <!-- Cards Section -->
    <div class="cards-container">

        <!-- Total Customers -->
        <div class="dashboard-card">
            <img class="icon" runat="server" src="~/Images/customer.png" alt="Total Customers" />
            <div class="card-content">
                <span class="title">Total Customers</span>
                <asp:Label ID="lblTotalCustomers" runat="server" Text="0" CssClass="number"></asp:Label>
            </div>
        </div>

        <!-- Total Payments -->
        <div class="dashboard-card">
            <img class="icon" runat="server" src="~/Images/payment-method.png" alt="Total Payments" />
            <div class="card-content">
                <span class="title total-payments-title">Total Payments</span>
                <asp:Label ID="lblTotalPayments" runat="server" Text="R0" CssClass="number"></asp:Label>
            </div>
        </div>

        <!-- Pending vs Completed Jobs -->
        <div class="dashboard-card">
            <img class="icon" runat="server" src="~/Images/job-offer.png" alt="Jobs" />
            <div class="card-content">
                <span class="title">
                    <span class="completed-text">Completed Jobs</span> vs <span class="pending-text">Pending Jobs</span>
                </span>
                <div class="jobs-values">
                    <asp:Label ID="lblCompletedJobs" runat="server" Text="0" CssClass="number completed"></asp:Label>
                    <span class="vs-text">vs</span>
                    <asp:Label ID="lblPendingJobs" runat="server" Text="0" CssClass="number pending"></asp:Label>
                </div>
            </div>
        </div>

        <!-- Popular Manufacturer -->
        <div class="dashboard-card">
            <img class="icon" runat="server" src="~/Images/car-repair.png" alt="Popular Manufacturer" />
            <div class="card-content">
                <span class="title">Popular Manufacturer</span>
                <asp:Label ID="lblPopularManufacturer" runat="server" Text="-" CssClass="number"></asp:Label>
            </div>
        </div>

    </div>

   <!-- Graph Section: 3 Boxes -->
<!-- Graph Section -->
    <div class="graph-container">
        <div class="graph-box">
            <canvas id="graph1Canvas" style="width:100%; height:100%;"></canvas>
        </div>
        <div class="graph-box">
            <canvas id="graph2Canvas" style="width:100%; height:100%;"></canvas>
        </div>
        <div class="graph-box">
            <canvas id="graph3Canvas" style="width:100%; height:100%;"></canvas>
        </div>
    </div>

    <!-- Chart.js Library -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script>

        // Graph 1


        new Chart(document.getElementById('graph1Canvas').getContext('2d'), {
            type: 'bar',
            data: {
                labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
                datasets: [{
                    label: 'Total Invoices (R10k)',
                    data: [12, 19, 8, 15, 22, 10, 14, 16, 9, 20, 18, 11],
                    backgroundColor: 'rgba(54,162,235,0.6)',
                    barPercentage: 0.5,
                    categoryPercentage: 0.6
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    title: {
                        display: true,
                        text: 'Total Invoices per Month',
                        color: 'white',
                        font: { size: 18, weight: 'bold' }
                    },
                    legend: {
                        labels: { color: 'white' }
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        title: { display: true, text: 'Invoices (R10k)', color: 'white' },
                        ticks: { color: 'white' }
                    },
                    x: {
                        title: { display: true, text: 'Months', color: 'white' },
                        ticks: { color: 'white', autoSkip: false, maxRotation: 45, minRotation: 45, font: { size: 10 } }
                    }
                }
            }
        });

        // Graph 2 (line chart)
        new Chart(document.getElementById('graph2Canvas').getContext('2d'), {
            type: 'line',
            data: {
                labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
                datasets: [{
                    label: 'Quotations',
                    data: [5, 10, 7, 12, 9, 8, 11, 14, 6, 13, 10, 15], // populate from server if needed
                    borderColor: 'rgba(255,99,132,0.8)',
                    backgroundColor: 'rgba(255,99,132,0.2)',
                    fill: true,
                    tension: 0.3, // makes the line smooth
                    pointRadius: 5
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        labels: {
                            color: 'white'
                        }
                    },
                    title: {
                        display: true,
                        text: 'No. of Quotations per Month',
                        color: 'white',
                        font: { size: 18, weight: 'bold' }
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        title: {
                            display: true,
                            text: 'No of Quotations',
                            color: 'white'
                        },
                        ticks: {
                            color: 'white'
                        }
                    },
                    x: {
                        title: {
                            display: true,
                            text: 'Months',
                            color: 'white'
                        },
                        ticks: {
                            color: 'white',
                            autoSkip: false,
                            maxRotation: 45,
                            minRotation: 45
                        }
                    }
                }
            }
        });

        // Graph 3: Mixed Chart (Bar + Line) with all months shown
        new Chart(document.getElementById('graph3Canvas').getContext('2d'), {
            data: {
                labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'], // All months
                datasets: [
                    {
                        type: 'bar',
                        label: 'Completed Jobs',
                        data: [30, 25, 40, 20, 35, 28, 30, 32, 25, 27, 29, 31],
                        backgroundColor: 'lawngreen',
                        barPercentage: 0.5, // Reduce bar width
                        categoryPercentage: 0.5
                    },

                    {
                        type: 'bar',
                        label: 'Pending Jobs',
                        data: [10, 15, 5, 12, 8, 10, 9, 11, 14, 10, 12, 8],
                        barPercentage: 0.5,
                        backgroundColor: '#FFD700',
                        categoryPercentage: 0.5
                    }
                ]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        labels: {
                            color: 'white'
                        }
                    },
                    title: {
                        display: true,
                        text: 'Jobs Status per Month',
                        color: 'white',
                        font: { size: 16, weight: 'bold' }
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        title: {
                            display: true,
                            text: 'Number of Jobs',
                            color: 'white'
                        },
                        ticks: {
                            color: 'white'
                        }
                    },
                    x: {
                        title: {
                            display: true,
                            text: 'Months',
                            color: 'white'
                        },
                        ticks: {
                            color: 'white',
                            autoSkip: false,
                            maxRotation: 45,
                            minRotation: 45
                        }
                    }
                }
            }
        });
    </script>

</asp:Content>
