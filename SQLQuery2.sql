use db_a98f3b_mvuyelwafitnessdb
go

--Alter table sales.Order_Item
--Add PROD_ID VARCHAR(12)

delete from sales.Orders
DBCC CHECKIDENT ('sales.Orders', RESEED,0)

--Alter table sales.Orders
--add SESSION_ID int 


--Alter table sales.Orders
--add foreign key (DEL_STATUS_ID) references sales.Delivery_Status(DEL_STATUS_ID)


--Alter table sales.Products
--add image nvarchar(max)

--delete from users.Customer

--insert into sales.Payment_Statuses values('Paid')
--insert into sales.Payment_Statuses values('Outstanding')

--create table sales.Delivery_Status
--(
--	DEL_STATUS_ID int identity(1,1) primary key not null,
--	Name varchar(100),
--)

--insert into sales.Delivery_Status values('Delivered')
--insert into sales.Delivery_Status values('Packing')
--insert into sales.Delivery_Status values('Processing')
--insert into sales.Delivery_Status values('Awaiting Payment')

select * From sales.Orders
select * from sales.Order_Item
--select * from sales.Payment_Statuses
--select * from sales.Delivery_Status


