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
    #def readFileToList():
    #    xl_workbook = xlrd.open_workbook(self.filename)
    #    xl_sheet = xl_workbook.sheet_by_index(0)


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
    testDatabaseOperation()
    pass
