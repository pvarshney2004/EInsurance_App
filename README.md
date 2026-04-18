# 🛡️ E-Insurance Application

## 📌 Project Description
The **E-Insurance Application** is a role-based web platform designed to digitize and streamline insurance operations. It allows different types of users—**Customers, Admins, Insurance Agents, and Employees**—to interact with the system based on their roles.

The application provides secure authentication using **JWT (JSON Web Token)** and enables users to manage policies, payments, and administrative data efficiently. Customers can purchase and view policies, while admins have full control over managing users, and customer-related data.

The system also focuses on:
- Secure access with role-based authorization (RBAC)
- Efficient policy and payment management
- Administrative control over users data
- Scalability and performance optimization

---

## 🛠 Tech Stack
- **Language:** C#  
- **Framework:** ASP.NET Core MVC  
- **Frontend:** Razor Views (HTML, CSS, Bootstrap)  
- **Backend:** ASP.NET Core MVC (Controllers, Models)  
- **Database:** SQL Server  
- **ORM:** Entity Framework Core  
- **Authentication:** JWT (JSON Web Token)  
- **Tools:** Visual Studio  
- **Version Control:** Git & GitHub  

---

## 🎯 Key Features
- 🔐 JWT-based Authentication & Authorization  
- 📄 Policy Management (View & Track)  
- 👤 Role-Based Access Control  
- ⚙️ Admin Dashboard for User Management  
- 💳 Payment & Policy Tracking  

---

## 🔐 1. User Authentication

### **Actors**
- Customer
- Admin
- Insurance Agent
- Employee

### **Description**
Users can log in to the system using JWT authentication. Based on their role, they are granted appropriate access.

### **Preconditions**
- User must have a registered account.

### **Postconditions**
- User is authenticated.
- JWT token is generated.
- Role-based access is granted.

### **Steps**
1. User navigates to the login page.
2. User enters credentials.
3. System validates credentials.
4. JWT token is generated.
5. User is granted access based on role.

---

## 📄 2. View Policies

### **Actors**
- Customer
- Admin

### **Description**
Customers can view their own policies and payment details. Admins can view policies of specific customers.

### **Preconditions**
- User must be authenticated.

### **Postconditions**
- Policy and payment details are displayed.

### **Steps (Customer)**
1. Customer logs in.
2. Navigates to the **My Policies** section.
3. System retrieves and displays policies and payment details.

### **Steps (Admin)**
1. Admin logs in.
2. Navigates to the **Customer Policies** section.
3. Searches for a customer.
4. System retrieves and displays policies and payment details.

---

## ⚙️ 3. Manage Users

### **Actors**
- Admin

### **Description**
Admins can perform CRUD (Create, Read, Update, Delete) operations on users.

### **Preconditions**
- Admin must be authenticated.

### **Postconditions**
- User data is successfully updated.

### **Steps**
1. Admin logs in.
2. Navigates to the **Manage Users** section.
3. Performs CRUD operations:
   - Create
   - Read
   - Update
   - Delete
4. System updates the database.
5. Confirmation is displayed.

---

## 🛒 4. Policy Purchase

### **Actors**
- Customer

### **Description**
Customers can browse available insurance policies and purchase one or multiple policies by providing the required details.

### **Preconditions**
- Customer must be authenticated.

### **Postconditions**
- Selected policy is successfully added to the customer’s account.

### **Steps**
1. Customer logs in.
2. Navigates to the **Available Policies** section.
3. Selects a policy.
4. Enters required details (e.g., personal info, policy preferences).
5. System processes the purchase.
6. Policy is added to the customer’s account.
7. Confirmation message is displayed.

---

## 👤 Author
**Prashant Varshney**  
B.Tech CSE (Data Analytics) 
