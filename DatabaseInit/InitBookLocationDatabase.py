# -*- coding: utf-8 -*-
#初始化RFID层架标签数据，同时初始化地图信息数据
import pymssql
import xlrd
class LocationToDatabase(object):
    def __init__(self,servername="DESKTOP-PSQP38H",username="sa",password="19831122",databasename="Location"):
        self.servername=servername
        self.username=username
        self.password=password
        self.databasename=databasename
        #初始化数据库连接
        self.connection = pymssql.connect(server=servername, user=username, password=password, database=databasename,charset="utf8")
    def close(self):
        self.connection.close()
    def clearAllTable(self):
        cursor=self.connection.cursor()
        delcommand="TRUNCATE TABLE Shelf"       
        cursor.execute(delcommand)
        self.connection.commit()
    def readShelfRfidFileToDatabae(self,filename):   
        book=xlrd.open_workbook(filename)
        sheetNameList=book.sheet_names()
        sheetNameList=sheetNameList[1:len(sheetNameList)]
        for sheetName in sheetNameList:
            table=book.sheet_by_name(sheetName)
            ###一次读取一张表格的内容
            beginRow=3
            nrows=table.nrows
            cursor=self.connection.cursor()
            for currentRow in range(beginRow,nrows):
                rowData=table.row_values(currentRow)
                #print rowData
                floor=int(rowData[1])
                selection=rowData[3].strip()
                row=int(rowData[6])
                side=rowData[8].strip()
                col=int(rowData[10])
                shelfFloor=int(rowData[12])
                code=rowData[14].strip()
                #desc=rowData[15].strip()
                insertCommand=u"insert into Shelf (floor,selection,row,col,side,shelfFloor,rfidCode) values (%s,'%s',%s,%s,'%s',%s,'%s')" %(floor,selection,row,col,side,shelfFloor,code)
                insertCommand=insertCommand.encode("utf8")
                print ".",
                cursor.execute(insertCommand)
            self.connection.commit()
            print ""
if __name__=="__main__":
    locationToDatabase=LocationToDatabase()
    locationToDatabase.clearAllTable()
    print "begin insert data into shelf"
    locationToDatabase.readShelfRfidFileToDatabae("XLSDIR/shelfRFID.xls")
    print "insert data into shelf end"
    locationToDatabase.close()
