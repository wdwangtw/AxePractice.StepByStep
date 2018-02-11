use master
go

if exists (select * from sys.databases where name='TestDB')
drop database [TestDB]
go

create database [TestDB]
go

use [TestDB]
go

create table employees (
    id bigint identity(1,1) primary key,
    name varchar(255)
)
go


create table salary (
    id bigint identity(1,1) primary key,
	fee bigint
);

create table location (
	employee_id bigint,
    country varchar(64),
    constraint fk_location_employee_id foreign key (employee_id) references employees (id),
);


INSERT INTO employees VALUES
    ('kingwonder')
go

INSERT INTO salary VALUES
    (100)
go

INSERT INTO location VALUES
    (1, 'China')
go



