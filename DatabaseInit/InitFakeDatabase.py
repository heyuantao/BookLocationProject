# -*- coding: utf-8 -*-
# 这个文件的作用是将excel文件的内容都入到数据库中
import pymssql
import xlrd

class FakeBookInformationItem(object):
    bookName=""
    bookAccessCode=""
    bookRfidCode=""
    def __init__(self):
        pass

class XLSReaderOfBookInformation(object):
    def __init__(self,filename):
        self.filename=filename
    def readFileToList(self):        # return the list of FakeBokInformationItem
        xl_workbook = xlrd.open_workbook(self.filename)
        xl_sheet = xl_workbook.sheet_by_index(0)
        self.nrows=xl_sheet.nrows
        self.ncols=xl_sheet.ncols
        self.beginrow=1        
        ##begin to read file
        returnList=[]
        
        for i in range(1,self.ncols+1):
            bookname=xl_sheet.cell(i,0).value
            accesscode=xl_sheet.cell(i,1).value
            rfidcode=xl_sheet.cell(i,2).value
            #print bookname,accesscode,rfidcode 
            bookInformationItem=FakeBookInformationItem()   
            bookInformationItem.bookName=bookname
            bookInformationItem.bookAccessCode=accesscode
            bookInformationItem.bookRfidCode=rfidcode    
            returnList.append(bookInformationItem)
            
        return returnList

class BookInformationDatabase(object):
    
    def __init__(self,servername="DESKTOP-PSQP38H",username="sa",password="19831122",databasename="Dblibrary"):
        self.servername=servername
        self.username=username
        self.password=password
        self.databasename=databasename
        self.connection = pymssql.connect(server=servername, user=username, password=password, database=databasename,charset="utf8")

    def clearAllTable(self): #delete all recode in two table
        cursor=self.connection.cursor()
        delcommand="TRUNCATE TABLE tmxxb"
        cursor.execute(delcommand)
        delcommand="TRUNCATE TABLE wxxxb"
        cursor.execute(delcommand)
        self.connection.commit()
    def insertListDataIntoTable(self,itemlist):
        cursor=self.connection.cursor()
        #cursor.execute("insert into tmxxb (索取号,条形码) values ('TP123','0x213213')")
        for item in itemlist:
            insertcommand=u"insert into tmxxb (索取号,条形码) values (\'"+item.bookAccessCode+"\',\'"+item.bookRfidCode+"\')"
            insertcommand=insertcommand.encode("utf8")
            print insertcommand
            cursor.execute(insertcommand)
        self.connection.commit()
            

if __name__=="__main__":
    #this code is use for test database function,common it
    #testDatabaseOperation()
    xlsfile=XLSReaderOfBookInformation("XLSDIR/bookInformation.xlsx")
    fakeBookInformationList=xlsfile.readFileToList()
    database=BookInformationDatabase()
    database.clearAllTable()
    print "begin insert data into database !"
    database.insertListDataIntoTable(fakeBookInformationList)
    print "insert data end !"
