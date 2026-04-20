# 🛡️ E-Insurance Application

## 📌 Project Description
The **E-Insurance Application** is a role-based web platform designed to digitize and streamline insurance operations. It supports multiple user roles—**Customer, Admin, Insurance Agent, and Employee**—each with controlled access to system features.

The application uses **JWT (JSON Web Token)** for secure authentication and implements **Role-Based Access Control (RBAC)** to ensure proper authorization. It enables users to manage policies, track payments, and perform administrative operations efficiently.

### 🎯 Core Objectives
- Provide secure and scalable insurance management  
- Simplify policy purchase and tracking  
- Enable centralized administrative control  
- Improve performance and usability  

---

## 🛠️ Tech Stack
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

## 🚀 Key Features
- 🔐 Secure JWT-based Authentication & Authorization  
- 👤 Role-Based Access Control (RBAC)  
- 📄 Policy Management (View, Purchase, Track)  
- 💳 Payment Tracking  
- ⚙️ Admin Dashboard for User Management  
- 📊 Premium & Commission Calculations  

---

# 📚 Use Cases

---

## 🔐 UC-01: User Authentication

### **Actors**
Customer, Admin, Insurance Agent, Employee  

### **Description**
Users log in using credentials and receive a JWT token for secure, role-based access.

### **Preconditions**
- User must be registered.

### **Postconditions**
- User is authenticated and authorized.

### **Steps**
1. Navigate to login page  
2. Enter credentials  
3. System validates credentials  
4. JWT token is generated  
5. Access granted based on role  

---

## 📄 UC-02: View Policies

### **Actors**
Customer, Admin  

### **Description**
Customers can view their own policies and payment details, while Admins can view policies of specific customers.

### **Preconditions**
- User must be authenticated.

### **Postconditions**
- Policy and payment details are displayed.

### **Steps (Customer)**
1. Login to the system  
2. Navigate to **My Policies**  
3. View policies and payment details  

### **Steps (Admin)**
1. Login to the system  
2. Navigate to **Customer Policies**  
3. Search for a customer  
4. View policies and payment details  

---

## ⚙️ UC-03: Manage Users

### **Actors**
Admin  

### **Description**
Admin can perform CRUD operations on user data.

### **Preconditions**
- Admin must be authenticated.

### **Postconditions**
- User data is created, updated, or deleted.

### **Steps**
1. Login as Admin  
2. Navigate to **Manage Users**  
3. Perform operations:
   - Create  
   - Read  
   - Update  
   - Delete  
4. System updates database  
5. Confirmation displayed  

---

## 🛒 UC-04: Policy Purchase

### **Actors**
Customer  

### **Description**
Customers can browse and purchase insurance policies.

### **Preconditions**
- Customer must be authenticated.

### **Postconditions**
- Policy is added to customer account.

### **Steps**
1. Login as Customer  
2. Navigate to **Available Policies**  
3. Select a policy  
4. Enter required details  
5. Confirm purchase  
6. Policy is added to account  

---

## 💰 UC-05: Premium Calculation

### **Actors**
Customer, Insurance Agent  

### **Description**
Users can calculate premium based on factors like age and policy type.

### **Preconditions**
- User must be authenticated.

### **Postconditions**
- Premium amount is displayed.

### **Steps**
1. Login to system  
2. Navigate to **Premium Calculator**  
3. Enter details (age, policy type, etc.)  
4. System calculates premium  
5. Display result  

---

## 📊 UC-06: Commission Calculation

### **Actors**
Admin  

### **Description**
Admin calculates commission for insurance agents based on policies sold.

### **Preconditions**
- Admin must be authenticated.

### **Postconditions**
- Commission details are displayed.

### **Steps**
1. Login as Admin  
2. Navigate to **Commission Calculator**  
3. Select an agent  
4. System retrieves policies  
5. Calculate commission  
6. Display results  

---

## 👤 Author
**Prashant Varshney**  
B.Tech CSE (Data Analytics)  
