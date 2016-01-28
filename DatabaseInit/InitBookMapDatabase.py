# -*- coding: utf-8 -*-
import pymssql
import xlrd
from enum import Enum
     
class Map(object):
    def __init__(self):
        self.location=""
        self.position=""
        self.type=""
        
    def loadFromString(self,parseAndFormat,recorderString):
        self=parseAndFormat.parseFun(recorderString)
        
    #def saveToString(self,parseAndFormat):
    #    tempString=parseAndFormat.formatFun(self,self)
    #    return tempString
    
class ParseAndFormat(object): 
    def parseFun(self,recorderString):
        mapItem=Map()
        mapItem.location=recorderString[0].strip()
        mapItem.position=recorderString[1].strip()
        mapItem.type=recorderString[2].strip()
        return mapItem
    #def formatFun(self,object):
    #    pass ##待处理
     
class BookMapDatabase(object):
    def __init__(self,servername="DESKTOP-PSQP38H",username="sa",password="19831122",databasename="Location"):
        self.servername=servername
        self.username=username
        self.password=password
        self.databasename=databasename
        self.connection = pymssql.connect(server=servername, user=username, password=password, database=databasename,charset="utf8")
        self.cursor=self.connection.cursor()
        self.parseAndFormat=ParseAndFormat()
        
    def close(self):
        self.connection.close()
        
    def clearAllTable(self): #delete all recode in two table
        cursor=self.connection.cursor()
        delcommand="TRUNCATE TABLE Map"
        cursor.execute(delcommand)
        self.connection.commit()
        
    def insertOneItemIntoTable(self,oneItem):
        insertCommand=u"insert into Map (location,position,type) values ('%s','%s','%s')" %(oneItem.location,oneItem.position,oneItem.type)
        self.cursor.execute(insertCommand)
        self.connection.commit()
    
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
            
if __name__=="__main__":
    bookMapDatabase=BookMapDatabase()
    bookMapDatabase.clearAllTable()
    bookMapDatabase.insertListDataIntoTable("filename")
    bookMapDatabase.close()        