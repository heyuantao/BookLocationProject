# -*- coding: utf-8 -*-
import pymssql
import xlrd
#from enum import Enum

def MapItem(object):
    def __init__(self,location="",type="",position="",rfidOfShelf=""):
        self.location=location
        self.type=type
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
    def __init__(self,xlsFilename,convertObj):
        self.xlsFilename=xlsFilename
        
        self.xl_workbook = xlrd.open_workbook(self.xlsFilename)
        self.xl_sheet = self.xl_workbook.sheet_by_index(0) #默认所有的数据都存储在第一个表格中
        self.nrows=self.xl_sheet.nrows
        self.beginRow=1  
        self.currentRow=1
        
    def readOneObject(self): 
        #if there are data ,and return a object .otherwise return None
        if self.currentRow==self.nrows:
            return None       
        rowData=self.xl_sheet.row_values(self.currentRow)
        self.currentRow=self.currentRow+1
        oneItem=MapItem()
        oneItem.location=rowData[0].strip()
        oneItem.type=rowData[0].strip()
        oneItem.position=rowData[0].strip()
        oneItem.rfidOfShelf=rowData[0].strip()
        return oneItem
    def close(self):
        self.xl_workbook.close()
     
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
    '''
    def insertListDataIntoTable(self,xlsFilename):
        xl_workbook = xlrd.open_workbook(xlsFilename)
        xl_sheet = xl_workbook.sheet_by_index(0) #默认所有的数据都存储在第一个表格中
        nrows=xl_sheet.nrows
        beginRow=1  #第一行是表格名称
        ###开始读取表格数据
        for currentRow in range(beginRow,nrows):
            rowData=xl_sheet.row_values(currentRow)
            mapItem=Map().loadFromString(self.parseAndFormat, rowData)
            self.insertOneItemIntoTable(mapItem)
    '''
class ReadOfMapExcelIntoDatabase(object):
    def __init__(self,databaseName,excelFileName,convertClass):
        self.databbase=MapDatabase(databasename=databaseName)
        self.excelFile=MapXLSReader()
    def read(self):
        while True:
            oneItem=self.excelFile.readOneObject()
            if oneItem is None:  # read finished
                break
            self.databbase.insertOneItemIntoTable(oneItem)
    def close(self):
        self.excelFile.close()
        self.databbase.close()
        
if __name__=="__main__":
    readOfMapExcelIntoDatabase=ReadOfMapExcelIntoDatabase()  
    readOfMapExcelIntoDatabase.read()  