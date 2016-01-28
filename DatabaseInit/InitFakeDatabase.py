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
    def readFileToList(self):
        xl_workbook = xlrd.open_workbook(self.filename)
        xl_sheet = xl_workbook.sheet_by_index(0)
        self.nrows=xl_sheet.nrows
        self.ncols=xl_sheet.ncols
        self.beginrow=1        
        ##begin to read file
        for i in range(1,3):
            bookname=xl_sheet.cell(i,0).value
            accesscode=xl_sheet.cell(i,1).value
            rfidcode=xl_sheet.cell(i,2).value
            print bookname,accesscode,rfidcode        


def testDatabaseInsert(databaseConnection):
    cursor=databaseConnection.cursor()
    cursor.execute("insert into TestTable (testKey,testValue) values ('this','go')")
    databaseConnection.commit()
    print "Insert Success !"

def testDatabaseSelect(databaseConnection):
    cursor=databaseConnection.cursor()
    cursor.execute("select * from TestTable")
    print "Begin display"
    for row in cursor:
        print row
    print "End display"

def testDatabaseOperation():
    servername="DESKTOP-PSQP38H"
    username="sa"
    password="19831122"
    databasename="TestDatabase"
    #database connection
    print "Connect Databaes !"
    connection = pymssql.connect(server=servername, user=username, password=password, database=databasename)
    ###insert data
    testDatabaseInsert(connection)
    ###display data
    testDatabaseSelect(connection)
    ##close database
    connection.close()
    print "Close Database !"

def createFakeDataForTest():
    #FakeBookInformationList=
    #insert the fake data,real data is to large
    pass
if __name__=="__main__":
    #this code is use for test database function,common it
    #testDatabaseOperation()
    xlsfile=XLSReaderOfBookInformation("XLSDIR/bookInformation.xlsx")
    xlsfile.readFileToList()
    pass
