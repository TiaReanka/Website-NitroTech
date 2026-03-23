**NitroTech Workshop Manager 💼**
Streamlined web-based workshop management system for handling business operations efficiently.

NitroTech Workshop Manager is a full-featured web application developed as a university project to simplify and centralize workshop operations. The system focuses on role-based access control, ensuring that each user interacts with the platform according to their responsibilities, while maintaining security and operational efficiency.

---

## 📌 Features

* 🔐 **Role-Based Access Control** – Four distinct roles (Director, Manager, Administrator, Clerk) with clearly defined permissions.
* 📊 **Comprehensive Management System** – Create, view, and manage quotations, invoices, statements, and payments.
* 👥 **User Management (Director Only)** – Add, archive, and assign roles to users.
* 🧾 **Financial Tracking** – Maintain accurate records of transactions and statements.
* 📦 **Inventory Reporting** – Monitor and update inventory (restricted to higher-level roles).
* 🧑‍💼 **Customer Management** – Update and maintain customer information across all relevant roles.
* ⚙️ **Pre-Populated Login System** – Ready-to-use accounts for testing different access levels.

---

## 🛠 Built With:

* ASP.NET Web Application (Visual Studio Solution)
* C#
* SQL Database (via TableAdapters)
* Visual Studio 2022

Custom system design includes:

* Role-based authentication and authorization
* Modular structure for handling invoices, quotations, payments, and reports
* Secure login system with security questions

---

## 👤 User Roles

* **Director**
  Full system access, including user management, inventory, financial records, and reporting.

* **Manager**
  Full operational access except user role management.

* **Administrator**
  Can manage financial documents and customer details but cannot modify inventory or users.

* **Clerk**
  Limited to quotations, invoices, and customer updates.

---

## 🔑 Sample Login Credentials

* **Director**
  Username: JadenNaidoo
  Password: #carloVer87.

* **Manager**
  Username: DesanNaidoo
  Password: NaiMDes23!

* **Administrator**
  Username: MarguGovender
  Password: mAr@jaE.gov1

* **Clerk**
  Username: CarlSainz
  Password: L$FerrAr9W

---

## 📁 Folder Structure

```
NitroTechWebsite/  
│  
├── NitroTechWebsite.sln        # Visual Studio solution file  
├── Controllers/               # Handles application logic  
├── Models/                    # Data models and database interaction  
├── Views/                     # Frontend UI pages  
├── Scripts/                   # Client-side scripts  
├── Content/                   # Styling and static assets  
├── App_Data/                  # Database files  
├── README.md  
```

---

## 🚀 How to Run

### 🧰 Requirements

* Visual Studio 2022
* .NET Framework / ASP.NET support
* Access to UKZN network (via Global Protect VPN)

### 🔧 Build Instructions (Visual Studio)

1. Connect to Global Protect (UKZN network).
2. Open `NitroTechWebsite.sln` in Visual Studio.
3. Select **Debug** or **Release** mode.
4. Click the **Start** button to run the application.

---

## 🎯 System Capabilities

* Secure multi-user access
* Efficient business workflow automation
* Centralized data handling for workshop operations
* Scalable structure for future enhancements

---

## 🧠 Developer Insight

This project demonstrates:

* Implementation of **role-based system design**
* Strong **Object-Oriented Programming (OOP)** in C#
* Database integration using **TableAdapters**
* Designing secure authentication systems
* Building scalable, maintainable web applications
* Team collaboration in a full-stack development environment

---

## 📫 Contact

Created by Tia Reanka Naidoo, Calvin Pillay, Prineshan Govender, Tia Dindayal,and Deanna Gounden as part of a university IT project.
Email: [tiarnaidoo@gmail.com]
