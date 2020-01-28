-- Creator:       MySQL Workbench 8.0.15/ExportSQLite Plugin 0.1.0
-- Author:        Mike Obermeyer
-- Caption:       Initial DB
-- Project:       EE Lab Management
-- Changed:       2019-04-16 14:26
-- Created:       2018-03-28 09:46
-- Description:
--   Use this plugin to export file.
--   
--   https://github.com/tatsushid/mysql-wb-exportsqlite
--   
--   Then export file.
--   Tools>Catalog>ExportSqliteTableCoumns
PRAGMA foreign_keys = ON;

-- Schema: mydb
ATTACH "mydb.sdb" AS "mydb";
BEGIN;
CREATE TABLE "mydb"."CourseType"(
  "CourseTypeID" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
  "name" VARCHAR(45),
  "requiresLocker" INTEGER
);
CREATE TABLE "mydb"."Status"(
  "statusID" VARCHAR(45) PRIMARY KEY NOT NULL,
  "description" VARCHAR(45) NOT NULL
);
CREATE TABLE "mydb"."Vendor"(
  "vendorID" INTEGER PRIMARY KEY NOT NULL,
  "name" VARCHAR(45),
  "url" VARCHAR(45),
  "contactFirst" VARCHAR(45),
  "contactLast" VARCHAR(45),
  "email" VARCHAR(45),
  "phone" VARCHAR(45),
  "cell" VARCHAR(45)
);
CREATE TABLE "mydb"."ItemType"(
  "itemTypeID" INTEGER PRIMARY KEY NOT NULL,
  "name" VARCHAR(45),-- Resistor, Ocilloscope,
  "valueSuffix" VARCHAR(45)
);
CREATE TABLE "mydb"."EventType"(
  "eventTypeID" VARCHAR(45) PRIMARY KEY NOT NULL,
  "description" VARCHAR(45),
  CONSTRAINT "description_UNIQUE"
    UNIQUE("description"),
  CONSTRAINT "eventTypeID_UNIQUE"
    UNIQUE("eventTypeID")
);
CREATE TABLE "mydb"."UserType"(
  "userTypeID" INTEGER PRIMARY KEY NOT NULL,
  "typeName" VARCHAR(45)
);
CREATE TABLE "mydb"."User"(
  "userID" INTEGER PRIMARY KEY NOT NULL,
  "first" VARCHAR(45),
  "last" VARCHAR(45),
  "sid" INTEGER,
  "email" VARCHAR(80),
  "phone" VARCHAR(45),
  "cell" VARCHAR(45),
  "userTypeFK" INTEGER,
  CONSTRAINT "userTypeFK"
    FOREIGN KEY("userTypeFK")
    REFERENCES "UserType"("userTypeID")
);
CREATE INDEX "mydb"."User.userTypeFK_idx" ON "User" ("userTypeFK");
CREATE TABLE "mydb"."LockerType"(
  "lockerTypeID" INTEGER PRIMARY KEY NOT NULL,
  "name" VARCHAR(45),
  "length" INTEGER,
  "width" INTEGER,
  "height" INTEGER
);
CREATE TABLE "mydb"."Room"(
  "roomID" INTEGER PRIMARY KEY NOT NULL,
  "building" VARCHAR(6),
  "wing" VARCHAR(45),
  "roomNumber" VARCHAR(45),
  "subRoom" VARCHAR(45),-- A B or C...
  "name" VARCHAR(45)
);
CREATE TABLE "mydb"."PartLocation"(
  "partLocationID" INTEGER PRIMARY KEY NOT NULL,
  "text" VARCHAR(45)
);
CREATE TABLE "mydb"."SemesterName"(
  "semesterNameID" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
  "name" VARCHAR(45),
  "session" VARCHAR(45),
  "numberOfWeeks" INTEGER
);
CREATE TABLE "mydb"."Lock"(
  "lockID" INTEGER PRIMARY KEY NOT NULL CHECK("lockID">=0),
  "cw1" INTEGER,
  "ccw" INTEGER,
  "cw2" INTEGER
);
CREATE TABLE "mydb"."Course"(
  "courseID" INTEGER PRIMARY KEY NOT NULL,
  "subject" VARCHAR(4),
  "catalog" INTEGER,-- number
  "title" VARCHAR(80),
  "description" VARCHAR(400),
  "credit" INTEGER,
  "typeFK" INTEGER,
  "classCol" VARCHAR(45),
  CONSTRAINT "TypeFK"
    FOREIGN KEY("typeFK")
    REFERENCES "CourseType"("CourseTypeID")
);
CREATE INDEX "mydb"."Course.TypeFK_idx" ON "Course" ("typeFK");
CREATE TABLE "mydb"."Semester"(
  "semesterID" INTEGER PRIMARY KEY NOT NULL,
  "version" INTEGER,
  "nameFK" INTEGER,-- 1=Fall, 2=Winter, 3=Spring, 4=Summer
  "year" INTEGER,
  "scheduleDate" DATE,
  "schedulePostDate" DATE,
  CONSTRAINT "nameFK"
    FOREIGN KEY("nameFK")
    REFERENCES "SemesterName"("semesterNameID")
);
CREATE INDEX "mydb"."Semester.nameFK_idx" ON "Semester" ("nameFK");
CREATE TABLE "mydb"."Photo"(
  "photoID" INTEGER PRIMARY KEY NOT NULL,
  "image" BLOB
);
CREATE TABLE "mydb"."Calendar"(
  "calendarID" INTEGER PRIMARY KEY NOT NULL,
  "subject" VARCHAR(45),
  "semesterFK" INTEGER,
  "eventTypeFK" VARCHAR(45),
  "startDate" DATETIME,
  "endDate" DATETIME,
  CONSTRAINT "semesterFK"
    FOREIGN KEY("semesterFK")
    REFERENCES "Semester"("semesterID"),
  CONSTRAINT "eventTypeFK"
    FOREIGN KEY("eventTypeFK")
    REFERENCES "EventType"("eventTypeID")
);
CREATE INDEX "mydb"."Calendar.semesterFK_idx" ON "Calendar" ("semesterFK");
CREATE INDEX "mydb"."Calendar.eventTypeFK_idx" ON "Calendar" ("eventTypeFK");
CREATE TABLE "mydb"."InventoryLocation"(
  "inventoryLocationID" INTEGER PRIMARY KEY NOT NULL,
  "location" VARCHAR(45),-- Drawer, Station, Cabinet, Cupboard, Rolling Shelves
  "refinement1" VARCHAR(45),
  "refinement2" VARCHAR(45),
  "roomFK" INTEGER,
  CONSTRAINT "roomFK"
    FOREIGN KEY("roomFK")
    REFERENCES "Room"("roomID")
);
CREATE INDEX "mydb"."InventoryLocation.roomFK_idx" ON "InventoryLocation" ("roomFK");
CREATE TABLE "mydb"."Schedule"(
  "scheduleID" INTEGER PRIMARY KEY NOT NULL,
  "courseFK" INTEGER,
  "section" INTEGER,
  "semesterFK" INTEGER,
  "instructor1FK" INTEGER,
  "instructor2FK" INTEGER,
  "room1FK" INTEGER,
  "room2FK" INTEGER,
  "statusFK" INTEGER,
  "days" INTEGER,
  "startTime" DATETIME,
  "endTime" DATETIME,
  CONSTRAINT "semesterFK"
    FOREIGN KEY("semesterFK")
    REFERENCES "Semester"("semesterID"),
  CONSTRAINT "courseFK"
    FOREIGN KEY("courseFK")
    REFERENCES "Course"("courseID"),
  CONSTRAINT "room2FK"
    FOREIGN KEY("room2FK")
    REFERENCES "Room"("roomID"),
  CONSTRAINT "statusFK"
    FOREIGN KEY("statusFK")
    REFERENCES "Status"("statusID"),
  CONSTRAINT "instructor1FK"
    FOREIGN KEY("instructor1FK")
    REFERENCES "User"("userID"),
  CONSTRAINT "instructor2FK"
    FOREIGN KEY("instructor2FK")
    REFERENCES "User"("userID"),
  CONSTRAINT "room1FK"
    FOREIGN KEY("room1FK")
    REFERENCES "Room"("roomID")
);
CREATE INDEX "mydb"."Schedule.semesterFK_idx" ON "Schedule" ("semesterFK");
CREATE INDEX "mydb"."Schedule.classFK_idx" ON "Schedule" ("courseFK");
CREATE INDEX "mydb"."Schedule.roomFK_idx" ON "Schedule" ("room2FK");
CREATE INDEX "mydb"."Schedule.statusFK_idx" ON "Schedule" ("statusFK");
CREATE INDEX "mydb"."Schedule.instructor_idx" ON "Schedule" ("instructor1FK");
CREATE INDEX "mydb"."Schedule.instructor2FK_idx" ON "Schedule" ("instructor2FK");
CREATE INDEX "mydb"."Schedule.room1FK_idx" ON "Schedule" ("room1FK");
CREATE TABLE "mydb"."Item"(
  "itemID" INTEGER PRIMARY KEY NOT NULL,
  "description" VARCHAR(45),-- Depending on type may be disabled
  "value" VARCHAR(45),
  "barCode" INTEGER,
  "legacyTag" INTEGER,
  "serialNumber" VARCHAR(45),
  "wattage" INTEGER,
  "voltage" INTEGER,
  "tolerance" INTEGER,
  "itemTypeFK" INTEGER,
  "photoFK" INTEGER,
  CONSTRAINT "itemTypeFK"
    FOREIGN KEY("itemTypeFK")
    REFERENCES "ItemType"("itemTypeID")
    ON DELETE RESTRICT,
  CONSTRAINT "photoFK"
    FOREIGN KEY("photoFK")
    REFERENCES "Photo"("photoID")
    ON DELETE SET NULL
);
CREATE INDEX "mydb"."Item.itemTypeFK_idx" ON "Item" ("itemTypeFK");
CREATE INDEX "mydb"."Item.photoFK_idx" ON "Item" ("photoFK");
CREATE TABLE "mydb"."Locker"(
  "lockerID" INTEGER PRIMARY KEY NOT NULL CHECK("lockerID">=0),
  "number" INTEGER,
  "row" INTEGER,
  "col" INTEGER,
  "typeFK" INTEGER,
  "classFK" INTEGER,
  "section" INTEGER,
  "userFK" INTEGER,
  "lockFK" INTEGER,
  "roomFK" INTEGER,
  CONSTRAINT "lockFK"
    FOREIGN KEY("lockFK")
    REFERENCES "Lock"("lockID"),
  CONSTRAINT "classFK"
    FOREIGN KEY("classFK")
    REFERENCES "Course"("courseID"),
  CONSTRAINT "roomFK"
    FOREIGN KEY("roomFK")
    REFERENCES "Room"("roomID"),
  CONSTRAINT "userFK"
    FOREIGN KEY("userFK")
    REFERENCES "User"("userID"),
  CONSTRAINT "lockerTypeFK"
    FOREIGN KEY("typeFK")
    REFERENCES "LockerType"("lockerTypeID")
);
CREATE INDEX "mydb"."Locker.lockFK_idx" ON "Locker" ("lockFK");
CREATE INDEX "mydb"."Locker.classFK_idx" ON "Locker" ("classFK");
CREATE INDEX "mydb"."Locker.roomFK_idx" ON "Locker" ("roomFK");
CREATE INDEX "mydb"."Locker.userFK_idx" ON "Locker" ("userFK");
CREATE INDEX "mydb"."Locker.lockerTypeFK_idx" ON "Locker" ("typeFK");
CREATE TABLE "mydb"."Source"(
  "sourceID" INTEGER NOT NULL,
  "url" VARCHAR(80),
  "priceEach" INTEGER,
  "purchaseDate" DATE,
  "itemFK" INTEGER,
  "vendorFK" INTEGER,
  CONSTRAINT "vendorFK"
    FOREIGN KEY("vendorFK")
    REFERENCES "Vendor"("vendorID"),
  CONSTRAINT "itemFK"
    FOREIGN KEY("itemFK")
    REFERENCES "Item"("itemID")
    ON DELETE CASCADE
);
CREATE INDEX "mydb"."Source.vendorFK_idx" ON "Source" ("vendorFK");
CREATE INDEX "mydb"."Source.itemFK_idx" ON "Source" ("itemFK");
CREATE TABLE "mydb"."Inventory"(
  "inventoryID" INTEGER PRIMARY KEY NOT NULL,
  "quantity" INTEGER,
  "itemFK" INTEGER,
  "inventoryLocationFK" INTEGER,
  CONSTRAINT "itemFK"
    FOREIGN KEY("itemFK")
    REFERENCES "Item"("itemID"),
  CONSTRAINT "inventoryLocationFK"
    FOREIGN KEY("inventoryLocationFK")
    REFERENCES "InventoryLocation"("inventoryLocationID")
);
CREATE INDEX "mydb"."Inventory.itemFK_idx" ON "Inventory" ("itemFK");
CREATE INDEX "mydb"."Inventory.inventoryLocationFK_idx" ON "Inventory" ("inventoryLocationFK");
CREATE TABLE "mydb"."CoursePartsList"(
  "classPartsListID" INTEGER PRIMARY KEY NOT NULL,
  "classFK" INTEGER,
  "itemFK" INTEGER,
  "quantity" VARCHAR(45),
  "partLocationFK" INTEGER,
  CONSTRAINT "classFK"
    FOREIGN KEY("classFK")
    REFERENCES "Course"("courseID"),
  CONSTRAINT "itemFK"
    FOREIGN KEY("itemFK")
    REFERENCES "Item"("itemID"),
  CONSTRAINT "partLocationFK"
    FOREIGN KEY("partLocationFK")
    REFERENCES "PartLocation"("partLocationID")
);
CREATE INDEX "mydb"."CoursePartsList.classFK_idx" ON "CoursePartsList" ("classFK");
CREATE INDEX "mydb"."CoursePartsList.itemFK_idx" ON "CoursePartsList" ("itemFK");
CREATE INDEX "mydb"."CoursePartsList.partLocationFK_idx" ON "CoursePartsList" ("partLocationFK");
COMMIT;
