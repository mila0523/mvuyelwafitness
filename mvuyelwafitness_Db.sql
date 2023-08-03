--use mvuyelwafitness_Db
--go

use db_a98f3b_mvuyelwafitnessdb
go

create schema sales;
go

create schema users;
go

create schema finance;
go

--Admin tables--
Create table users.Administrators
(
	USER_ID int identity(1,1) primary key not null,
	Name varchar(50),
	Surname varchar(50),
	Email varchar(50) not null,
	Password varchar(50) not null,
	Address varchar(200),
	Reset_key varchar(50) not null,
)

Create table users.Customer
(
	CUSTOMER_ID int identity(1,1) primary key not null,
	Name varchar(50),
	Surname varchar(50),
	Email varchar(50) not null,
	Password varchar(50) not null,
	Address varchar(200),
	Reset_key varchar(50) not null,
)
go

--Sales--
Create table sales.Products
(
	PROD_ID int identity(1,1) primary key not null,
	Name varchar(50),
	Price decimal,
	Description varchar(500),
	Other_Info varchar(500),
	Quantity_stock int,
)

Create Table sales.Orders
(
	ORDER_NUM int identity(1,1) primary key not null,
	CUSTOMER_ID int not null,
	foreign key (CUSTOMER_ID) references users.Customer(CUSTOMER_ID),
	Date datetime,
	Total decimal,
	Paymet_Status varchar(20)
)


Create table sales.Order_Item
(
	ITEM_ID int identity(1,1) primary key not null,
	PROD_ID int not null,
	ORDER_NUM int not null,
	foreign key (PROD_ID) references sales.Products(PROD_ID),
	Quantity int
)
go

--Finance--
Create table finance.Payments
(
	PAYMENT_ID int identity(1,1) primary key not null,
	CUSTOMER_ID int not null,
	foreign key (CUSTOMER_ID) references users.Customer(CUSTOMER_ID),
	Date datetime,
	Description varchar(100),
	Amount decimal,
)


