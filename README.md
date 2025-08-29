# OnlyMembers

OnlyMembers is a simple RESTful API built with .NET 8 and MySQL to manage members, their reward points, and coupon redemptions. 
It uses JWT token-based authentication and serves a minimal frontend for registration, login, and dashboard.

---

## Features

- Member registration and OTP verification (dummy OTP)
- JWT-based authentication (login with member name)
- Add reward points based on purchases (₹100 purchase = 10 points)
- View total points for a member
- Redeem points for coupon codes (100 points = ₹10 coupon)
- Simple frontend for registration, login, dashboard, and coupon management

---


### Setup
0. **Configure database connection**
put your mysql user and password
then create database
CREATE DATABASE onlymembersdb;

1. **Clone the repository**
git clone https://github.com/anveetpal01/HoshoOnlyMembers
cd HoshoOnlyMembers

3. **Run database migrations**
   dotnet tool install --global dotnet-ef
   dotnet ef database update
   
5. **Run the application**
  dotnet run

### Using the Frontend

- navigate to `https://localhost:5002/index.html`
- Register a new member, verify OTP, then login.
- Access the dashboard to add purchases, view points, and redeem coupons.


<img width="1024" height="1536" alt="dataFlow" src="https://github.com/user-attachments/assets/8164079d-07b2-41ef-97b9-7b3b1c82ecdd" />



