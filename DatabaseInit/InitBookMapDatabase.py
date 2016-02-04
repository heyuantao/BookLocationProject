# -*- coding: utf-8 -*-
import pymssql
import xlrd
#from enum import Enum

class MapItem(object):
    def __init__(self,location="",objType="",position="",rfidOfShelf=""):
        self.location=location
        self.type=objType
        self.position=position
        self.rfidOfShelf=rfidOfShelf
    #def loadFromString(self,recorderString):        
    #    self.location=recorderString[0].strip()
    #    self.type=recorderString[1].strip()
     #   self.position=recorderString[2].strip()
    #    self.rfidOfShelf=recorderString[3].strip()
        #self=parseAndFormat.parseFun(recorderString)
        
    #def saveToString(self,parseAndFormat):
    #    tempString=parseAndFormat.formatFun(self,self)
    #    return tempString

class MapXLSReader(object):
    def __init__(self,xlsFilename):
        self.xlsFilename=xlsFilename
        
        self.xl_workbook = xlrd.open_workbook(self.xlsFilename)
        self.xl_sheet = self.xl_workbook.sheet_by_index(0) #从第一个表格开始
        self.tableNumber=len(self.xl_workbook.sheet_names()) #计算出有多少个表
        self.nrows=self.xl_sheet.nrows
        self.tableIndex=0;  #第一个表的索引为0
        self.beginRow=1  
        self.currentRow=1
        
    def readOneObject(self): 
        #if there are data ,and return a object .otherwise return None
        if self.tableIndex==self.tableNumber:
            return None       
        if self.currentRow==self.nrows:
            #每当一个表格的内容读取完成后，设置开始读取下一个表格
            self.tableIndex=self.tableIndex+1
            if self.tableIndex==self.tableNumber:
                return None
            self.xl_sheet = self.xl_workbook.sheet_by_index(self.tableIndex) 
            #print self.tableIndex           
            self.currentRow=1
            self.nrows=self.xl_sheet.nrows
        rowData=self.xl_sheet.row_values(self.currentRow)
        #print self.tableIndex,
        self.currentRow=self.currentRow+1
        oneItem=MapItem()
        oneItem.location=rowData[0].strip()
        oneItem.type=rowData[1].strip()
        oneItem.position=rowData[2].strip()
        oneItem.rfidOfShelf=rowData[3].strip()
        #print oneItem
        return oneItem
    def close(self):
        pass
        #self.xl_workbook.close()
     
class MapDatabase(object):
    def __init__(self,servername="DESKTOP-MI02F8C",username="sa",password="19831122",databasename="Location"):
        self.servername=servername
        self.username=username
        self.password=password
        self.databasename=databasename
        self.connection = pymssql.connect(server=servername, user=username, password=password, database=databasename,charset="utf8")
        self.cursor=self.connection.cursor()
        #self.parseAndFormat=ParseAndFormat()
        
    def close(self):
        self.connection.close()
        
    def clearAllTable(self): #delete all recode in two table
        cursor=self.connection.cursor()
        delcommand="TRUNCATE TABLE Map"
        cursor.execute(delcommand)
        self.connection.commit()
        
    def insertOneItemIntoTable(self,oneItem):
        insertCommand=u"insert into Map (location,type,position,rfidOfShelf) values ('%s','%s','%s','%s')" %(oneItem.location,oneItem.type,oneItem.position,oneItem.rfidOfShelf)
        self.cursor.execute(insertCommand)
        self.connection.commit()
        
class ReadOfMapExcelIntoDatabase(object):
    def __init__(self,databaseName,excelFileName):
        self.databbase=MapDatabase(databasename=databaseName)
        self.excelFile=MapXLSReader(excelFileName)
        self.databbase.clearAllTable()
    def read(self):
        print "Begin to read File !"
        while True:
            oneItem=self.excelFile.readOneObject()
            if oneItem is None:  # read finished
                print ""
                print "Read Excel File Finished !"
                break
            self.databbase.insertOneItemIntoTable(oneItem)
            print ".",
    def close(self):
        self.excelFile.close()
        self.databbase.close()
        
if __name__=="__main__":
    readOfMapExcelIntoDatabase=ReadOfMapExcelIntoDatabase("Location","XLSDIR/mapFileSample.xls")  
    readOfMapExcelIntoDatabase.read()  
    readOfMapExcelIntoDatabase.close()