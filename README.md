# OnlyMembers

OnlyMembers is a simple RESTful API built with .NET 8 and MySQL to manage members, their reward points, and coupon redemptions.  
It uses JWT token-based authentication and serves a minimal frontend for registration, login, and dashboard.

---

## Features

- Member registration and OTP verification (dummy OTP)
- JWT-based authentication (login with member name & password)
- Add reward points based on purchases (₹100 purchase = 10 points)
- View total points for a member
- Redeem points for coupon codes (100 points = ₹10 coupon)
- Simple frontend for registration, login, dashboard, and coupon management

---


### Setup
0. **Configure database connection**  
put your mysql user and password in appsettings.json  
then create database  
CREATE DATABASE OnlyMemberDb;

1. **Clone the repository**
git clone http://github.com/anveetpal01/HoshoOnlyMembers
cd HoshoOnlyMembers  
dotnet restore

3. **Run database migrations**
   dotnet tool install --global dotnet-ef  
   dotnet ef database update
   
5. **Run the application**
  

### Using the Frontend

- navigate to `http://localhost:5002/index.html`
- Register a new member, verify OTP, then login.
- Access the dashboard to add purchases, view points, and redeem coupons.

### Data Flow Diagram
<img width="512" height="768" alt="dataFlow" src="https://github.com/user-attachments/assets/8164079d-07b2-41ef-97b9-7b3b1c82ecdd" />

### Database Schema (Tables + Sample Data)
CREATE TABLE Members (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Mobile VARCHAR(10) NOT NULL UNIQUE,
    Email VARCHAR(255) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    Otp VARCHAR(10),
    IsVerified BOOLEAN DEFAULT FALSE,
    Points INT DEFAULT 0
);

CREATE TABLE PointTransactions (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    MemberId INT NOT NULL,
    PurchaseAmount INT NOT NULL,
    PointsAdded INT NOT NULL,
    Date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (MemberId) REFERENCES Members(Id)
);

CREATE TABLE Coupons (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    MemberId INT NOT NULL,
    PointsRedeemed INT NOT NULL,
    ValueInRupees INT NOT NULL,
    CouponCode VARCHAR(50) NOT NULL UNIQUE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (MemberId) REFERENCES Members(Id)
);



### Postman Collection URL - 
URL- https://github.com/anveetpal01/HoshoOnlyMembers/blob/main/OnlyMemberCollection.postman_collection.json
