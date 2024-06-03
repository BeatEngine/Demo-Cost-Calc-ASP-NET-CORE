USE demo1;

CREATE TABLE "Calculation" ("Id" bigint identity(1,1), "Name" varchar(100),
"Description" text,"Creation" datetime DEFAULT GETDATE(), CONSTRAINT "PK_Calculation_Id" PRIMARY KEY("Id"));

CREATE TABLE "Costrecord" ("Id" bigint identity(1,1), "CalcId" bigint,
"Name" varchar(100), "Period" INT,"Value" real, CONSTRAINT "PK_Costrecord_Id" PRIMARY KEY("Id"),
CONSTRAINT "FK_CalcId" FOREIGN KEY("CalcId") REFERENCES "Calculation"("Id") ON DELETE CASCADE);

CREATE LOGIN adm   
    WITH PASSWORD = '123456';

CREATE USER adm FOR LOGIN adm;

ALTER ROLE db_owner ADD MEMBER adm;


