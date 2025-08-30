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
CREATE DATABASE onlymembersdb;

1. **Clone the repository**  
git clone https://github.com/anveetpal01/HoshoOnlyMembers.git  
cd HoshoOnlyMembers  
dotnet restore

3. **Run database migrations**  
   dotnet tool install --global dotnet-ef
   dotnet ef migrations add Initial create
   dotnet ef database update
   (add sample data)  
5. **Run the application**  
  dotnet run

### Using the Frontend

- navigate to `http://localhost:5002/index.html`
- Register a new member, verify OTP, then login.
- Access the dashboard to add purchases, view points, and redeem coupons.

### Data Flow Diagram
<img width="512" height="768" alt="dataFlow" src="https://github.com/user-attachments/assets/8164079d-07b2-41ef-97b9-7b3b1c82ecdd" />

### Database Schema (Tables + Sample Data)  
Database -  
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


Sample Data -  
INSERT INTO Members (Id, Name, Mobile, Email, PasswordHash, Otp, IsVerified, Points) VALUES
(5, 'user5', '2222222222', 'user5@example.com', 'AQAAAAIAAYagAAAAEHONaMYtey2UnMdqW+8dtcmplVanIG5NlJgNApPnzC3vIYqDJUYg6VZRCGlcSMOcTQ==', '1234', TRUE, 99550),
(6, 'user6', '4444444444', 'user6@example.com', 'AQAAAAIAAYagAAAAEM/2bTH1udnf9KBPqDiSiKJ8uJnhCiPn3aof35eA+sDQtrNVak0b62kcJhZFaM2LHw==', '1234', TRUE, 0),
(7, 'user7', '7777777777', 'user7@example.com', 'AQAAAAIAAYagAAAAEMjjGu0Esy1lIRnutGUqFXyJe5KrTAiKm/mRaBc15vtpHRDV3sVgYyijhewcPFcHlw==', '1234', TRUE, 0);  

INSERT INTO PointTransactions (Id, MemberId, PurchaseAmount, PointsAdded, Date) VALUES
(1, 5, 500, 50, '2025-08-29 16:19:50.481480'),
(2, 5, 1000, 100, '2025-08-29 16:22:03.359724'),
(3, 7, 1000, 100, '2025-08-29 17:41:57.444956'),
(4, 5, 1000000, 100000, '2025-08-29 17:50:55.578118');  

INSERT INTO Coupons (Id, MemberId, PointsRedeemed, ValueInRupees, CouponCode, CreatedAt) VALUES
(1, 5, 100, 10, 'CUP-E60F3BAD', '2025-08-29 16:25:01.565312'),
(2, 7, 100, 10, 'CUP-9E267EC0', '2025-08-29 17:42:11.696384'),
(3, 5, 500, 50, 'CUP-74BBDBE7', '2025-08-29 17:51:07.398616');  


(Note - password for user5 is 1234abcdqw)

### Postman Collection URL - 
URL- https://github.com/anveetpal01/HoshoOnlyMembers/blob/main/OnlyMemberCollection.postman_collection.json
